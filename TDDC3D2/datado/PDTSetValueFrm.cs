using System;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.datado
{
    public partial class PDTSetValueFrm : Form
    {
        public PDTSetValueFrm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;
        IFeatureLayer dltbLayer = null;
        IFeatureLayer pdtLayer = null;


        private void PDTSetValueFrm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbPDTLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbDLTBLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            int dltbIdx = 0; int pdtidx = 0;
            for (int i = 0; i < this.cmbPDTLayer.Properties.Items.Count; i++)
            {
                string str = this.cmbPDTLayer.Properties.Items[i].ToString();
                if (str.ToUpper().Trim().StartsWith("PDT"))
                {
                    pdtidx = i;
                    break;
                }
            }
            this.cmbPDTLayer.SelectedIndex = pdtidx;

            for (int i = 0; i < this.cmbDLTBLayer.Properties.Items.Count; i++)
            {
                string str = this.cmbDLTBLayer.Properties.Items[i].ToString();
                if (str.ToUpper().Trim().StartsWith("DLTB"))
                {
                    dltbIdx = i;
                    break;
                }
            }
            this.cmbDLTBLayer.SelectedIndex = dltbIdx;

        }

        private void UpdateStatus(string txt)
        {
            memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + txt;
            Application.DoEvents();
        }

        private string getPdjb(IGeometry dltbGeo, IFeatureClass pdtClass)
        {
            string Gdpdjb = "";

            ITopologicalOperator pTop = dltbGeo as ITopologicalOperator;
            pTop.Simplify();

            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = dltbGeo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            double maxMj = 0;
            IFeatureCursor pCursor = pdtClass.Search(pSF, true);
            IFeature aPdt = null;
            try
            {
                while ((aPdt = pCursor.NextFeature()) != null)
                {
                    IGeometry dPdtGeo = aPdt.Shape;
                    dPdtGeo.SnapToSpatialReference();

                    IGeometry interGeo = pTop.Intersect(dPdtGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (interGeo.IsEmpty)
                        continue;

                    //交于一个面
                    IArea area = interGeo as IArea;
                    if (maxMj < area.Area)
                    {
                        maxMj = area.Area;
                        Gdpdjb=FeatureHelper.GetFeatureStringValue(aPdt, "PDJB").Trim();  //从新赋值

                    }
                }
            }
            catch(Exception ex)
            { 
            
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Gdpdjb == "")
            {
            };

            return Gdpdjb;


        }


        private string getPdjb2(IGeometry dltbGeo, IFeatureLayer pdtLayer)
        {
            string Gdpdjb = "";

            double maxMj = 0;
            Dictionary<string, double> pd = new Dictionary<string, double>();
            ITopologicalOperator pTop = dltbGeo as ITopologicalOperator;
            pTop.Simplify();

            IIdentify idpdt = pdtLayer as IIdentify;
            IArray arPdt = idpdt.Identify(dltbGeo);
            if (arPdt != null)
            {
                try
                {
                    for (int i = 0; i < arPdt.Count; i++)
                    {
                        IFeatureIdentifyObj obj = arPdt.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                        IFeature aFeature = aRow.Row as IFeature;

                        IGeometry interGeo = pTop.Intersect(aFeature.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        if (interGeo.IsEmpty)
                            continue;

                        //交于一个面
                        IArea area = interGeo as IArea;
                        //if (maxMj < area.Area)
                        //{
                        maxMj = area.Area;
                        Gdpdjb = FeatureHelper.GetFeatureStringValue(aFeature, "PDJB").Trim();  //从新赋值

                        //}
                        if (pd.ContainsKey(Gdpdjb))
                        {
                            pd[Gdpdjb] += maxMj;
                        }
                        else
                        {
                            pd.Add(Gdpdjb, maxMj);
                        }
                    }
                }
                catch (Exception cex)
                {
                }
            }
            //ISpatialFilter pSF = new SpatialFilterClass();
            //pSF.Geometry = dltbGeo;
            //pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //double maxMj = 0;
            //IFeatureCursor pCursor = pdtClass.Search(pSF, true);
            //IFeature aPdt = null;
            //try
            //{
            //    while ((aPdt = pCursor.NextFeature()) != null)
            //    {
            //        IGeometry dPdtGeo = aPdt.Shape;
            //        dPdtGeo.SnapToSpatialReference();

            //        IGeometry interGeo = pTop.Intersect(dPdtGeo, esriGeometryDimension.esriGeometry2Dimension);
            //        if (interGeo.IsEmpty)
            //            continue;

            //        //交于一个面
            //        IArea area = interGeo as IArea;
            //        if (maxMj < area.Area)
            //        {
            //            maxMj = area.Area;
            //            Gdpdjb = FeatureHelper.GetFeatureStringValue(aPdt, "PDJB").Trim();  //从新赋值

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
            //}

            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (pd.Count > 0)
            {
                KeyValuePair<string,double> jg = pd.OrderByDescending(x => x.Value).First();
                Gdpdjb = jg.Key;
            }
            else
            {
                Gdpdjb = "";
            }

            return Gdpdjb;


        }

        

        private void DoAXzq(IFeature  aXzq)
        {
            string xzqdm=FeatureHelper.GetFeatureStringValue(aXzq,"XZQDM");
            string xzqmc=FeatureHelper.GetFeatureStringValue(aXzq,"XZQMC");

            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = aXzq.Shape;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            pSF.WhereClause = "DLBM in ('0101','0102','0103')";
            

            IWorkspaceEdit pWSEdit = this.currWS as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();
            try
            {

                IFeatureCursor pDltbCursor = this.dltbLayer.FeatureClass.Update(pSF as IQueryFilter, true);
                IFeature aDltb = null;
                while ((aDltb = pDltbCursor.NextFeature()) != null)
                {
                    string gdpdjb = this.getPdjb2(aDltb.Shape, this.pdtLayer);
                    if (gdpdjb.Trim() != "")
                    {
                        FeatureHelper.SetFeatureValue(aDltb, "GDPDJB", gdpdjb); //坡度级别
                        pDltbCursor.UpdateFeature(aDltb);

                    }
                }

                OtherHelper.ReleaseComObject(pDltbCursor);


                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
                this.UpdateStatus(xzqmc + "(" + xzqdm + ")处理完毕...");
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
                this.UpdateStatus(xzqmc + "处理失败。"+ex.Message);
                
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.cmbDLTBLayer.Text.Trim() == "") return;
            if (this.cmbPDTLayer.Text.Trim() == "") return;

            dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbDLTBLayer.Text.Trim()));
            pdtLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbPDTLayer.Text.Trim()));
            if (dltbLayer == null || pdtLayer == null) return;

            IFeatureClass dltbClass = dltbLayer.FeatureClass;
            IFeatureClass pdtClass = pdtLayer.FeatureClass;

            IFeatureLayer xzqLayer = LayerHelper.QueryLayerByModelName(this.currMap, "XZQ");
            if (xzqLayer == null)
            {
                MessageBox.Show("请首先加载XZQ图层！");
                return;
            }

            IFeatureClass pXzqClass = xzqLayer.FeatureClass;
            IIdentify idXzq = xzqLayer as IIdentify;
            IArray arrayXzq = idXzq.Identify((pXzqClass as IGeoDataset).Extent);
            if (arrayXzq == null)
            {
                MessageBox.Show("行政区没有数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.UpdateStatus("开始处理...");
            for (int i = 0; i < arrayXzq.Count; i++)
            {
                IFeatureIdentifyObj obj = arrayXzq.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                IFeature aFeature = aRow.Row as IFeature;

                DoAXzq(aFeature);

            }

            MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
