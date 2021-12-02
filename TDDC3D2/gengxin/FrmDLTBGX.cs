using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.gengxin
{
    public partial class FrmDLTBGX : Form
    {
        public IWorkspace currWs = null;
        private long maxBSM;

        public FrmDLTBGX()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void btnDLTBGX_Click(object sender, EventArgs e)
        {
            //根据变更后标识码相同的数据进行合并
            if (string.IsNullOrWhiteSpace(txtDLTBBH.Text))
            {
                MessageBox.Show("请选择变化图斑的数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("变化图斑数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("原图斑更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            UpdateStatus("正在汇总变更后面积");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            string filePath = currWs.PathName;
            ESRI.ArcGIS.AnalysisTools.Statistics pStatistics = new ESRI.ArcGIS.AnalysisTools.Statistics();
            pStatistics.in_table = filePath + @"\TDGX\DLTBGXGC";
            pStatistics.out_table = filePath + @"\DLTBGXTMP";
            pStatistics.case_field = "BGHTBBSM";
            pStatistics.statistics_fields = "BGHKCMJ SUM;BGHTBDLMJ SUM";
            try
            {
                gp.Execute(pStatistics, null);
            }
            catch
            {
                UpdateStatus("汇总错误");
                return;
            }

            UpdateStatus("正在生成图斑更新层数据");
            List<string> bsms = new List<string>();
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            ITable pTable = pFeatureWorkspace.OpenTable("DLTBGXTMP");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pTable, "BGHTBBSM");
            IFeatureClass pGXGCClass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            IFeatureClass pGXClass = pFeatureWorkspace.OpenFeatureClass("DLTBGX");
            (pGXClass as ITable).DeleteSearchedRows(null);
            IFeatureClass pDLTBClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBClass, "BSM");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pGXClass.Insert(true);
                comRel.ManageLifetime(pInsert);
                using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pBHCursor = pBHClass.Search(null, true);
                    comRel2.ManageLifetime(pBHCursor);
                    IFeature pBH;
                    while ((pBH = pBHCursor.NextFeature()) != null)
                    {
                        string bsm = pBH.get_Value(pBHClass.FindField("BSM")).ToString();
                        bsms.Add(bsm);
                        IFeatureBuffer pFeatureBuffer = pGXClass.CreateFeatureBuffer();
                        for (int i = 0; i < pFeatureBuffer.Fields.FieldCount; i++)
                        {
                            IField pField = pFeatureBuffer.Fields.Field[i];
                            int iFieldIndex = pBHClass.FindField(pField.Name);
                            if (pField.Type != esriFieldType.esriFieldTypeOID && iFieldIndex != -1 && pField.Name != "SHAPE_Area" && pField.Name != "SHAPE_Length")
                            {
                                pFeatureBuffer.set_Value(i, pBH.get_Value(iFieldIndex));
                            }
                        }
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel21 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IQueryFilter pQueryFilter = new QueryFilterClass();
                            pQueryFilter.WhereClause = "BGHTBBSM = '" + bsm + "'";
                            ICursor pCursor = pTable.Search(pQueryFilter, true);
                            comRel21.ManageLifetime(pCursor);
                            IRow pRow = pCursor.NextRow();
                            if (pRow == null) continue;
                            pFeatureBuffer.set_Value(pGXClass.FindField("TBMJ"), (double)pRow.get_Value(pTable.FindField("SUM_BGHKCMJ")) + (double)pRow.get_Value(pTable.FindField("SUM_BGHTBDLMJ")));
                            pFeatureBuffer.set_Value(pGXClass.FindField("KCMJ"), pRow.get_Value(pTable.FindField("SUM_BGHKCMJ")));
                            pFeatureBuffer.set_Value(pGXClass.FindField("TBDLMJ"), pRow.get_Value(pTable.FindField("SUM_BGHTBDLMJ")));
                        }
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                }
                using (ESRI.ArcGIS.ADF.ComReleaser comRel3 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pGXGCCursor = pGXGCClass.Update(null, true);
                    comRel3.ManageLifetime(pGXGCCursor);
                    IFeature pGXGC;
                    while ((pGXGC = pGXGCCursor.NextFeature()) != null)
                    {
                        string bsm = pGXGC.get_Value(pGXGCClass.FindField("BGHTBBSM")).ToString();
                        if (bsms.Contains(bsm)) continue;
                        long newbsm = maxBSM++;
                        pGXGC.set_Value(pGXGCClass.FindField("BGHTBBSM"), newbsm);
                        pGXGCCursor.UpdateFeature(pGXGC);
                        IFeatureBuffer pFeatureBuffer = pGXClass.CreateFeatureBuffer();
                        pFeatureBuffer.Shape = pGXGC.ShapeCopy;
                        pFeatureBuffer.set_Value(pGXClass.FindField("BSM"), newbsm);
                        pFeatureBuffer.set_Value(pGXClass.FindField("DLBM"), pGXGC.get_Value(pGXGCClass.FindField("BGQDLBM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("DLMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQDLMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("QSXZ"), pGXGC.get_Value(pGXGCClass.FindField("BGQQSXZ")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("QSDWDM"), pGXGC.get_Value(pGXGCClass.FindField("BGQQSDWDM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("QSDWMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQQSDWMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("ZLDWDM"), pGXGC.get_Value(pGXGCClass.FindField("BGQZLDWDM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("ZLDWMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQZLDWMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("KCDLBM"), pGXGC.get_Value(pGXGCClass.FindField("BGQKCDLBM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("KCXS"), pGXGC.get_Value(pGXGCClass.FindField("BGQKCXS")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("KCMJ"), pGXGC.get_Value(pGXGCClass.FindField("BGQKCMJ")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("TBDLMJ"), pGXGC.get_Value(pGXGCClass.FindField("BGQTBDLMJ")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("GDLX"), pGXGC.get_Value(pGXGCClass.FindField("BGQGDLX")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("GDPDJB"), pGXGC.get_Value(pGXGCClass.FindField("BGQGDPDJB")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("XZDWKD"), pGXGC.get_Value(pGXGCClass.FindField("BGQXZDWKD")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("TBXHDM"), pGXGC.get_Value(pGXGCClass.FindField("BGQTBXHDM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("TBXHMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQTBXHMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("ZZSXDM"), pGXGC.get_Value(pGXGCClass.FindField("BGQZZSXDM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("ZZSXMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQZZSXMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("GDDB"), pGXGC.get_Value(pGXGCClass.FindField("BGQGDDB")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("FRDBS"), pGXGC.get_Value(pGXGCClass.FindField("BGQFRDBS")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("CZCSXM"), pGXGC.get_Value(pGXGCClass.FindField("BGQCZCSXM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("MSSM"), pGXGC.get_Value(pGXGCClass.FindField("BGQMSSM")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("HDMC"), pGXGC.get_Value(pGXGCClass.FindField("BGQHDMC")));
                        pFeatureBuffer.set_Value(pGXClass.FindField("TBMJ"), (double)pGXGC.get_Value(pGXGCClass.FindField("BGQKCMJ")) + (double)pGXGC.get_Value(pGXGCClass.FindField("BGQTBDLMJ")));
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel31 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IQueryFilter pQueryFilter2 = new QueryFilterClass();
                            pQueryFilter2.WhereClause = "BSM = '" + pGXGC.get_Value(pGXGCClass.FindField("BGQTBBSM")).ToString() + "'";
                            IFeatureCursor pFeatureCursor = pDLTBClass.Search(pQueryFilter2, true);
                            comRel3.ManageLifetime(pFeatureCursor);
                            IFeature pDLTB = pFeatureCursor.NextFeature();
                            pFeatureBuffer.set_Value(pGXClass.FindField("YSDM"), pDLTB.get_Value(pDLTBClass.FindField("YSDM")));
                            pFeatureBuffer.set_Value(pGXClass.FindField("TBYBH"), pDLTB.get_Value(pDLTBClass.FindField("TBYBH")));
                            pFeatureBuffer.set_Value(pGXClass.FindField("TBBH"), pDLTB.get_Value(pDLTBClass.FindField("TBBH")));
                        }
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                    pGXGCCursor.Flush();
                }
                pInsert.Flush();
            }
            (pTable as IDataset).Delete();
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pBHClass);
            UpdateStatus("生成完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMergeSameDLBM_Click(object sender, EventArgs e)
        {
            //将地类图斑更新层中，每个村级调查区中相邻的同地类进行合并
            IFeatureClass pDLTBGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBGXClass, "BSM");
            IFeatureLayer pFLayer = new FeatureLayerClass();
            pFLayer.FeatureClass = pDLTBGXClass;
            IIdentify pGX = pFLayer as IIdentify;
            IFeatureClass pDLTBGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            string[] aryField = { "YSDM", "DLBM", "DLMC", "QSXZ", "QSDWDM", "QSDWMC", "ZLDWDM", "ZLDWMC", "KCDLBM", "KCXS", "GDLX", "GDPDJB", "XZDWKD", "TBXHDM", "TBXHMC", "ZZSXDM", "ZZSXMC", "GDDB", "FRDBS", "CZCSXM", "MSSM", "HDMC" };//, "SJNF"
            UpdateStatus("正在合并更新图层中的同地类图斑");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                IQueryFilterDefinition pQueryFilterDef = pQueryFilter as IQueryFilterDefinition;
                pQueryFilterDef.PostfixClause = "Order By BSM DESC";
                IFeatureCursor pBHCursor = pDLTBGXClass.Update(pQueryFilter, true);
                comRel.ManageLifetime(pBHCursor);
                IFeature pBH;
                while ((pBH = pBHCursor.NextFeature()) != null)
                {
                    string bsm = pBH.get_Value(pDLTBGXClass.FindField("BSM")).ToString();
                    Console.WriteLine(bsm);
                    if (string.IsNullOrWhiteSpace(bsm) || bsm == "DEL") continue;
                    ITopologicalOperator pTopo = pBH.ShapeCopy as ITopologicalOperator;
                    //ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    //List<string> lstWhere = new List<string>();
                    //foreach (string item in aryField)
                    //{
                    //    int iFieldIndex = pDLTBGXClass.FindField(item);
                    //    if (iFieldIndex != -1)
                    //    {
                    //        IField pField = pDLTBGXClass.Fields.Field[iFieldIndex];
                    //        if (pField.Type == esriFieldType.esriFieldTypeString)
                    //        {
                    //            lstWhere.Add(item + " = '" + pBH.get_Value(iFieldIndex).ToString().Trim() + "'");
                    //        }
                    //        else
                    //        {
                    //            lstWhere.Add(item + " = " + pBH.get_Value(iFieldIndex).ToString());
                    //        }
                    //    }
                    //}
                    //lstWhere.Add("BSM <> '" + bsm + "'");
                    //lstWhere.Add("BSM <> 'DEL'");
                    //pSpatialFilter.Geometry = pBH.ShapeCopy;
                    //pSpatialFilter.GeometryField = pDLTBGXClass.ShapeFieldName;
                    //pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    //pSpatialFilter.WhereClause = string.Join(" And ", lstWhere.ToArray());
                    //using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                    IArray id_result = pGX.Identify(pBH.ShapeCopy);
                    if (id_result != null)
                    {
                        //IFeatureCursor pBHCursor2 = pDLTBGXClass.Update(pSpatialFilter, true);
                        //comRel2.ManageLifetime(pBHCursor2);
                        //IFeature pDLTB;
                        //while ((pDLTB = pBHCursor2.NextFeature()) != null)
                        for (int i = 0; i < id_result.Count; i++)
                        {
                            //获取识别结果记录中的属性记录
                            IIdentifyObj featureIdentifyobj = (IIdentifyObj)id_result.get_Element(i);
                            IRowIdentifyObject iRowIdentifyObject = featureIdentifyobj as IRowIdentifyObject;
                            IRow pRow = iRowIdentifyObject.Row;//添加引用GeoDatabase
                            IFeature pDLTB = pRow as IFeature;
                            Boolean b = false;
                            if (pDLTB.get_Value(pDLTBGXClass.FindField("BSM")).ToString().ToUpper() == bsm || pDLTB.get_Value(pDLTBGXClass.FindField("BSM")).ToString().ToUpper() == "DEL") continue;
                            foreach (string item in aryField)
                            {
                                int iFieldIndex = pDLTBGXClass.FindField(item);
                                if (iFieldIndex != -1)
                                {
                                    IField pField = pDLTBGXClass.Fields.Field[iFieldIndex];
                                    if (pBH.get_Value(pDLTBGXClass.FindField(item)).ToString().ToUpper() != pDLTB.get_Value(pDLTBGXClass.FindField(item)).ToString().ToUpper())
                                    {
                                        b = true;
                                        break;
                                    }
                                }
                            }
                            if (b) continue;
                            IGeometry pGeo = pTopo.Intersect(pDLTB.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                            if (pGeo != null)
                            {
                                try
                                {
                                    double mj = (double)pDLTB.get_Value(pDLTBGXClass.FindField("TBMJ"));
                                }
                                catch { continue; }
                                //if (pBH.get_Value(pDLTBGXClass.FindField("BSM")).ToString() == bsm || pBH.get_Value(pDLTBGXClass.FindField("BSM")).ToString() == "DEL") continue;
                                long newbsm = this.maxBSM++;
                                pGeo = pTopo.Union(pDLTB.ShapeCopy);
                                pBH.Shape = pGeo;
                                if (pBH.get_Value(pDLTBGXClass.FindField("BSM")).ToString() != "DEL") pBH.set_Value(pDLTBGXClass.FindField("BSM"), newbsm);
                                pBH.set_Value(pDLTBGXClass.FindField("TBMJ"), (double)pBH.get_Value(pDLTBGXClass.FindField("TBMJ")) + (double)pDLTB.get_Value(pDLTBGXClass.FindField("TBMJ")));
                                pBH.set_Value(pDLTBGXClass.FindField("KCMJ"), (double)pBH.get_Value(pDLTBGXClass.FindField("KCMJ")) + (double)pDLTB.get_Value(pDLTBGXClass.FindField("KCMJ")));
                                pBH.set_Value(pDLTBGXClass.FindField("TBDLMJ"), (double)pBH.get_Value(pDLTBGXClass.FindField("TBDLMJ")) + (double)pDLTB.get_Value(pDLTBGXClass.FindField("TBDLMJ")));
                                pBHCursor.UpdateFeature(pBH);
                                pDLTB.Delete();
                                //pDLTB.set_Value(pDLTBGXClass.FindField("BSM"), "DEL");
                                //pBHCursor2.UpdateFeature(pDLTB);
                                using (ESRI.ArcGIS.ADF.ComReleaser comRele = new ESRI.ArcGIS.ADF.ComReleaser())
                                {
                                    ISpatialFilter pSpaFilter = new SpatialFilterClass();
                                    pSpaFilter.Geometry = pGeo;
                                    pSpaFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                                    pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                                    IFeatureCursor pUpdate = pDLTBGXGCClass.Update(pSpaFilter, true);
                                    comRele.ManageLifetime(pUpdate);
                                    IFeature pF;
                                    while ((pF = pUpdate.NextFeature()) != null)
                                    {
                                        pF.set_Value(pDLTBGXGCClass.FindField("BGHTBBSM"), newbsm);
                                        pF.set_Value(pDLTBGXGCClass.FindField("BGXW"), "2");
                                        pUpdate.UpdateFeature(pF);
                                    }
                                    pUpdate.Flush();
                                }
                            }
                        }
                        //pBHCursor2.Flush();
                    }
                }
                pBHCursor.Flush();
                //pQueryFilter = new QueryFilterClass();
                //pQueryFilter.WhereClause = "BSM = 'DEL'";
                //(pDLTBGXClass as ITable).DeleteSearchedRows(pQueryFilter);
            }
            UpdateStatus("正在合并三调库中的同地类图斑");
            IFeatureClass pDLTBClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            pFeatureLayer.FeatureClass = pDLTBClass;
            IIdentify pFL = pFeatureLayer as IIdentify;
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBClass, "BSM");
            ArrayList bsms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pDLTBGXGCClass, null, "BGQTBBSM");
            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTBGXGCClass, "BSM");
            if (maxBSM == 0)
            {
                maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTBClass, "BSM");
            }
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                IQueryFilterDefinition pQueryFilterDef = pQueryFilter as IQueryFilterDefinition;
                pQueryFilterDef.PostfixClause = "Order By BSM DESC";
                IFeatureCursor pBHCursor = pDLTBGXClass.Update(pQueryFilter, true);
                comRel.ManageLifetime(pBHCursor);
                IFeatureCursor pDLTBGXGCCursor = pDLTBGXGCClass.Insert(true);
                comRel.ManageLifetime(pDLTBGXGCCursor);
                IFeature pBH;
                while ((pBH = pBHCursor.NextFeature()) != null)
                {
                    string bsm = pBH.get_Value(pDLTBGXClass.FindField("BSM")).ToString();
                    if (string.IsNullOrWhiteSpace(bsm)) continue;
                    Console.WriteLine(bsm);
                    ITopologicalOperator pTopo = pBH.ShapeCopy as ITopologicalOperator;
                    //ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    //List<string> lstWhere = new List<string>();
                    //foreach (string item in aryField)
                    //{
                    //    int iFieldIndex = pDLTBGXClass.FindField(item);
                    //    if (iFieldIndex != -1)
                    //    {
                    //        IField pField = pDLTBGXClass.Fields.Field[iFieldIndex];
                    //        if (pField.Type == esriFieldType.esriFieldTypeString)
                    //        {
                    //            lstWhere.Add(item + " = '" + pBH.get_Value(iFieldIndex).ToString() + "'");
                    //        }
                    //        else
                    //        {
                    //            lstWhere.Add(item + " = " + pBH.get_Value(iFieldIndex).ToString());
                    //        }
                    //    }
                    //}
                    //lstWhere.Add("BSM <> '" + bsm + "'");
                    //lstWhere.Add("BSM <> 'DEL'");
                    //pSpatialFilter.Geometry = pBH.ShapeCopy;
                    //pSpatialFilter.GeometryField = pDLTBClass.ShapeFieldName;
                    //pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    //pSpatialFilter.WhereClause = string.Join(" And ", lstWhere.ToArray());
                    //using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                    //{
                        //IFeatureCursor pBHCursor2 = pDLTBClass.Search(pSpatialFilter, true);
                        //comRel2.ManageLifetime(pBHCursor2);
                    IArray id_result = pFL.Identify(pBH.ShapeCopy);
                    if (id_result != null)
                    {
                        //IFeature pDLTB;
                        //while ((pDLTB = pBHCursor2.NextFeature()) != null)
                        for (int i = 0; i < id_result.Count; i++)
                        {
                            //获取识别结果记录中的属性记录
                            IIdentifyObj featureIdentifyobj = (IIdentifyObj)id_result.get_Element(i);
                            IRowIdentifyObject iRowIdentifyObject = featureIdentifyobj as IRowIdentifyObject;
                            IRow pRow = iRowIdentifyObject.Row;//添加引用GeoDatabase
                            if (bsms.Contains(pRow.get_Value(pDLTBClass.FindField("BSM")).ToString())) continue;
                            IFeature pDLTB = pRow as IFeature;
                            Boolean b = false;
                            foreach (string item in aryField)
                            {
                                int iFieldIndex = pDLTBClass.FindField(item);
                                if (iFieldIndex != -1)
                                {
                                    IField pField = pDLTBClass.Fields.Field[iFieldIndex];
                                    if (pBH.get_Value(pDLTBGXClass.FindField(item)).ToString().ToUpper() != pDLTB.get_Value(pDLTBClass.FindField(item)).ToString().ToUpper())
                                    {
                                        b = true;
                                        break;
                                    }
                                }
                            }
                            if (b) continue;
                            IGeometry pGeo = pTopo.Intersect(pDLTB.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                            if (pGeo != null)
                            {
                                long newbsm = this.maxBSM++;
                                pGeo = pTopo.Union(pDLTB.ShapeCopy);
                                pBH.Shape = pGeo;
                                pBH.set_Value(pDLTBGXClass.FindField("BSM"), newbsm);
                                pBH.set_Value(pDLTBGXClass.FindField("TBMJ"), (double)pBH.get_Value(pDLTBGXClass.FindField("TBMJ")) + (double)pDLTB.get_Value(pDLTBGXClass.FindField("TBMJ")));
                                pBH.set_Value(pDLTBGXClass.FindField("KCMJ"), (double)pBH.get_Value(pDLTBGXClass.FindField("KCMJ")) + (double)pDLTB.get_Value(pDLTBGXClass.FindField("KCMJ")));
                                pBH.set_Value(pDLTBGXClass.FindField("TBDLMJ"), (double)pBH.get_Value(pDLTBGXClass.FindField("TBDLMJ")) + (double)pDLTB.get_Value(pDLTBGXClass.FindField("TBDLMJ")));
                                pBHCursor.UpdateFeature(pBH);
                                using (ESRI.ArcGIS.ADF.ComReleaser comRele = new ESRI.ArcGIS.ADF.ComReleaser())
                                {
                                    ISpatialFilter pSpaFilter = new SpatialFilterClass();
                                    pSpaFilter.Geometry = pGeo;
                                    pSpaFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                                    pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                                    IFeatureCursor pUpdate = pDLTBGXGCClass.Update(pSpaFilter, true);
                                    comRele.ManageLifetime(pUpdate);
                                    IFeature pF;
                                    while ((pF = pUpdate.NextFeature()) != null)
                                    {
                                        pF.set_Value(pDLTBGXGCClass.FindField("BGHTBBSM"), newbsm);
                                        pF.set_Value(pDLTBGXGCClass.FindField("BGXW"), "2");
                                        pUpdate.UpdateFeature(pF);
                                    }
                                    pUpdate.Flush();
                                }
                                IFeatureBuffer pFeatureBuffer = pDLTBGXGCClass.CreateFeatureBuffer();
                                pFeatureBuffer.Shape = pDLTB.ShapeCopy;
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BSM"), maxBSM++);
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGXW"), "2");
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("TBBGMJ"), pDLTB.get_Value(pDLTBClass.FindField("TBMJ")));
                                long tbbh = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(pDLTBGXGCClass, "TBBH", "ZLDWDM = '" + pDLTB.get_Value(pDLTBClass.FindField("ZLDWDM")).ToString() + "'");
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGTBBH"), tbbh++);
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQTBBSM"), pDLTB.get_Value(pDLTBClass.FindField("BSM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQDLBM"), pDLTB.get_Value(pDLTBClass.FindField("DLBM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQDLMC"), pDLTB.get_Value(pDLTBClass.FindField("DLMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQQSXZ"), pDLTB.get_Value(pDLTBClass.FindField("QSXZ")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQQSDWDM"), pDLTB.get_Value(pDLTBClass.FindField("QSDWDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQQSDWMC"), pDLTB.get_Value(pDLTBClass.FindField("QSDWMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQZLDWDM"), pDLTB.get_Value(pDLTBClass.FindField("ZLDWDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQZLDWMC"), pDLTB.get_Value(pDLTBClass.FindField("ZLDWMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQKCDLBM"), pDLTB.get_Value(pDLTBClass.FindField("KCDLBM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQKCXS"), pDLTB.get_Value(pDLTBClass.FindField("KCXS")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQKCMJ"), pDLTB.get_Value(pDLTBClass.FindField("KCMJ")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQTBDLMJ"), pDLTB.get_Value(pDLTBClass.FindField("TBDLMJ")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQGDLX"), pDLTB.get_Value(pDLTBClass.FindField("GDLX")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQGDPDJB"), pDLTB.get_Value(pDLTBClass.FindField("GDPDJB")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQXZDWKD"), pDLTB.get_Value(pDLTBClass.FindField("XZDWKD")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQTBXHDM"), pDLTB.get_Value(pDLTBClass.FindField("TBXHDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQTBXHMC"), pDLTB.get_Value(pDLTBClass.FindField("TBXHMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQZZSXDM"), pDLTB.get_Value(pDLTBClass.FindField("ZZSXDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQZZSXMC"), pDLTB.get_Value(pDLTBClass.FindField("ZZSXMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQGDDB"), pDLTB.get_Value(pDLTBClass.FindField("GDDB")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQFRDBS"), pDLTB.get_Value(pDLTBClass.FindField("FRDBS")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQCZCSXM"), pDLTB.get_Value(pDLTBClass.FindField("CZCSXM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGQMSSM"), pDLTB.get_Value(pDLTBClass.FindField("MSSM")));

                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHTBBSM"), pBH.get_Value(pDLTBClass.FindField("BSM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHDLBM"), pBH.get_Value(pDLTBClass.FindField("DLBM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHDLMC"), pBH.get_Value(pDLTBClass.FindField("DLMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHQSXZ"), pBH.get_Value(pDLTBClass.FindField("QSXZ")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHQSDWDM"), pBH.get_Value(pDLTBClass.FindField("QSDWDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHQSDWMC"), pBH.get_Value(pDLTBClass.FindField("QSDWMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHZLDWDM"), pBH.get_Value(pDLTBClass.FindField("ZLDWDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHZLDWMC"), pBH.get_Value(pDLTBClass.FindField("ZLDWMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHKCDLBM"), pBH.get_Value(pDLTBClass.FindField("KCDLBM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHKCXS"), pBH.get_Value(pDLTBClass.FindField("KCXS")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHKCMJ"), pDLTB.get_Value(pDLTBClass.FindField("KCMJ")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHTBDLMJ"), pDLTB.get_Value(pDLTBClass.FindField("TBDLMJ")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHGDLX"), pBH.get_Value(pDLTBClass.FindField("GDLX")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHGDPDJB"), pBH.get_Value(pDLTBClass.FindField("GDPDJB")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHXZDWKD"), pBH.get_Value(pDLTBClass.FindField("XZDWKD")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHTBXHDM"), pBH.get_Value(pDLTBClass.FindField("TBXHDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHTBXHMC"), pBH.get_Value(pDLTBClass.FindField("TBXHMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHZZSXDM"), pBH.get_Value(pDLTBClass.FindField("ZZSXDM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHZZSXMC"), pBH.get_Value(pDLTBClass.FindField("ZZSXMC")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHGDDB"), pBH.get_Value(pDLTBClass.FindField("GDDB")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHFRDBS"), pBH.get_Value(pDLTBClass.FindField("FRDBS")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHCZCSXM"), pBH.get_Value(pDLTBClass.FindField("CZCSXM")));
                                pFeatureBuffer.set_Value(pDLTBGXGCClass.FindField("BGHMSSM"), pBH.get_Value(pDLTBClass.FindField("MSSM")));
                                pDLTBGXGCCursor.InsertFeature(pFeatureBuffer);
                            }
                        }
                    }
                }
                pBHCursor.Flush();
                pDLTBGXGCCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            UpdateStatus("合并完成");
            MessageBox.Show("合并完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtDLTBBH_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openDia = new OpenFileDialog();
            openDia.Filter = "SHP文件（*.shp）|*.shp";
            if (openDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDLTBBH.Text = openDia.FileName;
            }
        }

        private void FrmDLTBGX_Load(object sender, EventArgs e)
        {
            IFeatureClass pDLTBGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTBGXGCClass, "BSM"); maxBSM++;
        }
    }
}
