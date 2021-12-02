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
namespace RCIS.DataInterface.VCT2
{
    public class VCTInAPointClass
    {
        private string sgSeparator = ",";

        Hashtable ghasTableStructure = null;
        string aTabName = "";

        IFeatureDataset pDestFeaDS = null;
        public VCTInAPointClass(string _tabName,Hashtable _htTableStruct,IFeatureDataset _ds)
        {
            this.aTabName = _tabName;
            this.ghasTableStructure = _htTableStruct;
            this.pDestFeaDS = _ds;
        }
        
        public void WriteAPtClass()
        {
            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;
            
            string aPtDataFile = sTmpFilePath + "\\POINT_" + aTabName.ToUpper() + ".DATA";
            string aPtAttFile = sTmpFilePath + "\\POINT_" + aTabName.ToUpper() + ".ATTR";
            if (!System.IO.File.Exists(aPtAttFile) || !System.IO.File.Exists(aPtDataFile))
            {
                //找不到该文件。掠过
                //aFileFinish(this.aTabName + "没有数据！");
                //allFileFinish();
                return;
            }

            TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[aTabName];

            // IWorkspace pWS = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(sTmpFilePath + "\\" + aTabName);
            IWorkspace pWS = this.pDestFeaDS.Workspace;
            IFeatureClass pFeatureClass = (pWS as IFeatureWorkspace).OpenFeatureClass(aTabName);
            IWorkspaceEdit pWSE = pWS as IWorkspaceEdit;
            
            IFeatureClassLoad pFCLoad = pFeatureClass as IFeatureClassLoad;
            if (pFCLoad != null)
                pFCLoad.LoadOnlyMode = true;

            pWSE.StartEditing(true);
            pWSE.StartEditOperation();
            IFeatureCursor pFCur = pFeatureClass.Insert(true);


            #region 表结构信息
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

            int ngMBBSM = 0;  //BSM


            StreamReader dataReader = new StreamReader(aPtDataFile, System.Text.Encoding.GetEncoding("GB2312"));
            StreamReader attrReader = new StreamReader(aPtAttFile, System.Text.Encoding.GetEncoding("GB2312"),true);
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
                    string[] sZJP = sDataLine.Split(sgSeparator.ToCharArray());
                    PointClass newP = new PointClass();
                    newP.PutCoords(Convert.ToDouble(sZJP[0]), Convert.ToDouble(sZJP[1]));


                    

                    //把newP加到对应的SHAPEFILE内:
                    IFeatureBuffer Buffer = pFeatureClass.CreateFeatureBuffer();
                    Buffer.Shape = (IGeometry)newP;
                    Buffer.set_Value(nFldIndex[0], (object)nMBBSM);        //第一个字段为BSM or MBBSM

                    //上面读完了一个点的数据、下面马上读对应的一行属性数据:
                    string sAttLine = attrReader.ReadLine();
                    if (sAttLine != null)
                    {
                        string[] aAttrArray = sAttLine.Trim().Split(sgSeparator.ToCharArray());

                        if (aAttrArray[0].Equals("无") == false && aAttrArray.Length == nAttrFldGS)
                        {
                            //表示没有对应的属性:
                            for (int i = 0; i < nAttrFldGS; i++)
                            {

                                #region

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
                                    Buffer.set_Value(nPos, (object)oo2);
                                }
                                #endregion

                            }
                        }
                    }
                    pFCur.InsertFeature(Buffer);
                    pFCur.Flush();

                    #endregion

                } //while

                dataReader.Close();
                attrReader.Close();
                if (pWSE.IsBeingEdited() == true)
                {
                    pWSE.StopEditOperation();
                    pWSE.StopEditing(true);
                }
                if (pFCur != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFCur);
                }
                if (pFCLoad != null)
                    pFCLoad.LoadOnlyMode = false;

            }
            catch (Exception E)
            {

                if (pWSE.IsBeingEdited())
                {
                    pWSE.AbortEditOperation();
                    pWSE.StopEditing(true);
                }
                if (pFCLoad != null)
                    pFCLoad.LoadOnlyMode = false;

                //aFileFinish(this.aTabName + E.ToString());
            }

           System.Runtime.InteropServices.Marshal.ReleaseComObject(pWS);

            //aFileFinish(this.aTabName + "导入完毕！");
            //allFileFinish();

        }

        //public delegate void InvokeAFile(string txt);
        //public InvokeAFile aFileFinish;  //倒入完一个文件
        //public delegate void InvokeFinish();
        //public InvokeFinish allFileFinish; //完成

    }
}
