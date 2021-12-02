using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Collections;
using System.Collections.Generic;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Text;
using RCIS.GISCommon;
namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("f6d57311-561c-4d43-a979-677463d9cdcd")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.ZdszWhCommand")]
    public sealed class ZdszWhCommand : BaseCommand
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
        public ZdszWhCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "属性维护"; //localizable text
            base.m_caption = "宗地四至维护";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "宗地四至维护";  //localizable text
            base.m_name = "宗地四至维护";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
        /// 获取邻宗地
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="aFea"></param>
        /// <returns></returns>
        public List<IFeature> GetNabList(IWorkspace ws, IFeature aFea)
        {
            List<IFeature> pNabList = new List<IFeature>();
            IFeatureWorkspace pFeatureWorkspace = ws as IFeatureWorkspace;
            IFeatureClass pfc = pFeatureWorkspace.OpenFeatureClass("ZD");
            ISpatialFilter sf = new SpatialFilterClass();
            IGeometry neighGeo = aFea.ShapeCopy;
            ITopologicalOperator pTopo = neighGeo as ITopologicalOperator;
            neighGeo = pTopo.Buffer(2); //做缓冲

            sf.Geometry = neighGeo;
            sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = pfc.Search(sf, false);
            int featurecount = pfc.FeatureCount(sf);
            if (featurecount == 0) return null;
            IFeature pFea = pCursor.NextFeature();
            try
            {

                while (pFea != null)
                {
                    pNabList.Add(pFea);
                    pFea = pCursor.NextFeature();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            return pNabList;
        }
        private string FormatInfo(List<string> pList)
        {
            if (pList == null) return "空地";
            if (pList.Count == 0) return "空地";
            string s = "";
            foreach (string str in pList)
            {
                s = s + str;
            }
            s = s.Remove(s.Length - 1, 1);
            if (s == "") return "空地";
            return s;
        }

        private string getQlr(IWorkspace ws, string zdbsm)
        {
            //根据宗地标识码获取权利人
            string qlr = "";
            ITable pTab = null;
            try
            {
                pTab = (ws as IFeatureWorkspace).OpenTable("ZD_QLR");
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "ZDDM='" + zdbsm + "'";
                IRow aRow = null;
                ICursor pCursor = pTab.Search(pQF, false);
                while ((aRow = pCursor.NextRow()) != null)
                {
                    string aqlr = FeatureHelper.GetRowValue(aRow, "QLRMC").ToString();
                    qlr += aqlr + " ";
                }
               
            }
            catch (Exception ex)
            {
            }
            return qlr;
        }

        /// <summary>
        /// 获取宗地四至，
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="aFea"></param>
        /// <param name="sEast"></param>
        /// <param name="sWest"></param>
        /// <param name="sNorth"></param>
        /// <param name="sSouth"></param>
        public  void GetZDSZName(IWorkspace ws, IFeature aFea, ref string sEast,
            ref string sWest, ref string sNorth, ref string sSouth)
        {

            List<IFeature> pNabList = GetNabList(ws, aFea);
            if (pNabList == null) return;
            if (pNabList.Count == 0) return;
            try
            {
                IPoint pCentrePt = (aFea.ShapeCopy as IArea).Centroid;
                ILine pLine = new LineClass();
                IPoint pNabPt = null;
                double pi = Math.PI;
                double angle = 0.0;
                StringBuilder builder = new StringBuilder();
                List<string> pEastList = new List<string>();
                List<string> pWestList = new List<string>();
                List<string> pSouthList = new List<string>();
                List<string> pNorthList = new List<string>();

                string housename = "";
                foreach (IFeature pFea in pNabList)
                {
                    pNabPt = (pFea.ShapeCopy as IArea).Centroid;
                    pLine.PutCoords(pCentrePt, pNabPt);
                    angle = pLine.Angle * 180 / pi;

                    string zdbsm=FeatureHelper.GetFeatureStringValue(pFea,"ZDDM");
                    housename = this.getQlr(ws, zdbsm);
                    if (angle != 0)
                    {
                        if ((angle >= -45) && (angle <= 45))
                        {
                            pEastList.Add(housename + ",");
                        }
                        else if ((angle >= 45) && (angle <= 135))
                        {
                            pNorthList.Add(housename + ",");
                        }
                        else if (((angle >= 135) && (angle <= 180)) || ((angle >= -180) && (angle <= -135)))
                        {
                            pWestList.Add(housename + ",");
                        }
                        else if ((angle <= -45) && (angle >= -135))
                        {
                            pSouthList.Add(housename + ",");
                        }
                    }
                }

                sEast = FormatInfo(pEastList);
                sWest = FormatInfo(pWestList);
                sNorth = FormatInfo(pNorthList);
                sSouth = FormatInfo(pSouthList);
            }
            catch { }
        }


        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ZdszWhCommand.OnClick implementation
            IMap pmap = this.m_hookHelper.ActiveView.FocusMap;
            IGeoFeatureLayer zdLyr = LayerHelper.QueryLayerByModelName(pmap, "ZD");
            ArrayList arZds = LayerHelper.GetSelectedFeature(pmap, zdLyr, esriGeometryType.esriGeometryPolygon);
            if (arZds.Count == 0)
            {
                MessageBox.Show("请首先选中一块或者多块宗地!",
                    "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IWorkspace globalWs = RCIS.Global.GlobalEditObject.CurrentEngineEditor.EditWorkspace;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();

            try
            {
                int allCount = arZds.Count;
                // int icount = 1;
                foreach (IFeature aZd in arZds)
                {
                    string sEast = "";
                    string sWest = "";
                    string sNorth = "";
                    string sSouth = "";
                    GetZDSZName(globalWs, aZd, ref sEast, ref sWest, ref sNorth, ref sSouth);
                    string str = "东：" + sEast + " 南：" + sSouth + " 西：" + sWest + " 北：" + sNorth;
                    FeatureHelper.SetFeatureValue(aZd, "ZDSZ", str);
                    //2015-10-22修改，东西南北分开
                  
                    aZd.Store();
                    //spProgress.Caption = "正在生成第" + (icount.ToString()) + "块宗地四至...";
                    //spProgress.EditValue = Utilities.Precent(0, allCount, icount);
                    //icount++;
                    Application.DoEvents();
                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("tqzdsz");
                MessageBox.Show("生成完毕!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.ToString());
            }
           
        }

        #endregion
    }
}
