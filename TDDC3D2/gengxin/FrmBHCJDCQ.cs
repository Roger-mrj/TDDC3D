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

namespace TDDC3D.gengxin
{
    public partial class FrmBHCJDCQ : Form
    {
        public IWorkspace currWs = null;

        public FrmBHCJDCQ()
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

        private void txtBHCJDCQ_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog saveDia = new SaveFileDialog();
            saveDia.Filter = "SHP文件（*.shp）|*.shp";
            if (saveDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtBHCJDCQ.Text = saveDia.FileName;
            }
        }

        private void btnDLTBGX_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBHCJDCQ.Text))
            {
                MessageBox.Show("请设置保存变化村级调查区的SHP文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pTarFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtBHCJDCQ.Text);
            if (pTarFeatureClass != null)
            {
                if (MessageBox.Show("文件已经存在，是否覆盖？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    (pTarFeatureClass as IDataset).Delete();
                }
                else
                {
                    UpdateStatus("由于文件存在，退出");
                    return;
                }
            }
            UpdateStatus("开始提取变化村级调查区");
            IFeatureWorkspace pFWS = currWs as IFeatureWorkspace;
            IFeatureClass pCJDCQClass = pFWS.OpenFeatureClass("CJDCQ");
            pTarFeatureClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(txtBHCJDCQ.Text, pCJDCQClass.ShapeType, (pCJDCQClass as IGeoDataset).SpatialReference, pCJDCQClass.Fields);
            using (ESRI.ArcGIS.ADF.ComReleaser ComRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pTarFeatureClass.Insert(true);
                ComRel.ManageLifetime(pInsert);
                IFeatureClass pXZQGXGCClass = pFWS.OpenFeatureClass("XZQGXGC");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pXZQGXGCCursor = pXZQGXGCClass.Search(null, true);
                    comRel.ManageLifetime(pXZQGXGCCursor);
                    IFeature pFeature;
                    while ((pFeature = pXZQGXGCCursor.NextFeature()) != null)
                    {
                        string dwdm1 = pFeature.get_Value(pXZQGXGCClass.FindField("BGQXZQDM")).ToString();
                        string dwdm2 = pFeature.get_Value(pXZQGXGCClass.FindField("BGHXZQDM")).ToString();
                        if (string.IsNullOrWhiteSpace(dwdm2) || dwdm1 == dwdm2) continue;
                        ITopologicalOperator pTop = pFeature.ShapeCopy as ITopologicalOperator;
                        ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                        pSpatialFilter.Geometry = pFeature.ShapeCopy;
                        pSpatialFilter.GeometryField = pCJDCQClass.ShapeFieldName;
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpatialFilter.SpatialRelDescription = "T********";
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IFeatureCursor pSearch = pCJDCQClass.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pSearch);
                            IFeature pSour;
                            while ((pSour = pSearch.NextFeature()) != null)
                            {
                                IGeometry pGeo = pTop.Intersect(pSour.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                                IFeatureBuffer pTar = pTarFeatureClass.CreateFeatureBuffer();
                                pTar.Shape = pGeo;
                                pTar.set_Value(pTarFeatureClass.FindField("YSDM"), pSour.get_Value(pCJDCQClass.FindField("YSDM")));
                                pTar.set_Value(pTarFeatureClass.FindField("ZLDWDM"), dwdm2);
                                pTar.set_Value(pTarFeatureClass.FindField("ZLDWMC"), pSour.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                                pTar.set_Value(pTarFeatureClass.FindField("MSSM"), pSour.get_Value(pCJDCQClass.FindField("MSSM")));
                                if (!string.IsNullOrWhiteSpace(pSour.get_Value(pCJDCQClass.FindField("HDMC")).ToString()))
                                    pTar.set_Value(pTarFeatureClass.FindField("HDMC"), pSour.get_Value(pCJDCQClass.FindField("HDMC")));
                                if (!string.IsNullOrWhiteSpace(pSour.get_Value(pCJDCQClass.FindField("BZ")).ToString()))
                                    pTar.set_Value(pTarFeatureClass.FindField("BZ"), pSour.get_Value(pCJDCQClass.FindField("BZ")));
                                pInsert.InsertFeature(pTar);
                            }
                        }
                    }
                    pInsert.Flush();
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCClass);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTarFeatureClass);
            UpdateStatus("提取完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            IFeatureClass pFeatureclass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtBHCJDCQ.Text);
            if (pFeatureclass == null)
            {
                MessageBox.Show("提取数据为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateStatus("正在打开变化村级调查区");
            List<string> arr = GetUniqueValuesByFeatureClass(pFeatureclass,"ZLDWDM");
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass sdFeatureclass = pFeaWorkspace.OpenFeatureClass("CJDCQ");
            UpdateStatus("正在编写村级调查区单位代码");            
            for (int i = 0; i < arr.Count; i++)
            {
                IQueryFilter queryFilter = new QueryFilterClass();
                string search="ZLDWDM LIKE '"+arr[i]+"%'";
                queryFilter.WhereClause = search;
                long maxNum = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(sdFeatureclass,"ZLDWDM",search);
                long max;
                if (maxNum == 0)
                    max = long.Parse(arr[i] + "000");
                else
                    max = long.Parse(maxNum.ToString().Substring(0, maxNum.ToString().Length - 7));
                IFeatureCursor pFeatureCursor = pFeatureclass.Update(queryFilter, true);
                IFeature pFeature = null;
                while ((pFeature = pFeatureCursor.NextFeature()) != null)
                {
                    max++;
                    pFeature.set_Value(pFeature.Fields.FindField("ZLDWDM"), max.ToString() + "0000000");
                    pFeatureCursor.UpdateFeature(pFeature);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureCursor);
            }
            UpdateStatus("村级调查区单位代码编写完毕");
            MessageBox.Show("完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
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
