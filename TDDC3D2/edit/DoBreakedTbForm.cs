using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.edit
{
    public partial class DoBreakedTbForm : Form
    {
        public DoBreakedTbForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IWorkspace currWS = null;
        private void DissoveMergeTbForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap,true,esriGeometryType.esriGeometryPolygon);
            
        }


        private void DoIntersec(IFeatureClass pFC, IFeature aDltb)
        {
            //找与他压盖的 面
            ITopologicalOperator ptop = aDltb.Shape as ITopologicalOperator;
            List<IFeature> lst = GetFeaturesHelper.getFeaturesByGeo(pFC, aDltb.Shape, esriSpatialRelEnum.esriSpatialRelIntersects);
            foreach (IFeature aIntersec in lst)
            {
                if (aIntersec.OID == aDltb.OID)
                    continue;
                IGeometry pGeo = ptop.Intersect(aIntersec.Shape, esriGeometryDimension.esriGeometry2Dimension);
                if (pGeo.IsEmpty)
                    continue;
                IGeometry restGeo = ptop.Difference(pGeo);
                if (restGeo.IsEmpty)
                {
                    continue;
                }
                aDltb.Shape = restGeo;
                aDltb.Store();
            }
        }

        private void DoATB(IFeature aSrcDltb,double threshhold,double minLengthHold,double mjHold)
        {
            IGeometry aGeo = aSrcDltb.ShapeCopy;
            //获取所有点
            IPointCollection ptCols = aGeo as IPointCollection;
            //第一个夹角

            List<IGeometry> newSmallGeo = new List<IGeometry>();

            double minAngle = 180;
         
            IPoint pt1 = ptCols.get_Point(0);
            IPoint pt2 = ptCols.get_Point(1);

            //第一条线段边长
            ILine line1 = new LineClass();
            line1.PutCoords(pt1, pt2);
            double length1 = line1.Length;
            double minLen = length1;

            IPoint pt3 = null;
            GeometryEnvironment geometryEnvironment = new GeometryEnvironment();
            IConstructAngle constructAngle = geometryEnvironment as IConstructAngle;

            for (int j = 1; j < ptCols.PointCount - 1; j++)
            {
                pt3 = ptCols.get_Point(j + 1);
                double piangle = constructAngle.ConstructThreePoint(pt1, pt2, pt3);
                double angle = piangle * 180 / Math.PI;

                //第二条线段边长
                ILine line2 = new LineClass();
                line2.PutCoords(pt3, pt2);
                double length2 = line2.Length;
                if (length2 < length1)
                {
                    minLen = length2; //最小边长
                }

                //将该夹角转换为正值
                if (angle < 0)
                {
                    angle = 360 + angle;
                }
                if (angle > 180)
                {
                    angle = 360 - angle;
                }

                if (minAngle > angle)
                {
                    minAngle = angle;
                    //如果两条边的最小长度比 设定的长度阈值还要大，则不处理
                    if (minLen < minLengthHold)
                    {
                        
                        #region  //如果找到一个夹角，则生成一个新的图版，然后minangle 继续最大，找第二个符合条件的
                        if (angle <= threshhold)
                        {
                            //三点合并，并于原来图形合并
                            IPointCollection points = new MultipointClass();
                            points.AddPoint(pt1);
                            points.AddPoint(pt2);
                            points.AddPoint(pt3);

                            Ring ring = new RingClass();
                            for (int idx = 0; idx < points.PointCount; idx++)
                            {
                                ring.AddPoint(points.get_Point(idx), Type.Missing, Type.Missing);
                            }
                            ring.AddPoint(points.get_Point(0));  //必须补充起点，作为终点

                            IGeometryCollection pointPolygon = new PolygonClass();
                            pointPolygon.AddGeometry(ring as IGeometry, Type.Missing, Type.Missing);
                            IPolygon polygonGeo = pointPolygon as IPolygon;
                            //简化节点次序
                            polygonGeo.SimplifyPreserveFromTo();
                            //如果这个新图形的面积 ，大于阈值，也不处理
                            if ((polygonGeo as IArea).Area < mjHold)
                            {
                                newSmallGeo.Add(polygonGeo);
                            }

                            

                            minAngle = 180;

                        }
                        #endregion 
                    }
                    

                }

                pt1 = pt2;
                pt2 = pt3;

            }
            //逐个将小图斑合并
            ITopologicalOperator pTop = aGeo as ITopologicalOperator;
            if (newSmallGeo.Count > 0)
            {
                
                IGeometry newShape = null;
                foreach (IGeometry newGeo in newSmallGeo)
                {
                    newShape = pTop.Union(newGeo);
                    pTop = newShape as ITopologicalOperator;
                }
                aSrcDltb.Shape = newShape;
                aSrcDltb.Store();
            }
        }

      
        

        //private void Excute(string layerName,double distance)
        //{

        //    IFeatureClass pFC = (this.currWS as IFeatureWorkspace).OpenFeatureClass(layerName);
        //    IFeatureLayer pFeaLayer = new FeatureLayerClass();
        //    pFeaLayer.FeatureClass = pFC;
        //    IIdentify identify = pFeaLayer as IIdentify;
        //    ESRI.ArcGIS.esriSystem.IArray arrIds = identify.Identify((this.currMap as IActiveView).Extent);
        //    if (arrIds == null) return;
        //    //获取当前要素数量
        //    for (int i = 0; i < arrIds.Count; i++)
        //    {
        //        IFeatureIdentifyObj idObj = arrIds.get_Element(i) as IFeatureIdentifyObj;
        //        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
        //        IFeature aSrcDltb = pRow.Row as IFeature;
                
        //        //处理裂隙
        //        ArrayList arTouchedFeas =RCIS.GISCommon.GetFeaturesByGeoHelper.getTouchedFeature(pFC, aSrcDltb.ShapeCopy);
        //        foreach (IFeature aTouchedFea in arTouchedFeas)
        //        {
        //            // 原始面上的所有点，对 相邻边面的每条边，算最短距离，记录最短距离的那条边和这个点
        //            //如果最短距离超过 容差了，则不处理
        //            IPointCollection ptCols = aSrcDltb.ShapeCopy as IPointCollection;
        //            ISegmentCollection segCols = aTouchedFea.ShapeCopy as ISegmentCollection;

        //            double minDist =9999999;
        //            IPoint inPt1 = null, inpt2 = null, inpt3 = null;
        //            IGeometry newSmallPolygon = null;
        //            #region 查找裂隙
        //            for (int idx = 0; idx < ptCols.PointCount; idx++)
        //            {
        //                IPoint aSrcPt = ptCols.get_Point(idx);
        //                //这点属于相交的部分掠过
        //                ITopologicalOperator pTopSrcPt = aSrcPt as ITopologicalOperator;
        //                if (!pTopSrcPt.Intersect(aTouchedFea.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension).IsEmpty)
        //                    continue;

        //                for (int idx2 = 0; idx2 < segCols.SegmentCount; idx2++)
        //                {
        //                    ISegment aSeg = segCols.get_Segment(idx2);
        //                    ICurve aCurve = aSeg as ICurve;
        //                    IPoint outPt = null;
        //                    double distAlongCurveFrom = 0; //曲线其实点到输出点部分的长度
        //                    double distFromCurve = 0;//输出点到输入点的距离
        //                    bool isRightSide = false;//输入点是否在曲线的右边
        //                    aCurve.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, aSrcPt, false, outPt, ref distAlongCurveFrom, ref distFromCurve, ref isRightSide);

        //                    #region 判断距离
        //                    if (minDist > distFromCurve)
        //                    {
        //                        minDist = distFromCurve;
        //                    }
        //                    if (distFromCurve > distance)
        //                    {
                                
        //                        continue;
        //                    }
        //                    //如果距离是 0 的也掠过
        //                    if (distFromCurve < 0.0001)
        //                    {
        //                        continue;
        //                    }
        //                    #endregion 
        //                    inPt1 = aCurve.FromPoint;
        //                    inpt2 = aCurve.ToPoint;
        //                    inpt3 = aSrcPt;
        //                    if (inPt1 != null && inpt2 != null & inpt3 != null)
        //                    {
        //                        //否则，三点构成一个面
        //                        IPointCollection myPCol = new PolygonClass();
        //                        myPCol.AddPoint(inPt1);
        //                        myPCol.AddPoint(inpt2);
        //                        myPCol.AddPoint(inpt3);
        //                        //把IPointCollection转换为面
        //                        IPolygon newPolygon = myPCol as IPolygon;
        //                        newPolygon.SimplifyPreserveFromTo();
                               
        //                        //如果这个 面与临街图版交与一个面，也是不合理的
        //                        ITopologicalOperator pTmpTop = newPolygon as ITopologicalOperator;
        //                        if (pTmpTop.Intersect(aTouchedFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension).IsEmpty)
        //                        {
        //                            newSmallPolygon = newPolygon;
        //                        }

        //                    }
                            

        //                    break;

        //                }
        //                if (newSmallPolygon != null)
        //                    break;
        //            }
        //            #endregion

        //            if (newSmallPolygon != null)
        //            { //把这个裂隙 合并到 原始图斑里面
        //                newSmallPolygon.SnapToSpatialReference();
        //                ITopologicalOperator ptop = aSrcDltb.ShapeCopy as ITopologicalOperator;
        //                aSrcDltb.Shape = ptop.Union(newSmallPolygon);
        //                ptop.Simplify();

        //                aSrcDltb.Store();
        //            }
                                       
        //        }
        //    }
        //}


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string layerName = this.cmbLayers.Text.Trim();
            if (layerName == "")
                return;
            layerName = OtherHelper.GetLeftName(layerName);

            //各种容差值
            double threshold = (double)spinEdit1.Value;
            double lengthHold = (double)spenEdtLen.Value;
            double mjHold = (double)spinEditMj.Value;

            IFeatureClass pFC = (this.currWS as IFeatureWorkspace).OpenFeatureClass(layerName);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍后...", "正在处理裂隙，请稍等...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                int icount = 0;
                if (this.radioGroup1.SelectedIndex == 1)
                {
                    //当前范围要素                   
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = (this.currMap as IActiveView).Extent;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor pCursor = pFC.Search(pSF, false);
                    try
                    {
                        IFeature aSrcDltb = null;
                        while ((aSrcDltb = pCursor.NextFeature()) != null)
                        {
                            if (aSrcDltb.OID == 142)
                            {

                            }

                            wait.SetCaption("正在处理要素" + aSrcDltb.OID);
                            DoATB(aSrcDltb, threshold, lengthHold,mjHold);   
                            
                            //紧接着要处理压盖
                            DoIntersec(pFC, aSrcDltb);
                            icount++;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        OtherHelper.ReleaseComObject(pCursor);
                    }
                }
                else if (this.radioGroup1.SelectedIndex == 0)
                {
                    //选中要素
                    IGeoFeatureLayer curLyr=LayerHelper.QueryLayerByModelName(this.currMap,layerName);
                    ArrayList arr= LayerHelper.GetSelectedFeature(this.currMap, curLyr, esriGeometryType.esriGeometryPolygon);
                    foreach (IFeature aTb in arr)
                    {
                        wait.SetCaption("正在处理要素" + aTb.OID);
                        DoATB(aTb, threshold, lengthHold,mjHold);

                        //紧接着要处理压盖
                        DoIntersec(pFC, aTb);

                        icount++;
                    }
                }
                
                //Excute(layerName, distance);

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("mergedosmall2");
                wait.Close();
                MessageBox.Show("执行完毕！共处理要素"+icount+"个。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                (this.currMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewAll, null, (this.currMap as IActiveView).Extent);
                
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }




            

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
