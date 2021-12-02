using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
namespace TDDC3D.edit
{
    public partial class TBHBOptionForm : Form
    {
        public TBHBOptionForm()
        {
            InitializeComponent();
        }

        public IMapControl3 mapControl = null;


        public List<IFeature> inFeatures = new System.Collections.Generic.List<IFeature>();
        public IFeature outTB = null;
        Dictionary<int, IFeature> dicFeas = new System.Collections.Generic.Dictionary<int , IFeature>();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int[] idx=this.gridView1.GetSelectedRows();
            if (idx.Length == 0)
            {
                MessageBox.Show("请首先选择要继承属性的图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string oid=this.gridView1.GetRowCellValue(idx[0], "OID").ToString();
            int iOid = 0;
            int.TryParse(oid, out iOid);
            outTB = dicFeas[iOid];

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void ZDHBOptionForm_Load(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "OID";
            dt.Columns.Add(dc);
                
                
            dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "BSM";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "TBBH";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "DLBM";
            dt.Columns.Add(dc);
            dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "DLMC";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "QSDWMC";
            dt.Columns.Add(dc);
            dc = new DataColumn();
            dc.DataType = typeof(String);
            dc.ColumnName = "ZLDWMC";
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.DataType = typeof(double);
            dc.ColumnName = "TBMJ";
            dt.Columns.Add(dc);

            
            foreach (IFeature aFea in inFeatures)
            {
                string bsm = FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                if (!dicFeas.ContainsKey(aFea.OID))
                {
                    dicFeas.Add(aFea.OID, aFea);
                }

                DataRow aRow = dt.NewRow();
                aRow["OID"] = aFea.OID;
                aRow["BSM"] = bsm;
                aRow["TBBH"] = FeatureHelper.GetFeatureStringValue(aFea, "TBBH");
                aRow["DLBM"] = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                aRow["DLMC"] = FeatureHelper.GetFeatureStringValue(aFea, "DLMC");
                aRow["QSDWMC"] = FeatureHelper.GetFeatureStringValue(aFea, "QSDWMC");
                aRow["ZLDWMC"] = FeatureHelper.GetFeatureStringValue(aFea, "ZLDWMC");
                aRow["TBMJ"] = FeatureHelper.GetFeatureDoubleValue(aFea, "TBMJ");
                dt.Rows.Add(aRow);


            }

            this.gridControl1.DataSource = dt;


        }
        
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
                return;
            
            //定位 闪烁
            int[] idx = this.gridView1.GetSelectedRows();
            string oid = this.gridView1.GetRowCellValue(idx[0], "OID").ToString();
            int iOid = 0;
            int.TryParse(oid, out iOid);
            IFeature aFea = dicFeas[iOid];
            if (aFea != null)
            {
                IGeometry geom = aFea.ShapeCopy;
                this.mapControl.FlashShape(geom, 3, 300, null);

            }
        }
    }
}
