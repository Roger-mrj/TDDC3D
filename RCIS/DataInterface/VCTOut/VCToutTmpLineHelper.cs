using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using System.IO;

namespace RCIS.DataInterface.VCTOut
{
    public class VCToutTmpLineHelper
    {

        #region 废弃
        /// <summary>
        /// 根据面 采用空间查询方式获取 界线数据，记录在列表中
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="lineClass"></param>
        /// <param name="idorbsm"></param>
        /// <returns></returns>
        //public static  List<LineObject> getJxObjByPolygon(IGeometry geo, IFeatureClass lineClass, int idorbsm)
        //{
        //    string className = (lineClass as IDataset).Name;
            

        //    //宗地用空间关系
        //    List<LineObject> lst = new List<LineObject>();
        //    ITopologicalOperator pTop = geo as ITopologicalOperator;
        //    IGeometry pBound = pTop.Boundary;
        //    IRelationalOperator pRO = pBound as IRelationalOperator;

        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.Geometry = pBound;  //aPolygon as IGeometry;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;    //用边界 包含 关系
        //    pSF.GeometryField = lineClass.ShapeFieldName;
            
        //    IFeatureCursor pCursor = lineClass.Search(pSF as IQueryFilter, false);
        //    IFeature aLineFeature = null;
        //    try
        //    {
        //        while ((aLineFeature = pCursor.NextFeature()) != null)
        //        {
        //            //记录之
        //            LineObject lineobj = new LineObject();
        //            if (idorbsm == 0)
        //            {
        //                long fid = aLineFeature.OID;
        //                if (fid == 0)  //fid 是 0 ，这个 如何加标记
        //                {
        //                    fid = -99999999;
        //                }
        //                lineobj.GID = fid;
        //            }
        //            else
        //            {
        //                long bsm = 0;
        //                long.TryParse(FeatureHelper.GetFeatureStringValue(aLineFeature, "BSM"), out bsm);
        //                lineobj.GID = bsm;
        //            }

        //            lineobj.Shape = aLineFeature.Shape as IPolyline;
        //            lst.Add(lineobj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    }

        //    return lst;
        //}


        //public static List<LineObject> getJxObjByPolygon3(IGeometry geo, IFeatureClass lineClass, long jxStart)
        //{
            
        //    //宗地用空间关系
        //    List<LineObject> lst = new List<LineObject>();
        //    ITopologicalOperator pTop = geo as ITopologicalOperator;
        //    IGeometry pBound = pTop.Boundary;
        //    IRelationalOperator pRO = pBound as IRelationalOperator;

        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.Geometry = pBound;  
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;    //用边界 包含 关系
        //    pSF.GeometryField = lineClass.ShapeFieldName;

        //    IFeatureCursor pCursor = lineClass.Search(pSF as IQueryFilter, false);
        //    IFeature aLineFeature = null;
        //    try
        //    {
        //        while ((aLineFeature = pCursor.NextFeature()) != null)
        //        {
        //            //记录之
        //            LineObject lineobj = new LineObject();
        //            lineobj.GID = jxStart * 10 + aLineFeature.OID;  //要素序号仍然是*10+oid
        //            lineobj.Shape = aLineFeature.Shape as IPolyline;
        //            lst.Add(lineobj);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    }

        //    return lst;
        //}

        /// <summary>
        /// 根据面 采用空间查询方式获取 界线数据，记录在列表中
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="lineClass"></param>
        /// <param name="idorbsm"></param>
        /// <returns></returns>
        //public static List<LineObject> getJxObjByPolygon3(IGeometry geo, long dltbStart, IFeatureClass lineClass, int idorbsm)
        //{
        //    //宗地用空间关系
        //    List<LineObject> lst = new List<LineObject>();
        //    ITopologicalOperator pTop = geo as ITopologicalOperator;
        //    IGeometry pBound = pTop.Boundary;
        //    IRelationalOperator pRO = pBound as IRelationalOperator;


        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.Geometry = pBound;  //aPolygon as IGeometry;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;    //用边界 包含 关系
        //    pSF.GeometryField = lineClass.ShapeFieldName;

