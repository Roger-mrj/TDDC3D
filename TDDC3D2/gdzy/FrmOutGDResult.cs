using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using RCIS.Database;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TDDC3D.gdzy
{
    public partial class FrmOutGDResult : Form
    {
        public FrmOutGDResult()
        {
            InitializeComponent();
        }
        public IWorkspace pCurrWs = null;
        Dictionary<string, string> dmmc = new Dictionary<string, string>();

        private void FrmOutGDResult_Load(object sender, EventArgs e)
        {
            if (pCurrWs != null)
            {
                IFeatureClass pXZQClass = null;
                string xzdm = "";
                try
                {
                    pXZQClass = (pCurrWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
                }
                catch { }
                if (pXZQClass != null)
                {
                    IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                    if (firstFea != null)
                    {
                        xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                        txtXZQDM.Text = xzdm.Length >= 6 ? xzdm.Substring(0, 6) : "";
                    }
                }
            }
            dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(pCurrWs as IFeatureWorkspace, "XZQ", "XZQDM", "XZQMC");
            
            string SQL = "Select MC From SYS_XZQ Where DM='" + txtXZQDM.Text + "'";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(SQL, "tmp");
            string mc = dt.Rows[0][0].ToString();
            dmmc.Add(txtXZQDM.Text, mc);

            dmmc = dmmc.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);

        }

        private void txtExportPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dia = new FolderBrowserDialog();
            if (dia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtExportPath.Text = dia.SelectedPath;
            }
        }
        private DataTable TP(DataTable dt, string mjField, double zmj)
        {
            object o = dt.Compute("Sum(" + mjField + ")", "");
            double tempmj;
            double.TryParse(o.ToString(), out tempmj);

            DataView dv = new DataView(dt);
            dv.Sort = mjField + " desc";
            DataTable t = dv.ToTable();
            if (tempmj != zmj)
            {
                double mj = 0;
                if (tempmj > zmj)
                {
                    mj = Math.Round(tempmj - zmj, 2);
                    int count = int.Parse((mj * 100).ToString());
                    for (int i = 0; i < count; i++)
                    {
                        int n = i % t.Rows.Count;
                        t.Rows[n][mjField] = double.Parse(t.Rows[i][mjField].ToString()) - 0.01;
                    }
                }
                else
                {
                    mj = Math.Round(zmj - tempmj, 2);
                    int count = int.Parse((mj * 100).ToString());
                    for (int i = 0; i < count; i++)
                    {
                        int n = i % t.Rows.Count;
                        t.Rows[n][mjField] = double.Parse(t.Rows[i][mjField].ToString()) + 0.01;
                    }
                }
            }
            return t;
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            
            if (Directory.Exists(txtExportPath.Text + "\\县级成果"))
            {
                MessageBox.Show("成果目录已经存在，请修改导出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("正在输出耕地资源质量分类成果包");

            DataRow dr3 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + txtXZQDM.Text + @"""", "tmp");
            string xian = dr3 == null ? "" : dr3[1].ToString();
            string destFolder = string.Format(txtExportPath.Text + "\\县级成果\\{0}{1}",txtXZQDM.Text,xian);
            
            Directory.CreateDirectory(destFolder);
            RCIS.Utility.File_DirManipulate.FolderCopy(Application.StartupPath + "\\SystemConf\\耕地资源质量分类成果", destFolder);
            
            string SJdestExcelDir = destFolder + "\\数据库成果";
            UpdateStatus("正在导出数据库成果");

            IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
            pWorkspaceFactory.Create(SJdestExcelDir, txtXZQDM.Text+"GDZYZLFL.GDB", null, 0);
            IWorkspace pGDBWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(SJdestExcelDir + "\\" + txtXZQDM.Text + "GDZYZLFL.GDB");
            IDataset pSourceDataset = (pCurrWs as IFeatureWorkspace).OpenFeatureDataset("GDZY");
            RCIS.GISCommon.WorkspaceHelper2.CreateFeatrueDataset(pGDBWs, pSourceDataset.Name, (pSourceDataset as IGeoDataset).SpatialReference);
            IFeatureDataset pTargetDataset=(pGDBWs as IFeatureWorkspace).OpenFeatureDataset(pSourceDataset.Name);
            IFeatureClassContainer pContainer=pSourceDataset as IFeatureClassContainer;
            for (int i = 0; i < pContainer.ClassCount; i++)
			{
                IFeatureClass pFeaClass=pContainer.get_Class(i);
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(pCurrWs, pGDBWs, (pFeaClass as IDataset).Name, (pFeaClass as IDataset).Name, pTargetDataset, null);
			}
            ConvertFeatureClassXZQ(pCurrWs, pGDBWs, "XZQ", "XZQ", pTargetDataset, null);
            ConvertFeatureClassXZQ(pCurrWs, pGDBWs, "XZQJX", "XZQJX", pTargetDataset, null);
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(pCurrWs, pGDBWs, "ZJ", "ZJ", pTargetDataset, null);

           
            //成果报表
            IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, "FLDY", "FLDY", null);
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, "KCFLDY", "KCFLDY", null);
            pTmpWs.ExecuteSQL("update FLDY set zldwdm = substring(zldwdm,1,9)");
            pTmpWs.ExecuteSQL("update KCFLDY set zldwdm = substring(zldwdm,1,9)");
            //string sql = "select zldwdm,szjb,zrqdm,pdjb,tchdjb,trzdjb,tryjzhljb,trphzjb,trswdyxjb,trzjswrzkjb,gdejdljb,tbdlmj from fldy";
            //DataTable table = RCIS.GISCommon.EsriDatabaseHelper.ITable2DataTable(pTmpWs, sql);

            //sql = "select zldwdm,szjb,zrqdm,pdjb,tchdjb,trzdjb,tryjzhljb,trphzjb,trswdyxjb,trzjswrzkjb,gdejdljb,tbdlmj from kcfldy";
            //DataTable tableKc = RCIS.GISCommon.EsriDatabaseHelper.ITable2DataTable(pTmpWs, sql);

            //DataTable dtResult = GetSumByDatatable(table);
            //object o = table.Compute("Sum(tbdlmj)", "");
            //double zmj;
            //double.TryParse(o.ToString(), out zmj);
            //zmj = Math.Round(zmj / 10000, 2);
            //dtResult = TP(dtResult, "tbdlmj", zmj);
            //DataTable dtResultKc = GetSumByDatatable(tableKc);
            //o = tableKc.Compute("Sum(tbdlmj)", "");
            //double.TryParse(o.ToString(), out zmj);
            //zmj = Math.Round(zmj / 10000, 2);
            //dtResultKc = TP(dtResultKc, "tbdlmj", zmj);
            //List<DataTable> dtList = initReport(dtResult,txtXZQDM.Text,xian);
            //List<DataTable> dtListKc = initReport(dtResultKc, txtXZQDM.Text, xian, true);

            //
            string sql = "select zldwdm,szjb,zrqdm,pdjb,tchdjb,trzdjb,tryjzhljb,trphzjb,swdyxjb,trzjswrjb,gdejdljb,tbdlmj from fldy";
            DataTable table = RCIS.GISCommon.EsriDatabaseHelper.ITable2DataTable(pTmpWs, sql);

            sql = "select zldwdm,szjb,zrqdm,pdjb,tchdjb,trzdjb,tryjzhljb,trphzjb,swdyxjb,trzjswrjb,gdejdljb,tbdlmj from kcfldy";
            DataTable tableKc = RCIS.GISCommon.EsriDatabaseHelper.ITable2DataTable(pTmpWs, sql);

            //DataTable dtResult = GetSumByDatatableFL(table);
            //object o = table.Compute("Sum(tbdlmj)", "");
            //double zmj;
            //double.TryParse(o.ToString(), out zmj);
            //zmj = Math.Round(zmj / 10000, 2);
            //dtResult = TP(dtResult, "tbdlmj", zmj);
            //DataTable dtResultKc = GetSumByDatatableKC(tableKc);
            //o = tableKc.Compute("Sum(tbdlmj)", "");
            //double.TryParse(o.ToString(), out zmj);
            //zmj = Math.Round(zmj / 10000, 2);
            //dtResultKc = TP(dtResultKc, "tbdlmj", zmj);
            List<DataTable> dtList = initReport(table, txtXZQDM.Text, xian);
            List<DataTable> dtListKc = initReport(tableKc, txtXZQDM.Text, xian, true);
            //


            string GDdestExcelDir = destFolder + "\\数据成果\\耕地地类";
            string HFdestExcelDir = destFolder + "\\数据成果\\恢复地类";
            UpdateStatus("正在输出成果报表");

            exportExcelHZ(dtList, GDdestExcelDir, "耕地资源质量分类面积汇总表", txtXZQDM.Text, xian, false);
            exportExcelZRQ(dtList[0], GDdestExcelDir, "耕地资源质量分类面积汇总表-自然区", txtXZQDM.Text, xian, false);
            exportExcel(dtList[1], GDdestExcelDir, "耕地资源质量分类面积汇总表-坡度", txtXZQDM.Text, xian,true, false);
            exportExcel(dtList[2], GDdestExcelDir, "耕地资源质量分类面积汇总表-土层厚度", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[3], GDdestExcelDir, "耕地资源质量分类面积汇总表-土壤质地", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[4], GDdestExcelDir, "耕地资源质量分类面积汇总表-土壤有机质含量", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[5], GDdestExcelDir, "耕地资源质量分类面积汇总表-土壤pH值", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[6], GDdestExcelDir, "耕地资源质量分类面积汇总表-生物多样性", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[7], GDdestExcelDir, "耕地资源质量分类面积汇总表-土壤重金属污染状况", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[8], GDdestExcelDir, "耕地资源质量分类面积汇总表-熟制", txtXZQDM.Text, xian, true, false);
            exportExcel(dtList[9], GDdestExcelDir, "耕地资源质量分类面积汇总表-耕地二级地类", txtXZQDM.Text, xian, true, false);

            exportExcelHZ(dtListKc, HFdestExcelDir, "耕地资源质量分类面积汇总表(JKHF)", txtXZQDM.Text, xian, false);
            exportExcelZRQ(dtListKc[0], HFdestExcelDir, "耕地资源质量分类面积汇总表-自然区", txtXZQDM.Text, xian, false);
            exportExcel(dtListKc[1], HFdestExcelDir, "耕地资源质量分类面积汇总表-坡度", txtXZQDM.Text, xian,false, false);
            exportExcel(dtListKc[2], HFdestExcelDir, "耕地资源质量分类面积汇总表-土层厚度", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[3], HFdestExcelDir, "耕地资源质量分类面积汇总表-土壤质地", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[4], HFdestExcelDir, "耕地资源质量分类面积汇总表-土壤有机质含量", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[5], HFdestExcelDir, "耕地资源质量分类面积汇总表-土壤pH值", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[6], HFdestExcelDir, "耕地资源质量分类面积汇总表-生物多样性", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[7], HFdestExcelDir, "耕地资源质量分类面积汇总表-土壤重金属污染状况", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[8], HFdestExcelDir, "耕地资源质量分类面积汇总表-熟制", txtXZQDM.Text, xian, false, false);
            exportExcel(dtListKc[9], HFdestExcelDir, "耕地资源质量分类面积汇总表-耕地二级地类(JKHF)", txtXZQDM.Text, xian, false, false);

            MessageBox.Show("成果包输出完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private static bool ConvertFeatureClassXZQ(IWorkspace sourceWorkspace,
           IWorkspace targetWorkspace, string nameOfSourceFeatureClass,
           string nameOfTargetFeatureClass, IFeatureDataset pFeatureDataset, string whereClause)
        {
            try
            {
                IFeatureDatasetName pName = pFeatureDataset.FullName as IFeatureDatasetName;
                //create source workspace name 
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
                //create source dataset name   
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = nameOfSourceFeatureClass;

                //create target workspace name   
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                //create target dataset name    
                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();

                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = nameOfTargetFeatureClass;

                //Open input Featureclass to get field definitions.  
                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
                IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
                //Validate the field names because you are converting between different workspace types.   
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IFields targetFeatureClassFields;
                IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
                IEnumFieldError enumFieldError;
                // Most importantly set the input and validate workspaces! 
                fieldChecker.InputWorkspace = sourceWorkspace;
                fieldChecker.ValidateWorkspace = targetWorkspace;
                fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError,
                    out targetFeatureClassFields);


                IField pField = targetFeatureClassFields.get_Field(targetFeatureClassFields.FindField("BZ"));
                IFieldEdit2 pEdit = pField as IFieldEdit2;
                pEdit.Length_2 = 200;
                // Loop through the output fields to find the geomerty field   
                IField geometryField;
                for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
                {
                    if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        //if (targetFeatureClassFields.get_Field(i).Name == "BZ")
                        //    targetFeatureClassFields.get_Field(i).Length = 200;
                        geometryField = targetFeatureClassFields.get_Field(i);
                        // Get the geometry field's geometry defenition          
                        IGeometryDef geometryDef = geometryField.GeometryDef;
                        //Give the geometry definition a spatial index grid count and grid size     
                        IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                        targetFCGeoDefEdit.GridCount_2 = 1;
                        targetFCGeoDefEdit.set_GridSize(0, 0);
                        
                        //Allow ArcGIS to determine a valid grid size for the data loaded     
                        targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
                        // we want to convert all of the features    
                        IQueryFilter queryFilter = new QueryFilterClass();
                        queryFilter.WhereClause = whereClause;
                        // Load the feature class            
                        IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                        IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName,
                            queryFilter, pName, targetFeatureClassName,
                            geometryDef, targetFeatureClassFields, "", 1000, 0);
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }


        //private List<DataTable> initReport(DataTable dtRes,string xzqdm,string xzqmc, bool isHFDL = false)
        //{
        //    List<DataTable> dtLst = new List<DataTable>();

        //    string statisticsField = "";
        //    string sql = "";
        //    for (int m = 0; m < 10; m++)
        //    {
        //        DataTable dtTmp = new DataTable();
        //        dtTmp.Columns.Add("mc");
        //        dtTmp.Columns.Add("dm");
        //        dtTmp.Columns.Add("hj", typeof(double));
        //        if (m == 0)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-自然区");
        //            sql = "select zrqdm from SYS_QGGXSSZRQHSZ where xzqdm='" + xzqdm + "' ";
        //            statisticsField = "zrqdm";
        //        }
        //        else if (m == 1)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-坡度");
        //            sql = "select dm from DIC_36坡度级别代码表 ";
        //            statisticsField = "pdjb";
        //        }
        //        else if (m == 2)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-土层厚度");
        //            sql = "select dm from DIC_29土层厚度代码表 ";
        //            statisticsField = "tchdjb";
        //        }
        //        else if (m == 3)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤质地");
        //            sql = "select dm from DIC_30土壤质地代码表 ";
        //            statisticsField = "trzdjb";
        //        }
        //        else if (m == 4)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤有机质含量");
        //            sql = "select dm from DIC_31土壤有机质含量代码表 ";
        //            statisticsField = "tryjzhljb";
        //        }
        //        else if (m == 5)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤 pH 值");
        //            sql = "select dm from DIC_32土壤PH值代码表 ";
        //            statisticsField = "trphzjb";
        //        }
        //        else if (m == 6)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-生物多样性");
        //            sql = "select dm from DIC_33生物多样性代码表 ";
        //            if (isHFDL)
        //                statisticsField = "swdyxjb";
        //            else
        //                statisticsField = "trswdyxjb";
        //        }
        //        else if (m == 7)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤重金属污染状况");
        //            sql = "select dm from DIC_33生物多样性代码表 ";
        //            statisticsField = "trzjswrjb";
        //        }
        //        else if (m == 8)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-熟制报表");
        //            sql = "select dm from DIC_35熟制代码表 ";
        //            statisticsField = "szjb";
        //        }
        //        else if (m == 9)
        //        {
        //            UpdateStatus("正在统计耕地资源质量分类面积汇总表-耕地二级地类");
        //            sql = "select dm from DIC_36耕地二级代码表 ";
        //            statisticsField = "gdejdljb";
        //        }

        //        DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
        //        if (m == 9)
        //        {
        //            if (isHFDL)
        //            {
        //                for (int i = 3; i < dt.Rows.Count; i++)
        //                {
        //                    dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 0; i < 3; i++)
        //                {
        //                    dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
        //                }
        //            }

        //        }
        //        else
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
        //            }
        //        }

        //        DataRow dr = dtTmp.NewRow();
        //        for (int i = 0; i < dtTmp.Columns.Count; i++)
        //        {
        //            if (dtTmp.Columns[i].ColumnName.ToString() == "dm")
        //                dr["dm"] = xzqdm;
        //            else if (dtTmp.Columns[i].ColumnName.ToString() == "mc")
        //                dr["mc"] = xzqmc;
        //            else
        //                dr[dtTmp.Columns[i].ColumnName.ToString()] = 0;
        //        }
        //        dtTmp.Rows.Add(dr);
        //        createTable(dtRes, statisticsField, "zldwdm", "tbdlmj", ref dtTmp);
        //        dtLst.Add(dtTmp);
        //    }
        //    return dtLst;
        //}
        private List<DataTable> initReport(DataTable dtRes, string xzqdm, string xzqmc, bool isHFDL = false)
        {
            List<DataTable> dtLst = new List<DataTable>();

            string statisticsField = "";
            string sql = "";
            for (int m = 0; m < 10; m++)
            {
                DataTable dtTmp = new DataTable();
                dtTmp.Columns.Add("mc");
                dtTmp.Columns.Add("dm");
                dtTmp.Columns.Add("hj", typeof(double));
                if (m == 0)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-自然区");
                    sql = "select zrqdm from SYS_QGGXSSZRQHSZ where xzqdm='" + xzqdm + "' ";
                    statisticsField = "zrqdm";
                }
                else if (m == 1)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-坡度");
                    sql = "select dm from DIC_36坡度级别代码表 ";
                    statisticsField = "pdjb";
                }
                else if (m == 2)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土层厚度");
                    sql = "select dm from DIC_29土层厚度代码表 ";
                    statisticsField = "tchdjb";
                }
                else if (m == 3)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤质地");
                    sql = "select dm from DIC_30土壤质地代码表 ";
                    statisticsField = "trzdjb";
                }
                else if (m == 4)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤有机质含量");
                    sql = "select dm from DIC_31土壤有机质含量代码表 ";
                    statisticsField = "tryjzhljb";
                }
                else if (m == 5)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤 pH 值");
                    sql = "select dm from DIC_32土壤PH值代码表 ";
                    statisticsField = "trphzjb";
                }
                else if (m == 6)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-生物多样性");
                    sql = "select dm from DIC_33生物多样性代码表 ";
                    //if (isHFDL)
                        statisticsField = "swdyxjb";
                    //else
                    //    statisticsField = "trswdyxjb";
                }
                else if (m == 7)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤重金属污染状况");
                    sql = "select dm from DIC_34土壤重金属污染状况类型 ";
                    statisticsField = "trzjswrjb";


                }
                else if (m == 8)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-熟制报表");
                    sql = "select dm from DIC_35熟制代码表 ";
                    statisticsField = "szjb";
                }
                else if (m == 9)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-耕地二级地类");
                    sql = "select dm from DIC_36耕地二级代码表 ";
                    statisticsField = "gdejdljb";
                }

                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (m == 9)
                {
                    if (isHFDL)
                    {
                        for (int i = 3; i < dt.Rows.Count; i++)
                        {
                            dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
                        }
                    }

                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
                    }
                }

                DataRow dr = dtTmp.NewRow();
                for (int i = 0; i < dtTmp.Columns.Count; i++)
                {
                    if (dtTmp.Columns[i].ColumnName.ToString() == "dm")
                        dr["dm"] = xzqdm;
                    else if (dtTmp.Columns[i].ColumnName.ToString() == "mc")
                        dr["mc"] = xzqmc;
                    else
                        dr[dtTmp.Columns[i].ColumnName.ToString()] = 0;
                }
                dtTmp.Rows.Add(dr);
                createTable(dtRes, statisticsField, "zldwdm", "tbdlmj", ref dtTmp);
                dtLst.Add(dtTmp);
            }
            return dtLst;
        }

        private void UpdateStatus(string txt)
        {
            this.memoEdit1.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + memoEdit1.Text;
            Application.DoEvents();
        }

        private DataTable GetSumByDatatableFL(DataTable dt)
        {
            var query = dt.AsEnumerable()
                .GroupBy(d => new
                {
                    zldwdm = d.Field<string>("zldwdm"),
                    szjb = d.Field<string>("szjb"),
                    zrqdm = d.Field<string>("zrqdm"),
                    pdjb = d.Field<string>("pdjb"),
                    tchdjb = d.Field<string>("tchdjb"),
                    trzdjb = d.Field<string>("trzdjb"),
                    tryjzhljb = d.Field<string>("tryjzhljb"),
                    trphzjb = d.Field<string>("trphzjb"),
                    trswdyxjb = d.Field<string>("trswdyxjb"),
                    trzjswrzkjb = d.Field<string>("trzjswrjb"),
                    gdejdljb = d.Field<string>("gdejdljb")
                })
                .Select(g => new
                {
                    zldwdm = g.Key.zldwdm,
                    szjb = g.Key.szjb,
                    zrqdm = g.Key.zrqdm,
                    pdjb = g.Key.pdjb,
                    tchdjb = g.Key.tchdjb,
                    trzdjb = g.Key.trzdjb,
                    tryjzhljb = g.Key.tryjzhljb,
                    trphzjb = g.Key.trphzjb,
                    trswdyxjb = g.Key.trswdyxjb,
                    trzjswrzkjb = g.Key.trzjswrzkjb,
                    gdejdljb = g.Key.gdejdljb,
                    tbdlmj = Math.Round(g.Sum(d => d.Field<double>("tbdlmj")) / 10000, 6)
                });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("zldwdm");
            dtResult.Columns.Add("szjb");
            dtResult.Columns.Add("zrqdm");
            dtResult.Columns.Add("pdjb");
            dtResult.Columns.Add("tchdjb");
            dtResult.Columns.Add("trzdjb");
            dtResult.Columns.Add("tryjzhljb");
            dtResult.Columns.Add("trphzjb");
            dtResult.Columns.Add("trswdyxjb");
            dtResult.Columns.Add("trzjswrjb");
            dtResult.Columns.Add("gdejdljb");
            dtResult.Columns.Add(new DataColumn("tbdlmj", typeof(float)));
            query.ToList().ForEach(q => dtResult.Rows.Add(
                q.zldwdm,
                q.szjb,
                q.zrqdm,
                q.pdjb,
                q.tchdjb,
                q.trzdjb,
                q.tryjzhljb,
                q.trphzjb,
                q.trswdyxjb,
                q.trzjswrzkjb,
                q.gdejdljb,
                q.tbdlmj));
            return dtResult;
        }

        private DataTable GetSumByDatatableKC(DataTable dt)
        {
            var query = dt.AsEnumerable()
                .GroupBy(d => new
                {
                    zldwdm = d.Field<string>("zldwdm"),
                    szjb = d.Field<string>("szjb"),
                    zrqdm = d.Field<string>("zrqdm"),
                    pdjb = d.Field<string>("pdjb"),
                    tchdjb = d.Field<string>("tchdjb"),
                    trzdjb = d.Field<string>("trzdjb"),
                    tryjzhljb = d.Field<string>("tryjzhljb"),
                    trphzjb = d.Field<string>("trphzjb"),
                    trswdyxjb = d.Field<string>("swdyxjb"),
                    trzjswrzkjb = d.Field<string>("trzjswrjb"),
                    gdejdljb = d.Field<string>("gdejdljb")
                })
                .Select(g => new
                {
                    zldwdm = g.Key.zldwdm,
                    szjb = g.Key.szjb,
                    zrqdm = g.Key.zrqdm,
                    pdjb = g.Key.pdjb,
                    tchdjb = g.Key.tchdjb,
                    trzdjb = g.Key.trzdjb,
                    tryjzhljb = g.Key.tryjzhljb,
                    trphzjb = g.Key.trphzjb,
                    trswdyxjb = g.Key.trswdyxjb,
                    trzjswrzkjb = g.Key.trzjswrzkjb,
                    gdejdljb = g.Key.gdejdljb,
                    tbdlmj = Math.Round(g.Sum(d => d.Field<double>("tbdlmj")) / 10000, 6)
                });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("zldwdm");
            dtResult.Columns.Add("szjb");
            dtResult.Columns.Add("zrqdm");
            dtResult.Columns.Add("pdjb");
            dtResult.Columns.Add("tchdjb");
            dtResult.Columns.Add("trzdjb");
            dtResult.Columns.Add("tryjzhljb");
            dtResult.Columns.Add("trphzjb");
            dtResult.Columns.Add("swdyxjb");
            dtResult.Columns.Add("trzjswrjb");
            dtResult.Columns.Add("gdejdljb");
            dtResult.Columns.Add(new DataColumn("tbdlmj", typeof(float)));
            query.ToList().ForEach(q => dtResult.Rows.Add(
                q.zldwdm,
                q.szjb,
                q.zrqdm,
                q.pdjb,
                q.tchdjb,
                q.trzdjb,
                q.tryjzhljb,
                q.trphzjb,
                q.trswdyxjb,
                q.trzjswrzkjb,
                q.gdejdljb,
                q.tbdlmj));
            return dtResult;
        }

        private void exportExcel(DataTable dt, string destExcelDir, string tableName, string xzqdm, string xzqmc, bool isGD,Boolean isVisible = false, int iScale = 1)
        {
            try
            {
                string tmplateFile = RCIS.Global.AppParameters.TemplatePath + @"\" + tableName + ".xlsx";
                string dateStr;
                if (isVisible)
                    dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                else
                    dateStr = "";
                if (tableName.Contains("JKHF"))
                    tableName = tableName.Replace("(JKHF)","");
                string excelReportFilename = destExcelDir + @"\(" + xzqdm + ")" + xzqmc + tableName + "" + dateStr + ".xlsx";
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                int rowIndex = 4;
                int colIndex = 0;

                //cells[1, 1].PutValue(XZQDM);
                //cells[1, 3].PutValue(XZQMC);
                //cells[1, 4].PutValue("单位：");
                //cells[1, 5].PutValue(DateTime.Now.ToLongDateString());
                //if (iScale == 15) cells[1, 5].PutValue("亩");
                //else
                //    cells[1, 5].PutValue("公顷");

                if (dt.Rows.Count == 0)
                {
                    List<string> arr = new List<string>() ;
                    if(isGD)
                        arr=RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "FLDY", "ZLDWDM", 9);
                    else
                        arr = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "KCFLDY", "ZLDWDM", 9);

                    arr.Insert(0,xzqdm);
                    //int i = 0;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        string item = arr[i];
                        string value = dmmc[item];

                        cells[rowIndex + i, colIndex].SetStyle(styleTxt);
                        cells[rowIndex + i, colIndex].PutValue(value);

                        cells[rowIndex + i, colIndex + 1].SetStyle(styleTxt);
                        cells[rowIndex + i, colIndex + 1].PutValue(item);

                        for (int k = 2; k < dt.Columns.Count; k++)
                        {
                            cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                            double mj = 0;
                            cells[rowIndex + i, colIndex + k].PutValue(mj);
                        }
                        //i++;
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k < 2)
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleTxt);
                                cells[rowIndex + i, colIndex + k].PutValue(dt.Rows[i][k].ToString());
                            }
                            else
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                                double mj = 0;
                                double.TryParse(dt.Rows[i][k].ToString(), out mj);
                                cells[rowIndex + i, colIndex + k].PutValue(mj);
                            }
                        }
                    }
                }


                wk.Save(excelReportFilename);
                if (isVisible)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(excelReportFilename);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void exportExcelHZ(List<DataTable> dtList, string destExcelDir, string tableName, string XZQDM, string XZQMC, Boolean isVisible = false, int iScale = 1)
        {
            try
            {
                string tmplateFile = System.Environment.CurrentDirectory + @"\template\" + tableName + ".xlsx";
                string dateStr;
                if (isVisible)
                    dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                else
                    dateStr = "";

                string excelReportFilename = destExcelDir + @"\(" + XZQDM + ")" + XZQMC + (tableName.IndexOf("(") == -1 ? tableName : tableName.Substring(0, tableName.IndexOf("("))) + "" + dateStr + ".xlsx";
                if (File.Exists(excelReportFilename)) File.Delete(excelReportFilename);
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleNum.VerticalAlignment = Aspose.Cells.TextAlignmentType.Center;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                int rowIndex = 3;
                int colIndex = 2;

                string sql = "select zrqdm,zrqmc from SYS_QGGXSSZRQHSZ where xzqdm='" + XZQDM + "'";
                DataTable dtZrq = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (dtZrq.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到" + XZQMC + "的自然区数据，请导入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string zrqdm = dtZrq.Rows[0][0].ToString();
                int izrq = 0;
                int.TryParse(zrqdm, out izrq);
                //cells[2, 2].SetStyle(styleTxt);
                //cells[2, 2].PutValue(dtZrq.Rows[0][0].ToString());
                //cells[3, 2].SetStyle(styleTxt);
                //cells[3, 2].PutValue(dtZrq.Rows[0][1].ToString());
                List<string> hjVal = new List<string>();
                for (int i = 0; i < dtList.Count; i++)
                {
                    DataTable dt = dtList[i];
                    if (dt.Rows.Count > 0)
                    {
                        hjVal.Add(dt.Rows[0][2].ToString());
                        for (int j = 3; j < dt.Columns.Count; j++)
                        {
                            double val = 0;
                            double.TryParse(dt.Rows[0][j].ToString(), out val);
                            rowIndex++;
                            cells[rowIndex, 2 + izrq - 1].SetStyle(styleNum,true);
                            cells[rowIndex, 2 + izrq - 1].PutValue(val);

                            cells[rowIndex, 51].SetStyle(styleNum, true);
                            cells[rowIndex, 51].PutValue(val);
                        }
                    }
                    else
                    {
                        hjVal.Add(0.ToString());
                        for (int j = 3; j < dt.Columns.Count; j++)
                        {
                            double val = 0;
                            rowIndex++;
                            cells[rowIndex, 2 + izrq - 1].SetStyle(styleNum,true);
                            cells[rowIndex, 2 + izrq - 1].PutValue(val);

                            cells[rowIndex, 51].SetStyle(styleNum, true);
                            cells[rowIndex, 51].PutValue(val);
                        }
                    }

                }

                //cells[4, 51].SetStyle(styleNum,true);
                //cells[4, 51].PutValue(double.Parse(hjVal[0]));

                //cells[5, 51].SetStyle(styleNum, true);
                //cells[5, 51].PutValue(double.Parse(hjVal[1]));

                //cells[10, 51].SetStyle(styleNum, true);
                //cells[10, 51].PutValue(double.Parse(hjVal[2]));

                //cells[13, 51].SetStyle(styleNum, true);
                //cells[13, 51].PutValue(double.Parse(hjVal[3]));

                //cells[16, 51].SetStyle(styleNum, true);
                //cells[16, 51].PutValue(double.Parse(hjVal[4]));

                //cells[19, 51].SetStyle(styleNum, true);
                //cells[19, 51].PutValue(double.Parse(hjVal[5]));

                //cells[24, 51].SetStyle(styleNum, true);
                //cells[24, 51].PutValue(double.Parse(hjVal[6]));

                //cells[27, 51].SetStyle(styleNum, true);
                //cells[27, 51].PutValue(double.Parse(hjVal[7]));

                //cells[30, 51].SetStyle(styleNum, true);
                //cells[30, 51].PutValue(double.Parse(hjVal[8]));

                //cells[33, 51].SetStyle(styleNum, true);
                //cells[33, 51].PutValue(double.Parse(hjVal[9]));


                wk.Save(excelReportFilename);
                if (isVisible)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(excelReportFilename);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void exportExcelZRQ(DataTable dt, string destExcelDir, string tableName, string XZQDM, string XZQMC, Boolean isVisible = false, int iScale = 1)
        {
            try
            {
                string tmplateFile = System.Environment.CurrentDirectory + @"\template\" + tableName + ".xlsx";
                string dateStr;
                if (isVisible)
                    dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                else
                    dateStr = "";

                string excelReportFilename = destExcelDir + @"\(" + XZQDM + ")" + XZQMC + (tableName.IndexOf("(") == -1 ? tableName : tableName.Substring(0, tableName.IndexOf("("))) + "" + dateStr + ".xlsx";
                if (File.Exists(excelReportFilename)) File.Delete(excelReportFilename);
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                if (dt.Rows.Count < 2)
                {
                    MessageBox.Show("" + tableName + "表统计失败，请完善数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                string sql = "select zrqdm,zrqmc from SYS_QGGXSSZRQHSZ where xzqdm='" + XZQDM + "'";
                DataTable dtZrq = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (dtZrq.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到" + XZQMC + "的自然区数据，请导入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string zrqdm = dtZrq.Rows[0][0].ToString();
                int izrq = 0;
                int.TryParse(zrqdm, out izrq);

                int rowIndex = 4;
                int colIndex = izrq - 1;

                //cells[1, 1].PutValue(XZQDM);
                //cells[1, 3].PutValue(XZQMC);
                //cells[1, 4].PutValue("单位：");
                //cells[1, 5].PutValue(DateTime.Now.ToLongDateString());
                //if (iScale == 15) cells[1, 5].PutValue("亩");
                //else
                //    cells[1, 5].PutValue("公顷");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 52; j++)
                    {
                        cells[rowIndex + i, j].SetStyle(styleTxt);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k < 3)
                            {
                                cells[rowIndex + i, k].SetStyle(styleTxt);
                                cells[rowIndex + i, k].PutValue(dt.Rows[i][k].ToString());
                            }
                            else
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                                double mj = 0;
                                cells[rowIndex + i, colIndex + k].PutValue(mj);
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k < 3)
                            {
                                cells[rowIndex + i, k].SetStyle(styleTxt);
                                cells[rowIndex + i, k].PutValue(dt.Rows[i][k].ToString());
                                if (k == 2)
                                {
                                    cells[rowIndex + i, k].SetStyle(styleNum, true);
                                    cells[rowIndex + i, k].PutValue(double.Parse(dt.Rows[i][k].ToString()));
                                }
                            }
                            else
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                                double mj = 0;
                                double.TryParse(dt.Rows[i][k].ToString(), out mj);
                                cells[rowIndex + i, colIndex + k].PutValue(mj);
                            }
                        }
                    }
                }


                wk.Save(excelReportFilename);
                if (isVisible)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(excelReportFilename);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void createTable(DataTable dataTable, string groupingColumn, string nameColumn, string dataColumn, ref DataTable resultTable)
        {
            //取出不重复的需要转换的列的数据
            DataTable distinct_GroupingColumn = dataTable.DefaultView.ToTable(true, groupingColumn);
            //取出不重复的名称列
            DataTable dictinct_NameColumn = dataTable.DefaultView.ToTable(true, nameColumn);
            //List<string> lstID = (from d in dictinct_NameColumn.AsEnumerable() select d.Field<string>("zldwdm").ToString().Substring(0, 9)).ToList();
            var lstID = dictinct_NameColumn.AsEnumerable().GroupBy(a => a.Field<string>("zldwdm").Substring(0, 9)).ToList();
            //构建新表group d by d.m
            //DataTable table = new DataTable();
            #region 构建新表的列
            //将名称列添加进新表
            DataColumn newColumn = new DataColumn();
            newColumn.ColumnName = nameColumn;

            #endregion
            #region 向新表中添加数据
            DataRow newRow;
            foreach (var item in lstID)
            {
                //添加一个新行
                newRow = resultTable.NewRow();
                //为此新行添加第一个行数据
                //newRow[nameColumn] = item.Key;
                //为此新行添加列数据
                double hj = 0;
                double xianHJ = 0;
                foreach (DataRow r in distinct_GroupingColumn.Rows)
                {
                    if (!resultTable.Columns.Contains(r[groupingColumn].ToString()))
                    {
                        UpdateStatus("" + groupingColumn + "字段存在错误值，请完善数据。");
                        resultTable.Rows.Clear();
                        return;
                    }
                    object val = dataTable.Compute("sum(tbdlmj)", nameColumn + " like '" + item.Key.ToString() + "%' and " + groupingColumn + " ='" + r[groupingColumn].ToString() + "'");
                    double sum = 0;
                    if(!string.IsNullOrWhiteSpace(val.ToString()))
                        sum = (double)val;
                    //double.TryParse(val.ToString(), out sum);
                    sum = Math.Round(sum/10000, 6);
                    hj += sum;
                    if (sum != 0)
                    {
                        //将数据与新建表进行连合
                        newRow[r[groupingColumn].ToString()] = sum;
                        newRow["dm"] = item.Key;
                        newRow["mc"] = dmmc[item.Key];
                        resultTable.Rows[0][r[groupingColumn].ToString()] = sum + double.Parse(resultTable.Rows[0][r[groupingColumn].ToString()].ToString());
                    }
                    xianHJ += double.Parse(resultTable.Rows[0][r[groupingColumn].ToString()].ToString());
                }
                newRow["hj"] = hj;
                resultTable.Rows.Add(newRow);
                resultTable.Rows[0]["hj"] = xianHJ;
            }
            #endregion

            //return table;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
