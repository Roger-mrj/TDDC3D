﻿using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Data;
using System.Windows.Forms;
namespace TDDC3D.analysis
{
    public partial class TK2GdFxForm : Form
    {
        public TK2GdFxForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beDstFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "shp文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDstFile.Text = dlg.FileName;
        }
        private DataTable buildTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("BGQDLBM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("BGHDLBM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("BGMJ", typeof(double));
            dt.Columns.Add(dc);

            dt.PrimaryKey = new DataColumn[] { dt.Columns[0], dt.Columns[1] };
            return dt;

        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.gridControl1.ExportToExcelOld(dlg.FileName);
            MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void TK2GdFxForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbByzLayers, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string byzname = OtherHelper.GetLeftName(this.cmbByzLayers.Text.Trim());
            if (byzname == "") return;
            string dltbName = OtherHelper.GetLeftName(this.cmbDltbLayer.Text);
            if (dltbName == "") return;
            if (this.beDstFile.Text.Trim() == "") return;
            string outShpFileName = this.beDstFile.Text.Trim();
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始进行叠加分析...", "稍等");
            wait.Show();
            try
            {
                ILayer byzLyr = LayerHelper.QueryLayerByModelName(this.currMap, byzname);
                ILayer dltbLyr = LayerHelper.QueryLayerByModelName(this.currMap, dltbName);
                #region    //先进行叠加分析
                IFeatureClass class1 = (dltbLyr as IFeatureLayer).FeatureClass;
                IFeatureClass class2 = (byzLyr as IFeatureLayer).FeatureClass;
                IGpValueTableObject valTbl = new GpValueTableObjectClass();
                valTbl.SetColumns(2);
                object row = "";
                object rank = 1;

                row = class1;
                valTbl.SetRow(0, ref row);
                valTbl.SetValue(0, 1, ref rank);

                row = class2;
                valTbl.SetRow(1, ref row);
                rank = 2;
                valTbl.SetValue(1, 1, ref rank);

                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
                intersect.in_features = valTbl;
                intersect.out_feature_class = outShpFileName;
                intersect.join_attributes = "NO_FID";
                intersect.output_type = "INPUT";

                gp.Execute(intersect, null);
                string s = GpToolHelper.ReturnMessages(gp);

                if ((s.ToUpper().Contains("ERROR")) || (s.Contains("失败")))
                {
                    wait.Close();
                    MessageBox.Show("gp执行失败。" + s, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion
                wait.SetCaption("正在提取和计算...");

                DataTable resultDT = buildTable();

                IWorkspace pWS = WorkspaceHelper2.GetShapefileWorkspace(outShpFileName);
                IFeatureClass pFC = (pWS as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(outShpFileName));
                //打开目标类，只保留新增建设用地部分               
                IFeatureCursor pCursor = null;
                int currDh = -1;
                try
                {
                    pCursor = pFC.Update(null, false);
                    IFeature pFeature = null;
                    while ((pFeature = pCursor.NextFeature()) != null)
                    {
                        string dlbm = FeatureHelper.GetFeatureStringValue(pFeature, "DLBM");
                        string olddlbm = FeatureHelper.GetFeatureStringValue(pFeature, "NYYPDL");
                        if (dlbm.StartsWith("01") && olddlbm.Equals("123"))
                        {
                            //冲击算图斑面积
                            if (currDh == -1)
                            {
                                IPoint selectPoint = (pFeature.ShapeCopy as IArea).Centroid;
                                double X = selectPoint.X;
                                currDh = (int)(X / 1000000);////WK---带号     
                            }

                            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                            double tbmj = area.SphereArea(pFeature.ShapeCopy, currDh);
                            FeatureHelper.SetFeatureValue(pFeature, "TBMJ", tbmj);
                            pFeature.Store();

                            //记录
                            DataRow[] resultRow = resultDT.Select("BGQDLBM='" + olddlbm + "' and BGHDLBM='" + dlbm + "'");
                            if (resultRow.Length != 0)
                            {
                                //修改
                                DataRow aResult = resultRow[0];
                                double olmj = (double)aResult[2];
                                aResult[2] = olmj + tbmj;

                            }
                            else
                            {
                                //新增
                                DataRow aResult = resultDT.NewRow();
                                aResult[0] = olddlbm;
                                aResult[1] = dlbm;
                                aResult[2] = tbmj;
                                resultDT.Rows.Add(aResult);
                            }
                        }
                        else
                        {
                            //删除
                            pFeature.Delete();
                        }

                    }



                }
                catch (Exception ex)
                {
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
                wait.SetCaption("正在分组统计...");
                this.gridControl1.DataSource = resultDT;


                //加载到地图
                IFeatureLayer pFeaturelyr = new FeatureLayerClass();
                pFeaturelyr.FeatureClass = pFC;
                pFeaturelyr.Name = pFC.AliasName;
                this.currMap.AddLayer(pFeaturelyr);
                wait.Close();

            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
