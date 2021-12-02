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
    public partial class RasterInfoForm : Form
    {
        public RasterInfoForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        private IRasterLayer rasterlayer;

        public IRasterLayer Rasterlayer
        {
            get
            {
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
                return pLyr as IRasterLayer;

            }
            set { rasterlayer = value; }
        }

        private void RasterInfoForm_Load(object sender, EventArgs e)
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
            if (this.cmbLayers.Properties.Items.Count > 0)
            {
                this.cmbLayers.SelectedIndex = 0;
            }
        }

        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Rasterlayer == null) return;
            RasterInfoClass ri=new RasterInfoClass();
            
            try
            {

                IRaster pRaster = this.Rasterlayer.Raster;


                IRaster2 pRaster2 = pRaster as IRaster2;
                IRasterDataset pRDS = pRaster2.RasterDataset;
                IRasterPyramid3 pRPyramid = pRDS as IRasterPyramid3;
                IRasterProps rasterProps = pRaster as IRasterProps;

                IRasterBandCollection rasterBandCol = pRaster as IRasterBandCollection;
                
                ri.Colsrows = rasterProps.Width + "," + rasterProps.Height;
                ri.NumberBands = rasterBandCol.Count;
                double dX = rasterProps.MeanCellSize().X; //栅格的宽度
                double dY = rasterProps.MeanCellSize().Y; //栅格的高度
                ri.CellSize = dX + "," + dY;
                IEnvelope extent = rasterProps.Extent; //当前栅格数据集的范围
                ri.Top = extent.YMax;
                ri.Bottom = extent.YMin;
                ri.Left = extent.XMin;
                ri.Right = extent.XMax;
                ri.PixelType = rasterProps.PixelType.ToString();
                #region 位
                switch (rasterProps.PixelType)
                {
                    case rstPixelType.PT_CHAR:
                        ri.PixelDepth = 8;
                        break;
                    case rstPixelType.PT_CLONG:
                        ri.PixelDepth = 8;
                        break;
                    case rstPixelType.PT_LONG:
                        ri.PixelDepth = 32;
                        break;
                    case rstPixelType.PT_SHORT:
                        ri.PixelDepth = 16;
                        break;
                    case rstPixelType.PT_U1:
                        ri.PixelDepth = 1;
                        break;
                    case rstPixelType.PT_U2:
                        ri.PixelDepth = 2;
                        break;
                    case rstPixelType.PT_U4:
                        ri.PixelDepth = 4;
                        break;
                    case rstPixelType.PT_UCHAR:
                        ri.PixelDepth = 8;
                        break;
                    case rstPixelType.PT_ULONG:
                        ri.PixelDepth = 32;
                        break;
                    case rstPixelType.PT_USHORT:
                        ri.PixelDepth = 16;
                        break;
                }
                #endregion

                ri.Format = pRDS.Format;
                ri.PyramidLevel = pRPyramid.PyramidLevel;

                //ri.Colormap = pRaster2.Colormap.ToString();
                IGeoDataset pGeoDs = pRDS as IGeoDataset;
                ISpatialReference pSR = pGeoDs.SpatialReference;
                if (pSR != null)
                {
                    ri.SpatialRefName = pSR.Name;
                    IProjectedCoordinateSystem prjSR = pSR as IProjectedCoordinateSystem;
                    IGeographicCoordinateSystem gcs = prjSR.GeographicCoordinateSystem;
                    ri.Project = prjSR.Projection.Name;
                    ri.Datum = gcs.Datum.Name;
                    ri.CenterMeridian = gcs.PrimeMeridian.Name;
                    ri.SemiMajorAxis = gcs.Datum.Spheroid.SemiMajorAxis;
                    ri.SemiMinorAxis = gcs.Datum.Spheroid.SemiMinorAxis;

                    ri.FalseEasting = prjSR.FalseEasting;
                    ri.FalseNorthing = prjSR.FalseNorthing;
                    
                    this.propertyGridControl1.SelectedObject = ri;

                    
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
