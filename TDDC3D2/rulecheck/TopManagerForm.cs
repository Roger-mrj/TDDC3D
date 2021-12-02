using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

using RuleCheck;
namespace TDDC3D.datado
{
    public partial class TopManagerForm : Form
    {
        public TopManagerForm()
        {
            InitializeComponent();
        }

        public IWorkspace currWs = null;
        public IMapControl3 mapctl = null;
        private void TopManagerForm_Load(object sender, EventArgs e)
        {
            this.lstTops.Items.Clear();
            if (this.currWs != null)
            {
                //获取所有拓扑
                IFeatureDataset pFeaDS = (currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                ITopologyContainer pTopC = pFeaDS as ITopologyContainer;
                for (int i = 0; i < pTopC.TopologyCount; i++)
                {
                    ITopology ptop = pTopC.get_Topology(i);

                    IDataset pDS = ptop as IDataset;
                    if (pDS != null)
                    {
                        this.lstTops.Items.Add(pDS.Name.ToUpper());
                    }


                }
            }


            this.dtError = buildTable();


            
        }

        private void cmdCreate_Click(object sender, EventArgs e)
        {
            if (this.lstTops.SelectedItem == null) return;
            string sName = this.lstTops.SelectedItem.ToString();
            try
            {
                IFeatureDataset pFeaDS = (currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                ITopologyContainer pTopC = pFeaDS as ITopologyContainer;

                ITopology ptop = pTopC.get_TopologyByName(sName.ToUpper());
                if (ptop != null)
                {
                    IDataset pDS = ptop as IDataset;
                    if (pDS != null)
                    {
                        this.lstTops.Items.Remove(this.lstTops.SelectedItem);
                        pDS.Delete();
                        this.lstTops.Items.Remove(this.lstTops.SelectedItem);
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        DataTable dtError = null;
        private DataTable buildTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("RULEID", typeof(string));
            dt.Columns.Add(dc);  //规则编号
            dc = new DataColumn("ERRINFO", typeof(string));
            dt.Columns.Add(dc); //错误描述
            dc = new DataColumn("ERRTYPE", typeof(string)); //错误类型
            dt.Columns.Add(dc);
            dc = new DataColumn("ERRLAYER", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("ERRBSM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("ERROID", typeof(string));
            dt.Columns.Add(dc);

            return dt;
        }

        private void GetTopErrors(ITopology topology, string ruleId, string errorLevel)
        {

            IFeatureDataset tdghDataset = topology.FeatureDataset;  //获取topology所在数据集，为了后面根据classid找到该class
            IFeatureClassContainer tdghClassContainer = tdghDataset as IFeatureClassContainer;

            //获取拓扑错误

            this.dtError.Rows.Clear();

            IErrorFeatureContainer errorFeatureContainer = (IErrorFeatureContainer)topology;
            IGeoDataset geoDataset = (IGeoDataset)topology;
            ISpatialReference spatialReference = geoDataset.SpatialReference;

            IEnumTopologyErrorFeature enumTopologyErrorFeature = errorFeatureContainer.get_ErrorFeatures(spatialReference,
                null, geoDataset.Extent, true, false);

            int iCount = 0;
            // Get the first error feature (if any exist) and display its properties.
            ITopologyErrorFeature topologyErrorFeature = enumTopologyErrorFeature.Next();
            while (topologyErrorFeature != null)
            {
                try
                {
                    DataRow dr = dtError.NewRow();
                    dr["RULEID"] = ruleId;
                    if (topologyErrorFeature.TopologyRule != null)
                    {
                        dr["ERRINFO"] = (topologyErrorFeature.TopologyRule as ITopologyRule).Name;
                    }
                    
                    IFeatureClass originClass = tdghClassContainer.get_ClassByID(topologyErrorFeature.OriginClassID);

                    dr["ERRLAYER"] = (originClass as IDataset).Name;
                    dr["ERRTYPE"] = "图形错误";
                    if (topologyErrorFeature.OriginOID > 0)
                    {

                        IFeature oringinFea = originClass.GetFeature(topologyErrorFeature.OriginOID);
                        string sBsm = FeatureHelper.GetFeatureStringValue(oringinFea, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);

                        dr["ERROID"] = oringinFea.OID;
                        dr["ERRBSM"] = iBsm.ToString();
                        dtError.Rows.Add(dr);

                        iCount++;
                    }
                }
                catch (Exception ex)
                {
                }
                //else
                //{
                //    IWorkspace pWS = tdghDataset.Workspace;
                //    IWorkspaceEdit pWSE = pWS as IWorkspaceEdit;
                //    pWSE.StartEditing(false);
                //    IFeatureBuffer pFB = originClass.CreateFeatureBuffer();
                //    IFeatureCursor pFC = originClass.Insert(false);
                //    IFeature pF = pFB as IFeature;
                    
                //    pFB = pF as IFeatureBuffer;
                //    pFC.InsertFeature(pFB);
                //    pFC.Flush();
                //    pWSE.StopEditing(true);
                //    dr["ERROID"] = pF.OID;
                //    string sBsm = FeatureHelper.GetFeatureStringValue(pF, "BSM");
                //    int iBsm = -1;
                //    int.TryParse(sBsm, out iBsm);
                //    dr["ERRBSM"] = iBsm.ToString();
                //}
                
                topologyErrorFeature = enumTopologyErrorFeature.Next();
            }

           


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.lstTops.SelectedItem == null) return;
            string sName = this.lstTops.SelectedItem.ToString();
            IFeatureDataset pFeaDS = (currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            ITopologyContainer pTopC = pFeaDS as ITopologyContainer;

            ITopology ptop = pTopC.get_TopologyByName(sName.ToUpper());
            if (ptop == null)
            {
                MessageBox.Show("找不到该拓扑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ptop.ValidateTopology((pFeaDS as IGeoDataset).Extent);
                GetTopErrors(ptop, sName, "1");
                this.gridControlError.DataSource = null;
                this.gridControlError.DataSource = this.dtError;
                this.gridControlError.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Default;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gridControlError_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewError.SelectedRowsCount == 0)
                return;
            string layerName = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "ERRLAYER").ToString();
            string oid = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "ERROID").ToString();
            int ioid = Convert.ToInt32(oid);
            //定位
            IFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(this.mapctl.ActiveView.FocusMap, layerName);
            if (pLyr == null)
            {
                MessageBox.Show("没加载该图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureSelection pSelection = pLyr as IFeatureSelection;
            pSelection.Clear();

            if (pLyr == null) return;
            IFeatureClass pClass = pLyr.FeatureClass;
            if (pClass == null) return;
            if (ioid < 0) return;
            IFeature pFeature = pClass.GetFeature(ioid);
            if (pFeature != null)
            {
                IGeometry pGeo = pFeature.ShapeCopy;
                IEnvelope env = pGeo.Envelope;
                env.Expand(1.5, 1.5, true);
                this.mapctl.ActiveView.Extent = env;

                pSelection.Add(pFeature);

                this.mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapctl.ActiveView.Extent.Envelope);
                this.mapctl.ActiveView.ScreenDisplay.UpdateWindow();
                if (pGeo != null)
                {
                    this.mapctl.FlashShape(pGeo, 3, 300, null);
                }
            
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //添加拓扑层到地图中
            if (this.lstTops.SelectedItem == null) return;
            string sName = this.lstTops.SelectedItem.ToString();
            IFeatureDataset pFeaDS = (currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            ITopologyContainer pTopC = pFeaDS as ITopologyContainer;

            ITopology ptop = pTopC.get_TopologyByName(sName.ToUpper());
            if (ptop == null)
            {
                MessageBox.Show("找不到该拓扑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                ITopologyLayer topLayer = new TopologyLayerClass();
                topLayer.Topology = ptop;

                
                (topLayer as ILayer).Name = (ptop as IDataset).Name;
                this.mapctl.AddLayer(topLayer as ILayer, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
