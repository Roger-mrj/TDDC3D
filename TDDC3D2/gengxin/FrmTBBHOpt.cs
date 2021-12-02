using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using RCIS.Utility;
using System.Collections;
using ESRI.ArcGIS.Geometry;
//using TDDC3D.datado;

namespace TDDC3D.gengxin
{
    public partial class FrmTBBHOpt : Form
    {
        public IWorkspace currWs = null;
        private IFeatureClass dltbClass = null;
        private IFeatureClass cjdcqClass = null;

        public FrmTBBHOpt()
        {
            InitializeComponent();
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmTBBHOpt_Load(object sender, EventArgs e)
        {
            dltbClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            cjdcqClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass("CJDCQ");

            Dictionary<string,string> dmmc = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(currWs as IFeatureWorkspace, "DLTBGX", "ZLDWDM", "ZLDWMC");
            foreach (KeyValuePair<string, string> aItem in dmmc)
            {
                int idx = this.chkXzqList.Items.Add(aItem.Key + "|" + aItem.Value);
                this.chkXzqList.SetItemChecked(idx, true);
            }
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

                    //int oid = 0;
                    //int.TryParse(OtherHelper.GetLeftName(txt), out oid);
                    ////IFeature aXzqFea = cjdcqClass.GetFeature(oid);   // getAXzq(oldxzqdm);

                    //List<IFeature> lstXzq = getXzqs(oldxzqdm);

                    string xzqmc = OtherHelper.GetRightName(txt);
                    wait.SetCaption("正在执行" + xzqmc + "的图斑编号处理...");
                    Application.DoEvents();

                    //if (aXzqFea != null)
                    //if (lstXzq.Count > 0)
                    //{
                        long iStartH = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne((currWs as IFeatureWorkspace).OpenFeatureClass("DLTB"),"TBBH","ZLDWDM='"+oldxzqdm+"'")+1; //起始点号

                        //IPolygon extent = RCIS.GISCommon.GeometryHelper.UnionPolygon(lstXzq);
                        //找到所有地类图斑
                        ArrayList arAllTbs = getAllTb(oldxzqdm); //getAllTb(aXzqFea.ShapeCopy);

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
                                RCIS.GISCommon.FeatureHelper.SetFeatureValue(aTb, "TBBH", tbbh);
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

                    //}
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

        private List<IFeature> getXzqs(string xzqdm)
        {
            List<IFeature> lst = new List<IFeature>();
            IFeature aFea = null;
            string where = "ZLDWDM='" + xzqdm + "'";
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = where;
            IFeatureCursor cursor = dltbClass.Search(pQF, false);
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

        private ArrayList getAllTb(string dwdm)
        {

            ArrayList arTbs = new ArrayList();
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.WhereClause = "ZLDWDM='" + dwdm + "'";
            //pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            //pSF.Geometry = xzqGeo;
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
    }
}
