using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;


using RCIS.GISCommon;

namespace RCIS.MapTool
{
    public partial class ObjectPropertyForm : Form
    {
        private IPolygon m_identifyGeom;

        private IColor m_identifyColor;

        public ObjectPropertyForm()
        {
            InitializeComponent();
        }
        private void InitOnCreate()
        {
            this.m_identifyColor = ColorHelper.CreateColor(0, 0, 255);
        }

        private AxMapControl mMapCtrl;
        public AxMapControl MapControl
        {
            get
            {
                return this.mMapCtrl;
            }
            set
            {
                this.mMapCtrl = value;
            }
        }

        private void ClearIdnetifyGeometry()
        {
            this.m_identifyGeom = null;
        }
        public IPoint QueryLocation
        {
            get
            {
                if (this.m_identifyGeom != null
                    && !this.m_identifyGeom.IsEmpty)
                {
                    if (this.m_identifyGeom is IPoint)
                    {
                        return this.m_identifyGeom as IPoint;
                    }
                    else
                    {
                        IArea aArea = this.m_identifyGeom as IArea;
                        if (aArea != null)
                            return aArea.Centroid;
                    }
                }
                return new PointClass();
            }
        }

        private IActiveView m_activeView;

        public IActiveView ActiveView
        {
            get
            {
                return this.m_activeView;
            }
            set
            {
                try
                {
                    this.m_activeView = value;
                    this.RefreshLayer();
                }
                catch  { }
            }
        }
        public void RefreshLayer()
        {
            if (this.m_activeView != null)
            {
                this.targetLayerComboBox.Items.Clear();
                TargetLayerList topLayer = new TargetLayerList("最上的图层");
                TargetLayerList selectLayer = new TargetLayerList("可选的图层");
                TargetLayerList visibleLayer = new TargetLayerList("可见的图层");

                ArrayList singleLayerList = new ArrayList();
                int layerCount = (this.m_activeView.FocusMap).LayerCount;
                bool hasFirst = false;
                for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                {
                    ILayer curLayer = (this.m_activeView.FocusMap).get_Layer(layerIndex);
                    if (curLayer is IGeoFeatureLayer)
                    {
                        if (curLayer.Visible)
                        {
                            if (!hasFirst)
                            {//最顶部的图层
                                topLayer.CartoLayerList.Add(curLayer);
                                hasFirst = true;
                            }
                            visibleLayer.CartoLayerList.Add(curLayer);
                            TargetLayerList curLayerList = new TargetLayerList(curLayer.Name);
                            curLayerList.CartoLayerList.Add(curLayer);

                            singleLayerList.Add(curLayerList);
                            IGeoFeatureLayer geoLayer = curLayer as IGeoFeatureLayer;
                            if (geoLayer.Selectable)
                            {
                                selectLayer.CartoLayerList.Add(geoLayer);
                            }
                        }
                    }
                }
                if (singleLayerList.Count > 0)
                {
                    this.targetLayerComboBox.Items.Add(topLayer);
                    this.targetLayerComboBox.Items.Add(selectLayer);
                    this.targetLayerComboBox.Items.Add(visibleLayer);
                    foreach (TargetLayerList curTargetList in singleLayerList)
                    {
                        this.targetLayerComboBox.Items.Add(curTargetList);
                    }
                    try
                    {
                        this.targetLayerComboBox.SelectedItem = visibleLayer;
                    }
                    catch { }
                }
            }
        }
        public void Identify(IPolygon pPoly)
        {
            try
            {
                this.objectTreeView.Nodes.Clear();
                this.objectInfoListView.Columns.Clear();
                this.objectInfoListView.Items.Clear();
                this.ClearIdnetifyGeometry();
                this.m_identifyGeom = pPoly;
                this.m_identifyGeom.Project(this.MapControl.SpatialReference);

                if (this.m_curTargetLayerList != null
                    && pPoly != null && !pPoly.IsEmpty)
                {

                    this.ShowQueryLocation(this.QueryLocation);

                    ISpatialFilter queryFilter = new SpatialFilterClass();
                    queryFilter.Geometry = pPoly;
                    queryFilter.GeometryField = "SHAPE";
                    queryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                    ArrayList cartoLayerList = this.m_curTargetLayerList.CartoLayerList;
                    int layerCount = cartoLayerList.Count;
                    double step = 0;
                    if (layerCount > 0)
                    {
                        step = 100.0 / layerCount;
                    }
                    for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                    {
                        #region 查询具体某一图层中的数据
                        IGeoFeatureLayer geoLayer = cartoLayerList[layerIndex] as IGeoFeatureLayer;
                        if (geoLayer != null)
                        {
                            IFeatureCursor geoCur = geoLayer.Search(queryFilter, false);
                            IFeature geoFea = geoCur.NextFeature();
                            ArrayList geoFeaList = new ArrayList();
                            while (geoFea != null)
                            {
                                geoFeaList.Add(geoFea);
                                geoFea = geoCur.NextFeature();
                            }
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(geoCur);
                            if (geoFeaList.Count > 0)
                            {
                                #region 如果有数据 添加到书中
                                TreeNode layerNode = new TreeNode(geoLayer.Name);
                                SelectedLayerNode selLayer = new SelectedLayerNode();
                                selLayer.SelectedLayer = geoLayer as IFeatureLayer;
                                selLayer.SelectedFeature = geoFeaList;
                                layerNode.Tag = selLayer;

                                if (geoLayer.DisplayField == null || geoLayer.DisplayField.Equals(""))
                                {
                                    geoLayer.DisplayField = geoLayer.FeatureClass.OIDFieldName;
                                }
                                int dispFldIndex = geoLayer.FeatureClass.FindField(geoLayer.DisplayField);

                                foreach (IFeature curGeoFea in geoFeaList)
                                {
                                    if (dispFldIndex >= 0)
                                    {
                                        object ov = curGeoFea.get_Value(dispFldIndex);
                                        TreeNode geoFeaNode = new TreeNode(ov.ToString());
                                        geoFeaNode.Tag = curGeoFea;
                                        layerNode.Nodes.Add(geoFeaNode);
                                    }
                                }
                                this.objectTreeView.Nodes.Add(layerNode);
                                #endregion
                            }
                        }
                        #endregion
                    }
                    if (this.objectTreeView.Nodes.Count > 0)
                    {
                        TreeNode aNode = this.objectTreeView.Nodes[0];
                        while (aNode.Nodes.Count > 0)
                        {
                            aNode = aNode.Nodes[0];
                        }
                        this.objectTreeView.SelectedNode = aNode;
                    }
                }
                //this.m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, pPoly as IEnvelope);

                this.m_activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.m_activeView.Extent);
            }
            catch(Exception ex) { }

        }
        public void Identify(IPoint pPoint)
        {
            try
            {
                if (pPoint == null || pPoint.IsEmpty)
                    return;
                IGeometry qGeom = (pPoint as ITopologicalOperator)
                    .Buffer(0.1);
                this.Identify(qGeom as IPolygon);
            }
            catch  { }
        }
        public void Identify(IEnvelope pEnv)
        {
            if (pEnv == null || pEnv.IsEmpty)
                return;
            PolygonClass aPoly = new PolygonClass();
            aPoly.SetRectangle(pEnv);
            this.Identify(aPoly);
        }

