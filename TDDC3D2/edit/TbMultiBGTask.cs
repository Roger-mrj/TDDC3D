using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;


using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using System.Collections;

using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.edit
{
    [Guid("fe55e7f5-438d-4d56-8ac8-731d8b84312d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.TbMultiBGTask")]
    public class TbMultiBGTask : ESRI.ArcGIS.Controls.IEngineEditTask
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
            EngineEditTasks.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            EngineEditTasks.Unregister(regKey);

        }

        #endregion
        #endregion

        IEngineEditor m_engineEditor;
        IEngineEditSketch m_editSketch;
        IEngineEditLayers m_editLayer;

        #region "IEngineEditTask Implementations"
        public void Activate(ESRI.ArcGIS.Controls.IEngineEditor editor, ESRI.ArcGIS.Controls.IEngineEditTask oldTask)
        {
            if (editor == null)
                return;

            //Initialize class member variables.
            m_engineEditor = editor;
            m_editSketch = editor as IEngineEditSketch;
            m_editSketch.GeometryType = esriGeometryType.esriGeometryPolygon;

            m_editLayer = m_editSketch as IEngineEditLayers;

        }

        public void Deactivate()
        {
            // TODO: Add TbMultiBGTask.Deactivate implementation
            

            //Release object references.
            m_engineEditor = null;
            m_editSketch = null;
            m_editLayer = null;
        }
      
        public string GroupName
        {
            get
            {
                // TODO: Add TbMultiBGTask.GroupName getter implementation
                return  "Modify Tasks";
            }
        }

        public string Name
        {
            get
            {
                // TODO: Add TbMultiBGTask.Name getter implementation
                return "Multi TB Change ";
            }
        }

        public void OnDeleteSketch()
        {
            // TODO: Add TbMultiBGTask.OnDeleteSketch implementation
        }

        public void OnFinishSketch()
        {
            // TODO: Add TbMultiBGTask.OnFinishSketch implementation
            #region 初始化
            IGeometry myGeometry = m_editSketch.Geometry;
            if (myGeometry.IsEmpty)
            {
                MessageBox.Show("当前图形为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IPolygon aPolygon = myGeometry as IPolygon;
            if (aPolygon.IsEmpty)
            {
                MessageBox.Show("当前图形必须为面图形！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ITopologicalOperator2 pCutGeoTop = myGeometry as ITopologicalOperator2;
            pCutGeoTop.IsKnownSimple_2 = false;
            pCutGeoTop.Simplify();

            myGeometry.SnapToSpatialReference();

            IFeatureWorkspace pFeaWs = this.m_engineEditor.EditWorkspace as IFeatureWorkspace;
            IFeatureClass dltbTClass = pFeaWs.OpenFeatureClass("DLTB");            
           
            
            #endregion
            //地类图斑分割，按照新的图形的边界线，对要素进行分割
            
            this.m_engineEditor.StartOperation();

            try
            {
                Dictionary<string, string> dicFeaValue = new Dictionary<string, string>();                
               
                //获取三个图层与该图层相交的部分
                List<IFeature> lstIntersetDltb = GetDLTBFeatures(myGeometry, dltbTClass); //交于一个面的地类图斑

                //是否记录历史
                if (RCIS.Global.AppParameters.GX_HISTORY)
                {
                    try
                    {
                        IFeatureDataset pDS = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                        TDDC3D.edit.GxHistoryHelper.CreateHGXTable(pDS);
                        GxHistoryHelper.InsertHistory(pFeaWs as IWorkspace, lstIntersetDltb);
                    }
                    catch { }

                }
                
                //新图斑
                List<IFeature> newDltbs = sys.YWCommonHelper.DltbBg(lstIntersetDltb, dltbTClass, myGeometry, dicFeaValue);
                
                this.m_engineEditor.StopOperation("multitbbg");
                
                IActiveView activeView = m_engineEditor.Map as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, null);
                MessageBox.Show("变更完成!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.m_engineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
           
        }

        public string UniqueName
        {
            get
            {
                // TODO: Add TbMultiBGTask.UniqueName getter implementation
                return "TbMultiBGTask";
            }
        }
        #endregion

        /// <summary>
        /// 获取交与一个面的地类图斑
        /// </summary>
        /// <param name="cutGeo"></param>
        /// <param name="aClass"></param>
        /// <returns></returns>
        private List<IFeature> GetDLTBFeatures(IGeometry cutGeo, IFeatureClass aClass)
        {
            List<IFeature> lstZD = new List<IFeature>();

            cutGeo.Project((aClass as IGeoDataset).SpatialReference);
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = cutGeo;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;

            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {
                    IGeometry aGeo = aFea.ShapeCopy;
                    ITopologicalOperator ptop = aGeo as ITopologicalOperator;
                    IGeometry pInterSectGeo = ptop.Intersect(cutGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (pInterSectGeo != null && !pInterSectGeo.IsEmpty)
                    {

                        lstZD.Add(aFea);
                    }
                    aFea = featureCursor.NextFeature();
                }
            }
            catch(Exception ex)
            { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                GC.Collect();
            }
            return lstZD;
        }

       
        


    }
}
