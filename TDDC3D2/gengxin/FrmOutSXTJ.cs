using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using System.Collections;
using RCIS.Utility;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;

namespace TDDC3D.gengxin
{
    public partial class FrmOutSXTJ : Form
    {
        public FrmOutSXTJ()
        {
            InitializeComponent();
        }

        //变量
        public DevExpress.XtraTab.XtraTabControl m_myTab;

        public ESRI.ArcGIS.Controls.AxMapControl m_MapControl;
        public ESRI.ArcGIS.Controls.AxPageLayoutControl m_PageControl;
        private IMapFrame m_myMapFrame;
        public IWorkspace currWS = null;

        public double m_dJ1, m_dW1, m_dJ3, m_dW3;

        IFeatureLayer dltbLayer = null;
        IFeatureLayer xzqLayer = null;
        IFeatureLayer pzwjstdLayer = null;
        IFeatureLayer czcdydLayer = null;
        IFeatureLayer tfhLayer = null; //tfh图层

        IFeatureLayer cjdcqLayer = null; //村级行政区


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
                    return (IMapFrame)ele;
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

        private void FrmOutSXTJ_Load(object sender, EventArgs e)
        {
            ZJSetup();

            this.xzqLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "XZQ");
            this.dltbLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "DLTB");
            this.pzwjstdLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "PZWJSTD");
            this.czcdydLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "CZCDYD");
            this.tfhLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "TFH");

            //3-29 日增加 村级行政区
            this.cjdcqLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.ActiveView.FocusMap, "CJDCQ");
        }

        private void beDestDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            string destDirs = dlg.SelectedPath;
            this.beDestDir.Text = destDirs;
        }

        private void btnPatchXzqT_Click(object sender, EventArgs e)
        {
            if (this.beDestDir.Text.Trim() == "")
            {
                MessageBox.Show("请指定输出路径！");
                return;
            }
            string destDir = this.beDestDir.Text.Trim();

            IMap myMap = this.m_MapControl.ActiveView.FocusMap;
            m_myMapFrame = this.GetIMapFrame();


            if (xzqLayer == null)
            {
                MessageBox.Show("请首先加载行政区图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            iTH = 1;

            //初始化变量            
            string sScale = cmbCTBLC.SelectedItem.ToString();
            double dScale = Convert.ToDouble(sScale);

            double dBufferJl = 0;
            double.TryParse(this.textBoxTKJL.Text, out dBufferJl);
            double dMJL = dBufferJl * dScale / 1000.0;		//M

            string tmp = this.beDestDir.Text.Trim() + "\\tmp.gdb";
            IWorkspace tmpWS = null;
            if (Directory.Exists(tmp))
            {
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(tmp);
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
                pWorkspaceFactory.Create(this.beDestDir.Text.Trim(), "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(tmp);
            }

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在按行政单位输出图件和数据...", "请稍等...");
            wait.Show();

            //IMap pMap=this.m_MapControl.ActiveView.FocusMap;
            //for (int i = 0; i < pMap.LayerCount; i++)
            //{
            //    if (pMap.get_Layer(i) is GroupLayer)
            //    {
            //        ICompositeLayer pLayer = pMap.get_Layer(i) as ICompositeLayer;
            //        for (int j = 0; j < pLayer.Count; j++)
            //        {
            //            string name = (((pLayer.get_Layer(j) as IDataLayer).DataSourceName) as IDatasetName).Name;
            //            bool a = RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWS, tmpWS, name,name , null);
            //        }
            //    }
            //}

            IQueryFilter pQueryFilter = null;
            if (this.txtPatchXzdm.Text.Trim() != "")
            {
                pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = "ZLDWDM like '" + this.txtPatchXzdm.Text.Trim() + "%'";
            }
            wait.SetCaption("正在输出地类图斑数据...");
            bool b = RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWS, tmpWS, "DLTB", "DLTB", pQueryFilter);
            wait.SetCaption("正在输出村级调查区数据...");
            b = RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWS, tmpWS, "CJDCQ", "CJDCQ", pQueryFilter);
            if (pQueryFilter != null)
                pQueryFilter.WhereClause = "XZQDM like '" + this.txtPatchXzdm.Text.Trim() + "%'";
            wait.SetCaption("正在输出行政区数据...");
            b = RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(currWS, tmpWS, "XZQ", "XZQ", pQueryFilter);
            IFeatureClass pClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            //缩编
            TBSB(pClass, "TBMJ", double.Parse(txtDltbMjRX.Text), wait);
            try
            {
                wait.SetCaption("正在输出" + txtPatchXzdm.Text + "土地利用图...");
                IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(cjdcqLayer.FeatureClass);
                pGeo.SpatialReference = m_MapControl.SpatialReference;
                OutACunT(pGeo, dBufferJl, dScale, destDir,tmp,tmpWS as IFeatureWorkspace);
                wait.Close();
                MessageBox.Show("输出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
                //RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                //OtherHelper.ReleaseComObject(pQf);

            }
        }


        /// <summary>
        /// 输出一个村图
        /// </summary>
        private void OutACunT(IGeometry xzqGeo, double dBufferJl, double dScale, string destDir,string dataPath,IFeatureWorkspace pDataSourceFW)
        {

            sycCommonLib.sycCommonFuns CommonClassDLL = new sycCommonLib.sycCommonFuns();
            
            IMap myMap = this.m_MapControl.ActiveView.FocusMap;
            this.m_RealCtPolygon = xzqGeo;
            this.m_bufferPolygon = (m_RealCtPolygon as ITopologicalOperator).Buffer(dBufferJl);
            this.m_MapControl.Extent = m_bufferPolygon.Envelope;

            this.m_MapControl.Map.ClearSelection();
            this.m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.m_MapControl.ActiveView.Extent.Envelope);
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            deleteAllElement(pageCon);

            m_PageControl.ActiveView.FocusMap.MapScale = dScale;
            this.m_MapControl.ActiveView.FocusMap.MapScale = dScale;

            IObjectCopy objectCopy = new ObjectCopyClass();
            object toCopyMap = this.m_MapControl.ActiveView.FocusMap;
            object copiedMap = objectCopy.Copy(toCopyMap);
            object toOverwriteMap = m_PageControl.ActiveView.FocusMap;
            objectCopy.Overwrite(copiedMap, ref toOverwriteMap);

            m_PageControl.ActiveView.Extent = m_bufferPolygon.Envelope;  //确定当前区域            

            OutOutFrame(dScale);

            SetSymboZJ(pDataSourceFW);
            
            //[04]:ZhengShi:

            IActiveView mapAct = m_PageControl.ActiveView.FocusMap as IActiveView;
            IActiveView pageAct = m_PageControl.ActiveView;
            IPointCollection pCol = m_bufferPolygon as IPointCollection;


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
            jwdPt.PutCoords(m_dJ1, m_dW1);
            NKP1 = CoordinateTransHelper.JWD2XY(myMap, jwdPt);
            jwdPt.PutCoords(m_dJ3, m_dW1);
            NKP2 = CoordinateTransHelper.JWD2XY(myMap, jwdPt);
            jwdPt.PutCoords(m_dJ3, m_dW3);
            NKP3 = CoordinateTransHelper.JWD2XY(myMap, jwdPt);
            jwdPt.PutCoords(m_dJ1, m_dW3);
            NKP4 = CoordinateTransHelper.JWD2XY(myMap, jwdPt);

            env = new EnvelopeClass();
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

            //注记标题等
            #region 注记标题等
            if (true)
            {
                string sBT = "";
                sBT = this.memoBTZJ.Text + "\r\n" + m_XzqMC + "\r\n" + m_XzqDm;
                string sLD = this.memoZxjZj.Text;
                string sRD = this.memoYXJZJ.Text;

                string delimStr = "\r\n";
                char[] delimiter = delimStr.ToCharArray();
                string[] sBTSz = sBT.Split(delimiter);
                int nLen = sBTSz.Length;
                int nJS = 0;
                double dZG = 8;
                double dTextAF = CommonClassDLL.syc_CalAngle(ref WKP4, ref WKP3) * 180.0 / Math.PI;

                for (int i = nLen - 1; i >= 0; i--)
                {
                    string ss = sBTSz[i].Trim();
                    if (ss.Length == 0)
                        continue;

                    PointClass pp1 = new PointClass();
                    pp1.X = (WKP3.X + WKP4.X) * 0.5;
                    pp1.Y = (WKP3.Y + WKP4.Y) * 0.5;
                    PointClass pp2 = new PointClass();
                    pp2.X = pp1.X;
                    pp2.Y = pp1.Y + 16.0 + nJS * (dZG + 2.0);
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
                    element.Geometry = pp2;
                    pageCon.AddElement(element, 0);
                }

                string[] LDSz = sLD.Split(delimiter);
                nLen = LDSz.Length;
                nJS = 0;
                dZG = 3.5;

                for (int i = 0; i < nLen; i++)
                {
                    string ss = LDSz[i].Trim();
                    if (ss.Length == 0)
                        continue;

                    PointClass pp2 = new PointClass();
                    pp2.X = NKP1.X;
                    pp2.Y = NKP1.Y - 16 - nJS * (dZG + 2.0);
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
                    element.Geometry = pp2;
                    pageCon.AddElement(element, 0);
                }

                string[] RDSz = sRD.Split(delimiter);
                nLen = RDSz.Length;
                nJS = 0;
                dZG = 3.5;

                for (int i = 0; i < nLen; i++)
                {
                    string ss = RDSz[i].Trim();
                    if (ss.Length == 0)
                        continue;
                    ss = ss + "     ";

                    PointClass pp2 = new PointClass();
                    pp2.X = NKP2.X - 30.0;
                    pp2.Y = NKP2.Y - 16 - nJS * (dZG + 2.0);
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
                    element.Geometry = pp2;
                    pageCon.AddElement(element, 0);
                }

                if (true)
                {
                    PointClass pp1 = new PointClass();
                    pp1.X = (WKP1.X + WKP2.X) * 0.5;
                    pp1.Y = (WKP1.Y + WKP2.Y) * 0.5;
                    PointClass pp2 = new PointClass();
                    pp2.X = pp1.X;
                    pp2.Y = pp1.Y - 10.0;

                    int nScale = Convert.ToInt32(dScale);
                    string ss = "1:" + nScale.ToString();
                    System.Drawing.Font dotNetFont = new System.Drawing.Font("宋体", 1, FontStyle.Bold);
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
                    element.Geometry = pp2;
                    pageCon.AddElement(element, 0);
                }
            }
            #endregion

            #region   //方里网

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

            if (true)
            {

                ArrayList pLegendFiles = GetAllLegendInfo();
                ArrayList MSz = this.getUniqDLBM();
                ArrayList OtherSz = this.getOtherUniLegend();

                #region  生成 图例
                //图例:
                IPoint TLP = new PointClass();
                double dA = CommonClassDLL.syc_CalAngle(ref NKP4, ref NKP3);
                ((IConstructPoint)TLP).ConstructAngleDistance(tmpP3, dA, 15.0);

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
                                        sTLName = sFileName.Substring(nPos + 1, nPos2 - nPos - 1);
                                        sTLFile = Application.StartupPath + @"\图例\" + sFileName;
                                        break;
                                    }
                                }
                            }
                        }

                        //Create Elements:
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
                            Missing = Type.Missing;
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

                            dotNetFont = new System.Drawing.Font("黑体", 1);
                            textSymbol = new TextSymbolClass();
                            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                            dZH = 3.5;	//mm
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
                    } //for(int iM=0;...
                } //if(MSz.Count!=0)



                //...
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
                            Missing = Type.Missing;
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

                            dotNetFont = new System.Drawing.Font("黑体", 1);
                            textSymbol = new TextSymbolClass();
                            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                            dZH = 3.5;	//mm
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
                    } //for(int iX=0;...
                }
                #endregion

                //... ...
            }



            outMjGTDW(LDP, RUP);

            this.m_MapControl.ActiveView.GraphicsContainer.DeleteAllElements();
            IGraphicsContainerSelect pGCSelect = this.m_PageControl.PageLayout as IGraphicsContainerSelect;
            pGCSelect.UnselectAllElements();

            m_PageControl.ZoomToWholePage();
            m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);

            IMxdContents pMxd = m_PageControl.PageLayout as IMxdContents;
            IMapDocument m_MapDocument = new MapDocumentClass();
            string destFile = destDir + @"\" + m_XzqDm + m_XzqMC + "土地利用图" + (iTH < 0 ? "" : (iTH++).ToString()) + ".mxd";
            m_MapDocument.New(destFile);
            m_MapDocument.ReplaceContents(pMxd);

            m_MapDocument.Save(m_MapDocument.UsesRelativePaths, true);
            RCIS.Utility.OtherHelper.ReleaseComObject(m_MapDocument);
            CommonClassDLL.Dispose();

            IFeatureLayer pXZQfl = null;
            IFeatureLayer pDLTBfl = null;
            IFeatureLayer pPZWJSXMfl = null;
            IFeatureLayer pCZCfl = null;
            
            IMapDocument pMapDoc = new MapDocumentClass();
            pMapDoc.Open(destFile);
            IMap pMap = pMapDoc.get_Map(0);
            IWorkspace pWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(dataPath);
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLayer = pMap.get_Layer(i);
                if (pLayer != null)
                {
                    if (pLayer is GroupLayer)
                    {
                        ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                        for (int j = 0; j < pCLayer.Count; j++)
                        {
                            IFeatureLayer layer = pCLayer.get_Layer(j) as IFeatureLayer;
                            IDataLayer2 pDataLayer = (IDataLayer2)layer;
                            IDatasetName pDsName = (IDatasetName)(pDataLayer.DataSourceName);
                            try
                            {
                                layer.FeatureClass = (pWS as IFeatureWorkspace).OpenFeatureClass(pDsName.Name);
                            }
                            catch { }
                            if (pDsName.Name.ToUpper().Trim() == "XZQ")
                                pXZQfl = layer;
                            if (pDsName.Name.ToUpper().Trim() == "DLTB")
                                pDLTBfl = layer;
                            if (pDsName.Name.ToUpper().Trim() == "CZCDYD")
                                pCZCfl = layer;
                            if (pDsName.Name.ToUpper().Trim() == "PZWJSXM")
                                pPZWJSXMfl = layer;
                        }

                    }
                    else
                    {
                        IFeatureLayer layer = pLayer as IFeatureLayer;
                        IDataLayer2 pDataLayer = (IDataLayer2)layer;
                        IDatasetName pDsName = (IDatasetName)(pDataLayer.DataSourceName);
                        try
                        {
                            layer.FeatureClass = (pWS as IFeatureWorkspace).OpenFeatureClass(pDsName.Name);
                        }
                        catch { }
                        if (pDsName.Name.ToUpper().Trim() == "XZQ")
                            pXZQfl = layer;
                        if (pDsName.Name.ToUpper().Trim() == "DLTB")
                            pDLTBfl = layer;
                        if (pDsName.Name.ToUpper().Trim() == "CZCDYD")
                            pCZCfl = layer;
                        if (pDsName.Name.ToUpper().Trim() == "PZWJSXM")
                            pPZWJSXMfl = layer;
                    }

                }
            }
            //IGraphicsContainer pGpContainer = pMap as IGraphicsContainer;

            pMapDoc.Save(pMapDoc.UsesRelativePaths, true);


        }

        private void TBSB(IFeatureClass pFeatureclass, string fieldName, double minMJ, DevExpress.Utils.WaitDialogForm wait) 
        {
            string[] nydArr = { "0101", "0102", "0103", "0201", "0202", "0203", "0204", "0301", "0302", "0303", "0304",
                                  "0305", "0306", "0307", "0401", "0402", "0403", "1006", "1103", "1104", "1107", "1202", "1203"};
            string[] jsydArr = { "05H1", "0508", "0601", "0602", "0603", "0701", "0702", "08H1", "08H2", "0809", "0810",
                                   "09", "1001", "1002", "1003", "1004", "1005", "1007", "1008", "1009", "1109", "1201"};
            string[] wlydArr = { "0404", "1101", "1102", "1105", "1106", "1108", "1110", "1204", "1205", "1206", "1207"};
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = ""+fieldName+"<"+minMJ+"";
            int count = pFeatureclass.FeatureCount(pQueryFilter);
            int num = 1;
            IFeatureCursor pFeatureCursor = pFeatureclass.Update(pQueryFilter,true);
            IFeature pFeature;
            List<IFeature> noIntersect = new List<IFeature>();
            while((pFeature=pFeatureCursor.NextFeature())!=null)
            {
                wait.SetCaption("图斑缩编中："+num+++"/"+count+"");
                double mj=0;
                double.TryParse(pFeature.get_Value(pFeatureclass.FindField(fieldName)).ToString(),out mj);
                if (mj > minMJ)
                    continue;
                string pdlbm = pFeature.get_Value(pFeatureclass.FindField("DLBM")).ToString();
                string[] arr = { };
                if (nydArr.Contains(pdlbm))
                    arr = nydArr;
                else if (jsydArr.Contains(pdlbm))
                    arr = jsydArr;
                else
                    arr = wlydArr;
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = pFeature.ShapeCopy;
                pSF.GeometryField = pFeatureclass.ShapeFieldName;
                //pSF.WhereClause = "" + fieldName + ">=" + minMJ + "";
                pSF.SpatialRel=esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = pFeatureclass.Search(pSF, true);
                IFeature pIntersectsFea=pCursor.NextFeature();
                List<int> dlArr = new List<int>();
                List<int> tdlArr = new List<int>();
                List<int> sdlArr = new List<int>();
                bool b = false;
                while(pIntersectsFea!=null)
                {
                    if (pFeature.OID.ToString() != pIntersectsFea.OID.ToString())
                    {
                        string dlbm = pIntersectsFea.get_Value(pIntersectsFea.Fields.FindField("DLBM")).ToString();
                        if (pdlbm == dlbm)
                        {
                            ITopologicalOperator pTO = (pIntersectsFea.ShapeCopy) as ITopologicalOperator;
                            pIntersectsFea.Shape = pTO.Union(pFeature.ShapeCopy);
                            double mj1 = 0, mj2 = 0;
                            double.TryParse(pIntersectsFea.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj1);
                            double.TryParse(pFeature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj2);
                            pIntersectsFea.set_Value(pFeatureclass.FindField(fieldName), mj1 + mj2);
                            pIntersectsFea.Store();
                            pFeatureCursor.DeleteFeature();
                            b = true;
                            RCIS.Utility.OtherHelper.ReleaseComObject(pTO);
                            break;
                        }
                        else
                        {
                            if (pdlbm.Substring(0, 2) == dlbm.Substring(0, 2))
                            {
                                tdlArr.Add(pIntersectsFea.OID);
                            }
                            else
                            {
                                if (arr.Contains(pdlbm) && arr.Contains(dlbm))
                                    sdlArr.Add(pIntersectsFea.OID);
                                else
                                    dlArr.Add(pIntersectsFea.OID);
                            }
                        }
                    }
                    pIntersectsFea = pCursor.NextFeature();
                }
                pCursor.Flush();
                RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                if (b == false)
                {
                    if (tdlArr.Count > 0)
                    {
                        IFeature feature = pFeatureclass.GetFeature(tdlArr[0]);
                        ITopologicalOperator pTO = (feature.ShapeCopy) as ITopologicalOperator;
                        feature.Shape = pTO.Union(pFeature.ShapeCopy);
                        double mj1 = 0, mj2 = 0;
                        double.TryParse(feature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj1);
                        double.TryParse(pFeature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj2);
                        feature.set_Value(pFeatureclass.FindField(fieldName), mj1 + mj2);
                        feature.Store();
                        pFeatureCursor.DeleteFeature();
                        RCIS.Utility.OtherHelper.ReleaseComObject(feature);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pTO);
                        continue;
                    }
                    else
                    {
                        if (sdlArr.Count > 0)
                        {
                            IFeature feature = pFeatureclass.GetFeature(sdlArr[0]);
                            ITopologicalOperator pTO = (feature.ShapeCopy) as ITopologicalOperator;
                            feature.Shape = pTO.Union(pFeature.ShapeCopy);
                            double mj1 = 0, mj2 = 0;
                            double.TryParse(feature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj1);
                            double.TryParse(pFeature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj2);
                            feature.set_Value(pFeatureclass.FindField(fieldName), mj1 + mj2);
                            feature.Store();
                            pFeatureCursor.DeleteFeature();
                            RCIS.Utility.OtherHelper.ReleaseComObject(feature);
                            RCIS.Utility.OtherHelper.ReleaseComObject(pTO);
                            continue;
                        }
                        else
                        {
                            if (dlArr.Count > 0)
                            {
                                IFeature feature = pFeatureclass.GetFeature(dlArr[0]);
                                ITopologicalOperator pTO = (feature.ShapeCopy) as ITopologicalOperator;
                                feature.Shape = pTO.Union(pFeature.ShapeCopy);
                                double mj1 = 0, mj2 = 0;
                                double.TryParse(feature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj1);
                                double.TryParse(pFeature.get_Value(pFeatureclass.FindField(fieldName)).ToString(), out mj2);
                                feature.set_Value(pFeatureclass.FindField(fieldName), mj1 + mj2);
                                feature.Store();
                                pFeatureCursor.DeleteFeature();
                                RCIS.Utility.OtherHelper.ReleaseComObject(feature);
                                RCIS.Utility.OtherHelper.ReleaseComObject(pTO);
                                continue;
                            }
                            
                        }
                    }
                }
            }
            pFeatureCursor.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pFeatureCursor);
        }


        public class FLW
        {
            public FLW() { }

            public string sBS;
            public string sZJ1, sZJ2;
            public IPoint PP1, PP2;
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

            //  OtherSz.Add("地类界.tif");  //此处貌似不对，后续再改
            //IFeatureLayer dljxLayer = LayerHelper.QueryLayerByModelName(this.m_MapControl.Map, "DLJX");
            //if (dljxLayer!=null)
            //{
            //    IFeatureClass dljxClass = dljxLayer.FeatureClass;
            //    ArrayList arrDljxs = FeatureHelper.GetUniqueFieldValueByDataStatistics(dljxClass, "DLJX", this.m_RealCtPolygon);
            //}
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


                IElement ele = m_myMapFrame as IElement;
                IEnvelope env = ele.Geometry.Envelope;
                LDP = new PointClass();
                LDP.X = env.XMin;
                LDP.Y = env.YMin;
                RUP = new PointClass();
                RUP.X = env.XMax;
                RUP.Y = env.YMax;
                IGraphicsContainer pageCon = m_PageControl.GraphicsContainer;

                PointClass NeedP = new PointClass();
                NeedP.X = LDP.X - dCTDW_GAP;
                NeedP.Y = LDP.Y;

                System.Drawing.Font dotNetFont = new System.Drawing.Font("黑体", 1, FontStyle.Regular);
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

                NeedP.X = RUP.X;
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
                if (eleArr[i] is IMapFrame) ;
                else pageCon.DeleteElement((IElement)eleArr[i]);
            }
            #endregion
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

            m_PageControl.ActiveView.FocusMap.ClipGeometry = m_RealCtPolygon;
            m_PageControl.ActiveView.Refresh();

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


        /// <summary>
        /// 生成注记
        /// </summary>
        private void SetSymboZJ(IFeatureWorkspace pTmpWS)
        {
            IGraphicsContainer pageCon = m_PageControl.ActiveView.GraphicsContainer;
            IFeatureClass pDLTB = null;
            try 
            {
                pDLTB = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch { }
            if (pDLTB != null)
            {
                if (this.chkDltbZJ.Checked)
                {
                    //ISpatialFilter pSF = new SpatialFilterClass();
                    //pSF.Geometry = m_RealCtPolygon;
                    //pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    //pSF.WhereClause = " TBMJ  > " + txtDltbMjRX.Text;

                    //int iCount = this.dltbLayer.FeatureClass.FeatureCount(pSF as IQueryFilter);
                    //bool isLarge = iCount > 5000 ? true : false;
                    //if (this.chkDltbZJ.Checked && !isLarge)
                    //{
                        SymbolizeDLTBZJ(pageCon, pDLTB, m_RealCtPolygon);
                    //}
                }
                if (this.chkTBXMC.Checked)
                {
                    SymbolizeTBXHMCZJ(pageCon, pDLTB, m_RealCtPolygon);
                }

            }

            IFeatureClass pXZQ = null;
            try 
            {
                pXZQ = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("XZQ");
            }
            catch { }
            if (pXZQ != null)
            {
                if (this.chkXZQZJ.Checked)
                {
                    SymbolizeXZQZJ(pageCon, pXZQ, m_bufferPolygon);
                }

            }
            IFeatureClass pPZWJSXM = null;
            try
            {
                pPZWJSXM = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("PZWJSXM");
            }
            catch { }
            if (pPZWJSXM != null)
            {
                if (this.chkPZWJSXM.Checked)
                {
                    SymbolizePzwjstdZJ(pageCon, pPZWJSXM, m_RealCtPolygon);
                }
            }
            IFeatureClass pCZC = null;
            try
            {
                pCZC = (pTmpWS as IFeatureWorkspace).OpenFeatureClass("CZCDYD");
            }
            catch { }
            if (pCZC != null)
            {
                if (this.chkCzcDM.Checked)
                {
                    SymbolizeCZCDMZJ(pageCon, pCZC, m_RealCtPolygon);
                }
            }
        }

        # region 注记
        private void SymbolizeDLTBZJ(IGraphicsContainer pGc, IFeatureClass dltbLyr, IGeometry pGeo)
        {
            // IArea area = pGeo as IArea;

            double dltbMjRx = 0.0;
            try
            {
                dltbMjRx = double.Parse(txtDltbMjRX.Text);
            }
            catch { }


            //IIdentify dltbIndentify = dltbLyr as IIdentify;
            IFeatureCursor pCursor = dltbLyr.Search(null,true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                if ((pFeature.Shape as IArea).Area > dltbMjRx)
                {

                    IPoint ppoint = (pFeature.ShapeCopy as IArea).LabelPoint;
                    int nX = 0, nY = 0;
                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sTBBH = FeatureHelper.GetFeatureStringValue(pFeature, "TBBH");
                    string sQsxz = FeatureHelper.GetFeatureStringValue(pFeature, "QSXZ");
                    string sDL = FeatureHelper.GetFeatureStringValue(pFeature, "DLBM");


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
                    //if (i > 0 && i % 10000 == 0)
                        m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                }
            }
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);

            //IArray arrDltbIDs = dltbIndentify.Identify(pGeo);
            //for (int i = 0; i < arrDltbIDs.Count; i++)
            //{
            //    IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
            //    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
            //    IFeature pfea = pRow.Row as IFeature;

            //    if ((pfea.Shape as IArea).Area > dltbMjRx)
            //    {

            //        IPoint ppoint = (pfea.ShapeCopy as IArea).LabelPoint;
            //        int nX = 0, nY = 0;
            //        (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
            //        IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            //        string sTBBH = FeatureHelper.GetFeatureStringValue(pfea, "TBBH");
            //        string sQsxz = FeatureHelper.GetFeatureStringValue(pfea, "QSXZ");
            //        string sDL = FeatureHelper.GetFeatureStringValue(pfea, "DLBM");


            //        if (sQsxz.StartsWith("1") || sQsxz.StartsWith("2"))
            //        {
            //            sQsxz = "G";
            //        }
            //        else
            //        {
            //            sQsxz = "";
            //        }


            //        //IPoint pPT =  GetPoint(curZJP, 0.25, 0.15);

            //        #region //分子:
            //        System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboDLTBFont.Text, 1, FontStyle.Underline);
            //        ITextSymbol textSymbol = new TextSymbolClass();
            //        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

            //        textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboDLTBSize.Text.Trim());
            //        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            //        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
            //        textSymbol.Color = ColorHelper.CreateColor(this.ceDLTB.Color);
            //        TextElementClass textEle = new TextElementClass();
            //        textEle.Symbol = textSymbol;
            //        textEle.Text = sTBBH + sQsxz;
            //        IElement element = (IElement)textEle;
            //        element.Geometry = curZJP;
            //        pGc.AddElement(element, 0);
            //        #endregion
            //        #region  //分母
            //        dotNetFont = new System.Drawing.Font(this.cboDLTBFont.Text, 1);
            //        textSymbol = new TextSymbolClass();
            //        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

            //        textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboDLTBSize.Text.Trim());
            //        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            //        textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
            //        textSymbol.Color = ColorHelper.CreateColor(this.ceDLTB.Color);
            //        textEle = new TextElementClass();
            //        textEle.Symbol = textSymbol;
            //        textEle.Text = sDL;
            //        element = (IElement)textEle;
            //        element.Geometry = curZJP;
            //        pGc.AddElement(element, 0);
            //        #endregion
            //        if (i > 0 && i % 10000 == 0)
            //            m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            //    }
            //}
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
        private void SymbolizeXZQZJ(IGraphicsContainer pGc, IFeatureClass xzqLyr, IGeometry pGeo)
        {

            IFeatureCursor pCursor = xzqLyr.Search(null,true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                IGeometry pXZQGeo = pFeature.ShapeCopy;
                // IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(pXZQGeo, esriGeometryDimension.esriGeometry2Dimension);
                IPoint ppoint = (pXZQGeo as IArea).LabelPoint;  //(pInterGeo as IArea).LabelPoint;
                int nX = 0, nY = 0;

                (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                string sText = FeatureHelper.GetFeatureStringValue(pFeature, "XZQMC");

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
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);


            //IIdentify xzqId = xzqLyr as IIdentify;

            //IArray arXZQIDS = xzqId.Identify(pGeo);
            //if (arXZQIDS == null)
            //    return;

            //try
            //{
            //    for (int i = 0; i < arXZQIDS.Count; i++)
            //    {
            //        IFeatureIdentifyObj idObj = arXZQIDS.get_Element(i) as IFeatureIdentifyObj;
            //        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
            //        IFeature pfea = pRow.Row as IFeature;
            //        IGeometry pXZQGeo = pfea.ShapeCopy;
            //        // IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(pXZQGeo, esriGeometryDimension.esriGeometry2Dimension);
            //        IPoint ppoint = (pXZQGeo as IArea).LabelPoint;  //(pInterGeo as IArea).LabelPoint;
            //        int nX = 0, nY = 0;

            //        (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
            //        IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            //        string sText = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");

            //        System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXZQFont.Text, 1);
            //        ITextSymbol textSymbol = new TextSymbolClass();
            //        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

            //        textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXZQSize.Text);
            //        textSymbol.Color = ColorHelper.CreateColor(this.ceXZQ.Color);
            //        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            //        TextElementClass textEle = new TextElementClass();
            //        textEle.Symbol = textSymbol;
            //        textEle.Text = sText;

            //        IElement element = (IElement)textEle;
            //        element.Geometry = curZJP;
            //        pGc.AddElement(element, 0);
            //    }
            //}
            //catch (Exception ex)
            //{ }



        }
        //批准未建设项目名称注记
        private void SymbolizePzwjstdZJ(IGraphicsContainer pGc, IFeatureClass pzwjstdLyr, IGeometry pGeo)
        {
            IFeatureCursor pCursor = pzwjstdLyr.Search(null,true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                IGeometry APzwjsGeo = pFeature.ShapeCopy;
                IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
                IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                int nX = 0, nY = 0;

                (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                string sText = FeatureHelper.GetFeatureStringValue(pFeature, "BZXMMC");

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
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            //IIdentify pzwjsIdentify = pzwjstdLyr as IIdentify;
            //IArray arPzwjstdIds = pzwjsIdentify.Identify(pGeo);
            //if (arPzwjstdIds == null)
            //    return;
            //try
            //{
            //    for (int i = 0; i < arPzwjstdIds.Count; i++)
            //    {
            //        IFeatureIdentifyObj idObj = arPzwjstdIds.get_Element(i) as IFeatureIdentifyObj;
            //        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
            //        IFeature pfea = pRow.Row as IFeature;
            //        IGeometry APzwjsGeo = pfea.ShapeCopy;
            //        IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
            //        IPoint ppoint = (pInterGeo as IArea).LabelPoint;
            //        int nX = 0, nY = 0;

            //        (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
            //        IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            //        string sText = FeatureHelper.GetFeatureStringValue(pfea, "BZXMMC");

            //        System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXMMCFont.Text, 1);
            //        ITextSymbol textSymbol = new TextSymbolClass();
            //        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

            //        textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXMMCFontsize.Text);
            //        textSymbol.Color = ColorHelper.CreateColor(this.ceXMMCColor.Color);
            //        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            //        TextElementClass textEle = new TextElementClass();
            //        textEle.Symbol = textSymbol;
            //        textEle.Text = sText;

            //        IElement element = (IElement)textEle;
            //        element.Geometry = curZJP;
            //        pGc.AddElement(element, 0);
            //    }
            //}
            //catch (Exception ex)
            //{ }
        }

        private void SymbolizeCZCDMZJ(IGraphicsContainer pGc, IFeatureClass czcdmLayer, IGeometry pGeo)
        {
            IFeatureCursor pCursor = czcdmLayer.Search(null,true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                IGeometry APzwjsGeo = pFeature.ShapeCopy;
                IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
                IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                int nX = 0, nY = 0;

                (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                string sText = FeatureHelper.GetFeatureStringValue(pFeature, "CZCLX");

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
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            //IIdentify czcdydIdentify = czcdmLayer as IIdentify;
            //IArray arCzcdmIds = czcdydIdentify.Identify(pGeo);
            //if (arCzcdmIds == null)
            //    return;
            //try
            //{
            //    for (int i = 0; i < arCzcdmIds.Count; i++)
            //    {
            //        IFeatureIdentifyObj idObj = arCzcdmIds.get_Element(i) as IFeatureIdentifyObj;
            //        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
            //        IFeature pfea = pRow.Row as IFeature;
            //        IGeometry APzwjsGeo = pfea.ShapeCopy;
            //        IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
            //        IPoint ppoint = (pInterGeo as IArea).LabelPoint;
            //        int nX = 0, nY = 0;

            //        (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
            //        IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            //        string sText = FeatureHelper.GetFeatureStringValue(pfea, "CZCLX");

            //        System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboCZCDMFont.Text, 1);
            //        ITextSymbol textSymbol = new TextSymbolClass();
            //        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

            //        textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboCZCDMFontSize.Text);
            //        textSymbol.Color = ColorHelper.CreateColor(this.ceCZCDMColor.Color);
            //        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            //        TextElementClass textEle = new TextElementClass();
            //        textEle.Symbol = textSymbol;
            //        textEle.Text = sText;

            //        IElement element = (IElement)textEle;
            //        element.Geometry = curZJP;
            //        pGc.AddElement(element, 0);
            //    }
            //}
            //catch (Exception ex)
            //{ }
        }

        //细化名称
        private void SymbolizeTBXHMCZJ(IGraphicsContainer pGc, IFeatureClass dltbLyr, IGeometry pGeo)
        {
            IFeatureCursor pCursor = dltbLyr.Search(null,true);
            IFeature pFeature;
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                IGeometry APzwjsGeo = pFeature.ShapeCopy;
                IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
                IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                ppoint.PutCoords(ppoint.X + 5, ppoint.Y + 5);
                int nX = 0, nY = 0;

                (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                string sText = FeatureHelper.GetFeatureStringValue(pFeature, "TBXHMC");

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
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            //IIdentify dltbIdentify = dltbLyr as IIdentify;
            //IArray arDltbIds = dltbIdentify.Identify(pGeo);
            //if (arDltbIds == null)
            //    return;
            //try
            //{
            //    for (int i = 0; i < arDltbIds.Count; i++)
            //    {
            //        IFeatureIdentifyObj idObj = arDltbIds.get_Element(i) as IFeatureIdentifyObj;
            //        IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
            //        IFeature pfea = pRow.Row as IFeature;
            //        IGeometry APzwjsGeo = pfea.ShapeCopy;
            //        IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(APzwjsGeo, esriGeometryDimension.esriGeometry2Dimension);
            //        IPoint ppoint = (pInterGeo as IArea).LabelPoint;
            //        ppoint.PutCoords(ppoint.X + 5, ppoint.Y + 5);
            //        int nX = 0, nY = 0;

            //        (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
            //        IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
            //        string sText = FeatureHelper.GetFeatureStringValue(pfea, "TBXHMC");

            //        System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboTBXHMCFont.Text, 1);
            //        ITextSymbol textSymbol = new TextSymbolClass();
            //        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

            //        textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboTBXHMCFontSize.Text);
            //        textSymbol.Color = ColorHelper.CreateColor(this.ceTBXHColor.Color);
            //        textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            //        TextElementClass textEle = new TextElementClass();
            //        textEle.Symbol = textSymbol;
            //        textEle.Text = sText;

            //        IElement element = (IElement)textEle;
            //        element.Geometry = curZJP;
            //        pGc.AddElement(element, 0);
            //    }
            //}
            //catch (Exception ex)
            //{ }
        }
        #endregion

        

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

            string sql = "JB=9 and QSDWDM like '" + this.txtPatchXzdm.Text.Trim() + "'";
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
    }
}
