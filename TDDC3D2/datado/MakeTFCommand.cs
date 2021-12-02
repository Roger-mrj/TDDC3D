using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.Collections.Generic;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
namespace TDDC3D.datado
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("571ce1df-8db5-4a29-bc10-4b9c069fec6f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.datado.MakeTFCommand")]
    public sealed class MakeTFCommand : BaseCommand
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
        private IEngineEditor m_engineEditor;

        public MakeTFCommand(IEngineEditor editor)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "ͼ������";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "ͼ������";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                this.m_engineEditor = editor;
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }
        private ILayer tfhLayer = null;
  
        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
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

        //"ת��Ϊ��
        public double trans2Sec(double arc)
        {
            int degree = (int)arc;
            double aSmall = (arc - degree) * 60;
            int minute = (int)aSmall;

            aSmall = (aSmall - minute) * 60;
            aSmall = Math.Round(aSmall, 6);

            return degree * 3600 + minute * 60 + aSmall;

        }

        public double trans2Dec(double sec)
        {
            return sec / 3600;
        }

        List<IPoint> getNewInterPt(IPoint p1, IPoint p2)
        {
            List<IPoint> lst = new List<IPoint>();
            //�����ά�� ����
            if ((p2.Y > p1.Y) && (p2.X==p1.X))
            {
                //��Y�ϲ�ֵ
                IPoint startPt = p1;
                while (startPt.Y < p2.Y)
                {
                    //Y����Ϊ��
                    double newYt = trans2Sec(startPt.Y);
                    double newY = (int)newYt + 1;
                    if (trans2Dec(newY) >= p2.Y)
                        break;
                    IPoint newPt = new PointClass();
                    newPt.X = p1.X;
                    newPt.Y = trans2Dec(newY);
                    lst.Add(newPt);
                    startPt = newPt;
                }

            }
            else if ((p2.Y < p1.Y) && (p2.X == p1.X))
            {
                //�½�
                //��Y�ϲ�ֵ
                IPoint startPt = p2;
                while (startPt.Y < p1.Y)
                {
                    //Y����Ϊ��
                    double newYt = trans2Sec(startPt.Y);
                    double newY = (int)newYt + 1;
                    if (trans2Dec(newY) >= p1.Y)
                        break;
                    IPoint newPt = new PointClass();
                    newPt.X = p2.X;
                    newPt.Y = trans2Dec(newY);
                    lst.Add(newPt);
                    startPt = newPt;
                }
            }
            else if ((p2.Y == p1.Y) && (p2.X > p1.X))
            {
                IPoint startPt = p1;
                while (startPt.X < p2.X)
                {
                    
                    double newYt = trans2Sec(startPt.X);
                    double newX = (int)newYt + 1;
                    if (trans2Dec(newX) >= p2.X)
                        break;
                    IPoint newPt = new PointClass();
                    newPt.X = trans2Dec(newX);
                    newPt.Y = p1.Y;
                    lst.Add(newPt);
                    startPt = newPt;
                }
            }
            else if ((p2.Y == p1.Y) && (p2.X < p1.X))
            {
                IPoint startPt = p2;
                while (startPt.X < p1.X)
                {
                    //Y����Ϊ��
                    double newXt = trans2Sec(startPt.X);
                    double newX = (int)newXt + 1;
                    if (trans2Dec(newX) >= p1.X)
                        break;
                    IPoint newPt = new PointClass();
                    newPt.X = trans2Dec(newX);
                    newPt.Y = p1.Y;
                    lst.Add(newPt);
                    startPt = newPt;
                }
            }
            return lst;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add MakeTFCommand.OnClick implementation
            tfhLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(m_hookHelper.FocusMap, "TFH");
            //2018��9��3�գ������޸ģ����ͼ�������жϣ�����������������ݷ�Χ
            ILayer xzqLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(m_hookHelper.FocusMap, "XZQ");
            
            if (null == tfhLayer)
            {
                MessageBox.Show("û��ͼ����ͼ��!");
                return;
            }
            if (xzqLayer == null)
            {
                MessageBox.Show("û��������ͼ��!");
                return;
            }
            IFeatureClass xzqClass = (xzqLayer as IFeatureLayer).FeatureClass;
            if (xzqClass.FeatureCount(null) == 0)
            {
                MessageBox.Show("������ͼ��û������!");
                return;
            }

            IEnvelope pEnv = xzqLayer.AreaOfInterest;
            IPoint leftDown = new PointClass(); 
            leftDown.X = pEnv.LowerLeft.X - 300;
            leftDown.Y = pEnv.LowerLeft.Y - 1000;
            IPoint rightUp = new PointClass();
            rightUp.X = pEnv.UpperRight.X + 300;
            rightUp.Y = pEnv.UpperRight.Y + 1700;

            IPoint jwdLeftDown = RCIS.GISCommon.CoordinateTransHelper.XY2JWD(this.m_hookHelper.ActiveView.FocusMap, leftDown);
            IPoint jwdRightUp = RCIS.GISCommon.CoordinateTransHelper.XY2JWD(this.m_hookHelper.ActiveView.FocusMap, rightUp);

            TFHInfoForm tfhFrm = new TFHInfoForm();
            tfhFrm.TFLeft = jwdLeftDown.X;
            tfhFrm.TFTop = jwdRightUp.Y;
            tfhFrm.TFBottom = jwdLeftDown.Y;
            tfhFrm.TFRight = jwdRightUp.X;          

            if (DialogResult.OK == tfhFrm.ShowDialog())
            {
                
                DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("��ʼ����...", "��������ͼ��...");
                wait.Show();
                this.m_engineEditor.StartOperation();
                try
                {
                    TFHFactory2 tfFactory = new TFHFactory2();
                    TFHScale mTfhscale;
                    #region  //     //1:2000   //1:5000  //1:10000
                    //1:25000
                    switch (tfhFrm.TFType)
                    {
                        case 0: //1:1000 
                            mTfhscale=TFHScale.J;
                            break;
                        case 1:
                            mTfhscale = TFHScale.I;
                            break;
                        case 2:
                            mTfhscale = TFHScale.H;
                            break;
                        case 3:
                            mTfhscale = TFHScale.G;
                            break;
                        case 4:
                            mTfhscale = TFHScale.F;
                            break;
                        case 5:
                            mTfhscale = TFHScale.E; //5��
                            break;
                        default:
                            mTfhscale = TFHScale.A; //100��
                            break;
                    }

                    #endregion 

                    tfFactory.FromExtent2TFH(tfhFrm.TFLeft, tfhFrm.TFBottom, tfhFrm.TFRight, tfhFrm.TFTop, mTfhscale);

                    IList<TFHObject>  lst = tfFactory.TFHList;  //��ȡ��������Ϣ
                    wait.SetCaption("������õ�ͼ��" + lst.Count + "��...");

                    #region  //ɾ���ɵ�
                    IFeatureClass m_pFeatureClass = (tfhLayer as IFeatureLayer).FeatureClass;
                    ITable pTable = m_pFeatureClass as ITable;
                    pTable.DeleteSearchedRows(null);
                    #endregion

                    int m_tfhIndex = m_pFeatureClass.Fields.FindField("TFH");
                    int m_oldTFHIndex = m_pFeatureClass.Fields.FindField("OLDTFH");
                    //���Ҫ��
                    IFeatureBuffer m_buffer = m_pFeatureClass.CreateFeatureBuffer();
                    IFeatureCursor m_cursor = m_pFeatureClass.Insert(true);
                    int icount=1;
                    foreach (TFHObject tfhObj in lst)
                    {
                        #region ����һ��
                        IPolygon poly = new PolygonClass();
                        IPointCollection ptCol = poly as IPointCollection;
                        IList<TFHPoint> ptList = tfhObj.ComputePtList();
                        object missing = Type.Missing;
                        for (int j = 0; j < ptList.Count; j++)
                        {
                            IPoint point = new PointClass();
                            point.X = ptList[j].m_x;
                            point.Y = ptList[j].m_y;
                            ptCol.AddPoint(point, ref missing, ref missing); //�����һ����
                            if (tfhFrm.IsInterpolate)
                            {
                                //��ֵ
                                if (j < ptList.Count-1)
                                {
                                    IPoint p2 = new PointClass();
                                    p2.X = ptList[j + 1].m_x;
                                    p2.Y = ptList[j + 1].m_y;

                                    List<IPoint> interPts = this.getNewInterPt(point, p2);
                                    foreach (IPoint aPt in interPts)
                                    {
                                        ptCol.AddPoint(aPt);
                                    }
                                }
                            }
                        }
                        //poly.SimplifyPreserveFromTo();
                        //poly.Project(this.m_hookHelper.ActiveView.FocusMap.SpatialReference);

                        //2018��9��3�գ������޸ģ����ͼ�������жϣ���ͼ��ͶӰ�ɵ�ǰ��ͼͶӰ���ж����������ཻ��������ͼ��
                        IGeometry pGeo = RCIS.GISCommon.CoordinateTransHelper.JWD2XY(m_hookHelper.ActiveView.FocusMap, poly);
                        ISpatialFilter spatialfilter = new SpatialFilter();
                        spatialfilter.Geometry = pGeo;
                        spatialfilter.GeometryField = xzqClass.ShapeFieldName;
                        spatialfilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IFeatureCursor pFC = xzqClass.Search(spatialfilter, false);
                        IFeature pF = pFC.NextFeature();
                        if (pF != null)
                        {
                            m_buffer.Shape = poly;
                            m_buffer.set_Value(m_tfhIndex, tfhObj.NewTFH);
                            m_buffer.set_Value(m_oldTFHIndex, tfhObj.OldTFH);

                            m_cursor.InsertFeature(m_buffer);
                            wait.SetCaption("������" + icount++ + "��...");
                        }
                        

                        #endregion 
                        
                        if (icount % 50 == 0)
                        {
                            Application.DoEvents();
                        }
                       
                    }
                    m_cursor.Flush();

                    this.m_engineEditor.StopOperation("tfhcal");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_cursor);
                    wait.Close();
                    MessageBox.Show("������ϣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.m_hookHelper.ActiveView.Refresh();

                }
                catch (Exception ex)
                {
                    if (wait != null)
                        wait.Close();
                    this.m_engineEditor.AbortOperation();
                    MessageBox.Show(ex.Message);
                }

            }
        }

        #endregion
    }
}
