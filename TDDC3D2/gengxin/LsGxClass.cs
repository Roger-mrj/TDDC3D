using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using RCIS.Global;
using System.Data;
using RCIS.Utility;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesGDB;
using RCIS.DataInterface.VCTOut;
using ESRI.ArcGIS.Geoprocessing;

namespace TDDC3D.gengxin
{
    public class LsGxClass
    {
        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(int hProcess);

        public IWorkspace currWs = GlobalEditObject.GlobalWorkspace;
        public IMapControl2 pMapCtl = null;
        public string xzdm = "";
        public string bgData = "";
        public DevExpress.XtraEditors.MemoEdit info = null;


        public void getCJDCQGX()
        {
            IWorkspace pTmpWs = DeleteAndNewTmpGDB();
            GpToolHelper gp = new GpToolHelper();

            //bool b = gp.Dissolve(currWs.PathName + "\\TDGX\\DLTBGX", pTmpWs.PathName + "\\bgc", new[] { "ZLDWDM", "ZLDWMC" });
            bool b = RCIS.GISCommon.GpToolHelper.Union_analysis(currWs.PathName + "\\TDGX\\DLTBGX" + ";" + currWs.PathName + "\\TDDC\\CJDCQ", pTmpWs.PathName + "\\bgc");
            pTmpWs.ExecuteSQL("delete from bgc where FID_DLTBGX=-1 OR (ZLDWDM=ZLDWDM_1 AND ZLDWMC=ZLDWMC_1)");
            //IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry((pTmpWs as IFeatureWorkspace).OpenFeatureClass("bgc"));
            IFeatureClass pCJDCQ = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
            IFeatureClass pCJDCQGX = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
            b = RCIS.GISCommon.GpToolHelper.GP_TabulateIntersection(pTmpWs.PathName + "\\bgc", "OBJECTID", currWs.PathName + "\\TDDC\\CJDCQ", "ZLDWDM", pTmpWs.PathName + "\\Tab");
            ArrayList arr = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics((pTmpWs as IFeatureWorkspace).OpenTable("Tab"), null, "ZLDWDM");
            ArrayList arr1 = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics((pTmpWs as IFeatureWorkspace).OpenFeatureClass("bgc"), null, "ZLDWDM");
            arr.AddRange(arr1);
            (pTmpWs as IFeatureWorkspace).CreateFeatureClass("CJDCQSD", pCJDCQ.Fields, null, null, esriFeatureType.esriFTSimple, pCJDCQ.ShapeFieldName, null);
            IFeatureClass pSDC = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("CJDCQSD");
            IFeatureCursor pCursor = pCJDCQ.Search(null, true);
            IFeatureCursor pInsert = pSDC.Insert(true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                if (arr.Contains(pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")).ToString().Trim()))
                {
                    pInsert.InsertFeature(pFeature as  IFeatureBuffer);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            }
            pInsert.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            b = RCIS.GISCommon.GpToolHelper.Update(pTmpWs.PathName + "\\CJDCQSD", pTmpWs.PathName + "\\bgc", pTmpWs.PathName + "\\cUpdate");

            string[] aaa = { "ZLDWDM", "ZLDWMC", "MSSM" };
            object inFea = pTmpWs.PathName + "\\cUpdate";
            object outFea = pTmpWs.PathName + "\\CJDCQGX";
            b = gp.Dissolve(inFea, outFea, aaa);
            //b= RCIS.GISCommon.GpToolHelper.Dissolve(inFea, outFea, aaa);
            currWs.ExecuteSQL("delete from CJDCQGX");
            b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\CJDCQGX", currWs.PathName + "\\TDGX\\CJDCQGX");
            string cjdcqYSDM = GetValueFromMDBByLayerName("CJDCQGX");
            currWs.ExecuteSQL("update CJDCQGX set YSDM='" + cjdcqYSDM + "'");
            Dictionary<string, string> dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "CJDCQ", "ZLDWDM", "ZLDWMC");
            IQueryFilter pQF=new QueryFilterClass();
            pQF.WhereClause="ZLDWMC='' OR ZLDWMC IS NULL";
            IFeatureCursor pFeaCursor = pCJDCQGX.Update(pQF,true);
            IFeature pFea;
            while ((pFea = pFeaCursor.NextFeature())!=null) 
            {
                string dwdm = pFea.get_Value(pFea.Fields.FindField("ZLDWDM")).ToString().Trim();
                if (dmmc.Keys.Contains(dwdm))
                    pFea.set_Value(pFea.Fields.FindField("ZLDWMC"), dmmc[dwdm]);
                pFeaCursor.UpdateFeature(pFea);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFea);
            }
            pFeaCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);

            //if (dmmcGX.Values.Contains("")||dmmcGX.Values.Contains(null))
            //{
            //    Dictionary<string, string> dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "CJDCQ", "ZLDWDM", "ZLDWMC");
            //    foreach (string item in dmmcGX.Keys)
            //    {
            //        if(dmmcGX[item].ToString().Trim()==""&&dmmc.Keys.Contains(item))
            //            currWs.ExecuteSQL("update CJDCQGX set ZLDWMC='"+dmmc[item]+"' where ZLDWDM='"+item+"'");
            //    }
            //}
        }

