using System;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.IO;

namespace RCIS.GISCommon
{
    public class SpatialRefHelper
    {



        /// <summary>  
        /// 获取空投影  
        /// </summary>  
        /// <returns></returns>  
        public static ISpatialReference CreateUnKnownSpatialReference()
        {
            ISpatialReference pSpatialReference = new UnknownCoordinateSystemClass();
            pSpatialReference.SetDomain(0, 99999999, 0, 99999999);//设置空间范围  
            return pSpatialReference;
        }  


        /// <summary>
        /// 获得投影信息
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static string FormatSR(ISpatialReference sr)
        {
            #region 显示SR
            if (sr != null)
            {
                StringBuilder srBuilder = new StringBuilder();
                if (sr is UnknownCoordinateSystemClass)
                {
                    srBuilder.Append("UnknownCoordinateSystem");
                }
                else if (sr is IProjectedCoordinateSystem)
                {
                    #region 格式化
                    IProjectedCoordinateSystem prjSR = sr as IProjectedCoordinateSystem;
                    srBuilder.Append("投影坐标系:\n");
                    srBuilder.Append("  Name:").Append(prjSR.Name).Append("\n");
                    srBuilder.Append("  Alias:").Append(prjSR.Alias).Append("\n");
                    srBuilder.Append("  Abbreviation:").Append(prjSR.Abbreviation).Append("\n");
                    srBuilder.Append("  Remarks:").Append(prjSR.Remarks).Append("\n");
                    srBuilder.Append("投影:").Append(prjSR.Projection.Name).Append("\n");
                    srBuilder.Append("投影参数:\n");
                    srBuilder.Append("   False_Easting:").Append(prjSR.FalseEasting).Append("\n");
                    srBuilder.Append("   False_Northing:").Append(prjSR.FalseNorthing).Append("\n");
                    srBuilder.Append("   Central_Meridian:").Append(prjSR.get_CentralMeridian(true)).Append("\n");
                    srBuilder.Append("   Scale_Factor:").Append(prjSR.ScaleFactor).Append("\n");
                    srBuilder.Append("   Latitude_Of_Origin:0\n");
                    srBuilder.Append("Linear Unit:").Append(prjSR.CoordinateUnit.Name).Append("(").Append(prjSR.CoordinateUnit.MetersPerUnit).Append(")\n");
                    srBuilder.Append("Geographic Coordinate System:\n");
                    IGeographicCoordinateSystem gcs = prjSR.GeographicCoordinateSystem;
                    srBuilder.Append("  Name:").Append(gcs.Name).Append("\n");
                    srBuilder.Append("  Alias:").Append(gcs.Alias).Append("\n");
                    srBuilder.Append("  Abbreviation:").Append(gcs.Abbreviation).Append("\n");
                    srBuilder.Append("  Remarks:").Append(gcs.Remarks).Append("\n");
                    srBuilder.Append("  Angular Unit:").Append(gcs.CoordinateUnit.Name).Append("(").Append(gcs.CoordinateUnit.RadiansPerUnit).Append(")\n");
                    srBuilder.Append("  Prime Meridian:").Append(gcs.PrimeMeridian.Name).Append("(").Append(gcs.PrimeMeridian.Longitude).Append(")\n");

                    srBuilder.Append("  Datum:").Append(gcs.Datum.Name).Append("\n");
                    srBuilder.Append("    Spheroid:").Append(gcs.Datum.Spheroid.Name).Append("\n");
                    srBuilder.Append("      Semimajor Axis:").Append(gcs.Datum.Spheroid.SemiMajorAxis).Append("\n");
                    srBuilder.Append("      Semiminor Axis:").Append(gcs.Datum.Spheroid.SemiMinorAxis).Append("\n");
                    srBuilder.Append("      Inverse Flattening:").Append((1 / gcs.Datum.Spheroid.Flattening)).Append("\n");
                    srBuilder.Append("X/Y Domain:\n");
                    try
                    {
                        double minX = 0, minY = 0, maxX = 0, maxY = 0, xyScale = 0;
                        sr.GetDomain(out minX, out maxX, out minY, out maxY);
                        sr.GetFalseOriginAndUnits(out minX, out minY, out xyScale);
                        srBuilder.Append(" Min X:").Append(minX).Append("\n");
                        srBuilder.Append(" Min Y:").Append(minY).Append("\n");
                        srBuilder.Append(" Max X:").Append(maxX).Append("\n");
                        srBuilder.Append(" Max Y:").Append(maxY).Append("\n");
                        srBuilder.Append(" XYScale:").Append(xyScale).Append("\n");
                        srBuilder.Append("\n");
                    }
                    catch (Exception ex)
                    {
                    }
                    srBuilder.Append("Z Domain:\n");
                    try
                    {
                        double minZ, maxZ, zScale = 0;
                        sr.GetZDomain(out minZ, out maxZ);
                        sr.GetZFalseOriginAndUnits(out minZ, out zScale);
                        srBuilder.Append("  Min Z:").Append(minZ).Append("\n");
                        srBuilder.Append("  Max Z:").Append(maxZ).Append("\n");
                        srBuilder.Append("  ZScale:").Append(zScale).Append("\n");
                        srBuilder.Append("\n");
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        srBuilder.Append("M Domain:\n");
                        double minM, maxM, mScale = 0;
                        sr.GetMDomain(out minM, out maxM);
                        sr.GetMFalseOriginAndUnits(out minM, out mScale);
                        srBuilder.Append("  Min M:").Append(minM).Append("\n");
                        srBuilder.Append("  Max M:").Append(maxM).Append("\n");
                        srBuilder.Append("  MScale:").Append(mScale).Append("\n");
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                else if (sr is IGeographicCoordinateSystem)
                {
                    #region 格式化
                    srBuilder.Append("Geographic Coordinate System:\n");
                    IGeographicCoordinateSystem gcs = sr as IGeographicCoordinateSystem;
                    srBuilder.Append("  Name:").Append(gcs.Name).Append("\n");
                    srBuilder.Append("  Alias:").Append(gcs.Alias).Append("\n");
                    srBuilder.Append("  Abbreviation:").Append(gcs.Abbreviation).Append("\n");
                    srBuilder.Append("  Remarks:").Append(gcs.Remarks).Append("\n");
                    srBuilder.Append("  Angular Unit:").Append(gcs.CoordinateUnit.Name).Append("(").Append(gcs.CoordinateUnit.RadiansPerUnit).Append(")\n");
                    srBuilder.Append("  Prime Meridian:").Append(gcs.PrimeMeridian.Name).Append("(").Append(gcs.PrimeMeridian.Longitude).Append(")\n");

                    srBuilder.Append("  Datum:").Append(gcs.Datum.Name).Append("\n");
                    srBuilder.Append("    Spheroid:").Append(gcs.Datum.Spheroid.Name).Append("\n");
                    srBuilder.Append("      Semimajor Axis:").Append(gcs.Datum.Spheroid.SemiMajorAxis).Append("\n");
                    srBuilder.Append("      Semiminor Axis:").Append(gcs.Datum.Spheroid.SemiMinorAxis).Append("\n");
                    srBuilder.Append("      Inverse Flattening:").Append((1 / gcs.Datum.Spheroid.Flattening)).Append("\n");
                    #endregion
                }
                String srStr = srBuilder.ToString();
                return srStr;
            }
            else
            {
                return "";
            }
            #endregion
        }


        public static string GetSrPrjPath(string sDir, string subStr)
        {
            if ((sDir == "") || (!Directory.Exists(sDir)))
            {
                return "";
            }
            string[] sFiles = Directory.GetFiles(sDir);
            foreach (string filename in sFiles)
            {
                if (filename.Contains(subStr + ".prj"))
                {
                    return filename;
                }
            }
            return "";
        }

        /// <summary>
        /// 从文件构建空间参考
        /// </summary>
        /// <param name="highPrecision"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ISpatialReference ConstructCoordinateSystem(bool highPrecision, string path)
        {
            ISpatialReferenceFactory3 spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference spatialReference = spatialReferenceFactory.CreateESRISpatialReferenceFromPRJFile(path) as ISpatialReference3;

            IControlPrecision2 controlPrecision = spatialReference as IControlPrecision2;

            //Determines whether you are constructing a high or low.
            controlPrecision.IsHighPrecision = highPrecision;
            ISpatialReferenceResolution spatialReferenceResolution = spatialReference as ISpatialReferenceResolution;

            //These three methods are the keys, construct horizon, then set the default x,y resolution and tolerance.
            spatialReferenceResolution.ConstructFromHorizon();

            //Set the default x,y resolution value.
            spatialReferenceResolution.SetDefaultXYResolution();

            //Set the default x,y tolerance value.
            ISpatialReferenceTolerance spatialReferenceTolerance = spatialReference as ISpatialReferenceTolerance;
            spatialReferenceTolerance.SetDefaultXYTolerance();
            return spatialReference;

            //double xMin;
            //double xMax;
            //double yMin;
            //double yMax;
            //spatialReference.GetDomain(out xMin, out xMax, out yMin, out yMax);

            //System.Windows.Forms.MessageBox.Show("Domain : " + xMin + ", " + xMax + ", " + yMin + ", " + yMax);


        }



    }
}
