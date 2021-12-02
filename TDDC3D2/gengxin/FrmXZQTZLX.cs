using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System.Collections;

namespace TDDC3D.gengxin
{
    public partial class FrmXZQTZLX : Form
    {
        public IMap _CurMap;

        public FrmXZQTZLX()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmXZQTZLX_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(cboXZQ, _CurMap);
            LayerHelper.LoadLayer2Combox(cboDLTBGX, _CurMap);
            LayerHelper.LoadLayer2Combox(cboDLTBGX2, _CurMap);
            for (int i = 0; i < cboXZQ.Properties.Items.Count; i++)
            {
                string layerName = cboXZQ.Properties.Items[i].ToString();
                string talleName = layerName.Split('|')[0];
                if (talleName == "XZQ")
                {
                    cboXZQ.Text = layerName;
                    break;
                }
            }
            for (int i = 0; i < cboDLTBGX.Properties.Items.Count; i++)
            {
                string layerName = cboDLTBGX.Properties.Items[i].ToString();
                string talleName = layerName.Split('|')[0];
                if (talleName == "DLTBGXGC")
                {
                    cboDLTBGX.Text = layerName;
                    cboDLTBGX2.Text = layerName;
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboDLTBGX.Text) ||
                string.IsNullOrWhiteSpace(cboXZQ.Text))
            {
                MessageBox.Show("请先选择图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureLayer xzqLayer = LayerHelper.QueryLayerByModelName(_CurMap, cboXZQ.Text.Split('|')[0]);
            if (xzqLayer == null) return;
            IFeatureLayer dltbgxLayer = LayerHelper.QueryLayerByModelName(_CurMap, cboDLTBGX.Text.Split('|')[0]);
            if (dltbgxLayer == null) return;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在更新……", "提示");
            wait.Show();
            string fieldName = "XZQTZLX";
            int iXZQ = dltbgxLayer.FeatureClass.Fields.FindField(fieldName);
            if (iXZQ == -1)
            {
                if ((dltbgxLayer.FeatureClass as IDataset).Name != "DLTBGXGC")
                {
                    wait.Close();
                    MessageBox.Show("选择图层不是地类图斑更新过程层或者该图层没有行政区调整类型字段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                IField pField = EsriDatabaseHelper.CreateTextField(fieldName, "行政区调整类型", 1);
                IClass pClass = dltbgxLayer.FeatureClass as IClass;
                pClass.AddField(pField);
                iXZQ = dltbgxLayer.FeatureClass.Fields.FindField(fieldName);
            }
            
            long total = dltbgxLayer.FeatureClass.FeatureCount(null);
            long inow = 1;
            IIdentify dltbIndentify = xzqLayer as IIdentify;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pUpdate = dltbgxLayer.FeatureClass.Update(null, true);
                comRel.ManageLifetime(pUpdate);
                IFeature pFeature;
                while ((pFeature = pUpdate.NextFeature()) != null)
                {
                    wait.SetCaption(string.Format("正在更新{0}/{1}……", inow++, total));
                    if (pFeature.get_Value(iXZQ) == null || string.IsNullOrWhiteSpace(pFeature.get_Value(iXZQ).ToString()))
                    {
                        Boolean isIntersect = false;
                        ITopologicalOperator pTop = pFeature.ShapeCopy as ITopologicalOperator;
                        IArray arrDltbIDs = dltbIndentify.Identify(pFeature.ShapeCopy);
                        for (int i = 0; i < arrDltbIDs.Count; i++)
                        {
                            IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature pfea = pRow.Row as IFeature;

                            IGeometry pGeo = pTop.Intersect(pfea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                            if (pGeo != null)
                            {
                                IArea pArea = pGeo as IArea;
                                if (pArea.Area > 1)
                                {
                                    isIntersect = true;
                                    break;
                                }
                            }
                        }
                        if (isIntersect)
                        {
                            pFeature.set_Value(iXZQ, "0");
                        }
                        else
                        {
                            pFeature.set_Value(iXZQ, cboValue.Text.Split('|')[0]);
                        }
                        pUpdate.UpdateFeature(pFeature);
                    }
                }
                pUpdate.Flush();
            }
            wait.Close();
            MessageBox.Show("更新完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboDLTBGX2.Text))
            {
                MessageBox.Show("请先选择图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureLayer dltbgxLayer = LayerHelper.QueryLayerByModelName(_CurMap, cboDLTBGX2.Text.Split('|')[0]);
            if (dltbgxLayer == null) return;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在更新……", "提示");
            wait.Show();
            string fieldName = "XZQTZLX";
            int iXZQ = dltbgxLayer.FeatureClass.Fields.FindField(fieldName);
            if (iXZQ == -1)
            {
                IField pField = EsriDatabaseHelper.CreateTextField(fieldName, "行政区调整类型", 1);
                IClass pClass = dltbgxLayer.FeatureClass as IClass;
                pClass.AddField(pField);
                iXZQ = dltbgxLayer.FeatureClass.Fields.FindField(fieldName);
            }
            long total = dltbgxLayer.FeatureClass.FeatureCount(null);
            long inow = 1;
            List<IFeature> selFeatures = LayerHelper.GetSelectedFeature(_CurMap, dltbgxLayer as IGeoFeatureLayer);
            if (selFeatures.Count == 0)
            {
                MessageBox.Show("没有选中的图斑更新层数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                wait.Close();
                return;
            }
            foreach (IFeature pFeature in selFeatures)
            {
                wait.SetCaption(string.Format("正在更新{0}/{1}……", inow++, total));
                //if (pFeature.get_Value(iXZQ) == null || string.IsNullOrWhiteSpace(pFeature.get_Value(iXZQ).ToString()))
                //{
                    pFeature.set_Value(iXZQ, cboValue2.Text.Split('|')[0]);
                    pFeature.Store();
                //}
            }
            wait.Close();
            MessageBox.Show("更新完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
