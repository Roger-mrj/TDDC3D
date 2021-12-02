using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using System.Collections;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Geoprocessor;
using RCIS.Global;
using ESRI.ArcGIS.Controls;
using System.Runtime.InteropServices;

namespace TDDC3D.gengxin
{
    public partial class FrmAKeyProcessing : Form
    {
        public FrmAKeyProcessing()
        {
            InitializeComponent();
        }

        public IWorkspace currWs = null;
        public IMapControl2 pMapCtl = null;

        string xzdm = "";
        string bgData = "";


        private void btnOK_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtXian.Text.Trim()))
            {
                MessageBox.Show("行政区图层为空，无法获取行政区代码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            xzdm = txtXian.Text.Trim();
            bgData = "" + dateEdit1.Text + "/12/31";

            TDDC3D.gengxin.LsGxClass gx = new LsGxClass();
            gx.xzdm = xzdm;
            gx.bgData = bgData;
            gx.pMapCtl = pMapCtl;
            gx.info = info;
            //提取行政区更新层
            if (chkGetXzqGx.Checked)
            {
                gx.UpdateStatus("开始提取行政区更新层！");
                gx.getCJDCQGX();
                gx.getXZQGX();
                //getXzqGx();
                gx.UpdateStatus("行政区更新层提取完毕！");
            }
            //重建更新层标识码
            if (chkBsm.Checked)
            {
                gx.UpdateStatus("开始重排更新层标识码！");
                gx.rebuildBsm();
                gx.UpdateStatus("更新层标识码重排完毕！");
            }
            //重排图斑编号
            if (chkTbbh.Checked)
            {
                gx.UpdateStatus("开始重排更新层图斑编号！");
                gx.rebuildTbbh();
                gx.UpdateStatus("更新层图斑编号重排完毕！");
            }
            //提取过程
            if (chkTqgc.Checked)
            {
                double mj = 0;
                double.TryParse(txtMj.Text,out mj);
                gx.UpdateStatus("开始提取过程！");
                gx.ProduceDLTBGXGC(mj,checkEdit1.Checked);
                gx.ProduceCJDCQGXGC();
                gx.ProduceXZQGXGC();
                gx.UpdateStatus("提取过程完毕！");
            }
            //提取路面范围
            //if (chkLmfw.Checked)
            //{
            //    UpdateStatus("开始提取路面范围！");
            //    getLmfw();
            //    UpdateStatus("路面范围提取完毕！");
            //}
            //提取城镇村等用地更新层
            if (chkCzcgx.Checked)
            {
                gx.UpdateStatus("开始提取城镇村等用地更新层！");
                gx.getCzcgx();
                gx.UpdateStatus("正在融合城镇村等用地更新层。");
                gx.dissoveCzcgx();
                gx.UpdateStatus("正在计算城镇村等用地更新层面积。");
                gx.jsczcmj();
                gx.UpdateStatus("城镇村等用地更新层提取完毕！");
            }
            MessageBox.Show("一键处理完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void FrmAKeyProcessing_Load(object sender, EventArgs e)
        {
            dateEdit1.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            dateEdit1.Properties.ShowToday = false;
            //dateEdit1.Properties.ShowM = false;
            dateEdit1.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dateEdit1.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
            dateEdit1.Properties.Mask.EditMask = "yyyy";
            dateEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
            //string txtData = (System.DateTime.Now.Year - 1).ToString();
            double month = System.DateTime.Now.Month;
            if(month<7)
                dateEdit1.EditValue = System.DateTime.Now.AddYears(-1);
            else
                dateEdit1.EditValue = System.DateTime.Now.AddYears(0);


            //县代码
            IFeatureClass pXZQClass = null;
            try
            {
                pXZQClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQClass != null)
            {
                List<string> dms = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(this.currWs as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                txtXian.Properties.Items.Clear();
                foreach (string dm in dms)
                {
                    txtXian.Properties.Items.Add(dm);
                }
                if (txtXian.Properties.Items.Count > 0) txtXian.SelectedIndex = 0;
            }

            chkCheckAll.CheckState = CheckState.Checked;
        }

        private void chkBhqz_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckAll.Checked)
            {
                chkGetXzqGx.CheckState = CheckState.Checked;
                chkBsm.CheckState = CheckState.Checked;
                chkTbbh.CheckState = CheckState.Checked;
                chkTqgc.CheckState = CheckState.Checked;
                chkCzcgx.CheckState = CheckState.Checked;
                //chkLmfw.CheckState = CheckState.Checked;
            }
            else
            {
                chkGetXzqGx.CheckState = CheckState.Unchecked;
                chkBsm.CheckState = CheckState.Unchecked;
                chkTbbh.CheckState = CheckState.Unchecked;
                chkTqgc.CheckState = CheckState.Unchecked;
                chkCzcgx.CheckState = CheckState.Unchecked;
                //chkLmfw.CheckState = CheckState.Unchecked;

            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                txtMj.Text = "";
                txtMj.Enabled = true;
            }
            else
            {
                txtMj.Text = "";
                txtMj.Enabled = false;
            }
        }
    }
}
