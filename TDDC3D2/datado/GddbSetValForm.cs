using System;
using System.Windows.Forms;
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
    public partial class GddbSetValForm : Form
    {
        public GddbSetValForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;
        IFeatureLayer dltbLayer = null;
        IFeatureLayer pdtLayer = null;
        private void GddbSetValForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbGDDBLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbDLTBLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            int dltbIdx = 0; int pdtidx = 0;
            for (int i = 0; i < this.cmbGDDBLayer.Properties.Items.Count; i++)
            {
                string str = this.cmbGDDBLayer.Properties.Items[i].ToString();
                if (str.ToUpper().Trim().StartsWith("GDDB"))
                {
                    pdtidx = i;
                    break;
                }
            }
            this.cmbGDDBLayer.SelectedIndex = pdtidx;

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

        private string getGDDB(IGeometry dltbGeo, IFeatureLayer pdtLayer,string fieldName)
        {
            string gddb = "";

            double maxMj = 0;
            Dictionary<string, double> gds = new Dictionary<string, double>();
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
                        gddb = FeatureHelper.GetFeatureStringValue(aFeature, fieldName).Trim();  //从新赋值

                        //}
                        if (gds.ContainsKey(gddb))
                        {
                            gds[gddb] += maxMj;
                        }
                        else
                        {
                            gds.Add(gddb, maxMj);
                        }

                    }
                }
                catch (Exception cex)
                {
                }
            }
          
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (gds.Count > 0)
            {
                KeyValuePair<string, double> jg = gds.OrderByDescending(x => x.Value).First();
                gddb = jg.Key;
            }
            else
            {
                gddb = "";
            }

            return gddb;


        }
        private void DoAXzq(IFeature aXzq,string gddbField)
        {
            string xzqdm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");
            string xzqmc = FeatureHelper.GetFeatureStringValue(aXzq, "XZQMC");

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
                    string gdpdjb = this.getGDDB(aDltb.Shape, this.pdtLayer, gddbField);
                    if (gdpdjb.Trim() != "")
                    {
                        FeatureHelper.SetFeatureValue(aDltb, "GDDB", gdpdjb); 
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
                this.UpdateStatus(xzqmc + "处理失败。" + ex.Message);

            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.cmbDLTBLayer.Text.Trim() == "") return;
            if (this.cmbGDDBLayer.Text.Trim() == "") return;
            if (this.cmbField.Text.Trim() == "") return;


            dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbDLTBLayer.Text.Trim()));
            pdtLayer = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbGDDBLayer.Text.Trim()));
            if (dltbLayer == null || pdtLayer == null) return;

            IFeatureClass dltbClass = dltbLayer.FeatureClass;
            IFeatureClass pdtClass = pdtLayer.FeatureClass;
            string fieldName = OtherHelper.GetLeftName(this.cmbField.Text);

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

                DoAXzq(aFeature,fieldName);

            }

            MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
