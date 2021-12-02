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
using ESRI.ArcGIS.Geoprocessor;

namespace TDDC3D.output
{
    public partial class FrmOutRsTable : Form
    {
        public FrmOutRsTable()
        {
            InitializeComponent();
        }
        public IMap pMap = null;
        IFeatureClass pShpClass = null;
        IFeatureClass pTbClass = null;
        private void txtShp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openfiledia = new OpenFileDialog();
            openfiledia.Filter = "SHP格式数据（*.shp）|*.shp";
            if (openfiledia.ShowDialog() == DialogResult.OK)
            {
                pShpClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(openfiledia.FileName);

                string erroField = "";

                List<string> fieldArr = new List<string>() { "XZQDM","JCBH","QSX","HSX","JCMJ" };
                for (int i = 0; i < fieldArr.Count; i++)
                {
                    if (pShpClass.FindField(fieldArr[i]) == -1)
                        erroField += fieldArr[i] + ",";
                }

                if (erroField.Length > 0)
                {
                    MessageBox.Show("选择数据缺失"+erroField+"必要字段","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    pShpClass = null;
                    txtShp.Text = "";
                }
                else
                    txtShp.Text = openfiledia.FileName;
            }
        }

        private void FrmOutRsTable_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(cmbLay, pMap);
        }

        private void txtPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = folder.SelectedPath;
            }
        }

        private void cmbLay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cmbLay.SelectedItem.ToString()))
            {
                string className = cmbLay.SelectedItem.ToString();
                className = className.Substring(0, className.IndexOf("|"));
                pTbClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass(className);
                if (pTbClass.FindField("DLBM") == -1)
                {
                    pTbClass = null;
                    cmbLay.Text = "";
                }
            }
            
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            if (pTbClass == null || pShpClass == null)
            {
                MessageBox.Show("请正确选择数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("请正确选择输出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string exportFile = txtPath.Text + "\\遥感监测图斑信息核实记录表.mdb";
            if (System.IO.File.Exists(exportFile))
            {
                MessageBox.Show("文件已经存在，请更换存储位置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在计算内外业图斑关系……", "提示");
            wait.Show();
            System.IO.File.Copy(Application.StartupPath + @"\SystemConf\遥感监测信息表.mdb", exportFile);
            IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
            string tbPath = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "//" + pTbClass.FeatureDataset.Name + "//" + (pTbClass as IDataset).Name;
            string fieldArr = "FID;XZQDM;JCBH;TBLX;QSX;HSX;JCMJ";
            string Arr =  "DLBM" ;

            Boolean b = GP_TabulateIntersection(txtShp.Text, fieldArr, tbPath, Arr, pTmpWs.PathName + "\\RsTable");
            if (!b)
            {
                MessageBox.Show("计算错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<string> fidArr = new List<string>();
            ITable pTab = (pTmpWs as IFeatureWorkspace).OpenTable("RsTable");
            ICursor pCursor = pTab.Search(null,true);
            IRow pRow;
            while((pRow=pCursor.NextRow())!=null)
            {
                double mj = double.Parse(pRow.get_Value(pRow.Fields.FindField("Area")).ToString());
                double bl = double.Parse(pRow.get_Value(pRow.Fields.FindField("PERCENTAGE")).ToString());
                if (!(mj > double.Parse(txtS.Text) || bl > double.Parse(txtP.Text))) continue;
                string id = (double.Parse(pRow.get_Value(pRow.Fields.FindField("FID")).ToString())+1).ToString();
                string xzdm = pRow.get_Value(pRow.Fields.FindField("XZQDM")).ToString();
                string tbh = pRow.get_Value(pRow.Fields.FindField("JCBH")).ToString();
                string tblx = pRow.get_Value(pRow.Fields.FindField("TBLX")).ToString();
                string qsx = pRow.get_Value(pRow.Fields.FindField("QSX")).ToString();
                string hsx = pRow.get_Value(pRow.Fields.FindField("HSX")).ToString();
                double jcmj = double.Parse(pRow.get_Value(pRow.Fields.FindField("JCMJ")).ToString());
                string dlbm = pRow.get_Value(pRow.Fields.FindField("DLBM")).ToString().Trim();
                string isJZ = "是";
                if (string.IsNullOrWhiteSpace(dlbm))
                    isJZ = "否";
                if (!fidArr.Contains(id))
                {
                    string sql = "insert into 遥感监测图斑信息核实记录表 values("+id+","+xzdm+",'"+tbh+"','"+tblx+"','"+qsx+"','"+hsx+"',"+jcmj+",'"+dlbm+"','','"+isJZ+"','')";
                    int m = RCIS.Database.LS_ResultMDBHelper.ExecuteSQL(sql, exportFile);
                    fidArr.Add(id);
                }
                else
                {
                    string sql = "select 是否变更 from 遥感监测图斑信息核实记录表 where 序号="+id+"";
                    DataTable dt = RCIS.Database.LS_ResultMDBHelper.GetDataTable(sql, "遥感监测图斑信息核实记录表", exportFile);
                    string val = dt.Rows[0][0].ToString();
                    if (!val.Contains(dlbm))
                        val += "/"+dlbm+"";
                    sql = "update 遥感监测图斑信息核实记录表 set 是否变更='"+val+"',是否举证='是' where 序号="+id+"";
                    int m = RCIS.Database.LS_ResultMDBHelper.ExecuteSQL(sql, exportFile);
                }
            }

            for (int i = 0; i < pShpClass.FeatureCount(null); i++)
            {
                if (!fidArr.Contains((i + 1).ToString()))
                {
                    IFeature pFea = pShpClass.GetFeature(i);
                    string xzdm = pFea.get_Value(pFea.Fields.FindField("XZQDM")).ToString();
                    string tbh = pFea.get_Value(pFea.Fields.FindField("JCBH")).ToString();
                    string tblx = pFea.get_Value(pFea.Fields.FindField("TBLX")).ToString();
                    string qsx = pFea.get_Value(pFea.Fields.FindField("QSX")).ToString();
                    string hsx = pFea.get_Value(pFea.Fields.FindField("HSX")).ToString();
                    double jcmj = double.Parse(pFea.get_Value(pFea.Fields.FindField("JCMJ")).ToString());
                    string sql = "insert into 遥感监测图斑信息核实记录表 values(" + (i+1) + "," + xzdm + ",'" + tbh + "','" + tblx + "','" + qsx + "','" + hsx + "'," + jcmj + ",'否','','否','')";
                    int m = RCIS.Database.LS_ResultMDBHelper.ExecuteSQL(sql, exportFile);
                    fidArr.Add((i+1).ToString());
                }
            }

            wait.Close();
            MessageBox.Show("输出完毕","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            
        }

        private Boolean GP_TabulateIntersection(string ZoneFeatures, string ZoneField, string ClassFeatures, string ClassField, string outTable)
        {
            Geoprocessor geoprocessor = new Geoprocessor();
            geoprocessor.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.TabulateIntersection TabInter = new ESRI.ArcGIS.AnalysisTools.TabulateIntersection();

            TabInter.in_zone_features = ZoneFeatures;
            TabInter.zone_fields = ZoneField;
            TabInter.in_class_features = ClassFeatures;
            TabInter.class_fields = ClassField;
            TabInter.out_table = outTable;
            try
            {
                geoprocessor.Execute(TabInter, null);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
