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

namespace TDDC3D.gengxin
{
    public partial class FrmXZQGXGC : Form
    {
        public IWorkspace currWs = null;
        long maxBSM;

        public FrmXZQGXGC()
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
            if (string.IsNullOrWhiteSpace(txtXZQBH.Text))
            {
                MessageBox.Show("请选择变化行政区的数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtXZQBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("变化行政区数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("原行政区更新过程层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            UpdateStatus("正在处理变化行政区数据");
            IGeometry pGeometry = RCIS.GISCommon.GeometryHelper.MergeGeometry(pBHClass);
            UpdateStatus("查找与变化行政区重叠的三调行政区");
            IFeatureWorkspace pFWor = currWs as IFeatureWorkspace;
            IFeatureClass pXZQClass = pFWor.OpenFeatureClass("XZQ");
            string XZQSD = Application.StartupPath + @"\tmp\XZQSD.shp";
            string XZQGXGC = Application.StartupPath + @"\tmp\XZQGXGC.shp";
            IFeatureClass pSDClass;
            if (File.Exists(XZQSD))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQSD);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            if (File.Exists(XZQGXGC))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQGXGC);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            pSDClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(XZQSD, esriGeometryType.esriGeometryPolygon, (pXZQClass as IGeoDataset).SpatialReference, pXZQClass.Fields);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.GeometryField = pXZQClass.ShapeFieldName;
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pSpatialFilter.SpatialRelDescription = "T********";
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsertCursor = pSDClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeatureCursor pSearchCursor = pXZQClass.Search(pSpatialFilter, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IGeometry pGeoIntersect = pTop.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (pGeoIntersect != null)
                    {
                        IArea pArea = pGeoIntersect as IArea;
                        if (pArea.Area > 0.0001)
                        {
                            pInsertCursor.InsertFeature(pFeature as IFeatureBuffer);
                        }
                    }
                }
                pInsertCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pBHClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            UpdateStatus("正在进行叠加分析");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();

            pUnion.in_features = XZQSD + ";" + txtXZQBH.Text;
            pUnion.out_feature_class = XZQGXGC;
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
            IFeatureClass pSourClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQGXGC);
            IFeatureClass pTarClass = null;
            try
            {
                pTarClass = pFWor.OpenFeatureClass("XZQGXGC");
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
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pSourClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));
                    string bghbsm = pFeature.get_Value(pSourClass.FindField("BSM_1")).ToString();
                    if (string.IsNullOrWhiteSpace(bghbsm))
                    {
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), maxBSM++);
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));
                    }
                    else
                    {
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pSourClass.FindField("BSM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM_1")));
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC_1")));
                    }
                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                    #endregion
                    pInsertCursor.InsertFeature(pFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            UpdateStatus("正在提取同名的行政区");
            System.Collections.ArrayList xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGQXZQDM");
            List<string> xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("XZQDM = '" + item.ToString() + "'");
            }
            string sWhere = string.Join(" Or ", xzqdm);
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pXZQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pXZQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pXZQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQDM"), pFeature.get_Value(pXZQClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQMC"), pFeature.get_Value(pXZQClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pXZQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pXZQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pXZQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pXZQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), maxBSM++);
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pXZQClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pXZQClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pXZQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pXZQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pXZQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pXZQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), DateTime.ParseExact("20191231", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGQXZQDM = '" + pFeature.get_Value(pXZQClass.FindField("XZQDM")) + "'");
                }
                pInsert.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQClass);
            UpdateStatus("处理完成");
            MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtXZQBH_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openDia = new OpenFileDialog();
            openDia.Filter = "SHP文件（*.shp）|*.shp";
            if (openDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtXZQBH.Text = openDia.FileName;
                IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtXZQBH.Text);
                maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pBHClass, "BSM"); maxBSM++;
            }
        }

        private void btnComputeBSM_Click(object sender, EventArgs e)
        {
            UpdateStatus("正在打开行政区更新过程图层");
            IFeatureClass pBHClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtXZQBH.Text);
            if (pBHClass == null)
            {
                MessageBox.Show("行政区数据没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pGCClass = null;
            try
            {
                pGCClass = pFeaWorkspace.OpenFeatureClass("XZQGXGC");
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
            UpdateStatus("正在重排标识码");
            maxBSM = EditFieldFromMaxNum(maxBSM, pGCClass, "BSM", "");
            UpdateStatus("标识码重排完毕");
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
    }
}
