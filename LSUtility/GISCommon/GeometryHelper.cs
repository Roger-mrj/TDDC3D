using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

using RCIS.Utility;

namespace RCIS.GISCommon
{
    public class GeometryHelper
    {
        public static double _tolerance = 0.0001;
        /// <summary>
        /// 点构成面
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static IPolygon pts2polygon(List<IPoint> pts)
        {
            Ring ring1 = new RingClass();
            object missing = Type.Missing;
            foreach (IPoint aPt in pts)
            {
                ring1.AddPoint(aPt, ref missing, ref missing);
            }
            
            IGeometryCollection pointPolygon = new PolygonClass();
            pointPolygon.AddGeometry(ring1 as IGeometry, ref missing, ref missing);
            IPolygon polyGonGeo = pointPolygon as IPolygon;
            polyGonGeo.SimplifyPreserveFromTo();
            return polyGonGeo;
        }


        /// 返回点到图形之间的垂直距离
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="pGeo"></param>
        /// <returns></returns>
        public static double getPointToGeoDis(IPoint pPoint, IGeometry pGeo)
        {
            IProximityOperator Pro = pPoint as IProximityOperator;
            double dis = Pro.ReturnDistance(pGeo);
            return dis;
        }
       
        ///// <summary>
        ///// 利用两点生成一条PolyLine
        ///// </summary>
        ///// <param name="p1"></param>
        ///// <param name="p2"></param>
        ///// <returns></returns>
        //private IPolyline CreatePolyLineByTwoPoint(IPoint p1, IPoint p2)
        //{
        //    INewLineFeedback m_LineFeed = new NewLineFeedback();
        //    m_LineFeed.Start(p1);
        //    m_LineFeed.AddPoint(p2);
        //    IPolyline m_PolyLine = m_LineFeed.Stop();
        //    return m_PolyLine;
        //}

        ///// <summary>
        ///// 判断两个点是否相等
        ///// </summary>
        ///// <param name="p1"></param>
        ///// <param name="p2"></param>
        ///// <returns></returns>
        //public Boolean PointEquals(IPoint p1, IPoint p2)
        //{
        //    double x1 = p1.X;
        //    double y1 = p1.Y;
        //    double x2 = p2.X;
        //    double y2 = p2.Y;

        //    double distance = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
        //    if (distance < 0.001)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
      
        ///// <summary>
        ///// 计算两点的角度
        ///// </summary>
        ///// <param name="p1"></param>
        ///// <param name="p2"></param>
        ///// <returns></returns>
        //public double Cal2PointAngle(IPoint p1, IPoint p2)
        //{
        //    double Dx = p1.X - p2.X;
        //    double Dy = p1.Y - p2.Y;
        //    if (Dx == 0 && Dy == 0)
        //    {
        //        return -999999999; //一条边两点重合了，可以忽略不计，实际情况没有
        //    }
        //    else if (Dy < 0) //3-4 象限
        //    {
        //        return Math.PI * 2 + Math.Atan2(Dy, Dx);
        //    }
        //    else
        //    {
        //        return Math.Atan2(Dy, Dx);
        //    }
        //}


        //public static IGeometry MergeGeometry(IFeatureClass xzqClass,IQueryFilter pQF)
        //{
        //    object missing = Type.Missing;
        //    IGeometry geometryBag = new GeometryBagClass();
        //    IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            
        //    IFeatureCursor pCursor = xzqClass.Search(pQF, false);
        //    IFeature aFeature = null;
        //    while ((aFeature = pCursor.NextFeature()) != null)
        //    {
        //        geometryCollection.AddGeometry(aFeature.ShapeCopy, ref missing, ref missing);
        //    }

        //    ITopologicalOperator unionedPolygon = new PolygonClass();
        //    unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
        //    unionedPolygon.Simplify();

        //    OtherHelper.ReleaseComObject(pCursor);
        //    return unionedPolygon as IGeometry;
        //}


        /// <summary>
        /// 获取该类合要素外围线
        /// </summary>
        /// <param name="pFC"></param>
        /// <returns></returns>
        public static IGeometry MergeGeometry(IFeatureClass pFC)
        {
            object missing = Type.Missing;
            IGeometry geometryBag = new GeometryBagClass();
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;

            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = (pFC as IGeoDataset).Extent;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = pFC.Search(pSF, true);
            IFeature aFeature = null;
            while ((aFeature = pCursor.NextFeature()) != null)
            {
                geometryCollection.AddGeometry(aFeature.ShapeCopy, ref missing, ref missing);
                OtherHelper.ReleaseComObject(aFeature);
            }
            OtherHelper.ReleaseComObject(pCursor);
            //OtherHelper.ReleaseComObject(pFC);
            OtherHelper.ReleaseComObject(pSF);
            ITopologicalOperator unionedPolygon = new PolygonClass();
            unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
            unionedPolygon.Simplify();
            
            return unionedPolygon as IGeometry;
        }

