using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

using RuleCheck;

namespace TDDC3D.rulecheck
{
    /// <summary>
    /// 城镇村范围用地专项检查
    /// </summary>
    public class RuleCheck5
    {
        private IWorkspace currWs = null;
        private IEnvelope currEnv = null;
        private DevExpress.XtraEditors.LabelControl lblStatus = null;

        IFeatureClass czcdydClass = null;
        IFeatureClass dltbClass = null;
        public RuleCheck5(IWorkspace _ws, IEnvelope _currGeo, DevExpress.XtraEditors.LabelControl _label)
        {
            this.currWs = _ws;
            this.lblStatus = _label;
            this.currEnv = _currGeo;
            getAllClass();
        }

        private void getAllClass()
        {
            IFeatureWorkspace pFeatureWorkspace = this.currWs as IFeatureWorkspace;
            try
            {
                czcdydClass = pFeatureWorkspace.OpenFeatureClass("CZCDYD");
            }
            catch { }

            try
            {
                dltbClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            }
            catch { }



        }

        private bool existHole(IGeometry geo)
        {
            bool b = false;
            IGeometryBag exteriorRingGeoBag = (geo as IPolygon4).ExteriorRingBag;
            IGeometryCollection extRingEoCol = exteriorRingGeoBag as IGeometryCollection; //获得所有外环
            for (int i = 0; i < extRingEoCol.GeometryCount; i++)
            {
                IGeometry extRingGeometry = extRingEoCol.get_Geometry(i);
                IGeometryCollection geoPolygon = new PolygonClass();

                IGeometryBag interiorRingGeoBag = (geo as IPolygon4).get_InteriorRingBag(extRingGeometry as IRing);
                IGeometryCollection inRingGeomCollection = interiorRingGeoBag as IGeometryCollection;
                if (inRingGeomCollection.GeometryCount>0)
                    b=true;
            }
            return b;
        }

