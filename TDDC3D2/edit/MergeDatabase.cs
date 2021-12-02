using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessor;

namespace TDDC3D.edit
{
    public partial class MergeDatabase : Form
    {
        public ESRI.ArcGIS.Geodatabase.IWorkspace tarWorkspace;
        private List<IWorkspace> SourWorkspaces;

        public MergeDatabase()
        {
            InitializeComponent();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在检测数据库文件。", "提示");
                wait.Show();
                txtPath.Text = folder.SelectedPath;
                //检测目录中GDB文件数量，并打开
                string[] dirs = System.IO.Directory.GetDirectories(txtPath.Text, "*.gdb", System.IO.SearchOption.AllDirectories);
                List<ESRI.ArcGIS.Geodatabase.IWorkspace> lstGDB = new List<ESRI.ArcGIS.Geodatabase.IWorkspace>();
                for (int i = 0; i < dirs.Count(); i++)
                {
                    ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(dirs[i]);
                    if (pWorkspace != null) lstGDB.Add(pWorkspace);
                }
                txtInfo.Text += string.Format("检测到GDB数据库{0}个。\r\n", lstGDB.Count.ToString());
                txtInfo.SelectionStart = txtInfo.Text.Length;
                txtInfo.ScrollToCaret();
                //监测目录中mdb文件数量，并打开
                dirs = System.IO.Directory.GetFiles(txtPath.Text, "*.mdb", System.IO.SearchOption.AllDirectories);
                List<ESRI.ArcGIS.Geodatabase.IWorkspace> lstMDB = new List<ESRI.ArcGIS.Geodatabase.IWorkspace>();
                for (int i = 0; i < dirs.Count(); i++)
                {
                    ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetAccessWorkspace(dirs[i]);
                    if (pWorkspace != null) lstMDB.Add(pWorkspace);
                }
                txtInfo.Text += string.Format("检测到MDB数据库{0}个。\r\n", lstMDB.Count.ToString());
                txtInfo.SelectionStart = txtInfo.Text.Length;
                txtInfo.ScrollToCaret();
                //监测目录中SHP数量，并确定文件夹数量
                string[] files = System.IO.Directory.GetFiles(txtPath.Text, "*.shp", System.IO.SearchOption.AllDirectories);
                List<string> dirss = new List<string>();
                foreach (string file in files)
                {
                    string dir = System.IO.Path.GetDirectoryName(file);
                    if (!dirss.Contains(dir)) dirss.Add(dir);
                }
                List<ESRI.ArcGIS.Geodatabase.IWorkspace> lstSHP = new List<ESRI.ArcGIS.Geodatabase.IWorkspace>();
                foreach (string adir in dirss)
                {
                    ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(adir);
                    if (pWorkspace != null) lstSHP.Add(pWorkspace);
                }
                txtInfo.Text += string.Format("检测到SHP目录{0}个。\r\n", lstSHP.Count.ToString());
                txtInfo.SelectionStart = txtInfo.Text.Length;
                txtInfo.ScrollToCaret();
                SourWorkspaces = new List<IWorkspace>();
                if (lstGDB.Count > 0) SourWorkspaces.AddRange(lstGDB);
                if (lstMDB.Count > 0) SourWorkspaces.AddRange(lstMDB);
                if (lstSHP.Count > 0) SourWorkspaces.AddRange(lstSHP);
                wait.Close();
                btnMerge.Enabled = true;
                btnMerge.Focus();
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            try
            {
                btnQuit.Enabled = false;
                btnMerge.Enabled = false;
                foreach (IWorkspace sourWorkspace in SourWorkspaces)
                {
                    DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始导入数据", "提示");
                    wait.Show();
                    txtInfo.Text += "开始导入" + sourWorkspace.PathName + "\r\n";
                    txtInfo.SelectionStart = txtInfo.Text.Length;
                    txtInfo.ScrollToCaret();
                    Application.DoEvents();

                    GDBAppend(sourWorkspace, tarWorkspace);
                    txtInfo.Text += "\r\n";
                    txtInfo.SelectionStart = txtInfo.Text.Length;
                    txtInfo.ScrollToCaret();
                    wait.Close();
                    btnMerge.Enabled = true;
                    btnQuit.Enabled = true;
                }
            }
            catch
            {
                btnMerge.Enabled = true;
                btnQuit.Enabled = true;
            }
            MessageBox.Show("合并完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GDBAppend(IWorkspace pSourWorkspace, IWorkspace pTarWorkspace)
        {
            //遍历导入FeatureDataset的数据
            IEnumDataset pTarEnumDataset = tarWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pTarDataset;
            while ((pTarDataset = pTarEnumDataset.Next()) != null)
            {
                IFeatureDataset pTarFeatureDataset = pTarDataset as IFeatureDataset;
                IEnumDataset pTarSubEnumDataset = pTarFeatureDataset.Subsets;
                IDataset pTarSubDataset;
                pTarSubEnumDataset.Reset();
                while ((pTarSubDataset = pTarSubEnumDataset.Next()) != null)
                {
                    string datasetName = pTarSubDataset.Name;
                    txtInfo.Text += "开始导入" + datasetName;
                    txtInfo.SelectionStart = txtInfo.Text.Length;
                    txtInfo.ScrollToCaret();
                    Application.DoEvents();
                    IWorkspace2 pWorkSpace2 = pSourWorkspace as IWorkspace2;
                    if (pWorkSpace2.NameExists[esriDatasetType.esriDTFeatureClass, datasetName])
                    {
                        IFeatureWorkspace pFeatureWorkspace = pSourWorkspace as IFeatureWorkspace;
                        IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(datasetName);
                        string sourceFile;
                        if (pFeatureClass.FeatureDataset == null)
                        {
                            sourceFile = pSourWorkspace.PathName + "\\" + datasetName;
                        }
                        else
                        {
                            sourceFile = pSourWorkspace.PathName + "\\" + pFeatureClass.FeatureDataset.Name + "\\" + datasetName;
                        }
                        if (pSourWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace) sourceFile += ".shp";
                        try
                        {
                            Geoprocessor gp = new Geoprocessor();
                            gp.OverwriteOutput = true;
                            ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                            apend.inputs = sourceFile;
                            apend.target = pTarWorkspace.PathName + "\\" + pTarDataset.Name + "\\" + pTarSubDataset.Name;
                            apend.schema_type = "NO_TEST";
                            gp.Execute(apend, null);
                            txtInfo.Text += "-成功\r\n";
                            txtInfo.SelectionStart = txtInfo.Text.Length;
                            txtInfo.ScrollToCaret();
                            Application.DoEvents();
                        }
                        catch
                        {
                            txtInfo.Text += "-失败\r\n";
                            txtInfo.SelectionStart = txtInfo.Text.Length;
                            txtInfo.ScrollToCaret();
                            Application.DoEvents();
                        }
                    }
                    else
                    {
                        txtInfo.Text += "-未找到\r\n";
                        txtInfo.SelectionStart = txtInfo.Text.Length;
                        txtInfo.ScrollToCaret();
                        Application.DoEvents();
                    }
                }
            }
            //遍历导入FeatureDataset的数据
            pTarEnumDataset = tarWorkspace.get_Datasets(esriDatasetType.esriDTTable);
            while ((pTarDataset = pTarEnumDataset.Next()) != null)
            {
                string datasetName = pTarDataset.Name;
                txtInfo.Text += "开始导入" + datasetName;
                txtInfo.SelectionStart = txtInfo.Text.Length;
                txtInfo.ScrollToCaret();
                Application.DoEvents();
                IWorkspace2 pWorkSpace2 = pSourWorkspace as IWorkspace2;
                if (pWorkSpace2.NameExists[esriDatasetType.esriDTTable, datasetName])
                {
                    try
                    {
                        Geoprocessor gp = new Geoprocessor();
                        gp.OverwriteOutput = true;
                        ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                        apend.inputs = pSourWorkspace.PathName + "\\" + datasetName;
                        apend.target = pTarWorkspace.PathName + "\\" + datasetName;
                        apend.schema_type = "NO_TEST";
                        gp.Execute(apend, null);
                        txtInfo.Text += "-成功\r\n";
                        txtInfo.SelectionStart = txtInfo.Text.Length;
                        txtInfo.ScrollToCaret();
                        Application.DoEvents();
                    }
                    catch
                    {
                        txtInfo.Text += "-失败\r\n";
                        txtInfo.SelectionStart = txtInfo.Text.Length;
                        txtInfo.ScrollToCaret();
                        Application.DoEvents();
                    }
                }
                else
                {
                    txtInfo.Text += "-未找到\r\n";
                    txtInfo.SelectionStart = txtInfo.Text.Length;
                    txtInfo.ScrollToCaret();
                    Application.DoEvents();
                }

            }
        }
    }
}
