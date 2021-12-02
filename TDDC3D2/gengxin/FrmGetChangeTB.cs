using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TDDC3D.gengxin
{
    public partial class FrmGetChangeTB : Form
    {
        public FrmGetChangeTB()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;

        private void FrmGetChangeTB_Load(object sender, EventArgs e)
        {
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureDataset pFeaDS = pFeaWs.OpenFeatureDataset("TDGX");
            List<IFeatureClass> allFC = DatabaseHelper.getAllFeatureClass(pFeaDS);
            cmbXq.Properties.Items.Clear();
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;
                if (className == "CJDCQGX") cmbXq.Text = className + "|" + alias;
                this.cmbXq.Properties.Items.Add(className + "|" + alias);
            }
        }

        private void UpdateStatus(string txt)
        {
            info.Text = DateTime.Now.ToShortTimeString() + ":" + txt + "\r\n" + info.Text;
            Application.DoEvents();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbXq.Text))
            {
                MessageBox.Show("图层选择错误。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UpdateStatus("正在准备数据……");
            string className = cmbXq.Text.Substring(0,cmbXq.Text.IndexOf("|"));
            IWorkspace pTmpWS = DeleteAndNewTmpGDB();
            UpdateStatus("正在提取变化图斑……");
            bool b = RCIS.GISCommon.GpToolHelper.Intersect_analysis(currWs.PathName+"\\TDDC\\DLTB"+";"+currWs.PathName+"\\TDGX\\"+className+"",pTmpWS.PathName+"\\bhtb");
            pTmpWS.ExecuteSQL("delete from bhtb where zldwdm=zldwdm_1 and zldwmc=zldwmc_1");
            pTmpWS.ExecuteSQL("update bhtb set zldwdm=zldwdm_1,zldwmc=zldwmc_1");

            //赋值权属单位代码
            pTmpWS.ExecuteSQL("update bhtb set qsdwdm=zldwdm_1,qsdwmc=zldwmc_1 where qsxz<>'10' and qsxz<>'20'");
            IQueryFilter pQFil = new QueryFilterClass();
            pQFil.WhereClause = "qsxz='10' or qsxz='20'";
            IFeatureClass pFeaClass = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("bhtb");
            RCIS.GISCommon.DatabaseHelper.CreateIndex(pFeaClass, "QSXZ");
            IFeatureCursor pFeaCur = pFeaClass.Update(pQFil,true);
            IFeature pFea;
            while((pFea=pFeaCur.NextFeature())!=null)
            {
                string qsdwdm_1 = pFea.get_Value(pFea.Fields.FindField("ZLDWDM_1")).ToString();
                qsdwdm_1 = qsdwdm_1.Substring(0,12);
                string qsdwdm = pFea.get_Value(pFea.Fields.FindField("QSDWDM")).ToString();
                qsdwdm = qsdwdm.Substring(12);
                qsdwdm = qsdwdm_1 + qsdwdm;
                pFea.set_Value(pFea.Fields.FindField("QSDWDM"),qsdwdm);
                pFeaCur.UpdateFeature(pFea);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFea);
            }
            pFeaCur.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCur);
            RCIS.GISCommon.DatabaseHelper.DeleteIndex(pFeaClass, "QSXZ");
            //

            //RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWS, "DLTBGX", "DLTBGX", null);
            IQueryFilter pQF=new QueryFilterClass();
            pQF.WhereClause="1=0";
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWS, "DLTBGX", "BHTBGX", pQF);
            RCIS.Utility.OtherHelper.ReleaseComObject(pQF);
            b=RCIS.GISCommon.GpToolHelper.Append(pTmpWS.PathName + "\\bhtb", pTmpWS.PathName + "\\BHTBGX");

            //IFeatureClass pFeaClass = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("bhtb");
            //IFeatureCursor pFeaCursor = pFeaClass.Search(null,true);
            //IFeatureClass pBHTBGX = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("BHTBGX");
            //IFeatureCursor pInsert = pBHTBGX.Insert(true);
            //IFeature pFeature;
            //UpdateStatus("正在为变化图斑添加属性……");
            //while ((pFeature = pFeaCursor.NextFeature()) != null)
            //{
            //    IFeatureBuffer pBuffer = pBHTBGX.CreateFeatureBuffer();
            //    pBuffer.Shape = pFeature.ShapeCopy;
            //    pBuffer.set_Value(pBuffer.Fields.FindField("BSM"), pFeature.get_Value(pFeature.Fields.FindField("BSM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("YSDM"), pFeature.get_Value(pFeature.Fields.FindField("YSDM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("TBYBH"), pFeature.get_Value(pFeature.Fields.FindField("TBYBH")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("TBBH"), pFeature.get_Value(pFeature.Fields.FindField("TBBH")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("DLBM"), pFeature.get_Value(pFeature.Fields.FindField("DLBM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("DLMC"), pFeature.get_Value(pFeature.Fields.FindField("DLMC")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("QSXZ"), pFeature.get_Value(pFeature.Fields.FindField("QSXZ")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("QSDWDM"), pFeature.get_Value(pFeature.Fields.FindField("QSDWDM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("QSDWMC"), pFeature.get_Value(pFeature.Fields.FindField("QSDWMC")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("ZLDWDM"), pFeature.get_Value(pFeature.Fields.FindField("ZLDWDM_1")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("ZLDWMC"), pFeature.get_Value(pFeature.Fields.FindField("ZLDWMC_1")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("TBMJ"), pFeature.get_Value(pFeature.Fields.FindField("TBMJ")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("KCDLBM"), pFeature.get_Value(pFeature.Fields.FindField("KCDLBM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("KCXS"), pFeature.get_Value(pFeature.Fields.FindField("KCXS")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("KCMJ"), pFeature.get_Value(pFeature.Fields.FindField("KCMJ")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("TBDLMJ"), pFeature.get_Value(pFeature.Fields.FindField("TBDLMJ")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("GDLX"), pFeature.get_Value(pFeature.Fields.FindField("GDLX")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("GDPDJB"), pFeature.get_Value(pFeature.Fields.FindField("GDPDJB")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("XZDWKD"), pFeature.get_Value(pFeature.Fields.FindField("XZDWKD")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("TBXHDM"), pFeature.get_Value(pFeature.Fields.FindField("TBXHDM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("TBXHMC"), pFeature.get_Value(pFeature.Fields.FindField("TBXHMC")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("ZZSXDM"), pFeature.get_Value(pFeature.Fields.FindField("ZZSXDM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("ZZSXMC"), pFeature.get_Value(pFeature.Fields.FindField("ZZSXMC")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("GDDB"), pFeature.get_Value(pFeature.Fields.FindField("GDDB")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("FRDBS"), pFeature.get_Value(pFeature.Fields.FindField("FRDBS")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("CZCSXM"), pFeature.get_Value(pFeature.Fields.FindField("CZCSXM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("SJNF"), pFeature.get_Value(pFeature.Fields.FindField("SJNF")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("MSSM"), pFeature.get_Value(pFeature.Fields.FindField("MSSM")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("HDMC"), pFeature.get_Value(pFeature.Fields.FindField("HDMC")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("BZ"), pFeature.get_Value(pFeature.Fields.FindField("BZ")));
            //    pBuffer.set_Value(pBuffer.Fields.FindField("GXSJ"), "2020/12/31");
            //    pInsert.InsertFeature(pBuffer);
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pBuffer);
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            //}
            //pInsert.Flush();
            //RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            //RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            //RCIS.GISCommon.GpToolHelper.RepairGeometry(pTmpWS.PathName + "\\BHTBGX");
            UpdateStatus("正在合并变化图斑……");
            b = RCIS.GISCommon.GpToolHelper.Update(pTmpWS.PathName + "\\BHTBGX", currWs.PathName + "\\TDGX\\DLTBGX", pTmpWS.PathName + "\\DLTBGX");
            currWs.ExecuteSQL("delete from DLTBGX");
            b = RCIS.GISCommon.GpToolHelper.Append(pTmpWS.PathName + "\\DLTBGX", currWs.PathName + "\\TDGX\\DLTBGX");
            //RCIS.Utility.OtherHelper.ReleaseComObject(pFeaClass);
            //RCIS.Utility.OtherHelper.ReleaseComObject(pBHTBGX);
            RCIS.Utility.OtherHelper.ReleaseComObject(pTmpWS);
            
            UpdateStatus("变化图斑提取完毕……");
            MessageBox.Show("完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private IWorkspace DeleteAndNewTmpGDB()
        {
            string path = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;

            if (Directory.Exists(path))
            {
                try
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                catch
                {
                    foreach (string tmp in Directory.GetFileSystemEntries(path))
                    {
                        if (File.Exists(tmp))
                        {
                            //如果有子文件删除文件
                            File.Delete(tmp);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(path);
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            return tmpWS;
        }
    }
}
