using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RCIS.Database;

namespace RuleCheck
{
    public class CheckErrorHelper
    {
        /// <summary>
        /// 插入一条错误明细
        /// </summary>
        /// <param name="aError"></param>
        /// <returns></returns>
        public static bool InsertAError(ACheckErrorObj aError)
        {
            bool bRet = true;
            AccdbOperateHelper dbhelper = new AccdbOperateHelper(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into CHK_ERRORLIST(规则编号,错误描述,错误类型,涉及图层,要素ID,要素BSM,错误级别) values('")
                .Append(aError.ruleId).Append("','").Append(aError.errorDescription).Append("','").Append(aError.errorType)
                .Append("','").Append(aError.errorLayer).Append("',").Append(aError.featureId).Append(",").Append(aError.featureBSM)
                .Append(",'").Append(aError.errorLevel).Append("' ) ");
                string sql = sb.ToString();
                bRet=dbhelper.ExecuteSQLNonquery(sql);
            }
            catch (Exception ex)
            {
                bRet = false;
            }
            finally
            {
                dbhelper.Close();
            }
            //IDbString dbstr = new AccessDbString(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    try
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append("insert into CHK_ERRORLIST(规则编号,错误描述,错误类型,涉及图层,要素ID,要素BSM,错误级别) values('")
            //        .Append(aError.ruleId).Append("','").Append(aError.errorDescription).Append("','").Append(aError.errorType)
            //        .Append("','").Append(aError.errorLayer).Append("',").Append(aError.featureId).Append(",").Append(aError.featureBSM)
            //        .Append(",'").Append(aError.errorLevel).Append("' ) ");
            //        string sql = sb.ToString();
            //        conn.ExecuteNonQuery(sql);
            //    }
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }
            //}
            return bRet;
        }


        /// <summary>
        /// 清空检查结果，以备第二次检查
        /// </summary>
        /// <param name="aTask"></param>
        public static bool ClearErrors()
        {
            bool bRet = true;
            AccdbOperateHelper dbhelper = new AccdbOperateHelper(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            try
            {
                string delSql = "delete from CHK_ERRORLIST  ";
                bRet = dbhelper.ExecuteSQLNonquery(delSql);
            }
            catch (Exception ex)
            {
                bRet = false;
            }
            finally
            {
                dbhelper.Close();
            }
            return bRet;

            //IDbString dbstr = new AccessDbString(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    try
            //    {
            //        string delSql = "delete from CHK_ERRORLIST  ";
            //        conn.ExecuteNonQuery(delSql);
                  
            //    }
            //    catch (Exception ex)
            //    {
            //        return false;
            //    }
            //}
            //return true;
        }


        public static void DeleAError( string ruleId, string ruleContent)
        {
            AccdbOperateHelper dbhelper = new AccdbOperateHelper(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            try
            {
                
                bool bRet = dbhelper.ExecuteSQLNonquery(" delete   from CHK_ERRORLIST where  规则编号='" + ruleId + "' and 错误描述='" + ruleContent + "'  ");
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                dbhelper.Close();
            }


            //IDbString dbstr = new AccessDbString(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    try
            //    {
            //        conn.ExecuteNonQuery(" delete   from CHK_ERRORLIST where  规则编号='" + ruleId + "' and 错误描述='" + ruleContent + "'  ");
            //    }
            //    catch { return; }
            //}
        }

    }
}