        //    IFeatureCursor pCursor = lineClass.Search(pSF as IQueryFilter, false);
        //    IFeature aLineFeature = null;
        //    try
        //    {
        //        while ((aLineFeature = pCursor.NextFeature()) != null)
        //        {
        //            //记录之
        //            LineObject lineobj = new LineObject();
        //            if (idorbsm == 0)
        //            {                        
        //                long fid = dltbStart + aLineFeature.OID;  //dltbline中线的 序号
        //                lineobj.GID = fid;
        //            }
        //            else
        //            {
        //                long bsm = 0;
        //                long.TryParse(FeatureHelper.GetFeatureStringValue(aLineFeature, "BSM"), out bsm);
        //                lineobj.GID = bsm;
        //            }

        //            lineobj.Shape = aLineFeature.Shape as IPolyline;
        //            lst.Add(lineobj);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    }

        //    return lst;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        //public static double Cal2PointAngle(IPoint p1, IPoint p2)
        //{
        //    //double Dx = p1.X - p2.X;
        //    //double Dy = p1.Y - p2.Y;
        //    //if (Dx == 0 && Dy == 0)
        //    //{
        //    //    return -999999999; //一条边两点重合了，可以忽略不计，实际情况没有
        //    //}
        //    //else if (Dy < 0) //3-4 象限
        //    //{
        //    //    return Math.PI * 2 + Math.Atan2(Dy, Dx);
        //    //}
        //    //else
        //    //{
        //    //    return Math.Atan2(Dy, Dx);
        //    //}

        //    ILine aLine = new LineClass();
        //    aLine.PutCoords(p1, p2);
        //    double angle = aLine.Angle;
        //    if (angle < 0)
        //    {
        //        angle = angle + Math.PI * 2;
        //    }

        //    return angle;  //水平夹角

        //}

        //public static LineObject getNextLineByMinAngle(List<LineObject> lineLst, IPoint prePoint, IPoint currPt, ref int fromOrTo)
        //{
        //    LineObject resultLine = null;
        //    double inAngle = Cal2PointAngle(currPt, prePoint);  //d当前点和前一个点的边 与平行线的角度
        //    double minAngleDiff = 99;

        //    foreach (LineObject line in lineLst)
        //    {
        //        IPoint fromPt = (line.Shape as IPolyline).FromPoint;
        //        IPoint toPt = (line.Shape as IPolyline).ToPoint;
        //        //有的 边 起始点和结束点 是一样的，tama de 


        //        if (GeometryHelper.PointEquals(fromPt, currPt))
        //        {
        //            IPoint secPt = (line.Shape as IPointCollection).get_Point(1);
        //            double angle2 = Cal2PointAngle(currPt, secPt);
        //            if (Math.Abs(angle2 - inAngle) < minAngleDiff)
        //            {
        //                minAngleDiff = Math.Abs(angle2 - inAngle);
        //                resultLine = line;
        //                fromOrTo = 1;
        //            }
        //        }
        //        //之所以对比了 起始点和终止点，是因为有的环绕一圈回来，起始点和终止点相同，但是与他的前一个点不在一个角度，所以 不是else
        //        if (GeometryHelper.PointEquals(currPt, toPt))
        //        {
        //            //与倒数第二个点的夹角
        //            IPoint secPt = (line.Shape as IPointCollection).get_Point((line.Shape as IPointCollection).PointCount - 2);
        //            double angle2 = Cal2PointAngle(currPt, secPt);
        //            if (Math.Abs(angle2 - inAngle) < minAngleDiff)
        //            {
        //                minAngleDiff = Math.Abs(angle2 - inAngle);
        //                resultLine = line;
        //                fromOrTo = -1;
        //            }
        //        }              


        //    }



        //    return resultLine;
        //}
             

