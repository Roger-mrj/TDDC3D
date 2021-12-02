using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using TDDC3D.sys;
using ESRI.ArcGIS.Controls;

namespace TDDC3D.edit
{
    public partial class SmallDltbMergeForm : Form
    {
        public SmallDltbMergeForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IWorkspace currWs = null;
        public IMapControl2 mapControl = null;

        IFeatureLayer dltbLayer = null;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SmallDltbMergeForm_Load(object sender, EventArgs e)
        {
         
            LayerHelper.LoadLayer2Combox(this.cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);

            int idx1 = -1;
            for (int i = 0; i < this.cmbLayer.Properties.Items.Count; i++)
            {
                string name = this.cmbLayer.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLTB"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbLayer.SelectedIndex = idx1;

            this.tableNotUniton = this.buildTable();
            this.tableSmallTb = this.buildTable();
            this.tableDl101101 = this.buildTable();

        }

        public decimal WlydStmj
        {
            get
            {
               return   this.spinEdit3.Value;
            }
        }

        public decimal NydStmj
        {
            get {
                return this.spinEdit2.Value;
            }
        }
        public decimal JsydStmj
        {
            get {
                return this.spinEdit1.Value;
            }
        }


        private DataTable tableSmallTb = null;
        private DataTable tableNotUniton = null;

        private DataTable tableDl101101 = null;

        private DataTable  buildTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("BSM", typeof(string));
            dt.Columns.Add(dc);

            dc = new DataColumn("TBYBH", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("TBBH", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("DLBM", typeof(string));
            dt.Columns.Add(dc);
            dc = new DataColumn("TBMJ", typeof(double));
            dt.Columns.Add(dc);
            dc = new DataColumn("QSXZ", typeof(string));
            dt.Columns.Add(dc);
            dc=new DataColumn("OBJECTID",typeof(long));
            dt.Columns.Add(dc);

            return dt;
        }


        private List<IFeature> getSmallTb()
        {          
           
            IFeatureClass pFC = this.dltbLayer.FeatureClass;
            string shpFld = pFC.ShapeFieldName + "_AREA";
            //找到所有 符合条件的面
            List<IFeature> allFeatures = new List<IFeature>();
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = this.mapControl.Extent as IGeometry;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.SubFields = shpFld;

            double maxmj = 0;
            if (maxmj < (double)this.NydStmj)
            {
                maxmj =(double) this.NydStmj;
            }
            if (maxmj<(double)this.JsydStmj)
            {
                maxmj=(double)this.JsydStmj;
            }
            if (maxmj<(double)this.WlydStmj)
            {
                maxmj=(double)this.WlydStmj;
            }

            pSF.WhereClause = shpFld + "  < " + maxmj;
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = pFC.Search(pSF, false);
                comrelease.ManageLifetime(cursor);

                IFeature aFea = null;
                while ((aFea = cursor.NextFeature()) != null)
                {
                    double mj=(aFea.Shape as IArea).Area;
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                    if (sys.YWCommonHelper.IsNydExp1202(dlbm) &&  (mj <(double)this.NydStmj) )
                    {
                        allFeatures.Add(aFea);
                    }
                    else if (sys.YWCommonHelper.isJsydAnd1202(dlbm) && (mj < (double)this.JsydStmj))
                    {
                        allFeatures.Add(aFea);
                    }
                    else if (sys.YWCommonHelper.isWlyd(dlbm) && (mj < (double)this.WlydStmj))
                    {
                        allFeatures.Add(aFea);
                    }
                    else if (mj < (double)this.JsydStmj)
                    {
                        allFeatures.Add(aFea);
                    }
                    
                    
                }
            }
            return allFeatures;
        }

        private IFeature getWithinXzq(IFeature inTb)
        {
            IFeatureClass xzqClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = inTb.ShapeCopy;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
            IFeatureCursor pCusor = xzqClass.Search(pSF, false);
            IFeature aXzq = pCusor.NextFeature();
            OtherHelper.ReleaseComObject(pCusor);
            return aXzq;
        }

        private IFeature getWithinTDQSQ(IFeature inTb)
        {
            IFeatureClass xzqClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = inTb.ShapeCopy;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin;
            IFeatureCursor pCusor = xzqClass.Search(pSF, false);
            IFeature aZD = pCusor.NextFeature();
            OtherHelper.ReleaseComObject(pCusor);
            return aZD;
        }

        private bool PolygonEqual(IGeometry geo1, IGeometry geo2)
        {
            IRelationalOperator pRO = geo1 as IRelationalOperator;
            IRelationalOperator pRO2 = geo2 as IRelationalOperator;
            if (pRO.Contains(geo2) && pRO2.Contains(geo1))
            {
                return true;
            }
            else
                return false;
        }
        
        ///// <summary>
        ///// 找最大图斑，权属单位代码，坐落单位代码，权属性质都相同才行
        ///// </summary>
        ///// <param name="inTb"></param>
        ///// <returns></returns>
        //private IFeature getMaxFeature(IFeature inTb)
        //{

        //    IFeature currFea = null;
        //    double mj = (inTb.ShapeCopy as IArea).Area;
        //    double maxMj = 0;
        //    string oldZldwdm = FeatureHelper.GetFeatureStringValue(inTb, "ZLDWDM").ToUpper();
        //    string  oldQsdm = FeatureHelper.GetFeatureStringValue(inTb, "QSDWDM").ToUpper();

        //    IFeature oldZD = getWithinTDQSQ(inTb);

        //    if (oldZldwdm.Trim() == "")
        //    {
        //        IFeature oldXzq = getWithinXzq(inTb);
        //        if (oldXzq != null)
        //        {
        //            oldZldwdm = FeatureHelper.GetFeatureStringValue(oldXzq, "XZQDM").Trim();
        //        }
        //    }
        //    if (oldZldwdm == "")
        //        return null;
            
        //    IFeatureClass pFC = this.dltbLayer.FeatureClass;
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    string shpFld = pFC.ShapeFieldName + "_AREA";

        //    pSF.Geometry = inTb.ShapeCopy as IGeometry;
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
        //    pSF.SubFields = shpFld;
        //    pSF.WhereClause =shpFld + " >=" + mj;
        //    using (ComReleaser comrelease = new ComReleaser())
        //    {
        //        IFeatureCursor cursor = pFC.Search(pSF, false);
        //        comrelease.ManageLifetime(cursor);

        //        IFeature aFea = null;
        //        while ((aFea = cursor.NextFeature()) != null)
        //        {
        //            string  newZldwdm = FeatureHelper.GetFeatureStringValue(aFea, "ZLDWDM").ToUpper();
        //            if (newZldwdm.Trim() == "")
        //            {
        //                //找改图斑所在的佐罗单位
        //                IFeature newFeainXzq = getWithinXzq(aFea);
        //                if (newFeainXzq != null)
        //                {
        //                    newZldwdm = FeatureHelper.GetFeatureStringValue(newFeainXzq, "XZQDM").Trim();
        //                }
        //            }
        //            if (newZldwdm == "")
        //            {
        //                aFea = cursor.NextFeature();
        //                continue;
        //            }
        //            string newQsdwdm = FeatureHelper.GetFeatureStringValue(aFea, "QSDWDM").ToUpper();

        //            double newMj = (aFea.ShapeCopy as IArea).Area;
        //            //坐落单位代码和权属单位代码不一致的略过
        //            if (newZldwdm != oldZldwdm)
        //            {
        //                continue;
        //            }
        //            if (oldQsdm != newQsdwdm)
        //            {
        //                continue;
        //            }
        //            IFeature newZD = getWithinTDQSQ(aFea);
        //            if (!this.PolygonEqual(newZD.Shape, oldZD.Shape))
        //            {
        //                //如果所在宗地图形不是一个位置
        //                continue;
        //            }
                  
        //            if (newMj > maxMj)
        //            {
        //                maxMj = newMj;
        //                currFea = aFea;
        //            }

        //        }
        //    }
        //    return currFea;
        //}


        private List<IFeature> allSmallFea = new List<IFeature>();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.dltbLayer == null)
                return;
            if (this.tableSmallTb.Rows.Count == 0)
            {
                MessageBox.Show("没找到破碎图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            IFeatureClass dltbClass = dltbLayer.FeatureClass;
           
            //IFeatureLayer zdLayer = LayerHelper.QueryLayerByModelName(this.currMap, "TDQSQ");
            //if (zdLayer == null)
            //{
            //    MessageBox.Show("找不到土地权属区图层！");
            //    return;
            //}

            this.Cursor = Cursors.WaitCursor;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等...", "正在处理...");
            wait.Show();

            tableNotUniton.Rows.Clear();
            tableDl101101.Rows.Clear();

            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            
            try
            {
                
              //  List<int> lstExcepted = new List<int>();
                int allCount = allSmallFea.Count;
                int inum = 0;
                foreach (IFeature aSmall in allSmallFea)
                {
                    //重新判断一下面积，因为相邻碎图斑处理后有可能发生变化了
                    double currmj=(aSmall.Shape as IArea).Area;
                    string dlbm = FeatureHelper.GetFeatureStringValue(aSmall, "DLBM");
                    if (this.chk101101.Checked)
                    {
                        //地类10 1101不合并
                        if (dlbm.StartsWith("10") || dlbm == "1101")
                        {
                            //加入到sheet3中
                            DataRow aRow =this.tableDl101101.NewRow();
                            aRow["BSM"] = FeatureHelper.GetFeatureStringValue(aSmall, "BSM");
                            aRow["TBYBH"] = FeatureHelper.GetFeatureStringValue(aSmall, "TBYBH");
                            aRow["TBBH"] = FeatureHelper.GetFeatureStringValue(aSmall, "TBBH");
                            aRow["DLBM"] = FeatureHelper.GetFeatureStringValue(aSmall, "DLBM");
                            aRow["TBMJ"] = (aSmall.Shape as IArea).Area;  // FeatureHelper.GetFeatureDoubleValue(aSmall, "TBMJ");
                            aRow["QSXZ"] = FeatureHelper.GetFeatureStringValue(aSmall, "QSXZ");
                            aRow["OBJECTID"] = aSmall.OID;
                            tableDl101101.Rows.Add(aRow);
                            continue;
                        }
                    }

                    if (YWCommonHelper.IsNydExp1202(dlbm) && currmj >(double) this.NydStmj)
                        continue;
                    if (YWCommonHelper.isJsydAnd1202(dlbm) && currmj > (double)this.JsydStmj)
                        continue;
                    if (YWCommonHelper.isWlyd(dlbm) && currmj >(double) this.WlydStmj)
                        continue;
                   
                    //找到与之相邻的 图斑
                    //如果该图斑 座落单位代码 和 相邻不同，则掠过
                    IFeature maxFea = SmallTbDoHelper.getMaxFeatureSameDL2(aSmall, dltbClass,this.chk101101.Checked);

                    if (maxFea == null)
                        maxFea = SmallTbDoHelper.getMaxFeatureSameDL1(aSmall, dltbClass, this.chk101101.Checked); //相同一级类最大的图斑
                    if (maxFea == null)
                        maxFea = SmallTbDoHelper.getMaxFeatureSame3DL(aSmall, dltbClass, this.chk101101.Checked);


                    if (maxFea == null)
                    {
                        //表示没找到 这个 可以合并的图斑

                        DataRow aRow = tableNotUniton.NewRow();
                        aRow["BSM"] = FeatureHelper.GetFeatureStringValue(aSmall, "BSM");
                        aRow["TBYBH"] = FeatureHelper.GetFeatureStringValue(aSmall, "TBYBH");
                        aRow["TBBH"] = FeatureHelper.GetFeatureStringValue(aSmall, "TBBH");
                        aRow["DLBM"] = FeatureHelper.GetFeatureStringValue(aSmall, "DLBM");
                        aRow["TBMJ"] = (aSmall.Shape as IArea).Area;   // FeatureHelper.GetFeatureDoubleValue(aSmall, "TBMJ");
                        aRow["QSXZ"] = FeatureHelper.GetFeatureStringValue(aSmall, "QSXZ");
                        aRow["OBJECTID"] = aSmall.OID;
                        tableNotUniton.Rows.Add(aRow);

                        continue;
                    }

                    
                    if (maxFea != null)
                    {


                        IGeometry maxGeo = maxFea.ShapeCopy as IGeometry;
                        ITopologicalOperator pTop = maxGeo as ITopologicalOperator;
                        IGeometry newGeo = pTop.Union(aSmall.ShapeCopy);
                        pTop.Simplify();
                        newGeo.SnapToSpatialReference();
                        maxFea.Shape = newGeo;

                        IPoint selectPoint = (maxFea.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        int currDh = (int)(X / 1000000);////WK---带号
                        //LSSphereArea.GGP.ClsSphereArea area = new LSSphereArea.GGP.ClsSphereArea();
                        SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                        double tbmj = area.SphereArea(maxFea.ShapeCopy, currDh);
                        FeatureHelper.SetFeatureValue(maxFea, "TBMJ", tbmj);
                        maxFea.Store();

                        inum++;

                        aSmall.Delete();
                    }

                    wait.SetCaption("已处理" + inum + "个...");

                }
                //foreach (IFeature aSmall in allSmallFea)
                //{
                //    aSmall.Delete();
                //}
                this.Cursor = Cursors.Default;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("smalldltbunion");


                this.gridControlUnFinish.DataSource=this.tableNotUniton;
                this.xtraTabControl1.SelectedTabPageIndex = 1;

                this.gridControlDL101101.DataSource = this.tableDl101101;


                wait.Close();
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent);

                MessageBox.Show("共找到破碎图斑" + allCount + "个。共处理" + inum + "个。");
            }
            catch (Exception ex)
            {
                wait.Close();

                this.Cursor = Cursors.Default;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }

            
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void labelControl4_Click(object sender, EventArgs e)
        {

        }

        private void spinEdit3_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "")
                return;
            string className = RCIS.Utility.OtherHelper.GetLeftName(this.cmbLayer.Text.Trim());
            dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (this.dltbLayer == null)
                return;

            IFeatureLayer zdLayer = LayerHelper.QueryLayerByModelName(this.currMap, "CJDCQ");
            if (zdLayer == null)
            {
                MessageBox.Show("找不到村级调查区图层！");
                return;
            }



            //查找图斑
            allSmallFea = getSmallTb();
            if (allSmallFea.Count == 0)
            {
                MessageBox.Show("当前范围没有满足条件的图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }
            this.tableSmallTb.Rows.Clear();

            foreach (IFeature aFea in allSmallFea)
            {
                DataRow aRow = this.tableSmallTb.NewRow();
                aRow["BSM"] = FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                aRow["TBYBH"] = FeatureHelper.GetFeatureStringValue(aFea, "TBYBH");
                aRow["TBBH"] = FeatureHelper.GetFeatureStringValue(aFea, "TBBH");
                aRow["DLBM"] = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                aRow["TBMJ"] = (aFea.Shape as IArea).Area;  // FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ");
                aRow["QSXZ"] = FeatureHelper.GetFeatureStringValue(aFea, "QSXZ");
                aRow["OBJECTID"] = aFea.OID;
                this.tableSmallTb.Rows.Add(aRow);

            }
            this.gridControlSmall.DataSource = this.tableSmallTb;


        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewSmalltb.SelectedRowsCount == 0)
                return;
            if (this.dltbLayer == null) return;
            string oid = this.gridViewSmalltb.GetRowCellValue(this.gridViewSmalltb.FocusedRowHandle, "OBJECTID").ToString();
            Int32 ioid = Convert.ToInt32(oid);
            //定位
            try
            {
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

        private void gridControl2_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 获取要素，svalue是整型的
        /// </summary>
        /// <param name="fldName"></param>
        /// <param name="sValue"></param>
        /// <param name="pFC"></param>
        /// <returns></returns>
        private IFeature GetFeature(string fldName, string sValue, IFeatureClass pFC)
        {
            IQueryFilter qf = new QueryFilterClass();
            qf.WhereClause = fldName + " =" + sValue + "";
            IFeatureCursor pCursor = pFC.Search(qf, false);
            IFeature pFea = pCursor.NextFeature();

            if (pFea != null)
            {
                OtherHelper.ReleaseComObject(pCursor);
                return pFea;
            }
            OtherHelper.ReleaseComObject(qf);
            OtherHelper.ReleaseComObject(pCursor);
            return null;
        }

    

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //将未完成合并到周边的大图斑
            if (this.tableNotUniton.Rows.Count == 0)
            {
                MessageBox.Show("没有未完成合并的图斑！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass dltbClass=this.dltbLayer.FeatureClass;
            this.Cursor = Cursors.WaitCursor;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等...", "正在处理...");
            wait.Show();

            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();

            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                int inum = 0;
                foreach (DataRow aRow in this.tableNotUniton.Rows)
                {
                    string oid = aRow["OBJECTID"].ToString();
                    IFeature smallFeature = this.GetFeature("OBJECTID", oid, dltbClass);
                    //if (FeatureHelper.GetFeatureStringValue(smallFeature, "DLBM") == "1006")
                    //    continue;
                    if (smallFeature == null)
                        continue;

                    IFeature maxFea = SmallTbDoHelper.getMaxFeature(smallFeature,dltbClass,this.chk101101.Checked);

                    if (maxFea != null)
                    {
                        IGeometry maxGeo = maxFea.ShapeCopy as IGeometry;
                        ITopologicalOperator pTop = maxGeo as ITopologicalOperator;
                        IGeometry newGeo = pTop.Union(smallFeature.ShapeCopy);
                        pTop.Simplify();
                        newGeo.SnapToSpatialReference();
                        maxFea.Shape = newGeo;                       
                        
                        double tbmj = area.SphereArea(maxFea.ShapeCopy);
                        FeatureHelper.SetFeatureValue(maxFea, "TBMJ", tbmj);
                        maxFea.Store();

                        inum++;
                        smallFeature.Delete();
                    }

                    wait.SetCaption("已处理" + inum + "个...");

                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("finishSmalltb");
                wait.Close();
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent);
                MessageBox.Show("共"+this.tableNotUniton.Rows.Count+"个图斑，共处理完成"+inum+"个图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                if (wait==null)
                    wait.Close();
                this.Cursor = Cursors.Default;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //将未完成合并到周边的大图斑
            if (this.tableNotUniton.Rows.Count == 0)
            {
                MessageBox.Show("没有未完成合并的图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass dltbClass = this.dltbLayer.FeatureClass;
            this.Cursor = Cursors.WaitCursor;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等...", "正在处理...");
            wait.Show();

            SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();

            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                int inum = 0;
                List<string > lstDoned = new List<string >(); //记录已经处理过的id

                foreach (DataRow aRow in this.tableNotUniton.Rows)
                {
                    string oid = aRow["OBJECTID"].ToString();
                    IFeature smallFeature = this.GetFeature("OBJECTID", oid, dltbClass);
                    //if (FeatureHelper.GetFeatureStringValue(smallFeature, "DLBM") == "1006")
                    //    continue;
                    if (smallFeature == null)
                        continue;

                    IFeature maxFea = SmallTbDoHelper.getMaxFeature2(smallFeature, dltbClass,this.chk101101.Checked);

                    if (maxFea != null)
                    {
                        IGeometry maxGeo = maxFea.ShapeCopy as IGeometry;
                        ITopologicalOperator pTop = maxGeo as ITopologicalOperator;
                        IGeometry newGeo = pTop.Union(smallFeature.ShapeCopy);
                        pTop.Simplify();
                        newGeo.SnapToSpatialReference();
                        maxFea.Shape = newGeo;

                        double tbmj = area.SphereArea(maxFea.ShapeCopy);
                        FeatureHelper.SetFeatureValue(maxFea, "TBMJ", tbmj);
                        maxFea.Store();

                        inum++;
                        smallFeature.Delete();


                        lstDoned.Add(oid);
                       
                    }

                    wait.SetCaption("已处理" + inum + "个...");

                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("finishSmalltb");
                wait.Close();

                for (int kk = this.tableNotUniton.Rows.Count - 1; kk >= 0; kk--)
                {
                    DataRow aRow = this.tableNotUniton.Rows[kk];
                    string oid = aRow["OBJECTID"].ToString();
                    if (lstDoned.Contains(oid))
                    {
                        this.tableNotUniton.Rows.RemoveAt(kk);
                    }
                }

                this.Cursor = Cursors.Default;
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent);
                MessageBox.Show("共" + this.tableNotUniton.Rows.Count + "个图斑，共处理完成" + inum + "个图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.gridControlUnFinish.DataSource = this.tableNotUniton;


            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                this.Cursor = Cursors.Default;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
        }

        



        #region  10和1101地类图斑列表右键菜单
        private void gridControlDL101101_Click(object sender, EventArgs e)
        {
            
        }
        
        #endregion 

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
                    for (int i = 0; i < this.gridViewSmalltb.SelectedRowsCount; i++)
                    {
                        int idx = this.gridViewSmalltb.GetSelectedRows()[i];
                        try
                        {
                            string sid = this.gridViewSmalltb.GetRowCellValue(idx, fldName).ToString();
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
                    for (int i = 0; i < this.gridViewUnFinished.SelectedRowsCount; i++)
                    {
                        int idx = this.gridViewUnFinished.GetSelectedRows()[i];
                        try
                        {
                            string sid = this.gridViewUnFinished.GetRowCellValue(idx, fldName).ToString();
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
                    for (int i = 0; i < this.gridView01101.SelectedRowsCount; i++)
                    {
                        int idx = this.gridView01101.GetSelectedRows()[i];
                        try
                        {
                            string sid = this.gridView01101.GetRowCellValue(idx, fldName).ToString();
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
            }
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                       

            
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (this.dltbLayer == null) return;
            if (this.gridViewSmalltb.SelectedRowsCount == 0)
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
                        string sBSM = this.gridViewSmalltb.GetRowCellValue(gridViewSmalltb.FocusedRowHandle, fldName).ToString();
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
                case 1:
                    try
                    {
                        string sBSM = this.gridViewUnFinished.GetRowCellValue(gridViewUnFinished.FocusedRowHandle, fldName).ToString();
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
                case 2:
                    try
                    {
                        string sBSM = this.gridView01101.GetRowCellValue(gridView01101.FocusedRowHandle, fldName).ToString();
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

        private void gridControlSmall_Click(object sender, EventArgs e)
        {

        }

        private void gridControlUnFinish_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewUnFinished.SelectedRowsCount == 0)
                return;
            if (this.dltbLayer == null) return;
            string oid = this.gridViewUnFinished.GetRowCellValue(this.gridViewUnFinished.FocusedRowHandle, "OBJECTID").ToString();
            Int32 ioid = Convert.ToInt32(oid);
            //定位
            try
            {
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

        private void gridControlDL101101_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView01101.SelectedRowsCount == 0)
                return;
            if (this.dltbLayer == null) return;
            string oid = this.gridView01101.GetRowCellValue(this.gridView01101.FocusedRowHandle, "OBJECTID").ToString();
            Int32 ioid = Convert.ToInt32(oid);
            //定位
            try
            {
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

    }
}
