using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.gengxin
{
    public partial class FrmDLTBCZCSX : Form
    {
        public IWorkspace _curWS;
        public IMap _CurMap;

        public FrmDLTBCZCSX()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDLTBCZCSX_Load(object sender, EventArgs e)
        {
            if ((_curWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, "XZQ"])
            {
                List<string> dms = FeatureHelper.GetDMMCDicByQueryDef(this._curWS as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                cboXZQ.Properties.Items.Clear();
                foreach (string dm in dms)
                {
                    cboXZQ.Properties.Items.Add(dm);
                }
                if (cboXZQ.Properties.Items.Count > 0) cboXZQ.SelectedIndex = 0;
            } 
            LayerHelper.LoadLayer2Combox(cboCZCGX, _CurMap);
            for (int i = 0; i < cboCZCGX.Properties.Items.Count; i++)
            {
                string layerName = cboCZCGX.Properties.Items[i].ToString();
                string talleName = layerName.Split('|')[0];
                if (talleName == "CZCDYDGX")
                {
                    cboCZCGX.Text = layerName;
                    break;
                }
            }
            LayerHelper.LoadLayer2Combox(cboDLTBGX, _CurMap);
            for (int i = 0; i < cboDLTBGX.Properties.Items.Count; i++)
            {
                string layerName = cboDLTBGX.Properties.Items[i].ToString();
                string talleName = layerName.Split('|')[0];
                if (talleName == "DLTBGX")
                {
                    cboDLTBGX.Text = layerName;
                    break;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboCZCGX.Text) || string.IsNullOrWhiteSpace(cboDLTBGX.Text) || string.IsNullOrWhiteSpace(cboXZQ.Text))
            {
                MessageBox.Show("请先选择参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!MathHelper.IsNumeric(cboXZQ.Text) || cboXZQ.Text.Length != 6)
            {
                MessageBox.Show("县区代码必须是六位数字。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string dltbgx = cboDLTBGX.Text.Split('|')[0];
            if (!(_curWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, dltbgx])
            {
                MessageBox.Show("数据库中没有找到地类图斑更新层数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string czcgx = cboCZCGX.Text.Split('|')[0];
            if (!(_curWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, czcgx])
            {
                MessageBox.Show("数据库中没有找到城镇村等用地更新层数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在计算图斑与城镇村的空间关系……", "提示");
            wait.Show();
            IWorkspace tmpWS = WorkspaceHelper2.DeleteAndNewTmpGDB();
            string dltbpath = WorkspaceHelper2.GetFeatureClassPath(_curWS, dltbgx);
            string czcpath = WorkspaceHelper2.GetFeatureClassPath(_curWS, czcgx);
            Boolean b = GpToolHelper.GP_TabulateIntersection(dltbpath, "OBJECTID", czcpath, "CZCLX", tmpWS.PathName + "\\dltbczc");
            if (!b)
            {
                MessageBox.Show("提取失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                wait.Close();
                return;
            }
            DataTable dt = EsriDatabaseHelper.ITable2DataTable(tmpWS, "Select OBJECTID_1, CZCLX, AREA From dltbczc");
            wait.SetCaption("正在提取信息……");
            using(ESRI.ArcGIS.ADF.ComReleaser comRe = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureClass pdltbgx = (_curWS as IFeatureWorkspace).OpenFeatureClass(dltbgx);
                IFeatureCursor pUpdate = pdltbgx.Update(null, true);
                comRe.ManageLifetime(pdltbgx);
                comRe.ManageLifetime(pUpdate);
                int iCZCSXM = pdltbgx.FindField("CZCSXM");
                IFeature pFeature;
                while ((pFeature = pUpdate.NextFeature()) != null)
                {
                    DataRow[] drs = dt.Select("OBJECTID_1 = " + pFeature.OID, "AREA DESC");
                    if (drs.Count() > 0)
                    {
                        pFeature.set_Value(iCZCSXM, drs[0]["CZCLX"]);
                    }
                    else
                    {
                        pFeature.set_Value(iCZCSXM, "");
                    }
                    pUpdate.UpdateFeature(pFeature);
                }
                pUpdate.Flush();
            }
            wait.Close();
            MessageBox.Show("提取完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
