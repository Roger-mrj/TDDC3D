using System;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using System.Collections;


namespace TDDC3D.edit
{
    public partial class SmoothPolygonForm : Form
    {
        public SmoothPolygonForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
        public IMap currMap = null;
        public IWorkspace currWS = null;

        private void SmoothPolygonForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap, true, esriGeometryType.esriGeometryPolygon);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string layerName = this.cmbLayers.Text.Trim();
            if (layerName == "")
                return;
            layerName = OtherHelper.GetLeftName(layerName);
            double threshold = (double)spinEdit1.Value;

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍后...", "正在进行平滑，请稍等...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                int icount = 0;
                IGeoFeatureLayer curLyr = LayerHelper.QueryLayerByModelName(this.currMap, layerName);
                ArrayList arr = LayerHelper.GetSelectedFeature(this.currMap, curLyr, esriGeometryType.esriGeometryPolygon);
                foreach (IFeature aTb in arr)
                {
                    wait.SetCaption("正在处理要素" + aTb.OID);
                    IPolygon apolygon = aTb.ShapeCopy as IPolygon;
                    apolygon.Smooth(threshold);
                    aTb.Shape = apolygon;
                    aTb.Store();
                    icount++;
                }

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("smooth");
                wait.Close();
                MessageBox.Show("执行完毕！共处理要素" + icount + "个。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (this.currMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewAll, null, (this.currMap as IActiveView).Extent);

            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

        }



    }
}
