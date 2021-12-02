using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Global;
using RCIS.Utility;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Database;

namespace TDDC3D.output
{
    public partial class FrmOutTable : Form
    {
        public FrmOutTable()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        string zldw = "";

        private void FrmOutTable_Load(object sender, EventArgs e)
        {
            this.beDestDir.Text = AppParameters.OutputPath + "\\Excel";
            LoadTreeFromQsdm();
            zldw = OtherHelper.GetLeftName(this.tvXzq.Nodes[0].Text);
        }

        public string CurrDW
        {
            get
            {
                if (this.rgDanwei.SelectedIndex == 0)
                {
                    return "公顷";
                }
                else
                {
                    return "亩";
                }
            }
        }

        private void LoadTreeFromQsdm()
        {
            this.tvXzq.Nodes.Clear();
            ITable pTable = null;
            if (this.currWs == null)
            {

                return;
            }
            try
            {
                pTable = (this.currWs as IFeatureWorkspace).OpenTable("QSDWDMB");
                List<string> lstXian = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 6);
                List<string> lstXiang = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 9);
                List<string> lstCun = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 12);
                foreach (string aXian in lstXian)
                {
                    TreeNode axianNode = tvXzq.Nodes.Add(aXian);
                    string aXianDm = OtherHelper.GetLeftName(aXian);
                    foreach (string aXiang in lstXiang)
                    {
                        string xiangDm = OtherHelper.GetLeftName(aXiang);
                        if (xiangDm.StartsWith(aXianDm))
                        {
                            TreeNode aXiangNode = axianNode.Nodes.Add(aXiang);
                            foreach (string aCun in lstCun)
                            {
                                string aCunDm = OtherHelper.GetLeftName(aCun);
                                if (aCunDm.StartsWith(xiangDm))
                                {
                                    aXiangNode.Nodes.Add(aCun);
                                }
                            }
                        }

                    }
                }

