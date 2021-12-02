using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using sycCommonLib;
using System.Collections;
using System.IO;

using RCIS.Utility;
using RCIS.GISCommon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using RCIS.Global;
using DevExpress.XtraGrid;
using System.Runtime.InteropServices;

namespace TDDC3D.output
{
    public partial class LSOutForm2 : Form
    {
        public LSOutForm2()
        {
            InitializeComponent();
        }

        //变量
        public DevExpress.XtraTab.XtraTabControl m_myTab;

        public ESRI.ArcGIS.Controls.AxMapControl m_MapControl;
        public ESRI.ArcGIS.Controls.AxPageLayoutControl m_PageControl;
        private IMapFrame m_myMapFrame;

        /// <summary>
        /// 花边矩形
        /// </summary>
        private IPolygon pBKPolygon;


        public string m_XzqMC, m_sTFH;
        public string m_XzqDm;

        public IGeometry m_RealCtPolygon;
        public IGeometry m_bufferPolygon;

        public long iTH = -10000;

        public double pageMargein
        {
            get
            {
                try
                {
                    return Convert.ToDouble(this.txtMargin.Text);
                }
                catch
                {
                    return 50;
                }
            }
        }
        private IMapFrame GetIMapFrame()
        {
            IGraphicsContainer container = this.m_PageControl.GraphicsContainer;// .GraphicsContainer;
            container.Reset();

            IElement ele = container.Next();
            while (ele != null)
            {
                if (ele is IMapFrame)
                {
                    IElementProperties pProEle = ele as IElementProperties;
                    if (pProEle.Name != "XZQSLT")
                    {
                        return (IMapFrame)ele;
                    }
                }
                ele = container.Next();
            }

            return (IMapFrame)null;
        }


        private void LoadAllFont(DevExpress.XtraEditors.ComboBoxEdit cmb)
        {
            //如何获得系统字体列表 
            cmb.Properties.Items.Clear();
            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in fonts.Families)
            {
                cmb.Properties.Items.Add(family.Name);
            }

        }
        //注记字体初始化
        private void ZJSetup()
        {
            LoadAllFont(this.cboDLTBFont);
            LoadAllFont(this.cboXZQFont);
            LoadAllFont(this.cboTBXHMCFont);
            LoadAllFont(this.cboXMMCFont);
            LoadAllFont(this.cboCZCDMFont);

            this.ceDLTB.EditValue = System.Drawing.Color.Black;
            this.cboDLTBFont.Text = "方正细等线简体";
            this.cboDLTBSize.Text = "8.5";

            this.ceTBXHColor.EditValue = System.Drawing.Color.Black; //图斑细化名称
            this.cboTBXHMCFont.Text = "方正细等线简体";
            this.cboTBXHMCFontSize.Text = "8.5";

            this.ceTBXHColor.EditValue = System.Drawing.Color.Black; //项目名称
            this.cboXMMCFont.Text = "黑体";
            this.cboXMMCFontsize.Text = "14";

            this.ceCZCDMColor.EditValue = System.Drawing.Color.Red;
            this.cboCZCDMFont.Text = "黑体";
            this.cboCZCDMFontSize.Text = "12";

            this.ceXZQ.EditValue = System.Drawing.Color.Black;
            this.cboXZQFont.Text = "黑体";
            this.cboXZQSize.EditValue = "25";

        }

