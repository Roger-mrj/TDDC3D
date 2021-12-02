using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace RCIS.DataInterface.VCT3In
{
    public class VCT3InAPOINT2Shp
    {
        private string SgSeparator = ",";

        Hashtable ghasTableStructure = null;
        string aTabName = "";
       
        string shpfile = "";
        public VCT3InAPOINT2Shp(string _tabName, Hashtable _ht, string _shpile)
        {
            this.aTabName = _tabName;
            this.ghasTableStructure = _ht;
            this.shpfile = _shpile;
            
        }


        public VCT3InAPOINT2Shp()
        {

        }

        /// <summary>
        /// 导入注记
        /// </summary>
        public void InAnnotationClass()
        {
            string sTmpFilePath = System.Windows.Forms.Application.StartupPath + "\\VCTProcess";

            string sDataFile =sTmpFilePath+" \\ANNOTATION.DATA";
            if (!System.IO.File.Exists(sDataFile))
            {
                return;
            }
            #region 准备工作
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
            string strVectorFile = sTmpFilePath+"\\ZJ\\ZJ.SHP";
            OSGeo.OGR.Ogr.RegisterAll();
            string strDriverName = "ESRI Shapefile";
            int count = OSGeo.OGR.Ogr.GetDriverCount();
            OSGeo.OGR.Driver oDriver = OSGeo.OGR.Ogr.GetDriverByName(strDriverName);
            if (oDriver == null)
            {
                return;
            }
            OSGeo.OGR.DataSource ds = OSGeo.OGR.Ogr.Open(strVectorFile, 1);
            if (ds == null)
            {
                return;
            }
            int iLayerCount = ds.GetLayerCount();

            // 获取第一个图层  
            OSGeo.OGR.Layer oLayer = ds.GetLayerByIndex(0);
            if (oLayer == null)
            {
                return;
            }
            #endregion 
            OSGeo.OGR.FeatureDefn oDefn = oLayer.GetLayerDefn();
            OSGeo.OGR.Feature oFeature = null;
            StreamReader dataReader = new StreamReader(sDataFile, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                while (dataReader.EndOfStream == false)
                {
                   
                    string sDataLine = dataReader.ReadLine().Trim().ToUpper();
                    string[] strs = sDataLine.Split(',');
                    string bsm = strs[0];
                    string ysdm = strs[1];
                    string zjnr = strs[2];
                    double x = 0;
                    double y = 0;
                    double jd = 0;
                    double.TryParse(strs[3], out x);
                    double.TryParse(strs[4], out y);
                    double.TryParse(strs[5], out jd);
                    oFeature = new OSGeo.OGR.Feature(oDefn);
                    OSGeo.OGR.Geometry ptGeo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPoint);
                    ptGeo.AddPoint(x,y, 0);
                    oFeature.SetGeometry(ptGeo);
                    int iPos = oDefn.GetFieldIndex("BSM");
                    oFeature.SetField(iPos, bsm);
                    iPos = oDefn.GetFieldIndex("YSDM");
                    oFeature.SetField(iPos, ysdm);
                    iPos = oDefn.GetFieldIndex("ZJNR");
                    oFeature.SetField(iPos, zjnr);
                    iPos = oDefn.GetFieldIndex("ZJFX");
                    oFeature.SetField(iPos, jd);
                    oLayer.CreateFeature(oFeature);

                    if (oFeature != null) oFeature.Dispose();

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dataReader.Close();
                if (oLayer != null) oLayer.Dispose();
                if (ds != null) ds.Dispose();
            }
        }

        public void InAPointClass()
        {
            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;

            TableStructBeginEnd3 curItem = (TableStructBeginEnd3)ghasTableStructure[aTabName];

            string aPtDataFile = sTmpFilePath + "\\POINT_" + aTabName.ToUpper() + ".DATA";
            string aPtAttFile = sTmpFilePath + "\\POINT_" + aTabName.ToUpper() + ".ATTR";
            if (!System.IO.File.Exists(aPtAttFile) || !System.IO.File.Exists(aPtDataFile))
            {
                //找不到该文件。掠过
            
                return;
            }
            
            #region 准备工作
            //OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
            string strVectorFile = this.shpfile;
            OSGeo.OGR.Ogr.RegisterAll();
            string strDriverName = "ESRI Shapefile";
            int count = OSGeo.OGR.Ogr.GetDriverCount();
            OSGeo.OGR.Driver oDriver = OSGeo.OGR.Ogr.GetDriverByName(strDriverName);
            if (oDriver == null)
            {
                return;
            }
            OSGeo.OGR.DataSource ds = OSGeo.OGR.Ogr.Open(strVectorFile, 1);
            if (ds == null)
            {
                return;
            }
            int iLayerCount = ds.GetLayerCount();

            // 获取第一个图层  
            OSGeo.OGR.Layer oLayer = ds.GetLayerByIndex(0);
            if (oLayer == null)
            {
                return;
            }
            OSGeo.OGR.FeatureDefn oDefn = oLayer.GetLayerDefn();
            OSGeo.OGR.Feature oFeature = null;
            //字段类型和 索引
            int[] nFldIndx = new int[oDefn.GetFieldCount()];
            int[] nFldType = new int[oDefn.GetFieldCount()];
            int nAttrFldGs = curItem.aZDMCs.Count;
            try
            {
                for (int i = 0; i < nAttrFldGs; i++)
                {
                    string sName = (string)curItem.aZDMCs[i];
                    int iPos = oDefn.GetFieldIndex(sName);
                    nFldIndx[i] = iPos;
                    string sType = (string)curItem.aZDLXs[i];
                    if (sType.Equals("INT"))
                        nFldType[i] = 1;
                    else if (sType.Equals("DOUBLE"))
                        nFldType[i] = 2;
                    else if (sType.Equals("CHAR"))
                        nFldType[i] = 3;
                    else
                        nFldType[i] = 4;

                }
            }
            catch (Exception ex)
            {
            }

            #endregion 

            long ngMBBSM = 0;  //BSM
            StreamReader dataReader = new StreamReader(aPtDataFile, System.Text.Encoding.GetEncoding("GB2312"));
            StreamReader attrReader = new StreamReader(aPtAttFile, System.Text.Encoding.GetEncoding("GB2312"), true);
            try
            {

                while (dataReader.EndOfStream == false)
                {
                    #region 写入一个数据
                    string sDataLine = dataReader.ReadLine().Trim().ToUpper();
                    long nMBBSM = Convert.ToInt64(sDataLine);
                    ngMBBSM = nMBBSM;
                    //下一行 坐标
                    sDataLine = dataReader.ReadLine().Trim().ToUpper();
                    string[] sZJP = sDataLine.Split(this.SgSeparator.ToCharArray());

                    oFeature = new OSGeo.OGR.Feature(oDefn);
                    OSGeo.OGR.Geometry ptGeo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPoint);
                    ptGeo.AddPoint(Convert.ToDouble(sZJP[0]), Convert.ToDouble(sZJP[1]), 0);


                    oFeature.SetGeometry(ptGeo);
                    //写属性
                    string sAttLine = attrReader.ReadLine();
                    if (sAttLine != null)
                    {
                        string[] aAttrArray = sAttLine.Split(SgSeparator.ToCharArray());

                        //从第二个 开始导入，第一个是唯一数字标识
                        for (int k = 1; k < nAttrFldGs + 1; k++)
                        {
                            if (aAttrArray.Length == k) break;
                            int nPos = nFldIndx[k - 1];
                            int nType = nFldType[k - 1];

                            if (nPos != -1)
                            {
                                object oo = aAttrArray[k];
                                string ss = oo.ToString().Trim();
                                int nValue = 0;
                                double dValue = 0;
                                string sValue = "";
                                if (nType == 1)
                                {
                                    int.TryParse(ss, out nValue);
                                    oFeature.SetField(nPos, nValue);
                                }
                                else if (nType == 2)
                                {

                                    double.TryParse(ss, out dValue);
                                    oFeature.SetField(nPos, dValue);
                                }
                                else
                                {
                                    sValue = ss;
                                    oFeature.SetField(nPos, sValue);
                                }
                            }
                        }

                    }
                    oLayer.CreateFeature(oFeature);

                    if (oFeature != null) oFeature.Dispose();

                    #endregion

                } //while

                


            }
            catch (Exception E)
            {


            }
            finally
            {
                dataReader.Close();
                attrReader.Close();
                if (oLayer != null) oLayer.Dispose();
                if (ds != null) ds.Dispose();
            }

        }
    }
}
