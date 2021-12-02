using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

using RCIS.MapTool;

namespace RCIS.MapCmd
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("526a6192-ad91-41a3-aa7e-b3bac106e96e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapCmd.AbsolutelyXY2GeoCommand")]
    public sealed class AbsolutelyXY2GeoCommand : BaseCommand
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
        public AbsolutelyXY2GeoCommand(  IEngineEditor _value   )
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "导入绝对坐标";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "导入绝对坐标";  //localizable text
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

        #region Overriden Class Methods

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

        
            // TODO: Add AbsolutelyXY2GeoCommand.OnClick implementation
            Absolutely2GeomtryFrm aForm = new Absolutely2GeomtryFrm(esriGeometryType.esriGeometryPolygon);

            if (aForm.ShowDialog() == DialogResult.OK)
            {
                IGeometry aGeom = aForm.OutputGeometry;
                if (aGeom == null || aGeom.IsEmpty)
                {
                    MessageBox.Show("不能处理所输入的几何图形!", "绝对坐标"
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                   // this.engineEditor.StartOperation();
                    try
                    {

                        IEngineEditSketch m_editSketch = this.engineEditor as IEngineEditSketch;
                        m_editSketch.Geometry = aGeom;
                        //IFeatureLayer currFeaLyr = GlobalEditObject.GetCurrentEditLayer(this.engineEditor);                      

                        //IFeatureClass aclass = currFeaLyr.FeatureClass;
                        //IFeature newFeat = aclass.CreateFeature();
                        //newFeat.Shape = aGeom;
                        //newFeat.Store();

                        //IFeatureSelection select = currFeaLyr as IFeatureSelection;
                        //select.Add(newFeat);

                        m_editSketch.FinishSketch();

                        IMapControl2 mapctl = this.m_hookHelper.Hook as IMapControl2;
                        mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography,
                            null, mapctl.Extent);
                    }
                    catch { MessageBox.Show("生成几何图形不正确，请检查坐标点！"); }
                   // this.engineEditor.StopOperation("absolutexy2geo");

                }
            }
        }

        #endregion
    }
}
