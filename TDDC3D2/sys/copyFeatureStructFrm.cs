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

using RCIS.Utility;
using RCIS.GISCommon;

namespace TDDC3D.sys
{
    public partial class copyFeatureStructFrm : Form
    {
        public copyFeatureStructFrm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        public IWorkspace currWS = null;
        public string  srcFcName = "";

        private void copyFeatureStructFrm_Load(object sender, EventArgs e)
        {
            //加载所有要素数据集
            this.cmbDatasets.Properties.Items.Clear();

            IEnumDataset pEnumDs = this.currWS.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pDS = pEnumDs.Next();
            while (pDS != null)
            {
                this.cmbDatasets.Properties.Items.Add(pDS.Name.ToUpper());
                pDS = pEnumDs.Next();
            }
            if (this.cmbDatasets.Properties.Items.Count > 0)
            {
                this.cmbDatasets.SelectedIndex = 0;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.srcFcName == "")
                return;
            if (this.cmbDatasets.Text.Trim() == "")
                return;
            if (this.txtClassName.Text.Trim() == "")
                return;
            string aliasName = txtAlias.Text.Trim();
            if (aliasName == "")
            {
                aliasName = this.txtClassName.Text.Trim();
            }
            string destClassName = this.txtClassName.Text.Trim();

            try
            {
                //复制要素类结构            
                IWorkspace2 currWs2 = this.currWS as IWorkspace2;
                if (currWs2.get_NameExists(esriDatasetType.esriDTFeatureClass, destClassName))
                {
                    MessageBox.Show("该目标图层已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                IFeatureDataset pFeaDS = (this.currWS as IFeatureWorkspace).OpenFeatureDataset(this.cmbDatasets.Text);
                IFeatureClass pSrcFC = (this.currWS as IFeatureWorkspace).OpenFeatureClass(srcFcName);

                //实倒化字段集合对象
                IFields pFields = new FieldsClass();
                IFieldsEdit tFieldsEdit = (IFieldsEdit)pFields;

                //创建几何对象字段定义
                IGeometryDef tGeometryDef = new GeometryDefClass();
                IGeometryDefEdit tGeometryDefEdit = tGeometryDef as IGeometryDefEdit;
                //指定几何对象字段属性值

                tGeometryDefEdit.GeometryType_2 = pSrcFC.ShapeType;
                tGeometryDefEdit.GridCount_2 = 1;
                tGeometryDefEdit.set_GridSize(0, 1000);
                tGeometryDefEdit.SpatialReference_2 = (pFeaDS as IGeoDataset).SpatialReference;

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

                //添加其他字段
                for (int i = 0; i < pSrcFC.Fields.FieldCount; i++)
                {
                    IField aFld = pSrcFC.Fields.get_Field(i);
                    if (aFld.Type == esriFieldType.esriFieldTypeGUID || aFld.Type == esriFieldType.esriFieldTypeOID || aFld.Type == esriFieldType.esriFieldTypeOID
                        || aFld.Type == esriFieldType.esriFieldTypeGeometry
                        )
                    {
                        continue;
                    }
                    IField newFld = new FieldClass();
                    IFieldEdit newFldEdt = newFld as IFieldEdit;
                    newFldEdt.Name_2 = aFld.Name;
                    newFldEdt.AliasName_2 = aFld.AliasName;
                    newFldEdt.Type_2 = aFld.Type;
                    newFldEdt.Length_2 = aFld.Length;
                    newFldEdt.Precision_2 = aFld.Precision;
                    newFldEdt.Scale_2 = aFld.Scale;

                    tFieldsEdit.AddField(newFld);

                }
                IFeatureClass tFeatureClass = pFeaDS.CreateFeatureClass(destClassName,
                    pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                IClassSchemaEdit2 pCSEdit = tFeatureClass as IClassSchemaEdit2;
                pCSEdit.AlterAliasName(this.txtAlias.Text.Trim());

                MessageBox.Show("创建成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;

                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }
    }
}
