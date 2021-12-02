using System;
using System.Data;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
namespace RCIS.Database
{
    public class LS_DBExcelHelper
    {
        /// <summary>
        /// 将Excel文件内容读取到Dataset中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable Excel2DataSet(string filePath) // 将数据存到数据集中 
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;
            connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

            //else
            //{
            //    connStr = "Provider=Microsoft.ACE.OLEDB.13.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 13.0;HDR=NO;IMEX=1\"";
            //}
            string sql_F = "Select * FROM [{0}]";
            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dataTable = new DataTable();

            try
            {
                // 初始化连接，并打开                  
                conn = new OleDbConnection(connStr);
                conn.Open();
                da = new OleDbDataAdapter();
                da.SelectCommand = new OleDbCommand(String.Format(sql_F, "Sheet$"), conn);

                

                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
            }
            finally
            {                  // 关闭连接                  
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }
            conn.Close();
            da.Dispose();
            conn.Dispose();
            return dataTable;
        }


        public static  string getFirstsheetName(string srcFile)
        {
            Excel.Application myExcel = null;
            Excel.Workbook myBook = null;
            string sheetName = "";
            try
            {
                myExcel=new Excel.Application();
                object missing = System.Reflection.Missing.Value;
                myExcel.Application.Workbooks.Open(srcFile, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing); //this.txtFile.Text为Excel文件的全路径
                myBook = myExcel.Workbooks[1];

                //获取第一个Sheet
                Excel.Worksheet sheet = (Excel.Worksheet)myBook.Sheets[1];
                sheetName = sheet.Name; //Sheet名

            }
            catch { }
            finally
            {
                myBook.Close(Type.Missing, Type.Missing, Type.Missing);
                myExcel.Quit();
                myBook = null;
                myExcel = null;
            }
            

            
            return sheetName;

        }

        /// <summary>
        /// 将Excel文件内容读取到Dataset中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable Excel2Datatable(string filePath, int sheetIndex = 0) // 将数据存到数据集中 
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;
            switch (System.IO.Path.GetExtension(filePath).ToLower())
            {
                case ".xls":
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
                case ".xlsx":
                    connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
                default:
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
            }

            string sql_F = "Select * FROM [{0}]";
            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dataTable = new DataTable();

            try
            {
                // 初始化连接，并打开                  
                conn = new OleDbConnection(connStr);
                conn.Open();
                DataTable sheetNames = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (sheetNames.Rows.Count > sheetIndex && sheetIndex >= 0)
                {
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(String.Format(sql_F, sheetNames.Rows[sheetIndex]["TABLE_NAME"].ToString()), conn);
                    da.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {                  // 关闭连接                  
                if ((conn != null) && (conn.State == ConnectionState.Open))
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }

            return dataTable;
        }

        /// <summary>
        /// 将Excel文件内容读取到Dataset中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable Excel2Datatable(string filePath,string sheetName) // 将数据存到数据集中 
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;
            switch (System.IO.Path.GetExtension(filePath).ToLower())
            {
                case ".xls":
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
                case ".xlsx":
                    connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
                default:
                    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                    break;
            }

            //else
            //{
            //    connStr = "Provider=Microsoft.ACE.OLEDB.13.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 13.0;HDR=NO;IMEX=1\"";
            //}
            string sql_F = "Select * FROM [{0}]";
            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dataTable = new DataTable();

            try
            {
                // 初始化连接，并打开                  
                conn = new OleDbConnection(connStr);
                conn.Open();
                da = new OleDbDataAdapter();
                da.SelectCommand = new OleDbCommand(String.Format(sql_F, sheetName+"$"), conn);



                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
            }
            finally
            {                  // 关闭连接                  
                if ((conn!=null )&& (conn.State == ConnectionState.Open) )
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }
            
            return dataTable;
        }


        // 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// <param name="FILE_NAME">文件路径</param>
        /// <returns>文件的编码类型</returns>

        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }
        /// 通过给定的文件流，判断文件的编码类型
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        public static DataTable CSV2Datatable(string filePath)//从csv读取数据返回table
        {
            System.Text.Encoding encoding = GetType(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);

            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
        //


        public static DataTable CSV2Datatable2(string filePath)//从csv读取数据返回table
        {
            System.Text.Encoding encoding = GetType(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);

            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn("列"+i);
                        dt.Columns.Add(dc);
                    }

                    
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = tableHead[j];
                    }
                    dt.Rows.Add(dr);

                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            

            sr.Close();
            fs.Close();
            return dt;
        }


        /// <summary>
        /// Delete special symbol
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelQuota(string str)
        {
            string result = str;
            string[] strQuota = { "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "`", ";", "'", ",", ".", "/", ":", "/,", "<", ">", "?" };
            for (int i = 0; i < strQuota.Length; i++)
            {
                if (result.IndexOf(strQuota[i]) > -1)
                    result = result.Replace(strQuota[i], "");
            }
            return result;
        }

        /// <summary>
        /// Export the data from datatable to CSV file
        /// </summary>
        /// <param name="grid"></param>
        public static void ExportDataGridToCSV(DataTable dt,string destFile)
        {
           

            System.IO.FileStream fs = new FileStream(destFile, System.IO.FileMode.CreateNew, 
                System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, new System.Text.UnicodeEncoding());
            //Tabel header
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sw.Write(dt.Columns[i].ColumnName);
                sw.Write("\t");
            }
            sw.WriteLine("");
            //Table body
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sw.Write(DelQuota(dt.Rows[i][j].ToString()));
                    sw.Write("\t");
                }
                sw.WriteLine("");
            }
            sw.Flush();
            sw.Close();
            
        }


        /// <summary>
        /// Export the data from datatable to CSV file
        /// </summary>
        /// <param name="grid"></param>
        public static void ExportDataGridToCSV2(DataTable dt, string destFile)
        {


            System.IO.FileStream fs = new FileStream(destFile, System.IO.FileMode.CreateNew,
                System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, new System.Text.UTF8Encoding());
           
            //Table body
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string myStr = DelQuota(dt.Rows[i][j].ToString());
                    
                    sw.Write(myStr);
                    sw.Write("\t");
                }
                sw.WriteLine("");
            }
            sw.Flush();
            sw.Close();

        }

    }
}
