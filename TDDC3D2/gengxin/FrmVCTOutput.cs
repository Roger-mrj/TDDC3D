using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using RCIS.DataInterface.VCTOut;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using System.Collections;

namespace TDDC3D.gengxin
{
    public partial class FrmVCTOutput : Form
    {
        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(int hProcess);

        private IWorkspace m_pWorkspace = null;

        public IWorkspace PWorkspace
        {
            get { return m_pWorkspace; }
            set { m_pWorkspace = value; }
        }

        private string xianDM = "";

        /// <summary>
        /// 县代码
        /// </summary>
        public string XianDM
        {
            get { return xianDM; }
            set { xianDM = value; }
        }

        public int DH;
        public LsGxClass gx = null;

        //记录 类名对应的中文名  要素名
        private Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
        private Dictionary<string, string> dicClassCNName = new Dictionary<string, string>();

        public FrmVCTOutput()
        {
            InitializeComponent();
        }

        private void UpdateStatus(string txt)
        {
            memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + txt;
            Application.DoEvents();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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

        private void beDestVCTfile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.FileName = "2001H2020" + this.txtXian.Text.Trim().ToString() + ".VCT";
            dlg.Filter = "VCT文件|*.VCT";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestVCTfile.Text = dlg.FileName;
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


        private void FrmVCTOutput_Load(object sender, EventArgs e)
        {
            if (DH > 0)
                txtDH.Text = DH.ToString();
            try
            {
                //获取所有要素代码
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM  where type in ('POINT','LINE','POLYGON')  ", "ysdm");
                foreach (DataRow dr in dt.Rows)
                {
                    dicClassCNName.Add(dr["CLASSNAME"].ToString(), dr["ALIASNAME"].ToString());
                    dicClassYsdm.Add(dr["CLASSNAME"].ToString(), dr["YSDM"].ToString());
                }
            }
            catch (Exception ex) { }



            //县代码
            IFeatureClass pXZQClass = null;
            try
            {
                pXZQClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQClass != null)
            {
                List<string> dms = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                txtXian.Properties.Items.Clear();
                foreach (string dm in dms)
                {
                    txtXian.Properties.Items.Add(dm);
                }
                if (txtXian.Properties.Items.Count > 0) txtXian.SelectedIndex = 0;
            }


            string temppath = System.Environment.GetEnvironmentVariable("TEMP");
            if (temppath.EndsWith("\\"))
                temppath += "tmp";
            else temppath += "\\tmp";
            this.beTmpDir.Text = temppath;
        }
        //LsGxClass gx = null;
        private void btnDataConvert_Click(object sender, EventArgs e)
        {
            gx = new LsGxClass();
            gx.xzdm = txtXian.Text;
            gx.info = memoLog;
            if (!string.IsNullOrWhiteSpace(this.beTmpDir.Text) && Directory.Exists(this.beTmpDir.Text))
            {
                gx.VCTDataPrepare(this.beTmpDir.Text);
                MessageBox.Show("数据准备完成，请继续后续工作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("文件夹不存在或未选择临时文件夹。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);



            //IFeatureDataset featureDataset = (this.m_pWorkspace as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            //if (featureDataset == null)
            //{
            //    IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            //    featureDataset = pEnumDs.Next() as IFeatureDataset;
            //}
            //if (featureDataset == null)
            //{
            //    UpdateStatus("找不到数据集，退出...");
            //    return;
            //}

            //string temppath = this.beTmpDir.Text;
            //if (!Directory.Exists(temppath))
            //{
            //    Directory.CreateDirectory(temppath);
            //}
            //string fileName = "vct.gdb";
            //string shpPath = temppath + "\\" + fileName;
            //if (!System.IO.Directory.Exists(shpPath))
            //{
            //    //不存在则创建
            //    //创建一个临时库

            //    try
            //    {
            //        IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            //        pWorkspaceFactory.Create(temppath, fileName, null, 0);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //        return;
            //    }
            //}
            //else
            //{
            //    RCIS.Utility.FileHelper.DelectDir(shpPath);
            //    System.IO.Directory.Delete(shpPath);
            //    IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            //    pWorkspaceFactory.Create(temppath, fileName, null, 0);
            //}

            //UpdateStatus("正在进行预处理...");
            //IWorkspace pTarWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(shpPath);
            //IEnumDataset srcDS = pTarWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
            //IDataset aDs = null;
            //while ((aDs = srcDS.Next()) != null)
            //{
            //    try
            //    {
            //        aDs.Delete();
            //    }
            //    catch { }
            //}


            //List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构            
            ////导出各要素
            //try
            //{
            //    foreach (TableStruct ts in lstTables)
            //    {
            //        IFeatureClass pFC = null;
            //        try
            //        {
            //            pFC = (this.m_pWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
            //            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(this.m_pWorkspace, pTarWorkspace, ts.className, ts.className, null);
            //        }
            //        catch { }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            ////2021年3月22日08:46:22   数据过滤   只是zldwdm或者qsdwdm变更的，不进入更新层和更新过程层

            //try
            //{
            //    IFeatureClass pGXGCClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            //    IFeatureClass pGXClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            //    IQueryFilter pQf = new QueryFilterClass();
            //    pQf.WhereClause = "BGXW='1' and xzqtzlx<>'2' and xzqtzlx<>'4'";
            //    IFeatureCursor pFeaCursor = pGXGCClass.Update(pQf, true);
            //    IFeature pFeature;
            //    while ((pFeature = pFeaCursor.NextFeature()) != null)
            //    {
            //        bool isDel = true;
            //        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
            //        {
            //            string filedName = pFeature.Fields.Field[i].Name.ToString().Trim().ToUpper();

            //            if (filedName.Contains("BGQ") && !filedName.Contains("BSM") && !filedName.Contains("TBBH") && !filedName.Contains("ZLDWDM") && !filedName.Contains("ZLDWMC") && !filedName.Contains("QSDWDM") && !filedName.Contains("QSDWMC"))
            //            {
            //                if (pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() != pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim())
            //                {
            //                    if (filedName == "BGQGDDB" && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() == "0") && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim() == "0"))
            //                        continue;
            //                    else
            //                    {
            //                        isDel = false;
            //                        break;
            //                    }

            //                }
            //            }
            //        }
            //        if (isDel)
            //        {
            //            pFeature.set_Value(pFeature.Fields.FindField("BSM"), "DEL");
            //            pFeaCursor.UpdateFeature(pFeature);
            //        }
            //        RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            //    }
            //    pFeaCursor.Flush();
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            //    IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
            //    IQueryFilter pQfilter = new QueryFilterClass();
            //    pQfilter.WhereClause = "bsm='DEL'";
            //    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pTarWorkspace, pTmpWs, "DLTBGXGC", "GXGC", pQfilter);
            //    pTarWorkspace.ExecuteSQL("delete from dltbgxgc where bsm='DEL'");
            //    pTarWorkspace.ExecuteSQL("update dltbgxgc set BGHTBBSM='',BGHDLBM='',BGHDLMC='',BGHQSXZ='',BGHQSDWDM='',BGHQSDWMC='',BGHZLDWDM='',BGHZLDWMC='',BGHKCDLBM='',BGHKCXS=0,BGHKCMJ=0,BGHTBDLMJ=0,BGHGDLX='',BGHGDPDJB='',BGHXZDWKD=0,BGHTBXHDM='',BGHTBXHMC='',BGHZZSXDM='',BGHZZSXMC='',BGHGDDB=0,BGHFRDBS='',BGHCZCSXM='',BGHMSSM='',BGHHDMC='',BGHTBBH='' where xzqtzlx='4' or xzqtzlx='2'");
            //    bool b = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(pTarWorkspace.PathName + "\\DLTBGX", pTmpWs.PathName + "\\GXGC", pTmpWs.PathName + "\\SpatialJoinGx", "CONTAINS");
            //    if (!b)
            //    {
            //        UpdateStatus("叠加分析错误");
            //        return;
            //    }
            //    pTarWorkspace.ExecuteSQL("delete from dltbgx");
            //    pTmpWs.ExecuteSQL("delete from SpatialJoinGx where Join_Count=1");

            //    IFeatureClass pSDClass = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("SpatialJoinGx");
            //    for (int i = pSDClass.Fields.FieldCount - 1; i >= 0; i--)
            //    {
            //        IField pField = pSDClass.Fields.get_Field(i);
            //        if (pField.Name.Contains("_1"))
            //            (pSDClass as ITable).DeleteField(pField);
            //    }

            //    b = RCIS.GISCommon.GpToolHelper.Append(pTmpWs.PathName + "\\SpatialJoinGx", pTarWorkspace.PathName + "\\DLTBGX");
            //    if (!b)
            //    {
            //        UpdateStatus("叠加分析错误");
            //        return;
            //    }
            //    //2021年10月22日15:01:28过滤图斑、村级调查区、行政区更新层调出数据
            //    pTarWorkspace.ExecuteSQL("delete from dltbgx where zldwdm not like '" + txtXian.Text + "%'");
            //    pTarWorkspace.ExecuteSQL("delete from cjdcqgx where zldwdm not like '" + txtXian.Text + "%'");
            //    pTarWorkspace.ExecuteSQL("delete from xzqgx where xzqdm not like '" + txtXian.Text + "%'");

            //    RCIS.GISCommon.GpToolHelper.RepairGeometry(pTarWorkspace.PathName + "\\DLTBGX");
            //    RCIS.GISCommon.GpToolHelper.RepairGeometry(pTarWorkspace.PathName + "\\DLTBGXGC");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //bool bRet = true;
            ////执行面转线
            //this.Cursor = Cursors.WaitCursor;
            //IWorkspace2 wsname2 = pTarWorkspace as IWorkspace2;
            //foreach (TableStruct ts in lstTables)
            //{
            //    if (ts.type.ToUpper() == "POLYGON")
            //    {
            //        string shpfileName = shpPath + "\\" + ts.className.ToUpper();
            //        string lineShpFile = shpPath + "\\" + ts.className.ToUpper() + "line";
            //        if (wsname2.get_NameExists(esriDatasetType.esriDTFeatureClass, ts.className))
            //        {
            //            IFeatureClass pFClass = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
            //            if (pFClass.FeatureCount(null) > 0)
            //            {
            //                bRet &= PolygonToline(shpfileName, lineShpFile);
            //            }
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFClass);
            //        }
            //    }
            //}

            //this.Cursor = Cursors.Default;
            //if (bRet == false)
            //{
            //    UpdateStatus("关联图层失败，退出...");

            //    return;
            //}

            //if (bRet == false)
            //{
            //    UpdateStatus("关联图层失败，退出...");

            //    return;
            //}
            //else
            //{
            //    UpdateStatus("数据准备完成，请继续！");
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pTarWorkspace);

            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    MessageBox.Show("数据准备完成，请继续后续工作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnDataExport_Click(object sender, EventArgs e)
        {
            if (gx != null && !string.IsNullOrWhiteSpace(this.beDestVCTfile.Text))
            {
                int dh = int.Parse(txtDH.Text.ToString());
                if (dh == 0)
                {
                    IQueryFilter pQf = new QueryFilterClass();
                    pQf.WhereClause = "XZQDM LIKE '" + txtXian.Text + "%'";
                    IFeatureClass pXZQ = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
                    IFeature pFea = RCIS.GISCommon.GetFeaturesHelper.GetFirstFeature(pXZQ, pQf);
                    IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    dh = (int)(X / 1000000);////WK---带号
                }
                gx.xzdm = txtXian.Text;
                gx.VCTDataOutput(this.beDestVCTfile.Text, this.beTmpDir.Text, dh);
                MessageBox.Show("VCT导出完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("请先进行数据准备并选择数据输出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);


            //#region 错误控制
            //if (this.beDestVCTfile.Text.Trim() == "")
            //    return;
            //string destFile = this.beDestVCTfile.Text.Trim();
            //if (!destFile.ToUpper().EndsWith(".VCT"))
            //{
            //    destFile += ".VCT";
            //}
            //destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile) + "GXGC" + System.IO.Path.GetExtension(destFile);
            //if (System.IO.File.Exists(destFile))
            //{
            //    System.IO.File.Delete(destFile);
            //}
            ////预处理

            //if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
            //}

            //RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");


            //IFeatureWorkspace pFeaWs = this.m_pWorkspace as IFeatureWorkspace;
            //IWorkspace2 pWS2 = m_pWorkspace as IWorkspace2;
            ////找到数据集
            //IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            //if (featureDataset == null)
            //{
            //    IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            //    featureDataset = pEnumDs.Next() as IFeatureDataset;
            //}
            //if (featureDataset == null)
            //{
            //    UpdateStatus("找不到数据集，退出...");
            //    return;
            //}


            //List<TableStruct> lstTables = this.GetLstTables(featureDataset, "GX");  //获取所有表结构
            //if (lstTables.Count == 0)
            //{
            //    UpdateStatus("当前数据库中没有需要导出的数据，退出...");
            //    return;
            //}

            //string temppath = this.beTmpDir.Text;
            //VCTOut12 outvct = new VCTOut12(temppath);
            //int iDh = 0;
            //int.TryParse(this.txtDH.Text.Trim(), out iDh);
            //outvct.dh = iDh;
            //outvct.gdbWorkspace = this.m_pWorkspace as IFeatureWorkspace;
            //outvct.gdbDataset = featureDataset;
            //outvct.allTableStruct = lstTables;
            //outvct.DoByAXzq = true;
            //outvct.includezj = false;
            //#endregion
            ////导出shp
            //try
            //{
            //    UpdateStatus("开始导出文件头...");
            //    outvct.ExportFileHead3();
            //    UpdateStatus("导出文件头结束...");
            //    outvct.ExportPoint3();

            //    UpdateStatus("导出点文件结束...");
            //    outvct.ExportLine3();

            //    UpdateStatus("导出线文件结束...");
            //    outvct.ExportFill3();
            //    UpdateStatus("导出面文件结束...");
            //    outvct.ExportAnotation3();
            //    UpdateStatus("导出注记结束...");
            //    outvct.ExportAttribute3();
            //    UpdateStatus("导出属性结束...");

            //    string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
            //    System.Array.Sort(allFiles);
            //    ConcatenateFiles(destFile, allFiles);

            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle.ToInt32());
            //    UpdateStatus("合并完成，导出完毕！");


            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus(ex.ToString());
            //}

            //#region 错误控制
            //if (this.beDestVCTfile.Text.Trim() == "")
            //    return;
            //destFile = this.beDestVCTfile.Text.Trim();
            //if (!destFile.ToUpper().EndsWith(".VCT"))
            //{
            //    destFile += ".VCT";
            //}
            //destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile) + "GX" + System.IO.Path.GetExtension(destFile);
            //if (System.IO.File.Exists(destFile))
            //{
            //    System.IO.File.Delete(destFile);
            //}
            ////预处理

            //if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
            //}

            //RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");


            //pFeaWs = this.m_pWorkspace as IFeatureWorkspace;
            //pWS2 = m_pWorkspace as IWorkspace2;
            ////找到数据集
            //featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_GX_NAME);
            //if (featureDataset == null)
            //{
            //    IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            //    featureDataset = pEnumDs.Next() as IFeatureDataset;
            //}
            //if (featureDataset == null)
            //{
            //    UpdateStatus("找不到数据集，退出...");
            //    return;
            //}


            //lstTables = this.GetLstTables(featureDataset, "GXGC");  //获取所有表结构
            //if (lstTables.Count == 0)
            //{
            //    UpdateStatus("当前数据库中没有需要导出的数据，退出...");
            //    return;
            //}



            //temppath = this.beTmpDir.Text;
            //outvct = new VCTOut12(temppath);
            //iDh = 0;
            //int.TryParse(this.txtDH.Text.Trim(), out iDh);
            //outvct.dh = iDh;
            //outvct.gdbWorkspace = this.m_pWorkspace as IFeatureWorkspace;
            //outvct.gdbDataset = featureDataset;
            //outvct.allTableStruct = lstTables;
            //outvct.DoByAXzq = true;
            //outvct.includezj = false;
            //#endregion
            ////导出shp
            //try
            //{
            //    UpdateStatus("开始导出文件头...");
            //    outvct.ExportFileHead3();
            //    UpdateStatus("导出文件头结束...");
            //    outvct.ExportPoint3();

            //    UpdateStatus("导出点文件结束...");
            //    outvct.ExportLine3();

            //    UpdateStatus("导出线文件结束...");
            //    outvct.ExportFill3();
            //    UpdateStatus("导出面文件结束...");
            //    outvct.ExportAnotation3();
            //    UpdateStatus("导出注记结束...");
            //    outvct.ExportAttribute3();
            //    UpdateStatus("导出属性结束...");

            //    string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
            //    System.Array.Sort(allFiles);
            //    ConcatenateFiles(destFile, allFiles);

            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    EmptyWorkingSet(System.Diagnostics.Process.GetCurrentProcess().Handle.ToInt32());
            //    UpdateStatus("合并完成，导出完毕！");

            //    MessageBox.Show("导出结束！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus(ex.ToString());
            //}
        }

        private Dictionary<string, string> getYsdmClassName()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                //获取所有要素代码
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM where type in ('POINT','LINE','POLYGON') ", "ysdm");
                foreach (DataRow dr in dt.Rows)
                {
                    dic.Add(dr["YSDM"].ToString(), dr["CLASSNAME"].ToString());

                }
            }
            catch { }
            return dic;
        }

