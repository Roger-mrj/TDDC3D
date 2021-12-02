using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.Utility;

namespace TDDC3D.sys
{
    public partial class NewDatasetForm : Form
    {
        public NewDatasetForm()
        {
            InitializeComponent();
        }

        private string currNewDsName = "";


        private IWorkspace currWs = null;

        public IWorkspace CurrWs
        {
            get
            {
                return currWs;
            }

            set
            {
                currWs = value;
            }
        }

        public string CurrNewDsName
        {
            get
            {
                return currNewDsName;
            }

        }

        private void beSprj_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "空间参考文件|*.prj";
            dlg.InitialDirectory = Application.StartupPath + @"\srprj\";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beSprj.Text = dlg.FileName;
                

        }




        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.txtDatasetName.Text.Trim()=="")
            {
                MessageBox.Show("请输入数据集名称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (this.beSprj.Text.Trim()=="")
            {
                MessageBox.Show("请选择数据集空间参考文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (!this.beSprj.Text.Trim().ToUpper().EndsWith(".PRJ"))
            {
                MessageBox.Show("请选择正确的数据集空间参考文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //判断该数据集是否存在
            string dsName = this.txtDatasetName.Text.Trim().ToUpper();
            IFeatureDataset pFeaDs = null;
            try
            {
                pFeaDs = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(dsName);
            }
            catch { }
            if (pFeaDs!=null)
            {
                MessageBox.Show("该数据集已存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            IFeatureDataset gpFDS = null;

            //根据获取的merinan 选择合适的空间参考
            ISpatialReference pSR = null;
            try
            {
                string sSrprjFile = this.beSprj.Text.Trim();
                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                pSR = RCIS.GISCommon.SpatialRefHelper.ConstructCoordinateSystem(true, sSrprjFile);

                //修改容差范围
                double dtolerance = 0.0001;
                ISpatialReferenceTolerance spatialReferenceTolerance = pSR as ISpatialReferenceTolerance;
                spatialReferenceTolerance.XYTolerance = dtolerance;
                ISpatialReferenceResolution resolutionTolerance = pSR as ISpatialReferenceResolution;
                resolutionTolerance.set_XYResolution(true, dtolerance);
                resolutionTolerance.set_ZResolution(true, dtolerance);

                gpFDS = (this.CurrWs as IFeatureWorkspace).CreateFeatureDataset(dsName,  pSR);
                this.currNewDsName = dsName;
                
                this.DialogResult = DialogResult.OK;
                Close();

            }
            catch (Exception ex)
            {
                RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, "新建数据集");
            }

            
            


        }
    }
}
