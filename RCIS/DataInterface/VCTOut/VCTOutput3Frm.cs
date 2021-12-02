using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
namespace RCIS.DataInterface.VCTOut
{
    public partial class VCTOutput3Frm : Form
    {

        [DllImport("psapi.dll")]
        private static extern int EmptyWorkingSet(int hProcess);


        public VCTOutput3Frm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

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

        //记录 类名对应的中文名  要素名
        private Dictionary<string, string> dicClassYsdm = new Dictionary<string, string>();
        private Dictionary<string, string> dicClassCNName = new Dictionary<string, string>();
        //获取所有表结构
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

        private void beDestVCTfile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.FileName = "2001H2019"+this.xianDM+".VCT";
            dlg.Filter = "VCT文件|*.VCT";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestVCTfile.Text = dlg.FileName;
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

        private void UpdateStatus(string txt)
        {
            memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + txt;
            Application.DoEvents();
        }


        private void VCTOutput3Frm_Load(object sender, EventArgs e)
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
            catch(Exception ex) { }


            string temppath = System.Environment.GetEnvironmentVariable("TEMP");
            if (temppath.EndsWith("\\"))
                temppath += "tmp";
            else temppath += "\\tmp";
            this.beTmpDir.Text = temppath;

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
        private List<string> getAllTopPolygon()
        {
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select CLASSNAME from sys_ysdm where type  ='POLYGON' and classname<>'DLTB' ", "ysdm");
            //DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select CLASSNAME from sys_ysdm where type  ='POLYGON'  ", "ysdm");

            List<String> lst = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                
                lst.Add("TP_"+dr["CLASSNAME"].ToString().Trim().ToUpper());
            }

            return lst;
        }

        //private List<string> getAllTopPolygonOld()
        //{
        //    //DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select CLASSNAME from sys_ysdm where type  ='POLYGON' and classname<>'DLTB' ", "ysdm");
        //    DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select CLASSNAME from sys_ysdm where type  ='POLYGON'  ", "ysdm");

        //    List<String> lst = new List<string>();
        //    foreach (DataRow dr in dt.Rows)
        //    {

        //        lst.Add("TP_" + dr["CLASSNAME"].ToString().Trim().ToUpper());
        //    }

        //    return lst;
        //}

