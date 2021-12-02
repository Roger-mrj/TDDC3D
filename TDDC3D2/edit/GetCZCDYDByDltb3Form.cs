using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geoprocessor;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using RCIS.Global;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace TDDC3D.edit
{
    public partial class GetCZCDYDByDltb3Form : Form
    {
        public GetCZCDYDByDltb3Form()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IMapControl2 pMapCtl = null;


      


        private void GetCZCDYDByDltb3Form_Load(object sender, EventArgs e)
        {
            this.xtraTabControl1.SelectedTabPageIndex = 0;
         

            this.cmbSrcLayer.Properties.Items.Clear();
            this.cmbTargetlayer.Properties.Items.Clear();

            LayerHelper.LoadLayer2Combox(this.cmbSrcLayer, currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbTargetlayer, currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbSrcLayer2, currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbTargetlayer2, currMap, esriGeometryType.esriGeometryPolygon);

            int idx1 = 0;
            int idx2 = 0;
            int idx3 = 0;
            int idx4 = 0;
            for (int i = 0; i < this.cmbSrcLayer.Properties.Items.Count; i++)
            {
                string txt = this.cmbSrcLayer.Properties.Items[i].ToString().Trim();
                if (OtherHelper.GetLeftName(txt).ToUpper() == "DLTB")
                {
                    idx1 = i;
                    break;
                }
            }
            for (int i = 0; i < this.cmbSrcLayer2.Properties.Items.Count; i++)
            {
                string txt = this.cmbSrcLayer2.Properties.Items[i].ToString().Trim();
                if (OtherHelper.GetLeftName(txt).ToUpper() == "DLTBGX")
                {
                    idx3 = i;
                    break;
                }
            }
            for (int i = 0; i < this.cmbTargetlayer.Properties.Items.Count; i++)
            {
                string txt = this.cmbTargetlayer.Properties.Items[i].ToString().Trim();
                if (OtherHelper.GetLeftName(txt).ToUpper() == "CZCDYD")
                {
                    idx2 = i;
                    break;
                }
            }
            for (int i = 0; i < this.cmbTargetlayer2.Properties.Items.Count; i++)
            {
                string txt = this.cmbTargetlayer2.Properties.Items[i].ToString().Trim();
                if (OtherHelper.GetLeftName(txt).ToUpper() == "CZCDYDGX")
                {
                    idx4 = i;
                    break;
                }
            }
            this.cmbSrcLayer.SelectedIndex = idx1;
            this.cmbTargetlayer.SelectedIndex = idx2;
            this.cmbSrcLayer2.SelectedIndex = idx3;
            this.cmbTargetlayer2.SelectedIndex = idx4;

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


        private bool CopyCzcdm(IFeatureClass srcFC, IFeatureClass czcdydClass)
        {
            //string className=System.IO.Path.GetFileNameWithoutExtension(srcShp);
            //IFeatureClass srcFC = WorkspaceHelper2.GetShapefileFeatureClass(srcShp);
            string name = (czcdydClass as IDataset).Name;
            RCIS.Global.GlobalEditObject.GlobalWorkspace.ExecuteSQL("delete from "+name+"");
            IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWsEdt.StartEditing(false);
            pWsEdt.StartEditOperation();
            string ysdm = GetValueFromMDBByLayerName(name);
            IFeatureCursor srcCursor = srcFC.Search(null, true);
            IFeatureCursor insertCursor = czcdydClass.Insert(true);
            try
            {
                IFeature srcFeature = null;
                while ((srcFeature = srcCursor.NextFeature()) != null)
                {
                    IFeatureBuffer feaBuffer = czcdydClass.CreateFeatureBuffer();
                    string czclx=FeatureHelper.GetFeatureStringValue(srcFeature,"CZCLX");
                    string czcdm=FeatureHelper.GetFeatureStringValue(srcFeature,"CZCDM");

                    FeatureHelper.SetFeatureBufferValue(feaBuffer, "CZCLX", czclx);
                    FeatureHelper.SetFeatureBufferValue(feaBuffer, "CZCDM", czcdm);
                    FeatureHelper.SetFeatureBufferValue(feaBuffer, "YSDM", ysdm);
                    FeatureHelper.SetFeatureBufferValue(feaBuffer, "GXSJ", "2020/12/31");
                    feaBuffer.Shape = srcFeature.Shape;
                    insertCursor.InsertFeature(feaBuffer);
                    insertCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(srcFeature);
                }


                pWsEdt.StopEditOperation();
                pWsEdt.StopEditing(true);
            }
            catch (Exception ex)
            {
                pWsEdt.AbortEditOperation();
                pWsEdt.StopEditing(false);
                return false;
            }
            finally
            {
                OtherHelper.ReleaseComObject(insertCursor);
                OtherHelper.ReleaseComObject(srcCursor);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            (srcFC as IDataset).Delete();


            return true;
        }

        private string GetValueFromMDBByLayerName(string layerName)
        {
            string sql = "select YSDM from SYS_YSDM where CLASSNAME='" + layerName + "'";
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable(sql, "tmp");
            return dt.Rows[0][0].ToString();
        }

        private void GetByDltb(IFeatureClass dltbClass, IFeatureClass czcdydClass)
        {

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行提取...", "请稍等...");
            wait.Show();

            IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWsEdt.StartEditing(false);
            pWsEdt.StartEditOperation();

            try
            {
                string where = "CZCSXM in ('201','201A','202','202A','203','203A','204','205')";
                IQueryFilter pQf = new QueryFilterClass();
                pQf.WhereClause = where;
                IFeatureCursor dltbCursor = dltbClass.Search(pQf, true);
                IFeature aDltb = null;

                IFeatureCursor insertCursor = czcdydClass.Insert(true);
                IFeatureBuffer pTargetBuffer = czcdydClass.CreateFeatureBuffer();    
                try
                {
                    while ((aDltb = dltbCursor.NextFeature()) != null)
                    {
                        IGeometry srcShp = aDltb.ShapeCopy;
                        string czclx = FeatureHelper.GetFeatureStringValue(aDltb, "CZCSXM");
                        pTargetBuffer.Shape = srcShp;
                        FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "YSDM", "2099010300");
                        FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCLX", czclx.Trim());
                        FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCMJ", FeatureHelper.GetFeatureDoubleValue(aDltb, "TBMJ"));
                        string zldwdm = FeatureHelper.GetFeatureStringValue(aDltb, "ZLDWDM");
                        string czcdm = zldwdm.Trim();
                        if (czclx.StartsWith("201"))
                        {
                            if (czcdm.Length > 6)
                            {
                                czcdm = czcdm.Substring(0, 6).PadRight(19, '0');
                            }
                        }
                        else if (czclx.StartsWith("202"))
                        {
                            if (czcdm.Length > 9)
                            {
                                czcdm = czcdm.Substring(0, 9).PadRight(19, '0');
                            }
                        }
                        FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCDM", czcdm.Trim());
                        insertCursor.InsertFeature(pTargetBuffer);
                        insertCursor.Flush();
                        RCIS.Utility.OtherHelper.ReleaseComObject(aDltb);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    OtherHelper.ReleaseComObject(dltbCursor);
                    OtherHelper.ReleaseComObject(pTargetBuffer);
                    OtherHelper.ReleaseComObject(insertCursor);
                }

                pWsEdt.StopEditOperation();
                pWsEdt.StopEditing(true);

                wait.Close();
                MessageBox.Show("提取完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pWsEdt.AbortEditOperation();
                pWsEdt.StopEditing(false);
                wait.Close();
                MessageBox.Show(ex.Message);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }


        private void getDltbToCzcgx(IFeatureClass dltbClass, IFeatureClass czcdydClass)
        {
            ISchemaLock pSchemaLock = null;
            pSchemaLock = dltbClass as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            IClassSchemaEdit4 pClassSchemaEdit = dltbClass as IClassSchemaEdit4;
            pClassSchemaEdit.AlterFieldName("CZCSXM", "CZCLX");
            pClassSchemaEdit.AlterFieldName("ZLDWDM", "CZCDM");
            //pClassSchemaEdit.AlterFieldName("CZCSXM", "CZCLX");
            pClassSchemaEdit.AlterFieldName("ZLDWMC", "CZCMC");
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            (dltbClass as IDataset).Workspace.ExecuteSQL("delete from " + (dltbClass as IDataset).Name + " where CZCLX='' or CZCLX is null");
            bool b = RCIS.GISCommon.GpToolHelper.Append((dltbClass as IDataset).Workspace.PathName + "\\" + (dltbClass as IDataset).Name, (czcdydClass as IDataset).Workspace.PathName + "\\TDGX\\CZCDYDGX");
            if (!b)
            {
                MessageBox.Show("gp工具错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "CZCLX like '202%' or CZCLX like '201%'";
            IFeatureCursor pCursor = czcdydClass.Update(pQf,true);
            IFeature pFea ;
            while ((pFea = pCursor.NextFeature()) != null)
            {
                string czcdm = pFea.get_Value(pFea.Fields.FindField("CZCDM")).ToString().Trim();
                string czclx = pFea.get_Value(pFea.Fields.FindField("CZCLX")).ToString().Trim();
                if (czclx.StartsWith("201"))
                    pFea.set_Value(pFea.Fields.FindField("CZCDM"), czcdm.Substring(0, 6).PadRight(19, '0'));
                else if (czclx.StartsWith("202"))
                    pFea.set_Value(pFea.Fields.FindField("CZCDM"), czcdm.Substring(0, 9).PadRight(19, '0'));
                pCursor.UpdateFeature(pFea);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFea);
            }
            pCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

            //(czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set CZCDM=substring(CZCDM,1,6)+'0000000000000' where CZCLX like '201%'");
            //(czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set CZCDM=substring(CZCDM,1,9)+0000000000 where CZCLX like '202%'");
            
        }
               

        private void btnOk_Click(object sender, EventArgs e)
        {
            //相当于 提取 201，202，203，204，205 ，然后 ，修改城镇村等用地类型 ，城镇村代码，城镇村面积
            if (this.cmbSrcLayer.Text.Trim() == "" || this.cmbTargetlayer.Text.Trim() == "")
                return;

            string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            string czcdydClassName = OtherHelper.GetLeftName(this.cmbTargetlayer.Text);
            IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, dltbClassName);
            IFeatureLayer czcdydLayer = LayerHelper.QueryLayerByModelName(this.currMap, czcdydClassName);
            if (dltbLayer == null || czcdydLayer == null)
            {
                MessageBox.Show("加载图层不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass dltbClass = dltbLayer.FeatureClass;
            IFeatureClass czcdydClass = czcdydLayer.FeatureClass;
            switch (MessageBox.Show("是否删除城镇村图层中原有的数据？","提示",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question))
            {
                case DialogResult.Cancel:
                    return;
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
                    pWsEdt.StartEditing(false);
                    pWsEdt.StartEditOperation();
                    ITable pTable = czcdydClass as ITable;
                    pTable.DeleteSearchedRows(null);
                    pWsEdt.StopEditOperation();
                    pWsEdt.StopEditing(true);
                    break;
                default:
                    break;
            }
            GetByDltb(dltbClass, czcdydClass);

            this.pMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

     

        private bool DissovePolygon(string destShp)
        {
            string className = "CZCDYD";
            
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.Dissolve dissolve = new ESRI.ArcGIS.DataManagementTools.Dissolve();
                string inParam = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\TDDC\\" + className;
                dissolve.in_features = inParam;
                dissolve.multi_part = "SINGLE_PART";
                dissolve.dissolve_field = "CZCLX;CZCDM";
                dissolve.out_feature_class = destShp;                              
              
                gp.Execute(dissolve, null);
                string sMsg = GpToolHelper.ReturnMessages(gp);

                if (sMsg.Contains("ERROR") || sMsg.Contains("失败"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

       
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            //执行融合string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            string czcdydClassName = OtherHelper.GetLeftName(this.cmbTargetlayer.Text);
            IFeatureLayer czcdydLayer = LayerHelper.QueryLayerByModelName(this.currMap, czcdydClassName);
            if ( czcdydLayer == null)
            {
                MessageBox.Show("加载图层不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass czcdydClass = czcdydLayer.FeatureClass;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string tmpPath = Application.StartupPath + "\\tmp\\";
                string tmpShpfile = tmpPath + "czcydtmp.shp";
                //bool bOk = DissovePolygon(tmpShpfile);

                string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
                IWorkspace tmpWS = null;
                if (Directory.Exists(tmp))
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                else
                {
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                }
                string[] arr={"CZCLX","CZCDM"};
                IFeatureClass pTmpClass=RCIS.GISCommon.GpToolHelper.Dissolve(czcdydClass, tmpWS, "czcdydDissolve", arr);
                if (pTmpClass==null)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("融合过程失败");
                    return;
                }
                if (CopyCzcdm(pTmpClass, czcdydClass))
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("融合完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtXZQDM.Text))
            {
                MessageBox.Show("当前县代码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //相当于 提取 201，202，203，204，205 ，然后 ，修改城镇村等用地类型 ，城镇村代码，城镇村面积
            if (this.cmbSrcLayer.Text.Trim() == "" || this.cmbTargetlayer.Text.Trim() == "")
                return;
            
            string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer2.Text);
            string czcdydClassName = OtherHelper.GetLeftName(this.cmbTargetlayer2.Text);
            IFeatureLayer dltbLayer = LayerHelper.QueryLayerByModelName(this.currMap, dltbClassName);
            IFeatureLayer czcdydLayer = LayerHelper.QueryLayerByModelName(this.currMap, czcdydClassName);
            if (dltbLayer == null || czcdydLayer == null)
            {
                MessageBox.Show("加载图层不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass dltbClass = dltbLayer.FeatureClass;
            IFeatureClass czcdydClass = czcdydLayer.FeatureClass;
           
            //判断城镇村属性码是否标准
            List<string> czcArr =new List<string>() { "201", "202", "203", "204", "205", "201A", "202A", "203A" };
            ArrayList czcsxmArr = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(dltbClass, null, "CZCSXM");
            List<string> erroDm = new List<string>();
            string erroStr = "";
            foreach (string item in czcsxmArr)
            {
                if (!czcArr.Contains(item)&&!string.IsNullOrWhiteSpace(item))
                    erroDm.Add(item);
            }
            if (erroDm.Count > 0)
            {
                erroStr = "地类图斑更新层中城镇村属性码为";
                for (int i = 0; i < erroDm.Count; i++)
                {
                    erroStr += "'"+erroDm[i] + "',";
                }
                erroStr += "填写不规范。";
            }
            if (erroStr.Length > 0)
            {
                MessageBox.Show(erroStr, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //List<string>

            switch (MessageBox.Show("是否删除城镇村更新层中原有的数据？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Cancel:
                    return;
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Yes:
                    IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
                    pWsEdt.StartEditing(false);
                    pWsEdt.StartEditOperation();
                    ITable pTable = czcdydClass as ITable;
                    pTable.DeleteSearchedRows(null);
                    pWsEdt.StopEditOperation();
                    pWsEdt.StopEditing(true);
                    break;
                default:
                    break;
            }
            //try
            //{
            //    if (Directory.Exists(Application.StartupPath + @"\tmp\tmp.gdb"))
            //    {
            //        IWorkspace pWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            //        IDataset pDataset = pWS as IDataset;
            //        pDataset.Delete();

            //    }
            //    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
            //    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
            //}
            //catch (Exception ex) { }
            //IWorkspaceFactory tmpWorkspaceFactory = new FileGDBWorkspaceFactory();
            //IWorkspace tmpWS = tmpWorkspaceFactory.OpenFromFile(Application.StartupPath + @"\tmp\tmp.gdb", 0);
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行提取...", "请稍等...");
            wait.Show();
            IWorkspace tmpWS = DeleteAndNewTmpGDB();
            bool b = RCIS.GISCommon.GpToolHelper.Update(GlobalEditObject.GlobalWorkspace.PathName + @"\TDDC\DLTB", GlobalEditObject.GlobalWorkspace.PathName + @"\TDGX\DLTBGX", Application.StartupPath + @"\tmp\tmp.gdb\NewDLTB");

            ////将调出的数据保存，之后用来擦除
            IQueryFilter pQf = new QueryFilterClass();
            pQf.WhereClause = "xzqdm not like '" + txtXZQDM.Text.ToString().Trim() + "%'";
            RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(RCIS.Global.GlobalEditObject.GlobalWorkspace, tmpWS, "XZQGX", "XZQOut", pQf);
            b = RCIS.GISCommon.GpToolHelper.Erase_analysis(Application.StartupPath + @"\tmp\tmp.gdb\NewDLTB", tmpWS.PathName + "\\XZQOut", tmpWS.PathName + "\\newTB");

            getDltbToCzcgx((tmpWS as IFeatureWorkspace).OpenFeatureClass("newTB"), czcdydClass);
            this.pMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            wait.Close();
            MessageBox.Show("提取完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //执行融合string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            string czcdydClassName = OtherHelper.GetLeftName(this.cmbTargetlayer2.Text);
            IFeatureLayer czcdydLayer = LayerHelper.QueryLayerByModelName(this.currMap, czcdydClassName);
            if (czcdydLayer == null)
            {
                MessageBox.Show("加载图层不全！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行数据融合","请稍等...");
            wait.Show();
            IFeatureClass czcdydClass = czcdydLayer.FeatureClass;

            //(czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set CZCDM=substring(CZCDM,1,9),BZ='0000000000' where CZCLX like '202%'");
            //(czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set CZCDM=BZ where CZCLX like '202%'");


            this.Cursor = Cursors.WaitCursor;
            try
            {
                string tmpPath = Application.StartupPath + "\\tmp\\";
                string tmpShpfile = tmpPath + "czcydtmp.shp";
                //bool bOk = DissovePolygon(tmpShpfile);

                string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
                IWorkspace tmpWS = DeleteAndNewTmpGDB();
                //if (Directory.Exists(tmp))
                //{
                //    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                //    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                //    pEnumDataset.Reset();
                //    IDataset pDataset;
                //    while ((pDataset = pEnumDataset.Next()) != null)
                //    {
                //        pDataset.Delete();
                //    }
                //}
                //else
                //{
                //    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                //    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                //    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                //}
                string[] arr = { "CZCLX", "CZCDM" };
                //IFeatureClass pTmpClass = RCIS.GISCommon.GpToolHelper.Dissolve(czcdydClass, tmpWS, "czcdydDissolve", arr);
                object inFea = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\TDGX\\CZCDYDGX";
                object outFea = tmpWS.PathName + "\\czcdydDissolve";
                RCIS.GISCommon.GpToolHelper gp = new GpToolHelper();
                bool b = gp.Dissolve(inFea, outFea, arr);
                IFeatureClass pTmpClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("czcdydDissolve");

                if (pTmpClass == null)
                {
                    wait.Close();
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("融合过程失败");
                    return;
                }
                //if (CopyCzcdm(pTmpClass, czcdydClass))
                //{
                //    wait.Close();
                //    this.Cursor = Cursors.Default;
                //    MessageBox.Show("融合完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                string name = (czcdydClass as IDataset).Name;
                RCIS.Global.GlobalEditObject.GlobalWorkspace.ExecuteSQL("delete from " + name + "");
                if (RCIS.GISCommon.GpToolHelper.Append(tmpWS.PathName + "\\czcdydDissolve", (czcdydClass as IDataset).Workspace.PathName+"\\TDGX\\CZCDYDGX"))
                {
                    //(czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set GXSJ="+DateTime.Parse("2020/12/31")+"");
                    (czcdydClass as IDataset).Workspace.ExecuteSQL("update " + (czcdydClass as IDataset).Name + " set YSDM='2099010300'");

                    wait.Close();
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("融合完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                wait.Close();
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
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
