using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

using RCIS.GISCommon;

namespace TDDC3D.datado
{
    public partial class createTopologyFrm : Form
    {
        public createTopologyFrm()
        {
            InitializeComponent();
        }
        public IWorkspace currWS = null;
        private ITopology m_top;
        private double XYTolerance;//XY容差值

        private string Change(string str)
        {
            switch (str)
            {
                case "esriGeometryPoint":
                    str = "点状";
                    break;
                case "esriGeometryPolygon":
                    str = "面状";
                    break;
                case "esriGeometryPolyline":
                    str = "线状";
                    break;
                case "esriGeometryMultipoint":
                    str = "点状";
                    break;
                case "esriGeometryLine":
                    str = "线状";
                    break;
                default:
                    str = "其他";
                    break;
            }
            return str;
        }
        private void LoadLayers()
        {
            try
            {

                this.listView1.Items.Clear();
                IFeatureWorkspace pFWS = currWS as IFeatureWorkspace;
                IFeatureDataset pFeaDS = pFWS.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                IFeatureClassContainer featureClassContainer = pFeaDS as IFeatureClassContainer;
                for (int i = 0; i < featureClassContainer.ClassCount; i++)
                {
                    IFeatureClass pFC = featureClassContainer.get_Class(i);
                    IFeatureLayer pFeaLayer = new FeatureLayerClass();
                    pFeaLayer.FeatureClass = pFC;
                    if (pFeaLayer != null)
                    {
                        ITopologyContainer pTopC = pFeaDS as ITopologyContainer;
                        ITopology pTop;

                        bool b = false;

                        for (int j = 0; j < pTopC.TopologyCount; j++)
                        {
                            pTop = pTopC.get_Topology(j);

                            ITopologyProperties pTopPro;
                            pTopPro = (ITopologyProperties)pTop;
                            IEnumFeatureClass pEnumFC = pTopPro.Classes;
                            IFeatureClass existingTopoFC;
                            existingTopoFC = pEnumFC.Next();
                            while (existingTopoFC != null)
                            {
                                string sTopoFCAliasName = existingTopoFC.AliasName;
                                if (sTopoFCAliasName == pFeaLayer.FeatureClass.AliasName)
                                {
                                    b = true;
                                    break;
                                }

                                existingTopoFC = pEnumFC.Next();

                            }
                            if (b)
                                break;
                        }
                        if (!b)
                        {
                            ListViewItem lv = new ListViewItem();

                            lv.SubItems.Add(pFeaLayer.FeatureClass.AliasName);
                            lv.SubItems.Add(Change(pFeaLayer.FeatureClass.ShapeType.ToString()));
                            lv.SubItems.Add((pFC as IDataset).Name.ToUpper());
                            this.listView1.Items.Add(lv);

                        }

                    }

                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void createTopologyFrm_Load(object sender, EventArgs e)
        {
            LoadLayers();

            this.txtCluster.Text = "0.001";
            this.cmdCreate.Enabled = false;
        }

        private void cmdSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                this.listView1.Items[i].Checked = true;
            }
        }

        private void cmdUnSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                this.listView1.Items[i].Checked = !(this.listView1.Items[i].Checked);
            }
        }

