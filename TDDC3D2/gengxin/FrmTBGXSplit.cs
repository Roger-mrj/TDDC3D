using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;

namespace TDDC3D.gengxin
{
    public partial class FrmTBGXSplit : Form
    {
        public FrmTBGXSplit()
        {
            InitializeComponent();
        }
        public IWorkspace pCurrentWorkspace;
        public IMap currMap = null;
        private void FrmTBGXSplit_Load(object sender, EventArgs e)
        {
            IEnumDataset pEnumDataset = pCurrentWorkspace.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                if (pDataset.Type == esriDatasetType.esriDTFeatureDataset)
                {
                    IFeatureClassContainer pFCC = pDataset as IFeatureClassContainer;
                    IEnumFeatureClass pEnumFC = pFCC.Classes;
                    pEnumFC.Reset();
                    IFeatureClass pFeatureClass = pEnumFC.Next();
                    while (pFeatureClass != null)
                    {
                        comSoueceLayer.Properties.Items.Add(pFeatureClass.AliasName + "|" + (pFeatureClass as IDataset).Name);
                        comTargetLayer.Properties.Items.Add(pFeatureClass.AliasName + "|" + (pFeatureClass as IDataset).Name);
                        //Layers.Add(pFeatureClass.AliasName,(pFeatureClass as IDataset).Name);
                        pFeatureClass = pEnumFC.Next();
                    }
                }
                //else
                //{
                //    //comLayer.Properties.Items.Add((pDataset as IFeatureClass).AliasName + "|" + pDataset.Name);
                //}
                pDataset = pEnumDataset.Next();
            }

