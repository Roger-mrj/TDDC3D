using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.sys
{
    public partial class QuickImportForm : Form
    {
        public QuickImportForm()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private List<IWorkspace> lstOriginWs = new List<IWorkspace>();//元要素类列表
        private List<string> lstOriginDsName = new List<string>(); //原要素类名称

        

        //IWorkspace OriginWS;//源数据的WS
        IWorkspace m_DestinationWS;//目标数据的WS
        public IWorkspace DestinationWS
        {
            get { return m_DestinationWS; }
            set { m_DestinationWS = value; }
        }
        private void QuickImportForm_Load(object sender, EventArgs e)
        {
            LoadDatasets();  //当前已经有 目标数据库切打开
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

        private void simpleButton2_Click(object sender, EventArgs e)
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
            this.lblStatus.Text = "开始导入...";
            Application.DoEvents();
            try
            {
                string destDsName = this.cmbLocalFeatureclasses.Text.Trim();
                IFeatureDataset destDs = (this.DestinationWS as IFeatureWorkspace).OpenFeatureDataset(destDsName);
                IWorkspace2 tmpws2 = this.DestinationWS as IWorkspace2;

                for (int i = 0; i < this.lstOriginDsName.Count; i++)
                {
                    string srcDsName = this.lstOriginDsName[i];
                    IWorkspace srcWs = this.lstOriginWs[i];
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
                    lblStatus.Text = "开始导入"+srcDsName+"...";
                    Application.DoEvents();
                    if (EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(srcWs, DestinationWS, srcDsName, srcDsName, destDs, null))
                    {
                        //修改别名                        
                        Application.DoEvents();
                        IFeatureClass srcClass = (srcWs as IFeatureWorkspace).OpenFeatureClass(srcDsName);
                        string aliasName = srcClass.AliasName;
                        IFeatureClass pDestFeaClass = (this.DestinationWS as IFeatureWorkspace).OpenFeatureClass(srcDsName);
                        IClassSchemaEdit2 pClassSchemaEdit2 = pDestFeaClass as IClassSchemaEdit2;
                        pClassSchemaEdit2.AlterAliasName(aliasName);
                                               
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
            IWorkspace  OriginWS = (srcClass as IDataset).Workspace;
            string OriginFCName = LayerHelper.GetClassShortName(srcClass as IDataset);
            
            this.btSounceFC.Text = OriginWS.PathName + "\\" + OriginFCName;

            //添加项
            this.lstSrcFeasPath.Items.Add(this.btSounceFC.Text.Trim());
            lstOriginWs.Add(OriginWS);
            lstOriginDsName.Add(OriginFCName);

        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            //移除项
            if (this.lstSrcFeasPath.SelectedItem == null) return;
            int idx = this.lstSrcFeasPath.SelectedIndex;
            this.lstSrcFeasPath.Items.RemoveAt(idx);

            this.lstOriginWs.RemoveAt(idx);
            this.lstOriginDsName.RemoveAt(idx);

        }

        private void QuickImportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.lstOriginWs.Count > 0)
            {
                for (int i = lstOriginWs.Count-1; i >= 0; i--)
                {
                    OtherHelper.ReleaseComObject(this.lstOriginWs[i]);
                }
            }

            if (this.m_DestinationWS != RCIS.Global.GlobalEditObject.GlobalWorkspace)
            {
                OtherHelper.ReleaseComObject(this.m_DestinationWS);
            }

        }
    }
}
