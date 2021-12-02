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
using RCIS.Database;
using ESRI.ArcGIS.esriSystem;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Collections;
//using Excel = Microsoft.Office.Interop.Excel;

using RCIS.Global;
using ESRI.ArcGIS.Geoprocessor;

namespace TDDC3D.output
{
    public partial class StatesForm : Form
    {
        public StatesForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
        public IWorkspace currWs = null;


        public string CurrDW
        {
            get
            {
                if (this.rgDanwei.SelectedIndex == 0)
                {
                    return "公顷";
                }
                else
                {
                    return "亩";
                }
            }
        }

        //public void DeletDLTBXZQ()
        //{
        //    string destFileName = RCIS.Global.AppParameters.ConfPath + "\\setup.mdb";
        //    IWorkspaceName targetWorkspaceName = new WorkspaceNameClass
        //    {
        //        WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory",
        //        PathName = destFileName
        //    };
        //    IName targetWorkspaceIName = (IName)targetWorkspaceName;
        //    IWorkspace targetWorkspace = (IWorkspace)targetWorkspaceIName.Open();

        //    if ((targetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, "DLTB"))
        //    {
        //        IDataset pDS = (targetWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTB") as IDataset;
        //        pDS.Delete();
        //    }
        //    if ((targetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, "XZQ"))
        //    {
        //        IDataset pDS = (targetWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ") as IDataset;
        //        pDS.Delete();
        //    }
            

        //}

        //public bool SpatialJoin()
        //{
        //    ESRI.ArcGIS.AnalysisTools.SpatialJoin sjoin = new ESRI.ArcGIS.AnalysisTools.SpatialJoin();
        //    sjoin.target_features = currWs.PathName + "\\" +RCIS.Global.AppParameters.DATASET_DEFAULT_NAME+  "\\DLTB";
        //    sjoin.join_features = currWs.PathName + "\\" +RCIS.Global.AppParameters.DATASET_DEFAULT_NAME+"\\XZQ";
        //    sjoin.out_feature_class = RCIS.Global.AppParameters.ConfPath + "\\result.mdb\\DLTB";
        //    sjoin.join_operation = "JOIN_ONE_TO_ONE";
        //    sjoin.match_option = "INTERSECTS";
        //    sjoin.join_type = "KEEP_ALL";

        //    string serr = "";

        //    Geoprocessor gp = new Geoprocessor();
        //    gp.OverwriteOutput = true;
        //    try
        //    {
        //        gp.Execute(sjoin, null);
        //        if (gp.MessageCount > 0)
        //        {
        //            for (int icout = 0; icout <= gp.MessageCount - 1; icout++)
        //            {
        //                serr+=gp.GetMessage(icout);
        //            }
        //        }
        //        if (serr.Contains("Succeeded") || serr.Contains("成功"))
        //        {
        //            return true;
        //        }
        //        else
        //            return false;

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        ///// <summary>
        ///// 把地类图斑XZQ，拷贝到当前mdb中，方便执行sql语句
        ///// </summary>
        //public void ConvertShapefileToFeatureClass(string className)
        //{
            
            
        //    // Create a name object for the source (shapefile) workspace and open it.
        //    IWorkspaceName sourceWorkspaceName = new WorkspaceNameClass
        //    {
        //        WorkspaceFactoryProgID = "esriDataSourcesGDB.FileGDBWorkspaceFactory",
        //        PathName =this.currWs.PathName

        //    };
        //    IName sourceWorkspaceIName = (IName)sourceWorkspaceName;
        //    IWorkspace sourceWorkspace = (IWorkspace)sourceWorkspaceIName.Open();

        //    string destFileName = RCIS.Global.AppParameters.ConfPath + "\\result.mdb";
        //    // Create a name object for the target (file GDB) workspace and open it.
        //    IWorkspaceName targetWorkspaceName = new WorkspaceNameClass
        //    {
        //        WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory",
        //        PathName = destFileName
        //    };
        //    IName targetWorkspaceIName = (IName)targetWorkspaceName;
        //    IWorkspace targetWorkspace = (IWorkspace)targetWorkspaceIName.Open();

        //    if ((targetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, className))
        //    {
        //        IDataset pDS = (targetWorkspace as IFeatureWorkspace).OpenFeatureClass(className) as IDataset;
        //        pDS.Delete();
        //    }


        //    // Create a name object for the source dataset.
        //    IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
        //    IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
        //    sourceDatasetName.Name = className;
        //    sourceDatasetName.WorkspaceName = sourceWorkspaceName;

        //    // Create a name object for the target dataset.
        //    IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
        //    IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
        //    targetDatasetName.Name = className;
        //    targetDatasetName.WorkspaceName = targetWorkspaceName;

        //    // Open source feature class to get field definitions.
        //    IName sourceName = (IName)sourceFeatureClassName;
        //    IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();

        //    // Create the objects and references necessary for field validation.
        //    IFieldChecker fieldChecker = new FieldCheckerClass();
        //    IFields sourceFields = sourceFeatureClass.Fields;
        //    IFields targetFields = null;
        //    IEnumFieldError enumFieldError = null;

        //    // Set the required properties for the IFieldChecker interface.
        //    fieldChecker.InputWorkspace = sourceWorkspace;
        //    fieldChecker.ValidateWorkspace = targetWorkspace;

        //    // Validate the fields and check for errors.
        //    fieldChecker.Validate(sourceFields, out enumFieldError, out targetFields);
        //    // Loop through the output fields to find the geomerty field   
        //    IField geometryField;
        //    for (int i = 0; i < targetFields.FieldCount; i++)
        //    {
        //        if (targetFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
        //        {
        //            geometryField = targetFields.get_Field(i);
        //            // Get the geometry field's geometry defenition          
        //            IGeometryDef geometryDef = geometryField.GeometryDef;
        //            //Give the geometry definition a spatial index grid count and grid size     
        //            IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
        //            targetFCGeoDefEdit.GridCount_2 = 1;
        //            targetFCGeoDefEdit.set_GridSize(0, 0);
        //            //Allow ArcGIS to determine a valid grid size for the data loaded     
        //            targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
        //            // we want to convert all of the features    
                   
        //            // Load the feature class            
        //            IFeatureDataConverter fctofc = new FeatureDataConverterClass();
        //            IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName,
        //                null, null, targetFeatureClassName,
        //                geometryDef, targetFields, "", 1000, 0);
        //            break;
        //        }
        //    }
        //    OtherHelper.ReleaseComObject(targetWorkspace);

        //}

        /// <summary>
        /// 吧地类图斑 相关属性表读取出数据，生产那个基础统计表的临时表
        /// </summary>
        //private void Dltb2BaseTable2()
        //{
        //    LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_TMP");
        //    //直接拷贝表过来了，娘的
        //    string sql = "insert into  HZ_TMP (ZLDWDM,ZLDWMC,QSDWDM,QSDWMC,QSXZ,DLBM,GDLX,GDPDJB,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,PMMJ)  "
        //        + " select zldwdm,zldwmc,qsdwdm,qsdwmc,qsxz,dlbm,gdlx,gdpdjb,ZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,sum(TBDLMJ) as PMMJ from DLTB "
        //        + " group by zldwdm,zldwmc,qsdwdm,qsdwmc,qsxz,dlbm,gdlx,gdpdjb,ZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS ";
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

         

        //    //拷入 田坎系数 为 1203的
        //  //  sql = "select ZLDWDM,ZLDWMC,QSDWDM,QSDWMC,QSXZ,KCDLBM,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS, sum(KCMJ) as mj from DLTB where KCMJ>0 group by ZLDWDM,ZLDWMC,QSDWDM,QSDWMC,QSXZ,KCDLBM,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS ";
        //    sql = " insert into  HZ_TMP (ZLDWDM,ZLDWMC,QSDWDM,QSDWMC,QSXZ,DLBM,GDLX,GDPDJB,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,PMMJ) "
        //        + " select ZLDWDM,ZLDWMC,QSDWDM,QSDWMC,QSXZ,KCDLBM,GDLX,GDPDJB,ZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,SUM(KCMJ) "
        //        + " from DLTB where KCMJ>0 group by ZLDWDM,ZLDWMC,QSDWDM,QSDWMC,QSXZ,KCDLBM,GDLX,GDPDJB,ZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS ";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

         
            

        //}

        //private Dictionary<string, string> getZldwdmMc()
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    ITable pTable = (this.currWs as IFeatureWorkspace).OpenTable("QSDWDMB");
        //    IRow aRow = null;
        //    ICursor pCursor = pTable.Search(null, false);
        //    try
        //    {
        //        while ((aRow = pCursor.NextRow()) != null)
        //        {
        //            dic.Add(FeatureHelper.GetRowValue(aRow, "QSDWDM").ToString(), FeatureHelper.GetRowValue(aRow, "QSDWMC").ToString());
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        OtherHelper.ReleaseComObject(pCursor);
        //    }
        //    return dic;
        //}

        #region 初始化汇总表
        #region 字符串转换为浮点型
        private double String2Double(string str)
        {
            try
            {
                if (str == "") return 0.00;
                double a = 0.00;
                double.TryParse(str, out a);
                return a;
            }
            catch { return 0.00; }

        }
        #endregion
        ///// <summary>
        ///// 转化为调平基础表
        ///// </summary>
        ///// <param name="sMJKind"></param>
        //private void ChangeTMP2JCB(string sMJKind)
        //{

        //    string sql = "delete from HZ_JCB";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //
        //    sql = " select  ZLDWDM,QSDWDM,QSXZ,GDPDJB ,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM,DLBM,FRDBS,sum(" + sMJKind
        //        + ") as MJ  from hz_tmp group by ZLDWDM,QSDWDM,QSXZ,GDPDJB ,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM,DLBM,FRDBS ";
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    //表转置
        //    //建一个新表
        //    sql = "SELECT top 1 * FROM HZ_JCB "; //当前是个空表
        //    DataTable resultDt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    resultDt.Rows.Clear();
        //    foreach (DataRow firstDr in dt.Rows)
        //    {
                
        //        string zldwdm = firstDr["ZLDWDM"].ToString().Trim();
        //        if (zldwdm == "")
        //            continue;
        //        string qsdwdm = firstDr["QSDWDM"].ToString().Trim();
        //        string qsxz = firstDr["QSXZ"].ToString().Trim();
        //        string gdlx = firstDr["GDLX"].ToString().Trim().ToUpper();
        //        string PDJB = firstDr["GDPDJB"].ToString().Trim();

        //        string dlbm = firstDr["DLBM"].ToString().Trim();  //末级地类，包括A，K
                
        //        //if (dlbm.EndsWith("A"))
        //        //{
        //        //    dlbm = dlbm.Substring(0, dlbm.Length - 1);
        //        //}
        //        string frdbs = firstDr["FRDBS"].ToString().Trim();

        //        string czcsxm = firstDr["CZCSXM"].ToString().Trim();
        //        string gdzzlx = firstDr["GDZZSXDM"].ToString().Trim();
        //        string tbxhdm = firstDr["TBXHDM"].ToString().Trim();
        //        string mssm = firstDr["MSSM"].ToString().Trim();


        //        double mj = 0;
        //        double.TryParse(firstDr["MJ"].ToString(), out mj);
        //        DataRow[] selRows = resultDt.Select(" ZLDWDM='" + zldwdm + "' and QSDWDM='" + qsdwdm + "' and QSXZ='" + qsxz + "' and GDLX='" + gdlx + "' and GDPDJB='" + PDJB
        //            + "' and CZCSXM='" + czcsxm + "' and GDZZSXDM='" + gdzzlx + "' and TBXHDM='" + tbxhdm + "' and MSSM='" + mssm + "' and FRDBS='"+frdbs+"' ");
        //        if (!resultDt.Columns.Contains("D" + dlbm))
        //        {
        //            continue;
        //        }
        //        try
        //        {
        //            if (selRows.Length > 0)
        //            {
        //                //已经存在
        //                DataRow selRow = selRows[0];

        //                if (selRow["D" + dlbm].ToString().Trim() != "")
        //                {
        //                    double oldmj = 0;
        //                    double.TryParse(selRow["D" + dlbm].ToString(), out oldmj);
        //                    selRow["D" + dlbm] = mj + oldmj;
        //                }
        //                else
        //                {
        //                    selRow["D" + dlbm] = mj;
        //                }
        //            }
        //            else
        //            {
        //                DataRow newrow = resultDt.NewRow();
        //                newrow["ZLDWDM"] = zldwdm;
        //                newrow["QSDWDM"] = qsdwdm;
        //                newrow["QSXZ"] = qsxz;
        //                newrow["GDLX"] = gdlx;
        //                newrow["GDPDJB"] = PDJB;
        //                newrow["CZCSXM"] = czcsxm;  //城镇村属性码
        //                newrow["GDZZSXDM"] = gdzzlx;
        //                newrow["TBXHDM"] = tbxhdm;
        //                newrow["MSSM"] = mssm;

        //                newrow["D" + dlbm] = mj;
        //                newrow["FRDBS"] = frdbs;
        //                resultDt.Rows.Add(newrow);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }


        //    }

        //    //写入数据库
        //    foreach (DataRow dr in resultDt.Rows)
        //    {
        //        try
        //        {
        //            StringBuilder sb2 = new StringBuilder();
        //            sb2.Append("insert into HZ_JCB(ZLDWDM,QSDWDM,QSXZ,GDLX,GDPDJB,GDZZSXDM, CZCSXM,TBXHDM,MSSM,FRDBS,")
        //            .Append("D0101,D0102,D0103,D0201,D0201K, D0202,D0202K, D0203,D0203K,D0204,D0204K,D0301,D0301K, D0302,D0302K,")
        //            .Append("D0303,D0304,D0305,D0306,D0307,D0307K,")
        //            .Append("D0401,D0402,D0403,D0403K,D0404,D05H1,D0508,D0601,D0602,D0603,D0701,D0702,")
        //             .Append("D08H1,D08H2,D08H2A,D0809,D0810,D0810A,D09,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //            .Append("D1101,D1102,D1103,D1104,D1104A,D1104K,D1105,D1106,D1107,D1107A,D1108,D1109,D1110,")
        //             .Append("D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //            .Append("values ('").Append(dr["ZLDWDM"].ToString()).Append("','").Append(dr["QSDWDM"].ToString()).Append("','")
        //            .Append(dr["QSXZ"].ToString()).Append("','").Append(dr["GDLX"].ToString()).Append("','").Append(dr["GDPDJB"].ToString()).Append("','")
        //            .Append(dr["GDZZSXDM"].ToString()).Append("','").Append(dr["CZCSXM"].ToString())
        //            .Append("','").Append(dr["TBXHDM"].ToString()).Append("','").Append(dr["MSSM"].ToString())
        //            .Append("','").Append(dr["FRDBS"].ToString())
        //            .Append("',").Append(String2Double(dr["D0101"].ToString())).Append(",").Append(String2Double(dr["D0102"].ToString())).Append(",").Append(String2Double(dr["D0103"].ToString())).Append(",").
        //            Append(String2Double(dr["D0201"].ToString())).Append(",").Append(String2Double(dr["D0201K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0202"].ToString())).Append(",").Append(String2Double(dr["D0202K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0203"].ToString())).Append(",").Append(String2Double(dr["D0203K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0204"].ToString())).Append(",").Append(String2Double(dr["D0204K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0301"].ToString())).Append(",").Append(String2Double(dr["D0301K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0302"].ToString())).Append(",").Append(String2Double(dr["D0302K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0303"].ToString())).Append(",").Append(String2Double(dr["D0304"].ToString())).Append(",").Append(String2Double(dr["D0305"].ToString())).Append(",").Append(String2Double(dr["D0306"].ToString())).Append(",").
        //            Append(String2Double(dr["D0307"].ToString())).Append(",").Append(String2Double(dr["D0307K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0401"].ToString())).Append(",").Append(String2Double(dr["D0402"].ToString())).Append(",").
        //            Append(String2Double(dr["D0403"].ToString())).Append(",").Append(String2Double(dr["D0403K"].ToString())).Append(",").
        //            Append(String2Double(dr["D0404"].ToString())).Append(",").
        //            Append(String2Double(dr["D05H1"].ToString())).Append(",").Append(String2Double(dr["D0508"].ToString())).Append(",").
        //            Append(String2Double(dr["D0601"].ToString())).Append(",").Append(String2Double(dr["D0602"].ToString())).Append(",").Append(String2Double(dr["D0603"].ToString())).Append(",").
        //            Append(String2Double(dr["D0701"].ToString())).Append(",").Append(String2Double(dr["D0702"].ToString())).Append(",").
        //            Append(String2Double(dr["D08H1"].ToString())).Append(",").Append(String2Double(dr["D08H2"].ToString())).Append(",").Append(String2Double(dr["D08H2A"].ToString())).Append(",").
        //            Append(String2Double(dr["D0809"].ToString())).Append(",").Append(String2Double(dr["D0810"].ToString())).Append(",").Append(String2Double(dr["D0810A"].ToString())).Append(",").
        //            Append(String2Double(dr["D09"].ToString())).Append(",").
        //            Append(String2Double(dr["D1001"].ToString())).Append(",").Append(String2Double(dr["D1002"].ToString())).Append(",").Append(String2Double(dr["D1003"].ToString())).Append(",").Append(String2Double(dr["D1004"].ToString())).Append(",").
        //            Append(String2Double(dr["D1005"].ToString())).Append(",").Append(String2Double(dr["D1006"].ToString())).Append(",").Append(String2Double(dr["D1007"].ToString())).Append(",").Append(String2Double(dr["D1008"].ToString())).Append(",").Append(String2Double(dr["D1009"].ToString())).Append(",").
        //            Append(String2Double(dr["D1101"].ToString())).Append(",").Append(String2Double(dr["D1102"].ToString())).Append(",").Append(String2Double(dr["D1103"].ToString())).Append(",").
        //            Append(String2Double(dr["D1104"].ToString())).Append(",").Append(String2Double(dr["D1104A"].ToString())).Append(",").Append(String2Double(dr["D1104K"].ToString())).Append(",").
        //            Append(String2Double(dr["D1105"].ToString())).Append(",").Append(String2Double(dr["D1106"].ToString())).Append(",").
        //            Append(String2Double(dr["D1107"].ToString())).Append(",").Append(String2Double(dr["D1107A"].ToString())).Append(",").
        //            Append(String2Double(dr["D1108"].ToString())).Append(",").Append(String2Double(dr["D1109"].ToString())).Append(",").Append(String2Double(dr["D1110"].ToString())).Append(",").

        //            Append(String2Double(dr["D1201"].ToString())).Append(",").Append(String2Double(dr["D1202"].ToString())).Append(",").
        //            Append(String2Double(dr["D1203"].ToString())).Append(",").Append(String2Double(dr["D1204"].ToString())).Append(",").Append(String2Double(dr["D1205"].ToString())).Append(",").
        //            Append(String2Double(dr["D1206"].ToString())).Append(",").
        //            Append(String2Double(dr["D1207"].ToString())).Append(" ) ");
        //            sql = sb2.ToString();
        //            int i = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //            if (i <= 0)
        //            {
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }


        //    }
        //    //计算小分类合计
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("update HZ_JCB set D0201HJ=iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K),")
        //        .Append(" D0202HJ=iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K),")
        //        .Append(" D0203HJ=iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K),")
        //        .Append(" D0204HJ=iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K),")
        //        .Append(" D0301HJ=iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K),")
        //        .Append(" D0302HJ=iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K),")
        //        .Append(" D0307HJ=iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K),")
        //        .Append(" D0403HJ=iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K),")
        //        .Append(" D08H2HJ=iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A),")
        //        .Append(" D0810HJ=iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A),")
        //        .Append(" D1104HJ=iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K),")
        //        .Append(" D1107HJ=iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A) ");
        //    sql = sb.ToString();
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //计算小计和合计
        //    sb.Clear();
        //    sb.Append("update HZ_JCB set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
        //         .Append(" D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
        //        .Append("D02=iif(isnull(D0201HJ),0,D0201HJ)+iif(isnull(D0202HJ),0,D0202HJ)+iif(isnull(D0203HJ),0,D0203HJ)+iif(isnull(D0204HJ),0,D0204HJ),")
        //        .Append("D03=iif(isnull(D0301HJ),0,D0301HJ)+iif(isnull(D0302HJ),0,D0302HJ)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307HJ),0,D0307HJ),")
        //        .Append("D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403HJ),0,D0403HJ)+iif(isnull(D0404),0,D0404),")
        //        .Append("D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
        //        .Append("D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
        //        .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
        //        .Append("D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2HJ),0,D08H2HJ)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810HJ),0,D0810HJ),")
        //        .Append("D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
        //        .Append("D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
        //        .Append("iif(isnull(D1104HJ),0,D1104HJ)+iif(isnull(D1107HJ),0,D1107HJ)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
        //        .Append("D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207)  ");

        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_JCB set TDZMJ=iif(isnull(D00),0,D00)+iif(isnull(D01),0,D01)+iif(isnull(D02),0,D02)+iif(isnull(D03),0,D03)+iif(isnull(D04),0,D04)")
        //        .Append("+iif(isnull(D05),0,D05)+iif(isnull(D06),0,D06)+iif(isnull(D07),0,D07)+ iif(isnull(D08),0,D08) ")
        //        .Append("+iif(isnull(D09),0,D09)+iif(isnull(D10),0,D10)+iif(isnull(D11),0,D11)+iif(isnull(D12),0,D12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sql = "update HZ_JCB set OLDAREA=TDZMJ ";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //}


        /// <summary>
        /// 获取所有海岛行政代码
        /// </summary>
        /// <returns></returns>
        private ArrayList getHD_XZQDM()
        {
            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "MSSM='01'";
            return FeatureHelper.GetUniqueFieldValueByDataStatistics(pFC, pQF, "XZQDM");
        }

       
        //初始化海岛土地分类面积
        //private void InitHDTable()
        //{
        //    string sql = "delete from HZ_HD_BZ ";
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_HD_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //        .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //        .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //        .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //        .Append(" select ZLDWDM,sum(TDZMJ),sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //        .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //        .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //        .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //        .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //        .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_JCB ")
        //        .Append(" where mssm='01'  ")
        //        .Append("group by ZLDWDM ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_HD_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append("select left(ZLDWDM,12),sum(TDZMJ),sum(D00), sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_HD_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12)");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_HD_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append("select left(ZLDWDM,9),sum(TDZMJ),sum(D00), sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_HD_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,9)");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_HD_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append(" select left(ZLDWDM,6),sum(TDZMJ),sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207)  from HZ_HD_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,6)");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //}

