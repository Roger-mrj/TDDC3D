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
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using RCIS.Utility;
using RCIS.GISCommon;
using ESRI.ArcGIS.DataManagementTools;

namespace TDDC3D.sys
{
    public partial class AppendFCForm : Form
    {
        public AppendFCForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Append apend = new ESRI.ArcGIS.DataManagementTools.Append();
            
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
                this.cmbLocalFeatureclasses.Properties.Items.Clear();
                IEnumDataset pEnumDs = this.DestinationWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDS = pEnumDs.Next();
                while (pDS != null)
                {
                    IFeatureClassContainer pFeatureClsConner = (IFeatureClassContainer)pDS;
                    for (int i = 0; i < pFeatureClsConner.ClassCount; i++)
                    {
                        IFeatureClass pFeauteClass = pFeatureClsConner.get_Class(i);
                        this.cmbLocalFeatureclasses.Properties.Items.Add(pDS.Name.ToUpper()+"\\"+(pFeauteClass as IDataset).Name.ToUpper()+"|"+pFeauteClass.AliasName.ToString());

                    }
                                        
                    pDS = pEnumDs.Next();
                }
            }
        }
        private void AppendFCForm_Load(object sender, EventArgs e)
        {
            LoadDatasets();  //当前已经有 目标数据库切打开
        }

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

        private void btSounceFC_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            AddDataForm frm = new AddDataForm();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            ILayer currLyr = frm.resultLyr;
            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            if (currFeaLyr == null)
                return;
            IFeatureClass srcClass = currFeaLyr.FeatureClass;
            IWorkspace OriginWS = (srcClass as IDataset).Workspace;
            string OriginFCName = LayerHelper.GetClassShortName(srcClass as IDataset);

            string inFCs = OriginWS.PathName + "\\" + OriginFCName;
            if (OriginWS.Type == esriWorkspaceType.esriFileSystemWorkspace)
            {
                inFCs += ".shp";
            }
            
            this.btSounceFC.Text = inFCs;

            //添加项
            this.lstSrcFeasPath.Items.Add(this.btSounceFC.Text.Trim());
           
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            //移除项
            if (this.lstSrcFeasPath.SelectedItem == null) return;
            int idx = this.lstSrcFeasPath.SelectedIndex;
            this.lstSrcFeasPath.Items.RemoveAt(idx);

        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            if (this.lstSrcFeasPath.Items.Count == 0)
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
            try
            {
                
                string infcs = "";
                if (this.lstSrcFeasPath.Items.Count > 0)
                {
                    infcs += this.lstSrcFeasPath.Items[0].ToString();
                }
                for (int i = 1; i < this.lstSrcFeasPath.Items.Count; i++)
                {
                    infcs +=";"+ this.lstSrcFeasPath.Items[i].ToString();
                }
               
                string outfc = this.m_DestinationWS.PathName + "\\" + RCIS.Utility.OtherHelper.GetLeftName(this.cmbLocalFeatureclasses.Text);
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();
                
                apend.inputs = infcs;
                apend.target = outfc;
                apend.schema_type = "NO_TEST";
                gp.Execute(apend, null);
                string s = GpToolHelper.ReturnMessages(gp);
                this.Cursor = Cursors.Default;
                if ((s.ToUpper().Contains("ERROR")) || (s.Contains("失败")))
                {

                    MessageBox.Show("执行失败。\r\n" + s, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("执行成功！\r\n" , "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private void AppendFCForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            

            if (this.m_DestinationWS != RCIS.Global.GlobalEditObject.GlobalWorkspace)
            {
                OtherHelper.ReleaseComObject(this.m_DestinationWS);
            }

        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdCancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
