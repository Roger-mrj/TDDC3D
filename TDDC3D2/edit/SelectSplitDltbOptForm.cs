using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
namespace TDDC3D.edit
{
    public partial class SelectSplitDltbOptForm : Form
    {
        public SelectSplitDltbOptForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IWorkspace currWS = null;
        List<IFeature> lstXZQ;
        private  string dltbLayerName = "";

        private void XzqSplitDltbOptForm_Load(object sender, EventArgs e)
        {
            if (currMap.SelectionCount > 0)
            {
                lstXZQ = new List<IFeature>();
                ISelection selection = currMap.FeatureSelection;
                IEnumFeatureSetup iEnumFeatureSetup = (IEnumFeatureSetup)selection;
                iEnumFeatureSetup.AllFields = true;
                IEnumFeature pEnumFeature = (IEnumFeature)iEnumFeatureSetup;
                pEnumFeature.Reset();
                IFeature pFeature = pEnumFeature.Next();
                while (pFeature != null)
                {
                    if (pFeature.ShapeCopy.GeometryType == esriGeometryType.esriGeometryPolygon) lstXZQ.Add(pFeature);
                    pFeature = pEnumFeature.Next();
                }
                labelControl1.Text = "当前有" + lstXZQ.Count.ToString() + "个选择的面";
                if (lstXZQ.Count == 0)
                {
                    MessageBox.Show("没有选择要素面。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("没有选择要素面。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            this.cmbDltb.Properties.Items.Clear();

            
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = this.currMap.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeaLyr = childLyr as IFeatureLayer;
                            IFeatureClass currClass = currFeaLyr.FeatureClass;
                            string clsName = (currClass as IDataset).Name.ToUpper();
                            if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            {
                                this.cmbDltb.Properties.Items.Add(clsName);
                            }
                        }
                    }
                }
                else if (currLyr is IFeatureLayer)
                {
                    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                    IFeatureClass currClass = currFeaLyr.FeatureClass;
                    string clsName = (currClass as IDataset).Name.ToUpper();
                    if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        this.cmbDltb.Properties.Items.Add(clsName);
                    }
                }

            
                
            }
            int idx2 = -1;

            for (int i = 0; i < this.cmbDltb.Properties.Items.Count; i++)
            {
                string name = this.cmbDltb.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLTB") && name.Length>4 )
                {
                    idx2 = i;
                    break;
                }
            }
            this.cmbDltb.SelectedIndex = idx2;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbDltb.Text.Trim()=="")
            {
                MessageBox.Show("请选择对应图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.dltbLayerName = this.cmbDltb.Text.Trim();


            IFeatureClass dltbClass = null;
            //IFeatureClass xzqClass = null;
            try
            {
                IFeatureWorkspace pFeaWs = currWS as IFeatureWorkspace;
                dltbClass = pFeaWs.OpenFeatureClass(dltbLayerName);
                //xzqClass = pFeaWs.OpenFeatureClass(xzqLayerName);
            }
            catch { }
            if (dltbClass == null)
            {
                MessageBox.Show("找不到必备图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<int> errList = new List<int>();
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("图斑分割", "正在处理，请稍等...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                
                sys.YWCommonHelper.SplitDltbBySelect(lstXZQ, dltbClass, wait, (this.currMap as IActiveView).Extent,ref errList);
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("xzqsplittb");
                wait.Close();
                MessageBox.Show("分割完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

            memoEdit1.Text = "";
            foreach (int oid in errList)
            {
                memoEdit1.Text += oid + "\r\n";
            }
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
