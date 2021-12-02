using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;

using RCIS.GISCommon;
using System.Runtime.InteropServices;


namespace RCIS.Controls
{
    public partial class LayoutControlPanel : UserControl
    {

        public AxPageLayoutControl LayoutControl
        {
            get
            {
                return this.axPageLayoutControl;
            }
        }

        public AxToolbarControl LayoutBarControl
        {
            get
            {
                return this.axToolbarControl1;
            }
        }

        private IMapDocument m_MapDocument;
        private List<IGeoFeatureLayer> m_pLayerList = new List<IGeoFeatureLayer>();

        private void SaveDocument()
        {
            try
            {
                //Check that the document is not read only
                if (m_MapDocument.get_IsReadOnly(m_MapDocument.DocumentFilename) == true)
                {

                    MessageBox.Show("该地图文档为只读文件，不能修改");
                    return;
                }
                //Save with the current relative path setting
                m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);

            }
            catch { }
        }
        public string GetRightName(string sName, string sflag)
        {
            try
            {
                string ss;
                if (sName == "")
                {
                    return "";
                }
                if (sName.Contains(sflag))
                {
                    ss = sName.Substring(sName.IndexOf(sflag) + 1, sName.Length - sName.IndexOf(sflag) - 1).Trim();
                }
                else
                {
                    return sName;
                }
                return ss;
            }
            catch { return ""; }
        }

