using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDDC3D.sys
{
    public partial class DictionaryForm : Form
    {
        public DictionaryForm()
        {
            InitializeComponent();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RefreshData(string tabName)
        {

            this.gridControl1.DataSource = null;
            this.gridView1.Columns.Clear();

            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from " + tabName,"tmp");
            this.gridControl1.DataSource = dt;

            Dictionary<string, string> dicComments = new Dictionary<string, string>();
            dicComments.Add("DM", "代码");
            dicComments.Add("MC", "名称");
            dicComments.Add("KZDDJ", "控制点等级");

            for (int i = 0; i < this.gridView1.Columns.Count; i++)
            {
                string fldName = this.gridView1.Columns[i].FieldName.ToUpper().Trim();


                if (dicComments.ContainsKey(fldName))
                {
                    this.gridView1.Columns[i].Caption = dicComments[fldName];
                }
            }

        }

        private void DictionaryForm_Load(object sender, EventArgs e)
        {
            if (this.tvAllTable.Nodes.Count == 0)
                return;
            if (this.tvAllTable.Nodes[0].Nodes.Count == 0)
                return;
            this.tvAllTable.ExpandAll();

            TreeNode firstNode = this.tvAllTable.Nodes[0].Nodes[0];

            string aTab = firstNode.Tag.ToString();
            RefreshData(aTab);
        }

        private void tvAllTable_DoubleClick(object sender, EventArgs e)
        {
            if (this.tvAllTable.SelectedNode == null)
                return;
            if (this.tvAllTable.SelectedNode.Tag == null)
                return;
            string tabName = this.tvAllTable.SelectedNode.Tag.ToString().Trim();
            if (tabName == "")
                return;
            RefreshData(tabName);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            dlg.FileName = "字典表";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                this.gridControl1.ExportToXls(dlg.FileName);
                MessageBox.Show("导出完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
