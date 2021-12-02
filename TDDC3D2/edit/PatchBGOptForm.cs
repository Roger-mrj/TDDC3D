using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.edit
{
    public partial class PatchBGOptForm : Form
    {
        public PatchBGOptForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IWorkspace currWs = null;
        private void PatchBGOptForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap);


            m_datatable = new DataTable();
            DataColumn dc = new DataColumn("OID", typeof(int));
            m_datatable.Columns.Add(dc);
            dc = new DataColumn("ERROR", typeof(string));
            m_datatable.Columns.Add(dc);

        }
    

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {
        
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        ////与 新建图形 相交的地类界线 有哪些
        //private List<IFeature> GetFeatures(IGeometry cutGeo, IFeatureClass aClass)
        //{
        //    List<IFeature> lstdljx = new List<IFeature>();

        //    ISpatialFilter spatialFilter = new SpatialFilterClass();
        //    spatialFilter.Geometry = cutGeo;
        //    spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //    IFeatureClass featureClass = aClass;
        //    IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
        //    IFeature aFea = featureCursor.NextFeature();
        //    try
        //    {
        //        while (aFea != null)
        //        {
        //            lstdljx.Add(aFea);
        //            aFea = featureCursor.NextFeature();
        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
        //    }
        //    return lstdljx;
        //}

        /// <summary>
        /// 获取交与一个面的地类图斑
        /// </summary>
        /// <param name="cutGeo"></param>
        /// <param name="aClass"></param>
        /// <returns></returns>
        private List<IFeature> GetDLTBFeatures(IGeometry cutGeo, IFeatureClass aClass)
        {
            List<IFeature> lstZD = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = cutGeo;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;

            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {
                    IGeometry aGeo = aFea.ShapeCopy;
                    
                    aGeo.Project(cutGeo.SpatialReference);
                    ITopologicalOperator ptop = aGeo as ITopologicalOperator;
                    IGeometry pInterSectGeo = ptop.Intersect(cutGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (pInterSectGeo != null && !pInterSectGeo.IsEmpty)
                    {

                        lstZD.Add(aFea);
                    }
                    aFea = featureCursor.NextFeature();
                }
            }
            catch(Exception ex) { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return lstZD;
        }

        private DataTable m_datatable = null;
        private DateTime gxsj;

        private void ATBBG(IFeature aBgFea)
        {
            IGeometry myGeometry = aBgFea.ShapeCopy;
            try
            {
                string dlbm = FeatureHelper.GetFeatureStringValue(aBgFea, "DLBM");
                string dlmc = FeatureHelper.GetFeatureStringValue(aBgFea, "DLMC");
                Dictionary<string, string> dicFeaValue = new Dictionary<string, string>();
                dicFeaValue.Add("DLBM", dlbm);
                dicFeaValue.Add("DLMC", dlmc);

              
                ////获取图层与该图层相交的部分
                List<IFeature> lstIntersetDltb = GetDLTBFeatures(myGeometry, dltbClass); //交于一个面的地类图斑            
                //新图斑
                List<IFeature> newDltbs = sys.YWCommonHelper.DltbBg(lstIntersetDltb, dltbClass, aBgFea, dicFeaValue);                
               
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
            catch (Exception ex)
            {
                DataRow aErr = m_datatable.NewRow();
                aErr["OID"] = aBgFea.OID;
                aErr["ERROR"] = ex.Message;
                m_datatable.Rows.Add(aErr);
            }
        }
        IFeatureClass bgTbClass = null;
        IFeatureClass dltbClass = null;     
       
        IFeatureClass cjdcqClass = null;

        private void patchBg(IQueryFilter pQF)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始变更", "请稍等...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();



            IEnvelope pEvn = null;
            if (pQF != null)
            {
                pEvn=(pQF as ISpatialFilter).Geometry.Envelope;
            }
            else
            {
                pEvn=(this.bgTbClass as IGeoDataset).Extent;
            }
            
            //所有要素
            List<int> errList = new List<int>();
            if (chkXzqSplit.Checked)
            {
                //先用行政区 将 范围内 临时图斑先分割
                
                sys.YWCommonHelper.SplitDltbByXzq(this.cjdcqClass, this.bgTbClass, wait, 
                   pEvn  , ref errList);
              
            }

           
            
            IFeatureCursor pFeaCursor = bgTbClass.Search(pQF, false);
            IFeature aBgFea = null;
            try
            {
                while ((aBgFea = pFeaCursor.NextFeature()) != null)
                {
                    wait.SetCaption("正在变更" + aBgFea.OID + "...");
                    ATBBG(aBgFea);

                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("patchmultibg");
                wait.Close();
                if (m_datatable.Rows.Count == 0)
                {
                    MessageBox.Show("变更成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.gridControl1.DataSource = m_datatable;
                    MessageBox.Show("变更完成，但存在错误!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
            }
            finally
            {
                OtherHelper.ReleaseComObject(pFeaCursor);
            }
        }

        //private List<int> interDltbOID = new List<int>();//记录当前所有变更图斑的OID

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //首先 得到所有图斑要素

            #region 前置条件
            if (this.cmbLayers.Text.Trim()=="")
                return;
            string ClassName = this.cmbLayers.Text.Trim();
            ClassName = OtherHelper.GetLeftName(ClassName);
            if (ClassName == "DLTB")
            {
                MessageBox.Show("当前这个层不应该选择DLTB吧！");
                return;
            }

            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            
            try
            {
                cjdcqClass = pFeaWs.OpenFeatureClass("CJDCQ");
                dltbClass = pFeaWs.OpenFeatureClass("DLTB");
            }
            catch { }
            if ((cjdcqClass == null)  || (dltbClass==null))
            {
                MessageBox.Show("找不到必备图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion 
            

            bgTbClass = pFeaWs.OpenFeatureClass(ClassName);
            this.gxsj = DateTime.Now;
            this.m_datatable.Rows.Clear();                    
            

            //如果历史,先处理历史
            if (RCIS.Global.AppParameters.GX_HISTORY)
            {
                this.lblstatus.Text = "开始记录历史...";
                Application.DoEvents();
                List<IFeature> lstBgtbs = new List<IFeature>();
                //获取所有与变更图斑相交图斑
                if (this.radioGroup1.SelectedIndex == 2)
                {
                    IFeatureLayer pFeaLyr = LayerHelper.QueryLayerByModelName(this.currMap, ClassName);
                    lstBgtbs = LayerHelper.GetSelectedFeature(this.currMap, pFeaLyr as IGeoFeatureLayer);
                }
                else
                {
                    
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                    if (this.radioGroup1.SelectedIndex == 0)
                    {
                        pSF.Geometry = (bgTbClass as IGeoDataset).Extent;
                    }
                    else if (this.radioGroup1.SelectedIndex == 1)
                    {
                        pSF.Geometry = (this.currMap as IActiveView).Extent;
                    }
                    IFeatureCursor pFeaCursor = bgTbClass.Search(pSF as IQueryFilter, false);
                    IFeature aBgFea = null;
                    while ((aBgFea = pFeaCursor.NextFeature()) != null)
                    {
                        lstBgtbs.Add(aBgFea);
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);

                }
                
                //与这些变更图斑相交的 图斑
                IGeometry interGeo = GeometryHelper.UnionPolygon(lstBgtbs);
                List<IFeature> lstInterDltb = GetDLTBFeatures(interGeo, dltbClass);
                try
                {
                    IFeatureDataset pDS = (currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                    TDDC3D.edit.GxHistoryHelper.CreateHGXTable(pDS);
                    GxHistoryHelper.InsertHistory(currWs, lstInterDltb, this.gxsj);
                }
                catch { }
                this.lblstatus.Text = "";
                Application.DoEvents();
            }


            if (this.radioGroup1.SelectedIndex == 0)
            {
                patchBg(null);
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                //当前范围
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pSF.Geometry = (this.currMap as IActiveView).Extent;
                patchBg(pSF as IQueryFilter);
            }
            else if (this.radioGroup1.SelectedIndex == 2)
            {
                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始变更", "请稍等...");
                wait.Show();                

                IFeatureLayer pFeaLyr=LayerHelper.QueryLayerByModelName(this.currMap,ClassName);
                ArrayList arSelFea= LayerHelper.GetSelectedFeature(this.currMap, pFeaLyr as IGeoFeatureLayer,esriGeometryType.esriGeometryPolygon);
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                try
                {

                    List<int> errId = new List<int>();
                    if (!this.chkXzqSplit.Checked )
                    {
                        //都没切割
                        foreach (IFeature aFea in arSelFea)
                        {
                            wait.SetCaption("正在变更" + aFea.OID + "...");
                            ATBBG(aFea);
                        }
                    }
                    else
                    {
                        List<IFeature> newList = new List<IFeature>();
                        foreach (IFeature aFea in arSelFea)
                        {
                            List<IFeature> splitList = sys.YWCommonHelper.SplitDltbByXzq2(this.cjdcqClass, bgTbClass, wait, aFea, ref errId);
                            newList.AddRange(splitList.ToArray());
                            
                        }
                        foreach (IFeature aFea in newList)
                        {                            
                            wait.SetCaption("正在变更" + aFea.OID + "...");
                            ATBBG(aFea);
                        }
                    }
                    
                                        
                    
                    
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("patchmultibg");
                    wait.Close();
                    if (m_datatable.Rows.Count == 0)
                    {
                        MessageBox.Show("变更成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        this.gridControl1.DataSource = m_datatable;
                        MessageBox.Show("变更完成，但存在错误!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                    if (wait != null)
                        wait.Close();
                }
                
                
            }
            
        }

        private void 导出ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xlsx";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                this.gridControl1.ExportToXlsx(dlg.FileName);
                MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
