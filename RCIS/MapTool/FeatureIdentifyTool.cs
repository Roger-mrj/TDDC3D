using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace RCIS.MapTool
{
    /// <summary>
    /// Summary description for FeatureIdentifyTool.
    /// </summary>
    [Guid("555c3310-62f4-46be-8098-7ae0cf097c4d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.FeatureIdentifyTool")]
    public sealed class FeatureIdentifyTool : BaseTool
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


        private IPoint m_lastPoint = null;
        private AxMapControl m_mapCtrl = null;
        private ObjectPropertyForm mPropertyForm;


        public FeatureIdentifyTool( AxMapControl mapctrl  )
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "查看信息";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "查看信息";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                //string bitmapResourceName = GetType().Name + ".bmp";
                //bitmapResourceName = Application.StartupPath + @"\mapcur\" + bitmapResourceName;
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                //base.m_bitmap = new Bitmap( bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
                //base.m_cursor = new System.Windows.Forms.Cursor(Application.StartupPath+@"\mapcur\"+  GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }

            this.m_mapCtrl = mapctrl;

        }

        #region Overriden Class Methods

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
            // TODO: Add FeatureIdentifyTool.OnClick implementation
        }
        void OnPropertyFormClosed(object sender, FormClosedEventArgs e)
        {
            this.mPropertyForm = null;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            base.OnMouseDown(Button, Shift, X, Y);

            // TODO:  Add FeatureIdentifyTool.OnMouseDown implementation
            IScreenDisplay disp = m_hookHelper.ActiveView.ScreenDisplay;
            this.m_lastPoint = disp.DisplayTransformation.ToMapPoint(X, Y);

            if (Button == 1)
            {
                if (this.mPropertyForm == null)
                {
                    this.mPropertyForm = new ObjectPropertyForm();
                    this.mPropertyForm.FormClosed += new FormClosedEventHandler(OnPropertyFormClosed);
                }
                this.mPropertyForm.ActiveView = m_hookHelper.ActiveView;

                if (this.m_mapCtrl.Parent is Form)
                {
                    this.mPropertyForm.Owner = this.m_mapCtrl.Parent as Form;
                }
                this.mPropertyForm.MapControl = this.m_mapCtrl;

                RubberEnvelopeClass rubber = new RubberEnvelopeClass();
                IEnvelope aEnv = rubber.TrackNew(disp, null) as IEnvelope;
                try
                {
                    if (aEnv == null || aEnv.IsEmpty
                        || aEnv.Width <= 0
                        || aEnv.Height <= 0)
                    {
                        this.mPropertyForm.Identify(m_lastPoint);
                    }
                    else this.mPropertyForm.Identify(aEnv);
                }
                catch  { }

                this.mPropertyForm.Show();
            }
           
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add FeatureIdentifyTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add FeatureIdentifyTool.OnMouseUp implementation
        }
        #endregion
    }
}
