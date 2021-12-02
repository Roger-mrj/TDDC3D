using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System.Text;
using RCIS.GISCommon;
using RCIS.Utility;

using sycCommonLib;
namespace TDDC3D.output
{
    /// <summary>
    /// Summary description for LSOutTool2.
    /// </summary>
    [Guid("39332a5a-6ccf-4c04-b00f-950b7dcfa220")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.output.LSOutTool2")]
    public sealed class LSOutTool2 : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;


        //����
        public string m_sFW;  //��Χ

        /// <summary>
        /// ͼ�ξ����ڿ����
        /// </summary>
        public double m_dMMJL; 
        public string m_sScale;  //������
        public LSOutForm2 m_UseForm;

        /// <summary>
        /// ��ȡһ����Ҫ��
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        private IFeature GetACunFea(IPoint pt, IFeatureLayer needLayer)
        {
                      
            IIdentify pIdentify = needLayer as IIdentify;
            IArray pIDs = pIdentify.Identify((IGeometry)pt);
            if (pIDs == null)
            {
                return null;
            }
            if (pIDs.Count==0) return null;

            IFeatureIdentifyObj pFeatIdObj = pIDs.get_Element(0) as IFeatureIdentifyObj;
            IRowIdentifyObject pRowObj = pFeatIdObj as IRowIdentifyObject;
            IFeature pFeat = pRowObj.Row as IFeature;
            return pFeat;
        }

        private IFeature GetXZQFea(string sName, IPoint pp)
        {
            string sNeedFeatureClassName = "XZQ";
            IFeatureLayer needLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(this.m_hookHelper.FocusMap, sNeedFeatureClassName);
            IIdentify pIdentify = needLayer as IIdentify;
            IArray pIDs = pIdentify.Identify((IGeometry)pp);
            if (pIDs == null)
            {
                return null;
            }
                        
            for (int i = 0; i < pIDs.Count; i++)
            {
                //�����������ĵ�һ������:
                IFeatureIdentifyObj pFeatIdObj = pIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRowObj = pFeatIdObj as IRowIdentifyObject;
                IFeature pFeat = pRowObj.Row as IFeature;
                return pFeat;
            }
            return null;
        }

        public override int Cursor
        {
            get
            {
                // TODO:  ��� LSOutTool.Cursor getter ʵ��
                return System.Windows.Forms.Cursors.Cross.Handle.ToInt32();
            }
        }

        public LSOutTool2()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text 
            base.m_caption = "";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add LSOutTool2.OnClick implementation
        }



