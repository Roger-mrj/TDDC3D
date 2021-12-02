using System;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace RCIS.Database
{
    public class LS_LlfxMDBHelper
    {
        /// <summary>
        /// 默认配置文件MDB路径
        /// </summary>
        public static string m_MDBFilePath = Application.StartupPath + @"\SystemConf\llfx.mdb";


        public  static OleDbConnection GetConn(string sMDBFilePath)
        {
            OleDbConnection oldDBConn = null;
            //string connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
            //            sMDBFilePath + @";Persist Security Info=false;Jet OLEDB:Database Password=KFQJY_2012_7K";
            string connStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sMDBFilePath;
            try
            {
                oldDBConn = new OleDbConnection(connStr);
            }
            catch (Exception e)
            {
                return null;
            }

            try
            {
                if (oldDBConn.State == System.Data.ConnectionState.Closed)
                {
                    oldDBConn.Open();
                }
                return oldDBConn;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 不指定MDB路径，默认为配置文件
        /// 返回Datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, string sTableName)
        {
            return GetDataTable(sql, sTableName, m_MDBFilePath);
        }
        /// <summary>
        /// 指定MDB路径，返回Datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="?"></param>
        /// <param name="sMDBFilePath"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, string sTableName, string sMDBFilePath)
        {
            OleDbConnection myConn = GetConn(sMDBFilePath);
            if (myConn == null)
            {
                return null;
            }
            OleDbDataAdapter myDA = new OleDbDataAdapter(sql, myConn);
            DataSet myDS = new DataSet();
            try
            {
                myDA.Fill(myDS, sTableName);
                return myDS.Tables[sTableName];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                    myConn.Dispose();
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
            DataTable dt = GetDataTable(sSQLString, sTable);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }


        /// <summary>
        /// 指定MDB路径，执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sMDBFilePath"></param>
        /// <returns></returns>
        public static int ExecuteSQL(string sql, string sMDBFilePath)
        {
            OleDbConnection myConn = GetConn(sMDBFilePath);
            if (myConn == null)
            {
                return -1;
            }
            try
            {
                OleDbCommand myCMD = new OleDbCommand(sql, myConn);
                int i = myCMD.ExecuteNonQuery();
                myCMD.Dispose();
                return i;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                    myConn.Dispose();
                }
            }
        }

        /// <summary>
        /// 不指定MDB路径，默认为配置文件路径
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteSQLNonquery(string sql)
        {
            return ExecuteSQL(sql, m_MDBFilePath);
        }
    }
}
