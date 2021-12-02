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
    public partial class XzqSplitDltbOptForm : Form
    {
        public XzqSplitDltbOptForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IWorkspace currWS = null;

        private  string dltbLayerName = "";
        private  string xzqLayerName = "";

        private void XzqSplitDltbOptForm_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbDltb, this.currMap, esriGeometryType.esriGeometryPolygon);
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbXzq, this.currMap, esriGeometryType.esriGeometryPolygon);

            //this.cmbXzq.Properties.Items.Clear();
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
            //        this.cmbXzq.Properties.Items.Add(clsName);
            //        this.cmbDltb.Properties.Items.Add(clsName);
            //    }
            //}
            int idx1 = -1;
            int idx2 = -1;
            for (int i = 0; i < this.cmbXzq.Properties.Items.Count; i++)
            {
                string name = this.cmbXzq.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("XZQ"))
                {
                    idx1=i;
                    break;
                }
            }
            this.cmbXzq.SelectedIndex = idx1;


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
            if ((this.cmbXzq.Text.Trim() == "") || (this.cmbDltb.Text.Trim()==""))
            {
                MessageBox.Show("请选择对应图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.xzqLayerName = RCIS.Utility.OtherHelper.GetLeftName( this.cmbXzq.Text.Trim());
            this.dltbLayerName =  RCIS.Utility.OtherHelper.GetLeftName( this.cmbDltb.Text.Trim());


            IFeatureClass dltbClass = null;
            IFeatureClass xzqClass = null;
            try
            {
                IFeatureWorkspace pFeaWs = currWS as IFeatureWorkspace;
                dltbClass = pFeaWs.OpenFeatureClass(dltbLayerName);
                xzqClass = pFeaWs.OpenFeatureClass(xzqLayerName);
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
                
                sys.YWCommonHelper.SplitDltbByXzq(xzqClass, dltbClass, wait, (this.currMap as IActiveView).Extent,ref errList);
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

        }
    }
}
