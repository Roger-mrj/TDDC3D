using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using RCIS.Utility;
using RCIS.Global;
using ESRI.ArcGIS.esriSystem;

namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("4f5549de-5680-49d8-a765-a41b50065e3d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.CutPolygonBySelectLineCommand")]
    public sealed class CutPolygonBySelectLineCommand : BaseCommand
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

        public CutPolygonBySelectLineCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "选中线分割面";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "选中线分割面";  //localizable text
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
            // TODO: Add CutPolygonBySelectLineCommand.OnClick implementation
            //按选中线进行分割
            IFeatureLayer currLyr = GlobalEditObject.CurrEditTargetLayer;
            IEnumFeature enumFeatures = GlobalEditObject.CurrentEngineEditor.EditSelection;
            IFeature aSelFea;
            List<IFeature> lstSelLine = new List<IFeature>();
            while ((aSelFea = enumFeatures.Next()) != null)
            {
                if (aSelFea.Shape.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
                {
                    lstSelLine.Add(aSelFea);
                }
            }

            if (lstSelLine.Count == 0)
            {
                MessageBox.Show("请首先选中线段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                IFeatureClass pFC = GlobalEditObject.CurrEditTargetLayer.FeatureClass;
                foreach (IFeature sLineFea in lstSelLine)
                {
                    //找到与之相交的要素
                    List<IFeature> polygonFeas = GetFeaturesHelper.getFeaturesByGeo(pFC, sLineFea.Shape, esriSpatialRelEnum.esriSpatialRelIntersects);
                    foreach (IFeature aPolyonFel in polygonFeas)
                    {
                        try
                        {
                            IFeatureEdit featureEdit = aPolyonFel as IFeatureEdit;
                            ISet newFeaturesSet = featureEdit.Split(sLineFea.Shape);
                            //New features have been created.
                            if (newFeaturesSet != null)
                            {
                                newFeaturesSet.Reset();
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                }
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                GlobalEditObject.CurrentEngineEditor.StopOperation("cutpolygonbyline");
                MessageBox.Show("分割完毕!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                GlobalEditObject.CurrentEngineEditor.AbortOperation();
            }
            
        }

        #endregion
    }
}
