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
using DevExpress.Utils;
using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.analysis
{
    public partial class NewCZCForm : Form
    {
        public IWorkspace currWorkspace = null;
        public NewCZCForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSHPED_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "SHP格式文件|*.SHP";
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSHPED.Text = openfile.FileName;
                IWorkspace sourWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(txtSHPED.Text));
                IFeatureClass pFeatureClass1 = (sourWorkspace as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileName(txtSHPED.Text));
                if (pFeatureClass1.Fields.FindField("dlbm") == -1)
                {
                    MessageBox.Show("没有找到地类字段，请确认当前数据是否正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSHPED.Text = "";
                    return;
                }
                txtSHPResult.Text = System.IO.Path.GetDirectoryName(openfile.FileName) + "\\";
                btnPick.Focus();
            }
        }

        string ej = "'051','052','053','054','061','062','063','071','072','081','082','083','084','085','086','087','088','091','092','093','094','095','101','102','103','105','106','107','113','118','121','201','202','203','204','205'";
        private void btnPick_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSHPED.Text) || string.IsNullOrWhiteSpace(txtSHPResult.Text))
            {
                MessageBox.Show("请设置好二调数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtjs.Text) || string.IsNullOrWhiteSpace(txt20.Text))
            {
                MessageBox.Show("请设置好各种参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            WaitDialogForm wait = new WaitDialogForm("开始处理。", "提示");
            wait.Show();
            IFeatureClass pCZCFeatureClass = (currWorkspace as IFeatureWorkspace).OpenFeatureClass("CZCDYD");
            if (pCZCFeatureClass.FeatureCount(null) == 0)
            {
                MessageBox.Show("城镇村中没有数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pEDDLTBFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtSHPED.Text);
            string shpName = "扩进城镇村的二调图斑";
            IWorkspace tarWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(txtSHPED.Text));
            if ((tarWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, shpName))
            {
                IDataset pDataset = (tarWorkspace as IFeatureWorkspace).OpenFeatureClass(shpName) as IDataset;
                pDataset.Delete();
            }
            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
            //pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateOIDField());
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateGeometryField(ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon,(pEDDLTBFeatureClass as IGeoDataset).SpatialReference));
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("TBBH", "图斑编号", 20));
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("DLBM", "图斑编号", 20));
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("ZLDWDM", "坐落单位代码", 20));
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("ZLDWMC", "坐落单位名称", 20));
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("KDSM", "扩大说明", 100));
            pFieldsEdit.AddField(RCIS.GISCommon.EsriDatabaseHelper.CreateNumberField("KDMJ", "扩大面积"));
            IFeatureClass pEDinCZCFeatureClass = (tarWorkspace as IFeatureWorkspace).CreateFeatureClass(shpName, pFields, null, null, esriFeatureType.esriFTSimple, "shape", "");
            IFeatureBuffer EDinCZC;
            IFeatureCursor EDinCZCCursor = pEDinCZCFeatureClass.Insert(true);

            IFeatureCursor pCZCCursor = pCZCFeatureClass.Search(null, true);
            IFeature Czc;
            int i = 1;
            while ((Czc = pCZCCursor.NextFeature()) != null)
            {
                wait.SetCaption("正在计算城镇村数据" + Czc.OID.ToString());
                string czclx = Czc.get_Value(Czc.Fields.FindField("CZCLX")).ToString();
                IGeometry CzcGeometry = Czc.ShapeCopy;
                ITopologicalOperator pTopoCzc = CzcGeometry as ITopologicalOperator;
                if (!pTopoCzc.IsKnownSimple) pTopoCzc.Simplify();
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = CzcGeometry;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor edCursor = pEDDLTBFeatureClass.Search(pSpatialFilter, false);
                IFeature edFeature;
                while ((edFeature = edCursor.NextFeature()) != null)
                {
                    string dlbm = edFeature.get_Value(edFeature.Fields.FindField("DLBM")).ToString();
                    if (!ej.Contains(dlbm))
                    {
                        IGeometry edGeometry = edFeature.ShapeCopy;
                        IGeometry pIntersect = null;
                        double dArea = GeometryHelper.GetIntersectArea(edGeometry, CzcGeometry, ref pIntersect);
                        if (dArea > double.Parse(txtjs.Text))
                        {
                            EDinCZC = pEDinCZCFeatureClass.CreateFeatureBuffer();
                            EDinCZC.Shape = pIntersect;
                            FeatureHelper.SetFeatureBufferValue(EDinCZC, "TBBH", FeatureHelper.GetFeatureStringValue(edFeature, "TBBH"));
                            FeatureHelper.SetFeatureBufferValue(EDinCZC, "DLBM", FeatureHelper.GetFeatureStringValue(edFeature, "DLBM"));
                            FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWDM", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWDM"));
                            FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWMC", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWMC"));
                            FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDSM", "非建设用地流向城镇村范围");
                            FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDMJ", Math.Round(dArea, 2));
                            EDinCZCCursor.InsertFeature(EDinCZC);
                            i++;
                        }
                    }
                    else if (dlbm.Substring(0, 2) == "20")
                    {
                        IGeometry edGeometry = edFeature.ShapeCopy;
                        IGeometry pIntersect = null;
                        switch (dlbm.Substring(0, 3))
                        { 
                            case "204":
                                if (czclx == "203" || czclx == "202" || czclx == "201")
                                {
                                    double dArea = GeometryHelper.GetIntersectArea(edGeometry, CzcGeometry, ref pIntersect);
                                    if (dArea > double.Parse(txt20.Text))
                                    {
                                        EDinCZC = pEDinCZCFeatureClass.CreateFeatureBuffer();
                                        EDinCZC.Shape = pIntersect;
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "TBBH", FeatureHelper.GetFeatureStringValue(edFeature, "TBBH"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "DLBM", FeatureHelper.GetFeatureStringValue(edFeature, "DLBM"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWDM", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWDM"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWMC", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWMC"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDSM", "20属性发生变化");
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDMJ", Math.Round(dArea, 2));
                                        EDinCZCCursor.InsertFeature(EDinCZC);
                                        i++;
                                    }
                                }
                                break;
                            case "203":
                                if (czclx == "202" || czclx == "201")
                                {
                                    double dArea = GeometryHelper.GetIntersectArea(edGeometry, CzcGeometry, ref pIntersect);
                                    if (dArea > double.Parse(txt20.Text))
                                    {
                                        EDinCZC = pEDinCZCFeatureClass.CreateFeatureBuffer();
                                        EDinCZC.Shape = pIntersect;
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "TBBH", FeatureHelper.GetFeatureStringValue(edFeature, "TBBH"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "DLBM", FeatureHelper.GetFeatureStringValue(edFeature, "DLBM"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWDM", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWDM"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWMC", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWMC"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDSM", "20属性发生变化");
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDMJ", Math.Round(dArea, 2));
                                        EDinCZCCursor.InsertFeature(EDinCZC);
                                        i++;
                                    }
                                }
                                break;
                            case "202":
                                if (czclx == "201")
                                {
                                    double dArea = GeometryHelper.GetIntersectArea(edGeometry, CzcGeometry, ref pIntersect);
                                    if (dArea > double.Parse(txt20.Text))
                                    {
                                        EDinCZC = pEDinCZCFeatureClass.CreateFeatureBuffer();
                                        EDinCZC.Shape = pIntersect;
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "TBBH", FeatureHelper.GetFeatureStringValue(edFeature, "TBBH"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "DLBM", FeatureHelper.GetFeatureStringValue(edFeature, "DLBM"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWDM", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWDM"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "ZLDWMC", FeatureHelper.GetFeatureStringValue(edFeature, "ZLDWMC"));
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDSM", "20属性发生变化");
                                        FeatureHelper.SetFeatureBufferValue(EDinCZC, "KDMJ", Math.Round(dArea, 2));
                                        EDinCZCCursor.InsertFeature(EDinCZC);
                                        i++;
                                    }
                                }
                                break;
                        }
                    }
                }
                OtherHelper.ReleaseComObject(edCursor);
                if (i > 100) EDinCZCCursor.Flush();
            }
            OtherHelper.ReleaseComObject(pCZCCursor);
            EDinCZCCursor.Flush();
            wait.Close();
            MessageBox.Show("分析完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
