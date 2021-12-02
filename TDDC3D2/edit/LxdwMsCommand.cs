using System;
using System.Drawing;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using RCIS.Global;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("a067250f-a994-49d1-b16a-887a6a30ac60")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.LxdwMsCommand")]
    public sealed class LxdwMsCommand : BaseCommand
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
        public LxdwMsCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "零星地物灭失";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "零星地物灭失";  //localizable text
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

        public IFeature getFeaturesByGeo(IFeatureClass targetClass, IGeometry pGeo, esriSpatialRelEnum rel)
        {
            IFeature retFea = null;
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSR = new SpatialFilterClass();
                pSR.Geometry = pGeo;
                pSR.SpatialRel = rel;
                IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                retFea = aFea;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

            }
            return retFea;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add LxdwMsCommand.OnClick implementation
            IMap aMap = m_hookHelper.ActiveView.FocusMap;
            
            IFeatureClass DltbClass = null;
            try
            {
                DltbClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch { }
            if (DltbClass == null)
                return;
            
            IEnumFeature aEnumObj = aMap.FeatureSelection as IEnumFeature;
            IFeature currFea = aEnumObj.Next();
            List<IFeature> lstFeas = new List<IFeature>();
            while (currFea != null)
            {
                ITable table = currFea.Table;
                IDataset ds = table as IDataset;
                if (ds.Name.ToUpper() != "LXDW")
                {
                    currFea = aEnumObj.Next();
                    continue;
                }

                lstFeas.Add(currFea);
                currFea = aEnumObj.Next();
            }
            if (lstFeas.Count <1)
            {
                MessageBox.Show("当前地图必须选中一个以上零星地物!");
                return;
            }
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (IFeature aLxdw in lstFeas)
                {
                    //找到该零星地物所在的图斑，把面积 加到 所在 地类图斑中
                    IFeature aDltb = getFeaturesByGeo(DltbClass, aLxdw.Shape, esriSpatialRelEnum.esriSpatialRelWithin);
                    if (aDltb == null)
                        continue;
                    double lxdwmj = FeatureHelper.GetFeatureDoubleValue(aLxdw, "MJ");
                    double oldTbdlmj = FeatureHelper.GetFeatureDoubleValue(aDltb, "TBDLMJ");
                    double oldLxdwmj = FeatureHelper.GetFeatureDoubleValue(aDltb, "LXDWMJ");

                    oldLxdwmj -= lxdwmj;
                    oldTbdlmj += lxdwmj;

                    FeatureHelper.SetFeatureValue(aDltb, "LXDWMJ", RCIS.Utility.MathHelper.Round(oldLxdwmj, 2));
                    FeatureHelper.SetFeatureValue(aDltb, "TBDLMJ", RCIS.Utility.MathHelper.Round(oldTbdlmj, 2));
                    aDltb.Store();

                    aLxdw.Delete();
                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("lxdwms");
                MessageBox.Show("操作完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
           
            


        }

        #endregion
    }
}
