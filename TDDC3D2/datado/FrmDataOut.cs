using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace TDDC3D.datado
{
    public partial class FrmDataOut : Form
    {
        public IWorkspace _curWS;
        public IMap _curMap;

        public FrmDataOut()
        {
            InitializeComponent();
        }

        private void FrmDataOut_Load(object sender, EventArgs e)
        {
            txtPath.Text = System.IO.Path.GetDirectoryName(_curWS.PathName);
            LoadXZQ("XZQ", "XZQDM", "XZQMC");
            chkSelectAll.CheckState = CheckState.Checked;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadXZQ(string fcName, string keyFieldName, string valueFieldName)
        {
            if (!(_curWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, fcName])
            {
                MessageBox.Show("没有找到对应要素类。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在加载，请稍后……", "提示");
            wait.Show();
            lstXZQ.Items.Clear();
            Dictionary<string,string> dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(_curWS as IFeatureWorkspace, fcName, keyFieldName, valueFieldName);
            var dicSort = from objDic in dmmc orderby objDic.Key ascending select objDic;
            foreach (KeyValuePair<string, string> kvp in dicSort)
            {
                int i = lstXZQ.Items.Add((kvp.Key.Length > 12 ? kvp.Key.Substring(0, 12) : kvp.Key) + "|" + kvp.Value);
                lstXZQ.Items[i].CheckState = chkSelectAll.CheckState;
            }
            wait.Close();
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lstXZQ.Items.Count; i++)
            {
                lstXZQ.Items[i].CheckState = chkSelectAll.CheckState;
            }
        }

        private void rdoXZQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoXZQ.SelectedIndex == 0)
            {
                LoadXZQ("XZQ", "XZQDM", "XZQMC");
            }
            else
            {
                LoadXZQ("CJDCQ", "ZLDWDM", "ZLDWMC");
            }
        }

        private void txtPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog foldDia = new FolderBrowserDialog();
            if (foldDia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = foldDia.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPath.Text)) return;
            if (!Directory.Exists(txtPath.Text)) return;
            if (lstXZQ.CheckedItems.Count == 0) return;
            IWorkspace2 ws2 = _curWS as IWorkspace2;
            if (!ws2.NameExists[esriDatasetType.esriDTFeatureDataset, "TDDC"] || !ws2.NameExists[esriDatasetType.esriDTFeatureDataset, "TDGX"])
            {
                MessageBox.Show("数据库结构不正确。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理……", "提示");
            wait.Show();
            for (int i = 0; i < lstXZQ.CheckedItems.Count; i++)
            {
                string selXZQ = lstXZQ.CheckedItems[i].ToString();
                lstXZQ.SelectedValue = selXZQ;
                string[] dmmc = selXZQ.Split('|');
                string dm = dmmc[0];
                string mc = dmmc[1];
                IFeatureDataset pFeatureDataset = (_curWS as IFeatureWorkspace).OpenFeatureDataset("TDDC");
                wait.SetCaption("正在" + mc + "创建数据库……");
                string folderName = txtPath.Text + "\\" + mc + "(" + dm + ")";
                if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);
                
                IWorkspace outws = WorkspaceHelper2.DeleteAndNewGDB(folderName + "\\" + mc + "(" + dm + ").gdb");
                WorkspaceHelper2.CreateFeatrueDataset(outws, "TDDC", (pFeatureDataset as IGeoDataset).SpatialReference);
                if (rdoXZQ.SelectedIndex == 0)
                {
                    wait.SetCaption("处理" + mc + "的行政区……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "XZQ", "XZQ", (outws as IFeatureWorkspace).OpenFeatureDataset("TDDC"), "XZQDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的村级调查区……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "CJDCQ", "CJDCQ", (outws as IFeatureWorkspace).OpenFeatureDataset("TDDC"), "ZLDWDM Like '" + dm + "%'");
                }
                else
                {
                    wait.SetCaption("处理" + mc + "的村级调查区……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "CJDCQ", "CJDCQ", (outws as IFeatureWorkspace).OpenFeatureDataset("TDDC"), "ZLDWDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的行政区……");
                    GpToolHelper.Clip(_curWS.PathName + "\\TDDC\\XZQ", outws.PathName + "\\TDDC\\CJDCQ", outws.PathName + "\\TDDC\\XZQ");
                }
                IFeatureClassContainer pFCContainer = pFeatureDataset as IFeatureClassContainer;
                IEnumFeatureClass penumFeatureClass = pFCContainer.Classes;
                penumFeatureClass.Reset();
                IFeatureClass pFeatureClass;
                while ((pFeatureClass = penumFeatureClass.Next()) != null)
                {
                    wait.SetCaption("处理" + mc + "的" + pFeatureClass.AliasName + "……");
                    IDataset pDataset = pFeatureClass as IDataset;
                    if (pDataset.Name.ToUpper() != "XZQ" && pDataset.Name.ToUpper() != "CJDCQ")
                    {
                        int iFieldIndex = pFeatureClass.FindField("ZLDWDM");
                        if (iFieldIndex == -1)
                        {
                            GpToolHelper.Clip(_curWS.PathName + "\\TDDC\\" + pDataset.Name, outws.PathName + "\\TDDC\\CJDCQ", outws.PathName + "\\TDDC\\" + pDataset.Name);
                        }
                        else
                        {
                            EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, pDataset.Name, pDataset.Name, (outws as IFeatureWorkspace).OpenFeatureDataset("TDDC"), "ZLDWDM Like '" + dm + "%'");
                        }
                    }
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(penumFeatureClass);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFCContainer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureDataset);

                pFeatureDataset = (_curWS as IFeatureWorkspace).OpenFeatureDataset("TDGX");
                WorkspaceHelper2.CreateFeatrueDataset(outws, "TDGX", (pFeatureDataset as IGeoDataset).SpatialReference);
                if (rdoXZQ.SelectedIndex == 0)
                {
                    wait.SetCaption("处理" + mc + "的行政区更新……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "XZQGX", "XZQGX", (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), "XZQDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的行政区更新过程……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "XZQGXGC", "XZQGXGC", (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), "BGQXZQDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的村级调查区更新……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "CJDCQGX", "CJDCQGX", (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), "ZLDWDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的村级调查区更新过程……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "CJDCQGXGC", "CJDCQGXGC", (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), "BGQZLDWDM Like '" + dm + "%'");
                }
                else
                {
                    wait.SetCaption("处理" + mc + "的村级调查区更新……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "CJDCQGX", "CJDCQGX", (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), "ZLDWDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的村级调查区更新过程……");
                    EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, "CJDCQGXGC", "CJDCQGXGC", (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), "BGQZLDWDM Like '" + dm + "%'");
                    wait.SetCaption("处理" + mc + "的行政区更新……");
                    GpToolHelper.Clip(_curWS.PathName + "\\TDGX\\XZQGX", outws.PathName + "\\TDGX\\CJDCQ", outws.PathName + "\\TDGX\\XZQGX");
                    wait.SetCaption("处理" + mc + "的行政区更新过程……");
                    GpToolHelper.Clip(_curWS.PathName + "\\TDGX\\XZQGXGC", outws.PathName + "\\TDGX\\CJDCQ", outws.PathName + "\\TDGX\\XZQGXGC");
                }
                pFCContainer = pFeatureDataset as IFeatureClassContainer;
                penumFeatureClass = pFCContainer.Classes;
                penumFeatureClass.Reset();
                while ((pFeatureClass = penumFeatureClass.Next()) != null)
                {
                    IDataset pDataset = pFeatureClass as IDataset;
                    if (pDataset.Name.ToUpper() != "XZQGX" && pDataset.Name.ToUpper() != "CJDCQGX" &&
                        pDataset.Name.ToUpper() != "XZQGXGC" && pDataset.Name.ToUpper() != "CJDCQGXGC")
                    {
                        wait.SetCaption("处理" + mc + "的" + pFeatureClass.AliasName + "……");
                        string fieldName = "ZLDWDM";
                        int iFieldIndex = pFeatureClass.FindField("ZLDWDM");
                        if (iFieldIndex == -1)
                        {
                            iFieldIndex = pFeatureClass.FindField("BGQZLDWDM");
                            fieldName = "BGQZLDWDM";
                        }
                        if (iFieldIndex == -1)
                        {
                            GpToolHelper.Clip(_curWS.PathName + "\\TDGX\\" + pDataset.Name, outws.PathName + "\\TDDC\\CJDCQ", outws.PathName + "\\TDGX\\" + pDataset.Name);
                        }
                        else
                        {
                            EsriDatabaseHelper.ConvertFeatureClass2FeatureDataset(_curWS, outws, pDataset.Name, pDataset.Name, (outws as IFeatureWorkspace).OpenFeatureDataset("TDGX"), fieldName + " Like '" + dm + "%'");
                        }
                    }
                }
                wait.SetCaption("保存" + mc + "的地图文件……");
                IMapDocument pmxd = new MapDocumentClass();
                IMxdContents pMxdC = _curMap as IMxdContents;
                pmxd.New(folderName + "\\" + mc + "(" + dm + ").mxd");
                for (int n = 0; n < pMxdC.Map.LayerCount; n++)
                {
                    ILayer pLayer = pMxdC.Map.Layer[n];
                    if (pLayer is IFeatureLayer)
                    {
                        IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                        IDataset pDataset = pFeatureLayer.FeatureClass as IDataset; 
                        if (pDataset != null && (outws as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, pDataset.Name])
                        {
                            pFeatureLayer.FeatureClass = (outws as IFeatureWorkspace).OpenFeatureClass(pDataset.Name);
                        }
                    }
                    else if (pLayer is ICompositeLayer)
                    {
                        ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                        for (int m = 0; m < pCLayer.Count; m++)
                        {
                            ILayer pSubLayer = pCLayer.Layer[m];
                            IFeatureLayer pFeatureLayer = pSubLayer as IFeatureLayer;
                            IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                            if (pDataset != null && (outws as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, pDataset.Name])
                            {
                                pFeatureLayer.FeatureClass = (outws as IFeatureWorkspace).OpenFeatureClass(pDataset.Name);
                            }
                        }
                    }
                }
                pmxd.ReplaceContents(pMxdC);
                pmxd.Save(true, true);
                RCIS.Utility.OtherHelper.ReleaseComObject(penumFeatureClass);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFCContainer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureDataset);
                RCIS.Utility.OtherHelper.ReleaseComObject(outws);
            }
            for (int n = 0; n < _curMap.LayerCount; n++)
            {
                ILayer pLayer = _curMap.Layer[n];
                if (pLayer is IFeatureLayer)
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                    pFeatureLayer.FeatureClass = (_curWS as IFeatureWorkspace).OpenFeatureClass(pDataset.Name);
                }
                else if (pLayer is ICompositeLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int m = 0; m < pCLayer.Count; m++)
                    {
                        ILayer pSubLayer = pCLayer.Layer[m];
                        IFeatureLayer pFeatureLayer = pSubLayer as IFeatureLayer;
                        IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                        pFeatureLayer.FeatureClass = (_curWS as IFeatureWorkspace).OpenFeatureClass(pDataset.Name);
                    }
                }
            }
            wait.Close();
            MessageBox.Show("分库完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
