using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.Utility;
using RCIS.Controls;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
namespace TDDC3D.raster
{
    public partial class NewMosaicDsForm : Form
    {
        public NewMosaicDsForm()
        {
            InitializeComponent();
        }

        private void NewMosaicDsForm_Load(object sender, EventArgs e)
        {

        }

        IWorkspace currWs = null;

        public IWorkspace CurrWs
        {
            get { return currWs; }
            set { currWs = value; }
        }
        private string datasetName = "";

        public string DatasetName
        {
            get { return datasetName; }
            set { datasetName = value; }
        }

        ISpatialReference currSR = null;



        private ISpatialReference getSRFromTxt()
        {
            string sSrprjFile = this.beSprj.Text.Trim();
            ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference pSR = SpatialRefHelper.ConstructCoordinateSystem(true, sSrprjFile);
            //修改容差范围
            
            pSR.SetDomain(-999999999999, 999999999999, -999999999999, 999999999999);
            return pSR;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //导入空间参考
            AddDataForm frm = new AddDataForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            ILayer currLyr = frm.resultLyr;
            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            if (currFeaLyr == null)
                return;
            try
            {
                IFeatureClass srcClass = currFeaLyr.FeatureClass;
                this.currSR = (srcClass as IGeoDataset).SpatialReference;

            }
            catch (Exception ex)
            {
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
            this.currSR = getSRFromTxt();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (currSR == null)
            {
                MessageBox.Show("请选择空间参考！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtDatasetName.Text.Trim() == "")
            {
                MessageBox.Show("输入项不可为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string dsName = this.txtDatasetName.Text.Trim();
                IMosaicDataset ds= MosaicDatasetHelper.CreateMosaicDataset(this.currWs, dsName, this.currSR);
                this.datasetName = dsName;
                this.DialogResult = DialogResult.OK;
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
