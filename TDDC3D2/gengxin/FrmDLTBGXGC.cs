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
using System.IO;
using System.Collections;

namespace TDDC3D.gengxin
{
    public partial class FrmDLTBGXGC : Form
    {
        public IWorkspace currWs = null;
        long maxBSM;

        public FrmDLTBGXGC()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void btnGXGC_Click(object sender, EventArgs e)
        {
            //数据处理流程：
            //1.合并所有变化图斑
            //2.查找重叠三调图斑
            //3.变化图斑与重叠三调图斑进行叠加分析
            //4.数据更新进地类图斑更新过程图层
            if (string.IsNullOrWhiteSpace(txtDLTBBH.Text))
            {
                MessageBox.Show("请选择变化图斑的数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("变化图斑数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("原图斑更新过程层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            UpdateStatus("正在处理变化图斑数据");
            
            IGeometry pGeometry = RCIS.GISCommon.GeometryHelper.MergeGeometry(pBHClass);
            UpdateStatus("查找与变化图斑重叠的三调图斑");
            IFeatureWorkspace pFWor = currWs as IFeatureWorkspace;
            IFeatureClass pDLTBClass = pFWor.OpenFeatureClass("DLTB");
            string SDDLTBFILE = Application.StartupPath + @"\tmp\SDDLTB.shp";
            string DLTBGXGC = Application.StartupPath + @"\tmp\DLTBGXGC.shp";
            IFeatureClass pSDClass;
            if (File.Exists(SDDLTBFILE))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(SDDLTBFILE);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            if (File.Exists(DLTBGXGC))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(DLTBGXGC);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            pSDClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(SDDLTBFILE, esriGeometryType.esriGeometryPolygon, (pDLTBClass as IGeoDataset).SpatialReference, pDLTBClass.Fields);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.GeometryField = pDLTBClass.ShapeFieldName;
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
            pSpatialFilter.SpatialRelDescription = "T********";
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsertCursor = pSDClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeatureCursor pSearchCursor = pDLTBClass.Search(pSpatialFilter, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    pInsertCursor.InsertFeature(pFeature as IFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pBHClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            UpdateStatus("正在进行叠加分析");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();

            pUnion.in_features = SDDLTBFILE + ";" + txtDLTBBH.Text;
            pUnion.out_feature_class = DLTBGXGC;
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            UpdateStatus("正在导入叠加结果");
            IFeatureClass pSourClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(DLTBGXGC);
            IFeatureClass pTarClass = null;
            try
            {
                pTarClass = pFWor.OpenFeatureClass("DLTBGXGC");
            }
            catch
            {
                UpdateStatus("数据库没有升级，请先升级数据库。");
                return;
            }
            ITable pTable = pTarClass as ITable;
            pTable.DeleteSearchedRows(null);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pSearchCursor = pSourClass.Search(null, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeatureCursor pInsertCursor = pTarClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), pFeature.get_Value(pSourClass.FindField("BGXW")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQTBBSM"), pFeature.get_Value(pSourClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDLBM"), pFeature.get_Value(pSourClass.FindField("DLBM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDLMC"), pFeature.get_Value(pSourClass.FindField("DLMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQQSXZ"), pFeature.get_Value(pSourClass.FindField("QSXZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQQSDWDM"), pFeature.get_Value(pSourClass.FindField("QSDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQQSDWMC"), pFeature.get_Value(pSourClass.FindField("QSDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQKCDLBM"), pFeature.get_Value(pSourClass.FindField("KCDLBM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQKCXS"), pFeature.get_Value(pSourClass.FindField("KCXS")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQKCMJ"), pFeature.get_Value(pSourClass.FindField("KCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQTBDLMJ"), pFeature.get_Value(pSourClass.FindField("TBDLMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQGDLX"), pFeature.get_Value(pSourClass.FindField("GDLX")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQGDPDJB"), pFeature.get_Value(pSourClass.FindField("GDPDJB")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZDWKD"), pFeature.get_Value(pSourClass.FindField("XZDWKD")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQTBXHDM"), pFeature.get_Value(pSourClass.FindField("TBXHDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQTBXHMC"), pFeature.get_Value(pSourClass.FindField("TBXHMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZZSXDM"), pFeature.get_Value(pSourClass.FindField("ZZSXDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZZSXMC"), pFeature.get_Value(pSourClass.FindField("ZZSXMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQGDDB"), pFeature.get_Value(pSourClass.FindField("GDDB")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQFRDBS"), pFeature.get_Value(pSourClass.FindField("FRDBS")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQCZCSXM"), pFeature.get_Value(pSourClass.FindField("CZCSXM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));

                    string bghbsm = pFeature.get_Value(pSourClass.FindField("BSM_1")).ToString();
                    if (string.IsNullOrWhiteSpace(bghbsm))
                    {
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBBSM"), maxBSM++);
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHDLBM"), pFeature.get_Value(pSourClass.FindField("DLBM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHDLMC"), pFeature.get_Value(pSourClass.FindField("DLMC")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHQSXZ"), pFeature.get_Value(pSourClass.FindField("QSXZ")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHQSDWDM"), pFeature.get_Value(pSourClass.FindField("QSDWDM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHQSDWMC"), pFeature.get_Value(pSourClass.FindField("QSDWMC")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHKCDLBM"), pFeature.get_Value(pSourClass.FindField("KCDLBM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHKCXS"), pFeature.get_Value(pSourClass.FindField("KCXS")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHKCMJ"), pFeature.get_Value(pSourClass.FindField("KCMJ")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBDLMJ"), pFeature.get_Value(pSourClass.FindField("TBDLMJ")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHGDLX"), pFeature.get_Value(pSourClass.FindField("GDLX")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHGDPDJB"), pFeature.get_Value(pSourClass.FindField("GDPDJB")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZDWKD"), pFeature.get_Value(pSourClass.FindField("XZDWKD")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBXHDM"), pFeature.get_Value(pSourClass.FindField("TBXHDM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBXHMC"), pFeature.get_Value(pSourClass.FindField("TBXHMC")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZZSXDM"), pFeature.get_Value(pSourClass.FindField("ZZSXDM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZZSXMC"), pFeature.get_Value(pSourClass.FindField("ZZSXMC")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHGDDB"), pFeature.get_Value(pSourClass.FindField("GDDB")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHFRDBS"), pFeature.get_Value(pSourClass.FindField("FRDBS")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHCZCSXM"), pFeature.get_Value(pSourClass.FindField("CZCSXM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));
                    }
                    else
                    {
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBBSM"), bghbsm);
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHDLBM"), pFeature.get_Value(pSourClass.FindField("DLBM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHDLMC"), pFeature.get_Value(pSourClass.FindField("DLMC_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHQSXZ"), pFeature.get_Value(pSourClass.FindField("QSXZ_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHQSDWDM"), pFeature.get_Value(pSourClass.FindField("QSDWDM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHQSDWMC"), pFeature.get_Value(pSourClass.FindField("QSDWMC_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHKCDLBM"), pFeature.get_Value(pSourClass.FindField("KCDLBM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHKCXS"), pFeature.get_Value(pSourClass.FindField("KCXS_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHKCMJ"), pFeature.get_Value(pSourClass.FindField("KCMJ_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBDLMJ"), pFeature.get_Value(pSourClass.FindField("TBDLMJ_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHGDLX"), pFeature.get_Value(pSourClass.FindField("GDLX_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHGDPDJB"), pFeature.get_Value(pSourClass.FindField("GDPDJB_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZDWKD"), pFeature.get_Value(pSourClass.FindField("XZDWKD_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBXHDM"), pFeature.get_Value(pSourClass.FindField("TBXHDM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHTBXHMC"), pFeature.get_Value(pSourClass.FindField("TBXHMC_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZZSXDM"), pFeature.get_Value(pSourClass.FindField("ZZSXDM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHZZSXMC"), pFeature.get_Value(pSourClass.FindField("ZZSXMC_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHGDDB"), pFeature.get_Value(pSourClass.FindField("GDDB_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHFRDBS"), pFeature.get_Value(pSourClass.FindField("FRDBS_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHCZCSXM"), pFeature.get_Value(pSourClass.FindField("CZCSXM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC_1")));
                    }
                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                    #endregion
                    pInsertCursor.InsertFeature(pFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            UpdateStatus("处理完成");
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtDLTBBH_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openDia = new OpenFileDialog();
            openDia.Filter = "SHP文件（*.shp）|*.shp";
            if (openDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDLTBBH.Text = openDia.FileName;
                IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
                maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pBHClass, "BSM"); maxBSM++;
            }
        }

        private void btnComputeMJ_Click(object sender, EventArgs e)
        {
            //面积计算流程
            //1.计算变更前标识码和次数，从而知道变更前图斑对应分割的个数
            //2.按照变更前标识码和次数关系分摊面积
            UpdateStatus("正在计算更新过程层中的三调图斑标识码");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            string filePath = currWs.PathName;
            ESRI.ArcGIS.AnalysisTools.Frequency pFrequency = new ESRI.ArcGIS.AnalysisTools.Frequency();
            pFrequency.in_table = filePath + @"\TDGX\DLTBGXGC";
            pFrequency.out_table = filePath + @"\DLTBGXGCTMP";
            pFrequency.frequency_fields = "BGQTBBSM";
            pFrequency.summary_fields = "SHAPE_Area";
            try
            {
                gp.Execute(pFrequency, null);
            }
            catch
            {
                UpdateStatus("计算错误");
                return;
            }
            UpdateStatus("开始计算面积");
            ITable pTable = (currWs as IFeatureWorkspace).OpenTable("DLTBGXGCTMP");
            IFeatureClass pFeatureClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pFeatureClass, "BGQTBBSM");
            IFeatureClass pDLTBClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBClass, "BSM");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ICursor pCursor = pTable.Search(null, true);
                comRel.ManageLifetime(pCursor);
                IRow pRow;
                while ((pRow = pCursor.NextRow()) != null)
                {
                    int count = (int)pRow.get_Value(pTable.FindField("FREQUENCY"));
                    string bsm = (string)pRow.get_Value(pTable.FindField("BGQTBBSM"));
                    double mj = (double)pRow.get_Value(pTable.FindField("SHAPE_Area"));
                    using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                    {
                        IQueryFilter pDLTBFilter = new QueryFilterClass();
                        pDLTBFilter.WhereClause = "BSM = '" + bsm + "'";
                        IFeatureCursor pDLTBCursor = pDLTBClass.Search(pDLTBFilter, true);
                        comRel2.ManageLifetime(pDLTBCursor);
                        IFeature pDLTB = pDLTBCursor.NextFeature();
                        IQueryFilter pQueryFilter = new QueryFilterClass();
                        pQueryFilter.WhereClause = "BGQTBBSM = '" + bsm + "'";
                        IFeatureCursor pFeatureCursor = pFeatureClass.Update(pQueryFilter, true);
                        comRel2.ManageLifetime(pFeatureCursor);
                        IFeature pFeature;
                        if (count == 1)
                        {
                            pFeature = pFeatureCursor.NextFeature();
                            double kcmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("KCMJ")).ToString(), out kcmj);
                            double dlmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("TBDLMJ")).ToString(), out dlmj);
                            pFeature.set_Value(pFeatureClass.FindField("TBBGMJ"), kcmj + dlmj);
                            double kcxs2 = 0;
                            double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGHKCXS")).ToString(), out kcxs2);
                            if (kcxs2 == 0)
                            {
                                pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), kcmj + dlmj);
                            }
                            else
                            {
                                double kcmj2 = Math.Round((kcmj + dlmj) * kcxs2, 2);
                                pFeature.set_Value(pFeatureClass.FindField("BGHKCMJ"), kcmj2);
                                pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), kcmj + dlmj - kcmj2);
                            }
                            pFeatureCursor.UpdateFeature(pFeature);
                        }
                        else
                        {
                            pFeature = pFeatureCursor.NextFeature();
                            double sumkcmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("KCMJ")).ToString(), out sumkcmj);
                            double sumdlmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("TBDLMJ")).ToString(), out sumdlmj);
                            double subkcmj = sumkcmj;
                            double subdlmj = sumdlmj;
                            for (int i = 1; i < count; i++)
                            {
                                double txmj = (double)pFeature.get_Value(pFeatureClass.FindField("SHAPE_Area"));
                                double bgmj = Math.Round(txmj / mj * (sumkcmj + sumdlmj), 2);
                                double kcmji = Math.Round(txmj / mj * sumkcmj, 2);
                                pFeature.set_Value(pFeatureClass.FindField("TBBGMJ"), bgmj);
                                pFeature.set_Value(pFeatureClass.FindField("BGQKCMJ"), kcmji);
                                pFeature.set_Value(pFeatureClass.FindField("BGQTBDLMJ"), bgmj - kcmji);
                                subkcmj -= kcmji;
                                subdlmj -= (bgmj - kcmji);
                                double kcxs2 = 0;
                                double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGHKCXS")).ToString(), out kcxs2);
                                if (kcxs2 == 0)
                                {
                                    pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), bgmj);
                                }
                                else
                                {
                                    double kcmj2 = Math.Round(bgmj * kcxs2, 2);
                                    pFeature.set_Value(pFeatureClass.FindField("BGHKCMJ"), kcmj2);
                                    pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), Math.Round(bgmj - kcmj2,2));
                                }
                                pFeatureCursor.UpdateFeature(pFeature);
                                pFeature = pFeatureCursor.NextFeature();
                            }
                            pFeature.set_Value(pFeatureClass.FindField("TBBGMJ"), Math.Round(subkcmj + subdlmj, 2));
                            pFeature.set_Value(pFeatureClass.FindField("BGQKCMJ"), Math.Round(subkcmj, 2));
                            pFeature.set_Value(pFeatureClass.FindField("BGQTBDLMJ"), Math.Round(subdlmj, 2));
                            double kcxs3 = 0;
                            double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGHKCXS")).ToString(), out kcxs3);
                            if (kcxs3 == 0)
                            {
                                pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), Math.Round(subkcmj + subdlmj,2));
                            }
                            else
                            {
                                double kcmj2 = Math.Round((subkcmj + subdlmj) * kcxs3, 2);
                                pFeature.set_Value(pFeatureClass.FindField("BGHKCMJ"), kcmj2);
                                pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), Math.Round(subkcmj + subdlmj - kcmj2,2));
                            }
                            pFeatureCursor.UpdateFeature(pFeature);
                        }
                        pFeatureCursor.Flush();
                    }
                }
            }
            (pTable as IDataset).Delete();
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureClass);
            UpdateStatus("计算完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnComputeBSM_Click(object sender, EventArgs e)
        {
            UpdateStatus("正在打开变化图斑更新过程图层");
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("变化图斑数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pGCClass = null;
            try
            {
                pGCClass=pFeaWorkspace.OpenFeatureClass("DLTBGXGC");
            }
            catch {
                MessageBox.Show("未找到更新过程图层，请先生成更新过程图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (pGCClass == null)
            {
                MessageBox.Show("更新过程层数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("正在重排标识码");
            maxBSM=EditFieldFromMaxNum(maxBSM, pGCClass, "BSM", "");
            UpdateStatus("标识码重排完毕");
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnComputeTBBH_Click(object sender, EventArgs e)
        {
            UpdateStatus("正在打开变化图斑更新过程图层");
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("变化图斑数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pGCClass = null;
            try
            {
                pGCClass=pFeaWorkspace.OpenFeatureClass("DLTBGXGC");
            }
            catch
            {
                MessageBox.Show("未找到更新过程图层，请先生成更新过程图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (pGCClass == null)
            {
                MessageBox.Show("更新过程层数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("正在重排图斑编号");
            List<string> arr = new List<string>();
            arr = GetUniqueValuesByFeatureClass(pBHClass, "ZLDWDM");
            for (int i = 0; i < arr.Count; i++)
            {
                string search = "ZLDWDM='" + arr[i] + "'";
                string write = "BGHZLDWDM='" + arr[i] + "'";
                long max = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(pBHClass, "TBBH", search);
                max= EditFieldFromMaxNum(max, pGCClass, "BGTBBH", write);
            }
            UpdateStatus("图斑编号重排完毕");
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private long EditFieldFromMaxNum(long maxNum, IFeatureClass writeFeatureClass, string writeField, string writeWhere)
        {
            //写入标识码或图斑编号
            int writeFieldNum = writeFeatureClass.FindField(writeField);
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = writeWhere;
            IFeatureCursor featureCursor = writeFeatureClass.Update(queryFilter, true);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                feature.set_Value(writeFieldNum, maxNum++);
                featureCursor.UpdateFeature(feature);
                feature = featureCursor.NextFeature();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(featureCursor);
            return maxNum;
        }

        private List<string> GetUniqueValuesByFeatureClass(IFeatureClass pFeatureClass, string FieldName)
        {
            List<string> arrValues = new List<string>();
            DataStatisticsClass pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pFeatureClass.Search(null, false) as ICursor;
            pDataStatistics.Field = FieldName;
            IEnumerator pEnum = pDataStatistics.UniqueValues;
            while (pEnum.MoveNext())
            {
                string temp = pEnum.Current.ToString();
                arrValues.Add(temp);
            }
            return arrValues;
        }
    }
}
