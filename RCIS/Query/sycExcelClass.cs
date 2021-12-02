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
            //空间分析输出:
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
                    object oo = curKey;     //应该是TJField的各类型Value:
                    double dSum = 0.0;        //该类型的Sum:
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

                //A: 把这些信息输出到XLS文件中:
                Excel.Application thisApp = new Excel.ApplicationClass();
                thisApp.DisplayAlerts = false;
                Excel.Workbooks thisWBS = thisApp.Workbooks;
                object oMiss = Type.Missing;
                Excel.Workbook thisWB = thisWBS.Open(sXLSTemplate, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);

                //A-1: 先把该模板另存为、不动模板文件:
                int nPos1 = sXLSTemplate.LastIndexOf('\\');
                string sA = sXLSTemplate.Substring(0, nPos1);
                int nPos2 = sA.LastIndexOf('\\');
                string sB = sA.Substring(0, nPos2);
                string sOutFile = sB + @"\Output\空间分析结果.XLS";
                thisWB.SaveAs(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, XlSaveAsAccessMode.xlExclusive, oMiss, oMiss, oMiss, oMiss, oMiss);
                thisWB = thisWBS.Open(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);
                Excel.Worksheet curSheet = thisWB.Worksheets.get_Item(1) as Excel.Worksheet;  //第一个Sheet作为工作用

                //A-2: 对第一列、合并单元格，输出标题
                Excel.Range range = curSheet.get_Range("A1", "E1");
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 30;
                range.Font.Bold = true;
                range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                curSheet.Cells[1, 1] = "空间分析输出结果";

                //A-3: 输出统计字段＋计算字段
                range = curSheet.get_Range(curSheet.Cells[2, 1], curSheet.Cells[2, 2]);
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                curSheet.Cells[2, 1] = "统计字段: " + sTJFldName;

                range = curSheet.get_Range(curSheet.Cells[2, 4], curSheet.Cells[2, 5]);
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                curSheet.Cells[2, 4] = "计算字段: " + sJSFldName;

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

                //A-4: 输出各种统计值下的所有纪录:
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

                    //打印各列标题:
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

                    //打印该类型的所有纪录:
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
            //属性查询输出:
            sErrorInfo = "";

            try
            {
                DataRowCollection myRows = myTable.Rows;

                //A: 把这些信息输出到XLS文件中:
                Excel.Application thisApp = new Excel.ApplicationClass();
                thisApp.DisplayAlerts = false;
                Excel.Workbooks thisWBS = thisApp.Workbooks;
                object oMiss = Type.Missing;
                Excel.Workbook thisWB = thisWBS.Open(sXLSTemplate, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);

                //A-1: 先把该模板另存为、不动模板文件:
                int nPos1 = sXLSTemplate.LastIndexOf('\\');
                string sA = sXLSTemplate.Substring(0, nPos1);
                int nPos2 = sA.LastIndexOf('\\');
                string sB = sA.Substring(0, nPos2);
                string sOutFile = sB + @"\Output\属性查询结果.XLS";
                thisWB.SaveAs(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, XlSaveAsAccessMode.xlExclusive, oMiss, oMiss, oMiss, oMiss, oMiss);
                thisWB = thisWBS.Open(sOutFile, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss, oMiss);
                Excel.Worksheet curSheet = thisWB.Worksheets.get_Item(1) as Excel.Worksheet;  //第一个Sheet作为工作用

                //A-2: 对第一列、合并单元格，输出标题
                Excel.Range range = curSheet.get_Range("A1", "E1");
                range.Merge(00);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.RowHeight = 40;
                range.Font.Bold = true;
                range.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                curSheet.Cells[1, 1] = "属性查询输出结果\r\n[层名:" + sOutLayerName + ",记录数:" + myRows.Count.ToString() + "] ";

                //A-3: 输出dataGrid中的各行
                int nStartLine = 3;

                //打印各列标题:
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

                //打印所有纪录:
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
