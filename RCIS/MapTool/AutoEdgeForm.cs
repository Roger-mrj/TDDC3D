using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using System.Collections.Generic;
using RCIS.Utility;

namespace RCIS.MapTool
{
    public partial class AutoEdgeForm : Form
    {
        public AutoEdgeForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IMapControl3 currMapCtl = null;
        public IEnvelope pEnv = null;

        public double distance
        {
            get {
                double d = 0;
                double.TryParse(this.txtDistance.Text, out d);
                d = d / 100.0;
                return d;
            }
        }

        public List<IFeature> lstXzqFeatures = null;

        private void AutoEdgeForm_Load(object sender, EventArgs e)
        {
            LayerHelper.LoadLayer2Combox(this.cmbDltbLayer, this.currMap, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
            this.cmbmasterXzq.Properties.Items.Clear();
            this.cmbSecondXZQ.Properties.Items.Clear();
            foreach (IFeature aXzq in lstXzqFeatures)
            {
                this.cmbmasterXzq.Properties.Items.Add(FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM") + "|" + FeatureHelper.GetFeatureStringValue(aXzq, "XZQMC"));
                this.cmbSecondXZQ.Properties.Items.Add(FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM") + "|" + FeatureHelper.GetFeatureStringValue(aXzq, "XZQMC"));
            }
            this.radioGroup1.SelectedIndex = 2;

        }

        private void cmbmasterXzq_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        List<IFeature> dltbA = new List<IFeature>();
        List<IFeature> dltbB = new List<IFeature>();
        Dictionary<IPoint, IPoint> dicPts = new Dictionary<IPoint, IPoint>();

        private int getIdx(IPointCollection ptCol, IPoint targetPt)
        {
            IRelationalOperator pRO = targetPt as IRelationalOperator; 
            int idx = -1;
            for (int i = 0; i < ptCol.PointCount; i++)
            {
                if (pRO.Equals(ptCol.get_Point(i)))
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            Dictionary<IPoint, IPoint> selPts = new Dictionary<IPoint, IPoint>();
            foreach (TreeNode aNode in treeView1.Nodes[0].Nodes)
            {
                if (aNode.Checked)
                {
                    IPoint aPta = aNode.Tag as IPoint;
                    selPts.Add(aPta, dicPts[aPta]);
                }
            }
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {
                foreach (KeyValuePair<IPoint, IPoint> aItem in selPts)
                {
                    
                    
                    IPoint key = aItem.Key;
                    IPoint val = aItem.Value;
                    IPoint targetPt = new PointClass();
                    if (this.radioGroup1.SelectedIndex == 0)
                    {
                        targetPt = key;
                    }
                    else if (this.radioGroup1.SelectedIndex == 1)
                    {
                        targetPt = val;
                    }
                    else
                    {
                        //计算得到一个中间点
                        targetPt.PutCoords((key.X + val.X) / 2, (key.Y + val.Y) / 2);
                    }
                    
                    

                    //DLTB 中包含A点的 所有图版，A点替换为 中间点
                    foreach(IFeature aFea in  dltbA)
                    {
                        IPolygon geo = aFea.ShapeCopy as IPolygon ;
                        IPointCollection ptCol = geo as IPointCollection;                        
                        //找到这个点的索引
                        int idx = this.getIdx(ptCol, aItem.Key);
                        if (idx > -1)
                        {
                            ptCol.UpdatePoint(idx, targetPt);
                            aFea.Shape = ptCol as IPolygon;
                            aFea.Store();
                        }
                        

                    }

                    //DLTBB 中 也随之替换
                    foreach (IFeature aFea in dltbB)
                    {
                        IPolygon geo = aFea.ShapeCopy as IPolygon;
                        IPointCollection ptCol = geo as IPointCollection;
                        //找到这个点的索引
                        int idx = this.getIdx(ptCol, aItem.Value);
                        if (idx > -1)
                        {
                            ptCol.UpdatePoint(idx, targetPt);
                            aFea.Shape = ptCol as IPolygon;
                            aFea.Store();
                        }
                        

                    }


                }
                
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("doedge");
                this.currMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }



        }

        private void cmbSecondXZQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        //取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
                parentNode.Checked = state;
                if (currNode.Parent.Parent != null)
                {
                    setParentNodeCheckedState(currNode.Parent, state);
                }
        }
        //选中节点之后，选中节点的所有子节点
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes=currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            } 
        }

       


        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            } 
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode == null) return;
            IPoint currPt = this.treeView1.SelectedNode.Tag as IPoint;          
            
            this.currMapCtl.FlashShape(currPt as IGeometry);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            IEnvelope currEnv = this.currMapCtl.ActiveView.Extent;

