using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RCIS.Database;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;

namespace TDDC3D.sxvalue
{
    public partial class DltbSetValueForm : Form
    {
        public DltbSetValueForm()
        {
            InitializeComponent();
        }

        public List<IFeature> sendFeatures;


        private Dictionary<string, string> dicDlbm = new Dictionary<string, string>();
        private Dictionary<string, string> dicTbxhdm = new Dictionary<string, string>();
        private Dictionary<string, string> dicGdzzsxdm = new Dictionary<string, string>();
        private void DltbSetValueForm_Load(object sender, EventArgs e)
        {
            DataTable dt = LS_SetupMDBHelper.GetDataTable("select dm,mc from DIC_39种植属性代码表 ", "zzsx");
            foreach (DataRow dr in dt.Rows)
            {
                dicGdzzsxdm.Add(dr["DM"].ToString(),dr["MC"].ToString());
                this.ZZSXDM.Properties.Items.Add(dr["DM"].ToString());
            }

            dt = LS_SetupMDBHelper.GetDataTable("select dm,mc from DIC_38图斑细化类型代码表 ", "tbxh");
            foreach (DataRow dr in dt.Rows)
            {
                dicTbxhdm.Add(dr["DM"].ToString(), dr["MC"].ToString());
                this.TBXHDM.Properties.Items.Add(dr["DM"].ToString());
            }

            dt = LS_SetupMDBHelper.GetDataTable("select dm,mc from 三调工作分类", "gzfl");
            foreach (DataRow dr in dt.Rows)
            {
                dicDlbm.Add(dr["DM"].ToString(), dr["MC"].ToString());
                this.DLBM.Properties.Items.Add(dr["DM"].ToString());
            }
            
        }

        private void DLBM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dlbm = this.DLBM.Text.Trim();
            if (dicDlbm.ContainsKey(dlbm))
            {
                this.DLMC.Text = dicDlbm[dlbm];
            }
        }

        private void TBXHDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sTbxhdm = this.TBXHDM.Text.Trim();
            if (dicTbxhdm.ContainsKey(sTbxhdm))
            {
                this.TBXHMC.Text = dicTbxhdm[sTbxhdm];
            }
        }

        private void GDZZSXDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sgdzzsx = this.ZZSXDM.Text.Trim();
            if (dicGdzzsxdm.ContainsKey(sgdzzsx))
            {
                this.ZZSXMC.Text = dicGdzzsxdm[sgdzzsx];
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (sendFeatures.Count == 0)
            {
                MessageBox.Show("当前选中0个要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (IFeature aFea in sendFeatures)
                {
                    foreach (Control c in this.groupControl1.Controls)
                    {
                        string name = c.Name.ToUpper();
                        string sVal = "";
                        if (c is DevExpress.XtraEditors.TextEdit)
                        {
                            sVal = ((DevExpress.XtraEditors.TextEdit)c).Text;
                        }
                        else if (c is DevExpress.XtraEditors.ComboBoxEdit)
                        {
                            sVal = ((DevExpress.XtraEditors.ComboBoxEdit)c).Text;
                        }
                        if (sVal.Trim() == "") continue;
                        int idx = aFea.Fields.FindField(name);
                        if (idx == -1)
                            continue;
                        IField pfld = aFea.Fields.get_Field(idx);
                        if (pfld.Type == esriFieldType.esriFieldTypeInteger || pfld.Type == esriFieldType.esriFieldTypeSmallInteger)
                        {
                            int iVal;
                            int.TryParse(sVal,out iVal);
                            aFea.set_Value(idx,iVal);
                        }
                        else if (pfld.Type == esriFieldType.esriFieldTypeSingle || pfld.Type == esriFieldType.esriFieldTypeDouble)
                        {
                            double dVal = 0;
                            double.TryParse(sVal,out dVal);
                            aFea.set_Value(idx,dVal);
                        }
                        else
                        {
                            aFea.set_Value(idx, sVal);
                        }

                        aFea.Store();

                    }
                    
                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("setvalue");
                MessageBox.Show(  "保存完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception cex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(cex.Message);
            }
        }


    }
}
