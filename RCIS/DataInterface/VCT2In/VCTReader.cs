using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;

namespace RCIS.DataInterface.VCT2
{
    public  class VCTReader
    {

        public bool IsInExtTable = false;

        private string sgSeparator = ",";
        /// <summary>
        /// 分割符 ,
        /// </summary>
        public string SgSeparator
        {
            get { return sgSeparator; }
            set { sgSeparator = value; }
        }


        string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;

        public Hashtable ghasTableStructure=new Hashtable();     //以'属性表名'为Key,存放TableStructClass信息
        public  Hashtable ghasFeatuerCode=new Hashtable();   //该Hash以"要素代码+要素名称+属性表名"为KEY,


        private string GetSrPrjPath(string sDir, string subStr)
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

        public ISpatialReference getSpatialRef(string vctFile)
        {
            int iMerian = this.GetMeridian(vctFile);

            iMerian = iMerian / 3;
            string sSrprjFile = GetSrPrjPath(Application.StartupPath + @"\srprj\CGCS2000", iMerian.ToString());
            ISpatialReference spatialReference = null;
            if (System.IO.File.Exists(sSrprjFile))
            {
                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                spatialReference = RCIS.GISCommon.SpatialRefHelper.ConstructCoordinateSystem(true, sSrprjFile);
            }
            else
            {
                spatialReference = new UnknownCoordinateSystemClass();
            }
            return spatialReference;

        }

        /// <summary>
        /// 根据要素代码获得属性表名
        /// </summary>
        /// <param name="nYSDM2"></param>
        /// <returns></returns>
        private string getSXBMByYsdm(int nYSDM2)
        {
            string tabName = "";
            foreach (string key in ghasFeatuerCode.Keys)
            {
                FeatureCodeBeginEnd curItem = (FeatureCodeBeginEnd)ghasFeatuerCode[key];
                if (curItem.sYSDM.ToUpper() == nYSDM2.ToString().ToUpper())
                {
                    tabName = curItem.sSXBM;
                    break;
                }
            }
            return tabName;
        }

        private string getJHLXByYsdm(string tableName)
        {
            string jhlx = "";
            foreach (string key in ghasFeatuerCode.Keys)
            {
                FeatureCodeBeginEnd curItem = (FeatureCodeBeginEnd)ghasFeatuerCode[key];
                if (curItem.sSXBM.ToUpper() == tableName.ToUpper())
                {
                    jhlx = curItem.sJHLX;
                    break;
                }
            }
            return jhlx;
        }

        private Hashtable NoOnlyPolygon = new Hashtable();
        public VCTReader()
        { 
            //非独立面
            NoOnlyPolygon.Add("DLTB", "DLJX");
            NoOnlyPolygon.Add("XZQ", "XZQJX");
            NoOnlyPolygon.Add("ZD", "JZX");
        
        }
        /// <summary>
        /// 读取下一个非空行
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private string  ReadNextNoBlankRow(StreamReader sr)
        {
            if (sr.EndOfStream) return "";
            string s=sr.ReadLine().Trim().ToUpper();
            while (s=="")
            {
                s = sr.ReadLine().Trim().ToUpper();
            }
            return s;
        }
        
