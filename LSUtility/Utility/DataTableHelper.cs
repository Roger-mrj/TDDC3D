using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LSUtility
{
    public class DataTableHelper
    {
        public static DataTable GetSumByDatatable(DataTable dt, string groupField, string statisticsField)
        {
            var query = dt.AsEnumerable().GroupBy(t => t.Field<string>(groupField))
                      .Select(g => new
                      {
                          c1 = g.Key,
                          c3 = g.Sum(m => Convert.ToDouble(m[statisticsField]))
                      });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(groupField);
            dtResult.Columns.Add(new DataColumn(statisticsField, typeof(float)));
            query.ToList().ForEach(q => dtResult.Rows.Add(q.c1, q.c3));
            return dtResult;
        }

        public static DataTable GetSumByDatatable(DataTable dt, string groupField, string statisticsField,int len)
        {
            var query = dt.AsEnumerable().GroupBy(t => t.Field<string>(groupField).Substring(0,len))
                      .Select(g => new
                      {
                          c1 = g.Key,
                          c3 = Math.Round(g.Sum(m => Convert.ToDouble(m[statisticsField])), 2)
                      });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(groupField);
            dtResult.Columns.Add(new DataColumn(statisticsField, typeof(double)));
            query.ToList().ForEach(q => dtResult.Rows.Add(q.c1, q.c3));
            return dtResult;
        }


        //public static DataTable GetSumByDatatable(DataTable dt, string groupField1, string groupField2, string statisticsField)
        //{
        //    var query = from c in dt.AsEnumerable().Group c By new {
        //        a = c.Field<string>(groupField1), 
        //        b = c.Field<string>(groupField2)
        //    }
        //        into s 
        //        select new 
        //              {
        //                  a = s.Select(c => c.Field<string>(groupField1)).First(),
        //                  c2=g.Key[1],
        //                  c3 = g.Sum(m => Convert.ToDouble(m[statisticsField]))
        //              };

        //    DataTable dtResult = new DataTable();
        //    dtResult.Columns.Add(groupField1);
        //    dtResult.Columns.Add(groupField2);
        //    dtResult.Columns.Add(new DataColumn(statisticsField, typeof(float)));
        //    query.ToList().ForEach(q => dtResult.Rows.Add(q.c1, q.c3));
        //    return dtResult;
        //}

        public static string getDataTable2InsertSQL(DataTable dt, string tableName)
        {
            string sql = string.Empty;
            string insertSQL = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                sql = string.Empty;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sql += "'" + dr[i].ToString() + "' ,";
                }
                insertSQL = "INTO " + tableName + " VALUES (" + sql.Substring(0, sql.Length - 1) + ") ";
            }
            return @"INSERT ALL " + insertSQL + " SELECT 1 FROM DUAL";
        }

        public static string getDataTableWhereClause(DataTable dt, string colName ,int colNumber, Boolean isAnd = false)
        {
            string sWhere = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                sWhere = string.Empty;
                sWhere += colName + " = '" + dr[colNumber].ToString() + (isAnd ? "' and," : "'  or,");
            }
            return sWhere.Substring(0, sWhere.Length - 4);
        }
    }
}
