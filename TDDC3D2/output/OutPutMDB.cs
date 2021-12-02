using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using RCIS.GISCommon;
using RCIS.Utility;
using RCIS.Database;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TDDC3D.output
{
    public partial class OutPutMDB : Form
    {
        public OutPutMDB()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSHP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openfiledia = new OpenFileDialog();
            openfiledia.Filter = "SHP格式数据（*.shp）|*.shp";
            if (openfiledia.ShowDialog() == DialogResult.OK)
            {
                int iTBBH = -1;
                int fieldindex = 0;
                IFeatureClass pFClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(openfiledia.FileName);
                txtSHP.Text = openfiledia.FileName;
                cboField.Properties.Items.Clear();

                IFields pFields = pFClass.Fields;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    IField pField = pFields.Field[i];
                    if (pField.Type != esriFieldType.esriFieldTypeBlob && pField.Type != esriFieldType.esriFieldTypeDate && pField.Type != esriFieldType.esriFieldTypeGeometry && pField.Type != esriFieldType.esriFieldTypeOID)
                    {
                        cboField.Properties.Items.Add(pField.Name);
                        if (pField.Name.ToLower() == "tbbh") iTBBH = fieldindex;
                        fieldindex++;
                    }
                }
                if (cboField.Properties.Items.Count > 0)
                {
                    if (iTBBH == -1)
                        cboField.SelectedIndex = 0;
                    else
                        cboField.SelectedIndex = iTBBH;
                }
            }
        }

        private void txtExportPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtExportPath.Text = folder.SelectedPath;
            }
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

        private Boolean GP_TabulateIntersection(IFeatureClass ZoneFeatures, string ZoneField, string ClassFeatures, string ClassField, string outTable)
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSHP.Text) || string.IsNullOrWhiteSpace(txtExportPath.Text) || string.IsNullOrWhiteSpace(txtS.Text) || string.IsNullOrWhiteSpace(cboField.Text))
            {
                MessageBox.Show("请先设置参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string exportFile = txtExportPath.Text + "\\举证图斑信息表.mdb";
            if (System.IO.File.Exists(exportFile))
            {
                MessageBox.Show("文件已经存在，请更换存储位置！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在计算内外业图斑关系……", "提示");
            wait.Show();
            IFeatureClass pFeatureClass = null;
            RCIS.GISCommon.GpToolHelper.RepairGeometry(RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\TDDC\\DLTB");

            if(checkEdit1.Checked)
                pFeatureClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("dltb");
            if (checkEdit2.Checked)
            {
                IWorkspace pTmpWS = DeleteAndNewTmpGDB();
                RCIS.GISCommon.GpToolHelper.Update(RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\TDDC\\DLTB", RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\TDGX\\DLTBGX", pTmpWS.PathName + "\\nmDLTB");
                RCIS.GISCommon.GpToolHelper.RepairGeometry(pTmpWS.PathName + "\\nmDLTB");
                pFeatureClass = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("nmDLTB");
            }
            string outTable = System.IO.Path.GetDirectoryName(txtSHP.Text) + "\\NWY.dbf";
            Boolean b = GP_TabulateIntersection(txtSHP.Text, cboField.Text, RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\" + RCIS.Global.AppParameters.DATASET_DEFAULT_NAME + "\\" + "dltb", "BSM", outTable);
            if (!b) 
            {
                MessageBox.Show("计算错误。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            System.IO.File.Copy(Application.StartupPath + @"\SystemConf\举证图斑信息表.mdb", exportFile);
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, true);
            IFeature pFeature = pFeatureCursor.NextFeature();
            if (pFeature != null)
            {
                string zldwdm = FeatureHelper.GetFeatureStringValue(pFeature, "zldwdm");
                if (string.IsNullOrWhiteSpace(zldwdm) || zldwdm.Length <= 6)
                {
                    MessageBox.Show("图斑中坐落单位代码填写错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string dwdm = zldwdm.Substring(0, 6);
                DataRow dr1 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + dwdm.Substring(0, 2) + @"0000""", "tmp");
                DataRow dr2 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + dwdm.Substring(0, 4) + @"00""", "tmp");
                DataRow dr3 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + dwdm + @"""", "tmp");
                string sheng = dr1 == null ? "" : dr1[1].ToString();
                string shi = dr2 == null ? "" : dr2[1].ToString();
                string xian = dr3 == null ? "" : dr3[1].ToString();
                long i = 1;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                //IQueryFilterDefinition2 pQueryFilterDef = pQueryFilter as IQueryFilterDefinition2;
                //pQueryFilterDef.PostfixClause = "Order By BSM";
                IWorkspace pWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(outTable);
                ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable(System.IO.Path.GetFileName(outTable));
                using (ESRI.ArcGIS.ADF.ComReleaser comReleaser = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    ICursor pCursor = pTable.Search(pQueryFilter, true);
                    comReleaser.ManageLifetime(pCursor);
                    //string oldBSM = "";
                    string sBSM = "";
                    string sTBBH = "";
                    List<string> bsms = new List<string>();
                    IRow pRow;
                    string sql;
                    while ((pRow = pCursor.NextRow()) != null)
                    {
                        wait.SetCaption("提示：正在计算" + pRow.OID.ToString());
                        double mj = double.Parse(pRow.get_Value(pRow.Fields.FindField("Area")).ToString());
                        double bl = double.Parse(pRow.get_Value(pRow.Fields.FindField("PERCENTAGE")).ToString());
                        if (!(mj > double.Parse(txtS.Text) || bl > double.Parse(txtP.Text))) continue;
                        Application.DoEvents();
                        sBSM = pRow.get_Value(pRow.Fields.FindField("BSM")).ToString();
                        sTBBH = pRow.get_Value(pRow.Fields.FindField(cboField.Text)).ToString();
                        //if (oldBSM != pRow.get_Value(pRow.Fields.FindField("BSM")).ToString())
                        if (bsms.Contains(sBSM))
                        {
                            //if (oldBSM != "" && sTBBH != "")
                            //{
                            sql = string.Format("Update 举证图斑信息表 Set 举证图斑预编号 = 举证图斑预编号 + '/{1}' Where 对应三调图斑标识码 = '{0}'", sBSM, sTBBH);
                            int m = LS_ResultMDBHelper.ExecuteSQL(sql, exportFile);
                            //}
                            //sBSM = pRow.get_Value(pRow.Fields.FindField("BSM")).ToString();
                            //sTBBH = pRow.get_Value(pRow.Fields.FindField(cboField.Text)).ToString();
                            //oldBSM = sBSM;
                        }
                        else
                        {
                            sql = string.Format("Insert Into 举证图斑信息表 (序号,行政区划代码,省名,地市名,县名,对应三调图斑标识码,举证图斑预编号) Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", (i++).ToString(), dwdm, sheng, shi, xian, sBSM, sTBBH);
                            int m = LS_ResultMDBHelper.ExecuteSQL(sql, exportFile);
                            bsms.Add(sBSM);
                            //sTBBH += "/" + pRow.get_Value(pRow.Fields.FindField(cboField.Text)).ToString();
                        }
                    }
                    //if (sBSM != "" && sTBBH != "")
                    //{
                    //    sql = string.Format("Insert Into 举证图斑信息表 (序号,行政区划代码,省名,地市名,县名,对应三调图斑标识码,举证图斑预编号) Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", (i++).ToString(), dwdm, sheng, shi, xian, sBSM, sTBBH);
                    //    int n = LS_ResultMDBHelper.ExecuteSQL(sql, exportFile);
                    //}
                }
            }
            OtherHelper.ReleaseComObject(pFeatureCursor);
            wait.Close();
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private IWorkspace DeleteAndNewTmpGDB()
        {
            string path = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;

            if (Directory.Exists(path))
            {
                try
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                catch
                {
                    foreach (string tmp in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(tmp))
                        {
                            //如果有子文件删除文件
                            File.Delete(tmp);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(path);
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            return tmpWS;
        }
    }
}
