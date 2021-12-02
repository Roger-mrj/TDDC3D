using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesRaster;


namespace TDDC3D.raster
{
    public partial class RasterSlopeForm : Form
    {
        public RasterSlopeForm()
        {
            InitializeComponent();
        }

        private IMap currMap = null;

        /// <summary>
        /// 当前地图对象
        /// </summary>
        public IMap CurrMap
        {
            get { return currMap; }
            set { currMap = value; }
        }

        private void RasterSlopeForm_Load(object sender, EventArgs e)
        {
            this.cmbLayers.Properties.Items.Clear();
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
            if (this.cmbLayers.Properties.Items.Count > 0)
            {
                this.cmbLayers.SelectedIndex = 0;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beDestRasterPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "GRID文件|*.*";
            dlg.FileName =  "SLP";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destfile = dlg.FileName;
            this.beDestRasterPath.Text = destfile;
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string inRastername=this.cmbLayers.Text.Trim();
            ILayer currLyr=null;
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer aLyr = currMap.get_Layer(i);
                if (currLyr is IRasterLayer)
                {
                    if (aLyr.Name.ToUpper() == inRastername.ToUpper())
                    {
                        currLyr = aLyr;
                        break;
                    }    
                }
                else if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pCL = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < pCL.Count; kk++)
                    {
                        ILayer childLyr = pCL.get_Layer(kk);
                        if (childLyr is RasterLayer)
                        {
                            if (childLyr.Name.ToUpper() == inRastername.ToUpper())
                            {
                                currLyr = aLyr;
                                break;
                            }
                        }
                    }
                }

                              

            }


            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (currLyr == null) return;
                IRaster currRaster = (currLyr as IRasterLayer).Raster;
                IGeoDataset pGEoDs = currRaster as IGeoDataset;

                string sDir = System.IO.Path.GetDirectoryName(this.beDestRasterPath.Text);
                string sOutName = System.IO.Path.GetFileName(this.beDestRasterPath.Text);
                
                IWorkspace destWs = RCIS.GISCommon.WorkspaceHelper2.GetRasterWorkspace(sDir);

                RCIS.GISCommon.RasterHelper.ProduceSlopeData(sOutName, sDir, "DEGREE", 1, pGEoDs, destWs);
                IRasterLayer rasterLayer = new RasterLayerClass();
                rasterLayer.CreateFromFilePath(this.beDestRasterPath.Text);
                this.currMap.AddLayer(rasterLayer);
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.Enabled = true;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
            }
           

        }
    }
}
