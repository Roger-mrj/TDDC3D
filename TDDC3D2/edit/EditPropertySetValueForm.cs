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
using System.Collections;

using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.edit
{
    public partial class EditPropertySetValueForm : Form
    {
        public EditPropertySetValueForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;


        public esriSpatialRelEnum curJHGX
        {
            get
            {
                if (this.cmbJHGX.SelectedIndex == 0)
                {
                    return esriSpatialRelEnum.esriSpatialRelContains;
                }
                else
                {
                    return esriSpatialRelEnum.esriSpatialRelIntersects;
                }
            }
        }

        private void EditPropertySetValueForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbLayer,  this.currMap);

            LayerHelper.LoadLayer2Combox(this.cmbFWTC, this.currMap, esriGeometryType.esriGeometryPolygon); 
            if (this.cmbLayer.Properties.Items.Count > 0)
            {
                this.cmbLayer.SelectedIndex = 0;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.Text.Trim() == "")
                return;
            if (this.cmbField.Text.Trim() == "")
                return;
            if (this.txtValues.Enabled)
            {
                if (this.txtValues.Text.Trim() == "")
                {
                    if (MessageBox.Show("确定要将所选字段赋为空值么？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                    
            }
            

            string className = OtherHelper.GetLeftName(this.cmbLayer.Text.Trim());
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            IFeatureClass pTargetFC = currLayer.FeatureClass;
            string currFld =OtherHelper.GetLeftName( this.cmbField.Text);

            int idx=  pTargetFC.FindField(currFld);
            if (idx == -1)
                return;
            IField pTargetFld = pTargetFC.Fields.get_Field(idx);

            if (this.radioGroup1.SelectedIndex == 0)
            {
                #region 选中要素
                System.Collections.ArrayList arrSels = LayerHelper.GetSelectedFeature(currMap, currLayer as IGeoFeatureLayer, pTargetFC.ShapeType);
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                try
                {
                    foreach (IFeature aFea in arrSels)
                    {
                        if (pTargetFld.Type == esriFieldType.esriFieldTypeInteger || pTargetFld.Type == esriFieldType.esriFieldTypeSingle || pTargetFld.Type == esriFieldType.esriFieldTypeDouble)
                        {
                            double dV = 0;
                            double.TryParse(this.txtValues.Text, out dV);
                            FeatureHelper.SetFeatureValue(aFea, currFld, (object)dV);
                            aFea.Store();
                        }
                        else
                        {
                            FeatureHelper.SetFeatureValue(aFea, currFld, txtValues.Text.Trim());
                            aFea.Store();
                        }

                    }
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("setvalues");
                    MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                    MessageBox.Show(ex.Message);
                }
                #endregion


            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                #region 当前范围
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = (this.currMap as IActiveView).Extent;
                pSF.SpatialRel = this.curJHGX;
                IFeatureCursor pCursor = pTargetFC.Search(pSF as IQueryFilter, false);
                IFeature aFea = null;
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                try
                {
                    while ((aFea = pCursor.NextFeature()) != null)
                    {
                        if (pTargetFld.Type == esriFieldType.esriFieldTypeInteger || pTargetFld.Type == esriFieldType.esriFieldTypeSingle || pTargetFld.Type == esriFieldType.esriFieldTypeDouble)
                        {
                            double dV = 0;
                            double.TryParse(this.txtValues.Text, out dV);
                            FeatureHelper.SetFeatureValue(aFea, currFld, (object)dV);
                            aFea.Store();
                        }
                        else
                        {
                            FeatureHelper.SetFeatureValue(aFea, currFld, txtValues.Text.Trim());
                            aFea.Store();
                        }
                    }
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("setvalues");
                    MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    OtherHelper.ReleaseComObject(pCursor);
                }
                #endregion


            }
            else if (this.radioGroup1.SelectedIndex == 2)
            {
                //按图层选中要素 范围线 进行 选择要素
                if (this.cmbFWTC.Text.Trim() == "")
                {
                    MessageBox.Show("选择范围所在图层。");
                    return;
                }
                string fwClassName = OtherHelper.GetLeftName(this.cmbFWTC.Text.Trim());
                IFeatureLayer fwLayer = LayerHelper.QueryLayerByModelName(this.currMap, fwClassName);
                if (fwLayer == null)
                    return;
                ArrayList arSelFea= LayerHelper.GetSelectedFeature(this.currMap, fwLayer as IGeoFeatureLayer, esriGeometryType.esriGeometryPolygon);

                if (arSelFea.Count == 0)
                {
                    MessageBox.Show("范围所在图层没有选中要素!");
                    return;
                }
                if (this.radioGroupSetValue.SelectedIndex == 0)
                {
                    #region 赋值固定值
                    IPolygon polFw = GeometryHelper.UnionPolygon(arSelFea);
                    if (!polFw.IsEmpty)
                    {
                        #region 范围
                        ISpatialFilter pSF = new SpatialFilterClass();
                        pSF.Geometry = polFw as IGeometry;
                        pSF.SpatialRel = this.curJHGX;
                        IFeatureCursor pCursor = pTargetFC.Search(pSF as IQueryFilter, false);
                        IFeature aFea = null;
                        RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                        try
                        {
                            while ((aFea = pCursor.NextFeature()) != null)
                            {
                                if (pTargetFld.Type == esriFieldType.esriFieldTypeInteger || pTargetFld.Type == esriFieldType.esriFieldTypeSingle || pTargetFld.Type == esriFieldType.esriFieldTypeDouble)
                                {
                                    double dV = 0;
                                    double.TryParse(this.txtValues.Text, out dV);
                                    FeatureHelper.SetFeatureValue(aFea, currFld, (object)dV);
                                    aFea.Store();
                                }
                                else
                                {
                                    FeatureHelper.SetFeatureValue(aFea, currFld, txtValues.Text.Trim());
                                    aFea.Store();
                                }
                            }
                            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("setvalues");
                            MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            OtherHelper.ReleaseComObject(pCursor);
                        }
                        #endregion
                    }
                    #endregion 

                }
                else if (this.radioGroupSetValue.SelectedIndex == 1)
                {
                    IFeatureClass fwClass = fwLayer.FeatureClass;
                    int fldIdx = fwClass.FindField(OtherHelper.GetLeftName(this.cmbSelLayerField.Text.Trim()));
                    if (fldIdx == -1)
                    {
                        MessageBox.Show("找不到范围图层字段！");
                        return;
                    }
                    #region 赋值 选中图层的某一字段值

                    DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理数据", "请稍候...");
                    wait.Show();
                    try
                    {
                        foreach (IFeature aQueryFea in arSelFea)
                        {
                            object objValue = aQueryFea.get_Value(fldIdx);
                            wait.SetCaption("正在赋值" + objValue.ToString() + "...");
                            Application.DoEvents();
                            SetAFwFldValue(pTargetFC, aQueryFea.ShapeCopy, objValue, pTargetFld);
                                                     
                        }
                        RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("setvalues");
                        wait.Close();
                        MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch (Exception ex)
                    {
                        RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                        if (wait != null)
                            wait.Close();
                    }
                    #endregion 
                }



                

            }


        }


        private void SetAFwFldValue(IFeatureClass pFeaClass, IGeometry aFw, object objValue, IField pField)
        {
            string fldName = pField.Name;
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = aFw;
            pSF.SpatialRel = this.curJHGX;
            IFeatureCursor pCursor = pFeaClass.Search(pSF as IQueryFilter, false);
            IFeature aFea = null;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                while ((aFea = pCursor.NextFeature()) != null)
                {
                    if (pField.Type == esriFieldType.esriFieldTypeInteger || pField.Type == esriFieldType.esriFieldTypeSingle || pField.Type == esriFieldType.esriFieldTypeDouble)
                    {
                        double dV = 0;
                        double.TryParse(objValue.ToString(), out dV);
                        FeatureHelper.SetFeatureValue(aFea, fldName, (object)dV);
                        aFea.Store();
                    }
                    else
                    {
                        string sVal = objValue.ToString().Trim();
                        if (fldName.ToUpper() == "QSDWDM" || fldName.ToUpper() == "ZLDWDM")
                        {
                            //如果是取行政区代码，则补7个0;
                            if (sVal.Length < 19)
                            {
                                sVal=sVal.PadRight(19, '0');
                            }
                            
                        }
                        FeatureHelper.SetFeatureValue(aFea, fldName, sVal);
                        aFea.Store();
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                OtherHelper.ReleaseComObject(pCursor);
            }  
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        IFeatureLayer currLayer = null;
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //找到对应字段
            if (this.cmbLayer.Text.Trim() == "")
                return;

            string className = OtherHelper.GetLeftName( this.cmbLayer.Text.Trim());
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            IFeatureClass pFeaClass = currLayer.FeatureClass;

            this.cmbField.Properties.Items.Clear();
            for (int i = 0; i < pFeaClass.Fields.FieldCount; i++)
            {
                IField aFld = pFeaClass.Fields.get_Field(i);
                if (aFld.Type == esriFieldType.esriFieldTypeString)
                {
                    this.cmbField.Properties.Items.Add(aFld.Name.ToUpper() + "|" + aFld.AliasName);
                }
            }

            if (this.cmbField.Properties.Items.Count > 0)
            {
                this.cmbField.SelectedIndex = 0;
            }


        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 2)
            {
                this.cmbFWTC.Enabled = true;
                //如果按选中范围，则 范围图层字段 可用
                this.radioGroupSetValue.Enabled = true;
                this.cmbJHGX.Enabled = true;

            }
            else
            {
                this.cmbFWTC.Enabled = false;
                this.radioGroupSetValue.Enabled = false;
                this.cmbJHGX.Enabled = false;
            }
        }

        private void radioGroupSetValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroupSetValue.SelectedIndex == 0)
            {
                this.txtValues.Enabled = true;
                this.cmbSelLayerField.Enabled = false;
            }
            else if (this.radioGroupSetValue.SelectedIndex==1)
            {
                this.txtValues.Enabled = false;
                this.cmbSelLayerField.Enabled = true;
            }
        }

        private void cmbFWTC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbFWTC.Text.Trim() == "")
                return;

            string className = OtherHelper.GetLeftName(this.cmbFWTC.Text.Trim());
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            IFeatureClass pFeaClass = currLayer.FeatureClass;

            this.cmbSelLayerField.Properties.Items.Clear();
            for (int i = 0; i < pFeaClass.Fields.FieldCount; i++)
            {
                IField aFld = pFeaClass.Fields.get_Field(i);
                if (aFld.Type == esriFieldType.esriFieldTypeString)
                {
                    this.cmbSelLayerField.Properties.Items.Add(aFld.Name.ToUpper() + "|" + aFld.AliasName);
                }
            }

            if (this.cmbSelLayerField.Properties.Items.Count > 0)
            {
                this.cmbSelLayerField.SelectedIndex = 0;
            }
        }
    }
}
