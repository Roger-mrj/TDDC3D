using System;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using System.Runtime.InteropServices;
using sycCommonLib;

namespace sycWS
{
	/// <summary>
	/// 
	/// </summary>
	public class LSOutTool : ESRI.ArcGIS.ADF.BaseClasses.BaseTool
	{
		private IHookHelper m_HookHelper = new HookHelperClass();

		public ESRI.ArcGIS.Controls.AxMapControl myMapControl;
		public string m_sFW;
		public double m_dMMJL;
		public string m_sScale;
		public LSOutForm m_UseForm;
		public LSOutTool()
		{
			// 
			// TODO: 在此处添加构造函数逻辑
			//
		}
	
		public override void OnCreate(object hook)
		{
			// TODO:  添加 LSOutTool.OnCreate 实现
			m_HookHelper.Hook = hook;
		}
        private IFeature GetXZQFea(string sName,IPoint pp)
        {
            string sNeedFeatureClassName = "XZQ" ;
            IFeatureLayer  needLayer = null;
            needLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(myMapControl.Map, sNeedFeatureClassName);
            if (needLayer == null)
            {
                return null;
            }
            //for (int i = 0; i < myMapControl.Map.LayerCount; i++)
            //{
            //    ILayer myLayer = myMapControl.get_Layer(i);
            //    if (myLayer is IFeatureLayer)
            //    {
            //        IFeatureLayer pFLayer = myLayer as IFeatureLayer;
            //        IFeatureClass cls = pFLayer.FeatureClass;
            //        IDataset dataSet = cls as IDataset;
            //        string sLayerName = (((myLayer as IFeatureLayer).FeatureClass) as IDataset).BrowseName.ToUpper();
            //        int nPos = sLayerName.LastIndexOf(".");
            //        if (nPos != -1)
            //        {
            //            string ss = sLayerName.Substring(nPos + 1).Trim().ToUpper();
            //            sLayerName = ss.Trim().ToUpper();
            //        }
            //        if (sLayerName.Equals(sNeedFeatureClassName) == true)
            //        {
            //            needLayer = myLayer as IFeatureLayer;
            //            break;
            //        }
            //    }
            //}

            IIdentify pIdentify = needLayer as IIdentify;
            IArray pIDs = pIdentify.Identify((IGeometry)pp);
            if (pIDs == null)
            {
                return null;
            }
            
            for (int i = 0; i < pIDs.Count; i++)
            {
                //找满足条件的第一个特性:
                IFeatureIdentifyObj pFeatIdObj = pIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRowObj = pFeatIdObj as IRowIdentifyObject;
                IFeature pFeat = pRowObj.Row as IFeature;
                return pFeat;
            }
            return null;
        }
        //private IFeatureLayer Fealyr(string lyrName)
        //{
        //    for (int i = 0; i < myMapControl.Map.LayerCount; i++)
        //    {
        //        ILayer myLayer = myMapControl.get_Layer(i);
        //        if (myLayer is IFeatureLayer)
        //        {
        //            IFeatureLayer pFLayer = myLayer as IFeatureLayer;
        //            IFeatureClass cls = pFLayer.FeatureClass;
        //            IDataset dataSet = cls as IDataset;
        //            string sLayerName = (((myLayer as IFeatureLayer).FeatureClass) as IDataset).BrowseName.ToUpper();
        //            int nPos = sLayerName.LastIndexOf(".");
        //            if (nPos != -1)
        //            {
        //                string ss = sLayerName.Substring(nPos + 1).Trim().ToUpper();
        //                sLayerName = ss.Trim().ToUpper();
        //            }
        //            if (sLayerName.Equals(lyrName) == true)
        //            {
        //                return pFLayer;
        //                break;
        //            }
        //        }
        //    }
        //    return null;
        //}
		public override void OnMouseDown(int Button, int Shift, int X, int Y)
		{
			// TODO:  添加 LSOutTool.OnMouseDown 实现
			if(Button==1) 
			{
                sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
                double m_dScale = Convert.ToDouble(m_sScale);

                if ((m_sFW.Equals("村图") == true) ||
                    (m_sFW.Equals("乡图") == true) ||
                    (m_sFW.Equals("县图") == true))
                {
                    //行政区在一个表内、不再区分县乡村，目前按照XZQDM[12]来区分:
                    //XX　XX   XX   XXX  XXX
                    //省  市　县  乡  村
                    //假如最后的3位不为"000" ==> 村;
                    //如果最后3位为"000"、而倒数第二个3位不等"000" ==> 乡;
                    string sNeedFeatureClassName = "XZQ";
                    string sNeedNameField = "XZQMC";
                    string sNeedCodeField = "XZQDM";

                    IMap myMap = (IMap)myMapControl.ActiveView.FocusMap;
                   
                    IFeatureLayer needLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(myMap, sNeedFeatureClassName);
                    if (needLayer == null)
                    {
                        MessageBox.Show(" 在当前加载的数据中、没发现需要的特性类[" + sNeedFeatureClassName + "]! ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }

                    IActiveView act = myMapControl.ActiveView.FocusMap as IActiveView;
                    IPoint pp = act.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    IIdentify pIdentify = needLayer as IIdentify; //行政区层查找
                    IArray pIDs = pIdentify.Identify((IGeometry)pp);
                    if (pIDs == null)
                    {
                        MessageBox.Show(" 没找到需要的区域、重来... ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    IGeometry lsgeo = null;
                    IFeature needFeat = null;
                    for (int i = 0; i < pIDs.Count; i++)
                    {
                        //找满足条件的第一个特性:
                        IFeatureIdentifyObj pFeatIdObj = pIDs.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRowObj = pFeatIdObj as IRowIdentifyObject;
                        IFeature pFeat = pRowObj.Row as IFeature;
                        int nPos = pFeat.Fields.FindField(sNeedCodeField);
                        object oCode = pFeat.get_Value(nPos);
                        if (oCode is System.DBNull)
                            continue;
                        string sCode = oCode.ToString().Trim();
                       
                        if (m_sFW.Equals("村图") == true)
                        {
                            //if (sCode.Length != 12)
                            //    continue;
                            //检查后３位:
                          IFeature lsfea=  GetXZQFea(sCode, pp);
                          string sCode2 = lsfea.get_Value(nPos).ToString();
                          if (sCode2.Substring(9).Equals("000") == false)
                          {
                              needFeat = lsfea; //这就算找到该村了，
                              break;
                          }
                        }
                        else if (m_sFW.Equals("乡图") == true)
                        {

                            #region  //第一个倒数３位＝"000"，而第二个倒数３位不为"000";
                            //IFeatureLayer Lyrfea = Fealyr("XZQ");
                            IQueryFilter qur = new QueryFilterClass();
                            //  qur.WhereClause = "substr(XZQDM,";
                            //-------
                            IFeature lsfea = GetXZQFea(sCode, pp);
                            string sCode2 = lsfea.get_Value(nPos).ToString();

                            qur.WhereClause = "\"XZQDM\" like  '" + sCode2.Substring(0, 9) + "%'";

                            IFeatureCursor pCur = needLayer.FeatureClass.Search(qur, false);
                            IFeature xfea = pCur.NextFeature();
                            if (xfea == null)
                                break;
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

                            lsgeo = uGeo;
                            (lsgeo as ITopologicalOperator).Simplify();
                            lsgeo.Project(this.myMapControl.Map.SpatialReference);
                            needFeat = pFeat;
                            #endregion 

                            break;

                        }
                        else if (m_sFW.Equals("县图") == true)
                        {
                            needFeat = pFeat;
                            break;
                        }
                    }
                    if (needFeat == null)
                    {
                        MessageBox.Show(" 没找到需要的区域、重来... ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                 
                    string sRetName = "", sRetCode = "";
                    int nIndex = needFeat.Fields.FindField(sNeedNameField);
                    if (nIndex != -1)
                    {
                        object oName = needFeat.get_Value(nIndex);
                        sRetName = oName.ToString();
                    }
                    nIndex = needFeat.Fields.FindField(sNeedCodeField);
                    if (nIndex != -1)
                    {
                        object oCode = needFeat.get_Value(nIndex);
                        sRetCode = oCode.ToString();

                    }
                    nIndex = needFeat.Fields.FindField("Shape");
                   
                    object oShape = needFeat.get_Value(nIndex);
                    if (lsgeo == null)
                    {
                        lsgeo= oShape as IGeometry;
                    }
                    myMapControl.FlashShape(lsgeo, 3, 300, null);
                    double dMJL = m_dMMJL * m_dScale / 1000.0;		//M

                    IGeometry bufferPolygon = lsgeo;
                    if (dMJL > 0.0)
                        bufferPolygon = ((ITopologicalOperator)lsgeo).Buffer(dMJL);

                    IEnvelope env = ((IGeometry)bufferPolygon).Envelope;
                    IPoint LDP = new PointClass();
                    LDP.PutCoords(env.XMin, env.YMin);
                    IPoint RDP = new PointClass();
                    RDP.PutCoords(env.XMax, env.YMin);
                    IPoint RUP = new PointClass();
                    RUP.PutCoords(env.XMax, env.YMax);
                    IPoint LUP = new PointClass();
                    LUP.PutCoords(env.XMin, env.YMax);

                    myMap = myMapControl.ActiveView.FocusMap;
                    double dJ1 = 0.0, dW1 = 0.0;
                    string sRetErrorInfo = "";
                    bool bRet = syc_XY2JWD(myMap, LDP, out dJ1, out dW1, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    double dJ2 = 0.0, dW2 = 0.0;
                    sRetErrorInfo = "";
                    bRet = syc_XY2JWD(myMap, RDP, out dJ2, out dW2, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    double dJ3 = 0.0, dW3 = 0.0;
                    sRetErrorInfo = "";
                    bRet = syc_XY2JWD(myMap, RUP, out dJ3, out dW3, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    double dJ4 = 0.0, dW4 = 0.0;
                    sRetErrorInfo = "";
                    bRet = syc_XY2JWD(myMap, LUP, out dJ4, out dW4, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
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

                    m_UseForm.m_dJ1 = dMinJ;
                    m_UseForm.m_dW1 = dMinW;
                    m_UseForm.m_dJ3 = dMaxJ;
                    m_UseForm.m_dW3 = dMaxW;
                    m_UseForm.m_sName = sRetName;
                    m_UseForm.m_sCode = sRetCode;
                    m_UseForm.m_MyPolygon = lsgeo;
                    m_UseForm.m_bufferPolygon = (IGeometry)bufferPolygon;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;
                    //... ...
                }
                else if (m_sFW.Equals("标准分幅") == true)
                {
                    //标准分幅:

                    //[01]:... ...
                    IActiveView act = myMapControl.ActiveView.FocusMap as IActiveView;
                    IPoint pp = act.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

                    IMap myMap = myMapControl.ActiveView.FocusMap;
                    double dJ = 0.0, dW = 0.0;
                    string sRetErrorInfo = "";
                    bool bRet = syc_XY2JWD(myMap, pp, out dJ, out dW, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }

                    double dJ1 = 0.0, dW1 = 0.0, dJ3 = 0.0, dW3 = 0.0;
                    StringBuilder sOldTFH = new StringBuilder(50);
                    int nRet = DLIB.GetTufuInfo(dJ, dW, m_dScale, ref dJ1, ref dW1, ref dJ3, ref dW3, sOldTFH);
                    if (nRet == 0)
                    {
                        MessageBox.Show("非梯形分幅或者其他的坐标问题，无法计算出所在图幅!");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    StringBuilder sNewTFH = new StringBuilder(100);
                    nRet = DLIB.GetNewWaima(m_dScale, dJ, dW, sNewTFH);
                    if (nRet == -1)
                    {
                        MessageBox.Show("非梯形分幅或者其他的坐标问题，无法计算出图幅号!");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    string sBZTFH = sNewTFH.ToString();

                    IPoint pp1 = new PointClass();
                    IPoint pp2 = new PointClass();
                    IPoint pp3 = new PointClass();
                    IPoint pp4 = new PointClass();
                    CommonClass.syc_JWD2XY(myMap, dJ1, dW1, ref pp1, out sRetErrorInfo);
                    CommonClass.syc_JWD2XY(myMap, dJ3, dW1, ref pp2, out sRetErrorInfo);
                    CommonClass.syc_JWD2XY(myMap, dJ3, dW3, ref pp3, out sRetErrorInfo);
                    CommonClass.syc_JWD2XY(myMap, dJ1, dW3, ref pp4, out sRetErrorInfo);

                    object oo = Type.Missing;
                    PolygonClass tf = new PolygonClass();
                    ((IPointCollection)tf).AddPoint(pp1, ref oo, ref oo);
                    ((IPointCollection)tf).AddPoint(pp2, ref oo, ref oo);
                    ((IPointCollection)tf).AddPoint(pp3, ref oo, ref oo);
                    ((IPointCollection)tf).AddPoint(pp4, ref oo, ref oo);
                    ((IPointCollection)tf).AddPoint(pp1, ref oo, ref oo);

                    IGraphicsContainer mapCon = myMapControl.ActiveView.GraphicsContainer;
                    mapCon.DeleteAllElements();

                    RgbColor fillColor = new RgbColorClass();
                    fillColor.Red = 255;
                    SimpleFillSymbolClass fillSym = new SimpleFillSymbolClass();
                    fillSym.Color = fillColor;
                    fillSym.Style = esriSimpleFillStyle.esriSFSCross;

                    PolygonElementClass ele = new PolygonElementClass();
                    ele.Geometry = (IGeometry)tf;
                    ele.Symbol = fillSym;

                    mapCon.AddElement(ele, 0);
                    myMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    //返回到主界面:
                    m_UseForm.m_MyPolygon = (IGeometry)ele.Geometry;
                    m_UseForm.m_bufferPolygon = (IGeometry)ele.Geometry;
                    m_UseForm.m_sTFH = sBZTFH;
                    m_UseForm.m_dJ1 = dJ1;
                    m_UseForm.m_dW1 = dW1;
                    m_UseForm.m_dJ3 = dJ3;
                    m_UseForm.m_dW3 = dW3;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;
                    //... ...
                }
                else if (m_sFW.Equals("任意区域") == true)
                {
                    IMap myMap = (IMap)myMapControl.ActiveView.FocusMap;
                    IActiveView act = myMapControl.ActiveView.FocusMap as IActiveView;

                    IGeometry oShape = myMapControl.TrackPolygon();
                    if (oShape == null)
                    {
                        MessageBox.Show("区域绘画错误!");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    object oo = Type.Missing;
                    myMapControl.FlashShape(oShape, 5, 300, oo);

                    string sRetName = "自绘画区域输出", sRetCode = "自绘画区域代码";
                    //... ...

                    double dMJL = m_dMMJL * m_dScale / 1000.0;		//M
                    IGeometry bufferPolygon = oShape as IGeometry;
                    if (dMJL > 0.0)
                        bufferPolygon = ((ITopologicalOperator)oShape).Buffer(dMJL);

                    IEnvelope env = ((IGeometry)bufferPolygon).Envelope;
                    IPoint LDP = new PointClass();
                    LDP.PutCoords(env.XMin, env.YMin);
                    IPoint RDP = new PointClass();
                    RDP.PutCoords(env.XMax, env.YMin);
                    IPoint RUP = new PointClass();
                    RUP.PutCoords(env.XMax, env.YMax);
                    IPoint LUP = new PointClass();
                    LUP.PutCoords(env.XMin, env.YMax);

                    myMap = myMapControl.ActiveView.FocusMap;
                    double dJ1 = 0.0, dW1 = 0.0;
                    string sRetErrorInfo = "";
                    bool bRet = syc_XY2JWD(myMap, LDP, out dJ1, out dW1, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    double dJ2 = 0.0, dW2 = 0.0;
                    sRetErrorInfo = "";
                    bRet = syc_XY2JWD(myMap, RDP, out dJ2, out dW2, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    double dJ3 = 0.0, dW3 = 0.0;
                    sRetErrorInfo = "";
                    bRet = syc_XY2JWD(myMap, RUP, out dJ3, out dW3, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    double dJ4 = 0.0, dW4 = 0.0;
                    sRetErrorInfo = "";
                    bRet = syc_XY2JWD(myMap, LUP, out dJ4, out dW4, out sRetErrorInfo);
                    if (bRet == false)
                    {
                        MessageBox.Show(sRetErrorInfo);
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
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

                    m_UseForm.m_dJ1 = dMinJ;
                    m_UseForm.m_dW1 = dMinW;
                    m_UseForm.m_dJ3 = dMaxJ;
                    m_UseForm.m_dW3 = dMaxW;
                    m_UseForm.m_sName = sRetName;
                    m_UseForm.m_sCode = sRetCode;
                    m_UseForm.m_MyPolygon = (IGeometry)oShape;
                    m_UseForm.m_bufferPolygon = (IGeometry)bufferPolygon;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;
                }
                CommonClass.Dispose();
            }
			base.OnMouseDown (Button, Shift, X, Y);
		}
		public override int Cursor
		{
			get
			{
				// TODO:  添加 LSOutTool.Cursor getter 实现
				return System.Windows.Forms.Cursors.Cross.Handle.ToInt32();
			}
		}
        public bool syc_XY2JWD(IMap myMap, IPoint pp, out double dJ, out double dW, out string sRetErrorInfo)
        {
            dJ = dW = 0.0;
            sRetErrorInfo = "";
            IProjectedCoordinateSystem system = myMap.SpatialReference as IProjectedCoordinateSystem;
            IGeographicCoordinateSystem geographicCoordinateSystem = system.GeographicCoordinateSystem;
            if ((geographicCoordinateSystem == null) || (system == null))
            {
                sRetErrorInfo = "坐标系统有问题，请检查[ErrorCode:01]!";
                return false;
            }
            IPoint point = new PointClass();
            point.PutCoords(pp.X, pp.Y);
            IGeometry geometry = point;
            geometry.SpatialReference = system;
            geometry.Project(geographicCoordinateSystem);
            double dHD = (point.X * 3.141592654) / 180.0;
            double num2 = (point.Y * 3.141592654) / 180.0;
            dJ = DLIB.HD2DFM(dHD);
            dW = DLIB.HD2DFM(num2);
            if ((((dJ <= 72.0) || (dJ >= 133.0)) || (dW <= 3.0)) || (dW >= 52.0))
            {
                sRetErrorInfo = "坐标系统有问题，请检查[ErrorCode:02]!";
                return false;
            }
            return true;
        }
		public override bool Enabled
		{
			get
			{
				//Set the enabled property
				if (m_HookHelper.ActiveView != null) 
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		//... ...
	}
}
