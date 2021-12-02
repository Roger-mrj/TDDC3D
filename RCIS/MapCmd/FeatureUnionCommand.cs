using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Collections;
using System.Collections.Generic;

using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("89a4b9ab-e585-4b4c-be06-b46a2ed47e1f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("LSGIS.MapTool.FeatureUnionCommand")]
    public sealed class FeatureUnionCommand : BaseCommand
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


        public FeatureUnionCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text 
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


        private double getSumVal(List<IFeature> lstFeas, string field)
        {
            double mj = 0;
            foreach (IFeature aFea in lstFeas)
            {
                double tmp = RCIS.GISCommon.FeatureHelper.GetFeatureDoubleValue(aFea, field);
                mj += tmp;
            }
            return mj;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add FeatureUnionCommand.OnClick implementation
            if (m_hookHelper.ActiveView == null)
            {
                return;
            }
            IMap aMap = m_hookHelper.ActiveView.FocusMap;

            IFeatureLayer currLayer = RCIS.Global.GlobalEditObject.CurrEditTargetLayer;
            IFeatureClass targetFC = currLayer.FeatureClass ;
            string fcName = (targetFC as IDataset).Name.ToUpper();

            ArrayList arSelFea = RCIS.GISCommon.LayerHelper.GetSelectedFeature(aMap,currLayer as IGeoFeatureLayer, targetFC.ShapeType);

            if (arSelFea.Count < 2)
            {
                MessageBox.Show("当前编辑图层必须选中两个以上要素!");
                return;
            }

            

            FeatureUionOptionForm optionFrm = new FeatureUionOptionForm();
            optionFrm.mapControl = this.m_hookHelper.Hook as IMapControl3;
            optionFrm.inFeatures = arSelFea;
            if (optionFrm.ShowDialog() == DialogResult.Cancel)
                return;
            IFeature parentTB = optionFrm.outFeature;

            IWorkspace pWs = RCIS.Global.GlobalEditObject.CurrentEngineEditor.EditWorkspace;
            IWorkspaceEdit pWsEdit = pWs as IWorkspaceEdit;
            pWsEdit.StartEditOperation();
            try
            {
                //记录面积字段 及其合计值
                List<string> lstMjField = new List<string>();
                for (int i = 0; i < currLayer.FeatureClass.Fields.FieldCount; i++)
                {
                    IField pFld = currLayer.FeatureClass.Fields.get_Field(i);
                    if (pFld.Type == esriFieldType.esriFieldTypeDouble || pFld.Type == esriFieldType.esriFieldTypeSingle)
                    {
                        if (pFld.Name.ToUpper().EndsWith("MJ"))
                        {
                            lstMjField.Add(pFld.Name.ToUpper());
                        }
                    }
                }


                Dictionary<string, double> dicSummj = new Dictionary<string, double>();
                foreach (string aFld in lstMjField)
                {
                    double sumMj = 0;
                    foreach (IFeature aFea in arSelFea)
                    {
                        sumMj += RCIS.GISCommon.FeatureHelper.GetFeatureDoubleValue(aFea, aFld);
                    }
                    dicSummj.Add(aFld, sumMj);
                }
                //开始合并

                IFeature firstFea = arSelFea[0] as IFeature;
                IGeometry firstGeo = firstFea.ShapeCopy;
                IGeometry newGeo = null;
                ITopologicalOperator pFirstTop = firstGeo as ITopologicalOperator;
                for (int k = 1; k < arSelFea.Count; k++)
                {
                    IFeature elseGeo = arSelFea[k] as IFeature;
                    newGeo = pFirstTop.Union(elseGeo.ShapeCopy);
                    pFirstTop = newGeo as ITopologicalOperator;
                    
                }
                firstFea.Shape = newGeo;
                RCIS.GISCommon.FeatureHelper.CopyFeature(parentTB, firstFea);

                //赋值面积
                foreach (KeyValuePair<string, double> aItem in dicSummj)
                {

                    RCIS.GISCommon.FeatureHelper.SetFeatureValue(firstFea, aItem.Key, aItem.Value);
                }

                firstFea.Store();

                //其他要素删除
                for (int k = arSelFea.Count - 1; k > 0; k--)
                {
                    (arSelFea[k] as IFeature).Delete();
                }
                pWsEdit.StopEditOperation();
                MessageBox.Show("合并成功!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.m_hookHelper.ActiveView.Refresh();

            }
            catch(Exception ex) {
                pWsEdit.AbortEditOperation();
                MessageBox.Show(ex.Message);

            }
            
            

        }

        #endregion
    }
}
