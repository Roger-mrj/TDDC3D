using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.gengxin
{
    public partial class FrmSummary : Form
    {
        public IWorkspace currWs = null;

        public FrmSummary()
        {
            InitializeComponent();
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

        private void btnSummary_Click(object sender, EventArgs e)
        {
            //数据汇总流程
            //1.计算变更后行政区，村级调查区的图形范围
            //2.计算变更后行政区，村级调查区中变更图斑的面积
            //3.计算变更后行政区，村级调查区中三调图斑面积并汇总
            UpdateStatus("开始计算村级调查区更新过程层面积");
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pCJDCQGXGCClass = pFeatureWorkspace.OpenFeatureClass("CJDCQGXGC");
            IFeatureClass pDLTBClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            IFeatureClass pDLTBGXGCClass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pCJDCQGXGCClass, null, "BGHZLDWDM");
            foreach (var item in dwdms)
            {
                string dwdm = item.ToString();
                double DCMJ = 0;
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHZLDWDM Like '" + dwdm + "%'";
                    IFeatureCursor pCJDCQGXGCCursor = pCJDCQGXGCClass.Update(pQueryFilter, true);
                    comRel.ManageLifetime(pCJDCQGXGCCursor);
                    List<string> bsms = new List<string>();
                    IFeature pCJDCQGXGC;
                    while ((pCJDCQGXGC = pCJDCQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGQZLDWDM")).ToString();
                        string bghdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGHZLDWDM")).ToString();
                        if (currDh == 0)
                        {
                            ESRI.ArcGIS.Geometry.IPoint selectPoint = (pCJDCQGXGC.ShapeCopy as IArea).Centroid;
                            double X = selectPoint.X;
                            currDh = (int)(X / 1000000);////WK---带号
                        }
                        
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pCJDCQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "BGQZLDWDM = '" + bgqdwdm + "' And BGHZLDWDM = '" + bghdwdm + "'";
                            IFeatureCursor pDLTBGXGCCursor = pDLTBGXGCClass.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pDLTBGXGCCursor);
                            IFeature pDLTBGXGC;
                            while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                            {
                                string bsm = pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("BGQTBBSM")).ToString();
                                bsms.Add(bsm);
                                double TBMJ = double.Parse(pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("BGMJ")).ToString());
                                BGMJ += TBMJ;
                                DCMJ += TBMJ;
                            }
                        }
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pCJDCQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "ZLDWDM = '" + bgqdwdm + "'";
                            IFeatureCursor pDLTBCursor = pDLTBClass.Search(pSpatialFilter, true);
                            IFeature pDLTB;
                            while ((pDLTB = pDLTBCursor.NextFeature()) != null)
                            {
                                string bsm = pDLTB.get_Value(pDLTBClass.FindField("BSM")).ToString();
                                if (bsms.Contains(bsm)) continue;
                                double TBMJ = double.Parse(pDLTB.get_Value(pDLTBClass.FindField("TBMJ")).ToString());
                                BGMJ += TBMJ;
                                DCMJ += TBMJ;
                            }
                        }
                        double JSMJ = area.SphereArea(pCJDCQGXGC.ShapeCopy, currDh);
                        pCJDCQGXGC.set_Value(pCJDCQGXGCClass.FindField("BGHJSMJ"), Math.Round(JSMJ, 2));
                        pCJDCQGXGC.set_Value(pCJDCQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pCJDCQGXGCCursor.UpdateFeature(pCJDCQGXGC);
                    }
                    pCJDCQGXGCCursor.Flush();
                }
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pCJDCQGXGCClass as ITable, "BGHDCMJ", Math.Round(DCMJ, 2), "BGHZLDWDM = '" + dwdm + "'");
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCClass);
            
            UpdateStatus("开始计算村级调查区更新层面积");
            //ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            //gp.OverwriteOutput = true;
            //string filePath = currWs.PathName;
            //ESRI.ArcGIS.AnalysisTools.Statistics pStatistics = new ESRI.ArcGIS.AnalysisTools.Statistics();
            //pStatistics.in_table = filePath + @"\TDGX\CJDCQGXGC";
            //pStatistics.out_table = filePath + @"\CJDCQStatistics";
            //pStatistics.statistics_fields = "BGHDCMJ SUM";//;BGHJSMJ SUM";
            //pStatistics.case_field = "BGHZLDWDM";
            //try
            //{
            //    gp.Execute(pStatistics, null);
            //}
            //catch
            //{
            //    UpdateStatus("计算错误");
            //    return;
            //}
            //ITable pTable = pFeatureWorkspace.OpenTable("CJDCQStatistics");
            
            //using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            //{
            //    ICursor pCursor = pTable.Search(null, true);
            //    comRel.ManageLifetime(pCursor);
            //    IRow pRow;
            //    while ((pRow = pCursor.NextRow()) != null)
            //    {
            //        string dwdm = pRow.get_Value(pTable.FindField("BGHZLDWDM")).ToString();
            //        //double jsmj = double.Parse(pRow.get_Value(pTable.FindField("SUM_BGHJSMJ")).ToString());
            //        double dcmj = double.Parse(pRow.get_Value(pTable.FindField("SUM_BGHDCMJ")).ToString());
            //        RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pFeatureClass as ITable, "DCMJ", Math.Round(dcmj, 2), "ZLDWDM = '" + dwdm + "'");
            //    }
            //}
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass("CJDCQGX");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pFeatureCurosr = pFeatureClass.Update(null, true);
                comRel.ManageLifetime(pFeatureCurosr);
                IFeature pFeature;
                while ((pFeature = pFeatureCurosr.NextFeature()) != null)
                {
                    double jsmj = 0;
                    double dcmj = 0;
                    string bsm = pFeature.get_Value(pFeatureClass.FindField("BSM")).ToString();
                    using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                    {
                        IQueryFilter pQ = new QueryFilterClass();
                        pQ.WhereClause = "BGHBSM = '" + bsm + "'";
                        IFeatureCursor pSearch = pCJDCQGXGCClass.Search(pQ, true);
                        comRel2.ManageLifetime(pSearch);
                        IFeature pF;
                        while ((pF = pSearch.NextFeature()) != null)
                        {
                            if (dcmj == 0) dcmj = double.Parse(pF.get_Value(pCJDCQGXGCClass.FindField("BGHDCMJ")).ToString());
                            double mj = double.Parse(pF.get_Value(pCJDCQGXGCClass.FindField("BGHJSMJ")).ToString());
                            jsmj += mj;
                        }
                    }
                    pFeature.set_Value(pFeatureClass.FindField("DCMJ"), Math.Round(dcmj, 2));
                    pFeature.set_Value(pFeatureClass.FindField("JSMJ"), Math.Round(jsmj, 2));
                    pFeatureCurosr.UpdateFeature(pFeature);
                }
                pFeatureCurosr.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureClass);
            //(pTable as IDataset).Delete();
            UpdateStatus("计算完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSumXZQ_Click(object sender, EventArgs e)
        {
            //数据汇总流程
            //1.计算变更后行政区，行政区的图形范围
            //2.计算变更后行政区，行政区中变更图斑的面积
            //3.计算变更后行政区，行政区中三调图斑面积并汇总
            UpdateStatus("开始计算行政区更新过程层面积");
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQGXGCClass = pFeatureWorkspace.OpenFeatureClass("XZQGXGC");
            IFeatureClass pDLTBClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            IFeatureClass pDLTBGXGCClass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pXZQGXGCClass, null, "BGHXZQDM");
            foreach (var item in dwdms)
            {
                string dwdm = item.ToString();
                double DCMJ = 0;
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHXZQDM Like '" + dwdm + "%'";
                    IFeatureCursor pXZQGXGCCursor = pXZQGXGCClass.Update(pQueryFilter, true);
                    comRel.ManageLifetime(pXZQGXGCCursor);
                    List<string> bsms = new List<string>();
                    IFeature pXZQGXGC;
                    while ((pXZQGXGC = pXZQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGQXZQDM")).ToString();
                        string bghdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGHXZQDM")).ToString();
                        if (currDh == 0)
                        {
                            ESRI.ArcGIS.Geometry.IPoint selectPoint = (pXZQGXGC.ShapeCopy as IArea).Centroid;
                            double X = selectPoint.X;
                            currDh = (int)(X / 1000000);////WK---带号
                        }

                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pXZQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "BGQZLDWDM Like '" + bgqdwdm + "%' And BGHZLDWDM Like '" + bghdwdm + "%'";
                            IFeatureCursor pDLTBGXGCCursor = pDLTBGXGCClass.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pDLTBGXGCCursor);
                            IFeature pDLTBGXGC;
                            while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                            {
                                string bsm = pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("BGQTBBSM")).ToString();
                                bsms.Add(bsm);
                                double TBMJ = double.Parse(pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("BGMJ")).ToString());
                                BGMJ += TBMJ;
                                DCMJ += TBMJ;
                            }
                        }
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pXZQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "ZLDWDM Like '" + bgqdwdm + "%'";
                            IFeatureCursor pDLTBCursor = pDLTBClass.Search(pSpatialFilter, true);
                            IFeature pDLTB;
                            while ((pDLTB = pDLTBCursor.NextFeature()) != null)
                            {
                                string bsm = pDLTB.get_Value(pDLTBClass.FindField("BSM")).ToString();
                                if (bsms.Contains(bsm)) continue;
                                double TBMJ = double.Parse(pDLTB.get_Value(pDLTBClass.FindField("TBMJ")).ToString());
                                BGMJ += TBMJ;
                                DCMJ += TBMJ;
                            }
                        }
                        double JSMJ = area.SphereArea(pXZQGXGC.ShapeCopy, currDh);
                        pXZQGXGC.set_Value(pXZQGXGCClass.FindField("BGHJSMJ"), Math.Round(JSMJ, 2));
                        pXZQGXGC.set_Value(pXZQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pXZQGXGCCursor.UpdateFeature(pXZQGXGC);
                    }
                    pXZQGXGCCursor.Flush();
                }
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pXZQGXGCClass as ITable, "BGHDCMJ", Math.Round(DCMJ, 2), "BGHXZQDM = '" + dwdm + "'");
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCClass);

            UpdateStatus("开始计算行政区更新层面积");
            //ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            //gp.OverwriteOutput = true;
            //string filePath = currWs.PathName;
            //ESRI.ArcGIS.AnalysisTools.Statistics pStatistics = new ESRI.ArcGIS.AnalysisTools.Statistics();
            //pStatistics.in_table = filePath + @"\TDGX\XZQGXGC";
            //pStatistics.out_table = filePath + @"\XZQStatistics";
            //pStatistics.statistics_fields = "BGHDCMJ SUM";//;BGHJSMJ SUM";
            //pStatistics.case_field = "BGHXZQDM";
            //try
            //{
            //    gp.Execute(pStatistics, null);
            //}
            //catch
            //{
            //    UpdateStatus("计算错误");
            //    return;
            //}
            //ITable pTable = pFeatureWorkspace.OpenTable("XZQStatistics");
            //
            //using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            //{
            //    ICursor pCursor = pTable.Search(null, true);
            //    comRel.ManageLifetime(pCursor);
            //    IRow pRow;
            //    while ((pRow = pCursor.NextRow()) != null)
            //    {
            //        string dwdm = pRow.get_Value(pTable.FindField("BGHXZQDM")).ToString();
            //        //double jsmj = double.Parse(pRow.get_Value(pTable.FindField("SUM_BGHJSMJ")).ToString());
            //        double dcmj = double.Parse(pRow.get_Value(pTable.FindField("SUM_BGHDCMJ")).ToString());
            //        RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pFeatureClass as ITable, "DCMJ", Math.Round(dcmj, 2), "XZQDM = '" + dwdm + "'");
            //    }
            //}
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass("XZQGX");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pFeatureCurosr = pFeatureClass.Update(null, true);
                comRel.ManageLifetime(pFeatureCurosr);
                IFeature pFeature;
                while ((pFeature = pFeatureCurosr.NextFeature()) != null)
                {
                    double jsmj = 0;
                    double dcmj = 0;
                    string bsm = pFeature.get_Value(pFeatureClass.FindField("BSM")).ToString();
                    using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                    {
                        IQueryFilter pQ = new QueryFilterClass();
                        pQ.WhereClause = "BGHBSM = '" + bsm + "'";
                        IFeatureCursor pSearch = pXZQGXGCClass.Search(pQ, true);
                        comRel2.ManageLifetime(pSearch);
                        IFeature pF;
                        while ((pF = pSearch.NextFeature()) != null)
                        {
                            if (dcmj == 0) dcmj = double.Parse(pF.get_Value(pXZQGXGCClass.FindField("BGHDCMJ")).ToString());
                            double mj = double.Parse(pF.get_Value(pXZQGXGCClass.FindField("BGHJSMJ")).ToString());
                            jsmj += mj;
                        }
                    }
                    pFeature.set_Value(pFeatureClass.FindField("DCMJ"), Math.Round(dcmj, 2));
                    pFeature.set_Value(pFeatureClass.FindField("JSMJ"), Math.Round(jsmj, 2));
                    pFeatureCurosr.UpdateFeature(pFeature);
                }
                pFeatureCurosr.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureClass);
            //(pTable as IDataset).Delete();
            UpdateStatus("计算完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
