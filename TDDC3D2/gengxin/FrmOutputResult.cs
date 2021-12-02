using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Database;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using RCIS.DataInterface.VCTOut;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using System.Runtime.InteropServices;
using TDDC3D.output;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Collections;

namespace TDDC3D.gengxin
{
    public partial class FrmOutputResult : Form
    {
        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(int hProcess);

        public FrmOutputResult()
        {
            InitializeComponent();
        }
        public IWorkspace currWorkspace;
        int currDh;
        string destExcelDir;
        string CurrDW = "公顷";
        //记录 类名对应的中文名  要素名
        private Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
        private Dictionary<string, string> dicClassCNName = new Dictionary<string, string>();
        private Dictionary<string, string> dicQsdwdm = new Dictionary<string, string>();

        private void txtExportPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dia = new FolderBrowserDialog();
            if (dia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtExportPath.Text = dia.SelectedPath;
            }
        }

        private void txtMetaData_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "元数据文件(*.xml)|*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtMetaData.Text = ofd.FileName;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //try
            //{
            if (string.IsNullOrWhiteSpace(txtKzmjLD.Text) && radioGroup1.SelectedIndex == 0)
            {
                MessageBox.Show("请输入控制面积或者改用基础报表方式取得年初面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string bbPath = "";
            if (radioGroup1.SelectedIndex == 1)
                bbPath = filePathBB.Text;
            if (radioGroup1.SelectedIndex == 1 && string.IsNullOrWhiteSpace(bbPath))
            {
                MessageBox.Show("请选择基础报表路径或者改用图斑方式统计年初面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtExportPath.Text) || string.IsNullOrWhiteSpace(txtXZQDM.Text) || txtXZQDM.Text.Length != 6)
            {
                MessageBox.Show("请设置功能参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (buttonEdit1.Text.Length > 0 && buttonEdit1.Text.Substring(buttonEdit1.Text.LastIndexOf("\\") + 1) != "基础数据包")
            {
                MessageBox.Show("基础数据包路径选择错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<string> erroMessage =null;
            clsOutputData.checkData((RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC"), bbPath, txtXZQDM.Text, ref erroMessage);
            if (erroMessage.Count > 0)
            {
                char c = (char)13;
                MessageBox.Show(string.Join(c.ToString(),erroMessage),"提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            #region 创建生成成果包的目录
            //创建生成成果包的目录
            DataRow dr1 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + txtXZQDM.Text.Substring(0, 2) + @"0000""", "tmp");
            DataRow dr2 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + txtXZQDM.Text.Substring(0, 4) + @"00""", "tmp");
            DataRow dr3 = LS_SetupMDBHelper.GetDataRow(@"select * from SYS_XZQ Where DM = """ + txtXZQDM.Text + @"""", "tmp");
            string sheng = dr1 == null ? "" : dr1[1].ToString();
            string shi = dr2 == null ? "" : dr2[1].ToString();
            string xian = dr3 == null ? "" : dr3[1].ToString();
            string destFolder = string.Format(txtExportPath.Text + "\\{0}{1}{2}({3})"+dateEdit1.Text+"年度国土变更调查数据库更新成果", sheng, shi, xian, txtXZQDM.Text);
            if (Directory.Exists(destFolder))
            {
                MessageBox.Show("成果目录已经存在，请修改导出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Directory.CreateDirectory(destFolder);
            RCIS.Utility.File_DirManipulate.FolderCopy(Application.StartupPath + "\\SystemConf\\第三次国土调查统一时点更新数据成果", destFolder);
            destExcelDir = destFolder + "\\更新数据包\\汇总表格\\";
            //if (File.Exists(Application.StartupPath + "\\template\\第三次全国国土调查有关情况统计表.xlsx"))
            //    File.Copy(Application.StartupPath + "\\template\\第三次全国国土调查有关情况统计表.xlsx", destExcelDir + "\\" + dateEdit1.Text + "(" + txtXZQDM.Text + ")第三次全国国土调查有关情况统计表.xlsx");
            #endregion

            if (chkOnlyReport.Checked)
            {
                StatisticsBGB(txtXZQDM.Text,bbPath);
                ExportBGB(destFolder + "\\更新数据包\\汇总表格", txtXZQDM.Text, xian);
                //RCIS.Database.LS_TysdMDBHelper.ConnectionClose();

            }
            else
            {
                //报表
                StatisticsBGB(txtXZQDM.Text, bbPath);
                ExportBGB(destFolder + "\\更新数据包\\汇总表格", txtXZQDM.Text, xian);
                //VCT
                LsGxClass gx = new LsGxClass();
                gx.xzdm = txtXZQDM.Text;
                //gx.bgData = bgData;
                //gx.pMapCtl = pMapCtl;
                gx.info = memoLog;
                string temppath = System.Environment.GetEnvironmentVariable("TEMP");
                if (temppath.EndsWith("\\"))
                    temppath += "tmp";
                else temppath += "\\tmp";
                gx.VCTDataPrepare(temppath);
                string filePath = destFolder + "\\更新数据包\\标准格式数据\\" + "2001H2020" + txtXZQDM.Text + ".VCT";

                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "XZQDM LIKE '" + txtXZQDM.Text + "%'";
                IFeatureClass pXZQ = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
                IFeature pFea = RCIS.GISCommon.GetFeaturesHelper.GetFirstFeature(pXZQ, pQf);
                IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                double X = selectPoint.X;
                currDh = (int)(X / 1000000);////WK---带号

                gx.VCTDataOutput(filePath, temppath, currDh);

                if (string.IsNullOrWhiteSpace(txtMetaData.Text))
                    File.Copy(Application.StartupPath + @"\template\metadata_bgdc.xml", destFolder + "\\更新数据包\\标准格式数据\\" + "2001H2020" + txtXZQDM.Text + ".xml");
                else
                    File.Copy(txtMetaData.Text, destFolder + "\\更新数据包\\标准格式数据\\" + "2001H2020" + txtXZQDM.Text + ".xml");
                //基础数据包
                string jcbPath = buttonEdit1.Text;
                if (!string.IsNullOrWhiteSpace(jcbPath))
                {
                    UpdateStatus("正在生成基础数据包...");

                    RCIS.Utility.File_DirManipulate.FolderCopy(jcbPath, destFolder + "\\基础数据包");

                }
                //原格式数据
                UpdateStatus("正在输出原格式数据...");
                string gdbPath = destFolder + "\\更新数据包\\原格式数据";
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(gdbPath, txtXZQDM.Text + "GX.gdb", null, 0);
                pWorkspaceFactory.Create(gdbPath, txtXZQDM.Text + "GXGC.gdb", null, 0);
                RCIS.Utility.OtherHelper.ReleaseComObject(pWorkspaceFactory);
                IWorkspace pGXWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(gdbPath + "\\" + txtXZQDM.Text + "GX.gdb");
                IWorkspace pGXGCWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(gdbPath + "\\" + txtXZQDM.Text + "GXGC.gdb");
                IDataset pdataSet = (this.currWorkspace as IFeatureWorkspace).OpenFeatureDataset("TDGX");
                IFeatureDataset pGXDataset = (pGXWS as IFeatureWorkspace).CreateFeatureDataset(pdataSet.Name, (pdataSet as IGeoDataset).SpatialReference);
                IFeatureDataset pGXGCDataset = (pGXGCWS as IFeatureWorkspace).CreateFeatureDataset(pdataSet.Name, (pdataSet as IGeoDataset).SpatialReference);
                IFeatureClassContainer pClassContaniner = pdataSet as IFeatureClassContainer;
                for (int i = 0; i < pClassContaniner.ClassCount; i++)
                {
                    IFeatureClass pFC = pClassContaniner.get_Class(i);
                    string name = (pFC as IDataset).Name;
                    if (name.Substring(name.Length - 2) == "GC")
                    {
                        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(this.currWorkspace, pGXGCWS, name, name, pGXGCDataset, null);
                        IClassSchemaEdit2 pClassSchemaEdit2 = (pGXGCWS as IFeatureWorkspace).OpenFeatureClass(name) as IClassSchemaEdit2;
                        pClassSchemaEdit2.AlterAliasName(pFC.AliasName);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pClassSchemaEdit2);
                    }
                    else
                    {
                        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(this.currWorkspace, pGXWS, name, name, pGXDataset, null);
                        IClassSchemaEdit2 pClassSchemaEdit2 = (pGXWS as IFeatureWorkspace).OpenFeatureClass(name) as IClassSchemaEdit2;
                        pClassSchemaEdit2.AlterAliasName(pFC.AliasName);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pClassSchemaEdit2);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pFC);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pGXDataset);
                RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCDataset);
                RCIS.Utility.OtherHelper.ReleaseComObject(pdataSet);
                RCIS.Utility.OtherHelper.ReleaseComObject(pClassContaniner);
                RCIS.Utility.OtherHelper.ReleaseComObject(pGXWS);
                RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCWS);
            }
            UpdateStatus("国土变更调查数据库更新成果包输出完毕...");
            MessageBox.Show("导出完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch(Exception ex)
            //{
            //    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, ex.ToString());
            //    return;
            //}
            
            
        }

        private void ExportBGB(string destDir, string dwdm, string dwmc) 
        {
            int iScale = 1;
            //if (rgDanwei.SelectedIndex == 1) iScale = 15;
            //表2 变更一览表
            UpdateStatus("正在输出变更一览表...");
            output.clsOutputData.ExportToExcel_BGYLB(destDir, dwdm, dwmc,dateEdit1.Text, false);

            //表2 土地利用现状变更表
            UpdateStatus("正在输出土地利用现状变更表...");
            output.clsOutputData.ExportToExcel_BG_TDLYXZBGB(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表3 耕地坡度分级面积变化统计表
            UpdateStatus("正在输出耕地坡度分级面积变化统计表...");
            output.clsOutputData.ExportToExcel_BG_YJFLQSXZ(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表3 三大类土地利用现状变化平衡统计表
            UpdateStatus("正在输出三大类土地利用现状变化平衡统计表...");
            output.clsOutputData.ExportToExcel_BG_SDL(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表5 城镇村及工矿用地面积变化统计表
            UpdateStatus("正在输出城镇村及工矿用地面积变化统计表...");
            output.clsOutputData.ExportToExcel_BG_CZCGK(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表6 耕地细化调查情况变化统计表
            UpdateStatus("正在输出耕地细化调查情况变化统计表...");
            output.clsOutputData.ExportToExcel_BG_GDXH(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            ////表7基本农田汇总情况统计表
            //UpdateStatus("正在输出基本农田汇总情况统计表...");
            //output.clsOutputData.ExportToExcel_BG_JBNT(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表8 废弃与垃圾填埋细化标注变化统计表
            UpdateStatus("正在输出废弃与垃圾填埋细化标注变化统计表...");
            output.clsOutputData.ExportToExcel_BG_FQLJTM(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表9 部分细化地类面积变化统计表
            UpdateStatus("正在输出部分细化地类面积变化统计表...");
            output.clsOutputData.ExportToExcel_BG_BFXHDL(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表10 可调整地类面积变化统计表
            UpdateStatus("正在输出可调整地类面积变化统计表...");
            output.clsOutputData.ExportToExcel_BG_KTZDL(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表11 工业用地按类型汇总变化统计表
            UpdateStatus("正在输出工业用地按类型汇总变化统计表...");
            output.clsOutputData.ExportToExcel_BG_GYYD(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表12 灌丛草地汇总情况变化统计表
            UpdateStatus("正在输出灌丛草地汇总情况变化统计表...");
            output.clsOutputData.ExportToExcel_BG_GCCD(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表13 林区范围内种植园用地变化统计表
            UpdateStatus("正在输出林区范围内种植园用地变化统计表...");
            output.clsOutputData.ExportToExcel_BG_LQYD(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表14 即可恢复与工程恢复种植属性变化统计表
            UpdateStatus("正在输出即可恢复与工程恢复种植属性变化统计表...");
            output.clsOutputData.ExportToExcel_BG_JKHFYGCHF(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表15 耕地种植类型面积变化统计表
            UpdateStatus("正在输出耕地种植类型面积变化统计表...");
            output.clsOutputData.ExportToExcel_BG_GDZZLX(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            //表16 耕地坡度分级面积变化统计表
            UpdateStatus("正在输出耕地坡度分级面积变化统计表...");
            output.clsOutputData.ExportToExcel_BG_GDPD(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

            ////表17 无居民海岛现状调查分类面积变化统计表
            //output.clsOutputData.ExportToExcel_BG_WRHD(destDir, dwdm, dwmc, dateEdit1.Text, false, iScale);

        }

        private void StatisticsBGB(string dwdm,string bbPath) 
        {
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

            Application.DoEvents();
            //RCIS.Database.LS_TysdMDBHelper.ConnectionOpen();


            if (string.IsNullOrWhiteSpace(bbPath))
            {
                UpdateStatus("正在提取数据...");
                clsOutputData.CopyDltb(currWorkspace);
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
            output.clsOutputData.StatisticsBGLYB(dwdm, currWorkspace);
            UpdateStatus("变更一览表统计完成");

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
                    string val = excelRow[1].ToString();
                    double.TryParse(val, out kzmj);
                }
                double bhmj = Math.Round(bghkzmj, 2) - Math.Round(kzmj, 2);
                System.Threading.Thread.Sleep(10000);
                clsOutputData.AreaLeveling(Math.Round(bhmj, 2));
            }

            UpdateStatus("变更基础表统计完成");

            StatisticsBB(dwdm, bbPath);

            System.Threading.Thread.Sleep(10000);
            //调整后年初面积出现负值平差
            DataTable dt = clsOutputData.StatisticsDatatableTZHNC(dwdm);
            if (dt.Rows.Count > 0)
            {
                UpdateStatus("正在进行年度变更基础表平差...");
                clsOutputData.StatisticsPC(dt, dwdm, bbPath,true);
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

        private void VCTDataOutput(string destFile, string temppath, int iDh) 
        {
            #region 错误控制

            //string destFile = filePath;
            if (!destFile.ToUpper().EndsWith(".VCT"))
            {
                destFile += ".VCT";
            }
            destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile) + "GXGC" + System.IO.Path.GetExtension(destFile);
            if (System.IO.File.Exists(destFile))
            {
                System.IO.File.Delete(destFile);
            }
            //预处理

            if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
            }

            RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");


            IFeatureWorkspace pFeaWs = this.currWorkspace as IFeatureWorkspace;
            IWorkspace2 pWS2 = currWorkspace as IWorkspace2;
            //找到数据集
            IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = currWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }


            List<TableStruct> lstTables = this.GetLstTables(featureDataset, "GX");  //获取所有表结构
            if (lstTables.Count == 0)
            {
                UpdateStatus("当前数据库中没有需要导出的数据，退出...");
                return;
            }



            //string temppath = this.beTmpDir.Text;
            // VCTOut11 outvct = new VCTOut11(temppath);   
            VCTOut12 outvct = new VCTOut12(temppath);
            //int iDh = 0;
            //int.TryParse(this.txtDH.Text.Trim(), out iDh);
            outvct.dh = iDh;
            outvct.gdbWorkspace = this.currWorkspace as IFeatureWorkspace;
            outvct.gdbDataset = featureDataset;
            outvct.allTableStruct = lstTables;
            outvct.DoByAXzq = true;
            outvct.includezj = false;
            #endregion
            //导出shp
            try
            {
                //outvct.ModifyAllTabs();
                UpdateStatus("开始导出文件头...");
                outvct.ExportFileHead3();
                UpdateStatus("导出文件头结束...");
                outvct.ExportPoint3();

                UpdateStatus("导出点文件结束...");
                outvct.ExportLine3();

                UpdateStatus("导出线文件结束...");
                outvct.ExportFill3();
                UpdateStatus("导出面文件结束...");
                outvct.ExportAnotation3();
                UpdateStatus("导出注记结束...");
                outvct.ExportAttribute3();
                UpdateStatus("导出属性结束...");

                string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
                System.Array.Sort(allFiles);
                ConcatenateFiles(destFile, allFiles);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle.ToInt32());
                UpdateStatus("合并完成，导出完毕！");


            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }

            #region 错误控制
            
            //destFile = filePath;
            if (!destFile.ToUpper().EndsWith(".VCT"))
            {
                destFile += ".VCT";
            }

            destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile).Substring(0, System.IO.Path.GetFileNameWithoutExtension(destFile).Length-2) + System.IO.Path.GetExtension(destFile);
            if (System.IO.File.Exists(destFile))
            {
                System.IO.File.Delete(destFile);
            }
            //预处理

            if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
            }

            RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");


            pFeaWs = this.currWorkspace as IFeatureWorkspace;
            pWS2 = currWorkspace as IWorkspace2;
            //找到数据集
            featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = currWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }


            lstTables = this.GetLstTables(featureDataset, "GXGC");  //获取所有表结构
            if (lstTables.Count == 0)
            {
                UpdateStatus("当前数据库中没有需要导出的数据，退出...");
                return;
            }



            //temppath = this.beTmpDir.Text;
            // VCTOut11 outvct = new VCTOut11(temppath);   
            outvct = new VCTOut12(temppath);
            //iDh = 0;
            //int.TryParse(this.txtDH.Text.Trim(), out iDh);
            outvct.dh = iDh;
            outvct.gdbWorkspace = this.currWorkspace as IFeatureWorkspace;
            outvct.gdbDataset = featureDataset;
            outvct.allTableStruct = lstTables;
            outvct.DoByAXzq = true;
            outvct.includezj = false;
            #endregion
            //导出shp
            try
            {
                //outvct.ModifyAllTabs();
                UpdateStatus("开始导出文件头...");
                outvct.ExportFileHead3();
                UpdateStatus("导出文件头结束...");
                outvct.ExportPoint3();

                UpdateStatus("导出点文件结束...");
                outvct.ExportLine3();

                UpdateStatus("导出线文件结束...");
                outvct.ExportFill3();
                UpdateStatus("导出面文件结束...");
                outvct.ExportAnotation3();
                UpdateStatus("导出注记结束...");
                outvct.ExportAttribute3();
                UpdateStatus("导出属性结束...");

                string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
                System.Array.Sort(allFiles);
                ConcatenateFiles(destFile, allFiles);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle.ToInt32());
                UpdateStatus("合并完成，导出完毕！");

                //MessageBox.Show("导出结束！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }
        }

        private void ConcatenateFiles(string outputFile, string[] inputFiles)
        {
            using (Stream output = File.OpenWrite(outputFile))
            {
                foreach (string inputFile in inputFiles)
                {
                    using (Stream input = File.OpenRead(inputFile))
                    {
                        input.CopyTo(output);
                    }
                }
            }
        }

        public List<TableStruct> GetLstTables(IDataset mDataset, string NotEnd)
        {
            List<TableStruct> lstTables = new List<TableStruct>();

            IEnumDataset penumDataset = mDataset.Subsets;
            IDataset table = penumDataset.Next();
            while (table != null)
            {
                if (table.Name.ToUpper().StartsWith("TP_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                if (table.Name.Contains("_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                //掠过zj
                if (table.Name.ToUpper() == "ZJ")
                {
                    table = penumDataset.Next();
                    continue;
                }
                if (table.Name.ToUpper().EndsWith(NotEnd))
                {
                    table = penumDataset.Next();
                    continue;
                }

                IFeatureClass tableCls = table as IFeatureClass;
                string sName = table.Name;
                string sGraph = "Point";
                if (tableCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    sGraph = "Point";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    sGraph = "Line";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    sGraph = "Polygon";
                }

                if (dicClassCNName.ContainsKey(sName))
                {
                    string featureCode = dicClassYsdm[sName];
                    string FeatureName = dicClassCNName[sName];
                    lstTables.Add(new TableStruct(FeatureName, featureCode, sName, sGraph));
                }


                table = penumDataset.Next();
            }
            return lstTables;

        }

        public List<TableStruct> GetLstTables(IDataset mDataset)
        {
            List<TableStruct> lstTables = new List<TableStruct>();

            IEnumDataset penumDataset = mDataset.Subsets;
            IDataset table = penumDataset.Next();
            while (table != null)
            {
                if (table.Name.ToUpper().StartsWith("TP_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                if (table.Name.Contains("_"))
                {
                    table = penumDataset.Next();
                    continue;
                }
                //掠过zj
                if (table.Name.ToUpper() == "ZJ")
                {
                    table = penumDataset.Next();
                    continue;
                }

                IFeatureClass tableCls = table as IFeatureClass;
                string sName = table.Name;
                string sGraph = "Point";
                if (tableCls.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    sGraph = "Point";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    sGraph = "Line";
                }
                else if (tableCls.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    sGraph = "Polygon";
                }

                if (dicClassCNName.ContainsKey(sName))
                {
                    string featureCode = dicClassYsdm[sName];
                    string FeatureName = dicClassCNName[sName];
                    lstTables.Add(new TableStruct(FeatureName, featureCode, sName, sGraph));
                }


                table = penumDataset.Next();
            }
            return lstTables;

        }

        private void VCTDataPrepare(string temppath) 
        {
            IFeatureDataset featureDataset = (this.currWorkspace as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = currWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }

            //string temppath = this.beTmpDir.Text;
            if (!Directory.Exists(temppath))
            {
                Directory.CreateDirectory(temppath);
            }
            string fileName = "vct.gdb";
            string shpPath = temppath + "\\" + fileName;

            if (!System.IO.Directory.Exists(shpPath))
            {
                //不存在则创建
                //创建一个临时库

                try
                {
                    IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                    pWorkspaceFactory.Create(temppath, fileName, null, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            else
            {
                IWorkspace pTmpWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(shpPath);
                if (pTmpWorkspace == null)
                {
                    RCIS.Utility.FileHelper.DelectDir(shpPath);
                    System.IO.Directory.Delete(shpPath);
                }
                else
                    (pTmpWorkspace as IDataset).Delete();
                try
                {
                    IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                    pWorkspaceFactory.Create(temppath, fileName, null, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            UpdateStatus("正在进行预处理...");
            IWorkspace pTarWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(shpPath);
            IEnumDataset srcDS = pTarWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            IDataset aDs = null;
            while ((aDs = srcDS.Next()) != null)
            {
                try
                {
                    aDs.Delete();
                }
                catch { }
            }


            List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构            
            //导出各要素
            try
            {
                foreach (TableStruct ts in lstTables)
                {
                    IFeatureClass pFC = null;
                    try
                    {
                        pFC = (this.currWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
                        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(this.currWorkspace, pTarWorkspace, ts.className, ts.className, null);
                    }
                    catch { }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //2021年3月22日08:46:22   数据过滤   只是zldwdm或者qsdwdm变更的，不进入更新层和更新过程层

            try
            {
                //IFeatureClass pGXGCClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
                //pTarWorkspace.ExecuteSQL("update dltbgxgc set bgqgddb='0'  where bgqgddb='' or bgqgddb=' ' or bgqgddb is null");
                //IQueryFilter pQf = new QueryFilterClass();
                //pQf.WhereClause = "bgxw='1' and bgqdlbm=bghdlbm and bgqqsxz=bghqsxz and bgqkcdlbm=bghkcdlbm and bgqgdlx=bghgdlx and bgqgdpdjb=bghgdpdjb and bgqtbxhdm=bghtbxhdm and bgqzzsxdm=bghzzsxdm and bgqgddb=bghgddb and bgqfrdbs=bghfrdbs and bgqczcsxm=bghczcsxm and bgqmssm=bghmssm and bgqhdmc=bghhdmc";
                //ArrayList arr = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pGXGCClass, pQf, "BGHTBBSM");
                //for (int i = 0; i < arr.Count; i++)
                //{
                //    string bsm = arr[i].ToString();
                //    pTarWorkspace.ExecuteSQL("delete from dltbgx where bsm='" + bsm + "'");
                //}

                IFeatureClass pGXGCClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
                IFeatureClass pGXClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "BGXW='1'";
                IFeatureCursor pFeaCursor = pGXGCClass.Update(pQf, true);
                IFeature pFeature;
                while ((pFeature = pFeaCursor.NextFeature()) != null)
                {
                    bool isDel = true;
                    for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                    {
                        string filedName = pFeature.Fields.Field[i].Name.ToString().Trim().ToUpper();

                        if (filedName.Contains("BGQ") && !filedName.Contains("BSM") && !filedName.Contains("TBBH") && !filedName.Contains("ZLDWDM") && !filedName.Contains("ZLDWMC") && !filedName.Contains("QSDWDM") && !filedName.Contains("QSDWMC"))
                        {
                            if (pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() != pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim())
                            {
                                if (filedName == "BGQGDDB" && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() == "0") && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim() == "0"))
                                    continue;
                                else
                                {
                                    isDel = false;
                                    break;
                                }

                            }
                        }
                    }
                    if (isDel)
                    {
                        //string bghbsm = pFeature.get_Value(pFeature.Fields.FindField("BGHTBBSM")).ToString().Trim();
                        //pTarWorkspace.ExecuteSQL("delete from dltbgx where bsm='" + bghbsm + "'");
                        pFeature.set_Value(pFeature.Fields.FindField("BSM"), "DEL");
                        pFeaCursor.UpdateFeature(pFeature);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                }
                pFeaCursor.Flush();
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
                IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
                IQueryFilter pQfilter = new QueryFilterClass();
                pQfilter.WhereClause = "bsm='DEL'";
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pTarWorkspace, pTmpWs, "DLTBGXGC", "GXGC", pQfilter);
                pTarWorkspace.ExecuteSQL("delete from dltbgxgc where bsm='DEL'");
                bool b = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(pTarWorkspace.PathName + "\\DLTBGX", pTmpWs.PathName + "\\GXGC", pTmpWs.PathName + "\\SpatialJoinGx", "CONTAINS");
                if (!b)
                {
                    UpdateStatus("叠加分析错误");
                    return;
                }
                pTarWorkspace.ExecuteSQL("delete from dltbgx");
                pTmpWs.ExecuteSQL("delete from SpatialJoinGx where Join_Count=1");

                IFeatureClass pSDClass = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("SpatialJoinGx");
                for (int i = pSDClass.Fields.FieldCount - 1; i >= 0; i--)
                {
                    IField pField = pSDClass.Fields.get_Field(i);
                    if (pField.Name.Contains("_1"))
                        (pSDClass as ITable).DeleteField(pField);
                }

                b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\SpatialJoinGx",pTarWorkspace.PathName + "\\DLTBGX");
                if (!b)
                {
                    UpdateStatus("叠加分析错误");
                    return;
                }
                RCIS.GISCommon.GpToolHelper.RepairGeometry(pTarWorkspace.PathName + "\\DLTBGX");
                RCIS.GISCommon.GpToolHelper.RepairGeometry(pTarWorkspace.PathName + "\\DLTBGXGC");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            bool bRet = true;
            //执行面转线
            this.Cursor = Cursors.WaitCursor;
            IWorkspace2 wsname2 = pTarWorkspace as IWorkspace2;
            foreach (TableStruct ts in lstTables)
            {
                if (ts.type.ToUpper() == "POLYGON")
                {
                    string shpfileName = shpPath + "\\" + ts.className.ToUpper();
                    string lineShpFile = shpPath + "\\" + ts.className.ToUpper() + "line";
                    if (wsname2.get_NameExists(esriDatasetType.esriDTFeatureClass, ts.className))
                    {
                        IFeatureClass pFClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
                        if (pFClass.FeatureCount(null) > 0)
                        {
                            bRet &= PolygonToline(shpfileName, lineShpFile);
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFClass);
                    }
                }
            }

            this.Cursor = Cursors.Default;
            if (bRet == false)
            {
                UpdateStatus("关联图层失败，退出...");

                return;
            }

            if (bRet == false)
            {
                UpdateStatus("关联图层失败，退出...");

                return;
            }
            else
            {
                UpdateStatus("数据准备完成，请继续！");
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pTarWorkspace);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //MessageBox.Show("数据准备完成，请继续后续工作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool PolygonToline(string dltbFile, string lineFile)
        {
            Geoprocessor gp = new Geoprocessor();
            ESRI.ArcGIS.DataManagementTools.PolygonToLine toLine = new ESRI.ArcGIS.DataManagementTools.PolygonToLine();
            try
            {

                toLine.in_features = dltbFile;
                toLine.out_feature_class = lineFile;
                toLine.neighbor_option = "IDENTIFY_NEIGHBORS";
                IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(toLine, null);
                string sMsg = "";
                if (gp.MessageCount > 0)
                {
                    for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                    {
                        sMsg += gp.GetMessage(Count);
                    }
                }
                if (sMsg.Contains("ERROR") || sMsg.Contains("失败"))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        

        private void UpdateStatus(string txt)
        {
            memoLog.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + memoLog.Text;
            Application.DoEvents();
        }

        private void FrmOutputResult_Load(object sender, EventArgs e)
        {
            //获取所有要素代码
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM where type in ('POINT','LINE','POLYGON') ", "ysdm");
            foreach (DataRow dr in dt.Rows)
            {
                dicClassCNName.Add(dr["CLASSNAME"].ToString(), dr["ALIASNAME"].ToString());
                dicClassYsdm.Add(dr["CLASSNAME"].ToString(), dr["YSDM"].ToString());
            }
            if (currWorkspace != null)
            {
                IFeatureClass pXZQClass = null;
                //string xzdm = "";
                try
                {
                    pXZQClass = (currWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
                }
                catch { }
                //currDh = 38;
                //if (pXZQClass != null)
                //{
                //    IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                //    if (firstFea != null)
                //    {
                //        xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                //        IPoint selectPoint = (firstFea.ShapeCopy as IArea).Centroid;
                //        double X = selectPoint.X;
                //        currDh = (int)(X / 1000000);////WK---带号
                //        txtXZQDM.Text = xzdm.Length >= 6 ? xzdm.Substring(0, 6) : "";
                //    }
                //}

                //县代码
                if (pXZQClass != null)
                {
                    List<string> dms = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                    txtXZQDM.Properties.Items.Clear();
                    foreach (string dm in dms)
                    {
                        txtXZQDM.Properties.Items.Add(dm);
                    }
                    if (txtXZQDM.Properties.Items.Count > 0) txtXZQDM.SelectedIndex = 0;
                }
            }

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
            radioGroup1.SelectedIndex = 1;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dia = new FolderBrowserDialog();
            if (dia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                buttonEdit1.Text = dia.SelectedPath;
                if (Directory.Exists(dia.SelectedPath + "\\汇总表格"))
                {
                    radioGroup1.SelectedIndex = 1;
                    filePathBB.Text = dia.SelectedPath + "\\汇总表格";
                }
            }
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
