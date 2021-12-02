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

namespace TDDC3D.gdzy
{
    public partial class FrmGetGDData : Form
    {
        public FrmGetGDData()
        {
            InitializeComponent();
        }

        public IWorkspace pCurrWs = null;
        public IMap currMap = null;

        private void FrmGetGDData_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(cmbSourceLay, currMap);
            if (cmbSourceLay.Properties.Items.Contains("DLTB|地类图斑"))
                cmbSourceLay.SelectedItem = "DLTB|地类图斑";
        }

        //private void UpdateStatus(string txt)
        //{
        //    this.memoEdit1.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + memoEdit1.Text;
        //    Application.DoEvents();
        //}

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!chkFL.Checked&&!chkFZ.Checked&&!chkKcfl.Checked&&!chkSD.Checked)
            {
                MessageBox.Show("请选择目标图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (rdoResource.SelectedIndex == 0 && string.IsNullOrWhiteSpace(cmbSourceLay.Text))
            {
                MessageBox.Show("请选择源图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (rdoResource.SelectedIndex == 1 && string.IsNullOrWhiteSpace(txtSHP.Text))
            {
                MessageBox.Show("请选择源图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((pCurrWs as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureDataset, "GDZY"] == false)
            {
                MessageBox.Show("数据库没有升级，请先升级数据库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍后...", "正在提取");
            wait.Show();
            IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
            string tbName=cmbSourceLay.Text;
            tbName=tbName.Substring(0,tbName.IndexOf("|"));

            if (chkFL.Checked)
            {
                bool b = RCIS.GISCommon.GpToolHelper.DeleteFeatures(pCurrWs.PathName + "\\GDZY\\FLDY");
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "DLBM like '01%'";
                if (rdoResource.SelectedIndex == 0)
                {
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, tbName, "FLDY", pQf);
                }
                else
                {
                    IFeatureClass gdFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtSHP.Text);
                    if (gdFeatureClass == null)
                    {
                        MessageBox.Show("外部数据源无法打开。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
                    if (gdFeatureClass.Fields.FindField("dlbm") == -1)
                    {
                        MessageBox.Show("没有地类编码字段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
                    IWorkspace shpWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(txtSHP.Text);
                    string shpName = System.IO.Path.GetFileNameWithoutExtension(txtSHP.Text);
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(shpWorkspace, pTmpWs, shpName, "FLDY", pQf);
                }
                IFeatureClass pFLDY = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("FLDY");
                if (pFLDY.FindField("GDPDJB") != -1)
                {
                    ISchemaLock pSchemaLock = null;
                    pSchemaLock = pFLDY as ISchemaLock;
                    pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
                    IClassSchemaEdit4 pClassSchemaEdit = pFLDY as IClassSchemaEdit4;
                    pClassSchemaEdit.AlterFieldName("GDPDJB", "PDJB");
                    pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                }
                if (pFLDY.FindField("SWDYXJB") != -1)
                {
                    ISchemaLock pSchemaLock = null;
                    pSchemaLock = pFLDY as ISchemaLock;
                    pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
                    IClassSchemaEdit4 pClassSchemaEdit = pFLDY as IClassSchemaEdit4;
                    pClassSchemaEdit.AlterFieldName("SWDYXJB", "TRSWDYXJB");
                    pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                }
                b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\FLDY", pCurrWs.PathName + "\\GDZY\\FLDY");
                if (!b)
                {
                    MessageBox.Show("分类单元数据提取失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    wait.Close();
                    return;
                }
            }
            if (chkKcfl.Checked)
            {
                bool b = RCIS.GISCommon.GpToolHelper.DeleteFeatures(pCurrWs.PathName + "\\GDZY\\KCFLDY");
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "ZZSXDM='GCHF' OR ZZSXDM='JKHF'";
                if (rdoResource.SelectedIndex == 0)
                {
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, tbName, "KCFLDY", pQf);
                }
                else
                {
                    IFeatureClass gdFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtSHP.Text);
                    if (gdFeatureClass == null)
                    {
                        MessageBox.Show("外部数据源无法打开。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
                    if (gdFeatureClass.Fields.FindField("ZZSXDM") == -1)
                    {
                        MessageBox.Show("没有地类编码字段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
                    IWorkspace shpWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(txtSHP.Text);
                    string shpName = System.IO.Path.GetFileNameWithoutExtension(txtSHP.Text);
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(shpWorkspace, pTmpWs, shpName, "KCFLDY", pQf);
                    IFeatureClass pFLDY = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("KCFLDY");
                    if (pFLDY.FindField("TRSWDYXJB") != -1)
                    {
                        ISchemaLock pSchemaLock = null;
                        pSchemaLock = pFLDY as ISchemaLock;
                        pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
                        IClassSchemaEdit4 pClassSchemaEdit = pFLDY as IClassSchemaEdit4;
                        pClassSchemaEdit.AlterFieldName("TRSWDYXJB", "SWDYXJB");
                        pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }
                    
                }
                b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\KCFLDY", pCurrWs.PathName + "\\GDZY\\KCFLDY");
                    if (!b)
                    {
                        MessageBox.Show("扩充分类单元数据提取失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
            }
            if (chkFZ.Checked)
            {
                bool b = RCIS.GISCommon.GpToolHelper.DeleteFeatures(pCurrWs.PathName + "\\GDZY\\FZTB");
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "(dlbm like '02' or dlbm like '03%' or dlbm like '04%' or dlbm like '10%' or dlbm like '11%' or dlbm like '12%' or dlbm='0603') and zzsxdm<>'JKHF' and zzsxdm<>'GCHF'";
                if (rdoResource.SelectedIndex == 0)
                {
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, tbName, "FZTB", pQf);
                }
                else
                {
                    IFeatureClass gdFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtSHP.Text);
                    if (gdFeatureClass == null)
                    {
                        MessageBox.Show("外部数据源无法打开。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
                    if (gdFeatureClass.Fields.FindField("dlbm") == -1 || gdFeatureClass.Fields.FindField("ZZSXDM") == -1)
                    {
                        MessageBox.Show("没有地类编码或种植属性代码字段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        wait.Close();
                        return;
                    }
                    IWorkspace shpWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(txtSHP.Text);
                    string shpName = System.IO.Path.GetFileNameWithoutExtension(txtSHP.Text);
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(shpWorkspace, pTmpWs, shpName, "FZTB", pQf);
                }
                b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\FZTB", pCurrWs.PathName + "\\GDZY\\FZTB");
                if (!b)
                {
                    MessageBox.Show("辅助图斑数据提取失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    wait.Close();
                    return;
                }
            }
            if (chkSD.Checked)
            {
                bool b = RCIS.GISCommon.GpToolHelper.DeleteFeatures(pCurrWs.PathName + "\\GDZY\\SDGDZLDJ");
                b = RCIS.GISCommon.GpToolHelper.Append(pCurrWs.PathName + "\\GDZY\\FLDY", pCurrWs.PathName + "\\GDZY\\SDGDZLDJ");
                if (!b)
                {
                    MessageBox.Show("三调耕地质量等级数据提取失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    wait.Close();
                    return;
                }
            }
            wait.Close();
            MessageBox.Show("耕地数据提取完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoResource_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rdoResource.SelectedIndex)
            {
                case 1:
                    cmbSourceLay.Enabled = false;
                    txtSHP.Enabled = true;
                    break;
                default:
                    cmbSourceLay.Enabled = true;
                    txtSHP.Enabled = false;
                    break;
            }
        }

        private void txtSHP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "shp文件|*.shp";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtSHP.Text = dlg.FileName;
            }
        }

    }
}
