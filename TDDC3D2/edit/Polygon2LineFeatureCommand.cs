using System;
using System.Drawing;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using RCIS.Global;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("4d437cb6-155e-40a7-ae91-d41d0bb6f492")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.Polygon2LineFeatureCommand")]
    public sealed class Polygon2LineFeatureCommand : BaseCommand
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
        public Polygon2LineFeatureCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "提取面边界";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "提取面边界";  //localizable text
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

        private IPolyline PolygonToLine(IPolygon pPolygon)
        {
            //一条线？
            IGeometryCollection pGeometryCollectionPolygon;
            IClone pClone;
            ISegmentCollection pSegmentCollectionPath;
            object o = Type.Missing;
            IGeometryCollection pGeoCol = new PolylineClass();
            pClone = (IClone)pPolygon;
            pGeometryCollectionPolygon = pClone.Clone() as IGeometryCollection;
            for (int i = 0; i < pGeometryCollectionPolygon.GeometryCount; i++)
            {
                pSegmentCollectionPath = new PathClass();
                pSegmentCollectionPath.AddSegmentCollection(pGeometryCollectionPolygon.get_Geometry(i) as ISegmentCollection);
                pGeoCol.AddGeometry(pSegmentCollectionPath as IGeometry, ref o, ref o);
            }
            return pGeoCol as IPolyline;
        }

        /// <summary>
        /// 提取公共边界,返回图形
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="extentGeo"></param>
        /// <param name="aLine"></param>
        /// <returns></returns>
        private List<IGeometry> getIntersectLine(IFeatureClass featureClass, IGeometry extentGeo)
        {
            List<IGeometry> lst = new List<IGeometry>();
            ITopologicalOperator pTOP = extentGeo as ITopologicalOperator;
            IRelationalOperator pRO1=extentGeo as IRelationalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                
                ISpatialFilter pSR = new SpatialFilterClass();
                pSR.Geometry = extentGeo;
                pSR.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = featureClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                while (aFea != null)
                {
                    IGeometry aGeo = aFea.ShapeCopy;
                    IRelationalOperator pRO2 = aGeo as IRelationalOperator;

                    if ((pRO2.Contains(extentGeo)) && (pRO1.Contains(aGeo)))
                    {
                        aFea = pCursor.NextFeature();
                        continue;
                    }
                    IGeometry interGeo = pTOP.Intersect(aGeo, esriGeometryDimension.esriGeometry1Dimension);
                    if (!interGeo.IsEmpty)
                    {
                        //IPolyline aPolyline = interGeo as IPolyline;
                        lst.Add(interGeo);
                    }
                                   
                    aFea = pCursor.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

            }
            return lst;
        }



       

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add Polygon2LineFeatureCommand.OnClick implementation
            IMap aMap = m_hookHelper.ActiveView.FocusMap;

            GetPolygonBoundaryForm frm = new GetPolygonBoundaryForm();
            frm.currMap = aMap;
            if (frm.ShowDialog() == DialogResult.Cancel)
                return;
            string polygonLayerName = frm.polygonLayerName;
            string lineLayerName = frm.PolylineLayerName;
            IGeoFeatureLayer polygonLayer = LayerHelper.QueryLayerByModelName(aMap, polygonLayerName);
            IFeatureClass polygonClass = polygonLayer.FeatureClass;
            //获得选中图斑
            ArrayList arSelectedPolygon = LayerHelper.GetSelectedFeature(aMap, polygonLayer,esriGeometryType.esriGeometryPolygon);
            if (arSelectedPolygon.Count == 0)
            {
                MessageBox.Show("当前未选中要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始提取", "正在执行，请稍后...");
            wait.Show();

            IFeatureLayer polylineLayer = LayerHelper.QueryLayerByModelName(aMap, lineLayerName);
            IFeatureClass lineClass = polylineLayer.FeatureClass;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (IFeature aPolygonFea in arSelectedPolygon)
                {
                    wait.SetCaption("已提取要素" + aPolygonFea.OID + "...");
                    IPolygon  aPolygon = aPolygonFea.ShapeCopy as IPolygon ;
                    //IPolyline polyline = PolygonToLine(aPolygon);
                    //IFeature newLineFea = lineClass.CreateFeature();
                    //newLineFea.Shape = polyline;
                    //newLineFea.Store(); 
                    ////先转化为一条边
                    ////判断跟其他交叉，如果有 其他线交与 之交与一条线，则用这条线的两个断点将之分割
                    //List<IGeometry> lstInterLine = getIntersectLine(polygonClass, aPolygon);

                    //foreach (IGeometry aLine in lstInterLine)
                    //{
                    //    IPolyline aPolyline = aLine as IPolyline;
                    //    IPoint fromPt = aPolyline.FromPoint;
                    //    IPoint toPt = aPolyline.ToPoint;

                    //    ISet set = null;
                    //    try
                    //    {
                    //        set = (newLineFea as IFeatureEdit).Split(fromPt); 
                    //    }
                    //    catch { }
                    //    if (set != null)
                    //    {
                    //        set.Reset();
                    //        for (IFeature newFea2 = set.Next() as IFeature; newFea2 != null; newFea2 = set.Next() as IFeature)
                    //        {
                    //            IFeature newLineFea2 = lineClass.CreateFeature();
                    //            newLineFea2.Shape = newFea2.ShapeCopy;
                    //            newLineFea2.Store(); 
                    //        }
                    //    }
                        


                    //}

                    IGeometryCollection geoPolygon = new PolygonClass();

                    IGeometryBag exteriorRingGeoBag = (aPolygon as IPolygon4).ExteriorRingBag;
                    IGeometryCollection extRingEoCol = exteriorRingGeoBag as IGeometryCollection; //获得所有外环
                    for (int i = 0; i < extRingEoCol.GeometryCount; i++)
                    {
                        IGeometry extRingGeometry = extRingEoCol.get_Geometry(i);                        
                        geoPolygon.AddGeometry(extRingGeometry);



                        //如果这个图形有内环
                        IGeometryBag interiorRingGeoBag = (aPolygon as IPolygon4).get_InteriorRingBag(extRingGeometry as IRing);
                        IGeometryCollection inRingGeomCollection = interiorRingGeoBag as IGeometryCollection;
                        for (int k = 0; k < inRingGeomCollection.GeometryCount; k++)
                        {
                            IGeometry interRingGeo = inRingGeomCollection.get_Geometry(k);
                            geoPolygon.AddGeometry(interRingGeo);
                        }

                    }

                    //获取所有面状图形
                    for (int j = 0; j < geoPolygon.GeometryCount; j++)
                    {
                        IGeometry aGeoPoly = geoPolygon.get_Geometry(j); //一个环

                        IPointCollection polygonPoints = aGeoPoly as IPointCollection;
                        for (int i = 0; i < polygonPoints.PointCount - 1; i++)
                        {
                            IPoint pt1 = polygonPoints.get_Point(i);
                            IPoint pt2 = polygonPoints.get_Point(i + 1);
                            ILine aLIne = new LineClass();
                            aLIne.PutCoords(pt1, pt2);

                            ISegmentCollection gc = new PolylineClass();
                            gc.AddSegment(aLIne as ISegment);
                            IPolyline polyline = gc as IPolyline;

                            IFeature newFea = lineClass.CreateFeature();
                            newFea.Shape = polyline;
                            newFea.Store();
                        }

                    }
                    
                    
                    
                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("polygon2line");
                wait.Close();
                MessageBox.Show("生成完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }


        }

        #endregion
    }
}
