using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using RCIS.Utility;
using System.Collections;

namespace TDDC3D.edit
{
    public partial class TbFeatureMergeForm : Form
    {
        public TbFeatureMergeForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;
        public IWorkspace currWS = null;

        private IFeatureClass dltbClass = null;

        private DataTable m_datatable = null;
        private void TbFeatureMergeForm_Load(object sender, EventArgs e)
        {
            
            //this.cmbLayers.Properties.Items.Clear();
            //for (int i = 0; i < currMap.LayerCount; i++)
            //{
            //    ILayer currLyr = this.currMap.get_Layer(i);
            //    if (!(currLyr is IFeatureLayer)) continue;
            //    IFeatureLayer currFeaLyr = currLyr as IFeatureLayer;
            //    IFeatureClass currClass = currFeaLyr.FeatureClass;
            //    string clsName = (currClass as IDataset).Name.ToUpper();
            //    if (currClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            //    {
            //        this.cmbLayers.Properties.Items.Add(clsName + "|"+currClass.AliasName);
            //    }
            //}
            LayerHelper.LoadLayer2Combox(this.cmbLayers, this.currMap, esriGeometryType.esriGeometryPolygon);
            

            m_datatable = new DataTable();
            DataColumn dc = new DataColumn("OID", typeof(int));
            m_datatable.Columns.Add(dc);
            dc = new DataColumn("ERROR", typeof(string));
            m_datatable.Columns.Add(dc);


        }
        //private ArrayList getTouchedFeature(IFeatureClass pFC, IGeometry geo)
        //{
        //    //邻接的 要素
        //    ArrayList ar = new ArrayList();
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
        //    pSF.Geometry = geo;
        //    IFeatureCursor pCursor = pFC.Search(pSF as IQueryFilter, false);
        //    IFeature aFea = null;
        //    try
        //    {
        //        while ((aFea = pCursor.NextFeature()) != null)
        //        {
        //            ar.Add(aFea);

        //        }
        //    }
        //    catch { }
        //    finally
        //    {
        //        OtherHelper.ReleaseComObject(pCursor);
        //    }
        //    return ar;
        //}


        IPolygon getMergePolygon(List<int> lstoids, IFeatureClass dltbClass)
        {
            //object missing = Type.Missing;
            //IGeometry geometryBag = new GeometryBagClass();
            //IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;
            //foreach (int aOid in lstoids)
            //{
            //    IFeature aFea = dltbClass.GetFeature(aOid);
            //    geometryCollection.AddGeometry(aFea.ShapeCopy, ref missing, ref missing);

            //}
            //ITopologicalOperator unionedPolygon = new PolygonClass();
            //unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);
            //IPolygon polygon = unionedPolygon as IPolygon;
            //return polygon;

            IFeature firstFea = dltbClass.GetFeature(lstoids[0]);
            IGeometry geo = null;
            ITopologicalOperator ptop = firstFea.ShapeCopy as ITopologicalOperator;
            for (int i = 1; i < lstoids.Count; i++)
            {
                IFeature afea = dltbClass.GetFeature(lstoids[i]);
                geo = ptop.Union(afea.ShapeCopy);
                ptop = geo as ITopologicalOperator;
            }
            ptop.Simplify();
            geo.SnapToSpatialReference();
            return geo as IPolygon;
            
        }

        private void delteFeatures(List<int> lstOid, IFeatureClass dltbClass)
        {
            string fldName = dltbClass.OIDFieldName;
            string where = fldName + " in (";
            foreach(int oid in lstOid)
            {
                where += oid + ",";
            }
            if (where.EndsWith(","))
                where = where.Remove(where.Length - 1, 1);
            where += " ) ";

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = where;
            ITable pTable = dltbClass as ITable;
            pTable.DeleteSearchedRows(pQueryFilter);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pQueryFilter);
        }





        /// <summary>
        /// 合并选中要素
        /// </summary>
        /// <param name="pFlyr"></param>
        /// <returns></returns>
        private IGeometry MergeSelectionFea(IFeatureLayer pFlyr)
        {
            IEnumGeometry pGeos = new EnumFeatureGeometryClass();
            IEnumGeometryBind pGeosBind = pGeos as IEnumGeometryBind;
            IFeatureSelection pflyrSelection = pFlyr as IFeatureSelection;
            pflyrSelection.SelectFeatures(null, esriSelectionResultEnum.esriSelectionResultNew, false); pGeosBind.BindGeometrySource(null, pflyrSelection.SelectionSet);
            pGeos.Reset();
            IPolygon sPoly = new PolygonClass();
            ITopologicalOperator pTopo = sPoly as ITopologicalOperator;
            pTopo.ConstructUnion(pGeos);
            pTopo.Simplify();
            sPoly = pTopo as IPolygon;
            sPoly.SnapToSpatialReference();
            pflyrSelection.Clear();
            return sPoly;
        }

       

