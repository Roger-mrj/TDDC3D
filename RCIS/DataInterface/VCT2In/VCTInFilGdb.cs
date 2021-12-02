using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;


namespace RCIS.DataInterface.VCT2
{
    public partial class VCTInFilGdb : Form
    {
        public VCTInFilGdb()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        

        private void UpdateStatus(string txt)
        {
            memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":"+txt;
            Application.DoEvents();
        }

        //int threadCount = 0;


        ////跨线程调用
        //private delegate void deleFinishThread();//一个线程结束
        //private delegate void deleSetProcess(string txt);// //定义委托
        //private void finishThread()  //提示完成
        //{
        //    if (threadCount > 0)
        //        threadCount--;
        //    if (threadCount == 0)
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            Delegate d = new deleFinishThread(finishThread);
        //            this.Invoke(d, null);
        //        }
        //        else
        //        {
        //            this.Enabled = true;
        //            MessageBox.Show("全部导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //        }

        //    }
        //}
        //private void UpdateUIProcess(string txt)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        deleSetProcess d = new deleSetProcess(UpdateUIProcess);
        //        this.BeginInvoke(d, new object[] { txt });
        //    }
        //    else
        //    {
        //        this.memoLog.Text += "\r\n" + DateTime.Now.ToString()+":"+ txt;

        //    }
        //}

        

        private void btnOk_Click(object sender, EventArgs e)
        {
            
            

            
            if (this.beVCTFile.Text.Trim() == "") return;
            if (this.beDestDir.Text.Trim() == "") return;

            
            string destFile = this.beDestDir.Text + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.beVCTFile.Text) + ".gdb";
            if (System.IO.Directory.Exists(destFile))
            {
                MessageBox.Show("该目录已经存在！");
                return;
            }
            memoLog.Text = "";
            string sourceDir = RCIS.Global.AppParameters.TemplatePath + @"\VCTInput.gdb";
            RCIS.Utility.File_DirManipulate.FolderCopy(sourceDir, destFile);
            //创建数据集
            IWorkspace gpWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(destFile);
            if (gpWs == null)
            {
                memoLog.Text += "\r\n" + DateTime.Now.ToString() + "创建文件数据库失败，是否找不到模板文件...";
                MessageBox.Show("创建文件数据库失败！");
                return;
            }

            string sDSName = RCIS.Global.AppParameters.DATASET_DEFAULT_NAME; //数据集名称
            string vctFile = this.beVCTFile.Text.Trim();
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            VCTReader reader = new VCTReader();
            reader.IsInExtTable = this.chkExtATTR.Checked;

            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;
            if (Directory.Exists(sTmpFilePath))
            {
                RCIS.Utility.FileHelper.DelectDir(sTmpFilePath);
            }
            
            IFeatureDataset gpFDS=null;
            ISpatialReference spatialReference=null;
            try
            {
                spatialReference = reader.getSpatialRef(vctFile);

                IFeatureWorkspace pFcWps = gpWs as IFeatureWorkspace;
                gpFDS = pFcWps.CreateFeatureDataset(sDSName, spatialReference);

            }
            catch (Exception ex)
            {
                MessageBox.Show("创建数据集失败！");
                memoLog.Text +="\r\n"+ DateTime.Now.ToString() + ":" + ex.Message;
                return;
            }
            if (gpFDS == null)
                return;
            Hashtable ghasTableStructure=  null;
            Hashtable ghasFeatuerCode=null;
            this.Cursor = Cursors.WaitCursor;

            //Hashtable htLinePos = new Hashtable();


