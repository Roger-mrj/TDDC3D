using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.ADF;
using RCIS.GISCommon;
using RCIS.Utility;
namespace TDDC3D.zrzy
{
    public partial class JBNTTBTqOptForm : Form
    {
        public JBNTTBTqOptForm()
        {
            InitializeComponent();
        }

        public IMap currMap = null;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void JBNTTBTqOptForm_Load(object sender, EventArgs e)
        {
            this.cmbSrcLayer.Properties.Items.Clear();
            this.cmbTargetlayer.Properties.Items.Clear();

            LayerHelper.LoadLayer2Combox(this.cmbSrcLayer, currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbTargetlayer, currMap, esriGeometryType.esriGeometryPolygon);


            ControlStyleHelper.FindComboxItem(this.cmbSrcLayer, "DLTB", true);
            ControlStyleHelper.FindComboxItem(this.cmbTargetlayer, "YJJBNTTB", true);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.cmbSrcLayer.Text.Trim() == "" || this.cmbTargetlayer.Text.Trim() == "")
                return;

            string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            string jbtnClassName = OtherHelper.GetLeftName(this.cmbTargetlayer.Text);
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在提取数据，请稍等", "请稍等...");
            wait.Show();

            IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWsEdt.StartEditing(false);
            pWsEdt.StartEditOperation();

            IFeatureWorkspace pFeaWs = RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;

            try
            {
                
                IFeatureClass dltbClass = pFeaWs.OpenFeatureClass(dltbClassName);
                IFeatureClass jbntClass = pFeaWs.OpenFeatureClass(jbtnClassName);
                

                using (ComReleaser comReleaser = new ComReleaser())
                {
                    IFeatureCursor insertCursor = jbntClass.Insert(true);
                    comReleaser.ManageLifetime(insertCursor);
                    //从dltb查询
                    IFeatureCursor queryCursor=null;
                    IQueryFilter pQF = null;
                    if (this.radioGFwOpt.SelectedIndex==0)
                    {
                        ISpatialFilter pSF = new SpatialFilterClass();
                        pSF.Geometry=(this.currMap as IActiveView).Extent;
                        pSF.SpatialRel=esriSpatialRelEnum.esriSpatialRelIntersects;
                        pSF.WhereClause="DLBM in ('0101','0102','0103') ";
                        pQF = pSF as IQueryFilter;
                    }
                    else if (this.radioGFwOpt.SelectedIndex == 1)
                    {
                        pQF = new QueryFilterClass();
                        pQF.WhereClause = "DLBM in ('0101','0102','0103') ";
                    }
                    queryCursor = dltbClass.Search(pQF, false);
                    comReleaser.ManageLifetime(queryCursor);

                    //目标要素缓冲
                    IFeatureBuffer pTargetBuffer = jbntClass.CreateFeatureBuffer();
                    comReleaser.ManageLifetime(pTargetBuffer);

                    try
                    {
                        IFeature aFeature = null;
                        int iCount = 0;
                        while ((aFeature = queryCursor.NextFeature()) != null)
                        {
                            IGeometry srcShp = aFeature.ShapeCopy;
                            pTargetBuffer.Shape = srcShp;
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "YSDM", "2005010300");
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "DLBM", FeatureHelper.GetFeatureStringValue(aFeature, "DLBM"));
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "DLMC", FeatureHelper.GetFeatureStringValue(aFeature, "DLMC"));
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "JBNTMJ", FeatureHelper.GetFeatureDoubleValue(aFeature, "TBDLMJ"));
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "GLTBBSM", FeatureHelper.GetFeatureStringValue(aFeature, "BSM"));
                            insertCursor.InsertFeature(pTargetBuffer);

                            iCount++;
                            if (iCount % 200 == 0)
                            {
                                insertCursor.Flush();
                                wait.SetCaption("当前已提取" + iCount + "个图斑...");
                                Application.DoEvents();
                            }

                        }
                        insertCursor.Flush();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ///nnd释放
                        OtherHelper.ReleaseComObject(pQF);
                        OtherHelper.ReleaseComObject(pTargetBuffer);
                        OtherHelper.ReleaseComObject(insertCursor);
                        OtherHelper.ReleaseComObject(queryCursor);
                        
                    }
                    
                }

                pWsEdt.StopEditOperation();
                pWsEdt.StopEditing(true);
                wait.Close();
                MessageBox.Show("提取完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pWsEdt.AbortEditOperation();
                pWsEdt.StopEditing(false);
                if (wait != null)
                {
                    wait.Close();
                }
                MessageBox.Show(ex.Message);
            }
        }
    }
}
