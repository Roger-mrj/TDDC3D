using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using RCIS.GISCommon;
using RCIS.Utility;

namespace TDDC3D.datado
{
    public partial class ZldwSetValueForm : Form
    {
        public ZldwSetValueForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        public IMap currMap = null;


        private IFeatureClass xzqClass = null;

        private IFeatureClass zdClass = null;

        private void ZldwSetValueForm_Load(object sender, EventArgs e)
        {
            if (currWs == null)
                return;
            this.chkXzqList.Items.Clear();
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;

            xzqClass = pFeaWs.OpenFeatureClass("XZQ");            
            IFeatureLayer xzqLyr = new FeatureLayerClass();
            xzqLyr.FeatureClass = xzqClass;
            //获取所有行政区要素
            IIdentify xzqIndentify = xzqLyr as IIdentify;
            IDataset xzqDS = xzqClass as IDataset;
            IGeoDataset xzqGeoDs = xzqDS as IGeoDataset;
            IArray arrXZQIDs = xzqIndentify.Identify(xzqGeoDs.Extent);
            if (arrXZQIDs != null)
            {
                for (int i = 0; i < arrXZQIDs.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arrXZQIDs.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    string xzqmc = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");
                    string xzqdm = FeatureHelper.GetFeatureStringValue(pfea, "XZQDM");

                    int idx = this.chkXzqList.Items.Add(xzqdm + "|" + xzqmc);
                    this.chkXzqList.SetItemChecked(idx, true);
                }
            }

            //添加宗地代码

            zdClass = pFeaWs.OpenFeatureClass("CJDCQ");
            IFeatureLayer zdLyr = new FeatureLayerClass();
            zdLyr.FeatureClass = zdClass;
            IIdentify zdIndentify = zdLyr as IIdentify;
            IDataset zdDS = zdClass as IDataset;
            IGeoDataset zdGeoDs = zdDS as IGeoDataset;
            IArray arrZdIds = zdIndentify.Identify(zdGeoDs.Extent);
            if (arrZdIds != null)
            {
                for (int i = 0; i < arrZdIds.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arrZdIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    string zddm = FeatureHelper.GetFeatureStringValue(pfea, "QSQDM");
                    if (zddm.Trim() == "")
                        continue;
                    //if (zddm.Length>12)
                    //{
                    //    zddm = zddm.Substring(0, 12);
                    //}
                    int idx = this.chkZDList.Items.Add(zddm);
                    this.chkZDList.SetItemChecked(idx, true);
                }

            }
            RCIS.GISCommon.LayerHelper.LoadLayer2Combox(this.cmbLayers, currMap);

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkXzqList.Items.Count; i++)
            {
                this.chkXzqList.SetItemChecked(i, true);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkXzqList.Items.Count; i++)
            {
                this.chkXzqList.SetItemChecked(i, !this.chkXzqList.GetItemChecked(i));
            }
        }
        //private ArrayList getAllTb(IGeometry xzqGeo,IFeatureClass dltbClass)
        //{
        //    ArrayList arTbs=new ArrayList();
        //    ISpatialFilter pSF = new SpatialFilterClass();
        //    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
        //    pSF.Geometry = xzqGeo;
        //    IFeatureCursor pCursor = dltbClass.Search(pSF, false);
        //    IFeature aFeature = null;
        //    while ((aFeature = pCursor.NextFeature()) != null)
        //    {
        //        arTbs.Add(aFeature);
        //    }
        //    OtherHelper.ReleaseComObject(pCursor);
            

        //    return arTbs;
        
        //}

