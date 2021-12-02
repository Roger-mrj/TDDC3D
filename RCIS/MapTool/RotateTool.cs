using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace RCIS.MapTool
{
    /// <summary>
    /// Summary description for RotateTool.
    /// </summary>
    [Guid("1ee6aecd-3c6d-4e89-8580-0c101805da7d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.RotateTool")]
    public sealed class RotateTool : BaseTool
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
        private IRotateTracker m_pRotateTracker = null;
        private IActiveView m_pActiveview = null;
        private IFeature m_pCurrFea = null;
        private IMapControl2 m_pMapCtrl = null;


        private IEngineEditor m_globalEditor = null;

        public RotateTool(IEngineEditor engineEditor)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")

            this.m_globalEditor = engineEditor;

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


                m_pActiveview = this.m_hookHelper.ActiveView;
                m_pMapCtrl = this.m_hookHelper.Hook as IMapControl2;



                IMap aMap = m_hookHelper.ActiveView.FocusMap;
                IEnumFeature aEnumObj = aMap.FeatureSelection as IEnumFeature;
                m_pCurrFea = aEnumObj.Next();
                if (m_pCurrFea == null)
                {
                    MessageBox.Show("当前地图没有被选中要素!");
                    m_pMapCtrl.CurrentTool = null;
                    return;
                }

                this.m_pRotateTracker = new EngineRotateTrackerClass();

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
            // TODO: Add RotateTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add RotateTool.OnMouseDown implementation
            try
            {
                IScreenDisplay pScreenDisplay = this.m_pActiveview.ScreenDisplay;
                m_pRotateTracker.Display = pScreenDisplay;
                m_pRotateTracker.ClearGeometry();

                IGeometry pGeo = m_pCurrFea.Extent as IGeometry;
                IArea pArea = pGeo as IArea;
                if (pArea == null)
                {
                    m_pRotateTracker.Origin = m_pCurrFea.Extent.LowerLeft;
                }
                else
                {
                    m_pRotateTracker.Origin = pArea.Centroid;
                }




                m_pRotateTracker.AddGeometry(m_pCurrFea.ShapeCopy);

                if (m_pRotateTracker != null)
                {
                    m_pRotateTracker.OnMouseDown();
                }
            }
            catch (Exception ex)
            {
            }

            base.OnMouseDown(Button, Shift, X, Y);

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add RotateTool.OnMouseMove implementation
            IPoint pPoint = null;
            if (m_pRotateTracker != null)
            {
                if (this.m_pRotateTracker.Origin.IsEmpty != true)
                {
                    pPoint = m_pActiveview.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    m_pRotateTracker.OnMouseMove(pPoint);

                    m_pActiveview.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, m_pActiveview.Extent);

                    m_pRotateTracker.Refresh();
                }


             
            }
            base.OnMouseMove(Button, Shift, X, Y);

        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add RotateTool.OnMouseUp implementation
            bool bChanged;
            ITransform2D pTransform2D;
            if (m_pRotateTracker != null)
            {
                bChanged = m_pRotateTracker.OnMouseUp();
                if (!bChanged)
                {
                    return; ;
                }
                pTransform2D = this.m_pCurrFea.ShapeCopy as ITransform2D;
                pTransform2D.Rotate(m_pRotateTracker.Origin, m_pRotateTracker.Angle);

                IGeometry geo = pTransform2D as IGeometry;
                if (geo == null || geo.IsEmpty)
                {
                    return;
                }
                this.m_globalEditor.StartOperation();

                try
                {
                    this.m_pCurrFea.Shape = geo;
                    this.m_pCurrFea.Store();
                    this.m_globalEditor.StopOperation("rotate");
                    IGraphicsContainer pGraphicsContainer = this.m_pActiveview.GraphicsContainer;
                    pGraphicsContainer.DeleteAllElements();

                    this.m_pRotateTracker = null;
                    this.m_pMapCtrl.CurrentTool = null;// cmd as ESRI.ArcGIS.SystemUI.ITool;
                    m_pActiveview.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography, null, this.m_pActiveview.Extent);
                }
                catch (Exception ex)
                {
                    this.m_globalEditor.AbortOperation();
                }
                
                               
             
            }

            base.OnMouseUp(Button, Shift, X, Y);
        }
        #endregion
    }
}
