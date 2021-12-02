using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;

namespace RCIS.MapTool
{
    public partial class DeltaXYForm : Form
    {
        public DeltaXYForm()
        {
            InitializeComponent();
        }

        public IEngineEditSketch m_editSketch;
        private IActiveView m_av;
        private IMapControl3 m_mapctl = null;

        public IMapControl3 Mapctl
        {
            get { return m_mapctl; }
            set { m_mapctl = value; }
        }
        public IActiveView currentActiveview
        {
            get { return m_av; }
            set { m_av = value; }
        }


        private IPoint m_resultPt;

        public IPoint ResultPt
        {
            get { return m_resultPt; }
            set { m_resultPt = value; }
        }

        private void DeltaXYForm_Load(object sender, EventArgs e)
        {
            IGeometry geo = this.m_editSketch.Geometry;
            IPointCollection ptCol = geo as IPointCollection;
            if (ptCol.PointCount > 1)
            {
                IPoint firstPt = ptCol.get_Point(ptCol.PointCount-2);
                IPoint secondPt = ptCol.get_Point(ptCol.PointCount-1);

                this.spinEditX.Value =(decimal)( secondPt.X - firstPt.X);
                this.spinEditY.Value =(decimal)( secondPt.Y - firstPt.Y);
            }

        }


        private  ISimpleMarkerSymbol getMarkerSymbol()
        {
            ISimpleMarkerSymbol m_markerSym = new SimpleMarkerSymbolClass();
            m_markerSym.Color = ColorHelper.CreateColor(255, 0, 0);
            m_markerSym.Style = esriSimpleMarkerStyle.esriSMSSquare;
            m_markerSym.OutlineColor = ColorHelper.CreateColor(255, 255, 0);
            m_markerSym.OutlineSize = 1;
            m_markerSym.Outline = true;
            m_markerSym.Size = 6;
            return m_markerSym;
        }

        private IPoint getPoint()
        {
            double deltX = (double)this.spinEditX.Value;
            double deltY = (double)this.spinEditY.Value;
            IPoint aPt = new PointClass();
            aPt.PutCoords(deltX, deltY);

            IGeometry geo = this.m_editSketch.Geometry;
            IPointCollection ptCol = geo as IPointCollection;
            if (ptCol.PointCount > 1)
            {
                IPoint firstPt = ptCol.get_Point(ptCol.PointCount - 2);
                aPt.X = aPt.X + firstPt.X;
                aPt.Y = aPt.Y + firstPt.Y;
            }
            return aPt;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            //预览
            ISimpleMarkerSymbol sym = getMarkerSymbol();
            

            this.m_av.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            this.m_av.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            IPoint aPt = getPoint();
            DisplayHelper.DrawGeometry(this.m_av.ScreenDisplay, aPt, sym as ISymbol);

            
            this.m_av.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            this.Mapctl.FlashShape(aPt, 3, 300, sym);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.m_resultPt = getPoint();

            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
