using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;

using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;

namespace TDDC3D.datado
{
    public partial class IntersecPolygonForm : Form
    {
        public IntersecPolygonForm()
        {
            InitializeComponent();
        }

        
        private string pathName = "";
        public IMap currMap;
        public IWorkspace currWs;

        private IFeatureDataset pFeatureDataset = null;

        private void IntersecPolygonForm_Load(object sender, EventArgs e)
        {
            pathName = this.currWs.PathName;
            pathName += "\\" + RCIS.Global.AppParameters.DATASET_DEFAULT_NAME + "\\";
            pFeatureDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);

            LayerHelper.LoadLayer2Combox(this.cmbLayer1, this.currMap, true, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbLayer2, this.currMap, true, esriGeometryType.esriGeometryPolygon);
            
            if (this.cmbLayer1.Properties.Items.Count>0)
            {
                this.cmbLayer1.SelectedIndex =0;
                this.cmbLayer2.SelectedIndex = 0;
            }
            
        }

        private void cmbLayer1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtDestLayer.Text = OtherHelper.GetLeftName(this.cmbLayer1.Text.Trim()) + "_" + OtherHelper.GetLeftName(this.cmbLayer2.Text.Trim() )+ "_Intersect";
            
        }

        private void cmbLayer2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtDestLayer.Text = OtherHelper.GetLeftName(this.cmbLayer1.Text.Trim()) + "_" + OtherHelper.GetLeftName(this.cmbLayer2.Text.Trim()) + "_Intersect";

        }

        

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.txtDestLayer.Text.Trim() == "")
                return;
            if (this.cmbLayer1.Text.Trim() == "")
                return;
            if (this.cmbLayer2.Text.Trim() == "")
                return;
            if (this.cmbLayer1.Text.Trim().ToUpper().Equals(this.cmbLayer2.Text.Trim().ToUpper()))
            {
                MessageBox.Show("不能选择相同图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string targetClassName = this.txtDestLayer.Text.Trim();
            string outShpFileName = this.pathName + "\\" + this.txtDestLayer.Text.Trim();

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "正在执行叠加操作...");
            wait.Show();

            try
            {
                IFeatureClass inputFeatClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(OtherHelper.GetLeftName(  this.cmbLayer1.Text));
                IFeatureClass clipFeatClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(OtherHelper.GetLeftName(this.cmbLayer2.Text));
                IGpValueTableObject valTbl = new GpValueTableObjectClass();
                valTbl.SetColumns(2);
                object row = "";
                object rank = 1;

                row = inputFeatClass;
                valTbl.SetRow(0, ref row);
                valTbl.SetValue(0, 1, ref rank);

                row = clipFeatClass;
                valTbl.SetRow(1, ref row);
                rank = 2;
                valTbl.SetValue(1, 1, ref rank);
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Intersect intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
                intersect.in_features = valTbl;
                intersect.out_feature_class = outShpFileName;
                intersect.join_attributes = "NO_FID";
                intersect.output_type = "INPUT";

                gp.Execute(intersect, null);
                string s=GpToolHelper.ReturnMessages(gp);
                


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
            catch(Exception ex)
            {
                if (wait!=null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

            


        }
    }
}
