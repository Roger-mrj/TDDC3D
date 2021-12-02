using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;


namespace MYEngineEditTasks
{
    [Guid("ccb009a5-ae75-4ed9-a2c3-12688f380188")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MYEngineEditTasks.CutPolygonWithoutSelect")]
    public class CutPolygonWithoutSelect : ESRI.ArcGIS.Controls.IEngineEditTask
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


            //MessageBox.Show("请在地图上绘制分割线."
            //    + "\n当前编辑对象所在的图层上,凡是被线条完全穿越的图形都会被分割."
            //    , "分割", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // TODO: Add CutPolygonWithoutSelect.Activate implementation
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
            // TODO: Add CutPolygonWithoutSelect.Deactivate implementation
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
                // TODO: Add CutPolygonWithoutSelect.GroupName getter implementation
                return "Modify Tasks";
            }
        }

        public string Name
        {
            get
            {
                return "Cut Polygons Without Selection (guojie)";
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
            // TODO: Add CutPolygonWithoutSelect.OnDeleteSketch implementation
        }

        public void OnFinishSketch()
        {
            if (m_editSketch == null)
                return;

           
            //Change the cursor to be hourglass shape.
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            IFeatureClass featureClass = m_editLayer.TargetLayer.FeatureClass;

            IGeometry cutGeometry = m_editSketch.Geometry;
            if (cutGeometry.IsEmpty)
            {
                return;
            }
            ITopologicalOperator2 topoOperator = cutGeometry as ITopologicalOperator2;
            topoOperator.IsKnownSimple_2 = false;
            topoOperator.Simplify();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = m_editSketch.Geometry;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature originFeature = null;
            ArrayList comErrors = new ArrayList();
            m_engineEditor.StartOperation();

            try
            {
                while ((originFeature = featureCursor.NextFeature()) != null)
                {
                    IFeatureEdit featureEdit = originFeature as IFeatureEdit;
                    if (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {

                        ITopologicalOperator pTop = originFeature.ShapeCopy as ITopologicalOperator;
                        cutGeometry = pTop.Intersect(cutGeometry as IPolyline, esriGeometryDimension.esriGeometry0Dimension);
                        IPointCollection ptCos = cutGeometry as IPointCollection;
                        if (ptCos.PointCount > 0)
                        {
                            cutGeometry = ptCos.get_Point(0);
                        }

                    }
                    try
                    {
                        ISet newFeaturesSet = featureEdit.Split(cutGeometry);

                        if (newFeaturesSet != null)
                        {
                            newFeaturesSet.Reset();
                        }

                        comErrors.Add(String.Format("OID为{0}的要素分割成功！", originFeature.OID.ToString()));
                    }
                    catch (Exception ex)
                    {

                    }
                }
                m_engineEditor.StopOperation("Cut Polygons Without Selection");
                StringBuilder stringBuilder = new StringBuilder("以下要素的分割结果: \n", 200);
                foreach (string comError in comErrors)
                {
                    stringBuilder.AppendLine(comError);
                }

                MessageBox.Show(stringBuilder.ToString(), "分割过程");

                m_engineEditor.Map.ClearSelection();
                IActiveView activeView = m_engineEditor.Map as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
            }
            catch (Exception ex)
            {
                m_engineEditor.AbortOperation();
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            }



            //try
            //{
            //    //Get the geometry that performs the cut from the edit sketch.
            //    IGeometry cutGeometry = m_editSketch.Geometry;
            //    ITopologicalOperator2 topoOperator = cutGeometry as ITopologicalOperator2;
            //    topoOperator.IsKnownSimple_2 = false;
            //    topoOperator.Simplify();

            //    ISpatialFilter spatialFilter = new SpatialFilterClass();
            //    spatialFilter.Geometry = m_editSketch.Geometry;
            //    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                
            //    IFeatureClass featureClass = m_editLayer.TargetLayer.FeatureClass;
            //    IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);

            //    IFeature origFeature = featureCursor.NextFeature();
            //    if (origFeature != null)
            //    {
            //        ArrayList comErrors = new ArrayList();
            //        m_engineEditor.StartOperation();

            //        while (origFeature != null)
            //        {
            //            try
            //            {
            //                #region 
            //                IFeatureEdit featureEdit = origFeature as IFeatureEdit;
            //                if (featureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            //                {

            //                }
            //                else if (featureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
            //                {

            //                    ITopologicalOperator pTop = origFeature.ShapeCopy as ITopologicalOperator;
            //                    cutGeometry= pTop.Intersect(cutGeometry as IPolyline, esriGeometryDimension.esriGeometry0Dimension);
            //                    IPointCollection ptCos = cutGeometry as IPointCollection;
            //                    if (ptCos.PointCount > 0)
            //                    {
            //                        cutGeometry = ptCos.get_Point(0);
            //                    }

            //                }
            //                ISet newFeaturesSet = featureEdit.Split(cutGeometry);

            //                if (newFeaturesSet != null)
            //                {
            //                    newFeaturesSet.Reset();
            //                    hasCutPolygons = true;
            //                }

            //                comErrors.Add(String.Format("OID为{0}的要素分割成功！", origFeature.OID.ToString()));
            //                #endregion 


            //            }
            //            catch (COMException comExc)
            //            {
            //                //comErrors.Add(String.Format("OID: {0}, 错误: {1} , {2}", origFeature.OID.ToString(), comExc.ErrorCode, comExc.Message));
            //            }
            //            finally
            //            {
            //                origFeature = featureCursor.NextFeature();
            //            }
            //        }
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            //        if (hasCutPolygons)
            //        {
            //            //Clear the map's selection.
            //            m_engineEditor.Map.ClearSelection();
            //            IActiveView activeView = m_engineEditor.Map as IActiveView;
            //            activeView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, activeView.Extent);

            //            m_engineEditor.StopOperation("Cut Polygons Without Selection");

                        
            //        }
            //        else
            //        {
            //            m_engineEditor.AbortOperation();
            //        }

            //        //report any errors that have arisen while splitting features
            //        if (comErrors.Count > 0)
            //        {
            //            StringBuilder stringBuilder = new StringBuilder("以下要素的分割结果: \n", 200);
            //            foreach (string comError in comErrors)
            //            {
            //                stringBuilder.AppendLine(comError);
            //            }

            //            MessageBox.Show(stringBuilder.ToString(), "分割过程");
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("未能成功完成分割任务.\n" + e.Message);
            //    m_engineEditor.AbortOperation();
            //}
            //finally
            //{
            //    //Change the cursor shape to default.
            //    System.Windows.Forms.Cursor.Current = Cursors.Default;

            //    //不管成功失败，都切换为默认任务
            //  //  ResetTask();               
       
            //}
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
                    if (name.ToUpper().Trim().Equals("ControlToolsEditing_CreateNewFeatureTask".ToUpper()) )
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
                // TODO: Add CutPolygonWithoutSelect.UniqueName getter implementation
                return "EngineEditTasksCutPolygonWithoutSelect";
            }
        }
        #endregion


        #region IEngineEditTask private methods
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

        #endregion



       

    }
}
