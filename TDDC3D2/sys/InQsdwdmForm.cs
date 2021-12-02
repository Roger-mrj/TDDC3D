using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Database;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.sys
{
    public partial class InQsdwdmForm : Form
    {
        public InQsdwdmForm()
        {
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IWorkspace currWs = null;

        private void simpleButton1_Click(object sender, EventArgs e)
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

            if (srcTable!=null)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    //导入到 数据库中
                    IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
                    ITable pTable = null;
                    try
                    {
                        pTable = pFeaWs.OpenTable("QSDWDMB");
                    }

                    catch { }
                    if (pTable == null)
                    {
                        MessageBox.Show("数据库中找不到权属单位代码表！");
                        return;
                    }
                    foreach (DataRow dr in srcTable.Rows)
                    {
                        IRow aRow = pTable.CreateRow();
                        FeatureHelper.SetRowValue(aRow, "QSDWDM", dr[0].ToString());
                        FeatureHelper.SetRowValue(aRow, "QSDWMC", dr[1].ToString());
                        int len = dr[0].ToString().Trim().Length;
                        FeatureHelper.SetRowValue(aRow, "JB", len);
                        aRow.Store();
                    }
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message);
                }
               
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            dlg.FileName = "数据";
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
        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }

                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];
                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }

                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void InQsdwdmForm_Load(object sender, EventArgs e)
        {
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            ITable pTable = null;
            try
            {
                pTable = pFeaWs.OpenTable("QSDWDMB");
            }

            catch { }
            if (pTable == null)
            {
                if (MessageBox.Show("数据库中找不到权属单位代码表,是否创建一个空表？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
                    return;

                //创建一个表
                //建立字段集合:
                IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                //建立字段:ObjectID
                IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "OBJECTID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldsEdit.AddField(pField);

                 pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "QSDWDM";
                pFieldEdit.Length_2 = 100;
                pFieldEdit.AliasName_2 = "权属单位代码";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;   //缺省
                pFieldsEdit.AddField(pField);

                pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "QSDWMC";
                pFieldEdit.Length_2 = 200;
                pFieldEdit.AliasName_2 = "权属单位名称";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;   //缺省
                pFieldsEdit.AddField(pField);

                pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "JB";
                pFieldEdit.AliasName_2 = "级别";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;   //缺省
                pFieldsEdit.AddField(pField);

                pTable = pFeaWs.CreateTable("QSDWDMB", pFields, null, null, "");

            }
            DataTable dt = this.ToDataTable(pTable);
            this.gridControl1.DataSource = dt;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确实要清空数据么，不可恢复？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            try
            {
                string sql="delete from QSDWDMB ";
                this.currWs.ExecuteSQL(sql);
                this.gridControl1.DataSource=null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

            if (this.currWs == null) return;
            this.Cursor = Cursors.WaitCursor;

            IFeatureClass XZQClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            IFeatureClass CjdcqClass=(this.currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQ");  //村
           
            //List<string> lstXian = new List<string>();
            //List<string> lstXiang = new List<string>(); //记录已存在的乡
            //List<string> lstCun = new List<string>(); //记录 已存在的村

            Dictionary<string, string> dicXzq = new Dictionary<string, string>();// 记录行政区代码名称

            DataTable datable = new DataTable();
            DataColumn dc = new DataColumn("DM",typeof(string));
            datable.Columns.Add(dc);
            dc=new DataColumn("MC",typeof(string));
            datable.Columns.Add(dc);

            //2019-3-1修改，xzq和tdqsq结构变化
            //从土地权属区 读取村级代码
            IFeatureCursor cjdcqCursor = null;
            IFeature aQszFea = null;
            cjdcqCursor = CjdcqClass.Search(null, true);
            try
            {
                while ((aQszFea = cjdcqCursor.NextFeature()) != null)
                {
                    string dm = FeatureHelper.GetFeatureStringValue(aQszFea, "ZLDWDM");  
                    string mc = FeatureHelper.GetFeatureStringValue(aQszFea, "ZLDWMC");
                    if (dm.Length > 12)
                    {
                        dm = dm.Substring(0, 12);
                    }

                    if (!dicXzq.ContainsKey(dm))
                    {
                        dicXzq.Add(dm, mc);
                    }

                    //把县级的加上
                    string xianDm = dm.Substring(0, 6);
                    if (!dicXzq.ContainsKey(xianDm))
                    {
                        dicXzq.Add(xianDm,"全县");
                    }

                }
            }
            catch (Exception ex)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cjdcqCursor);
            }

            

            //从行政区图层自动生成，都是乡
            IFeatureCursor pFeaCursor = null;
            IFeature aCunFea = null;
            pFeaCursor = XZQClass.Search(null, true);
            try
            {
                
                while ((aCunFea = pFeaCursor.NextFeature()) != null)
                {
                    string dm = FeatureHelper.GetFeatureStringValue(aCunFea, "XZQDM");  //行政区只有乡
                    string mc = FeatureHelper.GetFeatureStringValue(aCunFea, "XZQMC");
                    
                    //乡级代码和名称
                    if (!dicXzq.ContainsKey(dm))
                    {
                        dicXzq.Add(dm, mc);
                    }
                }              

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
            }


            //加入到 datagrid
            foreach (KeyValuePair<string, string> aItem in dicXzq)
            {
                DataRow aRow = datable.NewRow();
                aRow["DM"] = aItem.Key.ToString();
                aRow["MC"] = aItem.Value;
                datable.Rows.Add(aRow);
            }            
            
            //绑定 grid
            this.gridView1.Columns.Clear();
            this.gridControl1.DataSource = datable;


            
            try
            {
                //导入到 数据库中
                IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
                ITable pTable = null;
                try
                {
                    pTable = pFeaWs.OpenTable("QSDWDMB");
                }

                catch { }
                if (pTable == null)
                {
                    MessageBox.Show("数据库中找不到权属单位代码表！");
                    return;
                }
                foreach (DataRow dr in datable.Rows)
                {
                    IRow aRow = pTable.CreateRow();
                    FeatureHelper.SetRowValue(aRow, "QSDWDM", dr[0].ToString());
                    FeatureHelper.SetRowValue(aRow, "QSDWMC", dr[1].ToString());
                    int len = dr[0].ToString().Trim().Length;
                    FeatureHelper.SetRowValue(aRow, "JB", len);
                    aRow.Store();
                }
                this.Cursor = Cursors.Default;
                MessageBox.Show("导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

            
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }
    }
}