        private void btnOut_Click(object sender, EventArgs e)
        {
            //if (this.beDestVCTfile.Text.Trim() == "")
            //    return;
            //string destFile = this.beDestVCTfile.Text.Trim();
            //if (!destFile.ToUpper().EndsWith(".VCT"))
            //{
            //    destFile += ".VCT";
            //}
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
            //IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            //if (featureDataset == null)
            //{
            //    IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            //    featureDataset = pEnumDs.Next() as IFeatureDataset;
            //}
            //if (featureDataset == null)
            //{
            //    UpdateStatus("找不到数据集，无法创建拓扑，退出...");
            //    return;
            //}
            //VCTOut5 outvct = new VCTOut5();
            
            //int iDh=0;
            //int.TryParse(this.txtDH.Text.Trim(),out iDh);
            
            //outvct.dh = iDh;           
            
            //List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构
            //if (lstTables.Count == 0)
            //{
            //    UpdateStatus("当前数据库中没有需要导出的数据，退出...");
            //    return;
            //}
            //////填充Spatial Cache,与identify冲突
            ////ISpatialCacheManager spatialCacheManager = (ISpatialCacheManager)(this.m_pWorkspace);
            ////IEnvelope cacheExtent = (featureDataset as IGeoDataset).Extent;

            ////cacheExtent.Expand(1, 1, true);
            ////检测是否存在缓存
            ////if (!spatialCacheManager.CacheIsFull)
            ////{
            ////    //不过不存在，则创建缓存
            ////    spatialCacheManager.FillCache(cacheExtent);
            ////}

            //#region  //首先创建拓扑
            //memoLog.Text = "";
            //UpdateStatus("正在创建拓扑...");
            //ISchemaLock schemaLock = (ISchemaLock)featureDataset;
            //ITopology topology = null;
            ////获取所有拓扑名
            //List<string> allTopNames = getAllTopPolygon();

            
            //try
            //{
            //    schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
            //    ITopologyContainer topologyContainer = (ITopologyContainer)featureDataset;

            //    //删除所有拓扑
            //    for (int i = topologyContainer.TopologyCount - 1; i >= 0; i--)
            //    {
            //        topology = topologyContainer.get_Topology(i);
            //        IDataset ds = topology as IDataset;
            //        ds.Delete();
            //    }
            //    #region  //创建新的拓扑
            //    foreach (string aTopName in allTopNames)
            //    {
                    
            //        switch (aTopName)
            //        {
            //            case "TP_XZQ":
            //                try
            //                {
            //                    IFeatureClass xzqClass = pFeaWs.OpenFeatureClass("XZQ");
            //                    IFeatureClass xzqJxClass = pFeaWs.OpenFeatureClass("XZQJX");
            //                    topology = topologyContainer.CreateTopology(aTopName, topologyContainer.DefaultClusterTolerance, -1, "");
            //                    topology.AddClass(xzqClass, 5, 1, 1, false);
            //                    topology.AddClass(xzqJxClass, 5, 1, 1, false);
            //                }
            //                catch { }
            //                break;

            //            //case "TP_ZD":
            //            case "TP_CJDCQ":
            //                try
            //                {
            //                    IFeatureClass ZDClass = pFeaWs.OpenFeatureClass("CJDCQ");
            //                    IFeatureClass dltbClass = pFeaWs.OpenFeatureClass("DLTB");                                
            //                    topology = topologyContainer.CreateTopology(aTopName, topologyContainer.DefaultClusterTolerance, -1, "");
            //                    topology.AddClass(ZDClass, 5, 1, 1, false);                                
            //                    topology.AddClass(dltbClass, 5, 1, 1, false);
            //                }
            //                catch { }
            //                break;

            //            default:
            //                //其他独立面
            //                int ipos = aTopName.IndexOf("_");
            //                string ClassName = aTopName.Substring(ipos + 1);
            //                try
            //                {

            //                    IFeatureClass pdtClass = pFeaWs.OpenFeatureClass(ClassName);
            //                    topology = topologyContainer.CreateTopology(aTopName, topologyContainer.DefaultClusterTolerance, -1, "");
            //                    topology.AddClass(pdtClass, 5, 1, 1, false);
            //                }
            //                catch { }
            //                break;

            //        }
            //    }
            //    #endregion 


            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus("创建拓扑失败。退出"+ex.ToString());
            //    return;
            //}
            //finally
            //{
            //    schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            //}

            //#endregion
            //try
            //{
            //    UpdateStatus("开始导出文件头...");
            //    outvct.mWorkspace = this.m_pWorkspace as IFeatureWorkspace;
            //    outvct.mDataset = featureDataset;
            //    outvct.allTableStruct = lstTables;

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
            //    UpdateStatus("合并完成，导出完毕！");
            //    MessageBox.Show("导出结束！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //}
            //catch (Exception ex)
            //{
            //    UpdateStatus(ex.ToString());
            //}
            //finally
            //{
            //    spatialCacheManager.EmptyCache();
            //}
        }

        #region 废弃
        //private void simpleButton3_Click(object sender, EventArgs e)
        //{
            
        //    if (this.beDestVCTfile.Text.Trim() == "")
        //        return;
        //    string destFile = this.beDestVCTfile.Text.Trim();
        //    if (!destFile.ToUpper().EndsWith(".VCT"))
        //    {
        //        destFile += ".VCT";
        //    }
        //    if (System.IO.File.Exists(destFile))
        //    {
        //        System.IO.File.Delete(destFile);
        //    }
        //    //预处理

        //    if (!Directory.Exists(Application.StartupPath + @"\VCTEX"))
        //    {
        //        Directory.CreateDirectory(Application.StartupPath + @"\VCTEX");
        //    }
        //    RCIS.Utility.FileHelper.DelectDir(Application.StartupPath + @"\VCTEX");
        //    IFeatureWorkspace pFeaWs = this.m_pWorkspace as IFeatureWorkspace;
        //    IWorkspace2 pWS2 = m_pWorkspace as IWorkspace2;
        //    //找到数据集
        //    IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
        //    if (featureDataset == null)
        //    {
        //        IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
        //        featureDataset = pEnumDs.Next() as IFeatureDataset;
        //    }
        //    if (featureDataset == null)
        //    {
        //        UpdateStatus("找不到数据集，无法创建拓扑，退出...");
        //        return;
        //    }
        //    VCTOut3 outvct = new VCTOut3();
        //    outvct.kzsxb = this.chkKzsxb.Checked;
        //    int iDh=0;
        //    int.TryParse(this.txtDH.Text.Trim(),out iDh);            
        //    outvct.dh = iDh;
        //    outvct.dlmYsdm = true;


