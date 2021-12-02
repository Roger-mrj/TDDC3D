using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using RCIS.Query;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;


namespace RCIS
{
    /// <summary>
    /// Summary description for RedLineTool.
    /// </summary>
    [Guid("a3900423-faa0-4b8c-84c4-48501a60d042")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.RedLineTool")]
    public sealed class RedLineTool : BaseTool
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

        public SpatialQueryForm2 m_UseForm;
        public string m_sDrawType;			//"Point" ,"Line" , "Polygon";
        public string m_sOper;					//"Draw", "Select"


        public RedLineTool()
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
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add RedLineTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {

            if (Button == 1)
            {
                sycCommonLib.sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();

                IMap myMap = this.m_hookHelper.ActiveView.FocusMap;
                IActiveView act = this.m_hookHelper.ActiveView.FocusMap as IActiveView;

                #region 交互画实体
                if (m_sOper.Equals("Draw") == true)
                {
                    IGeometry myGeo = null;
                    if (m_sDrawType.Equals("Point") == true)
                    {
                        IRubberBand pRubberBand = new RubberPointClass();
                        myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    }
                    else if (m_sDrawType.Equals("Line") == true)
                    {
                        IRubberBand pRubberBand = new RubberLineClass();
                        myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    }
                    else if (m_sDrawType.Equals("Polygon") == true)
                    {
                        IRubberBand pRubberBand = new RubberPolygonClass();
                        myGeo = pRubberBand.TrackNew(act.ScreenDisplay, null);
                    }
                    else
                    {
                        MessageBox.Show("无法确认待绘制的实体类型: " + m_sDrawType + " !");
                        m_UseForm.Visible = true;
                        return;
                    }
                    if (myGeo == null)
                    {
                        MessageBox.Show(" 交互绘制实体错误! ");
                        m_UseForm.Visible = true;
                        return;
                    }
                    object mySymbol = Type.Missing;
                    IMapControl2  mapControl=this.m_hookHelper.Hook as IMapControl2;
                    if (mapControl != null)
                    {
                        mapControl.FlashShape(myGeo, 5, 300, mySymbol);
                    }

                    //在MapContrl上画这个元素:
                    IElement createEle = null;
                    if (true)
                    {
                        IGraphicsContainer mapCon = act.GraphicsContainer;
                        mapCon.DeleteAllElements();

                        ISymbol needSym = CommonClass.syc_CreateDefaultSymbol(myGeo.GeometryType);
                        if (myGeo.GeometryType == esriGeometryType.esriGeometryPoint)
                        {
                            IElement element = new MarkerElementClass();
                            element.Geometry = myGeo;
                            IMarkerElement markerElement = (IMarkerElement)element;
                            markerElement.Symbol = needSym as IMarkerSymbol;
                            mapCon.AddElement(element, 0);
                            createEle = element;
                        }
                        else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            LineElementClass LineEle = new LineElementClass();
                            LineEle.Geometry = (IGeometry)myGeo;
                            LineEle.Symbol = needSym as ILineSymbol;
                            mapCon.AddElement(LineEle, 0);
                            createEle = LineEle;
                        }
                        else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            PolygonElementClass polyEle = new PolygonElementClass();
                            polyEle.Geometry = myGeo;
                            polyEle.Symbol = needSym as IFillSymbol;
                            mapCon.AddElement(polyEle, 0);
                            createEle = polyEle;
                        }
                        act.PartialRefresh(esriViewDrawPhase.esriViewBackground, createEle, act.Extent);
                    }

                    //把myGeo的坐标保存起来:
                    string sOutFileName = Application.StartupPath + @"\空间分析.TXT";
                    StreamWriter sw = new StreamWriter(sOutFileName);
                    if (m_sDrawType.Equals("Point") == true)
                    {
                        IPoint pp = myGeo as IPoint;
                        string sXY = pp.X.ToString("F3") + "," + pp.Y.ToString("F3");
                        sw.WriteLine(sXY);
                    }
                    else
                    {
                        //Line or Polygon:
                        IPointCollection pCol = myGeo as IPointCollection;
                        for (int i = 0; i < pCol.PointCount; i++)
                        {
                            IPoint pp = pCol.get_Point(i);
                            string sXY = pp.X.ToString("F3") + "," + pp.Y.ToString("F3");
                            sw.WriteLine(sXY);
                        }
                    }
                    sw.Close();
                    m_UseForm.m_SelGeo = myGeo;
                    m_UseForm.m_CreateEle = createEle;
                    m_UseForm.Visible = true;
                }
                #endregion


                #region 选择存在的实体
                if (m_sOper.Equals("Select") == true)
                {
                    IPoint pp = act.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    IEnvelope pSearchEnvelope = new EnvelopeClass();
                    pSearchEnvelope.XMin = pp.X - 0.1;
                    pSearchEnvelope.YMin = pp.Y - 0.1;
                    pSearchEnvelope.XMax = pp.X + 0.1;
                    pSearchEnvelope.YMax = pp.Y + 0.1;
                    IFeature pNeedFeature = null;
                    esriGeometryType esriType;
                    if (m_sDrawType.Equals("Point") == true)
                        esriType = esriGeometryType.esriGeometryPoint;
                    else if (m_sDrawType.Equals("Line") == true)
                        esriType = esriGeometryType.esriGeometryPolyline;
                    else if (m_sDrawType.Equals("Polygon") == true)
                        esriType = esriGeometryType.esriGeometryPolygon;
                    else
                    {
                        MessageBox.Show("传入的类型错误!");
                        m_UseForm.Visible = true;
                        return;
                    }
                    CommonClass.syc_GetNearestObject(myMap, pSearchEnvelope, esriType, ref pNeedFeature);
                    if (pNeedFeature == null)
                    {
                        MessageBox.Show("没选择到需要的实体!");
                        m_UseForm.Visible = true;
                        return;
                    }

                    IGeometry myGeo = pNeedFeature.ShapeCopy;
                    object mySymbol = Type.Missing;
                    IMapControl2 myMapControl = this.m_hookHelper.Hook as IMapControl2;
                    if (myMapControl != null)
                    {
                        myMapControl.FlashShape(myGeo, 5, 300, mySymbol);
                    }
                    

                    //根据选择的集合、建立一Ele:
                    IElement createEle = null;
                    if (true)
                    {
                        IGraphicsContainer mapCon = act.GraphicsContainer;
                        mapCon.DeleteAllElements();

                        ISymbol needSym = CommonClass.syc_CreateDefaultSymbol(myGeo.GeometryType);
                        if (myGeo.GeometryType == esriGeometryType.esriGeometryPoint)
                        {
                            IElement element = new MarkerElementClass();
                            element.Geometry = myGeo;
                            IMarkerElement markerElement = (IMarkerElement)element;
                            markerElement.Symbol = needSym as IMarkerSymbol;
                            mapCon.AddElement(element, 0);
                            createEle = element;
                        }
                        else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            LineElementClass LineEle = new LineElementClass();
                            LineEle.Geometry = (IGeometry)myGeo;
                            LineEle.Symbol = needSym as ILineSymbol;
                            mapCon.AddElement(LineEle, 0);
                            createEle = LineEle;
                        }
                        else if (myGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            PolygonElementClass polyEle = new PolygonElementClass();
                            polyEle.Geometry = myGeo;
                            polyEle.Symbol = needSym as IFillSymbol;
                            mapCon.AddElement(polyEle, 0);
                            createEle = polyEle;
                        }
                        act.PartialRefresh(esriViewDrawPhase.esriViewBackground, createEle, act.Extent);
                    }

                    //把myGeo的坐标保存起来:
                    string sOutFileName = Application.StartupPath + @"\空间分析.TXT";
                    StreamWriter sw = new StreamWriter(sOutFileName);
                    if (m_sDrawType.Equals("Point") == true)
                    {
                        IPoint p1 = myGeo as IPoint;
                        string sXY = p1.X.ToString("F3") + "," + p1.Y.ToString("F3");
                        sw.WriteLine(sXY);
                    }
                    else
                    {
                        //Line or Polygon:
                        IPointCollection pCol = myGeo as IPointCollection;
                        for (int i = 0; i < pCol.PointCount; i++)
                        {
                            IPoint p2 = pCol.get_Point(i);
                            string sXY = p2.X.ToString("F3") + "," + p2.Y.ToString("F3");
                            sw.WriteLine(sXY);
                        }
                    }
                    sw.Close();
                    m_UseForm.m_SelGeo = myGeo;
                    m_UseForm.m_CreateEle = createEle;
                    m_UseForm.Visible = true;
                }
                #endregion

                CommonClass.Dispose();
            }
            base.OnMouseDown(Button, Shift, X, Y);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add RedLineTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add RedLineTool.OnMouseUp implementation
        }
        #endregion
    }
}