         private IFeature getAXzq(string xzqdm)
        {
            IFeature aFea = null;
            string where = "XZQDM='" + xzqdm + "'";
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            
            IFeatureCursor cursor = xzqClass.Search(pQF, false);
            aFea = cursor.NextFeature();
            OtherHelper.ReleaseComObject(cursor);
            return aFea;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayers.Text.Trim() == "") return ;
            IFeatureClass pFC = (this.currWs as IFeatureWorkspace).OpenFeatureClass(OtherHelper.GetLeftName( this.cmbLayers.Text.Trim()));
            string fcName=(pFC as IDataset).Name.ToUpper();
            if (pFC.Fields.FindField("ZLDWDM") < 0   || pFC.Fields.FindField("QSDWDM") < 0)
            {
                MessageBox.Show("该图层没有座落单位代码要处理！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行编号，请稍等...", "处理中");
            wait.Show();
           
            try
            {
                //先清空，然后后面可以按照 空值 删除了
                string sql = "update " + fcName + " set ZLDWDM='' ,QSDWDM='',ZLDWMC='',QSDWMC='' ";
                this.currWs.ExecuteSQL(sql);

                for (int i = 0; i < this.chkXzqList.CheckedItems.Count; i++)
                {
                    string txt = this.chkXzqList.CheckedItems[i].ToString().Trim();
                    string xzqdm = OtherHelper.GetLeftName(txt);
                    string oldDm = xzqdm;
                    if (xzqdm.Length<19)
                    {
                        xzqdm = xzqdm.PadRight(19, '0');
                    }
                    string xzqmc = OtherHelper.GetRightName(txt);
                    wait.SetCaption("正在执行" + xzqmc + "的图斑坐落单位代码赋值...");
                    Application.DoEvents();

                    IFeature aXzqFea = getAXzq(oldDm);
                    if (aXzqFea != null)
                    {
                        //找到所有地类图斑
                        List<IFeature> arAllTbs = GetFeaturesHelper.getFeaturesByGeo(pFC, aXzqFea.ShapeCopy, esriSpatialRelEnum.esriSpatialRelContains);
                        //ArrayList arAllTbs =  //getAllTb(aXzqFea.ShapeCopy,pFC);                        
                        IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
                        pWSEdit.StartEditing(true);
                        pWSEdit.StartEditOperation();
                        try
                        {
                            foreach (IFeature aTb in arAllTbs)
                            {
                                FeatureHelper.SetFeatureValue(aTb, "ZLDWDM",xzqdm);
                                FeatureHelper.SetFeatureValue(aTb,"ZLDWMC",xzqmc);

                                aTb.Store();
                            }

                            pWSEdit.StopEditOperation();
                            pWSEdit.StopEditing(true);
                                
                        }
                        catch (Exception ex)
                        {
                            pWSEdit.AbortEditOperation();
                            pWSEdit.StopEditing(false);
                        }                        
                    }
                }

                for (int i = 0; i < this.chkZDList.CheckedItems.Count; i++)
                {
                    string zddm = this.chkZDList.CheckedItems[i].ToString().Trim();

                    wait.SetCaption("正在执行" + zddm + "的图斑权属单位代码赋值...");
                    Application.DoEvents();
                    //找到 权属单位代码对应的图形
                    List<IFeature> lst = GetFeaturesHelper.getFeaturesBySql(zdClass, "QSQDM = '" + zddm + "'");
                    //IPolygon polygon= GeometryHelper.UnionPolygon(lst);
                    if (lst.Count == 0) continue;
                    IPolygon polygon = lst[0].ShapeCopy as IPolygon;
                    if (!polygon.IsEmpty)
                    {
                        //找到所有地类图斑
                        List<IFeature> arAllTbs = GetFeaturesHelper.getFeaturesByGeo(pFC, polygon as IGeometry, esriSpatialRelEnum.esriSpatialRelContains);
                        IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
                        pWSEdit.StartEditing(true);
                        pWSEdit.StartEditOperation();
                        try
                        {
                            string dm2 = zddm.Substring(0,12).PadRight(19, '0');    

                            foreach (IFeature aTb in arAllTbs)
                            {

                                FeatureHelper.SetFeatureValue(aTb, "QSDWDM", dm2);
                                aTb.Store();
                            }
                            pWSEdit.StopEditOperation();
                            pWSEdit.StopEditing(true);

                        }
                        catch (Exception ex)
                        {
                            pWSEdit.AbortEditOperation();
                            pWSEdit.StopEditing(false);
                        }
                    }
                }

                wait.Close();
                MessageBox.Show("赋值完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }
        

        }
    }
}