        private void btnExportIndex_Click(object sender, EventArgs e)
        {
            if (this.beDestVCTfile.Text.Trim() == "") return;
            string destFile = beDestVCTfile.Text;
            destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile) + "GXGC" + System.IO.Path.GetExtension(destFile);
            if (!System.IO.File.Exists(destFile))
            {
                MessageBox.Show("该VCT文件不存在！");
                return;
            }
            try
            {
                UpdateStatus("开始生成索引...");
                VCTIdxCreator create = new VCTIdxCreator();
                create.VctFile = destFile;
                create.AllYsdmClassName = this.getYsdmClassName();
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
                MessageBox.Show("失败！");
                UpdateStatus(ex.ToString());
            }
            destFile = beDestVCTfile.Text;
            destFile = System.IO.Path.GetDirectoryName(destFile) + "\\" + System.IO.Path.GetFileNameWithoutExtension(destFile) + "GX" + System.IO.Path.GetExtension(destFile);
            if (!System.IO.File.Exists(destFile))
            {
                MessageBox.Show("该VCT文件不存在！");
                return;
            }
            try
            {
                UpdateStatus("开始生成索引...");
                VCTIdxCreator create = new VCTIdxCreator();
                create.VctFile = destFile;
                create.AllYsdmClassName = this.getYsdmClassName();
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
                MessageBox.Show("生成索引文件完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("失败！");
                UpdateStatus(ex.ToString());
            }
        }

        private void beTmpDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            beTmpDir.Text = dlg.SelectedPath;
        }
    }
}
