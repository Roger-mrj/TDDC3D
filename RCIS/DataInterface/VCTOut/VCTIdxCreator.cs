using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
namespace RCIS.DataInterface.VCTOut
{
    public class VCTIdxCreator
    {
        #region 基本成员
        private string vctFile = "";

        public string VctFile
        {
            get { return vctFile; }
            set { vctFile = value; }
        }
        
        
        /// <summary>
        /// 所有表
        /// </summary>
        private Dictionary<string, string> allYsdmClassName = new Dictionary<string, string>();
        /// <summary>
        /// 所有类名和要素代码
        /// </summary>
        public Dictionary<string, string> AllYsdmClassName
        {
            get { return allYsdmClassName; }
            set { allYsdmClassName = value; }
        }
        Encoding gb2312 = Encoding.GetEncoding("GB2312");

        long currFilePos = 0;
        // <summary>
        /// 读取下一个非空行
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private string ReadNextNoBlankRow(StreamReader sr)
        {
            if (sr.EndOfStream) return "";
            string s = sr.ReadLine().ToUpper();
            this.currFilePos += s.Length + 2;
            while (s == "")
            {
                s = sr.ReadLine().ToUpper();
                this.currFilePos += s.Length + 2;
            }
            return s.Trim();
        }

        private string getDestFileName(string tabname)
        {
            string dir = System.IO.Path.GetDirectoryName(this.vctFile);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(this.vctFile);
            string newDir = dir + "\\" + fileName + "_idx";
            //如果不存在，则创建
            if (!System.IO.Directory.Exists(newDir))
            {
                System.IO.Directory.CreateDirectory(newDir);
            }
            return newDir + "\\" + tabname + ".idx";
        }
        #endregion

        private Hashtable htAttrs = new Hashtable(); //记录所有属性bsm，及位置

        private Hashtable htLineEnv = new Hashtable(); //记录线范围和bsm

        private lineEnv calEnv(List<lineEnv> oldEnv)
        {
            lineEnv newEnv = new lineEnv();
            newEnv.xmin = 999999999;
            newEnv.ymin = 999999999;
            newEnv.xmax = 0;
            newEnv.xmax = 0;
            foreach (lineEnv aOldEnv in oldEnv)
            {
                if (aOldEnv.xmax > newEnv.xmax)
                    newEnv.xmax = aOldEnv.xmax;
                if (aOldEnv.ymax > newEnv.ymax)
                    newEnv.ymax = aOldEnv.ymax;
                if (aOldEnv.xmin < newEnv.xmin)
                    newEnv.xmin = aOldEnv.xmin;
                if (aOldEnv.ymin < newEnv.ymin)
                    newEnv.ymin = aOldEnv.ymin;
            }
            return newEnv;
        }
        /// <summary>
        /// 记录 范围
        /// </summary>
        struct lineEnv
        {
            public double xmin;
            public double xmax;
            public double ymin;
            public double ymax;
        }

        public void GetAllAttrsPos()
        {
            htAttrs.Clear();
            Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");
            string sReadLine = "";
            this.currFilePos = 0;
            using (StreamReader sr = new StreamReader(vctFile, gb2312))
            {
                //下面是属性
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {
                    if (sReadLine.ToUpper().Equals("ATTRIBUTEBEGIN"))
                    {
                        break;
                    }
                }
                sReadLine = this.ReadNextNoBlankRow(sr);
                //下面一行 可能是 attr end ,可能是  另一个表开头
                if (!sReadLine.ToUpper().Equals("Attributeend".ToUpper()))
                {
                    //不是结束
                    string tableName = sReadLine; //表名
                    while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                    {
                        //一行数据
                        long sxpos = this.currFilePos;
                        //下一行有可能是数据，也有可能结束
                        if (sReadLine.ToUpper().Equals("TableEnd".ToUpper()))
                        {
                            if (sReadLine.ToUpper().Equals("Attributeend".ToUpper()))
                                break;
                            //读取下一个表名
                            sReadLine = this.ReadNextNoBlankRow(sr);
                            tableName = sReadLine;
                        }
                        else
                        {

                            string[] sArray = sReadLine.Split(',');
                            if (sArray.Length > 1)
                            {
                                try
                                {
                                    string bsm = sArray[0];
                                    htAttrs.Add(bsm, sxpos);//添加到属性位置索引 哈希表
                                }
                                catch(Exception ex)
                                { 
                                }
                            }
                        }
                    }
                }     
                sr.Close();
            }
        }


