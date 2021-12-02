using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using RCIS.GISCommon;
using RCIS.Utility;

using System.Data;

namespace TDDC3D.sys
{
    public class YWCommonHelper
    {
        /// <summary>
        /// 获取所有要素代码 及中文名
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LoadAllZjYsdm()
        {
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select ysdm,aliasname from SYS_YSDM where type='ANNOTATION' ", "TMP");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr["ysdm"].ToString(), dr["aliasname"].ToString());
            }
            return dic;
        }

        /// <summary>
        /// 获取所有地类编码和名称
        /// </summary>
        /// <returns></returns>
        public static List<string> getAllDlbm()
        {
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select DM,MC from 三调工作分类", "tmp");
            List<string> allDlbm = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                allDlbm.Add(dr["DM"].ToString() + "|" + dr["MC"].ToString());
            }
            return allDlbm;
        }

        public static List<string> getOnlyDlbm()
        {
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select DM,MC from 三调工作分类", "tmp");
            List<string> allDlbm = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                allDlbm.Add(dr["DM"].ToString());
            }
            return allDlbm;
        }

        public static bool priorityDLBM2(string dlbm1, string dlbm2)
        {
            if (dlbm1 == "" && dlbm2 != "")
                return false;
            if (dlbm1 != "" && dlbm2 == "")
                return true;
            if (dlbm2 == "" && dlbm1 == "")
                return true;

            string sql = "select * from SYS_XZDWHB_DLSORT WHERE DLBM='" + dlbm1 + "' OR DLBM='" + dlbm2 + "' ";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            int sort1=0, sort2=0;
            foreach(DataRow dr in dt.Rows)
            {
                string dl=dr["DLBM"].ToString();
               
                if (dl==dlbm1)
                {
                    int.TryParse(dr["sort"].ToString(),out sort1);
                }
                else if (dl==dlbm2)
                {
                    int.TryParse(dr["sort"].ToString(),out sort2);
                }
            }
            if (sort1 < sort2)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断第一个地类是否比第二个地类优先
        /// </summary>
        /// <param name="dlbm1"></param>
        /// <param name="dlbm2"></param>
        /// <returns></returns>
        public static bool priorityDLBM(string dlbm1, string dlbm2)
        {
            if ((dlbm1=="1001" ) && (dlbm2!="1001"))
            {
                return true;
            }
            if ((dlbm1 == "1003") && (dlbm2 != "1001") )
                return true;
            if ((dlbm1 == "1006") && (dlbm2 != "1001") && (dlbm2 != "1003"))
                return true;
            if ((dlbm1 == "1107") && (dlbm2 != "1001") && (dlbm2 != "1003") && (dlbm2 != "1006") )
                return true;
            return false;
        }

        /// <summary>
        /// 获取行政代码 从权属单位代码表中
        /// </summary>
        /// <param name="pTable"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static  List<string> getXzqFromQsdwdm(ITable pTable, int num)
        {
            List<string> lst = new List<string>();
            IQueryFilter pQF = new QueryFilterClass();
            ICursor aCursor = null;
           
            try
            {
                pQF.WhereClause = "JB=" + num;
                aCursor = pTable.Search(pQF, false);
                IRow aRow = null;
                while ((aRow = aCursor.NextRow()) != null)
                {
                    lst.Add(FeatureHelper.GetRowValue(aRow, "QSDWDM").ToString() + "|" + FeatureHelper.GetRowValue(aRow, "QSDWMC").ToString());
                }
            }
            catch(Exception ex) { }
            finally
            {
                OtherHelper.ReleaseComObject(aCursor);
            }
           
            
            return lst;
        }


        public static List<IFeature> SplitDltbByXzq2(IFeatureClass xzqClass, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IFeature selFea, ref List<int> errId)
        {

            List<int> alreadySplitIds = new List<int>();

            List<IFeature> lst = new List<IFeature>();
            //与当前这个要素 相交的行政区
            List<IFeature> allXzqs = GetFeaturesHelper.getFeaturesByGeo(xzqClass, selFea.Shape.Envelope as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);

            foreach (IFeature aXzqFea in allXzqs)
            {

                string xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQDM");

                #region 切割图斑

                IRelationalOperator pRO = aXzqFea.Shape as IRelationalOperator;
                if (pRO.Contains(selFea.Shape))
                {
                    //包含该图斑
                    lst.Add(selFea);

                }
                else
                {
                    ITopologicalOperator pTopXzqGeo = aXzqFea.ShapeCopy as ITopologicalOperator;  //行政区图形
                    IGeometry pXzqJX = pTopXzqGeo.Boundary; //行政区界线
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(selFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (!cutPolygon.IsEmpty)
                    {
                        //如果相交与一个面

                        if (cutPolygon.Equals(selFea.Shape))
                        {
                            lst.Add(selFea);
                        }
                        else
                        {
                            //并且这个面与原来的不相等
                            try
                            {
                                int selFeaId = selFea.OID;

                                if (!alreadySplitIds.Contains(selFeaId))
                                {
                                    IFeatureEdit featureEdit = selFea as IFeatureEdit;
                                    ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                                    newFeaturesSet.Reset();
                                    IFeature newTb = newFeaturesSet.Next() as IFeature;
                                    while (newTb != null)
                                    {
                                        lst.Add(newTb);
                                        newTb = newFeaturesSet.Next() as IFeature;
                                    }

                                    alreadySplitIds.Add(selFeaId); //表示这个图斑已经被分割了，相邻那个
                                }
                                

                            }
                            catch (Exception ex)
                            {
                                if (!errId.Contains(selFea.OID))
                                {
                                    errId.Add(selFea.OID);
                                }
                            }

                        }
                    }
                   
                }

                
                #endregion

            }
            return lst;

        }


        public static void SplitDltbByCjdcq2(IFeatureClass xzqClass, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IEnvelope extent, ref List<int> errId)
        {

            //获取当前范围的所有行政区
            ISpatialFilter pXzqSF = new SpatialFilter();
            pXzqSF.Geometry = extent as IGeometry;
            pXzqSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor xzqCursor = xzqClass.Search(pXzqSF as IQueryFilter, true);
            IFeature aXzqFea = null;
            try
            {
                while ((aXzqFea = xzqCursor.NextFeature()) != null)
                {
                    string xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWMC");
                    string xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWDM");
                    #region 切割图斑
                    ITopologicalOperator pTopXzqGeo = aXzqFea.ShapeCopy as ITopologicalOperator;
                    IGeometry pXzqJX = pTopXzqGeo.Boundary;

                    //取查找 所有与边界相交的 地类图斑
                    ITopologicalOperator pJxTop = pXzqJX as ITopologicalOperator;
                    wait.SetCaption("正在用 [" + xzqmc + "] 切割图斑...");

                    using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
                    {

                        ISpatialFilter pDltbSr = new SpatialFilterClass();
                        pDltbSr.Geometry = pXzqJX;
                        pDltbSr.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pCursor = dltbClass.Search(pDltbSr as IQueryFilter, true);
                        release.ManageLifetime(pCursor);
                        IFeature aDltbFea = null;
                        while ((aDltbFea = pCursor.NextFeature()) != null)
                        {
                            
                            #region 判断 分割
                            IGeometry intersecLine = pJxTop.Intersect(aDltbFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                            if (intersecLine.IsEmpty)
                            {
                                continue;
                            }
                            IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltbFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                            if (cutPolygon.IsEmpty)
                            {
                                continue;
                            }
                            //如果交与一条线,且不包含
                            if (GeometryHelper.IsContain(aXzqFea.Shape, aDltbFea.ShapeCopy))
                            {
                                continue;
                            }
                            
                            //分割之
                            try
                            {
                                if (aDltbFea.OID == 99)
                                {
                                }
                                IFeatureEdit featureEdit = aDltbFea as IFeatureEdit;
                                ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                                newFeaturesSet.Reset();
                                IFeature newTb = newFeaturesSet.Next() as IFeature;
                                while (newTb != null)
                                {
                                    newTb = newFeaturesSet.Next() as IFeature;
                                }

                            }
                            catch (Exception ex)
                            {
                                if (!errId.Contains(aDltbFea.OID))
                                {
                                    errId.Add(aDltbFea.OID);
                                }
                            }
                            #endregion 
                            
                        }                       

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pDltbSr);
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();

                    }


                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                OtherHelper.ReleaseComObject(pXzqSF);
                OtherHelper.ReleaseComObject(xzqCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }         


        }

        public static void SplitDltbByCjdcq(IFeatureClass xzqClass, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IEnvelope extent, ref List<int> errId)
        {

            //获取当前范围的所有行政区
            List<IFeature> allXzqs = GetFeaturesHelper.getFeaturesByGeo(xzqClass, extent as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);

            foreach (IFeature aXzqFea in allXzqs)
            {

                string xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWDM");

                #region 切割图斑

                List<IFeature> interDltbs = GetFeaturesHelper.getFeatureByBoundaryIntersect(dltbClass, aXzqFea.ShapeCopy);
                ITopologicalOperator pTopXzqGeo = aXzqFea.ShapeCopy as ITopologicalOperator;
                IGeometry pXzqJX = pTopXzqGeo.Boundary;
                //用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称

                for (int kk = interDltbs.Count - 1; kk >= 0; kk--)
                {
                    IFeature aDltb = interDltbs[kk];
                    if (wait != null)
                    {
                        wait.SetCaption("正在用 [" + xzqmc + "] 切割图斑...");
                    }
                    //如果行政区 和 该图斑不是交于一个面，则略过
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (!cutPolygon.IsEmpty)
                    {
                        try
                        {
                            IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                            ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                            newFeaturesSet.Reset();
                            IFeature newTb = newFeaturesSet.Next() as IFeature;
                            while (newTb != null)
                            {
                                newTb = newFeaturesSet.Next() as IFeature;
                            }

                        }
                        catch (Exception ex)
                        {
                            if (!errId.Contains(aDltb.OID))
                            {
                                errId.Add(aDltb.OID);
                            }
                        }
                    }

                    interDltbs.RemoveAt(kk);
                    if (aDltb!=null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(aDltb);
                    }
                    

                    
                }

                //foreach (IFeature aDltb in interDltbs)
                //{

                //    //如果行政区 和 该图斑不是交于一个面，则略过
                //    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                //    if (cutPolygon.IsEmpty)
                //    {
                //        continue;
                //    }
                //    if (wait != null)
                //    {
                //        wait.SetCaption("正在用 [" + xzqmc + "] 切割图斑...");
                //    }

                //    try
                //    {
                //        IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                //        ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                //        newFeaturesSet.Reset();
                //        IFeature newTb = newFeaturesSet.Next() as IFeature;
                //        while (newTb != null)
                //        {

                //            newTb = newFeaturesSet.Next() as IFeature;
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //        if (!errId.Contains(aDltb.OID))
                //        {
                //            errId.Add(aDltb.OID);
                //        }
                //    }

                //}
                #endregion


                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            allXzqs.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            

        }


        /// <summary>
        /// 行政区分割图斑
        /// </summary>
        /// <param name="zdClass"></param>
        /// <param name="dltbClass"></param>
        /// <param name="wait"></param>
        /// <param name="extent"></param>
        public static  void SplitDltbByXzq(IFeatureClass xzqClass, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IEnvelope extent,ref List<int> errId)
        {

            //获取当前范围的所有行政区
            List<IFeature> allXzqs = GetFeaturesHelper.getFeaturesByGeo(xzqClass, extent as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
            
            foreach (IFeature aXzqFea in allXzqs)
            {
            
                string xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQDM");
               
                #region 切割图斑

                List<IFeature> interDltbs = GetFeaturesHelper.getFeatureByBoundaryIntersect(dltbClass, aXzqFea.ShapeCopy);
                ITopologicalOperator pTopXzqGeo = aXzqFea.ShapeCopy as ITopologicalOperator;
                IGeometry pXzqJX = pTopXzqGeo.Boundary;
                //用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称
                foreach (IFeature aDltb in interDltbs)
                {
                    
                    //如果行政区 和 该图斑不是交于一个面，则略过
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (cutPolygon.IsEmpty)
                    {                       
                        continue;
                    }
                    if (wait != null)
                    {
                        wait.SetCaption("正在用 [" + xzqmc + "] 切割图斑...");
                    }

                    try
                    {                        
                        IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                        ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                        newFeaturesSet.Reset();
                        IFeature newTb = newFeaturesSet.Next() as IFeature;
                        while (newTb != null)
                        {
                           
                            newTb = newFeaturesSet.Next() as IFeature;
                        }

                    }
                    catch (Exception ex)
                    {
                        if (!errId.Contains(aDltb.OID))
                        {
                            errId.Add(aDltb.OID);
                        }
                    }

                }
                #endregion

            }
            
           
        }

        public static void SplitDltbBySelect(List<IFeature> allXzqs, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IEnvelope extent, ref List<int> errId)
        {
            foreach (IFeature aXzqFea in allXzqs)
            {

                //string xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQMC");
                //string xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQDM");

                #region 切割图斑

                List<IFeature> interDltbs = GetFeaturesHelper.getFeatureByBoundaryIntersect(dltbClass, aXzqFea.ShapeCopy);
                ITopologicalOperator pTopXzqGeo = aXzqFea.ShapeCopy as ITopologicalOperator;
                IGeometry pXzqJX = pTopXzqGeo.Boundary;
                //用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称
                foreach (IFeature aDltb in interDltbs)
                {

                    //如果行政区 和 该图斑不是交于一个面，则略过
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (cutPolygon.IsEmpty)
                    {
                        continue;
                    }
                    if (wait != null)
                    {
                        wait.SetCaption("正在切割图斑...");
                    }

                    try
                    {
                        IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                        ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                        newFeaturesSet.Reset();
                        IFeature newTb = newFeaturesSet.Next() as IFeature;
                        while (newTb != null)
                        {

                            newTb = newFeaturesSet.Next() as IFeature;
                        }


                    }
                    catch (Exception ex)
                    {
                        if (!errId.Contains(aDltb.OID))
                        {
                            errId.Add(aDltb.OID);
                        }
                    }

                }
                #endregion

            }


        }

        /// <summary>
        /// 宗地分割图斑
        /// </summary>
        /// <param name="zdClass"></param>
        /// <param name="dltbClass"></param>
        /// <param name="wait"></param>
        /// <param name="extent"></param>
        /// <param name="errId"></param>
        public static void SplitDltbByZd(IFeatureClass zdClass, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IEnvelope extent, ref List<int> errId)
        {


            List<IFeature> allZds = GetFeaturesHelper.getFeaturesByGeo(zdClass, extent as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
            foreach (IFeature aZdFea in allZds)
            {
                #region 切割图斑

                List<IFeature> interDltbs = GetFeaturesHelper.getFeatureByBoundaryIntersect(dltbClass, aZdFea.ShapeCopy);
                ITopologicalOperator pTopXzqGeo = aZdFea.ShapeCopy as ITopologicalOperator;
                IGeometry pXzqJX = pTopXzqGeo.Boundary;
                //用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称
                foreach (IFeature aDltb in interDltbs)
                {

                    //如果行政区 和 该图斑不是交于一个面，则略过
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (cutPolygon.IsEmpty)
                    {
                        continue;
                    }
                    wait.SetCaption("正在用 [" + aZdFea.OID + "] 切割图斑...");

                    try
                    {
                        IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                        ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                        newFeaturesSet.Reset();
                        IFeature newTb = newFeaturesSet.Next() as IFeature;
                        while (newTb != null)
                        {

                            newTb = newFeaturesSet.Next() as IFeature;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!errId.Contains(aDltb.OID))
                        {
                            errId.Add(aDltb.OID);
                        }
                    }

                }
                #endregion

            }

        }

        public static List<IFeature>  SplitDltbByZd2(IFeatureClass zdClass, IFeatureClass dltbClass, DevExpress.Utils.WaitDialogForm wait, IFeature selFea, ref List<int> errId)
        {

            List<int> alreadySplitIds = new List<int>();

            List<IFeature> lst = new List<IFeature>();

            List<IFeature> allZds = GetFeaturesHelper.getFeaturesByGeo(zdClass, selFea.Shape.Envelope as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
            foreach (IFeature aZdFea in allZds)
            {

                IRelationalOperator pRO = aZdFea.Shape as IRelationalOperator;
                if (pRO.Contains(selFea.Shape))
                {
                    //包含该图斑
                    lst.Add(selFea);

                }
                else
                {
                    #region 切割图斑


                    ITopologicalOperator pTopXzqGeo = aZdFea.ShapeCopy as ITopologicalOperator;
                    IGeometry pXzqJX = pTopXzqGeo.Boundary;
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(selFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (!cutPolygon.IsEmpty)
                    {
                        //如果相交与一个面

                        if (cutPolygon.Equals(selFea.Shape))
                        {
                            lst.Add(selFea);
                        }
                        else
                        {
                            //并且这个面与原来的不相等
                            try
                            {
                                int selFeaId = selFea.OID;

                                if (!alreadySplitIds.Contains(selFeaId))
                                {
                                    IFeatureEdit featureEdit = selFea as IFeatureEdit;
                                    ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                                    newFeaturesSet.Reset();
                                    IFeature newTb = newFeaturesSet.Next() as IFeature;
                                    while (newTb != null)
                                    {
                                        lst.Add(newTb);
                                        newTb = newFeaturesSet.Next() as IFeature;
                                    }

                                    alreadySplitIds.Add(selFeaId); //表示这个图斑已经被分割了，相邻那个
                                }


                            }
                            catch (Exception ex)
                            {
                                if (!errId.Contains(selFea.OID))
                                {
                                    errId.Add(selFea.OID);
                                }
                            }

                        }
                    }

                    //List<IFeature> interDltbs = GetFeaturesByGeoHelper.getFeatureByBoundaryIntersect(dltbClass, aZdFea.ShapeCopy);
                    //ITopologicalOperator pTopXzqGeo = aZdFea.ShapeCopy as ITopologicalOperator;
                    //IGeometry pXzqJX = pTopXzqGeo.Boundary;
                    ////用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称
                    //foreach (IFeature aDltb in interDltbs)
                    //{

                    //    //如果行政区 和 该图斑不是交于一个面，则略过
                    //    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    //    if (cutPolygon.IsEmpty)
                    //    {
                    //        continue;
                    //    }
                    //    wait.SetCaption("正在用 [" + aZdFea.OID + "] 切割图斑...");

                    //    try
                    //    {
                    //        IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                    //        ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                    //        newFeaturesSet.Reset();
                    //        IFeature newTb = newFeaturesSet.Next() as IFeature;
                    //        while (newTb != null)
                    //        {
                    //            lst.Add(newTb);
                    //            newTb = newFeaturesSet.Next() as IFeature;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        if (!errId.Contains(aDltb.OID))
                    //        {
                    //            errId.Add(aDltb.OID);
                    //        }
                    //    }

                    //}
                    #endregion
                }

                

            }
           
            return lst;
        }

        /// <summary>
        /// 设置权属单位代码和坐落单位代码，行政区分割时候用
        /// </summary>
        /// <param name="aXzqFea"></param>
        /// <param name="dltbClass"></param>
        /// <param name="arSplitedTb"></param>
        public static  void SetZldwdm(IFeature aXzqFea, IFeatureClass dltbClass)
        {
            //为哎行政区 中的 图斑属性赋值

            //找到所有地类图斑
            List<IFeature> arAllTbs = GetFeaturesHelper.getFeaturesByGeo(dltbClass, aXzqFea.ShapeCopy, esriSpatialRelEnum.esriSpatialRelContains);

            IRelationalOperator pTop = aXzqFea.ShapeCopy as IRelationalOperator;
            foreach (IFeature newTb  in arAllTbs)
            {
                if (pTop.Contains(newTb.ShapeCopy))
                {
                    string xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQMC");
                    string xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQDM");
                    FeatureHelper.SetFeatureValue(newTb, "QSDWDM", xzqdm);
                    FeatureHelper.SetFeatureValue(newTb, "QSDWMC", xzqmc);
                    FeatureHelper.SetFeatureValue(newTb, "ZLDWDM", xzqdm);
                    FeatureHelper.SetFeatureValue(newTb, "ZLDWMC", xzqmc);
                    newTb.Store();
                }
                
            }

        }

        private static List<IPoint> getSplitedPoints2(IPolygon aDltb, IGeometry newGeoBoundry)
        {
            List<IPoint> splitPts = new List<IPoint>();

            ITopologicalOperator pTbJxBound = aDltb as ITopologicalOperator;
            //先交与一条线
            IGeometry pInterGeo = pTbJxBound.Intersect(newGeoBoundry, esriGeometryDimension.esriGeometry1Dimension);
            if (!pInterGeo.IsEmpty)
            {
                IPointCollection ptCols = pInterGeo as IPointCollection;
                for (int i = 0; i < ptCols.PointCount; i++)
                {
                    IPoint aPt = ptCols.get_Point(i);
                    if (!splitPts.Contains(aPt))
                    {
                        splitPts.Add(aPt);
                    }
                }

            }
            //再交一个点
            pInterGeo = pTbJxBound.Intersect(newGeoBoundry, esriGeometryDimension.esriGeometry0Dimension);
            if (!pInterGeo.IsEmpty)   //无交叉的，都交不着
            {
                IPointCollection ptCols = pInterGeo as IPointCollection;
                for (int i = 0; i < ptCols.PointCount; i++)
                {
                    IPoint aPt = ptCols.get_Point(i);
                    if (!splitPts.Contains(aPt))
                    {
                        splitPts.Add(aPt);
                    }
                }
            }
            return splitPts;
        }

        private static List<IPoint> getSplitedPoints(IPolyline  aDljx, IGeometry newGeoBoundary)
        {
            List<IPoint> splitPts = new List<IPoint>();
           
            //原来的地类界线 与 新图形的边界线 的交点求出来
            ITopologicalOperator pAJxTp = aDljx as ITopologicalOperator;
            //先交与一条线
            IGeometry pInterGeo = pAJxTp.Intersect(newGeoBoundary, esriGeometryDimension.esriGeometry1Dimension);
            if (!pInterGeo.IsEmpty)
            {
                IPointCollection ptCols = pInterGeo as IPointCollection;
                for (int i = 0; i < ptCols.PointCount; i++)
                {
                    IPoint aPt = ptCols.get_Point(i);
                    if (!splitPts.Contains(aPt))
                    {
                        splitPts.Add(aPt);
                    }
                }

            }
            //再交一个点
            pInterGeo = pAJxTp.Intersect(newGeoBoundary, esriGeometryDimension.esriGeometry0Dimension);
            if (!pInterGeo.IsEmpty)   //无交叉的，都交不着
            {
                IPointCollection ptCols = pInterGeo as IPointCollection;
                for (int i = 0; i < ptCols.PointCount; i++)
                {
                    IPoint aPt = ptCols.get_Point(i);
                    if (!splitPts.Contains(aPt))
                    {
                        splitPts.Add(aPt);
                    }
                }
            }

            ////交点重新捕捉到 界线上
            //foreach (IPoint aPt in splitPts)
            //{
            //    IPoint hitPoint = new PointClass();
            //    double hitDistance = 0; int hitPartIndex = 0; int hitSegmentIndex = 0; bool bReightSide = false;
            //    esriGeometryHitPartType hitTpye = esriGeometryHitPartType.esriGeometryPartBoundary;
            //    double searchRadius = 0.1;
            //    IHitTest hitShape = aDljx as IHitTest;
            //    hitShape.HitTest(aPt, searchRadius, hitTpye, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bReightSide);
            //    if (hitPoint.IsEmpty == false)
            //    {
            //        newSplitPts.Add(hitPoint);
            //    }
            //    else
            //    {
            //        newSplitPts.Add(aPt);
            //    }

            //}
            return splitPts;

        }

        /// <summary>
        /// //地类界线变更,返回新生成的地类界限
        /// </summary>
        /// <param name="lstDljx"></param>
        /// <param name="dljxClass"></param>
        /// <param name="cutGeo"></param>
        public static List<IFeature>  DljxBg(List<IFeature> lstDljx, IFeatureClass dljxClass, IPolygon cutGeo)
        {

            List<IFeature> newDljx = new List<IFeature>(); //记录新生成的

            ITopologicalOperator2 pCutGeoTop = cutGeo as ITopologicalOperator2;

            IGeometry newGeoBound = pCutGeoTop.Boundary;  //新图形的  边界线
            foreach (IFeature aFea in lstDljx)
            {
                

                List<IPoint> newSplitPts = getSplitedPoints(aFea.ShapeCopy as IPolyline, newGeoBound);
                
                //利用交点，进行元界线分割
                IRelationalOperator pCutGeoRel = cutGeo as IRelationalOperator;
                List<IPolyline> newJx = GeometryHelper.SplitALineAtPoints(aFea.ShapeCopy as IPolyline, newSplitPts);
                //割成许多界线
                if (newJx.Count > 1)
                {
                    foreach (IPolyline aSplitNewJX in newJx)
                    {
                        aSplitNewJX.SpatialReference = cutGeo.SpatialReference;
                        aSplitNewJX.SnapToSpatialReference();
                        if (!pCutGeoRel.Contains(aSplitNewJX))
                        {
                            //FeatureHelper.SetFeatureValue(bgFea, "BGXW", "0");
                            IFeature bgDljxFea = dljxClass.CreateFeature();
                            bgDljxFea.Shape = aSplitNewJX;
                            FeatureHelper.CopyFeature(aFea, bgDljxFea);
                            bgDljxFea.Store();

                            newDljx.Add(bgDljxFea); //没删掉的  返回，后面可能地类图斑变更后还会删掉

                        }

                    }
                    //如果割成多条，原来的删掉
                    aFea.Delete();
                }
                else
                {
                    newDljx.Add(aFea); //没变化的 也 放到里面返回
                }
                

              
            }
            return newDljx;
        }

        /// <summary>
        /// 判断 该点 是否在 list中
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="aPt"></param>
        /// <returns></returns>
        public static bool isExistPoint(List<IPoint> lst, IPoint aPt)
        {
            bool isExist = false;
            foreach (IPoint aPoint in lst)
            {
                IRelationalOperator pRel = aPoint as IRelationalOperator;
                if (pRel.Equals(aPt as IGeometry))
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        /// <summary>
        /// 获取面 所有外环内环线
        /// </summary>
        /// <param name="cutGeo"></param>
        /// <returns></returns>
        public static List<IPolyline> Polygon2Line(IGeometry cutGeo)
        {
            List<IPolyline> allLines = new List<IPolyline>();

            object missing1 = Type.Missing;
            object missing2 = Type.Missing;
            IPolygon4 newPolygon = cutGeo as IPolygon4;
            IGeometryBag exteriorRingGeometryBag = newPolygon.ExteriorRingBag;
            IGeometryCollection exteriorRingGeometryCollection = exteriorRingGeometryBag as IGeometryCollection;
            for (int i = 0; i < exteriorRingGeometryCollection.GeometryCount; i++)
            {
                IGeometry exteriorRingGeometry = exteriorRingGeometryCollection.get_Geometry(i);
                ////外环上所有的点
                //IPointCollection exteriorRingPointCollection = exteriorRingGeometry as IPointCollection;
                //该外环的内环
                IGeometryBag interiorRingGeometryBag = newPolygon.get_InteriorRingBag(exteriorRingGeometry as IRing);
                IGeometryCollection interiorRingGeometryCollection = interiorRingGeometryBag as IGeometryCollection;
                for (int k = 0; k < interiorRingGeometryCollection.GeometryCount; k++)
                {
                    IGeometry interiorRingGeometry = interiorRingGeometryCollection.get_Geometry(k);
                    ISegmentCollection interSegs = interiorRingGeometry as ISegmentCollection;

                    IGeometryCollection inPolyline = new PolylineClass();
                    ISegmentCollection inAPath = new PathClass();
                     for (int k1 = 0; k1 < interSegs.SegmentCount; k1++)
                     {
                         ISegment s = interSegs.get_Segment(k1);
                         IPoint fromPoint = s.FromPoint;
                         IPoint toPoint = s.ToPoint;
                         ILine pLine = new LineClass();
                         pLine.PutCoords(fromPoint, toPoint);
                         inAPath.AddSegment(pLine as ISegment, ref missing1, ref missing2);

                         
                     }
                     
                     inPolyline.AddGeometry(inAPath as IGeometry, ref missing1, ref missing2);
                     allLines.Add(inPolyline as IPolyline);
                }


                //外环 形成线
                ISegmentCollection outerSegs = exteriorRingGeometry as ISegmentCollection;
                IGeometryCollection outPolyline = new PolylineClass();
                ISegmentCollection outerAPath = new PathClass();
                for (int i1 = 0; i1 < outerSegs.SegmentCount; i1++)
                {
                    ISegment s = outerSegs.get_Segment(i1);
                    IPoint fromPoint = s.FromPoint;
                    IPoint toPoint = s.ToPoint;
                    ILine pLine = new LineClass();
                    pLine.PutCoords(fromPoint, toPoint);
                    outerAPath.AddSegment(pLine as ISegment, ref missing1, ref missing2);
                                        
                }
                outPolyline.AddGeometry(outerAPath as IGeometry, ref missing1, ref missing2);
                allLines.Add(outPolyline as IPolyline);
            }
            return allLines;

        }

        public static List<IFeature> buildNewTbjx2(List<IFeature> oldDljxs,IFeatureClass dljxClass, IGeometry cutGeo)
        {
            List<IFeature> newDljxs = new List<IFeature>();
            List<IPolyline> allBoundary = Polygon2Line(cutGeo);            
            foreach (IPolyline aBoundary in allBoundary)
            {
                
                List<IPoint> newSplitPts = new List<IPoint>();
                //某个环线
                foreach (IFeature aOldDljx in oldDljxs)
                {

                    List<IPoint> lsttmp = getSplitedPoints(aOldDljx.ShapeCopy as IPolyline, aBoundary);
                    foreach (IPoint aTmpPt in lsttmp)
                    {
                        

                        if (!isExistPoint(newSplitPts, aTmpPt))
                        {
                            newSplitPts.Add(aTmpPt);
                        }
                       
                    }
                }
                //没交点，可能是内环
                if (newSplitPts.Count == 0)
                {
                    IFeature newJxFea = dljxClass.CreateFeature();
                    newJxFea.Shape = aBoundary;
                    FeatureHelper.SetFeatureValue(newJxFea, "YSDM", "2001040000");
                    FeatureHelper.SetFeatureValue(newJxFea, "DLJXLX", "9");
                    newJxFea.Store();
                    newDljxs.Add(newJxFea);

                }
                else
                {
                    //闭环 打不段？
                    //ISegmentCollection segCollection = aBoundary as ISegmentCollection;
                    //newSplitPts.Add((aBoundary as IPolyline).FromPoint);

                    //for (int i = 0; i < segCollection.SegmentCount; i++)
                    //{
                    //    ISegment aSeg = segCollection.get_Segment(i);
                    //    ICurve aCurve = aSeg as ICurve;
                    //    IPoint toPt = aCurve.ToPoint;
                    //    newSplitPts.Add(toPt);

                    //}

                    
                    List<IPolyline> newDljxGeo = GeometryHelper.SplitALineAtPoints(aBoundary, newSplitPts);
                    //生成新的地类界线
                    foreach (IPolyline anewJx in newDljxGeo)
                    {
                        //竟然有非常碎的线
                        if (anewJx.Length < 0.0001)
                            continue;

                        bool isExist = false;
                        foreach (IFeature aFea in oldDljxs)
                        {
                            if (GeometryHelper.LineEquals(aFea.ShapeCopy as IPolyline, anewJx))
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            IFeature newJxFea = dljxClass.CreateFeature();
                            newJxFea.Shape = anewJx;
                            FeatureHelper.SetFeatureValue(newJxFea, "YSDM", "2001040000");
                            FeatureHelper.SetFeatureValue(newJxFea, "DLJXLX", "9");
                            newJxFea.Store();

                            newDljxs.Add(newJxFea);
                            
                        }
                    }
                }

                


            }
            return newDljxs;

        }

        public static List<IFeature> BuildNewTbJx(List<IFeature> oldDljxs, IFeatureClass dljxClass, IGeometry cutGeo)
        {
            List<IFeature> newBoundayJx = new List<IFeature>();
            



            ITopologicalOperator2 pCutGeoTop = cutGeo as ITopologicalOperator2;
            IPolyline newGeoBound = pCutGeoTop.Boundary as IPolyline;  //新图形的  边界线
           
            List<IPoint> newSplitPts = new List<IPoint>();
            #region 断点
            //边界线所有断点，也加进来
            ISegmentCollection segCollection = newGeoBound as ISegmentCollection;
            newSplitPts.Add((newGeoBound as IPolyline).FromPoint);

            for (int i = 0; i < segCollection.SegmentCount; i++)
            {
                ISegment aSeg = segCollection.get_Segment(i);
                ICurve aCurve = aSeg as ICurve;
                IPoint toPt = aCurve.ToPoint;
                newSplitPts.Add(toPt);

            }

            #endregion 
            foreach (IFeature aOldDljx in oldDljxs)
            {
                List<IPoint> lsttmp = getSplitedPoints(newGeoBound , aOldDljx.ShapeCopy as IPolyline);
                foreach (IPoint aTmpPt in lsttmp)
                {
                    if (!isExistPoint(newSplitPts, aTmpPt))
                    {
                        newSplitPts.Add(aTmpPt);
                    }
                }
            }


           

            //所有交点  得到，然后分割

            List<IPolyline> newDljx = GeometryHelper.SplitALineAtPoints(newGeoBound, newSplitPts);
            //生成新的地类界线
            foreach (IPolyline anewJx in newDljx)
            {
                bool isExist = false;
                foreach (IFeature aFea in oldDljxs)
                {
                    if (GeometryHelper.LineEquals(aFea.ShapeCopy as IPolyline,anewJx))
                    {
                        isExist=true;
                        break;
                    }
                }
                if (!isExist)
                {
                    IFeature newJxFea = dljxClass.CreateFeature();
                    newJxFea.Shape = anewJx;
                    FeatureHelper.SetFeatureValue(newJxFea, "YSDM", "2001040000");
                    FeatureHelper.SetFeatureValue(newJxFea, "DLJXLX", "9");
                    newJxFea.Store();

                    newBoundayJx.Add(newJxFea);
                }

                
            }
            return newBoundayJx;
        }


        ////新的地类界线
        ///// <summary>
        ///// 新增的地类界线
        ///// </summary>
        ///// <param name="lstDljx"></param>
        ///// <param name="lstDltb">相交的图斑</param>
        ///// <param name="dljxClass"></param>
        ///// <param name="cutGeo">新图形</param>
        //public static List<IFeature> BuildNewDljx( List<IFeature> lstDltb, IFeatureClass dljxClass, IGeometry cutGeo)
        //{
        //    List<IFeature> newBoundayJx = new List<IFeature>();
        //    ITopologicalOperator2 pCutGeoTop = cutGeo as ITopologicalOperator2;
        //    IGeometry newGeoBound = pCutGeoTop.Boundary;  //新图形的  边界线

        //    List<IPoint> lstSplitPoint = new List<IPoint>();

        //    #region  形成新的边界线S
        //    foreach (IFeature aFea in lstDltb)
        //    {
        //        //每块图斑的边界线 与新的边图形相交
        //        ITopologicalOperator pTbTop = aFea.ShapeCopy as ITopologicalOperator;
        //        IGeometry pTbJxBound = pTbTop.Boundary;
        //        ITopologicalOperator pTbJxBoundTop = pTbJxBound as ITopologicalOperator;
        //        IGeometry pGeoInter = pTbJxBoundTop.Intersect(cutGeo, esriGeometryDimension.esriGeometry1Dimension);
        //        if (!pGeoInter.IsEmpty)
        //        {
        //            IPointCollection ptCols = pGeoInter as IPointCollection;
        //            for (int i = 0; i < ptCols.PointCount; i++)
        //            {
        //                IPoint aPt = ptCols.get_Point(i);

        //                bool isExist = isExistPoint(lstSplitPoint, aPt);
        //                if (!isExist)
        //                {
        //                    lstSplitPoint.Add(aPt);
        //                }


        //            }
        //        }
        //        pGeoInter = pTbJxBoundTop.Intersect(cutGeo, esriGeometryDimension.esriGeometry0Dimension);
        //        if (!pGeoInter.IsEmpty)
        //        {
        //            IPointCollection ptCols = pGeoInter as IPointCollection;
        //            for (int i = 0; i < ptCols.PointCount; i++)
        //            {
        //                IPoint aPt = ptCols.get_Point(i);

        //                bool isExist = isExistPoint(lstSplitPoint, aPt);
        //                if (!isExist)
        //                {
        //                    lstSplitPoint.Add(aPt);
        //                }


        //            }
        //        }
        //    }

        //    #region 断点
        //    //边界线所有断点，也加进来
        //    ISegmentCollection segCollection = newGeoBound as ISegmentCollection;
        //    for (int i = 0; i < segCollection.SegmentCount; i++)
        //    {
        //        ISegment aSeg = segCollection.get_Segment(i);
        //        ICurve aCurve = aSeg as ICurve;

        //        IPoint fromPt = aCurve.FromPoint;
        //        IPoint toPt = aCurve.ToPoint;

        //        bool isExist = isExistPoint(lstSplitPoint, fromPt);
        //        if (!isExist)
        //        {
        //            lstSplitPoint.Add(fromPt);
        //        }

        //        if (!isExistPoint(lstSplitPoint, toPt))
        //        {
        //            lstSplitPoint.Add(toPt);
        //        }

        //    }
        //    #endregion 

        //    //将新图斑边界线 按交点分割
        //    IPolyline newGeoPolyline = newGeoBound as IPolyline;
        //    List<IPolyline> newDljx = GeometryHelper.SplitALineAtPoints(newGeoPolyline, lstSplitPoint);

        //    //生成新的地类界线
        //    foreach (IPolyline anewJx in newDljx)
        //    {
        //        //该条与 其他地类接线相同
        //        IFeature newJxFea = dljxClass.CreateFeature();
        //        newJxFea.Shape = anewJx;
        //        FeatureHelper.SetFeatureValue(newJxFea, "YSDM", "2001040000");
        //        FeatureHelper.SetFeatureValue(newJxFea, "DLJXLX", "9");
        //        newJxFea.Store();

        //        newBoundayJx.Add(newJxFea);
        //    }

            
        //    #endregion

        //    return newBoundayJx;

        //}


        /// <summary>
        /// //获取传入图形 包含的 其他要素
        /// </summary>
        /// <param name="cutGeo"></param>
        /// <param name="aClass"></param>
        /// <returns></returns>
        public static List<IFeature> GetContainerFeatures(IGeometry cutGeo, IFeatureClass aClass)
        {
            List<IFeature> lstdljx = new List<IFeature>();
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = cutGeo;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            IFeatureClass featureClass = aClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {
                    lstdljx.Add(aFea);
                    aFea = featureCursor.NextFeature();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            }
            return lstdljx;
        }

        private static IFeature CreateNewDltb(IFeature aBgqDltb, IGeometry newGeo, IFeatureClass dltbClass)
        {
            IFeature newDltb = dltbClass.CreateFeature();
            newDltb.Shape = newGeo as IPolygon;
            IPoint selectPoint = (newDltb.ShapeCopy as IArea).Centroid;
            double X = selectPoint.X;
            int currDh = (int)(X / 1000000);////WK---带号

            //LSSphereArea.GGP.ClsSphereArea area = new LSSphereArea.GGP.ClsSphereArea();
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            double tbmj = area.SphereArea(newDltb.ShapeCopy, currDh);
            FeatureHelper.SetFeatureValue(newDltb, "TBMJ", tbmj);
            newDltb.Store();
            return newDltb;
        }


        private static IFeature CreateNewDltb(IFeature aBgqDltb, IFeature bghDltb, IFeatureClass dltbClass)
        {
            IFeature newDltb = dltbClass.CreateFeature();
            newDltb.Shape = bghDltb.Shape as IPolygon;

            ////赋属性值,有值的替换，没有值的 不替换
            for (int i = 0; i < newDltb.Fields.FieldCount; i++)
            {
                IField pFld = newDltb.Fields.get_Field(i);
                if (pFld.Type == esriFieldType.esriFieldTypeGeometry || pFld.Type == esriFieldType.esriFieldTypeGlobalID
                    || pFld.Type == esriFieldType.esriFieldTypeGUID || pFld.Type == esriFieldType.esriFieldTypeOID
                    || pFld.Type == esriFieldType.esriFieldTypeRaster || pFld.Type == esriFieldType.esriFieldTypeXML)
                {
                    continue;
                }
                if (pFld.Editable == false)
                    continue;
                string fldName = pFld.Name;
                //if (fldName.ToUpper() == "ZLDWDM")
                //{
                //}
                int fidx = bghDltb.Fields.FindField(fldName);
                if (fidx == -1)
                    continue;

                object value = bghDltb.get_Value(fidx);
                if (value == null || value == DBNull.Value)
                    continue;
                try
                {
                    string sVal = value.ToString();
                    if (sVal.Trim() == "")
                        continue;
                    newDltb.set_Value(i, value);
                }
                catch (Exception ex)
                {
                }
            }

            IPoint selectPoint = (newDltb.ShapeCopy as IArea).Centroid;
            double X = selectPoint.X;
            int currDh = (int)(X / 1000000);////WK---带号

            //LSSphereArea.GGP.ClsSphereArea area = new LSSphereArea.GGP.ClsSphereArea();
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            double tbmj = area.SphereArea(newDltb.ShapeCopy, currDh);
            FeatureHelper.SetFeatureValue(newDltb, "TBMJ", tbmj);
            newDltb.Store();
            return newDltb;
        }


        /// <summary>
        /// //地类图斑变更,返回新要素
        /// </summary>
        /// <param name="inDltbs"></param>
        /// <param name="dltbClass"></param>
        /// <param name="newGeo"></param>
        public static List<IFeature>  DltbBg(List<IFeature> inDltbs, IFeatureClass dltbClass, IGeometry newGeo,Dictionary<string,string> values)
        {
            
            List<IFeature> lstNewDltbs = new List<IFeature>();
            //记录总面积
            double zmj = 0;
            foreach (IFeature aFea in inDltbs)
            {
                zmj += FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ");
            }

            if (inDltbs.Count == 0)
                return null;
            //首先新建图斑
            IFeature aBgqDltb = inDltbs[0];
            IFeature newDltb = CreateNewDltb(aBgqDltb, newGeo, dltbClass);
            foreach (KeyValuePair<string, string> aItem in values)
            {
                FeatureHelper.SetFeatureValue(newDltb, aItem.Key, aItem.Value);
            }
            FeatureHelper.SetFeatureValue(newDltb, "BSM", -1);
            newDltb.Store();

            lstNewDltbs.Add(newDltb);  //记录下来,以便面积调平

            //cutGeo是新宗地图形
            //处理其他宗地
            ITopologicalOperator2 pCutGeoTop = newGeo as ITopologicalOperator2;
            IRelationalOperator pCutGeoRel = newGeo as IRelationalOperator;
            IGeometry newGeoBound = pCutGeoTop.Boundary;  //新图形的  边界线
            //分割后的图斑如果保存在 新建图形内部的，灭失掉
            #region 地类图斑变更
            foreach (IFeature aFeature in inDltbs)
            {
                //完全包含在内的直接删除
                if (pCutGeoRel.Contains(aFeature.ShapeCopy))
                {
                    aFeature.Delete();
                    continue;
                }
                //其余的分割
                IFeatureEdit featureEdit = aFeature as IFeatureEdit;
                ISet newFeaturesSet = featureEdit.Split(newGeoBound);
                if (newFeaturesSet.Count > 1)
                {
                    newFeaturesSet.Reset();
                    //分割后的 图斑
                    IFeature newFeature = newFeaturesSet.Next() as IFeature;
                    while (newFeature != null)
                    {
                        //切出来的新图斑图形 如果与 变更后图斑相等，则删除
                        if (pCutGeoRel.Equals(newFeature.Shape))
                        {
                            newFeature.Delete();
                        }else
                        if ((pCutGeoRel.Contains(newFeature.ShapeCopy)) && !(newFeature.ShapeCopy as IRelationalOperator).Contains(newGeo))  
                        {                            
                            newFeature.Delete();                         
                            
                        }
                        else
                        {
                            #region 赋属性
                            IPoint selectPoint = (newFeature.ShapeCopy as IArea).Centroid;
                            double X = selectPoint.X;
                            int currDh = (int)(X / 1000000);////WK---带号
                            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();

                            double tbmj = area.SphereArea(newFeature.ShapeCopy, currDh);
                            FeatureHelper.SetFeatureValue(newFeature, "TBMJ", tbmj);
                            FeatureHelper.SetFeatureValue(newFeature, "BSM", -1);
                            newFeature.Store();
                            #endregion
                            lstNewDltbs.Add(newFeature);
                        }
                        newFeature = newFeaturesSet.Next() as IFeature;
                    }
                }

                

            }
            #endregion

            mjtp(zmj, lstNewDltbs);  //面积调平
            return lstNewDltbs;


        }


        public static List<IFeature> DltbBg(List<IFeature> inDltbs, IFeatureClass dltbClass, IFeature bghFeature, Dictionary<string, string> values)
        {

            List<IFeature> lstNewDltbs = new List<IFeature>();
            //记录 变更前地类图斑 图斑面积总面积
            //double zmj = 0;
            //foreach (IFeature aFea in inDltbs)
            //{
            //    zmj += FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ");
            //}

            if (inDltbs.Count == 0)
                return null;
            //首先新建图斑
            IFeature aBgqDltb = inDltbs[0];
            //根据变更后的地类图斑 ，新建一个 要素，其属性继承过来
            IFeature newDltb = CreateNewDltb(aBgqDltb, bghFeature, dltbClass);
            foreach (KeyValuePair<string, string> aItem in values)
            {
                FeatureHelper.SetFeatureValue(newDltb, aItem.Key, aItem.Value);
            }
            FeatureHelper.SetFeatureValue(newDltb, "BSM", -1);
            newDltb.Store();          
            lstNewDltbs.Add(newDltb);  //记录下来,以便面积调平
           

           
            //新图形将切割原来的地类图斑
            IGeometry newGeo = bghFeature.Shape;
            IRelationalOperator pCutGeoRel = newGeo as IRelationalOperator;
            
            #region 地类图斑变更
            foreach (IFeature sourceFea in inDltbs)
            {
                //完全包含在内的直接删除
                if (pCutGeoRel.Contains(sourceFea.ShapeCopy))
                {
                    sourceFea.Delete();
                    continue;
                }

                long id = sourceFea.OID;

                //用原来的 图斑切割新图形
                ITopologicalOperator2 sourceTop2 = sourceFea.ShapeCopy as ITopologicalOperator2;
                sourceTop2.IsKnownSimple_2 = true;
                sourceTop2.Simplify();
                IGeometry outputGeo = sourceTop2.Difference(newGeo);
                // 切割后 多部分打散
                List<IGeometry> outGeos = BreakGeometry(outputGeo);
                //新增图斑
                foreach (IGeometry aGeo in outGeos)
                {
                    if (aGeo.IsEmpty)
                        continue;
                    IFeature aTargetFea = dltbClass.CreateFeature();
                    aTargetFea.Shape = aGeo ;
                    //拷贝其他属性
                    FeatureHelper.CopyFeature(sourceFea, aTargetFea);
                    //try
                    //{
                    //    IPoint selectPoint = (newDltb.ShapeCopy as IArea).Centroid;
                    //    double X = selectPoint.X;
                    //    int currDh = (int)(X / 1000000);////WK---带号
                    //    SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                    //    double tbmj = area.SphereArea(aTargetFea.ShapeCopy, currDh);
                    //    FeatureHelper.SetFeatureValue(aTargetFea, "TBMJ", tbmj);
                    //}
                    //catch (Exception ex)
                    //{
                    //   // throw new Exception("变更过程中"+aTargetFea.OID + "要素椭球面积计算失败!");
                    //}
                    aTargetFea.Store();

                    lstNewDltbs.Add(aTargetFea); // 记录以便调平
                }
                
                //旧图斑删除
                sourceFea.Delete();

            }            
            #endregion

         //   mjtp(zmj, lstNewDltbs);  //面积调平
            return lstNewDltbs;


        }

        /// <summary>
        /// 根据输入的代码从权属代码表获取到名称
        /// </summary>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public static string getXZQMC(string sCode)
        {
            IFeatureWorkspace pFeaWs = RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
            ITable pQsdwdmTab = null;
            try
            {
                pQsdwdmTab = pFeaWs.OpenTable("QSDWDMB");
            }
            catch { return ""; }
            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "QSDWDM='" + sCode + "'";
            IRow aFea = null;
            ICursor pCursor = pQsdwdmTab.Search(pQf, false);
            try
            {
                aFea = pCursor.NextRow();
                if (aFea != null)
                {
                    string mc = RCIS.GISCommon.FeatureHelper.GetRowValue(aFea, "QSDWMC").ToString().Trim();
                    return mc;
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                if (aFea != null)
                {
                    OtherHelper.ReleaseComObject(aFea);
                }
                OtherHelper.ReleaseComObject(pCursor);
                OtherHelper.ReleaseComObject(pQf);
            }
            
        }
        
        /// <summary>
        /// 将输入图斑面积 调平到 总面积一致
        /// </summary>
        /// <param name="zmj"></param>
        /// <param name="lstNewZds"></param>
        public static void mjtp(double zmj, List<IFeature> lstNewZds)
        {
            if (lstNewZds.Count == 0)
                return;

            double newZmj = 0;
            //新要素类面积和， 同时记录面积最大的新要素
            IFeature maxFea = lstNewZds[0];
            double maxMj = FeatureHelper.GetFeatureDoubleValue(maxFea, "TBMJ");

            foreach (IFeature newFea in lstNewZds)
            {
                double mj = FeatureHelper.GetFeatureDoubleValue(newFea, "TBMJ");
                newZmj += mj;
                if (maxMj < mj)
                {
                    maxMj = mj;
                    maxFea = newFea;
                }
            }
            //获得新要素中最大的一个，将差值 平进去

            //求差值
            double diff = zmj - newZmj;
            FeatureHelper.SetFeatureValue(maxFea, "TBMJ", MathHelper.Round(maxMj + diff, 2));
            maxFea.Store();


        }


        /// <summary>
        /// 是否二调的农用地
        /// </summary>
        /// <param name="dl"></param>
        /// <returns></returns>
        public static bool Is2DNyd(string dl)
        {
            if (dl.ToUpper().StartsWith("01") || dl.ToUpper().StartsWith("02")
                || dl.ToUpper().StartsWith("03") || dl.ToUpper() == "041" || dl.ToUpper() == "042"
                || dl.ToUpper() == "104" || dl.ToUpper() == "114" || dl.ToUpper() == "117"
                || dl.ToUpper() == "122" || dl.ToUpper() == "123")
                return true;
            else return false;
        }

        /// <summary>
        /// 农用地
        /// </summary>
        /// <param name="dl"></param>
        /// <returns></returns>
        public static bool IsNyd(string dl)
        {
            if (dl.ToUpper().StartsWith("01") || dl.ToUpper().StartsWith("02")
                || dl.ToUpper().StartsWith("03") || dl.ToUpper() == "0401" || dl.ToUpper() == "0402" || dl.ToUpper() == "0403"
                || dl.ToUpper() == "1006" || dl.ToUpper() == "1103" || dl.ToUpper() == "1104" || dl.ToUpper() == "1107"
                || dl.ToUpper() == "1203" || dl == "1202" )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是建设用地
        /// </summary>
        /// <param name="dl"></param>
        /// <returns></returns>
        public static bool isJsyd(string dl)
        {
            dl = dl.ToUpper();
            if (dl.StartsWith("05") || dl.StartsWith("06") || dl.StartsWith("07") || dl.StartsWith("08")
                || dl.StartsWith("09") || dl == "1001" || dl == "1002" || dl == "1003" || dl == "1004" || dl == "1005" || dl == "1007"
                || dl == "1008" || dl == "1009" || dl == "1109" || dl == "1201" )
                return true;
            else return false;
        }

        public static bool is2DJsyd(string dl)
        {
            dl = dl.Trim();
            if (dl.StartsWith("20") || dl == "101" || dl == "102" || dl == "105" || dl == "106" || dl == "107" || dl == "113" || dl == "118" || dl == "121")
                return true;
            else return false;
        }

        /// <summary>
        /// 农用地除了设施农用地
        /// </summary>
        /// <param name="dl"></param>
        /// <returns></returns>
        public static bool IsNydExp1202(string dl)
        {
            if (dl.ToUpper().StartsWith("01") || dl.ToUpper().StartsWith("02")
                || dl.ToUpper().StartsWith("03") || dl.ToUpper() == "0401" || dl.ToUpper() == "0402" || dl.ToUpper() == "0403"
                || dl.ToUpper() == "1006" || dl.ToUpper() == "1103" || dl.ToUpper() == "1104" || dl.ToUpper() == "1107"
                || dl.ToUpper() == "1203")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isJsydAnd1202(string dl)
        {
            dl = dl.ToUpper();
            if (dl.StartsWith("05") || dl.StartsWith("06") || dl.StartsWith("07") || dl.StartsWith("08")
                || dl.StartsWith("09") || dl == "1001" || dl == "1002" || dl == "1003" || dl == "1004" || dl == "1005" || dl == "1007"
                || dl == "1008" || dl == "1009" || dl == "1109" || dl == "1201" || dl=="1202" )
                return true;
            else return false;
        }

        public static bool isWlyd(string dl)
        {
            dl = dl.ToUpper();
            if (dl == "0404" || dl == "1101" || dl == "1102" || dl == "1105" || dl == "1106" || dl == "1108" || dl == "1110" || dl == "1204"
                || dl == "1205" || dl == "1206" || dl == "1207")
                return true;
            else return false;
        }

        public static bool Is2DWlyd(string dl)
        {
            dl = dl.Trim();
            if (dl == "043" || dl == "111" || dl == "112" || dl == "115" || dl == "116" || dl == "119"
                || dl == "124" || dl == "125" || dl == "126" || dl == "127")
            {
                return true;
            }
            else return false;
        }
        //地类界线 灭失
        public static void DljxMs(List<IFeature> lstDljx)
        {
            foreach (IFeature aFea in lstDljx)
            {
                aFea.Delete();
            }
        }


        public static List<IGeometry> BreakGeometry(IGeometry sourceGeo)
        {
            List<IGeometry> lst = new List<IGeometry>();
            IGeometryBag exteriorRingGeoBag = (sourceGeo as IPolygon4).ExteriorRingBag;
            IGeometryCollection extRingEoCol = exteriorRingGeoBag as IGeometryCollection; //获得所有外环

            for (int i = 0; i < extRingEoCol.GeometryCount; i++)
            {
                IGeometry extRingGeometry = extRingEoCol.get_Geometry(i);
                IGeometryCollection geoPolygon = new PolygonClass();
                geoPolygon.AddGeometry(extRingGeometry);
                
                //如果这个图形有内环
                IGeometryBag interiorRingGeoBag = (sourceGeo as IPolygon4).get_InteriorRingBag(extRingGeometry as IRing);
                IGeometryCollection inRingGeomCollection = interiorRingGeoBag as IGeometryCollection;
                for (int k = 0; k < inRingGeomCollection.GeometryCount; k++)
                {
                    IGeometry interRingGeo = inRingGeomCollection.get_Geometry(k);
                    geoPolygon.AddGeometry(interRingGeo);
                }

                IPolygon4 polyGonGeo = geoPolygon as IPolygon4;
                polyGonGeo.Project(sourceGeo.SpatialReference);
                lst.Add(geoPolygon as IGeometry);
            }
                      

            return lst;
        }

        /// <summary>
        /// 多部分打散
        /// </summary>
        /// <param name="aFea"></param>
        public static List<IFeature>  BreakupFeature(IFeature aFea,IFeatureClass pFC)
        {

            List<IFeature> lst = new List<IFeature>();

            IGeometry aGeo = aFea.ShapeCopy;
            IGeometryBag exteriorRingGeoBag = (aGeo as IPolygon4).ExteriorRingBag;
            IGeometryCollection extRingEoCol = exteriorRingGeoBag as IGeometryCollection; //获得所有外环
           
            for (int i = 0; i < extRingEoCol.GeometryCount; i++)
            {
                IGeometry extRingGeometry = extRingEoCol.get_Geometry(i);
                IGeometryCollection geoPolygon = new PolygonClass();
                geoPolygon.AddGeometry(extRingGeometry);



                //如果这个图形有内环
                IGeometryBag interiorRingGeoBag = (aGeo as IPolygon4).get_InteriorRingBag(extRingGeometry as IRing);
                IGeometryCollection inRingGeomCollection = interiorRingGeoBag as IGeometryCollection;
                for (int k = 0; k < inRingGeomCollection.GeometryCount; k++)
                {
                    IGeometry interRingGeo = inRingGeomCollection.get_Geometry(k);
                    geoPolygon.AddGeometry(interRingGeo);
                }

                IPolygon4 polyGonGeo = geoPolygon as IPolygon4;
                polyGonGeo.Project(aFea.Shape.SpatialReference);
                
                IFeature newFeaturre = pFC.CreateFeature();
                IFeatureEdit newFeaEdit = newFeaturre as IFeatureEdit;
                //娘的，属性全没了
                newFeaturre.Shape = polyGonGeo as IGeometry;
                FeatureHelper.CopyFeature(aFea, newFeaturre);
                newFeaturre.Store();

                lst.Add(newFeaturre);

            }

            //旧的删除
            aFea.Delete();

            return lst;

        }


    }
}