        public bool IsNumeric(string str) //判断是否为数字(包括小数点)  
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"\d|\.");
            return reg1.IsMatch(str);
        }
        public bool IsRealNumeric(string str) //判断是否为纯字  
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"\d");
            return reg1.IsMatch(str);
        }

        private void txtCluster_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!this.IsNumeric(e.KeyChar.ToString()) && (int)(e.KeyChar) != 8)//如果不是数字和退格 
            {
                e.Handled = true;    //禁止显示 
            }
            else
            {
                if (txtCluster.Text.IndexOf(".") > 0) //如果已经有了一个小数点 
                {
                    if (!this.IsRealNumeric(e.KeyChar.ToString()) && (int)(e.KeyChar) != 8)
                        e.Handled = true;
                }
            }
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (this.listView2.CheckedItems.Count == 0)
            {
                MessageBox.Show("请首先选中需要移除的拓扑规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            for (int i = 0; i < this.listView2.CheckedItems.Count; i++)
            {
                this.listView2.CheckedItems[i].Remove();
            }
        }

        private void cmdAddRule_Click(object sender, EventArgs e)
        {
            this.xtraTabControl1.SelectedTabPageIndex = 2;
            LoadSelectItem();
        }

        private void LoadSelectItem()
        {
            if (this.listView1.CheckedItems.Count == 0) return;
            this.cboFC1.Properties.Items.Clear();
            for (int i = 0; i < this.listView1.CheckedItems.Count; i++)
            {

                this.cboFC1.Text = this.listView1.CheckedItems[0].SubItems[1].Text + "|" + this.listView1.CheckedItems[0].SubItems[2].Text + "-" + this.listView1.CheckedItems[0].SubItems[3].Text;
                this.cboFC1.Properties.Items.AddRange(new object[] {
					this.listView1.CheckedItems[i].SubItems[1].Text +"|"+this.listView1.CheckedItems[i].SubItems[2].Text+"-" +this.listView1.CheckedItems[i].SubItems[3].Text													  
												                  
														 });
            }
            this.cboFC1.SelectedIndex = -1;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdCancel1_Click(object sender, EventArgs e)
        {
            this.xtraTabControl1.SelectedTabPageIndex = 1;
        }

        private void cmdRemoveAll_Click(object sender, EventArgs e)
        {
            this.listView2.Items.Clear();
        }

        private void AddRules(string sShapeType)
        {
            if (sShapeType == "") return;

            this.cboRule.Properties.Items.Clear();
            switch (sShapeType)
            {
                case "点状":
                    this.cboRule.Text = "";


                    this.cboRule.Properties.Items.AddRange(new object[] {

                     "点被线的尾节点覆盖|"+esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint
												                  
														 });
                    break;
                case "线状":
                    this.cboRule.Text = "";
                    this.cboRule.Properties.Items.AddRange(new object[] {

                        "线的尾节点被点覆盖|"+esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint,
                        "不存在自相交的线|"+esriTopologyRuleType.esriTRTLineNoIntersection,
                        "无重合的线|"+esriTopologyRuleType.esriTRTLineNoOverlap,
                        "线被区域的边线覆盖|"+esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary,
                        "线不能悬挂|"+esriTopologyRuleType.esriTRTLineNoDangles 
												                  
														 });
                    break;
                case "面状":
                    this.cboRule.Text = "";
                    this.cboRule.Properties.Items.AddRange(new object[] {

                        "面是封闭的|"+esriTopologyRuleType.esriTRTAreaNoGaps,
                        "面不相交|"+esriTopologyRuleType.esriTRTAreaNoOverlap,
                        "不能与其他要素类重叠|"+esriTopologyRuleType.esriTRTAreaNoOverlapArea,
                        "一个面的边界被线覆盖|"+esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine,
                        "被另一个面包含|"+esriTopologyRuleType.esriTRTAreaCoveredByArea
                       					                  
														 });
                    break;
                default:
                    break;
            }
        }

        private void cboFC1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sShapeType = this.cboFC1.Text;
                this.cboRule.Properties.Items.Clear();
                sShapeType = sShapeType.Substring(sShapeType.IndexOf("|") + 1, (sShapeType.IndexOf("-") - sShapeType.IndexOf("|") - 1));
                this.AddRules(sShapeType);
                this.cboFC2.Properties.Items.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        #region 根据添加的规则获得FeatureClass
        private void AddFeatureClassForRules(string sRule)
        {
            sRule = sRule.Substring(sRule.IndexOf("|") + 1, (sRule.Length - sRule.IndexOf("|") - 1));

            switch (sRule)
            {

                case "esriTRTPointCoveredByLineEndpoint":
                    LoadSelectGeometryItem("线状");
                    break;
                case "esriTRTLineNoDangles":
                    LoadSelectGeometryItem("线状");
                    break;
                case "esriTRTLineEndpointCoveredByPoint":
                    LoadSelectGeometryItem("点状");
                    break;
                case "esriTRTLineCoveredByAreaBoundary":
                    LoadSelectGeometryItem("面状");
                    break;
                case "esriTRTLineNoOverlap":
                    LoadSelectGeometryItem("线状");
                    break;
                case "esriTRTLineNoSelfIntersect":
                    LoadSelectGeometryItem("线状");
                    break;
                case "esriTRTLineNoIntersection":
                    LoadSelectGeometryItem("线状");
                    break;

                case "esriTRTAreaNoGaps":
                    LoadSelectGeometryItem("面状");
                    break;
                case "esriTRTAreaNoOverlap":
                    LoadSelectGeometryItem("面状");
                    break;
                case "esriTRTAreaBoundaryCoveredByLine":
                    LoadSelectGeometryItem("线状");
                    break;
                case "esriTRTAreaCoveredByArea":
                    LoadSelectGeometryItem("面状");
                    break;
                case "esriTRTAreaNoOverlapArea":
                    LoadSelectGeometryItem("面状");
                    break;
                default:
                    break;
            }
        }
        private void LoadSelectGeometryItem(string sGeo)
        {
            if (this.listView1.CheckedItems.Count == 0) return;
            this.cboFC2.Properties.Items.Clear();
            for (int i = 0; i < this.listView1.CheckedItems.Count; i++)
            {
                if (this.listView1.CheckedItems[i].SubItems[2].Text == sGeo)
                {
                    this.cboFC2.Text = this.listView1.CheckedItems[i].SubItems[1].Text + "|" + this.listView1.CheckedItems[i].SubItems[2].Text + "-" + this.listView1.CheckedItems[i].SubItems[3].Text;
                    this.cboFC2.Properties.Items.AddRange(new object[] {
					  this.listView1.CheckedItems[i].SubItems[1].Text +"|"+this.listView1.CheckedItems[i].SubItems[2].Text+"-" +this.listView1.CheckedItems[i].SubItems[3].Text													  
												                  
														 });
                }
            }
        }
        #endregion 

        private void cboRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cboFC2.Properties.Items.Clear();
                AddFeatureClassForRules(this.cboRule.Text);
            }
            catch (Exception ex)
            { }
        }

        

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (this.xtraTabControl1.SelectedTabPageIndex== 2)
            {
                LoadSelectItem();
            }
            
        }

       
        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                #region 条件判断
                if (this.cboFC1.Text == "")
                {

                    MessageBox.Show("要素类为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                if (this.cboRule.Text == "")
                {

                    MessageBox.Show("拓扑规则为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion
                ListViewItem lv = new ListViewItem();
                string sFC1 = this.cboFC1.Text;
                sFC1 = sFC1.Substring(sFC1.IndexOf("-") + 1, sFC1.Length - sFC1.IndexOf("-") - 1);
                string sFC2 = this.cboFC2.Text;
                sFC2 = sFC2.Substring(sFC2.IndexOf("-") + 1, sFC2.Length - sFC2.IndexOf("-") - 1);
                string sRule = this.cboRule.Text;
                sRule = sRule.Substring(sRule.IndexOf("|") + 1, sRule.Length - sRule.IndexOf("|") - 1);
                lv.SubItems.Add(sFC1);
                lv.SubItems.Add(sRule);
                lv.SubItems.Add(sFC2);
                this.listView2.Items.Add(lv);
                this.xtraTabControl1.SelectedTabPageIndex = 1;
                this.cmdCreate.Enabled = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void cmdCreate_Click(object sender, EventArgs e)
        {

            #region 条件判断
            if (this.listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请首先选中需要建立拓扑的图层", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.txtTopoName.Text == "")
            {

                MessageBox.Show("拓扑名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtCluster.Text == "")
            {

                MessageBox.Show("容差值为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            double sCluster = 0.0;
            try
            {
                sCluster = Convert.ToDouble(this.txtCluster.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("容差值不是数字类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (sCluster < XYTolerance)
            {
                MessageBox.Show("你所设定的容差值不能小于数据集默认的容差值，将导致拓扑出错", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IWorkspace2 m_space = this.currWS as IWorkspace2;
            if (m_space.get_NameExists(esriDatasetType.esriDTTopology, this.txtTopoName.Text) == true)
            {
                MessageBox.Show("该拓扑名称已经存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.listView2.Items.Count == 0)
            {
                MessageBox.Show("请添加拓扑规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion
            this.Cursor = Cursors.WaitCursor;
            try
            {
                
                               
                IFeatureWorkspace pFWS = this.currWS as IFeatureWorkspace;

                IDataset pDS = pFWS.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
                IVersionedObject3 vObj = pDS as IVersionedObject3;
                if (vObj!=null &&  vObj.IsRegisteredAsVersioned)
                {
                    MessageBox.Show("数据库已经注册版本，要建立拓扑需要撤消版本！\n 如果撤销版本请首先进行当前数据备份.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ITopologyContainer pTopC = pDS as ITopologyContainer;
                IFeatureClassContainer pFCC = (IFeatureClassContainer)pDS;
                //ProgressForm formP = ProgressForm.StartProgressThread("正在进行拓扑关系构建，请稍等...");
               

                ITopology top = pTopC.CreateTopology(this.txtTopoName.Text, sCluster, -1, "");

                IFeatureClass pFC;

                for (int i = 0; i < this.listView1.CheckedItems.Count; i++)
                {
                    pFC = pFCC.get_ClassByName(this.listView1.CheckedItems[i].SubItems[3].Text);

                    top.AddClass(pFC, 5, 1, 1, false);
                }
                ITopologyRuleContainer topologyRuleContainer;
                ITopologyRule topologyRule;
                IFeatureClass pOriginFC;//源要素类
                IFeatureClass pDestinationFC;//目标要素类
              
                for (int i = 0; i < this.listView2.Items.Count; i++)
                {
                    // Create rule 1 of 2   

                    topologyRuleContainer = (ITopologyRuleContainer)top;
                    topologyRule = new TopologyRuleClass();
                    pOriginFC = pFCC.get_ClassByName(this.listView2.Items[i].SubItems[1].Text);
                    pDestinationFC = pFCC.get_ClassByName(this.listView2.Items[i].SubItems[3].Text);
                    topologyRule.TopologyRuleType = (esriTopologyRuleType)Enum.Parse(typeof(esriTopologyRuleType), this.listView2.Items[i].SubItems[2].Text);
                    topologyRule.OriginClassID = pOriginFC.ObjectClassID;
                    topologyRule.DestinationClassID = pDestinationFC.ObjectClassID;
                    topologyRule.AllOriginSubtypes = true;
                    topologyRule.AllDestinationSubtypes = true;
                    topologyRule.Name = this.listView2.Items[i].SubItems[1].Text + " " + this.listView2.Items[i].SubItems[2].Text + " " + this.listView2.Items[i].SubItems[3].Text;
                    // Add the rule to the Topology    
                    if (topologyRuleContainer.get_CanAddRule(topologyRule))
                    {
                        topologyRuleContainer.AddRule(topologyRule);
                    }
                }

                LoadLayers();
                this.Cursor = Cursors.Default;
                MessageBox.Show("拓扑创建完毕", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                

            }
            catch (Exception ee)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("" + ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
