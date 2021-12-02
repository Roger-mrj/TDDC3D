using System;
using System.Collections.Generic;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.edit
{
    public class SmallTbDoHelper
    {


        public static  IFeature getMaxFeatureSame3DL(IFeature inTb, IFeatureClass dltbClass, double mjTolerance, ref List<int> lstExcepted)
        {
            string oldDlbm = FeatureHelper.GetFeatureStringValue(inTb, "DLBM");
            IFeature currFea = null;
            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();
           
            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            //pSF.WhereClause = "BZ<>'DEL'";

            //pSF.WhereClause = "ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";
            //pSF.WhereClause = shpFld + " >" + mjTolerance + "  and ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID)
                            continue;
                        if (aFea.get_Value(aFea.Fields.FindField("BZ")).ToString().Trim() == "DEL")
                            continue;
                        string newDlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                        bool isSame=false;
                        if (sys.YWCommonHelper.IsNyd(newDlbm) && sys.YWCommonHelper.IsNyd(oldDlbm))
                        {
                            isSame=true;
                        }
                        if (sys.YWCommonHelper.isJsyd(newDlbm) && sys.YWCommonHelper.isJsyd(oldDlbm))
                        {
                            isSame = true;
                        }
                        if (sys.YWCommonHelper.isWlyd(newDlbm) && sys.YWCommonHelper.isWlyd(oldDlbm))
                        {
                            isSame = true;
                        }
                        if (!isSame) continue;
                        double newMj = (aFea.ShapeCopy as IArea).Area;

                        //if (lstExcepted.Contains(aFea.OID))
                        //    continue;
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }

        /// <summary>
        /// 先找二级地类相同的
        /// </summary>
        /// <param name="inTb"></param>
        /// <param name="dltbClass"></param>
        /// <returns></returns>
        public static  IFeature getMaxFeatureSameDL2(IFeature inTb, IFeatureClass dltbClass, double mjTolerance, ref List<int> lstExcepted)
        {
            string oldDlbm = FeatureHelper.GetFeatureStringValue(inTb, "DLBM");
            IFeature currFea = null;

            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();


            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            pSF.WhereClause = shpFld + " >" + mjTolerance + " and DLBM='" + oldDlbm + "' and ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {

                        double newMj = (aFea.ShapeCopy as IArea).Area;

                        if (lstExcepted.Contains(aFea.OID))
                            continue;
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }


        public static  IFeature getMaxFeatureSameDL1(IFeature inTb, IFeatureClass dltbClass, double mjTolerance, ref List<int> lstExcepted)
        {
            string oldDlbm = FeatureHelper.GetFeatureStringValue(inTb, "DLBM");
            IFeature currFea = null;

            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();


            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            //pSF.WhereClause = "BZ<>'DEL'";
            //pSF.WhereClause = "ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";
            //pSF.WhereClause = shpFld + " >" + mjTolerance + " and ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";

            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID)
                            continue;
                        if (aFea.get_Value(aFea.Fields.FindField("BZ")).ToString().Trim() == "DEL")
                            continue;
                        double newMj = (aFea.ShapeCopy as IArea).Area;
                        string dlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                        if (getDlbmYj(oldDlbm) != getDlbmYj(dlbm))
                        {
                            //一级类相同
                            continue;
                        }
                        //if (lstExcepted.Contains(aFea.OID))
                        //    continue;
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }

        public static string getDlbmYj(string dlbm)
        {
            List<string> arr = new List<string>(){ "01","02","05","06","07","08","09","10","12" };
            List<string> arr00 = new List<string>() { "0303", "0304", "0306", "0402", "0603", "1105", "1106", "1108" };
            List<string> arr03 = new List<string>() { "0301", "0302", "0305", "0307" };
            List<string> arr04 = new List<string>() { "0401", "0403", "0404" };
            List<string> arr11 = new List<string>() { "1101", "1102", "1103", "1104", "1107", "1109", "1110" };
            if (arr.Contains(dlbm.Substring(0, 2)))
                return dlbm.Substring(0, 2);
            else if (arr00.Contains(dlbm.Substring(0, 4)))
                return "00";
            else if (arr03.Contains(dlbm.Substring(0, 4)))
                return "03";
            else if (arr04.Contains(dlbm.Substring(0, 4)))
                return "04";
            else if (arr11.Contains(dlbm.Substring(0, 4)))
                return "11";
            else
                return "";
        }


        public static  IFeature getMaxFeature(IFeature inTb, IFeatureClass dltbClass, double mjTolerance, ref List<int> lstExcepted)
        {

            IFeature currFea = null;

            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();

            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            //pSF.WhereClause = "BZ<>'DEL'";

            //pSF.WhereClause = "ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";
            //pSF.WhereClause = shpFld + " >" + mjTolerance + " and ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID)
                            continue;
                        if (aFea.get_Value(aFea.Fields.FindField("BZ")).ToString().Trim() == "DEL")
                            continue;
                        double newMj = (aFea.ShapeCopy as IArea).Area;
                        //if (lstExcepted.Contains(aFea.OID))
                        //    continue;
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }


        public static IFeature getMaxFeatureSame3DL(IFeature inTb, IFeatureClass dltbClass,bool ignore101101)
        {
            IFeature aZd = getWithinCJDCQ(inTb);
            IRelationalOperator pRelation = null;  //用来判断宗地包含关系 ++++++guojie 12-27 ++ ,判断宗地包含关系
            if (aZd != null)
            {
                pRelation = aZd.Shape as IRelationalOperator;
            }

            string oldDlbm = FeatureHelper.GetFeatureStringValue(inTb, "DLBM");
            IFeature currFea = null;
            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();
            string qsxz = FeatureHelper.GetFeatureStringValue(inTb, "QSXZ").ToUpper(); //权属性质


            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            pSF.WhereClause =" ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "' and QSXZ='"+qsxz+"'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID) continue;
                        //++++++guojie 12-27 ++ ,判断宗地包含关系
                        if (pRelation != null)
                        {
                            if (!pRelation.Contains(aFea.Shape))
                            {
                                continue;
                            }
                        }
                        //+++++++++++++++++++++++++++++++++

                        


                        //排除道路地类，所有道路都不做处理
                        string newDlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                        if (ignore101101)
                        {

                            if (newDlbm.StartsWith("10") || (newDlbm == "1101"))
                            {
                                continue;
                            }

                        }

                        bool isSame = false;
                        if (sys.YWCommonHelper.IsNyd(newDlbm) && sys.YWCommonHelper.IsNyd(oldDlbm))
                        {
                            isSame = true;
                        }
                        if (sys.YWCommonHelper.isJsyd(newDlbm) && sys.YWCommonHelper.isJsyd(oldDlbm))
                        {
                            isSame = true;
                        }
                        if (sys.YWCommonHelper.isWlyd(newDlbm) && sys.YWCommonHelper.isWlyd(oldDlbm))
                        {
                            isSame = true;
                        }
                        if (!isSame) continue;
                        double newMj = (aFea.ShapeCopy as IArea).Area;

                       
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }

        public static IFeature getWithinCJDCQ(IFeature inTb)
        {
            try
            {
                IFeatureClass xzqClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = inTb.ShapeCopy;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                IFeatureCursor pCusor = xzqClass.Search(pSF, false);
                IFeature aZD = pCusor.NextFeature();
                OtherHelper.ReleaseComObject(pCusor);
                return aZD;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    
        /// <summary>
        /// 找二级地类相同的
        /// </summary>
        /// <param name="inTb"></param>
        /// <param name="dltbClass"></param>
        /// <param name="ignore10"> 是否忽略10 1101 地类</param>
        /// <returns></returns>
        public static IFeature getMaxFeatureSameDL2(IFeature inTb, IFeatureClass dltbClass,bool ignore101101)
        {
            IFeature aDCQ = getWithinCJDCQ(inTb);
            IRelationalOperator pRelation = null;  //用来判断宗地包含关系 ++++++guojie 12-27 ++ ,判断宗地包含关系
            if (aDCQ != null)
            {
                pRelation = aDCQ.Shape as IRelationalOperator;
            }

            string oldDlbm = FeatureHelper.GetFeatureStringValue(inTb, "DLBM");
            IFeature currFea = null;

            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();
            string qsxz = FeatureHelper.GetFeatureStringValue(inTb, "QSXZ").ToUpper(); //权属性质


            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";
            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            pSF.WhereClause =  "  DLBM='" + oldDlbm + "' and ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "' and QSXZ='"+qsxz+"'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID) continue;
                        
                        //++++++guojie 12-27 ++ ,判断宗地包含关系
                        if (pRelation != null)
                        {
                            if (!pRelation.Contains(aFea.Shape))
                            {
                                continue;
                            }
                        }

                        if (ignore101101)
                        {
                            string newDlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                            if (newDlbm.StartsWith("10") || (newDlbm == "1101"))
                            {
                                continue;
                            }

                        }

                        //+++++++++++++++++++++++++++++++++
                        double newMj = (aFea.ShapeCopy as IArea).Area;                       
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }

            //增加判断条件，需要在同一个宗地范围下
            


            return currFea;
        }


        public static IFeature getMaxFeatureSameDL1(IFeature inTb, IFeatureClass dltbClass, bool ignore101101)
        {

            IFeature aZd = getWithinCJDCQ(inTb);
            IRelationalOperator pRelation = null;  //用来判断宗地包含关系 ++++++guojie 12-27 ++ ,判断宗地包含关系
            if (aZd != null)
            {
                pRelation = aZd.Shape as IRelationalOperator;
            }


            string oldDlbm = FeatureHelper.GetFeatureStringValue(inTb, "DLBM");
            IFeature currFea = null;

            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();
            string qsxz = FeatureHelper.GetFeatureStringValue(inTb, "QSXZ").ToUpper(); //权属性质

            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            pSF.WhereClause =  "  ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "' and QSXZ='"+qsxz+"'";

            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID) continue;
                        //++++++guojie 12-27 ++ ,判断宗地包含关系
                        if (pRelation != null)
                        {
                            if (!pRelation.Contains(aFea.Shape))
                            {
                                continue;
                            }
                        }
                        //+++++++++++++++++++++++++++++++++
                        if (ignore101101)
                        {
                            string newDlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                            if (newDlbm.StartsWith("10") || (newDlbm == "1101"))
                            {
                                continue;
                            }

                        }

                        double newMj = (aFea.ShapeCopy as IArea).Area;
                        string dlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                        if (oldDlbm.Substring(0, 2) != dlbm.Substring(0, 2))
                        {
                            //一级类相同
                            continue;
                        }
                      
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }

        /// <summary>
        /// 按相交边长
        /// </summary>
        /// <param name="inTb"></param>
        /// <param name="dltbClass"></param>
        /// <param name="isnore101101"></param>
        /// <returns></returns>
        public static IFeature getMaxFeature2(IFeature inTb, IFeatureClass dltbClass, bool ignore101101)
        {
            IFeature currFea = null;

            double maxLen = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();
            string qsxz = FeatureHelper.GetFeatureStringValue(inTb, "QSXZ").ToUpper(); //权属性质

            ITopologicalOperator pInTop=inTb.ShapeCopy as ITopologicalOperator;

            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";
            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            pSF.WhereClause = " ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "' and QSXZ='" + qsxz + "'";

            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID) continue;
                        if (ignore101101)
                        {
                            string newDlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                            if (newDlbm.StartsWith("10") || (newDlbm == "1101"))
                            {
                                continue;
                            }

                        }


                        //该图斑与输入图斑相交的边
                        IGeometry interGeo = pInTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                        if (interGeo != null && !interGeo.IsEmpty)
                        {
                            IPolyline pLine = interGeo as IPolyline;
                            if (!pLine.IsEmpty)
                            {
                                double newLen = pLine.Length;
                                if (newLen > maxLen)
                                {
                                    maxLen = newLen;
                                    currFea = aFea;
                                }
                            }
                               

                        }


                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }

        public static IFeature getMaxFeature(IFeature inTb, IFeatureClass dltbClass, bool ignore101101)
        {
            IFeature aZd = getWithinCJDCQ(inTb);

            IRelationalOperator pRelation = null;  //用来判断宗地包含关系 ++++++guojie 12-27 ++ ,判断宗地包含关系
            if (aZd != null)
            {
                pRelation = aZd.Shape as IRelationalOperator;
            }

            IFeature currFea = null;

            double maxMj = 0;
            string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
            string oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();
            string qsxz = FeatureHelper.GetFeatureStringValue(inTb, "QSXZ").ToUpper(); //权属性质

            ISpatialFilter pSF = new SpatialFilterClass();
            string shpFld = dltbClass.ShapeFieldName + "_AREA";

            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;
            pSF.WhereClause =  " ZLDWDM='" + oldZldwdm + "' and QSDWDM='" + oldQsdm + "' and QSXZ='"+qsxz+"'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == inTb.OID) continue;
                        //++++++guojie 12-27 ++ ,判断宗地包含关系
                        if (pRelation != null)
                        {
                            if (!pRelation.Contains(aFea.Shape))
                            {
                                continue;
                            }
                        }
                        //+++++++++++++++++++++++++++++++++
                        if (ignore101101)
                        {
                            string newDlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                            if (newDlbm.StartsWith("10") || (newDlbm == "1101"))
                            {
                                continue;
                            }

                        }


                        double newMj = (aFea.ShapeCopy as IArea).Area;                       
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            return currFea;
        }


        /// <summary>
        /// 单纯找 与该图形相交最大的图形
        /// </summary>
        /// <param name="inGeo"></param>
        /// <param name="dltbClass"></param>
        /// <returns></returns>
        public static IFeature getMaxFeature(IGeometry inGeo, IFeatureClass dltbClass,int oid)
        {
            
            IFeature currFea = null;
            ITopologicalOperator pTopIn = inGeo as ITopologicalOperator;
            pTopIn.Simplify();

            double maxMj = 0;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = inGeo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == oid) continue;
                        IGeometry intersect = pTopIn.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                        if (intersect.IsEmpty)
                        {
                            continue;
                        }


                        double newMj = (aFea.ShapeCopy as IArea).Area;
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
            return currFea;
        }

    }
}
