using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms;
using RCIS.Global;
using ESRI.ArcGIS.Controls;

using ESRI.ArcGIS.Geodatabase;
using System.Collections;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
namespace RCIS.MapTool
{
    public partial class CutPolygonIntersectOptForm : Form
    {
        public CutPolygonIntersectOptForm()
        {
            InitializeComponent();
        }




        public List<IFeature> inFeatures = new List<IFeature>();

        public int cutOption = 0; //0 切除，1 为合并
        public IFeature outFeature = null;



        Dictionary<int, IFeature> dicFeas = new System.Collections.Generic.Dictionary<int, IFeature>();
        

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.gridControl1.Enabled = false;
            }
            else
            {
                this.gridControl1.Enabled = true;
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.cutOption = 0;

            }
            else
            {
                this.cutOption = 1; 
                int[] idx = this.gridView1.GetSelectedRows();
                if (idx.Length == 0)
                {
                    MessageBox.Show("请首先选择一个要保留图形的要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int[] selIdx = this.gridView1.GetSelectedRows();
                if (selIdx.Length>0)
                {
                    outFeature = dicFeas[selIdx[0]];
                }
            }
            

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void CutPolygonIntersectOptForm_Load(object sender, EventArgs e)
        {
            this.gridControl1.Enabled = false;

            DataTable m_dataTable = new DataTable();
            IFeature aFeature1 = inFeatures[0] as IFeature;
            IFeature aFeature2 = inFeatures[1];
            IFeatureClass pFC1 = aFeature1.Class as IFeatureClass;
            IFeatureClass pFC2 = aFeature2.Class as IFeatureClass;
            //找共同字段
            List<string> lstCommonField = new List<string>();

            for (int fi = 0; fi < pFC1.Fields.FieldCount; fi++)
            {
                IField curFld = pFC1.Fields.get_Field(fi);
                string fldName = curFld.Name.ToUpper();
                //找不到该字段
                if (pFC2.Fields.FindField(fldName.ToUpper())==-1)
                    continue;
                if (fldName.Equals(pFC1.ShapeFieldName.ToUpper()))
                    continue;
                

                DataColumn dc = new DataColumn(curFld.Name);
                dc.Caption = curFld.AliasName;
                if (curFld.Type == esriFieldType.esriFieldTypeInteger
                    || curFld.Type == esriFieldType.esriFieldTypeSmallInteger
                    || curFld.Type == esriFieldType.esriFieldTypeOID)
                {
                    dc.DataType = typeof(Int32);
                }
                else if (curFld.Type == esriFieldType.esriFieldTypeSingle
                    || curFld.Type == esriFieldType.esriFieldTypeDouble)
                {
                    dc.DataType = typeof(Double);
                }
                else dc.DataType = typeof(string);
                m_dataTable.Columns.Add(dc);

                lstCommonField.Add(fldName);

            }
            //提取值
            int iCount = 0;
            dicFeas.Add(iCount++, aFeature1);
            dicFeas.Add(iCount++, aFeature2);

            foreach (IFeature aFea in this.inFeatures)
            {
                DataRow curRow = m_dataTable.NewRow();
                foreach (string aFld in lstCommonField)
                {
                    if (m_dataTable.Columns[aFld].DataType == typeof(string))
                    {
                        curRow[aFld] = FeatureHelper.GetFeatureStringValue(aFea, aFld);
                    }
                    else if (m_dataTable.Columns[aFld].DataType == typeof(int))
                    {
                        int iValue = 9;
                        int.TryParse(FeatureHelper.GetFeatureStringValue(aFea, aFld),out iValue);
                        curRow[aFld] = iValue;
                    }
                    else if (m_dataTable.Columns[aFld].DataType == typeof(double))
                    {
                        curRow[aFld] = FeatureHelper.GetFeatureDoubleValue(aFea, aFld);
                    }

                }
                m_dataTable.Rows.Add(curRow);
            }

            

            this.gridControl1.DataSource = m_dataTable;
        }
        public IMapControl3 mapControl = null;
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
                return;

            //定位 闪烁
            int[] idx = this.gridView1.GetSelectedRows();
            if (idx.Length == 0) return;
            
            IFeature aFea = dicFeas[idx[0]];
            if (aFea != null)
            {
                IGeometry geom = aFea.ShapeCopy;
                this.mapControl.FlashShape(geom, 3, 300, null);

            }
        }
    }
}
