using System;
using System.Collections;
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
    public partial class TbbhSetForm : Form
    {
        public TbbhSetForm()
        {
            InitializeComponent();
        }


        public IWorkspace currWs = null;
        public IMap currMap = null;

        private IFeatureClass cjdcqClass = null;
        private IFeatureClass dltbClass = null;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TbbhSetForm_Load(object sender, EventArgs e)
        {
            if (currWs == null)
                return;

            dltbClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            

            LayerHelper.LoadLayer2Combox(this.cmbLayer, this.currMap,  esriGeometryType.esriGeometryPolygon);
            int idx1 = -1;
            for (int i = 0; i < this.cmbLayer.Properties.Items.Count; i++)
            {
                string name = this.cmbLayer.Properties.Items[i].ToString().Trim().ToUpper();
                name = OtherHelper.GetLeftName(name);
                if (name.ToUpper().Equals("CJDCQ"))
                {
                    idx1 = i;
                    break;
                }
            }
            this.cmbLayer.SelectedIndex = idx1;
            


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

        private List<IFeature> getXzqs(string xzqdm)
        {
            List<IFeature> lst = new List<IFeature>();
            IFeature aFea = null;
            string where = "ZLDWDM='" + xzqdm + "'";
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            IFeatureCursor cursor = cjdcqClass.Search(pQF, false);
            try
            {
                
                while ((aFea = cursor.NextFeature()) != null)
                {
                    lst.Add(aFea);
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                OtherHelper.ReleaseComObject(cursor);
                OtherHelper.ReleaseComObject(pQF);

            }
            
            return lst;

        }

        //private IFeature getAXzq(string xzqdm)
        //{
        //    IFeature aFea = null;
        //    string where = "ZLDWDM='" + xzqdm + "'";
        //    IQueryFilter pQF = new QueryFilterClass();
        //    pQF.WhereClause = where;
            
        //    IFeatureCursor cursor = cjdcqClass.Search(pQF, false);
        //    aFea = cursor.NextFeature();
        //    OtherHelper.ReleaseComObject(cursor);
        //    return aFea;
        //}

        private ArrayList getAllTb(IGeometry xzqGeo)
        {
            
            ArrayList arTbs = new ArrayList();
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            pSF.Geometry = xzqGeo;
            pSF.set_OutputSpatialReference(dltbClass.ShapeFieldName, (dltbClass as IGeoDataset).SpatialReference);
            IFeatureCursor pCursor = dltbClass.Search(pSF, false);
            IFeature aFeature = null;
            while ((aFeature = pCursor.NextFeature()) != null)
            {
                arTbs.Add(aFeature);
            }
            OtherHelper.ReleaseComObject(pCursor);
            return arTbs;
        
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {


            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行图斑编号，请稍等...", "处理中");
            wait.Show();

            try
            {

                for (int i = 0; i < this.chkXzqList.CheckedItems.Count; i++)
                {
                    string txt = this.chkXzqList.CheckedItems[i].ToString().Trim();
                    string oldxzqdm = OtherHelper.GetLeftName(txt);
                    string xzqdm = "";
                    if (oldxzqdm.Length > 12)
                    {
                        xzqdm = oldxzqdm.Substring(0, 12);
                    }

                    int  oid =0;
                    int.TryParse(OtherHelper.GetLeftName(txt), out oid);
                    //IFeature aXzqFea = cjdcqClass.GetFeature(oid);   // getAXzq(oldxzqdm);

                    List<IFeature> lstXzq = getXzqs(oldxzqdm);

                    string xzqmc = OtherHelper.GetRightName(txt);
                    wait.SetCaption("正在执行" + xzqmc + "的图斑编号处理...");
                    Application.DoEvents();

                   
                    //if (aXzqFea != null)
                    if (lstXzq.Count>0)
                    {
                        int iStartH = 1; //起始点号

                        IPolygon extent = GeometryHelper.UnionPolygon(lstXzq);
                        //找到所有地类图斑
                        ArrayList arAllTbs = getAllTb(extent); //getAllTb(aXzqFea.ShapeCopy);

                        IComparer comparer = new tbbhCompare();
                        arAllTbs.Sort(comparer);

                        IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
                        pWSEdit.StartEditing(true);
                        pWSEdit.StartEditOperation();
                        try
                        {
                            foreach (IFeature aTb in arAllTbs)
                            {
                                //string tbbh1 = xzqdm + (iStartH).ToString().PadLeft(6, '0');
                                string tbbh = iStartH.ToString();
                                //FeatureHelper.SetFeatureValue(aTb, "TBYBH", tbbh1);
                                FeatureHelper.SetFeatureValue(aTb, "TBBH", tbbh);
                                aTb.Store();
                                iStartH++;
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
                MessageBox.Show("编号完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.cmbLayer.Text.Trim() == "") return;

            this.chkXzqList.Items.Clear();
            Dictionary<string, string> dicXzqdm = new Dictionary<string, string>();


            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            string xzqClassName = OtherHelper.GetLeftName(this.cmbLayer.Text);
            cjdcqClass = pFeaWs.OpenFeatureClass(xzqClassName);

            try
            {

                IFeatureLayer xzqLyr = new FeatureLayerClass();
                xzqLyr.FeatureClass = cjdcqClass;
                //获取所有行政区要素
                IIdentify xzqIndentify = xzqLyr as IIdentify;
                IDataset xzqDS = cjdcqClass as IDataset;
                IGeoDataset xzqGeoDs = xzqDS as IGeoDataset;
                IArray array = xzqIndentify.Identify(xzqGeoDs.Extent);
                for (int i = 0; i < array.Count; i++)
                {
                    IFeatureIdentifyObj idObj = array.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                   // string cunid = pfea.OID.ToString();
                    string xzqmc = FeatureHelper.GetFeatureStringValue(pfea, "ZLDWMC");
                    string xzqdm = FeatureHelper.GetFeatureStringValue(pfea, "ZLDWDM");

                    if (!dicXzqdm.ContainsKey(xzqdm))
                    {
                        dicXzqdm.Add(xzqdm, xzqmc);
                    }
                }

                foreach (KeyValuePair<string, string> aItem in dicXzqdm)
                {
                    int idx = this.chkXzqList.Items.Add(aItem.Key + "|" + aItem.Value);
                    this.chkXzqList.SetItemChecked(idx, true);
                }

            }
            catch (Exception ex)
            {
            }

           

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            

            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行编号，请稍等...", "处理中");
            wait.Show();

            try
            {

                for (int i = 0; i < this.chkXzqList.CheckedItems.Count; i++)
                {
                    string txt = this.chkXzqList.CheckedItems[i].ToString().Trim();
                    string oldxzqdm = OtherHelper.GetLeftName(txt);
                    string xzqdm = "";
                    if (oldxzqdm.Length > 12)
                    {
                        xzqdm = oldxzqdm.Substring(0, 12);
                    }

                    string xzqmc = OtherHelper.GetRightName(txt);
                    wait.SetCaption("正在执行" + xzqmc + "的图斑编号处理...");
                    Application.DoEvents();

                    //IFeature aXzqFea = getAXzq(oldxzqdm);
                    List<IFeature> lstXzq = getXzqs(oldxzqdm);

                   // if (aXzqFea != null)
                    if (lstXzq.Count>0)
                    {
                        int iStartH = 1; //起始点号

                        //找到所有地类图斑
                        IPolygon extent = GeometryHelper.UnionPolygon(lstXzq);

                        //ArrayList arAllTbs = getAllTb(aXzqFea.ShapeCopy);
                        ArrayList arAllTbs = getAllTb(extent);

                        IComparer comparer = new tbbhCompare();
                        arAllTbs.Sort(comparer);

                        IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
                        pWSEdit.StartEditing(true);
                        pWSEdit.StartEditOperation();
                        try
                        {
                            foreach (IFeature aTb in arAllTbs)
                            {
                                string tbbh1 = xzqdm + (iStartH).ToString().PadLeft(6, '0');
                                string tbbh = iStartH.ToString();
                                FeatureHelper.SetFeatureValue(aTb, "TBYBH", tbbh1);
                                FeatureHelper.SetFeatureValue(aTb, "TBBH", tbbh);
                                aTb.Store();
                                iStartH++;
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
                MessageBox.Show("编号完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }
        }

      

        private void simpleButton7_Click(object sender, EventArgs e)
        {

            int fldIdx = dltbClass.Fields.FindField("TBBHYBH");
            if (fldIdx == -1)
            {
                MessageBox.Show("找不到图斑保护预编号字段！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

             DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行图斑保护预编号，请稍等...", "处理中");
            wait.Show();

            try
            {

                for (int i = 0; i < this.chkXzqList.CheckedItems.Count; i++)
                {
                    string txt = this.chkXzqList.CheckedItems[i].ToString().Trim();
                    string oldxzqdm = OtherHelper.GetLeftName(txt);
                    string xzqdm = "";
                    if (oldxzqdm.Length > 12)
                    {
                        xzqdm = oldxzqdm.Substring(0, 12);
                    }

                    string xzqmc = OtherHelper.GetRightName(txt);
                    wait.SetCaption("正在执行" + xzqmc + "的图斑编号处理...");
                    Application.DoEvents();
                    //IFeature aXzqFea = getAXzq(oldxzqdm);
                    List<IFeature> lstXzq = getXzqs(oldxzqdm);

                    if (lstXzq.Count > 0)
                   // if (aXzqFea != null)
                    {
                        //找到所有地类图斑
                        //ArrayList arAllTbs = getAllTb(aXzqFea.ShapeCopy);
                        IPolygon extent = GeometryHelper.UnionPolygon(lstXzq);

                        //ArrayList arAllTbs = getAllTb(aXzqFea.ShapeCopy);
                        ArrayList arAllTbs = getAllTb(extent);

                        IComparer comparer = new bhbhCompare();  //按照图斑保护预编号 这个字符型的进行排序，降序
                        arAllTbs.Sort(comparer);

                        //d得到最大值
                        IFeature maxFeature =  arAllTbs[0] as IFeature;
                        string maxTbybh = FeatureHelper.GetFeatureStringValue(maxFeature, "TBYBH");  //获取图斑预编号的最大值
                        long newBh = 1;
                        bool bOk= long.TryParse(maxTbybh, out newBh);

                        IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
                        pWSEdit.StartEditing(true);
                        pWSEdit.StartEditOperation();
                        try
                        {
                            foreach (IFeature aTb in arAllTbs)
                            {
                                if (FeatureHelper.GetFeatureStringValue(aTb, "TBBHYBH").Trim() == "")
                                {
                                    //如果图斑保护预编号为空，则对 TBYBH从新进行赋值
                                    if (bOk)
                                    {
                                        //在最大值上加1
                                        newBh++;
                                        FeatureHelper.SetFeatureValue(aTb, "TBYBH", newBh.ToString());
                                        aTb.Store();
                                    }
                                    else
                                    {
                                        //没成功得到最大值，说明都是空的，
                                        string tbbh1 = xzqdm + (newBh).ToString().PadLeft(6, '0');
                                        FeatureHelper.SetFeatureValue(aTb, "TBYBH", tbbh1);
                                        aTb.Store();
                                        newBh++;
                                    }
                                }

                                
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
                MessageBox.Show("编号完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (wait != null)
                    wait.Close();
                MessageBox.Show(ex.Message);
            }

        }

      
    }

    public class bhbhCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            IFeature fea1 = x as IFeature;
            IFeature fea2 = y as IFeature;

            string tbbhybh1 = FeatureHelper.GetFeatureStringValue(fea1, "TBYBH");
            string tbbhybh2 = FeatureHelper.GetFeatureStringValue(fea2, "TBYBH");

            long l1, l2;
            long.TryParse(tbbhybh1, out  l1);
            long.TryParse(tbbhybh2, out l2);

            if (l1 > l2)
            {
                return -1;
            }
            else if (l1 == l2)
            {
                return 0;
            }
            else 
            {
                return 1;
            }                


        }
    }

    public class tbbhCompare : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            IFeature fea1 = x as IFeature;
            IFeature fea2 = y as IFeature;

            IPoint pt1 = (fea1.Shape as IArea).Centroid as IPoint;
            IPoint pt2 = (fea2.Shape as IArea).Centroid as IPoint;

            if (pt1.Y > pt2.Y)
            {
                return -1;
            }
            else
                if (pt1.Y == pt2.Y)
                {
                    if (pt1.X < pt2.X)
                        return -1;
                    else
                        if (pt1.X == pt2.X)
                            return 0;
                        else
                            return 1;
                }
                else
                {
                    return 1;
                }


        }
    }

}
