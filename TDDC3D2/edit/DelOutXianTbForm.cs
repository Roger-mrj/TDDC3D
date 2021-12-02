using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.edit
{
    public partial class DelOutXianTbForm : Form
    {
        public DelOutXianTbForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        public IMap currMap = null;
        private void DelOutXianTbForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap);
            LayerHelper.LoadLayer2Combox(this.cmbXZQLayer, this.currMap, esriGeometryType.esriGeometryPolygon);

            int idx = -1;
            for (int i = 0; i < this.cmbXZQLayer.Properties.Items.Count; i++)
            {
                if (this.cmbXZQLayer.Properties.Items[i].ToString().Trim().ToUpper().StartsWith("XZQ"))
                {
                    idx = i;
                    break;
                }
            }
            this.cmbXZQLayer.SelectedIndex = idx;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private List<IFeature> lstSelFeas = new List<IFeature>();
        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "") return;
            if (this.cmbXZQLayer.Text.Trim() == "") return;

            string fcName = this.cmbLayers.Text.Trim();
            fcName = OtherHelper.GetLeftName(fcName);

            string xzqName = OtherHelper.GetLeftName(this.cmbXZQLayer.Text.Trim());

            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(fcName);
            IFeatureClass xzqClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(xzqName);
            //查找行政区外面的图斑
            DevExpress.Utils.WaitDialogForm frm = new DevExpress.Utils.WaitDialogForm("正在查询...", "请稍等...");
            frm.Show();
            lstSelFeas.Clear();

            IGeometry xzqGeo = GeometryHelper.MergeGeometry(xzqClass);
            lstSelFeas = GetFeaturesHelper.getInsectXzqLIneFeature(pFC, xzqGeo);
            this.lblNum.Text = "当前共有" + lstSelFeas.Count + "个要素符合条件";
            frm.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (IFeature aFea in lstSelFeas)
                {
                    aFea.Delete();
                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("delteoutxzqfeature");
                
                MessageBox.Show("删除完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
        }
    }
}
