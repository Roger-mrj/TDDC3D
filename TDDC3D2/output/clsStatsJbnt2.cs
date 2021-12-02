using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using RCIS.Utility;
using RCIS.Database;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using ESRI.ArcGIS.Geometry;


namespace TDDC3D.output
{
    /// <summary>
    /// 统计永久基本农田
    /// </summary>
    public class clsStatsJbnt2
    {
        private IWorkspace currWs = null;
        private IFeatureClass yjjbnttbFeatureClass = null;
        private IFeatureClass dltbClass = null;

        public clsStatsJbnt2(IWorkspace _ws)
        {
            try
            {
                this.currWs = _ws;
                IFeatureWorkspace pfeaWs = this.currWs as IFeatureWorkspace;
                dltbClass = pfeaWs.OpenFeatureClass("DLTB");
                yjjbnttbFeatureClass = pfeaWs.OpenFeatureClass("YJJBNTTB");
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


        public void InitYjjbnt()
        {
            string sql = "delete from HZ_YJJBNT";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "delete from HZ_YJJBNT_BZ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            List<string> lstDlbm = sys.YWCommonHelper.getOnlyDlbm();

            sql = @"Transform round(sum(TBDLMJ / 10000),2) select zldwdm,zldwmc from HZ_YJJBNT_TMP group by zldwdm,zldwmc Pivot ""D"" + Left(XZYT,4)";
            DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "jbnt");
                        
            foreach (DataRow dr in dt.Rows)
            {
                string cols = "";
                string values = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() == "ZLDWDM" || dt.Columns[i].ColumnName.ToUpper() == "ZLDWMC" || lstDlbm.Contains(dt.Columns[i].ColumnName.Replace("D","")))
                    {
                        cols += dt.Columns[i].ColumnName + ",";
                        if (dt.Columns[i].DataType == typeof(string))
                        {
                            values += string.IsNullOrWhiteSpace(dr[i].ToString()) ? @"""""," : @"""" + dr[i].ToString() + @""",";
                        }
                        else
                        {
                            values += string.IsNullOrWhiteSpace(dr[i].ToString()) ? "0," : dr[i].ToString() + ",";
                        }
                    }
                }
                sql = "Insert Into HZ_YJJBNT_BZ (" + cols.Substring(0, cols.Length - 1) + ") Values (" + values.Substring(0, values.Length - 1) + ")";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            }

            //
            //Dictionary<string, string> dicZldw = this.getZldwdmMc();
            //foreach (KeyValuePair<string, string> aItem in dicZldw)
            //{
            //    if (aItem.Key.Trim().Length != 12)
            //        continue;
            //    //sql = "insert into HZ_YJJBNT_BZ(ZLDWDM,ZLDWMC) values('" + aItem.Key + "','" + aItem.Value + "' ) ";
            //    //iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //    ////统计下面的 各地类
            //    //foreach (string adl in lstDlbm)
            //    //{
            //    //    double summj = 0;
            //    //    FeatureHelper.StatsFieldSumValue(yjjbnttbFeatureClass, "DLBM like '" + adl + "%' and ZLDWDM like '" + aItem.Key + "%'", "YJJBNTMJ");

            //    //    sql = "update HZ_YJJBNT_BZ set " + adl.ToUpper() + "=" + summj + " where ZLDWDM='" + aItem.Key + "'";
            //    //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //    //}
                
            //}