        public static IGeometry MergeGeometry(IFeatureClass pFC, string sWhere)
        {
            try 
            {
                object missing = Type.Missing;
                IGeometry geometryBag = new GeometryBagClass();
                IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;

                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = (pFC as IGeoDataset).Extent;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pSF.WhereClause = sWhere;
                IFeatureCursor pCursor = pFC.Search(pSF, false);
                IFeature aFeature = null;
                while ((aFeature = pCursor.NextFeature()) != null)
                {
                    ITopologicalOperator pT = aFeature.ShapeCopy as ITopologicalOperator;
                    pT.Simplify();
                    geometryCollection.AddGeometry(pT as IGeometry, ref missing, ref missing);
                }

                ITopologicalOperator unionedPolygon = new PolygonClass();
                unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
                unionedPolygon.Simplify();

                OtherHelper.ReleaseComObject(pCursor);
                return unionedPolygon as IGeometry;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public static List<IGeometry> MultiGeometryToList(IGeometry aGeo, ISpatialReference pSpaRef)
        {
            List<IGeometry> GeoCol = new List<IGeometry>();
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
                polyGonGeo.Project(pSpaRef);

                GeoCol.Add(polyGonGeo as IGeometry);
            }
            return GeoCol;
        }

        /// <summary>
        /// 合并多个面要素 的图形
        /// </summary>
        /// <param name="inFeatures"></param>
        public static IPolygon UnionPolygon(ArrayList inFeatures)
        {
             object missing=Type.Missing;
             IGeometry geometryBag = new GeometryBagClass();
             IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
             foreach (IFeature aFea in inFeatures)
             {
                 geometryCollection.AddGeometry(aFea.ShapeCopy,ref missing,ref missing);
             }


             ITopologicalOperator union = new PolygonClass();
             union.ConstructUnion(geometryBag as IEnumGeometry);
             union.Simplify();
             IPolygon retPolygon = union as IPolygon;
             return retPolygon;
        }

        /// <summary>
        /// 合并多个面要素 的图形
        /// </summary>
        /// <param name="inFeatures"></param>
        public static IPolygon UnionPolygon(List<IFeature> inFeatures)
        {
            object missing = Type.Missing;
            IGeometry geometryBag = new GeometryBagClass();
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            foreach (IFeature aFea in inFeatures)
            {
                geometryCollection.AddGeometry(aFea.ShapeCopy, ref missing, ref missing);
            }


            ITopologicalOperator union = new PolygonClass();
            union.ConstructUnion(geometryBag as IEnumGeometry);
            IPolygon retPolygon = union as IPolygon;
            return retPolygon;
        }


        public static IPolygon UnionPolygon(List<IGeometry> inGeos)
        {
            object missing = Type.Missing;
            IGeometry geometryBag = new GeometryBagClass();
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            foreach (IGeometry  aFea in inGeos)
            {
                geometryCollection.AddGeometry(aFea, ref missing, ref missing);
            }


            ITopologicalOperator union = new PolygonClass();
            union.ConstructUnion(geometryBag as IEnumGeometry);
            IPolygon retPolygon = union as IPolygon;
            return retPolygon;
        }

        //延长线
        /// <summary>
        /// 延长线
        /// </summary>
        /// <param name="pline"></param>
        /// <returns></returns>
        public static IPolyline ExtentSplitLine(IPolyline pline)
        {//只有在分割宗地的时候才使用延长的线条.
            pline = (pline as IClone).Clone() as IPolyline;
            IPoint aFstPt = new PointClass();
            IPoint aLstPt = new PointClass();
            pline.QueryPoint(esriSegmentExtension.esriExtendTangentAtFrom, -0.1, false, aFstPt);
            pline.QueryPoint(esriSegmentExtension.esriExtendTangentAtTo, pline.Length + 0.1, false, aLstPt);
            ISegment aFstSeg = new LineClass();
            aFstSeg.FromPoint = aFstPt;
            aFstSeg.ToPoint = pline.FromPoint;

            ISegment aLstSeg = new LineClass();
            aLstSeg.FromPoint = pline.ToPoint;
            aLstSeg.ToPoint = aLstPt;
            if (!aFstSeg.IsEmpty)
            {
                (pline as ISegmentCollection).InsertSegments(0, 1, ref aFstSeg);
            }
            if (!aLstSeg.IsEmpty)
            {
                (pline as ISegmentCollection).AddSegments(1, ref aLstSeg);
            }
            return pline;
        }

        /// <summary>
        /// 判断两点是否相等
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static bool IsEquals(IPoint pt1, IPoint pt2)
        {
            ITopologicalOperator pTop = pt1 as ITopologicalOperator;
            IGeometry aGeo = pTop.Intersect(pt2, esriGeometryDimension.esriGeometry0Dimension);
            if (aGeo.IsEmpty)
                return false;
            else
                return true;
        }

        ///// <summary>
        ///// 平头buffer
        ///// </summary>
        ///// <param name="myLine">线</param>
        ///// <param name="bufferDis">buffer的距离</param>
        ///// <returns></returns>
        //public static IPolygon FlatBuffer(IPolyline myLine, double bufferDis)
        //{
        //    object o = System.Type.Missing;
        //    //分别对输入的线平移两次（正方向和负方向）
        //    IConstructCurve newCurve = new PolylineClass();
        //    newCurve.ConstructOffset(myLine, bufferDis, ref o, ref o);
        //    IPointCollection pCol = newCurve as IPointCollection;

        //    IConstructCurve newCurve2 = new PolylineClass();
        //    newCurve2.ConstructOffset(myLine, -1 * bufferDis, ref o, ref o);
        //    //把第二次平移的线的所有节点翻转
        //    IPolyline addline = newCurve2 as IPolyline;
        //    addline.ReverseOrientation();


        //    //把第二条的所有节点放到第一条线的IPointCollection里面
        //    IPointCollection pCol2 = addline as IPointCollection;
        //    pCol.AddPointCollection(pCol2);


        //    //用面去初始化一个IPointCollection
        //    IPointCollection myPCol = new PolygonClass();
        //    myPCol.AddPointCollection(pCol);
        //    //把IPointCollection转换为面
        //    IPolygon myPolygon = myPCol as IPolygon;
        //    //简化节点次序
        //    myPolygon.SimplifyPreserveFromTo();
        //    return myPolygon;
        //}

        /// <summary>
        /// 单向缓冲
        /// </summary>
        /// <param name="myLine"></param>
        /// <param name="bufferDis"></param>
        /// <param name="zf">正向 为 1，负向 -1</param>
        /// <returns></returns>
        public static IPolygon FlatBufferOneway(IPolyline myLine, double bufferDis,int zf)
        {
            object o = System.Type.Missing;
            //分别对输入的线平移
            IConstructCurve mycurve = new PolylineClass();
            mycurve.ConstructOffset(myLine, zf*bufferDis, ref o, ref o);           
            IPolyline addline = mycurve as IPolyline;
            addline.ReverseOrientation();  //新线 反转



            IPointCollection pCol =myLine  as IPointCollection;
            IPointCollection newPtCols = addline as IPointCollection;
            newPtCols.AddPointCollection(pCol);

            //用面去初始化一个IPointCollection
            IPointCollection myPCol = new PolygonClass();
            myPCol.AddPointCollection(newPtCols);
            //把IPointCollection转换为面
            IPolygon myPolygon = myPCol as IPolygon;
            //简化节点次序
            myPolygon.SimplifyPreserveFromTo();

           

            return myPolygon;
        }


        public static IPolygon ConstructPolygonByLine(IPolyline line1, IPolyline line2)
        {
            IPointCollection ptCols1 = line1 as IPointCollection;
            IPointCollection ptCol2 = line2 as IPointCollection;

            //用面去初始化一个IPointCollection
            IPointCollection myPCol = new PolygonClass();
            myPCol.AddPointCollection(ptCols1);
            myPCol.AddPointCollection(ptCol2);
            //把IPointCollection转换为面
            IPolygon myPolygon = myPCol as IPolygon;
            //简化节点次序
            myPolygon.SimplifyPreserveFromTo();
            return myPolygon;
        }

        /// <summary>
        /// 单向缓冲
        /// </summary>
        /// <param name="inLine"></param>
        /// <param name="bufferDis"></param>
        /// <param name="zf">正向 为 1，负向 -1</param>
        /// <returns></returns>
        //public static IPolygon FlatBufferOneway(IPolyline inLine, double bufferDis, int zf,IFeatureClass dltbClass)
        //{
        //    object o = System.Type.Missing;
        //    //分别对输入的线平移
        //    IConstructCurve newCurve = new PolylineClass();
        //    newCurve.ConstructOffset(inLine, zf * bufferDis, ref o, ref o);
        //    IPolyline addline = newCurve as IPolyline;
        //    addline.ReverseOrientation();  //新线 反转

        //    IPolygon myPolygon = ConstructPolygonByLine(addline, inLine);
                     

        //    ITopologicalOperator pTopNewPolygon = myPolygon as ITopologicalOperator;
        //    //这半边框与地类图斑叠加，判断是否有交于一个点的
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.Geometry = myPolygon as IGeometry;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //    IFeatureCursor pCursor = dltbClass.Search(pSF as IQueryFilter, false);
        //    IFeature aDltb = null;
        //    while ((aDltb = pCursor.NextFeature()) != null)
        //    {
        //        IGeometry intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry2Dimension);
        //        if (!intersecPoint.IsEmpty)
        //            continue;
        //        intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry1Dimension);
        //        if (!intersecPoint.IsEmpty)
        //            continue;

        //        intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry0Dimension);
        //        if (!intersecPoint.IsEmpty)
        //        {
        //            if ((intersecPoint as IPointCollection).PointCount != 1)
        //                continue;
        //            IPoint aPt = (intersecPoint as IPointCollection).get_Point(0);

        //            ITopologicalOperator pTopDltb = aDltb.Shape as ITopologicalOperator;
        //            IGeometry boundry = pTopDltb.Boundary;
        //            IConstructCurve constructCurve = new PolylineClass();
        //            bool isExtensionPerfomed = false;


                   
                    
        //            //如果交于一个点，
        //            IPolyline extendLine = null;
        //            //判断这个点 是 属于这两条线的哪一条的
        //            if (isPointOnLine(aPt,inLine))
        //            {
        //                extendLine =addline ;
        //                //延伸                       
        //                constructCurve.ConstructExtended(addline, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
        //                if (!(constructCurve as IPolyline).IsEmpty)
        //                {
        //                    extendLine = constructCurve as IPolyline;
        //                }
        //                myPolygon = ConstructPolygonByLine(extendLine, inLine);

        //            }
        //            else if ( isPointOnLine(aPt,addline))
        //            {
        //                extendLine = inLine;
        //                //延伸                        
        //                constructCurve.ConstructExtended(inLine, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
        //                if (!(constructCurve as IPolyline).IsEmpty)
        //                {
        //                    extendLine = constructCurve as IPolyline;
        //                }
        //                myPolygon = ConstructPolygonByLine(addline, extendLine);
        //            }



        //        }
        //    }
        //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pCursor);


        //    return myPolygon;
        //}




        ///// <summary>
        ///// 单向缓冲,同时延伸到 交于一个点的 地类图斑
        ///// </summary>
        ///// <param name="myLine"></param>
        ///// <param name="bufferDis"></param>
        ///// <param name="zf">正向 为 1，负向 -1</param>
        ///// <returns></returns>
        //public static IPolygon FlatBufferOneway(IPolyline myLine, double bufferDis, int zf, IGeometry targetFromGeo, IGeometry targetToGeo)
        //{
        //    object o = System.Type.Missing;
        //    //分别对输入的线平移
        //    IConstructCurve mycurve = new PolylineClass();
        //    mycurve.ConstructOffset(myLine, zf*bufferDis, ref o, ref o);           
        //    IPolyline addline = mycurve as IPolyline;
        //    addline.ReverseOrientation();  //新线 反转

        //    if (zf == 1 && targetToGeo != null)
        //    {
        //        //延伸至
        //        ITopologicalOperator pTop = targetToGeo as ITopologicalOperator;
        //        IGeometry boundry= pTop.Boundary;
        //        IConstructCurve constructCurve = new PolylineClass();
        //        bool isExtensionPerfomed = false;
        //        constructCurve.ConstructExtended(addline, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
        //        if (!(constructCurve as IPolyline).IsEmpty)
        //        {
        //            addline = constructCurve as IPolyline;
        //        }
        //    }
        //    if (zf == -1 && targetFromGeo != null)
        //    {
        //        //延伸至
        //        ITopologicalOperator pTop = targetFromGeo as ITopologicalOperator;
        //        IGeometry boundry = pTop.Boundary;
        //        ICurve boundryCurve = boundry as ICurve;

        //        IConstructCurve constructCurve = new PolylineClass();
        //        bool isExtensionPerfomed = false;
        //        constructCurve.ConstructExtended(addline, boundryCurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
        //        IPolyline newPolyline=constructCurve as IPolyline;
        //        if (!newPolyline.IsEmpty)
        //        {
        //            addline = newPolyline;
        //        }
                
        //    }



        //    IPointCollection pCol =myLine  as IPointCollection;
        //    IPointCollection newPtCols = addline as IPointCollection;
        //    newPtCols.AddPointCollection(pCol);

        //    //用面去初始化一个IPointCollection
        //    IPointCollection myPCol = new PolygonClass();
        //    myPCol.AddPointCollection(newPtCols);
        //    //把IPointCollection转换为面
        //    IPolygon myPolygon = myPCol as IPolygon;
        //    //简化节点次序
        //    myPolygon.SimplifyPreserveFromTo();
        //    return myPolygon;
        //}

        public static byte[] SaveGeoToStream(IGeometry  pGeo)
        {
            byte[] resultBT = null;
            if (pGeo is IPersistStream)
            {
                IPersistStream ps = pGeo as IPersistStream;
                XMLStreamClass stream = new XMLStreamClass();
                ps.Save(stream, 0);
                resultBT = stream.SaveToBytes();
            }
            return resultBT;
        }
        public static void LoadGeoFromStream(IGeometry  pGeo, byte[] paramLayerContent)
        {
            //if (pGeo == null
            //    || paramLayerContent == null
            //    || paramLayerContent.Length == 0)
            //{
            //    return;
            //}
            try
            {
                IPersistStream ps = pGeo as IPersistStream;
                XMLStreamClass stream = new XMLStreamClass();
                stream.LoadFromBytes(ref paramLayerContent);
                ps.Load(stream);
            }
            catch(Exception ex) { }
        }


        public static bool IsCCW(IPolygon py)
        {
            IPointCollection ptCol = py as IPointCollection;
            int ptCount = ptCol.PointCount;
            IPoint prePt = null;
            IPoint curPt = null;
            for (int pi = 0; pi < ptCount; pi++)
            {
                IPoint aPt = ptCol.get_Point(pi);
                if (curPt == null)
                {
                    curPt = aPt;

                }

                if (aPt.Y >= curPt.Y)
                {
                    curPt = aPt;
                    if (pi > 0) prePt = ptCol.get_Point(pi - 1);
                    else prePt = ptCol.get_Point(ptCount - 1);
                }

            }
            double dx = curPt.X - prePt.X;
            return dx >= 0;
        }

        public static string ShapeTypeName(esriGeometryType paramGT)
        {
            string resultName = "";
            if (esriGeometryType.esriGeometryPoint == paramGT)
            {
                resultName = "点";
            }
            else if (esriGeometryType.esriGeometryPolyline == paramGT)
            {
                resultName = "线";
            }
            else if (esriGeometryType.esriGeometryPolygon == paramGT)
            {
                resultName = "面";
            }
            return resultName;
        }

        public static string CreateSpatialReferenceString(ISpatialReference sr)
        {
            string srStr = null;
            try
            {
                if (sr != null && (sr is IProjectedCoordinateSystem))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("PROJCS[");

                    IProjectedCoordinateSystem prjCS = sr as IProjectedCoordinateSystem;
                    string prjName = sr.Name;
                    builder.Append("\"" + prjName + "\",");
                    #region 地理坐标系统
                    IGeographicCoordinateSystem gcs = prjCS.GeographicCoordinateSystem;
                    string gcsName = gcs.Name;
                    builder.Append("GEOGCS[");
                    builder.Append("\"" + gcsName + "\",");

                    #region 基准面

                    IDatum datum = gcs.Datum;
                    string datumName = datum.Name;
                    builder.Append("DATUM[");
                    builder.Append("\"" + datumName + "\",");
                    ISpheroid spheroid = datum.Spheroid;
                    string spheroidName = spheroid.Name;
                    builder.Append("SPHEROID[");
                    builder.Append("\"" + spheroidName + "\",");
                    double majorRadius = spheroid.SemiMajorAxis;
                    builder.Append(majorRadius);
                    builder.Append(",");
                    double inverseFlat = 1 / spheroid.Flattening;
                    builder.Append(inverseFlat);
                    builder.Append("]],");
                    #endregion
                    #region 中央子午线
                    IPrimeMeridian primeMeridian = gcs.PrimeMeridian;
                    string pmName = primeMeridian.Name;
                    builder.Append("PRIMEM[");
                    builder.Append("\"" + pmName + "\",");
                    double longtitude = primeMeridian.Longitude;
                    builder.Append(longtitude);

                    builder.Append("],");
                    #endregion
                    builder.Append("UNIT[");
                    IAngularUnit angUnit = gcs.CoordinateUnit;
                    string angUnitName = angUnit.Name;
                    double angUnitNum = angUnit.RadiansPerUnit;
                    builder.Append("\"Degree\",");
                    builder.Append(angUnitNum);
                    builder.Append("]],");
                    #endregion
                    IProjection projection = prjCS.Projection;
                    string prjectionName = projection.Name;
                    builder.Append("PROJECTION");
                    builder.Append("[" + prjectionName + "],");

                    double false_e = prjCS.FalseEasting;
                    double false_n = prjCS.FalseNorthing;
                    double center_meridian = prjCS.get_CentralMeridian(true);
                    double scale_factor = prjCS.ScaleFactor;
                    IProjectedCoordinateSystem2 prjCS2 = prjCS as IProjectedCoordinateSystem2;
                    double latitude_origin = prjCS2.LatitudeOfOrigin;
                    ILinearUnit linearUnit = prjCS.CoordinateUnit;
                    double linearUnitNum = linearUnit.MetersPerUnit;
                    builder.Append("PARAMETER[\"False_Easting\",");
                    builder.Append(false_e);
                    builder.Append("],");

                    builder.Append("PARAMETER[\"False_Northing\",");
                    builder.Append(false_n);
                    builder.Append("],");

                    builder.Append("PARAMETER[\"Central_Meridian\",");
                    builder.Append(center_meridian);
                    builder.Append("],");

                    builder.Append("PARAMETER[\"Scale_Factor\",");
                    builder.Append(scale_factor);
                    builder.Append("],");

                    builder.Append("PARAMETER[\"Latitude_Of_Origin\",");
                    builder.Append(latitude_origin);
                    builder.Append("],");

                    builder.Append("UNIT[\"Meter\",");
                    builder.Append(linearUnitNum);
                    builder.Append("]]");
                    srStr = builder.ToString();
                }
            }
            catch (Exception ex)
            {
                srStr = null;
            }
            return srStr;

        }


