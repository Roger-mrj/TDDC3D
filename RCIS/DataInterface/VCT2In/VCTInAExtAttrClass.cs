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
    /// <summary>
    /// 读入扩展属性表
    /// </summary>
    public class VCTInAExtAttrClass
    {
        string sgSeparator=",";
        string aTabName = "";
        IWorkspace pWS = null;
        Hashtable ghasTableStructure = null;
        public VCTInAExtAttrClass(string _tabName, Hashtable _htTableStruct,IWorkspace _WS)
        {
            this.aTabName = _tabName;
            this.pWS = _WS;
            this.ghasTableStructure = _htTableStruct;
        }

        public void WriteExtTable()
        {
            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;

            string aPtAttFile = sTmpFilePath + "\\EXT_" + aTabName.ToUpper() + ".ATTR";
            IFeatureWorkspace pFWS=this.pWS as IFeatureWorkspace;
            ITable pTable = pFWS.OpenTable(aTabName);
            ICursor pCur = pTable.Insert(true);
            IWorkspaceEdit pWSE = pWS as IWorkspaceEdit;
            pWSE.StartEditing(true);
            pWSE.StartEditOperation();
            
            TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[aTabName];
            if (curItem==null) return;
            IFields flds = pTable.Fields;
            int[] nFldIndex = new int[flds.FieldCount];
            int[] nFldType = new int[flds.FieldCount];     //如果字段类型为数字、设置为1
            int[] nFldLength = new int[flds.FieldCount];    //仅针对char类型的字段长度
            int nAttrFldGS = curItem.aZDMCs.Count;
            #region 结构信息
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

            int nRecordGS = 0;
            StreamReader attrReader = new StreamReader(aPtAttFile, System.Text.Encoding.GetEncoding("GB2312"));
            while (attrReader.EndOfStream == false)
            {
                    string sAttLine = attrReader.ReadLine().Trim().ToUpper();
                    string[] aAttrArray = sAttLine.Split(sgSeparator.ToCharArray());

                    IRowBuffer Buffer = pTable.CreateRowBuffer();
                    if (aAttrArray.Length == nAttrFldGS)
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
                                Buffer.set_Value(nPos, (object)oo2);
                            }
                        }
                    }
                    pCur.InsertRow(Buffer);                        
                    nRecordGS++;
                        
                } //while
                attrReader.Close();
                pCur.Flush();
                if (pWSE.IsBeingEdited() == true)
                {
                    pWSE.StopEditOperation();
                    pWSE.StopEditing(true);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCur);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWS);
                    
            }                

        
    


    }
}
