using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using RCIS.GISCommon;

namespace TDDC3D.gdzy
{
    public partial class FrmGetOtherByFLDY : Form
    {
        public IWorkspace _curWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;

        public FrmGetOtherByFLDY()
        {
            InitializeComponent();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!chkSWDYX.Checked && !chkTCHD.Checked && !chkTRPH.Checked && !chkTRZD.Checked && !chkTRZJS.Checked && !chkYJZHL.Checked)
            {
                MessageBox.Show("请融合生成的数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string tbName = "";
            switch (cmbLayer.Text)
            {
                case "分类单元":
                    tbName = "FLDY";
                    break;
                case "扩充分类单元":
                    tbName = "KCFLDY";
                    break;
                default:
                    break;

            }
            if (string.IsNullOrWhiteSpace(tbName))
            {
                MessageBox.Show("数据源图层不正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在融合数据……","提示");
            wait.Show();
            IFeatureWorkspace pFeatureWorkspace = _curWS as IFeatureWorkspace;
            IFeatureClass sourFeatureClass = pFeatureWorkspace.OpenFeatureClass(tbName);
            //图层厚度
            if (chkTCHD.Checked)
            {
                IFeatureClass tarFeatureClass = pFeatureWorkspace.OpenFeatureClass("TCHDTB");
                (tarFeatureClass as ITable).DeleteSearchedRows(null);
                dissoveFeatureClass(sourFeatureClass, "TCHDJB", "TCHD", tarFeatureClass, "TCHDXXZ", "TCHDSXZ");
            }
            //土壤质地

            //土壤有机质含量
            if (chkYJZHL.Checked)
            {
                IFeatureClass tarFeatureClass = pFeatureWorkspace.OpenFeatureClass("TRYJZHLTB");
                (tarFeatureClass as ITable).DeleteSearchedRows(null);
                dissoveFeatureClass(sourFeatureClass, "TRYJZHLJB", "TRYJZHL", tarFeatureClass, "TRYJZHLXXZ", "TRYJZHLSXZ");
            }
            //土壤PH值
            if (chkTRPH.Checked)
            {
                IFeatureClass tarFeatureClass = pFeatureWorkspace.OpenFeatureClass("TRPHZTB");
                (tarFeatureClass as ITable).DeleteSearchedRows(null);
                dissoveFeatureClassPH(sourFeatureClass, "TRPHZJB", "TRPHZ", tarFeatureClass, "TRPHZXX", "TRPHZSX");
            }
            //生物多样性
            if (chkSWDYX.Checked)
            {
                IFeatureClass tarFeatureClass = pFeatureWorkspace.OpenFeatureClass("SWDYX");
                (tarFeatureClass as ITable).DeleteSearchedRows(null);
                dissoveFeatureClass(sourFeatureClass, "TRSWDYXJB", tarFeatureClass, "SWDYX");
            }
            //土壤重金属污染状况
            if (chkTRZJS.Checked)
            {
                IFeatureClass tarFeatureClass = pFeatureWorkspace.OpenFeatureClass("TRZJSWRZK");
                (tarFeatureClass as ITable).DeleteSearchedRows(null);
                dissoveFeatureClass(sourFeatureClass, "TRZJSWRZKJB", tarFeatureClass, "TRZJSWRZK");
            }
            wait.Close();
            MessageBox.Show("数据融合完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dissoveFeatureClass(IFeatureClass sourFeatureClass, string groupField, IFeatureClass tarFeatureClass, string tarField)
        {
            ArrayList dms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(sourFeatureClass, null, groupField);
            IFeatureCursor pInsert = tarFeatureClass.Insert(true);
            foreach (string dm in dms)
            {
                if (!string.IsNullOrWhiteSpace(dm))
                {
                    IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(sourFeatureClass, groupField + " = '" + dm + "'");
                    List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (sourFeatureClass as IGeoDataset).SpatialReference);
                    foreach (IGeometry item in pCJs)
                    {
                        IFeatureBuffer pFeatureBuffer = tarFeatureClass.CreateFeatureBuffer();
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField(tarField), dm);
                        pFeatureBuffer.Shape = item;
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                    pInsert.Flush();
                }
            }
        }

        private void dissoveFeatureClassPH(IFeatureClass sourFeatureClass, string groupField, string valField, IFeatureClass tarFeatureClass, string minField, string maxField)
        {
            ArrayList dms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(sourFeatureClass, null, groupField);
            IFeatureLayer featureLayer = new FeatureLayerClass();
            featureLayer.FeatureClass = sourFeatureClass;
            IFeatureCursor pInsert = tarFeatureClass.Insert(true);
            foreach (string dm in dms)
            {
                if (!string.IsNullOrWhiteSpace(dm))
                {
                    IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(sourFeatureClass, groupField + " = '" + dm + "'");
                    List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (sourFeatureClass as IGeoDataset).SpatialReference);
                    foreach (IGeometry item in pCJs)
                    {
                        IRelationalOperator pRel = item as IRelationalOperator;
                        float min = 0;
                        float max = 0;
                        IIdentify dltbIndentify = featureLayer as IIdentify;
                        IArray arrDltbIDs = dltbIndentify.Identify(item);
                        for (int i = 0; i < arrDltbIDs.Count; i++)
                        {
                            IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature pfea = pRow.Row as IFeature;
                            IArea pArea = pfea.ShapeCopy as IArea;
                            IPoint pPoint = pArea.LabelPoint;
                            if (pRel.Contains(pPoint))
                            {
                                string val = FeatureHelper.GetFeatureStringValue(pfea, valField);
                                float iVal = 0;
                                float.TryParse(val, out iVal);
                                if (min == 0 || min > iVal)
                                {
                                    min = iVal;
                                }
                                if (max == 0 || max > iVal)
                                {
                                    max = iVal;
                                }
                            }
                        }
                        IFeatureBuffer pFeatureBuffer = tarFeatureClass.CreateFeatureBuffer();
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField(minField), min);
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField(maxField), max);
                        pFeatureBuffer.Shape = item;
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                    pInsert.Flush();
                }
            }
        }

        private void dissoveFeatureClass(IFeatureClass sourFeatureClass, string groupField, string valField, IFeatureClass tarFeatureClass, string minField, string maxField)
        {
            ArrayList dms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(sourFeatureClass, null, groupField);
            IFeatureLayer featureLayer = new FeatureLayerClass();
            featureLayer.FeatureClass = sourFeatureClass;
            IFeatureCursor pInsert = tarFeatureClass.Insert(true);
            foreach (string dm in dms)
            {
                if (!string.IsNullOrWhiteSpace(dm))
                {
                    IGeometry pCJ = RCIS.GISCommon.GeometryHelper.MergeGeometry(sourFeatureClass, groupField + " = '" + dm + "'");
                    List<IGeometry> pCJs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pCJ, (sourFeatureClass as IGeoDataset).SpatialReference);
                    foreach (IGeometry item in pCJs)
                    {
                        IRelationalOperator pRel = item as IRelationalOperator;
                        int min = 0;
                        int max = 0;
                        IIdentify dltbIndentify = featureLayer as IIdentify;
                        IArray arrDltbIDs = dltbIndentify.Identify(item);
                        for (int i = 0; i < arrDltbIDs.Count; i++)
                        {
                            IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature pfea = pRow.Row as IFeature;
                            IArea pArea = pfea.ShapeCopy as IArea;
                            IPoint pPoint = pArea.LabelPoint;
                            if (pRel.Contains(pPoint))
                            {
                                string val = FeatureHelper.GetFeatureStringValue(pfea, valField);
                                int iVal = 0;
                                int.TryParse(val, out iVal);
                                if (min == 0 || min > iVal)
                                {
                                    min = iVal;
                                }
                                if (max == 0 || max > iVal)
                                {
                                    max = iVal;
                                }
                            }
                        }
                        IFeatureBuffer pFeatureBuffer = tarFeatureClass.CreateFeatureBuffer();
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField(minField), min);
                        pFeatureBuffer.set_Value(pFeatureBuffer.Fields.FindField(maxField), max);
                        pFeatureBuffer.Shape = item;
                        pInsert.InsertFeature(pFeatureBuffer);
                    }
                    pInsert.Flush();
                }
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            chkSWDYX.Checked = chkAll.Checked;
            chkTCHD.Checked = chkAll.Checked;
            chkTRPH.Checked = chkAll.Checked;
            //chkTRZD.Checked = chkAll.Checked;
            chkTRZJS.Checked = chkAll.Checked;
            chkYJZHL.Checked = chkAll.Checked;
        }
    }
}
