using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Global;
using System.IO;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Collections;
using ESRI.ArcGIS.Controls;
using DevExpress.XtraTab;
using RCIS.Utility;
using ESRI.ArcGIS.SystemUI;
using sycCommonLib;
using RCIS.Database;

namespace TDDC3D.output
{
    public partial class FrmExportKZMJJHB : Form
    {
        double dLat = 0;//纬度差
        double dLon = 0;//经度差

        public FrmExportKZMJJHB()
        {
            InitializeComponent();
            IWorkspace2 pWS = GlobalEditObject.GlobalWorkspace as IWorkspace2;
            if (pWS.NameExists[esriDatasetType.esriDTFeatureClass, "TFH"])
            {
                IFeatureClass pFS = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("TFH");
                IFeatureCursor pFC = pFS.Search(null, false);
                IFeature pF = pFC.NextFeature();
                RCIS.Utility.OtherHelper.ReleaseComObject(pFC);
                if (pF != null)
                {
                    string tfh = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(pF, "TFH");
                    if (tfh.Length > 3)
                    {
                        switch (tfh.Substring(3,1).ToUpper())
                        {
                            case "K":
                                cmbCTBLC.Text = "500";
                                dLat = MathHelper.ConvertDegreesToDigital("0°0′6.25″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°0′9.375″");
                                break;
                            case "J":
                                cmbCTBLC.Text = "1000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°0′12.5″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°0′18.75″");
                                break;
                            case "I":
                                cmbCTBLC.Text = "2000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°0′25″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°0′37.5″");
                                break;
                            case "H":
                                cmbCTBLC.Text = "5000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°1′15″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°1′52.5″");
                                break;
                            case "G":
                                cmbCTBLC.Text = "10000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°2′30″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°3′45″");
                                break;
                            case "F":
                                cmbCTBLC.Text = "25000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°5′00″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°7′30″");
                                break;
                            case "E":
                                cmbCTBLC.Text = "50000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°10′00″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°15′00″");
                                break;
                            case "D":
                                cmbCTBLC.Text = "100000";
                                dLat = MathHelper.ConvertDegreesToDigital("0°20′00″");
                                dLon = MathHelper.ConvertDegreesToDigital("0°30′00″");
                                break;
                            case "C":
                                cmbCTBLC.Text = "250000";
                                dLat = MathHelper.ConvertDegreesToDigital("1°");
                                dLon = MathHelper.ConvertDegreesToDigital("1°30′");
                                break;
                            case "B":
                                cmbCTBLC.Text = "500000";
                                dLat = MathHelper.ConvertDegreesToDigital("2°");
                                dLon = MathHelper.ConvertDegreesToDigital("3°");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public XtraTabControl m_myTab;
        public AxMapControl m_MapControl;
        public AxPageLayoutControl m_PageControl;
        IMapFrame m_myMapFrame = null;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            WaitDialogForm wait = null;
            IFeatureClass pXZQFC = null;
            IFeatureClass pTFHPC = null;
            IFeatureWorkspace pFeaWS = null;
            IWorkspace pTempWS = null;
            IFeatureWorkspace pTempFeaWS = null;
            sycCommonFuns CommonClassDLL = new sycCommonFuns();
            try
            {
                wait = new WaitDialogForm("正在生成，请稍候...", "提示");
                wait.Show();

                pFeaWS = GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
                pXZQFC = pFeaWS.OpenFeatureClass("XZQ");
                if (pXZQFC == null || pXZQFC.FeatureCount(null) <= 0)
                {
                    wait.Close();
                    MessageBox.Show("行政区图层无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                pTFHPC = pFeaWS.OpenFeatureClass("TFH");
                if (pTFHPC == null || pTFHPC.FeatureCount(null) <= 0)
                {
                    wait.Close();
                    MessageBox.Show("图幅接合表无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < m_MapControl.Map.LayerCount; i++)
                {
                    ILayer pLayer = m_MapControl.Map.Layer[i];
                    pLayer.Visible = false;
                }

                string sScale = cmbCTBLC.SelectedItem.ToString();
                double dScale = Convert.ToDouble(sScale);

                string tempPath = AppDomain.CurrentDomain.BaseDirectory + "Temp";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                string tempGDB = tempPath + "\\temp.gdb";

                pTempWS = WorkspaceHelper2.DeleteAndNewGDB(tempGDB);
                pTempFeaWS = pTempWS as IFeatureWorkspace;

                //融合行政区图层
                RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(GlobalEditObject.GlobalWorkspace, pTempWS, "TFH", "TF", null);
                GpToolHelper gpHelper = new GpToolHelper();
                bool xzqResult = gpHelper.Dissolve(pXZQFC, tempGDB + "\\disXZQ", null);
                if (!xzqResult)
                {
                    wait.Close();
                    MessageBox.Show("融合行政区图层失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //用行政区融合后的图形切割图幅表
                bool idenResult = GpToolHelper.Identity(pTFHPC, tempGDB + "\\tfResult", tempGDB + "\\disXZQ");
                if (!idenResult)
                {
                    wait.Close();
                    MessageBox.Show("图层切割失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IFeatureClass pResultFC = pTempFeaWS.OpenFeatureClass("tfResult");

                IGeoDataset pDataset = pXZQFC as IGeoDataset;
                ISpatialReference pSpatialReference = pDataset.SpatialReference;

                IFeatureLayer pFeaLyr = new FeatureLayerClass();
                pFeaLyr.FeatureClass = pTempFeaWS.OpenFeatureClass("TF");
                pFeaLyr.Name = "图幅接合表";


                IFeatureLayer pFealyrXZQ = new FeatureLayerClass();
                pFealyrXZQ.FeatureClass = pXZQFC;
                pFealyrXZQ.Name = "行政区";
                //设置行政区图层样式
                SimpleFillSymbolClass simp = new SimpleFillSymbolClass();
                simp.Style = esriSimpleFillStyle.esriSFSHollow;
                simp.Outline = RCIS.GISCommon.SymbolHelper.GetSymbolFromFile("Line Symbols", "660200") as ILineSymbol;
                ISimpleRenderer pRender = new SimpleRendererClass();
                pRender.Symbol = simp;
                IGeoFeatureLayer pGeoFeaLyrXZQ = pFealyrXZQ as IGeoFeatureLayer;
                pGeoFeaLyrXZQ.Renderer = pRender as IFeatureRenderer;
                //ILayerEffects pLayerEffects = pFealyrXZQ as ILayerEffects;

                //pLayerEffects.Transparency = 100;//行政区图层设置为全透明，否则压盖图幅表

                //设置图幅图层样式
                simp = new SimpleFillSymbolClass();
                simp.Style = esriSimpleFillStyle.esriSFSHollow;
                simp.Outline.Color = ColorHelper.CreateColor(Color.Black);
                pRender = new SimpleRendererClass();
                pRender.Symbol = simp;
                IGeoFeatureLayer pGeoFeaLyrTF = pFeaLyr as IGeoFeatureLayer;
                pGeoFeaLyrTF.Renderer = pRender as IFeatureRenderer;
                //UniqValueDrawing(pFeaLyr, "TFH");
                //UniqValueDrawing(pFealyrXZQ, "XZQDM");

                //生成图表
                this.m_MapControl.Map.ClearSelection();
                this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                this.m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.m_MapControl.ActiveView.Extent);
                IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
                deleteAllElement(pageCon);

                m_PageControl.ActiveView.Activate(m_PageControl.hWnd);
                m_PageControl.ActiveView.FocusMap.MapScale = dScale;
                IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
                IActiveView pageAct = m_PageControl.ActiveView;
                IPointCollection pCol = pFeaLyr.AreaOfInterest.Envelope as IPointCollection;

                m_PageControl.ActiveView.Extent = pFeaLyr.AreaOfInterest.Envelope;  //确定当前区域
                m_myMapFrame = this.GetIMapFrame();
                m_myMapFrame.Map.AddLayer(pFeaLyr);
                m_myMapFrame.Map.AddLayer(pFealyrXZQ);
                wait.SetCaption("正在生成外框...");
                OutOutFrame(pFeaLyr.AreaOfInterest.Envelope.Width + 11 * 0.5 * dScale, pFeaLyr.AreaOfInterest.Envelope.Height + 9 * 0.5 * dScale, dScale);

                #region 内框
                wait.SetCaption("正在生成内框...");

                IPoint NKP1 = new PointClass();
                IPoint NKP2 = new PointClass();
                IPoint NKP3 = new PointClass();
                IPoint NKP4 = new PointClass();

                IEnvelope pEnv = pFeaLyr.AreaOfInterest.Envelope;
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
                //pageCon.AddElement(LineEle, 0);

                //ILine pLine = new LineClass();

                //pLine.FromPoint = NKP1;
                //pLine.ToPoint = NKP2;
                //PointClass tmpP1 = new PointClass();
                //((IConstructPoint)tmpP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -2 * 0.5 / 3 * dScale, false);
                //PointClass tmpP2 = new PointClass();
                //double dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP2);
                //((IConstructPoint)tmpP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 5 * 0.5 / 3 * dScale, false);

                //pLine.FromPoint = NKP4;
                //pLine.ToPoint = NKP3;
                //PointClass tmpP4 = new PointClass();
                //((IConstructPoint)tmpP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -2 * 0.5 / 3 * dScale, false);
                //PointClass tmpP3 = new PointClass();
                //dLen = CommonClassDLL.syc_CalLength(ref NKP3, ref NKP4);
                //((IConstructPoint)tmpP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 5 * 0.5 / 3 * dScale, false);

                //pLine.FromPoint = NKP1;
                //pLine.ToPoint = NKP4;
                //PointClass tmpPP1 = new PointClass();
                //((IConstructPoint)tmpPP1).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -2 * 0.5 / 3 * dScale, false);
                //PointClass tmpPP4 = new PointClass();
                //dLen = CommonClassDLL.syc_CalLength(ref NKP1, ref NKP4);
                //((IConstructPoint)tmpPP4).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 3 * 0.5 / 3 * dScale, false);

                //pLine.FromPoint = NKP2;
                //pLine.ToPoint = NKP3;
                //PointClass tmpPP2 = new PointClass();
                //((IConstructPoint)tmpPP2).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, -2 * 0.5 / 3 * dScale, false);
                //PointClass tmpPP3 = new PointClass();
                //dLen = CommonClassDLL.syc_CalLength(ref NKP2, ref NKP3);
                //((IConstructPoint)tmpPP3).ConstructAlong(pLine, esriSegmentExtension.esriExtendTangents, dLen + 3 * 0.5 / 3 * dScale, false);


                //double dAF1 = CommonClassDLL.syc_CalAngle(ref tmpP4, ref tmpP1);
                //double dAF2 = CommonClassDLL.syc_CalAngle(ref tmpPP2, ref tmpPP1);
                //IPoint WKP1 = new PointClass();
                //((IConstructPoint)WKP1).ConstructAngleIntersection(tmpP1, dAF1, tmpPP1, dAF2);

                //dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP1, ref tmpPP2);
                //dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP3, ref tmpP2);
                //IPoint WKP2 = new PointClass();
                //((IConstructPoint)WKP2).ConstructAngleIntersection(tmpPP2, dAF1, tmpP2, dAF2);

                //dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP4, ref tmpPP3);
                //dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP2, ref tmpP3);
                //IPoint WKP3 = new PointClass();
                //((IConstructPoint)WKP3).ConstructAngleIntersection(tmpPP3, dAF1, tmpP3, dAF2);

                //dAF1 = CommonClassDLL.syc_CalAngle(ref tmpPP3, ref tmpPP4);
                //dAF2 = CommonClassDLL.syc_CalAngle(ref tmpP1, ref tmpP4);


                //IPoint WKP4 = new PointClass();
                //((IConstructPoint)WKP4).ConstructAngleIntersection(tmpPP4, dAF1, tmpP4, dAF2);
                double xMin, xMax, yMin, yMax;
                IGeoDataset pGeodataset = pXZQFC as IGeoDataset;
                double x = 0; double y = 0;
                //左下
                getPoint(pEnv.XMin, pEnv.YMin, 0 - 3 * dLat, 0 - 2 * dLon, pGeodataset.SpatialReference, out x, out y);
                IPoint WKP1 = new PointClass();
                WKP1.PutCoords(x,y);
                xMin = x; yMin = y;
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(WKP1, out nX, out nY);
                IPoint nWP1 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                WKP1.PutCoords(nWP1.X, nWP1.Y);
                //左上
                getPoint(pEnv.XMin, pEnv.YMax, 2 * dLat, 0 - 2 * dLon, pGeodataset.SpatialReference, out x, out y);
                IPoint WKP2 = new PointClass();
                WKP2.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(WKP2, out nX, out nY);
                IPoint nWP2 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                WKP2.PutCoords(nWP2.X, nWP2.Y);
                //右上
                getPoint(pEnv.XMax, pEnv.YMax, 2 * dLat, 5 * dLon, pGeodataset.SpatialReference, out x, out y);
                IPoint WKP3 = new PointClass();
                WKP3.PutCoords(x, y);
                xMax = x; yMax = y;
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(WKP3, out nX, out nY);
                IPoint nWP3 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                WKP3.PutCoords(nWP3.X, nWP3.Y);
                //右下
                getPoint(pEnv.XMax, pEnv.YMin, 0 - 3 * dLat, 5 * dLon, pGeodataset.SpatialReference, out x, out y);
                IPoint WKP4 = new PointClass();
                WKP4.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(WKP4, out nX, out nY);
                IPoint nWP4 = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                WKP4.PutCoords(nWP4.X, nWP4.Y);
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
                //pageCon.AddElement(LineEle, 0);
                #endregion

                wait.SetCaption("正在计算面积...");
                Boolean b = JSMJ(pTempWS, pGeodataset.SpatialReference);
                if (!b) return;

                wait.SetCaption("正在绘制表格...");
                //绘制表格中的各条线
                DrawLines(xMin, xMax, yMin, yMax, pEnv.XMin, pEnv.XMax, pEnv.YMin, pEnv.YMax, pGeodataset.SpatialReference);

                wait.SetCaption("正在填写内容...");
                WriteElement(pTempWS, pGeodataset.SpatialReference, xMin, pEnv.YMax, pEnv.XMax, pEnv.YMin);
                //添加标题
                IPoint btPoint = new PointClass();
                double btX = (WKP3.X + WKP2.X) * 0.5;
                double btY = (WKP3.Y + WKP2.Y) * 0.5 + 16;
                btPoint.PutCoords(btX, btY);
                Font dotNetFont = new Font("黑体", 1, FontStyle.Bold);
                ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                textSymbol.Size = 16 / 25.4 * 72.0;
                textSymbol.Angle = 0;
                textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = txtBT.Text;
                IElement element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);

                //添加标记要素
                AddZJElement(pResultFC, pSpatialReference);

                //this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
                IGraphicsContainerSelect pGCSelect = this.m_PageControl.PageLayout as IGraphicsContainerSelect;
                pGCSelect.UnselectAllElements();

                ICommand myTool = new ControlsMapPanToolClass();
                myTool.OnCreate(this.m_MapControl.Object);
                this.m_MapControl.CurrentTool = myTool as ITool;

                //m_PageControl.ZoomToWholePage();
                m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);

                this.m_myTab.SelectedTabPageIndex = 1;
                wait.Close();
                this.Close();
                MessageBox.Show("生成完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成异常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (wait != null)
                {
                    wait.Close();
                }
                if (pXZQFC != null)
                {
                    Marshal.FinalReleaseComObject(pXZQFC);
                }
                if (pTFHPC != null)
                {
                    Marshal.FinalReleaseComObject(pTFHPC);
                }
                if (pTempWS != null)
                {
                    Marshal.FinalReleaseComObject(pTempFeaWS);
                }
                if (pTempFeaWS != null)
                {
                    Marshal.FinalReleaseComObject(pTempFeaWS);
                }
            }
        }

        private void WriteElement(IWorkspace tempWS, ISpatialReference pSR, double minX, double maxY, double maxX, double minY)
        {
            IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
            IActiveView pageAct = m_PageControl.ActiveView;
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            int nX = 0, nY = 0;
            double x = 0;
            double y = 0;
            Font dotNetFont = new Font("黑体", 1, FontStyle.Bold);
            ITextSymbol textSymbol = new TextSymbolClass();
            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
            ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
            textSymbol.Size = 100;
            textSymbol.Angle = 0;
            textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
            
            DataTable dtTFNew = null;
            IFeatureClass pTFNEW = null;
            if ((tempWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, "tfResult"])
            {
                dtTFNew = EsriDatabaseHelper.ITable2DataTable(tempWS, "Select * From tfResult");
                pTFNEW = (tempWS as IFeatureWorkspace).OpenFeatureClass("tfResult");
            }
            DataTable dtMJ = LS_SetupMDBHelper.GetDataTable("Select * From TFMJ" + cmbCTBLC.Text, "TFMJ");
            List<string> hengTF1 = new List<string>();
            List<string> shuTF1 = new List<string>();
            List<string> hengTF0 = new List<string>();
            List<string> shuTF0 = new List<string>();
            for (int i = 0; i < dtTFNew.Rows.Count; i++)
            {
                string tfh = dtTFNew.Rows[i]["TFH"].ToString();
                string shu = tfh.Substring(4, 3);
                string heng = tfh.Substring(7);
                if (heng.Substring(0, 1) == "0")
                {
                    if (!hengTF0.Contains(heng)) hengTF0.Add(heng);
                }
                else
                {
                    if (!hengTF1.Contains(heng)) hengTF1.Add(heng);
                }
                if (shu.Substring(0, 1) == "0")
                {
                    if (!shuTF0.Contains(shu)) shuTF0.Add(shu);
                }
                else
                {
                    if (!shuTF1.Contains(shu)) shuTF1.Add(shu);
                }
            }
            hengTF0.Sort(); hengTF1.Sort(); shuTF0.Sort(); shuTF1.Sort();
            List<string> hengTF = new List<string>();
            if (hengTF0.Count > 0 && hengTF0.ToArray()[0] == "001")
            {
                hengTF.AddRange(hengTF1); hengTF.AddRange(hengTF0);
            }
            else
            {
                hengTF.AddRange(hengTF0); hengTF.AddRange(hengTF1);
            }
            List<string> shuTF = new List<string>();
            if (shuTF0.Count > 0 && shuTF0.ToArray()[0] == "001")
            {
                shuTF.AddRange(shuTF1); shuTF.AddRange(shuTF0);
            }
            else
            {
                shuTF.AddRange(shuTF0); shuTF.AddRange(shuTF1);
            }
            int n = 0; Boolean b = false;
            foreach (string item in shuTF)
            {
                DataRow[] drs = dtTFNew.Select(string.Format("SUBSTRING(TFH,5,3) = '{0}'", item));
                //填写左侧面积
                int oid = (int)drs.FirstOrDefault()["OBJECTID"]; 
                IFeature pFeature = pTFNEW.GetFeature(oid);
                IPoint pPoint = (pFeature.ShapeCopy as IArea).LabelPoint;
                string lat = MathHelper.ConvertDigitalToDegrees(pPoint.Y);
                DataRow dr = dtMJ.Select(string.Format("minL < '{0}' And maxL > '{0}'", lat)).FirstOrDefault();
                
                getPoint(minX, maxY, 0 - n * dLat - 0.5 * dLat, 0.5 * dLon, pSR, out x, out y);
                IPoint btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                IPoint nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                TextElementClass textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = dr["mj"].ToString();
                IElement element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);
                //填写左侧经纬度
                if (b)
                {
                    getPoint(minX, maxY, 0 - n * dLat, 1.6 * dLon , pSR, out x, out y);
                    btPoint = new PointClass();
                    btPoint.PutCoords(x, y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                    nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    btPoint.PutCoords(nPoint.X, nPoint.Y);
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = dr["maxL"].ToString();
                    element = (IElement)textEle;
                    element.Geometry = btPoint;
                    pageCon.AddElement(element, 0);

                }
                b = true;
                //填写右侧面积
                var query = drs.AsEnumerable().GroupBy(t => t.Field<string>("TFH"))
                      .Select(g => new
                      {
                          c1 = g.Key,
                          c2 = g.Count(),
                          c3 = g.Sum(m => Convert.ToDouble(m["jsmj"]))
                      });
                int ffgs = query.AsEnumerable().Where(t => t.c2 > 1).Count();
                double ffmj = query.AsEnumerable().Where(t => t.c2 > 1).Sum(g => g.c3);
                int mfgs = query.AsEnumerable().Where(t => t.c2 == 1).Count();
                double mfmj = query.AsEnumerable().Where(t => t.c2 == 1).Sum(g => g.c3);
                double qmj = ffmj + mfmj;
                getPoint(maxX, maxY, 0 - n * dLat - 0.5 * dLat, 1.2 * dLon, pSR, out x, out y);
                btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ffgs.ToString();
                element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);

                getPoint(maxX, maxY, 0 - n * dLat - 0.5 * dLat, 2 * dLon, pSR, out x, out y);
                btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = ffmj.ToString();
                element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);

                getPoint(maxX, maxY, 0 - n * dLat - 0.5 * dLat, 2.75 * dLon, pSR, out x, out y);
                btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = mfgs.ToString();
                element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);

                getPoint(maxX, maxY, 0 - n * dLat - 0.5 * dLat, 3.5 * dLon, pSR, out x, out y);
                btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = mfmj.ToString();
                element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);

                getPoint(maxX, maxY, 0 - n * dLat - 0.5 * dLat, 4.5 * dLon, pSR, out x, out y);
                btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = qmj.ToString();
                element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);
                n++;
            }

            //填写上下经纬度
            DataTable dtTF = EsriDatabaseHelper.ITable2DataTable(GlobalEditObject.GlobalWorkspace, "Select * From TFH");
            pTFNEW = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("TFH");
            b = false; n = 0;
            foreach (string item in hengTF)
            {
                //下侧面积
                double mj = dtTFNew.Select(string.Format("SUBSTRING(TFH,8,3) = '{0}'", item)).Sum(m => Convert.ToDouble(m["jsmj"]));
                getPoint(minX, minY, 0 - 1.5 * dLat, n * dLon + 2.5 * dLon, pSR, out x, out y);
                IPoint btPoint = new PointClass();
                btPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                IPoint nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                btPoint.PutCoords(nPoint.X, nPoint.Y);
                ITextElement textEle = new TextElementClass();
                textEle.Symbol = textSymbol;
                textEle.Text = mj.ToString();
                IElement element = (IElement)textEle;
                element.Geometry = btPoint;
                pageCon.AddElement(element, 0);
                if (b)
                {
                    //上经纬度
                    DataRow[] drs = dtTF.Select(string.Format("SUBSTRING(TFH,8,3) = '{0}'", item));
                    int oid = (int)drs.FirstOrDefault()["OBJECTID"];
                    IFeature pFeature = pTFNEW.GetFeature(oid);
                    string lon = MathHelper.ConvertDigitalToDegrees(pFeature.ShapeCopy.Envelope.XMin, 1);
                    getPoint(minX, maxY, 0.5 * dLat, n * dLon + 2 * dLon, pSR, out x, out y);
                    btPoint = new PointClass();
                    btPoint.PutCoords(x, y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                    nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    btPoint.PutCoords(nPoint.X, nPoint.Y);
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = lon;
                    element = (IElement)textEle;
                    element.Geometry = btPoint;
                    pageCon.AddElement(element, 0);
                    //下经纬度
                    getPoint(minX, minY, 0 - 0.5 * dLat, n * dLon + 2 * dLon, pSR, out x, out y);
                    btPoint = new PointClass();
                    btPoint.PutCoords(x, y);
                    mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(btPoint, out nX, out nY);
                    nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    btPoint.PutCoords(nPoint.X, nPoint.Y);
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = lon;
                    element = (IElement)textEle;
                    element.Geometry = btPoint;
                    pageCon.AddElement(element, 0);
                }
                b = true;
                n++;
            }
            //左上面积
            getPoint(minX, maxY, 1.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            IPoint pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            IPoint nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            ITextElement txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "图幅理论面积";
            IElement ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左上比例尺
            getPoint(minX, maxY, 0.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "1：" + cmbCTBLC.Text;
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左上纬度
            getPoint(minX, maxY, dLat, 1.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "纬度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左下说明
            getPoint(minX, minY, 0 - 1.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "界内纵向累加值";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左下比例尺
            getPoint(minX, minY, 0 - 1.5 * dLat, 1.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "1：" + cmbCTBLC.Text;
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左下说明
            getPoint(minX, minY, 0 - 2.5 * dLat, dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "界内总面积合计";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左下纬度
            getPoint(minX, minY, 0 - 0.25 * dLat, 1.25 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "纬度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //左下经度
            getPoint(minX, minY, 0 - 0.75 * dLat, 1.75 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "经度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下纬度
            getPoint(maxX, minY, 0 - 0.25 * dLat, 0.75 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "纬度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下经度
            getPoint(maxX, minY, 0 - 0.75 * dLat, 0.25 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "经度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下说明
            getPoint(maxX, minY, 0 - 1.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "横向合计";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下说明
            getPoint(maxX, minY, 0 - 2.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "界内总面积";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上纬度
            getPoint(maxX, maxY, dLat, 0.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "纬度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 1.5 * dLat, 3 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "界内横向累加值";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 0.75 * dLat, 3 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "1：" + cmbCTBLC.Text;
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 0.25 * dLat, 1.25 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "破幅个数";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 0.25 * dLat, 2 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "破幅面积";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 0.25 * dLat, 2.75 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "整幅个数";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 0.25 * dLat, 3.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "整幅面积";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右上说明
            getPoint(maxX, maxY, 0.25 * dLat, 4.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "整幅面积";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //上经度
            getPoint((minX + maxX) / 2, maxY, 1.5 * dLat, 0, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = "经度";
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //下面积
            var q = dtTFNew.AsEnumerable().GroupBy(t => t.Field<string>("TFH"))
                      .Select(g => new
                      {
                          c1 = g.Key,
                          c2 = g.Count(),
                          c3 = g.Sum(m => Convert.ToDouble(m["jsmj"]))
                      });
            int zffgs = q.AsEnumerable().Where(t => t.c2 > 1).Count();
            double zffmj = q.AsEnumerable().Where(t => t.c2 > 1).Sum(g => g.c3);
            int zmfgs = q.AsEnumerable().Where(t => t.c2 == 1).Count();
            double zmfmj = q.AsEnumerable().Where(t => t.c2 == 1).Sum(g => g.c3);
            double zmj = zffmj + zmfmj;
            //下面积
            getPoint((minX + maxX) / 2, minY, 0 - 2.5 * dLat, 0, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zmj.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下面积
            getPoint(maxX, minY, 0 - 2.5 * dLat, 3 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zmj.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下面积
            getPoint(maxX, minY, 0 - 1.5 * dLat, 3 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zmj.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
            //右下汇总面积
            getPoint(maxX, minY, 0 - 0.5 * dLat, 1.2 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zffgs.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);

            getPoint(maxX, minY, 0 - 0.5 * dLat, 2 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zffmj.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);

            getPoint(maxX, minY, 0 - 0.5 * dLat, 2.75 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zmfgs.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);

            getPoint(maxX, minY, 0 - 0.5 * dLat, 3.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zmfmj.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);

            getPoint(maxX, minY, 0 - 0.5 * dLat, 4.5 * dLon, pSR, out x, out y);
            pP = new PointClass();
            pP.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(pP, out nX, out nY);
            nP = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            pP.PutCoords(nP.X, nP.Y);
            txtEle = new TextElementClass();
            txtEle.Symbol = textSymbol;
            txtEle.Text = zmj.ToString();
            ele = (IElement)txtEle;
            ele.Geometry = pP;
            pageCon.AddElement(ele, 0);
        }

        private Boolean JSMJ(IWorkspace tempWS, ISpatialReference pSR)
        {
            //ISpatialReferenceFactory pfactory = new SpatialReferenceEnvironmentClass();
            //ISpatialReference pSR = pfactory.CreateGeographicCoordinateSystem(4490);
            //根据字典中的理论面积给每个图幅计算面积，破幅的根据比例分配
            DataTable dtZMJ = LS_SetupMDBHelper.GetDataTable("Select * From TFMJ" + cmbCTBLC.Text, "TFMJ");
            if (dtZMJ == null || dtZMJ.Rows.Count == 0)
            {
                MessageBox.Show("图幅控制面积没有找到。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            DataTable dtTFNew = null;
            IFeatureClass pTFNEW = null;
            if ((tempWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, "tfResult"])
            {
                dtTFNew = EsriDatabaseHelper.ITable2DataTable(tempWS, "Select * From tfResult");
                pTFNEW = (tempWS as IFeatureWorkspace).OpenFeatureClass("tfResult");
            }
            if (dtTFNew != null)
            {
                IFeatureClass pTFClass = (GlobalEditObject.GlobalWorkspace as IFeatureWorkspace).OpenFeatureClass("TFH");
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pTFCursor = pTFClass.Search(null, true);
                    comRel.ManageLifetime(pTFCursor);
                    IFeature pTF;
                    while ((pTF = pTFCursor.NextFeature()) != null)
                    {
                        string tfh = FeatureHelper.GetFeatureStringValue(pTF, "TFH");
                        IPoint pPoint = (pTF.ShapeCopy as IArea).LabelPoint;
                        //pPoint.Project(pSR);
                        string lat = MathHelper.ConvertDigitalToDegrees(pPoint.Y);
                        DataRow dr = dtZMJ.Select(string.Format("minL < '{0}' And maxL > '{0}'", lat)).FirstOrDefault();
                        if (dr != null)
                        {
                            double zmj = 0;
                            double.TryParse(dr["mj"].ToString(), out zmj);
                            DataRow[] drs = dtTFNew.Select("TFH = '" + tfh + "'");
                            if (drs.Count() == 1)
                            {
                                drs[0]["jsmj"] = zmj;
                            }
                            else
                            {
                                Dictionary<int, double> oidmj = new Dictionary<int, double>();
                                for (int n = 0; n < drs.Count(); n++)
                                {
                                    int oid = (int)drs[n]["OBJECTID"];
                                    IFeature pTFN = pTFNEW.GetFeature(oid);
                                    IGeometry pGeo = pTFN.ShapeCopy;
                                    pGeo.Project(pSR);
                                    oidmj.Add(oid, (pGeo as IArea).Area);
                                }
                                double zmj2 = oidmj.Values.Sum();
                                Dictionary<int, double> oidmjs = new Dictionary<int, double>();
                                foreach (KeyValuePair<int,double> item in oidmj)
                                {
                                    oidmjs.Add(item.Key, Math.Round(item.Value / zmj2 * zmj, 1));
                                }
                                zmj2 = oidmjs.Values.Sum();
                                var dicSort = from objDic in oidmjs orderby objDic.Value descending select objDic;
                                if (Math.Round(zmj2, 1) != zmj)
                                { 
                                    if (zmj2 > zmj)
                                    {
                                        foreach (KeyValuePair<int,double> item in dicSort)
                                        {
                                            oidmjs[item.Key] = item.Value - 0.1;
                                            if (zmj == oidmjs.Values.Sum()) break;
                                        }
                                    }
                                    else
                                    {
                                        foreach (KeyValuePair<int, double> item in dicSort)
                                        {
                                            oidmjs[item.Key] = item.Value + 0.1;
                                            if (zmj == oidmjs.Values.Sum()) break;
                                        }
                                    }
                                }
                                for (int n = 0; n < drs.Count(); n++)
                                {
                                    drs[n]["jsmj"] = oidmjs[(int)drs[n]["OBJECTID"]];
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("没有找到" + lat + "的控制面积。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
                using (ESRI.ArcGIS.ADF.ComReleaser comR = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pUpdate = pTFNEW.Update(null, true);
                    comR.ManageLifetime(pUpdate);
                    int index = pTFNEW.FindField("jsmj");
                    IFeature pTF;
                    while ((pTF = pUpdate.NextFeature()) != null)
                    {
                        pTF.set_Value(index, dtTFNew.Select("OBJECTID = " + pTF.OID).FirstOrDefault()["jsmj"]);
                        pUpdate.UpdateFeature(pTF);
                    }
                    pUpdate.Flush();
                }
            }
            return true;
        }

        private void DrawLines(double xMin, double xMax, double yMin, double yMax, double xMinE, double xMaxE, double yMinE, double yMaxE, ISpatialReference pSR)
        {
            IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
            IActiveView pageAct = m_PageControl.ActiveView;
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            double x = 0; double y = 0;
            int nX = 0, nY = 0;
            int m;
            IPoint fromPoint = new PointClass();
            fromPoint.PutCoords(xMin, yMin);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            IPoint nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            IPoint toPoint = new PointClass();
            toPoint.PutCoords(xMax, yMin);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            PolylineClass NewPol = new PolylineClass();
            IPointCollection pCol = (IPointCollection)NewPol;
            object Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);
            SimpleLineSymbolClass lineSym = new SimpleLineSymbolClass();
            lineSym.Width = 0.2;
            IColor eleColor = ColorHelper.CreateColor(0, 0, 0);
            lineSym.Color = eleColor;
            LineElementClass LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);

            //全的横线和左侧横线
            Boolean b = false;
            m = 1;
            double yNow = yMin;
            while (yNow < yMax)
            {
                getPoint(xMin, yNow, dLat, 0, pSR, out x, out y);
                if (y > yMax) break;
                if (Math.Abs(y - yMaxE) < 1000) { y = yMaxE + 0.1; b = true; }
                fromPoint = new PointClass();
                fromPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                fromPoint.PutCoords(nPoint.X, nPoint.Y);

                getPoint(xMin, yNow, dLat, dLon, pSR, out x, out y);
                if (Math.Abs(y - yMaxE) < 1000) { y = yMaxE + 0.1; b = true; }
                toPoint = new PointClass();
                if (y < yMinE || y > yMaxE)
                {
                    toPoint.PutCoords(xMax, y); 
                }
                else
                {
                    toPoint.PutCoords(x, y);
                }
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                toPoint.PutCoords(nPoint.X, nPoint.Y);

                NewPol = new PolylineClass();
                pCol = (IPointCollection)NewPol;
                Missing = Type.Missing;
                pCol.AddPoint(fromPoint, ref Missing, ref Missing);
                pCol.AddPoint(toPoint, ref Missing, ref Missing);
                
                LineEle = new LineElementClass();
                LineEle.Geometry = (IGeometry)NewPol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);
                yNow = y;
                m++;
                if (b) break;
            }
            //上1
            getPoint(xMin, yNow, dLat, 0, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMin, yNow, dLat, dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            //上2
            getPoint(xMin, yNow, dLat, 2 * dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMax, yNow, dLat, 0 - 5 * dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            //上3
            getPoint(xMax, yNow, dLat, 0 - 4 * dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMax, yNow, dLat, 0, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            //上4
            getPoint(xMax, yNow, 0.5 * dLat, 0 - 4 * dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMax, yNow, 0.5 * dLat, 0, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            yNow = y;
            //上5
            getPoint(xMin, yNow, 1.5 * dLat, 0, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMax, yNow, 1.5 * dLat, 0, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);

            //右侧横线
            yNow = yMinE;
            while (yNow < yMaxE)
            {
                getPoint(xMaxE, yNow, dLat, dLon, pSR, out x, out y);
                if (y > yMax) break;
                if (Math.Abs(y - yMaxE) < 1000) y = yMaxE + 0.1;
                fromPoint = new PointClass();
                fromPoint.PutCoords(x, y);
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                fromPoint.PutCoords(nPoint.X, nPoint.Y);

                getPoint(xMaxE, yNow, dLat, 5 * dLon, pSR, out x, out y);
                if (Math.Abs(y - yMaxE) < 1000) y = yMaxE + 0.1;
                toPoint = new PointClass();
                toPoint.PutCoords(x, y);
                
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                toPoint.PutCoords(nPoint.X, nPoint.Y);

                NewPol = new PolylineClass();
                pCol = (IPointCollection)NewPol;
                Missing = Type.Missing;
                pCol.AddPoint(fromPoint, ref Missing, ref Missing);
                pCol.AddPoint(toPoint, ref Missing, ref Missing);

                LineEle = new LineElementClass();
                LineEle.Geometry = (IGeometry)NewPol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);
                yNow = y;
            }

            //竖线
            fromPoint = new PointClass();
            fromPoint.PutCoords(xMin, yMin);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            toPoint = new PointClass();
            toPoint.PutCoords(xMin, yMax);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);
            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            b = false;
            m = 1;
            double xNow = xMin;
            while (xNow < xMax + double.Parse(cmbCTBLC.Text))
            {
                getPoint(xNow, yMin, dLat, dLon, pSR, out x, out y);
                if (x > xMax) break;
                if (Math.Abs(x - xMaxE) < 1000) { x = xMaxE + 0.1; b = true; }
                if (b) getPoint(xNow, yMin, 0, dLon, pSR, out x, out y);
                fromPoint = new PointClass();
                fromPoint.PutCoords(x, y);
                if (m == 2)
                {
                    getPoint(xNow, yMin, 0, dLon, pSR, out x, out y);
                    fromPoint.PutCoords(x, y);
                }
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                fromPoint.PutCoords(nPoint.X, nPoint.Y);

                getPoint(xNow, yMax, 0, dLon, pSR, out x, out y);
                if (Math.Abs(x - xMaxE) < 1000) { x = xMaxE + 0.1; b = true; }
                
                toPoint = new PointClass();
                if (x < xMinE || x > xMaxE)
                {
                    toPoint.PutCoords(x, y);
                }
                else
                {
                    getPoint(xNow, yMinE, 0 - dLat, dLon, pSR, out x, out y);
                    toPoint.PutCoords(x, y);
                }
                
                mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
                nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                toPoint.PutCoords(nPoint.X, nPoint.Y);

                NewPol = new PolylineClass();
                pCol = (IPointCollection)NewPol;
                Missing = Type.Missing;
                pCol.AddPoint(fromPoint, ref Missing, ref Missing);
                pCol.AddPoint(toPoint, ref Missing, ref Missing);

                LineEle = new LineElementClass();
                LineEle.Geometry = (IGeometry)NewPol;
                LineEle.Symbol = lineSym;
                pageCon.AddElement(LineEle, 0);
                xNow = x;
                m++;
                if (b) break;
            }
            //右1
            getPoint(xNow, yMin, 0, dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xNow, yMax, 0, dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            xNow = x;
            //右2
            getPoint(xNow, yMin, 2 * dLat, 0.5 * dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xNow, yMax, 0 - 1.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            xNow = x;
            //右3
            getPoint(xNow, yMin, 2 * dLat, dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xNow, yMax, 0 - 1.5 * dLat, dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            xNow = x;
            //右4
            getPoint(xNow, yMin, 2 * dLat, 0.5 * dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xNow, yMax, 0 - 1.5 * dLat, 0.5 * dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            xNow = x;
            //右5
            getPoint(xNow, yMin, 2 * dLat, dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xNow, yMax, 0 - 1.5 * dLat, dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            xNow = x;
            //右6
            getPoint(xNow, yMin, 0, dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xNow, yMax, 0, dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            xNow = x;

            //左斜
            getPoint(xMin, yMin, 2 * dLat, dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMin, yMin, 3 * dLat, 2 * dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
            //右斜
            getPoint(xMax, yMin, 3 * dLat, 0 - 5 * dLon, pSR, out x, out y);
            fromPoint = new PointClass();
            fromPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(fromPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            fromPoint.PutCoords(nPoint.X, nPoint.Y);

            getPoint(xMax, yMin,  2 * dLat, 0 - 4 * dLon, pSR, out x, out y);
            toPoint = new PointClass();
            toPoint.PutCoords(x, y);
            mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(toPoint, out nX, out nY);
            nPoint = pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            toPoint.PutCoords(nPoint.X, nPoint.Y);

            NewPol = new PolylineClass();
            pCol = (IPointCollection)NewPol;
            Missing = Type.Missing;
            pCol.AddPoint(fromPoint, ref Missing, ref Missing);
            pCol.AddPoint(toPoint, ref Missing, ref Missing);

            LineEle = new LineElementClass();
            LineEle.Geometry = (IGeometry)NewPol;
            LineEle.Symbol = lineSym;
            pageCon.AddElement(LineEle, 0);
        }

        private void getPoint(double x, double y, double dLat, double dLon, ISpatialReference pSR, out double outX, out double outy)
        {
            ISpatialReferenceFactory pfactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference sLL = pfactory.CreateGeographicCoordinateSystem(4490);
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(x, y);
            pPoint.SpatialReference = pSR;
            pPoint.Project(sLL);
            IPoint newPoint = new PointClass();
            newPoint.PutCoords(pPoint.X + dLon, pPoint.Y + dLat);
            newPoint.SpatialReference = sLL;
            newPoint.Project(pSR);
            outX = newPoint.X;
            outy = newPoint.Y;
        }

        /// <summary>
        /// 出图外框
        /// </summary>
        private void OutOutFrame(double Width, double Height, double dScale)
        {
            double dFrameWidth = Width / dScale * 1000.0;
            double dFrameHeight = Height / dScale * 1000.0;
            //页边距
            double margin = 100;
            double dTop = margin, dLeft = margin;
            double dRight = margin, dBottom = margin;
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

        private void AddZJElement(IFeatureClass pFC, ISpatialReference pSpatialReference)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            IFeatureCursor pFeaCur = null;
            try
            {
                pFeaCur = pFC.Search(null, false);
                IFeature pFea = null;
                IQueryFilter pFilter = new QueryFilterClass();
                while ((pFea = pFeaCur.NextFeature()) != null)
                {
                    string tfh = FeatureHelper.GetFeatureStringValue(pFea, "TFH");
                    pFilter.WhereClause = string.Format("TFH='{0}'", tfh);
                    string showTXT = string.Empty;
                    if (pFC.FeatureCount(pFilter) > 1)//存在重复图幅号，说明是破幅
                    {
                        pFea.Shape.Project(pSpatialReference);
                        showTXT = FeatureHelper.GetFeatureStringValue(pFea, "jsmj");// MathHelper.Round((pFea.ShapeCopy as IArea).Area, 1).ToString();
                    }
                    else
                    {
                        showTXT = tfh;
                    }

                    (pFea.ShapeCopy as ITopologicalOperator).Simplify();
                    IPoint ppoint = (pFea.ShapeCopy as IArea).LabelPoint;
                    ppoint.Project(pSpatialReference);
                    int nX = 0, nY = 0;
                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);

                    Font dotNetFont = new Font("黑体", 1, FontStyle.Bold);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                    textSymbol.Size = 100;
                    textSymbol.Angle = 0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = showTXT;
                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pageCon.AddElement(element, 0);

                    Marshal.FinalReleaseComObject(pFea);
                }
                m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pFeaCur != null)
                {
                    Marshal.FinalReleaseComObject(pFeaCur);
                }
            }
        }

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

        private void UniqValueDrawing(IFeatureLayer pFeaLayer, string field)
        {
            IGeoFeatureLayer pLyr = (pFeaLayer as IGeoFeatureLayer);
            if (pLyr == null)
                return;

            ArrayList arry = FeatureHelper.GetUniqueFieldValueByDataStatistics(pFeaLayer.FeatureClass, null, field);
            try
            {
                IUniqueValueRenderer pRender = new UniqueValueRendererClass();
                #region 获取唯一值

                ISymbol symd = SymbolHelper.CreateTmpSym(pFeaLayer.FeatureClass.ShapeType);

                pRender.FieldCount = 1;
                pRender.set_Field(0, field);
                pRender.DefaultSymbol = symd as ISymbol;
                pRender.UseDefaultSymbol = true;

                for (int i = 0; i < arry.Count; i++)
                {
                    ISymbol symx = SymbolHelper.CreateTmpSym(pFeaLayer.FeatureClass.ShapeType);
                    pRender.AddValue(arry[i].ToString(), field, symx as ISymbol);
                    pRender.set_Label(arry[i].ToString(), arry[i].ToString());
                    pRender.set_Symbol(arry[i].ToString(), symx as ISymbol);
                }
                #endregion

                IRgbColor pColor1 = new RgbColorClass();
                pColor1.Red = 255;
                pColor1.Green = 255;
                pColor1.Blue = 255;

                #region 渲染
                for (int ny = 0; ny < pRender.ValueCount; ny++)
                {
                    string xv = pRender.get_Value(ny);
                    ISymbol jsy = pRender.get_Symbol(xv);
                    (jsy as ISimpleFillSymbol).Color = pColor1;
                    pRender.set_Symbol(xv, jsy);
                }

                pRender.ColorScheme = "Custom";
                pRender.set_FieldType(0, true);
                pLyr.Renderer = pRender as IFeatureRenderer;
                #endregion
            }
            catch (Exception ex) { }

        }

        private IGeometry UnionGeo(IFeatureClass pFC)
        {
            IFeatureCursor pFeaCur = null;
            try
            {
                IGeometry geometryBag = new GeometryBagClass();

                IGeoDataset geoDataset = pFC as IGeoDataset;
                geometryBag.SpatialReference = geoDataset.SpatialReference;

                pFeaCur = pFC.Search(null, false);

                IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;

                IFeature pFea = null;
                object missing = Type.Missing;
                while ((pFea = pFeaCur.NextFeature()) != null)
                {
                    geometryCollection.AddGeometry(pFea.ShapeCopy, ref missing, ref missing);
                }

                ITopologicalOperator unionedPolygon = new PolygonClass();
                unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
                unionedPolygon.Simplify();
                return unionedPolygon as IPolygon;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
