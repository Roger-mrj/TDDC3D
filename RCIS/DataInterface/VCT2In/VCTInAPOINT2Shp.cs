using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace RCIS.DataInterface.VCT2
{
    public class VCTInAPOINT2Shp
    {
        private string SgSeparator = ",";

        Hashtable ghasTableStructure = null;
        string aTabName = "";
       
        string shpfile = "";
        public VCTInAPOINT2Shp(string _tabName, Hashtable _ht, string _shpile)
        {
            this.aTabName = _tabName;
            this.ghasTableStructure = _ht;
            this.shpfile = _shpile;
            
        }

        public void InAPointClass()
        {
            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;

            TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[aTabName];

            string aPtDataFile = sTmpFilePath + "\\POINT_" + aTabName.ToUpper() + ".DATA";
            string aPtAttFile = sTmpFilePath + "\\POINT_" + aTabName.ToUpper() + ".ATTR";
            if (!System.IO.File.Exists(aPtAttFile) || !System.IO.File.Exists(aPtDataFile))
            {
                //找不到该文件。掠过
                //aFileFinish(this.aTabName + "没有数据！");
                //allFileFinish();
                return;
            }



            #region 准备工作
            //OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            //// 为了使属性表字段支持中文，请添加下面这句
            //OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
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

            int ngMBBSM = 0;  //BSM
            StreamReader dataReader = new StreamReader(aPtDataFile, System.Text.Encoding.GetEncoding("GB2312"));
            StreamReader attrReader = new StreamReader(aPtAttFile, System.Text.Encoding.GetEncoding("GB2312"), true);
            try
            {

                while (dataReader.EndOfStream == false)
                {
                    #region 写入一个数据
                    string sDataLine = dataReader.ReadLine().Trim().ToUpper();
                    int nMBBSM = Convert.ToInt32(sDataLine);
                    ngMBBSM = nMBBSM;
                    //下一行 坐标
                    sDataLine = dataReader.ReadLine().Trim().ToUpper();
                    string[] sZJP = sDataLine.Split(this.SgSeparator.ToCharArray());
                  
                    oFeature=new OSGeo.OGR.Feature(oDefn);
                    OSGeo.OGR.Geometry ptGeo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPoint);
                    ptGeo.AddPoint(Convert.ToDouble(sZJP[0]), Convert.ToDouble(sZJP[1]), 0);


                    oFeature.SetGeometry(ptGeo);
                    //写属性
                    string sAttLine = attrReader.ReadLine();
                    if (sAttLine != null)
                    {
                        string[] aAttrArray = sAttLine.Split(SgSeparator.ToCharArray());

                        for (int k = 0; k < nAttrFldGs; k++)
                        {
                            if (aAttrArray.Length == k) break;
                            int nPos = nFldIndx[k];
                            int nType = nFldType[k];
                            
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

                dataReader.Close();
                attrReader.Close();
               

            }
            catch (Exception E)
            {

                
            }

        }
    }
}
