using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using System;
using System.Data;
using System.Windows.Forms;

namespace TDDC3D.analysis
{
    public partial class TdlyjgfxForm : Form
    {
        public TdlyjgfxForm()
        {
            InitializeComponent();

           


        }
        private string getXzdm()
        {
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
            return xzdm;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private DataTable CreateChartData(DataTable table1)
        {
           
            DataTable table = new DataTable("Table1");
            table.Columns.Add("Name", typeof(String));
            table.Columns.Add("Value", typeof(Double));
            DataRow aRow=table1.Rows[0];

            for (int i = 0; i < table1.Columns.Count; i++)
            {
                string colName = table1.Columns[i].ColumnName.ToString();
                double val = 0;
                double.TryParse(aRow[i].ToString(), out val);
                var array = new object[] { colName, val };
                table.Rows.Add(array);
            }
            return table;
        }

        /// <summary>
        /// 根据数据创建一个饼状图
        /// </summary>
        /// <returns></returns>
        private void BuilderDevChart()
        {
            
        }

        private void RefreshChart2(string xzdm)
        {
            

            chartControl1.Series.Clear();
            string sql = "SELECT D01 as 耕地, D02 as 园地,  D03 as 林地,  D04 as 草地,  D05 as 商服用地,"
            +"D06 as 工矿仓储用地,  D07 as 住宅用地,  D08 as 公共管理与公共服务用地,  D09 as 特殊用地,"
            +"D10 as 交通运输用地,  D11 as 水域及水利设施用地,  D12 as 其他土地 FROM HZ_ZL_BZ where ZLDWDM='" + xzdm + "' ";
            DataTable table = RCIS.Database.LS_ResultMDBHelper.GetDataTable(sql, "tmp");
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("当前数据必须通过成果报表中的统计功能得到！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DevExpress.XtraCharts.Series _pieSeries = new DevExpress.XtraCharts.Series("利用结构分析", DevExpress.XtraCharts.ViewType.Pie);
            _pieSeries.ValueDataMembers[0] = "Value";
            _pieSeries.ArgumentDataMember = "Name";
            _pieSeries.DataSource = CreateChartData(table);
            chartControl1.Series.Add(_pieSeries);
            //_pieSeries.SetPiePercentage();
            _pieSeries.LegendPointOptions.PointView =DevExpress.XtraCharts.PointView.Argument;


            sql = "select TotalAreaG as 国有,TotalAreaJ as 集体 from HZ_QS_BZ where ZLDWDM='" + xzdm + "' ";
            this.chartControl2.Series.Clear();
            DataTable table2 = RCIS.Database.LS_ResultMDBHelper.GetDataTable(sql, "tmp");
            if (table.Rows.Count == 0)
            {
                MessageBox.Show("当前数据必须通过成果报表中的统计功能得到！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DevExpress.XtraCharts.Series _pieSeries2 = new DevExpress.XtraCharts.Series("权属结构分析", DevExpress.XtraCharts.ViewType.Pie);
            _pieSeries2.ValueDataMembers[0] = "Value";
            _pieSeries2.ArgumentDataMember = "Name";
            _pieSeries2.DataSource = CreateChartData(table2);
            chartControl2.Series.Add(_pieSeries2);
            //_pieSeries.SetPiePercentage();
            _pieSeries2.LegendPointOptions.PointView = DevExpress.XtraCharts.PointView.Argument;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "JPG文件|*.jpg";
            dlg.FileName = "图表";
            if (dlg.ShowDialog() != DialogResult.Cancel)
            {
                try
                {
                    if (this.xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        this.chartControl1.ExportToImage(dlg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else if (this.xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        this.chartControl2.ExportToImage(dlg.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    
                    MessageBox.Show("导出完毕！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, "");
                }

            }
        }

        private void chartControl1_Click(object sender, EventArgs e)
        {

        }

        private void TdlyjgfxForm_Load(object sender, EventArgs e)
        {
            string xzdm=this.getXzdm();
            RefreshChart2(xzdm);
        }
    }
}
