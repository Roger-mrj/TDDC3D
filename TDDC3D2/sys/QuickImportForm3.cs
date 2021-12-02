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
using ESRI.ArcGIS.Geoprocessor;
using RCIS.Utility;
using RCIS.GISCommon;
namespace TDDC3D.sys
{
    public partial class QuickImportForm3 : Form
    {
        public QuickImportForm3()
        {
            InitializeComponent();
            radioGroup1.SelectedIndex = 2;
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

        private void LoadDatasets()
        {
            if (this.DestinationWS != null)
            {
                int iSelectIndex = -1;
                this.cmbLocalFeatureclasses.Properties.Items.Clear();
                IEnumDataset pEnumDs = this.DestinationWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDS = pEnumDs.Next();
                while (pDS != null)
                {
                    if (pDS.Name.ToUpper() == "TDDC") iSelectIndex = cmbLocalFeatureclasses.Properties.Items.Count;
                    this.cmbLocalFeatureclasses.Properties.Items.Add(pDS.Name.ToUpper());
                    pDS = pEnumDs.Next();
                }
                if (iSelectIndex >= 0) cmbLocalFeatureclasses.SelectedIndex = iSelectIndex;
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
            simpleButton2.Focus();
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
                simpleButton2.Focus();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0 && this.beShpSrc.Text == "")
            {
                MessageBox.Show("请首先加载一个源要素数据库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (radioGroup1.SelectedIndex == 1 && this.beFGDB.Text == "")
            {
                MessageBox.Show("请首先加载一个源要素数据库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (radioGroup1.SelectedIndex == 2 && this.bePGDB.Text == "")
            {
                MessageBox.Show("请首先加载一个源要素数据库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                //获取所有要素类
                IFeatureWorkspace pFeaWs = srcWs as IFeatureWorkspace;
                IEnumDataset pEnumDs = srcWs.get_Datasets(esriDatasetType.esriDTAny);
                IDataset pDatset = null;
                
                while ((pDatset = pEnumDs.Next()) != null)
                {
                    if (pDatset is IFeatureClass)
                    {
                        if (tmpws2.get_NameExists(esriDatasetType.esriDTFeatureClass, pDatset.Name))
                        {
                            this.lblStatus.Text = "开始导入" + pDatset.Name + "...";
                            Application.DoEvents();
                            string srcName = radioGroup1.SelectedIndex == 0 ? this.srcWs.PathName + "\\" + pDatset.Name + ".shp" : this.srcWs.PathName + "\\" + pDatset.Name;
                            string outfc = this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text) + "\\" + pDatset.Name;
                            try
                            {
                                Geoprocessor gp = new Geoprocessor();
                                gp.OverwriteOutput = true;
                                ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                                apend.inputs = srcName;
                                apend.target = outfc;
                                apend.schema_type = "NO_TEST";
                                gp.Execute(apend, null);
                            }
                            catch
                            {

                            }

                        }
                        else if (pDatset.Name.ToLower() == "CJDCQ")
                        {
                            string fName = "CJDCQ";
                            this.lblStatus.Text = "开始导入" + pDatset.Name + "...";
                            Application.DoEvents();
                            string srcName = radioGroup1.SelectedIndex == 0 ? this.srcWs.PathName + "\\" + pDatset.Name + ".shp" : this.srcWs.PathName + "\\" + pDatset.Name;
                            string outfc = this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text) + "\\" + fName;
                            try
                            {
                                Geoprocessor gp = new Geoprocessor();
                                gp.OverwriteOutput = true;
                                ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                                apend.inputs = srcName;
                                apend.target = outfc;
                                apend.schema_type = "NO_TEST";
                                gp.Execute(apend, null);
                            }
                            catch
                            {

                            }
                        }
                        else if (pDatset.Name.ToLower() == "CJDCQJX")
                        {
                            string fName = "CJDCQJX";
                            this.lblStatus.Text = "开始导入" + pDatset.Name + "...";
                            Application.DoEvents();
                            string srcName = radioGroup1.SelectedIndex == 0 ? this.srcWs.PathName + "\\" + pDatset.Name + ".shp" : this.srcWs.PathName + "\\" + pDatset.Name;
                            string outfc = this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text) + "\\" + fName;
                            try
                            {
                                Geoprocessor gp = new Geoprocessor();
                                gp.OverwriteOutput = true;
                                ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                                apend.inputs = srcName;
                                apend.target = outfc;
                                apend.schema_type = "NO_TEST";
                                gp.Execute(apend, null);
                            }
                            catch
                            {

                            }
                        }
                    }
                    else if (pDatset is IFeatureDataset)
                    {
                        IFeatureClassContainer pContainer = pDatset as IFeatureClassContainer;
                        for (int k = 0; k < pContainer.ClassCount; k++)
                        {
                            IFeatureClass pFC = pContainer.get_Class(k);
                            this.lblStatus.Text = "开始导入" + pFC.AliasName + "...";
                            Application.DoEvents();
                            if (tmpws2.get_NameExists(esriDatasetType.esriDTFeatureClass, (pFC as IDataset).Name))
                            {
                                string srcName = radioGroup1.SelectedIndex == 0 ? this.srcWs.PathName + "\\" + pDatset.Name + ".shp" : this.srcWs.PathName + "\\" + pDatset.Name + "\\" + (pFC as IDataset).Name;
                                string outfc = (pFC as IDataset).Name.ToUpper() == "TFH" ?this.m_DestinationWS.PathName + "\\TF\\" + (pFC as IDataset).Name:this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text) + "\\" + (pFC as IDataset).Name;
                                try
                                {
                                    Geoprocessor gp = new Geoprocessor();
                                    gp.OverwriteOutput = true;
                                    ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                                    apend.inputs = srcName;
                                    apend.target = outfc;
                                    apend.schema_type = "NO_TEST";
                                    gp.Execute(apend, null);
                                }
                                catch 
                                {

                                }
                                
                            }
                            else if (pDatset.Name.ToLower() == "CJDCQ")
                            {
                                string fName = "CJDCQ";
                                this.lblStatus.Text = "开始导入" + pDatset.Name + "...";
                                Application.DoEvents();
                                string srcName = radioGroup1.SelectedIndex == 0 ? this.srcWs.PathName + "\\" + pDatset.Name + ".shp" : this.srcWs.PathName + "\\" + pDatset.Name;
                                string outfc = this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text) + "\\" + fName;
                                try
                                {
                                    Geoprocessor gp = new Geoprocessor();
                                    gp.OverwriteOutput = true;
                                    ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();
                                    
                                    apend.inputs = srcName;
                                    apend.target = outfc;
                                    apend.schema_type = "NO_TEST";
                                    gp.Execute(apend, null);
                                }
                                catch
                                {

                                }
                            }
                            else if (pDatset.Name.ToLower() == "CJDCQJX")
                            {
                                string fName = "CJDCQJX";
                                this.lblStatus.Text = "开始导入" + pDatset.Name + "...";
                                Application.DoEvents();
                                string srcName = radioGroup1.SelectedIndex == 0 ? this.srcWs.PathName + "\\" + pDatset.Name + ".shp" : this.srcWs.PathName + "\\" + pDatset.Name;
                                string outfc = this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text) + "\\" + fName;
                                try
                                {
                                    Geoprocessor gp = new Geoprocessor();
                                    gp.OverwriteOutput = true;
                                    ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();

                                    apend.inputs = srcName;
                                    apend.target = outfc;
                                    apend.schema_type = "NO_TEST";
                                    gp.Execute(apend, null);
                                }
                                catch
                                {

                                }
                            }
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

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (radioGroup1.SelectedIndex)
            {
                case 0:
                    beShpSrc.Enabled = true;
                    beFGDB.Enabled = false;
                    bePGDB.Enabled = false;
                    beFGDB.Text = "";
                    bePGDB.Text = "";
                    break;
                case 1:
                    beShpSrc.Enabled = false;
                    beFGDB.Enabled = true;
                    bePGDB.Enabled = false;
                    beShpSrc.Text = "";
                    bePGDB.Text = "";
                    break;
                case 2:
                    beShpSrc.Enabled = false;
                    beFGDB.Enabled = false;
                    bePGDB.Enabled = true;
                    beShpSrc.Text = "";
                    beFGDB.Text = "";
                    break;
                default:
                    break;
            }
        }

        private void beShpSrc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beShpSrc.Text = dlg.SelectedPath;
            srcWs = WorkspaceHelper2.GetShapefileWorkspace(this.beShpSrc.Text);
            simpleButton2.Focus();
        }

        private void beFGDB_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string selPath = dlg.SelectedPath;
            this.beFGDB.Text = selPath;
            this.srcWs = WorkspaceHelper2.GetFileGdbWorkspace(selPath);
            simpleButton2.Focus();
        }

    }
}
