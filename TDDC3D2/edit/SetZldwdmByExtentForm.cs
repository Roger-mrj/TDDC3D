using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

using System.Collections;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.edit
{
    /// <summary>
    /// 2019-3 修改 ，改为 由土地权属区的坐落单位代码 赋值给dltb
    /// 2019-4修改，土地权属区 改为村级调查区
    /// </summary>
    public partial class SetZldwdmByExtentForm : Form
    {
        public SetZldwdmByExtentForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        public IMap currMap = null;


        private IFeatureClass cjdcqClass = null;
        private List<IFeature> allXzq = new List<IFeature>();


        private void SetZldwdmByExtentForm_Load(object sender, EventArgs e)
        {
            if (currWs == null)
                return;
            


            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbXzq, this.currMap, esriGeometryType.esriGeometryPolygon);
            int idx1 = 0;
            for (int i = 0; i < this.cmbXzq.Properties.Items.Count; i++)
            {
                string name = this.cmbXzq.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("CJDCQ"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbXzq.SelectedIndex = idx1;

            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap);
            int idx2 = 0;
            for (int i = 0; i < this.cmbLayers.Properties.Items.Count; i++)
            {
                string name = this.cmbLayers.Properties.Items[i].ToString().Trim().ToUpper();
                if (name.Contains("DLTB"))
                {
                    idx2 = i;
                    break;
                }
            }
            this.cmbLayers.SelectedIndex = idx2;

        }
        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "") return;
            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass( RCIS.Utility.OtherHelper.GetLeftName( this.cmbLayers.Text.Trim()));
            string fcName = (pFC as IDataset).Name.ToUpper();
            if (pFC.Fields.FindField("ZLDWDM") < 0)
            {
                MessageBox.Show("该图层没有座落单位代码要处理！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行编号，请稍等...", "处理中");
            wait.Show();
            try
            {
                             
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                foreach(IFeature aXzq in allXzq)
                {
                    //string xzqdm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");
                    //string xzqmc = FeatureHelper.GetFeatureStringValue(aXzq, "XZQMC");

                    string zldwdm = FeatureHelper.GetFeatureStringValue(aXzq, "ZLDWDM");
                    string zldwmc = FeatureHelper.GetFeatureStringValue(aXzq, "ZLDWMC");

                    if (zldwdm.Length < 19)
                    {
                        zldwdm = zldwdm.PadRight(19, '0');
                    }
                    wait.SetCaption("正在执行" + zldwdm + "的图斑单位代码赋值...");
                                       
                    //找到所有地类图斑
                    List<IFeature> arAllTbs = GetFeaturesHelper.getFeaturesByGeo(pFC, aXzq.ShapeCopy, esriSpatialRelEnum.esriSpatialRelContains);
                    
                    try
                    {
                        foreach (IFeature aTb in arAllTbs)
                        {
                            FeatureHelper.SetFeatureValue(aTb, "ZLDWDM", zldwdm);
                            FeatureHelper.SetFeatureValue(aTb, "ZLDWMC", zldwmc);
                            if (this.checkEdit1.Checked)
                            {
                                FeatureHelper.SetFeatureValue(aTb, "QSDWDM", zldwdm);
                                FeatureHelper.SetFeatureValue(aTb, "QSDWMC", zldwmc);
                            }
                            aTb.Store();
                        }
                    }
                    catch (Exception ex)
                    {
                      
                    }
                    
                }
                wait.Close();
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("zldwdmsetvalue");
                MessageBox.Show("赋值完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }
        
        }

        private void cmbXzq_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chkCjdcqList.Items.Clear();
            
            string cjdcqName = OtherHelper.GetLeftName(this.cmbXzq.Text);
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            try
            {
                cjdcqClass = pFeaWs.OpenFeatureClass(cjdcqName);
            }
            catch { }

            if (cjdcqClass == null)
                return;
            allXzq = GetFeaturesHelper.getFeaturesByGeo(cjdcqClass, (this.currMap as IActiveView).Extent, esriSpatialRelEnum.esriSpatialRelIntersects);
            this.groupControl1.Text = "当前共" + allXzq.Count + "个调查区";
            foreach (IFeature aXzq in allXzq)
            {
                string qsqmc = FeatureHelper.GetFeatureStringValue(aXzq, "ZLDWMC");
                string zldwdm = FeatureHelper.GetFeatureStringValue(aXzq, "ZLDWDM");
                int idx = this.chkCjdcqList.Items.Add(zldwdm + "|" + qsqmc);
                this.chkCjdcqList.SetItemChecked(idx, true);
            }

        }
    }
}
