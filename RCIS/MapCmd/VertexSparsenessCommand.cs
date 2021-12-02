using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Collections;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
namespace RCIS.MapTool
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("5800b3d7-f6fd-436f-abb4-c9c91504a800")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("RCIS.MapTool.VertexSparsenessCommand")]
    public sealed class VertexSparsenessCommand : BaseCommand
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
        public VertexSparsenessCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "节点抽稀";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "节点抽稀";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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

        private IGeometry Process(IGeometry pGeom, ISpatialReference pSR)
        {
            if(pGeom==null||pGeom.IsEmpty )return pGeom;
            IGeometry aResult = (pGeom as IClone).Clone()
                                as IGeometry;
            string resultDist= OtherHelper.InputBox("请输入最小点间距","小于该间距的会被删除","0.5");    
            
            double m_paramValue = 0.00;
            double.TryParse(resultDist,out m_paramValue);
            
            #region 只有线和面才处理
            IGeometryCollection aGC = aResult as IGeometryCollection;
                
            for(int gi=0;gi<aGC.GeometryCount ;gi++)
            {
                IGeometry aPart=aGC.get_Geometry (gi);  //每一部分
                ISegmentCollection aSegCol=aPart as ISegmentCollection ;
                for(int ai=0;ai<aSegCol.SegmentCount ;ai++)
                {
                    //每一段
                    ISegment iSeg=aSegCol.get_Segment (ai);
                    if(iSeg is ILine)
                    {
                        int aj = ai + 1;
                        while (aj < aSegCol.SegmentCount)
                        {
                            ISegment jSeg=aSegCol.get_Segment (aj);
                            if(jSeg is ILine)
                            {
                                if ((GeometryHelper.OnLine(iSeg.FromPoint
                                    , iSeg.ToPoint, jSeg.ToPoint, pSR)||
                                    (iSeg.Length<m_paramValue)))
                                {
                                    iSeg.ToPoint = jSeg.ToPoint;                                       
                                    aSegCol.RemoveSegments(aj, 1, false);
                                       
                                }
                                else break;
                            }else break;
                        }                            
                    }
                }
            }
            #endregion
            return aResult;
        
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add VertexSparsenessCommand.OnClick implementation
            IMap pMap = m_hookHelper.ActiveView.FocusMap;
            IFeatureLayer currLayer = RCIS.Global.GlobalEditObject.CurrEditTargetLayer;
            IFeatureClass targetFC = currLayer.FeatureClass ;
            string fcName = (targetFC as IDataset).Name.ToUpper();
            ISpatialReference pSR = (targetFC as IGeoDataset).SpatialReference;

            IFeatureLayer currLyr = LayerHelper.QueryLayerByModelName(pMap, fcName);
            ArrayList arSels= LayerHelper.GetSelectedFeature(pMap, currLayer as IGeoFeatureLayer, targetFC.ShapeType);
            if (arSels.Count == 0)
            {
                MessageBox.Show("当前地图必须选中一个以上编辑要素!");
                return;
            }

            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (IFeature aFea in arSels)
                {
                    IGeometry aSelGeom = aFea.ShapeCopy;
                    if (aSelGeom is IPolyline || aSelGeom is IPolygon)
                    {
                        IGeometry resultG = this.Process(aSelGeom,pSR);
                        aFea.Shape = resultG;
                        aFea.Store();
                    }
                }
                MessageBox.Show("抽稀完成！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("vertexsparseness");
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
            }



        }

        #endregion
    }
}
