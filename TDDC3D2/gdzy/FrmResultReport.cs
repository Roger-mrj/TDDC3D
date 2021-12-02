using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;

namespace TDDC3D.gdzy
{
    public partial class FrmResultReport : Form
    {
        public FrmResultReport()
        {
            InitializeComponent();
        }
        public IWorkspace pCurrWs = null;
        private void btnPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            btnPath.Text = dlg.SelectedPath;
        }
        Dictionary<string, string> dmmc = new Dictionary<string, string>();
        string xzqdm = "";
        string xzqmc = "";
        private void FrmResultReport_Load(object sender, EventArgs e)
        {
            btnPath.Text = Application.StartupPath + @"\output\Excel";
            IFeatureClass pFeaClass=(pCurrWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            IFeature pFea=RCIS.GISCommon.GetFeaturesHelper.GetFirstFeature(pFeaClass,null);
            xzqdm=pFea.get_Value(pFea.Fields.FindField("XZQDM")).ToString();
            xzqdm=xzqdm.Substring(0,6);
            string SQL="Select MC From SYS_XZQ Where DM='"+xzqdm+"'";
            DataTable dt=RCIS.Database.LS_SetupMDBHelper.GetDataTable(SQL,"tmp");
            xzqmc = dt.Rows[0][0].ToString();
            dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(pCurrWs as IFeatureWorkspace, "XZQ", "XZQDM", "XZQMC");


            //Dictionary<string, string> dicSort = from objDic in dmmc orderby objDic.Value descending select objDic;


            if (dt.Rows.Count > 0)
            {
                TreeNode tn = treeXZQ.Nodes.Add(xzqdm+"|"+dt.Rows[0][0].ToString());
                foreach (string item in dmmc.Keys)
                {
                    TreeNode xq = tn.Nodes.Add(item+"|"+dmmc[item]);
                }
            }
            dmmc.Add(xzqdm, dt.Rows[0][0].ToString());
            treeXZQ.ExpandAll();

            dmmc = dmmc.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);

        }
        private DataTable TP(DataTable dt, string mjField, double zmj)
        {
            object o = dt.Compute("Sum(" + mjField + ")", "");
            double tempmj;
            double.TryParse(o.ToString(), out tempmj);

            DataView dv = new DataView(dt);
            dv.Sort = mjField + " desc";
            DataTable t = dv.ToTable();
            if (tempmj != zmj)
            {
                double mj = 0;
                if (tempmj > zmj)
                {
                    mj = Math.Round(tempmj - zmj, 2);
                    int count = int.Parse((mj * 100).ToString());
                    for (int i = 0; i < count; i++)
                    {
                        int n = i % t.Rows.Count;
                        t.Rows[n][mjField] = double.Parse(t.Rows[i][mjField].ToString()) - 0.01;
                    }
                }
                else
                {
                    mj = Math.Round(zmj - tempmj, 2);
                    int count = int.Parse((mj * 100).ToString());
                    for (int i = 0; i < count; i++)
                    {
                        int n = i % t.Rows.Count;
                        t.Rows[n][mjField] = double.Parse(t.Rows[i][mjField].ToString()) + 0.01;
                    }
                }
            }
            return t;
        }
        List<DataTable> dtList;
        List<DataTable> dtListKc;
        private void btnInit_Click(object sender, EventArgs e)
        {
            IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, "FLDY", "FLDY", null);
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(pCurrWs, pTmpWs, "KCFLDY", "KCFLDY", null);
            pTmpWs.ExecuteSQL("update FLDY set zldwdm = substring(zldwdm,1,9)");
            pTmpWs.ExecuteSQL("update KCFLDY set zldwdm = substring(zldwdm,1,9)");
            string sql = "select zldwdm,szjb,zrqdm,pdjb,tchdjb,trzdjb,tryjzhljb,trphzjb,swdyxjb,trzjswrjb,gdejdljb,tbdlmj from fldy";
            DataTable table = RCIS.GISCommon.EsriDatabaseHelper.ITable2DataTable(pTmpWs, sql);

            sql = "select zldwdm,szjb,zrqdm,pdjb,tchdjb,trzdjb,tryjzhljb,trphzjb,swdyxjb,trzjswrjb,gdejdljb,tbdlmj from kcfldy";
            DataTable tableKc = RCIS.GISCommon.EsriDatabaseHelper.ITable2DataTable(pTmpWs, sql);

