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
    [Guid("0b145dc4-a930-48cf-9e93-11abfbf83091")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.LayerTableCommand2")]
    public sealed class LayerTableCommand2 : BaseCommand
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

        private LayerTableForm2 tabForm = null;
        
        private void GetnewForm()
        {
            if (tabForm != null)
            {
                if (tabForm.IsDisposed)//如果已经销毁，则重新创建子窗口对象
                {
                    tabForm = new LayerTableForm2();//此为你双击打开的FORM
                    tabForm.Env = this.m_hookHelper.ActiveView.Extent;
                    tabForm.Show();
                    tabForm.Focus();
                }
                else
                {
                    tabForm.Env = this.m_hookHelper.ActiveView.Extent;
                    tabForm.Show();
                    tabForm.Focus();
                }
            }
            else
            {
                tabForm = new LayerTableForm2();
                tabForm.Env = this.m_hookHelper.ActiveView.Extent;
                tabForm.Show();
                tabForm.Focus();
            }
        }

        public LayerTableCommand2()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "显示当前范围属性表(只读)";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "显示当前范围属性表";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
            // TODO: Add LayerTableCommand2.OnClick implementation
            IMapControl3 mapCtl = m_hookHelper.Hook as IMapControl3;
            object customProperty = mapCtl.CustomProperty;
            if (customProperty == null)
                return;
            ILayer thisLyr = customProperty as ILayer;
            IEnvelope env=this.m_hookHelper.ActiveView.Extent;
            if (thisLyr is IFeatureLayer)
            {
                IFeatureLayer senLyr = thisLyr as IFeatureLayer;
                GetnewForm();
                tabForm.FeatureLayer = senLyr;
                tabForm.mapCtrol3 = mapCtl;
                tabForm.ShowGridData();
            }

        }

        #endregion
    }
}
