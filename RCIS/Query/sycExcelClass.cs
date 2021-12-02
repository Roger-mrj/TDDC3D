using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using Microsoft.Office.Interop.Excel;
namespace RCIS.Query
{
    public class sycExcelClass
    {
        public sycExcelClass() { }

        public bool OutToExcel(string sTJFldName, string sJSFldName, System.Data.DataTable myTable, string sXLSTemplate, out string sErrorInfo)
        {
            //�ռ�������:
            sErrorInfo = "";

            try
            {
                DataRowCollection myRows = myTable.Rows;
                Hashtable tj = new Hashtable();
                for (int i = 0; i < myRows.Count; i++)
                {
                    DataRow curRow = myRows[i];
                    object oo = curRow[sTJFldName];
                    if (tj.ContainsKey(oo) == false)
                        tj.Add(oo, 1);
                    else
                    {
                        int nGS = (int)(tj[oo]) + 1;
                        tj[oo] = nGS;
                    }
                }

                Hashtable tj02 = new Hashtable();
                ICollection myKeys = tj.Keys;
                foreach (object curKey in myKeys)
                {
                    object oo = curKey;     //Ӧ����TJField�ĸ�����Value:
                    double dSum = 0.0;        //�����͵�Sum:
                    for (int i = 0; i < myRows.Count; i++)
                    {
                        DataRow curRow = myRows[i];
                        object oNeed = curRow[sTJFldName];
                        if (oo.Equals(oNeed) == true)
                        {
                            object oValue = curRow[sJSFldName];
                            string sValue = oValue.ToString();
                            bool bOK = true;
                            for (int j = 0; j < sValue.Length; j++)
                            {
                                if (sValue[j] == '0' || sValue[j] == '1' || sValue[j] == '2' || sValue[j] == '3' || sValue[j] == '4' ||
                                    sValue[j] == '5' || sValue[j] == '6' || sValue[j] == '7' || sValue[j] == '8' || sValue[j] == '9' ||
                                    sValue[j] == '.' || sValue[j] == '+' || sValue[j] == '-') ;
                                else
                                {
                                    bOK = false;
                                    break;
                                }
                            } //for(int j=0;...
                            if (bOK == true)
                            {
                                dSum = dSum + Convert.ToDouble(sValue);
                            }
                        }
                    } //for(int i=0;...
                    tj02.Add(oo, dSum);
                }

                //A: ����Щ��Ϣ�����XLS�ļ���:
                Excel.Application thisApp = new Excel.ApplicationClass();
                thisApp.DisplayAlerts = false;
                Excel.Workbooks thisWBS = thisApp.Workbooks;
                object oMiss = Type.Missing;
                Excel.Workbook thisWB = thisWBS.Open(sXLSTemplate, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);

                //A-1: �ȰѸ�ģ�����Ϊ������ģ���ļ�:
                int nPos1 = sXLSTemplate.LastIndexOf('\\');
                string sA = sXLSTemplate.Substring(0, nPos1);
                int nPos2 = sA.LastIndexOf('\\');
                string sB = sA.Substring(0, nPos2);
                string sOutFile = sB + @"\Output\�ռ�������.XLS";
                thisWB.SaveAs(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, XlSaveAsAccessMode.xlExclusive, oMiss, oMiss, oMiss, oMiss, oMiss);
                thisWB = thisWBS.Open(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);
                Excel.Worksheet curSheet = thisWB.Worksheets.get_Item(1) as Excel.Worksheet;  //��һ��Sheet��Ϊ������

                //A-2: �Ե�һ�С��ϲ���Ԫ���������
                Excel.Range range = curSheet.get_Range("A1", "E1");
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 30;
                range.Font.Bold = true;
                range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                curSheet.Cells[1, 1] = "�ռ����������";

                //A-3: ���ͳ���ֶΣ������ֶ�
                range = curSheet.get_Range(curSheet.Cells[2, 1], curSheet.Cells[2, 2]);
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                curSheet.Cells[2, 1] = "ͳ���ֶ�: " + sTJFldName;

                range = curSheet.get_Range(curSheet.Cells[2, 4], curSheet.Cells[2, 5]);
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                curSheet.Cells[2, 4] = "�����ֶ�: " + sJSFldName;

                int nStartLine = 3;
                foreach (object curKey in tj02.Keys)
                {
                    string sTJ = curKey.ToString();
                    string sSum = tj02[curKey].ToString();

                    range = curSheet.get_Range(curSheet.Cells[nStartLine, 1], curSheet.Cells[nStartLine, 2]);
                    range.Merge(00);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    curSheet.Cells[nStartLine, 1] = sTJ;

                    range = curSheet.get_Range(curSheet.Cells[nStartLine, 4], curSheet.Cells[nStartLine, 5]);
                    range.Merge(00);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    curSheet.Cells[nStartLine, 4] = sSum;

                    nStartLine++;
                }

                //A-4: �������ͳ��ֵ�µ����м�¼:
                nStartLine++;
                foreach (object curKey in tj02.Keys)
                {
                    object oCurTJZ = curKey;
                    range = curSheet.get_Range(curSheet.Cells[nStartLine, 1], curSheet.Cells[nStartLine, 2]);
                    range.Merge(00);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    curSheet.Cells[nStartLine, 1] = sTJFldName + ":" + oCurTJZ.ToString();
                    nStartLine++;

                    //��ӡ���б���:
                    range = curSheet.get_Range(curSheet.Cells[nStartLine, 1], curSheet.Cells[nStartLine, myTable.Columns.Count]);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.RowHeight = 25;
                    range.Font.Bold = false;
                    range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                    for (int i = 0; i < myTable.Columns.Count; i++)
                    {
                        DataColumn curCol = myTable.Columns[i];
                        curSheet.Cells[nStartLine, i + 1] = curCol.ColumnName;
                    }
                    nStartLine++;

                    //��ӡ�����͵����м�¼:
                    for (int i = 0; i < myRows.Count; i++)
                    {
                        DataRow curRow = myRows[i];
                        if (curRow[sTJFldName].Equals(oCurTJZ) == true)
                        {
                            for (int j = 0; j < myTable.Columns.Count; j++)
                            {
                                string sColName = myTable.Columns[j].ColumnName;
                                object oV = curRow[sColName];
                                curSheet.Cells[nStartLine, j + 1] = oV.ToString();
                            }
                            nStartLine++;
                        }
                    } //for(int i=0;...

                    nStartLine++;
                }
                thisWB.Save();
                thisApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(thisApp);

                Process[] myProcesses = Process.GetProcessesByName("Excel");
                foreach (Process myProcess in myProcesses)
                {
                    myProcess.Kill();
                }

                Process myProcess2 = new Process();
                myProcess2.StartInfo.FileName = sOutFile;
                myProcess2.Start();
                return true;
            }
            catch (Exception E)
            {
                sErrorInfo = E.Message;
                return false;
            }

            //... ...
        }