        #endregion 

        
        public static LineObject getNextLine(List<LineObject> lineLst, IPoint prePoint, IPoint currPt, ref int fromOrTo, IGeometry afill)
        {
            List<Point3Object> lstResult = new List<Point3Object>();
            #region 找到三点对象
            for (int i = 0; i < lineLst.Count; i++)
            {
                LineObject aLineObj = lineLst[i];
                IPolyline lineShp = aLineObj.Shape as IPolyline;
                IPointCollection linePtCols=lineShp as IPointCollection;
                IPoint fromPt = lineShp.FromPoint;
                IPoint toPt = lineShp.ToPoint;      
               
                if (GeometryHelper.PointEquals(fromPt, currPt))
                {
                    //下一条边的起始点 与 传入点相同， 
                    fromOrTo =1;
                    Point3Object aPt3Obj = new Point3Object();
                    aPt3Obj.index = i;
                    aPt3Obj.fromOrTo = fromOrTo;
                    aPt3Obj.prePt=prePoint;
                    aPt3Obj.currPt=currPt;
                    aPt3Obj.nextPt = linePtCols.get_Point(1);
                    lstResult.Add(aPt3Obj);
                }
                if (GeometryHelper.PointEquals(currPt, toPt))
                {
                    //下一条边的 结束点 与传入点相同
                    fromOrTo = -1;
                    Point3Object aPt3Obj = new Point3Object();
                    aPt3Obj.index = i;
                    aPt3Obj.fromOrTo = fromOrTo;
                    aPt3Obj.prePt = prePoint;
                    aPt3Obj.currPt = currPt;
                    aPt3Obj.nextPt = linePtCols.get_Point(linePtCols.PointCount-2);
                    lstResult.Add(aPt3Obj);
                }


            }
            #endregion 

            //绝大多数情况下，应该是 1
            if (lstResult.Count == 0) return null;
            if (lstResult.Count == 1)
            {
                fromOrTo = lstResult[0].fromOrTo;
                return lineLst[lstResult[0].index]; // 返回该线段

            }
            //if (lstResult.Count == 3)
            //{
            //}
            //如果超过一个对象，则遍历找  与 外环对象相同次序的那个
            IPointCollection fillPtCols = afill as IPointCollection;

            foreach (Point3Object aObj in lstResult)
            {
                //int firstPtIdx = -1;
                //int secondPtIdx = -1;
                //int thirdPtIdx = -1;
                IPoint nextPt=aObj.nextPt;
                int num=fillPtCols.PointCount;
                

                //一种特殊情况，如果三个点分别市 倒数第2个点，第0个点，第一个点
                if (GeometryHelper.PointEquals(prePoint,fillPtCols.get_Point(num-2))
                    && GeometryHelper.PointEquals(currPt,fillPtCols.get_Point(0))
                    && GeometryHelper.PointEquals(nextPt,fillPtCols.get_Point(1)) )
                {
                    //直接认为正确
                    fromOrTo = aObj.fromOrTo;
                    return lineLst[aObj.index]; 
                }
                else if (GeometryHelper.PointEquals(nextPt, fillPtCols.get_Point(num - 2))
                    && GeometryHelper.PointEquals(currPt, fillPtCols.get_Point(0))
                    && GeometryHelper.PointEquals(prePoint, fillPtCols.get_Point(1)))
                {
                    //或者分别市第一个点，第0 个点，倒数第2个点
                    fromOrTo = aObj.fromOrTo;
                    return lineLst[aObj.index]; 
                }


                //判断三个点是否是 顺序排列在  面上的
                for (int kk = 1; kk < num - 1; kk++)
                {
                    IPoint tmpPt0 = fillPtCols.get_Point(kk - 1);
                    IPoint tmpPt1 = fillPtCols.get_Point(kk);
                    IPoint tmpPt2 = fillPtCols.get_Point(kk + 1);

                    if (GeometryHelper.PointEquals(tmpPt0, prePoint) && GeometryHelper.PointEquals(tmpPt1, currPt)
                        && GeometryHelper.PointEquals(tmpPt2, nextPt))
                    {
                        //方向一致
                        fromOrTo = aObj.fromOrTo;
                        return lineLst[aObj.index];
                    }
                    if (GeometryHelper.PointEquals(tmpPt2, prePoint) && GeometryHelper.PointEquals(tmpPt1, currPt)
                        && GeometryHelper.PointEquals(tmpPt0, nextPt))
                    {
                        fromOrTo = aObj.fromOrTo;
                        return lineLst[aObj.index];
                    }

                }

                //for (int kk = 0; kk <num-1 ; kk++)
                //{
                //    IPoint tmpPt = fillPtCols.get_Point(kk);
                //    if (GeometryHelper.PointEquals(tmpPt, prePoint))
                //    {
                //        firstPtIdx = kk;
                //    }
                //    if (GeometryHelper.PointEquals(tmpPt, currPt))
                //    {
                //        secondPtIdx = kk;
                //    }
                //    if (GeometryHelper.PointEquals(tmpPt, nextPt))
                //    {
                //        thirdPtIdx = kk;
                //    }
                //    if ((firstPtIdx > -1) && (secondPtIdx > -1) && (thirdPtIdx > -1))
                //        break;
                //}
                //if ((firstPtIdx > -1) && (secondPtIdx > -1) && (thirdPtIdx > -1))
                //{
                //    if ((secondPtIdx - firstPtIdx == 1) && (thirdPtIdx - secondPtIdx == 1))
                //    {
                //        fromOrTo = aObj.fromOrTo;
                //        return lineLst[aObj.index];
                //    }

                //    if ((firstPtIdx - secondPtIdx == 1) && (secondPtIdx-thirdPtIdx == 1))
                //    {
                //        fromOrTo = aObj.fromOrTo;
                //        return lineLst[aObj.index];
                //    }

                    
                //}
                
            }
            

            return null;
        }
    

