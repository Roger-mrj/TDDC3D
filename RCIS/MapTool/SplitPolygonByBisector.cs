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


using RCIS.GISCommon;

namespace MYEngineEditTasks
{
    [Guid("ce1a39f3-37f8-4c1d-bd65-84f51da138c0")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MYEngineEditTasks.SplitPolygonByBisector")]
    ///构建等分线批量分割 多边形
    public class SplitPolygonByBisector : ESRI.ArcGIS.Controls.IEngineEditTask
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


        #region 成员变量
        IEngineEditor m_engineEditor;
        IEngineEditSketch m_editSketch;
        IEngineEditLayers m_editLayer;

        private string defaultName = "ControlsEditSplitPolygonByBisector";
        #endregion

        #region "IEngineEditTask Implementations"  初始化

        public void Activate(ESRI.ArcGIS.Controls.IEngineEditor editor, ESRI.ArcGIS.Controls.IEngineEditTask oldTask)
        {
            // TODO: Add SplitPolygonByBisector.Activate implementation
            if (editor == null)
                return;

            //Initialize class member variables.
            m_engineEditor = editor;
            m_editSketch = editor as IEngineEditSketch;
            //该功能为画 多点
            m_editSketch.GeometryType = esriGeometryType.esriGeometryMultipoint;
            m_editLayer = m_editSketch as IEngineEditLayers;

            //Wire editor events.        
            ((IEngineEditEvents_Event)m_editSketch).OnTargetLayerChanged += new IEngineEditEvents_OnTargetLayerChangedEventHandler(OnTargetLayerChanged);
            ((IEngineEditEvents_Event)m_editSketch).OnCurrentTaskChanged += new IEngineEditEvents_OnCurrentTaskChangedEventHandler(OnCurrentTaskChanged);
        

        }

        public void Deactivate()
        {
            // TODO: Add SplitPolygonByBisector.Deactivate implementation
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
                // TODO: Add SplitPolygonByBisector.GroupName getter implementation
                return defaultName;
            }
        }

