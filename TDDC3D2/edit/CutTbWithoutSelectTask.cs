using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Windows.Forms;


namespace RCIS.MapTool
{
    [Guid("679cae09-4a3c-463c-bb60-22c996734f0a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.CutTbWithoutSelectTask")]
    public class CutTbWithoutSelectTask : ESRI.ArcGIS.Controls.IEngineEditTask
    {

        IEngineEditor m_engineEditor;
        IEngineEditSketch m_editSketch;
        IEngineEditLayers m_editLayer;

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

        #region "IEngineEditTask Implementations"
        public void Activate(ESRI.ArcGIS.Controls.IEngineEditor editor, ESRI.ArcGIS.Controls.IEngineEditTask oldTask)
        {
            // TODO: Add CutTbWithoutSelectTask.Activate implementation
            if (editor == null)
                return;

            //Initialize class member variables.
            m_engineEditor = editor;
            m_editSketch = editor as IEngineEditSketch;
            m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
            m_editLayer = m_editSketch as IEngineEditLayers;

            //Wire editor events.        
            ((IEngineEditEvents_Event)m_editSketch).OnTargetLayerChanged += new IEngineEditEvents_OnTargetLayerChangedEventHandler(OnTargetLayerChanged);
            ((IEngineEditEvents_Event)m_editSketch).OnCurrentTaskChanged += new IEngineEditEvents_OnCurrentTaskChangedEventHandler(OnCurrentTaskChanged);
        }

        public void Deactivate()
        {
            // TODO: Add CutTbWithoutSelectTask.Deactivate implementation
            ((IEngineEditEvents_Event)m_engineEditor).OnTargetLayerChanged -= OnTargetLayerChanged;
            ((IEngineEditEvents_Event)m_engineEditor).OnCurrentTaskChanged -= OnCurrentTaskChanged;

            //Release object references.
            m_engineEditor = null;
            m_editSketch = null;
            m_editLayer = null;
        }

        public string GroupName
        {
            get
            {
                // TODO: Add CutTbWithoutSelectTask.GroupName getter implementation
                return "Modify Tasks";
            }
        }

        public string Name
        {
            get
            {
                // TODO: Add CutTbWithoutSelectTask.Name getter implementation
                return  "Cut TB Without Selection ";
            }
        }
        public void OnCurrentTaskChanged()
        {
            UpdateSketchToolStatus();
        }

