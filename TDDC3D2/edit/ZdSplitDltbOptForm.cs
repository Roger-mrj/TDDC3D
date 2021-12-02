using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
namespace TDDC3D.edit
{
    public partial class ZdSplitDltbOptForm : Form
    {
        public ZdSplitDltbOptForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;

        private string dltbLayerName = "";
        private string zdLayerName = "";

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if ((this.cmbZD.Text.Trim() == "") || (this.cmbDltb.Text.Trim() == ""))
            {
                MessageBox.Show("请选择对应图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.zdLayerName =RCIS.Utility.OtherHelper.GetLeftName( this.cmbZD.Text.Trim());
            this.dltbLayerName = RCIS.Utility.OtherHelper.GetLeftName(this.cmbDltb.Text.Trim());


            IFeatureClass dltbClass = null;
            IFeatureClass xzqClass = null;
            try
            {
                IFeatureWorkspace pFeaWs = currWS as IFeatureWorkspace;
                dltbClass = pFeaWs.OpenFeatureClass(dltbLayerName);
                xzqClass = pFeaWs.OpenFeatureClass(zdLayerName);
            }
            catch { }
            if ((dltbClass == null) || (xzqClass == null))
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

                sys.YWCommonHelper.SplitDltbByZd(xzqClass, dltbClass, wait, (this.currMap as IActiveView).Extent, ref errList);
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

        private void ZdSplitDltbOptForm_Load(object sender, EventArgs e)
        {
            //this.cmbZD.Properties.Items.Clear();
            //this.cmbDltb.Properties.Items.Clear();


            //for (int i = 0; i < currMap.LayerCount; i++)
            //{
            //    ILayer currLyr = this.currMap.get_Layer(i);
            //    if (!(currLyr is IFeatureLayer)) continue;
            //    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            //    IFeatureClass currClass = currFeaLyr.FeatureClass;
            //    string clsName = (currClass as IDataset).Name.ToUpper();
            //    if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            //    {
            //        this.cmbZD.Properties.Items.Add(clsName);
            //        this.cmbDltb.Properties.Items.Add(clsName);
            //    }
            //}
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbDltb, this.currMap, esriGeometryType.esriGeometryPolygon);
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbZD, this.currMap, esriGeometryType.esriGeometryPolygon);


            int idx1 = -1;
            int idx2 = -1;
            for (int i = 0; i < this.cmbZD.Properties.Items.Count; i++)
            {
                string name = this.cmbZD.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("CJDCQ"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbZD.SelectedIndex = idx1;


            for (int i = 0; i < this.cmbDltb.Properties.Items.Count; i++)
            {
                string name = this.cmbDltb.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLTB") && name.Length >= 4)
                {
                    idx2 = i;
                    break;
                }
            }
            this.cmbDltb.SelectedIndex = idx2;
        }
    }
}
