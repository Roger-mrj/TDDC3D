using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using RCIS.Utility;

using RCIS.GISCommon;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;

namespace RCIS.MapTool
{
    /// <summary>
    /// Summary description for GeometryExtentTool.
    /// </summary>
    [Guid("6f9a94a7-ef82-4cc3-8b1d-2e0146f1cef7")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.GeometryExtentTool")]
    public sealed class GeometryExtentTool : BaseTool
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
        private static IGeometry m_CKGeo;
        private IPolyline pPolyline;    //基准线
        IEngineEditor m_engineEditor;

        public GeometryExtentTool(IEngineEditor editor)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "延伸线";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "延伸线";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            this.m_engineEditor = editor;
            MessageBox.Show("先选择基准线(面)，再选择要延伸的线（一次可以多选）");
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
                m_CKGeo = null;
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
            // TODO: Add GeometryExtentTool.OnClick implementation
        }


        private IGeometry  selectBygeo(IFeatureLayer fealayer,IGeometry geo)
        {
            IGeometry reGeo = null;
            if (fealayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint && fealayer.Selectable && fealayer.Visible)
            {
                ISpatialFilter filter = new SpatialFilterClass();
                filter.Geometry = geo;
                filter.GeometryField = "SHAPE";
                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCurosr = fealayer.FeatureClass.Search(filter, false);
                IFeature fea = pCurosr.NextFeature();
                if (fea != null)
                {
                    this.m_hookHelper.ActiveView.FocusMap.ClearSelection();
                    this.m_hookHelper.ActiveView.FocusMap.SelectFeature(fealayer, fea);
                    this.m_hookHelper.ActiveView.Refresh();
                    reGeo = fea.Shape;
                   

                }
                OtherHelper.ReleaseComObject(pCurosr);
            }
            return reGeo;
        }

        public  IGeometry GetSelectGeometry( IGeometry geo)
        {
            IMap map = this.m_hookHelper.ActiveView.FocusMap;
            IGeometry reGeo = null;
            for (int i = 0; i < map.LayerCount; i++)
            {
                ILayer lyr = map.get_Layer(i);
                if (lyr is IGeoFeatureLayer)
                {
                    IFeatureLayer fealayer = lyr as IFeatureLayer;
                    reGeo= selectBygeo(fealayer,geo);
                    if (reGeo != null)
                        break;
                }
                else if (lyr is IGroupLayer)
                {
                    ICompositeLayer pComLyr = lyr as ICompositeLayer;
                    for (int kk = 0; kk < pComLyr.Count; kk++)
                    {
                        ILayer childLyr = pComLyr.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer fealayer = childLyr as IFeatureLayer;
                            reGeo = selectBygeo(fealayer, geo);
                            if (reGeo != null)
                                break;
                        }
                    }
                }
            }
            return reGeo;

        }

        public ArrayList GetSelectGeometryList( IGeometry geo)
        {
            ArrayList list = new ArrayList();
            IMap map = this.m_hookHelper.ActiveView.FocusMap;

            for (int i = 0; i < map.LayerCount; i++)
            {
                ILayer lyr = map.get_Layer(i);
                if (lyr is IGroupLayer)
                {
                    ICompositeLayer pComLyr = lyr as ICompositeLayer;
                    for (int kk = 0; kk < pComLyr.Count; kk++)
                    {
                        ILayer childLyr = pComLyr.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer fealayer = childLyr as IFeatureLayer;
                            if (fealayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline && fealayer.Selectable && fealayer.Visible)
                            {
                                ISpatialFilter filter = new SpatialFilterClass();
                                filter.Geometry = geo;
                                filter.GeometryField = "SHAPE";
                                filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                                IFeatureCursor pCurosr = fealayer.FeatureClass.Search(filter, false);
                                IFeature fea = pCurosr.NextFeature();
                                while (fea != null)
                                {
                                    list.Add(fea);

                                    fea = pCurosr.NextFeature();
                                }
                                OtherHelper.ReleaseComObject(pCurosr);
                            }
                        }
                    }
                }
                else if (lyr is IFeatureLayer)
                {
                    IFeatureLayer fealayer = lyr as IFeatureLayer;
                    if (fealayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline && fealayer.Selectable && fealayer.Visible)
                    {
                        ISpatialFilter filter = new SpatialFilterClass();
                        filter.Geometry = geo;
                        filter.GeometryField = "SHAPE";
                        filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pCurosr = fealayer.FeatureClass.Search(filter, false);
                        IFeature fea = pCurosr.NextFeature();
                        while (fea != null)
                        {
                            list.Add(fea);

                            fea = pCurosr.NextFeature();
                        }
                        OtherHelper.ReleaseComObject(pCurosr);
                    }
                }
            }
            this.m_hookHelper.ActiveView.Refresh();
            return list;

        }


        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add GeometryExtentTool.OnMouseDown implementation
            if (Button == 1)
            {
                if (m_CKGeo == null)
                {
                    IGeometry geo = null;
                    RubberEnvelopeClass pRub = new RubberEnvelopeClass();
                    IActiveView pA = this.m_hookHelper.ActiveView;
                    IEnvelope pE = pRub.TrackNew(pA.ScreenDisplay, null) as IEnvelope;
                    if (pE == null || pE.IsEmpty)
                    {
                        IPoint mapP = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                        geo = (mapP as ITopologicalOperator).Buffer(5);
                    }
                    else
                    {
                        geo = pE as IGeometry;
                    }

                    m_CKGeo = GetSelectGeometry( geo);
                    if (m_CKGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                        pPolyline = m_CKGeo as IPolyline;
                    else
                    {
                        ITopologicalOperator pTopo = m_CKGeo as ITopologicalOperator;
                        pPolyline = pTopo.Boundary as IPolyline;
                    }
                    
                }
                else
                {
                    IGeometry geo = null;
                    RubberEnvelopeClass pRub = new RubberEnvelopeClass();
                    IActiveView pA = this.m_hookHelper.ActiveView;
                    IEnvelope pE = pRub.TrackNew(pA.ScreenDisplay, null) as IEnvelope;
                    if (pE == null || pE.IsEmpty)
                    {
                        IPoint mapP = pA.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                        geo = (mapP as ITopologicalOperator).Buffer(5);
                    }
                    else
                    {
                        geo = pE as IGeometry;
                    }
                    // 
                    #region

                    ArrayList list = GetSelectGeometryList( geo);
                    (pPolyline as ITopologicalOperator).Simplify();

                    this.m_engineEditor.StartOperation();
                    
                    for (int i = 0; i < list.Count; i++)
                    {
                        IFeature fea = list[i] as IFeature;
                        try
                        {
                            ICurve curve = pPolyline as ICurve;
                            ICurve curve1 = fea.Shape as ICurve;
                            IConstructCurve constructCurve = new PolylineClass();
                            bool isExtensionPerfomed = false;
                            constructCurve.ConstructExtended(curve1, curve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);

                            if (isExtensionPerfomed)
                            {
                                fea.Shape = constructCurve as IGeometry;
                                fea.Store();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    m_engineEditor.StopOperation("geometryextent");
                    #endregion
                    this.m_hookHelper.ActiveView.Refresh();
                }
            } //取消
            else
            {
                System.Drawing.Point aPt = new System.Drawing.Point(X, Y);
                m_CKGeo = null;
            }

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add GeometryExtentTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add GeometryExtentTool.OnMouseUp implementation
        }
        #endregion
    }
}
