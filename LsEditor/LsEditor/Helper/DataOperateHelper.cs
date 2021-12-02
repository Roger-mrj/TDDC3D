using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace RCIS.Helper
{
    public class DataOperateHelper
    {
        public static string Connstring = "";
        /// <summary>
        /// 备份连接串
        /// </summary>
        public static string sCZDJConn = "";
        public static string sOraConn = "";

        #region //连接数据库
        public static OleDbConnection getConn()
        {

            try
            {
                OleDbConnection m_pConn;
                m_pConn = new OleDbConnection(DataOperateHelper.Connstring);

                return m_pConn;

            }
            catch (Exception e)
            {
                MessageBox.Show("警告:" + e.Message, "警告提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return null;

            }

        }
        #endregion

        #region//执行 OleDbCommand命令

        public static void getcmd(string sqlStr)
        {
            OleDbConnection myConn = getConn();
            try
            {

                myConn.Open();
                OleDbCommand mycmd = new OleDbCommand(sqlStr, myConn);
                mycmd.ExecuteNonQuery();
                mycmd.Dispose();
                myConn.Close();
                myConn.Dispose();
            }
            catch (Exception ex)
            {
                //RCIS.Helper.ErrorHelper.ShowErrorForm(ex, ""); 
                throw ex;
                return;
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
        #endregion

        #region//此方法用来执行SQL语句,并返回一个DataSet类型的数据集对象

        public static DataSet getDataSet(string sqlStr, string sTable)
        {
            OleDbConnection myConn = getConn();
            try
            {
                OleDbDataAdapter myDA = new OleDbDataAdapter(sqlStr, myConn);

                DataSet myDS = new DataSet();
                if (myDS.Tables.Contains(sTable))
                {
                    myDS.Tables[sTable].Clear();

                }

                myDA.Fill(myDS, sTable);

                return myDS;
            }
            catch (Exception ex)
            {
                //  RCIS.Helper.ErrorHelper.ShowErrorForm(ex, "");
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
        #endregion

        #region //获得SQL语句中查询结果的唯一值
        public static string GetOnlyRowValue(string sql, string sTable)
        {
            string str = "";
            DataSet ds = getDataSet(sql, sTable);
            if (ds.Tables[sTable].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[sTable].Rows[0];
                str = dr[0].ToString();
            }
            return str;
        }

        #endregion

        #region //获得SQL语句中查询得到唯一行
        public static DataRow GetOnlyRow(string sql, string sTable)
        {
            DataRow dr = null;
            DataSet ds = getDataSet(sql, sTable);
            if (ds.Tables[sTable].Rows.Count > 0)
            {
                dr = ds.Tables[sTable].Rows[0];

            }
            return dr;
        }

        #endregion
    }
}