        /// <summary>
        /// 获取所有要素类的哈希表,及字段结构哈希表
        /// </summary>
        public bool GetFeatureCodes(StreamReader VCTReader)
        {
            bool bOK=false;
            string sReadLine="";
            while ((sReadLine = ReadNextNoBlankRow(VCTReader)) != "")
            {
                //进入 featurebegin
                if (sReadLine.ToUpper() == "FeatureCodeBegin".ToUpper())
                {
                    break;
                }                

            }
            while ((sReadLine = ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("FeatureCodeEnd".ToUpper()))
                {
                    bOK = true;
                    break;
                }
                #region 获取要素类
                string[] sArray = sReadLine.Split(sgSeparator.ToCharArray());
                string sYSDM = sArray[0].Trim().ToUpper();
                string sYSMC = sArray[1].Trim().ToUpper();
                string sJHLX = sArray[2].Trim().ToUpper();
                string sColorR = sArray[3].Trim().ToUpper();
                string sColorG = sArray[4].Trim().ToUpper();
                string sColorB = sArray[5].Trim().ToUpper();
                string sSXBM = sArray[6].Trim().ToUpper();     //基本属性表名 
                if (sArray.Length > 7)
                {
                    //有扩展属性表:
                    int nGS = sArray.Length - 7;
                    ArrayList aKZSXBMs = new ArrayList();
                    for (int k = 7; k < sArray.Length; k++)
                        aKZSXBMs.Add(sArray[k].Trim().ToUpper());

                    FeatureCodeBeginEnd curItem = new FeatureCodeBeginEnd();
                    curItem.sYSDM = sYSDM;
                    curItem.sYSMC = sYSMC.Trim().ToUpper();
                    curItem.sJHLX = sJHLX.Trim().ToUpper();
                    curItem.sColorR = sColorR.Trim().ToUpper();
                    curItem.sColorG = sColorG.Trim().ToUpper();
                    curItem.sColorB = sColorB.Trim().ToUpper();
                    curItem.sSXBM = sSXBM.Trim().ToUpper();
                    curItem.aKZSXBM = aKZSXBMs;
                    //curItem.bSel = false;

                    string sKey = sYSDM + "," + sYSMC + "," + sSXBM;
                    if (ghasFeatuerCode.ContainsKey(sKey) == false)
                        ghasFeatuerCode.Add(sKey, curItem);
                }
                else
                {
                    FeatureCodeBeginEnd curItem = new FeatureCodeBeginEnd();
                    curItem.sYSDM = sYSDM;
                    curItem.sYSMC = sYSMC.Trim().ToUpper();
                    curItem.sJHLX = sJHLX.Trim().ToUpper();
                    curItem.sColorR = sColorR.Trim().ToUpper();
                    curItem.sColorG = sColorG.Trim().ToUpper();
                    curItem.sColorB = sColorB.Trim().ToUpper();
                    curItem.sSXBM = sSXBM.Trim().ToUpper();


                    string sKey = sYSDM + "," + sYSMC + "," + sSXBM;
                    if (ghasFeatuerCode.ContainsKey(sKey) == false)
                        ghasFeatuerCode.Add(sKey, curItem);
                }
                #endregion
            }
            

            
            return bOK;
        }

