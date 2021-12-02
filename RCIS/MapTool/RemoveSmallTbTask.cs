using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using RCIS.GISCommon;
using ESRI.ArcGIS.ADF;
namespace RCIS.MapTool
{
    [Guid("4f7ae46b-6fe9-48f9-b40f-55cf8fb9446d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.RemoveSmallTbTask")]
    public class RemoveSmallTbTask : ESRI.ArcGIS.Controls.IEngineEditTask
    {
        #region Private Members
        IEngineEditor m_engineEditor;
        IEngineEditSketch m_editSketch;
        IEngineEditLayers m_editLayer;




        #endregion

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
            // TODO: Add RemoveSmallTbTask.Activate implementation
            if (editor == null) return;
            m_engineEditor = editor;
            m_editSketch = editor as IEngineEditSketch;
            m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
            m_editLayer = m_editSketch as IEngineEditLayers;

            ((IEngineEditEvents_Event)m_editSketch).OnTargetLayerChanged += new IEngineEditEvents_OnTargetLayerChangedEventHandler(OnTargetLayerChanged);
            ((IEngineEditEvents_Event)m_editSketch).OnCurrentTaskChanged += new IEngineEditEvents_OnCurrentTaskChangedEventHandler(OnCurrentTaskChanged);
        }
        private void UpdateSketchToolStatus()
        {
            if (m_editLayer == null)
                return;

            //Only enable the sketch tool if there is a polygon target layer.
            if (m_editLayer.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon && m_editLayer.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolyline)
                m_editSketch.GeometryType = esriGeometryType.esriGeometryNull;
            else
                //Set the edit sketch geometry type to be esriGeometryPolyline.
                m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
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
        public void OnCurrentTaskChanged()
        {
            UpdateSketchToolStatus();
        }

        public void OnTargetLayerChanged()
        {
            UpdateSketchToolStatus();
        }
        public void Deactivate()
        {
            // TODO: Add RemoveSmallTbTask.Deactivate implementation
            // TODO: Add EditReshapeTask.Deactivate implementation
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
                // TODO: Add RemoveSmallTbTask.GroupName getter implementation
                return "Modify Tasks";
            }
        }

        public string Name
        {
            get
            {
                // TODO: Add RemoveSmallTbTask.Name getter implementation
                return default(string);
            }
        }

        public void OnDeleteSketch()
        {
            // TODO: Add RemoveSmallTbTask.OnDeleteSketch implementation
        }

        private IFeature getMaxFeature(IFeature inTb,List<int> oids, IFeatureClass dltbClass)
        {

            IFeature currFea = null;            
            //double maxMj = 0;
            double maxLen = 0;
        
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = inTb.ShapeCopy as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);

                IFeature aFea = null;
                while ((aFea = cursor.NextFeature()) != null)
                {
                    if (!oids.Contains(aFea.OID))
                    {
                        ITopologicalOperator ptop = aFea.Shape as ITopologicalOperator;
                        //交于线条
                        IGeometry interLine = ptop.Intersect(inTb.Shape, esriGeometryDimension.esriGeometry1Dimension);
                        if (!interLine.IsEmpty)
                        {
                            double len = (interLine as IPolyline).Length;
                            if (len > maxLen)
                            {
                                maxLen = len;
                                currFea = aFea;
                            }
                        }

                    }

                }
            }
            return currFea;
        }

        public void OnFinishSketch()
        {
            // TODO: Add RemoveSmallTbTask.OnFinishSketch implementation
            if (m_editSketch == null) return;
            List<IFeature> interFeatures = new List<IFeature>();
            IGeometry cutGeometry = m_editSketch.Geometry;
            ITopologicalOperator2 topoOperator = cutGeometry as ITopologicalOperator2;
            topoOperator.IsKnownSimple_2 = false;
            topoOperator.Simplify();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = m_editSketch.Geometry;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //找到所有相交的要素
            IFeatureClass featureClass = m_editLayer.TargetLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature origFeature = featureCursor.NextFeature();
            while (origFeature != null)
            {
                interFeatures.Add(origFeature);
                origFeature = featureCursor.NextFeature();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);

            if (interFeatures.Count == 0)
            {
                MessageBox.Show("当前选中编辑图层与绘制线并无交集！");
                return;
            }

            List<int> exceptFeaId = new List<int>(); //被排除的那部分id

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            this.m_engineEditor.StartOperation();
            try
            {
                //对所有要素进行分割
                //每分割一个，就将小面 合并到 隔壁较大图斑中去
                foreach (IFeature aInterFeature in interFeatures)
                {
                    IFeatureEdit featureEdit = aInterFeature as IFeatureEdit;
                    ISet newFeaturesSet = null;
                    try
                    {
                        newFeaturesSet = featureEdit.Split(cutGeometry);
                    }
                    catch (Exception ex)
                    {
                    }
                    if (newFeaturesSet != null)
                    {
                        newFeaturesSet.Reset();
                        #region 对于分割的部分，删除所有小的，保留大的
                        IFeature firstFea = newFeaturesSet.Next() as IFeature;
                        IArea firstArea = firstFea.Shape as IArea;
                        double firstMj = firstArea.Area;
                        List<IFeature> newFeas = new List<IFeature>();
                        newFeas.Add(firstFea);

                        exceptFeaId.Add(firstFea.OID);//这些 id的图斑在搜索周围大图斑的时候 ，都将略过

                        int i = 0;
                        int idxMax = i; //记录 最大面积大的索引
                        //找出最大面积
                        IFeature nextFea = newFeaturesSet.Next() as IFeature;
                        while (nextFea != null)
                        {
                            i++;
                            if ((nextFea.Shape as IArea).Area > firstMj)
                            {
                                firstMj = (nextFea.Shape as IArea).Area;
                                idxMax = i;
                            }
                            
                            newFeas.Add(nextFea);
                            exceptFeaId.Add(nextFea.OID);

                            nextFea = newFeaturesSet.Next() as IFeature;

                        }


                        //最大的那一个保留
                        IFeatureClass pFeaClss = m_editLayer.TargetLayer.FeatureClass;
                        IFeature maxFea = pFeaClss.CreateFeature();
                        maxFea.Shape = newFeas[idxMax].ShapeCopy;
                        RCIS.GISCommon.FeatureHelper.CopyFeature(newFeas[idxMax], maxFea);
                        maxFea.Store();

                        
                        //其余的都并入到周边较大图形
                        for (int j = newFeas.Count - 1; j >= 0; j--)
                        {
                            if (j != idxMax)
                            {
                                //获取临街图形
                                IFeature touchedFea = this.getMaxFeature(newFeas[j],exceptFeaId, pFeaClss);
                                if (touchedFea != null)
                                {
                                    ITopologicalOperator pTouchedTop = touchedFea.Shape as ITopologicalOperator;
                                    IGeometry newUnionGeo = pTouchedTop.Union(newFeas[j].Shape);
                                    touchedFea.Shape = newUnionGeo;
                                    touchedFea.Store();


                                    
                                }
                            }
                            newFeas[j].Delete();
                            
                        }


                        #endregion


                    }

                }
               
                m_engineEditor.StopOperation("uniont topology");
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                
                IActiveView activeView = m_engineEditor.Map as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);


            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                m_engineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
                
        }

        public string UniqueName
        {
            get
            {
                // TODO: Add RemoveSmallTbTask.UniqueName getter implementation
                return "RemoveSmallTbEditTask";
            }
        }
        #endregion

    }
}
