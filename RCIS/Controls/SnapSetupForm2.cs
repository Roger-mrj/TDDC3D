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
using ESRI.ArcGIS.Controls;
using RCIS.Global;
using RCIS.GISCommon;

namespace RCIS.Controls
{
    public partial class SnapSetupForm2 : Form
    {
        public SnapSetupForm2()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        IEngineSnapEnvironment snapEnvironment = null;

        public IEngineEditor globalEngineEditor;
        private int  getAddedAgent(string sLayerName)
        {

            for (int i = 0; i < snapEnvironment.SnapAgentCount; i++)
            {
                IEngineFeatureSnapAgent featureSnapAgent = snapEnvironment.get_SnapAgent(i) as IEngineFeatureSnapAgent;
                
                string snapClassName = LayerHelper.GetClassShortName(featureSnapAgent.FeatureClass as IDataset);
                if (sLayerName.ToUpper().Equals(snapClassName.Trim().ToUpper()))
                {
                    return i;
                }
            }
            return -1;
        }


        private bool bFist = false;


        private void AddANode(ILayer currLyr)
        {
           

            //第一层儿子节点
            IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            if (currFeaLyr== null) return;
            IFeatureClass currClass = currFeaLyr.FeatureClass;
            string clsName = (currClass as IDataset).Name.ToUpper();

            TreeNode classNode = new TreeNode(clsName);
            int agentIdx = getAddedAgent(clsName);
            classNode.Checked = (agentIdx > -1);

            //加入孙子节点
            TreeNode childNode1 = new TreeNode("节点");
            childNode1.Tag = esriGeometryHitPartType.esriGeometryPartVertex;

            TreeNode childNode2 = new TreeNode("端点");
            childNode2.Tag = esriGeometryHitPartType.esriGeometryPartEndpoint;

            TreeNode childNode3 = new TreeNode("边界");
            childNode3.Tag = esriGeometryHitPartType.esriGeometryPartBoundary;


            if (classNode.Checked)
            {
                IEngineFeatureSnapAgent agent = this.snapEnvironment.get_SnapAgent(agentIdx) as IEngineFeatureSnapAgent;
                if ((agent.HitType ^ esriGeometryHitPartType.esriGeometryPartBoundary ^ esriGeometryHitPartType.esriGeometryPartEndpoint) == esriGeometryHitPartType.esriGeometryPartNone)
                {
                    childNode1.Checked = false;
                }
                else childNode1.Checked = true;
                if ((agent.HitType ^ esriGeometryHitPartType.esriGeometryPartBoundary ^ esriGeometryHitPartType.esriGeometryPartVertex) == esriGeometryHitPartType.esriGeometryPartNone)
                {
                    childNode2.Checked = false;
                }
                else childNode2.Checked = true;

                if ((agent.HitType ^ esriGeometryHitPartType.esriGeometryPartEndpoint ^ esriGeometryHitPartType.esriGeometryPartVertex) == esriGeometryHitPartType.esriGeometryPartNone)
                {
                    childNode3.Checked = false;
                }
                else childNode3.Checked = true;

            }
            classNode.Nodes.Add(childNode1);
            classNode.Nodes.Add(childNode2);
            classNode.Nodes.Add(childNode3);

            this.tvSnap.Nodes.Add(classNode);
        }
        private void SnapSetupForm2_Load(object sender, EventArgs e)
        {
            snapEnvironment =Global.GlobalEditObject.CurrentEngineEditor  as IEngineSnapEnvironment;
            this.tvSnap.Nodes.Clear();
            bFist = true;
            for (int i = 0; i < currMap.LayerCount; i++)
            {
                ILayer currLyr = this.currMap.get_Layer(i);
                if (currLyr is IFeatureLayer)
                {
                    if (!currLyr.Visible) continue;
                    AddANode(currLyr as ILayer);
                }
                else if (currLyr is IGroupLayer)
                {
                    ICompositeLayer pCLyr = currLyr as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr.Visible && childLyr is IFeatureLayer)
                        {
                            AddANode(childLyr);
                        }
                    }
                }

                


            }
            if (this.tvSnap.Nodes.Count > 0)
            {
                this.tvSnap.Nodes[0].Expand();
            }
            bFist = false;
        }

        private void tvSnap_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //为叶子节点
            if (bFist)
                return;

            if (e.Node.Nodes.Count == 0)
            {
                TreeNode parentNode = e.Node.Parent;
                string className = parentNode.Text.Trim();
                int idx = getAddedAgent(className);
                if (e.Node.Checked)
                {
                    if (idx > -1)
                    {
                        IEngineFeatureSnapAgent featureSnapAgent = this.snapEnvironment.get_SnapAgent(idx) as IEngineFeatureSnapAgent;
                        featureSnapAgent.HitType |= (esriGeometryHitPartType)e.Node.Tag;
                    }
                    
                }
                else
                {
                    if (idx > -1)
                    {
                        //否则只移除该类型
                        IEngineFeatureSnapAgent featureSnapAgent = this.snapEnvironment.get_SnapAgent(idx) as IEngineFeatureSnapAgent;
                        esriGeometryHitPartType type = (esriGeometryHitPartType)e.Node.Tag;
                        featureSnapAgent.HitType = featureSnapAgent.HitType ^ type;
                    }
                    
                }

            }
            else
            {
                string className = e.Node.Text.Trim();
                if (e.Node.Checked)
                {
                    IEngineFeatureSnapAgent featureSnapAgent = new EngineFeatureSnap();

                    IFeatureLayer pFeaLyr = LayerHelper.QueryLayerByModelName(this.currMap, className);
                    IFeatureClass pFeatClass = pFeaLyr.FeatureClass;
                    featureSnapAgent.FeatureClass = pFeatClass;
                    featureSnapAgent.HitType = esriGeometryHitPartType.esriGeometryPartVertex | esriGeometryHitPartType.esriGeometryPartEndpoint | esriGeometryHitPartType.esriGeometryPartBoundary;
                    snapEnvironment.AddSnapAgent(featureSnapAgent);

                    foreach (TreeNode aNode in e.Node.Nodes)
                    {
                        aNode.Checked = true;
                    }

                    ((IEngineEditProperties2)GlobalEditObject.CurrentEngineEditor).SnapTips = true;
                    
                    
                }
                else
                {
                    foreach (TreeNode aNode in e.Node.Nodes)
                    {
                        aNode.Checked = false;
                    }
                    int idx = getAddedAgent(className);
                    if (idx > -1)
                    {
                        snapEnvironment.RemoveSnapAgent(idx);
                        
                    }
                    
                }
            }
        }

      
     
    }
}