        public void getXZQGX()
        {
            IWorkspace pTmpWs = DeleteAndNewTmpGDB();
            GpToolHelper gp = new GpToolHelper();

            //bool b = gp.Dissolve(currWs.PathName + "\\TDGX\\DLTBGX", pTmpWs.PathName + "\\bgx", new[] { "ZLDWDM", "ZLDWMC" });
            bool b = RCIS.GISCommon.GpToolHelper.Union_analysis(currWs.PathName + "\\TDGX\\DLTBGX" + ";" + currWs.PathName + "\\TDDC\\XZQ", pTmpWs.PathName + "\\bgx");
            pTmpWs.ExecuteSQL("update bgx set zldwdm=substring(zldwdm,1,9)");
            pTmpWs.ExecuteSQL("delete from bgx where FID_DLTBGX=-1 OR ZLDWDM=XZQDM ");
            //IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry((pTmpWs as IFeatureWorkspace).OpenFeatureClass("bgx"));
            IFeatureClass pXZQ = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            b = RCIS.GISCommon.GpToolHelper.GP_TabulateIntersection(pTmpWs.PathName + "\\bgx", "OBJECTID", currWs.PathName + "\\TDDC\\XZQ", "XZQDM", pTmpWs.PathName + "\\Tab");
            ArrayList arr = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics((pTmpWs as IFeatureWorkspace).OpenTable("Tab"), null, "XZQDM");
            ArrayList arr1 = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics((pTmpWs as IFeatureWorkspace).OpenFeatureClass("bgx"), null, "ZLDWDM");
            arr.AddRange(arr1);
            (pTmpWs as IFeatureWorkspace).CreateFeatureClass("XZQSD", pXZQ.Fields, null, null, esriFeatureType.esriFTSimple, pXZQ.ShapeFieldName, null);
            IFeatureClass pSDC = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("XZQSD");
            IFeatureCursor pCursor = pXZQ.Search(null, true);
            IFeatureCursor pInsert = pSDC.Insert(true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                if (arr.Contains(pFeature.get_Value(pFeature.Fields.FindField("XZQDM")).ToString().Trim()))
                {
                    pInsert.InsertFeature(pFeature as IFeatureBuffer);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            }
            pInsert.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            pTmpWs.ExecuteSQL("update bgx set xzqdm=zldwdm");
            b = RCIS.GISCommon.GpToolHelper.Update(pTmpWs.PathName + "\\XZQSD", pTmpWs.PathName + "\\bgx", pTmpWs.PathName + "\\xUpdate");
            string[] aaa = { "XZQDM","MSSM"};
            object inFea = pTmpWs.PathName + "\\xUpdate";
            object outFea = pTmpWs.PathName + "\\XZQGX";
            b = gp.Dissolve(inFea, outFea, aaa);
            //b= RCIS.GISCommon.GpToolHelper.Dissolve(inFea, outFea, aaa);
            currWs.ExecuteSQL("delete from XZQGX");
            b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\XZQGX", currWs.PathName + "\\TDGX\\XZQGX");
            string xzqYSDM = GetValueFromMDBByLayerName("XZQGX");
            currWs.ExecuteSQL("update XZQGX set YSDM='" + xzqYSDM + "'");

            Dictionary<string, string> dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "XZQ", "XZQDM", "XZQMC");
            IFeatureClass pXZQGX = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGX");
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "XZQMC='' OR XZQMC IS NULL";
            IFeatureCursor pFeaCursor = pXZQGX.Update(pQF, true);
            IFeature pFea;
            while ((pFea = pFeaCursor.NextFeature()) != null)
            {
                string dwdm = pFea.get_Value(pFea.Fields.FindField("XZQDM")).ToString().Trim();
                if (dmmc.Keys.Contains(dwdm))
                    pFea.set_Value(pFea.Fields.FindField("XZQMC"), dmmc[dwdm]);
                pFeaCursor.UpdateFeature(pFea);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFea);
            }
            pFeaCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);

        }

        public ArrayList GetUniqueFieldValueByDataStatistics(IFeatureClass pFC, string fieldName, IGeometry geo)
        {
            ArrayList arrValues = new ArrayList();
            try
            {
                ISpatialFilter pQueryFilter = new SpatialFilterClass();
                pQueryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                pQueryFilter.Geometry = geo;
                IFeatureCursor pFeatureCursor = null;
                pQueryFilter.SubFields = fieldName;
                pFeatureCursor = pFC.Search(pQueryFilter, true);

                IDataStatistics pDataStati = new DataStatisticsClass();
                pDataStati.Field = fieldName;
                pDataStati.Cursor = (ICursor)pFeatureCursor;

                IEnumerator pEnumerator = pDataStati.UniqueValues;
                pEnumerator.Reset();
                while (pEnumerator.MoveNext())
                {
                    object pObj = pEnumerator.Current;
                    arrValues.Add(pObj.ToString());
                }

                arrValues.Sort();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQueryFilter);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            catch (Exception ex)
            {
            }
            return arrValues;
        }

        public void getLmfw()
        {
            IWorkspace tmpWS = DeleteAndNewTmpGDB();
            RCIS.GISCommon.GpToolHelper.Update(GlobalEditObject.GlobalWorkspace.PathName + @"\TDDC\DLTB", GlobalEditObject.GlobalWorkspace.PathName + @"\TDGX\DLTBGX", Application.StartupPath + @"\tmp\tmp.gdb\NMDLTB");
            tmpWS.ExecuteSQL("delete from NMDLTB where DLBM<>'1001' AND DLBM<>'1003'");
            IFeatureClass pNMtb = (tmpWS as IFeatureWorkspace).OpenFeatureClass("NMDLTB");
            if (pNMtb.FeatureCount(null) > 0)
            {
                string sql = "select YSDM,LayerDM from SYS_YSDM where CLASSNAME='LMFWGX'";
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                string cdm = "", ysdm = "";
                if (dt.Rows.Count > 0)
                {
                    ysdm = dt.Rows[0][0].ToString().Trim();
                    cdm = dt.Rows[0][1].ToString().Trim();
                }
                string where = "BSM LIKE '" + xzdm + "%'";
                long BSM = 0;
                string maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringOrderBy((currWs as IFeatureWorkspace).OpenFeatureClass("LMFW"), "BSM", where);
                //maxBSM++;
                cdm = xzdm + cdm;

                if (maxBSM.ToString().Length > 8)
                    BSM = long.Parse(maxBSM.Substring(10, 8));
                BSM++;
                IFeatureCursor pCursor = pNMtb.Update(null, true);
                IFeature pFeature;
                while ((pFeature = pCursor.NextFeature()) != null)
                {
                    pFeature.set_Value(pFeature.Fields.FindField("YSDM"), ysdm);
                    pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + BSM++.ToString().PadLeft(8, '0'));
                    pCursor.UpdateFeature(pFeature);
                    OtherHelper.ReleaseComObject(pFeature);
                }
                pCursor.Flush();
                OtherHelper.ReleaseComObject(pCursor);
                ISchemaLock pSchemaLock = null;
                pSchemaLock = pNMtb as ISchemaLock;
                pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
                IClassSchemaEdit4 pClassSchemaEdit = pNMtb as IClassSchemaEdit4;

                pClassSchemaEdit.AlterFieldName("TBMJ", "MJ");
                pClassSchemaEdit.AlterFieldName("XZDWKD", "KD");
                pClassSchemaEdit.AlterFieldName("DLMC", "MC");
                pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                currWs.ExecuteSQL("delete from LMFWGX");
                bool B = RCIS.GISCommon.GpToolHelper.Append(Application.StartupPath + @"\tmp\tmp.gdb\NMDLTB", currWs.PathName + "\\TDGX\\LMFWGX");

            }

        }

        public void jsczcmj()
        {

            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;

            IFeatureClass pCZCDYDGX = pFeaWorkspace.OpenFeatureClass("CZCDYDGX");
            if (pCZCDYDGX == null || pCZCDYDGX.FeatureCount(null) <= 0)
            {
                MessageBox.Show("城镇村等用地更新层无数据！");
                return;
            }
            int czcmjIndex = pCZCDYDGX.Fields.FindField("CZCMJ");
            if (czcmjIndex <= -1)
            {
                MessageBox.Show("未找到城镇村面积字段！");
                return;
            }
            IWorkspace tmpWS = DeleteAndNewTmpGDB();


            //标识码
            Dictionary<string, string> dicDm = getCDM();
            string cdm = "0000";
            if (dicDm.ContainsKey("CZCDYDGX"))
            {
                cdm = dicDm["CZCDYDGX"];
            }
            else
            {
                if (dicDm.ContainsKey("CZCDYDGX"))
                {
                    cdm = dicDm["CZCDYDGX"];
                }
            }
            string where = "CZCDM LIKE '" + xzdm + "%'";
            IFeatureClass pBC = (currWs as IFeatureWorkspace).OpenFeatureClass("CZCDYD");
            long maxBSM = 0;
            maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pBC, "BSM", where);
            maxBSM++;
            if (maxBSM.ToString().Length < 8)
            {
                string bsm = xzdm + cdm + maxBSM.ToString().PadLeft(8, '0');
                long.TryParse(bsm, out maxBSM);
            }

            //用更新层更新基础层生成年末库
            string inFea = currWs.PathName + "\\TDDC\\DLTB";
            string gxFea = currWs.PathName + "\\TDGX\\DLTBGX";
            string outFea = tmpWS.PathName + "\\nmdltb";
            bool b = RCIS.GISCommon.GpToolHelper.Update(inFea, gxFea, outFea);
            if (b == false)
            {
                MessageBox.Show("叠加分析错误！");
                return;
            }
            //创建临时文件，存储交集制表结果
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "tmp";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            string resultMDBPath = tempPath + "\\result.mdb";
            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "SystemConf\\result.mdb", resultMDBPath, true);
            //交集制表，获取CZCDYDGX和DLTB层空间叠加关系
            string zoneFea = currWs.PathName + "\\TDGX\\CZCDYDGX";
            bool TabulateResult = GpToolHelper.GP_TabulateIntersection(zoneFea, "OBJECTID", outFea, "TBMJ", resultMDBPath + "\\result");
            if (TabulateResult == false)
            {
                MessageBox.Show("交集制表分析错误！");
                return;
            }
            //从交集制表结果中循环赋值给CZCDYDGX层的CZCMJ字段
            RCIS.Database.AccdbOperateHelper mdbHelper = new RCIS.Database.AccdbOperateHelper(resultMDBPath);
            DataTable dt = mdbHelper.GetDatatable("select OBJECTID_1,sum(tbmj) as ZMJ from result where area>0.01 group by OBJECTID_1");
            if (dt == null || dt.Rows.Count <= 0)
            {
                MessageBox.Show("未找到交集制表结果！");
                return;
            }
            IFeatureCursor pCZCDYDGXCur = pCZCDYDGX.Search(null, false);
            IFeature pFeaCZC = null;
            while ((pFeaCZC = pCZCDYDGXCur.NextFeature()) != null)
            {
                DataRow[] drs = dt.Select(string.Format("OBJECTID_1={0}", pFeaCZC.OID));
                if (drs != null && drs.Length > 0)
                {
                    pFeaCZC.set_Value(czcmjIndex, MathHelper.Round(double.Parse(drs[0]["ZMJ"].ToString()), 2));
                    pFeaCZC.set_Value(pFeaCZC.Fields.FindField("GXSJ"), bgData);
                    pFeaCZC.set_Value(pFeaCZC.Fields.FindField("BSM"), maxBSM++);
                    pFeaCZC.Store();
                }
                if (pFeaCZC != null)
                {
                    Marshal.FinalReleaseComObject(pFeaCZC);
                }
            }
            Dictionary<string, string> dicQsdwdm = getZldwdmMc();
            foreach (KeyValuePair<string, string> aItem in dicQsdwdm)
            {
                string dm = aItem.Key.Trim();
                if (dm.Length == 6)
                {
                    dm += "0000000000000";
                }
                else if (dm.Length == 9)
                {
                    dm += "0000000000";
                }
                else if (dm.Length == 12)
                {
                    dm += "0000000";
                }
                string sql = "update CZCDYDGX set CZCMC='" + aItem.Value.Trim() + "' where CZCDM='" + dm + "'";
                currWs.ExecuteSQL(sql);
                Application.DoEvents();
            }

            if (tmpWS != null)
            {
                Marshal.FinalReleaseComObject(tmpWS);
            }
            if (pCZCDYDGX != null)
            {
                Marshal.FinalReleaseComObject(pCZCDYDGX);
            }
            if (pCZCDYDGXCur != null)
            {
                Marshal.FinalReleaseComObject(pCZCDYDGXCur);
            }
            if (dt != null)
            {
                dt.Dispose();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        public Dictionary<string, string> getZldwdmMc()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ITable pTable = null;
            try
            {
                pTable = (currWs as IFeatureWorkspace).OpenTable("QSDWDMB");
            }
            catch (Exception ex)
            {
            }
            if (pTable == null)
            {
                UpdateStatus("未找到QSDWDMB,城镇村名称需自行赋值。");
                return dic;
            }


            IRow aRow = null;
            ICursor pCursor = pTable.Search(null, false);
            try
            {
                while ((aRow = pCursor.NextRow()) != null)
                {
                    dic.Add(FeatureHelper.GetRowValue(aRow, "QSDWDM").ToString(), FeatureHelper.GetRowValue(aRow, "QSDWMC").ToString());
                }
            }
            catch { }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
            }
            return dic;
        }

        public void dissoveCzcgx()
        {
            //执行融合string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            IFeatureClass czcdydClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CZCDYDGX");
            if (czcdydClass == null)
            {
                MessageBox.Show("加载图层不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行数据融合", "请稍等...");
            //wait.Show();
            //this.Cursor = Cursors.WaitCursor;
            try
            {
                string tmpPath = Application.StartupPath + "\\tmp\\";
                string tmpShpfile = tmpPath + "czcydtmp.shp";
                //bool bOk = DissovePolygon(tmpShpfile);

                string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
                IWorkspace tmpWS = DeleteAndNewTmpGDB();

                string[] arr = { "CZCLX", "CZCDM" };
                //IFeatureClass pTmpClass = RCIS.GISCommon.GpToolHelper.Dissolve(czcdydClass, tmpWS, "czcdydDissolve", arr);
                object inFea = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\TDGX\\CZCDYDGX";
                object outFea = tmpWS.PathName + "\\czcdydDissolve";
                RCIS.GISCommon.GpToolHelper gp = new GpToolHelper();
                bool b = gp.Dissolve(inFea, outFea, arr);
                IFeatureClass pTmpClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("czcdydDissolve");

                if (pTmpClass == null)
                {
                    //wait.Close();
                    //this.Cursor = Cursors.Default;
                    MessageBox.Show("融合过程失败");
                    return;
                }

                string name = (czcdydClass as IDataset).Name;
                RCIS.Global.GlobalEditObject.GlobalWorkspace.ExecuteSQL("delete from " + name + "");
                if (RCIS.GISCommon.GpToolHelper.Append(tmpWS.PathName + "\\czcdydDissolve", (czcdydClass as IDataset).Workspace.PathName + "\\TDGX\\CZCDYDGX"))
                {
                    //(czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set GXSJ="+DateTime.Parse("2020/12/31")+"");
                    string sql = "select ysdm from sys_ysdm where classname='CZCDYDGX'";
                    DataRow dr = RCIS.Database.LS_SetupMDBHelper.GetDataRow(sql, "sys_ysdm");
                    if (dr != null)
                    {
                        string ysdm = dr[0].ToString();
                        (czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set YSDM='"+ysdm+"'");
                    }

                    //wait.Close();
                    //this.Cursor = Cursors.Default;
                    //MessageBox.Show("融合完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\CZCDYDGX");
            }
            catch (Exception ex)
            {
                //wait.Close();
                //this.Cursor = Cursors.Default;
                UpdateStatus(ex.Message);
            }
        }

        public void getCzcgx()
        {
            //相当于 提取 201，202，203，204，205 ，然后 ，修改城镇村等用地类型 ，城镇村代码，城镇村面积

            IFeatureClass dltbClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            IFeatureClass czcdydClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CZCDYDGX");

            if (dltbClass == null || czcdydClass == null)
            {
                MessageBox.Show("加载图层不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //判断城镇村属性码是否标准
            List<string> czcArr = new List<string>() { "201", "202", "203", "204", "205", "201A", "202A", "203A" };
            ArrayList czcsxmArr = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(dltbClass, null, "CZCSXM");
            List<string> erroDm = new List<string>();
            string erroStr = "";
            foreach (string item in czcsxmArr)
            {
                if (!czcArr.Contains(item) && !string.IsNullOrWhiteSpace(item))
                    erroDm.Add(item);
            }
            if (erroDm.Count > 0)
            {
                erroStr = "地类图斑更新层中城镇村属性码为";
                for (int i = 0; i < erroDm.Count; i++)
                {
                    erroStr += "'" + erroDm[i] + "',";
                }
                erroStr += "填写不规范。";
            }
            if (erroStr.Length > 0)
            {
                MessageBox.Show(erroStr, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //List<string>
            IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWsEdt.StartEditing(false);
            pWsEdt.StartEditOperation();
            ITable pTable = czcdydClass as ITable;
            pTable.DeleteSearchedRows(null);
            pWsEdt.StopEditOperation();
            pWsEdt.StopEditing(true);
            //switch (MessageBox.Show("是否删除城镇村更新层中原有的数据？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            //{
            //    case DialogResult.Cancel:
            //        return;
            //        break;
            //    case DialogResult.No:
            //        break;
            //    case DialogResult.Yes:
            //        IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            //        pWsEdt.StartEditing(false);
            //        pWsEdt.StartEditOperation();
            //        ITable pTable = czcdydClass as ITable;
            //        pTable.DeleteSearchedRows(null);
            //        pWsEdt.StopEditOperation();
            //        pWsEdt.StopEditing(true);
            //        break;
            //    default:
            //        break;
            //}

            //DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行提取...", "请稍等...");
            //wait.Show();
            IWorkspace tmpWS = DeleteAndNewTmpGDB();
            RCIS.GISCommon.GpToolHelper.Update(GlobalEditObject.GlobalWorkspace.PathName + @"\TDDC\DLTB", GlobalEditObject.GlobalWorkspace.PathName + @"\TDGX\DLTBGX", Application.StartupPath + @"\tmp\tmp.gdb\NewDLTB");

            tmpWS.ExecuteSQL("delete from NewDLTB where zldwdm not like '"+xzdm+"%'");

            getDltbToCzcgx((tmpWS as IFeatureWorkspace).OpenFeatureClass("NewDLTB"), czcdydClass);
            this.pMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            //wait.Close();
            //MessageBox.Show("提取完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void getDltbToCzcgx(IFeatureClass dltbClass, IFeatureClass czcdydClass)
        {
            ISchemaLock pSchemaLock = null;
            pSchemaLock = dltbClass as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = dltbClass as IClassSchemaEdit4;
            pClassSchemaEdit.AlterFieldName("CZCSXM", "CZCLX");
            pClassSchemaEdit.AlterFieldName("ZLDWDM", "CZCDM");
            //pClassSchemaEdit.AlterFieldName("CZCSXM", "CZCLX");
            pClassSchemaEdit.AlterFieldName("ZLDWMC", "CZCMC");
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            (dltbClass as IDataset).Workspace.ExecuteSQL("delete from " + (dltbClass as IDataset).Name + " where CZCLX='' or CZCLX is null");
            bool b = RCIS.GISCommon.GpToolHelper.Append((dltbClass as IDataset).Workspace.PathName + "\\" + (dltbClass as IDataset).Name, (czcdydClass as IDataset).Workspace.PathName + "\\TDGX\\CZCDYDGX");
            if (!b)
            {
                MessageBox.Show("gp工具错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "CZCLX like '202%' or CZCLX like '201%'";
            IFeatureCursor pCursor = czcdydClass.Update(pQf, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string czcdm = pFea.get_Value(pFea.Fields.FindField("CZCDM")).ToString().Trim();
                string czclx = pFea.get_Value(pFea.Fields.FindField("CZCLX")).ToString().Trim();
                if (czclx.StartsWith("201"))
                    pFea.set_Value(pFea.Fields.FindField("CZCDM"), czcdm.Substring(0, 6).PadRight(19, '0'));
                else if (czclx.StartsWith("202"))
                    pFea.set_Value(pFea.Fields.FindField("CZCDM"), czcdm.Substring(0, 9).PadRight(19, '0'));
                pCursor.UpdateFeature(pFea);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

        }

        public void ProduceXZQGXGC()
        {
            IFeatureWorkspace pFWor = currWs as IFeatureWorkspace;
            IFeatureClass pGXFeaClass = null;
            try
            {
                pGXFeaClass = pFWor.OpenFeatureClass("XZQGX");
                if (pGXFeaClass.FeatureCount(null) == 0)
                {
                    //MessageBox.Show("行政区更新层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                    //message = false;
                    //return message;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到行政区更新层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //if (MessageBox.Show("原行政区更新过程层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            UpdateStatus("正在处理更新层行政区数据");
            IGeometry pGeometry = RCIS.GISCommon.GeometryHelper.MergeGeometry(pGXFeaClass);
            UpdateStatus("正在查找与变化行政区重叠的三调行政区");
            IFeatureClass pXZQClass = null;
            try
            {
                pXZQClass = pFWor.OpenFeatureClass("XZQ");
                if (pXZQClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("行政区层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到行政区层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string XZQSD = Application.StartupPath + @"\tmp\XZQSD.shp";
            string XZQGXGC = Application.StartupPath + @"\tmp\XZQGXGC.shp";
            IFeatureClass pSDClass;
            if (File.Exists(XZQSD))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQSD);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            if (File.Exists(XZQGXGC))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQGXGC);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            pSDClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(XZQSD, esriGeometryType.esriGeometryPolygon, (pXZQClass as IGeoDataset).SpatialReference, pXZQClass.Fields);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.GeometryField = pXZQClass.ShapeFieldName;
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pSpatialFilter.SpatialRelDescription = "T********";
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsertCursor = pSDClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeatureCursor pSearchCursor = pXZQClass.Search(pSpatialFilter, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IGeometry pGeoIntersect = pTop.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (pGeoIntersect != null)
                    {
                        IArea pArea = pGeoIntersect as IArea;
                        if (pArea.Area > 0.0001)
                        {
                            pInsertCursor.InsertFeature(pFeature as IFeatureBuffer);
                        }
                    }
                }
                pInsertCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            UpdateStatus("正在进行叠加分析");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
            string gxPath = currWs.PathName + "\\TDGX\\XZQGX";
            pUnion.in_features = XZQSD + ";" + gxPath;
            pUnion.out_feature_class = XZQGXGC;
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                UpdateStatus("叠加分析错误");
                //message = false;
                //return message;
                return;
            }
            string MultipartToSinglepart = Application.StartupPath + "\\tmp\\MultipartToSinglepart.shp";
            if (File.Exists(MultipartToSinglepart))
            {
                ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart)) as IDataset).Delete();
            }
            bool result = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(XZQGXGC, MultipartToSinglepart);
            if (result == false)
            {
                UpdateStatus("叠加分析错误");
                //message = false;
                //return message;
                return;
            }
            UpdateStatus("正在导入叠加结果");
            ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQGXGC)) as IDataset).Delete();
            IFeatureClass pSourClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart);
            IFeatureClass pTarClass = null;
            try
            {
                pTarClass = pFWor.OpenFeatureClass("XZQGXGC");
            }
            catch
            {
                UpdateStatus("数据库没有升级，请先升级数据库。");
                //message = false;
                //return message;
                return;
            }
            string where = "";
            if (xzdm.Length > 0)
                where = "XZQDM LIKE '" + xzdm + "%'";


            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQClass, "BSM", where) + 1;
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pGXFeaClass, "BSM", where) + 1;
            long maxBSM = 0;
            if (max1 > max2)
                maxBSM = max1;
            else
                maxBSM = max2;

            //long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQClass,"BSM")+1;
            ITable pTable = pTarClass as ITable;
            pTable.DeleteSearchedRows(null);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pSearchCursor = pSourClass.Search(null, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeatureCursor pInsertCursor = pTarClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    if (pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString().Trim() == pFeature.get_Value(pFeature.Fields.FindField("BSM_1")).ToString().Trim() || pFeature.get_Value(pFeature.Fields.FindField("BZ_1")).ToString().Trim() == "S")
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    else
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pSourClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pSourClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pSourClass.FindField("BSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ_1")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pSourClass.FindField("BZ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC_1")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsertCursor.InsertFeature(pFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            UpdateStatus("正在提取同名的行政区");
            System.Collections.ArrayList xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGQXZQDM");
            List<string> xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("XZQDM = '" + item.ToString() + "'");
            }
            string sWhere = string.Join(" Or ", xzqdm);
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pXZQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pXZQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pXZQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQDM"), pFeature.get_Value(pXZQClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQMC"), pFeature.get_Value(pXZQClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pXZQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pXZQClass.FindField("BZ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pXZQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pXZQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pXZQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pXZQClass.FindField("BSM")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), maxBSM++);
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pXZQClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pXZQClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pXZQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pXZQClass.FindField("BZ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pXZQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pXZQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pXZQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    //RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGQXZQDM = '" + pFeature.get_Value(pXZQClass.FindField("XZQDM")) + "'");
                }
                pInsert.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeaClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTarClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSourClass);
            currWs.ExecuteSQL("update xzqgx set bz=''");
            currWs.ExecuteSQL("update xzqgxgc set bghbz=''");
            //currWs.ExecuteSQL("delete from xzqgxgc where BGQBSM is null or BGHBSM is null or BGQBSM='' or BGHBSM=''");
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\XZQGXGC");

            JSMJXZQ("");

            XZQJX1();

            //currWs.ExecuteSQL("update dltbgxgc set BGHTBBSM='',BGHDLBM='',BGHDLMC='',BGHQSXZ='',BGHQSDWDM='',BGHQSDWMC='',BGHZLDWDM='',BGHZLDWMC='',BGHKCDLBM='',BGHKCXS=0,BGHKCMJ=0,BGHTBDLMJ=0,BGHGDLX='',BGHGDPDJB='',BGHXZDWKD=0,BGHTBXHDM='',BGHTBXHMC='',BGHZZSXDM='',BGHZZSXMC='',BGHGDDB=0,BGHFRDBS='',BGHCZCSXM='',BGHMSSM='',BGHHDMC='',BGHTBBH='' where xzqtzlx='4' or xzqtzlx='2'");

            currWs.ExecuteSQL("update xzqgxgc set bgxw='3' where bgqxzqdm='' or bgqxzqdm is null");
            currWs.ExecuteSQL("update xzqgxgc set bgxw='0' where bghxzqdm like '000000%'");
            currWs.ExecuteSQL("update xzqgxgc set BGHBSM='',BGHXZQDM='',BGHXZQMC='',BGHDCMJ=0,BGHBZ='',BGHMSSM='',BGHHDMC='' where bghxzqdm not like '" + xzdm + "%'");


            UpdateStatus("行政区更新过程层提取完毕");

        }

        public void XZQJX1()
        {
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQJXGX = pFeaWorkspace.OpenFeatureClass("XZQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            (pXZQJXGX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取行政区界线");
            List<string> IDwbh = new List<string>();
            string xzqjcgxYSDM = GetValueFromMDBByLayerName("XZQJXGX");
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");

            //IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("XZQGX");
            //IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);

            bool updata = RCIS.GISCommon.GpToolHelper.Update(filePath + "\\TDDC\\XZQ", filePath + "\\TDGX\\XZQGX", tmpWS.PathName + "\\xzqnmk");

            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "xzqdm not like '" + xzdm + "%'";
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(RCIS.Global.GlobalEditObject.GlobalWorkspace, tmpWS, "XZQGX", "XZQOut", pQf);
            bool b1 = RCIS.GISCommon.GpToolHelper.Erase_analysis(filePath + "\\TDGX\\XZQGX", tmpWS.PathName + "\\XZQOut", tmpWS.PathName + "\\xzqNew");
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry((tmpWS as IFeatureWorkspace).OpenFeatureClass("xzqNew"));


            IFeatureClass pGXJX = GPPolygonToLineByFeatureclass(tmpWS.PathName + "\\xzqnmk", tmpWS);
            IFeatureClass pXZQJX = pFeaWorkspace.OpenFeatureClass("XZQJX");

            //县代码

            string where = "";
            if (xzdm.Length > 0)
                where = "BSM LIKE '" + xzdm + "%'";

            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQJX, "BSM", where) + 1;

            if (maxBSM.ToString().Length < 18)
            {
                Dictionary<string, string> dicDm = getCDM();
                string cdm = "0000";
                if (dicDm.ContainsKey("XZQJXGX"))
                {
                    cdm = dicDm["XZQJXGX"];
                }
                else
                {
                    if (dicDm.ContainsKey("XZQJX"))
                    {
                        cdm = dicDm["XZQJX"];
                    }
                }
                string bsm = xzdm + cdm + maxBSM.ToString().PadLeft(8, '0');
                maxBSM = long.Parse(bsm);
            }

            ISpatialFilter pSpatialFil = new SpatialFilterClass();
            pSpatialFil.Geometry = pGeo;
            pSpatialFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pGXJXCursor = pGXJX.Search(null, true);
            IFeature pFeature;

            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pXZQJX;
            IIdentify pIdentify = pLayer as IIdentify;
            IFeatureCursor pInsert = pXZQJXGX.Insert(true);
            List<int> arr = new List<int>();
            while ((pFeature = pGXJXCursor.NextFeature()) != null)
            {
                if (!GetIntersectLength(pFeature, pGeo))
                    continue;
                bool b = false;
                List<IFeature> pList0 = new List<IFeature>();
                IArray pArray = pIdentify.Identify(pFeature.ShapeCopy);
                if (pArray != null)
                {
                    IRelationalOperator pRel = (pFeature.ShapeCopy) as IRelationalOperator;
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pFea = pRow.Row as IFeature;
                        if (pRel.Equals(pFea.ShapeCopy))
                        {
                            IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pFea.get_Value(pFea.Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pFea.get_Value(pFea.Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pFea.get_Value(pFea.Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "4");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pFea.OID);
                            b = true;
                            break;
                        }
                        else
                        {
                            if (GetIntersectLen(pFea, pFeature))
                                pList0.Add(pFea);
                        }
                    }
                }
                if (!b)
                {
                    IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                    pFeaBuffer.Shape = pFeature.ShapeCopy;
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), maxBSM++);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), "660200");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), "600001");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "3");
                    pInsert.InsertFeature(pFeaBuffer);
                    for (int i = 0; i < pList0.Count; i++)
                    {
                        if (!arr.Contains(pList0[i].OID))
                        {
                            pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pList0[i].ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pList0[i].get_Value(pList0[i].Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pList0[i].get_Value(pList0[i].Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pList0[i].get_Value(pList0[i].Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pList0[i].OID);
                        }
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);

            }

            IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("XZQGX");
            pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);

            ISpatialFilter pSf = new SpatialFilterClass();
            pSf.Geometry = pGeo;
            pSf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = pXZQJX.Search(pSf, true);
            IFeature pJXFea;
            while ((pJXFea = pCursor.NextFeature()) != null)
            {
                if (arr.Contains(pJXFea.OID)||!GetIntersectLen(pJXFea,pGeo))
                    continue;
                IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                pFeaBuffer.Shape = pJXFea.ShapeCopy;
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pJXFea.get_Value(pJXFea.Fields.FindField("BSM")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pJXFea.get_Value(pJXFea.Fields.FindField("JXLX")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pJXFea.get_Value(pJXFea.Fields.FindField("JXXZ")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                pInsert.InsertFeature(pFeaBuffer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pJXFea);

            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);


            RCIS.Utility.OtherHelper.ReleaseComObject(pGXJXCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\XZQJXGX");

        }

        public void JSMJXZQ(string awm)
        {
            //数据汇总流程
            //1.计算变更后行政区，行政区的图形范围
            //2.计算变更后行政区，行政区中变更图斑的面积
            //3.计算变更后行政区，行政区中三调图斑面积并汇总
            IWorkspace pTmpWs = null;
            IFeatureClass pTB = null;
            UpdateStatus("正在计算行政区更新过程层面积");

            pTmpWs = DeleteAndNewTmpGDB();
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWs, "DLTBGXGC", "TBGXGC", null);
            IFeatureClass pTBGXGC = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TBGXGC");
            ISchemaLock pSchemaLock = null;
            pSchemaLock = pTBGXGC as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = pTBGXGC as IClassSchemaEdit4;
            pClassSchemaEdit.AlterFieldName("TBBGMJ", "TBMJ");
            pClassSchemaEdit.AlterFieldName("BGHZLDWDM", "ZLDWDM");
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

            bool b = RCIS.GISCommon.GpToolHelper.Update(currWs.PathName + "\\TDDC\\DLTB", pTmpWs.PathName + "\\TBGXGC", pTmpWs.PathName + "\\TB");
            if (!b)
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            pTB = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TB");
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pTB;
            IIdentify pIdentify = pLayer as IIdentify;

            RCIS.Utility.OtherHelper.ReleaseComObject(pTBGXGC);

            IFeatureClass pXZQGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGXGC");
            Dictionary<string, double> dm_mj = new Dictionary<string, double>();
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
                    IFeature pXZQGXGC;
                    while ((pXZQGXGC = pXZQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGQXZQDM")).ToString();
                        string bghdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGHXZQDM")).ToString();

                        IArray pArr = pIdentify.Identify(pXZQGXGC.ShapeCopy);
                        if (pArr != null)
                        {
                            for (int i = 0; i < pArr.Count; i++)
                            {
                                IFeatureIdentifyObj idObj = pArr.get_Element(i) as IFeatureIdentifyObj;
                                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                                IFeature pFeature = pRow.Row as IFeature;
                                string zl = pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")).ToString().Substring(0, bghdwdm.Length);
                                if (bghdwdm == zl)
                                {
                                    bool B = GetIntersectArea(pFeature, pXZQGXGC);
                                    if (B == true)
                                    {
                                        double TBMJ = double.Parse(pFeature.get_Value(pTB.FindField("TBMJ")).ToString());
                                        BGMJ += TBMJ;
                                        DCMJ += TBMJ;
                                    }
                                    RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                                }
                            }
                        }

                        pXZQGXGC.set_Value(pXZQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pXZQGXGCCursor.UpdateFeature(pXZQGXGC);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGC);
                    }
                    pXZQGXGCCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCCursor);
                }
                dm_mj.Add(dwdm, DCMJ);
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pXZQGXGCClass as ITable, "BGHDCMJ", Math.Round(dm_mj[dwdm], 2), "BGHXZQDM = '" + dwdm + "'");
                Application.DoEvents();
            }
            UpdateStatus("正在计算行政区更新层面积");
            IFeatureClass pXZQGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGX");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            IFeatureCursor pCursor = pXZQGXClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("XZQDM")).ToString();
                if (currDh == 0)
                {
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号
                }
                double JSMJ = area.SphereArea(pFea.ShapeCopy, currDh);
                pFea.set_Value(pFea.Fields.FindField("JSMJ"), Math.Round(JSMJ, 2));
                pFea.set_Value(pFea.Fields.FindField("DCMJ"), Math.Round(dm_mj[zldwdm], 2));
                pFea.set_Value(pFea.Fields.FindField("GXSJ"), bgData);

                pCursor.UpdateFeature(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTB);
            //UpdateStatus("行政区面积计算完成");
        }

        public void ProduceCJDCQGXGC()
        {
            IFeatureWorkspace pFWor = currWs as IFeatureWorkspace;
            IFeatureClass pGXFeatureclass = null;
            try
            {
                pGXFeatureclass = pFWor.OpenFeatureClass("CJDCQGX");
                if (pGXFeatureclass.FeatureCount(null) == 0)
                {
                    //MessageBox.Show("村级调查区更新层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到村级调查区更新层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("正在处理变化村级调查区数据");
            IGeometry pGeometry = RCIS.GISCommon.GeometryHelper.MergeGeometry(pGXFeatureclass);
            UpdateStatus("正在查找与变化村级调查区重叠的三调村级调查区");
            IFeatureClass pCJDCQClass = null;
            try
            {
                pCJDCQClass = pFWor.OpenFeatureClass("CJDCQ");
                if (pCJDCQClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("村级调查区层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("未找到村级调查区层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string CJDCQSD = Application.StartupPath + @"\tmp\CJDCQSD.shp";
            string CJDCQGXGC = Application.StartupPath + @"\tmp\CJDCQGXGC.shp";
            IFeatureClass pSDClass;
            if (File.Exists(CJDCQSD))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(CJDCQSD);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            if (File.Exists(CJDCQGXGC))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(CJDCQGXGC);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            pSDClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(CJDCQSD, esriGeometryType.esriGeometryPolygon, (pCJDCQClass as IGeoDataset).SpatialReference, pCJDCQClass.Fields);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.GeometryField = pCJDCQClass.ShapeFieldName;
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pSpatialFilter.SpatialRelDescription = "T********";
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsertCursor = pSDClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeatureCursor pSearchCursor = pCJDCQClass.Search(pSpatialFilter, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IGeometry pGeoIntersect = pTop.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (pGeoIntersect != null)
                    {
                        IArea pArea = pGeoIntersect as IArea;
                        if (pArea.Area > 0.001)
                        {
                            pInsertCursor.InsertFeature(pFeature as IFeatureBuffer);
                        }
                    }
                }
                pInsertCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            UpdateStatus("正在进行叠加分析");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
            string gxPath = currWs.PathName + "\\TDGX\\CJDCQGX";
            pUnion.in_features = CJDCQSD + ";" + gxPath;
            pUnion.out_feature_class = CJDCQGXGC;
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            string MultipartToSinglepart = Application.StartupPath + "\\tmp\\MultipartToSinglepart.shp";
            if (File.Exists(MultipartToSinglepart))
            {
                ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart)) as IDataset).Delete();
            }
            bool result = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(CJDCQGXGC, MultipartToSinglepart);
            if (result == false)
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            UpdateStatus("正在导入叠加结果");
            ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(CJDCQGXGC)) as IDataset).Delete();
            IFeatureClass pSourClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart);
            IFeatureClass pTarClass = null;
            try
            {
                pTarClass = pFWor.OpenFeatureClass("CJDCQGXGC");
            }
            catch
            {
                UpdateStatus("数据库没有升级，请先升级数据库。");
                return;
            }

            string where = "";
            if (xzdm.Length > 0)
                where = "ZLDWDM LIKE '" + xzdm + "%'";
            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pCJDCQClass, "BSM", where) + 1;
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pGXFeatureclass, "BSM", where) + 1;
            long maxBSM = 0;
            if (max1 > max2)
                maxBSM = max1;
            else
                maxBSM = max2;
            ITable pTable = pTarClass as ITable;
            pTable.DeleteSearchedRows(null);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pSearchCursor = pSourClass.Search(null, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeatureCursor pInsertCursor = pTarClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    if (pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString().Trim() == pFeature.get_Value(pFeature.Fields.FindField("BSM_1")).ToString().Trim() || pFeature.get_Value(pFeature.Fields.FindField("BZ_1")).ToString().Trim() == "S")
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    else
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pSourClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pSourClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pSourClass.FindField("BSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ_1")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pSourClass.FindField("BZ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC_1")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsertCursor.InsertFeature(pFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            UpdateStatus("正在提取变更前同名的村级调查区");
            ArrayList xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGQZLDWDM");
            List<string> xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("ZLDWDM = '" + item.ToString() + "'");
            }
            string sWhere = string.Join(" Or ", xzqdm);
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pCJDCQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pCJDCQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");


                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pCJDCQClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pCJDCQClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    //RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGQZLDWDM = '" + pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")) + "'");
                }
                pInsert.Flush();
            }
            UpdateStatus("正在提取变更后同名的村级调查区");
            xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGHZLDWDM");
            xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("ZLDWDM = '" + item.ToString() + "'");
            }
            sWhere = string.Join(" Or ", xzqdm);
            pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pCJDCQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pCJDCQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);


                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pCJDCQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pCJDCQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    //RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGHZLDWDM = '" + pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")) + "'");
                }
                pInsert.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSourClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTarClass);

            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureclass);
            currWs.ExecuteSQL("update cjdcqgx set bz=''");
            currWs.ExecuteSQL("update cjdcqgxgc set bghbz=''");
            //currWs.ExecuteSQL("delete from cjdcqgxgc where BGQBSM is null or BGHBSM is null or BGQBSM='' or BGHBSM=''");
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\CJDCQGXGC");

            JSMJCJDCQ("");

            CJDCQJX1();

            currWs.ExecuteSQL("update cjdcqgxgc set bgxw='3' where bgqzldwdm='' or bgqzldwdm is null");
            currWs.ExecuteSQL("update cjdcqgxgc set bgxw='0' where bghzldwdm like '000000%'");
            currWs.ExecuteSQL("update cjdcqgxgc set BGHBSM='',BGHZLDWDM='',BGHZLDWMC='',BGHDCMJ=0,BGHBZ='',BGHMSSM='',BGHHDMC='' where bghzldwdm not like '" + xzdm + "%'");


            UpdateStatus("村级调查区更新过程层提取完毕");
        }

        public void CJDCQJX1()
        {
            //0（灭失）、3（新增）、4（无变化）
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pCJDCQJXGX = pFeaWorkspace.OpenFeatureClass("CJDCQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            (pCJDCQJXGX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取村级调查区界线");
            List<string> IDwbh = new List<string>();
            string cjdcqjcgxYSDM = GetValueFromMDBByLayerName("CJDCQJXGX");
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = DeleteAndNewTmpGDB();

            //IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("CJDCQGX");
            //IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);

            bool updata = RCIS.GISCommon.GpToolHelper.Update(filePath + "\\TDDC\\CJDCQ", filePath + "\\TDGX\\CJDCQGX", tmpWS.PathName + "\\cjdcqnmk");

            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "xzqdm not like '" + xzdm + "%'";
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(RCIS.Global.GlobalEditObject.GlobalWorkspace, tmpWS, "XZQGX", "XZQOut", pQf);
            //bool b1 = RCIS.GISCommon.GpToolHelper.Erase_analysis(tmpWS.PathName + "\\cjdcqNew", tmpWS.PathName + "\\XZQOut", tmpWS.PathName + "\\cjdcqnmk");
            bool b1 = RCIS.GISCommon.GpToolHelper.Erase_analysis(filePath + "\\TDGX\\CJDCQGX", tmpWS.PathName + "\\XZQOut", tmpWS.PathName + "\\cjdcqNew");
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry((tmpWS as IFeatureWorkspace).OpenFeatureClass("cjdcqNew"));


            IFeatureClass pGXJX = GPPolygonToLineByFeatureclass(tmpWS.PathName + "\\cjdcqnmk", tmpWS);
            IFeatureClass pCJDCQJX = pFeaWorkspace.OpenFeatureClass("CJDCQJX");

            //县代码  排标识码

            string where = "";
            if (xzdm.Length > 0)
                where = "BSM LIKE '" + xzdm + "%'";

            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pCJDCQJX, "BSM", where) + 1;

            if (maxBSM.ToString().Length < 18)
            {
                Dictionary<string, string> dicDm = getCDM();
                string cdm = "0000";
                if (dicDm.ContainsKey("CJDCQJXGX"))
                {
                    cdm = dicDm["CJDCQJXGX"];
                }
                else
                {
                    if (dicDm.ContainsKey("CJDCQJX"))
                    {
                        cdm = dicDm["CJDCQJX"];
                    }
                }
                string bsm = xzdm + cdm + maxBSM.ToString().PadLeft(8, '0');
                maxBSM = long.Parse(bsm);
            }

            ISpatialFilter pSpatialFil = new SpatialFilterClass();
            pSpatialFil.Geometry = pGeo;
            pSpatialFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pGXJXCursor = pGXJX.Search(pSpatialFil, true);
            IFeature pFeature;

            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pCJDCQJX;
            IIdentify pIdentify = pLayer as IIdentify;
            IFeatureCursor pInsert = pCJDCQJXGX.Insert(true);
            List<int> arrWBH = new List<int>();
            List<int> arr = new List<int>();
            while ((pFeature = pGXJXCursor.NextFeature()) != null)
            {
                if (!GetIntersectLength(pFeature, pGeo))
                    continue;
                bool b = false;
                List<IFeature> pList0 = new List<IFeature>();
                IArray pArray = pIdentify.Identify(pFeature.ShapeCopy);
                if (pArray != null)
                {
                    IRelationalOperator pRel = (pFeature.ShapeCopy) as IRelationalOperator;
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pFea = pRow.Row as IFeature;
                        if (pRel.Equals(pFea.ShapeCopy))
                        {
                            IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pFea.get_Value(pFea.Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pFea.get_Value(pFea.Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pFea.get_Value(pFea.Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "4");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pFea.OID);
                            b = true;
                            break;
                        }
                        else
                        {
                            if (GetIntersectLen(pFea, pFeature))
                                pList0.Add(pFea);
                        }
                    }
                }
                if (!b)
                {
                    IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                    pFeaBuffer.Shape = pFeature.ShapeCopy;
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), maxBSM++);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), "660200");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), "600001");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "3");
                    pInsert.InsertFeature(pFeaBuffer);
                    for (int i = 0; i < pList0.Count; i++)
                    {
                        if (!arr.Contains(pList0[i].OID))
                        {
                            pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pList0[i].ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pList0[i].get_Value(pList0[i].Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pList0[i].get_Value(pList0[i].Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pList0[i].get_Value(pList0[i].Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pList0[i].OID);
                        }
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);

            }

            //bool bb=RCIS.GISCommon.GpToolHelper.Erase_analysis(filePath + "\\TDDC\\CJDCQJX", filePath + "\\TDGX\\CJDCQJXGX", tmpWS.PathName + "\\JX0");
            //IFeatureClass pJX0 = (tmpWS as IFeatureWorkspace).OpenFeatureClass("JX0");
            IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("CJDCQGX");
            pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);

            ISpatialFilter pSf = new SpatialFilterClass();
            pSf.Geometry = pGeo;
            pSf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pSf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            IFeatureCursor pCursor = pCJDCQJX.Search(pSf, true);
            IFeature pJXFea;
            while ((pJXFea = pCursor.NextFeature()) != null)
            {
                if (arr.Contains(pJXFea.OID)||!GetIntersectLen(pJXFea,pGeo))
                    continue;
                IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                pFeaBuffer.Shape = pJXFea.ShapeCopy;
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pJXFea.get_Value(pJXFea.Fields.FindField("BSM")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pJXFea.get_Value(pJXFea.Fields.FindField("JXLX")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pJXFea.get_Value(pJXFea.Fields.FindField("JXXZ")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                pInsert.InsertFeature(pFeaBuffer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pJXFea);

            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

            RCIS.Utility.OtherHelper.ReleaseComObject(pGXJXCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\CJDCQJXGX");

        }

        public bool GetIntersectLength(IFeature pFeature, IGeometry pGeo)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pGeo, esriGeometryDimension.esriGeometry1Dimension);
            if (pGeoIntersect != null)
            {
                IPolyline pLine = pGeoIntersect as IPolyline;
                if (pLine.Length > 0.0001)
                {
                    b = true;
                }
            }
            return b;
        }

        public bool GetIntersectLength(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
            if (pGeoIntersect != null)
            {
                IPolyline pLine = pGeoIntersect as IPolyline;
                if (pLine.Length > 0.0001)
                {
                    b = true;
                }
            }
            return b;
        }

        public IFeatureClass GPPolygonToLineByFeatureclass(string inputFile, IWorkspace pTmpWS)
        {
            IFeatureClass pFeaClass = null;
            //string name = "sdsdf";
            string name = Guid.NewGuid().ToString().Replace("-", "");
            name = "a" + name.Substring(0, 10);
            //name = "xzqgxjx";
            string temp = pTmpWS.PathName + "\\" + name;

            ESRI.ArcGIS.DataManagementTools.PolygonToLine PolygonToLine = new ESRI.ArcGIS.DataManagementTools.PolygonToLine();
            PolygonToLine.in_features = inputFile;
            PolygonToLine.neighbor_option = "IDENTIFY_NEIGHBORS";
            PolygonToLine.out_feature_class = temp;
            Geoprocessor geoProcessor = new Geoprocessor();
            try
            {
                geoProcessor.Execute(PolygonToLine, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return pFeaClass;
            }
            pFeaClass = (pTmpWS as IFeatureWorkspace).OpenFeatureClass(name);
            return pFeaClass;
        }

        public bool GetIntersectArea(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            if (pGeoIntersect != null)
            {
                IArea pArea = pGeoIntersect as IArea;
                if (pArea.Area > 0.0001)
                {
                    b = true;
                }
            }
            return b;
        }

        public void JSMJCJDCQ(string awm)
        {
            //数据汇总流程
            //1.计算变更后行政区，村级调查区的图形范围
            //2.计算变更后行政区，村级调查区中变更图斑的面积
            //3.计算变更后行政区，村级调查区中三调图斑面积并汇总
            UpdateStatus("正在计算村级调查区更新过程层面积");

            IWorkspace pTmpWs = DeleteAndNewTmpGDB();
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWs, "DLTBGXGC", "TBGXGC", null);
            IFeatureClass pTBGXGC = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TBGXGC");
            ISchemaLock pSchemaLock = null;
            pSchemaLock = pTBGXGC as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = pTBGXGC as IClassSchemaEdit4;
            pClassSchemaEdit.AlterFieldName("TBBGMJ", "TBMJ");
            pClassSchemaEdit.AlterFieldName("BGHZLDWDM", "ZLDWDM");
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

            bool b = RCIS.GISCommon.GpToolHelper.Update(currWs.PathName + "\\TDDC\\DLTB", pTmpWs.PathName + "\\TBGXGC", pTmpWs.PathName + "\\TB");
            if (!b)
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            IFeatureClass pTB = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TB");


            IFeatureClass pCJDCQGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGXGC");

            Dictionary<string, double> dm_mj = new Dictionary<string, double>();

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
                    IFeature pCJDCQGXGC;
                    while ((pCJDCQGXGC = pCJDCQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGQZLDWDM")).ToString();
                        string bghdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGHZLDWDM")).ToString();
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pCJDCQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pTB.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "ZLDWDM = '" + bghdwdm + "'";
                            IFeatureCursor pDLTBGXGCCursor = pTB.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pDLTBGXGCCursor);
                            IFeature pDLTBGXGC;
                            while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                            {
                                b = GetIntersectArea(pDLTBGXGC, pCJDCQGXGC);
                                if (b == true)
                                {
                                    double TBMJ = double.Parse(pDLTBGXGC.get_Value(pTB.FindField("TBMJ")).ToString());
                                    BGMJ += TBMJ;
                                    DCMJ += TBMJ;
                                }
                                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGC);
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCCursor);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                        }
                        pCJDCQGXGC.set_Value(pCJDCQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pCJDCQGXGCCursor.UpdateFeature(pCJDCQGXGC);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGC);
                    }
                    pCJDCQGXGCCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCCursor);
                }
                dm_mj.Add(dwdm, DCMJ);
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pCJDCQGXGCClass as ITable, "BGHDCMJ", Math.Round(dm_mj[dwdm], 2), "BGHZLDWDM = '" + dwdm + "'");
            }
            UpdateStatus("正在计算村级调查区更新层面积");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            IFeatureClass pCJDCQGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
            IFeatureCursor pCursor = pCJDCQGXClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("ZLDWDM")).ToString();
                if (currDh == 0)
                {
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号
                }
                double JSMJ = area.SphereArea(pFea.ShapeCopy, currDh);
                pFea.set_Value(pFea.Fields.FindField("JSMJ"), Math.Round(JSMJ, 2));
                pFea.set_Value(pFea.Fields.FindField("DCMJ"), Math.Round(dm_mj[zldwdm], 2));
                pFea.set_Value(pFea.Fields.FindField("GXSJ"), bgData);
                pCursor.UpdateFeature(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTB);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTBGXGC);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCClass);
            //UpdateStatus("村级调查区面积计算完成");
        }

        public void ProduceDLTBGXGC(double tzMj,bool isAdjust)
        {
            UpdateStatus("正在提取DLTB内的变化图斑。");

            //UpdateStatus("开始生成更新过程图层");
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pDLTB = null;
            try
            {
                pDLTB = pFeatureWorkspace.OpenFeatureClass("DLTB");
                if (pDLTB.FeatureCount(null) == 0)
                {
                    MessageBox.Show("地类图斑层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到地类图斑层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IFeatureClass pGXGCFeatureCLass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            IFeatureClass pGXFeatureClass = null;
            try
            {
                pGXFeatureClass = pFeatureWorkspace.OpenFeatureClass("DLTBGX");
                if (pGXFeatureClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("地类图斑更新层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到地类图斑更新层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string where = "";
            if (xzdm.Length > 0)
                where = "ZLDWDM LIKE '" + xzdm + "%'";


            long maxNum;
            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pGXFeatureClass, "BSM", where);
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTB, "BSM", where);
            if (max1 > max2)
                maxNum = max1 + 1;
            else
                maxNum = max2 + 1;
            //删除更新过程层数据
            bool del = RCIS.GISCommon.GpToolHelper.DeleteFeatures(currWs.PathName + "\\TDGX\\DLTBGXGC");
            if (!del)
            {
                MessageBox.Show("数据删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //ITable pTable = pGXGCFeatureCLass as ITable;
            //pTable.DeleteSearchedRows(null);
            //获取变化图斑数据
            //IFeatureClass pBHFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txt);
            //提取出三调地类图斑内与变化图斑相交的图斑   保存到临时tmp内  
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = DeleteAndNewTmpGDB();

            //为更新过程层添加变更前后图斑编号字段
            IFeatureClass pGXGC = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            if (pGXGC.FindField("BGQTBBH") == -1)
            {
                IField pField = EsriDatabaseHelper.CreateTextField("BGQTBBH", "变更前图斑编号", 8);
                IClass pClass = pGXGC as IClass;
                pClass.AddField(pField);
                //pClass.AddIndex(
            }
            if (pGXGC.FindField("BGHTBBH") == -1)
            {
                IField pField = EsriDatabaseHelper.CreateTextField("BGHTBBH", "变更后图斑编号", 8);
                IClass pClass = pGXGC as IClass;
                pClass.AddField(pField);
            }

            bool union = RCIS.GISCommon.GpToolHelper.Union_analysis(currWs.PathName + "\\TDDC\\DLTB"+";"+currWs.PathName + "\\TDGX\\DLTBGX", tmpWS.PathName + "\\dltbUnion");
            ITable pUnionTab = (tmpWS as IFeatureWorkspace).OpenTable("dltbUnion");
            IQueryFilter pFil=new QueryFilterClass();
            pFil.WhereClause="FID_DLTBGX=-1";
            pUnionTab.DeleteSearchedRows(pFil);

            bool spatialJoin = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(currWs.PathName + "\\TDDC\\DLTB", tmpWS.PathName + "\\dltbUnion", tmpWS.PathName + "\\sdtb", "KEEP_COMMON", "contains");
            //bool spatialJoin = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(currWs.PathName + "\\TDDC\\DLTB", currWs.PathName + "\\TDGX\\DLTBGX", tmpWS.PathName + "\\sdtb");
            if (!spatialJoin)
            {
                UpdateStatus("查找三调图斑失败");
                return;
            }
            tmpWS.ExecuteSQL("delete from sdtb where Join_Count=0");
            IFeatureClass pSDFeatureClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("sdtb");
            for (int i = pSDFeatureClass.Fields.FieldCount - 1; i >= 0; i--)
            {
                IField pField = pSDFeatureClass.Fields.get_Field(i);
                if (pField.Name.Contains("_1"))
                    (pSDFeatureClass as ITable).DeleteField(pField);
            }

            //将变化图斑与三调图斑尽行融合 存到DLTBGXGC内
            UpdateStatus("正在生成地类图斑更新过程层数据。");

            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
            string gxPath = currWs.PathName + "\\TDGX\\DLTBGX";
            tmp += "\\sdtb";
            pUnion.in_features = tmp + ";" + gxPath;
            pUnion.out_feature_class = Application.StartupPath + "\\tmp\\tmp.gdb\\rh";
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                string mess = "";
                for (int i = 0; i < gp.MessageCount; i++)
                {
                    mess += gp.GetMessage(i) + "\r\n";
                }
                MessageBox.Show(mess.ToString());
                UpdateStatus("叠加分析错误");
                return;
            }

            bool b = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(tmpWS.PathName + "\\rh", tmpWS.PathName + "\\multipartRH");


            //
            //循环DLTBGXGC图层进行属性赋值
            UpdateStatus("正在为地类图斑更新过程层数据添加属性。");
            IFeatureClass pRHFeatureclass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("multipartRH");
            ISchemaLock pSchemaLock = null;
            pSchemaLock = pRHFeatureclass as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = pRHFeatureclass as IClassSchemaEdit4;

            pClassSchemaEdit.AlterFieldName("BSM", "BGQTBBSM");
            pClassSchemaEdit.AlterFieldName("TBBH", "BGQTBBH");
            pClassSchemaEdit.AlterFieldName("DLBM", "BGQDLBM");
            pClassSchemaEdit.AlterFieldName("DLMC", "BGQDLMC");
            pClassSchemaEdit.AlterFieldName("QSXZ", "BGQQSXZ");
            pClassSchemaEdit.AlterFieldName("QSDWDM", "BGQQSDWDM");
            pClassSchemaEdit.AlterFieldName("QSDWMC", "BGQQSDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWMC", "BGQZLDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWDM", "BGQZLDWDM");
            pClassSchemaEdit.AlterFieldName("KCDLBM", "BGQKCDLBM");
            pClassSchemaEdit.AlterFieldName("KCXS", "BGQKCXS");
            pClassSchemaEdit.AlterFieldName("KCMJ", "BGQKCMJ");
            pClassSchemaEdit.AlterFieldName("TBDLMJ", "BGQTBDLMJ");
            pClassSchemaEdit.AlterFieldName("GDLX", "BGQGDLX");
            pClassSchemaEdit.AlterFieldName("GDPDJB", "BGQGDPDJB");
            pClassSchemaEdit.AlterFieldName("XZDWKD", "BGQXZDWKD");
            pClassSchemaEdit.AlterFieldName("TBXHDM", "BGQTBXHDM");
            pClassSchemaEdit.AlterFieldName("TBXHMC", "BGQTBXHMC");
            pClassSchemaEdit.AlterFieldName("ZZSXDM", "BGQZZSXDM");
            pClassSchemaEdit.AlterFieldName("ZZSXMC", "BGQZZSXMC");
            pClassSchemaEdit.AlterFieldName("GDDB", "BGQGDDB");
            pClassSchemaEdit.AlterFieldName("FRDBS", "BGQFRDBS");
            pClassSchemaEdit.AlterFieldName("CZCSXM", "BGQCZCSXM");
            pClassSchemaEdit.AlterFieldName("MSSM", "BGQMSSM");
            pClassSchemaEdit.AlterFieldName("HDMC", "BGQHDMC");

            pClassSchemaEdit.AlterFieldName("BSM_1", "BGHTBBSM");
            pClassSchemaEdit.AlterFieldName("TBBH_1", "BGHTBBH");
            pClassSchemaEdit.AlterFieldName("DLBM_1", "BGHDLBM");
            pClassSchemaEdit.AlterFieldName("DLMC_1", "BGHDLMC");
            pClassSchemaEdit.AlterFieldName("QSXZ_1", "BGHQSXZ");
            pClassSchemaEdit.AlterFieldName("QSDWDM_1", "BGHQSDWDM");
            pClassSchemaEdit.AlterFieldName("QSDWMC_1", "BGHQSDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWMC_1", "BGHZLDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWDM_1", "BGHZLDWDM");
            pClassSchemaEdit.AlterFieldName("KCDLBM_1", "BGHKCDLBM");
            pClassSchemaEdit.AlterFieldName("KCXS_1", "BGHKCXS");
            pClassSchemaEdit.AlterFieldName("KCMJ_1", "BGHKCMJ");
            pClassSchemaEdit.AlterFieldName("TBDLMJ_1", "BGHTBDLMJ");
            pClassSchemaEdit.AlterFieldName("GDLX_1", "BGHGDLX");
            pClassSchemaEdit.AlterFieldName("GDPDJB_1", "BGHGDPDJB");
            pClassSchemaEdit.AlterFieldName("XZDWKD_1", "BGHXZDWKD");
            pClassSchemaEdit.AlterFieldName("TBXHDM_1", "BGHTBXHDM");
            pClassSchemaEdit.AlterFieldName("TBXHMC_1", "BGHTBXHMC");
            pClassSchemaEdit.AlterFieldName("ZZSXDM_1", "BGHZZSXDM");
            pClassSchemaEdit.AlterFieldName("ZZSXMC_1", "BGHZZSXMC");
            pClassSchemaEdit.AlterFieldName("GDDB_1", "BGHGDDB");
            pClassSchemaEdit.AlterFieldName("FRDBS_1", "BGHFRDBS");
            pClassSchemaEdit.AlterFieldName("CZCSXM_1", "BGHCZCSXM");
            pClassSchemaEdit.AlterFieldName("MSSM_1", "BGHMSSM");
            pClassSchemaEdit.AlterFieldName("HDMC_1", "BGHHDMC");

            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

            tmpWS.ExecuteSQL("delete from multipartRH where BGHTBBSM is null or BGHTBBSM=''");
            //tmpWS.ExecuteSQL("delete from multipartRH where  BGHTBBSM is null or BGHTBBSM=''");
            IField field = RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("BGXW", "变更行为", 1);
            pRHFeatureclass.AddField(field);

            tmpWS.ExecuteSQL("update multipartRH set bgxw='1' where bgqtbbsm=bghtbbsm");
            tmpWS.ExecuteSQL("update multipartRH set bgxw='2' where bgqtbbsm<>bghtbbsm");
            tmpWS.ExecuteSQL("update multipartRH set bgxw='1' where bz_1='S'");

            b = RCIS.GISCommon.GpToolHelper.Append(tmpWS.PathName + "\\multipartRH", currWs.PathName + "\\TDGX\\DLTBGXGC");
            b = RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGXGC");

            currWs.ExecuteSQL("update dltbgx set BZ=''");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx=''");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='3' where bgqtbbsm is null or bgqtbbsm=''");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='4' where bghzldwdm like '000000%'");

            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='0' where substring(BGQZLDWDM,1,6)= '" + xzdm + "' and substring(BGHZLDWDM,1,6) = '" + xzdm + "' and xzqtzlx<>'3'");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='1' where substring(BGQZLDWDM,1,6)<> '" + xzdm + "' and substring(BGHZLDWDM,1,6) = '" + xzdm + "'  and xzqtzlx<>'3'");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='2' where substring(BGQZLDWDM,1,6)= '" + xzdm + "' and substring(BGHZLDWDM,1,6) <> '" + xzdm + "'  and xzqtzlx<>'3'  and xzqtzlx<>'4'");


            currWs.ExecuteSQL("update dltbgxgc set bgxw='0' where xzqtzlx='4'");
            currWs.ExecuteSQL("update dltbgxgc set bgxw='3' where xzqtzlx='3'");

            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTB);
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureClass);

            //计算变更面积

            //JSMJ(tmpWS);
            MJTP(pGXGCFeatureCLass, maxNum,tzMj,isAdjust);
            JSMJGX(tmpWS);

            UpdateStatus("地类图斑更新过程层提取完毕。");

        }

        public void JSMJGX(IWorkspace pTmpWS)
        {
            UpdateStatus("正在计算地类图斑更新层面积");
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            string filePath = currWs.PathName;
            ESRI.ArcGIS.AnalysisTools.Statistics pStatistics = new ESRI.ArcGIS.AnalysisTools.Statistics();
            pStatistics.in_table = filePath + @"\TDGX\DLTBGXGC";
            pStatistics.out_table = pTmpWS.PathName + @"\DLTBGXTMP";
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
            List<string> bsms = new List<string>();
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            ITable pTable = (pTmpWS as IFeatureWorkspace).OpenTable("DLTBGXTMP");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pTable, "BGHTBBSM");
            //IFeatureClass pGXGCClass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            IFeatureClass pGXClass = pFeatureWorkspace.OpenFeatureClass("DLTBGX");
            //IFeatureClass pDLTBClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            //RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBClass, "BSM");
            IFeatureCursor pFeatureCursor = pGXClass.Update(null, true);
            IFeature pFeature;
            while ((pFeature = pFeatureCursor.NextFeature()) != null)
            {
                string bsm = pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString();
                using (ESRI.ArcGIS.ADF.ComReleaser comRel21 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHTBBSM = '" + bsm + "'";
                    ICursor pCursor = pTable.Search(pQueryFilter, true);
                    comRel21.ManageLifetime(pCursor);
                    IRow pRow = pCursor.NextRow();
                    if (pRow == null)
                        continue;
                    double sumkcmj = 0;
                    double.TryParse(pRow.get_Value(pTable.FindField("SUM_BGHKCMJ")).ToString(), out sumkcmj);
                    double sumtbdlmj = 0;
                    double.TryParse(pRow.get_Value(pTable.FindField("SUM_BGHTBDLMJ")).ToString(), out sumtbdlmj);

                    double tbmj = Math.Round(sumkcmj + sumtbdlmj, 2);
                    pFeature.set_Value(pGXClass.FindField("TBMJ"), tbmj);
                    double kcxs = 0;
                    double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("KCXS")).ToString(), out kcxs);
                    double kcmj = Math.Round(tbmj * kcxs, 2);
                    pFeature.set_Value(pGXClass.FindField("KCMJ"), kcmj);
                    pFeature.set_Value(pGXClass.FindField("TBDLMJ"), Math.Round(tbmj - kcmj, 2));
                    pFeatureCursor.UpdateFeature(pFeature);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                }
            }

            RCIS.Utility.OtherHelper.ReleaseComObject(pGXClass);
            (pTable as IDataset).Delete();
            //UpdateStatus("地类图斑更新层变更面积计算完成。");
        }

        public void MJTP(IFeatureClass gxgcClass, long maxBsm,double mjDiff,bool isTZ)
        {
            //对所有更新过程数据，按照更新时间进行分组
            //ArrayList BGHTBBSM = FeatureHelper.GetUniqueFieldValueByDataStatistics(gxgcClass, null, "BGHTBBSM");
            UpdateStatus("正在重排地类图斑更新过程层标识码。");
            List<string> bgqBsms = new List<string>();
            List<double> bgqZmj = new List<double>();
            //找到该更新时间 对应所有数据，
            IFeatureCursor cursor = gxgcClass.Update(null, false);
            IFeature aFea = null;
            try
            {
                while ((aFea = cursor.NextFeature()) != null)
                {
                    //首先计算其变更后tb面积，重新计算
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (aFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    int currDh = (int)(X / 1000000);////WK---带号  
                    SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                    double tbmj = area.SphereArea(aFea.ShapeCopy, currDh);
                    if (tbmj <= 0.1)
                    {
                        string bgqbsm = aFea.get_Value(aFea.Fields.FindField("BGQTBBSM")).ToString();
                        string bghbsm = aFea.get_Value(aFea.Fields.FindField("BGHTBBSM")).ToString();
                        ISpatialFilter pSF = new SpatialFilterClass();
                        bool del = false;
                        pSF.Geometry = aFea.ShapeCopy;
                        pSF.GeometryField = gxgcClass.ShapeFieldName;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
                        pSF.WhereClause = "BGHTBBSM='" + bghbsm + "'";
                        // and BGHTBBSM='" + bghbsm + "'
                        IFeatureCursor pCursor = gxgcClass.Update(pSF, true);
                        IFeature pFeature;
                        while ((pFeature = pCursor.NextFeature()) != null)
                        {
                            if (GetIntersectLen(aFea, pFeature))
                            {
                                IPolygon pPoylon = UnionPolygon(aFea.ShapeCopy, pFeature.ShapeCopy);
                                pFeature.Shape = pPoylon;
                                double qmmj = area.SphereArea(pPoylon, currDh);
                                pFeature.set_Value(aFea.Fields.FindField("TBBGMJ"), qmmj);
                                aFea.Delete();
                                pCursor.UpdateFeature(pFeature);
                                pCursor.Flush();
                                del = true;
                                break;
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                        }
                        if (!del)
                        {
                            aFea.Delete();
                            //cursor.UpdateFeature(pFeature);
                        }

                        RCIS.Utility.OtherHelper.ReleaseComObject(pSF);
                        RCIS.Utility.OtherHelper.ReleaseComObject(aFea);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                        continue;
                    }
                    aFea.set_Value(aFea.Fields.FindField("TBBGMJ"), tbmj);
                    aFea.set_Value(aFea.Fields.FindField("BSM"), maxBsm++);
                    aFea.set_Value(aFea.Fields.FindField("GXSJ"), bgData);
                    cursor.UpdateFeature(aFea);
                    //计算变更前总面积
                    string bsm = FeatureHelper.GetFeatureStringValue(aFea, "BGQTBBSM");
                    if (!bgqBsms.Contains(bsm))
                    {
                        bgqBsms.Add(bsm);
                        double bgqtbmj = FeatureHelper.GetFeatureDoubleValue(aFea, "BGQTBDLMJ") + FeatureHelper.GetFeatureDoubleValue(aFea, "BGQKCMJ");
                        bgqZmj.Add(bgqtbmj);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(aFea);
                }
                cursor.Flush();
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
            //currWs.ExecuteSQL("delete from dltbgxgx where shape_area<0.01");

            UpdateStatus("正在计算地类图斑更新过程层面积。");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(gxgcClass, "BGQTBBSM");
            for (int kk = 0; kk < bgqBsms.Count; kk++)
            {
                double abgqzmj = bgqZmj[kk];
                string absm = bgqBsms[kk];

                //计算变更后图斑面积和
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "BGQTBBSM ='" + absm + "' ";
                double bghZmj = 0;

                ArrayList BghFeatures = new ArrayList();
                IFeatureCursor cur = gxgcClass.Search(pQF, false);
                IFeature abghfea = null;
                try
                {
                    while ((abghfea = cur.NextFeature()) != null)
                    {
                        BghFeatures.Add(abghfea);
                        bghZmj += FeatureHelper.GetFeatureDoubleValue(abghfea, "TBBGMJ");
                        //RCIS.Utility.OtherHelper.ReleaseComObject(abghfea);
                    }
                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cur);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                }
                //2021年10月18日10:10:59添加，计算行政区调整类型为3 的调入数据面积（未调平）
                if (string.IsNullOrWhiteSpace(absm.ToString()))
                {
                    for (int i = 0; i < BghFeatures.Count; i++)
                    {
                        IFeature aTPFea = BghFeatures[i] as IFeature;
                        string bgmj=aTPFea.get_Value(aTPFea.Fields.FindField("TBBGMJ")).ToString();
                        double tbmj = MathHelper.RoundEx(double.Parse(bgmj), 2);
                        //double bgqkcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGQKCXS");
                        //double bgqkcmj = MathHelper.RoundEx(tbmj * bgqkcxs, 2);
                        //aTPFea.set_Value(aTPFea.Fields.FindField("BGQTBDLMJ"), MathHelper.RoundEx(tbmj - bgqkcmj, 2));
                        //aTPFea.set_Value(aTPFea.Fields.FindField("BGQKCMJ"), MathHelper.RoundEx(bgqkcmj, 2));
                        double kcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGHKCXS");
                        if (kcxs > 0)
                        {
                            double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHKCMJ"), kcmj);
                            double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), dlmj);
                        }
                        else
                        {
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), tbmj);
                        }
                        aTPFea.Store();
                        RCIS.Utility.OtherHelper.ReleaseComObject(aTPFea);
                    }
                }
                else
                {
                    if (BghFeatures.Count == 1)
                    {
                        IFeature aTPFea = BghFeatures[0] as IFeature;
                        aTPFea.set_Value(aTPFea.Fields.FindField("TBBGMJ"), abgqzmj);
                        double tbmj = MathHelper.RoundEx(abgqzmj, 2);
                        double bgqkcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGQKCXS");
                        double bgqkcmj = MathHelper.RoundEx(tbmj * bgqkcxs, 2);
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGQTBDLMJ"), MathHelper.RoundEx(tbmj - bgqkcmj, 2));
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGQKCMJ"), MathHelper.RoundEx(bgqkcmj, 2));
                        double kcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGHKCXS");
                        if (kcxs > 0)
                        {
                            double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHKCMJ"), kcmj);
                            double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), dlmj);
                        }
                        else
                        {
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), tbmj);
                        }
                        aTPFea.Store();
                        RCIS.Utility.OtherHelper.ReleaseComObject(aTPFea);
                    }

                    else
                    {
                        //变更后 按照图斑面积排序
                        IComparer comparer = new TBBGMJ();
                        BghFeatures.Sort(comparer);
                        //调平
                        bool zf = true;
                        double diff = MathHelper.RoundEx(abgqzmj - bghZmj, 2);
                        //if (Math.Abs(diff) < 0.0001) continue;
                        if (diff < 0)
                        {
                            zf = false;
                        }
                        diff = Math.Abs(diff);
                        int iNum = (int)(diff / 0.01);
                        int shang = iNum / BghFeatures.Count;
                        int yushu = iNum % BghFeatures.Count;
                        for (int i = 0; i < BghFeatures.Count; i++)
                        {
                            IFeature aTPFea = BghFeatures[i] as IFeature;

                            double bghtbmj = FeatureHelper.GetFeatureDoubleValue(aTPFea, "TBBGMJ");
                            if (Math.Abs(diff) > 0.0001)
                            {
                                if (i < yushu)
                                {
                                    bghtbmj += (zf ? 0.01 * (shang + 1) : -0.01 * (shang + 1));
                                }
                                else
                                {
                                    bghtbmj += (zf ? 0.01 * shang : -0.01 * shang);
                                }
                            }
                            double tbmj = MathHelper.RoundEx(bghtbmj, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("TBBGMJ"), tbmj);
                            double bgqkcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGQKCXS");
                            double bgqkcmj = MathHelper.RoundEx(tbmj * bgqkcxs, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGQTBDLMJ"), MathHelper.RoundEx(tbmj - bgqkcmj, 2));
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGQKCMJ"), MathHelper.RoundEx(bgqkcmj, 2));
                            double kcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGHKCXS");
                            if (kcxs > 0)
                            {
                                double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                                aTPFea.set_Value(aTPFea.Fields.FindField("BGHKCMJ"), kcmj);
                                double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                                aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), dlmj);
                            }
                            else
                            {
                                aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), tbmj);
                            }
                            aTPFea.Store();
                            RCIS.Utility.OtherHelper.ReleaseComObject(aTPFea);
                        }

                    }
                }
                //RCIS.Utility.OtherHelper.ReleaseComObject(BghFeatures);
            }
            RCIS.GISCommon.DatabaseHelper.DeleteIndex(gxgcClass, "BGQTBBSM");
            //2021年10月19日09:52:33若有行政区调入调出，找出行政区调整类型为3的数据进行控制面积平差，其他不进行平差
            ArrayList arr= RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(gxgcClass, null, "XZQTZLX");
            if (isTZ&&arr.Contains("3"))
            {
                //mjDiff是调整后的控制面积减去调整前的控制面积，用mjDiff加上调出就是调入的控制面积，再减去XZQTZLX为1的就是XZQTZLX为3的控制面积
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "XZQTZLX='2' OR XZQTZLX='4'";
                double mjOut = RCIS.GISCommon.FeatureHelper.StatsFieldSumValue(gxgcClass, pQf, "TBBGMJ");
                mjOut = MathHelper.RoundEx(mjOut, 2);
                pQf.WhereClause = "XZQTZLX='1'";
                double mjIn1 = RCIS.GISCommon.FeatureHelper.StatsFieldSumValue(gxgcClass, pQf, "TBBGMJ");
                mjIn1 = MathHelper.RoundEx(mjIn1, 2);
                pQf.WhereClause = "XZQTZLX='3'";
                double mjIn3 = RCIS.GISCommon.FeatureHelper.StatsFieldSumValue(gxgcClass, pQf, "TBBGMJ");
                mjIn3 = MathHelper.RoundEx(mjIn3, 2);
                //mjDiff = MathHelper.RoundEx(Math.Abs(mjDiff), 2);
                mjDiff = mjDiff + mjOut - mjIn1;
                if (mjDiff != mjIn3)
                {
                    ArrayList Features = RCIS.GISCommon.GetFeaturesHelper.getFeaturesBySql(gxgcClass, pQf);
                    IComparer comparer = new TBBGMJ();
                    Features.Sort(comparer);
                    //调平
                    double tpVal = 0.01;
                    double diff = MathHelper.RoundEx(mjDiff - mjIn3, 2);
                    //if (Math.Abs(diff) < 0.0001) continue;
                    if (diff < 0)
                    {
                        tpVal = -0.01;
                    }
                    diff = Math.Abs(diff);
                    int iNum = (int)(diff / 0.01);
                    int shang = iNum / Features.Count;
                    int yushu = iNum % Features.Count;
                    for (int i = 0; i < Features.Count; i++)
                    {
                        double currVal = tpVal * shang;
                        if (yushu > i)
                            currVal += tpVal;
                        IFeature pFea = Features[i] as IFeature;
                        double bgmj = double.Parse(pFea.get_Value(pFea.Fields.FindField("TBBGMJ")).ToString());
                        double tbmj= bgmj + currVal;
                        if (tbmj != bgmj)
                        {
                            pFea.set_Value(pFea.Fields.FindField("TBBGMJ"),tbmj);
                            double kcxs = 0;
                            double.TryParse(pFea.get_Value(pFea.Fields.FindField("BGHKCXS")).ToString(),out kcxs);
                            double kcmj = MathHelper.RoundEx(tbmj*kcxs,2);
                            double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                            pFea.set_Value(pFea.Fields.FindField("BGHTBDLMJ"), dlmj);
                            pFea.set_Value(pFea.Fields.FindField("BGHKCMJ"), kcmj);
                            pFea.Store();
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pFea);

                    }
                }
            }

            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGXGC");
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGX");

        }

        public bool GetIntersectLen(IFeature pFeature, IFeature pFea)
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

        public bool GetIntersectLen(IFeature pFeature, IGeometry pGeometry)
        {
            bool b = false;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
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

        public IPolygon UnionPolygon(IGeometry pGeo1, IGeometry pGeo2)
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

        public void rebuildTbbh()
        {
            UpdateStatus("正在重排地类图斑更新层图斑编号。");
            IFeatureClass pDltb = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            IFeatureClass pDltbGx = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            long iStartH = 1; //起始点号
            long.TryParse((FeatureHelper.GetMaxStringNumberEveryOne(pDltb, "TBBH") + 1).ToString(), out iStartH);
            if (pDltbGx == null)
            {
                return;
            }
            //IPolygon extent = RCIS.GISCommon.GeometryHelper.UnionPolygon(lstXzq);
            //找到所有地类图斑
            ArrayList arAllTbs = getAllTb(pDltbGx); //getAllTb(aXzqFea.ShapeCopy);

            //IComparer comparer = new tbbhCompare();
            //arAllTbs.Sort(comparer);

            IWorkspaceEdit pWSEdit = (pDltbGx as IDataset).Workspace as IWorkspaceEdit;
            pWSEdit.StartEditing(true);
            pWSEdit.StartEditOperation();
            try
            {
                foreach (IFeature aTb in arAllTbs)
                {
                    //string tbbh1 = xzqdm + (iStartH).ToString().PadLeft(6, '0');
                    //string tbbh = chkLmfw.Checked ? tbbhTxt.Text + iStartH.ToString() : iStartH.ToString();
                    //FeatureHelper.SetFeatureValue(aTb, "TBYBH", tbbh1);
                    RCIS.GISCommon.FeatureHelper.SetFeatureValue(aTb, "TBBH", iStartH.ToString());
                    aTb.Store();
                    iStartH++;
                }
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
            }
        }

        public ArrayList getAllTb(IFeatureClass dltbClass)
        {

            ArrayList arTbs = new ArrayList();
            IFeatureCursor pCursor = dltbClass.Search(null, false);
            IFeature aFeature = null;
            while ((aFeature = pCursor.NextFeature()) != null)
            {
                arTbs.Add(aFeature);
            }
            OtherHelper.ReleaseComObject(pCursor);
            return arTbs;

        }

        public void rebuildBsm()
        {
            Dictionary<string, string> dicDm = getCDM();
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureDataset pFeaDS = pFeaWs.OpenFeatureDataset("TDGX");
            List<IFeatureClass> allFC = RCIS.GISCommon.DatabaseHelper.getAllFeatureClass(pFeaDS);
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;
                if (className.ToUpper().EndsWith("GXGC") || className == "XZQJXGX" || className == "CJDCQJXGX") continue;
                //string className = this.chkListFCs.CheckedItems[i].ToString();
                //className = RCIS.Utility.OtherHelper.GetLeftName(className);
                string classbase = className.Substring(0, className.Length - 2);
                string cdm = "0000";
                if (dicDm.ContainsKey(className.ToUpper()))
                {
                    cdm = dicDm[className.ToUpper()];
                }
                else
                {
                    if (dicDm.ContainsKey(classbase))
                    {
                        cdm = dicDm[className.ToUpper()];
                    }
                }
                //UpdateStatus("处理" + className + "中...");

                IWorkspaceEdit pWsEdit = this.currWs as IWorkspaceEdit;
                pWsEdit.StartEditing(false);
                pWsEdit.StartEditOperation();

                IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(className);
                if (classbase == "CZKFBJ")
                    classbase = "CSKFBJ";
                if (classbase.ToUpper() == "HAX")
                    continue;
                IFeatureClass pBC = null;

                try
                {
                    pBC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(classbase);
                }
                catch (Exception ex)
                {
                    UpdateStatus(className + "层处理失败");
                    continue;
                }
                string where = "";
                if (pBC.FindField("XZQDM") != -1)
                    where = "XZQDM like '" + xzdm + "%'";
                else if (pBC.FindField("ZLDWDM") != -1)
                    where = "ZLDWDM LIKE '" + xzdm + "%'";
                else if (pBC.FindField("CZCDM") != -1)
                    where = "CZCDM LIKE '" + xzdm + "%'";
                else
                    where = "BSM  LIKE '" + xzdm + "%'";
                string max = RCIS.GISCommon.FeatureHelper.GetMaxStringOrderBy(pBC, "BSM", where);
                long maxBSM = 0;
                if (max.Length > 8)
                {
                    maxBSM = long.Parse(max.Substring(10, 8));
                    cdm = max.Substring(0, 10);
                }
                else
                {
                    cdm = xzdm + cdm;
                }
                maxBSM++;
                //if (maxBSM.ToString().Length < 8)
                //{
                //    string bsm = txtXian.Text.Trim() + cdm + maxBSM.ToString().PadLeft(8, '0');
                //    long.TryParse(bsm, out maxBSM);
                //}
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pUpdateCursor = pFC.Update(null, true);
                    comRel.ManageLifetime(pUpdateCursor);
                    IFeature pFeature;
                    IFeatureLayer pLayer = new FeatureLayerClass();
                    pLayer.FeatureClass = pBC;
                    IIdentify pIdentify = pLayer as IIdentify;
                    if (classbase.ToUpper() == "DLTB" || classbase.ToUpper() == "XZQ" || classbase.ToUpper() == "CJDCQ")
                    {
                        while ((pFeature = pUpdateCursor.NextFeature()) != null)
                        {
                            pFeature.set_Value(pFeature.Fields.FindField("GXSJ"),bgData);
                            IRelationalOperator pRel = pFeature.ShapeCopy as IRelationalOperator;
                            bool b = false;
                            IArray pArry = pIdentify.Identify(pFeature.ShapeCopy);
                            if (pArry != null)
                            {
                                for (int m = 0; m < pArry.Count; m++)
                                {
                                    IFeatureIdentifyObj idObj = pArry.get_Element(m) as IFeatureIdentifyObj;
                                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                                    IFeature pFea = pRow.Row as IFeature;
                                    if (pRel.Equals(pFea.ShapeCopy))
                                    {
                                        b = true;

                                        string bgqbsm = pFea.get_Value(pFea.Fields.FindField("BSM")).ToString().Trim();
                                        if (bgqbsm.Substring(0, 6) == xzdm)
                                            pFeature.set_Value(pFeature.Fields.FindField("BSM"), bgqbsm);
                                        else
                                        {
                                            pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + maxBSM++.ToString().PadLeft(8, '0'));
                                            pFeature.set_Value(pFeature.Fields.FindField("BZ"), "S");
                                        }
                                        break;
                                    }
                                }
                            }
                            if (b == false)
                            {
                                pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + maxBSM++.ToString().PadLeft(8, '0'));
                                //maxBSM++;
                            }
                            pUpdateCursor.UpdateFeature(pFeature);
                        }
                    }
                    else
                    {
                        while ((pFeature = pUpdateCursor.NextFeature()) != null)
                        {
                            pFeature.set_Value(pFeature.Fields.FindField("BSM"), cdm + maxBSM++.ToString().PadLeft(8, '0'));

                            pUpdateCursor.UpdateFeature(pFeature);
                        }
                    }
                }

                pWsEdit.StopEditOperation();
                pWsEdit.StopEditing(true);
            }
        }

        public Dictionary<string, string> getCDM()
        {
            string sql = "select CLASSNAME,LayerDM from SYS_YSDM  where type in ('POINT','LINE','POLYGON')  ";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr["CLASSNAME"].ToString().Trim().ToUpper(), dr["LayerDM"].ToString().Trim());
            }
            return dic;

        }

        public void getXzqGx()
        {

            if (currWs == null) return;
            UpdateStatus("正在准备数据……");
            IWorkspace tmpWS = DeleteAndNewTmpGDB();
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
                Dictionary<string, string> cjdcqdmmc = GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "DLTBGX", "ZLDWDM", "ZLDWMC", ref sameDm);
                if (sameDm.Length > 0 && cjdcqdmmc == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
                Dictionary<string, string> cjdcqdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "CJDCQBHtmp", "ZLDWDM", "ZLDWMC", ref sameDm);
                if (sameDm.Length > 0 && cjdcqdmmc == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
                Dictionary<string, string> dmmcgx = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "CJDCQGX", "ZLDWDM", "ZLDWMC", ref sameDm);
                if (sameDm.Length > 0 && dmmcgx == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
                Dictionary<string, string> cjdcqdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "CJDCQGX", "ZLDWDM", "ZLDWMC", ref sameDm);
                if (sameDm.Length > 0 && cjdcqdmmc == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("GXSJ"), bgData);
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
            Dictionary<string, string> dmmc = GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "XZQ", "XZQDM", "XZQMC", ref sameDm);
            if (sameDm.Length > 0 && dmmc == null)
            {
                UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\CJDCQGX");

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
                Dictionary<string, string> XZQdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "XZQBHtmp", "XZQDM", "XZQMC", ref sameDm);
                if (sameDm.Length > 0 && XZQdmmc == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
                Dictionary<string, string> dmmcgx = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "XZQGX", "XZQDM", "XZQMC", ref sameDm);
                if (sameDm.Length > 0 && dmmcgx == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");

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
                Dictionary<string, string> XZQdmmc = GetDMMCDicByQueryDef(tmpWS as IFeatureWorkspace, "XZQGX", "XZQDM", "XZQMC", ref sameDm);
                if (sameDm.Length > 0 && XZQdmmc == null)
                {
                    UpdateStatus("坐落单位代码为" + sameDm + "存在重名。");
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
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField("GXSJ"), bgData);
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
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\XZQGX");

            if (newXZQ.Count == 0)
            {
                //MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private Dictionary<string, string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, string valueField, ref string erroDM)
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
                        erroDM += "(" + mc + "," + dmmc[dm] + ")";
                        return null;
                    }
                    dmmc.Add(dm, mc);
                }
            }
            return dmmc;
        }

        public string GetValueFromMDBByLayerName(string layerName)
        {
            string sql = "select YSDM from SYS_YSDM where CLASSNAME='" + layerName + "'";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            return dt.Rows[0][0].ToString();
        }

        public IWorkspace DeleteAndNewTmpGDB()
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

        public void UpdateStatus(string txt)
        {
            if (info != null)
            {
                info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            }
            Application.DoEvents();
        }

        //导出vct

        int currDh;
        //记录 类名对应的中文名  要素名
        private Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
        private Dictionary<string, string> dicClassCNName = new Dictionary<string, string>();
        private Dictionary<string, string> dicQsdwdm = new Dictionary<string, string>();

        public void VCTDataOutput(string destFile, string temppath, int iDh)
        {
            #region 错误控制
           
            //string destFile = filePath;
            if (!destFile.ToUpper().EndsWith(".VCT"))
            {
                destFile += ".VCT";
            }
            destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile) + "GXGC" + System.IO.Path.GetExtension(destFile);
            if (System.IO.File.Exists(destFile))
            {
                System.IO.File.Delete(destFile);
            }
            //预处理

            if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
            }

            RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");


            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IWorkspace2 pWS2 = currWs as IWorkspace2;
            //找到数据集
            IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }


            List<TableStruct> lstTables = this.GetLstTables(featureDataset, "GX");  //获取所有表结构
            if (lstTables.Count == 0)
            {
                UpdateStatus("当前数据库中没有需要导出的数据，退出...");
                return;
            }



            //string temppath = this.beTmpDir.Text;
            // VCTOut11 outvct = new VCTOut11(temppath);   
            VCTOut12 outvct = new VCTOut12(temppath);
            //int iDh = 0;
            //int.TryParse(this.txtDH.Text.Trim(), out iDh);
            outvct.dh = iDh;
            outvct.gdbWorkspace = this.currWs as IFeatureWorkspace;
            outvct.gdbDataset = featureDataset;
            outvct.allTableStruct = lstTables;
            outvct.DoByAXzq = true;
            outvct.includezj = false;
            #endregion
            //导出shp
            try
            {
                //outvct.ModifyAllTabs();
                UpdateStatus("开始导出文件头...");
                outvct.ExportFileHead3();
                UpdateStatus("导出文件头结束...");
                outvct.ExportPoint3();

                UpdateStatus("导出点文件结束...");
                outvct.ExportLine3();

                UpdateStatus("导出线文件结束...");
                outvct.ExportFill3();
                UpdateStatus("导出面文件结束...");
                outvct.ExportAnotation3();
                UpdateStatus("导出注记结束...");

                //IWorkspac
                //shpwo.ExecuteSQL("update dltbgxgc set BGHTBBSM='',BGHDLBM='',BGHDLMC='',BGHQSXZ='',BGHQSDWDM='',BGHQSDWMC='',BGHZLDWDM='',BGHZLDWMC='',BGHKCDLBM='',BGHKCXS=0,BGHKCMJ=0,BGHTBDLMJ=0,BGHGDLX='',BGHGDPDJB='',BGHXZDWKD=0,BGHTBXHDM='',BGHTBXHMC='',BGHZZSXDM='',BGHZZSXMC='',BGHGDDB=0,BGHFRDBS='',BGHCZCSXM='',BGHMSSM='',BGHHDMC='',BGHTBBH='' where xzqtzlx='4' or xzqtzlx='2'");

                outvct.ExportAttribute3();
                UpdateStatus("导出属性结束...");

                string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
                System.Array.Sort(allFiles);
                ConcatenateFiles(destFile, allFiles);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle.ToInt32());
                UpdateStatus("合并完成，导出完毕！");


            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }

            #region 错误控制

            //destFile = filePath;
            if (!destFile.ToUpper().EndsWith(".VCT"))
            {
                destFile += ".VCT";
            }

            destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile).Substring(0, System.IO.Path.GetFileNameWithoutExtension(destFile).Length - 2) + System.IO.Path.GetExtension(destFile);
            if (System.IO.File.Exists(destFile))
            {
                System.IO.File.Delete(destFile);
            }
            //预处理

            if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
            }

            RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");


            pFeaWs = this.currWs as IFeatureWorkspace;
            pWS2 = currWs as IWorkspace2;
            //找到数据集
            featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }


            lstTables = this.GetLstTables(featureDataset, "GXGC");  //获取所有表结构
            if (lstTables.Count == 0)
            {
                UpdateStatus("当前数据库中没有需要导出的数据，退出...");
                return;
            }



            //temppath = this.beTmpDir.Text;
            // VCTOut11 outvct = new VCTOut11(temppath);   
            outvct = new VCTOut12(temppath);
            //iDh = 0;
            //int.TryParse(this.txtDH.Text.Trim(), out iDh);
            outvct.dh = iDh;
            outvct.gdbWorkspace = this.currWs as IFeatureWorkspace;
            outvct.gdbDataset = featureDataset;
            outvct.allTableStruct = lstTables;
            outvct.DoByAXzq = true;
            outvct.includezj = false;
            #endregion
            //导出shp
            try
            {
                //outvct.ModifyAllTabs();
                UpdateStatus("开始导出文件头...");
                outvct.ExportFileHead3();
                UpdateStatus("导出文件头结束...");
                outvct.ExportPoint3();

                UpdateStatus("导出点文件结束...");
                outvct.ExportLine3();

                UpdateStatus("导出线文件结束...");
                outvct.ExportFill3();
                UpdateStatus("导出面文件结束...");
                outvct.ExportAnotation3();
                UpdateStatus("导出注记结束...");
                outvct.ExportAttribute3();
                UpdateStatus("导出属性结束...");

                string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
                System.Array.Sort(allFiles);
                ConcatenateFiles(destFile, allFiles);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle.ToInt32());
                UpdateStatus("合并完成，导出完毕！");

                //MessageBox.Show("导出结束！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }
        }

        public void ConcatenateFiles(string outputFile, string[] inputFiles)
        {
            using (Stream output = File.OpenWrite(outputFile))
            {
                foreach (string inputFile in inputFiles)
                {
                    using (Stream input = File.OpenRead(inputFile))
                    {
                        input.CopyTo(output);
                    }
                }
            }
        }

        public List<TableStruct> GetLstTables(IDataset mDataset, string NotEnd)
        {
            List<TableStruct> lstTables = new List<TableStruct>();

            IEnumDataset penumDataset = mDataset.Subsets;
            IDataset table = penumDataset.Next();
            while (table != null)
            {
                if (table.Name.ToUpper().StartsWith("TP_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                if (table.Name.Contains("_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                //掠过zj
                if (table.Name.ToUpper() == "ZJ")
                {
                    table = penumDataset.Next();
                    continue;
                }
                if (table.Name.ToUpper().EndsWith(NotEnd))
                {
                    table = penumDataset.Next();
                    continue;
                }

                IFeatureClass tableCls = table as IFeatureClass;
                string sName = table.Name;
                string sGraph = "Point";
                if (tableCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    sGraph = "Point";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    sGraph = "Line";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    sGraph = "Polygon";
                }

                if (dicClassCNName.ContainsKey(sName))
                {
                    string featureCode = dicClassYsdm[sName];
                    string FeatureName = dicClassCNName[sName];
                    lstTables.Add(new TableStruct(FeatureName, featureCode, sName, sGraph));
                }


                table = penumDataset.Next();
            }
            return lstTables;

        }

        public void VCTDataPrepare(string temppath)
        {
            //获取所有要素代码
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM where type in ('POINT','LINE','POLYGON') ", "ysdm");
            dicClassCNName = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dicClassCNName.Add(dr["CLASSNAME"].ToString(), dr["ALIASNAME"].ToString());
                dicClassYsdm.Add(dr["CLASSNAME"].ToString(), dr["YSDM"].ToString());
            }


            IFeatureDataset featureDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }

            //string temppath = this.beTmpDir.Text;
            if (!Directory.Exists(temppath))
            {
                Directory.CreateDirectory(temppath);
            }
            string fileName = "vct.gdb";
            string shpPath = temppath + "\\" + fileName;

            if (!System.IO.Directory.Exists(shpPath))
            {
                //不存在则创建
                //创建一个临时库

                try
                {
                    IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                    pWorkspaceFactory.Create(temppath, fileName, null, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else
            {
                IWorkspace pTmpWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(shpPath);
                if (pTmpWorkspace == null)
                {
                    RCIS.Utility.FileHelper.DelectDir(shpPath);
                    System.IO.Directory.Delete(shpPath);
                }
                else
                    (pTmpWorkspace as IDataset).Delete();
                try
                {
                    IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                    pWorkspaceFactory.Create(temppath, fileName, null, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            UpdateStatus("正在进行预处理...");
            IWorkspace pTarWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(shpPath);
            IEnumDataset srcDS = pTarWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            IDataset aDs = null;
            while ((aDs = srcDS.Next()) != null)
            {
                try
                {
                    aDs.Delete();
                }
                catch { }
            }


            List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构            
            //导出各要素
            try
            {
                foreach (TableStruct ts in lstTables)
                {
                    IFeatureClass pFC = null;
                    try
                    {
                        pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(ts.className);
                        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(this.currWs, pTarWorkspace, ts.className, ts.className, null);
                    }
                    catch { }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //2021年3月22日08:46:22   数据过滤   只是zldwdm或者qsdwdm变更的，不进入更新层和更新过程层
            try
            {
                IFeatureClass pGXGCClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
                IFeatureClass pGXClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "BGXW='1' and XZQTZLX<>'2' AND XZQTZLX<>'4'";
                //pQf.WhereClause = "BGXW='1' OR BGXW='0'";
                IFeatureCursor pFeaCursor = pGXGCClass.Update(pQf, true);
                IFeature pFeature;
                while ((pFeature = pFeaCursor.NextFeature()) != null)
                {
                    bool isDel = true;
                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        string filedName = pFeature.Fields.Field[i].Name.ToString().Trim().ToUpper();
                    
                        if (filedName.Contains("BGQ") && !filedName.Contains("BSM") && !filedName.Contains("TBBH") && !filedName.Contains("ZLDWDM") && !filedName.Contains("ZLDWMC") && !filedName.Contains("QSDWDM") && !filedName.Contains("QSDWMC"))
                        {
                            if (pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() != pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim())
                            {
                                if (filedName == "BGQGDDB" && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() == "0") && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim() == "0"))
                                    continue;
                                else
                                {
                                    isDel = false;
                                    break;
                                }
                    
                            }
                        }
                    }
                    if (isDel)
                    {
                        pFeature.set_Value(pFeature.Fields.FindField("BSM"), "DEL");
                        pFeaCursor.UpdateFeature(pFeature);
                    }
                    //}
                    RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                }
                pFeaCursor.Flush();
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
                IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
                IQueryFilter pQfilter = new QueryFilterClass();
                pQfilter.WhereClause = "bsm='DEL'";
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pTarWorkspace, pTmpWs, "DLTBGXGC", "GXGC", pQfilter);
                pTarWorkspace.ExecuteSQL("delete from dltbgxgc where bsm='DEL' OR XZQTZLX='2'");
                //pTarWorkspace.ExecuteSQL("update dltbgxgc set BGHZLDWDM=BGQZLDWDM where XZQTZLX='4'");
                //pTarWorkspace.ExecuteSQL("update dltbgxgc set BGHTBBSM='',BGHDLBM='',BGHDLMC='',BGHQSXZ='',BGHQSDWDM='',BGHQSDWMC='',BGHZLDWDM='',BGHZLDWMC='',BGHKCDLBM='',BGHKCXS=0,BGHKCMJ=0,BGHTBDLMJ=0,BGHGDLX='',BGHGDPDJB='',BGHXZDWKD=0,BGHTBXHDM='',BGHTBXHMC='',BGHZZSXDM='',BGHZZSXMC='',BGHGDDB=0,BGHFRDBS='',BGHCZCSXM='',BGHMSSM='',BGHHDMC='',BGHTBBH='' where xzqtzlx='4' or xzqtzlx='2'");
                bool b = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(pTarWorkspace.PathName + "\\DLTBGX", pTmpWs.PathName + "\\GXGC", pTmpWs.PathName + "\\SpatialJoinGx", "CONTAINS");
                if (!b)
                {
                    UpdateStatus("叠加分析错误");
                    return;
                }
                pTarWorkspace.ExecuteSQL("delete from dltbgx");
                pTmpWs.ExecuteSQL("delete from SpatialJoinGx where Join_Count=1");

                IFeatureClass pSDClass = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("SpatialJoinGx");
                for (int i = pSDClass.Fields.FieldCount - 1; i >= 0; i--)
                {
                    IField pField = pSDClass.Fields.get_Field(i);
                    if (pField.Name.Contains("_1"))
                        (pSDClass as ITable).DeleteField(pField);
                }
                b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\SpatialJoinGx", pTarWorkspace.PathName + "\\DLTBGX");
                if (!b)
                {
                    UpdateStatus("叠加分析错误");
                    return;
                }
                //2021年10月22日15:01:28过滤图斑、村级调查区、行政区更新层调出数据
                pTarWorkspace.ExecuteSQL("delete from dltbgx where zldwdm not like '" + xzdm + "%'");
                pTarWorkspace.ExecuteSQL("delete from cjdcqgx where zldwdm not like '" + xzdm + "%'");
                pTarWorkspace.ExecuteSQL("delete from xzqgx where xzqdm not like '" + xzdm + "%'");

                RCIS.GISCommon.GpToolHelper.RepairGeometry(pTarWorkspace.PathName + "\\DLTBGX");
                RCIS.GISCommon.GpToolHelper.RepairGeometry(pTarWorkspace.PathName + "\\DLTBGXGC");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            bool bRet = true;
            //执行面转线
            //this.Cursor = Cursors.WaitCursor;
            IWorkspace2 wsname2 = pTarWorkspace as IWorkspace2;
            foreach (TableStruct ts in lstTables)
            {
                if (ts.type.ToUpper() == "POLYGON")
                {
                    string shpfileName = shpPath + "\\" + ts.className.ToUpper();
                    string lineShpFile = shpPath + "\\" + ts.className.ToUpper() + "line";
                    if (wsname2.get_NameExists(esriDatasetType.esriDTFeatureClass, ts.className))
                    {
                        IFeatureClass pFClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
                        if (pFClass.FeatureCount(null) > 0)
                        {
                            bRet &= PolygonToline(shpfileName, lineShpFile);
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFClass);
                    }
                }
            }

            //this.Cursor = Cursors.Default;
            if (bRet == false)
            {
                UpdateStatus("关联图层失败，退出...");

                return;
            }

            if (bRet == false)
            {
                UpdateStatus("关联图层失败，退出...");

                return;
            }
            else
            {
                UpdateStatus("数据准备完成，请继续！");
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pTarWorkspace);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //MessageBox.Show("数据准备完成，请继续后续工作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public List<TableStruct> GetLstTables(IDataset mDataset)
        {
            List<TableStruct> lstTables = new List<TableStruct>();

            IEnumDataset penumDataset = mDataset.Subsets;
            IDataset table = penumDataset.Next();
            while (table != null)
            {
                if (table.Name.ToUpper().StartsWith("TP_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                if (table.Name.Contains("_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                //掠过zj
                if (table.Name.ToUpper() == "ZJ")
                {
                    table = penumDataset.Next();
                    continue;
                }

                IFeatureClass tableCls = table as IFeatureClass;
                string sName = table.Name;
                string sGraph = "Point";
                if (tableCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    sGraph = "Point";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    sGraph = "Line";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    sGraph = "Polygon";
                }

                if (dicClassCNName.ContainsKey(sName))
                {
                    string featureCode = dicClassYsdm[sName];
                    string FeatureName = dicClassCNName[sName];
                    lstTables.Add(new TableStruct(FeatureName, featureCode, sName, sGraph));
                }


                table = penumDataset.Next();
            }
            return lstTables;

        }

        public bool PolygonToline(string dltbFile, string lineFile)
        {
            Geoprocessor gp = new Geoprocessor();
            ESRI.ArcGIS.DataManagementTools.PolygonToLine toLine = new ESRI.ArcGIS.DataManagementTools.PolygonToLine();
            try
            {

                toLine.in_features = dltbFile;
                toLine.out_feature_class = lineFile;
                toLine.neighbor_option = "IDENTIFY_NEIGHBORS";
                IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(toLine, null);
                string sMsg = "";
                if (gp.MessageCount > 0)
                {
                    for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                    {
                        sMsg += gp.GetMessage(Count);
                    }
                }
                if (sMsg.Contains("ERROR") || sMsg.Contains("失败"))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}
