using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
namespace RCIS.Controls
{
    public partial class FrmUniqvalueRender : Form
    {
        public IFeatureClass currFeatClass = null;
        public string resultField = "";

        public FrmUniqvalueRender()
        {
            InitializeComponent();
        }

        private void FrmUniqvalueRender_Load(object sender, EventArgs e)
        {
            if (currFeatClass == null)
                return;
            IFields fields = currFeatClass.Fields;
            cmbFields.Items.Clear();
            for (int i = 0; i < fields.FieldCount; i++)
            {

                IField thisFeld = fields.get_Field(i);
                if (thisFeld.Name.ToUpper().Contains("SHAPE"))
                {
                    continue;
                }
                cmbFields.Items.Add(thisFeld.Name + "|" + thisFeld.AliasName);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string txt = cmbFields.Text;
            if (txt.IndexOf("|") > -1)
            {
                resultField = txt.Substring(0, txt.IndexOf("|"));
            }
        }
    }
}