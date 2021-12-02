using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geodatabase;


namespace RCIS.MapTool
{

    public enum PointType
    {
        Null,
        ImgPoint = 0,
        GeoPoint       
    }

    /// <summary>
    /// Summary description for GeoReferenceTool.
    /// </summary>
    [Guid("525ff2c1-a74e-4b2c-bacf-f09e20b11150")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.GeoReferenceTool")]
    public sealed class GeoReferenceTool : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;

        public GeoReferenceTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        private IRasterLayer m_pRasterLayer;
        public IGeoReference GeoReferenceObject
        {
            get
            {
                return this.m_pRasterLayer as IGeoReference;
            }
        }
        public IRasterLayer CurrentLayer
        {
            get
            {
                return this.m_pRasterLayer;
            }
            set
            {
                this.m_pRasterLayer = value;
            }
        }        

        private static GeoReferenceToolbarFrm frm = null;
        private PointType m_ptType = PointType.Null;

        private IPointCollection m_imgPtCol = new MultipointClass();  //影像点
        private IPointCollection m_geoPtCol = new MultipointClass();  //矢量点
        private ISymbol m_imgSymbol;
        private ISymbol m_geoSymbol;
        private ISymbol m_lineSymbol;



        public PointType PointType
        {
            get
            {
                return this.m_ptType;
            }
            set
            {
                this.m_ptType = value;
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code

            this.m_imgSymbol = SymbolHelper.CreateSymbolFromColor
            (StyleClass.StyleClassMarker, ColorHelper.CreateColor(255, 0, 0));
            (this.m_imgSymbol as IMarkerSymbol).Size = 4;
            this.m_geoSymbol = SymbolHelper.CreateSymbolFromColor
            (StyleClass.StyleClassMarker, ColorHelper.CreateColor(0, 0, 255));
            (this.m_geoSymbol as IMarkerSymbol).Size = 4;
            this.m_lineSymbol = SymbolHelper.CreateLineDirectionSymbol() as ISymbol;
            this.m_cursor = Cursors.Cross;

           


        }

        private GeoReferenceToolbarFrm GetFrm()
        {
            if (frm == null || frm.IsDisposed)
                frm = new GeoReferenceToolbarFrm(m_hookHelper);
            this.CurrentLayer = frm.currRasterLyr;
          

            return frm;

        }
        private void PutImgPoint(IPoint imgPt)
        {
            if (this.m_imgPtCol == null)
                this.m_imgPtCol = new MultipointClass();
            if (imgPt != null)
                this.m_imgPtCol.AddPoints(1, ref imgPt);
        }
        private void PutGeoPoint(IPoint geoPt)
        {
            if (this.m_geoPtCol == null)
            {
                this.m_geoPtCol = new MultipointClass();
            }
            if (geoPt != null)
                this.m_geoPtCol.AddPoints(1, ref geoPt);
        }
        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add GeoReferenceTool.OnClick implementation
            frm = GetFrm();        
            frm.Show();
            frm.TopMost = true;
            frm.geoRefEvent += new Global.GeoReference_EventHandler(this.MakeRef); //触发事件
            frm.clearPtEvent += new Global.GeoReference_ClearPoint_EventHandler(this.ClearPoints);
            frm.registerEvent += new Global.GeoReference_Register_EventHandler(this.Register);
            frm.resetEvent += new Global.GeoReference_Reset_EventHandler(this.Reset);
        }
        public void ClearPoints()
        {
            this.m_imgPtCol = new MultipointClass();
            this.m_geoPtCol = new MultipointClass();
            this.m_ptType = PointType.Null;
            this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography
                    , null, this.m_hookHelper.ActiveView.Extent);
        }
        public void Register()
        {
            if (this.m_pRasterLayer != null)
            {
                this.GeoReferenceObject.Register();
                this.ClearPoints();
                IEnvelope aCurEnv = this.CurrentLayer.AreaOfInterest;
                aCurEnv.Expand(2, 2, true);
                this.m_hookHelper.ActiveView.Extent = aCurEnv;
            }
        }

        public void Reset()
        {
            if (this.m_pRasterLayer != null)
            {
                IGeoReference aRefObj = this.m_pRasterLayer
                as IGeoReference;
                aRefObj.Reset();
                this.ClearPoints();
                
            }
        }

