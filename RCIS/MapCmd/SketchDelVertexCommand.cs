using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.Carto;

namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("3a84494a-0584-4916-826e-f773dd25890e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.SketchDelVertexCommand")]
    public sealed class SketchDelVertexCommand : BaseCommand
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


        private IEngineEditor engineEditor = null;

        public SketchDelVertexCommand(IEngineEditor _value )
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "删除前一结点(&D)";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "删除前一草图结点";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            this.engineEditor = _value;
            try
            {
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
            // TODO: Add SketchDelVertexCommand.OnClick implementation
            IEngineEditSketch m_editSketch = this.engineEditor as IEngineEditSketch;
            IPointCollection ptCol = m_editSketch.Geometry as IPointCollection;
            if (ptCol.PointCount <= 1)
            {
                System.Windows.Forms.MessageBox.Show("只有一个结点，请使用删除草图工具！");
                return;
            }
            if (m_editSketch.Geometry is IPolygon)
            {
                IPoint lastPt = ptCol.get_Point(ptCol.PointCount - 2);
                ptCol.RemovePoints(ptCol.PointCount - 2, 1);
                m_editSketch.VertexDeleted(lastPt);
            }
            else if (m_editSketch.Geometry is IPolyline)
            {
                IPoint lastPt = ptCol.get_Point(ptCol.PointCount - 1);
                ptCol.RemovePoints(ptCol.PointCount - 1, 1);
                m_editSketch.VertexDeleted(lastPt);
            }
            
           
            m_editSketch.ModifySketch();
            m_editSketch.RefreshSketch();
            IMapControl2 mapctl = this.m_hookHelper.Hook as IMapControl2;
            mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll,
                null, mapctl.Extent);

        }

        #endregion
    }
}
