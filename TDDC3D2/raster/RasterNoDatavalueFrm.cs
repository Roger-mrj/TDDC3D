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
using ESRI.ArcGIS.DataSourcesRaster;

namespace TDDC3D.raster
{
    public partial class RasterNoDatavalueFrm : Form
    {
        public RasterNoDatavalueFrm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        public IMap currMap = null;
        private void RasterNoDatavalueFrm_Load(object sender, EventArgs e)
        {
            cmbLayers.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = currMap.get_Layer(i);
                if (currLyr is IRasterLayer)
                {
                    this.cmbLayers.Properties.Items.Add(currLyr.Name);
                }
                else if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pCL = currLyr as ICompositeLayer;
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
                if ((curLayer.Name.ToUpper() == layername.ToUpper()) && (curLayer is IRasterLayer))
                {
                    pLyr = curLayer;
                    break;
                }

            }
            if (pLyr == null)
                return;
            IRasterLayer pRasterLyr = pLyr as IRasterLayer;
            IRasterProps pRstProps = pRasterLyr.Raster as IRasterProps;
            pRstProps.NoDataValue = 0;
            MessageBox.Show("去除完毕!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            Close();

        }
    }
}