        //可调整地类汇总
        //private void InitKtzTable()
        //{
        //    string sql = "delete from HZ_JBNTWKTZ_BZ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_JBNTWKTZ_BZ(ZLDWDM,D0201K,D0202K,D0203K,D0204K,D0301K,D0302K,D0307K,D0403K,D1104K) ")
        //    .Append(" select ZLDWDM,sum(D0201K),sum(D0202K),sum(D0203K),sum(D0204K),sum(D0301K),sum(D0302K),sum(D0307K),")
        //    .Append("sum(D0403K),sum(D1104K) from HZ_JCB  group by ZLDWDM ");
        //    sql = sb.ToString();
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb = new StringBuilder();
        //    sb.Append("update HZ_JBNTWKTZ_BZ set DKHJ=iif(isnull(D0201K),0,D0201K)+iif(isnull(D0202K),0,D0202K)+iif(isnull(D0203K),0,D0203K)+iif(isnull(D0204K),0,D0204K)+")
        //        .Append("iif(isnull(D0301K),0,D0301K)+iif(isnull(D0302K),0,D0302K)+iif(isnull(D0307K),0,D0307K)+iif(isnull(D0403K),0,D0403K)+iif(isnull(D1104K),0,D1104K) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //村级
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_JBNTWKTZ_BZ(ZLDWDM,DKHJ,D0201K,D0202K,D0203K,D0204K,D0301K,D0302K,D0307K,D0403K,D1104K) ")
        //        .Append(" select left(ZLDWDM,12),sum(DKHJ),sum(D0201K),sum(D0202K),sum(D0203K),sum(D0204K),sum(D0301K),sum(D0302K),sum(D0307K),sum(D0403K),sum(D1104K) ")
        //        .Append(" from HZ_JBNTWKTZ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //乡级
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_JBNTWKTZ_BZ(ZLDWDM,DKHJ,D0201K,D0202K,D0203K,D0204K,D0301K,D0302K,D0307K,D0403K,D1104K) ")
        //        .Append(" select left(ZLDWDM,9),sum(DKHJ),sum(D0201K),sum(D0202K),sum(D0203K),sum(D0204K),sum(D0301K),sum(D0302K),sum(D0307K),sum(D0403K),sum(D1104K) ")
        //        .Append(" from HZ_JBNTWKTZ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_JBNTWKTZ_BZ(ZLDWDM,DKHJ,D0201K,D0202K,D0203K,D0204K,D0301K,D0302K,D0307K,D0403K,D1104K) ")
        //        .Append(" select left(ZLDWDM,6),sum(DKHJ),sum(D0201K),sum(D0202K),sum(D0203K),sum(D0204K),sum(D0301K),sum(D0302K),sum(D0307K),sum(D0403K),sum(D1104K) ")
        //        .Append(" from HZ_JBNTWKTZ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //}
       
        ////标准表
        //private void InitZlTable()
        //{
        //    //guojie 2017-12-20日修改，地类修改
        //    string sql = "delete from HZ_ZL_BZ ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_ZL_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201, D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append("  select ZLDWDM,sum(TDZMJ) ,sum(D00), sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201HJ),sum(D0202HJ),sum(D0203HJ),sum(D0204HJ),")
        //    .Append("sum(D03),sum(D0301HJ),sum(D0302HJ),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307HJ),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403HJ),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2HJ),sum(D0809),sum(D0810HJ),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104HJ),sum(D1105),sum(D1106),sum(D1107HJ),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_JCB ")
        //    .Append("group by ZLDWDM ");
        //    sql = sb.ToString();
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //村
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_ZL_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append("  select  left(ZLDWDM,12),sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
        //    .Append("sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) ")
        //    .Append(" from HZ_ZL_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12)");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb=new StringBuilder();
        //    sb.Append("insert into HZ_ZL_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append("  select  left(ZLDWDM,9),sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
        //    .Append("sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) ")
        //    .Append(" from HZ_ZL_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,9)");
        //    sql=sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb=new StringBuilder();
        //    sb.Append("insert into HZ_ZL_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append("select  left(ZLDWDM,6),sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
        //    .Append("sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) ")
        //    .Append(" from HZ_ZL_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,6)");
        //    sql=sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);            
            
        //}

        ////部分细化地类
        //private void InitBfxhdlTable()
        //{
        //    //从tmp中，直接 汇总过来，换算为公顷即可
        //    string sql = "delete from HZ_BFXHDL ";
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sql = "insert into HZ_BFXHDL(ZLDWDM,ZLDWMC) select distinct ZLDWDM,ZLDWMC from HZ_TMP  ";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sql = "select ZLDWDM,DLBM, sum(PMMJ) as mj  from HZ_TMP where right(DLBM,1)='A' group by ZLDWDM,DLBM ";
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "hz");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string zldwdm = dr["ZLDWDM"].ToString().Trim();
        //        string dlbm=dr["DLBM"].ToString().Trim();
        //        double mj=0;
        //        double.TryParse(dr["mj"].ToString(),out mj);
        //        mj = MathHelper.Round(mj / 10000, 2);
        //        sql = "update HZ_BFXHDL set D" + dlbm + " = " + mj+" where ZLDWDM='"+zldwdm+"' ";
        //        iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    }

        //    sql = "select ZLDWDM,CZCSXM,sum(PMMJ) as mj from HZ_TMP where right(CZCSXM,1)='A' group by ZLDWDM,CZCSXM ";
        //    DataTable dt2 = LS_ResultMDBHelper.GetDataTable(sql, "hz");
        //    foreach (DataRow dr in dt2.Rows)
        //    {
        //        string zldwdm = dr["ZLDWDM"].ToString().Trim();
        //        string czcsxm = dr["CZCSXM"].ToString().Trim();
        //        double mj = 0;
        //        double.TryParse(dr["mj"].ToString(), out mj);
        //        mj = MathHelper.Round(mj / 10000, 2);
        //        sql = "update HZ_BFXHDL set D" + czcsxm + " = " + mj + " where ZLDWDM='" + zldwdm + "' ";
        //        iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    }
        //    //计算小计
        //    sql = "update HZ_BFXHDL set D20A=iif(isnull(D201A),0,D201A)+iif(isnull(D202A),0,D202A)+iif(isnull(D203A),0,D203A) ";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //汇总
        //    StringBuilder sb=new StringBuilder();
        //    sb.Append("insert into HZ_BFXHDL(ZLDWDM,D08H2A,D0810A,D1104A,D1107A,D20A,D201A,D202A,D203A) ")
        //    .Append("select left(ZLDWDM,12),sum(D08H2A),sum(D0810A),sum(D1104A),sum(D1107A),sum(D20A),sum(D201A),sum(D202A),sum(D203A) from HZ_BFXHDL ")
        //    .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_BFXHDL(ZLDWDM,D08H2A,D0810A,D1104A,D1107A,D20A,D201A,D202A,D203A) ")
        //    .Append("select left(ZLDWDM,9),sum(D08H2A),sum(D0810A),sum(D1104A),sum(D1107A),sum(D20A),sum(D201A),sum(D202A),sum(D203A) from HZ_BFXHDL ")
        //    .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_BFXHDL(ZLDWDM,D08H2A,D0810A,D1104A,D1107A,D20A,D201A,D202A,D203A) ")
        //    .Append("select left(ZLDWDM,6),sum(D08H2A),sum(D0810A),sum(D1104A),sum(D1107A),sum(D20A),sum(D201A),sum(D202A),sum(D203A) from HZ_BFXHDL ")
        //    .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //}

        ////飞地的权属
        //private void InitFDQSTable()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    string sql = "delete from HZ_FDQS_BZ ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sql = "insert into HZ_FDQS_BZ(ZLDWDM,QSDWDM,TOTALAREA) select ZLDWDM,QSDWDM ,sum(TDZMJ) from HZ_JCB where FRDBS='1'  group by ZLDWDM,QSDWDM ";
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //修改国有
        //    sql = "select ZLDWDM,QSDWDM,sum(TDZMJ)as gjzmj,sum(D00) as D00, sum(D01) as D01,sum(D02) as D02,sum(D03) as D03,sum(D04)as D04,"
        //    + "sum(D05) as D05,sum(D06) as D06,sum(D07) as D07,sum(D08) as D08,sum(D09) as D09,sum(D10) as D10,sum(D11) as D11,sum(D12) as D12 "
        //    + " from HZ_JCB where  FRDBS='1'  and  QSXZ <'30' group by zldwdm,qsdwdm ";
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double zmj = 0,d00=0, d010 = 0, d020 = 0, d030 = 0, d040 = 0, d050 = 0, d060 = 0, d070 = 0, d080 = 0, d090 = 0, d100 = 0, d110 = 0, d120 = 0;
        //        double.TryParse(dr["gjzmj"].ToString(), out zmj);
        //        double.TryParse(dr["D00"].ToString(), out d00);
        //        double.TryParse(dr["D01"].ToString(), out d010);
        //        double.TryParse(dr["D02"].ToString(), out d020);
        //        double.TryParse(dr["D03"].ToString(), out d030);
        //        double.TryParse(dr["D04"].ToString(), out d040);
        //        double.TryParse(dr["D05"].ToString(), out d050);
        //        double.TryParse(dr["D06"].ToString(), out d060);
        //        double.TryParse(dr["D07"].ToString(), out d070);
        //        double.TryParse(dr["D08"].ToString(), out d080);
        //        double.TryParse(dr["D09"].ToString(), out d090);
        //        double.TryParse(dr["D10"].ToString(), out d100);
        //        double.TryParse(dr["D11"].ToString(), out d110);
        //        double.TryParse(dr["D12"].ToString(), out d120);

        //        sb = new StringBuilder();
        //        sb.Append("update HZ_FDQS_BZ set TOTALAREAG=").Append(zmj).Append(",D00G=").Append(d00).Append(",D01G=").Append(d010).Append(",")
        //            .Append("D02G=").Append(d020).Append(",D03G=").Append(d030).Append(",D04G=").Append(d040).Append(",")
        //            .Append("D05G=").Append(d050).Append(",D06G=").Append(d060).Append(",D07G=").Append(d070).Append(",")
        //            .Append("D08G=").Append(d080).Append(",D09G=").Append(d090).Append(",D10G=").Append(d100).Append(",")
        //            .Append("D11G=").Append(d110).Append(",D12G=").Append(d120).Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' and QSDWDM='")
        //            .Append(dr["QSDWDM"].ToString()).Append("' ");
        //        sql = sb.ToString();
        //        iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }
        //    //修改集体
        //    sql = "select ZLDWDM,QSDWDM,sum(TDZMJ)as jtzmj,sum(D00) as D00,sum(D01) as D01,sum(D02) as D02,sum(D03) as D03,sum(D04)as D04,"
        //    + "sum(D05) as D05,sum(D06) as D06,sum(D07) as D07,sum(D08) as D08,sum(D09) as D09,sum(D10) as D10,sum(D11) as D11,sum(D12) as D12 "
        //    + " from HZ_JCB where  FRDBS='1'  and QSXZ >'20' group by zldwdm,qsdwdm ";

        //    dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double zmj = 0, d00=0,d010 = 0, d020 = 0, d030 = 0, d040 = 0, d050 = 0, d060 = 0, d070 = 0, d080 = 0, d090 = 0, d100 = 0, d110 = 0, d120 = 0;
        //        double.TryParse(dr["jtzmj"].ToString(), out zmj);
        //        double.TryParse(dr["D00"].ToString(), out d00);
        //        double.TryParse(dr["D01"].ToString(), out d010);
        //        double.TryParse(dr["D02"].ToString(), out d020);
        //        double.TryParse(dr["D03"].ToString(), out d030);
        //        double.TryParse(dr["D04"].ToString(), out d040);
        //        double.TryParse(dr["D05"].ToString(), out d050);
        //        double.TryParse(dr["D06"].ToString(), out d060);
        //        double.TryParse(dr["D07"].ToString(), out d070);
        //        double.TryParse(dr["D08"].ToString(), out d080);
        //        double.TryParse(dr["D09"].ToString(), out d090);
        //        double.TryParse(dr["D10"].ToString(), out d100);
        //        double.TryParse(dr["D11"].ToString(), out d110);
        //        double.TryParse(dr["D12"].ToString(), out d120);

        //        sb = new StringBuilder();
        //        sb.Append("update HZ_FDQS_BZ set TOTALAREAJ=").Append(zmj).Append(",D00J=").Append(d00).Append(", D01J=").Append(d010).Append(",")
        //            .Append("D02J=").Append(d020).Append(",D03J=").Append(d030).Append(",D04J=").Append(d040).Append(",")
        //            .Append("D05J=").Append(d050).Append(",D06J=").Append(d060).Append(",D07J=").Append(d070).Append(",")
        //            .Append("D08J=").Append(d080).Append(",D09J=").Append(d090).Append(",D10J=").Append(d100).Append(",")
        //            .Append("D11J=").Append(d110).Append(",D12J=").Append(d120).Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' and QSDWDM='")
        //            .Append(dr["QSDWDM"].ToString()).Append("' ");
        //        sql = sb.ToString();
        //        iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }

        //    //计算合计
        //    sb = new StringBuilder();
        //    sb.Append(" update HZ_FDQS_BZ set D00=iif(isnull(D00G),0,D00G)+iif(isnull(D00J),0,D00J),")
        //        .Append(" D01=iif(isnull(D01G),0,D01G)+iif(isnull(D01J),0,D01J),")
        //        .Append(" D02=iif(isnull(D02G),0,D02G)+iif(isnull(D02J),0,D02J),")
        //        .Append(" D03=iif(isnull(D03G),0,D03G)+iif(isnull(D03J),0,D03J),")
        //        .Append(" D04=iif(isnull(D04G),0,D04G)+iif(isnull(D04J),0,D04J),")

        //         .Append(" D05=iif(isnull(D05G),0,D05G)+iif(isnull(D05J),0,D05J),")
        //          .Append(" D06=iif(isnull(D06G),0,D06G)+iif(isnull(D06J),0,D06J),")
        //         .Append(" D07=iif(isnull(D07G),0,D07G)+iif(isnull(D07J),0,D07J),")
        //         .Append(" D08=iif(isnull(D08G),0,D08G)+iif(isnull(D08J),0,D08J),")
        //         .Append(" D09=iif(isnull(D09G),0,D09G)+iif(isnull(D09J),0,D09J),")

        //        .Append(" D10=iif(isnull(D10G),0,D10G)+iif(isnull(D10J),0,D10J),")
        //        .Append(" D11=iif(isnull(D11G),0,D11G)+iif(isnull(D11J),0,D11J),")
        //        .Append(" D12=iif(isnull(D12G),0,D12G)+iif(isnull(D12J),0,D12J) ");
        //    sql = sb.ToString();
        //    int iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //村汇总的时候 从已有的里面 加 村 不相等条件
        //    sb = new StringBuilder();
        //    sb.Append(" insert into HZ_FDQS_BZ(ZLDWDM,QSDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D00G,D00J,D01,D01G,D01J,D02,D02G,D02J,")
        //    .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

        //    .Append(" select left(ZLDWDM,12),left(QSDWDM,12), sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D00G),sum(D00J),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
        //    .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
        //    .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
        //    .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_FDQS_BZ ")
        //    .Append(" where len(ZLDWDM)=19  and left(ZLDWDM,12)<>left(QSDWDM,12)  group by left(ZLDWDM,12),left(QSDWDM,12)");
        //    sql = sb.ToString();
        //    iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //乡汇总
        //    sb = new StringBuilder();
        //    sb.Append(" insert into HZ_FDQS_BZ(ZLDWDM,QSDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D00G,D00J,D01,D01G,D01J,D02,D02G,D02J,")
        //    .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

        //    .Append(" select left(ZLDWDM,9),left(QSDWDM,9), sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D00G),sum(D00J),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
        //    .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
        //    .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
        //    .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_FDQS_BZ ")
        //    .Append(" where len(ZLDWDM)=19  and left(ZLDWDM,9)<>left(QSDWDM,9)  group by left(ZLDWDM,9),left(QSDWDM,9)");
        //    sql = sb.ToString();
        //    iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //县汇总
        //    sb = new StringBuilder();
        //    sb.Append(" insert into HZ_FDQS_BZ(ZLDWDM,QSDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D00G,D00J,D01,D01G,D01J,D02,D02G,D02J,")
        //    .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

        //    .Append(" select left(ZLDWDM,6),left(QSDWDM,6), sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D00G),sum(D00J),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
        //    .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
        //    .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
        //    .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_FDQS_BZ ")
        //    .Append(" where len(ZLDWDM)=19  and left(ZLDWDM,6)<>left(QSDWDM,6)  group by left(ZLDWDM,6),left(QSDWDM,6)");
        //    sql = sb.ToString();
        //    iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //}

        ////权属性质
        //private void InitQsTable()
        //{
            