        private void GetLineEnvs()
        {
            htLineEnv.Clear();
            string[] sfiles = new string[] {"TopoArcs", "CJDCQJX", "XZQJX" };
            foreach (string aTabname in sfiles)
            {
                string theIdxFile = getDestFileName(aTabname);
                if (!System.IO.File.Exists(theIdxFile))
                {
                    continue;
                }
                //读取并生成哈希表
                
                using (StreamReader sr = new StreamReader(theIdxFile, gb2312))
                {
                    string aLinestr = "";
                    aLinestr = sr.ReadLine();
                    aLinestr = sr.ReadLine();
                    aLinestr = sr.ReadLine();
                    //第四行开始
                    while (sr.EndOfStream == false)
                    {
                        aLinestr = sr.ReadLine();
                        if (aLinestr.Trim().EndsWith("IndexEnd"))
                            break;
                        string[] strs = aLinestr.Split(',');
                        string bsm = strs[0];
                        lineEnv env = new lineEnv();
                        env.xmin =double.Parse( strs[1]);
                        env.ymin = double.Parse(strs[2]);
                        env.xmax =double.Parse( strs[3]);
                        env.ymax = double.Parse(strs[4]);
                        if (!htLineEnv.ContainsKey(bsm))
                        {
                            htLineEnv.Add(bsm, env);
                        }
                    }
                    sr.Close();
                }
            }
        }

