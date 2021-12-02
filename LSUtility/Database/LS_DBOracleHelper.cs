using System;
using System.Data;
using System.Data.OleDb;
namespace RCIS.Database
{
    public class LS_DBOracleHelper
    {
        /// <summary>
        /// Oracle数据库连接参数字符串
        /// </summary>
        private static string m_OracleOLEDBConnstring = "";
        /// <summary>
        /// 连接字符串，用于OLEDB连接
        /// 常用这个
        /// </summary>
        public static string OracleOLEDBConnstring
        {
            get
            {
                return m_OracleOLEDBConnstring;
            }
            set
            {
                m_OracleOLEDBConnstring = value;
            }
        }

        private static string m_OrcaleClientConnstring = "";
        /// <summary>
        /// 连接字符串，用于 OracleClient连接
        /// 设计数据文件的读写，用这个
        /// </summary>
        public static string OracleClientConnstring
        {
            get
            {
                return m_OrcaleClientConnstring;
            }
            set
            {
                m_OrcaleClientConnstring = value;
            }
        }
        #region //连接数据库
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public   static OleDbConnection GetConn()
        {
            try
            {
                OleDbConnection m_pConn;
                m_pConn = new OleDbConnection(m_OracleOLEDBConnstring);

                return m_pConn;

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return null;

            }

        }
        #endregion

        public static DataTable GetDatatable(string sqlStr, string sTable)
        {
            OleDbConnection myConn = GetConn();
            try
            {
                OleDbDataAdapter myDA = new OleDbDataAdapter(sqlStr, myConn);

                DataSet myDS = new DataSet();

                myDA.Fill(myDS, sTable);

                return myDS.Tables[sTable];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myConn != null)
                {
                    if (myConn.State == ConnectionState.Open)
                        myConn.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条SQL语句，返回第一行DataRow
        /// 一般为 Select 语句
        /// </summary>
        /// <param name="sSQLString"></param>
        /// <param name="sTable"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(string sSQLString, string sTable)
        {
            DataTable dt = GetDatatable(sSQLString, sTable);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }



        public static DataTable GetDatatable(string sqlStr)
        {
            OleDbConnection myConn = GetConn();
            try
            {
                OleDbDataAdapter myDA = new OleDbDataAdapter(sqlStr, myConn);

                DataSet myDS = new DataSet();

                myDA.Fill(myDS, "tab");

                return myDS.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myConn != null)
                {
                    if (myConn.State == ConnectionState.Open)
                        myConn.Close();
                }
            }
        }

        #region//执行 OleDbCommand命令

        /// <summary>
        /// 执行一条语句，返回影响行数
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static int ExecuteSQLNonquery(string sqlStr)
        {
            int count = 0;
            var myConn = GetConn();
            try
            {
                myConn.Open();
                OleDbCommand mycmd = new OleDbCommand(sqlStr, myConn);
                count = mycmd.ExecuteNonQuery();

                mycmd.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
            catch (Exception ex)
            {
                count = -1;
            }
            finally
            {
                if (myConn != null)
                {
                    if (myConn.State == ConnectionState.Open)
                        myConn.Close();
                }
            }
            return count;
        }
        #endregion
    }
}
