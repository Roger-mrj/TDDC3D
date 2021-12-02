using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Collections;

namespace SphereArea
{
    public class SphereAreaClass
    {
        double PI = 3.14159265358979;
        //'中央经线
        double CenterL = 0;

        double RHO = 206264.8062471;

        private double ParamA;
        private double ParamB;
        private double ParamC;
        private double ParamD;
        private double ParamE;
        private const double ZERO = 0.000000000001;


        double aRadius = 6378137;
        //'椭球短半轴：
        double bRadius = 6356752.31414036;
        //'椭球扁率：
        double ParaAF = 1 / 298.257222101;

        /// <summary>
        /// //'椭球第一偏心率：
        /// </summary>
        double ParaE1 = 0.0066943800229;
        /// <summary>
        /// '椭球第二偏心率：
        /// </summary>
        double ParaE2 = 0.00673949677548;
        //'极点子午圈曲率半径：
        double ParaC = 6399593.62586;

        //'k0:
        double Parak0 = 1.57048761144159E-07;// '1.57048687472752E-07
        //'k1:
        double Parak1 = 5.05250178820567E-03;// '5.05250559291393E-03
        //'k2:
        double Parak2 = 2.98472900956587E-05; //'2.98473350966158E-05
        //'k3:
        double Parak3 = 2.41626669230084E-07;//' 2.41627215981336E-07
        //'k4:
        double Parak4 = 2.22241238938534E-09; //'2.22241909461273E-09


        private double StandardLat=0; // '选定本初子午线为参考经线


        private double DDH; //度带号
        private double CenterX = 0; //'从坐标系统里获取的中央经线

        
        /// <summary>
        /// //"高斯坐标反解算法"
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="center"></param>
        public void ComputeXYGeo(double x, double y, ref double B, ref double L, double center)
        {          

            double yl, bf;
            string str = ((int)y).ToString().Trim();
            if (str.Length == 8)
                yl = y - 500000 - DDH * 1000000;
            else
                yl = y - 500000;

            double E = Parak0 * x;                     
            double Se = Math.Sin(E);
            bf = E + Math.Cos(E) * (Parak1 * Se - Parak2 * Math.Pow(Se, 3) + Parak3 * Math.Pow(Se, 5) - Parak4 * Math.Pow(Se, 7));
            
            double V, t, N, n2, v2t, yn, t2, g;
            g = 1;
            t = Math.Tan(bf);
            n2 = ParaE2 * Math.Pow(Math.Cos(bf), 2);

            V = Math.Sqrt(1 + n2);
            N = ParaC / V;
            yn = yl / N;
            v2t = Math.Pow(V, 2) * t;
            t2 = Math.Pow(t, 2);

            B = bf - v2t * Math.Pow(yn, 2) / 2.0 + (5.0 + 3.0 * t2 + n2 - 9.0 * n2 * t2) * v2t * Math.Pow(yn, 4) / 24.0 
                - (61.0 + 90.0 * t2 + 45.0 * Math.Pow(t2, 2)) * v2t * Math.Pow(yn, 6) / 720.0;


         

            B = TransArcToDegree(B);

            double cbf= 1.0 / Math.Cos(bf);
            L = cbf * yn - (1.0 + 2.0 * t2 + n2) * cbf * Math.Pow(yn, 3) / 6.0 + (5.0 + 28.0 * t2 + 24.0 * Math.Pow(t2, 2) 
                + 6.0 * n2 + 8.0 * n2 * t2) * cbf * Math.Pow(yn, 5) / 120.0 + center;

            //保留8 位有效数字
           // L = this.validDecimal(L, 8);
         
            L = TransArcToDegree(L);

        }

