using System;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using RCIS.GISCommon;

namespace RCIS.Global
{
    /// <summary>
    /// 草图工具事件监听
    /// 调用方式 
    /// 定义 eventListner   private Global.SketchEventListener eventListner = null;
    ///  eventListner = new SketchEventListener(this.m_engineEditor);
    ///  eventListner.AddEvent();
    /// 移除  eventListner.RemoveEvent(); //移除事件     
    /// </summary>
    class SketchEventListener
    {
        private IEngineEditor m_engineEditor = null;
        private IEngineEditSketch m_engitSketch = null;



        private IActiveView activeView = null;
        //辅助线
        private IElement tmpEle1, tmpEle2 = null;

        //private AxMapControl m_mapCtrol = null;

        public SketchEventListener(IEngineEditor editor)
        {
            this.m_engineEditor = editor;
            this.m_engitSketch = editor as IEngineEditSketch;

            
            
        }

        /// <summary>
        /// 加载监听事件
        /// </summary>
        public void AddEvent()
        {
            ((IEngineEditEvents_Event)m_engitSketch).OnVertexAdded += new IEngineEditEvents_OnVertexAddedEventHandler(OnVertexAdded);
            ((IEngineEditEvents_Event)m_engitSketch).OnSketchFinished += new IEngineEditEvents_OnSketchFinishedEventHandler(OnSketchFinished);

            ((IEngineEditEvents_Event)m_engitSketch).OnStopOperation += new IEngineEditEvents_OnStopOperationEventHandler(OnStopOperation);
            ((IEngineEditEvents_Event)m_engitSketch).OnStopEditing += new IEngineEditEvents_OnStopEditingEventHandler(OnStopEditing);
           
        }

        /// <summary>
        /// 卸载监听事件
        /// </summary>
        public void RemoveEvent()
        {
            ((IEngineEditEvents_Event)m_engitSketch).OnVertexAdded -= OnVertexAdded;
            ((IEngineEditEvents_Event)m_engitSketch).OnSketchFinished -= OnSketchFinished;
            ((IEngineEditEvents_Event)m_engitSketch).OnStopOperation -= OnStopOperation;
            ((IEngineEditEvents_Event)m_engitSketch).OnStopEditing -= OnStopEditing;

        }

        //刷新事件
        void OnAfterDrawSketchEventHandler(IDisplay Display)
        {
            IPoint lastPt = this.m_engitSketch.LastPoint; //最后的点坐标
            
        }


        /// <summary>
        /// 终止编辑，删除元素
        /// </summary>
        /// <param name="saveChanges"></param>
        public void OnStopEditing(bool saveChanges)
        {

            activeView = m_engineEditor.Map as IActiveView;
            IGraphicsContainer pGC = activeView.GraphicsContainer;
            pGC.DeleteElement(tmpEle1);
            pGC.DeleteElement(tmpEle2);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, activeView.Extent);
        }

        public void OnSketchFinished()
        {

            activeView = m_engineEditor.Map as IActiveView;
            IGraphicsContainer pGC = activeView.GraphicsContainer;
            pGC.DeleteElement(tmpEle1);
            pGC.DeleteElement(tmpEle2);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, activeView.Extent);
        }


        private void OnStopOperation()
        {
            activeView = m_engineEditor.Map as IActiveView;
            IGraphicsContainer pGC = activeView.GraphicsContainer;
            pGC.DeleteElement(tmpEle1);
            pGC.DeleteElement(tmpEle2);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, activeView.Extent);
        }

        private ILineSymbol BuildLineSymbol()
        {
            
            ILineSymbol pLineSym = new SimpleLineSymbol();
            pLineSym.Width = 2.2;
            pLineSym.Color = ColorHelper.CreateColor(System.Drawing.Color.Yellow);
            return pLineSym;
        }

        //根据两点构建线
        private IPolyline BuildPolyline(IPoint pt1, IPoint pt2)
        {
            ISegmentCollection pPath = new PolylineClass();
            ILine pLine3 = new LineClass();
            pLine3.PutCoords(pt1, pt2);
            
            object missing1 = Type.Missing;

            pPath.AddSegment(pLine3 as ISegment, ref missing1, ref missing1);
            return pPath as IPolyline;
        }

        public void OnVertexAdded(IPoint point)
        {
           
            if (this.m_engitSketch.GeometryType != esriGeometryType.esriGeometryPolygon)
                return;

            IPointCollection pPtCol = this.m_engitSketch.Geometry as IPointCollection;
            if (pPtCol.PointCount != 3)
                return;

            activeView = m_engineEditor.Map as IActiveView;
            IGraphicsContainer pGC = activeView.GraphicsContainer;

            //根据第一个点和第二个点,构建辅助线
            IPoint firstPt = pPtCol.get_Point(0);
            IPoint secondPt = pPtCol.get_Point(1);
            ILine pLine = new LineClass();
            pLine.PutCoords(firstPt, secondPt);
            IConstructPoint PConstructPts = new PointClass();

            //符号
            ILineSymbol pSym = BuildLineSymbol() ;

            try
            {

                ISegment pfirstSeg = pLine as ISegment;
                PConstructPts.ConstructPerpendicular(pfirstSeg, esriSegmentExtension.esriNoExtension, secondPt, 10, true);
                IPoint newPt = PConstructPts as IPoint;
                IPolyline firstLine = BuildPolyline(secondPt, newPt);

                ILineElement aElemnt = new LineElementClass();
                (aElemnt as IElement).Geometry = firstLine as IGeometry;
                aElemnt.Symbol = pSym;
                tmpEle1 = aElemnt as IElement;
                pGC.AddElement(tmpEle1, 0);


                PConstructPts.ConstructPerpendicular(pfirstSeg, esriSegmentExtension.esriNoExtension, secondPt, -10, true);
                newPt = PConstructPts as IPoint;
                IPolyline secondLine = BuildPolyline(newPt, secondPt);

                aElemnt = new LineElementClass();
                (aElemnt as IElement).Geometry = secondLine as IGeometry;
                aElemnt.Symbol = pSym;
                tmpEle2=aElemnt as IElement;
                pGC.AddElement(tmpEle2, 0);



                activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, this.activeView.Extent);
            }
            catch (Exception ex)
            {
            }


        }



    }
}