                if (this.tvXzq.Nodes.Count > 0)
                {
                    this.tvXzq.Nodes[0].Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //全选
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = true;
                }
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = !ce.Checked;
                }
            }
        }
        private Dictionary<string, string> dicQsdwdm = new Dictionary<string, string>();
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.beDestDir.Text.Trim() == "") return;
            //报表输出
            if (this.tvXzq.SelectedNode == null)
            {
                MessageBox.Show("请首先选择某个行政区划。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string zldwdm = OtherHelper.GetLeftName(this.tvXzq.SelectedNode.Text);


            if (dicQsdwdm.Count == 0)
                dicQsdwdm = clsOutputData.getZldwdmMc(currWs);
            DataTable dt = null;// this.GetDataTable(zldwdm, this.radioGroup1.SelectedIndex);

            this.Cursor = Cursors.WaitCursor;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始导出汇总表...", "请稍等...");
            wait.Show();

            try
            {

                if (this.chkTab1.Checked)//一级
                {
                    wait.SetCaption("正在导出表" + this.chkTab1.Text);
                    Application.DoEvents();
                    dt = clsOutputData.GetDataTable(zldwdm, 0);
                    clsOutputData.ExportToExcel1_OneXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab2.Checked)//二级
                {
                    wait.SetCaption("正在导出表" + this.chkTab2.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 1);
                    //clsOutputData.ExportToExcel2_TwoXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);

                    //替换用其他 Excel写入组件，提高速度
                    try
                    {
                        string destfile = clsOutputData.ExportToExcel2_TwoXQTJ2(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                        System.Diagnostics.Process.Start(destfile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (this.chkTab3.Checked)//权属
                {
                    wait.SetCaption("正在导出表" + this.chkTab3.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 2);
                    clsOutputData.ExportToExcel3_QSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab4.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab4.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 3);
                    clsOutputData.ExportToExcel4_CZCGKTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);//城镇村及工况用地
                }
                if (this.chkTab5.Checked)//耕地
                {
                    wait.SetCaption("正在导出表" + this.chkTab5.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 4);
                    clsOutputData.ExportToExcel5_GDPD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab6.Checked) // 种植类型
                {
                    wait.SetCaption("正在导出表" + this.chkTab6.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 5);
                    clsOutputData.ExportToExcel6_GDZZLX(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab7.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab7.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 6);
                    clsOutputData.ExportToExcel7_LQYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);  //林区园地
                }
                if (this.chkTab8.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab8.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 7);
                    clsOutputData.ExportToExcel8_GCXSCD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab9.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab9.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 8);
                    clsOutputData.Exporttoexcel9_GYCCYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab10.Checked)
                {
                    // 可调整
                    wait.SetCaption("正在导出表" + this.chkTab10.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 9);
                    clsOutputData.ExportToExcel10_Ktz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab11.Checked)
                {
                    //部分细化地类
                    wait.SetCaption("正在导出表" + this.chkTab11.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 10);
                    clsOutputData.ExportToExcel11_BfxhdlHzb(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab12.Checked)
                {
                    //耕地细化调查情况统计表
                    wait.SetCaption("正在导出表" + this.chkTab12.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 11);
                    clsOutputData.ExportToExcel12_GDXHTJB(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab13.Checked)
                {
                    //批准未建设
                    wait.SetCaption("正在导出表" + this.chkTab13.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 12);
                    clsOutputData.ExportToExcel13_Pzwjstd(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkTab14.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkTab14.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 13);
                    clsOutputData.ExportToExcel14_PzwjsXz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFRD1.Checked)//飞地一级
                {
                    wait.SetCaption("正在导出表" + this.chkFRD1.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 14);
                    clsOutputData.ExportToExcel15_OneFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFrd2.Checked)//飞地二级
                {
                    wait.SetCaption("正在导出表" + this.chkFrd2.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 15);
                    clsOutputData.ExportToExcel16_TwoFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFrd3.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkFrd3.Text);
                    //飞地权属
                    dt = clsOutputData.GetDataTable(zldwdm, 16);
                    clsOutputData.ExportToExcel17_FDQSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkFrd4.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkFrd4.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 17);
                    clsOutputData.ExportToExcel18_FD_CZCTable(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkHd1.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkHd1.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 18);
                    clsOutputData.ExportToExcel19_OneHDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkHd2.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkHd2.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 19);
                    clsOutputData.ExportToExclel20_HDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkYjjbntXz.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkYjjbntXz.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 20);
                    clsOutputData.ExportToExcel20_Yjjbnt(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkWrHdxz.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkWrHdxz.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 21);
                    clsOutputData.ExportToExcel21_WJMHD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);

                }
                if (this.chkFQLJTM.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkFQLJTM.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 22);
                    clsOutputData.ExportToExclel22_FQLJTM(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                if (this.chkGDHF.Checked)
                {
                    wait.SetCaption("正在导出表" + this.chkGDHF.Text);
                    dt = clsOutputData.GetDataTable(zldwdm, 23);
                    clsOutputData.ExportToExclel23_JKHFGCHF(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                }
                wait.Close();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();

                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Default;
            }
            
        }

        private void btnOutXlsAll_Click(object sender, EventArgs e)
        {
            //输出所有报表
            if (this.beDestDir.Text.Trim() == "") return;
            //报表输出
            if (this.tvXzq.SelectedNode == null)
            {
                MessageBox.Show("请首先选择某个行政区划。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string zldwdm = OtherHelper.GetLeftName(this.tvXzq.SelectedNode.Text);

            this.Cursor = Cursors.WaitCursor;
            if (dicQsdwdm.Count == 0)
                dicQsdwdm = clsOutputData.getZldwdmMc(currWs);
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始导出...", "正在导出，请稍候...");
            wait.Show();
            try
            {
                for (int i = 0; i <= 21; i++)
                {
                    wait.SetCaption("正在导出第" + i + "个表...");
                    Application.DoEvents();
                    DataTable dt = clsOutputData.GetDataTable(zldwdm, i);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        continue;
                    }
                    switch (i)
                    {
                        case 0:
                            clsOutputData.ExportToExcel1_OneXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 1:
                            clsOutputData.ExportToExcel2_TwoXQTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 2:
                            clsOutputData.ExportToExcel3_QSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 3:
                            clsOutputData.ExportToExcel4_CZCGKTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 4:
                            clsOutputData.ExportToExcel5_GDPD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 5:
                            clsOutputData.ExportToExcel6_GDZZLX(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 6:
                            clsOutputData.ExportToExcel7_LQYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 7:
                            clsOutputData.ExportToExcel8_GCXSCD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 8:
                            clsOutputData.Exporttoexcel9_GYCCYD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 9:
                            clsOutputData.ExportToExcel10_Ktz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 10:
                            clsOutputData.ExportToExcel11_BfxhdlHzb(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 11:
                            clsOutputData.ExportToExcel12_GDXHTJB(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 12:
                            clsOutputData.ExportToExcel13_Pzwjstd(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 13:
                            clsOutputData.ExportToExcel14_PzwjsXz(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 14:
                            clsOutputData.ExportToExcel15_OneFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 15:
                            clsOutputData.ExportToExcel16_TwoFDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 16:
                            clsOutputData.ExportToExcel17_FDQSXZ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 17:
                            clsOutputData.ExportToExcel18_FD_CZCTable(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 18:
                            clsOutputData.ExportToExcel19_OneHDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 19:
                            clsOutputData.ExportToExclel20_HDTJ(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 20:
                            clsOutputData.ExportToExcel20_Yjjbnt(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;
                        case 21:
                            clsOutputData.ExportToExcel21_WJMHD(dt, beDestDir.Text, CurrDW, zldwdm, dicQsdwdm, true);
                            break;

                    }
                }

                this.Cursor = Cursors.Default;
                wait.Close();
                MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);

            }
            
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrWhiteSpace(buttonEdit1.Text))
            {
                MessageBox.Show("请选择变更基础表路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }

            if (string.IsNullOrWhiteSpace(txtKzmjHD.Text) || string.IsNullOrWhiteSpace(txtKzmjLD.Text))
            {
                MessageBox.Show("请输入控制面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            this.Cursor = Cursors.WaitCursor;
            this.listBoxControl1.Items.Clear();
            //try
            //{
                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":开始从矢量中提取地类图斑数据...");
                Application.DoEvents();

                if (System.IO.File.Exists(Application.StartupPath + @"\SystemConf\backup.mdb"))
                {
                    System.IO.File.Delete(Application.StartupPath + @"\SystemConf\result.mdb");
                    System.IO.File.Copy(Application.StartupPath + @"\SystemConf\backup.mdb", Application.StartupPath + @"\SystemConf\result.mdb");
                }
                else
                {
                    //执行，删除已有图层
                    clsOutputData.DeletDLTBXZQ();
                    clsOutputData.CompactResultMdb();
                }

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在提取数据...");
                Application.DoEvents();
                //if (!clsOutputData.SpatialJoin(currWs))
                //{
                //    this.Cursor = Cursors.Default;
                //    MessageBox.Show("空间关系处理不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                clsOutputData.CopyDltb(currWs);
                //clsOutputData.CopyJBNT(currWs);

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在初步进行平方米统计...");
                Application.DoEvents();
                clsOutputData.Dltb2BaseTable2();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在进行生成基础统计...");
                Application.DoEvents();
                clsOutputData.ChangeTMP2JCB("PMMJ");

                double ldmj = 0;
                double.TryParse(txtKzmjLD.Text, out ldmj);
                double hdmj = 0;
                double.TryParse(txtKzmjHD.Text, out hdmj);

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在进行调平.....");
                Application.DoEvents();
                ClsTP tp = new ClsTP();
                tp.ChangeTableDW2GQ("HZ_JCB");  //转化为公顷
                tp.MakeBalance("00", ldmj);
                tp.MakeBalance("01", hdmj);

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按辖区坐落汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按辖区坐落汇总统计...";
                clsOutputData.InitZlTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按权属性质汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按权属性质汇总统计...";
                clsOutputData.InitQsTable();
                alterTDQS(buttonEdit1.Text + "//2020("+zldw+")土地利用现状一级分类面积按权属性质变化统计表.xls");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按耕地坡度汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按耕地坡度汇总统计...";
                clsOutputData.InitGdTable();
                alterGDPD(buttonEdit1.Text + "//2020(" + zldw + ")耕地坡度分级面积变化统计表.xlsx");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将城镇村及工矿用地面积汇总表统计...");
                Application.DoEvents();
                clsOutputData.InitCZCGKTable();
                alterCZC(buttonEdit1.Text + "//2020(" + zldw + ")城镇村及工矿用地面积变化统计表.xlsx");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按耕地种植类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGdZzlxTable();
                alterGDZZLX(buttonEdit1.Text + "//2020(" + zldw + ")耕地种植类型面积变化统计表.xlsx");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行林区园地汇总统计...");
                Application.DoEvents();
                clsOutputData.InitLQYDTable();
                alterLQYD(buttonEdit1.Text + "//2020(" + zldw + ")林区范围内种植园用地变化统计表.xlsx");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行灌丛草地汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGCCDXSCDTable();
                alterGCCD(buttonEdit1.Text + "//2020(" + zldw + ")灌丛草地汇总情况变化统计表.xls");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行工业用地类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGYCCTable();
                alterGYYD(buttonEdit1.Text + "//2020(" + zldw + ")工业用地按类型汇总变化统计表.xls");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行耕地细化调查按类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGdxhdcTable();
                alterGDXH(buttonEdit1.Text + "//2020(" + zldw + ")耕地细化调查情况变化统计表.xls");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行可调整地类面积统计...");
                Application.DoEvents();
                clsOutputData.InitKtzTable();
                alterKTZDL(buttonEdit1.Text + "//2020(" + zldw + ")可调整地类面积变化统计表.xls");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将提取批准未建设数据...");
                Application.DoEvents();
                clsStatsPzwjs pzwjs = new clsStatsPzwjs(this.currWs);
                pzwjs.getPzwjsTmp();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将汇总批准未建设基础表...");
                Application.DoEvents();
                pzwjs.InitPzwjsJCB();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将批准未建设现状情况统计表进行汇总统计...");
                Application.DoEvents();
                pzwjs.initPzwjsXzBzTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将批准未建设建设用地用途情况进行统计...");
                Application.DoEvents();
                pzwjs.InitPzwjsBZTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按飞地汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按飞地汇总统计...";
                clsOutputData.InitFdTable();
                clsOutputData.InitFDQSTable();  //飞地权属
                clsOutputData.InitFd_CZCGKTable();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将基础表数据进行按海岛面积汇总统计...");
                Application.DoEvents();
                clsOutputData.InitHDTable();

                clsStatsWRHD hdtj = new clsStatsWRHD(this.currWs);
                hdtj.InitWjmHd();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将进行部分地类细化汇总表...");
                Application.DoEvents();
                clsOutputData.InitBfxhdlTable();
                alterBFXH(buttonEdit1.Text + "//2020(" + zldw + ")部分细化地类面积变化统计表.xlsx");


                //this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在提取永久基本农田图斑数据表...");
                //Application.DoEvents();
                //clsStatsYjjbnt yjjbnt = new clsStatsYjjbnt(this.currWs);
                //yjjbnt.getYjjbntTmp();
                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将汇总永久基本农田数据并统计...");
                Application.DoEvents();
                //yjjbnt.InitYjjbntJCB();
                //yjjbnt.initYjjbntXzBzTable();
                clsStatsJbnt2 statJbnt = new clsStatsJbnt2(this.currWs);
                statJbnt.getYJJBNTTmp();
                statJbnt.InitYjjbnt();

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将进行废弃与垃圾填埋细化标注汇总...");
                Application.DoEvents();
                clsOutputData.InitFQLJTMTable();
                alterLJTM(buttonEdit1.Text + "//2020(" + zldw + ")废弃与垃圾填埋细化标注变化统计表.xlsx");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":正在将进行即可恢复与工程恢复种植属性汇总...");
                Application.DoEvents();
                clsOutputData.InitJKHFGCHFTable();
                alterJKHFGCHF(buttonEdit1.Text + "//2020(" + zldw + ")即可恢复与工程恢复种植属性变化统计表.xls");

                this.listBoxControl1.Items.Insert(0, DateTime.Now.ToShortTimeString() + ":基础数据完成初始化。.");


                this.Cursor = Cursors.Default;
                MessageBox.Show("初始化基础库完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //    MessageBox.Show(ex.Message);
            //}
            

        }

        private static Aspose.Cells.Row readExcel(string excelPath, int rowIndex) 
        {
            Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelPath);
            Aspose.Cells.Worksheet ws = wk.Worksheets[0];
            Aspose.Cells.Row pRow = ws.Cells.GetRow(rowIndex);
            return pRow;
        }

        

        private void alterTDQS(string excelName) 
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 11);
            List<string> fields = new List<string>() { "D00G","D00J","D01G", "D01J", "D02G", "D02J", "D03G", "D03J", "D04G", "D04J", "D05G", "D05J", "D06G", "D06J", "D07G", "D07J", "D08G", "D08J", "D09G", "D09J", "D10G", "D10J", "D11G", "D11J", "D12G", "D12J" };
            List<int> columnIndex = new List<int>() { 5,6,8,9,11,12,14,15,17,18,20,21,23,24,26,27,29,30,32,33,35,36,38,39,41,42 };
            string tabName = "HZ_QS_BZ";
            Leveling(pRow, tabName, fields, columnIndex);
            //计算合计
            StringBuilder sb = new StringBuilder();
            sb.Append(" update HZ_QS_BZ set  D00=iif(isnull(D00G),0,D00G)+iif(isnull(D00J),0,D00J),  D01=iif(isnull(D01G),0,D01G)+iif(isnull(D01J),0,D01J),")
                .Append(" D02=iif(isnull(D02G),0,D02G)+iif(isnull(D02J),0,D02J),")
                .Append(" D03=iif(isnull(D03G),0,D03G)+iif(isnull(D03J),0,D03J),")
                .Append(" D04=iif(isnull(D04G),0,D04G)+iif(isnull(D04J),0,D04J),")
                 .Append(" D05=iif(isnull(D05G),0,D05G)+iif(isnull(D05J),0,D05J),")
                  .Append(" D06=iif(isnull(D06G),0,D06G)+iif(isnull(D06J),0,D06J),")
                 .Append(" D07=iif(isnull(D07G),0,D07G)+iif(isnull(D07J),0,D07J),")
                 .Append(" D08=iif(isnull(D08G),0,D08G)+iif(isnull(D08J),0,D08J),")
                 .Append(" D09=iif(isnull(D09G),0,D09G)+iif(isnull(D09J),0,D09J),")
                .Append(" D10=iif(isnull(D10G),0,D10G)+iif(isnull(D10J),0,D10J),")
                .Append(" D11=iif(isnull(D11G),0,D11G)+iif(isnull(D11J),0,D11J),")
                .Append(" D12=iif(isnull(D12G),0,D12G)+iif(isnull(D12J),0,D12J) ");
            string sql = sb.ToString();
            int iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append(" update HZ_QS_BZ set  TOTALAREAG=iif(isnull(D00G),0,D00G)+iif(isnull(D01G),0,D01G)")
                .Append(" +iif(isnull(D02G),0,D02G)")
                .Append(" +iif(isnull(D03G),0,D03G)")
                .Append(" +iif(isnull(D04G),0,D04G)")
                 .Append(" +iif(isnull(D05G),0,D05G)")
                  .Append(" +iif(isnull(D06G),0,D06G)")
                 .Append(" +iif(isnull(D07G),0,D07G)")
                 .Append(" +iif(isnull(D08G),0,D08G)")
                 .Append(" +iif(isnull(D09G),0,D09G)")
                .Append(" +iif(isnull(D10G),0,D10G)")
                .Append(" +iif(isnull(D11G),0,D11G)")
                .Append(" +iif(isnull(D12G),0,D12G)");
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

            sb = new StringBuilder();
            sb.Append(" update HZ_QS_BZ set  TOTALAREAJ=iif(isnull(D00J),0,D00J)+iif(isnull(D01G),0,D01J)")
                .Append(" +iif(isnull(D02J),0,D02J)")
                .Append(" +iif(isnull(D03J),0,D03J)")
                .Append(" +iif(isnull(D04J),0,D04J)")
                 .Append(" +iif(isnull(D05J),0,D05J)")
                  .Append(" +iif(isnull(D06J),0,D06J)")
                 .Append(" +iif(isnull(D07J),0,D07J)")
                 .Append(" +iif(isnull(D08J),0,D08J)")
                 .Append(" +iif(isnull(D09J),0,D09J)")
                .Append(" +iif(isnull(D10J),0,D10J)")
                .Append(" +iif(isnull(D11J),0,D11J)")
                .Append(" +iif(isnull(D12J),0,D12J)");
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

            sql = "update HZ_QS_BZ set TOTALAREA=TOTALAREAG+TOTALAREAJ";
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //逐级汇总
            sb = new StringBuilder();
            sb.Append(" insert into HZ_QS_BZ(ZLDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D00G,D00J,D01,D01G,D01J,D02,D02G,D02J,")
            .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

            .Append(" select left(ZLDWDM,9),sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D00G),sum(D00J),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
            .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
            .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
            .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_QS_BZ ")
            .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,9)");
            sql = sb.ToString();
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append(" insert into HZ_QS_BZ(ZLDWDM,TOTALAREA,TOTALAREAG,TOTALAREAJ,D00,D00G,D00J,D01,D01G,D01J,D02,D02G,D02J,")
            .Append("D03,D03G,D03J,D04,D04G,D04J,D05,D05G,D05J,D06,D06G,D06J,D07,D07G,D07J,D08,D08G,D08J,D09,D09G,D09J,D10,D10G,D10J,D11,D11G,D11J,D12,D12G,D12J ) ")

            .Append(" select left(ZLDWDM,6),sum(TOTALAREA),sum(TOTALAREAG),sum(TOTALAREAJ),sum(D00),sum(D00G),sum(D00J),sum(D01),sum(D01G),sum(D01J),sum(D02),sum(D02G),sum(D02J),")
            .Append("sum(D03),sum(D03G),sum(D03J),sum(D04),sum(D04G),sum(D04J),sum(D05),sum(D05G),sum(D05J),")
            .Append("sum(D06),sum(D06G),sum(D06J),sum(D07),sum(D07G),sum(D07J),sum(D08),sum(D08G),sum(D08J),sum(D09),sum(D09G),sum(D09J),")
            .Append(" sum(D10),sum(D10G),sum(D10J),sum(D11),sum(D11G),sum(D11J),sum(D12),sum(D12G),sum(D12J) from HZ_QS_BZ ")
            .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,6)");
            sql = sb.ToString();
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = "update HZ_QS_BZ set TOTALAREAG=iif(isnull(D00G),0,D00G)+iif(isnull(D01G),0,D01G)+iif(isnull(D02G),0,D02G)+iif(isnull(D03G),0,D03G)+iif(isnull(D04G),0,D04G)+iif(isnull(D05G),0,D05G)+iif(isnull(D06G),0,D06G)+iif(isnull(D07G),0,D07G)+iif(isnull(D08G),0,D08G)+iif(isnull(D09G),0,D09G)+iif(isnull(D10G),0,D10G)+iif(isnull(D11G),0,D11G)+iif(isnull(D12G),0,D12G)";
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = "update HZ_QS_BZ set TOTALAREAJ=iif(isnull(D00J),0,D00J)+iif(isnull(D01J),0,D01J)+iif(isnull(D02J),0,D02J)+iif(isnull(D03J),0,D03J)+iif(isnull(D04J),0,D04J)+iif(isnull(D05J),0,D05J)+iif(isnull(D06J),0,D06J)+iif(isnull(D07J),0,D07J)+iif(isnull(D08J),0,D08J)+iif(isnull(D09J),0,D09J)+iif(isnull(D10J),0,D10J)+iif(isnull(D11J),0,D11J)+iif(isnull(D12J),0,D12J)";
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = "update HZ_QS_BZ set TOTALAREA=iif(isnull(TOTALAREAG),0,TOTALAREAG)+iif(isnull(TOTALAREAJ),0,TOTALAREAJ)";
            iresult = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterGDPD(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 11);
            List<string> fields = new List<string>() { "D2", "D26T", "D26P", "D615T", "D615P", "D1525T", "D1525P", "D25T", "D25P" };
            List<int> columnIndex = new List<int>() { 2, 4, 5, 7, 8, 10, 11, 13, 14 };
            string tabName = "HZ_GD_BZ";
            Leveling(pRow, tabName, fields, columnIndex);
            //计算小计
            string sql = "update HZ_GD_BZ set D26=iif(isnull(D26T),0,D26T)+iif(isnull(D26P),0,D26P),D615=iif(isnull(D615T),0,D615T)+iif(isnull(D615P),0,D615P),"
                + " D1525=iif(isnull(D1525T),0,D1525T)+iif(isnull(D1525P),0,D1525P),D25=iif(isnull(D25P),0,D25P)+iif(isnull(D25T),0,D25T) ";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = " insert into HZ_GD_BZ(ZLDWDM,TOTALAREA,D2,D26,D26T,D26P,D615,D615T,D615P,D1525,D1525T,D1525P,D25,D25T,D25P) "
            + " select left(ZLDWDM,9), sum(TOTALAREA),sum(D2),sum(D26),sum(D26T),sum(D26P),sum(D615),sum(D615T),sum(D615P),sum(D1525),sum(D1525T),sum(D1525P),sum(D25),sum(D25T),sum(D25P) from HZ_GD_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,9)";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = "insert into HZ_GD_BZ(ZLDWDM,TOTALAREA,D2,D26,D26T,D26P,D615,D615T,D615P,D1525,D1525T,D1525P,D25,D25T,D25P) "
                + " select left(ZLDWDM,6), sum(TOTALAREA),sum(D2),sum(D26),sum(D26T),sum(D26P),sum(D615),sum(D615T),sum(D615P),sum(D1525),sum(D1525T),sum(D1525P),sum(D25),sum(D25T),sum(D25P) from HZ_GD_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,6)";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = "update HZ_GD_BZ set TOTALAREA=iif(isnull(D2),0,D2)+iif(isnull(D26),0,D26)+iif(isnull(D615),0,D615)+iif(isnull(D1525),0,D1525)+iif(isnull(D25),0,D25)";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void alterGDZZLX(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "D0101LS", "D0101FLS", "D0101LYFL", "D0101XG", "D0101LLJZ", "D0101WG", "D0102LS", "D0102FLS", "D0102LYFL", "D0102XG", "D0102LLJZ", "D0102WG", "D0103LS", "D0103FLS", "D0103LYFL", "D0103XG", "D0103LLJZ", "D0103WG" };
            List<int> columnIndex = new List<int>() { 9,10,11,12,13,14,16,17,18,19,20,21,23,24,25,26,27,28 };
            string tabName = "HZ_GDZZLX_BZ";
            Leveling(pRow, tabName, fields, columnIndex);
            //iif(isnull(D0101WG),0,D0101WG)
            string sql = @"update HZ_GDZZLX_BZ set D0101=iif(isnull(D0101LS),0,D0101LS)+iif(isnull(D0101FLS),0,D0101FLS)+iif(isnull(D0101LYFL),0,D0101LYFL)+iif(isnull(D0101XG),0,D0101XG)+iif(isnull(D0101LLJZ),0,D0101LLJZ)+iif(isnull(D0101WG),0,D0101WG),
                                                   D0102=iif(isnull(D0102LS),0,D0102LS)+iif(isnull(D0102FLS),0,D0102FLS)+iif(isnull(D0102LYFL),0,D0102LYFL)+iif(isnull(D0102XG),0,D0102XG)+iif(isnull(D0102LLJZ),0,D0102LLJZ)+iif(isnull(D0102WG),0,D0102WG),
                                                   D0103=iif(isnull(D0103LS),0,D0103LS)+iif(isnull(D0103FLS),0,D0103FLS)+iif(isnull(D0103LYFL),0,D0103LYFL)+iif(isnull(D0103XG),0,D0103XG)+iif(isnull(D0103LLJZ),0,D0103LLJZ)+iif(isnull(D0103WG),0,D0103WG),
                                                   D01LS=iif(isnull(D0101LS),0,D0101LS)+iif(isnull(D0102LS),0,D0102LS)+iif(isnull(D0103LS),0,D0103LS),
                                                   D01FLS=iif(isnull(D0101FLS),0,D0101FLS)+iif(isnull(D0102FLS),0,D0102FLS)+iif(isnull(D0103FLS),0,D0103FLS),
                                                   D01LYFL=iif(isnull(D0101LYFL),0,D0101LYFL)+iif(isnull(D0102LYFL),0,D0102LYFL)+iif(isnull(D0103LYFL),0,D0103LYFL),
                                                   D01XG=iif(isnull(D0101XG),0,D0101XG)+iif(isnull(D0102XG),0,D0102XG)+iif(isnull(D0103XG),0,D0103XG),
                                                   D01LLJZ=iif(isnull(D0101LLJZ),0,D0101LLJZ)+iif(isnull(D0102LLJZ),0,D0102LLJZ)+iif(isnull(D0103LLJZ),0,D0103LLJZ),
                                                   D01WG=iif(isnull(D0101WG),0,D0101WG)+iif(isnull(D0102WG),0,D0102WG)+iif(isnull(D0103WG),0,D0103WG)";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = @"update HZ_GDZZLX_BZ set D01=D01LS+D01FLS+D01LYFL+D01XG+D01LLJZ+D01WG";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //乡镇级汇总
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_GDZZLX_BZ(ZLDWDM,D01,D01LS,D01FLS,D01LYFL,D01XG,D01LLJZ,D01WG,")
            .Append(" D0101,D0101LS,D0101FLS,D0101LYFL,D0101XG,D0101LLJZ,D0101WG,")
            .Append(" D0102,D0102LS,D0102FLS,D0102LYFL,D0102XG,D0102LLJZ,D0102WG,")
            .Append(" D0103,D0103LS,D0103FLS,D0103LYFL,D0103XG,D0103LLJZ,D0103WG) ")
            .Append(" select left(ZLDWDM,9), sum(D01),sum(D01LS),sum(D01FLS),sum(D01LYFL),sum(D01XG),sum(D01LLJZ),sum(D01WG),")
            .Append(" sum(D0101),sum(D0101LS),sum(D0101FLS),sum(D0101LYFL),sum(D0101XG),sum(D0101LLJZ),sum(D0101WG),")
            .Append(" sum(D0102),sum(D0102LS),sum(D0102FLS),sum(D0102LYFL),sum(D0102XG),sum(D0102LLJZ),sum(D0102WG),")
            .Append(" sum(D0103),sum(D0103LS),sum(D0103FLS),sum(D0103LYFL),sum(D0103XG),sum(D0103LLJZ),sum(D0103WG) ")
            .Append(" from HZ_GDZZLX_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //县级汇总
            sb = new StringBuilder();
            sb.Append("insert into HZ_GDZZLX_BZ(ZLDWDM,D01,D01LS,D01FLS,D01LYFL,D01XG,D01LLJZ,D01WG,")
            .Append(" D0101,D0101LS,D0101FLS,D0101LYFL,D0101XG,D0101LLJZ,D0101WG,")
            .Append(" D0102,D0102LS,D0102FLS,D0102LYFL,D0102XG,D0102LLJZ,D0102WG,")
            .Append(" D0103,D0103LS,D0103FLS,D0103LYFL,D0103XG,D0103LLJZ,D0103WG) ")
            .Append(" select left(ZLDWDM,6), sum(D01),sum(D01LS),sum(D01FLS),sum(D01LYFL),sum(D01XG),sum(D01LLJZ),sum(D01WG),")
            .Append(" sum(D0101),sum(D0101LS),sum(D0101FLS),sum(D0101LYFL),sum(D0101XG),sum(D0101LLJZ),sum(D0101WG),")
            .Append(" sum(D0102),sum(D0102LS),sum(D0102FLS),sum(D0102LYFL),sum(D0102XG),sum(D0102LLJZ),sum(D0102WG),")
            .Append(" sum(D0103),sum(D0103LS),sum(D0103FLS),sum(D0103LYFL),sum(D0103XG),sum(D0103LLJZ),sum(D0103WG) ")
            .Append(" from HZ_GDZZLX_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void alterLQYD(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "D0201", "D0202", "D0203", "D0204" };
            List<int> columnIndex = new List<int>() { 2,3,4,5 };
            string tabName = "HZ_LQFWNYD_BZ";
            Leveling(pRow, tabName, fields, columnIndex);

            string sql = @"update HZ_LQFWNYD_BZ set D02=D0201+D0202+D0203+D0204";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
           
            //逐级汇总到乡
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_LQFWNYD_BZ(ZLDWDM,D02,D0201,D0202,D0203,D0204) ")
                .Append("select left(ZLDWDM,9),sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204) from  HZ_LQFWNYD_BZ ")
                .Append(" where  len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //汇总到县
            sb = new StringBuilder();
            sb.Append("insert into HZ_LQFWNYD_BZ(ZLDWDM,D02,D0201,D0202,D0203,D0204) ")
                .Append("select left(ZLDWDM,6),sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204) from  HZ_LQFWNYD_BZ ")
                .Append(" where  len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void alterGCCD(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "DGCCD0401", "DGCCD0402", "DGCCD0403", "DGCCD0404" };
            List<int> columnIndex = new List<int>() { 2, 3, 4, 5 };
            string tabName = "HZ_GCXSCD_BZ";
            Leveling(pRow, tabName, fields, columnIndex);

            string sql = @"update HZ_GCXSCD_BZ set DGCCD=DGCCD0401+DGCCD0402+DGCCD0403+DGCCD0404";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_GCXSCD_BZ(zldwdm,DGCCD,DGCCD0401,DGCCD0402,DGCCD0403,DGCCD0404 ) ")
                .Append(" select left(ZLDWDM,9),sum(DGCCD),sum(DGCCD0401),sum(DGCCD0402),sum(DGCCD0403),sum(DGCCD0404)")
               .Append("  from HZ_GCXSCD_BZ ")
               .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //胡总到县
            sb = new StringBuilder();
            sb.Append("insert into HZ_GCXSCD_BZ(zldwdm,DGCCD,DGCCD0401,DGCCD0402,DGCCD0403,DGCCD0404) ")
                .Append(" select left(ZLDWDM,6),sum(DGCCD),sum(DGCCD0401),sum(DGCCD0402),sum(DGCCD0403),sum(DGCCD0404)")
               .Append("  from HZ_GCXSCD_BZ ")
               .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterGYYD(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "DHDGY", "DGTGY", "DMTGY", "DSNGY", "DBLGY", "DDLGY" };
            List<int> columnIndex = new List<int>() { 2, 3, 4, 5,6,7 };
            string tabName = "HZ_GYCC_BZ";
            Leveling(pRow, tabName, fields, columnIndex);

            string sql = @"update HZ_GYCC_BZ set D0601=DHDGY+DGTGY+DMTGY+DSNGY+DBLGY+DDLGY";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_GYCC_BZ (ZLDWDM,D0601,DHDGY,DGTGY,DMTGY,DSNGY,DBLGY,DDLGY) ")
              .Append(" select left(ZLDWDM,9),sum(D0601),sum(DHDGY),sum(DGTGY),sum(DMTGY),sum(DSNGY),sum(DBLGY),sum(DDLGY) from HZ_GYCC_BZ ")
              .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            //县级
            sb = new StringBuilder();
            sb.Append("insert into HZ_GYCC_BZ (ZLDWDM,D0601,DHDGY,DGTGY,DMTGY,DSNGY,DBLGY,DDLGY) ")
              .Append(" select left(ZLDWDM,6),sum(D0601),sum(DHDGY),sum(DGTGY),sum(DMTGY),sum(DSNGY),sum(DBLGY),sum(DDLGY) from HZ_GYCC_BZ ")
              .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterGDXH(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "DHDGD0101", "DHDGD0102", "DHDGD0103", "DHQGD0101", "DHQGD0102", "DHQGD0103", "DLQGD0101", "DLQGD0102", "DLQGD0103", "DMQGD0101", "DMQGD0102", "DMQGD0103", "DSHGD0101", "DSHGD0102", "DSHGD0103", "DSMGD0101", "DSMGD0102", "DSMGD0103", "DYJGD0101", "DYJGD0102", "DYJGD0103" };
            List<int> columnIndex = new List<int>() { 3, 4, 5, 7,8,9,11,12,13,15,16,17,19,20,21,23,24,25,27,28,29 };
            string tabName = "HZ_GDXHDCTJ_BZ";
            Leveling(pRow, tabName, fields, columnIndex);

            string sql = @"update HZ_GDXHDCTJ_BZ set DHDGD01=DHDGD0101+DHDGD0102+DHDGD0103,
                                                     DHQGD01=DHQGD0101+DHQGD0102+DHQGD0103,
                                                     DLQGD01=DLQGD0101+DLQGD0102+DLQGD0103,
                                                     DMQGD01=DMQGD0101+DMQGD0102+DMQGD0103,
                                                     DSHGD01=DSHGD0101+DSHGD0102+DSHGD0103,
                                                     DSMGD01=DSMGD0101+DSMGD0102+DSMGD0103,
                                                     DYJGD01=DYJGD0101+DYJGD0102+DYJGD0103";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "update HZ_GDXHDCTJ_BZ set D01=DHDGD01+DHQGD01+DLQGD01+DMQGD01+DSHGD01+DSMGD01+DYJGD01";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_GDXHDCTJ_BZ(ZLDWDM,D01,DHDGD01,DHDGD0101,DHDGD0102,DHDGD0103,DHQGD01,DHQGD0101,DHQGD0102,DHQGD0103,")
                .Append(" DLQGD01,DLQGD0101,DLQGD0102,DLQGD0103,DMQGD01,DMQGD0101,DMQGD0102,DMQGD0103,DSHGD01,DSHGD0101,DSHGD0102,DSHGD0103,DSMGD01,DSMGD0101,DSMGD0102,DSMGD0103,DYJGD01,DYJGD0101,DYJGD0102,DYJGD0103 ) ")
                .Append(" select left(ZLDWDM,9),sum(D01),sum(DHDGD01),sum(DHDGD0101),sum(DHDGD0102),sum(DHDGD0103),sum(DHQGD01),sum(DHQGD0101),sum(DHQGD0102),sum(DHQGD0103),")
                .Append(" sum(DLQGD01),sum(DLQGD0101),sum(DLQGD0102),sum(DLQGD0103),sum(DMQGD01),sum(DMQGD0101),sum(DMQGD0102),sum(DMQGD0103),sum(DSHGD01),sum(DSHGD0101),sum(DSHGD0102),sum(DSHGD0103), ")
                .Append(" sum(DSMGD01),sum(DSMGD0101),sum(DSMGD0102),sum(DSMGD0103) ")
                .Append(",sum(DYJGD01),sum(DYJGD0101),sum(DYJGD0102),sum(DYJGD0103) ")
                .Append(" from HZ_GDXHDCTJ_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("insert into HZ_GDXHDCTJ_BZ(ZLDWDM,D01,DHDGD01,DHDGD0101,DHDGD0102,DHDGD0103,DHQGD01,DHQGD0101,DHQGD0102,DHQGD0103,")
                .Append(" DLQGD01,DLQGD0101,DLQGD0102,DLQGD0103,DMQGD01,DMQGD0101,DMQGD0102,DMQGD0103,DSHGD01,DSHGD0101,DSHGD0102,DSHGD0103,DSMGD01,DSMGD0101,DSMGD0102,DSMGD0103,DYJGD01,DYJGD0101,DYJGD0102,DYJGD0103 ) ")
                .Append(" select left(ZLDWDM,6),sum(D01),sum(DHDGD01),sum(DHDGD0101),sum(DHDGD0102),sum(DHDGD0103),sum(DHQGD01),sum(DHQGD0101),sum(DHQGD0102),sum(DHQGD0103),")
                .Append(" sum(DLQGD01),sum(DLQGD0101),sum(DLQGD0102),sum(DLQGD0103),sum(DMQGD01),sum(DMQGD0101),sum(DMQGD0102),sum(DMQGD0103),sum(DSHGD01),sum(DSHGD0101),sum(DSHGD0102),sum(DSHGD0103), ")
                .Append(" sum(DSMGD01),sum(DSMGD0101),sum(DSMGD0102),sum(DSMGD0103) ")
                .Append(",sum(DYJGD01),sum(DYJGD0101),sum(DYJGD0102),sum(DYJGD0103) ")
                .Append(" from HZ_GDXHDCTJ_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterKTZDL(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "D0201K", "D0202K", "D0203K", "D0204K", "D0301K", "D0302K", "D0307K", "D0403K", "D1104K" };
            List<int> columnIndex = new List<int>() { 2,3, 4, 5, 6,7, 8, 9, 10 };
            string tabName = "HZ_JBNTWKTZ_BZ";
            Leveling(pRow, tabName, fields, columnIndex);


            string sql = "update HZ_JBNTWKTZ_BZ set DKHJ=D0201K+D0202K+D0203K+D0204K+D0301K+D0302K+D0307K+D0403K+D1104K";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //乡级
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HZ_JBNTWKTZ_BZ(ZLDWDM,DKHJ,D0201K,D0202K,D0203K,D0204K,D0301K,D0302K,D0307K,D0403K,D1104K) ")
                .Append(" select left(ZLDWDM,9),sum(DKHJ),sum(D0201K),sum(D0202K),sum(D0203K),sum(D0204K),sum(D0301K),sum(D0302K),sum(D0307K),sum(D0403K),sum(D1104K) ")
                .Append(" from HZ_JBNTWKTZ_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("insert into HZ_JBNTWKTZ_BZ(ZLDWDM,DKHJ,D0201K,D0202K,D0203K,D0204K,D0301K,D0302K,D0307K,D0403K,D1104K) ")
                .Append(" select left(ZLDWDM,6),sum(DKHJ),sum(D0201K),sum(D0202K),sum(D0203K),sum(D0204K),sum(D0301K),sum(D0302K),sum(D0307K),sum(D0403K),sum(D1104K) ")
                .Append(" from HZ_JBNTWKTZ_BZ where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterBFXH(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "D08H2A", "D0810A", "D1104A", "D1107A", "D201A", "D202A", "D203A" };
            List<int> columnIndex = new List<int>() { 1,2, 3, 4, 6, 7, 8 };
            string tabName = "HZ_BFXHDL";
            Leveling(pRow, tabName, fields, columnIndex);

            //计算小计
            string sql = "update HZ_BFXHDL set D20A=iif(isnull(D201A),0,D201A)+iif(isnull(D202A),0,D202A)+iif(isnull(D203A),0,D203A) ";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //汇总
            StringBuilder sb = new StringBuilder();
            sb = new StringBuilder();
            sb.Append("insert into HZ_BFXHDL(ZLDWDM,D08H2A,D0810A,D1104A,D1107A,D20A,D201A,D202A,D203A) ")
            .Append("select left(ZLDWDM,9),sum(D08H2A),sum(D0810A),sum(D1104A),sum(D1107A),sum(D20A),sum(D201A),sum(D202A),sum(D203A) from HZ_BFXHDL ")
            .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("insert into HZ_BFXHDL(ZLDWDM,D08H2A,D0810A,D1104A,D1107A,D20A,D201A,D202A,D203A) ")
            .Append("select left(ZLDWDM,6),sum(D08H2A),sum(D0810A),sum(D1104A),sum(D1107A),sum(D20A),sum(D201A),sum(D202A),sum(D203A) from HZ_BFXHDL ")
            .Append(" where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterLJTM(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "fq0602", "fq1001", "fq1003", "ljtm0301", "ljtm0302", "ljtm0305", "ljtm0307", "ljtm0404" };
            List<int> columnIndex = new List<int>() { 2, 3, 4, 6, 7, 8,9,10 };
            string tabName = "HZ_FQLJTM_BZ";
            Leveling(pRow, tabName, fields, columnIndex);

            string sql = @"Insert Into hz_fqljtm_bz (zldwdm,fq,fq0602,fq1001,fq1003,ljtm,ljtm0301,ljtm0302,ljtm0305,ljtm0307,ljtm0404) Select left(zldwdm,9),sum(fq),sum(fq0602),sum(fq1001),sum(fq1003),sum(ljtm),sum(ljtm0301),sum(ljtm0302),sum(ljtm0305),sum(ljtm0307),sum(ljtm0404) From hz_fqljtm_bz group by left(zldwdm,9)";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = @"Insert Into hz_fqljtm_bz (zldwdm,fq,fq0602,fq1001,fq1003,ljtm,ljtm0301,ljtm0302,ljtm0305,ljtm0307,ljtm0404) Select left(zldwdm,6),sum(fq),sum(fq0602),sum(fq1001),sum(fq1003),sum(ljtm),sum(ljtm0301),sum(ljtm0302),sum(ljtm0305),sum(ljtm0307),sum(ljtm0404) From hz_fqljtm_bz where len(zldwdm)=9 group by left(zldwdm,6)";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = "Update hz_fqljtm_bz Set fq = fq0602 + fq1001 + fq1003";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "Update hz_fqljtm_bz Set ljtm = ljtm0301 + ljtm0302 + ljtm0305 + ljtm0307 + ljtm0404";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        private void alterJKHFGCHF(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 10);
            List<string> fields = new List<string>() { "jkhf0201", "jkhf0201k", "jkhf0202", "jkhf0202k", "jkhf0203", "jkhf0203k", "jkhf0204", "jkhf0204k", "jkhf0301", "jkhf0301k", "jkhf0302", "jkhf0302k", "jkhf0305", "jkhf0307", "jkhf0307k", "jkhf0403k", "jkhf0404", "jkhf1104", "jkhf1104k","gchf0201", "gchf0201k", "gchf0202", "gchf0202k", "gchf0203", "gchf0203k", "gchf0204", "gchf0204k", "gchf0301", "gchf0301k", "gchf0302", "gchf0302k", "gchf0305", "gchf0307", "gchf0307k", "gchf0403k", "gchf0404", "gchf1104", "gchf1104k" };
            List<int> columnIndex = new List<int>() { 3, 4,5, 6, 7, 8, 9, 10,11,12,13,14,15,16,17,18,19,20,21,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41 };
            string tabName = "HZ_JKHFGCHF_BZ";
            Leveling(pRow, tabName, fields, columnIndex);

            string sql = @"insert into HZ_JKHFGCHF_BZ (zldwdm,hj,jkhf,jkhf0201,jkhf0201k,jkhf0202,jkhf0202k,jkhf0203,jkhf0203k,jkhf0204,jkhf0204k,jkhf0301,jkhf0301k,jkhf0302,jkhf0302k,jkhf0305,jkhf0307,jkhf0307k,jkhf0403k,jkhf0404,jkhf1104,jkhf1104k,gchf,gchf0201,gchf0201k,gchf0202,gchf0202k,gchf0203,gchf0203k,gchf0204,gchf0204k,gchf0301,gchf0301k,gchf0302,gchf0302k,gchf0305,gchf0307,gchf0307k,gchf0403k,gchf0404,gchf1104,gchf1104k) Select left(zldwdm,9),sum(hj),sum(jkhf),sum(jkhf0201),sum(jkhf0201k),sum(jkhf0202),sum(jkhf0202k),sum(jkhf0203),sum(jkhf0203k),sum(jkhf0204),sum(jkhf0204k),sum(jkhf0301),sum(jkhf0301k),sum(jkhf0302),sum(jkhf0302k),sum(jkhf0305),sum(jkhf0307),sum(jkhf0307k),sum(jkhf0403k),sum(jkhf0404),sum(jkhf1104),sum(jkhf1104k),sum(gchf),sum(gchf0201),sum(gchf0201k),sum(gchf0202),sum(gchf0202k),sum(gchf0203),sum(gchf0203k),sum(gchf0204),sum(gchf0204k),sum(gchf0301),sum(gchf0301k),sum(gchf0302),sum(gchf0302k),sum(gchf0305),sum(gchf0307),sum(gchf0307k),sum(gchf0403k),sum(gchf0404),sum(gchf1104),sum(gchf1104k) From HZ_JKHFGCHF_BZ Where Len(zldwdm) = 12 Group By left(zldwdm,9)";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sql = @"insert into HZ_JKHFGCHF_BZ (zldwdm,hj,jkhf,jkhf0201,jkhf0201k,jkhf0202,jkhf0202k,jkhf0203,jkhf0203k,jkhf0204,jkhf0204k,jkhf0301,jkhf0301k,jkhf0302,jkhf0302k,jkhf0305,jkhf0307,jkhf0307k,jkhf0403k,jkhf0404,jkhf1104,jkhf1104k,gchf,gchf0201,gchf0201k,gchf0202,gchf0202k,gchf0203,gchf0203k,gchf0204,gchf0204k,gchf0301,gchf0301k,gchf0302,gchf0302k,gchf0305,gchf0307,gchf0307k,gchf0403k,gchf0404,gchf1104,gchf1104k) Select left(zldwdm,6),sum(hj),sum(jkhf),sum(jkhf0201),sum(jkhf0201k),sum(jkhf0202),sum(jkhf0202k),sum(jkhf0203),sum(jkhf0203k),sum(jkhf0204),sum(jkhf0204k),sum(jkhf0301),sum(jkhf0301k),sum(jkhf0302),sum(jkhf0302k),sum(jkhf0305),sum(jkhf0307),sum(jkhf0307k),sum(jkhf0403k),sum(jkhf0404),sum(jkhf1104),sum(jkhf1104k),sum(gchf),sum(gchf0201),sum(gchf0201k),sum(gchf0202),sum(gchf0202k),sum(gchf0203),sum(gchf0203k),sum(gchf0204),sum(gchf0204k),sum(gchf0301),sum(gchf0301k),sum(gchf0302),sum(gchf0302k),sum(gchf0305),sum(gchf0307),sum(gchf0307k),sum(gchf0403k),sum(gchf0404),sum(gchf1104),sum(gchf1104k) From HZ_JKHFGCHF_BZ Where Len(zldwdm) = 9 Group By left(zldwdm,6) ";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            sql = "Update HZ_JKHFGCHF_BZ Set jkhf = jkhf0201 + jkhf0202+jkhf0203+jkhf0204+jkhf0301+jkhf0302+jkhf0305+jkhf0307+jkhf0403k+jkhf0404+jkhf1104+jkhf1104k";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "Update HZ_JKHFGCHF_BZ Set gchf = gchf0201 + gchf0202+gchf0203+gchf0204+gchf0301+gchf0302+gchf0305+gchf0307+gchf0403k+gchf0404+gchf1104+gchf1104k";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = "Update HZ_JKHFGCHF_BZ Set hj = jkhf + gchf";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void alterCZC(string excelName)
        {
            Aspose.Cells.Row pRow = readExcel(excelName, 11);
            //201
            List<string> fields = new List<string>() 
            { "D2010303", "D2010304", "D2010306", "D2010402", "D2010603", "D2011105", "D2011106", "D2011108", "D2010101", "D2010102", "D2010103", "D2010201",
                "D2010202", "D2010203", "D2010204", "D2010301", "D2010302", "D2010305", "D2010307", "D2010401", "D2010403", "D2010404", "D20105H1", "D2010508", 
                "D2010601", "D2010602", "D2010701", "D2010702", "D20108H1", "D20108H2", "D2010809", "D2010810", "D20109", "D2011001", "D2011002", "D2011003",
                "D2011004", "D2011005", "D2011006", "D2011007", "D2011008", "D2011009", "D2011101", "D2011102", "D2011103", "D2011104", "D2011107", "D2011109",
                "D2011110", "D2011201", "D2011202", "D2011203", "D2011204", "D2011205", "D2011206", "D2011207"};
            List<int> columnIndex = new List<int>() 
            { 72, 73, 74, 75, 76, 77, 78, 79, 81, 82, 83,85, 86, 87, 88, 90, 91, 92, 93, 95, 96, 97, 99, 100, 102, 103, 105, 106, 108, 109, 110, 111, 112, 114, 115, 116, 
                117, 118, 119, 120, 121, 122, 124, 125, 126, 127, 128, 129, 130, 132, 133, 134, 135, 136, 137, 138 };
            string tabName = "HZ_CZCGK_BZ1";
            Leveling(pRow, tabName, fields, columnIndex);
            string sql = @"update HZ_CZCGK_BZ1 set D20100=D2010303+D2010304+D2010306+D2010402+D2010603+D2011105+D2011106+D2011108,
                                                   D20101=D2010101+D2010102+D2010103,
                                                   D20102=D2010201+D2010202+D2010203+D2010204,
                                                   D20103=D2010301+D2010302+D2010305+D2010307,
                                                   D20104=D2010401+D2010403+D2010404,
                                                   D20105=D20105H1+D2010508,
                                                   D20106=D2010601+D2010602,
                                                   D20107=D2010701+D2010702,
                                                   D20108=D20108H1+D20108H2+D2010809+D2010810,
                                                   D20110=D2011001+D2011002+D2011003+D2011004+D2011005+D2011006+D2011007+D2011008+D2011009,
                                                   D20111=D2011101+D2011102+D2011103+D2011104+D2011107+D2011109+D2011110,
                                                   D20112=D2011201+D2011202+D2011203+D2011204+D2011205+D2011206+D2011207";
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = @"update HZ_CZCGK_BZ1 set D201=D20100+D20101+D20102+D20103+D20104+D20105+D20106+D20107+D20108+D20109+D20110+D20111+D20112";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //202
            fields = new List<string>() 
            { "D2020303","D2020304","D2020306","D2020402","D2020603","D2021105","D2021106","D2021108","D2020101","D2020102","D2020103","D2020201","D2020202",
                "D2020203","D2020204","D2020301","D2020302","D2020305","D2020307","D2020401","D2020403","D2020404","D20205H1","D2020508","D2020601","D2020602",
                "D2020701","D2020702","D20208H1","D20208H2","D2020809","D2020810","D20209","D2021001","D2021002","D2021003","D2021004","D2021005","D2021006",
                "D2021007","D2021008","D2021009","D2021101","D2021102","D2021103","D2021104","D2021107","D2021109","D2021110","D2021201","D2021202","D2021203",
                "D2021204","D2021205","D2021206","D2021207"};
            columnIndex = new List<int>() 
            { 141, 142, 143, 144, 145, 146, 147, 148, 150, 151, 152, 154, 155, 156, 157, 159, 160, 161, 162, 164, 165, 166, 168, 169, 171, 172, 174, 175, 177, 178, 179,
                180, 181, 183, 184, 185, 186, 187, 188, 189, 190, 191, 193, 194, 195, 196, 197, 198, 199, 201, 202, 203, 204, 205, 206, 207 };
            tabName = "HZ_CZCGK_BZ2";
            Leveling(pRow, tabName, fields, columnIndex);
            sql = @"update HZ_CZCGK_BZ2 set D20200=D2020303+D2020304+D2020306+D2020402+D2020603+D2021105+D2021106+D2021108,
                                                   D20201=D2020101+D2020102+D2020103,
                                                   D20202=D2020201+D2020202+D2020203+D2020204,
                                                   D20203=D2020301+D2020302+D2020305+D2020307,
                                                   D20204=D2020401+D2020403+D2020404,
                                                   D20205=D20205H1+D2020508,
                                                   D20206=D2020601+D2020602,
                                                   D20207=D2020701+D2020702,
                                                   D20208=D20208H1+D20208H2+D2020809+D2020810,
                                                   D20210=D2021001+D2021002+D2021003+D2021004+D2021005+D2021006+D2021007+D2021008+D2021009,
                                                   D20211=D2021101+D2021102+D2021103+D2021104+D2021107+D2021109+D2021110,
                                                   D20212=D2021201+D2021202+D2021203+D2021204+D2021205+D2021206+D2021207";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = @"update HZ_CZCGK_BZ2 set D202=D20200+D20201+D20202+D20203+D20204+D20205+D20206+D20207+D20208+D20209+D20210+D20211+D20212";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //203
            fields = new List<string>() 
            { "D2030303", "D2030304", "D2030306", "D2030402", "D2030603", "D2031105", "D2031106", "D2031108", "D2030101", "D2030102", "D2030103", "D2030201",
                "D2030202", "D2030203", "D2030204", "D2030301", "D2030302", "D2030305", "D2030307", "D2030401", "D2030403", "D2030404", "D20305H1", "D2030508", 
                "D2030601", "D2030602", "D2030701", "D2030702", "D20308H1", "D20308H2", "D2030809", "D2030810", "D20309", "D2031001", "D2031002", "D2031003",
                "D2031004", "D2031005", "D2031006", "D2031007", "D2031008", "D2031009", "D2031101", "D2031102", "D2031103", "D2031104", "D2031107", "D2031109",
                "D2031110", "D2031201", "D2031202", "D2031203", "D2031204", "D2031205", "D2031206", "D2031207"};
            columnIndex = new List<int>() 
            { 210, 211, 212, 213, 214, 215, 216, 217, 219, 220, 221, 223, 224, 225, 226, 228, 229, 230, 231, 233, 234, 235, 237, 238, 240, 241, 243, 244, 246, 247,
                248, 249, 250, 252, 253, 254, 255, 256, 257, 258, 259, 260, 262, 263, 264, 265, 266, 267, 268, 270, 271, 272, 273, 274, 275, 276 };
            tabName = "HZ_CZCGK_BZ3";
            Leveling(pRow, tabName, fields, columnIndex);
            sql = @"update HZ_CZCGK_BZ3 set D20300=D2030303+D2030304+D2030306+D2030402+D2030603+D2031105+D2031106+D2031108,
                                                   D20301=D2030101+D2030102+D2030103,
                                                   D20302=D2030201+D2030202+D2030203+D2030204,
                                                   D20303=D2030301+D2030302+D2030305+D2030307,
                                                   D20304=D2030401+D2030403+D2030404,
                                                   D20305=D20305H1+D2030508,
                                                   D20306=D2030601+D2030602,
                                                   D20307=D2030701+D2030702,
                                                   D20308=D20308H1+D20308H2+D2030809+D2030810,
                                                   D20310=D2031001+D2031002+D2031003+D2031004+D2031005+D2031006+D2031007+D2031008+D2031009,
                                                   D20311=D2031101+D2031102+D2031103+D2031104+D2031107+D2031109+D2031110,
                                                   D20312=D2031201+D2031202+D2031203+D2031204+D2031205+D2031206+D2031207";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = @"update HZ_CZCGK_BZ3 set D203=D20300+D20301+D20302+D20303+D20304+D20305+D20306+D20307+D20308+D20309+D20310+D20311+D20312";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //204
            fields = new List<string>() 
            { "D2040303", "D2040304", "D2040306", "D2040402", "D2040603", "D2041105", "D2041106", "D2041108", "D2040101", "D2040102", "D2040103", "D2040201", 
                "D2040202", "D2040203", "D2040204", "D2040301", "D2040302", "D2040305", "D2040307", "D2040401", "D2040403", "D2040404", "D20405H1", "D2040508",
                "D2040601", "D2040602", "D2040701", "D2040702", "D20408H1", "D20408H2", "D2040809", "D2040810", "D20409", "D2041001", "D2041002", "D2041003",
                "D2041004", "D2041005", "D2041006", "D2041007", "D2041008", "D2041009", "D2041101", "D2041102", "D2041103", "D2041104", "D2041107", "D2041109", 
                "D2041110", "D2041201", "D2041202", "D2041203", "D2041204", "D2041205", "D2041206", "D2041207"};
            columnIndex = new List<int>() 
            { 279, 280, 281, 282, 283, 284, 285, 286, 288, 289, 290, 292, 293, 294, 295, 297, 298, 299, 300, 302, 303, 304, 306, 307, 309, 310, 312, 313, 315, 
                316, 317, 318, 319, 321, 322, 323, 324, 325, 326, 327, 328, 329, 331, 332, 333, 334, 335, 336, 337, 339, 340, 341, 342, 343, 344, 345 };
            tabName = "HZ_CZCGK_BZ4";
            Leveling(pRow, tabName, fields, columnIndex);
            sql = @"update HZ_CZCGK_BZ4 set D20400=D2040303+D2040304+D2040306+D2040402+D2040603+D2041105+D2041106+D2041108,
                                                   D20401=D2040101+D2040102+D2040103,
                                                   D20402=D2040201+D2040202+D2040203+D2040204,
                                                   D20403=D2040301+D2040302+D2040305+D2040307,
                                                   D20404=D2040401+D2040403+D2040404,
                                                   D20405=D20405H1+D2040508,
                                                   D20406=D2040601+D2040602,
                                                   D20407=D2040701+D2040702,
                                                   D20408=D20408H1+D20408H2+D2040809+D2040810,
                                                   D20410=D2041001+D2041002+D2041003+D2041004+D2041005+D2041006+D2041007+D2041008+D2041009,
                                                   D20411=D2041101+D2041102+D2041103+D2041104+D2041107+D2041109+D2041110,
                                                   D20412=D2041201+D2041202+D2041203+D2041204+D2041205+D2041206+D2041207";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = @"update HZ_CZCGK_BZ4 set D204=D20400+D20401+D20402+D20403+D20404+D20405+D20406+D20407+D20408+D20409+D20410+D20411+D20412";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            //205
            fields = new List<string>() 
            { "D2050303", "D2050304", "D2050306", "D2050402", "D2050603", "D2051105", "D2051106", "D2051108", "D2050101", "D2050102", "D2050103", "D2050201", 
                "D2050202", "D2050203", "D2050204", "D2050301", "D2050302", "D2050305", "D2050307", "D2050401", "D2050403", "D2050404", "D20505H1", "D2050508",
                "D2050601", "D2050602", "D2050701", "D2050702", "D20508H1", "D20508H2", "D2050809", "D2050810", "D20509", "D2051001", "D2051002", "D2051003", 
                "D2051004", "D2051005", "D2051006", "D2051007", "D2051008", "D2051009", "D2051101", "D2051102", "D2051103", "D2051104", "D2051107", "D2051109",
                "D2051110", "D2051201", "D2051202", "D2051203", "D2051204", "D2051205", "D2051206", "D2051207"};
            columnIndex = new List<int>() 
            { 348,349,350,351,352,353,354,355,357,358,359,361,362,363,364,366,367,368,369,371,372,373,375,376,378,379,381,382,384,385,386,387,388,390,391,392,
                393,394,395,396,397,398,400,401,402,403,404,405,406,408,409,410,411,412,413,414};
            tabName = "HZ_CZCGK_BZ5";
            Leveling(pRow, tabName, fields, columnIndex);
            sql = @"update HZ_CZCGK_BZ5 set D20500=D2050303+D2050304+D2050306+D2050402+D2050603+D2051105+D2051106+D2051108,
                                                   D20501=D2050101+D2050102+D2050103,
                                                   D20502=D2050201+D2050202+D2050203+D2050204,
                                                   D20503=D2050301+D2050302+D2050305+D2050307,
                                                   D20504=D2050401+D2050403+D2050404,
                                                   D20505=D20505H1+D2050508,
                                                   D20506=D2050601+D2050602,
                                                   D20507=D2050701+D2050702,
                                                   D20508=D20508H1+D20508H2+D2050809+D2050810,
                                                   D20510=D2051001+D2051002+D2051003+D2051004+D2051005+D2051006+D2051007+D2051008+D2051009,
                                                   D20511=D2051101+D2051102+D2051103+D2051104+D2051107+D2051109+D2051110,
                                                   D20512=D2051201+D2051202+D2051203+D2051204+D2051205+D2051206+D2051207";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = @"update HZ_CZCGK_BZ5 set D205=D20500+D20501+D20502+D20503+D20504+D20505+D20506+D20507+D20508+D20509+D20510+D20511+D20512";
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            //计算 201 202 203 204  小计

            StringBuilder sb = new StringBuilder();
            sb.Append("update HZ_CZCGK_BZ1 set D201=iif(isnull(D20100),0,D20100)+iif(isnull(D20101),0,D20101)+iif(isnull(D20102),0,D20102)+iif(isnull(D20103),0,D20103)+")
              .Append("iif(isnull(D20104),0,D20104)+iif(isnull(D20105),0,D20105)+iif(isnull(D20106),0,D20106)+iif(isnull(D20107),0,D20107)+")
              .Append("iif(isnull(D20108),0,D20108)+iif(isnull(D20109),0,D20109)+iif(isnull(D20110),0,D20110)+iif(isnull(D20111),0,D20111)+iif(isnull(D20112),0,D20112)");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("update HZ_CZCGK_BZ2 set  D202=iif(isnull(D20200),0,D20200)+iif(isnull(D20201),0,D20201)+iif(isnull(D20202),0,D20202)+iif(isnull(D20203),0,D20203)+")
              .Append("iif(isnull(D20204),0,D20204)+iif(isnull(D20205),0,D20205)+iif(isnull(D20206),0,D20206)+iif(isnull(D20207),0,D20207)+")
              .Append("iif(isnull(D20208),0,D20208)+iif(isnull(D20209),0,D20209)+iif(isnull(D20210),0,D20210)+iif(isnull(D20211),0,D20211)+iif(isnull(D20212),0,D20212) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("update HZ_CZCGK_BZ3 set  D203=iif(isnull(D20300),0,D20300)+iif(isnull(D20301),0,D20301)+iif(isnull(D20302),0,D20302)+iif(isnull(D20303),0,D20303)+")
              .Append("iif(isnull(D20304),0,D20304)+iif(isnull(D20305),0,D20305)+iif(isnull(D20306),0,D20306)+iif(isnull(D20307),0,D20307)+")
              .Append("iif(isnull(D20308),0,D20308)+iif(isnull(D20309),0,D20309)+iif(isnull(D20310),0,D20310)+iif(isnull(D20311),0,D20311)+iif(isnull(D20312),0,D20312) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("update HZ_CZCGK_BZ4 set  D204=iif(isnull(D20400),0,D20400)+iif(isnull(D20401),0,D20401)+iif(isnull(D20402),0,D20402)+iif(isnull(D20403),0,D20403)+")
              .Append("iif(isnull(D20404),0,D20404)+iif(isnull(D20405),0,D20405)+iif(isnull(D20406),0,D20406)+iif(isnull(D20407),0,D20407)+")
              .Append("iif(isnull(D20408),0,D20408)+iif(isnull(D20409),0,D20409)+iif(isnull(D20410),0,D20410)+iif(isnull(D20411),0,D20411)+iif(isnull(D20412),0,D20412) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb = new StringBuilder();
            sb.Append("update HZ_CZCGK_BZ5 set  D205=iif(isnull(D20500),0,D20500)+iif(isnull(D20501),0,D20501)+iif(isnull(D20502),0,D20502)+iif(isnull(D20503),0,D20503)+")
              .Append("iif(isnull(D20504),0,D20504)+iif(isnull(D20505),0,D20505)+iif(isnull(D20506),0,D20506)+iif(isnull(D20507),0,D20507)+")
              .Append("iif(isnull(D20508),0,D20508)+iif(isnull(D20509),0,D20509)+iif(isnull(D20510),0,D20510)+iif(isnull(D20511),0,D20511)+iif(isnull(D20512),0,D20512) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //计算20 总计 //跨表了

            sb = new StringBuilder();
            sb.Append("update HZ_CZCGK_View1 set D20=iif(isnull(D201),0,D201)+iif(isnull(D202),0,D202)+iif(isnull(D203),0,D203)+iif(isnull(D204),0,D204)+iif(isnull(D205),0,D205),")
                .Append("D2000=iif(isnull(D20100),0,D20100)+iif(isnull(D20200),0,D20200)+iif(isnull(D20300),0,D20300)+iif(isnull(D20400),0,D20400)+iif(isnull(D20500),0,D20500),")
                .Append("D2001=iif(isnull(D20101),0,D20101)+iif(isnull(D20201),0,D20201)+iif(isnull(D20301),0,D20301)+iif(isnull(D20401),0,D20401)+iif(isnull(D20501),0,D20501),")
                .Append("D200101=iif(isnull(D2010101),0,D2010101)+iif(isnull(D2020101),0,D2020101)+iif(isnull(D2030101),0,D2030101)+iif(isnull(D2040101),0,D2040101)+iif(isnull(D2050101),0,D2050101),")
                .Append("D200102=iif(isnull(D2010102),0,D2010102)+iif(isnull(D2020102),0,D2020102)+iif(isnull(D2030102),0,D2030102)+iif(isnull(D2040102),0,D2040102)+iif(isnull(D2050102),0,D2050102),")
                .Append("D200103=iif(isnull(D2010103),0,D2010103)+iif(isnull(D2020103),0,D2020103)+iif(isnull(D2030103),0,D2030103)+iif(isnull(D2040103),0,D2040103)+iif(isnull(D2050103),0,D2050103),")
                .Append("D2002=iif(isnull(D20102),0,D20102)+iif(isnull(D20202),0,D20202)+iif(isnull(D20302),0,D20302)+iif(isnull(D20402),0,D20402)+iif(isnull(D20502),0,D20502),")
                .Append("D200201=iif(isnull(D2010201),0,D2010201)+iif(isnull(D2020201),0,D2020201)+iif(isnull(D2030201),0,D2030201)+iif(isnull(D2040201),0,D2040201)+iif(isnull(D2050201),0,D2050201),")
                .Append("D200202=iif(isnull(D2010202),0,D2010202)+iif(isnull(D2020202),0,D2020202)+iif(isnull(D2030202),0,D2030202)+iif(isnull(D2040202),0,D2040202)+iif(isnull(D2050202),0,D2050202),")
                .Append("D200203=iif(isnull(D2010203),0,D2010203)+iif(isnull(D2020203),0,D2020203)+iif(isnull(D2030203),0,D2030203)+iif(isnull(D2040203),0,D2040203)+iif(isnull(D2050203),0,D2050203),")
                .Append("D200204=iif(isnull(D2010204),0,D2010204)+iif(isnull(D2020204),0,D2020204)+iif(isnull(D2030204),0,D2030204)+iif(isnull(D2040204),0,D2040204)+iif(isnull(D2050204),0,D2050204),")

                .Append("D2003=iif(isnull(D20103),0,D20103)+iif(isnull(D20203),0,D20203)+iif(isnull(D20303),0,D20303)+iif(isnull(D20403),0,D20403)+iif(isnull(D20503),0,D20503),")
                .Append("D200301=iif(isnull(D2010301),0,D2010301)+iif(isnull(D2020301),0,D2020301)+iif(isnull(D2030301),0,D2030301)+iif(isnull(D2040301),0,D2040301)+iif(isnull(D2050301),0,D2050301),")
                .Append("D200302=iif(isnull(D2010302),0,D2010302)+iif(isnull(D2020302),0,D2020302)+iif(isnull(D2030302),0,D2030302)+iif(isnull(D2040302),0,D2040302)+iif(isnull(D2050302),0,D2050302),")
                .Append("D200303=iif(isnull(D2010303),0,D2010303)+iif(isnull(D2020303),0,D2020303)+iif(isnull(D2030303),0,D2030303)+iif(isnull(D2040303),0,D2040303)+iif(isnull(D2050303),0,D2050303),")
                .Append("D200304=iif(isnull(D2010304),0,D2010304)+iif(isnull(D2020304),0,D2020304)+iif(isnull(D2030304),0,D2030304)+iif(isnull(D2040304),0,D2040304)+iif(isnull(D2050304),0,D2050304),")
                .Append("D200305=iif(isnull(D2010305),0,D2010305)+iif(isnull(D2020305),0,D2020305)+iif(isnull(D2030305),0,D2030305)+iif(isnull(D2040305),0,D2040305)+iif(isnull(D2050305),0,D2050305),")
                .Append("D200306=iif(isnull(D2010306),0,D2010306)+iif(isnull(D2020306),0,D2020306)+iif(isnull(D2030306),0,D2030306)+iif(isnull(D2040306),0,D2040306)+iif(isnull(D2050306),0,D2050306),")
                .Append("D200307=iif(isnull(D2010307),0,D2010307)+iif(isnull(D2020307),0,D2020307)+iif(isnull(D2030307),0,D2030307)+iif(isnull(D2040307),0,D2040307)+iif(isnull(D2050307),0,D2050307),")
                .Append("D2004=iif(isnull(D20104),0,D20104)+iif(isnull(D20204),0,D20204)+iif(isnull(D20304),0,D20304)+iif(isnull(D20404),0,D20404)+iif(isnull(D20504),0,D20504),")
                .Append("D200401=iif(isnull(D2010401),0,D2010401)+iif(isnull(D2020401),0,D2020401)+iif(isnull(D2030401),0,D2030401)+iif(isnull(D2040401),0,D2040401)+iif(isnull(D2050401),0,D2050401),")
                .Append("D200402=iif(isnull(D2010402),0,D2010402)+iif(isnull(D2020402),0,D2020402)+iif(isnull(D2030402),0,D2030402)+iif(isnull(D2040402),0,D2040402)+iif(isnull(D2050402),0,D2050402),")
                .Append("D200403=iif(isnull(D2010403),0,D2010403)+iif(isnull(D2020403),0,D2020403)+iif(isnull(D2030403),0,D2030403)+iif(isnull(D2040403),0,D2040403)+iif(isnull(D2050403),0,D2050403),")
                .Append("D200404=iif(isnull(D2010404),0,D2010404)+iif(isnull(D2020404),0,D2020404)+iif(isnull(D2030404),0,D2030404)+iif(isnull(D2040404),0,D2040404)+iif(isnull(D2050404),0,D2050404) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear();
            sb.Append("update HZ_CZCGK_View2 set D2005=iif(isnull(D20105),0,D20105)+iif(isnull(D20205),0,D20205)+iif(isnull(D20305),0,D20305)+iif(isnull(D20405),0,D20405)+iif(isnull(D20505),0,D20505),")
                .Append("D2005H1=iif(isnull(D20105H1),0,D20105H1)+iif(isnull(D20205H1),0,D20205H1)+iif(isnull(D20305H1),0,D20305H1)+iif(isnull(D20405H1),0,D20405H1)+iif(isnull(D20505H1),0,D20505H1),")
                .Append("D200508=iif(isnull(D2010508),0,D2010508)+iif(isnull(D2020508),0,D2020508)+iif(isnull(D2030508),0,D2030508)+iif(isnull(D2040508),0,D2040508)+iif(isnull(D2050508),0,D2050508),")
                .Append("D2006=iif(isnull(D20106),0,D20106)+iif(isnull(D20206),0,D20206)+iif(isnull(D20306),0,D20306)+iif(isnull(D20406),0,D20406)+iif(isnull(D20506),0,D20506),")
                .Append("D200601=iif(isnull(D2010601),0,D2010601)+iif(isnull(D2020601),0,D2020601)+iif(isnull(D2030601),0,D2030601)+iif(isnull(D2040601),0,D2040601)+iif(isnull(D2050601),0,D2050601),")
                .Append("D200602=iif(isnull(D2010602),0,D2010602)+iif(isnull(D2020602),0,D2020602)+iif(isnull(D2030602),0,D2030602)+iif(isnull(D2040602),0,D2040602)+iif(isnull(D2050602),0,D2050602),")
                .Append("D200603=iif(isnull(D2010603),0,D2010603)+iif(isnull(D2020603),0,D2020603)+iif(isnull(D2030603),0,D2030603)+iif(isnull(D2040603),0,D2040603)+iif(isnull(D2050603),0,D2050603),")
                .Append("D2007=iif(isnull(D20107),0,D20107)+iif(isnull(D20207),0,D20207)+iif(isnull(D20307),0,D20307)+iif(isnull(D20407),0,D20407)+iif(isnull(D20507),0,D20507),")
                .Append("D200701=iif(isnull(D2010701),0,D2010701)+iif(isnull(D2020701),0,D2020701)+iif(isnull(D2030701),0,D2030701)+iif(isnull(D2040701),0,D2040701)+iif(isnull(D2050701),0,D2050701),")
                .Append("D200702=iif(isnull(D2010702),0,D2010702)+iif(isnull(D2020702),0,D2020702)+iif(isnull(D2030702),0,D2030702)+iif(isnull(D2040702),0,D2040702)+iif(isnull(D2050702),0,D2050702),")
                .Append("D2008=iif(isnull(D20108),0,D20108)+iif(isnull(D20208),0,D20208)+iif(isnull(D20308),0,D20308)+iif(isnull(D20408),0,D20408)+iif(isnull(D20508),0,D20508),")
                .Append("D2008H1=iif(isnull(D20108H1),0,D20108H1)+iif(isnull(D20208H1),0,D20208H1)+iif(isnull(D20308H1),0,D20308H1)+iif(isnull(D20408H1),0,D20408H1)+iif(isnull(D20508H1),0,D20508H1),")
                .Append("D2008H2=iif(isnull(D20108H2),0,D20108H2)+iif(isnull(D20208H2),0,D20208H2)+iif(isnull(D20308H2),0,D20308H2)+iif(isnull(D20408H2),0,D20408H2)+iif(isnull(D20508H2),0,D20508H2),")
                .Append("D200809=iif(isnull(D2010809),0,D2010809)+iif(isnull(D2020809),0,D2020809)+iif(isnull(D2030809),0,D2030809)+iif(isnull(D2040809),0,D2040809)+iif(isnull(D2050809),0,D2050809),")
                .Append("D200810=iif(isnull(D2010810),0,D2010810)+iif(isnull(D2020810),0,D2020810)+iif(isnull(D2030810),0,D2030810)+iif(isnull(D2040810),0,D2040810)+iif(isnull(D2050810),0,D2050810),")
                .Append("D2009=iif(isnull(D20109),0,D20109)+iif(isnull(D20209),0,D20209)+iif(isnull(D20309),0,D20309)+iif(isnull(D20409),0,D20409)+iif(isnull(D20509),0,D20509) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear();
            sb.Append(" update HZ_CZCGK_View3 set D2010=iif(isnull(D20110),0,D20110)+iif(isnull(D20210),0,D20210)+iif(isnull(D20310),0,D20310)+iif(isnull(D20410),0,D20410)+iif(isnull(D20510),0,D20510),")
                .Append("D201001=iif(isnull(D2011001),0,D2011001)+iif(isnull(D2021001),0,D2021001)+iif(isnull(D2031001),0,D2031001)+iif(isnull(D2041001),0,D2041001)+iif(isnull(D2051001),0,D2051001),")
                .Append("D201002=iif(isnull(D2011002),0,D2011002)+iif(isnull(D2021002),0,D2021002)+iif(isnull(D2031002),0,D2031002)+iif(isnull(D2041002),0,D2041002)+iif(isnull(D2051002),0,D2051002),")
                .Append("D201003=iif(isnull(D2011003),0,D2011003)+iif(isnull(D2021003),0,D2021003)+iif(isnull(D2031003),0,D2031003)+iif(isnull(D2041003),0,D2041003)+iif(isnull(D2051003),0,D2051003),")
                .Append("D201004=iif(isnull(D2011004),0,D2011004)+iif(isnull(D2021004),0,D2021004)+iif(isnull(D2031004),0,D2031004)+iif(isnull(D2041004),0,D2041004)+iif(isnull(D2051004),0,D2051004),")
                .Append("D201005=iif(isnull(D2011005),0,D2011005)+iif(isnull(D2021005),0,D2021005)+iif(isnull(D2031005),0,D2031005)+iif(isnull(D2041005),0,D2041005)+iif(isnull(D2051005),0,D2051005),")
                .Append("D201006=iif(isnull(D2011006),0,D2011006)+iif(isnull(D2021006),0,D2021006)+iif(isnull(D2031006),0,D2031006)+iif(isnull(D2041006),0,D2041006)+iif(isnull(D2051006),0,D2051006),")
                .Append("D201007=iif(isnull(D2011007),0,D2011007)+iif(isnull(D2021007),0,D2021007)+iif(isnull(D2031007),0,D2031007)+iif(isnull(D2041007),0,D2041007)+iif(isnull(D2051007),0,D2051007),")
                .Append("D201008=iif(isnull(D2011008),0,D2011008)+iif(isnull(D2021008),0,D2021008)+iif(isnull(D2031008),0,D2031008)+iif(isnull(D2041008),0,D2041008)+iif(isnull(D2051008),0,D2051008),")
                .Append("D201009=iif(isnull(D2011009),0,D2011009)+iif(isnull(D2021009),0,D2021009)+iif(isnull(D2031009),0,D2031009)+iif(isnull(D2041009),0,D2041009)+iif(isnull(D2051009),0,D2051009),")
                .Append("D2011=iif(isnull(D20111),0,D20111)+iif(isnull(D20211),0,D20211)+iif(isnull(D20311),0,D20311)+iif(isnull(D20411),0,D20411)+iif(isnull(D20511),0,D20511),")
               .Append("D201101=iif(isnull(D2011101),0,D2011101)+iif(isnull(D2021101),0,D2021101)+iif(isnull(D2031101),0,D2031101)+iif(isnull(D2041101),0,D2041101)+iif(isnull(D2051101),0,D2051101),")
               .Append("D201102=iif(isnull(D2011102),0,D2011102)+iif(isnull(D2021102),0,D2021102)+iif(isnull(D2031102),0,D2031102)+iif(isnull(D2041102),0,D2041102)+iif(isnull(D2051102),0,D2051102),")
               .Append("D201103=iif(isnull(D2011103),0,D2011103)+iif(isnull(D2021103),0,D2021103)+iif(isnull(D2031103),0,D2031103)+iif(isnull(D2041103),0,D2041103)+iif(isnull(D2051103),0,D2051103),")
               .Append("D201104=iif(isnull(D2011104),0,D2011104)+iif(isnull(D2021104),0,D2021104)+iif(isnull(D2031104),0,D2031104)+iif(isnull(D2041104),0,D2041104)+iif(isnull(D2051104),0,D2051104),")
               .Append("D201105=iif(isnull(D2011105),0,D2011105)+iif(isnull(D2021105),0,D2021105)+iif(isnull(D2031105),0,D2031105)+iif(isnull(D2041105),0,D2041105)+iif(isnull(D2051105),0,D2051105),")
               .Append("D201106=iif(isnull(D2011106),0,D2011106)+iif(isnull(D2021106),0,D2021106)+iif(isnull(D2031106),0,D2031106)+iif(isnull(D2041106),0,D2041106)+iif(isnull(D2051106),0,D2051106),")
               .Append("D201107=iif(isnull(D2011107),0,D2011107)+iif(isnull(D2021107),0,D2021107)+iif(isnull(D2031107),0,D2031107)+iif(isnull(D2041107),0,D2041107)+iif(isnull(D2051107),0,D2051107),")
               .Append("D201108=iif(isnull(D2011108),0,D2011108)+iif(isnull(D2021108),0,D2021108)+iif(isnull(D2031108),0,D2031108)+iif(isnull(D2041108),0,D2041108)+iif(isnull(D2051108),0,D2051108),")
               .Append("D201109=iif(isnull(D2011109),0,D2011109)+iif(isnull(D2021109),0,D2021109)+iif(isnull(D2031109),0,D2031109)+iif(isnull(D2041109),0,D2041109)+iif(isnull(D2051109),0,D2051109),")
               .Append("D201110=iif(isnull(D2011110),0,D2011110)+iif(isnull(D2021110),0,D2021110)+iif(isnull(D2031110),0,D2031110)+iif(isnull(D2041110),0,D2041110)+iif(isnull(D2051110),0,D2051110),")
               .Append("D2012=iif(isnull(D20112),0,D20112)+iif(isnull(D20212),0,D20212)+iif(isnull(D20312),0,D20312)+iif(isnull(D20412),0,D20412)+iif(isnull(D20512),0,D20512),")
               .Append("D201201=iif(isnull(D2011201),0,D2011201)+iif(isnull(D2021201),0,D2021201)+iif(isnull(D2031201),0,D2031201)+iif(isnull(D2041201),0,D2041201)+iif(isnull(D2051201),0,D2051201),")
               .Append("D201202=iif(isnull(D2011202),0,D2011202)+iif(isnull(D2021202),0,D2021202)+iif(isnull(D2031202),0,D2031202)+iif(isnull(D2041202),0,D2041202)+iif(isnull(D2051202),0,D2051202),")
               .Append("D201203=iif(isnull(D2011203),0,D2011203)+iif(isnull(D2021203),0,D2021203)+iif(isnull(D2031203),0,D2031203)+iif(isnull(D2041203),0,D2041203)+iif(isnull(D2051203),0,D2051203),")
               .Append("D201204=iif(isnull(D2011204),0,D2011204)+iif(isnull(D2021204),0,D2021204)+iif(isnull(D2031204),0,D2031204)+iif(isnull(D2041204),0,D2041204)+iif(isnull(D2051204),0,D2051204),")
               .Append("D201205=iif(isnull(D2011205),0,D2011205)+iif(isnull(D2021205),0,D2021205)+iif(isnull(D2031205),0,D2031205)+iif(isnull(D2041205),0,D2041205)+iif(isnull(D2051205),0,D2051205),")
               .Append("D201206=iif(isnull(D2011206),0,D2011206)+iif(isnull(D2021206),0,D2021206)+iif(isnull(D2031206),0,D2031206)+iif(isnull(D2041206),0,D2041206)+iif(isnull(D2051206),0,D2051206),")
               .Append("D201207=iif(isnull(D2011207),0,D2011207)+iif(isnull(D2021207),0,D2021207)+iif(isnull(D2031207),0,D2031207)+iif(isnull(D2041207),0,D2041207)+iif(isnull(D2051207),0,D2051207) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //汇总到乡
            sb = new StringBuilder();
            sb.Append("insert into HZ_CZCGK_BZ1(ZLDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
                .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
                .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
                .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
                .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
                .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
                .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
                .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
                .Append("D20105,D20105H1,D2010508,D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
                .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
                .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
                .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
                .Append(" select left(ZLDWDM,9),sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
                .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
                .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
                .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
                .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
                .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
                .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
                .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
                .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
                .Append("sum(D201),sum(D20100),sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
                .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
                .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
                .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
                .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
                .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
                .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
                .Append(" from HZ_CZCGK_BZ1 where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            // 还没完
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ2(ZLDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
               .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
               .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
               .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
               .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
               .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
               .Append(" select left(ZLDWDM,9),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
               .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
               .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
               .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
               .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
               .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
               .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
               .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
               .Append(" from HZ_CZCGK_BZ2 where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ3(ZLDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,D20302,D2030201,D2030202,D2030203,D2030204,")
               .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307, D20304,D2030401,D2030402,D2030403,D2030404,")
               .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
               .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
               .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
               .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207 ) ")
               .Append(" select left(ZLDWDM,9),sum(D203),sum(D20300),sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
               .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
               .Append("sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307), ")
               .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
               .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
               .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
               .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
               .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207) ")
               .Append(" from HZ_CZCGK_BZ3 where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ4(ZLDWDM,D204,D20400,D20401,D2040101,D2040102,D2040103,D20402,D2040201,D2040202,D2040203,D2040204,")
               .Append("D20403,D2040301,D2040302,D2040303,D2040304,D2040305,D2040306,D2040307, D20404,D2040401,D2040402,D2040403,D2040404,")
               .Append("D20405,D20405H1,D2040508,D20406,D2040601,D2040602,D2040603,D20407,D2040701,D2040702,D20408,D20408H1,D20408H2,D2040809,D2040810,")
               .Append("D20409,D20410,D2041001,D2041002,D2041003,D2041004,D2041005,D2041006,D2041007,D2041008,D2041009,")
               .Append("D20411,D2041101,D2041102,D2041103,D2041104,D2041105,D2041106,D2041107,D2041108,D2041109,D2041110,")
               .Append("D20412,D2041201,D2041202,D2041203,D2041204,D2041205,D2041206,D2041207 ) ")
               .Append(" select left(ZLDWDM,9),sum(D204),sum(D20400),sum(D20401),sum(D2040101),sum(D2040102),sum(D2040103),")
               .Append("sum(D20402),sum(D2040201),sum(D2040202),sum(D2040203),sum(D2040204),")
               .Append("sum(D20403),sum(D2040301),sum(D2040302),sum(D2040303),sum(D2040304),sum(D2040305),sum(D2040306),sum(D2040307), ")
               .Append("sum(D20404),sum(D2040401),sum(D2040402),sum(D2040403),sum(D2040404),")
               .Append("sum(D20405),sum(D20405H1),sum(D2040508),sum(D20406),sum(D2040601),sum(D2040602),sum(D2040603),sum(D20407),sum(D2040701),sum(D2040702),sum(D20408),sum(D20408H1),sum(D20408H2),sum(D2040809),sum(D2040810),")
               .Append("sum(D20409),sum(D20410),sum(D2041001),sum(D2041002),sum(D2041003),sum(D2041004),sum(D2041005),sum(D2041006),sum(D2041007),sum(D2041008),sum(D2041009),")
               .Append("sum(D20411),sum(D2041101),sum(D2041102),sum(D2041103),sum(D2041104),sum(D2041105),sum(D2041106),sum(D2041107),sum(D2041108),sum(D2041109),sum(D2041110),")
               .Append("sum(D20412),sum(D2041201),sum(D2041202),sum(D2041203),sum(D2041204),sum(D2041205),sum(D2041206),sum(D2041207) ")
               .Append(" from HZ_CZCGK_BZ4 where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ5(ZLDWDM,D205,D20500,D20501,D2050101,D2050102,D2050103,D20502,D2050201,D2050202,D2050203,D2050204,")
               .Append("D20503,D2050301,D2050302,D2050303,D2050304,D2050305,D2050306,D2050307, D20504,D2050401,D2050402,D2050403,D2050404,")
               .Append("D20505,D20505H1,D2050508,D20506,D2050601,D2050602,D2050603,D20507,D2050701,D2050702,D20508,D20508H1,D20508H2,D2050809,D2050810,")
               .Append("D20509,D20510,D2051001,D2051002,D2051003,D2051004,D2051005,D2051006,D2051007,D2051008,D2051009,")
               .Append("D20511,D2051101,D2051102,D2051103,D2051104,D2051105,D2051106,D2051107,D2051108,D2051109,D2051110,")
               .Append("D20512,D2051201,D2051202,D2051203,D2051204,D2051205,D2051206,D2051207 ) ")
               .Append(" select left(ZLDWDM,9),sum(D205),sum(D20500),sum(D20501),sum(D2050101),sum(D2050102),sum(D2050103),")
               .Append("sum(D20502),sum(D2050201),sum(D2050202),sum(D2050203),sum(D2050204),")
               .Append("sum(D20503),sum(D2050301),sum(D2050302),sum(D2050303),sum(D2050304),sum(D2050305),sum(D2050306),sum(D2050307), ")
               .Append("sum(D20504),sum(D2050401),sum(D2050402),sum(D2050403),sum(D2050404),")
               .Append("sum(D20505),sum(D20505H1),sum(D2050508),sum(D20506),sum(D2050601),sum(D2050602),sum(D2050603),sum(D20507),sum(D2050701),sum(D2050702),sum(D20508),sum(D20508H1),sum(D20508H2),sum(D2050809),sum(D2050810),")
               .Append("sum(D20509),sum(D20510),sum(D2051001),sum(D2051002),sum(D2051003),sum(D2051004),sum(D2051005),sum(D2051006),sum(D2051007),sum(D2051008),sum(D2051009),")
               .Append("sum(D20511),sum(D2051101),sum(D2051102),sum(D2051103),sum(D2051104),sum(D2051105),sum(D2051106),sum(D2051107),sum(D2051108),sum(D2051109),sum(D2051110),")
               .Append("sum(D20512),sum(D2051201),sum(D2051202),sum(D2051203),sum(D2051204),sum(D2051205),sum(D2051206),sum(D2051207) ")
               .Append(" from HZ_CZCGK_BZ5 where len(ZLDWDM)=12 group by left(ZLDWDM,9) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            //汇总到县
            sb = new StringBuilder();
            sb.Append("insert into HZ_CZCGK_BZ1(ZLDWDM,D20,D2000,D2001,D200101,D200102,D200103,D2002,D200201,D200202,D200203,D200204,")
                .Append("D2003,D200301,D200302,D200303,D200304,D200305,D200306,D200307,D2004,D200401,D200402,D200403,D200404,")
                .Append("D2005,D2005H1,D200508,D2006,D200601,D200602,D200603,D2007,D200701,D200702,D2008,D2008H1,D2008H2,D200809,D200810,")
                .Append("D2009,D2010,D201001,D201002,D201003,D201004,D201005,D201006,D201007,D201008,D201009,")
                .Append("D2011,D201101,D201102,D201103,D201104,D201105,D201106,D201107,D201108,D201109,D201110,")
                .Append("D2012,D201201,D201202,D201203,D201204,D201205,D201206,D201207,")
                .Append("D201,D20100,D20101,D2010101,D2010102,D2010103,D20102,D2010201,D2010202,D2010203,D2010204,")
                .Append("D20103,D2010301,D2010302,D2010303,D2010304,D2010305,D2010306,D2010307,D20104,D2010401,D2010402,D2010403,D2010404,")
                .Append("D20105,D20105H1,D2010508,D20106,D2010601,D2010602,D2010603,D20107,D2010701,D2010702,D20108,D20108H1,D20108H2,D2010809,D2010810,")
                .Append("D20109,D20110,D2011001,D2011002,D2011003,D2011004,D2011005,D2011006,D2011007,D2011008,D2011009,")
                .Append("D20111,D2011101,D2011102,D2011103,D2011104,D2011105,D2011106,D2011107,D2011108,D2011109,D2011110,")
                .Append("D20112,D2011201,D2011202,D2011203,D2011204,D2011205,D2011206,D2011207 )")
                .Append(" select left(ZLDWDM,6),sum(D20),sum(D2000),sum(D2001),sum(D200101),sum(D200102),sum(D200103),")
                .Append(" sum(D2002),sum(D200201),sum(D200202),sum(D200203),sum(D200204),")
                .Append(" sum(D2003),sum(D200301),sum(D200302),sum(D200303),sum(D200304),sum(D200305),sum(D200306),sum(D200307),")
                .Append("sum(D2004),sum(D200401),sum(D200402),sum(D200403),sum(D200404),")
                .Append("sum(D2005),sum(D2005H1),sum(D200508),sum(D2006),sum(D200601),sum(D200602),sum(D200603),sum(D2007),sum(D200701), sum(D200702),")
                .Append("sum(D2008),sum(D2008H1),sum(D2008H2),sum(D200809),sum(D200810),")
                .Append("sum(D2009),sum(D2010),sum(D201001),sum(D201002),sum(D201003),sum(D201004),sum(D201005),sum(D201006),sum(D201007),sum(D201008),sum(D201009),")
                .Append("sum(D2011),sum(D201101),sum(D201102),sum(D201103),sum(D201104),sum(D201105),sum(D201106),sum(D201107),sum(D201108),sum(D201109),sum(D201110),")
                .Append("sum(D2012),sum(D201201),sum(D201202),sum(D201203),sum(D201204),sum(D201205),sum(D201206),sum(D201207), ")
                .Append("sum(D201),sum(D20100),sum(D20101),sum(D2010101),sum(D2010102),sum(D2010103), sum(D20102),sum(D2010201),sum(D2010202),sum(D2010203),sum(D2010204),")
                .Append("sum(D20103),sum(D2010301),sum(D2010302),sum(D2010303),sum(D2010304),sum(D2010305),sum(D2010306),sum(D2010307), ")
                .Append("sum(D20104),sum(D2010401),sum(D2010402),sum(D2010403),sum(D2010404),")
                .Append("sum(D20105),sum(D20105H1),sum(D2010508),sum(D20106),sum(D2010601),sum(D2010602),sum(D2010603),sum(D20107),sum(D2010701),sum(D2010702),sum(D20108),sum(D20108H1),sum(D20108H2),sum(D2010809),sum(D2010810),")
                .Append("sum(D20109),sum(D20110),sum(D2011001),sum(D2011002),sum(D2011003),sum(D2011004),sum(D2011005),sum(D2011006),sum(D2011007),sum(D2011008),sum(D2011009),")
                .Append("sum(D20111),sum(D2011101),sum(D2011102),sum(D2011103),sum(D2011104),sum(D2011105),sum(D2011106),sum(D2011107),sum(D2011108),sum(D2011109),sum(D2011110),")
                .Append("sum(D20112),sum(D2011201),sum(D2011202),sum(D2011203),sum(D2011204),sum(D2011205),sum(D2011206),sum(D2011207)")
                .Append(" from HZ_CZCGK_BZ1 where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            // 还没完
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);


            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ2(ZLDWDM,D202,D20200,D20201,D2020101,D2020102,D2020103,D20202,D2020201,D2020202,D2020203,D2020204,")
               .Append("D20203,D2020301,D2020302,D2020303,D2020304,D2020305,D2020306,D2020307, D20204,D2020401,D2020402,D2020403,D2020404,")
               .Append("D20205,D20205H1,D2020508,D20206,D2020601,D2020602,D2020603,D20207,D2020701,D2020702,D20208,D20208H1,D20208H2,D2020809,D2020810,")
               .Append("D20209,D20210,D2021001,D2021002,D2021003,D2021004,D2021005,D2021006,D2021007,D2021008,D2021009,")
               .Append("D20211,D2021101,D2021102,D2021103,D2021104,D2021105,D2021106,D2021107,D2021108,D2021109,D2021110,")
               .Append("D20212,D2021201,D2021202,D2021203,D2021204,D2021205,D2021206,D2021207 ) ")
               .Append(" select left(ZLDWDM,6),sum(D202),sum(D20200),sum(D20201),sum(D2020101),sum(D2020102),sum(D2020103),")
               .Append("sum(D20202),sum(D2020201),sum(D2020202),sum(D2020203),sum(D2020204),")
               .Append("sum(D20203),sum(D2020301),sum(D2020302),sum(D2020303),sum(D2020304),sum(D2020305),sum(D2020306),sum(D2020307), ")
               .Append("sum(D20204),sum(D2020401),sum(D2020402),sum(D2020403),sum(D2020404),")
               .Append("sum(D20205),sum(D20205H1),sum(D2020508),sum(D20206),sum(D2020601),sum(D2020602),sum(D2020603),sum(D20207),sum(D2020701),sum(D2020702),sum(D20208),sum(D20208H1),sum(D20208H2),sum(D2020809),sum(D2020810),")
               .Append("sum(D20209),sum(D20210),sum(D2021001),sum(D2021002),sum(D2021003),sum(D2021004),sum(D2021005),sum(D2021006),sum(D2021007),sum(D2021008),sum(D2021009),")
               .Append("sum(D20211),sum(D2021101),sum(D2021102),sum(D2021103),sum(D2021104),sum(D2021105),sum(D2021106),sum(D2021107),sum(D2021108),sum(D2021109),sum(D2021110),")
               .Append("sum(D20212),sum(D2021201),sum(D2021202),sum(D2021203),sum(D2021204),sum(D2021205),sum(D2021206),sum(D2021207) ")
               .Append(" from HZ_CZCGK_BZ2 where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ3(ZLDWDM,D203,D20300,D20301,D2030101,D2030102,D2030103,")
            .Append("D20302,D2030201,D2030202,D2030203,D2030204,")
            .Append("D20303,D2030301,D2030302,D2030303,D2030304,D2030305,D2030306,D2030307,D20304,D2030401,D2030402,D2030403,D2030404,")
                .Append("D20305,D20305H1,D2030508,D20306,D2030601,D2030602,D2030603,D20307,D2030701,D2030702,D20308,D20308H1,D20308H2,D2030809,D2030810,")
                .Append("D20309,D20310,D2031001,D2031002,D2031003,D2031004,D2031005,D2031006,D2031007,D2031008,D2031009,")
                .Append("D20311,D2031101,D2031102,D2031103,D2031104,D2031105,D2031106,D2031107,D2031108,D2031109,D2031110,")
                .Append("D20312,D2031201,D2031202,D2031203,D2031204,D2031205,D2031206,D2031207 ) ")
                .Append(" select left(ZLDWDM,6),sum(D203),sum(D20300), sum(D20301),sum(D2030101),sum(D2030102),sum(D2030103),")
            .Append("sum(D20302),sum(D2030201),sum(D2030202),sum(D2030203),sum(D2030204),")
            .Append(" sum(D20303),sum(D2030301),sum(D2030302),sum(D2030303),sum(D2030304),sum(D2030305),sum(D2030306),sum(D2030307),")
            .Append("sum(D20304),sum(D2030401),sum(D2030402),sum(D2030403),sum(D2030404),")
            .Append("sum(D20305),sum(D20305H1),sum(D2030508),sum(D20306),sum(D2030601),sum(D2030602),sum(D2030603),sum(D20307),sum(D2030701),sum(D2030702),sum(D20308),sum(D20308H1),sum(D20308H2),sum(D2030809),sum(D2030810),")
            .Append("sum(D20309),sum(D20310),sum(D2031001),sum(D2031002),sum(D2031003),sum(D2031004),sum(D2031005),sum(D2031006),sum(D2031007),sum(D2031008),sum(D2031009),")
            .Append("sum(D20311),sum(D2031101),sum(D2031102),sum(D2031103),sum(D2031104),sum(D2031105),sum(D2031106),sum(D2031107),sum(D2031108),sum(D2031109),sum(D2031110),")
            .Append("sum(D20312),sum(D2031201),sum(D2031202),sum(D2031203),sum(D2031204),sum(D2031205),sum(D2031206),sum(D2031207)")
            .Append("  from HZ_CZCGK_BZ3 where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ4(ZLDWDM,D204,D20400,D20401,D2040101,D2040102,D2040103,D20402,D2040201,D2040202,D2040203,D2040204,")
               .Append("D20403,D2040301,D2040302,D2040303,D2040304,D2040305,D2040306,D2040307, D20404,D2040401,D2040402,D2040403,D2040404,")
               .Append("D20405,D20405H1,D2040508,D20406,D2040601,D2040602,D2040603,D20407,D2040701,D2040702,D20408,D20408H1,D20408H2,D2040809,D2040810,")
               .Append("D20409,D20410,D2041001,D2041002,D2041003,D2041004,D2041005,D2041006,D2041007,D2041008,D2041009,")
               .Append("D20411,D2041101,D2041102,D2041103,D2041104,D2041105,D2041106,D2041107,D2041108,D2041109,D2041110,")
               .Append("D20412,D2041201,D2041202,D2041203,D2041204,D2041205,D2041206,D2041207 ) ")
               .Append(" select left(ZLDWDM,6),sum(D204),sum(D20400),sum(D20401),sum(D2040101),sum(D2040102),sum(D2040103),")
               .Append("sum(D20402),sum(D2040201),sum(D2040202),sum(D2040203),sum(D2040204),")
               .Append("sum(D20403),sum(D2040301),sum(D2040302),sum(D2040303),sum(D2040304),sum(D2040305),sum(D2040306),sum(D2040307), ")
               .Append("sum(D20404),sum(D2040401),sum(D2040402),sum(D2040403),sum(D2040404),")
               .Append("sum(D20405),sum(D20405H1),sum(D2040508),sum(D20406),sum(D2040601),sum(D2040602),sum(D2040603),sum(D20407),sum(D2040701),sum(D2040702),sum(D20408),sum(D20408H1),sum(D20408H2),sum(D2040809),sum(D2040810),")
               .Append("sum(D20409),sum(D20410),sum(D2041001),sum(D2041002),sum(D2041003),sum(D2041004),sum(D2041005),sum(D2041006),sum(D2041007),sum(D2041008),sum(D2041009),")
               .Append("sum(D20411),sum(D2041101),sum(D2041102),sum(D2041103),sum(D2041104),sum(D2041105),sum(D2041106),sum(D2041107),sum(D2041108),sum(D2041109),sum(D2041110),")
               .Append("sum(D20412),sum(D2041201),sum(D2041202),sum(D2041203),sum(D2041204),sum(D2041205),sum(D2041206),sum(D2041207) ")
               .Append(" from HZ_CZCGK_BZ4 where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            sb.Clear();
            sb.Append(" insert into HZ_CZCGK_BZ5(ZLDWDM,D205,D20500,D20501,D2050101,D2050102,D2050103,D20502,D2050201,D2050202,D2050203,D2050204,")
               .Append("D20503,D2050301,D2050302,D2050303,D2050304,D2050305,D2050306,D2050307, D20504,D2050401,D2050402,D2050403,D2050404,")
               .Append("D20505,D20505H1,D2050508,D20506,D2050601,D2050602,D2050603,D20507,D2050701,D2050702,D20508,D20508H1,D20508H2,D2050809,D2050810,")
               .Append("D20509,D20510,D2051001,D2051002,D2051003,D2051004,D2051005,D2051006,D2051007,D2051008,D2051009,")
               .Append("D20511,D2051101,D2051102,D2051103,D2051104,D2051105,D2051106,D2051107,D2051108,D2051109,D2051110,")
               .Append("D20512,D2051201,D2051202,D2051203,D2051204,D2051205,D2051206,D2051207 ) ")
               .Append(" select left(ZLDWDM,6),sum(D205),sum(D20500),sum(D20501),sum(D2050101),sum(D2050102),sum(D2050103),")
               .Append("sum(D20502),sum(D2050201),sum(D2050202),sum(D2050203),sum(D2050204),")
               .Append("sum(D20503),sum(D2050301),sum(D2050302),sum(D2050303),sum(D2050304),sum(D2050305),sum(D2050306),sum(D2050307), ")
               .Append("sum(D20504),sum(D2050401),sum(D2050402),sum(D2050403),sum(D2050404),")
               .Append("sum(D20505),sum(D20505H1),sum(D2050508),sum(D20506),sum(D2050601),sum(D2050602),sum(D2050603),sum(D20507),sum(D2050701),sum(D2050702),sum(D20508),sum(D20508H1),sum(D20508H2),sum(D2050809),sum(D2050810),")
               .Append("sum(D20509),sum(D20510),sum(D2051001),sum(D2051002),sum(D2051003),sum(D2051004),sum(D2051005),sum(D2051006),sum(D2051007),sum(D2051008),sum(D2051009),")
               .Append("sum(D20511),sum(D2051101),sum(D2051102),sum(D2051103),sum(D2051104),sum(D2051105),sum(D2051106),sum(D2051107),sum(D2051108),sum(D2051109),sum(D2051110),")
               .Append("sum(D20512),sum(D2051201),sum(D2051202),sum(D2051203),sum(D2051204),sum(D2051205),sum(D2051206),sum(D2051207) ")
               .Append(" from HZ_CZCGK_BZ5 where len(ZLDWDM)=12 group by left(ZLDWDM,6) ");
            sql = sb.ToString();
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void Leveling(Aspose.Cells.Row pRow,string tabName, List<string> fields, List<int> columnIndex) 
        {
            string sql = "select * from " + tabName + " where zldwdm='"+zldw+"'";
            DataRow dr = RCIS.Database.LS_ResultMDBHelper.GetDataRow(sql, "tmp");

            for (int i = 0; i < fields.Count; i++)
            {
                double bgVal = double.Parse(pRow.GetCellOrNull(columnIndex[i]).Value.ToString());
                double jcVal = double.Parse(string.IsNullOrWhiteSpace(dr[fields[i]].ToString()) ? "0" : dr[fields[i]].ToString());
                double diffVal =Math.Round(bgVal - jcVal,2);
                double tzVal = 0.01;
                if (diffVal < 0)
                    tzVal = -0.01;
                if (diffVal == 0)
                    continue;
                sql = "select zldwdm," + fields[i] + " from " + tabName + " where len(zldwdm)>9 and " + fields[i] + ">0 order by " + fields[i] + " DESC";
                DataTable dtTmp = RCIS.Database.LS_ResultMDBHelper.GetDataTable(sql, "tmp");
                int count = (int)Math.Abs(diffVal / 0.01);
                int integer = count / dtTmp.Rows.Count;
                int remainder = count % dtTmp.Rows.Count;
                for (int j = 0; j < dtTmp.Rows.Count; j++)
                {
                    string dwdm = dtTmp.Rows[j][0].ToString();
                    double val = double.Parse(dtTmp.Rows[j][1].ToString());
                    double finallyVal = 0;
                    if (j < remainder)
                        finallyVal = val + tzVal * integer + tzVal;
                    else
                        finallyVal = val + tzVal * integer;
                    if (finallyVal == val)
                        continue;
                    else
                    {
                        sql = "update " + tabName + " set " + fields[i] + "=" + Math.Round(finallyVal,2) + " where zldwdm='" + dwdm + "'";
                        int update = RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                    }

                }
            }
            sql = "delete from "+tabName+" where len(zldwdm)<12";
            int del = RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }


        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestDir.Text = dlg.SelectedPath;
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.buttonEdit1.Text = dlg.SelectedPath;
        }
    }
}
