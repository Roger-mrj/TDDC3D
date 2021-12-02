using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.Data;

using RCIS.GISCommon;
using RCIS.Utility;
using RCIS.Database;

namespace TDDC3D.output
{
    /// <summary>
    /// 统计批准未建设土地
    /// </summary>
    public class clsStatsPzwjs
    {
        private IWorkspace currWs = null;

        
        private IFeatureClass dltbClass = null;
        private IFeatureClass pzwjstdClass = null;

        public clsStatsPzwjs(IWorkspace _ws)
        {
            this.currWs = _ws;
            IFeatureWorkspace pFeaWs = _ws as IFeatureWorkspace;
           
            dltbClass = pFeaWs.OpenFeatureClass("DLTB");
            pzwjstdClass = pFeaWs.OpenFeatureClass("PZWJSTD");

        }




        public void getPzwjsTmp()
        {

            RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_PZWJS_TMP");
            //分村 统计 批准未建设土地，换算为公顷，写入 统计表
            IFeatureCursor pFeaCusor = pzwjstdClass.Search(null, false);
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            try
            {
                IFeature aPWJ = null;
                while ((aPWJ = pFeaCusor.NextFeature()) != null)
                {
                    
                    #region 一个
                    IGeometry aPZWJS = aPWJ.Shape;
                    ISpatialFilter pSf = new SpatialFilterClass();
                    pSf.Geometry = aPZWJS;
                    pSf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    string pzyt = FeatureHelper.GetFeatureStringValue(aPWJ, "PZYT");

                    IFeatureCursor dltbCursor = this.dltbClass.Search(pSf as IQueryFilter, true);
                    try
                    {
                        IFeature aDLTB = null;
                        while ((aDLTB = dltbCursor.NextFeature()) != null)
                        {
                            string zldwdm = FeatureHelper.GetFeatureStringValue(aDLTB, "ZLDWDM");
                            string zldwmc = FeatureHelper.GetFeatureStringValue(aDLTB, "ZLDWMC");
                            string dLBM = FeatureHelper.GetFeatureStringValue(aDLTB, "DLBM");
                            double dlmj =0;
                            if ((aPZWJS as IRelationalOperator).Contains(aDLTB.Shape))
                            {
                                dlmj = FeatureHelper.GetFeatureDoubleValue(aDLTB, "TBDLMJ");  //地类面积

                            }
                            else
                            {
                                double tkxs = FeatureHelper.GetFeatureDoubleValue(aDLTB, "KCXS");
                                string kcdlbm = FeatureHelper.GetFeatureStringValue(aDLTB, "KCDLBM"); //扣除地类编码
                                //交出来的面
                                ITopologicalOperator pTop = aPZWJS as ITopologicalOperator;
                                IGeometry pInterGeo = pTop.Intersect(aDLTB.Shape, esriGeometryDimension.esriGeometry2Dimension);
                                if (!pInterGeo.IsEmpty)
                                {
                                    IPoint selectPoint = (pInterGeo as IArea).Centroid;
                                    double X = selectPoint.X;
                                    int currDh = (int)(X / 1000000);////WK---带号
                                    double tbmj = area.SphereArea(pInterGeo, currDh);  //图斑面积
                                    dlmj = tbmj * (1 - tkxs);
                                    dlmj = MathHelper.RoundEx(dlmj, 2);

                                    
                                    if (kcdlbm.Trim() != "")
                                    {
                                        //扣除的这部分加进去
                                        double tkmj = MathHelper.RoundEx(tbmj * tkxs,2);
                                        //插入一条数据
                                        string sql = "insert into HZ_PZWJS_TMP(ZLDWDM,ZLDWMC,PZYT,XZYT,TBDLMJ) values('" + zldwdm + "','"
                                            + zldwmc + "','" + pzyt + "','" + kcdlbm + "'," + tkmj + " ) ";
                                        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    }
                                }
                            }
                            //插入一条数据
                            string sql2 = "insert into HZ_PZWJS_TMP(ZLDWDM,ZLDWMC,PZYT,XZYT,TBDLMJ) values('" + zldwdm + "','"
                                + zldwmc + "','" + pzyt + "','" + dLBM + "'," + dlmj + " ) ";
                            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql2);                            
                            
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(dltbCursor);
                    }
                    #endregion 

                   
                    

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCusor);
            }

            //转 换 汇总至 表 HZ_PZWJS_BZ
        }