        private void ShowQueryLocation(IPoint idPt)
        {
            if (idPt != null && !idPt.IsEmpty)
            {
                double px = Math.Round(idPt.X, 4);
                double py = Math.Round(idPt.Y, 4);
                this.lbPosition.Text = "中心位置(" + px + "," + py + ")";
            }
        }
        private TargetLayerList m_curTargetLayerList;

        private void targetLayerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当前图层改变了
            object selItem = this.targetLayerComboBox.SelectedItem;
            this.m_curTargetLayerList = selItem as TargetLayerList;
            if (this.m_identifyGeom != null
                && !this.m_identifyGeom.IsEmpty)
            {
                this.Identify(this.m_identifyGeom);
            }
        }

        private void objectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selNode = this.objectTreeView.SelectedNode;

            if (selNode != null)
            {
                if (selNode.Tag is IFeature)
                {
                    SelectedLayerNode layerNode = selNode.Parent.Tag as SelectedLayerNode;
                    this.ShowFeature(layerNode.SelectedLayer, selNode.Tag as IFeature);


                }
                else if (selNode.Tag is SelectedLayerNode)
                {
                    this.ShowFeatureLayer(selNode.Tag as SelectedLayerNode);
                }
            }
            else
            {
                this.propertyGroup.Visible = false;
            }
        }

        private void ShowFeature(IFeatureLayer curLayer, IFeature curFea)
        {
            Size newSize = this.Size;
            newSize.Width = 500;
            this.Size = newSize;
            this.objectInfoListView.Columns.Clear();
            this.objectInfoListView.Items.Clear();

            this.objectInfoListView.Columns.Add("属性名称", 150, HorizontalAlignment.Left);
            this.objectInfoListView.Columns.Add("属性数值", 300, HorizontalAlignment.Left);

            int fldCount = curFea.Fields.FieldCount;
            for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
            {
                #region 逐个字段处理
                IField curFld = curFea.Fields.get_Field(fldIndex);
                ListViewItem lvi = new ListViewItem();
                if (fldIndex % 2 == 0)
                {
                    lvi.BackColor = Color.Wheat;
                }
                if (curFld.Type == esriFieldType.esriFieldTypeOID)
                {//图形ID
                    lvi.Text = "要素ID";
                    lvi.SubItems.Add(curFea.get_Value(fldIndex).ToString());
                }
                else if (curFld.Type == esriFieldType.esriFieldTypeGeometry)
                {
                    #region 图形字段
                    lvi.Text = "要素图形";
                    esriGeometryType geoType = curLayer.FeatureClass.ShapeType;
                    if (curFea.Shape == null)
                    {
                        lvi.SubItems.Add("空图形");
                    }
                    else if (esriGeometryType.esriGeometryPoint == geoType)
                    {
                        lvi.SubItems.Add("点图形");
                    }
                    else if (esriGeometryType.esriGeometryPolyline == geoType)
                    {
                        lvi.SubItems.Add("线图形");
                    }
                    else if (esriGeometryType.esriGeometryPolygon == geoType)
                    {
                        lvi.SubItems.Add("面图形");
                    }
                    #endregion
                }
                else if (curFld == curLayer.FeatureClass.AreaField)
                {//图形面积
                    lvi.Text = "图形计算面积";
                    lvi.SubItems.Add(curFea.get_Value(fldIndex).ToString());
                }
                else if (curFld == curLayer.FeatureClass.LengthField)
                {//图形长度
                    lvi.Text = "图形计算长度";
                    lvi.SubItems.Add(curFea.get_Value(fldIndex).ToString());
                }
                else
                {//图形其他属性
                    lvi.Text = curFld.AliasName;
                    lvi.SubItems.Add(curFea.get_Value(fldIndex).ToString());
                }
                this.objectInfoListView.Items.Add(lvi);
                #endregion
            }
            this.ActiveView.FocusMap.ClearSelection();
            this.ActiveView.FocusMap.SelectFeature(curLayer, curFea);
        }
        private void ShowFeatureLayer(SelectedLayerNode curSelNode)
        {
            Size newSize = this.Size;
            newSize.Width = 840;
            this.Size = newSize;
            this.ActiveView.FocusMap.ClearSelection();
            if (curSelNode != null && curSelNode.SelectedLayer != null)
            {
                this.objectInfoListView.Items.Clear();
                this.objectInfoListView.Columns.Clear();
                this.objectInfoListView.Columns.Add("序号", 40, HorizontalAlignment.Left);

                IFeatureClass selFeaClass = curSelNode.SelectedLayer.FeatureClass;
                int fldCount = selFeaClass.Fields.FieldCount;
                int shpFieldIndex = -1;
                //ProgressPanel.TaskStepCount =fldCount;

                for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
                {
                    #region 添加列
                    //ProgressPanel.ForwardStep ();
                    IField curFld = selFeaClass.Fields.get_Field(fldIndex);

                    if (curFld.Type == esriFieldType.esriFieldTypeOID)
                    {
                        this.objectInfoListView.Columns.Add("要素ID", 60, HorizontalAlignment.Left);
                    }
                    else if (curFld.Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        shpFieldIndex = fldIndex;
                        this.objectInfoListView.Columns.Add("要素几何图形", 60, HorizontalAlignment.Left);
                    }
                    else if (curFld == selFeaClass.AreaField)
                    {
                        this.objectInfoListView.Columns.Add("图形计算面积", 60, HorizontalAlignment.Left);
                    }
                    else if (curFld == selFeaClass.LengthField)
                    {
                        this.objectInfoListView.Columns.Add("图形计算长度", 60, HorizontalAlignment.Left);
                    }
                    else
                    {
                        this.objectInfoListView.Columns.Add(curFld.AliasName, 60, HorizontalAlignment.Left);
                    }

                    #endregion
                }
                string geoType = "";

                if (selFeaClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    geoType = "点图形";
                }
                else if (selFeaClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    geoType = "线图形";
                }
                else if (selFeaClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    geoType = "面图形";
                }
                int geoCount = curSelNode.SelectedFeature.Count;
                //ProgressPanel.TaskStepCount =geoCount;
                for (int geoIndex = 0; geoIndex < geoCount; geoIndex++)
                {
                    //ProgressPanel.ForwardStep ();
                    IFeature curGeoFea = curSelNode.SelectedFeature[geoIndex] as IFeature;
                    this.ActiveView.FocusMap.SelectFeature(curSelNode.SelectedLayer, curGeoFea);
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = (geoIndex + 1).ToString();
                    if (geoIndex % 2 == 0)
                    {
                        lvi.BackColor = Color.Wheat;
                    }
                    for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
                    {

                        if (fldIndex == shpFieldIndex)
                        {
                            if (curGeoFea.Shape == null)
                            {
                                lvi.SubItems.Add("空图形");
                            }
                            else
                            {
                                lvi.SubItems.Add(geoType);
                            }
                        }
                        else
                        {
                            object ov = curGeoFea.get_Value(fldIndex);
                            lvi.SubItems.Add(ov.ToString());
                        }
                    }
                    lvi.Tag = curGeoFea;
                    this.objectInfoListView.Items.Add(lvi);
                }
            }
            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.ActiveView.Extent);
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            //定位到要素
            TreeNode selNode = this.objectTreeView.SelectedNode;
            if (selNode != null)
            {
                if (selNode.Tag is IFeature)
                {
                    IFeature selFea = selNode.Tag as IFeature;
                    IGeometry selGeom = selFea.Shape;
                    if (selGeom != null && !selGeom.IsEmpty)
                    {
                        IEnvelope selEnv = selGeom.Envelope;
                        selEnv.Expand(1.5, 1.5, true);
                        this.ActiveView.Extent = selEnv;
                        this.ActiveView.Refresh();
                    }
                }
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            //闪烁图形
            TreeNode selNode = this.objectTreeView.SelectedNode;
            if (selNode != null)
            {
                if (selNode.Tag is IFeature)
                {
                    IFeature selFea = selNode.Tag as IFeature;
                    IGeometry selGeom = selFea.ShapeCopy;
                    if (selGeom != null && !selGeom.IsEmpty)
                    {
                        ITopologicalOperator pTop = selGeom as ITopologicalOperator;
                        if (selGeom is IPoint)
                        {
                            selGeom = pTop.Buffer(0.5);
                        }
                        else if (selGeom is IPolyline)
                        {
                            selGeom = pTop.Buffer(0.5);
                        }

                        ISimpleFillSymbol iFillSymbol;
                        ISymbol iSymbol;
                        IRgbColor iRgbColor;
                        iFillSymbol = new SimpleFillSymbol();
                        iFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        iFillSymbol.Outline.Width = 12;
                        iRgbColor = new RgbColor();
                        iRgbColor.Red = 0;
                        iRgbColor.Green = 162;
                        iRgbColor.Blue = 232;
                        iFillSymbol.Color = iRgbColor;
                        iSymbol = (ISymbol)iFillSymbol;
                        iSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;  

                        if (this.MapControl != null)
                        {
                            this.MapControl.FlashShape(selGeom, 3, 300, iSymbol);
                        }
                    }
                }
            }
        }

        private void objectTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode selNode = this.objectTreeView.GetNodeAt(e.X, e.Y);
                if (selNode != null)
                {
                    this.objectTreeView.SelectedNode = selNode;
                }
                this.contextMenu1.Show(this.objectTreeView, new System.Drawing.Point(e.X, e.Y));
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            //特殊标记要素
            //TreeNode selNode = this.objectTreeView.SelectedNode;
            //if (selNode != null)
            //{
            //    if (selNode.Tag is IFeature)
            //    {
            //        IFeature selFea = selNode.Tag as IFeature;
            //        IGeometry selGeom = selFea.Shape;
            //        this.m_emrmarkCmd.MarkerName = this.m_markerName;
            //        this.m_emrmarkCmd.Mark = false;
            //        this.m_emrmarkCmd.Execute();

            //        //this.m_emrmarkCmd .EarmarkerSymbol=SymbolHelper.CreateSymbolFromColor (StyleClass .GetStyleClass (
            //        this.m_emrmarkCmd.GeometryBeMarkered = selGeom;
            //        this.m_emrmarkCmd.Mark = true;
            //        this.m_emrmarkCmd.Execute();
            //    }
            //}
        }

        private class TargetLayerList
        {
            public TargetLayerList(string layerName)
            {
                this.LayerListName = layerName;
            }
            public string LayerListName = "";
            public ArrayList CartoLayerList = new ArrayList();
            public override string ToString()
            {
                return this.LayerListName;
            }
        }
        private class SelectedLayerNode
        {//图层节点
            public IFeatureLayer SelectedLayer;
            public ArrayList SelectedFeature;
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                this.m_identifyColor = ColorHelper.CreateColor(cd.Color);
            }
        }

        private void objectInfoListView_KeyDown(object sender, KeyEventArgs e)
        {
            //实现复制功能
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (this.objectInfoListView.SelectedItems.Count > 0)
                {
                    //将复制的内容放入剪切板中
                    ListViewItem aItem = objectInfoListView.SelectedItems[0];
                    string s = "";
                    for (int i = 0; i < aItem.SubItems.Count; i++)
                    {
                        s += " " + aItem.SubItems[i].Text;
                    }
                    
                    Clipboard.SetDataObject(s);
                }
            }

        }

        private void 复制内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.objectInfoListView.SelectedItems.Count > 0)
            {
                //将复制的内容放入剪切板中
                ListViewItem aItem = objectInfoListView.SelectedItems[0];
                string s = "";
                for (int i = 0; i < aItem.SubItems.Count; i++)
                {
                    s += " " + aItem.SubItems[i].Text;
                }

                Clipboard.SetDataObject(s);
            }
        }



    }
}