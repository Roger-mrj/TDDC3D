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
    public partial class XxdwkdSetValForm : Form
    {
        public XxdwkdSetValForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        private void XxdwkdSetValForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(cmbLayer, currMap, esriGeometryType.esriGeometryPolygon);
            //加载所有地类
            List<string> sAllDlbm = sys.YWCommonHelper.getAllDlbm();    
            foreach (string adl in sAllDlbm)
            {
                if (adl.Substring(0, 2) == "03" || adl == "1101" || adl == "1103" || adl == "1109" || adl == "1107" || adl == "1006")
                {
                    int idx=this.cmbDlbms.Properties.Items.Add(adl,true);
                }
                else
                {
                    this.cmbDlbms.Properties.Items.Add(adl, true);
                }
                
               
            }
        }

        private List<string> exceptDLBM = new List<string>();

        private System.Collections.ArrayList arSelectFea = new System.Collections.ArrayList();

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string dltbClassName = OtherHelper.GetLeftName(this.cmbLayer.Text);
            if (dltbClassName.Trim().ToUpper() == "")
                return;
            IFeatureLayer pFeaLyr = LayerHelper.QueryLayerByModelName(this.currMap, dltbClassName);
            if (pFeaLyr == null) return;
            string kdFieldName = "XZDWKD";
            if (dltbClassName.ToUpper() == "LMFW")
            {
                kdFieldName = "KD";
            }
            //排除地类
            exceptDLBM.Clear();
            for (int k = 0; k < this.cmbDlbms.Properties.Items.Count; k++)
            {
                if (this.cmbDlbms.Properties.Items[k].CheckState == CheckState.Checked)
                {
                    exceptDLBM.Add(OtherHelper.GetLeftName(this.cmbDlbms.Properties.Items[k].ToString()));
                }
            }

            IFeatureClass dltbClass = pFeaLyr.FeatureClass;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                //计算范围
                if (this.radioGroupExtent.SelectedIndex == 0)
                {
                    //当前范围
                    //当前范围
                    IIdentify identify = currLayer as IIdentify;
                    IArray array = identify.Identify((this.currMap as IActiveView).Extent);
                    for (int i = 0; i < array.Count; i++)
                    {
                        IFeatureIdentifyObj idObj = array.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                        IFeature aFea = pRow.Row as IFeature;

                        string dlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                        if (exceptDLBM.Contains(dlbm))
                        {
                            double mj = (aFea.Shape as IArea).Area;
                            double len = (aFea.Shape as IPolygon).Length;
                            len = len / 2;
                            double kd = mj / len;

                            //2019-3月 线性图斑宽度  改为 线装地物宽度
                            //FeatureHelper.SetFeatureValue(aFea, "XXTBKD", MathHelper.RoundEx(kd,1) );
                            FeatureHelper.SetFeatureValue(aFea, kdFieldName, MathHelper.RoundEx(kd, 1));
                            aFea.Store();
                        }
                        lblstatus.Text = "当前已经计算" + i + "个...";
                        lblstatus.Update();
                    }
                }
                else if (this.radioGroupExtent.SelectedIndex == 1)
                {
                    int i=0;
                    foreach (IFeature aFea in arSelectFea)
                    {

                        string dlbm = FeatureHelper.GetFeatureStringValue(aFea, "DLBM");
                        if (exceptDLBM.Contains(dlbm))
                        {
                            double mj = (aFea.Shape as IArea).Area;
                            double len = (aFea.Shape as IPolygon).Length;
                            len = len / 2;
                            double kd = mj / len;
                            //2019-3月 线性图斑宽度  改为 线装地物宽度
                            //FeatureHelper.SetFeatureValue(aFea, "XXTBKD", MathHelper.RoundEx(kd, 1));
                            FeatureHelper.SetFeatureValue(aFea, kdFieldName, MathHelper.RoundEx(kd, 1));
                            aFea.Store();
                        }
                        lblstatus.Text = "当前已经计算" + (++i) + "个...";
                        lblstatus.Update();
                    }
                }
                

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("calxxtbkd");
                MessageBox.Show("计算完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }

        }

        IFeatureLayer currLayer = null;
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //得到该图层数量
            string className = this.cmbLayer.Text.Trim().Split('|')[0];
            currLayer = LayerHelper.QueryLayerByModelName(this.currMap, className);
            if (currLayer == null)
                return;
            try
            {
                IIdentify identify = currLayer as IIdentify;
                IFeatureSelection pFSelection = currLayer as IFeatureSelection;
                IArray array = identify.Identify((this.currMap as IActiveView).Extent);
                int allCount = array.Count;

                arSelectFea = LayerHelper.GetSelectedFeature(this.currMap, currLayer as IGeoFeatureLayer, currLayer.FeatureClass.ShapeType);
                this.lblNumbers.Text = "当前区域共" + allCount + "个要素。选中" + arSelectFea.Count + "个要素";
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
