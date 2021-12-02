using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Controls;
using System.Collections;

namespace TDDC3D.edit
{
    public partial class TBHBBySxFrm : Form
    {
        public TBHBBySxFrm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        public IWorkspace currWS = null;
        public IMapControl2 mapControl;
        

        private void TBHBBySxFrm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(cmbLayers, currMap, esriGeometryType.esriGeometryPolygon);
            int idx = 0;
            for (int kk = 0; kk < this.cmbLayers.Properties.Items.Count; kk++)
            {
                string dm = OtherHelper.GetLeftName(this.cmbLayers.Properties.Items[kk].ToString());
                if (dm == "DLTB")
                {
                    idx = kk;
                    break;

                }
            }
            this.cmbLayers.SelectedIndex = idx;


            List<string> sAllDlbm = sys.YWCommonHelper.getAllDlbm();

            //加载所有地类
            foreach (string adl in sAllDlbm)
            {
                int kk=this.cmbExceptedDlbms.Properties.Items.Add(adl);
                if (adl.StartsWith("10") || adl.StartsWith("01") || adl == "1101")
                {
                    this.cmbExceptedDlbms.Properties.Items[kk].CheckState = CheckState.Checked;
                }
            }


            this.tableUnionTb = buildTable();
           // this.tableDl101101 = buildTable();
            this.tableError = buildTable();

        }

        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }
        
        private DataTable tableUnionTb = null;
        //private DataTable tableDl101101 = null;
        private DataTable tableError = null;

        private DataTable buildTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("BSM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("ZLDWDM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("QSDWDM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("DLBM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("QSXZ", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("ZZSXDM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("TBXHDM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("TBMJ", typeof(double));
            dt.Columns.Add(dc);
            dc = new DataColumn("OBJECTID", typeof(long));
            dt.Columns.Add(dc);
          
            return dt;
        }

        


        private void getHbFeas(IFeatureClass pFC, IFeature inFea)
        {
            string dlbm = FeatureHelper.GetFeatureStringValue(inFea, "DLBM");
            
            IGeometry geo = inFea.Shape;
            ITopologicalOperator pTop = geo as ITopologicalOperator;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = geo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            


            string qsdwdm = FeatureHelper.GetFeatureStringValue(inFea, "QSDWDM");
            string zldwdm = FeatureHelper.GetFeatureStringValue(inFea, "ZLDWDM");
            string qsxz = FeatureHelper.GetFeatureStringValue(inFea, "QSXZ");
            string tbxhdm = FeatureHelper.GetFeatureStringValue(inFea, "TBXHDM");
            string zzsxdm = FeatureHelper.GetFeatureStringValue(inFea, "ZZSXDM");
            string czcsxm = FeatureHelper.GetFeatureStringValue(inFea, "CZCSXM");

            pSF.WhereClause = "DLBM='" + dlbm + "' and QSDWDM='" + qsdwdm + "' and ZLDWDM='" + zldwdm + "' and CZCSXM='"+czcsxm+"' ";
            IFeatureCursor pCursor = pFC.Search(pSF as IQueryFilter, true);
            IFeature aDltb = null;
            bool isUnion = false;
          
            try
            {
                while ((aDltb = pCursor.NextFeature()) != null)
                {
                    if (inFea.OID == aDltb.OID)
                        continue;
                    #region 查找
                    string tbxhdm2 = FeatureHelper.GetFeatureStringValue(aDltb, "TBXHDM");
                    if (tbxhdm != tbxhdm2)
                        continue;
                    string zzsxdm2 = FeatureHelper.GetFeatureStringValue(aDltb, "ZZSXDM");
                    if (zzsxdm != zzsxdm2)
                        continue;

                    IGeometry tmpGeo = pTop.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry1Dimension);
                    if (!tmpGeo.IsEmpty)
                    {
                        //交于一个线
                        isUnion = true;
                        break;
                    }
                    else
                    {
                        tmpGeo = pTop.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        if (!tmpGeo.IsEmpty)
                        {
                            isUnion = true;
                            break;
                        }

                    }
                    #endregion

                }
                if (isUnion)
                {
                    //找到可以与他合并的
                    DataRow aRow = this.tableUnionTb.NewRow();
                    //如果不为空
                    aRow["BSM"] = FeatureHelper.GetFeatureStringValue(inFea, "BSM").ToString();
                    aRow["QSDWDM"] = FeatureHelper.GetFeatureStringValue(inFea, "QSDWDM").ToString();
                    aRow["ZLDWDM"] = FeatureHelper.GetFeatureStringValue(inFea, "ZLDWDM").ToString();
                    aRow["DLBM"] = FeatureHelper.GetFeatureStringValue(inFea, "DLBM").ToString();
                    aRow["TBXHDM"] = FeatureHelper.GetFeatureStringValue(inFea, "TBXHDM").ToString();
                    aRow["ZZSXDM"] = FeatureHelper.GetFeatureStringValue(inFea, "ZZSXDM").ToString();
                    aRow["TBMJ"] = (inFea.Shape as IArea).Area;
                    aRow["OBJECTID"] = inFea.OID;
                    this.tableUnionTb.Rows.Add(aRow);

                    //记录这个ID，
                    //alreadyExistId.Add(inFea.OID);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        private void findHB(IFeatureClass dltbClass)
        {
            //获取排除的地类
            List<string> exceptDLBM = new List<string>();
            for (int k = 0; k < this.cmbExceptedDlbms.Properties.Items.Count; k++)
            {
                if (this.cmbExceptedDlbms.Properties.Items[k].CheckState == CheckState.Checked)
                {
                    exceptDLBM.Add(OtherHelper.GetLeftName(this.cmbExceptedDlbms.Properties.Items[k].ToString()));
                }
            }
            IGeometry geo = (this.currMap as IActiveView).Extent;
            ISpatialFilter pSF=new SpatialFilterClass();
            pSF.Geometry=geo;
            pSF.SpatialRel=esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = dltbClass.Search(pSF as IQueryFilter, true);
            IFeature aFeature = null;
            try
            {
                while ((aFeature = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                    if (exceptDLBM.Contains(dlbm))
                    {
                        continue;
                    }
                    getHbFeas(dltbClass, aFeature);


                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                OtherHelper.ReleaseComObject(pSF);
                OtherHelper.ReleaseComObject(pCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //查找
            if (this.cmbLayers.Text.Trim() == "")
                return;
            string className = RCIS.Utility.OtherHelper.GetLeftName(this.cmbLayers.Text.Trim());
            dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (dltbLayer == null)
                return;
            
           
            this.tableUnionTb.Rows.Clear();
            this.Cursor = Cursors.WaitCursor;
            IFeatureClass dltbClass = dltbLayer.FeatureClass;
            if (dltbClass.FeatureCount(null) == 0) return;

            


            //查找 可以合并的图斑
            findHB(dltbClass);
            this.Cursor = Cursors.Default;
            this.gridControlSmall.DataSource = this.tableUnionTb;
            this.gridControlSmall.Refresh();
            
            if (this.tableUnionTb.Rows.Count == 0)
            {
                MessageBox.Show("没有找到需要合并的要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        ArrayList getUnioning(IFeatureClass dltbClass,IFeature inFea)
        {
            ArrayList ar=new ArrayList();
            IGeometry geo = inFea.Shape;
            ITopologicalOperator pTop = geo as ITopologicalOperator;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = geo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            string dlbm = FeatureHelper.GetFeatureStringValue(inFea, "DLBM");
            string qsdwdm = FeatureHelper.GetFeatureStringValue(inFea, "QSDWDM");
            string zldwdm = FeatureHelper.GetFeatureStringValue(inFea, "ZLDWDM");
            string qsxz = FeatureHelper.GetFeatureStringValue(inFea, "QSXZ");
            string tbxhdm = FeatureHelper.GetFeatureStringValue(inFea, "TBXHDM");
            string zzsxdm = FeatureHelper.GetFeatureStringValue(inFea, "ZZSXDM");

            pSF.WhereClause = "DLBM='" + dlbm + "' and QSDWDM='" + qsdwdm + "' and ZLDWDM='" + zldwdm + "' ";
            IFeatureCursor pCursor = dltbClass.Search(pSF as IQueryFilter, false);
            IFeature aDltb = null;
            
            try
            {
                while ((aDltb = pCursor.NextFeature()) != null)
                {
                    if (inFea.OID == aDltb.OID)
                        continue;
                    #region 查找
                    string tbxhdm2 = FeatureHelper.GetFeatureStringValue(aDltb, "TBXHDM");
                    if (tbxhdm != tbxhdm2)
                        continue;
                    string zzsxdm2 = FeatureHelper.GetFeatureStringValue(aDltb, "ZZSXDM");
                    if (zzsxdm != zzsxdm2)
                        continue;

                    IGeometry tmpGeo = pTop.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry1Dimension);
                    if (!tmpGeo.IsEmpty)
                    {                        //交于一个线
                        ar.Add(aDltb);
                    }
                    else
                    {
                        tmpGeo = pTop.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry2Dimension);
                        if (!tmpGeo.IsEmpty)
                        {
                            ar.Add(aDltb);
                        }

                    }
                    #endregion

                }
               
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            return ar;               

        }

        //合并
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            if (dltbLayer == null)
                return;
            if (this.tableUnionTb.Rows.Count == 0) return;
            //this.tableDl101101.Rows.Clear();
            this.tableError.Rows.Clear();

            List<string> exceptDLBM=new List<string>();
            for (int k = 0; k < this.cmbExceptedDlbms.Properties.Items.Count; k++)
            {
                if (this.cmbExceptedDlbms.Properties.Items[k].CheckState == CheckState.Checked)
                {
                    exceptDLBM.Add(OtherHelper.GetLeftName(this.cmbExceptedDlbms.Properties.Items[k].ToString()));
                }
            }


            IFeatureClass dltbClass = dltbLayer.FeatureClass;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                for (int ii = this.tableUnionTb.Rows.Count - 1; ii >= 0; ii--)
                {
                    DataRow aRow = this.tableUnionTb.Rows[ii];
                    string dlbm = aRow["DLBM"].ToString();
                    //if ( exceptDLBM.Contains(dlbm))
                    //{
                    //    this.tableDl101101.Rows.Add(aRow.ItemArray);
                    //    this.tableUnionTb.Rows.RemoveAt(ii);
                    //    continue;
                    //}
                    int oid = 0;
                    string soid = aRow["OBJECTID"].ToString();
                    int.TryParse(soid, out oid);
                    IFeature aFeature = null;
                    try
                    {
                        aFeature = dltbClass.GetFeature(oid);
                    }
                    catch (Exception ex)
                    {
                    }

                    if (aFeature == null)
                    {
                        this.tableUnionTb.Rows.RemoveAt(ii);
                        continue;  //找不到，可能已经被合并完了
                    }
                    try
                    {
                        ArrayList arFeas = this.getUnioning(dltbClass, aFeature);
                        if (arFeas.Count > 0)
                        {
                            arFeas.Add(aFeature);
                            IGeometry geo = GeometryHelper.UnionPolygon(arFeas);
                            //创建要素
                            IFeature firstFea = arFeas[0] as IFeature;
                            firstFea.Shape = geo;
                            firstFea.Store();
                            //删除其他
                             for (int kk = 1; kk < arFeas.Count; kk++)
                                {
                                    IFeature delFea = arFeas[kk] as IFeature;
                                    delFea.Delete();
                                }
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        //失败，需要单独处理
                        this.tableError.Rows.Add(aRow.ItemArray);
                        continue;
                    }
                    this.tableUnionTb.Rows.RemoveAt(ii);

                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("hbtb");
                this.gridControlSmall.RefreshDataSource();
                this.gridControlError.DataSource = this.tableError;
                //this.gridControl101101.DataSource = this.tableDl101101;

                MessageBox.Show("合并完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }

        }

        private IFeatureLayer dltbLayer = null;

        private void gridControlSmall_Click(object sender, EventArgs e)
        {
            
        }

        private void gridControlSmall_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewUnion.SelectedRowsCount == 0)
                return;
            if (dltbLayer == null) return;
            string oid = this.gridViewUnion.GetRowCellValue(this.gridViewUnion.FocusedRowHandle, "OBJECTID").ToString();

            //定位
            try
            {
                Int32 ioid = Convert.ToInt32(oid);
                IFeatureSelection pSelection = dltbLayer as IFeatureSelection;
                IFeatureClass pClass = dltbLayer.FeatureClass;
                if (pClass == null) return;
                if (ioid < 0) return;
                IFeature pFeature = pClass.GetFeature(ioid);
                if (pFeature != null)
                {
                    IGeometry pGeo = pFeature.ShapeCopy;
                    IEnvelope env = pGeo.Envelope;
                    env.Expand(1.5, 1.5, true);
                    this.mapControl.ActiveView.Extent = env;


                    this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                    this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    if (pGeo != null)
                    {
                        this.mapControl.FlashShape(pGeo, 3, 300, null);
                    }

                }
            }
            catch (Exception cex)
            {
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (this.dltbLayer == null) return;
            IFeatureSelection pSelection = this.dltbLayer as IFeatureSelection;
            pSelection.Clear();
            IFeatureClass pFC = this.dltbLayer.FeatureClass;
            string fldName = "OBJECTID";

            int oid = 0;
            switch (this.xtraTabControl1.SelectedTabPageIndex)
            {
                case 0:
                    for (int i = 0; i < this.gridViewUnion.SelectedRowsCount; i++)
                    {
                        int idx = this.gridViewUnion.GetSelectedRows()[i];
                        try
                        {
                            string sid = this.gridViewUnion.GetRowCellValue(idx, fldName).ToString();
                            oid = Convert.ToInt32(sid);
                            IFeature selFea = pFC.GetFeature(oid);
                            if (selFea != null)
                            {
                                IGeometry geom = selFea.Shape;
                                if (geom != null && !geom.IsEmpty)
                                {
                                    pSelection.Add(selFea);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < this.gridViewError.SelectedRowsCount; i++)
                    {
                        int idx = this.gridViewError.GetSelectedRows()[i];
                        try
                        {
                            string sid = this.gridViewError.GetRowCellValue(idx, fldName).ToString();
                            oid = Convert.ToInt32(sid);
                            IFeature selFea = pFC.GetFeature(oid);
                            if (selFea != null)
                            {
                                IGeometry geom = selFea.Shape;
                                if (geom != null && !geom.IsEmpty)
                                {
                                    pSelection.Add(selFea);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    break;
                case 2:
                    //for (int i = 0; i < this.gridView101101.SelectedRowsCount; i++)
                    //{
                    //    int idx = this.gridView101101.GetSelectedRows()[i];
                    //    try
                    //    {
                    //        string sid = this.gridView101101.GetRowCellValue(idx, fldName).ToString();
                    //        oid = Convert.ToInt32(sid);
                    //        IFeature selFea = pFC.GetFeature(oid);
                    //        if (selFea != null)
                    //        {
                    //            IGeometry geom = selFea.Shape;
                    //            if (geom != null && !geom.IsEmpty)
                    //            {
                    //                pSelection.Add(selFea);
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //}

                    break;
            }
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                       

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (this.dltbLayer == null) return;
            if (this.gridViewUnion.SelectedRowsCount == 0)
                return;


            IFeatureSelection pSelection = this.dltbLayer as IFeatureSelection;
            pSelection.Clear();
            IFeatureClass pFC = this.dltbLayer.FeatureClass;
            string fldName = "OBJECTID";

            int oid = 0;
            switch (this.xtraTabControl1.SelectedTabPageIndex)
            {
                case 0:
                    try
                    {
                        string sBSM = this.gridViewUnion.GetRowCellValue(gridViewUnion.FocusedRowHandle, fldName).ToString();
                        oid = Convert.ToInt32(sBSM);
                        IFeature selFea = pFC.GetFeature(oid);
                        if (selFea != null)
                        {
                            IGeometry geom = selFea.Shape;
                            if (geom != null && !geom.IsEmpty)
                            {
                                IEnvelope env = geom.Envelope;
                                env.Expand(1.5, 1.5, true);
                                this.mapControl.ActiveView.Extent = env;

                                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                                this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                                this.mapControl.FlashShape(geom, 3, 300, null);


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    break;
                //case 1:
                //    try
                //    {
                //        string sBSM = this.gridViewUnFinished.GetRowCellValue(gridViewUnFinished.FocusedRowHandle, fldName).ToString();
                //        oid = Convert.ToInt32(sBSM);
                //        IFeature selFea = pFC.GetFeature(oid);
                //        if (selFea != null)
                //        {
                //            IGeometry geom = selFea.Shape;
                //            if (geom != null && !geom.IsEmpty)
                //            {
                //                IEnvelope env = geom.Envelope;
                //                env.Expand(1.5, 1.5, true);
                //                this.mapControl.ActiveView.Extent = env;

                //                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                //                this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                //                this.mapControl.FlashShape(geom, 3, 300, null);


                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //    }

                //    break;
                //case 2:
                //    try
                //    {
                //        string sBSM = this.gridView01101.GetRowCellValue(gridView01101.FocusedRowHandle, fldName).ToString();
                //        oid = Convert.ToInt32(sBSM);
                //        IFeature selFea = pFC.GetFeature(oid);
                //        if (selFea != null)
                //        {
                //            IGeometry geom = selFea.Shape;
                //            if (geom != null && !geom.IsEmpty)
                //            {
                //                IEnvelope env = geom.Envelope;
                //                env.Expand(1.5, 1.5, true);
                //                this.mapControl.ActiveView.Extent = env;

                //                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                //                this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                //                this.mapControl.FlashShape(geom, 3, 300, null);


                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //    break;
            }

          
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (this.dltbLayer != null)
            {
                IFeatureSelection pSelection = this.dltbLayer as IFeatureSelection;
                pSelection.Clear();

            }
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
        }

        private void gridControlError_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewError.SelectedRowsCount == 0)
                return;
            if (dltbLayer == null) return;
            string oid = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "OBJECTID").ToString();

            //定位
            try
            {
                Int32 ioid = Convert.ToInt32(oid);
                IFeatureSelection pSelection = dltbLayer as IFeatureSelection;
                IFeatureClass pClass = dltbLayer.FeatureClass;
                if (pClass == null) return;
                if (ioid < 0) return;
                IFeature pFeature = pClass.GetFeature(ioid);
                if (pFeature != null)
                {
                    IGeometry pGeo = pFeature.ShapeCopy;
                    IEnvelope env = pGeo.Envelope;
                    env.Expand(1.5, 1.5, true);
                    this.mapControl.ActiveView.Extent = env;


                    this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                    this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    if (pGeo != null)
                    {
                        this.mapControl.FlashShape(pGeo, 3, 300, null);
                    }

                }
            }
            catch (Exception cex)
            {
            }
        }

        private void gridControl101101_DoubleClick(object sender, EventArgs e)
        {
            //if (this.gridView101101.SelectedRowsCount == 0)
            //    return;
            //if (dltbLayer == null) return;
            //string oid = this.gridView101101.GetRowCellValue(this.gridView101101.FocusedRowHandle, "OBJECTID").ToString();

            ////定位
            //try
            //{
            //    Int32 ioid = Convert.ToInt32(oid);
            //    IFeatureSelection pSelection = dltbLayer as IFeatureSelection;
            //    IFeatureClass pClass = dltbLayer.FeatureClass;
            //    if (pClass == null) return;
            //    if (ioid < 0) return;
            //    IFeature pFeature = pClass.GetFeature(ioid);
            //    if (pFeature != null)
            //    {
            //        IGeometry pGeo = pFeature.ShapeCopy;
            //        IEnvelope env = pGeo.Envelope;
            //        env.Expand(1.5, 1.5, true);
            //        this.mapControl.ActiveView.Extent = env;


            //        this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
            //        this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
            //        if (pGeo != null)
            //        {
            //            this.mapControl.FlashShape(pGeo, 3, 300, null);
            //        }

            //    }
            //}
            //catch (Exception cex)
            //{
            //}
        }
    }
}
