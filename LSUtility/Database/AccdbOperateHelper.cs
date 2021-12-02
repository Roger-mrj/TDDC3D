using System;
using System.Data;
using System.Data.OleDb;

namespace RCIS.Database
{
    //// <summary>  
    /// AccdbOperateHelper 的摘要说明，以下信息请完整保留   
    /// 请在数据传递完毕后调用Close()方法，关闭数据链接。
    /// 新版本 可以 链接mdb和 mdbx ，只有字典表为替换，其他模块全部使用该类
    /// </summary>   
    public class AccdbOperateHelper
    {
        #region 变量声明处
        public OleDbConnection Conn;
        public string ConnString;//连接字符串  
        #endregion


        //构造函数与连接关闭数据库
        #region 构造函数与连接关闭数据库
        /**/
        /// <summary>  
        /// 构造函数   
        /// </summary>   
        /// <param name="Dbpath">ACCESS数据库路径</param>   
        public AccdbOperateHelper(string Dbpath)
        {
            string ext = System.IO.Path.GetExtension(Dbpath);
            if (ext.ToUpper().Trim() == ".MDB")
            {
                ConnString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=";
            }
            else if (ext.ToUpper().Trim() == ".ACCDB")
            {
                ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;;Data Source=";
            }
            //ConnString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=";  

            ConnString += Dbpath;
            Conn = new OleDbConnection(ConnString);
            Conn.Open();




        }

        /**/
        /// <summary>  
        /// 打开数据源链接   
        /// </summary>   
        /// <returns></returns>   
        public OleDbConnection DbConn()
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            return Conn;
        }

        /**/
        /// <summary>  
        /// 请在数据传递完毕后调用该函数，关闭数据链接。   
        /// </summary>   
        public void Close()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    return;

                Conn.Close();
            }
            catch { }
        }
        #endregion


        #region 数据库基本操作
        /**/
        /// <summary>  
        /// 根据SQL命令返回数据DataTable数据表,   
        /// 可直接作为dataGridView的数据源   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public DataTable GetDatatable(string SQL)
        {
            try
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                OleDbCommand command = new OleDbCommand(SQL, Conn);
                adapter.SelectCommand = command;
                DataTable Dt = new DataTable();
                adapter.Fill(Dt);
                return Dt;
            }
            catch (Exception ex) { }
            return null;



        }


        public bool isHaveData(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql, Conn);
            OleDbDataReader reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read())
                    return true;
                else
                    return false;
            }
            catch { return false; }
            finally
            {
                reader.Close();
            }
        }


        /**/
        /// <summary>  
        /// 根据SQL命令返回数据DataSet数据集，其中的表可直接作为dataGridView的数据源。   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <param name="subtableName">在返回的数据集中所添加的表的名称</param>   
        /// <returns></returns>   
        public DataSet SelectToDataSet(string SQL, string subtableName)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand(SQL, Conn);
            adapter.SelectCommand = command;
            DataSet Ds = new DataSet();
            Ds.Tables.Add(subtableName);
            adapter.Fill(Ds, subtableName);
            return Ds;
        }

        /**/
        /// <summary>  
        /// 在指定的数据集中添加带有指定名称的表，由于存在覆盖已有名称表的危险，返回操作之前的数据集。   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <param name="subtableName">添加的表名</param>   
        /// <param name="DataSetName">被添加的数据集名</param>   
        /// <returns></returns>   
        public DataSet SelectToDataSet(string SQL, string subtableName, DataSet DataSetName)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand(SQL, Conn);
            adapter.SelectCommand = command;
            DataTable Dt = new DataTable();
            DataSet Ds = new DataSet();
            Ds = DataSetName;
            adapter.Fill(DataSetName, subtableName);
            return Ds;
        }

        /**/
        /// <summary>  
        /// 根据SQL命令返回OleDbDataAdapter，   
        /// 使用前请在主程序中添加命名空间System.Data.OleDb   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public OleDbDataAdapter SelectToOleDbDataAdapter(string SQL)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbCommand command = new OleDbCommand(SQL, Conn);
            adapter.SelectCommand = command;
            return adapter;
        }

        /**/
        /// <summary>  
        /// 执行SQL命令，不需要返回数据的修改，删除可以使用本函数   
        /// </summary>   
        /// <param name="SQL"></param>   
        /// <returns></returns>   
        public bool ExecuteSQLNonquery(string SQL)
        {
            OleDbCommand cmd = new OleDbCommand(SQL, Conn);
            try
            {
                int i= cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion


        /// <summary>99
        /// 获取唯一值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetOnlyRow(string sql)
        {
            string result = "";
            OleDbCommand cmd = new OleDbCommand(sql, Conn);
            OleDbDataReader reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    result = reader.GetValue(0).ToString();
                }
            }
            catch (Exception ex) { }
            finally
            {
                reader.Close();
            }
            return result;

        }


    }  
}
