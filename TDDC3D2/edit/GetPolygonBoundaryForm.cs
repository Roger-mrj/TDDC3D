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
    public partial class GetPolygonBoundaryForm : Form
    {
        public GetPolygonBoundaryForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
        }

        public string polygonLayerName = "";
        public string PolylineLayerName = "";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if ((this.cmbPolygonLayer.Text.Trim() == "") || (this.cmbPolylinelayer.Text.Trim() == ""))
            {
                MessageBox.Show("请选择对应图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            polygonLayerName = this.cmbPolygonLayer.Text.Trim();
            PolylineLayerName = this.cmbPolylinelayer.Text.Trim();

            this.DialogResult = DialogResult.OK;
            Close();

        }

        private void GetPolygonBoundaryForm_Load(object sender, EventArgs e)
        {
            this.cmbPolygonLayer.Properties.Items.Clear();
            this.cmbPolylinelayer.Properties.Items.Clear();

            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbPolygonLayer, currMap,esriGeometryType.esriGeometryPolygon);
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbPolygonLayer, currMap, esriGeometryType.esriGeometryPolyline);

           
            int idx1 = -1;
            int idx2 = -1;
            for (int i = 0; i < this.cmbPolygonLayer.Properties.Items.Count; i++)
            {
                string name = this.cmbPolygonLayer.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLTB"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbPolygonLayer.SelectedIndex = idx1;


            for (int i = 0; i < this.cmbPolylinelayer.Properties.Items.Count; i++)
            {
                string name = this.cmbPolylinelayer.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLJX"))
                {
                    idx2 = i;
                    break;
                }
            }
            this.cmbPolylinelayer.SelectedIndex = idx2;
        }
    }
}
