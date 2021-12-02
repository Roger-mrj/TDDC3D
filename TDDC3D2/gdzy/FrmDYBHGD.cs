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
    public partial class FrmDYBHGD : Form
    {
        public FrmDYBHGD()
        {
            InitializeComponent();
        }
        public IWorkspace pCurrWs = null;
        public string name = "";

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkList.Items.Clear();
            if(cmbLayer.SelectedIndex==0)
                name="FLDY";
            else if(cmbLayer.SelectedIndex==1)
                name="KCFLDY";
            Dictionary<string, string> dmmc = GetDMMCDicByQueryDef(pCurrWs as IFeatureWorkspace, name, "ZLDWDM", "ZLDWMC");
            foreach (KeyValuePair<string, string> aItem in dmmc)
            {
                int idx = this.chkList.Items.Add(aItem.Key.Substring(0,12) + "|" + aItem.Value);
                this.chkList.SetItemChecked(idx, true);
            }
            if(chkList.Items.Count>0)
                chkAll.CheckState = CheckState.Checked;
        }

        private Dictionary<string, string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, string valueField)
        {
            Dictionary<string, string> dmmc = new Dictionary<string, string>();
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
                comRel.ManageLifetime(pQDef);
                pQDef.Tables = tableName + " Group By " + keyField + "," + valueField;
                pQDef.SubFields = keyField + "," + valueField;
                ICursor pCur = pQDef.Evaluate();
                comRel.ManageLifetime(pCur);
                IRow pRow;
                while ((pRow = pCur.NextRow()) != null)
                {
                    string dm = pRow.get_Value(0).ToString();
                    string mc = pRow.get_Value(1).ToString();
                    if(!dmmc.Keys.Contains(dm))
                        dmmc.Add(dm, mc);
                }
            }
            return dmmc;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("请选择图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在赋值","请稍等");
            wait.Show();
            IFeatureClass pFeaClass = (pCurrWs as IFeatureWorkspace).OpenFeatureClass(name);
            for (int i = 0; i < chkList.CheckedItems.Count; i++)
            {
                string zldwdm = chkList.CheckedItems[i].ToString();
                string zldwmc = zldwdm.Substring(zldwdm.IndexOf("|")+1);
                zldwdm = zldwdm.Substring(0,zldwdm.IndexOf("|"));
                wait.SetCaption("正在为"+zldwmc+"赋值");
                IQueryFilter pQf=new QueryFilterClass();
                pQf.WhereClause="zldwdm like '"+zldwdm+"%'";
                IFeatureCursor pCursor = pFeaClass.Update(pQf,true);
                IFeature pFeature;
                long num = 0;
                while ((pFeature = pCursor.NextFeature()) != null)
                {
                    num++;
                    string dybh = zldwdm + num.ToString().PadLeft(7, '0');
                    pFeature.set_Value(pFeature.Fields.FindField("DYBH"),dybh);
                    pCursor.UpdateFeature(pFeature);
                }
                pCursor.Flush();
                RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                //RCIS.GISCommon.
            }
            wait.Close();
            MessageBox.Show("赋值完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void checkEdit1_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
            {
                chkList.CheckAll();
            }
            if (!chkAll.Checked)
            {
                chkList.UnCheckAll();
            }
        }

        private void FrmDYBHGD_Load(object sender, EventArgs e)
        {
            name = "FLDY";
            Dictionary<string, string> dmmc = GetDMMCDicByQueryDef(pCurrWs as IFeatureWorkspace, name, "ZLDWDM", "ZLDWMC");
            foreach (KeyValuePair<string, string> aItem in dmmc)
            {
                int idx = this.chkList.Items.Add(aItem.Key.Substring(0, 12) + "|" + aItem.Value);
                this.chkList.SetItemChecked(idx, true);
            }
            if (chkList.Items.Count > 0)
                chkAll.CheckState = CheckState.Checked;
        }
    }
}
