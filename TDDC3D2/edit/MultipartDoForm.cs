using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.edit
{
    public partial class MultipartDoForm : Form
    {
        public MultipartDoForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;

        private void MultipartDoForm_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);

        }
        List<IFeature> lstMultiFeas = new List<IFeature>();
        IFeatureLayer currLayer = null;
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //得到该图层数量
            string className = OtherHelper.GetLeftName( this.cmbLayer.Text.Trim());
            currLayer= LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍后...", "正在查找多部分要素，请稍等...");
            wait.Show();
            try{
                lstMultiFeas.Clear();
                IIdentify identify = currLayer as IIdentify;
               // IFeatureSelection pFSelection = currLayer as IFeatureSelection;
                IArray array = identify.Identify((this.currMap as IActiveView).Extent);
                
                int allCount = 0;
                int iCount = 0;
                if (array != null)
                {
                    allCount = array.Count;
                    for (int i = 0; i < array.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = array.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature aFea = pRow.Row as IFeature;

                        IGeometry pGeometry = aFea.ShapeCopy;
                        IGeometryCollection pGeocoll = pGeometry as IGeometryCollection;
                        if (pGeocoll.GeometryCount > 1)
                        {
                            lstMultiFeas.Add(aFea);
                            iCount++;
                            //同时选择要素
                            // pFSelection.Add(aFea);
                        }


                    }
                }
                wait.Close();
                (this.currMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewAll, null, (this.currMap as IActiveView).Extent);
                this.lblNumbers.Text = "当前区域共" + allCount + "个要素,多部分要素" + iCount + "个。";
            }
            catch(Exception ex)
            {
                if (wait!=null)
                    wait.Close();
                MessageBox.Show(ex.Message);

            }
            
            


        }

        private void DoCurrentFw()
        {
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                IFeatureClass pFC = currLayer.FeatureClass;
                //如果是 范围内所有要素
                if (pFC.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    foreach (IFeature aFea in lstMultiFeas)
                    {
                        DoAPolylineFeature(aFea);
                    }
                }
                else if (pFC.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    foreach (IFeature aFea in lstMultiFeas)
                    {
                        //2017-11-22修改，polygon 改为 打断多个外环。
                        DoAPolygonFeature(aFea);
                    }
                }                

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("multipartdo");
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
        }

        private void DoAPolylineFeature(IFeature aFea)
        {
            IFeatureClass pFC = currLayer.FeatureClass;
            IGeometry aGeo = aFea.ShapeCopy;

            IGeometryCollection pGeoCol = aGeo as IGeometryCollection;
            for (int i = 0; i < pGeoCol.GeometryCount; i++)
            {
                IFeature newFeaturre = pFC.CreateFeature();
                IFeatureEdit newFeaEdit = newFeaturre as IFeatureEdit;
                //娘的，属性全没了
                IGeometry newGeom = pGeoCol.get_Geometry(i);               
                IGeometryCollection geoLine = new PolylineClass();
                geoLine.AddGeometry(newGeom);
                IPolyline curLine = geoLine as IPolyline;
                curLine.SimplifyNetwork();
                newFeaturre.Shape = curLine;                
                FeatureHelper.CopyFeature(aFea, newFeaturre);
                newFeaturre.Store();
            }
            //旧的删除
            aFea.Delete();
        }

        private void DoAPolygonFeature(IFeature aFea)
        {
            

            IFeatureClass pFC = currLayer.FeatureClass;
            IGeometry aGeo = aFea.ShapeCopy;
            IGeometryBag exteriorRingGeoBag = (aGeo as IPolygon4).ExteriorRingBag;
            IGeometryCollection extRingEoCol = exteriorRingGeoBag as IGeometryCollection; //获得所有外环
            for (int i = 0; i < extRingEoCol.GeometryCount; i++)
            {
                IGeometry extRingGeometry = extRingEoCol.get_Geometry(i);
                IGeometryCollection geoPolygon = new PolygonClass();
                geoPolygon.AddGeometry(extRingGeometry);
                
                

                //如果这个图形有内环
                IGeometryBag interiorRingGeoBag = (aGeo as IPolygon4).get_InteriorRingBag(extRingGeometry as IRing);
                IGeometryCollection inRingGeomCollection = interiorRingGeoBag as IGeometryCollection;
                for (int k = 0; k < inRingGeomCollection.GeometryCount; k++)
                {
                    IGeometry interRingGeo = inRingGeomCollection.get_Geometry(k);
                    geoPolygon.AddGeometry(interRingGeo);
                }

                IPolygon4 polyGonGeo = geoPolygon as IPolygon4;
                polyGonGeo.Project(aFea.Shape.SpatialReference);
                


                IFeature newFeaturre = pFC.CreateFeature();
                IFeatureEdit newFeaEdit = newFeaturre as IFeatureEdit;
                //娘的，属性全没了
                newFeaturre.Shape = polyGonGeo as IGeometry;
                FeatureHelper.CopyFeature(aFea, newFeaturre);
                newFeaturre.Store();
            }

            //旧的删除
            aFea.Delete();

        }

        //private void DoAFeature(IFeature aFea)
        //{
        //    IFeatureClass pFC = currLayer.FeatureClass;
        //    IGeometry aGeo = aFea.ShapeCopy;

            


        //    IGeometryCollection pGeoCol = aGeo as IGeometryCollection;
        //    for (int i = 0; i < pGeoCol.GeometryCount; i++)
        //    {
        //        IFeature newFeaturre = pFC.CreateFeature();
        //        IFeatureEdit newFeaEdit = newFeaturre as IFeatureEdit;
        //        //娘的，属性全没了
        //        IGeometry newGeom = pGeoCol.get_Geometry(i);
        //        newGeom.SpatialReference = aFea.Shape.SpatialReference;
        //        if (aFea.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
        //        {
        //            IGeometryCollection geoPolygon = new PolygonClass();
        //            geoPolygon.AddGeometry(newGeom);
        //            IPolygon polyGonGeo = geoPolygon as IPolygon;
        //         //   polyGonGeo.SimplifyPreserveFromTo();
        //            newFeaturre.Shape = polyGonGeo;

        //        }
        //        else if (aFea.Shape.GeometryType == esriGeometryType.esriGeometryPolyline)
        //        {
        //            IGeometryCollection geoLine = new PolylineClass();
        //            geoLine.AddGeometry(newGeom);
        //            IPolyline curLine = geoLine as IPolyline;
        //            curLine.SimplifyNetwork();
        //            newFeaturre.Shape = curLine;
        //        }
        //        FeatureHelper.CopyFeature(aFea, newFeaturre);
        //        newFeaturre.Store();

        //    }
        //    //旧的删除
        //    aFea.Delete();
        //}
        private void doSelectFeature()
        {
            System.Collections.ArrayList arSel = LayerHelper.GetSelectedFeature(this.currMap, currLayer as IGeoFeatureLayer, currLayer.FeatureClass.ShapeType);
            if (arSel.Count == 0)
            {
                MessageBox.Show("当前没有选中要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                if (currLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    foreach (IFeature aFea in arSel)
                    {
                        DoAPolylineFeature(aFea);
                    }
                }
                else if (currLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    foreach (IFeature aFea in arSel)
                    {
                        //2017-11-22修改，polygon 改为 打断多个外环。
                        DoAPolygonFeature(aFea);
                    }
                }
                
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("multipartdo");
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (currLayer == null)
                return;
            if (this.radioGroup1.SelectedIndex == 0)
            {
                doSelectFeature();
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                this.DoCurrentFw();
            }
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