        public void WritePointIdx()
        {
            string sReadLine = "";
            StreamWriter dataWriter = null;
            //写点文件索引
            using (StreamReader sr = new StreamReader(vctFile, gb2312))
            {
                #region 获取所点对象
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {
                    if (sReadLine == "POINTBEGIN")
                        break; //还没开始
                }
                string oldYsdm = "0"; //
                
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {
                    //读取所有点图形
                    if (sReadLine.Equals("POINTEND"))
                    {
                        break;
                    }
                    string bsm = sReadLine;
                    #region  //下面6行
                    //要素代码
                    //样式
                    //点特征类型
                    //点数
                    //X,Y
                    //0

                    //下一行就是bsm
                    
                    string newYsdm = this.ReadNextNoBlankRow(sr); //ysdm
                    string tabname = newYsdm;
                    if (this.allYsdmClassName.ContainsKey(newYsdm))
                    {
                        tabname = this.allYsdmClassName[newYsdm];  //表名
                    }
                        
                    if (newYsdm != oldYsdm)
                    {
                        if (dataWriter != null)
                        {
                            dataWriter.WriteLine("IndexEnd");
                            dataWriter.Close();
                        }
                        string destFile = getDestFileName(tabname);
                        dataWriter = new StreamWriter(destFile, false, gb2312);
                        dataWriter.WriteLine("DataMark:LANDUSE-IDX");
                        dataWriter.WriteLine("3.0");
                        dataWriter.WriteLine("IndexBegin");
                        oldYsdm = newYsdm;
                    }

                    sReadLine = this.ReadNextNoBlankRow(sr);
                    sReadLine = this.ReadNextNoBlankRow(sr);
                    sReadLine = this.ReadNextNoBlankRow(sr);
                    //此处即使坐标位置
                    long txwz = this.currFilePos;

                    #endregion 
                    

                    sReadLine = this.ReadNextNoBlankRow(sr);//

                    AObjIndex aIndex = new AObjIndex();
                    aIndex.bsm = bsm;                    
                    aIndex.txPos = txwz;
                    string[] splitStr = sReadLine.Split(',');
                    if (splitStr.Length > 1)
                    {
                        try
                        {
                            aIndex.lstAllPts.Add(new AIndexPoint(double.Parse(splitStr[0]), double.Parse(splitStr[1])));
                        }
                        catch { }
                    }
                    aIndex.calEnvelop();
                    aIndex.sxPos= this.htAttrs.ContainsKey(aIndex.bsm)?long.Parse( this.htAttrs[aIndex.bsm].ToString()) :0 ;

                    sReadLine = this.ReadNextNoBlankRow(sr); // 0

                    //写入该要素
                    dataWriter.WriteLine(aIndex.bsm + "," + aIndex.minX + "," + aIndex.minY + "," + aIndex.maxX + "," + aIndex.maxY + "," + aIndex.txPos + "," + aIndex.sxPos);

                    this.htAttrs.Remove(aIndex.bsm); //为了节省内存，删去该bsm
                }

                

                #endregion

                sr.Close();
                if (dataWriter != null)
                {
                    dataWriter.WriteLine("IndexEnd");
                    dataWriter.Close();
                }
            }
        }


        
        /// <summary>
        /// 生成拓扑线索引文件
        /// </summary>
        public void WriteTopLineIdx()
        {
            string sReadLine = "";
            StreamWriter dataWriter = null;
            string tabName = "TopoArcs";
            string destFile = getDestFileName(tabName);
            dataWriter = new StreamWriter(destFile, false, gb2312);
            dataWriter.WriteLine("DataMark:LANDUSE-IDX");
            dataWriter.WriteLine("3.0");
            dataWriter.WriteLine("IndexBegin");

           
            //文件索引
            using (StreamReader sr = new StreamReader(vctFile, gb2312))
            {
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {
                    if (sReadLine.Equals("LINEBEGIN")) break;
                }
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {

                    if (sReadLine.Equals("LINEEND"))  //结尾跳出
                        break;
                    string bsm = sReadLine.Trim();
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
                    string newYsdm = ReadNextNoBlankRow(sr).Trim(); //要素代码                                     
                    
                    sReadLine = ReadNextNoBlankRow(sr); //样式编码
                    sReadLine = ReadNextNoBlankRow(sr);  //线特征类型
                    sReadLine = ReadNextNoBlankRow(sr);
                    sReadLine = ReadNextNoBlankRow(sr);
                    sReadLine = ReadNextNoBlankRow(sr);  //点数n                    
                    long txPos = this.currFilePos;  //记录图形位置

                    int nPointGS = 0;
                    nPointGS = Convert.ToInt32(sReadLine);
                    string strPoints = "";

                    for (int i = 0; i < nPointGS; i++)
                    {
                        sReadLine = ReadNextNoBlankRow(sr);  //1个点
                        strPoints += (sReadLine) + ";";
                    }
                    //0
                    sReadLine = this.ReadNextNoBlankRow(sr);

                    if (strPoints.EndsWith(";"))
                        strPoints = strPoints.Remove(strPoints.Length - 1, 1);

                    #region //只写入拓扑线
                    if (newYsdm == "1099000000")
                    {
                        AObjIndex aindex = new AObjIndex();
                        aindex.bsm = bsm;
                       
                        aindex.txPos = txPos;
                        aindex.sxPos = 0;
                        try
                        {
                            string[] sIdxPts = strPoints.Split(';');
                            foreach (string aIdxPt in sIdxPts)
                            {
                                string[] xys = aIdxPt.Split(',');
                                if (xys.Length > 1)
                                {
                                    aindex.lstAllPts.Add(new AIndexPoint(double.Parse(xys[0]), double.Parse(xys[1])));
                                }
                            }
                        }
                        catch { }
                        aindex.calEnvelop();
                        dataWriter.WriteLine(aindex.bsm + "," + aindex.minX + "," + aindex.minY + "," + aindex.maxX + "," + aindex.maxY + "," + aindex.txPos + "," + aindex.sxPos);

                    }
                    #endregion 

                }

                sr.Close();

                dataWriter.WriteLine("IndexEnd");
                dataWriter.Close();
            }
        }

