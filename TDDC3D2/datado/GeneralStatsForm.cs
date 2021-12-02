using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using RCIS.Utility;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace TDDC3D.datado
{
    public partial class GeneralStatsForm : Form
    {
        public GeneralStatsForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IMap currMap = null;
        public IWorkspace currWs = null;

        private DataTable m_dataTable = null; //行政区
        



        private DataTable m_XzqTable = null; //图斑面积
        private DataTable m_CjdcqTable = null;//权属区

        private DataTable m_zrzyTjTable = null; //自然资源面积

        private void GeneralStatsForm_Load(object sender, EventArgs e)
        {

            m_dataTable = new DataTable();
            DataColumn dc = new DataColumn("GROUP", typeof(string));
            m_dataTable.Columns.Add(dc);
            dc = new DataColumn("SUMS", typeof(double));
            m_dataTable.Columns.Add(dc);
            dc = new DataColumn("MAXS", typeof(double));
            m_dataTable.Columns.Add(dc);
            dc = new DataColumn("MINS", typeof(double));
            m_dataTable.Columns.Add(dc);
            dc = new DataColumn("MEANS", typeof(double));
            m_dataTable.Columns.Add(dc);

            //初始化结构
            this.m_XzqTable = new DataTable();
            dc = new DataColumn("QSDWDM", typeof(string));
            this.m_XzqTable.Columns.Add(dc);
            dc = new DataColumn("QSDWMC", typeof(string));
            this.m_XzqTable.Columns.Add(dc);
            dc = new DataColumn("TBMJ", typeof(double));
            this.m_XzqTable.Columns.Add(dc);
            dc = new DataColumn("MSSM", typeof(string));
            this.m_XzqTable.Columns.Add(dc); //描述说明
            dc = new DataColumn("TBNUM", typeof(int));
            this.m_XzqTable.Columns.Add(dc);

            //权属区
            this.m_CjdcqTable = new DataTable();
            dc = new DataColumn("QSDWDM", typeof(string));
            this.m_CjdcqTable.Columns.Add(dc);
            dc = new DataColumn("QSDWMC", typeof(string));
            this.m_CjdcqTable.Columns.Add(dc);
            dc = new DataColumn("TBMJ", typeof(double));
            this.m_CjdcqTable.Columns.Add(dc);
            dc = new DataColumn("MSSM", typeof(string));
            this.m_CjdcqTable.Columns.Add(dc); //描述说明
            dc = new DataColumn("TBNUM", typeof(int));
            this.m_CjdcqTable.Columns.Add(dc);


            this.m_zrzyTjTable = new DataTable();
            dc = new DataColumn("QSDWDM", typeof(string));
            this.m_zrzyTjTable.Columns.Add(dc);
            dc = new DataColumn("QSDWMC", typeof(string));
            this.m_zrzyTjTable.Columns.Add(dc);
            dc = new DataColumn("SLMJ", typeof(double));
            this.m_zrzyTjTable.Columns.Add(dc);
            dc = new DataColumn("SENLMJ", typeof(double));
            this.m_zrzyTjTable.Columns.Add(dc);
            dc = new DataColumn("CYMJ", typeof(double));
            this.m_zrzyTjTable.Columns.Add(dc);
            dc = new DataColumn("HDMJ", typeof(double));
            this.m_zrzyTjTable.Columns.Add(dc);
            dc = new DataColumn("TTMJ", typeof(double));
            this.m_zrzyTjTable.Columns.Add(dc);


            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayers, currMap);
            
            this.xtraTabControl1.SelectedTabPageIndex = 1;
        }



        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "")
                return;
            this.cmbTjFields.Properties.Items.Clear();
            this.cmbGroupField.Properties.Items.Clear();

            string ClassName =OtherHelper.GetLeftName( this.cmbLayers.Text.Trim());
            IFeatureWorkspace pFeaWs=this.currWs as IFeatureWorkspace;
            IFeatureClass pFeatureClass = pFeaWs.OpenFeatureClass(ClassName);
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                IField aFld = pFeatureClass.Fields.get_Field(i);
                string fldName = aFld.Name.Trim();

                if (aFld.Type==esriFieldType.esriFieldTypeString)
                {
                    this.cmbGroupField.Properties.Items.Add(aFld.Name+"|"+aFld.AliasName);
                }
                if (aFld.Type == esriFieldType.esriFieldTypeInteger || aFld.Type == esriFieldType.esriFieldTypeDouble ||
                    aFld.Type==esriFieldType.esriFieldTypeSingle || aFld.Type==esriFieldType.esriFieldTypeSmallInteger
                    )
                {
                    this.cmbTjFields.Properties.Items.Add(aFld.Name + "|" + aFld.AliasName);
                }
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureClass);


        }

        private void buildXzqMj(IFeatureClass pXzqClass,IFeatureClass pDltbClass)
        {
            IFeatureLayer pXzqLayer = new FeatureLayerClass();
            pXzqLayer.FeatureClass = pXzqClass;
            IIdentify identify = pXzqLayer as IIdentify;
            IDataset xzqDS = pXzqClass as IDataset;
            IGeoDataset xzqGeoDs = xzqDS as IGeoDataset;
            IArray xzqIds = identify.Identify(xzqGeoDs.Extent);
            for (int i = 0; i < xzqIds.Count; i++)
            {
                IFeatureIdentifyObj idObj = xzqIds.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                IFeature pfea = pRow.Row as IFeature;
                string xzqmc = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(pfea, "XZQDM");
                string mssm = FeatureHelper.GetFeatureStringValue(pfea, "MSSM");

                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                //pSF.WhereClause = "ZLDWDM='" + xzqdm + "'";
                pSF.Geometry = pfea.ShapeCopy;

                //double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                //FeatureHelper.StatsFieldValue(pDltbClass, pSF as IQueryFilter, "TBMJ", out dmax, out dmin, out dsum, out dmean);
                double  dsum = FeatureHelper.StatsFieldSumValue(pDltbClass, pSF as IQueryFilter, "TBMJ");
                int tbnum = pDltbClass.FeatureCount(pSF as IQueryFilter);
                dsum = MathHelper.Round(dsum, 2);
                DataRow dr = this.m_XzqTable.NewRow();
                dr["QSDWDM"] = xzqdm;
                dr["QSDWMC"] = xzqmc;
                dr["TBMJ"] = dsum;
                dr["MSSM"] = mssm;
                dr["TBNUM"] = tbnum;
                this.m_XzqTable.Rows.Add(dr);

               

            }
            this.gridControl2.DataSource = this.m_XzqTable;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pXzqLayer);

        }

        private void BuildCjdcqMj(IFeatureClass pCjdcqClass,IFeatureClass pDltbClass)
        {
            IFeatureLayer pQSQLayer = new FeatureLayerClass();
            pQSQLayer.FeatureClass = pCjdcqClass;
            IIdentify identify = pQSQLayer as IIdentify;
            IDataset qsqDS = pCjdcqClass as IDataset;
            IGeoDataset cjdcqDs = qsqDS as IGeoDataset;
            IArray cjdcqIds = identify.Identify(cjdcqDs.Extent);
            if (cjdcqIds != null)
            {
                for (int i = 0; i < cjdcqIds.Count; i++)
                {
                    IFeatureIdentifyObj idObj = cjdcqIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    
                    IFeature pfea = pRow.Row as IFeature;
                    string xzqmc = FeatureHelper.GetFeatureStringValue(pfea, "ZLDWMC");
                    string xzqdm = FeatureHelper.GetFeatureStringValue(pfea, "ZLDWDM");
                   


                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    pSF.Geometry = pfea.ShapeCopy;

                    double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                    
                    //FeatureHelper.StatsFieldValue(pDltbClass, pSF as IQueryFilter, "TBMJ", out dmax, out dmin, out dsum, out dmean);
                    dsum = FeatureHelper.StatsFieldSumValue(pDltbClass, pSF as IQueryFilter, "TBMJ");
                    dsum = MathHelper.Round(dsum, 2);
                    int tbnum = pDltbClass.FeatureCount(pSF as IQueryFilter);

                    DataRow dr = this.m_CjdcqTable.NewRow();
                    dr["QSDWDM"] = xzqdm;
                    dr["QSDWMC"] = xzqmc;
                    dr["TBMJ"] = dsum;
                    dr["MSSM"] = "";
                    dr["TBNUM"] = tbnum;
                    this.m_CjdcqTable.Rows.Add(dr);

                }
            }
            this.gridControl3.DataSource = this.m_CjdcqTable;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pQSQLayer);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            if (this.xtraTabControl1.SelectedTabPageIndex == 0)
            {
                #region 常规统计
                if (this.cmbLayers.Text.Trim() == "")
                    return;
                string calssName =OtherHelper.GetLeftName( this.cmbLayers.Text.Trim());
                if (this.cmbTjFields.Text.Trim() == "")
                    return;
                string tjField = OtherHelper.GetLeftName(this.cmbTjFields.Text.Trim());

                this.Cursor = Cursors.WaitCursor;

                
                IFeatureClass pFeatureClass = pFeaWs.OpenFeatureClass(calssName);
                double dmax = 0, dmin = 0, dsum = 0, dmean = 0;
                if (this.cmbGroupField.Text.Trim() == "")
                {
                    //没有分组字段
                    FeatureHelper.StatsFieldValue(pFeatureClass, null,tjField, out dmax, out dmin, out dsum, out dmean);
                    this.m_dataTable.Rows.Clear();
                    DataRow dr = m_dataTable.NewRow();
                    dr["MAXS"] = MathHelper.RoundEx( dmax,2);
                    dr["MINS"] = MathHelper.RoundEx(dmin,2);
                    dr["SUMS"] =MathHelper.RoundEx(dsum,2);
                    dr["MEANS"] = MathHelper.RoundEx(dmean,2);
                    this.m_dataTable.Rows.Add(dr);
                    this.gridControl1.DataSource = this.m_dataTable;


                }
                else
                {
                    string groupField = OtherHelper.GetLeftName(this.cmbGroupField.Text.Trim());
                    this.m_dataTable.Rows.Clear();
                    ArrayList arUniq = FeatureHelper.GetUniqueFieldValueByDataStatistics(pFeatureClass,null, groupField);
                    foreach (string aValue in arUniq)
                    {
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = groupField + " = '" + aValue + "'";
                        FeatureHelper.StatsFieldValue(pFeatureClass, pQf, tjField, out dmax, out dmin, out dsum, out dmean);
                        DataRow dr = m_dataTable.NewRow();
                        dr["GROUP"] = aValue;
                        dr["MAXS"] = MathHelper.RoundEx(dmax, 2);
                        dr["MINS"] = MathHelper.RoundEx(dmin, 2);
                        dr["SUMS"] = MathHelper.RoundEx(dsum, 2);
                        dr["MEANS"] = MathHelper.RoundEx(dmean, 2);
                        this.m_dataTable.Rows.Add(dr);
                    }
                    this.gridControl1.DataSource = this.m_dataTable;

                }
                this.Cursor = Cursors.Default;
                #endregion 

            }
            else if (this.xtraTabControl1.SelectedTabPageIndex == 1)
            {
                #region 统计土地利用
                
                IFeatureClass pXzqClass = null;
                IFeatureClass pDltbClass = null;
                IFeatureClass pQsqClass = null;
                try
                {
                    pXzqClass = pFeaWs.OpenFeatureClass("XZQ");
                    pDltbClass = pFeaWs.OpenFeatureClass("DLTB");
                    pQsqClass = pFeaWs.OpenFeatureClass("CJDCQ");

                }
                catch { }
                if (pXzqClass == null) return;
                if (pDltbClass == null) return;
                if (pQsqClass == null) return;


                this.m_XzqTable.Rows.Clear();
                this.m_CjdcqTable.Rows.Clear();

                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "正在计算，请稍等...");
                wait.Show();
                wait.SetCaption("正在统计XZQ的图斑面积...");
                buildXzqMj(pXzqClass,pDltbClass);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                wait.SetCaption("正在统计CJDCQ的图斑面积...");
                BuildCjdcqMj(pQsqClass, pDltbClass);

                wait.Close();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pXzqClass);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pDltbClass);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQsqClass);

                GC.Collect();
                GC.WaitForPendingFinalizers();

                #endregion 

            }
            
        }

     

        private void xtraTabControl1_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSetCmj_Click(object sender, EventArgs e)
        {
            if (m_XzqTable == null)
                return;

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "正在计算，请稍等...");
            wait.Show();
            try
            {
                foreach (DataRow dr in this.m_XzqTable.Rows)
                {
                    string qsdwdm = dr["QSDWDM"].ToString();
                    string mssm = dr["MSSM"].ToString();

                    double dmj = 0;
                    double.TryParse(dr["TBMJ"].ToString(), out dmj);
                    //string sql = "update XZQ set JSMJ=" + dmj + " where XZQDM='" + qsdwdm + "' and MSSM='"+mssm+"'";
                    //this.currWs.ExecuteSQL(sql);

                    //2019-3-11修改,控制面积改为调查面积
                    //sql = "update XZQ set KZMJ=" + dmj + " where XZQDM='" + qsdwdm + "' and MSSM='" + mssm + "' ";
                    string   sql = "update XZQ set DCMJ=" + dmj + " where XZQDM='" + qsdwdm + "' and MSSM='" + mssm + "'";
                    this.currWs.ExecuteSQL(sql);
                }


                //m_qsqTable 针对权属单位代码 分组求和


                //先计算zldwdm 唯一值
                DataTable dtZldwdm = m_CjdcqTable.DefaultView.ToTable(true, "QSDWDM");
                foreach(DataRow aZldw in dtZldwdm.Rows)
                {
                    string dm = aZldw["QSDWDM"].ToString();
                    object sum = m_CjdcqTable.Compute("SUM(TBMJ)", "QSDWDM='" + dm + "'");
                    double dSum=(double)sum;

                    string sql = "update CJDCQ set DCMJ=" + dSum + " where ZLDWDM='" + dm + "'";
                    this.currWs.ExecuteSQL(sql);


                }                

                //foreach (DataRow dr2 in this.m_QsqTable.Rows)
                //{
                //    string qsdwdm = dr2["QSDWDM"].ToString();
                //    double dmj = 0;
                //    double.TryParse(dr2["TBMJ"].ToString(), out dmj);
                //    //string sql = "update TDQSQ set JSMJ=" + dmj + " where ZLDWDM='" + qsdwdm + "'";
                //    //this.currWs.ExecuteSQL(sql);

                //    string sql = "update TDQSQ set DCMJ=" + dmj + " where ZLDWDM='" + qsdwdm + "'";
                //    this.currWs.ExecuteSQL(sql);

                //}
                wait.Close();
                MessageBox.Show("赋值完毕!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);

            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {

            //if (this.xtraTabControl1.SelectedTabPageIndex == 1)
            //{
            //    this.btnSetCmj.Visible = true;
            //}
            //else
            //{
            //    this.btnSetCmj.Visible = false;

            //}
        }

        private void 导出ExcelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "EXCEL文件|*.xls";
            dlg.FileName = "计算结果";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destName = dlg.FileName;
            try
            {
                this.gridControl2.ExportToXls(destName);
                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
            }
        }

        private void 导出ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "EXCEL文件|*.xls";
            dlg.FileName = "计算结果";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destName = dlg.FileName;
            try
            {
                this.gridControl3.ExportToXls(destName);
                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "EXCEL文件|*.xls";
            dlg.FileName = "计算结果";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destName = dlg.FileName;
            try
            {
                this.gridControl1.ExportToXls(destName);
                MessageBox.Show("导出成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
            }
        }
        
        //private void simpleButton3_Click(object sender, EventArgs e)
        //{
        //    //List<long> lst1 = new List<long>();
        //    //List<long> lst2 = new List<long>();
        //    //Dictionary<string, long> dic = new Dictionary<string, long>();

        //    //IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
        //    //IFeatureClass pXzqClass = null;
        //    //IFeatureClass pDltbClass = null;
        //    //IFeatureClass pQsqClass = null;
        //    //try
        //    //{
        //    //    pXzqClass = pFeaWs.OpenFeatureClass("XZQ");
        //    //    pDltbClass = pFeaWs.OpenFeatureClass("DLTB");
        //    //    pQsqClass = pFeaWs.OpenFeatureClass("CJDCQ");

        //    //}
        //    //catch { }
        //    //if (pXzqClass == null) return;
        //    //if (pDltbClass == null) return;
        //    //if (pQsqClass == null) return;

        //    //IQueryFilter pQf = new QueryFilterClass();
        //    //pQf.WhereClause = "ZLDWDM like '140311101%'";
        //    //IFeatureCursor pCursor = pQsqClass.Search(pQf, true);
        //    //IFeature aQsq = null;
        //    //while ((aQsq = pCursor.NextFeature()) != null)
        //    //{
        //    //    IGeometry geo = aQsq.Shape;
        //    //    string dm = FeatureHelper.GetFeatureStringValue(aQsq, "ZLDWDM");
        //    //    string mc = FeatureHelper.GetFeatureStringValue(aQsq, "ZLDWMC");

        //    //    int num = 0;
        //    //    ISpatialFilter pSF = new SpatialFilterClass();
        //    //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
        //    //    pSF.Geometry = geo;
        //    //    IFeatureCursor dltbCursor = pDltbClass.Search(pSF as IQueryFilter, true);
        //    //    IFeature adltb = null;
        //    //    while ((adltb = dltbCursor.NextFeature()) != null)
        //    //    {
        //    //        num++;
        //    //        lst1.Add(adltb.OID);
                    
        //    //    }
        //    //    int num2 = pDltbClass.FeatureCount(pSF as IQueryFilter);
        //    //    dic.Add(dm, num);

        //    //    System.Runtime.InteropServices.Marshal.ReleaseComObject(dltbCursor);
        //    //}
        //    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);



        //    //pQf = new QueryFilterClass();
        //    //pQf.WhereClause = "XZQDM ='" + 140311101 + "'";
        //    //IFeatureCursor xzqCursor = pXzqClass.Search(pQf, false);
        //    //IFeature aXzq = null;
        //    //while ((aXzq = xzqCursor.NextFeature()) != null)
        //    //{
        //    //    IGeometry geo = aXzq.Shape;
        //    //    ISpatialFilter pSF = new SpatialFilterClass();
        //    //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
        //    //    pSF.Geometry = geo;
        //    //    IFeatureCursor dltbCursor = pDltbClass.Search(pSF as IQueryFilter, true);
        //    //    IFeature adltb = null;
        //    //    while ((adltb = dltbCursor.NextFeature()) != null)
        //    //    {
        //    //        lst2.Add(adltb.OID);
        //    //    }
        //    //    System.Runtime.InteropServices.Marshal.ReleaseComObject(dltbCursor);
        //    //}
        //    //System.Runtime.InteropServices.Marshal.ReleaseComObject(xzqCursor);

        //    //foreach (long l in lst1)
        //    //{
        //    //    if (!lst2.Contains(l))
        //    //    {
        //    //        MessageBox.Show(l.ToString());
        //    //    }
        //    //}

        //}


      
    }
}