        /// <summary>
        /// 获取所有结构
        /// </summary>
        /// <param name="VCTReader"></param>
        /// <returns></returns>
        public bool GetAllTableStruct(StreamReader VCTReader)
        {
            bool bOK = false;
            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("TABLESTRUCTUREBEGIN"))
                    break;
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                try
                {
                   
                    //后面就是结构信息
                    if (sReadLine.Equals("TABLESTRUCTUREEND"))
                    {
                        bOK = true;
                        break;
                    }
                    string[] sArray = sReadLine.Split(sgSeparator.ToCharArray());
                    string sTblName = sArray[0].Trim().ToUpper(); //表名
                    int nFlds = Convert.ToInt32(sArray[1]);

                    ArrayList aZDMCs = new ArrayList();   //字段名称
                    ArrayList aZDLXs = new ArrayList();    //字段类型: double,char,date
                    ArrayList aZDJD = new ArrayList();     //字段的精度. double[10.3],char[20],date[];
                    ArrayList aZDJD2 = new ArrayList();

                    for (int i = 0; i < nFlds; i++)
                    {
                        #region 获取字段信息
                        sReadLine = ReadNextNoBlankRow(VCTReader);
                        string[] sFlds = sReadLine.Split(sgSeparator.ToCharArray());

                        int nn = sFlds.Length;
                        string sFldName = sFlds[0].Trim().ToUpper();
                        string sFldType = sFlds[1].Trim().ToUpper();
                        int nCharLen = 255;
                        int nIntLen = 10;
                        int nDblPre = 15, nDblScale = 3;
                        string sNeedType = "";
                        if (sFldType.Equals("CHAR"))
                        {
                            nCharLen = Convert.ToInt32(sFlds[2]);
                            sNeedType = "CHAR";
                        }
                        else if (sFldType.Equals("INT") == true || sFldType.Equals("INTEGER") == true)
                        {
                            nIntLen = Convert.ToInt32(sFlds[2]);
                            sNeedType = "INT";
                        }
                        else if (sFldType.Equals("FLOAT") == true || sFldType.Equals("DOUBLE") == true)
                        {
                            nDblPre = Convert.ToInt32(sFlds[2]);
                            if (sFlds.Length > 3)
                            {
                                nDblScale = Convert.ToInt32(sFlds[3]);
                            }
                            sNeedType = "DOUBLE";
                        }
                        else if (sFldType.Equals("DATE") == true)
                        {
                            sNeedType = "CHAR";
                            nCharLen = Convert.ToInt32(sFlds[2]);
                        }
                        else
                        {
                            sNeedType = "CHAR";
                        }

                        //...
                        aZDMCs.Add(sFldName);
                        aZDLXs.Add(sNeedType);
                        if (sNeedType.Equals("DOUBLE"))
                        {
                            aZDJD.Add(nDblPre);
                            aZDJD2.Add(nDblScale);
                        }
                        else if (sNeedType.Equals("CHAR"))
                        {
                            aZDJD.Add(nCharLen);
                            aZDJD2.Add("");
                        }
                        else if (sNeedType.Equals("INT"))
                        {
                            aZDJD.Add(nIntLen);
                            aZDJD2.Add("");
                        }
                        else
                        {
                            //DATE:
                            aZDJD.Add("");
                            aZDJD2.Add("");
                        }
                        #endregion

                    } //for(int i=0;

                    TableStructBeginEnd curItem = new TableStructBeginEnd();
                    curItem.nZDGS = nFlds;
                    curItem.aZDLXs = aZDLXs;
                    curItem.aZDMCs = aZDMCs;
                    curItem.aZDJD = aZDJD;
                    curItem.aZDJD2 = aZDJD2;

                    ghasTableStructure.Add(sTblName, curItem);   //以属性表名为KEY,保存每个表的信息


                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return bOK;
        }


        public int GetMeridian(string vctfile)
        {
            int dMeridian = 117;
            if ((vctfile.Trim() == "") || (!File.Exists(vctfile)))
            {
                throw new Exception("VCT文件不存在！");
                //return -1;
            }
            bool bOK = false;
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            StreamReader VCTReader = new StreamReader(vctfile, gb2312);
            string sReadLine = VCTReader.ReadLine().Trim().ToUpper();
            #region 读取headbegin--->HeadEnd之间的信息
            while (sReadLine != null)
            {
                if (sReadLine.Equals("HEADBEGIN"))
                {
                    string aLine = VCTReader.ReadLine().Trim().ToUpper();
                    while (aLine != null)
                    {
                        //Meridian
                        if (aLine.Length > 9 && aLine.Substring(0, 8).ToUpper().Equals("MERIDIAN") == true)
                        {
                            string sMeridian = aLine.Substring(9).Trim();
                            try
                            {
                                dMeridian = Convert.ToInt32(sMeridian);
                            }
                            catch { }
                            bOK = true;
                            break;
                        }
                        else
                            if (aLine.Equals("HEADEND"))
                            {
                                bOK = true;
                                break;
                            }
                        aLine = VCTReader.ReadLine().Trim().ToUpper();
                    }
                }

                if (bOK == true || VCTReader.EndOfStream == true)
                    break;
                sReadLine = VCTReader.ReadLine().Trim().ToUpper();
            }
            VCTReader.Close();
            #endregion
            if (bOK == false)
            {

                throw new Exception("VCT文件格式错误: 没有HeadEnd标识!");
                //return -1;
            }
            return dMeridian;

        }


