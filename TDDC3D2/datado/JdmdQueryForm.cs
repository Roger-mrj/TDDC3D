using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using RCIS.GISCommon;
using RCIS.Utility;


namespace TDDC3D.datado
{
    public partial class JdmdQueryForm : Form
    {
        public JdmdQueryForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IMapControl2 currMapControl = null;
        public IWorkspace currWs = null;
        private IWorkspaceEdit currWorkspaceEdit = null;


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }


        

        public double MinMd
        {
            get {
                double d = 1;
                double.TryParse(this.txtMinPjmd.Text, out d);
                return d; }
        }       

        public double MaxMd
        {
            get
            {
                double d = 1;
                double.TryParse(this.txtMaxPjmd.Text, out d);
                return d;
            }
        }

        ///// <summary>
        ///// 单边最大长度
        ///// </summary>
        //public double SingleMaxLen
        //{
        //    get
        //    {
        //        double d = 1;
        //        double.TryParse(this.txtSingleLen.Text, out d);
        //        return d;
        //    }
        //}

        private IFeatureLayer currFeatureLayer = null;
       
        private void JdmdQueryForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayer1, this.currMap, true, esriGeometryType.esriGeometryPolygon);

            int idx = 0;
            for (int i = 0; i < this.cmbLayer1.Properties.Items.Count; i++)
            {
                string txt=this.cmbLayer1.Properties.Items[i].ToString();
                if (OtherHelper.GetLeftName(txt) == "DLTB")
                {
                    idx = i;
                    break;
                }

            }
            this.cmbLayer1.SelectedIndex = idx;

            this.currWorkspaceEdit = currWs as IWorkspaceEdit;

        }

        

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer1.Text.Trim() == "")
            {
                return;
            }
            string layerName = OtherHelper.GetLeftName(this.cmbLayer1.Text);
            currFeatureLayer = LayerHelper.QueryLayerByModelName(this.currMap, layerName);
            IIdentify identify = currFeatureLayer as IIdentify;
            IArray arDltbs = identify.Identify((currFeatureLayer.FeatureClass as IGeoDataset).Extent);
            if (arDltbs == null)
                return;
            this.lstIds.Items.Clear();

            for (int i = 0; i < arDltbs.Count; i++)
            {
                IFeatureIdentifyObj obj = arDltbs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                IFeature aFeature = aRow.Row as IFeature;

                //平均密度
                IGeometry aGeometry = aFeature.Shape;
                IPointCollection ptCol = aGeometry as IPointCollection;
                ITopologicalOperator pTopBoundy = aGeometry as ITopologicalOperator;
                IPolyline pLine = pTopBoundy.Boundary as IPolyline;
                double dLen = pLine.Length;
                double md = dLen / ptCol.PointCount;
                if (md < MinMd || md > MaxMd)
                {
                    this.lstIds.Items.Add(aFeature.OID);
                }
                
            }

            MessageBox.Show("检查完毕，共找到" + lstIds.Items.Count + "个要素!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        ArrayList arAllpoints = new ArrayList();

      

        private void lstProperties_DoubleClick(object sender, EventArgs e)
        {
            if (this.currMapControl == null)
                return;
            if (lstProperties.SelectedItems[0] == null)
                return;
            string sId = lstProperties.SelectedItems[0].Text.Trim();
            int id = Convert.ToInt32(sId);

            IPoint currPt = arAllpoints[id] as IPoint;
            currMapControl.FlashShape(currPt as IGeometry);
        }

        private void lstIds_DoubleClick(object sender, EventArgs e)
        {
            if (this.lstIds.Items.Count == 0) return;
            if (this.currFeatureLayer == null) return;

            //找到这个要素
            int id = Convert.ToInt32(lstIds.SelectedItem.ToString());
            IFeature currFea = this.currFeatureLayer.FeatureClass.GetFeature(id);
            IGeometry currGeo = currFea.ShapeCopy as IGeometry;

            #region 加载点
            arAllpoints.Clear();
            if (currGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                IGeometryCollection pGC = currGeo as IGeometryCollection;
                for (int i = 0; i < pGC.GeometryCount; i++)
                {
                    IRing curRing = pGC.get_Geometry(i) as IRing;
                    IPointCollection curPC = curRing as IPointCollection;
                    for (int j = 0; j < curPC.PointCount; j++)
                    {
                        IPoint aPt = curPC.get_Point(j);
                        arAllpoints.Add(aPt);
                    }
                }

            }
            else if (currGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                IPointCollection points = currGeo as IPointCollection;
                for (int i = 0; i < points.PointCount; i++)
                {
                    IPoint aPT = points.get_Point(i);
                    arAllpoints.Add(aPT);

                }
            }


            lstProperties.Items.Clear();

            int index = 0;
            foreach (IPoint apt in arAllpoints)
            {

                ListViewItem newItem = lstProperties.Items.Add(index.ToString());
                newItem.SubItems.Add(apt.X.ToString());
                newItem.SubItems.Add(apt.Y.ToString());
                index++;

            }
            #endregion 

            //将
            IEnvelope env = currGeo.Envelope;
            env.Expand(2, 2, true);
            this.currMapControl.ActiveView.Extent = env;
            this.currMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.currMapControl.ActiveView.Extent);
            this.currMapControl.ActiveView.ScreenDisplay.UpdateWindow();
            if (currGeo != null)
            {
                this.currMapControl.FlashShape(currGeo, 3, 300, null);
            }




            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.lstIds.Items.Count == 0) return;
            //提取相关要素到shp
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "SHP文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string layerName = OtherHelper.GetLeftName(this.cmbLayer1.Text);
                currFeatureLayer = LayerHelper.QueryLayerByModelName(this.currMap, layerName);
                IFeatureClass pFC = currFeatureLayer.FeatureClass;
                ArrayList arFeatures = new ArrayList();
                for (int k = 0; k < lstIds.Items.Count; k++)
                {
                    int firstId = (int)lstIds.Items[k];
                    IFeature aFeature = pFC.GetFeature(firstId);
                    arFeatures.Add(aFeature);
                }
                IGeometry fwGeo = GeometryHelper.UnionPolygon(arFeatures);
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = fwGeo;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;


                IDataset inDataSet = pFC as IDataset;
                string inClassName = inDataSet.Name;
                IWorkspace inWorkspace = inDataSet.Workspace;
                string shapefileName = dlg.FileName;
                string folderPath = System.IO.Path.GetDirectoryName(shapefileName);
                string shortName = System.IO.Path.GetFileNameWithoutExtension(shapefileName); //shapefileName.Substring(idxStart + 1, idxEnd - idxStart - 1);
                IWorkspaceFactory targetWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                IWorkspace targetWorkspace = targetWorkspaceFactory.OpenFromFile(folderPath, 0);
                EsriDatabaseHelper.ConvertFeatureClass(inWorkspace, targetWorkspace, inClassName, shortName, pSF as IQueryFilter);

                this.Cursor = Cursors.Default;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        //private double calLen(IPoint pt1, IPoint pt2)
        //{
        //    double distance1 = Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y));
        //    return distance1;
        //}


       
        //抽稀处理
        private IGeometry Process(IGeometry pGeom,double distance)
        {

            
            if (pGeom == null || pGeom.IsEmpty) return pGeom;
            IGeometry aResult = (pGeom as IClone).Clone()
                                as IGeometry;

            ISpatialReference pSR = pGeom.SpatialReference;

            #region 只有线和面才处理
            IGeometryCollection aGC = aResult as IGeometryCollection;

            for (int gi = 0; gi < aGC.GeometryCount; gi++)
            {
                IGeometry aPart = aGC.get_Geometry(gi);  //每一部分
                ISegmentCollection aSegCol = aPart as ISegmentCollection;
                for (int ai = 0; ai < aSegCol.SegmentCount; ai++)
                {
                    //每一段
                    ISegment iSeg = aSegCol.get_Segment(ai);
                    if (iSeg is ILine)
                    {
                        int aj = ai + 1;
                        while (aj < aSegCol.SegmentCount)
                        {
                            ISegment jSeg = aSegCol.get_Segment(aj);
                            if (jSeg is ILine)
                            {
                                //如果再同一条线上，不会发生图形变化，不删除
                                if (GeometryHelper.OnLine180(iSeg.FromPoint
                                    , iSeg.ToPoint, jSeg.ToPoint, pSR))
                                {
                                    aj++;
                                    continue;
                                }
                                
                                if (
                                    (GeometryHelper.OnLine(iSeg.FromPoint
                                    , iSeg.ToPoint, jSeg.ToPoint, pSR) ||
                                    (iSeg.Length < distance))  )
                                {
                                    iSeg.ToPoint = jSeg.ToPoint;
                                    aSegCol.RemoveSegments(aj, 1, false);

                                }
                                else break;
                            }
                            else break;
                        }
                    }
                }
            }
            #endregion
            return aResult;

        }

        /// <summary>
        /// 多部分打散
        /// </summary>
        /// <param name="aGeo"></param>
        private List<IGeometry> ClipMultiGeo(IGeometry  aGeo)
        {

            List<IGeometry> lst = new List<IGeometry>();
            if (aGeo.IsEmpty) return lst;
           
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
                polyGonGeo.Project(aGeo.SpatialReference);

                IGeometry newGeo =  polyGonGeo as IGeometry;
                lst.Add(newGeo);
                
            }

            return lst;

        }

        private bool StartEditOp()
        {
            bool retVal = false;

            if (!this.currWorkspaceEdit.IsBeingEdited())
            {
                // Not being edited so start here
                currWorkspaceEdit.StartEditing(false);
                retVal = true;
            }

            // Start operation
            currWorkspaceEdit.StartEditOperation();
            return retVal;
        }

        /// <summary>
        /// Stops the edit operation.
        /// </summary>
        /// <remarks>
        /// This method stops an edit operation started with a call to 
        /// <see cref="StartEditOp"/>. If the weStartedEditing parameter is true, this
        /// method will also end the edit session.
        /// </remarks>
        /// <param name="weStartedEditing">if set to <c>true</c> [we started editing].</param>
        private void StopEditOp(bool weStartedEditing)
        {
            currWorkspaceEdit.StopEditOperation();
            if (weStartedEditing)
            {
                // We started the edit session so stop it here
                currWorkspaceEdit.StopEditing(true);
            }
        }

        private void AbortEditOp(bool weStartedEditing)
        {
            currWorkspaceEdit.AbortEditOperation();
            if (weStartedEditing)
            {
                // We started the edit session so stop it here
                currWorkspaceEdit.StopEditing(false);
            }
        }

        /// <summary>
        /// 获取交于一个面的
        /// </summary>
        /// <param name="targetClass"></param>
        /// <param name="pGeo"></param>
        /// <param name="rel"></param>
        /// <returns></returns>
        public List<IFeature> getFeaturesByGeo(IFeatureClass targetClass, IFeature inFea)
        {
            List<IFeature> lst = new List<IFeature>();
            IGeometry pGeo = inFea.ShapeCopy;
            ITopologicalOperator pTop = pGeo as ITopologicalOperator;
            pTop.Simplify();
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {

                ISpatialFilter pSR = new SpatialFilterClass();
                pGeo.Project((targetClass as IGeoDataset).SpatialReference);
                pSR.Geometry = pGeo;
                pSR.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = targetClass.Search(pSR as IQueryFilter, false);
                release.ManageLifetime(pCursor);
                IFeature aFea =null;
                
                while ((aFea=pCursor.NextFeature()) != null)
                {
                    if (aFea.OID == inFea.OID)
                        continue;
                    //IGeometry interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    //if (interGeo.IsEmpty)
                    //    continue;
                    lst.Add(aFea);
                    
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSR);


            }
            return lst;
        }


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.lstIds.Items.Count == 0) return;
            if (this.currFeatureLayer == null) return;

            int selIdx = lstIds.SelectedIndex;
            if (selIdx < 0) return;

            string layerName = OtherHelper.GetLeftName(this.cmbLayer1.Text);
            currFeatureLayer = LayerHelper.QueryLayerByModelName(this.currMap, layerName);
            IFeatureClass pFC=currFeatureLayer.FeatureClass;


            IFeature oldFeature = pFC.GetFeature((int)this.lstIds.Items[selIdx]);//得到要抽稀的要素
            IGeometry oldGeo = oldFeature.ShapeCopy; //旧图形
            if ((oldGeo as IPointCollection).PointCount < 6)
            {
                MessageBox.Show("结点数太少！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IGeometry resultGeo = Process(oldFeature.ShapeCopy, 0.5);//得到新图形
            
            if (resultGeo.IsEmpty)
            {
                MessageBox.Show("失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ITopologicalOperator pTop1 = oldGeo as ITopologicalOperator;
            IGeometry unionGeo = pTop1.Difference(resultGeo); //旧的减去新的，是需要跟周边合并的，



            List<IGeometry> lstSmallGeo = this.ClipMultiGeo(unionGeo);
            if (lstSmallGeo.Count == 0)
            {
                MessageBox.Show("打散后失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

                

            this.Cursor = Cursors.WaitCursor;

            bool weStartedEditing = this.StartEditOp();
            try
            {
                oldFeature.Shape = resultGeo;
                oldFeature.Store();

                

                //其他要素合并
                foreach (IGeometry aSmallGeo in lstSmallGeo)
                {
                    //与周边大的合并
                    IFeature maxFea = edit.SmallTbDoHelper.getMaxFeature(aSmallGeo, pFC, oldFeature.OID);
                    if (maxFea == null)
                    {
                        //创建新的要素
                        IFeature newFeature = pFC.CreateFeature();                        
                        FeatureHelper.CopyFeature(oldFeature, newFeature);                        
                        newFeature.Shape = aSmallGeo;
                        newFeature.Store();

                    }
                    else
                    {
                        //合并

                        ITopologicalOperator smallTop = aSmallGeo as ITopologicalOperator;
                        smallTop.Simplify();
                        IGeometry newGeo= smallTop.Union(maxFea.ShapeCopy);                        
                        newGeo.SnapToSpatialReference();

                        maxFea.Shape = newGeo;
                        maxFea.Store();
                    }

                }


                //新生成的 图形信息 压盖道其他要素的，把其他要素切掉
                List<IFeature> intersectFeatures = this.getFeaturesByGeo(pFC, oldFeature);
                foreach (IFeature aIntersect in intersectFeatures)
                {

                    IGeometry interSecGeo = aIntersect.Shape;
                    ITopologicalOperator pTmpTop = interSecGeo as ITopologicalOperator;
                    pTmpTop.Simplify();
                    IGeometry newGeo = pTmpTop.Difference(oldFeature.Shape);
                    if (newGeo.IsEmpty)
                        continue;
                    aIntersect.Shape = newGeo;
                    aIntersect.Store();

                    
                }


                this.StopEditOp(weStartedEditing);

                this.Cursor = Cursors.Default;

                MessageBox.Show("抽稀完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.currMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography| esriViewDrawPhase.esriViewGeoSelection, null, this.currMapControl.ActiveView.Extent);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.AbortEditOp(weStartedEditing);
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

            
            
        }

        
    }
}