        public void OnTargetLayerChanged()
        {
            UpdateSketchToolStatus();
        }

        
        public void OnDeleteSketch()
        {
            // TODO: Add CutTbWithoutSelectTask.OnDeleteSketch implementation
            
           
        }
        //是否 面包含点
        private bool ContainsPoint(IPolygon pPoly, IPoint pPt)
        {
            if (pPoly == null) return false;
            bool result = (pPoly as IRelationalOperator).Contains(pPt);
            return result;
        }
        /// <summary>
        /// 选中被线条穿越的宗地
        /// </summary>
        /// <param name="pLine"></param>
        /// <returns></returns>
        private void SelectSplitFeature(IPolyline pLine, List<IFeature> pSplitedList, List<IFeature> pTouchedList)
        {
            if (pSplitedList == null) return;
            if (pTouchedList == null) return;

            IFeatureLayer m_zdLayer = RCIS.Global.GlobalEditObject.CurrEditTargetLayer;
            this.m_engineEditor.Map.ClearSelection();
            IFeatureClass aClass = m_zdLayer.FeatureClass;

            IActiveView activeView = m_engineEditor.Map as IActiveView;

            ISpatialFilter sp = new SpatialFilterClass();
            sp.Geometry = pLine;
            sp.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor aCursor = aClass.Search(sp, false);
            IRelationalOperator relOp = pLine as IRelationalOperator;
            IFeature aFea = aCursor.NextFeature();
            while (aFea != null)
            {
                bool isSplited = false;
                IPolygon aGeom = aFea.ShapeCopy as IPolygon;
                if (!relOp.Touches(aGeom))
                {
                    if (!this.ContainsPoint(aGeom, pLine.FromPoint)
                        && !this.ContainsPoint(aGeom, pLine.ToPoint))
                    {//只要那些被完全穿越的
                        pSplitedList.Add(aFea);
                        isSplited = true;
                        activeView.FocusMap.SelectFeature(m_zdLayer as ILayer, aFea);
                    }
                }
                if (!isSplited)
                {
                    pTouchedList.Add(aFea);
                }
                aFea = aCursor.NextFeature();



            }
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);

        }
        public void OnFinishSketch()
        {
            // TODO: Add CutTbWithoutSelectTask.OnFinishSketch implementation
            if (m_editSketch == null)
                return;

            // bool hasCutPolygons = false;

            //Change the cursor to be hourglass shape.           

            IFeatureWorkspace pFeaWs = this.m_engineEditor.EditWorkspace as IFeatureWorkspace;
           // IFeatureClass dljxclass = pFeaWs.OpenFeatureClass("DLJX");  //取消
            IFeatureClass dltbClass = pFeaWs.OpenFeatureClass("DLTB");

            IGeometry cutGeometry = m_editSketch.Geometry;
            ITopologicalOperator2 topoOperator = cutGeometry as ITopologicalOperator2;
            topoOperator.IsKnownSimple_2 = false;
            topoOperator.Simplify();

            m_engineEditor.StartOperation();

            IPolyline pLine = RCIS.GISCommon.GeometryHelper.ExtentSplitLine(cutGeometry as IPolyline);

            List<IFeature> aSplitedList = new List<IFeature>();
            List<IFeature> aTouchedList = new List<IFeature>();
            this.SelectSplitFeature(pLine, aSplitedList, aTouchedList);

            if (aSplitedList.Count == 0)
            {
                MessageBox.Show("没有图斑被所绘制的线条穿越,不能开始分割!"
                , "分割", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
           
            try
            {
                //是否记录历史
                if (RCIS.Global.AppParameters.GX_HISTORY)
                {
                    try
                    {
                        IFeatureDataset pDS = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                        TDDC3D.edit.GxHistoryHelper.CreateHGXTable(pDS);
                        TDDC3D.edit.GxHistoryHelper.InsertHistory(this.m_engineEditor.EditWorkspace, aSplitedList);
                    }
                    catch { }
                }

                foreach (IFeature aFea in aSplitedList)
                {
                    #region  地类图斑分割
                    

                    ITopologicalOperator pBoundtop = aFea.ShapeCopy as ITopologicalOperator;
                    IGeometry geoBound = pBoundtop.Boundary;
                    ITopologicalOperator pIntertop = geoBound as ITopologicalOperator;
                    IGeometry pGeoInter = pIntertop.Intersect(pLine as IGeometry, esriGeometryDimension.esriGeometry0Dimension);

                    

                    IFeatureEdit featureEdit = aFea as IFeatureEdit;
                    ISet newFeaturesSet = featureEdit.Split(pLine as IGeometry);
                    newFeaturesSet.Reset();
                    //新图斑编号，和新标志吗，tBmj重新计算，其他暂不变
                    #region 生成新的分割图斑

                    IFeature aBgTb = newFeaturesSet.Next() as IFeature;
                    while (aBgTb != null)
                    {
                        IPoint selectPoint = (aBgTb.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        int currDh = (int)(X / 1000000);////WK---带号
                       // LSSphereArea.GGP.ClsSphereArea area = new LSSphereArea.GGP.ClsSphereArea();
                        SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();

                        double tbmj = area.SphereArea(aBgTb.ShapeCopy, currDh);
                        FeatureHelper.SetFeatureValue(aBgTb, "TBMJ", tbmj);

                        FeatureHelper.SetFeatureValue(aBgTb, "BSM", -1);
                        aBgTb.Store();

                        aBgTb = newFeaturesSet.Next() as IFeature;
                    }
                    #endregion
                    #endregion

                }
                
                
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("TBFG");

                MessageBox.Show("分割完成,请分别修改每个分割后要素属性！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                IActiveView activeView = this.m_engineEditor.Map as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection | esriViewDrawPhase.esriViewGeography,
                    null, activeView.Extent);

            }
            catch (Exception ex)
            {
                MessageBox.Show("未能成功完成分割任务.\n" + ex.Message);
                m_engineEditor.AbortOperation();

            }
            finally
            {
                //不管成功失败，都切换为默认任务
               // ResetTask();
            }
                     



        }
        
        private void ResetTask()
        {
            //ControlToolsEditing_CreateNewFeatureTask
            if (this.m_engineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                for (int i = 0; i < m_engineEditor.TaskCount; i++)
                {
                    IEngineEditTask task = m_engineEditor.get_Task(i);
                    string name = task.UniqueName;
                    if (name.ToUpper().Trim().Equals("ControlToolsEditing_CreateNewFeatureTask".ToUpper()))
                    {
                        m_engineEditor.CurrentTask = task;
                        break;
                    }
                }
            }
        }

        public string UniqueName
        {
            get
            {
                // TODO: Add CutTbWithoutSelectTask.UniqueName getter implementation
                return "CutTbWithoutSelectTask";
            }
        }
        #endregion

        private List<IFeature> GetFeatures(IGeometry cutGeo, IFeatureClass aClass)
        {
            List<IFeature> lstdljx = new List<IFeature>();

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
                    lstdljx.Add(aFea);
                    aFea = featureCursor.NextFeature();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            }
            return lstdljx;
        }

        #region IEngineEditTask private methods
        private void UpdateSketchToolStatus()
        {
            if (m_editLayer == null)
                return;

            //Only enable the sketch tool if there is a polygon target layer.
            if (m_editLayer.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
                m_editSketch.GeometryType = esriGeometryType.esriGeometryNull;
            else
                //Set the edit sketch geometry type to be esriGeometryPolyline.
                m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
        }
        #endregion

    }
}
