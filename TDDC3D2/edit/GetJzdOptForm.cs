using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;




namespace TDDC3D.edit
{
    /// <summary>
    /// 已废弃
    /// </summary>
    public partial class GetJzdOptForm : Form
    {
        public GetJzdOptForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;

        public string polygonLayerName = "";
        public string PointLayerName = "";



        private void GetJzdOptForm_Load(object sender, EventArgs e)
        {
            this.cmbPolygonLayer.Properties.Items.Clear();
            this.cmbPointlayer.Properties.Items.Clear();


            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = this.currMap.get_Layer(i);
                if (!(currLyr is IFeatureLayer)) continue;
                IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                IFeatureClass currClass = currFeaLyr.FeatureClass;
                string clsName = (currClass as IDataset).Name.ToUpper();
                if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    this.cmbPolygonLayer.Properties.Items.Add(clsName);

                }
                else if (currClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    this.cmbPointlayer.Properties.Items.Add(clsName);
                }
            }
            int idx1 = -1;
            int idx2 = -1;
            for (int i = 0; i < this.cmbPolygonLayer.Properties.Items.Count; i++)
            {
                string name = this.cmbPolygonLayer.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("ZD"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbPolygonLayer.SelectedIndex = idx1;


            for (int i = 0; i < this.cmbPointlayer.Properties.Items.Count; i++)
            {
                string name = this.cmbPointlayer.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("JZD"))
                {
                    idx2 = i;
                    break;
                }
            }
            this.cmbPointlayer.SelectedIndex = idx2;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if ((this.cmbPolygonLayer.Text.Trim() == "") || (this.cmbPointlayer.Text.Trim() == ""))
            {
                MessageBox.Show("请选择对应图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            polygonLayerName = this.cmbPolygonLayer.Text.Trim();
            PointLayerName = this.cmbPointlayer.Text.Trim();

            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
