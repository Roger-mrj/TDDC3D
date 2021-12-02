using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Collections;

namespace TDDC3D.gengxin
{
    public partial class FrmBSM : Form
    {
        public FrmBSM()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        private void btnOK_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理", "请稍后");
            IFeatureClass pDLTB = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            IFeatureClass pDLTBGX = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTB, "BSM");
            long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberOrderBy(pDLTBGX, "BSM");
            long maxBsm = 0;
            if (max1 > max2)
                maxBsm = max1;
            else
                maxBsm = max2;
            maxBsm++;
            int num = 0;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "BSM =''";
                IFeatureCursor pUpdateCursor = pDLTBGX.Update(pQF, true);
                comRel.ManageLifetime(pUpdateCursor);
                IFeature pFeature;
                IFeatureLayer pLayer = new FeatureLayerClass();
                pLayer.FeatureClass = pDLTB;
                IIdentify pIdentify = pLayer as IIdentify;
                while ((pFeature = pUpdateCursor.NextFeature()) != null)
                {
                    IRelationalOperator pRel = pFeature.ShapeCopy as IRelationalOperator;
                    //ITopologicalOperator pTop = pFeature.ShapeCopy as ITopologicalOperator;
                    bool b = false;
                    IArray pArry = pIdentify.Identify(pFeature.ShapeCopy);
                    if (pArry != null)
                    {
                        for (int m = 0; m < pArry.Count; m++)
                        {
                            IFeatureIdentifyObj idObj = pArry.get_Element(m) as IFeatureIdentifyObj;
                            IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                            IFeature pFea = pRow.Row as IFeature;
                            if (pRel.Equals(pFea.ShapeCopy))
                            {
                                b = true;
                                pFeature.set_Value(pFeature.Fields.FindField("BSM"), pFea.get_Value(pFea.Fields.FindField("BSM")));
                                pFeature.set_Value(pFeature.Fields.FindField("TBBH"), 0);
                                num++;
                                break;
                            }
                        }
                    }
                    if (b == false)
                    {
                        pFeature.set_Value(pFeature.Fields.FindField("BSM"), maxBsm++);
                        pFeature.set_Value(pFeature.Fields.FindField("TBBH"), 0);
                        num++;
                    }
                    pUpdateCursor.UpdateFeature(pFeature);
                }
                pUpdateCursor.Flush();
            }
            wait.Close();
            MessageBox.Show("完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理", "请稍后");
            IFeatureClass pDLTB = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTB");
            IFeatureClass pDLTBGX = (currWs as IFeatureWorkspace).OpenFeatureClass("DLTBGX");
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = "TBBH='0'";
            ArrayList dwdms = RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pDLTBGX, pQF, "ZLDWDM");
            int num = 0;
            for (int i = 0; i < dwdms.Count; i++)
            {
                string zldwdm = dwdms[i].ToString();
                long max1 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(pDLTB, "TBBH", "ZLDWDM='" + zldwdm + "'");
                long max2 = RCIS.GISCommon.FeatureHelper.GetMaxStringNumberEveryOne(pDLTBGX, "TBBH", "ZLDWDM='" + zldwdm + "'");
                long maxTbbh = 0;
                if (max1 > max2)
                    maxTbbh = max1;
                else
                    maxTbbh = max2;
                maxTbbh++;
                pQF = new QueryFilterClass();
                pQF.WhereClause = "TBBH='0' and ZLDWDM='" + zldwdm + "'";
                IFeatureCursor pFeaCursor = pDLTBGX.Update(pQF, true);
                IFeature pFeature;
                while ((pFeature = pFeaCursor.NextFeature()) != null)
                {
                    pFeature.set_Value(pFeature.Fields.FindField("TBBH"), maxTbbh++.ToString());
                    num++;
                    pFeaCursor.UpdateFeature(pFeature);
                    //RCIS.Utility.OtherHelper.ReleaseComObject(pFeature);
                }
                RCIS.Utility.OtherHelper.ReleaseComObject(pFeaCursor);
            }
            wait.Close();
            MessageBox.Show("完成","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在处理", "请稍后");

            IFeatureWorkspace pFeaWs = RCIS.Global.GlobalEditObject.GlobalWorkspace as IFeatureWorkspace;
            //IFeatureClass pDLTBGXGC = pFeaWs.OpenFeatureClass("DLTBGXGC");
            List<string> bsm = RCIS.GISCommon.FeatureHelper.GetDMMCDicByQueryDef(pFeaWs, "DLTBGXGC", "BGQTBBSM", 18);
            IFeatureClass pDLTBH = pFeaWs.OpenFeatureClass("DLTBH");
            ITopologicalOperator pTop = RCIS.GISCommon.GeometryHelper.MergeGeometry(pDLTBH) as ITopologicalOperator;
            IFeatureClass pDLTB = pFeaWs.OpenFeatureClass("DLTB");
            IFeatureClass pDLTBGX=pFeaWs.OpenFeatureClass("DLTBGX");
            for (int i = 0; i < bsm.Count; i++)
            {
                string tbbsm = bsm[i];
                List<IFeature> pFeatures = RCIS.GISCommon.GetFeaturesHelper.getFeaturesBySql(pDLTB, "BSM='"+tbbsm+"'");
                if (pFeatures.Count > 0)
                {
                    IFeature pFea = pFeatures[0];
                    string tbbh = pFea.get_Value(pFea.Fields.FindField("TBBH")).ToString();
                    ISpatialFilter pSF = new SpatialFilterClass();
                    pSF.Geometry = pFea.ShapeCopy;
                    pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    IFeatureCursor pCursor = pDLTBGX.Update(pSF,true);
                    IFeature feature;
                    while((feature=pCursor.NextFeature())!=null)
                    {
                        if ((pTop.Intersect(feature.ShapeCopy,esriGeometryDimension.esriGeometry2Dimension) as IArea).Area == 0)
                        {
                            feature.set_Value(feature.Fields.FindField("BSM"), tbbsm);
                            feature.set_Value(feature.Fields.FindField("TBBH"), tbbh);
                            pCursor.UpdateFeature(feature);
                            break;
                        }
                        RCIS.Utility.OtherHelper.ReleaseComObject(feature);
                    }
                    pCursor.Flush();
                    RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                }
            }

            wait.Close();
            MessageBox.Show("完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //private Dictionary<string, string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, string valueField)
        //{
        //    Dictionary<string, string> dmmc = new Dictionary<string, string>();
        //    using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
        //    {
        //        IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
        //        comRel.ManageLifetime(pQDef);
        //        pQDef.Tables = tableName + " Group By " + keyField + "," + valueField;
        //        pQDef.SubFields = keyField + "," + valueField;
        //        ICursor pCur = pQDef.Evaluate();
        //        comRel.ManageLifetime(pCur);
        //        IRow pRow;
        //        while ((pRow = pCur.NextRow()) != null)
        //        {
        //            string dm = pRow.get_Value(0).ToString();
        //            //if (dm.Length > 12)
        //            //dm = dm.Substring(0,12);
        //            string mc = pRow.get_Value(1).ToString();
        //            if (!dmmc.Keys.Contains(dm))
        //                dmmc.Add(dm, mc);
        //        }
        //    }
        //    return dmmc;
        //}

    }
}
