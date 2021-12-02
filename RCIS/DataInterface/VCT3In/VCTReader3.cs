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

namespace RCIS.DataInterface.VCT3In
{
    public  class VCTReader3
    {


        class aLineGeoPos
        {
            /// <summary>
            /// 起始位置
            /// </summary>
            public long startPos;
            public long offset;

            public aLineGeoPos(long _start, long _end)
            {
                this.startPos = _start;
                this.offset = _end;

            }
        }

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


        string sTmpFilePath = Application.StartupPath + "\\VCTProcess";

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

           // iMerian = iMerian / 3;
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
        private string getSXBMByYsdm(Int64 nYSDM2)
        {
            string tabName = "";
            foreach (string key in ghasFeatuerCode.Keys)
            {
                FeatureCodeBeginEnd3 curItem = (FeatureCodeBeginEnd3)ghasFeatuerCode[key];
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
                FeatureCodeBeginEnd3 curItem = (FeatureCodeBeginEnd3)ghasFeatuerCode[key];
                if (curItem.sSXBM.ToUpper() == tableName.ToUpper())
                {
                    jhlx = curItem.sJHLX;
                    break;
                }
            }
            return jhlx;
        }

       // private Hashtable NoOnlyPolygon = new Hashtable();
        public VCTReader3()
        { 
            //非独立面
            
           // NoOnlyPolygon.Add("XZQ", "XZQJX");
           // NoOnlyPolygon.Add("ZD", "JZX");
        
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
                string sSXBM = sArray[3].Trim().ToUpper();     //基本属性表名 
                FeatureCodeBeginEnd3 curItem = new FeatureCodeBeginEnd3();
                curItem.sYSDM = sYSDM;
                curItem.sYSMC = sYSMC.Trim().ToUpper();
                curItem.sJHLX = sJHLX.Trim().ToUpper();                
                curItem.sSXBM = sSXBM.Trim().ToUpper();

                if (sJHLX.ToUpper().Trim() != "ANNOTATION")
                {
                    //注记层暂不处理
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
                    if (sReadLine.Equals("TABLESTRUCTUREEND"))
                    {
                        bOK = true;
                        break;
                    }
                    //后面就是结构信息
                    string[] sArray = sReadLine.Split(sgSeparator.ToCharArray());

                    string sTblName = sArray[0].Trim().ToUpper(); //表名
                    int nFlds = Convert.ToInt32(sArray[1]);  //字段个数
                    bool isKzsx = false;
                    if (sArray.Length > 2)
                    {
                        string nogeo = sArray[2].ToUpper().Trim();
                        if (nogeo == "NONEGEOMETRY")
                        {
                            isKzsx = true;
                        }
                    }

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
                            sNeedType = "DATE";
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

                    } 
                    
                    TableStructBeginEnd3 curItem = new TableStructBeginEnd3();
                    curItem.nZDGS = nFlds;
                    curItem.aZDLXs = aZDLXs;
                    curItem.aZDMCs = aZDMCs;
                    curItem.aZDJD = aZDJD;
                    curItem.aZDJD2 = aZDJD2;
                    curItem.isKzsxb = isKzsx; //是否是扩展属性表
                    ghasTableStructure.Add(sTblName, curItem);   //以属性表名为KEY,保存每个表的信息

                    //以0 为终止
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);
                    
                    
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
            int dMeridian = 38;
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
                        if (aLine.ToUpper().StartsWith("Parameters".ToUpper()))
                        {
                            string[] strs = aLine.Split(',');
                            if (strs.Length == 6)
                            {
                                string sMeridian = strs[5];
                                try
                                {
                                    dMeridian = Convert.ToInt32(sMeridian);
                                }
                                catch { }
                                bOK = true;
                                break;
                            }
                        }
                        //if (aLine.Length > 9 && aLine.Substring(0, 8).ToUpper().Equals("MERIDIAN") == true)
                        //{
                        //    string sMeridian = aLine.Substring(9).Trim();
                        //    try
                        //    {
                        //        dMeridian = Convert.ToInt32(sMeridian);
                        //    }
                        //    catch { }
                        //    bOK = true;
                        //    break;
                        //}
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

