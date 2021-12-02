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
using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.Controls
{
    public partial class Polygon2CentroidForm : Form
    {
        public Polygon2CentroidForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;

        private void Polygon2CentroidForm_Load(object sender, EventArgs e)
        {
            this.cmbPolygonLayer.Properties.Items.Clear();
            this.cmbPointLayer.Properties.Items.Clear();

            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = this.currMap.get_Layer(i);
                if (currLyr is IFeatureLayer)
                {
                    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
                    if (currFeaLyr == null) continue;

                    IFeatureClass currClass = currFeaLyr.FeatureClass;
                    string clsName = (currClass as IDataset).Name.ToUpper();
                    string aliasName = currClass.AliasName;
                    if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        this.cmbPolygonLayer.Properties.Items.Add(clsName + "|" + aliasName);

                    }
                    else if (currClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        this.cmbPointLayer.Properties.Items.Add(clsName + "|" + aliasName);
                    }
                }
                else if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pCLyr = currLyr as ICompositeLayer;
                     for (int j = 0; j < pCLyr.Count; j++)
                     {
                         IFeatureLayer flyr = pCLyr.get_Layer(j) as IFeatureLayer;
                         if (flyr == null) continue;
                         IFeatureClass currClass = flyr.FeatureClass;
                         string clsName = (currClass as IDataset).Name.ToUpper();
                         string aliasName = currClass.AliasName;
                         if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                         {
                             this.cmbPolygonLayer.Properties.Items.Add(clsName + "|" + aliasName);
                         }
                         else if (currClass.ShapeType == esriGeometryType.esriGeometryPoint)
                         {
                             this.cmbPointLayer.Properties.Items.Add(clsName + "|" + aliasName);
                         }

                     }

                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void patchDo(IQueryFilter pQF,IFeatureClass polygonClass,IFeatureClass pointClass)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始提取", "请稍等...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            //所有要素
            IFeatureCursor pFeaCursor = polygonClass.Search(pQF, false);
            IFeature aFea = null;
            try
            {

                while ((aFea = pFeaCursor.NextFeature()) != null)
                {
                    wait.SetCaption("正在提取" + aFea.OID + "...");
                    IFeature newFea = pointClass.CreateFeature();
                    newFea.Shape = (aFea.ShapeCopy as IArea).Centroid;
                    newFea.Store();

                }
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("getcentroid");
                wait.Close();
                MessageBox.Show("提取成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
            }
            finally
            {
                OtherHelper.ReleaseComObject(pFeaCursor);
                if (aFea != null)
                {
                    OtherHelper.ReleaseComObject(aFea);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.cmbPointLayer.Text.Trim() == "" || this.cmbPolygonLayer.Text.Trim() == "")
                return;
            string polygonClassName = OtherHelper.GetLeftName(this.cmbPolygonLayer.Text);
            string pointClassName = OtherHelper.GetLeftName(this.cmbPointLayer.Text);
            try
            {
                IFeatureLayer polygonLyr = LayerHelper.QueryLayerByModelName(this.currMap, polygonClassName);
                IFeatureClass polygonClass = polygonLyr.FeatureClass;
                IFeatureLayer pointLyr = LayerHelper.QueryLayerByModelName(this.currMap, pointClassName);
                IFeatureClass pointClass = pointLyr.FeatureClass;
                if (this.radioGroup1.SelectedIndex == 0)
                {
                    patchDo(null,polygonClass,pointClass);

                }
                else if (this.radioGroup1.SelectedIndex == 1)
                {
                    //当前范围
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    pSF.Geometry = (this.currMap as IActiveView).Extent;
                    patchDo(pSF as IQueryFilter, polygonClass, pointClass);
                }
                else if (this.radioGroup1.SelectedIndex == 2)
                {
                    System.Collections.ArrayList arSel = LayerHelper.GetSelectedFeature(this.currMap, polygonLyr as IGeoFeatureLayer, polygonClass.ShapeType);
                    DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("开始提取", "请稍等...");
                    wait.Show();
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                    try
                    {

                        foreach(IFeature aFea in arSel)
                        {
                            wait.SetCaption("正在提取" + aFea.OID + "...");
                            IFeature newFea = pointClass.CreateFeature();
                            newFea.Shape = (aFea.ShapeCopy as IArea).Centroid;
                            newFea.Store();

                        }
                        RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("getcentroid");
                        wait.Close();
                        MessageBox.Show("提取成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                        if (wait != null)
                            wait.Close();
                        MessageBox.Show(ex.Message);
                    }
                    
                }
                (this.currMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