        //    List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构
        //    if (lstTables.Count == 0)
        //    {
        //        UpdateStatus("当前数据库中没有需要导出的数据，退出...");
        //        return;
        //    }
        //    #region  //首先创建拓扑
        //    memoLog.Text = "";
        //    UpdateStatus("正在创建拓扑...");
        //    ISchemaLock schemaLock = (ISchemaLock)featureDataset;
        //    ITopology topology = null;
        //    //获取所有拓扑名
        //    List<string> allTopNames = getAllTopPolygonOld();

            
        //    try
        //    {
        //        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
        //        ITopologyContainer topologyContainer = (ITopologyContainer)featureDataset;

        //        //删除所有拓扑
        //        for (int i = topologyContainer.TopologyCount - 1; i >= 0; i--)
        //        {
        //            topology = topologyContainer.get_Topology(i);
        //            IDataset ds = topology as IDataset;
        //            ds.Delete();
        //        }
        //        #region  //创建新的拓扑
        //        foreach (string aTopName in allTopNames)
        //        {
                    
        //            switch (aTopName)
        //            {
        //                case "TP_XZQ":
        //                    try
        //                    {
        //                        IFeatureClass xzqClass = pFeaWs.OpenFeatureClass("XZQ");
        //                        IFeatureClass xzqJxClass = pFeaWs.OpenFeatureClass("XZQJX");
        //                        topology = topologyContainer.CreateTopology(aTopName, topologyContainer.DefaultClusterTolerance, -1, "");
        //                        topology.AddClass(xzqClass, 5, 1, 1, false);
        //                        topology.AddClass(xzqJxClass, 5, 1, 1, false);
        //                    }
        //                    catch { }
        //                    break;
                        
        //                case "TP_ZD":
        //                    try
        //                    {
        //                        IFeatureClass ZDClass = pFeaWs.OpenFeatureClass("ZD");
        //                        IFeatureClass jzxClass = pFeaWs.OpenFeatureClass("JZX");
        //                        topology = topologyContainer.CreateTopology(aTopName, topologyContainer.DefaultClusterTolerance, -1, "");
        //                        topology.AddClass(ZDClass, 5, 1, 1, false);
        //                        topology.AddClass(jzxClass, 5, 1, 1, false);
        //                    }
        //                    catch { }
        //                    break;

        //                default:
        //                    //其他独立面
        //                    int ipos = aTopName.IndexOf("_");
        //                    string ClassName = aTopName.Substring(ipos + 1);
        //                    try
        //                    {

        //                        IFeatureClass pdtClass = pFeaWs.OpenFeatureClass(ClassName);
        //                        topology = topologyContainer.CreateTopology(aTopName, topologyContainer.DefaultClusterTolerance, -1, "");
        //                        topology.AddClass(pdtClass, 5, 1, 1, false);
        //                    }
        //                    catch { }
        //                    break;

        //            }
        //        }
        //        #endregion 


        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus("创建拓扑失败，是否缺乏必备图层和数据集。退出。");
        //        return;
        //    }
        //    finally
        //    {
        //        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
        //    }

        //    #endregion
        //    try
        //    {
        //        UpdateStatus("开始导出文件头...");
        //        outvct.mWorkspace = this.m_pWorkspace as IFeatureWorkspace;
        //        outvct.mDataset = featureDataset;
        //        outvct.allTableStruct = lstTables;

        //        outvct.ExportFileHead3();
        //        UpdateStatus("导出文件头结束...");
        //        outvct.ExportPoint3();
        //        UpdateStatus("导出点文件结束...");
        //        outvct.ExportLine3();
        //        UpdateStatus("导出线文件结束...");

        //        outvct.ExportFill3();
        //        UpdateStatus("导出面文件结束...");

        //        outvct.ExportAnotation3();

        //        outvct.ExportAttribute3();
        //        UpdateStatus("导出属性结束...");