        /// <summary>
        /// 边排序，郭杰
        /// </summary>
        /// <param name="pLineList"></param>
        /// <returns></returns>
        public static List<long> OrderEdges(List<LineObject> pLineList,IGeometry aFill)
        {           

            List<long> rList = new List<long>();
            int fromOrTo = 1; //1:from, -1 ,to 
            bool sx1 = true;
            try
            {

                while (pLineList.Count > 0)
                {
                    int lineIdx = 0;
                    for (int idx = 0; idx < pLineList.Count; idx++)
                    {
                        LineObject tmpLine = pLineList[idx];
                        IPolyline aLine = tmpLine.Shape;
                        //判断这条边的图形信息 是否在 面上，返回顺时针或者逆时针，外环与 面结点次序相同的是顺时针，内环 则是逆时针
                        if (LineInPart(aLine, aFill as IPointCollection, ref sx1))
                        {
                            //找到这条边，并且返回 线顺序
                            lineIdx = idx;
                            break;
                        }
                    }


                    LineObject aLineObj = pLineList[lineIdx];//第一条边                
                    LineInPart(aLineObj.Shape, aFill as IPointCollection, ref sx1); //第一条边是否顺时针，
                    rList.Add(aLineObj.GID * fromOrTo);  //或者内环第一条边

                    IPoint curPt = (aLineObj.Shape).ToPoint;  //终止点
                    IPointCollection ptCols = aLineObj.Shape as IPointCollection;
                    IPoint prePt = ptCols.get_Point(ptCols.PointCount - 2);// 倒数第二个点,根据前一个点和当前点判断 走向

                    pLineList.RemoveAt(lineIdx);

                    //从第一条边开始，随即的，从他的Topoint开始找
                    bool findTheEnd = false;

                    while (!findTheEnd)
                    {

                        //1表示下一条边的frompoint 与 该点相连m，-1表示下一条边的topoint与改点相连                   
                        //LineObject NextLine = getNextLineByMinAngle(pLineList, prePt, curPt, ref fromOrTo);
                        LineObject NextLine = getNextLine(pLineList, prePt, curPt, ref fromOrTo, aFill);
                        if (NextLine != null)
                        {
                            findTheEnd = false;
                            if (fromOrTo == 1)
                            {
                                rList.Add(NextLine.GID);
                                curPt = (NextLine.Shape as IPolyline).ToPoint;
                                IPointCollection nextLinePts = NextLine.Shape as IPointCollection;
                                prePt = nextLinePts.get_Point(nextLinePts.PointCount - 2);// 倒数第二个点

                                pLineList.Remove(NextLine);

                            }
                            else if (fromOrTo == -1)
                            {
                                rList.Add(-NextLine.GID);
                                curPt = (NextLine.Shape as IPolyline).FromPoint;

                                IPointCollection nextLinePts = NextLine.Shape as IPointCollection;
                                prePt = nextLinePts.get_Point(1);// 第二个点

                                pLineList.Remove(NextLine);
                            }
                        }
                        else
                        {
                            //找不到 这条边，
                            findTheEnd = true;
                        }

                        //如果找不到下一条边，则 可能是环，则 先输出个0 ，然后继续下面的循环
                        if (findTheEnd && pLineList.Count > 0)
                        {
                            rList.Add(0);

                            //内环跟外环第一条边方向相反，本来没环，作为环处理
                            if (sx1)
                            {
                                //如果顺时针
                                fromOrTo = -1;
                            }
                            else
                            {
                                fromOrTo = 1;
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return rList;
        }
        
        /// <summary>
        /// 判断这条边的起始点和终止点是否都在 点集合中，返回次序
        /// </summary>
        /// <param name="aLine"></param>
        /// <param name="ptCols"></param>
        private static bool LineInPart(IPolyline aLine, IPointCollection ptCols, ref bool sx)
        {
            IPointCollection linePoints = aLine as IPointCollection;

            //用前两个点 跟 面的其中两点的 顺序 相对比，看是否一致，根据这个判断线的走向，因为面的外环都是顺时针的
            IPoint linePt0 = linePoints.get_Point(0);
            IPoint linePt1 = linePoints.get_Point(1);  //第二个点

            for (int i = 0; i < ptCols.PointCount - 1; i++)
            {
                //如果面上紧挨着的俩点的与传入的线段俩点相同
                IPoint firstPt = ptCols.get_Point(i);
                if (GeometryHelper.PointEquals(firstPt, linePt0))
                {

                    IPoint secondPt = ptCols.get_Point(i + 1);
                    if (GeometryHelper.PointEquals(secondPt, linePt1))
                    {
                        sx = true; // 线方向与 面线方向一致 
                        return true;
                    }


                }
            }
            //如果 逆时针方向两个点与传入两个点相同
            for (int i = 0; i < ptCols.PointCount - 1; i++)
            {
                IPoint firstPt = ptCols.get_Point(i);
                if (GeometryHelper.PointEquals(firstPt, linePt1))
                {

                    IPoint secondPt = ptCols.get_Point(i + 1);
                    if (GeometryHelper.PointEquals(secondPt, linePt0))
                    {
                        sx = false; // 线方向与 面不一致,逆时针
                        return true;
                    }


                }
            }

            return false;

        }
        

        public static List<long> OrderEdgesRing(List<LineObject> pLineList, IGeometry fillGeo)
        {
            List<long> rList = new List<long>();

            try
            {
                //线找到 第一部分，也就是外环对应的边
                bool xx1 = true, xx2 = true; //xx1表示  外环 线方向 ,true为顺时针

                int fromOrTo = 1;
                IGeometryCollection pGeoCol = fillGeo as IGeometryCollection;
                IGeometry outGeo = pGeoCol.get_Geometry(0);
                int lineIdx = 0;
                for (int idx = 0; idx < pLineList.Count; idx++)
                {
                    LineObject aLineObj = pLineList[idx];
                    IPolyline aLine = aLineObj.Shape;
                    if (LineInPart(aLine, outGeo as IPointCollection, ref xx1))
                    {
                        //找到这条边，并且返回 线顺序
                        lineIdx = idx;
                        break;
                    }
                }


                LineObject firstLineObj = pLineList[lineIdx];//第一条边   
                rList.Add(firstLineObj.GID);
                IPoint curPt = (firstLineObj.Shape).ToPoint;  //终止点

                IPointCollection ptCols = firstLineObj.Shape as IPointCollection;
                IPoint prePt = ptCols.get_Point(ptCols.PointCount - 2);// 倒数第二个点

                pLineList.RemoveAt(lineIdx);

                while (pLineList.Count > 0)
                {
                    
                    //LineObject NextLine = getNextLineByMinAngle(pLineList, prePt, curPt, ref fromOrTo);
                    LineObject NextLine = getNextLine(pLineList, prePt, curPt, ref fromOrTo, outGeo);

                    if (NextLine != null)
                    {
                        if (fromOrTo == 1)
                        {
                            //如果下一条边与这条变方向相同
                            rList.Add(NextLine.GID);
                            curPt = (NextLine.Shape as IPolyline).ToPoint;
                            IPointCollection nextLinePts = NextLine.Shape as IPointCollection;
                            prePt = nextLinePts.get_Point(nextLinePts.PointCount - 2);

                            pLineList.Remove(NextLine);

                        }
                        else if (fromOrTo == -1)
                        {
                            //如果方向不相同
                            rList.Add(-NextLine.GID);
                            curPt = (NextLine.Shape as IPolyline).FromPoint;
                            IPointCollection nextLinePts = NextLine.Shape as IPointCollection;
                            prePt = nextLinePts.get_Point(1);// 第二个点

                            pLineList.Remove(NextLine);
                        }
                    }
                    else
                    {
                        //找不到了，跳出，
                        break;
                    }
                   
                }
                                

                for (int kk = 1; kk < pGeoCol.GeometryCount; kk++)
                {
                    if (pLineList.Count == 0) return rList;
                    IGeometry innerGeo = pGeoCol.get_Geometry(kk);
                    ////判断这个与外环是否有交点
                    //IGeometry outPolygon = GeometryHelper.Ring2Polygon(outGeo as IRing);
                    //IGeometry innerPolygon = GeometryHelper.Ring2Polygon(innerGeo as IRing);
                    //ITopologicalOperator pTop = outPolygon as ITopologicalOperator;

                    //if (pTop.Intersect(innerPolygon, esriGeometryDimension.esriGeometry0Dimension).IsEmpty != true)
                    //{
                    //    break;
                    //}

                    rList.Add(0); //添加一个环标识

                    ////后面各个环
                    lineIdx = 0;
                    for (int idx = 0; idx < pLineList.Count; idx++)
                    {

                        LineObject aLineObj = pLineList[idx];
                        IPolyline aLine = aLineObj.Shape;

                        
                        //先找第一条边  所在索引
                        if (LineInPart(aLine, innerGeo as IPointCollection, ref xx2))
                        {
                            //xx2 表示与内环方向 ，如果true，则内环是逆时针
                            lineIdx = idx;
                            break;

                        }
                    }
                    int prefix = 1;
                    firstLineObj = pLineList[lineIdx];//第一条边
                    //if (firstLineObj.GID == 33000336)
                    //{
                    //}

                    if (xx1 == xx2)
                    {
                        //外环顺时针，内环逆时针 ，正确
                        prefix = 1;
                    }
                    else
                    {
                        //如果内环第一条边与外环第一条边 方向相同。
                        prefix = -1;
                    }

                    //输出第一个边的时候，保证序号与外环是相反的
                    rList.Add(firstLineObj.GID * prefix);
                    ptCols = firstLineObj.Shape as IPointCollection;                   

                    if (xx1==xx2)
                    {
                        //如果内环是逆时针 外环顺时针，
                        curPt = (firstLineObj.Shape).ToPoint;  //终止点
                        prePt = ptCols.get_Point(ptCols.PointCount - 2);// 倒数第二个点
                    }
                    else
                    {
                        curPt = (firstLineObj.Shape).FromPoint;  //终止点
                        prePt = ptCols.get_Point(1);// 

                    }                                     
                    pLineList.RemoveAt(lineIdx);

                    while (pLineList.Count > 0)
                    {
                        
                       // LineObject NextLine = getNextLineByMinAngle(pLineList, prePt, curPt, ref fromOrTo);
                        LineObject NextLine = getNextLine(pLineList, prePt, curPt, ref fromOrTo, innerGeo);
                        if (NextLine == null)
                        {
                            break;
                        }
                        else
                        {
                            if (fromOrTo == 1)
                            {
                                rList.Add(NextLine.GID );  
                                curPt = (NextLine.Shape as IPolyline).ToPoint;
                                IPointCollection nextLinePts = NextLine.Shape as IPointCollection;
                                prePt = nextLinePts.get_Point(nextLinePts.PointCount - 2);

                                pLineList.Remove(NextLine);

                            }
                            else if (fromOrTo == -1)
                            {
                                rList.Add(-NextLine.GID );
                                curPt = (NextLine.Shape as IPolyline).FromPoint;
                                IPointCollection nextLinePts = NextLine.Shape as IPointCollection;
                                prePt = nextLinePts.get_Point(1);// 第二个点

                                pLineList.Remove(NextLine);
                            }
                        }
                        
                       
                    }

                }

            }
            catch (Exception ex)
            {
            }          
            
            return rList;
        }


        

        /// <summary>
        /// 根据属性 查找 面对应的边
        /// </summary>
        /// <param name="lineClass"></param>
        /// <param name="lstart"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public  static List<LineObject> getPolygonJxBySx(IFeatureClass lineClass, long lstart,long fid)
        {
            List<LineObject> lst = new List<LineObject>();
            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "LEFT_FID=" + fid + " or RIGHT_FID=" + fid;
            IFeatureCursor pCursor = lineClass.Search(pQf, true);
            try
            {
                IFeature aLine = null;
                while ((aLine = pCursor.NextFeature()) != null)
                {
                    long lineOid = aLine.OID;
                    lineOid = lstart + lineOid;                    
                    lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQf);
            }
            return lst;
        }

        /// <summary>
        /// 在传入范围内获取 地类图斑对应的关联线层
        /// </summary>
        /// <param name="lineClass"></param>
        /// <param name="extent"></param>
        /// <returns></returns>
        public static Dictionary<long, List<LineObject>> getDltbLineObjs(IFeatureClass lineClass,long lstart, IGeometry extent)
        {
            Dictionary<long, List<LineObject>> dicDltbLines = new Dictionary<long, List<LineObject>>();
            IFeature aLine = null;
            IFeatureCursor pCursor = null;
            if (extent == null)
            {
                pCursor = lineClass.Search(null, false);
            }
            else
            {
                ISpatialFilter pSF = new SpatialFilter();
                pSF.Geometry = extent;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pCursor = lineClass.Search(pSF as IQueryFilter, false);
            }

            
            try
            {
                while ((aLine = pCursor.NextFeature()) != null)
                {
                    long lineOid = aLine.OID;
                    lineOid = lstart + lineOid;  //线的序号 用对应关联图层起始 数字 +线自身ID，
                    
                    //两侧面id
                    long tbfid = long.Parse(FeatureHelper.GetFeatureStringValue(aLine, "LEFT_FID"));                 
                    if (tbfid > -1)
                    {
                        tbfid += lstart * 10;  //百万级*10 就是千万级了，面的数量应该达不到几百万，所以面的id 与线的id不会重复，但是根据这个换算关系，可以直接得到线
                        if (dicDltbLines.ContainsKey(tbfid))
                        {
                            List<LineObject> lst = dicDltbLines[tbfid];
                            lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
                            dicDltbLines[tbfid] = lst;
                        }
                        else
                        {
                            List<LineObject> lst = new List<LineObject>();
                            lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
                            dicDltbLines.Add(tbfid, lst);
                        }
                    }
                  
                    long tbfid2 = long.Parse(FeatureHelper.GetFeatureStringValue(aLine, "RIGHT_FID"));
                    if (tbfid2 > -1)
                    {
                        tbfid2 += lstart * 10;


                        if (dicDltbLines.ContainsKey(tbfid2))
                        {
                            List<LineObject> lst = dicDltbLines[tbfid2];
                            lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
                            dicDltbLines[tbfid2] = lst;
                        }
                        else
                        {
                            List<LineObject> lst = new List<LineObject>();
                            lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
                            dicDltbLines.Add(tbfid2, lst);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return dicDltbLines;
        }


        ///// <summary>
        ///// 在传入范围内获取 地类图斑对应的关联线层
        ///// </summary>
        ///// <param name="lineClass"></param>
        ///// <param name="extent"></param>
        ///// <returns></returns>
        //public static Dictionary<long, List<LineObject>> getDltbLineObjs(IFeatureClass lineClass,  IGeometry extent)
        //{
        //    Dictionary<long, List<LineObject>> dicDltbLines = new Dictionary<long, List<LineObject>>();
        //    ISpatialFilter pSF = new SpatialFilter();
        //    pSF.Geometry = extent;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //    IFeature aLine = null;
        //    IFeatureCursor pCursor = lineClass.Search(pSF as IQueryFilter, false);
        //    try
        //    {
        //        while ((aLine = pCursor.NextFeature()) != null)
        //        {
        //            long lineOid = aLine.OID;

        //            if (aLine.OID == 0)
        //            {
        //                lineOid = -99999999;

        //            }

        //            long tbfid = long.Parse(FeatureHelper.GetFeatureStringValue(aLine, "LEFT_FID"));
                   

        //            long tbfid2 = long.Parse(FeatureHelper.GetFeatureStringValue(aLine, "RIGHT_FID"));
                    
        //            if (dicDltbLines.ContainsKey(tbfid))
        //            {
        //                List<LineObject> lst = dicDltbLines[tbfid];
        //                lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
        //                dicDltbLines[tbfid] = lst;
        //            }
        //            else
        //            {
        //                List<LineObject> lst = new List<LineObject>();
        //                lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
        //                dicDltbLines.Add(tbfid, lst);
        //            }
        //            if (dicDltbLines.ContainsKey(tbfid2))
        //            {
        //                List<LineObject> lst = dicDltbLines[tbfid2];
        //                lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
        //                dicDltbLines[tbfid2] = lst;
        //            }
        //            else
        //            {
        //                List<LineObject> lst = new List<LineObject>();
        //                lst.Add(new LineObject(lineOid, aLine.Shape as IPolyline));
        //                dicDltbLines.Add(tbfid2, lst);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //    }
        //    return dicDltbLines;
        //}



    }
}