        /// <summary>
        /// 汇总批准未建设 用途分类面积表
        /// </summary>
        public void InitPzwjsBZTable()
        {
            //批准未建设标准表
            string sql = "delete from HZ_PZWJS_BZ ";
            LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_PZWJS_BZ(ZLDWDM,ZLDWMC) select distinct ZLDWDM,ZLDWMC from HZ_PZWJS_JCB ");
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());
            sql = "select ZLDWDM,PZYT,sum(TDZMJ) as mj from HZ_PZWJS_JCB  where PZYT<>'' group by ZLDWDM,PZYT ";
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "mtp");
            foreach (DataRow dr in dt.Rows)
            {
                string zldwdm = dr["ZLDWDM"].ToString();
                double mj = 0;
                double.TryParse(dr["mj"].ToString(), out mj);
                string pzyt = dr["PZYT"].ToString().Trim().ToUpper();
                sql = "update HZ_PZWJS_BZ set D" + pzyt + " = " + mj + " where ZLDWDM='" + zldwdm + "' ";
                LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            }
            //计算小计合计
            //算小计
            sb = new StringBuilder();
            sb.Append("update HZ_PZWJS_BZ set D00=iif(isnull(D0603),0,D0603),D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append(" D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702),")
                .Append(" D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810),")
                .Append(" D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)")
                .Append("+iif(isnull(D1005),0,D1005)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append(" D11=D1109 ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //合计
            sb = new StringBuilder();
            sb.Append(" update HZ_PZWJS_BZ set DHJ=iif(isnull(D00),0,D00)+ iif(isnull(D05),0,D05)+iif(isnull(D06),0,D06)+iif(isnull(D07),0,D07)+iif(isnull(D08),0,D08)")
                .Append("+iif(isnull(D09),0,D09)+iif(isnull(D10),0,D10)+iif(isnull(D11),0,D11 ) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            ////汇总到乡
            //sb = new StringBuilder();
            //sb.Append("insert into HZ_PZWJS_BZ(ZLDWDM,DHJ,D00,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
            //    .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1007,D1008,D1009,D11,D1109 ) ")
            //    .Append("select left(ZLDWDM,12),sum(DHJ),sum(D00),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
            //    .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),")
            //.Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1007),sum(D1008),sum(D1009),sum(D11),sum(D1109)  ")
            //.Append(" from HZ_PZWJS_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
            //sql = sb.ToString();
            //iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("insert into HZ_PZWJS_BZ(ZLDWDM,DHJ,D00,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
                .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1007,D1008,D1009,D11,D1109 ) ")
                .Append("select left(ZLDWDM,9),sum(DHJ),sum(D00),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
                .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),")
            .Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1007),sum(D1008),sum(D1009),sum(D11),sum(D1109)  ")
            .Append(" from HZ_PZWJS_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("insert into HZ_PZWJS_BZ(ZLDWDM,DHJ,D00,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
                .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1007,D1008,D1009,D11,D1109 ) ")
                .Append("select left(ZLDWDM,6),sum(DHJ),sum(D00),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
                .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),")
            .Append("sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1007),sum(D1008),sum(D1009),sum(D11),sum(D1109)  ")
            .Append(" from HZ_PZWJS_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        /// <summary>
        /// 汇总批准未建设 占用现状分类面积表
        /// </summary>
        public void initPzwjsXzBzTable()
        {
            string sql = "delete from HZ_PZWJSXZ_BZ";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_PZWJSXZ_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
            .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508, D06,D0601,D0602,D0603,D07,D0701,D0702,")
            .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
            .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
            .Append("  select ZLDWDM,sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
            .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
            .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
            .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
            .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) from HZ_PZWJS_JCB ")
            .Append("group by ZLDWDM ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //sb = new StringBuilder();
            //sb.Append("insert into HZ_PZWJSXZ_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
            //.Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
            //.Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
            //.Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
            //.Append("select left(ZLDWDM,12),sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
            //.Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
            //.Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
            //.Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
            //.Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) ")
            //.Append(" from HZ_PZWJSXZ_BZ where len(ZLDWDM)=19 group by left(ZLDWDM,12)");
            //sql = sb.ToString();
            //iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sb = new StringBuilder();
            sb.Append("insert into HZ_PZWJSXZ_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
            .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
            .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
            .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
            .Append("select left(ZLDWDM,9),sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
            .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
            .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
            .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
            .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) ")
            .Append(" from HZ_PZWJSXZ_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,9)");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            

            sb = new StringBuilder();
            sb.Append("insert into HZ_PZWJSXZ_BZ(ZLDWDM,TDZMJ,D00,D01,D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,D03,D0301,D0302,D0303,D0304,D0305,D0306,D0307,")
            .Append("D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,D07,D0701,D0702,")
            .Append("D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D11,D1101,D1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,")
            .Append("D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
            .Append("select left(ZLDWDM,6),sum(TDZMJ) ,sum(D00),sum(D01),sum(D0101),sum(D0102),sum(D0103),")
            .Append("sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),")
            .Append("sum(D04),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),sum(D0602),sum(D0603),sum(D07),sum(D0701),sum(D0702),")
            .Append("sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D11),sum(D1101),sum(D1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),")
            .Append("sum(D12),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207) ")
            .Append(" from HZ_PZWJSXZ_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,6)");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        public void InitPzwjsJCB()  //批准未建设基础表
        {
            string sql = "delete from HZ_PZWJS_JCB";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into HZ_PZWJS_JCB(ZLDWDM,ZLDWMC,PZYT) select distinct ZLDWDM,ZLDWMC,PZYT from HZ_PZWJS_TMP ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "select ZLDWDM,PZYT,XZYT,sum(TBDLMJ) as mj from HZ_PZWJS_TMP group by ZLDWDM,PZYT,XZYT ";
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "yt");
            foreach (DataRow dr in dt.Rows)
            {
                string zldwdm = dr["ZLDWDM"].ToString();
                double mj = 0;               

                double.TryParse(dr["mj"].ToString(), out mj);
                mj = MathHelper.RoundEx(mj / 10000, 2);  //转化为公顷

                string pzyt = dr["PZYT"].ToString().Trim().ToUpper();
                string xzyt = dr["XZYT"].ToString().Trim().ToUpper();
                if (pzyt == "" || xzyt == "") continue;
                sql = "update HZ_PZWJS_JCB set D" + xzyt + " = " + mj + " where ZLDWDM='" + zldwdm + "' and PZYT='"+pzyt+"' ";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            }


            StringBuilder sb = new StringBuilder();
            sb.Append("update HZ_PZWJS_JCB set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
                .Append("D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                .Append("D02=iif(isnull(D0201),0,D0201)+iif(isnull(D0202),0,D0202)+iif(isnull(D0203),0,D0203)+iif(isnull(D0204),0,D0204),")
                .Append("D03=iif(isnull(D0301),0,D0301)+iif(isnull(D0302),0,D0302)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307),0,D0307),")
                .Append("D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403),0,D0403)+iif(isnull(D0404),0,D0404),")
                .Append("D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append("D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                .Append("D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810),")
                .Append("D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append("D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+")
                .Append("iif(isnull(D1104),0,D1104)+iif(isnull(D1107),0,D1107)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                .Append("D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207)  ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            sb = new StringBuilder();
            sb.Append("update HZ_PZWJS_JCB set TDZMJ=iif(isnull(D00),0,D00)+iif(isnull(D01),0,D01)+iif(isnull(D02),0,D02)+iif(isnull(D03),0,D03)+iif(isnull(D04),0,D04)")
                .Append("+iif(isnull(D05),0,D05)+iif(isnull(D06),0,D06)+iif(isnull(D07),0,D07)+ iif(isnull(D08),0,D08) ")
                .Append("+iif(isnull(D09),0,D09)+iif(isnull(D10),0,D10)+iif(isnull(D11),0,D11)+iif(isnull(D12),0,D12) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

    
         
            

        }


        



    }
}