        public static IPoint ParsePoint(string aLine, bool hasPtNO, bool hasZ, char splitChar)
        {
            if (aLine == null) return null;

            if (splitChar.Equals(" "))
            {
                aLine = TextHelper.Compress(aLine);
            }
            string splitStr = "" + splitChar;
            string[] aTextAry = aLine.Split(splitStr.ToCharArray());
            if (aTextAry.Length < 2) return null;
            IPoint resultPt = null;
            #region 解析点
            double ptx = 0, pty = 0, ptz = 0;
            if (hasPtNO)
            {
                if (aTextAry.Length < 3) return null;
                ptx = TextHelper.ParseDouble(aTextAry[1], double.NaN);
                pty = TextHelper.ParseDouble(aTextAry[2], double.NaN);

                if (hasZ && aTextAry.Length >= 4)
                {
                    ptz = TextHelper.ParseDouble(aTextAry[3], 0);
                }

            }
            else
            {
                ptx = TextHelper.ParseDouble(aTextAry[0], double.NaN);
                pty = TextHelper.ParseDouble(aTextAry[1], double.NaN);

                if (hasZ && aTextAry.Length >= 3)
                {
                    ptz = TextHelper.ParseDouble(aTextAry[2], 0);
                }
            }
            if (double.IsNaN(ptx)
                || double.IsNaN(pty)
                || double.IsNaN(ptz))
            {
                return null;
            }
            else
            {
                resultPt = new PointClass();
                resultPt.PutCoords(ptx, pty);
                resultPt.Z = ptz;
            }
            #endregion
            return resultPt;
        }