        /// <summary>
        /// 直接计算面积，自动获取带号
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <returns></returns>
        public double SphereArea(IGeometry pGeometry)
        {
            double pArea = 0;
            IPolygon4 pP4;
            IGeometryCollection pGC;
            IGeometryCollection pGC1;

            IProjectedCoordinateSystem pPC;



            if (pGeometry.SpatialReference != null)
            {
                if (pGeometry.SpatialReference is IProjectedCoordinateSystem)
                {
                    pPC = pGeometry.SpatialReference as IProjectedCoordinateSystem;
                    this.CenterX = pPC.get_CentralMeridian(true);
                }
            }
            IPoint selectPoint = (pGeometry as IArea).Centroid;
            double X = selectPoint.X;
            int _DDH = (int)(X / 1000000);////WK---带号

            if (CenterX == 0) CenterX = _DDH * 3;
            pP4 = pGeometry as IPolygon4;
            pP4.SimplifyPreserveFromTo();
            pGC = pP4.ExteriorRingBag as IGeometryCollection;

            for (int j = 0; j < pGC.GeometryCount; j++)
            {
                pArea -= GetSphereArea_1(pGC.get_Geometry(j), _DDH);

                pGC1 = pP4.get_InteriorRingBag(pGC.get_Geometry(j) as IRing) as IGeometryCollection;
                for (int k = 0; k < pGC1.GeometryCount; k++)
                {
                    pArea -= GetSphereArea_1(pGC1.get_Geometry(k), _DDH);
                }
            }
            pArea = Round(pArea, 2);
            return pArea;
        }


        /// <summary>
        /// 计算图形椭球面积
        /// </summary>
        /// <param name="pGeometry">图形</param>
        /// <param name="_DDH">带号</param>
        /// <returns></returns>
        public double SphereArea(IGeometry pGeometry, double _DDH)
        {
            double pArea = 0;
            IPolygon4 pP4;
            IGeometryCollection pGC;
            IGeometryCollection pGC1;

            IProjectedCoordinateSystem pPC;

            if (pGeometry.SpatialReference != null)
            {
                if (pGeometry.SpatialReference is IProjectedCoordinateSystem)
                {
                    pPC = pGeometry.SpatialReference as IProjectedCoordinateSystem;
                    this.CenterX = pPC.get_CentralMeridian(true);
                }
            }
            if (CenterX == 0) CenterX = _DDH * 3;
            pP4 = pGeometry as IPolygon4;
            pP4.SimplifyPreserveFromTo();
            pGC = pP4.ExteriorRingBag as IGeometryCollection;

            for (int j = 0; j < pGC.GeometryCount; j++)
            {
                pArea -= GetSphereArea_1(pGC.get_Geometry(j), _DDH);
                
                pGC1 = pP4.get_InteriorRingBag(pGC.get_Geometry(j) as IRing) as IGeometryCollection;
                for (int k = 0; k < pGC1.GeometryCount; k++)
                {
                    pArea -= GetSphereArea_1(pGC1.get_Geometry(k), _DDH);
                }
            }
            pArea = Round(pArea, 2);           
            return pArea;
        }

        public IGeometry getNewGeo(IGeometry pGeometry)
        {
            IPointCollection PntColl = pGeometry as IPointCollection;
            //记录下这些点

            ArrayList arSrcPts = new ArrayList();
            for (int i = 0; i < PntColl.PointCount; i++)
            {
                arSrcPts.Add(PntColl.get_Point(i));
            }
            //插值之后的点
            ArrayList arTargetPts = new ArrayList();
            for (int i = 0; i < arSrcPts.Count - 1; i++)
            {
                arTargetPts.Add(arSrcPts[i]);
                //中间差值点
                ArrayList interPts = this.getInterPt(arSrcPts[i] as IPoint, arSrcPts[i + 1] as IPoint);
                foreach (IPoint aInterPt in interPts)
                {
                    arTargetPts.Add(aInterPt);
                }

            }
            //插入最后一个点
            arTargetPts.Add(arSrcPts[arSrcPts.Count - 1]);

            IPolygon pol = new PolygonClass();
            IPointCollection newGC = pol as IPointCollection;
            foreach (IPoint ap in arTargetPts)
            {
                newGC.AddPoint(ap);
            }

            return pol;
        }


        //"弧度转换为度分秒"
        public double TransArcToDegree(double arc)
        {
            double degree;
            double min, sec, ret, tmp;
            ret = arc * 180 / PI;
            degree = FormatValue(ret, 100, 100);
            tmp = (ret - degree) * 60;
            min = FormatValue(tmp, 100, 100);
            sec = (tmp - min) * 60;
            // '秒保留到小数点后6位，四舍五入
            //sec = Round(sec, 6);
            
            double result=degree * 3600 + min * 60 + sec;

            return this.Round(result, 6);

        }

