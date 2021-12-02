using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("e831b0bd-11b8-4b77-8e95-7c92ca8f291f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.InAShpGeoCommand")]
    public sealed class InAShpGeoCommand : BaseCommand
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
        public InAShpGeoCommand(IEngineEditor _value )
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "����shp��Χ��";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "����shp��Χ��";  //localizable text
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
            // TODO: Add InAShpGeoCommand.OnClick implementation

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SHP�ļ�|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            //�򿪸�shp�����ռ䣬�򿪸�shpҪ����
            string className = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
            IWorkspace pShpWs = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(dlg.FileName);
            if (pShpWs==null)
            {
                MessageBox.Show("�򿪸�shp�����ռ���Ч��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                IFeatureWorkspace pFeaWs = pShpWs as IFeatureWorkspace;
                IFeatureClass pClass = pFeaWs.OpenFeatureClass(className);
                IFeatureCursor pFeaCur = pClass.Search(null, false);
                
                try
                {
                    IFeature aFea = pFeaCur.NextFeature();
                    if (aFea != null)
                    {
                        IGeometry pGeo = aFea.ShapeCopy;
                        

                        IEngineEditSketch m_editSketch = this.engineEditor as IEngineEditSketch;
                        if (pGeo.GeometryType != m_editSketch.GeometryType)
                        {
                            MessageBox.Show("����ͼ�μ���������Ŀ�꼸�����Ͳ�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                        m_editSketch.Geometry = pGeo;
                        m_editSketch.FinishSketch();

                        IMapControl2 mapctl = this.m_hookHelper.Hook as IMapControl2;
                        mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography,
                            null, mapctl.Extent);
                    }
                }
                catch { }
                finally
                {
                    Marshal.ReleaseComObject(pFeaCur);
                    Marshal.ReleaseComObject(pFeaWs);
                    Marshal.ReleaseComObject(pShpWs);
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        #endregion
    }
}
