using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RCIS.Database;
using RCIS.Utility;
using System.Data;
namespace TDDC3D.output
{
    /// <summary>
    /// 该类根据国家新的规程要求对数据进行调平
    /// </summary>
    /// 具体调平算法
    /// **********************************************
    /* （一）基础计算表正确性检查
    将基础计算表中的土地总面积（平方米）进行汇总，与县级行政辖区控制面积（平方米）进行比较，
    如果不一致，应检查核对重新计算汇总。
    （二）基础统计表控制
    将县级行政辖区的控制面积单位换算到公顷，保留2位小数，作为下一步面积调平的控制数a。
    将基础计算表经面积单位换算得到基础统计表，汇总基础统计表的土地总面积字段，得到汇总值b。
    （三）基础统计表调平
    1．计算调平控制数a与汇总值b的差值，得到调平数c；
    2．调平数c/0.01就是要调平的数目d，将数目d除以村个数，得到商e及余数f。
    3．按照各村的面积从大到小找出前f个村，这些村的调平面积为（e+1）*0.01，其余的村调平面积为e*0.01。
   
    4．本村内记录数的调平方法与上述方法相同。
    5．各记录的土地总面积=原土地总面积+调平面积，
       调平后的各记录的土地总面积字段的数值做为这个记录中横向各地类面积值调平控制面积g。
    6．计算调平控制面积g与村级单位的各二级地类汇总值h的差值，得到调平数j。
    7．按照地类编码倒序的优先原则对记录的二级地类面积进行面积调平，
       当地类面积中有1公顷以上数据时，在1公顷以下的地类数据不参与调平。
     * 横向需要注意1：可能全部小于1公顷 也要进行相关的调平
     * 但是只要有一个大于1公顷  就只对大于1公顷的数据进行调平
        **/
    public class ClsTP
    {

        public ClsTP()
        {
            SetDlbm();
        }

        #region 字符串转换为整数
        private int String2Int(string str)
        {
            try
            {
                if (str == "") return 0;
                int a = 0;
                int.TryParse(str, out a);
                return a;
            }
            catch { return 0; }

        }
        #endregion
        
        #region 浮点型转换为整数
        private int Double2Int(double str)
        {
            try
            {
                if (str == 0.00) return 0;
                int a = 0;
                int.TryParse(str.ToString(), out a);
                return a;
            }
            catch { return 0; }

        }
        #endregion

        private double String2Double(string str)
        {
            try
            {
                if (str == "") return 0.00;
                double a = 0.00;
                double.TryParse(str, out a);
                return a;
            }
            catch { return 0.00; }

        }

       
        //所有地类编码
        private List<string> listDLBM = new List<string>();
        private void SetDlbm()
        {
            listDLBM = new List<string>();
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select DM from 三调工作分类 order by SORT", "FL");
            foreach (DataRow dr in dt.Rows)
            {
                listDLBM.Add("D"+dr["DM"].ToString().Trim());
            }
            
        }
     
        
        


       
        /// <summary>
        /// 取辖区 总面积 
        /// </summary>
        /// <param name="sTable"></param>
        /// <param name="isGq">是否转化为公顷</param>
        /// <returns></returns>
        private double GetXQKZMJ(string sTable,bool isGq, string mssm)
        {
            double mj = 0;
            string sql = "select sum(TDZMJ) from " + sTable + " Where MSSM= '" + mssm + "'";
            DataRow dr = LS_ResultMDBHelper.GetDataRow(sql, "tmp");
            if (dr == null)
                return 0;
            string smj = dr[0].ToString();
            double.TryParse(smj, out mj);
            if (isGq)
            {
                //辖区控制总面积  换算成公顷
                mj = mj / 10000;
                return MathHelper.RoundEx(mj, 2);
            }
            else
            {
                return mj;
            }

        }

        private double GetXQKZMJLDHD(string sTable, bool isGq,string mssm)
        {
            double zmj = 0;
            string sql = "select sum(TDZMJ) from " + sTable + " where MSSM = '" + mssm + "'";
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double mj = 0;
                string smj = dt.Rows[i][0].ToString();
                double.TryParse(smj, out mj);
                if (isGq)
                {
                    //辖区控制总面积  换算成公顷
                    mj = mj / 10000;
                    zmj += MathHelper.RoundEx(mj, 2);
                }
                else
                {
                    zmj += mj;
                }
            }
            return zmj;

        }
       

        
        #region 对横向地类进行调平

