using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.sys
{
    public partial class LayerSelectForm : Form
    {
        public LayerSelectForm()
        {
            InitializeComponent();
        }

        public IWorkspace sendWs = null;
        public List<string> retFeaClassName = new List<string>();
        public List<string> _loadLayerStr = new List<string>(); //已经加载的layer
        List<string> _notNullLayers = new List<string>();


        private string[] checkStr = new string[]
        { "DLTB", "XZQ", "XZQJX", "PDT", "CZCDYD", "CZCDYDGX", "PZWJSTD", "CJDCQ", "CJDCQJX", "CCWJQ", "GFBQ", "LSYD",
            "DLTBGX", "DLTBGXGC", "CJDCQGX", "CJDCQGXGC", "XZQGX", "XZQGXGC", "CJDCQJXGX", "XZQJXGX",
            "FLDY", "KCFLDY","FZTB"
        };

        /// <summary>        
        /// 选择子节点时触发        
        /// </summary>        
        /// <param name="node"></param>       
        /// /// <param name="check"></param>        
        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].Tag != null && node.Nodes[i].Tag.ToString() == "1")
                {
                    continue;
                }

                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        /// <summary>        
        /// 选择父节点时触发        
        /// </summary>       
        /// /// <param name="node"></param>        
        // /<param name="check"></param>        
        private void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }

        }

        private void LayerSelectForm_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dicAllDataset = new Dictionary<string, string>();
                IEnumDataset DSs = sendWs.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureDataset);
                DSs.Reset();
                IDataset ds = null;
                while ((ds = DSs.Next()) != null)
                {
                    //所有数据集
                    dicAllDataset.Add(ds.Name.ToUpper(), ds.BrowseName.ToUpper());
                }
                if (dicAllDataset.Count == 0)
                {
                    MessageBox.Show("打开不是标准库，相关建库功能无法使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                foreach (KeyValuePair<string, string> aItem in dicAllDataset)
                {
                    TreeListNode rootNode = this.treeList1.AppendNode(aItem.Key.ToString().Trim(), null);
                    rootNode.SetValue(0, aItem.Key.ToString());
                    rootNode.SetValue(1, aItem.Value.ToString());

                    //获取子节点
                    IFeatureDataset pFeaDs = (sendWs as IFeatureWorkspace).OpenFeatureDataset(aItem.Key.ToString());
                    IFeatureClassContainer pClassContaniner = pFeaDs as IFeatureClassContainer;
                    for (int i = 0; i < pClassContaniner.ClassCount; i++)
                    {
                        IFeatureClass pFC = pClassContaniner.get_Class(i);
                        IDataset pFCDs = pFC as IDataset;
                        string fcName = pFCDs.Name;
                        string alias = pFC.AliasName;
                        if (pFC.FeatureCount(null) > 0)
                        {
                            _notNullLayers.Add(fcName);
                        }

                        TreeListNode subNode = treeList1.AppendNode(fcName, rootNode);
                        subNode.SetValue(0, fcName);
                        subNode.SetValue(1, alias);
                        if (_loadLayerStr != null && _loadLayerStr.Count > 0)
                        {
                            if (_loadLayerStr.Contains(fcName))
                            {
                                subNode.Tag = 1;
                                subNode.Checked = true;
                                SetCheckedParentNodes(subNode, CheckState.Checked);

                            }
                        }
                        else if (checkStr != null && checkStr.Contains(fcName))
                        {
                            subNode.Checked = true;
                            SetCheckedParentNodes(subNode, CheckState.Checked);
                        }
                    }
                }
                treeList1.ExpandAll();
            }
            catch
            {
                MessageBox.Show("初始化图层树失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //List<IFeatureClass> lstClasses = new List<IFeatureClass>();
            //IEnumDataset DSs = sendWs.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
            //IFeatureWorkspace fws = sendWs as IFeatureWorkspace;
            //DSs.Reset();
            //IDataset ds = DSs.Next();
            //while (ds != null)
            //{
            //    if (ds.Type == esriDatasetType.esriDTFeatureClass)
            //    {
            //        IFeatureClass curFeatCls = ds as IFeatureClass;
            //        lstClasses.Add(curFeatCls);

            //    }
            //    else if (ds.Type == esriDatasetType.esriDTFeatureDataset)
            //    {
            //        //提取出其内的FeatureClass:

            //        IFeatureDataset Feats = ds as IFeatureDataset;
            //        IFeatureClassContainer FeatCon = (IFeatureClassContainer)Feats;
            //        IEnumFeatureClass featClasses = FeatCon.Classes;

            //        featClasses.Reset();
            //        IFeatureClass myFeatClass = featClasses.Next();
            //        while (myFeatClass != null)
            //        {
            //            lstClasses.Add(myFeatClass);
            //            myFeatClass = featClasses.Next();
            //        } //while(...
            //    }
            //    ds = DSs.Next();
            //}

            //this.chkListLayers.Items.Clear();
            //foreach (IFeatureClass aFC in lstClasses)
            //{
            //    string className = (aFC as IDataset).Name.ToUpper();
            //    this.chkListLayers.Items.Add(className + "|" + aFC.AliasName);
            //}


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                TreeListNodes parentNodes = this.treeList1.Nodes;
                bool isLoadNullLayer = false;
                foreach (TreeListNode aRuleGroup in parentNodes)
                {
                    TreeListNodes ruleNodes = aRuleGroup.Nodes;
                    foreach (TreeListNode aRule in ruleNodes)
                    {
                        string fcName = aRule.GetValue(0).ToString();

                        if (aRule.CheckState == CheckState.Checked && !_loadLayerStr.Contains(fcName) && !_notNullLayers.Contains(fcName))
                        {
                            if (DialogResult.Cancel == MessageBox.Show("加载图层中包含空图层,可能会导致\n\r地图范围异常,是否移除空图层？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                            {
                                isLoadNullLayer = true;
                            }
                            goto endForeach;
                        }
                    }
                }

                endForeach:;

                foreach (TreeListNode aRuleGroup in parentNodes)
                {
                    TreeListNodes ruleNodes = aRuleGroup.Nodes;
                    foreach (TreeListNode aRule in ruleNodes)
                    {
                        string fcName = aRule.GetValue(0).ToString();

                        if (aRule.CheckState == CheckState.Checked && !_loadLayerStr.Contains(fcName))
                        {
                            if (!isLoadNullLayer && !_notNullLayers.Contains(fcName))
                            {
                                continue;
                            }
                            this.retFeaClassName.Add(fcName);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("加载图层数据失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList1_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag.ToString() == "1")
            {
                e.CanCheck = false;
            }
            else
            {
                e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void rdbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            TreeListNodes nodes = this.treeList1.Nodes;
            foreach (TreeListNode node in nodes)
            {
                if (node.Tag != null && node.Tag.ToString() == "1")
                {
                    continue;
                }
                node.CheckState = CheckState.Checked;
                SetCheckedChildNodes(node, CheckState.Checked);
            }
        }

        private void rdbSelectNull_CheckedChanged(object sender, EventArgs e)
        {
            TreeListNodes nodes = this.treeList1.Nodes;
            foreach (TreeListNode node in nodes)
            {
                node.CheckState = CheckState.Unchecked;
                SetCheckedChildNodes(node, CheckState.Unchecked);
            }
        }

        private void rdbSelectNotNull_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbSelectNotNull.Checked)
            {
                rdbSelectNull_CheckedChanged(sender, e);

                foreach (string item in _notNullLayers)
                {
                    TreeListNode node = this.treeList1.FindNodeByFieldValue("FCNAME", item);
                    if (node.Tag != null && node.Tag.ToString() == "1")
                    {
                        continue;
                    }
                    node.CheckState = CheckState.Checked;
                    SetCheckedParentNodes(node, CheckState.Checked);
                }
            }
        }

        private void treeList1_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {

            if (e.Node.Tag != null && e.Node.Tag.ToString() == "1")
            {
                e.Appearance.BackColor = Color.LightGray;
                e.Appearance.Options.UseBackColor = true;
            }
        }

        private void treeList1_CustomDrawNodeCheckBox(object sender, DevExpress.XtraTreeList.CustomDrawNodeCheckBoxEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag.ToString() == "1")
            {
                e.ObjectArgs.State = DevExpress.Utils.Drawing.ObjectState.Disabled;
            }
        }
    }
}
