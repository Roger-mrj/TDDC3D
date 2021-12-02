using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using RCIS.Utility;
using ESRI.ArcGIS.Carto;
//using Microsoft.Office.Interop.Excel;
using ESRI.ArcGIS.Controls;

namespace TDDC3D.gengxin
{
    public partial class FrmDealAttribute : Form
    {
        public FrmDealAttribute()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        public IMapControl3 mapctl = null;
        
        private Dictionary<string, string> getMCS(string table)
        {
            Dictionary<string, string> dicDlbm = new Dictionary<string, string>();
            System.Data.DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from " + table, "tmp");
            foreach (DataRow dr in dt.Rows)
            {
                dicDlbm.Add(dr["DM"].ToString(), dr["MC"].ToString());
            }
            return dicDlbm;
        }

        private void outErro_Click(object sender, EventArgs e)
        {
            //导出错误日志
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destName = dlg.FileName;
            this.gridControl1.ExportToXls(destName);
            MessageBox.Show("导出完毕!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            
            
        }

        private void gridControl1_ProcessGridKey_1(object sender, KeyEventArgs e)
        {
            DevExpress.XtraGrid.Views.Base.ColumnView view = (sender as DevExpress.XtraGrid.GridControl).FocusedView as DevExpress.XtraGrid.Views.Base.ColumnView;
            if (view == null) return;
            if (e.KeyCode == Keys.Delete && e.Control && view.OptionsBehavior.AllowDeleteRows!= DevExpress.Utils.DefaultBoolean.False && view.SelectedRowsCount > 0)
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("确定删除所选行？", "提问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    view.DeleteSelectedRows();
            }
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            //try
            //{
                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("数据准备", "正在处理，请稍等...");
                wait.Show();
                bool bGD = false;
                bool bXZ = false;
                IFeatureClass pDLTBFeaClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
                Dictionary<string, double> arr = new Dictionary<string, double>();
                //for (int j = 2; j < 6; j++)
                //{
                //    IQueryFilter pQF=new QueryFilterClass();
                //    pQF.WhereClause="GDLX='TT' AND GDPDJB='"+j+"'";
                //    ArrayList arrValue = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pDLTBFeaClass,pQF,"KCDLXS");
                //    if (arrValue.Count > 0)
                //        arr.Add("TT"+j,double.Parse(arrValue[0].ToString()));
                //    pQF.WhereClause = "GDLX='PD' AND GDPDJB='" + j + "'";
                //    arrValue = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pDLTBFeaClass, pQF, "KCDLXS");
                //    if (arrValue.Count > 0)
                //        arr.Add("PD" + j, double.Parse(arrValue[0].ToString()));
                //}
                Dictionary<string, string> qsdw = new Dictionary<string, string>();
                IFeatureCursor pFeaCursor = pDLTBFeaClass.Search(null, true);
                IFeature pFeature;
                while ((pFeature = pFeaCursor.NextFeature()) != null)
                {
                    string qsdwdm = pFeature.get_Value(pDLTBFeaClass.FindField("QSDWDM")).ToString();
                    string qsdwmc = pFeature.get_Value(pDLTBFeaClass.FindField("QSDWMC")).ToString();
                    if (!qsdw.Keys.Contains(qsdwdm))
                        qsdw.Add(qsdwdm, qsdwmc);
                    string gdlx = pFeature.get_Value(pDLTBFeaClass.FindField("GDLX")).ToString().Trim();
                    string gdpdjb = pFeature.get_Value(pDLTBFeaClass.FindField("GDPDJB")).ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(gdlx) && !string.IsNullOrWhiteSpace(gdpdjb))
                    {
                        if (!arr.Keys.Contains(gdlx + gdpdjb))
                            arr.Add(gdlx + gdpdjb, double.Parse(pFeature.get_Value(pDLTBFeaClass.FindField("KCXS")).ToString()));
                    }
                }
                wait.SetCaption("正在完善地类名称等...");
                Dictionary<string, string> dicDlbm = getMCS("三调工作分类");
                foreach (KeyValuePair<string, string> aItem in dicDlbm)
                {
                    string sql = "update DLTBGX set DLMC='" + aItem.Value.Trim() + "' where DLBM='" + aItem.Key.Trim() + "'";
                    currWs.ExecuteSQL(sql);
                }
                Dictionary<string, string> dicsTBXHDM = getMCS("DIC_38图斑细化类型代码表");
                foreach (KeyValuePair<string, string> aItem in dicsTBXHDM)
                {
                    string sql = "update DLTBGX set TBXHMC='" + aItem.Value.Trim() + "' where TBXHDM='" + aItem.Key.Trim() + "'";
                    currWs.ExecuteSQL(sql);
                }
                Dictionary<string, string> dics = getMCS("DIC_39种植属性代码表");
                foreach (KeyValuePair<string, string> aItem in dics)
                {
                    string sql = "update DLTBGX set ZZSXMC='" + aItem.Value.Trim() + "' where ZZSXDM='" + aItem.Key.Trim() + "'";
                    currWs.ExecuteSQL(sql);
                }

                try
                {
                    DataTable dtdm = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select YSDM from SYS_YSDM where CLASSNAME='DLTBGX'", "tmp");
                    string YSDM = "update DLTBGX set YSDM='" + dtdm.Rows[0][0] + "'";
                    currWs.ExecuteSQL(YSDM);
                }
                catch { }
                string[] jkgcArr = { "0201", "0202", "0203", "0204", "0301", "0302", "0305", "0307", "0201K", "0202K", "0203K", "0204K", "0301K", "0302K", "0307K","0403K","0404","1104","1104K","1104A" };
                IFeatureClass pGXFeaClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
                IFeatureCursor pGXCursor = pGXFeaClass.Update(null, true);
                IFeature pGXFeature;
                int count = (pGXFeaClass as ITable).RowCount(null);
                int num = 1;
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("要素ID");
                dt.Columns.Add("错误问题");
                while ((pGXFeature = pGXCursor.NextFeature()) != null)
                {
                    wait.SetCaption("" + num++ + "/" + count + "");

                    //判断即可恢复和工程恢复
                    string zzsxdm = pGXFeature.get_Value(pGXFeature.Fields.FindField("ZZSXDM")).ToString().Trim();
                    if (zzsxdm == "JKHF" || zzsxdm == "GCHF")
                    { 
                        string dl=pGXFeature.get_Value(pGXFeature.Fields.FindField("DLBM")).ToString().Trim();
                        if (!jkgcArr.Contains(dl))
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                            dr[1] = "地类编码或种植属性代码错误";
                            dt.Rows.Add(dr);
                        }
                    }

                    ESRI.ArcGIS.Geometry.IPoint pPoint = (pGXFeature.ShapeCopy as IArea).LabelPoint;
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = pPoint;
                    pSF.GeometryField = pDLTBFeaClass.ShapeFieldName;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
                    IFeatureCursor pDLTBCursor = pDLTBFeaClass.Search(pSF, true);
                    IFeature pDLTBFeature = pDLTBCursor.NextFeature();
                    if (pDLTBFeature != null)
                    {
                        string dlbm = pGXFeature.get_Value(pGXFeature.Fields.FindField("DLBM")).ToString();
                        string dlmc = pGXFeature.get_Value(pGXFeature.Fields.FindField("DLMC")).ToString();
                        string tbxhdm = pGXFeature.get_Value(pGXFeature.Fields.FindField("TBXHDM")).ToString();
                        string TBXHMC = pGXFeature.get_Value(pGXFeature.Fields.FindField("TBXHMC")).ToString();
                        string ZZSXDM = pGXFeature.get_Value(pGXFeature.Fields.FindField("ZZSXDM")).ToString();
                        string ZZSXMC = pGXFeature.get_Value(pGXFeature.Fields.FindField("ZZSXMC")).ToString();
                        if (string.IsNullOrWhiteSpace(dlbm) || string.IsNullOrWhiteSpace(dlmc))
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                            dr[1] = "地类编码错误";
                            dt.Rows.Add(dr);
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(tbxhdm) && string.IsNullOrWhiteSpace(TBXHMC))
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                            dr[1] = "图斑细化代码错误";
                            dt.Rows.Add(dr);
                        }
                        if (!string.IsNullOrWhiteSpace(ZZSXDM) && string.IsNullOrWhiteSpace(ZZSXMC))
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                            dr[1] = "种植属性代码错误";
                            dt.Rows.Add(dr);
                        }
                        if (dlbm.Substring(0, 2) == "01")
                        {
                            if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("GDPDJB")).ToString()) || string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("ZZSXDM")).ToString()))
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                dr[1] = "耕地未填耕地坡度级别或种植属性代码";
                                dt.Rows.Add(dr);
                            }
                            if (!string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("XZDWKD")).ToString().Trim()) && pGXFeature.get_Value(pGXFeaClass.FindField("XZDWKD")).ToString().Trim() != "0")
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                dr[1] = "非线状地物存在线状地物属性";
                                dt.Rows.Add(dr);
                                bXZ = true;
                            }
                            int gdpdjb = 0;
                            int.TryParse(pGXFeature.get_Value(pGXFeature.Fields.FindField("GDPDJB")).ToString().Trim(), out gdpdjb);
                            if (gdpdjb > 1)
                            {
                                if (pGXFeature.get_Value(pGXFeature.Fields.FindField("GDLX")).ToString().Trim() != "PD" && pGXFeature.get_Value(pGXFeature.Fields.FindField("GDLX")).ToString().Trim() != "TT")
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                    dr[1] = "耕地类型填写错误";
                                    dt.Rows.Add(dr);
                                }
                                string gdlx = pGXFeature.get_Value(pGXFeature.Fields.FindField("GDLX")).ToString().Trim() + gdpdjb.ToString();
                                if (arr.Keys.Contains(gdlx))
                                {
                                    pGXFeature.set_Value(pGXFeature.Fields.FindField("KCXS"), arr[gdlx]);
                                    pGXFeature.set_Value(pGXFeature.Fields.FindField("KCDLBM"), "1203");
                                }
                                else
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                    dr[1] = "未找到对应耕地坡度级别，无法填写扣除地类系数";
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                        else
                        {
                            if ((!string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("KCDLBM")).ToString().Trim()) && pGXFeature.get_Value(pGXFeaClass.FindField("KCDLBM")).ToString().Trim() != "0")
                                || (!string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("GDLX")).ToString().Trim()) && pGXFeature.get_Value(pGXFeaClass.FindField("GDLX")).ToString().Trim() != "0")
                                || !string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("GDPDJB")).ToString().Trim())
                                || (!string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("KCXS")).ToString().Trim()) && pGXFeature.get_Value(pGXFeaClass.FindField("KCXS")).ToString().Trim() != "0")
                                || (!string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("KCMJ")).ToString().Trim()) && pGXFeature.get_Value(pGXFeaClass.FindField("KCMJ")).ToString().Trim() != "0"))
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                dr[1] = "非耕地存在耕地属性";
                                dt.Rows.Add(dr);
                                bGD = true;
                            }

                            if ((dlbm == "1001" || dlbm == "1002" || dlbm == "1003" || dlbm == "1004" || dlbm == "1006" || dlbm == "1009"
                                || dlbm == "1107" || dlbm == "1101" || dlbm == "1107A"))
                            {
                                //double kdmj=0;
                                //double.TryParse(pGXFeature.get_Value(pGXFeature.Fields.FindField("XZDWKD")).ToString(),out kdmj);
                                //if (kdmj <= 0)
                                //{
                                //    double mj = (pGXFeature.Shape as IArea).Area;
                                //    double len = (pGXFeature.Shape as IPolygon).Length;
                                //    len = len / 2;
                                //    double kd = mj / len;
                                //    pGXFeature.set_Value(pGXFeature.Fields.FindField("XZDWKD"), MathHelper.RoundEx(kd, 1));
                                //}
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeaClass.FindField("XZDWKD")).ToString().Trim()) && pGXFeature.get_Value(pGXFeaClass.FindField("XZDWKD")).ToString().Trim() != "0")
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                    dr[1] = "非线状地物存在线状地物属性";
                                    dt.Rows.Add(dr);
                                    bXZ = true;
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("QSXZ")).ToString().Trim()))
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("QSXZ"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("QSXZ")));

                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("QSDWDM")).ToString().Trim()))
                        {
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("QSDWDM"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("QSDWDM")));
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("QSDWMC"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("QSDWMC")));
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("QSDWMC")).ToString().Trim()))
                            {
                                if (qsdw.Keys.Contains(pGXFeature.get_Value(pGXFeature.Fields.FindField("QSDWDM")).ToString().Trim()))
                                    pGXFeature.set_Value(pGXFeature.Fields.FindField("QSDWMC"), qsdw[pGXFeature.get_Value(pGXFeature.Fields.FindField("QSDWDM")).ToString().Trim()]);
                                else
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = pGXFeature.get_Value(pGXFeature.Fields.FindField("OBJECTID")).ToString();
                                    dr[1] = "未找到对应权属单位代码的权属单位名称";
                                    dt.Rows.Add(dr);
                                }
                            }

                        }

                        //if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("QSDWMC")).ToString().Trim()))
                        //    pGXFeature.set_Value(pGXFeature.Fields.FindField("QSDWMC"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("QSDWMC")));

                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("ZLDWDM")).ToString().Trim()))
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("ZLDWDM"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("ZLDWDM")));

                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("ZLDWMC")).ToString().Trim()))
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("ZLDWMC"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("ZLDWMC")));

                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("MSSM")).ToString().Trim()))
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("MSSM"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("MSSM")));

                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("HDMC")).ToString().Trim()))
                            pGXFeature.set_Value(pGXFeature.Fields.FindField("HDMC"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("HDMC")));

                        //if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("SJNF")).ToString().Trim()) || pGXFeature.get_Value(pGXFeature.Fields.FindField("SJNF")).ToString().Trim() == "0")
                        //    pGXFeature.set_Value(pGXFeature.Fields.FindField("SJNF"), pDLTBFeature.get_Value(pDLTBFeature.Fields.FindField("SJNF")));

                        //if(string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("GDDB")).ToString().Trim())||pGXFeature.get_Value(pGXFeature.Fields.FindField("GDDB")).ToString().Trim()=="0")
                        //    pGXFeature.set_Value(pGXFeature.Fields.FindField("GDDB"), DBNull.Value);
                        if (string.IsNullOrWhiteSpace(pGXFeature.get_Value(pGXFeature.Fields.FindField("FRDBS")).ToString().Trim()))
                        {
                            if (pGXFeature.get_Value(pGXFeature.Fields.FindField("ZLDWDM")).ToString().Trim().Substring(0, 12) != pGXFeature.get_Value(pGXFeature.Fields.FindField("QSDWDM")).ToString().Trim().Substring(0, 12)
                            && pGXFeature.get_Value(pGXFeature.Fields.FindField("QSXZ")).ToString().Substring(0, 1) != "1" && pGXFeature.get_Value(pGXFeature.Fields.FindField("QSXZ")).ToString().Substring(0, 1) != "2")
                                pGXFeature.set_Value(pGXFeature.Fields.FindField("FRDBS"), "1");
                            else
                                pGXFeature.set_Value(pGXFeature.Fields.FindField("FRDBS"), "0");
                        }
                        

                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBCursor);
                    pGXCursor.UpdateFeature(pGXFeature);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pGXCursor);

                gridControl1.DataSource = dt;
                gridView1.Columns[1].Width = 190;

                if (bXZ == true)
                    simpleButton2.Enabled = true;
                if (bGD == true)
                    simpleButton1.Enabled = true;
                if (bXZ == false)
                    simpleButton2.Enabled = false;
                if (bGD == false)
                    simpleButton1.Enabled = false;
                wait.Close();
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, ex.ToString());
            //    return;
            //}
        }

        private void btnNO_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "update DLTBGX SET GDLX='',GDPDJB='',KCXS=0,KCDLBM ='' where DLBM not in ('0101','0102','0103')";
            currWs.ExecuteSQL(sql);
            sql = "update DLTBGX SET ZZSXDM='',ZZSXMC='' where ((DLBM<>'0101' and DLBM<>'0102' and DLBM<>'0103') and (ZZSXDM='LS' or ZZSXDM='FLS' or ZZSXDM='LYFL' or ZZSXDM='XG' or ZZSXDM='LLJZ' or ZZSXDM='WG'))";
            currWs.ExecuteSQL(sql);
            //sql = "update DLTBGX SET ZZSXDM='',ZZSXMC='' where (DLBM<>'0101' and DLBM<>'0102' and DLBM<>'0103') and (ZZSXDM='LS' or ZZSXDM='FLS' or ZZSXDM='LYFL' or ZZSXDM='XG' or ZZSXDM='LLJZ' or ZZSXDM='WG')";
            //currWs.ExecuteSQL(sql);
            MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "update DLTBGX SET XZDWKD =0 where DLBM<>'1001' and DLBM<>'1002' and DLBM<>'1003' and DLBM<>'1002' and DLBM<>'1003' and DLBM<>'1002' and DLBM<>'1003' and DLBM<>'1004' and DLBM<>'1006' and DLBM<>'1009' and DLBM<>'1107' and DLBM<>'1101' and DLBM<>'1107A'";
            currWs.ExecuteSQL(sql);
            MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void FrmDealAttribute_Load(object sender, EventArgs e)
        {

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int selectRow = gridView1.GetSelectedRows()[0];
                string oid = this.gridView1.GetRowCellValue(selectRow, "要素ID").ToString();
                IFeatureWorkspace pFW = currWs as IFeatureWorkspace;
                IFeatureClass pClass = pFW.OpenFeatureClass("DLTBGX");
                IFeature pFeature = pClass.GetFeature(int.Parse(oid));
                ITopologicalOperator ptop = pFeature.ShapeCopy as ITopologicalOperator;
                IGeometry buffGeo = ptop.Buffer(1);
                IEnvelope env = buffGeo.Envelope;
                env.Expand(1.5, 1.5, true);
                this.mapctl.ActiveView.Extent = env;
                this.mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapctl.ActiveView.Extent);
                this.mapctl.ActiveView.ScreenDisplay.UpdateWindow();
                this.mapctl.FlashShape(buffGeo, 3, 300, null);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理","请稍后...");
            wait.Show();
            IFeatureClass pDltbgx = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            IFeatureCursor pCursor = pDltbgx.Update(null,true);
            IFeature pFeature;
            int count = pDltbgx.FeatureCount(null);
            int currCount = 1;
            while((pFeature=pCursor.NextFeature())!=null)
            {
                wait.SetCaption(currCount+++"/"+count);
                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    if (!pFeature.Fields.get_Field(i).Name.ToUpper().Contains("SHAPE") && pFeature.Fields.get_Field(i).Name.ToUpper() != "OBJECTID" && pFeature.get_Value(i).ToString().Contains(" "))
                    {
                        object val = pFeature.get_Value(i).ToString().Trim();
                        pFeature.set_Value(i,val);
                    }
                }
                pCursor.UpdateFeature(pFeature);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            wait.Close();
            MessageBox.Show("处理完毕","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        
    
    }
}
