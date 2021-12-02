using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using System.IO;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using RCIS.Utility;
using ESRI.ArcGIS.DataSourcesOleDB;
using System.Data.OleDb;
using RCIS.GISCommon;
using TDDC3D.edit;

namespace TDDC3D.gengxin
{
    public partial class FrmCreateProcess : Form
    {

        public IMap currMap = null;
        public IWorkspace currWs = null;
        string xzdm = "";
        public FrmCreateProcess()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //try 
            //{
                if (checkDLTB.Checked == false && checkCJDCQ.Checked == false && checkXZQ.Checked == false)
                {
                    MessageBox.Show("请至少选择一个将要提取过程的图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                xzdm = txtXian.Text.Trim();
                bgData = ""+dateEdit1.Text+"/12/31";
            //2021年10月15日09:45:34 调用和修改都在类里边实现，没有调用和修改窗体里的方法。
                TDDC3D.gengxin.LsGxClass gx = new LsGxClass();
                gx.xzdm = xzdm;
                gx.bgData = bgData;
                //gx.pMapCtl = pMapCtl;
                gx.info = info;
                if (checkDLTB.Checked)
                {
                    double mj = 0;
                    double.TryParse(txtMj.Text, out mj);
                    gx.ProduceDLTBGXGC(mj,checkEdit1.Checked);
                }
                if (checkCJDCQ.Checked)
                {
                    gx.ProduceCJDCQGXGC();
                }
                if (checkXZQ.Checked)
                {
                    gx.ProduceXZQGXGC();
                }
                MessageBox.Show("完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, ex.ToString());
            //    return;
            //}
            
        }

        string bgData = "";
        private void FrmCreateProcess_Load(object sender, EventArgs e)
        {
            IFeatureWorkspace pFW = currWs as IFeatureWorkspace;
            IFeatureClass pDLTB = pFW.OpenFeatureClass("DLTB");
            IFeatureClass pDLTBGX = pFW.OpenFeatureClass("DLTBGX");
            IFeatureClass pXZQ = pFW.OpenFeatureClass("XZQ");
            IFeatureClass pXZQGX = pFW.OpenFeatureClass("XZQGX");
            IFeatureClass pCJDCQ = pFW.OpenFeatureClass("CJDCQ");
            IFeatureClass pCJDCQGX = pFW.OpenFeatureClass("CJDCQGX");
            if (pDLTB.FeatureCount(null) == 0 || pDLTBGX.FeatureCount(null) == 0)
                checkDLTB.Enabled = false;
            if (pXZQ.FeatureCount(null) == 0 || pXZQGX.FeatureCount(null) == 0)
                checkXZQ.Enabled = false;
            if (pCJDCQ.FeatureCount(null) == 0 || pCJDCQGX.FeatureCount(null) == 0)
                checkCJDCQ.Enabled = false;


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


            //县代码
            IFeatureClass pXZQClass = null;
            try
            {
                pXZQClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQClass != null)
            {
                List<string> dms = FeatureHelper.GetDMMCDicByQueryDef(this.currWs as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                txtXian.Properties.Items.Clear();
                foreach (string dm in dms)
                {
                    txtXian.Properties.Items.Add(dm);
                }
                if (txtXian.Properties.Items.Count > 0) txtXian.SelectedIndex = 0;
            }
        }

        //后边都是窗体里的方法，2021年10月15日09:47:09之后不进行使用和修改，尽量不要删除。
        #region

        private void ProduceCJDCQJXGX()
        {
            //变化村级调查区
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pCJDCQJX = pFeaWorkspace.OpenFeatureClass("CJDCQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;

            (pCJDCQJX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取村级调查区界线");

            string tmpPath = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;
            if (Directory.Exists(tmpPath))
            {
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                pEnumDataset.Reset();
                IDataset pDataset;
                while ((pDataset = pEnumDataset.Next()) != null)
                {
                    pDataset.Delete();
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            //IFeatureClass mFeaClass = GPUpdate(filePath + "\\TDDC\\XZQ",filePath + "\\TDGX\\XZQGXGC");
            //IFeatureClass allFeaClass = GPPolygonToLineByFeatureclass((mFeaClass as IDataset).Workspace.PathName + "\\" + (mFeaClass as IDataset).Name+".shp");
            IFeatureClass pGXGCFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\CJDCQGXGC", tmpWS);
            IFeatureClass pGXFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\CJDCQGX", tmpWS);
            //IFeatureClass pSDFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDDC\\CJDCQ");
            IFeatureClass pSDFeatureclass = pFeaWorkspace.OpenFeatureClass("CJDCQJX");

            IFeatureClass mFeaClass = GPUpdate(filePath + "\\TDDC\\CJDCQ", filePath + "\\TDGX\\CJDCQGXGC");
            IFeatureClass allFeaClass = GPPolygonToLineByFeatureclass((mFeaClass as IDataset).Workspace.PathName + "\\" + (mFeaClass as IDataset).Name + ".shp", tmpWS);
            IFeatureCursor allFeaCursor = allFeaClass.Update(null, true);
            IFeature allFeature;
            while ((allFeature = allFeaCursor.NextFeature()) != null)
            {
                ISpatialFilter SpaFil = new SpatialFilterClass();
                SpaFil = new SpatialFilterClass();
                SpaFil.Geometry = allFeature.ShapeCopy;
                SpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                SpaFil.SpatialRelDescription = "T********";
                IFeatureCursor cursor = pGXGCFeatureclass.Search(SpaFil, true);
                IFeature feature = cursor.NextFeature();

                bool bl = false;
                while (feature != null)
                {
                    bl = GetIntersectLength(allFeature, feature);
                    if (bl == true)
                        break;
                    feature = cursor.NextFeature();
                }
                if (bl == false)
                {
                    allFeaCursor.DeleteFeature();
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(cursor);

            }
            allFeaCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(allFeaCursor);
            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pFeaWorkspace.OpenFeatureClass("CJDCQJX"), "BSM") + 1;
            UpdateStatus("正在生成村级调查区界线更新层");
            string cjdcqjcgxYSDM = GetValueFromMDBByLayerName("CJDCQJXGX");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pCJDCQJX.Insert(true);
                comRel.ManageLifetime(pInsert);
                using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pFeaCursor = pSDFeatureclass.Search(null, true);
                    comRel2.ManageLifetime(pFeaCursor);
                    IFeature pFeature;
                    ISpatialFilter pSpaFil = null;
                    while ((pFeature = pFeaCursor.NextFeature()) != null)
                    {
                        pSpaFil = new SpatialFilterClass();
                        pSpaFil.Geometry = pFeature.ShapeCopy;
                        pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpaFil.SpatialRelDescription = "T********";
                        IFeatureCursor pGXGCFeatureCursor = allFeaClass.Search(pSpaFil, false);
                        IFeature pGXGCFeature;
                        IFeature tempFeature = null;
                        bool B = false;
                        int tmp = 0;
                        while ((pGXGCFeature = pGXGCFeatureCursor.NextFeature()) != null)
                        {
                            B = false;
                            B = GetIntersectLength(pFeature, pGXGCFeature);
                            if (B == true)
                            {
                                if (tmp == 0)
                                {
                                    tempFeature = pGXGCFeature;
                                    tmp += 1;
                                }
                                else
                                {
                                    pSpaFil = new SpatialFilterClass();
                                    pSpaFil.Geometry = pGXGCFeature.ShapeCopy;
                                    pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                                    pSpaFil.SpatialRelDescription = "T********";
                                    IFeatureCursor pGXFeaCursor = pGXFeatureclass.Search(pSpaFil, true);
                                    IFeature pGXFeature;
                                    while ((pGXFeature = pGXFeaCursor.NextFeature()) != null)
                                    {
                                        if (GetIntersectLength(pGXGCFeature, pGXFeature) == true)
                                        {
                                            IFeatureBuffer pFeaBuffer = pCJDCQJX.CreateFeatureBuffer();
                                            int pFieldNum = pCJDCQJX.FindField("BGXW");
                                            pFeaBuffer.Shape = pGXGCFeature.ShapeCopy;
                                            pFeaBuffer.set_Value(pCJDCQJX.FindField("BSM"), maxBSM++);
                                            pFeaBuffer.set_Value(pCJDCQJX.FindField("YSDM"), cjdcqjcgxYSDM);
                                            pFeaBuffer.set_Value(pCJDCQJX.FindField("JXLX"), "660200");
                                            pFeaBuffer.set_Value(pCJDCQJX.FindField("JXXZ"), "600001");
                                            pFeaBuffer.set_Value(pCJDCQJX.FindField("GXSJ"), bgData);
                                            pFeaBuffer.set_Value(pFieldNum, "3");//新增
                                            pInsert.InsertFeature(pFeaBuffer);
                                            break;
                                        }
                                    }
                                    RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeaCursor);
                                    tmp += 1;
                                }
                            }
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCFeatureCursor);

                        if (tmp == 1)
                        {
                            IFeatureCursor pGXCursor = pGXFeatureclass.Search(pSpaFil, true);
                            IFeature pGXFea;
                            bool b = false;
                            while ((pGXFea = pGXCursor.NextFeature()) != null)
                            {
                                b = GetIntersectLength(pGXFea, pFeature);
                                if (b == true)
                                {
                                    IFeatureBuffer pFeaBuffer = pCJDCQJX.CreateFeatureBuffer();
                                    int pFieldNum = pCJDCQJX.FindField("BGXW");
                                    pFeaBuffer.Shape = pFeature.ShapeCopy;
                                    pFeaBuffer.set_Value(pCJDCQJX.FindField("BSM"), pFeature.get_Value(pSDFeatureclass.FindField("BSM")).ToString());
                                    pFeaBuffer.set_Value(pCJDCQJX.FindField("YSDM"), cjdcqjcgxYSDM);
                                    pFeaBuffer.set_Value(pCJDCQJX.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                                    pFeaBuffer.set_Value(pCJDCQJX.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                                    pFeaBuffer.set_Value(pCJDCQJX.FindField("GXSJ"), bgData);
                                    pFeaBuffer.set_Value(pFieldNum, "4");//无变化
                                    pInsert.InsertFeature(pFeaBuffer);
                                    break;
                                }
                            }
                            if (b == false)
                            {
                                IFeatureBuffer pFeaBuffer = pCJDCQJX.CreateFeatureBuffer();
                                int pFieldNum = pCJDCQJX.FindField("BGXW");
                                pFeaBuffer.Shape = pFeature.ShapeCopy;
                                pFeaBuffer.set_Value(pCJDCQJX.FindField("BSM"), pFeature.get_Value(pSDFeatureclass.FindField("BSM")).ToString());
                                pFeaBuffer.set_Value(pCJDCQJX.FindField("YSDM"), cjdcqjcgxYSDM);
                                pFeaBuffer.set_Value(pCJDCQJX.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                                pFeaBuffer.set_Value(pCJDCQJX.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                                pFeaBuffer.set_Value(pCJDCQJX.FindField("GXSJ"), bgData);
                                pFeaBuffer.set_Value(pFieldNum, "0");//灭失
                                pInsert.InsertFeature(pFeaBuffer);
                            }

                        }
                        if (tmp > 1)
                        {
                            IFeatureBuffer pFeaBuffer = pCJDCQJX.CreateFeatureBuffer();
                            int pFieldNum = pCJDCQJX.FindField("BGXW");
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pCJDCQJX.FindField("BSM"), pFeature.get_Value(pSDFeatureclass.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pCJDCQJX.FindField("YSDM"), cjdcqjcgxYSDM);
                            pFeaBuffer.set_Value(pCJDCQJX.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                            pFeaBuffer.set_Value(pCJDCQJX.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                            pFeaBuffer.set_Value(pCJDCQJX.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFieldNum, "0");//灭失
                            pInsert.InsertFeature(pFeaBuffer);

                            pSpaFil = new SpatialFilterClass();
                            pSpaFil.Geometry = tempFeature.ShapeCopy;
                            pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                            pSpaFil.SpatialRelDescription = "T********";
                            IFeatureCursor pGXFeatureCursor = pGXFeatureclass.Search(pSpaFil, true);
                            IFeature pGXFeature;
                            while ((pGXFeature = pGXFeatureCursor.NextFeature()) != null)
                            {
                                if (GetIntersectLength(tempFeature, pGXFeature) == true)
                                {
                                    IFeatureBuffer pFeatureBuffer = pCJDCQJX.CreateFeatureBuffer();
                                    pFeatureBuffer.Shape = tempFeature.ShapeCopy;
                                    pFeatureBuffer.set_Value(pCJDCQJX.FindField("BSM"), maxBSM++);
                                    pFeatureBuffer.set_Value(pCJDCQJX.FindField("YSDM"), cjdcqjcgxYSDM);
                                    pFeatureBuffer.set_Value(pCJDCQJX.FindField("JXLX"), "660200");
                                    pFeatureBuffer.set_Value(pCJDCQJX.FindField("JXXZ"), "600001");
                                    pFeatureBuffer.set_Value(pCJDCQJX.FindField("GXSJ"), bgData);
                                    pFeatureBuffer.set_Value(pFieldNum, "3");//新增
                                    pInsert.InsertFeature(pFeatureBuffer);
                                    break;
                                }
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureCursor);

                        }
                    }
                    IFeatureCursor pCursor = pGXGCFeatureclass.Search(null, true);
                    IFeature pFea;
                    while ((pFea = pCursor.NextFeature()) != null)
                    {
                        pSpaFil = new SpatialFilterClass();
                        pSpaFil.Geometry = pFea.ShapeCopy;
                        pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpaFil.SpatialRelDescription = "T********";
                        IFeatureCursor pSDCursor = pSDFeatureclass.Search(pSpaFil, true);
                        IFeature pSD = pSDCursor.NextFeature();
                        if (pSD != null)
                        {
                            if (GetIntersectLength(pSD, pFea) == false)
                                pSD = null;
                        }
                        IFeatureCursor pGXCursor = pGXFeatureclass.Search(pSpaFil, true);
                        IFeature pGX = pGXCursor.NextFeature();
                        if (pSD == null && pGX != null)
                        {
                            IFeatureBuffer pFeatureBuffer = pCJDCQJX.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = pFea.ShapeCopy;
                            pFeatureBuffer.set_Value(pCJDCQJX.FindField("BSM"), maxBSM++);
                            pFeatureBuffer.set_Value(pCJDCQJX.FindField("YSDM"), cjdcqjcgxYSDM);
                            pFeatureBuffer.set_Value(pCJDCQJX.FindField("JXLX"), "660200");
                            pFeatureBuffer.set_Value(pCJDCQJX.FindField("JXXZ"), "600001");
                            pFeatureBuffer.set_Value(pCJDCQJX.FindField("GXSJ"), bgData);
                            int pFieldNum = pCJDCQJX.FindField("BGXW");
                            pFeatureBuffer.set_Value(pFieldNum, "3");//新增
                            pInsert.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSDCursor);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pGXCursor);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

                }
                pInsert.Flush();
            }




            (pGXGCFeatureclass as IDataset).Delete();
            (pGXFeatureclass as IDataset).Delete();
            //(pSDFeatureclass as IDataset).Delete();
            (mFeaClass as IDataset).Delete();
            (allFeaClass as IDataset).Delete();

            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "BGXW='4'";
            IFeatureCursor Cursor = pCJDCQJX.Update(pQF, true);
            IFeature pJX;
            while ((pJX = Cursor.NextFeature()) != null)
            {
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = pJX.ShapeCopy;
                pSF.WhereClause = "BGXW='3'";
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pcursor = pCJDCQJX.Search(pSF, true);
                IFeature pFea;
                while ((pFea = pcursor.NextFeature()) != null)
                {
                    bool b = GetIntersectLength(pJX, pFea);
                    if (b == true)
                    {
                        pJX.set_Value(pCJDCQJX.FindField("BGXW"), 0);
                        Cursor.UpdateFeature(pJX);
                        continue;
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pcursor);
            }
            Cursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(Cursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQJX);

            UpdateStatus("村级调查区界线更新层生成完毕");
            //MessageBox.Show("村级调查区界线更新层生成完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CJDCQJX()
        {
            //0（灭失）、3（新增）、4（无变化）
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pCJDCQJXGX = pFeaWorkspace.OpenFeatureClass("CJDCQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            (pCJDCQJXGX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取村级调查区界线");
            List<string> IDwbh = new List<string>();
            string cjdcqjcgxYSDM = GetValueFromMDBByLayerName("CJDCQJXGX");
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = DeleteAndNewTmpGDB();
            //if (Directory.Exists(tmp))
            //{
            //    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            //    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
            //    pEnumDataset.Reset();
            //    IDataset pDataset;
            //    while ((pDataset = pEnumDataset.Next()) != null)
            //    {
            //        pDataset.Delete();
            //    }
            //}
            //else
            //{
            //    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
            //    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
            //    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            //}
            //IFeatureClass pSDFeatureClass = (tmpWS as IFeatureWorkspace).CreateFeatureClass("sdtb", pDLTB.Fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", null);

            IFeatureClass pGXJX = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\CJDCQGX", tmpWS);
            IFeatureClass pCJDCQJX = pFeaWorkspace.OpenFeatureClass("CJDCQJX");
            IFeatureClass pCJDCQGX = pFeaWorkspace.OpenFeatureClass("CJDCQGX");
            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pCJDCQJX, "BSM") + 1;
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCJDCQGX);
            IRelationalOperator pRelOperator = pGeo as IRelationalOperator;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = pGeo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.GeometryField = pCJDCQJX.ShapeFieldName;
            IFeatureCursor pFeatureCursor = pCJDCQJX.Search(pSF, true);
            IFeature pFeature;
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pGXJX;
            IIdentify pIdentify = pLayer as IIdentify;
            IFeatureCursor pInsert = pCJDCQJXGX.Insert(true);
            while ((pFeature = pFeatureCursor.NextFeature()) != null)
            {
                if (!pRelOperator.Contains(pFeature.ShapeCopy) && !pRelOperator.Crosses(pFeature.ShapeCopy))
                    continue;
                bool b = false;
                IFeatureBuffer pBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                pBuffer.Shape = pFeature.ShapeCopy;
                pBuffer.set_Value(pBuffer.Fields.FindField("BSM"), pFeature.get_Value(pFeature.Fields.FindField("BSM")));
                pBuffer.set_Value(pBuffer.Fields.FindField("YSDM"), pFeature.get_Value(pFeature.Fields.FindField("YSDM")));
                pBuffer.set_Value(pBuffer.Fields.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                pBuffer.set_Value(pBuffer.Fields.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                pBuffer.set_Value(pBuffer.Fields.FindField("GXSJ"), bgData);
                IRelationalOperator pRel = (pFeature.ShapeCopy) as IRelationalOperator;
                IArray pArray = pIdentify.Identify(pFeature.ShapeCopy);
                if (pArray != null)
                {
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pFea = pRow.Row as IFeature;
                        if (pRel.Equals(pFea.ShapeCopy))
                        {
                            IDwbh.Add(pFea.OID.ToString());
                            b = true;
                            break;
                        }
                    }
                }
                if (b == true)
                    pBuffer.set_Value(pBuffer.Fields.FindField("BGXW"), "4");
                else
                    pBuffer.set_Value(pBuffer.Fields.FindField("BGXW"), "0");
                pInsert.InsertFeature(pBuffer);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureCursor);
            string where = "";
            for (int j = 0; j < IDwbh.Count; j++)
            {
                where += "OBJECTID<>" + IDwbh[j] + " and ";
            }
            if (where.Length > 0)
                where = where.Substring(0, where.Length - 4);
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            IFeatureCursor pCursor = pGXJX.Search(pQF, true);
            IFeature pF;
            while ((pF = pCursor.NextFeature()) != null)
            {
                IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                pFeaBuffer.Shape = pF.ShapeCopy;
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), maxBSM++);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), "660200");
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), "600001");
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "3");
                pInsert.InsertFeature(pFeaBuffer);

            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            (pGXJX as IDataset).Delete();
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXJX);
            RCIS.Utility.OtherHelper.ReleaseComObject(tmpWS);

        }

        private void CJDCQJX1()
        {
            //0（灭失）、3（新增）、4（无变化）
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pCJDCQJXGX = pFeaWorkspace.OpenFeatureClass("CJDCQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            (pCJDCQJXGX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取村级调查区界线");
            List<string> IDwbh = new List<string>();
            string cjdcqjcgxYSDM = GetValueFromMDBByLayerName("CJDCQJXGX");
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = DeleteAndNewTmpGDB();

            IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("CJDCQGX");
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);

            bool updata = RCIS.GISCommon.GpToolHelper.Update(filePath + "\\TDDC\\CJDCQ", filePath + "\\TDGX\\CJDCQGX", tmpWS.PathName + "\\cjdcqnmk");

            IFeatureClass pGXJX = GPPolygonToLineByFeatureclass(tmpWS.PathName + "\\cjdcqnmk", tmpWS);
            IFeatureClass pCJDCQJX = pFeaWorkspace.OpenFeatureClass("CJDCQJX");

            //县代码  排标识码

            string where = "";
            if (xzdm.Length > 0)
                where = "BSM LIKE '" + xzdm + "%'";

            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pCJDCQJX, "BSM", where) + 1;

            if (maxBSM.ToString().Length < 18)
            {
                Dictionary<string, string> dicDm = getCDM();
                string cdm = "0000";
                if (dicDm.ContainsKey("CJDCQJXGX"))
                {
                    cdm = dicDm["CJDCQJXGX"];
                }
                else
                {
                    if (dicDm.ContainsKey("CJDCQJX"))
                    {
                        cdm = dicDm["CJDCQJX"];
                    }
                }
                string bsm = xzdm + cdm + maxBSM.ToString().PadLeft(8, '0');
                maxBSM = long.Parse(bsm);
            }

            ISpatialFilter pSpatialFil = new SpatialFilterClass();
            pSpatialFil.Geometry = pGeo;
            pSpatialFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pGXJXCursor = pGXJX.Search(pSpatialFil, true);
            IFeature pFeature;

            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pCJDCQJX;
            IIdentify pIdentify = pLayer as IIdentify;
            IFeatureCursor pInsert = pCJDCQJXGX.Insert(true);
            List<int> arrWBH = new List<int>();
            List<int> arr = new List<int>();
            while ((pFeature = pGXJXCursor.NextFeature()) != null)
            {
                if (!GetIntersectLength(pFeature, pGeo))
                    continue;
                bool b = false;
                List<IFeature> pList0 = new List<IFeature>();
                IArray pArray = pIdentify.Identify(pFeature.ShapeCopy);
                if (pArray != null)
                {
                    IRelationalOperator pRel = (pFeature.ShapeCopy) as IRelationalOperator;
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pFea = pRow.Row as IFeature;
                        if (pRel.Equals(pFea.ShapeCopy))
                        {
                            IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pFea.get_Value(pFea.Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pFea.get_Value(pFea.Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pFea.get_Value(pFea.Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "4");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pFea.OID);
                            b = true;
                            break;
                        }
                        else
                        {
                            if (GetIntersectLen(pFea, pFeature))
                                pList0.Add(pFea);
                        }
                    }
                }
                if (!b)
                {
                    IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                    pFeaBuffer.Shape = pFeature.ShapeCopy;
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), maxBSM++);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), "660200");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), "600001");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "3");
                    pInsert.InsertFeature(pFeaBuffer);
                    for (int i = 0; i < pList0.Count; i++)
                    {
                        if (!arr.Contains(pList0[i].OID))
                        {
                            pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pList0[i].ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pList0[i].get_Value(pList0[i].Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pList0[i].get_Value(pList0[i].Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pList0[i].get_Value(pList0[i].Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pList0[i].OID);
                        }
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);

            }

            //bool bb=RCIS.GISCommon.GpToolHelper.Erase_analysis(filePath + "\\TDDC\\CJDCQJX", filePath + "\\TDGX\\CJDCQJXGX", tmpWS.PathName + "\\JX0");
            //IFeatureClass pJX0 = (tmpWS as IFeatureWorkspace).OpenFeatureClass("JX0");

            ISpatialFilter pSf = new SpatialFilterClass();
            pSf.Geometry = pGeo;
            pSf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            IFeatureCursor pCursor = pCJDCQJX.Search(pSf, true);
            IFeature pJXFea;
            while ((pJXFea = pCursor.NextFeature()) != null)
            {
                if (arr.Contains(pJXFea.OID))
                    continue;
                IFeatureBuffer pFeaBuffer = pCJDCQJXGX.CreateFeatureBuffer();
                pFeaBuffer.Shape = pJXFea.ShapeCopy;
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pJXFea.get_Value(pJXFea.Fields.FindField("BSM")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), cjdcqjcgxYSDM);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pJXFea.get_Value(pJXFea.Fields.FindField("JXLX")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pJXFea.get_Value(pJXFea.Fields.FindField("JXXZ")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                pInsert.InsertFeature(pFeaBuffer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pJXFea);

            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

            RCIS.Utility.OtherHelper.ReleaseComObject(pGXJXCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);

        }

        private Dictionary<string, string> getCDM()
        {
            string sql = "select CLASSNAME,LayerDM from SYS_YSDM  where type in ('POINT','LINE','POLYGON')  ";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                dic.Add(dr["CLASSNAME"].ToString().Trim().ToUpper(), dr["LayerDM"].ToString().Trim());
            }
            return dic;

        }

        private void XZQJX1()
        {
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQJXGX = pFeaWorkspace.OpenFeatureClass("XZQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            (pXZQJXGX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取行政区界线");
            List<string> IDwbh = new List<string>();
            string xzqjcgxYSDM = GetValueFromMDBByLayerName("XZQJXGX");
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");

            IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("XZQGX");
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);

            bool updata = RCIS.GISCommon.GpToolHelper.Update(filePath + "\\TDDC\\XZQ", filePath + "\\TDGX\\XZQGX", tmpWS.PathName + "\\xzqnmk");


            IFeatureClass pGXJX = GPPolygonToLineByFeatureclass(tmpWS.PathName + "\\xzqnmk", tmpWS);
            IFeatureClass pXZQJX = pFeaWorkspace.OpenFeatureClass("XZQJX");

            //县代码

            string where = "";
            if (xzdm.Length > 0)
                where = "BSM LIKE '" + xzdm + "%'";

            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQJX, "BSM", where) + 1;

            if (maxBSM.ToString().Length < 18)
            {
                Dictionary<string, string> dicDm = getCDM();
                string cdm = "0000";
                if (dicDm.ContainsKey("XZQJXGX"))
                {
                    cdm = dicDm["XZQJXGX"];
                }
                else
                {
                    if (dicDm.ContainsKey("XZQJX"))
                    {
                        cdm = dicDm["XZQJX"];
                    }
                }
                string bsm = xzdm + cdm + maxBSM.ToString().PadLeft(8, '0');
                maxBSM = long.Parse(bsm);
            }

            ISpatialFilter pSpatialFil = new SpatialFilterClass();
            pSpatialFil.Geometry = pGeo;
            pSpatialFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pGXJXCursor = pGXJX.Search(null, true);
            IFeature pFeature;

            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pXZQJX;
            IIdentify pIdentify = pLayer as IIdentify;
            IFeatureCursor pInsert = pXZQJXGX.Insert(true);
            List<int> arr = new List<int>();
            while ((pFeature = pGXJXCursor.NextFeature()) != null)
            {
                if (!GetIntersectLength(pFeature, pGeo))
                    continue;
                bool b = false;
                List<IFeature> pList0 = new List<IFeature>();
                IArray pArray = pIdentify.Identify(pFeature.ShapeCopy);
                if (pArray != null)
                {
                    IRelationalOperator pRel = (pFeature.ShapeCopy) as IRelationalOperator;
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pFea = pRow.Row as IFeature;
                        if (pRel.Equals(pFea.ShapeCopy))
                        {
                            IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pFea.get_Value(pFea.Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pFea.get_Value(pFea.Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pFea.get_Value(pFea.Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "4");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pFea.OID);
                            b = true;
                            break;
                        }
                        else
                        {
                            if (GetIntersectLen(pFea, pFeature))
                                pList0.Add(pFea);
                        }
                    }
                }
                if (!b)
                {
                    IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                    pFeaBuffer.Shape = pFeature.ShapeCopy;
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), maxBSM++);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), "660200");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), "600001");
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                    pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "3");
                    pInsert.InsertFeature(pFeaBuffer);
                    for (int i = 0; i < pList0.Count; i++)
                    {
                        if (!arr.Contains(pList0[i].OID))
                        {
                            pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                            pFeaBuffer.Shape = pList0[i].ShapeCopy;
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pList0[i].get_Value(pList0[i].Fields.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pList0[i].get_Value(pList0[i].Fields.FindField("JXLX")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pList0[i].get_Value(pList0[i].Fields.FindField("JXXZ")).ToString());
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                            pInsert.InsertFeature(pFeaBuffer);
                            arr.Add(pList0[i].OID);
                        }
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);

            }

            //bool bb = RCIS.GISCommon.GpToolHelper.Erase_analysis(filePath + "\\TDDC\\XZQJX", filePath + "\\TDGX\\XZQJXGX", tmpWS.PathName + "\\JX00");
            //IFeatureClass pJX0 = (tmpWS as IFeatureWorkspace).OpenFeatureClass("JX00");
            //IFeatureClass pCjdcqgx = pFeaWorkspace.OpenFeatureClass("XZQGX");
            //IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pCjdcqgx);
            ISpatialFilter pSf = new SpatialFilterClass();
            pSf.Geometry = pGeo;
            pSf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            IFeatureCursor pCursor = pXZQJX.Search(pSf, true);
            IFeature pJXFea;
            while ((pJXFea = pCursor.NextFeature()) != null)
            {
                if (arr.Contains(pJXFea.OID))
                    continue;
                IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                pFeaBuffer.Shape = pJXFea.ShapeCopy;
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), pJXFea.get_Value(pJXFea.Fields.FindField("BSM")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), pJXFea.get_Value(pJXFea.Fields.FindField("JXLX")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), pJXFea.get_Value(pJXFea.Fields.FindField("JXXZ")).ToString());
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "0");
                pInsert.InsertFeature(pFeaBuffer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pJXFea);

            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);


            RCIS.Utility.OtherHelper.ReleaseComObject(pGXJXCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            //

        }

        private void XZQJX()
        {
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQJXGX = pFeaWorkspace.OpenFeatureClass("XZQJXGX");
            //if (MessageBox.Show("原村级调查区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            (pXZQJXGX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取行政区界线");
            List<string> IDwbh = new List<string>();
            string xzqjcgxYSDM = GetValueFromMDBByLayerName("XZQJXGX");
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            //if (Directory.Exists(tmp))
            //{
            //    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            //    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
            //    pEnumDataset.Reset();
            //    IDataset pDataset;
            //    while ((pDataset = pEnumDataset.Next()) != null)
            //    {
            //        pDataset.Delete();
            //    }
            //}
            //else
            //{
            //    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
            //    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
            //    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            //}
            //IFeatureClass pSDFeatureClass = (tmpWS as IFeatureWorkspace).CreateFeatureClass("sdtb", pDLTB.Fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", null);

            IFeatureClass pGXJX = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\XZQGX", tmpWS);
            IFeatureClass pXZQJX = pFeaWorkspace.OpenFeatureClass("XZQJX");
            IFeatureClass pXZQGX = pFeaWorkspace.OpenFeatureClass("XZQGX");
            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQJX, "BSM") + 1;
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pXZQGX);
            IRelationalOperator pRelOperator = pGeo as IRelationalOperator;
            //ITopologicalOperator pTopo=pgeo
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = pGeo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.GeometryField = pXZQJX.ShapeFieldName;
            IFeatureCursor pFeatureCursor = pXZQJX.Search(pSF, true);
            IFeature pFeature;
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pGXJX;
            IIdentify pIdentify = pLayer as IIdentify;
            IFeatureCursor pInsert = pXZQJXGX.Insert(true);
            while ((pFeature = pFeatureCursor.NextFeature()) != null)
            {
                if (!pRelOperator.Contains(pFeature.ShapeCopy) && !pRelOperator.Crosses(pFeature.ShapeCopy))
                    continue;
                bool b = false;
                IFeatureBuffer pBuffer = pXZQJXGX.CreateFeatureBuffer();
                pBuffer.Shape = pFeature.ShapeCopy;
                pBuffer.set_Value(pBuffer.Fields.FindField("BSM"), pFeature.get_Value(pFeature.Fields.FindField("BSM")));
                pBuffer.set_Value(pBuffer.Fields.FindField("YSDM"), pFeature.get_Value(pFeature.Fields.FindField("YSDM")));
                pBuffer.set_Value(pBuffer.Fields.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                pBuffer.set_Value(pBuffer.Fields.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                pBuffer.set_Value(pBuffer.Fields.FindField("GXSJ"), bgData);
                IRelationalOperator pRel = (pFeature.ShapeCopy) as IRelationalOperator;
                IArray pArray = pIdentify.Identify(pFeature.ShapeCopy);
                if (pArray != null)
                {
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature pFea = pRow.Row as IFeature;
                        if (pRel.Equals(pFea.ShapeCopy))
                        {
                            IDwbh.Add(pFea.OID.ToString());
                            b = true;
                            break;
                        }
                    }
                }
                if (b == true)
                    pBuffer.set_Value(pBuffer.Fields.FindField("BGXW"), "4");
                else
                    pBuffer.set_Value(pBuffer.Fields.FindField("BGXW"), "0");
                pInsert.InsertFeature(pBuffer);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureCursor);
            string where = "";
            for (int j = 0; j < IDwbh.Count; j++)
            {
                where += "OBJECTID<>" + IDwbh[j] + " and ";
            }
            if (where.Length > 0)
                where = where.Substring(0, where.Length - 4);
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            IFeatureCursor pCursor = pGXJX.Search(pQF, true);
            IFeature pF;
            while ((pF = pCursor.NextFeature()) != null)
            {
                IFeatureBuffer pFeaBuffer = pXZQJXGX.CreateFeatureBuffer();
                pFeaBuffer.Shape = pF.ShapeCopy;
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BSM"), maxBSM++);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("YSDM"), xzqjcgxYSDM);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXLX"), "660200");
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("JXXZ"), "600001");
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("GXSJ"), bgData);
                pFeaBuffer.set_Value(pFeaBuffer.Fields.FindField("BGXW"), "3");
                pInsert.InsertFeature(pFeaBuffer);

            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            (pGXJX as IDataset).Delete();
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXJX);
            RCIS.Utility.OtherHelper.ReleaseComObject(tmpWS);

        }

        private IWorkspace DeleteAndNewTmpGDB()
        {
            string path = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;

            if (Directory.Exists(path))
            {
                try
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                catch
                {
                    foreach (string tmp in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(tmp))
                        {
                            //如果有子文件删除文件
                            File.Delete(tmp);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(path);
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            return tmpWS;
        }

        private void JSMJCJDCQ()
        {
            //数据汇总流程
            //1.计算变更后行政区，村级调查区的图形范围
            //2.计算变更后行政区，村级调查区中变更图斑的面积
            //3.计算变更后行政区，村级调查区中三调图斑面积并汇总

            IFeatureClass pCJDCQGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGXGC");
            IFeatureClass pDLTBGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            IFeatureClass pDLTBClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            Dictionary<string, double> dm_mj = new Dictionary<string, double>();

            UpdateStatus("开始计算村级调查区更新过程层面积");
            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pCJDCQGXGCClass, null, "BGHZLDWDM");
            foreach (var item in dwdms)
            {
                string dwdm = item.ToString();
                double DCMJ = 0;
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHZLDWDM Like '" + dwdm + "%'";
                    IFeatureCursor pCJDCQGXGCCursor = pCJDCQGXGCClass.Update(pQueryFilter, true);
                    comRel.ManageLifetime(pCJDCQGXGCCursor);
                    List<string> bsms = new List<string>();
                    IFeature pCJDCQGXGC;
                    while ((pCJDCQGXGC = pCJDCQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGQZLDWDM")).ToString();
                        string bghdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGHZLDWDM")).ToString();
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pCJDCQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "BGQZLDWDM = '" + bgqdwdm + "' And BGHZLDWDM = '" + bghdwdm + "'";
                            IFeatureCursor pDLTBGXGCCursor = pDLTBGXGCClass.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pDLTBGXGCCursor);
                            IFeature pDLTBGXGC;
                            while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                            {
                                bool b = GetIntersectArea(pDLTBGXGC, pCJDCQGXGC);
                                if (b == true)
                                {
                                    string bsm = pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("BGQTBBSM")).ToString();
                                    bsms.Add(bsm);
                                    double TBMJ = double.Parse(pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("TBBGMJ")).ToString());
                                    BGMJ += TBMJ;
                                    DCMJ += TBMJ;
                                }
                                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGC);
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCCursor);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                        }
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pCJDCQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "ZLDWDM = '" + bgqdwdm + "'";
                            IFeatureCursor pDLTBCursor = pDLTBClass.Search(pSpatialFilter, true);
                            IFeature pDLTB;
                            while ((pDLTB = pDLTBCursor.NextFeature()) != null)
                            {
                                string bsm = pDLTB.get_Value(pDLTBClass.FindField("BSM")).ToString();
                                if (bsms.Contains(bsm)) continue;
                                bool b = GetIntersectArea(pDLTB, pCJDCQGXGC);
                                if (b == true)
                                {
                                    double TBMJ = double.Parse(pDLTB.get_Value(pDLTBClass.FindField("TBMJ")).ToString());
                                    BGMJ += TBMJ;
                                    DCMJ += TBMJ;
                                }
                                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTB);
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBCursor);
                        }
                        pCJDCQGXGC.set_Value(pCJDCQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pCJDCQGXGCCursor.UpdateFeature(pCJDCQGXGC);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGC);

                    }
                    pCJDCQGXGCCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCCursor);
                }
                dm_mj.Add(dwdm, DCMJ);
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pCJDCQGXGCClass as ITable, "BGHDCMJ", Math.Round(dm_mj[dwdm], 2), "BGHZLDWDM = '" + dwdm + "'");
            }
            UpdateStatus("开始计算村级调查区更新层面积");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            IFeatureClass pCJDCQGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
            IFeatureCursor pCursor = pCJDCQGXClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("ZLDWDM")).ToString();
                if (currDh == 0)
                {
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号
                }
                double JSMJ = area.SphereArea(pFea.ShapeCopy, currDh);
                pFea.set_Value(pFea.Fields.FindField("JSMJ"), Math.Round(JSMJ, 2));
                pFea.set_Value(pFea.Fields.FindField("DCMJ"), Math.Round(dm_mj[zldwdm], 2));
                pCursor.UpdateFeature(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCClass);
            UpdateStatus("村级调查区面积计算完成");
        }

        private void JSMJCJDCQ(string awm)
        {
            //数据汇总流程
            //1.计算变更后行政区，村级调查区的图形范围
            //2.计算变更后行政区，村级调查区中变更图斑的面积
            //3.计算变更后行政区，村级调查区中三调图斑面积并汇总
            UpdateStatus("正在计算村级调查区更新过程层面积");

            IWorkspace pTmpWs = DeleteAndNewTmpGDB();
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWs, "DLTBGXGC", "TBGXGC", null);
            IFeatureClass pTBGXGC = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TBGXGC");
            ISchemaLock pSchemaLock = null;
            pSchemaLock = pTBGXGC as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = pTBGXGC as IClassSchemaEdit4;
            pClassSchemaEdit.AlterFieldName("TBBGMJ", "TBMJ");
            pClassSchemaEdit.AlterFieldName("BGHZLDWDM", "ZLDWDM");
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

            bool b = RCIS.GISCommon.GpToolHelper.Update(currWs.PathName + "\\TDDC\\DLTB", pTmpWs.PathName + "\\TBGXGC", pTmpWs.PathName + "\\TB");
            if (!b)
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            IFeatureClass pTB = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TB");


            IFeatureClass pCJDCQGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGXGC");

            Dictionary<string, double> dm_mj = new Dictionary<string, double>();

            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pCJDCQGXGCClass, null, "BGHZLDWDM");
            foreach (var item in dwdms)
            {
                string dwdm = item.ToString();
                double DCMJ = 0;
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHZLDWDM Like '" + dwdm + "%'";
                    IFeatureCursor pCJDCQGXGCCursor = pCJDCQGXGCClass.Update(pQueryFilter, true);
                    comRel.ManageLifetime(pCJDCQGXGCCursor);
                    IFeature pCJDCQGXGC;
                    while ((pCJDCQGXGC = pCJDCQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGQZLDWDM")).ToString();
                        string bghdwdm = pCJDCQGXGC.get_Value(pCJDCQGXGCClass.FindField("BGHZLDWDM")).ToString();
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pCJDCQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pTB.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "ZLDWDM = '" + bghdwdm + "'";
                            IFeatureCursor pDLTBGXGCCursor = pTB.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pDLTBGXGCCursor);
                            IFeature pDLTBGXGC;
                            while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                            {
                                b = GetIntersectArea(pDLTBGXGC, pCJDCQGXGC);
                                if (b == true)
                                {
                                    double TBMJ = double.Parse(pDLTBGXGC.get_Value(pTB.FindField("TBMJ")).ToString());
                                    BGMJ += TBMJ;
                                    DCMJ += TBMJ;
                                }
                                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGC);
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCCursor);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                        }
                        pCJDCQGXGC.set_Value(pCJDCQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pCJDCQGXGCCursor.UpdateFeature(pCJDCQGXGC);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGC);
                    }
                    pCJDCQGXGCCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCCursor);
                }
                dm_mj.Add(dwdm, DCMJ);
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pCJDCQGXGCClass as ITable, "BGHDCMJ", Math.Round(dm_mj[dwdm], 2), "BGHZLDWDM = '" + dwdm + "'");
            }
            UpdateStatus("正在计算村级调查区更新层面积");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            IFeatureClass pCJDCQGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQGX");
            IFeatureCursor pCursor = pCJDCQGXClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("ZLDWDM")).ToString();
                if (currDh == 0)
                {
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号
                }
                double JSMJ = area.SphereArea(pFea.ShapeCopy, currDh);
                pFea.set_Value(pFea.Fields.FindField("JSMJ"), Math.Round(JSMJ, 2));
                pFea.set_Value(pFea.Fields.FindField("DCMJ"), Math.Round(dm_mj[zldwdm], 2));
                pCursor.UpdateFeature(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTB);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTBGXGC);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQGXGCClass);
            //UpdateStatus("村级调查区面积计算完成");
        }

        private void ProduceCJDCQGXGC()
        {
            IFeatureWorkspace pFWor = currWs as IFeatureWorkspace;
            IFeatureClass pGXFeatureclass = null;
            try
            {
                pGXFeatureclass = pFWor.OpenFeatureClass("CJDCQGX");
                if (pGXFeatureclass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("村级调查区更新层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到村级调查区更新层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            UpdateStatus("正在处理变化村级调查区数据");
            IGeometry pGeometry = RCIS.GISCommon.GeometryHelper.MergeGeometry(pGXFeatureclass);
            UpdateStatus("正在查找与变化村级调查区重叠的三调村级调查区");
            IFeatureClass pCJDCQClass = null;
            try
            {
                pCJDCQClass = pFWor.OpenFeatureClass("CJDCQ");
                if (pCJDCQClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("村级调查区层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("未找到村级调查区层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string CJDCQSD = Application.StartupPath + @"\tmp\CJDCQSD.shp";
            string CJDCQGXGC = Application.StartupPath + @"\tmp\CJDCQGXGC.shp";
            IFeatureClass pSDClass;
            if (File.Exists(CJDCQSD))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(CJDCQSD);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            if (File.Exists(CJDCQGXGC))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(CJDCQGXGC);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            pSDClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(CJDCQSD, esriGeometryType.esriGeometryPolygon, (pCJDCQClass as IGeoDataset).SpatialReference, pCJDCQClass.Fields);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.GeometryField = pCJDCQClass.ShapeFieldName;
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pSpatialFilter.SpatialRelDescription = "T********";
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsertCursor = pSDClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeatureCursor pSearchCursor = pCJDCQClass.Search(pSpatialFilter, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IGeometry pGeoIntersect = pTop.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (pGeoIntersect != null)
                    {
                        IArea pArea = pGeoIntersect as IArea;
                        if (pArea.Area > 0.001)
                        {
                            pInsertCursor.InsertFeature(pFeature as IFeatureBuffer);
                        }
                    }
                }
                pInsertCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            UpdateStatus("正在进行叠加分析");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
            string gxPath = currWs.PathName + "\\TDGX\\CJDCQGX";
            pUnion.in_features = CJDCQSD + ";" + gxPath;
            pUnion.out_feature_class = CJDCQGXGC;
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                UpdateStatus("叠加分析错误");
            }
            string MultipartToSinglepart = Application.StartupPath + "\\tmp\\MultipartToSinglepart.shp";
            if (File.Exists(MultipartToSinglepart))
            {
                ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart)) as IDataset).Delete();
            }
            bool result = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(CJDCQGXGC, MultipartToSinglepart);
            if (result == false)
            {
                UpdateStatus("叠加分析错误");
            }
            UpdateStatus("正在导入叠加结果");
            ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(CJDCQGXGC)) as IDataset).Delete();
            IFeatureClass pSourClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart);
            IFeatureClass pTarClass = null;
            try
            {
                pTarClass = pFWor.OpenFeatureClass("CJDCQGXGC");
            }
            catch
            {
                UpdateStatus("数据库没有升级，请先升级数据库。");
            }

            string where = "";
            if (xzdm.Length > 0)
                where = "ZLDWDM LIKE '" + xzdm + "%'";
            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pCJDCQClass, "BSM", where) + 1;
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pGXFeatureclass, "BSM", where) + 1;
            long maxBSM = 0;
            if (max1 > max2)
                maxBSM = max1;
            else
                maxBSM = max2;
            ITable pTable = pTarClass as ITable;
            pTable.DeleteSearchedRows(null);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pSearchCursor = pSourClass.Search(null, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeatureCursor pInsertCursor = pTarClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    if (pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString().Trim() == pFeature.get_Value(pFeature.Fields.FindField("BSM_1")).ToString().Trim() || pFeature.get_Value(pFeature.Fields.FindField("BZ_1")).ToString().Trim() == "S")
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    else
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pSourClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pSourClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pSourClass.FindField("BSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pSourClass.FindField("ZLDWDM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pSourClass.FindField("ZLDWMC_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ_1")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pSourClass.FindField("BZ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC_1")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsertCursor.InsertFeature(pFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            UpdateStatus("正在提取变更前同名的村级调查区");
            ArrayList xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGQZLDWDM");
            List<string> xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("ZLDWDM = '" + item.ToString() + "'");
            }
            string sWhere = string.Join(" Or ", xzqdm);
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pCJDCQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pCJDCQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");


                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pCJDCQClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pCJDCQClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    //RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGQZLDWDM = '" + pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")) + "'");
                }
                pInsert.Flush();
            }
            UpdateStatus("正在提取变更后同名的村级调查区");
            xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGHZLDWDM");
            xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("ZLDWDM = '" + item.ToString() + "'");
            }
            sWhere = string.Join(" Or ", xzqdm);
            pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pCJDCQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pCJDCQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);


                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pCJDCQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pCJDCQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWDM"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHZLDWMC"), pFeature.get_Value(pCJDCQClass.FindField("ZLDWMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pCJDCQClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pCJDCQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pCJDCQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pCJDCQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    //RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGHZLDWDM = '" + pFeature.get_Value(pCJDCQClass.FindField("ZLDWDM")) + "'");
                }
                pInsert.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCJDCQClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSourClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTarClass);

            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureclass);
            currWs.ExecuteSQL("update cjdcqgx set bz=''");
            currWs.ExecuteSQL("update cjdcqgxgc set bghbz=''");
            currWs.ExecuteSQL("delete from cjdcqgxgc where BGQBSM is null or BGHBSM is null or BGQBSM='' or BGHBSM=''");


            JSMJCJDCQ("");

            CJDCQJX1();
            UpdateStatus("村级调查区更新过程层提取完毕");


        }

        private void ProduceXZQJXGX()
        {

            //变化行政区
            IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pXZQJX = pFeaWorkspace.OpenFeatureClass("XZQJXGX");
            //if (MessageBox.Show("原行政区界线更新层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;

            (pXZQJX as ITable).DeleteSearchedRows(null);
            string filePath = currWs.PathName;
            UpdateStatus("正在提取行政区界线");
            string tmpPath = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;
            if (Directory.Exists(tmpPath))
            {
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                pEnumDataset.Reset();
                IDataset pDataset;
                while ((pDataset = pEnumDataset.Next()) != null)
                {
                    pDataset.Delete();
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            IFeatureClass pGXGCFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\XZQGXGC", tmpWS);
            IFeatureClass pGXFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDGX\\XZQGX", tmpWS);
            //IFeatureClass pSDFeatureclass = GPPolygonToLineByFeatureclass(filePath + "\\TDDC\\XZQ");
            IFeatureClass pSDFeatureclass = pFeaWorkspace.OpenFeatureClass("XZQJX");

            IFeatureClass mFeaClass = GPUpdate(filePath + "\\TDDC\\XZQ", filePath + "\\TDGX\\XZQGXGC");
            IFeatureClass allFeaClass = GPPolygonToLineByFeatureclass((mFeaClass as IDataset).Workspace.PathName + "\\" + (mFeaClass as IDataset).Name + ".shp", tmpWS);
            IFeatureCursor allFeaCursor = allFeaClass.Update(null, true);
            IFeature allFeature;
            while ((allFeature = allFeaCursor.NextFeature()) != null)
            {
                ISpatialFilter SpaFil = new SpatialFilterClass();
                SpaFil = new SpatialFilterClass();
                SpaFil.Geometry = allFeature.ShapeCopy;
                SpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                SpaFil.SpatialRelDescription = "T********";
                IFeatureCursor cursor = pGXGCFeatureclass.Search(SpaFil, true);
                IFeature feature = cursor.NextFeature();
                bool bl = false;
                //if (allFeature.get_Value(allFeature.Fields.FindField("LEFT_FID")).ToString() == "3")
                //{
                //    string a = "";
                //}
                while (feature != null)
                {
                    bl = GetIntersectLength(allFeature, feature);
                    if (bl == true)
                        break;
                    feature = cursor.NextFeature();
                }
                if (bl == false)
                {
                    allFeaCursor.DeleteFeature();
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(cursor);

            }
            allFeaCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(allFeaCursor);
            long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pFeaWorkspace.OpenFeatureClass("XZQJX"), "BSM") + 1;

            UpdateStatus("正在生成行政区界线更新层");
            string xzqjxgxYSDM = GetValueFromMDBByLayerName("XZQJXGX");
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsert = pXZQJX.Insert(true);
                comRel.ManageLifetime(pInsert);
                using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pFeaCursor = pSDFeatureclass.Search(null, true);
                    comRel2.ManageLifetime(pFeaCursor);
                    IFeature pFeature;
                    ISpatialFilter pSpaFil = null;
                    while ((pFeature = pFeaCursor.NextFeature()) != null)
                    {
                        pSpaFil = new SpatialFilterClass();
                        pSpaFil.Geometry = pFeature.ShapeCopy;
                        pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpaFil.SpatialRelDescription = "T********";
                        IFeatureCursor pGXGCFeatureCursor = allFeaClass.Search(pSpaFil, false);
                        IFeature pGXGCFeature;
                        IFeature tempFeature = null;
                        bool B = false;
                        int tmp = 0;
                        while ((pGXGCFeature = pGXGCFeatureCursor.NextFeature()) != null)
                        {
                            B = false;
                            B = GetIntersectLength(pFeature, pGXGCFeature);
                            if (B == true)
                            {
                                if (tmp == 0)
                                {
                                    tempFeature = pGXGCFeature;
                                    tmp += 1;
                                }
                                else
                                {
                                    pSpaFil = new SpatialFilterClass();
                                    pSpaFil.Geometry = pGXGCFeature.ShapeCopy;
                                    pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                                    pSpaFil.SpatialRelDescription = "T********";
                                    IFeatureCursor pGXFeaCursor = pGXFeatureclass.Search(pSpaFil, true);
                                    IFeature pGXFeature;
                                    while ((pGXFeature = pGXFeaCursor.NextFeature()) != null)
                                    {
                                        if (GetIntersectLength(pGXGCFeature, pGXFeature) == true)
                                        {
                                            IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                                            int pFieldNum = pXZQJX.FindField("BGXW");
                                            pFeaBuffer.Shape = pGXGCFeature.ShapeCopy;
                                            pFeaBuffer.set_Value(pXZQJX.FindField("BSM"), maxBSM++);
                                            pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), xzqjxgxYSDM);
                                            pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                                            pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                                            pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                                            pFeaBuffer.set_Value(pFieldNum, "3");//新增
                                            pInsert.InsertFeature(pFeaBuffer);
                                            break;
                                        }
                                    }
                                    RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeaCursor);
                                    tmp += 1;
                                }
                            }
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCFeatureCursor);

                        if (tmp == 1)
                        {
                            IFeatureCursor pGXCursor = pGXFeatureclass.Search(pSpaFil, true);
                            IFeature pGXFea;
                            bool b = false;
                            while ((pGXFea = pGXCursor.NextFeature()) != null)
                            {
                                b = GetIntersectLength(pGXFea, pFeature);
                                if (b == true)
                                {
                                    IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                                    int pFieldNum = pXZQJX.FindField("BGXW");
                                    pFeaBuffer.Shape = pFeature.ShapeCopy;
                                    pFeaBuffer.set_Value(pXZQJX.FindField("BSM"), pFeature.get_Value(pSDFeatureclass.FindField("BSM")).ToString());
                                    pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), xzqjxgxYSDM);
                                    pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                                    pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                                    pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                                    pFeaBuffer.set_Value(pFieldNum, "4");//无变化
                                    pInsert.InsertFeature(pFeaBuffer);
                                    break;
                                }
                            }
                            if (b == false)
                            {
                                IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                                int pFieldNum = pXZQJX.FindField("BGXW");
                                pFeaBuffer.Shape = pFeature.ShapeCopy;
                                pFeaBuffer.set_Value(pXZQJX.FindField("BSM"), pFeature.get_Value(pSDFeatureclass.FindField("BSM")).ToString());
                                pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), xzqjxgxYSDM);
                                pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                                pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                                pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                                pFeaBuffer.set_Value(pFieldNum, "0");//灭失
                                pInsert.InsertFeature(pFeaBuffer);
                            }
                            //IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                            //int pFieldNum = pXZQJX.FindField("BGXW");
                            //pFeaBuffer.Shape = pFeature.ShapeCopy;
                            //pFeaBuffer.set_Value(pXZQJX.FindField("BSM"), maxBSM++);
                            //pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), "1000600200");
                            //pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                            //pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                            //pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                            //pFeaBuffer.set_Value(pFieldNum, "3");//无变化
                            //pInsert.InsertFeature(pFeaBuffer);
                        }
                        if (tmp > 1)
                        {
                            IFeatureBuffer pFeaBuffer = pXZQJX.CreateFeatureBuffer();
                            int pFieldNum = pXZQJX.FindField("BGXW");
                            pFeaBuffer.Shape = pFeature.ShapeCopy;
                            pFeaBuffer.set_Value(pXZQJX.FindField("BSM"), pFeature.get_Value(pSDFeatureclass.FindField("BSM")).ToString());
                            pFeaBuffer.set_Value(pXZQJX.FindField("YSDM"), xzqjxgxYSDM);
                            pFeaBuffer.set_Value(pXZQJX.FindField("JXLX"), pFeature.get_Value(pFeature.Fields.FindField("JXLX")));
                            pFeaBuffer.set_Value(pXZQJX.FindField("JXXZ"), pFeature.get_Value(pFeature.Fields.FindField("JXXZ")));
                            pFeaBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                            pFeaBuffer.set_Value(pFieldNum, "0");//灭失
                            pInsert.InsertFeature(pFeaBuffer);

                            pSpaFil = new SpatialFilterClass();
                            pSpaFil.Geometry = tempFeature.ShapeCopy;
                            pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                            pSpaFil.SpatialRelDescription = "T********";
                            IFeatureCursor pGXFeatureCursor = pGXFeatureclass.Search(pSpaFil, true);
                            IFeature pGXFeature;
                            while ((pGXFeature = pGXFeatureCursor.NextFeature()) != null)
                            {
                                if (GetIntersectLength(tempFeature, pGXFeature) == true)
                                {
                                    IFeatureBuffer pFeatureBuffer = pXZQJX.CreateFeatureBuffer();
                                    pFeatureBuffer.Shape = tempFeature.ShapeCopy;
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("BSM"), maxBSM++);
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("YSDM"), xzqjxgxYSDM);
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                                    pFeatureBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                                    pFeatureBuffer.set_Value(pFieldNum, "3");//新增
                                    pInsert.InsertFeature(pFeatureBuffer);
                                    break;
                                }
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureCursor);

                        }
                    }
                    IFeatureCursor pCursor = pGXGCFeatureclass.Search(null, true);
                    IFeature pFea;
                    while ((pFea = pCursor.NextFeature()) != null)
                    {
                        pSpaFil = new SpatialFilterClass();
                        pSpaFil.Geometry = pFea.ShapeCopy;
                        pSpaFil.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                        pSpaFil.SpatialRelDescription = "T********";
                        IFeatureCursor pSDCursor = pSDFeatureclass.Search(pSpaFil, true);
                        IFeature pSD = pSDCursor.NextFeature();
                        if (pSD != null)
                        {
                            if (GetIntersectLength(pSD, pFea) == false)
                                pSD = null;
                        }
                        IFeatureCursor pGXCursor = pGXFeatureclass.Search(pSpaFil, true);
                        IFeature pGX = pGXCursor.NextFeature();
                        if (pSD == null && pGX != null)
                        {
                            IFeatureBuffer pFeatureBuffer = pXZQJX.CreateFeatureBuffer();
                            pFeatureBuffer.Shape = pFea.ShapeCopy;
                            pFeatureBuffer.set_Value(pXZQJX.FindField("BSM"), maxBSM++);
                            pFeatureBuffer.set_Value(pXZQJX.FindField("YSDM"), xzqjxgxYSDM);
                            pFeatureBuffer.set_Value(pXZQJX.FindField("JXLX"), "660200");
                            pFeatureBuffer.set_Value(pXZQJX.FindField("JXXZ"), "600001");
                            pFeatureBuffer.set_Value(pXZQJX.FindField("GXSJ"), bgData);
                            int pFieldNum = pXZQJX.FindField("BGXW");
                            pFeatureBuffer.set_Value(pFieldNum, "3");//新增
                            pInsert.InsertFeature(pFeatureBuffer);
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSDCursor);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pGXCursor);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

                }
                pInsert.Flush();
            }


            (pGXGCFeatureclass as IDataset).Delete();
            (pGXFeatureclass as IDataset).Delete();
            //(pSDFeatureclass as IDataset).Delete();
            (mFeaClass as IDataset).Delete();
            (allFeaClass as IDataset).Delete();

            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "BGXW='4'";
            IFeatureCursor Cursor = pXZQJX.Update(pQF, true);
            IFeature pJX;
            while ((pJX = Cursor.NextFeature()) != null)
            {
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = pJX.ShapeCopy;
                pSF.WhereClause = "BGXW='3'";
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pcursor = pXZQJX.Search(pSF, true);
                IFeature pFea;
                while ((pFea = pcursor.NextFeature()) != null)
                {
                    bool b = GetIntersectLength(pJX, pFea);
                    if (b == true)
                    {
                        pJX.set_Value(pXZQJX.FindField("BGXW"), 0);
                        Cursor.UpdateFeature(pJX);
                        continue;
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pcursor);
            }
            Cursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(Cursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQJX);

            UpdateStatus("行政区界线更新层生成完毕");
        }

        private void JSMJXZQ()
        {
            //数据汇总流程
            //1.计算变更后行政区，行政区的图形范围
            //2.计算变更后行政区，行政区中变更图斑的面积
            //3.计算变更后行政区，行政区中三调图斑面积并汇总
            IFeatureClass pXZQGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGXGC");
            IFeatureClass pDLTBGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            IFeatureClass pDLTBClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            Dictionary<string, double> dm_mj = new Dictionary<string, double>();
            UpdateStatus("开始计算行政区更新过程层面积");
            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pXZQGXGCClass, null, "BGHXZQDM");
            foreach (var item in dwdms)
            {
                string dwdm = item.ToString();
                double DCMJ = 0;
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHXZQDM Like '" + dwdm + "%'";
                    IFeatureCursor pXZQGXGCCursor = pXZQGXGCClass.Update(pQueryFilter, true);
                    comRel.ManageLifetime(pXZQGXGCCursor);
                    List<string> bsms = new List<string>();
                    IFeature pXZQGXGC;
                    while ((pXZQGXGC = pXZQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGQXZQDM")).ToString();
                        string bghdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGHXZQDM")).ToString();

                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pXZQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "BGQZLDWDM Like '" + bgqdwdm + "%' And BGHZLDWDM Like '" + bghdwdm + "%'";
                            IFeatureCursor pDLTBGXGCCursor = pDLTBGXGCClass.Search(pSpatialFilter, true);
                            comRel2.ManageLifetime(pDLTBGXGCCursor);
                            IFeature pDLTBGXGC;
                            while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                            {
                                bool B = GetIntersectArea(pDLTBGXGC, pXZQGXGC);
                                if (B == true)
                                {
                                    string bsm = pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("BGQTBBSM")).ToString();
                                    bsms.Add(bsm);
                                    double TBMJ = double.Parse(pDLTBGXGC.get_Value(pDLTBGXGCClass.FindField("TBBGMJ")).ToString());
                                    BGMJ += TBMJ;
                                    DCMJ += TBMJ;
                                }
                                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGC);
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCCursor);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                        }
                        IGeometry pGeometry = pXZQGXGC.ShapeCopy;
                        ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
                        using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        {
                            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                            pSpatialFilter.Geometry = pXZQGXGC.ShapeCopy;
                            pSpatialFilter.GeometryField = pDLTBGXGCClass.ShapeFieldName;
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            pSpatialFilter.WhereClause = "ZLDWDM Like '" + bgqdwdm + "%'";
                            IFeatureCursor pDLTBCursor = pDLTBClass.Search(pSpatialFilter, true);
                            IFeature pDLTB;
                            while ((pDLTB = pDLTBCursor.NextFeature()) != null)
                            {
                                string bsm = pDLTB.get_Value(pDLTBClass.FindField("BSM")).ToString();
                                if (bsms.Contains(bsm)) continue;
                                IGeometry pGeoIntersect = pTop.Intersect(pDLTB.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                                if (pGeoIntersect != null)
                                {
                                    IArea pArea = pGeoIntersect as IArea;
                                    if (pArea.Area > 0.0001)
                                    {
                                        double TBMJ = double.Parse(pDLTB.get_Value(pDLTBClass.FindField("TBMJ")).ToString());
                                        BGMJ += TBMJ;
                                        DCMJ += TBMJ;
                                    }
                                    RCIS.Utility.OtherHelper.ReleaseComObject(pArea);

                                }
                                RCIS.Utility.OtherHelper.ReleaseComObject(pGeoIntersect);
                                RCIS.Utility.OtherHelper.ReleaseComObject(pDLTB);

                                //bool B = GetIntersectArea(pDLTB,pXZQGXGC);
                                //if(B==true)
                                //{
                                //    double TBMJ = double.Parse(pDLTB.get_Value(pDLTBClass.FindField("TBMJ")).ToString());
                                //    BGMJ += TBMJ;
                                //    DCMJ += TBMJ;
                                //}
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBCursor);

                        }
                        pXZQGXGC.set_Value(pXZQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pXZQGXGCCursor.UpdateFeature(pXZQGXGC);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGC);

                    }
                    pXZQGXGCCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCCursor);
                }
                dm_mj.Add(dwdm, DCMJ);
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pXZQGXGCClass as ITable, "BGHDCMJ", Math.Round(dm_mj[dwdm], 2), "BGHXZQDM = '" + dwdm + "'");
                Application.DoEvents();
            }
            UpdateStatus("开始计算行政区更新层面积");
            IFeatureClass pXZQGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGX");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            IFeatureCursor pCursor = pXZQGXClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("XZQDM")).ToString();
                if (currDh == 0)
                {
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号
                }
                double JSMJ = area.SphereArea(pFea.ShapeCopy, currDh);
                pFea.set_Value(pFea.Fields.FindField("JSMJ"), Math.Round(JSMJ, 2));
                pFea.set_Value(pFea.Fields.FindField("DCMJ"), Math.Round(dm_mj[zldwdm], 2));
                pCursor.UpdateFeature(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBClass);
            UpdateStatus("行政区面积计算完成");
        }

        private void JSMJXZQ(string awm)
        {
            //数据汇总流程
            //1.计算变更后行政区，行政区的图形范围
            //2.计算变更后行政区，行政区中变更图斑的面积
            //3.计算变更后行政区，行政区中三调图斑面积并汇总
            IWorkspace pTmpWs = null;
            IFeatureClass pTB = null;
            UpdateStatus("正在计算行政区更新过程层面积");

            pTmpWs = DeleteAndNewTmpGDB();
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWs, "DLTBGXGC", "TBGXGC", null);
            IFeatureClass pTBGXGC = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TBGXGC");
            ISchemaLock pSchemaLock = null;
            pSchemaLock = pTBGXGC as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = pTBGXGC as IClassSchemaEdit4;
            pClassSchemaEdit.AlterFieldName("TBBGMJ", "TBMJ");
            pClassSchemaEdit.AlterFieldName("BGHZLDWDM", "ZLDWDM");
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

            bool b = RCIS.GISCommon.GpToolHelper.Update(currWs.PathName + "\\TDDC\\DLTB", pTmpWs.PathName + "\\TBGXGC", pTmpWs.PathName + "\\TB");
            if (!b)
            {
                UpdateStatus("叠加分析错误");
                return;
            }
            pTB = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("TB");
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = pTB;
            IIdentify pIdentify = pLayer as IIdentify;

            RCIS.Utility.OtherHelper.ReleaseComObject(pTBGXGC);

            IFeatureClass pXZQGXGCClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGXGC");
            Dictionary<string, double> dm_mj = new Dictionary<string, double>();
            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pXZQGXGCClass, null, "BGHXZQDM");
            foreach (var item in dwdms)
            {
                string dwdm = item.ToString();
                double DCMJ = 0;
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHXZQDM Like '" + dwdm + "%'";
                    IFeatureCursor pXZQGXGCCursor = pXZQGXGCClass.Update(pQueryFilter, true);
                    comRel.ManageLifetime(pXZQGXGCCursor);
                    IFeature pXZQGXGC;
                    while ((pXZQGXGC = pXZQGXGCCursor.NextFeature()) != null)
                    {
                        double BGMJ = 0;
                        string bgqdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGQXZQDM")).ToString();
                        string bghdwdm = pXZQGXGC.get_Value(pXZQGXGCClass.FindField("BGHXZQDM")).ToString();

                        IArray pArr = pIdentify.Identify(pXZQGXGC.ShapeCopy);
                        if (pArr != null)
                        {
                            for (int i = 0; i < pArr.Count; i++)
                            {
                                IFeatureIdentifyObj idObj = pArr.get_Element(i) as IFeatureIdentifyObj;
                                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                                IFeature pFeature = pRow.Row as IFeature;
                                string zl = pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM")).ToString().Substring(0, bghdwdm.Length);
                                if (bghdwdm == zl)
                                {
                                    bool B = GetIntersectArea(pFeature, pXZQGXGC);
                                    if (B == true)
                                    {
                                        double TBMJ = double.Parse(pFeature.get_Value(pTB.FindField("TBMJ")).ToString());
                                        BGMJ += TBMJ;
                                        DCMJ += TBMJ;
                                    }
                                    RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                                }
                            }
                        }

                        //using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                        //{
                        //    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                        //    pSpatialFilter.Geometry = pXZQGXGC.ShapeCopy;
                        //    pSpatialFilter.GeometryField = pTB.ShapeFieldName;
                        //    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                        //    pSpatialFilter.WhereClause = "ZLDWDM Like '" + bghdwdm + "%'";
                        //    IFeatureCursor pDLTBGXGCCursor = pTB.Search(pSpatialFilter, true);
                        //    comRel2.ManageLifetime(pDLTBGXGCCursor);
                        //    IFeature pDLTBGXGC;
                        //    while ((pDLTBGXGC = pDLTBGXGCCursor.NextFeature()) != null)
                        //    {
                        //        bool B = GetIntersectArea(pDLTBGXGC, pXZQGXGC);
                        //        if (B == true)
                        //        {
                        //            double TBMJ = double.Parse(pDLTBGXGC.get_Value(pTB.FindField("TBMJ")).ToString());
                        //            BGMJ += TBMJ;
                        //            DCMJ += TBMJ;
                        //        }
                        //        RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGC);
                        //    }
                        //    RCIS.Utility.OtherHelper.ReleaseComObject(pDLTBGXGCCursor);
                        //    RCIS.Utility.OtherHelper.ReleaseComObject(pSpatialFilter);
                        //}
                        pXZQGXGC.set_Value(pXZQGXGCClass.FindField("BGMJ"), Math.Round(BGMJ, 2));
                        pXZQGXGCCursor.UpdateFeature(pXZQGXGC);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGC);
                    }
                    pXZQGXGCCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCCursor);
                }
                dm_mj.Add(dwdm, DCMJ);
                RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pXZQGXGCClass as ITable, "BGHDCMJ", Math.Round(dm_mj[dwdm], 2), "BGHXZQDM = '" + dwdm + "'");
                Application.DoEvents();
            }
            UpdateStatus("正在计算行政区更新层面积");
            IFeatureClass pXZQGXClass = (currWs as IFeatureWorkspace).OpenFeatureClass("XZQGX");
            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
            int currDh = 0;
            IFeatureCursor pCursor = pXZQGXClass.Update(null, true);
            IFeature pFea;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string zldwdm = pFea.get_Value(pFea.Fields.FindField("XZQDM")).ToString();
                if (currDh == 0)
                {
                    ESRI.ArcGIS.Geometry.IPoint selectPoint = (pFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号
                }
                double JSMJ = area.SphereArea(pFea.ShapeCopy, currDh);
                pFea.set_Value(pFea.Fields.FindField("JSMJ"), Math.Round(JSMJ, 2));
                pFea.set_Value(pFea.Fields.FindField("DCMJ"), Math.Round(dm_mj[zldwdm], 2));
                pCursor.UpdateFeature(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQGXGCClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTB);
            //UpdateStatus("行政区面积计算完成");
        }

        private void ProduceXZQGXGC()
        {
            IFeatureWorkspace pFWor = currWs as IFeatureWorkspace;
            IFeatureClass pGXFeaClass = null;
            try
            {
                pGXFeaClass = pFWor.OpenFeatureClass("XZQGX");
                if (pGXFeaClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("行政区更新层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //message = false;
                    //return message;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到行政区更新层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //message = false;
                //return message;
            }
            //if (MessageBox.Show("原行政区更新过程层中的数据将被删除，然后重新生成，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
            UpdateStatus("正在处理更新层行政区数据");
            IGeometry pGeometry = RCIS.GISCommon.GeometryHelper.MergeGeometry(pGXFeaClass);
            UpdateStatus("正在查找与变化行政区重叠的三调行政区");
            IFeatureClass pXZQClass = null;
            try
            {
                pXZQClass = pFWor.OpenFeatureClass("XZQ");
                if (pXZQClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("行政区层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到行政区层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string XZQSD = Application.StartupPath + @"\tmp\XZQSD.shp";
            string XZQGXGC = Application.StartupPath + @"\tmp\XZQGXGC.shp";
            IFeatureClass pSDClass;
            if (File.Exists(XZQSD))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQSD);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            if (File.Exists(XZQGXGC))
            {
                pSDClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQGXGC);
                IDataset pDataset = pSDClass as IDataset;
                pDataset.Delete();
            }
            pSDClass = RCIS.GISCommon.WorkspaceHelper2.CreateSHP(XZQSD, esriGeometryType.esriGeometryPolygon, (pXZQClass as IGeoDataset).SpatialReference, pXZQClass.Fields);
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.GeometryField = pXZQClass.ShapeFieldName;
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //pSpatialFilter.SpatialRelDescription = "T********";
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pInsertCursor = pSDClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeatureCursor pSearchCursor = pXZQClass.Search(pSpatialFilter, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IGeometry pGeoIntersect = pTop.Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (pGeoIntersect != null)
                    {
                        IArea pArea = pGeoIntersect as IArea;
                        if (pArea.Area > 0.0001)
                        {
                            pInsertCursor.InsertFeature(pFeature as IFeatureBuffer);
                        }
                    }
                }
                pInsertCursor.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            UpdateStatus("正在进行叠加分析");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
            string gxPath = currWs.PathName + "\\TDGX\\XZQGX";
            pUnion.in_features = XZQSD + ";" + gxPath;
            pUnion.out_feature_class = XZQGXGC;
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                UpdateStatus("叠加分析错误");
                //message = false;
                //return message;
            }
            string MultipartToSinglepart = Application.StartupPath + "\\tmp\\MultipartToSinglepart.shp";
            if (File.Exists(MultipartToSinglepart))
            {
                ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart)) as IDataset).Delete();
            }
            bool result = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(XZQGXGC, MultipartToSinglepart);
            if (result == false)
            {
                UpdateStatus("叠加分析错误");
                //message = false;
                //return message;
            }
            UpdateStatus("正在导入叠加结果");
            ((RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(XZQGXGC)) as IDataset).Delete();
            IFeatureClass pSourClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(MultipartToSinglepart);
            IFeatureClass pTarClass = null;
            try
            {
                pTarClass = pFWor.OpenFeatureClass("XZQGXGC");
            }
            catch
            {
                UpdateStatus("数据库没有升级，请先升级数据库。");
                //message = false;
                //return message;
            }
            string where = "";
            if (xzdm.Length > 0)
                where = "XZQDM LIKE '" + xzdm + "%'";


            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQClass, "BSM", where) + 1;
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pGXFeaClass, "BSM", where) + 1;
            long maxBSM = 0;
            if (max1 > max2)
                maxBSM = max1;
            else
                maxBSM = max2;

            //long maxBSM = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pXZQClass,"BSM")+1;
            ITable pTable = pTarClass as ITable;
            pTable.DeleteSearchedRows(null);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureCursor pSearchCursor = pSourClass.Search(null, true);
                comRel.ManageLifetime(pSearchCursor);
                IFeatureCursor pInsertCursor = pTarClass.Insert(true);
                comRel.ManageLifetime(pInsertCursor);
                IFeature pFeature;
                while ((pFeature = pSearchCursor.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    if (pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString().Trim() == pFeature.get_Value(pFeature.Fields.FindField("BSM_1")).ToString().Trim() || pFeature.get_Value(pFeature.Fields.FindField("BZ_1")).ToString().Trim() == "S")
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    else
                        pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "2");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BSM"), maxBSM++);

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pSourClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pSourClass.FindField("BZ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pSourClass.FindField("BSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pSourClass.FindField("XZQDM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pSourClass.FindField("XZQMC_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pSourClass.FindField("DCMJ_1")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pSourClass.FindField("JSMJ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pSourClass.FindField("BZ_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pSourClass.FindField("MSSM_1")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pSourClass.FindField("HDMC_1")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsertCursor.InsertFeature(pFeatureBuffer);
                }
                pInsertCursor.Flush();
            }
            UpdateStatus("正在提取同名的行政区");
            System.Collections.ArrayList xzqdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pTarClass, null, "BGQXZQDM");
            List<string> xzqdm = new List<string>();
            foreach (var item in xzqdms)
            {
                xzqdm.Add("XZQDM = '" + item.ToString() + "'");
            }
            string sWhere = string.Join(" Or ", xzqdm);
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pTarClass);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel7 = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ISpatialFilter pSpaFilter = new SpatialFilterClass();
                pSpaFilter.Geometry = pGeo;
                pSpaFilter.GeometryField = pXZQClass.ShapeFieldName;
                pSpaFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelRelation;
                pSpaFilter.SpatialRelDescription = "FF*F*****";
                pSpaFilter.WhereClause = sWhere;
                IFeatureCursor pSearch = pXZQClass.Search(pSpaFilter, true);
                comRel7.ManageLifetime(pSearch);
                IFeatureCursor pInsert = pTarClass.Insert(true);
                comRel7.ManageLifetime(pInsert);
                IFeature pFeature;
                while ((pFeature = pSearch.NextFeature()) != null)
                {
                    IFeatureBuffer pFeatureBuffer = pTarClass.CreateFeatureBuffer();
                    #region 各字段赋值
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBSM"), pFeature.get_Value(pXZQClass.FindField("BSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQDM"), pFeature.get_Value(pXZQClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQXZQMC"), pFeature.get_Value(pXZQClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQDCMJ"), pFeature.get_Value(pXZQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQBZ"), pFeature.get_Value(pXZQClass.FindField("BZ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGQJSMJ"), pFeature.get_Value(pXZQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQMSSM"), pFeature.get_Value(pXZQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGQHDMC"), pFeature.get_Value(pXZQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), pFeature.get_Value(pXZQClass.FindField("BSM")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHBSM"), maxBSM++);
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGXW"), "1");
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQDM"), pFeature.get_Value(pXZQClass.FindField("XZQDM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHXZQMC"), pFeature.get_Value(pXZQClass.FindField("XZQMC")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHDCMJ"), pFeature.get_Value(pXZQClass.FindField("DCMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHBZ"), pFeature.get_Value(pXZQClass.FindField("BZ")));
                    //pFeatureBuffer.set_Value(pTarClass.FindField("BGHJSMJ"), pFeature.get_Value(pXZQClass.FindField("JSMJ")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHMSSM"), pFeature.get_Value(pXZQClass.FindField("MSSM")));
                    pFeatureBuffer.set_Value(pTarClass.FindField("BGHHDMC"), pFeature.get_Value(pXZQClass.FindField("HDMC")));

                    pFeatureBuffer.set_Value(pTarClass.FindField("GXSJ"), bgData);
                    #endregion
                    pInsert.InsertFeature(pFeatureBuffer);
                    //RCIS.GISCommon.FeatureHelper.UpdateFieldValues(pTarClass as ITable, "BGXW", "2", "BGQXZQDM = '" + pFeature.get_Value(pXZQClass.FindField("XZQDM")) + "'");
                }
                pInsert.Flush();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pXZQClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSDClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeaClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTarClass);
            RCIS.Utility.OtherHelper.ReleaseComObject(pSourClass);
            currWs.ExecuteSQL("update xzqgx set bz=''");
            currWs.ExecuteSQL("update xzqgxgc set bghbz=''");
            currWs.ExecuteSQL("delete from xzqgxgc where BGQBSM is null or BGHBSM is null or BGQBSM='' or BGHBSM=''");

            //if (message == true)
            //{
            JSMJXZQ("");
            //ProduceXZQJXGX();
            XZQJX1();
            UpdateStatus("行政区更新过程层提取完毕");

        }

        private void ProduceDLTBGXGC()
        {
            UpdateStatus("正在提取DLTB内的变化图斑。");

            //UpdateStatus("开始生成更新过程图层");
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            IFeatureClass pDLTB = null;
            try
            {
                pDLTB = pFeatureWorkspace.OpenFeatureClass("DLTB");
                if (pDLTB.FeatureCount(null) == 0)
                {
                    MessageBox.Show("地类图斑层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到地类图斑层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            IFeatureClass pGXGCFeatureCLass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            IFeatureClass pGXFeatureClass = null;
            try
            {
                pGXFeatureClass = pFeatureWorkspace.OpenFeatureClass("DLTBGX");
                if (pGXFeatureClass.FeatureCount(null) == 0)
                {
                    MessageBox.Show("地类图斑更新层为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到地类图斑更新层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            string where = "";
            if (xzdm.Length > 0)
                where = "ZLDWDM LIKE '" + xzdm + "%'";


            long maxNum;
            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pGXFeatureClass, "BSM", where);
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTB, "BSM", where);
            if (max1 > max2)
                maxNum = max1 + 1;
            else
                maxNum = max2 + 1;
            //删除更新过程层数据
            bool del = RCIS.GISCommon.GpToolHelper.DeleteFeatures(currWs.PathName + "\\TDGX\\DLTBGXGC");
            if (!del)
            {
                MessageBox.Show("数据删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //ITable pTable = pGXGCFeatureCLass as ITable;
            //pTable.DeleteSearchedRows(null);
            //获取变化图斑数据
            //IFeatureClass pBHFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txt);
            //提取出三调地类图斑内与变化图斑相交的图斑   保存到临时tmp内  
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = DeleteAndNewTmpGDB();

            //为更新过程层添加变更前后图斑编号字段
            IFeatureClass pGXGC = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            if (pGXGC.FindField("BGQTBBH") == -1)
            {
                IField pField = EsriDatabaseHelper.CreateTextField("BGQTBBH", "变更前图斑编号", 8);
                IClass pClass = pGXGC as IClass;
                pClass.AddField(pField);
                //pClass.AddIndex(
            }
            if (pGXGC.FindField("BGHTBBH") == -1)
            {
                IField pField = EsriDatabaseHelper.CreateTextField("BGHTBBH", "变更后图斑编号", 8);
                IClass pClass = pGXGC as IClass;
                pClass.AddField(pField);
            }

            bool spatialJoin = RCIS.GISCommon.GpToolHelper.SpatialJoin_analysis(currWs.PathName + "\\TDDC\\DLTB", currWs.PathName + "\\TDGX\\DLTBGX", tmpWS.PathName + "\\sdtb");
            if (!spatialJoin)
            {
                UpdateStatus("查找三调图斑失败");
                return;
            }
            tmpWS.ExecuteSQL("delete from sdtb where Join_Count=0");
            IFeatureClass pSDFeatureClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("sdtb");
            for (int i = pSDFeatureClass.Fields.FieldCount - 1; i >= 0; i--)
            {
                IField pField = pSDFeatureClass.Fields.get_Field(i);
                if (pField.Name.Contains("_1"))
                    (pSDFeatureClass as ITable).DeleteField(pField);
            }

            //将变化图斑与三调图斑尽行融合 存到DLTBGXGC内
            UpdateStatus("正在生成地类图斑更新过程层数据。");

            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
            string gxPath = currWs.PathName + "\\TDGX\\DLTBGX";
            tmp += "\\sdtb";
            pUnion.in_features = tmp + ";" + gxPath;
            pUnion.out_feature_class = Application.StartupPath + "\\tmp\\tmp.gdb\\rh";
            pUnion.join_attributes = "ALL";
            try
            {
                gp.Execute(pUnion, null);
            }
            catch
            {
                string mess = "";
                for (int i = 0; i < gp.MessageCount; i++)
                {
                    mess += gp.GetMessage(i) + "\r\n";
                }
                MessageBox.Show(mess.ToString());
                UpdateStatus("叠加分析错误");
                return;
            }

            bool b = RCIS.GISCommon.GpToolHelper.MultipartToSinglepart(tmpWS.PathName + "\\rh", tmpWS.PathName + "\\multipartRH");


            //
            //循环DLTBGXGC图层进行属性赋值
            UpdateStatus("正在为地类图斑更新过程层数据添加属性。");
            IFeatureClass pRHFeatureclass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("multipartRH");
            ISchemaLock pSchemaLock = null;
            pSchemaLock = pRHFeatureclass as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = pRHFeatureclass as IClassSchemaEdit4;

            pClassSchemaEdit.AlterFieldName("BSM", "BGQTBBSM");
            pClassSchemaEdit.AlterFieldName("TBBH", "BGQTBBH");
            pClassSchemaEdit.AlterFieldName("DLBM", "BGQDLBM");
            pClassSchemaEdit.AlterFieldName("DLMC", "BGQDLMC");
            pClassSchemaEdit.AlterFieldName("QSXZ", "BGQQSXZ");
            pClassSchemaEdit.AlterFieldName("QSDWDM", "BGQQSDWDM");
            pClassSchemaEdit.AlterFieldName("QSDWMC", "BGQQSDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWMC", "BGQZLDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWDM", "BGQZLDWDM");
            pClassSchemaEdit.AlterFieldName("KCDLBM", "BGQKCDLBM");
            pClassSchemaEdit.AlterFieldName("KCXS", "BGQKCXS");
            pClassSchemaEdit.AlterFieldName("KCMJ", "BGQKCMJ");
            pClassSchemaEdit.AlterFieldName("TBDLMJ", "BGQTBDLMJ");
            pClassSchemaEdit.AlterFieldName("GDLX", "BGQGDLX");
            pClassSchemaEdit.AlterFieldName("GDPDJB", "BGQGDPDJB");
            pClassSchemaEdit.AlterFieldName("XZDWKD", "BGQXZDWKD");
            pClassSchemaEdit.AlterFieldName("TBXHDM", "BGQTBXHDM");
            pClassSchemaEdit.AlterFieldName("TBXHMC", "BGQTBXHMC");
            pClassSchemaEdit.AlterFieldName("ZZSXDM", "BGQZZSXDM");
            pClassSchemaEdit.AlterFieldName("ZZSXMC", "BGQZZSXMC");
            pClassSchemaEdit.AlterFieldName("GDDB", "BGQGDDB");
            pClassSchemaEdit.AlterFieldName("FRDBS", "BGQFRDBS");
            pClassSchemaEdit.AlterFieldName("CZCSXM", "BGQCZCSXM");
            pClassSchemaEdit.AlterFieldName("MSSM", "BGQMSSM");
            pClassSchemaEdit.AlterFieldName("HDMC", "BGQHDMC");

            pClassSchemaEdit.AlterFieldName("BSM_1", "BGHTBBSM");
            pClassSchemaEdit.AlterFieldName("TBBH_1", "BGHTBBH");
            pClassSchemaEdit.AlterFieldName("DLBM_1", "BGHDLBM");
            pClassSchemaEdit.AlterFieldName("DLMC_1", "BGHDLMC");
            pClassSchemaEdit.AlterFieldName("QSXZ_1", "BGHQSXZ");
            pClassSchemaEdit.AlterFieldName("QSDWDM_1", "BGHQSDWDM");
            pClassSchemaEdit.AlterFieldName("QSDWMC_1", "BGHQSDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWMC_1", "BGHZLDWMC");
            pClassSchemaEdit.AlterFieldName("ZLDWDM_1", "BGHZLDWDM");
            pClassSchemaEdit.AlterFieldName("KCDLBM_1", "BGHKCDLBM");
            pClassSchemaEdit.AlterFieldName("KCXS_1", "BGHKCXS");
            pClassSchemaEdit.AlterFieldName("KCMJ_1", "BGHKCMJ");
            pClassSchemaEdit.AlterFieldName("TBDLMJ_1", "BGHTBDLMJ");
            pClassSchemaEdit.AlterFieldName("GDLX_1", "BGHGDLX");
            pClassSchemaEdit.AlterFieldName("GDPDJB_1", "BGHGDPDJB");
            pClassSchemaEdit.AlterFieldName("XZDWKD_1", "BGHXZDWKD");
            pClassSchemaEdit.AlterFieldName("TBXHDM_1", "BGHTBXHDM");
            pClassSchemaEdit.AlterFieldName("TBXHMC_1", "BGHTBXHMC");
            pClassSchemaEdit.AlterFieldName("ZZSXDM_1", "BGHZZSXDM");
            pClassSchemaEdit.AlterFieldName("ZZSXMC_1", "BGHZZSXMC");
            pClassSchemaEdit.AlterFieldName("GDDB_1", "BGHGDDB");
            pClassSchemaEdit.AlterFieldName("FRDBS_1", "BGHFRDBS");
            pClassSchemaEdit.AlterFieldName("CZCSXM_1", "BGHCZCSXM");
            pClassSchemaEdit.AlterFieldName("MSSM_1", "BGHMSSM");
            pClassSchemaEdit.AlterFieldName("HDMC_1", "BGHHDMC");

            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);

            tmpWS.ExecuteSQL("delete from multipartRH where BGQTBBSM is null or BGHTBBSM is null or BGQTBBSM='' or BGHTBBSM=''");
            //tmpWS.ExecuteSQL("delete from multipartRH where  BGHTBBSM is null or BGHTBBSM=''");
            IField field = RCIS.GISCommon.EsriDatabaseHelper.CreateTextField("BGXW", "变更行为", 1);
            pRHFeatureclass.AddField(field);

            tmpWS.ExecuteSQL("update multipartRH set bgxw='1' where bgqtbbsm=bghtbbsm");
            tmpWS.ExecuteSQL("update multipartRH set bgxw='2' where bgqtbbsm<>bghtbbsm");
            tmpWS.ExecuteSQL("update multipartRH set bgxw='1' where bz_1='S'");

            b = RCIS.GISCommon.GpToolHelper.Append(tmpWS.PathName + "\\multipartRH", currWs.PathName + "\\TDGX\\DLTBGXGC");
            b = RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGXGC");

            currWs.ExecuteSQL("update dltbgx set BZ=''");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx=''");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='0' where substring(BGQZLDWDM,1,6)= '" + xzdm + "' and substring(BGHZLDWDM,1,6) = '" + xzdm + "'");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='1' where substring(BGQZLDWDM,1,6)<> '" + xzdm + "' and substring(BGHZLDWDM,1,6) = '" + xzdm + "'");
            currWs.ExecuteSQL("update dltbgxgc set xzqtzlx='2' where substring(BGQZLDWDM,1,6)= '" + xzdm + "' and substring(BGHZLDWDM,1,6) <> '" + xzdm + "'");

            RCIS.Utility.OtherHelper.ReleaseComObject(pDLTB);
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXFeatureClass);

            //计算变更面积

            //JSMJ(tmpWS);
            MJTP(pGXGCFeatureCLass, maxNum);
            JSMJGX(tmpWS);

            UpdateStatus("地类图斑更新过程层提取完毕。");

        }

        private void MJTP(IFeatureClass gxgcClass, long maxBsm)
        {
            //对所有更新过程数据，按照更新时间进行分组
            //ArrayList BGHTBBSM = FeatureHelper.GetUniqueFieldValueByDataStatistics(gxgcClass, null, "BGHTBBSM");
            UpdateStatus("正在重排地类图斑更新过程层标识码。");
            List<string> bgqBsms = new List<string>();
            List<double> bgqZmj = new List<double>();
            //找到该更新时间 对应所有数据，
            IFeatureCursor cursor = gxgcClass.Update(null, false);
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
                    if (tbmj <= 0.1)
                    {
                        string bgqbsm = aFea.get_Value(aFea.Fields.FindField("BGQTBBSM")).ToString();
                        string bghbsm = aFea.get_Value(aFea.Fields.FindField("BGHTBBSM")).ToString();
                        ISpatialFilter pSF = new SpatialFilterClass();
                        bool del = false;
                        pSF.Geometry = aFea.ShapeCopy;
                        pSF.GeometryField = gxgcClass.ShapeFieldName;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
                        pSF.WhereClause = "BGHTBBSM='" + bghbsm + "'";
                        // and BGHTBBSM='" + bghbsm + "'
                        IFeatureCursor pCursor = gxgcClass.Update(pSF, true);
                        IFeature pFeature;
                        while ((pFeature = pCursor.NextFeature()) != null)
                        {
                            if (GetIntersectLen(aFea, pFeature))
                            {
                                IPolygon pPoylon = UnionPolygon(aFea.ShapeCopy, pFeature.ShapeCopy);
                                pFeature.Shape = pPoylon;
                                double qmmj = area.SphereArea(pPoylon, currDh);
                                pFeature.set_Value(aFea.Fields.FindField("TBBGMJ"), qmmj);
                                aFea.Delete();
                                pCursor.UpdateFeature(pFeature);
                                pCursor.Flush();
                                del = true;
                                break;
                            }
                            RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                        }
                        if (!del)
                        {
                            aFea.Delete();
                            //cursor.UpdateFeature(pFeature);
                        }

                        RCIS.Utility.OtherHelper.ReleaseComObject(pSF);
                        RCIS.Utility.OtherHelper.ReleaseComObject(aFea);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                        continue;
                    }
                    aFea.set_Value(aFea.Fields.FindField("TBBGMJ"), tbmj);
                    aFea.set_Value(aFea.Fields.FindField("BSM"), maxBsm++);
                    aFea.set_Value(aFea.Fields.FindField("GXSJ"), bgData);
                    cursor.UpdateFeature(aFea);
                    //计算变更前总面积
                    string bsm = FeatureHelper.GetFeatureStringValue(aFea, "BGQTBBSM");
                    if (!bgqBsms.Contains(bsm))
                    {
                        bgqBsms.Add(bsm);
                        double bgqtbmj = FeatureHelper.GetFeatureDoubleValue(aFea, "BGQTBDLMJ") + FeatureHelper.GetFeatureDoubleValue(aFea, "BGQKCMJ");
                        bgqZmj.Add(bgqtbmj);
                    }
                    RCIS.Utility.OtherHelper.ReleaseComObject(aFea);
                }
                cursor.Flush();
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
            //currWs.ExecuteSQL("delete from dltbgxgx where shape_area<0.01");

            UpdateStatus("正在计算地类图斑更新过程层面积。");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(gxgcClass, "BGQTBBSM");
            for (int kk = 0; kk < bgqBsms.Count; kk++)
            {
                double abgqzmj = bgqZmj[kk];
                string absm = bgqBsms[kk];

                //计算变更后图斑面积和
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "BGQTBBSM ='" + absm + "' ";
                double bghZmj = 0;

                ArrayList BghFeatures = new ArrayList();
                IFeatureCursor cur = gxgcClass.Search(pQF, false);
                IFeature abghfea = null;
                try
                {
                    while ((abghfea = cur.NextFeature()) != null)
                    {
                        BghFeatures.Add(abghfea);
                        bghZmj += FeatureHelper.GetFeatureDoubleValue(abghfea, "TBBGMJ");
                        //RCIS.Utility.OtherHelper.ReleaseComObject(abghfea);
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
                    IFeature aTPFea = BghFeatures[0] as IFeature;
                    aTPFea.set_Value(aTPFea.Fields.FindField("TBBGMJ"), abgqzmj);
                    double tbmj = MathHelper.RoundEx(abgqzmj, 2);
                    double bgqkcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGQKCXS");
                    double bgqkcmj = MathHelper.RoundEx(tbmj * bgqkcxs, 2);
                    aTPFea.set_Value(aTPFea.Fields.FindField("BGQTBDLMJ"), MathHelper.RoundEx(tbmj - bgqkcmj, 2));
                    aTPFea.set_Value(aTPFea.Fields.FindField("BGQKCMJ"), MathHelper.RoundEx(bgqkcmj, 2));
                    double kcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGHKCXS");
                    if (kcxs > 0)
                    {
                        double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGHKCMJ"), kcmj);
                        double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), dlmj);
                    }
                    else
                    {
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), tbmj);
                    }
                    aTPFea.Store();
                    RCIS.Utility.OtherHelper.ReleaseComObject(aTPFea);
                }
                else
                {
                    //变更后 按照图斑面积排序
                    IComparer comparer = new TBBGMJ();
                    BghFeatures.Sort(comparer);
                    //调平
                    bool zf = true;
                    double diff = MathHelper.RoundEx(abgqzmj - bghZmj, 2);
                    //if (Math.Abs(diff) < 0.0001) continue;
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

                        double bghtbmj = FeatureHelper.GetFeatureDoubleValue(aTPFea, "TBBGMJ");
                        if (Math.Abs(diff) > 0.0001)
                        {
                            if (i < yushu)
                            {
                                bghtbmj += (zf ? 0.01 * (shang + 1) : -0.01 * (shang + 1));
                            }
                            else
                            {
                                bghtbmj += (zf ? 0.01 * shang : -0.01 * shang);
                            }
                        }
                        double tbmj = MathHelper.RoundEx(bghtbmj, 2);
                        aTPFea.set_Value(aTPFea.Fields.FindField("TBBGMJ"), tbmj);
                        double bgqkcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGQKCXS");
                        double bgqkcmj = MathHelper.RoundEx(tbmj * bgqkcxs, 2);
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGQTBDLMJ"), MathHelper.RoundEx(tbmj - bgqkcmj, 2));
                        aTPFea.set_Value(aTPFea.Fields.FindField("BGQKCMJ"), MathHelper.RoundEx(bgqkcmj, 2));
                        double kcxs = FeatureHelper.GetFeatureDoubleValue(aTPFea, "BGHKCXS");
                        if (kcxs > 0)
                        {
                            double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHKCMJ"), kcmj);
                            double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), dlmj);
                        }
                        else
                        {
                            aTPFea.set_Value(aTPFea.Fields.FindField("BGHTBDLMJ"), tbmj);
                        }
                        aTPFea.Store();
                        RCIS.Utility.OtherHelper.ReleaseComObject(aTPFea);
                    }

                }
                //RCIS.Utility.OtherHelper.ReleaseComObject(BghFeatures);
            }
            RCIS.GISCommon.DatabaseHelper.DeleteIndex(gxgcClass, "BGQTBBSM");
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGXGC");
            RCIS.GISCommon.GpToolHelper.RepairGeometry(currWs.PathName + "\\TDGX\\DLTBGX");

            //重新计算图斑地类面积
            //IFeatureCursor updateCursor = gxgcClass.Update(null, false);
            //IFeature aupdateFea = null;
            //try
            //{
            //    while ((aupdateFea = updateCursor.NextFeature()) != null)
            //    {
            //        //调平的时候顺便把 地类面积一起计算
            //        double tbmj = FeatureHelper.GetFeatureDoubleValue(aupdateFea, "TBBGMJ");

            //        double bgqkcxs = FeatureHelper.GetFeatureDoubleValue(aupdateFea, "BGQKCXS");
            //        double bgqkcmj = MathHelper.RoundEx(tbmj * bgqkcxs, 2);
            //        FeatureHelper.SetFeatureValue(aupdateFea, "BGQTBDLMJ", MathHelper.RoundEx(tbmj - bgqkcmj, 2));
            //        FeatureHelper.SetFeatureValue(aupdateFea, "BGQKCMJ", MathHelper.RoundEx(bgqkcmj, 2));

            //        double kcxs = FeatureHelper.GetFeatureDoubleValue(aupdateFea, "BGHKCXS");
            //        if (kcxs > 0)
            //        {
            //            double kcmj = MathHelper.RoundEx(tbmj * kcxs, 2);
            //            FeatureHelper.SetFeatureValue(aupdateFea, "BGHKCMJ", kcmj);
            //            double dlmj = MathHelper.RoundEx(tbmj - kcmj, 2);
            //            FeatureHelper.SetFeatureValue(aupdateFea, "BGHTBDLMJ", dlmj);
            //        }
            //        else
            //        {
            //            FeatureHelper.SetFeatureValue(aupdateFea, "BGHTBDLMJ", tbmj);
            //        }
            //        aupdateFea.Store();
            //    }
            //}
            //catch { }
            //finally
            //{
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            //}
        }

        public static IPolygon UnionPolygon(IGeometry pGeo1, IGeometry pGeo2)
        {
            object missing = Type.Missing;
            IGeometry geometryBag = new GeometryBagClass();
            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            geometryCollection.AddGeometry(pGeo1, ref missing, ref missing);
            geometryCollection.AddGeometry(pGeo2, ref missing, ref missing);
            ITopologicalOperator union = new PolygonClass();
            union.ConstructUnion(geometryBag as IEnumGeometry);
            IPolygon retPolygon = union as IPolygon;
            return retPolygon;
        }

        private void JSMJGX(IWorkspace pTmpWS)
        {
            UpdateStatus("正在计算地类图斑更新层面积");
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            string filePath = currWs.PathName;
            ESRI.ArcGIS.AnalysisTools.Statistics pStatistics = new ESRI.ArcGIS.AnalysisTools.Statistics();
            pStatistics.in_table = filePath + @"\TDGX\DLTBGXGC";
            pStatistics.out_table = pTmpWS.PathName + @"\DLTBGXTMP";
            pStatistics.case_field = "BGHTBBSM";
            pStatistics.statistics_fields = "BGHKCMJ SUM;BGHTBDLMJ SUM";
            try
            {
                gp.Execute(pStatistics, null);
            }
            catch
            {
                UpdateStatus("汇总错误");
                return;
            }
            List<string> bsms = new List<string>();
            IFeatureWorkspace pFeatureWorkspace = currWs as IFeatureWorkspace;
            ITable pTable = (pTmpWS as IFeatureWorkspace).OpenTable("DLTBGXTMP");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pTable, "BGHTBBSM");
            //IFeatureClass pGXGCClass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            IFeatureClass pGXClass = pFeatureWorkspace.OpenFeatureClass("DLTBGX");
            //IFeatureClass pDLTBClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            //RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBClass, "BSM");
            IFeatureCursor pFeatureCursor = pGXClass.Update(null, true);
            IFeature pFeature;
            while ((pFeature = pFeatureCursor.NextFeature()) != null)
            {
                string bsm = pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString();
                using (ESRI.ArcGIS.ADF.ComReleaser comRel21 = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    pQueryFilter.WhereClause = "BGHTBBSM = '" + bsm + "'";
                    ICursor pCursor = pTable.Search(pQueryFilter, true);
                    comRel21.ManageLifetime(pCursor);
                    IRow pRow = pCursor.NextRow();
                    if (pRow == null)
                        continue;
                    double sumkcmj = 0;
                    double.TryParse(pRow.get_Value(pTable.FindField("SUM_BGHKCMJ")).ToString(), out sumkcmj);
                    double sumtbdlmj = 0;
                    double.TryParse(pRow.get_Value(pTable.FindField("SUM_BGHTBDLMJ")).ToString(), out sumtbdlmj);

                    double tbmj = Math.Round(sumkcmj + sumtbdlmj, 2);
                    pFeature.set_Value(pGXClass.FindField("TBMJ"), tbmj);
                    double kcxs = 0;
                    double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("KCXS")).ToString(), out kcxs);
                    double kcmj = Math.Round(tbmj * kcxs, 2);
                    pFeature.set_Value(pGXClass.FindField("KCMJ"), kcmj);
                    pFeature.set_Value(pGXClass.FindField("TBDLMJ"), Math.Round(tbmj - kcmj, 2));
                    pFeatureCursor.UpdateFeature(pFeature);
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                }
            }
            //pFeatureCursor.Flush();
            //RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureCursor);
            ////计算线状地物宽度
            //IFeatureClass pDLTB = pFeatureWorkspace.OpenFeatureClass("DLTB");
            //IFeatureClass pGXGCClass = pFeatureWorkspace.OpenFeatureClass("DLTBGXGC");
            //RCIS.GISCommon.DatabaseHelper.CreateIndex(pGXGCClass, "BGHTBBSM");
            //string[] xzdw = { "1001", "1002", "1003", "1004", "1006", "1009", "1107", "1101", "1107A" };
            //IQueryFilter pQF = new QueryFilterClass();
            //pQF.WhereClause = "DLBM='1001' or DLBM='1002' or DLBM='1003' or DLBM='1004' or DLBM='1006' or DLBM='1009' or DLBM='1107' or  DLBM='1101' or DLBM='1107A'";
            //IFeatureCursor pXZDWcursor = pGXClass.Update(pQF, true);
            //IFeature pXZDW;
            //IFeatureLayer pLayer = new FeatureLayerClass();
            //pLayer.FeatureClass = pDLTB;
            //IIdentify pIdentify = pLayer as IIdentify;
            //while ((pXZDW = pXZDWcursor.NextFeature()) != null)
            //{

            //    double xzdwkd = 0;
            //    bool b = false;
            //    IRelationalOperator pRel = pXZDW.ShapeCopy as IRelationalOperator;
            //    IArray pArry = pIdentify.Identify(pXZDW.ShapeCopy);
            //    if (pArry != null)
            //    {
            //        for (int i = 0; i < pArry.Count; i++)
            //        {
            //            IFeatureIdentifyObj idObj = pArry.get_Element(i) as IFeatureIdentifyObj;
            //            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
            //            IFeature pFea = pRow.Row as IFeature;
            //            if (pRel.Equals(pFea.ShapeCopy))
            //            {
            //                if (pXZDW.get_Value(pXZDW.Fields.FindField("DLBM")).ToString().Trim() == pFea.get_Value(pFea.Fields.FindField("DLBM")).ToString().Trim())
            //                {
            //                    double.TryParse(pFea.get_Value(pFea.Fields.FindField("XZDWKD")).ToString(), out xzdwkd);
            //                    b = true;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    if (b == false)
            //    {
            //        double mj = (pXZDW.Shape as IArea).Area;
            //        double len = (pXZDW.Shape as IPolygon).Length;
            //        len = len / 2;
            //        xzdwkd = mj / len;
            //    }

            //    pXZDW.set_Value(pXZDW.Fields.FindField("XZDWKD"), MathHelper.RoundEx(xzdwkd, 1));
            //    //RCIS.Utility.OtherHelper.ReleaseComObject(cursor);
            //    pXZDWcursor.UpdateFeature(pXZDW);

            //    string bsm = pXZDW.get_Value(pXZDW.Fields.FindField("BSM")).ToString();
            //    IQueryFilter pQueryFil = new QueryFilterClass();
            //    pQueryFil.WhereClause = "BGHTBBSM='"+bsm+"'";

            //    IFeatureCursor pGXGCCursor = pGXGCClass.Update(pQueryFil, true);
            //    IFeature pGXGCFea;
            //    while ((pGXGCFea = pGXGCCursor.NextFeature()) != null)
            //    {
            //        pGXGCFea.set_Value(pGXGCFea.Fields.FindField("BGHXZDWKD"), MathHelper.RoundEx(xzdwkd, 1));
            //        pGXGCCursor.UpdateFeature(pGXGCFea);
            //    }
            //    pGXGCCursor.Flush();
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pGXGCCursor);

            //}
            //RCIS.GISCommon.DatabaseHelper.DeleteIndex(pGXGCClass, "BGHTBBSM");
            //RCIS.Utility.OtherHelper.ReleaseComObject(pXZDWcursor);
            RCIS.Utility.OtherHelper.ReleaseComObject(pGXClass);

            (pTable as IDataset).Delete();
            //UpdateStatus("地类图斑更新层变更面积计算完成。");
        }

        private void JSMJ(IWorkspace pTmpWS)
        {
            //面积计算流程
            //1.计算变更前标识码和次数，从而知道变更前图斑对应分割的个数
            //2.按照变更前标识码和次数关系分摊面积
            UpdateStatus("正在计算地类图斑更新过程层中的三调图斑标识码。");
            //构造Geoprocessor
            ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
            gp.OverwriteOutput = true;
            string filePath = currWs.PathName;
            ESRI.ArcGIS.AnalysisTools.Frequency pFrequency = new ESRI.ArcGIS.AnalysisTools.Frequency();
            pFrequency.in_table = filePath + @"\TDGX\DLTBGXGC";
            pFrequency.out_table = pTmpWS.PathName + @"\DLTBGXGCTMP";
            pFrequency.frequency_fields = "BGQTBBSM";
            pFrequency.summary_fields = "SHAPE_Area";
            try
            {
                gp.Execute(pFrequency, null);
            }
            catch
            {
                UpdateStatus("计算错误");
                return;
            }
            UpdateStatus("正在计算地类图斑更新过程层面积。");
            ITable pTable = (pTmpWS as IFeatureWorkspace).OpenTable("DLTBGXGCTMP");
            IFeatureClass pFeatureClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pFeatureClass, "BGQTBBSM");
            IFeatureClass pDLTBClass = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pDLTBClass, "BSM");


            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                ICursor pCursor = pTable.Search(null, true);
                comRel.ManageLifetime(pCursor);
                IRow pRow;
                while ((pRow = pCursor.NextRow()) != null)
                {
                    int count = (int)pRow.get_Value(pTable.FindField("FREQUENCY"));
                    string bsm = (string)pRow.get_Value(pTable.FindField("BGQTBBSM"));
                    double mj = (double)pRow.get_Value(pTable.FindField("SHAPE_Area"));
                    using (ESRI.ArcGIS.ADF.ComReleaser comRel2 = new ESRI.ArcGIS.ADF.ComReleaser())
                    {
                        IQueryFilter pDLTBFilter = new QueryFilterClass();
                        pDLTBFilter.WhereClause = "BSM = '" + bsm + "'";
                        IFeatureCursor pDLTBCursor = pDLTBClass.Search(pDLTBFilter, true);
                        comRel2.ManageLifetime(pDLTBCursor);
                        IFeature pDLTB = pDLTBCursor.NextFeature();
                        if (pDLTB == null)
                            continue;
                        IQueryFilter pQueryFilter = new QueryFilterClass();
                        pQueryFilter.WhereClause = "BGQTBBSM = '" + bsm + "'";
                        IFeatureCursor pFeatureCursor = pFeatureClass.Update(pQueryFilter, true);
                        comRel2.ManageLifetime(pFeatureCursor);
                        IFeature pFeature;
                        if (count == 1)
                        {
                            pFeature = pFeatureCursor.NextFeature();
                            double kcmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("KCMJ")).ToString(), out kcmj);
                            double dlmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("TBDLMJ")).ToString(), out dlmj);
                            double tbmj = kcmj + dlmj;
                            pFeature.set_Value(pFeatureClass.FindField("TBBGMJ"), tbmj);
                            double bgqkcxs = 0;
                            double.TryParse(pFeature.get_Value(pFeature.Fields.FindField("BGQKCXS")).ToString(), out bgqkcxs);
                            double bgqkcmj = Math.Round(tbmj * bgqkcxs, 2);
                            pFeature.set_Value(pFeatureClass.FindField("BGQKCMJ"), bgqkcmj);
                            pFeature.set_Value(pFeatureClass.FindField("BGQTBDLMJ"), Math.Round(tbmj - bgqkcmj, 2));
                            double bghkcxs = 0;
                            double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGHKCXS")).ToString(), out bghkcxs);
                            double bghkcmj = Math.Round(tbmj * bghkcxs, 2);
                            pFeature.set_Value(pFeatureClass.FindField("BGHKCMJ"), bghkcmj);
                            pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), Math.Round(tbmj - bghkcmj, 2));
                            pFeatureCursor.UpdateFeature(pFeature);
                        }
                        else
                        {
                            pFeature = pFeatureCursor.NextFeature();
                            double sumkcmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("KCMJ")).ToString(), out sumkcmj);
                            double sumdlmj = 0;
                            double.TryParse(pDLTB.get_Value(pDLTBClass.FindField("TBDLMJ")).ToString(), out sumdlmj);
                            double subkcmj = sumkcmj;
                            double subdlmj = sumdlmj;
                            for (int i = 1; i < count; i++)
                            {
                                double txmj = (double)pFeature.get_Value(pFeatureClass.FindField("SHAPE_Area"));
                                double bgmj = Math.Round(txmj / mj * (sumkcmj + sumdlmj), 2);
                                pFeature.set_Value(pFeatureClass.FindField("TBBGMJ"), bgmj);

                                double bgqkcxs = 0;
                                double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGQKCXS")).ToString(), out bgqkcxs);
                                double bgqkcmj = Math.Round(bgmj * bgqkcxs, 2);
                                pFeature.set_Value(pFeatureClass.FindField("BGQKCMJ"), bgqkcmj);
                                pFeature.set_Value(pFeatureClass.FindField("BGQTBDLMJ"), Math.Round(bgmj - bgqkcmj, 2));
                                double bghkcxs = 0;
                                double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGHKCXS")).ToString(), out bghkcxs);
                                double bghkcmj = Math.Round(bgmj * bghkcxs, 2);
                                pFeature.set_Value(pFeatureClass.FindField("BGHKCMJ"), bghkcmj);
                                pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), Math.Round(bgmj - bghkcmj, 2));

                                double kcmji = Math.Round(txmj / mj * sumkcmj, 2);

                                subkcmj -= kcmji;
                                subdlmj -= (bgmj - kcmji);
                                pFeatureCursor.UpdateFeature(pFeature);
                                pFeature = pFeatureCursor.NextFeature();
                            }
                            double tbmj = Math.Round(subkcmj + subdlmj, 2);
                            pFeature.set_Value(pFeatureClass.FindField("TBBGMJ"), tbmj);
                            double kcxs = 0;
                            double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGQKCXS")).ToString(), out kcxs);
                            double kcmj = Math.Round(tbmj * kcxs, 2);
                            pFeature.set_Value(pFeatureClass.FindField("BGQKCMJ"), kcmj);
                            pFeature.set_Value(pFeatureClass.FindField("BGQTBDLMJ"), Math.Round(tbmj - kcmj, 2));

                            double kcxs1 = 0;
                            double.TryParse(pFeature.get_Value(pFeatureClass.FindField("BGHKCXS")).ToString(), out kcxs1);
                            double kcmj1 = Math.Round(tbmj * kcxs1, 2);
                            pFeature.set_Value(pFeatureClass.FindField("BGHKCMJ"), kcmj1);
                            pFeature.set_Value(pFeatureClass.FindField("BGHTBDLMJ"), Math.Round(tbmj - kcmj1, 2));

                            pFeatureCursor.UpdateFeature(pFeature);
                        }
                        pFeatureCursor.Flush();
                    }
                }
            }
            (pTable as IDataset).Delete();
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureClass);
            //MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private IFeatureClass GPUpdate(string inputFea, string updateFea)
        {
            IFeatureClass pFeaClass = null;
            string name = Guid.NewGuid().ToString().Replace("-", "") + ".shp";
            string temp = Application.StartupPath + "\\tmp\\" + name;

            ESRI.ArcGIS.AnalysisTools.Update update = new ESRI.ArcGIS.AnalysisTools.Update();
            update.in_features = inputFea;
            update.update_features = updateFea;
            update.out_feature_class = temp;
            Geoprocessor geoProcessor = new Geoprocessor();
            try
            {
                geoProcessor.Execute(update, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return pFeaClass;
            }
            pFeaClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(temp);
            return pFeaClass;
        }

        private IFeatureClass GPPolygonToLineByFeatureclass(string inputFile, IWorkspace pTmpWS)
        {
            IFeatureClass pFeaClass = null;
            //string name = "sdsdf";
            string name = Guid.NewGuid().ToString().Replace("-", "");
            name = "a" + name.Substring(0, 10);
            //name = "xzqgxjx";
            string temp = pTmpWS.PathName + "\\" + name;

            ESRI.ArcGIS.DataManagementTools.PolygonToLine PolygonToLine = new ESRI.ArcGIS.DataManagementTools.PolygonToLine();
            PolygonToLine.in_features = inputFile;
            PolygonToLine.neighbor_option = "IDENTIFY_NEIGHBORS";
            PolygonToLine.out_feature_class = temp;
            Geoprocessor geoProcessor = new Geoprocessor();
            try
            {
                geoProcessor.Execute(PolygonToLine, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return pFeaClass;
            }
            pFeaClass = (pTmpWS as IFeatureWorkspace).OpenFeatureClass(name);
            return pFeaClass;
        }

        private bool GetIntersectLen(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
            if (pGeoIntersect != null)
            {
                IPolyline pLine = pGeoIntersect as IPolyline;
                if (pLine.Length > 0.001)
                {
                    b = true;
                }
            }
            return b;
        }

        private bool GetIntersectLength(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
            if (pGeoIntersect != null)
            {
                IPolyline pLine = pGeoIntersect as IPolyline;
                if (pLine.Length > 0.0001)
                {
                    b = true;
                }
            }
            return b;
        }

        private bool GetIntersectLength(IFeature pFeature, IGeometry pGeo)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;

            IGeometry pGeoIntersect = pTop.Intersect(pGeo, esriGeometryDimension.esriGeometry1Dimension);
            if (pGeoIntersect != null)
            {
                IPolyline pLine = pGeoIntersect as IPolyline;
                if (pLine.Length > 0.0001)
                {
                    b = true;
                }
            }
            return b;
        }

        private bool GetIntersectArea(IFeature pFeature, IFeature pFea)
        {
            bool b = false;
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            IGeometry pGeoIntersect = pTop.Intersect(pFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            if (pGeoIntersect != null)
            {
                IArea pArea = pGeoIntersect as IArea;
                if (pArea.Area > 0.0001)
                {
                    b = true;
                }
                //RCIS.Utility.OtherHelper.ReleaseComObject(pArea);
            }
            //RCIS.Utility.OtherHelper.ReleaseComObject(pGeoIntersect);
            //RCIS.Utility.OtherHelper.ReleaseComObject(pGeometry);
            //RCIS.Utility.OtherHelper.ReleaseComObject(pTop);
            return b;
        }

        private long EditFieldFromMaxNum(long maxNum, IFeatureClass writeFeatureClass, string writeField, string writeWhere)
        {
            //写入标识码或图斑编号
            int writeFieldNum = writeFeatureClass.FindField(writeField);
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = writeWhere;
            IFeatureCursor featureCursor = writeFeatureClass.Update(queryFilter, true);
            IFeature feature = featureCursor.NextFeature();
            while (feature != null)
            {
                feature.set_Value(writeFieldNum, maxNum++);
                featureCursor.UpdateFeature(feature);
                feature = featureCursor.NextFeature();
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(featureCursor);
            return maxNum;
        }

        private List<string> GetUniqueValuesByFeatureClass(IFeatureClass pFeatureClass, string FieldName)
        {
            List<string> arrValues = new List<string>();
            DataStatisticsClass pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pFeatureClass.Search(null, false) as ICursor;
            pDataStatistics.Field = FieldName;
            IEnumerator pEnum = pDataStatistics.UniqueValues;
            while (pEnum.MoveNext())
            {
                string temp = pEnum.Current.ToString();
                arrValues.Add(temp);
            }
            return arrValues;
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private string GetValueFromMDBByLayerName(string layerName)
        {
            string sql = "select YSDM from SYS_YSDM where CLASSNAME='" + layerName + "'";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            return dt.Rows[0][0].ToString();
        }
        
        #endregion

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


    class TBBGMJ : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            IFeature fea1 = x as IFeature;
            IFeature fea2 = y as IFeature;
            try
            {
                double tbmj1 = FeatureHelper.GetFeatureDoubleValue(fea1, "TBBGMJ");
                double tbmj2 = FeatureHelper.GetFeatureDoubleValue(fea2, "TBBGMJ");
                if (tbmj1 > tbmj2)
                {
                    return -1;
                }
                else if (tbmj1 == tbmj2)
                {
                    return 0;
                }
                else return 1;

            }
            catch (Exception ex)
            {
                return -1;

            }

        }
    }
}
