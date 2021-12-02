using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;


namespace TDDC3D.raster
{
    public partial class clipRasterFrm : Form
    {
        public clipRasterFrm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IMap currMap = null;

        private void beDestFilePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "img文件|*.img";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destFile = dlg.FileName;
            this.beDestFilePath.Text = destFile;
        }

        private void clipRasterFrm_Load(object sender, EventArgs e)
        {
            cmbLayers.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = currMap.get_Layer(i);
                if (currLyr is IRasterLayer)
                {
                    this.cmbLayers.Properties.Items.Add(currLyr.Name);
                }
                else if (currLyr is  IGroupLayer)
                {
                    ICompositeLayer pCL=currLyr as ICompositeLayer;
                    for (int kk = 0; kk < pCL.Count; kk++)
                    {
                        ILayer childLyr = pCL.get_Layer(kk);
                        if (childLyr is IRasterLayer)
                        {
                            this.cmbLayers.Properties.Items.Add(childLyr.Name);
                        }
                    }
                }

            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "")
                return;
            if (this.beDestFilePath.Text.Trim() == "")
                return;
            IPolygon  pGeo = null;
            ILayer pLyr = null;
            int layerCount = this.currMap.LayerCount;
            string layername = this.cmbLayers.Text.Trim().ToUpper();
            for (int i = 0; i < layerCount; i++)
            {
                ILayer curLayer = this.currMap.get_Layer(i);
                if (curLayer is IGroupLayer)
                {
                    ICompositeLayer pCL = curLayer as ICompositeLayer;
                    for (int kk = 0; kk < pCL.Count; kk++)
                    {
                        ILayer childLyr = pCL.get_Layer(kk);
                        if ((childLyr.Name.ToUpper() == layername.ToUpper()) && (childLyr is IRasterLayer))
                        {
                            pLyr = curLayer;
                            break;
                        }
                    }
                }
                else
                if ((curLayer.Name.ToUpper() == layername.ToUpper())  && (curLayer is IRasterLayer))
                {
                    pLyr =curLayer;
                    break;
                }

            }
            double dbuffer = 0;
            double.TryParse(this.txtBuffer.Text, out dbuffer);
            if (pLyr == null)
                return;
            IRasterLayer pRasterLyr=pLyr as IRasterLayer;
            if (this.radioGroup1.SelectedIndex == 0)
            {

                IEnumFeature aEnumObj = this.currMap.FeatureSelection as IEnumFeature;
                IFeature currFea = aEnumObj.Next();
                List<IFeature> lstFeas = new List<IFeature>();
                while (currFea != null)
                {
                    lstFeas.Add(currFea);
                    currFea = aEnumObj.Next();
                }
                if (lstFeas.Count == 0)
                {
                    MessageBox.Show("当前没有选择要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (lstFeas[0].Shape is IPolygon) return;
                pGeo = lstFeas[0].ShapeCopy as IPolygon;
            }
            else
            {
                IEnvelope pEnvelope=(this.currMap as IActiveView).Extent;

                IPointCollection ptCols = new PolygonClass();
                
                double xMax = pEnvelope.XMax;
                double xMin = pEnvelope.XMin;
                double yMax = pEnvelope.YMax;
                double yMin = pEnvelope.YMin;
                IPoint pt = new PointClass();
                pt.PutCoords(xMin, yMin);
                ptCols.AddPoint(pt);
                pt = new PointClass();
                pt.PutCoords(xMin, yMax);
                ptCols.AddPoint(pt);

                pt = new PointClass();
                pt.PutCoords(xMax, yMax);
                ptCols.AddPoint(pt);
                pt = new PointClass();
                pt.PutCoords(xMax, yMin);
                ptCols.AddPoint(pt);

                pGeo = ptCols as IPolygon;

            }
            if (pGeo == null || pGeo.IsEmpty)
            {
                MessageBox.Show("找不到正确的图形范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            pGeo = (pGeo as ITopologicalOperator).Buffer(dbuffer) as IPolygon ;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                RasterHelper.RasterClip(pRasterLyr, pGeo, this.beDestFilePath.Text);
                this.Cursor = Cursors.Default;
                MessageBox.Show("剪切成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
