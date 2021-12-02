using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;

using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;


namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("063f0e45-4dd3-4f6b-ae20-b724cefe1f23")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("LSGIS.MapTool.FeaturesDeleteTool")]
    public sealed class FeaturesDeleteCommand : BaseCommand
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
        private IEngineEditor m_engineEditor = null;

        public FeaturesDeleteCommand( IEngineEditor _value  )
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")


            this.m_engineEditor = _value;

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
            // TODO: Add FeaturesDeleteTool.OnClick implementation

            if (m_hookHelper.ActiveView == null)
            {
                return;
            }
            IMap aMap = m_hookHelper.ActiveView.FocusMap;
            IEnumFeature aEnumObj = aMap.FeatureSelection as IEnumFeature;
            IFeature currFea = aEnumObj.Next();
            if (currFea == null)
            {
                MessageBox.Show("当前地图没有被选中要素!");
                return;
            }
                        
            int icount = 0;
            while (currFea != null)
            {
                icount++;
                currFea = aEnumObj.Next();
            }
            if (MessageBox.Show("您当前选中" + icount.ToString() + "个要素，确认要全部删除么?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
                return;

            m_engineEditor.StartOperation();
            aEnumObj.Reset();
            currFea = aEnumObj.Next();
            while (currFea != null)
            {
                try
                {
                    currFea.Delete();

                }
                catch{ }
                currFea = aEnumObj.Next();
            }
            m_engineEditor.StopOperation("deletefeatures");
            this.m_hookHelper.ActiveView.Refresh();
        }

        #endregion
    }
}