        private double FormatValue(double inputVal, long precsion, long scaleNum)
        {
            return ((long)(inputVal * precsion) - ((long)(inputVal * precsion)) % scaleNum) / precsion;

        }

        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        private double Round(double v, int x)
        {
            bool isNegative = false;
            //如果是负数
            if (v < 0)
            {
                isNegative = true;
                v = -v;
            }

            int IValue = 1;
            for (int i = 1; i <= x; i++)
            {
                IValue = IValue * 10;
            }
            double big = v * IValue;
            long lbig = (long)(v * IValue);

            double ws = big - lbig;
            double Int = 0;
            if (ws < 0.5)
                Int = Math.Round(v * IValue, 0);
            else
                Int = Math.Round(v * IValue + 0.5, 0);


            v = Int / IValue;

            if (isNegative)
            {
                v = -v;
            }

            return v;
        }

        private ArrayList getInterPt(IPoint startPt, IPoint endPt)
        {
            ArrayList ar = new ArrayList();
            ILine aLine = new LineClass();
            aLine.FromPoint = startPt;
            aLine.ToPoint = endPt;

            double allLen = aLine.Length;

            if (allLen <= 70) return ar;
            //插值点个数
            int iCount = (int)(allLen / 70);
            //由个数计算每段长度
            double interLineLen = allLen / (iCount + 1);
            double deltLen = 0;
            for (int i = 0; i < iCount; i++)
            {
                deltLen = interLineLen * (i + 1);
                if (deltLen >= allLen) break;
                //查询该点
                IPoint outPt = new PointClass();
                aLine.QueryPoint(esriSegmentExtension.esriExtendEmbeddedAtTo, deltLen, false, outPt);
                //outPt.X = Round(outPt.X, 6);               
               // outPt.Y = Round(outPt.Y, 6);
                ar.Add(outPt);

            }
            return ar;

        }


