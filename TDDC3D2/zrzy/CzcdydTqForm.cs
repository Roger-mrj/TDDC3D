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
    public partial class CzcdydTqForm : Form
    {
        public CzcdydTqForm()
        {
            InitializeComponent();
        }

        private void CzcdydTqForm_Load(object sender, EventArgs e)
        {
            this.cmbSrcLayer.Properties.Items.Clear();
            this.cmbTargetlayer.Properties.Items.Clear();

            LayerHelper.LoadLayer2Combox(this.cmbSrcLayer, currMap, esriGeometryType.esriGeometryPolygon);
            LayerHelper.LoadLayer2Combox(this.cmbTargetlayer, currMap, esriGeometryType.esriGeometryPolygon);
        }

        public IMap currMap = null;

        private void btnOk_Click(object sender, EventArgs e)
        {
            //相当于 提取 201，202，203，204，205 ，然后 ，修改城镇村等用地类型 ，城镇村代码，城镇村面积
            if (this.cmbSrcLayer.Text.Trim() == "" || this.cmbTargetlayer.Text.Trim() == "")
                return;

            string dltbClassName = OtherHelper.GetLeftName(this.cmbSrcLayer.Text);
            string czcdydClassName = OtherHelper.GetLeftName(this.cmbTargetlayer.Text);
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在提取数据，请稍等", "请稍等...");
            wait.Show();



            IWorkspaceEdit pWsEdt = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            pWsEdt.StartEditing(false);
            pWsEdt.StartEditOperation();

            try
            {
                IFeatureWorkspace pFeaWs = RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
                IFeatureClass dltbClass = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(currMap, dltbClassName).FeatureClass;// pFeaWs.OpenFeatureClass(dltbClassName);
                IFeatureClass czcdydClass = pFeaWs.OpenFeatureClass(czcdydClassName);
                
                using (ComReleaser comReleaser = new ComReleaser())
                {
                    IFeatureCursor insertCursor = czcdydClass.Insert(true);
                    comReleaser.ManageLifetime(insertCursor);
                    //从dltb查询
                    IFeatureCursor queryCursor = null;
                    IQueryFilter pQF = null;
                    if (this.radioGFwOpt.SelectedIndex == 0)
                    {
                        ISpatialFilter pSF = new SpatialFilterClass();
                        pSF.Geometry = (this.currMap as IActiveView).Extent;
                        pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        pSF.WhereClause = "DLBM in ('201','202','203','204','205') ";
                        pQF = pSF as IQueryFilter;
                    }
                    else if (this.radioGFwOpt.SelectedIndex == 1)
                    {
                        pQF = new QueryFilterClass();
                        pQF.WhereClause = "DLBM in ('201','202','203','204','205') ";
                    }
                    queryCursor = dltbClass.Search(pQF, false);
                    comReleaser.ManageLifetime(queryCursor);

                    //目标要素缓冲
                    IFeatureBuffer pTargetBuffer = czcdydClass.CreateFeatureBuffer();
                    comReleaser.ManageLifetime(pTargetBuffer);

                    try
                    {
                        IFeature aFeature = null;
                        int iCount = 0;
                        while ((aFeature = queryCursor.NextFeature()) != null)
                        {
                            IGeometry srcShp = aFeature.ShapeCopy;
                            pTargetBuffer.Shape = srcShp;
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "YSDM", "2099010300");
                            string dlbm=FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCLX",dlbm );
                            FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCMJ", FeatureHelper.GetFeatureDoubleValue(aFeature, "TBMJ"));
                            string zldwdm = FeatureHelper.GetFeatureStringValue(aFeature, "ZLDWDM");
                            if (dlbm.ToUpper() == "201")
                            {
                                FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCDM", zldwdm.Substring(0, 6));
                            }
                            else if (dlbm.ToUpper() == "202")
                            {
                                FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCDM", zldwdm.Substring(0, 9));
                            }
                            else 
                            {
                                //填坐落单位代码
                                FeatureHelper.SetFeatureBufferValue(pTargetBuffer, "CZCDM", zldwdm);
                            }
                                                        
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