        public void Check51012()
        {
            this.lblStatus.Text = "正在进行城镇村范围内空洞及属性码检查...";
            this.lblStatus.Update();

            //不能有空洞
            IFeatureLayer czcdydLayer = new FeatureLayerClass();
            czcdydLayer.FeatureClass = czcdydClass;
            IIdentify identify = czcdydLayer as IIdentify;
            IArray array = identify.Identify((czcdydClass as IGeoDataset).Extent);
            if (array == null) return;
            try
            {
                for (int i = 0; i < array.Count; i++)
                {
                    IFeatureIdentifyObj idObj = array.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature aFea = pRow.Row as IFeature;
                    if (this.existHole(aFea.Shape))
                    {
                        string sBsm = FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);

                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "城镇村等用地不能有空洞";
                        aError.errorLayer = "CZCDYD";
                        aError.featureId = aFea.OID;
                        string sRuleId = "5101";
                        aError.ruleId = sRuleId;
                        aError.errorLevel = "1";
                        aError.errorType = "城镇村范围专项检查";
                        CheckErrorHelper.InsertAError(aError);

                    }

                    //5102 
                    string czclx = FeatureHelper.GetFeatureStringValue(aFea, "CZCLX");
                    string czcdm = FeatureHelper.GetFeatureStringValue(aFea, "CZCDM");
                    if (czcdm.Length < 19)
                    {
                        //小于19位
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "城镇村代码与城镇村类型不相符";
                        aError.errorLayer = "CZCDYD";
                        aError.featureId = aFea.OID;
                        string sRuleId = "5102";
                        aError.ruleId = sRuleId;
                        aError.errorLevel = "1";
                        aError.errorType = "城镇村范围专项检查";
                        CheckErrorHelper.InsertAError(aError);
                    }
                    else
                    {
                        #region 各种比较
                        bool isError = false;
                        if (czclx == "201A" || czclx == "201")
                        {
                            if (czcdm.Substring(6, 13) != "0000000000000" && czcdm.Substring(0, 6) != "000000")
                            {
                                isError = true;
                            }
                        }
                        else if (czclx == "202" || czclx == "202A")
                        {
                            if (czcdm.Substring(9, 10) != "0000000000" && czcdm.Substring(0, 9) != "000000000")
                            {
                                isError = true;
                            }
                        }
                        else if (czclx == "203" || czclx == "203A" || czclx == "204" || czclx == "205")
                        {
                            //为坐落单位代码
                        }
                        #endregion
                        if (isError)
                        {
                            ACheckErrorObj aError = new ACheckErrorObj();
                            aError.errorDescription = "城镇村代码正确性";
                            aError.errorLayer = "CZCDYD";
                            aError.featureId = aFea.OID;
                            string sRuleId = "5102";
                            aError.ruleId = sRuleId;
                            aError.errorLevel = "1";
                            aError.errorType = "城镇村范围专项检查";
                            CheckErrorHelper.InsertAError(aError);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(czcdydLayer);
            }



        }


        public void Check5104()
        {
            //漏标
            this.lblStatus.Text = "正在进行地类图斑的城镇村属性码漏标检查...";
            this.lblStatus.Update();
            IGeometry fwGeo = GeometryHelper.MergeGeometry(czcdydClass); //城镇村范围
            IRelationalOperator pRO = fwGeo as IRelationalOperator;

            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "(DLBM like '05%') or (DLBM like '06%') or (DLBM like '07%') or (DLBM like '08%') or (DLBM in ('09','1004','1005','1201')) ";
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aFeature = null;
            try
            {
                int icount = 0;
                while ((aFeature = pCursor.NextFeature()) != null)
                {
                    if (pRO.Contains(aFeature.Shape))
                        continue;
                    //如果没有被包含进来，则为漏标
                    string dlbm = FeatureHelper.GetFeatureStringValue(aFeature, "DLBM");
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription =dlbm+ "图斑对应的城镇村属性码漏标";
                    aError.errorLayer = "DLTB";
                    aError.featureId = aFeature.OID;
                    string sRuleId = "5102";
                    aError.ruleId = sRuleId;
                    aError.errorLevel = "2";
                    aError.errorType = "城镇村范围专项检查";
                    CheckErrorHelper.InsertAError(aError);
                    icount++;
                    if (icount > 200)
                    {
                        //太多了，跳出
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }

            

        }

        public void Check5103()
        {
            this.lblStatus.Text = "正在进行城镇村范围与地类图斑属性码一致性检查...";
            this.lblStatus.Update();

            IFeatureLayer czcdydLayer = new FeatureLayerClass();
            czcdydLayer.FeatureClass = czcdydClass;
            IIdentify identify = czcdydLayer as IIdentify;
            IArray array = identify.Identify((czcdydClass as IGeoDataset).Extent);
            if (array == null) return;
            for (int i = 0; i < array.Count; i++)
            {
                IFeatureIdentifyObj idObj = array.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                IFeature aFea = pRow.Row as IFeature;
                string czclx = FeatureHelper.GetFeatureStringValue(aFea, "CZCLX");
                if (czclx == "")
                {
                    continue;
                }
                string sql = "";
               
                
                switch (czclx)
                {
                    case "201A":
                    case "202A":
                    case "203A":
                        sql = "DLBM<>'0601'";                        
                        break;
                    case "204":
                        sql = "DLBM not in  ('0602','0603','1201')";
                        break;
                    case "205":
                        sql = "DLBM<>'09'";
                        break;
                    case "203":
                        sql = "DLBM in ('1001','1002','1006','1101')";
                        break;
                    case "201":
                    case "202":
                        sql = "DLBM='1006'";
                        break;
                }
                //
                ISpatialFilter pQF = new SpatialFilterClass();
                pQF.Geometry = aFea.Shape;
                pQF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                pQF.WhereClause = sql;
                IFeatureCursor pcursor = dltbClass.Search(pQF, true);
                IFeature pFeature = null;
                try
                {
                    int icount = 0;
                    while ((pFeature = pcursor.NextFeature()) != null)
                    {
                        string dlbm = FeatureHelper.GetFeatureStringValue(pFeature, "DLBM");
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "城镇村范围与地类图斑中地类代码不相符-【"+dlbm+"】";
                        aError.errorLayer = "DLTB";
                        aError.featureId = pFeature.OID;
                        string sRuleId = "5102";
                        aError.ruleId = sRuleId;
                        aError.errorLevel = "1";
                        aError.errorType = "城镇村范围专项检查";
                        CheckErrorHelper.InsertAError(aError);
                        icount++;
                        if (icount > 100)
                        {
                            //跳出了，要不太慢
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pcursor);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(czcdydLayer);
                }
            }
        }

    }
}