            if (comSoueceLayer.Properties.Items.Contains("地类图斑|DLTB"))
                comSoueceLayer.SelectedItem = "地类图斑|DLTB";
            if (comTargetLayer.Properties.Items.Contains("地类图斑更新|DLTBGX"))
                comTargetLayer.SelectedItem = "地类图斑更新|DLTBGX";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (comSoueceLayer.SelectedItem.ToString() != "地类图斑|DLTB")
            {
                MessageBox.Show("请选择地类图斑层", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; 
            }
            if (comTargetLayer.SelectedItem.ToString() != "地类图斑更新|DLTBGX")
            {
                MessageBox.Show("请选择地类图斑更新层", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; 
            }

            string dltbLayerName = RCIS.Utility.OtherHelper.GetRightName(this.comSoueceLayer.Text.Trim());
            string dltbgxLayerName = RCIS.Utility.OtherHelper.GetRightName(this.comTargetLayer.Text.Trim());
            //return;
            IFeatureClass dltbClass = null;
            IFeatureClass dltbgxClass = null;
            try
            {
                IFeatureWorkspace pFeaWs = pCurrentWorkspace as IFeatureWorkspace;
                dltbClass = pFeaWs.OpenFeatureClass(dltbLayerName);
                dltbgxClass = pFeaWs.OpenFeatureClass(dltbgxLayerName);
            }
            catch { }
            if (dltbgxClass == null||dltbClass==null)
            {
                MessageBox.Show("找不到必备图层。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("图斑分割", "正在处理，请稍等...");
            
            //this.Visible = false;
            wait.Show();
            //System.Threading.Thread.Sleep(10000);
            //wait.Close();
            //return;
            string tmp = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;
            if (Directory.Exists(tmp))
            {
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
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
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            //if (File.Exists(Application.StartupPath + "\\tmp\\intersectDLTB"))
            //{
            //    (RCIS.GISCommon.WorkspaceHelper2.GetShapefileFeatureClass(Application.StartupPath + "\\tmp\\intersectDLTB") as IDataset).Delete();
            //}
            IFeatureClass pIntersectClass = null;
            pIntersectClass = (tmpWS as IFeatureWorkspace).CreateFeatureClass("intersectDLTB", dltbgxClass.Fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", null);
            
            IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(dltbgxClass);
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.Geometry = pGeo;
            pSF.GeometryField = dltbgxClass.ShapeFieldName;
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor pCursor = dltbClass.Search(pSF,true);
            IFeature pFeature;
            IFeatureCursor pInsert = pIntersectClass.Insert(true);
            while ((pFeature = pCursor.NextFeature()) != null)
            {
                IFeatureBuffer pBuffer = pIntersectClass.CreateFeatureBuffer();
                pBuffer.Shape = pFeature.ShapeCopy;
                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string fieldName = pFeature.Fields.get_Field(i).Name;
                    if (!fieldName.ToUpper().Contains("SHAPE") && fieldName.ToUpper() != "OBJECTID" && fieldName.ToUpper() != "SJNF")
                    {
                        //object val = pFeature.get_Value(i).ToString() as object;
                        pBuffer.set_Value(pBuffer.Fields.FindField(fieldName), pFeature.get_Value(i));
                    }
                }
                pInsert.InsertFeature(pBuffer);
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);

            }
            //pInsert.Flush();
            RCIS.Utility.OtherHelper.ReleaseComObject(pInsert);
            RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
            //List<string> arr=new List<string>();
            //if(radioGroup1.SelectedIndex==0)
            //    arr.Add("QSXZ");
            //else
            //    arr.Add("QSDWDM");
            //string[] fields=arr.ToArray();
            string[] qsxz = new string[] { "QSXZ"};
            string[] qsdw = new string[] { "QSDWDM" };
            string[] qsdwaa = new string[] { };
            IFeatureClass qsdwClass = RCIS.GISCommon.GpToolHelper.Dissolve(pIntersectClass, tmpWS, "qsdwDissolve", qsdw);
            IFeatureClass qsxzClass = RCIS.GISCommon.GpToolHelper.Dissolve(pIntersectClass, tmpWS, "qsxzDissolve", qsxz);
            //bool b = RCIS.GISCommon.GpToolHelper.Dissolve(tmpWS.PathName + "\\intersectDLTB", tmpWS.PathName + "\\qssssxz", qsdwaa);

            //IFeatureClass qsdwClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("qsdw");
            //IFeatureClass qsxzClass = (tmpWS as IFeatureWorkspace).OpenFeatureClass("qs");
            //IFeatureClass qsxzClass = RCIS.GISCommon.GpToolHelper.Dissolve(pIntersectClass, tmpWS, "qsxzDissolve", qsxz);
            
            List<int> errList = new List<int>();
            //DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("图斑分割", "正在处理，请稍等...");
            //wait.Show();
            wait.SetCaption("图斑分割");
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {

                SplitDLTBGX(qsxzClass, dltbgxClass, qsxz[0], wait, (this.currMap as IActiveView).Extent, ref errList);
                SplitDLTBGX(qsdwClass, dltbgxClass, qsdw[0], wait, (this.currMap as IActiveView).Extent, ref errList);
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("xzqsplittb");
                wait.Close();
                MessageBox.Show("分割完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

            memoEdit1.Text = "";
            foreach (int oid in errList)
            {
                memoEdit1.Text += oid + "\r\n";
            }
        }


        private void SplitDLTBGX(IFeatureClass zdClass, IFeatureClass dltbClass,string whereField, DevExpress.Utils.WaitDialogForm wait, IEnvelope extent, ref List<int> errId)
        {

            List<IFeature> allZds = GetFeaturesHelper.getFeaturesByGeo(zdClass, extent as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
            foreach (IFeature aZdFea in allZds)
            {
                #region 切割图斑

                List<IFeature> interDltbs = getFeaByBoundaryIntersect(dltbClass, aZdFea.ShapeCopy,whereField);
                ITopologicalOperator pTopXzqGeo = aZdFea.ShapeCopy as ITopologicalOperator;
                IGeometry pXzqJX = pTopXzqGeo.Boundary;
                //用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称
                foreach (IFeature aDltb in interDltbs)
                {

                    //如果行政区 和 该图斑不是交于一个面，则略过
                    IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                    if (cutPolygon.IsEmpty)
                    {
                        continue;
                    }
                    wait.SetCaption("正在用 [" + aZdFea.OID + "] 切割图斑...");

                    try
                    {
                        IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                        ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                        newFeaturesSet.Reset();
                        IFeature newTb = newFeaturesSet.Next() as IFeature;
                        while (newTb != null)
                        {

                            newTb = newFeaturesSet.Next() as IFeature;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!errId.Contains(aDltb.OID))
                        {
                            errId.Add(aDltb.OID);
                        }
                    }

                }
                #endregion

            }

        }

        private List<IFeature> getFeaByBoundaryIntersect(IFeatureClass targetClass, IGeometry xzqGeo,string whereField)
        {
            List<IFeature> lst = new List<IFeature>();
            ITopologicalOperator pTop = xzqGeo as ITopologicalOperator;
            IGeometry pXzqJX = pTop.Boundary;
            ITopologicalOperator pJxTop = pXzqJX as ITopologicalOperator;
            using (ESRI.ArcGIS.ADF.ComReleaser release = new ESRI.ArcGIS.ADF.ComReleaser())
            {

                ISpatialFilter pSR = new SpatialFilterClass();
                pSR.Geometry = pXzqJX;
                pSR.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pSR.WhereClause = "" + whereField + " ='' or " + whereField + " is null";
                IQueryFilter pQF = new QueryFilterClass();
                pQF = pSR as IQueryFilter;
                //pQF.WhereClause = ""+whereField+" is null";
                IFeatureCursor pCursor = targetClass.Search(pQF, false);
                release.ManageLifetime(pCursor);
                IFeature aFea = pCursor.NextFeature();
                while (aFea != null)
                {

                    IGeometry intersecLine = pJxTop.Intersect(aFea.ShapeCopy, esriGeometryDimension.esriGeometry1Dimension);
                    if (intersecLine.IsEmpty)
                    {
                        aFea = pCursor.NextFeature();
                        continue;
                    }

                    //如果交与一条线,且不包含
                    if (GeometryHelper.IsContain(xzqGeo, aFea.ShapeCopy))
                    {
                        aFea = pCursor.NextFeature();
                        continue;
                    }

                    lst.Add(aFea);
                    aFea = pCursor.NextFeature();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pSR);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
            return lst;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
