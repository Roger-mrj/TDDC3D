using System;
using System.Drawing;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using RCIS.Global;
using RCIS.GISCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using RCIS.Utility;
using RCIS.GISCommon;
using System.Collections;
using ESRI.ArcGIS.esriSystem;


namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("41a85502-2662-4a3c-bdfb-248f96441782")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.Xzdw2DltbCommand")]
    public sealed class Xzdw2DltbCommand : BaseCommand
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
        public Xzdw2DltbCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "��״����תͼ��";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "��״����תͼ��";  //localizable text
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
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
        
        IFeatureClass dltb_TClass = null;
        IFeatureClass dltbClass = null;
        IFeatureClass xzqClass = null;

        //��ȡ�ཻ������ж����,С��ָ�������
        

        private List<IFeature> getTouchedFeatures(IFeature inFeature, IFeatureClass aClass)
        {
            List<IFeature> retFeatures = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = inFeature.ShapeCopy;
            ITopologicalOperator pTop = inFeature.ShapeCopy as ITopologicalOperator;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
            IFeatureClass featureClass = aClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {

                    if (aFea != inFeature)
                    {
                        retFeatures.Add(aFea);
                    }
                    aFea = featureCursor.NextFeature();

                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                ////��������  
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();  
            }
            return retFeatures;
        }

        private List<IFeature> getInter1Features(IFeature inFeature, IFeatureClass aClass, string where)
        {
            List<IFeature> retFeatures = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = inFeature.ShapeCopy;
            spatialFilter.WhereClause = where;

            ITopologicalOperator pTop = inFeature.ShapeCopy as ITopologicalOperator;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {
                    if (aFea != inFeature)
                    {
                        IGeometry interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        if (!interGeo.IsEmpty)
                        {
                            retFeatures.Add(aFea);
                        }
                        else
                        {
                            interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                            {
                                if (!interGeo.IsEmpty)
                                {
                                    retFeatures.Add(aFea);
                                }
                            }
                        }
                    }
                    aFea = featureCursor.NextFeature();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                ////��������  
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
            return retFeatures;
        }

        //�ཻ��һ����ģ�
        private List<IFeature> GetInterPolygonFeatures(IFeature inFeature, IFeatureClass aClass)
        {
            List<IFeature> retFeatures = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry =inFeature.ShapeCopy ;
            ITopologicalOperator pTop = inFeature.ShapeCopy as ITopologicalOperator;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;
            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {

                    if (aFea != inFeature)
                    {
                        IGeometry interGeo = pTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        if (!interGeo.IsEmpty)
                        {
                            retFeatures.Add(aFea);
                            
                        }

                       
                    }
                    aFea = featureCursor.NextFeature();
                    
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(spatialFilter);
                ////��������  
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();  

            }
            return retFeatures;
        }

        /// <summary>
        /// ������Ҫ��ͼ���иA��ȥ��B�ཻ�Ĳ���,����������ཻ���򷵻�A
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private IGeometry GeometryDiffence(IFeature A, IFeature B)
        {
            ITopologicalOperator pTopInterDltb = A.ShapeCopy as ITopologicalOperator;
            IGeometry interGeo = pTopInterDltb.Intersect(B.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
            if (!interGeo.IsEmpty)
            {
                IGeometry oldDltbGeo = pTopInterDltb.Difference(interGeo);
                
                return oldDltbGeo;
            }
            else
            {
                return A.ShapeCopy;
            }
            
        }

        /// <summary>
        /// ��ȡ���߽��������յ㽻�� һ����ĵ���ͼ��
        /// </summary>
        /// <param name="aLine"></param>
        /// <returns></returns>
        private List<IFeature> getInterDltb(IPolyline aLine,IPoint fromPt)
        {
            List<IFeature> result = new List<IFeature>();
            List<IFeature> tmpDltbs = GetFeaturesHelper.getFeaturesByGeo(dltbClass, fromPt, esriSpatialRelEnum.esriSpatialRelIntersects);
            foreach (IFeature aFea in tmpDltbs)
            {
                ITopologicalOperator ptop = aFea.Shape as ITopologicalOperator;
                IGeometry geo = ptop.Intersect(aLine, esriGeometryDimension.esriGeometry0Dimension);
                if (geo.IsEmpty)
                    continue;
                result.Add(aFea);
            }
            return result;
                    
        }


        public IPolygon FlatBufferOneway(IPolyline inLine, double bufferDis, int zf, bool isExtent,double dExtentLen,IFeatureClass dltbClass)
        {
            object o = System.Type.Missing;
            //�ֱ���������ƽ��
            IConstructCurve newCurve = new PolylineClass();
            newCurve.ConstructOffset(inLine, zf * bufferDis, ref o, ref o);
            IPolyline addline = newCurve as IPolyline;
            addline.ReverseOrientation();  //���� ��ת

            IPolygon myPolygon =GeometryHelper.ConstructPolygonByLine(addline, inLine);
            if (isExtent)
            {
                #region ��������
                ITopologicalOperator pTopNewPolygon = myPolygon as ITopologicalOperator;
                //���߿������ͼ�ߵ��ӣ��ж��Ƿ��н���һ�����
                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = myPolygon as IGeometry;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pCursor = dltbClass.Search(pSF as IQueryFilter, false);
                IFeature aDltb = null;
                while ((aDltb = pCursor.NextFeature()) != null)
                {
                    IGeometry intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry2Dimension);
                    if (!intersecPoint.IsEmpty)
                        continue;
                    intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry1Dimension);
                    if (!intersecPoint.IsEmpty)
                        continue;

                    intersecPoint = pTopNewPolygon.Intersect(aDltb.Shape, esriGeometryDimension.esriGeometry0Dimension);
                    if (!intersecPoint.IsEmpty)
                    {
                        if ((intersecPoint as IPointCollection).PointCount != 1)
                            continue;
                        IPoint aPt = (intersecPoint as IPointCollection).get_Point(0);

                        ITopologicalOperator pTopDltb = aDltb.Shape as ITopologicalOperator;
                        IGeometry boundry = pTopDltb.Boundary;
                        IConstructCurve constructCurve = new PolylineClass();
                        bool isExtensionPerfomed = false;
                        

                        //�������һ���㣬
                        IPolyline extendLine = null;
                        //�ж������ �� �����������ߵ���һ����
                        if (GeometryHelper.isPointOnLine(aPt, inLine))
                        {
                            extendLine = addline;
                            //����                       
                            constructCurve.ConstructExtended(addline, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
                            if (!(constructCurve as IPolyline).IsEmpty)
                            {
                                extendLine = constructCurve as IPolyline;
                            }
                            double lenDiff=extendLine.Length - addline.Length;
                            if ( lenDiff< dExtentLen  &&  lenDiff>0 )
                            {
                                addline = extendLine;
                                //myPolygon = GeometryHelper.ConstructPolygonByLine(extendLine, inLine);
                            }

                        }
                        else if (GeometryHelper.isPointOnLine(aPt, addline))
                        {
                            extendLine = inLine;
                            //����                        
                            constructCurve.ConstructExtended(inLine, boundry as ICurve, (int)esriCurveExtension.esriDefaultCurveExtension, ref isExtensionPerfomed);
                            if (!(constructCurve as IPolyline).IsEmpty)
                            {
                                extendLine = constructCurve as IPolyline;
                            }
                            double lenDiff = extendLine.Length - addline.Length;
                            if (lenDiff < dExtentLen && lenDiff>0)
                            {
                                inLine = extendLine;
                                //myPolygon = GeometryHelper.ConstructPolygonByLine(addline, extendLine);
                            }
                        }



                    }
                }
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(pCursor);
                #endregion 

            }
            myPolygon = GeometryHelper.ConstructPolygonByLine(addline, inLine);
            return myPolygon;
        }


        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add Xzdw2DltbCommand.OnClick implementation
            IMap aMap = m_hookHelper.ActiveView.FocusMap;

            IFeatureLayer DltbLayer = LayerHelper.QueryLayerByModelName(aMap, "DLTB") as IFeatureLayer;
            if (DltbLayer != null)
            {
                this.dltbClass = DltbLayer.FeatureClass;
            }

            IGeoFeatureLayer xzdwLayer = LayerHelper.QueryLayerByModelName(aMap, "XZDW");
            ArrayList lstXzdw = LayerHelper.GetSelectedFeature(aMap, xzdwLayer as IGeoFeatureLayer, esriGeometryType.esriGeometryPolyline);
            if (lstXzdw.Count < 1)
            {
                MessageBox.Show("��ǰ��ͼ����ѡ��һ��������״����!");
                return;
            }
            
            Xzdw2WhereOptForm frm = new Xzdw2WhereOptForm();
            frm.currMap = aMap;
            frm.Text += "  ��ѡ����״����" + lstXzdw.Count + "����";
            if (frm.ShowDialog() == DialogResult.Cancel)
                return;
            string dltbClassName = frm.RetClassName;
            bool isDelXzdw = frm.IsDelXzdw;  //ɾ��ԭ������װ����

            bool isDotopInter = frm.IsDoTopInter;//����ͼ��ѹ��

        
            bool isExtend=frm.IsExtentSnap;  //�Զ���������
            double dExtentLen=frm.dExtentLen; //������������󳤶�
                     
            bool isUnion = frm.IsUnion;          

            IFeatureWorkspace pFeaWs=RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
            try{
                dltb_TClass = pFeaWs.OpenFeatureClass(dltbClassName);               
            }
            catch{}
            if (dltb_TClass == null)
            {
                MessageBox.Show("�Ҳ�����Ӧ�ĵ���ͼ�߲�!");
                return;
            }
                
           
            DateTime dtStart = DateTime.Now;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("����ת��", "���ڴ������Ե�...");
            wait.Show();
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();

            string logFile = Application.StartupPath + @"\output\log" + DateTime.Now.ToString("yyyyMMddHHmmss")+".txt";
            RCIS.Utility.TxtLogEntity log = new TxtLogEntity(logFile);
            try
            {
                int iCount = 0;
                int emptyCount = 0;
                foreach (IFeature aXzdw in lstXzdw)
                {
                    IGeometry aLine = aXzdw.ShapeCopy;
                    if (aLine.IsEmpty)
                    {
                        log.log(aXzdw.OID + "Ҫ��ͼ��Ϊ�գ�\r\n");
                        continue;
                    }
                    double kd=FeatureHelper.GetFeatureDoubleValue(aXzdw,"KD");
                    if (kd <= 0)
                    {
                        log.log(aXzdw.OID + "Ҫ����״��������0��\r\n");
                        continue;
                    }

                   

                    IPolygon aTbPolygon = null;
                    IPolygon aTbPolygon1 = null; //���
                    IPolygon aTbPolygon2 = null;
                    try
                    {

                        aTbPolygon1 = this.FlatBufferOneway(aLine as IPolyline, kd / 2, 1, isExtend, dExtentLen, dltbClass);
                        aTbPolygon2 = this.FlatBufferOneway(aLine as IPolyline, kd / 2, -1, isExtend, dExtentLen, dltbClass);

                        
                        //�������������潻��һ��ĵ���ͼ��                        
                        //�ϲ�
                        object missing = Type.Missing;
                        IGeometryBag pGeometryBag = new GeometryBag() as IGeometryBag;
                        pGeometryBag.SpatialReference = aLine.SpatialReference; 
                        IGeometryCollection pGeometryCollection = pGeometryBag as IGeometryCollection;
                        pGeometryCollection.AddGeometry(aTbPolygon1 as IGeometry, ref missing, ref missing);
                        pGeometryCollection.AddGeometry(aTbPolygon2 as IGeometry, ref missing, ref missing);

                        aTbPolygon= new PolygonClass();
                        ITopologicalOperator pTopologicalOperator =aTbPolygon  as ITopologicalOperator; 
                        pTopologicalOperator.ConstructUnion(pGeometryCollection as IEnumGeometry);

                      
                        


                    }
                    catch (Exception ex)
                    {
                        log.log(aXzdw.OID + "Ҫ�����ɻ���ʧ�ܣ�"+ex.Message+" \r\n");
                    }
                    if (aTbPolygon.IsEmpty)
                    {
                        log.log(aXzdw.OID + "Ҫ�����ɻ����ͼ��Ϊ�գ�" +  " \r\n");
                    }
                    #region ����Ҫ�أ���ֵ����
                    IFeature aNewDltb = dltb_TClass.CreateFeature();
                    aNewDltb.Shape = aTbPolygon as IGeometry;                   
                    string newDlbm=FeatureHelper.GetFeatureStringValue(aXzdw,"DLBM");
                    string newQsdwdm=FeatureHelper.GetFeatureStringValue(aXzdw, "QSDWDM1");
                    string newQsxz=FeatureHelper.GetFeatureStringValue(aXzdw, "QSXZ");

                    FeatureHelper.SetFeatureValue(aNewDltb, "YSDM", "2001010100");
                    FeatureHelper.SetFeatureValue(aNewDltb,"DLBM",newDlbm);
                    FeatureHelper.SetFeatureValue(aNewDltb, "DLMC", FeatureHelper.GetFeatureStringValue(aXzdw, "DLMC"));
                    //Ȩ����λ���� �����䵥λ���� ��ʱ ��ֵ�����滹��Ҫ�Ķ�
                    FeatureHelper.SetFeatureValue(aNewDltb, "QSDWDM",newQsdwdm );
                    FeatureHelper.SetFeatureValue(aNewDltb, "QSDWMC", FeatureHelper.GetFeatureStringValue(aXzdw, "QSDWMC1"));
                    FeatureHelper.SetFeatureValue(aNewDltb, "ZLDWDM", FeatureHelper.GetFeatureStringValue(aXzdw, "ZLDWDM1"));
                    FeatureHelper.SetFeatureValue(aNewDltb, "QSXZ",newQsxz );


                    FeatureHelper.SetFeatureValue(aNewDltb, "XZDWKD", kd);  //��ֵ���
                    aNewDltb.Store();
                    #endregion 

                    bool isNewGeoEmpty = false;
                    #region //Ԥ������ͼ�� ����
                    //if (isDelSmallTb)
                    //{
                    //    //��ͼ�������ͼ���ཻ������ͼ��
                    //    List<IGeometry> lstSmallGeo = this.getIntersectSmallGeoByTb(dltbClass, aTbPolygon as IGeometry, dSmallTbmj);
                    //    ITopologicalOperator pTopDiff = aTbPolygon as ITopologicalOperator;
                    //    IGeometry restGeos = aTbPolygon;

                    //    foreach (IGeometry aGeo in lstSmallGeo)
                    //    {
                    //        restGeos= pTopDiff.Difference(aGeo);
                    //        if (restGeos.IsEmpty)
                    //            break;
                    //        pTopDiff = restGeos as ITopologicalOperator;
                    //    }

                    //    if (!restGeos.IsEmpty)
                    //    {
                    //        aNewDltb.Shape = restGeos;
                    //        aNewDltb.Store();
                    //    }
                    //    else
                    //    {
                    //        aNewDltb.Delete();  //�����û�ˣ��� ������ǰ�������ͼ�� Ϊ��
                    //        emptyCount++;
                    //        isNewGeoEmpty=true;
                    //    }
                    //}

                    #endregion 


                    if (isUnion && !isNewGeoEmpty)
                    {
                        #region �ϲ�
                        //��ȡ���ڵ�
                        List<IFeature> delFeatures = new List<IFeature>();
                        //2017��11-20���޸ģ�����һ����� �����ٺϲ�
                        List<IFeature> arTouchedFeature = this.getInter1Features(aNewDltb, dltb_TClass,
                            //GetFeaturesByGeoHelper.getFeatures(dltbClass, aNewDltb.ShapeCopy, esriSpatialRelEnum.esriSpatialRelIntersects,
                            "DLBM='" + newDlbm + "' and QSXZ='" + newQsxz + "'   and XZDWKD="+kd);
                            //GetFeaturesByGeoHelper.getFeaturesByGeo(dltbClass, aNewDltb.ShapeCopy, esriSpatialRelEnum.esriSpatialRelIntersects);
                        if (arTouchedFeature.Count > 0)
                        {
                            
                            IGeometry newGeo = aNewDltb.ShapeCopy;
                            ITopologicalOperator pTop = aNewDltb.ShapeCopy as ITopologicalOperator;                            
                            foreach (IFeature aTouched in arTouchedFeature)
                            {
                                if (aTouched.OID == aNewDltb.OID)
                                    continue;
                                
                                newGeo = pTop.Union(aTouched.ShapeCopy);
                                pTop.Simplify();                                
                                pTop = newGeo as ITopologicalOperator;
                                delFeatures.Add(aTouched);
                            }
                            aNewDltb.Shape = newGeo;
                            aNewDltb.Store();
                        }
                        foreach (IFeature adel in delFeatures)
                        {
                            adel.Delete();
                            
                        }
                        #endregion 

                    }



                    if (isDotopInter && !isNewGeoEmpty)
                    {
                        #region //�˴�����ͼ��ѹ��
                        List<IFeature> lstIntersecPolygonFeatures = this.GetInterPolygonFeatures(aNewDltb, dltb_TClass);  //����һ�����
                        //�����ˣ��Ͳ�Ҫ��
                        
                        foreach (IFeature aInterDltb in lstIntersecPolygonFeatures)
                        {
                            if (aInterDltb.OID == aNewDltb.OID)
                                continue;
                            //�жϵ��࣬�������·����·����ũ���·�ཻ��ũ���·��ȥ
                            //����������ٰ���ȣ���·��ȥխ·��
                            //Ȼ���Ⱥ�����
                            string oldDlbm=FeatureHelper.GetFeatureStringValue(aInterDltb,"DLBM");

                            double oldKd = FeatureHelper.GetFeatureDoubleValue(aInterDltb, "XZDWKD"); //ѹ��ͼ�߿��

                            if (sys.YWCommonHelper.priorityDLBM2(newDlbm,oldDlbm))
                            {
                                #region  //�����Ҫ�� ���������� �ɵģ���� �ɵ�ɾȥ�ཻ����
                                try
                                {
                                    IGeometry oldGeo = this.GeometryDiffence(aInterDltb, aNewDltb);
                                    if (oldGeo.IsEmpty)
                                    {
                                        aInterDltb.Delete();
                                        emptyCount++;
                                    }
                                    else
                                    {
                                        aInterDltb.Shape = oldGeo;
                                        aInterDltb.Store();
                                    }
                                   
                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion 

                            }
                            else if (kd>oldKd)
                            {
                                #region ��ȴ�Ĳ��п��С��
                                try
                                {
                                    IGeometry oldGeo = this.GeometryDiffence(aInterDltb, aNewDltb);
                                    if (oldGeo.IsEmpty)
                                    {
                                        aInterDltb.Delete();
                                        emptyCount++;
                                    }
                                    else
                                    {
                                        aInterDltb.Shape = oldGeo;
                                        aInterDltb.Store();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                                #endregion 
                            }
                            else 
                            {
                                //���� ����ͼ�� ��ȥ �ཻ����
                                IGeometry newDltbGeo = this.GeometryDiffence(aNewDltb, aInterDltb);
                                //�˴���Ҫ�ж�һ���Ƿ�Ϊ�գ���Ϊ�и�����֮��ͼ�ξͱ���
                                if (newDltbGeo.IsEmpty)
                                {
                                    isNewGeoEmpty = true; //��ʾ��ͼ�� �г�û��
                                    break;
                                }
                                aNewDltb.Shape = newDltbGeo;
                                aNewDltb.Store();                              

                            }                      
                            
                        }
                        if (isNewGeoEmpty)
                        {
                            //����ǿյģ�ɾ��
                            aNewDltb.Delete();                            
                            emptyCount++;
                        }

                        #region ����
                        else
                        {
                            
                            // if (isCalTbmj)
                            //{
                            //    #region ��� //�����Ҫ����
                            //    IPoint selectPoint = (aNewDltb.ShapeCopy as IArea).Centroid;
                            //    double X = selectPoint.X;
                            //    int currDh = (int)(X / 1000000);////WK---����
                            //   // LSSphereArea.GGP.ClsSphereArea area = new LSSphereArea.GGP.ClsSphereArea();

                            //    SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();

                            //    double tbmj = area.SphereArea(aNewDltb.ShapeCopy, currDh);
                            //    FeatureHelper.SetFeatureValue(aNewDltb, "TBMJ", tbmj);
                            //    FeatureHelper.SetFeatureValue(aNewDltb, "TBDLMJ", tbmj);
                            //    aNewDltb.Store();
                            //    #endregion 
                            //}  



                        }
                        #endregion

                        #endregion
                    }
                    
                    iCount++;
                    if (iCount % 50 == 0)
                    {
                        wait.SetCaption("�Ѵ���" + iCount + "��...");                       
                    }
                }                  

                if (isDelXzdw)
                {
                    wait.SetCaption("��ʼɾ����״����...");
                    foreach (IFeature aXzdw in lstXzdw)
                    {
                        aXzdw.Delete();
                    }
                }

                IActiveView activeView = aMap as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography | esriViewDrawPhase.esriViewGeoSelection, null, activeView.Extent);
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("XZDW2DLTB");

                wait.Close();
                DateTime dtEnd = DateTime.Now;
                TimeSpan ts = dtEnd.Subtract(dtStart);

                MessageBox.Show("ת����ϣ�\r\n"+ ts.Hours+"Сʱ"+ts.Minutes+"���ӡ�\r\n��ɾ��"+emptyCount+"����ͼ��!" , "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (log.ErrCount  > 0)
                {
                    System.Diagnostics.Process.Start(logFile);
                }
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
    }
}
