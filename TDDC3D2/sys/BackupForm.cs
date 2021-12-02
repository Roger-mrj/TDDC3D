using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDDC3D.sys
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.beFileGdb.Enabled = true;
                this.beMdb.Enabled = false;
                this.beShp.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                this.beFileGdb.Enabled = false;
                this.beMdb.Enabled = true;
                this.beShp.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 2)
            {
                this.beFileGdb.Enabled = false;
                this.beMdb.Enabled = false;
                this.beShp.Enabled = true;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beFileGdb_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beFileGdb.Text = dlg.SelectedPath;
        }

        private void beMdb_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PGDB文件|*.MDB";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beMdb.Text = dlg.FileName;
        }

        private void beShp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SHP文件|*.SHP";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beShp.Text = dlg.FileName;
        }

        private void buttonEdit4_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestDir.Text = dlg.SelectedPath;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.beDestDir.Text.Trim() == "") return;

            if (this.radioGroup1.SelectedIndex == 0)
            {
                if (this.beFileGdb.Text.Trim() == "") return;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string dirName = this.beFileGdb.Text.Trim();
                    int lidx=dirName.LastIndexOf("\\");
                    if (lidx>-1)
                    {
                        dirName = dirName.Substring( lidx + 1);
                    }

                    string destDir = this.beDestDir.Text + "\\" + dirName;
                    RCIS.Utility.File_DirManipulate.FolderCopy(this.beFileGdb.Text, destDir);
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("拷贝完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message);
                }

            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                if (this.beMdb.Text.Trim() == "") return;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string fileName = System.IO.Path.GetFileName(this.beMdb.Text.Trim());
                    string destFile = this.beDestDir.Text + "\\" + fileName;
                    RCIS.Utility.File_DirManipulate.FileCopy(this.beMdb.Text, destFile);
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("拷贝完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message);
                }
            }
            else if (this.radioGroup1.SelectedIndex == 2)
            {
                if (this.beShp.Text.Trim() == "")
                    return;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    //获取所有与shp文件名称相同的文件
                    string shpName = System.IO.Path.GetFileNameWithoutExtension(this.beShp.Text.Trim());

                    string srcDir = System.IO.Path.GetDirectoryName(this.beShp.Text.Trim());
                    string destDir = this.beDestDir.Text.Trim();
                    string[] sFiles = System.IO.Directory.GetFiles(srcDir, shpName + ".*");
                    foreach (string aFile in sFiles)
                    {
                        string destFileName=System.IO.Path.GetFileName(aFile);
                        RCIS.Utility.File_DirManipulate.FileCopy(aFile, destDir + "\\" + destFileName);
                    }

                    this.Cursor = Cursors.Default;
                    MessageBox.Show("拷贝完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
