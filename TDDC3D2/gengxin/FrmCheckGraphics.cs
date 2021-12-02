using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.DataSourcesOleDB
using System.Data.OleDb;
using ESRI.ArcGIS.DataSourcesOleDB;
using ESRI.ArcGIS.Carto;

namespace TDDC3D.gengxin
{
    public partial class FrmCheckGraphics : Form
    {
        public FrmCheckGraphics()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        public IMapControl3 mapctl = null;
        IWorkspace pTmpWs = null;

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                double area = 0;
                double angle = 0;
                double.TryParse(txtAngle.Text, out angle);
                double.TryParse(txtArea.Text, out area);
                if (angle == 0)
                {
                    MessageBox.Show("请输入狭长图斑值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("请稍等", "正在处理...");
                wait.Show();

                //生成本地文件 写入上次面积和角度
                StreamWriter sw = new StreamWriter(Application.StartupPath + "\\tmp\\txtwriter.txt", false);
                sw.WriteLine(area);
                sw.WriteLine(angle);
                sw.Close();

                IFeatureClass pDLTBGX = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
                IFeatureClass pDLTB = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");

                //IWorkspace pTmpWs = null;
                string path = Application.StartupPath + "\\tmp\\tmpGraphics.gdb";
                if (Directory.Exists(path))
                {
                    try
                    {
                        pTmpWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                        IEnumDataset pEnumDataset = pTmpWs.get_Datasets(esriDatasetType.esriDTAny);
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
                        pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmpGraphics.gdb", null, 0);
                        pTmpWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmpGraphics.gdb");
                    }
                }
                else
                {
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmpGraphics.gdb", null, 0);
                    pTmpWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmpGraphics.gdb");
                }

                IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(pDLTBGX);
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = pGeo;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWs, pTmpWs, "DLTB", "TB", pSF as IQueryFilter);

                ESRI.ArcGIS.Geoprocessor.Geoprocessor gp = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Union pUnion = new ESRI.ArcGIS.AnalysisTools.Union();
                string gxPath = currWs.PathName + "\\TDGX\\DLTBGX";
                string tmppath = pTmpWs.PathName + "\\TB";
                pUnion.in_features = tmppath + ";" + gxPath;
                pUnion.out_feature_class = pTmpWs.PathName + "\\chechGraphics";
                pUnion.join_attributes = "ALL";
                try
                {
                    gp.Execute(pUnion, null);
                }
                catch
                {

                    string mess = "";
                    for (int i = 0; i < gp.MessageCount; i++)
                    {
                        mess += gp.GetMessage(i) + "\r\n";
                    }
                    wait.Close();
                    MessageBox.Show(mess.ToString());
                    //UpdateStatus("叠加分析错误");

                }

                string sql = "select OBJECTID,BSM FROM chechGraphics where (SHAPE_Area<" + area + ")";
                DataTable DT1 = ITable2DataTable(pTmpWs, sql);
                DT1.Columns[1].ColumnName = "图斑标识码";
                DT1.Columns.Add("描述");
                for (int i = 0; i < DT1.Rows.Count; i++)
                {
                    DT1.Rows[i][2] = "面积小于输入值";
                }

                sql = "select OBJECTID,BSM FROM chechGraphics where (SHAPE_Length*2/SHAPE_Area>" + angle + ")";
                DataTable DT2 = ITable2DataTable(pTmpWs, sql);
                DT2.Columns[1].ColumnName = "图斑标识码";
                DT2.Columns.Add("描述");
                for (int i = 0; i < DT2.Rows.Count; i++)
                {
                    DT2.Rows[i][2] = "角度大于输入值";
                }
                DT1.Merge(DT2);

                gridControl1.DataSource = DT1;
                wait.Close();
                MessageBox.Show("处理完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            }
            catch(Exception ex)
            {
                RCIS.Utility.LS_ErrorHelper.ShowErrorForm(ex,ex.ToString());
                return;
            }
            
            
        }

        public static DataTable ITable2DataTable(IWorkspace pWorkspace, string sql)
        {
            
            IFDOToADOConnection fdoToadoConnection = new FdoAdoConnectionClass();
            //ADODB.Connection adoConnection = (ADODB.Connection)fdoToadoConnection.CreateADOConnection(ipWS);
            ADODB.Connection adoConnection = new ADODB.Connection();
            fdoToadoConnection.Connect(pWorkspace, adoConnection);

            ADODB.Recordset adoRecordSet = new ADODB.Recordset();
            adoRecordSet.Open(sql, adoConnection, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic, 0);

            OleDbDataAdapter custDA = new OleDbDataAdapter();
            DataTable dt = new DataTable("ArcGISTable");
            custDA.Fill(dt, adoRecordSet);

            adoRecordSet.Close();
            adoConnection.Close();
            adoConnection = null;
            adoRecordSet = null;
            return dt;
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

        private void FrmCheckGraphics_Load(object sender, EventArgs e)
        {
            IFeatureClass pFeatureclass = null;
            string path = Application.StartupPath + "\\tmp\\tmpGraphics.gdb";
            try
            {
                pTmpWs = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                pFeatureclass = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("chechGraphics");
            }
            catch
            { }
            if (pFeatureclass != null)
            {
                //读取本地文件
                string txt1="", txt2="";
                if (File.Exists(Application.StartupPath + "\\tmp\\txtwriter.txt"))//判断文件是否存在
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\tmp\\txtwriter.txt", false);
                    txt1 = sr.ReadLine().ToString();
                    txt2 = sr.ReadLine().ToString();
                    sr.Close();
                }
                double area = 0;
                double angle = 0;
                double.TryParse(txt1, out area);
                double.TryParse(txt2, out angle);
                string sql = "select OBJECTID,BSM FROM chechGraphics where (SHAPE_Area<" + area + ")";
                DataTable DT1 = ITable2DataTable(pTmpWs, sql);
                DT1.Columns[1].ColumnName = "图斑标识码";
                DT1.Columns.Add("描述");
                for (int i = 0; i < DT1.Rows.Count; i++)
                {
                    DT1.Rows[i][2] = "面积小于输入值";
                }

                sql = "select OBJECTID,BSM FROM chechGraphics where (SHAPE_Length*2/SHAPE_Area>" + angle + ")";
                DataTable DT2 = ITable2DataTable(pTmpWs, sql);
                DT2.Columns[1].ColumnName = "图斑标识码";
                DT2.Columns.Add("描述");
                for (int i = 0; i < DT2.Rows.Count; i++)
                {
                    DT2.Rows[i][2] = "角度大于输入值";
                }
                DT1.Merge(DT2);

                gridControl1.DataSource = DT1;
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int selectRow = gridView1.GetSelectedRows()[0];
                string oid = this.gridView1.GetRowCellValue(selectRow, "OBJECTID").ToString();
                //IFeatureWorkspace pFW = currWs as IFeatureWorkspace;
                IFeatureClass pClass = (pTmpWs as IFeatureWorkspace).OpenFeatureClass("chechGraphics");
                IFeature pFeature = pClass.GetFeature(int.Parse(oid));
                ITopologicalOperator ptop = pFeature.ShapeCopy as ITopologicalOperator;
                IGeometry buffGeo = ptop.Buffer(1);
                IEnvelope env = buffGeo.Envelope;
                env.Expand(1.5, 1.5, true);
                this.mapctl.ActiveView.Extent = env;
                this.mapctl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapctl.ActiveView.Extent);
                this.mapctl.ActiveView.ScreenDisplay.UpdateWindow();
                this.mapctl.FlashShape(buffGeo, 3, 300, null);
            }
        }

        private void deleteRow_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                int selectRow = gridView1.GetSelectedRows()[0];
                string oid = this.gridView1.GetRowCellValue(selectRow, "OBJECTID").ToString();
                pTmpWs.ExecuteSQL("delete from chechGraphics where OBJECTID="+oid+"");
                FrmCheckGraphics_Load(null, null);
            }
        }
    }
}