        //        string[] allFiles = System.IO.Directory.GetFiles(Application.StartupPath + "\\VCTEX", "*.VCT");
        //        System.Array.Sort(allFiles);
        //        ConcatenateFiles(destFile, allFiles);

        //        UpdateStatus("合并完成，导出完毕！");
        //        MessageBox.Show("导出结束！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateStatus(ex.ToString());
        //    }
        //}
        #endregion 


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.beDestVCTfile.Text.Trim() == "") return;
            if (!System.IO.File.Exists(this.beDestVCTfile.Text.Trim()))
            {
                MessageBox.Show("该VCT文件不存在！");
                return;
            }
            try
            {
                UpdateStatus("开始生成索引...");
                VCTIdxCreator create = new VCTIdxCreator();
                create.VctFile = this.beDestVCTfile.Text.Trim();
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




       /// <summary>
       /// 面转线
       /// </summary>
       /// <param name="dltbFile"></param>
       /// <param name="lineFile"></param>
       /// <returns></returns>
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

       

        private void beTmpDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beTmpDir.Text = dlg.SelectedPath;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            IFeatureDataset featureDataset = (this.m_pWorkspace as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                featureDataset = pEnumDs.Next() as IFeatureDataset;
            }
            if (featureDataset == null)
            {
                UpdateStatus("找不到数据集，退出...");
                return;
            }

            string temppath = this.beTmpDir.Text;
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
                catch(Exception ex)
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
                        pFC=(this.m_pWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
                        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(this.m_pWorkspace, pTarWorkspace, ts.className, ts.className, null);
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


            
            //List<TableStruct> lstTables = this.GetLstTables(featureDataset);  //获取所有表结构
            //bool bRet = true;
            //UpdateStatus("正在预处理...");



            //foreach (TableStruct ts in lstTables)
            //{
            //    IFeatureClass pFC = (this.m_pWorkspace as IFeatureWorkspace).OpenFeatureClass(ts.className);
            //    string shpfileName = temppath + "\\" + ts.className.ToUpper() + ".shp";
            //    bRet &= VCTOutPublic.ExportShp(pFC, null, shpfileName);
            //}
            //if (bRet == false)
            //{
            //    UpdateStatus("生成失败，退出...");
            //    return;
            //}
            ////所有面，除了XZQ，cjdcq，生成线图层
            //foreach (TableStruct ts in lstTables)
            //{
            //    if (ts.type.ToUpper() == "POLYGON")
            //    {
                    
            //        //2019-3 宗地  改为 cjdcq 
            //        //if (ts.className.ToUpper() != "XZQ" && ts.className.ToUpper() != "CJDCQ")
            //        //if ( ts.className.ToUpper() != "CJDCQ")  // 2019-5月 因质检软件倒不进去，所以去掉xzq判定条件 ，新质检软件不用公边了
            //        //{
            //            string shpfileName = temppath + "\\" + ts.className.ToUpper() + ".shp";
            //            string lineShpFile = temppath + "\\" + ts.className.ToUpper() + "line.shp";
            //            if (System.IO.File.Exists(shpfileName))
            //            {
            //                bRet &= PolygonToline(shpfileName, lineShpFile);
            //            }

            //       // }
            //    }
            //}

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
                MessageBox.Show("数据准备完成，请继续后续工作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            #region 错误控制
            if (this.beDestVCTfile.Text.Trim() == "")
                return;
            string destFile = this.beDestVCTfile.Text.Trim();
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


            IFeatureWorkspace pFeaWs = this.m_pWorkspace as IFeatureWorkspace;
            IWorkspace2 pWS2 = m_pWorkspace as IWorkspace2;
            //找到数据集
            IFeatureDataset featureDataset = pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            if (featureDataset == null)
            {
                IEnumDataset pEnumDs = m_pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
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



            string temppath = this.beTmpDir.Text;           
           // VCTOut11 outvct = new VCTOut11(temppath);   
            VCTOut12 outvct = new VCTOut12(temppath);
            int iDh = 0;
            int.TryParse(this.txtDH.Text.Trim(), out iDh);
            outvct.dh = iDh;
            outvct.gdbWorkspace = this.m_pWorkspace as IFeatureWorkspace;
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

                MessageBox.Show("导出结束！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);                

            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }
            
           

        }
         
        
    }
}