            try
            {
                

                #region 拆分文件
                using (StreamReader sr = new StreamReader(vctFile, gb2312))
                {

                      //以'属性表名'为Key,存放TableStructClass信息
                    reader.GetFeatureCodes(sr);
                    reader.GetAllTableStruct(sr);
                    ghasTableStructure = reader.ghasTableStructure;
                    ghasFeatuerCode = reader.ghasFeatuerCode;


                    UpdateStatus("读取VCT结构信息完毕！");
                    Dictionary<string, ArrayList> dicPt = reader.getNewPointDataFile(sr);
                    UpdateStatus("拆分点文件结束！");
                    
                   // Dictionary<string, ArrayList> dicLine = reader.getNewLineDataFile2(sr, ref  htLinePos);

                    Dictionary<string, ArrayList> dicLine = reader.getNewLineDataFile(sr);
                    UpdateStatus("拆分线文件结束！");
                    Dictionary<string, ArrayList> dicPolygon = reader.getNewPolygonDataFile(sr);
                    UpdateStatus("拆分面文件结束！");
                    #region 合并为一个

                    foreach (KeyValuePair<string, ArrayList> aItem in dicLine)
                    {
                        if (!dicPt.ContainsKey(aItem.Key))
                        {
                            dicPt.Add(aItem.Key, aItem.Value);
                        }
                    }
                    dicLine.Clear();
                    foreach (KeyValuePair<string, ArrayList> aItem in dicPolygon)
                    {
                        if (!dicPt.ContainsKey(aItem.Key))
                        {
                            dicPt.Add(aItem.Key, aItem.Value);
                        }
                    }
                    dicPolygon.Clear();
                    #endregion

                    //整理属性文件
                    reader.GetNewAttrFile(sr, dicPt);
                    UpdateStatus("拆分属性文件结束！");
                    sr.Close();

                    dicPt.Clear();
                }
                
               
                #endregion

            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }

            //创建shp
            foreach (DictionaryEntry de in ghasFeatuerCode)
            {
                FeatureCodeBeginEnd aFCCode = (FeatureCodeBeginEnd)de.Value;
                string aTabName = aFCCode.sSXBM;
                string aJhlx = aFCCode.sJHLX;
                TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[aTabName];

                if (aJhlx.ToUpper() == "POINT")
                {
                    VCTHelper.CreateFeatureClass(gpFDS, esriGeometryType.esriGeometryPoint, aTabName, curItem);
                }
                else if (aJhlx.ToUpper() == "LINE")
                {

                    VCTHelper.CreateFeatureClass(gpFDS, esriGeometryType.esriGeometryPolyline, aTabName, curItem);

                }
                else if (aJhlx.ToUpper() == "POLYGON")
               // if (aJhlx.ToUpper() == "POLYGON")
                {
                    VCTHelper.CreateFeatureClass(gpFDS, esriGeometryType.esriGeometryPolygon, aTabName, curItem);
                }

                
                

            }
            

            //this.Enabled = false;
            //threadCount=ghasFeatuerCode.Count; //线程数量
            UpdateStatus( "结构创建完毕！");

            foreach (DictionaryEntry de in ghasFeatuerCode)
            {

                FeatureCodeBeginEnd aFCCode = (FeatureCodeBeginEnd)de.Value;
                string aTabName = aFCCode.sSXBM;
                string aJhlx = aFCCode.sJHLX;
                if (aJhlx.ToUpper() == "POINT")
                {
                    VCTInAPointClass inPoint = new VCTInAPointClass(aTabName, ghasTableStructure, gpFDS);
                    inPoint.WriteAPtClass();
                    UpdateStatus(aTabName + "导入完毕！");

                    //inPoint.aFileFinish += new VCTInAPointClass.InvokeAFile(UpdateUIProcess);
                    //inPoint.allFileFinish += new VCTInAPointClass.InvokeFinish(finishThread);


                    //ThreadStart aStart = new ThreadStart(inPoint.WriteAPtClass);
                    //Thread thread = new Thread(aStart);
                    //thread.Start();
                }
                else if (aJhlx.ToUpper() == "LINE")
                {
                    VCTInALineClass inLine = new VCTInALineClass(aTabName, ghasTableStructure, gpFDS);
                    inLine.InALineClass();
                    UpdateStatus(aTabName + "导入完毕！");


                }
                else
                if  (aJhlx.ToUpper() == "POLYGON") 
                {
                    VCTInAPolygonClass inAM = new VCTInAPolygonClass(aTabName, ghasTableStructure, gpFDS);                   
                    inAM.InAPolygonClass();
                    UpdateStatus(aTabName + "导入完毕！");
                    UpdateStatus(aTabName + " 图形时间" + inAM.geoOpTick.ToString() + "    文件读取" + inAM.fileTick.ToString()+"  写属性"+inAM.writeatttick.ToString());
                    
                }
                
                
            }
            if (this.chkExtATTR.Checked)
            {
                
                //扩展属性
                ArrayList needKzbs = new ArrayList();
                foreach (DictionaryEntry de in ghasFeatuerCode)
                {
                    FeatureCodeBeginEnd aFCCode = (FeatureCodeBeginEnd)de.Value;
                    ArrayList aKzb = aFCCode.aKZSXBM;
                    if (aKzb != null && aKzb.Count > 0)
                    {
                        for (int k = 0; k < aKzb.Count; k++)
                        {
                            string sKzbName = (string)aKzb[k];
                            if (VCTHelper.CreateAttrTable(gpFDS.Workspace, sKzbName, ghasTableStructure))
                            {
                                try
                                {
                                    VCTInAExtAttrClass inATable=new VCTInAExtAttrClass(sKzbName,ghasTableStructure,gpWs);
                                    UpdateStatus("导入扩展表"+sKzbName+"成功！");
                                }
                                catch (Exception ex)
                                {
                                    UpdateStatus("导入扩展表"+sKzbName+"失败"+ex.Message);
                                }
                            }
                        }
                    }
                }


            }


