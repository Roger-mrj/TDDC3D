using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using ESRI.ArcGIS.Controls;

using RCIS.Utility;
namespace TDDC3D.edit
{
    public partial class TbmjCalculateForm : Form
    {
        public TbmjCalculateForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public AxMapControl mapcontrol = null;

        IFeatureLayer currLayer = null;
        private void TbmjCalculateForm_Load(object sender, EventArgs e)
        {
            //this.cmbLayer.Properties.Items.Clear();
            //for (int i = 0; i < currMap.LayerCount; i++)
            //{
            //    ILayer currLyr = this.currMap.get_Layer(i);
            //    if (!(currLyr is IFeatureLayer)) continue;
            //    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            //    IFeatureClass currClass = currFeaLyr.FeatureClass;
            //    string clsName = (currClass as IDataset).Name.ToUpper();
            //    if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            //    {

            //        this.cmbLayer.Properties.Items.Add(clsName);

            //    }
            //}

            LayerHelper.LoadLayer2Combox(this.cmbLayer, this.currMap, esriGeometryType.esriGeometryPolygon);

            int idx1 = -1;
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }


       

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "")
                return;
            if (this.currLayer == null)
                return;
            if (this.cmbField.Text.Trim()=="") return;
            string mjField=OtherHelper.GetLeftName(this.cmbField.Text);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在计算面积，请稍等", "请稍等...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                int currDh = 0;

                if (this.radioGroup1.SelectedIndex == 0)
                {
                    int i = 0;
                    IEnvelope pEnv = (this.currMap as IActiveView).Extent; //当前范围
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = pEnv as IGeometry;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IFeatureCursor updateCursor = currLayer.FeatureClass.Update(pSF as IQueryFilter, false);
                    IFeature aupdateFea = null;
                    try
                    {
                        while ((aupdateFea = updateCursor.NextFeature()) != null)
                        {
                            IPoint selectPoint = (aupdateFea.ShapeCopy as IArea).Centroid;
                            double X = selectPoint.X;
                            currDh = (int)(X / 1000000);////WK---带号

                            double tbmj = area.SphereArea(aupdateFea.ShapeCopy, currDh);
                            FeatureHelper.SetFeatureValue(aupdateFea, mjField, tbmj);
                            updateCursor.UpdateFeature(aupdateFea);


                            if (i % 50 == 0)
                            {
                                wait.SetCaption("当前已经计算" + i + "个图斑...");
                                Application.DoEvents();
                            }
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pSF);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                    }                   
                                      
                }
                else if (this.radioGroup1.SelectedIndex == 1)
                {
                    int ifrist = 0;
                    foreach (IFeature aFea in arSelectFea)
                    {

                        IPoint selectPoint = (aFea.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        currDh = (int)(X / 1000000);////WK---带号

                        double tbmj = area.SphereArea(aFea.ShapeCopy, currDh);

                        //SphereArea.SphereAreaDecimal decmj = new SphereArea.SphereAreaDecimal();
                        //double mj2 = Convert.ToDouble(decmj.SphereArea(aFea.ShapeCopy, (decimal)currDh));

                        FeatureHelper.SetFeatureValue(aFea,mjField , tbmj);
                        aFea.Store();

                        //IGeometry geo = area.getNewGeo(aFea.ShapeCopy);
                        //IFeature newFeature = currLayer.FeatureClass.CreateFeature();
                        //newFeature.Shape = geo;
                        //newFeature.Store();
                        ifrist++;

                        if (ifrist % 50 == 0)
                        {
                            wait.SetCaption("当前已经计算" + ifrist + "个图斑...");
                            Application.DoEvents();
                        }

                    }
                }
                
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("caltbmj");
                wait.Close();
                MessageBox.Show("计算完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait!=null)
                {
                    wait.Close();
                }
                MessageBox.Show(ex.Message);
            }

            


        }

        private System.Collections.ArrayList arSelectFea = new System.Collections.ArrayList();

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //得到该图层数量
            string className =OtherHelper.GetLeftName( this.cmbLayer.Text.Trim());
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            try
            {               
                //IIdentify identify = currLayer as IIdentify;
                IFeatureSelection pFSelection = currLayer as IFeatureSelection;
                //IArray array = identify.Identify(this.mapcontrol.ActiveView.Extent.Envelope);
                //if (array == null)
                //    return;

                IFeatureClass pFC = currLayer.FeatureClass;
                ISpatialFilter pQF=new SpatialFilterClass();
                pQF.Geometry=this.mapcontrol.Extent;
                pQF.SpatialRel=esriSpatialRelEnum.esriSpatialRelIntersects;
                int allCount = pFC.FeatureCount(pQF as IQueryFilter);

                arSelectFea = LayerHelper.GetSelectedFeature(this.currMap, currLayer as IGeoFeatureLayer, currLayer.FeatureClass.ShapeType);
                this.lblNumbers.Text = "当前区域共" + allCount + "个要素。选中"+arSelectFea.Count+"个要素";

                this.cmbField.Properties.Items.Clear();
                for (int i = 0; i < currLayer.FeatureClass.Fields.FieldCount; i++)
                {
                    IField aFld = currLayer.FeatureClass.Fields.get_Field(i);
                    if (aFld.Type == esriFieldType.esriFieldTypeDouble || aFld.Type==esriFieldType.esriFieldTypeSingle)
                    {
                        this.cmbField.Properties.Items.Add(aFld.Name.ToUpper() + "|" + aFld.AliasName);
                    }
                }

                int idx = -1;
                for (int i=0;i<this.cmbField.Properties.Items.Count;i++)
                {
                    if (OtherHelper.GetLeftName(this.cmbField.Properties.Items[i].ToString().Trim()).ToUpper() == "TBMJ")
                    {
                        idx = i;
                        break;
                    }
                }
                this.cmbField.SelectedIndex = idx;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            
        }
    }
}
