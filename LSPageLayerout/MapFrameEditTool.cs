using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.Collections;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

using RCIS.GISCommon;

namespace BGHZSH.MapTool
{
    /// <summary>
    /// Summary description for MapFrameEditTool.
    /// </summary>
    [Guid("7ac938d9-f364-4821-b3f5-21b0cd110481")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("BGHZSH.MapTool.MapFrameEditTool")]
    public sealed class MapFrameEditTool : BaseTool
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper;


        private esriTrackerLocation m_trackerLoc = esriTrackerLocation.LocationNone;
        private bool m_shouldAction = false;
        private IMapFrame m_dataFrame = null;
        private IPoint m_lastPoint = null;
        private IPolygon m_dataFramePoly = null;
        private ISelectionTracker m_selTracker = null;

        private AxPageLayoutControl m_PageLayerCtl = null;

        public MapFrameEditTool(AxPageLayoutControl control)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "编辑外图框";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")

            this.m_PageLayerCtl = control;
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add MapFrameEditTool.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add MapFrameEditTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            //鼠标点击
            this.m_shouldAction = false;
            IGraphicsContainer gc = this.m_hookHelper.ActiveView.GraphicsContainer;
            IPoint pagePoint = this.m_PageLayerCtl.ToPagePoint(X, Y);
            if (this.m_dataFrame != null)
            {//如果存在DataFrame
                m_trackerLoc = this.m_selTracker.HitTest(pagePoint);
                if (esriTrackerLocation.LocationNone == this.m_trackerLoc)
                {
                    this.ClearSelectMapFrame();
                    this.m_dataFrame = null;
                    this.m_selTracker.ShowHandles = false;
                    this.m_selTracker = null;
                    //首先获取当前点击的DataFrame
                    this.m_dataFrame = this.QueryDataFrame(pagePoint) as IMapFrame;
                    if (this.m_dataFrame != null)
                    {
                        this.m_dataFramePoly = (this.m_dataFrame as IElement).Geometry as IPolygon;
                        this.m_cursor = System.Windows.Forms.Cursors.SizeAll;
                        this.m_selTracker = (this.m_dataFrame as IElement).SelectionTracker;
                        this.DrawSelectMapFrame();
                        this.m_trackerLoc = esriTrackerLocation.LocationNone;
                    }
                    this.m_hookHelper.ActiveView.Refresh();
                }
                else
                {
                    m_shouldAction = true;
                }
            }
            else
            {
                //首先获取当前点击的DataFrame
                this.m_dataFrame = this.QueryDataFrame(pagePoint) as IMapFrame;
                if (this.m_dataFrame != null)
                {
                    this.m_dataFramePoly = (this.m_dataFrame as IElement).Geometry as IPolygon;
                    this.DrawSelectMapFrame();
                    this.m_cursor = System.Windows.Forms.Cursors.SizeAll;
                    this.m_selTracker = (this.m_dataFrame as IElement).SelectionTracker;
                    this.m_trackerLoc = esriTrackerLocation.LocationNone;
                    this.m_hookHelper.ActiveView.Refresh();
                }
            }
            this.m_lastPoint = pagePoint;  
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            IPoint pagePoint = this.m_PageLayerCtl.ToPagePoint(X, Y);
            if (this.m_dataFrame == null)
            {
                if (this.HasDataFrame(pagePoint))
                {
                    this.m_cursor = System.Windows.Forms.Cursors.SizeAll;
                }
                else
                {
                    this.m_cursor = System.Windows.Forms.Cursors.Arrow;
                }
            }
            else if (this.m_dataFrame != null)
            {
                if (this.m_shouldAction)
                {//如果有当前DataFrame
                    this.MoveMapFrame(this.m_lastPoint, pagePoint);
                    this.m_cursor = System.Windows.Forms.Cursors.SizeAll;
                }
                else
                {
                    esriTrackerLocation loc = this.m_selTracker.HitTest(pagePoint);
                    if (esriTrackerLocation.LocationNone == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.Arrow;
                    }
                    else if (esriTrackerLocation.LocationInterior == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeAll;
                    }
                    else if (esriTrackerLocation.LocationBottomLeft == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeNESW;
                    }
                    else if (esriTrackerLocation.LocationTopRight == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeNESW;
                    }
                    else if (esriTrackerLocation.LocationBottomRight == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    }
                    else if (esriTrackerLocation.LocationTopLeft == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeNWSE;
                    }
                    else if (esriTrackerLocation.LocationMiddleLeft == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeWE;
                    }
                    else if (esriTrackerLocation.LocationMiddleRight == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeWE;
                    }
                    else if (esriTrackerLocation.LocationBottomMiddle == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeNS;
                    }
                    else if (esriTrackerLocation.LocationTopMiddle == loc)
                    {
                        this.m_cursor = System.Windows.Forms.Cursors.SizeNS;
                    }
                }
            }
            //无论如何也要将当前点设置好
            this.m_lastPoint = pagePoint;
            base.OnMouseMove(Button, Shift, X, Y);
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (this.m_dataFrame != null && this.m_shouldAction)
            {
                IPoint pagePoint = this.m_PageLayerCtl.ToPagePoint(X, Y);
                this.StopMoveMapFrame(this.m_lastPoint, pagePoint);
                this.m_shouldAction = false;
            }
            base.OnMouseUp(Button, Shift, X, Y);
        }
        #endregion


        private IElement QueryDataFrame(IPoint locatePoint)
        {
            IElement dataFrame = null;
            IGraphicsContainer gc = this.m_hookHelper.ActiveView.GraphicsContainer;
            IEnumElement eleAry = gc.LocateElements(locatePoint, 0);
            if (eleAry == null)
                return dataFrame;
            IElement pageEle = eleAry.Next();
            ArrayList mapFrameList = new ArrayList();
            while (pageEle != null)
            {
                if (pageEle is IMapFrame)
                {
                    mapFrameList.Add(pageEle);
                }
                pageEle = eleAry.Next();
            }
            if (mapFrameList.Count > 0)
            {
                if (mapFrameList.Count == 1)
                {
                    dataFrame = mapFrameList[0] as IElement;
                }
                else
                {
                    #region 如果IMapFrame多于一个那么选取面积最小的那个
                    int frameCount = mapFrameList.Count;
                    IElement bestFrame = mapFrameList[0] as IElement;
                    double minArea = double.MaxValue;
                    IArea areaGeom = bestFrame.Geometry as IArea;
                    if (areaGeom == null)
                        minArea = Double.MaxValue;
                    else
                    {
                        minArea = Math.Abs(areaGeom.Area);
                    }
                    for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                    {
                        IElement curEle = mapFrameList[frameIndex] as IElement;
                        double curArea = Double.MaxValue;
                        areaGeom = curEle.Geometry as IArea;
                        if (areaGeom == null)
                            curArea = Double.MaxValue;
                        else
                        {
                            curArea = Math.Abs(areaGeom.Area);
                        }
                        if (curArea < minArea)
                        {
                            minArea = curArea;
                            bestFrame = curEle;
                        }
                    }
                    dataFrame = bestFrame;
                    #endregion
                }
            }
            return dataFrame;
        }
        private bool HasDataFrame(IPoint locatePoint)
        {
            bool hasDataFrame = false;
            IGraphicsContainer gc = this.m_PageLayerCtl.ActiveView.GraphicsContainer;
            IEnumElement eleAry = gc.LocateElements(locatePoint, 0);
            if (eleAry != null)
            {
                IElement pageEle = eleAry.Next();
                while (pageEle != null)
                {
                    if (pageEle is IMapFrame)
                    {
                        hasDataFrame = true;
                        break;
                    }
                    pageEle = eleAry.Next();
                }
            }
            return hasDataFrame;
        }

        private void MoveMapFrame(IPoint fromPoint, IPoint toPoint)
        {
            if (this.m_dataFrame != null)
            {
                this.ClearSelectMapFrame();
                switch (this.m_trackerLoc)
                {
                    case esriTrackerLocation.LocationInterior:
                        {
                            #region 整体移动
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;
                            ;
                            ITransform2D transform = this.m_dataFramePoly as ITransform2D;
                            transform.Move(deltaX, deltaY);
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationBottomLeft:
                        {//移动左下角 需要修改的是左上、左下、右下
                            #region 移动左下点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;
                            IPoint topLeft = frameEnv.UpperLeft;
                            topLeft.X += deltaX;
                            frameEnv.UpperLeft = topLeft;

                            IPoint lowRight = frameEnv.LowerRight;
                            lowRight.Y += deltaY;
                            frameEnv.LowerRight = lowRight;
                            frameEnv.LowerLeft = toPoint;
                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationBottomMiddle:
                        {//下边中间 需要修改 左下、右下
                            #region 移动中下点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;
                            //右下
                            IPoint lowRight = frameEnv.LowerRight;
                            lowRight.Y += deltaY;
                            //左下
                            IPoint lowLeft = frameEnv.LowerLeft;
                            lowLeft.Y += deltaY;

                            frameEnv.LowerLeft = lowLeft;
                            frameEnv.LowerRight = lowRight;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationBottomRight:
                        {//右下点 需要修改左下、右下、右上
                            #region 移动右下点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;
                            //右下
                            IPoint lowRight = frameEnv.LowerRight;
                            lowRight.Y += deltaY;
                            //左下
                            IPoint lowLeft = frameEnv.LowerLeft;
                            lowLeft.Y += deltaY;
                            //右上
                            IPoint topRight = frameEnv.UpperRight;
                            topRight.X += deltaX;
                            frameEnv.LowerLeft = lowLeft;
                            frameEnv.LowerRight = lowRight;
                            frameEnv.UpperRight = topRight;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationTopLeft:
                        {//左上 需要修改左上、右上、左下
                            #region 移动左上点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;

                            IPoint rightTop = frameEnv.UpperRight;
                            rightTop.Y += deltaY;

                            IPoint leftLow = frameEnv.LowerLeft;
                            leftLow.X += deltaX;

                            frameEnv.UpperLeft = toPoint;
                            frameEnv.LowerLeft = leftLow;
                            frameEnv.UpperRight = rightTop;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationTopMiddle:
                        {//中上 需要移动左上、右上
                            #region 移动中上点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;

                            IPoint leftTop = frameEnv.UpperLeft;
                            leftTop.Y += deltaY;

                            IPoint rightTop = frameEnv.UpperRight;
                            rightTop.Y += deltaY;

                            frameEnv.UpperLeft = leftTop;
                            frameEnv.UpperRight = rightTop;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationTopRight:
                        {//右上 需要移动左上、右上、右下
                            #region 移动右上点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;

                            IPoint leftTop = frameEnv.UpperLeft;
                            leftTop.Y += deltaY;

                            IPoint rightLow = frameEnv.LowerRight;
                            rightLow.X += deltaX;

                            frameEnv.UpperLeft = leftTop;
                            frameEnv.LowerRight = rightLow;
                            frameEnv.UpperRight = toPoint;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                    case esriTrackerLocation.LocationMiddleLeft:
                        {//中左点  需要移动左上和左下
                            #region 移动中左点
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;

                            IPoint leftTop = frameEnv.UpperLeft;
                            leftTop.X += deltaX;

                            IPoint leftLow = frameEnv.LowerLeft;
                            leftLow.X += deltaX;

                            frameEnv.LowerLeft = leftLow;
                            frameEnv.UpperLeft = leftTop;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;

                            #endregion
                        }
                    case esriTrackerLocation.LocationMiddleRight:
                        {//中右点 需要移动右上和右下
                            #region 移动中右
                            IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                            double deltaX = toPoint.X - fromPoint.X;
                            double deltaY = toPoint.Y - fromPoint.Y;

                            IPoint topRight = frameEnv.UpperRight;
                            topRight.X += deltaX;

                            IPoint lowRight = frameEnv.LowerRight;
                            lowRight.X += deltaX;

                            frameEnv.UpperRight = topRight;
                            frameEnv.LowerRight = lowRight;

                            PolygonClass poly = new PolygonClass();
                            poly.SetRectangle(frameEnv);
                            this.m_dataFramePoly = poly;
                            break;
                            #endregion
                        }
                }
                this.DrawSelectMapFrame();
                IEnvelope refreshEnv = this.m_PageLayerCtl.ActiveView.Extent;
                this.m_PageLayerCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, refreshEnv);
            }
        }
        private void StopMoveMapFrame(IPoint fromPoint, IPoint toPoint)
        {
            this.MoveMapFrame(fromPoint, toPoint);
            (this.m_dataFrame as IElement).Geometry = this.m_dataFramePoly;
            this.m_PageLayerCtl.ActiveView.Refresh();
        }
        private void DrawSelectMapFrame()
        {
            if (this.m_dataFrame != null)
            {
                IEnvelope frameEnv = this.m_dataFramePoly.Envelope;
                IPoint center = (frameEnv as IArea).Centroid;
                double width = frameEnv.Width / 2;
                double height = frameEnv.Height / 2;
                this.PutEdgePointElement("MapFrame-BL", center.X - width, center.Y - height);
                this.PutEdgePointElement("MapFrame-ML", center.X - width, center.Y);
                this.PutEdgePointElement("MapFrame-TL", center.X - width, center.Y + height);
                this.PutEdgePointElement("MapFrame-TM", center.X, center.Y + height);
                this.PutEdgePointElement("MapFrame-TR", center.X + width, center.Y + height);
                this.PutEdgePointElement("MapFrame-MR", center.X + width, center.Y);
                this.PutEdgePointElement("MapFrame-BR", center.X + width, center.Y - height);
                this.PutEdgePointElement("MapFrame-BM", center.X, center.Y - height);
                this.PutMapFrameElement();
            }
        }
        private void ClearSelectMapFrame()
        {
            this.DeleteEdgePointElement("MapFrame-BL");
            this.DeleteEdgePointElement("MapFrame-ML");
            this.DeleteEdgePointElement("MapFrame-TL");
            this.DeleteEdgePointElement("MapFrame-TM");
            this.DeleteEdgePointElement("MapFrame-TR");
            this.DeleteEdgePointElement("MapFrame-MR");
            this.DeleteEdgePointElement("MapFrame-BR");
            this.DeleteEdgePointElement("MapFrame-BM");
            this.DeleteMapFrameElement();
        }
        private void DeleteEdgePointElement(string name)
        {
            IGraphicsContainer gc = this.m_PageLayerCtl.ActiveView.GraphicsContainer;
            IElement delEle = null;
            IElement ptEle = gc.Next();
            while (ptEle != null)
            {
                IElementProperties props = ptEle as IElementProperties;
                if (props.Name.Equals(name))
                {
                    delEle = ptEle;
                    gc.Reset();
                    break;
                }
                ptEle = gc.Next();
            }
            if (delEle != null)
            {
                IGeometry geom = delEle.Geometry;
                gc.DeleteElement(delEle);
            }
        }

        private void DeleteMapFrameElement()
        {
            IGraphicsContainer gc = this.m_PageLayerCtl.ActiveView.GraphicsContainer;
            IElement delEle = null;
            IElement ptEle = gc.Next();
            while (ptEle != null)
            {
                IElementProperties props = ptEle as IElementProperties;
                if (props.Name.Equals("MapFrame-OL"))
                {
                    delEle = ptEle;
                    gc.Reset();
                    break;
                }
                ptEle = gc.Next();
            }
            if (delEle != null)
            {
                IGeometry geom = delEle.Geometry;
                gc.DeleteElement(delEle);
            }
        }
        private void PutEdgePointElement(string name, double px, double py)
        {
            SimpleMarkerSymbolClass marker = new SimpleMarkerSymbolClass();
            marker.Color = ColorHelper.CreateColor(200, 0, 0);
            marker.Size = 4;
            marker.Style = esriSimpleMarkerStyle.esriSMSSquare;

            IPoint anchorPoint = new PointClass();
            anchorPoint.PutCoords(px, py);

            MarkerElementClass ptEle = new MarkerElementClass();
            ptEle.Name = name;
            ptEle.Geometry = anchorPoint;
            ptEle.Symbol = marker as IMarkerSymbol;
            IGraphicsContainer gc = this.m_PageLayerCtl.ActiveView.GraphicsContainer;
            gc.AddElement(ptEle, 0);
        }

        private void PutMapFrameElement()
        {
            if (this.m_dataFramePoly != null)
            {
                ITopologicalOperator topOp = this.m_dataFramePoly as ITopologicalOperator;
                IGeometry outlineGeom = topOp.Boundary;

                SimpleLineSymbolClass outlineStyle = new SimpleLineSymbolClass();
                outlineStyle.Width = 1.5;
                outlineStyle.Color = ColorHelper.CreateColor(0, 0, 250);
                outlineStyle.Style = esriSimpleLineStyle.esriSLSDash;

                LineElementClass outlineEle = new LineElementClass();
                outlineEle.Geometry = outlineGeom;
                outlineEle.Symbol = outlineStyle as ILineSymbol;

                IElementProperties props = outlineEle as IElementProperties;
                props.Name = "MapFrame-OL";

                IGraphicsContainer gc = this.m_PageLayerCtl.ActiveView.GraphicsContainer;
                gc.AddElement(outlineEle, 0);
            }
        }

    }
}