        public static MultipointClass ReadFromFile(string pFile
            , bool pHasZ, bool pHasLineNO, char splitChar)
        {
            MultipointClass aMP = new MultipointClass();
            if (pHasZ) (aMP as IZAware).ZAware = true;
            IPointCollection rPC = aMP as IPointCollection;
            StreamReader aReader = null;
            try
            {
                aReader = new StreamReader(pFile);
                string aLine = aReader.ReadLine();
                while (aLine != null)
                {
                    if (!aLine.Trim().Equals(""))
                    {
                        IPoint aPt = ParsePoint(aLine, pHasLineNO, pHasZ, splitChar);
                        if (aPt != null && !aPt.IsEmpty)
                        {
                            rPC.AddPoints(1, ref aPt);
                        }
                    }
                    aLine = aReader.ReadLine();
                }
            }
            catch (Exception ex) { }
            if (aReader != null)
            {
                aReader.Close();
            }
            return aMP;
        }


        public static String FormatPoint(IPoint aPt, bool hasZ, char splitChar)
        {
            if (aPt == null && aPt.IsEmpty) return "";
            StringBuilder aBuilder = new StringBuilder();
            aBuilder.Append(aPt.X).Append(splitChar)
            .Append(aPt.Y);
            if (hasZ)
            {
                aBuilder.Append(splitChar)
                    .Append(aPt.Z);
            }
            string result = aBuilder.ToString();
            return result;
        }