        public bool syc_XY2JWD(IMap myMap, IPoint pp, out double dJ, out double dW, out string sRetErrorInfo)
        {
            dJ = dW = 0.0;
            sRetErrorInfo = "";
            IProjectedCoordinateSystem system = myMap.SpatialReference as IProjectedCoordinateSystem;
            IGeographicCoordinateSystem geographicCoordinateSystem = system.GeographicCoordinateSystem;
            if ((geographicCoordinateSystem == null) || (system == null))
            {
                sRetErrorInfo = "����ϵͳ�����⣬����[ErrorCode:01]!";
                return false;
            }
            IPoint point = new PointClass();
            point.PutCoords(pp.X, pp.Y);
            IGeometry geometry = point;
            geometry.SpatialReference = system;
            geometry.Project(geographicCoordinateSystem);
            double dHD = (point.X * 3.141592654) / 180.0;
            double num2 = (point.Y * 3.141592654) / 180.0;
            dJ = DLIB.HD2DFM(dHD);  //����ת�ȷ���    

            

            dW = DLIB.HD2DFM(num2);




            if ((((dJ <= 72.0) || (dJ >= 133.0)) || (dW <= 3.0)) || (dW >= 52.0))
            {
                sRetErrorInfo = "����ϵͳ�����⣬����[ErrorCode:02]!";
                return false;
            }
            return true;
        }

        

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LSOutTool2.OnMouseDown implementation
            if (Button == 1)
            {

                IMapControl3 myMapControl = this.m_hookHelper.Hook as IMapControl3;
                sycCommonFuns CommonClass = new sycCommonLib.sycCommonFuns();
                double m_dScale = Convert.ToDouble(m_sScale);

                IMap myMap = this.m_hookHelper.ActiveView.FocusMap;

                if (m_sFW.Equals("��ͼ"))
                {
                    IFeatureLayer needLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(myMap, "CJDCQ");
                    if (needLayer == null)
                    {
                        MessageBox.Show(" �ڵ�ǰ���ص������С�û������Ҫ��������[CJDCQ]! ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    IPoint pp = this.m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    pp.Project(myMap.SpatialReference);
                    IFeature aCun = GetACunFea(pp, needLayer);
                    if (aCun == null)
                    {
                        MessageBox.Show(" û�ҵ���Ҫ����������... ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }

                    string sRetCode = FeatureHelper.GetFeatureStringValue(aCun, "ZLDWDM");
                    string sRetName = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aCun, "ZLDWMC");
                    IGeometry  fwGeo = aCun .ShapeCopy;  //��Χ   
                    if (fwGeo == null || fwGeo.IsEmpty)
                    {
                        MessageBox.Show(" û�ҵ���Ҫ����������... ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }


                    double dMJL = m_dMMJL * m_dScale / 1000.0;		//M

                    IGeometry bufferPolygon = fwGeo;
                    if (dMJL > 0.0)
                        bufferPolygon = ((ITopologicalOperator)fwGeo).Buffer(dMJL);


                    myMapControl.FlashShape(fwGeo, 4, 300, null);
                    myMapControl.Extent = bufferPolygon.Envelope;


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

                    m_UseForm.m_dJ1 = dMinJ;
                    m_UseForm.m_dW1 = dMinW;
                    m_UseForm.m_dJ3 = dMaxJ;
                    m_UseForm.m_dW3 = dMaxW;
                    m_UseForm.m_XzqMC = sRetName;
                    m_UseForm.m_XzqDm = sRetCode;
                    m_UseForm.m_RealCtPolygon = fwGeo;
                    m_UseForm.m_bufferPolygon = (IGeometry)bufferPolygon;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;

                    

                }
                else 
                if (
                    (m_sFW.Equals("��ͼ") == true) ||
                    (m_sFW.Equals("��ͼ") == true))
                {
                    //��������һ�����ڡ�������������壬Ŀǰ����XZQDM[12]������:
                    //XX��XX   XX   XXX  XXX
                    //ʡ  �С���  ��  ��
                    //��������3λ��Ϊ"000" ==> ��;
                    //������3λΪ"000"���������ڶ���3λ����"000" ==> ��;
                   
                    string sNeedFeatureClassName = "XZQ";
                    string sNeedNameField = "XZQMC";
                    string sNeedCodeField = "XZQDM";
                    string sRetName = "", sRetCode = "";

                    #region �ҵ���Ӧ������ ��
                   
                    IFeatureLayer needLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(myMap,sNeedFeatureClassName);
                    if (needLayer == null)
                    {
                        MessageBox.Show(" �ڵ�ǰ���ص������С�û������Ҫ��������[" + sNeedFeatureClassName + "]! ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    #endregion
                    
                    IFeatureClass xzqClass = needLayer.FeatureClass;
                    IActiveView act = this.m_hookHelper.ActiveView;
                    IPoint pp = act.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                    pp.Project(act.FocusMap.SpatialReference);
                    IIdentify pIdentify = needLayer as IIdentify; //�����������
                    IArray pIDs = pIdentify.Identify((IGeometry)pp);
                    if (pIDs == null)
                    {
                        MessageBox.Show(" û�ҵ���Ҫ����������... ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    IGeometry fwGeo = null;
                    

                    for (int i = 0; i < pIDs.Count; i++)
                    {
                        #region  //�����������ĵ�һ������:
                        IFeatureIdentifyObj pFeatIdObj = pIDs.get_Element(i) as IFeatureIdentifyObj;
                        IRowIdentifyObject pRowObj = pFeatIdObj as IRowIdentifyObject;
                        IFeature pFeat = pRowObj.Row as IFeature;

                        string sCode = FeatureHelper.GetFeatureStringValue(pFeat, sNeedCodeField); //��ȡ��һ��Ҫ����������
                        #endregion 

                        //if (m_sFW.Equals("��ͼ") == true)
                        //2019-3�޸ģ�ֱ�Ӵ�XZQͼ���ȡ�ľ�����

                        if (m_sFW.Equals("��ͼ") == true)  
                        {
                            //����λ���� 0 
                            IFeature lsfea = GetXZQFea(sCode, pp);
                            sRetCode = FeatureHelper.GetFeatureStringValue(lsfea, sNeedCodeField);
                            if (sRetCode.Length > 9)
                            {
                                sRetCode = sRetCode.Substring(0, 9);
                            }
                            sRetName = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(lsfea, sNeedNameField);
                            fwGeo = lsfea.ShapeCopy;  //��Χ 
                               
                            break;
                           
                        }
                        #region ԭ��ͼ ������
                        //else if (m_sFW.Equals("��ͼ") == true)
                        //{
                        //    #region ��ͼ
                        //    ////��һ��������λ��"000"�����ڶ���������λ��Ϊ"000";                            
                        //    //IQueryFilter qur = new QueryFilterClass();
                        //    ////  qur.WhereClause = "substr(XZQDM,";
                        //    ////-------
                        //    //IFeature lsfea = GetXZQFea(sCode, pp);  //d�õ�ǰ9λ
                        //    //string sCode2 = FeatureHelper.GetFeatureStringValue(lsfea, sNeedCodeField); //ֻ�д���
                        //    //sRetCode = sCode.Substring(0, 9);
                        //    //sRetName = sys.YWCommonHelper.getXZQMC(sRetCode);
                            

                        //    //qur.WhereClause = "\"XZQDM\" like  '" + sCode2.Substring(0, 9) + "%'";
                        //    //IFeatureCursor pCur = xzqClass.Search(qur, false);
                        //    //IFeature xfea = pCur.NextFeature();
                        //    //if (xfea == null)
                        //    //    break;
                        //    //IGeometry uGeo = xfea.Shape;
                        //    //xfea = pCur.NextFeature();
                        //    //while (xfea != null)
                        //    //{
                        //    //    try
                        //    //    {
                        //    //        ITopologicalOperator top = uGeo as ITopologicalOperator;
                        //    //        uGeo = top.Union(xfea.Shape);
                        //    //    }
                        //    //    catch
                        //    //    { }
                        //    //    xfea = pCur.NextFeature();

                        //    //}
                        //    //System.Runtime.InteropServices.Marshal.ReleaseComObject(pCur);

                        //    //fwGeo = uGeo;
                        //    //(fwGeo as ITopologicalOperator).Simplify();
                        //    //fwGeo.Project(act.FocusMap.SpatialReference);
                            

                        //    //break;
                        //    #endregion 

                        //}
                        #endregion 

                        else if (m_sFW.Equals("��ͼ") == true)
                        {
                            //�������ͼ
                            IFeature lsfea = GetXZQFea(sCode, pp);
                            sRetCode = FeatureHelper.GetFeatureStringValue(lsfea, sNeedCodeField);
                            sRetCode = sRetCode.Substring(0, 6); //6λ����
                            sRetName = sys.YWCommonHelper.getXZQMC(sRetCode);
                            //ȫ�ط�Χ��ͼ��
                            fwGeo = GeometryHelper.GeometryBag(xzqClass);

                           
                            break;
                        }
                    }
                    if (fwGeo== null  || fwGeo.IsEmpty)
                    {
                        MessageBox.Show(" û�ҵ���Ҫ����������... ");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }            
                    
                    
                    double dMJL = m_dMMJL * m_dScale / 1000.0;		//M

                    IGeometry bufferPolygon = fwGeo;
                    if (dMJL > 0.0)
                        bufferPolygon = ((ITopologicalOperator)fwGeo).Buffer(dMJL);

                    
                    myMapControl.FlashShape(fwGeo, 4, 300, null);
                    myMapControl.Extent = bufferPolygon.Envelope;
                    

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

                    m_UseForm.m_dJ1 = dMinJ;
                    m_UseForm.m_dW1 = dMinW;
                    m_UseForm.m_dJ3 = dMaxJ;
                    m_UseForm.m_dW3 = dMaxW;
                    m_UseForm.m_XzqMC = sRetName;
                    m_UseForm.m_XzqDm = sRetCode;
                    m_UseForm.m_RealCtPolygon = fwGeo;
                    m_UseForm.m_bufferPolygon = (IGeometry)bufferPolygon;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;
                    //... ...
                }
                else if (m_sFW.Equals("��׼�ַ�") == true)
                {
                    #region  //��׼�ַ�:

                    //[01]:... ...
                    IActiveView act = myMapControl.ActiveView.FocusMap as IActiveView;
                    IPoint pp = act.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

                   
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
                        MessageBox.Show("�����ηַ������������������⣬�޷����������ͼ��!");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    StringBuilder sNewTFH = new StringBuilder(100);
                    nRet = DLIB.GetNewWaima(m_dScale, dJ, dW, sNewTFH);
                    //string newTfh = CoordinateTransHelper.getTFH(dJ, dW, m_dScale);

                    if (nRet == -1)
                    {
                        MessageBox.Show("�����ηַ������������������⣬�޷������ͼ����!");
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

                   
                    //myMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    myMapControl.FlashShape((IGeometry)ele.Geometry, 4, 300, null);
                    myMapControl.Extent = ele.Geometry.Envelope;

                    //���ص�������:
                    //Ҫ���ص�ֵ
                    m_UseForm.m_RealCtPolygon = (IGeometry)ele.Geometry;
                    m_UseForm.m_bufferPolygon = (IGeometry)ele.Geometry;
                    m_UseForm.m_sTFH = sBZTFH;
                    m_UseForm.m_dJ1 = dJ1;
                    m_UseForm.m_dW1 = dW1;
                    m_UseForm.m_dJ3 = dJ3;
                    m_UseForm.m_dW3 = dW3;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;

                    #endregion 

                    //... ...
                }
                else if (m_sFW.Equals("��������") == true)
                {
                    #region ��������
                    
                    IActiveView act = myMapControl.ActiveView.FocusMap as IActiveView;

                    IGeometry oShape = myMapControl.TrackPolygon();
                    if ( (oShape == null)  || (oShape.IsEmpty))
                    {
                        MessageBox.Show("����滭����!");
                        if (m_UseForm.Visible == false)
                            m_UseForm.Visible = true;
                        return;
                    }
                    oShape.Project(myMap.SpatialReference);
                    object oo = Type.Missing;
                    
                    string sRetName = "�Ի滭�������", sRetCode = "�Ի滭�������";
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
                    IPoint ldp1 = CoordinateTransHelper.XY2JWD(myMap, LDP);
                    dJ1 = ldp1.X;
                    dW1 = ldp1.Y;
                    //bool bRet = syc_XY2JWD(myMap, LDP, out dJ1, out dW1, out sRetErrorInfo);
                    //if (bRet == false)
                    //{
                    //    MessageBox.Show(sRetErrorInfo);
                    //    if (m_UseForm.Visible == false)
                    //        m_UseForm.Visible = true;
                    //    return;
                    //}
                    double dJ2 = 0.0, dW2 = 0.0;
                    IPoint rdp1 = CoordinateTransHelper.XY2JWD(myMap, RDP);
                    dJ2 = rdp1.X;
                    dW2 = rdp1.Y;
                    //sRetErrorInfo = "";
                    //bRet = syc_XY2JWD(myMap, RDP, out dJ2, out dW2, out sRetErrorInfo);
                    //if (bRet == false)
                    //{
                    //    MessageBox.Show(sRetErrorInfo);
                    //    if (m_UseForm.Visible == false)
                    //        m_UseForm.Visible = true;
                    //    return;
                    //}
                    double dJ3 = 0.0, dW3 = 0.0;
                    IPoint rup1 = CoordinateTransHelper.XY2JWD(myMap, RUP);
                    dJ3 = rup1.X;
                    dW3 = rup1.Y;
                    //sRetErrorInfo = "";
                    //bRet = syc_XY2JWD(myMap, RUP, out dJ3, out dW3, out sRetErrorInfo);
                    //if (bRet == false)
                    //{
                    //    MessageBox.Show(sRetErrorInfo);
                    //    if (m_UseForm.Visible == false)
                    //        m_UseForm.Visible = true;
                    //    return;
                    //}
                    double dJ4 = 0.0, dW4 = 0.0;
                    IPoint lup1 = CoordinateTransHelper.XY2JWD(myMap, LUP);
                    dJ4 = lup1.X;
                    dW4 = lup1.Y;
                    //sRetErrorInfo = "";
                    //bRet = syc_XY2JWD(myMap, LUP, out dJ4, out dW4, out sRetErrorInfo);
                    //if (bRet == false)
                    //{
                    //    MessageBox.Show(sRetErrorInfo);
                    //    if (m_UseForm.Visible == false)
                    //        m_UseForm.Visible = true;
                    //    return;
                    //}

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


                    myMapControl.FlashShape(oShape, 4, 300, null);
                    myMapControl.Extent = bufferPolygon.Envelope;
                    //myMapControl.ActiveView.ScreenDisplay.UpdateWindow();
                    //myMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, myMapControl.ActiveView.Extent);
                                        
                    m_UseForm.m_dJ1 = dMinJ;
                    m_UseForm.m_dW1 = dMinW;
                    m_UseForm.m_dJ3 = dMaxJ;
                    m_UseForm.m_dW3 = dMaxW;
                    m_UseForm.m_XzqMC = sRetName;
                    m_UseForm.m_XzqDm = sRetCode;
                    m_UseForm.m_RealCtPolygon = (IGeometry)oShape;
                    m_UseForm.m_bufferPolygon = (IGeometry)bufferPolygon;
                    if (m_UseForm.Visible == false)
                        m_UseForm.Visible = true;

                    #endregion

                }
                CommonClass.Dispose();
            }
            base.OnMouseDown(Button, Shift, X, Y);
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LSOutTool2.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LSOutTool2.OnMouseUp implementation
        }
        #endregion
    }
}