        /// <summary>
        /// 写入实际线索引文件
        /// </summary>
        public void WriteRealLineIdx()
        {
            string sReadLine = "";
            StreamWriter dataWriter = null;
            string oldYsdm="";
            //文件索引
            using (StreamReader sr = new StreamReader(vctFile, gb2312))
            {
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {
                    if (sReadLine.Equals("LINEBEGIN")) break;
                }
                bool bEnd = false;
                while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                {

                    if (sReadLine.Equals("LINEEND"))  //结尾跳出
                        break;
                    string bsm = sReadLine.Trim();
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
                    string newYsdm = ReadNextNoBlankRow(sr).Trim(); //要素代码  
                    int nPointGS = 0;             
                    while (newYsdm == "1099000000")
                    {
                        #region //跳出该循环，不断读取知道 到 一个 0,一直读取到下一个要素代码
                        sReadLine = ReadNextNoBlankRow(sr); //样式编码
                        sReadLine = ReadNextNoBlankRow(sr);  //线特征类型
                        sReadLine = ReadNextNoBlankRow(sr);
                        sReadLine = ReadNextNoBlankRow(sr);
                        sReadLine = ReadNextNoBlankRow(sr);  //点数n                   
                        
                        nPointGS = Convert.ToInt32(sReadLine);
                        for (int i = 0; i < nPointGS; i++)
                        {
                            sReadLine = ReadNextNoBlankRow(sr);  //1个点
                        }
                        //0
                        sReadLine = this.ReadNextNoBlankRow(sr);

                        //继续next 
                        sReadLine = this.ReadNextNoBlankRow(sr);
                        if (sReadLine.Trim() == "LINEEND")
                        {
                            bEnd = true;
                            break;
                        }
                        bsm = sReadLine.Trim();
                        newYsdm = ReadNextNoBlankRow(sr).Trim(); //要素代码  
                        #endregion 

                    }

                    if (bEnd)
                    {
                        break;
                    }
                    string tabName = newYsdm;
                    if (this.allYsdmClassName.ContainsKey(newYsdm))
                    {
                        tabName = this.allYsdmClassName[newYsdm];
                    }



                    if (newYsdm != oldYsdm)
                    {
                        if (dataWriter != null)
                        {
                            dataWriter.WriteLine("IndexEnd");
                            dataWriter.Close();
                        }
                        string destFile = getDestFileName(tabName);
                        dataWriter = new StreamWriter(destFile, false, gb2312);
                        dataWriter.WriteLine("DataMark:LANDUSE-IDX");
                        dataWriter.WriteLine("3.0");
                        dataWriter.WriteLine("IndexBegin");
                        oldYsdm = newYsdm;
                    }

                    sReadLine = ReadNextNoBlankRow(sr); //样式编码
                    sReadLine = ReadNextNoBlankRow(sr);  //线特征类型
                    sReadLine = ReadNextNoBlankRow(sr);
                    sReadLine = ReadNextNoBlankRow(sr);
                    sReadLine = ReadNextNoBlankRow(sr);  //点数n                    
                    long txPos = this.currFilePos;  //记录图形位置

                    nPointGS = 0;
                    nPointGS = Convert.ToInt32(sReadLine);
                    string strPoints = "";
                    for (int i = 0; i < nPointGS; i++)
                    {
                        sReadLine = ReadNextNoBlankRow(sr);  //1个点
                        strPoints += (sReadLine) + ";";
                    }
                    //0
                    sReadLine = this.ReadNextNoBlankRow(sr);

                    if (strPoints.EndsWith(";"))
                        strPoints = strPoints.Remove(strPoints.Length - 1, 1);

                    AObjIndex aindex = new AObjIndex();
                    aindex.bsm = bsm;
                    aindex.txPos = txPos;
                    aindex.sxPos = htAttrs.ContainsKey(aindex.bsm) ? long.Parse(htAttrs[bsm].ToString()) : 0;
                    if (htAttrs.ContainsKey(aindex.bsm))
                    {
                        htAttrs.Remove(aindex.bsm);
                    }
                    try
                    {
                        string[] sIdxPts = strPoints.Split(';');
                        foreach (string aIdxPt in sIdxPts)
                        {
                            string[] xys = aIdxPt.Split(',');
                            if (xys.Length > 1)
                            {
                                aindex.lstAllPts.Add(new AIndexPoint(double.Parse(xys[0]), double.Parse(xys[1])));
                            }
                        }
                    }
                    catch { }
                    aindex.calEnvelop();
                    dataWriter.WriteLine(aindex.bsm + "," + aindex.minX + "," + aindex.minY + "," + aindex.maxX + "," + aindex.maxY + "," + aindex.txPos + "," + aindex.sxPos);
                    
                    

                }

                sr.Close();
                if (dataWriter != null)
                {
                    dataWriter.WriteLine("IndexEnd");
                    dataWriter.Close();
                }
            }
        }