        private bool CheckIsNeedExport(string sLayerName)
        {
            try
            {
                if (this.m_pLayerList.Count == 0) return true;
                for (int i = 0; i < m_pLayerList.Count; i++)
                {
                    string sName = (m_pLayerList[i].FeatureClass as IDataset).Name;
                    if (sName.Contains("."))
                    {
                        sName = GetRightName(sName, ".");
                    }
                    if (sName == sLayerName)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        public void EditMXD(IMapDocument m_MapDoc, string sPath)
        {

            IMap pMap = m_MapDoc.get_Map(0);
            ILayer pLayer = null;
            IDataLayer2 pDataLayer = null;
            IWorkspaceName pWorkspaceName = null;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                pLayer = pMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = pLayer as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IDataLayer2)
                        {
                            IDatasetName pDataName = null;
                            pDataLayer = childLyr as IDataLayer2;
                            try
                            {
                                pDataName = pDataLayer.DataSourceName as IDatasetName;
                                try
                                {
                                    pDataLayer.Disconnect();
                                }
                                catch { }
                                pDataName = pDataLayer.DataSourceName as IDatasetName;
                                pWorkspaceName = WorkspaceHelper2.GetWorkspaceName(sPath);

                                pDataLayer.DataSourceName = pWorkspaceName as IName;
                                pDataName.WorkspaceName = pWorkspaceName;
                                pDataLayer.Connect(pDataName as IName);
                                pDataName = pDataLayer.DataSourceName as IDatasetName;


                            }
                            catch (Exception ex) { }
                        }

                    }
                }
                else if (pLayer is IDataLayer2)
                {
                    IDatasetName pDataName = null;
                    pDataLayer = pLayer as IDataLayer2;
                    try
                    {
                        pDataName = pDataLayer.DataSourceName as IDatasetName;
                        try
                        {
                            pDataLayer.Disconnect();
                        }
                        catch { }
                        pDataName = pDataLayer.DataSourceName as IDatasetName;
                        pWorkspaceName = WorkspaceHelper2.GetWorkspaceName(sPath);

                        pDataLayer.DataSourceName = pWorkspaceName as IName;
                        pDataName.WorkspaceName = pWorkspaceName;
                        pDataLayer.Connect(pDataName as IName);
                        pDataName = pDataLayer.DataSourceName as IDatasetName;


                    }
                    catch (Exception ex) { }
                }
            }
            m_MapDoc.Save(true, true);
            m_MapDoc.ActiveView.Refresh();

        }

        public void MxdEdit(IMapDocument m_MapDoc, string sPath)
        {

            IMap pMap = m_MapDoc.get_Map(0);
            ILayer pLayer = null;
            IDataLayer2 pDataLayer = null;
            IWorkspaceName pWorkspaceName = null;


            for (int i = 0; i < pMap.LayerCount; i++)
            {
                pLayer = pMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = pLayer as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IDataLayer2)
                        {
                            IDatasetName pDataName = null;
                            pDataLayer = childLyr as IDataLayer2;
                            try
                            {
                                pDataName = pDataLayer.DataSourceName as IDatasetName;
                                try
                                {
                                    pDataLayer.Disconnect();
                                }
                                catch { }
                                pDataName = pDataLayer.DataSourceName as IDatasetName;
                                pWorkspaceName = WorkspaceHelper2.GetWorkspaceName(sPath);
                                pDataLayer.DataSourceName = pWorkspaceName as IName;
                                pDataName.WorkspaceName = pWorkspaceName;
                                pDataLayer.Connect(pDataName as IName);
                                pDataName = pDataLayer.DataSourceName as IDatasetName;


                            }
                            catch (Exception ex) { }
                        }

                    }
                }
                else
                    if (pLayer is IDataLayer2)
                    {
                        IDatasetName pDataName = null;
                        pDataLayer = pLayer as IDataLayer2;
                        try
                        {
                            pDataName = pDataLayer.DataSourceName as IDatasetName;
                            try
                            {
                                pDataLayer.Disconnect();
                            }
                            catch { }
                            pDataName = pDataLayer.DataSourceName as IDatasetName;
                            pWorkspaceName = WorkspaceHelper2.GetWorkspaceName(sPath);
                            pDataLayer.DataSourceName = pWorkspaceName as IName;
                            pDataName.WorkspaceName = pWorkspaceName;
                            pDataLayer.Connect(pDataName as IName);
                            pDataName = pDataLayer.DataSourceName as IDatasetName;


                        }
                        catch (Exception ex) { }
                    }
            }
            m_MapDoc.Save(true, true);
            m_MapDoc.ActiveView.Refresh();

        }

        private IWorkspace CreateAccessWorkspace(string sFilePath)
        {
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            IWorkspaceName workspaceName = workspaceFactory.Create(System.IO.Path.GetDirectoryName(sFilePath),
                System.IO.Path.GetFileNameWithoutExtension(sFilePath) + ".mdb", null, 0);
            ESRI.ArcGIS.esriSystem.IName name = (ESRI.ArcGIS.esriSystem.IName)workspaceName;
            //Open a reference to the access workspace through the name object        
            IWorkspace pGDB_workspace = (IWorkspace)name.Open();
            return pGDB_workspace;
        }

        private IFeatureClass CreateFeatureCls(IFeatureDataset destDataset, IFeatureClass srcFeatCls)
        {

            //建立字段集合:
            IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

            //建立字段:ObjectID
            IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = "OBJECTID";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            pFieldsEdit.AddField(pField);

            //create the geometry field
            pField = new FieldClass();
            pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = "Shape";
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            IGeometryDef pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            pGeoDefEdit.SpatialReference_2 = (srcFeatCls as IGeoDataset).SpatialReference;
            pGeoDefEdit.GridCount_2 = 1;
            pGeoDefEdit.set_GridSize(0, 0.5);
            pGeoDefEdit.AvgNumPoints_2 = 2;
            pGeoDefEdit.HasM_2 = false;
            pGeoDefEdit.HasZ_2 = false;
            pGeoDefEdit.GeometryType_2 = srcFeatCls.ShapeType; // NewFeatureClassType;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pField);

            //加其他的字段...
            IFields srcFields = srcFeatCls.Fields;
            string sShapeFieldName = "";
            for (int index = 0; index < srcFeatCls.Fields.FieldCount; index++)
            {

                IField srcField = srcFeatCls.Fields.get_Field(index);
                if ((srcField.Type == esriFieldType.esriFieldTypeGeometry)
                    || (srcField.Type == esriFieldType.esriFieldTypeOID))
                {
                    continue;
                }
                pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = srcField.Name;
                pFieldEdit.Type_2 = srcField.Type;
                pFieldsEdit.AddField(pField);
            }
            for (int index = 0; index < pFields.FieldCount; index++)
            {

                IField srcField = pFields.get_Field(index);
                if (srcField.Type == esriFieldType.esriFieldTypeGeometry)
                {
                    sShapeFieldName = srcField.Name;
                    break;
                }
            }
            string destFeatLyrName = LayerHelper.GetFeatureLayerTableName(srcFeatCls);
            IFeatureClass destFeatCls = destDataset.CreateFeatureClass(destFeatLyrName, pFields, null, null,
                    esriFeatureType.esriFTSimple, sShapeFieldName, "");
            return destFeatCls;

        }

        public LayoutControlPanel()
        {
            InitializeComponent();
        }

        private void 保存文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "另存为";
            saveFileDialog1.Filter = "地图文件 (*.mxd)|*.mxd";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string sFilePath = saveFileDialog1.FileName;
            if (sFilePath.Trim() == "")
                return;

            m_MapDocument = null;


            try
            {
                IMxdContents pMxd = this.axPageLayoutControl.PageLayout as IMxdContents;
                m_MapDocument = new MapDocumentClass();
                m_MapDocument.New(saveFileDialog1.FileName);

                m_MapDocument.ReplaceContents(pMxd);
                m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存mxd文档时出错!" + ex.ToString());
                return;
            }

            IMap pMap = m_MapDocument.get_Map(0);


            DialogResult dr = MessageBox.Show("保存完毕，是否导出与MXD文件相关联的数据源文件？", "提示",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DialogResult.Yes == dr)
            {
                DevExpress.Utils.WaitDialogForm waitFrm = new DevExpress.Utils.WaitDialogForm("请稍等...", "开始创建数据表结构...");
                waitFrm.Show();
                try
                {
                    sFilePath = System.IO.Path.GetDirectoryName(saveFileDialog1.FileName);
                    sFilePath = sFilePath + "\\" + System.IO.Path.GetFileNameWithoutExtension(saveFileDialog1.FileName) + ".mdb";
                    if (System.IO.File.Exists(sFilePath))
                    {
                        System.IO.File.Delete(sFilePath);
                    }


                    IGeometry pClipGeo = pMap.ClipGeometry;                  //pMap.ClipGeometry;

                    if (pClipGeo != null) pClipGeo = (pClipGeo as ITopologicalOperator).Buffer(1);

                    #region  创建 mdb，

                    //TaskDiscriptor.TaskCaption = "开始创建数据表...";
                    IWorkspace pGDB_workspace = CreateAccessWorkspace(sFilePath);
                    if (pGDB_workspace == null)
                    {
                        return;
                    }
                    //创建数据集

                    ISpatialReference pSf = pMap.SpatialReference;
                    IFeatureDataset destDataset = (pGDB_workspace as IFeatureWorkspace).CreateFeatureDataset("TDDC", pSf);

                    #region 创建表结构
                    //for (int i = 0; i < pMap.LayerCount; i++)
                    //{
                    //    if (pMap.get_Layer(i).Visible)
                    //    {
                    //        ILayer srcLyr = pMap.get_Layer(i);
                    //        if (srcLyr is IGroupLayer)
                    //        {
                    //            ICompositeLayer compositeLayer = srcLyr as ICompositeLayer;
                    //            for (int kk = 0; kk < compositeLayer.Count; kk++)
                    //            {
                    //                ILayer childLyr = compositeLayer.get_Layer(kk);
                    //                if (childLyr is IFeatureLayer)
                    //                {
                    //                    IFeatureLayer srcfeaLyr = childLyr as IFeatureLayer;
                    //                    IFeatureClass srcFeaCls = srcfeaLyr.FeatureClass;
                    //                    CreateFeatureCls(destDataset, srcFeaCls);
                    //                }

                    //            }

                    //        }
                    //        else if (srcLyr is IFeatureLayer)
                    //        {
                    //            IFeatureLayer srcfeaLyr = srcLyr as IFeatureLayer;
                    //            IFeatureClass srcFeaCls = srcfeaLyr.FeatureClass;

                    //            CreateFeatureCls(destDataset, srcFeaCls);
                    //        }
                    //    }
                    //}


                    #endregion


                    //然后导出数据 到该数据集中
                    IWorkspaceEdit destWsEdt = pGDB_workspace as IWorkspaceEdit;
                    //destWsEdt.StartEditing(true);
                    //destWsEdt.StartEditOperation();
                    //获取当前的数据
                    #endregion


                    for (int i = 0; i < pMap.LayerCount; i++)
                    {
                        ILayer srcLyr = pMap.get_Layer(i);
                        if (!srcLyr.Visible)
                            continue;
                        waitFrm.SetCaption("现导出第" + (i + 1) + "个，名称为：" + srcLyr.Name);
                        Application.DoEvents();

                        #region 导入当前表数据
                        if (srcLyr is IGroupLayer)
                        {
                            ICompositeLayer compositeLayer = srcLyr as ICompositeLayer;
                            for (int kk = 0; kk < compositeLayer.Count; kk++)
                            {
                                ILayer childLyr = compositeLayer.get_Layer(kk);
                                if (childLyr is IFeatureLayer)
                                {
                                    ImportCurrData(pGDB_workspace, childLyr, pClipGeo);
                                }
                            }
                        }
                        else if (srcLyr is IFeatureLayer)
                        {
                            ImportCurrData(pGDB_workspace, srcLyr, pClipGeo);
                        }

                        #endregion

                    }
                    //destWsEdt.StopEditOperation();
                    //destWsEdt.StopEditing(true);

                    MxdEdit(this.m_MapDocument, sFilePath);
                    waitFrm.Close();
                    MessageBox.Show("保存成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (waitFrm != null)
                    {
                        if (waitFrm.Visible)
                        {
                            waitFrm.Close();
                        }
                    }
                    MessageBox.Show(ex.ToString());

                }
                finally
                {
                    if (waitFrm != null)
                    {
                        if (waitFrm.Visible)
                        {
                            waitFrm.Close();
                        }
                    }
                }

            }




        }

        private void ImportCurrData(IWorkspace pGDB_workspace, ILayer srcLyr, IGeometry pClipGeo)
        {
            IFeatureLayer srcfeaLyr = srcLyr as IFeatureLayer;
            IFeatureClass lyrFeaCls = srcfeaLyr.FeatureClass;
            string destFeatLyrName = LayerHelper.GetFeatureLayerTableName(lyrFeaCls);


            //IFeatureClass destFeatCls = null;
            //try
            //{
            //    destFeatCls = (pGDB_workspace as IFeatureWorkspace).OpenFeatureClass(destFeatLyrName);
            //}
            //catch { }
            //if (destFeatCls == null)
            //    return;
            //查询出当前所出数据
            //ISpatialFilter pSpFilter = new SpatialFilterClass();
            //pClipGeo.Project((lyrFeaCls as IGeoDataset).SpatialReference);
            //pClipGeo.SpatialReference = (lyrFeaCls as IGeoDataset).SpatialReference;
            //pClipGeo.SnapToSpatialReference();
            //pSpFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //pSpFilter.Geometry = pClipGeo;
            //pClipGeo.Project(pClipGeo.SpatialReference);
            //IFeatureCursor srcCursor = lyrFeaCls.Search(null, false);
            //IFeature srcFeat = srcCursor.NextFeature();
            //IFeatureCursor insertCursor = null;

            int count = 0;
            try
            {
                //if (srcFeat == null)
                //    return;
                //IFeatureBuffer pFeaBuffer = destFeatCls.CreateFeatureBuffer();
                //insertCursor = destFeatCls.Insert(true);
                //while (srcFeat != null)
                //{
                //    pFeaBuffer.Shape = srcFeat.ShapeCopy;
                //    FeatureHelper.CopyFeature(srcFeat, pFeaBuffer as ESRI.ArcGIS.Geodatabase.IFeature);
                //    insertCursor.InsertFeature(pFeaBuffer);
                //    insertCursor.Flush();

                //    count++;
                //    srcFeat = srcCursor.NextFeature();
                //}
                EsriDatabaseHelper.ConvertFeatureClass(RCIS.Global.GlobalEditObject.GlobalWorkspace, pGDB_workspace, destFeatLyrName, destFeatLyrName, null);
            }
            catch { }
            finally
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(srcCursor);
                //if (insertCursor != null)
                //{
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(insertCursor);
                //}

                if (srcfeaLyr != null)
                {
                    Marshal.FinalReleaseComObject(srcfeaLyr);
                }
                if (lyrFeaCls != null)
                {
                    Marshal.FinalReleaseComObject(lyrFeaCls);
                }
                //if (destFeatCls != null)
                //{
                //    Marshal.FinalReleaseComObject(destFeatCls);
                //}
            }

        }

        private void OpenDocument(string sFilePath)
        {
            if (m_MapDocument != null) m_MapDocument.Close();

            //Create a new map document
            m_MapDocument = new MapDocumentClass();


            //Open the map document selected
            m_MapDocument.Open(sFilePath, "");
            //Set the PageLayoutControl page layout to the map document page layout
            this.axPageLayoutControl.PageLayout = m_MapDocument.PageLayout;

        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开地图文件";
            openFileDialog1.Filter = "地图文件 (*.mxd)|*.mxd";
            openFileDialog1.ShowDialog();

            // Exit if no map document is selected
            string sFilePath = openFileDialog1.FileName;
            if (sFilePath == "")
            {
                return;
            }
            //Open document
            OpenDocument((sFilePath));
        }

        private void 直接打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.axPageLayoutControl.PrintPageLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查打印设备是否安装或打印驱动程序是否完备");
            }
        }

        public IMapFrame GetMapFrame
        {

            get
            {
                IGraphicsContainer container = axPageLayoutControl.GraphicsContainer;
                container.Reset();

                IElement ele = container.Next();
                while (ele != null)
                {
                    if (ele is IMapFrame)
                    {
                        return (IMapFrame)ele;
                    }
                    ele = container.Next();
                }

                return (IMapFrame)null;
            }
        }

        private void 打印预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewForm frm = new PreviewForm();
            frm.m_PageControl = this.axPageLayoutControl;
            frm.ShowDialog();
        }

        private void 文本标注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.TextElementEditTool(this.axPageLayoutControl);
            cmd.OnCreate(this.axPageLayoutControl.Object);
            this.axPageLayoutControl.CurrentTool = cmd as ITool;
        }

        private void 导出图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutputImageForm frm = new OutputImageForm();
            frm.m_PageControl = this.axPageLayoutControl;
            frm.ShowDialog();
        }

    }
}
