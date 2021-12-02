using System;
using System.Drawing;
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


using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;


namespace TDDC3D.edit
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("68e84d6a-497e-4fed-bbcf-e5f2ad0f459a")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("TDDC3D.edit.GetJZDCommand")]
    public sealed class GetJZDCommand : BaseCommand
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
        public GetJZDCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "提取界址点";  //localizable text 
            base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            base.m_toolTip = "提取界址点";  //localizable text
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

        private string ReturnMessages(Geoprocessor gp)
        {
            string s = "";
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    s+= gp.GetMessage(Count) ;
                    //Console.WriteLine(gp.GetMessage(Count));
                }
            }
            return s;
        }

        private string  RunTool(Geoprocessor geoprocessor, IGPProcess process, ITrackCancel TC)
        {

            // Set the overwrite output option to true
            geoprocessor.OverwriteOutput = true;

            // Execute the tool            
            try
            {
                geoprocessor.Execute(process, null);
                return  ReturnMessages(geoprocessor);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return ReturnMessages(geoprocessor);
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

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add GetJZDCommand.OnClick implementation
            IMap aMap = m_hookHelper.ActiveView.FocusMap;

            GetJzdOptForm frm = new GetJzdOptForm();
            frm.currMap = aMap;
            if (frm.ShowDialog() == DialogResult.Cancel) return;

            string polygonLyrName = frm.polygonLayerName; //面要素类名称
            string pointLyrName = frm.PointLayerName;   //点要素类名称
            IFeatureLayer polygonLayer = LayerHelper.QueryLayerByModelName(this.m_hookHelper.ActiveView.FocusMap, polygonLyrName);

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在提取...", "请等待...");
            wait.Show();

            string inClassPath = RCIS.Global.GlobalEditObject.GlobalWorkspace.PathName + "\\" + RCIS.Global.AppParameters.DATASET_DEFAULT_NAME + "\\" + polygonLyrName.ToUpper();
            wait.SetCaption ( "正在提取结点...");

            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            string tmpOutShp1 = System.Windows.Forms.Application.StartupPath + "\\tmp\\vertic2pt"+DateTime.Now.ToString("yyyyMMddHHmmss")+ ".shp";
            ESRI.ArcGIS.DataManagementTools.FeatureVerticesToPoints toPts = new FeatureVerticesToPoints();
            toPts.in_features = inClassPath;
            toPts.out_feature_class = tmpOutShp1;
            Geoprocessor GP = new Geoprocessor();
            string err=this.RunTool(GP,toPts,null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                wait.Close();
                MessageBox.Show("结点提取出错!\r\n"+err);
                return ;
            }
            wait.SetCaption("正在删除重复");
            DeleteIdentical delIdentical = new DeleteIdentical();
            delIdentical.in_dataset = tmpOutShp1;
            delIdentical.fields = "SHAPE";
            err = RunTool(GP, delIdentical, null);
            if ((!err.Contains("Succeeded")) && (!err.Contains("成功")))
            {
                wait.Close();
                MessageBox.Show("去除重复结点提取出错!\r\n" + err);
                return;
            }

            //然后进行界址点添加记录
            IWorkspace srcWs = WorkspaceHelper2.GetShapefileWorkspace(tmpOutShp1);
            IFeatureWorkspace srcFeaWs = srcWs as IFeatureWorkspace;
            IFeatureClass srcFC=srcFeaWs.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(tmpOutShp1));
            IFeatureLayer srcFLayer = new FeatureLayerClass();
            srcFLayer.FeatureClass = srcFC;
            IIdentify pIdentify = srcFLayer as IIdentify;
            IArray pIDs = pIdentify.Identify( (srcFC as IGeoDataset).Extent );
            if (pIDs == null || pIDs.Count == 0)
            {
                wait.Close();
                MessageBox.Show("提取点数据为空？！");
                return;
            }

            wait.SetCaption("开始插入界址点数据...");

            IFeatureLayer pointLyr = LayerHelper.QueryLayerByModelName(aMap, pointLyrName);
            IFeatureClass pointClass = pointLyr.FeatureClass;
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();

            IFeatureBuffer pointBuffer = pointClass.CreateFeatureBuffer();
            IFeatureCursor featureCursor = pointClass.Insert(true);
            try
            {
                for (int i = 0; i < pIDs.Count; i++)
                {
                    IFeatureIdentifyObj pFeatIdObj = pIDs.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject aSrcRow = pFeatIdObj as IRowIdentifyObject;
                    IFeature aSrcFea = aSrcRow.Row as IFeature;

                    pointBuffer.Shape = aSrcFea.ShapeCopy;
                    FeatureHelper.SetFeatureBufferValue(pointBuffer, "JZDH", i.ToString());
                    FeatureHelper.SetFeatureBufferValue(pointBuffer, "JBLX", "9");
                    FeatureHelper.SetFeatureBufferValue(pointBuffer, "JZDLX","9");
                    featureCursor.InsertFeature(pointBuffer);
                }
                featureCursor.Flush();

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("polygon2point");
                wait.Close();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(srcFLayer);
                if ((srcFC as IDataset).CanDelete())
                {
                    (srcFC as IDataset).Delete();
                }

                MessageBox.Show("生成完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pointBuffer);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(srcFeaWs);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(srcWs);


            }
            //清除所有shp
            

            



            //ArrayList arSelectedPolygon = LayerHelper.GetSelectedFeature
            //    (aMap, polygonLayer as IGeoFeatureLayer, esriGeometryType.esriGeometryPolygon);

            //if (arSelectedPolygon.Count == 0)
            //{
            //    MessageBox.Show("当前未选中要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在提取...", "请等待...");
            //wait.Show();


            //IFeatureLayer pointLyr = LayerHelper.QueryLayerByModelName(aMap, pointLyrName);
            //IFeatureClass pointClass = pointLyr.FeatureClass;
            //RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            //try
            //{
            //    foreach (IFeature aPolygonFea in arSelectedPolygon)
            //    {
            //        IPolygon aPolygon = aPolygonFea.ShapeCopy as IPolygon;

            //        wait.SetCaption("正在提取" + aPolygonFea.OID + "的点...");

            //        IPointCollection polygonPoints = aPolygon as IPointCollection;
            //        for (int i = 0; i < polygonPoints.PointCount - 1; i++)
            //        {
            //            IPoint aPt = polygonPoints.get_Point(i);
            //            ////查询有无重复
            //            //ISpatialFilter pSF = new SpatialFilterClass();
            //            //pSF.Geometry = aPt;
            //            //pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            //            //if (pointClass.FeatureCount(pSF as IQueryFilter) > 0)
            //            //{
            //            //    continue;
            //            //}
                        
            //            IFeature newFea = pointClass.CreateFeature();
            //            newFea.Shape = aPt;
            //            newFea.Store();
            //        }
            //    }
            //    RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("polygon2point");
            //    wait.Close();
            //    MessageBox.Show("生成完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    if (wait!=null)
            //        wait.Close();
            //    RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
            //    MessageBox.Show(ex.Message);
            //}




        }

        #endregion
    }
}