        private void DLTP2(string sTable)
        {
            string sql = "";
            #region  //如果纵向已经平了之后，直接看横向的
            if (sTable == "HZ_JCB")
            {
                StringBuilder sb=new StringBuilder();
                sb.Append("select  ZLDWDM,QSDWDM,QSXZ, GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM, FRDBS,TDZMJ,")
                    .Append("iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
                    .Append("iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K)+iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K)+iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K)+iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K)+")
                    .Append("iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K)+iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K)+")
                    .Append("iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K)+iif(isnull(D0404),0,D0404)+")
                    .Append("iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+")
                    .Append("iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+")
                    .Append("iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A)+")
                    .Append("iif(isnull(D09),0,D09)+")
                    .Append("iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
                    .Append("iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                    .Append("iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
                    .Append("iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) as MJ ")
                  .Append(" from ").Append(sTable).Append(" ")
                    .Append(" order by TDZMJ desc ");
                sql = sb.ToString();
            }
            else if (sTable == "HZ_JBNT_JCB")
            {
                
            }
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
            double TDZMJ = 0.00;//总面积
            double MJ = 0.00;//各二级地类之和
            double dTPS = 0.00;//两者差值  调平数
            double   dTPNum = 0;//调平数目
           
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string zldwdm = dr["ZLDWDM"].ToString().Trim();
                    string qsdwdm = dr["QSDWDM"].ToString().Trim();
                    string qsxz = dr["QSXZ"].ToString().Trim();
                    string gdpdj = dr["GDPDJB"].ToString().Trim();
                    string gdlx = dr["GDLX"].ToString().Trim();
                    string gdzzsx = dr["GDZZSXDM"].ToString().Trim();
                    string czcsxm = dr["CZCSXM"].ToString().Trim();
                    string tbxhdm = dr["TBXHDM"].ToString().Trim();
                    string mssm = dr["MSSM"].ToString().Trim();
                    string frdbs = dr["FRDBS"].ToString().Trim();

                    string whereClause = " ZLDWDM='" + zldwdm + "' and QSDWDM='" + qsdwdm + "' and QSXZ='" + qsxz + "' and GDPDJB='" + gdpdj + "' and GDLX='"
                        + gdlx + "' and GDZZSXDM='" + gdzzsx + "' and CZCSXM='" + czcsxm + "' and TBXHDM='" + tbxhdm + "' and MSSM='" + mssm + "' and FRDBS='"+frdbs+"' ";

                    TDZMJ = String2Double(dr["TDZMJ"].ToString());
                   

                    MJ = String2Double(dr["MJ"].ToString());
                    dTPS =MathHelper.Round( TDZMJ - MJ,2);
                    if (dTPS == 0) continue;


                    bool isNav = false;
                    if (dTPS > 0)
                    {
                        isNav = false; //总面积比合计面积大
                    }
                    else
                    {
                        isNav = true; // 比合计面积小 
                    }
                    
                    dTPS = Math.Abs(dTPS);//调平数
                    dTPNum =MathHelper.Round( dTPS / 0.01,0);
                    //放入面积小于等于1公顷
                    List<string> pMaxDLMC = new List<string>();
                    List<double> pMaxDLMJ = new List<double>();

                    //放入面积大于1公顷
                    List<string> pMinDLMC = new List<string>();
                    List<double> pMinDLMJ = new List<double>();

                    #region 获得有值的二级地类和面积
                    sql = "select * from HZ_JCB_PFM  where " + whereClause;  //这个 需要从原来平方米的 数量里面查询
                    double s2DLMJ = 0.00;
                    string s2DL = "";
                    //按照顺序进行插入列表
                    //后面取值进行倒序排列 从最后一行往前取值
                    DataRow ddr = LS_ResultMDBHelper.GetDataRow(sql, "tmp");
                    
                    for (int index = 0; index < this.listDLBM.Count; index++)
                    {
                        s2DLMJ = String2Double(ddr[listDLBM[index]].ToString());
                        s2DL = listDLBM[index];

                        if (s2DLMJ > 0)
                        {
                           // if (s2DLMJ > 1)
                            if (s2DLMJ > 10000)  //大于10000平米
                            {
                                pMaxDLMC.Add(s2DL);
                                pMaxDLMJ.Add(s2DLMJ);
                            }
                            //else if ((s2DLMJ <= 1) && (s2DLMJ > 0))
                            else if ((s2DLMJ <= 10000) && (s2DLMJ > 0))
                            {
                                pMinDLMC.Add(s2DL);
                                pMinDLMJ.Add(s2DLMJ);
                            }
                        }
                    }
                    #endregion

                  
                   

                    //对获得大于1公顷的处理
                    if (pMaxDLMC.Count > 0)
                    {
                        
                        #region >1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMaxDLMC.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMaxDLMC.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMaxDLMC.Count + sRemainder;
                       
                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMaxDLMC.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = pMaxDLMC.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMaxDLMJ[j] / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);
                                    sql = "update " + sTable + " set " + pMaxDLMC[j] + "="+ tpmj+" where "+whereClause;
                                    int iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {
                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = pMaxDLMC.Count - 1; j >= 0; j--)
                            {
                                sMax = (sQuotient + 1) * 0.01;
                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }
                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxDLMJ[j] / 10000, 2) + sMax;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                        sql = "update " + sTable + " set " + pMaxDLMC[j] + "=" +gqMj+" where "+whereClause;
                                        int result = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxDLMJ[j] / 10000, 2) + sMin;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                       // sql = "update " + sTable + " set " + pMaxDLMC[j] + "=" + pMaxDLMJ[j] + "+(" + sMin + ") where "+whereClause;
                                        sql = "update " + sTable + " set " + pMaxDLMC[j] + "="+ gqMj+"   where " + whereClause;
                                        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                        #endregion
                    }
                    else if ((pMaxDLMC.Count == 0) && (pMinDLMC.Count > 0))
                    {//如果一行都是小于等于1公顷也是同样的处理
                        #region <1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMinDLMC.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMinDLMC.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMinDLMC.Count + sRemainder;

                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMinDLMC.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = pMinDLMC.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMinDLMJ[j] / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);

                                    sql = "update " + sTable + " set " + pMinDLMC[j] + "=" +tpmj+ " where " + whereClause;
                                    int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = pMinDLMC.Count - 1; j >= 0; j--)
                            {
                                sMax = (sQuotient + 1) * 0.01;

                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double tpmj = MathHelper.Round(pMinDLMJ[j] / 10000, 2) + sMax;
                                        tpmj = MathHelper.Round(tpmj, 2);
                                        sql = "update " + sTable + " set " + pMinDLMC[j] + "=" +tpmj+"  where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double tpmj= MathHelper.Round( pMinDLMJ[j] /10000,2)+sMin;
                                        tpmj=MathHelper.Round(tpmj,2);
                                        sql = "update " + sTable + " set " + pMinDLMC[j] + "=" + tpmj + " where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //降序加0.01
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / this.listDLBM.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(this.listDLBM.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = this.listDLBM.Count + sRemainder;
                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / this.listDLBM.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {
                                    
                                    sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sTPMJ + ") where " + whereClause;
                                    int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {
                                sMax = (sQuotient + 1) * 0.01;
                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMax + ") where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMin + ") where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }                
                
            }

                              

            //从新整理一下经过变更后的列，使其二级地类之和等于一级地类
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("update HZ_JCB set D0201HJ=iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K),")
                .Append(" D0202HJ=iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K),")
                .Append(" D0203HJ=iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K),")
                .Append(" D0204HJ=iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K),")
                .Append(" D0301HJ=iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K),")
                .Append(" D0302HJ=iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K),")
                .Append(" D0307HJ=iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K),")
                .Append(" D0403HJ=iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K),")
                .Append(" D08H2HJ=iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A),")
                .Append(" D0810HJ=iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A),")
                .Append(" D1104HJ=iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K),")
                .Append(" D1107HJ=iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A) ");
            sql = sb2.ToString();
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb2.Clear();
            sb2.Append("update HZ_JCB set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
            .Append(" D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                .Append("D02=iif(isnull(D0201HJ),0,D0201HJ)+iif(isnull(D0202HJ),0,D0202HJ)+iif(isnull(D0203HJ),0,D0203HJ)+iif(isnull(D0204HJ),0,D0204HJ),")
                .Append("D03=iif(isnull(D0301HJ),0,D0301HJ)+iif(isnull(D0302HJ),0,D0302HJ)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307HJ),0,D0307HJ),")
                .Append("D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403HJ),0,D0403HJ)+iif(isnull(D0404),0,D0404),")
                .Append("D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append("D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                .Append("D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2HJ),0,D08H2HJ)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810HJ),0,D0810HJ),")
                .Append("D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append("D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                .Append("iif(isnull(D1104HJ),0,D1104HJ)+iif(isnull(D1107HJ),0,D1107HJ)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                .Append("D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207)  ");
            sql = sb2.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            #endregion
        }


        private void DLTP(string sTable)
        {
            string sql = "";
            #region 获取不相等的
            if (sTable == "HZ_JCB")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select  ZLDWDM,QSDWDM,QSXZ, GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM, FRDBS,TDZMJ,")
                    .Append("iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
                    .Append("iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K)+iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K)+iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K)+iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K)+")
                    .Append("iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K)+iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K)+")
                    .Append("iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K)+iif(isnull(D0404),0,D0404)+")
                    .Append("iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+")
                    .Append("iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+")
                    .Append("iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A)+")
                    .Append("iif(isnull(D09),0,D09)+")
                    .Append("iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
                    .Append("iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                    .Append("iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
                    .Append("iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) as MJ ")
                  .Append(" from ").Append(sTable).Append(" ")
                    .Append(" order by TDZMJ desc ");
                sql = sb.ToString();
            }
           
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
            double TDZMJ = 0.00;//总面积
            double MJ = 0.00;//各二级地类之和
            double dTPS = 0.00;//两者差值  调平数
            double dTPNum = 0;//调平数目
            #endregion 

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string zldwdm = dr["ZLDWDM"].ToString().Trim();
                    string qsdwdm = dr["QSDWDM"].ToString().Trim();
                    string qsxz = dr["QSXZ"].ToString().Trim();
                    string gdpdj = dr["GDPDJB"].ToString().Trim();
                    string gdlx = dr["GDLX"].ToString().Trim();
                    string gdzzsx = dr["GDZZSXDM"].ToString().Trim();
                    string czcsxm = dr["CZCSXM"].ToString().Trim();
                    string tbxhdm = dr["TBXHDM"].ToString().Trim();
                    string mssm = dr["MSSM"].ToString().Trim();
                    string frdbs = dr["FRDBS"].ToString().Trim();

                    string whereClause = " ZLDWDM='" + zldwdm + "' and QSDWDM='" + qsdwdm + "' and QSXZ='" + qsxz + "' and GDPDJB='" + gdpdj + "' and GDLX='"
                        + gdlx + "' and GDZZSXDM='" + gdzzsx + "' and CZCSXM='" + czcsxm + "' and TBXHDM='" + tbxhdm + "' and MSSM='" + mssm + "' and FRDBS='" + frdbs + "' ";

                    TDZMJ = String2Double(dr["TDZMJ"].ToString());
                    MJ = String2Double(dr["MJ"].ToString());      //公顷的
                    dTPS = MathHelper.Round(TDZMJ - MJ, 2);
                    if (dTPS == 0) continue;
                    bool isNav = false;
                    if (dTPS > 0)
                    {
                        isNav = false; //总面积比合计面积大
                    }
                    else
                    {
                        isNav = true; // 比合计面积小 
                    }

                    dTPS = Math.Abs(dTPS);//调平数
                    dTPNum = MathHelper.Round(dTPS / 0.01, 0);
                    ////放入面积小于等于1公顷
                    //List<string> pMaxDLMC = new List<string>();
                    //List<double> pMaxDLMJ = new List<double>();
                    List<TPItem> pMaxTPItem = new List<TPItem>();

                    ////放入面积大于1公顷
                    //List<string> pMinDLMC = new List<string>();
                    //List<double> pMinDLMJ = new List<double>();
                    List<TPItem> pMinTPItem = new List<TPItem>();

                    #region 获得有值的二级地类和面积
                    sql = "select * from HZ_JCB_PFM  where " + whereClause;  //这个 需要从原来平方米的 数量里面查询
                    double s2DLMJ = 0.00;
                    string s2DL = "";
                    //按照顺序进行插入列表
                    //后面取值进行倒序排列 从最后一行往前取值
                    DataRow ddr = LS_ResultMDBHelper.GetDataRow(sql, "tmp");

                    for (int index = 0; index < this.listDLBM.Count; index++)
                    {
                        s2DLMJ = String2Double(ddr[listDLBM[index]].ToString());
                        s2DL = listDLBM[index];

                        if (s2DLMJ > 0)
                        {
                            // if (s2DLMJ > 1)
                            if (s2DLMJ > 10000)  //大于10000平米
                            {
                                //pMaxDLMC.Add(s2DL);
                                //pMaxDLMJ.Add(s2DLMJ);
                                pMaxTPItem.Add(new TPItem(s2DL,s2DLMJ,listDLBM));
                            }
                            //else if ((s2DLMJ <= 1) && (s2DLMJ > 0))
                            else if ((s2DLMJ <= 10000) && (s2DLMJ > 0))
                            {
                                //pMinDLMC.Add(s2DL);
                                //pMinDLMJ.Add(s2DLMJ);
                                pMinTPItem.Add(new TPItem(s2DL, s2DLMJ, listDLBM));
                            }
                        }
                    }
                    #endregion


                    //对获得大于1公顷的处理
                    if (pMaxTPItem.Count > 0)
                    {

                        pMaxTPItem.Sort(); //已经是从大到小了

                        #region >1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMaxTPItem.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMaxTPItem.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMaxTPItem.Count + sRemainder;

                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMaxTPItem.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            //for (int j = pMaxTPItem.Count - 1; j >= 0; j--)
                            for (int j =0;j<= pMaxTPItem.Count - 1;j++)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMaxTPItem[j].dlmj / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);
                                    sql = "update " + sTable + " set " + pMaxTPItem[j].dldm + "=" + tpmj + " where " + whereClause;
                                    int iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {
                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            
                            sMax = (sQuotient + 1) * 0.01;
                            sMin = sQuotient * 0.01;
                            if (isNav == true)
                            {
                                sMax = 0 - sMax;
                                sMin = 0 - sMin;
                            }

                            for (int j = 0; j < pMaxTPItem.Count;j++ )
                            {                                
                                //放入Max
                                if (j < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxTPItem[j].dlmj / 10000, 2) + sMax;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                        sql = "update " + sTable + " set " + pMaxTPItem[j].dldm + "=" + gqMj + " where " + whereClause;
                                        int result = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxTPItem[j].dlmj / 10000, 2) + sMin;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                        // sql = "update " + sTable + " set " + pMaxDLMC[j] + "=" + pMaxDLMJ[j] + "+(" + sMin + ") where "+whereClause;
                                        sql = "update " + sTable + " set " + pMaxTPItem[j].dldm + "=" + gqMj + "   where " + whereClause;
                                        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                //flag++;
                            }
                        }
                        #endregion
                    }
                    else if ((pMaxTPItem.Count == 0) && (pMinTPItem .Count > 0))
                    {//如果一行都是小于等于1公顷也是同样的处理
                        #region <1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMinTPItem.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMinTPItem.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMinTPItem.Count + sRemainder;

                        pMinTPItem.Sort();

                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMinTPItem.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = 0; j < pMinTPItem.Count; j++)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMinTPItem[j].dlmj / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);

                                    sql = "update " + sTable + " set " + pMinTPItem[j].dldm + "=" + tpmj + " where " + whereClause;
                                    int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = 0; j < pMinTPItem.Count; j++)
                            {
                                sMax = (sQuotient + 1) * 0.01;
                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double tpmj = MathHelper.Round(pMinTPItem[j].dlmj / 10000, 2) + sMax;
                                        tpmj = MathHelper.Round(tpmj, 2);
                                        sql = "update " + sTable + " set " + pMinTPItem[j].dldm + "=" + tpmj + "  where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double tpmj = MathHelper.Round(pMinTPItem[j].dlmj / 10000, 2) + sMin;
                                        tpmj = MathHelper.Round(tpmj, 2);
                                        sql = "update " + sTable + " set " + pMinTPItem[j].dldm + "=" + tpmj + " where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //降序加0.01
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / this.listDLBM.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(this.listDLBM.Count.ToString())));

                        //都是0 ，的就按照地类代码倒序
                        if (sRemainder < 0)
                            sRemainder = this.listDLBM.Count + sRemainder;
                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / this.listDLBM.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {

                                    sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sTPMJ + ") where " + whereClause;
                                    int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;
                            sMax = (sQuotient + 1) * 0.01;
                            sMin = sQuotient * 0.01;
                            if (isNav == true)
                            {
                                sMax = 0 - sMax;
                                sMin = 0 - sMin;
                            }

                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {                               

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMax + ") where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMin + ") where " + whereClause;
                                        int tmpr = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

            }



            //从新整理一下经过变更后的列，使其二级地类之和等于一级地类
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("update HZ_JCB set D0201HJ=iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K),")
                .Append(" D0202HJ=iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K),")
                .Append(" D0203HJ=iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K),")
                .Append(" D0204HJ=iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K),")
                .Append(" D0301HJ=iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K),")
                .Append(" D0302HJ=iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K),")
                .Append(" D0307HJ=iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K),")
                .Append(" D0403HJ=iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K),")
                .Append(" D08H2HJ=iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A),")
                .Append(" D0810HJ=iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A),")
                .Append(" D1104HJ=iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K),")
                .Append(" D1107HJ=iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A) ");
            sql = sb2.ToString();
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb2.Clear();
            sb2.Append("update HZ_JCB set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
            .Append(" D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                .Append("D02=iif(isnull(D0201HJ),0,D0201HJ)+iif(isnull(D0202HJ),0,D0202HJ)+iif(isnull(D0203HJ),0,D0203HJ)+iif(isnull(D0204HJ),0,D0204HJ),")
                .Append("D03=iif(isnull(D0301HJ),0,D0301HJ)+iif(isnull(D0302HJ),0,D0302HJ)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307HJ),0,D0307HJ),")
                .Append("D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403HJ),0,D0403HJ)+iif(isnull(D0404),0,D0404),")
                .Append("D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append("D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                .Append("D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2HJ),0,D08H2HJ)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810HJ),0,D0810HJ),")
                .Append("D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append("D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                .Append("iif(isnull(D1104HJ),0,D1104HJ)+iif(isnull(D1107HJ),0,D1107HJ)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                .Append("D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207)  ");
            sql = sb2.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            
            #endregion
        }

        private void ChangeTableDW2GQ2(string sTable)
        {
            string sql = "";
            int iRet;
            try
            {
                if (sTable == "HZ_JBNT_BH_JCB")
                {
                    sql = "update " + sTable + " set TDZMJ=round(TDZMJ/10000,2),D010=round(D010/10000,2),D011=round(D011/10000,2),D012=round(D012/10000,2),D013=round(D013/10000,2)" +
                               ",OLDAREA=round(OLDAREA/10000,2)";
                    iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                }
                else
                {

                    iRet = LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_JCB");

                    DataTable dt = LS_ResultMDBHelper.GetDataTable("select * from HZ_JCB_PFM ", "tmp");
                    foreach (DataRow dr in dt.Rows)
                    {

                        #region 都要四舍五入

                        double oldmj = dr["OLDAREA"].ToString().Trim() == "" ? 0 : double.Parse(dr["OLDAREA"].ToString());
                        oldmj = MathHelper.Round(oldmj / 10000, 2);

                        double d0101 = dr["D0101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0101"].ToString());
                        d0101 = MathHelper.Round(d0101 / 10000, 2);
                        double d0102 = dr["D0102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0102"].ToString());
                        d0102 = MathHelper.Round(d0102 / 10000, 2);
                        double d0103 = dr["D0103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0103"].ToString());
                        d0103 = MathHelper.Round(d0103 / 10000, 2);
                        double D01 = MathHelper.Round(d0101 + d0102 + d0103, 2);

                        double D0201 = dr["D0201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201"].ToString());
                        D0201 = MathHelper.Round(D0201 / 10000, 2);
                        double D0202 = dr["D0202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202"].ToString());
                        D0202 = MathHelper.Round(D0202 / 10000, 2);
                        double D0201K = dr["D0201K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201K"].ToString());
                        D0201K = MathHelper.Round(D0201K / 10000, 2);
                        double D0202K = dr["D0202K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202K"].ToString());
                        D0202K = MathHelper.Round(D0202K / 10000, 2);
                        double D0203 = dr["D0203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203"].ToString());
                        D0203 = MathHelper.Round(D0203 / 10000, 2);
                        double D0203K = dr["D0203K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203K"].ToString());
                        D0203K = MathHelper.Round(D0203K / 10000, 2);
                        double D0204 = dr["D0204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204"].ToString());
                        D0204 = MathHelper.Round(D0204 / 10000, 2);
                        double D0204K = dr["D0204K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204K"].ToString());
                        D0204K = MathHelper.Round(D0204K / 10000, 2);
                        double D02 = MathHelper.Round(D0201 + D0201K + D0202 + D0202K + D0203 + D0203K + D0204 + D0204K, 2);

                        double D0301 = dr["D0301"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301"].ToString());
                        D0301 = MathHelper.Round(D0301 / 10000, 2);
                        double D0301K = dr["D0301K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301K"].ToString());
                        D0301K = MathHelper.Round(D0301K / 10000, 2);
                        double D0302 = dr["D0302"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302"].ToString());
                        D0302 = MathHelper.Round(D0302 / 10000, 2);
                        double D0302K = dr["D0302K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302K"].ToString());
                        D0302K = MathHelper.Round(D0302K / 10000, 2);
                        double D0303 = dr["D0303"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0303"].ToString());
                        D0303 = MathHelper.Round(D0303 / 10000, 2);
                        double D0304 = dr["D0304"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0304"].ToString());
                        D0304 = MathHelper.Round(D0304 / 10000, 2);
                        double D0305 = dr["D0305"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0305"].ToString());
                        D0305 = MathHelper.Round(D0305 / 10000, 2);
                        double D0306 = dr["D0306"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0306"].ToString());
                        D0306 = MathHelper.Round(D0306 / 10000, 2);
                        double D0307 = dr["D0307"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307"].ToString());
                        D0307 = MathHelper.Round(D0307 / 10000, 2);
                        double D0307K = dr["D0307K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307K"].ToString());
                        D0307K = MathHelper.Round(D0307K / 10000, 2);
                        double d03 = MathHelper.Round(D0301 + D0301K + D0302 + D0302K + D0305 + D0307 + D0307K, 2);

                        double D0401 = dr["D0401"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0401"].ToString());
                        D0401 = MathHelper.Round(D0401 / 10000, 2);
                        double D0402 = dr["D0402"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0402"].ToString());
                        D0402 = MathHelper.Round(D0402 / 10000, 2);
                        double D0403 = dr["D0403"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403"].ToString());
                        D0403 = MathHelper.Round(D0403 / 10000, 2);
                        double D0403K = dr["D0403K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403K"].ToString());
                        D0403K = MathHelper.Round(D0403K / 10000, 2);
                        double D0404 = dr["D0404"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0404"].ToString());
                        D0404 = MathHelper.Round(D0404 / 10000, 2);
                        double d04 = MathHelper.Round(D0401 + D0403 + D0403K + D0404, 2);

                        double D05H1 = dr["D05H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D05H1"].ToString());
                        D05H1 = MathHelper.Round(D05H1 / 10000, 2);
                        double D0508 = dr["D0508"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0508"].ToString());
                        D0508 = MathHelper.Round(D0508 / 10000, 2);
                        double d05 = MathHelper.Round(D05H1 + D0508, 2);

                        double D0601 = dr["D0601"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0601"].ToString());
                        D0601 = MathHelper.Round(D0601 / 10000, 2);
                        double D0602 = dr["D0602"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0602"].ToString());
                        D0602 = MathHelper.Round(D0602 / 10000, 2);
                        double D0603 = dr["D0603"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0603"].ToString());
                        D0603 = MathHelper.Round(D0603 / 10000, 2);
                        double d06 = MathHelper.Round(D0601 + D0602, 2);

                        double D0701 = dr["D0701"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0701"].ToString());
                        D0701 = MathHelper.Round(D0701 / 10000, 2);
                        double D0702 = dr["D0702"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0702"].ToString());
                        D0702 = MathHelper.Round(D0702 / 10000, 2);
                        double D07 = MathHelper.Round(D0701 + D0702, 2);

                        double D08H1 = dr["D08H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H1"].ToString());
                        D08H1 = MathHelper.Round(D08H1 / 10000, 2);
                        double D08H2 = dr["D08H2"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2"].ToString());
                        D08H2 = MathHelper.Round(D08H2 / 10000, 2);
                        double D08H2A = dr["D08H2A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2A"].ToString());
                        D08H2A = MathHelper.Round(D08H2A / 10000, 2);
                        double D0810A = dr["D0810A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810A"].ToString());
                        D0810A = MathHelper.Round(D0810A / 10000, 2);
                        double D0810 = dr["D0810"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810"].ToString());
                        D0810 = MathHelper.Round(D0810 / 10000, 2);
                        double D0809 = dr["D0809"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0809"].ToString());
                        D0809 = MathHelper.Round(D0809 / 10000, 2);
                        double D08 = MathHelper.Round(D08H1 + D08H2 + D08H2A + D0809 + D0810 + D0810A, 2);
                        double D09 = dr["D09"].ToString().Trim() == "" ? 0 : double.Parse(dr["D09"].ToString());
                        D09 = MathHelper.Round(D09 / 10000, 2);

                        double D1001 = dr["D1001"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1001"].ToString());
                        D1001 = MathHelper.Round(D1001 / 10000, 2);
                        double D1002 = dr["D1002"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1002"].ToString());
                        D1002 = MathHelper.Round(D1002 / 10000, 2);
                        double D1003 = dr["D1003"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1003"].ToString());
                        D1003 = MathHelper.Round(D1003 / 10000, 2);
                        double D1004 = dr["D1004"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1004"].ToString());
                        D1004 = MathHelper.Round(D1004 / 10000, 2);
                        double D1005 = dr["D1005"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1005"].ToString());
                        D1005 = MathHelper.Round(D1005 / 10000, 2);
                        double D1006 = dr["D1006"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1006"].ToString());
                        D1006 = MathHelper.Round(D1006 / 10000, 2);
                        double D1007 = dr["D1007"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1007"].ToString());
                        D1007 = MathHelper.Round(D1007 / 10000, 2);
                        double D1008 = dr["D1008"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1008"].ToString());
                        D1008 = MathHelper.Round(D1008 / 10000, 2);
                        double D1009 = dr["D1009"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1009"].ToString());
                        D1009 = MathHelper.Round(D1009 / 10000, 2);
                        double D10 = MathHelper.Round(D1001 + D1002 + D1003 + D1004 + D1005 + D1006 + D1007 + D1008 + D1009, 2);

                        double D1101 = dr["D1101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1101"].ToString());
                        D1101 = MathHelper.Round(D1101 / 10000, 2);
                        double D1102 = dr["D1102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1102"].ToString());
                        D1102 = MathHelper.Round(D1102 / 10000, 2);
                        double D1103 = dr["D1103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1103"].ToString());
                        D1103 = MathHelper.Round(D1103 / 10000, 2);
                        double D1104 = dr["D1104"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104"].ToString());
                        D1104 = MathHelper.Round(D1104 / 10000, 2);
                        double D1104A = dr["D1104A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104A"].ToString());
                        D1104A = MathHelper.Round(D1104A / 10000, 2);
                        double D1104K = dr["D1104K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104K"].ToString());
                        D1104K = MathHelper.Round(D1104K / 10000, 2);
                        double D1105 = dr["D1105"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1105"].ToString());
                        D1105 = MathHelper.Round(D1105 / 10000, 2);
                        double D1106 = dr["D1106"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1106"].ToString());
                        D1106 = MathHelper.Round(D1106 / 10000, 2);
                        double D1107 = dr["D1107"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107"].ToString());
                        D1107 = MathHelper.Round(D1107 / 10000, 2);
                        double D1107A = dr["D1107A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107A"].ToString());
                        D1107A = MathHelper.Round(D1107A / 10000, 2);
                        double D1108 = dr["D1108"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1108"].ToString());
                        D1108 = MathHelper.Round(D1108 / 10000, 2);
                        double D1109 = dr["D1109"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1109"].ToString());
                        D1109 = MathHelper.Round(D1109 / 10000, 2);
                        double D1110 = dr["D1110"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1110"].ToString());
                        D1110 = MathHelper.Round(D1110 / 10000, 2);
                        double D11 = MathHelper.Round(D1101 + D1102 + D1103 + D1104 + D1104A + D1104K + D1107 + D1107A + D1109 + D1110, 2);

                        double D1201 = dr["D1201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1201"].ToString());
                        D1201 = MathHelper.Round(D1201 / 10000, 2);
                        double D1202 = dr["D1202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1202"].ToString());
                        D1202 = MathHelper.Round(D1202 / 10000, 2);
                        double D1203 = dr["D1203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1203"].ToString());
                        D1203 = MathHelper.Round(D1203 / 10000, 2);
                        double D1204 = dr["D1204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1204"].ToString());
                        D1204 = MathHelper.Round(D1204 / 10000, 2);
                        double D1205 = dr["D1205"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1205"].ToString());
                        D1205 = MathHelper.Round(D1205 / 10000, 2);
                        double D1206 = dr["D1206"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1206"].ToString());
                        D1206 = MathHelper.Round(D1206 / 10000, 2);
                        double D1207 = dr["D1207"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1207"].ToString());
                        D1207 = MathHelper.Round(D1207 / 10000, 2);
                        double D12 = MathHelper.Round(D1201 + D1202 + D1203 + D1204 + D1205 + D1206 + D1207, 2);

                        double D00 = MathHelper.Round(D0303 + D0304 + D0306 + D0402 + D0603 + D1105 + D1106 + D1108, 2);
                        double tdzmj = MathHelper.Round(D00 + D01 + D02 + d03 + d04 + d05 + d06 + D07 + D08 + D09 + D10 + D11 + D12, 2);

                        #endregion

                        StringBuilder sb = new StringBuilder();
                        sb.Append("insert into HZ_JCB(ZLDWDM,QSDWDM,QSXZ,GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,tdzmj,")
                        .Append("D00,D01, D0101,D0102,D0103,D02,D0201,D0201K,D0202,D0202K,D0203,D0203K,D0204,D0204K,D03, D0301, D0301K,D0302,D0302K,D0303,D0304, D0305,  D0306,  D0307,  D0307K,")
                        .Append("D04,D0401,D0402,D0403,D0403K,D0404,D05,D05H1, D0508,")
                        .Append(" D06,D0601,  D0602,  D0603, D07, D0701,  D0702, D08, D08H1,  D08H2,  D08H2A,  D0809,  D0810,  D0810A,  D09, ")
                        .Append("D10, D1001,  D1002,  D1003,  D1004,  D1005,  D1006,  D1007,  D1008,  D1009, ")
                        .Append("D11, D1101,  D1102,  D1103,  D1104,  D1104A,  D1104K,  D1105,  D1106,  D1107,  D1107A,  D1108,  D1109,D1110,")
                        .Append("D12, D1201,  D1202,  D1203,  D1204,  D1205,  D1206,D1207,OLDAREA )")
                        .Append("values ('").Append(dr["ZLDWDM"].ToString()).Append("','").Append(dr["QSDWDM"].ToString()).Append("','").Append(dr["QSXZ"].ToString()).Append("','")
                        .Append(dr["GDPDJB"].ToString()).Append("','").Append(dr["GDLX"].ToString()).Append("','").Append(dr["GDZZSXDM"].ToString()).Append("','")
                        .Append(dr["CZCSXM"].ToString()).Append("','").Append(dr["TBXHDM"].ToString()).Append("','").Append(dr["MSSM"].ToString()).Append("','")
                        .Append(dr["FRDBS"].ToString()).Append("',").Append(tdzmj).Append(",").Append(D00).Append(",").Append(D01).Append(",")
                        .Append(d0101).Append(",").Append(d0102).Append(",").Append(d0103).Append(",")
                        .Append(D02).Append(",").Append(D0201).Append(",").Append(D0201K).Append(",").Append(D0202).Append(",").Append(D0202K).Append(",").Append(D0203).Append(",").Append(D0203K).Append(",").Append(D0204).Append(",").Append(D0204K).Append(",")
                        .Append(d03).Append(",").Append(D0301).Append(",").Append(D0301K).Append(",").Append(D0302).Append(",").Append(D0302K).Append(",").Append(D0303).Append(",").Append(D0304).Append(",")
                        .Append(D0305).Append(",").Append(D0306).Append(",").Append(D0307).Append(",").Append(D0307K).Append(",")

                        .Append(d04).Append(",").Append(D0401).Append(",").Append(D0402).Append(",").Append(D0403).Append(",").Append(D0403K).Append(",").Append(D0404).Append(",")
                        .Append(d05).Append(",").Append(D05H1).Append(",").Append(D0508).Append(",")
                        .Append(d06).Append(",").Append(D0601).Append(",").Append(D0602).Append(",").Append(D0603).Append(",")
                        .Append(D07).Append(",").Append(D0701).Append(",").Append(D0702).Append(",")
                        .Append(D08).Append(",").Append(D08H1).Append(",").Append(D08H2).Append(",").Append(D08H2A).Append(",").Append(D0809).Append(",").Append(D0810).Append(",").Append(D0810A).Append(",")
                        .Append(D09).Append(",")
                        .Append(D10).Append(",").Append(D1001).Append(",").Append(D1002).Append(",").Append(D1003).Append(",").Append(D1004).Append(",").Append(D1005).Append(",").Append(D1006).Append(",").Append(D1007).Append(",")
                        .Append(D1008).Append(",").Append(D1009).Append(",")
                        .Append(D11).Append(",").Append(D1101).Append(",").Append(D1102).Append(",").Append(D1103).Append(",").Append(D1104).Append(",")
                        .Append(D1104A).Append(",").Append(D1104K).Append(",").Append(D1105).Append(",").Append(D1106).Append(",").Append(D1107).Append(",").Append(D1107A).Append(",")
                        .Append(D1108).Append(",").Append(D1109).Append(",").Append(D1110).Append(",")
                        .Append(D12).Append(",").Append(D1201).Append(",").Append(D1202).Append(",").Append(D1203).Append(",")
                        .Append(D1204).Append(",").Append(D1205).Append(",").Append(D1206).Append(",")
                        .Append(D1207).Append(",").Append(oldmj).Append(" ) ");
                        sql = sb.ToString();

                        iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                    }

                }

            }
            catch (Exception ex) { }

        }

        /// <summary>
        /// 把表里 的数据都转化为公顷
        /// </summary>
        /// <param name="sTable"></param>
        public void ChangeTableDW2GQ(string sTable)
        {
            string  sql = "";
            int iRet ;
            try
            {
                if (sTable == "HZ_JBNT_BH_JCB")
                {
                    sql = "update " + sTable + " set TDZMJ=round(TDZMJ/10000,2),D010=round(D010/10000,2),D011=round(D011/10000,2),D012=round(D012/10000,2),D013=round(D013/10000,2)" +
                               ",OLDAREA=round(OLDAREA/10000,2)";
                    iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                }
                else
                {

                    iRet= LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_JCB");

                    DataTable dt =LS_ResultMDBHelper.GetDataTable(  "select * from HZ_JCB_PFM " ,"tmp");
                    foreach (DataRow dr in dt.Rows)
                    {

                        #region 都要四舍五入

                        double oldmj = dr["OLDAREA"].ToString().Trim() == "" ? 0 : double.Parse(dr["OLDAREA"].ToString());
                        oldmj=MathHelper.Round(oldmj/10000,2);

                        double d0101 = dr["D0101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0101"].ToString());
                        d0101 = MathHelper.Round(d0101 / 10000, 2);
                        double d0102 = dr["D0102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0102"].ToString());
                        d0102 = MathHelper.Round(d0102 / 10000, 2);
                        double d0103 = dr["D0103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0103"].ToString());
                        d0103 = MathHelper.Round(d0103 / 10000, 2);
                        double D01 = MathHelper.Round(d0101 + d0102 + d0103, 2);

                        double D0201 = dr["D0201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201"].ToString());
                        D0201 = MathHelper.Round(D0201 / 10000, 2);
                        double D0202 = dr["D0202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202"].ToString());
                        D0202 = MathHelper.Round(D0202 / 10000, 2);
                        double D0201K = dr["D0201K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201K"].ToString());
                        D0201K = MathHelper.Round(D0201K / 10000, 2);
                        double D0202K = dr["D0202K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202K"].ToString());
                        D0202K = MathHelper.Round(D0202K / 10000, 2);
                        double D0203 = dr["D0203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203"].ToString());
                        D0203 = MathHelper.Round(D0203 / 10000, 2);
                        double D0203K = dr["D0203K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203K"].ToString());
                        D0203K = MathHelper.Round(D0203K / 10000, 2);
                        double D0204 = dr["D0204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204"].ToString());
                        D0204 = MathHelper.Round(D0204 / 10000, 2);
                        double D0204K = dr["D0204K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204K"].ToString());
                        D0204K = MathHelper.Round(D0204K / 10000, 2);
                        double D02 = MathHelper.Round(D0201 + D0201K + D0202 + D0202K + D0203 + D0203K + D0204 + D0204K, 2);

                        double D0301 = dr["D0301"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301"].ToString());
                        D0301 = MathHelper.Round(D0301 / 10000, 2);
                        double D0301K = dr["D0301K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301K"].ToString());
                        D0301K = MathHelper.Round(D0301K / 10000, 2);
                        double D0302 = dr["D0302"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302"].ToString());
                        D0302 = MathHelper.Round(D0302 / 10000, 2);
                        double D0302K = dr["D0302K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302K"].ToString());
                        D0302K = MathHelper.Round(D0302K / 10000, 2);
                        double D0303 = dr["D0303"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0303"].ToString());
                        D0303 = MathHelper.Round(D0303 / 10000, 2);
                        double D0304 = dr["D0304"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0304"].ToString());
                        D0304 = MathHelper.Round(D0304 / 10000, 2);
                        double D0305 = dr["D0305"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0305"].ToString());
                        D0305 = MathHelper.Round(D0305 / 10000, 2);
                        double D0306 = dr["D0306"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0306"].ToString());
                        D0306 = MathHelper.Round(D0306 / 10000, 2);
                        double D0307 = dr["D0307"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307"].ToString());
                        D0307 = MathHelper.Round(D0307 / 10000, 2);
                        double D0307K = dr["D0307K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307K"].ToString());
                        D0307K = MathHelper.Round(D0307K / 10000, 2);
                        double d03 = MathHelper.Round(D0301 + D0301K + D0302 + D0302K + D0305 + D0307 + D0307K, 2);

                        double D0401 = dr["D0401"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0401"].ToString());
                        D0401 = MathHelper.Round(D0401 / 10000, 2);
                        double D0402 = dr["D0402"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0402"].ToString());
                        D0402 = MathHelper.Round(D0402 / 10000, 2);
                        double D0403 = dr["D0403"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403"].ToString());
                        D0403 = MathHelper.Round(D0403 / 10000, 2);
                        double D0403K = dr["D0403K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403K"].ToString());
                        D0403K = MathHelper.Round(D0403K / 10000, 2);
                        double D0404 = dr["D0404"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0404"].ToString());
                        D0404 = MathHelper.Round(D0404 / 10000, 2);
                        double d04 = MathHelper.Round(D0401 + D0403 + D0403K + D0404,2);
                      
                        double D05H1 = dr["D05H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D05H1"].ToString());
                        D05H1 = MathHelper.Round(D05H1 / 10000, 2);
                        double D0508 = dr["D0508"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0508"].ToString());
                        D0508 = MathHelper.Round(D0508 / 10000, 2);
                        double d05 = MathHelper.Round(D05H1 + D0508, 2);                        
                        
                        double D0601 = dr["D0601"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0601"].ToString());
                        D0601 = MathHelper.Round(D0601 / 10000, 2);
                        double D0602 = dr["D0602"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0602"].ToString());
                        D0602 = MathHelper.Round(D0602 / 10000, 2);
                        double D0603 = dr["D0603"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0603"].ToString());
                        D0603 = MathHelper.Round(D0603 / 10000, 2);
                        double d06 = MathHelper.Round(D0601+D0602, 2);
                       
                        double D0701 = dr["D0701"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0701"].ToString());
                        D0701 = MathHelper.Round(D0701 / 10000, 2);
                        double D0702 = dr["D0702"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0702"].ToString());
                        D0702 = MathHelper.Round(D0702 / 10000, 2);
                        double D07 = MathHelper.Round(D0701 + D0702, 2);
                        
                        double D08H1 = dr["D08H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H1"].ToString());
                        D08H1 = MathHelper.Round(D08H1 / 10000, 2);
                        double D08H2 = dr["D08H2"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2"].ToString());
                        D08H2 = MathHelper.Round(D08H2 / 10000, 2);
                        double D08H2A = dr["D08H2A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2A"].ToString());
                        D08H2A = MathHelper.Round(D08H2A / 10000, 2);
                        double D0810A = dr["D0810A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810A"].ToString());
                        D0810A = MathHelper.Round(D0810A / 10000, 2);
                        double D0810 = dr["D0810"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810"].ToString());
                        D0810 = MathHelper.Round(D0810 / 10000, 2);
                        double D0809 = dr["D0809"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0809"].ToString());
                        D0809 = MathHelper.Round(D0809 / 10000, 2);
                        double D08 = MathHelper.Round(D08H1 + D08H2 + D08H2A + D0809 + D0810 + D0810A, 2);
                        double D09 = dr["D09"].ToString().Trim() == "" ? 0 : double.Parse(dr["D09"].ToString());
                        D09 = MathHelper.Round(D09 / 10000, 2);
                       
                        double D1001 = dr["D1001"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1001"].ToString());
                        D1001 = MathHelper.Round(D1001 / 10000, 2);
                        double D1002 = dr["D1002"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1002"].ToString());
                        D1002 = MathHelper.Round(D1002 / 10000, 2);
                        double D1003 = dr["D1003"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1003"].ToString());
                        D1003 = MathHelper.Round(D1003 / 10000, 2);
                        double D1004 = dr["D1004"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1004"].ToString());
                        D1004 = MathHelper.Round(D1004 / 10000, 2);
                        double D1005 = dr["D1005"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1005"].ToString());
                        D1005 = MathHelper.Round(D1005 / 10000, 2);
                        double D1006 = dr["D1006"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1006"].ToString());
                        D1006 = MathHelper.Round(D1006 / 10000, 2);
                        double D1007 = dr["D1007"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1007"].ToString());
                        D1007 = MathHelper.Round(D1007 / 10000, 2);
                        double D1008 = dr["D1008"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1008"].ToString());
                        D1008 = MathHelper.Round(D1008 / 10000, 2);
                        double D1009 = dr["D1009"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1009"].ToString());
                        D1009 = MathHelper.Round(D1009 / 10000, 2);
                        double D10 = MathHelper.Round(D1001 + D1002 + D1003 + D1004 + D1005 + D1006 + D1007 + D1008 + D1009, 2);

                        double D1101 = dr["D1101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1101"].ToString());
                        D1101 = MathHelper.Round(D1101 / 10000, 2);
                        double D1102 = dr["D1102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1102"].ToString());
                        D1102 = MathHelper.Round(D1102 / 10000, 2);
                        double D1103 = dr["D1103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1103"].ToString());
                        D1103 = MathHelper.Round(D1103 / 10000, 2);
                        double D1104 = dr["D1104"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104"].ToString());
                        D1104 = MathHelper.Round(D1104 / 10000, 2);
                        double D1104A = dr["D1104A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104A"].ToString());
                        D1104A = MathHelper.Round(D1104A / 10000, 2);
                        double D1104K = dr["D1104K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104K"].ToString());
                        D1104K = MathHelper.Round(D1104K / 10000, 2);
                        double D1105 = dr["D1105"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1105"].ToString());
                        D1105 = MathHelper.Round(D1105 / 10000, 2);
                        double D1106 = dr["D1106"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1106"].ToString());
                        D1106 = MathHelper.Round(D1106 / 10000, 2);
                        double D1107 = dr["D1107"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107"].ToString());
                        D1107 = MathHelper.Round(D1107 / 10000, 2);
                        double D1107A = dr["D1107A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107A"].ToString());
                        D1107A = MathHelper.Round(D1107A / 10000, 2);
                        double D1108 = dr["D1108"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1108"].ToString());
                        D1108 = MathHelper.Round(D1108 / 10000, 2);
                        double D1109 = dr["D1109"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1109"].ToString());
                        D1109 = MathHelper.Round(D1109 / 10000, 2);
                        double D1110 = dr["D1110"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1110"].ToString());
                        D1110 = MathHelper.Round(D1110 / 10000, 2);
                        double D11 = MathHelper.Round(D1101 + D1102 + D1103 + D1104 + D1104A + D1104K + D1107 + D1107A + D1109 + D1110, 2);
                       
                        double D1201 = dr["D1201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1201"].ToString());
                        D1201 = MathHelper.Round(D1201 / 10000, 2);
                        double D1202 = dr["D1202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1202"].ToString());
                        D1202 = MathHelper.Round(D1202 / 10000, 2);
                        double D1203 = dr["D1203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1203"].ToString());
                        D1203 = MathHelper.Round(D1203 / 10000, 2);
                        double D1204 = dr["D1204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1204"].ToString());
                        D1204 = MathHelper.Round(D1204 / 10000, 2);
                        double D1205 = dr["D1205"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1205"].ToString());
                        D1205 = MathHelper.Round(D1205 / 10000, 2);
                        double D1206 = dr["D1206"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1206"].ToString());
                        D1206 = MathHelper.Round(D1206 / 10000, 2);
                        double D1207 = dr["D1207"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1207"].ToString());
                        D1207 = MathHelper.Round(D1207 / 10000, 2);
                        double D12 = MathHelper.Round(D1201 + D1202 + D1203 + D1204 + D1205 + D1206 + D1207, 2);

                        double D00 = MathHelper.Round(D0303 + D0304 + D0306 + D0402 + D0603 + D1105 + D1106 + D1108, 2);
                       // double tdzmj = MathHelper.Round(D00 + D01 + D02 + d03 + d04 + d05 + d06 + D07 + D08 + D09 + D10 + D11 + D12, 2);

                        double tdzmj = dr["TDZMJ"].ToString().Trim() == "" ? 0 : double.Parse(dr["TDZMJ"].ToString());
                        tdzmj = MathHelper.Round(tdzmj/10000, 2);
                        #endregion 

                        StringBuilder sb = new StringBuilder();
                        sb.Append("insert into HZ_JCB(ZLDWDM,QSDWDM,QSXZ,GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,tdzmj,")
                        .Append("D00,D01, D0101,D0102,D0103,D02,D0201,D0201K,D0202,D0202K,D0203,D0203K,D0204,D0204K,D03, D0301, D0301K,D0302,D0302K,D0303,D0304, D0305,  D0306,  D0307,  D0307K,")
                        .Append("D04,D0401,D0402,D0403,D0403K,D0404,D05,D05H1, D0508,")
                        .Append(" D06,D0601,  D0602,  D0603, D07, D0701,  D0702, D08, D08H1,  D08H2,  D08H2A,  D0809,  D0810,  D0810A,  D09, ")
                        .Append("D10, D1001,  D1002,  D1003,  D1004,  D1005,  D1006,  D1007,  D1008,  D1009, ")
                        .Append("D11, D1101,  D1102,  D1103,  D1104,  D1104A,  D1104K,  D1105,  D1106,  D1107,  D1107A,  D1108,  D1109,D1110,")
                        .Append("D12, D1201,  D1202,  D1203,  D1204,  D1205,  D1206,D1207,OLDAREA )")
                        .Append("values ('").Append(dr["ZLDWDM"].ToString()).Append("','").Append(dr["QSDWDM"].ToString()).Append("','").Append(dr["QSXZ"].ToString()).Append("','")
                        .Append(dr["GDPDJB"].ToString()).Append("','").Append(dr["GDLX"].ToString()).Append("','").Append(dr["GDZZSXDM"].ToString()).Append("','")
                        .Append(dr["CZCSXM"].ToString()).Append("','").Append(dr["TBXHDM"].ToString()).Append("','").Append(dr["MSSM"].ToString()).Append("','")
                        .Append(dr["FRDBS"].ToString()).Append("',").Append(tdzmj).Append(",").Append(D00).Append(",").Append(D01).Append(",")
                        .Append(d0101).Append(",").Append(d0102).Append(",").Append(d0103).Append(",")
                        .Append(D02).Append(",").Append(D0201).Append(",").Append(D0201K).Append(",").Append(D0202).Append(",").Append(D0202K).Append(",").Append(D0203).Append(",").Append(D0203K).Append(",").Append(D0204).Append(",").Append(D0204K).Append(",")
                        .Append(d03).Append(",").Append(D0301).Append(",").Append(D0301K).Append(",").Append(D0302).Append(",").Append(D0302K).Append(",").Append(D0303).Append(",").Append(D0304).Append(",")
                        .Append(D0305).Append(",") .Append(D0306).Append(",").Append(D0307).Append(",").Append(D0307K).Append(",")

                        .Append(d04).Append(",").Append(D0401).Append(",").Append(D0402).Append(",").Append(D0403).Append(",").Append(D0403K).Append(",").Append(D0404).Append(",")
                        .Append(d05).Append(",").Append(D05H1).Append(",").Append(D0508).Append(",")
                        .Append(d06).Append(",").Append(D0601).Append(",").Append(D0602).Append(",").Append(D0603).Append(",")
                        .Append(D07).Append(",").Append(D0701).Append(",").Append(D0702).Append(",")
                        .Append(D08).Append(",").Append(D08H1).Append(",").Append(D08H2).Append(",").Append(D08H2A).Append(",").Append(D0809).Append(",").Append(D0810).Append(",").Append(D0810A).Append(",")
                        .Append(D09).Append(",") 
                        .Append(D10).Append(",").Append(D1001).Append(",").Append(D1002 ).Append(",").Append(D1003).Append(",").Append(D1004 ).Append(",").Append(D1005 ).Append(",").Append(D1006).Append(",").Append(D1007).Append(",")
                        .Append(D1008).Append(",").Append(D1009).Append(",")
                        .Append(D11).Append(",").Append(D1101).Append(",").Append(D1102).Append(",").Append(D1103).Append(",").Append(D1104).Append(",")
                        .Append(D1104A ).Append(",").Append(D1104K).Append(",").Append(D1105).Append(",").Append(D1106).Append(",").Append(D1107).Append(",").Append(D1107A).Append(",")
                        .Append(D1108).Append(",").Append(D1109).Append(",").Append(D1110).Append(",")
                        .Append(D12).Append(",").Append(D1201).Append(",").Append(D1202).Append(",").Append(D1203).Append(",")
                        .Append(D1204).Append(",").Append(D1205).Append(",").Append(D1206).Append(",")
                        .Append(D1207).Append(",").Append(oldmj).Append(" ) ");
                        sql = sb.ToString();
                       
                        iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                    }                  
                  
                }
                
            }
            catch (Exception ex) { }

        }
        

        /// <summary>
        /// 获得单位个数
        /// </summary>
        /// <param name="sTable"></param>
        /// <returns></returns>
        private int GetXZDWNum(string sTable)
        {
            try
            {
                string sql = "select distinct  zldwdm   from " + sTable + "  ";
                DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
                return dt.Rows.Count;
            }
            catch { return 0; }
        }
        
        
        /// <summary>
        /// 获得调平多的单位列表
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sYear"></param>
        /// <param name="sTable"></param>
        /// <param name="sRemainder"></param>
        /// <returns></returns>
        private List<string> GetTPDWList(string sTable, int sRemainder, int skipNum = 0, string mssm = "00")
        {
            //获得单位个数是总面积的大小排序
            try
            {
                string sql = "select sum(TDZMJ),ZLDWDM  from " + sTable +
                    " Where MSSM = '" + mssm + "' group by ZLDWDM order by sum(TDZMJ) desc";
                //sql = "select *  from (" + sql + ") where rownum<=" + sRemainder;

                List<string> pList = new List<string>();
                DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
                int icount = 0;
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (icount == sRemainder + skipNum)
                            break;
                        if (icount < skipNum)
                        {
                            icount++;
                            continue;
                        }
                        pList.Add(dr["ZLDWDM"].ToString());
                        icount++;
                    }
                    return pList;
                }
                return null;
            }
            catch { return null; }

        }
      
        #region 对需要调平的单位进行调平总面积
        /// <summary>
        /// 对需要调平的单位进行调平总面积
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sYear"></param>
        /// <param name="sTable"></param>
        /// <param name="sXZQDM"></param>需要调平的行政区代码
        /// <param name="sTPArea"></param>需要调平的面积
        private void MakeBalance2Area(string sTable,string sXZQDM, double sTPArea, string smssm)
        {
            try
            {
               
                ///面积小于等于1 的不参与调平
                string sql = "select Count(*)  from " + sTable +
                    " where  ZLDWDM='" + sXZQDM + "' And MSSM = '" + smssm + "' ";
                //本村的单位个数
                DataRow onlyRow = LS_ResultMDBHelper.GetDataRow(sql, "tmp");
                if (onlyRow == null) return;
                int XZQDWNum = String2Int(onlyRow[0].ToString());
                if (XZQDWNum == 0) return;
                //本村的调平数目
                double sTPNum = sTPArea / 0.01;
                int sQuotient;//商值
                sQuotient = Double2Int(Math.Truncate(sTPNum / XZQDWNum));

                int sRemainder;//余数
                sRemainder = Double2Int(Math.IEEERemainder(sTPNum, String2Double(XZQDWNum.ToString())));
                if (sRemainder < 0)
                    sRemainder = XZQDWNum + sRemainder;
                //大的调平面积
                double sMaxTPArea = (sQuotient + 1) * 0.01;

                //小的调平面积
                double sMinTPArea = sQuotient * 0.01;

                if (this.isNegative == true)
                {
                    sMaxTPArea = (-sMaxTPArea);
                    sMinTPArea = (-sMinTPArea);
                }
                int icount = 0;
                sql = "select * from " + sTable + " where ZLDWDM='" + sXZQDM + "'  And MSSM = '" + smssm + "' order by TDZMJ desc ";
                DataTable tpDt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
                foreach (DataRow aRow in tpDt.Rows)
                {

                    string zldwdm = aRow["ZLDWDM"].ToString().Trim();
                    string qsdwdm = aRow["QSDWDM"].ToString().Trim();
                    string qsxz = aRow["QSXZ"].ToString().Trim();
                    string gdpdj = aRow["GDPDJB"].ToString().Trim();
                    string gdlx = aRow["GDLX"].ToString().Trim();
                    string gdzzsx = aRow["GDZZSXDM"].ToString().Trim();
                    string czcsxm = aRow["CZCSXM"].ToString().Trim();
                    string tbxhdm = aRow["TBXHDM"].ToString().Trim();
                    string mssm = aRow["MSSM"].ToString().Trim();
                    string frdbs = aRow["FRDBS"].ToString().Trim();
                    double tdzmj = 0;
                    double.TryParse(aRow["TDZMJ"].ToString(), out tdzmj);

                    string whereClause = " ZLDWDM='" + zldwdm + "' and QSDWDM='" + qsdwdm + "' and QSXZ='" + qsxz + "' and GDPDJB='" + gdpdj + "' and GDLX='" + gdlx + "' "
                           + " and GDZZSXDM='" + gdzzsx + "' and CZCSXM='" + czcsxm + "' and TBXHDM='" + tbxhdm + "' and MSSM='" + mssm + "' and FRDBS='" + frdbs + "' ";

                    if (icount >= sRemainder)
                    {
                        if (sMinTPArea == 0)
                            break;
                        tdzmj = MathHelper.Round(tdzmj + sMinTPArea, 2);
                       // sql = "update " + sTable + " set TDZMJ=tdzmj+" + sMinTPArea + " where " + whereClause;
                        sql = "update " + sTable + " set TDZMJ=" + tdzmj + " where " + whereClause;
                        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                        
                    }
                    else
                    {                        
                        //sql = "update " + sTable + " set TDZMJ=tdzmj+(" + sMaxTPArea + ") where " + whereClause;
                        tdzmj = MathHelper.Round(tdzmj + sMaxTPArea, 2);
                        sql = "update " + sTable + " set TDZMJ=" + tdzmj + " where " + whereClause;
                        int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                    }

                    icount++;
                }

            }
            catch { }
        }
        #endregion
        

        //获得基础统计表和基础汇总表的差值 判断调平数的正负号
        private bool isNegative = false;
        public void MakeBalance(string mssm,double kzmj)
        {
            string sTablePfm = "HZ_JCB_PFM";
            string sTable = "HZ_JCB";

            string sql = "select * from " + sTable + " Where mssm = '" + mssm + "'";
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "temp");
            if (dt.Rows.Count == 0) return;

            //double dXzqKzmj = 0;
            //dXzqKzmj = GetXQKZMJLDHD(sTablePfm, true, mssm);  //平方米面积和 转公顷

            double dXzqKzmj = kzmj;


            if (dXzqKzmj == 0) return;

            //ChangeTableDW2GQ(sTable);  //转化为公顷

            //计算下去汇总面积 换算为公顷的和
            double dXQHzmj = GetXQKZMJ(sTable, false, mssm);  //GetXQHZMJ(sTable);
            //double dXQHzmj = kzmj;  //GetXQHZMJ(sTable);

            if (dXQHzmj == 0) return;

            if (dXzqKzmj==dXQHzmj)
            {
                //如果纵向已经调平，看横向的
                DLTP(sTable);
            }
            else
            {
                //纵横都不平
                #region//纵横都不平
                double sTPS = 0.00;//调平数            
                sTPS = Math.Round(dXzqKzmj - dXQHzmj, 2);
                if (sTPS < 0)
                {
                    isNegative = true;  //是付的
                }
                else
                {
                    isNegative = false;
                }
                sTPS = MathHelper.RoundEx(sTPS, 2);
                sTPS = Math.Abs(sTPS);
                double sTPNum = 0;//调平数目
                sTPNum =MathHelper.Round( sTPS / 0.01,0);


                int sNum = 0;//乡镇、村子个数
                sNum = GetXZDWNum(sTable);
                int sQuotient;//商值
                sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((sTPNum / sNum))));

                int sRemainder;//余数
                sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(sTPNum), String2Double(sNum.ToString())));

                if (sRemainder < 0)
                    sRemainder = sNum + sRemainder;

                double sTPMaxArea;//调平大面积
                sTPMaxArea = (sQuotient + 1) * 0.01;
                List<string> pMaxList = GetTPDWList(sTable, sRemainder, 0, mssm);
                string sStrXZDWDM;
                if (pMaxList != null)
                {
                    if (pMaxList.Count != 0)
                    {

                       
                        for (int i = 0; i < pMaxList.Count; i++)
                        {
                            sStrXZDWDM = pMaxList[i];
                            MakeBalance2Area( sTable, sStrXZDWDM, sTPMaxArea, mssm);
                        }
                    }
                }

                double sTPMinArea;//调平小面积
                sTPMinArea = sQuotient * 0.01;
                if (sTPMinArea != 0)
                {
                    List<string> pMinList = GetTPDWList(sTable, (int)sTPNum - sRemainder, sRemainder, mssm);
                    if (pMinList != null)
                    {
                        if (pMinList.Count != 0)
                        {

                            for (int i = 0; i < pMinList.Count; i++)
                            {
                                sStrXZDWDM = pMinList[i];
                                MakeBalance2Area( sTable, sStrXZDWDM, sTPMinArea, mssm);
                            }
                        }
                    }
                }
                //调平地类
                DLTP(sTable);
                #endregion
            }
        }


       

    }

    public class TPItem : IComparable
    {
        public string dldm = "";
        public double dlmj = 0;
        public List<string> lstDlbms = new List<string>();
        public TPItem(string _dm, double _mj,List<string> dls)
        {
            this.dldm = _dm;
            this.dlmj = _mj;
            this.lstDlbms = dls;
        }

        /// <summary>
         /// 实现接口中的方法
       /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public int CompareTo(Object obj)
         {
             TPItem I2 = (TPItem)obj;
            
             //因为int32实现了接口IComparable，那么int也有CompareTo方法，直接调用该方法就行
             //从大到小排序
             if (I2.dlmj > this.dlmj)
             {
                 return 1;
             }
             else if (I2.dlmj < this.dlmj)
             {
                 return -1;
             }
             else
             {
                 //如果相等
                 int idx1 = lstDlbms.IndexOf(this.dldm);
                 int idx2 = lstDlbms.IndexOf(I2.dldm);
                 if (idx1 < idx2)
                 {
                     return -1;
                 }
                 else
                 {
                     return 1;
                 }
             }

         }

    }

   
    //统一时点更新报表输出-基础表公顷转换为平方米调平
    public class TPJCB
    {

        public TPJCB()
        {
            SetDlbm();
        }

        #region 字符串转换为整数
        private int String2Int(string str)
        {
            try
            {
                if (str == "") return 0;
                int a = 0;
                int.TryParse(str, out a);
                return a;
            }
            catch { return 0; }

        }
        #endregion
        
        #region 浮点型转换为整数
        private int Double2Int(double str)
        {
            try
            {
                if (str == 0.00) return 0;
                int a = 0;
                int.TryParse(str.ToString(), out a);
                return a;
            }
            catch { return 0; }

        }
        #endregion

        private double String2Double(string str)
        {
            try
            {
                if (str == "") return 0.00;
                double a = 0.00;
                double.TryParse(str, out a);
                return a;
            }
            catch { return 0.00; }

        }

       
        //所有地类编码
        private List<string> listDLBM = new List<string>();
        private void SetDlbm()
        {
            listDLBM = new List<string>();
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select DM from 三调工作分类 order by SORT", "FL");
            foreach (DataRow dr in dt.Rows)
            {
                listDLBM.Add("D"+dr["DM"].ToString().Trim());
            }
            
        }
     
        
        


       
        /// <summary>
        /// 取辖区 总面积 
        /// </summary>
        /// <param name="sTable"></param>
        /// <param name="isGq">是否转化为公顷</param>
        /// <returns></returns>
        private double GetXQKZMJ(string sTable,bool isGq, string mssm)
        {
            double mj = 0;
            string sql = "select sum(BGMJ) from " + sTable + "";
            DataRow dr = LS_TysdMDBHelper.GetDataRow(sql, "tmp");
            if (dr == null)
                return 0;
            string smj = dr[0].ToString();
            double.TryParse(smj, out mj);
            if (isGq)
            {
                //辖区控制总面积  换算成公顷
                mj = mj / 10000;
                return MathHelper.RoundEx(mj, 2);
            }
            else
            {
                return mj;
            }

        }

        private double GetXQKZMJLDHD(string sTable, bool isGq,string mssm)
        {
            double zmj = 0;
            string sql = "select sum(BGMJ) from " + sTable + "";
            DataTable dt = LS_TysdMDBHelper.GetDataTable(sql, "tmp");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                double mj = 0;
                string smj = dt.Rows[i][0].ToString();
                double.TryParse(smj, out mj);
                if (isGq)
                {
                    //辖区控制总面积  换算成公顷
                    mj = mj / 10000;
                    zmj += MathHelper.RoundEx(mj, 2);
                }
                else
                {
                    zmj += mj;
                }
            }
            return zmj;

        }
       

        
        #region 对横向地类进行调平

        private void DLTP2(string sTable)
        {
            string sql = "";
            #region  //如果纵向已经平了之后，直接看横向的
            if (sTable == "BG_JCB")
            {
                StringBuilder sb=new StringBuilder();
                sb.Append("select  ZLDWDM,QSDWDM,QSXZ, GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM, FRDBS,TDZMJ,")
                    .Append("iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
                    .Append("iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K)+iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K)+iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K)+iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K)+")
                    .Append("iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K)+iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K)+")
                    .Append("iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K)+iif(isnull(D0404),0,D0404)+")
                    .Append("iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+")
                    .Append("iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+")
                    .Append("iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A)+")
                    .Append("iif(isnull(D09),0,D09)+")
                    .Append("iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
                    .Append("iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                    .Append("iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
                    .Append("iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) as MJ ")
                  .Append(" from ").Append(sTable).Append(" ")
                    .Append(" order by TDZMJ desc ");
                sql = sb.ToString();
            }
            else if (sTable == "HZ_JBNT_JCB")
            {
                
            }
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
            double TDZMJ = 0.00;//总面积
            double MJ = 0.00;//各二级地类之和
            double dTPS = 0.00;//两者差值  调平数
            double   dTPNum = 0;//调平数目
           
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string zldwdm = dr["ZLDWDM"].ToString().Trim();
                    string qsdwdm = dr["QSDWDM"].ToString().Trim();
                    string qsxz = dr["QSXZ"].ToString().Trim();
                    string gdpdj = dr["GDPDJB"].ToString().Trim();
                    string gdlx = dr["GDLX"].ToString().Trim();
                    string gdzzsx = dr["GDZZSXDM"].ToString().Trim();
                    string czcsxm = dr["CZCSXM"].ToString().Trim();
                    string tbxhdm = dr["TBXHDM"].ToString().Trim();
                    string mssm = dr["MSSM"].ToString().Trim();
                    string frdbs = dr["FRDBS"].ToString().Trim();

                    string whereClause = " ZLDWDM='" + zldwdm + "' and QSDWDM='" + qsdwdm + "' and QSXZ='" + qsxz + "' and GDPDJB='" + gdpdj + "' and GDLX='"
                        + gdlx + "' and GDZZSXDM='" + gdzzsx + "' and CZCSXM='" + czcsxm + "' and TBXHDM='" + tbxhdm + "' and MSSM='" + mssm + "' and FRDBS='"+frdbs+"' ";

                    TDZMJ = String2Double(dr["TDZMJ"].ToString());
                   

                    MJ = String2Double(dr["MJ"].ToString());
                    dTPS =MathHelper.Round( TDZMJ - MJ,2);
                    if (dTPS == 0) continue;


                    bool isNav = false;
                    if (dTPS > 0)
                    {
                        isNav = false; //总面积比合计面积大
                    }
                    else
                    {
                        isNav = true; // 比合计面积小 
                    }
                    
                    dTPS = Math.Abs(dTPS);//调平数
                    dTPNum =MathHelper.Round( dTPS / 0.01,0);
                    //放入面积小于等于1公顷
                    List<string> pMaxDLMC = new List<string>();
                    List<double> pMaxDLMJ = new List<double>();

                    //放入面积大于1公顷
                    List<string> pMinDLMC = new List<string>();
                    List<double> pMinDLMJ = new List<double>();

                    #region 获得有值的二级地类和面积
                    sql = "select * from BG_JCB_PFM  where " + whereClause;  //这个 需要从原来平方米的 数量里面查询
                    double s2DLMJ = 0.00;
                    string s2DL = "";
                    //按照顺序进行插入列表
                    //后面取值进行倒序排列 从最后一行往前取值
                    DataRow ddr = LS_TysdMDBHelper.GetDataRow(sql, "tmp");
                    
                    for (int index = 0; index < this.listDLBM.Count; index++)
                    {
                        s2DLMJ = String2Double(ddr[listDLBM[index]].ToString());
                        s2DL = listDLBM[index];

                        if (s2DLMJ > 0)
                        {
                           // if (s2DLMJ > 1)
                            if (s2DLMJ > 10000)  //大于10000平米
                            {
                                pMaxDLMC.Add(s2DL);
                                pMaxDLMJ.Add(s2DLMJ);
                            }
                            //else if ((s2DLMJ <= 1) && (s2DLMJ > 0))
                            else if ((s2DLMJ <= 10000) && (s2DLMJ > 0))
                            {
                                pMinDLMC.Add(s2DL);
                                pMinDLMJ.Add(s2DLMJ);
                            }
                        }
                    }
                    #endregion

                  
                   

                    //对获得大于1公顷的处理
                    if (pMaxDLMC.Count > 0)
                    {
                        
                        #region >1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMaxDLMC.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMaxDLMC.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMaxDLMC.Count + sRemainder;
                       
                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMaxDLMC.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = pMaxDLMC.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMaxDLMJ[j] / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);
                                    sql = "update " + sTable + " set " + pMaxDLMC[j] + "="+ tpmj+" where "+whereClause;
                                    int iresult = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {
                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = pMaxDLMC.Count - 1; j >= 0; j--)
                            {
                                sMax = (sQuotient + 1) * 0.01;
                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }
                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxDLMJ[j] / 10000, 2) + sMax;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                        sql = "update " + sTable + " set " + pMaxDLMC[j] + "=" +gqMj+" where "+whereClause;
                                        int result = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxDLMJ[j] / 10000, 2) + sMin;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                       // sql = "update " + sTable + " set " + pMaxDLMC[j] + "=" + pMaxDLMJ[j] + "+(" + sMin + ") where "+whereClause;
                                        sql = "update " + sTable + " set " + pMaxDLMC[j] + "="+ gqMj+"   where " + whereClause;
                                        LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                        #endregion
                    }
                    else if ((pMaxDLMC.Count == 0) && (pMinDLMC.Count > 0))
                    {//如果一行都是小于等于1公顷也是同样的处理
                        #region <1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMinDLMC.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMinDLMC.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMinDLMC.Count + sRemainder;

                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMinDLMC.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = pMinDLMC.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMinDLMJ[j] / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);

                                    sql = "update " + sTable + " set " + pMinDLMC[j] + "=" +tpmj+ " where " + whereClause;
                                    int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = pMinDLMC.Count - 1; j >= 0; j--)
                            {
                                sMax = (sQuotient + 1) * 0.01;

                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double tpmj = MathHelper.Round(pMinDLMJ[j] / 10000, 2) + sMax;
                                        tpmj = MathHelper.Round(tpmj, 2);
                                        sql = "update " + sTable + " set " + pMinDLMC[j] + "=" +tpmj+"  where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double tpmj= MathHelper.Round( pMinDLMJ[j] /10000,2)+sMin;
                                        tpmj=MathHelper.Round(tpmj,2);
                                        sql = "update " + sTable + " set " + pMinDLMC[j] + "=" + tpmj + " where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //降序加0.01
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / this.listDLBM.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(this.listDLBM.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = this.listDLBM.Count + sRemainder;
                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / this.listDLBM.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {
                                    
                                    sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sTPMJ + ") where " + whereClause;
                                    int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {
                                sMax = (sQuotient + 1) * 0.01;
                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMax + ") where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMin + ") where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }                
                
            }

                              

            //从新整理一下经过变更后的列，使其二级地类之和等于一级地类
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("update HZ_JCB set D0201HJ=iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K),")
                .Append(" D0202HJ=iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K),")
                .Append(" D0203HJ=iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K),")
                .Append(" D0204HJ=iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K),")
                .Append(" D0301HJ=iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K),")
                .Append(" D0302HJ=iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K),")
                .Append(" D0307HJ=iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K),")
                .Append(" D0403HJ=iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K),")
                .Append(" D08H2HJ=iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A),")
                .Append(" D0810HJ=iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A),")
                .Append(" D1104HJ=iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K),")
                .Append(" D1107HJ=iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A) ");
            sql = sb2.ToString();
            int iret = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);

            sb2.Clear();
            sb2.Append("update HZ_JCB set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
            .Append(" D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                .Append("D02=iif(isnull(D0201HJ),0,D0201HJ)+iif(isnull(D0202HJ),0,D0202HJ)+iif(isnull(D0203HJ),0,D0203HJ)+iif(isnull(D0204HJ),0,D0204HJ),")
                .Append("D03=iif(isnull(D0301HJ),0,D0301HJ)+iif(isnull(D0302HJ),0,D0302HJ)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307HJ),0,D0307HJ),")
                .Append("D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403HJ),0,D0403HJ)+iif(isnull(D0404),0,D0404),")
                .Append("D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append("D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                .Append("D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2HJ),0,D08H2HJ)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810HJ),0,D0810HJ),")
                .Append("D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append("D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                .Append("iif(isnull(D1104HJ),0,D1104HJ)+iif(isnull(D1107HJ),0,D1107HJ)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                .Append("D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207)  ");
            sql = sb2.ToString();
            iret = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
            #endregion
        }


        private void DLTP(string sTable)
        {
            string sql = "";
            #region 获取不相等的
            if (sTable == "BG_JCB")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select  BGMJ,BGQDLBM, BGQQSXZ, BGQZLDWDM, BGQGDLX, BGQGDPDJB, BGQTBXHDM, BGQZZSXDM, BGQCZCSXM, BGQMSSM,BGHDLBM,BGHQSXZ, BGHZLDWDM, BGHGDLX, BGHGDPDJB, BGHTBXHDM, BGHZZSXDM, BGHCZCSXM, BGHMSSM,")
                    .Append("iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
                    .Append("iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K)+iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K)+iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K)+iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K)+")
                    .Append("iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K)+iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K)+")
                    .Append("iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K)+iif(isnull(D0404),0,D0404)+")
                    .Append("iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+")
                    .Append("iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+")
                    .Append("iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A)+")
                    .Append("iif(isnull(D09),0,D09)+")
                    .Append("iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
                    .Append("iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                    .Append("iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
                    .Append("iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) as MJ ")
                  .Append(" from ").Append(sTable).Append(" ")
                    .Append(" order by BGMJ desc ");
                sql = sb.ToString();
            }
           
            DataTable dt = LS_TysdMDBHelper.GetDataTable(sql, "tmp");
            double TDZMJ = 0.00;//总面积
            double MJ = 0.00;//各二级地类之和
            double dTPS = 0.00;//两者差值  调平数
            double dTPNum = 0;//调平数目
            #endregion 

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    string BGQDLBM = dr["BGQDLBM"].ToString().Trim();
                    string BGQQSXZ = dr["BGQQSXZ"].ToString().Trim();
                    string BGQZLDWDM = dr["BGQZLDWDM"].ToString().Trim();
                    string BGQGDLX = dr["BGQGDLX"].ToString().Trim();
                    string BGQGDPDJB = dr["BGQGDPDJB"].ToString().Trim();
                    string BGQTBXHDM = dr["BGQTBXHDM"].ToString().Trim();
                    string BGQZZSXDM = dr["BGQZZSXDM"].ToString().Trim();
                    string BGQCZCSXM = dr["BGQCZCSXM"].ToString().Trim();
                    string BGQMSSM = dr["BGQMSSM"].ToString().Trim();
                    string BGHDLBM = dr["BGHDLBM"].ToString().Trim();
                    string BGHQSXZ = dr["BGHQSXZ"].ToString().Trim();
                    string BGHZLDWDM = dr["BGHZLDWDM"].ToString().Trim();
                    string BGHGDLX = dr["BGHGDLX"].ToString().Trim();
                    string BGHGDPDJB = dr["BGHGDPDJB"].ToString().Trim();
                    string BGHTBXHDM = dr["BGHTBXHDM"].ToString().Trim();
                    string BGHZZSXDM = dr["BGHZZSXDM"].ToString().Trim();
                    string BGHCZCSXM = dr["BGHCZCSXM"].ToString().Trim();
                    string BGHMSSM = dr["BGHMSSM"].ToString().Trim();

                    string whereClause = " BGQDLBM='" + BGQDLBM + "' and BGQQSXZ='" + BGQQSXZ + "' and BGQZLDWDM='" + BGQZLDWDM + "' and BGQGDLX='" + BGQGDLX + "' and BGQGDPDJB='"
                        + BGQGDPDJB + "' and BGQTBXHDM='" + BGQTBXHDM + "' and BGQZZSXDM='" + BGQZZSXDM + "' and BGQCZCSXM='" + BGQCZCSXM + "' and BGQMSSM='" + BGQMSSM + "' and BGHDLBM='"
                        + BGHDLBM + "' and BGHQSXZ='" + BGHQSXZ + "' and BGHZLDWDM='" + BGHZLDWDM + "' and BGHGDLX='" + BGHGDLX + "' and BGHGDPDJB='" + BGHGDPDJB + "' and BGHTBXHDM='"
                        + BGHTBXHDM + "' and BGHZZSXDM='" + BGHZZSXDM + "' and BGHCZCSXM='" + BGHCZCSXM + "' and BGHMSSM='" + BGHMSSM + "' ";

                    TDZMJ = String2Double(dr["BGMJ"].ToString());
                    MJ = String2Double(dr["MJ"].ToString());      //公顷的
                    dTPS = MathHelper.Round(TDZMJ - MJ, 2);
                    if (dTPS == 0) continue;
                    bool isNav = false;
                    if (dTPS > 0)
                    {
                        isNav = false; //总面积比合计面积大
                    }
                    else
                    {
                        isNav = true; // 比合计面积小 
                    }

                    dTPS = Math.Abs(dTPS);//调平数
                    dTPNum = MathHelper.Round(dTPS / 0.01, 0);
                    ////放入面积小于等于1公顷
                    //List<string> pMaxDLMC = new List<string>();
                    //List<double> pMaxDLMJ = new List<double>();
                    List<TPItem> pMaxTPItem = new List<TPItem>();

                    ////放入面积大于1公顷
                    //List<string> pMinDLMC = new List<string>();
                    //List<double> pMinDLMJ = new List<double>();
                    List<TPItem> pMinTPItem = new List<TPItem>();

                    #region 获得有值的二级地类和面积
                    sql = "select * from BG_JCB_PFM  where " + whereClause;  //这个 需要从原来平方米的 数量里面查询
                    double s2DLMJ = 0.00;
                    string s2DL = "";
                    //按照顺序进行插入列表
                    //后面取值进行倒序排列 从最后一行往前取值
                    DataRow ddr = LS_TysdMDBHelper.GetDataRow(sql, "tmp");

                    for (int index = 0; index < this.listDLBM.Count; index++)
                    {
                        s2DLMJ = String2Double(ddr[listDLBM[index]].ToString());
                        s2DL = listDLBM[index];

                        if (s2DLMJ > 0)
                        {
                            // if (s2DLMJ > 1)
                            if (s2DLMJ > 10000)  //大于10000平米
                            {
                                //pMaxDLMC.Add(s2DL);
                                //pMaxDLMJ.Add(s2DLMJ);
                                pMaxTPItem.Add(new TPItem(s2DL,s2DLMJ,listDLBM));
                            }
                            //else if ((s2DLMJ <= 1) && (s2DLMJ > 0))
                            else if ((s2DLMJ <= 10000) && (s2DLMJ > 0))
                            {
                                //pMinDLMC.Add(s2DL);
                                //pMinDLMJ.Add(s2DLMJ);
                                pMinTPItem.Add(new TPItem(s2DL, s2DLMJ, listDLBM));
                            }
                        }
                    }
                    #endregion


                    //对获得大于1公顷的处理
                    if (pMaxTPItem.Count > 0)
                    {

                        pMaxTPItem.Sort(); //已经是从大到小了

                        #region >1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMaxTPItem.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMaxTPItem.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMaxTPItem.Count + sRemainder;

                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMaxTPItem.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            //for (int j = pMaxTPItem.Count - 1; j >= 0; j--)
                            for (int j =0;j<= pMaxTPItem.Count - 1;j++)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMaxTPItem[j].dlmj / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);
                                    sql = "update " + sTable + " set " + pMaxTPItem[j].dldm + "=" + tpmj + " where " + whereClause;
                                    int iresult = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {
                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            
                            sMax = (sQuotient + 1) * 0.01;
                            sMin = sQuotient * 0.01;
                            if (isNav == true)
                            {
                                sMax = 0 - sMax;
                                sMin = 0 - sMin;
                            }

                            for (int j = 0; j < pMaxTPItem.Count;j++ )
                            {                                
                                //放入Max
                                if (j < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxTPItem[j].dlmj / 10000, 2) + sMax;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                        sql = "update " + sTable + " set " + pMaxTPItem[j].dldm + "=" + gqMj + " where " + whereClause;
                                        int result = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double gqMj = MathHelper.Round(pMaxTPItem[j].dlmj / 10000, 2) + sMin;
                                        gqMj = MathHelper.Round(gqMj, 2);
                                        // sql = "update " + sTable + " set " + pMaxDLMC[j] + "=" + pMaxDLMJ[j] + "+(" + sMin + ") where "+whereClause;
                                        sql = "update " + sTable + " set " + pMaxTPItem[j].dldm + "=" + gqMj + "   where " + whereClause;
                                        LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                //flag++;
                            }
                        }
                        #endregion
                    }
                    else if ((pMaxTPItem.Count == 0) && (pMinTPItem .Count > 0))
                    {//如果一行都是小于等于1公顷也是同样的处理
                        #region <1公顷
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / pMinTPItem.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(pMinTPItem.Count.ToString())));

                        if (sRemainder < 0)
                            sRemainder = pMinTPItem.Count + sRemainder;

                        pMinTPItem.Sort();

                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / pMinTPItem.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = 0; j < pMinTPItem.Count; j++)
                            {
                                if (sTPMJ != 0)
                                {
                                    double tpmj = MathHelper.Round(pMinTPItem[j].dlmj / 10000, 2) + sTPMJ;
                                    tpmj = MathHelper.Round(tpmj, 2);

                                    sql = "update " + sTable + " set " + pMinTPItem[j].dldm + "=" + tpmj + " where " + whereClause;
                                    int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;

                            for (int j = 0; j < pMinTPItem.Count; j++)
                            {
                                sMax = (sQuotient + 1) * 0.01;
                                sMin = sQuotient * 0.01;
                                if (isNav == true)
                                {
                                    sMax = 0 - sMax;
                                    sMin = 0 - sMin;
                                }

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        double tpmj = MathHelper.Round(pMinTPItem[j].dlmj / 10000, 2) + sMax;
                                        tpmj = MathHelper.Round(tpmj, 2);
                                        sql = "update " + sTable + " set " + pMinTPItem[j].dldm + "=" + tpmj + "  where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        double tpmj = MathHelper.Round(pMinTPItem[j].dlmj / 10000, 2) + sMin;
                                        tpmj = MathHelper.Round(tpmj, 2);
                                        sql = "update " + sTable + " set " + pMinTPItem[j].dldm + "=" + tpmj + " where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //降序加0.01
                        int sQuotient;//商值
                        sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((dTPNum / this.listDLBM.Count))));

                        int sRemainder;//余数
                        sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(dTPNum), String2Double(this.listDLBM.Count.ToString())));

                        //都是0 ，的就按照地类代码倒序
                        if (sRemainder < 0)
                            sRemainder = this.listDLBM.Count + sRemainder;
                        if (sRemainder == 0)
                        {
                            //没有余数 进行全部横向有值数据》1公顷的平摊
                            double sTPMJ = dTPS / this.listDLBM.Count;
                            sTPMJ = MathHelper.Round(sTPMJ, 2);
                            if (isNav == true)
                            {
                                sTPMJ = 0 - sTPMJ;
                            }
                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {
                                if (sTPMJ != 0)
                                {

                                    sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sTPMJ + ") where " + whereClause;
                                    int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                }
                            }
                        }
                        else
                        {

                            //有余数  根据余数按照地类倒序进行调平  
                            double sMax = 0.00;
                            double sMin = 0.00;
                            int flag = 0;
                            sMax = (sQuotient + 1) * 0.01;
                            sMin = sQuotient * 0.01;
                            if (isNav == true)
                            {
                                sMax = 0 - sMax;
                                sMin = 0 - sMin;
                            }

                            for (int j = this.listDLBM.Count - 1; j >= 0; j--)
                            {                               

                                //放入Max
                                if (flag < Math.Abs(sRemainder))
                                {
                                    if (sMax != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMax + ") where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                else //放入Mini
                                {
                                    if (sMin != 0)
                                    {
                                        sql = "update " + sTable + " set " + this.listDLBM[j] + "=" + this.listDLBM[j] + "+(" + sMin + ") where " + whereClause;
                                        int tmpr = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                                flag++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

            }



            //从新整理一下经过变更后的列，使其二级地类之和等于一级地类
            StringBuilder sb2 = new StringBuilder();
            sb2.Append("update HZ_JCB set D0201HJ=iif(isnull(D0201),0,D0201)+iif(isnull(D0201K),0,D0201K),")
                .Append(" D0202HJ=iif(isnull(D0202),0,D0202)+iif(isnull(D0202K),0,D0202K),")
                .Append(" D0203HJ=iif(isnull(D0203),0,D0203)+iif(isnull(D0203K),0,D0203K),")
                .Append(" D0204HJ=iif(isnull(D0204),0,D0204)+iif(isnull(D0204K),0,D0204K),")
                .Append(" D0301HJ=iif(isnull(D0301),0,D0301)+iif(isnull(D0301K),0,D0301K),")
                .Append(" D0302HJ=iif(isnull(D0302),0,D0302)+iif(isnull(D0302K),0,D0302K),")
                .Append(" D0307HJ=iif(isnull(D0307),0,D0307)+iif(isnull(D0307K),0,D0307K),")
                .Append(" D0403HJ=iif(isnull(D0403),0,D0403)+iif(isnull(D0403K),0,D0403K),")
                .Append(" D08H2HJ=iif(isnull(D08H2),0,D08H2)+iif(isnull(D08H2A),0,D08H2A),")
                .Append(" D0810HJ=iif(isnull(D0810),0,D0810)+iif(isnull(D0810A),0,D0810A),")
                .Append(" D1104HJ=iif(isnull(D1104),0,D1104)+iif(isnull(D1104A),0,D1104A)+iif(isnull(D1104K),0,D1104K),")
                .Append(" D1107HJ=iif(isnull(D1107),0,D1107)+iif(isnull(D1107A),0,D1107A) ");
            sql = sb2.ToString();
            int iret = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);

            sb2.Clear();
            sb2.Append("update HZ_JCB set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
            .Append(" D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                .Append("D02=iif(isnull(D0201HJ),0,D0201HJ)+iif(isnull(D0202HJ),0,D0202HJ)+iif(isnull(D0203HJ),0,D0203HJ)+iif(isnull(D0204HJ),0,D0204HJ),")
                .Append("D03=iif(isnull(D0301HJ),0,D0301HJ)+iif(isnull(D0302HJ),0,D0302HJ)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307HJ),0,D0307HJ),")
                .Append("D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403HJ),0,D0403HJ)+iif(isnull(D0404),0,D0404),")
                .Append("D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append("D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                .Append("D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2HJ),0,D08H2HJ)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810HJ),0,D0810HJ),")
                .Append("D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append("D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                .Append("iif(isnull(D1104HJ),0,D1104HJ)+iif(isnull(D1107HJ),0,D1107HJ)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                .Append("D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207)  ");
            sql = sb2.ToString();
            iret = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);


            
            #endregion
        }

        private void ChangeTableDW2GQ2(string sTable)
        {
            string sql = "";
            int iRet;
            try
            {
                if (sTable == "HZ_JBNT_BH_JCB")
                {
                    sql = "update " + sTable + " set TDZMJ=round(TDZMJ/10000,2),D010=round(D010/10000,2),D011=round(D011/10000,2),D012=round(D012/10000,2),D013=round(D013/10000,2)" +
                               ",OLDAREA=round(OLDAREA/10000,2)";
                    iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                }
                else
                {

                    iRet = LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_JCB");

                    DataTable dt = LS_ResultMDBHelper.GetDataTable("select * from HZ_JCB_PFM ", "tmp");
                    foreach (DataRow dr in dt.Rows)
                    {

                        #region 都要四舍五入

                        double oldmj = dr["OLDAREA"].ToString().Trim() == "" ? 0 : double.Parse(dr["OLDAREA"].ToString());
                        oldmj = MathHelper.Round(oldmj / 10000, 2);

                        double d0101 = dr["D0101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0101"].ToString());
                        d0101 = MathHelper.Round(d0101 / 10000, 2);
                        double d0102 = dr["D0102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0102"].ToString());
                        d0102 = MathHelper.Round(d0102 / 10000, 2);
                        double d0103 = dr["D0103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0103"].ToString());
                        d0103 = MathHelper.Round(d0103 / 10000, 2);
                        double D01 = MathHelper.Round(d0101 + d0102 + d0103, 2);

                        double D0201 = dr["D0201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201"].ToString());
                        D0201 = MathHelper.Round(D0201 / 10000, 2);
                        double D0202 = dr["D0202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202"].ToString());
                        D0202 = MathHelper.Round(D0202 / 10000, 2);
                        double D0201K = dr["D0201K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201K"].ToString());
                        D0201K = MathHelper.Round(D0201K / 10000, 2);
                        double D0202K = dr["D0202K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202K"].ToString());
                        D0202K = MathHelper.Round(D0202K / 10000, 2);
                        double D0203 = dr["D0203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203"].ToString());
                        D0203 = MathHelper.Round(D0203 / 10000, 2);
                        double D0203K = dr["D0203K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203K"].ToString());
                        D0203K = MathHelper.Round(D0203K / 10000, 2);
                        double D0204 = dr["D0204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204"].ToString());
                        D0204 = MathHelper.Round(D0204 / 10000, 2);
                        double D0204K = dr["D0204K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204K"].ToString());
                        D0204K = MathHelper.Round(D0204K / 10000, 2);
                        double D02 = MathHelper.Round(D0201 + D0201K + D0202 + D0202K + D0203 + D0203K + D0204 + D0204K, 2);

                        double D0301 = dr["D0301"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301"].ToString());
                        D0301 = MathHelper.Round(D0301 / 10000, 2);
                        double D0301K = dr["D0301K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301K"].ToString());
                        D0301K = MathHelper.Round(D0301K / 10000, 2);
                        double D0302 = dr["D0302"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302"].ToString());
                        D0302 = MathHelper.Round(D0302 / 10000, 2);
                        double D0302K = dr["D0302K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302K"].ToString());
                        D0302K = MathHelper.Round(D0302K / 10000, 2);
                        double D0303 = dr["D0303"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0303"].ToString());
                        D0303 = MathHelper.Round(D0303 / 10000, 2);
                        double D0304 = dr["D0304"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0304"].ToString());
                        D0304 = MathHelper.Round(D0304 / 10000, 2);
                        double D0305 = dr["D0305"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0305"].ToString());
                        D0305 = MathHelper.Round(D0305 / 10000, 2);
                        double D0306 = dr["D0306"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0306"].ToString());
                        D0306 = MathHelper.Round(D0306 / 10000, 2);
                        double D0307 = dr["D0307"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307"].ToString());
                        D0307 = MathHelper.Round(D0307 / 10000, 2);
                        double D0307K = dr["D0307K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307K"].ToString());
                        D0307K = MathHelper.Round(D0307K / 10000, 2);
                        double d03 = MathHelper.Round(D0301 + D0301K + D0302 + D0302K + D0305 + D0307 + D0307K, 2);

                        double D0401 = dr["D0401"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0401"].ToString());
                        D0401 = MathHelper.Round(D0401 / 10000, 2);
                        double D0402 = dr["D0402"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0402"].ToString());
                        D0402 = MathHelper.Round(D0402 / 10000, 2);
                        double D0403 = dr["D0403"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403"].ToString());
                        D0403 = MathHelper.Round(D0403 / 10000, 2);
                        double D0403K = dr["D0403K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403K"].ToString());
                        D0403K = MathHelper.Round(D0403K / 10000, 2);
                        double D0404 = dr["D0404"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0404"].ToString());
                        D0404 = MathHelper.Round(D0404 / 10000, 2);
                        double d04 = MathHelper.Round(D0401 + D0403 + D0403K + D0404, 2);

                        double D05H1 = dr["D05H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D05H1"].ToString());
                        D05H1 = MathHelper.Round(D05H1 / 10000, 2);
                        double D0508 = dr["D0508"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0508"].ToString());
                        D0508 = MathHelper.Round(D0508 / 10000, 2);
                        double d05 = MathHelper.Round(D05H1 + D0508, 2);

                        double D0601 = dr["D0601"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0601"].ToString());
                        D0601 = MathHelper.Round(D0601 / 10000, 2);
                        double D0602 = dr["D0602"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0602"].ToString());
                        D0602 = MathHelper.Round(D0602 / 10000, 2);
                        double D0603 = dr["D0603"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0603"].ToString());
                        D0603 = MathHelper.Round(D0603 / 10000, 2);
                        double d06 = MathHelper.Round(D0601 + D0602, 2);

                        double D0701 = dr["D0701"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0701"].ToString());
                        D0701 = MathHelper.Round(D0701 / 10000, 2);
                        double D0702 = dr["D0702"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0702"].ToString());
                        D0702 = MathHelper.Round(D0702 / 10000, 2);
                        double D07 = MathHelper.Round(D0701 + D0702, 2);

                        double D08H1 = dr["D08H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H1"].ToString());
                        D08H1 = MathHelper.Round(D08H1 / 10000, 2);
                        double D08H2 = dr["D08H2"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2"].ToString());
                        D08H2 = MathHelper.Round(D08H2 / 10000, 2);
                        double D08H2A = dr["D08H2A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2A"].ToString());
                        D08H2A = MathHelper.Round(D08H2A / 10000, 2);
                        double D0810A = dr["D0810A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810A"].ToString());
                        D0810A = MathHelper.Round(D0810A / 10000, 2);
                        double D0810 = dr["D0810"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810"].ToString());
                        D0810 = MathHelper.Round(D0810 / 10000, 2);
                        double D0809 = dr["D0809"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0809"].ToString());
                        D0809 = MathHelper.Round(D0809 / 10000, 2);
                        double D08 = MathHelper.Round(D08H1 + D08H2 + D08H2A + D0809 + D0810 + D0810A, 2);
                        double D09 = dr["D09"].ToString().Trim() == "" ? 0 : double.Parse(dr["D09"].ToString());
                        D09 = MathHelper.Round(D09 / 10000, 2);

                        double D1001 = dr["D1001"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1001"].ToString());
                        D1001 = MathHelper.Round(D1001 / 10000, 2);
                        double D1002 = dr["D1002"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1002"].ToString());
                        D1002 = MathHelper.Round(D1002 / 10000, 2);
                        double D1003 = dr["D1003"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1003"].ToString());
                        D1003 = MathHelper.Round(D1003 / 10000, 2);
                        double D1004 = dr["D1004"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1004"].ToString());
                        D1004 = MathHelper.Round(D1004 / 10000, 2);
                        double D1005 = dr["D1005"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1005"].ToString());
                        D1005 = MathHelper.Round(D1005 / 10000, 2);
                        double D1006 = dr["D1006"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1006"].ToString());
                        D1006 = MathHelper.Round(D1006 / 10000, 2);
                        double D1007 = dr["D1007"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1007"].ToString());
                        D1007 = MathHelper.Round(D1007 / 10000, 2);
                        double D1008 = dr["D1008"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1008"].ToString());
                        D1008 = MathHelper.Round(D1008 / 10000, 2);
                        double D1009 = dr["D1009"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1009"].ToString());
                        D1009 = MathHelper.Round(D1009 / 10000, 2);
                        double D10 = MathHelper.Round(D1001 + D1002 + D1003 + D1004 + D1005 + D1006 + D1007 + D1008 + D1009, 2);

                        double D1101 = dr["D1101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1101"].ToString());
                        D1101 = MathHelper.Round(D1101 / 10000, 2);
                        double D1102 = dr["D1102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1102"].ToString());
                        D1102 = MathHelper.Round(D1102 / 10000, 2);
                        double D1103 = dr["D1103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1103"].ToString());
                        D1103 = MathHelper.Round(D1103 / 10000, 2);
                        double D1104 = dr["D1104"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104"].ToString());
                        D1104 = MathHelper.Round(D1104 / 10000, 2);
                        double D1104A = dr["D1104A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104A"].ToString());
                        D1104A = MathHelper.Round(D1104A / 10000, 2);
                        double D1104K = dr["D1104K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104K"].ToString());
                        D1104K = MathHelper.Round(D1104K / 10000, 2);
                        double D1105 = dr["D1105"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1105"].ToString());
                        D1105 = MathHelper.Round(D1105 / 10000, 2);
                        double D1106 = dr["D1106"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1106"].ToString());
                        D1106 = MathHelper.Round(D1106 / 10000, 2);
                        double D1107 = dr["D1107"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107"].ToString());
                        D1107 = MathHelper.Round(D1107 / 10000, 2);
                        double D1107A = dr["D1107A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107A"].ToString());
                        D1107A = MathHelper.Round(D1107A / 10000, 2);
                        double D1108 = dr["D1108"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1108"].ToString());
                        D1108 = MathHelper.Round(D1108 / 10000, 2);
                        double D1109 = dr["D1109"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1109"].ToString());
                        D1109 = MathHelper.Round(D1109 / 10000, 2);
                        double D1110 = dr["D1110"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1110"].ToString());
                        D1110 = MathHelper.Round(D1110 / 10000, 2);
                        double D11 = MathHelper.Round(D1101 + D1102 + D1103 + D1104 + D1104A + D1104K + D1107 + D1107A + D1109 + D1110, 2);

                        double D1201 = dr["D1201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1201"].ToString());
                        D1201 = MathHelper.Round(D1201 / 10000, 2);
                        double D1202 = dr["D1202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1202"].ToString());
                        D1202 = MathHelper.Round(D1202 / 10000, 2);
                        double D1203 = dr["D1203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1203"].ToString());
                        D1203 = MathHelper.Round(D1203 / 10000, 2);
                        double D1204 = dr["D1204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1204"].ToString());
                        D1204 = MathHelper.Round(D1204 / 10000, 2);
                        double D1205 = dr["D1205"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1205"].ToString());
                        D1205 = MathHelper.Round(D1205 / 10000, 2);
                        double D1206 = dr["D1206"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1206"].ToString());
                        D1206 = MathHelper.Round(D1206 / 10000, 2);
                        double D1207 = dr["D1207"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1207"].ToString());
                        D1207 = MathHelper.Round(D1207 / 10000, 2);
                        double D12 = MathHelper.Round(D1201 + D1202 + D1203 + D1204 + D1205 + D1206 + D1207, 2);

                        double D00 = MathHelper.Round(D0303 + D0304 + D0306 + D0402 + D0603 + D1105 + D1106 + D1108, 2);
                        double tdzmj = MathHelper.Round(D00 + D01 + D02 + d03 + d04 + d05 + d06 + D07 + D08 + D09 + D10 + D11 + D12, 2);

                        #endregion

                        StringBuilder sb = new StringBuilder();
                        sb.Append("insert into HZ_JCB(ZLDWDM,QSDWDM,QSXZ,GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,tdzmj,")
                        .Append("D00,D01, D0101,D0102,D0103,D02,D0201,D0201K,D0202,D0202K,D0203,D0203K,D0204,D0204K,D03, D0301, D0301K,D0302,D0302K,D0303,D0304, D0305,  D0306,  D0307,  D0307K,")
                        .Append("D04,D0401,D0402,D0403,D0403K,D0404,D05,D05H1, D0508,")
                        .Append(" D06,D0601,  D0602,  D0603, D07, D0701,  D0702, D08, D08H1,  D08H2,  D08H2A,  D0809,  D0810,  D0810A,  D09, ")
                        .Append("D10, D1001,  D1002,  D1003,  D1004,  D1005,  D1006,  D1007,  D1008,  D1009, ")
                        .Append("D11, D1101,  D1102,  D1103,  D1104,  D1104A,  D1104K,  D1105,  D1106,  D1107,  D1107A,  D1108,  D1109,D1110,")
                        .Append("D12, D1201,  D1202,  D1203,  D1204,  D1205,  D1206,D1207,OLDAREA )")
                        .Append("values ('").Append(dr["ZLDWDM"].ToString()).Append("','").Append(dr["QSDWDM"].ToString()).Append("','").Append(dr["QSXZ"].ToString()).Append("','")
                        .Append(dr["GDPDJB"].ToString()).Append("','").Append(dr["GDLX"].ToString()).Append("','").Append(dr["GDZZSXDM"].ToString()).Append("','")
                        .Append(dr["CZCSXM"].ToString()).Append("','").Append(dr["TBXHDM"].ToString()).Append("','").Append(dr["MSSM"].ToString()).Append("','")
                        .Append(dr["FRDBS"].ToString()).Append("',").Append(tdzmj).Append(",").Append(D00).Append(",").Append(D01).Append(",")
                        .Append(d0101).Append(",").Append(d0102).Append(",").Append(d0103).Append(",")
                        .Append(D02).Append(",").Append(D0201).Append(",").Append(D0201K).Append(",").Append(D0202).Append(",").Append(D0202K).Append(",").Append(D0203).Append(",").Append(D0203K).Append(",").Append(D0204).Append(",").Append(D0204K).Append(",")
                        .Append(d03).Append(",").Append(D0301).Append(",").Append(D0301K).Append(",").Append(D0302).Append(",").Append(D0302K).Append(",").Append(D0303).Append(",").Append(D0304).Append(",")
                        .Append(D0305).Append(",").Append(D0306).Append(",").Append(D0307).Append(",").Append(D0307K).Append(",")

                        .Append(d04).Append(",").Append(D0401).Append(",").Append(D0402).Append(",").Append(D0403).Append(",").Append(D0403K).Append(",").Append(D0404).Append(",")
                        .Append(d05).Append(",").Append(D05H1).Append(",").Append(D0508).Append(",")
                        .Append(d06).Append(",").Append(D0601).Append(",").Append(D0602).Append(",").Append(D0603).Append(",")
                        .Append(D07).Append(",").Append(D0701).Append(",").Append(D0702).Append(",")
                        .Append(D08).Append(",").Append(D08H1).Append(",").Append(D08H2).Append(",").Append(D08H2A).Append(",").Append(D0809).Append(",").Append(D0810).Append(",").Append(D0810A).Append(",")
                        .Append(D09).Append(",")
                        .Append(D10).Append(",").Append(D1001).Append(",").Append(D1002).Append(",").Append(D1003).Append(",").Append(D1004).Append(",").Append(D1005).Append(",").Append(D1006).Append(",").Append(D1007).Append(",")
                        .Append(D1008).Append(",").Append(D1009).Append(",")
                        .Append(D11).Append(",").Append(D1101).Append(",").Append(D1102).Append(",").Append(D1103).Append(",").Append(D1104).Append(",")
                        .Append(D1104A).Append(",").Append(D1104K).Append(",").Append(D1105).Append(",").Append(D1106).Append(",").Append(D1107).Append(",").Append(D1107A).Append(",")
                        .Append(D1108).Append(",").Append(D1109).Append(",").Append(D1110).Append(",")
                        .Append(D12).Append(",").Append(D1201).Append(",").Append(D1202).Append(",").Append(D1203).Append(",")
                        .Append(D1204).Append(",").Append(D1205).Append(",").Append(D1206).Append(",")
                        .Append(D1207).Append(",").Append(oldmj).Append(" ) ");
                        sql = sb.ToString();

                        iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                    }

                }

            }
            catch (Exception ex) { }

        }

        /// <summary>
        /// 把表里 的数据都转化为公顷
        /// </summary>
        /// <param name="sTable"></param>
        public void ChangeTableDW2GQ(string sTable)
        {
            string  sql = "";
            int iRet ;
            try
            {
                if (sTable == "HZ_JBNT_BH_JCB")
                {
                    sql = "update " + sTable + " set TDZMJ=round(TDZMJ/10000,2),D010=round(D010/10000,2),D011=round(D011/10000,2),D012=round(D012/10000,2),D013=round(D013/10000,2)" +
                               ",OLDAREA=round(OLDAREA/10000,2)";
                    iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                }
                else
                {

                    iRet= LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_JCB");

                    DataTable dt =LS_ResultMDBHelper.GetDataTable(  "select * from HZ_JCB_PFM " ,"tmp");
                    foreach (DataRow dr in dt.Rows)
                    {

                        #region 都要四舍五入

                        double oldmj = dr["OLDAREA"].ToString().Trim() == "" ? 0 : double.Parse(dr["OLDAREA"].ToString());
                        oldmj=MathHelper.Round(oldmj/10000,2);

                        double d0101 = dr["D0101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0101"].ToString());
                        d0101 = MathHelper.Round(d0101 / 10000, 2);
                        double d0102 = dr["D0102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0102"].ToString());
                        d0102 = MathHelper.Round(d0102 / 10000, 2);
                        double d0103 = dr["D0103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0103"].ToString());
                        d0103 = MathHelper.Round(d0103 / 10000, 2);
                        double D01 = MathHelper.Round(d0101 + d0102 + d0103, 2);

                        double D0201 = dr["D0201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201"].ToString());
                        D0201 = MathHelper.Round(D0201 / 10000, 2);
                        double D0202 = dr["D0202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202"].ToString());
                        D0202 = MathHelper.Round(D0202 / 10000, 2);
                        double D0201K = dr["D0201K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0201K"].ToString());
                        D0201K = MathHelper.Round(D0201K / 10000, 2);
                        double D0202K = dr["D0202K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0202K"].ToString());
                        D0202K = MathHelper.Round(D0202K / 10000, 2);
                        double D0203 = dr["D0203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203"].ToString());
                        D0203 = MathHelper.Round(D0203 / 10000, 2);
                        double D0203K = dr["D0203K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0203K"].ToString());
                        D0203K = MathHelper.Round(D0203K / 10000, 2);
                        double D0204 = dr["D0204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204"].ToString());
                        D0204 = MathHelper.Round(D0204 / 10000, 2);
                        double D0204K = dr["D0204K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0204K"].ToString());
                        D0204K = MathHelper.Round(D0204K / 10000, 2);
                        double D02 = MathHelper.Round(D0201 + D0201K + D0202 + D0202K + D0203 + D0203K + D0204 + D0204K, 2);

                        double D0301 = dr["D0301"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301"].ToString());
                        D0301 = MathHelper.Round(D0301 / 10000, 2);
                        double D0301K = dr["D0301K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0301K"].ToString());
                        D0301K = MathHelper.Round(D0301K / 10000, 2);
                        double D0302 = dr["D0302"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302"].ToString());
                        D0302 = MathHelper.Round(D0302 / 10000, 2);
                        double D0302K = dr["D0302K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0302K"].ToString());
                        D0302K = MathHelper.Round(D0302K / 10000, 2);
                        double D0303 = dr["D0303"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0303"].ToString());
                        D0303 = MathHelper.Round(D0303 / 10000, 2);
                        double D0304 = dr["D0304"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0304"].ToString());
                        D0304 = MathHelper.Round(D0304 / 10000, 2);
                        double D0305 = dr["D0305"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0305"].ToString());
                        D0305 = MathHelper.Round(D0305 / 10000, 2);
                        double D0306 = dr["D0306"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0306"].ToString());
                        D0306 = MathHelper.Round(D0306 / 10000, 2);
                        double D0307 = dr["D0307"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307"].ToString());
                        D0307 = MathHelper.Round(D0307 / 10000, 2);
                        double D0307K = dr["D0307K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0307K"].ToString());
                        D0307K = MathHelper.Round(D0307K / 10000, 2);
                        double d03 = MathHelper.Round(D0301 + D0301K + D0302 + D0302K + D0305 + D0307 + D0307K, 2);

                        double D0401 = dr["D0401"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0401"].ToString());
                        D0401 = MathHelper.Round(D0401 / 10000, 2);
                        double D0402 = dr["D0402"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0402"].ToString());
                        D0402 = MathHelper.Round(D0402 / 10000, 2);
                        double D0403 = dr["D0403"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403"].ToString());
                        D0403 = MathHelper.Round(D0403 / 10000, 2);
                        double D0403K = dr["D0403K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0403K"].ToString());
                        D0403K = MathHelper.Round(D0403K / 10000, 2);
                        double D0404 = dr["D0404"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0404"].ToString());
                        D0404 = MathHelper.Round(D0404 / 10000, 2);
                        double d04 = MathHelper.Round(D0401 + D0403 + D0403K + D0404,2);
                      
                        double D05H1 = dr["D05H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D05H1"].ToString());
                        D05H1 = MathHelper.Round(D05H1 / 10000, 2);
                        double D0508 = dr["D0508"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0508"].ToString());
                        D0508 = MathHelper.Round(D0508 / 10000, 2);
                        double d05 = MathHelper.Round(D05H1 + D0508, 2);                        
                        
                        double D0601 = dr["D0601"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0601"].ToString());
                        D0601 = MathHelper.Round(D0601 / 10000, 2);
                        double D0602 = dr["D0602"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0602"].ToString());
                        D0602 = MathHelper.Round(D0602 / 10000, 2);
                        double D0603 = dr["D0603"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0603"].ToString());
                        D0603 = MathHelper.Round(D0603 / 10000, 2);
                        double d06 = MathHelper.Round(D0601+D0602, 2);
                       
                        double D0701 = dr["D0701"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0701"].ToString());
                        D0701 = MathHelper.Round(D0701 / 10000, 2);
                        double D0702 = dr["D0702"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0702"].ToString());
                        D0702 = MathHelper.Round(D0702 / 10000, 2);
                        double D07 = MathHelper.Round(D0701 + D0702, 2);
                        
                        double D08H1 = dr["D08H1"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H1"].ToString());
                        D08H1 = MathHelper.Round(D08H1 / 10000, 2);
                        double D08H2 = dr["D08H2"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2"].ToString());
                        D08H2 = MathHelper.Round(D08H2 / 10000, 2);
                        double D08H2A = dr["D08H2A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D08H2A"].ToString());
                        D08H2A = MathHelper.Round(D08H2A / 10000, 2);
                        double D0810A = dr["D0810A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810A"].ToString());
                        D0810A = MathHelper.Round(D0810A / 10000, 2);
                        double D0810 = dr["D0810"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0810"].ToString());
                        D0810 = MathHelper.Round(D0810 / 10000, 2);
                        double D0809 = dr["D0809"].ToString().Trim() == "" ? 0 : double.Parse(dr["D0809"].ToString());
                        D0809 = MathHelper.Round(D0809 / 10000, 2);
                        double D08 = MathHelper.Round(D08H1 + D08H2 + D08H2A + D0809 + D0810 + D0810A, 2);
                        double D09 = dr["D09"].ToString().Trim() == "" ? 0 : double.Parse(dr["D09"].ToString());
                        D09 = MathHelper.Round(D09 / 10000, 2);
                       
                        double D1001 = dr["D1001"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1001"].ToString());
                        D1001 = MathHelper.Round(D1001 / 10000, 2);
                        double D1002 = dr["D1002"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1002"].ToString());
                        D1002 = MathHelper.Round(D1002 / 10000, 2);
                        double D1003 = dr["D1003"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1003"].ToString());
                        D1003 = MathHelper.Round(D1003 / 10000, 2);
                        double D1004 = dr["D1004"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1004"].ToString());
                        D1004 = MathHelper.Round(D1004 / 10000, 2);
                        double D1005 = dr["D1005"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1005"].ToString());
                        D1005 = MathHelper.Round(D1005 / 10000, 2);
                        double D1006 = dr["D1006"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1006"].ToString());
                        D1006 = MathHelper.Round(D1006 / 10000, 2);
                        double D1007 = dr["D1007"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1007"].ToString());
                        D1007 = MathHelper.Round(D1007 / 10000, 2);
                        double D1008 = dr["D1008"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1008"].ToString());
                        D1008 = MathHelper.Round(D1008 / 10000, 2);
                        double D1009 = dr["D1009"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1009"].ToString());
                        D1009 = MathHelper.Round(D1009 / 10000, 2);
                        double D10 = MathHelper.Round(D1001 + D1002 + D1003 + D1004 + D1005 + D1006 + D1007 + D1008 + D1009, 2);

                        double D1101 = dr["D1101"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1101"].ToString());
                        D1101 = MathHelper.Round(D1101 / 10000, 2);
                        double D1102 = dr["D1102"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1102"].ToString());
                        D1102 = MathHelper.Round(D1102 / 10000, 2);
                        double D1103 = dr["D1103"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1103"].ToString());
                        D1103 = MathHelper.Round(D1103 / 10000, 2);
                        double D1104 = dr["D1104"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104"].ToString());
                        D1104 = MathHelper.Round(D1104 / 10000, 2);
                        double D1104A = dr["D1104A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104A"].ToString());
                        D1104A = MathHelper.Round(D1104A / 10000, 2);
                        double D1104K = dr["D1104K"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1104K"].ToString());
                        D1104K = MathHelper.Round(D1104K / 10000, 2);
                        double D1105 = dr["D1105"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1105"].ToString());
                        D1105 = MathHelper.Round(D1105 / 10000, 2);
                        double D1106 = dr["D1106"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1106"].ToString());
                        D1106 = MathHelper.Round(D1106 / 10000, 2);
                        double D1107 = dr["D1107"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107"].ToString());
                        D1107 = MathHelper.Round(D1107 / 10000, 2);
                        double D1107A = dr["D1107A"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1107A"].ToString());
                        D1107A = MathHelper.Round(D1107A / 10000, 2);
                        double D1108 = dr["D1108"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1108"].ToString());
                        D1108 = MathHelper.Round(D1108 / 10000, 2);
                        double D1109 = dr["D1109"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1109"].ToString());
                        D1109 = MathHelper.Round(D1109 / 10000, 2);
                        double D1110 = dr["D1110"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1110"].ToString());
                        D1110 = MathHelper.Round(D1110 / 10000, 2);
                        double D11 = MathHelper.Round(D1101 + D1102 + D1103 + D1104 + D1104A + D1104K + D1107 + D1107A + D1109 + D1110, 2);
                       
                        double D1201 = dr["D1201"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1201"].ToString());
                        D1201 = MathHelper.Round(D1201 / 10000, 2);
                        double D1202 = dr["D1202"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1202"].ToString());
                        D1202 = MathHelper.Round(D1202 / 10000, 2);
                        double D1203 = dr["D1203"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1203"].ToString());
                        D1203 = MathHelper.Round(D1203 / 10000, 2);
                        double D1204 = dr["D1204"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1204"].ToString());
                        D1204 = MathHelper.Round(D1204 / 10000, 2);
                        double D1205 = dr["D1205"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1205"].ToString());
                        D1205 = MathHelper.Round(D1205 / 10000, 2);
                        double D1206 = dr["D1206"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1206"].ToString());
                        D1206 = MathHelper.Round(D1206 / 10000, 2);
                        double D1207 = dr["D1207"].ToString().Trim() == "" ? 0 : double.Parse(dr["D1207"].ToString());
                        D1207 = MathHelper.Round(D1207 / 10000, 2);
                        double D12 = MathHelper.Round(D1201 + D1202 + D1203 + D1204 + D1205 + D1206 + D1207, 2);

                        double D00 = MathHelper.Round(D0303 + D0304 + D0306 + D0402 + D0603 + D1105 + D1106 + D1108, 2);
                       // double tdzmj = MathHelper.Round(D00 + D01 + D02 + d03 + d04 + d05 + d06 + D07 + D08 + D09 + D10 + D11 + D12, 2);

                        double tdzmj = dr["TDZMJ"].ToString().Trim() == "" ? 0 : double.Parse(dr["TDZMJ"].ToString());
                        tdzmj = MathHelper.Round(tdzmj/10000, 2);
                        #endregion 

                        StringBuilder sb = new StringBuilder();
                        sb.Append("insert into HZ_JCB(ZLDWDM,QSDWDM,QSXZ,GDPDJB,GDLX,GDZZSXDM,CZCSXM,TBXHDM,MSSM,FRDBS,tdzmj,")
                        .Append("D00,D01, D0101,D0102,D0103,D02,D0201,D0201K,D0202,D0202K,D0203,D0203K,D0204,D0204K,D03, D0301, D0301K,D0302,D0302K,D0303,D0304, D0305,  D0306,  D0307,  D0307K,")
                        .Append("D04,D0401,D0402,D0403,D0403K,D0404,D05,D05H1, D0508,")
                        .Append(" D06,D0601,  D0602,  D0603, D07, D0701,  D0702, D08, D08H1,  D08H2,  D08H2A,  D0809,  D0810,  D0810A,  D09, ")
                        .Append("D10, D1001,  D1002,  D1003,  D1004,  D1005,  D1006,  D1007,  D1008,  D1009, ")
                        .Append("D11, D1101,  D1102,  D1103,  D1104,  D1104A,  D1104K,  D1105,  D1106,  D1107,  D1107A,  D1108,  D1109,D1110,")
                        .Append("D12, D1201,  D1202,  D1203,  D1204,  D1205,  D1206,D1207,OLDAREA )")
                        .Append("values ('").Append(dr["ZLDWDM"].ToString()).Append("','").Append(dr["QSDWDM"].ToString()).Append("','").Append(dr["QSXZ"].ToString()).Append("','")
                        .Append(dr["GDPDJB"].ToString()).Append("','").Append(dr["GDLX"].ToString()).Append("','").Append(dr["GDZZSXDM"].ToString()).Append("','")
                        .Append(dr["CZCSXM"].ToString()).Append("','").Append(dr["TBXHDM"].ToString()).Append("','").Append(dr["MSSM"].ToString()).Append("','")
                        .Append(dr["FRDBS"].ToString()).Append("',").Append(tdzmj).Append(",").Append(D00).Append(",").Append(D01).Append(",")
                        .Append(d0101).Append(",").Append(d0102).Append(",").Append(d0103).Append(",")
                        .Append(D02).Append(",").Append(D0201).Append(",").Append(D0201K).Append(",").Append(D0202).Append(",").Append(D0202K).Append(",").Append(D0203).Append(",").Append(D0203K).Append(",").Append(D0204).Append(",").Append(D0204K).Append(",")
                        .Append(d03).Append(",").Append(D0301).Append(",").Append(D0301K).Append(",").Append(D0302).Append(",").Append(D0302K).Append(",").Append(D0303).Append(",").Append(D0304).Append(",")
                        .Append(D0305).Append(",") .Append(D0306).Append(",").Append(D0307).Append(",").Append(D0307K).Append(",")

                        .Append(d04).Append(",").Append(D0401).Append(",").Append(D0402).Append(",").Append(D0403).Append(",").Append(D0403K).Append(",").Append(D0404).Append(",")
                        .Append(d05).Append(",").Append(D05H1).Append(",").Append(D0508).Append(",")
                        .Append(d06).Append(",").Append(D0601).Append(",").Append(D0602).Append(",").Append(D0603).Append(",")
                        .Append(D07).Append(",").Append(D0701).Append(",").Append(D0702).Append(",")
                        .Append(D08).Append(",").Append(D08H1).Append(",").Append(D08H2).Append(",").Append(D08H2A).Append(",").Append(D0809).Append(",").Append(D0810).Append(",").Append(D0810A).Append(",")
                        .Append(D09).Append(",") 
                        .Append(D10).Append(",").Append(D1001).Append(",").Append(D1002 ).Append(",").Append(D1003).Append(",").Append(D1004 ).Append(",").Append(D1005 ).Append(",").Append(D1006).Append(",").Append(D1007).Append(",")
                        .Append(D1008).Append(",").Append(D1009).Append(",")
                        .Append(D11).Append(",").Append(D1101).Append(",").Append(D1102).Append(",").Append(D1103).Append(",").Append(D1104).Append(",")
                        .Append(D1104A ).Append(",").Append(D1104K).Append(",").Append(D1105).Append(",").Append(D1106).Append(",").Append(D1107).Append(",").Append(D1107A).Append(",")
                        .Append(D1108).Append(",").Append(D1109).Append(",").Append(D1110).Append(",")
                        .Append(D12).Append(",").Append(D1201).Append(",").Append(D1202).Append(",").Append(D1203).Append(",")
                        .Append(D1204).Append(",").Append(D1205).Append(",").Append(D1206).Append(",")
                        .Append(D1207).Append(",").Append(oldmj).Append(" ) ");
                        sql = sb.ToString();
                       
                        iRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                    }                  
                  
                }
                
            }
            catch (Exception ex) { }

        }
        

        /// <summary>
        /// 获得单位个数
        /// </summary>
        /// <param name="sTable"></param>
        /// <returns></returns>
        private int GetXZDWNum(string sTable)
        {
            try
            {
                string sql = "select distinct  bghzldwdm   from " + sTable + "  ";
                DataTable dt = LS_TysdMDBHelper.GetDataTable(sql, "tmp");
                return dt.Rows.Count;
            }
            catch { return 0; }
        }
        
        
        /// <summary>
        /// 获得调平多的单位列表
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sYear"></param>
        /// <param name="sTable"></param>
        /// <param name="sRemainder"></param>
        /// <returns></returns>
        private List<string> GetTPDWList(string sTable, int sRemainder, int skipNum = 0, string mssm = "00")
        {
            //获得单位个数是总面积的大小排序
            try
            {
                string sql = "select sum(BGMJ),BGHZLDWDM  from " + sTable +
                    "  group by BGHZLDWDM order by sum(BGMJ) desc";
                //sql = "select *  from (" + sql + ") where rownum<=" + sRemainder;

                List<string> pList = new List<string>();
                DataTable dt = LS_TysdMDBHelper.GetDataTable(sql, "tmp");
                int icount = 0;
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (icount == sRemainder + skipNum)
                            break;
                        if (icount < skipNum)
                        {
                            icount++;
                            continue;
                        }
                        pList.Add(dr["BGHZLDWDM"].ToString());
                        icount++;
                    }
                    return pList;
                }
                return null;
            }
            catch { return null; }

        }
      
        #region 对需要调平的单位进行调平总面积
        /// <summary>
        /// 对需要调平的单位进行调平总面积
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sYear"></param>
        /// <param name="sTable"></param>
        /// <param name="sXZQDM"></param>需要调平的行政区代码
        /// <param name="sTPArea"></param>需要调平的面积
        private void MakeBalance2Area(string sTable,string sXZQDM, double sTPArea, string smssm)
        {
            try
            {
               
                ///面积小于等于1 的不参与调平
                string sql = "select Count(*)  from " + sTable +
                    " where  BGHZLDWDM='" + sXZQDM + "' ";
                //本村的单位个数
                DataRow onlyRow = LS_TysdMDBHelper.GetDataRow(sql, "tmp");
                if (onlyRow == null) return;
                int XZQDWNum = String2Int(onlyRow[0].ToString());
                if (XZQDWNum == 0) return;
                //本村的调平数目
                double sTPNum = sTPArea / 0.01;
                int sQuotient;//商值
                sQuotient = Double2Int(Math.Truncate(sTPNum / XZQDWNum));

                int sRemainder;//余数
                sRemainder = Double2Int(Math.IEEERemainder(sTPNum, String2Double(XZQDWNum.ToString())));
                if (sRemainder < 0)
                    sRemainder = XZQDWNum + sRemainder;
                //大的调平面积
                double sMaxTPArea = (sQuotient + 1) * 0.01;

                //小的调平面积
                double sMinTPArea = sQuotient * 0.01;

                if (this.isNegative == true)
                {
                    sMaxTPArea = (-sMaxTPArea);
                    sMinTPArea = (-sMinTPArea);
                }
                int icount = 0;
                sql = "select * from " + sTable + " where BGHZLDWDM='" + sXZQDM + "'  order by BGMJ desc ";
                DataTable tpDt = LS_TysdMDBHelper.GetDataTable(sql, "tmp");
                foreach (DataRow aRow in tpDt.Rows)
                {

                    //string zldwdm = aRow["ZLDWDM"].ToString().Trim();
                    //string qsdwdm = aRow["QSDWDM"].ToString().Trim();
                    //string qsxz = aRow["QSXZ"].ToString().Trim();
                    //string gdpdj = aRow["GDPDJB"].ToString().Trim();
                    //string gdlx = aRow["GDLX"].ToString().Trim();
                    //string gdzzsx = aRow["GDZZSXDM"].ToString().Trim();
                    //string czcsxm = aRow["CZCSXM"].ToString().Trim();
                    //string tbxhdm = aRow["TBXHDM"].ToString().Trim();
                    //string mssm = aRow["MSSM"].ToString().Trim();
                    //string frdbs = aRow["FRDBS"].ToString().Trim();
                    //double tdzmj = 0;
                    //double.TryParse(aRow["TDZMJ"].ToString(), out tdzmj);

                    //string whereClause = " ZLDWDM='" + zldwdm + "' and QSDWDM='" + qsdwdm + "' and QSXZ='" + qsxz + "' and GDPDJB='" + gdpdj + "' and GDLX='" + gdlx + "' "
                    //       + " and GDZZSXDM='" + gdzzsx + "' and CZCSXM='" + czcsxm + "' and TBXHDM='" + tbxhdm + "' and MSSM='" + mssm + "' and FRDBS='" + frdbs + "' ";


                    string BGQDLBM = aRow["BGQDLBM"].ToString().Trim();
                    string BGQQSXZ = aRow["BGQQSXZ"].ToString().Trim();
                    string BGQZLDWDM = aRow["BGQZLDWDM"].ToString().Trim();
                    string BGQGDLX = aRow["BGQGDLX"].ToString().Trim();
                    string BGQGDPDJB = aRow["BGQGDPDJB"].ToString().Trim();
                    string BGQTBXHDM = aRow["BGQTBXHDM"].ToString().Trim();
                    string BGQZZSXDM = aRow["BGQZZSXDM"].ToString().Trim();
                    string BGQCZCSXM = aRow["BGQCZCSXM"].ToString().Trim();
                    string BGQMSSM = aRow["BGQMSSM"].ToString().Trim();
                    string BGHDLBM = aRow["BGHDLBM"].ToString().Trim();
                    string BGHQSXZ = aRow["BGHQSXZ"].ToString().Trim();
                    string BGHZLDWDM = aRow["BGHZLDWDM"].ToString().Trim();
                    string BGHGDLX = aRow["BGHGDLX"].ToString().Trim();
                    string BGHGDPDJB = aRow["BGHGDPDJB"].ToString().Trim();
                    string BGHTBXHDM = aRow["BGHTBXHDM"].ToString().Trim();
                    string BGHZZSXDM = aRow["BGHZZSXDM"].ToString().Trim();
                    string BGHCZCSXM = aRow["BGHCZCSXM"].ToString().Trim();
                    string BGHMSSM = aRow["BGHMSSM"].ToString().Trim();
                    double tdzmj = 0;
                    double.TryParse(aRow["BGMJ"].ToString(), out tdzmj);

                    string whereClause = " BGQDLBM='" + BGQDLBM + "' and BGQQSXZ='" + BGQQSXZ + "' and BGQZLDWDM='" + BGQZLDWDM + "' and BGQGDLX='" + BGQGDLX + "' and BGQGDPDJB='"
                        + BGQGDPDJB + "' and BGQTBXHDM='" + BGQTBXHDM + "' and BGQZZSXDM='" + BGQZZSXDM + "' and BGQCZCSXM='" + BGQCZCSXM + "' and BGQMSSM='" + BGQMSSM + "' and BGHDLBM='"
                        + BGHDLBM + "' and BGHQSXZ='" + BGHQSXZ + "' and BGHZLDWDM='" + BGHZLDWDM + "' and BGHGDLX='" + BGHGDLX + "' and BGHGDPDJB='" + BGHGDPDJB + "' and BGHTBXHDM='"
                        + BGHTBXHDM + "' and BGHZZSXDM='" + BGHZZSXDM + "' and BGHCZCSXM='" + BGHCZCSXM + "' and BGHMSSM='" + BGHMSSM + "' ";


                    if (icount >= sRemainder)
                    {
                        if (sMinTPArea == 0)
                            break;
                        tdzmj = MathHelper.Round(tdzmj + sMinTPArea, 2);
                       // sql = "update " + sTable + " set TDZMJ=tdzmj+" + sMinTPArea + " where " + whereClause;
                        sql = "update " + sTable + " set BGMJ=" + tdzmj + " where " + whereClause;
                        LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                        
                    }
                    else
                    {                        
                        //sql = "update " + sTable + " set TDZMJ=tdzmj+(" + sMaxTPArea + ") where " + whereClause;
                        tdzmj = MathHelper.Round(tdzmj + sMaxTPArea, 2);
                        sql = "update " + sTable + " set BGMJ=" + tdzmj + " where " + whereClause;
                        int iret = LS_TysdMDBHelper.ExecuteSQLNonquery(sql);
                    }

                    icount++;
                }

            }
            catch { }
        }
        #endregion
        

        //获得基础统计表和基础汇总表的差值 判断调平数的正负号
        private bool isNegative = false;
        public void MakeBalance(string mssm)
        {
            string sTablePfm = "BG_JCB_PFM";
            string sTable = "BG_JCB";

            string sql = "select * from " + sTable + " ";
            DataTable dt = LS_TysdMDBHelper.GetDataTable(sql, "temp");
            if (dt.Rows.Count == 0) return;

            double dXzqKzmj = 0;
            dXzqKzmj = GetXQKZMJLDHD(sTablePfm, true, mssm);  //平方米面积和 转公顷

            if (dXzqKzmj == 0) return;

            //ChangeTableDW2GQ(sTable);  //转化为公顷

            //计算下去汇总面积 换算为公顷的和
            double dXQHzmj = GetXQKZMJ(sTable,false, mssm);  //GetXQHZMJ(sTable);
            if (dXQHzmj == 0) return;

            if (dXzqKzmj!=dXQHzmj)
            {
                //纵横都不平
                #region//纵横都不平
                double sTPS = 0.00;//调平数            
                sTPS = Math.Round(dXzqKzmj - dXQHzmj, 2);
                if (sTPS < 0)
                {
                    isNegative = true;  //是付的
                }
                else
                {
                    isNegative = false;
                }
                sTPS = MathHelper.RoundEx(sTPS, 2);
                sTPS = Math.Abs(sTPS);
                double sTPNum = 0;//调平数目
                sTPNum =MathHelper.Round( sTPS / 0.01,0);


                int sNum = 0;//乡镇、村子个数
                sNum = GetXZDWNum(sTable);
                int sQuotient;//商值
                sQuotient = Double2Int(Math.Truncate(Convert.ToDouble((sTPNum / sNum))));

                int sRemainder;//余数
                sRemainder = Double2Int(Math.IEEERemainder(Convert.ToDouble(sTPNum), String2Double(sNum.ToString())));

                if (sRemainder < 0)
                    sRemainder = sNum + sRemainder;

                double sTPMaxArea;//调平大面积
                sTPMaxArea = (sQuotient + 1) * 0.01;
                List<string> pMaxList = GetTPDWList(sTable, sRemainder, 0, mssm);
                string sStrXZDWDM;
                if (pMaxList != null)
                {
                    if (pMaxList.Count != 0)
                    {

                       
                        for (int i = 0; i < pMaxList.Count; i++)
                        {
                            sStrXZDWDM = pMaxList[i];
                            MakeBalance2Area( sTable, sStrXZDWDM, sTPMaxArea, mssm);
                        }
                    }
                }

                double sTPMinArea;//调平小面积
                sTPMinArea = sQuotient * 0.01;
                if (sTPMinArea != 0)
                {
                    List<string> pMinList = GetTPDWList(sTable, (int)sTPNum - sRemainder, sRemainder, mssm);
                    if (pMinList != null)
                    {
                        if (pMinList.Count != 0)
                        {

                            for (int i = 0; i < pMinList.Count; i++)
                            {
                                sStrXZDWDM = pMinList[i];
                                MakeBalance2Area( sTable, sStrXZDWDM, sTPMinArea, mssm);
                            }
                        }
                    }
                }
                
                #endregion
            }
        }


       

    }
}