        public bool OutToExcel02(string sOutLayerName, System.Data.DataTable myTable, string sXLSTemplate, out string sErrorInfo)
        {
            //���Բ�ѯ���:
            sErrorInfo = "";

            try
            {
                DataRowCollection myRows = myTable.Rows;

                //A: ����Щ��Ϣ�����XLS�ļ���:
                Excel.Application thisApp = new Excel.ApplicationClass();
                thisApp.DisplayAlerts = false;
                Excel.Workbooks thisWBS = thisApp.Workbooks;
                object oMiss = Type.Missing;
                Excel.Workbook thisWB = thisWBS.Open(sXLSTemplate, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);

                //A-1: �ȰѸ�ģ�����Ϊ������ģ���ļ�:
                int nPos1 = sXLSTemplate.LastIndexOf('\\');
                string sA = sXLSTemplate.Substring(0, nPos1);
                int nPos2 = sA.LastIndexOf('\\');
                string sB = sA.Substring(0, nPos2);
                string sOutFile = sB + @"\Output\���Բ�ѯ���.XLS";
                thisWB.SaveAs(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, XlSaveAsAccessMode.xlExclusive, oMiss, oMiss, oMiss, oMiss, oMiss);
                thisWB = thisWBS.Open(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);
                Excel.Worksheet curSheet = thisWB.Worksheets.get_Item(1) as Excel.Worksheet;  //��һ��Sheet��Ϊ������

                //A-2: �Ե�һ�С��ϲ���Ԫ���������
                Excel.Range range = curSheet.get_Range("A1", "E1");
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 40;
                range.Font.Bold = true;
                range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                curSheet.Cells[1, 1] = "���Բ�ѯ������\r\n[����:" + sOutLayerName + ",��¼��:" + myRows.Count.ToString() + "] ";

                //A-3: ���dataGrid�еĸ���
                int nStartLine = 3;

                //��ӡ���б���:
                range = curSheet.get_Range(curSheet.Cells[nStartLine, 1], curSheet.Cells[nStartLine, myTable.Columns.Count]);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 20;
                range.Font.Bold = false;
                range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                for (int i = 0; i < myTable.Columns.Count; i++)
                {
                    DataColumn curCol = myTable.Columns[i];
                    curSheet.Cells[nStartLine, i + 1] = curCol.ColumnName;
                }
                nStartLine++;

                //��ӡ���м�¼:
                for (int i = 0; i < myRows.Count; i++)
                {
                    DataRow curRow = myRows[i];
                    for (int j = 0; j < myTable.Columns.Count; j++)
                    {
                        string sColName = myTable.Columns[j].ColumnName;
                        object oV = curRow[sColName];
                        curSheet.Cells[nStartLine, j + 1] = oV.ToString();
                    }
                    nStartLine++;
                } //for(int i=0;...
                thisWB.Save();
                thisApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(thisApp);

                Process[] myProcesses = Process.GetProcessesByName("Excel");
                foreach (Process myProcess in myProcesses)
                {
                    myProcess.Kill();
                }

                Process myProcess2 = new Process();
                myProcess2.StartInfo.FileName = sOutFile;
                myProcess2.Start();
                return true;
            }
            catch (Exception E)
            {
                sErrorInfo = E.Message;
                return false;
            }

            //... ...
        }

    }
}
