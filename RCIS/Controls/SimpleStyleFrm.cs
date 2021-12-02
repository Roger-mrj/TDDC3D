using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;


namespace RCIS.Controls
{
    public partial class SimpleStyleFrm : Form
    {
        public SimpleStyleFrm(IActiveView pView, IGeoFeatureLayer pLayer)
        {
            InitializeComponent();
            this.m_pView = pView;
            this.m_geoLayer = pLayer;
        }

        private IGeoFeatureLayer m_geoLayer;
        private IActiveView m_pView;

        private ISymbol CreateSymbol()
        {
            try
            {
                IColor aFillColor = ColorHelper.CreateColor(this.ceFill.Color);
                IColor aOutlineColor = ColorHelper.CreateColor(this.ceOutline.Color);
                double aSize = 1;
                double.TryParse(this.teSize.Text, System.Globalization.NumberStyles.Any
                    , new System.Globalization.NumberFormatInfo()
                    , out aSize);
                double aWidth = 1;
                double.TryParse(this.teWidth.Text, System.Globalization.NumberStyles.Any
                    , new System.Globalization.NumberFormatInfo()
                    , out aWidth);

                if (this.m_geoLayer == null) return null;
                if (this.m_geoLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                   
                        SimpleMarkerSymbolClass aStyle = new SimpleMarkerSymbolClass();
                        aStyle.Color = aFillColor;
                        aStyle.Size = aSize;
                        return aStyle as ISymbol;
                    
                }
                else if (this.m_geoLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    SimpleLineSymbolClass aStyle = new SimpleLineSymbolClass();
                    aStyle.Color = aOutlineColor;
                    aStyle.Width = aWidth;
                    return aStyle as ISymbol;
                }
                else if (this.m_geoLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    SimpleFillSymbolClass aStyle = new SimpleFillSymbolClass();
                    aStyle.Color = ColorHelper.CreateColor((int)this.ceFill.Color.R, (int)this.ceFill.Color.G, (int)this.ceFill.Color.B);
                    SimpleLineSymbolClass aOutline = new SimpleLineSymbolClass();
                    aOutline.Color = aOutlineColor;
                    aOutline.Width = aWidth;
                    aStyle.Outline = aOutline;

                    if (this.ceFill.Color.Name.ToUpper().Equals("TRANSPARENT"))
                    {
                        aStyle.Style = esriSimpleFillStyle.esriSFSHollow;
                    }

                    return aStyle as ISymbol;
                }
            }
            catch 
            {
            }
            return null;
        }

        private void UpdatePicture()
        {
            try
            {
                ISymbol aSymbol = this.CreateSymbol();
                if (aSymbol != null)
                {
                    Image aImg = SymbolHelper.StyleToImage(
                        aSymbol, this.pbStyle.Width, this.pbStyle.Height);
                    this.pbStyle.Image = aImg;
                }
            }
            catch  { }
        }

        private void ceFill_EditValueChanged(object sender, EventArgs e)
        {
            this.UpdatePicture();
        }

        private void ceOutline_EditValueChanged(object sender, EventArgs e)
        {
            this.UpdatePicture();
        }

        private void teSize_TextChanged(object sender, EventArgs e)
        {
            this.UpdatePicture();
        }

        private void teWidth_TextChanged(object sender, EventArgs e)
        {
            this.UpdatePicture();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ISymbol aSym = this.CreateSymbol();
            if (aSym != null)
            {
                SimpleRendererClass aRender = new SimpleRendererClass();
                aRender.Symbol = aSym;
                this.m_geoLayer.Renderer = aRender as IFeatureRenderer;
                if (this.m_pView != null)
                {
                    this.m_pView.PartialRefresh(esriViewDrawPhase.esriViewGeography
                        , this.m_geoLayer, this.m_pView.Extent.Envelope);
                }
            }
            this.Close();
        }

    }
}