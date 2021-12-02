using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.DataSourcesFile;
using DevExpress.XtraTreeList.Nodes;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.output
{
    public partial class OutXzqDataForm : Form
    {
        public OutXzqDataForm()
        {
            InitializeComponent();
        }
        public IWorkspace sendWs = null;
        public string sendXZDM = "";

        public List<string> retFeaClassName = new List<string>();

        
        private void OutXzqDataForm_Load(object sender, EventArgs e)
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

                bool ischecked = false;
                //获取子节点
                IFeatureDataset pFeaDs = (sendWs as IFeatureWorkspace).OpenFeatureDataset(aItem.Key.ToString());
                IFeatureClassContainer pClassContaniner = pFeaDs as IFeatureClassContainer;
                for (int i = 0; i < pClassContaniner.ClassCount; i++)
                {
                    IFeatureClass pFC = pClassContaniner.get_Class(i);
                    IDataset pFCDs = pFC as IDataset;
                    string fcName = pFCDs.Name;
                    string alias = pFC.AliasName;

                    TreeListNode subNode = treeList1.AppendNode(fcName, rootNode);
                    subNode.SetValue(0, fcName);
                    subNode.SetValue(1, alias);
                    subNode.Checked = true;
                }
                if (ischecked)
                {
                    rootNode.Checked = true;

                }
                else
                {
                    rootNode.Checked = false;
                }
            }
            treeList1.ExpandAll();
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

        IFeatureClass xzqClass = null;


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if ((this.radioGroup1.SelectedIndex == 0) && (this.beShpDestDir.Text.Trim() == "") )
            {
                return;
            }
            if ((this.radioGroup1.SelectedIndex == 1) && (this.beDestMdbFile.Text.Trim() == ""))
                return;


            retFeaClassName.Clear();
            TreeListNodes parentNodes = this.treeList1.Nodes;
            foreach (TreeListNode aRuleGroup in parentNodes)
            {
                TreeListNodes ruleNodes = aRuleGroup.Nodes;
                foreach (TreeListNode aRule in ruleNodes)
                {
                    string fcName = aRule.GetValue(0).ToString();
                    if (aRule.CheckState == CheckState.Checked)
                    {
                        this.retFeaClassName.Add(fcName);
                    }
                }
            }

            // 获取 
            IFeatureClass xzqClass = (this.sendWs as IFeatureWorkspace).OpenFeatureClass("XZQ");
            string prefix = this.sendXZDM.Substring(0, 6);
            string xzdm = RCIS.Utility.OtherHelper.listcount(this.sendXZDM);
            IGeometry fwGeo = null;
            ISpatialFilter pSF = null;

            try{
                #region 找范围
                if (xzdm.Length == 6)
                {
                    //整县
                }
                else if (xzdm.Length == 9)
                {
                    //乡                  
                    //2019-4-2 日修改，行政区 为乡级
                    string where = "XZQDM ='" + xzdm + "'";
                    IQueryFilter qur = new QueryFilterClass();
                    qur.WhereClause = where;
                    IFeatureCursor pCur = xzqClass.Search(qur, false);
                    IFeature xfea = pCur.NextFeature();
                    if (xfea == null)
                        return;
                    IGeometry uGeo = xfea.Shape;
                    xfea = pCur.NextFeature();
                    while (xfea != null)
                    {
                        try
                        {
                            ITopologicalOperator top = uGeo as ITopologicalOperator;
                            uGeo = top.Union(xfea.Shape);
                        }
                        catch
                        { }
                        xfea = pCur.NextFeature();

                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCur);
                    fwGeo = uGeo;
                    pSF = new SpatialFilterClass();
                    pSF.Geometry = fwGeo;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                }
                else if (xzdm.Length == 12)
                {
                    //村
                    IFeatureClass cjDCQClass = (this.sendWs as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
                    //2019-4-3修改，村级调查区
                    string where = "XZQDM like '" + xzdm + "%'";
                    IFeature aFea = RCIS.GISCommon.GetFeaturesHelper.getFirstFeatureBySql(cjDCQClass, where);
                    if (aFea == null)
                    {
                        return;
                    }
                    fwGeo = aFea.ShapeCopy;
                    pSF = new SpatialFilterClass();
                    pSF.Geometry = fwGeo;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                }
                 #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取范围失败！","提示",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }


            if (this.radioGroup1.SelectedIndex == 0)
            {
                
                
                this.Cursor = Cursors.WaitCursor;
                try
                {                  

                    foreach (string aName in retFeaClassName)
                    {
                        string destShp = this.beShpDestDir.Text.Trim() + "\\" + prefix + aName + ".shp";

                        IFeatureClass pFC = (this.sendWs as IFeatureWorkspace).OpenFeatureClass(aName);
                        lblStatus.Text = "正在导出" + aName + "...";
                        Export(pFC, pSF as IQueryFilter, destShp);
                    }
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("导出结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message);
                }
            
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在导出数据...", "请稍等...");
                wait.Show();
                try
                {
                    IWorkspace destWs = RCIS.GISCommon.WorkspaceHelper2.CreateAccessWorkspace(this.beDestMdbFile.Text);
                    foreach (string aName in retFeaClassName)
                    {
                        IFeatureClass pFC = (this.sendWs as IFeatureWorkspace).OpenFeatureClass(aName);
                        wait.SetCaption("正在导出" + aName + "...");
                        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(this.sendWs, destWs, aName, aName, pSF as IQueryFilter);
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(destWs);

                    wait.Close();
                    MessageBox.Show("导出结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    wait.Close();
                    MessageBox.Show(ex.Message);
                }

            }
            


            

        }

       

       
        private bool Export(
            IFeatureClass fc, IQueryFilter queryFilter, string shapefileName)
        {
            try
            {
                string folderPath = System.IO.Path.GetDirectoryName(shapefileName);
                string shortName = System.IO.Path.GetFileNameWithoutExtension(shapefileName);
                IWorkspaceFactory targetWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                IWorkspace targetWorkspace = targetWorkspaceFactory.OpenFromFile(folderPath, 0);

                IWorkspace sourceWs = (fc as IDataset).Workspace;
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(sourceWs, targetWorkspace, (fc as IDataset).Name, shortName, queryFilter);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        //private bool Export(
        //    IFeatureClass fc, IQueryFilter queryFilter, string shapefileName)
        //{

            

        //    try
        //    {
        //        string folderPath = System.IO.Path.GetDirectoryName(shapefileName);
        //        //int idxStart = shapefileName.LastIndexOf("\\");
        //        //int idxEnd = shapefileName.LastIndexOf(".");
        //        //string folderPath = shapefileName.Substring(0, idxStart);
        //        string shortName = System.IO.Path.GetFileNameWithoutExtension(shapefileName); //shapefileName.Substring(idxStart + 1, idxEnd - idxStart - 1);
                
        //        IWorkspaceFactory targetWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
        //        IWorkspace targetWorkspace = targetWorkspaceFactory.OpenFromFile(folderPath, 0);

                
        //        IDataset inDataSet = fc as IDataset;
        //        IFeatureClassName inFCName = inDataSet.FullName as IFeatureClassName;
               
        //        IWorkspace inWorkspace = inDataSet.Workspace;

        //        IDataset outDataSet = targetWorkspace as IDataset;
        //        IWorkspaceName outWorkspaceName = outDataSet.FullName as IWorkspaceName;

        //        IFeatureClassName outFCName = new FeatureClassNameClass();
        //        IDatasetName dataSetName = outFCName as IDatasetName;
        //        dataSetName.WorkspaceName = outWorkspaceName;
        //        dataSetName.Name = shortName;


        //        IFieldChecker fieldChecker = new FieldCheckerClass();
        //        fieldChecker.InputWorkspace = inWorkspace;
        //        fieldChecker.ValidateWorkspace = targetWorkspace;

        //        IFields fields = fc.Fields;
        //        IFields outFields = null;
        //        IEnumFieldError enumFieldError = null;
        //        fieldChecker.Validate(fields, out enumFieldError, out outFields);

        //        IFeatureDataConverter featureDataConverter = new FeatureDataConverterClass();
        //        featureDataConverter.ConvertFeatureClass(inFCName, queryFilter, null, outFCName, null, outFields, "", 100, 0);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}


        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList1_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel) return;
            this.beShpDestDir.Text = dlg.SelectedPath;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void beDestMdbFile_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "MDB文件|*.mdb";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.beDestMdbFile.Text = dlg.FileName;


        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.beShpDestDir.Enabled = true;
                this.beDestMdbFile.Enabled = false;
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                this.beDestMdbFile.Enabled = true;
                this.beShpDestDir.Enabled = false;
            }
        }

    }
}
