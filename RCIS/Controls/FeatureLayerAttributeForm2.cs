using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using System.Collections;
using stdole;

using RCIS.Utility;
using RCIS.GISCommon;
using ESRI.ArcGIS.esriSystem;

namespace RCIS.Controls
{
    public partial class FeatureLayerAttributeForm2 : Form
    {
        private ILayer m_pLayer;
        private IActiveView m_activeView;
        private bool m_shouldAction = true;

        public IMapControl3 mapCtrol3 = null;

        public IActiveView ActiveView
        {
            get
            {
                return this.m_activeView;
            }
            set
            {
                this.m_activeView = value;
            }
        }
        public ILayer CurrentLayer
        {
            get
            {
                return this.m_pLayer;
            }
            set
            {
                this.m_pLayer = value;
            }
        }
        public IGeoFeatureLayer FeatureLayer
        {
            set
            {
                this.m_pLayer = value;
            }
            get
            {
                return this.m_pLayer as IGeoFeatureLayer;
            }
        }
        public FeatureLayerAttributeForm2()
        {
            InitializeComponent();
        }

        #region 信息初始化
        public void InitGeneralInfo()
        {
            #region 提取基本信息
            if (this.CurrentLayer != null)
            {
                string layerName = this.CurrentLayer.Name;
                string modelName = layerName;
                if (this.FeatureLayer != null)
                {
                    modelName = LayerHelper.GetClassShortName(this.FeatureLayer.FeatureClass as IDataset);
                }
                if (!this.lbLayerName.Text.EndsWith(":" + layerName))
                {
                    this.lbLayerName.Text = "图层名称:" + layerName;
                    this.lbModelName.Text = "数据源名称:" + modelName;
                    ILayerEffects layerEff = this.CurrentLayer as ILayerEffects;
                    if (layerEff != null)
                    {
                        this.teAlpha.Text = layerEff.Transparency.ToString();
                        int s = 0;
                        int.TryParse(this.teAlpha.Text, out s);

                        this.trackBar1.Value = s;

                    }
                    double maxScale = this.CurrentLayer.MaximumScale;
                    double minScale = this.CurrentLayer.MinimumScale;
                    if (Double.IsNaN(maxScale)) maxScale = 0;
                    if (Double.IsNaN(minScale)) minScale = 0;
                    if (maxScale == 0.0 && minScale == 0.0)
                    {
                        this.cbUseScale.Checked = false;
                        this.gbScale.Enabled = false;
                    }
                    else
                    {
                        this.cbUseScale.Checked = true;
                        this.gbScale.Enabled = true;
                        this.teMaxScale.Text = maxScale.ToString();
                        this.teMinScale.Text = minScale.ToString();
                    }
                }
            }
            #endregion
        }

        public void InitDisplayField()
        {
            if (this.FeatureLayer != null)
            {
                if (this.cbDisplayField.Items.Count <= 0)
                {//只有首选字段列表中没有数据的时候才需要初始化
                    this.cbDisplayField.Items.Clear();
                    this.lvDisplayFieldList.Items.Clear();
                    IFeatureClass curClass = this.FeatureLayer.FeatureClass;
                    int fldCount = curClass.Fields.FieldCount;
                    int selIndex = -1;//原来的首选字段的索引
                    #region 提取原来的首选字段
                    string displayField = this.FeatureLayer.DisplayField;
                    if (displayField == null) displayField = "";
                    displayField = displayField.ToUpper();
                    #endregion
                    for (int ci = 0; ci < fldCount; ci++)
                    {
                        IField curField = curClass.Fields.get_Field(ci);
                        ComboBoxItem cbi = new ComboBoxItem(curField, curField.AliasName, ci + 1);
                        this.cbDisplayField.Items.Add(cbi);
                        ListViewItem lvi = new ListViewItem((ci + 1).ToString());
                        lvi.SubItems.Add(curField.Name);
                        lvi.SubItems.Add(curField.AliasName);
                        lvi.SubItems.Add(DatabaseHelper.QueryFieldTypeName(curField.Type));
                        lvi.SubItems.Add(DatabaseHelper.QueryFieldLength(curField).ToString());
                        lvi.SubItems.Add(DatabaseHelper.QueryFieldPrecision(curField).ToString());
                        lvi.Tag = curField;
                        this.lvDisplayFieldList.Items.Add(lvi);
                        if (ci % 2 == 0) lvi.BackColor = Color.Wheat;
                        if (curField.Name.ToUpper().Equals(displayField))
                        {
                            selIndex = ci;
                        }
                    }
                    if (this.cbDisplayField.Items.Count > 0 && selIndex >= 0)
                    {
                        this.cbDisplayField.SelectedIndex = selIndex;
                    }
                }
            }
        }
        public void InitLayerDisplayIndex()
        {
            if (this.lvLayerList.Items.Count <= 0)
            {
                IMap layerMap = this.ActiveView.FocusMap;
                this.ceLayerSelectable.CheckedChanged += new EventHandler(OnLayerSelectableChanged);
                int layerCount = layerMap.LayerCount;
                for (int li = 0; li < layerCount; li++)
                {
                    ILayer curLayer = layerMap.get_Layer(li);
                    ListViewItem lvi = new ListViewItem((li + 1).ToString());
                    lvi.SubItems.Add(curLayer.Name);
                    lvi.SubItems.Add(curLayer.Name);
                    lvi.SubItems.Add(LayerHelper.LayerTypeName(curLayer));
                    if (curLayer is IGeoFeatureLayer)
                    {
                        IGeoFeatureLayer geoLayer = curLayer as IGeoFeatureLayer;
                        string modelName = LayerHelper.GetClassShortName(geoLayer.FeatureClass as IDataset);
                        lvi.SubItems[2].Text = modelName;
                        if (geoLayer.Selectable)
                        {
                            lvi.SubItems.Add("可选");
                        }
                        else
                        {
                            lvi.SubItems.Add("不可选");
                        }
                    }
                    else
                    {
                        lvi.SubItems.Add("----");
                    }
                    lvi.Tag = curLayer;
                    this.lvLayerList.Items.Add(lvi);
                    if (li % 2 == 0) lvi.BackColor = Color.Wheat;
                }
            }
        }