            //DataTable dtResult = GetSumByDatatableFL(table);
            //object o = table.Compute("Sum(tbdlmj)", "");
            //double zmj;
            //double.TryParse(o.ToString(), out zmj);
            //zmj = Math.Round(zmj / 10000, 2);
            //dtResult = TP(dtResult, "tbdlmj", zmj);
            //DataTable dtResultKc = GetSumByDatatableKC(tableKc);
            //o = tableKc.Compute("Sum(tbdlmj)", "");
            //double.TryParse(o.ToString(), out zmj);
            //zmj = Math.Round(zmj / 10000, 2);
            //dtResultKc = TP(dtResultKc, "tbdlmj", zmj);
            dtList = initReport(table);
            dtListKc = initReport(tableKc, true);
            MessageBox.Show("初始化完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<DataTable> initReport(DataTable dtRes,bool isHFDL=false)
        {
            List<DataTable> dtLst = new List<DataTable>();

            string statisticsField = "";
            string sql = "";
            for (int m = 0; m < 10; m++)
            {
                DataTable dtTmp = new DataTable();
                dtTmp.Columns.Add("mc");
                dtTmp.Columns.Add("dm");
                dtTmp.Columns.Add("hj", typeof(double));
                if (m == 0)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-自然区");
                    sql = "select zrqdm from SYS_QGGXSSZRQHSZ where xzqdm='" + xzqdm + "' ";
                    statisticsField = "zrqdm";
                }
                else if (m == 1)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-坡度");
                    sql = "select dm from DIC_36坡度级别代码表 ";
                    statisticsField = "pdjb";
                }
                else if (m == 2)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土层厚度");
                    sql = "select dm from DIC_29土层厚度代码表 ";
                    statisticsField = "tchdjb";
                }
                else if (m == 3)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤质地");
                    sql = "select dm from DIC_30土壤质地代码表 ";
                    statisticsField = "trzdjb";
                }
                else if (m == 4)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤有机质含量");
                    sql = "select dm from DIC_31土壤有机质含量代码表 ";
                    statisticsField = "tryjzhljb";
                }
                else if (m == 5)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤 pH 值");
                    sql = "select dm from DIC_32土壤PH值代码表 ";
                    statisticsField = "trphzjb";
                }
                else if (m == 6)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-生物多样性");
                    sql = "select dm from DIC_33生物多样性代码表 ";
                    //if (isHFDL)
                        statisticsField = "swdyxjb";
                    //else
                    //    statisticsField = "trswdyxjb";
                }
                else if (m == 7)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-土壤重金属污染状况");
                    sql = "select dm from DIC_34土壤重金属污染状况类型 ";
                    statisticsField = "trzjswrjb";
                    

                }
                else if (m == 8)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-熟制报表");
                    sql = "select dm from DIC_35熟制代码表 ";
                    statisticsField = "szjb";
                }
                else if (m == 9)
                {
                    UpdateStatus("正在统计耕地资源质量分类面积汇总表-耕地二级地类");
                    sql = "select dm from DIC_36耕地二级代码表 ";
                    statisticsField = "gdejdljb";
                }

                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (m == 9)
                {
                    if (isHFDL)
                    {
                        for (int i = 3; i < dt.Rows.Count; i++)
                        {
                            dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
                        }
                    }
                    
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtTmp.Columns.Add(dt.Rows[i][0].ToString(), typeof(double));
                    }
                }
                
                DataRow dr = dtTmp.NewRow();
                for (int i = 0; i < dtTmp.Columns.Count; i++)
                {
                    if (dtTmp.Columns[i].ColumnName.ToString() == "dm")
                        dr["dm"] = xzqdm;
                    else if (dtTmp.Columns[i].ColumnName.ToString() == "mc")
                        dr["mc"] = xzqmc;
                    else
                        dr[dtTmp.Columns[i].ColumnName.ToString()] = 0;
                }
                dtTmp.Rows.Add(dr);
                createTable(dtRes, statisticsField, "zldwdm", "tbdlmj", ref dtTmp);
                dtLst.Add(dtTmp);
            }
            return dtLst;
        }

        private void UpdateStatus(string txt)
        {
            this.memoEdit1.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + memoEdit1.Text;
            Application.DoEvents();
        }

        //private DataTable GetSumByDatatableFL(DataTable dt)
        //{
        //    var query = dt.AsEnumerable()
        //        .GroupBy(d => new
        //        {
        //            zldwdm = d.Field<string>("zldwdm"),
        //            szjb = d.Field<string>("szjb"),
        //            zrqdm = d.Field<string>("zrqdm"),
        //            pdjb = d.Field<string>("pdjb"),
        //            tchdjb = d.Field<string>("tchdjb"),
        //            trzdjb = d.Field<string>("trzdjb"),
        //            tryjzhljb = d.Field<string>("tryjzhljb"),
        //            trphzjb = d.Field<string>("trphzjb"),
        //            trswdyxjb = d.Field<string>("trswdyxjb"),
        //            trzjswrzkjb = d.Field<string>("szjb"),
        //            gdejdljb = d.Field<string>("gdejdljb")
        //        })
        //        .Select(g => new
        //        {
        //            zldwdm = g.Key.zldwdm,
        //            szjb = g.Key.szjb,
        //            zrqdm = g.Key.zrqdm,
        //            pdjb = g.Key.pdjb,
        //            tchdjb = g.Key.tchdjb,
        //            trzdjb = g.Key.trzdjb,
        //            tryjzhljb = g.Key.tryjzhljb,
        //            trphzjb = g.Key.trphzjb,
        //            trswdyxjb = g.Key.trswdyxjb,
        //            trzjswrzkjb = g.Key.trzjswrzkjb,
        //            gdejdljb = g.Key.gdejdljb,
        //            tbdlmj = Math.Round(g.Sum(d => d.Field<double>("tbdlmj")) / 10000,6)
        //        });

        //    DataTable dtResult = new DataTable();
        //    dtResult.Columns.Add("zldwdm");
        //    dtResult.Columns.Add("szjb");
        //    dtResult.Columns.Add("zrqdm");
        //    dtResult.Columns.Add("pdjb");
        //    dtResult.Columns.Add("tchdjb");
        //    dtResult.Columns.Add("trzdjb");
        //    dtResult.Columns.Add("tryjzhljb");
        //    dtResult.Columns.Add("trphzjb");
        //    dtResult.Columns.Add("trswdyxjb");
        //    dtResult.Columns.Add("trzjswrjb");
        //    dtResult.Columns.Add("gdejdljb");
        //    dtResult.Columns.Add(new DataColumn("tbdlmj", typeof(float)));
        //    query.ToList().ForEach(q => dtResult.Rows.Add(
        //        q.zldwdm,
        //        q.szjb,
        //        q.zrqdm,
        //        q.pdjb,
        //        q.tchdjb,
        //        q.trzdjb,
        //        q.tryjzhljb,
        //        q.trphzjb,
        //        q.trswdyxjb,
        //        q.trzjswrzkjb,
        //        q.gdejdljb,
        //        q.tbdlmj));
        //    return dtResult;
        //}

        //private DataTable GetSumByDatatableKC(DataTable dt)
        //{
        //    var query = dt.AsEnumerable()
        //        .GroupBy(d => new
        //        {
        //            zldwdm = d.Field<string>("zldwdm"),
        //            szjb = d.Field<string>("szjb"),
        //            zrqdm = d.Field<string>("zrqdm"),
        //            pdjb = d.Field<string>("pdjb"),
        //            tchdjb = d.Field<string>("tchdjb"),
        //            trzdjb = d.Field<string>("trzdjb"),
        //            tryjzhljb = d.Field<string>("tryjzhljb"),
        //            trphzjb = d.Field<string>("trphzjb"),
        //            trswdyxjb = d.Field<string>("swdyxjb"),
        //            trzjswrzkjb = d.Field<string>("szjb"),
        //            gdejdljb = d.Field<string>("gdejdljb")
        //        })
        //        .Select(g => new
        //        {
        //            zldwdm = g.Key.zldwdm,
        //            szjb = g.Key.szjb,
        //            zrqdm = g.Key.zrqdm,
        //            pdjb = g.Key.pdjb,
        //            tchdjb = g.Key.tchdjb,
        //            trzdjb = g.Key.trzdjb,
        //            tryjzhljb = g.Key.tryjzhljb,
        //            trphzjb = g.Key.trphzjb,
        //            trswdyxjb = g.Key.trswdyxjb,
        //            trzjswrzkjb = g.Key.trzjswrzkjb,
        //            gdejdljb = g.Key.gdejdljb,
        //            tbdlmj = Math.Round(g.Sum(d => d.Field<double>("tbdlmj")) / 10000,6)
        //        });

        //    DataTable dtResult = new DataTable();
        //    dtResult.Columns.Add("zldwdm");
        //    dtResult.Columns.Add("szjb");
        //    dtResult.Columns.Add("zrqdm");
        //    dtResult.Columns.Add("pdjb");
        //    dtResult.Columns.Add("tchdjb");
        //    dtResult.Columns.Add("trzdjb");
        //    dtResult.Columns.Add("tryjzhljb");
        //    dtResult.Columns.Add("trphzjb");
        //    dtResult.Columns.Add("swdyxjb");
        //    dtResult.Columns.Add("trzjswrjb");
        //    dtResult.Columns.Add("gdejdljb");
        //    dtResult.Columns.Add(new DataColumn("tbdlmj", typeof(float)));
        //    query.ToList().ForEach(q => dtResult.Rows.Add(
        //        q.zldwdm,
        //        q.szjb,
        //        q.zrqdm,
        //        q.pdjb,
        //        q.tchdjb,
        //        q.trzdjb,
        //        q.tryjzhljb,
        //        q.trphzjb,
        //        q.trswdyxjb,
        //        q.trzjswrzkjb,
        //        q.gdejdljb,
        //        q.tbdlmj));
        //    return dtResult;
        //}
        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(btnPath.Text))
            {
                MessageBox.Show("请选择输出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dtList == null || dtList.Count != 10)
            {
                MessageBox.Show("请先初始化基础库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (radioGroup1.SelectedIndex == 0)
            {
                if (chk1.Checked)
                    exportExcelHZ(dtList, btnPath.Text, "耕地资源质量分类面积汇总表", xzqdm, xzqmc, true);
                if (chk2.Checked)
                    exportExcelZRQ(dtList[0], btnPath.Text, "耕地资源质量分类面积汇总表-自然区", xzqdm, xzqmc, true);
                if (chk3.Checked)
                    exportExcel(dtList[1], btnPath.Text, "耕地资源质量分类面积汇总表-坡度", xzqdm, xzqmc,true, true);
                if (chk4.Checked)
                    exportExcel(dtList[2], btnPath.Text, "耕地资源质量分类面积汇总表-土层厚度", xzqdm, xzqmc, true, true);
                if (chk5.Checked)
                    exportExcel(dtList[3], btnPath.Text, "耕地资源质量分类面积汇总表-土壤质地", xzqdm, xzqmc, true, true);
                if (chk6.Checked)
                    exportExcel(dtList[4], btnPath.Text, "耕地资源质量分类面积汇总表-土壤有机质含量", xzqdm, xzqmc, true, true);
                if (chk7.Checked)
                    exportExcel(dtList[5], btnPath.Text, "耕地资源质量分类面积汇总表-土壤pH值", xzqdm, xzqmc, true, true);
                if (chk8.Checked)
                    exportExcel(dtList[6], btnPath.Text, "耕地资源质量分类面积汇总表-生物多样性", xzqdm, xzqmc, true, true);
                if (chk9.Checked)
                    exportExcel(dtList[7], btnPath.Text, "耕地资源质量分类面积汇总表-土壤重金属污染状况", xzqdm, xzqmc, true, true);
                if (chk10.Checked)
                    exportExcel(dtList[8], btnPath.Text, "耕地资源质量分类面积汇总表-熟制", xzqdm, xzqmc, true, true);
                if (chk11.Checked)
                    exportExcel(dtList[9], btnPath.Text, "耕地资源质量分类面积汇总表-耕地二级地类", xzqdm, xzqmc, true, true);
                
            }
            if (radioGroup1.SelectedIndex == 1)
            {
                if (chk1.Checked)
                    exportExcelHZ(dtListKc, btnPath.Text, "耕地资源质量分类面积汇总表(JKHF)", xzqdm, xzqmc, true);
                if (chk2.Checked)
                    exportExcelZRQ(dtListKc[0], btnPath.Text, "耕地资源质量分类面积汇总表-自然区", xzqdm, xzqmc, true);
                if (chk3.Checked)
                    exportExcel(dtListKc[1], btnPath.Text, "耕地资源质量分类面积汇总表-坡度", xzqdm, xzqmc, false, true);
                if (chk4.Checked)
                    exportExcel(dtListKc[2], btnPath.Text, "耕地资源质量分类面积汇总表-土层厚度", xzqdm, xzqmc, false, true);
                if (chk5.Checked)
                    exportExcel(dtListKc[3], btnPath.Text, "耕地资源质量分类面积汇总表-土壤质地", xzqdm, xzqmc, false, true);
                if (chk6.Checked)
                    exportExcel(dtListKc[4], btnPath.Text, "耕地资源质量分类面积汇总表-土壤有机质含量", xzqdm, xzqmc, false, true);
                if (chk7.Checked)
                    exportExcel(dtListKc[5], btnPath.Text, "耕地资源质量分类面积汇总表-土壤pH值", xzqdm, xzqmc, false, true);
                if (chk8.Checked)
                    exportExcel(dtListKc[6], btnPath.Text, "耕地资源质量分类面积汇总表-生物多样性", xzqdm, xzqmc, false, true);
                if (chk9.Checked)
                    exportExcel(dtListKc[7], btnPath.Text, "耕地资源质量分类面积汇总表-土壤重金属污染状况", xzqdm, xzqmc, false, true);
                if (chk10.Checked)
                    exportExcel(dtListKc[8], btnPath.Text, "耕地资源质量分类面积汇总表-熟制", xzqdm, xzqmc, false, true);
                if (chk11.Checked)
                    exportExcel(dtListKc[9], btnPath.Text, "耕地资源质量分类面积汇总表-耕地二级地类(JKHF)", xzqdm, xzqmc, false, true);
            }

            
        }


        //private void exportExcel(DataTable dt,string destExcelDir,string tableName, string XZQDM, string XZQMC, Boolean isVisible = false, int iScale = 1)
        //{
        //    try
        //    {
        //        if (dt.Rows.Count < 2)
        //        {
        //            MessageBox.Show("" + tableName + "表统计失败，请完善数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + @"\" + tableName + ".xlsx";
        //        string dateStr;
        //        if (isVisible)
        //            dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
        //          + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
        //        else
        //            dateStr = "";

        //        string excelReportFilename = destExcelDir + @"\(" + XZQDM + ")" +xzqmc+ tableName + "" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
        //        Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
        //        Aspose.Cells.Cells cells = sheet.Cells;
        //        //边框和 数值 格式
        //        Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
        //        styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Number = 2;
        //        styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Right;

        //        Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
        //        styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
        //        styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Number = 49; //@

        //        int rowIndex = 4;
        //        int colIndex = 0;

        //        //cells[1, 1].PutValue(XZQDM);
        //        //cells[1, 3].PutValue(XZQMC);
        //        //cells[1, 4].PutValue("单位：");
        //        //cells[1, 5].PutValue(DateTime.Now.ToLongDateString());
        //        //if (iScale == 15) cells[1, 5].PutValue("亩");
        //        //else
        //        //    cells[1, 5].PutValue("公顷");

        //        if (dt.Rows.Count == 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                for (int k = 0; k < dt.Columns.Count; k++)
        //                {
        //                    if (k < 2)
        //                    {
        //                        cells[rowIndex + i, colIndex + k].SetStyle(styleTxt);
        //                        cells[rowIndex + i, colIndex + k].PutValue(dt.Rows[i][k].ToString());
        //                    }
        //                    else
        //                    {
        //                        cells[rowIndex + i, colIndex + k].SetStyle(styleNum,true);
        //                        double mj = 0;
        //                        cells[rowIndex + i, colIndex + k].PutValue(mj);
        //                    }
                            
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                for (int k = 0; k < dt.Columns.Count; k++)
        //                {
        //                    if (k < 2)
        //                    {
        //                        cells[rowIndex + i, colIndex + k].SetStyle(styleTxt);
        //                        cells[rowIndex + i, colIndex + k].PutValue(dt.Rows[i][k].ToString());
        //                    }
        //                    else
        //                    {
        //                        cells[rowIndex + i, colIndex + k].SetStyle(styleNum,true);
        //                        double mj = 0;
        //                        double.TryParse(dt.Rows[i][k].ToString(), out mj);
        //                        cells[rowIndex + i, colIndex + k].PutValue(mj);
        //                    }
        //                }
        //            }
        //        }


        //        wk.Save(excelReportFilename);
        //        if (isVisible)
        //        {
        //            try
        //            {
        //                System.Diagnostics.Process.Start(excelReportFilename);
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void exportExcelHZ(List<DataTable> dtList, string destExcelDir, string tableName, string XZQDM, string XZQMC, Boolean isVisible = false, int iScale = 1)
        //{
        //    try
        //    {
                
        //        string tmplateFile = RCIS.Global.AppParameters.TemplatePath + @"\" + tableName + ".xlsx";
        //        string dateStr;
        //        if (isVisible)
        //            dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
        //          + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
        //        else
        //            dateStr = "";

        //        string excelReportFilename = destExcelDir + @"\(" + XZQDM + ")" + xzqmc + tableName + "" + dateStr + ".xlsx";
        //        System.IO.File.Copy(tmplateFile, excelReportFilename);

        //        Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
        //        Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
        //        Aspose.Cells.Cells cells = sheet.Cells;
        //        //边框和 数值 格式
        //        Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
        //        styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleNum.Number = 2;
        //        styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
        //        styleNum.VerticalAlignment = Aspose.Cells.TextAlignmentType.Center;

        //        Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
        //        styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
        //        styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
        //        styleTxt.Number = 49; //@

        //        int rowIndex = 3;
        //        int colIndex = 2;

        //        string sql = "select zrqdm,zrqmc from SYS_QGGXSSZRQHSZ where xzqdm='" + xzqdm + "'";
        //        DataTable dtZrq = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
        //        cells[2, 2].SetStyle(styleTxt);
        //        cells[2, 2].PutValue(dtZrq.Rows[0][0].ToString());
        //        cells[3, 2].SetStyle(styleTxt);
        //        cells[3, 2].PutValue(dtZrq.Rows[0][1].ToString());
        //        List<string> hjVal = new List<string>();
        //        for (int i = 0; i < dtList.Count; i++)
        //        {
        //            DataTable dt = dtList[i];
        //            if (dt.Rows.Count > 0)
        //            {
        //                hjVal.Add(dt.Rows[0][2].ToString());
        //                for (int j = 3; j < dt.Columns.Count; j++)
        //                {
        //                    double val = 0;
        //                    double.TryParse(dt.Rows[0][j].ToString(),out val);
        //                    rowIndex++;
        //                    cells[rowIndex, 2].SetStyle(styleNum,true);
        //                    cells[rowIndex, 2].PutValue(val);
        //                }
        //            }
        //            else
        //            {
        //                hjVal.Add(0.ToString());
        //                for (int j = 3; j < dt.Columns.Count; j++)
        //                {
        //                    double val = 0;
        //                    rowIndex++;
        //                    cells[rowIndex, 2].SetStyle(styleNum,true);
        //                    cells[rowIndex, 2].PutValue(val);
        //                }
        //            }

        //        }

        //        cells[4, 3].SetStyle(styleNum,true);
        //        cells[4, 3].PutValue(double.Parse(hjVal[0]));

        //        cells[5, 3].SetStyle(styleNum,true);
        //        cells[5, 3].PutValue(double.Parse(hjVal[1]));

        //        cells[10, 3].SetStyle(styleNum,true);
        //        cells[10, 3].PutValue(double.Parse(hjVal[2]));

        //        cells[13, 3].SetStyle(styleNum,true);
        //        cells[13, 3].PutValue(double.Parse(hjVal[3]));

        //        cells[16, 3].SetStyle(styleNum,true);
        //        cells[16, 3].PutValue(double.Parse(hjVal[4]));

        //        cells[19, 3].SetStyle(styleNum,true);
        //        cells[19, 3].PutValue(double.Parse(hjVal[5]));

        //        cells[24, 3].SetStyle(styleNum,true);
        //        cells[24, 3].PutValue(double.Parse(hjVal[6]));

        //        cells[27, 3].SetStyle(styleNum,true);
        //        cells[27, 3].PutValue(double.Parse(hjVal[7]));

        //        cells[30, 3].SetStyle(styleNum,true);
        //        cells[30, 3].PutValue(double.Parse(hjVal[8]));

        //        cells[33, 3].SetStyle(styleNum,true);
        //        cells[33, 3].PutValue(double.Parse(hjVal[9]));


        //        wk.Save(excelReportFilename);
        //        if (isVisible)
        //        {
        //            try
        //            {
        //                System.Diagnostics.Process.Start(excelReportFilename);
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void createTable(DataTable dataTable, string groupingColumn, string nameColumn, string dataColumn,ref DataTable resultTable)
        //{
        //    //取出不重复的需要转换的列的数据
        //    DataTable distinct_GroupingColumn = dataTable.DefaultView.ToTable(true, groupingColumn);
        //    //取出不重复的名称列
        //    DataTable dictinct_NameColumn = dataTable.DefaultView.ToTable(true, nameColumn);
        //    //List<string> lstID = (from d in dictinct_NameColumn.AsEnumerable() select d.Field<string>("zldwdm").ToString().Substring(0, 9)).ToList();
        //    var lstID = dictinct_NameColumn.AsEnumerable().GroupBy(a => a.Field<string>("zldwdm").Substring(0, 9)).ToList();
        //    //构建新表group d by d.m
        //    //DataTable table = new DataTable();
        //    #region 构建新表的列
        //    //将名称列添加进新表
        //    DataColumn newColumn = new DataColumn();
        //    newColumn.ColumnName = nameColumn;
            
        //    #endregion
        //    #region 向新表中添加数据
        //    DataRow newRow;
        //    foreach (var item in lstID)
        //    {
        //        //添加一个新行
        //        newRow = resultTable.NewRow();
        //        //为此新行添加第一个行数据
        //        //newRow[nameColumn] = item.Key;
        //        //为此新行添加列数据
        //        double hj = 0;
        //        double xianHJ = 0;
        //        foreach (DataRow r in distinct_GroupingColumn.Rows)
        //        {
        //            if (!resultTable.Columns.Contains(r[groupingColumn].ToString()))
        //            {
        //                UpdateStatus("" + groupingColumn + "字段存在错误值，请完善数据。");
        //                resultTable.Rows.Clear();
        //                return;
        //            }
        //            object val = dataTable.Compute("sum(tbdlmj)", nameColumn + " like '" + item.Key.ToString() + "%' and " + groupingColumn + " ='" + r[groupingColumn].ToString() + "'");
        //            double sum =0;
        //            if (!string.IsNullOrWhiteSpace(val.ToString()))
        //                sum = (float)val;
        //            //double.TryParse(val.ToString(), out sum);
        //            sum = Math.Round(sum, 6);
        //            hj += sum;
        //            if (sum != 0)
        //            {
        //                //将数据与新建表进行连合
        //                newRow[r[groupingColumn].ToString()] = sum;
        //                newRow["dm"] = item.Key;
        //                newRow["mc"] = dmmc[item.Key];
        //                resultTable.Rows[0][r[groupingColumn].ToString()] = sum + double.Parse(resultTable.Rows[0][r[groupingColumn].ToString()].ToString());
        //            }
        //            xianHJ += double.Parse(resultTable.Rows[0][r[groupingColumn].ToString()].ToString());
        //        }
        //        newRow["hj"] = hj;
        //        resultTable.Rows.Add(newRow);
        //        resultTable.Rows[0]["hj"] = xianHJ;
        //    }
        //    #endregion

        //    //return table;
        //}

        private DataTable GetSumByDatatableFL(DataTable dt)
        {
            var query = dt.AsEnumerable()
                .GroupBy(d => new
                {
                    zldwdm = d.Field<string>("zldwdm"),
                    szjb = d.Field<string>("szjb"),
                    zrqdm = d.Field<string>("zrqdm"),
                    pdjb = d.Field<string>("pdjb"),
                    tchdjb = d.Field<string>("tchdjb"),
                    trzdjb = d.Field<string>("trzdjb"),
                    tryjzhljb = d.Field<string>("tryjzhljb"),
                    trphzjb = d.Field<string>("trphzjb"),
                    trswdyxjb = d.Field<string>("trswdyxjb"),
                    trzjswrzkjb = d.Field<string>("trzjswrjb"),
                    gdejdljb = d.Field<string>("gdejdljb")
                })
                .Select(g => new
                {
                    zldwdm = g.Key.zldwdm,
                    szjb = g.Key.szjb,
                    zrqdm = g.Key.zrqdm,
                    pdjb = g.Key.pdjb,
                    tchdjb = g.Key.tchdjb,
                    trzdjb = g.Key.trzdjb,
                    tryjzhljb = g.Key.tryjzhljb,
                    trphzjb = g.Key.trphzjb,
                    trswdyxjb = g.Key.trswdyxjb,
                    trzjswrzkjb = g.Key.trzjswrzkjb,
                    gdejdljb = g.Key.gdejdljb,
                    tbdlmj = Math.Round(g.Sum(d => d.Field<double>("tbdlmj")) / 10000, 6)
                });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("zldwdm");
            dtResult.Columns.Add("szjb");
            dtResult.Columns.Add("zrqdm");
            dtResult.Columns.Add("pdjb");
            dtResult.Columns.Add("tchdjb");
            dtResult.Columns.Add("trzdjb");
            dtResult.Columns.Add("tryjzhljb");
            dtResult.Columns.Add("trphzjb");
            dtResult.Columns.Add("trswdyxjb");
            dtResult.Columns.Add("trzjswrjb");
            dtResult.Columns.Add("gdejdljb");
            dtResult.Columns.Add(new DataColumn("tbdlmj", typeof(float)));
            query.ToList().ForEach(q => dtResult.Rows.Add(
                q.zldwdm,
                q.szjb,
                q.zrqdm,
                q.pdjb,
                q.tchdjb,
                q.trzdjb,
                q.tryjzhljb,
                q.trphzjb,
                q.trswdyxjb,
                q.trzjswrzkjb,
                q.gdejdljb,
                q.tbdlmj));
            return dtResult;
        }

        private DataTable GetSumByDatatableKC(DataTable dt)
        {
            var query = dt.AsEnumerable()
                .GroupBy(d => new
                {
                    zldwdm = d.Field<string>("zldwdm"),
                    szjb = d.Field<string>("szjb"),
                    zrqdm = d.Field<string>("zrqdm"),
                    pdjb = d.Field<string>("pdjb"),
                    tchdjb = d.Field<string>("tchdjb"),
                    trzdjb = d.Field<string>("trzdjb"),
                    tryjzhljb = d.Field<string>("tryjzhljb"),
                    trphzjb = d.Field<string>("trphzjb"),
                    trswdyxjb = d.Field<string>("swdyxjb"),
                    trzjswrzkjb = d.Field<string>("trzjswrjb"),
                    gdejdljb = d.Field<string>("gdejdljb")
                })
                .Select(g => new
                {
                    zldwdm = g.Key.zldwdm,
                    szjb = g.Key.szjb,
                    zrqdm = g.Key.zrqdm,
                    pdjb = g.Key.pdjb,
                    tchdjb = g.Key.tchdjb,
                    trzdjb = g.Key.trzdjb,
                    tryjzhljb = g.Key.tryjzhljb,
                    trphzjb = g.Key.trphzjb,
                    trswdyxjb = g.Key.trswdyxjb,
                    trzjswrzkjb = g.Key.trzjswrzkjb,
                    gdejdljb = g.Key.gdejdljb,
                    tbdlmj = Math.Round(g.Sum(d => d.Field<double>("tbdlmj")) / 10000, 6)
                });

            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("zldwdm");
            dtResult.Columns.Add("szjb");
            dtResult.Columns.Add("zrqdm");
            dtResult.Columns.Add("pdjb");
            dtResult.Columns.Add("tchdjb");
            dtResult.Columns.Add("trzdjb");
            dtResult.Columns.Add("tryjzhljb");
            dtResult.Columns.Add("trphzjb");
            dtResult.Columns.Add("swdyxjb");
            dtResult.Columns.Add("trzjswrjb");
            dtResult.Columns.Add("gdejdljb");
            dtResult.Columns.Add(new DataColumn("tbdlmj", typeof(float)));
            query.ToList().ForEach(q => dtResult.Rows.Add(
                q.zldwdm,
                q.szjb,
                q.zrqdm,
                q.pdjb,
                q.tchdjb,
                q.trzdjb,
                q.tryjzhljb,
                q.trphzjb,
                q.trswdyxjb,
                q.trzjswrzkjb,
                q.gdejdljb,
                q.tbdlmj));
            return dtResult;
        }

        private void exportExcel(DataTable dt, string destExcelDir, string tableName, string xzqdm, string xzqmc,bool isGD, Boolean isVisible = false, int iScale = 1)
        {
            try
            {
                string tmplateFile = RCIS.Global.AppParameters.TemplatePath + @"\" + tableName + ".xlsx";
                string dateStr;
                if (isVisible)
                    dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                else
                    dateStr = "";
                if (tableName.Contains("JKHF"))
                    tableName = tableName.Replace("(JKHF)", "");
                string excelReportFilename = destExcelDir + @"\(" + xzqdm + ")" + xzqmc + tableName + "" + dateStr + ".xlsx";
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
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
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                int rowIndex = 4;
                int colIndex = 0;

                //cells[1, 1].PutValue(XZQDM);
                //cells[1, 3].PutValue(XZQMC);
                //cells[1, 4].PutValue("单位：");
                //cells[1, 5].PutValue(DateTime.Now.ToLongDateString());
                //if (iScale == 15) cells[1, 5].PutValue("亩");
                //else
                //    cells[1, 5].PutValue("公顷");

                if (dt.Rows.Count == 0)
                {
                    List<string> arr = new List<string>();
                    if(isGD)
                        arr = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "FLDY", "ZLDWDM", 9);
                    else
                        arr = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "KCFLDY", "ZLDWDM", 9);

                    arr.Insert(0, xzqdm);
                    //int i = 0;
                    for (int i = 0; i < arr.Count; i++)
                    {
                        string item = arr[i];
                        string value = dmmc[item];

                        cells[rowIndex + i, colIndex].SetStyle(styleTxt);
                        cells[rowIndex + i, colIndex].PutValue(value);

                        cells[rowIndex + i, colIndex + 1].SetStyle(styleTxt);
                        cells[rowIndex + i, colIndex + 1].PutValue(item);

                        for (int k = 2; k < dt.Columns.Count; k++)
                        {
                            cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                            double mj = 0;
                            cells[rowIndex + i, colIndex + k].PutValue(mj);
                        }
                        //i++;
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k < 2)
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleTxt);
                                cells[rowIndex + i, colIndex + k].PutValue(dt.Rows[i][k].ToString());
                            }
                            else
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                                double mj = 0;
                                double.TryParse(dt.Rows[i][k].ToString(), out mj);
                                cells[rowIndex + i, colIndex + k].PutValue(mj);
                            }
                        }
                    }
                }


                wk.Save(excelReportFilename);
                if (isVisible)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(excelReportFilename);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void exportExcelHZ(List<DataTable> dtList, string destExcelDir, string tableName, string XZQDM, string XZQMC, Boolean isVisible = false, int iScale = 1)
        {
            try
            {
                string tmplateFile = System.Environment.CurrentDirectory + @"\template\" + tableName + ".xlsx";
                string dateStr;
                if (isVisible)
                    dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                else
                    dateStr = "";

                string excelReportFilename = destExcelDir + @"\(" + XZQDM + ")" + XZQMC + (tableName.IndexOf("(") == -1 ? tableName : tableName.Substring(0, tableName.IndexOf("("))) + "" + dateStr + ".xlsx";
                if (File.Exists(excelReportFilename)) File.Delete(excelReportFilename);
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
                Aspose.Cells.Worksheet sheet = wk.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                //边框和 数值 格式
                Aspose.Cells.Style styleNum = wk.Styles[wk.Styles.Add()];
                styleNum.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleNum.Number = 2;
                styleNum.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleNum.VerticalAlignment = Aspose.Cells.TextAlignmentType.Center;

                Aspose.Cells.Style styleTxt = wk.Styles[wk.Styles.Add()];
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                int rowIndex = 3;
                int colIndex = 2;

                string sql = "select zrqdm,zrqmc from SYS_QGGXSSZRQHSZ where xzqdm='" + XZQDM + "'";
                DataTable dtZrq = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (dtZrq.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到" + XZQMC + "的自然区数据，请导入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string zrqdm = dtZrq.Rows[0][0].ToString();
                int izrq = 0;
                int.TryParse(zrqdm, out izrq);
                //cells[2, 2].SetStyle(styleTxt);
                //cells[2, 2].PutValue(dtZrq.Rows[0][0].ToString());
                //cells[3, 2].SetStyle(styleTxt);
                //cells[3, 2].PutValue(dtZrq.Rows[0][1].ToString());
                List<string> hjVal = new List<string>();
                for (int i = 0; i < dtList.Count; i++)
                {
                    DataTable dt = dtList[i];
                    if (dt.Rows.Count > 0)
                    {
                        hjVal.Add(dt.Rows[0][2].ToString());
                        for (int j = 3; j < dt.Columns.Count; j++)
                        {
                            double val = 0;
                            double.TryParse(dt.Rows[0][j].ToString(), out val);
                            rowIndex++;
                            cells[rowIndex, 2 + izrq - 1].SetStyle(styleNum, true);
                            cells[rowIndex, 2 + izrq - 1].PutValue(val);

                            cells[rowIndex, 51].SetStyle(styleNum, true);
                            cells[rowIndex, 51].PutValue(val);

                        }
                    }
                    else
                    {
                        hjVal.Add(0.ToString());
                        for (int j = 3; j < dt.Columns.Count; j++)
                        {
                            double val = 0;
                            rowIndex++;
                            cells[rowIndex, 2 + izrq - 1].SetStyle(styleNum, true);
                            cells[rowIndex, 2 + izrq - 1].PutValue(val);

                            cells[rowIndex, 51].SetStyle(styleNum, true);
                            cells[rowIndex, 51].PutValue(val);
                        }
                    }

                }

                //cells[4, 51].SetStyle(styleNum, true);
                //cells[4, 51].PutValue(double.Parse(hjVal[0]));

                //cells[5, 51].SetStyle(styleNum, true);
                //cells[5, 51].PutValue(double.Parse(hjVal[1]));

                //cells[10, 51].SetStyle(styleNum, true);
                //cells[10, 51].PutValue(double.Parse(hjVal[2]));

                //cells[13, 51].SetStyle(styleNum, true);
                //cells[13, 51].PutValue(double.Parse(hjVal[3]));

                //cells[16, 51].SetStyle(styleNum, true);
                //cells[16, 51].PutValue(double.Parse(hjVal[4]));

                //cells[19, 51].SetStyle(styleNum, true);
                //cells[19, 51].PutValue(double.Parse(hjVal[5]));

                //cells[24, 51].SetStyle(styleNum, true);
                //cells[24, 51].PutValue(double.Parse(hjVal[6]));

                //cells[27, 51].SetStyle(styleNum, true);
                //cells[27, 51].PutValue(double.Parse(hjVal[7]));

                //cells[30, 51].SetStyle(styleNum, true);
                //cells[30, 51].PutValue(double.Parse(hjVal[8]));

                //cells[33, 51].SetStyle(styleNum, true);
                //cells[33, 51].PutValue(double.Parse(hjVal[9]));


                wk.Save(excelReportFilename);
                if (isVisible)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(excelReportFilename);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void exportExcelZRQ(DataTable dt, string destExcelDir, string tableName, string XZQDM, string XZQMC, Boolean isVisible = false, int iScale = 1)
        {
            try
            {
                string tmplateFile = System.Environment.CurrentDirectory + @"\template\" + tableName + ".xlsx";
                string dateStr;
                if (isVisible)
                    dateStr = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                else
                    dateStr = "";

                string excelReportFilename = destExcelDir + @"\(" + XZQDM + ")" + XZQMC + (tableName.IndexOf("(") == -1 ? tableName : tableName.Substring(0, tableName.IndexOf("("))) + "" + dateStr + ".xlsx";
                if (File.Exists(excelReportFilename)) File.Delete(excelReportFilename);
                System.IO.File.Copy(tmplateFile, excelReportFilename);

                if (dt.Rows.Count < 2)
                {
                    MessageBox.Show("" + tableName + "表统计失败，请完善数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Aspose.Cells.Workbook wk = new Aspose.Cells.Workbook(excelReportFilename);
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
                styleTxt.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                styleTxt.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
                styleTxt.Number = 49; //@

                string sql = "select zrqdm,zrqmc from SYS_QGGXSSZRQHSZ where xzqdm='" + XZQDM + "'";
                DataTable dtZrq = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
                if (dtZrq.Rows.Count == 0)
                {
                    MessageBox.Show("没有找到" + XZQMC + "的自然区数据，请导入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string zrqdm = dtZrq.Rows[0][0].ToString();
                int izrq = 0;
                int.TryParse(zrqdm, out izrq);

                int rowIndex = 4;
                int colIndex = izrq - 1;

                //cells[1, 1].PutValue(XZQDM);
                //cells[1, 3].PutValue(XZQMC);
                //cells[1, 4].PutValue("单位：");
                //cells[1, 5].PutValue(DateTime.Now.ToLongDateString());
                //if (iScale == 15) cells[1, 5].PutValue("亩");
                //else
                //    cells[1, 5].PutValue("公顷");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 52; j++)
                    {
                        cells[rowIndex + i, j].SetStyle(styleTxt);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k < 3)
                            {
                                cells[rowIndex + i, k].SetStyle(styleTxt);
                                cells[rowIndex + i, k].PutValue(dt.Rows[i][k].ToString());
                            }
                            else
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                                double mj = 0;
                                cells[rowIndex + i, colIndex + k].PutValue(mj);
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dt.Columns.Count; k++)
                        {
                            if (k < 3)
                            {
                                cells[rowIndex + i, k].SetStyle(styleTxt);
                                cells[rowIndex + i, k].PutValue(dt.Rows[i][k].ToString());
                                if (k == 2)
                                {
                                    cells[rowIndex + i, k].SetStyle(styleNum,true);
                                    cells[rowIndex + i, k].PutValue(double.Parse(dt.Rows[i][k].ToString()));
                                }
                            }
                            else
                            {
                                cells[rowIndex + i, colIndex + k].SetStyle(styleNum, true);
                                double mj = 0;
                                double.TryParse(dt.Rows[i][k].ToString(), out mj);
                                cells[rowIndex + i, colIndex + k].PutValue(mj);
                            }
                        }
                    }
                }


                wk.Save(excelReportFilename);
                if (isVisible)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(excelReportFilename);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void createTable(DataTable dataTable, string groupingColumn, string nameColumn, string dataColumn, ref DataTable resultTable)
        {
            //取出不重复的需要转换的列的数据
            DataTable distinct_GroupingColumn = dataTable.DefaultView.ToTable(true, groupingColumn);
            //取出不重复的名称列
            DataTable dictinct_NameColumn = dataTable.DefaultView.ToTable(true, nameColumn);
            //List<string> lstID = (from d in dictinct_NameColumn.AsEnumerable() select d.Field<string>("zldwdm").ToString().Substring(0, 9)).ToList();
            var lstID = dictinct_NameColumn.AsEnumerable().GroupBy(a => a.Field<string>("zldwdm").Substring(0, 9)).ToList();
            //构建新表group d by d.m
            //DataTable table = new DataTable();
            #region 构建新表的列
            //将名称列添加进新表
            DataColumn newColumn = new DataColumn();
            newColumn.ColumnName = nameColumn;

            #endregion
            #region 向新表中添加数据
            DataRow newRow;
            foreach (var item in lstID)
            {
                //添加一个新行
                newRow = resultTable.NewRow();
                //为此新行添加第一个行数据
                //newRow[nameColumn] = item.Key;
                //为此新行添加列数据
                double hj = 0;
                double xianHJ = 0;
                foreach (DataRow r in distinct_GroupingColumn.Rows)
                {
                    if (!resultTable.Columns.Contains(r[groupingColumn].ToString()))
                    {
                        UpdateStatus("" + groupingColumn + "字段存在错误值，请完善数据。");
                        resultTable.Rows.Clear();
                        return;
                    }
                    object val = dataTable.Compute("sum(tbdlmj)", nameColumn + " like '" + item.Key.ToString() + "%' and " + groupingColumn + " ='" + r[groupingColumn].ToString() + "'");
                    double sum = 0;
                    if (!string.IsNullOrWhiteSpace(val.ToString()))
                        sum = (double)val;
                    //double.TryParse(val.ToString(), out sum);
                    sum = Math.Round(sum / 10000, 6);
                    hj += sum;
                    if (sum != 0)
                    {
                        //将数据与新建表进行连合
                        newRow[r[groupingColumn].ToString()] = sum;
                        newRow["dm"] = item.Key;
                        newRow["mc"] = dmmc[item.Key];
                        resultTable.Rows[0][r[groupingColumn].ToString()] = sum + double.Parse(resultTable.Rows[0][r[groupingColumn].ToString()].ToString());
                    }
                    xianHJ += double.Parse(resultTable.Rows[0][r[groupingColumn].ToString()].ToString());
                }
                newRow["hj"] = hj;
                resultTable.Rows.Add(newRow);
                resultTable.Rows[0]["hj"] = xianHJ;
            }
            #endregion

            //return table;
        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(btnPath.Text))
            {
                MessageBox.Show("请选择输出路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dtList.Count != 10)
            {
                MessageBox.Show("请先初始化基础库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (radioGroup1.SelectedIndex == 0)
            {
                exportExcelHZ(dtList, btnPath.Text, "耕地资源质量分类面积汇总表", xzqdm, xzqmc, true);
                exportExcelZRQ(dtList[0], btnPath.Text, "耕地资源质量分类面积汇总表-自然区", xzqdm, xzqmc, false);
                exportExcel(dtList[1], btnPath.Text, "耕地资源质量分类面积汇总表-坡度", xzqdm, xzqmc, true, false);
                exportExcel(dtList[2], btnPath.Text, "耕地资源质量分类面积汇总表-土层厚度", xzqdm, xzqmc, true, false);
                exportExcel(dtList[3], btnPath.Text, "耕地资源质量分类面积汇总表-土壤质地", xzqdm, xzqmc, true, false);
                exportExcel(dtList[4], btnPath.Text, "耕地资源质量分类面积汇总表-土壤有机质含量", xzqdm, xzqmc, true, false);
                exportExcel(dtList[5], btnPath.Text, "耕地资源质量分类面积汇总表-土壤pH值", xzqdm, xzqmc, true, false);
                exportExcel(dtList[6], btnPath.Text, "耕地资源质量分类面积汇总表-生物多样性", xzqdm, xzqmc, true, false);
                exportExcel(dtList[7], btnPath.Text, "耕地资源质量分类面积汇总表-土壤重金属污染状况", xzqdm, xzqmc, true, false);
                exportExcel(dtList[8], btnPath.Text, "耕地资源质量分类面积汇总表-熟制", xzqdm, xzqmc, true, false);
                exportExcel(dtList[9], btnPath.Text, "耕地资源质量分类面积汇总表-耕地二级地类", xzqdm, xzqmc,true, false);

            }
            if (radioGroup1.SelectedIndex == 1)
            {
                exportExcelHZ(dtListKc, btnPath.Text, "耕地资源质量分类面积汇总表(JKHF)", xzqdm, xzqmc, false);
                exportExcelZRQ(dtListKc[0], btnPath.Text, "耕地资源质量分类面积汇总表-自然区", xzqdm, xzqmc, false);
                exportExcel(dtListKc[1], btnPath.Text, "耕地资源质量分类面积汇总表-坡度", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[2], btnPath.Text, "耕地资源质量分类面积汇总表-土层厚度", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[3], btnPath.Text, "耕地资源质量分类面积汇总表-土壤质地", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[4], btnPath.Text, "耕地资源质量分类面积汇总表-土壤有机质含量", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[5], btnPath.Text, "耕地资源质量分类面积汇总表-土壤pH值", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[6], btnPath.Text, "耕地资源质量分类面积汇总表-生物多样性", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[7], btnPath.Text, "耕地资源质量分类面积汇总表-土壤重金属污染状况", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[8], btnPath.Text, "耕地资源质量分类面积汇总表-熟制", xzqdm, xzqmc, false, false);
                exportExcel(dtListKc[9], btnPath.Text, "耕地资源质量分类面积汇总表-耕地二级地类(JKHF)", xzqdm, xzqmc, false, false);
            }
        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButton1.Checked)
            {
                chk1.CheckState = CheckState.Checked;
                chk2.CheckState = CheckState.Checked;
                chk3.CheckState = CheckState.Checked;
                chk4.CheckState = CheckState.Checked;
                chk5.CheckState = CheckState.Checked;
                chk6.CheckState = CheckState.Checked;
                chk7.CheckState = CheckState.Checked;
                chk8.CheckState = CheckState.Checked;
                chk9.CheckState = CheckState.Checked;
                chk10.CheckState = CheckState.Checked;
                chk11.CheckState = CheckState.Checked;
            }
            else
            {
                chk1.CheckState = CheckState.Unchecked;
                chk2.CheckState = CheckState.Unchecked;
                chk3.CheckState = CheckState.Unchecked;
                chk4.CheckState = CheckState.Unchecked;
                chk5.CheckState = CheckState.Unchecked;
                chk6.CheckState = CheckState.Unchecked;
                chk7.CheckState = CheckState.Unchecked;
                chk8.CheckState = CheckState.Unchecked;
                chk9.CheckState = CheckState.Unchecked;
                chk10.CheckState = CheckState.Unchecked;
                chk11.CheckState = CheckState.Unchecked;
            }
        }
    }
}