        private void LSOutForm2_Load(object sender, EventArgs e)
        {
            ZJSetup();

            this.xzqLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "XZQ");
            this.dltbLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "DLTB");
            this.pzwjstdLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "PZWJSTD");
            this.czcdydLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "CZCDYD");
            this.tfhLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "TFH");

            //3-29 日增加 村级行政区
            this.cjdcqLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "CJDCQ");

            //初始化图层范围选择框
            gluExtent.Properties.NullText = "输入关键字查询";//空时的值
            this.gluExtent.Properties.AutoComplete = false;
            this.gluExtent.Properties.ImmediatePopup = true;
            this.gluExtent.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.gluExtent.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.gluExtent.Properties.ShowFooter = false;

            AddDeleteButton(gluExtent, "");
            FilterLookup(gluExtent.Properties);

            cmbCTFW.SelectedIndex = 4;   //设置出图类型为任意区域
            cmbOutType.SelectedIndex = 0;

        }

        /// <summary>
        /// 设置gridlookupedit多列模糊查询
        /// </summary>
        /// <param name="repGLUEdit"></param>
        private void FilterLookup(DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repGLUEdit)
        {
            repGLUEdit.EditValueChanging += (sender, e) =>
            {
                this.BeginInvoke(new System.Windows.Forms.MethodInvoker(() =>
                {
                    GridLookUpEdit edit = sender as GridLookUpEdit;
                    DevExpress.XtraGrid.Views.Grid.GridView view = edit.Properties.View as DevExpress.XtraGrid.Views.Grid.GridView;
                    //获取GriView私有变量
                    System.Reflection.FieldInfo extraFilter = view.GetType().GetField("extraFilter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    List<DevExpress.Data.Filtering.CriteriaOperator> columnsOperators = new List<DevExpress.Data.Filtering.CriteriaOperator>();
                    foreach (GridColumn col in view.VisibleColumns)
                    {
                        if (col.Visible && col.ColumnType == typeof(string))
                            columnsOperators.Add(new DevExpress.Data.Filtering.FunctionOperator(DevExpress.Data.Filtering.FunctionOperatorType.Contains,
                                new DevExpress.Data.Filtering.OperandProperty(col.FieldName),
                                new DevExpress.Data.Filtering.OperandValue(edit.Text)));
                    }
                    string filterCondition = new DevExpress.Data.Filtering.GroupOperator(DevExpress.Data.Filtering.GroupOperatorType.Or, columnsOperators).ToString();
                    extraFilter.SetValue(view, filterCondition);
                    //获取GriView中处理列过滤的私有方法
                    System.Reflection.MethodInfo ApplyColumnsFilterEx = view.GetType().GetMethod("ApplyColumnsFilterEx", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    ApplyColumnsFilterEx.Invoke(view, null);
                }));
            };

        }

        /// <summary>
        /// 给选择区域框添加删除按钮
        /// </summary>
        /// <param name="lue"></param>
        /// <param name="prompttext">提醒语句</param>
        public static void AddDeleteButton(GridLookUpEdit lue, string prompttext)
        {
            prompttext = string.IsNullOrEmpty(prompttext) ? "删除选中项" : prompttext;
            lue.Properties.Buttons.AddRange(new EditorButton[]
            {
                new EditorButton(
                    ButtonPredefines.Delete,
                    "删除", -1, true, true, false, ImageLocation.MiddleCenter,
                    null,
                    new KeyShortcut(Keys.Delete),
                    new SerializableAppearanceObject(),
                    prompttext,
                    "Delete",
                    null,
                    true)
            });
            lue.ButtonClick += new ButtonPressedEventHandler(lue_ButtonClick);
        }

        /// <summary>
        /// 删除gridLookUpEdit中选择的内容
        /// </summary>
        static void lue_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                GridLookUpEdit _curLue = sender as GridLookUpEdit;
                _curLue.EditValue = null;

            }
        }
        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ICommand myTool = new ControlsMapPanToolClass();
            myTool.OnCreate(this.m_MapControl.Object);
            m_MapControl.CurrentTool = myTool as ITool;
            m_RealCtPolygon = null;
            m_bufferPolygon = null;

            //...
            IGraphicsContainer mapCon = m_MapControl.ActiveView.GraphicsContainer;
            deleteAllElement(mapCon);

            IActiveView act = m_MapControl.ActiveView;
            act.Refresh();
            //...
            Close();
        }
        public class FLW
        {
            public FLW() { }

            public string sBS;
            public string sZJ1, sZJ2;
            public IPoint PP1, PP2;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //点选待输出的元素:
            double dMMJL = 50.0;
            string sFW = cmbCTFW.SelectedItem.ToString();
            if (sFW.Equals("标准分幅") == false)
            {
                try
                {
                    dMMJL = Convert.ToDouble(this.textBoxTKJL.Text.Trim());
                }
                catch (Exception E)
                {
                    MessageBox.Show(" 参数错误: " + E.Message + " !");
                    return;
                }
            }
            string sScale = cmbCTBLC.SelectedItem.ToString();
            this.Visible = false;

            LSOutTool2 MyTool = new LSOutTool2();
            MyTool.OnCreate(this.m_MapControl.Object);
            MyTool.m_UseForm = this;
            MyTool.m_sFW = sFW;
            MyTool.m_sScale = sScale;
            MyTool.m_dMMJL = dMMJL;
            m_MapControl.CurrentTool = MyTool;
        }
        # region 注记
        private void SymbolizeDLTBZJ(IGraphicsContainer pGc, IFeatureLayer dltbLyr, IGeometry pGeo)
        {
            // IArea area = pGeo as IArea;

            double dltbMjRx = 0.0;
            try
            {
                dltbMjRx = double.Parse(txtDltbMjRX.Text);
            }
            catch { }

            //IFeatureLayer pLay = m_PageControl.ActiveView.FocusMap.get_Layer(0) as IFeatureLayer;
            //IIdentify dltbIndentify = pLay as IIdentify;
            IIdentify dltbIndentify = dltbLyr as IIdentify;
            IArray arrDltbIDs = dltbIndentify.Identify(pGeo);
            ITopologicalOperator pTop = pGeo as ITopologicalOperator;
            pTop.Simplify();
            for (int i = 0; i < arrDltbIDs.Count; i++)
            {
                IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                IFeature pfea = pRow.Row as IFeature;
                IGeometry pG = pfea.ShapeCopy;
                ((ITopologicalOperator)pG).Simplify();
                if (pGeo.SpatialReference != pG.SpatialReference) pGeo.Project(pG.SpatialReference);
                IGeometry pIntersctGeo = pTop.Intersect(pG, esriGeometryDimension.esriGeometry2Dimension);// (pGeo as ITopologicalOperator).Intersect(pfea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                (pIntersctGeo as ITopologicalOperator).Simplify();
                if ((pfea.Shape as IArea).Area > dltbMjRx && !pIntersctGeo.IsEmpty)
                {

                    IPoint ppoint = (pIntersctGeo as IArea).LabelPoint;
                    //if (ppoint.X < pGeo.Envelope.XMax && ppoint.X > pGeo.Envelope.XMin && ppoint.Y < pGeo.Envelope.YMax && ppoint.Y > pGeo.Envelope.YMin)
                    //{
                    //IPoint ppoint = (pfea.ShapeCopy as IArea).LabelPoint;
                    int nX = 0, nY = 0;
                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sTBBH = FeatureHelper.GetFeatureStringValue(pfea, "TBBH");
                    string sQsxz = FeatureHelper.GetFeatureStringValue(pfea, "QSXZ");
                    string sDL = FeatureHelper.GetFeatureStringValue(pfea, "DLBM");


                    if (sQsxz.StartsWith("1") || sQsxz.StartsWith("2"))
                    {
                        sQsxz = "G";
                    }
                    else
                    {
                        sQsxz = "";
                    }


                    //IPoint pPT =  GetPoint(curZJP, 0.25, 0.15);

                    #region //分子:
                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboDLTBFont.Text, 1, FontStyle.Underline);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboDLTBSize.Text.Trim());
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    textSymbol.Color = ColorHelper.CreateColor(this.ceDLTB.Color);
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sTBBH + sQsxz;
                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                    #endregion
                    #region  //分母
                    dotNetFont = new System.Drawing.Font(this.cboDLTBFont.Text, 1);
                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboDLTBSize.Text.Trim());
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textSymbol.Color = ColorHelper.CreateColor(this.ceDLTB.Color);
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sDL;
                    element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                    #endregion
                    if (i > 0 && i % 10000 == 0)
                        m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    //}
                }
            }
        }

        ////线状地物注记
        //private void SymbolizeXZDWZJ(IGraphicsContainer pGc, IFeatureLayer xzdwLyr, IGeometry pGeo)
        //{
        //    double dXzdlCdRx = 0.0;
        //    try
        //    {
        //        dXzdlCdRx = double.Parse(txtXZDWCDRX.Text);
        //    }
        //    catch { }

        //    IIdentify xzdwIdentify = xzdwLyr as IIdentify;
        //    IArray arXzdwIDs = xzdwIdentify.Identify(pGeo);
        //    for (int i = 0; i < arXzdwIDs.Count; i++)
        //    {
        //        IFeatureIdentifyObj identifyObj = arXzdwIDs.get_Element(i) as IFeatureIdentifyObj;
        //        IRowIdentifyObject pRow = identifyObj as IRowIdentifyObject;
        //        IFeature pfea = pRow.Row as IFeature;

        //        if ((pfea.ShapeCopy as IPolyline).Length > dXzdlCdRx) //大于一定长度的才显示宽度标记
        //        {
        //            IPoint pPoint = new PointClass();
        //            double lineLength = (pfea.ShapeCopy as IPolyline).Length / 2; //显示在中间部分

        //            (pfea.ShapeCopy as IPolyline).QueryPoint(esriSegmentExtension.esriExtendAtFrom, lineLength, false, pPoint); //获取中间点

        //            int nX = 0, nY = 0;
        //            (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(pPoint, out nX, out nY);

        //            nX += 2;

        //            IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
        //            string sText = FeatureHelper.GetFeatureStringValue(pfea, "KD"); //宽度标注

        //            System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXZDWFont.Text, 1);
        //            ITextSymbol textSymbol = new TextSymbolClass();
        //            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

        //            textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXZDWSize.Text);
        //            textSymbol.Color = ColorHelper.CreateColor(this.ceXZDW.Color);
        //            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
        //            TextElementClass textEle = new TextElementClass();
        //            textEle.Symbol = textSymbol;
        //            textEle.Text = sText;
        //            IElement element = (IElement)textEle;
        //            element.Geometry = curZJP;
        //            pGc.AddElement(element, 0);
        //            if (i > 0 && i % 10000 == 0)
        //                m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        //        }


        //    }


        //}

        //行政区注记
        private void SymbolizeXZQZJ(IGraphicsContainer pGc, IFeatureLayer xzqLyr, IGeometry pGeo)
        {

            IIdentify xzqId = xzqLyr as IIdentify;

            IArray arXZQIDS = xzqId.Identify(pGeo);
            if (arXZQIDS == null)
                return;

            try
            {
                for (int i = 0; i < arXZQIDS.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arXZQIDS.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    IGeometry pXZQGeo = pfea.ShapeCopy;
                    IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(pXZQGeo, esriGeometryDimension.esriGeometry2Dimension);
                    (pInterGeo as ITopologicalOperator).Simplify();
                    if (pInterGeo.IsEmpty)
                    {
                        continue;
                    }
                    //(pInterGeo as IArea).LabelPoint;
                    IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                    //IPoint ppoint = (pXZQGeo as IArea).LabelPoint;  
                    int nX = 0, nY = 0;

                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sText = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");

                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXZQFont.Text, 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXZQSize.Text);
                    textSymbol.Color = ColorHelper.CreateColor(this.ceXZQ.Color);
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sText;

                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                }
            }
            catch (Exception ex)
            { }



        }
        //批准未建设项目名称注记
        private void SymbolizePzwjstdZJ(IGraphicsContainer pGc, IFeatureLayer pzwjstdLyr, IGeometry pGeo)
        {
            IIdentify pzwjsIdentify = pzwjstdLyr as IIdentify;

            IArray arPzwjstdIds = pzwjsIdentify.Identify(pGeo);
            if (arPzwjstdIds == null)
                return;

            try
            {
                for (int i = 0; i < arPzwjstdIds.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arPzwjstdIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    IGeometry APzwjsGeo = pfea.ShapeCopy;
                    IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (pInterGeo.IsEmpty)
                    {
                        continue;
                    }
                    IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                    int nX = 0, nY = 0;

                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sText = FeatureHelper.GetFeatureStringValue(pfea, "BZXMMC");

                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXMMCFont.Text, 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXMMCFontsize.Text);
                    textSymbol.Color = ColorHelper.CreateColor(this.ceXMMCColor.Color);
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sText;

                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                }
            }
            catch (Exception ex)
            { }
        }

        private void SymbolizeCZCDMZJ(IGraphicsContainer pGc, IFeatureLayer czcdmLayer, IGeometry pGeo)
        {
            IIdentify czcdydIdentify = czcdmLayer as IIdentify;

            IArray arCzcdmIds = czcdydIdentify.Identify(pGeo);
            if (arCzcdmIds == null)
                return;

            try
            {
                for (int i = 0; i < arCzcdmIds.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arCzcdmIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    IGeometry APzwjsGeo = pfea.ShapeCopy;
                    IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (pInterGeo.IsEmpty)
                    {
                        continue;
                    }
                    IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                    int nX = 0, nY = 0;

                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sText = FeatureHelper.GetFeatureStringValue(pfea, "CZCLX");

                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboCZCDMFont.Text, 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboCZCDMFontSize.Text);
                    textSymbol.Color = ColorHelper.CreateColor(this.ceCZCDMColor.Color);
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sText;

                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                }
            }
            catch (Exception ex)
            { }
        }

        //细化名称
        private void SymbolizeTBXHMCZJ(IGraphicsContainer pGc, IFeatureLayer dltbLyr, IGeometry pGeo)
        {
            IIdentify dltbIdentify = dltbLyr as IIdentify;
            IArray arDltbIds = dltbIdentify.Identify(pGeo);
            if (arDltbIds == null)
                return;

            try
            {
                for (int i = 0; i < arDltbIds.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arDltbIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    IGeometry APzwjsGeo = pfea.ShapeCopy;
                    IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (pInterGeo.IsEmpty)
                    {
                        continue;
                    }
                    IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                    ppoint.PutCoords(ppoint.X + 5, ppoint.Y + 5);
                    int nX = 0, nY = 0;

                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sText = FeatureHelper.GetFeatureStringValue(pfea, "TBXHMC");

                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboTBXHMCFont.Text, 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboTBXHMCFontSize.Text);
                    textSymbol.Color = ColorHelper.CreateColor(this.ceTBXHColor.Color);
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sText;

                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        private void deleteAllElement(IGraphicsContainer pageCon)
        {
            #region 删除原来的element
            pageCon.Reset();
            IElement ele = pageCon.Next();
            int nGS = 0;
            while (ele != null)
            {
                nGS++;
                ele = pageCon.Next();
            }

            object[] eleArr = new object[nGS];
            pageCon.Reset();
            ele = pageCon.Next();
            nGS = 0;
            while (ele != null)
            {
                eleArr[nGS] = ele;
                nGS++;
                ele = pageCon.Next();
            }
            for (int i = 0; i < nGS; i++)
            {
                if (eleArr[i] is IMapFrame)
                {
                    IElement pTempELe = eleArr[i] as IElement;
                    IElementProperties pProEle = pTempELe as IElementProperties;
                    if (pProEle.Name == "XZQSLT")
                    {
                        pageCon.DeleteElement((IElement)eleArr[i]);
                    }
                }
                else
                {
                    pageCon.DeleteElement((IElement)eleArr[i]);
                }
            }
            #endregion
        }

        public double m_dJ1, m_dW1, m_dJ3, m_dW3;

        IFeatureLayer dltbLayer = null;
        IFeatureLayer xzqLayer = null;
        IFeatureLayer pzwjstdLayer = null;
        IFeatureLayer czcdydLayer = null;
        IFeatureLayer tfhLayer = null; //tfh图层

        IFeatureLayer cjdcqLayer = null; //村级行政区

        /// <summary>
        /// 判断要素位置是否重复
        /// </summary>
        /// <returns>true-重复，false-不重复</returns>
        private bool IsSameLocation()
        {
            List<string> lstSoucre = new List<string>();
            lstSoucre.Add(cboXZQLocation.SelectedItem.ToString());
            lstSoucre.Add(cboZBZLocation.SelectedItem.ToString());
            lstSoucre.Add(cboTLLocation.SelectedItem.ToString());
            lstSoucre.Add(cboHZBLocation.SelectedItem.ToString());

            List<string> simpleSource = new List<string>();
            foreach (string item in lstSoucre)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                if (!simpleSource.Contains(item))
                {
                    simpleSource.Add(item);
                }
                else
                {
                    return true;
                }
            }
            return false;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            sycCommonFuns CommonClassDLL = new sycCommonFuns();

            //出图范围
            if (m_bufferPolygon == null)
            {
                MessageBox.Show("在Map地图上、点选了要出图的范围了？");
                return;
            }
            List<string> lstXZQCT = new List<string>() { "村图", "乡图", "县图" };
            List<string> lstOtherCT = new List<string>() { "标准分幅", "任意区域" };
            if (IsSameLocation() && lstXZQCT.Contains(cmbCTFW.SelectedItem.ToString()))
            {
                MessageBox.Show("要素位置重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sFW = cmbCTFW.SelectedItem.ToString();		//标准分幅，村图，乡图，县图
            string sScale = cmbCTBLC.SelectedItem.ToString();
            double dScale = Convert.ToDouble(sScale);

            WaitDialogForm wait = new WaitDialogForm("正在出图，请稍候...", "请等待...");
            wait.Show();

            try
            {
                IMap myMap = this.m_MapControl.ActiveView.FocusMap;

                this.m_MapControl.Map.ClearSelection();
                this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                this.m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.m_MapControl.ActiveView.Extent);
                IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
                deleteAllElement(pageCon);

                m_PageControl.ActiveView.Activate(m_PageControl.hWnd);
                m_PageControl.ActiveView.FocusMap.MapScale = dScale;
                IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
                IActiveView pageAct = m_PageControl.ActiveView;
                IPointCollection pCol = m_bufferPolygon as IPointCollection;

                m_PageControl.ActiveView.Extent = m_bufferPolygon.Envelope;  //确定当前区域
                m_myMapFrame = this.GetIMapFrame();

                wait.SetCaption("正在生成外框...");
                OutOutFrame(dScale);

                wait.SetCaption("正在生成标注信息...");
                SetSymboZJ();

                IPoint NKP1 = new PointClass();
                IPoint NKP2 = new PointClass();
                IPoint NKP3 = new PointClass();
                IPoint NKP4 = new PointClass();
                if (sFW.Equals("标准分幅") == true)
                {
                    #region 标准图幅
                    IEnvelope pEnv = m_bufferPolygon.Envelope;
                    NKP1.PutCoords(pEnv.XMin, pEnv.YMin);
                    NKP2.PutCoords(pEnv.XMax, pEnv.YMin);
                    NKP3.PutCoords(pEnv.XMax, pEnv.YMax);
                    NKP4.PutCoords(pEnv.XMin, pEnv.YMax);

                    int nX = 0, nY = 0;
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP1, out nX, out nY);
                    IPoint newP1 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP1.PutCoords(newP1.X, newP1.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP2, out nX, out nY);
                    IPoint newP2 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP2.PutCoords(newP2.X, newP2.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP3, out nX, out nY);
                    IPoint newP3 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP3.PutCoords(newP3.X, newP3.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP4, out nX, out nY);
                    IPoint newP4 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP4.PutCoords(newP4.X, newP4.Y);
                    #endregion
                }
                else
                {
                    #region
                    IPoint NKP11 = new PointClass();
                    IPoint NKP22 = new PointClass();
                    IPoint NKP33 = new PointClass();
                    IPoint NKP44 = new PointClass();

                    NKP1 = m_bufferPolygon.Envelope.LowerLeft;
                    NKP2 = m_bufferPolygon.Envelope.LowerRight;
                    NKP3 = m_bufferPolygon.Envelope.UpperRight;
                    NKP4 = m_bufferPolygon.Envelope.UpperLeft;

                    int nX = 0, nY = 0;
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP1, out nX, out nY);
                    IPoint newP1 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP1.PutCoords(newP1.X, newP1.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP2, out nX, out nY);
                    IPoint newP2 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP2.PutCoords(newP2.X, newP2.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP3, out nX, out nY);
                    IPoint newP3 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP3.PutCoords(newP3.X, newP3.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP4, out nX, out nY);
                    IPoint newP4 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP4.PutCoords(newP4.X, newP4.Y);
                    #endregion
                }

                #region 内框:
                wait.SetCaption("正在生成内框及经纬网信息...");

                PolylineClass NewPol = new PolylineClass();
                pCol = (IPointCollection)NewPol;
                object Missing = Type.Missing;
                pCol.AddPoint(NKP1, ref Missing, ref Missing);
                pCol.AddPoint(NKP2, ref Missing, ref Missing);
                pCol.AddPoint(NKP3, ref Missing, ref Missing);
                pCol.AddPoint(NKP4, ref Missing, ref Missing);
                pCol.AddPoint(NKP1, ref Missing, ref Missing);

                SimpleLineSymbolClass lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.2;
                IColor eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;
                LineElementClass LineEle = new LineElementClass();
                LineEle.Geometry = (IGeometry)NewPol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                ILine pLine = new LineClass();

                pLine.FromPoint = NKP1;
                pLine.ToPoint = NKP2;
                PointClass tmpP1 = new PointClass();
                ((IConstructPoint)tmpP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                PointClass tmpP2 = new PointClass();
                double dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP2);
                ((IConstructPoint)tmpP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

                pLine.FromPoint = NKP4;
                pLine.ToPoint = NKP3;
                PointClass tmpP4 = new PointClass();
                ((IConstructPoint)tmpP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                PointClass tmpP3 = new PointClass();
                dLen = CommonClassDLL.syc_CalLength(ref NKP3, ref NKP4);
                ((IConstructPoint)tmpP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

                pLine.FromPoint = NKP1;
                pLine.ToPoint = NKP4;
                PointClass tmpPP1 = new PointClass();
                ((IConstructPoint)tmpPP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                PointClass tmpPP4 = new PointClass();
                dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP4);
                ((IConstructPoint)tmpPP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

                pLine.FromPoint = NKP2;
                pLine.ToPoint = NKP3;
                PointClass tmpPP2 = new PointClass();
                ((IConstructPoint)tmpPP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                PointClass tmpPP3 = new PointClass();
                dLen = CommonClassDLL.syc_CalLength(ref NKP2, ref NKP3);
                ((IConstructPoint)tmpPP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);


                double dAF1 = CommonClassDLL.syc_CalAngle(ref tmpP4, ref tmpP1);
                double dAF2 = CommonClassDLL.syc_CalAngle(ref tmpPP2, ref tmpPP1);
                IPoint WKP1 = new PointClass();
                ((IConstructPoint)WKP1).ConstructAngleIntersection(tmpP1, dAF1, tmpPP1, dAF2);

                dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP1, ref tmpPP2);
                dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP3, ref tmpP2);
                IPoint WKP2 = new PointClass();
                ((IConstructPoint)WKP2).ConstructAngleIntersection(tmpPP2, dAF1, tmpP2, dAF2);

                dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP4, ref tmpPP3);
                dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP2, ref tmpP3);
                IPoint WKP3 = new PointClass();
                ((IConstructPoint)WKP3).ConstructAngleIntersection(tmpPP3, dAF1, tmpP3, dAF2);

                dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP3, ref tmpPP4);
                dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP1, ref tmpP4);


                IPoint WKP4 = new PointClass();
                ((IConstructPoint)WKP4).ConstructAngleIntersection(tmpPP4, dAF1, tmpP4, dAF2);

                NewPol = new PolylineClass();
                pCol = (IPointCollection)NewPol;
                Missing = Type.Missing;
                pCol.AddPoint(WKP1, ref Missing, ref Missing);
                pCol.AddPoint(WKP2, ref Missing, ref Missing);
                pCol.AddPoint(WKP3, ref Missing, ref Missing);
                pCol.AddPoint(WKP4, ref Missing, ref Missing);
                pCol.AddPoint(WKP1, ref Missing, ref Missing);

                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.2;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;
                LineEle = new LineElementClass();
                LineEle.Geometry = (IGeometry)NewPol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                Ring ring = new RingClass();
                for (int i = 0; i < pCol.PointCount; i++)
                {
                    ring.AddPoint(pCol.Point[i], Type.Missing, Type.Missing);
                }
                IGeometryCollection pointPolygon = new PolygonClass();
                pointPolygon.AddGeometry(ring as IGeometry, Type.Missing, Type.Missing);

                IPolygon pNKGeo = pointPolygon as IPolygon;
                #endregion

                //行政区图创建花边
                if (lstXZQCT.Contains(sFW))
                {
                    CreateNeatline(pNKGeo);
                }
                #region 经纬度
                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP1, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpP1, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dW1.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = NKP1;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = NKP1;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP1, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpPP1, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dJ1.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = tmpPP1;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = tmpPP1;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP2, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpP2, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dW1.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = NKP2;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = NKP2;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP2, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpPP2, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dJ3.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = tmpPP2;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = tmpPP2;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP3, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpP3, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dW3.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = NKP3;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = NKP3;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP3, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpPP3, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dJ3.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = tmpPP3;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = tmpPP3;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP4, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpP4, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dW3.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = NKP4;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = NKP4;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    object o = Type.Missing;
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(NKP4, ref o, ref o);
                    ((IPointCollection)pol).AddPoint(tmpPP4, ref o, ref o);
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    string sJWD = m_dJ1.ToString("F10");
                    int nPos = sJWD.IndexOf(".");
                    string sD = sJWD.Substring(0, nPos);
                    string sF = sJWD.Substring(nPos + 1, 2);
                    string sM = sJWD.Substring(nPos + 3, 6);
                    double dM = Convert.ToDouble(sM) / 10000.0;
                    if (Math.Abs(dM - 60.0) < 0.1)
                    {
                        sM = "00";
                        int nF = Convert.ToInt32(sF) + 1;
                        int nD = Convert.ToInt32(sD);
                        if (Math.Abs(nF - 60) < 0.01)
                            nD = nD + 1;
                        sF = nF.ToString("D02");
                        sD = nD.ToString();
                    }
                    else
                    {
                        sM = sM.Substring(0, 2);
                    }
                    sD = sD + "°";
                    sF = sF + "'";
                    sM = sM + "\"";

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sD;
                    IElement element = (IElement)textEle;
                    element.Geometry = tmpPP4;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.20;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sF + sM;
                    element = (IElement)textEle;
                    element.Geometry = tmpPP4;
                    pageCon.AddElement(element, 0);
                }

                #endregion

                #region 注记、标题、比例尺
                IPoint pBTPoint = new PointClass();
                IPoint pZXJPoint = new PointClass();
                IPoint pYXJPoint = new PointClass();
                if (lstXZQCT.Contains(sFW))
                {
                    pBTPoint.X = (pBKPolygon.Envelope.XMin + pBKPolygon.Envelope.XMax) * 0.5;
                    pBTPoint.Y = pBKPolygon.Envelope.YMax;
                    pZXJPoint.X = pBKPolygon.Envelope.LowerLeft.X;
                    pZXJPoint.Y = pBKPolygon.Envelope.LowerLeft.Y;
                    pYXJPoint.X = pBKPolygon.Envelope.LowerRight.X;
                    pYXJPoint.Y = pBKPolygon.Envelope.LowerRight.Y;
                }
                else if (lstOtherCT.Contains(sFW))
                {
                    pBTPoint.X = (WKP3.X + WKP4.X) * 0.5;
                    pBTPoint.Y = (WKP3.Y + WKP4.Y) * 0.5;
                    pZXJPoint.X = WKP1.X;
                    pZXJPoint.Y = WKP1.Y;
                    pYXJPoint.X = WKP2.X;
                    pYXJPoint.Y = WKP2.Y;
                    //pZXJPoint.X = NKP1.X;
                    //pZXJPoint.Y = NKP1.Y;
                    //pYXJPoint.X = NKP2.X;
                    //pYXJPoint.Y = NKP2.Y;
                }
                string sBT = "";
                if (cmbCTFW.SelectedItem.ToString().Equals("标准分幅") == true)
                {
                    sBT = this.memoBTZJ.Text + "\r\n" + m_sTFH;
                    AddZJTitle(WKP3, WKP4, pBTPoint, pZXJPoint, pYXJPoint, sBT,true);
                }
                else
                {
                    sBT = this.memoBTZJ.Text + "\r\n" + m_XzqMC + "\r\n" + m_XzqDm;
                    AddZJTitle(WKP3, WKP4, pBTPoint, pZXJPoint, pYXJPoint, sBT);
                }
                    
                //添加比例尺
                IPoint pScalePoint = new PointClass();
                if (lstXZQCT.Contains(sFW))
                {
                    pScalePoint.X = (pBKPolygon.Envelope.XMin + pBKPolygon.Envelope.XMax) * 0.5;
                    pScalePoint.Y = pBKPolygon.Envelope.YMin - 10;
                }
                else if (lstOtherCT.Contains(sFW))
                {
                    pScalePoint.X = (WKP1.X + WKP2.X) * 0.5;
                    pScalePoint.Y = (WKP1.Y + WKP2.Y) * 0.5 - 10;
                }
                AddScale(WKP3, WKP4, sScale, pScalePoint);
                #endregion

                #region 添加分幅图四周的行政区名称
                if (sFW.Equals("标准分幅") == true)
                {
                    int n = 0;//编号变量
                    List<TFPoint> tfPoints = new List<TFPoint>();//存放图幅边界与行政区相交的点，包括编号、点、行政区级别（乡、村）、位置（上、下、左、右）、行政区名称等
                    //List<string> xy = new List<string>();//存放所有的点位置，用于判断点是否重复、点距离
                    //上边框
                    IPointCollection line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperLeft);
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperRight);
                    IGeometry upGeo = (line as ITopologicalOperator).Buffer(10);
                    //下边框
                    line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerLeft);
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerRight);
                    IGeometry downGeo = (line as ITopologicalOperator).Buffer(10);
                    //左边框
                    line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerLeft);
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperLeft);
                    IGeometry leftGeo = (line as ITopologicalOperator).Buffer(10);
                    //右边框
                    line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperRight);
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerRight);
                    IGeometry rightGeo = (line as ITopologicalOperator).Buffer(10);

                    ////添加行政区的相交点
                    //IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    //pFeatureLayer.FeatureClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
                    //IIdentify pIdentify = pFeatureLayer as IIdentify;
                    //IArray pArray = pIdentify.Identify(m_RealCtPolygon);
                    //for (int i = 0; i < pArray.Count; i++)
                    //{
                    //    IFeatureIdentifyObj pFIObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                    //    IRowIdentifyObject pRIObj = pFIObj as IRowIdentifyObject;
                    //    IFeature pFeature = pRIObj.Row as IFeature;
                    //    IPointCollection pPointCollection = (m_RealCtPolygon as ITopologicalOperator).Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
                    //    for (int j = 0; j < pPointCollection.PointCount; j++)
                    //    {
                    //        IPoint pPoint = pPointCollection.Point[j];
                    //        TFPoint tfPoint = new TFPoint();
                    //        tfPoint.No = n++;
                    //        tfPoint.labelPoint = pPoint;
                    //        tfPoint.xzqLevel = "乡";
                    //        tfPoint.directPosition = getPosition(upGeo, downGeo, leftGeo, rightGeo, pPoint);
                    //        tfPoint.xzqMC = FeatureHelper.GetFeatureStringValue(pFeature, "XZQMC");
                    //        if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                    //        {
                    //            IPoint newPoint = new PointClass();
                    //            newPoint.PutCoords(pPoint.X - 2, pPoint.Y);
                    //            if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                    //            {
                    //                tfPoint.direct = -1;
                    //            }
                    //            else
                    //            {
                    //                tfPoint.direct = 1;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            IPoint newPoint = new PointClass();
                    //            newPoint.PutCoords(pPoint.X, pPoint.Y - 2);
                    //            if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                    //            {
                    //                tfPoint.direct = -1;
                    //            }
                    //            else
                    //            {
                    //                tfPoint.direct = 1;
                    //            }
                    //        }
                    //        Boolean b = getDistance(tfPoint, tfPoints);
                    //        if (b) tfPoints.Add(tfPoint);
                    //        xy.Add(Math.Round(pPoint.X, 2) + "," + Math.Round(pPoint.Y, 2));
                    //    }
                    //}
                    //添加村级调查区的相交点
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
                    IIdentify pIdentify = pFeatureLayer as IIdentify;
                    IArray pArray = pIdentify.Identify(m_RealCtPolygon);
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj pFIObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRIObj = pFIObj as IRowIdentifyObject;
                        IFeature pFeature = pRIObj.Row as IFeature;
                        IPointCollection pPointCollection = (m_RealCtPolygon as ITopologicalOperator).Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
                        for (int j = 0; j < pPointCollection.PointCount; j++)
                        {
                            IPoint pPoint = pPointCollection.Point[j];
                            //if (tfPoints.Count == 0)
                            //{
                            //    TFPoint tfPoint = new TFPoint();
                            //    tfPoint.No = n++;
                            //    tfPoint.labelPoint = pPoint;
                            //    tfPoint.xzqLevel = "村";
                            //    tfPoint.directPosition = getPosition(upGeo, downGeo, leftGeo, rightGeo, pPoint);
                            //    tfPoint.xzqMC = FeatureHelper.GetFeatureStringValue(pFeature, "ZLDWMC");
                            //    if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                            //    {
                            //        IPoint newPoint = new PointClass();
                            //        newPoint.PutCoords(pPoint.X - 2, pPoint.Y);
                            //        if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                            //        {
                            //            tfPoint.direct = -1;
                            //        }
                            //        else
                            //        {
                            //            tfPoint.direct = 1;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        IPoint newPoint = new PointClass();
                            //        newPoint.PutCoords(pPoint.X, pPoint.Y - 2);
                            //        if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                            //        {
                            //            tfPoint.direct = -1;
                            //        }
                            //        else
                            //        {
                            //            tfPoint.direct = 1;
                            //        }
                            //    }
                            //    tfPoints.Add(tfPoint);
                            //}
                            //else if (!xy.Contains(Math.Round(pPoint.X, 2) + "," + Math.Round(pPoint.Y, 2)))
                            {
                                TFPoint tfPoint = new TFPoint();
                                tfPoint.No = n++;
                                tfPoint.labelPoint = pPoint;
                                tfPoint.xzqLevel = "村";
                                tfPoint.directPosition = getPosition(upGeo, downGeo, leftGeo, rightGeo, pPoint);
                                tfPoint.xzqMC = FeatureHelper.GetFeatureStringValue(pFeature, "ZLDWMC");
                                if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                                {
                                    IPoint newPoint = new PointClass();
                                    newPoint.PutCoords(pPoint.X - 2, pPoint.Y);
                                    if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                                    {
                                        tfPoint.direct = -1;
                                    }
                                    else
                                    {
                                        tfPoint.direct = 1;
                                    }
                                }
                                else
                                {
                                    IPoint newPoint = new PointClass();
                                    newPoint.PutCoords(pPoint.X, pPoint.Y - 2);
                                    if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                                    {
                                        tfPoint.direct = -1;
                                    }
                                    else
                                    {
                                        tfPoint.direct = 1;
                                    }
                                }
                                Boolean b = getDistance(tfPoint, tfPoints);
                                if (b) tfPoints.Add(tfPoint);
                            }
                        }
                    }
                    foreach (TFPoint tfP in tfPoints)
                    {
                        switch (tfP.directPosition)
                        {
                            case "上":
                                addTextElement(tfP.labelPoint.X + tfP.direct * 8 * tfP.xzqMC.Length, tfP.labelPoint.Y + 15, tfP.xzqMC);
                                break;
                            case "下":
                                addTextElement(tfP.labelPoint.X + tfP.direct * 8 * tfP.xzqMC.Length, tfP.labelPoint.Y - 15, tfP.xzqMC);
                                break;
                            case "左":
                                addTextElement(tfP.labelPoint.X - 15, tfP.labelPoint.Y + tfP.direct * 8 * tfP.xzqMC.Length, tfP.xzqMC, true);
                                break;
                            case "右":
                                addTextElement(tfP.labelPoint.X + 15, tfP.labelPoint.Y + tfP.direct * 8 * tfP.xzqMC.Length, tfP.xzqMC, true);
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion

                #region 左上角结合表
                if (sFW.Equals("标准分幅") == true)
                {
                    PointClass[] ZJPos = new PointClass[8];
                    if (true)
                    {
                        object o = Type.Missing;

                        PointClass pp = new PointClass();
                        pp.X = NKP4.X;
                        pp.Y = NKP4.Y + 16;
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                PointClass MidP = new PointClass();
                                double dx = i * 15;
                                double dy = j * 8;
                                double daf1 = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                                ((IConstructPoint)MidP).ConstructAngleDistance(pp, daf1, dx);
                                PointClass BaseP = new PointClass();
                                double daf2 = CommonClassDLL.syc_CalAngle(ref NKP1, ref NKP4);
                                ((IConstructPoint)BaseP).ConstructAngleDistance(MidP, daf2, dy);

                                //计算p1,p2,p3,p4[逆]
                                PointClass p1 = new PointClass();
                                p1.X = BaseP.X;
                                p1.Y = BaseP.Y;
                                p1.Z = 0.0;

                                PointClass p2 = new PointClass();
                                ((IConstructPoint)p2).ConstructAngleDistance(p1, daf1, 15);

                                PointClass p3 = new PointClass();
                                ((IConstructPoint)p3).ConstructAngleDistance(p2, daf2, 8);

                                double daf3 = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP4);
                                PointClass p4 = new PointClass();
                                ((IConstructPoint)p4).ConstructAngleDistance(p3, daf3, 15);

                                PolylineClass pol = new PolylineClass();
                                ((IPointCollection)pol).AddPoint(p1, ref o, ref o);
                                ((IPointCollection)pol).AddPoint(p2, ref o, ref o);
                                ((IPointCollection)pol).AddPoint(p3, ref o, ref o);
                                ((IPointCollection)pol).AddPoint(p4, ref o, ref o);
                                ((IPointCollection)pol).AddPoint(p1, ref o, ref o);
                                lineSym = new SimpleLineSymbolClass();
                                lineSym.Width = 0.1;
                                eleColor = ColorHelper.CreateColor(0, 0, 0);
                                lineSym.Color = eleColor;
                                LineEle = new LineElementClass();
                                LineEle.Geometry = pol;
                                LineEle.Symbol = lineSym;
                                pageCon.AddElement(LineEle, 0);

                                if (i == 1 && j == 1)
                                {
                                    PolygonClass pog = new PolygonClass();
                                    ((IPointCollection)pog).AddPoint(p1, ref o, ref o);
                                    ((IPointCollection)pog).AddPoint(p2, ref o, ref o);
                                    ((IPointCollection)pog).AddPoint(p3, ref o, ref o);
                                    ((IPointCollection)pog).AddPoint(p4, ref o, ref o);
                                    ISimpleFillSymbol fillSym = new SimpleFillSymbolClass();
                                    fillSym.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                                    lineSym = new SimpleLineSymbolClass();
                                    lineSym.Style = esriSimpleLineStyle.esriSLSNull;
                                    fillSym.Outline = lineSym;
                                    PolygonElementClass pogEle = new PolygonElementClass();
                                    pogEle.Geometry = pog;
                                    pogEle.Symbol = fillSym;
                                    pageCon.AddElement(pogEle, 0);
                                }

                                PointClass CenterP = new PointClass();
                                CenterP.X = (p1.X + p3.X) * 0.5;
                                CenterP.Y = (p1.Y + p3.Y) * 0.5;
                                if (i == 0 && j == 0)
                                    ZJPos[0] = CenterP;
                                else if (i == 1 && j == 0)
                                    ZJPos[1] = CenterP;
                                else if (i == 2 && j == 0)
                                    ZJPos[2] = CenterP;
                                else if (i == 2 && j == 1)
                                    ZJPos[3] = CenterP;
                                else if (i == 2 && j == 2)
                                    ZJPos[4] = CenterP;
                                else if (i == 1 && j == 2)
                                    ZJPos[5] = CenterP;
                                else if (i == 0 && j == 2)
                                    ZJPos[6] = CenterP;
                                else if (i == 0 && j == 1)
                                    ZJPos[7] = CenterP;
                            }
                        }

                        double dDJ = (DLIB.DFM2D(m_dJ1) + DLIB.DFM2D(m_dJ3)) * 0.5;
                        double dDW = (DLIB.DFM2D(m_dW1) + DLIB.DFM2D(m_dW3)) * 0.5;
                        double dDelJ = 0.0, dDelW = 0.0;
                        DLIB.GetDelDFM(dScale, ref dDelJ, ref dDelW);
                        double dDelJ_D = DLIB.DFM2D(dDelJ);
                        double dDelW_D = DLIB.DFM2D(dDelW);

                        double[] dJSz = new double[8];
                        double[] dWSz = new double[8];
                        dJSz[0] = dDJ - dDelJ_D;
                        dWSz[0] = dDW - dDelW_D;
                        dJSz[1] = dDJ;
                        dWSz[1] = dDW - dDelW_D;
                        dJSz[2] = dDJ + dDelJ_D;
                        dWSz[2] = dDW - dDelW_D;
                        dJSz[3] = dDJ + dDelJ_D;
                        dWSz[3] = dDW;
                        dJSz[4] = dDJ + dDelJ_D;
                        dWSz[4] = dDW + dDelW_D;
                        dJSz[5] = dDJ;
                        dWSz[5] = dDW + dDelW_D;
                        dJSz[6] = dDJ - dDelJ_D;
                        dWSz[6] = dDW + dDelW_D;
                        dJSz[7] = dDJ - dDelJ_D;
                        dWSz[7] = dDW;

                        string[] sTFHSz = new string[8];
                        for (int i = 0; i < 8; i++)
                        {
                            double dJ_D = dJSz[i];
                            double dW_D = dWSz[i];
                            double dJ = DLIB.HD2DFM(dJ_D * Math.PI / 180.0);
                            double dW = DLIB.HD2DFM(dW_D * Math.PI / 180.0);
                            StringBuilder ss = new StringBuilder(100);
                            double dJ1 = 0.0, dW1 = 0.0, dJ3 = 0.0, dW3 = 0.0;
                            DLIB.GetNewWaima(dScale, dJ, dW, ss);
                            string sTFH = ss.ToString();
                            sTFHSz[i] = sTFH;
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1);
                            ITextSymbol textSymbol = new TextSymbolClass();
                            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                            double dZH = 1.80;	//mm
                            textSymbol.Size = dZH / 25.4 * 72.0;
                            double daf = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3) * 180.0 / Math.PI;
                            textSymbol.Angle = daf;
                            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                            TextElementClass textEle = new TextElementClass();
                            textEle.Symbol = textSymbol;
                            textEle.Text = sTFHSz[i];
                            IElement element = (IElement)textEle;
                            element.Geometry = ZJPos[i];
                            pageCon.AddElement(element, 0);
                        }
                    }
                }
                #endregion

                #region   //方里网
                wait.SetCaption("正在生成方里网信息...");
                //四个角的坐标
                IPoint LDP = new PointClass();
                IPoint tmpJwd = new PointClass();
                tmpJwd.PutCoords(m_dJ1, m_dW1);
                LDP = CoordinateTransHelper.JWD2XY(myMap, tmpJwd);

                IPoint RDP = new PointClass();
                tmpJwd.PutCoords(m_dJ3, m_dW1);
                RDP = CoordinateTransHelper.JWD2XY(myMap, tmpJwd);

                IPoint RUP = new PointClass();
                tmpJwd.PutCoords(m_dJ3, m_dW3);
                RUP = CoordinateTransHelper.JWD2XY(myMap, tmpJwd);

                IPoint LUP = new PointClass();
                tmpJwd.PutCoords(m_dJ1, m_dW3);
                LUP = CoordinateTransHelper.JWD2XY(myMap, tmpJwd);

                int nXGS = 0;
                FLW[] pXFLW = new FLW[500];
                int nBJ = 0;
                int nLastX_KM = 0;
                IPoint LastP = new PointClass();
                while (true)
                {
                    FLW flw = new FLW();
                    if (nBJ == 0)
                    {
                        nBJ++;

                        //第一次:
                        int nX = (int)(LDP.X / 1000.0);
                        int nX2 = nX + 1;
                        double dDel = (nX2 * 1000.0 - LDP.X) / dScale * 1000.0;

                        pLine = new LineClass();
                        pLine.FromPoint = NKP1;
                        pLine.ToPoint = NKP2;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP2.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP4.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nX2.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pXFLW[nXGS] = flw;
                        nXGS++;

                        nLastX_KM = nX2;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    }
                    else
                    {
                        //非第一次:
                        int nCurX_KM = nLastX_KM + 1;
                        int nDist_KM = 1;
                        double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                        pLine = new LineClass();
                        pLine.FromPoint = LastP;
                        pLine.ToPoint = NKP2;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP2.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP4.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nCurX_KM.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pXFLW[nXGS] = flw;
                        nXGS++;

                        nLastX_KM = nCurX_KM;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                        if (flw.PP1.X > NKP2.X)
                        {
                            nXGS = nXGS - 1;
                            break;
                        }
                    }
                }

                for (int i = 0; i < nXGS; i++)
                {
                    object oo = Type.Missing;
                    FLW flw = pXFLW[i] as FLW;

                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                    ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    //Text:
                    pLine = new LineClass();
                    pLine.FromPoint = flw.PP1;
                    pLine.ToPoint = flw.PP2;
                    IPoint ZJP = new PointClass();
                    ((IConstructPoint)ZJP).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, 9.0, false);

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 2.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ1;
                    IElement element = (IElement)textEle;
                    element.Geometry = ZJP;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ2;
                    element = (IElement)textEle;
                    element.Geometry = ZJP;
                    pageCon.AddElement(element, 0);
                }

                int nSGS = 0;
                FLW[] pSFLW = new FLW[500];
                nBJ = 0;
                nLastX_KM = 0;
                LastP = new PointClass();
                while (true)
                {
                    FLW flw = new FLW();
                    if (nBJ == 0)
                    {
                        nBJ++;

                        //第一次:
                        int nX = (int)(LUP.X / 1000.0);
                        int nX2 = nX + 1;
                        double dDel = (nX2 * 1000.0 - LUP.X) / dScale * 1000.0;

                        pLine = new LineClass();
                        pLine.FromPoint = NKP4;
                        pLine.ToPoint = NKP3;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP2.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP4.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nX2.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pSFLW[nSGS] = flw;
                        nSGS++;

                        nLastX_KM = nX2;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    }
                    else
                    {
                        //非第一次:
                        int nCurX_KM = nLastX_KM + 1;
                        int nDist_KM = 1;
                        double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                        pLine = new LineClass();
                        pLine.FromPoint = LastP;
                        pLine.ToPoint = NKP3;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP2.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP4.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nCurX_KM.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pSFLW[nSGS] = flw;
                        nSGS++;

                        nLastX_KM = nCurX_KM;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                        if (flw.PP1.X > NKP3.X)
                        {
                            nSGS = nSGS - 1;
                            break;
                        }
                    }
                } //while(1)

                for (int i = 0; i < nSGS; i++)
                {
                    object oo = Type.Missing;
                    FLW flw = pSFLW[i] as FLW;

                    //线:PP1-PP2
                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                    ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    //Text:
                    pLine = new LineClass();
                    pLine.FromPoint = flw.PP1;
                    pLine.ToPoint = flw.PP2;
                    IPoint ZJP = new PointClass();
                    ((IConstructPoint)ZJP).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, 11.0, false);

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 2.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ1;
                    IElement element = (IElement)textEle;
                    element.Geometry = ZJP;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ2;
                    element = (IElement)textEle;
                    element.Geometry = ZJP;
                    pageCon.AddElement(element, 0);
                }


                int nZGS = 0;
                FLW[] pZFLW = new FLW[500];
                nBJ = 0;
                int nLastY_KM = 0;
                LastP = new PointClass();
                while (true)
                {
                    FLW flw = new FLW();
                    if (nBJ == 0)
                    {
                        nBJ++;

                        //第一次:
                        int nY = (int)(LDP.Y / 1000.0);
                        int nY2 = nY + 1;
                        double dDel = (nY2 * 1000.0 - LDP.Y) / dScale * 1000.0;

                        pLine = new LineClass();
                        pLine.FromPoint = NKP1;
                        pLine.ToPoint = NKP4;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP4.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP2.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nY2.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pZFLW[nZGS] = flw;
                        nZGS++;

                        nLastY_KM = nY2;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    }
                    else
                    {
                        //非第一次:
                        int nCurY_KM = nLastY_KM + 1;
                        int nDist_KM = 1;
                        double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                        pLine = new LineClass();
                        pLine.FromPoint = LastP;
                        pLine.ToPoint = NKP4;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP4.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP2.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nCurY_KM.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pZFLW[nZGS] = flw;
                        nZGS++;

                        nLastY_KM = nCurY_KM;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                        if (flw.PP1.Y > NKP4.Y)
                        {
                            nZGS = nZGS - 1;
                            break;
                        }
                    }
                } //while(1)

                for (int i = 0; i < nZGS; i++)
                {
                    object oo = Type.Missing;
                    FLW flw = pZFLW[i] as FLW;

                    //线:PP1-PP2
                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                    ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    //Text:
                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 2.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ1;
                    IElement element = (IElement)textEle;
                    element.Geometry = flw.PP2;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ2;
                    element = (IElement)textEle;
                    element.Geometry = flw.PP1;
                    pageCon.AddElement(element, 0);
                }

                int nYGS = 0;
                FLW[] pYFLW = new FLW[500];
                nBJ = 0;
                nLastY_KM = 0;
                LastP = new PointClass();
                while (true)
                {
                    FLW flw = new FLW();
                    if (nBJ == 0)
                    {
                        nBJ++;

                        //第一次:
                        int nY = (int)(RDP.Y / 1000.0);
                        int nY2 = nY + 1;
                        double dDel = (nY2 * 1000.0 - RDP.Y) / dScale * 1000.0;

                        pLine = new LineClass();
                        pLine.FromPoint = NKP2;
                        pLine.ToPoint = NKP3;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP4.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP2.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nY2.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pYFLW[nYGS] = flw;
                        nYGS++;

                        nLastY_KM = nY2;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    }
                    else
                    {
                        //非第一次:
                        int nCurY_KM = nLastY_KM + 1;
                        int nDist_KM = 1;
                        double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                        pLine = new LineClass();
                        pLine.FromPoint = LastP;
                        pLine.ToPoint = NKP3;
                        IPoint PP1 = new PointClass();
                        ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                        flw.PP1 = PP1;

                        IPoint p1 = new PointClass();
                        p1.X = (NKP1.X + NKP4.X) * 0.5;
                        p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                        IPoint p2 = new PointClass();
                        p2.X = (NKP3.X + NKP2.X) * 0.5;
                        p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                        double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                        IPoint PP2 = new PointClass();
                        ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                        flw.PP2 = PP2;

                        string sBS = nCurY_KM.ToString();
                        int nLen = sBS.Length;
                        string sZJ1 = sBS.Substring(0, nLen - 2);
                        string sZJ2 = sBS.Substring(nLen - 2, 2);
                        flw.sZJ1 = sZJ1;
                        flw.sZJ2 = sZJ2;
                        flw.sBS = sBS;

                        pYFLW[nYGS] = flw;
                        nYGS++;

                        nLastY_KM = nCurY_KM;
                        LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                        if (flw.PP1.Y > NKP3.Y)
                        {
                            nYGS = nYGS - 1;
                            break;
                        }
                    }
                } //while(1)

                for (int i = 0; i < nYGS; i++)
                {
                    object oo = Type.Missing;
                    FLW flw = pYFLW[i] as FLW;

                    //线:PP1-PP2
                    PolylineClass pol = new PolylineClass();
                    ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                    ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.1;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;
                    LineEle = new LineElementClass();
                    LineEle.Geometry = pol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    //Text:
                    System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    double dZH = 2.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ1;
                    IElement element = (IElement)textEle;
                    element.Geometry = flw.PP1;
                    pageCon.AddElement(element, 0);

                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    dZH = 3.0;	//mm
                    textSymbol.Size = dZH / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = flw.sZJ2;
                    element = (IElement)textEle;
                    element.Geometry = flw.PP2;
                    pageCon.AddElement(element, 0);
                }


                if (chkTKFLW.Checked == false)
                {
                    ArrayList LeftRightLines = new ArrayList();
                    for (int i = 0; i < nZGS; i++)
                    {
                        string sLeftBS = pZFLW[i].sBS;
                        bool bExist = false;
                        int nIndex = -1;
                        for (int j = 0; j < nYGS; j++)
                        {
                            if (pYFLW[j].sBS.Equals(sLeftBS) == true)
                            {
                                bExist = true;
                                nIndex = j;
                                break;
                            }
                        }
                        if (bExist == true)
                        {
                            object oo = Type.Missing;
                            PolylineClass pol = new PolylineClass();
                            ((IPointCollection)pol).AddPoint(pZFLW[i].PP1, ref oo, ref oo);
                            ((IPointCollection)pol).AddPoint(pYFLW[nIndex].PP1, ref oo, ref oo);
                            LeftRightLines.Add(pol);
                        }
                    }

                    ArrayList UpperDownLines = new ArrayList();
                    for (int i = 0; i < nXGS; i++)
                    {
                        string sDownBS = pXFLW[i].sBS;
                        bool bExist = false;
                        int nIndex = -1;
                        for (int j = 0; j < nSGS; j++)
                        {
                            if (pSFLW[j].sBS.Equals(sDownBS) == true)
                            {
                                bExist = true;
                                nIndex = j;
                                break;
                            }
                        }
                        if (bExist == true)
                        {
                            object oo = Type.Missing;
                            PolylineClass pol = new PolylineClass();
                            ((IPointCollection)pol).AddPoint(pXFLW[i].PP1, ref oo, ref oo);
                            ((IPointCollection)pol).AddPoint(pSFLW[nIndex].PP1, ref oo, ref oo);
                            UpperDownLines.Add(pol);
                        }
                    }

                    int nGS1 = LeftRightLines.Count;
                    for (int i = 0; i < nGS1; i++)
                    {
                        IPolyline LRLine = LeftRightLines[i] as IPolyline;
                        IPointCollection pCol1 = LRLine as IPointCollection;
                        IPoint P1 = pCol1.get_Point(0);
                        IPoint P2 = pCol1.get_Point(1);
                        double dA1 = CommonClassDLL.syc_CalAngle(ref P1, ref P2);

                        int nGS2 = UpperDownLines.Count;
                        for (int j = 0; j < nGS2; j++)
                        {
                            IPolyline UDLine = UpperDownLines[j] as IPolyline;
                            IPointCollection pCol2 = UDLine as IPointCollection;
                            IPoint PP1 = pCol2.get_Point(0);
                            IPoint PP2 = pCol2.get_Point(1);
                            double dA2 = CommonClassDLL.syc_CalAngle(ref PP1, ref PP2);

                            IPoint JP = new PointClass();
                            ((IConstructPoint)JP).ConstructAngleIntersection(P1, dA1, PP1, dA2);	//Intersection

                            //以JP为中心,dA1+dA2产生4点:
                            IPoint p1 = new PointClass();
                            ((IConstructPoint)p1).ConstructAngleDistance(JP, dA1, 5.0);
                            IPoint p2 = new PointClass();
                            ((IConstructPoint)p2).ConstructAngleDistance(JP, dA1 + Math.PI, 5.0);
                            IPoint p3 = new PointClass();
                            ((IConstructPoint)p3).ConstructAngleDistance(JP, dA2, 5.0);
                            IPoint p4 = new PointClass();
                            ((IConstructPoint)p4).ConstructAngleDistance(JP, dA2 + Math.PI, 5.0);

                            //线:p1-p2;p3-p4
                            object oo = Type.Missing;
                            PolylineClass pol = new PolylineClass();
                            ((IPointCollection)pol).AddPoint(p1, ref oo, ref oo);
                            ((IPointCollection)pol).AddPoint(p2, ref oo, ref oo);
                            lineSym = new SimpleLineSymbolClass();
                            lineSym.Width = 0.1;
                            eleColor = ColorHelper.CreateColor(0, 0, 0);
                            lineSym.Color = eleColor;
                            LineEle = new LineElementClass();
                            LineEle.Geometry = pol;
                            LineEle.Symbol = lineSym;
                            pageCon.AddElement(LineEle, 0);

                            pol = new PolylineClass();
                            ((IPointCollection)pol).AddPoint(p3, ref oo, ref oo);
                            ((IPointCollection)pol).AddPoint(p4, ref oo, ref oo);
                            lineSym = new SimpleLineSymbolClass();
                            lineSym.Width = 0.1;
                            eleColor = ColorHelper.CreateColor(0, 0, 0);
                            lineSym.Color = eleColor;
                            LineEle = new LineElementClass();
                            LineEle.Geometry = pol;
                            LineEle.Symbol = lineSym;
                            pageCon.AddElement(LineEle, 0);
                        }
                    }
                }
                #endregion

                #region 图例、指北针、面积汇总表、行政区缩略图
                if (!string.IsNullOrEmpty(cboTLLocation.SelectedItem.ToString()) && lstXZQCT.Contains(sFW))
                {
                    wait.SetCaption("正在查找所有图例信息，并加载...");
                    AddLegendNB(NKP2, NKP3, NKP4, pCol);
                }
                if (lstOtherCT.Contains(sFW))
                {
                    wait.SetCaption("正在查找所有图例信息，并加载...");
                    AddLegendWB(NKP2, NKP3, NKP4, pCol);
                }

                IEnvelope pZBZEnv = new EnvelopeClass();
                if (!string.IsNullOrEmpty(cboZBZLocation.SelectedItem.ToString()) && lstXZQCT.Contains(sFW))
                {
                    IPolygon pZBZBK = StructPolygon(cboZBZLocation.SelectedItem.ToString(), 30, 50);

                    //让指北针位置在外框中心位置
                    pZBZEnv.PutCoords(pZBZBK.Envelope.XMin + pZBZBK.Envelope.Width / 2, pZBZBK.Envelope.YMin + pZBZBK.Envelope.Height / 2, pZBZBK.Envelope.XMin + pZBZBK.Envelope.Width / 2, pZBZBK.Envelope.YMin + pZBZBK.Envelope.Height / 2);
                }
                if (lstOtherCT.Contains(sFW))
                {
                    IPoint TLP = new PointClass();
                    double dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                    ((IConstructPoint)TLP).ConstructAngleDistance(tmpP3, dA, 15.0);
                    pZBZEnv = m_PageControl.Page.PrintableBounds;
                    pZBZEnv.XMin = TLP.X;
                    pZBZEnv.XMax = TLP.X + pZBZEnv.Width * 0.98;
                    pZBZEnv.YMax = TLP.Y + pZBZEnv.Height * 0.053;
                    pZBZEnv.YMin = TLP.Y + pZBZEnv.Height * 0.013;
                }
                wait.SetCaption("正在添加指北针信息...");
                AddNorthArrow(m_myMapFrame, pZBZEnv);

                if (!string.IsNullOrEmpty(cboHZBLocation.SelectedItem.ToString()) && lstXZQCT.Contains(sFW))
                {
                    wait.SetCaption("正在生成面积汇总表信息...");
                    bool isXXZQ = sFW == "乡图" || sFW == "村图" ? false : true;
                    string selectedXZQDM = "";
                    if (!isXXZQ)
                    {
                        selectedXZQDM = m_XzqDm;
                    }
                    AddHZB(isXXZQ, selectedXZQDM);
                }
                if (!string.IsNullOrEmpty(cboXZQLocation.SelectedItem.ToString()) && (sFW == "村图" || sFW == "乡图"))
                {
                    wait.SetCaption("正在生成行政区缩略图...");
                    AddXZQLayer(m_XzqDm, sFW);
                }
                #endregion

                wait.SetCaption("正在输出其他整饬信息...");
                if (lstOtherCT.Contains(sFW))
                {
                    IElement mapFrameEle = m_myMapFrame as IElement;
                    outMjGTDW(NKP1, NKP3,true);
                }
                else if (lstXZQCT.Contains(sFW))
                {
                    outMjGTDW(pBKPolygon.Envelope.LowerLeft, pBKPolygon.Envelope.UpperRight);
                }
                this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                IGraphicsContainerSelect pGCSelect = this.m_PageControl.PageLayout as IGraphicsContainerSelect;
                pGCSelect.UnselectAllElements();

                ICommand myTool = new ControlsMapPanToolClass();
                myTool.OnCreate(this.m_MapControl.Object);
                this.m_MapControl.CurrentTool = myTool as ITool;
                m_bufferPolygon = null;
                m_RealCtPolygon = null;

                m_PageControl.ZoomToWholePage();
                m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
                CommonClassDLL.Dispose();

                wait.Close();

                this.m_myTab.SelectedTabPageIndex = 1;
                this.Close();
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void addTextElement(double x, double y, string text, Boolean isVertical = false)
        {
            IGraphicsContainer pageCon = m_PageControl.GraphicsContainer;
            IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
            IActiveView pageAct = m_PageControl.ActiveView;

            PointClass NeedP = new PointClass();
            NeedP.PutCoords(x, y);
            int nX = 0, nY = 0;
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NeedP, out nX, out nY);
            IPoint newP1 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            NeedP.PutCoords(newP1.X, newP1.Y);

            Font dotNetFont = new Font("黑体", 1, FontStyle.Regular);
            ITextSymbol textSymbol = new TextSymbolClass();
            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
            ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
            textSymbol.Size = 18;
            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
            ((ICharacterOrientation)textSymbol).CJKCharactersRotation = true;
            TextElementClass textEle = new TextElementClass();
            textEle.Symbol = textSymbol;
            textEle.Text = text;
            IElement element = (IElement)textEle;
            element.Geometry = NeedP;
            pageCon.AddElement(element, 0);
            if (isVertical)
            {
                ITransform2D trans = (ITransform2D)element;
                trans.Rotate(NeedP, -Math.PI / 2);
                IEnvelope aBound = new EnvelopeClass();
                element.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, aBound);
            }
        }

        private Boolean getDistance(TFPoint tfPoint, List<TFPoint> tfPoints)
        {
            Boolean b = true;
            IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
            IActiveView pageAct = m_PageControl.ActiveView;

            int nX = 0, nY = 0;
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(tfPoint.labelPoint, out nX, out nY);
            IPoint fromPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);

            foreach (TFPoint item in tfPoints)
            {
                if (item.xzqMC == tfPoint.xzqMC && item.directPosition == tfPoint.directPosition)
                {
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(item.labelPoint, out nX, out nY);
                    IPoint toPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                    {
                        Console.WriteLine(Math.Abs((fromPoint.X + tfPoint.direct * 8 * tfPoint.xzqMC.Length) - (toPoint.X + item.direct * 8 * item.xzqMC.Length)));
                        if (Math.Abs((fromPoint.X + tfPoint.direct * 8 * tfPoint.xzqMC.Length) - (toPoint.X + item.direct * 8 * item.xzqMC.Length)) < (item.xzqMC.Length * 18)) b = false;
                    }
                    else
                    {
                        if (Math.Abs((fromPoint.Y + tfPoint.direct * 8 * tfPoint.xzqMC.Length) - (toPoint.Y + item.direct * 8 * item.xzqMC.Length)) < (item.xzqMC.Length * 18)) b = false;
                    }
                }
            }
            return b;
        }

        private string getPosition(IGeometry upGeo, IGeometry downGeo, IGeometry leftGeo, IGeometry rightGeo, IPoint pPoint)
        {
            if (!(upGeo as ITopologicalOperator).Intersect(pPoint, esriGeometryDimension.esriGeometry0Dimension).IsEmpty)
                return "上";
            else if (!(downGeo as ITopologicalOperator).Intersect(pPoint, esriGeometryDimension.esriGeometry0Dimension).IsEmpty)
                return "下";
            else if (!(leftGeo as ITopologicalOperator).Intersect(pPoint, esriGeometryDimension.esriGeometry0Dimension).IsEmpty)
                return "左";
            else if (!(rightGeo as ITopologicalOperator).Intersect(pPoint, esriGeometryDimension.esriGeometry0Dimension).IsEmpty)
                return "右";
            else
                return "";
        }

        /// <summary>
        /// 添加注记标题
        /// </summary>
        /// <param name="WKP3"></param>
        /// <param name="WKP4"></param>
        /// <param name="pBTLoc">标题位置</param>
        /// <param name="pZXJLoc">左下角注记位置</param>
        /// <param name="pYXJLoc">右下角注记位置</param>
        private void AddZJTitle(IPoint WKP3, IPoint WKP4, IPoint pBTLoc, IPoint pZXJLoc, IPoint pYXJLoc, string title)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            string sBT = title;

            string sLD = this.memoZxjZj.Text;
            string sRD = this.memoYXJZJ.Text;

            string delimStr = "\r\n";
            char[] delimiter = delimStr.ToCharArray();
            string[] sBTSz = sBT.Split(delimiter);
            int nLen = sBTSz.Length;
            int nJS = 0;
            double dZG = 16;
            if (cmbCTFW.SelectedItem.ToString() == "乡图")
            {
                dZG = 25;
            }
            sycCommonFuns CommonClassDLL = new sycCommonLib.sycCommonFuns();
            double dTextAF = CommonClassDLL.syc_CalAngle(ref WKP4, ref WKP3) * 180.0 / Math.PI;

            //添加标题
            pBTLoc.Y = pBTLoc.Y + 16;
            for (int i = nLen - 1; i >= 0; i--)
            {
                string ss = sBTSz[i].Trim();
                if (ss.Length == 0)
                    continue;

                pBTLoc.Y = pBTLoc.Y + nJS * (dZG + 2.0);
                nJS++;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = dZG / 25.4 * 72.0;
                textSymbol.Angle = dTextAF;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ss;
                IElement element = (IElement)textEle;
                element.Geometry = pBTLoc;
                pageCon.AddElement(element, 0);
            }

            string[] LDSz = sLD.Split(delimiter);
            nLen = LDSz.Length;
            nJS = 0;
            dZG = 3.5;
            pZXJLoc.Y = pZXJLoc.Y - 12;
            pZXJLoc.X = pZXJLoc.X + 16;
            //添加左下角注记
            for (int i = 0; i < nLen; i++)
            {
                string ss = LDSz[i].Trim();
                if (ss.Length == 0)
                    continue;

                pZXJLoc.Y = pZXJLoc.Y - nJS * (dZG + 2.0);
                nJS++;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 90;
                textSymbol.Size = dZG / 25.4 * 72.0;
                textSymbol.Angle = dTextAF;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ss;
                IElement element = (IElement)textEle;
                element.Geometry = pZXJLoc;
                pageCon.AddElement(element, 0);
            }

            string[] RDSz = sRD.Split(delimiter);
            nLen = RDSz.Length;
            nJS = 0;
            dZG = 3.5;
            //添加右下角注记
            pYXJLoc.X = pYXJLoc.X - 28;
            pYXJLoc.Y = pYXJLoc.Y - 16;
            for (int i = 0; i < nLen; i++)
            {
                string ss = RDSz[i].Trim();
                if (ss.Length == 0)
                    continue;
                ss = ss + "     ";

                pYXJLoc.Y = pYXJLoc.Y - nJS * (dZG + 2.0);
                nJS++;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 90;
                textSymbol.Size = dZG / 25.4 * 72.0;
                textSymbol.Angle = dTextAF;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ss;
                IElement element = (IElement)textEle;
                element.Geometry = pYXJLoc;
                pageCon.AddElement(element, 0);
            }
        }


        /// <summary>
        /// 添加注记标题(标准分幅）
        /// </summary>
        /// <param name="WKP3"></param>
        /// <param name="WKP4"></param>
        /// <param name="pBTLoc">标题位置</param>
        /// <param name="pZXJLoc">左下角注记位置</param>
        /// <param name="pYXJLoc">右下角注记位置</param>
        private void AddZJTitle(IPoint WKP3, IPoint WKP4, IPoint pBTLoc, IPoint pZXJLoc, IPoint pYXJLoc, string title,bool isFF)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            string sBT = title;

            string sLD = this.memoZxjZj.Text;
            string sRD = this.memoYXJZJ.Text;

            string delimStr = "\r\n";
            char[] delimiter = delimStr.ToCharArray();
            string[] sBTSz = sBT.Split(delimiter);
            int nLen = sBTSz.Length;
            int nJS = 0;
            double dZG = 16;
            if (cmbCTFW.SelectedItem.ToString() == "乡图")
            {
                dZG = 25;
            }
            sycCommonFuns CommonClassDLL = new sycCommonLib.sycCommonFuns();
            double dTextAF = CommonClassDLL.syc_CalAngle(ref WKP4, ref WKP3) * 180.0 / Math.PI;

            //添加标题
            pBTLoc.Y = pBTLoc.Y + 16;
            for (int i = nLen - 1; i >= 0; i--)
            {
                string ss = sBTSz[i].Trim();
                if (ss.Length == 0)
                    continue;

                pBTLoc.Y = pBTLoc.Y + nJS * (dZG + 2.0);
                nJS++;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = dZG / 25.4 * 72.0;
                textSymbol.Angle = dTextAF;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ss;
                IElement element = (IElement)textEle;
                element.Geometry = pBTLoc;
                pageCon.AddElement(element, 0);
            }

            string[] LDSz = sLD.Split(delimiter);
            nLen = LDSz.Length;
            nJS = 0;
            dZG = 3.5;
            pZXJLoc.Y = pZXJLoc.Y - 12;
            pZXJLoc.X = pZXJLoc.X + 12;
            //添加左下角注记
            for (int i = 0; i < nLen; i++)
            {
                string ss = LDSz[i].Trim();
                if (ss.Length == 0)
                    continue;

                pZXJLoc.Y = pZXJLoc.Y - nJS * (dZG + 2.0);
                nJS++;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 90;
                textSymbol.Size = dZG / 25.4 * 72.0;
                textSymbol.Angle = dTextAF;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ss;
                IElement element = (IElement)textEle;
                element.Geometry = pZXJLoc;
                pageCon.AddElement(element, 0);
            }

            string[] RDSz = sRD.Split(delimiter);
            nLen = RDSz.Length;
            nJS = 0;
            dZG = 3.5;
            //添加右下角注记
            pYXJLoc.X = pYXJLoc.X - 24;
            pYXJLoc.Y = pYXJLoc.Y - 16;
            for (int i = 0; i < nLen; i++)
            {
                string ss = RDSz[i].Trim();
                if (ss.Length == 0)
                    continue;
                ss = ss + "     ";

                pYXJLoc.Y = pYXJLoc.Y - nJS * (dZG + 2.0);
                nJS++;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 90;
                textSymbol.Size = dZG / 25.4 * 72.0;
                textSymbol.Angle = dTextAF;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ss;
                IElement element = (IElement)textEle;
                element.Geometry = pYXJLoc;
                pageCon.AddElement(element, 0);
            }
        }


        /// <summary>
        /// 添加比例尺
        /// </summary>
        /// <param name="WKP3"></param>
        /// <param name="WKP4"></param>
        /// <param name="curScale">比例尺值</param>
        /// <param name="scaleLoc">比例尺位置</param>
        private void AddScale(IPoint WKP3, IPoint WKP4, string curScale, IPoint scaleLoc)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            sycCommonFuns CommonClassDLL = new sycCommonFuns();
            double dTextAF = CommonClassDLL.syc_CalAngle(ref WKP4, ref WKP3) * 180.0 / Math.PI;

            int nScale = Convert.ToInt32(curScale);
            string ss = "1:" + nScale.ToString();
            Font dotNetFont = new Font("宋体", 1, FontStyle.Bold);
            ITextSymbol textSymbol = new TextSymbolClass();
            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
            ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
            textSymbol.Size = 5.0 / 25.4 * 72.0;
            textSymbol.Angle = dTextAF;
            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
            TextElementClass textEle = new TextElementClass();
            textEle.Symbol = textSymbol;
            textEle.Text = ss;
            IElement element = (IElement)textEle;
            element.Geometry = scaleLoc;
            pageCon.AddElement(element, 0);

            #region 添加线段比例尺
            IMapSurroundFrame pMapSurroundFrame = new MapSurroundFrameClass();
            pMapSurroundFrame.MapFrame = m_myMapFrame;

            IEnvelope envelope = new EnvelopeClass();
            //envelope.PutCoords((pBKPolygon.Envelope.XMin + pBKPolygon.Envelope.XMax) * 0.5 - 50, pBKPolygon.Envelope.YMin - 25, (pBKPolygon.Envelope.XMin + pBKPolygon.Envelope.XMax) * 0.5 + 50, pBKPolygon.Envelope.YMin - 15);
            envelope.PutCoords(scaleLoc.X - 50, scaleLoc.Y - 45, scaleLoc.X + 50, scaleLoc.Y - 10);
            IEnvelope pNewEnv = new EnvelopeClass();

            //从serverStyle文件中获取线段比例尺
            IScaleBar pScaleBar = GetScaleBarFromServerStyle() as IScaleBar;
            //此处修改比例尺属性
            //单位
            pScaleBar.Units = esriUnits.esriMeters;
            pScaleBar.UnitLabel = "米";
            //比例尺分为多少段
            pScaleBar.Divisions = (short)5;
            //每段代表多少米
            pScaleBar.Division = (double)nScale / 100;
            //每段分为多少个小段
            pScaleBar.Subdivisions = 0;
            //在0前面有几段
            pScaleBar.DivisionsBeforeZero = 0;

            //获取比例尺外框的实际大小，根据获取的外框更新element元素的外框使比例尺居中
            pScaleBar.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, envelope, pNewEnv);
            envelope.PutCoords(scaleLoc.X - pNewEnv.Width / 2, scaleLoc.Y - 15, scaleLoc.X + pNewEnv.Width / 2, scaleLoc.Y - 5);

            pMapSurroundFrame.MapSurround = (IMapSurround)pScaleBar;
            IElementProperties pElePro = null;
            IElement pBLCEle = (IElement)pMapSurroundFrame;
            pBLCEle.Geometry = envelope;
            pElePro = pBLCEle as IElementProperties;
            pElePro.Name = "ScaleBar";
            pageCon.AddElement(pBLCEle, 0);
            #endregion
        }

        /// <summary>
        /// 在图件内部生成图例
        /// </summary>
        /// <param name="NKP2"></param>
        /// <param name="NKP3"></param>
        /// <param name="NKP4"></param>
        /// <param name="pCol"></param>
        private void AddLegendNB(IPoint NKP2, IPoint NKP3, IPoint NKP4, IPointCollection pCol)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            sycCommonFuns CommonClassDLL = new sycCommonFuns();
            // 找到所有图例符号，查询数据中真实具有的 图例                   
            ArrayList pLegendFiles = GetAllLegendInfo();
            ArrayList MSz = this.getUniqDLBM();
            ArrayList OtherSz = this.getOtherUniLegend();

            //图例边框宽度
            double tlBKWidth = ((MSz.Count + OtherSz.Count) % 10 == 0 ? (MSz.Count + OtherSz.Count) / 10 : (MSz.Count + OtherSz.Count) / 10 + 1) * 75;
            //构建存放图例的框  
            //IEnvelope pEnvelope = ((IElement)m_myMapFrame).Geometry.Envelope;
            //pEnvelope.SpatialReference = m_myMapFrame.Map.SpatialReference;
            IPolygon pPol = StructPolygon(cboTLLocation.SelectedItem.ToString(), tlBKWidth, 90);
            IElement pPolygonEle = new PolygonElementClass();
            pPolygonEle.Geometry = pPol;

            ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbol();
            pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            pSimpleFillSymbol.Color = ColorHelper.CreateColor(255, 255, 255);
            IFillShapeElement pFillEle = pPolygonEle as IFillShapeElement;
            pFillEle.Symbol = pSimpleFillSymbol;

            pageCon.AddElement(pPolygonEle, 0);

            MSz.Sort(); //small-->big DLCode
            int tlCount = 0;
            double tlMaxWidth = 0;
            IPoint FirstP = new PointClass();
            FirstP.PutCoords(pPolygonEle.Geometry.Envelope.UpperLeft.X + 5, pPolygonEle.Geometry.Envelope.UpperLeft.Y - 5);
            if (MSz.Count != 0)
            {
                for (int iM = 0; iM < MSz.Count; iM++)
                {
                    //3bit地类代码:
                    string sCurCode = MSz[iM] as string;

                    //在pLegendFiles中找是否存在该图例:
                    string sTLName = "";
                    string sTLFile = "";
                    for (int k = 0; k < pLegendFiles.Count; k++)
                    {
                        string sFileName = pLegendFiles[k] as string;
                        int nPos = sFileName.IndexOf("-");
                        if (nPos != -1)
                        {
                            string sTmpCode = sFileName.Substring(0, nPos);
                            if (sTmpCode.Equals(sCurCode) == true)
                            {
                                int nPos2 = sFileName.IndexOf(".");
                                if (nPos2 != -1)
                                {
                                    sTLName = sFileName.Split('.')[0].Replace('-', ' ');//sTLName = sFileName.Substring(nPos + 1, nPos2 - nPos - 1);
                                    sTLFile = Application.StartupPath + @"\图例\" + sFileName;
                                    break;
                                }
                            }
                        }
                    }

                    if (sTLName.Length != 0 && sTLFile.Length != 0)
                    {
                        TifPictureElementClass jpgEle = new TifPictureElementClass();
                        jpgEle.SavePictureInDocument = true;
                        jpgEle.ImportPictureFromFile(sTLFile);
                        jpgEle.MaintainAspectRatio = true;

                        double dDX = 14.0;
                        double dDY = 7.0;
                        IPoint pp1 = new PointClass();
                        pp1.PutCoords(FirstP.X, FirstP.Y);

                        double dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                        IPoint pp2 = new PointClass();
                        ((IConstructPoint)pp2).ConstructAngleDistance(pp1, dA, dDX);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint pp3 = new PointClass();
                        ((IConstructPoint)pp3).ConstructAngleDistance(pp2, dA, dDY);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP4);
                        IPoint pp4 = new PointClass();
                        ((IConstructPoint)pp4).ConstructAngleDistance(pp3, dA, dDX);

                        PolygonClass pos = new PolygonClass();
                        pCol = (IPointCollection)pos;
                        object Missing = Type.Missing;
                        pCol.AddPoint(pp1, ref Missing, ref Missing);
                        pCol.AddPoint(pp2, ref Missing, ref Missing);
                        pCol.AddPoint(pp3, ref Missing, ref Missing);
                        pCol.AddPoint(pp4, ref Missing, ref Missing);
                        ((IElement)jpgEle).Geometry = pos;
                        pageCon.AddElement(jpgEle, 0);

                        dA = CommonClassDLL.syc_CalAngle(ref pp1, ref pp2);
                        IPoint pp = new PointClass();
                        pp.X = (pp2.X + pp3.X) * 0.5;
                        pp.Y = (pp2.Y + pp3.Y) * 0.5;
                        IPoint pp5 = new PointClass();
                        ((IConstructPoint)pp5).ConstructAngleDistance(pp, dA, 10.0);

                        Font dotNetFont = new System.Drawing.Font("黑体", 1);
                        TextSymbolClass textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.5;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                        ITextElement textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sTLName;
                        IElement element = (IElement)textEle;
                        element.Geometry = pp5;
                        pageCon.AddElement(element, 0);
                        tlCount++;

                        //获取要素外框
                        IEnvelope pEleEnv = new EnvelopeClass();
                        element.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, pEleEnv);
                        //记录文字要素最大宽度，防止下一列图例压盖前一列文字
                        if (pEleEnv.Width > tlMaxWidth)
                        {
                            tlMaxWidth = pEleEnv.Width;
                            //最后一列更新所有图例最大宽度
                            //if (MSz.Count / 10 == tlCount / 10)
                            //{
                            //    pPolMax = pEleEnv.LowerRight;
                            //}
                        }

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint newP = new PointClass();
                        ((IConstructPoint)newP).ConstructAngleDistance(pp1, dA, 8);	//2图例间距离
                        //图例换列
                        if (tlCount % 10 == 0 && tlCount != 0)
                        {
                            FirstP.X = pEleEnv.UpperLeft.X + tlMaxWidth + 5;
                            FirstP.Y = pPolygonEle.Geometry.Envelope.UpperLeft.Y - 5;
                            tlMaxWidth = 0;
                        }
                        else
                        {
                            FirstP.X = newP.X;
                            FirstP.Y = newP.Y;
                        }
                    }
                }
            }

            if (OtherSz.Count != 0)
            {
                for (int iO = 0; iO < OtherSz.Count; iO++)
                {
                    //不带路径的TIF文件:
                    string sTIF = OtherSz[iO] as string;
                    int nPos = sTIF.IndexOf(".");
                    if (nPos == -1)
                    {
                        continue;
                    }

                    string sTLName = sTIF.Substring(0, nPos);
                    string sTLFile = "";

                    //检查文件是否存在:
                    for (int k = 0; k < pLegendFiles.Count; k++)
                    {
                        string sFileName = pLegendFiles[k] as string;
                        if (sFileName.Equals(sTIF) == true)
                        {
                            sTLFile = Application.StartupPath + @"\图例\" + sFileName;
                            break;
                        }
                    }

                    //Create Elements:
                    if (sTLName.Length != 0 && sTLFile.Length != 0)
                    {
                        TifPictureElementClass jpgEle = new TifPictureElementClass();
                        jpgEle.ImportPictureFromFile(sTLFile);
                        jpgEle.MaintainAspectRatio = true;

                        double dDX = 14;
                        double dDY = 7;
                        IPoint pp1 = new PointClass();
                        pp1.PutCoords(FirstP.X, FirstP.Y);

                        double dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                        IPoint pp2 = new PointClass();
                        ((IConstructPoint)pp2).ConstructAngleDistance(pp1, dA, dDX);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint pp3 = new PointClass();
                        ((IConstructPoint)pp3).ConstructAngleDistance(pp2, dA, dDY);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP4);
                        IPoint pp4 = new PointClass();
                        ((IConstructPoint)pp4).ConstructAngleDistance(pp3, dA, dDX);

                        PolygonClass pos = new PolygonClass();
                        pCol = (IPointCollection)pos;
                        object Missing = Type.Missing;
                        pCol.AddPoint(pp1, ref Missing, ref Missing);
                        pCol.AddPoint(pp2, ref Missing, ref Missing);
                        pCol.AddPoint(pp3, ref Missing, ref Missing);
                        pCol.AddPoint(pp4, ref Missing, ref Missing);
                        ((IElement)jpgEle).Geometry = pos;
                        pageCon.AddElement(jpgEle, 0);

                        //图例名:
                        dA = CommonClassDLL.syc_CalAngle(ref pp1, ref pp2);
                        IPoint pp = new PointClass();
                        pp.X = (pp2.X + pp3.X) * 0.5;
                        pp.Y = (pp2.Y + pp3.Y) * 0.5;
                        IPoint pp5 = new PointClass();
                        ((IConstructPoint)pp5).ConstructAngleDistance(pp, dA, 10.0);

                        Font dotNetFont = new Font("黑体", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.5;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                        ITextElement textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sTLName;
                        IElement element = (IElement)textEle;
                        element.Geometry = pp5;
                        pageCon.AddElement(element, 0);

                        tlCount++;
                        IEnvelope pEleEnv = new EnvelopeClass();
                        element.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, pEleEnv);
                        //记录文字要素最大宽度，防止下一列图例压盖前一列文字
                        if (pEleEnv.Width > tlMaxWidth)
                        {
                            tlMaxWidth = pEleEnv.Width;
                            //最后一列更新所有图例最大宽度
                            //if (MSz.Count / 10 == tlCount / 10)
                            //{
                            //    pPolMax = pEleEnv.LowerRight;
                            //}
                        }

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint newP = new PointClass();
                        ((IConstructPoint)newP).ConstructAngleDistance(pp1, dA, 15.0);	//2图例间距离
                        //图例换列
                        if (tlCount % 10 == 0 && tlCount != 0)
                        {
                            FirstP.X = pEleEnv.UpperLeft.X + tlMaxWidth + 5;
                            FirstP.Y = pPolygonEle.Geometry.Envelope.UpperLeft.Y - 5;
                            tlMaxWidth = 0;
                        }
                        else
                        {
                            FirstP.X = newP.X;
                            FirstP.Y = newP.Y;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在图件外部生成图例
        /// </summary>
        /// <param name="NKP2"></param>
        /// <param name="NKP3"></param>
        /// <param name="NKP4"></param>
        /// <param name="pCol"></param>
        private void AddLegendWB(IPoint NKP2, IPoint NKP3, IPoint NKP4, IPointCollection pCol)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            sycCommonFuns CommonClassDLL = new sycCommonFuns();
            ArrayList pLegendFiles = GetAllLegendInfo();
            ArrayList MSz = this.getUniqDLBM();
            ArrayList OtherSz = this.getOtherUniLegend();
            //图例:
            ILine pLine = new LineClass();
            pLine.FromPoint = NKP4;
            pLine.ToPoint = NKP3;
            PointClass tmpP4 = new PointClass();
            ((IConstructPoint)tmpP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
            PointClass tmpP3 = new PointClass();
            double dLen = CommonClassDLL.syc_CalLength(ref NKP3, ref NKP4);
            ((IConstructPoint)tmpP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);
            IPoint TLP = new PointClass();
            double dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
            ((IConstructPoint)TLP).ConstructAngleDistance(tmpP3, dA, 18.0);

            System.Drawing.Font dotNetFont = new System.Drawing.Font("黑体", 1);
            ITextSymbol textSymbol = new TextSymbolClass();
            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
            double dZH = 6.5;	//mm
            textSymbol.Size = dZH / 25.4 * 72.0;
            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
            TextElementClass textEle = new TextElementClass();
            textEle.Symbol = textSymbol;
            textEle.Text = "图 例";
            IElement element = (IElement)textEle;
            element.Geometry = TLP;
            pageCon.AddElement(element, 0);

            dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
            IPoint PP = new PointClass();
            ((IConstructPoint)PP).ConstructAngleDistance(tmpP3, dA, 3.0);
            dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
            IPoint FirstP = new PointClass();
            ((IConstructPoint)FirstP).ConstructAngleDistance(PP, dA, 12.5);
            MSz.Sort(); //small-->big DLCode
            if (MSz.Count != 0)
            {
                for (int iM = 0; iM < MSz.Count; iM++)
                {
                    //3bit地类代码:
                    string sCurCode = MSz[iM] as string;

                    //在pLegendFiles中找是否存在该图例:
                    string sTLName = "";
                    string sTLFile = "";
                    for (int k = 0; k < pLegendFiles.Count; k++)
                    {
                        string sFileName = pLegendFiles[k] as string;
                        int nPos = sFileName.IndexOf("-");
                        if (nPos != -1)
                        {
                            string sTmpCode = sFileName.Substring(0, nPos);
                            if (sTmpCode.Equals(sCurCode) == true)
                            {
                                int nPos2 = sFileName.IndexOf(".");
                                if (nPos2 != -1)
                                {
                                    sTLName = sFileName.Split('.')[0].Replace("-", " ");//sTLName = sFileName.Substring(nPos + 1, nPos2 - nPos - 1);
                                    sTLFile = Application.StartupPath + @"\图例\" + sFileName;
                                    break;
                                }
                            }
                        }
                    }

                    if (sTLName.Length != 0 && sTLFile.Length != 0)
                    {
                        TifPictureElementClass jpgEle = new TifPictureElementClass();
                        jpgEle.SavePictureInDocument = true;
                        jpgEle.ImportPictureFromFile(sTLFile);
                        jpgEle.MaintainAspectRatio = true;

                        double dDX = 14.0;
                        double dDY = 7.0;
                        IPoint pp1 = new PointClass();
                        pp1.PutCoords(FirstP.X, FirstP.Y);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                        IPoint pp2 = new PointClass();
                        ((IConstructPoint)pp2).ConstructAngleDistance(pp1, dA, dDX);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint pp3 = new PointClass();
                        ((IConstructPoint)pp3).ConstructAngleDistance(pp2, dA, dDY);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP4);
                        IPoint pp4 = new PointClass();
                        ((IConstructPoint)pp4).ConstructAngleDistance(pp3, dA, dDX);

                        PolygonClass pos = new PolygonClass();
                        pCol = (IPointCollection)pos;
                        object Missing = Type.Missing;
                        pCol.AddPoint(pp1, ref Missing, ref Missing);
                        pCol.AddPoint(pp2, ref Missing, ref Missing);
                        pCol.AddPoint(pp3, ref Missing, ref Missing);
                        pCol.AddPoint(pp4, ref Missing, ref Missing);
                        ((IElement)jpgEle).Geometry = pos;
                        pageCon.AddElement(jpgEle, 0);

                        dA = CommonClassDLL.syc_CalAngle(ref pp1, ref pp2);
                        IPoint pp = new PointClass();
                        pp.X = (pp2.X + pp3.X) * 0.5;
                        pp.Y = (pp2.Y + pp3.Y) * 0.5;
                        IPoint pp5 = new PointClass();
                        ((IConstructPoint)pp5).ConstructAngleDistance(pp, dA, 3.0);

                        dotNetFont = new System.Drawing.Font("黑体", 1);
                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 2.9;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sTLName;
                        element = (IElement)textEle;
                        element.Geometry = pp5;
                        pageCon.AddElement(element, 0);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint newP = new PointClass();
                        ((IConstructPoint)newP).ConstructAngleDistance(pp1, dA, 8);	//2图例间距离
                        FirstP.X = newP.X;
                        FirstP.Y = newP.Y;
                    }
                }
            }

            if (OtherSz.Count != 0)
            {
                for (int iO = 0; iO < OtherSz.Count; iO++)
                {
                    //不带路径的TIF文件:
                    string sTIF = OtherSz[iO] as string;
                    int nPos = sTIF.IndexOf(".");
                    if (nPos != -1) ;
                    else continue;

                    string sTLName = sTIF.Substring(0, nPos);
                    string sTLFile = "";

                    //检查文件是否存在:
                    for (int k = 0; k < pLegendFiles.Count; k++)
                    {
                        string sFileName = pLegendFiles[k] as string;
                        if (sFileName.Equals(sTIF) == true)
                        {
                            sTLFile = Application.StartupPath + @"\图例\" + sFileName;
                            break;
                        }
                    }

                    //Create Elements:
                    if (sTLName.Length != 0 && sTLFile.Length != 0)
                    {
                        TifPictureElementClass jpgEle = new TifPictureElementClass();
                        jpgEle.ImportPictureFromFile(sTLFile);
                        jpgEle.MaintainAspectRatio = true;

                        double dDX = 14;
                        double dDY = 7;
                        IPoint pp1 = new PointClass();
                        pp1.PutCoords(FirstP.X, FirstP.Y);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                        IPoint pp2 = new PointClass();
                        ((IConstructPoint)pp2).ConstructAngleDistance(pp1, dA, dDX);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint pp3 = new PointClass();
                        ((IConstructPoint)pp3).ConstructAngleDistance(pp2, dA, dDY);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP4);
                        IPoint pp4 = new PointClass();
                        ((IConstructPoint)pp4).ConstructAngleDistance(pp3, dA, dDX);

                        PolygonClass pos = new PolygonClass();
                        pCol = (IPointCollection)pos;
                        object Missing = Type.Missing;
                        pCol.AddPoint(pp1, ref Missing, ref Missing);
                        pCol.AddPoint(pp2, ref Missing, ref Missing);
                        pCol.AddPoint(pp3, ref Missing, ref Missing);
                        pCol.AddPoint(pp4, ref Missing, ref Missing);
                        ((IElement)jpgEle).Geometry = pos;
                        pageCon.AddElement(jpgEle, 0);

                        //图例名:
                        dA = CommonClassDLL.syc_CalAngle(ref pp1, ref pp2);
                        IPoint pp = new PointClass();
                        pp.X = (pp2.X + pp3.X) * 0.5;
                        pp.Y = (pp2.Y + pp3.Y) * 0.5;
                        IPoint pp5 = new PointClass();
                        ((IConstructPoint)pp5).ConstructAngleDistance(pp, dA, 3.0);

                        dotNetFont = new System.Drawing.Font("黑体", 1);
                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 2.9;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sTLName;
                        element = (IElement)textEle;
                        element.Geometry = pp5;
                        pageCon.AddElement(element, 0);

                        dA = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP2);
                        IPoint newP = new PointClass();
                        ((IConstructPoint)newP).ConstructAngleDistance(pp1, dA, 15.0);	//2图例间距离
                        FirstP.X = newP.X;
                        FirstP.Y = newP.Y;
                    }
                }
            }
        }

        /// <summary>
        /// 添加行政区缩略图
        /// </summary>
        private void AddXZQLayer(string curXZQDM, string tjType)
        {
            string layerName = "";
            string fieldName = "";
            string displayFieldName = "";
            if (tjType == "村图")
            {
                layerName = "CJDCQ";
                fieldName = "ZLDWDM";
                displayFieldName = "ZLDWMC";
            }
            else if (tjType == "乡图")
            {
                layerName = "XZQ";
                fieldName = "XZQDM";
                displayFieldName = "XZQMC";
            }
            else
            {
                return;
            }

            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            IPolygon pXZQBK = StructPolygon(cboXZQLocation.SelectedItem.ToString(), 150, 150);
            IElement pPolygonEle = new PolygonElementClass();
            pPolygonEle.Geometry = pXZQBK;

            ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbol();
            pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            pSimpleFillSymbol.Color = ColorHelper.CreateColor(255, 255, 255);
            IFillShapeElement pFillEle = pPolygonEle as IFillShapeElement;
            pFillEle.Symbol = pSimpleFillSymbol;

            pageCon.AddElement(pPolygonEle, 0);

            IFeatureLayer pShowLayer = ColorLayer(layerName, fieldName, curXZQDM, displayFieldName);

            IMap pMap = new MapClass();
            pMap.AddLayer(pShowLayer);

            IMapFrame pMapFrame = new MapFrameClass();
            pMapFrame.Map = pMap;

            IElement pXZQEle = pMapFrame as IElement;
            pXZQEle.Geometry = pXZQBK;

            IElementProperties pProEle = pXZQEle as IElementProperties;
            pProEle.Name = "XZQSLT";

            pageCon.AddElement(pXZQEle, 0);
        }


        private IFeatureLayer ColorLayer(string layerName, string fieldName, string xzqdm, string displayFieldName)
        {
            IWorkspace pTarWS = null;
            try
            {
                string tempPath = AppDomain.CurrentDomain.BaseDirectory + "Temp";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                string tempGDB = tempPath + "\\temp.gdb";

                pTarWS = WorkspaceHelper2.DeleteAndNewGDB(tempGDB);

                EsriDatabaseHelper.ConvertFeatureClass(GlobalEditObject.GlobalWorkspace, pTarWS, layerName, layerName, null);
                IFeatureClass pXZQFC = (pTarWS as IFeatureWorkspace).OpenFeatureClass(layerName);
                IFeatureLayer pFeaLyr = new FeatureLayerClass();
                pFeaLyr.FeatureClass = pXZQFC;

                UniqValueDrawing(pFeaLyr, fieldName, xzqdm, displayFieldName);

                return pFeaLyr;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (pTarWS != null)
                {
                    Marshal.FinalReleaseComObject(pTarWS);
                }
            }
        }

        private void UniqValueDrawing(IFeatureLayer pFeaLayer, string fieldName, string fieldValue, string displayField)
        {
            IGeoFeatureLayer pLyr = (pFeaLayer as IGeoFeatureLayer);
            if (pLyr == null)
                return;

            ArrayList arry = FeatureHelper.GetUniqueFieldValueByDataStatistics(pFeaLayer.FeatureClass, null, fieldName);
            try
            {
                IUniqueValueRenderer pRender = new UniqueValueRendererClass();
                #region 获取唯一值

                ISymbol symd = SymbolHelper.CreateTmpSym(pFeaLayer.FeatureClass.ShapeType);

                pRender.FieldCount = 1;
                pRender.set_Field(0, fieldName);
                pRender.DefaultSymbol = symd as ISymbol;
                pRender.UseDefaultSymbol = true;

                for (int i = 0; i < arry.Count; i++)
                {
                    ISymbol symx = SymbolHelper.CreateTmpSym(pFeaLayer.FeatureClass.ShapeType);
                    pRender.AddValue(arry[i].ToString(), fieldName, symx as ISymbol);
                    pRender.set_Label(arry[i].ToString(), arry[i].ToString());
                    pRender.set_Symbol(arry[i].ToString(), symx as ISymbol);
                }
                #endregion

                IRgbColor pColor1 = new RgbColorClass();
                pColor1.Red = 255;
                pColor1.Green = 0;
                pColor1.Blue = 0;
                IRgbColor pColor2 = new RgbColorClass();
                pColor2.Red = 255;
                pColor2.Green = 255;
                pColor2.Blue = 0;

                #region 渲染
                for (int ny = 0; ny < pRender.ValueCount; ny++)
                {
                    string xv = pRender.get_Value(ny);
                    ISymbol jsy = pRender.get_Symbol(xv);
                    if (xv == fieldValue)
                    {
                        (jsy as ISimpleFillSymbol).Color = pColor1;
                    }
                    else
                    {
                        (jsy as ISimpleFillSymbol).Color = pColor2;
                    }
                    pRender.set_Symbol(xv, jsy);
                }

                pRender.ColorScheme = "Custom";
                pRender.set_FieldType(0, true);

                pLyr.Renderer = pRender as IFeatureRenderer;
                pLyr.DisplayField = displayField;
                pLyr.DisplayAnnotation = true;
                #endregion
            }
            catch (Exception ex) { }

        }

        /// <summary>
        /// 添加一级类面积汇总表
        /// </summary>
        /// <param name="isXJXZQ">是否是县级行政区true-是；false-不是</param>
        /// <param name="curXZQDM">当前出图的行政区代码（县级不需要输入）</param>
        private void AddHZB(bool isXJXZQ, string curXZQDM = "")
        {
            IFeatureWorkspace pFeaWS = null;
            IFeatureClass pDLTBFC = null;
            ICursor cursor = null;
            try
            {
                if (!isXJXZQ && string.IsNullOrEmpty(curXZQDM))
                {
                    return;
                }
                Dictionary<string, string> dicDLBM = new Dictionary<string, string>();
                dicDLBM.Add("湿地(00)", "'0303','0304','0306','0402','0603','1105','1106','1108'");
                dicDLBM.Add("耕地(01)", "'0101','0102','0103'");
                dicDLBM.Add("种植园用地(02)", "'0201','0202','0203','0204'");
                dicDLBM.Add("林地(03)", "'0301','0302','0305','0307'");
                dicDLBM.Add("草地(04)", "'0401','0403','0404'");
                dicDLBM.Add("商业服务业用地(05)", "'05H1','0508'");
                dicDLBM.Add("工矿用地(06)", "'0601','0602'");
                dicDLBM.Add("住宅用地(07)", "'0701','0702'");
                dicDLBM.Add("公共管理与公共服务用地(08)", "'08H1','08H2','0809','0810'");
                dicDLBM.Add("特殊用地(09)", "'09'");
                dicDLBM.Add("交通运输用地(10)", "'1001','1002','1003','1004','1005','1006','1007','1008','1009'");
                dicDLBM.Add("水域及水利设施用地(11)", "'1101','1102','1103','1104','1107','1109','1110'");
                dicDLBM.Add("其他土地(12)", "'1201','1202','1203','1204','1205','1206','1207'");

                IQueryFilter pQueryFilter = new QueryFilterClass();
                pFeaWS = GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
                pDLTBFC = pFeaWS.OpenFeatureClass("DLTB");

                DataTable dtSource = new DataTable();
                DataColumn col1 = new DataColumn();
                col1.ColumnName = "地类";
                dtSource.Columns.Add(col1);
                DataColumn col2 = new DataColumn();
                col2.ColumnName = "面积(平方米)";
                dtSource.Columns.Add(col2);
                double totalMJ = 0;
                string zlWhere = "";
                if (!isXJXZQ)
                {
                    zlWhere = string.Format("and ZLDWDM like '{0}%'", curXZQDM);
                }
                foreach (KeyValuePair<string, string> item in dicDLBM)
                {
                    pQueryFilter.WhereClause = string.Format("DLBM in ({0}) {1}", item.Value, zlWhere);
                    IDataStatistics dataStatistics = new DataStatisticsClass();
                    dataStatistics.Field = "TBDLMJ";
                    cursor = (ICursor)pDLTBFC.Search(pQueryFilter, false);
                    dataStatistics.Cursor = cursor;
                    IStatisticsResults statisticsResults = dataStatistics.Statistics;
                    double mj = statisticsResults.Sum;
                    if (mj > 0)
                    {
                        DataRow drNew = dtSource.NewRow();
                        drNew["地类"] = item.Key;
                        drNew["面积(平方米)"] = MathHelper.Round(mj, 2);
                        dtSource.Rows.Add(drNew);
                        totalMJ += MathHelper.Round(mj, 2);
                    }
                }
                //田坎面积放入其他土地
                IDataStatistics dataTK = new DataStatisticsClass();
                dataTK.Field = "KCMJ";
                if (!isXJXZQ)
                {
                    pQueryFilter.WhereClause = string.Format("ZLDWDM like '{0}%'", curXZQDM);
                    cursor = (ICursor)pDLTBFC.Search(pQueryFilter, false);
                }
                else
                {
                    cursor = (ICursor)pDLTBFC.Search(null, false);
                }
                dataTK.Cursor = cursor;
                IStatisticsResults sumResult = dataTK.Statistics;
                double tkmj = sumResult.Sum;
                if (tkmj > 0)
                {
                    totalMJ += MathHelper.Round(tkmj, 2);
                    DataRow[] drQT = dtSource.Select("地类='其他土地(12)'");
                    if (drQT.Length > 0)
                    {
                        drQT[0]["面积(平方米)"] = double.Parse(drQT[0]["面积(平方米)"].ToString()) + MathHelper.Round(tkmj, 2);
                    }
                    else
                    {
                        DataRow newRow = dtSource.NewRow();
                        newRow["地类"] = "其他土地(12)";
                        newRow["面积(平方米)"] = MathHelper.Round(tkmj, 2);
                    }
                }
                if (totalMJ > 0)
                {
                    DataRow drTotal = dtSource.NewRow();
                    drTotal["地类"] = "合计";
                    drTotal["面积(平方米)"] = totalMJ;
                    dtSource.Rows.Add(drTotal);
                }

                double rowHeight = 10;
                double rowWidth = 50;

                IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
                IPolygon pPolygonHZB = StructPolygon(cboHZBLocation.SelectedItem.ToString(), rowWidth * dtSource.Columns.Count, rowHeight * (dtSource.Rows.Count + 1));
                IElement pEle = new PolygonElementClass();
                pEle.Geometry = pPolygonHZB;

                ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbol();
                pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                pSimpleFillSymbol.Color = ColorHelper.CreateColor(255, 255, 255);
                IFillShapeElement pFillEle = pEle as IFillShapeElement;
                pFillEle.Symbol = pSimpleFillSymbol;

                pageCon.AddElement(pEle, 0);

                IPointCollection pPoints = CreateTable(pPolygonHZB, dtSource.Rows.Count + 1, dtSource.Columns.Count);
                //将数据源插入对应位置
                int i = 0;
                //列标题
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    System.Drawing.Font dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Bold);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                    textSymbol.Size = 10;
                    //textSymbol.Angle = dTextAF;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = dtSource.Columns[j].ColumnName;
                    IElement element = (IElement)textEle;
                    element.Geometry = pPoints.Point[i];
                    pageCon.AddElement(element, 0);
                    i++;
                }
                //具体数据
                foreach (DataRow rowTemp in dtSource.Rows)
                {
                    foreach (DataColumn colTemp in dtSource.Columns)
                    {
                        System.Drawing.Font dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Regular);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                        textSymbol.Size = 10;
                        //textSymbol.Angle = dTextAF;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = rowTemp[colTemp.ColumnName].ToString();
                        IElement element = (IElement)textEle;
                        element.Geometry = pPoints.Point[i];
                        pageCon.AddElement(element, 0);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (pDLTBFC != null)
                {
                    Marshal.FinalReleaseComObject(pDLTBFC);
                }
                if (cursor != null)
                {
                    Marshal.FinalReleaseComObject(cursor);
                }
            }
        }

        /// <summary>
        /// 创建汇总表的表格，并返回各单元格的中心点坐标集合
        /// </summary>
        /// <param name="pPol"></param>
        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns></returns>
        private IPointCollection CreateTable(IPolygon pPol, int rowCount, int columnCount)
        {
            double rowHelght = pPol.Envelope.Height / rowCount;
            double columnWidth = pPol.Envelope.Width / columnCount;
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;

            ISimpleLineSymbol pSimpleFillSymbol = new SimpleLineSymbol();
            pSimpleFillSymbol.Color = ColorHelper.CreateColor(0, 0, 0);

            //从上到下，从左到右记录每个单元格中心点的坐标，用于插入数据
            IPointCollection pPoints = new MultipointClass();
            for (int i = 0; i < columnCount; i++)
            {
                IPoint p = new PointClass();
                p.PutCoords(pPol.Envelope.XMin + columnWidth * i + columnWidth / 2, pPol.Envelope.YMax - rowHelght / 2);
                pPoints.AddPoint(p);
            }


            //创建横线
            for (int i = rowCount - 1; i >= 1; i--)
            {
                IPolyline line = new PolylineClass();
                IPoint pStart = new PointClass();
                pStart.PutCoords(pPol.Envelope.XMin, pPol.Envelope.YMin + rowHelght * i);
                line.FromPoint = pStart;
                IPoint pEnd = new PointClass();
                pEnd.PutCoords(pPol.Envelope.XMax, pPol.Envelope.YMin + rowHelght * i);
                line.ToPoint = pEnd;
                IElement pLineEle = new LineElementClass();
                pLineEle.Geometry = line;
                ILineElement pLine = pLineEle as ILineElement;
                pLine.Symbol = pSimpleFillSymbol;
                pageCon.AddElement(pLineEle, 0);

                for (int j = 0; j < columnCount; j++)
                {
                    IPoint p = new PointClass();
                    p.PutCoords(pPol.Envelope.XMin + columnWidth * (j + 0.5), pPol.Envelope.YMin + rowHelght * (i - 0.5));
                    pPoints.AddPoint(p);
                }
            }
            //创建竖线
            for (int i = 1; i < columnCount; i++)
            {
                IPolyline line = new PolylineClass();
                IPoint pStart = new PointClass();
                pStart.PutCoords(pPol.Envelope.XMin + columnWidth * i, pPol.Envelope.YMin);
                line.FromPoint = pStart;
                IPoint pEnd = new PointClass();
                pEnd.PutCoords(pPol.Envelope.XMin + columnWidth * i, pPol.Envelope.YMax);
                line.ToPoint = pEnd;
                IElement pLineEle = new LineElementClass();
                pLineEle.Geometry = line;
                ILineElement pLine = pLineEle as ILineElement;
                pLine.Symbol = pSimpleFillSymbol;
                pageCon.AddElement(pLineEle, 0);
            }
            return pPoints;
        }

        /// <summary>
        /// 构建矩形框，存储图例、行政区、汇总表等
        /// </summary>
        /// <param name="startLoc">在哪个角</param>
        /// <param name="pNK">内框</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        private IPolygon StructPolygon(string startLoc, double width, double height)
        {
            try
            {
                IEnvelope pNK = m_bufferPolygon.Envelope;
                int nX = 0, nY = 0;
                IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
                IActiveView pageAct = m_PageControl.ActiveView;

                IPoint p1 = new PointClass();
                IPoint p2 = new PointClass();
                IPoint p3 = new PointClass();
                IPoint p4 = new PointClass();
                IPoint pPoint = null;
                IPointCollection pPoints = new MultipointClass();
                switch (startLoc)
                {
                    case "左上":
                        mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pNK.UpperLeft, out nX, out nY);
                        pPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                        //pPoint = pNK.Envelope.UpperLeft;
                        p1.PutCoords(pPoint.X, pPoint.Y);
                        pPoints.AddPoint(p1);
                        p2.PutCoords(pPoint.X + width, pPoint.Y);
                        pPoints.AddPoint(p2);
                        p3.PutCoords(pPoint.X + width, pPoint.Y - height);
                        pPoints.AddPoint(p3);
                        p4.PutCoords(pPoint.X, pPoint.Y - height);
                        pPoints.AddPoint(p4);
                        break;
                    case "左下":

                        mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pNK.LowerLeft, out nX, out nY);
                        pPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                        //pPoint.PutCoords(newP1.X, newP1.Y);

                        //pPoint = pNK.Envelope.LowerLeft;
                        p1.PutCoords(pPoint.X, pPoint.Y);
                        pPoints.AddPoint(p1);
                        p2.PutCoords(pPoint.X, pPoint.Y + height);
                        pPoints.AddPoint(p2);
                        p3.PutCoords(pPoint.X + width, pPoint.Y + height);
                        pPoints.AddPoint(p3);
                        p4.PutCoords(pPoint.X + width, pPoint.Y);
                        pPoints.AddPoint(p4);
                        break;
                    case "右上":
                        mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pNK.UpperRight, out nX, out nY);
                        pPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                        p1.PutCoords(pPoint.X - width, pPoint.Y);
                        pPoints.AddPoint(p1);
                        p2.PutCoords(pPoint.X, pPoint.Y);
                        pPoints.AddPoint(p2);
                        p3.PutCoords(pPoint.X, pPoint.Y - height);
                        pPoints.AddPoint(p3);
                        p4.PutCoords(pPoint.X - width, pPoint.Y - height);
                        pPoints.AddPoint(p4);
                        break;
                    case "右下":
                        mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pNK.LowerRight, out nX, out nY);
                        pPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                        p1.PutCoords(pPoint.X - width, pPoint.Y);
                        pPoints.AddPoint(p1);
                        p2.PutCoords(pPoint.X - width, pPoint.Y + height);
                        pPoints.AddPoint(p2);
                        p3.PutCoords(pPoint.X, pPoint.Y + height);
                        pPoints.AddPoint(p3);
                        p4.PutCoords(pPoint.X, pPoint.Y);
                        pPoints.AddPoint(p4);
                        break;
                    default:
                        break;
                }
                Ring ring = new RingClass();
                for (int i = 0; i < pPoints.PointCount; i++)
                {
                    ring.AddPoint(pPoints.Point[i], Type.Missing, Type.Missing);
                }
                ring.AddPoint(pPoints.Point[0], Type.Missing, Type.Missing);//必须补充起点，作为终点
                IGeometryCollection pointPolygon = new PolygonClass();
                pointPolygon.AddGeometry(ring as IGeometry, Type.Missing, Type.Missing);

                //    //测试
                //      IRgbColor pMarkerSymbolColor = new RgbColor();
                //pMarkerSymbolColor.Red = 255;
                //pMarkerSymbolColor.Green = 0;
                //pMarkerSymbolColor.Blue = 0;

                //// 轮廓线颜色
                //IRgbColor pOutlineColor = new RgbColor();
                //pOutlineColor.Red = 0;
                //pOutlineColor.Green = 0;
                //pOutlineColor.Blue = 255;

                //// 创建符号
                //ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbol();
                //pSimpleMarkerSymbol.Angle = 30;
                //pSimpleMarkerSymbol.Color = pMarkerSymbolColor;
                //pSimpleMarkerSymbol.Size = 15;
                //pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
                //pSimpleMarkerSymbol.Outline = true;
                //pSimpleMarkerSymbol.OutlineColor = pOutlineColor;
                //pSimpleMarkerSymbol.OutlineSize = 3;

                //    IMarkerElement pMarkerElement = new MarkerElementClass();
                //    pMarkerElement.Symbol = pSimpleMarkerSymbol;
                //    IElement pElement = pMarkerElement as IElement;
                //    pElement.Geometry = p1;
                //    IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
                //    pageCon.AddElement(pElement, 0);
                IPolygon polygonGeo = pointPolygon as IPolygon;
                return polygonGeo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加指北针
        /// </summary>
        /// <param name="mapFrame">地图边框</param>
        /// <param name="pPoint">指北针图形</param>
        private void AddNorthArrow(IMapFrame mapFrame, IEnvelope pEnv)
        {
            IUID uid = new UIDClass();
            uid.Value = "esriCarto.MarkerNorthArrow";

            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null); // Dynamic Cast
            IElement element = mapSurroundFrame as IElement;
            element.Geometry = pEnv;
            element.Activate(m_PageControl.ActiveView.ScreenDisplay);

            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            pageCon.AddElement(element, 0);
            IMapSurround mapSurround = mapSurroundFrame.MapSurround;
            IMarkerNorthArrow markerNorthArrow = mapSurround as IMarkerNorthArrow;
            IMarkerSymbol markerSymbol = markerNorthArrow.MarkerSymbol;
            ICharacterMarkerSymbol characterMarkerSymbol = markerSymbol as ICharacterMarkerSymbol;
            characterMarkerSymbol.CharacterIndex = 177;
            markerNorthArrow.MarkerSymbol = characterMarkerSymbol;
        }

        /// <summary>
        /// 创建图件花边框
        /// </summary>
        /// <param name="pWBPolygon"></param>
        public void CreateNeatline(IPolygon pWBPolygon)
        {
            //创建线元素
            IElement neatlineElement = new LineElementClass();

            IPointCollection pPoints = new MultipointClass();
            IPoint p1 = new PointClass();
            p1.PutCoords(pWBPolygon.Envelope.UpperLeft.X - 5, pWBPolygon.Envelope.UpperLeft.Y + 5);
            pPoints.AddPoint(p1);
            IPoint p2 = new PointClass();
            p2.PutCoords(pWBPolygon.Envelope.UpperRight.X + 5, pWBPolygon.Envelope.UpperRight.Y + 5);
            pPoints.AddPoint(p2);
            IPoint p3 = new PointClass();
            p3.PutCoords(pWBPolygon.Envelope.LowerRight.X + 5, pWBPolygon.Envelope.LowerRight.Y - 5);
            pPoints.AddPoint(p3);
            IPoint p4 = new PointClass();
            p4.PutCoords(pWBPolygon.Envelope.LowerLeft.X - 5, pWBPolygon.Envelope.LowerLeft.Y - 5);
            pPoints.AddPoint(p4);
            pPoints.AddPoint(p1);

            Ring ring = new RingClass();
            for (int i = 0; i < pPoints.PointCount; i++)
            {
                ring.AddPoint(pPoints.Point[i], Type.Missing, Type.Missing);
            }
            IGeometryCollection pointPolygon = new PolygonClass();
            pointPolygon.AddGeometry(ring as IGeometry, Type.Missing, Type.Missing);

            pBKPolygon = pointPolygon as IPolygon;
            ISegmentCollection segmentCollction = pBKPolygon as ISegmentCollection;
            ISegmentCollection polyline = new Polyline() as ISegmentCollection;
            object missing = Type.Missing;
            for (int m = 0; m < segmentCollction.SegmentCount; m++)
            {
                ISegment pSeg = segmentCollction.get_Segment(m);
                polyline.AddSegment(pSeg, ref missing, ref missing);
            }
            IGeometry pGeo = polyline as IPolyline;

            neatlineElement.Geometry = pGeo;

            //从符号库中获取图框的符号
            ISymbol pLineSymbol = GetSymbolFromFile();
            (neatlineElement as ILineElement).Symbol = pLineSymbol as ILineSymbol;

            //将线元素添加到地图中
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            pageCon.AddElement(neatlineElement, 0);
        }

        /// <summary>
        /// 获取边框样式
        /// </summary>
        /// <returns></returns>
        private ISymbol GetSymbolFromFile()
        {
            string styleFile = RCIS.Global.AppParameters.StylePath + @"\style.ServerStyle";
            IStyleGallery styleGalley = new ESRI.ArcGIS.Display.ServerStyleGalleryClass();
            IStyleGalleryStorage styleGalleryStorage = styleGalley as IStyleGalleryStorage;
            styleGalleryStorage.AddFile(styleFile);

            IEnumStyleGalleryItem enumStyleGalleryItem = styleGalley.get_Items("Line Symbols", styleFile, "");
            string symName = "TJBK";
            //enumStyleGalleryItem.Reset();
            IStyleGalleryItem styleGalleryItem = enumStyleGalleryItem.Next();
            ISymbol symbol = null;
            while (styleGalleryItem != null)
            {
                if (styleGalleryItem.Name.ToUpper().Trim() == symName.ToUpper().Trim())
                {
                    symbol = (ISymbol)styleGalleryItem.Item;
                    break;
                }
                styleGalleryItem = enumStyleGalleryItem.Next();
            }
            styleGalleryStorage.RemoveFile(styleFile);
            return symbol as ISymbol;
        }

        /// <summary>
        /// 获取比例尺样式
        /// </summary>
        /// <returns></returns>
        private IScaleBar GetScaleBarFromServerStyle()
        {
            IEnumStyleGalleryItem enumStyleGalleryItem = null;
            IStyleGalleryItem styleGalleryItem = null;
            IStyleGallery styleGalley = null;
            IStyleGalleryStorage styleGalleryStorage = null;

            try
            {
                string styleFile = RCIS.Global.AppParameters.StylePath + @"\style.ServerStyle";
                styleGalley = new ESRI.ArcGIS.Display.ServerStyleGalleryClass();
                styleGalleryStorage = styleGalley as IStyleGalleryStorage;
                styleGalleryStorage.AddFile(styleFile);

                enumStyleGalleryItem = styleGalley.get_Items("Scale Bars", styleFile, "");
                string symName = "XDBLC";
                enumStyleGalleryItem.Reset();
                styleGalleryItem = enumStyleGalleryItem.Next();
                IScaleBar symbol = null;
                while (styleGalleryItem != null)
                {
                    if (styleGalleryItem.Name.ToUpper().Trim() == symName.ToUpper().Trim())
                    {
                        symbol = (IScaleBar)styleGalleryItem.Item;
                        break;
                    }
                    Marshal.FinalReleaseComObject(styleGalleryItem);
                    styleGalleryItem = enumStyleGalleryItem.Next();
                }
                styleGalleryStorage.RemoveFile(styleFile);
                return symbol;
            }
            catch (Exception ex)
            {
                if (enumStyleGalleryItem != null)
                {
                    Marshal.FinalReleaseComObject(enumStyleGalleryItem);
                }
                if (styleGalleryItem != null)
                {
                    Marshal.FinalReleaseComObject(styleGalleryItem);
                }
                if (styleGalley != null)
                {
                    Marshal.FinalReleaseComObject(styleGalley);
                }
                if (styleGalleryStorage != null)
                {
                    Marshal.FinalReleaseComObject(styleGalleryStorage);
                }
                return null;
            }
        }

        /// <summary>
        /// 获取地类代码唯一值
        /// </summary>
        /// <returns></returns>
        private ArrayList getUniqDLBM()
        {
            IFeatureLayer DLTBLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "DLTB");
            ArrayList MSz = new ArrayList();
            if (DLTBLayer != null)
            {
                IFeatureClass dltbClass = DLTBLayer.FeatureClass;
                MSz = FeatureHelper.GetUniqueFieldValueByDataStatistics(dltbClass, "DLBM", m_RealCtPolygon);
            }
            return MSz;
        }

        /// <summary>
        /// 获取其他图例唯一值
        /// </summary>
        /// <returns></returns>
        private ArrayList getOtherUniLegend()
        {
            ArrayList OtherSz = new ArrayList(); //各种界线
            IFeatureLayer XzqjxLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "XZQJX");
            if (XzqjxLayer != null)
            {
                IFeatureClass xzqjxClass = XzqjxLayer.FeatureClass;
                ArrayList arJxlx = FeatureHelper.GetUniqueFieldValueByDataStatistics(xzqjxClass, "JXLX", m_RealCtPolygon);
                foreach (string sCode in arJxlx)
                {
                    string sTIF = "";
                    if (sCode.Equals("620200") == true)
                        sTIF = "国界.tif";
                    else if (sCode.Equals("630200") == true)
                        sTIF = "省、自治区、直辖市界.tif";
                    else if (sCode.Equals("640200") == true)
                        sTIF = "地区、州、地级市界.tif";
                    else if (sCode.Equals("650200") == true)
                        sTIF = "县、区、县级市界.tif";
                    else if (sCode.Equals("660200") == true)
                        sTIF = "乡、镇、街道界.tif";
                    else if (sCode.Equals("670500") == true)
                        sTIF = "村界.tiff";
                    if (sTIF != "")
                    {
                        if (!OtherSz.Contains(sTIF))
                        {
                            OtherSz.Add(sTIF);
                        }
                    }
                }
            }

            IFeatureLayer DgxLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "DGX");
            if (DgxLayer != null)
            {
                IFeatureClass dgxClass = DgxLayer.FeatureClass;
                ArrayList arJxlx = FeatureHelper.GetUniqueFieldValueByDataStatistics(dgxClass, "DGXLX", m_RealCtPolygon);
                foreach (string sCode in arJxlx)
                {
                    string sTIF = "";
                    if (sCode.Equals("710101") == true)
                        sTIF = "首曲线.tif";
                    else if (sCode.Equals("710102") == true)
                        sTIF = "计曲线.tif";
                    if (!OtherSz.Contains(sTIF) && sTIF != "")
                    {
                        OtherSz.Add(sTIF);
                    }
                }
            }

            IFeatureLayer clkzdLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "CLKZD");
            if (DgxLayer != null)
            {
                IFeatureClass clkzdClass = DgxLayer.FeatureClass;
                if (clkzdClass.FindField("KZDLX") > -1)
                {
                    ArrayList arKzdlx = FeatureHelper.GetUniqueFieldValueByDataStatistics(clkzdClass, "KZDLX", m_RealCtPolygon);
                    foreach (string sCode in arKzdlx)
                    {
                        string sTIF = "";
                        if (sCode.Equals("110102") == true)
                            sTIF = "三角点.tif";
                        else if (sCode.Equals("110200") == true)
                            sTIF = "高程点.tif";
                        if (!OtherSz.Contains(sTIF) && sTIF != "")
                        {
                            OtherSz.Add(sTIF);
                        }
                    }
                }
            }
            return OtherSz;
        }

        /// <summary>
        /// 查找所有TIF文件代表的图例名
        /// </summary>
        /// <returns></returns>
        private ArrayList GetAllLegendInfo()
        {
            ArrayList pLegendFiles = new ArrayList();
            DirectoryInfo legendDir = new DirectoryInfo(Application.StartupPath + @"\图例");
            FileInfo[] legendFileNames = legendDir.GetFiles();
            foreach (FileInfo curFile in legendFileNames)
            {
                string sFileName = curFile.Name;
                int nPos = sFileName.IndexOf(".");
                string sExt = sFileName.Substring(nPos + 1).Trim().ToUpper();
                if (sExt.Equals("TIF") == true)
                {
                    pLegendFiles.Add(sFileName);	//不含路径
                }
            }
            return pLegendFiles;
        }



        /// <summary>
        /// 密级和官图单位
        /// </summary>
        /// <param name="LDP">左下角坐标</param>
        /// <param name="RUP">右上角坐标</param>
        private void outMjGTDW(IPoint LDP, IPoint RUP)
        {
            double dCTDW_ZG = 8.0;
            string sCTDW = "**市土地管理局";
            double dCTDW_GAP = 13.0;

            double dMM_ZG = 4.5;
            string sMM = "秘密: ";
            double dMM_GAP = 13.0;

            sCTDW = this.txtGTDW.Text.Trim();
            sMM = this.txtMj.Text.Trim();
            try
            {
                IGraphicsContainer pageCon = m_PageControl.GraphicsContainer;

                PointClass NeedP = new PointClass();
                NeedP.X = LDP.X - dCTDW_GAP;
                NeedP.Y = LDP.Y + (dCTDW_ZG * 2);

                Font dotNetFont = new Font("黑体", 1, FontStyle.Regular);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = dCTDW_ZG / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                ((ICharacterOrientation)textSymbol).CJKCharactersRotation = true;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sCTDW;
                IElement element = (IElement)textEle;
                element.Geometry = NeedP;
                pageCon.AddElement(element, 0);
                ITransform2D trans = (ITransform2D)element;
                trans.Rotate(NeedP, -Math.PI / 2);
                IEnvelope aBound = new EnvelopeClass();
                element.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, aBound);

                IPoint newRDP = new PointClass();
                newRDP.X = aBound.XMax;
                newRDP.Y = aBound.YMin;
                trans.Move(NeedP.X - newRDP.X, NeedP.Y - newRDP.Y);

                NeedP.X = RUP.X - (dMM_ZG * 4);
                NeedP.Y = RUP.Y + dMM_GAP;

                dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Regular);
                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = dMM_ZG / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sMM;
                element = (IElement)textEle;
                element.Geometry = NeedP;
                pageCon.AddElement(element, 0);
            }
            catch (Exception E)
            {
                MessageBox.Show("错误:" + E.Message);
                return;
            }
        }


        /// <summary>
        /// 密级和官图单位
        /// </summary>
        /// <param name="LDP">左下角坐标</param>
        /// <param name="RUP">右上角坐标</param>
        private void outMjGTDW(IPoint LDP, IPoint RUP,bool isFF)
        {
            double dCTDW_ZG = 6.0;
            string sCTDW = "**市土地管理局";
            double dCTDW_GAP = 13.0;

            double dMM_ZG = 3.5;
            string sMM = "秘密: ";
            double dMM_GAP = 13.0;

            sCTDW = this.txtGTDW.Text.Trim();
            sMM = this.txtMj.Text.Trim();
            try
            {
                IGraphicsContainer pageCon = m_PageControl.GraphicsContainer;

                PointClass NeedP = new PointClass();
                NeedP.X = LDP.X - dCTDW_GAP;
                NeedP.Y = LDP.Y ;
                //NeedP.Y = LDP.Y + (dCTDW_ZG);

                Font dotNetFont = new Font("黑体", 1, FontStyle.Regular);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = dCTDW_ZG / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                ((ICharacterOrientation)textSymbol).CJKCharactersRotation = true;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sCTDW;
                IElement element = (IElement)textEle;
                element.Geometry = NeedP;
                pageCon.AddElement(element, 0);
                ITransform2D trans = (ITransform2D)element;
                trans.Rotate(NeedP, -Math.PI / 2);
                IEnvelope aBound = new EnvelopeClass();
                element.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, aBound);

                IPoint newRDP = new PointClass();
                newRDP.X = aBound.XMax;
                newRDP.Y = aBound.YMin;
                trans.Move(NeedP.X - newRDP.X, NeedP.Y - newRDP.Y);

                NeedP.X = RUP.X ;
                //NeedP.X = RUP.X - (dMM_ZG);
                NeedP.Y = RUP.Y + dMM_GAP;

                dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Regular);
                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = dMM_ZG / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sMM;
                element = (IElement)textEle;
                element.Geometry = NeedP;
                pageCon.AddElement(element, 0);
            }
            catch (Exception E)
            {
                MessageBox.Show("错误:" + E.Message);
                return;
            }
        }

        /// <summary>
        /// 生成注记
        /// </summary>
        private void SetSymboZJ()
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            if (this.dltbLayer != null)
            {
                if (this.chkDltbZJ.Checked)
                {
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = m_RealCtPolygon;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    pSF.WhereClause = " TBMJ  > " + txtDltbMjRX.Text;

                    int iCount = this.dltbLayer.FeatureClass.FeatureCount(pSF as IQueryFilter);
                    bool isLarge = iCount > 5000 ? true : false;
                    if (this.chkDltbZJ.Checked && !isLarge)
                    {
                        SymbolizeDLTBZJ(pageCon, this.dltbLayer, m_RealCtPolygon);
                    }
                }
                if (this.chkTBXMC.Checked)
                {
                    SymbolizeTBXHMCZJ(pageCon, this.dltbLayer, m_RealCtPolygon);
                }

            }

            if (this.xzqLayer != null)
            {
                if (this.chkXZQZJ.Checked)
                {
                    SymbolizeXZQZJ(pageCon, this.xzqLayer, m_bufferPolygon);
                }

            }

            if (this.pzwjstdLayer != null)
            {
                if (this.chkPZWJSXM.Checked)
                {
                    SymbolizePzwjstdZJ(pageCon, this.pzwjstdLayer, m_RealCtPolygon);
                }
            }
            if (this.czcdydLayer != null)
            {
                if (this.chkCzcDM.Checked)
                {
                    SymbolizeCZCDMZJ(pageCon, this.czcdydLayer, m_RealCtPolygon);
                }
            }
        }

        /// <summary>
        /// 出图外框
        /// </summary>
        private void OutOutFrame(double dScale)
        {


            double dFrameWidth = m_bufferPolygon.Envelope.Width / dScale * 1000.0;
            double dFrameHeight = m_bufferPolygon.Envelope.Height / dScale * 1000.0;

            //页边距
            double dTop = pageMargein, dLeft = pageMargein;
            double dRight = pageMargein, dBottom = pageMargein;
            double dPageWidth = dFrameWidth + dLeft + dRight;
            double dPageHeight = dFrameHeight + dTop + dBottom;



            IPage page = m_PageControl.Page;
            page.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriMillimeters;
            page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;
            page.FormID = esriPageFormID.esriPageFormCUSTOM;
            page.PutCustomSize(dPageWidth, dPageHeight);  //设置纸张大小

            //背景色
            IColor PageBackColor = ColorHelper.CreateColor(255, 255, 255);
            page.BackgroundColor = PageBackColor;

            IEnvelope Env = ((IElement)m_myMapFrame).Geometry.Envelope;
            double dX1 = Env.XMin;
            double dY1 = Env.YMin;
            double dX3 = Env.XMax;
            double dY3 = Env.YMax;
            double dCurHeight = Env.Height;
            double dCurWidth = Env.Width;

            IPoint curPoint = new PointClass();
            curPoint.PutCoords(dX1, dY1);
            IPoint toPoint = new PointClass();
            toPoint.PutCoords(dLeft, dBottom);

            ITransform2D trans = (ITransform2D)m_myMapFrame;
            trans.Move(toPoint.X - curPoint.X, toPoint.Y - curPoint.Y);
            double dWScale = dFrameWidth / dCurWidth;
            double dHScale = dFrameHeight / dCurHeight;
            trans.Scale(toPoint, dWScale, dHScale);



            ISymbolBorder border = new SymbolBorderClass();
            SimpleLineSymbolClass lineSym = new SimpleLineSymbolClass();
            lineSym.Width = 0.01;
            lineSym.Style = esriSimpleLineStyle.esriSLSNull;
            IColor Color = ColorHelper.CreateColor(255, 0, 0, 0);
            lineSym.Color = Color;
            border.LineSymbol = lineSym;
            m_myMapFrame.Border = (IBorder)border;

            ISymbolBackground back = new SymbolBackgroundClass();
            SimpleFillSymbolClass fillSym = new SimpleFillSymbolClass();
            fillSym.Style = esriSimpleFillStyle.esriSFSSolid;
            fillSym.Color = PageBackColor;
            lineSym = new SimpleLineSymbolClass();
            lineSym.Style = esriSimpleLineStyle.esriSLSNull;
            fillSym.Outline = lineSym;
            back.FillSymbol = fillSym;
            m_myMapFrame.Background = back;
            if (!cmbCTFW.SelectedItem.ToString().Equals("标准分幅"))
            {
                m_PageControl.ActiveView.FocusMap.ClipGeometry = m_RealCtPolygon;
                m_PageControl.ActiveView.Refresh();
            }
        }



        /// <summary>
        /// 输出一个村图
        /// </summary>
        private void OutACunT(IGeometry xzqGeo, double dBufferJl, double dScale, string destDir, double DPI, string TJType = "")
        {
            sycCommonFuns CommonClassDLL = new sycCommonFuns();

            IMap myMap = this.m_MapControl.ActiveView.FocusMap;
            this.m_RealCtPolygon = xzqGeo;
            this.m_bufferPolygon = (m_RealCtPolygon as ITopologicalOperator).Buffer(dBufferJl);
            this.m_MapControl.Extent = m_bufferPolygon.Envelope;

            this.m_MapControl.Map.ClearSelection();
            this.m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.m_MapControl.ActiveView.Extent.Envelope);
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            deleteAllElement(pageCon);

            m_PageControl.ActiveView.Extent = m_bufferPolygon.Envelope;  //确定当前区域            

            OutOutFrame(dScale);

            this.m_PageControl.ActiveView.Activate(this.m_PageControl.hWnd);
            this.m_PageControl.ActiveView.FocusMap.MapScale = dScale;
            IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
            IActiveView pageAct = m_PageControl.ActiveView;
            IPointCollection pCol = m_bufferPolygon as IPointCollection;

            SetSymboZJ();
            #region 行政区图
            //乡村图:        
            IEnvelope env = ((IGeometry)this.m_bufferPolygon).Envelope;
            IPoint LDP = new PointClass();
            LDP.PutCoords(env.XMin, env.YMin);
            IPoint RDP = new PointClass();
            RDP.PutCoords(env.XMax, env.YMin);
            IPoint RUP = new PointClass();
            RUP.PutCoords(env.XMax, env.YMax);
            IPoint LUP = new PointClass();
            LUP.PutCoords(env.XMin, env.YMax);

            double dJ1 = 0.0, dW1 = 0.0;
            IPoint jwdPt1 = CoordinateTransHelper.XY2JWD(myMap, LDP);
            dJ1 = jwdPt1.X;
            dW1 = jwdPt1.Y;
            double dJ2 = 0.0, dW2 = 0.0;
            IPoint jwdPt2 = CoordinateTransHelper.XY2JWD(myMap, RDP);
            dJ2 = jwdPt2.X;
            dW2 = jwdPt2.Y;
            double dJ3 = 0.0, dW3 = 0.0;
            IPoint jwdPt3 = CoordinateTransHelper.XY2JWD(myMap, RUP);
            dJ3 = jwdPt3.X;
            dW3 = jwdPt3.Y;
            double dJ4 = 0.0, dW4 = 0.0;
            IPoint jwdPt4 = CoordinateTransHelper.XY2JWD(myMap, LUP);
            dJ4 = jwdPt4.X;
            dW4 = jwdPt4.Y;
            double dMaxJ12 = Math.Max(dJ1, dJ2);
            double dMaxJ34 = Math.Max(dJ3, dJ4);
            double dMaxJ = Math.Max(dMaxJ12, dMaxJ34);
            double dMaxW12 = Math.Max(dW1, dW2);
            double dMaxW34 = Math.Max(dW3, dW4);
            double dMaxW = Math.Max(dMaxW12, dMaxW34);
            double dMinJ12 = Math.Min(dJ1, dJ2);
            double dMinJ34 = Math.Min(dJ3, dJ4);
            double dMinJ = Math.Min(dMinJ12, dMinJ34);
            double dMinW12 = Math.Min(dW1, dW2);
            double dMinW34 = Math.Min(dW3, dW4);
            double dMinW = Math.Min(dMinW12, dMinW34);

            m_dJ1 = dMinJ;
            m_dW1 = dMinW;
            m_dJ3 = dMaxJ;
            m_dW3 = dMaxW;


            IPoint NKP1 = new PointClass();
            IPoint NKP2 = new PointClass();
            IPoint NKP3 = new PointClass();
            IPoint NKP4 = new PointClass();
            IPoint jwdPt = new PointClass();

            NKP1 = m_bufferPolygon.Envelope.LowerLeft;
            NKP2 = m_bufferPolygon.Envelope.LowerRight;
            NKP3 = m_bufferPolygon.Envelope.UpperRight;
            NKP4 = m_bufferPolygon.Envelope.UpperLeft;

            int nX = 0, nY = 0;
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP1, out nX, out nY);
            IPoint newP1 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            NKP1.PutCoords(newP1.X, newP1.Y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP2, out nX, out nY);
            IPoint newP2 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            NKP2.PutCoords(newP2.X, newP2.Y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP3, out nX, out nY);
            IPoint newP3 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            NKP3.PutCoords(newP3.X, newP3.Y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP4, out nX, out nY);
            IPoint newP4 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            NKP4.PutCoords(newP4.X, newP4.Y);
            #endregion

            #region 内框
            PolylineClass NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            object Missing = Type.Missing;
            pCol.AddPoint(NKP1, ref Missing, ref Missing);
            pCol.AddPoint(NKP2, ref Missing, ref Missing);
            pCol.AddPoint(NKP3, ref Missing, ref Missing);
            pCol.AddPoint(NKP4, ref Missing, ref Missing);
            pCol.AddPoint(NKP1, ref Missing, ref Missing);

            SimpleLineSymbolClass lineSym = new SimpleLineSymbolClass();
            lineSym.Width = 0.2;
            IColor eleColor = ColorHelper.CreateColor(0, 0, 0);
            lineSym.Color = eleColor;
            LineElementClass LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);

            ILine pLine = new LineClass();

            pLine.FromPoint = NKP1;
            pLine.ToPoint = NKP2;
            PointClass tmpP1 = new PointClass();
            ((IConstructPoint)tmpP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
            PointClass tmpP2 = new PointClass();
            double dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP2);
            ((IConstructPoint)tmpP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

            pLine.FromPoint = NKP4;
            pLine.ToPoint = NKP3;
            PointClass tmpP4 = new PointClass();
            ((IConstructPoint)tmpP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
            PointClass tmpP3 = new PointClass();
            dLen = CommonClassDLL.syc_CalLength(ref NKP3, ref NKP4);
            ((IConstructPoint)tmpP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

            pLine.FromPoint = NKP1;
            pLine.ToPoint = NKP4;
            PointClass tmpPP1 = new PointClass();
            ((IConstructPoint)tmpPP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
            PointClass tmpPP4 = new PointClass();
            dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP4);
            ((IConstructPoint)tmpPP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

            pLine.FromPoint = NKP2;
            pLine.ToPoint = NKP3;
            PointClass tmpPP2 = new PointClass();
            ((IConstructPoint)tmpPP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
            PointClass tmpPP3 = new PointClass();
            dLen = CommonClassDLL.syc_CalLength(ref NKP2, ref NKP3);
            ((IConstructPoint)tmpPP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

            double dAF1 = CommonClassDLL.syc_CalAngle(ref tmpP4, ref tmpP1);
            double dAF2 = CommonClassDLL.syc_CalAngle(ref tmpPP2, ref tmpPP1);
            IPoint WKP1 = new PointClass();
            ((IConstructPoint)WKP1).ConstructAngleIntersection(tmpP1, dAF1, tmpPP1, dAF2);

            dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP1, ref tmpPP2);
            dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP3, ref tmpP2);
            IPoint WKP2 = new PointClass();
            ((IConstructPoint)WKP2).ConstructAngleIntersection(tmpPP2, dAF1, tmpP2, dAF2);

            dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP4, ref tmpPP3);
            dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP2, ref tmpP3);
            IPoint WKP3 = new PointClass();
            ((IConstructPoint)WKP3).ConstructAngleIntersection(tmpPP3, dAF1, tmpP3, dAF2);

            dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP3, ref tmpPP4);
            dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP1, ref tmpP4);

            IPoint WKP4 = new PointClass();
            ((IConstructPoint)WKP4).ConstructAngleIntersection(tmpPP4, dAF1, tmpP4, dAF2);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(WKP1, ref Missing, ref Missing);
            pCol.AddPoint(WKP2, ref Missing, ref Missing);
            pCol.AddPoint(WKP3, ref Missing, ref Missing);
            pCol.AddPoint(WKP4, ref Missing, ref Missing);
            pCol.AddPoint(WKP1, ref Missing, ref Missing);

            lineSym = new SimpleLineSymbolClass();
            lineSym.Width = 0.2;
            eleColor = ColorHelper.CreateColor(0, 0, 0);
            lineSym.Color = eleColor;
            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);

            Ring ring = new RingClass();
            for (int i = 0; i < pCol.PointCount; i++)
            {
                ring.AddPoint(pCol.Point[i], Type.Missing, Type.Missing);
            }
            IGeometryCollection pointPolygon = new PolygonClass();
            pointPolygon.AddGeometry(ring as IGeometry, Type.Missing, Type.Missing);
            IPolygon pNKGeo = pointPolygon as IPolygon;
            #endregion

            //添加花边
            CreateNeatline(pNKGeo);

            #region 经纬度
            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP1, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpP1, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dW1.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = NKP1;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = NKP1;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP1, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpPP1, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dJ1.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = tmpPP1;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = tmpPP1;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP2, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpP2, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dW1.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = NKP2;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = NKP2;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP2, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpPP2, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dJ3.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = tmpPP2;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = tmpPP2;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP3, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpP3, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dW3.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = NKP3;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = NKP3;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP3, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpPP3, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dJ3.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = tmpPP3;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = tmpPP3;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP4, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpP4, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dW3.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = NKP4;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = NKP4;
                pageCon.AddElement(element, 0);
            }

            if (true)
            {
                object o = Type.Missing;
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(NKP4, ref o, ref o);
                ((IPointCollection)pol).AddPoint(tmpPP4, ref o, ref o);
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                string sJWD = m_dJ1.ToString("F10");
                int nPos = sJWD.IndexOf(".");
                string sD = sJWD.Substring(0, nPos);
                string sF = sJWD.Substring(nPos + 1, 2);
                string sM = sJWD.Substring(nPos + 3, 6);
                double dM = Convert.ToDouble(sM) / 10000.0;
                if (Math.Abs(dM - 60.0) < 0.1)
                {
                    sM = "00";
                    int nF = Convert.ToInt32(sF) + 1;
                    int nD = Convert.ToInt32(sD);
                    if (Math.Abs(nF - 60) < 0.01)
                        nD = nD + 1;
                    sF = nF.ToString("D02");
                    sD = nD.ToString();
                }
                else
                {
                    sM = sM.Substring(0, 2);
                }
                sD = sD + "°";
                sF = sF + "'";
                sM = sM + "\"";

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sD;
                IElement element = (IElement)textEle;
                element.Geometry = tmpPP4;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.20;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = sF + sM;
                element = (IElement)textEle;
                element.Geometry = tmpPP4;
                pageCon.AddElement(element, 0);
            }

            #endregion

            #region 注记、标题、比例尺
            string sBT = this.memoBTZJ.Text + "\r\n" + m_XzqMC + "\r\n" + m_XzqDm;
            IPoint pBTPoint = new PointClass();
            IPoint pZXJPoint = new PointClass();
            IPoint pYXJPoint = new PointClass();
            pBTPoint.X = (pBKPolygon.Envelope.XMin + pBKPolygon.Envelope.XMax) * 0.5;
            pBTPoint.Y = pBKPolygon.Envelope.YMax;
            pZXJPoint.X = pBKPolygon.Envelope.LowerLeft.X;
            pZXJPoint.Y = pBKPolygon.Envelope.LowerLeft.Y;
            pYXJPoint.X = pBKPolygon.Envelope.LowerRight.X;
            pYXJPoint.Y = pBKPolygon.Envelope.LowerRight.Y;
            AddZJTitle(WKP3, WKP4, pBTPoint, pZXJPoint, pYXJPoint, sBT);

            IPoint pScalePoint = new PointClass();
            pScalePoint.X = (pBKPolygon.Envelope.XMin + pBKPolygon.Envelope.XMax) * 0.5;
            pScalePoint.Y = pBKPolygon.Envelope.YMin - 10;
            AddScale(WKP3, WKP4, dScale.ToString(), pScalePoint);
            #endregion

            #region 方里网

            int nXGS = 0;
            FLW[] pXFLW = new FLW[500];
            int nBJ = 0;
            int nLastX_KM = 0;
            IPoint LastP = new PointClass();
            while (true)
            {
                FLW flw = new FLW();
                if (nBJ == 0)
                {
                    nBJ++;

                    #region  //第一次:
                    nX = (int)(LDP.X / 1000.0);
                    int nX2 = nX + 1;
                    double dDel = (nX2 * 1000.0 - LDP.X) / dScale * 1000.0;

                    pLine = new LineClass();
                    pLine.FromPoint = NKP1;
                    pLine.ToPoint = NKP2;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP2.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP4.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nX2.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pXFLW[nXGS] = flw;
                    nXGS++;

                    nLastX_KM = nX2;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    #endregion
                }
                else
                {
                    #region   //非第一次:
                    int nCurX_KM = nLastX_KM + 1;
                    int nDist_KM = 1;
                    double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                    pLine = new LineClass();
                    pLine.FromPoint = LastP;
                    pLine.ToPoint = NKP2;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP2.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP4.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nCurX_KM.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pXFLW[nXGS] = flw;
                    nXGS++;

                    nLastX_KM = nCurX_KM;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    if (flw.PP1.X > NKP2.X)
                    {
                        nXGS = nXGS - 1;
                        break;
                    }
                    #endregion
                }
            } //while(1)

            for (int i = 0; i < nXGS; i++)
            {
                object oo = Type.Missing;
                FLW flw = pXFLW[i] as FLW;

                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                //Text:
                pLine = new LineClass();
                pLine.FromPoint = flw.PP1;
                pLine.ToPoint = flw.PP2;
                IPoint ZJP = new PointClass();
                ((IConstructPoint)ZJP).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, 9.0, false);

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 2.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ1;
                IElement element = (IElement)textEle;
                element.Geometry = ZJP;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ2;
                element = (IElement)textEle;
                element.Geometry = ZJP;
                pageCon.AddElement(element, 0);
            }


            int nSGS = 0;
            FLW[] pSFLW = new FLW[500];
            nBJ = 0;
            nLastX_KM = 0;
            LastP = new PointClass();
            while (true)
            {
                FLW flw = new FLW();
                if (nBJ == 0)
                {
                    nBJ++;

                    #region  //第一次:
                    nX = (int)(LUP.X / 1000.0);
                    int nX2 = nX + 1;
                    double dDel = (nX2 * 1000.0 - LUP.X) / dScale * 1000.0;

                    pLine = new LineClass();
                    pLine.FromPoint = NKP4;
                    pLine.ToPoint = NKP3;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP2.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP4.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nX2.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pSFLW[nSGS] = flw;
                    nSGS++;

                    nLastX_KM = nX2;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    #endregion

                }
                else
                {
                    #region  //非第一次:
                    int nCurX_KM = nLastX_KM + 1;
                    int nDist_KM = 1;
                    double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                    pLine = new LineClass();
                    pLine.FromPoint = LastP;
                    pLine.ToPoint = NKP3;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP2.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP4.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nCurX_KM.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pSFLW[nSGS] = flw;
                    nSGS++;

                    nLastX_KM = nCurX_KM;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    if (flw.PP1.X > NKP3.X)
                    {
                        nSGS = nSGS - 1;
                        break;
                    }
                    #endregion

                }
            } //while(1)

            for (int i = 0; i < nSGS; i++)
            {
                object oo = Type.Missing;
                FLW flw = pSFLW[i] as FLW;

                //线:PP1-PP2
                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                //Text:
                pLine = new LineClass();
                pLine.FromPoint = flw.PP1;
                pLine.ToPoint = flw.PP2;
                IPoint ZJP = new PointClass();
                ((IConstructPoint)ZJP).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, 11.0, false);

                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 2.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ1;
                IElement element = (IElement)textEle;
                element.Geometry = ZJP;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ2;
                element = (IElement)textEle;
                element.Geometry = ZJP;
                pageCon.AddElement(element, 0);
            }


            int nZGS = 0;
            FLW[] pZFLW = new FLW[500];
            nBJ = 0;
            int nLastY_KM = 0;
            LastP = new PointClass();
            while (true)
            {
                FLW flw = new FLW();
                if (nBJ == 0)
                {
                    nBJ++;

                    //第一次:
                    nY = (int)(LDP.Y / 1000.0);
                    int nY2 = nY + 1;
                    double dDel = (nY2 * 1000.0 - LDP.Y) / dScale * 1000.0;

                    pLine = new LineClass();
                    pLine.FromPoint = NKP1;
                    pLine.ToPoint = NKP4;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP4.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP2.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nY2.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pZFLW[nZGS] = flw;
                    nZGS++;

                    nLastY_KM = nY2;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                }
                else
                {
                    //非第一次:
                    int nCurY_KM = nLastY_KM + 1;
                    int nDist_KM = 1;
                    double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                    pLine = new LineClass();
                    pLine.FromPoint = LastP;
                    pLine.ToPoint = NKP4;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP4.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP2.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nCurY_KM.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pZFLW[nZGS] = flw;
                    nZGS++;

                    nLastY_KM = nCurY_KM;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    if (flw.PP1.Y > NKP4.Y)
                    {
                        nZGS = nZGS - 1;
                        break;
                    }
                }
            } //while(1)

            for (int i = 0; i < nZGS; i++)
            {
                object oo = Type.Missing;
                FLW flw = pZFLW[i] as FLW;

                //线:PP1-PP2
                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                //Text:
                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 2.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ1;
                IElement element = (IElement)textEle;
                element.Geometry = flw.PP2;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ2;
                element = (IElement)textEle;
                element.Geometry = flw.PP1;
                pageCon.AddElement(element, 0);
            }

            int nYGS = 0;
            FLW[] pYFLW = new FLW[500];
            nBJ = 0;
            nLastY_KM = 0;
            LastP = new PointClass();
            while (true)
            {
                FLW flw = new FLW();
                if (nBJ == 0)
                {
                    nBJ++;

                    //第一次:
                    nY = (int)(RDP.Y / 1000.0);
                    int nY2 = nY + 1;
                    double dDel = (nY2 * 1000.0 - RDP.Y) / dScale * 1000.0;

                    pLine = new LineClass();
                    pLine.FromPoint = NKP2;
                    pLine.ToPoint = NKP3;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP4.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP2.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nY2.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pYFLW[nYGS] = flw;
                    nYGS++;

                    nLastY_KM = nY2;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                }
                else
                {
                    //非第一次:
                    int nCurY_KM = nLastY_KM + 1;
                    int nDist_KM = 1;
                    double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                    pLine = new LineClass();
                    pLine.FromPoint = LastP;
                    pLine.ToPoint = NKP3;
                    IPoint PP1 = new PointClass();
                    ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                    flw.PP1 = PP1;

                    IPoint p1 = new PointClass();
                    p1.X = (NKP1.X + NKP4.X) * 0.5;
                    p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                    IPoint p2 = new PointClass();
                    p2.X = (NKP3.X + NKP2.X) * 0.5;
                    p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                    double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                    IPoint PP2 = new PointClass();
                    ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                    flw.PP2 = PP2;

                    string sBS = nCurY_KM.ToString();
                    int nLen = sBS.Length;
                    string sZJ1 = sBS.Substring(0, nLen - 2);
                    string sZJ2 = sBS.Substring(nLen - 2, 2);
                    flw.sZJ1 = sZJ1;
                    flw.sZJ2 = sZJ2;
                    flw.sBS = sBS;

                    pYFLW[nYGS] = flw;
                    nYGS++;

                    nLastY_KM = nCurY_KM;
                    LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                    if (flw.PP1.Y > NKP3.Y)
                    {
                        nYGS = nYGS - 1;
                        break;
                    }
                }
            } //while(1)

            for (int i = 0; i < nYGS; i++)
            {
                object oo = Type.Missing;
                FLW flw = pYFLW[i] as FLW;

                //线:PP1-PP2
                PolylineClass pol = new PolylineClass();
                ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                lineSym = new SimpleLineSymbolClass();
                lineSym.Width = 0.1;
                eleColor = ColorHelper.CreateColor(0, 0, 0);
                lineSym.Color = eleColor;
                LineEle = new LineElementClass();
                LineEle.Geometry = pol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);

                //Text:
                System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                double dZH = 2.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ1;
                IElement element = (IElement)textEle;
                element.Geometry = flw.PP1;
                pageCon.AddElement(element, 0);

                textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                dZH = 3.0;	//mm
                textSymbol.Size = dZH / 25.4 * 72.0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = flw.sZJ2;
                element = (IElement)textEle;
                element.Geometry = flw.PP2;
                pageCon.AddElement(element, 0);
            }


            if (chkTKFLW.Checked == false)
            {
                #region 方里网注记
                ArrayList LeftRightLines = new ArrayList();
                for (int i = 0; i < nZGS; i++)
                {
                    string sLeftBS = pZFLW[i].sBS;
                    bool bExist = false;
                    int nIndex = -1;
                    for (int j = 0; j < nYGS; j++)
                    {
                        if (pYFLW[j].sBS.Equals(sLeftBS) == true)
                        {
                            bExist = true;
                            nIndex = j;
                            break;
                        }
                    }
                    if (bExist == true)
                    {
                        object oo = Type.Missing;
                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(pZFLW[i].PP1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(pYFLW[nIndex].PP1, ref oo, ref oo);
                        LeftRightLines.Add(pol);
                    }
                }

                ArrayList UpperDownLines = new ArrayList();
                for (int i = 0; i < nXGS; i++)
                {
                    string sDownBS = pXFLW[i].sBS;
                    bool bExist = false;
                    int nIndex = -1;
                    for (int j = 0; j < nSGS; j++)
                    {
                        if (pSFLW[j].sBS.Equals(sDownBS) == true)
                        {
                            bExist = true;
                            nIndex = j;
                            break;
                        }
                    }
                    if (bExist == true)
                    {
                        object oo = Type.Missing;
                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(pXFLW[i].PP1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(pSFLW[nIndex].PP1, ref oo, ref oo);
                        UpperDownLines.Add(pol);
                    }
                }

                int nGS1 = LeftRightLines.Count;
                for (int i = 0; i < nGS1; i++)
                {
                    IPolyline LRLine = LeftRightLines[i] as IPolyline;
                    IPointCollection pCol1 = LRLine as IPointCollection;
                    IPoint P1 = pCol1.get_Point(0);
                    IPoint P2 = pCol1.get_Point(1);
                    double dA1 = CommonClassDLL.syc_CalAngle(ref P1, ref P2);

                    int nGS2 = UpperDownLines.Count;
                    for (int j = 0; j < nGS2; j++)
                    {
                        IPolyline UDLine = UpperDownLines[j] as IPolyline;
                        IPointCollection pCol2 = UDLine as IPointCollection;
                        IPoint PP1 = pCol2.get_Point(0);
                        IPoint PP2 = pCol2.get_Point(1);
                        double dA2 = CommonClassDLL.syc_CalAngle(ref PP1, ref PP2);

                        IPoint JP = new PointClass();
                        ((IConstructPoint)JP).ConstructAngleIntersection(P1, dA1, PP1, dA2);	//Intersection

                        //以JP为中心,dA1+dA2产生4点:
                        IPoint p1 = new PointClass();
                        ((IConstructPoint)p1).ConstructAngleDistance(JP, dA1, 5.0);
                        IPoint p2 = new PointClass();
                        ((IConstructPoint)p2).ConstructAngleDistance(JP, dA1 + Math.PI, 5.0);
                        IPoint p3 = new PointClass();
                        ((IConstructPoint)p3).ConstructAngleDistance(JP, dA2, 5.0);
                        IPoint p4 = new PointClass();
                        ((IConstructPoint)p4).ConstructAngleDistance(JP, dA2 + Math.PI, 5.0);

                        //线:p1-p2;p3-p4
                        object oo = Type.Missing;
                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(p1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(p2, ref oo, ref oo);
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(p3, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(p4, ref oo, ref oo);
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);
                    }
                }
                #endregion

            }
            #endregion

            //添加图例
            if (!string.IsNullOrEmpty(cboTLLocation.SelectedItem.ToString()))
            {
                AddLegendNB(NKP2, NKP3, NKP4, pCol);
            }
            if (!string.IsNullOrEmpty(cboZBZLocation.SelectedItem.ToString()))
            {
                IEnvelope pZBZEnv = new EnvelopeClass();
                IPolygon pZBZBK = StructPolygon(cboZBZLocation.SelectedItem.ToString(), 30, 50);

                //让指北针位置在外框中心位置
                pZBZEnv.PutCoords(pZBZBK.Envelope.XMin + pZBZBK.Envelope.Width / 2, pZBZBK.Envelope.YMin + pZBZBK.Envelope.Height / 2, pZBZBK.Envelope.XMin + pZBZBK.Envelope.Width / 2, pZBZBK.Envelope.YMin + pZBZBK.Envelope.Height / 2);
                //添加指北针
                AddNorthArrow(m_myMapFrame, pZBZEnv);
            }
            //添加面积汇总表
            if (!string.IsNullOrEmpty(cboHZBLocation.SelectedItem.ToString()))
            {
                AddHZB(false, m_XzqDm);
            }
            //添加行政区缩略图
            if (!string.IsNullOrEmpty(cboXZQLocation.SelectedItem.ToString()))
            {
                AddXZQLayer(m_XzqDm, TJType);
            }
            outMjGTDW(pBKPolygon.Envelope.LowerLeft, pBKPolygon.Envelope.UpperRight);

            this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
            IGraphicsContainerSelect pGCSelect = this.m_PageControl.PageLayout as IGraphicsContainerSelect;
            pGCSelect.UnselectAllElements();

            ICommand myTool = new ControlsMapPanToolClass();
            myTool.OnCreate(this.m_MapControl.Object);
            this.m_MapControl.CurrentTool = myTool as ITool;
            m_bufferPolygon = null;
            m_RealCtPolygon = null;

            m_PageControl.ZoomToWholePage();
            m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);

            string sType = cmbOutType.SelectedItem.ToString().Trim();

            if (string.IsNullOrEmpty(sType) || sType.Equals("MXD"))   //生成arcmap文档
            {
                IMxdContents pMxd = m_PageControl.PageLayout as IMxdContents;
                IMapDocument m_MapDocument = new MapDocumentClass();
                string destFile = destDir + @"\" + m_XzqDm + m_XzqMC + "土地利用图" + (iTH < 0 ? "" : (iTH++).ToString()) + ".mxd";
                m_MapDocument.New(destFile);

                m_MapDocument.ReplaceContents(pMxd);
                m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);
                m_MapDocument.Close();
            }
            else  //导出图片
            {
                string destFile = destDir + @"\" + m_XzqDm + m_XzqMC + "土地利用图" + (iTH < 0 ? "" : (iTH++).ToString()) + "." + sType;
                string sRetErrorInfo = "";
                bool bRet = CommonClassDLL.ExportActiveViewParameterized(pageAct, (long)DPI, 1, false, destFile, ref sRetErrorInfo);
            }
            m_PageControl.ActiveView.Deactivate();
            m_MapControl.ActiveView.Refresh();

            CommonClassDLL.Dispose();
        }

        private void getAllXiang(string prefixCode, ref List<string> lstDm, ref List<string> lstMc)
        {
            //获取所有乡以及乡对应代码，名称，从权属代码表中取
            ITable qsdwdmTab = null;
            try
            {
                qsdwdmTab = (RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenTable("QSDWDMB");
            }
            catch { }
            if (qsdwdmTab == null)
            {
                return;
            }

            string sql = "JB=9 and QSDWDM like '" + this.txtPatchXzdm.Text.Trim() + "%'";
            IQueryFilter pQF = new QueryFilter();
            pQF.WhereClause = sql;
            ICursor pCursor = qsdwdmTab.Search(pQF, false);
            IRow aRow = null;
            try
            {
                while ((aRow = pCursor.NextRow()) != null)
                {
                    string dm = FeatureHelper.GetRowValue(aRow, "QSDWDM").ToString();
                    string mc = FeatureHelper.GetRowValue(aRow, "QSDWMC").ToString();
                    if (!lstDm.Contains(dm))
                    {
                        lstDm.Add(dm);
                        lstMc.Add(mc);
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
        }

        //返回乡的图形
        private IGeometry getAXiangGeo(string xiangdm)
        {

            IQueryFilter qur = new QueryFilterClass();
            qur.WhereClause = "\"XZQDM\" like  '" + xiangdm + "%'";
            IFeatureCursor pCur = this.xzqLayer.FeatureClass.Search(qur, false);
            IFeature xfea = pCur.NextFeature();
            if (xfea == null)
                return null;
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
            (uGeo as ITopologicalOperator).Simplify();
            uGeo.Project(this.m_MapControl.ActiveView.FocusMap.SpatialReference);
            return uGeo;
        }

        public bool JWD2XY(IMap myMap, double dJ, double dW, ref IPoint retP, out string sRetErrorInfo)
        {
            sRetErrorInfo = "";

            //根据JWD-->XY
            ISpatialReference pSR = myMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pSR as IProjectedCoordinateSystem;
            IGeographicCoordinateSystem pGCS = pPCS.GeographicCoordinateSystem;
            if (pGCS == null || pPCS == null)
            {
                sRetErrorInfo = "坐标系统有问题，请检查[ErrorCode:01]!";
                return false;
            }


            IPoint PP = new PointClass();
            PP.PutCoords(dJ, dW);
            IGeometry pGeo = PP as IGeometry;
            pGeo.SpatialReference = pGCS;
            pGeo.Project((ISpatialReference)pPCS);

            //double falseX,falseY,xyUnits;
            //falseX=falseY=xyUnits=0.0;
            //pPCS.GetFalseOriginAndUnits(ref falseX,ref falseY,ref xyUnits);
            retP.X = PP.X;
            retP.Y = PP.Y;

            return true;
        }

        private void gluExtent_EditValueChanged(object sender, EventArgs e)
        {
            if (gluExtent.EditValue == null || gluExtent.EditValue.ToString().Trim() == "")
            {
                this.simpleButton1.Enabled = true;
                m_bufferPolygon = null;
                m_RealCtPolygon = null;
            }
            else
            {
                try
                {
                    this.simpleButton1.Enabled = false;


                    string sScale = cmbCTBLC.SelectedItem.ToString();
                    int dScale = Convert.ToInt32(sScale);
                    string ctfw = cmbCTFW.SelectedItem.ToString().Trim();
                    if (ctfw.Equals("标准分幅"))
                    {
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = "TFH = '" + gluExtent.EditValue.ToString().Trim() + "'";
                        IFeatureCursor pCursor = tfhLayer.FeatureClass.Search(pQf, false);
                        IFeature aXzqFea = null;
                        if ((aXzqFea = pCursor.NextFeature()) == null)
                        {
                            throw new Exception("未获取到{" + gluExtent.EditValue.ToString().Trim() + "}数据");
                        }

                        IEnvelope pEnv = aXzqFea.ShapeCopy.Envelope;  //经纬度坐标
                        m_dJ1 = pEnv.XMin;
                        m_dW1 = pEnv.YMin;
                        m_dJ3 = pEnv.XMax;
                        m_dW3 = pEnv.YMax;

                        IPoint pp1 = new PointClass();
                        IPoint pp2 = new PointClass();
                        IPoint pp3 = new PointClass();
                        IPoint pp4 = new PointClass();
                        string sRetErrorInfo = "";

                        JWD2XY(this.m_MapControl.ActiveView.FocusMap, m_dJ1, m_dW1, ref pp1, out sRetErrorInfo);
                        JWD2XY(this.m_MapControl.ActiveView.FocusMap, m_dJ3, m_dW1, ref pp2, out sRetErrorInfo);
                        JWD2XY(this.m_MapControl.ActiveView.FocusMap, m_dJ3, m_dW3, ref pp3, out sRetErrorInfo);
                        JWD2XY(this.m_MapControl.ActiveView.FocusMap, m_dJ1, m_dW3, ref pp4, out sRetErrorInfo);

                        object oo = Type.Missing;
                        PolygonClass tf = new PolygonClass();
                        ((IPointCollection)tf).AddPoint(pp1, ref oo, ref oo);
                        ((IPointCollection)tf).AddPoint(pp2, ref oo, ref oo);
                        ((IPointCollection)tf).AddPoint(pp3, ref oo, ref oo);
                        ((IPointCollection)tf).AddPoint(pp4, ref oo, ref oo);
                        ((IPointCollection)tf).AddPoint(pp1, ref oo, ref oo);

                        PolygonElementClass ele = new PolygonElementClass();
                        ele.Geometry = (IGeometry)tf;

                        m_RealCtPolygon = (IGeometry)ele.Geometry;
                        m_bufferPolygon = (IGeometry)ele.Geometry;
                        this.m_MapControl.Extent = m_RealCtPolygon.Envelope;
                    }
                    else
                    {
                        IFeatureLayer currLayer;
                        string strSql;
                        switch (ctfw)
                        {
                            case "村图": currLayer = cjdcqLayer; strSql = "ZLDWDM = '"; break;
                            default:
                                currLayer = xzqLayer; strSql = "XZQDM = '"; break;

                        }
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = strSql + gluExtent.EditValue.ToString().Trim() + "'";
                        IFeatureCursor pCursor = currLayer.FeatureClass.Search(pQf, false);
                        IFeature aXzqFea = null;
                        if ((aXzqFea = pCursor.NextFeature()) == null)
                        {
                            throw new Exception("未获取到{" + gluExtent.EditValue.ToString().Trim() + "}数据");
                        }

                        double dBufferJl = 0;
                        double.TryParse(this.textBoxTKJL.Text, out dBufferJl);
                        this.m_RealCtPolygon = aXzqFea.ShapeCopy;
                        ITopologicalOperator pTopOpe = m_RealCtPolygon as ITopologicalOperator;
                        pTopOpe.Simplify();
                        try
                        {
                            this.m_bufferPolygon = pTopOpe.Buffer(dBufferJl);
                        }
                        catch (Exception ex)
                        {
                            string aa = ex.ToString();
                        }
                        this.m_MapControl.Extent = m_bufferPolygon.Envelope;
                        IMap myMap = this.m_MapControl.ActiveView.FocusMap;

                        m_XzqDm = gluExtent.EditValue.ToString().Trim();

                        IEnvelope env = ((IGeometry)this.m_bufferPolygon).Envelope;
                        IPoint LDP = new PointClass();
                        LDP.PutCoords(env.XMin, env.YMin);
                        IPoint RDP = new PointClass();
                        RDP.PutCoords(env.XMax, env.YMin);
                        IPoint RUP = new PointClass();
                        RUP.PutCoords(env.XMax, env.YMax);
                        IPoint LUP = new PointClass();
                        LUP.PutCoords(env.XMin, env.YMax);

                        double dJ1 = 0.0, dW1 = 0.0;
                        IPoint jwdPt1 = CoordinateTransHelper.XY2JWD(myMap, LDP);
                        dJ1 = jwdPt1.X;
                        dW1 = jwdPt1.Y;
                        double dJ2 = 0.0, dW2 = 0.0;
                        IPoint jwdPt2 = CoordinateTransHelper.XY2JWD(myMap, RDP);
                        dJ2 = jwdPt2.X;
                        dW2 = jwdPt2.Y;
                        double dJ3 = 0.0, dW3 = 0.0;
                        IPoint jwdPt3 = CoordinateTransHelper.XY2JWD(myMap, RUP);
                        dJ3 = jwdPt3.X;
                        dW3 = jwdPt3.Y;
                        double dJ4 = 0.0, dW4 = 0.0;
                        IPoint jwdPt4 = CoordinateTransHelper.XY2JWD(myMap, LUP);
                        dJ4 = jwdPt4.X;
                        dW4 = jwdPt4.Y;
                        double dMaxJ12 = Math.Max(dJ1, dJ2);
                        double dMaxJ34 = Math.Max(dJ3, dJ4);
                        double dMaxJ = Math.Max(dMaxJ12, dMaxJ34);
                        double dMaxW12 = Math.Max(dW1, dW2);
                        double dMaxW34 = Math.Max(dW3, dW4);
                        double dMaxW = Math.Max(dMaxW12, dMaxW34);
                        double dMinJ12 = Math.Min(dJ1, dJ2);
                        double dMinJ34 = Math.Min(dJ3, dJ4);
                        double dMinJ = Math.Min(dMinJ12, dMinJ34);
                        double dMinW12 = Math.Min(dW1, dW2);
                        double dMinW34 = Math.Min(dW3, dW4);
                        double dMinW = Math.Min(dMinW12, dMinW34);

                        m_dJ1 = dMinJ;
                        m_dW1 = dMinW;
                        m_dJ3 = dMaxJ;
                        m_dW3 = dMaxW;
                    }
                }
                catch (Exception ex)
                {
                    EditorButton btn = new EditorButton(ButtonPredefines.Delete);
                    lue_ButtonClick(gluExtent, new ButtonPressedEventArgs(btn));
                    MessageBox.Show(ex.Message, "整饬输出");
                }
            }
        }

        private string GetScaleDM()
        {
            string sScale = cmbCTBLC.SelectedItem.ToString();
            switch (sScale)
            {
                case "500":
                    return "K";
                case "1000": //1:1000 
                    return "J";
                case "2000":
                    return "I";
                case "5000":
                    return "H";
                case "10000":
                    return "G";
                case "25000":
                    return "F";
                case "50000":
                    return "E";
                case "100000":
                    return "D";
                case "250000":
                    return "C";
                default:
                    return "B";
            }
        }

        private void cmbCTFW_SelectedIndexChanged(object sender, EventArgs e)
        {
            //清空当前选择区域的内容
            DataTable dt = new DataTable();
            dt.Columns.Add("code", typeof(string));
            dt.Columns.Add("name", typeof(string));

            gluExtent.Properties.DataSource = dt;
            gluExtent.Properties.DisplayMember = "name";
            gluExtent.Properties.ValueMember = "code";

            string ctfw = cmbCTFW.SelectedItem.ToString().Trim();

            if (ctfw.Equals("任意区域") || ctfw.Equals("县图"))
            {
                this.gluExtent.Visible = false;
                gluExtent.EditValue = null;
            }
            else
            {
                this.gluExtent.Visible = true;


                if (ctfw.Equals("标准分幅"))
                {
                    //根据比例尺获取分幅图层中的图幅号
                    try
                    {
                        gluExtent.Properties.View.Columns[0].Caption = "图幅号";
                        gluExtent.Properties.View.Columns[1].Caption = "比例尺";
                        if (tfhLayer == null)
                        {
                            return;
                        }
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = "TFH like '%" + GetScaleDM() + "%'";
                        IFeatureCursor pCursor = tfhLayer.FeatureClass.Search(pQf, false);
                        IFeature aTfhFea = null;

                        while ((aTfhFea = pCursor.NextFeature()) != null)
                        {
                            string tfhStr = FeatureHelper.GetFeatureStringValue(aTfhFea, "TFH");
                            if (tfhStr.Substring(3, 1).Equals(GetScaleDM()))
                            {
                                dt.Rows.Add(cmbCTBLC.SelectedItem.ToString(), tfhStr);

                            }
                        }

                        gluExtent.Properties.DataSource = dt;
                        gluExtent.Properties.DisplayMember = "name";
                        gluExtent.Properties.ValueMember = "name";

                    }
                    catch (Exception)
                    {

                        MessageBox.Show("图幅号图层为空或图层加载失败!", "整饬输出");
                    }
                }
                else
                {


                    gluExtent.Properties.View.Columns[0].Caption = "行政区名称";
                    gluExtent.Properties.View.Columns[1].Caption = "行政区代码";
                    IFeatureLayer currLayer;

                    //获取行政区代码和名称
                    switch (ctfw)
                    {
                        case "村图": currLayer = cjdcqLayer; break;
                        default:
                            currLayer = xzqLayer; break;
                    }
                    try
                    {
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = "";

                        IFeatureCursor pCursor = currLayer.FeatureClass.Search(pQf, false);
                        IFeature aXzqFea = null;

                        while ((aXzqFea = pCursor.NextFeature()) != null)
                        {
                            string xzqdm, xzqmc;
                            if (ctfw.Equals("乡图"))
                            {
                                xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQDM");
                                xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "XZQMC");
                            }
                            else
                            {
                                xzqdm = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWDM");
                                xzqmc = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWMC");
                            }
                            dt.Rows.Add(xzqdm, xzqmc);
                        }

                        gluExtent.Properties.DataSource = dt;
                        gluExtent.Properties.DisplayMember = "name";
                        gluExtent.Properties.ValueMember = "code";
                    }
                    catch
                    {
                        MessageBox.Show("加载行政区出错!", "整饬输出");
                    }

                }
            }
        }

        private void cmbCTBLC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCTFW.SelectedItem.ToString().Equals("标准分幅"))
            {
                cmbCTFW_SelectedIndexChanged(sender, e);
            }
        }

        private void btnPatchXiangtu_Click(object sender, EventArgs e)
        {
            //批量出乡图
            if (this.beDestDir.Text.Trim() == "")
            {
                MessageBox.Show("请指定输出路径！");
                return;
            }
            string destDir = this.beDestDir.Text.Trim();
            if (xzqLayer == null)
            {
                MessageBox.Show("请首先加载行政区图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtPatchXzdm.Text.Trim().Length > 9)
            {
                //超出9位，位数不对
                MessageBox.Show("输入行政代码位数超过9位，无法找到下辖乡。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            double DPI = 0;
            if (numericUpDown1.Visible == true)
            {
                try
                {
                    DPI = (double)numericUpDown1.Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("分辨率数值异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            List<string> xiangdms = new List<string>();
            List<string> xiangMcs = new List<string>();

            this.getAllXiang(this.txtPatchXzdm.Text.Trim(), ref xiangdms, ref xiangMcs);
            if (xiangdms.Count == 0)
            {
                MessageBox.Show("找不到权属单位代码表！");
                return;
            }

            if (IsSameLocation())
            {
                MessageBox.Show("要素位置重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //初始化变量            
            string sScale = cmbCTBLC.SelectedItem.ToString();
            double dScale = Convert.ToDouble(sScale);

            double dBufferJl = 0;
            double.TryParse(this.textBoxTKJL.Text, out dBufferJl);
            double dMJL = dBufferJl * dScale / 1000.0;		//M

            IMap myMap = this.m_MapControl.ActiveView.FocusMap;
            m_myMapFrame = this.GetIMapFrame();

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在批量输出村级图...", "请稍等...");
            wait.Show();

            try
            {
                for (int i = 0; i < xiangdms.Count; i++)
                {
                    m_XzqDm = xiangdms[i];
                    m_XzqMC = xiangMcs[i];
                    IGeometry aXiangGeo = getAXiangGeo(m_XzqDm);
                    if (aXiangGeo == null)
                        continue;
                    wait.SetCaption("正在输出" + m_XzqMC + "土地利用图...");
                    OutACunT(aXiangGeo, dBufferJl, dScale, destDir, DPI, "乡图");
                }
                wait.Close();
                m_myTab.SelectedTabPageIndex = 1;
                MessageBox.Show("生成完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                wait.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPatchXzqT_Click(object sender, EventArgs e)
        {
            if (this.beDestDir.Text.Trim() == "")
            {
                MessageBox.Show("请指定输出路径！");
                return;
            }

            double DPI = 0;
            if (numericUpDown1.Visible == true)
            {
                try
                {
                    DPI = (double)numericUpDown1.Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("分辨率数值异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string destDir = this.beDestDir.Text.Trim();

            IMap myMap = this.m_MapControl.ActiveView.FocusMap;
            m_myMapFrame = this.GetIMapFrame();


            if (xzqLayer == null)
            {
                MessageBox.Show("请首先加载行政区图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (IsSameLocation())
            {
                MessageBox.Show("要素位置重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iTH = 1;

            //初始化变量            
            string sScale = cmbCTBLC.SelectedItem.ToString();
            double dScale = Convert.ToDouble(sScale);

            double dBufferJl = 0;
            double.TryParse(this.textBoxTKJL.Text, out dBufferJl);
            double dMJL = dBufferJl * dScale / 1000.0;		//M

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在批量输出村级图...", "请稍等...");
            wait.Show();

            IQueryFilter pQf = null;
            if (this.txtPatchXzdm.Text.Trim() != "")
            {
                pQf = new QueryFilterClass();
                pQf.WhereClause = "ZLDWDM like '" + this.txtPatchXzdm.Text.Trim() + "%'";
            }
            IFeatureCursor pCursor = cjdcqLayer.FeatureClass.Search(pQf, false);
            IFeature aXzqFea = null;
            try
            {
                while ((aXzqFea = pCursor.NextFeature()) != null)
                {
                    m_XzqDm = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWDM");
                    m_XzqMC = FeatureHelper.GetFeatureStringValue(aXzqFea, "ZLDWMC");
                    wait.SetCaption("正在输出" + m_XzqMC + "土地利用图...");

                    OutACunT(aXzqFea.ShapeCopy, dBufferJl, dScale, destDir, DPI, "村图");
                }
                wait.Close();
                OtherHelper.ReleaseComObject(pCursor);
                //OtherHelper.ReleaseComObject(pQf);
                m_myTab.SelectedTabPageIndex = 1;
                MessageBox.Show("输出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.m_MapControl.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
                OtherHelper.ReleaseComObject(pCursor);
            }
        }

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destDirs = dlg.SelectedPath;
            this.beDestDir.Text = destDirs;

        }

        //批量出标准分幅图
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            #region 判断条件
            if (this.beDestDir.Text.Trim() == "")
            {
                MessageBox.Show("请指定输出路径！");
                return;
            }
            //批量标准分幅
            if (this.tfhLayer == null)
            {
                MessageBox.Show("没有加载图幅号图层！");
                return;
            }
            ArrayList allTF = LayerHelper.GetSelectedFeature(this.m_MapControl.ActiveView.FocusMap, this.tfhLayer as IGeoFeatureLayer, esriGeometryType.esriGeometryPolygon);
            if (allTF.Count == 0)
            {
                MessageBox.Show("图幅号图层中没有数据！");
                return;
            }
            double DPI = 0;
            if (numericUpDown1.Visible == true)
            {
                try
                {
                    DPI = (double)numericUpDown1.Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("分辨率数值异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            IFeature firstFea = allTF[0] as IFeature;
            double dScale = -1;
            #region 获得比例尺
            string firstTfh = FeatureHelper.GetFeatureStringValue(firstFea, "TFH");
            if (firstTfh.Length > 4)
            {
                string scale = firstTfh.Substring(3, 1);
                switch (scale)
                {
                    //十万
                    case "D":
                        dScale = 100000;
                        break;
                    //五万
                    case "E":
                        dScale = 50000;
                        break;
                    case "F":
                        dScale = 25000;
                        break;
                    //一万
                    case "G":
                        dScale = 10000;
                        break;
                    case "H":
                        dScale = 5000;
                        break;
                    case "I":
                        dScale = 2000;
                        break;
                    case "J":
                        dScale = 1000;
                        break;
                }
            }
            #endregion
            if (dScale == -1)
            {
                MessageBox.Show("获取不到比例尺，图幅号不正确。");
                return;
            }
            #endregion

            string destDir = this.beDestDir.Text.Trim();
            sycCommonFuns CommonClassDLL = new sycCommonLib.sycCommonFuns();
            IMap myMap = this.m_MapControl.ActiveView.FocusMap;
            m_myMapFrame = this.GetIMapFrame();
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在批量输出图...", "请稍等...");
            wait.Show();

            try
            {
                foreach (IFeature aTF in allTF)
                {
                    #region 基本参数
                    IGeometry aTFGeo = aTF.ShapeCopy;
                    m_RealCtPolygon = aTFGeo;
                    m_bufferPolygon = aTFGeo;
                    m_sTFH = FeatureHelper.GetFeatureStringValue(aTF, "TFH");
                    IEnvelope pEnv = aTFGeo.Envelope;
                    IPoint LD = new PointClass();
                    LD.PutCoords(pEnv.XMin, pEnv.YMin);
                    IPoint RU = new PointClass();
                    RU.PutCoords(pEnv.XMax, pEnv.YMax);
                    string sErrorInfo = "";

                    CommonClassDLL.syc_XY2JWD(this.m_MapControl.ActiveView.FocusMap, LD, out m_dJ1, out m_dW1, out sErrorInfo);
                    CommonClassDLL.syc_XY2JWD(this.m_MapControl.ActiveView.FocusMap, RU, out m_dJ3, out m_dW3, out sErrorInfo);

                    #endregion

                    wait.SetCaption("正在输出" + this.m_sTFH + "分幅图...");
                    Application.DoEvents();

                    this.m_MapControl.Extent = aTFGeo.Envelope;
                    this.m_MapControl.Map.ClearSelection();
                    this.m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.m_MapControl.ActiveView.Extent.Envelope);
                    IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
                    deleteAllElement(pageCon);

                    m_PageControl.ActiveView.Extent = m_bufferPolygon.Envelope;  //确定当前区域               
                    OutOutFrame(dScale);
                    SetSymboZJ();

                    m_PageControl.ActiveView.Activate(m_PageControl.hWnd);
                    m_PageControl.ActiveView.FocusMap.MapScale = dScale;
                    IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
                    IActiveView pageAct = m_PageControl.ActiveView;
                    IPointCollection pCol = m_bufferPolygon as IPointCollection;

                    #region 标准图幅
                    IPoint NKP1 = new PointClass();
                    IPoint NKP2 = new PointClass();
                    IPoint NKP3 = new PointClass();
                    IPoint NKP4 = new PointClass();

                    //四个点，用最大最小值
                    NKP1.PutCoords(pEnv.XMin, pEnv.YMin);
                    NKP2.PutCoords(pEnv.XMax, pEnv.YMin);
                    NKP3.PutCoords(pEnv.XMax, pEnv.YMax);
                    NKP4.PutCoords(pEnv.XMin, pEnv.YMax);

                    EnvelopeClass env = new EnvelopeClass();
                    env.XMin = 0.0;
                    env.XMax = 1.0;
                    env.YMax = 0.0;
                    env.YMax = 1.0;
                    pageAct.Extent = env;
                    pageAct.Refresh();

                    int nX = 0, nY = 0;
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP1, out nX, out nY);
                    IPoint newP1 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP1.PutCoords(newP1.X, newP1.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP2, out nX, out nY);
                    IPoint newP2 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP2.PutCoords(newP2.X, newP2.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP3, out nX, out nY);
                    IPoint newP3 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP3.PutCoords(newP3.X, newP3.Y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP4, out nX, out nY);
                    IPoint newP4 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    NKP4.PutCoords(newP4.X, newP4.Y);
                    #endregion

                    #region 内框:

                    PolylineClass NewPol = new PolylineClass();
                    pCol = (IPointCollection)NewPol;
                    object Missing = Type.Missing;
                    pCol.AddPoint(NKP1, ref Missing, ref Missing);
                    pCol.AddPoint(NKP2, ref Missing, ref Missing);
                    pCol.AddPoint(NKP3, ref Missing, ref Missing);
                    pCol.AddPoint(NKP4, ref Missing, ref Missing);
                    pCol.AddPoint(NKP1, ref Missing, ref Missing);

                    SimpleLineSymbolClass lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.2;
                    IColor eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;
                    LineElementClass LineEle = new LineElementClass();
                    LineEle.Geometry = (IGeometry)NewPol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);

                    ILine pLine = new LineClass();

                    pLine.FromPoint = NKP1;
                    pLine.ToPoint = NKP2;
                    PointClass tmpP1 = new PointClass();
                    ((IConstructPoint)tmpP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                    PointClass tmpP2 = new PointClass();
                    double dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP2);
                    ((IConstructPoint)tmpP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

                    pLine.FromPoint = NKP4;
                    pLine.ToPoint = NKP3;
                    PointClass tmpP4 = new PointClass();
                    ((IConstructPoint)tmpP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                    PointClass tmpP3 = new PointClass();
                    dLen = CommonClassDLL.syc_CalLength(ref NKP3, ref NKP4);
                    ((IConstructPoint)tmpP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

                    pLine.FromPoint = NKP1;
                    pLine.ToPoint = NKP4;
                    PointClass tmpPP1 = new PointClass();
                    ((IConstructPoint)tmpPP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                    PointClass tmpPP4 = new PointClass();
                    dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP4);
                    ((IConstructPoint)tmpPP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);

                    pLine.FromPoint = NKP2;
                    pLine.ToPoint = NKP3;
                    PointClass tmpPP2 = new PointClass();
                    ((IConstructPoint)tmpPP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -12.0, false);
                    PointClass tmpPP3 = new PointClass();
                    dLen = CommonClassDLL.syc_CalLength(ref NKP2, ref NKP3);
                    ((IConstructPoint)tmpPP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 12.0, false);


                    double dAF1 = CommonClassDLL.syc_CalAngle(ref tmpP4, ref tmpP1);
                    double dAF2 = CommonClassDLL.syc_CalAngle(ref tmpPP2, ref tmpPP1);
                    IPoint WKP1 = new PointClass();
                    ((IConstructPoint)WKP1).ConstructAngleIntersection(tmpP1, dAF1, tmpPP1, dAF2);

                    dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP1, ref tmpPP2);
                    dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP3, ref tmpP2);
                    IPoint WKP2 = new PointClass();
                    ((IConstructPoint)WKP2).ConstructAngleIntersection(tmpPP2, dAF1, tmpP2, dAF2);

                    dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP4, ref tmpPP3);
                    dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP2, ref tmpP3);
                    IPoint WKP3 = new PointClass();
                    ((IConstructPoint)WKP3).ConstructAngleIntersection(tmpPP3, dAF1, tmpP3, dAF2);

                    dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP3, ref tmpPP4);
                    dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP1, ref tmpP4);


                    IPoint WKP4 = new PointClass();
                    ((IConstructPoint)WKP4).ConstructAngleIntersection(tmpPP4, dAF1, tmpP4, dAF2);

                    NewPol = new PolylineClass();
                    pCol = (IPointCollection)NewPol;
                    Missing = Type.Missing;
                    pCol.AddPoint(WKP1, ref Missing, ref Missing);
                    pCol.AddPoint(WKP2, ref Missing, ref Missing);
                    pCol.AddPoint(WKP3, ref Missing, ref Missing);
                    pCol.AddPoint(WKP4, ref Missing, ref Missing);
                    pCol.AddPoint(WKP1, ref Missing, ref Missing);

                    lineSym = new SimpleLineSymbolClass();
                    lineSym.Width = 0.2;
                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                    lineSym.Color = eleColor;
                    LineEle = new LineElementClass();
                    LineEle.Geometry = (IGeometry)NewPol;
                    LineEle.Symbol = lineSym;
                    pageCon.AddElement(LineEle, 0);
                    #endregion

                    #region 经纬度
                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP1, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpP1, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dW1.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = NKP1;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = NKP1;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP1, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpPP1, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dJ1.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = tmpPP1;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = tmpPP1;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP2, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpP2, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dW1.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = NKP2;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = NKP2;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP2, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpPP2, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dJ3.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = tmpPP2;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = tmpPP2;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP3, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpP3, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dW3.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = NKP3;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = NKP3;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP3, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpPP3, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dJ3.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = tmpPP3;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = tmpPP3;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP4, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpP4, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dW3.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = NKP4;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = NKP4;
                        pageCon.AddElement(element, 0);
                    }

                    if (true)
                    {
                        object o = Type.Missing;
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(NKP4, ref o, ref o);
                        ((IPointCollection)pol).AddPoint(tmpPP4, ref o, ref o);
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        string sJWD = m_dJ1.ToString("F10");
                        int nPos = sJWD.IndexOf(".");
                        string sD = sJWD.Substring(0, nPos);
                        string sF = sJWD.Substring(nPos + 1, 2);
                        string sM = sJWD.Substring(nPos + 3, 6);
                        double dM = Convert.ToDouble(sM) / 10000.0;
                        if (Math.Abs(dM - 60.0) < 0.1)
                        {
                            sM = "00";
                            int nF = Convert.ToInt32(sF) + 1;
                            int nD = Convert.ToInt32(sD);
                            if (Math.Abs(nF - 60) < 0.01)
                                nD = nD + 1;
                            sF = nF.ToString("D02");
                            sD = nD.ToString();
                        }
                        else
                        {
                            sM = sM.Substring(0, 2);
                        }
                        sD = sD + "°";
                        sF = sF + "'";
                        sM = sM + "\"";

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sD;
                        IElement element = (IElement)textEle;
                        element.Geometry = tmpPP4;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.20;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = sF + sM;
                        element = (IElement)textEle;
                        element.Geometry = tmpPP4;
                        pageCon.AddElement(element, 0);
                    }

                    #endregion

                    #region 注记、标题、比例尺
                    IPoint pBTPoint = new PointClass();
                    IPoint pZXJPoint = new PointClass();
                    IPoint pYXJPoint = new PointClass();
                    pBTPoint.X = (WKP3.X + WKP4.X) * 0.5;
                    pBTPoint.Y = (WKP3.Y + WKP4.Y) * 0.5;
                    pZXJPoint.X = NKP1.X;
                    pZXJPoint.Y = NKP1.Y;
                    pYXJPoint.X = NKP2.X;
                    pYXJPoint.Y = NKP2.Y;
                    string sBT = this.memoBTZJ.Text + "\r\n" + m_sTFH;
                    AddZJTitle(WKP3, WKP4, pBTPoint, pZXJPoint, pYXJPoint, sBT);

                    PointClass pp1 = new PointClass();
                    pp1.X = (WKP1.X + WKP2.X) * 0.5;
                    pp1.Y = (WKP1.Y + WKP2.Y) * 0.5;
                    PointClass pp2 = new PointClass();
                    pp2.X = pp1.X;
                    pp2.Y = pp1.Y - 10.0;
                    int nScale = Convert.ToInt32(dScale);
                    AddScale(WKP3, WKP4, nScale.ToString(), pp2);
                    #endregion

                    #region 添加分幅图四周的行政区名称
                    
                    int n = 0;//编号变量
                    List<TFPoint> tfPoints = new List<TFPoint>();//存放图幅边界与行政区相交的点，包括编号、点、行政区级别（乡、村）、位置（上、下、左、右）、行政区名称等
                    //List<string> xy = new List<string>();//存放所有的点位置，用于判断点是否重复、点距离
                    //上边框
                    IPointCollection line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperLeft);
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperRight);
                    IGeometry upGeo = (line as ITopologicalOperator).Buffer(10);
                    //下边框
                    line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerLeft);
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerRight);
                    IGeometry downGeo = (line as ITopologicalOperator).Buffer(10);
                    //左边框
                    line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerLeft);
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperLeft);
                    IGeometry leftGeo = (line as ITopologicalOperator).Buffer(10);
                    //右边框
                    line = new PolylineClass();
                    line.AddPoint(m_RealCtPolygon.Envelope.UpperRight);
                    line.AddPoint(m_RealCtPolygon.Envelope.LowerRight);
                    IGeometry rightGeo = (line as ITopologicalOperator).Buffer(10);

                    ////添加行政区的相交点
                    //IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    //pFeatureLayer.FeatureClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("XZQ");
                    //IIdentify pIdentify = pFeatureLayer as IIdentify;
                    //IArray pArray = pIdentify.Identify(m_RealCtPolygon);
                    //for (int i = 0; i < pArray.Count; i++)
                    //{
                    //    IFeatureIdentifyObj pFIObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                    //    IRowIdentifyObject pRIObj = pFIObj as IRowIdentifyObject;
                    //    IFeature pFeature = pRIObj.Row as IFeature;
                    //    IPointCollection pPointCollection = (m_RealCtPolygon as ITopologicalOperator).Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
                    //    for (int j = 0; j < pPointCollection.PointCount; j++)
                    //    {
                    //        IPoint pPoint = pPointCollection.Point[j];
                    //        TFPoint tfPoint = new TFPoint();
                    //        tfPoint.No = n++;
                    //        tfPoint.labelPoint = pPoint;
                    //        tfPoint.xzqLevel = "乡";
                    //        tfPoint.directPosition = getPosition(upGeo, downGeo, leftGeo, rightGeo, pPoint);
                    //        tfPoint.xzqMC = FeatureHelper.GetFeatureStringValue(pFeature, "XZQMC");
                    //        if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                    //        {
                    //            IPoint newPoint = new PointClass();
                    //            newPoint.PutCoords(pPoint.X - 2, pPoint.Y);
                    //            if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                    //            {
                    //                tfPoint.direct = -1;
                    //            }
                    //            else
                    //            {
                    //                tfPoint.direct = 1;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            IPoint newPoint = new PointClass();
                    //            newPoint.PutCoords(pPoint.X, pPoint.Y - 2);
                    //            if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                    //            {
                    //                tfPoint.direct = -1;
                    //            }
                    //            else
                    //            {
                    //                tfPoint.direct = 1;
                    //            }
                    //        }
                    //        Boolean b = getDistance(tfPoint, tfPoints);
                    //        if (b) tfPoints.Add(tfPoint);
                    //        xy.Add(Math.Round(pPoint.X, 2) + "," + Math.Round(pPoint.Y, 2));
                    //    }
                    //}
                    //添加村级调查区的相交点
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("CJDCQ");
                    IIdentify pIdentify = pFeatureLayer as IIdentify;
                    IArray pArray = pIdentify.Identify(m_RealCtPolygon);
                    for (int i = 0; i < pArray.Count; i++)
                    {
                        IFeatureIdentifyObj pFIObj = pArray.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRIObj = pFIObj as IRowIdentifyObject;
                        IFeature pFeature = pRIObj.Row as IFeature;
                        IPointCollection pPointCollection = (m_RealCtPolygon as ITopologicalOperator).Intersect(pFeature.ShapeCopy, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
                        for (int j = 0; j < pPointCollection.PointCount; j++)
                        {
                            IPoint pPoint = pPointCollection.Point[j];
                            //if (tfPoints.Count == 0)
                            //{
                            //    TFPoint tfPoint = new TFPoint();
                            //    tfPoint.No = n++;
                            //    tfPoint.labelPoint = pPoint;
                            //    tfPoint.xzqLevel = "村";
                            //    tfPoint.directPosition = getPosition(upGeo, downGeo, leftGeo, rightGeo, pPoint);
                            //    tfPoint.xzqMC = FeatureHelper.GetFeatureStringValue(pFeature, "ZLDWMC");
                            //    if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                            //    {
                            //        IPoint newPoint = new PointClass();
                            //        newPoint.PutCoords(pPoint.X - 2, pPoint.Y);
                            //        if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                            //        {
                            //            tfPoint.direct = -1;
                            //        }
                            //        else
                            //        {
                            //            tfPoint.direct = 1;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        IPoint newPoint = new PointClass();
                            //        newPoint.PutCoords(pPoint.X, pPoint.Y - 2);
                            //        if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                            //        {
                            //            tfPoint.direct = -1;
                            //        }
                            //        else
                            //        {
                            //            tfPoint.direct = 1;
                            //        }
                            //    }
                            //    tfPoints.Add(tfPoint);
                            //}
                            //else if (!xy.Contains(Math.Round(pPoint.X, 2) + "," + Math.Round(pPoint.Y, 2)))
                            {
                                TFPoint tfPoint = new TFPoint();
                                tfPoint.No = n++;
                                tfPoint.labelPoint = pPoint;
                                tfPoint.xzqLevel = "村";
                                tfPoint.directPosition = getPosition(upGeo, downGeo, leftGeo, rightGeo, pPoint);
                                tfPoint.xzqMC = FeatureHelper.GetFeatureStringValue(pFeature, "ZLDWMC");
                                if (tfPoint.directPosition == "上" || tfPoint.directPosition == "下")
                                {
                                    IPoint newPoint = new PointClass();
                                    newPoint.PutCoords(pPoint.X - 2, pPoint.Y);
                                    if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                                    {
                                        tfPoint.direct = -1;
                                    }
                                    else
                                    {
                                        tfPoint.direct = 1;
                                    }
                                }
                                else
                                {
                                    IPoint newPoint = new PointClass();
                                    newPoint.PutCoords(pPoint.X, pPoint.Y - 2);
                                    if ((pFeature.ShapeCopy as IRelationalOperator).Contains(newPoint))
                                    {
                                        tfPoint.direct = -1;
                                    }
                                    else
                                    {
                                        tfPoint.direct = 1;
                                    }
                                }
                                Boolean b = getDistance(tfPoint, tfPoints);
                                if (b) tfPoints.Add(tfPoint);
                            }
                        }
                    }
                    foreach (TFPoint tfP in tfPoints)
                    {
                        switch (tfP.directPosition)
                        {
                            case "上":
                                addTextElement(tfP.labelPoint.X + tfP.direct * 8 * tfP.xzqMC.Length, tfP.labelPoint.Y + 15, tfP.xzqMC);
                                break;
                            case "下":
                                addTextElement(tfP.labelPoint.X + tfP.direct * 8 * tfP.xzqMC.Length, tfP.labelPoint.Y - 15, tfP.xzqMC);
                                break;
                            case "左":
                                addTextElement(tfP.labelPoint.X - 15, tfP.labelPoint.Y + tfP.direct * 8 * tfP.xzqMC.Length, tfP.xzqMC, true);
                                break;
                            case "右":
                                addTextElement(tfP.labelPoint.X + 15, tfP.labelPoint.Y + tfP.direct * 8 * tfP.xzqMC.Length, tfP.xzqMC, true);
                                break;
                            default:
                                break;
                        }
                    }
                    
                    #endregion

                    #region 左上角结合表
                    if (true)
                    {
                        PointClass[] ZJPos = new PointClass[8];
                        if (true)
                        {
                            object o = Type.Missing;

                            PointClass pp = new PointClass();
                            pp.X = NKP4.X;
                            pp.Y = NKP4.Y + 16;
                            for (int i = 0; i < 3; i++)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    PointClass MidP = new PointClass();
                                    double dx = i * 15;
                                    double dy = j * 8;
                                    double daf1 = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                                    ((IConstructPoint)MidP).ConstructAngleDistance(pp, daf1, dx);
                                    PointClass BaseP = new PointClass();
                                    double daf2 = CommonClassDLL.syc_CalAngle(ref NKP1, ref NKP4);
                                    ((IConstructPoint)BaseP).ConstructAngleDistance(MidP, daf2, dy);

                                    //计算p1,p2,p3,p4[逆]
                                    PointClass p1 = new PointClass();
                                    p1.X = BaseP.X;
                                    p1.Y = BaseP.Y;
                                    p1.Z = 0.0;

                                    PointClass p2 = new PointClass();
                                    ((IConstructPoint)p2).ConstructAngleDistance(p1, daf1, 15);

                                    PointClass p3 = new PointClass();
                                    ((IConstructPoint)p3).ConstructAngleDistance(p2, daf2, 8);

                                    double daf3 = CommonClassDLL.syc_CalAngle(ref NKP3, ref NKP4);
                                    PointClass p4 = new PointClass();
                                    ((IConstructPoint)p4).ConstructAngleDistance(p3, daf3, 15);

                                    PolylineClass pol = new PolylineClass();
                                    ((IPointCollection)pol).AddPoint(p1, ref o, ref o);
                                    ((IPointCollection)pol).AddPoint(p2, ref o, ref o);
                                    ((IPointCollection)pol).AddPoint(p3, ref o, ref o);
                                    ((IPointCollection)pol).AddPoint(p4, ref o, ref o);
                                    ((IPointCollection)pol).AddPoint(p1, ref o, ref o);
                                    lineSym = new SimpleLineSymbolClass();
                                    lineSym.Width = 0.1;
                                    eleColor = ColorHelper.CreateColor(0, 0, 0);
                                    lineSym.Color = eleColor;
                                    LineEle = new LineElementClass();
                                    LineEle.Geometry = pol;
                                    LineEle.Symbol = lineSym;
                                    pageCon.AddElement(LineEle, 0);

                                    if (i == 1 && j == 1)
                                    {
                                        PolygonClass pog = new PolygonClass();
                                        ((IPointCollection)pog).AddPoint(p1, ref o, ref o);
                                        ((IPointCollection)pog).AddPoint(p2, ref o, ref o);
                                        ((IPointCollection)pog).AddPoint(p3, ref o, ref o);
                                        ((IPointCollection)pog).AddPoint(p4, ref o, ref o);
                                        ISimpleFillSymbol fillSym = new SimpleFillSymbolClass();
                                        fillSym.Style = esriSimpleFillStyle.esriSFSBackwardDiagonal;
                                        lineSym = new SimpleLineSymbolClass();
                                        lineSym.Style = esriSimpleLineStyle.esriSLSNull;
                                        fillSym.Outline = lineSym;
                                        PolygonElementClass pogEle = new PolygonElementClass();
                                        pogEle.Geometry = pog;
                                        pogEle.Symbol = fillSym;
                                        pageCon.AddElement(pogEle, 0);
                                    }

                                    PointClass CenterP = new PointClass();
                                    CenterP.X = (p1.X + p3.X) * 0.5;
                                    CenterP.Y = (p1.Y + p3.Y) * 0.5;
                                    if (i == 0 && j == 0)
                                        ZJPos[0] = CenterP;
                                    else if (i == 1 && j == 0)
                                        ZJPos[1] = CenterP;
                                    else if (i == 2 && j == 0)
                                        ZJPos[2] = CenterP;
                                    else if (i == 2 && j == 1)
                                        ZJPos[3] = CenterP;
                                    else if (i == 2 && j == 2)
                                        ZJPos[4] = CenterP;
                                    else if (i == 1 && j == 2)
                                        ZJPos[5] = CenterP;
                                    else if (i == 0 && j == 2)
                                        ZJPos[6] = CenterP;
                                    else if (i == 0 && j == 1)
                                        ZJPos[7] = CenterP;
                                }
                            }

                            double dDJ = (DLIB.DFM2D(m_dJ1) + DLIB.DFM2D(m_dJ3)) * 0.5;
                            double dDW = (DLIB.DFM2D(m_dW1) + DLIB.DFM2D(m_dW3)) * 0.5;
                            double dDelJ = 0.0, dDelW = 0.0;
                            DLIB.GetDelDFM(dScale, ref dDelJ, ref dDelW);
                            double dDelJ_D = DLIB.DFM2D(dDelJ);
                            double dDelW_D = DLIB.DFM2D(dDelW);

                            double[] dJSz = new double[8];
                            double[] dWSz = new double[8];
                            dJSz[0] = dDJ - dDelJ_D;
                            dWSz[0] = dDW - dDelW_D;
                            dJSz[1] = dDJ;
                            dWSz[1] = dDW - dDelW_D;
                            dJSz[2] = dDJ + dDelJ_D;
                            dWSz[2] = dDW - dDelW_D;
                            dJSz[3] = dDJ + dDelJ_D;
                            dWSz[3] = dDW;
                            dJSz[4] = dDJ + dDelJ_D;
                            dWSz[4] = dDW + dDelW_D;
                            dJSz[5] = dDJ;
                            dWSz[5] = dDW + dDelW_D;
                            dJSz[6] = dDJ - dDelJ_D;
                            dWSz[6] = dDW + dDelW_D;
                            dJSz[7] = dDJ - dDelJ_D;
                            dWSz[7] = dDW;

                            string[] sTFHSz = new string[8];
                            for (int i = 0; i < 8; i++)
                            {
                                double dJ_D = dJSz[i];
                                double dW_D = dWSz[i];
                                double dJ = DLIB.HD2DFM(dJ_D * Math.PI / 180.0);
                                double dW = DLIB.HD2DFM(dW_D * Math.PI / 180.0);
                                StringBuilder ss = new StringBuilder(100);
                                double dJ1 = 0.0, dW1 = 0.0, dJ3 = 0.0, dW3 = 0.0;
                                DLIB.GetNewWaima(dScale, dJ, dW, ss);
                                string sTFH = ss.ToString();
                                sTFHSz[i] = sTFH;
                            }

                            for (int i = 0; i < 8; i++)
                            {
                                System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1);
                                ITextSymbol textSymbol = new TextSymbolClass();
                                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                                double dZH = 1.80;	//mm
                                textSymbol.Size = dZH / 25.4 * 72.0;
                                double daf = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3) * 180.0 / Math.PI;
                                textSymbol.Angle = daf;
                                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                                TextElementClass textEle = new TextElementClass();
                                textEle.Symbol = textSymbol;
                                textEle.Text = sTFHSz[i];
                                IElement element = (IElement)textEle;
                                element.Geometry = ZJPos[i];
                                pageCon.AddElement(element, 0);
                            }
                        }
                    }
                    #endregion

                    #region 方里网

                    //四个角的坐标
                    IPoint LDP = new PointClass();
                    IPoint tmpJwd = new PointClass();
                    tmpJwd.PutCoords(m_dJ1, m_dW1);
                    CommonClassDLL.syc_JWD2XY(myMap, m_dJ1, m_dW1, ref LDP, out sErrorInfo);
                    IPoint RDP = new PointClass();
                    tmpJwd.PutCoords(m_dJ3, m_dW1);
                    CommonClassDLL.syc_JWD2XY(myMap, m_dJ3, m_dW1, ref RDP, out sErrorInfo);
                    IPoint RUP = new PointClass();
                    tmpJwd.PutCoords(m_dJ3, m_dW3);
                    CommonClassDLL.syc_JWD2XY(myMap, m_dJ3, m_dW3, ref RUP, out sErrorInfo);
                    IPoint LUP = new PointClass();
                    tmpJwd.PutCoords(m_dJ1, m_dW3);
                    CommonClassDLL.syc_JWD2XY(myMap, m_dJ1, m_dW3, ref LUP, out sErrorInfo);




                    int nXGS = 0;
                    FLW[] pXFLW = new FLW[500];
                    int nBJ = 0;
                    int nLastX_KM = 0;
                    IPoint LastP = new PointClass();
                    while (true)
                    {
                        FLW flw = new FLW();
                        if (nBJ == 0)
                        {
                            nBJ++;

                            #region  //第一次:

                            int nX2 = (int)(LDP.X / 1000.0) + 1;
                            double dDel = (nX2 * 1000.0 - LDP.X) / dScale * 1000.0;

                            pLine = new LineClass();
                            pLine.FromPoint = NKP1;
                            pLine.ToPoint = NKP2;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP2.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP4.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nX2.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pXFLW[nXGS] = flw;
                            nXGS++;

                            nLastX_KM = nX2;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                            #endregion
                        }
                        else
                        {
                            #region  //非第一次:
                            int nCurX_KM = nLastX_KM + 1;
                            int nDist_KM = 1;
                            double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                            pLine = new LineClass();
                            pLine.FromPoint = LastP;
                            pLine.ToPoint = NKP2;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP2.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP4.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nCurX_KM.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pXFLW[nXGS] = flw;
                            nXGS++;

                            nLastX_KM = nCurX_KM;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                            if (flw.PP1.X > NKP2.X)
                            {
                                nXGS = nXGS - 1;
                                break;
                            }
                            #endregion
                        }
                    } //while(1)

                    for (int i = 0; i < nXGS; i++)
                    {
                        object oo = Type.Missing;
                        FLW flw = pXFLW[i] as FLW;

                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        //Text:
                        pLine = new LineClass();
                        pLine.FromPoint = flw.PP1;
                        pLine.ToPoint = flw.PP2;
                        IPoint ZJP = new PointClass();
                        ((IConstructPoint)ZJP).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, 9.0, false);

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 2.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ1;
                        IElement element = (IElement)textEle;
                        element.Geometry = ZJP;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ2;
                        element = (IElement)textEle;
                        element.Geometry = ZJP;
                        pageCon.AddElement(element, 0);
                    }


                    int nSGS = 0;
                    FLW[] pSFLW = new FLW[500];
                    nBJ = 0;
                    nLastX_KM = 0;
                    LastP = new PointClass();
                    while (true)
                    {
                        FLW flw = new FLW();
                        if (nBJ == 0)
                        {
                            nBJ++;

                            #region  //第一次:

                            int nX2 = (int)(LUP.X / 1000.0) + 1;
                            double dDel = (nX2 * 1000.0 - LUP.X) / dScale * 1000.0;

                            pLine = new LineClass();
                            pLine.FromPoint = NKP4;
                            pLine.ToPoint = NKP3;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP2.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP4.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nX2.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pSFLW[nSGS] = flw;
                            nSGS++;

                            nLastX_KM = nX2;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                            #endregion

                        }
                        else
                        {
                            #region   //非第一次:
                            int nCurX_KM = nLastX_KM + 1;
                            int nDist_KM = 1;
                            double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                            pLine = new LineClass();
                            pLine.FromPoint = LastP;
                            pLine.ToPoint = NKP3;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP2.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP2.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP4.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP4.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nCurX_KM.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pSFLW[nSGS] = flw;
                            nSGS++;

                            nLastX_KM = nCurX_KM;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                            if (flw.PP1.X > NKP3.X)
                            {
                                nSGS = nSGS - 1;
                                break;
                            }
                            #endregion
                        }
                    } //while(1)

                    for (int i = 0; i < nSGS; i++)
                    {
                        object oo = Type.Missing;
                        FLW flw = pSFLW[i] as FLW;

                        //线:PP1-PP2
                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        //Text:
                        pLine = new LineClass();
                        pLine.FromPoint = flw.PP1;
                        pLine.ToPoint = flw.PP2;
                        IPoint ZJP = new PointClass();
                        ((IConstructPoint)ZJP).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, 11.0, false);

                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 2.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ1;
                        IElement element = (IElement)textEle;
                        element.Geometry = ZJP;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ2;
                        element = (IElement)textEle;
                        element.Geometry = ZJP;
                        pageCon.AddElement(element, 0);
                    }


                    int nZGS = 0;
                    FLW[] pZFLW = new FLW[500];
                    nBJ = 0;
                    int nLastY_KM = 0;
                    LastP = new PointClass();
                    while (true)
                    {
                        FLW flw = new FLW();
                        if (nBJ == 0)
                        {
                            nBJ++;

                            //第一次:

                            int nY2 = (int)(LDP.Y / 1000.0) + 1;
                            double dDel = (nY2 * 1000.0 - LDP.Y) / dScale * 1000.0;

                            pLine = new LineClass();
                            pLine.FromPoint = NKP1;
                            pLine.ToPoint = NKP4;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP4.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP2.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nY2.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pZFLW[nZGS] = flw;
                            nZGS++;

                            nLastY_KM = nY2;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                        }
                        else
                        {
                            //非第一次:
                            int nCurY_KM = nLastY_KM + 1;
                            int nDist_KM = 1;
                            double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                            pLine = new LineClass();
                            pLine.FromPoint = LastP;
                            pLine.ToPoint = NKP4;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP4.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP2.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p2, ref p1);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nCurY_KM.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pZFLW[nZGS] = flw;
                            nZGS++;

                            nLastY_KM = nCurY_KM;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                            if (flw.PP1.Y > NKP4.Y)
                            {
                                nZGS = nZGS - 1;
                                break;
                            }
                        }
                    } //while(1)

                    for (int i = 0; i < nZGS; i++)
                    {
                        object oo = Type.Missing;
                        FLW flw = pZFLW[i] as FLW;

                        //线:PP1-PP2
                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        //Text:
                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 2.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ1;
                        IElement element = (IElement)textEle;
                        element.Geometry = flw.PP2;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ2;
                        element = (IElement)textEle;
                        element.Geometry = flw.PP1;
                        pageCon.AddElement(element, 0);
                    }

                    int nYGS = 0;
                    FLW[] pYFLW = new FLW[500];
                    nBJ = 0;
                    nLastY_KM = 0;
                    LastP = new PointClass();
                    while (true)
                    {
                        FLW flw = new FLW();
                        if (nBJ == 0)
                        {
                            nBJ++;

                            //第一次:

                            int nY2 = (int)(RDP.Y / 1000.0) + 1;
                            double dDel = (nY2 * 1000.0 - RDP.Y) / dScale * 1000.0;

                            pLine = new LineClass();
                            pLine.FromPoint = NKP2;
                            pLine.ToPoint = NKP3;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP4.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP2.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nY2.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pYFLW[nYGS] = flw;
                            nYGS++;

                            nLastY_KM = nY2;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                        }
                        else
                        {
                            //非第一次:
                            int nCurY_KM = nLastY_KM + 1;
                            int nDist_KM = 1;
                            double dDel = (nDist_KM * 1000.0) / dScale * 1000.0;	//mm

                            pLine = new LineClass();
                            pLine.FromPoint = LastP;
                            pLine.ToPoint = NKP3;
                            IPoint PP1 = new PointClass();
                            ((IConstructPoint)PP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dDel, false);
                            flw.PP1 = PP1;

                            IPoint p1 = new PointClass();
                            p1.X = (NKP1.X + NKP4.X) * 0.5;
                            p1.Y = (NKP1.Y + NKP4.Y) * 0.5;
                            IPoint p2 = new PointClass();
                            p2.X = (NKP3.X + NKP2.X) * 0.5;
                            p2.Y = (NKP3.Y + NKP2.Y) * 0.5;
                            double dAF = CommonClassDLL.syc_CalAngle(ref p1, ref p2);
                            IPoint PP2 = new PointClass();
                            ((IConstructPoint)PP2).ConstructAngleDistance(PP1, dAF, 12.0);
                            flw.PP2 = PP2;

                            string sBS = nCurY_KM.ToString();
                            int nLen = sBS.Length;
                            string sZJ1 = sBS.Substring(0, nLen - 2);
                            string sZJ2 = sBS.Substring(nLen - 2, 2);
                            flw.sZJ1 = sZJ1;
                            flw.sZJ2 = sZJ2;
                            flw.sBS = sBS;

                            pYFLW[nYGS] = flw;
                            nYGS++;

                            nLastY_KM = nCurY_KM;
                            LastP.PutCoords(flw.PP1.X, flw.PP1.Y);
                            if (flw.PP1.Y > NKP3.Y)
                            {
                                nYGS = nYGS - 1;
                                break;
                            }
                        }
                    } //while(1)

                    for (int i = 0; i < nYGS; i++)
                    {
                        object oo = Type.Missing;
                        FLW flw = pYFLW[i] as FLW;

                        //线:PP1-PP2
                        PolylineClass pol = new PolylineClass();
                        ((IPointCollection)pol).AddPoint(flw.PP1, ref oo, ref oo);
                        ((IPointCollection)pol).AddPoint(flw.PP2, ref oo, ref oo);
                        lineSym = new SimpleLineSymbolClass();
                        lineSym.Width = 0.1;
                        eleColor = ColorHelper.CreateColor(0, 0, 0);
                        lineSym.Color = eleColor;
                        LineEle = new LineElementClass();
                        LineEle.Geometry = pol;
                        LineEle.Symbol = lineSym;
                        pageCon.AddElement(LineEle, 0);

                        //Text:
                        System.Drawing.Font dotNetFont = new System.Drawing.Font("Tahoma", 1);
                        ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        double dZH = 2.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        TextElementClass textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ1;
                        IElement element = (IElement)textEle;
                        element.Geometry = flw.PP1;
                        pageCon.AddElement(element, 0);

                        textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                        dZH = 3.0;	//mm
                        textSymbol.Size = dZH / 25.4 * 72.0;
                        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                        textEle = new TextElementClass();
                        textEle.Symbol = textSymbol;
                        textEle.Text = flw.sZJ2;
                        element = (IElement)textEle;
                        element.Geometry = flw.PP2;
                        pageCon.AddElement(element, 0);
                    }


                    if (chkTKFLW.Checked == false)
                    {
                        #region 方里网注记
                        ArrayList LeftRightLines = new ArrayList();
                        for (int i = 0; i < nZGS; i++)
                        {
                            string sLeftBS = pZFLW[i].sBS;
                            bool bExist = false;
                            int nIndex = -1;
                            for (int j = 0; j < nYGS; j++)
                            {
                                if (pYFLW[j].sBS.Equals(sLeftBS) == true)
                                {
                                    bExist = true;
                                    nIndex = j;
                                    break;
                                }
                            }
                            if (bExist == true)
                            {
                                object oo = Type.Missing;
                                PolylineClass pol = new PolylineClass();
                                ((IPointCollection)pol).AddPoint(pZFLW[i].PP1, ref oo, ref oo);
                                ((IPointCollection)pol).AddPoint(pYFLW[nIndex].PP1, ref oo, ref oo);
                                LeftRightLines.Add(pol);
                            }
                        }

                        ArrayList UpperDownLines = new ArrayList();
                        for (int i = 0; i < nXGS; i++)
                        {
                            string sDownBS = pXFLW[i].sBS;
                            bool bExist = false;
                            int nIndex = -1;
                            for (int j = 0; j < nSGS; j++)
                            {
                                if (pSFLW[j].sBS.Equals(sDownBS) == true)
                                {
                                    bExist = true;
                                    nIndex = j;
                                    break;
                                }
                            }
                            if (bExist == true)
                            {
                                object oo = Type.Missing;
                                PolylineClass pol = new PolylineClass();
                                ((IPointCollection)pol).AddPoint(pXFLW[i].PP1, ref oo, ref oo);
                                ((IPointCollection)pol).AddPoint(pSFLW[nIndex].PP1, ref oo, ref oo);
                                UpperDownLines.Add(pol);
                            }
                        }

                        int nGS1 = LeftRightLines.Count;
                        for (int i = 0; i < nGS1; i++)
                        {
                            IPolyline LRLine = LeftRightLines[i] as IPolyline;
                            IPointCollection pCol1 = LRLine as IPointCollection;
                            IPoint P1 = pCol1.get_Point(0);
                            IPoint P2 = pCol1.get_Point(1);
                            double dA1 = CommonClassDLL.syc_CalAngle(ref P1, ref P2);

                            int nGS2 = UpperDownLines.Count;
                            for (int j = 0; j < nGS2; j++)
                            {
                                IPolyline UDLine = UpperDownLines[j] as IPolyline;
                                IPointCollection pCol2 = UDLine as IPointCollection;
                                IPoint PP1 = pCol2.get_Point(0);
                                IPoint PP2 = pCol2.get_Point(1);
                                double dA2 = CommonClassDLL.syc_CalAngle(ref PP1, ref PP2);

                                IPoint JP = new PointClass();
                                ((IConstructPoint)JP).ConstructAngleIntersection(P1, dA1, PP1, dA2);	//Intersection

                                //以JP为中心,dA1+dA2产生4点:
                                IPoint p1 = new PointClass();
                                ((IConstructPoint)p1).ConstructAngleDistance(JP, dA1, 5.0);
                                IPoint p2 = new PointClass();
                                ((IConstructPoint)p2).ConstructAngleDistance(JP, dA1 + Math.PI, 5.0);
                                IPoint p3 = new PointClass();
                                ((IConstructPoint)p3).ConstructAngleDistance(JP, dA2, 5.0);
                                IPoint p4 = new PointClass();
                                ((IConstructPoint)p4).ConstructAngleDistance(JP, dA2 + Math.PI, 5.0);

                                //线:p1-p2;p3-p4
                                object oo = Type.Missing;
                                PolylineClass pol = new PolylineClass();
                                ((IPointCollection)pol).AddPoint(p1, ref oo, ref oo);
                                ((IPointCollection)pol).AddPoint(p2, ref oo, ref oo);
                                lineSym = new SimpleLineSymbolClass();
                                lineSym.Width = 0.1;
                                eleColor = ColorHelper.CreateColor(0, 0, 0);
                                lineSym.Color = eleColor;
                                LineEle = new LineElementClass();
                                LineEle.Geometry = pol;
                                LineEle.Symbol = lineSym;
                                pageCon.AddElement(LineEle, 0);

                                pol = new PolylineClass();
                                ((IPointCollection)pol).AddPoint(p3, ref oo, ref oo);
                                ((IPointCollection)pol).AddPoint(p4, ref oo, ref oo);
                                lineSym = new SimpleLineSymbolClass();
                                lineSym.Width = 0.1;
                                eleColor = ColorHelper.CreateColor(0, 0, 0);
                                lineSym.Color = eleColor;
                                LineEle = new LineElementClass();
                                LineEle.Geometry = pol;
                                LineEle.Symbol = lineSym;
                                pageCon.AddElement(LineEle, 0);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region  添加图例、指北针
                    AddLegendWB(NKP2, NKP3, NKP4, pCol);

                    IPoint TLP = new PointClass();
                    double dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                    ((IConstructPoint)TLP).ConstructAngleDistance(tmpP3, dA, 15.0);
                    IEnvelope pZBZEnv = m_PageControl.Page.PrintableBounds;
                    pZBZEnv.XMin = TLP.X;
                    pZBZEnv.XMax = TLP.X + pZBZEnv.Width * 0.98;
                    pZBZEnv.YMax = TLP.Y + pZBZEnv.Height * 0.053;
                    pZBZEnv.YMin = TLP.Y + pZBZEnv.Height * 0.013;
                    AddNorthArrow(m_myMapFrame, pZBZEnv);
                    #endregion

                    IElement mapFrameEle = m_myMapFrame as IElement;
                    outMjGTDW(mapFrameEle.Geometry.Envelope.LowerLeft, mapFrameEle.Geometry.Envelope.UpperRight);

                    this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                    IGraphicsContainerSelect pGCSelect = this.m_PageControl.PageLayout as IGraphicsContainerSelect;
                    pGCSelect.UnselectAllElements();

                    m_PageControl.ZoomToWholePage();
                    m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);

                    string sType = cmbOutType.SelectedItem.ToString().Trim();

                    if (string.IsNullOrEmpty(sType) || sType.Equals("MXD"))   //生成arcmap文档
                    {
                        IMxdContents pMxd = m_PageControl.PageLayout as IMxdContents;
                        IMapDocument m_MapDocument = new MapDocumentClass();
                        string destFile = destDir + @"\" + m_sTFH + "标准分幅图.mxd";
                        m_MapDocument.New(destFile);

                        m_MapDocument.ReplaceContents(pMxd);
                        m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);
                    }
                    else  //导出图片
                    {
                        string destFile = destDir + @"\" + m_sTFH + "标准分幅图" + "." + sType;
                        string sRetErrorInfo = "";
                        bool bRet = CommonClassDLL.ExportActiveViewParameterized(pageAct, (long)DPI, 1, false, destFile, ref sRetErrorInfo);
                    }
                    m_PageControl.ActiveView.Deactivate();
                    m_MapControl.ActiveView.Refresh();

                }
                wait.Close();
                m_myTab.SelectedTabPageIndex = 1;
                MessageBox.Show("输出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                wait.Close();
            }
            finally
            {
                CommonClassDLL.Dispose();

            }
        }

        private void cmbOutType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (new List<string>() { "JPG", "JPEG", "BMP", "GIF", "PDF", "TIFF" }.Contains(cmbOutType.SelectedItem.ToString()))
            {
                lbFBL.Visible = true;
                numericUpDown1.Visible = true;
            }
            else
            {
                lbFBL.Visible = false;
                numericUpDown1.Visible = false;
            }
        }
    }
    public class TFPoint
    {
        public int No;
        public IPoint labelPoint;
        public string xzqLevel;
        public string directPosition;
        public int direct;
        public string xzqMC;
    }
}
