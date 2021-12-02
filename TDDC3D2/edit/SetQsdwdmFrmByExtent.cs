using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace TDDC3D.edit
{
    /// <summary>
    /// 2019-3，改为与土地权属区图层进行叠加，，
    /// 
    /// </summary>
    public partial class SetQsdwdmFrmByExtent : Form
    {
        public SetQsdwdmFrmByExtent()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IWorkspace currWs = null;
        public IMap currMap = null;


        private IFeatureClass tdqsqClass = null;
        private List<IFeature> allZds = new List<IFeature>();

        private void SetQsdwdmFrmByExtent_Load(object sender, EventArgs e)
        {
            if (currWs == null)
                return;
            this.chkZdList.Items.Clear();
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            try
            {
                tdqsqClass = pFeaWs.OpenFeatureClass("CJDCQ");
            }
            catch { }
            if (tdqsqClass == null)
                return;
            allZds = GetFeaturesHelper.getFeaturesByGeo(tdqsqClass, (this.currMap as IActiveView).Extent, esriSpatialRelEnum.esriSpatialRelIntersects);
            this.groupControl1.Text = "当前共" + allZds.Count + "个权属区";
            foreach (IFeature aZd in allZds)
            {
                string zddm = FeatureHelper.GetFeatureStringValue(aZd, "QSQDM");
                string qsqmc = FeatureHelper.GetFeatureStringValue(aZd, "QSQMC"); //权属区名称
                
                int idx = this.chkZdList.Items.Add(zddm+"|"+ qsqmc);
                this.chkZdList.SetItemChecked(idx, true);

            }
            LayerHelper.LoadLayer2Combox(this.cmbLayers, currMap);

            //this.cmbLayers.Properties.Items.Clear();
            //for (int i = 0; i < currMap.LayerCount; i++)
            //{
            //    ILayer currLyr = this.currMap.get_Layer(i);
            //    if (!(currLyr is IFeatureLayer)) continue;

            //    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            //    IFeatureClass currClass = currFeaLyr.FeatureClass;
            //    string clsName = (currClass as IDataset).Name.ToUpper();
            //    this.cmbLayers.Properties.Items.Add(clsName);
            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "") return;
            string className = RCIS.Utility.OtherHelper.GetLeftName(this.cmbLayers.Text.Trim());
            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(className);

            string fcName = (pFC as IDataset).Name.ToUpper();
            if (pFC.Fields.FindField("QSDWDM") < 0)
            {
                MessageBox.Show("该图层没有权属单位代码要处理！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行编号，请稍等...", "处理中");
            wait.Show();

            try
            {

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
                foreach (IFeature aZd in allZds)
                {
                    string zddm = FeatureHelper.GetFeatureStringValue(aZd, "QSQDM");
                    string qsqMc = FeatureHelper.GetFeatureStringValue(aZd, "QSQMC");

                    //宗地代码取前12位
                    if (zddm.Length >=12)
                    {
                        zddm = zddm.Substring(0, 12);
                        
                    }
                    zddm = zddm.PadRight(19, '0');
                    wait.SetCaption("正在执行" + zddm + "的图斑权属单位代码赋值...");

                    //找到所有地类图斑
                    List<IFeature> arAllTbs = GetFeaturesHelper.getFeaturesByGeo(pFC, aZd.ShapeCopy, esriSpatialRelEnum.esriSpatialRelContains);

                    try
                    {
                        foreach (IFeature aTb in arAllTbs)
                        {                           
                            FeatureHelper.SetFeatureValue(aTb, "QSDWDM", zddm);
                            FeatureHelper.SetFeatureValue(aTb, "QSDWMC", qsqMc);
                            aTb.Store();
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                }
                wait.Close();
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("qsdwdmsetvalue");
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
    }
}
