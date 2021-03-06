using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;

using RCIS.Controls;

namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("0b9b0064-138d-4bfa-a443-d77ce9e49e7e")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.ExportLayerCommand")]
    public sealed class ExportLayerCommand : BaseCommand
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
        public ExportLayerCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "????????????...";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "????????????...";  //localizable text
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
        /// ??????????Shapefile????
        /// </summary>
        /// <returns></returns>
        private string GetDefaultFullName()
        {
            string tmpFileName =Application.StartupPath+"\\output\\Export_Output.shp";

            if (!(File.Exists(tmpFileName)))
            {
                return tmpFileName;
            }
            string folder = tmpFileName.Substring(0, tmpFileName.LastIndexOf("."));
            //string fileName = tmpFileName.Substring(tmpFileName.LastIndexOf("\\") + 1, tmpFileName.LastIndexOf(".") - 1);
            int count = 1;
            tmpFileName = folder + count.ToString() + ".shp";
            while (File.Exists(tmpFileName))
            {
                count += 1;
                tmpFileName = folder + count.ToString() + ".shp";
            }
            return tmpFileName;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ExportLayerCommand.OnClick implementation

            IMapControl3 mapCtl = (IMapControl3)m_hookHelper.Hook;

            bool bHasSelectedFeatures = false;
            object customProperty = mapCtl.CustomProperty;
            if (customProperty == null)
                return;
            ILayer pSelSymLayer = customProperty as ILayer;
            if (!(pSelSymLayer is IFeatureLayer))
            {
                return;
            }
            IFeatureClass pSelFeaClass = (pSelSymLayer as IFeatureLayer).FeatureClass;
            
            IFeatureSelection pselection = pSelSymLayer as IFeatureSelection;
            if (pselection == null)
            {
                MessageBox.Show("??????????????????????????????????????????????");
                return;
            }
            ISelectionSet pSelectionSet = (pSelSymLayer as IFeatureSelection).SelectionSet;

            if ((pSelectionSet.Count > 0))
            {
                //????????????????????
                bHasSelectedFeatures = true;
            }
            string layerName = pSelSymLayer.Name;

            FormDataExport frmDataExport = new FormDataExport();
            frmDataExport.SetExportFeatures(bHasSelectedFeatures);
            frmDataExport.ShapefilePath = this.GetDefaultFullName();
            if (frmDataExport.ShowDialog() == DialogResult.OK)
            {
                //sExportFeature=frmDataExport.ExportFeature;
                //MessageBox.Show("????????????\n" + frmDataExport.ShapefilePath);
                //this.pSelSymLayer//??????????????


                FormExportProgress frmEP = new FormExportProgress();
                frmEP.SelectedLayer = pSelSymLayer;
                frmEP.ActiveViewExtent = mapCtl.ActiveView.Extent;
                frmEP.FeatureClass = frmDataExport.ExportFeature;
                frmEP.ExportShapefilePath = frmDataExport.ShapefilePath;
                //frmEP.SourceWorkspace = this.GlobalSDEWorkspace;
                frmEP.SourceWorkspace = (pSelFeaClass as IDataset).Workspace;
                
                if (frmEP.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("??????????");
                }
                else
                {
                    MessageBox.Show("??????????");
                }
            }
        }

        #endregion
    }
}
