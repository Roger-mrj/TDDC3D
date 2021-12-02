using System;
using System.Collections.Generic;
using System.Collections;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.edit
{
    public partial class XzdwTbhPreBgDoSmallForm : Form
    {
        public XzdwTbhPreBgDoSmallForm()
        {
            InitializeComponent();
        }

        
        private void XzdwTbhPreBgDoSmallForm_Load(object sender, EventArgs e)
        {
            this.cmbLayers.Properties.Items.Clear();

            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap, esriGeometryType.esriGeometryPolygon);

            List<string> sAllDlbm = sys.YWCommonHelper.getAllDlbm();            
            //加载所有地类
            foreach (string adl in sAllDlbm)
            {
                int idx= this.cmbExceptedDlbms.Properties.Items.Add(adl);
                string dl=OtherHelper.GetLeftName(adl);
                if (sys.YWCommonHelper.isJsyd(dl) || dl == "1006" || dl == "1107")
                {

                    this.cmbExceptedDlbms.Properties.Items[idx].CheckState = CheckState.Unchecked;
                }
                else
                {
                    this.cmbExceptedDlbms.Properties.Items[idx].CheckState = CheckState.Checked;
                }
            }

        }
        public IMap currMap = null;
        public IWorkspace currWS = null;


        private List<IGeometry>  MultiPartDo(IGeometry aGeo)
        {
            List<IGeometry> lst = new List<IGeometry>();

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
                lst.Add(polyGonGeo);
            }

            return lst;

        }

        private List<IGeometry> getIntersectSmallGeoByTb(IFeatureClass dltbClass, IGeometry intersecGeo, double minAreaMj)
        {
            object missing = Type.Missing;   
            List<IGeometry> lstGeo = new List<IGeometry>();
            if (dltbClass == null)
                return lstGeo;

            //地类图斑排除地类


            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = intersecGeo;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            ITopologicalOperator pTop = intersecGeo as ITopologicalOperator;
            IFeatureCursor featureCursor = dltbClass.Search(spatialFilter, false);
            IFeature aFea = null;
            try
            {
                while ((aFea=featureCursor.NextFeature())!=null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                    if (this.ExceptedDlbm.Contains(dlbm))
                    {
                        continue;
                    }

                    IGeometry interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (!interGeo.IsEmpty)
                    {

                        IGeometryCollection pGeoCol = interGeo as IGeometryCollection;
                        if (pGeoCol.GeometryCount > 1)
                        {
                            List<IGeometry> resultGeo = this.MultiPartDo(interGeo);//打散
                            foreach (IGeometry aGeo in resultGeo)
                            {
                                if ((aGeo as IArea).Area < minAreaMj)
                                {
                                    lstGeo.Add(aGeo);
                                }
                            }
                        }
                        else
                        {
                            if ((interGeo as IArea).Area < minAreaMj)
                            {
                                lstGeo.Add(interGeo);
                            }
                        }

                        
                        
                    }

                    
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                ////垃圾回收  
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }

            return lstGeo;
        }


        private void   PreDo(IFeatureClass dltbClass, IFeature aTBFea, double dSmallTbmj)
        {
            //该图形与地类图斑相交后，所有图形
            List<IGeometry> lstSmallGeo = this.getIntersectSmallGeoByTb(dltbClass, aTBFea.Shape, dSmallTbmj);
            ITopologicalOperator pTopDiff = aTBFea.Shape as ITopologicalOperator;
            IGeometry restGeos = aTBFea.ShapeCopy;

            foreach (IGeometry aGeo in lstSmallGeo)
            {
                restGeos = pTopDiff.Difference(aGeo);
                if (restGeos.IsEmpty)
                    break;
                pTopDiff = restGeos as ITopologicalOperator;
            }

            if (!restGeos.IsEmpty)
            {
                aTBFea.Shape = restGeos;
                aTBFea.Store();
            }
            else
            {
                aTBFea.Delete();  //如果切没了，则 新增当前这个线物图斑 为空
            }
        }

        public List<string> ExceptedDlbm
        {
            get
            {
                List<string> lstExceptDlbm = new List<string>();
                for (int k = 0; k < this.cmbExceptedDlbms.Properties.Items.Count; k++)
                {
                    if (this.cmbExceptedDlbms.Properties.Items[k].CheckState == CheckState.Checked)
                    {
                        lstExceptDlbm.Add(OtherHelper.GetLeftName(this.cmbExceptedDlbms.Properties.Items[k].ToString()));
                    }
                }
                return lstExceptDlbm;
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string layerName = this.cmbLayers.Text.Trim();
            if (layerName == "")
                return;
            layerName = OtherHelper.GetLeftName(layerName);  //目标图层

           

            //各种容差值
            double mjHold = 0;
            double.TryParse(this.txtSmallMj.Text, out mjHold);
            IFeatureClass pFC = (this.currWS as IFeatureWorkspace).OpenFeatureClass(layerName);
            IFeatureClass dltbClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            
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
                           
                            wait.SetCaption("正在处理要素" + aSrcDltb.OID);
                            PreDo(dltbClass, aSrcDltb, mjHold);
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
                    IGeoFeatureLayer curLyr = LayerHelper.QueryLayerByModelName(this.currMap, layerName);
                    ArrayList arr = LayerHelper.GetSelectedFeature(this.currMap, curLyr, esriGeometryType.esriGeometryPolygon);
                    foreach (IFeature aTb in arr)
                    {
                        wait.SetCaption("正在处理要素" + aTb.OID);

                        PreDo(dltbClass, aTb, mjHold);
                        icount++;
                    }
                }

                //Excute(layerName, distance);

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("mergedosmall2");
                wait.Close();
                MessageBox.Show("执行完毕！共处理要素" + icount + "个。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
