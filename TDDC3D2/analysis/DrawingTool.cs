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
using RCIS.GISCommon;

namespace TDDC3D.analysis
{
    /// <summary>
    /// Summary description for DrawingTool.
    /// </summary>
    [Guid("bd0dbc0f-c9a9-4142-a592-31349620e6f2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.analysis.DrawingTool")]
    public sealed class DrawingTool : BaseTool
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
            GMxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GMxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        //private IGlobeHookHelper m_globeHookHelper = null;
         public bbfxForm m_UseForm;
        //public DrawType m_sDrawType;			//"Point" ,"Line" , "Polygon";
        public string m_sOper = "Draw";
        private IHookHelper m_hookHelper;

        public DrawingTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcGlobe or GlobeControl";  //localizable text 
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

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add DrawingTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add DataExtractTool.OnMouseDown implementation
            if (Button == 1)
            {
                IMap myMap = this.m_hookHelper.ActiveView.FocusMap;
                IActiveView act = this.m_hookHelper.ActiveView.FocusMap as IActiveView;

                #region 交互画实体
                if (m_sOper.Equals("Draw") == true)
                {
                    IGeometry myGeo = null;
                    IRubberBand pRubberBand;
                    //switch (m_sDrawType)
                    //{
                    //    case DrawType.Point:
                    //        pRubberBand = new RubberPointClass();
                    //        myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    //        break;
                    //    case DrawType.Line:
                    //        pRubberBand = new RubberLineClass();
                    //        myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    //        break;
                    //    case DrawType.Circle:
                    //        pRubberBand = new RubberCircleClass();
                    //        myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    //        break;
                    //    case DrawType.Polygon:
                    pRubberBand = new RubberPolygonClass();
                    myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    //        break;
                    //}

                    if (myGeo == null)
                    {
                        MessageBox.Show(" 交互绘制实体错误! ");
                        m_UseForm.Visible = true;
                        return;
                    }
                    object mySymbol = Type.Missing;
                    IMapControl2 mapControl = this.m_hookHelper.Hook as IMapControl2;
                    if (mapControl != null)
                    {
                        if (myGeo.GeometryType == esriGeometryType.esriGeometryCircularArc) myGeo = GeometryHelper.ConvertCircle2Polygon(myGeo as ICircularArc);
                        mapControl.FlashShape(myGeo, 5, 300, mySymbol);
                    }

                    //在MapContrl上画这个元素:
                    IElement createEle = null;
                    if (true)
                    {
                        IGraphicsContainer mapCon = act.GraphicsContainer;
                        mapCon.DeleteAllElements();

                        if (myGeo.GeometryType == esriGeometryType.esriGeometryPoint)
                        {
                            IElement element = new MarkerElementClass();
                            element.Geometry = myGeo;
                            IMarkerElement markerElement = (IMarkerElement)element;
                            mapCon.AddElement(element, 0);
                            createEle = element;
                        }
                        else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            LineElementClass LineEle = new LineElementClass();
                            LineEle.Geometry = (IGeometry)myGeo;
                            mapCon.AddElement(LineEle, 0);
                            createEle = LineEle;
                        }
                        else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            PolygonElementClass polyEle = new PolygonElementClass();
                            polyEle.Geometry = myGeo;
                            mapCon.AddElement(polyEle, 0);
                            createEle = polyEle;
                        }
                        act.PartialRefresh(esriViewDrawPhase.esriViewBackground, createEle, act.Extent);
                    }
                    m_UseForm.m_SelGeo = myGeo;
                    m_UseForm.Visible = true;
                    
                }
                #endregion
            }
            base.OnMouseDown(Button, Shift, X, Y);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add DrawingTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add DrawingTool.OnMouseUp implementation
        }
        #endregion
    }
}
