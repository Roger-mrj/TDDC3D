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
using ESRI.ArcGIS.Geometry;
namespace TDDC3D.edit
{
    public partial class Xzdw2WhereOptForm : Form
    {
        public Xzdw2WhereOptForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private void refreshDLData()
        {
            DataTable dtSort = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select DLBM,SORT,DLMC from SYS_XZDWHB_DLSORT order by sort", "sort");
            this.gridControl1.DataSource = dtSort;
        }
        private void Xzdw2WhereOptForm_Load(object sender, EventArgs e)
        {

            refreshDLData();

            List<string> lst=new List<string>();
            lst.Add("XZQ");
            lst.Add("CJDCQ");

            this.cmbLayers.Properties.Items.Clear();

            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap, esriGeometryType.esriGeometryPolygon);

            //this.cmbOneWayDirection.SelectedIndex = 0;
        }

      

        /// <summary>
        /// 返回要处理的要素类名称
        /// </summary>
        public string RetClassName
        {
            get {
                return RCIS.Utility.OtherHelper.GetLeftName(this.cmbLayers.Text.Trim()); ; 
            }
            
        }


       

        /// <summary>
        /// 是否删除线装地物
        /// </summary>
        public bool IsDelXzdw
        {
            get { return this.chkDelXzdw.Checked; }
        }
        
        /// <summary>
        /// 是否处理图形压盖
        /// </summary>
        public bool IsDoTopInter
        {
            get { return this.chkDoTopInter.Checked; }
        }
        
        /// <summary>
        /// 是否合并
        /// </summary>
        public bool IsUnion
        {
            get { return this.chkAutoUnion.Checked; }
        }




        private bool isExtentSnap = false; //预删除破碎图斑
        public bool IsExtentSnap
        {
            get { return this.chkAutoExtSnap.Checked; }
        }
        public double dExtentLen
        {
            get
            {
                double dmj = 0;
                double.TryParse(this.txtExtendLen.Text, out dmj);
                return dmj; 
            }
        }

  

        ///// <summary>
        ///// //最小碎图斑面积
        ///// </summary>
        //public double MinTbMj
        //{
        //    get {
        //        double dMinTbMj = 0;
        //        double.TryParse(this.txtMinMj.Text, out dMinTbMj);    
        //        return dMinTbMj; 
        //    }
        //}

        ///// <summary>
        ///// 是否单向缓冲
        ///// </summary>
        //public bool IsOneWay
        //{
        //    get { 
        //        if (this.radioGroupOneTwoWay.SelectedIndex==0)
        //            return false;
        //        else 
        //            return true;
        //    }
        //}
        
        ///// <summary>
        ///// 正向还是负向
        ///// </summary>
        //public int OneWaydirect
        //{
        //    get {
        //        if (this.cmbOneWayDirection.SelectedIndex == 0)
        //            return 1;
        //        else return -1;
        //    }
        //}


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "")
            {
                MessageBox.Show("请选择某个图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
                 

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0) return;
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            string dlbm = this.gridView1.GetRowCellValue(selectedHandle, "DLBM").ToString();
            int sort = 0;
            int.TryParse(this.gridView1.GetRowCellValue(selectedHandle, "SORT").ToString(), out sort);

            int oldsort = sort - 1;
            string sql = "update SYS_XZDWHB_DLSORT set sort=sort+1 where sort=" + oldsort;
            RCIS.Database.LS_SetupMDBHelper.ExecuteSQLNonquery(sql);

            sql = "update SYS_XZDWHB_DLSORT set sort=sort-1 where dlbm='" + dlbm + "' ";
            RCIS.Database.LS_SetupMDBHelper.ExecuteSQLNonquery(sql);

            refreshDLData();

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //下移
            if (this.gridView1.SelectedRowsCount == 0) return;
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            string dlbm = this.gridView1.GetRowCellValue(selectedHandle, "DLBM").ToString();
            int sort = 0;
            int.TryParse(this.gridView1.GetRowCellValue(selectedHandle, "SORT").ToString(), out sort);

            int oldsort = sort +1;
            string sql = "update SYS_XZDWHB_DLSORT set sort=sort-1 where sort=" + oldsort;
            RCIS.Database.LS_SetupMDBHelper.ExecuteSQLNonquery(sql);

            sql = "update SYS_XZDWHB_DLSORT set sort=sort+1 where dlbm='" + dlbm + "' ";
            RCIS.Database.LS_SetupMDBHelper.ExecuteSQLNonquery(sql);

            refreshDLData();
        }

        private void radioGroupOneTwoWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.radioGroupOneTwoWay.SelectedIndex==0)
            //{
            //    this.cmbOneWayDirection.Enabled = false;
            //}
            //else if (this.radioGroupOneTwoWay.SelectedIndex==1)
            //{
            //    this.cmbOneWayDirection.Enabled = true;
            //}
        }
    }
}
