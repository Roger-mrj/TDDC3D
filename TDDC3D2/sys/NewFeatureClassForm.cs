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

namespace TDDC3D.sys
{
    public partial class NewFeatureClassForm : Form
    {
        public NewFeatureClassForm()
        {
            InitializeComponent();
        }

        public IWorkspace currWS = null;
        public IFeatureDataset currDS = null;

        public string retFCName = "";
        public string retFCAliasName = "";

        public int retJHLX
        {
            get { return this.rgGeoType.SelectedIndex; }
        }

        private void NewFeatureClassForm_Load(object sender, EventArgs e)
        {
            IGeoDataset pGeods = currDS as IGeoDataset;
            this.txtSP.Text = pGeods.SpatialReference.Name;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (currDS == null)
                return;
            if (this.txtClassName.Text.Trim() == "")
                return;
            string aliasName=txtAlias.Text.Trim();
            if (aliasName == "")
            {
                aliasName = this.txtClassName.Text.Trim();
            }

            try
            {
                esriGeometryType pGeometryType;
                switch (this.rgGeoType.SelectedIndex)
                {
                    case 0:
                        pGeometryType = esriGeometryType.esriGeometryPoint;
                        break;
                    case 1:
                        pGeometryType = esriGeometryType.esriGeometryPolyline;
                        break;
                    case 2:
                        pGeometryType = esriGeometryType.esriGeometryPolygon;
                        break;
                    default:
                        pGeometryType = esriGeometryType.esriGeometryPolygon;
                        break;
                }

                //实倒化字段集合对象
                IFields pFields = new FieldsClass();
                IFieldsEdit tFieldsEdit = (IFieldsEdit)pFields;
 
                 //创建几何对象字段定义
                IGeometryDef tGeometryDef = new GeometryDefClass();
                IGeometryDefEdit tGeometryDefEdit = tGeometryDef as IGeometryDefEdit;
               //指定几何对象字段属性值
                tGeometryDefEdit.GeometryType_2 = pGeometryType;
                tGeometryDefEdit.GridCount_2 = 1;
                tGeometryDefEdit.set_GridSize(0, 1000);
                tGeometryDefEdit.SpatialReference_2 = (this.currDS as IGeoDataset).SpatialReference;

                //创建OID字段
                IField fieldOID = new FieldClass();
                IFieldEdit fieldEditOID = fieldOID as IFieldEdit;
                fieldEditOID.Name_2 = "OBJECTID";
                fieldEditOID.AliasName_2 = "OBJECTID";
                fieldEditOID.Type_2 = esriFieldType.esriFieldTypeOID;
                tFieldsEdit.AddField(fieldOID);

                //创建几何字段
                IField fieldShape = new FieldClass();
                IFieldEdit fieldEditShape = fieldShape as IFieldEdit;
                fieldEditShape.Name_2 = "SHAPE";
                fieldEditShape.AliasName_2 = "SHAPE";
                fieldEditShape.Type_2 = esriFieldType.esriFieldTypeGeometry;
                fieldEditShape.GeometryDef_2 = tGeometryDef;
                tFieldsEdit.AddField(fieldShape);

                IFeatureClass tFeatureClass = this.currDS.CreateFeatureClass(this.txtClassName.Text.Trim(),
                    pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");

                IClassSchemaEdit2 pCSEdit = tFeatureClass as IClassSchemaEdit2;
                pCSEdit.AlterAliasName(this.txtAlias.Text.Trim());


                this.retFCAliasName = this.txtAlias.Text.Trim();
                this.retFCName = this.txtClassName.Text.ToUpper().Trim();
                MessageBox.Show("创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
