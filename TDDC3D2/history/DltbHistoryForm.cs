using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.GISCommon;
using RCIS.Utility;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace TDDC3D.edit
{
    public partial class DltbHistoryForm : Form
    {
        public DltbHistoryForm()
        {
            InitializeComponent();
        }

        public IWorkspace currWs = null;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private IFeatureClass dltb_hClass = null;
        private IFeatureClass dltbClass = null;



        

        private void DltbHistoryForm_Load(object sender, EventArgs e)
        {
            try
            {
                dltb_hClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB_H");
                dltbClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            }
            catch (Exception ex)
            {
            }
            if (dltb_hClass == null)
            {
                MessageBox.Show("没有启用更新历史或者无更新历史数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ArrayList arGxsj = FeatureHelper.GetUniqueFieldValueByDataStatistics(dltb_hClass,null, "GXSJ");
            foreach (string aBgrq in arGxsj)
            {
                lstBgrq.Items.Add(aBgrq);
            }
        }
        private IGeometry getFirstGeo(string gsxj)
        {
            IGeometry geo = null;
            IQueryFilter pQF=new QueryFilterClass();
            pQF.WhereClause = "GXSJ='" + gsxj + "'";
            IFeatureCursor cursor = dltb_hClass.Search(pQF, false);
            try
            {
                IFeature aFea = cursor.NextFeature();
                if (aFea != null)
                    geo = aFea.ShapeCopy;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            }
            return geo;
        }


        


        /// <summary>
        /// 获取交与一个面的地类图斑
        /// </summary>
        /// <param name="cutGeo"></param>
        /// <param name="aClass"></param>
        /// <returns></returns>
        private List<IFeature> GetDLTBFeatures(IGeometry cutGeo, IFeatureClass aClass)
        {
            List<IFeature> lstZD = new List<IFeature>();

            ISpatialFilter spatialFilter = new SpatialFilterClass();
            spatialFilter.Geometry = cutGeo;
            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureClass featureClass = aClass;

            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
            IFeature aFea = featureCursor.NextFeature();
            try
            {
                while (aFea != null)
                {
                    IGeometry aGeo = aFea.ShapeCopy;
                    ITopologicalOperator ptop = aGeo as ITopologicalOperator;
                    IGeometry pInterSectGeo = ptop.Intersect(cutGeo, esriGeometryDimension.esriGeometry2Dimension);
                    if (pInterSectGeo != null && !pInterSectGeo.IsEmpty)
                    {

                        lstZD.Add(aFea);
                    }
                    aFea = featureCursor.NextFeature();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                GC.Collect();
            }
            return lstZD;
        }


        private void lstBgrq_DoubleClick(object sender, EventArgs e)
        {
            //根据批次 ，得到 所有变更过程记录
            if (this.lstBgrq.SelectedItem == null) return;
            string bgrq = this.lstBgrq.SelectedItem.ToString().Trim();
            Cursor = Cursors.WaitCursor;
            try
            {
                IGeometry extent = getFirstGeo(bgrq);
                IFeatureLayer pFeaLyr = new FeatureLayerClass();
                pFeaLyr.FeatureClass =dltb_hClass;
                pFeaLyr.Name = "历史地类图斑";
                
                IFeatureLayerDefinition pFlDefinition = pFeaLyr as IFeatureLayerDefinition;
                pFlDefinition.DefinitionExpression = "GXSJ='" + bgrq + "'";
                this.axMapControl1.ClearLayers();
                this.axMapControl1.AddLayer(pFeaLyr as ILayer);
                this.axMapControl1.Extent = extent.Envelope;
                this.axMapControl1.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //还原该批次
            if (MessageBox.Show("确实要将该批次地类图版数据进行还原么，不可恢复。\r\n还原时请严格遵循时间先后关系进行还原。",
                "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            if (this.lstBgrq.SelectedItem == null) return;
            IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
            pWSEdit.StartEditing(false);
            pWSEdit.StartEditOperation();
            try
            {
                //将时间对应所有要素，复制回原来的，将原来有交集的那些删掉

                string bgrq = this.lstBgrq.SelectedItem.ToString().Trim();
                List<IFeature> oldFeas = GetFeaturesHelper.getFeaturesBySql(dltb_hClass, "GXSJ='" + bgrq + "'");
                IGeometry cutGeo = GeometryHelper.UnionPolygon(oldFeas);
                List<IFeature> currFeatures = this.GetDLTBFeatures(cutGeo, dltbClass);
                foreach (IFeature currFea in currFeatures)
                {
                    currFea.Delete();
                }

                GxHistoryHelper.RestoreByHistory(dltbClass, oldFeas);


                if (MessageBox.Show("还原完毕，是否删除该历史纪录。", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                foreach (IFeature aoldFea in oldFeas)
                {
                    aoldFea.Delete();
                }
                Cursor = Cursors.Default;
                pWSEdit.StopEditOperation();
                pWSEdit.StopEditing(true);
                MessageBox.Show("还原完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pWSEdit.AbortEditOperation();
                pWSEdit.StopEditing(false);
                MessageBox.Show(ex.Message);
                Cursor = Cursors.Default;
            }
            



        }
    }
}
