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
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ADF;
namespace TDDC3D.edit
{
    public partial class UnionSelTBPatchForm : Form
    {
        public UnionSelTBPatchForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;

        private void UnionSelTBPatchForm_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);
        }

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string className = OtherHelper.GetLeftName(this.cmbLayer.Text);
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
        }

        IFeatureLayer currLayer = null;


        public IFeature getMaxFeature(IGeometry inGeo, IFeatureClass dltbClass, string zldwdm, int oid)
        {

            IFeature currFea = null;
            ITopologicalOperator pTopIn = inGeo as ITopologicalOperator;
            pTopIn.Simplify();

            double maxMj = 0;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = inGeo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            pSF.WhereClause = "ZLDWDM='" + zldwdm + "'";
            using (ComReleaser comrelease = new ComReleaser())
            {
                IFeatureCursor cursor = dltbClass.Search(pSF, false);
                comrelease.ManageLifetime(cursor);
                try
                {
                    IFeature aFea = null;
                    while ((aFea = cursor.NextFeature()) != null)
                    {
                        if (aFea.OID == oid) continue;
                        IGeometry intersect = pTopIn.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                        if (intersect.IsEmpty)
                        {
                            continue;
                        }


                        double newMj = (aFea.ShapeCopy as IArea).Area;
                        if (newMj > maxMj)
                        {
                            maxMj = newMj;
                            currFea = aFea;

                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
                catch (Exception ex)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
            return currFea;
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList arSel = LayerHelper.GetSelectedFeature(this.currMap, currLayer as IGeoFeatureLayer, currLayer.FeatureClass.ShapeType);
            if (arSel.Count == 0)
            {
                MessageBox.Show("当前没有选中要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            int icount = 0;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (IFeature aFeature in arSel)
                {
                    //IFeature maxFeature = SmallTbDoHelper.getMaxFeature(aFeature.Shape, currLayer.FeatureClass, aFeature.OID);
                    string zldwdm = FeatureHelper.GetFeatureStringValue(aFeature, "ZLDWDM");
                    IFeature maxFeature = this.getMaxFeature(aFeature.Shape, currLayer.FeatureClass, zldwdm, aFeature.OID);

                    if (maxFeature == null)
                    {
                        icount++;
                        continue;
                    }
                    ITopologicalOperator pTop = maxFeature.Shape as ITopologicalOperator;
                    pTop.Simplify();
                    IGeometry newGeo = pTop.Union(aFeature.Shape);
                    if (newGeo.IsEmpty)
                    {
                        icount++;
                        continue;
                    }
                    maxFeature.Shape = newGeo;
                    maxFeature.Store();
                    aFeature.Delete();

                }
                this.Cursor = Cursors.Default;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("multipartdo");
                MessageBox.Show("处理完毕！  未处理"+icount+"个！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }

            
        }
    }
}
