using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.sys
{
    public partial class TableStructForm : Form
    {
        public TableStructForm()
        {
            InitializeComponent();
        }

        private IWorkspace currWs = null;

        private void TableStructForm_Load(object sender, EventArgs e)
        {
            this.bePgdb.Enabled = false;

            fieldTable = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "FIELDNAME";
            dc.DataType = typeof(string);
            fieldTable.Columns.Add(dc);
            dc = new DataColumn("FIELDALIAS", typeof(string));
            fieldTable.Columns.Add(dc);
            dc=new DataColumn("FIELDTYPE",typeof(string));
            fieldTable.Columns.Add(dc);
            dc=new DataColumn("FIELDLENGTH",typeof(int));
            fieldTable.Columns.Add(dc);


            if (RCIS.Global.GlobalEditObject.GlobalWorkspace != null)
            {
                string pathName = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName;
                if (pathName.ToUpper().EndsWith(".GDB"))
                {
                    this.radioGroup1.SelectedIndex = 0;
                    this.beFgdb.Text = pathName;
                }
                else if (pathName.ToUpper().EndsWith(".MDB"))
                {
                    this.radioGroup1.SelectedIndex = 1;
                    this.bePgdb.Text = pathName;
                }
                this.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
                LoadDsTable();
                this.treeView1.ExpandAll();
            }

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.beFgdb.Enabled = true;
                this.bePgdb.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                this.beFgdb.Enabled = false;
                this.bePgdb.Enabled = true;
            }
        }


        public string SelFeatureClassName
        {
            get
            {
                if (this.treeView1.SelectedNode == null)
                    return "";
                string tag = this.treeView1.SelectedNode.Tag.ToString().ToUpper();
                if (!tag.StartsWith("C"))
                {
                    return "";
                }
                return OtherHelper.GetRightName(tag);
            }
        }

        private int getIdxByGeoType(IFeatureClass pFS)
        {
            if (pFS.ShapeType == esriGeometryType.esriGeometryPoint)
                return 1;
            else if (pFS.ShapeType == esriGeometryType.esriGeometryPolyline)
                return 2;
            else if (pFS.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                return 3;
            }
            else return 5;
        }

        private void LoadDsTable()
        {
            //加载所有数据集和要素类
            if (this.currWs == null)
                return;
            this.treeView1.Nodes.Clear();

            IEnumDataset pEnumDs = this.currWs.get_Datasets(esriDatasetType.esriDTAny);
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                if (pDs.Type == esriDatasetType.esriDTFeatureClass)
                {
                    TreeNode aNode=new TreeNode();
                    aNode.Text=(pDs as IFeatureClass).AliasName;
                    
                    aNode.Tag="C|"+pDs.Name.ToUpper(); //D 表示 数据集，C表示要素类
                    int imgidx = getIdxByGeoType(pDs as IFeatureClass);
                    aNode.ImageIndex = imgidx;
                    aNode.SelectedImageIndex = imgidx;
                    this.treeView1.Nodes.Add(aNode);

                }
                else if (pDs.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    //先加载 一个节点
                    TreeNode parentNode = new TreeNode();
                    parentNode.Text = pDs.Name.ToUpper();
                    parentNode.Tag ="D|"+ pDs.Name.ToUpper();
                    parentNode.ImageIndex = 0;
                    this.treeView1.Nodes.Add(parentNode);

                    //提取出其内的FeatureClass:
                    IFeatureWorkspace fws = currWs as IFeatureWorkspace;
                    IFeatureDataset Feats = pDs as IFeatureDataset;
                    IFeatureClassContainer FeatCon = (IFeatureClassContainer)Feats;
                    IEnumFeatureClass featClasses = FeatCon.Classes;

                    featClasses.Reset();
                    IFeatureClass myFeatClass = featClasses.Next();
                    while (myFeatClass != null)
                    {

                        TreeNode aNode = new TreeNode();
                        aNode.Text = myFeatClass.AliasName;
                        int imgidx = getIdxByGeoType(myFeatClass );

                        aNode.Tag = "C|"+(myFeatClass as IDataset).Name.ToUpper();
                        aNode.ImageIndex = imgidx;
                        aNode.SelectedImageIndex = imgidx;
                        parentNode.Nodes.Add(aNode);
                        
                        myFeatClass = featClasses.Next();
                    } //while(...
                }

                pDs = pEnumDs.Next();

            }

        }

        private void bePgdb_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PGDB文件|*.MDB";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.bePgdb.Text = dlg.FileName;
            currWs = WorkspaceHelper2.GetAccessWorkspace(dlg.FileName);
            if (currWs == null)
            {
                MessageBox.Show("工作空间打开失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LoadDsTable();
            this.treeView1.ExpandAll();
        }

        DataTable fieldTable = null;

        private void beFgdb_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beFgdb.Text = dlg.SelectedPath;
            currWs = WorkspaceHelper2.GetFileGdbWorkspace(dlg.SelectedPath);
            if (currWs == null)
            {
                MessageBox.Show("工作空间打开失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LoadDsTable();
            this.treeView1.ExpandAll();


        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.currWs == null)
                return;
            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (tag.StartsWith("D|"))
                return;

            try
            {
                string dsName = OtherHelper.GetRightName(tag);

                IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(dsName);

                RefreshFieldTab(pFC);
            }
            catch (Exception ex)
            {
            }

        }
        private void RefreshFieldTab(IFeatureClass pFC)
        {
            //显示所有字段
            this.fieldTable.Rows.Clear();

            for (int i = 0; i < pFC.Fields.FieldCount; i++)
            {
                IField pFld = pFC.Fields.get_Field(i);
                DataRow newRow = this.fieldTable.NewRow();
                newRow["FIELDNAME"] = pFld.Name.ToUpper();
                newRow["FIELDALIAS"] = pFld.AliasName;

                switch (pFld.Type)
                {
                    case esriFieldType.esriFieldTypeOID:
                        newRow["FIELDTYPE"] = "标识码";
                        break;


                    case esriFieldType.esriFieldTypeGeometry:
                        newRow["FIELDTYPE"] = "图形字段";
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                    case esriFieldType.esriFieldTypeSingle:
                        newRow["FIELDTYPE"] = "浮点型";
                        newRow["FIELDLENGTH"] = pFld.Precision;
                        break;
                    case esriFieldType.esriFieldTypeSmallInteger:
                    case esriFieldType.esriFieldTypeInteger:
                        newRow["FIELDTYPE"] = "整型";
                        newRow["FIELDLENGTH"] = pFld.Length;
                        break;

                    case esriFieldType.esriFieldTypeDate:
                        newRow["FIELDTYPE"] = "日期型";
                        break;
                    case esriFieldType.esriFieldTypeString:
                        newRow["FIELDTYPE"] = "字符型";
                        newRow["FIELDLENGTH"] = pFld.Length;
                        break;

                }

                this.fieldTable.Rows.Add(newRow);
            }


            this.gridControl1.DataSource = this.fieldTable;
        }

       


        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (this.SelFeatureClassName == "")
                return;
            if (this.gridView1.SelectedRowsCount == 0)
                return;
            string fieldName = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "FIELDNAME").ToString();
            string fieldType = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "FIELDTYPE").ToString();
            if ((fieldName.ToUpper().StartsWith("SHAPE"))
                || (fieldName.ToUpper().StartsWith("SHP"))
                || (fieldType == "标识码") || (fieldType=="图形字段")
                )
            {
                MessageBox.Show("该字段不允许删除!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("确认要删除该要字段么，不可恢复\r\n是否继续(是 或者 否)", "询问",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
                return;
            try
            {
                IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(SelFeatureClassName);
                int fldidx = pFC.FindField(fieldName);
                if (fldidx > -1)
                {
                    IField pField = pFC.Fields.get_Field(fldidx);
                    pFC.DeleteField(pField);
                }

                this.gridView1.DeleteSelectedRows();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (this.SelFeatureClassName == "")
                return;
            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(SelFeatureClassName);
            NewFieldForm frm = new NewFieldForm();
            frm.sendClass = pFC;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RefreshFieldTab(pFC);
            }

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {


            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (!tag.StartsWith("C|"))
            {
                MessageBox.Show("请先选中某个目标要素类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            copyFeatureStructFrm frm = new copyFeatureStructFrm();
            frm.currWS = this.currWs;
            frm.srcFcName = OtherHelper.GetRightName(tag);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDsTable();
                this.treeView1.ExpandAll();
            }

        }

      

        private void btnCreateHistoryLayer_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (tag.StartsWith("C|"))
            {
                MessageBox.Show("请先选中某个数据集！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string dsName = OtherHelper.GetRightName(tag);
            IFeatureDataset pFeaDs = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(dsName);
            TDDC3D.edit.GxHistoryHelper.CreateHGXTable(pFeaDs);
            LoadDsTable();
            this.treeView1.ExpandAll();
        }

        private void 新建数据集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currWs == null)
                return;
            NewDatasetForm frm = new NewDatasetForm();
            frm.CurrWs = this.currWs;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string newdsName = frm.CurrNewDsName;

                TreeNode aNode = new TreeNode(newdsName);
                aNode.Tag = "D|" + newdsName;
                aNode.ImageIndex = 0;
                aNode.SelectedImageIndex = 0;
                this.treeView1.Nodes.Add(aNode);
            }
        }

        private void 删除数据集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currWs == null)
                return;
            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (tag.StartsWith("C|"))
                return;
            string dsName = OtherHelper.GetRightName(tag);

            if (MessageBox.Show("删除该数据集将会删除数据集下所有要素类，并不可恢复\r\n是否继续(是 或者 否)", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
                return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                IFeatureDataset pFds = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(dsName);
                IFeatureClassContainer pClassContainer = pFds as IFeatureClassContainer;
                for (int i = pClassContainer.ClassCount - 1; i >= 0; i--)
                {
                    IFeatureClass pClass = pClassContainer.get_Class(i);
                    if (pClass != null)
                    {
                        (pClass as IDataset).Delete();
                    }
                }
                pFds.Delete();

                this.treeView1.SelectedNode.Remove();
                MessageBox.Show("数据集删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, "删除数据集。");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void 新建要素类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currWs == null)
                return;

            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (tag.StartsWith("C|"))
            {
                MessageBox.Show("请先选中某个数据集！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string dsName = OtherHelper.GetRightName(tag);
            IFeatureDataset pFeaDs = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(dsName);
            NewFeatureClassForm frm = new NewFeatureClassForm();
            frm.currWS = this.currWs;
            frm.currDS = pFeaDs;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int jhlx = frm.retJHLX;
                TreeNode aNode = new TreeNode();
                aNode.Tag = "C|" + frm.retFCName;
                aNode.Text = frm.retFCAliasName;
                aNode.ImageIndex = jhlx + 1;
                aNode.SelectedImageIndex = jhlx + 1;
                this.treeView1.SelectedNode.Nodes.Add(aNode);
            }
        }

        private void 删除要素类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.currWs == null)
                return;

            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (!tag.StartsWith("C|"))
            {
                MessageBox.Show("请先选中某个要素类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("确认要删除该要素类么，不可恢复\r\n是否继续(是 或者 否)", "询问",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
                return;

            try
            {
                string fcName = OtherHelper.GetRightName(tag);
                IFeatureClass pClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(fcName);
                IDataset pDS = pClass as IDataset;
                if (pDS.CanDelete())
                {
                    pDS.Delete();
                    this.treeView1.SelectedNode.Remove();
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("不允许删除，可能未释放，请重启软件。");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void 升级至三调数据库标准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //升级至三调数据库标准
            if (this.treeView1.SelectedNode == null)
                return;

            if (SelFeatureClassName == "")
            {
                MessageBox.Show("请先选中某个目标要素类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(SelFeatureClassName);
            try
            {
                //所有bsm的重新修改
                int bsmIdx = pFC.FindField("BSM");
                if (bsmIdx > -1)
                {
                    //删除旧的，重建新的
                    pFC.DeleteField(pFC.Fields.get_Field(bsmIdx));
                    IField pField = new FieldClass();
                    IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                    pFldEdit.Name_2 = "BSM";
                    pFldEdit.AliasName_2 = "标识码";
                    pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pFldEdit.Length_2 = 18;
                    pFC.AddField(pField);

                }

                if (SelFeatureClassName.ToUpper() == "XZQJX")
                {
                    //修改BSM，增加一个bz
                    IField pField = new FieldClass();
                    IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                    pFldEdit.Name_2 = "BZ";
                    pFldEdit.AliasName_2 = "备注";
                    pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pFldEdit.Length_2 = 200;
                    pFC.AddField(pField);
                }
                else 
                if (SelFeatureClassName.ToUpper() == "DLTB")
                {
                    #region  //增加字段 XXTBKD，TBXHDM，TBXHMC，GDZZSXDM，GDZZSXMC，GDDB，FRDBS，CZCSXM，SJNF
                    string fldName = "XZDWKD";
                    string aliasName = "线状地物宽度";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        pFldEdit.Precision_2 = 5;
                        pFldEdit.Scale_2 = 1;
                        pFC.AddField(pField);
                    }
                    fldName = "TBXHDM";
                    aliasName = "图斑细化代码";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 4;
                        pFC.AddField(pField);
                    }
                    fldName = "TBXHMC";
                    aliasName = "图斑细化名称";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 20;
                        pFC.AddField(pField);
                    }
                    fldName = "ZZSXDM";
                    aliasName = "耕地种植属性代码";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 2;
                        pFC.AddField(pField);
                    }
                    fldName = "ZZSXMC";
                    aliasName = "耕地种植属性名称";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 10;
                        pFC.AddField(pField);
                    }
                    fldName = "GDDB";
                    aliasName = "耕地等别";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 2;
                        pFC.AddField(pField);
                    }
                    fldName = "FRDBS";
                    aliasName = "飞入地标识";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 1;
                        pFC.AddField(pField);
                    }
                    fldName = "CZCSXM";
                    aliasName = "城镇村属性码";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFldEdit.Length_2 = 4;
                        pFC.AddField(pField);
                    }
                    fldName = "SJNF";
                    aliasName = "数据年份";
                    if (pFC.FindField(fldName) == -1)
                    {
                        IField pField = new FieldClass();
                        IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                        pFldEdit.Name_2 = fldName;
                        pFldEdit.AliasName_2 = aliasName;
                        pFldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                        pFC.AddField(pField);
                    }

                    int fidx = pFC.FindField("XZDWMJ");
                    if (fidx > -1)
                    {
                        pFC.DeleteField(pFC.Fields.get_Field(fidx));
                    }
                    fidx = pFC.FindField("LXDWMJ");
                    if (fidx > -1)
                    {
                        pFC.DeleteField(pFC.Fields.get_Field(fidx));
                    }

                    bool b= DatabaseHelper.ModifyFieldName(pFC, "TKXS", "KCXS", "扣除系数");
                    b= DatabaseHelper.ModifyFieldName(pFC, "TKMJ", "KCMJ", "扣除面积");
                    //fldName = "TKXS";
                    //aliasName = "扣除系数";
                    //if (pFC.FindField(fldName) > -1)
                    //{
                    //    int idx=pFC.FindField(fldName);
                    //    IField pField = pFC.Fields.get_Field(idx);
                    //    IFieldEdit2 pFldEdit = pField as IFieldEdit2;
                    //    pFldEdit.Name_2 = "KCXS";
                    //    pFldEdit.AliasName_2 = aliasName;


                    //}
                    #endregion 
                }
                MessageBox.Show("升级成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            string name = ""; string alias = "";
            string fcName = OtherHelper.GetRightName(tag);
            if (tag.StartsWith("C|"))
            {
                //数据集
                IFeatureClass pClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(fcName);
                IDataset pDS = pClass as IDataset;
                name = pDS.Name;
                alias = pClass.AliasName;
            }
            else if (tag.StartsWith("D|"))
            {
                IFeatureDataset pDS=(this.currWs as IFeatureWorkspace).OpenFeatureDataset(fcName);
                name=pDS.Name;
                alias=pDS.Name;
            }
            RenameFeatureclassFrm frm=new RenameFeatureclassFrm();
            frm.RetAliasName=alias;
            frm.RetClassName=name;
            if (frm.ShowDialog()==DialogResult.OK)
            {
                if (tag.StartsWith("C|"))
                {
                    //数据集
                    IFeatureClass pClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(fcName);
                    if (RCIS.GISCommon.EsriDatabaseHelper.RenameFeatureClassName(frm.RetClassName, frm.RetAliasName, pClass))
                    {
                        MessageBox.Show("重命名成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //LoadDsTable();
                        this.treeView1.SelectedNode.Text = frm.RetAliasName;
                        this.treeView1.SelectedNode.Tag = "C|" + frm.RetClassName;
                    }
                    else
                    {
                        MessageBox.Show("重命名失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (tag.StartsWith("D|"))
                {
                    IFeatureDataset pDS = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(fcName);
                    name = pDS.Name;
                    alias = pDS.Name;
                    if ( RCIS.GISCommon.EsriDatabaseHelper.RenameDataset(frm.RetClassName,  pDS))
                    {
                        MessageBox.Show("重命名成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //LoadDsTable();
                        this.treeView1.SelectedNode.Text = frm.RetAliasName;
                        this.treeView1.SelectedNode.Tag = "D|" + frm.RetClassName;

                    }
                    else
                    {
                        MessageBox.Show("重命名失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.currWs == null)
                return;
            IWorkspace2 currWS2 = this.currWs as IWorkspace2;
            if (currWS2.get_NameExists(esriDatasetType.esriDTFeatureClass, "TFH"))
            {
                MessageBox.Show("该要素类已存在！","提示",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (tag.StartsWith("C|"))
            {
                MessageBox.Show("请先选中某个数据集！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string dsName = OtherHelper.GetRightName(tag);
                IFeatureDataset pFeaDs = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(dsName);

                IFields pFields = new FieldsClass();
                IFieldsEdit tFieldsEdit = (IFieldsEdit)pFields;

                //创建几何对象字段定义
                IGeometryDef tGeometryDef = new GeometryDefClass();
                IGeometryDefEdit tGeometryDefEdit = tGeometryDef as IGeometryDefEdit;
                //指定几何对象字段属性值

                tGeometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                tGeometryDefEdit.GridCount_2 = 1;
                tGeometryDefEdit.set_GridSize(0, 1000);
                tGeometryDefEdit.SpatialReference_2 = (pFeaDs as IGeoDataset).SpatialReference;

                //创建OID字段
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = fieldOID as IFieldEdit;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                tFieldsEdit.AddField(fieldOID);

                //创建几何字段
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = fieldShape as IFieldEdit;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = tGeometryDef;
                tFieldsEdit.AddField(fieldShape);
                IField field = new FieldClass();
                IFieldEdit fldEdt = field as IFieldEdit;
                fldEdt.Name_2 = "TFH";
                fldEdt.AliasName_2 = "图幅号";
                fldEdt.Type_2 = esriFieldType.esriFieldTypeString;
                fldEdt.Length_2 = 50;
                tFieldsEdit.AddField(field);
                field = new FieldClass();
                fldEdt = field as IFieldEdit;
                fldEdt.Name_2 = "OLDTFH";
                fldEdt.AliasName_2 = "旧图幅号";
                fldEdt.Type_2 = esriFieldType.esriFieldTypeString;
                fldEdt.Length_2 = 50;
                tFieldsEdit.AddField(field);

                field = new FieldClass();
                IFieldEdit fieldEdit = field as IFieldEdit;
                fieldEdit.Name_2 = "JSMJ";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                fieldEdit.AliasName_2 = "计算面积";
                tFieldsEdit.AddField(field);

                IFeatureClass tFeatureClass = pFeaDs.CreateFeatureClass("TFH",
                        pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                LoadDsTable();
                this.treeView1.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //复制要素类


            if (this.treeView1.SelectedNode == null)
                return;
            string tag = this.treeView1.SelectedNode.Tag.ToString();
            if (!tag.StartsWith("C|"))
            {
                MessageBox.Show("请先选中某个目标要素类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CopyFeatureClassFrm frm = new CopyFeatureClassFrm();
            frm.currWS = this.currWs;
            frm.srcFcName = OtherHelper.GetRightName(tag);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDsTable();
                this.treeView1.ExpandAll();
            }

        }

        private void btnUpgradeDB_Click(object sender, EventArgs e)
        {

            if (currWs == null)
            {
                MessageBox.Show("请打开有效数据库！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IWorkspace2 pwork = currWs as IWorkspace2;


            ISpatialReference pSpatialRef = null;
            IEnumDataset pEnumDs = this.currWs.get_Datasets(esriDatasetType.esriDTAny);
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                if (pDs.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    if (pDs.Name.ToLower() == "tddc")
                    {
                        //IFeatureClass pFC = pDs as IFeatureClass;
                        IGeoDataset pGeoDataset = pDs as IGeoDataset;
                        pSpatialRef = pGeoDataset.SpatialReference;
                    }
                    if (pDs.Name.ToLower() == "gdzy")
                    {
                        MessageBox.Show("已是最新数据库！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                pDs = pEnumDs.Next();
            }
            if (pSpatialRef == null)
            {
                MessageBox.Show("待升级数据库不是标准数据库！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("数据库正在升级，请稍后。", "提示");
            wait.Show();
            string sourceDir = RCIS.Global.AppParameters.ConfPath + @"\standard.gdb";
            IWorkspace pSourWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(sourceDir);
            IEnumDataset pEnumDataset = pSourWorkspace.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    Boolean b = pwork.NameExists[esriDatasetType.esriDTFeatureDataset, pDataset.Name];
                    if (!b)
                    {
                        IFeatureDataset pTDDCFeatureDataset = (currWs as IFeatureWorkspace).CreateFeatureDataset(pDataset.Name, pSpatialRef);
                        IFeatureClassContainer pFCC = pDataset as IFeatureClassContainer;
                        IEnumFeatureClass pEnumFC = pFCC.Classes;
                        pEnumFC.Reset();
                        IFeatureClass pFeatureClass = pEnumFC.Next();
                        while (pFeatureClass != null)
                        {
                            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(pSourWorkspace, currWs, (pFeatureClass as IDataset).Name, (pFeatureClass as IDataset).Name, pTDDCFeatureDataset, null);
                            IClassSchemaEdit2 pClassSchemaEdit2 = (currWs as IFeatureWorkspace).OpenFeatureClass((pFeatureClass as IDataset).Name) as IClassSchemaEdit2;
                            pClassSchemaEdit2.AlterAliasName(pFeatureClass.AliasName);
                            pFeatureClass = pEnumFC.Next();
                        }
                    }
                }
                
                pDataset = pEnumDataset.Next();
            }
            wait.Close();
            MessageBox.Show("数据库升级完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddFields_Click(object sender, EventArgs e)
        {
            if (this.SelFeatureClassName == "")
                return;
            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(SelFeatureClassName);
            gengxin.FrmAddFields frm = new gengxin.FrmAddFields();
            frm.pFeaClass = pFC;
            frm.pCurrentWorkspace = currWs;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                RefreshFieldTab(pFC);
            }
        }
    }
}