        /// <summary>
        /// 计算椭球面积
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="_DDH"></param>
        /// <returns></returns>
        public double GetSphereArea_1(IGeometry pGeometry, double _DDH)
        {
            //'初始化参数
            double e2 = 0;
            double B1 = 0;
            double L1 = 0;
            double B2 = 0;
            double L2 = 0;
            double areaSum = 0;
           
            DDH = _DDH;
            //'中央经线
            CenterL = CenterX;
            
            e2 = ParaE1;
            ParamA = 1.0 + (3.0 / 6.0) * e2 + (30.0 / 80.0) * Math.Pow(e2, 2) + (35.0 / 112.0) * Math.Pow(e2, 3) + (630.0 / 2304.0) * Math.Pow(e2, 4);
            ParamB = (1.0 / 6.0) * e2 + (15.0 / 80.0) * Math.Pow(e2, 2) + (21.0 / 112.0) * Math.Pow(e2, 3) + (420.0 / 2304.0) * Math.Pow(e2, 4);
            ParamC = (3.0 / 80.0) * Math.Pow(e2, 2) + (7.0 / 112.0) * Math.Pow(e2, 3) + (180.0 / 2304.0) * Math.Pow(e2, 4);
            ParamD = (1.0 / 112.0) * Math.Pow(e2, 3) + (45.0 / 2304.0) * Math.Pow(e2, 4);
            ParamE = (5.0 / 2304.0) * Math.Pow(e2, 4);

            //'中央经线转换为弧度
            CenterL = CenterL * this.PI / 180;
            //'选定本初子午线为参考经线
            StandardLat = 0;

            IPointCollection PntColl = pGeometry as IPointCollection;
            //记录下这些点

            ArrayList arSrcPts = new ArrayList();
            for (int i = 0; i < PntColl.PointCount; i++)
            {                
                arSrcPts.Add(PntColl.get_Point(i));
            }
            //插值之后的点，最后一个点 和起始点相等

            ArrayList arTargetPts = new ArrayList();
            for (int i = 0; i < arSrcPts.Count - 1; i++)
            {
                if (i == 866)
                {
                }
                IPoint currPt = arSrcPts[i] as IPoint ;
                arTargetPts.Add(currPt);
                //中间差值点
                IPoint nextPt=arSrcPts[i + 1] as IPoint;
                ArrayList interPts = this.getInterPt(currPt,nextPt );
                foreach (IPoint aInterPt in interPts)
                {
                    arTargetPts.Add(aInterPt);
                }
            }
            //插入最后一个点
            arTargetPts.Add(arSrcPts[arSrcPts.Count - 1]);
            //得到 所有平面坐标点
                      
            //逐个计算
            for (int i = 0; i < arTargetPts.Count - 1; i++)
            {
               // Console.WriteLine("第" + i + "个梯形:");

                double y1 = (arTargetPts[i] as IPoint).Y;
                double x1 = (arTargetPts[i] as IPoint).X;
                double y2 = (arTargetPts[i + 1] as IPoint).Y;
                double x2 = (arTargetPts[i + 1] as IPoint).X;

              //  Console.WriteLine("p1:" + (arTargetPts[i] as IPoint).X + "," + (arTargetPts[i] as IPoint).Y);
             //   Console.WriteLine("p2:" + (arTargetPts[i + 1] as IPoint).X + "," + (arTargetPts[i + 1] as IPoint).Y);
                //高斯平面坐标保留4位小数，
                x1 = this.Round(x1, 4);
                y1 = this.Round(y1, 4);
                x2 = this.Round(x2, 4);
                y2 = this.Round(y2, 4);

                
                //转换为 大地坐标B，L
                ComputeXYGeo(y1, x1, ref B1, ref L1, CenterL);               
                ComputeXYGeo(y2, x2, ref B2, ref L2, CenterL);

                //Console.WriteLine("B1:" + B1 + ",L1:" + L1);
                //Console.WriteLine("B2:" + B2 + "," + B2);

                //'将经纬度转换为弧度值()
                B1 = B1 / RHO;
                L1 = L1 / RHO;
                B2 = B2 / RHO;
                L2 = L2 / RHO;
                              

                //'计算梯形面积
                double AreaVal = 0;//'/梯形面积值
                
                double deltL = (L2 + L1) / 2 - StandardLat;//';//经差
                double deltB =  B2 - B1; //'; //纬差
                double Bm = (B2 + B1) / 2 ;//';//纬度和
                              

                double[] ItemValue = new double[5];//';//计算变量 
                ItemValue[0] = ParamA * Math.Sin(deltB / 2.0) * Math.Cos(Bm);
                ItemValue[1] = ParamB * Math.Sin(3.0 * deltB / 2.0) * Math.Cos(3 * Bm);
                ItemValue[2] = ParamC * Math.Sin(5.0 * deltB / 2.0) * Math.Cos(5 * Bm);
                ItemValue[3] = ParamD * Math.Sin(7.0 * deltB / 2.0) * Math.Cos(7 * Bm);
                ItemValue[4] = ParamE * Math.Sin(9.0 * deltB / 2.0) * Math.Cos(9 * Bm);

                AreaVal = 2.0 * bRadius * deltL * bRadius * (ItemValue[0] - ItemValue[1] + ItemValue[2] - ItemValue[3] + ItemValue[4]);
                //AreaVal = this.validDecimal(AreaVal, 8);

                //Console.WriteLine("梯形" + i + "   area:" + AreaVal);
               // AreaVal = this.Round(AreaVal, 2);

                areaSum = areaSum + AreaVal;
                
            }
            
            return areaSum;            
        }


        /// <summary>
        /// 保留小数点后有效数字
        /// </summary>
        /// <param name="num"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        private double validDecimal(double num, int precision)
        {
            int iNum = (int)num;
            string s = (num - iNum).ToString("G" + precision);            
            return iNum + double.Parse(s);
            
        }

        public double SphereAreaTF(IGeometry pGeometry, double _DDH)
        {

            double pArea = 0;
            IPolygon4 pP4;
            IGeometryCollection pGC;
            IGeometryCollection pGC1;

            pP4 = pGeometry as IPolygon4;
            pP4.SimplifyPreserveFromTo();

            pGC = pP4.ExteriorRingBag as IGeometryCollection;
            for (int j = 0; j < pGC.GeometryCount; j++)
            {
                pArea -= GetSphereArea_1(pGC.get_Geometry(j), _DDH);
                pGC1 = pP4.get_InteriorRingBag(pGC.get_Geometry(j) as IRing) as IGeometryCollection;
                for (int k = 0; k < pGC1.GeometryCount; k++)
                {
                    pArea -= GetSphereArea_1(pGC1.get_Geometry(k), _DDH);
                }
            }
            return pArea;
        }

        
    }
}
