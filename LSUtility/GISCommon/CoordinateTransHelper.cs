using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System;
namespace RCIS.GISCommon
{
    public class CoordinateTransHelper
    {

        /// <summary>
        /// 经纬度转投影坐标
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pJW"></param>
        /// <returns></returns>
        public static IPoint JWD2XY(IMap pMap, IPoint pJW)
        {
            IPoint aPt = new PointClass();
            try
            {
                aPt = (pJW as IClone).Clone() as IPoint;
                ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
                ISpatialReference earthref = pSRF.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                ISpatialReference prjRef = pMap.SpatialReference;
                aPt.SpatialReference = earthref;
                aPt.Project(prjRef);
            }
            catch (Exception ex)
            {
                return null;
            }
            return aPt;
        }
        /// <summary>
        /// 经纬度转投影坐标
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pJW"></param>
        /// <returns></returns>
        public static IPolygon JWD2XY(IMap pMap, IPolygon pJW)
        {
            IPolygon aPt = new  PolygonClass();
            try
            {
                aPt = (pJW as IClone).Clone() as IPolygon;
                ISpatialReferenceFactory pSRF = new SpatialReferenceEnvironmentClass();
                ISpatialReference earthref = pSRF.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                ISpatialReference prjRef = pMap.SpatialReference;
                aPt.SpatialReference = earthref;
                aPt.Project(prjRef);
            }
            catch (Exception ex)
            {
                return null;
            }
            return aPt;
        }
        /// <summary>
        /// 将XY转化为经纬度.如果返回的IPoint为empty那么说明出错了。
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pXY"></param>
        /// <param name="pError"></param>
        /// <returns></returns>        
        public static IPoint XY2JWD(IMap pMap, IPoint pXY)
        {
            IPoint aPt = new PointClass();
            try
            {
                aPt = (pXY as IClone).Clone() as IPoint;

                ISpatialReference aSR = pMap.SpatialReference;
                if (aSR != null)
                {
                    if (aSR.Name != "Unknown")
                    {
                        aPt.SpatialReference = aSR;
                        aPt.SnapToSpatialReference();
                        if (aSR is IProjectedCoordinateSystem)
                        {
                            IGeographicCoordinateSystem gcs = (aSR as IProjectedCoordinateSystem).GeographicCoordinateSystem;
                            aPt.Project(gcs);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception ex) { }
            return aPt;
        }

        /// <summary>
        /// 经纬度格式化为 度分秒字符串
        /// </summary>
        /// <param name="pJWD"></param>
        /// <returns></returns>
        public static string FormatJWD(double pJWD)
        {
            int aDegre = (int)pJWD;
            double aSmall = (pJWD - aDegre) * 60;
            int minute = (int)aSmall;
            aSmall = (aSmall - minute) * 60;
            int second = (int)aSmall;
            return aDegre + "°" + minute + "'" + second + "\"";

        }
        /// <summary>
        /// 经纬度格式化为度分秒字符串
        /// </summary>
        /// <param name="pJWD"></param>
        /// <returns></returns>
        public static string FormatDFM(double pJWD)
        {
            int aDegre = (int)pJWD;
            double aSmall = (pJWD - aDegre) * 60;
            int minute = (int)aSmall;
            aSmall = (aSmall - minute) * 60;
            int second = (int)aSmall;

            return aDegre + "度" + minute + "分" + second + "秒";
        }

        public static double FormatValue(double inputVal, long precsion, long scaleNum)
        {
            return ((long)(inputVal * precsion) - ((long)(inputVal * precsion)) % scaleNum) / precsion;

        }
        

        

    }
}
