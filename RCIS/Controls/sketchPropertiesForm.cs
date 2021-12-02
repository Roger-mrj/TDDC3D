using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;


using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;

namespace RCIS.Controls
{
    public partial class sketchPropertiesForm : Form
    {
        public sketchPropertiesForm( IEngineEditor _value,AxMapControl _value2)
        {
            InitializeComponent();
            this.currEditor = _value;
            this.currMaptrol = _value2;
        }

        IEngineEditor currEditor = null;
        AxMapControl currMaptrol = null;
        List<IFeature> lstFeatures = new List<IFeature>();
        ArrayList arAllpoints = new ArrayList();

        IFeature currFea = null;
        private void sketchPropertiesForm_Load(object sender, EventArgs e)
        {
            if (currEditor == null)
                return;
            lstFeatures.Clear();
            lstIds.Items.Clear();

            IEnumFeature pEnumFeature = currEditor.EditSelection;
            IFeature  aFeat = pEnumFeature.Next();
            while (aFeat != null)
            {
                lstFeatures.Add(aFeat);
                lstIds.Items.Add(aFeat.OID);

                aFeat = pEnumFeature.Next();
            }

            

        }

        private void lstIds_SelectedIndexChanged(object sender, EventArgs e)
        {
            //找到这个要素
            int id = Convert.ToInt32(lstIds.SelectedItem.ToString());
            
            foreach (IFeature aFea in lstFeatures)
            {
                if (aFea.OID == id)
                {
                    currFea = aFea;
                    break;
                }
            }

            IGeometry currGeo = currFea.ShapeCopy as IGeometry;
            

            arAllpoints.Clear();
            if (currGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
            {
                IGeometryCollection pGC = currGeo as IGeometryCollection;
                for (int i = 0; i < pGC.GeometryCount; i++)
                {
                    IRing curRing = pGC.get_Geometry(i) as IRing;
                    IPointCollection curPC = curRing as IPointCollection;
                    for (int j = 0; j < curPC.PointCount; j++)
                    {
                        IPoint aPt = curPC.get_Point(j);
                        arAllpoints.Add(aPt);
                    }
                }

            }
            else if (currGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
            {
                IPointCollection points = currGeo as IPointCollection;
                for (int i = 0; i < points.PointCount; i++)
                {
                    IPoint aPT = points.get_Point(i);
                    arAllpoints.Add(aPT);

                }
            }

            
            lstProperties.Items.Clear();
            
            int index = 0;
            foreach (IPoint apt in arAllpoints)
            {

                ListViewItem newItem = lstProperties.Items.Add(index.ToString());
                newItem.SubItems.Add(apt.X.ToString());
                newItem.SubItems.Add(apt.Y.ToString());
                index++;
                
            }
            

        }

        private void lstProperties_DoubleClick(object sender, EventArgs e)
        {
            if (currMaptrol == null)
                return;
            if (lstProperties.SelectedItems[0] == null)
                return;
            string sId = lstProperties.SelectedItems[0].Text.Trim();
            int id = Convert.ToInt32(sId);

            IPoint currPt = arAllpoints[id] as IPoint ;

            currMaptrol.FlashShape(currPt as IGeometry);
            
        }

        private void 删除结点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.lstIds.SelectedIndex < 0)
            {
                MessageBox.Show("请首先选中某个结点!");
                return;
            }
            int idx=lstProperties.SelectedItems[0].Index;
            string sId = lstProperties.SelectedItems[0].Text.Trim();
            int id = Convert.ToInt32(sId);


            

            this.currEditor.StartOperation();
            try
            {
                IPointCollection pCS = currFea.ShapeCopy as IPointCollection;
                pCS.RemovePoints(id, 1);

                lstProperties.Items.RemoveAt(idx);

                currFea.Shape = pCS  as IGeometry  ;
                currFea.Store();
                
            }
            catch(Exception ex) { }
            finally
            {
                this.currEditor.StopOperation("enddeltevertex");

                this.currMaptrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }

        }
    }
}