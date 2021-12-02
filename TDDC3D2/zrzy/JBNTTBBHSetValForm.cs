using System;
using System.Collections;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;


using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.zrzy
{
    public partial class JBNTTBBHSetValForm : Form
    {
        public JBNTTBBHSetValForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;

        private IFeatureClass xzqClass = null;
        private IFeatureClass JbnttbClass = null;
        private void JBNTTBBHSetValForm_Load(object sender, EventArgs e)
        {
            if (currWs == null)
                return;
            this.chkXzqList.Items.Clear();
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            try
            {
                xzqClass = pFeaWs.OpenFeatureClass("XZQ");
                JbnttbClass = pFeaWs.OpenFeatureClass("YJJBNTTB");

            }
            catch (Exception ex)
            {
                MessageBox.Show("找不到对应要素类。\r\n" + ex.Message);
            }
            
            IFeatureLayer xzqLyr = new FeatureLayerClass();
            xzqLyr.FeatureClass = xzqClass;
            //获取所有行政区要素
            IIdentify xzqIndentify = xzqLyr as IIdentify;
            IDataset xzqDS = xzqClass as IDataset;
            IGeoDataset xzqGeoDs = xzqDS as IGeoDataset;
            IArray arrDltbIDs = xzqIndentify.Identify(xzqGeoDs.Extent);
            for (int i = 0; i < arrDltbIDs.Count; i++)
            {
                IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                IFeature pfea = pRow.Row as IFeature;
                string xzqmc = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");
                string xzqdm = FeatureHelper.GetFeatureStringValue(pfea, "XZQDM");

                int idx = this.chkXzqList.Items.Add(xzqdm + "|" + xzqmc);
                this.chkXzqList.SetItemChecked(idx, true);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkXzqList.Items.Count; i++)
            {
                this.chkXzqList.SetItemChecked(i, true);
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
            return aFea;
        }

        private ArrayList getAllTb(IGeometry xzqGeo)
        {

            ArrayList arTbs = new ArrayList();
            ISpatialFilter pSF = new SpatialFilterClass();
            pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            pSF.Geometry = xzqGeo;
            IFeatureCursor pCursor = JbnttbClass.Search(pSF, false);
            IFeature aFeature = null;
            while ((aFeature = pCursor.NextFeature()) != null)
            {
                arTbs.Add(aFeature);
            }
            OtherHelper.ReleaseComObject(pCursor);
            return arTbs;

        }

        private void btnUnSelect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkXzqList.Items.Count; i++)
            {
                this.chkXzqList.SetItemChecked(i, !this.chkXzqList.GetItemChecked(i));
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在执行编号，请稍等...", "处理中");
            wait.Show();
            
            try
            {
                
                for (int i = 0; i < this.chkXzqList.CheckedItems.Count; i++)
                {
                    string txt = this.chkXzqList.CheckedItems[i].ToString().Trim();
                    string xzqdm = OtherHelper.GetLeftName(txt);
                    string xzqmc = OtherHelper.GetRightName(txt);
                    wait.SetCaption("正在执行" + xzqmc + "的图斑编号处理...");
                    Application.DoEvents();

                    IFeature aXzqFea = getAXzq(xzqdm);
                    if (aXzqFea != null)
                    {
                        int iStartH = 1; //起始点号  //一个村一套
                        //找到所有地类图斑
                        ArrayList arAllTbs = getAllTb(aXzqFea.ShapeCopy);
                        IComparer comparer = new  datado.tbbhCompare();
                        arAllTbs.Sort(comparer);

                        IWorkspaceEdit pWSEdit = this.currWs as IWorkspaceEdit;
                        pWSEdit.StartEditing(true);
                        pWSEdit.StartEditOperation();
                        try
                        {
                            foreach (IFeature aTb in arAllTbs)
                            {
                                string bh = xzqdm + (iStartH.ToString().PadLeft(4, '0'));                               
                                FeatureHelper.SetFeatureValue(aTb, "BHKBH", bh);
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
        
    }
}
