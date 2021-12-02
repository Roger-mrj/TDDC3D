using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.GISCommon;
using RCIS.Utility;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.esriSystem;
using System.Reflection;
using RCIS.Database;
//using Microsoft.Office.Interop.Excel;
//using ExcelApplication = Microsoft.Office.Interop.Excel.ApplicationClass;
namespace TDDC3D.edit
{
    public partial class GetGXGCForm : Form
    {
        public GetGXGCForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
        public IWorkspace currWs = null;
        IWorkspaceEdit wkspcEdit = null;

        private IFeatureClass dltb_hClass = null;
        private IFeatureClass dltbClass = null;
        private IFeatureClass dltbGxgcClass = null;

        private IFeatureDataset pDataset = null;
        private void GetGXGCForm_Load(object sender, EventArgs e)
        {
            try
            {
                wkspcEdit = currWs as IWorkspaceEdit;
                dltb_hClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB_H");
                dltbClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
                dltbGxgcClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB_GXGC");
                pDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            }
            catch (Exception ex)
            {
            }
            if (dltb_hClass == null)
            {
                MessageBox.Show("没有启用更新历史或者无更新历史数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ArrayList arGxsj = FeatureHelper.GetUniqueFieldValueByDataStatistics(dltb_hClass, null, "GXSJ");
            foreach (string aBgrq in arGxsj)
            {
                this.checkedListBoxControl1.Items.Add(aBgrq);
            }
        }
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

        private void UpdateStatus(string txt)
        {
            this.memoLog.Text += "\r\n" + DateTime.Now.ToString() + ":" + txt;
            System.Windows.Forms.Application.DoEvents();
        }


        //历史层和变更后层进行叠加，生成临时文件
        private IFeatureClass GetIntersectClass(string bgqshp,string bghshp)
        {
            //"D:\第三次土地调查\测试数据\新建文件地理数据库.gdb\TDDC\DLTB #;
            //D:\第三次土地调查\ArcGIS版源程序\release\tmp\DLTB_H.shp #" 
            //D:\第三次土地调查\ArcGIS版源程序\release\tmp\dltbgxgc.shp ALL # INPUT


            string classshpName = "DLTBGXGC" + DateTime.Now.Millisecond.ToString() + DateTime.Now.Second.ToString();
            string tmpShp = System.Windows.Forms.Application.StartupPath + "\\tmp\\" + classshpName + ".shp"; //目标shp路径

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



            string err = RunTool(gp, pIntersect, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                return null;
            }
            IWorkspace tmpShpWs = WorkspaceHelper2.GetShapefileWorkspace(tmpShp);
            IFeatureWorkspace shpFeaWs = tmpShpWs as IFeatureWorkspace;
            IFeatureClass shpClass = shpFeaWs.OpenFeatureClass(classshpName);
            return shpClass;
        }

        private bool StartEditOp()
        {
            bool retVal = false;

            // Check to see if we're editing
            if (!wkspcEdit.IsBeingEdited())
            {
                // Not being edited so start here
                wkspcEdit.StartEditing(false);
                retVal = true;
            }

            // Start operation
            wkspcEdit.StartEditOperation();
            return retVal;
        }
        private void StopEditOp(bool weStartedEditing)
        {
            // Stop edit operation
            wkspcEdit.StopEditOperation();

            if (weStartedEditing)
            {
                // We started the edit session so stop it here
                wkspcEdit.StopEditing(true);
            }
        }

        private bool allBgq2Shp(string tmpFilePath,string bgqshp)
        {
            try
            {
                string firstItem = this.checkedListBoxControl1.CheckedItems[0].ToString();
                string sql = "GXSJ in ('" + firstItem + "'";
                for (int i = 1; i < this.checkedListBoxControl1.CheckedItemsCount; i++)
                {
                    sql += ",'" + this.checkedListBoxControl1.CheckedItems[i].ToString().Trim() + "'";
                }
                sql += ")";



                EsriDatabaseHelper.ExportFeature(dltb_hClass, tmpFilePath, sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        

        private void MJTP(IFeatureClass gxgcClass)
        {
            //对所有更新过程数据，按照更新时间进行分组
            ArrayList gxsjs= FeatureHelper.GetUniqueFieldValueByDataStatistics(gxgcClass, null, "GXSJ");
                                  
            foreach (string aSj in gxsjs)
            {
                List<string> bgqBsms = new List<string>();
                List<double> bgqZmj = new List<double>();                


                //找到该更新时间 对应所有数据，
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "GXSJ ='" + aSj + "' ";
                IFeatureCursor cursor = gxgcClass.Search(pQF, false);
                IFeature aFea = null;
                try
                {
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        //首先计算其变更后tb面积，重新计算
                        ESRI.ArcGIS.Geometry.IPoint selectPoint = (aFea.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        int currDh = (int)(X / 1000000);////WK---带号  
                        SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                        double tbmj = area.SphereArea(aFea.ShapeCopy, currDh);
                        FeatureHelper.SetFeatureValue(aFea, "BGHTBMJ", tbmj);
                        aFea.Store();

                        //计算变更前总面积
                        string bsm = FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                        if (!bgqBsms.Contains(bsm))
                        {
                            bgqBsms.Add(bsm);
                            bgqZmj.Add(FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ"));
                        }

                    }
                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                }
                              
                                
                for (int kk=0;kk<bgqBsms.Count;kk++)
                {
                    double abgqzmj = bgqZmj[kk];
                    string absm = bgqBsms[kk];

                    //计算变更后图斑面积和
                    pQF = new QueryFilterClass();
                    pQF.WhereClause = "BSM ='" + absm + "' ";
                    double bghZmj = 0;
                 
                    ArrayList BghFeatures = new ArrayList();
                    IFeatureCursor cur = gxgcClass.Search(pQF, false);
                    IFeature abghfea = null;
                    try
                    {
                        while ((abghfea = cur.NextFeature()) != null)
                        {
                            BghFeatures.Add(abghfea);
                            bghZmj += FeatureHelper.GetFeatureDoubleValue(abghfea, "BGHTBMJ");
                        }
                    }
                    catch { }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cur);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                    }


                    if (BghFeatures.Count == 1)
                    {
                        FeatureHelper.SetFeatureValue(BghFeatures[0] as IFeature, "BGHTBMJ",
                            FeatureHelper.GetFeatureDoubleValue(BghFeatures[0] as IFeature, "TBMJ"));
                        (BghFeatures[0] as IFeature).Store();
                    }
                    else
                    {
                        //变更后 按照图斑面积排序
                        IComparer comparer = new bghtbmj();
                        BghFeatures.Sort(comparer);
                        //调平
                        bool zf = true;
                        double diff =MathHelper.RoundEx( abgqzmj - bghZmj,2);
                        if (Math.Abs(diff) < 0.0001) continue;
                        if (diff < 0)
                        {
                            zf = false;
                        }
                        diff = Math.Abs(diff);
                        int iNum = (int)(diff / 0.01);
                        int shang = iNum / BghFeatures.Count;
                        int yushu = iNum % BghFeatures.Count;
                        for (int i = 0; i < BghFeatures.Count; i++)
                        {
                            IFeature aTPFea = BghFeatures[i] as IFeature;
                            double bghtbmj = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGHTBMJ");
                            if (i < yushu)
                            {

                                bghtbmj += (zf ? 0.01 * (shang + 1) : -0.01 * (shang + 1));
                                FeatureHelper.SetFeatureValue(aTPFea, "BGHTBMJ", MathHelper.RoundEx(bghtbmj, 2));
                            }
                            else
                            {
                                bghtbmj += (zf ? 0.01 * shang : -0.01 * shang);
                                FeatureHelper.SetFeatureValue(aTPFea, "BGHTBMJ", MathHelper.RoundEx(bghtbmj, 2));
                            }
                           
                            aTPFea.Store();
                        }
                    }
                }
               
                //重新计算图斑地类面积
                IFeatureCursor updateCursor = gxgcClass.Update(null, false);
                IFeature aupdateFea = null;
                try
                {
                    while ((aupdateFea = updateCursor.NextFeature()) != null)
                    {
                        //调平的时候顺便把 地类面积一起计算
                        double tbmj = FeatureHelper.GetFeatureDoubleValue(aupdateFea, "BGHTBMJ");
                        double kcxs = FeatureHelper.GetFeatureDoubleValue(aupdateFea, "BGHKCXS");
                        if (kcxs > 0)
                        {
                            double kcmj =MathHelper.RoundEx( tbmj * kcxs,2);
                            FeatureHelper.SetFeatureValue(aupdateFea, "KCMJ", kcmj);
                            double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                            FeatureHelper.SetFeatureValue(aupdateFea, "BGHTBDLMJ", dlmj);
                        }
                        else
                        {
                            FeatureHelper.SetFeatureValue(aupdateFea, "BGHTBDLMJ", tbmj);
                        }
                        aupdateFea.Store();
                    }
                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                }

                //
            }

                       


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.checkedListBoxControl1.CheckedItemsCount == 0)
            {
                MessageBox.Show("先选中历史数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string tmpFilePath = System.Windows.Forms.Application.StartupPath + @"\tmp";
            string bgqshp = tmpFilePath + "\\DLTB_H.shp";  //提取历史图层相关数据，只提取一部分
            
            try
            {
                this.UpdateStatus("开始提取选中日期历史数据...");
                allBgq2Shp(tmpFilePath, bgqshp);
                
                string bghshp = this.currWs.PathName + "\\"+RCIS.Global.AppParameters.DATASET_DEFAULT_NAME+ "\\DLTB";
                this.UpdateStatus("开始叠加分析...");
                IFeatureClass gxgcshp = GetIntersectClass(bgqshp, bghshp);
                if (gxgcshp == null)
                {
                    MessageBox.Show("没有分析结果！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                IFeatureLayer shpLyr = new FeatureLayerClass();
                shpLyr.FeatureClass = gxgcshp;
                IIdentify shpIds = shpLyr as IIdentify;
                IArray arShps = shpIds.Identify((gxgcshp as IGeoDataset).Extent);
                if (arShps == null)
                {
                    MessageBox.Show("没有分析结果！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ITable pTable = dltbGxgcClass as ITable;
                pTable.DeleteSearchedRows(null);  

                bool weStartedEditing = this.StartEditOp();
                this.UpdateStatus("开始插入分析结果信息...");
                using (ESRI.ArcGIS.ADF.ComReleaser comrelease = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor featureCursor = dltbGxgcClass.Insert(true);
                    comrelease.ManageLifetime(featureCursor);
                    for (int i = 0; i < arShps.Count; i++)
                    {
                        IFeatureIdentifyObj obj = arShps.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                        IFeature aFea = aRow.Row as IFeature;
                        //逐条 读取 

                        try
                        {
                            IFeatureBuffer featureBuffer = this.dltbGxgcClass.CreateFeatureBuffer();
                            featureBuffer.Shape = aFea.ShapeCopy;

                            for (int fdx = 0; fdx < this.dltb_hClass.Fields.FieldCount; fdx++)
                            {
                                IField aFld = null;
                                try
                                {
                                    aFld = dltb_hClass.Fields.get_Field(fdx);
                                }
                                catch { }
                                if (aFld == null) continue;

                                if (aFld.Type == esriFieldType.esriFieldTypeGUID || aFld.Type == esriFieldType.esriFieldTypeOID || aFld.Type == esriFieldType.esriFieldTypeOID
                                    || aFld.Type == esriFieldType.esriFieldTypeGeometry
                                    || aFld.Name.ToUpper().Contains("SHAPE_")
                                    )
                                {
                                    continue;
                                }
                                string fldName = aFld.Name.ToUpper();
                                string bghFldName = "BGH" + fldName;
                                
                                if (dltbGxgcClass.FindField(fldName) > -1)
                                {
                                    FeatureHelper.SetFeatureBufferValue(featureBuffer, fldName, FeatureHelper.GetFeatureValue(aFea, fldName));
                                }
                                if (dltbGxgcClass.FindField(bghFldName) > -1)
                                {
                                    FeatureHelper.SetFeatureBufferValue(featureBuffer, bghFldName, FeatureHelper.GetFeatureValue(aFea, fldName + "_1"));
                                }

                            }
                            FeatureHelper.SetFeatureBufferValue(featureBuffer, "GXSJ", FeatureHelper.GetFeatureValue(aFea, "GXSJ"));

                            featureCursor.InsertFeature(featureBuffer);
                        }
                        catch (Exception ex)
                        {
                            this.UpdateStatus("第" + i + "条导入失败." + ex.Message);
                        }
                        
                        

                    }
                    featureCursor.Flush();

                    //对更新过程中的每一条数据，需要重新计算图斑面积，进行调平，然后计算本图斑现在的图斑地类面积和   原来的图斑地类面积
                    //按照范围调平
                    this.UpdateStatus("开始进行前后面积调平...");
                    MJTP(dltbGxgcClass);


                    this.StopEditOp(weStartedEditing);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);

                }
                if ((gxgcshp as IDataset).CanDelete())
                {
                    (gxgcshp as IDataset).Delete();
                }
                try
                {
                    IWorkspace tmpWS = WorkspaceHelper2.GetShapefileWorkspace(tmpFilePath);
                    IFeatureClass dltbhClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("DLTB_H");
                    IDataset pTmpDltbHclass = dltbhClass as IDataset;
                    if (pTmpDltbHclass.CanDelete())
                    {
                        pTmpDltbHclass.Delete();
                    }
                }
                catch { }
                this.UpdateStatus("执行完毕。");
                MessageBox.Show("执行完毕！请加载GXGC层进行数据查看。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            

        }

        private void getYlbByGxgc(string xzdm)
        {
            using (ESRI.ArcGIS.ADF.ComReleaser comrelease = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor featureCursor = dltbGxgcClass.Search(null, false);
                comrelease.ManageLifetime(featureCursor);
                IFeature aFeature=null;
                while ((aFeature=featureCursor.NextFeature())!=null)
                {
                    string ydl = FeatureHelper.GetFeatureStringValue(aFeature, "BGQDLBM");
                    string xdl = FeatureHelper.GetFeatureStringValue(aFeature, "BGHDLBM");

                }
                

                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);

            }
        }

        private void InsertAYlb(string xzdm,string ydl, string xdl, double bgmj)
        {
            string sql = "insert into BG_YLB_PFM(xzdm,ydl,xdl,bgmj) values('" + xzdm + "','" + ydl + "','" + xdl + "'," + bgmj + ") ";
            int iret= RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

        private void BgbIn3Ware(string xcode)
        {
            
            //开始入库，从变更表中取
            string sql = "select * from BG_YLB where (xzdm='" + xcode + "' )    ";
            System.Data.DataTable dtYlb = LS_ResultMDBHelper.GetDataTable(sql,"tmp");

            string[] allDlh = new string[] { "D0101","D0102","D0103","D0201","D0202","D0203","D0204",
                 "D0301","D0302","D0303","D0304","D0305","D0306","D0307","D0401","D0402","D0403","D0404","D05H1","D0508","D0601","D0602","D0603",
                 "D0701","D0702","D08H1","D08H2","D0809","D0810","D09","D1001","D1002","D1003","D1004","D1005","D1006","D1007","D1008","D1009",
                 "D1101","D1102","D1103","D1104","D1105","D1106","D1107","D1108","D1109","D1110", "D1201","D1202","D1203","D1204","D1205","D1206","D1207"};

            foreach (string aDlh in allDlh)
            {
                sql = "insert into BG_PHB_T (xzdm,dlh) values('" + xcode + "','" + aDlh + "' ) ";
                int ret= LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                
            }

            foreach (DataRow dr in dtYlb.Rows)
            {
                string ydl = dr["YDL"].ToString();
                string xdl = dr["XDL"].ToString();
                double bgmj=0;
                double.TryParse(dr["BGMJ"].ToString(),out bgmj);
                sql="update BG_PHB_T set D"+xdl+" = "+bgmj+" where DLH='D"+ydl+"' and xzdm='"+xcode+"' ";
                int ret=LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

            }
            //AddLog("入库完毕...");

        }

        private void PutZeroSameDl(string scode)
        {
            string sqlwhere = "  and xzdm='" + scode + "' ";

            //纵向 同种地类变更清0           
            string strSql = " update BG_PHB_T set  D01=0  where left(dlh,3)='D01' ";
            int  bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);

            strSql = "update BG_PHB_T set D00=0  where dlh in ('D0303','D0304','D0402','D0306','D0603','D1105','D1106','D1108')" + sqlwhere;
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql);

            strSql = " update BG_PHB_T set  D02=0  where left(dlh,3)='D02' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D03=0  where  dlh in ('D0301','D0302','D0305','D0307','D03') ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set  D04=0  where  dlh in ('D0401','D0403','D04') ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D05=0  where left(dlh,3)='D05'  ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D06=0 where  dlh in ('D0601','D0602','D06') ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D07=0 where left(dlh,3)='D07' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D08=0 where left(dlh,3)='D08' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D09=0 where left(dlh,3)='D09' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D10=0 where left(dlh,3)='D10' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D11=0 where (left(dlh,3)='D11') and (dlh not in ('D1105','D1106','D1108')) ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = " update BG_PHB_T set  D12=0 where left(dlh,3)='D12' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);

            //横向同种地类变更清0
            strSql = "update BG_PHB_T set D0303=0,D0304=0,D0306=0,D0402=0,D0603=0,D1105-0,D1106=0,D1108=0 where dlh ='D00' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);

            strSql = " update BG_PHB_T set D0101=0,D0102=0,D0103=0 where dlh='D01' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D0201=0,D0202=0,D0203=0,D0204=0 where dlh='D02' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D0301=0,D0302=0,D0305=0,D0307=0 where dlh='D03' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D0401=0,D0403=0,D0404=0 where dlh='D04' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D05=0 where dlh='D05' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D0601=0,D0602=0 where dlh='D06' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D0701=0,D0702=0 where dlh='D07' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D08H1=0,D08H2=0,D0809=0,D0810=0 where dlh='D08' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D09=0 where dlh='D09' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D1001=0,D1002=0,D1003=0,D1004=0,D1005=0,D1006=0,D1007=0,D1008=0,D1009=0 where dlh='D10' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D1101=0,d1102=0,D1103=0,D1104=0,D1107=0,D1109=0,D1110=0 where dlh='D11' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);
            strSql = "update BG_PHB_T set D1201=0,D1202=0,D1203=0,D1204=0,D1205=0,D1206=0,D1207=0 where dlh='D12' ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strSql + sqlwhere);

        }

        private void PutNNZJ2Bgb(string scode) //在变更表生成年内增加
        {
            string[] f12 = new string[] { "D00", "D01", "D02", "D03", "D04","D05",  "D06", "D07", "D08", "D09", "D10","D11","D12" };

            int  bRet = LS_ResultMDBHelper.ExecuteSQLNonquery("delete from BG_PHB_T where DLH='D003'" );

            //        //计算三级级分类的年内增加
            StringBuilder sb = new StringBuilder();
            sb.Append(" insert into  BG_PHB_T(XZDM,dlh, D0101,D0102,D0103,D0201,D0202,D0203,D0204,")
                .Append(" D0301,D0302,D0303,D0304,D0305,D0306,D0307,D0401,D0402,D0403,D0404,D05H1,D0508,D0601,D0602,D0603,")
                .Append(" D0701,D0702,D08H1,D08H2,D0809,D0810,D09,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D1201,D1202,D1203,D1204,D1205,D1206,D1207,D000 ) ")
            .Append("  select '").Append(scode).Append("','D003',")
            .Append(" sum(D0101),sum(D0102),sum(D0103),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
                .Append(" sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05H1),sum(D0508),sum(D0601),sum(D0602),sum(D0603),")
                .Append(" sum(D0701),sum(D0702),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
                .Append(" sum(D1101),sum(d1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207),sum(D000)  ")
                .Append(" from BG_PHB_T ")
            .Append("  where (xzdm='").Append(scode).Append("') and DLH not in ('D001','D002','D003','D004','D00','D01','D02','D03','D04','D05','D06','D07','D08','D10','D11','D12')  ")
            .Append("  group by xzdm ");
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

            ////计算二级级分类年内增加
            for (int i = 0; i < f12.Length; i++)
            {
                string strsql = "select sum(" + f12[i] + ") as mj from  BG_PHB_T where (xzdm='" + scode + "') and  len(dlh)=3 and dlh <>'"+f12[i]+"'  ";
                System.Data.DataTable dt = LS_ResultMDBHelper.GetDataTable(strsql,"tmp");
                foreach (DataRow dr in dt.Rows)
                {
                    double mj = 0;
                    double.TryParse(dr["mj"].ToString(), out mj);
                    strsql = "update BG_PHB_T set " + f12[i] + "=" + mj + " where (xzdm='"
                        + scode + "')  and (Dlh='D003' )  ";
                    bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(strsql);
                }
            }
            

        }

        private void PutNMMj(string scode)
        {
            string[] alldl = new string[]{"D00","D01", "D0101","D0102","D0103","D02","D0201","D0202","D0203","D0204",
                "D03", "D0301","D0302","D0303","D0304","D0305","D0306","D0307","D04","D0401","D0402","D0403","D0404","D05","D05H1","D0508", "D06","D0601","D0602","D0603",
                "D07","D0701","D0702","D08","D08H1","D08H2","D0809","D0810","D09","D10","D1001","D1002","D1003","D1004","D1005","D1006","D1007","D1008","D1009",
                "D11","D1101","D1102","D1103","D1104","D1105","D1106","D1107","D1108","D1109","D1110","D12","D1201","D1202","D1203","D1204","D1205","D1206","D1207" };

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into BG_PHB_T(xzdm,DLH,D00,D01, D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,")
                .Append("D03, D0301,D0302,D0303,D0304,D0305,D0306,D0307,D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508, D06,D0601,D0602,D0603,")
                .Append(" D07,D0701,D0702,D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D11,D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207,D000 )")
                .Append(" select a.XZDM,'D004',");
            foreach (string adl in alldl)
            {
                sb.Append("iif(isnull(a." + adl + "),0,a." + adl + ") - iif(isnull(b." + adl + "),0,b." + adl + ") +iif(isnull(c." + adl + "),0,c." + adl + ") as " + adl + ",");
            }
            sb.Append("iif(isnull(a.D000),0,a.D000)-iif(isnull(b.D000),0,b.D000)+iif(isnull(c.D000),0,c.D000) as D000 ")
            .Append(" from ( BG_PHB_T  a inner join BG_PHB_T b on a.xzdm=b.xzdm )  inner join BG_PHB_T c on b.xzdm=c.xzdm  ")
            .Append(" where  a.dlh='D001' and b.dlh='D002' and c.dlh='D003'  and a.xzdm='" + scode + "' ");
            string sql = sb.ToString();
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
        }

 

        private void Build3Bgb(string xcode)
        {
            string sql = "delete from BG_PHB_T  ";
            int ret= RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            this.UpdateStatus("正在一览表入库汇总...");
            BgbIn3Ware(xcode);


            this.UpdateStatus("正在计算小计合计...");
            //根据二级类 生成一级类的变更流向，此时同类别的也包括在里面
            StringBuilder sb = new StringBuilder();
            //粗了湿地其他小计
            sb.Append(" insert into  BG_PHB_T(XZDM,dlh, D0101,D0102,D0103,D0201,D0202,D0203,D0204,")
                .Append(" D0301,D0302,D0303,D0304,D0305,D0306,D0307,D0401,D0402,D0403,D0404,D05H1,D0508,D0601,D0602,D0603,")
                .Append(" D0701,D0702,D08H1,D08H2,D0809,D0810,D09,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
                .Append(" select '" + xcode + "',left(dlh,3) as dlh,")
                .Append(" sum(D0101),sum(D0102),sum(D0103),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
                .Append(" sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05H1),sum(D0508),sum(D0601),sum(D0602),sum(D0603),")
                .Append(" sum(D0701),sum(D0702),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
                .Append(" sum(D1101),sum(d1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207)  ")
                .Append(" from BG_PHB_T  where xzdm='").Append(xcode).Append("' and (  dlh not in (  'D0303','D0304','D0306','D0402','D0603','D1105','D1108','D1106','D09' ) )")
                .Append(" group by xzdm,left(dlh,3) ");
            int bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

            sb.Clear();
            //计算00 的小计
            sb.Append(" insert into  BG_PHB_T(XZDM,dlh, D0101,D0102,D0103,D0201,D0202,D0203,D0204,")
                .Append(" D0301,D0302,D0303,D0304,D0305,D0306,D0307,D0401,D0402,D0403,D0404,D05H1,D0508,D0601,D0602,D0603,")
                .Append(" D0701,D0702,D08H1,D08H2,D0809,D0810,D09,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D1201,D1202,D1203,D1204,D1205,D1206,D1207 ) ")
                .Append(" select '" + xcode + "','D00',")
                .Append(" sum(D0101),sum(D0102),sum(D0103),sum(D0201),sum(D0202),sum(D0203),sum(D0204),")
                .Append(" sum(D0301),sum(D0302),sum(D0303),sum(D0304),sum(D0305),sum(D0306),sum(D0307),sum(D0401),sum(D0402),sum(D0403),sum(D0404),sum(D05H1),sum(D0508),sum(D0601),sum(D0602),sum(D0603),")
                .Append(" sum(D0701),sum(D0702),sum(D08H1),sum(D08H2),sum(D0809),sum(D0810),sum(D09),sum(D1001),sum(D1002),sum(D1003),sum(D1004),sum(D1005),sum(D1006),sum(D1007),sum(D1008),sum(D1009),")
                .Append(" sum(D1101),sum(d1102),sum(D1103),sum(D1104),sum(D1105),sum(D1106),sum(D1107),sum(D1108),sum(D1109),sum(D1110),sum(D1201),sum(D1202),sum(D1203),sum(D1204),sum(D1205),sum(D1206),sum(D1207)  ")
                .Append(" from BG_PHB_T  where xzdm='").Append(xcode).Append("' and (  dlh  in ( 'D0303','D0304','D0306','D0402','D0603','D1105','D1108','D1106' ) )")
                .Append(" group by xzdm ");
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

           

            sb = new StringBuilder();                      

            //把dl值为 3级分类的数据的年内减少合计 算出来
            sb = new StringBuilder();
            sb.Append(" update BG_PHB_T set D000=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
            .Append(" iif(isnull(D0201),0,D0201)+iif(isnull(D0202),0,D0202)+iif(isnull(D0203),0,D0203)+iif(isnull(D0204),0,D0204)+  ")
            .Append(" iif(isnull(D0301),0,D0301)+iif(isnull(D0302),0,D0302)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+ ")
            .Append(" iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0404),0,D0404)+iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+ ")
            .Append(" iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+ ")
            .Append(" iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D09),0,D09)+")
            .Append(" iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
            .Append(" iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+iif(isnull(D1104),0,D1104)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
            .Append(" iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) ")
            .Append(" where xzdm='").Append(xcode).Append("' ");
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());


            //计算每一行的小计
            sb = new StringBuilder();
            sb.Append(" update BG_PHB_T  set ")
                .Append("  D00=iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0306),0,D0306)+iif(isnull(D0402),0,D0402)+iif(isnull(D0603),0,D0603)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1108),0,D1108),")
                .Append("  D01=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103),")
                .Append("  D02=iif(isnull(D0201),0,D0201)+iif(isnull(D0202),0,D0202)+iif(isnull(D0203),0,D0203)+iif(isnull(D0204),0,D0204), ")
                .Append("  D03=iif(isnull(D0301),0,D0301)+iif(isnull(D0302),0,D0302)+iif(isnull(D0305),0,D0305)+iif(isnull(D0307),0,D0307),")
                .Append("  D04=iif(isnull(D0401),0,D0401)+iif(isnull(D0403),0,D0403)+iif(isnull(D0404),0,D0404),")
                .Append("  D05=iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508),")
                .Append("  D06=iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602),")
                .Append("  D07=iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702), ")
                .Append("  D08=iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810), ")
                .Append("  D10=iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009),")
                .Append("  D11=iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+iif(isnull(D1104),0,D1104)+iif(isnull(D1107),0,D1107)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110),")
                .Append("  D12=iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) ")
                .Append("  where xzdm='").Append(xcode).Append("' ");
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());

           
            PutZeroSameDl(xcode);//同种地类变更清0

            this.UpdateStatus("正在计算年初年末面积...");
            ////插入控制面积作为年初面积
            if (PutCmj2Bgb(xcode) == 0)
            {
                //如果没有年初面积
                sql = "insert into BG_PHB_T (XZDM,DLH) values('"
                     + xcode + "','D001') ";
                bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
                sql = "insert into BG_PHB_T (XZDM,DLH) values('"
                      + xcode + "','D004') ";
                bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

                PutNNZJ2Bgb(xcode);
                PutNNJS2Bgb(xcode);

            }
            else
            {
                PutNNZJ2Bgb(xcode);
                PutNNJS2Bgb(xcode);
                PutNMMj(xcode);
            }

            

           
            


            sql = " delete from BG_PHB  ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            sql = " insert into  BG_PHB select * from  BG_PHB_T  ";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            

            //加入排序字段
            sql = "update BG_PHB,BG_DLSORT set BG_PHB.SORT=BG_DLSORT.SORT where BG_DLSORT.DLH=BG_PHB.DLH";
            bRet = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);

        }

        //scode是6位县代码
        private int  PutCmj2Bgb(string sCode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into BG_PHB_T( XZDM,dlh,D00,D01, D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,")
                .Append("D03, D0301,D0302,D0303,D0304,D0305,D0306,D0307,D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508, D06,D0601,D0602,D0603,")
                .Append(" D07,D0701,D0702,D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D11,D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207,D000 ) ")
                .Append(" select '" + sCode + "','D001',D00,D01, D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204,")
                .Append("D03, D0301,D0302,D0303,D0304,D0305,D0306,D0307,D04,D0401,D0402,D0403,D0404,D05,D05H1,D0508,D06,D0601,D0602,D0603,")
                .Append(" D07,D0701,D0702,D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009,")
                .Append(" D11,D1101,d1102,D1103,D1104,D1105,D1106,D1107,D1108,D1109,D1110,D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207,TDZMJ from HZ_ZL_BZ where ZLDWDM='" + sCode + "' ");
            string sql = sb.ToString();
            int iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sql);
            return iret;

        }

        //计算年内减少
        private void PutNNJS2Bgb(string scode)
        {
            string[] Fl2 = new string[] {"D00", "D01", "D02", "D03", "D04", "D05", "D06", "D07", "D08","D09", "D10", "D11" ,"D12"};
            

            string strsql = "insert into bg_phb_T(xzdm,DLH) values ('" + scode + "','D002' ) ";
            int iret= RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery(strsql);

            //计算3级地类年内减少
            string sql = " select * from bg_phb_t where (xzdm='" + scode + "')  and (DLH not in ('D001','D002','D003','D004','D00','D01','D02','D03','D04','D06','D07','D08','D10','D11','D12') )  order by DLH ";
            System.Data.DataTable dt = LS_ResultMDBHelper.GetDataTable(sql,"tmp");
            foreach (DataRow dr in dt.Rows)
            {
                double mj = 0;
                double.TryParse(dr["D000"].ToString(), out mj);
                string dlh = dr["DLH"].ToString().Trim().ToUpper();
                //把纵向的地类号转化为根横向 的字段一样
                strsql = "update bg_phb_t set " + dlh + "=" + mj
                 + " where (XZDM='" + scode + "') and (DLH='D002') ";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(strsql);

            }

            //计算二级分类年内减少 合计
            StringBuilder sb = new StringBuilder();
            sb.Append(" update bg_phb_t set D000=iif(isnull(D0101),0,D0101)+iif(isnull(D0102),0,D0102)+iif(isnull(D0103),0,D0103)+")
            .Append(" iif(isnull(D0201),0,D0201)+iif(isnull(D0202),0,D0202)+iif(isnull(D0203),0,D0203)+iif(isnull(D0204),0,D0204)+  ")
            .Append(" iif(isnull(D0301),0,D0301)+iif(isnull(D0302),0,D0302)+iif(isnull(D0303),0,D0303)+iif(isnull(D0304),0,D0304)+iif(isnull(D0305),0,D0305)+iif(isnull(D0306),0,D0306)+iif(isnull(D0307),0,D0307)+ ")
            .Append(" iif(isnull(D0401),0,D0401)+iif(isnull(D0402),0,D0402)+iif(isnull(D0403),0,D0403)+iif(isnull(D0404),0,D0404)+iif(isnull(D05H1),0,D05H1)+iif(isnull(D0508),0,D0508)+ ")
            .Append(" iif(isnull(D0601),0,D0601)+iif(isnull(D0602),0,D0602)+iif(isnull(D0603),0,D0603)+iif(isnull(D0701),0,D0701)+iif(isnull(D0702),0,D0702)+ ")
            .Append(" iif(isnull(D08H1),0,D08H1)+iif(isnull(D08H2),0,D08H2)+iif(isnull(D0809),0,D0809)+iif(isnull(D0810),0,D0810)+iif(isnull(D09),0,D09)+")
            .Append(" iif(isnull(D1001),0,D1001)+iif(isnull(D1002),0,D1002)+iif(isnull(D1003),0,D1003)+iif(isnull(D1004),0,D1004)+iif(isnull(D1005),0,D1005)+iif(isnull(D1006),0,D1006)+iif(isnull(D1007),0,D1007)+iif(isnull(D1008),0,D1008)+iif(isnull(D1009),0,D1009)+")
            .Append(" iif(isnull(D1101),0,D1101)+iif(isnull(D1102),0,D1102)+iif(isnull(D1103),0,D1103)+iif(isnull(D1104),0,D1104)+iif(isnull(D1105),0,D1105)+iif(isnull(D1106),0,D1106)+iif(isnull(D1107),0,D1107)+iif(isnull(D1108),0,D1108)+iif(isnull(D1109),0,D1109)+iif(isnull(D1110),0,D1110)+")
            .Append(" iif(isnull(D1201),0,D1201)+iif(isnull(D1202),0,D1202)+iif(isnull(D1203),0,D1203)+iif(isnull(D1204),0,D1204)+iif(isnull(D1205),0,D1205)+iif(isnull(D1206),0,D1206)+iif(isnull(D1207),0,D1207) ")
            .Append(" where xzdm='").Append(scode).Append("'  and DLH='D002' ");
            iret = LS_ResultMDBHelper.ExecuteSQLNonquery(sb.ToString());


            //计算2级地类年内减少
            for (int i = 0; i < Fl2.Length; i++)
            {
                strsql = "select  iif(isnull(D00),0,D00)+iif(isnull(D01),0,D01)+iif(isnull(D02),0,D02)+iif(isnull(D03),0,D03)+"
                + "iif(isnull(D04),0,D04)+iif(isnull(D05),0,D05)+iif(isnull(D06),0,D06)+iif(isnull(D07),0,D07)+"
                + "iif(isnull(D08),0,D08)+iif(isnull(D09),0,D09)+iif(isnull(D10),0,D10)+iif(isnull(D11),0,D11)+iif(isnull(D12),0,D12)  as mj from bg_phb_t "
                + "  where (xzdm='" + scode + "') and (DLH='" + Fl2[i] + "' ) ";
                DataRow firstRow= LS_ResultMDBHelper.GetDataRow(strsql,"tmp");
                
                double mj = 0;
                if (firstRow != null)
                {
                    string sMj = firstRow[0].ToString();
                    double.TryParse(sMj, out mj);
                }


                strsql = " update  bg_phb_t  set " + Fl2[i] + "=" + mj + "  where (xzdm='" + scode + "')  and (DLH='D002' ) ";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(strsql);
                //同时更新最后一列的年内减少合计
                strsql = " update  bg_phb_t set D000=" + mj + " where (xzdm='" + scode + "') and  (DLH='" + Fl2[i] + "') ";
                iret = LS_ResultMDBHelper.ExecuteSQLNonquery(strsql);


            }


        }

        private void OutExcel(string xzdm,string destFile)
        {
            string srcFile = RCIS.Global.AppParameters.TemplatePath + "\\1土地利用现状变更表.xls";
            System.IO.File.Copy(srcFile, destFile);
            string sql = "select D00,D0303,D0304,D0306, D0402,D0603,D1105,D1106,D1108,D01, D0101,D0102,D0103,D02,D0201,D0202,D0203,D0204," +
            " D03,D0301,D0302,D0305,D0307,D04, D0401,D0403,D0404,D05,D05H1,D0508, D06,D0601,D0602," +
            " D07, D0701,D0702,D08,D08H1,D08H2,D0809,D0810,D09,D10,D1001,D1002,D1003,D1004,D1005,D1006,D1007,D1008,D1009," +
            " D11,D1101,d1102,D1103,D1104,D1107,D1109,D1110,D12,D1201,D1202,D1203,D1204,D1205,D1206,D1207,D000  from BG_PHB "
                + " where xzdm='" + xzdm + "'  order by SORT  ";
            System.Data.DataTable dt = LS_ResultMDBHelper.GetDataTable(sql, "tmp");
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

            int startRow = 7;
            int startCol = 5;
            int endCol = 74;


            try
            {
                #region 导出sheet1的数据
                cells[3, 2].PutValue(xzdm);

                for (int iRow = startRow; iRow < startRow + dt.Rows.Count; iRow++)
                {
                    DataRow dr = dt.Rows[iRow - startRow];
                    for (int iCol = startCol; iCol < endCol; iCol++)
                    {
                        double mj = 0.00;


                        double.TryParse(dr[iCol - startCol].ToString(), out mj);
                        if (mj > 0)
                        {

                        }
                        cells[iRow, iCol].PutValue(mj);
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
                //ExcelRS.Quit();

            }

        }


        private double getZmj(string tabName)
        {
            System.Data.DataRow arow = LS_ResultMDBHelper.GetDataRow("select sum(bgmj) as zmj from " + tabName, "tmp");
            double mj = 0;
            if (arow != null)
            {
                string smj = arow["zmj"].ToString();
                double.TryParse(smj, out mj);
            }
            return mj;

        }


        /// <summary>
        /// 第一个是平方米合计 得到的公顷总面积，第二个是 每个分项算成公顷后累加得到的总面积
        /// </summary>
        /// <param name="zmj"></param>
        /// <param name="gqzmj"></param>
        private void gqTP(double zmj,double gqzmj)
        {
            System.Data.DataTable gqTable = LS_ResultMDBHelper.GetDataTable("select ydl,xdl,bgmj from BG_YLB order by bgmj desc ","tmp");
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
                    LS_ResultMDBHelper.ExecuteSQLNonquery("update  BG_YLB set bgmj=" + mj + " where ydl='" + ydl + "' and xdl='" + xdl + "' ");
                }
            }
            else if (zmj < gqzmj)
            {
                for (int i = 0; i < iLen; i++)
                {
                    System.Data.DataRow arow = gqTable.Rows[i];
                    string ydl = arow["ydl"].ToString();
                    string xdl = arow["xdl"].ToString();
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
                    LS_ResultMDBHelper.ExecuteSQLNonquery("update  BG_YLB set bgmj=" + mj + " where ydl='" + ydl + "' and xdl='" + xdl + "' ");
                }
            }

        }


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (dltbGxgcClass == null) return;            

            IFeatureClass pXZQClass = null;
            string xzdm = "";
            try
            {
                pXZQClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQClass != null)
            {
                IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                if (firstFea != null)
                {
                    xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                }
            }
            if (xzdm.Length > 6)
            {
                xzdm = xzdm.Substring(0, 6);
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            dlg.FileName = "(" + xzdm + ")土地利用现状变更表.xls";
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
                RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery("delete from BG_YLB_PFM");

                this.UpdateStatus("正在提取一览表...");
                #region  //根据更新过程 提取一栏表
                IFeatureCursor pCursor = this.dltbGxgcClass.Search(null, false);
                try
                {
                    IFeature aGxgcFea = null;
                    while ((aGxgcFea = pCursor.NextFeature()) != null)
                    {
                        //判断原地类与现地类是否一致，
                        string ydl = FeatureHelper.GetFeatureStringValue(aGxgcFea, "DLBM").Trim();
                        string xdl = FeatureHelper.GetFeatureStringValue(aGxgcFea, "BGHDLBM").Trim();
                        double tbmj = FeatureHelper.GetFeatureDoubleValue(aGxgcFea, "BGHTBMJ");
                        double bghdlmj = FeatureHelper.GetFeatureDoubleValue(aGxgcFea, "BGHTBDLMJ");
                        double kcxs = FeatureHelper.GetFeatureDoubleValue(aGxgcFea, "KCXS");
                        double kcxs2 = FeatureHelper.GetFeatureDoubleValue(aGxgcFea, "BGHKCXS");
                        string kcdlbm = FeatureHelper.GetFeatureStringValue(aGxgcFea, "KCDLBM");
                        if (kcdlbm == "")
                            kcdlbm = "1203"; //默认田坎
                        //如果是耕地，田坎单算
                        #region   //如果一致，如果是耕地，判断扣除系数是否一致，如果不一致，
                        if (ydl.StartsWith("01") && xdl.StartsWith("01"))
                        {
                            //都是耕地，页存在内部变化情况
                            if (ydl != xdl)
                            {
                                InsertAYlb(xzdm, ydl, xdl, bghdlmj);
                            }

                            if (kcxs > kcxs2)
                            {

                                //说明田坎变耕地了，扣除系数变小了
                                double bgmj = tbmj * (kcxs - kcxs2);
                                InsertAYlb(xzdm, kcdlbm, xdl, bgmj);
                            }
                            else if (kcxs2 > kcxs)
                            {
                                //说明 耕地变田坎，扣除系数大了
                                double bgmj = tbmj * (kcxs2 - kcxs);
                                InsertAYlb(xzdm, ydl, kcdlbm, bgmj);
                            }

                        }
                        #endregion
                        else if (ydl.StartsWith("01") && !xdl.StartsWith("01"))
                        {
                            //如果原来是耕地，变为非耕地，地类变化，同时，如果扣除系数原来不是空，则意味着部分田坎页变成了 现地类
                            InsertAYlb(xzdm, ydl, xdl, bghdlmj);

                            if (kcxs > 0)
                            {
                                double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                                InsertAYlb(xzdm, kcdlbm, xdl, kcmj);
                            }

                        }
                        else if (!ydl.StartsWith("01") && xdl.StartsWith("01"))
                        {
                            //非耕地变为了耕地
                            InsertAYlb(xzdm, ydl, xdl, bghdlmj);
                            //如果现在扣除系数不为空，则 意味着 原来的地类变为了田坎
                            if (kcxs2 > 0)
                            {
                                double kcmj = MathHelper.RoundEx(tbmj * kcxs2, 2);
                                InsertAYlb(xzdm, ydl, kcdlbm, kcmj);
                            }
                        }
                        else if (!ydl.StartsWith("01") && !xdl.StartsWith("01"))
                        {
                            //不是耕地的，地类发生变化
                            if (ydl != xdl)
                            {
                                InsertAYlb(xzdm, ydl, xdl, bghdlmj);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
                #endregion


                //开始进行平衡表计算
                this.UpdateStatus("正在将一览表转化为公顷并调平...");
                //一览表 汇总候变为公顷
                RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery("delete from BG_YLB");
                RCIS.Database.LS_ResultMDBHelper.ExecuteSQLNonquery("insert into BG_YLB(XZDM,YDL,XDL,BGMJ) select XZDM,YDL,XDL,sum(bGMJ)/10000 from BG_YLB_PFM group by xzdm,ydl,xdl ");
                

                //记录平方米的 总变更面积
            
                double zmj = getZmj("BG_YLB_PFM"); //平方米的 总面积
                zmj = MathHelper.RoundEx(zmj / 10000, 2);
                double gqZmj = getZmj("BG_YLB");
                this.gqTP(zmj, gqZmj); //经过调平后，公顷一览表中 的合计数 相等了
                
                
                this.UpdateStatus("正在进行平衡表计算...");
                Build3Bgb(xzdm);

                this.UpdateStatus("正在导出Excel...");

                OutExcel(xzdm, destFile);
                this.UpdateStatus("导出完毕！");

                this.Cursor = Cursors.Default;
                MessageBox.Show("计算并导出完毕！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            

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
            catch (Exception ex){
                return -1;
            
            }

        }


    }
    
}
