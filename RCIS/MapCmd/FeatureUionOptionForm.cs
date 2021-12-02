using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Global;

using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.esriSystem;

using RCIS.Utility;
namespace RCIS.MapTool
{
    public partial class FeatureUionOptionForm : Form
    {
        public FeatureUionOptionForm()
        {
            InitializeComponent();
        }
        public IMapControl3 mapControl = null;
        public ArrayList inFeatures = new ArrayList();
        public IFeature outFeature = null;
        Dictionary<int, IFeature> dicFeas = new System.Collections.Generic.Dictionary<int, IFeature>();

        IFeatureClass pFC = null;
        private void FeatureUionOptionForm_Load(object sender, EventArgs e)
        {
            DataTable m_dataTable = new DataTable();
            IFeature aFeature = inFeatures[0] as IFeature;
            pFC = aFeature.Class as IFeatureClass;
            for (int fi = 0; fi < pFC.Fields.FieldCount; fi++)
            {
                IField curFld = pFC.Fields.get_Field(fi);
                string fldName = curFld.Name.ToUpper();
                DataColumn dc = new DataColumn(curFld.Name);

                if (fldName.Equals(pFC.ShapeFieldName.ToUpper()))
                {
                    dc.Caption = "几何图形";
                }
                else
                    if (pFC.LengthField != null && fldName.Equals(pFC.LengthField.Name.ToUpper()))
                    {
                        dc.Caption = "计算长度";
                    }
                    else
                        if (pFC.AreaField != null && fldName.Equals(pFC.AreaField.Name.ToUpper()))
                        {
                            dc.Caption = "计算面积";
                        }
                        else
                        {
                            dc.Caption = curFld.AliasName;
                        }
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

                

            }
            //提取值
            string oidFldName = pFC.OIDFieldName;
            foreach (IFeature aFea in inFeatures)
            {
                string bsm = FeatureHelper.GetFeatureStringValue(aFea, oidFldName);
                if (!dicFeas.ContainsKey(aFea.OID))
                {
                    dicFeas.Add(aFea.OID, aFea);
                }


                DataRow curRow = m_dataTable.NewRow();
                for (int fi = 0; fi < pFC.Fields.FieldCount; fi++)
                {
                    IField curFld = pFC.Fields.get_Field(fi);
                    if (curFld.Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        IGeometry curGeom = aFea.ShapeCopy; // qFea.Shape;
                        if (curGeom != null && !curGeom.IsEmpty)
                        {
                            curRow[fi] = "图形对象";
                        }
                        else
                        {
                            curRow[fi] = "";
                        }
                    }
                    else
                    {
                        curRow[fi] = aFea.get_Value(fi); //qFea.get_Value(fi);
                    }
                }

                m_dataTable.Rows.Add(curRow);
            }
            this.gridControl1.DataSource = m_dataTable;
            

           


        }

       

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int[] idx = this.gridView1.GetSelectedRows();
            if (idx.Length == 0)
            {
                MessageBox.Show("请首先选择要继承属性的图斑！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string oidFldname = pFC.OIDFieldName;
            string oid = this.gridView1.GetRowCellValue(idx[0], oidFldname).ToString();
            int iOid = 0;
            int.TryParse(oid, out iOid);
            outFeature = dicFeas[iOid];

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
                return;
            //定位 闪烁
            int[] idx = this.gridView1.GetSelectedRows();
            string oidFldname = pFC.OIDFieldName;
            string oid = this.gridView1.GetRowCellValue(idx[0], oidFldname).ToString();
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