        /// <summary>
        /// 将点文件拆分，并且记录点表名和 bsm顺序号，要根据这个顺序号 ，整理属性文件
        /// </summary>
        /// <param name="VCTReader"></param>
        /// <returns></returns>
        public Dictionary<string, ArrayList> getNewPointDataFile(StreamReader VCTReader)
        {
            Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");
                       
            Dictionary<string, ArrayList> dicPointTables = new Dictionary<string, ArrayList>();

            StreamWriter dataWriter = null;
            int oldysdm = 0;
            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine == "POINTBEGIN")
                    break; //还没开始
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                
                if (sReadLine.Equals("POINTEND"))
                {
                    break;
                }
                int nMBBSM = Convert.ToInt32(sReadLine);  //标志吗
                #region 下面跟着4行
                //要素代码
                //层名
                //点特征类型
                //X,Y
                string[] sNext4Line = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);
                    sNext4Line[i] = sReadLine;
                } //for(int i=0;
                int newYsdm = Convert.ToInt32(sNext4Line[0]);
                string tabName = getSXBMByYsdm(newYsdm);
                if (oldysdm != newYsdm)
                {
                    
                    if (!dicPointTables.ContainsKey(tabName))
                    {
                        //如果不存在，则创建之 
                        ArrayList nPBsm = new ArrayList();
                        dicPointTables.Add(tabName, nPBsm);
                       
                        if (dataWriter != null)
                        {
                            dataWriter.Close();
                        }
                        //创建之  
                        string sDataFile = sTmpFilePath + "\\POINT_" + tabName + ".DATA";
                        dataWriter = new StreamWriter(sDataFile, false, gb2312);

                        oldysdm = newYsdm;
                    }
                }
                ArrayList tmpList = dicPointTables[tabName];
                tmpList.Add(nMBBSM);
                dicPointTables[tabName] = tmpList;

                //只写入标识码，点坐标两行
                dataWriter.WriteLine(nMBBSM);
                dataWriter.WriteLine(sNext4Line[3]);
                

                #endregion

            }
            if (dataWriter != null)
                dataWriter.Close();

            return dicPointTables;

        }

        public Dictionary<string, ArrayList> getNewPolygonDataFile(StreamReader VCTReader)
        {
            Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");
            Dictionary<string, ArrayList> dicPolygonTables = new Dictionary<string, ArrayList>();

            StreamWriter dataWriter = null;
            int oldysdm = 0;
            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("POLYGONBEGIN")) break;
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                
                if (sReadLine.Equals("POLYGONEND"))
                    break;

                int nMBBSM = Convert.ToInt32(sReadLine);  //标志吗
                #region //下面跟着N行

                //要素代码
                //层名
                //注记点坐标X,Y
                //牵涉的线数n[每行8个线BSM]
                //L1,L2,...,L8
                //...
                //...,...,.....Ln
                int newYsdm = 0;
                sReadLine = this.ReadNextNoBlankRow(VCTReader);
                newYsdm = Convert.ToInt32(sReadLine);
                sReadLine = this.ReadNextNoBlankRow(VCTReader);
                sReadLine = this.ReadNextNoBlankRow(VCTReader);
                sReadLine = this.ReadNextNoBlankRow(VCTReader);
                int nLineGS = Convert.ToInt32(sReadLine);

                //因为每行8个，所以求下行数:
                int nNeedLines = nLineGS / 8;
                if (nLineGS % 8 != 0)
                    nNeedLines++;
                ArrayList noLines = new ArrayList();
                for (int i = 0; i < nNeedLines; i++)
                {
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);
                    noLines.Add(sReadLine);
                }

                #endregion 
                string tabName = getSXBMByYsdm(newYsdm);
                
                if (oldysdm != newYsdm)
                {
                    if (!dicPolygonTables.ContainsKey(tabName))
                    {
                        ArrayList nPBsm = new ArrayList();
                        dicPolygonTables.Add(tabName, nPBsm);
                        //如果不存在
                        if (dataWriter != null)
                        {
                            dataWriter.Close();
                        }
                        string sDataFile = sTmpFilePath + "\\POLYGON_" + tabName + ".DATA";
                        dataWriter = new StreamWriter(sDataFile, false, gb2312);
                        oldysdm = newYsdm;
                    }
                }
                dataWriter.WriteLine(nMBBSM);

                ArrayList tmpList = dicPolygonTables[tabName];
                tmpList.Add(nMBBSM);
                dicPolygonTables[tabName] = tmpList;

                string slines = "";
                foreach (string anoline in noLines)
                {
                    slines += anoline + ",";
                }
                if (slines.EndsWith(","))
                {
                    slines = slines.Remove(slines.Length - 1, 1);
                }
                dataWriter.WriteLine(slines);
            }
            if (dataWriter != null)
                dataWriter.Close();

            return dicPolygonTables;

        }




        /// <summary>
        /// 返回标识码 ，以便属性进行对应
        /// </summary>
        /// <param name="VCTReader"></param>
        /// <returns></returns>
        public Dictionary<string, ArrayList> getNewLineDataFile2(StreamReader VCTReader,ref Hashtable htLineDataPos)
        {
            Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");
            Dictionary<string, ArrayList> dicLineTables = new Dictionary<string, ArrayList>();

            StreamWriter dataWriter = null;
            int oldysdm = 0;
            long pos = 0;

            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("LINEBEGIN"))
                {
                    break;
                }
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {

                if (sReadLine.Equals("LINEEND"))  //结尾跳出
                    break;

                int nMBBSM = Convert.ToInt32(sReadLine);  //标志吗
                #region //下面跟着N行
                //要素代码
                //层名
                //线特征类型
                //点数n
                //X1,Y1;
                //...
                //Xn,Yn


                int newYsdm = 0;
                sReadLine = ReadNextNoBlankRow(VCTReader);
                newYsdm = Convert.ToInt32(sReadLine);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                int nPointGS = 0;
                nPointGS = Convert.ToInt32(sReadLine);

                string strPoints = "";
                for (int i = 0; i < nPointGS; i++)
                {
                    sReadLine = ReadNextNoBlankRow(VCTReader);
                    strPoints += (sReadLine) + ";";

                }
                if (strPoints.EndsWith(";"))
                    strPoints = strPoints.Remove(strPoints.Length - 1, 1);
                #endregion
                string tabName = getSXBMByYsdm(newYsdm);
                string jhlx = getJHLXByYsdm(tabName);
                if (oldysdm != newYsdm)
                {
                    //如果不存在，则创建之 
                    if (dataWriter != null)
                    {
                        dataWriter.Close();
                    }
                    //创建之  
                    string sDataFile = sTmpFilePath + "\\LINE_" + tabName + ".DATA";
                    dataWriter = new StreamWriter(sDataFile, false, gb2312);
                    pos = 0;

                    oldysdm = newYsdm;

                    if (!dicLineTables.ContainsKey(tabName))
                    {
                        if (jhlx == "LINE")
                        {
                            //如果是拓扑来的线，不记录标识码
                            ArrayList nPBsm = new ArrayList();
                            dicLineTables.Add(tabName, nPBsm);
                        }
                        
                    }

                }
                dataWriter.WriteLine(nMBBSM); //输出bsm
                pos += nMBBSM.ToString().Trim().Length + 2;

                if (jhlx == "LINE")
                {
                    //如果是拓扑来的线，不记录标识码
                    ArrayList tmpList = dicLineTables[tabName];
                    tmpList.Add(nMBBSM);
                    dicLineTables[tabName] = tmpList;
                }

                
                //输出一行
                dataWriter.WriteLine(strPoints);// 输出点坐标到一行               
                aLineGeoPos aPos = new aLineGeoPos(pos, strPoints.Length);  //记录该标识码 在文件中的位置
                htLineDataPos.Add(nMBBSM, aPos);

                pos += strPoints.Length + 2; //2 是换行
                
               
            }
            if (dataWriter != null)
                dataWriter.Close();

            return dicLineTables;

        }


        
         
        /// <summary>
        /// 返回标识码 ，以便属性进行对应
        /// </summary>
        /// <param name="VCTReader"></param>
        /// <returns></returns>
        public Dictionary<string, ArrayList> getNewLineDataFile(StreamReader VCTReader)
        {
            Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");
            Dictionary<string, ArrayList> dicLineTables = new Dictionary<string, ArrayList>();

            StreamWriter dataWriter = null;
            int oldysdm = 0;
            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("LINEBEGIN"))
                {
                    break;
                }
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                
                if (sReadLine.Equals("LINEEND"))  //结尾跳出
                    break;

                int nMBBSM = Convert.ToInt32(sReadLine);  //标志吗
                #region //下面跟着N行
                //要素代码
                //层名
                //线特征类型
                //点数n
                //X1,Y1;
                //...
                //Xn,Yn

                
                int newYsdm = 0;
                sReadLine = ReadNextNoBlankRow(VCTReader);
                newYsdm = Convert.ToInt32(sReadLine);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                int nPointGS = 0;
                nPointGS = Convert.ToInt32(sReadLine);

                string strPoints = "";
                for (int i = 0; i < nPointGS; i++)
                {
                    sReadLine = ReadNextNoBlankRow(VCTReader);
                    strPoints += (sReadLine) + ";";

                }
                if (strPoints.EndsWith(";"))
                    strPoints = strPoints.Remove(strPoints.Length - 1, 1);
                #endregion
                string tabName = getSXBMByYsdm(newYsdm);
                string jhlx = getJHLXByYsdm(tabName);
                if (oldysdm != newYsdm)
                {
                    if (dataWriter != null)
                    {
                        dataWriter.Close();
                    }
                    //创建之  
                    string sDataFile = sTmpFilePath + "\\LINE_" + tabName + ".DATA";
                    dataWriter = new StreamWriter(sDataFile, false, gb2312);
                    oldysdm = newYsdm;

                    if (!dicLineTables.ContainsKey(tabName))
                    {
                        if (jhlx == "LINE")
                        {
                            //如果是拓扑来的线，不记录标识码
                            ArrayList nPBsm = new ArrayList();
                            dicLineTables.Add(tabName, nPBsm);
                        }

                    }
                    
                }
                dataWriter.WriteLine(nMBBSM); //输出bsm

                if (jhlx == "LINE")
                {
                    //如果是拓扑来的线，不记录标识码
                    ArrayList tmpList = dicLineTables[tabName];
                    tmpList.Add(nMBBSM);
                    dicLineTables[tabName] = tmpList;
                }


                //输出一行
                dataWriter.WriteLine(strPoints);// 输出点坐标到一行
            }
            if (dataWriter != null)
                dataWriter.Close();

            return dicLineTables;

        }


        /// <summary>
        /// 不对属性文件进行重新排序
        /// </summary>
        /// <param name="VCTReader"></param>
        public void GetNewAttrFile(StreamReader VCTReader)
        {
            
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            StreamWriter dataWriter = null;

            string sReadLine = "";
            
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("ATTRIBUTEBEGIN"))
                {
                    break;
                }
            }
            sReadLine = this.ReadNextNoBlankRow(VCTReader);
            //下面一行 可能是 attr end ,可能是  另一个表开头
            if (sReadLine.Equals("Attributeend".ToUpper())) return;

            string oldTabName = sReadLine;  //表名
            string jhlx = this.getJHLXByYsdm(oldTabName);
            if (jhlx != "")
            {
                string sDataFile = sTmpFilePath + "\\" + jhlx + "_" + oldTabName + ".ATTR";
                dataWriter = new StreamWriter(sDataFile, false, gb2312);
            }
            else
            {
                //可能是扩展表
                string sDataFile = sTmpFilePath + "\\EXT_" + oldTabName + ".ATTR";
                dataWriter = new StreamWriter(sDataFile, false, gb2312);
            }
            //继续读取
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("TableEnd".ToUpper()))
                {


                    #region  //表示一个表结束，开始另一个表
                    if (dataWriter != null)
                        dataWriter.Close();
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);
                    if (sReadLine.Equals("Attributeend".ToUpper()))
                        break;
                    //读取下一个表名
                    oldTabName = sReadLine;
                    jhlx = this.getJHLXByYsdm(oldTabName);
                    if (jhlx != "")
                    {
                        string sDataFile = sTmpFilePath + "\\" + jhlx + "_" + oldTabName + ".ATTR";
                        dataWriter = new StreamWriter(sDataFile, false, gb2312);
                    }
                    else
                    {
                        //可能是扩展表
                        string sDataFile = sTmpFilePath + "\\EXT_" + oldTabName + ".ATTR";
                        dataWriter = new StreamWriter(sDataFile, false, gb2312);
                    }
                    #endregion


                }
                else
                {
                    dataWriter.WriteLine(sReadLine);
                }
            }
            if (dataWriter != null)
                dataWriter.Close();

        }

        public void GetNewAttrFile(StreamReader VCTReader,Dictionary<string,ArrayList> dicGeoBsm   )
        {
            
            Hashtable hasIndex=new Hashtable();
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            
           
            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("ATTRIBUTEBEGIN"))
                {
                    break;
                }
            }
            sReadLine = this.ReadNextNoBlankRow(VCTReader);
            //下面一行 可能是 attr end ,可能是  另一个表开头
            if (sReadLine.Equals("Attributeend".ToUpper())) return;
            string tableName=sReadLine;
            
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("TableEnd".ToUpper()))
                {
                    
                    //表示一个表结束，开始另一个表
                    #region  此时写入一个属性文件，为了释放内存
                    string jhlx=this.getJHLXByYsdm(tableName);
                    if (jhlx != "")
                    {
                        string newAttFile = sTmpFilePath + "\\" + jhlx.ToUpper() + "_" + tableName + ".ATTR";
                        if (dicGeoBsm.ContainsKey(tableName))
                        {
                            using (StreamWriter sw = new StreamWriter(newAttFile,false,gb2312 )  )
                            {

                                ArrayList arDataBsm = dicGeoBsm[tableName];//图形标识码
                                foreach (int aBsm in arDataBsm)
                                {
                                    if (hasIndex.ContainsKey(aBsm))
                                    {
                                        string val = hasIndex[aBsm].ToString().Trim();
                                        sw.WriteLine(val);
                                    }
                                }
                                sw.Close();
                                hasIndex.Clear();
                                //
                                dicGeoBsm.Remove(tableName);
                            }

                        }
                    }
                    else
                    {
                        //没有集合类型，可能是扩展表
                        if (IsInExtTable)
                        {
                            string newAttFile = sTmpFilePath + "\\EXT_" + tableName + ".ATTR";
                            using (StreamWriter sw = new StreamWriter(newAttFile, false, gb2312))
                            {
                                foreach (string value in hasIndex.Values)
                                {
                                    sw.WriteLine(value);
                                }

                             
                            }

                        }
                    }
                    
                    #endregion 
                   
                    if (sReadLine.Equals("Attributeend".ToUpper()))
                        break;
                    //读取下一个表名
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);
                    tableName=sReadLine;

                }
                else
                {
                    //如果是一行数据，记录下来
                    string[] sArray = sReadLine.Split(sgSeparator.ToCharArray());
                    if (sArray.Length > 1)
                    {
                        int nBSM = Convert.ToInt32(sArray[0]);
                        hasIndex.Add(nBSM, sReadLine);
                    }
                }
            }


        }

    }
}
