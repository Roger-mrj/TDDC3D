using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.Controls
{
    public partial class CalSelFeatureForm : Form
    {
        public CalSelFeatureForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        public IMap currMap = null;

        private void CalSelFeatureForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);

        }
        IFeatureLayer currLayer = null;
        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //找到对应字段
            string className = OtherHelper.GetLeftName(this.cmbLayers.Text.Trim());
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            IFeatureClass pFeaClass = currLayer.FeatureClass;

            this.cmbTjFields.Properties.Items.Clear();
           
            for (int i = 0; i < pFeaClass.Fields.FieldCount; i++)
            {
                IField aFld = pFeaClass.Fields.get_Field(i);
                if (aFld.Type == esriFieldType.esriFieldTypeSingle  || aFld.Type==esriFieldType.esriFieldTypeDouble)
                {
                    this.cmbTjFields.Properties.Items.Add(aFld.Name.ToUpper() + "|" + aFld.AliasName);
                    
                }
            }

          
        }

        private void cmbTjFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.currLayer==null )return;
            if (this.cmbTjFields.Text.Trim() == "")
                return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int icount = 0;
                double dSum = 0;
                string fldName = OtherHelper.GetLeftName(this.cmbTjFields.Text);
                this.Cursor = Cursors.Default;
                ArrayList arFeas= LayerHelper.GetSelectedFeature(this.currMap, currLayer as IGeoFeatureLayer, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);

                icount = arFeas.Count;
                this.txtcount.Text = icount.ToString();
                
                foreach (IFeature aFea in arFeas)
                {
                    int idx=aFea.Fields.FindField(fldName);
                    double dVal = 0;
                    if (idx>-1)
                    {
                        object obj = aFea.get_Value(idx);
                        double.TryParse(obj.ToString(), out dVal);
                    }
                    
                    dSum += dVal;
                }
                this.txtSum.Text = dSum.ToString();
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }



        }
    }
}