        public  void MakeRef()
        {
            if (this.m_pRasterLayer == null) return;
            if (this.m_geoPtCol == null
                || this.m_imgPtCol == null)
            {
                return;
            }


            if (!this.m_pRasterLayer.Valid)
            {
                MessageBox.Show("该影像图层处于未知状态,不能用来配准!",
                    "影像配准", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IGeoReference geoRef = this.m_pRasterLayer as IGeoReference;
            if (!geoRef.CanGeoRef)
            {
                MessageBox.Show("该影像图层不能在ArcGIS系统中配准!",
                    "影像配准", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            

            int aImgPtCount = this.m_imgPtCol.PointCount;
            int aGeoPtCount = this.m_geoPtCol.PointCount;
            if (aImgPtCount != aGeoPtCount)
            {
                MessageBox.Show("影像点和矢量点个数不一致."
                    + "\n 影像点数目:" + aImgPtCount
                    + "\n 矢量点数目:" + aGeoPtCount);
                return;
            }
           
            this.m_imgPtCol = geoRef.PointsTransform(this.m_imgPtCol, false);
            if (aImgPtCount == 1)
            {
                IPoint imgPt = this.m_imgPtCol.get_Point(0);
                IPoint geoPt = this.m_geoPtCol.get_Point(0);
                double deltaX = imgPt.X - geoPt.X;
                double deltaY = imgPt.Y - geoPt.Y;
                geoRef.Shift(-deltaX, -deltaY);
            }
            else if (aImgPtCount == 2)
            {
                geoRef.TwoPointsAdjust(this.m_imgPtCol, this.m_geoPtCol);
            }
            else if (aImgPtCount >= 3)
            {
                geoRef.Warp(this.m_imgPtCol, this.m_geoPtCol, 0);
            }
            //this.GeoReferenceObject.Register();
            this.ClearPoints();
            //IEnvelope aCurEnv = this.m_pRasterLayer.AreaOfInterest;
            //aCurEnv.Expand(2, 2, true);
            //this.m_hookHelper.ActiveView.Extent = aCurEnv;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            frm = GetFrm();
            frm.Show();
            frm.TopMost = true;

            // TODO:  Add GeoReferenceTool.OnMouseDown implementation
            if (this.CurrentLayer == null) return;
            this.m_ptType = frm.PtType;
            if (Button == 1)
            {
                IPoint aMapPt = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation
                .ToMapPoint(X, Y);
               
                if (this.m_ptType == PointType.GeoPoint)
                {
                    this.PutGeoPoint(aMapPt);
                    this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography
                    , null, this.m_hookHelper.ActiveView.Extent);
                }
                else if (this.m_ptType == PointType.ImgPoint)
                {
                    this.PutImgPoint(aMapPt);
                    this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography
                    , null, this.m_hookHelper.ActiveView.Extent);
                }
            }

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add GeoReferenceTool.OnMouseMove implementation
            
            

        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add GeoReferenceTool.OnMouseUp implementation
        }

        public override void Refresh(int hDC)
        {
            base.Refresh(hDC);
            if (this.CurrentLayer == null) return;
            IScreenDisplay disp = this.m_hookHelper.ActiveView.ScreenDisplay;
            for (int pi = 0; pi < this.m_imgPtCol.PointCount; pi++)
            {
                IPoint aImgPt = this.m_imgPtCol.get_Point(pi);
                DisplayHelper.DrawGeometry(disp, aImgPt, this.m_imgSymbol);
            }
            for (int pi = 0; pi < this.m_geoPtCol.PointCount; pi++)
            {
                IPoint aGeoPt = this.m_geoPtCol.get_Point(pi);
                DisplayHelper.DrawGeometry(disp, aGeoPt, this.m_geoSymbol);
            }
            for (int pi = 0; pi < this.m_imgPtCol.PointCount && pi < this.m_geoPtCol.PointCount; pi++)
            {
                IPolyline aLine = new PolylineClass();
                aLine.FromPoint = this.m_imgPtCol.get_Point(pi);
                aLine.ToPoint = this.m_geoPtCol.get_Point(pi);
                DisplayHelper.DrawGeometry(disp, aLine, this.m_lineSymbol);
            }
        }

        public override bool Deactivate()
        {
            

            frm.geoRefEvent -= new Global.GeoReference_EventHandler(this.MakeRef); //触发事件
            frm.clearPtEvent -= new Global.GeoReference_ClearPoint_EventHandler(this.ClearPoints);
            frm.registerEvent -= new Global.GeoReference_Register_EventHandler(this.Register);
            frm.resetEvent -= new Global.GeoReference_Reset_EventHandler(this.Reset);

            if (frm != null)
            {
                frm.Close();
                frm = null;
                
            }

            
            return true;
        }

        #endregion
    }
}
