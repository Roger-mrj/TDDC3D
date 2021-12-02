using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Windows.Forms;
namespace TDDC3D.datado
{
    public partial class Features2PolygonForm : Form
    {
        public Features2PolygonForm()
        {
            InitializeComponent();
        }

        private string pathName = "";
        public IMap currMap;
        public IWorkspace currWs;

        private IFeatureDataset pFeatureDataset = null;

        private void Features2PolygonForm_Load(object sender, EventArgs e)
        {
            pathName = this.currWs.PathName;
            pathName += "\\" + RCIS.Global.AppParameters.DATASET_DEFAULT_NAME + "\\";
            pFeatureDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);

            LayerHelper.LoadLayer2Combox(this.cmbLayer1, this.currMap, true, esriGeometryType.esriGeometryPolyline);

            if (this.cmbLayer1.Properties.Items.Count > 0)
            {
                this.cmbLayer1.SelectedIndex = 0;
            }
        }

        private void cmbLayer1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtDestLayer.Text = OtherHelper.GetLeftName(this.cmbLayer1.Text.Trim()) + "_toPolygon";
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.txtDestLayer.Text.Trim() == "")
                return;
            if (this.cmbLayer1.Text.Trim() == "")
                return;
        
            string targetClassName = this.txtDestLayer.Text.Trim();
            string outShpFileName = this.pathName + "\\" + this.txtDestLayer.Text.Trim();

            //string inshpname = this.pathName + "\\" + OtherHelper.GetLeftName(this.cmbLayer1.Text);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "正在执行叠加操作...");
            wait.Show();

            try
            {
                IFeatureClass inputFeatClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(OtherHelper.GetLeftName(this.cmbLayer1.Text));
                IGpValueTableObject valTbl = new GpValueTableObjectClass();
                valTbl.SetColumns(2);
                object row = "";
                object rank = 1;

                row = inputFeatClass;//
                valTbl.SetRow(0, ref row);
                valTbl.SetValue(0, 1, ref rank);                              
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.FeatureToPolygon toPolygon = new ESRI.ArcGIS.DataManagementTools.FeatureToPolygon();
                toPolygon.in_features = valTbl;
                toPolygon.out_feature_class = outShpFileName;
              
                gp.Execute(toPolygon, null);
                string s = GpToolHelper.ReturnMessages(gp);
                
                wait.Close();
                if ((s.ToUpper().Contains("ERROR")) || (s.Contains("失败")))
                {
                    MessageBox.Show("执行失败。" + s, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("执行成功。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try
                    {
                        IFeatureClass targetClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(targetClassName);
                        IFeatureLayer pFeaLyr = new FeatureLayerClass();
                        pFeaLyr.FeatureClass = targetClass;
                        pFeaLyr.Name = targetClassName;
                        this.currMap.AddLayer(pFeaLyr);
                        this.currMap.MoveLayer(pFeaLyr, 0);
                    }
                    catch { }

                }

            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

        }
    }
}