            //计算小计
            //计算小分类合计
            StringBuilder sb = new StringBuilder();
            sb.Append("update HZ_YJJBNT_BZ set D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
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

            //插入到 HZ_YJJBNT 中
            sb.Clear();
            sb.Append("insert into HZ_YJJBNT(ZLDWDM,ZLDWMC,D01,D0101,D0102,D0103,D00,D02,D03,D04,D05,D06,D07,D08,D09,D10,D11,D12) select ZLDWDM,ZLDWMC,D01,D0101,D0102,D0103,D00,D02,D03,D04,D05,D06,D07,D08,D09,D10,D11,D12 ")
                .Append(" FROM HZ_YJJBNT_BZ ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //计算小计合计
            sb.Clear();
            sb.Append("update HZ_YJJBNT set DQT=iif(isnull(D00),0,D00)+iif(isnull(D02),0,D02)+iif(isnull(D03),0,D03)+iif(isnull(D04),0,D04)+iif(isnull(D05),0,D05)+iif(isnull(D06),0,D06)")
                .Append("+iif(isnull(D07),0,D07)+iif(isnull(D08),0,D08)+iif(isnull(D09),0,D09)+iif(isnull(D10),0,D10)+iif(isnull(D11),0,D11)+iif(isnull(D12),0,D12) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append("update HZ_YJJBNT set YJJBNTMJ=DQT+D01 ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //逐级汇总
            //sb.Clear();
            //sb.Append("insert into HZ_YJJBNT(ZLDWDM,YJJBNTMJ,D01,D0101,D0102,D0103,DQT,D00,D02,D03,D04,D05,D06,D07,D08,D09,D10,D11,D12) ")
            //    .Append(" select left(ZLDWDM,12),sum(YJJBNTMJ),sum(D01),sum(D0101),sum(D0102),sum(D0103),sum(DQT),sum(D00),sum(D02),sum(D03),sum(D04),sum(D05),sum(D06),sum(D07),sum(D08),sum(D09),sum(D10),sum(D11),sum(D12) ")
            //    .Append(" from HZ_YJJBNT where len(ZLDWDM)=19 group by left(ZLDWDM,12) ");
            //sql = sb.ToString();
            //iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //逐级汇总
            sb.Clear();
            sb.Append("insert into HZ_YJJBNT(ZLDWDM,YJJBNTMJ,D01,D0101,D0102,D0103,DQT,D00,D02,D03,D04,D05,D06,D07,D08,D09,D10,D11,D12) ")
                .Append(" select left(ZLDWDM,12),sum(YJJBNTMJ),sum(D01),sum(D0101),sum(D0102),sum(D0103),sum(DQT),sum(D00),sum(D02),sum(D03),sum(D04),sum(D05),sum(D06),sum(D07),sum(D08),sum(D09),sum(D10),sum(D11),sum(D12) ")
                .Append(" from HZ_YJJBNT group by left(ZLDWDM,12) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear();
            sb.Append("insert into HZ_YJJBNT(ZLDWDM,YJJBNTMJ,D01,D0101,D0102,D0103,DQT,D00,D02,D03,D04,D05,D06,D07,D08,D09,D10,D11,D12) ")
                .Append(" select left(ZLDWDM,9),sum(YJJBNTMJ),sum(D01),sum(D0101),sum(D0102),sum(D0103),sum(DQT),sum(D00),sum(D02),sum(D03),sum(D04),sum(D05),sum(D06),sum(D07),sum(D08),sum(D09),sum(D10),sum(D11),sum(D12) ")
                .Append(" from HZ_YJJBNT where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear();
            sb.Append("insert into HZ_YJJBNT(ZLDWDM,YJJBNTMJ,D01,D0101,D0102,D0103,DQT,D00,D02,D03,D04,D05,D06,D07,D08,D09,D10,D11,D12) ")
                .Append(" select left(ZLDWDM,6),sum(YJJBNTMJ),sum(D01),sum(D0101),sum(D0102),sum(D0103),sum(DQT),sum(D00),sum(D02),sum(D03),sum(D04),sum(D05),sum(D06),sum(D07),sum(D08),sum(D09),sum(D10),sum(D11),sum(D12) ")
                .Append(" from HZ_YJJBNT where len(ZLDWDM)=9 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //foreach (KeyValuePair<string, string> aItem in dicZldw)
            //{
            //    sql = "update HZ_YJJBNT set ZLDWMC='" + aItem.Value + "' where ZLDWDM='" + aItem.Key + "'";
            //    iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //}

        }

        public void getYJJBNTTmp()
        {

            RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery("delete from HZ_YJJBNT_TMP");
            //分村 统计 批准未建设土地，换算为公顷，写入 统计表
            IFeatureCursor pFeaCusor = yjjbnttbFeatureClass.Search(null, false);
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
                    //string pzyt = FeatureHelper.GetFeatureStringValue(aPWJ, "PZYT");

                    IFeatureCursor dltbCursor = this.dltbClass.Search(pSf as IQueryFilter, true);
                    try
                    {
                        IFeature aDLTB = null;
                        while ((aDLTB = dltbCursor.NextFeature()) != null)
                        {
                            string zldwdm = FeatureHelper.GetFeatureStringValue(aDLTB, "ZLDWDM");
                            string zldwmc = FeatureHelper.GetFeatureStringValue(aDLTB, "ZLDWMC");
                            string dLBM = FeatureHelper.GetFeatureStringValue(aDLTB, "DLBM");
                            //string kcdlbm = FeatureHelper.GetFeatureStringValue(aDLTB, "KCDLBM"); //扣除地类编码
                            double dlmj = 0;
                            if ((aPZWJS as IRelationalOperator).Contains(aDLTB.Shape))
                            {
                                dlmj = FeatureHelper.GetFeatureDoubleValue(aDLTB, "TBDLMJ");  //地类面积
                                //if (kcdlbm.Trim() != "")
                                //{
                                //    //扣除的这部分加进去
                                //    double tkmj = FeatureHelper.GetFeatureDoubleValue(aDLTB, "KCMJ");
                                //    //插入一条数据
                                //    string sql = "insert into HZ_YJJBNT_TMP(ZLDWDM,ZLDWMC,XZYT,TBDLMJ) values('" + zldwdm + "','"
                                //        + zldwmc + "','" + kcdlbm + "'," + tkmj + " ) ";
                                //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                //}
                            }
                            else
                            {
                                double tkxs = FeatureHelper.GetFeatureDoubleValue(aDLTB, "KCXS");
                                //交出来的面
                                ITopologicalOperator pTop = aPZWJS as ITopologicalOperator;
                                IGeometry pInterGeo = pTop.Intersect(aDLTB.Shape, esriGeometryDimension.esriGeometry2Dimension);
                                if (!pInterGeo.IsEmpty)
                                {
                                    //IPoint selectPoint = (pInterGeo as IArea).Centroid;
                                    //double X = selectPoint.X;
                                    //int currDh = (int)(X / 1000000);////WK---带号
                                    //double tbmj = area.SphereArea(pInterGeo, currDh);  //图斑面积
                                    IArea pAreaIntersect = pInterGeo as IArea;
                                    IArea pAreaDLTB = aDLTB.ShapeCopy as IArea;
                                    double tbmj = pAreaIntersect.Area / pAreaDLTB.Area * FeatureHelper.GetFeatureDoubleValue(aDLTB, "TBDLMJ");
                                    dlmj = MathHelper.RoundEx(tbmj, 2);
                                    //dlmj = tbmj * (1 - tkxs);
                                    //dlmj = MathHelper.RoundEx(dlmj, 2);

                                    //if (kcdlbm.Trim() != "")
                                    //{
                                    //    //扣除的这部分加进去
                                    //    double tkmj = MathHelper.RoundEx(tbmj * tkxs, 2);
                                    //    //插入一条数据
                                    //    string sql = "insert into HZ_YJJBNT_TMP(ZLDWDM,ZLDWMC,XZYT,TBDLMJ) values('" + zldwdm + "','"
                                    //        + zldwmc + "','" + kcdlbm + "'," + tkmj + " ) ";
                                    //    LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                                    //}
                                }
                            }
                            //插入一条数据
                            string sql2 = "insert into HZ_YJJBNT_TMP(ZLDWDM,ZLDWMC,XZYT,TBDLMJ) values('" + zldwdm + "','"
                                + zldwmc + "','" + dLBM + "'," + dlmj + " ) ";
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

    }
}
