using ESRI.ArcGIS.Geodatabase;
using RCIS.Database;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace TDDC3D.datado
{
    public partial class DMMCWHForm : Form
    {
        public DMMCWHForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWS = null;

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Dictionary<string, string> dicQsdwdm = null;

        private Dictionary<string, string> getMCS(string table)
        {
            Dictionary<string, string> dicDlbm = new Dictionary<string, string>();
            DataTable dt = LS_SetupMDBHelper.GetDataTable("select * from "+table, "tmp");
            foreach (DataRow dr in dt.Rows)
            {
                dicDlbm.Add(dr["DM"].ToString(), dr["MC"].ToString());
            }
            return dicDlbm;
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;
            try
            {

                if (chkZldwmc.Checked)
                {
                    foreach (KeyValuePair<string, string> aItem in dicQsdwdm)
                    {
                        string sql = "update DLTB set ZLDWMC='" + aItem.Value.Trim() + "' where ZLDWDM='" + aItem.Key.Trim() + "0000000'";
                        this.currWS.ExecuteSQL(sql);
                        lblstatus.Text = "正在执行" + aItem.Value + "...";
                        Application.DoEvents();
                    }

                }
                if (chkQsdwmc.Checked)
                {
                    foreach (KeyValuePair<string, string> aItem in dicQsdwdm)
                    {
                        string sql = "update DLTB set QSDWMC='" + aItem.Value.Trim() + "' where QSDWDM='" + aItem.Key.Trim() + "0000000'";
                        this.currWS.ExecuteSQL(sql);
                        lblstatus.Text = "正在执行" + aItem.Value + "...";
                        Application.DoEvents();
                    }

                }
                lblstatus.Text = "";
                if (chkDLMC.Checked)
                {
                    Dictionary<string, string> dicDlbm = getMCS("三调工作分类");
                    foreach (KeyValuePair<string, string> aItem in dicDlbm)
                    {
                        string sql = "update DLTB set DLMC='" + aItem.Value.Trim() + "' where DLBM='" + aItem.Key.Trim() + "'";
                        this.currWS.ExecuteSQL(sql);
                    }
                }
                if (chkGDZZSXMC.Checked)
                {
                    Dictionary<string, string> dics = getMCS("DIC_39种植属性代码表");
                    foreach (KeyValuePair<string, string> aItem in dics)
                    {
                        string sql = "update DLTB set ZZSXMC='" + aItem.Value.Trim() + "' where ZZSXDM='" + aItem.Key.Trim() + "'";
                        this.currWS.ExecuteSQL(sql);
                    }
                }
                if (chkTBXHMC.Checked)
                {
                    Dictionary<string, string> dics = getMCS("DIC_38图斑细化类型代码表");
                    foreach (KeyValuePair<string, string> aItem in dics)
                    {
                        string sql = "update DLTB set TBXHMC='" + aItem.Value.Trim() + "' where TBXHDM='" + aItem.Key.Trim() + "'";
                        this.currWS.ExecuteSQL(sql);
                    }
                }


                if (chkGXDLMC.Checked)
                {
                    Dictionary<string, string> dicDlbm = getMCS("三调工作分类");
                    foreach (KeyValuePair<string, string> aItem in dicDlbm)
                    {
                        string sql = "update DLTBGX set DLMC='" + aItem.Value.Trim() + "' where DLBM='" + aItem.Key.Trim() + "'";
                        this.currWS.ExecuteSQL(sql);
                    }
                }
                if (chkGXZZSXMC.Checked)
                {
                    Dictionary<string, string> dics = getMCS("DIC_39种植属性代码表");
                    foreach (KeyValuePair<string, string> aItem in dics)
                    {
                        string sql = "update DLTBGX set ZZSXMC='" + aItem.Value.Trim() + "' where ZZSXDM='" + aItem.Key.Trim() + "'";
                        this.currWS.ExecuteSQL(sql);
                    }
                }
                if (chkGXTBXHMC.Checked)
                {
                    Dictionary<string, string> dics = getMCS("DIC_38图斑细化类型代码表");
                    foreach (KeyValuePair<string, string> aItem in dics)
                    {
                        string sql = "update DLTBGX set TBXHMC='" + aItem.Value.Trim() + "' where TBXHDM='" + aItem.Key.Trim() + "'";
                        this.currWS.ExecuteSQL(sql);
                    }
                }


                if (chkCZCDYDMC.Checked)
                {
                    foreach (KeyValuePair<string, string> aItem in dicQsdwdm)
                    {
                        string dm = aItem.Key.Trim();
                        if (dm.Length == 6)
                        {
                            dm += "0000000000000";
                        }
                        else if (dm.Length == 9)
                        {
                            dm += "0000000000";
                        }
                        else if (dm.Length == 12)
                        {
                            dm += "0000000";
                        }
                        string sql = "update CZCDYD set CZCMC='" + aItem.Value.Trim() + "' where CZCDM='" +dm + "'";
                        this.currWS.ExecuteSQL(sql);
                        lblstatus.Text = "正在执行" + aItem.Value + "...";
                        Application.DoEvents();
                    }
                }

                if (chkczcmc.Checked)
                {
                    foreach (KeyValuePair<string, string> aItem in dicQsdwdm)
                    {
                        string dm = aItem.Key.Trim();
                        if (dm.Length == 6)
                        {
                            dm += "0000000000000";
                        }
                        else if (dm.Length == 9)
                        {
                            dm += "0000000000";
                        }
                        else if (dm.Length == 12)
                        {
                            dm += "0000000";
                        }
                        string sql = "update CZCDYDGX set CZCMC='" + aItem.Value.Trim() + "' where CZCDM='" + dm + "'";
                        this.currWS.ExecuteSQL(sql);
                        lblstatus.Text = "正在执行" + aItem.Value + "...";
                        Application.DoEvents();
                    }
                }
                this.Cursor = Cursors.Default;
                MessageBox.Show("执行完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
        private Dictionary<string, string> getZldwdmMc()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ITable pTable = null;
            try
            {
                pTable = (this.currWS as IFeatureWorkspace).OpenTable("QSDWDMB");
            }
            catch (Exception ex)
            {
            }
            if (pTable == null)
            {
                MessageBox.Show("请首先维护权属单位代码表！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return dic;
            }

            
            IRow aRow = null;
            ICursor pCursor = pTable.Search(null, false);
            try
            {
                while ((aRow = pCursor.NextRow()) != null)
                {
                    dic.Add(FeatureHelper.GetRowValue(aRow, "QSDWDM").ToString(), FeatureHelper.GetRowValue(aRow, "QSDWMC").ToString());
                }
            }
            catch { }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
            }
            return dic;
        }
        private void DMMCWHForm_Load(object sender, EventArgs e)
        {

            dicQsdwdm = getZldwdmMc();
        }
    }
}
