using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using RCIS.Utility;
using System.IO;
using RCIS.Database;
using ESRI.ArcGIS.Geometry;
using RCIS.DataInterface.VCTOut;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace TDDC3D.output
{
    public partial class OutStandardForm : Form
    {
        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(int hProcess);

        public IWorkspace currWorkspace;
        int currDh;
        string destExcelDir;
        string CurrDW = "公顷";
        //记录 类名对应的中文名  要素名
        private Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
        private Dictionary<string, string> dicClassCNName = new Dictionary<string, string>();
        private Dictionary<string, string> dicQsdwdm = new Dictionary<string, string>();

        public OutStandardForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResultPacket1()
        {
            string temppath = Application.StartupPath + @"\output\";
            if (temppath.EndsWith("\\"))
                temppath += "tmp";
            else temppath += "\\tmp";
            IFeatureDataset featureDataset = (this.currWorkspace as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
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
                    }
                }
            }

            this.Cursor = Cursors.Default;

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
            }
        }
        /// <summary>
        /// 面转线
        /// </summary>
        /// <param name="dltbFile"></param>
        /// <param name="lineFile"></param>
        /// <returns></returns>
        public bool PolygonToline(string dltbFile, string lineFile)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                ESRI.ArcGIS.DataManagementTools.PolygonToLine toLine = new ESRI.ArcGIS.DataManagementTools.PolygonToLine();
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
        }
        private void ResultPacket2(string destFolder)
        {
            string temppath = Application.StartupPath + @"\output\"; 
            if (temppath.EndsWith("\\"))
                temppath += "tmp";
            else temppath += "\\tmp";

            #region 错误控制
            string destFile = destFolder;
            if (!destFile.ToUpper().EndsWith(".VCT"))
            {
                destFile += ".VCT";
            }
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
            IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
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


            List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构
            if (lstTables.Count == 0)
            {
                UpdateStatus("当前数据库中没有需要导出的数据，退出...");
                return;
            }

            // VCTOut11 outvct = new VCTOut11(temppath);   
            VCTOut12 outvct = new VCTOut12(temppath);
            int iDh = this.currDh;
            
            outvct.dh = iDh;
            outvct.gdbWorkspace = this.currWorkspace as IFeatureWorkspace;
            outvct.gdbDataset = featureDataset;
            outvct.allTableStruct = lstTables;
            outvct.DoByAXzq = this.chkByXzqDo.Checked;

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
                //VCTOutPublic.SaveXml(lstTables);
                //gdaloutline.outline outlines = new gdaloutline.outline(temppath);
                //string err = "";
                //bool bOk= outlines.ExportLine3(ref err);


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

                //UpdateStatus("查询线所需时间:" + outvct.findTime.Elapsed.ToString());
                //UpdateStatus("排序线所需时间:" + outvct.sortTime.Elapsed.ToString());

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

        private void ResultPacket3(string destFolder)
        {
            try
            {
            //生成VCT索引
                string destFile = destFolder + "\\基础数据包\\标准格式数据\\" + "2001H2019" + txtXZQDM.Text + ".VCT";
                UpdateStatus("开始生成索引...");
                VCTIdxCreator create = new VCTIdxCreator();
                create.VctFile = destFile;
                create.AllYsdmClassName = output.clsOutputData.getYsdmClassName();
                create.GetAllAttrsPos();
                UpdateStatus("属性位置获取完毕！");
                create.WritePointIdx();
                UpdateStatus("点索引文件生成完毕!");
                create.WriteRealLineIdx();
                UpdateStatus("真实线索引文件生成完毕！");
                create.WriteTopLineIdx();
                UpdateStatus("拓扑线索引文件生成完毕！");

                create.WritePolygonIdx();
                UpdateStatus("面索引文件生成完毕！");

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                //UpdateStatus(ex.ToString());
            }
        }

        private void ResultPacket4()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                UpdateStatus("开始从矢量中提取地类图斑数据...");
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

                UpdateStatus("正在提取数据...");
                Application.DoEvents();
                //if (!clsOutputData.SpatialJoin(currWs))
                //{
                //    this.Cursor = Cursors.Default;
                //    MessageBox.Show("空间关系处理不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                clsOutputData.CopyDltb(currWorkspace);
                //clsOutputData.CopyJBNT(currWorkspace);

                UpdateStatus("正在初步进行平方米统计...");
                Application.DoEvents();
                clsOutputData.Dltb2BaseTable2();

                UpdateStatus("正在进行生成基础统计...");
                Application.DoEvents();
                clsOutputData.ChangeTMP2JCB("PMMJ");

                double ldmj = 0;
                double.TryParse(txtKzmjLD.Text, out ldmj);
                double hdmj = 0;
                double.TryParse(txtKzmjHD.Text, out hdmj);

                UpdateStatus("正在进行调平.....");
                Application.DoEvents();
                ClsTP tp = new ClsTP();
                tp.ChangeTableDW2GQ("HZ_JCB");  //转化为公顷
                tp.MakeBalance("00",ldmj);
                tp.MakeBalance("01",hdmj);

                UpdateStatus("正在将基础表数据进行按辖区坐落汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按辖区坐落汇总统计...";
                clsOutputData.InitZlTable();

                UpdateStatus("正在将基础表数据进行按权属性质汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按权属性质汇总统计...";
                clsOutputData.InitQsTable();

                UpdateStatus("正在将基础表数据进行按耕地坡度汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按耕地坡度汇总统计...";
                clsOutputData.InitGdTable();

                UpdateStatus("正在将城镇村及工矿用地面积汇总表统计...");
                Application.DoEvents();
                clsOutputData.InitCZCGKTable();

                UpdateStatus("正在将基础表数据进行按耕地种植类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGdZzlxTable();

                UpdateStatus("正在将基础表数据进行林区园地汇总统计...");
                Application.DoEvents();
                clsOutputData.InitLQYDTable();

                UpdateStatus("正在将基础表数据进行灌丛草地汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGCCDXSCDTable();

                UpdateStatus("正在将基础表数据进行工业用地类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGYCCTable();

                UpdateStatus("正在将基础表数据进行耕地细化调查按类型汇总统计...");
                Application.DoEvents();
                clsOutputData.InitGdxhdcTable();

                UpdateStatus("正在将基础表数据进行可调整地类面积统计...");
                Application.DoEvents();
                clsOutputData.InitKtzTable();


                UpdateStatus("正在将提取批准未建设数据...");
                Application.DoEvents();
                clsStatsPzwjs pzwjs = new clsStatsPzwjs(this.currWorkspace);
                pzwjs.getPzwjsTmp();

                UpdateStatus("正在将汇总批准未建设基础表...");
                Application.DoEvents();
                pzwjs.InitPzwjsJCB();

                UpdateStatus("正在将批准未建设现状情况统计表进行汇总统计...");
                Application.DoEvents();
                pzwjs.initPzwjsXzBzTable();

                UpdateStatus("正在将批准未建设建设用地用途情况进行统计...");
                Application.DoEvents();
                pzwjs.InitPzwjsBZTable();

                UpdateStatus("正在将基础表数据进行按飞地汇总统计...");
                Application.DoEvents();// "正在将该范围的临时表数据进行按飞地汇总统计...";
                clsOutputData.InitFdTable();
                clsOutputData.InitFDQSTable();  //飞地权属
                clsOutputData.InitFd_CZCGKTable(); 

                UpdateStatus("正在将基础表数据进行按海岛面积汇总统计...");
                Application.DoEvents();
                clsOutputData.InitHDTable();

                clsStatsWRHD hdtj = new clsStatsWRHD(this.currWorkspace);
                hdtj.InitWjmHd();

                UpdateStatus("正在将进行部分地类细化汇总表...");
                Application.DoEvents();
                clsOutputData.InitBfxhdlTable();



                //UpdateStatus("正在提取永久基本农田图斑数据表...");
                //Application.DoEvents();
                //clsStatsYjjbnt yjjbnt = new clsStatsYjjbnt(this.currWs);
                //yjjbnt.getYjjbntTmp();
                UpdateStatus("正在将汇总永久基本农田数据并统计...");
                Application.DoEvents();
                //yjjbnt.InitYjjbntJCB();
                //yjjbnt.initYjjbntXzBzTable();
                clsStatsJbnt2 statJbnt = new clsStatsJbnt2(this.currWorkspace);
                statJbnt.getYJJBNTTmp();
                statJbnt.InitYjjbnt();

                UpdateStatus("正在将进行废弃与垃圾填埋细化标注汇总...");
                Application.DoEvents();
                clsOutputData.InitFQLJTMTable();

                UpdateStatus("正在将进行即可恢复与工程恢复种植属性汇总...");
                Application.DoEvents();
                clsOutputData.InitJKHFGCHFTable();

                UpdateStatus("基础数据完成初始化。.");
                            

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void ResultPacket5()
        {
            //2.输出表格
            string zldwdm = txtXZQDM.Text;


            if (dicQsdwdm.Count == 0)
                dicQsdwdm = clsOutputData.getZldwdmMc(currWorkspace);
            DataTable dt = null;// this.GetDataTable(zldwdm, this.radioGroup1.SelectedIndex);

            try
            {

                UpdateStatus("正在导出表1土地利用现状一级分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 0);
                clsOutputData.ExportToExcel1_OneXQTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表2土地利用现状二级分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 1);
                clsOutputData.ExportToExcel2_TwoXQTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);


                UpdateStatus("正在导出表3土地利用现状一级分类面积按权属性质汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 2);
                clsOutputData.ExportToExcel3_QSXZ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);


                UpdateStatus("正在导出表4 城镇村及工矿用地面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 3);
                clsOutputData.ExportToExcel4_CZCGKTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);//城镇村及工况用地

                UpdateStatus("正在导出表5 耕地坡度分级面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 4);
                clsOutputData.ExportToExcel5_GDPD(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表6 耕地种植类型面积统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 5);
                clsOutputData.ExportToExcel6_GDZZLX(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表7 林区范围内种植园地汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 6);
                clsOutputData.ExportToExcel7_LQYD(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);  //林区园地

                UpdateStatus("正在导出表8 灌丛草地汇总情况统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 7);
                clsOutputData.ExportToExcel8_GCXSCD(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表9 工业用地按类型汇总统计比表");
                dt = clsOutputData.GetDataTable(zldwdm, 8);
                clsOutputData.Exporttoexcel9_GYCCYD(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                // 可调整
                UpdateStatus("正在导出表10 可调整地类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 9);
                clsOutputData.ExportToExcel10_Ktz(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                //部分细化地类
                UpdateStatus("正在导出表11 部分细化地类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 10);
                clsOutputData.ExportToExcel11_BfxhdlHzb(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                //耕地细化调查情况统计表
                UpdateStatus("正在导出表12 耕地细化调查情况统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 11);
                clsOutputData.ExportToExcel12_GDXHTJB(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                //批准未建设
                UpdateStatus("正在导出表13 批准未建设建设用地用途情况统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 12);
                clsOutputData.ExportToExcel13_Pzwjstd(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表14 批准未建设建设用地现状情况统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 13);
                clsOutputData.ExportToExcel14_PzwjsXz(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表飞入地土地利用现状一级分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 14);
                clsOutputData.ExportToExcel15_OneFDTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表飞入地土地利用现状二级分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 15);
                clsOutputData.ExportToExcel16_TwoFDTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表飞入地土地利用现状一级分类按权属性质汇总表");
                //飞地权属
                dt = clsOutputData.GetDataTable(zldwdm, 16);
                clsOutputData.ExportToExcel17_FDQSXZ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表飞入地城镇村及工矿用地面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 17);
                clsOutputData.ExportToExcel18_FD_CZCTable(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表海岛土地利用现状一级分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 18);
                clsOutputData.ExportToExcel19_OneHDTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表海岛土地利用现状二级分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 19);
                clsOutputData.ExportToExclel20_HDTJ(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表15 永久基本农田现状情况统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 20);
                clsOutputData.ExportToExcel20_Yjjbnt(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表无居民海岛现状调查分类面积汇总表");
                dt = clsOutputData.GetDataTable(zldwdm, 21);
                clsOutputData.ExportToExcel21_WJMHD(dt, destExcelDir, CurrDW, txtXZQDM.Text, dicQsdwdm);

                UpdateStatus("正在导出表即可恢复与工程恢复种植属性汇总统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 22);
                clsOutputData.ExportToExclel22_FQLJTM(dt, destExcelDir, CurrDW, zldwdm, dicQsdwdm);

                UpdateStatus("正在导出表废弃与垃圾填埋细化标注汇总统计表");
                dt = clsOutputData.GetDataTable(zldwdm, 23);
                clsOutputData.ExportToExclel23_JKHFGCHF(dt, destExcelDir, CurrDW, zldwdm, dicQsdwdm);
            }
            catch (Exception ex)
            {  }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKzmjHD.Text) || string.IsNullOrWhiteSpace(txtKzmjLD.Text))
            {
                MessageBox.Show("请输入控制面积", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }
            if (string.IsNullOrWhiteSpace(txtExportPath.Text) || string.IsNullOrWhiteSpace(txtXZQDM.Text) || txtXZQDM.Text.Length != 6)
            {
                MessageBox.Show("请设置功能参数。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!(currWorkspace as IWorkspace2).NameExists[esriDatasetType.esriDTTable, "QSDWDMB"])
            {
                MessageBox.Show("没有找到权属单位代码表。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string destFolder = string.Format(txtExportPath.Text + "\\{0}{1}{2}({3})第三次国土调查成果", sheng, shi, xian, txtXZQDM.Text);
            if (Directory.Exists(destFolder))
            {
                MessageBox.Show("成果目录已经存在，请修改导出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Directory.CreateDirectory(destFolder);
            RCIS.Utility.File_DirManipulate.FolderCopy(Application.StartupPath + "\\SystemConf\\第三次国土调查成果", destFolder);
            destExcelDir = destFolder + "\\基础数据包\\汇总表格\\";
            if (File.Exists(Application.StartupPath + "\\template\\第三次全国国土调查有关情况统计表.xlsx"))
                File.Copy(Application.StartupPath + "\\template\\第三次全国国土调查有关情况统计表.xlsx", destExcelDir + "\\(" + txtXZQDM.Text + ")第三次全国国土调查有关情况统计表.xlsx");
            #endregion

            if (chkOnlyReport.CheckState == CheckState.Unchecked)
            {
                ResultPacket1();
                GC.Collect();
                Application.DoEvents();
                //2.输出VCT
                ResultPacket2(destFolder + "\\基础数据包\\标准格式数据\\" + "2001H2019" + txtXZQDM.Text + ".VCT");
                // ResultPacket3(destFolder);
                GC.Collect();
                Application.DoEvents();
                if (string.IsNullOrWhiteSpace(txtMetaData.Text))
                    File.Copy(Application.StartupPath + @"\template\metadata.xml", destFolder + "\\基础数据包\\标准格式数据\\" + "2001H2019" + txtXZQDM.Text + ".xml");
                else
                    File.Copy(txtMetaData.Text, destFolder + "\\基础数据包\\标准格式数据\\" + "2001H2019" + txtXZQDM.Text + ".xml");
            }
            ResultPacket4();
            GC.Collect();
            Application.DoEvents();
            ResultPacket5();
            
            MessageBox.Show("导出完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        private void UpdateStatus(string txt)
        {
            memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + txt;
            memoLog.SelectionStart = memoLog.Text.Length;
            memoLog.ScrollToCaret();
            Application.DoEvents();
        }

        private void OutStandardForm_Load(object sender, EventArgs e)
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
                string xzdm = "";
                try
                {
                    pXZQClass = (currWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
                }
                catch { }
                currDh = 38;
                if (pXZQClass != null)
                {
                    IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                    if (firstFea != null)
                    {
                        xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                        IPoint selectPoint = (firstFea.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        currDh = (int)(X / 1000000);////WK---带号
                        txtXZQDM.Text = xzdm.Length >= 6 ? xzdm.Substring(0, 6) : "";
                    }
                }
            }
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

    }
}
