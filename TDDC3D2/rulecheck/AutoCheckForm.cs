using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using DevExpress.XtraTreeList.Nodes;
namespace RuleCheck
{
    public partial class AutoCheckForm : Form
    {
        public AutoCheckForm()
        {
            InitializeComponent();
        }


        public IWorkspace currWS = null;
        public IEnvelope currEnv = null;

        public class CCheckRuleObj
        {
            public string ruleCode = "";
            public string ruleGroup = "";
            public string ruleContent = "";
            public double kcf = 0;
            public string description = "";
            public string errLevel = "1";
        }
        private Dictionary<string, string> GetAllGroups()
        {
            //获取所有分组名称
            Dictionary<string, string> dicGroup = new Dictionary<string, string>();
            RCIS.Database.AccdbOperateHelper dbhelper = new RCIS.Database.AccdbOperateHelper(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            try
            {
                string sql = "select distinct 检查分类,left(检查编号,1) as ruleid from CHK_RULES order by  left(检查编号,1) ";
                DataTable dt = dbhelper.GetDatatable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    dicGroup.Add(dr["ruleid"].ToString(), dr["检查分类"].ToString());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbhelper.Close();
            }

            //IDbString dbstr = new AccessDbString(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    string sql = "select distinct 检查分类,left(检查编号,1) as ruleid from CHK_RULES order by  left(检查编号,1) ";
            //    DataSet ds = conn.Fill(sql);
            //    DataTable dt = ds.Tables[0];
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        dicGroup.Add(dr["ruleid"].ToString(), dr["检查分类"].ToString());
            //    }
            //}
            return dicGroup;
        }

        //按分组获取规则
        private List<CCheckRuleObj> GetAllRules(string groupId)
        {

            List<CCheckRuleObj> lstRules = new List<CCheckRuleObj>();
            RCIS.Database.AccdbOperateHelper dbhelper = new RCIS.Database.AccdbOperateHelper(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            try
            {
                string sql = " SELECT  *  FROM CHK_RULES where left(检查编号,1)='" + groupId + "'    order by 检查编号 ";
                DataTable dt = dbhelper.GetDatatable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    CCheckRuleObj obj = new CCheckRuleObj();
                    obj.ruleCode = dr["检查编号"].ToString();
                    obj.description = dr["描述"].ToString();
                    obj.errLevel = dr["错误级别"].ToString();
                    obj.ruleGroup = dr["检查分类"].ToString();
                    obj.ruleContent = dr["检查内容"].ToString();
                    obj.kcf = Convert.ToDouble(dr["扣除分"].ToString());

                    lstRules.Add(obj);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                dbhelper.Close();
            }

            //IDbString dbstr = new AccessDbString(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    try
            //    {
            //        string sql = " SELECT  *  FROM CHK_RULES where left(检查编号,1)='" + groupId + "'    order by 检查编号 ";
            //        DataSet ds = conn.Fill(sql);
            //        DataTable dt = ds.Tables[0];
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            CCheckRuleObj obj = new CCheckRuleObj();
            //            obj.ruleCode = dr["检查编号"].ToString();
            //            obj.description = dr["描述"].ToString();
            //            obj.errLevel = dr["错误级别"].ToString();
            //            obj.ruleGroup = dr["检查分类"].ToString();
            //            obj.ruleContent = dr["检查内容"].ToString();
            //            obj.kcf = Convert.ToDouble(dr["扣除分"].ToString());

            //            lstRules.Add(obj);
            //        }
            //    }
            //    catch { }
            //}
            return lstRules;

        }
        private void LoadRule2Tree()
        {
            //加载规则
            treeListRules.Nodes.Clear();

            Dictionary<string, string> dicGroup = GetAllGroups();
            foreach (KeyValuePair<string, string> de in dicGroup)
            {
                string ruleid = de.Key.ToString();
                string ruleGroup = de.Value.ToString();

                List<CCheckRuleObj> lstSubRule = GetAllRules(ruleid);
                if (lstSubRule.Count == 0)
                    continue;

                TreeListNode rootNode = treeListRules.AppendNode(ruleid, null);
                rootNode.SetValue(0, ruleid);
                rootNode.SetValue(1, ruleGroup);
                rootNode.Checked = true;


                foreach (CCheckRuleObj aRule in lstSubRule)
                {
                    TreeListNode subNode = treeListRules.AppendNode(aRule.ruleCode, rootNode);
                    subNode.SetValue(0, aRule.ruleCode);
                    subNode.SetValue(1, aRule.ruleContent);
                    subNode.Tag = aRule;
                    subNode.Checked = true;
                }

            }
        }

        public DataTable currentCheckErrTab = null;

        private void AutoCheckForm_Load(object sender, EventArgs e)
        {
            LoadRule2Tree();
            treeListRules.ExpandAll();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < treeListRules.Nodes.Count; i++)
            {
                treeListRules.Nodes[i].CheckState = CheckState.Checked;
                SetCheckedChildNodes(treeListRules.Nodes[i], CheckState.Checked);
            }
        }
        /// <summary>        
        /// 选择子节点时触发        
        /// </summary>        
        /// <param name="node"></param>       
        /// /// <param name="check"></param>        
        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if ((this.currWS!=null) && (this.currWS!=RCIS.Global.GlobalEditObject.GlobalWorkspace ) )
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.currWS);
            }
            Close();
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
        private void treeListRules_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeListRules_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void treeListRules_Click(object sender, EventArgs e)
        {
            
        }

        private void treeListRules_MouseClick(object sender, MouseEventArgs e)
        {
            if (treeListRules.Selection.Count > 0)
            {
                TreeListNode node = treeListRules.Selection[0];
                CCheckRuleObj aRule = (CCheckRuleObj)node.Tag;
                if (aRule == null)
                {
                    memeoError.Text = "";
                }

                else
                {
                    memeoError.Text = aRule.description;
                }
            }
        }


