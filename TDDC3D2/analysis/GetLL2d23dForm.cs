using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Database;
using RCIS.Utility;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using System.Reflection;
using ESRI.ArcGIS.Geoprocessor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ESRI.ArcGIS.Geoprocessing;
//using Microsoft.Office.Interop.Excel;
//using ExcelApplication = Microsoft.Office.Interop.Excel.ApplicationClass;


namespace TDDC3D.analysis
{
    public partial class GetLL2d23dForm : Form
    {
        public GetLL2d23dForm()
        {
            InitializeComponent();
        }


        public IMap currMap = null;
        public IWorkspace currWs = null;
        IWorkspace targetWs = null; //更新过程所在工作空间
        IWorkspaceEdit srcWsEdit = null;

        private IFeatureClass dltbGxgcClass = null;      
       // IWorkspaceEdit targetWkspcEdit = null;

        string llfxName = "LLFX23";

        #region 运行gp工具函数
        // Function for returning the tool messages.
        private string ReturnMessages(Geoprocessor gp)
        {
            string ms = "";
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    ms += gp.GetMessage(Count);
                }
            }
            return ms;
        }
        private string RunTool(Geoprocessor geoprocessor, IGPProcess process, ITrackCancel TC)
        {
            geoprocessor.OverwriteOutput = true;

            try
            {
                geoprocessor.Execute(process, null);
                return ReturnMessages(geoprocessor);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return ReturnMessages(geoprocessor);
            }
        }
        #endregion 
        private void GetLL2d23dForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           
           
        }

        private void GetLL2d23dForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfxbak.mdb"))
                {
                    if (System.IO.File.Exists(System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfx.mdb"))
                    {
                        System.IO.File.Delete(System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfx.mdb");
                        System.IO.File.Copy(System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfxbak.mdb", System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfx.mdb");
                    }
                }
                if (Directory.Exists(Application.StartupPath + @"\tmp\temp.gdb"))
                {
                    IDataset pDataset = GetWorkspace(Application.StartupPath + @"\tmp\temp.gdb") as IDataset;
                    pDataset.Delete();
                }
            }
            catch
            { }

            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap);
            int idx = -1;
            for (int i = 0; i < this.cmbLayers.Properties.Items.Count; i++)
            {
                if (OtherHelper.GetLeftName(this.cmbLayers.Properties.Items[i].ToString().Trim()) == "DLTB")
                {
                    idx = i;
                    break;
                }
            }
            this.cmbLayers.SelectedIndex = idx;

            //删除要素类
            targetWs = WorkspaceHelper2.GetAccessWorkspace(System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfx.mdb");
            IFeatureWorkspace targetFeaWs = targetWs as IFeatureWorkspace;
            //targetWkspcEdit = targetWs as IWorkspaceEdit;

            srcWsEdit = this.currWs as IWorkspaceEdit;

            #region 获取行政区代码
            IFeatureClass pXZQClass = null;
            xzdm = "000000";
            try
            {
                pXZQClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch { }
            if (pXZQClass != null)
            {
                IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                if (firstFea != null)
                {
                    xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "ZLDWDM");
                }
            }
            if (xzdm.Length > 6)
            {
                xzdm = xzdm.Substring(0, 6);
            }
            #endregion 

        }

        private void beDltbHShp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SHP文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string dltbhFile = dlg.FileName;
            this.beDltbHShp.Text = dltbhFile;
        }


        private string tmpFilePath = System.Windows.Forms.Application.StartupPath + @"\tmp";  //临时文件夹

        private IFeatureClass GetIntersectClass(string bgqshp, string bghshp,ref string err)
        {
            //"D:\第三次土地调查\测试数据\新建文件地理数据库.gdb\TDDC\DLTB #;
            //D:\第三次土地调查\ArcGIS版源程序\release\tmp\DLTB_H.shp #" 
            //D:\第三次土地调查\ArcGIS版源程序\release\tmp\dltbgxgc.shp ALL # INPUT


           
            string tmpShp =targetWs.PathName+"\\DLTBGXGC"; //目标shp路径

            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Intersect pIntersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
            pIntersect.cluster_tolerance = 0.001;

            //C:\Users\guojie4\Desktop\test.mdb\BDC\asdsfd
            string inLyr1 = bgqshp; // this.m_srcWs.PathName + @"\BDC\ZD_H";
            string inLyr2 = bghshp; //this.m_srcWs.PathName + @"\BDC\ZD";
            pIntersect.in_features = inLyr1 + ";" + inLyr2;
            pIntersect.out_feature_class = tmpShp;
            pIntersect.join_attributes = "ALL";
            pIntersect.output_type = "INPUT";



            err = RunTool(gp, pIntersect, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return null;
            }
            
            IFeatureClass shpClass = (targetWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            return shpClass;
        }

        private IFeatureClass GetIntersectClassInGDB(string bgqshp, string bghshp, ref string err)
        {
            string tmpShp = this.currWs.PathName + "\\"+this.llfxName; //目标shp路径

            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Intersect pIntersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
            pIntersect.cluster_tolerance = 0.001;

            //C:\Users\guojie4\Desktop\test.mdb\BDC\asdsfd
            string inLyr1 = bgqshp; // this.m_srcWs.PathName + @"\BDC\ZD_H";
            string inLyr2 = bghshp; //this.m_srcWs.PathName + @"\BDC\ZD";
            pIntersect.in_features = inLyr1 + ";" + inLyr2;
            pIntersect.out_feature_class = tmpShp;
            pIntersect.join_attributes = "ALL";
            pIntersect.output_type = "INPUT";



            err = RunTool(gp, pIntersect, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return null;
            }

            IFeatureClass shpClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(this.llfxName);
            return shpClass;
        }

        private IFeatureClass GetIntersectClassInGDB(string bghshp)
        {
            string bgqshp = this.currWs.PathName + "\\" + this.llfxName;
            string tmpShp = this.currWs.PathName + "\\tempxzdw"; //目标shp路径

            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Intersect pIntersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
            pIntersect.cluster_tolerance = 0.001;

            //C:\Users\guojie4\Desktop\test.mdb\BDC\asdsfd
            string inLyr1 = bgqshp; // this.m_srcWs.PathName + @"\BDC\ZD_H";
            string inLyr2 = bghshp; //this.m_srcWs.PathName + @"\BDC\ZD";
            pIntersect.in_features = inLyr1 + ";" + inLyr2;
            pIntersect.out_feature_class = tmpShp;
            pIntersect.join_attributes = "ALL";
            pIntersect.output_type = "INPUT";
            try
            {
                gp.Execute(pIntersect, null);

            }
            catch (Exception err)
            {
            }

            IFeatureClass shpClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("tempxzdw");
            return shpClass;
        }

        private bool StartEditOp()
        {
            bool retVal = false;

            // Check to see if we're editing
            if (!this.srcWsEdit.IsBeingEdited())
            {
                // Not being edited so start here
                srcWsEdit.StartEditing(false);
                retVal = true;
            }

            // Start operation
            srcWsEdit.StartEditOperation();
            return retVal;
        }
        private void StopEditOp(bool weStartedEditing)
        {
            // Stop edit operation
            srcWsEdit.StopEditOperation();
            if (weStartedEditing)
            {
                // We started the edit session so stop it here
                srcWsEdit.StopEditing(true);
            }
        }

        private void UpdateStatus(string txt)
        {
            this.memoLog.Text = DateTime.Now.ToString() + ":" +txt+"\r\n"+  this.memoLog.Text;
            System.Windows.Forms.Application.DoEvents();
        }

        private void InsertAYlb(string xzdm,string ydl, string xdl, double bgmj)
        {
            string sql = "insert into BG_YLB_PFM(xzdm,ydl,xdl,bgmj) values('" + xzdm + "','" + ydl + "','" + xdl + "'," + bgmj + ") ";
            int iret= RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void InsertAYlb(string xzdm, string ydl, string xdl, double bgmj,
            string bgqzldwdm,string bghzldwdm,string bgqtbbh,string bghtbbh,
            string bgqqsxz,string bghqsxz,string bgqpdfj,string bghpdfj,string bgqgdlx,string bghgdlx,string jlbh
            )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" insert into BG_YLB_PFM(xzdm,ydl,xdl,bgmj,JLBH,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)")
                .Append(" values ('").Append(xzdm).Append("','").Append(ydl).Append("','").Append(xdl).Append("',").Append(bgmj)
                .Append(",'").Append(jlbh).Append("','").Append(bgqzldwdm).Append("','").Append(bghzldwdm).Append("','").Append(bgqtbbh).Append("','").Append(bghtbbh)
                .Append("','").Append(bgqqsxz).Append("','").Append(bghqsxz).Append("','").Append(bgqpdfj).Append("','").Append(bghpdfj).Append("','").Append(bgqgdlx)
                .Append("','").Append(bghgdlx).Append("' ) ");


            string sql = sb.ToString();  //"insert into BG_YLB_PFM(xzdm,ydl,xdl,bgmj) values('" + xzdm + "','" + ydl + "','" + xdl + "'," + bgmj + ") ";
            int iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
        }


        private void CalBgbByGc(string xzdm, bool xzdw)
        {
            this.UpdateStatus("正在提取一览表...");
            RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery("delete from BG_YLB_PFM");
            string bgjlbh = xzdm + DateTime.Now.ToString("yyyyMMdd").PadLeft(10, '0');
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM_12,DLBM_1,MJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1 from  dltbxzdw ");
            string sql = sb.ToString();  
            int iret;
            if (xzdw) 
            { 
                iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql); 
            }
            else
            {
                iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery("Update DLTBGXGC Set dlbm_1 = left(dlbm_12,4)");
            }
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) = 0 and iif(isnull( KCMJ),0,KCMJ) = 0");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) = 0");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123',DLBM_1,TKMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) = 0");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ - KCMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) = 0 and iif(isnull( KCMJ),0,KCMJ) <> 0");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,'1203',KCMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) = 0 and iif(isnull( KCMJ),0,KCMJ) <> 0");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) <> 0 and iif(isnull(TKMJ),0,TKMJ) > iif(isnull( KCMJ),0,KCMJ)");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123','1203',KCMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) <> 0 and iif(isnull(TKMJ),0,TKMJ) > iif(isnull( KCMJ),0,KCMJ)");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear();
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123',DLBM_1,TKMJ - KCMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) <> 0 and iif(isnull(TKMJ),0,TKMJ) > iif(isnull( KCMJ),0,KCMJ)");
            sql = sb.ToString();
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,'1203',KCMJ - TKMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) <> 0 and iif(isnull(TKMJ),0,TKMJ) < iif(isnull( KCMJ),0,KCMJ)");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear(); 
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123','1203',TKMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) <> 0 and iif(isnull(TKMJ),0,TKMJ) < iif(isnull( KCMJ),0,KCMJ)");
            sql = sb.ToString();  
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            sb.Clear();
            sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
            sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ - KCMJ + TKMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");
            sb.Append("where iif(isnull(TKMJ),0,TKMJ) <> 0 and iif(isnull( KCMJ),0,KCMJ) <> 0 and iif(isnull(TKMJ),0,TKMJ) < iif(isnull( KCMJ),0,KCMJ)");
            sql = sb.ToString();
            iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            //对于不是耕地的，直接插入
           // sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           // sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ");//where iif(isnull(TKMJ),0,TKMJ)>iif(isnull( KCMJ),0,KCMJ)  ");
           //// .Append(" where left(dlbm,2)<>'01' and left(dlbm_1,2)<>'01' ");
           // sql = sb.ToString();  //" select '" + xzdm + "',DLBM,DLBM_1,TBDLMJ_1 from DLTBGXGC where left(dlbm,2)<>'01' and left(dlbm_1,2)<>'01' ";
           // iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
           // //sb.Clear(); ;
           // //sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           // //sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,DLBM_1,TBDLMJ_1,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC where iif(isnull(TKMJ),0,TKMJ)<=iif(isnull( KCMJ),0,KCMJ)");
           // //// .Append(" where left(dlbm,2)<>'01' and left(dlbm_1,2)<>'01' ");
           // //sql = sb.ToString();  //" select '" + xzdm + "',DLBM,DLBM_1,TBDLMJ_1 from DLTBGXGC where left(dlbm,2)<>'01' and left(dlbm_1,2)<>'01' ";
           // //iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);

            try
            {
               
           //     //string bgqKcdlbm="123";
           //     //string bghKcdlbm="1203";

           //     //dlbm_1 是现状 ，dlbm 是变更前地类
           //     ////现地类是耕地，原地类不是耕地, 非耕地变为耕地，如果现地类有扣除系统，非耕地变为田坎
               
           //     //sb.Clear();
           //     //sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     //sb.Append(" select '" + xzdm + "','" + bgjlbh + "',DLBM,'1203', TBMJ_1*KCXS  ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //     //    .Append(" where left(dlbm,2)<>'01' and left(dlbm_1,2)='01' and  KCXS>0  ");
           //     //sql = sb.ToString();
           //     //iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);

           //     ////原地类是耕地，耕地变为现状，如果有田坎系数，田坎变为现状                
           //     //sb.Clear();
           //     //sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     //sb.Append(" select '" + xzdm + "','" + bgjlbh + "','123',DLBM_1, TBMJ_1*TKXS  ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //     //    .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)<>'01' and  TKXS>0  ");
           //     //sql = sb.ToString();
           //     //iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);


           //     ////现地类和原地类都是耕地，耕地内部变化，同时还需要有 扣除系统的对比
           //     //sb.Clear();
           //     //sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     //sb.Append(" select '" + xzdm + "','" + bgjlbh + "',DLBM,DLBM_1, TBDLMJ_1,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //     //    // .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)='01' and  TKXS>KCXS ");
           //     //    .Append(" where iif(isnull(TKXS),0,TKXS)>iif(isnull( KCXS),0,KCXS) ");
           //     //sql = sb.ToString();
           //     //iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
           //     sb.Clear();
           //     sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123',DLBM_1,  iif(isnull(TKMJ),0,TKMJ)-iif(isnull( KCMJ),0,KCMJ)  ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //        // .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)='01' and  TKXS>KCXS ");
           //         .Append(" where iif(isnull(TKMJ),0,TKMJ)>iif(isnull( KCMJ),0,KCMJ) ");
           //     sql = sb.ToString();
           //     iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
           //     sb.Clear();
           //     sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123','1203', KCMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //         // .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)='01' and  TKXS>KCXS ");
           //         .Append(" where iif(isnull(TKMJ),0,TKMJ)>iif(isnull( KCMJ),0,KCMJ) ");
           //     sql = sb.ToString();
           //     iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
           //     //sb.Clear();
           //     //sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     //sb.Append(" select '" + xzdm + "','" + bgjlbh + "',DLBM, DLBM_1, TBDLMJ  ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //     //    // .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)='01' and  TKXS<KCXS ");
           //     //    .Append(" where    iif(isnull(TKXS),0,TKXS)<iif(isnull( KCXS),0,KCXS) ");
           //     //sql = sb.ToString();
           //     //iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
           //     sb.Clear();
           //     sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     sb.Append(" select ZLDWDM_1,'" + bgjlbh + "',DLBM,'1203', iif(isnull( KCMJ),0,KCMJ)- iif(isnull(TKMJ),0,TKMJ)  ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //        // .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)='01' and  TKXS<KCXS ");
           //         .Append(" where    iif(isnull(TKMJ),0,TKMJ)<iif(isnull( KCMJ),0,KCMJ) ");
           //     sql = sb.ToString();
           //     iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
           //     sb.Clear();
           //     sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX)");
           //     sb.Append(" select ZLDWDM_1,'" + bgjlbh + "','123','1203', TKMJ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from  DLTBGXGC ")
           //         // .Append(" where  left(dlbm,2)='01' and left(dlbm_1,2)='01' and  TKXS<KCXS ");
           //         .Append(" where    iif(isnull(TKMJ),0,TKMJ)<iif(isnull( KCMJ),0,KCMJ) ");
           //     sql = sb.ToString();
           //     iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);

                sb.Clear();
                sb.Append("Delete From BG_YLB_PFM Where (YDL = '115' And XDL = '1105')");
                sb.Append(" Or (YDL = '116' And XDL = '1106')");
                sb.Append(" Or (YDL = '125' And XDL = '1108')");
                sb.Append(" Or (YDL = '011' And XDL = '0101')");
                sb.Append(" Or (YDL = '012' And XDL = '0102')");
                sb.Append(" Or (YDL = '013' And XDL = '0103')");
                sb.Append(" Or (YDL = '021' And XDL = '0201')");
                sb.Append(" Or (YDL = '022' And XDL = '0202')");
                sb.Append(" Or (YDL = '023' And XDL = '0204')");
                sb.Append(" Or (YDL = '031' And XDL = '0301')");
                sb.Append(" Or (YDL = '032' And XDL = '0305')");
                sb.Append(" Or (YDL = '033' And XDL = '0307')");
                sb.Append(" Or (YDL = '041' And XDL = '0401')");
                sb.Append(" Or (YDL = '042' And XDL = '0403')");
                sb.Append(" Or (YDL = '043' And XDL = '0404')");
                sb.Append(" Or (YDL = '101' And XDL = '1001')");
                sb.Append(" Or (YDL = '102' And XDL = '1003')");
                //sb.Append(" Or (YDL = '103' And XDL = '1004')");
                sb.Append(" Or (YDL = '103' And XDL = '1005')");
                sb.Append(" Or (YDL = '104' And XDL = '1006')");
                sb.Append(" Or (YDL = '105' And XDL = '1007')");
                sb.Append(" Or (YDL = '106' And XDL = '1008')");
                sb.Append(" Or (YDL = '107' And XDL = '1009')");
                sb.Append(" Or (YDL = '111' And XDL = '1101')");
                sb.Append(" Or (YDL = '112' And XDL = '1102')");
                sb.Append(" Or (YDL = '113' And XDL = '1103')");
                sb.Append(" Or (YDL = '114' And XDL = '1104')");
                sb.Append(" Or (YDL = '117' And XDL = '1107')");
                sb.Append(" Or (YDL = '118' And XDL = '1109')");
                sb.Append(" Or (YDL = '119' And XDL = '1110')");
                //sb.Append(" Or (YDL = '121' And XDL = '1201')");
                sb.Append(" Or (YDL = '122' And XDL = '1202')");
                sb.Append(" Or (YDL = '123' And XDL = '1203')");
                sb.Append(" Or (YDL = '124' And XDL = '1204')");
                sb.Append(" Or (YDL = '126' And XDL = '1205')");
                sb.Append(" Or (YDL = '127' And XDL = '1206')");
                sb.Append(" Or (YDL = '205' And XDL = '09')");
                sb.Append(" Or (YDL = '201' And XDL = '05H1')");
                sb.Append(" Or (YDL = '201' And XDL = '0508')");
                sb.Append(" Or (YDL = '202' And XDL = '05H1')");
                sb.Append(" Or (YDL = '202' And XDL = '0508')");
                sb.Append(" Or (YDL = '204' And XDL = '0601')");
                sb.Append(" Or (YDL = '204' And XDL = '0602')");
                sb.Append(" Or (YDL = '203' And XDL = '0701')");
                sb.Append(" Or (YDL = '203' And XDL = '0702')");
                sql = sb.ToString();
                iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);

                #region 废弃

                //sb.Clear();
                //sb.Append(" select DLBM,DLBM_1,TKXS,KCXS,TBMJ_1,TBDLMJ_1 ,ZLDWDM,ZLDWDM_1,TBBH,TBBH_1,QSXZ,QSXZ_1,GDPDJ,GDPDJB,GDLX,GDLX_1 from DLTBGXGC")
                //    .Append(" where left(dlbm,2)='01' or left(dlbm_1,2)='01' " );
                //sql=sb.ToString();
                //System.Data.DataTable datatable = LS_LlfxMDBHelper.GetDataTable(sql,"LL");

                ////RCIS.Database.LS_LlfxMDBHelper.GetDataTable("select DLBM,DLBM_1,TKXS,KCXS, TBMJ_1,TBDLMJ_1 from DLTBGXGC where left(dlbm,2)='01' or left(dlbm_1,2)='01' ","LL");

                //foreach (DataRow dr in datatable.Rows)
                //{
                //    string ydl = dr["DLBM"].ToString();
                //    if (ydl == "")
                //        continue;
                //    if (ydl.Length > 3)
                //        ydl = ydl.Substring(0, 3);
                //    string xdl = dr["DLBM_1"].ToString();
                //    if (xdl.Length > 4)
                //        xdl = xdl.Substring(0, 4);

                //    double tbmj = 0;
                //    double.TryParse(dr["TBMJ_1"].ToString(),out tbmj);
                //    double bghdlmj = 0;
                //    double.TryParse(dr["TBDLMJ_1"].ToString(), out bghdlmj);
                //    double kcxs = 0;
                //    double.TryParse(dr["TKXS"].ToString(), out  kcxs);
                //    double kcxs2 = 0;
                //    double.TryParse(dr["KCXS"].ToString(), out kcxs2);

                //    string bgqzldwdm=dr["ZLDWDM"].ToString();
                //    string bghzldwdm=dr["ZLDWDM_1"].ToString();
                //    string bgqtbbh=dr["TBBH"].ToString();
                //    string bghtbbh=dr["TBBH_1"].ToString();
                //    string bgqqsxz=dr["QSXZ"].ToString();
                //    string bghqsxz=dr["QSXZ_1"].ToString();
                //    string bgqpdfj=dr["GDPDJ"].ToString();
                //    string bghpdfj=dr["GDPDJB"].ToString();
                //    string bgqgdlx=dr["GDLX"].ToString();
                //    string bghgdlx=dr["GDLX_1"].ToString();

                //    if (ydl.StartsWith("01") && xdl.StartsWith("01"))
                //        {
                //            //都是耕地，页存在内部变化情况
                //            if (ydl != xdl)
                //            {
                //               // InsertAYlb(xzdm, ydl, xdl, bghdlmj);
                //                InsertAYlb(xzdm,ydl,xdl,bghdlmj,bgqzldwdm,bghzldwdm,bgqtbbh,bghtbbh,bgqqsxz,bghqsxz,bgqpdfj,bghpdfj,bgqgdlx,bghgdlx,bgjlbh);
                //            }

                //            if (kcxs > kcxs2)
                //            {
                //                //说明田坎变耕地了，扣除系数变小了
                //                double bgmj = tbmj * (kcxs - kcxs2);
                //                InsertAYlb(xzdm, bgqKcdlbm, xdl, bgmj,bgqzldwdm,bghzldwdm,bgqtbbh,bghtbbh,bgqqsxz,bghqsxz,bgqpdfj,bghpdfj,bgqgdlx,bghgdlx,bgjlbh);
                //            }
                //            else if (kcxs2 > kcxs)
                //            {
                //                //说明 耕地变田坎，扣除系数大了
                //                double bgmj = tbmj * (kcxs2 - kcxs);
                //                //InsertAYlb(xzdm, ydl, bghKcdlbm, bgmj);
                //                InsertAYlb(xzdm, ydl, bghKcdlbm, bgmj,bgqzldwdm,bghzldwdm,bgqtbbh,bghtbbh,bgqqsxz,bghqsxz,bgqpdfj,bghpdfj,bgqgdlx,bghgdlx,bgjlbh);
                //            }

                //        }
                       
                //        else if (ydl.StartsWith("01") && !xdl.StartsWith("01"))
                //        {
                //            //如果原来是耕地，变为非耕地，地类变化，同时，如果扣除系数原来不是空，则意味着部分田坎页变成了 现地类
                //            InsertAYlb(xzdm, ydl, xdl, bghdlmj);
                //            if (kcxs > 0)
                //            {
                //                double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                //                //InsertAYlb(xzdm, bgqKcdlbm, xdl, kcmj);
                //                InsertAYlb(xzdm, bgqKcdlbm, xdl, kcmj,bgqzldwdm,bghzldwdm,bgqtbbh,bghtbbh,bgqqsxz,bghqsxz,bgqpdfj,bghpdfj,bgqgdlx,bghgdlx,bgjlbh);
                //            }

                //        }
                //        else if (!ydl.StartsWith("01") && xdl.StartsWith("01"))
                //        {
                //            //非耕地变为了耕地
                //            InsertAYlb(xzdm, ydl, xdl, bghdlmj);
                //            //如果现在扣除系数不为空，则 意味着 原来的地类变为了田坎
                //            if (kcxs2 > 0)
                //            {
                //                double kcmj = MathHelper.RoundEx(tbmj * kcxs2, 2);
                //                //InsertAYlb(xzdm, ydl, bghKcdlbm, kcmj);
                //                InsertAYlb(xzdm, ydl, bghKcdlbm, kcmj,bgqzldwdm,bghzldwdm,bgqtbbh,bghtbbh,bgqqsxz,bghqsxz,bgqpdfj,bghpdfj,bgqgdlx,bghgdlx,bgjlbh);
                //            }
                //        }


                //}

                #endregion 

                //开始进行平衡表计算
                this.UpdateStatus("正在将一览表转化为公顷并调平...");
                //一览表 汇总候变为公顷
                RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery("delete from BG_YLB");
                iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery("insert into BG_YLB(XZDM,YDL,XDL,BGMJ) select XZDM,YDL,XDL,ROUND(sum(bGMJ)/10000,2) from BG_YLB_PFM   group by xzdm,ydl,xdl ");


                //记录平方米的 总变更面积

                double zmj = getZmj("BG_YLB_PFM"); //平方米的 总面积
                zmj = MathHelper.RoundEx(zmj / 10000, 2);
                double gqZmj = getZmj("BG_YLB");
                this.gqTP(zmj, gqZmj); //经过调平后，公顷一览表中 的合计数 相等了

            }
            catch (Exception ex)
            {
                
            }


        }
        private void BgbInWare(string xcode)
        {

            //开始入库，从变更表中取
            

            string sql = "Select xzdm from BG_YLB where (xzdm like '" + xcode + "%' )  group by xzdm";
            DataTable dttemp = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");

            string[] allXdl = new string[] { "D0101","D0102","D0103","D0201","D0202","D0203","D0204",
                 "D0301","D0302","D0303","D0304","D0305","D0306","D0307","D0401","D0402","D0403","D0404","D05H1","D0508","D0601","D0602","D0603",
                 "D0701","D0702","D08H1","D08H2","D0809","D0810","D09","D1001","D1002","D1003","D1004","D1005","D1006","D1007","D1008","D1009",
                 "D1101","D1102","D1103","D1104","D1105","D1106","D1107","D1108","D1109","D1110", "D1201","D1202","D1203","D1204","D1205","D1206","D1207"};
            string[] allYdl = new string[]{"D011","D012","D013","D021","D022","D023","D031","D032","D033","D041","D042","D043","D201","D202",
                "D203","D204","D205","D101","D102","D104","D105","D106","D107","D111","D112","D113","D114","D115","D116","D117","D118","D119",
                "D122","D123","D124","D125","D126","D127"//"D103","D121",
            };
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                string zldwdm = dttemp.Rows[i][0].ToString();
                Console.WriteLine(zldwdm);
                foreach (string aDlh in allYdl)
                {
                    sql = "insert into BG_PHB_23 (xzdm,dlh) values('" + zldwdm + "','" + aDlh + "' ) ";
                    int ret = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                    if (ret == 0) MessageBox.Show("KJL");
                }
                sql = @"select xzdm,left(ydl,3) as yldl,left(xdl,4) as xzdl,sum(bgmj) as bhmj from BG_YLB where (xzdm like '" + zldwdm + @"%' )  group by xzdm,left(ydl,3),left(xdl,4)  ";
                System.Data.DataTable dtYlb = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                foreach (DataRow dr in dtYlb.Rows)
                {
                    string ydl = dr["YLDL"].ToString();
                    if (ydl.Length > 3) ydl = ydl.Substring(0, 3);
                    string xdl = dr["XZDL"].ToString();
                    if (xdl.Length > 4) xdl = xdl.Substring(0, 4);
                    double bgmj = 0;
                    double.TryParse(dr["BHMJ"].ToString(), out bgmj);
                    sql = "update BG_PHB_23 set D" + xdl + " = " + bgmj + " where DLH='D" + ydl + "' and xzdm='" + dr["xzdm"].ToString() + "' ";
                    int ret = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                    if (ret == 0) MessageBox.Show("KJL");
                }
            }

            
            //AddLog("入库完毕...");

        }

        #region 废弃
        //private void in3dmj(string xcode)
        //{
        //    #region 计算三调面积
        //    try
        //    {
        //        IFeatureClass pDltb3 = (this.currWs as IFeatureWorkspace).OpenFeatureClass(OtherHelper.GetLeftName(this.cmbLayers.Text.Trim()));

        //        //string[] field2d=new string[]{"011","012","013","021","022","023","031","032","033","041","042","043",
        //        //"101","102","104","105","106","107","111","112","113","114","115","116","117","118","119","122","123","124","125","126","127",
        //        //"201","202","203","204","205"};
        //        double d00, d0303, d0304, d0306, d0402, d0603, d1105, d1106, d1108, d01, d0101, d0102, d0103, d02, d0201, d0202, d0203, d0204,
        //                d03, d0301, d0302, d0305, d0307, d04, d0401, d0403, d0404, d05, d05H1, d0508, d06, d0601, d0602,
        //                d07, d0701, d0702, d08, d08H1, d08H2, d0809, d0810, d09, d10, d1001, d1002, d1003, d1004, d1005, d1006, d1007, d1008, d1009,
        //                d11, d1101, d1102, d1103, d1104, d1107, d1109, d1110,
        //                d12, d1201, d1202, d1203, d1204, d1205, d1206, d1207, dhj = 0;
        //        d0303 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0303'", "TBDLMJ") / 10000, 2);
        //        d0304 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0304'", "TBDLMJ") / 10000, 2);
        //        d0306 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0306'", "TBDLMJ") / 10000, 2);
        //        d0402 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0402'", "TBDLMJ") / 10000, 2);
        //        d0603 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0603'", "TBDLMJ") / 10000, 2);
        //        d1105 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1105'", "TBDLMJ") / 10000, 2);
        //        d1106 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1106'", "TBDLMJ") / 10000, 2);
        //        d1108 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1108'", "TBDLMJ") / 10000, 2);

        //        d0101 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0101'", "TBDLMJ") / 10000, 2);
        //        d0102 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0102'", "TBDLMJ") / 10000, 2);
        //        d0103 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0103'", "TBDLMJ") / 10000, 2);

        //        d0201 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0201%'", "TBDLMJ") / 10000, 2);
        //        d0202 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0202%'", "TBDLMJ") / 10000, 2);
        //        d0203 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0203%'", "TBDLMJ") / 10000, 2);
        //        d0204 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0204%'", "TBDLMJ") / 10000, 2);

        //        d0301 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0301%'", "TBDLMJ") / 10000, 2);
        //        d0302 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0302%'", "TBDLMJ") / 10000, 2);
        //        d0305 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0305%'", "TBDLMJ") / 10000, 2);
        //        d0307 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0307%'", "TBDLMJ") / 10000, 2);

        //        d0401 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0401'", "TBDLMJ") / 10000, 2);
        //        d0403 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0403%'", "TBDLMJ") / 10000, 2);
        //        d0404 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0404'", "TBDLMJ") / 10000, 2);

        //        d05H1 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='05H1'", "TBDLMJ") / 10000, 2);
        //        d0508 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0508'", "TBDLMJ") / 10000, 2);

        //        d0601 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0601'", "TBDLMJ") / 10000, 2);
        //        d0602 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0602'", "TBDLMJ") / 10000, 2);

        //        d0701 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0701'", "TBDLMJ") / 10000, 2);
        //        d0702 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0702'", "TBDLMJ") / 10000, 2);

        //        d0809 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='0809'", "TBDLMJ") / 10000, 2);
        //        d08H1 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='08H1'", "TBDLMJ") / 10000, 2);
        //        d08H2 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '08H2%'", "TBDLMJ") / 10000, 2);
        //        d0810 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '0810%'", "TBDLMJ") / 10000, 2);

        //        d09 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='09'", "TBDLMJ") / 10000, 2);

        //        d1001 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1001'", "TBDLMJ") / 10000, 2);
        //        d1002 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1002'", "TBDLMJ") / 10000, 2);
        //        d1003 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1003'", "TBDLMJ") / 10000, 2);
        //        d1004 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1004'", "TBDLMJ") / 10000, 2);
        //        d1005 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1005'", "TBDLMJ") / 10000, 2);
        //        d1006 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1006'", "TBDLMJ") / 10000, 2);
        //        d1007 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1007'", "TBDLMJ") / 10000, 2);
        //        d1008 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1008'", "TBDLMJ") / 10000, 2);
        //        d1009 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1009'", "TBDLMJ") / 10000, 2);

        //        d1101 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1101'", "TBDLMJ") / 10000, 2);
        //        d1102 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1102'", "TBDLMJ") / 10000, 2);
        //        d1103 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1103'", "TBDLMJ") / 10000, 2);
        //        d1104 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '1104%'", "TBDLMJ") / 10000, 2);
        //        d1105 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1105'", "TBDLMJ") / 10000, 2);
        //        d1106 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1106'", "TBDLMJ") / 10000, 2);
        //        d1107 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM like '1107%'", "TBDLMJ") / 10000, 2);
        //        d1108 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1108'", "TBDLMJ") / 10000, 2);
        //        d1109 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1109'", "TBDLMJ") / 10000, 2);
        //        d1110 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1110'", "TBDLMJ") / 10000, 2);

        //        d1201 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1201'", "TBDLMJ") / 10000, 2);
        //        d1202 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1202'", "TBDLMJ") / 10000, 2);
        //        d1203 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1203'", "TBDLMJ") / 10000, 2);
        //        d1204 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1204'", "TBDLMJ") / 10000, 2);
        //        d1205 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1205'", "TBDLMJ") / 10000, 2);
        //        d1206 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1206'", "TBDLMJ") / 10000, 2);
        //        d1207 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "DLBM='1207'", "TBDLMJ") / 10000, 2);

        //        d1203 += MathHelper.Round(FeatureHelper.StatsFieldSumValue(pDltb3, "", "KCMJ") / 10000, 2);

        //        //转为公顷
        //        d00 = d0303 + d0304 + d0306 + d0402 + d0603 + d1105 + d1106 + d1108;
        //        d01 = d0101 + d0102 + d0103;
        //        d02 = d0201 + d0202 + d0203 + d0204;
        //        d03 = d0301 + d0302 + d0305 + d0307;
        //        d04 = d0401 + d0403;
        //        d05 = d05H1 + d0508;
        //        d06 = d0601 + d0602;
        //        d07 = d0701 + d0702;
        //        d08 = d08H1 + d08H2 + d0809 + d0810;
        //        d10 = d1001 + d1002 + d1003 + d1004 + d1005 + d1006 + d1007 + d1008 + d1009;
        //        d11 = d1101 + d1102 + d1103 + d1104 + d1107 + d1109 + d1110;
        //        d12 = d1201 + d1202 + d1203 + d1204 + d1205 + d1206 + d1207;
        //        dhj = d00 + d01 + d02 + d03 + d04 + d05 + d06 + d07 + d08 + d09 + d10 + d11 + d12;
        //    #endregion

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("insert into BG_PHb_23(XZDM,DLH,d00, d0303, d0304, d0306, d0402, d0603, d1105, d1106, d1108, d01, d0101, d0102, d0103, d02, d0201, d0202, d0203, d0204,")
        //            .Append(" d03, d0301, d0302, d0305, d0307, d04, d0401, d0403, d0404, d05, d05H1, d0508, d06, d0601, d0602, ")
        //            .Append(" d07, d0701, d0702, d08, d08H1, d08H2, d0809, d0810, d09,d10, d1001, d1002, d1003, d1004, d1005, d1006, d1007, d1008, d1009, ")
        //            .Append(" d11, d1101, d1102, d1103, d1104, d1107,  d1109, d1110, d12, d1201, d1202, d1203, d1204, d1205, d1206, d1207,D000 ) values('")
        //            .Append(xcode).Append("','D002',").Append(d00).Append(",").Append(d0303).Append(",").Append(d0304).Append(",").Append(d0306).Append(",").Append(d0402).Append(",")
        //            .Append(d0603).Append(",").Append(d1105).Append(",").Append(d1106).Append(",").Append(d1108).Append(",")
        //            .Append(d01).Append(",").Append(d0101).Append(",").Append(d0102).Append(",").Append(d0103).Append(",")
        //            .Append(d02).Append(",").Append(d0201).Append(",").Append(d0202).Append(",").Append(d0203).Append(",").Append(d0204).Append(",")
        //            .Append(d03).Append(",").Append(d0301).Append(",").Append(d0302).Append(",").Append(d0305).Append(",").Append(d0307).Append(",")
        //            .Append(d04).Append(",").Append(d0401).Append(",").Append(d0403).Append(",").Append(d0404).Append(",")
        //            .Append(d05).Append(",").Append(d05H1).Append(",").Append(d0508).Append(",").Append(d06).Append(",").Append(d0601).Append(",").Append(d0602).Append(",")
        //            .Append(d07).Append(",").Append(d0701).Append(",").Append(d0702).Append(",")
        //            .Append(d08).Append(",").Append(d08H1).Append(",").Append(d08H2).Append(",").Append(d0809).Append(",").Append(d0810).Append(",")
        //            .Append(d09).Append(",")
        //            .Append(d10).Append(",").Append(d1001).Append(",").Append(d1002).Append(",").Append(d1003).Append(",").Append(d1004).Append(",").Append(d1005).Append(",").Append(d1006).Append(",").Append(d1007).Append(",").Append(d1008).Append(",").Append(d1009).Append(",")
        //            .Append(d11).Append(",").Append(d1101).Append(",").Append(d1102).Append(",").Append(d1103).Append(",").Append(d1104).Append(",").Append(d1107).Append(",").Append(d1109).Append(",")
        //            .Append(d1110).Append(",")
        //            .Append(d12).Append(",").Append(d1201).Append(",").Append(d1202).Append(",").Append(d1203).Append(",").Append(d1204).Append(",").Append(d1205).Append(",").Append(d1206).Append(",").Append(d1207).Append(",")
        //            .Append(dhj).Append(") ");
        //        string sql = sb.ToString();
        //        int iret = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
        //    }
        //    catch { }

        //}

        //private void in2dmj(string xcode)
        //{
        //    try
        //    {
        //        IWorkspace ws2 = WorkspaceHelper2.GetShapefileWorkspace(this.beDltbHShp.Text);
        //        string className = System.IO.Path.GetFileNameWithoutExtension(this.beDltbHShp.Text);
        //        IFeatureClass dltb2d = (ws2 as IFeatureWorkspace).OpenFeatureClass(className);
        //        #region 计算面积
        //        double d01, d011, d012, d013, d02, d021, d022, d023,
        //                d03, d031, d032, d033, d04, d041, d042, d043, d20, d201, d202, d203, d204, d205,
        //                 d10, d101, d102, d104, d105, d106, d107,
        //                d11, d111, d112, d113, d114, d115, d116, d117, d118, d119,
        //                d12, d122, d123, d124, d125, d126, d127, dhj = 0;
        //        d011 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='011'", "TBDLMJ") / 10000, 2);
        //        d012 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='012'", "TBDLMJ") / 10000, 2);
        //        d013 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='013'", "TBDLMJ") / 10000, 2);

        //        d021 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '021'", "TBDLMJ") / 10000, 2);
        //        d022 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '022'", "TBDLMJ") / 10000, 2);
        //        d023 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '023'", "TBDLMJ") / 10000, 2);

        //        d031 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '031'", "TBDLMJ") / 10000, 2);
        //        d032 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '032'", "TBDLMJ") / 10000, 2);
        //        d033 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='033'", "TBDLMJ") / 10000, 2);

        //        d041 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='041'", "TBDLMJ") / 10000, 2);
        //        d043 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM ='043'", "TBDLMJ") / 10000, 2);
        //        d042 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='042'", "TBDLMJ") / 10000, 2);

        //        d201 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='201'", "TBDLMJ") / 10000, 2);
        //        d202 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='202'", "TBDLMJ") / 10000, 2);
        //        d203 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='203'", "TBDLMJ") / 10000, 2);
        //        d204 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='204'", "TBDLMJ") / 10000, 2);
        //        d205 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='205'", "TBDLMJ") / 10000, 2);


        //        d101 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='101'", "TBDLMJ") / 10000, 2);
        //        d102 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='102'", "TBDLMJ") / 10000, 2);
        //        d104 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='104'", "TBDLMJ") / 10000, 2);
        //        d105 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='105'", "TBDLMJ") / 10000, 2);
        //        d106 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='106'", "TBDLMJ") / 10000, 2);
        //        d107 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='107'", "TBDLMJ") / 10000, 2);


        //        d111 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='111'", "TBDLMJ") / 10000, 2);
        //        d112 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='112'", "TBDLMJ") / 10000, 2);
        //        d113 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='113'", "TBDLMJ") / 10000, 2);
        //        d114 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '114'", "TBDLMJ") / 10000, 2);
        //        d115 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='115'", "TBDLMJ") / 10000, 2);
        //        d116 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='116'", "TBDLMJ") / 10000, 2);
        //        d117 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM = '117'", "TBDLMJ") / 10000, 2);
        //        d118 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='118'", "TBDLMJ") / 10000, 2);
        //        d119 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='119'", "TBDLMJ") / 10000, 2);


        //        d122 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='122'", "TBDLMJ") / 10000, 2);
        //        d123 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='123'", "TBDLMJ") / 10000, 2);
        //        d124 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='124'", "TBDLMJ") / 10000, 2);
        //        d125 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='125'", "TBDLMJ") / 10000, 2);
        //        d126 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='126'", "TBDLMJ") / 10000, 2);
        //        d127 = MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "DLBM='127'", "TBDLMJ") / 10000, 2);

        //        d123 += MathHelper.Round(FeatureHelper.StatsFieldSumValue(dltb2d, "", "TKMJ") / 10000, 2);
        //        #endregion

        //        d01 = d011 + d012 + d013;
        //        d02 = d021 + d022 + d023;
        //        d03 = d031 + d032 + d033;
        //        d04 = d041 + d042 + d043;
        //        d20 = d201 + d202 + d203 + d204 + d205;

        //        d10 = d101 + d102 + d104 + d105 + d106 + d107;
        //        d11 = d111 + d112 + d113 + d114 + d115 + d116 + d117 + d118 + d119;
        //        d12 = d122 + d123 + d124 + d125 + d126 + d127;
        //        dhj =MathHelper.Round( d01 + d02 + d03 + d04 + d20 + d10 + d11 + d12,2);

        //        double d3_00 = d115 + d116 + d125;
        //        double d3_05 = d201 + d202;
        //        double d3_06 = d204;
        //        double d3_07 = d203;
        //        double d3_09 = d205;
        //        double d3_11 = d111 + d112 + d113 + d114 + d117 + d118 + d119;
        //        double d3_12 = d122 + d123 + d124 + d126 + d127;

        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("insert into BG_PHb_23(XZDM,DLH,d00, d1105, d1106, d1108, d01, d0101, d0102, d0103, d02, d0201, d0202, d0204,")
        //            .Append(" d03, d0301, d0305, d0307, d04, d0401, d0403, d0404, d05,  d06, ")
        //            .Append(" d07, d09, d10, d1001,  d1003, d1006, d1007, d1008, d1009, ")
        //            .Append(" d11, d1101, d1102, d1103, d1104, d1107,  d1109, d1110, d12, d1202, d1203, d1204, d1205, d1206,D000 ) values('")
        //            .Append(xcode).Append("','D003',").Append(d3_00).Append(",").Append(d115).Append(",").Append(d116).Append(",").Append(d125).Append(",")
        //            .Append(d01).Append(",").Append(d011).Append(",").Append(d012).Append(",").Append(d013).Append(",")
        //            .Append(d02).Append(",").Append(d021).Append(",").Append(d022).Append(",").Append(d023).Append(",")
        //            .Append(d03).Append(",").Append(d031).Append(",").Append(d032).Append(",").Append(d033).Append(",")
        //            .Append(d04).Append(",").Append(d041).Append(",").Append(d042).Append(",").Append(d043).Append(",")
        //            .Append(d3_05).Append(",").Append(d3_06).Append(",").Append(d3_07).Append(",").Append(d3_09).Append(",")
        //            .Append(d10).Append(",").Append(d101).Append(",").Append(d102).Append(",").Append(d104).Append(",").Append(d105).Append(",").Append(d106).Append(",").Append(d107).Append(",")
        //            .Append(d3_11).Append(",").Append(d111).Append(",").Append(d112).Append(",").Append(d113).Append(",").Append(d114).Append(",").Append(d117).Append(",").Append(d118).Append(",")
        //            .Append(d119).Append(",")
        //            .Append(d3_12).Append(",").Append(d122).Append(",").Append(d123).Append(",").Append(d124).Append(",").Append(d126).Append(",").Append(d127).Append(",")
        //            .Append(dhj).Append(") ");
        //        string sql = sb.ToString();
        //        int iret = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
        //    }
        //    catch { }

        //}

        #endregion 

        private DataTable GetSumByDatatable(DataTable dt, string groupField, string statisticsField)
        {
            var query = dt.AsEnumerable().GroupBy(t => t.Field<string>(groupField))
                      .Select(g => new
                      {
                          c1 = g.Key,
                          c3 = g.Sum(m => Convert.ToDouble(m[statisticsField]))
                      });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add(groupField);
            dtResult.Columns.Add(new DataColumn(statisticsField, typeof(float)));
            query.ToList().ForEach(q => dtResult.Rows.Add(q.c1, q.c3));
            return dtResult;
        }

        private void BgbIn2D(string xcode)
        {
            string sql = "Select xzdm from BG_YLB where (xzdm like '" + xcode + "%' )  group by xzdm";
            DataTable dttemp = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
            int iRet;
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                string dwdm = dttemp.Rows[i][0].ToString();
                Console.WriteLine(dwdm);
                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(string.Format("Insert Into BG_Phb_23 (xzdm,DLH) Values ('{0}','D001')", dwdm));
                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(string.Format("Insert Into BG_Phb_23 (xzdm,DLH) Values ('{0}','D005')", dwdm));
                DataTable dt0 = LS_LlfxMDBHelper.GetDataTable(string.Format("Select dlbm,sum(tbdlmj) as dlmj from dltbgxgc where zldwdm_1 = '{0}' Group By dlbm", dwdm), "tmp");
                sql = string.Format(@"select dlbm, sum(dlmj) as mj from (
                                Select dlbm,sum(tbdlmj) as dlmj from dltbgxgc where zldwdm_1 = '{0}' Group By dlbm
                                union all 
                                select dlbm_12 as dlbm,sum(mj) as dlmj from dltbxzdw where zldwdm_1 = '{0}' Group By dlbm_12 
                                union all
                                Select '123' as dlbm, sum(TKMJ) as dlmj From dltbgxgc where zldwdm_1 = '{0}') group by dlbm order by dlbm", dwdm);
                DataTable dt1 = LS_LlfxMDBHelper.GetDataTable(string.Format("select dlbm_12 as dlbm,sum(mj) as dlmj from dltbxzdw where zldwdm_1 = '{0}' Group By dlbm_12", dwdm), "tmp");
                DataTable dt2 = LS_LlfxMDBHelper.GetDataTable(string.Format("Select '123' as dlbm, sum(TKMJ) as dlmj From dltbgxgc where zldwdm_1 = '{0}'", dwdm), "tmp");
                dt0.Merge(dt1);
                dt0.Merge(dt2);
                DataTable dt = GetSumByDatatable(dt0, dt0.Columns[0].ColumnName, dt0.Columns[1].ColumnName); ;
                DataTable subDt;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string dldm = dt.Rows[j][0].ToString();
                    double mj = double.Parse(dt.Rows[j][1].ToString());
                    switch (dldm)
                    {
                        case "011":
                            sql = string.Format("Update BG_Phb_23 Set D0101 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "012":
                            sql = string.Format("Update BG_Phb_23 Set D0102 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "013":
                            sql = string.Format("Update BG_Phb_23 Set D0103 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "021":
                            sql = string.Format("Update BG_Phb_23 Set D0201 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "022":
                            sql = string.Format("Update BG_Phb_23 Set D0202 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "023":
                             sql = string.Format("Update BG_Phb_23 Set D0204 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                             iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                            //sql = string.Format("select left(dlbm_1,4) as dlbm, sum(tbdlmj_1) from dltbgxgc Where zldwdm_1 = '{0}' and dlbm = '023' and (dlbm_1 = '0203' or dlbm_1 = '0204') group by dlbm_1 order by dlbm_1", dwdm);
                            //subDt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                            //if (subDt.Rows.Count == 0)
                            //{
                            //    sql = string.Format("Update BG_Phb_23 Set D0203 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            //    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            //}
                            //else if (subDt.Rows.Count == 1)
                            //{
                            //    sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                            //    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            //}
                            //else
                            //{
                            //    for (int m = 1; m < subDt.Rows.Count; m++)
                            //    {
                            //        double subMJ = double.Parse(subDt.Rows[m][1].ToString());
                            //        sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(subMJ / 10000, 2), dwdm, subDt.Rows[m][0].ToString());
                            //        iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            //        mj -= subMJ;
                            //    }
                            //    sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                            //    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            //}
                            //break;
                        case "031":
                            sql = string.Format("select left(dlbm_1,4) as dlbm, sum(tbdlmj_1) from dltbgxgc Where zldwdm_1 = '{0}' and dlbm = '031' and (dlbm_1 = '0301' or dlbm_1 = '0302' or dlbm_1 = '0303' or dlbm_1 = '0304') group by dlbm_1 order by dlbm_1", dwdm);
                            subDt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                            if (subDt.Rows.Count == 0)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D0301 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else if (subDt.Rows.Count == 1)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else
                            {
                                for (int m = 1; m < subDt.Rows.Count; m++)
                                {
                                    double subMJ = double.Parse(subDt.Rows[m][1].ToString());
                                    sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(subMJ / 10000, 2), dwdm, subDt.Rows[m][0].ToString());
                                    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                                    mj -= subMJ;
                                }
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            break;
                        case "032":
                            sql = string.Format("Update BG_Phb_23 Set D0305 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "033":
                            sql = string.Format("Update BG_Phb_23 Set D0307 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "041":
                            sql = string.Format("Update BG_Phb_23 Set D0401 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "042":
                            sql = string.Format("Update BG_Phb_23 Set D0403 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "043":
                            sql = string.Format("Update BG_Phb_23 Set D0404 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "101":
                            sql = string.Format("select left(dlbm_1,4) as dlbm, sum(tbdlmj_1) from dltbgxgc Where zldwdm_1 = '{0}' and dlbm = '101' and (dlbm_1 = '1001' or dlbm_1 = '1002') group by dlbm_1 order by dlbm_1", dwdm);
                            subDt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                            if (subDt.Rows.Count == 0)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D1001 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else if (subDt.Rows.Count == 1)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else
                            {
                                for (int m = 1; m < subDt.Rows.Count; m++)
                                {
                                    double subMJ = double.Parse(subDt.Rows[m][1].ToString());
                                    sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(subMJ / 10000, 2), dwdm, subDt.Rows[m][0].ToString());
                                    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                                    mj -= subMJ;
                                }
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            break;
                        case "102":
                            sql = string.Format("Update BG_Phb_23 Set D1003 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "103":
                            sql = string.Format("select left(dlbm_1,4) as dlbm, sum(tbdlmj_1) from dltbgxgc Where zldwdm_1 = '{0}' and dlbm = '103' and (dlbm_1 = '1004' or dlbm_1 = '1005') group by dlbm_1 order by dlbm_1", dwdm);
                            subDt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                            if (subDt.Rows.Count == 0)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D1004 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else if (subDt.Rows.Count == 1)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else
                            {
                                for (int m = 1; m < subDt.Rows.Count; m++)
                                {
                                    double subMJ = double.Parse(subDt.Rows[m][1].ToString());
                                    sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(subMJ / 10000, 2), dwdm, subDt.Rows[m][0].ToString());
                                    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                                    mj -= subMJ;
                                }
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            break;
                        case "104":
                            sql = string.Format("Update BG_Phb_23 Set D1006 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "105":
                            sql = string.Format("Update BG_Phb_23 Set D1007 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "106":
                            sql = string.Format("Update BG_Phb_23 Set D1008 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "107":
                            sql = string.Format("Update BG_Phb_23 Set D1009 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "111":
                            sql = string.Format("Update BG_Phb_23 Set D1101 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "112":
                            sql = string.Format("Update BG_Phb_23 Set D1102 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "113":
                            sql = string.Format("Update BG_Phb_23 Set D1103 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "114":
                            sql = string.Format("Update BG_Phb_23 Set D1104 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "115":
                            sql = string.Format("Update BG_Phb_23 Set D1105 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "116":
                            sql = string.Format("Update BG_Phb_23 Set D1106 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "117":
                            sql = string.Format("Update BG_Phb_23 Set D1107 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "125":
                            sql = string.Format("Update BG_Phb_23 Set D1108 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "118":
                            sql = string.Format("Update BG_Phb_23 Set D1109 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "119":
                            sql = string.Format("Update BG_Phb_23 Set D1110 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "121":
                            sql = string.Format("Update BG_Phb_23 Set D1201 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "122":
                            sql = string.Format("Update BG_Phb_23 Set D1202 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "123":
                            sql = string.Format("Update BG_Phb_23 Set D1203 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "124":
                            sql = string.Format("Update BG_Phb_23 Set D1204 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "126":
                            sql = string.Format("Update BG_Phb_23 Set D1205 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                            iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            break;
                        case "127":
                            sql = string.Format("select left(dlbm_1,4) as dlbm, sum(tbdlmj_1) from dltbgxgc Where zldwdm_1 = '{0}' and dlbm = '127' and (dlbm_1 = '1206' or dlbm_1 = '1207') group by dlbm_1 order by dlbm_1", dwdm);
                            subDt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                            if (subDt.Rows.Count == 0)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D1206 = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm);
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else if (subDt.Rows.Count == 1)
                            {
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            else
                            {
                                for (int m = 1; m < subDt.Rows.Count; m++)
                                {

                                    double subMJ = 0;
                                    double.TryParse(subDt.Rows[m][1].ToString(), out subMJ);
                                    sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(subMJ / 10000, 2), dwdm, subDt.Rows[m][0].ToString());
                                    iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                                    mj -= subMJ;
                                }
                                sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", Math.Round(mj / 10000, 2), dwdm, subDt.Rows[0][0].ToString());
                                iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                            }
                            break;
                        default:
                            break;
                    }
                }
                //sql = string.Format("select left(dlbm_1,4) as dlbm, sum(tbdlmj) from dltbgxgc Where zldwdm_1 = '{0}' and left(dlbm,2) = '20' group by dlbm_1 order by dlbm_1", dwdm);
                sql = string.Format("select xdl,sum(bgmj) as mj from BG_YLB where xzdm = '{0}' and left(ydl,2) = '20' group by xdl order by xdl", dwdm);
                subDt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                for (int m = 0; m < subDt.Rows.Count; m++)
                {
                    string dl = subDt.Rows[m][0].ToString();
                    if (dl.Substring(0, 2) == "05" || dl.Substring(0, 2) == "06" || dl.Substring(0, 2) == "07" || dl.Substring(0, 2) == "08" || dl.Substring(0, 2) == "09")
                    {
                        double subMJ = double.Parse(subDt.Rows[m][1].ToString());
                        sql = string.Format("Update BG_Phb_23 Set D{2} = {0} Where xzdm = '{1}' And DLH = 'D001'", subMJ, dwdm, dl);
                        iRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                    }
                }
            }
            
        }

        private void BuildBgb(string xcode)
        {
            string sql = "delete from BG_PHB_23 ";
            int ret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
            this.UpdateStatus("正在一览表入库汇总...");
            
            BgbInWare(xcode);

           
            this.UpdateStatus("正在计算小计合计...");
            //根据二级类 生成一级类的变更流向，此时同类别的也包括在里面
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into  BG_PHB_23(XZDM,dlh, D0101,D0102,D0103,D0201,D0202,D0203,D0204,")
                .Append("D0301,D0302,D0303,D0304,D0305,D0306,D0307,D0401,D0402,D0403,D0404,D05H1,D0508,D0601,D0602,D0603,")
                .Append("D0701,D0702,D08H1,D08H2,D0809,D0810,D09,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append("D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
                .Append("select xzdm,left(dlh,3),")
                .Append("sum(D0101),sum(D0102),sum(D0103),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
                .Append("sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05H1),sum(D0508),sum(D0601),sum(D0602),sum(D0603),")
                .Append("sum(D0701),sum(D0702),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
                .Append("sum(D1101),sum(d1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207)")
                .Append(" from BG_PHB_23    where xzdm like '").Append(xcode).Append("%'  ")
                .Append("group by xzdm,left(dlh,3) ");
            int bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sb.ToString());

            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D00 = iif(isnull(D0303),0,D0303) + iif(isnull(D0304),0,D0304) + iif(isnull(D0306),0,D0306) + iif(isnull(D0402),0,D0402) + iif(isnull(D0603),0,D0603) + iif(isnull(D1105),0,D1105) + iif(isnull(D1106),0,D1106) + iif(isnull(D1108),0,D1108)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D01 = iif(isnull(D0101),0,D0101) + iif(isnull(D0102),0,D0102) + iif(isnull(D0103),0,D0103)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D02 = iif(isnull(D0201),0,D0201) + iif(isnull(D0202),0,D0202) + iif(isnull(D0203),0,D0203) + iif(isnull(D0204),0,D0204)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D03 = iif(isnull(D0301),0,D0301) + iif(isnull(D0302),0,D0302) + iif(isnull(D0305),0,D0305) + iif(isnull(D0307),0,D0307)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D04 = iif(isnull(D0401),0,D0401) + iif(isnull(D0403),0,D0403) + iif(isnull(D0404),0,D0404)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D05 = iif(isnull(D05H1),0,D05H1) + iif(isnull(D0508),0,D0508)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D06 = iif(isnull(D0601),0,D0601) + iif(isnull(D0602),0,D0602)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D07 = iif(isnull(D0701),0,D0701) + iif(isnull(D0702),0,D0702)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D08 = iif(isnull(D08H1),0,D08H1) + iif(isnull(D08H2),0,D08H2) + iif(isnull(D0809),0,D0809) + iif(isnull(D0810),0,D0810)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D10 = iif(isnull(D1001),0,D1001) + iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,d1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D11 = iif(isnull(D1101),0,D1101) + iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D12 = iif(isnull(D1201),0,D1201) + iif(isnull(D1202),0,D1202) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D20 = iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)");

            sb = new StringBuilder();

            //把dl值为 3级分类的数据的年内减少合计 算出来
            sb = new StringBuilder();
            sb.Append(" update BG_PHB_23 set D000=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
            .Append(" iif(isnull(D0201),0,D0201)+iif(isnull(D0202),0,D0202)+iif(isnull(D0203),0,D0203)+iif(isnull(D0204),0,D0204)+  ")
            .Append(" iif(isnull(D0301),0,D0301)+iif(isnull(D0302),0,D0302)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+ ")
            .Append(" iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0404),0,D0404)+iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+ ")
            .Append(" iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+ ")
            .Append(" iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D09),0,D09)+")
            .Append(" iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
            .Append(" iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+iif(isnull(D1104),0,D1104)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
            .Append(" iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) ")
            .Append(" where xzdm like '").Append(xcode).Append("%' And DLH <> 'D003'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sb.ToString());

            //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("insert into BG_PHB_23(XZDM,DLH) values('" + xcode + "','D001') ");
            
            //插入三调面积
           // in3dmj(xzdm); //002

            //插入二调面积，从shpdltb 直接统计得到每一个分地类的面积
          //  in2dmj(xzdm);
            //003
           

            PutNNZJ2Bgb(xcode);
            PutNNJS2Bgb(xcode); //年内减少
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D000 = 0 Where DLH = 'D002' Or  DLH = 'D003'");
            //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D000 = iif(isnull(D00),0,D00) + iif(isnull(D01),0,D01) + iif(isnull(D02),0,D02) + iif(isnull(D03),0,D03) + iif(isnull(D04),0,D04) + iif(isnull(D20),0,D20) + iif(isnull(D10),0,D10) + iif(isnull(D11),0,D11) + iif(isnull(D12),0,D12) Where DLH = 'D002'");
            //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D000 = iif(isnull(D00),0,D00) + iif(isnull(D01),0,D01) + iif(isnull(D02),0,D02) + iif(isnull(D03),0,D03) + iif(isnull(D04),0,D04) + iif(isnull(D20),0,D20) + iif(isnull(D10),0,D10) + iif(isnull(D11),0,D11) + iif(isnull(D12),0,D12) Where DLH = 'D003'");

            PutNMMj(xzdm);

            BgbIn2D(xcode);
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D00 = iif(isnull(D0303),0,D0303) + iif(isnull(D0304),0,D0304) + iif(isnull(D0306),0,D0306) + iif(isnull(D0402),0,D0402) + iif(isnull(D0603),0,D0603) + iif(isnull(D1105),0,D1105) + iif(isnull(D1106),0,D1106) + iif(isnull(D1108),0,D1108) Where DLH = 'D001' ");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D01 = iif(isnull(D0101),0,D0101) + iif(isnull(D0102),0,D0102) + iif(isnull(D0103),0,D0103) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D02 = iif(isnull(D0201),0,D0201) + iif(isnull(D0202),0,D0202) + iif(isnull(D0203),0,D0203) + iif(isnull(D0204),0,D0204) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D03 = iif(isnull(D0301),0,D0301) + iif(isnull(D0302),0,D0302) + iif(isnull(D0305),0,D0305) + iif(isnull(D0307),0,D0307) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D04 = iif(isnull(D0401),0,D0401) + iif(isnull(D0403),0,D0403) + iif(isnull(D0404),0,D0404) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D05 = iif(isnull(D05H1),0,D05H1) + iif(isnull(D0508),0,D0508) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D06 = iif(isnull(D0601),0,D0601) + iif(isnull(D0602),0,D0602) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D07 = iif(isnull(D0701),0,D0701) + iif(isnull(D0702),0,D0702) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D08 = iif(isnull(D08H1),0,D08H1) + iif(isnull(D08H2),0,D08H2) + iif(isnull(D0809),0,D0809) + iif(isnull(D0810),0,D0810) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D10 = iif(isnull(D1001),0,D1001) + iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,d1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D11 = iif(isnull(D1101),0,D1101) + iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D12 = iif(isnull(D1201),0,D1201) + iif(isnull(D1202),0,D1202) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207) Where DLH = 'D001'");
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D20 = iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09) Where DLH = 'D001'");
            
            bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("update BG_PHB_23 as b,BGSort as c set b.sort=c.sort1 where b.dlh=c.dlh "); //修改显示次序
           

        }
        //三调减二调
        private void PutNMMj(string scode)
        {
            string[] alldl = new string[]{"D00","D01", "D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
                "D03", "D0301","D0302","D0303","D0304","D0305","D0306","D0307","D04","D0401","D0402","D0403","D0404","D20","D05","D05H1","D0508", "D06","D0601","D0602","D0603",
                "D07","D0701","D0702","D08","D08H1","D08H2","D0809","D0810","D09","D10","D1001","D1002","D1003","D1004","D1005","D1006","D1007","D1008","D1009",
                "D11","D1101","D1102","D1103","D1104","D1105","D1106","D1107","D1108","D1109","D1110","D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207" };

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into BG_PHB_23(xzdm,DLH,D00,D01, D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,")
                .Append("D03, D0301,D0302,D0303,D0304,D0305,D0306,D0307,D04,D0401,D0402,D0403,D0404,D20,D05,D05H1,D0508, D06,D0601,D0602,D0603,")
                .Append(" D07,D0701,D0702,D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D11,D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207,D000 )")
                .Append(" select b.XZDM,'D004',");
            foreach (string adl in alldl)
            {
                sb.Append(" iif(isnull(b." + adl + "),0,b." + adl + ")- iif(isnull(c." + adl + "),0,c." + adl + ") as " + adl + ",");
            }
            sb.Append("iif(isnull(b.D000),0,b.D000) - iif(isnull(c.D000),0,c.D000) as D000 ")
            .Append(" from  BG_PHB_23   b   inner join BG_PHB_23 c on b.xzdm=c.xzdm  ")
            .Append(" where   b.dlh='D003' and c.dlh='D002'  ");
            string sql = sb.ToString();
            int iret = LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
        }

        //年内减少
        private void PutNNJS2Bgb(string scode)
        {
            string sql = "Select xzdm from BG_YLB where (xzdm like '" + scode + "%' )  group by xzdm";
            DataTable dttemp = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
            string strsql;
            int iret;
            //取 二调减少地类的和 ，放在二调减少这一行 
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                strsql = "insert into BG_PHB_23(xzdm,DLH) values ('" + dttemp.Rows[i][0].ToString() + "','D002' ) ";
                iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(strsql);
                //计算3级地类年内减少
                double d01, d011 = 0, d012 = 0, d013 = 0, d02, d021 = 0, d022 = 0, d023 = 0,
                            d03, d031 = 0, d032 = 0, d033 = 0, d04, d041 = 0, d042 = 0, d043 = 0, d20, d201 = 0, d202 = 0, d203 = 0, d204 = 0, d205 = 0,
                             d10, d101 = 0, d102 = 0,d103 = 0, d104 = 0, d105 = 0, d106 = 0, d107 = 0,
                            d11, d111 = 0, d112 = 0, d113 = 0, d114 = 0, d115 = 0, d116 = 0, d117 = 0, d118 = 0, d119 = 0,
                            d12, d121 = 0, d122 = 0, d123 = 0, d124 = 0, d125 = 0, d126 = 0, d127 = 0, dhj = 0;

                sql = " select * from BG_PHB_23 where (xzdm = '" + dttemp.Rows[i][0].ToString() + "')  and (DLH not in ('D001','D002','D003','D004','D005','D00','D01','D02','D03','D04','D20','D10','D11','D12') )  order by DLH ";
                System.Data.DataTable dt = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
                foreach (DataRow dr in dt.Rows)
                {
                    string dlh = dr["DLH"].ToString().Trim();
                    double mj = 0;
                    double.TryParse(dr["D000"].ToString(), out mj);
                    mj = MathHelper.Round(mj, 2);
                    #region 获取面积
                    switch (dlh.ToUpper())
                    {
                        case "D011":
                            d011 = mj;
                            break;
                        case "D012":
                            d012 = mj;
                            break;
                        case "D013":
                            d013 = mj;
                            break;
                        case "D021":
                            d021 = mj;
                            break;
                        case "D022":
                            d022 = mj;
                            break;
                        case "D023":
                            d023 = mj;
                            break;
                        case "D031":
                            d031 = mj;
                            break;
                        case "D032":
                            d032 = mj;
                            break;
                        case "D033":
                            d033 = mj;
                            break;
                        case "D041":
                            d041 = mj;
                            break;
                        case "D042":
                            d042 = mj;
                            break;
                        case "D043":
                            d043 = mj;
                            break;
                        case "D201":
                            d201 = mj;
                            break;
                        case "D202":
                            d202 = mj;
                            break;
                        case "D203":
                            d203 = mj;
                            break;
                        case "D204":
                            d204 = mj;
                            break;
                        case "D205":
                            d205 = mj;
                            break;
                        case "D101":
                            d101 = mj;
                            break;
                        case "D102":
                            d102 = mj;
                            break;
                        case "D103":
                            d103 = mj;
                            break;
                        case "D104":
                            d104 = mj;
                            break;
                        case "D105":
                            d105 = mj;
                            break;
                        case "D106":
                            d106 = mj;
                            break;
                        case "D107":
                            d107 = mj;
                            break;
                        case "D111":
                            d111 = mj;
                            break;
                        case "D112":
                            d112 = mj;
                            break;
                        case "D113":
                            d113 = mj;
                            break;
                        case "D114":
                            d114 = mj;
                            break;
                        case "D115":
                            d115 = mj;
                            break;
                        case "D116":
                            d116 = mj;
                            break;
                        case "D117":
                            d117 = mj;
                            break;
                        case "D118":
                            d118 = mj;
                            break;
                        case "D119":
                            d119 = mj;
                            break;
                        case "D121":
                            d121 = mj;
                            break;
                        case "D122":
                            d122 = mj;
                            break;
                        case "D123":
                            d123 = mj;
                            break;
                        case "D124":
                            d124 = mj;
                            break;
                        case "D125":
                            d125 = mj;
                            break;
                        case "D126":
                            d126 = mj;
                            break;
                        case "D127":
                            d127 = mj;
                            break;
                        default:
                            MessageBox.Show("kk");
                            break;
                    }
                    #endregion


                }
                //DataRow drsum;
                //string xzqdm = dttemp.Rows[i][0].ToString();
                //double c00 = 0;
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1106),0,D1106) + iif(isnull(D1108),0,D1108)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D115' ", "tmp");
                //c00 += double.Parse(drsum[0].ToString());
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1105),0,D1105) + iif(isnull(D1108),0,D1108)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D116' ", "tmp");
                //c00 += double.Parse(drsum[0].ToString());
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1105),0,D1105) + iif(isnull(D1106),0,D1106)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D125' ", "tmp");
                //c00 += double.Parse(drsum[0].ToString());

                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0102),0,D0102) + iif(isnull(D0103),0,D0103)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D011' ", "tmp");
                //    c01 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0101),0,D0101) + iif(isnull(D0103),0,D0103)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D012' ", "tmp");
                //    c01 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0102),0,D0102) + iif(isnull(D0101),0,D0101)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D013' ", "tmp");
                //    c01 += double.Parse(drsum[0].ToString());
                
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0202),0,D0202) + iif(isnull(D0203),0,D0203) + iif(isnull(D0204),0,D0204)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D021' ", "tmp");
                //    c02 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0201),0,D0201) + iif(isnull(D0203),0,D0203) + iif(isnull(D0204),0,D0204)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D022' ", "tmp");
                //    c02 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0202),0,D0202) + iif(isnull(D0203),0,D0203) + iif(isnull(D0201),0,D0201)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D023' ", "tmp");
                //    c02 += double.Parse(drsum[0].ToString());
                
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0302),0,D0302) + iif(isnull(D0305),0,D0305) + iif(isnull(D0307),0,D0307)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D031' ", "tmp");
                //    c03 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0302),0,D0302) + iif(isnull(D0301),0,D0301) + iif(isnull(D0307),0,D0307)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D032' ", "tmp");
                //    c03 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0302),0,D0302) + iif(isnull(D0305),0,D0305) + iif(isnull(D0301),0,D0301)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D033' ", "tmp");
                //    c03 += double.Parse(drsum[0].ToString());
                
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0403),0,D0403) + iif(isnull(D0404),0,D0404)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D041' ", "tmp");
                //    c04 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0401),0,D0401) + iif(isnull(D0404),0,D0404)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D042' ", "tmp");
                //    c04 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0403),0,D0403) + iif(isnull(D0401),0,D0401)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D043' ", "tmp");
                //    c04 += double.Parse(drsum[0].ToString());
                
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D101' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1001),0,D1001) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D102' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1001),0,D1001) + iif(isnull(D1003),0,D1003) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D103' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1001),0,D1001) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D104' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1001),0,D1001) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D105' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1001),0,D1001) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D106' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1001),0,D1001)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D107' ", "tmp");
                //    c10 += double.Parse(drsum[0].ToString());
                
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D111' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1101),0,D1101) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D112' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1101),0,D1101) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D113' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1101),0,D1101) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D114' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1101),0,D1101) + iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D115' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1101),0,D1101) + iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D116' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1101),0,D1101) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D117' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1101),0,D1101) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D118' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1101),0,D1101)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D119' ", "tmp");
                //    c11 += double.Parse(drsum[0].ToString());
                
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1202),0,D1202) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D121' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D122' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1202),0,D1202) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D123' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1202),0,D1202) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D124' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1202),0,D1202) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D125' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1202),0,D1202) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D126' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());
                //    drsum = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1202),0,D1202) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D127' ", "tmp");
                //    c12 += double.Parse(drsum[0].ToString());

                d01 = d011 + d012 + d013;
                d02 = d021 + d022 + d023;
                d03 = d031 + d032 + d033;
                d04 = d041 + d042 + d043;
                DataTable dt20 = LS_LlfxMDBHelper.GetDataTable("Select Sum(iif(isnull(D01),0,D01) + iif(isnull(D02),0,D02) + iif(isnull(D03),0,D03) + iif(isnull(D04),0,D04) + iif(isnull(D10),0,D10) + iif(isnull(D11),0,D11) + iif(isnull(D12),0,D12)) From BG_PHb_23 Where XZDM = '" + dttemp.Rows[i][0].ToString() + "' And DLH = 'D20'", "tmp");
                if (dt20.Rows.Count == 1)
                {
                    double.TryParse(dt20.Rows[0][0].ToString(), out d20);//d201 + d202 + d203 + d204 + d205;
                }
                else
                {
                    d20 = 0;
                }
                d10 = d101 + d102 + d103 + d104 + d105 + d106 + d107;
                d11 = d111 + d112 + d113 + d114 + d115 + d116 + d117 + d118 + d119;
                d12 = d121 + d122 + d123 + d124 + d125 + d126 + d127;
                //dhj = MathHelper.Round(c00+d01 + d02 + d03 + d04 + d20 + d10 + d11 + d12, 2);

                double d3_00 = d115 + d116 + d125;
                double d3_05 = 0;// d201 + d202;
                double d3_06 = 0;// d204;
                double d3_07 = 0;// d203;
                double d3_09 = d205;
                double d3_11 = d111 + d112 + d113 + d114 + d117 + d118 + d119;
                double d3_12 = d121 + d122 + d123 + d124 + d126 + d127;

                dhj = MathHelper.Round(d3_00 + d01 + d02 + d03 + d04 + d20 + d10 + d3_11 + d3_12 , 2);

                StringBuilder sb = new StringBuilder();
                sb.Append(" update  BG_PHB_23  set D00=").Append(d3_00).Append(",D1105=").Append(d115).Append(",D1106=").Append(d116)
                    .Append(",D1108=").Append(d125).Append(",D0101=").Append(d011).Append(",D0102=").Append(d012).Append(",D0103=").Append(d013)
                    .Append(",D0201=").Append(d021).Append(",D0202=").Append(d022).Append(",D0204=").Append(d023)
                    .Append(",D0301=").Append(d031).Append(",D0305=").Append(d032).Append(",D0307=").Append(d033)
                    .Append(",D0401=").Append(d041).Append(",D0403=").Append(d042).Append(",D0404=").Append(d043)
                    .Append(",D05=").Append(d3_05).Append(",D06=").Append(d3_06).Append(",D07=").Append(d3_07).Append(",D09=").Append(d3_09)
                    .Append(",D1001=").Append(d101).Append(",D1003=").Append(d102).Append(",D1006=").Append(d104).Append(",D1007=").Append(d105).Append(",D1008=").Append(d106).Append(",D1009=").Append(d107)
                    .Append(",D1101=").Append(d111).Append(",D1102=").Append(d112).Append(",D1103=").Append(d113).Append(",D1104=").Append(d114).Append(",D1107=").Append(d117).Append(",D1109=").Append(d118).Append(",D1110=").Append(d119)
                    .Append(",D1202=").Append(d122).Append(",D1203=").Append(d123).Append(",D1204=").Append(d124).Append(",D1205=").Append(d126).Append(",D1206=").Append(d127)
                    .Append(" where (xzdm = '" + dttemp.Rows[i][0].ToString() + "') and  (DLH='D002') ");//.Append(",D1005=").Append(d103).Append(",D1201=").Append(d121)
                strsql = sb.ToString();
                iret = LS_LlfxMDBHelper.ExecuteSQLNonquery(strsql);
                //StringBuilder sb = new StringBuilder();
                //sb.Append(" update  BG_PHB_23  set D00=").Append(d3_00).Append(",D1105=").Append(d115).Append(",D1106=").Append(d116)
                //    .Append(",D1108=").Append(d125).Append(",D01=").Append(d01).Append(",D0101=").Append(d011).Append(",D0102=").Append(d012).Append(",D0103=").Append(d013)
                //    .Append(",D02=").Append(d02).Append(",D0201=").Append(d021).Append(",D0202=").Append(d022).Append(",D0204=").Append(d023)
                //    .Append(",D03=").Append(d03).Append(",D0301=").Append(d031).Append(",D0305=").Append(d032).Append(",D0307=").Append(d033)
                //    .Append(",D04=").Append(d04).Append(",D0401=").Append(d041).Append(",D0403=").Append(d042).Append(",D0404=").Append(d043)
                //    .Append(",D05=").Append(d3_05).Append(",D06=").Append(d3_06).Append(",D07=").Append(d3_07).Append(",D09=").Append(d3_09)
                //    .Append(",D10=").Append(d10).Append(",D1001=").Append(d101).Append(",D1003=").Append(d102).Append(",D1005=").Append(d103).Append(",D1006=").Append(d104).Append(",D1007=").Append(d105).Append(",D1008=").Append(d106).Append(",D1009=").Append(d107)
                //    .Append(",D11=").Append(d3_11).Append(",D1101=").Append(d111).Append(",D1102=").Append(d112).Append(",D1103=").Append(d113).Append(",D1104=").Append(d114).Append(",D1107=").Append(d117).Append(",D1109=").Append(d118).Append(",D1110=").Append(d119)
                //    .Append(",D12=").Append(d3_12).Append(",D1201=").Append(d121).Append(",D1202=").Append(d122).Append(",D1203=").Append(d123).Append(",D1204=").Append(d124).Append(",D1205=").Append(d126).Append(",D1206=").Append(d127)
                //    .Append(",D20=").Append(d20)
                //    .Append(",D000=" + dhj + " where (xzdm = '" + dttemp.Rows[i][0].ToString() + "') and  (DLH='D002') ");
                //strsql = sb.ToString();
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery(strsql);
                ////double c01 = 0, c02 = 0, c03 = 0, c04 = 0, c10 = 0, c11 = 0, c12 = 0;
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D01 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D01'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D01=D01-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D02 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D02'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D02=D02-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D03 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D03'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D03=D03-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D04 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D04'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D04=D04-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D10 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D10'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D10=D10-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D11 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D11'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D11=D11-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D12 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D12'", "tmp");
                //iret = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D12=D12-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D002'");
            }
        }

        private void PutNNZJ2Bgb(string scode) //在变更表生成年内增加
        {
            string sql = "Select xzdm from BG_YLB where (xzdm like '" + scode + "%' )  group by xzdm";
            DataTable dttemp = LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                string[] xdl1 = new string[] { "D00", "D01", "D02", "D03", "D04", "D05", "D06", "D07", "D08", "D09", "D10", "D11", "D12" };
                //string[] ydl1 = new string[] { "D01", "D02", "D03", "D04", "D20", "D10", "D11", "D12" };

                //        //计算三级级分类的年内增加
                StringBuilder sb = new StringBuilder();
                sb.Append(" insert into  BG_PHB_23(XZDM,dlh, D0101,D0102,D0103,D0201,D0202,D0203,D0204,")
                    .Append(" D0301,D0302,D0303,D0304,D0305,D0306,D0307,D0401,D0402,D0403,D0404,D05H1,D0508,D0601,D0602,D0603,")
                    .Append(" D0701,D0702,D08H1,D08H2,D0809,D0810,D09,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                    .Append(" D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D1201,D1202,D1203,D1204,D1205,D1206,D1207,D000 ) ")
                .Append("  select xzdm,'D003',")
                .Append(" sum(D0101),sum(D0102),sum(D0103),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
                    .Append(" sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05H1),sum(D0508),sum(D0601),sum(D0602),sum(D0603),")
                    .Append(" sum(D0701),sum(D0702),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
                    .Append(" sum(D1101),sum(d1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207),sum(D000)  ")
                    .Append(" from BG_PHB_23 ")
                .Append("  where (xzdm = '").Append(dttemp.Rows[i][0].ToString()).Append("') and DLH not in ('D001','D002','D003','D004','D005','D01','D02','D03','D04','D20','D10','D11','D12')  ")
                .Append("  group by xzdm ");
                int bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sb.ToString());

                //计算小计合计



                //计算每一行的小计
                sb = new StringBuilder();
                sb.Append(" update BG_PHB_23  set ")
                    .Append("  D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
                    //.Append("  D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                    //.Append("  D02=iif(isnull(D0201),0,D0201)+iif(isnull(D0202),0,D0202)+iif(isnull(D0203),0,D0203)+iif(isnull(D0204),0,D0204), ")
                    //.Append("  D03=iif(isnull(D0301),0,D0301)+iif(isnull(D0302),0,D0302)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307),0,D0307),")
                    //.Append("  D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403),0,D0403)+iif(isnull(D0404),0,D0404),")
                    .Append("  D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                    .Append("  D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                    .Append("  D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                    .Append("  D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810) ")
                    //.Append("  D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                    //.Append("  D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+iif(isnull(D1104),0,D1104)+iif(isnull(D1107),0,D1107)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                    //.Append("  D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) ")
                    .Append("  where xzdm = '").Append(dttemp.Rows[i][0].ToString()).Append("'  and dlh='D003'  ");
                sql = sb.ToString();
                bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery(sb.ToString());

                
                //string xzqdm = dttemp.Rows[i][0].ToString();
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D20 = iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09) where xzdm = '" + xzqdm + "'  and dlh='D003' ");
                //string s;
                //DataTable dtsum = LS_LlfxMDBHelper.GetDataTable("Select * From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D003' ", "tmp");
                //DataRow dr;
                //string D01 = dtsum.Rows[0]["D01"].ToString();
                //if (!string.IsNullOrWhiteSpace(D01))
                //{
                //    double mj01 = double.Parse(D01);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0102),0,D0102) + iif(isnull(D0103),0,D0103)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D011' ", "tmp");
                //    mj01 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0101),0,D0101) + iif(isnull(D0103),0,D0103)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D012' ", "tmp");
                //    mj01 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0102),0,D0102) + iif(isnull(D0101),0,D0101)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D013' ", "tmp");
                //    mj01 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D01 = " + mj01 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}
                //string D00 = dtsum.Rows[0]["D00"].ToString();
                //if (!string.IsNullOrWhiteSpace(D00))
                //{
                //    double mj00 = double.Parse(D00);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1106),0,D1106) + iif(isnull(D1108),0,D1108)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D115' ", "tmp");
                //    mj00 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1105),0,D1105) + iif(isnull(D1108),0,D1108)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D116' ", "tmp");
                //    mj00 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1105),0,D1105) + iif(isnull(D1106),0,D1106)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D125' ", "tmp");
                //    mj00 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D00 = " + mj00 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}

                //double mj20 = 0;
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D01'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D02'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D03'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D04'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D10'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D11'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //dr = LS_LlfxMDBHelper.GetDataRow("Select Sum(iif(isnull(D05),0,D05) + iif(isnull(D06),0,D06) + iif(isnull(D07),0,D07) + iif(isnull(D08),0,D08) + iif(isnull(D09),0,D09)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D12'", "tmp");
                //mj20 += double.Parse(dr[0].ToString());
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D20 = " + mj20 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");

                //string D02 = dtsum.Rows[0]["D02"].ToString();
                //if (!string.IsNullOrWhiteSpace(D02))
                //{
                //    double mj02 = double.Parse(D02);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0202),0,D0202) + iif(isnull(D0203),0,D0203) + iif(isnull(D0204),0,D0204)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D021' ", "tmp");
                //    mj02 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0201),0,D0201) + iif(isnull(D0203),0,D0203) + iif(isnull(D0204),0,D0204)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D022' ", "tmp");
                //    mj02 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0202),0,D0202) + iif(isnull(D0203),0,D0203) + iif(isnull(D0201),0,D0201)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D023' ", "tmp");
                //    mj02 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D02 = " + mj02 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}
                //string D03 = dtsum.Rows[0]["D03"].ToString();
                //if (!string.IsNullOrWhiteSpace(D03))
                //{
                //    double mj03 = double.Parse(D03);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0302),0,D0302) + iif(isnull(D0305),0,D0305) + iif(isnull(D0307),0,D0307)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D031' ", "tmp");
                //    mj03 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0302),0,D0302) + iif(isnull(D0301),0,D0301) + iif(isnull(D0307),0,D0307)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D032' ", "tmp");
                //    mj03 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0302),0,D0302) + iif(isnull(D0305),0,D0305) + iif(isnull(D0301),0,D0301)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D033' ", "tmp");
                //    mj03 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D03 = " + mj03 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}
                //string D04 = dtsum.Rows[0]["D04"].ToString();
                //if (!string.IsNullOrWhiteSpace(D04))
                //{
                //    double mj04 = double.Parse(D04);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0403),0,D0403) + iif(isnull(D0404),0,D0404)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D041' ", "tmp");
                //    mj04 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0401),0,D0401) + iif(isnull(D0404),0,D0404)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D042' ", "tmp");
                //    mj04 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D0403),0,D0403) + iif(isnull(D0401),0,D0401)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D043' ", "tmp");
                //    mj04 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D04 = " + mj04 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}
                //string D10 = dtsum.Rows[0]["D10"].ToString();
                //if (!string.IsNullOrWhiteSpace(D10))
                //{
                //    double mj10 = double.Parse(D10);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D101' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1001),0,D1001) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D102' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1001),0,D1001) + iif(isnull(D1003),0,D1003) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D103' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1001),0,D1001) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D104' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1001),0,D1001) + iif(isnull(D1008),0,D1008) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D105' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1001),0,D1001) + iif(isnull(D1009),0,D1009)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D106' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1002),0,D1002) + iif(isnull(D1003),0,D1003) + iif(isnull(D1004),0,D1004) + iif(isnull(D1005),0,D1005) + iif(isnull(D1006),0,D1006) + iif(isnull(D1007),0,D1007) + iif(isnull(D1008),0,D1008) + iif(isnull(D1001),0,D1001)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D107' ", "tmp");
                //    mj10 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D10 = " + mj10 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}
                //string D11 = dtsum.Rows[0]["D11"].ToString();
                //if (!string.IsNullOrWhiteSpace(D11))
                //{
                //    double mj11 = double.Parse(D11);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D111' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1101),0,D1101) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D112' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1101),0,D1101) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D113' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1101),0,D1101) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D114' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1101),0,D1101) + iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D115' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1101),0,D1101) + iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D116' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1101),0,D1101) + iif(isnull(D1109),0,D1109) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D117' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1101),0,D1101) + iif(isnull(D1110),0,D1110)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D118' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1102),0,D1102) + iif(isnull(D1103),0,D1103) + iif(isnull(D1104),0,D1104) + iif(isnull(D1107),0,D1107) + iif(isnull(D1109),0,D1109) + iif(isnull(D1101),0,D1101)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D119' ", "tmp");
                //    mj11 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D11 = " + mj11 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}
                //string D12 = dtsum.Rows[0]["D12"].ToString();
                //if (!string.IsNullOrWhiteSpace(D12))
                //{
                //    double mj12 = double.Parse(D12);
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1202),0,D1202) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D121' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D122' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1202),0,D1202) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D123' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1202),0,D1202) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D124' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1202),0,D1202) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D125' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1202),0,D1202) + iif(isnull(D1206),0,D1206) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D126' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    dr = LS_LlfxMDBHelper.GetDataRow("Select sum(iif(isnull(D1201),0,D1201) + iif(isnull(D1203),0,D1203) + iif(isnull(D1204),0,D1204) + iif(isnull(D1205),0,D1205) + iif(isnull(D1202),0,D1202) + iif(isnull(D1207),0,D1207)) From BG_PHb_23 Where XZDM = '" + xzqdm + "' And DLH = 'D127' ", "tmp");
                //    mj12 -= double.Parse(dr[0].ToString());
                //    bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHb_23 Set D12 = " + mj12 + " Where XZDM = '" + xzqdm + "' And DLH = 'D003'");
                //}


                //DataRow drsum = LS_LlfxMDBHelper.GetDataRow("Select D01 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D01'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D01=D01-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D02 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D02'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D02=D02-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D03 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D03'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D03=D03-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D04 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D04'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D04=D04-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D10 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D10'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D10=D10-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D11 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D11'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D11=D11-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
                //drsum = LS_LlfxMDBHelper.GetDataRow("Select D12 From BG_PHB_23 Where xzdm = '" + xzqdm + "' and dlh = 'D12'", "tmp");
                //bRet = LS_LlfxMDBHelper.ExecuteSQLNonquery("Update BG_PHB_23 Set D12=D12-" + drsum[0].ToString() + " Where xzdm = '" + xzqdm + "' and dlh = 'D003'");
            }
        }

        /// <summary>
        /// 第一个是平方米合计 得到的公顷总面积，第二个是 每个分项算成公顷后累加得到的总面积
        /// </summary>
        /// <param name="zmj"></param>
        /// <param name="gqzmj"></param>
        private void gqTP(double zmj, double gqzmj)
        {
            System.Data.DataTable gqTable = LS_LlfxMDBHelper.GetDataTable("select xzdm,ydl,xdl,bgmj from BG_YLB order by bgmj desc ", "tmp");
            int iLen = gqTable.Rows.Count;

            double dDiff = MathHelper.RoundEx(Math.Abs(zmj - gqzmj), 2);
            int isum = (int)(dDiff / 0.01);
            int shang = isum / iLen; //商
            int yushu = isum % iLen; //余数

            if (zmj > gqzmj)
            {
                for (int i = 0; i < iLen; i++)
                {
                    System.Data.DataRow arow = gqTable.Rows[i];
                    string ydl = arow["ydl"].ToString();
                    string xdl = arow["xdl"].ToString();
                    string xzdm = arow["xzdm"].ToString();
                    double mj = 0;
                    double.TryParse(arow["bgmj"].ToString(), out mj);
                    if (i < yushu)
                    {
                        mj += 0.01 * (shang + 1);
                    }
                    else
                    {
                        mj += 0.01 * shang;
                    }
                    LS_LlfxMDBHelper.ExecuteSQLNonquery("update  BG_YLB set bgmj=" + mj + " where ydl='" + ydl + "' and xdl='" + xdl + "' and xzdm = '"+xzdm+"'");
                }
            }
            else if (zmj < gqzmj)
            {
                for (int i = 0; i < iLen; i++)
                {
                    System.Data.DataRow arow = gqTable.Rows[i];
                    string ydl = arow["ydl"].ToString();
                    string xdl = arow["xdl"].ToString();
                    string xzdm = arow["xzdm"].ToString();
                    double mj = 0;
                    double.TryParse(arow["bgmj"].ToString(), out mj);
                    if (i < yushu)
                    {
                        mj -= 0.01 * (shang + 1);
                    }
                    else
                    {
                        mj -= 0.01 * shang;
                    }
                    LS_LlfxMDBHelper.ExecuteSQLNonquery("update  BG_YLB set bgmj=" + mj + " where ydl='" + ydl + "' and xdl='" + xdl + "' and xzdm = '" + xzdm + "'");
                }
            }

        }


        private double getZmj(string tabName)
        {
            System.Data.DataRow arow = LS_LlfxMDBHelper.GetDataRow("select sum(bgmj) as zmj from " + tabName, "tmp");
            double mj = 0;
            if (arow != null)
            {
                string smj = arow["zmj"].ToString();
                double.TryParse(smj, out mj);
            }
            return mj;

        }


        private string xzdm = "";

        private string Import2Gxgc()
        {
            string err = "";
            string destShp = targetWs.PathName + "\\DLTBGXGC"; //目标shp路径
            string srcshp = this.currWs.PathName + "\\" + this.llfxName;
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass shp2shp = new ESRI.ArcGIS.ConversionTools.FeatureClassToFeatureClass();
            shp2shp.in_features = srcshp;
            shp2shp.out_path = targetWs.PathName;
            shp2shp.out_name = "DLTBGXGC";
            err = RunTool(gp, shp2shp, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return err.ToString();
            }
            else return "";


        }

        private void PatchCalTbmjnoXZDW(IFeatureClass dltbGxgcClass)
        {


            bool weStartedEditing = this.StartEditOp();

            try
            {
                using (ESRI.ArcGIS.ADF.ComReleaser comrelease = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor updateCursor = dltbGxgcClass.Update(null, true);
                    comrelease.ManageLifetime(updateCursor);
                    IFeature aFea = null;
                    SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                    string bgjlbh = xzdm + DateTime.Now.ToString("yyyyMMdd").PadLeft(10, '0');

                    while ((aFea = updateCursor.NextFeature()) != null)
                    {
                        //Console.WriteLine("图斑：" + aFea.OID); 
                        //ESRI.ArcGIS.Geometry.IPoint selectPoint = (aFea.ShapeCopy as IArea).Centroid;
                        //double X = selectPoint.X;
                        //int currDh = (int)(X / 1000000);////WK---带号  
                        if (aFea.OID % 100000 == 0) this.UpdateStatus("已经计算" + aFea.OID / 10000 + "万条数据");
                        //double tbmj = area.SphereArea(aFea.ShapeCopy, currDh);
                        double tbmj = MathHelper.Round((aFea.Shape as IArea).Area, 2);
                        FeatureHelper.SetFeatureValue(aFea, "TBMJ_1", tbmj);
                        double kcxs = FeatureHelper.GetFeatureDoubleValue(aFea, "KCXS");
                        double tkxs = chkTKXS.Checked ? FeatureHelper.GetFeatureDoubleValue(aFea, "TKXS") / 100 : FeatureHelper.GetFeatureDoubleValue(aFea, "TKXS");
                        double kcmj = 0;

                        string ydl = FeatureHelper.GetFeatureStringValue(aFea, "dlbm_1");
                        if (ydl.Length != 3)
                        {
                            ydl = FeatureHelper.GetFeatureStringValue(aFea, "dlbm");
                        }
                        else
                        {
                            FeatureHelper.SetFeatureValue(aFea, "dlbm", ydl);
                        }
                        string xdl = FeatureHelper.GetFeatureStringValue(aFea, "dlbm_12");
                        FeatureHelper.SetFeatureValue(aFea, "dlbm_1", xdl.Length > 4 ? xdl.Substring(0,4) : xdl);
                        string yzldm = FeatureHelper.GetFeatureStringValue(aFea, "zldwdm");
                        string xzldm = FeatureHelper.GetFeatureStringValue(aFea, "zldwdm_1");
                        string ytbbh = FeatureHelper.GetFeatureStringValue(aFea, "tbbh");
                        string xtbbh = FeatureHelper.GetFeatureStringValue(aFea, "tbbh_1");
                        string yqsxz = FeatureHelper.GetFeatureStringValue(aFea, "qsxz");
                        string xqsxz = FeatureHelper.GetFeatureStringValue(aFea, "qsxz_12");
                        FeatureHelper.SetFeatureValue(aFea, "qsxz_1", xqsxz);
                        if (kcxs > 0)
                        {
                            kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                            FeatureHelper.SetFeatureValue(aFea, "KCMJ", kcmj);

                        }
                        double tkmj = 0;
                        if (tkxs > 0)
                        {
                            tkmj = MathHelper.RoundEx(tbmj * tkxs, 2);
                            FeatureHelper.SetFeatureValue(aFea, "TKMJ", tkmj);
                            if (chkTKXS.Checked) FeatureHelper.SetFeatureValue(aFea, "TKXS", tkxs);
                        }
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ", MathHelper.RoundEx(tbmj - tkmj, 2));
                        //地类面积
                        double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ_1", dlmj);
                        updateCursor.UpdateFeature(aFea);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.StopEditOp(true); //保存              
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }
        

        private void PatchCalTbmj(IFeatureClass dltbGxgcClass)
        {
          
        
            bool weStartedEditing = this.StartEditOp();
            
            try
            {
                using (ESRI.ArcGIS.ADF.ComReleaser comrelease = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor updateCursor = dltbGxgcClass.Update(null, true);
                    comrelease.ManageLifetime(updateCursor);
                    IFeature aFea = null;
                    SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                    string bgjlbh = xzdm + DateTime.Now.ToString("yyyyMMdd").PadLeft(10, '0');

                    while ((aFea = updateCursor.NextFeature()) != null)
                    {
                        if (aFea.OID % 100000 == 0) this.UpdateStatus("已经计算" + aFea.OID / 10000 + "万条数据");
                        Console.WriteLine("图斑：" + aFea.OID); 
                        //ESRI.ArcGIS.Geometry.IPoint selectPoint = (aFea.ShapeCopy as IArea).Centroid;
                        //double X = selectPoint.X;
                        //int currDh = (int)(X / 1000000);////WK---带号  

                        //double tbmj = area.SphereArea(aFea.ShapeCopy, currDh);
                        double tbmj = MathHelper.Round((aFea.Shape as IArea).Area, 2);
                        FeatureHelper.SetFeatureValue(aFea, "TBMJ_1", tbmj);
                        double kcxs =  FeatureHelper.GetFeatureDoubleValue(aFea, "KCXS");
                        double tkxs = chkTKXS.Checked ? FeatureHelper.GetFeatureDoubleValue(aFea, "TKXS") / 100 : FeatureHelper.GetFeatureDoubleValue(aFea, "TKXS");
                        double kcmj = 0;

                        string ydl;// = FeatureHelper.GetFeatureStringValue(aFea, "dlbm");
                        string xdl = FeatureHelper.GetFeatureStringValue(aFea, "dlbm_1");
                        string yzldm = FeatureHelper.GetFeatureStringValue(aFea, "zldwdm");
                        string xzldm = FeatureHelper.GetFeatureStringValue(aFea, "zldwdm_1");
                        string ytbbh = FeatureHelper.GetFeatureStringValue(aFea, "tbbh");
                        string xtbbh = FeatureHelper.GetFeatureStringValue(aFea, "tbbh_1");
                        string yqsxz = FeatureHelper.GetFeatureStringValue(aFea, "qsxz");
                        string xqsxz = FeatureHelper.GetFeatureStringValue(aFea, "qsxz_1");

                        if (kcxs > 0)
                        {
                            kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                            FeatureHelper.SetFeatureValue(aFea, "KCMJ", kcmj);
                            
                        }
                        double tkmj = 0;
                        if (tkxs > 0)
                        {
                            tkmj = MathHelper.RoundEx(tbmj * tkxs, 2);
                            FeatureHelper.SetFeatureValue(aFea, "TKMJ", tkmj);
                            if (chkTKXS.Checked) FeatureHelper.SetFeatureValue(aFea, "TKXS", tkxs);
                        }
                        if (pIntersectXZDW != null)
                        {
                            IGeometry pGeo = aFea.ShapeCopy;
                            ITopologicalOperator pTop = pGeo as ITopologicalOperator;
                            IPolyline pLine = pTop.Boundary as IPolyline;
                            ITopologicalOperator pRel = pLine as ITopologicalOperator;
                            ISpatialFilter pSF = new SpatialFilterClass();
                            pSF.Geometry = pGeo;
                            pSF.SubFields = "SHAPE,DLBM,KD";
                            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            using (ESRI.ArcGIS.ADF.ComReleaser xzdwReleaser = new ESRI.ArcGIS.ADF.ComReleaser())
                            {
                                IFeatureCursor pFCursor = pIntersectXZDW.Search(pSF, true);
                                xzdwReleaser.ManageLifetime(pFCursor);
                                IFeature pF;
                                while ((pF = pFCursor.NextFeature()) != null)
                                {
                                    try
                                    {
                                        //Console.WriteLine("xzdw:" + pF.OID);
                                        //IPolyline pxzdw = pF.ShapeCopy as IPolyline;
                                        //ydl = FeatureHelper.GetFeatureStringValue(pF, "dlbm");
                                        //double kd = FeatureHelper.GetFeatureDoubleValue(pF, "KD");
                                        //double cd = pxzdw.Length;
                                        //double xmj = cd * kd;
                                        //tbmj -= xmj;
                                        //StringBuilder sb = new StringBuilder();
                                        //sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ)");
                                        //sb.Append(" Values ('" + (string.IsNullOrWhiteSpace(xzldm) ? xzdm : xzldm) + "','" + bgjlbh + "','" + ydl + "','" + xdl + "'," + xmj + ",'" + yzldm + "','" + xzldm + "','" + ytbbh +
                                        //    "','" + xtbbh + "','" + yqsxz + "','" + xqsxz + "')  ");
                                        //string sql = sb.ToString();
                                        //int iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                                        IPolyline pIntersectLine = pTop.Intersect(pF.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                                        if (pIntersectLine != null)
                                        {
                                            ydl = FeatureHelper.GetFeatureStringValue(pF, "dlbm");
                                            double kd = FeatureHelper.GetFeatureDoubleValue(pF, "KD");
                                            double cd = pIntersectLine.Length;
                                            //Console.WriteLine(pF.OID);
                                            double xmj = cd * kd;
                                            IPolyline pll = pRel.Intersect(pIntersectLine, esriGeometryDimension.esriGeometry1Dimension) as IPolyline;
                                            if (pll != null && pll.Length > 0) xmj = xmj / 2;
                                            if (xmj > 0)
                                            {
                                                tbmj -= xmj;
                                                StringBuilder sb = new StringBuilder();
                                                sb.Append("insert into BG_YLB_PFM(xzdm,JLBH,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ)");
                                                sb.Append(" Values ('" + (string.IsNullOrWhiteSpace(xzldm) ? xzdm : xzldm) + "','" + bgjlbh + "','" + ydl + "','" + xdl + "'," + xmj + ",'" + yzldm + "','" + xzldm + "','" + ytbbh +
                                                    "','" + xtbbh + "','" + yqsxz + "','" + xqsxz + "')  ");
                                                string sql = sb.ToString();
                                                int iret = RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery(sql);
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    { }
                                }
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFCursor);
                            }
                        }
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ", MathHelper.RoundEx(tbmj - tkmj, 2));
                        //地类面积
                        double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                        FeatureHelper.SetFeatureValue(aFea, "TBDLMJ_1", dlmj);
                        updateCursor.UpdateFeature(aFea);
                       
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.StopEditOp(true); //保存              
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        IFeatureClass pIntersectXZDW = null;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //计算
            if (this.beDltbHShp.Text.Trim() == "")
                return;
            if (this.cmbLayers.Text.Trim() == "")
                return;
            if (this.beXZDWHShp.Text.Trim() == "")
                return;
           
            string time = DateTime.Now.ToShortDateString();

            string bgqshp = "";
            if (rdoXZDW.SelectedIndex == 0)
            {
                bgqshp = this.beDltbHShp.Text;
            }
            else
            {
                bgqshp = XZDW2DLTB();
                if (bgqshp == "")
                {
                    this.UpdateStatus("转换失败");
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            //开始二调三调叠加分析
            string dltbName = OtherHelper.GetLeftName(this.cmbLayers.Text);
             
            this.Cursor = Cursors.WaitCursor;

            try
            {
                this.UpdateStatus("开始执行叠加分析...");
                try
                {
                    dltbGxgcClass = (this.targetWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
                    (dltbGxgcClass as IDataset).Delete();                   
                }
                catch {                   
                }
                try
                {

                    IFeatureClass llfxClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(this.llfxName);
                    (llfxClass as IDataset).Delete();
                }
                catch { }
                

                #region 叠加后的shp
                string bghshp = this.currWs.PathName + "\\"+RCIS.Global.AppParameters.DATASET_DEFAULT_NAME+"\\"+dltbName;

                string err = "";
                //此时生成在 当前三调数据中
                dltbGxgcClass = this.GetIntersectClassInGDB(bgqshp, bghshp,ref err);
                pIntersectXZDW = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(beXZDWHShp.Text);// this.GetIntersectClassInGDB(beXZDWHShp.Text);
                if (dltbGxgcClass == null)
                {
                    MessageBox.Show("没有分析结果！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.UpdateStatus(err);
                    this.Cursor = Cursors.Default;
                    return;
                }
                //叠加后为空
                if (dltbGxgcClass.FeatureCount(null) == 0)
                {
                    this.UpdateStatus("叠加后没有数据");
                    MessageBox.Show("叠加后没有数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Cursor = Cursors.Default;
                    return;
                }


                
                #endregion 
                #region 计算tbmj等
                RCIS.Database.LS_LlfxMDBHelper.ExecuteSQLNonquery("delete from BG_YLB_PFM");
                this.UpdateStatus("开始重新计算面积...");
                if (rdoXZDW.SelectedIndex == 0)
                {
                    this.PatchCalTbmj(dltbGxgcClass);
                }
                else
                {
                    this.PatchCalTbmjnoXZDW(dltbGxgcClass);
                }

                ITable pTable = dltbGxgcClass as ITable;
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = @"(DLBM = '115' And DLBM_1 = '1105')
                                             Or (DLBM = '116' And DLBM_1 = '1106')
                                             Or (DLBM = '125' And DLBM_1 = '1108')
                                             Or (DLBM = '011' And DLBM_1 = '0101')
                                             Or (DLBM = '012' And DLBM_1 = '0102')
                                             Or (DLBM = '013' And DLBM_1 = '0103')
                                             Or (DLBM = '021' And DLBM_1 = '0201')
                                             Or (DLBM = '022' And DLBM_1 = '0202')
                                             Or (DLBM = '023' And DLBM_1 = '0204')
                                             Or (DLBM = '031' And DLBM_1 = '0301')
                                             Or (DLBM = '032' And DLBM_1 = '0305')
                                             Or (DLBM = '033' And DLBM_1 = '0307')
                                             Or (DLBM = '041' And DLBM_1 = '0401')
                                             Or (DLBM = '042' And DLBM_1 = '0403')
                                             Or (DLBM = '043' And DLBM_1 = '0404')
                                             Or (DLBM = '101' And DLBM_1 = '1001')
                                             Or (DLBM = '102' And DLBM_1 = '1003')
                                             Or (DLBM = '104' And DLBM_1 = '1006')
                                             Or (DLBM = '105' And DLBM_1 = '1007')
                                             Or (DLBM = '106' And DLBM_1 = '1008')
                                             Or (DLBM = '107' And DLBM_1 = '1009')
                                             Or (DLBM = '111' And DLBM_1 = '1101')
                                             Or (DLBM = '112' And DLBM_1 = '1102')
                                             Or (DLBM = '113' And DLBM_1 = '1103')
                                             Or (DLBM = '114' And DLBM_1 = '1104')
                                             Or (DLBM = '117' And DLBM_1 = '1107')
                                             Or (DLBM = '118' And DLBM_1 = '1109')
                                             Or (DLBM = '119' And DLBM_1 = '1110')
                                             Or (DLBM = '122' And DLBM_1 = '1202')
                                             Or (DLBM = '123' And DLBM_1 = '1203')
                                             Or (DLBM = '124' And DLBM_1 = '1204')
                                             Or (DLBM = '126' And DLBM_1 = '1205')
                                             Or (DLBM = '127' And DLBM_1 = '1206')
                                             Or (DLBM = '205' And DLBM_1 = '09')
                                             Or (DLBM = '201' And DLBM_1 = '05H1')
                                             Or (DLBM = '201' And DLBM_1 = '0508')
                                             Or (DLBM = '202' And DLBM_1 = '05H1')
                                             Or (DLBM = '202' And DLBM_1 = '0508')
                                             Or (DLBM = '204' And DLBM_1 = '0601')
                                             Or (DLBM = '204' And DLBM_1 = '0602')
                                             Or (DLBM = '203' And DLBM_1 = '0701')
                                             Or (DLBM = '203' And DLBM_1 = '0702')";//Or (DLBM = '103' And DLBM_1 = '1004')Or (DLBM = '103' And DLBM_1 = '1005') Or (DLBM = '121' And DLBM_1 = '1201')
                pTable.DeleteSearchedRows(pQueryFilter);
                //计算完成后 将导入到llfx mdb中，方便写sql语句
                //IWorkspace tar = RCIS.GISCommon.WorkspaceHelper2.GetAccessWorkspace(System.Windows.Forms.Application.StartupPath + @"\SystemConf\llfx.mdb");
                System.Runtime.InteropServices.Marshal.ReleaseComObject(dltbGxgcClass); //要迁移到mdb中，释放调
                Boolean b = RCIS.GISCommon.EsriDatabaseHelper.CopyTable(this.currWs, targetWs, this.llfxName, "DLTBGXGC", null);
                if (!b)
                {
                    this.UpdateStatus("导入更新过程失败");
                    MessageBox.Show("导入更新过程失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Cursor = Cursors.Default;
                    return;
                }
                this.UpdateStatus("面积计算完成");

                #endregion 


                this.UpdateStatus("开始进计算流量...");
                CalBgbByGc( xzdm,true);

                this.UpdateStatus("正在进行平衡表计算...");
                BuildBgb(xzdm);

                this.Cursor = Cursors.Default;
                this.UpdateStatus("执行完毕");

                MessageBox.Show("执行完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.UpdateStatus(ex.Message);
                MessageBox.Show(ex.Message);
            }
            

        }

        private string XZDW2DLTB()
        {
            //#region 线状地物转面
            //bool isDotopInter = true;//处理图形压盖
            //bool isExtend = true;  //自动延伸吸附
            //double dExtentLen = 10; //延伸吸附的最大长度
            //bool isUnion = true;

            //IFeatureClass edDLTB = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(beDltbHShp.Text);
            //IFeatureClass edXZDW = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(beXZDWHShp.Text);
            //IWorkspace2 tempWork = RCIS.GISCommon.WorkspaceHelper2.GetShapefileWorkspace(Application.StartupPath + @"\\tmp") as IWorkspace2;
            //if (tempWork.NameExists[esriDatasetType.esriDTFeatureClass, "xzdw2dltb"])
            //{
            //    IFeatureClass tempFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(Application.StartupPath + @"\\tmp\\xzdw2dltb.shp");
            //    IDataset pD = tempFeatureClass as IDataset;
            //    pD.Delete();
            //}
            //IFeatureClass dltb_TClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(Application.StartupPath + @"\\tmp\\xzdw2dltb.shp", esriGeometryType.esriGeometryPolygon, (edDLTB as IGeoDataset).SpatialReference, edDLTB.Fields);
            //IField pField = new FieldClass();
            //IFieldEdit pFieldEdit = pField as IFieldEdit;
            //pFieldEdit.Name_2 = "XZDWKD";
            //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            //IClass pClass = dltb_TClass as IClass;
            //pClass.AddField(pField);
            //List<IFeature> lstXzdw = RCIS.GISCommon.GetFeaturesHelper.getFeaturesBySql(edXZDW, "");
            //this.UpdateStatus("线状地物转面");
            //int iCount = 0;
            //int emptyCount = 0;
            //foreach (IFeature aXzdw in lstXzdw)
            //{
            //    try
            //    {
            //        IGeometry aLine = aXzdw.ShapeCopy;
            //        if (aLine.IsEmpty)
            //        {
            //            continue;
            //        }
            //        double kd = FeatureHelper.GetFeatureDoubleValue(aXzdw, "KD");
            //        if (kd <= 0)
            //        {
            //            continue;
            //        }
            //        IPolygon aTbPolygon = null;
            //        IPolygon aTbPolygon1 = null; //半边
            //        IPolygon aTbPolygon2 = null;
            //        try
            //        {
            //            aTbPolygon1 = this.FlatBufferOneway(aLine as IPolyline, kd / 2, 1, isExtend, dExtentLen, edDLTB);
            //            aTbPolygon2 = this.FlatBufferOneway(aLine as IPolyline, kd / 2, -1, isExtend, dExtentLen, edDLTB);
            //            //查找与这两个面交于一点的地类图斑                        
            //            //合并
            //            object missing = Type.Missing;
            //            IGeometryBag pGeometryBag = new GeometryBag() as IGeometryBag;
            //            pGeometryBag.SpatialReference = aLine.SpatialReference;
            //            IGeometryCollection pGeometryCollection = pGeometryBag as IGeometryCollection;
            //            pGeometryCollection.AddGeometry(aTbPolygon1 as IGeometry, ref missing, ref missing);
            //            pGeometryCollection.AddGeometry(aTbPolygon2 as IGeometry, ref missing, ref missing);
            //            aTbPolygon = new PolygonClass();
            //            ITopologicalOperator pTopologicalOperator = aTbPolygon as ITopologicalOperator;
            //            pTopologicalOperator.ConstructUnion(pGeometryCollection as IEnumGeometry);
            //        }
            //        catch (Exception ex)
            //        {
            //        }

            //        #region 创建要素，赋值属性
            //        IFeature aNewDltb = dltb_TClass.CreateFeature();
            //        aNewDltb.Shape = aTbPolygon as IGeometry;
            //        string newDlbm = FeatureHelper.GetFeatureStringValue(aXzdw, "DLBM");
            //        string newQsdwdm = FeatureHelper.GetFeatureStringValue(aXzdw, "QSDWDM1");
            //        string newQsxz = FeatureHelper.GetFeatureStringValue(aXzdw, "QSXZ");
            //        FeatureHelper.SetFeatureValue(aNewDltb, "YSDM", "2001010100");
            //        FeatureHelper.SetFeatureValue(aNewDltb, "DLBM", newDlbm);
            //        FeatureHelper.SetFeatureValue(aNewDltb, "DLMC", FeatureHelper.GetFeatureStringValue(aXzdw, "DLMC"));
            //        //权属单位代码 ，坐落单位代码 暂时 赋值，后面还需要改动
            //        FeatureHelper.SetFeatureValue(aNewDltb, "QSDWDM", newQsdwdm);
            //        FeatureHelper.SetFeatureValue(aNewDltb, "QSDWMC", FeatureHelper.GetFeatureStringValue(aXzdw, "QSDWMC1"));
            //        FeatureHelper.SetFeatureValue(aNewDltb, "ZLDWDM", FeatureHelper.GetFeatureStringValue(aXzdw, "ZLDWDM1"));
            //        FeatureHelper.SetFeatureValue(aNewDltb, "QSXZ", newQsxz);
            //        FeatureHelper.SetFeatureValue(aNewDltb, "XZDWKD", kd);  //赋值宽度
            //        aNewDltb.Store();
            //        #endregion
            //        bool isNewGeoEmpty = false;
                    //if (isUnion && !isNewGeoEmpty)
                    //{
                    //    #region 合并
                    //    //获取相邻的
                    //    List<IFeature> delFeatures = new List<IFeature>();
                    //    //2017年11-20日修改，交与一个点的 ，不再合并
                    //    List<IFeature> arTouchedFeature = this.getInter1Features(aNewDltb, dltb_TClass,
                    //        //GetFeaturesByGeoHelper.getFeatures(dltbClass, aNewDltb.ShapeCopy, esriSpatialRelEnum.esriSpatialRelIntersects,
                    //        "DLBM='" + newDlbm + "' and QSXZ='" + newQsxz + "'   and XZDWKD=" + kd);
                    //    //GetFeaturesByGeoHelper.getFeaturesByGeo(dltbClass, aNewDltb.ShapeCopy, esriSpatialRelEnum.esriSpatialRelIntersects);
                    //    if (arTouchedFeature.Count > 0)
                    //    {
                    //        IGeometry newGeo = aNewDltb.ShapeCopy;
                    //        ITopologicalOperator pTop = aNewDltb.ShapeCopy as ITopologicalOperator;
                    //        foreach (IFeature aTouched in arTouchedFeature)
                    //        {
                    //            if (aTouched.OID == aNewDltb.OID)
                    //                continue;
                    //            newGeo = pTop.Union(aTouched.ShapeCopy);
                    //            pTop.Simplify();
                    //            pTop = newGeo as ITopologicalOperator;
                    //            delFeatures.Add(aTouched);
                    //        }
                    //        aNewDltb.Shape = newGeo;
                    //        aNewDltb.Store();
                    //    }

                    //    foreach (IFeature adel in delFeatures)
                    //    {
                    //        adel.Delete();
                    //    }
                    //    #endregion
                    //}

                    //if (isDotopInter && !isNewGeoEmpty)
                    //{
                    //    #region //此处处理图形压盖
                    //    List<IFeature> lstIntersecPolygonFeatures = this.GetInterPolygonFeatures(aNewDltb, dltb_TClass);  //交与一个面的
                    //    //切完了，就不要了
                    //    foreach (IFeature aInterDltb in lstIntersecPolygonFeatures)
                    //    {
                    //        if (aInterDltb.OID == aNewDltb.OID)
                    //            continue;
                    //        //判断地类，如果是铁路，公路，和农村道路相交，农村道路减去
                    //        //其他情况，再按宽度，宽路减去窄路，
                    //        //然后按先后次序减
                    //        string oldDlbm = FeatureHelper.GetFeatureStringValue(aInterDltb, "DLBM");
                    //        double oldKd = FeatureHelper.GetFeatureDoubleValue(aInterDltb, "XZDWKD"); //压盖图斑宽度
                    //        if (sys.YWCommonHelper.priorityDLBM2(newDlbm, oldDlbm))
                    //        {
                    //            #region  //如果新要素 地类优先于 旧的，则把 旧的删去相交部分
                    //            try
                    //            {
                    //                IGeometry oldGeo = this.GeometryDiffence(aInterDltb, aNewDltb);
                    //                if (oldGeo.IsEmpty)
                    //                {
                    //                    aInterDltb.Delete();
                    //                    emptyCount++;
                    //                }
                    //                else
                    //                {
                    //                    aInterDltb.Shape = oldGeo;
                    //                    aInterDltb.Store();
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //            }

                    //            #endregion
                    //        }
                    //        else if (kd > oldKd)
                    //        {
                    //            #region 宽度大的裁切宽度小的
                    //            try
                    //            {
                    //                IGeometry oldGeo = this.GeometryDiffence(aInterDltb, aNewDltb);
                    //                if (oldGeo.IsEmpty)
                    //                {
                    //                    aInterDltb.Delete();
                    //                    emptyCount++;
                    //                }
                    //                else
                    //                {
                    //                    aInterDltb.Shape = oldGeo;
                    //                    aInterDltb.Store();
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //            }
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            //否则 ，新图斑 切去 相交部分
                    //            IGeometry newDltbGeo = this.GeometryDiffence(aNewDltb, aInterDltb);
                    //            //此处还要判断一下是否为空，以为切割完了之后，图形就变了
                    //            if (newDltbGeo.IsEmpty)
                    //            {
                    //                isNewGeoEmpty = true; //表示新图斑 切除没了
                    //                break;
                    //            }
                    //            aNewDltb.Shape = newDltbGeo;
                    //            aNewDltb.Store();
                    //        }
                    //    }
                    //    if (isNewGeoEmpty)
                    //    {
                    //        //如果是空的，删除
                    //        aNewDltb.Delete();
                    //        emptyCount++;
                    //    }
                    //    #endregion
                    //}
            //        iCount++;
            //    }
            //    catch (Exception ex)
            //    {
            //        //RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
            //        //MessageBox.Show(ex.ToString());
            //    }
            //#endregion
            //}
            ////RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("XZDW2DLTB");

            this.UpdateStatus("线状地物转面");
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            try
            {
                ESRI.ArcGIS.AnalysisTools.Buffer pBuffer = new ESRI.ArcGIS.AnalysisTools.Buffer();

                pBuffer.in_features = beXZDWHShp.Text;
                pBuffer.out_feature_class = Application.StartupPath + @"\tmp\xzdw2dltb.shp";
                pBuffer.buffer_distance_or_field = "KD";
                pBuffer.line_side = "FULL";
                pBuffer.line_end_type = "ROUND";
                pBuffer.dissolve_option = "NONE";
                gp.Execute(pBuffer, null);
            }
            catch (Exception ex)
            {
                return "";
            }

            try
            {
                GPUpdate(beDltbHShp.Text, Application.StartupPath + @"\tmp\xzdw2dltb.shp", Application.StartupPath + @"\tmp\newdltb.shp");
            }
            catch (Exception ex)
            {
                return "";
            }

            try
            {
                ESRI.ArcGIS.DataManagementTools.DeleteIdentical pDel = new ESRI.ArcGIS.DataManagementTools.DeleteIdentical();
                pDel.in_dataset = Application.StartupPath + @"\tmp\newdltb.shp";
                pDel.fields = "shape";
                gp.Execute(pDel, null);
            }
            catch (Exception ex)
            {

            }

            this.UpdateStatus("转化完毕！");
            return Application.StartupPath + @"\tmp\newdltb.shp";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void OutYlb(string destFile)
        {
            string srcFile = RCIS.Global.AppParameters.TemplatePath + "\\二调三调变更一览表.xlsx";
            System.IO.File.Copy(srcFile, destFile, true);
            string sql = "select JLBH,XZDM,YDL,XDL,BGMJ,BGQZLDWDM,BGHZLDWDM,BGQTBBH,BGHTBBH,BGQQSXZ,BGHQSXZ,BGQPDFJ,BGHPDFJ,BGQGDLX,BGHGDLX FROM BG_YLB_PFM ";
            System.Data.DataTable dt = LS_LlfxMDBHelper.GetDataTable(sql, "ll");
            Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(destFile);
            Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
            Aspose.Cells.Cells cells = sheet.Cells;
            //边框和 数值 格式
            Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
            styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Number = 2;
            styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

            Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
            styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
            styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Number = 49; //@

            int startRow = 5;            
           
            try
            {
                #region 导出sheet1的数据
                cells[3,3].PutValue(this.xzdm);
                for (int iRow = startRow; iRow < startRow + dt.Rows.Count; iRow++)
                {
                    DataRow dr = dt.Rows[iRow - startRow];
                    cells[iRow, 2].PutValue(dr["JLBH"].ToString());
                    cells[iRow, 3].PutValue(dr["BGQZLDWDM"].ToString());
                    cells[iRow, 4].PutValue(dr["YDL"].ToString());
                    cells[iRow, 5].PutValue(dr["BGQTBBH"].ToString());
                    cells[iRow, 6].PutValue(dr["BGQQSXZ"].ToString());
                    cells[iRow, 7].PutValue(dr["BGQPDFJ"].ToString());
                    cells[iRow, 8].PutValue(dr["BGQGDLX"].ToString());
                    cells[iRow, 9].PutValue(dr["BGHZLDWDM"].ToString());
                    cells[iRow, 10].PutValue(dr["XDL"].ToString());
                    cells[iRow, 11].PutValue(dr["BGHTBBH"].ToString());
                    cells[iRow, 12].PutValue(dr["BGHQSXZ"].ToString());
                    cells[iRow, 13].PutValue(dr["BGHPDFJ"].ToString());
                    cells[iRow, 14].PutValue(dr["BGHGDLX"].ToString());

                    double mj = 0.00;
                    double.TryParse(dr["BGMJ"].ToString(), out mj);
                    cells[iRow, 15].PutValue(mj);

                    if (iRow % 1000 == 0)
                    {
                        UpdateStatus("已导出" + iRow + "条...");
                    }
                  
                }
                #endregion

                wk.Save(destFile);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                

            }


        }

        private void OutExcel(string destFile,string xzqdm)
        {
            string srcFile = RCIS.Global.AppParameters.TemplatePath + "\\二调三调流量分析表.xls";
            System.IO.File.Copy(srcFile, destFile,true);
            string sql = "select sum(D00),sum(D0303),sum(D0304),sum(D0306),sum(D0402),sum(D0603),sum(D1105),sum(D1106),sum(D1108),sum(D01),"
            + "sum(D0101),sum(D0102),sum(D0103),sum(D02),sum(D0201),sum(D0202),sum(D0203),sum(D0204),sum(D03),sum(D0301),"
            + "sum(D0302),sum(D0305),sum(D0307),sum(D04),sum(D0401),sum(D0403),sum(D0404),sum(D20),sum(D05),sum(D05H1),sum(D0508),sum(D06),sum(D0601),"
            + "sum(D0602),sum(D07),sum(D0701),sum(D0702),sum(D08),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),"
            + "sum(D10),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),"
            + "sum(D1009),sum( D11),sum(D1101),sum(d1102),sum(D1103),sum(D1104),sum(D1107),sum(D1109),sum(D1110),sum(D12),"
            + "sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207),sum(D000)  from BG_PHB_23 Where xzdm like '" + xzqdm + "%'"
                + "  group by left(xzdm," + xzqdm.Length + "),sort,dlh order by sort  ";
            System.Data.DataTable dt = RCIS.Database.LS_LlfxMDBHelper.GetDataTable(sql, "tmp");
            
            //var txtLicense = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "License.lic");
            //var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(txtLicense));
            //new Aspose.Cells.License().SetLicense(memoryStream);

            
            Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(destFile);
            //wk.
            Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
            Aspose.Cells.Cells cells = sheet.Cells;
            //边框和 数值 格式
            Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
            styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleNum.Number = 2;
            styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

            Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
            styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;
            styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            styleTxt.Number = 49; //@

            int startRow = 6;
            int startCol = 4;
            int endCol = 74;


            try
            {
                #region 导出sheet1的数据
                
                for (int iRow = startRow; iRow < startRow + dt.Rows.Count; iRow++)
                {
                    DataRow dr = dt.Rows[iRow - startRow];
                    for (int iCol = startCol; iCol < endCol; iCol++)
                    {
                        if (iRow == 11 && iCol == ToIndex("N")) continue;
                        if (iRow == 11 && iCol == ToIndex("O")) continue;
                        if (iRow == 11 && iCol == ToIndex("P")) continue;
                        if (iRow == 11 && iCol == ToIndex("Q")) continue;
                        if (iRow == 12 && iCol == ToIndex("N")) continue;
                        if (iRow == 12 && iCol == ToIndex("O")) continue;
                        if (iRow == 13 && iCol == ToIndex("N")) continue;
                        if (iRow == 13 && iCol == ToIndex("P")) continue;
                        if (iRow == 14 && iCol == ToIndex("N")) continue;
                        if (iRow == 14 && iCol == ToIndex("Q")) continue;
                        if (iRow == 15 && iCol == ToIndex("R")) continue;
                        if (iRow == 15 && iCol == ToIndex("S")) continue;
                        if (iRow == 15 && iCol == ToIndex("T")) continue;
                        if (iRow == 15 && iCol == ToIndex("U")) continue;
                        if (iRow == 15 && iCol == ToIndex("V")) continue;
                        if (iRow == 16 && iCol == ToIndex("R")) continue;
                        if (iRow == 16 && iCol == ToIndex("S")) continue;
                        if (iRow == 17 && iCol == ToIndex("N")) continue;
                        if (iRow == 17 && iCol == ToIndex("R")) continue;
                        if (iRow == 17 && iCol == ToIndex("T")) continue;
                        if (iRow == 18 && iCol == ToIndex("R")) continue;
                        if (iRow == 18 && iCol == ToIndex("V")) continue;
                        if (iRow == 19 && iCol == ToIndex("W")) continue;
                        if (iRow == 19 && iCol == ToIndex("X")) continue;
                        if (iRow == 19 && iCol == ToIndex("Y")) continue;
                        if (iRow == 19 && iCol == ToIndex("Z")) continue;
                        if (iRow == 19 && iCol == ToIndex("AA")) continue;
                        if (iRow == 20 && iCol == ToIndex("W")) continue;
                        if (iRow == 20 && iCol == ToIndex("X")) continue;
                        if (iRow == 21 && iCol == ToIndex("W")) continue;
                        if (iRow == 21 && iCol == ToIndex("Z")) continue;
                        if (iRow == 22 && iCol == ToIndex("W")) continue;
                        if (iRow == 22 && iCol == ToIndex("AA")) continue;
                        if (iRow == 23 && iCol == ToIndex("AB")) continue;
                        if (iRow == 23 && iCol == ToIndex("AC")) continue;
                        if (iRow == 23 && iCol == ToIndex("AD")) continue;
                        if (iRow == 23 && iCol == ToIndex("AE")) continue;
                        if (iRow == 24 && iCol == ToIndex("AB")) continue;
                        if (iRow == 24 && iCol == ToIndex("AC")) continue;
                        if (iRow == 25 && iCol == ToIndex("AB")) continue;
                        if (iRow == 25 && iCol == ToIndex("AD")) continue;
                        if (iRow == 26 && iCol == ToIndex("AB")) continue;
                        if (iRow == 26 && iCol == ToIndex("AE")) continue;
                        if (iRow == 27 && iCol == ToIndex("AF")) continue;
                        if (iRow == 27 && iCol == ToIndex("AG")) continue;
                        if (iRow == 27 && iCol == ToIndex("AH")) continue;
                        if (iRow == 27 && iCol == ToIndex("AI")) continue;
                        if (iRow == 27 && iCol == ToIndex("AJ")) continue;
                        if (iRow == 27 && iCol == ToIndex("AK")) continue;
                        if (iRow == 27 && iCol == ToIndex("AL")) continue;
                        if (iRow == 27 && iCol == ToIndex("AM")) continue;
                        if (iRow == 27 && iCol == ToIndex("AN")) continue;
                        if (iRow == 27 && iCol == ToIndex("AO")) continue;
                        if (iRow == 27 && iCol == ToIndex("AP")) continue;
                        if (iRow == 27 && iCol == ToIndex("AQ")) continue;
                        if (iRow == 27 && iCol == ToIndex("AR")) continue;
                        if (iRow == 27 && iCol == ToIndex("AS")) continue;
                        if (iRow == 27 && iCol == ToIndex("AT")) continue;
                        if (iRow == 27 && iCol == ToIndex("AU")) continue;
                        if (iRow == 28 && iCol == ToIndex("AF")) continue;
                        if (iRow == 29 && iCol == ToIndex("AF")) continue;
                        if (iRow == 30 && iCol == ToIndex("AF")) continue;
                        if (iRow == 31 && iCol == ToIndex("AF")) continue;
                        if (iRow == 32 && iCol == ToIndex("AF")) continue;
                        if (iRow == 33 && iCol == ToIndex("AV")) continue;
                        if (iRow == 33 && iCol == ToIndex("AW")) continue;
                        if (iRow == 33 && iCol == ToIndex("AX")) continue;
                        if (iRow == 33 && iCol == ToIndex("AY")) continue;
                        if (iRow == 33 && iCol == ToIndex("AZ")) continue;
                        if (iRow == 33 && iCol == ToIndex("BA")) continue;
                        if (iRow == 33 && iCol == ToIndex("BB")) continue;
                        if (iRow == 33 && iCol == ToIndex("BC")) continue;
                        if (iRow == 33 && iCol == ToIndex("BD")) continue;
                        if (iRow == 33 && iCol == ToIndex("BE")) continue;
                        if (iRow == 34 && iCol == ToIndex("AV")) continue;
                        if (iRow == 34 && iCol == ToIndex("AW")) continue;
                        if (iRow == 35 && iCol == ToIndex("AV")) continue;
                        if (iRow == 35 && iCol == ToIndex("AY")) continue;
                        if (iRow == 36 && iCol == ToIndex("AV")) continue;
                        if (iRow == 37 && iCol == ToIndex("AV")) continue;
                        if (iRow == 37 && iCol == ToIndex("BB")) continue;
                        if (iRow == 38 && iCol == ToIndex("AV")) continue;
                        if (iRow == 38 && iCol == ToIndex("BC")) continue;
                        if (iRow == 39 && iCol == ToIndex("AV")) continue;
                        if (iRow == 39 && iCol == ToIndex("BD")) continue;
                        if (iRow == 40 && iCol == ToIndex("AV")) continue;
                        if (iRow == 40 && iCol == ToIndex("BF")) continue;
                        if (iRow == 40 && iCol == ToIndex("BG")) continue;
                        if (iRow == 40 && iCol == ToIndex("BH")) continue;
                        if (iRow == 40 && iCol == ToIndex("BI")) continue;
                        if (iRow == 40 && iCol == ToIndex("BJ")) continue;
                        if (iRow == 40 && iCol == ToIndex("BK")) continue;
                        if (iRow == 40 && iCol == ToIndex("BL")) continue;
                        if (iRow == 40 && iCol == ToIndex("BM")) continue;
                        if (iRow == 41 && iCol == ToIndex("BF")) continue;
                        if (iRow == 41 && iCol == ToIndex("BG")) continue;
                        if (iRow == 41 && iCol == ToIndex("BH")) continue;
                        if (iRow == 41 && iCol == ToIndex("BI")) continue;
                        if (iRow == 41 && iCol == ToIndex("BJ")) continue;
                        if (iRow == 41 && iCol == ToIndex("BK")) continue;
                        if (iRow == 41 && iCol == ToIndex("BL")) continue;
                        if (iRow == 41 && iCol == ToIndex("BM")) continue;
                        if (iRow == 42 && iCol == ToIndex("BF")) continue;
                        if (iRow == 42 && iCol == ToIndex("BG")) continue;
                        if (iRow == 43 && iCol == ToIndex("BF")) continue;
                        if (iRow == 43 && iCol == ToIndex("BI")) continue;
                        if (iRow == 44 && iCol == ToIndex("BF")) continue;
                        if (iRow == 44 && iCol == ToIndex("BJ")) continue;
                        if (iRow == 45 && iCol == ToIndex("BF")) continue;
                        if (iRow == 45 && iCol == ToIndex("BJ")) continue;
                        if (iRow == 46 && iCol == ToIndex("BF")) continue;
                        if (iRow == 47 && iCol == ToIndex("BF")) continue;
                        if (iRow == 48 && iCol == ToIndex("BF")) continue;
                        if (iRow == 48 && iCol == ToIndex("BL")) continue;
                        if (iRow == 49 && iCol == ToIndex("BL")) continue;
                        if (iRow == 49 && iCol == ToIndex("BL")) continue;
                        if (iRow == 50 && iCol == ToIndex("BN")) continue;
                        if (iRow == 50 && iCol == ToIndex("BO")) continue;
                        if (iRow == 50 && iCol == ToIndex("BP")) continue;
                        if (iRow == 50 && iCol == ToIndex("BQ")) continue;
                        if (iRow == 50 && iCol == ToIndex("BR")) continue;
                        if (iRow == 50 && iCol == ToIndex("BS")) continue;
                        if (iRow == 50 && iCol == ToIndex("BT")) continue;
                        if (iRow == 50 && iCol == ToIndex("BU")) continue;
                        if (iRow == 51 && iCol == ToIndex("BN")) continue;
                        if (iRow == 51 && iCol == ToIndex("BO")) continue;
                        if (iRow == 51 && iCol == ToIndex("BP")) continue;
                        if (iRow == 51 && iCol == ToIndex("BQ")) continue;
                        if (iRow == 51 && iCol == ToIndex("BR")) continue;
                        if (iRow == 51 && iCol == ToIndex("BS")) continue;
                        if (iRow == 51 && iCol == ToIndex("BT")) continue;
                        if (iRow == 51 && iCol == ToIndex("BU")) continue;
                        if (iRow == 52 && iCol == ToIndex("BN")) continue;
                        if (iRow == 52 && iCol == ToIndex("BO")) continue;
                        if (iRow == 53 && iCol == ToIndex("BN")) continue;
                        if (iRow == 53 && iCol == ToIndex("BP")) continue;
                        if (iRow == 54 && iCol == ToIndex("BN")) continue;
                        if (iRow == 54 && iCol == ToIndex("BQ")) continue;
                        if (iRow == 55 && iCol == ToIndex("BN")) continue;
                        if (iRow == 55 && iCol == ToIndex("BR")) continue;
                        if (iRow == 56 && iCol == ToIndex("BN")) continue;
                        if (iRow == 57 && iCol == ToIndex("BN")) continue;
                        if (iRow == 57 && iCol == ToIndex("BS")) continue;
                        if (iRow == 58 && iCol == ToIndex("BN")) continue;
                        if (iRow == 58 && iCol == ToIndex("BT")) continue;
                        if (iRow == 55 && iCol == ToIndex("M")) continue;
                        if (iRow == 56 && iCol == ToIndex("BN")) continue;
                        if (iRow == 56 && iCol == ToIndex("BU")) continue;
                        double mj = 0.00;


                        double.TryParse(dr[iCol - startCol].ToString(), out mj);
                        if (mj != 0)
                        {
                            //cells[iRow, iCol].SetStyle(styleNum);
                            cells[iRow, iCol].PutValue(mj);
                        }
                        
                    }
                }
                #endregion

                wk.Save(destFile);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                

            }

        }

        private int ToIndex(string columnName)
        {
            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }


        private string ToName(int index)
        {
            List<string> chars = new List<string>();
            do
            {
                if (chars.Count > 0) index--;
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //导出excel
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            dlg.FileName = "二调三调流量表.xls";
            dlg.OverwritePrompt = false;
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destFile = dlg.FileName;
            try
            {
                if (System.IO.File.Exists(destFile))
                {
                    System.IO.File.Delete(destFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("该文件已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;           
            try
            {               

                this.UpdateStatus("正在导出Excel...");
                OutExcel(destFile, xzdm);
                this.UpdateStatus("导出完毕！");

                this.Cursor = Cursors.Default;
                MessageBox.Show("导出完毕！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(destFile);
                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            

        

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //生成一览表
            //导出excel
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xlsx";
            dlg.FileName = "二调三调变更一览表.xlsx";
            dlg.OverwritePrompt = false;
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destFile = dlg.FileName;
            try
            {
                if (System.IO.File.Exists(destFile))
                {
                    System.IO.File.Delete(destFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("该文件已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            try
            {

                this.UpdateStatus("正在导出一览表Excel...");
                OutYlb(destFile);
                this.UpdateStatus("一览表导出完毕！");

                this.Cursor = Cursors.Default;
                MessageBox.Show("导出完毕！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            



        }

        private void beXZDWHShp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SHP文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string dltbhFile = dlg.FileName;
            this.beXZDWHShp.Text = dltbhFile;
        }


        private IGeometry GeometryDiffence(IFeature A, IFeature B)
        {
            ITopologicalOperator pTopInterDltb = A.ShapeCopy as ITopologicalOperator;
            IGeometry interGeo = pTopInterDltb.Intersect(B.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            if (!interGeo.IsEmpty)
            {
                IGeometry oldDltbGeo = pTopInterDltb.Difference(interGeo);

                return oldDltbGeo;
            }
            else
            {
                return A.ShapeCopy;
            }

        }
        public IPolygon FlatBufferOneway(IPolyline inLine, double bufferDis, int zf, bool isExtent, double dExtentLen, IFeatureClass dltbClass)
        {
            object o = System.Type.Missing;
            //分别对输入的线平移
            IConstructCurve newCurve = new PolylineClass();
            newCurve.ConstructOffset(inLine, zf * bufferDis, ref o, ref o);
            IPolyline addline = newCurve as IPolyline;
            addline.ReverseOrientation();  //新线 反转

            IPolygon myPolygon = GeometryHelper.ConstructPolygonByLine(addline, inLine);
            if (isExtent)
            {
                #region 延伸吸附
                ITopologicalOperator pTopNewPolygon = myPolygon as ITopologicalOperator;
                //这半边框与地类图斑叠加，判断是否有交于一个点的
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = myPolygon as IGeometry;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = dltbClass.Search(pSF as IQueryFilter, false);
                IFeature aDltb = null;
                while ((aDltb = pCursor.NextFeature()) != null)
                {
                    IGeometry intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry2Dimension);
                    if (!intersecPoint.IsEmpty)
                        continue;
                    intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry1Dimension);
                    if (!intersecPoint.IsEmpty)
                        continue;

                    intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry0Dimension);
                    if (!intersecPoint.IsEmpty)
                    {
                        if ((intersecPoint as IPointCollection).PointCount != 1)
                            continue;
                        IPoint aPt = (intersecPoint as IPointCollection).get_Point(0);

                        ITopologicalOperator pTopDltb = aDltb.Shape as ITopologicalOperator;
                        IGeometry boundry = pTopDltb.Boundary;
                        IConstructCurve constructCurve = new PolylineClass();
                        bool isExtensionPerfomed = false;


                        //如果交于一个点，
                        IPolyline extendLine = null;
                        //判断这个点 是 属于这两条线的哪一条的
                        if (GeometryHelper.isPointOnLine(aPt, inLine))
                        {
                            extendLine = addline;
                            //延伸                       
                            constructCurve.ConstructExtended(addline, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
                            if (!(constructCurve as IPolyline).IsEmpty)
                            {
                                extendLine = constructCurve as IPolyline;
                            }
                            double lenDiff = extendLine.Length - addline.Length;
                            if (lenDiff < dExtentLen && lenDiff > 0)
                            {
                                addline = extendLine;
                                //myPolygon = GeometryHelper.ConstructPolygonByLine(extendLine, inLine);
                            }

                        }
                        else if (GeometryHelper.isPointOnLine(aPt, addline))
                        {
                            extendLine = inLine;
                            //延伸                        
                            constructCurve.ConstructExtended(inLine, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
                            if (!(constructCurve as IPolyline).IsEmpty)
                            {
                                extendLine = constructCurve as IPolyline;
                            }
                            double lenDiff = extendLine.Length - addline.Length;
                            if (lenDiff < dExtentLen && lenDiff > 0)
                            {
                                inLine = extendLine;
                                //myPolygon = GeometryHelper.ConstructPolygonByLine(addline, extendLine);
                            }
                        }



                    }
                }
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pCursor);
                #endregion

            }
            myPolygon = GeometryHelper.ConstructPolygonByLine(addline, inLine);
            return myPolygon;
        }
        private List<IFeature> getInter1Features(IFeature inFeature, IFeatureClass aClass, string where)
        {
            List<IFeature> retFeatures = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = inFeature.ShapeCopy;
            spatialFilter.WhereClause = where;

            ITopologicalOperator pTop = inFeature.ShapeCopy as ITopologicalOperator;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {
                    if (aFea != inFeature)
                    {
                        IGeometry interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        if (!interGeo.IsEmpty)
                        {
                            retFeatures.Add(aFea);
                        }
                        else
                        {
                            interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                            {
                                if (!interGeo.IsEmpty)
                                {
                                    retFeatures.Add(aFea);
                                }
                            }
                        }
                    }
                    aFea = featureCursor.NextFeature();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                ////垃圾回收  
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
            return retFeatures;
        }

        //相交于一个面的，
        private List<IFeature> GetInterPolygonFeatures(IFeature inFeature, IFeatureClass aClass)
        {
            List<IFeature> retFeatures = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = inFeature.ShapeCopy;
            ITopologicalOperator pTop = inFeature.ShapeCopy as ITopologicalOperator;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {

                    if (aFea != inFeature)
                    {
                        IGeometry interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        if (!interGeo.IsEmpty)
                        {
                            retFeatures.Add(aFea);

                        }


                    }
                    aFea = featureCursor.NextFeature();

                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                ////垃圾回收  
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
            return retFeatures;
        }
        private void GPUpdate(string inFeatures, string updateFeatures, string outFeatures)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Identity pUpdate = new ESRI.ArcGIS.AnalysisTools.Identity();

            pUpdate.in_features = inFeatures;
            pUpdate.identity_features = updateFeatures;
            pUpdate.out_feature_class = outFeatures;
            //pUpdate.keep_borders = "NO_BORDERS";
            //pUpdate.cluster_tolerance = 0.001;

            gp.Execute(pUpdate, null);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //导出excel
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destPath = dlg.SelectedPath;
            IFeatureClass pFeatureclass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("xzq");
            IFeatureCursor pFeatureCursor = pFeatureclass.Search(null, true);
            IFeature pFeature;
            while ((pFeature = pFeatureCursor.NextFeature()) != null)
            {
                string xzqdm = pFeature.get_Value(pFeatureclass.FindField("xzqdm")).ToString();
                string destFile = destPath + "\\(" + xzqdm + ")二调三调流量表.xls";
                try
                {
                    if (System.IO.File.Exists(destFile))
                    {
                        System.IO.File.Delete(destFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("该文件已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                try
                {

                    this.UpdateStatus("正在导出Excel...");
                    OutExcel(destFile, xzqdm);
                    this.UpdateStatus("导出完毕！");

                    this.Cursor = Cursors.Default;
                    
                    //System.Diagnostics.Process.Start(destFile);

                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message);
                }
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureCursor);
            MessageBox.Show("导出完毕！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private IPolygon union(IFeatureClass featureClass)
        {
            if (featureClass == null)
            { return null; }

            IGeoDataset geoDataset = featureClass as IGeoDataset;
            IGeometry geometryBag = new GeometryBagClass();
            geometryBag.SpatialReference = geoDataset.SpatialReference;
            IFeatureCursor featureCursor = featureClass.Search(null, true);
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            IFeature currentFeature = featureCursor.NextFeature();
            while (currentFeature != null)
            {
                object missing = Type.Missing;
                geometryCollection.AddGeometry(currentFeature.Shape, ref missing, ref missing);
                currentFeature = featureCursor.NextFeature();
            }
            ITopologicalOperator unionedPolygon = new PolygonClass();
            unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
            RCIS.Utility.OtherHelper.ReleaseComObject(featureCursor);
            return unionedPolygon as IPolygon;
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            /*
             * 计算流量分析过程 
             * 1.二调图斑与三调图斑进行叠加分析
             * 2.二三调叠加分析的图斑与二调线状地物进行叠加分析
             * 3.删除叠加分析后线状地物中重复数据
             * 4.将二三调叠加结果与二调线状地物处理结果进行交集制表
             * 5.计算线状地物面积，计算图斑面积
             */

            if (this.beDltbHShp.Text.Trim() == "")
                return;
            if (this.cmbLayers.Text.Trim() == "")
                return;
            if (this.beXZDWHShp.Text.Trim() == "")
                return;

            
            IWorkspace tempWorkspace = GetWorkspace(Application.StartupPath + @"\tmp\temp.gdb");
            string bgqDLTB = beDltbHShp.Text;
            string dltbName = OtherHelper.GetLeftName(this.cmbLayers.Text);
            string bghDLTB = currWs.PathName + "\\" + RCIS.Global.AppParameters.DATASET_DEFAULT_NAME + "\\" + dltbName;
            string err = "";
            UpdateStatus("开始叠加地类图斑");
            bool b = GP_Intersect(bgqDLTB,bghDLTB,Application.StartupPath + @"\tmp\temp.gdb\dltb",ref err);
            if (!b)
            {
                UpdateStatus("叠加地类图斑失败");
                UpdateStatus(err);
                return;
            }
            tempWorkspace.ExecuteSQL("update dltb set tbmj = Shape_Area");
            tempWorkspace.ExecuteSQL("update dltb set tbmj_1 = Shape_Area");
            UpdateStatus("开始叠加线状地物");
            b = GP_Intersect(Application.StartupPath + @"\tmp\temp.gdb\dltb", beXZDWHShp.Text, Application.StartupPath + @"\tmp\temp.gdb\xzdw", ref err);
            if (!b)
            {
                UpdateStatus("叠加线状地物失败");
                UpdateStatus(err);
                return;
            }
            UpdateStatus("处理线状地物数据");
            b = GP_DeleteSame(Application.StartupPath + @"\tmp\temp.gdb\xzdw", ref err);
            if (!b)
            {
                UpdateStatus("处理线状地物数据失败");
                UpdateStatus(err);
                return;
            }

            this.UpdateStatus("处理行政边界上的线状地物");
            IFeatureWorkspace tempFWorkspace = tempWorkspace as IFeatureWorkspace;
            IFeatureClass pXZQClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            IPolygon pPolygon = union(pXZQClass);
            if (pPolygon != null)
            {
                IGeometry pPolyline = (pPolygon as ITopologicalOperator).Boundary;
                IFeatureClass pxzdwClass = tempFWorkspace.OpenFeatureClass("xzdw");
                ISpatialFilter pSpatialFilter = new SpatialFilter();
                pSpatialFilter.Geometry = pPolyline;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pFeatureCursor = pxzdwClass.Update(pSpatialFilter, true);
                    comRel.ManageLifetime(pFeatureCursor);
                    IFeature pXZDW;
                    while ((pXZDW = pFeatureCursor.NextFeature()) != null)
                    {
                        double kd = FeatureHelper.GetFeatureDoubleValue(pXZDW, "KD");
                        FeatureHelper.SetFeatureValue(pXZDW, "KD", kd / 2);
                        pFeatureCursor.UpdateFeature(pXZDW);
                    }
                }
            }

            UpdateStatus("计算线状地物与图斑关系");
            IFeatureClass ptempClass = tempFWorkspace.OpenFeatureClass("dltb");
            string dltbID = ptempClass.OIDFieldName;
            ptempClass = tempFWorkspace.OpenFeatureClass("xzdw");
            string xzdwID = ptempClass.OIDFieldName;
            b = GP_TabulateIntersection(Application.StartupPath + @"\tmp\temp.gdb\dltb", dltbID + ";zldwdm;tbbh;zldwdm_1;tbbh_1;dlbm_1;",
                                        Application.StartupPath + @"\tmp\temp.gdb\xzdw", xzdwID + ";dlbm_12;kd;", Application.StartupPath + @"\tmp\temp.gdb\dltbxzdw", ref err);
            if (!b)
            {
                UpdateStatus("计算线状地物与图斑关系失败");
                UpdateStatus(err);
                return;
            }
            
            System.Runtime.InteropServices.Marshal.ReleaseComObject(ptempClass);
            
            this.UpdateStatus("导入地类图斑");
            b = RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(tempWorkspace, targetWs, "dltb", "DLTBGXGC", null);
            if (!b)
            {
                this.UpdateStatus("导入地类图斑失败");
                this.Cursor = Cursors.Default;
                return;
            }
            this.UpdateStatus("导入线状地物");
            b = RCIS.GISCommon.EsriDatabaseHelper.CopyTable(tempWorkspace, targetWs, "dltbxzdw", "dltbxzdw", null);
            if (!b)
            {
                this.UpdateStatus("导入线状地物失败");
                this.Cursor = Cursors.Default;
                return;
            }
            IWorkspace llfxWorkspace = GetWorkspace(Application.StartupPath + @"\SystemConf\llfx.mdb");
            IFeatureWorkspace llfxFwor = llfxWorkspace as IFeatureWorkspace;
            ITable pTable = llfxFwor.OpenTable("dltbxzdw");
            string xzdwOID = pTable.Fields.Field[7].Name;
            int i = LS_LlfxMDBHelper.ExecuteSQLNonquery(string.Format("select {0},count({0}) as gs into idgs from dltbxzdw group by {0}", xzdwOID));
            AddField(pTable, "gs", "gs", esriFieldType.esriFieldTypeInteger);
            AddField(pTable, "mj", "mj", esriFieldType.esriFieldTypeDouble);
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery(string.Format("update dltbxzdw a,idgs b set a.gs = b.gs where a.{0} = b.{0}", xzdwOID));
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update dltbxzdw set mj = length * kd / gs");
            
            if (chkTKXS.Checked)
            {
                i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set TKMJ = tbmj * TKXS / 100");
            }
            else
            {
                i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set TKMJ = tbmj * TKXS");
            }
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set KCMJ = tbmj_1 * KCXS");
            string dltbOID = pTable.Fields.Field[1].Name;
            pTable = llfxFwor.OpenTable("DLTBGXGC");
            dltbID = pTable.OIDFieldName;
            
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC a set a.tbdlmj_1 = a.tbmj_1 - a.kcmj");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update dltbgxgc set xzdwmj = 0");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery(string.Format("select {0},sum(mj) as mj1 into xzdwmj from dltbxzdw group by {0}", dltbOID));
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery(string.Format("update dltbgxgc a, xzdwmj b set a.xzdwmj = b.mj1 where a.{0} = b.{1}", dltbID, dltbOID));
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC a set a.tbdlmj = a.tbmj - a.tkmj - a.xzdwmj");

            //计算完成后 将导入到llfx mdb中，方便写sql语句
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pTable);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(llfxWorkspace);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(tempWorkspace);
            Application.DoEvents();
            GC.Collect();
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("Delete From BG_YLB_PFM");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("Delete From BG_YLB");
            this.UpdateStatus("开始进计算流量...");
            CalBgbByGc( xzdm,true);

            this.UpdateStatus("正在进行平衡表计算...");
            BuildBgb(xzdm);

            this.Cursor = Cursors.Default;
            this.UpdateStatus("执行完毕");

            MessageBox.Show("执行完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddField(ITable pTable, string name, string aliasName, esriFieldType FieldType)
        {
            //若存在，则不需添加
            if (pTable.Fields.FindField(name) > -1) return;
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.AliasName_2 = aliasName;
            pFieldEdit.Name_2 = name;
            pFieldEdit.Type_2 = FieldType;

            IClass pClass = pTable as IClass;
            pClass.AddField(pField);
        }
        private bool GP_DeleteSame(string inDataset, ref string err)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;

            ESRI.ArcGIS.DataManagementTools.DeleteIdentical delIdentical = new ESRI.ArcGIS.DataManagementTools.DeleteIdentical();
            delIdentical.in_dataset = inDataset;
            delIdentical.fields = "Shape";
            err = RunTool(gp, delIdentical, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return false;
            }

            return true;
        }

        private bool GP_TabulateIntersection(string ZoneFeatures, string ZoneField, string ClassFeatures, string ClassField, string outTable, ref string err)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;

            ESRI.ArcGIS.AnalysisTools.TabulateIntersection TabInter = new ESRI.ArcGIS.AnalysisTools.TabulateIntersection();
            TabInter.in_zone_features = ZoneFeatures;
            TabInter.zone_fields = ZoneField;
            TabInter.in_class_features = ClassFeatures;
            TabInter.class_fields = ClassField;
            TabInter.out_table = outTable;
            err = RunTool(gp, TabInter, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return false;
            }

            return true;
        }

        private bool GP_Intersect(string inShp1, string inShp2, string outShp, ref string err)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Intersect pIntersect = new ESRI.ArcGIS.AnalysisTools.Intersect();

            pIntersect.in_features = inShp1 + ";" + inShp2;
            pIntersect.out_feature_class = outShp;
            pIntersect.join_attributes = "ALL";
            pIntersect.output_type = "INPUT";

            err = RunTool(gp, pIntersect, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return false;
            }

            return true;
        }

        private bool GP_Buffer(string inShp, string outShp, string disOrField, string disType, string[] disField, ref string err)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Buffer pBuffer = new ESRI.ArcGIS.AnalysisTools.Buffer();

            pBuffer.in_features = inShp;
            pBuffer.out_feature_class = outShp;
            pBuffer.buffer_distance_or_field = disOrField;
            pBuffer.line_side = "FULL";
            pBuffer.line_end_type = "ROUND";
            pBuffer.dissolve_option = disType;
            IGpValueTableObject pObject = new GpValueTableObjectClass();//对多个字段进行融合添加
            pObject.SetColumns(1);
            foreach (string item in disField)
            {
                pObject.AddRow(item);
            }
            pBuffer.dissolve_field = pObject;

            err = RunTool(gp, pBuffer, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return false;
            }

            return true;
        }

        private bool GP_Dissolve(string inShp, string outShp, string disField, ref string err)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.DataManagementTools.Dissolve pDissolve = new ESRI.ArcGIS.DataManagementTools.Dissolve();

            pDissolve.in_features = inShp;
            pDissolve.out_feature_class = outShp;
            pDissolve.dissolve_field = disField;
            pDissolve.multi_part = "MULTI_PART";

            err = RunTool(gp, pDissolve, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return false;
            }

            return true;
        }

        private bool GP_Union(string inShp, string outShp, ref string err)
        {
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();

            pUnion.in_features = inShp;
            pUnion.out_feature_class = outShp;
            pUnion.join_attributes = "ALL";

            err = RunTool(gp, pUnion, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 打开临时地理数据库若不存在则创建
        /// </summary>
        /// <param name="filePath">文件路径例如：shp:"F:\\GIS测试数据\\hhh\\111.shp"  mdb:"F:\\GIS测试数据\\111.mdb"  gdb:"F:\\GIS测试数据\\111.gdb"</param>
        /// <returns></returns>
        private IWorkspace GetWorkspace(string filePath)
        {
            string sPathFloder = System.IO.Path.GetDirectoryName(filePath);
            string sPathName = System.IO.Path.GetFileName(filePath);
            string sFileExtension = System.IO.Path.GetExtension(filePath).ToUpper();
            if (!Directory.Exists(sPathFloder))
                Directory.CreateDirectory(sPathFloder);

            // Instantiate a workspace factory if not exists then create a workspace.
            IWorkspaceName pWorkspaceName = null;
            string sWorkspaceId = "esriDataSourcesGDB.FileGDBWorkspaceFactory";
            switch (sFileExtension)
            {
                case ".GDB":
                    sWorkspaceId = "esriDataSourcesGDB.FileGDBWorkspaceFactory";
                    if (!Directory.Exists(filePath))
                        pWorkspaceName = ((IWorkspaceFactory)Activator.CreateInstance(Type.GetTypeFromProgID(sWorkspaceId))).Create(sPathFloder, sPathName, null, 0);
                    else
                        pWorkspaceName = new WorkspaceNameClass()
                        {
                            WorkspaceFactoryProgID = sWorkspaceId,
                            PathName = filePath
                        };
                    break;
                case ".MDB":
                    sWorkspaceId = "esriDataSourcesGDB.AccessWorkspaceFactory";
                    if (!File.Exists(filePath))
                        pWorkspaceName = ((IWorkspaceFactory)Activator.CreateInstance(Type.GetTypeFromProgID(sWorkspaceId))).Create(sPathFloder, sPathName, null, 0);
                    else
                        pWorkspaceName = new WorkspaceNameClass()
                        {
                            WorkspaceFactoryProgID = sWorkspaceId,
                            PathName = filePath
                        };
                    break;
                case ".SHP":
                    sWorkspaceId = "esriDataSourcesFile.ShapefileWorkspaceFactory";
                    pWorkspaceName = new WorkspaceNameClass()
                    {
                        WorkspaceFactoryProgID = sWorkspaceId,
                        PathName = sPathFloder
                    };
                    break;
            }
            // Cast the workspace name object to the IName interface and open the workspace.
            IName name = (IName)pWorkspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }

        private void btnCompute2_Click(object sender, EventArgs e)
        {
            /*计算流量分析过程
             * 1.线状地物宽度除以2，并以该字段进行缓冲，同时按照dlbm和kd融合
             * 2.对处理后的线状地物进行联合
             * 3.对处理后的线状地物删除相同
             * 4.对处理后的线状地物进行联合
             * 5.将二调图斑与处理后的线状地物进行联合
             * 6.叠加分析处理后的二调图斑和三调图斑
             * 7.重新计算面积
             */

            if (this.beDltbHShp.Text.Trim() == "")
                return;
            if (this.cmbLayers.Text.Trim() == "")
                return;
            if (this.beXZDWHShp.Text.Trim() == "")
                return;


            IWorkspace tempWorkspace = GetWorkspace(Application.StartupPath + @"\tmp\temp.gdb");
            string bgqDLTB = beDltbHShp.Text;
            string dltbName = OtherHelper.GetLeftName(this.cmbLayers.Text);
            string bghDLTB = currWs.PathName + "\\" + RCIS.Global.AppParameters.DATASET_DEFAULT_NAME + "\\" + dltbName;
            string bgqXZDW = beXZDWHShp.Text;
            string err = "";
            UpdateStatus("开始处理线状地物宽度");
            IWorkspace pWSXZDW = GetWorkspace(bgqXZDW);
            EsriDatabaseHelper.ConvertFeatureClass(pWSXZDW, tempWorkspace, System.IO.Path.GetFileNameWithoutExtension(bgqXZDW), "xzdw", null);
            tempWorkspace.ExecuteSQL("Update xzdw Set KD = KD / 2");
            UpdateStatus("开始线状地物转面");
            bool b = GP_Buffer(Application.StartupPath + @"\tmp\temp.gdb\xzdw", Application.StartupPath + @"\tmp\temp.gdb\xzdw_Buffer", "KD", "LIST", new string[2] { "DLBM", "KD" }, ref err);
            if (!b)
            {
                UpdateStatus("线状地物转面失败");
                UpdateStatus(err);
                return;
            }
            UpdateStatus("开始处理线状地物生成的面数据");
            b = GP_Union(Application.StartupPath + @"\tmp\temp.gdb\xzdw_Buffer", Application.StartupPath + @"\tmp\temp.gdb\xzdw_Union", ref err);
            if (!b)
            {
                UpdateStatus("处理失败");
                UpdateStatus(err);
                return;
            }
            UpdateStatus("开始删除线状地物重复数据");
            b = GP_DeleteSame(Application.StartupPath + @"\tmp\temp.gdb\xzdw_Union", ref err);
            if (!b)
            {
                UpdateStatus("处理失败");
                UpdateStatus(err);
                return;
            }
            //UpdateStatus("开始处理线状地物碎数据");
            //b = GP_Dissolve(Application.StartupPath + @"\tmp\temp.gdb\xzdw_Union", Application.StartupPath + @"\tmp\temp.gdb\xzdw_OK", "DLBM;KD;", ref err);
            //if (!b)
            //{
            //    UpdateStatus("处理失败");
            //    UpdateStatus(err);
            //    return;
            //}
            this.UpdateStatus("叠加分析二调图斑和线状地物");
            b = GP_Union(bgqDLTB + ";" + Application.StartupPath + @"\tmp\temp.gdb\xzdw_Union;", Application.StartupPath + @"\tmp\temp.gdb\dltbxzdw", ref err);
            if (!b)
            {
                UpdateStatus("叠加分析失败");
                UpdateStatus(err);
                return;
            }
            tempWorkspace.ExecuteSQL("Delete From dltbxzdw Where DLBM = ''");
            tempWorkspace.ExecuteSQL("Update dltbxzdw Set DLBM = DLBM_1 Where DLBM_1 <> ''");
            this.UpdateStatus("叠加分析二调图斑和三调图斑");
            b = GP_Intersect(Application.StartupPath + @"\tmp\temp.gdb\dltbxzdw", bghDLTB, Application.StartupPath + @"\tmp\temp.gdb\dltbOK", ref err);

            this.UpdateStatus("导入地类图斑");
            b = RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(tempWorkspace, targetWs, "dltbOK", "DLTBGXGC", null);
            if (!b)
            {
                this.UpdateStatus("导入地类图斑失败");
                this.Cursor = Cursors.Default;
                return;
            }
            
            IWorkspace llfxWorkspace = GetWorkspace(Application.StartupPath + @"\SystemConf\llfx.mdb");
            IFeatureWorkspace llfxFwor = llfxWorkspace as IFeatureWorkspace;
            int i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set tbmj = shape_area");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set tbmj_1 = shape_area");
            
            if (chkTKXS.Checked)
            {
                i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set TKMJ = tbmj * TKXS / 100");
            }
            else
            {
                i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set TKMJ = tbmj * TKXS");
            }
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC set KCMJ = tbmj_1 * KCXS");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC a set a.tbdlmj = a.tbmj - a.tkmj");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("update DLTBGXGC a set a.tbdlmj_1 = a.tbmj_1 - a.kcmj");

            //计算完成后 将导入到llfx mdb中，方便写sql语句
            System.Runtime.InteropServices.Marshal.ReleaseComObject(llfxWorkspace);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(tempWorkspace);
            Application.DoEvents();
            GC.Collect();
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("Delete From BG_YLB_PFM");
            i = LS_LlfxMDBHelper.ExecuteSQLNonquery("Delete From BG_YLB");
            this.UpdateStatus("开始进计算流量...");
            CalBgbByGc(xzdm,false);

            this.UpdateStatus("正在进行平衡表计算...");
            BuildBgb(xzdm);

            this.Cursor = Cursors.Default;
            this.UpdateStatus("执行完毕");

            MessageBox.Show("执行完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    class bghtbmj : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            IFeature fea1 = x as IFeature;
            IFeature fea2 = y as IFeature;
            try
            {
                double tbmj1 = FeatureHelper.GetFeatureDoubleValue(fea1, "BGHTBMJ");
                double tbmj2 = FeatureHelper.GetFeatureDoubleValue(fea2, "BGHTBMJ");
                if (tbmj1 > tbmj2)
                {
                    return 1;
                }
                else if (tbmj1 == tbmj2)
                {
                    return 0;
                }
                else return -1;

            }
            catch (Exception ex)
            {
                return -1;

            }

        }

    }
}
