using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Windows.Forms;
namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("42d31927-3fb1-4537-8e4a-74231b4cb775")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.CutIntersectCommand")]
    public sealed class CutIntersectCommand : BaseCommand
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
        public CutIntersectCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "裁切相交部分";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
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
            // TODO: Add CutIntersectCommand.OnClick implementation
            IMap aMap = m_hookHelper.ActiveView.FocusMap;
            IEnumFeature aEnumObj = aMap.FeatureSelection as IEnumFeature;
            IFeature currFea = aEnumObj.Next();
            List<IFeature> lstFeas = new List<IFeature>();
            while (currFea != null)
            {
                lstFeas.Add(currFea);
                currFea = aEnumObj.Next();
            }
            if (lstFeas.Count < 2)
            {
                MessageBox.Show("当前地图必须选中两个要素!");
                return;
            }
            
            
            //保留一个
            IFeature feature1 = lstFeas[0] as IFeature;
            IFeature feature2 = lstFeas[1] as IFeature;
            if (feature1.Shape.GeometryType != feature2.Shape.GeometryType)
            {
                MessageBox.Show("两个要素图形必须具有相同的集合类型!");
            }
            
            ITopologicalOperator pTop = feature1.ShapeCopy as ITopologicalOperator;
            IGeometry interGeo =null;
            if (feature1.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                interGeo = pTop.Intersect(feature2.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            }
            else if (feature1.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                interGeo = pTop.Intersect(feature2.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
            }
            if (interGeo==null || interGeo.IsEmpty)
            {
                MessageBox.Show("相交部分为空，或者不需要裁切！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CutPolygonIntersectOptForm optFrm = new CutPolygonIntersectOptForm();
            optFrm.mapControl = this.m_hookHelper.Hook as IMapControl3;

            optFrm.inFeatures = lstFeas;
            if (optFrm.ShowDialog() == DialogResult.Cancel)
                return;
            int cutOpt = optFrm.cutOption;
            
            Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                ITopologicalOperator pTop1 = feature1.ShapeCopy as ITopologicalOperator;
                ITopologicalOperator pTop2 = feature2.ShapeCopy as ITopologicalOperator;

                if (cutOpt == 0)
                {
                    //直接切除
                    
                    feature1.Shape = pTop1.Difference(interGeo);
                    feature1.Store();
                    feature2.Shape = pTop2.Difference(interGeo);
                    feature2.Store();
                }
                else if (cutOpt == 1)
                {
                    IFeature outFeature = optFrm.outFeature;
                    if (feature1 == outFeature)
                    {
                        //保留1，切除2
                        feature2.Shape = pTop2.Difference(interGeo);
                        feature2.Store();
                    }
                    else
                    {
                        feature1.Shape = pTop1.Difference(interGeo);
                        feature1.Store();
                    }

                }
                Global.GlobalEditObject.CurrentEngineEditor.StopOperation("cutintersect");
                (aMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
                MessageBox.Show("裁切成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
            

        }

        #endregion
    }
}
