using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using RCIS.Global;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.Utility;
namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("b374c376-835a-4d19-9ca6-3559a5abbfa8")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.DLTBUnionCommand")]
    public sealed class DLTBUnionCommand : BaseCommand
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
        public DLTBUnionCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "地类图斑合并";  //localizable text 
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
            // TODO: Add DLTBUnionCommand.OnClick implementation
            if (m_hookHelper.ActiveView == null)
                return;
            IMap aMap = m_hookHelper.ActiveView.FocusMap;
            IEnumFeature aEnumObj = aMap.FeatureSelection as IEnumFeature;
            IFeature currFea = aEnumObj.Next();
            List<IFeature> lstOldFeas = new List<IFeature>();
            while (currFea != null)
            {
                lstOldFeas.Add(currFea);
                currFea = aEnumObj.Next();
            }
            if (lstOldFeas.Count < 2)
            {
                MessageBox.Show("当前地图必须选中两个以上要素!");
                return;
            }
            


            IFeatureWorkspace pFeatureWorkspace = GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
            
            

            TBHBOptionForm optionFrm = new TBHBOptionForm();
            optionFrm.mapControl = this.m_hookHelper.Hook as IMapControl3;
            optionFrm.inFeatures = lstOldFeas;
            if (optionFrm.ShowDialog() == DialogResult.Cancel)
                return;


            if (RCIS.Global.AppParameters.GX_HISTORY)
            {
                try
                {
                    IFeatureDataset pDS = pFeatureWorkspace.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                    TDDC3D.edit.GxHistoryHelper.CreateHGXTable(pDS);
                    GxHistoryHelper.InsertHistory(pFeatureWorkspace as IWorkspace, lstOldFeas);
                }
                catch { }

            }


            IFeature parentTB = optionFrm.outTB;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                double bghTbmj = 0; double bghTbdlmj = 0;
                //重新计算面积,其他属性字段不变，由arcgis自动继承
                foreach (IFeature tmpFea in lstOldFeas)
                {
                    double mj1 = FeatureHelper.GetFeatureDoubleValue(tmpFea, "TBMJ");
                    bghTbmj += mj1;
                    double mj2 = FeatureHelper.GetFeatureDoubleValue(tmpFea, "TBDLMJ");
                    bghTbdlmj += mj2;
                }

                IFeature firstFea = lstOldFeas[0];
                IGeometry firstGeo = firstFea.ShapeCopy;

                ISpatialReference pSR = firstGeo.SpatialReference;

                IGeometry newGeo = null;
                ITopologicalOperator pTopo = firstGeo as ITopologicalOperator;
                for (int k = 1; k < lstOldFeas.Count; k++)
                {
                    IFeature elseGeo = lstOldFeas[k];
                    newGeo = pTopo.Union(elseGeo.ShapeCopy);
                    pTopo = newGeo as ITopologicalOperator;
                }
                pTopo.Simplify();
                newGeo.SnapToSpatialReference();
                firstFea.Shape = newGeo;

                FeatureHelper.CopyFeature(parentTB, firstFea);

                #region  //赋值属性
                FeatureHelper.SetFeatureValue(firstFea, "TBMJ", MathHelper.Round(bghTbmj, 2));
                FeatureHelper.SetFeatureValue(firstFea, "TBDLMJ", MathHelper.Round(bghTbdlmj, 2));
                FeatureHelper.SetFeatureValue(firstFea, "BSM", -1);
                firstFea.Store();
                #endregion


                //原先的宗地删掉
                foreach (IFeature afea in lstOldFeas)
                {
                    if (afea.Equals(firstFea))
                        continue;
                    afea.Delete();
                }

                //合并后进行属性修改
                //ZdPropertiesFrm zdFrm = new ZdPropertiesFrm();
                //zdFrm.Text = "合并后宗地属性修改";
                //zdFrm.CurrFeature = firstFea;
                //zdFrm.ShowDialog();

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("dltbhb");
                MessageBox.Show("合并成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.m_hookHelper.ActiveView.Refresh();

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
