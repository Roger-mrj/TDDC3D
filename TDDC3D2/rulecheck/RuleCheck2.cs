using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace RuleCheck
{
    public class RuleCheck2
    {
        IWorkspace currWs = null;
        private DevExpress.XtraEditors.LabelControl lblStatus = null;

        public RuleCheck2(IWorkspace pws, DevExpress.XtraEditors.LabelControl _label)
        {
            this.currWs = pws;
            this.lblStatus = _label;
        }

        public void Check22()
        {

            this.lblStatus.Text = "开始数学基础检查...";
            this.lblStatus.Update();
            //获取所有数据集
            if (this.currWs == null) return;
            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            IFeatureDataset pDataset= pFeaWs.OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            IGeoDataset pGeoDs = pDataset as IGeoDataset;
            if (pGeoDs != null)
            {
                ISpatialReference pSF = pGeoDs.SpatialReference;
                if (pSF == null)
                {
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.ruleId = "2201";
                    aError.errorDescription = "数据集" + pDataset.Name + "没有空间参考！";
                    aError.errorLevel = "1";
                    aError.errorType = "数学基础";
                    CheckErrorHelper.InsertAError(aError);

                }
                else
                {
                    IProjectedCoordinateSystem prjSR = pSF as IProjectedCoordinateSystem;
                    IGeographicCoordinateSystem gcs = prjSR.GeographicCoordinateSystem;
                    if (!prjSR.Projection.Name.ToUpper().Contains("GAUSS_KRUGER"))
                    {
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.ruleId = "2203";
                        aError.errorDescription = "数据集" + pDataset.Name + "投影方式并非高斯-克吕格投影";
                        aError.errorLevel = "1";
                        aError.errorType = "数学基础";
                        CheckErrorHelper.InsertAError(aError);
                    }
                    if (!gcs.Name.ToUpper().Contains("2000"))
                    {
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.ruleId = "2201";
                        aError.errorDescription = "数据集" + pDataset.Name + "平面坐标没有采用国家2000坐标系";
                        aError.errorLevel = "1";
                        aError.errorType = "数学基础";
                        CheckErrorHelper.InsertAError(aError);
                    }

                }
            }
            //IEnumDataset pEnumDs=this.currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
           // IDataset pDataset = pEnumDs.Next();
            //while (pDataset!=null)
            //{
                
            //    pDataset = pEnumDs.Next();
            //}
            this.lblStatus.Text = "空间数学基础检查完毕。";
        }

    }
}
