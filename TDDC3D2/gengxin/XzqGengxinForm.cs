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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

using System.Collections;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.gengxin
{
    public partial class XzqGengxinForm : Form
    {
        public XzqGengxinForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
       
        private IFeatureClass xzqClass = null;
        private IFeatureClass dltbClass = null;

        private List<IFeature> allXzq = new List<IFeature>();

        private void XzqGengxinForm_Load(object sender, EventArgs e)
        {
            if (currWs == null)
                return;
            this.chkXzqList.Items.Clear();
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            try
            {
                xzqClass = pFeaWs.OpenFeatureClass("XZQ");
                dltbClass = pFeaWs.OpenFeatureClass("DLTB");
            }
            catch { }
            if (xzqClass == null || dltbClass==null ) 
                return;
            allXzq = GetFeaturesHelper.getFeaturesByGeo(xzqClass, (xzqClass as IGeoDataset).Extent as IGeometry , esriSpatialRelEnum.esriSpatialRelIntersects);            
            foreach (IFeature aXzq in allXzq)
            {
                string xzqmc = FeatureHelper.GetFeatureStringValue(aXzq, "XZQMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(aXzq, "XZQDM");

                int idx = this.chkXzqList.Items.Add(xzqdm + "|" + xzqmc);
                this.chkXzqList.SetItemChecked(idx, true);
            }


            
        }

        private IFeature getAXzq(string xzqdm)
        {
            IFeature aFea = null;
            string where = "XZQDM='" + xzqdm + "'";
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;

            IFeatureCursor cursor = xzqClass.Search(pQF, false);
            aFea = cursor.NextFeature();
            OtherHelper.ReleaseComObject(cursor);
            OtherHelper.ReleaseComObject(pQF);
            return aFea;
        }


        

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.chkXzqList.CheckedItemsCount == 0)
            {
                MessageBox.Show("请首先选择行政区！");
                return;
            }
            this.memoLog.Text = "";
            int currDh = -1;
            this.lblstatus.Text = "开始执行...";
            IWorkspaceEdit wsEdit = currWs as IWorkspaceEdit;
            wsEdit.StartEditing(true);
            wsEdit.StartEditOperation();
            try
            {
                for (int i = 0; i < this.chkXzqList.CheckedItemsCount; i++)
                {
                    string txt = this.chkXzqList.CheckedItems[i].ToString();
                    string xzqdm = OtherHelper.GetLeftName(txt);
                    string xzqmc = OtherHelper.GetRightName(txt);
                    IFeature aXzqFeature = this.getAXzq(xzqdm);
                    //椭球面积                            
                    if (currDh == -1)
                    {
                        IPoint selectPoint = (aXzqFeature.ShapeCopy as IArea).Centroid;
                        double X = selectPoint.X;
                        currDh = (int)(X / 1000000);////WK---带号     
                    }

                    #region 切割图斑 并附属性

                    List<IFeature> interDltbs = GetFeaturesHelper.getFeatureByBoundaryIntersect(dltbClass, aXzqFeature.ShapeCopy);
                    ITopologicalOperator pTopXzqGeo = aXzqFeature.ShapeCopy as ITopologicalOperator;
                    IGeometry pXzqJX = pTopXzqGeo.Boundary;
                    //用这个界限去切割 图斑 ，同时 赋值佐罗单位代码名称
                    foreach (IFeature aDltb in interDltbs)
                    {
                        lblstatus.Text = "开始处理图斑" + aDltb.OID + "...";
                        lblstatus.Update();
                        //如果行政区 和 该图斑不是交于一个面，则略过
                        IGeometry cutPolygon = pTopXzqGeo.Intersect(aDltb.ShapeCopy, esriGeometryDimension.esriGeometry2Dimension);
                        if (cutPolygon.IsEmpty)
                        {
                            continue;
                        }
                        try
                        {
                            IFeatureEdit featureEdit = aDltb as IFeatureEdit;
                            ISet newFeaturesSet = featureEdit.Split(pXzqJX);
                            newFeaturesSet.Reset();

                            IFeature newTb = newFeaturesSet.Next() as IFeature;
                            while (newTb != null)
                            {
                                newTb = newFeaturesSet.Next() as IFeature;                                
                                SphereArea.SphereAreaClass area = new SphereArea.SphereAreaClass();
                                double tbmj = area.SphereArea(newTb.ShapeCopy, currDh);
                                FeatureHelper.SetFeatureValue(newTb, "TBMJ", tbmj);
                                //计算图斑面积后   再计算扣除面积
                                double tkxs = FeatureHelper.GetFeatureDoubleValue(newTb, "KCXS");
                                if (tkxs == 0)
                                {
                                    FeatureHelper.SetFeatureValue(newTb, "TBDLMJ", tbmj);
                                }
                                else
                                {
                                    double TKMJ = MathHelper.RoundEx(tbmj * tkxs, 2);
                                    double dlmj = MathHelper.RoundEx(tbmj - TKMJ, 2);
                                    FeatureHelper.SetFeatureValue(newTb, "KCMJ", TKMJ);
                                    FeatureHelper.SetFeatureValue(newTb, "TBDLMJ", dlmj);
                                }
                                newTb.Store();
                            }
                        }
                        catch (Exception ex)
                        {
                            memoLog.Text += "\r\n" + aDltb.OID + "切割失败！";
                        }

                    }
                    #endregion

                    //将 该XZQ下 的 地类图斑的座落单位代码和名称
                    List<IFeature> arAllTbs = GetFeaturesHelper.getFeaturesByGeo(dltbClass, aXzqFeature.ShapeCopy, esriSpatialRelEnum.esriSpatialRelContains);
                    foreach (IFeature aTb in arAllTbs)
                    {
                        lblstatus.Text = "开始为" + xzqmc + "下的地类图斑的坐落单位代码赋值...";
                        lblstatus.Update();
                        FeatureHelper.SetFeatureValue(aTb, "ZLDWDM", xzqdm);
                        FeatureHelper.SetFeatureValue(aTb, "ZLDWMC", xzqmc);
                        aTb.Store();
                    }
                }
                lblstatus.Text = "";
                wsEdit.StopEditOperation();
                wsEdit.StopEditing(true);
                MessageBox.Show("执行完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.memoLog.Text.Trim() == "")
                {
                    this.xtraTabControl1.SelectedTabPageIndex = 1;
                }

            }
            catch (Exception ex)
            {
                wsEdit.AbortEditOperation();
                wsEdit.StopEditing(false);
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
