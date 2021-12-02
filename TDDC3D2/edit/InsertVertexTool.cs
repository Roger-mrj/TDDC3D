using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.edit
{
    /// <summary>
    /// Summary description for InsertVertexTool.
    /// </summary>
    [Guid("af665d34-73f7-4538-9408-52a0211772f9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.InsertVertexTool")]
    public sealed class InsertVertexTool : BaseTool
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
        private IEngineEditor m_engineEditor;
        private IEngineEditLayers m_editLayer;

        public InsertVertexTool(IEngineEditor editor)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "插入节点";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            this.m_engineEditor = editor;
            this.m_editLayer = m_engineEditor as IEngineEditLayers;
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

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
                MessageBox.Show("请设置待修改要素所在的图层为编辑图层，\r\n并选中待修改要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add InsertVertexTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add InsertVertexTool.OnMouseDown implementation
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add InsertVertexTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add InsertVertexTool.OnMouseUp implementation
            try
            {
                IEngineEditSketch editSketch = m_engineEditor as IEngineEditSketch;
                IGeometry editShape = editSketch.Geometry;

                //location clicked as a point object
                IPoint clickedPt = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

                #region local variables used in the HitTest

                IHitTest hitShape = editShape as IHitTest;
                IPoint hitPoint = new PointClass();
                double hitDistance = 0;
                int hitPartIndex = 0;
                int hitSegmentIndex = 0;
                bool bRightSide = false;
                esriGeometryHitPartType hitPartType = hitPartType = esriGeometryHitPartType.esriGeometryPartBoundary;

                //the searchRadius is the maximum distance away, in map units, from the shape that will be used
                //for the test - change to an appropriate value.
                double searchRadius = 1;                
                #endregion

                hitShape.HitTest(clickedPt, searchRadius, hitPartType, hitPoint, ref hitDistance, ref hitPartIndex, ref hitSegmentIndex, ref bRightSide);

                //在搜索半径内 是否捕捉到点
                if (hitPoint.IsEmpty == false)
                {
                    IEngineSketchOperation sketchOp = new EngineSketchOperationClass();
                    sketchOp.Start(m_engineEditor);

                    //Get the PointCollection for a specific path or ring by hitPartIndex to handle multi-part features
                    IGeometryCollection geometryCol = editShape as IGeometryCollection;
                    IPointCollection pathOrRingPointCollection = geometryCol.get_Geometry(hitPartIndex) as IPointCollection;

                    object missing = Type.Missing;
                    object hitSegmentIndexObject = new object();
                    hitSegmentIndexObject = hitSegmentIndex;
                    object partIndexObject = new object();
                    partIndexObject = hitPartIndex;
                    esriEngineSketchOperationType opType = esriEngineSketchOperationType.esriEngineSketchOperationGeneral;

                    pathOrRingPointCollection.AddPoint(clickedPt, ref missing, ref hitSegmentIndexObject);
                    sketchOp.SetMenuString("Insert Vertex (Custom)");
                    opType = esriEngineSketchOperationType.esriEngineSketchOperationVertexAdded;

                    //remove the old PointCollection from the GeometryCollection and replace with the new one
                    geometryCol.RemoveGeometries(hitPartIndex, 1);
                    geometryCol.AddGeometry(pathOrRingPointCollection as IGeometry, ref partIndexObject, ref missing);

                    sketchOp.Finish(null, opType, clickedPt);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Unexpected Error");
            }
        }
        #endregion
    }
}