        public void WritePolygonIdx()
        {
            
            this.GetLineEnvs();
            string sReadLine = "";
            StreamWriter dataWriter = null;
            string oldYsdm="";
            //文件索引
            try
            {
                using (StreamReader sr = new StreamReader(vctFile, gb2312))
                {
                    while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                    {
                        if (sReadLine.Equals("POLYGONBEGIN")) break;
                    }
                    while ((sReadLine = this.ReadNextNoBlankRow(sr)) != "")
                    {

                        if (sReadLine.Equals("POLYGONEND"))
                            break;

                        string bsm = sReadLine.Trim();
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

                        sReadLine = this.ReadNextNoBlankRow(sr);//要素代码
                        string newYsdm = sReadLine.Trim();
                        string tabName = "没找到";
                        if (allYsdmClassName.ContainsKey(newYsdm))
                        {
                            tabName = this.allYsdmClassName[newYsdm];
                        }
                        
                        if (oldYsdm != newYsdm)
                        {
                            string datafile = this.getDestFileName(tabName);
                            if (dataWriter != null)
                            {
                                dataWriter.WriteLine("IndexEnd");
                                dataWriter.Close();
                            }
                            dataWriter = new StreamWriter(datafile, false, gb2312);
                            dataWriter.WriteLine("DataMark:LANDUSE-IDX");
                            dataWriter.WriteLine("3.0");
                            dataWriter.WriteLine("IndexBegin");
                            oldYsdm = newYsdm;
                        }


                        sReadLine = this.ReadNextNoBlankRow(sr);  //样式
                        sReadLine = this.ReadNextNoBlankRow(sr);//特征类型
                        sReadLine = this.ReadNextNoBlankRow(sr); //注记点坐标X,Y
                        sReadLine = this.ReadNextNoBlankRow(sr); //面构成类型
                        sReadLine = this.ReadNextNoBlankRow(sr); //牵涉的线数n[每行8个线BSM]
                        long txPos = this.currFilePos;

                        int nLineGS = Convert.ToInt32(sReadLine);
                        int nNeedLines = nLineGS / 8;
                        if (nLineGS % 8 != 0)
                            nNeedLines++;
                        string alllines = "";
                        for (int i = 0; i < nNeedLines; i++)
                        {
                            sReadLine = this.ReadNextNoBlankRow(sr);
                            alllines += sReadLine.Trim() + ",";
                        }
                        if (alllines.EndsWith(","))
                            alllines = alllines.Remove(alllines.Length - 1);

                        sReadLine = this.ReadNextNoBlankRow(sr); //0

                        AObjIndex aIndex = new AObjIndex();
                        aIndex.bsm = bsm;
                        aIndex.sxPos = htAttrs.ContainsKey(aIndex.bsm) ? long.Parse(htAttrs[aIndex.bsm].ToString()) : 0;
                        htAttrs.Remove(aIndex.bsm);
                        aIndex.txPos = txPos;
                        //计算范围了
                        string[] arrLines = alllines.Split(',');
                        List<lineEnv> lst = new List<lineEnv>();
                        foreach (string aLineBsm in arrLines)
                        {
                            if (htLineEnv.ContainsKey(aLineBsm))
                            {
                                lst.Add((lineEnv)(htLineEnv[aLineBsm]));
                            }
                        }
                        lineEnv newEnv = this.calEnv(lst);
                        aIndex.minX = newEnv.xmin;
                        aIndex.minY = newEnv.ymin;
                        aIndex.maxX = newEnv.xmax;
                        aIndex.maxY = newEnv.ymax;
                        //输出
                        dataWriter.WriteLine(aIndex.bsm + "," + aIndex.minX + "," + aIndex.minY + "," + aIndex.maxX + "," + aIndex.maxY + "," + aIndex.txPos + "," + aIndex.sxPos);


                        #endregion
                    }
                    sr.Close();
                    if (dataWriter != null)
                    {
                        dataWriter.WriteLine("IndexEnd");
                        dataWriter.Close();
                    }
                    this.htAttrs.Clear();
                    this.htLineEnv.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
