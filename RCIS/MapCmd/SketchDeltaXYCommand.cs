using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("751a78df-8294-4f17-a86c-0da7be939e17")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.SketchDeltaXYCommand")]
    public sealed class SketchDeltaXYCommand : BaseCommand
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

        IEngineEditor m_engineEditor;
        IEngineEditSketch m_editSketch;
        IMapControl3 mapctl=null;

        private IHookHelper m_hookHelper = null;
        public SketchDeltaXYCommand(IEngineEditor editor,IMapControl3 _mapctl)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "增量 XY";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "增量 XY";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                this.m_engineEditor = editor;
                this.m_editSketch = m_engineEditor as IEngineEditSketch;
                this.mapctl = _mapctl;

                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods


        public override bool Enabled
        {
            get
            {
                if (this.m_editSketch == null)
                    return false;
                else if (this.m_editSketch.Geometry == null)
                    return false;
                else if ((this.m_editSketch.Geometry as IPointCollection).PointCount < 2)
                    return false;
                else return true;
                
            }
        }

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
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
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add SketchDeltaXYCommand.OnClick implementation
            DeltaXYForm frm = new DeltaXYForm();
            frm.currentActiveview = this.m_hookHelper.ActiveView;
            frm.m_editSketch = this.m_editSketch;
            frm.Mapctl = this.mapctl;
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.ResultPt != null)
                {
                    this.m_editSketch.VertexDeleted(this.m_editSketch.LastPoint);
                    this.m_editSketch.AddPoint(frm.ResultPt, true);

                    this.m_hookHelper.ActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewAll, null, null);
                }

            }
        }

        #endregion
    }
}
