using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;




namespace RCIS.MapTool
{
    /// <summary>
    /// Summary description for LayerAddImgTool.
    /// </summary>
    [Guid("182f7deb-d55f-4bbb-b203-1cd91f3fcd90")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.LayerAddImgTool")]
    public sealed class LayerAddImgTool : BaseTool
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper;


        private AxPageLayoutControl m_pageLayoutCtl = null;

        public LayerAddImgTool(AxPageLayoutControl control)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            this.m_pageLayoutCtl = control;
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

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add LayerAddImgTool.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add LayerAddImgTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LayerAddImgTool.OnMouseDown implementation
            if (Button == 1)
            {
                IActiveView pA = this.m_hookHelper.ActiveView;
                IRubberBand pRub = new RubberEnvelopeClass();
                IEnvelope pE = pRub.TrackNew(pA.ScreenDisplay, null) as IEnvelope;
                if ((pE == null) || (pE.IsEmpty))
                {
                    return;
                }
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Í¼Æ¬ÎÄ¼þ(*.jpg)|*.JPG";
                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;
                string fileName = dlg.FileName;
                try
                {
                    IPictureElement picele = new JpgPictureElementClass();
                    picele.SavePictureInDocument = true;
                    picele.ImportPictureFromFile(fileName);
                    IGeometry geo = pE as IGeometry;
                    IElement northEle = picele as IElement;
                    northEle.Geometry = geo;

                    this.m_hookHelper.ActiveView.GraphicsContainer.AddElement(northEle, 0);
                    object o = Type.Missing;
                    this.m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, o, pE);

                    this.m_pageLayoutCtl.CurrentTool = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LayerAddImgTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LayerAddImgTool.OnMouseUp implementation
        }
        #endregion
    }
}
