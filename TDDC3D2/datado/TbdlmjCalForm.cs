using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Utility;
using RCIS.GISCommon;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using System.Data;
using System.Runtime.InteropServices;

namespace TDDC3D.datado
{
    public partial class TbdlmjCalForm : Form
    {
        public TbdlmjCalForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;

        private void TbdlmjCalForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap);
            int idx = -1;
            for (int i = 0; i < this.cmbLayers.Properties.Items.Count; i++)
            {
                if (OtherHelper.GetLeftName(this.cmbLayers.Properties.Items[i].ToString().Trim()) == "DLTB")
                {
                    idx = i;
                    break;
                }
            }
            this.cmbLayers.SelectedIndex = idx;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "")
            {
                return;
            }
            string ClassName = OtherHelper.GetLeftName(this.cmbLayers.Text.Trim());
            IFeatureClass pFC = null;
            try
            {
                pFC = (this.currWS as IFeatureWorkspace).OpenFeatureClass(ClassName);
            }
            catch { }
            if (pFC == null)
            {
                MessageBox.Show("找不到要素类");
                return;
            }


            this.Cursor = Cursors.WaitCursor;
            lblStatus.Text = "正在计算中...";
            Application.DoEvents();
            IFeature aFea = null;
            IFeatureCursor pFeaCursor = null;
            //IQueryFilter pQF = new QueryFilterClass();
            //pQF.WhereClause = "KCXS>0";

            IWorkspaceEdit pWSEdit = this.currWS as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();

            try
            {
                //要四舍五入，roun
                int icount = 1;
                pFeaCursor = pFC.Update(null, false);
                while ((aFea = pFeaCursor.NextFeature()) != null)
                {
                    double tbmj = FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ");
                    double tkxs = FeatureHelper.GetFeatureDoubleValue(aFea, "KCXS");
                    if (tkxs == 0)
                    {
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ", tbmj);
                        FeatureHelper.SetFeatureValue(aFea, "KCMJ", 0);
                    }
                    else
                    {
                        double TKMJ = MathHelper.RoundEx(tbmj * tkxs, 2);
                        double dlmj = MathHelper.RoundEx(tbmj - TKMJ, 2);
                        FeatureHelper.SetFeatureValue(aFea, "KCMJ", TKMJ);
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ", dlmj);

                    }
                    pFeaCursor.UpdateFeature(aFea);

                    icount++;
                    if (icount % 100 == 0)
                    {
                        lblStatus.Text = "正在计算" + icount + "个要素...";
                        Application.DoEvents();
                    }
                }


                this.Cursor = Cursors.Default;
                lblStatus.Text = "";
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);

                MessageBox.Show("计算完毕,共更新" + icount + "条记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
                lblStatus.Text = "";
                this.Cursor = Cursors.Default;
            }
            finally
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);               
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            IFeatureClass czcdydClass = null;
            IFeatureClass dltbClass = null;
            try
            {
                czcdydClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass("CZCDYD");
                dltbClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch { }
            //获取CZC
            string idField = RCIS.GISCommon.LayerHelper.getLayerObjId(czcdydClass);
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = czcdydClass;
            IIdentify idJxs = pLayer as IIdentify;
            IArray arCzc = idJxs.Identify((czcdydClass as IGeoDataset).Extent);
            if (arCzc == null)
            {
                MessageBox.Show("城镇村等用地尚没有要素！");
                return;
            }
            try
            {
                Dictionary<long, double> dicMjs = new Dictionary<long, double>();
                for (int i = 0; i < arCzc.Count; i++)
                {
                    IFeatureIdentifyObj obj = arCzc.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aFeature = aRow.Row as IFeature;
                    //统计面积
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    //pSF.WhereClause = "ZLDWDM='" + xzqdm + "'";
                    pSF.Geometry = aFeature.ShapeCopy;

                    double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                    FeatureHelper.StatsFieldValue(dltbClass, pSF as IQueryFilter, "TBMJ", out dmax, out dmin, out dsum, out dmean);
                    if (!dicMjs.ContainsKey(aFeature.OID))
                    {
                        dsum = MathHelper.Round(dsum, 2);
                        dicMjs.Add(aFeature.OID, dsum);
                    }

                }
                //面积计算完毕，开始赋值
                foreach (KeyValuePair<long, double> aitem in dicMjs)
                {
                    this.currWS.ExecuteSQL("update CZCDYD set CZCMJ=" + aitem.Value + " where " + idField + " = " + aitem.Key.ToString());
                }
                MessageBox.Show("赋值完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            IFeatureClass gddbClass = null;
            IFeatureClass dltbClass = null;
            try
            {
                gddbClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass("GDDB");
                dltbClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch { }
            //获取CZC
            string idField = RCIS.GISCommon.LayerHelper.getLayerObjId(gddbClass);

            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = gddbClass;
            IIdentify idJxs = pLayer as IIdentify;
            IArray arGDDB = idJxs.Identify((gddbClass as IGeoDataset).Extent);
            if (arGDDB == null)
            {
                MessageBox.Show("耕地等别尚没有要素！");
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {
                Dictionary<long, double> dicTBDLMJ = new Dictionary<long, double>();
                Dictionary<long, double> dicKcdlmj = new Dictionary<long, double>();

                for (int i = 0; i < arGDDB.Count; i++)
                {
                    IFeatureIdentifyObj obj = arGDDB.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aFeature = aRow.Row as IFeature;
                    //统计面积
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    pSF.Geometry = aFeature.ShapeCopy;

                    double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                    FeatureHelper.StatsFieldValue(dltbClass, pSF as IQueryFilter, "TBDLMJ", out dmax, out dmin, out dsum, out dmean);
                    if (!dicTBDLMJ.ContainsKey(aFeature.OID))
                    {
                        dsum = MathHelper.Round(dsum, 2);
                        dicTBDLMJ.Add(aFeature.OID, dsum);
                    }

                    dsum = 0;
                    FeatureHelper.StatsFieldValue(dltbClass, pSF as IQueryFilter, "KCMJ", out dmax, out dmin, out dsum, out dmean);

                    if (!dicKcdlmj.ContainsKey(aFeature.OID))
                    {
                        dsum = MathHelper.Round(dsum, 2);
                        dicKcdlmj.Add(aFeature.OID, dsum);
                    }



                }
                //面积计算完毕，开始赋值
                foreach (KeyValuePair<long, double> aitem in dicTBDLMJ)
                {
                    this.currWS.ExecuteSQL("update GDDB set TBDLMJ=" + aitem.Value + " where " + idField + " = " + aitem.Key.ToString());
                }

                foreach (KeyValuePair<long, double> aitem in dicKcdlmj)
                {
                    this.currWS.ExecuteSQL("update GDDB set KCDLMJ=" + aitem.Value + " where " + idField + " = " + aitem.Key.ToString());
                }
                this.Cursor = Cursors.Default;

                MessageBox.Show("赋值完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private bool GetIntersectArea(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            pTop.Simplify();
            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            if (pGeoIntersect != null)
            {
                IArea pArea = pGeoIntersect as IArea;
                if (pArea.Area > 0.01)
                {
                    b = true;
                }
            }
            return b;
        }


        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请于提取过程之后使用本功能，否则面积计算存在错误！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行计算...", "请稍等...");
            wait.Show();
            simpleButton1.Enabled = false;
            simpleButton3.Enabled = false;
            simpleButton4.Enabled = false;
            simpleButton5.Enabled = false;
            IFeatureWorkspace pFeaWorkspace = currWS as IFeatureWorkspace;

            IFeatureClass pCZCDYDGX = pFeaWorkspace.OpenFeatureClass("CZCDYDGX");
            if (pCZCDYDGX == null || pCZCDYDGX.FeatureCount(null) <= 0)
            {
                wait.Close();
                simpleButton1.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                MessageBox.Show("城镇村等用地更新层无数据！");
                return;
            }
            int czcmjIndex = pCZCDYDGX.Fields.FindField("CZCMJ");
            if (czcmjIndex <= -1)
            {
                wait.Close();
                simpleButton1.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                MessageBox.Show("未找到城镇村面积字段！");
                return;
            }
            IWorkspace tmpWS = DeleteAndNewTmpGDB();

            //用更新层更新基础层生成年末库
            string inFea = currWS.PathName + "\\TDDC\\DLTB";
            string gxFea = currWS.PathName + "\\TDGX\\DLTBGX";
            string outFea = tmpWS.PathName + "\\nmdltb";
            bool b = RCIS.GISCommon.GpToolHelper.Update(inFea, gxFea, outFea);
            if (b == false)
            {
                wait.Close();
                simpleButton1.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                MessageBox.Show("叠加分析错误！");
                return;
            }
            //创建临时文件，存储交集制表结果
            string tempPath = AppDomain.CurrentDomain.BaseDirectory + "tmp";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            string resultMDBPath = tempPath + "\\result.mdb";
            File.Copy(AppDomain.CurrentDomain.BaseDirectory + "SystemConf\\result.mdb", resultMDBPath, true);
            //交集制表，获取CZCDYDGX和DLTB层空间叠加关系
            string zoneFea = currWS.PathName + "\\TDGX\\CZCDYDGX";
            bool TabulateResult = GpToolHelper.GP_TabulateIntersection(zoneFea, "OBJECTID", outFea, "TBMJ", resultMDBPath + "\\result");
            if (TabulateResult == false)
            {
                wait.Close();
                simpleButton1.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                MessageBox.Show("交集制表分析错误！");
                return;
            }
            //从交集制表结果中循环赋值给CZCDYDGX层的CZCMJ字段
            RCIS.Database.AccdbOperateHelper mdbHelper = new RCIS.Database.AccdbOperateHelper(resultMDBPath);
            DataTable dt = mdbHelper.GetDatatable("select OBJECTID_1,sum(tbmj) as ZMJ from result where area>0.01 group by OBJECTID_1");
            if (dt == null || dt.Rows.Count <= 0)
            {
                wait.Close();
                simpleButton1.Enabled = true;
                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                MessageBox.Show("未找到交集制表结果！");
                return;
            }
            IFeatureCursor pCZCDYDGXCur = pCZCDYDGX.Search(null, false);
            IFeature pFeaCZC = null;
            while ((pFeaCZC = pCZCDYDGXCur.NextFeature()) != null)
            {
                DataRow[] drs = dt.Select(string.Format("OBJECTID_1={0}", pFeaCZC.OID));
                if (drs != null && drs.Length > 0)
                {
                    pFeaCZC.set_Value(czcmjIndex, MathHelper.Round(double.Parse(drs[0]["ZMJ"].ToString()), 2));
                    pFeaCZC.Store();
                }
                if (pFeaCZC != null)
                {
                    Marshal.FinalReleaseComObject(pFeaCZC);
                }
            }

            if (tmpWS != null)
            {
                Marshal.FinalReleaseComObject(tmpWS);
            }
            if (pCZCDYDGX != null)
            {
                Marshal.FinalReleaseComObject(pCZCDYDGX);
            }
            if (pCZCDYDGXCur != null)
            {
                Marshal.FinalReleaseComObject(pCZCDYDGXCur);
            }
            if (dt != null)
            {
                dt.Dispose();
            }
            wait.Close();
            simpleButton1.Enabled = true;
            simpleButton3.Enabled = true;
            simpleButton4.Enabled = true;
            simpleButton5.Enabled = true;
            MessageBox.Show("赋值完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            //}
            //catch (Exception ex)
            //{
            //    wait.Close();
            //    simpleButton1.Enabled = true;
            //    simpleButton3.Enabled = true;
            //    simpleButton4.Enabled = true;
            //    simpleButton5.Enabled = true;
            //    MessageBox.Show(ex.Message);
            //}
        }

        private IWorkspace DeleteAndNewTmpGDB()
        {
            string path = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;

            if (Directory.Exists(path))
            {
                try
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                catch
                {
                    foreach (string tmp in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(tmp))
                        {
                            //如果有子文件删除文件
                            File.Delete(tmp);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(path);
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            return tmpWS;
        }
    }
}