        private ArrayList m_annoClassList = new ArrayList();
        public void InitAnnoClass()
        {
            if (this.m_annoClassList.Count <= 0)
            {
                this.m_annoClassList.Clear();
                if (this.FeatureLayer != null)
                {
                    this.cbDisplayAnno.Checked = this.FeatureLayer.DisplayAnnotation;
                    #region 提取注记字段
                    int fldCount = this.FeatureLayer.FeatureClass.Fields.FieldCount;
                    for (int fi = 0; fi < fldCount; fi++)
                    {
                        IField curFld = this.FeatureLayer.FeatureClass.Fields.get_Field(fi);
                        ComboBoxItem cbi = new ComboBoxItem(curFld, curFld.AliasName, fi + 1);
                        this.cbAnnoFieldList.Items.Add(cbi);
                    }
                    #endregion
                    #region 提取所有可用字体
                    FontFamily[] allFamily = FontFamily.Families;
                    int familyCount = allFamily.Length;
                    for (int fi = 0; fi < familyCount; fi++)
                    {
                        FontFamily ff = allFamily[fi];
                        ComboBoxItem cbi = new ComboBoxItem(ff, ff.Name, fi + 1);
                        this.cbFontName.Items.Add(cbi);
                    }
                    #endregion
                    #region 提取注记类
                    int classCount = this.FeatureLayer.AnnotationProperties.Count;
                    for (int ci = 0; ci < classCount; ci++)
                    {
                        AnnotationPropertyItem propItem = new AnnotationPropertyItem();
                        this.FeatureLayer.AnnotationProperties.QueryItem(ci, out propItem.Properties, out propItem.PlacedElementCollection, out propItem.UnplacedElementCollection);
                        propItem.Clone();
                        this.m_annoClassList.Add(propItem);
                    }
                    classCount = this.m_annoClassList.Count;
                    for (int ci = 0; ci < classCount; ci++)
                    {
                        AnnotationPropertyItem propItem = this.m_annoClassList[ci] as AnnotationPropertyItem;
                        ComboBoxItem cbi = new ComboBoxItem(propItem, propItem.Properties.Class, ci + 1);
                        this.cbAnnoClass.Items.Add(cbi);
                    }
                    if (this.cbAnnoClass.Items.Count > 0)
                        this.cbAnnoClass.SelectedIndex = 0;
                    #endregion

                }
            }
        }
        #endregion

        private void SetEnable(TabPage pTabPage, bool pEnable)
        {
            foreach (Control aCtrl in pTabPage.Controls)
            {
                aCtrl.Enabled = pEnable;
            }
        }
        private void FeatureLayerAttributeForm2_Load(object sender, EventArgs e)
        {
            if (this.FeatureLayer == null)
            {
                this.SetEnable(this.tabPageAnno, false);
                //this.SetEnable(this.tabPageData, false);
                this.SetEnable(this.tabPageDisplayField, false);
                //this.SetEnable(this.tabPageStyle, false);
            }
            this.InitGeneralInfo();
        }