        public string Name
        {
            get
            {
                // TODO: Add SplitPolygonByBisector.Name getter implementation
                return defaultName;
            }
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
                m_editSketch.GeometryType = esriGeometryType.esriGeometryMultipoint;
        }
        #endregion

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
            // TODO: Add SplitPolygonByBisector.OnDeleteSketch implementation
        }


        private IFeature GetDkFea(IGeometry cutGeometry)
        {
            ITopologicalOperator2 topoOperator = cutGeometry as ITopologicalOperator2;
            topoOperator.IsKnownSimple_2 = false;
            topoOperator.Simplify();
            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = m_editSketch.Geometry;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;


            //Find the polygon features that cross the sketch.
            IFeatureClass featureClass = m_editLayer.TargetLayer.FeatureClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            return aFea;
        }

        private IPolyline  GetTouchLine(IGeometry cutGeometry, IFeature dkFea)
        {
            object missing=Type.Missing;
            ISegmentCollection aLine = new PolylineClass();
            ISegment aSegment=null;

            IGeometry dkGeo = dkFea.ShapeCopy as IGeometry;
            ISegmentCollection pSegCol = dkGeo as ISegmentCollection;
            for (int i = 0; i < pSegCol.SegmentCount; i++)
            {
                ISegment pSeg= pSegCol.get_Segment(i);
                try
                {
                    ISegmentCollection aNewLine = new PolylineClass();
                    aNewLine.AddSegment(pSeg, ref missing, ref missing);

                    IPolyline aPolyline = aNewLine as IPolyline;
                    ITopologicalOperator ptop = aPolyline as ITopologicalOperator;
                    if (!(ptop.Intersect(cutGeometry, esriGeometryDimension.esriGeometry0Dimension).IsEmpty))
                    {
                        aSegment = pSeg;
                        break;
                    }
                }
                catch(Exception ex) { }
            }
            if (aSegment != null || !aSegment.IsEmpty)
            {
                try
                {
                    aLine.AddSegment(aSegment, ref missing, ref missing);
                }
                catch (Exception ex)
                {
                }
            }
            return aLine as IPolyline ;
        }


        private void DrawElementTest(IGeometry geo)
        {
            ILineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Width = 2;
            pLineSym.Color = ColorHelper.CreateColor(System.Drawing.Color.White);
            ILineElement aele = new LineElementClass();
            aele.Symbol = pLineSym;
            
            
            IElement pElement = aele as IElement;


            pElement.Geometry = geo;

            IActiveView activeView = m_engineEditor.Map as IActiveView;
            IGraphicsContainer pGC = activeView.GraphicsContainer;
            pGC.AddElement(pElement, 0);
        }


        public void OnFinishSketch()
        {
            // TODO: Add SplitPolygonByBisector.OnFinishSketch implementation
            //获取所有点
            if (m_editSketch == null)
                return;
            object missing=Type.Missing;

            IGeometry cutGeometry = m_editSketch.Geometry;
            IMultipoint pMultiPts = cutGeometry as IMultipoint;
            IPointCollection pPtCs = pMultiPts as IPointCollection;

            IFeature aDkFea = GetDkFea(cutGeometry);
            if (aDkFea == null)
                return;
            try
            {
                IPolyline splitLine = GetTouchLine(cutGeometry, aDkFea) ;
                if (splitLine == null || splitLine.IsEmpty)
                    return;





                ISegmentCollection pPaths = null;
                IGeometryCollection pPolyline = new PolylineClass();
                //先找到 与这些点相交的线，获取其起始点，终点,然后串联起来
                //

                List<IPoint> lstPts = new List<IPoint>();

                for (int i = 0; i < pPtCs.PointCount; i++)
                {
                    IPoint aPt = pPtCs.get_Point(i);
                    //根据 点 生成等分直线，然后进行分割之
                    IConstructLine pConstructLine = new LineClass();
                    pConstructLine.ConstructAngleBisector(splitLine.FromPoint, aPt, splitLine.ToPoint, 100, false  );
                   
                    pPaths = new PathClass();
                    pPaths.AddSegment(pConstructLine as ISegment, ref missing, ref missing);
                    pPolyline.AddGeometry(pPaths as IGeometry, ref missing, ref missing);
                    //同时记录其最后一个点
                    IPoint endPt = (pConstructLine as ILine).ToPoint;
                    if (endPt != aPt)
                    {
                        lstPts.Add(endPt);
                    }
                    else
                    {
                        lstPts.Add((pConstructLine as ILine).FromPoint);
                    }


                }
                //最后这些点 连接起来
                if (lstPts.Count >=2)
                {
                    for (int j = 1; j < lstPts.Count; j++)
                    {
                        ILine newLine = new LineClass();
                        newLine.PutCoords(lstPts[j - 1], lstPts[j]);

                        pPaths = new PathClass();
                        pPaths.AddSegment(newLine as ISegment, ref missing, ref missing);
                        pPolyline.AddGeometry(pPaths as IGeometry , ref missing, ref missing);
                        

                    }
                }               

                //DrawElementTest(pPolyline as IGeometry);

                try
                {

                    m_engineEditor.StartOperation();
                    IFeatureEdit featureEdit = aDkFea as IFeatureEdit;
                    ISet newFeaturesSet = featureEdit.Split(pPolyline as IPolyline);
                    m_engineEditor.StopOperation("cut");

                    IActiveView activeView = this.m_engineEditor.Map as IActiveView;
                    activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics|esriViewDrawPhase.esriViewGeography, null, activeView.Extent);

                }
                catch (Exception ex)
                {
                }

               

                

            }
            catch (Exception ex)
            {
                m_engineEditor.AbortOperation();
            }
            
            
           


           
            

        }

        public string UniqueName
        {
            get
            {
                // TODO: Add SplitPolygonByBisector.UniqueName getter implementation
                return defaultName;
            }
        }
        #endregion

    }
}
