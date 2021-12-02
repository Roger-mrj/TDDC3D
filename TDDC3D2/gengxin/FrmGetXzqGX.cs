using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.gengxin
{
    public partial class FrmGetXzqGX : Form
    {
        public IWorkspace currWs = null;

        public FrmGetXzqGX()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToShortTimeString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
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


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //try 
            //{
                //currWs.ExecuteSQL("delete from dltbgx where zldwdm<>'1403221062010000000'");
                if (!cmbDltb.Text.ToUpper().StartsWith("DLTBGX") || !cmbCjdcq.Text.ToUpper().StartsWith("CJDCQGX") || !cmbXzq.Text.ToUpper().StartsWith("XZQGX"))
                {
                    MessageBox.Show("图层选择错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (currWs == null) return;
                UpdateStatus("正在准备数据……");

                LsGxClass gx = new LsGxClass();
                string  bgData = "" + dateEdit1.Text + "/12/31";
                gx.bgData = bgData;
                gx.info = info;
                gx.UpdateStatus("开始提取行政区更新层！");
                gx.getCJDCQGX();
                gx.getXZQGX();
                //getXzqGx();
                gx.UpdateStatus("行政区更新层提取完毕！");
                MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
                IWorkspace tmpWS = DeleteAndNewTmpGDB();
                //IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                //pEnumDataset.Reset();
                //IDataset pDataset;
                //while ((pDataset = pEnumDataset.Next()) != null)
                //{
                //    pDataset.Delete();
                //}
                //IQueryFilter pQueryFilter = new QueryFilterClass();
                //pQueryFilter.WhereClause = "1=0";
                //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "CJDCQ", "CJDCQSD", pQueryFilter);
                //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "CJDCQGX", "CJDCQBHtmp", pQueryFilter);
                //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "CJDCQGX", "CJDCQBH", pQueryFilter);
                //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "XZQ", "XZQSD", pQueryFilter);
                //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "XZQGX", "XZQBHtmp", pQueryFilter);
                //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, tmpWS, "XZQGX", "XZQBH", pQueryFilter);
                string sameDm = "";

                IFeatureClass pCJDCQClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
                IFeatureLayer pCJDCQLayer = new FeatureLayerClass();
                pCJDCQLayer.FeatureClass = pCJDCQClass;
                IIdentify pIdenCJDCQ = pCJDCQLayer as IIdentify;
                IFeatureClass pXZQClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
                IFeatureLayer pXZQLayer = new FeatureLayerClass();
                pXZQLayer.FeatureClass = pXZQClass;
                IIdentify pIdenXZQ = pXZQLayer as IIdentify;
                IFeatureWorkspace pFW = tmpWS as IFeatureWorkspace;
                pFW.CreateFeatureClass("CJDCQSD", pCJDCQClass.Fields, null, null, esriFeatureType.esriFTSimple, pCJDCQClass.ShapeFieldName, null);
                pFW.CreateFeatureClass("CJDCQBHtmp", pCJDCQClass.Fields, null, null, esriFeatureType.esriFTSimple, pCJDCQClass.ShapeFieldName, null);
                pFW.CreateFeatureClass("CJDCQBH", pCJDCQClass.Fields, null, null, esriFeatureType.esriFTSimple, pCJDCQClass.ShapeFieldName, null);
                pFW.CreateFeatureClass("XZQSD", pXZQClass.Fields, null, null, esriFeatureType.esriFTSimple, pXZQClass.ShapeFieldName, null);
                pFW.CreateFeatureClass("XZQBHtmp", pXZQClass.Fields, null, null, esriFeatureType.esriFTSimple, pXZQClass.ShapeFieldName, null);
                pFW.CreateFeatureClass("XZQBH", pXZQClass.Fields, null, null, esriFeatureType.esriFTSimple, pXZQClass.ShapeFieldName, null);
                UpdateStatus("正在查找变化的村级调查区……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pCJDCQBHtmp = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQBHtmp");
                    IFeatureCursor pInsertCJDCQ = pCJDCQBHtmp.Insert(true);
                    comRel.ManageLifetime(pInsertCJDCQ);
                    Dictionary<string, string> cjdcqdmmc = GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "DLTBGX", "ZLDWDM", "ZLDWMC",ref sameDm);
                    if (sameDm.Length > 0 && cjdcqdmmc == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (KeyValuePair<string, string> cjdcq in cjdcqdmmc)
                    {
                        IFeatureClass pSearchClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
                        string dwdm = cjdcq.Key;
                        string dwmc = cjdcq.Value;
                        string mssm = "00";
                        IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(pSearchClass, "ZLDWDM = '" + cjdcq.Key + "' And ZLDWMC = '" + cjdcq.Value + "'");
                        List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (pSearchClass as IGeoDataset).SpatialReference);
                        foreach (IGeometry item in pCJs)
                        {
                            ITopologicalOperator pTop = item as ITopologicalOperator;
                            IArray pArry = pIdenCJDCQ.Identify(item);
                            if (pArry != null)
                            {
                                for (int i = 0; i < pArry.Count; i++)
                                {
                                    IFeatureIdentifyObj idObj = pArry.get_Element(i) as IFeatureIdentifyObj;
                                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                                    IFeature pfea = pRow.Row as IFeature;
                                    string zldwdm = pfea.get_Value(pfea.Fields.FindField("ZLDWDM")).ToString();
                                    string zldwmc = pfea.get_Value(pfea.Fields.FindField("ZLDWMC")).ToString();
                                    mssm = pfea.get_Value(pfea.Fields.FindField("MSSM")).ToString();
                                    IGeometry pGeo = pTop.Intersect(pfea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                                    if (pGeo != null)
                                    {
                                        IArea pArea = pGeo as IArea;
                                        if (pArea.Area > 0.1 && (dwdm != zldwdm || dwmc != zldwmc))
                                        {
                                            IFeatureBuffer pFeatureBuffer = pCJDCQBHtmp.CreateFeatureBuffer();
                                            pFeatureBuffer.Shape = pGeo;
                                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWDM"), dwdm);
                                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWMC"), dwmc);
                                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), mssm);
                                            pInsertCJDCQ.InsertFeature(pFeatureBuffer);
                                        }
                                    }
                                }
                            }
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSearchClass);

                    }
                    pInsertCJDCQ.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQBHtmp);

                }
                UpdateStatus("正在处理变化的村级调查区……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pCJDCQBH = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQBH");
                    IFeatureCursor pInsertCJDCQ = pCJDCQBH.Insert(true);
                    comRel.ManageLifetime(pInsertCJDCQ);
                    Dictionary<string, string> cjdcqdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "CJDCQBHtmp", "ZLDWDM", "ZLDWMC",ref sameDm);
                    if (sameDm.Length > 0 && cjdcqdmmc == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                        return;
                    }
                    foreach (KeyValuePair<string, string> cjdcq in cjdcqdmmc)
                    {
                        IFeatureClass pSearchClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQBHtmp");
                        string dwdm = cjdcq.Key;
                        string dwmc = cjdcq.Value;
                        string mssm = "00";
                        IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(pSearchClass, "ZLDWDM = '" + cjdcq.Key + "' And ZLDWMC = '" + cjdcq.Value + "'");
                        List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (pSearchClass as IGeoDataset).SpatialReference);
                        foreach (IGeometry item in pCJs)
                        {
                            IFeatureBuffer pFeatureBuffer = pCJDCQBH.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = item;
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWDM"), dwdm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWMC"), dwmc);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), mssm);
                            pInsertCJDCQ.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSearchClass);

                    }
                    pInsertCJDCQ.Flush();

                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQBH);

                }
                UpdateStatus("正在查找变化的三调村级调查区……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pCJDCQSD = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQSD");
                    IFeatureCursor pInsertCJDCQ = pCJDCQSD.Insert(true);
                    IFeatureClass pCJDCQBH = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQBH");
                    IGeometry pBH = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCJDCQBH);
                    ITopologicalOperator pTop = pBH as ITopologicalOperator;
                    IArray pArry = pIdenCJDCQ.Identify(pBH);
                    if (pArry != null)
                    {
                        for (int i = 0; i < pArry.Count; i++)
                        {
                            IFeatureIdentifyObj idObj = pArry.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature pfea = pRow.Row as IFeature;
                            IGeometry pGeo = pTop.Intersect(pfea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                            if (pGeo != null)
                            {
                                IArea pArea = pGeo as IArea;
                                if (pArea.Area > 0.1)
                                {
                                    IFeatureBuffer pFeatureBuffer = pCJDCQSD.CreateFeatureBuffer();
                                    pFeatureBuffer.Shape = pfea.ShapeCopy;
                                    pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWDM"), pfea.get_Value(pfea.Fields.FindField("ZLDWDM")).ToString());
                                    pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWMC"), pfea.get_Value(pfea.Fields.FindField("ZLDWMC")).ToString());
                                    pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), pfea.get_Value(pfea.Fields.FindField("MSSM")).ToString());
                                    pInsertCJDCQ.InsertFeature(pFeatureBuffer);
                                    using (ESRI.ArcGIS.ADF.ComReleaser comrel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                                    {
                                        IQueryFilter pQF = new QueryFilterClass();
                                        pQF.WhereClause = "ZLDWDM = '" + pfea.get_Value(pfea.Fields.FindField("ZLDWDM")).ToString() +
                                                        "' And ZLDWMC = '" + pfea.get_Value(pfea.Fields.FindField("ZLDWMC")).ToString() +
                                                        "' And " + pCJDCQClass.OIDFieldName + " <> " + pfea.OID;
                                        IFeatureCursor pFC = pCJDCQClass.Search(pQF, true);
                                        comrel2.ManageLifetime(pFC);
                                        IFeature pF;
                                        while ((pF = pFC.NextFeature()) != null)
                                        {
                                            IFeatureBuffer pFeatureBuffer2 = pCJDCQSD.CreateFeatureBuffer();
                                            pFeatureBuffer2.Shape = pF.ShapeCopy;
                                            pFeatureBuffer2.set_Value(pFeatureBuffer2.Fields.FindField("ZLDWDM"), pF.get_Value(pF.Fields.FindField("ZLDWDM")).ToString());
                                            pFeatureBuffer2.set_Value(pFeatureBuffer2.Fields.FindField("ZLDWMC"), pF.get_Value(pF.Fields.FindField("ZLDWMC")).ToString());
                                            pFeatureBuffer2.set_Value(pFeatureBuffer2.Fields.FindField("MSSM"), pF.get_Value(pF.Fields.FindField("MSSM")).ToString());
                                            pInsertCJDCQ.InsertFeature(pFeatureBuffer2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    pInsertCJDCQ.Flush();

                    RCIS.Utility.OtherHelper.ReleaseComObject(pInsertCJDCQ);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQSD);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQBH);

                }
                RCIS.GISCommon.GpToolHelper.Update(tmpWS.PathName + @"\CJDCQSD", tmpWS.PathName + @"\CJDCQBH", tmpWS.PathName + @"\CJDCQGX");
                UpdateStatus("正在导入村级调查区更新数据……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pAll = pCJDCQClass.Search(null, false);
                    comRel.ManageLifetime(pAll);
                    IFeatureClass pCJDCQGX = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
                    IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCJDCQGX);
                    Dictionary<string, string> dmmcgx = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "CJDCQGX", "ZLDWDM", "ZLDWMC",ref sameDm);
                    if (sameDm.Length > 0 && dmmcgx == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    ITopologicalOperator pT = pGeo as ITopologicalOperator;
                    IFeatureCursor pInsert = pCJDCQGX.Insert(true);
                    IFeature pFeature;
                    while ((pFeature = pAll.NextFeature()) != null)
                    {
                        string xzqdm = pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")).ToString();
                        string xzqmc = pFeature.get_Value(pFeature.Fields.FindField("ZLDWMC")).ToString();
                        if (dmmcgx.ContainsKey(xzqdm))
                        {
                            if (xzqmc == dmmcgx[xzqdm])
                            {
                                IGeometry pG = pT.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                                if (pG != null)
                                {
                                    IArea pArea = pG as IArea;
                                    if (pArea.Area < 0.05)
                                    {
                                        IFeatureBuffer pB = pCJDCQGX.CreateFeatureBuffer();
                                        pB.Shape = pFeature.ShapeCopy;
                                        pB.set_Value(pB.Fields.FindField("ZLDWDM"), pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")));
                                        pB.set_Value(pB.Fields.FindField("ZLDWMC"), pFeature.get_Value(pFeature.Fields.FindField("ZLDWMC")));
                                        pB.set_Value(pB.Fields.FindField("MSSM"), pFeature.get_Value(pFeature.Fields.FindField("MSSM")));
                                        pInsert.InsertFeature(pB);
                                    }
                                }
                            }
                        }
                    }
                    pInsert.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGX);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);

                }
                string cjdcqYSDM = GetValueFromMDBByLayerName("CJDCQGX");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pCJDCQGX = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
                    (pCJDCQGX as ITable).DeleteSearchedRows(null);
                    IFeatureCursor pInsertCJDCQ = pCJDCQGX.Insert(true);
                    comRel.ManageLifetime(pInsertCJDCQ);
                    Dictionary<string, string> cjdcqdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "CJDCQGX", "ZLDWDM", "ZLDWMC",ref sameDm);
                    if (sameDm.Length > 0 && cjdcqdmmc == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (KeyValuePair<string, string> cjdcq in cjdcqdmmc)
                    {
                        IFeatureClass pSearchClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
                        string dwdm = cjdcq.Key;
                        string dwmc = cjdcq.Value;
                        string mssm = "00";
                        IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(pSearchClass, "ZLDWDM = '" + cjdcq.Key + "' And ZLDWMC = '" + cjdcq.Value + "'");
                        List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (pSearchClass as IGeoDataset).SpatialReference);
                        foreach (IGeometry item in pCJs)
                        {
                            IFeatureBuffer pFeatureBuffer = pCJDCQGX.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = item;
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWDM"), dwdm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("ZLDWMC"), dwmc);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), mssm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("YSDM"), cjdcqYSDM);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("GXSJ"), ""+dateEdit1.Text+"/12/31");
                            pInsertCJDCQ.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSearchClass);

                    }
                    pInsertCJDCQ.Flush();

                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGX);

                }
                RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + @"\TDGX\CJDCQGX");
                UpdateStatus("正在查找变化的行政区……");
                List<string> newXZQ = new List<string>();
                Dictionary<string, string> dmmc =GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "XZQ", "XZQDM", "XZQMC",ref sameDm);
                if (sameDm.Length > 0 && dmmc == null)
                {
                    MessageBox.Show("坐落单位为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("SELECT DM, MC From SYS_XJXZQ", "tmp");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string dm = dt.Rows[i][0].ToString();
                    if (dmmc.ContainsKey(dm))
                    {
                        dmmc.Remove(dm);
                    }
                    dmmc.Add(dm, dt.Rows[i][1].ToString());
                }
                //行政区====================================================================================================================================
                UpdateStatus("正在查找变化的行政区……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pXZQBHtmp = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQBHtmp");
                    IFeatureCursor pInsertXZQ = pXZQBHtmp.Insert(true);
                    comRel.ManageLifetime(pInsertXZQ);
                    List<string> XZQdmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "CJDCQGX", "ZLDWDM", 9);
                    foreach (string XZQ in XZQdmmc)
                    {
                        IFeatureClass pSearchClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
                        string dwdm = XZQ;
                        string mssm = "00";
                        IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(pSearchClass, "ZLDWDM Like '" + XZQ + "%'");
                        List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (pSearchClass as IGeoDataset).SpatialReference);
                        foreach (IGeometry item in pCJs)
                        {
                            ITopologicalOperator pTop = item as ITopologicalOperator;
                            IArray pArry = pIdenXZQ.Identify(item);
                            if (pArry != null)
                            {
                                for (int i = 0; i < pArry.Count; i++)
                                {
                                    IFeatureIdentifyObj idObj = pArry.get_Element(i) as IFeatureIdentifyObj;
                                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                                    IFeature pfea = pRow.Row as IFeature;
                                    string zldwdm = pfea.get_Value(pfea.Fields.FindField("XZQDM")).ToString();
                                    mssm = pfea.get_Value(pfea.Fields.FindField("MSSM")).ToString();
                                    IGeometry pGeo = pTop.Intersect(pfea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                                    if (pGeo != null)
                                    {
                                        IArea pArea = pGeo as IArea;
                                        if (pArea.Area > 0.1 && (dwdm != zldwdm))
                                        {
                                            IFeatureBuffer pFeatureBuffer = pXZQBHtmp.CreateFeatureBuffer();
                                            pFeatureBuffer.Shape = pGeo;
                                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQDM"), dwdm);
                                            if (dmmc.ContainsKey(dwdm))
                                            {
                                                pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQMC"), dmmc[dwdm]);
                                            }
                                            else
                                            {
                                                newXZQ.Add(dwdm);
                                            }
                                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), mssm);

                                            pInsertXZQ.InsertFeature(pFeatureBuffer);
                                        }
                                    }
                                }
                            }
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSearchClass);

                    }
                    pInsertXZQ.Flush();

                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQBHtmp);

                }
                UpdateStatus("正在处理变化的行政区……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pXZQBH = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQBH");
                    IFeatureCursor pInsertXZQ = pXZQBH.Insert(true);
                    comRel.ManageLifetime(pInsertXZQ);
                    Dictionary<string, string> XZQdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "XZQBHtmp", "XZQDM", "XZQMC",ref sameDm);
                    if (sameDm.Length > 0 && XZQdmmc == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (KeyValuePair<string, string> XZQ in XZQdmmc)
                    {
                        IFeatureClass pSearchClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQBHtmp");
                        string dwdm = XZQ.Key;
                        string dwmc = XZQ.Value;
                        string mssm = "00";
                        string sWhere = "XZQDM = '" + XZQ.Key + "' And XZQMC = '" + XZQ.Value + "'";
                        if (string.IsNullOrWhiteSpace(XZQ.Value))
                        {
                            sWhere = "XZQDM = '" + XZQ.Key + "'";
                        }
                        IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(pSearchClass, sWhere);
                        List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (pSearchClass as IGeoDataset).SpatialReference);
                        foreach (IGeometry item in pCJs)
                        {
                            IFeatureBuffer pFeatureBuffer = pXZQBH.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = item;
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQDM"), dwdm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQMC"), dwmc);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), mssm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("YSDM"), "1000600100");

                            pInsertXZQ.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSearchClass);

                    }
                    pInsertXZQ.Flush();

                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQBH);

                }
                UpdateStatus("正在查找变化的三调行政区……");
                string xzqYSDM = GetValueFromMDBByLayerName("XZQGX");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pXZQSD = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQSD");
                    IFeatureCursor pInsertXZQ = pXZQSD.Insert(true);
                    IFeatureClass pXZQBH = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQBH");
                    IGeometry pBH = RCIS.GISCommon.GeometryHelper.MergeGeometry(pXZQBH);
                    ITopologicalOperator pTop = pBH as ITopologicalOperator;
                    IArray pArry = pIdenXZQ.Identify(pBH);
                    if (pArry != null)
                    {
                        for (int i = 0; i < pArry.Count; i++)
                        {
                            IFeatureIdentifyObj idObj = pArry.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature pfea = pRow.Row as IFeature;
                            IGeometry pGeo = pTop.Intersect(pfea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                            if (pGeo != null)
                            {
                                IArea pArea = pGeo as IArea;
                                if (pArea.Area > 0.1)
                                {
                                    IFeatureBuffer pFeatureBuffer = pXZQSD.CreateFeatureBuffer();
                                    pFeatureBuffer.Shape = pfea.ShapeCopy;
                                    pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQDM"), pfea.get_Value(pfea.Fields.FindField("XZQDM")).ToString());
                                    pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQMC"), pfea.get_Value(pfea.Fields.FindField("XZQMC")).ToString());
                                    pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), pfea.get_Value(pfea.Fields.FindField("MSSM")).ToString());
                                    //pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("YSDM"), xzqYSDM);

                                    pInsertXZQ.InsertFeature(pFeatureBuffer);
                                    using (ESRI.ArcGIS.ADF.ComReleaser comrel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                                    {
                                        IQueryFilter pQF = new QueryFilterClass();
                                        pQF.WhereClause = "XZQDM = '" + pfea.get_Value(pfea.Fields.FindField("XZQDM")).ToString() +
                                                        "' And XZQMC = '" + pfea.get_Value(pfea.Fields.FindField("XZQMC")).ToString() +
                                                        "' And " + pXZQClass.OIDFieldName + " <> " + pfea.OID;
                                        IFeatureCursor pFC = pXZQClass.Search(pQF, true);
                                        comrel2.ManageLifetime(pFC);
                                        IFeature pF;
                                        while ((pF = pFC.NextFeature()) != null)
                                        {
                                            IFeatureBuffer pFeatureBuffer2 = pXZQSD.CreateFeatureBuffer();
                                            pFeatureBuffer2.Shape = pF.ShapeCopy;
                                            pFeatureBuffer2.set_Value(pFeatureBuffer2.Fields.FindField("XZQDM"), pF.get_Value(pF.Fields.FindField("XZQDM")).ToString());
                                            pFeatureBuffer2.set_Value(pFeatureBuffer2.Fields.FindField("XZQMC"), pF.get_Value(pF.Fields.FindField("XZQMC")).ToString());
                                            pFeatureBuffer2.set_Value(pFeatureBuffer2.Fields.FindField("MSSM"), pF.get_Value(pF.Fields.FindField("MSSM")).ToString());
                                            //pFeatureBuffer2.set_Value(pFeatureBuffer.Fields.FindField("YSDM"), xzqYSDM);

                                            pInsertXZQ.InsertFeature(pFeatureBuffer2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    pInsertXZQ.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQBH);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQSD);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pInsertXZQ);

                }
                RCIS.GISCommon.GpToolHelper.Update(tmpWS.PathName + @"\XZQSD", tmpWS.PathName + @"\XZQBH", tmpWS.PathName + @"\XZQGX");
                UpdateStatus("正在导入行政区更新数据……");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pAll = pXZQClass.Search(null, false);
                    comRel.ManageLifetime(pAll);
                    IFeatureClass pXZQGX = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQGX");
                    IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pXZQGX);
                    Dictionary<string, string> dmmcgx = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "XZQGX", "XZQDM", "XZQMC",ref sameDm);
                    if (sameDm.Length > 0 && dmmcgx == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    ITopologicalOperator pT = pGeo as ITopologicalOperator;
                    IFeatureCursor pInsert = pXZQGX.Insert(true);
                    IFeature pFeature;
                    while ((pFeature = pAll.NextFeature()) != null)
                    {
                        string xzqdm = pFeature.get_Value(pFeature.Fields.FindField("XZQDM")).ToString();
                        string xzqmc = pFeature.get_Value(pFeature.Fields.FindField("XZQMC")).ToString();
                        if (dmmcgx.ContainsKey(xzqdm))
                        {
                            if (xzqmc == dmmcgx[xzqdm])
                            {
                                IGeometry pG = pT.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                                if (pG != null)
                                {
                                    IArea pArea = pG as IArea;
                                    if (pArea.Area < 0.02)
                                    {
                                        IFeatureBuffer pB = pXZQGX.CreateFeatureBuffer();
                                        pB.Shape = pFeature.ShapeCopy;
                                        pB.set_Value(pB.Fields.FindField("XZQDM"), pFeature.get_Value(pFeature.Fields.FindField("XZQDM")));
                                        pB.set_Value(pB.Fields.FindField("XZQMC"), pFeature.get_Value(pFeature.Fields.FindField("XZQMC")));
                                        pB.set_Value(pB.Fields.FindField("MSSM"), pFeature.get_Value(pFeature.Fields.FindField("MSSM")));
                                        //pB.set_Value(pB.Fields.FindField("YSDM"), xzqYSDM);
                                        //pB.set_Value(pB.Fields.FindField("GXSJ"), "2020/12/31");                                    
                                        pInsert.InsertFeature(pB);
                                    }
                                }
                            }
                        }
                    }
                    pInsert.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGX);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);

                }
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureClass pXZQGX = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGX");
                    (pXZQGX as ITable).DeleteSearchedRows(null);
                    IFeatureCursor pInsertXZQ = pXZQGX.Insert(true);
                    comRel.ManageLifetime(pInsertXZQ);
                    Dictionary<string, string> XZQdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "XZQGX", "XZQDM", "XZQMC",ref sameDm);
                    if (sameDm.Length > 0 && XZQdmmc == null)
                    {
                        MessageBox.Show("坐落单位代码为" + sameDm + "存在重名。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    foreach (KeyValuePair<string, string> XZQ in XZQdmmc)
                    {
                        IFeatureClass pSearchClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("XZQGX");
                        string dwdm = XZQ.Key;
                        string dwmc = XZQ.Value;
                        string mssm = "00";
                        IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(pSearchClass, "XZQDM = '" + XZQ.Key + "' And XZQMC = '" + XZQ.Value + "'");
                        List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (pSearchClass as IGeoDataset).SpatialReference);
                        foreach (IGeometry item in pCJs)
                        {
                            IFeatureBuffer pFeatureBuffer = pXZQGX.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = item;
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQDM"), dwdm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("XZQMC"), dwmc);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("MSSM"), mssm);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("YSDM"), xzqYSDM);
                            pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("GXSJ"), ""+dateEdit1.Text+"/12/31");
                            pInsertXZQ.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSearchClass);

                    }
                    pInsertXZQ.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGX);

                }
                RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + @"\TDGX\XZQGX");

                RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQClass);
                RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQLayer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pXZQClass);
                RCIS.Utility.OtherHelper.ReleaseComObject(pXZQLayer);


                if (newXZQ.Count == 0)
                {
                    MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("发现" + newXZQ.Count + "个新建乡镇，代码为“" + string.Join(",", newXZQ) + "”,需要赋值乡镇名称。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            //}
            //catch(Exception ex)
            //{
            //    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, ex.ToString());
            //    return;
            //}
        }

        private void FrmGetXzqGX_Load(object sender, EventArgs e)
        {
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureDataset pFeaDS = pFeaWs.OpenFeatureDataset("TDGX");
            List<IFeatureClass> allFC = DatabaseHelper.getAllFeatureClass(pFeaDS);
            this.cmbDltb.Properties.Items.Clear();
            cmbCjdcq.Properties.Items.Clear();
            cmbXzq.Properties.Items.Clear();
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;
                if (className == "DLTBGX") cmbDltb.Text = className + "|" + alias;
                if (className == "CJDCQGX") cmbCjdcq.Text = className + "|" + alias;
                if (className == "XZQGX") cmbXzq.Text = className + "|" + alias;
                this.cmbDltb.Properties.Items.Add(className + "|" + alias);
                this.cmbCjdcq.Properties.Items.Add(className + "|" + alias);
                this.cmbXzq.Properties.Items.Add(className + "|" + alias);
            }
            try
            {
                if (Directory.Exists(Application.StartupPath + @"\tmp\tmp.gdb"))
                {
                    IWorkspace pWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                    IDataset pDataset = pWS as IDataset;
                    pDataset.Delete();

                }
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                
            }
            catch (Exception ex) { }

            dateEdit1.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            dateEdit1.Properties.ShowToday = false;
            //dateEdit1.Properties.ShowM = false;
            dateEdit1.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dateEdit1.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
            dateEdit1.Properties.Mask.EditMask = "yyyy";
            dateEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
            string txtData = (System.DateTime.Now.Year - 1).ToString();
            dateEdit1.EditValue = System.DateTime.Now.AddYears(-1);
        }

        private string GetValueFromMDBByLayerName(string layerName) 
        {
            string sql = "select YSDM from SYS_YSDM where CLASSNAME='" + layerName + "'";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql,"tmp");
            return dt.Rows[0][0].ToString();
        }

        private Dictionary<string, string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, string valueField,ref string erroDM)
        {
            Dictionary<string, string> dmmc = new Dictionary<string, string>();
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
                comRel.ManageLifetime(pQDef);
                pQDef.Tables = tableName + " Group By " + keyField + "," + valueField;
                pQDef.SubFields = keyField + "," + valueField;
                ICursor pCur = pQDef.Evaluate();
                comRel.ManageLifetime(pCur);
                IRow pRow;
                while ((pRow = pCur.NextRow()) != null)
                {
                    string dm = pRow.get_Value(0).ToString();
                    string mc = pRow.get_Value(1).ToString();
                    if (dmmc.Keys.Contains(dm))
                    {
                        erroDM = dm;
                        erroDM += "("+mc+","+dmmc[dm]+")";
                        return null;
                    }
                    dmmc.Add(dm, mc);
                }
            }
            return dmmc;
        }
    }
}