        private void cbUseScale_CheckedChanged(object sender, EventArgs e)
        {
            this.gbScale.Enabled = this.cbUseScale.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.ApplyGeneralPage();
                this.ApplyAnnotate();

                this.ApplyDisplayField();
                this.ApplyLayerIndex();
                //this.ApplyRenderStyle();
                if (this.ActiveView != null)
                {
                    this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGraphics
                        , this.FeatureLayer, this.ActiveView.Extent.Envelope);
                }
            }
            catch
            {
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private bool m_generalApplied = false;
        private void ApplyGeneralPage()
        {
            if (!this.m_generalApplied)
            {
                try
                {
                    int alpha = Convert.ToInt32(this.teAlpha.Text);
                    double maxScale = 0.0, minScale = 0.0;
                    if (this.cbUseScale.Checked)
                    {
                        maxScale = Double.Parse(this.teMaxScale.Text, System.Globalization.NumberStyles.Any
                            , new System.Globalization.NumberFormatInfo());
                        minScale = Double.Parse(this.teMinScale.Text, System.Globalization.NumberStyles.Any
                            , new System.Globalization.NumberFormatInfo());
                    }
                    ILayerEffects layerEff = this.CurrentLayer as ILayerEffects;
                    if (layerEff.SupportsTransparency == true)
                    {
                        if (layerEff != null) layerEff.Transparency = (short)alpha;
                    }
                    int Brightness = Convert.ToInt32(this.teBrightness.Text.Trim());
                    int Contrast = Convert.ToInt32(this.teContrast.Text.Trim());
                    if (layerEff.SupportsBrightnessChange == true)
                    {
                        if (layerEff != null) layerEff.Brightness = (short)Brightness;
                    }
                    if (layerEff.SupportsContrastChange == true)
                    {
                        if (layerEff != null) layerEff.Contrast = (short)Contrast;
                    }
                    this.CurrentLayer.MaximumScale = maxScale;
                    this.CurrentLayer.MinimumScale = minScale;

                    this.m_generalApplied = true;
                }
                catch
                {

                    //RCIS.Helper.ErrorHelper.ShowErrorForm(ex, "在应用一般属性的时候出现异常");
                }
            }
        }


        #region 数据表格
        //private DataTable m_dataTable = new DataTable();

        //private void InitDataGrid()
        //{
        //    if (this.FeatureLayer != null)
        //    {
        //        if (this.m_dataTable.Columns.Count <= 0)
        //        {
        //            try
        //            {
        //                this.ShowGridData();
        //            }
        //            catch { }
        //        }
        //    }
        //}
        //private void ShowGridData()
        //{//显示数据
        //    if (this.FeatureLayer != null)
        //    {
        //        this.m_dataTable.Columns.Clear();
        //        this.m_dataTable.Rows.Clear();

        //        IFeatureClass curClass = this.FeatureLayer.FeatureClass;
        //        int fldCount = curClass.Fields.FieldCount;


        //        #region 构造字段

        //        for (int fi = 0; fi < fldCount; fi++)
        //        {
        //            IField curFld = curClass.Fields.get_Field(fi);
        //            string fldName = curFld.Name.ToUpper();
        //            DataColumn dc = new DataColumn(curFld.Name);

        //            if (fldName.Equals(curClass.ShapeFieldName.ToUpper()))
        //            {
        //                dc.Caption = "几何图形";
        //            }
        //            else
        //                if (curClass.LengthField != null && fldName.Equals(curClass.LengthField.Name.ToUpper()))
        //                {
        //                    dc.Caption = "计算长度";
        //                }
        //                else
        //                    if (curClass.AreaField != null && fldName.Equals(curClass.AreaField.Name.ToUpper()))
        //                    {
        //                        dc.Caption = "计算面积";
        //                    }
        //                    else
        //                    {
        //                        dc.Caption = curFld.AliasName;
        //                    }
        //            if (curFld.Type == esriFieldType.esriFieldTypeInteger
        //                || curFld.Type == esriFieldType.esriFieldTypeSmallInteger
        //                || curFld.Type == esriFieldType.esriFieldTypeOID)
        //            {
        //                dc.DataType = typeof(Int32);
        //            }
        //            else if (curFld.Type == esriFieldType.esriFieldTypeSingle
        //                || curFld.Type == esriFieldType.esriFieldTypeDouble)
        //            {
        //                dc.DataType = typeof(Double);
        //            }
        //            else dc.DataType = typeof(string);
        //            this.m_dataTable.Columns.Add(dc);
        //        }
        //        #endregion


        //        int objCount = curClass.FeatureCount(null);
        //        //int objOrder = 1;
        //        string dispFld = this.FeatureLayer.DisplayField;
        //        int dispFldIndex = curClass.Fields.FindField(dispFld);
        //        if (dispFldIndex < 0)
        //        {
        //            dispFld = curClass.Fields.get_Field(0).Name;
        //            dispFldIndex = 0;
        //        }
        //        #region 获取数据

        //        if (objCount > 0)
        //        {
        //            IFeatureCursor qCursor = curClass.Search(null, false);
        //            IFeature qFea = qCursor.NextFeature();

        //            try
        //            {
        //                while (qFea != null)
        //                {

        //                    object dispObj = qFea.get_Value(dispFldIndex);
        //                    if (dispObj == null) dispObj = "";
        //                    //this.TaskMonitor.PutTaskInfo("获取数据--" + dispFld + "=" + dispObj + "(" + objOrder + "/" + objCount + ")", (int)(objOrder * 100 / objCount));
        //                    DataRow curRow = this.m_dataTable.NewRow();
        //                    for (int fi = 0; fi < fldCount; fi++)
        //                    {
        //                        IField curFld = curClass.Fields.get_Field(fi);
        //                        if (curFld.Type == esriFieldType.esriFieldTypeGeometry)
        //                        {
        //                            IGeometry curGeom = qFea.Shape;
        //                            if (curGeom != null && !curGeom.IsEmpty)
        //                            {
        //                                curRow[fi] = "图形对象";
        //                            }
        //                            else
        //                            {
        //                                curRow[fi] = "";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            curRow[fi] = qFea.get_Value(fi);
        //                        }
        //                    }

        //                    this.m_dataTable.Rows.Add(curRow);

        //                    qFea = qCursor.NextFeature();
        //                }
        //                OtherHelper.ReleaseComObject(qCursor);
        //            }
        //            catch
        //            {
        //            }

        //        }
        //        #endregion
        //        this.gridControl.DataSource = this.m_dataTable;

        //    }
        //}
        //private void dataGridView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{

        //}
        #endregion


        private void ApplyDisplayField()
        {
            if (this.FeatureLayer != null)
            {
                ComboBoxItem cbi = this.cbDisplayField.SelectedItem as ComboBoxItem;
                if (cbi != null && cbi.ItemObject is IField)
                {
                    this.FeatureLayer.DisplayField = (cbi.ItemObject as IField).Name;
                }
            }
        }
        private void ApplyLayerIndex()
        {
            if (this.m_activeView != null)
            {
                IMap curMap = this.m_activeView.FocusMap;
                int itemCount = this.lvLayerList.Items.Count;
                for (int ii = 0; ii < itemCount; ii++)
                {
                    ListViewItem curItem = this.lvLayerList.Items[ii];
                    ILayer curLayer = curItem.Tag as ILayer;
                    int layerIndex = LayerHelper.IndexOfLayer(curMap, curLayer);
                    if (layerIndex >= 0 && layerIndex != ii)
                    {
                        curMap.MoveLayer(curLayer, ii);
                    }
                    if (curLayer is IGeoFeatureLayer)
                    {
                        IGeoFeatureLayer geoLayer = curLayer as IGeoFeatureLayer;
                        if (curItem.SubItems[4].Text.Equals("可选"))
                        {
                            geoLayer.Selectable = true;
                        }
                        else geoLayer.Selectable = false;
                    }
                }
            }
        }
        private void ApplyAnnotate()
        {
            if (this.FeatureLayer != null)
            {
                this.FeatureLayer.AnnotationProperties.Clear();
                foreach (AnnotationPropertyItem api in this.m_annoClassList)
                {
                    this.FeatureLayer.AnnotationProperties.Add(api.Properties);
                }
                this.FeatureLayer.DisplayAnnotation = this.cbDisplayAnno.Checked;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.CurrentLayer != null)
            {
                if (this.tabControl.SelectedTab == this.tabPageGeneral)
                {//一般属性
                    this.ApplyGeneralPage();
                    if (this.ActiveView != null)
                    {
                        this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, this.FeatureLayer, this.ActiveView.Extent.Envelope);
                    }
                }
                else if (this.tabControl.SelectedTab == this.tabPageDisplayField)
                {//首选字段
                    this.ApplyDisplayField();
                }
                else if (this.tabControl.SelectedTab == this.tabPageLayerIndex)
                {//图层次序
                    this.ApplyLayerIndex();
                    if (this.ActiveView != null)
                    {
                        this.ActiveView.Refresh();
                    }
                }
                else if (this.tabControl.SelectedTab == this.tabPageAnno)
                {
                    this.ApplyAnnotate();
                    if (this.ActiveView != null)
                    {
                        this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGraphics
                            , this.FeatureLayer, this.ActiveView.Extent);
                    }
                }
                //else if (this.tabControl.SelectedTab == this.tabPageStyle)
                //{
                //    this.ApplyRenderStyle();
                //}
            }
        }

        private void UpdateLayerListViewItemOrder()
        {
            if (this.lvLayerList.Items.Count > 0)
            {
                int itemCount = this.lvLayerList.Items.Count;
                for (int ii = 0; ii < itemCount; ii++)
                {
                    ListViewItem lvi = this.lvLayerList.Items[ii];
                    lvi.Text = (ii + 1).ToString();
                    if (ii % 2 == 0) lvi.BackColor = Color.Wheat;
                    else lvi.BackColor = Color.White;
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            //上移一层
            if (this.lvLayerList.SelectedItems.Count > 0)
            {
                ListViewItem lvi = this.lvLayerList.SelectedItems[0];
                int oldIndex = lvi.Index;
                if (oldIndex > 0)
                {
                    int newIndex = oldIndex - 1;
                    lvi.Remove();
                    this.lvLayerList.Items.Insert(newIndex, lvi);
                    lvi.Selected = true;
                    UpdateLayerListViewItemOrder();
                }
            }
            else
            {
                MessageBox.Show("必须选中一个图层");
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            //下移一层
            if (this.lvLayerList.SelectedItems.Count > 0)
            {
                ListViewItem lvi = this.lvLayerList.SelectedItems[0];
                int oldIndex = lvi.Index;
                if (oldIndex < this.lvLayerList.Items.Count - 1)
                {
                    int newIndex = oldIndex + 1;
                    lvi.Remove();
                    this.lvLayerList.Items.Insert(newIndex, lvi);
                    lvi.Selected = true;
                    UpdateLayerListViewItemOrder();
                }
            }
            else
            {
                MessageBox.Show("必须选中一个图层");
            }
        }

        private void btnAsTop_Click(object sender, EventArgs e)
        {
            //作为顶层
            if (this.lvLayerList.SelectedItems.Count > 0)
            {
                ListViewItem lvi = this.lvLayerList.SelectedItems[0];
                int oldIndex = lvi.Index;
                if (oldIndex > 0)
                {
                    int newIndex = 0;
                    lvi.Remove();
                    this.lvLayerList.Items.Insert(newIndex, lvi);
                    lvi.Selected = true;
                    UpdateLayerListViewItemOrder();
                }
            }
            else
            {
                MessageBox.Show("必须选中一个图层");
            }
        }

        private void btnAsBottom_Click(object sender, EventArgs e)
        {
            //作为底层
            if (this.lvLayerList.SelectedItems.Count > 0)
            {
                ListViewItem lvi = this.lvLayerList.SelectedItems[0];
                int oldIndex = lvi.Index;
                if (oldIndex < this.lvLayerList.Items.Count - 1)
                {
                    lvi.Remove();
                    int newIndex = this.lvLayerList.Items.Count;
                    this.lvLayerList.Items.Insert(newIndex, lvi);
                    lvi.Selected = true;
                    UpdateLayerListViewItemOrder();
                }
            }
            else
            {
                MessageBox.Show("必须选中一个图层");
            }
        }

        private void btnAllSelectable_Click(object sender, EventArgs e)
        {
            //全部可选
            foreach (ListViewItem lvi in this.lvLayerList.Items)
            {
                IGeoFeatureLayer curLayer = lvi.Tag as IGeoFeatureLayer;
                if (curLayer != null)
                {
                    lvi.SubItems[4].Text = "可选";
                }
            }
        }

        private void btnAllUnSelectable_Click(object sender, EventArgs e)
        {
            //全部不可选
            foreach (ListViewItem lvi in this.lvLayerList.Items)
            {
                IGeoFeatureLayer curLayer = lvi.Tag as IGeoFeatureLayer;
                if (curLayer != null)
                {
                    lvi.SubItems[4].Text = "不可选";
                }
            }
        }
        private void OnLayerSelectableChanged(object sender, EventArgs e)
        {//图层的可选择性改变
            if (this.lvLayerList.SelectedItems.Count > 0)
            {
                ListViewItem selItem = this.lvLayerList.SelectedItems[0];
                if (this.ceLayerSelectable.Checked)
                {
                    selItem.SubItems[4].Text = "可选";
                }
                else
                {
                    selItem.SubItems[4].Text = "不可选";
                }
            }
        }
        private void lvLayerList_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem selItem = this.lvLayerList.GetItemAt(e.X, e.Y);
            this.ceLayerSelectable.Visible = false;
            if (selItem != null)
            {
                selItem.Selected = true;
                if (selItem.Tag is IGeoFeatureLayer)
                {//只有要素图层才能选中
                    int colIndex = OtherHelper.QueryColumnIndex(this.lvLayerList, e.X);
                    if (colIndex == 4)
                    {//只有第四个字段才是控制选中的.
                        int colLocX = OtherHelper.QueryColumnLocation(this.lvLayerList, colIndex);
                        int colLocY = selItem.Bounds.Top;
                        this.ceLayerSelectable.Location = new System.Drawing.Point(colLocX, colLocY);
                        this.ceLayerSelectable.Visible = true;
                        if (selItem.SubItems[colIndex].Text.Equals("可选"))
                        {
                            this.ceLayerSelectable.Checked = true;
                        }
                        else
                        {
                            this.ceLayerSelectable.Checked = false;
                        }
                    }
                }
            }
        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            AnnotateClassForm classForm = new AnnotateClassForm();
            if (classForm.ShowDialog(this) == DialogResult.OK)
            {
                AnnotationPropertyItem propItem = new AnnotationPropertyItem();
                propItem.Properties.Class = classForm.AnnotateClassName;
                propItem.Properties.WhereClause = classForm.AnnotateWhereClause;
                this.m_annoClassList.Add(propItem);
                ComboBoxItem cbi = new ComboBoxItem(propItem, classForm.AnnotateClassName, this.m_annoClassList.Count);
                int newIndex = this.cbAnnoClass.Items.Add(cbi);
                this.cbAnnoClass.SelectedIndex = newIndex;
            }
        }

        private void btnDelClass_Click(object sender, EventArgs e)
        {
            //删除注记类
            if (this.cbAnnoClass.SelectedItem is ComboBoxItem)
            {
                ComboBoxItem cbi = this.cbAnnoClass.SelectedItem as ComboBoxItem;
                AnnotationPropertyItem propItem = cbi.ItemObject as AnnotationPropertyItem;
                this.m_annoClassList.Remove(propItem);
                this.cbAnnoClass.Items.Remove(cbi);
                if (this.cbAnnoClass.Items.Count > 0)
                {
                    this.cbAnnoClass.SelectedIndex = 0;
                }
            }
        }

        private void cbAnnoClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //选中注记类
            if (this.cbAnnoClass.SelectedItem is ComboBoxItem)
            {
                ComboBoxItem cbi = this.cbAnnoClass.SelectedItem as ComboBoxItem;
                AnnotationPropertyItem propItem = cbi.ItemObject as AnnotationPropertyItem;
                this.m_shouldAction = false;
                this.ceUseClass.Checked = propItem.Properties.DisplayAnnotation;
                if (propItem.LabelEngineLayerProperties.IsExpressionSimple)
                {//简单标注
                    this.ceAnnoField.Checked = true;
                    this.cbAnnoFieldList.Enabled = true;
                    this.ceAnnoExpress.Checked = false;
                    this.beAnnoExpress.Enabled = false;
                    string fldName = propItem.LabelEngineLayerProperties.Expression;
                    if (fldName.StartsWith("[")) fldName = fldName.Substring(1);
                    if (fldName.EndsWith("]")) fldName = fldName.Substring(0, fldName.Length - 1);
                    int fldIndex = this.FeatureLayer.FeatureClass.Fields.FindField(fldName);
                    if (fldIndex >= 0) this.cbAnnoFieldList.SelectedIndex = fldIndex;
                }
                else
                {//表达式标注
                    this.ceAnnoField.Checked = false;
                    this.cbAnnoFieldList.Enabled = false;
                    this.ceAnnoExpress.Checked = true;
                    this.beAnnoExpress.Enabled = true;
                }
                #region 提取符号字体
                ITextSymbol ts = propItem.LabelEngineLayerProperties.Symbol;
                string fName = "宋体"; float fSize = 8.0F; bool fBold = false;
                bool fItalic = false; bool fUnderline = false; bool fStroke = false;
                Color fColor = Color.Black;
                if (ts != null && ts.Font != null)
                {
                    fName = ts.Font.Name;
                    fSize = Convert.ToSingle(ts.Font.Size);
                    fBold = ts.Font.Bold;
                    fItalic = ts.Font.Italic;
                    fUnderline = ts.Font.Underline;
                    fStroke = ts.Font.Strikethrough;
                    fColor = ColorHelper.CreateColor(ts.Color);
                }
                #region 查找对应的字体名
                fName = fName.ToUpper();
                int fCount = this.cbFontName.Items.Count;
                for (int fi = 0; fi < fCount; fi++)
                {
                    ComboBoxItem fItem = this.cbFontName.Items[fi] as ComboBoxItem;
                    FontFamily ff = fItem.ItemObject as FontFamily;
                    if (ff != null && ff.Name.ToUpper().Equals(fName))
                    {
                        this.cbFontName.SelectedIndex = fi;
                        break;
                    }
                }
                if (this.cbFontName.SelectedItem == null && fCount > 0)
                {
                    this.cbFontName.SelectedIndex = 0;
                }
                #endregion
                #region 设置其它属性
                this.cbFontSize.Text = fSize.ToString();
                this.ceFontBold.Checked = fBold;
                this.ceFontItalic.Checked = fItalic;
                this.ceFontUnderline.Checked = fUnderline;
                this.ceFontStroke.Checked = fStroke;
                this.ceFontColor.EditValue = fColor;
                #endregion
                try
                {
                    Image styleImage = SymbolHelper.StyleToImage(ts as ISymbol, this.pbFontSample.Width, this.pbFontSample.Height);
                    this.pbFontSample.Image = styleImage;
                }
                catch
                {
                }
                #endregion
                this.m_shouldAction = true;
            }
        }

        private void ceUseClass_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_shouldAction)
            {
                if (this.cbAnnoClass.SelectedItem is ComboBoxItem)
                {
                    ComboBoxItem cbi = this.cbAnnoClass.SelectedItem as ComboBoxItem;
                    AnnotationPropertyItem propItem = cbi.ItemObject as AnnotationPropertyItem;
                    propItem.Properties.DisplayAnnotation = this.ceUseClass.Checked;
                }
            }
        }

        private void ceAnnoField_CheckedChanged(object sender, EventArgs e)
        {
            //使用字段
            if (this.m_shouldAction)
            {
                if (this.ceAnnoField.Checked)
                {//选中
                    this.cbAnnoFieldList.Enabled = true;
                    this.ceAnnoExpress.Checked = false;
                    if (this.CurrentAnnotateClass != null)
                    {
                        this.CurrentAnnotateClass.LabelEngineLayerProperties.IsExpressionSimple = true;
                        ComboBoxItem selItem = this.cbAnnoFieldList.SelectedItem as ComboBoxItem;
                        if (selItem != null)
                        {
                            IField curFld = selItem.ItemObject as IField;
                            this.CurrentAnnotateClass.LabelEngineLayerProperties.Expression = "[" + curFld.Name + "]";
                        }
                    }
                }
                else
                {//取消选中
                    this.cbAnnoFieldList.Enabled = false;
                }
            }
        }

        private void ceAnnoExpress_CheckedChanged(object sender, EventArgs e)
        {
            //使用表达式
            if (this.m_shouldAction)
            {
                if (this.ceAnnoExpress.Checked)
                {//选中
                    this.beAnnoExpress.Enabled = true;
                    this.ceAnnoField.Checked = false;
                    if (this.CurrentAnnotateClass != null)
                    {
                        this.CurrentAnnotateClass.LabelEngineLayerProperties.IsExpressionSimple = false;
                        string exp = this.beAnnoExpress.Text;
                        if (exp.StartsWith("JS:"))
                        {
                            this.CurrentAnnotateClass.LabelEngineLayerProperties.ExpressionParser = new AnnotationJScriptEngineClass();
                        }
                        else if (exp.StartsWith("VB:"))
                        {
                            this.CurrentAnnotateClass.LabelEngineLayerProperties.ExpressionParser = new AnnotationVBScriptEngineClass();
                        }
                        if (exp.Length > 3)
                        {
                            this.CurrentAnnotateClass.LabelEngineLayerProperties.Expression = exp.Substring(3);
                        }
                    }
                }
                else
                {//取消选中
                    this.beAnnoExpress.Enabled = false;
                }
            }
        }

        private void cbAnnoFieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //字段改变了
            if (this.CurrentAnnotateClass != null)
            {
                this.CurrentAnnotateClass.LabelEngineLayerProperties.IsExpressionSimple = true;
                ComboBoxItem selItem = this.cbAnnoFieldList.SelectedItem as ComboBoxItem;
                if (selItem != null)
                {
                    IField curFld = selItem.ItemObject as IField;
                    this.CurrentAnnotateClass.LabelEngineLayerProperties.Expression = "[" + curFld.Name + "]";
                }
            }
        }

        private void beAnnoExpress_EditValueChanged(object sender, EventArgs e)
        {
            //标注表达式改变了
            if (this.CurrentAnnotateClass != null)
            {
                this.CurrentAnnotateClass.LabelEngineLayerProperties.IsExpressionSimple = false;
                string exp = this.beAnnoExpress.Text;
                if (exp.StartsWith("JS:"))
                {
                    this.CurrentAnnotateClass.LabelEngineLayerProperties.ExpressionParser = new AnnotationJScriptEngineClass();
                }
                else if (exp.StartsWith("VB:"))
                {
                    this.CurrentAnnotateClass.LabelEngineLayerProperties.ExpressionParser = new AnnotationVBScriptEngineClass();
                }
                if (exp.Length > 3)
                {
                    this.CurrentAnnotateClass.LabelEngineLayerProperties.Expression = exp.Substring(3);
                }
            }
        }

        private void beAnnoExpress_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (this.CurrentAnnotateClass != null)
            {
                AnnotateExpressForm expForm = new AnnotateExpressForm();
                expForm.FeatureLayer = this.FeatureLayer;
                string exp = this.CurrentAnnotateClass.LabelEngineLayerProperties.Expression;
                string funName = "VB";
                IAnnotationExpressionEngine labelEngine = this.CurrentAnnotateClass.LabelEngineLayerProperties.ExpressionParser;
                AnnotationJScriptEngineClass JSEngine = new AnnotationJScriptEngineClass();
                AnnotationVBScriptEngineClass VBEngine = new AnnotationVBScriptEngineClass();
                if (labelEngine != null)
                {
                    if (labelEngine.Name.ToUpper().Equals(JSEngine.Name.ToUpper()))
                    {
                        funName = "JS:";
                    }
                    else if (labelEngine.Name.ToUpper().Equals(VBEngine.Name.ToUpper()))
                    {
                        funName = "VB:";
                    }
                }
                expForm.AnnotateExpress = funName + exp;
                if (expForm.ShowDialog(this) == DialogResult.OK)
                {
                    this.beAnnoExpress.Text = expForm.AnnotateExpress;
                }
            }
        }

        private void cbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //注记字体名称变化了
            #region
            this.ChangeCurrentFont();
            #endregion
        }

        private void cbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //注记字体大小变化了            
            this.ChangeCurrentFont();
        }

        private void ceFontBold_CheckedChanged(object sender, EventArgs e)
        {
            //注记
            this.ChangeCurrentFont();
        }

        private void ceFontItalic_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCurrentFont();
        }

        private void ceFontUnderline_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCurrentFont();
        }

        private void ceFontStroke_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCurrentFont();
        }

        private void ceFontColor_EditValueChanged(object sender, EventArgs e)
        {
            this.ChangeCurrentFont();
        }

        private void ChangeCurrentFont()
        {
            if (this.m_shouldAction)
            {
                if (this.CurrentAnnotateClass != null)
                {
                    ITextSymbol ts = this.CurrentAnnotateClass.LabelEngineLayerProperties.Symbol;
                    IFontDisp fDisp = this.CurrentFont;
                    if (fDisp != null)
                    {
                        ts.Font = fDisp;
                    }
                    try
                    {
                        ts.Color = ColorHelper.CreateColor((Color)this.ceFontColor.EditValue);
                    }
                    catch
                    {
                    }
                    Image sampleImage = SymbolHelper.StyleToImage(ts as ISymbol, this.pbFontSample.Width, this.pbFontSample.Height);
                    this.pbFontSample.Image = sampleImage;
                }
            }
        }

        private void btnAnnoScale_Click(object sender, EventArgs e)
        {
            if (this.CurrentAnnotateClass != null)
            {
                IAnnotateLayerProperties props = this.CurrentAnnotateClass.Properties;
                AnnotateScaleForm annoScale = new AnnotateScaleForm();
                if (props.AnnotationMaximumScale == 0
                    && props.AnnotationMinimumScale == 0.0)
                {
                    annoScale.ScaleSameToLayer = true;
                }
                else
                {
                    annoScale.ScaleSameToLayer = false;
                    annoScale.MaxScale = props.AnnotationMaximumScale;
                    annoScale.MinScale = props.AnnotationMinimumScale;
                }
                if (annoScale.ShowDialog(this) == DialogResult.OK)
                {
                    if (annoScale.ScaleSameToLayer)
                    {
                        props.AnnotationMaximumScale = 0;
                        props.AnnotationMinimumScale = 0;
                    }
                    else
                    {
                        props.AnnotationMaximumScale = annoScale.MaxScale;
                        props.AnnotationMinimumScale = annoScale.MinScale;
                    }
                }
            }
        }

        private void btnAnnoPlacement_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null && this.CurrentAnnotateClass != null)
            {
                ILabelEngineLayerProperties labelProp = this.CurrentAnnotateClass.LabelEngineLayerProperties;
                IBasicOverposterLayerProperties overposterProp = labelProp.BasicOverposterLayerProperties;
                if (this.FeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    AnnotatePlacementFill fillForm = new AnnotatePlacementFill();
                    fillForm.PlacementProperties = overposterProp as IBasicOverposterLayerProperties4;
                    if (fillForm.ShowDialog(this) == DialogResult.OK)
                    {
                        labelProp.BasicOverposterLayerProperties = fillForm.PlacementProperties as IBasicOverposterLayerProperties;
                    }
                }
                else if (this.FeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    AnnotatePlacementLine lineForm = new AnnotatePlacementLine();
                    lineForm.PlacementProperties = overposterProp as IBasicOverposterLayerProperties4;
                    if (lineForm.ShowDialog(this) == DialogResult.OK)
                    {
                        labelProp.BasicOverposterLayerProperties = lineForm.PlacementProperties as IBasicOverposterLayerProperties;
                    }
                }
                else if (this.FeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    AnnotatePlacementPoint ptForm = new AnnotatePlacementPoint();
                    ptForm.PlacementProperties = overposterProp as IBasicOverposterLayerProperties4;
                    if (ptForm.ShowDialog(this) == DialogResult.OK)
                    {
                        labelProp.BasicOverposterLayerProperties = ptForm.PlacementProperties as IBasicOverposterLayerProperties;
                    }
                }
                else
                {
                    MessageBox.Show("本系统不支持对该类型的图层做标注！", "不支持的图层类型");

                }
            }
        }
        private AnnotationPropertyItem CurrentAnnotateClass
        {
            get
            {
                AnnotationPropertyItem resultItem = null;
                if (this.cbAnnoClass.SelectedItem is ComboBoxItem)
                {

                    object itemObj = (this.cbAnnoClass.SelectedItem as ComboBoxItem).ItemObject;
                    resultItem = itemObj as AnnotationPropertyItem;
                }
                return resultItem;
            }
        }
        private IFontDisp CurrentFont
        {
            get
            {
                string fName = "宋体"; double fSize = 8.0; bool fBold = false;
                bool fItalic = false; bool fUnderline = false; bool fStroke = false;
                Color fColor = Color.Black;

                stdole.StdFontClass stdFont = new StdFontClass();
                ComboBoxItem fItem = this.cbFontName.SelectedItem as ComboBoxItem;
                if (fItem != null && fItem.ItemObject is FontFamily)
                {
                    fName = (fItem.ItemObject as FontFamily).Name;
                }
                if (!Double.TryParse(this.cbFontSize.Text, System.Globalization.NumberStyles.Any
                    , new System.Globalization.NumberFormatInfo(), out fSize))
                {
                    fSize = 8.0;
                }
                fBold = this.ceFontBold.Checked;
                fItalic = this.ceFontItalic.Checked;
                fUnderline = this.ceFontUnderline.Checked;
                fStroke = this.ceFontStroke.Checked;
                try
                {
                    fColor = (Color)this.ceFontColor.EditValue;
                }
                catch
                {
                }
                stdFont.Name = fName;
                stdFont.Size = Convert.ToDecimal(fSize);
                stdFont.Bold = fBold;
                stdFont.Italic = fItalic;
                stdFont.Underline = fUnderline;
                stdFont.Strikethrough = fStroke;
                IFontDisp fDisp = stdFont as IFontDisp;
                return fDisp;
            }
        }

        private void miCanceLoadData_Click(object sender, EventArgs e)
        {

            //this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.ActiveView.Extent);
        }

        private void mi_Click(object sender, EventArgs e)
        {
            //if (this.FeatureLayer != null)
            //{
            //    //找到第一列
            //    string fldName=this.dataGridView.Columns[0].FieldName.ToUpper();

            //    string sBSM = this.dataGridView.GetRowCellValue(dataGridView.FocusedRowHandle, fldName).ToString();

            //    IFeatureClass pFC = this.FeatureLayer.FeatureClass;

            //    IFeature selFea = GetFeature(fldName, sBSM, pFC);

            //    if (selFea != null)
            //    {
            //        IGeometry geom = selFea.Shape;
            //        if (geom != null && !geom.IsEmpty)
            //        {
            //            IEnvelope env = geom.Envelope;
            //            env.Expand(1.5, 1.5, true);
            //            this.ActiveView.Extent = env;


            //            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, env);
            //            this.ActiveView.ScreenDisplay.UpdateWindow();
            //            this.mapCtrol3.FlashShape(geom, 3, 300, null);


            //        }
            //    }
            //}
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            //if (this.FeatureLayer != null)
            //{
            //    string fldName = this.dataGridView.Columns[0].FieldName.ToUpper();

            //    string sBSM = this.dataGridView.GetRowCellValue(dataGridView.FocusedRowHandle, fldName).ToString();
            //    IFeatureClass pFC = this.FeatureLayer.FeatureClass;

            //    IFeature selFea = GetFeature(fldName,sBSM, pFC);
            //    if (selFea != null)
            //    {
            //        IGeometry geom = selFea.Shape;
            //        if (geom != null && !geom.IsEmpty)
            //        {
            //            string aLayerName = LayerHelper.GetClassShortName(selFea.Table as IDataset);
            //            ILayer aLayer = LayerHelper.QueryLayerByModelName(this.ActiveView.FocusMap, aLayerName);
            //            this.ActiveView.FocusMap.SelectFeature(aLayer, selFea);
            //            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, geom.Envelope);
            //            this.ActiveView.ScreenDisplay.UpdateWindow();
            //        }
            //    }
            //}
        }
        private IFeature GetFeature(string fldName, string sValue, IFeatureClass pFC)
        {
            IQueryFilter qf = new QueryFilterClass();
            qf.WhereClause = fldName+ " =" + sValue + "";
            IFeatureCursor pCursor = pFC.Search(qf, false);
            IFeature pFea = pCursor.NextFeature();

            if (pFea != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                return pFea;
            }
            OtherHelper.ReleaseComObject(pCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(qf);
            return null;
        }
        private void menuItem1_Click(object sender, EventArgs e)
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Execl files (*.xls)|*.xls|HTML files (*.html)|*.html";
            //saveFileDialog.FilterIndex = 0;
            //saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.CreatePrompt = true;
            //saveFileDialog.Title = "导出Excel文件到";

            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    if (saveFileDialog.FilterIndex == 1)
            //    {
            //        this.gridControl.ExportToXls(saveFileDialog.FileName);
            //    }
            //    else if (saveFileDialog.FilterIndex == 2)
            //    {
            //        this.gridControl.ExportToHtml(saveFileDialog.FileName);
            //    }
            //    MessageBox.Show("导出完毕");
            //}
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                this.teBrightness.Text = this.trackBar2.Value.ToString();

                int Brightness = Convert.ToInt32(this.teBrightness.Text);

                ILayerEffects layerEff = this.CurrentLayer as ILayerEffects;
                if (layerEff.SupportsBrightnessChange == true)
                {
                    if (layerEff != null) layerEff.Brightness = (short)Brightness;
                    this.m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, m_activeView.Extent);
                }
            }
            catch { }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            try
            {
                this.teContrast.Text = this.trackBar3.Value.ToString();

                ILayerEffects layerEff = this.CurrentLayer as ILayerEffects;
                int Contrast = Convert.ToInt32(this.teContrast.Text.Trim());

                if (layerEff.SupportsContrastChange == true)
                {
                    if (layerEff != null) layerEff.Contrast = (short)Contrast;
                    this.m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, m_activeView.Extent);
                }
            }
            catch { }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                this.teAlpha.Text = this.trackBar1.Value.ToString();

                int alpha = Convert.ToInt32(this.teAlpha.Text);

                ILayerEffects layerEff = this.CurrentLayer as ILayerEffects;
                if (layerEff.SupportsTransparency == true)
                {
                    if (layerEff != null) layerEff.Transparency = (short)alpha;
                    this.m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, m_activeView.Extent);
                }
            }
            catch { }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 初始化某些TabPage
            if (this.tabControl.SelectedTab == this.tabPageGeneral)
            {//一般信息
                this.InitGeneralInfo();
            }
            else if (this.tabControl.SelectedTab == this.tabPageAnno)
            {//图层注记
                this.InitAnnoClass();
            }
            else if (this.tabControl.SelectedTab == this.tabPageDisplayField)
            {//首选字段                
                this.InitDisplayField();
            }
            else if (this.tabControl.SelectedTab == this.tabPageLayerIndex)
            {//显示次序
                this.InitLayerDisplayIndex();
            }

            //else if (this.tabControl.SelectedTab == this.tabPageData)
            //{//数据
            //    this.InitDataGrid();
            //}

            #endregion
        }

    }

    public class AnnotationPropertyItem
    {
        public IAnnotateLayerProperties Properties;
        public IElementCollection PlacedElementCollection;
        public IElementCollection UnplacedElementCollection;
        public AnnotationPropertyItem()
        {
            Properties = new LabelEngineLayerPropertiesClass();
            this.PlacedElementCollection = new ElementCollectionClass();
            this.UnplacedElementCollection = new ElementCollectionClass();
        }
        public ILabelEngineLayerProperties LabelEngineLayerProperties
        {
            get
            {
                return this.Properties as ILabelEngineLayerProperties;
            }
        }
        public void Clone()
        {
            this.Properties = (this.Properties as IClone).Clone() as IAnnotateLayerProperties;
        }
    }
}