            string masterxzqdm = OtherHelper.GetLeftName(this.cmbmasterXzq.Text);
            string secondXzqdm = OtherHelper.GetLeftName(this.cmbSecondXZQ.Text);
            IGeometry masterGeo = null;
            IGeometry secondGeo = null;
            foreach (IFeature aFea in lstXzqFeatures)
            {
                string dm = FeatureHelper.GetFeatureStringValue(aFea, "XZQDM");
                if (dm == masterxzqdm)
                {
                    masterGeo = aFea.ShapeCopy;
                }
                else if (dm == secondXzqdm)
                {
                    secondGeo = aFea.ShapeCopy;
                }
            }
            if (masterGeo == null || secondGeo == null) return;
            IFeatureLayer dltbLyr = LayerHelper.QueryLayerByModelName(this.currMap, OtherHelper.GetLeftName(this.cmbDltbLayer.Text));
            if (dltbLyr == null) return;
            IFeatureClass dltbClass = dltbLyr.FeatureClass;
            //两个xzq 的公共边界
            ITopologicalOperator pTopmaster = masterGeo as ITopologicalOperator;
            IGeometry commonXzq = pTopmaster.Intersect(secondGeo, esriGeometryDimension.esriGeometry1Dimension);
            //一条线
            if (commonXzq.IsEmpty) return;
            IPolyline pCommonLine = commonXzq as IPolyline;
            ITopologicalOperator pTopCommon = pCommonLine as ITopologicalOperator;



            dltbA = GetFeaturesHelper.getFeaturesByGeo(dltbClass, pCommonLine, esriSpatialRelEnum.esriSpatialRelIntersects);
            for (int i = dltbA.Count - 1; i >= 0; i--)
            {
                IFeature aDltb = dltbA[i];
                string Zldwdm = FeatureHelper.GetFeatureStringValue(aDltb, "ZLDWDM");
                Zldwdm = Zldwdm.Substring(0, 12); //村A
                if (Zldwdm == masterxzqdm)
                {
                }
                else if (Zldwdm == secondXzqdm)
                {
                    dltbB.Add(aDltb);
                    dltbA.RemoveAt(i);
                }

            }

            //A 区内地类图斑 与公共边界的交点，
            List<IPoint> ptsA = new List<IPoint>();
            foreach (IFeature aTb in dltbA)
            {
                IGeometry interGeo = pTopCommon.Intersect(aTb.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                if (!interGeo.IsEmpty)
                {
                    IPointCollection ptCol = interGeo as IPointCollection;
                    for (int kk = 0; kk < ptCol.PointCount; kk++)
                    {
                        if ((currEnv as IRelationalOperator).Contains(ptCol.get_Point(kk)))
                            ptsA.Add(ptCol.get_Point(kk));
                    }
                }
                else
                {
                    interGeo = pTopCommon.Intersect(aTb.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension);
                    if (!interGeo.IsEmpty)
                    {
                        IPointCollection ptCol = interGeo as IPointCollection;
                        for (int kk = 0; kk < ptCol.PointCount; kk++)
                        {
                            if ((currEnv as IRelationalOperator).Contains(ptCol.get_Point(kk)))
                                ptsA.Add(ptCol.get_Point(kk));
                        }
                    }
                }
            }

            //b区内地类图版与公共边界的交点
            List<IPoint> ptsB = new List<IPoint>();
            foreach (IFeature aTb in dltbB)
            {
                IGeometry interGeo = pTopCommon.Intersect(aTb.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                if (!interGeo.IsEmpty)
                {
                    IPointCollection ptCol = interGeo as IPointCollection;
                    for (int kk = 0; kk < ptCol.PointCount; kk++)
                    {
                        if ((currEnv as IRelationalOperator).Contains(ptCol.get_Point(kk)))
                            ptsB.Add(ptCol.get_Point(kk));
                    }
                }
                else
                {
                    interGeo = pTopCommon.Intersect(aTb.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension);
                    if (!interGeo.IsEmpty)
                    {
                        IPointCollection ptCol = interGeo as IPointCollection;
                        for (int kk = 0; kk < ptCol.PointCount; kk++)
                        {
                            if ((currEnv as IRelationalOperator).Contains(ptCol.get_Point(kk)))
                                ptsB.Add(ptCol.get_Point(kk));
                        }
                    }
                }
            }

            //遍历A交点，在B区内找相近的A1，
            dicPts.Clear();
            foreach (IPoint apt in ptsA)
            {
                IProximityOperator po = apt as IProximityOperator;
                foreach (IPoint bpt in ptsB)
                {
                    if (po.ReturnDistance(bpt) == 0)
                        continue;
                    if (po.ReturnDistance(bpt) < this.distance)
                    {
                        if (!dicPts.ContainsKey(apt))
                        {
                            dicPts.Add(apt, bpt);
                        }
                    }
                }
            }
            this.treeView1.Nodes[0].Nodes.Clear();
            foreach (KeyValuePair<IPoint, IPoint> aItem in dicPts)
            {
                TreeNode node = new TreeNode();
                node.Text = "A|X:" + (aItem.Key as IPoint).X.ToString("F6") + ",Y:" + (aItem.Value as IPoint).Y.ToString("F6")
                    +"   B:|X:"+(aItem.Value as IPoint).X.ToString("F6")+",Y:"+(aItem.Value as IPoint).Y.ToString("F6");
                node.Tag = aItem.Key;
                this.treeView1.Nodes[0].Nodes.Add(node);
            }
            this.treeView1.Nodes[0].Expand();
        }


    }
}
