using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;

using OSGeo.GDAL;
using OSGeo.OGR;

namespace RCIS.DataInterface.VCT3In
{
    public class VCT3APolygon2Shp
    {
        string shpfile = "";

        private string SgSeparator = ",";
        //private Hashtable NoOnlyPolygon = new Hashtable();
        string aTabName = "";

        Hashtable ghasTableStructure = null;

        public long bufferSize = 2048;
        

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


        private string getAlineByPos(string linedatafile, long linePos)
        {
            string line = "";         
            System.IO.FileStream lineFs = new System.IO.FileStream(linedatafile, FileMode.Open);
            StreamReader lineSr = new StreamReader(lineFs, System.Text.Encoding.GetEncoding("GB2312"));
            try
            {
                lineFs.Seek(linePos, SeekOrigin.Begin);
                line = lineSr.ReadLine().Trim().ToUpper();                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                lineSr.Close();
                lineFs.Close();
            }
            return line;
        }

        //获取线 坐标，从线文件中，根据位置,t同时读取下100行备用
        private string getALineByPos(string linedatafile, long linePos, ref Hashtable htMemoLine)
        {
                      

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
                        //实际读了100行到内存中备用
                        string line2 = lineSr.ReadLine().Trim().ToUpper();
                        long  iBsm = 0;
                        long .TryParse(line2, out iBsm);
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

           

            return line;
        }
        
        public VCT3APolygon2Shp(string _tabName, Hashtable _ht, string _shpile)
        {
            this.aTabName = _tabName;
            this.ghasTableStructure = _ht;
            this.shpfile = _shpile;

            //非独立面           
            //NoOnlyPolygon.Add("XZQ", "XZQJX");
                        
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
           
            try
            {
                foreach (APointXY aPt in arPoints)
                {
                    if (aPt.flag == 0)
                    {
                        ringGeo.CloseRings();
                        geo.AddGeometryDirectly(ringGeo);
                        ringGeo = new Geometry(OSGeo.OGR.wkbGeometryType.wkbLinearRing);
                    }
                    else
                    {
                        ringGeo.AddPoint(aPt.X, aPt.Y, 0);
                    }
                }
                ringGeo.CloseRings();
                geo.AddGeometryDirectly(ringGeo);               
            }
            catch (Exception ex)
            {
            }

            return geo;
            
                       

        }


        public void InAPolygonClass()
        {


            string sTmpFilePath = Application.StartupPath + "\\VCTProcess";

            string sDataFile = sTmpFilePath + "\\POLYGON_" + aTabName.ToUpper() + ".DATA";
            string sAttrFile = sTmpFilePath + "\\POLYGON_" + aTabName.ToUpper() + ".ATTR";
            string LineBm = aTabName;
            //if (NoOnlyPolygon.ContainsKey(aTabName))
            //{
            //    LineBm = NoOnlyPolygon[aTabName].ToString().Trim().ToUpper();
            //}

            string linedatafile = sTmpFilePath + "\\LINE_" + LineBm + ".DATA";  //对应线文件名
            
            if (!System.IO.File.Exists(linedatafile))
            {
                linedatafile = sTmpFilePath + "\\LINE_1099000000.DATA";  //没有对应先文件，就这么处理
            }

            TableStructBeginEnd3 curItem = (TableStructBeginEnd3)ghasTableStructure[aTabName];

            if (!System.IO.File.Exists(sDataFile) || !System.IO.File.Exists(sAttrFile))
            {
                //找不到该文件。掠过              
                return;
            }

            //首先读取对应线层的 标识码和 坐标位置
            Hashtable needLineHas = this.getNeedLineXY2(linedatafile);
            
            #region 准备工作
            //OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            // 为了使属性表字段支持中文，请添加下面这句
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
            int nAttrFldGs = curItem.aZDMCs.Count;  //结构中字段数量
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
                    else if (sType.Equals("CHAR")  || sType.Equals("NVCHAR"))
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

                    try
                    {
                        string aLine = dataReader.ReadLine();
                        long nXH = Convert.ToInt64(aLine);
                        

                        aLine = dataReader.ReadLine();  //所有线标识码
                        string[] aArray = aLine.Split(this.SgSeparator.ToCharArray());
                        ArrayList arrNO = new ArrayList();  //带符号的当前 线号
                        for (int j = 0; j < aArray.Length; j++)
                        {
                            long nBSM = Convert.ToInt64(aArray[j]);
                            arrNO.Add(nBSM);
                        }
                        #region   //读取所有线节点坐标
                        ArrayList arPtXYObj = new ArrayList();
                        for (int i = 0; i < arrNO.Count; i++)
                        {
                            long nBSM = (long)arrNO[i]; //线的标识码;
                            long nAbsBsm = Math.Abs(nBSM);  //绝对值，有可能是负的
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
                                //如果当前缓存中有该标识码，则取出
                                ptLines = (string)htLineBuffer[nAbsBsm];
                            }
                            else
                            {
                                //如果没有，则从新到文件中取找，根据位置索引找其坐标
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
                    }
                    catch (Exception ex)
                    {
                    }
                    
                    try
                    {
                        string sAttLine = attrReader.ReadLine();

                        #region 写入属性
                        if (sAttLine != null)
                        {
                            string[] aAttrArray = sAttLine.Split(',');

                            //从第二个 开始导入，第一个是唯一数字标识
                            for (int k = 1; k < nAttrFldGs + 1; k++)
                            {
                                if (k == 1)
                                {                                   
                                    oFeature.SetField("BSM", aAttrArray[1]);
                                    continue;
                                }

                                if (aAttrArray.Length == k) break;
                                int nPos = nFldIndx[k - 1];
                                int nType = nFldType[k - 1];
                                if (nPos != -1)
                                {
                                    object oo = aAttrArray[k];
                                    string ss = oo.ToString().Trim();
                                    double dValue = 0;
                                    string sValue = "";
                                    
                                    if (nType == 1)
                                    {
                                        if (ss.Trim() != "")
                                        {
                                            int ival = int.Parse(ss);
                                            oFeature.SetField(nPos, ival);
                                        }
                                    }
                                    else if (nType == 2)
                                    {
                                        double.TryParse(ss, out dValue);
                                        oFeature.SetField(nPos, dValue);
                                    }
                                    else if (nType == 3)
                                    {
                                        sValue = ss;
                                        oFeature.SetField(nPos, sValue);
                                    }
                                    else
                                    {
                                        if (ss.Length >= 8)
                                        {
                                            int year = int.Parse(ss.Substring(0, 4));
                                            int month = int.Parse(ss.Substring(4, 2));
                                            int day = int.Parse(ss.Substring(6, 2));
                                            oFeature.SetField(nPos, year, month, day, 0, 0, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
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
                if (oLayer != null) oLayer.Dispose();  //释放
                if (ds != null) ds.Dispose();
            }
           
        }

    }
}