        //递归查找
        private void getMergedFeatures(IFeature inFea,ref  List<int> lst)
        {
            string qsxz = FeatureHelper.GetFeatureStringValue(inFea, "QSXZ");
            string qsdwdm = FeatureHelper.GetFeatureStringValue(inFea, "QSDWDM");
            string zldwdm = FeatureHelper.GetFeatureStringValue(inFea, "ZLDWDM");
            string dlbm = FeatureHelper.GetFeatureStringValue(inFea, "DLBM");
            string Tbxhdm = FeatureHelper.GetFeatureStringValue(inFea, "TBXHDM");
            string zzsxdm = FeatureHelper.GetFeatureStringValue(inFea, "ZZSXDM");


            //根据传入要素，不断查找相邻并可以合并的要素，记录其ID
            ISpatialFilter queryFilter = new SpatialFilterClass();
            queryFilter.Geometry = inFea.ShapeCopy;
            queryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelTouches;
            IFeatureCursor feaCursor = dltbClass.Search(queryFilter, false);
            IFeature aFeature = null;
            while ((aFeature = feaCursor.NextFeature()) != null)
            {
                string qsxz2 = FeatureHelper.GetFeatureStringValue(aFeature, "QSXZ");
                string qsdwdm2 = FeatureHelper.GetFeatureStringValue(aFeature, "QSDWDM");
                string zldwdm2 = FeatureHelper.GetFeatureStringValue(aFeature, "ZLDWDM");
                string dlbm2 = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                string Tbxhdm2 = FeatureHelper.GetFeatureStringValue(inFea, "TBXHDM");
                string zzsxdm2 = FeatureHelper.GetFeatureStringValue(inFea, "ZZSXDM");

                if ((qsxz == qsxz2) && (qsdwdm == qsdwdm2) && (zldwdm == zldwdm2) && (dlbm == dlbm2)
                    && (Tbxhdm==Tbxhdm2) && (zzsxdm==zzsxdm2)
                    )
                {
                    if (!lst.Contains(aFeature.OID))
                    {
                        lst.Add(aFeature.OID);
                        getMergedFeatures(aFeature, ref lst);
                    }
                }
                
            }
            OtherHelper.ReleaseComObject(feaCursor);

        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "") return;
            string dltbClsName = OtherHelper.GetLeftName(this.cmbLayers.Text);

            if (dltbClass == null)
            {
                dltbClass = (this.currWS as IFeatureWorkspace).OpenFeatureClass(dltbClsName);
            }
            m_datatable.Rows.Clear();
           
            IFeatureLayer dltbLayer = new FeatureLayerClass();
            dltbLayer.FeatureClass = dltbClass;

            List<List<int>> lstMergeSet = new List<List<int>>();
            List<int> queryedFeaturesOid = new List<int>();  //已经搜过的，不再搜了
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在进行融合，请稍等...", "请稍等");
            wait.Show();
            
            IGeometry geo = (this.currMap as IActiveView).Extent;          
            RCIS.Global.GlobalEditObject.CurrentEngineEditor.StartOperation();
            try
            {

                ISpatialFilter queryFilter = new SpatialFilterClass();
                queryFilter.Geometry = geo;
                queryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor cursor = dltbClass.Search(queryFilter, false);
                IFeature aFea = cursor.NextFeature();
                while (aFea != null)
                {
                    if (queryedFeaturesOid.Contains(aFea.OID))
                    {
                        aFea = cursor.NextFeature();
                        continue;
                    }
                    List<int> aMergeSet = new List<int>();
                    wait.SetCaption("正在查找"+aFea.OID+"的邻接要素集合...");
                    getMergedFeatures(aFea, ref aMergeSet);

                    queryedFeaturesOid = queryedFeaturesOid.Union(aMergeSet).ToList<int>(); 

                    if (aMergeSet.Count >1)
                    {
                        lstMergeSet.Add(aMergeSet);
                    }
                    
                    aFea = cursor.NextFeature();
                }
                OtherHelper.ReleaseComObject(cursor);

                foreach (List<int> aMergeLst in lstMergeSet)
                {
                    wait.SetCaption("正在合并"+aMergeLst[0]+"...");
                    try
                    {
                        //只有一个的不处理
                        IFeature firstFea = dltbClass.GetFeature(aMergeLst[0]);
                        IGeometry newGeo = getMergePolygon(aMergeLst, dltbClass);
                        IFeature newFea = dltbClass.CreateFeature();
                        FeatureHelper.CopyFeature(firstFea, newFea);
                        newFea.Shape = newGeo;
                        newFea.Store();

                        //删除旧的
                        delteFeatures(aMergeLst, dltbClass);
                    }
                    catch (Exception ex)
                    {
                        DataRow aErr = m_datatable.NewRow();
                        aErr["OID"] = aMergeLst[0];
                        aErr["ERROR"] = ex.Message;
                        m_datatable.Rows.Add(aErr);

                    }
                    
                }

                RCIS.Global.GlobalEditObject.CurrentEngineEditor.StopOperation("mergeallfeature");
                wait.Close();
                if (m_datatable.Rows.Count == 0)
                {
                    MessageBox.Show("合并完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("合并完成，但是部分未成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.gridControl1.DataSource = m_datatable;
                    this.xtraTabControl1.SelectedTabPageIndex = 1;
                }
                
                (this.currMap as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewAll, null, geo as IEnvelope);
                Close();
            }
            catch (Exception ex)
            {
                RCIS.Global.GlobalEditObject.CurrentEngineEditor.AbortOperation();
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

            

        }

        private void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }
    }
}
