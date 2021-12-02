using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Data;

namespace RCIS.GISCommon
{
    public class ControlHelper
    {
        public static void LoadXZQTree(TreeView treeView, IWorkspace ws3D)
        {
            IFeatureWorkspace pFWs = ws3D as IFeatureWorkspace;
            Dictionary<string, string> xzqs = FeatureHelper.GetDMMCDicByQueryDef(pFWs, "XZQ", "XZQDM", "XZQMC");
            Dictionary<string, string> cjdcqs = FeatureHelper.GetDMMCDicByQueryDef(pFWs, "CJDCQ", "ZLDWDM", "ZLDWMC");
            TreeNode rootNode = null;
            foreach (KeyValuePair<string,string> item in xzqs)
            {
                if (treeView.Nodes.Count == 0)
                {
                    DataRow dr = RCIS.Database.LS_SetupMDBHelper.GetDataRow("Select MC From SYS_XZQ Where DM = '" + item.Key.Substring(0, 6) + "'", "tmp");
                    if (dr == null)
                    {
                        rootNode = treeView.Nodes.Add(item.Key.Substring(0, 6) + "|全县");
                    }
                    else
                    {
                        rootNode = treeView.Nodes.Add(item.Key.Substring(0, 6) + "|" + dr[0].ToString());
                    }
                }
                TreeNode xzqNode = rootNode.Nodes.Add(item.Key + "|" + item.Value);
                var subCJDCQ = from a in cjdcqs
                               where a.Key.StartsWith(item.Key) 
                               select a;
                foreach (var subitem in subCJDCQ)
                {
                    xzqNode.Nodes.Add(subitem.Key.Substring(0, 12) + "|" + subitem.Value);
                }
            }
            if (rootNode != null) rootNode.Expand();
        }

        public static void LoadXZQTreeBGH(TreeView treeView, IWorkspace ws3D)
        {
            IFeatureWorkspace pFWs = ws3D as IFeatureWorkspace;
            Dictionary<string, string> xzqbgq = FeatureHelper.GetDMMCDicByQueryDef(pFWs, "XZQ", "XZQDM", "XZQMC");
            Dictionary<string, string> xzqbgh = FeatureHelper.GetDMMCDicByQueryDef(pFWs, "XZQGX", "XZQDM", "XZQMC");
            Dictionary<string, string> cjdcqbgq = FeatureHelper.GetDMMCDicByQueryDef(pFWs, "CJDCQ", "ZLDWDM", "ZLDWMC");
            Dictionary<string, string> cjdcqbgh = FeatureHelper.GetDMMCDicByQueryDef(pFWs, "CJDCQGX", "ZLDWDM", "ZLDWMC");
            Dictionary<string, string> xzqs = xzqbgq.Union(xzqbgh).ToDictionary(m => m.Key, m => m.Value);
            //Dictionary<string, string> cjdcqs = cjdcqbgq.Union(cjdcqbgh).ToDictionary(k => k.Key, k => k.Value);
            Dictionary<string, string> cjdcqs = cjdcqbgq;
            foreach (string item in cjdcqbgh.Keys)
            {
                if (cjdcqs.Keys.Contains(item))
                    cjdcqs[item] = cjdcqbgh[item];
                else
                    cjdcqs.Add(item, cjdcqbgh[item]);
            }


            TreeNode rootNode = null;
            foreach (KeyValuePair<string, string> item in xzqs)
            {
                if (treeView.Nodes.Count == 0) 
                {
                    DataRow dr = RCIS.Database.LS_SetupMDBHelper.GetDataRow("Select MC From SYS_XZQ Where DM = '" + item.Key.Substring(0, 6) + "'", "tmp");
                    if (dr == null)
                    {
                        rootNode = treeView.Nodes.Add(item.Key.Substring(0, 6) + "|全县");
                    }
                    else
                    {
                        rootNode = treeView.Nodes.Add(item.Key.Substring(0, 6) + "|" + dr[0].ToString());
                    }
                }
                TreeNode xzqNode = rootNode.Nodes.Add(item.Key + "|" + item.Value);
                var subCJDCQ = from a in cjdcqs
                               where a.Key.StartsWith(item.Key)
                               select a;
                foreach (var subitem in subCJDCQ)
                {
                    xzqNode.Nodes.Add(subitem.Key.Substring(0, 12) + "|" + subitem.Value);
                }
            }
            if (rootNode != null) rootNode.Expand();
        }
    }
}
