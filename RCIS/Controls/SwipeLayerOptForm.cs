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
using RCIS.GISCommon;
using RCIS.Utility;
namespace RCIS.Controls
{
    public partial class SwipeLayerOptForm : Form
    {
        public SwipeLayerOptForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;

        public ILayer retLyr = null;

        private void SwipeLayerOptForm_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayers, currMap);            
            if (this.cmbLayers.Properties.Items.Count > 0)
            {
                this.cmbLayers.SelectedIndex = 0;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string name = OtherHelper.GetLeftName(this.cmbLayers.Text);
            this.retLyr = LayerHelper.QueryLayerByModelName(currMap, name);            
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