        private void UpdateStatus(string txt)
        {
            memoLog.Text = DateTime.Now.ToString() + " : " + txt + "\r\n" + memoLog.Text;
            Application.DoEvents();
        }

        private void RefreshError()
        {
            //IDbString dbstr = new AccessDbString(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            //using (AccessConnectionManager conn = new AccessConnectionManager((AccessDbString)dbstr))
            //{
            //    DataSet dsErr = conn.Fill("select * from CHK_ERRORLIST ");
            //    DataTable dtErr = dsErr.Tables[0];
            //    this.gridControlError.DataSource = dtErr;

            //}

            RCIS.Database.AccdbOperateHelper dbhelper = new RCIS.Database.AccdbOperateHelper(RCIS.Global.AppParameters.ConfPath + @"\setup.mdb");
            try
            {
                DataTable dtErr = dbhelper.GetDatatable("select * from CHK_ERRORLIST ");
                this.gridControlError.DataSource = dtErr;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                dbhelper.Close();
            }
        }

        private void btnStartCheck_Click(object sender, EventArgs e)
        {
            //开始检查
            CheckErrorHelper.ClearErrors();
            List<string> allRules = new List<string>();

            #region //获取到所有选中的规则列表
            TreeListNodes parentNodes = this.treeListRules.Nodes;
            foreach (TreeListNode aRuleGroup in parentNodes)
            {
                TreeListNodes ruleNodes = aRuleGroup.Nodes;
                foreach (TreeListNode aRule in ruleNodes)
                {

                    string ruleId = aRule.GetValue(0).ToString();
                    if (aRule.CheckState == CheckState.Checked)
                    {
                        allRules.Add(ruleId);
                    }
                }
            }
            #endregion

            this.Cursor = Cursors.WaitCursor;
            this.UpdateStatus("开始执行检查...");
            try
            {

                if (allRules.Contains("1101") || allRules.Contains("1102") || allRules.Contains("1103") || allRules.Contains("1401"))
                {
                    //RuleCheck1 check1 = new RuleCheck1(aTask, this.lblStatus);
                    //check1.Check();
                }
                if (allRules.Contains("2201") || allRules.Contains("2202") || allRules.Contains("2203"))
                {
                    RuleCheck2 check2 = new RuleCheck2(this.currWS, this.lblStatus);
                    check2.Check22();
                    this.UpdateStatus("数学基础检查完成。");
                }


                if (allRules.Contains("3101") || allRules.Contains("3102") || allRules.Contains("3103")
                    || allRules.Contains("3104")|| allRules.Contains("3105")|| allRules.Contains("3106"))
                {

                    RuleCheck3 check3 = new RuleCheck3(this.currWS, this.lblStatus);
                    try
                    {
                        check3.Check3101();
                        check3.Check3104();
                        check3.Check3105();
                        check3.Check3106();
                        this.UpdateStatus("数据结构性检查完毕。");
                    }
                    catch (Exception ex)
                    {
                        this.UpdateStatus(ex.Message);
                    }

                }
                if (allRules.Contains("4101") || allRules.Contains("4201") || allRules.Contains("4202")||allRules.Contains("4203") ||
                    
                    allRules.Contains("4301") ||
                    allRules.Contains("4302") || allRules.Contains("4303") || allRules.Contains("4304") ||
                    allRules.Contains("4601") || allRules.Contains("4701")

                    )
                {
                    RuleCheck4 check4 = null;
                    if (this.chkCurrentExt.Checked)
                    {
                        check4 = new RuleCheck4(this.currWS, this.currEnv, this.lblStatus);
                    }
                    else
                    {
                        check4 = new RuleCheck4(this.currWS, null, this.lblStatus);
                    }
                    check4.Check4101();
                    check4.Check4201();

                    check4.Check4601();
                    check4.Check4701();
                    this.UpdateStatus("空间拓扑检查完毕！");
                    
                }
                if (allRules.Contains("5101") || allRules.Contains("5102") ||allRules.Contains("5103") || allRules.Contains("5104") )
                {
                    TDDC3D.rulecheck.RuleCheck5 check5 = new TDDC3D.rulecheck.RuleCheck5(this.currWS, this.currEnv, this.lblStatus);
                    check5.Check51012();
                    check5.Check5103();
                    check5.Check5104();
                    this.UpdateStatus("城镇村专项检查完毕！");
                }
                if (allRules.Contains("6101"))
                {
                    TDDC3D.rulecheck.RuleCheck6 check6 = new TDDC3D.rulecheck.RuleCheck6(this.currWS, this.currEnv, this.lblStatus);
                    check6.Check6101();
                    this.UpdateStatus("图斑细化代码和种植属性代码检查完毕！");
                }

                MessageBox.Show("检查完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshError();
                this.xtraTabControl1.SelectedTabPageIndex = 1;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.lblStatus.Text = "";
            }

            

        }

        private void brSrcmdbWs_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog()==DialogResult.Cancel)
            {
                return;

            }
            this.brSrcmdbWs.Text = dlg.SelectedPath;
            
            this.currWS = WorkspaceHelper2.GetFileGdbWorkspace(dlg.SelectedPath);
            if (currWS == null)
            {
                MessageBox.Show("打开工作空间失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            
        }

        private void 导出日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //导出错误日志
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel文件|*.xls";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destName = dlg.FileName;
            this.gridControlError.ExportToXls(destName);
            MessageBox.Show("导出完毕!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void 将日志显示在主窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.currentCheckErrTab = (DataTable)this.gridControlError.DataSource;
            this.Close();
        }
    }
}
