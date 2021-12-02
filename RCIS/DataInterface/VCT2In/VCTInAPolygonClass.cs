using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Collections;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace RCIS.DataInterface.VCT2
{
    public class VCTInAPolygonClass
    {
        private string SgSeparator = ",";
        private Hashtable NoOnlyPolygon = new Hashtable();

        Hashtable ghasTableStructure = null;
        string aTabName = "";

        IFeatureDataset pDestFeaDS = null;

        public long bufferSize = 4096;

        public double geoOpTick, fileTick,writeatttick;

       // public Hashtable needLineHas;

        private class APointXY
        {
            public int flag = 0; // 0 结束，1 正向，-1 负向，如果是0 ，后面无坐标
            public double  X ;
            public double  Y ;
        }

        public VCTInAPolygonClass(string _tabName, Hashtable _ht, IFeatureDataset _ds)
        {
            this.aTabName = _tabName;
            this.ghasTableStructure = _ht;
            this.pDestFeaDS = _ds;

            //非独立面
            NoOnlyPolygon.Add("DLTB", "DLJX");
            NoOnlyPolygon.Add("XZQ", "XZQJX");
            NoOnlyPolygon.Add("ZD", "JZX");

        }

        //private Hashtable getNeedLineXY(string filepath)
        //{
        //    Hashtable htLine = new Hashtable(); //此处只记录文件位置 
        //    Encoding gb2312 = Encoding.GetEncoding("GB2312");
        //    using (StreamReader reader = new StreamReader(filepath, gb2312))
        //    {
        //        string aLine = "";
        //        long pos = 0;
        //        while (!reader.EndOfStream)
        //        {
        //            aLine = reader.ReadLine();
        //            pos += aLine.Length + 2;
        //            Int32 bsm = Convert.ToInt32(aLine);
        //            aLine = reader.ReadLine();

        //            long endPos = pos + aLine.Length;
        //            htLine.Add(bsm, new aLinePosClass(pos,endPos) );

        //            //下一行的位置要修正

        //            pos += aLine.Length + 2;


        //        }
        //        reader.Close();
        //    }

        //    return htLine;
        //}

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
                    Int32 bsm = Convert.ToInt32(aLine);
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
        private string getALineByPos(string linedatafile, long linePos,ref Hashtable htMemoLine)
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
            fileTick +=ts.TotalMilliseconds;//计算时间


            return line;
        }
      
        ////获取线 坐标，从线文件中，根据位置
        //private string getALineByPos(string linedatafile, long linePos)
        //{
        //    string line = "";

        //    System.IO.FileStream lineFs = new System.IO.FileStream(linedatafile, FileMode.Open);
        //    StreamReader lineSr = new StreamReader(lineFs, System.Text.Encoding.GetEncoding("GB2312"),true);
        //    lineFs.Seek(linePos, SeekOrigin.Begin);
        //    line = lineSr.ReadLine().Trim().ToUpper();
        //    if (line.Trim() == "")
        //    {
        //        line = lineSr.ReadLine().Trim().ToUpper();
        //    }
        //    lineSr.Close();
        //    lineFs.Close();

        //    return line;
        //}


        /// <summary>
        /// 根据坐标 生成面
        /// </summary>
        /// <param name="arPoints"></param>
        /// <returns></returns>
        private IGeometry getAGeoByPts(ArrayList arPoints)
        {
            PolygonClass newPolygon = new PolygonClass();
            RingClass aPart = new RingClass();
            IPointCollection pCol = aPart as IPointCollection;
            for (int i = 0; i < arPoints.Count; i++)
            {
                APointXY aPt =(APointXY) arPoints[i];
                if (aPt.flag == 0)
                {
                    #region //这部分闭合
                    aPart.Close();
                    IGeometry aGeom = aPart as IGeometry;
                    (newPolygon as IGeometryCollection).AddGeometries(1, ref aGeom);
                    aPart = new RingClass();
                    pCol = aPart as IPointCollection;
                    #endregion 
                }
                else 
                {
                    IPoint newPt = new PointClass();
                    newPt.PutCoords(aPt.X, aPt.Y);
                    object oo = Type.Missing;
                    pCol.AddPoint(newPt, ref oo, ref oo);
                }
              
                   
            }
            if (aPart != null && !aPart.IsEmpty)
            {
                aPart.Close();
                IGeometry aGeom = aPart as IGeometry;
                (newPolygon as IGeometryCollection).AddGeometries(1, ref aGeom);
            }
            newPolygon.Close();
            return newPolygon;

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
                //aFileFinish(this.aTabName + "没有数据！");
                //allFileFinish();
                return;
            }

            //首先读取对应线层的 标识码和坐标
            Hashtable needLineHas = this.getNeedLineXY2(linedatafile);
          //  Hashtable needLineHas = this.getNeedLineXY(linedatafile);  

            int nSaveGS = 0; //记录面数量，隔一段时间保存
            int allCount = 0;

            //IWorkspace pWS = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(sTmpFilePath + "\\" + aTabName);
            IWorkspace pWS = this.pDestFeaDS.Workspace;

            IFeatureClass pFeatureClass = (pWS as IFeatureWorkspace).OpenFeatureClass(aTabName);
            // 读一个点坐标，读一个属性
            IWorkspaceEdit pWSE = pWS as IWorkspaceEdit;
            IFeatureClassLoad pFCLoad = pFeatureClass as IFeatureClassLoad;
            if (pFCLoad != null)
                pFCLoad.LoadOnlyMode = true;

            pWSE.StartEditing(true);
            pWSE.StartEditOperation();
            IFeatureCursor pFCur = pFeatureClass.Insert(true);



            #region 表结构信息，
            IFields flds = pFeatureClass.Fields;
            int[] nFldIndex = new int[flds.FieldCount];
            int[] nFldType = new int[flds.FieldCount];     //如果字段类型为数字、设置为1
            int[] nFldLength = new int[flds.FieldCount];    //仅针对char类型的字段长度

            int nAttrFldGS = curItem.aZDMCs.Count;
            for (int i = 0; i < nAttrFldGS; i++)
            {
                string sName = (string)curItem.aZDMCs[i];
                int nPos = flds.FindField(sName);
                nFldIndex[i] = nPos;

                string sType = (string)curItem.aZDLXs[i];
                if (sType.Equals("INT"))
                    nFldType[i] = 1;
                else if (sType.Equals("DOUBLE"))
                    nFldType[i] = 2;
                else if (sType.Equals("CHAR"))
                    nFldType[i] = 3;
                else
                    nFldType[i] = 4;

                if (sType.Equals("CHAR"))
                {
                    int nLen = (int)curItem.aZDJD[i];
                    nFldLength[i] = nLen;
                }
                else
                {
                    nFldLength[i] = -1;
                }
            }
            #endregion



            StreamReader dataReader = new StreamReader(sDataFile, Encoding.GetEncoding("GB2312"));
            StreamReader attrReader = new StreamReader(sAttrFile, Encoding.GetEncoding("GB2312"),true);
            Hashtable htLineBuffer = new Hashtable(); //线缓存
            try
            {
                while (dataReader.EndOfStream == false)
                {

                    

                   
                    string aLine = dataReader.ReadLine();
                    int nMBBSM = Convert.ToInt32(aLine);
                    aLine = dataReader.ReadLine();  //所有线标识码
                    string[] aArray = aLine.Split(this.SgSeparator.ToCharArray());
                    ArrayList arrNO = new ArrayList();  //带符号的当前 线号
                    for (int j = 0; j < aArray.Length; j++)
                    {
                        int nBSM = Convert.ToInt32(aArray[j]);
                        arrNO.Add(nBSM);
                    }
                    #region   //读取所有线节点坐标
                    ArrayList arPtXYObj = new ArrayList();
                    for (int i = 0; i < arrNO.Count; i++)
                    {
                        int nBSM=(int)arrNO[i]; //线的标识码;
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

                  

                    DateTime beforDT = DateTime.Now;
                    IGeometry newPolygon = this.getAGeoByPts(arPtXYObj);

                    if (newPolygon == null) continue;
                    if (newPolygon.IsEmpty) continue;
                    DateTime afterDT = System.DateTime.Now;
                    TimeSpan ts = afterDT.Subtract(beforDT);
                    geoOpTick += ts.TotalMilliseconds;//计算时间
                    //把newPolygon加到对应的SHAPEFILE内:
                    //读出对应的属性行:
                    IFeatureBuffer featureBuffer = pFeatureClass.CreateFeatureBuffer();
                    featureBuffer.Shape = (IGeometry)newPolygon;

                    featureBuffer.set_Value(nFldIndex[0], (object)nMBBSM);

                    string sAttLine = attrReader.ReadLine();
                    string[] aAttrArray = sAttLine.Split(SgSeparator.ToCharArray());
                    #region 赋值属性，
                    if (sAttLine != null)
                    {



                        DateTime beforDT1 = DateTime.Now;

                        //第一个字段为BSM or MBBSM
                        if (aAttrArray[0].Equals("无") == false && aAttrArray.Length == nAttrFldGS)
                        {

                            for (int i = 0; i < nAttrFldGS; i++)
                            {
                                int nPos = nFldIndex[i];
                                int nType = nFldType[i];
                                int nLen = nFldLength[i];
                                if (nPos != -1)
                                {
                                    object oo = aAttrArray[i];
                                    string ss = oo.ToString().Trim();
                                    int nValue = 0;
                                    double dValue = 0.0;
                                    string sValue = "";
                                    try
                                    {
                                        if (nType == 1)
                                            nValue = Convert.ToInt32(ss);
                                        else if (nType == 2)
                                            dValue = Convert.ToDouble(ss);
                                        else if (nType == 3)
                                        {
                                            if (ss.Length <= nLen)
                                                sValue = ss;
                                            else
                                                sValue = ss.Substring(0, nLen);
                                        }
                                    }
                                    catch (Exception InsideE)
                                    {
                                        //数据转换可能发生错误:
                                        ;
                                    }

                                    object oo2 = oo;
                                    if (nType == 1)
                                        oo2 = nValue;
                                    else if (nType == 2)
                                        oo2 = dValue;
                                    else if (nType == 3)
                                        oo2 = sValue;
                                    else
                                        oo2 = oo;
                                    featureBuffer.set_Value(nPos, (object)oo2);
                                }
                            }
                           
                        }

                        DateTime afterDT1 = System.DateTime.Now;
                        TimeSpan ts1 = afterDT.Subtract(beforDT1);
                        writeatttick += ts1.TotalMilliseconds;//计算时间
                         
                    }
                #endregion
                    
                    pFCur.InsertFeature(featureBuffer);
                    nSaveGS++;
                    allCount++;
                    #region  //1000条保存一次
                    if (nSaveGS == 20000)
                    {
                        nSaveGS = 0;
                        pFCur.Flush();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFCur);
                        pWSE.StopEditOperation();
                        pWSE.StopEditing(true);
                        nSaveGS = 0;

                        pWSE.StartEditing(true);
                        pWSE.StartEditOperation();
                        pFCur = pFeatureClass.Insert(true);
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                #region ex
                if (pWSE.IsBeingEdited())
                {
                    pWSE.AbortEditOperation();
                    pWSE.StopEditing(true);
                }
                if (pFCLoad != null)
                    pFCLoad.LoadOnlyMode = false;

               // aFileFinish(this.aTabName + ex.ToString());
                #endregion 


            }
            pFCur.Flush();
           
            pWSE.StopEditOperation();
            pWSE.StopEditing(true);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFCur);

            needLineHas.Clear();
            dataReader.Close();
            attrReader.Close();

            

            if (pFCLoad != null)
                pFCLoad.LoadOnlyMode = false;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureClass);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pWS);

            //aFileFinish(this.aTabName + "导入完毕,共"+allCount+"条！");
            //allFileFinish();
        }


        //public delegate void InvokeAFile(string txt);
        //public InvokeAFile aFileFinish;  //倒入完一个文件
        //public delegate void InvokeFinish();
        //public InvokeFinish allFileFinish; //完成
    }
}