        /// <summary>
        /// 判断点是否是图形的结点
        /// </summary>
        /// <param name="pHitGeom"></param>
        /// <param name="pHitPt"></param>
        /// <param name="pSR"></param>
        /// <returns></returns>
        public static bool IsVextexOf(IGeometry pHitGeom
            , IPoint pHitPt, ISpatialReference pSR)
        {
            if (pHitGeom == null || pHitGeom.IsEmpty
                || pHitPt == null || pHitGeom.IsEmpty)
            {
                return false;
            }
            if (pSR != null)
            {
                pHitGeom.SpatialReference = pSR;
                pHitPt.SpatialReference = pSR;
                pHitGeom.SnapToSpatialReference();
                pHitPt.SnapToSpatialReference();
            }
            if (pHitGeom is IPoint)
            {
                return PointEquals(pHitGeom as IPoint
                , pHitPt, 0.005);
            }
            else if (pHitGeom is IPointCollection)
            {
                IPointCollection ptCol = pHitGeom as IPointCollection;
                for (int pi = 0; pi < ptCol.PointCount; pi++)
                {
                    IPoint aPt = ptCol.get_Point(pi);
                    if (PointEquals(aPt, pHitPt, 0.005))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 在SplitePoint处打断线要素的图形，并去除新的线
        /// </summary>
        /// <param name="LineCurve">传入的线</param>
        /// <param name="SplitePoint">线上的一点</param>
        /// <returns></returns>
        public static IPolyline[] SpliteLineAtPoint1(IPolyline LineCurve, IPoint SplitePoint)
        {
            IPolyline[] Lines = new IPolyline[2];
            bool isSplit;
            int splitIndex, segIndex;
            LineCurve.SplitAtPoint(SplitePoint, true, false, out isSplit, out splitIndex, out segIndex);
            if (isSplit)
            {
                IPolyline newLine = new PolylineClass();
                ISegmentCollection lineSegCol = (ISegmentCollection)LineCurve;
                ISegmentCollection newSegCol = (ISegmentCollection)newLine;
                object o = Type.Missing;
                for (int j = segIndex; j < lineSegCol.SegmentCount; j++)
                {
                    newSegCol.AddSegment(lineSegCol.get_Segment(j), ref o, ref o);
                }
                lineSegCol.RemoveSegments(segIndex, lineSegCol.SegmentCount - segIndex, true);
                lineSegCol.SegmentsChanged();
                newSegCol.SegmentsChanged();
                IPolyline oldLine = lineSegCol as IPolyline;
                newLine = newSegCol as IPolyline;
                Lines[0] = newLine;
                Lines[1] = oldLine;
            }
            return Lines;
        }

       

        /// <summary>
        /// 打断 线
        /// </summary>
        /// <param name="aLine"></param>
        /// <param name="lstPts"></param>
        /// <returns></returns>
        public static List<IPolyline> SplitALineAtPoints(IPolyline aLine, List<IPoint> lstPts)
        {
            List<IPolyline> geoList = new List<IPolyline>();
            IPolycurve aCurve = aLine as IPolycurve;
            List<IPoint> sortPoints = new List<IPoint>();
            #region //点排序

            List<double> lstPtDistance = new List<double>();
            for (int i = 0; i < lstPts.Count; i++)
            {
                IPoint curPt = lstPts[i];

                IPoint outPt = null;
                double distanceAlongcurve = 0;
                double distanceFromCurve = 0;
                bool isright = false;
                bool asRatio = false;

                aCurve.QueryPointAndDistance(esriSegmentExtension.esriExtendAtFrom, curPt, asRatio, outPt, ref distanceAlongcurve, ref distanceFromCurve, ref isright);
                lstPtDistance.Add(distanceAlongcurve);
            }
            //按到from点距离排序
            while (lstPts.Count > 0)
            {
                double dMin = lstPtDistance[0];
                int index = 0;
                for (int i = 1; i < lstPts.Count; i++)
                {
                    if (dMin > lstPtDistance[i])
                    {
                        dMin = lstPtDistance[i];
                        index = i;
                    }

                }
                sortPoints.Add(lstPts[index]);
                lstPtDistance.RemoveAt(index);
                lstPts.RemoveAt(index);
            }
            #endregion
                     

            //按照次序 进行切割 方法1
            #region
            bool projectPoint = false;
            bool createPart = true;

            bool isSplitted;
            int newPartIndex;
            int newSegIndex;
            while (sortPoints.Count > 0)
            {
                IPoint splitPt = sortPoints[0];
                aCurve.SplitAtPoint(splitPt, projectPoint, createPart, out  isSplitted, out newPartIndex, out newSegIndex);

                IGeometryCollection geoColl = aCurve as IGeometryCollection;
                IPolyline leftLine = new PolylineClass();
                IPolyline rightLine = new PolylineClass();

                if (isSplitted)
                {

                    if (geoColl.GeometryCount == 2)
                    {

                        ISegmentCollection seg = geoColl.get_Geometry(0) as ISegmentCollection;
                        (leftLine as ISegmentCollection).AddSegmentCollection(seg);
                        ISegmentCollection seg2 = geoColl.get_Geometry(1) as ISegmentCollection;
                        (rightLine as ISegmentCollection).AddSegmentCollection(seg2);
                        if (rightLine.Length > 0.01 && leftLine.Length > 0.01)
                        {
                            geoList.Add(leftLine);
                            aCurve = rightLine as IPolycurve;

                        }
                    }
                    else
                    {
                        //不知道为什么，都不对
                        IPolyline newLine = new PolylineClass();
                        ISegmentCollection lineSegCol = (ISegmentCollection)aCurve;
                        ISegmentCollection newSegCol = (ISegmentCollection)newLine;
                        object o = Type.Missing;
                        for (int j = newSegIndex; j < lineSegCol.SegmentCount; j++)
                        {
                            newSegCol.AddSegment(lineSegCol.get_Segment(j), ref o, ref o);
                        }
                        lineSegCol.RemoveSegments(newSegIndex, lineSegCol.SegmentCount - newSegIndex, true);
                        lineSegCol.SegmentsChanged();
                        newSegCol.SegmentsChanged();
                        IPolyline oldLine = lineSegCol as IPolyline;
                        newLine = newSegCol as IPolyline;

                        geoList.Add(oldLine);
                        aCurve = newLine;
                    }

                }
                sortPoints.RemoveAt(0);



            }
            if (!(aCurve as IGeometry).IsEmpty)
            {

                geoList.Add(aCurve as IPolyline);
            }
            #endregion

            //while (sortPoints.Count > 0)
            //{
            //    IPoint aTmpPt = sortPoints[0] as IPoint;
            //    IPolyline[] newLines = SpliteLineAtPoint1(aCurve as IPolyline, aTmpPt);
            //    //返回的是两条新的线
            //    sortPoints.RemoveAt(0);
            //    if (newLines[0] != null && newLines[1] != null)
            //    {
            //        aCurve = newLines[1];
            //        geoList.Add(newLines[0]);
            //    }

            //}
            //if (!(aCurve as IGeometry).IsEmpty)
            //{
                
            //    geoList.Add(aCurve as IPolyline);
            //}
            return geoList;


        }

        //判断两个点是否相等
        public static bool PointEquals(IPoint p1, IPoint p2)
        {
            double x1 = p1.X;
            double y1 = p1.Y;
            double x2 = p2.X;
            double y2 = p2.Y;

            double distance = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
            if (distance < _tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断两个点是否可以认为一致.根据两点之间的距离计算。
        /// </summary>
        /// <param name="firstPt"></param>
        /// <param name="secondPt"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool PointEquals(IPoint firstPt
            , IPoint secondPt
            , double tolerance
            )
        {
            if (firstPt == null || firstPt.IsEmpty
                || secondPt == null || secondPt.IsEmpty)
            {
                return false;
            }
            double deltaX = Math.Pow(firstPt.X - secondPt.X, 2);
            double deltaY = Math.Pow(firstPt.Y - secondPt.Y, 2);
            tolerance = Math.Abs(tolerance);
            double distance = Math.Sqrt(deltaX + deltaY);
            return distance < tolerance;
        }



        /// <summary>
        /// 环生成面
        /// </summary>
        /// <param name="aring"></param>
        /// <returns></returns>
        public static  IGeometry Ring2Polygon(IRing aring)
        {
            IPolygon4 polygon = new PolygonClass();
            IGeometryCollection pGC = polygon as IGeometryCollection;
            pGC.AddGeometry(aring);
            return polygon;
            
        }

        /// <summary>
        /// 判断两条边是否相同
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static bool LineEquals2(IPolyline line1, IPolyline line2)
        {
            IRelationalOperator pRO = line1 as IRelationalOperator;
            return pRO.Equals(line2);
        }

        /// <summary>
        /// 判断两条线是否相同
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns></returns>
        public static bool LineEquals(IPolyline line1, IPolyline line2)
        {
            try
            {
                if (line1.Length < 0.00001 && line2.Length < 0.00001)
                {
                    //长度太小，
                    return true;
                }
                //如果相交部分的 长度 等于 line1 ，line2长度，则认为相等
                ITopologicalOperator pTop = line1 as ITopologicalOperator;
                IGeometry interGeo = pTop.Intersect(line2 as IGeometry, esriGeometryDimension.esriGeometry1Dimension);
                if (interGeo == null)
                    return false;
                if (interGeo.IsEmpty)
                    return false;
                IPolyline interLine = interGeo as IPolyline;
                if ((interLine.Length - line1.Length < 0.0001) && (interLine.Length - line2.Length < 0.0001))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 判断面是否完全包含 该线
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="elseGeo"></param>
        /// <returns></returns>
        public static bool IsContain(IGeometry fill, IGeometry elseGeo)
        {
            IRelationalOperator pRel = fill as IRelationalOperator;
            if (pRel.Contains(elseGeo))
                return true;
            else
                return false;

        }

        /// <summary>
        /// 判断 线是否在面的 右面,guojie xiugai
        /// </summary>
        /// <param name="line"></param>
        /// <param name="fill"></param>
        /// <returns></returns>
        public static bool IsRight(IGeometry line, IGeometry fill)
        {
            bool isRight = true;
            ICurve aCurve = line as ICurve;

            ILine normal = new LineClass();
            aCurve.QueryNormal(esriSegmentExtension.esriNoExtension, 0.5, false, 0.1, normal);

            //如果 面与该normal交于一条线，或者包含，则为右边
            IPointCollection ptcol = new PolylineClass();
            object o = Type.Missing;
            IRelationalOperator pRel = fill as IRelationalOperator;
            ITopologicalOperator pTop = fill as ITopologicalOperator;
            ptcol.AddPoint(normal.FromPoint, ref o, ref o);
            ptcol.AddPoint(normal.ToPoint, ref o, ref o);
            
            IPolyline polyline = ptcol as IPolyline;
            if (pRel.Contains(polyline))
            {
                isRight = true;
            }
            else
            {
                IGeometry pInterGeo = pTop.Intersect(polyline as IGeometry, esriGeometryDimension.esriGeometry1Dimension);
                if (!pInterGeo.IsEmpty)
                    isRight = true;
                else
                    isRight = false;

            }

            //if (pRel.Contains(normal as IGeometry) || pTop.Intersect(normal as IGeometry, esriGeometryDimension.esriGeometry1Dimension).IsEmpty == false)
            //{
            //    isRight = true;
            //}
            //else
            //{
            //    isRight = false;
            //}


            //IArea pArea = fill  as IArea;
            //IPoint inPoint = pArea.Centroid;
            //IPoint outPoint = new PointClass();
            //ICurve pCurve = line  as ICurve;
            //double distanceFromCurve = 0;
            //double distanceAlongCurve = 0;

            //pCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension,
            //    inPoint, true, outPoint, ref distanceAlongCurve, ref distanceFromCurve, ref isRight);
            //return isRight;
            return isRight;
        }

        public static List<IFeature> CutLine(IFeature aLineFea, IPointCollection ptCol)
        {
            List<IFeature> resultFeaList = new List<IFeature>();
            #region 多点分割
            IPolyline lineGeo = aLineFea.ShapeCopy as IPolyline;
            IPolyline leftLine, rightLine;
            leftLine = lineGeo as IPolyline;
            rightLine = lineGeo as IPolyline;
            ArrayList resultLineList = new ArrayList();
            resultLineList.Add(lineGeo);
            for (int pi = 0; pi < ptCol.PointCount; pi++)
            {
                IPoint aPt = ptCol.get_Point(pi);
                CutLine(resultLineList, aPt);
            }
            #endregion

            if (resultLineList.Count > 0)
            {
                aLineFea.Shape = resultLineList[0] as IGeometry;
                aLineFea.Store();
                resultFeaList.Add(aLineFea);
            }

            for (int li = 1; li < resultLineList.Count; li++)
            {
                IFeature newFea = (aLineFea.Table as IFeatureClass).CreateFeature();
                newFea.Shape = resultLineList[li] as IGeometry;
                newFea.Store();
                resultFeaList.Add(newFea);
            }
            return resultFeaList;

        }
        public static void CutLine(ArrayList aLineList, IPoint aPt)
        {
            for (int li = 0; li < aLineList.Count; li++)
            {
                IPolyline aLine = aLineList[li] as IPolyline;

                IPolyline leftLine = null, rightLine = null;
                if (CutLine(aLine, aPt, out leftLine, out rightLine))
                {
                    aLineList[li] = leftLine;
                    aLineList.Insert(li + 1, rightLine);
                    li++;
                }
            }
        }
        public static bool CutLine(IPolyline aLine, IPoint aPt
            , out IPolyline leftLine, out IPolyline rightLine)
        {
            leftLine = new PolylineClass();
            rightLine = new PolylineClass();
            if (aLine == null || aLine.IsEmpty
                || aPt == null || aPt.IsEmpty)
            {
                return false;
            }

            IPolycurve aCurver = (aLine as IClone).Clone() as IPolycurve;
            bool splitHappened = false;
            int newPartIndex = 0;
            int newSegIndex = 0;
            aCurver.SplitAtPoint(aPt, true, true, out splitHappened
            , out newPartIndex, out newSegIndex);
            if (splitHappened)
            {
                IGeometryCollection aGC = aCurver as IGeometryCollection;
                for (int gi = 0; gi < newPartIndex; gi++)
                {
                    IGeometry aPart = aGC.get_Geometry(gi);
                    (leftLine as IGeometryCollection).AddGeometries(1, ref aPart);
                }
                for (int gi = newPartIndex; gi < aGC.GeometryCount; gi++)
                {
                    IGeometry aPart = aGC.get_Geometry(gi);
                    (rightLine as IGeometryCollection).AddGeometries(1, ref aPart);
                }
            }
            return splitHappened;
        }
        
        /// <summary>
        /// 线将面进行分割，返回左右两个面
        /// </summary>
        /// <param name="pPoly"></param>
        /// <param name="pLine"></param>
        /// <param name="pLeftPoly"></param>
        /// <param name="pRightPoly"></param>
        /// <returns></returns>
        public static bool CutGeometry(IPolygon pPoly, IPolyline pLine, out IPolygon pLeftPoly, out IPolygon pRightPoly)
        {
            pLeftPoly = new PolygonClass();
            pRightPoly = new PolygonClass();
            try
            {
                IRelationalOperator relOp = pLine as IRelationalOperator;
                ArrayList leftList = new ArrayList();
                ArrayList rightList = new ArrayList();
                IGeometryCollection gc = pPoly as IGeometryCollection;
                for (int gi = 0; gi < gc.GeometryCount; gi++)
                {
                    IGeometry leftGeom = null, rightGeom = null;
                    IGeometry aGeom = gc.get_Geometry(gi);
                    IPolygon tempP = new PolygonClass();
                    (tempP as ISegmentCollection).AddSegmentCollection(aGeom as ISegmentCollection);
                    tempP.Close();
                    (tempP as ITopologicalOperator).Simplify();
                    if (!relOp.Disjoint(tempP))
                    {
                        try
                        {
                            (tempP as ITopologicalOperator).Cut(pLine, out leftGeom, out rightGeom);
                            if (leftGeom != null) leftList.Add(leftGeom);
                            if (rightGeom != null) rightList.Add(rightGeom);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        leftList.Add(tempP);
                    }
                }

                foreach (IGeometry aGeom in leftList)
                {
                    (pLeftPoly as IGeometryCollection).AddGeometryCollection
                    (aGeom as IGeometryCollection);
                }
                foreach (IGeometry aGeom in rightList)
                {
                    (pRightPoly as IGeometryCollection).AddGeometryCollection
                    (aGeom as IGeometryCollection);
                }
                (pLeftPoly as ITopologicalOperator).Simplify();
                (pRightPoly as ITopologicalOperator).Simplify();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void SnapToSR(IGeometry pGeom, ISpatialReference pSR)
        {
            try
            {
                if (pGeom == null || pGeom.IsEmpty) return;
                if (pSR == null) return;
                pGeom.SpatialReference = pSR;
                pGeom.SnapToSpatialReference();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 判断三个点是否在同一条直线上。
        /// 这里使用的是面积判断法。如果三个点组成的三角形的面积小于1
        /// 就认定为是在一条线上。面积使用的是海伦公式。
        /// 经测试比较有效。
        /// </summary>
        /// <param name="pFirstPt"></param>
        /// <param name="pSecondPt"></param>
        /// <param name="pThirdPt"></param>
        /// <returns></returns>
        public static bool OnLine(IPoint pFirstPt, IPoint pSecondPt, IPoint pThirdPt, ISpatialReference pSR)
        {
            if (pFirstPt == null || pSecondPt == null || pThirdPt == null)
                return false;
            if (pFirstPt.IsEmpty || pSecondPt.IsEmpty || pThirdPt.IsEmpty)
                return false;
            SnapToSR(pFirstPt, pSR);
            SnapToSR(pSecondPt, pSR);
            SnapToSR(pThirdPt, pSR);
            double deltaX = Math.Pow(pFirstPt.X - pSecondPt.X, 2);
            double deltaY = Math.Pow(pFirstPt.Y - pSecondPt.Y, 2);
            double distanceFS = Math.Sqrt(deltaX + deltaY);

            deltaX = Math.Pow(pFirstPt.X - pThirdPt.X, 2);
            deltaY = Math.Pow(pFirstPt.Y - pThirdPt.Y, 2);
            double distanceFT = Math.Sqrt(deltaX + deltaY);

            deltaX = Math.Pow(pSecondPt.X - pThirdPt.X, 2);
            deltaY = Math.Pow(pSecondPt.Y - pThirdPt.Y, 2);
            double distanceST = Math.Sqrt(deltaX + deltaY);

            double circle = (distanceFS + distanceFT + distanceST) / 2;
            double area = circle * (circle - distanceFS) * (circle - distanceFT)
            * (circle - distanceST);
            area = Math.Sqrt(area);
            return   (area < 0.2);
        }
        /// <summary>
        /// 在一条180度的线上
        /// </summary>
        /// <param name="pFirstPt"></param>
        /// <param name="pSecondPt"></param>
        /// <param name="pThirdPt"></param>
        /// <param name="pSR"></param>
        /// <returns></returns>
        public static bool OnLine180(IPoint pFirstPt, IPoint pSecondPt, IPoint pThirdPt, ISpatialReference pSR)
        {
            if (pFirstPt == null || pSecondPt == null || pThirdPt == null)
                return false;
            if (pFirstPt.IsEmpty || pSecondPt.IsEmpty || pThirdPt.IsEmpty)
                return false;
            SnapToSR(pFirstPt, pSR);
            SnapToSR(pSecondPt, pSR);
            SnapToSR(pThirdPt, pSR);
            double deltaX = Math.Pow(pFirstPt.X - pSecondPt.X, 2);
            double deltaY = Math.Pow(pFirstPt.Y - pSecondPt.Y, 2);
            double distanceFS = Math.Sqrt(deltaX + deltaY);

            deltaX = Math.Pow(pFirstPt.X - pThirdPt.X, 2);
            deltaY = Math.Pow(pFirstPt.Y - pThirdPt.Y, 2);
            double distanceFT = Math.Sqrt(deltaX + deltaY);

            deltaX = Math.Pow(pSecondPt.X - pThirdPt.X, 2);
            deltaY = Math.Pow(pSecondPt.Y - pThirdPt.Y, 2);
            double distanceST = Math.Sqrt(deltaX + deltaY);

            double circle = (distanceFS + distanceFT + distanceST) / 2;
            double area = circle * (circle - distanceFS) * (circle - distanceFT)
            * (circle - distanceST);
            area = Math.Sqrt(area);
            return area < 0.000001;
        }


        /// <summary>
        /// 点 在点集合中的位置
        /// </summary>
        /// <param name="aGeo"></param>
        /// <param name="aPt"></param>
        /// <returns></returns>
        private static int pointIndex(IGeometry aGeo, IPoint aPt)
        {
            IPointCollection fillPtCols = aGeo as IPointCollection;
            int idx = -1;
            for (int i = 0; i < fillPtCols.PointCount; i++)
            {
                if (GeometryHelper.PointEquals(aPt, fillPtCols.get_Point(i)))
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }

        // <summary>  
        /// 修改要素集空间参考  
        /// </summary>  
        /// <param name="pFeatureDataset">要素集</param>  
        /// <param name="pSpatialReference">新空间参考</param>  
        public static void AlterSpatialReference(IFeatureDataset pFeatureDataset, ISpatialReference pSpatialReference)
        {
            IGeoDataset pGeoDataset = pFeatureDataset as IGeoDataset;
            IGeoDatasetSchemaEdit pGeoDatasetSchemaEdit = pGeoDataset as IGeoDatasetSchemaEdit;
            if (pGeoDatasetSchemaEdit.CanAlterSpatialReference == true)
                pGeoDatasetSchemaEdit.AlterSpatialReference(pSpatialReference);
        }

        public static void AlterSpatialReference(IFeatureDataset pFeatureDataset, ISpatialReference pSpatialReference, double dTolerance)
        {
            IGeoDataset pGeoDataset = pFeatureDataset as IGeoDataset;
            IGeoDatasetSchemaEdit pGeoDatasetSchemaEdit = pGeoDataset as IGeoDatasetSchemaEdit;
            if (pGeoDatasetSchemaEdit.CanAlterSpatialReference == true)
                pGeoDatasetSchemaEdit.AlterSpatialReference(pSpatialReference);

            



            ISpatialReferenceResolution resolutionTolerance = pSpatialReference as ISpatialReferenceResolution;
            resolutionTolerance.ConstructFromHorizon();
            resolutionTolerance.set_XYResolution(true, dTolerance / 10);
            resolutionTolerance.set_ZResolution(true, dTolerance / 10);
            resolutionTolerance.MResolution = dTolerance / 10;

            ISpatialReferenceTolerance spatialReferenceTolerance = pSpatialReference as ISpatialReferenceTolerance;
            spatialReferenceTolerance.XYTolerance = dTolerance;
            spatialReferenceTolerance.ZTolerance = dTolerance;
            spatialReferenceTolerance.MTolerance = dTolerance;



            //resolutionTolerance.SetDefaultXYResolution();

            
             
        }


        /// <summary>  
        /// 修改要素类空间参考  
        /// </summary>  
        /// <param name="pFeatureClass">要素类</param>  
        /// <param name="pSpatialReference">新空间参考</param>  
        public static void AlterSpatialReference(IFeatureClass pFeatureClass, ISpatialReference pSpatialReference)
        {
            IGeoDataset pGeoDataset = pFeatureClass as IGeoDataset;
            IGeoDatasetSchemaEdit pGeoDatasetSchemaEdit = pGeoDataset as IGeoDatasetSchemaEdit;
            if (pGeoDatasetSchemaEdit.CanAlterSpatialReference == true)
                pGeoDatasetSchemaEdit.AlterSpatialReference(pSpatialReference);
        }

        public static void AlterSpatialReference(IFeatureClass pFeatureClass, ISpatialReference pSpatialReference,double dTolerance)
        {
            IGeoDataset pGeoDataset = pFeatureClass as IGeoDataset;
            IGeoDatasetSchemaEdit pGeoDatasetSchemaEdit = pGeoDataset as IGeoDatasetSchemaEdit;
            if (pGeoDatasetSchemaEdit.CanAlterSpatialReference == true)
                pGeoDatasetSchemaEdit.AlterSpatialReference(pSpatialReference);

            ISpatialReferenceResolution resolutionTolerance = pSpatialReference as ISpatialReferenceResolution;
            resolutionTolerance.ConstructFromHorizon();
            resolutionTolerance.set_XYResolution(true, dTolerance / 10);
            resolutionTolerance.set_ZResolution(true, dTolerance / 10);
            resolutionTolerance.MResolution = dTolerance / 10;

            ISpatialReferenceTolerance spatialReferenceTolerance = pSpatialReference as ISpatialReferenceTolerance;
            spatialReferenceTolerance.XYTolerance = dTolerance;
            spatialReferenceTolerance.ZTolerance = dTolerance;
            spatialReferenceTolerance.MTolerance = dTolerance;

                
        }
           


        /// <summary>
        /// 获取一个要素类图形外包络线
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static IPolygon GeometryBag(IFeatureClass featureClass)
        {

            //Check input objects.
            if (featureClass == null)
            {
                return null;
            }

            IGeoDataset geoDataset = featureClass as IGeoDataset;

            //You can use a spatial filter to create a subset of features to union together. 
            //To do that, uncomment the next line, and set the properties of the spatial filter here.
            //Also, change the first parameter in the IFeatureCursor.Seach method.
            //ISpatialFilter queryFilter = new SpatialFilterClass();

            IGeometry geometryBag = new GeometryBagClass();

            //Define the spatial reference of the bag before adding geometries to it.
            geometryBag.SpatialReference = geoDataset.SpatialReference;

            //Use a nonrecycling cursor so each returned geometry is a separate object. 
            IFeatureCursor featureCursor = featureClass.Search(null, false);

            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            IFeature currentFeature = featureCursor.NextFeature();

            while (currentFeature != null)
            {
                //Add a reference to this feature's geometry to the bag.
                //Since you don't specify the before or after geometry (missing),
                //the currentFeature.Shape IGeometry is added to the end of the geometryCollection.
                object missing = Type.Missing;
                geometryCollection.AddGeometry(currentFeature.Shape, ref missing, ref missing);

                currentFeature = featureCursor.NextFeature();
            }

            // Create the polygon that will be the union of the features returned from the search cursor.
            // The spatial reference of this feature does not need to be set ahead of time. The 
            // ConstructUnion method defines the constructed polygon's spatial reference to be the 
            // same as the input geometry bag.
            ITopologicalOperator unionedPolygon = new PolygonClass();
            unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);

            return unionedPolygon as IPolygon;
        }


        
       
        
        /// <summary>
        ///  /// 粗略判断一个已知点是否在线上
        /// </summary>
        /// <param name="pPoint"></param>
        /// <param name="myLine"></param>
        /// <returns></returns>
        public static  bool isPointOnLine(IPoint pPoint, IPolyline myLine)
        {
            ITopologicalOperator topo = pPoint as ITopologicalOperator;
            IGeometry buffer = topo.Buffer(0.0001); //缓冲一个极小的距离
            topo = buffer as ITopologicalOperator;
            IGeometry  pgeo = topo.Intersect(myLine, esriGeometryDimension.esriGeometry0Dimension) ;
            bool result = false;
            if (!pgeo.IsEmpty)
            {
                
                    result = true;
            }
            return result;
        }

        public static double GetIntersectArea(IGeometry pGeo1, IGeometry pGeo2, ref IGeometry pIntersect)
        {
            ITopologicalOperator pTopoed = pGeo1 as ITopologicalOperator;
            pTopoed.Simplify();
            pGeo1.Project(pGeo2.SpatialReference);
            pIntersect = pTopoed.Intersect(pGeo2, esriGeometryDimension.esriGeometry2Dimension);
            if (pIntersect != null && pIntersect.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                pTopoed = pIntersect as ITopologicalOperator;
                if (!pTopoed.IsKnownSimple) pTopoed.Simplify();
                IArea pArea = pIntersect as IArea;
                return Math.Round(pArea.Area, 2);
            }
            else
                return 0;
        }


        /// <summary>
        /// 圆转换多边形
        /// </summary>
        /// <param name="pCircle">圆图像对象</param>
        /// <returns>多边形图像对象</returns>
        public static IPolygon ConvertCircle2Polygon(ICircularArc pCircle)
        {
            object missing = Type.Missing;
            ISegmentCollection pSegmentColl = new RingClass();
            pSegmentColl.AddSegment((ISegment)pCircle, ref missing, ref missing);
            IRing pRing = (IRing)pSegmentColl;
            pRing.Close(); //得到闭合的环
            IGeometryCollection pGeometryCollection = new PolygonClass();
            pGeometryCollection.AddGeometry(pRing, ref missing, ref missing); //环转面
            IPolygon pPolygon = (IPolygon)pGeometryCollection;
            return pPolygon;
        }
    }
}