        //public int GetMeridian(string vctfile)
        //{
        //    int dMeridian = 117;
        //    if ((vctfile.Trim() == "") || (!File.Exists(vctfile)))
        //    {
        //        throw new Exception("VCT文件不存在！");
        //        //return -1;
        //    }
        //    bool bOK = false;
        //    Encoding gb2312 = Encoding.GetEncoding("GB2312");
        //    StreamReader VCTReader = new StreamReader(vctfile, gb2312);
        //    string sReadLine = VCTReader.ReadLine().Trim().ToUpper();
        //    #region 读取headbegin--->HeadEnd之间的信息
        //    while (sReadLine != null)
        //    {
        //        if (sReadLine.Equals("HEADBEGIN"))
        //        {
        //            string aLine = VCTReader.ReadLine().Trim().ToUpper();
        //            while (aLine != null)
        //            {
        //                //Meridian
        //                if (aLine.Length > 9 && aLine.Substring(0, 8).ToUpper().Equals("MERIDIAN") == true)
        //                {
        //                    string sMeridian = aLine.Substring(9).Trim();
        //                    try
        //                    {
        //                        dMeridian = Convert.ToInt32(sMeridian);
        //                    }
        //                    catch { }
        //                    bOK = true;
        //                    break;
        //                }
        //                else
        //                    if (aLine.Equals("HEADEND"))
        //                    {
        //                        bOK = true;
        //                        break;
        //                    }
        //                aLine = VCTReader.ReadLine().Trim().ToUpper();
        //            }
        //        }

        //        if (bOK == true || VCTReader.EndOfStream == true)
        //            break;
        //        sReadLine = VCTReader.ReadLine().Trim().ToUpper();
        //    }
        //    VCTReader.Close();
        //    #endregion
        //    if (bOK == false)
        //    {

        //        throw new Exception("VCT文件格式错误: 没有HeadEnd标识!");
        //        //return -1;
        //    }
        //    return dMeridian;

