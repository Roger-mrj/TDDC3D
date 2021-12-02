using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.Collections;
using RCIS.Utility;
using ESRI.ArcGIS.ADF;
namespace RCIS.GISCommon
{
    public class GetFeaturesHelper
    {


        

        /// <summary>
        /// 获取 要素，按照sql条件
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<IFeature> getFeaturesBySql(IFeatureClass targetClass, string where)
        {
            List<IFeature> lstSelFeas = new List<IFeature>();
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            IFeatureCursor pCursor = targetClass.Search(pQF, false);
            try
            {
                IFeature aFea = null;
                while ((aFea = pCursor.NextFeature()) != null)
                {
                    lstSelFeas.Add(aFea);
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
            return lstSelFeas;
        }

        public static ArrayList getFeaturesBySql(IFeatureClass targetClass, IQueryFilter pQF)
        {
            ArrayList lstSelFeas = new ArrayList();
            //IQueryFilter pQF = new QueryFilterClass();
            //pQF.WhereClause = where;
            IFeatureCursor pCursor = targetClass.Search(pQF, false);
            try
            {
                IFeature aFea = null;
                while ((aFea = pCursor.NextFeature()) != null)
                {
                    lstSelFeas.Add(aFea);
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
            return lstSelFeas;
        }

        public static IFeature GetFirstFeature(IFeatureClass pFC, IQueryFilter pQF)
        {
            IFeature aFea = null;
            IFeatureCursor pCursor = null;
            pCursor = pFC.Search(pQF, false);
            aFea = pCursor.NextFeature();
            OtherHelper.ReleaseComObject(pCursor);
            return aFea;
        }


        /// <summary>
        /// 返回第一个要素
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IFeature getFirstFeatureBySql(IFeatureClass targetClass, string where)
        {
            IFeature aFea = null;
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            IFeatureCursor pCursor = targetClass.Search(pQF, false);
            try
            {
                aFea = pCursor.NextFeature();
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
            return aFea;
        }


        /// <summary>
        /// 获取边界外图斑要素，且与行政区交于一条线的
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="xzqGeo"></param>
        /// <returns></returns>
        public static List<IFeature> getInsectXzqLIneFeature(IFeatureClass targetClass, IGeometry xzqGeo)
        {
            List<IFeature> lstSelFeas = new List<IFeature>();
            ITopologicalOperator pTop = xzqGeo as ITopologicalOperator;
            IGeometry pJX = pTop.Boundary;
            ITopologicalOperator pJxTop = pJX as ITopologicalOperator;
            //获取与行政区边界 相交的要素，
            ISpatialFilter pSR = new SpatialFilterClass();
            pSR.Geometry = pJX;
            pSR.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
            IFeature aFea = pCursor.NextFeature();
            while (aFea != null)
            {

                //如果交与一条线,且不包含
                if (!GeometryHelper.IsContain(xzqGeo, aFea.ShapeCopy))
                {
                    lstSelFeas.Add(aFea);
                    
                }
                aFea = pCursor.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pSR);
            

            return lstSelFeas;
        }

        /// <summary>
        /// 获取与行政区边界相交于一条线，并且不包含的 要素
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="xzqGeo"></param>
        /// <returns></returns>
        public static List<IFeature> getFeatureByBoundaryIntersect(IFeatureClass targetClass, IGeometry xzqGeo)
        {
            List<IFeature> lst = new List<IFeature>();
            ITopologicalOperator pTop = xzqGeo as ITopologicalOperator;
            IGeometry pXzqJX = pTop.Boundary;
            ITopologicalOperator pJxTop = pXzqJX as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {

                ISpatialFilter pSR = new SpatialFilterClass();
                pSR.Geometry = pXzqJX;
                pSR.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                while (aFea != null)
                {
                   
                    IGeometry intersecLine=pJxTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                    if (intersecLine.IsEmpty)
                    {
                        aFea = pCursor.NextFeature();
                        continue;
                    }
                    
                    //如果交与一条线,且不包含
                    if (GeometryHelper.IsContain(xzqGeo, aFea.ShapeCopy))
                    {
                        aFea = pCursor.NextFeature();
                        continue;
                    }                  

                    lst.Add(aFea);
                    aFea = pCursor.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSR);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
            return lst;
        }

        /// <summary>
        /// 邻接要素
        /// </summary>
        /// <param name="pFC"></param>
        /// <param name="geo"></param>
        /// <returns></returns>
        public static  ArrayList getTouchedFeature(IFeatureClass pFC, IGeometry geo)
        {
            //邻接的 要素
            ArrayList ar = new ArrayList();
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
            pSF.Geometry = geo;
            IFeatureCursor pCursor = pFC.Search(pSF as IQueryFilter, false);
            IFeature aFea = null;
            try
            {
                while ((aFea = pCursor.NextFeature()) != null)
                {
                    ar.Add(aFea);

                }
            }
            catch { }
            finally
            {
               RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
               ////垃圾回收  
               
            }
            return ar;
        }

        /// <summary>
        /// 获取所有要素类,根据空间关系
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="pGeo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static List<IFeature> getFeaturesByGeo(IFeatureClass targetClass, IGeometry pGeo, esriSpatialRelEnum rel)
        {
            List<IFeature> lst = new List<IFeature>();
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                
                ISpatialFilter pSR = new SpatialFilterClass();
                pGeo.Project((targetClass as IGeoDataset).SpatialReference);
                pSR.Geometry = pGeo;
                pSR.SpatialRel = rel;
                IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                while (aFea != null)
                {
                    lst.Add(aFea);
                    aFea = pCursor.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSR);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
            return lst;
        }



        /// <summary>
        /// 获取所有要素类,根据空间关系
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="pGeo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public static List<IFeature> getFeatures(IFeatureClass targetClass, IGeometry pGeo, esriSpatialRelEnum rel,string where)
        {
            List<IFeature> lst = new List<IFeature>();
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {

                ISpatialFilter pSR = new SpatialFilterClass();
                pSR.Geometry = pGeo;
                pSR.SpatialRel = rel;
                pSR.WhereClause = where;

                IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                while (aFea != null)
                {
                    lst.Add(aFea);
                    aFea = pCursor.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSR);
                

            }
            return lst;
        }
        

        /// <summary>
        /// 根据面 获取 界线
        /// </summary>
        /// <param name="envGeo"></param>
        /// <param name="jxClass"></param>
        /// <returns></returns>
        public static List<IFeature> GetJxByPolygon(IGeometry envGeo, IFeatureClass jxClass)
        {
            List<IFeature> result = new List<IFeature>();
            ITopologicalOperator pTop = envGeo as ITopologicalOperator;
            IFeatureLayer jxLayer = new FeatureLayerClass();
            jxLayer.FeatureClass = jxClass;
            IIdentify idJxs = jxLayer as IIdentify;
            IArray arJxs = idJxs.Identify(envGeo);
            if (arJxs == null)
                return result;
            for (int i = 0; i < arJxs.Count; i++)
            {
                IFeatureIdentifyObj obj = arJxs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                IFeature aFeature = aRow.Row as IFeature;
                //如果相交，并且维度为1
                IGeometry tmpGeo = pTop.Intersect(aFeature.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                if (!tmpGeo.IsEmpty)
                {
                    if (!result.Contains(aFeature))
                    {
                        result.Add(aFeature);
                    }
                }
            }


            return result;
        }
    }
}
