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
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TDDC3D.raster
{
    public partial class MosaicDsManageForm : Form
    {
        public MosaicDsManageForm()
        {
            InitializeComponent();
        }
        private IWorkspace currWs = null;
        private void LoadDsTable()
        {
            //加载所有数据集和要素类
            if (this.currWs == null)
                return;
            this.treeView1.Nodes.Clear();

            IEnumDataset pEnumDs = this.currWs.get_Datasets(esriDatasetType.esriDTMosaicDataset);
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                TreeNode aNode = new TreeNode();
                aNode.Text = pDs.Name;
                aNode.ImageIndex = 1;
                aNode.SelectedImageIndex = 1;
                this.treeView1.Nodes.Add(aNode);

                pDs = pEnumDs.Next();

            }

        }

        private void MosaicDsManageForm_Load(object sender, EventArgs e)
        {

            //try
            //{
            //    IWorkspaceFactory pWSFac = new SdeWorkspaceFactoryClass();

            //    PropertySet aProp = new PropertySetClass();

            //    aProp.SetProperty("server", "localhost");
            //    aProp.SetProperty("instance", "sde:oracle11g:orcl");
            //    aProp.SetProperty("user", "ydyl");
            //    aProp.SetProperty("password", "ydyl");
            //    aProp.SetProperty("version", "sde.DEFAULT");
            //    IWorkspace pWS = pWSFac.Open(aProp, 0);
            //    this.currWs = pWS;
            //}
            //catch (Exception ex)
            //{
            //}



            if (RCIS.Global.GlobalEditObject.GlobalWorkspace != null)
            {
                string pathName = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName;
                if (pathName.ToUpper().EndsWith(".GDB"))
                {
                    this.radioGroup1.SelectedIndex = 0;
                    this.beFgdb.Text = pathName;
                }
                else if (pathName.ToUpper().EndsWith(".MDB"))
                {
                    this.radioGroup1.SelectedIndex = 1;
                    this.bePgdb.Text = pathName;
                }
                this.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
                LoadDsTable();
                this.treeView1.ExpandAll();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beFgdb_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beFgdb.Text = dlg.SelectedPath;
            currWs = WorkspaceHelper2.GetFileGdbWorkspace(dlg.SelectedPath);
            if (currWs == null)
            {
                MessageBox.Show("工作空间打开失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LoadDsTable();
            this.treeView1.ExpandAll();
        }

        private void bePgdb_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PGDB文件|*.MDB";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.bePgdb.Text = dlg.FileName;
            currWs = WorkspaceHelper2.GetAccessWorkspace(dlg.FileName);
            if (currWs == null)
            {
                MessageBox.Show("工作空间打开失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LoadDsTable();
            this.treeView1.ExpandAll();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            string name = this.treeView1.SelectedNode.Text.Trim();
            try
            {
                if (MosaicDatasetHelper.DeleteMosaic(name, this.currWs))
                {
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.treeView1.SelectedNode.Remove();
                }
                else
                {
                    MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //创建
            NewMosaicDsForm frm = new NewMosaicDsForm();

            



            frm.CurrWs = this.currWs;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                TreeNode aNode = new TreeNode();
                aNode.Text = frm.DatasetName;
                aNode.ImageIndex = 1;
                aNode.SelectedImageIndex = 1;
                this.treeView1.Nodes.Add(aNode);
            }

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            //导入影像
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "影像文件|*.img;*.tif;*.JPG;*.bmp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = dlg.FileName;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                
                string name = this.treeView1.SelectedNode.Text.Trim();
                IMosaicWorkspaceExtensionHelper pMosaicWsExHelper = new MosaicWorkspaceExtensionHelperClass();
                IMosaicWorkspaceExtension pMosaicWsExt = pMosaicWsExHelper.FindExtension(this.currWs);
                IMosaicDataset ds = pMosaicWsExt.OpenMosaicDataset(name);
                if (MosaicDatasetHelper.ImportRasterToMosaic(filename, ds))
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.RefreshTable(this.treeView1.SelectedNode.Text.Trim());

                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("导入失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshTable(string dsName)
        {
            IMosaicDataset pDs = MosaicDatasetHelper.GetMosaicDataset(dsName, this.currWs);
            if (pDs != null)
            {
                ITable table = MosaicDatasetHelper.GetMosaicDatasetTable(pDs);
                DataTable dt = FeatureHelper.ToDataTable(table);
                this.gridView1.Columns.Clear();
                this.gridControl1.DataSource = dt;

            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            this.RefreshTable(this.treeView1.SelectedNode.Text.Trim());
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null)
                return;
            //创建overview
            string dsName = this.treeView1.SelectedNode.Text.Trim();
            IMosaicDataset pDs = MosaicDatasetHelper.GetMosaicDataset(dsName, this.currWs);
            if (pDs == null)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                
                MosaicDatasetHelper.BuildOverviewsOnMD(pDs);
                this.Cursor = Cursors.Default;
                MessageBox.Show("创建完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //删除某影像

        }
    }
}