        //}


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
                long nMBBSM = Convert.ToInt64(sReadLine);  //标志吗
                #region 下面跟着6行
                //要素代码
                //样式
                //点特征类型
                //点数
                //X,Y
                //0
                string[] sNext6Line = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);
                    sNext6Line[i] = sReadLine;
                } 
                int newYsdm = Convert.ToInt32(sNext6Line[0]);
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
                dataWriter.WriteLine(sNext6Line[4]);
                

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
            Int64 oldysdm = 0;
            string sReadLine = "";
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine.Equals("POLYGONBEGIN")) break;
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                
                if (sReadLine.Equals("POLYGONEND"))
                    break;
                long nMBBSM = 0;
                
                try
                {
                    nMBBSM = Convert.ToInt64(sReadLine);  //标志吗
                }
                catch { }
                
                    #region //下面跟着N行

                    //要素代码
                    //样式
                    //特征类型
                    //注记点坐标X,Y
                    //牵涉的线数n[每行8个线BSM]
                    //L1,L2,...,L8
                    //...
                    //...,...,.....Ln
                    //最后一行 0 
                    Int64 newYsdm = 0;
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);//要素代码
                    try
                    {
                        newYsdm = Convert.ToInt64(sReadLine);
                    }
                    catch { }
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);  //样式
                    sReadLine = this.ReadNextNoBlankRow(VCTReader);//特征类型
                    sReadLine = this.ReadNextNoBlankRow(VCTReader); //注记点坐标X,Y
                    sReadLine = this.ReadNextNoBlankRow(VCTReader); //面构成类型
                    sReadLine = this.ReadNextNoBlankRow(VCTReader); //牵涉的线数n[每行8个线BSM]


                    int nLineGS = 0;
                    try
                    {
                        nLineGS=Convert.ToInt32(sReadLine);
                    }
                    catch { }
                  

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

                    sReadLine = this.ReadNextNoBlankRow(VCTReader); // 0 

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

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return dicPolygonTables;

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

                long nMBBSM = Convert.ToInt64(sReadLine);  //标志吗
                #region //下面跟着N行
                //要素代码
                //样式编码
                //线特征类型
                //线段条数
                //线段类型
                //点数n
                //X1,Y1;
                //...
                //Xn,Yn
                //0

                
                int newYsdm = 0;
                sReadLine = ReadNextNoBlankRow(VCTReader);
                newYsdm = Convert.ToInt32(sReadLine); //要素代码
                sReadLine = ReadNextNoBlankRow(VCTReader); //样式编码
                sReadLine = ReadNextNoBlankRow(VCTReader);  //线特征类型
                sReadLine = ReadNextNoBlankRow(VCTReader);
                sReadLine = ReadNextNoBlankRow(VCTReader);
                sReadLine = ReadNextNoBlankRow(VCTReader);  //点数n
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
                //0
                sReadLine = this.ReadNextNoBlankRow(VCTReader);


                string tabName = getSXBMByYsdm(newYsdm);
                string jhlx = getJHLXByYsdm(tabName); 
                if (tabName.Trim() == "")
                {
                    //此处为空，表示为独立面 线对象，其要素代码为1099000000
                    tabName = "1099000000";
                }
                if (oldysdm != newYsdm)
                {
                    if (tabName == "1099000000")
                    {
                        //如果已存在，面向 1099000000 那部分，但是 又重新开始了
                        if (dataWriter != null)
                        {
                            dataWriter.Close();
                        }
                        //创建之  
                        string sDataFile = sTmpFilePath + "\\LINE_" + tabName + ".DATA";
                        dataWriter = new StreamWriter(sDataFile, true, gb2312);  //追加方式
                    }
                    else
                    {
                        //如果不存在，则创建之 
                        if (dataWriter != null)
                        {
                            dataWriter.Close();
                        }
                        //创建之  
                        string sDataFile = sTmpFilePath + "\\LINE_" + tabName + ".DATA";
                        dataWriter = new StreamWriter(sDataFile, false, gb2312);
                    }

                    //如不是独立面 线对象，则 记录 其bsm
                    if (jhlx == "LINE" && !dicLineTables.ContainsKey(tabName))
                    {
                        ArrayList nPBsm = new ArrayList();
                        dicLineTables.Add(tabName, nPBsm);
                    }
                    
                    
                    oldysdm = newYsdm;
                }
                dataWriter.WriteLine(nMBBSM); //输出bsm
                if (jhlx == "LINE")
                {
                    ArrayList tmpList = dicLineTables[tabName];
                    tmpList.Add(nMBBSM);
                    dicLineTables[tabName] = tmpList;
                }
                
                //输出一行
                dataWriter.WriteLine(strPoints);// 输出点坐标到一行
            }
            if (dataWriter != null)
                dataWriter.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return dicLineTables;

        }

        public void getNewAnnotationFile(StreamReader VCTReader)
        {
            Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");
            string sDataFile = sTmpFilePath + "\\ANNOTATION.DATA"; //拆分文件
            StreamWriter dataWriter = new StreamWriter(sDataFile,false, gb2312);
         
            string sReadLine = "";

            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {
                if (sReadLine == "ANNOTATIONBEGIN")
                    break; //还没开始
            }
            while ((sReadLine = this.ReadNextNoBlankRow(VCTReader)) != "")
            {

                if (sReadLine.Equals("ANNOTATIONEND"))
                {
                    break;
                }
                string  nMBBSM = sReadLine;  //标志吗
                #region 下面跟着6行
                string ysdm = this.ReadNextNoBlankRow(VCTReader);
                sReadLine = this.ReadNextNoBlankRow(VCTReader);  //unkown
                sReadLine = this.ReadNextNoBlankRow(VCTReader); //1
                string zjnr = this.ReadNextNoBlankRow(VCTReader); //注记内容
                sReadLine = this.ReadNextNoBlankRow(VCTReader);
                string[] tmpStr = sReadLine.Split(',');
                double x = 0;
                double y = 0;
                double jd = 0;
                if (tmpStr.Length == 3)
                {
                    
                    double.TryParse(tmpStr[0], out x);
                    double.TryParse(tmpStr[1], out y);
                    double.TryParse(tmpStr[2], out jd);

                }

                //写入bsm，ysdm，zjnr，x,y,jd
                dataWriter.WriteLine(nMBBSM + "," + ysdm + "," + zjnr + "," + x + "," + y + "," + jd);
                #endregion

            }
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
                                foreach (long  aBsm in arDataBsm)
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

                        long nBSM = -1;
                        long.TryParse(sArray[0], out nBSM);
                        if (!hasIndex.ContainsKey(nBSM))
                        {
                            hasIndex.Add(nBSM, sReadLine);
                        }
                        
                        
                    }
                }
            }


        }

    }

    

}
