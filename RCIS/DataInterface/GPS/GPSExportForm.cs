using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using RCIS.Utility ;
using RCIS.GISCommon;

namespace RCIS.DataExchange.GPS
{
    public partial class GPSExportForm : Form
    {
        public GPSExportForm()
        {
            InitializeComponent();
            
        }
        
        private void beGPSfile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "打开GPS点文件";
            ofd.ValidateNames = true;
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                this.beGPSfile.Text = ofd.FileName;
            }
        }

        private void beShapefile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Shapefile文件(*.shp)|*.shp";
            sfd.AddExtension = true;            
            sfd.OverwritePrompt = true;
            sfd.ValidateNames = true;
            sfd.FileName = TextHelper.FormatDateTime(DateTime.Now, "_");
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                this.beShapefile.Text = sfd.FileName;
            }
        }

        private void btnGO_Click(object sender, EventArgs e)
        {
            if (!File.Exists(this.beGPSfile.Text))
            {
                MessageBox.Show("找不到GPS点数据文件", "GPS数据转换",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(this.beShapefile .Text))
            {
                MessageBox.Show("目标Shapefile文件已经存在了!", "GPS数据转换",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.tbSplit.Text.Equals(""))
                this.tbSplit.Text = ",";
            GPSExporter aExp = new GPSExporter();
            aExp.Shapefile = this.beShapefile.Text;
            aExp.GPSFile = this.beGPSfile.Text;
            aExp.HasPointNO = this.cbPtNO.Checked;
            aExp.HasZ = this.cbHasZ.Checked;
            aExp.SplitChar = this.tbSplit.Text.Trim();
            
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (aExp.BeginExport())
                {
                    aExp.ExportToShapefile(null, null, false);
                    aExp.FinishExport();
                    MessageBox.Show("转换完毕！", "提示",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch  { MessageBox.Show("数据转换失败!"); }
            finally
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;

            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}