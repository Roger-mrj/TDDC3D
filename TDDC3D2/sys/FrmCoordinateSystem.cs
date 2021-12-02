using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.sys
{
    public partial class FrmCoordinateSystem : Form
    {
        public ISpatialReference currSR = null;

        //常用地理坐标系统
        int[] geographicArry = { 4214, 4490, 4326, 4610 };
        Dictionary<string, string[]> projectedDic;

        public FrmCoordinateSystem()
        {
            InitializeComponent();

            this.searchControl1.Client = this.treeList;//设置搜索绑定
            treeList.OptionsBehavior.EnableFiltering = true;//开启过滤功能
            treeList.OptionsFilter.FilterMode = FilterMode.Smart;//过滤模式
            treeList.FilterNode += TreeList_FilterNode;

        }

        private void TreeList_FilterNode(object sender, FilterNodeEventArgs e)
        {
            if (treeList.DataSource == null)
            {
                return;
            }

            string nodeText = e.Node.GetDisplayText("坐标名称");
            if (string.IsNullOrWhiteSpace(nodeText))
            {
                return;
            }

            bool IsVisible = nodeText.ToUpper().IndexOf(searchControl1.Text.ToUpper()) >= 0;
            if (IsVisible)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode Node = e.Node;
                while (Node != null)
                {
                    if (!Node.Visible)
                    {
                        Node.Visible = true;
                        Node = Node.ParentNode;
                    }
                    else
                    {
                        break;
                    }
                }
                e.Node.Visible = true;

            }
            else
            {
                e.Node.Visible = false;
            }
            e.Handled = true;
        }

        private void FrmCoordinateSystem_Load(object sender, EventArgs e)
        {
            //初始化常见的投影坐标
            projectedDic = new Dictionary<string, string[]>();
            projectedDic.Add("CSCG2000", new string[] { "4491-4554" });
            projectedDic.Add("XIAN 1980", new string[] { "2327-2390" });
            projectedDic.Add("Beijing 1954", new string[] { "2431-2430", "21413-21423", "21473-21483" });
            InitTree();
        }

        private void InitTree()
        {
            try
            {
                //创建地理坐标系目录
                TreeListNode geographicNode = this.treeList.AppendNode(null, null);
                geographicNode.SetValue(0, "地理坐标系");
                geographicNode.Tag = "geographic";

                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                //添加常用的地理坐标系
                foreach (int numID in geographicArry)
                {
                    
                    ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem(numID);
                    TreeListNode subNode = treeList.AppendNode(spatialReference.Name, geographicNode);
                    subNode.SetValue(0, spatialReference.Name);
                    subNode.SetValue(1, "EPSG:" + spatialReference.FactoryCode.ToString());
                    subNode.Tag = "content";
                }

                //创建投影坐标系目录
                TreeListNode projectedNode = this.treeList.AppendNode(null, null);
                projectedNode.SetValue(0, "投影坐标系");
                projectedNode.Tag = "projected";

                //添加常用的投影坐标系
                foreach (KeyValuePair<string, string[]> projectedPair in projectedDic)
                {
                    TreeListNode fileNode = treeList.AppendNode(null, projectedNode);
                    fileNode.SetValue(0, projectedPair.Key);
                    fileNode.Tag = "file";
                    foreach (string wkid in projectedPair.Value)
                    {
                        string[] wkidList = wkid.Split('-');
                        for (int i = int.Parse(wkidList[0]); i <= int.Parse(wkidList[1]); i++)
                        {
                            ISpatialReference spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem(i);
                            TreeListNode subNode = treeList.AppendNode(spatialReference.Name, fileNode);
                            subNode.SetValue(0, spatialReference.Name);
                            subNode.SetValue(1, "EPSG:" + spatialReference.FactoryCode.ToString());
                            subNode.Tag = "content";
                        }
                    }
                }
                geographicNode.ExpandAll();
            }
            catch
            {
                MessageBox.Show("加载坐标数据失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void treeList_GetSelectImage(object sender, GetSelectImageEventArgs e)
        {
            if (e.Node == null) return;
            if (e.Node.Tag != null && e.Node.Tag.ToString() == "content")
            {
                e.NodeImageIndex = 1;
            }
        }

        private void treeList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //若是父节点则返回
                if (treeList.FocusedNode.Tag == null || treeList.FocusedNode.Tag.ToString()!= "content")
                {
                    return;
                }
                else
                {
                    string wkid = treeList.FocusedNode.GetValue("WKID").ToString();
                    if (!string.IsNullOrEmpty(wkid))
                    {
                        int numId = int.Parse(wkid.Split(':')[1]);
                        ISpatialReference spatialReference;
                        ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                        if (treeList.FocusedNode.ParentNode.Tag != null && treeList.FocusedNode.ParentNode.Tag.ToString() == "geographic")
                        {
                            spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem(numId);
                        }
                        else
                        {
                            spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem(numId);

                        }
                        this.currSR = spatialReference;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("获取坐标文件失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void treeList_Click(object sender, EventArgs e)
        {
            try
            {
                //若是父节点则返回
                if (treeList.FocusedNode.Tag == null || treeList.FocusedNode.Tag.ToString() != "content")
                {
                    return;
                }
                else
                {
                    string wkid = treeList.FocusedNode.GetValue("WKID").ToString();
                    if (!string.IsNullOrEmpty(wkid))
                    {
                        int numId = int.Parse(wkid.Split(':')[1]);
                        ISpatialReference spatialReference;
                        ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                        if (treeList.FocusedNode.ParentNode.Tag != null && treeList.FocusedNode.ParentNode.Tag.ToString() == "geographic")
                        {
                            spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem(numId);
                        }
                        else
                        {
                            spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem(numId);

                        }
                        this.currSR = spatialReference;
                        this.memoEdit1.Text = this.currSR.Name + "\r\n" + this.currSR.Abbreviation + "\r\n" + this.currSR.Remarks;
                    }
                }
            }
            catch
            {
                MessageBox.Show("获取坐标文件失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
