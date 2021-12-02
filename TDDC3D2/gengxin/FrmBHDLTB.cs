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

namespace TDDC3D.gengxin
{
    public partial class FrmBHDLTB : Form
    {
        public IWorkspace currWs = null;

        public FrmBHDLTB()
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

        private void txtBHDLTB_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog saveDia = new SaveFileDialog();
            saveDia.Filter = "SHP文件（*.shp）|*.shp";
            if (saveDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtBHDLTB.Text = saveDia.FileName;
            }
        }

        private void btnDLTBBH_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBHDLTB.Text))
            {
                MessageBox.Show("请设置保存变化地类图斑的SHP文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pTarFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtBHDLTB.Text);
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
            UpdateStatus("开始提取变化地类图斑");
            IFeatureWorkspace pFWS = currWs as IFeatureWorkspace;
            IFeatureClass pDLTBClass = pFWS.OpenFeatureClass("DLTB");
            pTarFeatureClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(txtBHDLTB.Text, pDLTBClass.ShapeType, (pDLTBClass as IGeoDataset).SpatialReference, pDLTBClass.Fields);
            using (ESRI.ArcGIS.ADF.ComReleaser ComRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pTarFeatureClass.Insert(true);
                ComRel.ManageLifetime(pInsert);
                IFeatureClass pCJDCQGXGCClass = pFWS.OpenFeatureClass("CJDCQGXGC");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pXZQGXGCCursor = pCJDCQGXGCClass.Search(null, true);
                    comRel.ManageLifetime(pXZQGXGCCursor);
                    IFeature pFeature;
                    while ((pFeature = pXZQGXGCCursor.NextFeature()) != null)
                    {
                        string dwdm1 = pFeature.get_Value(pCJDCQGXGCClass.FindField("BGQZLDWDM")).ToString();
                        string dwdm2 = pFeature.get_Value(pCJDCQGXGCClass.FindField("BGHZLDWDM")).ToString();
                        string dwmc = pFeature.get_Value(pCJDCQGXGCClass.FindField("BGHZLDWMC")).ToString();
                        if (string.IsNullOrWhiteSpace(dwdm2) || dwdm1 == dwdm2) continue;
                        IPolygon pPolygon1 = pFeature.ShapeCopy as IPolygon;
                        pPolygon1.SimplifyPreserveFromTo();
                        ITopologicalOperator pTop = pPolygon1 as ITopologicalOperator;
                        ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                        pSpatialFilter.Geometry = pFeature.ShapeCopy;
                        pSpatialFilter.GeometryField = pDLTBClass.ShapeFieldName;
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpatialFilter.SpatialRelDescription = "T********";
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            IFeatureCursor pSearch = pDLTBClass.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pSearch);
                            IFeature pSour;
                            while ((pSour = pSearch.NextFeature()) != null)
                            {
                                IPolygon pPolygon2 = pSour.ShapeCopy as IPolygon;
                                pPolygon2.SimplifyPreserveFromTo();
                                IGeometry pGeo = pTop.Intersect(pPolygon2, esriGeometryDimension.esriGeometry2Dimension);
                                IArea pArea = pGeo as IArea;
                                if (pArea.Area < 0.001)
                                {
                                    Console.WriteLine(pSour.get_Value(pSour.Fields.FindField("BSM")).ToString());
                                    continue;
                                }
                                IFeatureBuffer pTar = pTarFeatureClass.CreateFeatureBuffer();
                                pTar.Shape = pGeo;
                                List<string> fieldName = new List<string>();
                                fieldName.Add(pTarFeatureClass.ShapeFieldName);
                                fieldName.Add("BSM"); fieldName.Add("TBBH"); fieldName.Add("TBYBH"); 
                                fieldName.Add("ZLDWDM"); fieldName.Add("ZLDWMC"); fieldName.Add("QSDWDM"); fieldName.Add("QSDWMC");
                                RCIS.GISCommon.FeatureHelper.SetFeatureBufferValueSameFieldName(pTar, pSour, fieldName);
                                RCIS.GISCommon.FeatureHelper.SetFeatureBufferValue(pTar, "ZLDWDM", dwdm2);
                                RCIS.GISCommon.FeatureHelper.SetFeatureBufferValue(pTar, "ZLDWMC", dwmc);
                                RCIS.GISCommon.FeatureHelper.SetFeatureBufferValue(pTar, "QSDWDM", dwdm2);
                                RCIS.GISCommon.FeatureHelper.SetFeatureBufferValue(pTar, "QSDWMC", dwmc);
                                pInsert.InsertFeature(pTar);
                            }
                        }
                    }
                    pInsert.Flush();
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCClass);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTarFeatureClass);
            UpdateStatus("提取完成");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
