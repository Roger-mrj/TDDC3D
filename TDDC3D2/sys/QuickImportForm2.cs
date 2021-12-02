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

using RCIS.Utility;
using RCIS.GISCommon;
namespace TDDC3D.sys
{
    public partial class QuickImportForm2 : Form
    {
        public QuickImportForm2()
        {
            InitializeComponent();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.beShpSrc.Enabled = true;
                this.bePGDB.Enabled = false;
                this.beFGDB.Enabled = false;
                this.beSqlLite.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                this.beShpSrc.Enabled = false;
                this.beFGDB.Enabled = true;
                this.bePGDB.Enabled = false;
                this.beSqlLite.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 2)
            {
                this.bePGDB.Enabled = true;
                this.beShpSrc.Enabled = false;
                this.beFGDB.Enabled = false;
                this.beSqlLite.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 3)
            {
                this.bePGDB.Enabled = false;
                this.beShpSrc.Enabled = false;
                this.beFGDB.Enabled = false;
                this.beSqlLite.Enabled = true;
            }

        }
        IWorkspace m_DestinationWS;//目标数据的WS
        public IWorkspace DestinationWS
        {
            get { return m_DestinationWS; }
            set { m_DestinationWS = value; }
        }
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void beShpSrc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beShpSrc.Text = dlg.SelectedPath;
            srcWs = WorkspaceHelper2.GetShapefileWorkspace(this.beShpSrc.Text);
            GetAllFCS(this.srcWs);

        }
        private void LoadDatasets()
        {
            if (this.DestinationWS != null)
            {
                this.cmbLocalFeatureclasses.Properties.Items.Clear();
                IEnumDataset pEnumDs = this.DestinationWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDS = pEnumDs.Next();
                while (pDS != null)
                {

                    this.cmbLocalFeatureclasses.Properties.Items.Add(pDS.Name.ToUpper());
                    pDS = pEnumDs.Next();
                }
            }
        }

        IWorkspace srcWs = null;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //加载目标要素类
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string selPath = dlg.SelectedPath;
            this.DestinationWS = WorkspaceHelper2.GetFileGdbWorkspace(dlg.SelectedPath);
            LoadDatasets();
        }

        private void GetAllFCS(IWorkspace ws)
        {
            this.chkListAllFC.Items.Clear();
            //获取所有要素类
            IFeatureWorkspace pFeaWs = ws as IFeatureWorkspace;
            IEnumDataset pEnumDs = ws.get_Datasets(esriDatasetType.esriDTAny);
            IDataset pDatset = null;
            while ((pDatset = pEnumDs.Next()) != null)
            {
                if (pDatset is IFeatureClass)
                {
                    this.chkListAllFC.Items.Add(pDatset.Name.ToUpper() + "|" + (pDatset as IFeatureClass).AliasName);
                }
                else if (pDatset is IFeatureDataset)
                {
                    IFeatureClassContainer pContainer = pDatset as IFeatureClassContainer;
                    for (int k = 0; k < pContainer.ClassCount; k++)
                    {
                        IFeatureClass pFC = pContainer.get_Class(k);

                        this.chkListAllFC.Items.Add( (pFC as IDataset).Name.ToUpper()+"|"+pFC.AliasName);
                    }
                }
            }
        }

        private void beFGDB_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string selPath = dlg.SelectedPath;
            this.beFGDB.Text = selPath;
            this.srcWs = WorkspaceHelper2.GetFileGdbWorkspace(selPath);
            GetAllFCS(this.srcWs);

        }

        private void bePGDB_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PGDB文件|*.mdb";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string file = dlg.FileName;
                this.bePGDB.Text = file;
                this.srcWs = WorkspaceHelper2.GetAccessWorkspace(file);
                GetAllFCS(this.srcWs);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.chkListAllFC.SelectedItems.Count == 0)
            {
                MessageBox.Show("请首先加载一个或多个源要素数据集。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.cmbLocalFeatureclasses.Text == "")
            {
                MessageBox.Show("目标数据集为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            this.lblStatus.Text = "开始导入...";
            Application.DoEvents();
            try
            {
                string destDsName = this.cmbLocalFeatureclasses.Text.Trim();
                IFeatureDataset destDs = (this.DestinationWS as IFeatureWorkspace).OpenFeatureDataset(destDsName);
                IWorkspace2 tmpws2 = this.DestinationWS as IWorkspace2;

                for (int i = 0; i < this.chkListAllFC.Items.Count; i++)
                {
                    if (this.chkListAllFC.Items[i].CheckState == CheckState.Checked)
                    {
                        string srcDsName = OtherHelper.GetLeftName(this.chkListAllFC.Items[i].ToString());


                        if (this.radioGroup1.SelectedIndex == 3)
                        {
                            if (srcDsName.ToUpper().StartsWith("MAIN."))
                            {
                                srcDsName = srcDsName.Substring(5);
                            }
                        }

                        //删除旧的
                        if (tmpws2.get_NameExists(esriDatasetType.esriDTFeatureClass, srcDsName))
                        {
                            IFeatureClass pDestFeaClass = (this.DestinationWS as IFeatureWorkspace).OpenFeatureClass(srcDsName);
                            IDataset pDestDest = pDestFeaClass as IDataset;
                            if (pDestDest.CanDelete())
                            {
                                pDestDest.Delete();
                            }

                        }
                        lblStatus.Text = "开始导入" + srcDsName + "...";
                        Application.DoEvents();
                        if (EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(srcWs, DestinationWS, srcDsName, srcDsName, destDs, null))
                        {
                            //修改别名                        
                            Application.DoEvents();
                            IFeatureClass srcClass = (srcWs as IFeatureWorkspace).OpenFeatureClass(srcDsName);
                            string aliasName = srcClass.AliasName;
                            if (this.radioGroup1.SelectedIndex == 3)
                            {
                                aliasName = srcDsName;
                            }
                            
                            IFeatureClass pDestFeaClass = (this.DestinationWS as IFeatureWorkspace).OpenFeatureClass(srcDsName);
                            IClassSchemaEdit2 pClassSchemaEdit2 = pDestFeaClass as IClassSchemaEdit2;
                            pClassSchemaEdit2.AlterAliasName(aliasName);

                        }
                    }

                }
                this.Cursor = Cursors.Default;
                this.lblStatus.Text = "";
                MessageBox.Show("导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private void QuickImportForm2_Load(object sender, EventArgs e)
        {
            LoadDatasets();  //当前已经有 目标数据库切打开
        }

        private void beSqlLite_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "sqllite文件|*.sqlite";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string file = dlg.FileName;
                this.bePGDB.Text = file;
                this.srcWs = WorkspaceHelper2.GetSqlliteWorkspace(file);
                GetAllFCS(this.srcWs);
            }
        }
    }
}
