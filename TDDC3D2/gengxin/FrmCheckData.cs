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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.gengxin
{
    public partial class FrmCheckData : Form
    {
        public AxMapControl mapControl;
        List<string> roles;

        public FrmCheckData()
        {
            InitializeComponent();
        }

        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void SetCheckedParentNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, CheckState check)
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
                if (b)
                {
                    node.ParentNode.CheckState = CheckState.Indeterminate;
                }
                else
                {
                    node.ParentNode.CheckState = check;
                }
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }  

        private void treCheckOption_MouseClick(object sender, MouseEventArgs e)
        {
            //TreeListHitInfo hitInfo = treCheckOption.CalcHitInfo(new Point(e.X, e.Y));
            //if (hitInfo.Node != null)
            //    hitInfo.Node.Checked = !hitInfo.Node.Checked;
        }

        private void FrmCheckData_Load(object sender, EventArgs e)
        {
            roles = new List<string>();
            roles.Add("地类名称与编码");
            roles.Add("权属性质");
            roles.Add("权属单位代码长度（19位）");
            roles.Add("坐落单位代码长度（19位）");
            roles.Add("扣除地类编码（必须是1203，同时地类是耕地）");
            roles.Add("扣除系数（必须是0<=x<1，同时地类是耕地）");
            roles.Add("耕地类型（地类是耕地）");
            roles.Add("耕地坡度级别（必须是1至5，同时地类是耕地）");
            roles.Add("线状地物宽度（地类必须是1001,1003,1004,1006,1009,1101,1107）");
            roles.Add("图斑细化代码和名称");
            roles.Add("种植属性代码和名称");
            treCheckOption.Nodes.Clear();
            TreeListNode treNode = treCheckOption.Nodes.Add("检查内容");
            treNode.SetValue(treCheckOption.Columns[0], "检查内容");
            foreach (string item in roles)
            {
                TreeListNode childNode = treNode.Nodes.Add(item);
                childNode.SetValue(treCheckOption.Columns[0], item);
            }
            treCheckOption.Nodes[0].ExpandAll();
        }

        private void txtDLTBBH_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog openDia = new OpenFileDialog();
            openDia.Filter = "SHP文件（*.shp）|*.shp";
            if (openDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDLTBBH.Text = openDia.FileName;
            }
        }

        private void treCheckOption_AfterCheckNode(object sender, NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);  
        }

        private void treCheckOption_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            if (e.PrevState == CheckState.Checked)
            {
                e.State = CheckState.Unchecked;
            }
            else
            {
                e.State = CheckState.Checked;
            }  
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDLTBBH.Text))
            {
                MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("开始检查");
            DataTable dtDL = RCIS.Database.LS_SetupMDBHelper.GetDataTable("Select DM, MC From 三调工作分类", "temp");
            Dictionary<string, string> dicDL = dtDL.Rows.Cast<DataRow>().ToDictionary(x => x["DM"].ToString(), x => x["MC"].ToString());
            DataTable dtTBXH = RCIS.Database.LS_SetupMDBHelper.GetDataTable("Select DM, MC From DIC_38图斑细化类型代码表", "temp");
            Dictionary<string, string> dicTBXH = dtTBXH.Rows.Cast<DataRow>().ToDictionary(x => x["DM"].ToString(), x => x["MC"].ToString());
            DataTable dtZZSX = RCIS.Database.LS_SetupMDBHelper.GetDataTable("Select DM, MC From DIC_39种植属性代码表", "temp");
            Dictionary<string, string> dicZZSX = dtZZSX.Rows.Cast<DataRow>().ToDictionary(x => x["DM"].ToString(), x => x["MC"].ToString());
            DataTable dtQSXZ = RCIS.Database.LS_SetupMDBHelper.GetDataTable("Select DM From DIC_37权属性质代码表", "temp");
            List<string> qsxzs = dtQSXZ.Rows.Cast<DataRow>().Select(x => x["DM"].ToString()).ToList();

            string[] gddl = { "0101", "0102", "0103" };
            string[] fieldName = { "ZLDWMC", "TBBH", "DLMC" };
            string[] fieldAlias = { "坐落单位名称", "图斑编号", "地类名称" };
            DataTable dt = new DataTable();
            dt.Columns.Add("编号");
            dt.Columns.Add("错误类型");
            foreach (string item in fieldAlias)
            {
                dt.Columns.Add(item);
            }

            IFeatureClass pFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                int n = 0;
                progressBarControl1.Properties.Minimum = 0;
                progressBarControl1.Properties.Maximum = pFeatureClass.FeatureCount(null);
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, true);
                comRel.ManageLifetime(pFeatureCursor);
                IFeature pFeature;
                while ((pFeature = pFeatureCursor.NextFeature()) != null)
                {
                    progressBarControl1.Position = ++n;
                    int objectid = pFeature.OID;
                    List<string> values = new List<string>();
                    foreach (string item in fieldName)
                    {
                        values.Add(pFeature.get_Value(pFeatureClass.FindField(item)).ToString());
                    }
                    foreach (TreeListNode item in treCheckOption.GetAllCheckedNodes())
                    {
                        switch (item.GetValue(treCheckOption.Columns[0]).ToString())
                        {
                            case "地类名称与编码":
                                string dldm = pFeature.get_Value(pFeatureClass.FindField("DLBM")).ToString();
                                string dlmc = pFeature.get_Value(pFeatureClass.FindField("DLMC")).ToString();
                                if (!dicDL.ContainsKey(dldm))
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = objectid;
                                    dr[1] = "地类错误";
                                    for (int i = 2; i < fieldName.Length + 2; i++)
                                    {
                                        dr[i] = values.ToArray()[i - 2];
                                    }
                                    dt.Rows.Add(dr);
                                }
                                else
                                {
                                    if (dicDL[dldm] != dlmc)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "地类错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                                break;
                            case "权属性质":
                                string qsxz = pFeature.get_Value(pFeatureClass.FindField("QSXZ")).ToString();
                                if (!qsxzs.Contains(qsxz))
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = objectid;
                                    dr[1] = "权属性质错误";
                                    for (int i = 2; i < fieldName.Length + 2; i++)
                                    {
                                        dr[i] = values.ToArray()[i - 2];
                                    }
                                    dt.Rows.Add(dr);
                                }
                                break;
                            case "权属单位代码长度（19位）":
                                string qsdwdm = pFeature.get_Value(pFeatureClass.FindField("QSDWDM")).ToString();
                                if (qsdwdm.Length != 19)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = objectid;
                                    dr[1] = "权属单位代码长度错误";
                                    for (int i = 2; i < fieldName.Length + 2; i++)
                                    {
                                        dr[i] = values.ToArray()[i - 2];
                                    }
                                    dt.Rows.Add(dr);
                                }
                                break;
                            case "坐落单位代码长度（19位）":
                                string zldwdm = pFeature.get_Value(pFeatureClass.FindField("ZLDWDM")).ToString();
                                if (zldwdm.Length != 19)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = objectid;
                                    dr[1] = "坐落单位代码长度错误";
                                    for (int i = 2; i < fieldName.Length + 2; i++)
                                    {
                                        dr[i] = values.ToArray()[i - 2];
                                    }
                                    dt.Rows.Add(dr);
                                }
                                break;
                            case "扣除地类编码（必须是1203，同时地类是耕地）":
                                string kcdlbm = pFeature.get_Value(pFeatureClass.FindField("KCDLBM")).ToString();
                                if (!string.IsNullOrWhiteSpace(kcdlbm))
                                {
                                    if (kcdlbm != "1203")
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "扣除地类编码错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        string dlbm = pFeature.get_Value(pFeatureClass.FindField("DLBM")).ToString();
                                        if (!gddl.Contains(dlbm))
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr[0] = objectid;
                                            dr[1] = "扣除地类编码错误";
                                            for (int i = 2; i < fieldName.Length + 2; i++)
                                            {
                                                dr[i] = values.ToArray()[i - 2];
                                            }
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                                break;
                            case "扣除系数（必须是0<=x<1，同时地类是耕地）":
                                double kcxs = double.Parse(pFeature.get_Value(pFeatureClass.FindField("KCXS")).ToString());
                                
                                if (kcxs < 0 || kcxs >= 1)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = objectid;
                                    dr[1] = "扣除系数错误";
                                    for (int i = 2; i < fieldName.Length + 2; i++)
                                    {
                                        dr[i] = values.ToArray()[i - 2];
                                    }
                                    dt.Rows.Add(dr);
                                }
                                else if (kcxs != 0)
                                {
                                    string dlbm = pFeature.get_Value(pFeatureClass.FindField("DLBM")).ToString();
                                    if (!gddl.Contains(dlbm))
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "扣除系数错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                                
                                break;
                            case "耕地类型（地类是耕地）":
                                string gdlx = pFeature.get_Value(pFeatureClass.FindField("GDLX")).ToString();
                                if (!string.IsNullOrWhiteSpace(gdlx))
                                {
                                    string dlbm = pFeature.get_Value(pFeatureClass.FindField("DLBM")).ToString();
                                    if (!gddl.Contains(dlbm))
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "扣除地类编码错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                                break;
                            case "耕地坡度级别（必须是1至5，同时地类是耕地）":
                                string GDPD = pFeature.get_Value(pFeatureClass.FindField("GDPDJB")).ToString();
                                if (!string.IsNullOrWhiteSpace(GDPD))
                                {
                                    if (GDPD != "1" && GDPD != "2" && GDPD != "3" && GDPD != "4" && GDPD != "5")
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "耕地坡度错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        string dlbm = pFeature.get_Value(pFeatureClass.FindField("DLBM")).ToString();
                                        if (!gddl.Contains(dlbm))
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr[0] = objectid;
                                            dr[1] = "耕地坡度错误";
                                            for (int i = 2; i < fieldName.Length + 2; i++)
                                            {
                                                dr[i] = values.ToArray()[i - 2];
                                            }
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                                break;
                            case "线状地物宽度（地类必须是1001,1003,1004,1006,1009,1101,1107）":
                                string xzdwkd = pFeature.get_Value(pFeatureClass.FindField("XZDWKD")).ToString();
                                if (xzdwkd != "0")
                                {
                                    string dlbm = pFeature.get_Value(pFeatureClass.FindField("DLBM")).ToString();
                                    if (dlbm != "1001" && dlbm != "1003" && dlbm != "1004" && dlbm != "1006" && dlbm != "1009" && dlbm != "1101" && dlbm != "1107")
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "耕地坡度错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                                break;
                            case "图斑细化代码和名称":
                                string tbxhdm = pFeature.get_Value(pFeatureClass.FindField("TBXHDM")).ToString();
                                string tbxhmc = pFeature.get_Value(pFeatureClass.FindField("TBXHMC")).ToString();
                                if (!string.IsNullOrWhiteSpace(tbxhdm + tbxhmc))
                                {
                                    if (!dicTBXH.ContainsKey(tbxhdm))
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "图斑细化错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        if (dicTBXH[tbxhdm] != tbxhmc)
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr[0] = objectid;
                                            dr[1] = "图斑细化错误";
                                            for (int i = 2; i < fieldName.Length + 2; i++)
                                            {
                                                dr[i] = values.ToArray()[i - 2];
                                            }
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                                break;
                            case "种植属性代码和名称":
                                string zzsxdm = pFeature.get_Value(pFeatureClass.FindField("ZZSXDM")).ToString();
                                string zzsxmc = pFeature.get_Value(pFeatureClass.FindField("ZZSXMC")).ToString();
                                if (!string.IsNullOrWhiteSpace(zzsxdm + zzsxmc))
                                {
                                    if (!dicZZSX.ContainsKey(zzsxdm))
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr[0] = objectid;
                                        dr[1] = "种植属性错误";
                                        for (int i = 2; i < fieldName.Length + 2; i++)
                                        {
                                            dr[i] = values.ToArray()[i - 2];
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        if (dicZZSX[zzsxdm] != zzsxmc)
                                        {
                                            DataRow dr = dt.NewRow();
                                            dr[0] = objectid;
                                            dr[1] = "种植属性错误";
                                            for (int i = 2; i < fieldName.Length + 2; i++)
                                            {
                                                dr[i] = values.ToArray()[i - 2];
                                            }
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            
            gridControl1.DataSource = dt;
            gridView1.BestFitColumns();
            UpdateStatus("检查完成");
            int err = dt.Rows.Count;
            if (err > 0)
            {
                UpdateStatus("共检查错误" + err + "个");
                tabCheckData.SelectedTabPageIndex = 1;
                if (pFeatureLayer != null)
                {
                    if (pFeatureLayer.Name != pFeatureClass.AliasName)
                    {
                        mapControl.Map.DeleteLayer(pFeatureLayer);
                    }
                    else
                    {
                        RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureClass);
                        return;
                    }
                }
                pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pFeatureClass.AliasName;
                mapControl.Map.AddLayer(pFeatureLayer);
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureClass);
            //MessageBox.Show("完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        IFeatureLayer pFeatureLayer = null;

        private void gridView1_Click(object sender, EventArgs e)
        {
            int objid = int.Parse(gridView1.GetDataRow(gridView1.FocusedRowHandle)[0].ToString());
            IFeatureClass pFeatureClass = RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(txtDLTBBH.Text);
            IFeature pFeature = pFeatureClass.GetFeature(objid);
            IGeometry pGeometry = pFeature.ShapeCopy;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            IActiveView pActiveView = mapControl.Map as IActiveView;
            pActiveView.Extent = pTop.Buffer(50).Envelope;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, pActiveView.Extent);
            Application.DoEvents();
            mapControl.FlashShape(pGeometry);
        }

        private void FrmCheckData_FormClosed(object sender, FormClosedEventArgs e)
        {
            mapControl.Map.DeleteLayer(pFeatureLayer);
        }
    }
}
