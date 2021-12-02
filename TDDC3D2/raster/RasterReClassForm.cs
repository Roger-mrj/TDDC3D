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
using ESRI.ArcGIS.DataSourcesRaster;

using RCIS.Utility;
using RCIS.GISCommon;


namespace TDDC3D.raster
{
    public partial class RasterReClassForm : Form
    {
        public RasterReClassForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beInputRaster_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "栅格文件|*.img";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beInputRaster.Text = dlg.FileName;
        }
        public IRaster pOutRaster = null;
        private void beOutputRaster_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog(); dlg.Filter = "栅格文件|*.img";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beOutputRaster.Text = dlg.FileName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.beOutputRaster.Text.Trim() == "") return;
            if (this.beInputRaster.Text.Trim() == "") return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                IWorkspaceFactory rasterWorkspaceFactory = new RasterWorkspaceFactoryClass();
                string sFilePath = System.IO.Path.GetDirectoryName(beInputRaster.Text);
                IWorkspace rasterWorkspace = rasterWorkspaceFactory.OpenFromFile(sFilePath, 0);
                IRasterWorkspace pRW = rasterWorkspace as IRasterWorkspace;
                IRasterDataset rasterDataset = pRW.OpenRasterDataset(System.IO.Path.GetFileName(this.beInputRaster.Text));
                string outputName = System.IO.Path.GetFileName(this.beOutputRaster.Text);
                string outdir = System.IO.Path.GetDirectoryName(this.beOutputRaster.Text);
                IRaster inRaster = rasterDataset.CreateDefaultRaster();
                this.pOutRaster = RasterHelper.ProduceReClass(outputName, outdir, pRW as IWorkspace, inRaster);
                

                this.DialogResult = DialogResult.OK;
                Close();
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
