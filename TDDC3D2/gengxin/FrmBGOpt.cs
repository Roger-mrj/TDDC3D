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
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Geometry;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Collections;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.gengxin
{
    public partial class FrmBGOpt : Form
    {
        public IMap currMap = null;
        public IWorkspace currWs = null;

        public FrmBGOpt()
        {
            InitializeComponent();
        }

        private void FrmBGOpt_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap);
            try {
                if (Directory.Exists(Application.StartupPath + @"\tmp\tmp.gdb"))
                {
                    IWorkspace pWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                    IDataset pDataset = pWS as IDataset;
                    pDataset.Delete();

                }
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
            }
            catch(Exception ex){}


            dateEdit1.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            dateEdit1.Properties.ShowToday = false;
            //dateEdit1.Properties.ShowM = false;
            dateEdit1.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dateEdit1.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
            dateEdit1.Properties.Mask.EditMask = "yyyy";
            dateEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
            string txtData = (System.DateTime.Now.Year-1).ToString();
            dateEdit1.EditValue = System.DateTime.Now.AddYears(-1);
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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


        private void btnOK_Click(object sender, EventArgs e)
        {
            //try
            //{
            //判断图层名是否填写，并且该图层在数据库中
            if (this.cmbLayers.Text.Trim() == "")
                return;
            string ClassName = this.cmbLayers.Text.Trim();
            ClassName = OtherHelper.GetLeftName(ClassName);
            if (ClassName == "DLTB")
            {
                MessageBox.Show("当前这个层不应该选择DLTB吧！");
                return;
            }
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureClass dltbClass = null;
            IFeatureClass dltbGX = null;
            try
            {
                dltbGX = pFeaWs.OpenFeatureClass("DLTBGX");
                dltbClass = pFeaWs.OpenFeatureClass("DLTB");
            }
            catch { }
            if (dltbClass == null || dltbGX == null)
            {
                MessageBox.Show("找不到必备图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass bgTbClass = null;//变更图斑图层
            try
            {
                bgTbClass = pFeaWs.OpenFeatureClass(ClassName);
            }
            catch
            {
                MessageBox.Show("当前数据库中找不到该图层！");
                return;
            }
            UpdateStatus("开始进行数据准备");

            //数据库备份到dltbh内
            IFeatureClass pDLTBH = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBH");
            (pDLTBH as ITable).DeleteSearchedRows(null);
            bool append = RCIS.GISCommon.GpToolHelper.Append(currWs.PathName + "\\TDGX\\DLTBGX", currWs.PathName + "\\TDLS\\DLTBH");


            IWorkspace tmpWS = DeleteAndNewTmpGDB();
            IFeatureWorkspace tmpFWS = tmpWS as IFeatureWorkspace;
            //查找所有相交的三调图斑，相交即可
            UpdateStatus("开始查找所有参与变更的图斑数据");

            bool spatialJoin = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(currWs.PathName + "\\TDDC\\DLTB", currWs.PathName + "\\TDGX\\DLTBGX", tmpWS.PathName + "\\SDDLTB");
            if (!spatialJoin)
            {
                UpdateStatus("查找三调图斑失败");
                return;
            }
            tmpWS.ExecuteSQL("delete from SDDLTB where Join_Count=0");
            IFeatureClass pSDClass = tmpFWS.OpenFeatureClass("SDDLTB");
            for (int i = pSDClass.Fields.FieldCount - 1; i >= 0; i--)
            {
                IField pField = pSDClass.Fields.get_Field(i);
                if (pField.Name.Contains("_1"))
                    (pSDClass as ITable).DeleteField(pField);
            }

            //IGeometry bgTbGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(bgTbClass);
            //IPolygon pPolygon = bgTbGeo as IPolygon;
            //pPolygon.SimplifyPreserveFromTo();

            //IQueryFilter pQueryFilter = new QueryFilterClass();
            //pQueryFilter.WhereClause = "1 = 0";
            //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "DLTB", "SDDLTB", pQueryFilter);
            //IFeatureClass pSDClass = tmpFWS.OpenFeatureClass("SDDLTB");
            //using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            //{
            //    IFeatureCursor pInsertCursor = pSDClass.Insert(true);
            //    ISpatialFilter pSpaFilter = new SpatialFilterClass();
            //    pSpaFilter.Geometry = pPolygon;
            //    pSpaFilter.GeometryField = dltbClass.ShapeFieldName;
            //    pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //    prgInfo.Properties.Maximum = bgTbClass.FeatureCount(null);
            //    prgInfo.Visible = true;
            //    Application.DoEvents();
            //    int iCount = 1;
            //    IFeatureCursor pFeatureCursor = dltbClass.Search(pSpaFilter, true);
            //    comRel.ManageLifetime(pFeatureCursor);
            //    IFeature pFeature;
            //    while ((pFeature = pFeatureCursor.NextFeature()) != null)
            //    {
            //        prgInfo.Position = iCount++;
            //        IFeatureBuffer pFeatureBuffer = pSDClass.CreateFeatureBuffer();
            //        pFeatureBuffer.Shape = pFeature.ShapeCopy;
            //        for (int i = 0; i < pSDClass.Fields.FieldCount; i++)
            //        {
            //            IField pField = pFeatureBuffer.Fields.Field[i];
            //            if (pField.Type != esriFieldType.esriFieldTypeOID && pField.Type != esriFieldType.esriFieldTypeGeometry && !pField.Name.ToLower().Contains("shape_") && pField.Name.ToUpper().Trim() != "SJNF")
            //            {
            //                string sFieldName = pField.Name;
            //                pFeatureBuffer.set_Value(pSDClass.FindField(sFieldName), pFeature.get_Value(dltbClass.FindField(sFieldName)));
            //            }
            //        }
            //        pInsertCursor.InsertFeature(pFeatureBuffer);
            //    }
            //    pInsertCursor.Flush();
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pInsertCursor);
            //    prgInfo.Visible = false;
            //}
            UpdateStatus("开始查找所有可以合并的图斑数据");
            bool a = RCIS.GISCommon.GpToolHelper.GP_TabulateIntersection(currWs.PathName + "\\TDGX\\DLTBGX", "OBJECTID", tmpWS.PathName + "\\SDDLTB", "BSM", tmpWS.PathName + "\\JJZB");
            ITable pTabulate = tmpFWS.OpenTable("JJZB");
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "AREA>0.01";
            IFeatureClass pCLass = pTabulate as IFeatureClass;
            List<string> arr = GetUniqueValuesByFeatureClass(pTabulate, pQF, "BSM");
            IFeatureCursor pCursor = pSDClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string BSM = pFea.get_Value(pFea.Fields.FindField("BSM")).ToString();
                if (!arr.Contains(BSM))
                {
                    pFea.set_Value(pFea.Fields.FindField("BZ"), "Del");
                }
                else
                    pFea.set_Value(pFea.Fields.FindField("BSM"), null);
                pCursor.UpdateFeature(pFea);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

            //使用GP工具更新功能生成临时更新层
            UpdateStatus("开始对变更图斑数据进行空间叠加分析");
            string updateFeatures = currWs.PathName;
            IFeatureDataset pFeatureDataset = bgTbClass.FeatureDataset;
            if (pFeatureDataset != null)
            {
                updateFeatures += @"\" + pFeatureDataset.Name;
            }
            updateFeatures += @"\" + (bgTbClass as IDataset).Name;
            //string outFeatures = Application.StartupPath + @"\tmp\tmp.gdb\DLTBGX";
            Boolean b = RCIS.GISCommon.GpToolHelper.Update(Application.StartupPath + @"\tmp\tmp.gdb\SDDLTB", updateFeatures, Application.StartupPath + @"\tmp\tmp.gdb\DLTBGXM");
            if (!b)
            {
                UpdateStatus("空间叠加分析失败");
                return;
            }
            b = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(Application.StartupPath + @"\tmp\tmp.gdb\DLTBGXM", Application.StartupPath + @"\tmp\tmp.gdb\DLTBGX");
            if (!b)
            {
                UpdateStatus("数据打散失败");
                return;
            }
            //导出1001（铁路用地）,1002（轨道交通用地）,1003（公路用地）,1004（城镇村道路用地）,1006（农村道路）,1101（河流水面）,1107（沟渠）这几个地类作为不合并
            //string[] disFields = { "YSDM", "DLBM", "DLMC", "QSXZ", "QSDWDM", "QSDWMC", "ZLDWDM", "ZLDWMC", "KCDLBM", "KCXS", "GDLX", "GDPDJB", "XZDWKD", "TBXHDM", "TBXHMC", "ZZSXDM", "ZZSXMC", "GDDB", "FRDBS", "CZCSXM", "MSSM", "HDMC" };//, "SJNF"

            UpdateStatus("合并同地类图斑");
            string[] GDSX = { "YSDM", "DLBM", "DLMC", "QSXZ", "QSDWDM", "QSDWMC", "ZLDWDM", "ZLDWMC", "KCDLBM", "KCXS", "GDLX", "GDPDJB", "TBXHDM", "TBXHMC", "ZZSXDM", "ZZSXMC", "GDDB", "FRDBS", "CZCSXM", "MSSM", "HDMC" };//, "SJNF"
            string[] FGDSX = { "YSDM", "DLBM", "DLMC", "QSXZ", "QSDWDM", "QSDWMC", "ZLDWDM", "ZLDWMC", "TBXHDM", "TBXHMC", "FRDBS", "CZCSXM", "MSSM", "HDMC", "ZZSXDM", "ZZSXMC" };//, "SJNF"
            string[] dlJT = { "1001", "1002", "1003", "1004", "1006", "1009", "1101", "1107", "1107A" };
            IFeatureClass pDLTBGXClass = tmpFWS.OpenFeatureClass("DLTBGX");

            //tmpWS.ExecuteSQL("update DLTBGX set ZZSXDM='',ZZSXMC='' where ZZSXDM is null");

            //IField pField1 = new Field();
            //IFieldEdit pFieldEdit = pField1 as IFieldEdit;
            ////' 设置字段属性
            //pFieldEdit.Name_2 = "GXSJ";
            //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;//字段类型
            //pFieldEdit.AliasName_2 = "更新时间";
            //pFieldEdit.Editable_2 = true;
            //IClass pClass = pDLTBGXClass as IClass;
            //pClass.AddField(pField1);

            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {

                prgInfo.Properties.Maximum = pDLTBGXClass.FeatureCount(null);
                prgInfo.Visible = true;
                int iCount = 1;
                IFeatureCursor pBHCursor = pDLTBGXClass.Update(null, true);
                comRel.ManageLifetime(pBHCursor);
                IFeature pBH;
                while ((pBH = pBHCursor.NextFeature()) != null)
                {
                    string dlbm = pBH.get_Value(pDLTBGXClass.FindField("DLBM")).ToString();
                    string bz = pBH.get_Value(pDLTBGXClass.FindField("BZ")).ToString();
                    prgInfo.Position = iCount++;
                    if (dlbm == "Del") continue;
                    pBH.set_Value(pBH.Fields.FindField("GXSJ"), "" + dateEdit1.Text + "/12/31");
                    pBHCursor.UpdateFeature(pBH);
                    if (dlJT.Contains(dlbm)) continue;

                    int oid = pBH.OID;

                    string[] disFields = null;
                    if (dlbm.Substring(0, 2) == "01")
                        disFields = GDSX;
                    else
                        disFields = FGDSX;

                    mergeSameDLTB(pBH, pDLTBGXClass, disFields);

                    RCIS.Utility.OtherHelper.ReleaseComObject(pBH);
                }
                pBHCursor.Flush();
                prgInfo.Visible = false;
            }
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "DLBM = 'Del' or BZ='Del'";
            (pDLTBGXClass as ITable).DeleteSearchedRows(pQueryFilter);
            //导入合并后的数据和之前的导出数据，需要判断与变更图斑有重叠的部分
            UpdateStatus("正在导入数据");
            //(dltbGX as ITable).DeleteSearchedRows(null);

            //b = RCIS.GISCommon.GpToolHelper.Update(tmpWS.PathName + "\\DLTBGX", tmpWS.PathName + "\\XZDW",tmpWS.PathName+"\\gxData");

            bool del = RCIS.GISCommon.GpToolHelper.DeleteFeatures(currWs.PathName + "\\TDGX\\DLTBGX");
            if (!del)
            {
                MessageBox.Show("数据删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //RCIS.GISCommon.GpToolHelper.
            b = RCIS.GISCommon.GpToolHelper.Append(tmpWS.PathName + "\\DLTBGX", currWs.PathName + "\\TDGX\\DLTBGX");
            b = RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGX");

            UpdateStatus("处理完成");
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch(Exception ex)
            //{
            //    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex,ex.ToString());
            //    return;
            //}
        }


        private void mergeSameDLTB(IFeature pBH, IFeatureClass pDLTBGXClass, string[] disFields) 
        {
            bool merge = false;
            ITopologicalOperator pTop = pBH.ShapeCopy as ITopologicalOperator;
            //
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = pBH.ShapeCopy;
            pSF.GeometryField = pDLTBGXClass.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.WhereClause = "DLBM<>'Del'";
            IFeatureCursor pCur = pDLTBGXClass.Search(pSF, true);
            IFeature pDLTB;
            while ((pDLTB = pCur.NextFeature()) != null)
            {

                if (pDLTB.OID == pBH.OID) continue;
                bool b = false;
                foreach (string item in disFields)
                {
                    int iFieldIndex = pDLTBGXClass.FindField(item);
                    if (iFieldIndex != -1)
                    {
                        IField pField = pDLTBGXClass.Fields.Field[iFieldIndex];
                        if (pBH.get_Value(pDLTBGXClass.FindField(item)).ToString().ToUpper().Trim() != pDLTB.get_Value(pDLTBGXClass.FindField(item)).ToString().ToUpper().Trim())
                        {
                            b = true;
                            break;
                        }
                    }
                }
                if (b) continue;
                IGeometry pGeo = pTop.Intersect(pDLTB.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                if (pGeo != null)
                {
                    IPolyline pLine = pGeo as IPolyline;
                    if (pLine.Length > 0.003)
                    {
                        IPolygon pPoly = UnionPolygon(pBH.ShapeCopy, pDLTB.ShapeCopy);
                        pBH.Shape = pPoly;
                        if (pBH.get_Value(pDLTBGXClass.FindField("BZ")).ToString() == "Del")
                            pBH.set_Value(pBH.Fields.FindField("BZ"), "");
                        pBH.set_Value(pBH.Fields.FindField("BSM"),null);
                        pBH.Store();
                        pDLTB.set_Value(pDLTBGXClass.FindField("DLBM"), "Del");
                        pDLTB.Store();
                        merge = true;
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pGeo);
                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTB);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCur);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTop);
            if(merge)
                mergeSameDLTB(pBH, pDLTBGXClass, disFields);
        }

        private List<string> GetUniqueValuesByFeatureClass(ITable pTable,IQueryFilter pQF, string FieldName)
        {
            List<string> arrValues = new List<string>();
            DataStatisticsClass pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pTable.Search(pQF, false) as ICursor;
            pDataStatistics.Field = FieldName;
            IEnumerator pEnum = pDataStatistics.UniqueValues;
            while (pEnum.MoveNext())
            {
                string temp = pEnum.Current.ToString();
                arrValues.Add(temp);
            }
            return arrValues;
        }

        private bool GetIntersectArea(IGeometry pGeometry, IFeature pFea)
        {
            bool b = false;
            //IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            if (pGeoIntersect != null)
            {
                IArea pArea = pGeoIntersect as IArea;
                if (pArea.Area > 0.001)
                {
                    b = true;
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pArea);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pGeoIntersect);
            //RCIS.Utility.OtherHelper.ReleaseComObject(pGeometry);
            //RCIS.Utility.OtherHelper.ReleaseComObject(pTop);
            return b;
        }

        private bool GetIntersectLen(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
            if (pGeoIntersect != null)
            {
                IPolyline pLine = pGeoIntersect as IPolyline;
                if (pLine.Length > 0.001)
                {
                    b = true;
                }
            }
            return b;
        }

        public static IPolygon UnionPolygon(IGeometry pGeo1,IGeometry pGeo2)
        {
            object missing = Type.Missing;
            IGeometry geometryBag = new GeometryBagClass();
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            geometryCollection.AddGeometry(pGeo1, ref missing, ref missing);
            geometryCollection.AddGeometry(pGeo2, ref missing, ref missing);
            ITopologicalOperator union = new PolygonClass();
            union.ConstructUnion(geometryBag as IEnumGeometry);
            IPolygon retPolygon = union as IPolygon;
            return retPolygon;
        }
        
    }
}
