using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.Database;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace TDDC3D.analysis
{
    public partial class KcdjfxForm : Form
    {
        public KcdjfxForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private void KcdjfxForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbDltbLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);

            int idx = 0;

            for (int i = 0; i < this.cmbDltbLayer.Properties.Items.Count; i++)
            {
                string name = OtherHelper.GetLeftName(this.cmbDltbLayer.Properties.Items[i].ToString());
                if (name.ToUpper().Equals("DLTB"))
                {
                    idx = i;
                    break;
                }
            }

            this.cmbDltbLayer.SelectedIndex = idx;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            dlg.FileName = "计算结果.xls";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.gridControl1.ExportToExcelOld(dlg.FileName);
            MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private Dictionary<string, string> getDLDMMC()
        {
            Dictionary<string, string> dicDlbm = new Dictionary<string, string>();
            DataTable dt = LS_SetupMDBHelper.GetDataTable("select * from 三调工作分类", "tmp");
            foreach (DataRow dr in dt.Rows)
            {
                dicDlbm.Add(dr["DM"].ToString(), dr["MC"].ToString());
            }
            return dicDlbm;
        }

        private void CalDltbmj(IGeometry pGeoExtent)
        {
            LS_ResultMDBHelper.ExecuteSQLNonquery("delete from SYS_ConfirmArea_tmp");

            IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbDltbLayer.Text.Trim()));
            IFeatureClass dltbClass = dltbLayer.FeatureClass;

            
            ISpatialFilter spFilter = new SpatialFilterClass();
            spFilter.Geometry = pGeoExtent;
            spFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor cursor = dltbClass.Search(spFilter, true);

            IRelationalOperator pRelation = pGeoExtent as IRelationalOperator;
            ITopologicalOperator pTopo = pGeoExtent as ITopologicalOperator;
            IFeature aFeature = null;
            try
            {
                while ((aFeature = cursor.NextFeature()) != null)
                {
                    double dlmj =0;
                    double kcxs = FeatureHelper.GetFeatureDoubleValue(aFeature, "KCXS");    
                    double kcmj=0;
                    if (pRelation.Contains(aFeature.Shape))
                    {
                        //完全包含的部分
                        dlmj = FeatureHelper.GetFeatureDoubleValue(aFeature, "TBDLMJ");
                    }
                    else
                    {
                        //相较于一个面的
                        IGeometry geoDltb = pTopo.Intersect(aFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        if (geoDltb.IsEmpty) continue;
                        //计算椭球面积
                        IPoint selectPoint = (geoDltb as IArea).Centroid;
                        double X = selectPoint.X;
                        int currDh = (int)(X / 1000000);////WK---带号    
                        SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                        double tbmj = area.SphereArea(geoDltb, currDh);
                        //看是否有扣除
                        if (kcxs > 0)
                        {
                            dlmj =MathHelper.Round( tbmj * (1 - kcxs),2);
                            kcmj=MathHelper.Round(tbmj*kcxs,2);
                        }
                       

                    }

                    string dlbm = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                    string dlmc = FeatureHelper.GetFeatureStringValue(aFeature, "DLMC");
                    string qsxz = FeatureHelper.GetFeatureStringValue(aFeature, "QSXZ");
                    if (qsxz.ToUpper().StartsWith("1") || qsxz.ToUpper().StartsWith("2"))
                    {
                        //国有
                        string sql = "insert into SYS_ConfirmArea_tmp(dlbm,dlmc,GY,JT) values ('" + dlbm + "','" + dlmc + "'," + dlmj + ",0) ";
                        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                        //如果 有扣除，则加一个田坎的
                        if (kcxs>0)
                        {
                            sql = "insert into SYS_ConfirmArea_tmp(dlbm,dlmc,GY,JT) values('1203','田坎'," + kcmj + ",0) ";
                            LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                        }
                            
                        
                    }
                    else
                    {
                        string sql = "insert into SYS_ConfirmArea_tmp(dlbm,dlmc,GY,JT) values ('" + dlbm + "','" + dlmc + "',0," + dlmj + ") ";
                        LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                        if (kcxs > 0)
                        {
                            sql = "insert into SYS_ConfirmArea_tmp(dlbm,dlmc,GY,JT) values('1203','田坎',0,"+kcmj+") ";
                            LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                        }

                    }
                    

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            }


        }

        private void SUMMJ()
        {
            //汇总
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery("delete from sys_confirmarea ");
            string sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT) select dlbm,dlmc,sum(GY),sum(JT) from SYS_ConfirmArea_tmp group by dlbm,dlmc ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "update SYS_ConfirmArea set zmj=iif(isnull(gy),0,gy)+iif(isnull(jt),0,jt) ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //各一级类
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '01','耕地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='01' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '02','园地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='02' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '03','林地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='03' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '04','草地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='04' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '05','商服用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='05' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '06','工业仓储用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='06' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '07','住宅用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='07' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '08','公共管理与公共服务用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='08' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '09','特殊用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='09' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '10','交通运输用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='10' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '11','水域及水利设施用地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='11' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "insert into SYS_ConfirmArea(dlbm,dlmc,GY,JT,zmj) select '12','其他土地',sum(gy),sum(jt),sum(zmj) from SYS_ConfirmArea where left(dlbm,2)='12' ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbDltbLayer.Text.Trim() == "") return;
            if (cmbLayers.Text.Trim().Equals(""))
            {
                ////WK---若界线面层选择框内容为空，返回
                MessageBox.Show("请选择作为界线的面层");
                return;
            }
            //获取 图层 要素
            IFeatureLayer pKjLayer= LayerHelper.QueryLayerByModelName(this.currMap,OtherHelper.GetLeftName(cmbLayers.Text.Trim()));
            IFeatureClass pFeaCls = pKjLayer.FeatureClass;
            IFeature selFeature = GetFeaturesHelper.GetFirstFeature(pFeaCls, null);
            if (selFeature == null)
            {
                MessageBox.Show("图层中没有要素！");
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始提取数据...", "稍等");
            wait.Show();
            try
            {
                CalDltbmj(selFeature.Shape);
                wait.SetCaption("开始汇总结果...");
                SUMMJ();
                wait.SetCaption("正在加载...");
                DataTable dt=LS_ResultMDBHelper.GetDataTable("select * from sys_confirmArea order by dlbm","area");
                this.gridControl1.DataSource=dt;
                wait.Close();
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }


        }
    }
}