            this.Cursor = Cursors.Default;
            MessageBox.Show("全部导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void beVCTFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string sFilter = "VCT文件(*.vct)|*.vct";
            dlg.Filter = sFilter;
            dlg.InitialDirectory = Application.StartupPath;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            this.beVCTFile.Text = dlg.FileName;
        }

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestDir.Text = dlg.SelectedPath;
        }

        private void Allshp2Fgdb(string sTmpFilePath,IFeatureDataset destDS)
        {
            IWorkspace destWS=destDS.Workspace;
            string[] shpfiles = System.IO.Directory.GetFiles(sTmpFilePath, "*.SHP", System.IO.SearchOption.AllDirectories);
            foreach (string aShp in shpfiles)
            {
                string shpName = System.IO.Path.GetFileNameWithoutExtension(aShp);
                IWorkspace srcWS = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(System.IO.Path.GetDirectoryName(aShp));
                if (srcWS != null)
                {
                    RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(srcWS, destWS, shpName, shpName, destDS, null);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(srcWS);
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            
            //转化为shp
            if (this.beVCTFile.Text.Trim() == "") return;
            if (this.beDestDir.Text.Trim() == "") return;
            string destFile = this.beDestDir.Text + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.beVCTFile.Text) + ".gdb";
            if (System.IO.Directory.Exists(destFile))
            {
                MessageBox.Show("该目录已经存在！");
                return;
            }
           

            memoLog.Text = "";
           
            Hashtable ghasTableStructure = null;
            Hashtable ghasFeatuerCode = null;
            this.Cursor = Cursors.WaitCursor;
            string vctFile = this.beVCTFile.Text;
            Encoding gb2312 = Encoding.GetEncoding("GB2312");

            VCTReader reader = new VCTReader();
            reader.IsInExtTable = this.chkExtATTR.Checked;

            string sTmpFilePath = RCIS.Global.AppParameters.VCTIN_TMP;
            if (Directory.Exists(sTmpFilePath))
            {
                RCIS.Utility.FileHelper.DelectDir(sTmpFilePath);
            }

            try
            {


                #region 拆分文件
                using (StreamReader sr = new StreamReader(vctFile, gb2312))
                {

                    //以'属性表名'为Key,存放TableStructClass信息
                    reader.GetFeatureCodes(sr);
                    reader.GetAllTableStruct(sr);
                    ghasTableStructure = reader.ghasTableStructure;
                    ghasFeatuerCode = reader.ghasFeatuerCode;


                    UpdateStatus("读取VCT结构信息完毕！");
                    Dictionary<string, ArrayList> dicPt = reader.getNewPointDataFile(sr);
                    UpdateStatus("拆分点文件结束！");
                    Dictionary<string, ArrayList> dicLine = reader.getNewLineDataFile(sr);
                    UpdateStatus("拆分线文件结束！");
                    Dictionary<string, ArrayList> dicPolygon = reader.getNewPolygonDataFile(sr);
                    UpdateStatus("拆分面文件结束！");
                    #region 合并为一个

                    foreach (KeyValuePair<string, ArrayList> aItem in dicLine)
                    {
                        if (!dicPt.ContainsKey(aItem.Key))
                        {
                            dicPt.Add(aItem.Key, aItem.Value);
                        }
                    }
                    dicLine.Clear();
                    foreach (KeyValuePair<string, ArrayList> aItem in dicPolygon)
                    {
                        if (!dicPt.ContainsKey(aItem.Key))
                        {
                            dicPt.Add(aItem.Key, aItem.Value);
                        }
                    }
                    dicPolygon.Clear();
                    #endregion

                    //整理属性文件
                    reader.GetNewAttrFile(sr, dicPt);
                    UpdateStatus("拆分属性文件结束！");
                    sr.Close();

                    dicPt.Clear();
                }

              
                #endregion

            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }

            //创建shp
            foreach (DictionaryEntry de in ghasFeatuerCode)
            {
                FeatureCodeBeginEnd aFCCode = (FeatureCodeBeginEnd)de.Value;
                string aTabName = aFCCode.sSXBM;
                string aJhlx = aFCCode.sJHLX;
                TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[aTabName];

                VCTHelper.CreateSHPGdal(sTmpFilePath + "\\" + aTabName, aJhlx.ToUpper(), aTabName, curItem);

            }
            UpdateStatus("结构创建完毕！");

          
            foreach (DictionaryEntry de in ghasFeatuerCode)
            {

                FeatureCodeBeginEnd aFCCode = (FeatureCodeBeginEnd)de.Value;
                string aTabName = aFCCode.sSXBM;
                string aJhlx = aFCCode.sJHLX;
                string shpFile = sTmpFilePath + "\\" + aTabName + "\\" + aTabName + ".shp";
                if (aJhlx.ToUpper() == "POINT")
                {
                    
                    VCT2.VCTInAPOINT2Shp inPt = new VCTInAPOINT2Shp(aTabName, ghasTableStructure, shpFile);
                    inPt.InAPointClass();
                    UpdateStatus(aTabName + "导入完毕！");

                }
                else if (aJhlx.ToUpper() == "LINE")
                {
                   
                    VCT2.VCTInALINE2SHP inLIne = new VCTInALINE2SHP(aTabName, ghasTableStructure,shpFile);
                    inLIne.InALineClass();
                    UpdateStatus(aTabName + "导入完毕！");


                }
                else
                    if (aJhlx.ToUpper() == "POLYGON")
                    {

                        VCTAPolygon2Shp inAM = new VCTAPolygon2Shp(aTabName, ghasTableStructure, shpFile);
                        inAM.InAPolygonClass();
                        UpdateStatus(aTabName + "导入完毕！");
                        //UpdateStatus(aTabName + " 图形时间" + inAM.geoOpTick.ToString() + "    文件读取" + inAM.fileTick.ToString() + "  写属性" + inAM.writeatttick.ToString());

                    }


            }
            
            
            string sourceDir = RCIS.Global.AppParameters.TemplatePath + @"\VCTInput.gdb";
            RCIS.Utility.File_DirManipulate.FolderCopy(sourceDir, destFile);
            //创建数据集
            IWorkspace destWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(destFile);
            if (destWS == null)
            {
                memoLog.Text += "\r\n" + DateTime.Now.ToString() + "创建文件数据库失败，是否找不到模板文件...";
                MessageBox.Show("创建文件数据库失败！");
                return;
            }

            string sDSName = RCIS.Global.AppParameters.DATASET_DEFAULT_NAME; //数据集名称
            //创建数据集
            //IFeatureDataset gpFDS=null;
            ISpatialReference spatialReference = null;
            IFeatureDataset destFDS = null;
            try
            {
                spatialReference = reader.getSpatialRef(vctFile);

                IFeatureWorkspace pFcWps = destWS as IFeatureWorkspace;
                destFDS = pFcWps.CreateFeatureDataset(sDSName, spatialReference);

            }
            catch (Exception ex)
            {
                MessageBox.Show("创建数据集失败！");
                memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + ex.Message;
                return;
            }
            if (destFDS == null)
                return;
            Allshp2Fgdb(sTmpFilePath, destFDS);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(destFDS);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(destWS);
            UpdateStatus("入库完毕！");
            this.Cursor = Cursors.Default;
            MessageBox.Show("全部导入完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
