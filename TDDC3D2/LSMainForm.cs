using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;
using Microsoft.Win32;

using RCIS.Global;
using RCIS.Controls;
using RCIS.Utility;
using RCIS.GISCommon;
using RCIS.MapTool;
using RCIS.Database;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TDDC3D
{
    public partial class LSMainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        #region 内部变量



        private IToolbarMenu m_menuLayer = null;  //右键菜单
        private IToolbarMenu m_menuGroupLayer = null;
        private IToolbarMenu m_menuMap = null;

        private IToolbarMenu m_menuSketch = null;
        private IToolbarMenu m_menuMapBrowse = null;  //地图浏览时通用菜单

        ILayerEffectProperties pEffectLayer = null;     
        

        private LayoutControlPanel m_layoutControlPanel = null;
        private MetaDataControl m_MetaDataPanel = null;

        private string currEditLayerName = "";

        private sys.ControlsSynchronizer m_controlsSynchronizer = null;

        public string CurrEditLayerName
        {
            get { return currEditLayerName; }
            set { currEditLayerName = value; }
        }
        //保存工程文件
        IMapDocument m_MapDocument = null;
        
        private string m_FormSkin = "";//皮肤
        
        #endregion

        #region tocc map右键菜单
        
        private void LoadMainMenuItems()
        {
            int i = 0;
            this.m_menuLayer = new ToolbarMenuClass();
            int idx=this.m_menuLayer.AddItem(new RCIS.MapCmd.ZoomToLayerCmd(), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            idx = this.m_menuLayer.AddItem(new RCIS.MapCmd.RemoveLyrCmd(), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapCmd.LayerSimpleRender(this.mapTocc), -1, i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapCmd.UniqValueRenderCmd(this.mapTocc), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapTool.StyleRenderCommand(this.mapTocc), -1, i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapTool.DltbStyleRenderCommand(this.mapTocc), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapTool.ExportLayerCommand(), -1, i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new LayerTableCommand3(), -1, i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new LayerTableCommand2(), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapCmd.QueryConditionCmd(), -1, i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapCmd.LayerProperyCmd(), -1, i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.AddItem(new RCIS.MapTool.DeleteAllFeatureCommand(), -1, i, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuLayer.SetHook(this.mapControl.Object);
                    
            //草图菜单
            i = 0;
            this.m_menuSketch = new ToolbarMenuClass();            
            this.m_menuSketch.AddItem(new RCIS.MapTool.SketchDelVertexCommand(GlobalEditObject.CurrentEngineEditor),-1,i++,false,esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuSketch.AddItem("esriControls.ControlsEditingSketchContextMenu", -1,i++, true, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuSketch.AddItem(new RCIS.MapCmd.AbsolutelyXY2GeoCommand(GlobalEditObject.CurrentEngineEditor), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuSketch.AddItem(new RCIS.MapTool.InAShpGeoCommand(GlobalEditObject.CurrentEngineEditor), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);                       
            this.m_menuSketch.SetHook(this.mapControl.Object);

            //分组图层菜单
            i = 0;
            m_menuGroupLayer = new ToolbarMenuClass();
            this.m_menuGroupLayer.AddItem(new RCIS.MapCmd.ZoomToLayerCmd(), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuGroupLayer.AddItem(new RCIS.MapCmd.RemoveLyrCmd(), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuGroupLayer.SetHook(this.mapControl.Object);

            //地图浏览菜单
            this.m_menuMapBrowse = new ToolbarMenuClass();
            i = 0;
            this.m_menuMapBrowse.AddItem(new ControlsMapFullExtentCommandClass(), -1, i++, false, esriCommandStyles.esriCommandStyleIconAndText);
            this.m_menuMapBrowse.AddItem(new ControlsMapZoomToLastExtentBackCommandClass(), -1, i++, false, esriCommandStyles.esriCommandStyleIconAndText);
            this.m_menuMapBrowse.AddItem(new ControlsMapZoomToLastExtentForwardCommandClass(), -1, i++, false, esriCommandStyles.esriCommandStyleIconAndText);
            this.m_menuMapBrowse.AddItem(new ControlsMapZoomInFixedCommandClass(), -1, i++, true, esriCommandStyles.esriCommandStyleIconAndText);
            this.m_menuMapBrowse.AddItem(new ControlsMapZoomOutFixedCommandClass(), -1, i++, false, esriCommandStyles.esriCommandStyleIconAndText);
            this.m_menuMapBrowse.AddItem(new ControlsMapRefreshViewCommandClass(), -1, i++, true, esriCommandStyles.esriCommandStyleIconAndText);
            this.m_menuMapBrowse.SetHook(this.mapControl.Object);

            //地图菜单
            this.m_menuMap = new ToolbarMenuClass();
            i = 0;
            this.m_menuMap = new ToolbarMenuClass();
            this.m_menuMap.AddItem(new RCIS.MapCmd.MapPropertyCmd(), -1, i++, false, esriCommandStyles.esriCommandStyleTextOnly);
            this.m_menuMap.SetHook(this.mapControl.Object);

        }
        private void mapTocc_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {

                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;
                //判断所选菜单的类型
                ITOCControl2 m_tocControl = this.mapTocc.Object as ITOCControl2;
                m_tocControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

                //确定选定的菜单类型，Map或是图层菜单
                if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    m_tocControl.SelectItem(layer, null);
                    IMapControl3 m_mapControl = this.mapControl.Object as IMapControl3;
                    m_mapControl.CustomProperty = layer;
                    if (layer is IGroupLayer)
                    {
                        m_menuGroupLayer.PopupMenu(e.x, e.y, m_tocControl.hWnd);
                    }
                    else
                    {
                        m_menuLayer.PopupMenu(e.x, e.y, m_tocControl.hWnd);
                        
                    }
                    
                }
                if (item == esriTOCControlItem.esriTOCControlItemMap)
                {
                    IMapControl3 m_mapControl = this.mapControl.Object as IMapControl3;
                    m_menuMap.PopupMenu(e.x, e.y, m_tocControl.hWnd);
                }

            }
        }

        #endregion

        #region 窗体事件

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (this.xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    this.m_controlsSynchronizer.ActivateMap();
                }

                if (this.xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    this.m_controlsSynchronizer.ActivatePageLayout();
                }

                if (this.xtraTabControl1.SelectedTabPageIndex == 2)
                {
                    //元数据
                    this.m_MetaDataPanel.Workspace = GlobalEditObject.GlobalWorkspace;
                }
            }
            catch
            {
            }
        }

        private void InitSkinGallery()
        {
            try
            {
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(this.skinRibbonGalleryBarItem, true);
                INIHelper ini = new INIHelper(RCIS.Global.AppParameters.ConfPath + "\\Setup.ini");
                this.m_FormSkin = ini.IniReadValue("system", "SkinName");
                if (this.m_FormSkin != "")
                {
                    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(this.m_FormSkin);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("初始化皮肤失败，有问题请与管理员联系！");
            }
        }

        #region 闪屏
        private SplashForm mSplash;
        private SplashForm SplashForm
        {
            get
            {
                if (this.mSplash == null)
                {
                    this.mSplash = new SplashForm();
                    this.mSplash.Width = 600;
                    this.mSplash.Height = 400;
                    this.mSplash.TopMost = false;
                    this.mSplash.Show();
                }
                return this.mSplash;
            }
        }


        #endregion
        private void LSMainForm_Load(object sender, EventArgs e)
        {
            mapControl.AutoKeyboardScrolling = false; //键盘
            mapControl.KeyIntercept = 1;
            this.xtraTabControl1.SelectedTabPageIndex = 0;

            btnXZQShow.Down = true;
            btnTOCShow.Down = true;
        }
        private void LSMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           

            try
            {
                LayerHelper.removeAllLayers(this.mapControl.Object as IMapControl3);

                string currentSkin = DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName;
                if (!currentSkin.Equals(this.m_FormSkin))
                {
                    INIHelper ini = new INIHelper(RCIS.Global.AppParameters.ConfPath + "\\Setup.ini");
                    ini.IniWriteValue("system", "SkinName", currentSkin);
                }

                //终止编辑
                if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
                {
                    GlobalEditObject.StopEditingObj(GlobalEditObject.CurrentEngineEditor);                    
                }
                if (GlobalEditObject.CurrentEngineEditor.EditWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.CurrentEngineEditor.EditWorkspace);
                }
                if (GlobalEditObject.CurrentEngineEditor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.CurrentEngineEditor);
                }
                                
                if (GlobalEditObject.GlobalWorkspace != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.GlobalWorkspace);
                }

                #region 释放菜单资源
                if (pEffectLayer != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pEffectLayer);
                if (m_menuLayer != null)
                {
                    m_menuLayer.RemoveAll();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_menuLayer);
                }
                if (m_menuMap != null)
                {
                    m_menuMap.RemoveAll();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_menuMap);
                }
                if (m_menuGroupLayer != null)
                {
                    m_menuGroupLayer.RemoveAll();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(m_menuGroupLayer);
                }
                if (m_menuMapBrowse != null)
                {
                    m_menuMapBrowse.RemoveAll();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_menuMapBrowse);
                }
                if (m_menuSketch != null)
                {
                    m_menuSketch.RemoveAll();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_menuSketch);
                }
                #endregion 

                if (m_MapDocument != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_MapDocument);
                }
                

                ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();                
                GC.Collect();
                GC.WaitForPendingFinalizers();

                
            }
            catch { }
            
        }

       

        //初始化
        public LSMainForm()
        {
            

            this.Visible = false;
            this.SplashForm.Caption = "正在初始化窗体...";
            InitializeComponent();

            //this.SplashForm.Caption = "正在验证许可...";
            //string s = FileHelper.ReadLicense(Application.StartupPath + @"\LSLicense.Dat", "tdc3d@LS");
            //if (s != "OK")
            //{
            //    MessageBox.Show(s);
            //    System.Environment.Exit(0);
            //}

            this.SplashForm.Caption = "正在构建菜单和工具条...";
            InitSkinGallery();
          

            RCIS.Global.AppParameters.InitElseOption();

            mapTocc.SetBuddyControl(this.mapControl);
            this.axToolbarControl1.SetBuddyControl(this.mapControl);

            setComponentEnable(false);
            Application.DoEvents();
            //创建菜单和工具栏
            this.SplashForm.Caption = "正在初始化全局变量...";
            
            #region   //初始化 单例 engineEditor

            pEffectLayer = new CommandsEnvironmentClass();  //卷帘图层用的

            GlobalEditObject.CurrentEngineEditor = new EngineEditorClass();
            GlobalEditObject.CurrentEngineEditor.EnableUndoRedo(true);            

            ////添加Task，new 和modify  是 arcgis 自动就有的task
            GlobalEditObject.CurrentEngineEditor.AddTask(new MYEngineEditTasks.CutPolygonWithoutSelect()); //分割
            GlobalEditObject.CurrentEngineEditor.AddTask(new MYEngineEditTasks.PanEditPolygonTask()); //平移
            GlobalEditObject.CurrentEngineEditor.AddTask(new RCIS.MapTool.CutTbWithoutSelectTask());//分割
            GlobalEditObject.CurrentEngineEditor.AddTask(new edit.TbMultiBGTask());
            GlobalEditObject.CurrentEngineEditor.AddTask(new RCIS.MapTool.RemoveSmallTbTask());
            GlobalEditObject.CurrentEngineEditor.AddTask(new RCIS.MapTool.EditReshapeTask());

            SketchToolHelper.SetSketchToolSymbol(GlobalEditObject.CurrentEngineEditor);
            
            this.SplashForm.Caption = "加载地图右键菜单...";
            //初始化右键菜单
            LoadMainMenuItems();

            //初始化行政代码树
            LoadTreeFromQsdm();
            #endregion
                        

            this.SplashForm.Caption = "正在加载排版组件...";
            Application.DoEvents();
            this.m_layoutControlPanel = new LayoutControlPanel();
            this.m_layoutControlPanel.Dock = DockStyle.Fill;
            this.m_layoutControlPanel.Visible = true;
            this.pageLayout.Controls.Add(this.m_layoutControlPanel);

            this.SplashForm.Caption = "正在加载元数据组件...";
            this.m_MetaDataPanel = new MetaDataControl();
            this.m_MetaDataPanel.Dock = DockStyle.Fill;
            this.m_MetaDataPanel.Visible = true;
            
            this.m_MetaDataPanel.MapControl = this.mapControl;
            this.pageMetadata.Controls.Add(this.m_MetaDataPanel);

            this.m_controlsSynchronizer = new sys.ControlsSynchronizer((IMapControl3)mapControl.Object, (IPageLayoutControl2)m_layoutControlPanel.LayoutControl.Object);
            this.m_controlsSynchronizer.BindControls(true);
            this.m_controlsSynchronizer.AddFrameworkControl(axToolbarControl1.Object);
            this.m_controlsSynchronizer.AddFrameworkControl(mapTocc.Object);
            this.m_controlsSynchronizer.AddFrameworkControl(m_layoutControlPanel.LayoutBarControl);

            this.SplashForm.Hide();
            this.Visible = true;
        }
        private void LSMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("确信要退出该软件么?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            try
            {
                if (this.m_MapDocument != null)
                {
                    this.m_MapDocument.Save(true, false);
                }
                GlobalEditObject.StopEditingObj(GlobalEditObject.CurrentEngineEditor);
            }
            catch (Exception ex) { }
        }

        private void LoadTreeFromQsdm()
        {
            this.tvXzq.Nodes.Clear();
            ITable pTable=null;
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                
                return;
            }
            try{
                pTable = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenTable("QSDWDMB");
                List<string> lstXian = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 6);
                List<string> lstXiang = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 9);
                List<string> lstCun = sys.YWCommonHelper.getXzqFromQsdwdm(pTable, 12);
                foreach (string aXian in lstXian)
                {
                    TreeNode axianNode= tvXzq.Nodes.Add(aXian);
                    string aXianDm = OtherHelper.GetLeftName(aXian);
                    foreach (string aXiang in lstXiang)
                    {
                        string xiangDm = OtherHelper.GetLeftName(aXiang);
                        if (xiangDm.StartsWith(aXianDm))
                        {
                            TreeNode aXiangNode = axianNode.Nodes.Add(aXiang);
                            foreach (string aCun in lstCun)
                            {
                                string aCunDm = OtherHelper.GetLeftName(aCun);
                                if (aCunDm.StartsWith(xiangDm))
                                {
                                    aXiangNode.Nodes.Add(aCun);
                                }
                            }
                        }
                        
                    }
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void barButtonItem54_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            datado.FrmExcel2SHP frm = new datado.FrmExcel2SHP();
            frm.ShowDialog();
        }

        private void 从权属代码表加载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTreeFromQsdm();
        }

        private IFeature GetFeature(string fldName, string sValue, IFeatureClass pFC,int isInt)
        {
            IQueryFilter qf = new QueryFilterClass();
            if (isInt == 1)
            {
                qf.WhereClause = fldName + " =" + sValue + "";
            }
            else
            {
                qf.WhereClause = fldName + " ='" + sValue + "'";
            }
            IFeatureCursor pCursor = pFC.Search(qf, false);
            IFeature pFea = pCursor.NextFeature();

            if (pFea != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                return pFea;
            }
            OtherHelper.ReleaseComObject(pCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(qf);
            return null;
        }


        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //定位图形
            if (this.gridViewError.SelectedRowsCount == 0)
                return;
            string layerName = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "涉及图层").ToString();
            string oid = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "要素ID").ToString();
            string sBsm = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "要素BSM").ToString();
            
            //定位
            try
            {
                IFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, layerName);
                IFeatureClass pClass = pLyr.FeatureClass;

                IFeature pFeature = null;
                if (oid.Trim() != "")
                {
                    pFeature = this.GetFeature("OBJECTID", oid, pClass,1);
                }

                if (pFeature == null)
                {
                    if (sBsm.Length > 18)
                    {
                        sBsm = sBsm.Substring(0, 18);
                    }
                    pFeature = this.GetFeature("BSM", sBsm, pClass,0);
                }
                
                
                if (pFeature != null)
                {
                    IGeometry pGeo = pFeature.ShapeCopy;
                    IEnvelope env = pGeo.Envelope;
                    env.Expand(1.5, 1.5, true);
                    this.mapControl.ActiveView.Extent = env;

                    this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                    this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    if (pGeo != null)
                    {
                        this.mapControl.FlashShape(pGeo, 3, 300, null);
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 删除该条ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.gridViewError.SelectedRowsCount == 0)
                return;
            this.gridViewError.DeleteSelectedRows();

        }

        //定位错误
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (this.gridViewError.SelectedRowsCount == 0)
                return;
            try
            {
                string layerName = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "涉及图层").ToString();
                string oid = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "要素ID").ToString();
                string sBsm = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "要素BSM").ToString();
              //  int ioid = Convert.ToInt32(oid);
                //定位
                IFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, layerName);
                IFeatureSelection pSelection = pLyr as IFeatureSelection;
                IFeatureClass pClass = pLyr.FeatureClass;

                IFeature pFeature = null;
                if (oid.Trim() != "")
                {
                    pFeature = this.GetFeature("OBJECTID", oid, pClass, 1);
                }

                if (pFeature == null)
                {
                    pFeature = this.GetFeature("BSM", sBsm, pClass, 0);
                }
                

                if (pFeature != null)
                {
                    pSelection.Add(pFeature);

                }
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
            }
            catch { }
        }

       
        private void gridControlError_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewError.SelectedRowsCount == 0)
                return;
            string layerName = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "涉及图层").ToString();
            string oid = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "要素ID").ToString();
            string sBsm = this.gridViewError.GetRowCellValue(this.gridViewError.FocusedRowHandle, "要素BSM").ToString();

            //定位
            try
            {
                IFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, layerName);
                IFeatureClass pClass = pLyr.FeatureClass;

                IFeature pFeature = null;
                if (oid.Trim() != "")
                {
                    pFeature = this.GetFeature("OBJECTID", oid, pClass, 1);
                }

                if (pFeature == null)
                {
                    pFeature = this.GetFeature("BSM", sBsm, pClass, 0);
                }


                if (pFeature != null)
                {
                    IGeometry pGeo = pFeature.ShapeCopy;
                    IEnvelope env = pGeo.Envelope;
                    env.Expand(1.5, 1.5, true);
                    this.mapControl.ActiveView.Extent = env;

                    this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                    this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    if (pGeo != null)
                    {
                        this.mapControl.FlashShape(pGeo, 3, 300, null);
                    }

                }
            }
            catch (Exception ex)
            {
            }

        }

        private void 导入国家质检软件结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel文件|*.xlsx";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                
                DataTable srcDb= RCIS.Database.Excel2DatasetHelper.GetExcelToDataTableBySheet(dlg.FileName, "错误图层$");
                DataTable toDb=(DataTable ) this.gridControlError.DataSource;
                if (toDb == null)
                {
                    toDb = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select top 1 * from CHK_ERRORLiST ","error");
                }
                toDb.Rows.Clear();

                Dictionary<string,string> dicClassCNName=new Dictionary<string,string>();
                DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM where type in ('POINT','LINE','POLYGON')  ", "ysdm");
                foreach (DataRow dr in dt.Rows)
                {
                    dicClassCNName.Add(dr["ALIASNAME"].ToString(), dr["CLASSNAME"].ToString());
                }


                for  (int ii=1;ii<srcDb.Rows.Count;ii++)
                {

                    DataRow aRow = srcDb.Rows[ii];

                    DataRow newRow = toDb.NewRow();
                    //涉及图层，错误描述，要素BSM
                    newRow["涉及图层"]=dicClassCNName.ContainsKey(aRow[1].ToString().Trim())?dicClassCNName[aRow[1].ToString().Trim()]:aRow[1].ToString();
                    newRow["要素BSM"]=aRow[2].ToString().Trim();
                    newRow["错误描述"]=aRow[4].ToString();
                    toDb.Rows.Add(newRow);
                }
                this.gridControlError.DataSource=toDb;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void 加载上次检查结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select * from CHK_ERRORLiST", "check");
            this.gridControlError.DataSource = dt;
            this.gridControlError.RefreshDataSource();
        }


        private void AddAGeoStyleItem(IFeatureLayer pFeaLyr)
        {
            DevExpress.XtraNavBar.NavBarItemLink aLink = null;
            if (pFeaLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                aLink = nvgpoint.AddItem();
            }
            else if (pFeaLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                aLink = nvgline.AddItem();
            }
            else if (pFeaLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                aLink = nvgpolygon.AddItem();
            }

            DevExpress.XtraNavBar.NavBarItem aItem = aLink.Item;

            string layerName =  (pFeaLyr.FeatureClass as IDataset).Name;

            aItem.Caption = layerName + "|" + pFeaLyr.Name;
            aItem.Hint = pFeaLyr.Name;
            Font aPressFont = aItem.AppearancePressed.Font;
            aPressFont = new Font(aPressFont, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            aItem.AppearancePressed.Font = aPressFont;
            if (pFeaLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                aItem.LargeImageIndex = 0;
            }
            else if (pFeaLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
            {
                aItem.LargeImageIndex = 1;
            }
            else if (pFeaLyr.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                aItem.LargeImageIndex = 2;
            }
            aItem.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(EditObjectLinkClicked);
        }

        private void LoadAllLayerStylePanel(IWorkspace currWs,bool isVisible)
        {

            this.navBarLayerStyle.Items.Clear();
            for (int i = 0; i < this.mapControl.LayerCount; i++)
            {
                ILayer pLyr = this.mapControl.get_Layer(i);                
                if (pLyr is IFeatureLayer)
                {
                    if (isVisible)
                    {
                        if (!pLyr.Visible)
                        {
                            continue;
                        }
                    }
                    IFeatureLayer pFeaLyr = pLyr as IFeatureLayer;
                    IFeatureClass pFC = pFeaLyr.FeatureClass;
                    IDataset pDS = pFC as IDataset;
                    
                    if (pDS.Workspace != currWs)
                        continue;
                    try
                    {
                        AddAGeoStyleItem(pFeaLyr);
                    }
                    catch { }
                }
                else if (pLyr is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = pLyr as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        IFeatureLayer pFeatureLayer = childLyr as IFeatureLayer;
                        if (pFeatureLayer == null)
                            continue;

                        if (isVisible)
                        {
                            if (!childLyr.Visible)
                            {
                                continue;
                            }
                        }


                        IFeatureClass pFC = pFeatureLayer.FeatureClass;
                        IDataset pDS = pFC as IDataset;

                        if (pDS.Workspace != currWs)
                            continue;
                        try
                        {
                            AddAGeoStyleItem(pFeatureLayer);
                        }
                        catch { }

                    }
                }

                


            }
        }

        private void EditObjectLinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            
            this.comEditLayer.EditValue = OtherHelper.GetLeftName(e.Link.Caption);
            this.currEditLayerName =OtherHelper.GetLeftName( e.Link.Caption);
            GlobalEditObject.SetTargetEditLayer(this.mapControl.Object as IMapControl2,
                GlobalEditObject.CurrentEngineEditor,
                currEditLayerName);


        }


        private void rgVisibleLayersEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //是否显示可见图层
            if (this.rgVisibleLayersEdit.SelectedIndex == 0)
            {
                LoadAllLayerStylePanel(GlobalEditObject.GlobalWorkspace, false);
            }
            else
            {
                LoadAllLayerStylePanel(GlobalEditObject.GlobalWorkspace, true);
            }
        }
        private void barButtonItem52_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导出成果包
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            output.OutStandardForm frm = new output.OutStandardForm();
            frm.currWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }
        private void barButtonItem51_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //城镇村扩大分析
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            analysis.NewCZCForm frm = new analysis.NewCZCForm();
            frm.currWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        #region 日志窗口
        private void br_view_errorpanel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.br_view_errorpanel.Down)
            {
                this.dockPanelError.Show();
            }
            else
            {
                this.dockPanelError.Hide();
            }
        }
        private void dockPanelError_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {

        }
        //日志窗口 可见
        private void dockPanelError_VisibilityChanged(object sender, DevExpress.XtraBars.Docking.VisibilityChangedEventArgs e)
        {
            if (this.dockPanelError.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            {
                if (!this.br_view_errorpanel.Down)
                {
                    this.br_view_errorpanel.Down = true;
                }
            }
            else if (this.dockPanelError.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Hidden)
            {
                if (this.br_view_errorpanel.Down)
                {
                    this.br_view_errorpanel.Down = false;
                }
            }
        }
        #endregion 

        #endregion
        
        #region 工程和文件

      
       
       

        //打开调查举证exe
        private void barButtonItem38_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("新版请使用安装版核查举证桌面端！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //string destFile = Application.StartupPath + "\\monitor\\WalkTMonitor.exe";
            //if (!System.IO.File.Exists(destFile))
            //{
            //    MessageBox.Show("不存在该模块！");
            //    return;
            //}
            //System.Diagnostics.Process.Start(destFile);
        }


        //退出
        private void br_exit_win_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();

            //try
            //{
            //    IFeatureClass pGXGCClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGXGC");
            //    IFeatureClass pGXClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            //    IQueryFilter pQf = new QueryFilterClass();
            //    pQf.WhereClause = "BGXW='1'";
            //    IFeatureCursor pFeaCursor = pGXGCClass.Update(pQf, true);
            //    IFeature pFeature;
            //    while ((pFeature = pFeaCursor.NextFeature()) != null)
            //    {
            //        string bsm = pFeature.get_Value(pFeature.Fields.FindField("BSM")).ToString();
            //        if (bsm == "420982211000158643")
            //            bsm = bsm;
            //        bool isDel = true;
            //        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
            //        {
            //            string filedName = pFeature.Fields.Field[i].Name.ToString().Trim().ToUpper();
            //            if (filedName.Contains("BGQ") && !filedName.Contains("BSM") && !filedName.Contains("TBBH"))
            //            {
            //                if (pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() != pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim())
            //                {
            //                    if (filedName == "BGQGDDB" && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName)).ToString().Trim() == "0") && (string.IsNullOrWhiteSpace(pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim()) || pFeature.get_Value(pFeature.Fields.FindField(filedName.Replace("BGQ", "BGH"))).ToString().Trim() == "0"))
            //                        continue;
            //                    else
            //                    {
            //                        isDel = false;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        if (isDel)
            //        {
            //            string bghbsm = pFeature.get_Value(pFeature.Fields.FindField("BGHTBBSM")).ToString().Trim();
            //            GlobalEditObject.GlobalWorkspace.ExecuteSQL("delete from dltbgx where bsm='" + bghbsm + "'");
            //            pFeature.set_Value(pFeature.Fields.FindField("BSM"), "DEL");
            //            pFeaCursor.UpdateFeature(pFeature);
            //        }
            //        RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
            //    }
            //    pFeaCursor.Flush();
            //    RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            //    GlobalEditObject.GlobalWorkspace.ExecuteSQL("delete from dltbgxgc where bsm='DEL'");

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
       
        //清空当前地图
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
                {
                    MessageBox.Show("请先终止编辑，确保操作不丢失！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                

                List<IWorkspace> lstAllWs = getAllWorkspace();
                LayerHelper.removeAllLayers(this.mapControl.Object as IMapControl3);

                foreach (IWorkspace ws in lstAllWs)
                {
                    //关闭资源锁定 
                    IWorkspaceFactoryLockControl  ipWsFactoryLock = (IWorkspaceFactoryLockControl)ws;
                    if (ipWsFactoryLock.SchemaLockingEnabled)
                    {
                        ipWsFactoryLock.DisableSchemaLocking();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ipWsFactoryLock);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);                    
                }
                //if (GlobalEditObject.GlobalWorkspace != null)
                //{
                //    IWorkspaceFactoryLockControl ipWsFactoryLock = (IWorkspaceFactoryLockControl)GlobalEditObject.GlobalWorkspace;
                //    if (ipWsFactoryLock.SchemaLockingEnabled)
                //    {
                //        ipWsFactoryLock.DisableSchemaLocking();
                //    }
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(ipWsFactoryLock);
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.GlobalWorkspace);
                //}
                GC.Collect();
                GC.WaitForPendingFinalizers(); 
            }
            catch(Exception ex) {
            }
        }

      
        private void bi_project_saveas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_MapDocument == null  ||  m_MapDocument.DocumentFilename==null)
            {
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Title = "另存为";
                saveDlg.Filter = "地图文件 (*.mxd)|*.mxd";
                if (saveDlg.ShowDialog() == DialogResult.Cancel)
                    return;
                try
                {
                    IMxdContents pMxd = this.mapControl.Map as IMxdContents;
                    m_MapDocument = new MapDocumentClass();
                    m_MapDocument.New(saveDlg.FileName);
                    m_MapDocument.ReplaceContents(pMxd);
                    m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);
                    MessageBox.Show("保存完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存mxd文档时出错!" + ex.ToString());
                    return;
                }
            }
            else
            {
                try
                {                    
                    IMxdContents pMxdc = this.mapControl.Map as IMxdContents;
                    IActiveView pActiveView = this.mapControl.Map as IActiveView;
                    m_MapDocument.ReplaceContents(pMxdc);
                    m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);

                    MessageBox.Show("保存完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

           
        }

        //另存为地图mxd
        private void barButtonItem35_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Title = "另存为";
            saveDlg.Filter = "地图文件 (*.mxd)|*.mxd";
            if (saveDlg.ShowDialog() == DialogResult.Cancel)
                return;
            
            try
            {
                IMxdContents pMxd = this.mapControl.Map as IMxdContents;
                m_MapDocument = new MapDocumentClass();
                m_MapDocument.New(saveDlg.FileName);
                m_MapDocument.ReplaceContents(pMxd);
                m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);
                MessageBox.Show("保存完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存mxd文档时出错!" + ex.ToString());
                return;
            }
        }


        private void br_project_open_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog2;
            openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Title = "Open Map Document";
            openFileDialog2.Filter = "Map Documents (*.mxd)|*.mxd";
            if (openFileDialog2.ShowDialog() == DialogResult.Cancel) return;

            string sFilePath = openFileDialog2.FileName;

            if (this.m_MapDocument == null)
            {
                this.m_MapDocument = new MapDocumentClass();
            }
            this.mapControl.MousePointer = esriControlsMousePointer.esriPointerHourglass;
            try
            {

                this.m_MapDocument.Open(sFilePath, "");
                for (int i = 0; i <= m_MapDocument.MapCount - 1; i++)
                {
                    this.mapControl.Map = m_MapDocument.get_Map(i);
                }

            }
            catch { }
            finally
            {
                this.mapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }

            try
            {
                //当前数据库第一个图层的 工作空间 ，赋给全局变量
                for (int i = 0; i < this.mapControl.LayerCount; i++)
                {
                    ILayer pLyr = this.mapControl.get_Layer(i);
                    if (pLyr is IFeatureLayer)
                    {
                        IFeatureLayer pFeaLyr = pLyr as IFeatureLayer;
                        IFeatureClass pClass = pFeaLyr.FeatureClass;
                        IFeatureDataset pDs = pClass.FeatureDataset;
                        RCIS.Global.GlobalEditObject.GlobalWorkspace = pDs.Workspace;
                        this.spComment.Caption ="工作路径: "+ pDs.Workspace.PathName;
                        break;
                    }
                    else if (pLyr is IGroupLayer)
                    {
                        ICompositeLayer pComLyr = pLyr as ICompositeLayer;
                        for (int kk = 0; kk < pComLyr.Count; kk++)
                        {
                            ILayer childLyr = pComLyr.get_Layer(kk);
                            if (childLyr is IFeatureLayer)
                            {
                                IFeatureLayer pFeaLyr = childLyr as IFeatureLayer;
                                IFeatureClass pClass = pFeaLyr.FeatureClass;
                                IFeatureDataset pDs = pClass.FeatureDataset;
                                RCIS.Global.GlobalEditObject.GlobalWorkspace = pDs.Workspace;
                                this.spComment.Caption = "工作路径: " + pDs.Workspace.PathName;
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public IFeatureLayer[] GetFeatureLayers(IMap pMap)
        {
            IFeatureLayer pFeatLayer;
            ICompositeLayer pCompLayer;
            List<IFeatureLayer> pList = new List<IFeatureLayer>();
            //遍历地图
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    //获得图层要素
                    pFeatLayer = pMap.get_Layer(i) as IFeatureLayer;
                    pList.Add(pFeatLayer);
                }
                else if (pMap.get_Layer(i) is IGroupLayer)
                {
                    //遍历图层组
                    pCompLayer = pMap.get_Layer(i) as ICompositeLayer;
                    String group = (pCompLayer as ILayer).Name;
                    for (int j = 0; j < pCompLayer.Count; j++)
                    {
                        if (pCompLayer.get_Layer(j) is IFeatureLayer)
                        {
                            pFeatLayer = pCompLayer.get_Layer(j) as IFeatureLayer;
                            pList.Add(pFeatLayer);
                        }
                    }
                }

            }
            return pList.ToArray();
        }


        private void AddOpenFileLayers()
        {
            try
            {
                List<string> workSpaceLayers = new List<string>();
                try
                {
                    //加载前获取当前项目内的图层
                    IFeatureLayer[] featureLayer = GetFeatureLayers(this.mapControl.Map);
                    foreach (IFeatureLayer aClassName in featureLayer)
                    {
                        string pathName = (aClassName.FeatureClass as IDataset).Workspace.PathName;
                        if (!GlobalEditObject.GlobalWorkspace.PathName.Equals(pathName))
                        {
                            continue;
                        }
                        workSpaceLayers.Add((aClassName.FeatureClass as IDataset).Name);

                    }
                }
                catch
                {
                    throw new Exception("加载当前项目内图层出错");
                }
                try
                {
                    this.spComment.Caption = "工作路径: " + GlobalEditObject.GlobalWorkspace.PathName;

                    sys.LayerSelectForm frm = new sys.LayerSelectForm();
                    frm.sendWs = GlobalEditObject.GlobalWorkspace;
                    frm._loadLayerStr = workSpaceLayers;
                    if (frm.ShowDialog() == DialogResult.Cancel)
                        return;
                    List<string> lstClass = frm.retFeaClassName;
                    if (lstClass.Count == 0) return;
                    #region  //加载图层
                    //this.mapControl.ClearLayers();

                    #region 获取字典表中的分组  //用分组图层试试
                    Dictionary<string, string> dicGroupYsdm = new Dictionary<string, string>();
                    List<string> allGroup = new List<string>(); //一共这些分组
                    string where = "'" + lstClass[0] + "'";
                    for (int i = 1; i < lstClass.Count; i++)
                    {
                        where += ",'" + lstClass[i] + "'";

                    }
                    DataTable dtysdm = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select classname,group from sys_ysdm where  type in ('POINT','LINE','POLYGON') AND   classname in ("
                    + where + ")  order by Ysdm desc ", "ysdm");
                    foreach (DataRow arow in dtysdm.Rows)
                    {
                        if (!dicGroupYsdm.ContainsKey(arow["classname"].ToString().Trim()))
                        {
                            dicGroupYsdm.Add(arow["classname"].ToString().Trim(), arow["group"].ToString().Trim());
                        }
                        if (!allGroup.Contains(arow["group"].ToString().Trim()))
                        {
                            allGroup.Add(arow["group"].ToString().Trim());
                        }
                    }
                    #endregion
                    //先添加图层
                    IFeatureWorkspace pFeaWs = GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
                    IFeatureLayer xzqLayer = null;
                    //移入分组
                    foreach (string aClassName in lstClass)
                    {
                        IFeatureClass pFeatureClass = pFeaWs.OpenFeatureClass(aClassName);
                        IFeatureLayer pFeaLyr = new FeatureLayerClass();
                        pFeaLyr.FeatureClass = pFeatureClass;
                        pFeaLyr.Name = pFeatureClass.AliasName;
                        if (AppParameters.LAYER_XZQ != aClassName)
                        {
                            pFeaLyr.Visible = false;
                        }
                        else
                        {
                            pFeaLyr.Visible = true;
                            xzqLayer = pFeaLyr;
                        }
                        if (dicGroupYsdm.ContainsKey(aClassName))
                        {
                            string groupName = dicGroupYsdm[aClassName];
                            IGroupLayer pGroupLayer = LayerHelper.QueryGroupLyrByName(this.mapControl.Map, groupName);
                            if (pGroupLayer == null)
                            {
                                pGroupLayer = new GroupLayerClass();
                                pGroupLayer.Name = groupName;
                                pGroupLayer.Visible = true;
                                pGroupLayer.SpatialReference = (pFeatureClass as IGeoDataset).SpatialReference;
                                this.mapControl.AddLayer(pGroupLayer as ILayer);
                                pGroupLayer.Expanded = false;
                            }

                            pGroupLayer.Add(pFeaLyr);

                            if (this.mapControl.SpatialReference == null)
                            {
                                this.mapControl.SpatialReference = (pFeatureClass as IGeoDataset).SpatialReference;
                            }

                        }
                        else
                        {
                            this.mapControl.AddLayer(pFeaLyr);
                        }
                    }
                    this.mapTocc.Update();

                    if (xzqLayer != null)
                    {
                        this.mapControl.Extent = xzqLayer.AreaOfInterest;
                        this.mapControl.ActiveView.Extent = xzqLayer.AreaOfInterest;
                    }
                    #endregion
                }
                catch
                {
                    throw new Exception("添加图层到图层列表中出错");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void AddOpenFileLayers2()
        //{
        //    this.spComment.Caption = "工作路径: " + GlobalEditObject.GlobalWorkspace.PathName;

      

        //    sys.LayerSelectForm frm = new sys.LayerSelectForm();
        //    frm.sendWs = GlobalEditObject.GlobalWorkspace;
        //    if (frm.ShowDialog() == DialogResult.Cancel)
        //        return;
        //    List<string> lstClass = frm.retFeaClassName;
        //    if (lstClass.Count == 0) return;
        //    #region  //加载图层
        //    this.mapControl.ClearLayers();
            
        //    ILayer aLyr = null;
        //    IFeatureWorkspace pFeaWs = GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
        //    foreach (string aClassName in lstClass)
        //    {
        //        //位于哪一个分组              
        //        IFeatureClass currFC = pFeaWs.OpenFeatureClass(aClassName);
        //        IFeatureLayer aFeatLyr = new FeatureLayerClass();
        //        aFeatLyr.FeatureClass = currFC;
        //        string className = (currFC as IDataset).Name.ToUpper();
        //        aFeatLyr.Name = currFC.AliasName;
        //        //if (currFC.AliasName.Trim() == "" || currFC.AliasName.Trim().ToUpper() == className)
        //        //{
        //        //    if (dicLayers.ContainsKey(className))
        //        //    {
        //        //        aFeatLyr.Name = dicLayers[className];
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    aFeatLyr.Name = currFC.AliasName;
        //        //}
        //        aLyr = aFeatLyr as ILayer;
                
        //        if (className != AppParameters.LAYER_XZQ)
        //        {
        //            aLyr.Visible = false;
        //        }
        //        this.mapControl.AddLayer(aLyr);
        //    }
        //    this.mapTocc.Update();

        //    #endregion
            

        //    #region 图层按点线面重排序,guojie ++ 2010-6-22

        //    List<ILayer> arPointlyrFrom = new List<ILayer>();
        //    List<ILayer> arPolygonLyrFrom = new List<ILayer>();


        //    for (int i = 0; i < this.mapControl.LayerCount; i++)
        //    {
        //        ILayer currLyr = mapControl.get_Layer(i);
        //        if (currLyr is IFeatureLayer2)
        //        {
        //            IFeatureLayer2 featLyr = currLyr as IFeatureLayer2;
        //            if (featLyr.ShapeType == esriGeometryType.esriGeometryPoint)
        //            {
        //                arPointlyrFrom.Add(currLyr);
        //            }
        //            else if (featLyr.ShapeType == esriGeometryType.esriGeometryPolygon)
        //            {
        //                arPolygonLyrFrom.Add(currLyr);
        //            }
        //        }

        //    }

        //    //点层往上挪
        //    foreach (ILayer thisLyr in arPointlyrFrom)
        //    {
        //        this.mapControl.Map.MoveLayer(thisLyr, 0);
        //    }
        //    //面层往下挪
        //    foreach (ILayer thisLyr in arPolygonLyrFrom)
        //    {
        //        this.mapControl.Map.MoveLayer(thisLyr, this.mapControl.Map.LayerCount);
        //    }
        //    #endregion
        //    if (aLyr != null)
        //    {
        //        this.mapControl.Extent = aLyr.AreaOfInterest;
        //    }
        //}

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            
            try
            {
                RegistryKey keyRead = Registry.CurrentUser;
                keyRead = keyRead.OpenSubKey("Software\\landstar");
                if (keyRead != null)
                {
                    object oPath = keyRead.GetValue("pgdbpath");
                    if (oPath != null)
                    {
                        dlg.FileName = oPath.ToString();

                        //dlg.RootFolder = oPath.ToString();
                    }
                }
            }
            catch { }

            dlg.Filter = "PGDB数据库|*.mdb";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = dlg.FileName;
            //if (GlobalEditObject.GlobalWorkspace != null)
            //{
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.GlobalWorkspace);
            //}
            GlobalEditObject.GlobalWorkspace = WorkspaceHelper2.GetAccessWorkspace(fileName);

            //记录路径
            try
            {
                RegistryKey keyWrite = Registry.CurrentUser;
                keyWrite = keyWrite.CreateSubKey("Software\\landstar");
                keyWrite.SetValue("pgdbpath", dlg.FileName);
            }
            catch { }

            if (GlobalEditObject.GlobalWorkspace != null)
            {
                //AddOpenFileLayers2();    
                AddOpenFileLayers();   
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshXzqTree();
        }

        private void 定位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tvXzq.Nodes.Count == 0)
                return;
            if (this.tvXzq.SelectedNode == null)
                return;

            string xzqdm = OtherHelper.GetLeftName(this.tvXzq.SelectedNode.Text);
            IGeoFeatureLayer xzqLyr = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, "CJDCQ");
            if (xzqLyr == null)
                return;
            IFeatureClass xzqClass = xzqLyr.FeatureClass;

            //判断行政区级别
            if (xzqdm.Length > 12)
            {
                return;
            }
            IGeometry geom = null;
            IEnvelope env = null;
            ITopologicalOperator ptop = null;
            IQueryFilter pQf = new QueryFilterClass();
            if (xzqdm.Length == 6)
            {
                //全县
                env = (xzqLyr as ILayer).AreaOfInterest;
            }
            else if (xzqdm.Length == 9)
            {
                pQf.WhereClause = "ZLDWDM like '" + xzqdm + "%' ";
                IFeatureCursor pCursor = xzqClass.Search(pQf, true) as IFeatureCursor;
                IFeature aXzq = pCursor.NextFeature();
                while  (aXzq != null)
                {
                    if (geom == null)
                    {
                        geom = aXzq.ShapeCopy;
                    }
                    else
                    {
                        geom = ptop.Union(aXzq.ShapeCopy);                      
                    }
                    ptop = geom as ITopologicalOperator;
                    
                    aXzq = pCursor.NextFeature();
                }
                ptop.Simplify();
                env = geom.Envelope;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            else if (xzqdm.Length == 12)
            {
                pQf.WhereClause = "ZLDWDM Like '" + xzqdm + "%' ";
                IFeatureCursor pCursor = xzqClass.Search(pQf, true) as IFeatureCursor;
                IFeature aXzq = pCursor.NextFeature();
                if (aXzq != null)
                {
                    geom = aXzq.ShapeCopy;
                    if (geom != null && !geom.IsEmpty)
                    {
                        env = geom.Envelope;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }            
            
            
            if (geom != null)
            {                
                env.Expand(1.5, 1.5, true);
                this.mapControl.ActiveView.Extent = env;
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
                this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                this.mapControl.FlashShape(geom, 3, 300, null);                
            }            

        }

        //从行政区图层加载行政区树
        private void RefreshXzqTree()
        {
            IFeatureClass xzqClass = null;
            IFeatureLayer xzqLyr = null;
            //for (int i = 0; i < this.mapControl.LayerCount; i++)
            //{
            //    ILayer currLyr = mapControl.get_Layer(i);
            //    if (currLyr is IFeatureLayer2)
            //    {
            //        IFeatureClass aFc = (currLyr as IFeatureLayer).FeatureClass;
            //        if ((aFc as IDataset).Name.ToUpper() == "XZQ")
            //        {
            //            xzqLyr = currLyr as IFeatureLayer;
            //            xzqClass = aFc;
            //            break;
            //        }
            //    }
            //}
            xzqLyr = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(mapControl.Map, "XZQ");
            if (xzqLyr == null) return;
            xzqClass = xzqLyr.FeatureClass;
            this.tvXzq.Nodes.Clear();

            //获取所有行政区要素
            IIdentify dltbIndentify = xzqLyr as IIdentify;
            IDataset xzqDS = xzqClass as IDataset;
            IGeoDataset xzqGeoDs = xzqDS as IGeoDataset;
            IArray arrDltbIDs = dltbIndentify.Identify(xzqGeoDs.Extent);
            for (int i = 0; i < arrDltbIDs.Count; i++)
            {
                IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                IFeature pfea = pRow.Row as IFeature;

                string xzqmc = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(pfea, "XZQDM");

                this.tvXzq.Nodes.Add(xzqdm + "|" + xzqmc);
            }
        }

        private void br_file_openfgdb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //打开filegdb文件
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            try
            {
                RegistryKey keyRead = Registry.CurrentUser;
                keyRead = keyRead.OpenSubKey("Software\\landstar");
                if (keyRead != null) 
                {
                    object oPath = keyRead.GetValue("filegdbpath");
                    if (oPath != null)
                    {
                        dlg.SelectedPath = oPath.ToString();
                        //dlg.RootFolder = oPath.ToString();
                    }                    
                }
            }
            catch { }

            
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            //if (GlobalEditObject.GlobalWorkspace != null)
            //{
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.GlobalWorkspace);
            //    GlobalEditObject.GlobalWorkspace = null;
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}
            string beforeFile = "";
            if (GlobalEditObject.GlobalWorkspace != null) beforeFile = GlobalEditObject.GlobalWorkspace.PathName;
            GlobalEditObject.GlobalWorkspace = WorkspaceHelper2.GetFileGdbWorkspace(dlg.SelectedPath);

            //记录路径
            try
            {
                RegistryKey keyWrite = Registry.CurrentUser;
                keyWrite = keyWrite.CreateSubKey("Software\\landstar");
                keyWrite.SetValue("filegdbpath", dlg.SelectedPath);
            }
            catch { }
            
            if (GlobalEditObject.GlobalWorkspace != null)
            {
                if (GlobalEditObject.GlobalWorkspace.PathName != beforeFile) mapControl.Map.ClearLayers();
                IFeatureDataset pFeatureDataset = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureDataset(AppParameters.DATASET_DEFAULT_NAME);
                RCIS.GISCommon.GeometryHelper._tolerance = ((pFeatureDataset as IGeoDataset).SpatialReference as ISpatialReferenceTolerance).XYTolerance;
                AddOpenFileLayers();
                //AddOpenFileLayers2();
            }
        }

        #endregion

        #region 地图窗口事件

        private void mapControl_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (this.Cursor == System.Windows.Forms.Cursors.SizeAll)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void mapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            IPoint aPt = new PointClass();
            aPt.PutCoords(e.mapX, e.mapY);

            #region 显示XY坐标
            double px = Math.Round(e.mapX, 3);
            double py = Math.Round(e.mapY, 3);
            string pStr = String.Format("{0},{1}", px, py);
            this.spMouseLoc.Caption = pStr;
            #endregion

            #region 显示经纬度
            ISpatialReference aSR = this.mapControl.ActiveView.FocusMap.SpatialReference;
            if (aSR == null)
                return;

            if (aSR.Name.ToLower() == "unknown")//没有投影不显示经纬度
            {
                this.spLoc2.Caption = "未知坐标系";

            }
            else
            {
                IPoint jwPt = CoordinateTransHelper.XY2JWD(this.mapControl.ActiveView.FocusMap
                , aPt);
                if (jwPt != null && !jwPt.IsEmpty)
                {
                    string xStr = CoordinateTransHelper.FormatJWD(jwPt.X);
                    string yStr = CoordinateTransHelper.FormatJWD(jwPt.Y);
                    this.spLoc2.Caption = String.Format("{0},{1}", xStr, yStr);
                }
            }
            #endregion

            if (e.button == 4)
            {
                //鼠标中键按下且拖动时           
                this.Cursor = System.Windows.Forms.Cursors.SizeAll;
                mousePoint = this.mapControl .ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
                double moveDisX = mousePoint.X - startPnt.X;
                double moveDisY = mousePoint.Y - startPnt.Y;

                mPoint.X = mPoint.X - moveDisX;
                mPoint.Y = mPoint.Y - moveDisY;

                mEnvelope.CenterAt(mPoint);
                mapControl.ActiveView.Extent = mEnvelope;
                mapControl.ActiveView.Refresh();
            }  
            


        }

        /// <summary>
        /// 用户过滤显示
        /// </summary>
        double currMapScale = 0;
        sys.clsLayerFilter filter = null;    

        private void mapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            int mapScale = (int)(this.mapControl.MapScale);
            if (mapScale > 0)
            {
                NumberFormatInfo info = new NumberFormatInfo();
                info.NumberGroupSeparator = ",";
                info.NumberGroupSizes = new int[] { 3 };
                string strScale = mapScale.ToString("N", info);
                int index = strScale.LastIndexOf(".");
                if (index >= 0)
                    strScale = strScale.Substring(0, index);
                this.spMapScale.Caption = " 1:" + strScale;

                double sScale = this.mapControl.ActiveView.FocusMap.MapScale;
                sScale = Math.Round(sScale, 2);

            }
            if (RCIS.Global.AppParameters.DISPLAY_FILTER)
            {
                //为提高矢量数据的浏览效率，添加矢量数据的过滤   
                if (this.mapControl.MapScale < 300000)//暂时给定一个地图比例尺的临界值
                {
                    if (currMapScale != this.mapControl.MapScale)
                    {
                        if (filter == null)
                            filter = new sys.clsLayerFilter();
                        filter.ExecuteFilt(this.mapControl);
                    }
                    currMapScale = this.mapControl.MapScale;
                }
            }
            
        }

        private void mapControl_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            mEnvelope = this.mapControl.ActiveView.Extent;
            mPoint.X = (mEnvelope.XMax + mEnvelope.XMin) / 2;
            mPoint.Y = (mEnvelope.YMax + mEnvelope.YMin) / 2;  
        }

        //用于鼠标滚轮操作
        IEnvelope mEnvelope = null;  
        IPoint mousePoint = null;//鼠标点击点  
        IPoint startPnt = new PointClass();
        IPoint mPoint = new PointClass();//缩放中心点 
        private void mapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateNotEditing)
                {
                    m_menuMapBrowse.PopupMenu(e.x, e.y, this.mapControl.hWnd);
                    return;
                }
                //根据 当前状态 和 工具 决定弹出什么菜单
                //if (e.button == 2) m_toolbarMenu.PopupMenu(e.x, e.y, this.mapControl.hWnd);            

                ICommand tool = this.mapControl.CurrentTool as ICommand;
                if (tool == null)
                    return;
                if (tool.Name.ToUpper().Equals("ControlToolsEditing_Sketch".ToUpper()))
                {
                    m_menuSketch.PopupMenu(e.x, e.y, this.mapControl.hWnd);
                }
               
            } 
            else 
            if (e.button == 4)
            {//中键按下时，记住按下点的位置  

                startPnt.X = mapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y).X;
                startPnt.Y = mapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y).Y;
                mEnvelope = mapControl.ActiveView.Extent;
                mPoint.X = (mEnvelope.XMax + mEnvelope.XMin) / 2;
                mPoint.Y = (mEnvelope.YMax + mEnvelope.YMin) / 2;
            }         

        }


        private void ZoomMap(double ratio)
        {
            IEnvelope envelop = this.mapControl.Extent;
            double h = envelop.Height * (1 + ratio);
            double w = envelop.Width * (1 + ratio);
            envelop.Width = w;
            envelop.Height = h;
            //mapControl.Extent = envelop;
            this.mapControl.Extent = envelop;
        }


        private void PanMap(double ratioX, double ratioY)
        {
            //Pans map by amount specified given in a fraction of the extent e.g. rationX=0.5, pan right by half a screen   
            IEnvelope envelope = this.mapControl.Extent;
            double h = envelope.Width;
            double w = envelope.Height;
            envelope.Offset(h * ratioX, w * ratioY);
            mapControl.Extent = envelope;
        }
        private void mapControl_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            switch (e.keyCode)
            {
                case (int)System.Windows.Forms.Keys.Up:
                    PanMap(0d, 0.1d);
                    break;
                case (int)System.Windows.Forms.Keys.Down:
                    PanMap(0d, -0.1d);
                    break;
                case (int)System.Windows.Forms.Keys.Left:
                    PanMap(-0.1d, 0d);
                    break;
                case (int)System.Windows.Forms.Keys.Right:
                    PanMap(0.1d, 0d);
                    break;
                case (int)Keys.Oemplus:
                    ZoomMap(-0.1);
                    break;
                case (int)Keys.OemMinus:
                    ZoomMap(0.1);
                    break;
            }

        }

        #endregion

        #region 地图浏览

        private void br_browsermap_wipe_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SwipeLayerOptForm frm = new SwipeLayerOptForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            if (frm.ShowDialog() == DialogResult.Cancel)
                return;

            //卷帘工具
            ILayer pSwipeLayer = frm.retLyr;
            pEffectLayer.SwipeLayer = pSwipeLayer;//设置卷帘图层
            ICommand pCommand = new ControlsMapSwipeTool();//调用卷帘工具
            pCommand.OnCreate(this.mapControl.Object);//绑定工具
            this.mapControl.CurrentTool = pCommand as ITool;

        }



        private void br_map_zoomout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new ControlsMapZoomOutToolClass();
            cmd.OnCreate(this.mapControl.Object);

            this.mapControl.CurrentTool = cmd as ITool;
        }

        //private void br_map_pan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsMapPanToolClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    this.mapControl.CurrentTool = cmd as ITool;
        //}

        private void br_map_mouse_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.mapControl.CurrentTool = null;
        }

        //private void br_map_zoomin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsMapZoomInToolClass();
        //    cmd.OnCreate(this.mapControl.Object);

        //    this.mapControl.CurrentTool = cmd as ITool;
        //}

        //private void br_map_fullmap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsMapFullExtentCommandClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    cmd.OnClick();
        //}

        //private void br_map_prev_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsMapZoomToLastExtentBackCommandClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    cmd.OnClick();
        //}

        //private void br_map_next_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsMapZoomToLastExtentForwardCommandClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    cmd.OnClick();
        //}

        //private void br_map_select_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsSelectFeaturesToolClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    this.mapControl.CurrentTool = cmd as ITool;
        //}

        //private void br_map_clear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsClearSelectionCommandClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    cmd.OnClick();
        //}

        //private void br_map_information_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsMapIdentifyToolClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    this.mapControl.CurrentTool = cmd as ITool;

        //    //ICommand cmd = new RCIS.MapTool.FeatureIdentifyTool(this.mapControl);
        //    //cmd.OnCreate(this.mapControl.Object);
        //    //this.mapControl.CurrentTool = cmd as ITool;
        //}

        //private void br_map_find_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
           
        //    ICommand cmd = new ControlsMapFindCommandClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    cmd.OnClick();
        //}

        //private void br_map_adddata_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ICommand cmd = new ControlsAddDataCommandClass();
        //    cmd.OnCreate(this.mapControl.Object);
        //    cmd.OnClick();
        //}

        private void br_mapbrowser_bookmarkAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IMap pMap = this.mapControl.ActiveView.FocusMap;

            AddBookmarkForm frm = new AddBookmarkForm();
            frm.currMap = pMap;
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string bookmarkName = frm.newBookMarkName;

            IAOIBookmark areaOfInterest = new AOIBookmarkClass();

            areaOfInterest.Location = this.mapControl.ActiveView.Extent;
            // Give the bookmark a name.
            areaOfInterest.Name = bookmarkName;
            // Add the bookmark to the map's bookmark collection. This adds the bookmark 
            // to the Bookmarks menu, which is accessible from the View menu.
            IMapBookmarks mapBookmarks = pMap as IMapBookmarks;
            mapBookmarks.AddBookmark(areaOfInterest);
        }

        private void br_mapbrowser_bookmarkmanage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            BookMarkManageFrm frm = new BookMarkManageFrm();
            frm.currActiveView = this.mapControl.ActiveView;
            frm.ShowDialog();
        }


        //private void br_mapbrowser_measure_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    ////测距
        //    //ICommand cmd = new ControlsMapMeasureToolClass();
        //    //cmd.OnCreate(this.mapControl.Object);
        //    //this.mapControl.CurrentTool = cmd as ITool;

        //    ICommand cmd = new Measure.MeasuredisTool();
        //    cmd.OnCreate(this.mapControl.Object);
        //    this.mapControl.CurrentTool = cmd as ITool;
        //}


        #endregion

        #region 开始编辑

        private void setComponentEnable(bool b)
        {
            this.rpgEditTool.Enabled = b;//编辑工具可用
            this.br_edit_stop.Enabled = b;
            this.br_edit_save.Enabled = b;
        }

        ////刷新当前图层列表
        //private void UpdateEditLayerList()
        //{



        //    this.comEditLayer.Properties.Items.Clear();
        //    IMap pMap = this.mapControl.Map;
        //    for (int i = 0; i < pMap.LayerCount; i++)
        //    {
        //        ILayer aLyr = pMap.get_Layer(i);
        //        string layerName = aLyr.Name.Trim();
        //        if (aLyr is IFeatureLayer)
        //        {
        //            layerName = ((aLyr as IFeatureLayer).FeatureClass as IDataset).Name;
        //        }



        //        if (aLyr is IFeatureLayer)
        //        {
        //            this.comEditLayer.Properties.Items.Add(layerName);
        //        }
        //    }

        //    if (this.comEditLayer.Properties.Items.Count > 0)
        //    {
        //        this.comEditLayer.EditValue = this.comEditLayer.Properties.Items[this.comEditLayer.Properties.Items.Count - 1];
        //    }
        //}


        //刷新当前图层列表
        private void UpdateEditLayerList(IWorkspace currWS)
        {
            
            this.comEditLayer.Properties.Items.Clear();
            IMap pMap = this.mapControl.Map;
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer aLyr = pMap.get_Layer(i);
                if (aLyr is IFeatureLayer)
                {
                    IFeatureLayer pFeaLyr = aLyr as IFeatureLayer;
                    IFeatureClass pFC = pFeaLyr.FeatureClass;
                    IDataset pDS = pFC as IDataset;
                    if (pDS.Workspace != currWS)
                        continue;

                    string layerName = aLyr.Name.Trim();
                    layerName = pDS.Name.ToUpper();
                    this.comEditLayer.Properties.Items.Add(layerName);
                }
                else if (aLyr is IGroupLayer)
                {
                    ICompositeLayer pCLyr = aLyr as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer pFeaLyr = childLyr as IFeatureLayer;
                            IFeatureClass pFC = pFeaLyr.FeatureClass;
                            IDataset pDS = pFC as IDataset;
                            if (pDS.Workspace != currWS)
                                continue;

                            string layerName = aLyr.Name.Trim();
                            layerName = pDS.Name.ToUpper();
                            this.comEditLayer.Properties.Items.Add(layerName);
                        }
                    }
                }

            }

            if (this.comEditLayer.Properties.Items.Count > 0)
            {
                this.comEditLayer.EditValue = this.comEditLayer.Properties.Items[this.comEditLayer.Properties.Items.Count - 1];
            }
        }

        private List<IWorkspace> getAllWorkspace()
        {
            List<IWorkspace> lstWses = new List<IWorkspace>();
            for (int i = 0; i < this.mapControl.LayerCount; i++)
            {
                ILayer aLyr = this.mapControl.get_Layer(i);
                if (aLyr is IFeatureLayer)
                {
                    IFeatureClass aFC = (aLyr as IFeatureLayer).FeatureClass;
                    IDataset aDS = aFC as IDataset;
                    IWorkspace tmpWs = aDS.Workspace;
                    if (!lstWses.Contains(tmpWs))
                    {
                        lstWses.Add(tmpWs);
                    }

                }
                else if (aLyr is IGroupLayer)
                {
                    ICompositeLayer pCLyr = aLyr as ICompositeLayer;
                    for (int j = 0; j < pCLyr.Count; j++)
                    {
                        ILayer childLyr = pCLyr.get_Layer(j);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureClass aFC = (childLyr as IFeatureLayer).FeatureClass;
                            IDataset aDS = aFC as IDataset;
                            IWorkspace tmpWs = aDS.Workspace;
                            if (!lstWses.Contains(tmpWs))
                            {
                                lstWses.Add(tmpWs);
                            }
                        }
                    }
                }

            }
            return lstWses;
        }
        private void br_edit_start_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            this.rgVisibleLayersEdit.SelectedIndex = 0;

            List<IWorkspace> lstAllWs = getAllWorkspace();
            IWorkspace currWs = null;
            if (lstAllWs.Count == 0)
            {
                MessageBox.Show("没有要编辑的矢量图层!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstAllWs.Count == 1)
            {
                currWs = lstAllWs[0];
            }
            else
            {
                sys.WorkspaceSelectForm frm = new sys.WorkspaceSelectForm();
                frm.sendWses = lstAllWs;
                if (frm.ShowDialog() == DialogResult.Cancel)
                    return;
                currWs = frm.retWs;
            }
            if (currWs == null)
                return;

            

            GlobalEditObject.GlobalWorkspace = currWs;

            UpdateEditLayerList(currWs); //开始编辑的时候，要知道当前地图有多少个 图层
            LoadAllLayerStylePanel(currWs,false);
            dockpanelLayerSelect.Show();
            
            setComponentEnable(true);
              
            try
            {
                
                this.mapControl.Map.ClearSelection();                

                GlobalEditObject.StartEditingObj(this.mapControl.Object as IMapControl2,
                     GlobalEditObject.CurrentEngineEditor,
                    this.comEditLayer.EditValue.ToString().Trim());
                this.mapControl.CurrentTool = null;
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);

                //默认工具


               
            }
            catch (Exception ex)
            {
                MessageBox.Show("开启编辑失败，请检查原始数据！\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }



        private void br_edit_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new ControlsEditingSaveCommandClass();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();


        }

        private void br_edit_stop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalEditObject.StopEditingObj(GlobalEditObject.CurrentEngineEditor);

            ICommand cmd = new ControlsMapPanToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
            //this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll,
            //    null, null);

            setComponentEnable(false);

            this.dockpanelLayerSelect.Hide();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        #endregion

        #region 编辑工具

        //自动接边
        private void bi_edit_autoJB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.AutoEdgeTool();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;

           
        }
        

        //宗地权利人
        private void br_edit_zdqlr_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.ZDQlrForm frm = new edit.ZDQlrForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }


        //节点属性
        private void bi_edittool_pointzb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sketchPropertiesForm propFrm = new sketchPropertiesForm(GlobalEditObject.CurrentEngineEditor, mapControl);
            propFrm.TopMost = true;
            propFrm.Show();
        }

        //图幅制作
        private void br_edit_TFMake_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            datado.MakeTFCommand cmd = new datado.MakeTFCommand(GlobalEditObject.CurrentEngineEditor);
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }


        private void br_edit_select_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            //GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "ControlToolsEditing_ModifyFeatureTask");
            ICommand cmd = new ControlsEditingEditToolClass();
            cmd.OnCreate(this.mapControl.Object);
            ITool tool = cmd as ITool;
            this.mapControl.CurrentTool = cmd as ITool;

            ((IEngineEditProperties2)GlobalEditObject.CurrentEngineEditor).StickyMoveTolerance = RCIS.Global.AppParameters.EDIT_STICKMOVETOLERANCE;
        }

        //掏出洞
        private void br_edit_hole_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.SetHoleForm frm = new edit.SetHoleForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        //联动编辑
        private void bi_edit_liandong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "RemoveSmallTbEditTask");

            ICommand cmd = new ControlsEditingSketchToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
        }


        private void bi_edit_reshape_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "EngineEditTasksMyReshape");          

            ICommand cmd = new ControlsEditingSketchToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
            
        }



        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //提取界址点
            edit.GetJZDCommand cmd = new edit.GetJZDCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();


        }

        //提取面边界
        private void bi_editool_getpolygonRing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //提取面边界
            edit.Polygon2LineFeatureCommand cmd = new edit.Polygon2LineFeatureCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_edit_sketchtool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "ControlToolsEditing_CreateNewFeatureTask");
            ICommand cmd = new ControlsEditingSketchToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
            ((IEngineEditProperties2)GlobalEditObject.CurrentEngineEditor).SnapTips = true;

        }

        private void br_edit_property_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                IEnumFeature pEnumFeature = GlobalEditObject.CurrentEngineEditor.EditSelection;
                IFeature selectFea = pEnumFeature.Next();
                if (selectFea == null)
                {
                    return;
                }
                string className = LayerHelper.GetClassShortName(selectFea.Class as IDataset);
                PropertyEditorForm frm = new PropertyEditorForm();
                frm.EditorObject = GlobalEditObject.CurrentEngineEditor;
                frm.ShowDialog();

            }
            catch { }
        }

        private void br_edit_circle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.RotateTool(GlobalEditObject.CurrentEngineEditor);
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
        }

        private void br_edit_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.FeaturesDeleteCommand(GlobalEditObject.CurrentEngineEditor);
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_edit_copy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new ControlsEditingCopyCommandClass();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_edit_paste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new ControlsEditingPasteCommandClass();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_edit_union_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.FeatureUnionCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void bi_edittool_pointSparseness_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //抽稀
            ICommand cmd = new RCIS.MapTool.VertexSparsenessCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_edit_split_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定以何种方式分割当前选中图层的数据？\r\n是：绘制线分割 否：按选中线进行分割 ", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Cancel)
                return;
            if (dr == DialogResult.Yes)
            {
                GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "EngineEditTasksCutPolygonWithoutSelect");
                this.br_edit_sketchtool.Enabled = true;
                ICommand cmd = new ControlsEditingSketchToolClass();
                cmd.OnCreate(this.mapControl.Object);
                this.mapControl.CurrentTool = cmd as ITool;
            }
            else
            {
                ICommand cmd=new  RCIS.MapTool.CutPolygonBySelectLineCommand();
                cmd.OnCreate(this.mapControl.Object);
                cmd.OnClick();
            }
            
        }

        private void br_edit_pan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "EngineEditTasksPanEditPolygon");
            ICommand cmd = new ControlsEditingSketchToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
        }
        //延伸
        private void br_edit_extent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.GeometryExtentTool(RCIS.Global.GlobalEditObject.CurrentEngineEditor);
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
        }

        private void br_edit_undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //撤销
            GlobalEditObject.MapUndoEdit();
            IActiveView pActiveView = (IActiveView)this.mapControl.ActiveView;
            pActiveView.FocusMap.ClearSelection();
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent.Envelope);

        }

        private void br_edit_redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GlobalEditObject.MapRedoEdit();
            IActiveView pActiveView = (IActiveView)this.mapControl.ActiveView;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent.Envelope);

        }

        private void br_edit_snap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            

            SnapSetupForm2 snapFrm = new SnapSetupForm2();
            snapFrm.globalEngineEditor = GlobalEditObject.CurrentEngineEditor;
            snapFrm.currMap = this.mapControl.ActiveView.FocusMap;
            snapFrm.ShowDialog();
        }

        private void bi_edit_zldwdmsetvalueByextent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //坐落单位代码赋值，当前范围内
            edit.SetZldwdmByExtentForm frm = new edit.SetZldwdmByExtentForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void br_edit_qsdwdmsetvalue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.SetQsdwdmFrmByExtent frm = new edit.SetQsdwdmFrmByExtent();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }


        //裁切相交部分
        private void bi_edit_cutOverlaps_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.MapTool.CutIntersectCommand cmd = new CutIntersectCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        //插入结点
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            ICommand cmd = new edit.InsertVertexTool(GlobalEditObject.CurrentEngineEditor);
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;

        }

        //删除节点
        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
            ICommand cmd = new edit.DelVertexTool(GlobalEditObject.CurrentEngineEditor);
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
           
        }


        //提取面中心点
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.Controls.Polygon2CentroidForm frm = new Polygon2CentroidForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }


        #endregion

        #region 辅助工具

        //显示线、面节点
        private void br_else_showVertex_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (this.br_else_showVertex.Caption == "显示线、面节点")
            //{
            //    sys.ShowVertexEleForm frm = new sys.ShowVertexEleForm();
            //    frm.currMap = this.mapControl.ActiveView.FocusMap;
            //    if (frm.ShowDialog() == DialogResult.OK)
            //    {
            //        string className = frm.retclassName;
            //        IFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, className);
            //        if (pLyr != null)
            //        {
            //            ISymbol markerSym = SymbolHelper.CreateSimpleMarkerSymbol(System.Drawing.Color.AliceBlue, 5) as ISymbol;

            //            IFeatureClass pFC = pLyr.FeatureClass;
            //            ISpatialFilter pSF = new SpatialFilterClass();
            //            pSF.Geometry = this.mapControl.ActiveView.Extent.Envelope;
            //            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //            IFeature aFea = null;
            //            IFeatureCursor pCursor = pFC.Search(pSF as IQueryFilter, false);
            //            try
            //            {
            //                while ((aFea = pCursor.NextFeature()) != null)
            //                {
            //                    IGeometry geo = aFea.Shape;
            //                    IPointCollection pts = geo as IPointCollection;
            //                    for (int i = 0; i < pts.PointCount; i++)
            //                    {
            //                        IMarkerElement newEle = new MarkerElementClass();
            //                        newEle.Symbol = markerSym as IMarkerSymbol;
            //                        IElement ele = newEle as IElement;
            //                        ele.Geometry = pts.get_Point(i) as IGeometry;
            //                        this.mapControl.ActiveView.GraphicsContainer.AddElement(newEle as IElement, 0);
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //            }
            //            finally
            //            {
            //                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            //            }
            //            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //        }
            //    }
            //    this.br_else_showVertex.Caption = "取消显示线、面节点";
            //}
            //else
            //{
            //    this.mapControl.ActiveView.GraphicsContainer.DeleteAllElements();
            //    this.br_else_showVertex.Caption = "显示线、面节点";

            //}

        }

        //备份
        private void br_data_backup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sys.BackupForm frm = new sys.BackupForm();
            frm.ShowDialog();


        }

        //计算选中
        private void br_else_CalSelection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.mapControl.ActiveView.FocusMap.LayerCount == 0)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RCIS.Controls.CalSelFeatureForm cal = new CalSelFeatureForm();
            cal.currMap = this.mapControl.Map;
            cal.ShowDialog();
        }
        //符号编辑
        private void br_styleManager_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + @"\LsEditor.exe");
        }

        //其他设置
        private void barButtonItem28_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sys.OtherSetupForm frm = new sys.OtherSetupForm();
            frm.ShowDialog();
            
        }

        //字典表
        private void br_else_dic_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sys.DictionaryForm frm = new sys.DictionaryForm();
            frm.ShowDialog();
        }



        private void br_othertool_fsbh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ElseTransform.CorTranslateForm myForm = new ElseTransform.CorTranslateForm();
            myForm.StartPosition = FormStartPosition.CenterScreen;
            myForm.m_MapControl = this.mapControl;
            myForm.ShowDialog();
        }


        private void bi_othertool_7bianhuan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ElseTransform.SevenParasCorTransForm myForm = new ElseTransform.SevenParasCorTransForm();
            myForm.StartPosition = FormStartPosition.CenterScreen;
            myForm.m_MapControl = this.mapControl;
            myForm.ShowDialog();
        }


        private void br_interface_e002shp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.DataExchange.E00.DataExchangeFormE002Shape frm = new RCIS.DataExchange.E00.DataExchangeFormE002Shape();
            frm.ShowDialog();
        }

        //VCT2 导入
        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.DataInterface.VCT2.VCTInFilGdb frm = new RCIS.DataInterface.VCT2.VCTInFilGdb();
            frm.ShowDialog();
        }

        private void br_interface_gps2shp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.DataExchange.GPS.GPSExportForm gpsFrm = new RCIS.DataExchange.GPS.GPSExportForm();
            gpsFrm.ShowDialog();
        }

        private void br_interface_cad2shp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.DataExchange.DWG2SHP.Cad2ShpForm cadFrm = new RCIS.DataExchange.DWG2SHP.Cad2ShpForm();
            cadFrm.ShowDialog();
        }

        private void br_interface_txt2shp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.DataInterface.Txt2ShpFrm frm = new RCIS.DataInterface.Txt2ShpFrm();
            frm.ShowDialog();
        }


        private void br_othertool_expXY_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new RCIS.MapTool.GeometryExportCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_othertool_unselectrender_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MultiLayerSelectForm aSelForm = new MultiLayerSelectForm(
                    this.mapControl.ActiveView.FocusMap
                    , "取消被选中面状图层渲染", "请选中一个图层");
            aSelForm.btnSelectAll.Visible = false;
            aSelForm.btnUnSelectAll.Visible = false;
            List<IGeoFeatureLayer> m_pLayerList = new List<IGeoFeatureLayer>();

            string sLayerName = "";
            if (aSelForm.ShowDialog() == DialogResult.OK)
            {
                m_pLayerList = aSelForm.SelectedLayerList;
                if (m_pLayerList.Count != 1)
                {
                    MessageBox.Show("请选择一个面状图层");
                    return;
                }
                else
                {
                    IGeoFeatureLayer pGFL = m_pLayerList[0];
                    if ((pGFL as IFeatureLayer).FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
                    {
                        MessageBox.Show("请选择一个面状图层");
                        return;
                    }
                    else
                    {
                        sLayerName = LayerHelper.GetFeatureLayerTableName(pGFL.FeatureClass);
                    }
                }
            }



            IMap pMap = this.mapControl.ActiveView.FocusMap;

            if (pMap == null) return;
            int LayerIndex = -1;
            for (int i = 0; i < this.mapControl.LayerCount; i++)
            {
                ILayer currLyr = this.mapControl.get_Layer(i);
                if (!(currLyr is IFeatureLayer))
                    continue;

                IFeatureLayer curFeaLyr = currLyr as IFeatureLayer;
                IFeatureClass featClass = curFeaLyr.FeatureClass;
                if (LayerHelper.GetClassShortName(featClass as IDataset).ToUpper().Equals(sLayerName.ToUpper()))
                {
                    LayerIndex = i;
                    break;
                }

            }


            if (LayerIndex == -1) return;
            IFeatureSelection pFeatureSel = (IFeatureSelection)pMap.get_Layer(LayerIndex);


            pFeatureSel.SetSelectionSymbol = false;

            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,
                null, null);
        }

        private void br_othertool_renderSelect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MultiLayerSelectForm aSelForm = new MultiLayerSelectForm(
                   this.mapControl.ActiveView.FocusMap
                   , "选择图层", "请选中一个图层");
            aSelForm.btnSelectAll.Visible = false;
            aSelForm.btnUnSelectAll.Visible = false;
            List<IGeoFeatureLayer> m_pLayerList = new List<IGeoFeatureLayer>();
            if (aSelForm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            m_pLayerList = aSelForm.SelectedLayerList;
            if (m_pLayerList.Count != 1)
            {
                MessageBox.Show("请选择一个面状图层", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            IGeoFeatureLayer pGFL = m_pLayerList[0];
            if ((pGFL as IFeatureLayer).FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
            {
                MessageBox.Show("请选择一个面状图层");
                return;
            }

            ISymbol dispSymbol = aSelForm.RetSymbol;
            IMap pMap = this.mapControl.ActiveView.FocusMap;
            if (pMap == null) return;
            int LayerIndex = -1;
            for (int i = 0; i < this.mapControl.LayerCount; i++)
            {
                ILayer currLyr = this.mapControl.get_Layer(i);
                if (!(currLyr is IGeoFeatureLayer))
                {

                    continue;
                }
                IGeoFeatureLayer geoFeature = currLyr as IGeoFeatureLayer;
                if (pGFL.Equals(geoFeature))
                {
                    LayerIndex = i;
                    break;
                }
            }
            if (LayerIndex == -1)
                return;
            IFeatureSelection pFeatureSel = (IFeatureSelection)pMap.get_Layer(LayerIndex);

            pFeatureSel.SetSelectionSymbol = true;
            pFeatureSel.SelectionSymbol = dispSymbol;
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection,
                null, null);
        }

        private void br_othertool_outImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.Controls.OutputImageForm frm = new OutputImageForm();
            frm.m_MapControl = this.mapControl;
            frm.m_PageControl = this.m_layoutControlPanel.LayoutControl;
            frm.m_myTab = this.xtraTabControl1;
            frm.ShowDialog();
        }
                

        //属性查询
        private void bi_query_objectproperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ObjectSearchForm objSearchForm = new ObjectSearchForm();
            objSearchForm.ActiveView = this.mapControl.ActiveView;
            objSearchForm.MapControl = this.mapControl;
            objSearchForm.Show(this);
        }

        //空间查询
        private void barButtonItem37_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.Query.SpatialQueryForm2 m_myForm = new RCIS.Query.SpatialQueryForm2();
            m_myForm.StartPosition = FormStartPosition.CenterScreen;
            m_myForm.m_MapControl = this.mapControl;
            if (m_myForm.Visible == false)
                m_myForm.Show(this);
        }
        
        #endregion


        #region  建库功能
        private void bi_bggctq_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //edit.GetGXGCForm frm = new edit.GetGXGCForm();
            //frm.currWs = GlobalEditObject.GlobalWorkspace;
            //frm.ShowDialog();
            gengxin.FrmCreateProcess frm = new gengxin.FrmCreateProcess();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.currMap = mapControl.Map;
            frm.ShowDialog();
        }

        private void br_sys_createIndex_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool bOk = true;
            try
            {
                IEnumDataset dss = RCIS.Global.GlobalEditObject.GlobalWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                dss.Reset();
                IDataset aDs = null;
                while ((aDs = dss.Next()) != null)
                {
                    IFeatureDataset pFeaDs = aDs as IFeatureDataset;
                    if (pFeaDs != null)
                    {
                        IFeatureClassContainer pclassContainer = pFeaDs as IFeatureClassContainer;
                        for (int i = 0; i < pclassContainer.ClassCount; i++)
                        {
                            IFeatureClass pFC = pclassContainer.get_Class(i);
                            bOk&=  LayerHelper.CreatePropIndex(pFC);
                        }
                    }
                }
                if (bOk)
                {
                    MessageBox.Show("创建完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("创建完毕！未完全成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void br_sx_jxxzlx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //界线性质，界线类型
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            datado.JxxzSetValueForm1cs frm = new datado.JxxzSetValueForm1cs();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void br_sx_gdxhdmsetvalue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        

        //追加数据
        private void bi_dbtool_Append_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sys.AppendFCForm frm = new sys.AppendFCForm();
            frm.DestinationWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //合并数据库
        private void barButtonItem48_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            edit.MergeDatabase frm = new edit.MergeDatabase();
            frm.tarWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //融合
        private void br_geometrypatch_dissovle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            datado.DissovleForm frm = new datado.DissovleForm();
            frm.ShowDialog();
        }


        //代码名称维护
        private void bi_SX_DMMCWH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //代码名称维护
            
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
    
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.DMMCWHForm frm = new datado.DMMCWHForm();
            frm.currWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

      
        //导入VCT
        private void bi_vct3in_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RCIS.DataInterface.VCT3In.VCT3InFGDB frm = new RCIS.DataInterface.VCT3In.VCT3InFGDB();
            frm.ShowDialog();
        }


        //bsm维护
        private void br_sxwh_BSMWH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            

            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.BSMWHForm frm = new datado.BSMWHForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

         private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sys.AltSpatialRefForm frm = new sys.AltSpatialRefForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //图件输出
        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  

            output.LSOutForm2 m_LSOutForm = new output.LSOutForm2();
            //sycWS.LSOutForm m_LSOutForm = new sycWS.LSOutForm();
            m_LSOutForm.StartPosition = FormStartPosition.CenterScreen;
            m_LSOutForm.m_MapControl = this.mapControl;
            m_LSOutForm.m_myTab = this.xtraTabControl1;
            m_LSOutForm.m_PageControl = this.m_layoutControlPanel.LayoutControl;
           
            if (m_LSOutForm.Visible == false)
                m_LSOutForm.Show();

        }

        private void 导出辖区数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null) return;
            if (this.tvXzq.Nodes.Count == 0)
                return;
            if (this.tvXzq.SelectedNode == null)
                return;

            string xzqdm = OtherHelper.GetLeftName(this.tvXzq.SelectedNode.Text);

            output.OutXzqDataForm frm = new output.OutXzqDataForm();
            frm.sendWs = GlobalEditObject.GlobalWorkspace;
            frm.sendXZDM = xzqdm;
            frm.ShowDialog();
            
        }

        //图斑地类面积重新计算
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            
            
        }


        //常规统计
        private void bi_dbtool_statsgeneral_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  
            datado.GeneralStatsForm frm = new datado.GeneralStatsForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }
        //统计汇总表
        private void br_dbtool_stats_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  

            output.StatesForm frm = new output.StatesForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //导入权属单位代码表
        private void bi_dbtool_importQsdwdmb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载工作空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            sys.InQsdwdmForm frm = new sys.InQsdwdmForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }


        //创建标准库
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sys.ConstructBZKForm frm = new sys.ConstructBZKForm();
            frm.ShowDialog();
            if (frm._isOK)
            {
                //打开filegdb文件
                GlobalEditObject.GlobalWorkspace = WorkspaceHelper2.GetFileGdbWorkspace(frm._GDBPath);

                //记录路径
                try
                {
                    RegistryKey keyWrite = Registry.CurrentUser;
                    keyWrite = keyWrite.CreateSubKey("Software\\landstar");
                    keyWrite.SetValue("filegdbpath", frm._GDBPath);
                }
                catch { }

                if (GlobalEditObject.GlobalWorkspace != null)
                {
                    IFeatureDataset pFeatureDataset = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureDataset(AppParameters.DATASET_DEFAULT_NAME);
                    RCIS.GISCommon.GeometryHelper._tolerance = ((pFeatureDataset as IGeoDataset).SpatialReference as ISpatialReferenceTolerance).XYTolerance;
                    AddOpenFileLayers();
                }
            }
        }
        

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //表结构维护
            sys.TableStructForm frm = new sys.TableStructForm();
            frm.ShowDialog();           

        }
        

        //导入单个要素
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sys.InAFeatureClassForm frm = new sys.InAFeatureClassForm();
            frm.DestinationWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //快速导入
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  

            //sys.QuickImportForm frm = new sys.QuickImportForm();
            sys.QuickImportForm2 frm = new sys.QuickImportForm2();
            frm.DestinationWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //删除重复要素
        private void barButtonItem32_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.DoDuplicateForm2 frm = new datado.DoDuplicateForm2();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        //删除重复要素
        private void barButtonItem31_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteIdenticalForm frm = new DeleteIdenticalForm();
            frm.ShowDialog();
        }

      

        private void bi_datado_kzmjtp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {                     

            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.KzmjTpForm frm = new datado.KzmjTpForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //面积计算及调平
        private void br_datado_mjtp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.CalmjandTpFrm frm = new datado.CalmjandTpFrm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

       


        /// <summary>
        /// /重拍标识码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //重排标识码
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
           
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    
            datado.BSMRebuildForm frm = new datado.BSMRebuildForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }


        //数据检查
        private void br_dbtool_datacheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  

            RuleCheck.AutoCheckForm frm = new RuleCheck.AutoCheckForm();
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace != null)
            {
                frm.currWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
                frm.currEnv = this.mapControl.ActiveView.Extent;

            }            
            frm.ShowDialog();
            DataTable chekErrTable = frm.currentCheckErrTab;
            if (chekErrTable != null)
            {
                this.gridControlError.DataSource = chekErrTable;
                this.dockPanelError.Show();
            }
        }


        /// <summary>
        /// 自定义建立拓扑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bi_dbool_topmanage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  

            datado.createTopologyFrm frm = new datado.createTopologyFrm();
            frm.currWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void bi_dbool_topcheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }  

            //拓扑管理
            datado.TopManagerForm frm = new datado.TopManagerForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.mapctl = this.mapControl.Object as IMapControl3;
            frm.Show();


        }


        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //要素代码赋值
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            sys.YsdmSetValueForm frm = new sys.YsdmSetValueForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        

        private void bi_dbtool_outTujian_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sycWS.LSOutForm m_LSOutForm = new sycWS.LSOutForm();
            m_LSOutForm.StartPosition = FormStartPosition.CenterScreen;
            m_LSOutForm.m_MapControl = this.mapControl;
            m_LSOutForm.m_myTab = this.xtraTabControl1;
            m_LSOutForm.m_PageControl = this.m_layoutControlPanel.LayoutControl;
            
            m_LSOutForm.currentWorkspace = RCIS.Global.GlobalEditObject.GlobalWorkspace;


            if (m_LSOutForm.Visible == false)
                m_LSOutForm.Show();

        }


        //面转线
        private void br_dbtool_polygon2line_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            RCIS.MapTool.Polygon2PolylineFrm frm = new Polygon2PolylineFrm();
            frm.CurrWorkspace = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void br_dbtool_featuretoPolygon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //叠加分析
            datado.Features2PolygonForm frm = new datado.Features2PolygonForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }

        //叠加分析
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //叠加分析
            datado.IntersecPolygonForm frm = new datado.IntersecPolygonForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }
        private void br_dbtool_dlchange_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //工作分类转换 测试可以执行 sql语句
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            edit.DLChangeForm frm = new edit.DLChangeForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
            
        }
        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //坐落单位赋值
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.ZldwSetValueForm frm = new datado.ZldwSetValueForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }
        

        //按照不同属性条件修改赋值
        private void br_sx_setValBysql_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            datado.SetValByWhereForm frm = new datado.SetValByWhereForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();

        }


        private void br_sxpatch_mjcal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //面积维护
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            datado.TbdlmjCalForm frm = new datado.TbdlmjCalForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = (GlobalEditObject.GlobalWorkspace);
            frm.ShowDialog();
        }

        #endregion
                
        #region 地物编辑功能

        //将选中图斑批量合并到大图斑
        private void br_edit_unionPatch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.UnionSelTBPatchForm frm = new edit.UnionSelTBPatchForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }


        //线状地物专面后 预变更，产生的小夹角，删除
        private void br_edtool_preDoSmall_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.XzdwTbhPreBgDoSmallForm dltbFrm = new edit.XzdwTbhPreBgDoSmallForm();
            dltbFrm.currMap = this.mapControl.ActiveView.FocusMap;
            dltbFrm.currWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            dltbFrm.ShowDialog();
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent.Envelope);
        }

       

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.DLTBUnionCommand cmd = new edit.DLTBUnionCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        private void br_edit_xxtbkdCal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.XxdwkdSetValForm frm = new edit.XxdwkdSetValForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //CutTbWithoutSelectTask
            GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "CutTbWithoutSelectTask");

            this.br_edit_sketchtool.Enabled = true;

            ICommand cmd = new ControlsEditingSketchToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
        }

        //零星地物灭失
        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //零星地物灭失
            edit.LxdwMsCommand cmd = new edit.LxdwMsCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        //线装地物灭失
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //线装地物灭失
            edit.XzdwmsCommand cmd = new edit.XzdwmsCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();

        }


        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增地类图斑
            ICommand cmd = new ControlsEditingSketchToolClass();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
            GlobalEditObject.ChangeEditTask(GlobalEditObject.CurrentEngineEditor, "TbMultiBGTask");


        }
        //删除行政区边界外图斑
        private void bi_edit_deleteOutXian_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.DelOutXianTbForm frm = new edit.DelOutXianTbForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent);
        }

       
        //地物编辑
        private void bi_edit_dwbj_xzdw2tb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //线装地物转面，
            ICommand cmd = new edit.Xzdw2DltbCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        //宗地分割图斑
        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.ZdSplitDltbOptForm frm = new edit.ZdSplitDltbOptForm();
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem26_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.XzqSplitDltbOptForm frm = new edit.XzqSplitDltbOptForm();
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }
        
        //图斑编号重计算
        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //图斑编号重计算
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.TbbhSetForm frm = new datado.TbbhSetForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;

            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }

        //贝塞尔平滑
        private void barButtonItem40_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.SmoothPolygonForm frm = new edit.SmoothPolygonForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //多部分打散
        private void bi_edit_multipartDo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            //多部分打散
            edit.MultipartDoForm frm = new edit.MultipartDoForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //图斑面积重新计算

            edit.TbmjCalculateForm calFrm = new edit.TbmjCalculateForm();
            calFrm.currMap = this.mapControl.Map;
            calFrm.mapcontrol = this.mapControl;
            calFrm.ShowDialog();

        }
        //破碎地类图斑合并命令
        private void barButtonItem27_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool isOpen = false;    //初始为未打开状态
            foreach (Form frm in Application.OpenForms) //遍历已打开窗口
              {
                  if (frm is edit.SmallDltbMergeForm)  
                  {
                      //如果此窗口已打开，则激活
                      frm.Activate();
                     isOpen = true;
                     break;
                 }
             }
            if (!isOpen)
            {
                edit.SmallDltbMergeForm dltbFrm = new edit.SmallDltbMergeForm();
                dltbFrm.currMap = this.mapControl.ActiveView.FocusMap;
                dltbFrm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
                dltbFrm.mapControl = this.mapControl.Object as IMapControl2;

                dltbFrm.Show();
            }
            //this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent);
        }

        //自动填充裂隙
        private void barButtonItem29_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.DoBreakedTbForm frm = new edit.DoBreakedTbForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }
        //合并图斑哦
        private void br_edittool_xwyshb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {           

            edit.TBHBBySxFrm frm = new edit.TBHBBySxFrm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.mapControl = this.mapControl.Object as IMapControl2;
            frm.Show();

        }
        private void br_edittool_patchBg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.PatchBGOptForm frm = new edit.PatchBGOptForm();
            frm.Enabled = true;
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        //统一赋属性值
        private void br_edit_setSameValue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IFeatureLayer dltbLayer = null;
            dltbLayer = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, "DLTB");
            if (dltbLayer != null)
            {
                List<IFeature> arFeas = LayerHelper.GetSelectedFeature(this.mapControl.ActiveView.FocusMap, dltbLayer as IGeoFeatureLayer);
                sxvalue.DltbSetValueForm frm = new sxvalue.DltbSetValueForm();
                frm.sendFeatures = arFeas;
                frm.ShowDialog();
            }
            
        }

        //自动提取宗地四至
        private void br_edit_tqzdsz_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand cmd = new edit.ZdszWhCommand();
            cmd.OnCreate(this.mapControl.Object);
            cmd.OnClick();
        }

        //属性批量赋值
        private void br_edit_setPropertyValue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.EditPropertySetValueForm frm = new edit.EditPropertySetValueForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        //选择面分割图斑
        private void barButtonItem34_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.SelectSplitDltbOptForm frm = new edit.SelectSplitDltbOptForm();
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }


        private void barButtonItem13_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //按照村级调查区分割图斑
            edit.CjdcqSplitDltbOptForm frm = new edit.CjdcqSplitDltbOptForm();
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();

        }
       


        #endregion

        
        #region 影像工具

        private void br_raster_information_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            raster.RasterInfoForm frm = new raster.RasterInfoForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void br_raster2polygon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            raster.Raster2PolygonFrm frm = new raster.Raster2PolygonFrm();
            frm.ShowDialog();
        }

        //坡向分析
        private void br_raster_Aspect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            raster.AspectFrm frm = new raster.AspectFrm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                IRaster pRaster = frm.pOutRaster;
                if (pRaster != null)
                {

                    IRasterLayer pRasterLyr = new RasterLayerClass();
                    pRasterLyr.CreateFromRaster(pRaster);
                    this.mapControl.AddLayer(pRasterLyr as ILayer, 0);
                    this.mapControl.Extent = (pRasterLyr as ILayer).AreaOfInterest;
                }
            }
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.mapControl.LayerCount == 0) return;
            raster.clipRasterFrm frm = new raster.clipRasterFrm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();

        }
        private void bi_raster_noblack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.mapControl.LayerCount == 0) return;
            raster.RasterNoDatavalueFrm frm = new raster.RasterNoDatavalueFrm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
            }
            
        }

        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.mapControl.LayerCount == 0) return;
            //配准
            ICommand cmd = new RCIS.MapTool.GeoReferenceTool();
            cmd.OnCreate(this.mapControl.Object);
            this.mapControl.CurrentTool = cmd as ITool;
        }
        //镶嵌数据集管理
        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //镶嵌数据集管理
            raster.MosaicDsManageForm frm = new raster.MosaicDsManageForm();
            frm.ShowDialog();
        }

        //影像渲染
        private void br_raster_render_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            raster.rasterRenderFrm fmRaster = new raster.rasterRenderFrm(this.mapControl, this.mapTocc);
            fmRaster.ShowDialog();
        }

        //重分类
        private void br_raster_reclass_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            raster.RasterReClassForm frm = new raster.RasterReClassForm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                IRaster pRaster = frm.pOutRaster;
                if (pRaster != null)
                {
                    IRaster2 pRaster2 = pRaster as IRaster2;
                    IRasterDataset pRDS = pRaster2.RasterDataset;
                    if (pRDS != null)
                    {
                        IRasterPyramid pRPyramid = pRDS as IRasterPyramid;
                        if (!pRPyramid.Present)
                        {
                            pRPyramid.Create();
                        }
                    }
                    IRasterLayer pRasterLyr = new RasterLayerClass();
                    pRasterLyr.CreateFromRaster(pRaster);
                    this.mapControl.AddLayer(pRasterLyr as ILayer, 0);
                    this.mapControl.Extent = (pRasterLyr as ILayer).AreaOfInterest;
                }
            }
        }
              

        //坡度分析
        private void bi_raster_slope_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            raster.SlopeForm frm = new raster.SlopeForm();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                IRaster pRaster = frm.pOutRaster;    
                
                if (pRaster != null)
                {
                    IRaster2 pRaster2 = pRaster as IRaster2;
                    IRasterDataset pRDS = pRaster2.RasterDataset;
                    if (pRDS != null)
                    {
                        IRasterPyramid pRPyramid = pRDS as IRasterPyramid;
                        if (!pRPyramid.Present)
                        {
                            pRPyramid.Create();
                        }
                    }
                    IRasterLayer pRasterLyr = new RasterLayerClass();
                    pRasterLyr.CreateFromRaster(pRaster);
                    this.mapControl.AddLayer(pRasterLyr as ILayer, 0);
                    this.mapControl.Extent = (pRasterLyr as ILayer).AreaOfInterest;
                }
            }
        }
              

        #endregion 


        #region 基本农田
        private void bi_edit_jbnt_tq_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //重排标识码
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    
            //基本农田图斑提取
            if (this.mapControl.LayerCount == 0) return;
            zrzy.JBNTTBTqOptForm frm = new zrzy.JBNTTBTqOptForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, this.mapControl.ActiveView.Extent.Envelope);

        }

        private void bi_edit_jbnttb_setBh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //保护图斑编号赋值
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //图斑编号重计算
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            zrzy.JBNTTBBHSetValForm frm = new zrzy.JBNTTBBHSetValForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }
        //田坎系数赋值
        private void br_sxwh_tkxs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //保护图斑编号赋值
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //图斑编号重计算
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.TKXSWhForm frm = new datado.TKXSWhForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void br_sx_setGddb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.GddbSetValForm frm = new datado.GddbSetValForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }


        private void br_sx_pdjbsetvalue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
           
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.PDTSetValueFrm frm = new datado.PDTSetValueFrm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        #endregion 

        
        #region VCT导出
        private void br_vctout_allfw_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pXZQClass = null;
            string xzdm = "";
            try
            {
                pXZQClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            int currDh = 38;
            if (pXZQClass != null)
            {
                IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                if (firstFea != null)
                {
                    xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                    IPoint selectPoint = (firstFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号

                }
            }
            if (xzdm.Length > 6)
            {
                xzdm = xzdm.Substring(0, 6);
            }
            RCIS.DataInterface.VCTOut.VCTOutput3Frm frm = new RCIS.DataInterface.VCTOut.VCTOutput3Frm();
            frm.XianDM = xzdm;
            frm.DH = currDh;
            frm.PWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }
        private void br_vctout_selectFW_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (GlobalEditObject.GlobalWorkspace == null)
            //{
            //    MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

            //if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            //{
            //    MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //output.OutVCTFwSelOptForm frm = new output.OutVCTFwSelOptForm();
            //frm.currMap = this.mapControl.ActiveView.FocusMap;
            //frm.PWorkspace = GlobalEditObject.GlobalWorkspace;
            //frm.ShowDialog();
        }


        //地图缩编       
        private void br_edit_mapIntegrate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            edit.MapIntegrateForm frm = new edit.MapIntegrateForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem53_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            output.OutPutMDB frm = new output.OutPutMDB();
            frm.ShowDialog();
        }

        #endregion
        
        #region 数据分析

        private void barButtonItem47_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            analysis.TdlyjgfxForm frm = new analysis.TdlyjgfxForm();
            frm.ShowDialog();
        }

        private void bi_anay_xzjsyd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //指定不一致图斑 和地类图斑叠加对比，输出shp，列表，并统计得到一个 结果
            analysis.XzjsydfxForm frm = new analysis.XzjsydfxForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();


        }

        private void bi_anay_n2w_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            analysis.N2WFxForm frm = new analysis.N2WFxForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void bi_analy_gdnbbh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            analysis.GdNbBhForm frm = new analysis.GdNbBhForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

      

        private void barButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            analysis.KcdjfxForm frm = new analysis.KcdjfxForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }
        private void barButtonItem43_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增耕地
            analysis.XzgdFxForm frm = new analysis.XzgdFxForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem44_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            analysis.JsnbhFxForm frm = new analysis.JsnbhFxForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem45_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //耕地变田坎
            analysis.Gd2TKForm frm = new analysis.Gd2TKForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }
        private void barButtonItem46_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //田坎变耕地
            analysis.TK2GdFxForm frm = new analysis.TK2GdFxForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }


        //提取重点地类变化图斑
        private void barButtonItem50_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //提取重点地类变化图斑
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            edit.PickChangeDLTB frm = new edit.PickChangeDLTB();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.Map;
            frm.ShowDialog();

        }


        private void barButtonItem49_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //二调到三调
            analysis.GetLL2d23dForm frm = new analysis.GetLL2d23dForm();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();

        }
        #endregion 

        #region 数据更新
        private void br_gengxin_xzqGx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //行政区不记录历史
            gengxin.XzqGengxinForm frm = new gengxin.XzqGengxinForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }
        //更新历史
        private void br_gengxin_history_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //更新历史
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //图斑编号重计算
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            edit.DltbHistoryForm frm = new edit.DltbHistoryForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();

        }

        #endregion 


        #region 专项调查
        private void bi_edit_czcdydtq_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    

            if (this.mapControl.LayerCount == 0) return;
            zrzy.CzcdydTqForm frm = new zrzy.CzcdydTqForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private void bi_edit_getCzcdydFromDltb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

 
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    

            edit.GetCZCDYDByDltb3Form frm = new edit.GetCZCDYDByDltb3Form();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.pMapCtl = this.mapControl.Object as IMapControl2;
            frm.Show();
        }

      
        #endregion 

        #region 视图窗口
        private void btnImportED_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sys.QuickImportForm3 frm = new sys.QuickImportForm3();
            frm.DestinationWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void btnShowXZQ_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockpanelXZQ.Show();
        }

        private void btnShowTOC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockpanelLayers.Show();
        }

        private void btnXZQShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.btnXZQShow.Down)
            {
                this.dockpanelXZQ.Show();
            }
            else
            {
                this.dockpanelXZQ.Hide();
            }
        }

        private void btnTOCShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.btnTOCShow.Down)
            {
                this.dockpanelLayers.Show();
            }
            else
            {
                this.dockpanelLayers.Hide();
            }
        }

        private void dockpanelLayers_VisibilityChanged(object sender, DevExpress.XtraBars.Docking.VisibilityChangedEventArgs e)
        {
            if (this.dockpanelLayers.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            {
                if (!this.btnTOCShow.Down)
                {
                    this.btnTOCShow.Down = true;
                }
            }
            else if (this.dockpanelLayers.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Hidden)
            {
                if (this.btnTOCShow.Down)
                {
                    this.btnTOCShow.Down = false;
                }
            }
        }

        private void dockpanelXZQ_VisibilityChanged(object sender, DevExpress.XtraBars.Docking.VisibilityChangedEventArgs e)
        {
            if (this.dockpanelXZQ.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            {
                if (!this.btnXZQShow.Down)
                {
                    this.btnXZQShow.Down = true;
                }
            }
            else if (this.dockpanelXZQ.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Hidden)
            {
                if (this.btnXZQShow.Down)
                {
                    this.btnXZQShow.Down = false;
                }
            }
        }

        private void dockPanelResult_VisibilityChanged(object sender, DevExpress.XtraBars.Docking.VisibilityChangedEventArgs e)
        {
            if (this.dockPanelResult.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Visible)
            {
                if (!this.btnResultShow.Down)
                {
                    this.btnResultShow.Down = true;
                }
            }
            else if (this.dockPanelResult.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Hidden)
            {
                if (this.btnResultShow.Down)
                {
                    this.btnResultShow.Down = false;
                }
            }
        }

        private void btnResultShow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.btnResultShow.Down)
            {
                this.dockPanelResult.Show();
            }
            else
            {
                this.dockPanelResult.Hide();
            }
        }

        #endregion 


        #region  //图形批处理
        private void br_edittool_searchedge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //重排标识码
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            edit.SearchEdgeTbForm frm = new edit.SearchEdgeTbForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (gridViewResult.Columns.Count < frm.resultTab.Columns.Count)
                {
                    this.gridViewResult.Columns.AddVisible("POINTID");
                }
                else if (gridViewResult.Columns.Count > frm.resultTab.Columns.Count)
                {
                    DevExpress.XtraGrid.Columns.GridColumn gc = this.gridViewResult.Columns.ColumnByFieldName("POINTID");
                    this.gridViewResult.Columns.Remove(gc);
                }
                this.gridControlResult.DataSource = frm.resultTab;
                this.dockPanelResult.Show();
                this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
        }

        private void br_patch_buildZj_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //生成注记层内容
            datado.BuildZjForm frm = new datado.BuildZjForm();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

       
        private void br_edittool_jdmd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //重排标识码
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            datado.JdmdQueryForm frm = new datado.JdmdQueryForm();
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.currMapControl = this.mapControl.Object as IMapControl2;
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.Show();
        }

       

        private void gridControlResult_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridViewResult.SelectedRowsCount == 0)
                return;
            string layerName = this.gridViewResult.GetRowCellValue(this.gridViewResult.FocusedRowHandle, "LAYER").ToString();
            string oid = this.gridViewResult.GetRowCellValue(this.gridViewResult.FocusedRowHandle, "OID").ToString();
            int ioid = Convert.ToInt32(oid);
            int iPointID = -1;
            if (gridViewResult.Columns.ColumnByFieldName("POINTID") != null)
            {
                string pointID = this.gridViewResult.GetRowCellValue(this.gridViewResult.FocusedRowHandle, "POINTID").ToString();
                iPointID = int.Parse(pointID);
            }

            IFeatureClass pTempLine = null;
            string sTempFile = Application.StartupPath + @"\tempLine.shp";
            if (System.IO.File.Exists(sTempFile))
            {
                pTempLine = WorkspaceHelper2.GetShapefileFeatureClass(sTempFile);
            }

            //定位
            IFeatureLayer pLyr = LayerHelper.QueryLayerByModelName(this.mapControl.ActiveView.FocusMap, layerName);
            if (pLyr == null) return;
            try
            {
                IFeatureSelection pSelection = pLyr as IFeatureSelection;

                IFeatureClass pClass = pLyr.FeatureClass;
                if (pClass == null) return;
                if (ioid < 0) return;
                IFeature pFeature = pClass.GetFeature(ioid);
                if (pFeature != null)
                {
                    this.mapControl.Map.ClearSelection();
                    IGeometry pGeo = pFeature.ShapeCopy;
                    IPoint pPoint = null;
                    if (pGeo.GeometryType == esriGeometryType.esriGeometryPolygon && iPointID >= 0)
                    {
                        pPoint = (pGeo as IPointCollection).Point[iPointID];
                    }
                    IEnvelope env = pGeo.Envelope;
                    env.Expand(2, 2, true);
                    this.mapControl.ActiveView.Extent = env;
                  
                    this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapControl.ActiveView.Extent);
                    this.mapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    if (pGeo != null)
                    {
                        this.mapControl.FlashShape(pGeo, 3, 300, null);
                        mapControl.Map.SelectFeature(pLyr, pFeature);
                    }
                    if (pPoint != null)
                    {
                        this.mapControl.FlashShape(pPoint, 3, 300, null);
                    }
                    if (pTempLine != null)
                    {
                        IQueryFilter pQueryFilter = new QueryFilterClass();
                        pQueryFilter.WhereClause = "id = " + pFeature.OID;
                        IFeatureCursor pFeatureCursor = pTempLine.Search(pQueryFilter, true);
                        IFeature pF = pFeatureCursor.NextFeature();
                        if (pF != null)
                        {
                            this.mapControl.FlashShape(pF.ShapeCopy, 3, 300, null);
                        }
                    }
                    this.mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
            }
            catch (Exception cex)
            {
            }
        }


        #endregion 

        //通过变化图斑和三调基础图斑生成更新过程图层
        private void barButtonItem57_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmDLTBGXGC frm = new gengxin.FrmDLTBGXGC();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void btnBHTBSXCL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmBHTBSXPCL frm = new gengxin.FrmBHTBSXPCL();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //通过地类图斑更新过程层数据生成地类图斑更新层数据
        private void barButtonItem58_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmDLTBGX frm = new gengxin.FrmDLTBGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //行政区属性批量更新
        private void barButtonItem56_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmXZQSXPLGX frm = new gengxin.FrmXZQSXPLGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //根据变化行政区生成行政区更新过程图层
        private void barButtonItem60_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmXZQGXGC frm = new gengxin.FrmXZQGXGC();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //生成行政区更新图层
        private void barButtonItem61_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmXZQGX frm = new gengxin.FrmXZQGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //生成行政区界线更新图层
        private void barButtonItem63_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmXZQJXGX frm = new gengxin.FrmXZQJXGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //根据变化的行政区提取变化的创建调查区功能
        private void barButtonItem64_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmBHCJDCQ frm = new gengxin.FrmBHCJDCQ();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //生成村级调查区更新过程图层
        private void barButtonItem66_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmCJDCQGXGC frm = new gengxin.FrmCJDCQGXGC();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //生成村级调查区更新图层
        private void barButtonItem67_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmCJDCQGX frm = new gengxin.FrmCJDCQGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //生成村级调查区界线更新图层
        private void barButtonItem68_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmCJDCQJXGX frm = new gengxin.FrmCJDCQJXGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //根据变化的村级调查区提取变化地类图斑
        private void barButtonItem69_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmBHDLTB frm = new gengxin.FrmBHDLTB();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //村级调查区属性批处理
        private void barButtonItem65_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmCJDCQSXPCL frm = new gengxin.FrmCJDCQSXPCL();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        //变化图斑属性检查
        private void barButtonItem70_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmCheckData frm = new gengxin.FrmCheckData();
            frm.mapControl = mapControl;
            frm.Show();
        }

        private void barCreateEndDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmSCNMSJK frm = new gengxin.FrmSCNMSJK();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem59_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmSummary frm = new gengxin.FrmSummary();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem71_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gengxin.FrmReport frm = new gengxin.FrmReport();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void BGGXCTQ_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //edit.GetGXGCForm frm = new edit.GetGXGCForm();
            //frm.currWs = GlobalEditObject.GlobalWorkspace;
            //frm.ShowDialog();
            gengxin.FrmBGOpt frm = new gengxin.FrmBGOpt();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.currMap = mapControl.Map;
            frm.ShowDialog();
        }

        private void barButtonItem72_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //更新层BSM重建
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmBSMOpt frm = new gengxin.FrmBSMOpt();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem73_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //重排更新层图斑编号
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //gengxin.FrmTBBHOpt frm = new gengxin.FrmTBBHOpt();
            //frm.currWs = GlobalEditObject.GlobalWorkspace;
            //frm.ShowDialog();
            gengxin.FrmTBBHOpt2 frm = new gengxin.FrmTBBHOpt2();
            frm.currMap = this.mapControl.Map;
            frm.ShowDialog();
        }

        private void barButtonItem74_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导出统一时点更新VCT
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureClass pXZQClass = null;
            string xzdm = "";
            try
            {
                pXZQClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            int currDh = 38;
            if (pXZQClass != null)
            {
                IFeature firstFea = GetFeaturesHelper.GetFirstFeature(pXZQClass, null);
                if (firstFea != null)
                {
                    xzdm = FeatureHelper.GetFeatureStringValue(firstFea, "XZQDM");
                    IPoint selectPoint = (firstFea.ShapeCopy as IArea).Centroid;
                    double X = selectPoint.X;
                    currDh = (int)(X / 1000000);////WK---带号

                }
            }
            if (xzdm.Length > 6)
            {
                xzdm = xzdm.Substring(0, 6);
            }
            gengxin.FrmVCTOutput frm = new gengxin.FrmVCTOutput();
            frm.XianDM = xzdm;
            frm.DH = currDh;
            frm.PWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem75_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //提取村级调查区、行政区更新层
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmGetXzqGX frm = new gengxin.FrmGetXzqGX();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem76_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //导出成果包
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmOutputResult frm = new gengxin.FrmOutputResult();
            frm.currWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem77_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //根据空间关系进行属性集成的功能
            if (GlobalEditObject.GlobalWorkspace == null)
                return;
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmSpatialUpdate frm = new gengxin.FrmSpatialUpdate();
            frm.currWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void btnSplitTBGX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gengxin.FrmTBGXSplit frm = new gengxin.FrmTBGXSplit();
            frm.pCurrentWorkspace = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.ActiveView.FocusMap;
            //frm.currMap = this.mapControl.ActiveView.FocusMap;
            frm.ShowDialog();
        }

        private void barButtonItem78_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmDealAttribute frm = new gengxin.FrmDealAttribute();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.mapctl = this.mapControl.Object as IMapControl3;
            frm.Show(this);
        }

        private void barButtonItem79_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gengxin.FrmOutSXTJ m_LSOutForm = new gengxin.FrmOutSXTJ();
            m_LSOutForm.StartPosition = FormStartPosition.CenterScreen;
            m_LSOutForm.m_MapControl = this.mapControl;
            m_LSOutForm.m_myTab = this.xtraTabControl1;
            m_LSOutForm.m_PageControl = this.m_layoutControlPanel.LayoutControl;
            m_LSOutForm.currWS = GlobalEditObject.GlobalWorkspace;
            if (m_LSOutForm.Visible == false)
                m_LSOutForm.Show();
        }

        private void barButtonItem80_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmCheckGraphics frm = new gengxin.FrmCheckGraphics();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.mapctl = this.mapControl.Object as IMapControl3;
            frm.Show(this);
        }

        private void barButtonItem81_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //edit.GetGXGCForm frm = new edit.GetGXGCForm();
            //frm.currWs = GlobalEditObject.GlobalWorkspace;
            //frm.ShowDialog();
            gengxin.FrmGetChangeTB frm = new gengxin.FrmGetChangeTB();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            //frm.currMap = mapControl.Map;
            frm.ShowDialog();
        }

       

        private void barButtonItem82_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmBSM frm = new gengxin.FrmBSM();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem83_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdzy.FrmGetGDData frm = new gdzy.FrmGetGDData();
            frm.pCurrWs = GlobalEditObject.GlobalWorkspace;
            frm.currMap = this.mapControl.Map;
            frm.ShowDialog();
        }

        private void btnGDBSM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //更新层BSM重建
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdzy.FrmBSMGD frm = new gdzy.FrmBSMGD();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem84_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdzy.FrmDYBHGD frm = new gdzy.FrmDYBHGD();
            frm.pCurrWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem86_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdzy.FrmGetSpatialAttribute frm = new gdzy.FrmGetSpatialAttribute();
            frm.pCurrWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem87_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gdzy.FrmResultReport frm = new gdzy.FrmResultReport();
            frm.pCurrWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem88_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gdzy.FrmOutGDResult frm = new gdzy.FrmOutGDResult();
            frm.pCurrWs = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void btnFLYS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gdzy.FrmGetOtherByFLDY frm = new gdzy.FrmGetOtherByFLDY();
            frm._curWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void barButtonItem85_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            analysis.bbfxForm frm = new analysis.bbfxForm();
            frm.mapControl = this.mapControl;
            frm.ShowDialog();
        }

        private void btnDbLayer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (GlobalEditObject.GlobalWorkspace == null || string.IsNullOrEmpty(GlobalEditObject.GlobalWorkspace.PathName))
                {
                    MessageBox.Show("当前没有加载数据库。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    AddOpenFileLayers();
                }
            }
            catch
            {
                MessageBox.Show("打开数据库出错。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXZQTZLX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmXZQTZLX frm = new gengxin.FrmXZQTZLX();
            frm._CurMap = mapControl.Map;
            frm.ShowDialog();
        }

        private void barButtonItem89_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            output.FrmOutRsTable frm = new output.FrmOutRsTable();
            frm.pMap = mapControl.Map;
            frm.ShowDialog();
        }

        private void barButtonItem90_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            output.FrmOutTable frm = new output.FrmOutTable();
            frm.currWs = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void btnDLTBCZC_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmDLTBCZCSX frm = new gengxin.FrmDLTBCZCSX();
            frm._curWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm._CurMap = mapControl.Map;
            frm.ShowDialog();
        }

        private void barButtonItem91_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("当前没有加载任何数据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.FrmDataOut frm = new datado.FrmDataOut();
            frm._curWS = RCIS.Global.GlobalEditObject.GlobalWorkspace;
            frm._curMap = mapControl.Map;
            frm.ShowDialog();
        }

        private void btnAkey_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //提取村级调查区、行政区更新层
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            gengxin.FrmAKeyProcessing frm = new gengxin.FrmAKeyProcessing();
            frm.currWs = GlobalEditObject.GlobalWorkspace;
            frm.pMapCtl = this.mapControl.Object as IMapControl2;
            frm.ShowDialog();
        }

        private void btnCheckDLTBGX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //更新数据检查
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            datado.FrmCheckDLTBGX frm = new datado.FrmCheckDLTBGX();
            frm._curWS = GlobalEditObject.GlobalWorkspace;
            frm.ShowDialog();
        }

        private void btnExportTJJHB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (GlobalEditObject.GlobalWorkspace == null)
            {
                MessageBox.Show("请首先加载数据空间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                MessageBox.Show("请首先结束编辑状态。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            output.FrmExportKZMJJHB frm = new output.FrmExportKZMJJHB();
            frm.m_MapControl = this.mapControl;
            frm.m_myTab = this.xtraTabControl1;
            frm.m_PageControl = this.m_layoutControlPanel.LayoutControl;
            frm.ShowDialog();
        }
    }

}
