using System;
using System.Collections.Generic;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.datado
{
    public partial class GdxhdmSetvalForm : Form
    {
        public GdxhdmSetvalForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private IFeatureLayer currLayer = null;
        IFeatureClass dltbClass = null;

        private void GdxhdmSetvalForm_Load(object sender, EventArgs e)
        {
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }


        private List<string> getZhouweiDl(IFeature infea)
        {
            List<string> lst = new List<string>();
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = infea.Shape;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pcursor = dltbClass.Search(pSF as IQueryFilter, true);
            IFeature aFeature = null;
            try
            {
                while ((aFeature = pcursor.NextFeature()) != null)
                {
                    if (aFeature.OID == infea.OID)
                        continue;
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                    if (!lst.Contains(dlbm))
                    {
                        lst.Add(dlbm);
                    }
                }

            }
            catch { }
            finally
            {
                OtherHelper.ReleaseComObject(pcursor);
                OtherHelper.ReleaseComObject(pSF);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return lst;
        }
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (currLayer == null) return;
            dltbClass = currLayer.FeatureClass;

            //对每条线进行赋值
            IIdentify idLyrs = currLayer as IIdentify;
            IArray arrayJx = idLyrs.Identify((currLayer.FeatureClass as IGeoDataset).Extent);
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
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                    if (dlbm.StartsWith("01"))
                    {
                        List<string> lstdl = this.getZhouweiDl(aFeature);
                        bool lqgd=false;
                        foreach (string aDl in lstdl)
                        {
                            
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

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classname = OtherHelper.GetLeftName(this.cmbLayer.Text);
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, classname);
        }
    }
}
