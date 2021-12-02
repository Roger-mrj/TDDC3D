using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using DevExpress.Utils;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

namespace TDDC3D.output
{
    public partial class FrmCreateTFH : Form
    {
        public IMap _pMap = null;
        public AxMapControl myMapControl;

        public FrmCreateTFH()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmCreateTFH_Load(object sender, EventArgs e)
        {
            int iTB = -1;
            int iCount = 0;
            if (_pMap != null)
            {
                for (int i = 0; i < _pMap.LayerCount; i++)
                {
                    ILayer pLayer = _pMap.get_Layer(i);
                    if (pLayer is IFeatureLayer)
                    {
                        if (pLayer.Name == "行政区") iTB = iCount;
                        comLayers.Properties.Items.Add(pLayer.Name);
                        iCount++;
                    }
                }
                if (comLayers.Properties.Items.Count > 0)
                {
                    if (iTB != -1)
                        comLayers.SelectedIndex = iTB;
                    else
                        comLayers.SelectedIndex = 0;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ILayer player =RCIS.GISCommon.LayerHelper.QueryLayerByModelName(_pMap, comLayers.Text);
            if (player == null)
            {
                MessageBox.Show("没有查找到图层！");
                return;
            }
            IFeatureClass pFeatureClass = (player as IFeatureLayer).FeatureClass;
            int iCount = pFeatureClass.FeatureCount(null);
            if (iCount == 0)
            {
                MessageBox.Show("图层中没有数据！");
                return;
            }
            IFeatureLayer tfhLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(_pMap, "tfh") as IFeatureLayer;
            if (tfhLayer == null)
            {
                MessageBox.Show("没有查找到图幅图层！");
                return;
            }

            WaitDialogForm frmWait = new WaitDialogForm("删除图幅表中原有数据", "生成图幅结合表");
            
            IFeatureClass pFC = tfhLayer.FeatureClass;
            try
            {
                IWorkspace pWS = pFC.FeatureDataset.Workspace;
                string fcname = (pFC as IDataset).Name;
                if (pWS != null)
                {
                    pWS.ExecuteSQL("delete from " + fcname);
                }
            }
            catch (Exception ex)
            {
            }

            frmWait.SetCaption("正在生成图幅结合表");

            CreateTF(pFC,pFeatureClass, player.AreaOfInterest.XMin, player.AreaOfInterest.YMin, player.AreaOfInterest.XMax, player.AreaOfInterest.YMax);

            //IPoint pp = new PointClass();
            //pp.X = player.AreaOfInterest.XMax;
            //pp.Y = player.AreaOfInterest.YMax;
            //CreateTF(pp);
            //pp = new PointClass();
            //pp.X = player.AreaOfInterest.XMin;
            //pp.Y = player.AreaOfInterest.YMin;
            //CreateTF(pp);

            

            frmWait.Close();
            MessageBox.Show("图幅生成完成！");
        }

        double dWidth = 0.0, dHeight = 0.0;
        private void CreateTF(IFeatureClass tf,IFeatureClass areaClass, double minX, double minY, double maxX, double maxY)
        {
            double nowX = minX,nowY = minY;
            while (nowX < maxX)
            {
                while(nowY < maxY)
                {
                    IPoint pPoint = new PointClass();
                    pPoint.X = nowX;
                    pPoint.Y = nowY;
                    CreateTF(pPoint,tf,areaClass);
                    nowY += dHeight;
                }
                nowY = minY;
                nowX += dWidth;
            }
            
        }

        private void CreateTF(IPoint pPoint, IFeatureClass tfClass, IFeatureClass areaClass)
        {
            sycCommonLib.sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
            double m_dScale = Convert.ToDouble(comBLC.Text);
            double dJ = 0.0, dW = 0.0;
            string sRetErrorInfo = "";
            bool bRet = CommonClass.syc_XY2JWD(_pMap, pPoint, out dJ, out dW, out sRetErrorInfo);
            IPoint newPt = CoordinateTransHelper.XY2JWD(_pMap, pPoint);
            
            StringBuilder sOldTFH = new StringBuilder(50);
            double dJ1 = 0.0, dW1 = 0.0, dJ3 = 0.0, dW3 = 0.0;
            int nRet = sycCommonLib.DLIB.GetTufuInfo(dJ, dW, m_dScale, ref dJ1, ref dW1, ref dJ3, ref dW3, sOldTFH);
            StringBuilder sNewTFH = new StringBuilder(100);
            nRet = sycCommonLib.DLIB.GetNewWaima(m_dScale, dJ, dW, sNewTFH);

            IPoint pp1 = new PointClass();
            IPoint pp2 = new PointClass();
            IPoint pp3 = new PointClass();
            IPoint pp4 = new PointClass();
            
            CommonClass.syc_JWD2XY(_pMap, dJ1, dW1, ref pp1, out sRetErrorInfo);
            CommonClass.syc_JWD2XY(_pMap, dJ3, dW1, ref pp2, out sRetErrorInfo);
            CommonClass.syc_JWD2XY(_pMap, dJ3, dW3, ref pp3, out sRetErrorInfo);
            CommonClass.syc_JWD2XY(_pMap, dJ1, dW3, ref pp4, out sRetErrorInfo);
            object oo = Type.Missing;
            PolygonClass tf = new PolygonClass();
            ((IPointCollection)tf).AddPoint(pp1, ref oo, ref oo);
            ((IPointCollection)tf).AddPoint(pp2, ref oo, ref oo);
            ((IPointCollection)tf).AddPoint(pp3, ref oo, ref oo);
            ((IPointCollection)tf).AddPoint(pp4, ref oo, ref oo);
            ((IPointCollection)tf).AddPoint(pp1, ref oo, ref oo);
            dWidth = pp3.X - pp1.X;
            dHeight = pp3.Y - pp1.Y;

            ISpatialFilter pSF = new SpatialFilter();
            pSF.Geometry = tf;
            pSF.GeometryField = areaClass.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pFC = areaClass.Search(pSF, false);
            IFeature pF = pFC.NextFeature();
            if (pF != null)
            {
                IFeatureCursor feaCursor = tfClass.Insert(true);
                IFeatureBuffer feaBuffer = tfClass.CreateFeatureBuffer();
                feaBuffer.Shape = tf;// IGeometry;//(这里的IGeometry可以是IPolygon，IPolyline，IPoint)
                int fieldindex = feaBuffer.Fields.FindField("tfh");
                if (fieldindex >= 0)
                {
                    feaBuffer.set_Value(fieldindex, sNewTFH.ToString());
                }
                fieldindex = feaBuffer.Fields.FindField("oldtfh");
                if (fieldindex >= 0)
                {
                    feaBuffer.set_Value(fieldindex, sOldTFH.ToString());
                }
                feaCursor.InsertFeature(feaBuffer);
            }

            //IGraphicsContainer mapCon = myMapControl.ActiveView.GraphicsContainer;
            ////mapCon.DeleteAllElements();

            //RgbColor fillColor = new RgbColorClass();
            //fillColor.Red = 255;
            //SimpleFillSymbolClass fillSym = new SimpleFillSymbolClass();
            //fillSym.Color = fillColor;
            //fillSym.Style = esriSimpleFillStyle.esriSFSCross;

            //PolygonElementClass ele = new PolygonElementClass();
            //ele.Geometry = (IGeometry)tf;
            //ele.Symbol = fillSym;

            //mapCon.AddElement(ele, 0);
            //myMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);


            //myMapControl.ActiveView.ScreenDisplay.UpdateWindow();
            //myMapControl.FlashShape((IGeometry)ele.Geometry, 4, 300, null);
            //myMapControl.Extent = ele.Geometry.Envelope;

        }

        //private ILayer GetLayerByLayerName(IMap pMap, string sName)
        //{
        //    ILayer getlayer = null;
        //    ILayer pLayer = null;
        //    for (int i = 0; i < pMap.LayerCount; i++)
        //    {
        //        pLayer = pMap.get_Layer(i);
        //        if (pLayer.Name.ToUpper() == sName.ToUpper())
        //        {
        //            getlayer = pLayer;
        //            break;
        //        }
        //    }
        //    return getlayer;
        //}

        //private ILayer GetLayerByTableName(IMap pMap, string sName)
        //{
        //    ILayer getlayer = null;
        //    ILayer pLayer = null;
        //    for (int i = 0; i < pMap.LayerCount; i++)
        //    {
        //        pLayer = pMap.get_Layer(i);
        //        if (pLayer is IFeatureLayer)
        //        {
        //            IFeatureLayer pfeaturelayer = pLayer as IFeatureLayer;
        //            IDataset pdataset = pfeaturelayer.FeatureClass as IDataset;
        //            if (pdataset.Name.ToUpper() == sName.ToUpper())
        //            {
        //                getlayer = pLayer;
        //                break;
        //            }
        //        }
        //    }
        //    return getlayer;
        //}
    }
}
