using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using TDDC3D.output;
using RCIS.Database;

namespace TDDC3D.gengxin
{
    public partial class FrmReport : Form
    {
        public IWorkspace currWs = null;

        public FrmReport()
        {
            InitializeComponent();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            //try 
            //{
                string dwdm = "";
                string bbPath = "";
                if (radioGroup1.SelectedIndex == 1)
                    bbPath = filePathBB.Text;
                if (tvXzq.SelectedNode != null) dwdm = tvXzq.SelectedNode.Text.Substring(0, tvXzq.SelectedNode.Text.IndexOf('|'));
                
                if (string.IsNullOrWhiteSpace(txtKzmjLD.Text)&&radioGroup1.SelectedIndex==0)
                {
                    MessageBox.Show("请输入控制面积或者改用基础报表方式取得年初面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (radioGroup1.SelectedIndex == 1 && string.IsNullOrWhiteSpace(bbPath))
                {
                    MessageBox.Show("请选择基础报表路径或者改用图斑方式统计年初面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string> erroMessage = null;
                clsOutputData.checkData((RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC"),bbPath,dwdm, ref erroMessage);
                if (erroMessage.Count > 0)
                {
                    char c = (char)13;
                    MessageBox.Show(string.Join(c.ToString(), erroMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                //RCIS.Database.LS_TysdMDBHelper.ConnectionOpen();

                if (string.IsNullOrWhiteSpace(bbPath))
                {
                    UpdateStatus("开始统计成果报表");
                    clsOutputData.CopyDltb(currWs);

                    UpdateStatus("正在初步进行平方米统计...");
                    Application.DoEvents();
                    clsOutputData.Dltb2BaseTable2();

                    UpdateStatus("正在进行生成基础统计...");
                    Application.DoEvents();
                    clsOutputData.ChangeTMP2JCB("PMMJ");

                    double ldmj = 0;
                    double.TryParse(txtKzmjLD.Text, out ldmj);
                    
                    UpdateStatus("正在进行调平.....");
                    Application.DoEvents();
                    ClsTP tp = new ClsTP();
                    tp.ChangeTableDW2GQ("HZ_JCB");  //转化为公顷
                    tp.MakeBalance("00", ldmj);
                }
                

                UpdateStatus("开始统计变更一览表");
                output.clsOutputData.StatisticsBGLYB(dwdm, currWs);
                UpdateStatus("变更一览表统计完成");
                //return;
                UpdateStatus("正在统计变更基础表");
                output.clsOutputData.StatisticsBGJCB();

                if (checkEdit1.Checked)
                {
                    double bghkzmj = 0;
                    double.TryParse(txttzhMJ.Text, out bghkzmj);
                    double kzmj = 0;
                    if (radioGroup1.SelectedIndex == 0)
                        double.TryParse(txtKzmjLD.Text, out kzmj);
                    else
                    {
                        DataRow excelRow = clsOutputData.getExcleRow(filePathBB.Text + "\\(" + dwdm + ")土地利用现状一级分类面积按权属性质汇总表.xlsx", dwdm);
                        string val=excelRow[1].ToString();
                        double.TryParse(val, out kzmj);
                    }
                    double bhmj = Math.Round(bghkzmj, 2) - Math.Round(kzmj,2);
                    System.Threading.Thread.Sleep(10000);
                    clsOutputData.AreaLeveling(Math.Round(bhmj,2));
                }

                UpdateStatus("变更基础表统计完成");
                StatisticsBB(dwdm,bbPath);
                Application.DoEvents();

                System.Threading.Thread.Sleep(10000);

                //调整后年初面积出现负值平差
                DataTable dt = clsOutputData.StatisticsDatatableTZHNC(dwdm);
                if (dt.Rows.Count > 0)
                {
                    UpdateStatus("正在进行年度变更基础表平差...");
                    clsOutputData.StatisticsPC(dt, dwdm, bbPath, true);
                    StatisticsBB(dwdm, bbPath);
                }
                //年末面积出现负值平差
                dt = new DataTable();
                dt = clsOutputData.StatisticsDatatable(dwdm);
                if (dt.Rows.Count > 0)
                {
                    UpdateStatus("正在进行年度变更基础表平差...");
                    clsOutputData.StatisticsPC(dt, dwdm, bbPath);
                    StatisticsBB(dwdm, bbPath);
                }
                
                //RCIS.Database.LS_TysdMDBHelper.ConnectionClose();

                MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, ex.ToString());
            //    return;
            //}
            
            
        }

        
        /// <summary>
        /// 统计报表
        /// </summary>
        /// <param name="dwdm"></param>
        private void StatisticsBB(string dwdm, string bbPath)
        {
            bool b = true;
            //表2  土地利用现状变更表
            UpdateStatus("正在统计土地利用现状变化平衡统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                output.clsOutputData.InitZlTable();
            output.clsOutputData.StatisticsTDLYXZBG(dwdm, bbPath);
            UpdateStatus("土地利用现状变化平衡统计表统计完成");
            //表3 土地利用现状一级分类面积按权属性质变化统计表
            UpdateStatus("正在统计土地利用现状一级分类面积按权属性质变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                output.clsOutputData.InitQsTable();
            output.clsOutputData.StatisticsYJFLQSXZ(dwdm, bbPath);
            UpdateStatus("土地利用现状一级分类面积按权属性质变化统计表统计完成");
            //表4 三大类土地利用现状变化平衡统计表
            UpdateStatus("正在统计三大类土地利用现状变化平衡统计表");
            output.clsOutputData.StatisticsSDL(dwdm);
            UpdateStatus("三大类土地利用现状变化平衡统计表统计完成");
            //表5 城镇村及工矿用地面积变化统计表
            UpdateStatus("正在统计城镇村及工矿用地面积变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                output.clsOutputData.InitCZCGKTable();
            output.clsOutputData.StatisticsCZCGK(dwdm, bbPath);
            UpdateStatus("城镇村及工矿用地面积变化统计表统计完成");
            //表6 耕地细化调查情况变化统计表
            UpdateStatus("正在统计耕地细化调查情况变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitGdxhdcTable();
            b = output.clsOutputData.StatisticsGDXH(dwdm, bbPath);
            if (b == false)
            {
                MessageBox.Show("地类编码不规范。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateStatus("耕地细化调查情况变化统计表统计完成");
            ////表7 基本农田统计汇总表
            //UpdateStatus("正在统计基本农田统计汇总表");
            //clsStatsJbnt2 statJbnt = new clsStatsJbnt2(this.currWs);
            //statJbnt.getYJJBNTTmp();
            //statJbnt.InitYjjbnt();
            //output.clsOutputData.StatisticsJBNT(dwdm);
            //UpdateStatus("基本农田统计汇总表统计完成");

            //表8 废弃与垃圾填埋细化标注变化统计表
            UpdateStatus("正在统计废弃与垃圾填埋细化标注变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitFQLJTMTable();
            output.clsOutputData.StatisticsFQLJTM(dwdm, bbPath);
            UpdateStatus("废弃与垃圾填埋细化标注变化统计表统计完成");

            //表9 部分细化地类面积变化统计表
            UpdateStatus("正在统计部分细化地类面积变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitBfxhdlTable();
            output.clsOutputData.StatisticsBFXHDL(dwdm, bbPath);
            UpdateStatus("部分细化地类面积变化统计表统计完成");

            //表10 可调整地类面积变化统计表
            UpdateStatus("正在统计可调整地类面积变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitKtzTable();
            output.clsOutputData.StatisticsKTZDL(dwdm, bbPath);
            UpdateStatus("可调整地类面积变化统计表统计完成");

            //表11 工业用地按类型汇总变化统计表
            UpdateStatus("正在统计工业用地按类型汇总变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitGYCCTable();
            output.clsOutputData.StatisticsGYYD(dwdm, bbPath);
            UpdateStatus("工业用地按类型汇总变化统计表统计完成");

            //表12 灌丛草地汇总情况变化统计表
            UpdateStatus("正在统计灌丛草地汇总情况变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitGCCDXSCDTable();
            output.clsOutputData.StatisticsGCCD(dwdm, bbPath);
            UpdateStatus("灌丛草地汇总情况变化统计表统计完成");

            //表13 林区范围内种植园用地变化统计表
            UpdateStatus("正在统计林区范围内种植园用地变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitLQYDTable();
            output.clsOutputData.StatisticsLQYD(dwdm, bbPath);
            UpdateStatus("林区范围内种植园用地变化统计表统计完成");

            //表14 即可恢复与工程恢复种植属性变化统计表
            string errdlbm = "";
            UpdateStatus("正在统计即可恢复与工程恢复种植属性变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitJKHFGCHFTable();
            b = output.clsOutputData.StatisticsJKHFYGCHF(dwdm, bbPath, ref errdlbm);
            if (b == false)
            {
                MessageBox.Show("地类编码为" + errdlbm + "存在恢复地类属性。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateStatus("即可恢复与工程恢复种植属性变化统计表统计完成");

            //表15 耕地种植类型面积变化统计表
            UpdateStatus("正在统计耕地种植类型面积变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitGdZzlxTable();
            output.clsOutputData.StatisticsGDZZLX(dwdm, bbPath);
            UpdateStatus("耕地种植类型面积变化统计表统计完成");

            //表16 耕地坡度分级面积变化统计表
            UpdateStatus("正在统计耕地坡度分级面积变化统计表");
            if (string.IsNullOrWhiteSpace(bbPath))
                clsOutputData.InitGdTable();
            output.clsOutputData.StatisticsBGGDPD(dwdm, bbPath);
            UpdateStatus("耕地坡度分级面积变化统计表统计完成");

            ////表17 无居民海岛现状调查分类面积变化统计表
            //UpdateStatus("正在统计无居民海岛现状调查分类面积变化统计表");
            //clsStatsWRHD hdtj = new clsStatsWRHD(this.currWs);
            //hdtj.InitWjmHd();
            //output.clsOutputData.StatisticsWRHD(dwdm);
            //UpdateStatus("无居民海岛现状调查分类面积变化统计表统计完成");
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void FrmReport_Load(object sender, EventArgs e)
        {
            beDestDir.Text = Application.StartupPath + @"\output\Excel";
            RCIS.GISCommon.ControlHelper.LoadXZQTreeBGH(tvXzq, currWs);

            try
            {
                string temptysd = Application.StartupPath + @"\SystemConf\backup.mdb";
                string tysd = Application.StartupPath + @"\SystemConf\result.mdb";
                if (File.Exists(tysd))
                {
                    File.Delete(tysd);
                    if (File.Exists(temptysd))
                        File.Copy(temptysd, tysd, true);
                }
                else
                {
                    if (File.Exists(temptysd))
                        File.Copy(temptysd, tysd, true);
                }
            }
            catch
            { }
            dateEdit1.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
            dateEdit1.Properties.ShowToday = false;
            //dateEdit1.Properties.ShowM = false;
            dateEdit1.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dateEdit1.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
            dateEdit1.Properties.Mask.EditMask = "yyyy";
            dateEdit1.Properties.Mask.UseMaskAsDisplayFormat = true;
            //string txtData = (System.DateTime.Now.Year - 1).ToString();
            double month = System.DateTime.Now.Month;
            if (month < 7)
                dateEdit1.EditValue = System.DateTime.Now.AddYears(-1);
            else
                dateEdit1.EditValue = System.DateTime.Now.AddYears(0);


            txttzhMJ.Enabled = false;

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

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.groupBoxTabs.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit)
                {
                    DevExpress.XtraEditors.CheckEdit ce = (DevExpress.XtraEditors.CheckEdit)c;
                    ce.Checked = true;
                }
            }
        }

        private void btnOutXlsSelect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(beDestDir.Text))
            {
                MessageBox.Show("请设置报表输出的文件夹。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string destDir = beDestDir.Text;
            if (!Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch
                {
                    MessageBox.Show("报表输出的文件夹设置错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            string dwdm = tvXzq.SelectedNode.Text.Substring(0, tvXzq.SelectedNode.Text.IndexOf('|'));
            string dwmc = tvXzq.SelectedNode.Text.Substring(tvXzq.SelectedNode.Text.IndexOf('|') + 1);

            int iScale = 1;
            if (rgDanwei.SelectedIndex == 1) iScale = 15;

            if (chkBGYLB.Checked)
            {
                //1变更一览表
                output.clsOutputData.ExportToExcel_BGYLB(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true);
            }
            if (chkBGPHB.Checked)
            {
                //表2土地利用现状变更表
                output.clsOutputData.ExportToExcel_BG_TDLYXZBGB(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true);
 
            }
            if (chkBGQSXZ.Checked)
            {
                //表3 耕地坡度分级面积变化统计表
                output.clsOutputData.ExportToExcel_BG_YJFLQSXZ(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGPHB3.Checked)
            {
                //表4 三大类土地利用现状变化平衡统计表
                output.clsOutputData.ExportToExcel_BG_SDL(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGCZC.Checked)
            {
                //表5 城镇村及工矿用地面积变化统计表
                output.clsOutputData.ExportToExcel_BG_CZCGK(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGGDXH.Checked)
            {
                //表6 耕地细化调查情况变化统计表
                output.clsOutputData.ExportToExcel_BG_GDXH(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chk7.Checked)
            {
                //表7基本农田汇总情况统计表
                output.clsOutputData.ExportToExcel_BG_JBNT(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGFQ.Checked)
            {
                //表8 废弃与垃圾填埋细化标注变化统计表
                output.clsOutputData.ExportToExcel_BG_FQLJTM(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGXHDL.Checked)
            {
                //表9 部分细化地类面积变化统计表
                output.clsOutputData.ExportToExcel_BG_BFXHDL(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGKTZ.Checked)
            {
                //表10 可调整地类面积变化统计表
                output.clsOutputData.ExportToExcel_BG_KTZDL(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGGY.Checked)
            {
                //表11 工业用地按类型汇总变化统计表
                output.clsOutputData.ExportToExcel_BG_GYYD(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGGCCD.Checked)
            {
                //表12 灌丛草地汇总情况变化统计表
                output.clsOutputData.ExportToExcel_BG_GCCD(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGLQYD.Checked)
            {
                //表13 林区范围内种植园用地变化统计表
                output.clsOutputData.ExportToExcel_BG_LQYD(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGGDHF.Checked)
            {
                //表14 即可恢复与工程恢复种植属性变化统计表
                output.clsOutputData.ExportToExcel_BG_JKHFYGCHF(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGGDZZLX.Checked)
            {
                //表15 耕地种植类型面积变化统计表
                output.clsOutputData.ExportToExcel_BG_GDZZLX(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chkBGGDPDFJ.Checked)
            {
                //表16 耕地坡度分级面积变化统计表
                output.clsOutputData.ExportToExcel_BG_GDPD(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
            if (chk17.Checked)
            {
                //表17 无居民海岛现状调查分类面积变化统计表
                output.clsOutputData.ExportToExcel_BG_WRHD(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, true, iScale);
            }
        }

        private void btnOutXlsAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(beDestDir.Text))
            {
                MessageBox.Show("请设置报表输出的文件夹。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string destDir = beDestDir.Text;
            if (!Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch
                {
                    MessageBox.Show("报表输出的文件夹设置错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            string dwdm = tvXzq.SelectedNode.Text.Substring(0, tvXzq.SelectedNode.Text.IndexOf('|'));
            string dwmc = tvXzq.SelectedNode.Text.Substring(tvXzq.SelectedNode.Text.IndexOf('|') + 1);
            int iScale = 1;
            if (rgDanwei.SelectedIndex == 1) iScale = 15;
            //1变更一览表
            output.clsOutputData.ExportToExcel_BGYLB(beDestDir.Text, dwdm, dwmc,dateEdit1.Text, false);
            //表2土地利用现状变更表
            output.clsOutputData.ExportToExcel_BG_TDLYXZBGB(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表3 耕地坡度分级面积变化统计表
            output.clsOutputData.ExportToExcel_BG_YJFLQSXZ(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表4 三大类土地利用现状变化平衡统计表
            output.clsOutputData.ExportToExcel_BG_SDL(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表5 城镇村及工矿用地面积变化统计表
            output.clsOutputData.ExportToExcel_BG_CZCGK(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表6 耕地细化调查情况变化统计表
            output.clsOutputData.ExportToExcel_BG_GDXH(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表7基本农田汇总情况统计表
            //output.clsOutputData.ExportToExcel_BG_JBNT(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表8 废弃与垃圾填埋细化标注变化统计表
            output.clsOutputData.ExportToExcel_BG_FQLJTM(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表9 部分细化地类面积变化统计表
            output.clsOutputData.ExportToExcel_BG_BFXHDL(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表10 可调整地类面积变化统计表
            output.clsOutputData.ExportToExcel_BG_KTZDL(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表11 工业用地按类型汇总变化统计表
            output.clsOutputData.ExportToExcel_BG_GYYD(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表12 灌丛草地汇总情况变化统计表
            output.clsOutputData.ExportToExcel_BG_GCCD(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表13 林区范围内种植园用地变化统计表
            output.clsOutputData.ExportToExcel_BG_LQYD(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表14 即可恢复与工程恢复种植属性变化统计表
            output.clsOutputData.ExportToExcel_BG_JKHFYGCHF(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表15 耕地种植类型面积变化统计表
            output.clsOutputData.ExportToExcel_BG_GDZZLX(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表16 耕地坡度分级面积变化统计表
            output.clsOutputData.ExportToExcel_BG_GDPD(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
            //表17 无居民海岛现状调查分类面积变化统计表
            //output.clsOutputData.ExportToExcel_BG_WRHD(beDestDir.Text, dwdm, dwmc, dateEdit1.Text, false, iScale);
        }

        

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            beDestDir.Text = dlg.SelectedPath;
        }

        private void filePathBB_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            filePathBB.Text = dlg.SelectedPath;
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                filePathBB.Text = "";
                filePathBB.Enabled = false;
                txtKzmjLD.Text = "";
                txtKzmjLD.Enabled = true;
            }
            else
            {
                filePathBB.Text = "";
                filePathBB.Enabled = true;
                txtKzmjLD.Text = "";
                txtKzmjLD.Enabled = false;
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                txttzhMJ.Enabled = true;
                txttzhMJ.Text = "";
            }
            else
            {
                txttzhMJ.Enabled = false;
                txttzhMJ.Text = "";
            }
        }

        
    }
}
