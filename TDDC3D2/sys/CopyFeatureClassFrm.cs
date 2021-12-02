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
using ESRI.ArcGIS.Geometry;

using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.sys
{
    public partial class CopyFeatureClassFrm : Form
    {
        public CopyFeatureClassFrm()
        {
            InitializeComponent();
        }
        public IWorkspace currWS = null;
        public string srcFcName = "";

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.srcFcName == "")
                return;
            if (this.cmbDatasets.Text.Trim() == "")
                return;
            if (this.txtClassName.Text.Trim() == "")
                return;
            string aliasName = txtAlias.Text.Trim();
            if (aliasName == "")
            {
                aliasName = this.txtClassName.Text.Trim();
            }
            string destClassName = this.txtClassName.Text.Trim();

            //目标数据集
            IFeatureDataset destDs = (this.currWS as IFeatureWorkspace).OpenFeatureDataset(cmbDatasets.Text.Trim());
            IWorkspace2 tmpws2 = this.currWS as IWorkspace2;

            
            //删除旧的
            if (tmpws2.get_NameExists(esriDatasetType.esriDTFeatureClass, destClassName))
            {
                IFeatureClass pDestFeaClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass(destClassName);
                IDataset pDestDest = pDestFeaClass as IDataset;
                if (pDestDest.CanDelete())
                {
                    pDestDest.Delete();
                }

            }
            lblStatus.Text = "开始导入" + srcFcName + "...";
            Application.DoEvents();
            if (EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(this.currWS, this.currWS, srcFcName, destClassName, destDs, null))
            {
                //修改别名                        
                Application.DoEvents();

                IFeatureClass pDestFeaClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass(destClassName);
                IClassSchemaEdit2 pClassSchemaEdit2 = pDestFeaClass as IClassSchemaEdit2;
                pClassSchemaEdit2.AlterAliasName(aliasName);

            }
            lblStatus.Text = "复制完毕！";
            MessageBox.Show("复制完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
       
        private void CopyFeatureClassFrm_Load(object sender, EventArgs e)
        {
            //加载所有要素数据集
            this.cmbDatasets.Properties.Items.Clear();

            IEnumDataset pEnumDs = this.currWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pDS = pEnumDs.Next();
            while (pDS != null)
            {
                this.cmbDatasets.Properties.Items.Add(pDS.Name.ToUpper());
                pDS = pEnumDs.Next();
            }
            if (this.cmbDatasets.Properties.Items.Count > 0)
            {
                this.cmbDatasets.SelectedIndex = 0;
            }
        }
    }
}
