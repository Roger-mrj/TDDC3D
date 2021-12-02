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
using System.Collections;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geoprocessor;

namespace TDDC3D.gengxin
{
    public partial class FrmSCNMSJK : Form
    {
        public IWorkspace currWs = null;
        public FrmSCNMSJK()
        {
            InitializeComponent();
        }

        private void txtDBPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string sFile = dlg.FileName;
            if (!sFile.ToUpper().EndsWith(".GDB"))
            {
                sFile += ".gdb";
            }
            this.txtDBPath.Text = sFile;
        }

        private void btnCreateDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtXZQDM.Text))
                {
                    MessageBox.Show("当前县代码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (this.txtDBPath.Text.Trim() == "")
                {
                    MessageBox.Show("输入项不可为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string destDir = this.txtDBPath.Text;
                if (System.IO.Directory.Exists(destDir))
                {
                    MessageBox.Show("目标数据库已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                UpdateStatus("正在创建年末标准库");
                this.Cursor = Cursors.WaitCursor;
                
                IFeatureWorkspace pFeaWorkspace = currWs as IFeatureWorkspace;
                IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                pWorkspaceFactory.Create(System.IO.Path.GetDirectoryName(destDir), System.IO.Path.GetFileName(destDir), null, 0);
                IWorkspace pTarWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(destDir);
                string sourceDir = RCIS.Global.AppParameters.ConfPath + @"\standard.gdb";
                IWorkspace pSourWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(sourceDir);
                IEnumDataset pEnumDataset = pSourWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                pEnumDataset.Reset();
                IDataset pDataset = pEnumDataset.Next();
                while (pDataset != null)
                {
                    if (pDataset.Name.ToLower() == "tddc" || pDataset.Name.ToLower() == "tdls" || pDataset.Name.ToLower() == "tdgx" || pDataset.Name.ToLower() == "tf")
                    {
                        UpdateStatus("正在创建" + pDataset.Name + "");
                        IDataset ZBDataset = pFeaWorkspace.OpenFeatureDataset(pDataset.Name);
                        ISpatialReference pSpatialReference = (ZBDataset as IGeoDataset).SpatialReference;
                        IFeatureDataset pTDDCFeatureDataset = (pTarWorkspace as IFeatureWorkspace).CreateFeatureDataset(pDataset.Name, pSpatialReference);
                        IFeatureClassContainer pFCC = pDataset as IFeatureClassContainer;
                        IEnumFeatureClass pEnumFC = pFCC.Classes;
                        pEnumFC.Reset();
                        IFeatureClass pFeatureClass = pEnumFC.Next();
                        while (pFeatureClass != null)
                        {
                            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(pSourWorkspace, pTarWorkspace, (pFeatureClass as IDataset).Name, (pFeatureClass as IDataset).Name, pTDDCFeatureDataset, null);
                            IClassSchemaEdit2 pClassSchemaEdit2 = (pTarWorkspace as IFeatureWorkspace).OpenFeatureClass((pFeatureClass as IDataset).Name) as IClassSchemaEdit2;
                            pClassSchemaEdit2.AlterAliasName(pFeatureClass.AliasName);
                            pFeatureClass = pEnumFC.Next();
                        }
                    }
                    pDataset = pEnumDataset.Next();
                }
                this.Cursor = Cursors.Default;
                UpdateStatus("年末标准库创建完毕");
                //将调出的数据保存，之后用来擦除
                IWorkspace pTmpWs = RCIS.GISCommon.WorkspaceHelper2.DeleteAndNewTmpGDB();
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = "xzqdm not like '" + txtXZQDM.Text.ToString().Trim() + "%'";
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWs, "XZQGX", "XZQOut", pQf);
                //打开待生成的gdb
                IWorkspace gdbWorkspace = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(txtDBPath.Text);
                IFeatureWorkspace gdbFeaWorkspace = gdbWorkspace as IFeatureWorkspace;
                //for循环  遍历tdgx内的要素集
                IDataset TDGX = pFeaWorkspace.OpenFeatureDataset("TDGX");
                IFeatureClassContainer pTDGXContainer = TDGX as IFeatureClassContainer;
                for (int i = 0; i < pTDGXContainer.ClassCount; i++)
                {
                    IFeatureClass pFC = pTDGXContainer.get_Class(i);
                    string name = (pFC as IDataset).Name;
                    if (name.Substring(name.Length - 2) == "GC")
                        continue;
                    string input = currWs.PathName + "\\TDDC\\" + name.Substring(0, name.Length - 2);
                    string target = gdbWorkspace.PathName + "\\TDDC\\" + name.Substring(0, name.Length - 2);
                    if (name == "CJDCQGX" || name == "DLTBGX" || name == "XZQGX")
                    {
                        //RCIS.GISCommon.GpToolHelper.es
                        UpdateStatus("正在生成" + name.Substring(0, name.Length - 2) + "");
                        bool b = RCIS.GISCommon.GpToolHelper.Update(input, currWs.PathName + "\\TDGX\\" + name, pTmpWs.PathName+"\\"+name);

                        b = RCIS.GISCommon.GpToolHelper.Erase_analysis(pTmpWs.PathName + "\\" + name, pTmpWs.PathName + "\\XZQOut", target);

                        //gdb
                        ////把tddc层数据导入
                        //GPAppend(input,target);
                        //string bsm = "";
                        //if (name.Contains("DLTB"))
                        //    bsm = "BGQTBBSM";
                        //else
                        //    bsm = "BGQBSM";
                        //IFeatureClass pFeaClasss = pFeaWorkspace.OpenFeatureClass(name+"GC");
                        //List<string> bgqBSM = GetUniqueValuesByFeatureClass(pFeaClasss, bsm);
                        //string where = "";
                        //if (bgqBSM.Count > 0)
                        //{
                        //    foreach (string item in bgqBSM)
                        //    {
                        //        where += "BSM ='" + item + "' or ";
                        //    }
                        //    where = where.Substring(0, where.Length - 4);
                        //}
                        //IQueryFilter pQuery = new QueryFilterClass();
                        //pQuery.WhereClause = where;
                        //IFeatureClass gdbFeatureClass = gdbFeaWorkspace.OpenFeatureClass(name.Substring(0, name.Length - 2));
                        //(gdbFeatureClass as ITable).DeleteSearchedRows(pQuery);
                        ////把更新层数据导入
                        //GPAppend(currWs.PathName + "\\TDGX\\" + name,target);
                    }
                    else
                    {
                        UpdateStatus("正在生成" + name.Substring(0, name.Length - 2) + "");
                        //更新层不为空
                        if (pFC != null)
                        {
                            if (name == "CZKFBJGX")
                                target = target.Replace("CZKFBJ", "CSKFBJ");
                            if (name == "HAXGX")
                                continue;
                            GPAppend(currWs.PathName + "\\TDGX\\" + name, target);
                        }
                        //更新层为空
                        else
                        {
                            if (name == "HAXGX")
                                continue;
                            if (name == "CZKFBJGX")
                                target = target.Replace("CZKFBJ", "CSKFBJ");
                            GPAppend(input, target);
                        }
                    }
                }

                gdbWorkspace.ExecuteSQL("delete from dltb where zldwdm not like '" + txtXZQDM.Text + "%'");
                gdbWorkspace.ExecuteSQL("delete from cjdcq where zldwdm not like '" + txtXZQDM.Text + "%'");
                gdbWorkspace.ExecuteSQL("delete from xzq where xzqdm not like '" + txtXZQDM.Text + "%'");
                
                MessageBox.Show("创建完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch(Exception ex)
            {
                RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex, ex.ToString());
                return;
            }


        }

        private void GPAppend(string sourceFile,string targetFile) 
        {
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.DataManagementTools.Append apend = new ESRI.ArcGIS.DataManagementTools.Append();
            apend.inputs = sourceFile;
            apend.target = targetFile;
            apend.schema_type = "NO_TEST";
            gp.Execute(apend, null);
        }

        private List<string> GetUniqueValuesByFeatureClass(IFeatureClass pFeatureClass, string FieldName)
        {
            List<string> arrValues = new List<string>();
            DataStatisticsClass pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pFeatureClass.Search(null, false) as ICursor;
            pDataStatistics.Field = FieldName;
            IEnumerator pEnum = pDataStatistics.UniqueValues;
            while (pEnum.MoveNext())
            {
                string temp = pEnum.Current.ToString();
                arrValues.Add(temp);
            }
            return arrValues;
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmSCNMSJK_Load(object sender, EventArgs e)
        {
            IFeatureClass pXZQClass = null;
            //string xzdm = "";
            try
            {
                pXZQClass = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQClass != null)
            {
                List<string> dms = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace, "XZQ", "XZQDM", 6);
                txtXZQDM.Properties.Items.Clear();
                foreach (string dm in dms)
                {
                    txtXZQDM.Properties.Items.Add(dm);
                }
                if (txtXZQDM.Properties.Items.Count > 0) txtXZQDM.SelectedIndex = 0;
            }
        }
    }
}
