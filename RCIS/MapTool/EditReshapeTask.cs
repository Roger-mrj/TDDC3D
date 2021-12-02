using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
namespace RCIS.MapTool
{
    [Guid("0a706b17-dc16-4b43-b893-8fbcc8f34f8a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.EditReshapeTask")]
    public class EditReshapeTask : ESRI.ArcGIS.Controls.IEngineEditTask
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
            // TODO: Add EditReshapeTask.Activate implementation
            if (editor == null) return;
            m_engineEditor = editor;
            m_editSketch = editor as IEngineEditSketch;
            m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
            m_editLayer = m_editSketch as IEngineEditLayers;

            ((IEngineEditEvents_Event)m_editSketch).OnTargetLayerChanged += new IEngineEditEvents_OnTargetLayerChangedEventHandler(OnTargetLayerChanged);
            ((IEngineEditEvents_Event)m_editSketch).OnCurrentTaskChanged += new IEngineEditEvents_OnCurrentTaskChangedEventHandler(OnCurrentTaskChanged);
        }

        public void Deactivate()
        {
            // TODO: Add EditReshapeTask.Deactivate implementation
            ((IEngineEditEvents_Event)m_engineEditor).OnTargetLayerChanged -= OnTargetLayerChanged;
            ((IEngineEditEvents_Event)m_engineEditor).OnCurrentTaskChanged -= OnCurrentTaskChanged;

            //Release object references.
            m_engineEditor = null;
            m_editSketch = null;
            m_editLayer = null;

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
        public string GroupName
        {
            get
            {
                // TODO: Add EditReshapeTask.GroupName getter implementation
                return "Modify Tasks";
            }
        }

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

        public string Name
        {
            get
            {
                // TODO: Add EditReshapeTask.Name getter implementation
                return default(string);
            }
        }

        public void OnDeleteSketch()
        {
            // TODO: Add EditReshapeTask.OnDeleteSketch implementation
        }

        public int  ExistPoint(IPoint srcPt, IPointCollection ptcols)
        {
            int  bRet = -1;
            for (int i = 0; i < ptcols.PointCount; i++)
            {
                IPoint aPt = ptcols.get_Point(i);
                if (aPt.Equals(srcPt))
                {
                    bRet = i;
                    break;
                }
                if (RCIS.GISCommon.GeometryHelper.PointEquals(srcPt, aPt))
                {
                    bRet = i;
                    break;
                }
            }
            return bRet;
        }


        private List<IPoint> getInterSecPts(IPolyline line1, IPolyline line2)
        {
            List<IPoint> allInterPts = new List<IPoint>(); //所有相交点
            #region 获取 绘制线与 新面 所有交点           

            ITopologicalOperator boundTop = line1 as ITopologicalOperator;
            IGeometry interSecGeo = boundTop.Intersect(line2 as IGeometry, esriGeometryDimension.esriGeometry0Dimension);
            if (!interSecGeo.IsEmpty)
            {
                IPointCollection pts = interSecGeo as IPointCollection;
                for (int k = 0; k < pts.PointCount; k++)
                {
                    allInterPts.Add(pts.get_Point(k));
                }
            }
            interSecGeo = boundTop.Intersect(this.m_editSketch.Geometry, esriGeometryDimension.esriGeometry1Dimension);
            if (!interSecGeo.IsEmpty)
            {
                IPointCollection pts = interSecGeo as IPointCollection;
                for (int k = 0; k < pts.PointCount; k++)
                {
                    allInterPts.Add(pts.get_Point(k));
                }
            }
            #endregion
            return allInterPts;
        }


        private IGeometry insertVetex(IGeometry polygon, IPoint clickedPt)
        {
           
            #region local variables used in the HitTest

            IHitTest hitShape = polygon as IHitTest;
            IPoint hitPoint = new PointClass();
            double hitDistance = 0;
            int hitPartIndex = 0;
            int hitSegmentIndex = 0;
            bool bRightSide = false;
            esriGeometryHitPartType hitPartType = hitPartType = esriGeometryHitPartType.esriGeometryPartBoundary;
            //the searchRadius is the maximum distance away, in map units, from the shape that will be used
            //for the test - change to an appropriate value.
            double searchRadius = 0.2;
            #endregion

            hitShape.HitTest(clickedPt, searchRadius, hitPartType, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);

            //在搜索半径内 是否捕捉到点
            if (hitPoint.IsEmpty == false)
            {
               

                //Get the PointCollection for a specific path or ring by hitPartIndex to handle multi-part features
                IGeometryCollection geometryCol = polygon as IGeometryCollection;
                IPointCollection pathOrRingPointCollection = geometryCol.get_Geometry(hitPartIndex) as IPointCollection;

                object missing = Type.Missing;
                object hitSegmentIndexObject = new object();
                hitSegmentIndexObject = hitSegmentIndex;
                object partIndexObject = new object();
                partIndexObject = hitPartIndex;
                esriEngineSketchOperationType opType = esriEngineSketchOperationType.esriEngineSketchOperationGeneral;

                pathOrRingPointCollection.AddPoint(clickedPt, ref missing, ref hitSegmentIndexObject);
                
                opType = esriEngineSketchOperationType.esriEngineSketchOperationVertexAdded;
                //remove the old PointCollection from the GeometryCollection and replace with the new one
                geometryCol.RemoveGeometries(hitPartIndex, 1);
                geometryCol.AddGeometry(pathOrRingPointCollection as IGeometry, ref partIndexObject, ref missing);

               

            }
            return polygon;
        }

        public void OnFinishSketch()
        {
            // TODO: Add EditReshapeTask.OnFinishSketch implementation
            if (m_editSketch == null) return;
            IEnumFeature enumFeatures = this.m_engineEditor.EditSelection;
            IFeature aSelFea;
            List<IFeature> lstSelFeas = new List<IFeature>();
            while ((aSelFea = enumFeatures.Next()) != null)
            {
                if (aSelFea.Shape.GeometryType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
                {
                    if (aSelFea.Class == this.m_editLayer.TargetLayer.FeatureClass)
                    {
                        lstSelFeas.Add(aSelFea);
                    }
                }
            }

            if (lstSelFeas.Count == 0)
            {
                MessageBox.Show("请首先在编辑图层选中一个面要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            this.m_engineEditor.StartOperation();
            try
            {

                IGeometry cutGeometry = m_editSketch.Geometry;
                ITopologicalOperator2 topoOperator = cutGeometry as ITopologicalOperator2;
                topoOperator.IsKnownSimple_2 = false;
                topoOperator.Simplify();


                IFeature destFeature = null;

                IFeature origFeature = lstSelFeas[0];
                IFeatureEdit featureEdit = origFeature as IFeatureEdit;
                ISet newFeaturesSet = null;
                try
                {
                    newFeaturesSet = featureEdit.Split(cutGeometry);
                }
                catch { }
                    

                if (newFeaturesSet != null)
                {
                    #region 对于分割的部分，删除所有小的，保留大的
                    IFeature firstFea = newFeaturesSet.Next() as IFeature;
                    IArea firstArea = firstFea.Shape as IArea;
                    double firstMj = firstArea.Area;
                    List<IFeature> newFeas = new List<IFeature>();
                    newFeas.Add(firstFea);
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
                        nextFea = newFeaturesSet.Next() as IFeature;

                    }

                    //最大的那一个保留
                    IFeatureClass pFeaClss = m_editLayer.TargetLayer.FeatureClass;
                    IFeature maxFea = pFeaClss.CreateFeature();
                    maxFea.Shape = newFeas[idxMax].ShapeCopy;
                    RCIS.GISCommon.FeatureHelper.CopyFeature(newFeas[idxMax], maxFea);
                    maxFea.Store();
                    destFeature = maxFea;
                    //其余的都删除
                    for (int j = newFeas.Count - 1; j >= 0; j--)
                    {
                        newFeas[j].Delete();
                    }
                    #endregion


                }
                IPolygon lastPolygon=null;
                if (destFeature == null)
                {
                    destFeature = lstSelFeas[0];
                    //没有分割，则插入点
                    ITopologicalOperator pDestTop1 = destFeature.Shape as ITopologicalOperator;
                    IPolyline pBoundJx1 = pDestTop1.Boundary as IPolyline;
                    List<IPoint> allInterPts1 = this.getInterSecPts(pBoundJx1, this.m_editSketch.Geometry as IPolyline);
                    //依次插入到 图形中
                    lastPolygon = destFeature.Shape as IPolygon;
                    foreach (IPoint aPt in allInterPts1)
                    {
                        lastPolygon = insertVetex(lastPolygon, aPt) as IPolygon;

                    }
                    destFeature.Shape = lastPolygon;
                    destFeature.Store();

                }
                else
                {
                    lastPolygon = destFeature.Shape as IPolygon;
                }

                //填充部分，边界相交，
                ITopologicalOperator pDestTop = lastPolygon as ITopologicalOperator;
                IPolyline pBoundJx = pDestTop.Boundary as IPolyline;
               // int count = (pBoundJx as IPointCollection).PointCount;

                List<IPoint> allInterPts = this.getInterSecPts(pBoundJx, this.m_editSketch.Geometry as IPolyline);
                List<IPoint> copyInterPts = new List<IPoint>();
                foreach (IPoint apt in allInterPts)
                {
                    IPoint newpt = new PointClass();
                    newpt.X = apt.X;
                    newpt.Y = apt.Y;
                    copyInterPts.Add(newpt);
                }

                List<IPolyline> newSketchLine = RCIS.GISCommon.GeometryHelper.SplitALineAtPoints(this.m_editSketch.Geometry as IPolyline, copyInterPts);//切割后新线
                //包含在 要素图形内部的去掉
                for (int jjj=newSketchLine.Count-1;jjj>=0;jjj--)
                {
                    if ((lastPolygon as IRelationalOperator).Contains(newSketchLine[jjj]))
                    {
                        newSketchLine.RemoveAt(jjj);
                    }
                    
                }

                List<int> interPtIdx = new List<int>();//获取所有交点 在面中点索引
                foreach (IPoint apt in allInterPts)
                {
                    interPtIdx.Add(this.ExistPoint(apt, lastPolygon as IPointCollection));
                }

                IPointCollection destFeatPts = lastPolygon as IPointCollection;
                List<IGeometry> newGeos = new List<IGeometry>();
                //逐个查找 不连续的点
                for (int i = 0; i < interPtIdx.Count - 1; i++)
                {
                    int start = interPtIdx[i];
                    int end = interPtIdx[i + 1];
                    if (Math.Abs(end - start) > 1)
                    {
                        List<IPoint> newPolygonPoints = new List<IPoint>();
                        if (start > end)
                        {
                            for (int idx = end; idx <= start; idx++)
                            {
                                newPolygonPoints.Add(destFeatPts.get_Point(idx));
                            }
                        }
                        else
                        {
                            for (int idx = start; idx <= end; idx++)
                            {
                                newPolygonPoints.Add(destFeatPts.get_Point(idx));
                            }
                        }

                        //再在外面那条线上按顺序找点
                        List<IPoint> elsePts = new List<IPoint>();
                        foreach (IPolyline aline in newSketchLine)
                        {
                            List<int> elsePtIdx = new List<int>();
                            int elseStart=this.ExistPoint( destFeatPts.get_Point(start),aline as IPointCollection);
                            int elseEnd=this.ExistPoint(destFeatPts.get_Point(end), aline as IPointCollection);
                            if (elseStart == -1 || elseEnd == -1)
                                continue;
                            //点 排序比较麻烦

                            if (elseStart - elseEnd > 1)
                            {
                                //加入中间这些点, 从 end 加到 start（）
                                if (end > start)
                                {
                                    for (int idxx = elseEnd + 1; idxx < elseStart; idxx++)
                                    {
                                        newPolygonPoints.Add((aline as IPointCollection).get_Point(idxx));
                                    }
                                }
                                else
                                {
                                    for (int idxx = elseStart - 1; idxx > elseEnd; idxx--)
                                    {
                                        newPolygonPoints.Add((aline as IPointCollection).get_Point(idxx));
                                    }
                                }
                            }
                            else if (elseEnd - elseStart > 1)
                            {
                                //加入中间这些点
                                if (end > start)
                                {
                                    for (int idxx = elseEnd - 1; idxx > elseStart; idxx--)
                                    {
                                        newPolygonPoints.Add((aline as IPointCollection).get_Point(idxx));
                                    }
                                }
                                else
                                {
                                    for (int idxx = elseStart + 1; idxx < elseEnd; idxx++)
                                    {
                                        newPolygonPoints.Add((aline as IPointCollection).get_Point(idxx));
                                    }
                                }
                            }
                        }
                        

                        if (newPolygonPoints.Count > 1)
                        {
                            IPolygon aPolygon = RCIS.GISCommon.GeometryHelper.pts2polygon(newPolygonPoints);
                            newGeos.Add(aPolygon);
                        }
                    }
                }
                newGeos.Add(lastPolygon);
               
               

                IPolygon newPoly = RCIS.GISCommon.GeometryHelper.UnionPolygon(newGeos);
                destFeature.Shape = newPoly;
                destFeature.Store();

                m_engineEditor.StopOperation("reshape With Selection");
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                 m_engineEditor.Map.SelectFeature(this.m_editLayer.TargetLayer as ILayer, destFeature);
                IActiveView activeView = m_engineEditor.Map as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography|esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);


            }
            catch (Exception ex)
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                m_engineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                
            }
            

        }

        public string UniqueName
        {
            get
            {
                // TODO: Add EditReshapeTask.UniqueName getter implementation
                return "EngineEditTasksMyReshape";
            }
        }
        #endregion

    }
}