        //    string sql = "delete from HZ_QS_BZ ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_QS_BZ(ZLDWDM,TOTALAREA) select ZLDWDM ,sum(TDZMJ) from HZ_JCB group by ZLDWDM ");
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());
        //    // 修改国有的
        //    sql="select ZLDWDM,sum(TDZMJ)as gjzmj,sum(D00) as D00,sum(D01) as D01,sum(D02) as D02,sum(D03) as D03,sum(D04)as D04,"
        //    +"sum(D05) as D05,sum(D06) as D06,sum(D07) as D07,sum(D08) as D08,sum(D09) as D09,sum(D10) as D10,sum(D11) as D11,sum(D12) as D12 "
        //    +" from HZ_JCB where QSXZ <'30' group by zldwdm ";

        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double zmj=0,d00=0,d010=0,d020=0,d030=0,d040=0,d050=0,d060=0,d070=0,d080=0,d090=0,d100=0,d110=0,d120=0;
        //        double.TryParse(dr["gjzmj"].ToString(),out zmj);
        //        double.TryParse(dr["D00"].ToString(), out d00);
        //        double.TryParse(dr["D01"].ToString(),out d010);
        //        double.TryParse(dr["D02"].ToString(),out d020);
        //        double.TryParse(dr["D03"].ToString(),out d030);
        //        double.TryParse(dr["D04"].ToString(),out d040);
        //        double.TryParse(dr["D05"].ToString(),out d050);
        //        double.TryParse(dr["D06"].ToString(),out d060);
        //        double.TryParse(dr["D07"].ToString(), out d070);
        //        double.TryParse(dr["D08"].ToString(), out d080);
        //        double.TryParse(dr["D09"].ToString(), out d090);
        //        double.TryParse(dr["D10"].ToString(), out d100);
        //        double.TryParse(dr["D11"].ToString(),out d110);
        //        double.TryParse(dr["D12"].ToString(),out d120);

        //        sb = new StringBuilder();
        //        sb.Append("update HZ_QS_BZ set TOTALAREAG=").Append(zmj).Append(",D00G=").Append(d00).Append(",D01G=").Append(d010).Append(",")
        //            .Append("D02G=").Append(d020).Append(",D03G=").Append(d030).Append(",D04G=").Append(d040).Append(",")
        //            .Append("D05G=").Append(d050).Append(",D06G=").Append(d060).Append(",D07G=").Append(d070).Append(",")
        //            .Append("D08G=").Append(d080).Append(",D09G=").Append(d090).Append(",D10G=").Append(d100).Append(",")                    
        //            .Append("D11G=").Append(d110).Append(",D12G=").Append(d120).Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //        sql = sb.ToString();
        //        int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }
        //    //修改集体
        //    sql = "select ZLDWDM,sum(TDZMJ)as jtzmj,sum(D00) as D00,sum(D01) as D01,sum(D02) as D02,sum(D03) as D03,sum(D04)as D04,"
        //    +"sum(D05) as D05,sum(D06) as D06,sum(D07) as D07,sum(D08) as D08,sum(D09) as D09,sum(D10) as D10,sum(D11) as D11,sum(D12) as D12 "
        //    +" from HZ_JCB where QSXZ >'20' group by zldwdm ";

        //    dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        double zmj = 0,d00=0, d010 = 0, d020 = 0, d030 = 0, d040 = 0, d050 = 0, d060 = 0, d070 = 0, d080 = 0, d090 = 0, d100 = 0, d110 = 0, d120 = 0;
        //        double.TryParse(dr["jtzmj"].ToString(), out zmj);
        //        double.TryParse(dr["D00"].ToString(), out d00);
        //        double.TryParse(dr["D01"].ToString(), out d010);
        //        double.TryParse(dr["D02"].ToString(), out d020);
        //        double.TryParse(dr["D03"].ToString(), out d030);
        //        double.TryParse(dr["D04"].ToString(), out d040);
        //        double.TryParse(dr["D05"].ToString(), out d050);
        //        double.TryParse(dr["D06"].ToString(), out d060);
        //        double.TryParse(dr["D07"].ToString(), out d070);
        //        double.TryParse(dr["D08"].ToString(), out d080);
        //        double.TryParse(dr["D09"].ToString(), out d090);
        //        double.TryParse(dr["D10"].ToString(), out d100);
        //        double.TryParse(dr["D11"].ToString(), out d110);
        //        double.TryParse(dr["D12"].ToString(), out d120);

        //        sb = new StringBuilder();
        //        sb.Append("update HZ_QS_BZ set TOTALAREAJ=").Append(zmj).Append(",D00J=").Append(d00).Append(",D01J=").Append(d010).Append(",")
        //            .Append("D02J=").Append(d020).Append(",D03J=").Append(d030).Append(",D04J=").Append(d040).Append(",")
        //            .Append("D05J=").Append(d050).Append(",D06J=").Append(d060).Append(",D07J=").Append(d070).Append(",")
        //            .Append("D08J=").Append(d080).Append(",D09J=").Append(d090).Append(",D10J=").Append(d100).Append(",")
        //            .Append("D11J=").Append(d110).Append(",D12J=").Append(d120).Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //        sql = sb.ToString();
        //        int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }

        //    //计算合计
        //    sb = new StringBuilder();
        //    sb.Append(" update HZ_QS_BZ set  D00=iif(isnull(D00G),0,D00G)+iif(isnull(D00J),0,D00J),  D01=iif(isnull(D01G),0,D01G)+iif(isnull(D01J),0,D01J),")
        //        .Append(" D02=iif(isnull(D02G),0,D02G)+iif(isnull(D02J),0,D02J),")
        //        .Append(" D03=iif(isnull(D03G),0,D03G)+iif(isnull(D03J),0,D03J),")
        //        .Append(" D04=iif(isnull(D04G),0,D04G)+iif(isnull(D04J),0,D04J),")

        //         .Append(" D05=iif(isnull(D05G),0,D05G)+iif(isnull(D05J),0,D05J),")
        //          .Append(" D06=iif(isnull(D06G),0,D06G)+iif(isnull(D06J),0,D06J),")
        //         .Append(" D07=iif(isnull(D07G),0,D07G)+iif(isnull(D07J),0,D07J),")
        //         .Append(" D08=iif(isnull(D08G),0,D08G)+iif(isnull(D08J),0,D08J),")
        //         .Append(" D09=iif(isnull(D09G),0,D09G)+iif(isnull(D09J),0,D09J),")

        //        .Append(" D10=iif(isnull(D10G),0,D10G)+iif(isnull(D10J),0,D10J),")
        //        .Append(" D11=iif(isnull(D11G),0,D11G)+iif(isnull(D11J),0,D11J),")
        //        .Append(" D12=iif(isnull(D12G),0,D12G)+iif(isnull(D12J),0,D12J) ");
        //    sql = sb.ToString();
        //    int iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append(" insert into HZ_QS_BZ(ZLDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D01,D01G,D01J,D02,D02G,D02J,")
        //    .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

        //    .Append(" select left(ZLDWDM,12),sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00), sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
        //    .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
        //    .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
        //    .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_QS_BZ ")
        //    .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,12)");
        //    sql = sb.ToString();
        //    iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //逐级汇总
        //    sb = new StringBuilder();
        //    sb.Append(" insert into HZ_QS_BZ(ZLDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D01,D01G,D01J,D02,D02G,D02J,")
        //    .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

        //    .Append(" select left(ZLDWDM,9),sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
        //    .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
        //    .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
        //    .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_QS_BZ ")
        //    .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,9)");
        //    sql = sb.ToString();
        //    iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append(" insert into HZ_QS_BZ(ZLDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D00G,D00J,D01,D01G,D01J,D02,D02G,D02J,")
        //    .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

        //    .Append(" select left(ZLDWDM,6),sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D00G),sum(D00J),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
        //    .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
        //    .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
        //    .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_QS_BZ ")
        //    .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,6)");
        //    sql = sb.ToString();
        //    iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //}

        //private void InitGdTable()
        //{
        //    string sql = "delete from HZ_GD_BZ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_GD_BZ(ZLDWDM,TOTALAREA) select ZLDWDM ,sum(D01) from HZ_JCB group by ZLDWDM ");
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString()); //插入耕地总面积

        //    //GDLX 为T 的 为 梯田，坡度级别  1 2 3 4 5 分别对应2度以下，2-6 6-15 15-25 25以上
        //    sql = "select ZLDWDM,GDLX,GDPDJB,sum(D01) as mj from HZ_JCB where GDPDJB<>''  group by ZLDWDM,GDLX,GDPDJB ";
        //    DataTable dt2 = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr2 in dt2.Rows)
        //    {
        //        string zldwdm=dr2["ZLDWDM"].ToString();
        //        string gdpdj = dr2["GDPDJB"].ToString().Trim(); //耕地坡度级别
        //        double dmj = 0;
        //        double.TryParse(dr2["mj"].ToString(), out dmj);
        //        string gdlx = dr2["GDLX"].ToString().Trim().ToUpper();
        //        switch (gdpdj)
        //        {
        //            case "1":
        //                sql = "update HZ_GD_BZ set D2=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //                break;
        //            case "2":
        //                if (gdlx == "T")
        //                {
        //                    sql = "update HZ_GD_BZ set D26T=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                else
        //                {
        //                    sql = "update HZ_GD_BZ set D26P=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //                break;
        //            case "3":
        //                if (gdlx == "T")
        //                {
        //                    sql = "update HZ_GD_BZ set D615T=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                else
        //                {
        //                    sql = "update HZ_GD_BZ set D615P=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //                break;
        //            case "4":
        //                if (gdlx == "T")
        //                {
        //                    sql = "update HZ_GD_BZ set D1525T=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                else
        //                {
        //                    sql = "update HZ_GD_BZ set D1525P=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //                break;
        //            case "5":
        //                if (gdlx == "T")
        //                {
        //                    sql = "update HZ_GD_BZ set D25T=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                else
        //                {
        //                    sql = "update HZ_GD_BZ set D25P=" + dmj + " where ZLDWDM='" + zldwdm + "' ";
        //                }
        //                LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //                break;
        //        }
        //    }

        //    //计算小计
        //    sql = "update HZ_GD_BZ set D26=iif(isnull(D26T),0,D26T)+iif(isnull(D26P),0,D26P),D615=iif(isnull(D615T),0,D615T)+iif(isnull(D615P),0,D615P),"
        //        + " D1525=iif(isnull(D1525T),0,D1525T)+iif(isnull(D1525P),0,D1525P),D25=iif(isnull(D25P),0,D25P)+iif(isnull(D25T),0,D25T) ";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    sql = " insert into HZ_GD_BZ(ZLDWDM,TOTALAREA,D2,D26,D26T,D26P,D615,D615T,D615P,D1525,D1525T,D1525P,D25,D25T,D25P) "
        //    + " select left(ZLDWDM,12), sum(TOTALAREA),sum(D2),sum(D26),sum(D26T),sum(D26P),sum(D615),sum(D615T),sum(D615P),sum(D1525),sum(D1525T),sum(D1525P),sum(D25),sum(D25T),sum(D25P) from HZ_GD_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12)";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sql = " insert into HZ_GD_BZ(ZLDWDM,TOTALAREA,D2,D26,D26T,D26P,D615,D615T,D615P,D1525,D1525T,D1525P,D25,D25T,D25P) "
        //    +" select left(ZLDWDM,9), sum(TOTALAREA),sum(D2),sum(D26),sum(D26T),sum(D26P),sum(D615),sum(D615T),sum(D615P),sum(D1525),sum(D1525T),sum(D1525P),sum(D25),sum(D25T),sum(D25P) from HZ_GD_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,9)";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sql="insert into HZ_GD_BZ(ZLDWDM,TOTALAREA,D2,D26,D26T,D26P,D615,D615T,D615P,D1525,D1525T,D1525P,D25,D25T,D25P) "
        //        +" select left(ZLDWDM,6), sum(TOTALAREA),sum(D2),sum(D26),sum(D26T),sum(D26P),sum(D615),sum(D615T),sum(D615P),sum(D1525),sum(D1525T),sum(D1525P),sum(D25),sum(D25T),sum(D25P) from HZ_GD_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,6)";
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //}


        //private void InitFd_CZCGKTable()
        //{
        //    string sql = "delete from HZ_FDCZCGK_BZ1";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sql = "delete from HZ_FDCZCGK_BZ2";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sql = "delete from HZ_FDCZCGK_BZ3";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_FDCZCGK_BZ1(ZLDWDM,QSDWDM) select distinct ZLDWDM,QSDWDM  from HZ_JCB where FRDBS='1'  ");
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString()); //插入总面积

        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery("insert into HZ_FDCZCGK_BZ2(ZLDWDM,QSDWDM) select distinct ZLDWDM,QSDWDM  from HZ_JCB  where FRDBS='1' ");
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery("insert into HZ_FDCZCGK_BZ3(ZLDWDM,QSDWDM) select distinct ZLDWDM,QSDWDM  from HZ_JCB  where FRDBS='1' ");

        //    sb = new StringBuilder();
        //    sb.Append("  select ZLDWDM,QSDWDM,left(CZCSXM,3) as BZ,sum(TDZMJ) as TDZMJ ,sum(D00) as D00,sum(D01) as D01,sum(D0101) as D0101,sum(D0102) as D0102,sum(D0103) as D0103,")
        //    .Append("sum(D02) as D02,sum(D0201HJ) as D0201,sum(D0202HJ) as D0202,sum(D0203HJ) as D0203,sum(D0204HJ) as D0204,")
        //    .Append("sum(D03) as D03,sum(D0301HJ) as D0301,sum(D0302HJ) as D0302,sum(D0303)as D0303,sum(D0304) as D0304,sum(D0305) as D0305,sum(D0306) as D0306,sum(D0307HJ) as D0307,")
        //    .Append("sum(D04) as D04,sum(D0401) as D0401,sum(D0402) as D0402,sum(D0403HJ) as D0403,sum(D0404) as D0404,sum(D05) as D05,")
        //    .Append("sum(D05H1) as D05H1,sum(D0508) as D0508,")
        //    .Append("sum(D06) as  D06,sum(D0601) as D0601,sum(D0602) as D0602,sum(D0603) as D0603,sum(D07) as D07,sum(D0701) as D0701,sum(D0702) as D0702,")
        //    .Append("sum(D08) as D08,sum(D08H1) as D08H1,sum(D08H2HJ) as D08H2,sum(D0809) as D0809,sum(D0810HJ) as D0810,sum(D09) as D09,sum(D10) as D10,")
        //    .Append("sum(D1001) as D1001,sum(D1002) as D1002,sum(D1003) as D1003,sum(D1004) as D1004,sum(D1005) as D1005,sum(D1006) as D1006,sum(D1007) as D1007,sum(D1008) as D1008,sum(D1009) as D1009,")
        //    .Append("sum(D11) as D11,sum(D1101) as D1101,sum(D1102) as D1102,sum(D1103) as D1103,sum(D1104HJ) as D1104,sum(D1105) as D1105,sum(D1106) as D1106,sum(D1107HJ) as D1107,sum(D1108) as D1108,sum(D1109) as D1109,sum(D1110) as D1110,")
        //   .Append("sum(D12) as D12,sum(D1201) as D1201,sum(D1202) as D1202,sum(D1203) as D1203,sum(D1204) as D1204,sum(D1205) as D1205,sum(D1206) as D1206,sum(D1207) as D1207 from HZ_JCB ")
        //   .Append("where FRDBS='1'   group by ZLDWDM,QSDWDM,left(CZCSXM,3) ");
        //    sql = sb.ToString();
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        #region 逐条修改
        //        string bz = dr["BZ"].ToString().Trim(); //201 202 203 204 205
        //        if (bz == "")
        //            continue;
        //        sb = new StringBuilder();
        //        try
        //        {
        //            switch (bz.ToUpper())
        //            {
        //                case "201":
        //                    sb.Clear();
        //                    sb.Append("update HZ_FDCZCGK_BZ1 set D").Append(bz).Append("00=").Append(dr["D00"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("01=").Append(dr["D01"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0101=").Append(dr["D0101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0102=").Append(dr["D0102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0103=").Append(dr["D0103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("02=").Append(dr["D02"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0201=").Append(dr["D0201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0202=").Append(dr["D0202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0203=").Append(dr["D0203"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0204=").Append(dr["D0204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("03=").Append(dr["D03"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0301=").Append(dr["D0301"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0302=").Append(dr["D0302"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0303=").Append(dr["D0303"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0304=").Append(dr["D0304"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0305=").Append(dr["D0305"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0306=").Append(dr["D0306"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0307=").Append(dr["D0307"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("04=").Append(dr["D04"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0401=").Append(dr["D0401"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0402=").Append(dr["D0402"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0403=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0404=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05=").Append(dr["D05"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05H1=").Append(dr["D05H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0508=").Append(dr["D0508"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("06=").Append(dr["D06"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0601=").Append(dr["D0601"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0602=").Append(dr["D0602"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0603=").Append(dr["D0603"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("07=").Append(dr["D07"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0701=").Append(dr["D0701"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0702=").Append(dr["D0702"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08=").Append(dr["D08"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H1=").Append(dr["D08H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H2=").Append(dr["D08H2"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0809=").Append(dr["D0809"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0810=").Append(dr["D0810"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("09=").Append(dr["D09"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("10=").Append(dr["D10"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1001=").Append(dr["D1001"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1002=").Append(dr["D1002"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1003=").Append(dr["D1003"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1004=").Append(dr["D1004"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1005=").Append(dr["D1005"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1006=").Append(dr["D1006"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1007=").Append(dr["D1007"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1008=").Append(dr["D1008"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1009=").Append(dr["D1009"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("11=").Append(dr["D11"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1101=").Append(dr["D1101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1102=").Append(dr["D1102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1103=").Append(dr["D1103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1104=").Append(dr["D1104"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1105=").Append(dr["D1105"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1106=").Append(dr["D1106"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1107=").Append(dr["D1107"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1108=").Append(dr["D1108"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1109=").Append(dr["D1109"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1110=").Append(dr["D1110"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("12=").Append(dr["D12"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1201=").Append(dr["D1201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1202=").Append(dr["D1202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1203=").Append(dr["D1204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1205=").Append(dr["D1206"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1207=").Append(dr["D1207"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("'  and QSDWDM='")
        //                        .Append(dr["QSDWDM"].ToString().Trim()).Append("' " ) ;
        //                    sql = sb.ToString();
        //                    break;
        //                case "202":
        //                    sb.Clear();
        //                    sb.Append("update HZ_FDCZCGK_BZ2 set D").Append(bz).Append("00=").Append(dr["D00"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("01=").Append(dr["D01"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0101=").Append(dr["D0101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0102=").Append(dr["D0102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0103=").Append(dr["D0103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("02=").Append(dr["D02"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0201=").Append(dr["D0201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0202=").Append(dr["D0202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0203=").Append(dr["D0203"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0204=").Append(dr["D0204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("03=").Append(dr["D03"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0301=").Append(dr["D0301"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0302=").Append(dr["D0302"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0303=").Append(dr["D0303"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0304=").Append(dr["D0304"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0305=").Append(dr["D0305"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0306=").Append(dr["D0306"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0307=").Append(dr["D0307"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("04=").Append(dr["D04"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0401=").Append(dr["D0401"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0402=").Append(dr["D0402"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0403=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0404=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05=").Append(dr["D05"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05H1=").Append(dr["D05H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0508=").Append(dr["D0508"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("06=").Append(dr["D06"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0601=").Append(dr["D0601"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0602=").Append(dr["D0602"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0603=").Append(dr["D0603"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("07=").Append(dr["D07"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0701=").Append(dr["D0701"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0702=").Append(dr["D0702"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08=").Append(dr["D08"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H1=").Append(dr["D08H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H2=").Append(dr["D08H2"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0809=").Append(dr["D0809"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0810=").Append(dr["D0810"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("09=").Append(dr["D09"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("10=").Append(dr["D10"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1001=").Append(dr["D1001"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1002=").Append(dr["D1002"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1003=").Append(dr["D1003"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1004=").Append(dr["D1004"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1005=").Append(dr["D1005"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1006=").Append(dr["D1006"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1007=").Append(dr["D1007"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1008=").Append(dr["D1008"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1009=").Append(dr["D1009"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("11=").Append(dr["D11"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1101=").Append(dr["D1101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1102=").Append(dr["D1102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1103=").Append(dr["D1103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1104=").Append(dr["D1104"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1105=").Append(dr["D1105"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1106=").Append(dr["D1106"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1107=").Append(dr["D1107"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1108=").Append(dr["D1108"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1109=").Append(dr["D1109"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1110=").Append(dr["D1110"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("12=").Append(dr["D12"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1201=").Append(dr["D1201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1202=").Append(dr["D1202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1203=").Append(dr["D1204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1205=").Append(dr["D1206"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1207=").Append(dr["D1207"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("'  and QSDWDM='")
        //                        .Append(dr["QSDWDM"].ToString().Trim()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;
        //                case "203":
        //                    sb.Clear();
        //                    sb.Append("update HZ_FDCZCGK_BZ3 set D").Append(bz).Append("00=").Append(dr["D00"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("01=").Append(dr["D01"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0101=").Append(dr["D0101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0102=").Append(dr["D0102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0103=").Append(dr["D0103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("02=").Append(dr["D02"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0201=").Append(dr["D0201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0202=").Append(dr["D0202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0203=").Append(dr["D0203"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0204=").Append(dr["D0204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("03=").Append(dr["D03"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0301=").Append(dr["D0301"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0302=").Append(dr["D0302"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0303=").Append(dr["D0303"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0304=").Append(dr["D0304"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0305=").Append(dr["D0305"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0306=").Append(dr["D0306"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0307=").Append(dr["D0307"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("04=").Append(dr["D04"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0401=").Append(dr["D0401"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0402=").Append(dr["D0402"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0403=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0404=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05=").Append(dr["D05"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05H1=").Append(dr["D05H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0508=").Append(dr["D0508"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("06=").Append(dr["D06"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0601=").Append(dr["D0601"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0602=").Append(dr["D0602"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0603=").Append(dr["D0603"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("07=").Append(dr["D07"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0701=").Append(dr["D0701"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0702=").Append(dr["D0702"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08=").Append(dr["D08"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H1=").Append(dr["D08H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H2=").Append(dr["D08H2"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0809=").Append(dr["D0809"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0810=").Append(dr["D0810"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("09=").Append(dr["D09"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("10=").Append(dr["D10"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1001=").Append(dr["D1001"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1002=").Append(dr["D1002"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1003=").Append(dr["D1003"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1004=").Append(dr["D1004"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1005=").Append(dr["D1005"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1006=").Append(dr["D1006"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1007=").Append(dr["D1007"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1008=").Append(dr["D1008"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1009=").Append(dr["D1009"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("11=").Append(dr["D11"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1101=").Append(dr["D1101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1102=").Append(dr["D1102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1103=").Append(dr["D1103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1104=").Append(dr["D1104"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1105=").Append(dr["D1105"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1106=").Append(dr["D1106"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1107=").Append(dr["D1107"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1108=").Append(dr["D1108"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1109=").Append(dr["D1109"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1110=").Append(dr["D1110"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("12=").Append(dr["D12"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1201=").Append(dr["D1201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1202=").Append(dr["D1202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1203=").Append(dr["D1204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1205=").Append(dr["D1206"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1207=").Append(dr["D1207"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("'  and QSDWDM='")
        //                        .Append(dr["QSDWDM"].ToString().Trim()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;
        //                case "204":
        //                    sb = new StringBuilder();
        //                    sb.Append("update HZ_FDCZCGK_BZ3 set D2040601=").Append(dr["D0602"].ToString()).Append(",D2040603=").Append(dr["D0603"].ToString())
        //                        .Append(",D2041201=").Append(dr["D1201"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' and QSDWDM='").Append(dr["QSDWDM"].ToString()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;
        //                case "205":
        //                    sb = new StringBuilder();
        //                    sb.Append("update HZ_FDCZCGK_BZ3 set D20509=").Append(dr["D09"].ToString())
        //                      .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("'  and QSDWDM='")
        //                        .Append(dr["QSDWDM"].ToString().Trim()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;

        //            }
        //            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                    
        //        }
        //        catch (Exception e)
        //        {
        //        }
        //        #endregion
        //    }

        //    //计算 201 202 203 204  小计

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_FDCZCGK_BZ1 set D201=iif(isnull(D20100),0,D20100)+ iif(isnull(D20101),0,D20101)+iif(isnull(D20102),0,D20102)+iif(isnull(D20103),0,D20103)+")
        //      .Append("iif(isnull(D20104),0,D20104)+iif(isnull(D20105),0,D20105)+iif(isnull(D20106),0,D20106)+iif(isnull(D20107),0,D20107)+")
        //      .Append("iif(isnull(D20108),0,D20108)+iif(isnull(D20109),0,D20109)+iif(isnull(D20110),0,D20110)+iif(isnull(D20111),0,D20111)+iif(isnull(D20112),0,D20112)");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_FDCZCGK_BZ2 set  D202=iif(isnull(D20200),0,D20200)+iif(isnull(D20201),0,D20201)+iif(isnull(D20202),0,D20202)+iif(isnull(D20203),0,D20203)+")
        //      .Append("iif(isnull(D20204),0,D20204)+iif(isnull(D20205),0,D20205)+iif(isnull(D20206),0,D20206)+iif(isnull(D20207),0,D20207)+")
        //      .Append("iif(isnull(D20208),0,D20208)+iif(isnull(D20209),0,D20209)+iif(isnull(D20210),0,D20210)+iif(isnull(D20211),0,D20211)+iif(isnull(D20212),0,D20212) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_FDCZCGK_BZ3 set  D203=iif(isnull(D20300),0,D20300)+iif(isnull(D20301),0,D20301)+iif(isnull(D20302),0,D20302)+iif(isnull(D20303),0,D20303)+")
        //      .Append("iif(isnull(D20304),0,D20304)+iif(isnull(D20305),0,D20305)+iif(isnull(D20306),0,D20306)+iif(isnull(D20307),0,D20307)+")
        //      .Append("iif(isnull(D20308),0,D20308)+iif(isnull(D20309),0,D20309)+iif(isnull(D20310),0,D20310)+iif(isnull(D20311),0,D20311)+iif(isnull(D20312),0,D20312),")
        //      .Append(" D204=iif(isnull(D2040602),0,D2040602)+iif(isnull(D2040603),0,D2040603)+iif(isnull(D2041201),0,D2041201) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    #region   //计算20 总计 //跨表了

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_FDCZCGK_View1 set D20=iif(isnull(D201),0,D201)+iif(isnull(D202),0,D202)+iif(isnull(D203),0,D203)+iif(isnull(D204),0,D204)+iif(isnull(D20509),0,D20509),")
        //        .Append("D2000=iif(isnull(D20100),0,D20100)+iif(isnull(D20200),0,D20200)+iif(isnull(D20300),0,D20300),")
        //        .Append("D2001=iif(isnull(D20101),0,D20101)+iif(isnull(D20201),0,D20201)+iif(isnull(D20301),0,D20301),")
        //        .Append("D200101=iif(isnull(D2010101),0,D2010101)+iif(isnull(D2020101),0,D2020101)+iif(isnull(D2030101),0,D2030101),")
        //        .Append("D200102=iif(isnull(D2010102),0,D2010102)+iif(isnull(D2020102),0,D2020102)+iif(isnull(D2030102),0,D2030102),")
        //        .Append("D200103=iif(isnull(D2010103),0,D2010103)+iif(isnull(D2020103),0,D2020103)+iif(isnull(D2030103),0,D2030103),")
        //        .Append("D2002=iif(isnull(D20102),0,D20102)+iif(isnull(D20202),0,D20202)+iif(isnull(D20302),0,D20302),")
        //        .Append("D200201=iif(isnull(D2010201),0,D2010201)+iif(isnull(D2020201),0,D2020201)+iif(isnull(D2030201),0,D2030201),")
        //        .Append("D200202=iif(isnull(D2010202),0,D2010202)+iif(isnull(D2020202),0,D2020202)+iif(isnull(D2030202),0,D2030202),")
        //        .Append("D200203=iif(isnull(D2010203),0,D2010203)+iif(isnull(D2020203),0,D2020203)+iif(isnull(D2030203),0,D2030203),")
        //        .Append("D200204=iif(isnull(D2010204),0,D2010204)+iif(isnull(D2020204),0,D2020204)+iif(isnull(D2030204),0,D2030204),")

        //        .Append("D2003=iif(isnull(D20103),0,D20103)+iif(isnull(D20203),0,D20203)+iif(isnull(D20303),0,D20303),")
        //        .Append("D200301=iif(isnull(D2010301),0,D2010301)+iif(isnull(D2020301),0,D2020301)+iif(isnull(D2030301),0,D2030301),")
        //        .Append("D200302=iif(isnull(D2010302),0,D2010302)+iif(isnull(D2020302),0,D2020302)+iif(isnull(D2030302),0,D2030302),")
        //        .Append("D200303=iif(isnull(D2010303),0,D2010303)+iif(isnull(D2020303),0,D2020303)+iif(isnull(D2030303),0,D2030303),")
        //        .Append("D200304=iif(isnull(D2010304),0,D2010304)+iif(isnull(D2020304),0,D2020304)+iif(isnull(D2030304),0,D2030304),")
        //        .Append("D200305=iif(isnull(D2010305),0,D2010305)+iif(isnull(D2020305),0,D2020305)+iif(isnull(D2030305),0,D2030305),")
        //        .Append("D200306=iif(isnull(D2010306),0,D2010306)+iif(isnull(D2020306),0,D2020306)+iif(isnull(D2030306),0,D2030306),")
        //        .Append("D200307=iif(isnull(D2010307),0,D2010307)+iif(isnull(D2020307),0,D2020307)+iif(isnull(D2030307),0,D2030307),")
        //        .Append("D2004=iif(isnull(D20104),0,D20104)+iif(isnull(D20204),0,D20204)+iif(isnull(D20304),0,D20304),")
        //        .Append("D200401=iif(isnull(D2010401),0,D2010401)+iif(isnull(D2020401),0,D2020401)+iif(isnull(D2030401),0,D2030401),")
        //        .Append("D200402=iif(isnull(D2010402),0,D2010402)+iif(isnull(D2020402),0,D2020402)+iif(isnull(D2030402),0,D2030402),")
        //        .Append("D200403=iif(isnull(D2010403),0,D2010403)+iif(isnull(D2020403),0,D2020403)+iif(isnull(D2030403),0,D2030403),")
        //        .Append("D200404=iif(isnull(D2010404),0,D2010404)+iif(isnull(D2020404),0,D2020404)+iif(isnull(D2030404),0,D2030404) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb.Clear();
        //    sb.Append("update HZ_FDCZCGK_View2 set D2005=iif(isnull(D20105),0,D20105)+iif(isnull(D20205),0,D20205)+iif(isnull(D20305),0,D20305),")
        //        .Append("D2005H1=iif(isnull(D20105H1),0,D20105H1)+iif(isnull(D20205H1),0,D20205H1)+iif(isnull(D20305H1),0,D20305H1),")
        //        .Append("D200508=iif(isnull(D2010508),0,D2010508)+iif(isnull(D2020508),0,D2020508)+iif(isnull(D2030508),0,D2030508),")
        //        .Append("D2006=iif(isnull(D20106),0,D20106)+iif(isnull(D20206),0,D20206)+iif(isnull(D20306),0,D20306),")
        //        .Append("D200601=iif(isnull(D2010601),0,D2010601)+iif(isnull(D2020601),0,D2020601)+iif(isnull(D2030601),0,D2030601),")
        //        .Append("D200602=iif(isnull(D2010602),0,D2010602)+iif(isnull(D2020602),0,D2020602)+iif(isnull(D2030602),0,D2030602)+iif(isnull(D2040602),0,D2040602),")
        //        .Append("D200603=iif(isnull(D2010603),0,D2010603)+iif(isnull(D2020603),0,D2020603)+iif(isnull(D2030603),0,D2030603)+iif(isnull(D2040603),0,D2040603),")
        //        .Append("D2007=iif(isnull(D20107),0,D20107)+iif(isnull(D20207),0,D20207)+iif(isnull(D20307),0,D20307),")
        //        .Append("D200701=iif(isnull(D2010701),0,D2010701)+iif(isnull(D2020701),0,D2020701)+iif(isnull(D2030701),0,D2030701),")
        //        .Append("D200702=iif(isnull(D2010702),0,D2010702)+iif(isnull(D2020702),0,D2020702)+iif(isnull(D2030702),0,D2030702),")
        //        .Append("D2008=iif(isnull(D20108),0,D20108)+iif(isnull(D20208),0,D20208)+iif(isnull(D20308),0,D20308),")
        //        .Append("D2008H1=iif(isnull(D20108H1),0,D20108H1)+iif(isnull(D20208H1),0,D20208H1)+iif(isnull(D20308H1),0,D20308H1),")
        //        .Append("D2008H2=iif(isnull(D20108H2),0,D20108H2)+iif(isnull(D20208H2),0,D20208H2)+iif(isnull(D20308H2),0,D20308H2),")
        //        .Append("D200809=iif(isnull(D2010809),0,D2010809)+iif(isnull(D2020809),0,D2020809)+iif(isnull(D2030809),0,D2030809),")
        //        .Append("D200810=iif(isnull(D2010810),0,D2010810)+iif(isnull(D2020810),0,D2020810)+iif(isnull(D2030810),0,D2030810),")
        //        .Append("D2009=iif(isnull(D20109),0,D20109)+iif(isnull(D20209),0,D20209)+iif(isnull(D20309),0,D20309)+iif(isnull(D20509),0,D20509) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb.Clear();
        //    sb.Append(" update HZ_FDCZCGK_View3 set D2010=iif(isnull(D20110),0,D20110)+iif(isnull(D20210),0,D20210)+iif(isnull(D20310),0,D20310),")
        //        .Append("D201001=iif(isnull(D2011001),0,D2011001)+iif(isnull(D2021001),0,D2021001)+iif(isnull(D2031001),0,D2031001),")
        //        .Append("D201002=iif(isnull(D2011002),0,D2011002)+iif(isnull(D2021002),0,D2021002)+iif(isnull(D2031002),0,D2031002),")
        //        .Append("D201003=iif(isnull(D2011003),0,D2011003)+iif(isnull(D2021003),0,D2021003)+iif(isnull(D2031003),0,D2031003),")
        //        .Append("D201004=iif(isnull(D2011004),0,D2011004)+iif(isnull(D2021004),0,D2021004)+iif(isnull(D2031004),0,D2031004),")
        //        .Append("D201005=iif(isnull(D2011005),0,D2011005)+iif(isnull(D2021005),0,D2021005)+iif(isnull(D2031005),0,D2031005),")
        //        .Append("D201006=iif(isnull(D2011006),0,D2011006)+iif(isnull(D2021006),0,D2021006)+iif(isnull(D2031006),0,D2031006),")
        //        .Append("D201007=iif(isnull(D2011007),0,D2011007)+iif(isnull(D2021007),0,D2021007)+iif(isnull(D2031007),0,D2031007),")
        //        .Append("D201008=iif(isnull(D2011008),0,D2011008)+iif(isnull(D2021008),0,D2021008)+iif(isnull(D2031008),0,D2031008),")
        //        .Append("D201009=iif(isnull(D2011009),0,D2011009)+iif(isnull(D2021009),0,D2021009)+iif(isnull(D2031009),0,D2031009),")
        //        .Append("D2011=iif(isnull(D20111),0,D20111)+iif(isnull(D20211),0,D20211)+iif(isnull(D20311),0,D20311),")
        //       .Append("D201101=iif(isnull(D2011101),0,D2011101)+iif(isnull(D2021101),0,D2021101)+iif(isnull(D2031101),0,D2031101),")
        //       .Append("D201102=iif(isnull(D2011102),0,D2011102)+iif(isnull(D2021102),0,D2021102)+iif(isnull(D2031102),0,D2031102),")
        //       .Append("D201103=iif(isnull(D2011103),0,D2011103)+iif(isnull(D2021103),0,D2021103)+iif(isnull(D2031103),0,D2031103),")
        //       .Append("D201104=iif(isnull(D2011104),0,D2011104)+iif(isnull(D2021104),0,D2021104)+iif(isnull(D2031104),0,D2031104),")
        //       .Append("D201105=iif(isnull(D2011105),0,D2011105)+iif(isnull(D2021105),0,D2021105)+iif(isnull(D2031105),0,D2031105),")
        //       .Append("D201106=iif(isnull(D2011106),0,D2011106)+iif(isnull(D2021106),0,D2021106)+iif(isnull(D2031106),0,D2031106),")
        //       .Append("D201107=iif(isnull(D2011107),0,D2011107)+iif(isnull(D2021107),0,D2021107)+iif(isnull(D2031107),0,D2031107),")
        //       .Append("D201108=iif(isnull(D2011108),0,D2011108)+iif(isnull(D2021108),0,D2021108)+iif(isnull(D2031108),0,D2031108),")
        //       .Append("D201109=iif(isnull(D2011109),0,D2011109)+iif(isnull(D2021109),0,D2021109)+iif(isnull(D2031109),0,D2031109),")
        //       .Append("D201110=iif(isnull(D2011110),0,D2011110)+iif(isnull(D2021110),0,D2021110)+iif(isnull(D2031110),0,D2031110),")
        //       .Append("D2012=iif(isnull(D20112),0,D20112)+iif(isnull(D20212),0,D20212)+iif(isnull(D20312),0,D20312),")
        //       .Append("D201201=iif(isnull(D2011201),0,D2011201)+iif(isnull(D2021201),0,D2021201)+iif(isnull(D2031201),0,D2031201),")
        //       .Append("D201202=iif(isnull(D2011202),0,D2011202)+iif(isnull(D2021202),0,D2021202)+iif(isnull(D2031202),0,D2031202),")
        //       .Append("D201203=iif(isnull(D2011203),0,D2011203)+iif(isnull(D2021203),0,D2021203)+iif(isnull(D2031203),0,D2031203),")
        //       .Append("D201204=iif(isnull(D2011204),0,D2011204)+iif(isnull(D2021204),0,D2021204)+iif(isnull(D2031204),0,D2031204),")
        //       .Append("D201205=iif(isnull(D2011205),0,D2011205)+iif(isnull(D2021205),0,D2021205)+iif(isnull(D2031205),0,D2031205),")
        //       .Append("D201206=iif(isnull(D2011206),0,D2011206)+iif(isnull(D2021206),0,D2021206)+iif(isnull(D2031206),0,D2031206),")
        //       .Append("D201207=iif(isnull(D2011207),0,D2011207)+iif(isnull(D2021207),0,D2021207)+iif(isnull(D2031207),0,D2031207) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    #endregion 

        //    //汇总到村
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_FDCZCGK_BZ1(ZLDWDM,QSDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
        //        .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
        //        .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
        //        .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
        //        .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
        //        .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
        //        .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
        //        .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
        //        .Append("D20105,D20105H1,D2010508,D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
        //        .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
        //        .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
        //        .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
        //        .Append(" select left(ZLDWDM,12),left(QSDWDM,12), sum(D20),sum(D2000), sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
        //        .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
        //        .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
        //        .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
        //        .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
        //        .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
        //        .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
        //        .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
        //        .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
        //        .Append("sum(D201),sum(D20100),sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
        //        .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
        //        .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
        //        .Append("sum(D20105),sum(D20105H1),sum(D2010508), sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
        //        .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
        //        .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
        //        .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
        //        .Append(" from HZ_FDCZCGK_BZ1 where len(ZLDWDM)=19 and left(ZLDWDM,12)<>left(QSDWDM,12) group by left(ZLDWDM,12),left(QSDWDM,12) ");
        //    // 还没完
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    sb.Clear();
        //    sb.Append(" insert into HZ_FDCZCGK_BZ2(ZLDWDM,QSDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
        //       .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
        //       .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
        //       .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
        //       .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
        //       .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
        //       .Append(" select left(ZLDWDM,12),left(QSDWDM,12),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
        //       .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
        //       .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
        //       .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
        //       .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
        //       .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
        //       .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
        //       .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
        //       .Append(" from HZ_FDCZCGK_BZ2 where len(ZLDWDM)=19 and left(ZLDWDM,12)<>left(QSDWDM,12) group by left(ZLDWDM,12),left(QSDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb.Clear();
        //    sb.Append(" insert into HZ_FDCZCGK_BZ3(ZLDWDM,QSDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
        //    .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
        //    .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
        //        .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
        //        .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
        //        .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
        //        .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207,")
        //        .Append("D204,D2040602,D2040603,D2041201, D20509 ) ")
        //        .Append(" select left(ZLDWDM,12),left(QSDWDM,12),sum(D203),sum(D20300),sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
        //    .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
        //    .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
        //    .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
        //    .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
        //    .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
        //    .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
        //    .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207),")
        //    .Append("sum(D204),sum(D2040602),sum(D2040603),sum(D2041201), sum(D20509) from HZ_FDCZCGK_BZ3 ")
        //    .Append(" where len(ZLDWDM)=19 and left(ZLDWDM,12)<>left(QSDWDM,12) group by left(ZLDWDM,12),left(QSDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //汇总到乡
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_FDCZCGK_BZ1(ZLDWDM,QSDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
        //        .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
        //        .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
        //        .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
        //        .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
        //        .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
        //        .Append("D201, D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
        //        .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
        //        .Append("D20105,D20105H1,D2010508, D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
        //        .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
        //        .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
        //        .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
        //        .Append(" select left(ZLDWDM,9),left(QSDWDM,9), sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
        //        .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
        //        .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
        //        .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
        //        .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
        //        .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
        //        .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
        //        .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
        //        .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
        //        .Append("sum(D201),sum(D20100), sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
        //        .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
        //        .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
        //        .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
        //        .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
        //        .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
        //        .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
        //        .Append(" from HZ_FDCZCGK_BZ1 where len(ZLDWDM)=19 and left(ZLDWDM,9)<>left(QSDWDM,9) group by left(ZLDWDM,9),left(QSDWDM,9) ");
        //    // 还没完
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    sb.Clear();
        //    sb.Append(" insert into HZ_FDCZCGK_BZ2(ZLDWDM,QSDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
        //       .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
        //       .Append("D20205,D20205H1,D2020508, D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
        //       .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
        //       .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
        //       .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
        //       .Append(" select left(ZLDWDM,9),left(QSDWDM,9),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
        //       .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
        //       .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
        //       .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
        //       .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
        //       .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
        //       .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
        //       .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
        //       .Append(" from HZ_FDCZCGK_BZ2 where len(ZLDWDM)=19 and left(ZLDWDM,9)<>left(QSDWDM,9) group by left(ZLDWDM,9),left(QSDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb.Clear();
        //    sb.Append(" insert into HZ_FDCZCGK_BZ3(ZLDWDM,QSDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
        //    .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
        //    .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
        //        .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
        //        .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
        //        .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
        //        .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207,")
        //        .Append("D204,D2040602,D2040603,D2041201, D20509 ) ")
        //        .Append(" select left(ZLDWDM,9),left(QSDWDM,9),sum(D203),sum(D20300),sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
        //    .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
        //    .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
        //    .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
        //    .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
        //    .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
        //    .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
        //    .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207),")
        //    .Append("sum(D204),sum(D2040602),sum(D2040603),sum(D2041201), sum(D20509) from HZ_FDCZCGK_BZ3 ")
        //    .Append(" where len(ZLDWDM)=19 and left(ZLDWDM,9)<>left(QSDWDM,9) group by left(ZLDWDM,9),left(QSDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //   //汇总到县
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_FDCZCGK_BZ1(ZLDWDM,QSDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
        //        .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
        //        .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
        //        .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
        //        .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
        //        .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
        //        .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
        //        .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
        //        .Append("D20105,D20105H1,D2010508,D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
        //        .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
        //        .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
        //        .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
        //        .Append(" select left(ZLDWDM,6),left(QSDWDM,6), sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
        //        .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
        //        .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
        //        .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
        //        .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
        //        .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
        //        .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
        //        .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
        //        .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
        //        .Append("sum(D201), sum(D20100),sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
        //        .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
        //        .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
        //        .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
        //        .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
        //        .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
        //        .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
        //        .Append(" from HZ_FDCZCGK_BZ1 where len(ZLDWDM)=19 and left(ZLDWDM,6)<>left(QSDWDM,6) group by left(ZLDWDM,6),left(QSDWDM,6) ");
        //    // 还没完
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    sb.Clear();
        //    sb.Append(" insert into HZ_FDCZCGK_BZ2(ZLDWDM,QSDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
        //       .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
        //       .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
        //       .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
        //       .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
        //       .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
        //       .Append(" select left(ZLDWDM,6),left(QSDWDM,6),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
        //       .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
        //       .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
        //       .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
        //       .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
        //       .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
        //       .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
        //       .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
        //       .Append(" from HZ_FDCZCGK_BZ2 where len(ZLDWDM)=19 and left(ZLDWDM,6)<>left(QSDWDM,6) group by left(ZLDWDM,6),left(QSDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb.Clear();
        //    sb.Append(" insert into HZ_FDCZCGK_BZ3(ZLDWDM,QSDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
        //    .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
        //    .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
        //        .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
        //        .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
        //        .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
        //        .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207,")
        //        .Append("D204,D2040602,D2040603,D2041201, D20509 ) ")
        //        .Append(" select left(ZLDWDM,6),left(QSDWDM,6),sum(D203),sum(D20300), sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
        //    .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
        //    .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
        //    .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
        //    .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
        //    .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
        //    .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
        //    .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207),")
        //    .Append("sum(D204),sum(D2040602),sum(D2040603),sum(D2041201), sum(D20509) from HZ_FDCZCGK_BZ3 ")
        //    .Append(" where len(ZLDWDM)=19 and left(ZLDWDM,6)<>left(QSDWDM,6) group by left(ZLDWDM,6),left(QSDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //}

    
        

        ///// <summary>
        ///// 城镇村工矿 标注汇总表初始化
        ///// </summary>
        //private void InitCZCGKTable()
        //{
        //    string sql = "delete from HZ_CZCGK_BZ1";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sql = "delete from HZ_CZCGK_BZ2";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sql = "delete from HZ_CZCGK_BZ3";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_CZCGK_BZ1(ZLDWDM) select distinct ZLDWDM  from HZ_JCB  ");
        //    int iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery("insert into HZ_CZCGK_BZ2(ZLDWDM) select distinct ZLDWDM  from HZ_JCB  ");
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery("insert into HZ_CZCGK_BZ3(ZLDWDM) select distinct ZLDWDM  from HZ_JCB  ");

        //    sb = new StringBuilder();
        //    sb.Append("  select ZLDWDM,left(CZCSXM,3) as BZ,sum(TDZMJ) as TDZMJ ,SUM(D00) AS D00,sum(D01) as D01,sum(D0101) as D0101,sum(D0102) as D0102,sum(D0103) as D0103,")
        //    .Append("sum(D02) as D02,sum(D0201HJ) as D0201,sum(D0202HJ) as D0202,sum(D0203HJ) as D0203,sum(D0204HJ) as D0204,")
        //    .Append("sum(D03) as D03,sum(D0301HJ) as D0301,sum(D0302HJ) as D0302,sum(D0303)as D0303,sum(D0304) as D0304,sum(D0305) as D0305,sum(D0306) as D0306,sum(D0307HJ) as D0307,")
        //    .Append("sum(D04) as D04,sum(D0401) as D0401,sum(D0402) as D0402,sum(D0403HJ) as D0403,sum(D0404) as D0404,sum(D05) as D05,sum(D05H1) as D05H1,sum(D0508) as D0508,")
        //    .Append("sum(D06) as  D06,sum(D0601) as D0601,sum(D0602) as D0602,sum(D0603) as D0603,sum(D07) as D07,sum(D0701) as D0701,sum(D0702) as D0702,")
        //    .Append("sum(D08) as D08,sum(D08H1) as D08H1,sum(D08H2HJ) as D08H2,sum(D0809) as D0809,sum(D0810HJ) as D0810,sum(D09) as D09,sum(D10) as D10,")
        //    .Append("sum(D1001) as D1001,sum(D1002) as D1002,sum(D1003) as D1003,sum(D1004) as D1004,sum(D1005) as D1005,sum(D1006) as D1006,sum(D1007) as D1007,sum(D1008) as D1008,sum(D1009) as D1009,")
        //    .Append("sum(D11) as D11,sum(D1101) as D1101,sum(D1102) as D1102,sum(D1103) as D1103,sum(D1104HJ) as D1104,sum(D1105) as D1105,sum(D1106) as D1106,sum(D1107HJ) as D1107,sum(D1108) as D1108,sum(D1109) as D1109,sum(D1110) as D1110,")
        //   .Append("sum(D12) as D12,sum(D1201) as D1201,sum(D1202) as D1202,sum(D1203) as D1203,sum(D1204) as D1204,sum(D1205) as D1205,sum(D1206) as D1206,sum(D1207) as D1207 from HZ_JCB ")
        //   .Append("group by ZLDWDM,left(CZCSXM,3) ");
        //    sql = sb.ToString();
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        #region 逐条修改
        //        string bz = dr["BZ"].ToString().Trim(); //201 202 203 204 205
        //        if (bz == "")
        //            continue;
        //        sb = new StringBuilder();
        //        try
        //        {
        //            switch (bz.ToUpper())
        //            {
        //                case "201":
        //                    sb.Clear();
        //                    sb.Append("update HZ_CZCGK_BZ1 set D").Append(bz).Append("00=").Append(dr["D00"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("01=").Append(dr["D01"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0101=").Append(dr["D0101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0102=").Append(dr["D0102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0103=").Append(dr["D0103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("02=").Append(dr["D02"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0201=").Append(dr["D0201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0202=").Append(dr["D0202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0203=").Append(dr["D0203"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0204=").Append(dr["D0204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("03=").Append(dr["D03"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0301=").Append(dr["D0301"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0302=").Append(dr["D0302"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0303=").Append(dr["D0303"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0304=").Append(dr["D0304"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0305=").Append(dr["D0305"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0306=").Append(dr["D0306"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0307=").Append(dr["D0307"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("04=").Append(dr["D04"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0401=").Append(dr["D0401"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0402=").Append(dr["D0402"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0403=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0404=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05=").Append(dr["D05"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05H1=").Append(dr["D05H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0508=").Append(dr["D0508"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("06=").Append(dr["D06"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0601=").Append(dr["D0601"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0602=").Append(dr["D0602"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0603=").Append(dr["D0603"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("07=").Append(dr["D07"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0701=").Append(dr["D0701"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0702=").Append(dr["D0702"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08=").Append(dr["D08"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H1=").Append(dr["D08H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H2=").Append(dr["D08H2"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0809=").Append(dr["D0809"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0810=").Append(dr["D0810"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("09=").Append(dr["D09"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("10=").Append(dr["D10"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1001=").Append(dr["D1001"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1002=").Append(dr["D1002"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1003=").Append(dr["D1003"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1004=").Append(dr["D1004"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1005=").Append(dr["D1005"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1006=").Append(dr["D1006"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1007=").Append(dr["D1007"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1008=").Append(dr["D1008"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1009=").Append(dr["D1009"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("11=").Append(dr["D11"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1101=").Append(dr["D1101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1102=").Append(dr["D1102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1103=").Append(dr["D1103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1104=").Append(dr["D1104"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1105=").Append(dr["D1105"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1106=").Append(dr["D1106"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1107=").Append(dr["D1107"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1108=").Append(dr["D1108"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1109=").Append(dr["D1109"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1110=").Append(dr["D1110"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("12=").Append(dr["D12"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1201=").Append(dr["D1201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1202=").Append(dr["D1202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1203=").Append(dr["D1204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1205=").Append(dr["D1206"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1207=").Append(dr["D1207"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;
        //                case "202":
        //                    sb.Clear();
        //                    sb.Append("update HZ_CZCGK_BZ2 set D").Append(bz).Append("00=").Append(dr["D00"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("01=").Append(dr["D01"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0101=").Append(dr["D0101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0102=").Append(dr["D0102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0103=").Append(dr["D0103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("02=").Append(dr["D02"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0201=").Append(dr["D0201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0202=").Append(dr["D0202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0203=").Append(dr["D0203"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0204=").Append(dr["D0204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("03=").Append(dr["D03"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0301=").Append(dr["D0301"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0302=").Append(dr["D0302"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0303=").Append(dr["D0303"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0304=").Append(dr["D0304"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0305=").Append(dr["D0305"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0306=").Append(dr["D0306"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0307=").Append(dr["D0307"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("04=").Append(dr["D04"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0401=").Append(dr["D0401"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0402=").Append(dr["D0402"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0403=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0404=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05=").Append(dr["D05"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05H1=").Append(dr["D05H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0508=").Append(dr["D0508"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("06=").Append(dr["D06"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0601=").Append(dr["D0601"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0602=").Append(dr["D0602"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0603=").Append(dr["D0603"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("07=").Append(dr["D07"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0701=").Append(dr["D0701"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0702=").Append(dr["D0702"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08=").Append(dr["D08"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H1=").Append(dr["D08H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H2=").Append(dr["D08H2"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0809=").Append(dr["D0809"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0810=").Append(dr["D0810"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("09=").Append(dr["D09"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("10=").Append(dr["D10"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1001=").Append(dr["D1001"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1002=").Append(dr["D1002"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1003=").Append(dr["D1003"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1004=").Append(dr["D1004"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1005=").Append(dr["D1005"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1006=").Append(dr["D1006"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1007=").Append(dr["D1007"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1008=").Append(dr["D1008"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1009=").Append(dr["D1009"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("11=").Append(dr["D11"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1101=").Append(dr["D1101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1102=").Append(dr["D1102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1103=").Append(dr["D1103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1104=").Append(dr["D1104"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1105=").Append(dr["D1105"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1106=").Append(dr["D1106"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1107=").Append(dr["D1107"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1108=").Append(dr["D1108"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1109=").Append(dr["D1109"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1110=").Append(dr["D1110"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("12=").Append(dr["D12"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1201=").Append(dr["D1201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1202=").Append(dr["D1202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1203=").Append(dr["D1204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1205=").Append(dr["D1206"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1207=").Append(dr["D1207"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;
        //                case "203":
        //                    sb.Clear();
        //                    sb.Append("update HZ_CZCGK_BZ3 set D").Append(bz).Append("00=").Append(dr["D00"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("01=").Append(dr["D01"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0101=").Append(dr["D0101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0102=").Append(dr["D0102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0103=").Append(dr["D0103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("02=").Append(dr["D02"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0201=").Append(dr["D0201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0202=").Append(dr["D0202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0203=").Append(dr["D0203"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0204=").Append(dr["D0204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("03=").Append(dr["D03"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0301=").Append(dr["D0301"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0302=").Append(dr["D0302"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0303=").Append(dr["D0303"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0304=").Append(dr["D0304"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0305=").Append(dr["D0305"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0306=").Append(dr["D0306"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0307=").Append(dr["D0307"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("04=").Append(dr["D04"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0401=").Append(dr["D0401"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0402=").Append(dr["D0402"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0403=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0404=").Append(dr["D0403"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05=").Append(dr["D05"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("05H1=").Append(dr["D05H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0508=").Append(dr["D0508"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("06=").Append(dr["D06"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0601=").Append(dr["D0601"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0602=").Append(dr["D0602"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0603=").Append(dr["D0603"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("07=").Append(dr["D07"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0701=").Append(dr["D0701"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0702=").Append(dr["D0702"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08=").Append(dr["D08"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H1=").Append(dr["D08H1"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("08H2=").Append(dr["D08H2"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0809=").Append(dr["D0809"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("0810=").Append(dr["D0810"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("09=").Append(dr["D09"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("10=").Append(dr["D10"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1001=").Append(dr["D1001"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1002=").Append(dr["D1002"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1003=").Append(dr["D1003"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1004=").Append(dr["D1004"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1005=").Append(dr["D1005"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1006=").Append(dr["D1006"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1007=").Append(dr["D1007"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1008=").Append(dr["D1008"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1009=").Append(dr["D1009"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("11=").Append(dr["D11"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1101=").Append(dr["D1101"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1102=").Append(dr["D1102"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1103=").Append(dr["D1103"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1104=").Append(dr["D1104"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1105=").Append(dr["D1105"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1106=").Append(dr["D1106"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1107=").Append(dr["D1107"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1108=").Append(dr["D1108"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1109=").Append(dr["D1109"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1110=").Append(dr["D1110"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("12=").Append(dr["D12"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1201=").Append(dr["D1201"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1202=").Append(dr["D1202"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1203=").Append(dr["D1204"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1205=").Append(dr["D1206"].ToString()).Append(",")
        //                        .Append("D").Append(bz).Append("1207=").Append(dr["D1207"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //                    sql = sb.ToString();


        //                    break;
        //                case "204":
        //                    sb = new StringBuilder();
        //                    sb.Append("update HZ_CZCGK_BZ3 set D2040602=").Append(dr["D0602"].ToString()).Append(",D2040603=").Append(dr["D0603"].ToString())
        //                        .Append(",D2041201=").Append(dr["D1201"].ToString())
        //                        .Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;
        //                case "205":
        //                    sb = new StringBuilder();
        //                    sb.Append("update HZ_CZCGK_BZ3 set D20509=").Append(dr["D09"].ToString()).Append(" where ZLDWDM='").Append(dr["ZLDWDM"].ToString()).Append("' ");
        //                    sql = sb.ToString();
        //                    break;

        //            }
        //            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                    
                

        //        }
        //        catch (Exception e)
        //        {
        //        }
        //        #endregion
        //    }
            
        //    //计算 201 202 203 204  小计
          
        //    sb = new StringBuilder();
        //    sb.Append("update HZ_CZCGK_BZ1 set D201=iif(isnull(D20100),0,D20100)+iif(isnull(D20101),0,D20101)+iif(isnull(D20102),0,D20102)+iif(isnull(D20103),0,D20103)+")
        //      .Append("iif(isnull(D20104),0,D20104)+iif(isnull(D20105),0,D20105)+iif(isnull(D20106),0,D20106)+iif(isnull(D20107),0,D20107)+")
        //      .Append("iif(isnull(D20108),0,D20108)+iif(isnull(D20109),0,D20109)+iif(isnull(D20110),0,D20110)+iif(isnull(D20111),0,D20111)+iif(isnull(D20112),0,D20112)");             
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_CZCGK_BZ2 set  D202=iif(isnull(D20200),0,D20200)+iif(isnull(D20201),0,D20201)+iif(isnull(D20202),0,D20202)+iif(isnull(D20203),0,D20203)+")
        //      .Append("iif(isnull(D20204),0,D20204)+iif(isnull(D20205),0,D20205)+iif(isnull(D20206),0,D20206)+iif(isnull(D20207),0,D20207)+")
        //      .Append("iif(isnull(D20208),0,D20208)+iif(isnull(D20209),0,D20209)+iif(isnull(D20210),0,D20210)+iif(isnull(D20211),0,D20211)+iif(isnull(D20212),0,D20212) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_CZCGK_BZ3 set  D203=iif(isnull(D20300),0,D20300)+iif(isnull(D20301),0,D20301)+iif(isnull(D20302),0,D20302)+iif(isnull(D20303),0,D20303)+")
        //      .Append("iif(isnull(D20304),0,D20304)+iif(isnull(D20305),0,D20305)+iif(isnull(D20306),0,D20306)+iif(isnull(D20307),0,D20307)+")
        //      .Append("iif(isnull(D20308),0,D20308)+iif(isnull(D20309),0,D20309)+iif(isnull(D20310),0,D20310)+iif(isnull(D20311),0,D20311)+iif(isnull(D20312),0,D20312),")
        //      .Append(" D204=iif(isnull(D2040602),0,D2040602)+iif(isnull(D2040603),0,D2040603)+iif(isnull(D2041201),0,D2041201) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    #region   //计算20 总计 //跨表了

        //    sb = new StringBuilder();
        //    sb.Append("update HZ_CZCGK_View1 set D20=iif(isnull(D201),0,D201)+iif(isnull(D202),0,D202)+iif(isnull(D203),0,D203)+iif(isnull(D204),0,D204)+iif(isnull(D20509),0,D20509),")
        //        .Append("D2000=iif(isnull(D20100),0,D20100)+iif(isnull(D20200),0,D20200)+iif(isnull(D20300),0,D20300),")
        //        .Append("D2001=iif(isnull(D20101),0,D20101)+iif(isnull(D20201),0,D20201)+iif(isnull(D20301),0,D20301),")
        //        .Append("D200101=iif(isnull(D2010101),0,D2010101)+iif(isnull(D2020101),0,D2020101)+iif(isnull(D2030101),0,D2030101),")
        //        .Append("D200102=iif(isnull(D2010102),0,D2010102)+iif(isnull(D2020102),0,D2020102)+iif(isnull(D2030102),0,D2030102),")
        //        .Append("D200103=iif(isnull(D2010103),0,D2010103)+iif(isnull(D2020103),0,D2020103)+iif(isnull(D2030103),0,D2030103),")
        //        .Append("D2002=iif(isnull(D20102),0,D20102)+iif(isnull(D20202),0,D20202)+iif(isnull(D20302),0,D20302),")
        //        .Append("D200201=iif(isnull(D2010201),0,D2010201)+iif(isnull(D2020201),0,D2020201)+iif(isnull(D2030201),0,D2030201),")
        //        .Append("D200202=iif(isnull(D2010202),0,D2010202)+iif(isnull(D2020202),0,D2020202)+iif(isnull(D2030202),0,D2030202),")
        //        .Append("D200203=iif(isnull(D2010203),0,D2010203)+iif(isnull(D2020203),0,D2020203)+iif(isnull(D2030203),0,D2030203),")
        //        .Append("D200204=iif(isnull(D2010204),0,D2010204)+iif(isnull(D2020204),0,D2020204)+iif(isnull(D2030204),0,D2030204),")

        //        .Append("D2003=iif(isnull(D20103),0,D20103)+iif(isnull(D20203),0,D20203)+iif(isnull(D20303),0,D20303),")
        //        .Append("D200301=iif(isnull(D2010301),0,D2010301)+iif(isnull(D2020301),0,D2020301)+iif(isnull(D2030301),0,D2030301),")
        //        .Append("D200302=iif(isnull(D2010302),0,D2010302)+iif(isnull(D2020302),0,D2020302)+iif(isnull(D2030302),0,D2030302),")
        //        .Append("D200303=iif(isnull(D2010303),0,D2010303)+iif(isnull(D2020303),0,D2020303)+iif(isnull(D2030303),0,D2030303),")
        //        .Append("D200304=iif(isnull(D2010304),0,D2010304)+iif(isnull(D2020304),0,D2020304)+iif(isnull(D2030304),0,D2030304),")
        //        .Append("D200305=iif(isnull(D2010305),0,D2010305)+iif(isnull(D2020305),0,D2020305)+iif(isnull(D2030305),0,D2030305),")
        //        .Append("D200306=iif(isnull(D2010306),0,D2010306)+iif(isnull(D2020306),0,D2020306)+iif(isnull(D2030306),0,D2030306),")
        //        .Append("D200307=iif(isnull(D2010307),0,D2010307)+iif(isnull(D2020307),0,D2020307)+iif(isnull(D2030307),0,D2030307),")
        //        .Append("D2004=iif(isnull(D20104),0,D20104)+iif(isnull(D20204),0,D20204)+iif(isnull(D20304),0,D20304),")
        //        .Append("D200401=iif(isnull(D2010401),0,D2010401)+iif(isnull(D2020401),0,D2020401)+iif(isnull(D2030401),0,D2030401),")
        //        .Append("D200402=iif(isnull(D2010402),0,D2010402)+iif(isnull(D2020402),0,D2020402)+iif(isnull(D2030402),0,D2030402),")
        //        .Append("D200403=iif(isnull(D2010403),0,D2010403)+iif(isnull(D2020403),0,D2020403)+iif(isnull(D2030403),0,D2030403),")
        //        .Append("D200404=iif(isnull(D2010404),0,D2010404)+iif(isnull(D2020404),0,D2020404)+iif(isnull(D2030404),0,D2030404) ");
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb.Clear();
        //    sb.Append("update HZ_CZCGK_View2 set D2005=iif(isnull(D20105),0,D20105)+iif(isnull(D20205),0,D20205)+iif(isnull(D20305),0,D20305),")
        //        .Append("D2005H1=iif(isnull(D20105H1),0,D20105H1)+iif(isnull(D20205H1),0,D20205H1)+iif(isnull(D20305H1),0,D20305H1),")
        //        .Append("D200508=iif(isnull(D2010508),0,D2010508)+iif(isnull(D2020508),0,D2020508)+iif(isnull(D2030508),0,D2030508),")
        //        .Append("D2006=iif(isnull(D20106),0,D20106)+iif(isnull(D20206),0,D20206)+iif(isnull(D20306),0,D20306),")
        //        .Append("D200601=iif(isnull(D2010601),0,D2010601)+iif(isnull(D2020601),0,D2020601)+iif(isnull(D2030601),0,D2030601),")
        //        .Append("D200602=iif(isnull(D2010602),0,D2010602)+iif(isnull(D2020602),0,D2020602)+iif(isnull(D2030602),0,D2030602)+iif(isnull(D2040602),0,D2040602),")
        //        .Append("D200603=iif(isnull(D2010603),0,D2010603)+iif(isnull(D2020603),0,D2020603)+iif(isnull(D2030603),0,D2030603)+iif(isnull(D2040603),0,D2040603),")
        //        .Append("D2007=iif(isnull(D20107),0,D20107)+iif(isnull(D20207),0,D20207)+iif(isnull(D20307),0,D20307),")
        //        .Append("D200701=iif(isnull(D2010701),0,D2010701)+iif(isnull(D2020701),0,D2020701)+iif(isnull(D2030701),0,D2030701),")
        //        .Append("D200702=iif(isnull(D2010702),0,D2010702)+iif(isnull(D2020702),0,D2020702)+iif(isnull(D2030702),0,D2030702),")
        //        .Append("D2008=iif(isnull(D20108),0,D20108)+iif(isnull(D20208),0,D20208)+iif(isnull(D20308),0,D20308),")
        //        .Append("D2008H1=iif(isnull(D20108H1),0,D20108H1)+iif(isnull(D20208H1),0,D20208H1)+iif(isnull(D20308H1),0,D20308H1),")
        //        .Append("D2008H2=iif(isnull(D20108H2),0,D20108H2)+iif(isnull(D20208H2),0,D20208H2)+iif(isnull(D20308H2),0,D20308H2),")
        //        .Append("D200809=iif(isnull(D2010809),0,D2010809)+iif(isnull(D2020809),0,D2020809)+iif(isnull(D2030809),0,D2030809),")
        //        .Append("D200810=iif(isnull(D2010810),0,D2010810)+iif(isnull(D2020810),0,D2020810)+iif(isnull(D2030810),0,D2030810),")
        //        .Append("D2009=iif(isnull(D20109),0,D20109)+iif(isnull(D20209),0,D20209)+iif(isnull(D20309),0,D20309)+iif(isnull(D20509),0,D20509) ");
        //    sql=sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    sb.Clear();
        //    sb.Append(" update HZ_CZCGK_View3 set D2010=iif(isnull(D20110),0,D20110)+iif(isnull(D20210),0,D20210)+iif(isnull(D20310),0,D20310),")
        //        .Append("D201001=iif(isnull(D2011001),0,D2011001)+iif(isnull(D2021001),0,D2021001)+iif(isnull(D2031001),0,D2031001),")
        //        .Append("D201002=iif(isnull(D2011002),0,D2011002)+iif(isnull(D2021002),0,D2021002)+iif(isnull(D2031002),0,D2031002),")
        //        .Append("D201003=iif(isnull(D2011003),0,D2011003)+iif(isnull(D2021003),0,D2021003)+iif(isnull(D2031003),0,D2031003),")
        //        .Append("D201004=iif(isnull(D2011004),0,D2011004)+iif(isnull(D2021004),0,D2021004)+iif(isnull(D2031004),0,D2031004),")
        //        .Append("D201005=iif(isnull(D2011005),0,D2011005)+iif(isnull(D2021005),0,D2021005)+iif(isnull(D2031005),0,D2031005),")
        //        .Append("D201006=iif(isnull(D2011006),0,D2011006)+iif(isnull(D2021006),0,D2021006)+iif(isnull(D2031006),0,D2031006),")
        //        .Append("D201007=iif(isnull(D2011007),0,D2011007)+iif(isnull(D2021007),0,D2021007)+iif(isnull(D2031007),0,D2031007),")
        //        .Append("D201008=iif(isnull(D2011008),0,D2011008)+iif(isnull(D2021008),0,D2021008)+iif(isnull(D2031008),0,D2031008),")
        //        .Append("D201009=iif(isnull(D2011009),0,D2011009)+iif(isnull(D2021009),0,D2021009)+iif(isnull(D2031009),0,D2031009),")
        //        .Append("D2011=iif(isnull(D20111),0,D20111)+iif(isnull(D20211),0,D20211)+iif(isnull(D20311),0,D20311),")
        //       .Append("D201101=iif(isnull(D2011101),0,D2011101)+iif(isnull(D2021101),0,D2021101)+iif(isnull(D2031101),0,D2031101),")
        //       .Append("D201102=iif(isnull(D2011102),0,D2011102)+iif(isnull(D2021102),0,D2021102)+iif(isnull(D2031102),0,D2031102),")
        //       .Append("D201103=iif(isnull(D2011103),0,D2011103)+iif(isnull(D2021103),0,D2021103)+iif(isnull(D2031103),0,D2031103),")
        //       .Append("D201104=iif(isnull(D2011104),0,D2011104)+iif(isnull(D2021104),0,D2021104)+iif(isnull(D2031104),0,D2031104),")
        //       .Append("D201105=iif(isnull(D2011105),0,D2011105)+iif(isnull(D2021105),0,D2021105)+iif(isnull(D2031105),0,D2031105),")
        //       .Append("D201106=iif(isnull(D2011106),0,D2011106)+iif(isnull(D2021106),0,D2021106)+iif(isnull(D2031106),0,D2031106),")
        //       .Append("D201107=iif(isnull(D2011107),0,D2011107)+iif(isnull(D2021107),0,D2021107)+iif(isnull(D2031107),0,D2031107),")
        //       .Append("D201108=iif(isnull(D2011108),0,D2011108)+iif(isnull(D2021108),0,D2021108)+iif(isnull(D2031108),0,D2031108),")
        //       .Append("D201109=iif(isnull(D2011109),0,D2011109)+iif(isnull(D2021109),0,D2021109)+iif(isnull(D2031109),0,D2031109),")
        //       .Append("D201110=iif(isnull(D2011110),0,D2011110)+iif(isnull(D2021110),0,D2021110)+iif(isnull(D2031110),0,D2031110),")
        //       .Append("D2012=iif(isnull(D20112),0,D20112)+iif(isnull(D20212),0,D20212)+iif(isnull(D20312),0,D20312),")
        //       .Append("D201201=iif(isnull(D2011201),0,D2011201)+iif(isnull(D2021201),0,D2021201)+iif(isnull(D2031201),0,D2031201),")
        //       .Append("D201202=iif(isnull(D2011202),0,D2011202)+iif(isnull(D2021202),0,D2021202)+iif(isnull(D2031202),0,D2031202),")
        //       .Append("D201203=iif(isnull(D2011203),0,D2011203)+iif(isnull(D2021203),0,D2021203)+iif(isnull(D2031203),0,D2031203),")
        //       .Append("D201204=iif(isnull(D2011204),0,D2011204)+iif(isnull(D2021204),0,D2021204)+iif(isnull(D2031204),0,D2031204),")
        //       .Append("D201205=iif(isnull(D2011205),0,D2011205)+iif(isnull(D2021205),0,D2021205)+iif(isnull(D2031205),0,D2031205),")
        //       .Append("D201206=iif(isnull(D2011206),0,D2011206)+iif(isnull(D2021206),0,D2021206)+iif(isnull(D2031206),0,D2031206),")
        //       .Append("D201207=iif(isnull(D2011207),0,D2011207)+iif(isnull(D2021207),0,D2021207)+iif(isnull(D2031207),0,D2031207) ");
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    #endregion 

        //    //汇总到村

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_CZCGK_BZ1(ZLDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
        //        .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
        //        .Append("D2005,D2005H1,D200508, D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
        //        .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
        //        .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
        //        .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
        //        .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
        //        .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
        //        .Append("D20105,D20105H1,D2010508, D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
        //        .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
        //        .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
        //        .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
        //        .Append(" select left(ZLDWDM,12),sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
        //        .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
        //        .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
        //        .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
        //        .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
        //        .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
        //        .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
        //        .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
        //        .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
        //        .Append("sum(D201), sum(D20100), sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
        //        .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
        //        .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
        //        .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
        //        .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
        //        .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
        //        .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
        //        .Append(" from HZ_CZCGK_BZ1 where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //     // 还没完
        //     sql = sb.ToString();
        //     iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //     sb.Clear();
        //     sb.Append(" insert into HZ_CZCGK_BZ2(ZLDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
        //        .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
        //        .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
        //        .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
        //        .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
        //        .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
        //        .Append(" select left(ZLDWDM,12),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
        //        .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
        //        .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
        //        .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
        //        .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
        //        .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
        //        .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
        //        .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
        //        .Append(" from HZ_CZCGK_BZ2 where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb.Clear();
        //    sb.Append(" insert into HZ_CZCGK_BZ3(ZLDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
        //    .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
        //    .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
        //        .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
        //        .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
        //        .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
        //        .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207,")
        //        .Append("D204,D2040602,D2040603,D2041201, D20509 ) ")
        //        .Append(" select left(ZLDWDM,12),sum(D203),sum(D20300),sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
        //    .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
        //    .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
        //    .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
        //    .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
        //    .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
        //    .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
        //    .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207),")
        //    .Append("sum(D204),sum(D2040602),sum(D2040603),sum(D2041201), sum(D20509) from HZ_CZCGK_BZ3 where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //   //汇总到乡
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_CZCGK_BZ1(ZLDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
        //        .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
        //        .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
        //        .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
        //        .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
        //        .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
        //        .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
        //        .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
        //        .Append("D20105,D20105H1,D2010508,D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
        //        .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
        //        .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
        //        .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
        //        .Append(" select left(ZLDWDM,9),sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
        //        .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
        //        .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
        //        .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
        //        .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
        //        .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
        //        .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
        //        .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
        //        .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
        //        .Append("sum(D201),sum(D20100),sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
        //        .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
        //        .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
        //        .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
        //        .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
        //        .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
        //        .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
        //        .Append(" from HZ_CZCGK_BZ1 where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    // 还没完
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    sb.Clear();
        //    sb.Append(" insert into HZ_CZCGK_BZ2(ZLDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
        //       .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
        //       .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
        //       .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
        //       .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
        //       .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
        //       .Append(" select left(ZLDWDM,9),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
        //       .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
        //       .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
        //       .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
        //       .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
        //       .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
        //       .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
        //       .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
        //       .Append(" from HZ_CZCGK_BZ2 where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb.Clear();
        //    sb.Append(" insert into HZ_CZCGK_BZ3(ZLDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
        //    .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
        //    .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
        //        .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
        //        .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
        //        .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
        //        .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207,")
        //        .Append("D204,D2040602,D2040603,D2041201, D20509 ) ")
        //        .Append(" select left(ZLDWDM,9),sum(D203),sum(D20300),sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
        //        .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
        //        .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
        //        .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
        //        .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
        //        .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
        //        .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
        //        .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207),")
        //        .Append("sum(D204),sum(D2040602),sum(D2040603),sum(D2041201), sum(D20509) from HZ_CZCGK_BZ3 where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //汇总到县
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_CZCGK_BZ1(ZLDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
        //        .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
        //        .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
        //        .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
        //        .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
        //        .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
        //        .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
        //        .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
        //        .Append("D20105,D20105H1,D2010508,D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
        //        .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
        //        .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
        //        .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
        //        .Append(" select left(ZLDWDM,6),sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
        //        .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
        //        .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
        //        .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
        //        .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
        //        .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
        //        .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
        //        .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
        //        .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
        //        .Append("sum(D201),sum(D20100),sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
        //        .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
        //        .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
        //        .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
        //        .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
        //        .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
        //        .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
        //        .Append(" from HZ_CZCGK_BZ1 where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    // 还没完
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    sb.Clear();
        //    sb.Append(" insert into HZ_CZCGK_BZ2(ZLDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
        //       .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
        //       .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
        //       .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
        //       .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
        //       .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
        //       .Append(" select left(ZLDWDM,6),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
        //       .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
        //       .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
        //       .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
        //       .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
        //       .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
        //       .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
        //       .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
        //       .Append(" from HZ_CZCGK_BZ2 where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb.Clear();
        //    sb.Append(" insert into HZ_CZCGK_BZ3(ZLDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
        //    .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
        //    .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
        //        .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
        //        .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
        //        .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
        //        .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207,")
        //        .Append("D204,D2040602,D2040603,D2041201, D20509 ) ")
        //        .Append(" select left(ZLDWDM,6),sum(D203),sum(D20300), sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
        //    .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
        //    .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
        //    .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
        //    .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
        //    .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
        //    .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
        //    .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207),")
        //    .Append("sum(D204),sum(D2040602),sum(D2040603),sum(D2041201), sum(D20509) from HZ_CZCGK_BZ3 where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //}

        ////灌丛草地
        //private void InitGCCDXSCDTable()
        //{
        //    string sql = "delete from HZ_GCXSCD_BZ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_GCXSCD_BZ(ZLDWDM) select distinct ZLDWDM from HZ_JCB where TBXHDM in ('GCCD') ");
        //    sql = sb.ToString();
        //    int iret=  LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());
        //    sb = new StringBuilder();
        //    sb.Append("select zldwdm,sum(D04) as DGCCD,sum(D0401) as DGCCD0401,sum(D0402) as DGCCD0402,sum(D0403HJ) as DGCCD0403,sum(D0404) as DGCCD0404 from HZ_JCB ")
        //    .Append(" where TBXHDM='GCCD' group by ZLDWDM ");
        //    sql = sb.ToString();
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql,"gccd");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string zldwdm = dr["ZLDWDM"].ToString();
        //        double d04, d0401, d0402, d0403, d0404 = 0;
        //        double.TryParse(dr["DGCCD"].ToString(), out d04);
        //        double.TryParse(dr["DGCCD0401"].ToString(), out d0401);
        //        double.TryParse(dr["DGCCD0402"].ToString(), out d0402);
        //        double.TryParse(dr["DGCCD0403"].ToString(), out d0403);
        //        double.TryParse(dr["DGCCD0404"].ToString(), out d0404);

        //        sb = new StringBuilder();
        //        sb.Append("update HZ_GCXSCD_BZ set DGCCD=").Append(d04).Append(",DGCCD0401=").Append(d0401).Append(",DGCCD0402=").Append(d0402)
        //            .Append(",DGCCD0403=").Append(d0403).Append(",DGCCD0404=").Append(d0404).Append(" where ZLDWDM='").Append(zldwdm).Append("' ");
        //        sql = sb.ToString();
        //        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }

          

        //    //汇总到乡
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GCXSCD_BZ(zldwdm,DGCCD,DGCCD0401,DGCCD0402,DGCCD0403,DGCCD0404 ) ")
        //        .Append(" select left(ZLDWDM,12),sum(DGCCD),sum(DGCCD0401),sum(DGCCD0402),sum(DGCCD0403),sum(DGCCD0404) ")
        //       .Append("  from HZ_GCXSCD_BZ ")
        //       .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GCXSCD_BZ(zldwdm,DGCCD,DGCCD0401,DGCCD0402,DGCCD0403,DGCCD0404 ) ")
        //        .Append(" select left(ZLDWDM,9),sum(DGCCD),sum(DGCCD0401),sum(DGCCD0402),sum(DGCCD0403),sum(DGCCD0404)")
        //       .Append("  from HZ_GCXSCD_BZ ")
        //       .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //胡总到县
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GCXSCD_BZ(zldwdm,DGCCD,DGCCD0401,DGCCD0402,DGCCD0403,DGCCD0404) ")
        //        .Append(" select left(ZLDWDM,6),sum(DGCCD),sum(DGCCD0401),sum(DGCCD0402),sum(DGCCD0403),sum(DGCCD0404)")
        //       .Append("  from HZ_GCXSCD_BZ ")
        //       .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //}

        ////工业用地细化
        //private void InitGYCCTable()
        //{
        //    string sql = "delete from HZ_GYCC_BZ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_GYCC_BZ (ZLDWDM,D0601) select ZLDWDM,sum(D0601) from HZ_JCB ")
        //        .Append(" where TBXHDM in ('HDGY','GTGY','MTGY','SNGY','BLGY','DLGY')  group by ZLDWDM ");
        //    int iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

        //    sql = "select ZLDWDM,TBXHDM,sum(D0601) as dmj from HZ_JCB  where TBXHDM in ('HDGY','GTGY','MTGY','SNGY','BLGY','DLGY') group by ZLDWDM,TBXHDM ";
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "gkcc");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string zldwdm = dr["ZLDWDM"].ToString();
        //        double mj = 0;
        //        double.TryParse(dr["dmj"].ToString(), out mj);
        //        string tbxhdm = dr["TBXHDM"].ToString().ToUpper().Trim();
        //        sql = "update HZ_GYCC_BZ set D" + tbxhdm + " = " + mj + " where ZLDWDM='" + zldwdm + "' ";
        //        iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                
        //    }

        //    //乡级
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GYCC_BZ (ZLDWDM,D0601,DHDGY,DGTGY,DMTGY,DSNGY,DBLGY,DDLGY) ")
        //      .Append(" select left(ZLDWDM,12),sum(D0601),sum(DHDGY),sum(DGTGY),sum(DMTGY),sum(DSNGY),sum(DBLGY),sum(DDLGY) from HZ_GYCC_BZ ")
        //      .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GYCC_BZ (ZLDWDM,D0601,DHDGY,DGTGY,DMTGY,DSNGY,DBLGY,DDLGY) ")
        //      .Append(" select left(ZLDWDM,9),sum(D0601),sum(DHDGY),sum(DGTGY),sum(DMTGY),sum(DSNGY),sum(DBLGY),sum(DDLGY) from HZ_GYCC_BZ ")
        //      .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //    //县级
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GYCC_BZ (ZLDWDM,D0601,DHDGY,DGTGY,DMTGY,DSNGY,DBLGY,DDLGY) ")
        //      .Append(" select left(ZLDWDM,6),sum(D0601),sum(DHDGY),sum(DGTGY),sum(DMTGY),sum(DSNGY),sum(DBLGY),sum(DDLGY) from HZ_GYCC_BZ ")
        //      .Append(" where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //}

        ////耕地细化调查情况统计表
        //private void InitGdxhdcTable()
        //{
        //    string sql = "delete * from HZ_GDXHDCTJ_BZ ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql); //耕地细化调查统计
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDXHDCTJ_BZ(ZLDWDM,D01) select ZLDWDM,sum(D01) from HZ_JCB where TBXHDM in ('HDGD','HQGD','LQGD','MQGD','SHGD','SMGD') ")
        //        .Append(" group by ZLDWDM ");
        //    int iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());
        //    sb = new StringBuilder();
        //    sb.Append("select ZLDWDM,TBXHDM,sum(D01) as D01,sum(D0101) as D0101,sum(D0102) as D0102,sum(D0103) as D0103 from HZ_JCB ")
        //        .Append(" where TBXHDM in ('HDGD','HQGD','LQGD','MQGD','SHGD','SMGD') group by ZLDWDM,TBXHDM ");
        //    sql = sb.ToString();
        //    DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "gdxh");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string zldwdm = dr["ZLDWDM"].ToString();
        //        string tbxhdm = dr["TBXHDM"].ToString().Trim().ToUpper();
        //        double d01, d0101, d0102, d0103 = 0;
        //        double.TryParse(dr["D01"].ToString(), out d01);
        //        double.TryParse(dr["D0101"].ToString(), out d0101);
        //        double.TryParse(dr["D0102"].ToString(), out d0102);
        //        double.TryParse(dr["D0103"].ToString(), out d0103);
        //        sql = "update HZ_GDXHDCTJ_BZ set D" + tbxhdm + "01 = " + d01 + ", D" + tbxhdm + "0101 = " + d0101 + ", "
        //            + "D" + tbxhdm + "0102=" + d0102 + ", D" + tbxhdm + "0103 = " + d0103 + " where ZLDWDM='" + zldwdm + "' ";
        //        iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }
        //    //到乡级
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDXHDCTJ_BZ(ZLDWDM,D01,DHDGD01,DHDGD0101,DHDGD0102,DHDGD0103,DHQGD01,DHQGD0101,DHQGD0102,DHQGD0103,")
        //        .Append(" DLQGD01,DLQGD0101,DLQGD0102,DLQGD0103,DMQGD01,DMQGD0101,DMQGD0102,DMQGD0103,DSHGD01,DSHGD0101,DSHGD0102,DSHGD0103,DSMGD01,DSMGD0101,DSMGD0102,DSMGD0103 ) ")
        //        .Append(" select left(ZLDWDM,12),sum(D01),sum(DHDGD01),sum(DHDGD0101),sum(DHDGD0102),sum(DHDGD0103),sum(DHQGD01),sum(DHQGD0101),sum(DHQGD0102),sum(DHQGD0103),")
        //        .Append(" sum(DLQGD01),sum(DLQGD0101),sum(DLQGD0102),sum(DLQGD0103),sum(DMQGD01),sum(DMQGD0101),sum(DMQGD0102),sum(DMQGD0103),sum(DSHGD01),sum(DSHGD0101),sum(DSHGD0102),sum(DSHGD0103), ")
        //        .Append(" sum(DSMGD01),sum(DSMGD0101),sum(DSMGD0102),sum(DSMGD0103) ")
        //        .Append(" from HZ_GDXHDCTJ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDXHDCTJ_BZ(ZLDWDM,D01,DHDGD01,DHDGD0101,DHDGD0102,DHDGD0103,DHQGD01,DHQGD0101,DHQGD0102,DHQGD0103,")
        //        .Append(" DLQGD01,DLQGD0101,DLQGD0102,DLQGD0103,DMQGD01,DMQGD0101,DMQGD0102,DMQGD0103,DSHGD01,DSHGD0101,DSHGD0102,DSHGD0103,DSMGD01,DSMGD0101,DSMGD0102,DSMGD0103 ) ")
        //        .Append(" select left(ZLDWDM,9),sum(D01),sum(DHDGD01),sum(DHDGD0101),sum(DHDGD0102),sum(DHDGD0103),sum(DHQGD01),sum(DHQGD0101),sum(DHQGD0102),sum(DHQGD0103),")
        //        .Append(" sum(DLQGD01),sum(DLQGD0101),sum(DLQGD0102),sum(DLQGD0103),sum(DMQGD01),sum(DMQGD0101),sum(DMQGD0102),sum(DMQGD0103),sum(DSHGD01),sum(DSHGD0101),sum(DSHGD0102),sum(DSHGD0103), ")
        //        .Append(" sum(DSMGD01),sum(DSMGD0101),sum(DSMGD0102),sum(DSMGD0103) ")
        //        .Append(" from HZ_GDXHDCTJ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDXHDCTJ_BZ(ZLDWDM,D01,DHDGD01,DHDGD0101,DHDGD0102,DHDGD0103,DHQGD01,DHQGD0101,DHQGD0102,DHQGD0103,")
        //        .Append(" DLQGD01,DLQGD0101,DLQGD0102,DLQGD0103,DMQGD01,DMQGD0101,DMQGD0102,DMQGD0103,DSHGD01,DSHGD0101,DSHGD0102,DSHGD0103,DSMGD01,DSMGD0101,DSMGD0102,DSMGD0103 ) ")
        //        .Append(" select left(ZLDWDM,6),sum(D01),sum(DHDGD01),sum(DHDGD0101),sum(DHDGD0102),sum(DHDGD0103),sum(DHQGD01),sum(DHQGD0101),sum(DHQGD0102),sum(DHQGD0103),")
        //        .Append(" sum(DLQGD01),sum(DLQGD0101),sum(DLQGD0102),sum(DLQGD0103),sum(DMQGD01),sum(DMQGD0101),sum(DMQGD0102),sum(DMQGD0103),sum(DSHGD01),sum(DSHGD0101),sum(DSHGD0102),sum(DSHGD0103), ")
        //        .Append(" sum(DSMGD01),sum(DSMGD0101),sum(DSMGD0102),sum(DSMGD0103) ")
        //        .Append(" from HZ_GDXHDCTJ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


        //}

     

        ////林区范围内园地汇总表
        //private void InitLQYDTable()
        //{
        //    string sql = "delete from HZ_LQFWNYD_BZ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_LQFWNYD_BZ(ZLDWDM,D02,D0201,D0202,D0203,D0204) ")
        //        .Append("select ZLDWDM,sum(D02),sum(D0201HJ),sum(D0202HJ),sum(D0203HJ),sum(D0204HJ) from HZ_JCB ")
        //        .Append(" where TBXHDM='LQYD' group by ZLDWDM ");
        //    sql = sb.ToString();
        //    int iret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_LQFWNYD_BZ(ZLDWDM,D02,D0201,D0202,D0203,D0204) ")
        //        .Append("select left(ZLDWDM,12),sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204) from  HZ_LQFWNYD_BZ ")
        //        .Append(" where  len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //逐级汇总到乡
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_LQFWNYD_BZ(ZLDWDM,D02,D0201,D0202,D0203,D0204) ")
        //        .Append("select left(ZLDWDM,9),sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204) from  HZ_LQFWNYD_BZ ")
        //        .Append(" where  len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //汇总到县
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_LQFWNYD_BZ(ZLDWDM,D02,D0201,D0202,D0203,D0204) ")
        //        .Append("select left(ZLDWDM,6),sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204) from  HZ_LQFWNYD_BZ ")
        //        .Append(" where  len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //}

        ///// <summary>
        ///// 初始化耕地种植类型面积表
        ///// </summary>
        //private void InitGdZzlxTable()
        //{
        //    string sql = "delete from HZ_GDZZLX_BZ";
        //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    //2019年3月21日 根据最新规范修改
        //    sb.Append("insert into HZ_GDZZLX_BZ(ZLDWDM,D01,D0101,D0102,D0103)  select  ZLDWDM ,sum(D01),sum(D0101),sum(D0102),sum(D0103) from HZ_JCB group by ZLDWDM ");
        //    int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString()); //插入耕地总面积
        //    //耕地种植类型
        //    sql = "select ZLDWDM,GDZZSXDM,sum(D01) as mj01,sum(D0101) as mj0101,sum(D0102) as mj0102,sum(D0103) as mj0103 from HZ_JCB where GDZZSXDM<>''  group by ZLDWDM,GDZZSXDM ";
        //    DataTable dt2 = LS_ResultMDBHelper.GetDataTable(sql, "tmp");

        //    foreach (DataRow dr2 in dt2.Rows)
        //    {
        //        string zldwdm = dr2["ZLDWDM"].ToString();
        //        string gdzzlx = dr2["GDZZSXDM"].ToString().Trim().ToUpper();

        //        double dmj01 = 0, dmj0101 = 0, dmj0102 = 0, dmj0103 = 0;
        //        double.TryParse(dr2["mj01"].ToString(), out dmj01);
        //        double.TryParse(dr2["mj0101"].ToString(), out dmj0101);
        //        double.TryParse(dr2["mj0102"].ToString(), out dmj0102);
        //        double.TryParse(dr2["mj0103"].ToString(), out dmj0103);
        //        sb = new StringBuilder();
        //        sb.Append("update HZ_GDZZLX_BZ set D01").Append(gdzzlx).Append(" = ").Append(dmj01).Append(",D0101")
        //            .Append(gdzzlx).Append(" = ").Append(dmj0101).Append(", D0102").Append(gdzzlx).Append(" = ").Append(dmj0102)
        //            .Append(",D0103").Append(gdzzlx).Append(" = ").Append(dmj0103).Append(" where ZLDWDM='").Append(zldwdm).Append("' ");
        //        sql = sb.ToString();
        //        iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    }
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDZZLX_BZ(ZLDWDM,D01,D01LS,D01FLS,D01LYFL,D01XG,D01LLJZ,D01WG,")
        //    .Append(" D0101,D0101LS,D0101FLS,D0101LYFL,D0101XG,D0101LLJZ,D0101WG,")
        //    .Append(" D0102,D0102LS,D0102FLS,D0102LYFL,D0102XG,D0102LLJZ,D0102WG,")
        //    .Append(" D0103,D0103LS,D0103FLS,D0103LYFL,D0103XG,D0103LLJZ,D0103WG) ")
        //    .Append(" select left(ZLDWDM,12), sum(D01),sum(D01LS),sum(D01FLS),sum(D01LYFL),sum(D01XG),sum(D01LLJZ),sum(D01WG),")
        //    .Append(" sum(D0101),sum(D0101LS),sum(D0101FLS),sum(D0101LYFL),sum(D0101XG),sum(D0101LLJZ),sum(D0101WG),")
        //    .Append(" sum(D0102),sum(D0102LS),sum(D0102FLS),sum(D0102LYFL),sum(D0102XG),sum(D0102LLJZ),sum(D0102WG),")
        //    .Append(" sum(D0103),sum(D0103LS),sum(D0103FLS),sum(D0103LYFL),sum(D0103XG),sum(D0103LLJZ),sum(D0103WG) ")
        //    .Append(" from HZ_GDZZLX_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //乡镇级汇总
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDZZLX_BZ(ZLDWDM,D01,D01LS,D01FLS,D01LYFL,D01XG,D01LLJZ,D01WG,")
        //    .Append(" D0101,D0101LS,D0101FLS,D0101LYFL,D0101XG,D0101LLJZ,D0101WG,")
        //    .Append(" D0102,D0102LS,D0102FLS,D0102LYFL,D0102XG,D0102LLJZ,D0102WG,")
        //    .Append(" D0103,D0103LS,D0103FLS,D0103LYFL,D0103XG,D0103LLJZ,D0103WG) ")
        //    .Append(" select left(ZLDWDM,9), sum(D01),sum(D01LS),sum(D01FLS),sum(D01LYFL),sum(D01XG),sum(D01LLJZ),sum(D01WG),")
        //    .Append(" sum(D0101),sum(D0101LS),sum(D0101FLS),sum(D0101LYFL),sum(D0101XG),sum(D0101LLJZ),sum(D0101WG),")
        //    .Append(" sum(D0102),sum(D0102LS),sum(D0102FLS),sum(D0102LYFL),sum(D0102XG),sum(D0102LLJZ),sum(D0102WG),")
        //    .Append(" sum(D0103),sum(D0103LS),sum(D0103FLS),sum(D0103LYFL),sum(D0103XG),sum(D0103LLJZ),sum(D0103WG) ")
        //    .Append(" from HZ_GDZZLX_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    //县级汇总
        //    sb = new StringBuilder();
        //    sb.Append("insert into HZ_GDZZLX_BZ(ZLDWDM,D01,D01,D01LS,D01FLS,D01LYFL,D01XG,D01LLJZ,D01WG,")
        //    .Append(" D0101,D0101LS,D0101FLS,D0101LYFL,D0101XG,D0101LLJZ,D0101WG,")
        //    .Append(" D0102,D0102LS,D0102FLS,D0102LYFL,D0102XG,D0102LLJZ,D0102WG,")
        //    .Append(" D0103,D0103LS,D0103FLS,D0103LYFL,D0103XG,D0103LLJZ,D0103WG) ")
        //    .Append(" select left(ZLDWDM,6), sum(D01),sum(D01LS),sum(D01FLS),sum(D01LYFL),sum(D01XG),sum(D01LLJZ),sum(D01WG),")
        //    .Append(" sum(D0101),sum(D0101LS),sum(D0101FLS),sum(D0101LYFL),sum(D0101XG),sum(D0101LLJZ),sum(D0101WG),")
        //    .Append(" sum(D0102),sum(D0102LS),sum(D0102FLS),sum(D0102LYFL),sum(D0102XG),sum(D0102LLJZ),sum(D0102WG),")
        //    .Append(" sum(D0103),sum(D0103LS),sum(D0103FLS),sum(D0103LYFL),sum(D0103XG),sum(D0103LLJZ),sum(D0103WG) ")
        //    .Append(" from HZ_GDZZLX_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

           

        //}

        //private void InitFdTable( )
        //{
        //    //飞地  土地利用汇总表，2017-12-21修改
        //    string sql = "delete from HZ_FD_BZ ";
        //    int iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("insert into HZ_FD_BZ(ZLDWDM,QSDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append(" select ZLDWDM,QSDWDM,sum(TDZMJ),sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201HJ),sum(D0202HJ),sum(D0203HJ),sum(D0204HJ),")
        //    .Append("sum(D03),sum(D0301HJ),sum(D0302HJ),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307HJ),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403HJ),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2HJ),sum(D0809),sum(D0810HJ),sum(D09),")
        //    .Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104HJ),sum(D1105),sum(D1106),sum(D1107HJ),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_JCB where FRDBS='1'  ")
        //    .Append("group by ZLDWDM,QSDWDM ");
        //    sql = sb.ToString();
        //    iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //村汇总
        //    sb.Clear();
        //    sb.Append("insert into HZ_FD_BZ(ZLDWDM,QSDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append(" select left(ZLDWDM,12),left(QSDWDM,12) , sum(TDZMJ),sum(D00), sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201HJ),sum(D0202HJ),sum(D0203HJ),sum(D0204HJ),")
        //    .Append("sum(D03),sum(D0301HJ),sum(D0302HJ),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307HJ),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403HJ),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2HJ),sum(D0809),sum(D0810HJ),sum(D09),")
        //    .Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104HJ),sum(D1105),sum(D1106),sum(D1107HJ),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_JCB where FRDBS='1' and left(ZLDWDM,12)<>left(QSDWDM,12) ")
        //    .Append(" group by left(ZLDWDM,12),left(QSDWDM,12) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //    //汇总到 乡
        //    sb.Clear();
        //    sb.Append("insert into HZ_FD_BZ(ZLDWDM,QSDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append(" select left(ZLDWDM,9),left(QSDWDM,9),sum(TDZMJ),sum(D00), sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201HJ),sum(D0202HJ),sum(D0203HJ),sum(D0204HJ),")
        //    .Append("sum(D03),sum(D0301HJ),sum(D0302HJ),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307HJ),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403HJ),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2HJ),sum(D0809),sum(D0810HJ),sum(D09),")
        //    .Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104HJ),sum(D1105),sum(D1106),sum(D1107HJ),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_JCB where FRDBS='1' and left(ZLDWDM,9)<>left(QSDWDM,9) ")
        //    .Append(" group by left(ZLDWDM,9),left(QSDWDM,9) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        //     //汇总到县
        //    sb.Clear();
        //    sb.Append("insert into HZ_FD_BZ(ZLDWDM,QSDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,")
        //    .Append("D02,D0201,D0202,D0203,D0204,")
        //    .Append("D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
        //    .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
        //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,")
        //    .Append("D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
        //    .Append("D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
        //    .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
        //    .Append(" select left(ZLDWDM,6),left(QSDWDM,6), sum(TDZMJ),sum(D00), sum(D01),sum(D0101),sum(D0102),sum(D0103),")
        //    .Append("sum(D02),sum(D0201HJ),sum(D0202HJ),sum(D0203HJ),sum(D0204HJ),")
        //    .Append("sum(D03),sum(D0301HJ),sum(D0302HJ),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307HJ),")
        //    .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403HJ),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
        //    .Append("sum(D08),sum(D08H1),sum(D08H2HJ),sum(D0809),sum(D0810HJ),sum(D09),")
        //    .Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
        //    .Append("sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104HJ),sum(D1105),sum(D1106),sum(D1107HJ),sum(D1108),sum(D1109),sum(D1110),")
        //    .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_JCB where FRDBS='1' and left(ZLDWDM,6)<>left(QSDWDM,6) ")
        //    .Append(" group by left(ZLDWDM,6),left(QSDWDM,6) ");
        //    sql = sb.ToString();
        //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            
        //}


        private void DoDLTBMSSM()
        {
            IWorkspace statWS=WorkspaceHelper2.GetAccessWorkspace(RCIS.Global.AppParameters.ConfPath+"\\setup.mdb");
            try
            {
                IFeatureWorkspace pFeaWS=statWS as IFeatureWorkspace;
                IFeatureClass dltbClass = pFeaWS.OpenFeatureClass("DLTB");
                IFeatureClass xzqClass = pFeaWS.OpenFeatureClass("XZQ");
                #region 修改结构
                IField pFld = new FieldClass();
                IFieldEdit2 pFldEdit = pFld as IFieldEdit2;
                pFldEdit.Name_2 = "MSSM";
                pFldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                pFldEdit.Length_2 = 10;
                dltbClass.AddField(pFld);




                #endregion 
                
                //赋值
                IFeatureLayer xzqLyr = new FeatureLayerClass();
                xzqLyr.FeatureClass = xzqClass;
                IIdentify idXZQ = xzqLyr as IIdentify;
                IArray arXzq = idXZQ.Identify((xzqClass as IGeoDataset).Extent);

                for (int i = 0; i < arXzq.Count; i++)
                {
                    IFeatureIdentifyObj obj = arXzq.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aXZQFea = aRow.Row as IFeature;

                    string mssm = FeatureHelper.GetFeatureStringValue(aXZQFea, "MSSM");

                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = aXZQFea.Shape;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    IFeatureCursor dltbCursor = dltbClass.Update(pSF as IQueryFilter, false);
                    IFeature aFeature = null;
                    try
                    {
                        while ((aFeature = dltbCursor.NextFeature()) != null)
                        {
                            FeatureHelper.SetFeatureValue(aFeature, "MSSM", mssm);
                            dltbCursor.UpdateFeature(aFeature);
                        }
                    }
                    catch { }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(dltbCursor);
                    }                  

                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(statWS);
            }

        }

        #endregion

        //基础表计算汇总
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKzmjHD.Text) || string.IsNullOrWhiteSpace(txtKzmjLD.Text))
            {
                MessageBox.Show("请输入控制面积","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;

            }
            this.Cursor = Cursors.WaitCursor;
            this.listBoxControl1.Items.Clear();
            try
            {
                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":开始从矢量中提取地类图斑数据...");
                Application.DoEvents();

                if (System.IO.File.Exists(Application.StartupPath + @"\SystemConf\backup.mdb"))
                {
                    System.IO.File.Delete(Application.StartupPath + @"\SystemConf\result.mdb");
                    System.IO.File.Copy(Application.StartupPath + @"\SystemConf\backup.mdb", Application.StartupPath + @"\SystemConf\result.mdb");
                }
                else
                {
                    //执行，删除已有图层
                    clsOutputData.DeletDLTBXZQ();
                    clsOutputData.CompactResultMdb();
                }

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在提取数据...");
                Application.DoEvents();
                //if (!clsOutputData.SpatialJoin(currWs))
                //{
                //    this.Cursor = Cursors.Default;
                //    MessageBox.Show("空间关系处理不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                clsOutputData.CopyDltb(currWs);
                //clsOutputData.CopyJBNT(currWs);

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在初步进行平方米统计...");
                Application.DoEvents();
                clsOutputData.Dltb2BaseTable2();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在进行生成基础统计...");
                Application.DoEvents();
                clsOutputData.ChangeTMP2JCB("PMMJ");

                double ldmj = 0;
                double.TryParse(txtKzmjLD.Text,out ldmj);
                double hdmj = 0;
                double.TryParse(txtKzmjHD.Text, out hdmj);

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在进行调平.....");
                Application.DoEvents();
                ClsTP tp = new ClsTP();
                tp.ChangeTableDW2GQ("HZ_JCB");  //转化为公顷
                tp.MakeBalance("00",ldmj);
                tp.MakeBalance("01",hdmj);

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按辖区坐落汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按辖区坐落汇总统计...";
                clsOutputData.InitZlTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按权属性质汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按权属性质汇总统计...";
                clsOutputData.InitQsTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按耕地坡度汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按耕地坡度汇总统计...";
                clsOutputData.InitGdTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将城镇村及工矿用地面积汇总表统计...");
                Application.DoEvents();
                clsOutputData.InitCZCGKTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按耕地种植类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGdZzlxTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行林区园地汇总统计...");
                Application.DoEvents();
                clsOutputData.InitLQYDTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行灌丛草地汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGCCDXSCDTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行工业用地类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGYCCTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行耕地细化调查按类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGdxhdcTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行可调整地类面积统计...");
                Application.DoEvents();
                clsOutputData.InitKtzTable();


                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将提取批准未建设数据...");
                Application.DoEvents();
                clsStatsPzwjs pzwjs = new clsStatsPzwjs(this.currWs);
                pzwjs.getPzwjsTmp();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将汇总批准未建设基础表...");
                Application.DoEvents();
                pzwjs.InitPzwjsJCB();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将批准未建设现状情况统计表进行汇总统计...");
                Application.DoEvents();
                pzwjs.initPzwjsXzBzTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将批准未建设建设用地用途情况进行统计...");
                Application.DoEvents();
                pzwjs.InitPzwjsBZTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按飞地汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按飞地汇总统计...";
                clsOutputData.InitFdTable();
                clsOutputData.InitFDQSTable();  //飞地权属
                clsOutputData.InitFd_CZCGKTable(); 

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按海岛面积汇总统计...");
                Application.DoEvents();
                clsOutputData.InitHDTable();

                clsStatsWRHD hdtj = new clsStatsWRHD(this.currWs);
                hdtj.InitWjmHd();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将进行部分地类细化汇总表...");
                Application.DoEvents();
                clsOutputData.InitBfxhdlTable();



                //this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在提取永久基本农田图斑数据表...");
                //Application.DoEvents();
                //clsStatsYjjbnt yjjbnt = new clsStatsYjjbnt(this.currWs);
                //yjjbnt.getYjjbntTmp();
                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将汇总永久基本农田数据并统计...");
                Application.DoEvents();
                //yjjbnt.InitYjjbntJCB();
                //yjjbnt.initYjjbntXzBzTable();
                clsStatsJbnt2 statJbnt = new clsStatsJbnt2(this.currWs);
                statJbnt.getYJJBNTTmp();
                statJbnt.InitYjjbnt();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将进行废弃与垃圾填埋细化标注汇总...");
                Application.DoEvents();
                clsOutputData.InitFQLJTMTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将进行即可恢复与工程恢复种植属性汇总...");
                Application.DoEvents();
                clsOutputData.InitJKHFGCHFTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":基础数据完成初始化。.");
                            

                this.Cursor = Cursors.Default;
                MessageBox.Show("初始化基础库完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            

        }

        

        #region 输出eXxcel表
        //辖区到excel 辖区一级

        //private bool setExcelCellFormat(Excel._Worksheet wSheet1, int rowIndex, int maxValue)
        //{
        //    try
        //    {
        //        //为报表添加单元格格式  by  YHM
        //        for (int k = 1; k <= maxValue; k++)
        //        {
        //            Excel.Range range = wSheet1.get_Range(wSheet1.Cells[rowIndex, k], wSheet1.Cells[rowIndex, k]);
        //            range.Borders.LineStyle = 0.01;     //设置单元格边框的粗细
        //            range.NumberFormatLocal = "@";
        //            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //            //给单元格加边框
        //            range.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    { return false; }
        //}

        public void ExportToExcel11_BfxhdlHzb(DataTable dt)
        {
            string destDir = this.beDestDir.Text.Trim();
            string[] allField = new string[] { "D08H2A", "D0810A", "D1104A", "D1107A", "D20A","D201A",
                "D202A","D203A"    };

            try
            {

                string tmplateFile = RCIS.Global.AppParameters.TemplatePath + @"\部分细化地类面积汇总表.xlsx";
                string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                   + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();

                string excelReportFilename = destDir + @"\部分细化地类面积汇总表" + dateStr + ".xlsx";
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
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@
                int rowIndex = 5;

                cells[2,1].PutValue("单位："+ this.CurrDW);  //单位

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr["ZLDWDM"].ToString().Length > 12)
                    {
                        continue;
                    }
                    if (dr["ZLDWDM"].ToString().Length == 9)
                    {
                        rowIndex++;
                    }
                    int iColNum = 1;

                    cells[rowIndex, 0].SetStyle(styleTxt);
                    cells[rowIndex, 1].SetStyle(styleTxt);   
                    cells[rowIndex, iColNum++].PutValue(dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "");
                    cells[rowIndex, iColNum++].PutValue(dr["ZLDWDM"].ToString());

                    for (int k = 0; k < allField.Length; k++)
                    {
                        double mj = 0;
                        double.TryParse(dr[allField[k]].ToString(), out mj);
                        if (this.CurrDW == "亩")
                            mj *= 15;
                        cells[rowIndex, iColNum].SetStyle(styleNum);
                        cells[rowIndex, iColNum].PutValue(mj.ToString("F2"));

                        iColNum++;
                    }
                    rowIndex++;
                }


                wk.Save(excelReportFilename);
                try
                {
                    System.Diagnostics.Process.Start(excelReportFilename);
                }
                catch (Exception ex)
                {

                }
              

            }
            catch (Exception ex)
            {
               
                throw ex;
            }

        }
        
        //public  void ExportToExcel1_OneXQTJ(DataTable dt)
        //{
        //    string destDir = this.beDestDir.Text.Trim();            

        //    string[] allField = new string[] { "TDZMJ","D00", "D01", "D02", "D03", "D04","D05",
        //        "D06","D07", "D08", "D09","D10","D11","D12"
        //    };

        //    try
        //    {

        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + @"\土地利用现状一级分类面积汇总表.xlsx";
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();

        //        string excelReportFilename = destDir + @"\土地利用现状一级分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);
        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        int rowIndex = 5;

        //        wSheet1.Cells[2, 1] ="单位："+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";

        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;

        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            setExcelCellFormat(wSheet1, rowIndex, 16);

        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(),out mj);
        //                if (this.CurrDW=="亩")
        //                    mj*=15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");

                        
        //            }

        //            rowIndex++;


        //        }
        //        wSheet1.get_Range(wSheet1.Cells[8, 3], wSheet1.Cells[rowIndex, 15]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //        objBook.Save();
        //        objApp.Visible = true;
                

        //    }
        //    catch (Exception ex)
        //    {               
        //        throw ex;
        //    }
            
        //}
        

        //耕地种植类型
        //public void ExportToExcel6_GDZZLX(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;


        //    //string[] allField = new string[] { "D01","D01GZ","D01XG","D01YM","D01LM","D01MC","D01KT","D01LLJZ","D01GSYY","D01SSLM","D01LH","D01WG",
        //    //                                   "D0101","D0101GZ","D0101XG","D0101YM","D0101LM","D0101MC","D0101KT","D0101LLJZ","D0101GSYY","D0101SSLM","D0101LH","D0101WG",
        //    //                                   "D0102","D0102GZ","D0102XG","D0102YM","D0102LM","D0102MC","D0102KT","D0102LLJZ","D0102GSYY","D0102SSLM","D0102LH","D0102WG",
        //    //                                   "D0103","D0103GZ","D0103XG","D0103YM","D0103LM","D0103MC","D0103KT","D0103LLJZ","D0103GSYY","D0103SSLM","D0103LH","D0103WG"
        //    //  };

        //    string[] allField = new string[] {"D01","D01LS","D01FLS","D01LYFL","D01XG","D01LLJZ","D01WG",
        //         "D0101","D0101LS","D0101FLS","D0101LYFL","D0101XG","D0101LLJZ","D0101WG",
        //         "D0102","D0102LS","D0102FLS","D0102LYFL","D0102XG","D0102LLJZ","D0102WG",
        //         "D0103","D0103LS","D0103FLS","D0103LYFL","D0103XG","D0103LLJZ","D0103WG"};

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\耕地种植类型面积统计表.xlsx";
        //        string excelReportFilename = destDir + "\\耕地种植类型面积统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        //根据模版规定
        //        int rowIndex =5;

        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");                       
                        
        //            }
        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);


        //            rowIndex++;
                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
                

        //    }
        //    catch (Exception ex)
        //    {               
        //        throw ex;
        //    }

        //}

        ///// <summary>
        ///// 导出城镇村及工矿用地内部信息统计表
        ///// </summary>
        //public void ExportToExcel4_CZCGKTJ(DataTable dt1)
        //{
           
        //    string destDir = this.beDestDir.Text;           

        //    string[] allField = new string[] { "D20",
        //        "D2000","D200303","D200304","D200306","D200402","D200603","D201105","D201106","D201108",
        //        "D2001","D200101","D200102","D200103",
        //        "D2002","D200201","D200202","D200203","D200204",
        //        "D2003","D200301","D200302","D200305","D200307",
        //        "D2004","D200401","D200403","D200404",
        //        "D2005","D2005H1","D200508",
        //        "D2006","D200601","D200602","D2007","D200701","D200702","D2008","D2008H1","D2008H2","D200809","D200810",
        //        "D2009","D2010","D201001","D201002","D201003","D201004","D201005","D201006","D201007","D201008","D201009",
        //        "D2011","D201101","D201102","D201103","D201104","D201107","D201109","D201110",
        //        "D2012","D201201","D201202","D201203","D201204","D201205","D201206","D201207",

        //        "D201", "D20100","D2010303","D2010304","D2010306","D2010402","D2010603","D2011105","D2011106","D2011108",
        //        "D20101","D2010101","D2010102","D2010103",
        //        "D20102","D2010201","D2010202","D2010203","D2010204",
        //        "D20103","D2010301","D2010302","D2010305","D2010307",
        //        "D20104","D2010401","D2010403","D2010404",
        //        "D20105","D20105H1","D2010508",
        //        "D20106","D2010601","D2010602","D20107","D2010701","D2010702","D20108","D20108H1","D20108H2","D2010809","D2010810",
        //        "D20109","D20110","D2011001","D2011002","D2011003","D2011004","D2011005","D2011006","D2011007","D2011008","D2011009",
        //        "D20111","D2011101","D2011102","D2011103","D2011104","D2011107","D2011109","D2011110",
        //        "D20112","D2011201","D2011202","D2011203","D2011204","D2011205","D2011206","D2011207",
        //        "D202", "D20200","D2030303","D2030304","D2030306","D2020402","D2020603","D2021105","D2021106","D2011008",
        //        "D20201","D2020101","D2020102","D2020103",
        //        "D20202","D2020201","D2020202","D2020203","D2020204",
        //        "D20203","D2030301","D2030302","D2030305","D2030307",
        //        "D20204","D2020401","D2020403","D2020404",
        //        "D20205","D20205H1","D2020508",
        //        "D20206","D2020601","D2020602","D20207","D2020701","D2020702","D20208","D20208H1","D20208H2","D2020809","D2020810",
        //        "D20109","D20110","D2011001","D2011002","D2011003","D2011004","D2011005","D2011006","D2011007","D2011009",
        //        "D20211","D2021101","D2021102","D2021103","D2021104","D2021107","D2021108","D2021109","D2021110",
        //        "D20212","D2021201","D2021202","D2021203","D2021204","D2021205","D2021206","D2021207",

        //         "D203","D20300","D2030303","D2030304","D2030306","D2030402","D2030603","D2031105","D2031106","D2031108",
        //         "D20301","D2030101","D2030102","D2030103",
        //         "D20302","D2030201","D2030202","D2030203","D2030204",
        //         "D20303","D2030301","D2030302","D2030305","D2030307",
        //         "D20304","D2030401","D2030403","D2030404",
        //        "D20305","D20305H1","D2030508",
        //        "D20306","D2030601","D2030602","D20307","D2030701","D2030702","D20308","D20308H1","D20308H2","D2030809",
        //        "D2030810","D20309","D20310","D2031001","D2031002","D2031003","D2031004","D2031005","D2031006","D2031007","D2031008",
        //        "D2031009","D20311","D2031101","D2031102","D2031103","D2031104","D2031107","D2031109",
        //        "D2031110","D20312","D2031201","D2031202","D2031203","D2031204","D2031205","D2031206","D2031207",
        //        "D204","D2040602","D2040603","D2041201", "D20509"};

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\城镇村及工矿用地面积汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\城镇村及工矿用地面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        //根据模版规定
        //        int rowIndex = 6;

        //        wSheet1.Cells[2,1] ="单位:"+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt1.Rows.Count; i++)
        //        {
        //           // wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 271]).NumberFormat = "@";
        //            DataRow dr = dt1.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;

        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
                        
                        
        //            }
        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;                    
        //        }
                

        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //飞地城镇村及工矿用地
        //public void ExportToExcel18_FD_CZCTable(DataTable dt1)
        //{
        //    string destDir = this.beDestDir.Text;

        //    string[] allField = new string[] { "D20",
        //        "D2000","D200303","D200304","D200306","D200402","D200603","D201105","D201106","D201108",
        //        "D2001","D200101","D200102","D200103",
        //        "D2002","D200201","D200202","D200203","D200204",
        //        "D2003","D200301","D200302","D200305","D200307",
        //        "D2004","D200401","D200403","D200404",
        //        "D2005","D2005H1","D200508",
        //        "D2006","D200601","D200602","D2007","D200701","D200702","D2008","D2008H1","D2008H2","D200809","D200810",
        //        "D2009","D2010","D201001","D201002","D201003","D201004","D201005","D201006","D201007","D201008","D201009",
        //        "D2011","D201101","D201102","D201103","D201104","D201107","D201109","D201110",
        //        "D2012","D201201","D201202","D201203","D201204","D201205","D201206","D201207",

        //        "D201", "D20100","D2010303","D2010304","D2010306","D2010402","D2010603","D2011105","D2011106","D2011108",
        //        "D20101","D2010101","D2010102","D2010103",
        //        "D20102","D2010201","D2010202","D2010203","D2010204",
        //        "D20103","D2010301","D2010302","D2010305","D2010307",
        //        "D20104","D2010401","D2010403","D2010404",
        //        "D20105","D20105H1","D2010508",
        //        "D20106","D2010601","D2010602","D20107","D2010701","D2010702","D20108","D20108H1","D20108H2","D2010809","D2010810",
        //        "D20109","D20110","D2011001","D2011002","D2011003","D2011004","D2011005","D2011006","D2011007","D2011008","D2011009",
        //        "D20111","D2011101","D2011102","D2011103","D2011104","D2011107","D2011109","D2011110",
        //        "D20112","D2011201","D2011202","D2011203","D2011204","D2011205","D2011206","D2011207",
        //        "D202", "D20200","D2030303","D2030304","D2030306","D2020402","D2020603","D2021105","D2021106","D2011008",
        //        "D20201","D2020101","D2020102","D2020103",
        //        "D20202","D2020201","D2020202","D2020203","D2020204",
        //        "D20203","D2030301","D2030302","D2030305","D2030307",
        //        "D20204","D2020401","D2020403","D2020404",
        //        "D20205","D20205H1","D2020508",
        //        "D20206","D2020601","D2020602","D20207","D2020701","D2020702","D20208","D20208H1","D20208H2","D2020809","D2020810",
        //        "D20109","D20110","D2011001","D2011002","D2011003","D2011004","D2011005","D2011006","D2011007","D2011009",
        //        "D20211","D2021101","D2021102","D2021103","D2021104","D2021107","D2021108","D2021109","D2021110",
        //        "D20212","D2021201","D2021202","D2021203","D2021204","D2021205","D2021206","D2021207",

        //         "D203","D20300","D2030303","D2030304","D2030306","D2030402","D2030603","D2031105","D2031106","D2031108",
        //         "D20301","D2030101","D2030102","D2030103",
        //         "D20302","D2030201","D2030202","D2030203","D2030204",
        //         "D20303","D2030301","D2030302","D2030305","D2030307",
        //         "D20304","D2030401","D2030403","D2030404",
        //        "D20305","D20305H1","D2030508",
        //        "D20306","D2030601","D2030602","D20307","D2030701","D2030702","D20308","D20308H1","D20308H2","D2030809",
        //        "D2030810","D20309","D20310","D2031001","D2031002","D2031003","D2031004","D2031005","D2031006","D2031007","D2031008",
        //        "D2031009","D20311","D2031101","D2031102","D2031103","D2031104","D2031107","D2031109",
        //        "D2031110","D20312","D2031201","D2031202","D2031203","D2031204","D2031205","D2031206","D2031207",
        //        "D204","D2040602","D2040603","D2041201", "D20509"};

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\飞入地城镇村及工矿用地面积汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\飞入地城镇村及工矿用地面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        //根据模版规定
        //        int rowIndex = 6;

        //        wSheet1.Cells[2,1] ="单位:"+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt1.Rows.Count; i++)
        //        {
        //            DataRow dr = dt1.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["QSDWDM"].ToString()) ? dicQsdwdm[dr["QSDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["QSDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");

                       

        //            }
        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;
        //        }


        //        objBook.Save();
        //        objApp.Visible = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 导出永久基本农田现状数据
        ///// </summary>
        ///// <param name="dt"></param>
        //public void ExportToExcel20_Yjjbnt(DataTable dt)
        //{
        //    string destDir = this.beDestDir.Text;

        //    //string[] allField = new string[] {"TDZMJ","D00","D0303","D0304","D0306","D0402","D0603","D1105","D1106","D1108",
        //    //    "D01","D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
        //    //    "D03","D0301","D0302","D0305","D0307","D04","D0401","D0403","D0404",
        //    //    "D05","D05H1","D0508", "D06", "D0601", "D0602",
        //    //    "D07", "D0701", "D0702", "D08","D08H1", "D08H2", "D0809", "D0810","D09","D10",
        //    //    "D1001", "D1002", "D1003", "D1004","D1005", "D1006","D1007", "D1008", "D1009",
        //    //    "D11","D1101","D1102","D1103","D1104","D1107","D1109","D1110",
        //    //    "D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207"
        //    //};

        //    string[] allField = new string[] {"YJJBNTMJ","D01","D0101","D0102","D0103","DQT","D00","D02","D03",
        //        "D04","D05","D06","D07",  "D08","D09","D10","D11","D12"
        //    };
            
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\永久基本农田现状情况统计表.xlsx";
        //        string excelReportFilename = destDir + "\\永久基本农田现状情况统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[3, 20] = this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 7;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");

        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);
        //            rowIndex++;

        //        }


        //        objBook.Save();
        //        objApp.Visible = true;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
            

        //}

        //public void ExportToExcel2_TwoXQTJ(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;

        //    string[] allField = new string[] {"TDZMJ","D00","D0303","D0304","D0306","D0402","D0603","D1105","D1106","D1108",
        //        "D01","D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
        //        "D03","D0301","D0302","D0305","D0307","D04","D0401","D0403","D0404",
        //        "D05","D05H1","D0508", "D06", "D0601", "D0602",
        //        "D07", "D0701", "D0702", "D08","D08H1", "D08H2", "D0809", "D0810","D09","D10",
        //        "D1001", "D1002", "D1003", "D1004","D1005", "D1006","D1007", "D1008", "D1009",
        //        "D11","D1101","D1102","D1103","D1104","D1107","D1109","D1110",
        //        "D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207"
        //    };

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状二级分类面积汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\土地利用现状二级分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 5;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");

                        
        //            }


        //            setExcelCellFormat(wSheet1, rowIndex, iColNum-1);

        //            rowIndex++;
                    
        //        }


        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw ex;
        //    }
            

            

        //}

        //public void ExportToExcel5_GDPD(DataTable dt)
        //{           
        //    string[] allField = new string[] { "TOTALAREA","D2","D26","D26T","D26P","D615","D615T","D615P","D1525",
        //        "D1525T","D1525P","D25","D25T","D25P" };
           
        //    string destDir = this.beDestDir.Text;
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //          + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\耕地坡度分级面积汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\耕地坡度分级面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);


        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        int rowIndex = 6;

        //        wSheet1.Cells[2,1] ="单位:"+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];

        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColIndx = 1;
        //            wSheet1.Cells[rowIndex, iColIndx++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";                    
        //            wSheet1.Cells[rowIndex, iColIndx++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColIndx++] = mj.ToString("F2");
        //            }

                   

        //            setExcelCellFormat(wSheet1, rowIndex, iColIndx-1);
        //            rowIndex++;
                    
        //        }
                
        //        objBook.Save();
        //        objApp.Visible = true;
               

        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw ex;
        //    }
         
        //}
        
        //public void ExportToExcel3_QSXZ(DataTable dt)
        //{
        //    //2017-12-25日修改，按照权属一级分类导出     
            
        //    string[] allField = new string[] { "TOTALAREA","TOTALAREAG","TOTALAREAJ","D00","D00G","D00J","D01","D01G","D01J","D02","D02G","D02J","D03","D03G","D03J",
        //                "D04","D04G","D04J","D05","D05G","D05J","D06","D06G","D06J",
        //                "D07","D07G","D07J","D08","D08G","D08J","D09","D09G","D09J","D10","D10G","D10J",
        //                "D11","D11G","D11J","D12","D12G","D12J" };
            
        //    string destDir = this.beDestDir.Text;

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //                      + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\土地利用现状一级分类面积按权属性质汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\土地利用现状一级分类面积按权属性质汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);


        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[2,1] = this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 6;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 1], wSheet1.Cells[rowIndex, 41]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColIdx = 1;
        //            wSheet1.Cells[rowIndex, iColIdx++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";

        //            #region Sheet1


        //            wSheet1.Cells[rowIndex, iColIdx++] = dr["ZLDWDM"].ToString();
        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColIdx++] = mj.ToString("F2");
        //            }


        //            #endregion

        //            setExcelCellFormat(wSheet1, rowIndex, iColIdx-1);
        //            rowIndex++;
                    
        //        }

        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {               
        //        throw ex;
        //    }
            


        //}

        //public void ExportToExcel17_FDQSXZ(DataTable dt)
        //{
        //    string[] allField = new string[] { "TOTALAREA","TOTALAREAG","TOTALAREAJ","D00","D00G","D00J","D01","D01G","D01J","D02","D02G","D02J","D03","D03G","D03J",
        //                "D04","D04G","D04J","D05","D05G","D05J","D06","D06G","D06J",
        //                "D07","D07G","D07J","D08","D08G","D08J","D09","D09G","D09J","D10","D10G","D10J",
        //                "D11","D11G","D11J","D12","D12G","D12J" };
           
        //    string destDir = this.beDestDir.Text;

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //                      + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\飞入地土地利用现状一级分类面积按权属性质汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\飞入地土地利用现状一级分类面积按权属性质汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[2,1] ="单位"+ this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 6;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 1], wSheet1.Cells[rowIndex, 43]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColIdx = 1;
        //            wSheet1.Cells[rowIndex, iColIdx++] = (i + 1).ToString(); //序号
        //            wSheet1.Cells[rowIndex, iColIdx++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColIdx++] = dr["ZLDWDM"].ToString();
        //            wSheet1.Cells[rowIndex, iColIdx++] = dicQsdwdm.ContainsKey(dr["QSDWDM"].ToString()) ? dicQsdwdm[dr["QSDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColIdx++] = dr["QSDWDM"].ToString();

        //            #region Sheet1
                    
                    
        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColIdx++] = mj.ToString("F2");
        //            }


        //            #endregion

        //            setExcelCellFormat(wSheet1, rowIndex, iColIdx - 1);
        //            rowIndex++;
                    
        //        }

        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
               
        //    }
            
        //}

        ////批准未建设情况
        //public void ExportToExcel13_Pzwjstd(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;
        //    DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始导出批准未建设建设用地用途情况统计表...", "请稍等...");
        //    wait.Show();

        //    string[] allField = new string[] { "DHJ","D00","D0603", "D05","D05H1","D0508", "D06", "D0601", "D0602",
        //        "D07", "D0701", "D0702", "D08","D08H1", "D08H2", "D0809", "D0810","D09","D10",
        //        "D1001", "D1002", "D1003", "D1004","D1005", "D1007", "D1008", "D1009",
        //        "D11","D1109"
        //    };

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\批准未建设的建设用地用途情况统计表.xlsx";
        //        string excelReportFilename = destDir + "\\批准未建设的建设用地用途情况统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);


        //        wSheet1.Cells[2,1] ="用途:"+ this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 5;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }
        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;
        //            wait.SetCaption("已经导出" + rowIndex + "条数据...");
        //        }

        //        objBook.Save();
        //        objApp.Visible = true;
        //        wait.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (wait != null)
        //            wait.Close();
        //        throw ex;
        //    }
        //}


        ////批准未建设情况占用现状
        //public void ExportToExcel14_PzwjsXz(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;

        //    string[] allField = new string[] { "TDZMJ","D00","D0303","D0304","D0306","D0402","D0603","D1105","D1106","D1108",
        //        "D01","D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
        //        "D03","D0301","D0302","D0305","D0307","D04","D0401","D0403","D0404",
        //        "D05","D05H1","D0508", "D06", "D0601", "D0602",
        //        "D07", "D0701", "D0702", "D08","D08H1", "D08H2", "D0809", "D0810","D09","D10",
        //        "D1001", "D1002", "D1003", "D1004","D1005", "D1006","D1007", "D1008", "D1009",
        //        "D11","D1101","D1102","D1103","D1104","D1107","D1109","D1110",
        //        "D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207"
        //    };

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\批准未建设的建设用地现状情况统计表.xlsx";
        //        string excelReportFilename = destDir + "\\批准未建设的建设用地现状情况统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[2,1] ="现状："+ this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 5;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;
                   
        //        }

        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw ex;
        //    }
        //}



        ////耕地细化调查统计表
        //public void ExportToExcel12_GDXHTJB(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;
            
        //    string[] allField = new string[] { "D01", "DHDGD01", "DHDGD0101", "DHDGD0102", "DHDGD0103",
        //    "DHQGD01", "DHQGD0101", "DHQGD0102", "DHQGD0103","DLQGD01", "DLQGD0101", "DLQGD0102", "DLQGD0103",
        //    "DMQGD01", "DMQGD0101", "DMQGD0102", "DMQGD0103","DSHGD01", "DSHGD0101", "DSHGD0102", "DSHGD0103",
        //    "DSMGD01", "DSMGD0101", "DSmGD0102", "DSMGD0103"
        //    };

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\耕地细化调查情况统计表.xlsx";
        //        string excelReportFilename = destDir + "\\耕地细化调查情况统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        //根据模版规定
        //        int rowIndex = 5;
        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }
        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;
                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw ex;
        //    }
        //}

        ////工业用地
        //public void Exporttoexcel9_GYCCYD(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;
            
        //    string[] allField = new string[] { "D0601", "DHDGY", "DGTGY", "DMTGY", "DSNGY" ,"DBLGY","DDLGY"};

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\工业用地按类型汇总统计表.xlsx";
        //        string excelReportFilename = destDir + "\\工业用地按类型汇总统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        //根据模版规定
        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        int rowIndex = 5;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
                    
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);


        //            rowIndex++;
                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {              
        //        throw ex;
        //    }
        //}

        ////灌丛草地，输出
        //public void ExportToExcel8_GCXSCD(DataTable dt)
        //{
           
        //    string destDir = this.beDestDir.Text;
           
        //    string[] allField = new string[] { "DGCCD", "DGCCD0401", "DGCCD0402", "DGCCD0403", "DGCCD0404" };

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\灌丛草地汇总情况统计表.xlsx";
        //        string excelReportFilename = destDir + "\\灌丛草地汇总情况统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 5;
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
                    
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }
        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;
                   
        //        }

        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {               
        //        throw ex;
        //    }
        //}

        ////基本农田外可调整
        //public void ExportToExcel10_Ktz(DataTable dt)
        //{
           
        //    string destDir = this.beDestDir.Text;
            
        //    string[] allField = new string[] {"DKHJ", "D0201K", "D0202K", "D0203K", "D0204K", "D0301K","D0302K","D0307K","D0403K","D1104K" };

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\基本农田外可调整地类面积汇总表.xlsx";
        //        string excelReportFilename = destDir + "\\基本农田外可调整地类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        wSheet1.Cells[2,1] ="单位:"+ this.CurrDW;  //单位

        //        //根据模版规定
        //        int rowIndex = 5;

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
                    
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;
                    
        //        }

        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
              
        //        throw ex;
        //    }
        //}

        //public void ExportToExcel19_OneHDTJ(DataTable dt)
        //{

           
        //    string[] allField = new string[] { "TDZMJ", "D00","D01", "D02", "D03", "D04","D05",
        //        "D06","D07", "D08", "D09","D10","D11","D12"
        //    };
            
        //    string destDir = this.beDestDir.Text;
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //          + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\海岛土地利用现状一级分类面积汇总表.xlsx";

        //        string excelReportFilename = destDir + "\\海岛土地利用现状一级分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        int rowIndex = 5;
        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
                   
        //            DataRow dr = dt.Rows[i];
        //            setExcelCellFormat(wSheet1, rowIndex, 18);
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = (i + 1).ToString();
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["HDMC"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);

        //            rowIndex++;

                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw ex;
        //    }


        //}

        //public void ExportToExclel20_HDTJ(DataTable dt)
        //{
        //    string[] allField = new string[] {"TDZMJ","D00","D0303","D0304","D0306","D0402","D0603","D1105","D1106","D1108",
        //        "D01","D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
        //        "D03","D0301","D0302","D0305","D0307","D04","D0401","D0403","D0404",
        //        "D05","D05H1","D0508", "D06", "D0601", "D0602",
        //        "D07", "D0701", "D0702", "D08","D08H1", "D08H2", "D0809", "D0810","D09","D10",
        //        "D1001", "D1002", "D1003", "D1004","D1005", "D1006","D1007", "D1008", "D1009",
        //        "D11","D1101","D1102","D1103","D1104","D1107","D1109","D1110",
        //        "D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207"
        //    };
           
        //    string destDir = this.beDestDir.Text;
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //          + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\海岛土地利用现状二级分类面积汇总表.xlsx";

        //        string excelReportFilename = destDir + "\\海岛土地利用现状二级分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);


        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        //根据模版规定
        //        int rowIndex = 5;
        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
                    
        //            DataRow dr = dt.Rows[i];
        //            #region Sheet1

        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = (i + 1).ToString(); //序号
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["HDMC"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum-1);

        //            #endregion

        //            rowIndex++;

                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
               
        //    }
        //    catch (Exception ex)
        //    {
              
        //        throw ex;
        //    }

        //}

        ////飞入地二级分类
        //public void ExportToExcel16_TwoFDTJ(DataTable dt)
        //{

        //    string[] allField = new string[] {"TDZMJ","D00","D0303","D0304","D0306","D0402","D0603","D1105","D1106","D1108",
        //        "D01","D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
        //        "D03","D0301","D0302","D0305","D0307","D04","D0401","D0403","D0404",
        //        "D05","D05H1","D0508", "D06", "D0601", "D0602",
        //        "D07", "D0701", "D0702", "D08","D08H1", "D08H2", "D0809", "D0810","D09","D10",
        //        "D1001", "D1002", "D1003", "D1004","D1005", "D1006","D1007", "D1008", "D1009",
        //        "D11","D1101","D1102","D1103","D1104","D1107","D1109","D1110",
        //        "D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207"
        //    };
           
        //    string destDir = this.beDestDir.Text;
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //          + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\飞入地土地利用现状二级分类面积汇总表.xlsx";

        //        string excelReportFilename = destDir + "\\飞入地土地利用现状二级分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);


        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        //根据模版规定
        //        int rowIndex = 5;
        //        wSheet1.Cells[2,1] ="单位:"+this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 4], wSheet1.Cells[rowIndex, 4]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            #region Sheet1

        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = (i + 1).ToString(); //序号
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["QSDWDM"].ToString()) ? dicQsdwdm[dr["QSDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["QSDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum);

        //            #endregion

        //            rowIndex++;

        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
               
        //    }
        //    catch (Exception ex)
        //    {
              
        //        throw ex;
        //    }

        //}
        
        //public void ExportToExcel15_OneFDTJ(DataTable dt)
        //{

            
        //    string[] allField = new string[] { "TDZMJ", "D00", "D01", "D02", "D03", "D04","D05",
        //        "D06","D07", "D08", "D09","D10","D11","D12"
        //    };

            
        //    string destDir = this.beDestDir.Text;
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //          + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\飞入地土地利用现状一级分类面积汇总表.xlsx";

        //        string excelReportFilename = destDir + "\\飞入地土地利用现状一级分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        int rowIndex = 5;
        //        wSheet1.Cells[2,1] ="单位:"+ this.CurrDW;  //单位
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 4], wSheet1.Cells[rowIndex, 4]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            setExcelCellFormat(wSheet1, rowIndex, 17);
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = (i + 1).ToString(); //序号
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["QSDWDM"].ToString()) ? dicQsdwdm[dr["QSDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["QSDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }
        //            rowIndex++;                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
                
        //    }
        //    catch (Exception ex)
        //    {
              
        //        throw ex;
        //    }


        //}

        //public void ExportToExcel21_WJMHD(DataTable dt)
        //{
        //    string[] allField = new string[] { "HDMJ","I1", "I101", "I102", "I103", "I104", "I105","I106",
        //        "I107","I108", "I109", "I110","I2","I201","I202","I203"
        //    };


        //    string destDir = this.beDestDir.Text;
        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //          + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\无居民海岛现状调查分类面积汇总表.xlsx";

        //        string excelReportFilename = destDir + "\\无居民海岛现状调查分类面积汇总表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);
        //        int rowIndex = 7;
        //        wSheet1.Cells[3, 19] = this.CurrDW;  //单位
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 2], wSheet1.Cells[rowIndex, 2]).NumberFormat = "@";
        //            wSheet1.get_Range(wSheet1.Cells[rowIndex, 4], wSheet1.Cells[rowIndex, 4]).NumberFormat = "@";
        //            DataRow dr = dt.Rows[i];
        //            setExcelCellFormat(wSheet1, rowIndex, 17);
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();                   
        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }
        //            rowIndex++;
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}


      

        ////林区园地
        //public void ExportToExcel7_LQYD(DataTable dt)
        //{
            
        //    string destDir = this.beDestDir.Text;           
        //    string[] allField = new string[] { "D02","D0201","D0202","D0203","D0204"};

        //    try
        //    {
        //        string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        //           + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString();
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + "\\林区范围内园地汇总统计表.xlsx";
        //        string excelReportFilename = destDir + "\\林区范围内种植园用地汇总统计表" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Excel.Application objApp = new Excel.ApplicationClass();
        //        objApp.Visible = false;
        //        Excel.Workbooks objWorkbooks = objApp.Workbooks;
        //        object o = Type.Missing;
        //        Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
        //        Excel.Sheets objSheets = objBook.Worksheets;
        //        Excel._Worksheet wSheet1 = (Excel._Worksheet)objSheets.get_Item(1);

        //        //根据模版规定
        //        int rowIndex = 5;
        //        wSheet1.Cells[2,1] ="单位："+ this.CurrDW;  //单位

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
                    
        //            DataRow dr = dt.Rows[i];
        //            if (dr["ZLDWDM"].ToString().Length > 12)
        //            {
        //                continue;
        //            }
        //            if (dr["ZLDWDM"].ToString().Length == 9)
        //            {
        //                rowIndex++;
        //            }
        //            int iColNum = 1;
        //            wSheet1.Cells[rowIndex, iColNum++] = dicQsdwdm.ContainsKey(dr["ZLDWDM"].ToString()) ? dicQsdwdm[dr["ZLDWDM"].ToString()] : "";
        //            wSheet1.Cells[rowIndex, iColNum++] = dr["ZLDWDM"].ToString();

        //            for (int k = 0; k < allField.Length; k++)
        //            {
        //                double mj = 0;
        //                double.TryParse(dr[allField[k]].ToString(), out mj);
        //                if (this.CurrDW == "亩")
        //                    mj *= 15;
        //                wSheet1.Cells[rowIndex, iColNum++] = mj.ToString("F2");
        //            }

        //            setExcelCellFormat(wSheet1, rowIndex, iColNum - 1);
        //            rowIndex++;
                    
        //        }
        //        objBook.Save();
        //        objApp.Visible = true;
               
        //    }
        //    catch (Exception ex)
        //    {
               
        //        throw ex;
        //    }
        //}
        #endregion 

        //报表输出
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.beDestDir.Text.Trim() == "") return;
            //报表输出
            if (this.tvXzq.SelectedNode == null)
            {
                MessageBox.Show("请首先选择某个行政区划。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string zldwdm = OtherHelper.GetLeftName(this.tvXzq.SelectedNode.Text);

                    
            if (dicQsdwdm.Count == 0)
                dicQsdwdm = clsOutputData.getZldwdmMc(currWs);
            DataTable dt = null;// this.GetDataTable(zldwdm, this.radioGroup1.SelectedIndex);
            
            this.Cursor = Cursors.WaitCursor;    
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始导出汇总表...", "请稍等...");
            wait.Show();

            try
            {
                
                if (this.chkTab1.Checked)//一级
                {
                    wait.SetCaption("正在导出表"+this.chkTab1.Text);
                    Application.DoEvents();
                    dt = clsOutputData.GetDataTable(zldwdm, 0);
                    clsOutputData.ExportToExcel1_OneXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab2.Checked)//二级
                {
                    wait.SetCaption("正在导出表" + this.chkTab2.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 1);
                    //clsOutputData.ExportToExcel2_TwoXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);

                    //替换用其他 Excel写入组件，提高速度
                    try
                    {
                        string destfile= clsOutputData.ExportToExcel2_TwoXQTJ2(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                        System.Diagnostics.Process.Start(destfile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (this.chkTab3.Checked)//权属
                {
                    wait.SetCaption("正在导出表" + this.chkTab3.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 2);
                    clsOutputData.ExportToExcel3_QSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab4.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab4.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 3);
                    clsOutputData.ExportToExcel4_CZCGKTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);//城镇村及工况用地
                }
                if (this.chkTab5.Checked)//耕地
                {
                    wait.SetCaption("正在导出表" + this.chkTab5.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 4);
                    clsOutputData.ExportToExcel5_GDPD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab6.Checked) // 种植类型
                {
                    wait.SetCaption("正在导出表" + this.chkTab6.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 5);
                    clsOutputData.ExportToExcel6_GDZZLX(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab7.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab7.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 6);
                    clsOutputData.ExportToExcel7_LQYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);  //林区园地
                }
                if (this.chkTab8.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab8.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 7);
                    clsOutputData.ExportToExcel8_GCXSCD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab9.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab9.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 8);
                    clsOutputData.Exporttoexcel9_GYCCYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab10.Checked)
                {
                    // 可调整
                    wait.SetCaption("正在导出表" + this.chkTab10.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 9);
                    clsOutputData.ExportToExcel10_Ktz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab11.Checked)
                {
                    //部分细化地类
                    wait.SetCaption("正在导出表" + this.chkTab11.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 10);
                    clsOutputData.ExportToExcel11_BfxhdlHzb(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab12.Checked)
                {
                    //耕地细化调查情况统计表
                    wait.SetCaption("正在导出表" + this.chkTab12.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 11);
                    clsOutputData.ExportToExcel12_GDXHTJB(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab13.Checked)
                {
                    //批准未建设
                    wait.SetCaption("正在导出表" + this.chkTab13.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 12);
                    clsOutputData.ExportToExcel13_Pzwjstd(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab14.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab14.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 13);
                    clsOutputData.ExportToExcel14_PzwjsXz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFRD1.Checked)//飞地一级
                {
                    wait.SetCaption("正在导出表" + this.chkFRD1.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 14);
                    clsOutputData.ExportToExcel15_OneFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFrd2.Checked)//飞地二级
                {
                    wait.SetCaption("正在导出表" + this.chkFrd2.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 15);
                    clsOutputData.ExportToExcel16_TwoFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFrd3.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkFrd3.Text);
                    //飞地权属
                    dt = clsOutputData.GetDataTable(zldwdm, 16);
                    clsOutputData.ExportToExcel17_FDQSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFrd4.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkFrd4.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 17);
                    clsOutputData.ExportToExcel18_FD_CZCTable(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkHd1.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkHd1.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 18);
                    clsOutputData.ExportToExcel19_OneHDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkHd2.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkHd2.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 19);
                    clsOutputData.ExportToExclel20_HDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkYjjbntXz.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkYjjbntXz.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 20);
                    clsOutputData.ExportToExcel20_Yjjbnt(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkWrHdxz.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkWrHdxz.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 21);
                    clsOutputData.ExportToExcel21_WJMHD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);

                }
                if (this.chkFQLJTM.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkFQLJTM.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 22);
                    clsOutputData.ExportToExclel22_FQLJTM(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkGDHF.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkGDHF.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 23);
                    clsOutputData.ExportToExclel23_JKHFGCHF(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                wait.Close();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Default;
            }
            
            
        }

        
        private Dictionary<string, string> dicQsdwdm = new Dictionary<string, string>();
        private void StatesForm_Load(object sender, EventArgs e)
        {
            this.beDestDir.Text = AppParameters.OutputPath + "\\Excel";
            LoadTreeFromQsdm();

        }

        /// <summary>
        /// 获取全书代码表，生辰树状结构
        /// </summary>
        private void LoadTreeFromQsdm()
        {
            this.tvXzq.Nodes.Clear();
            ITable pTable = null;
            if (this.currWs == null)
            {

                return;
            }
            try
            {
                pTable = (this.currWs as IFeatureWorkspace).OpenTable("QSDWDMB");
                List<string> lstXian = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 6);
                List<string> lstXiang = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 9);
                List<string> lstCun = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 12);
                foreach (string aXian in lstXian)
                {
                    TreeNode axianNode = tvXzq.Nodes.Add(aXian);
                    string aXianDm = OtherHelper.GetLeftName(aXian);
                    foreach (string aXiang in lstXiang)
                    {
                        string xiangDm = OtherHelper.GetLeftName(aXiang);
                        if (xiangDm.StartsWith(aXianDm))
                        {
                            TreeNode aXiangNode = axianNode.Nodes.Add(aXiang);
                            foreach (string aCun in lstCun)
                            {
                                string aCunDm = OtherHelper.GetLeftName(aCun);
                                if (aCunDm.StartsWith(xiangDm))
                                {
                                    aXiangNode.Nodes.Add(aCun);
                                }
                            }
                        }

                    }
                }

                if (this.tvXzq.Nodes.Count > 0)
                {
                    this.tvXzq.Nodes[0].Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestDir.Text = dlg.SelectedPath;
        }

        private void btnOutXlsAll_Click(object sender, EventArgs e)
        {
            //输出所有报表
            if (this.beDestDir.Text.Trim() == "") return;
            //报表输出
            if (this.tvXzq.SelectedNode == null)
            {
                MessageBox.Show("请首先选择某个行政区划。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string zldwdm = OtherHelper.GetLeftName(this.tvXzq.SelectedNode.Text);

            this.Cursor = Cursors.WaitCursor;
            if (dicQsdwdm.Count == 0)
                dicQsdwdm = clsOutputData.getZldwdmMc(currWs);
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始导出...", "正在导出，请稍候...");
            wait.Show();
            try
            {
                for (int i = 0; i <= 21; i++)
                {
                    wait.SetCaption("正在导出第" + i + "个表...");
                    Application.DoEvents();
                    DataTable dt = clsOutputData.GetDataTable(zldwdm, i);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        continue;
                    }
                    switch(i)
                    {
                        case 0:
                            clsOutputData.ExportToExcel1_OneXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 1:
                            clsOutputData.ExportToExcel2_TwoXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 2:
                            clsOutputData.ExportToExcel3_QSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 3:
                            clsOutputData.ExportToExcel4_CZCGKTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 4:
                            clsOutputData.ExportToExcel5_GDPD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 5:
                            clsOutputData.ExportToExcel6_GDZZLX(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 6:
                            clsOutputData.ExportToExcel7_LQYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 7 :
                            clsOutputData.ExportToExcel8_GCXSCD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 8:
                            clsOutputData.Exporttoexcel9_GYCCYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 9:
                            clsOutputData.ExportToExcel10_Ktz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 10:
                            clsOutputData.ExportToExcel11_BfxhdlHzb(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 11:
                            clsOutputData.ExportToExcel12_GDXHTJB(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 12:
                            clsOutputData.ExportToExcel13_Pzwjstd(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 13:
                            clsOutputData.ExportToExcel14_PzwjsXz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 14:
                            clsOutputData.ExportToExcel15_OneFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 15:
                            clsOutputData.ExportToExcel16_TwoFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 16:
                            clsOutputData.ExportToExcel17_FDQSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 17:
                            clsOutputData.ExportToExcel18_FD_CZCTable(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 18:
                            clsOutputData.ExportToExcel19_OneHDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 19:
                            clsOutputData.ExportToExclel20_HDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 20:
                            clsOutputData.ExportToExcel20_Yjjbnt(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 21:
                            clsOutputData.ExportToExcel21_WJMHD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        
                    }
                }
                
                this.Cursor = Cursors.Default;
                wait.Close();
                MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
                
            }
            
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //全选
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = true;
                }
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = !ce.Checked;
                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //记录为年初面积
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery("insert into HZ_ZL_BZ_NC select * from HZ_ZL_BZ ");
            if (iret > 0)
            {
                MessageBox.Show("写入" + iret + "条记录成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("写入失败！");
            }
        }


    }
}
