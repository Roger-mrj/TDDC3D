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
namespace TDDC3D.edit
{
    public partial class SetHoleForm : Form
    {
        public SetHoleForm()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        public IMap currMap = null;

        IFeatureLayer currLayer = null;
        private void SetHoleForm_Load(object sender, EventArgs e)
        {
            this.cmbLayer.Properties.Items.Clear();
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = this.currMap.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer currFeaLyr = childLyr as IFeatureLayer;
                            IFeatureClass currClass = currFeaLyr.FeatureClass;
                            string clsName = (currClass as IDataset).Name.ToUpper();
                            if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            {

                                this.cmbLayer.Properties.Items.Add(clsName);

                            }
                        }
                    }
                }
                else if (currLayer is IFeatureLayer)
                {
                    IFeatureLayer currFeaLyr = currLayer as IFeatureLayer;
                    IFeatureClass currClass = currFeaLyr.FeatureClass;
                    string clsName = (currClass as IDataset).Name.ToUpper();
                    if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {

                        this.cmbLayer.Properties.Items.Add(clsName);

                    }
                }


                
                
            }

            int idx1 = 0;
            for (int i = 0; i < this.cmbLayer.Properties.Items.Count; i++)
            {
                string name = this.cmbLayer.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLTB"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbLayer.SelectedIndex = idx1;
        }

        private System.Collections.ArrayList arSelectFea = new System.Collections.ArrayList();
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //得到该图层数量
            string className = this.cmbLayer.Text.Trim();
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            try
            {
                IIdentify identify = currLayer as IIdentify;
                IFeatureSelection pFSelection = currLayer as IFeatureSelection;
                IArray array = identify.Identify((this.currMap as IActiveView).Extent);
                 int allCount=0;
                if (array != null)
                {
                    allCount= array.Count;

                    
                }
                arSelectFea = LayerHelper.GetSelectedFeature(this.currMap, currLayer as IGeoFeatureLayer, currLayer.FeatureClass.ShapeType);
                this.lblNumbers.Text = "当前区域共" + allCount + "个要素。选中" + arSelectFea.Count + "个要素";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void SetHole(IFeature parentFea, List<IFeature> childFeature)
        {
            IGeometry parentGeo = parentFea.ShapeCopy;
     
            ITopologicalOperator pTop = parentGeo as ITopologicalOperator;
            foreach (IFeature aFea in childFeature)
            {
                if (aFea.OID == parentFea.OID)
                {
                    continue;
                }
                parentGeo=pTop.Difference(aFea.Shape);
                if (parentGeo.IsEmpty)
                {
                    break;
                }
                pTop = parentGeo as ITopologicalOperator;

            }
            if (parentGeo.IsEmpty)
            {
                parentFea.Delete();
            }
            else
            {
                parentFea.Shape = parentGeo;
                parentFea.Store();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "")
                return;
            if (this.currLayer == null)
                return;
            IFeatureClass pFC=currLayer.FeatureClass;

            
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            this.Cursor = Cursors.WaitCursor;

            try
            {              

                if (this.radioGroup1.SelectedIndex == 0)
                {
                    //当前范围
                    IIdentify identify = currLayer as IIdentify;
                    IFeatureSelection pFSelection = currLayer as IFeatureSelection;
                    IArray array = identify.Identify((this.currMap as IActiveView).Extent);
                    if (array != null)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            IFeatureIdentifyObj idObj = array.get_Element(i) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature aFea = pRow.Row as IFeature;

                            List<IFeature> containFeas = GetFeaturesHelper.getFeaturesByGeo(pFC, aFea.Shape, esriSpatialRelEnum.esriSpatialRelContains);
                            SetHole(aFea, containFeas);

                        }
                    }
                }
                else if (this.radioGroup1.SelectedIndex == 1)
                {
                    
                    foreach (IFeature aFea in arSelectFea)
                    {

                        List<IFeature> containFeas = GetFeaturesHelper.getFeaturesByGeo(pFC, aFea.Shape, esriSpatialRelEnum.esriSpatialRelContains);

                        SetHole(aFea, containFeas);
                        
                    }
                }

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("setHole");
                this.Cursor = Cursors.Default;
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }
    }
}
