using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace TDDC3D.datado
{
    /// <summary>
    /// 基本思路，获取所有图层最大最小值区间，如果当前图层+1 后BSM 不在其他图层区间范围内，则执行，如果在其他图层区间范围内，则当前图层的最大值修改为 所在范围的最大值
    /// 
    /// </summary>
    public partial class BSMWHForm : Form
    {
        public BSMWHForm()
        {
            InitializeComponent();
        }

        public IWorkspace currWs = null;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BSMWHForm_Load(object sender, EventArgs e)
        {
            //获取所有要素类
            IFeatureWorkspace pFeaws = this.currWs as IFeatureWorkspace;
            IFeatureDataset pDS = pFeaws.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            List<IFeatureClass> allFC = DatabaseHelper.getAllFeatureClass(pDS);
            this.chkListFCs.Items.Clear();
            foreach (IFeatureClass aClass in allFC)
            {
                string alias = aClass.AliasName;
                string className = (aClass as IDataset).Name;

                this.chkListFCs.Items.Add(className + "|" + alias, true);
            }
        }

        private long updateBSM1(IFeatureClass pFC, long max)
        {
            int fdx = pFC.FindField("BSM");
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "BSM is NULL";
            IFeatureCursor updateCursor = pFC.Update(pQF, false);
            try
            {

                IFeature feature = null;
                while ((feature = updateCursor.NextFeature()) != null)
                {
                    feature.set_Value(fdx, max++);
                    updateCursor.UpdateFeature(feature);
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }
            return max;
        }

        //BSM 类型改了，此处待定
        private long updateBSM2(IFeatureClass pFC,long max)
        {

        
            int fdx = pFC.FindField("BSM");           
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "BSM<0";
            IFeatureCursor updateCursor = pFC.Update(pQF, false);
            try
            {
                
                IFeature feature = null;
                while ((feature = updateCursor.NextFeature()) != null)
                {
                    feature.set_Value(fdx, max++);
                    updateCursor.UpdateFeature(feature);
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
            }
            return max;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //获取所有要素类的 最大值

            long lMax = 0;
            try
            {
                lblstatus.Text = "正在提取最大值...";
                
                IFeatureWorkspace pFeaws = this.currWs as IFeatureWorkspace;
                for (int i = 0; i < this.chkListFCs.CheckedItemsCount; i++)
                {
                    string className = this.chkListFCs.CheckedItems[i].ToString();
                    className = RCIS.Utility.OtherHelper.GetLeftName(className);
                    IFeatureClass pFC = pFeaws.OpenFeatureClass(className);
                    if (pFC.FindField("BSM") == -1)
                        continue;
                    if (pFC.FeatureCount(null) == 0)
                    {

                        continue;
                    }
                    double dMin, dMax, dSum, dMean = 0;
                    FeatureHelper.StatsFieldValue(pFC, null, "BSM", out dMax, out dMin, out dSum, out dMean);

                    long currMax = (long)dMax;
                    if (currMax > lMax)
                    {
                        lMax = currMax;
                    }

                }
                //最大值；
                lMax++;

                for (int i = 0; i < this.chkListFCs.CheckedItemsCount; i++)
                {
                    string className = this.chkListFCs.CheckedItems[i].ToString();
                    className = RCIS.Utility.OtherHelper.GetLeftName(className);
                    IFeatureClass pFC = pFeaws.OpenFeatureClass(className);
                    lblstatus.Text = "正在维护" + className + "层BSM...";
                    Application.DoEvents();

                    lMax= updateBSM1(pFC, lMax);
                    lMax= updateBSM2(pFC, lMax);
                }
                MessageBox.Show("维护完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkListFCs.Items.Count; i++)
            {
                this.chkListFCs.SetItemChecked(i, true);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.chkListFCs.Items.Count; i++)
            {
                this.chkListFCs.SetItemChecked(i, !this.chkListFCs.GetItemChecked(i));
            }
        }
    }
}
