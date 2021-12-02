using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.Database;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Data;
using System.Windows.Forms;

namespace TDDC3D.edit
{
    public partial class DLChangeForm : Form
    {
        public DLChangeForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        IFeatureLayer currLayer = null;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DLChangeForm_Load(object sender, EventArgs e)
        {
            
            LayerHelper.LoadLayer2Combox(this.cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);

            int idx1 = -1;
            for (int i = 0; i < this.cmbLayer.Properties.Items.Count; i++)
            {
                string name = OtherHelper.GetLeftName( this.cmbLayer.Properties.Items[i].ToString().Trim().ToUpper());
                if (name.Contains("DLTB"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbLayer.SelectedIndex = idx1;


            System.Data.DataTable dt= RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_DLBM_CHANGE", "dlzh");
            this.gridControl1.DataSource = dt;
        }

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //找到对应字段
            string className =OtherHelper.GetLeftName( this.cmbLayer.Text.Trim());
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            IFeatureClass pFeaClass = currLayer.FeatureClass;


            int dlbmIdx = 0;

            this.cmbField1.Properties.Items.Clear();
            this.cmbField2.Properties.Items.Clear();
            for (int i = 0; i < pFeaClass.Fields.FieldCount; i++)
            {
                IField aFld = pFeaClass.Fields.get_Field(i);
                if (aFld.Type == esriFieldType.esriFieldTypeString)
                {
                    this.cmbField1.Properties.Items.Add(aFld.Name.ToUpper()+"|"+aFld.AliasName);
                    this.cmbField2.Properties.Items.Add(aFld.Name.ToUpper()+"|"+aFld.AliasName);
                }
                if (aFld.Name.Trim() == "DLBM")
                {
                    dlbmIdx = i;
                }
            }

            if (this.cmbField1.Properties.Items.Count > 0)
            {
                this.cmbField1.SelectedIndex = dlbmIdx;
                this.cmbField2.SelectedIndex = dlbmIdx;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //开始转换
            if (this.currLayer == null)
                return;
            if (this.cmbField1.Text.Trim() == "" || this.cmbField2.Text.Trim() == "")
                return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                IFeatureClass pFC = currLayer.FeatureClass;
                string tableName = (pFC as IDataset).Name.ToUpper();
                string field1 =OtherHelper.GetLeftName( this.cmbField1.Text.Trim().ToUpper());
                string field2 =OtherHelper.GetLeftName( this.cmbField2.Text.Trim().ToUpper());
                DataTable dtRule = (DataTable)this.gridControl1.DataSource;

                foreach (DataRow aRule in dtRule.Rows)
                {
                    string olddlbm = aRule["二调地类编码"].ToString();
                    string newdlbm = aRule["工作分类编码"].ToString();

                   
                    
                    string sql = "update " + tableName + " set " + field2 + " ='" + newdlbm + "' where " + field1 + "='" + olddlbm + "'";
                    RCIS.Global.GlobalEditObject.GlobalWorkspace.ExecuteSQL(sql);
                }
                this.Cursor = Cursors.Default;
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

            

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            dlg.FileName = "转换规则";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                this.gridControl1.ExportToXls(dlg.FileName);
                MessageBox.Show("导出完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel文件|*.xls|CVS文件|*.cvs";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            string srcFile = dlg.FileName;
            //获取该Excel数据集
            DataTable srcTable = null;
            if (dlg.FileName.ToUpper().EndsWith("XLS"))
            {
                string sheetName = LS_DBExcelHelper.getFirstsheetName(srcFile);
                srcTable = LS_DBExcelHelper.Excel2Datatable(srcFile, sheetName);


                this.gridView1.Columns.Clear();
                this.gridControl1.DataSource = srcTable;
            }
            else if (dlg.FileName.ToUpper().EndsWith("CVS"))
            {
                try
                {
                    srcTable = LS_DBExcelHelper.CSV2Datatable(dlg.FileName);
                    this.gridView1.Columns.Clear();
                    this.gridControl1.DataSource = srcTable;
                }
                catch { }



            }

            MessageBox.Show("导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //if (srcTable != null)
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    try
            //    {
                    
            //        foreach (DataRow dr in srcTable.Rows)
            //        {
                       

            //        }
            //        this.Cursor = Cursors.Default;
            //        MessageBox.Show("导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch (Exception ex)
            //    {
            //        this.Cursor = Cursors.Default;
            //        MessageBox.Show(ex.Message);
            //    }

            //}
        }
    }
}
