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

namespace TDDC3D.datado
{
    public partial class JxxzSetValueForm1cs : Form
    {
        public JxxzSetValueForm1cs()
        {
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }


        public IMap currMap = null;
        IFeatureClass  xzqClass = null;
        IFeatureClass cjdcqClass = null;
        private void JxxzSetValueForm1cs_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayer, currMap, esriGeometryType.esriGeometryPolyline);
            IFeatureLayer xzqLayer=LayerHelper.QueryLayerByModelName(this.currMap, "XZQ");
            xzqClass = xzqLayer.FeatureClass;
            IFeatureLayer cjdcqLyr= LayerHelper.QueryLayerByModelName(this.currMap, "CJDCQ");
            cjdcqClass =cjdcqLyr.FeatureClass;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (currLyr == null) return;
            string classname = OtherHelper.GetLeftName(this.cmbLayer.Text);

            IPolygon xzqExtentPolygon = this.getXzqEnv();
            ITopologicalOperator pTop = xzqExtentPolygon as ITopologicalOperator;
            pTop.Simplify();
            IGeometry outBoundry = pTop.Boundary;
            IRelationalOperator pRelBoundry = outBoundry as IRelationalOperator;
            pTop.Simplify();

            //对每条线进行赋值
            IIdentify idLyrs = currLyr as IIdentify;
            IArray arrayJx = idLyrs.Identify((currLyr.FeatureClass as IGeoDataset).Extent);
            if (arrayJx == null)
            {
                MessageBox.Show("该界线层没数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IWorkspaceEdit pWsedt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWsedt.StartEditing(false);
            pWsedt.StartEditOperation();
            try
            {
                for (int i = 0; i < arrayJx.Count; i++)
                {
                    IFeatureIdentifyObj obj = arrayJx.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aRow = obj as IRowIdentifyObject;
                    IFeature aFeature = aRow.Row as IFeature;

                    //如果这条线是外围
                    if (pRelBoundry.Contains(aFeature.Shape))
                    {
                        FeatureHelper.SetFeatureValue(aFeature, "JXLX", "650200");
                        aFeature.Store();
                        
                    }
                    else 
                    if (classname.ToUpper() == "XZQJX")
                    {
                        FeatureHelper.SetFeatureValue(aFeature, "JXLX", "660200");
                        aFeature.Store();
                        
                    }
                    else if (classname.ToUpper() == "CJDCQJX")
                    {
                        //如果这条线与 某xzq交于一条线，
                        if (this.isXiangjie(aFeature.Shape))
                        {
                            FeatureHelper.SetFeatureValue(aFeature, "JXLX", "660200");
                            aFeature.Store();

                        }
                        else
                        {
                            FeatureHelper.SetFeatureValue(aFeature, "JXLX", "670500");
                            aFeature.Store();
                        }
                    }

                }


                pWsedt.StopEditOperation();
                pWsedt.StopEditing(true);
                MessageBox.Show("赋值完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pWsedt.AbortEditOperation();
                pWsedt.StopEditing(false);
                MessageBox.Show(ex.Message);
            }


        }
        IFeatureLayer currLyr = null;

        private IPolygon getXzqEnv()
        {
            List<IFeature> lstAllXzq =new List<IFeature>();
            IFeature aXzq = null;
            IFeatureCursor cursor = xzqClass.Search(null, false);
            try
            {
                while ((aXzq = cursor.NextFeature()) != null)
                {
                    lstAllXzq.Add(aXzq);
                }
            }
            catch { }
            finally
            {
                OtherHelper.ReleaseComObject(cursor);
            }
            IPolygon pol = GeometryHelper.UnionPolygon(lstAllXzq);
            return pol;
        }

        private bool isXiangjie(IGeometry jxGeo)
        {
            bool isOk = false;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = jxGeo;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = xzqClass.Search(pSF as IQueryFilter, true);
            IFeature aFeature = null;
            try
            {
                while ((aFeature = pCursor.NextFeature()) != null)
                {
                    //如果与该xzq边界交于一条线，则为乡界
                    ITopologicalOperator ptopXzq = aFeature.Shape as ITopologicalOperator;
                    ptopXzq.Simplify();
                    IGeometry xzqBoundry = ptopXzq.Boundary;
                    ITopologicalOperator ptopBoundry= xzqBoundry as ITopologicalOperator;
                    IGeometry geo = ptopBoundry.Intersect(jxGeo, esriGeometryDimension.esriGeometry1Dimension);
                    if (!geo.IsEmpty)
                    {
                        isOk = true;
                        break;
                        
                    }
                }
            }
            catch { }
            finally
            {
                OtherHelper.ReleaseComObject(pSF);
                OtherHelper.ReleaseComObject(pCursor);
            }
            return isOk;
        }

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classname = OtherHelper.GetLeftName(this.cmbLayer.Text);
            currLyr = LayerHelper.QueryLayerByModelName(this.currMap, classname);

            
            


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string classname = OtherHelper.GetLeftName(this.cmbLayer.Text);
            if (classname == "") return;
            RCIS.Global.GlobalEditObject.GlobalWorkspace.ExecuteSQL("update " + classname + " set JXXZ='600001'");

            MessageBox.Show("赋值已定界完毕！");
        }
    }
}
