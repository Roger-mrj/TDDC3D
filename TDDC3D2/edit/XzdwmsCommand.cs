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
using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("61307bd9-3afa-45a4-89ac-78702edbb001")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.XzdwmsCommand")]
    public sealed class XzdwmsCommand : BaseCommand
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
        public XzdwmsCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "线状地物灭失";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "线状地物灭失";  //localizable text
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

        
        public List<IFeature> getFeaturesByGeo(IFeatureClass targetClass, IGeometry pGeo)
        {
            List<IFeature> lst=new List<IFeature>();
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSR = new SpatialFilterClass();
                pSR.Geometry = pGeo;
                pSR.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                while (aFea != null)
                {
                    //如果不是交于一条线，则不要
                    IGeometry feaGeo = aFea.ShapeCopy;
                    ITopologicalOperator pTop = feaGeo as ITopologicalOperator;
                    IGeometry intersecGeo=pTop.Intersect(pGeo,esriGeometryDimension.esriGeometry1Dimension);
                    if (intersecGeo.IsEmpty || intersecGeo == null)
                    {
                        aFea = pCursor.NextFeature();
                        continue;
                    }

                    lst.Add(aFea);
                    aFea = pCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

            }
            return lst;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add XzdwmsCommand.OnClick implementation
            IMap aMap = m_hookHelper.ActiveView.FocusMap;
            IFeatureClass DltbClass = null;
            try
            {
                DltbClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch { }
            if (DltbClass == null)
            {
                MessageBox.Show("找不到地类图班层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IGeoFeatureLayer xzdwLayer = LayerHelper.QueryLayerByModelName(aMap, "XZDW");
            System.Collections.ArrayList lstFeas = LayerHelper.GetSelectedFeature(aMap, xzdwLayer, esriGeometryType.esriGeometryPolyline);
            
            if (lstFeas.Count < 1)
            {
                MessageBox.Show("当前地图必须选中一个以上线状地物!");
                return;
            }
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                //逐个线装地物操作
                foreach (IFeature aXzdw in lstFeas)
                {
                    
                    //找到 相交一条线的两个图斑，
                    List<IFeature> lstTbs = getFeaturesByGeo(DltbClass, aXzdw.ShapeCopy);
                    if (lstTbs.Count == 0)
                        continue;
                    double xzdwmj = FeatureHelper.GetFeatureDoubleValue(aXzdw, "XZDWMJ");
                    int xzdwmj100 = (int)(xzdwmj * 100);
                    double kcbl = FeatureHelper.GetFeatureDoubleValue(aXzdw, "KCBL");
                    if (kcbl == 0.5)
                    {
                        //如果扣除比例是0.5 ，
                        
                        
                        if (!MathHelper.IsOdd(xzdwmj100))
                        {
                            //如果现状地物面积*100 是偶数，则 两个图斑各加一半面积，
                            foreach (IFeature aDltb in lstTbs)
                            {
                                double kcxzdwmj = FeatureHelper.GetFeatureDoubleValue(aDltb, "XZDWMJ");
                                double tbdlmj = FeatureHelper.GetFeatureDoubleValue(aDltb, "TBDLMJ");

                                kcxzdwmj-=(xzdwmj100 /2/100);
                                tbdlmj += (xzdwmj100 / 2 / 100);
                                FeatureHelper.SetFeatureValue(aDltb, "XZDWMJ", kcxzdwmj);
                                FeatureHelper.SetFeatureValue(aDltb, "TBDLMJ", tbdlmj);
                                aDltb.Store();
                            }
                        }
                        else
                        {
                            //如果是奇数，按照扣除图斑编号，分出左右边
                            string leftTbbh = FeatureHelper.GetFeatureStringValue(aXzdw, "KCTBBH1");
                            string rightTbbh = FeatureHelper.GetFeatureStringValue(aXzdw, "KCTBBH2");
                            IFeature leftDltb=null, rightDltb=null;
                            foreach (IFeature aDltb in lstTbs)
                            {
                                if (leftTbbh == FeatureHelper.GetFeatureStringValue(aDltb, "TBBH"))
                                {
                                    leftDltb = aDltb;
                                }
                                else if (rightTbbh == FeatureHelper.GetFeatureStringValue(aDltb, "TBBH"))
                                {
                                    rightDltb = aDltb;
                                }
                            }
                            //左边面积+ 1/2+ 0.01，右边面积 +1/2
                            if ((leftDltb != null) && (rightDltb != null))
                            {
                                double leftkcxzdwmj = FeatureHelper.GetFeatureDoubleValue(leftDltb, "XZDWMJ");
                                double lefttbdlmj = FeatureHelper.GetFeatureDoubleValue(leftDltb, "TBDLMJ");
                                int  itmp = xzdwmj100 / 2;
                                double kc1 = ((double)itmp) / 100;
                                leftkcxzdwmj -= kc1;
                                lefttbdlmj += kc1;
                                FeatureHelper.SetFeatureValue(leftDltb, "XZDWMJ",  MathHelper.Round(leftkcxzdwmj,2));
                                FeatureHelper.SetFeatureValue(leftDltb, "TBDLMJ",  MathHelper.Round(lefttbdlmj,2));
                                leftDltb.Store();


                                double rightkcxzdwmj = FeatureHelper.GetFeatureDoubleValue(rightDltb, "XZDWMJ");
                                double righttbdlmj = FeatureHelper.GetFeatureDoubleValue(rightDltb, "TBDLMJ");
                                rightkcxzdwmj -= (kc1 + 0.01);
                                righttbdlmj += (kc1 +0.01);
                                FeatureHelper.SetFeatureValue(rightDltb, "XZDWMJ", MathHelper.Round( rightkcxzdwmj,2));
                                FeatureHelper.SetFeatureValue(rightDltb, "TBDLMJ", MathHelper.Round(righttbdlmj,2));
                                rightDltb.Store();
                            }
                        }                       
                                                
                    }
                    else
                    {
                        //如果 扣除比例是1，则找到所在图斑，直接面积加到屠版中即可
                        IFeature aDltb = lstTbs[0];
                        double kcxzdwmj = FeatureHelper.GetFeatureDoubleValue(aDltb, "XZDWMJ");
                        double tbdlmj = FeatureHelper.GetFeatureDoubleValue(aDltb, "TBDLMJ");
                        kcxzdwmj -= xzdwmj;
                        tbdlmj += xzdwmj;
                        FeatureHelper.SetFeatureValue(aDltb, "XZDWMJ", kcxzdwmj);
                        FeatureHelper.SetFeatureValue(aDltb, "TBDLMJ", tbdlmj);
                        aDltb.Store();

                    }

                    aXzdw.Delete();                    
                }

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("lxdwms");
                IActiveView activeView = aMap as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);
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
