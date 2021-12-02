using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.gdzy
{
    public partial class FrmGetSpatialAttribute : Form
    {
        public FrmGetSpatialAttribute()
        {
            InitializeComponent();
        }
        public IWorkspace pCurrWs = null;
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.CheckState == CheckState.Checked)
            {
                chkPD.CheckState = CheckState.Checked;
                chkZRQ.CheckState = CheckState.Checked;
                chkSZ.CheckState = CheckState.Checked;
                chkDL.CheckState = CheckState.Checked;
                chkHD.CheckState = CheckState.Checked;
                chkPH.CheckState = CheckState.Checked;
                chkSW.CheckState = CheckState.Checked;
                chkWR.CheckState = CheckState.Checked;
                chkYJZ.CheckState = CheckState.Checked;
                //chkZD.CheckState = CheckState.Checked;
                chkZLFLDM.CheckState = CheckState.Checked;
            }
            else
            {
                chkDL.CheckState = CheckState.Unchecked;
                chkHD.CheckState = CheckState.Unchecked;
                chkPD.CheckState = CheckState.Unchecked;
                chkPH.CheckState = CheckState.Unchecked;
                chkSW.CheckState = CheckState.Unchecked;
                chkSZ.CheckState = CheckState.Unchecked;
                chkWR.CheckState = CheckState.Unchecked;
                chkYJZ.CheckState = CheckState.Unchecked;
                chkZD.CheckState = CheckState.Unchecked;
                chkZRQ.CheckState = CheckState.Unchecked;
                chkZLFLDM.CheckState = CheckState.Unchecked;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbLayer.Text))
            {
                MessageBox.Show("请选择图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (chkDL.CheckState == CheckState.Unchecked &&
                chkHD.CheckState == CheckState.Unchecked &&
                chkPD.CheckState == CheckState.Unchecked &&
                chkPH.CheckState == CheckState.Unchecked &&
                chkSW.CheckState == CheckState.Unchecked &&
                chkSZ.CheckState == CheckState.Unchecked &&
                chkWR.CheckState == CheckState.Unchecked &&
                chkYJZ.CheckState == CheckState.Unchecked &&
                chkZD.CheckState == CheckState.Unchecked &&
                chkZRQ.CheckState == CheckState.Unchecked &&
                chkZLFLDM.CheckState == CheckState.Unchecked)
            {
                MessageBox.Show("请选择目标字段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在提取", "请稍等");
            wait.Show();
            string layerName = "";
            if (cmbLayer.Text.ToString() == "分类单元")
                layerName = "FLDY";
            else
                layerName = "KCFLDY";
            IFeatureClass pFeaClass = (pCurrWs as IFeatureWorkspace).OpenFeatureClass(layerName);
            if (chkDL.Checked)
            {
                wait.SetCaption("正在赋值耕地二级地类属性");
                if (layerName == "FLDY")
                {
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET GDEJDL = '',GDEJDLJB=''");
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET GDEJDL = DLMC,GDEJDLJB = substring(DLBM,4,1)");
                }
                else if (layerName == "KCFLDY")
                {
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET GDEJDL = '',GDEJDLJB=''");
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET GDEJDL = ZZSXMC");
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET GDEJDLJB = 'j' where ZZSXDM = 'JKHF'");
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET GDEJDLJB = 'g' where ZZSXDM = 'GCHF'");
                }
            }
            if (chkZRQ.Checked || chkSZ.Checked)
            {
                IFeature pFea = RCIS.GISCommon.GetFeaturesHelper.GetFirstFeature(pFeaClass, null);
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("ZLDWDM")).ToString();
                zldwdm = zldwdm.Substring(0, 6);
                string sql = "select * from SYS_QGGXSSZRQHSZ where xzqdm='" + zldwdm + "'";
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (dt.Rows.Count == 0)
                {
                    wait.Visible = false;
                    MessageBox.Show("未找到当前行政区无法赋值熟制和自然区属性，请联系管理员。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    wait.Visible = true;
                }
                else
                {
                    if (chkSZ.Checked)
                    {
                        wait.SetCaption("正在赋值熟制属性");
                        pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET SZ = '',SZJB=''");
                        pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET SZ = '" + dt.Rows[0]["SZMC"].ToString() + "',SZJB='" + dt.Rows[0]["SZDM"].ToString() + "'");
                    }
                    if (chkZRQ.Checked)
                    {
                        wait.SetCaption("正在赋值自然区属性");
                        pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET ZRQDM = '',ZRQMC=''");
                        pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET ZRQDM = '" + dt.Rows[0]["ZRQDM"].ToString() + "',ZRQMC='" + dt.Rows[0]["ZRQMC"].ToString() + "'");
                    }
                }
            }
            //坡度
            if (chkPD.Checked)
            {
                wait.SetCaption("正在赋值耕地坡度属性");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET PD = ''");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET PD = '≤2°' Where PDJB = '1'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET PD = '2°～6°' Where PDJB = '2'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET PD = '6°～15°' Where PDJB = '3'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET PD = '15°～25°' Where PDJB = '4'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET PD = '＞25°' Where PDJB = '5'");
            }
            //土层厚度
            if (chkHD.Checked)
            {
                wait.SetCaption("正在赋值耕地土层厚度属性");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TCHDJB = ''");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TCHDJB = '1' Where TCHD >= 100");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TCHDJB = '2' Where TCHD >= 60 And TCHD < 100");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TCHDJB = '3' Where TCHD < 60");
            }
            //生物多样性
            if (chkSW.Checked)
            {
                wait.SetCaption("正在赋值耕地生物多样性属性");
                string fieldName = "SWDYXJB";
                if (layerName == "KCFLDY") fieldName = "SWDYXJB";
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET " + fieldName + " = ''");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET " + fieldName + " = '1' Where SWDYX = '丰富'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET " + fieldName + " = '2' Where SWDYX = '一般'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET " + fieldName + " = '3' Where SWDYX = '不丰富'");
            }
            //土壤质地
            if (chkZD.Checked)
            {
                wait.SetCaption("正在赋值耕地土壤质地属性");

            }
            //土壤有机质含量
            if (chkYJZ.Checked)
            {
                wait.SetCaption("正在赋值耕地土壤有机质含量属性");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRYJZHLJB = ''");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRYJZHLJB = '1' Where TRYJZHL >= 20");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRYJZHLJB = '2' Where TRYJZHL >= 10 And TRYJZHL < 20");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRYJZHLJB = '3' Where TRYJZHL < 10");
            }
            //土壤PH值
            if (chkPH.Checked)
            {
                wait.SetCaption("正在赋值耕地土壤PH值属性");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRPHZJB = ''");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRPHZJB = '3b' Where TRPHZ >= 8.5");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRPHZJB = '2b' Where TRPHZ >= 7.5 And TRPHZ < 8.5");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRPHZJB = '10' Where TRPHZ >= 6.5 And TRPHZ < 7.5");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRPHZJB = '2a' Where TRPHZ >= 5.5 And TRPHZ < 6.5");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRPHZJB = '3a' Where TRPHZ < 5.5");
            }
            //土壤重金属污染状况
            if (chkWR.Checked)
            {
                wait.SetCaption("正在赋值耕地土壤重金属污染状况属性");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRZJSWRJB = ''");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRZJSWRJB = '1' Where TRZJSWRZK = '绿色'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRZJSWRJB = '2' Where TRZJSWRZK = '黄色'");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET TRZJSWRJB = '3' Where TRZJSWRZK = '红色'");
            }
            //质量分类代码
            if (chkZLFLDM.Checked)
            {
                wait.SetCaption("正在赋值质量分类代码属性");
                pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET ZLFLDM = ''");
                try
                {
                    pCurrWs.ExecuteSQL("UPDATE " + layerName + " SET ZLFLDM = ZRQDM + PDJB + TCHDJB + TRZDJB + TRYJZHLJB + TRPHZJB + TRSWDYXJB + TRZJSWRZKJB + SZJB + GDEJDLJB");
                }
                catch { }
            }
            wait.Close();
            MessageBox.Show("提取完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
