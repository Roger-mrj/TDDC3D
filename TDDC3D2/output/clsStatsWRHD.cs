using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


using RCIS.Utility;
using RCIS.Database;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Data;

namespace TDDC3D.output
{
    /// <summary>
    /// 统计无人海岛调查面积
    /// </summary>
    public class clsStatsWRHD
    {
        private IWorkspace currWs = null;
        private IFeatureClass wrhdFeatureClass = null;

        public clsStatsWRHD(IWorkspace _ws)
        {
            try
            {
                this.currWs = _ws;
                IFeatureWorkspace pfeaWs = this.currWs as IFeatureWorkspace;
                wrhdFeatureClass = pfeaWs.OpenFeatureClass("WJMHD");
            }
            catch { }
        }

        private Dictionary<string, string> getZldwdmMc()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ITable pTable = (this.currWs as IFeatureWorkspace).OpenTable("QSDWDMB");
            IRow aRow = null;
            ICursor pCursor = pTable.Search(null, false);
            try
            {
                while ((aRow = pCursor.NextRow()) != null)
                {
                    dic.Add(FeatureHelper.GetRowValue(aRow, "QSDWDM").ToString(), FeatureHelper.GetRowValue(aRow, "QSDWMC").ToString());
                }
            }
            catch { }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
            }
            return dic;
        }

        /// <summary>
        /// 初始化无居民海岛面积
        /// </summary>
        public void InitWjmHd()
        {
            string sql = "delete from HZ_WRHDLYXZ";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            if (wrhdFeatureClass.FeatureCount(null) == 0)
                return;
            string[] allDls = new string[] { 
                "i101","i102","i103","i104","i105","i106","i107","i108","i109","i110","i201","i202","i203"
            };
            //按照地类统计
            Dictionary<string, string> dicZldw = this.getZldwdmMc();
            foreach (KeyValuePair<string, string> aItem in dicZldw)
            {
                if (aItem.Key.Trim().Length!=12)
                    continue;
                sql = "insert into HZ_WRHDLYXZ(ZLDWDM,ZLDWMC) values('" + aItem.Key + "','" + aItem.Value + "' ) ";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                //统计下面的 各地类
                foreach (string adl in allDls)
                {
                    double summj = 0;
                    FeatureHelper.StatsFieldSumValue(wrhdFeatureClass, "LYXZFLBM='" + adl + "' and ZLDWDM like '"+aItem.Key+"%'", "MJ");

                    sql = "update HZ_WRHDLYXZ set " + adl.ToUpper() + "=" + summj + " where ZLDWDM='" + aItem.Key + "'";
                    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                }
            }

            //计算小计
            StringBuilder sb = new StringBuilder();
            sb.Append("update HZ_WRHDLYXZ set I1=iif(isnull(I101),I101)+iif(isnull(I102),I102)+iif(isnull(I103),I103)+iif(isnull(I104),I104)+")
                .Append("iif(isnull(I105),I105)+iif(isnull(I106),I106)+iif(isnull(I107),I107)+iif(isnull(I108),I108)+iif(isnull(I109),I109)+")
                .Append("iif(isnull(I110),I110),I2=iif(isnull(I201),0,I201)+iif(isnull(I202),0,I202)+iif(isnull(I203),0,I203) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql="update HZ_WRHDLYXZ set HDMJ=I1+I2 ";
            iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //逐级汇总到 乡级，县级
            sb.Clear();
            sb.Append("insert into HZ_WRHDLYXZ(ZLDWDM,HDMJ, I1,I101,I102,I103,I104,I105,I106,I107,I108,I109,I110,I2,I201,I202,I203) ")
                .Append(" select left(ZLDWDM,9),sum(HDMJ),sum(I1),sum(I101),sum(I102),sum(I103),sum(I104),sum(I105),sum(I106),")
                .Append(" sum(I107),sum(I108),sum(I109),sum(I110),sum(I2),sum(I201),sum(I202),sum(I203) from HZ_WRHDLYXZ ")
                .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

            sb.Clear();
            sb.Append("insert into HZ_WRHDLYXZ(ZLDWDM,HDMJ, I1,I101,I102,I103,I104,I105,I106,I107,I108,I109,I110,I2,I201,I202,I203) ")
                .Append(" select left(ZLDWDM,6),sum(HDMJ),sum(I1),sum(I101),sum(I102),sum(I103),sum(I104),sum(I105),sum(I106),")
                .Append(" sum(I107),sum(I108),sum(I109),sum(I110),sum(I2),sum(I201),sum(I202),sum(I203) from HZ_WRHDLYXZ ")
                .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            iret=LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());


            foreach (KeyValuePair<string, string> aItem in dicZldw)
            {
                sql = "update HZ_WRHDLYXZ set ZLDWMC='" + aItem.Value + "' where ZLDWDM='" + aItem.Key + "'";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            }

        }
    }
}
