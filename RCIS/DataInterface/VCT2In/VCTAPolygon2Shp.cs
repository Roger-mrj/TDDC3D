using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

using OSGeo.GDAL;
using OSGeo.OGR;

namespace RCIS.DataInterface.VCT2
{
    public class VCTAPolygon2Shp
    {
        string shpfile = "";

        private string SgSeparator = ",";
        private Hashtable NoOnlyPolygon = new Hashtable();
        string aTabName = "";

        Hashtable ghasTableStructure = null;

        public long bufferSize = 2048;
        public double geoOpTick, fileTick, writeatttick;

        private class APointXY
        {
            public int flag = 0; // 0 结束，1 正向，-1 负向，如果是0 ，后面无坐标
            public double  X ;
            public double  Y ;
        }
        private Hashtable getNeedLineXY2(string filepath)
        {
            Hashtable htLine = new Hashtable(); //此处只记录文件位置 
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            using (StreamReader reader = new StreamReader(filepath, gb2312))
            {
                string aLine = "";
                long pos = 0;
                while (!reader.EndOfStream)
                {
                    aLine = reader.ReadLine();
                    pos += aLine.Length + 2;
                    long  bsm = Convert.ToInt64(aLine);
                    aLine = reader.ReadLine();
                    htLine.Add(bsm, pos);
                    //下一行的位置要修正
                    pos += aLine.Length + 2;


                }
                reader.Close();
            }

            return htLine;
        }


        //获取线 坐标，从线文件中，根据位置,t同时读取下100行备用
        private string getALineByPos(string linedatafile, long linePos, ref Hashtable htMemoLine)
        {

            DateTime beforDT = System.DateTime.Now;

            string line = "";
            if (htMemoLine.Count >= this.bufferSize) //超过3000，重建缓存
                htMemoLine.Clear();
            System.IO.FileStream lineFs = new System.IO.FileStream(linedatafile, FileMode.Open);
            StreamReader lineSr = new StreamReader(lineFs, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                lineFs.Seek(linePos, SeekOrigin.Begin);
                line = lineSr.ReadLine().Trim().ToUpper();

                if (!lineSr.EndOfStream)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (lineSr.EndOfStream)
                        {
                            break;
                        }
                        //实际读了20行到内存中备用
                        string line2 = lineSr.ReadLine().Trim().ToUpper();
                        Int32 iBsm = 0;
                        Int32.TryParse(line2, out iBsm);
                        if (lineSr.EndOfStream)
                            break;
                        line2 = lineSr.ReadLine().Trim().ToUpper();
                        if (!htMemoLine.ContainsKey(iBsm))
                        {
                            htMemoLine.Add(iBsm, line2);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                lineSr.Close();
                lineFs.Close();
            }

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            fileTick += ts.TotalMilliseconds;//计算时间


            return line;
        }
        public VCTAPolygon2Shp(string _tabName, Hashtable _ht, string _shpile)
        {
            this.aTabName = _tabName;
            this.ghasTableStructure = _ht;
            this.shpfile = _shpile;

            //非独立面
            NoOnlyPolygon.Add("DLTB", "DLJX");
            NoOnlyPolygon.Add("XZQ", "XZQJX");
            NoOnlyPolygon.Add("ZD", "JZX");



            
        }


        /// <summary>
        /// 根据坐标 生成面
        /// </summary>
        /// <param name="arPoints"></param>
        /// <returns></returns>
        private OSGeo.OGR.Geometry getAGeoByPts(ArrayList arPoints, Layer oLayer)
        {
            

            OSGeo.OGR.Geometry geo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbPolygon);
            OSGeo.OGR.Geometry ringGeo = new OSGeo.OGR.Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
            foreach (APointXY aPt in arPoints)
            {
                if (aPt.flag == 0)
                {

                    geo.AddGeometryDirectly(ringGeo);
                    ringGeo = new Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
                }
                else
                {
                    ringGeo.AddPoint(aPt.X, aPt.Y, 0);
                }

            }
            geo.AddGeometryDirectly(ringGeo);

            return geo;
            
                       

        }


        public void InAPolygonClass()
        {


            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;

            string sDataFile = sTmpFilePath + "\\POLYGON_" + aTabName.ToUpper() + ".DATA";
            string sAttrFile = sTmpFilePath + "\\POLYGON_" + aTabName.ToUpper() + ".ATTR";
            string LineBm = aTabName;
            if (NoOnlyPolygon.ContainsKey(aTabName))
            {
                LineBm = NoOnlyPolygon[aTabName].ToString().Trim().ToUpper();
            }
            string linedatafile = sTmpFilePath + "\\LINE_" + LineBm + ".DATA";  //对应线文件名

            TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[aTabName];

            if (!System.IO.File.Exists(sDataFile) || !System.IO.File.Exists(sAttrFile))
            {
                //找不到该文件。掠过
               
                return;
            }

            //首先读取对应线层的 标识码和坐标
            Hashtable needLineHas = this.getNeedLineXY2(linedatafile);


            #region 准备工作
            //OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            //// 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
            string strVectorFile = this.shpfile;
            OSGeo.OGR.Ogr.RegisterAll();
            string strDriverName = "ESRI Shapefile";
            int count = Ogr.GetDriverCount();
            OSGeo.OGR.Driver oDriver = Ogr.GetDriverByName(strDriverName);
            if (oDriver == null)
            {
                return;
            }
            DataSource ds = Ogr.Open(strVectorFile, 1);
            if (ds == null)
            {
                return;
            }
            int iLayerCount = ds.GetLayerCount();

            // 获取第一个图层  
            Layer oLayer = ds.GetLayerByIndex(0);
            if (oLayer == null)
            {
                return;
            }
            FeatureDefn oDefn = oLayer.GetLayerDefn();
            Feature oFeature=null;
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

            StreamReader dataReader = new StreamReader(sDataFile, Encoding.GetEncoding("GB2312"));
            StreamReader attrReader = new StreamReader(sAttrFile, Encoding.GetEncoding("GB2312"));
            Hashtable htLineBuffer = new Hashtable(); //线缓存
            try
            {
                while (dataReader.EndOfStream == false)
                {

                    string aLine = dataReader.ReadLine();
                    long  nMBBSM = Convert.ToInt64(aLine);
                    aLine = dataReader.ReadLine();  //所有线标识码
                    string[] aArray = aLine.Split(this.SgSeparator.ToCharArray());
                    ArrayList arrNO = new ArrayList();  //带符号的当前 线号
                    for (int j = 0; j < aArray.Length; j++)
                    {
                        long  nBSM = Convert.ToInt64(aArray[j]);
                        arrNO.Add(nBSM);
                    }
                    #region   //读取所有线节点坐标
                    ArrayList arPtXYObj = new ArrayList();
                    for (int i = 0; i < arrNO.Count; i++)
                    {
                        long nbsm = (long)arrNO[i];

                        int nBSM = (int)arrNO[i]; //线的标识码;
                        int nAbsBsm = Math.Abs(nBSM);  //绝对值，有可能是负的
                        if (nAbsBsm == 0)
                        {
                            APointXY aObj = new APointXY();
                            aObj.flag = 0;
                            arPtXYObj.Add(aObj);
                            continue;
                        }
                        string ptLines = "";
                        if (htLineBuffer.ContainsKey(nAbsBsm))
                        {
                            ptLines = (string)htLineBuffer[nAbsBsm];
                        }
                        else
                        {
                            //如果没有，则，根据位置索引找其坐标
                            long linePos = (long)needLineHas[nAbsBsm];
                            ptLines = getALineByPos(linedatafile, linePos, ref htLineBuffer);
                        }
                        if (ptLines.Trim() == "")
                            continue;
                        string[] sPts = ptLines.Split(';');
                        if (nBSM > 0)
                        {
                            foreach (string sPt in sPts)
                            {
                                string[] sXY = sPt.Split(SgSeparator.ToCharArray());
                                double dX = Convert.ToDouble(sXY[0]);
                                double dY = Convert.ToDouble(sXY[1]);
                                APointXY aObj = new APointXY();
                                aObj.flag = 1;
                                aObj.X = dX;
                                aObj.Y = dY;
                                arPtXYObj.Add(aObj);

                            }
                        }
                        else if (nBSM < 0)
                        {
                            for (int k = sPts.Length - 1; k >= 0; k--)
                            {
                                string sPt = sPts[k].ToString();
                                string[] sXY = sPt.Split(SgSeparator.ToCharArray());
                                double dX = Convert.ToDouble(sXY[0]);
                                double dY = Convert.ToDouble(sXY[1]);
                                APointXY aObj = new APointXY();
                                aObj.flag = 1;
                                aObj.X = dX;
                                aObj.Y = dY;
                                arPtXYObj.Add(aObj);
                            }
                        }
                    }
                    #endregion

                    //DateTime beforDT = DateTime.Now;
                    Geometry geo = getAGeoByPts(arPtXYObj, oLayer);
                    oFeature = new Feature(oDefn);
                    oFeature.SetGeometry(geo);
                    oFeature.SetField("BSM", nMBBSM);

                    //DateTime afterDT = System.DateTime.Now;
                    //TimeSpan ts = afterDT.Subtract(beforDT);
                    //geoOpTick += ts.TotalMilliseconds;//计算时间

                    //写属性
                    string sAttLine = attrReader.ReadLine();
                    
                    if (sAttLine != null)
                    {
                        string[] aAttrArray = sAttLine.Split(',');

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
                }

            }
            catch (Exception ex)
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
