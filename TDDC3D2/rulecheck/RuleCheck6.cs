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
    public class RuleCheck6
    {
        private IWorkspace currWs = null;
        private IEnvelope currEnv = null;
        private DevExpress.XtraEditors.LabelControl lblStatus = null;

       
        IFeatureClass dltbClass = null;
        public RuleCheck6(IWorkspace _ws, IEnvelope _currGeo, DevExpress.XtraEditors.LabelControl _label)
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
                dltbClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            }
            catch { }            

        }


        private void Check11()
        {
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM in ('0602','0601','1001','1003','0401','0402','0403','0403K')) and ( ZZSXDM <>''  )";
            //如果地类是0602,1001,1003 ，0601，种植属性必须为空  0401,0402,0403 必须为空，02（除0203外）且图斑细化类型为LQYD，必须为空
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                  
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的要素种植属性代码必须为空";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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

        private void Check12()
        {
            IQueryFilter pQF = new QueryFilterClass();
            string 
                whereClause = "(DLBM like '01%') and (  TBXHDM in ('HDGD','HQGD','LQGD','MQGD','SHGD','SMGD') ) and (ZZSXDM not in ('LS','FLS','LYFL'))";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string xhdm=FeatureHelper.GetFeatureStringValue(aErrFea,"TBXHDM");
                    string zzsx=FeatureHelper.GetFeatureStringValue(aErrFea,"ZZSXDM");
                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "耕地的图斑细化代码和种植属性代码填写不统一,细化代码为【"+xhdm+"】，种植属性为【"+zzsx+"】";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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

        private void Check13()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM like '01%') and ( TBXHDM='') and (ZZSXDM not in ('LS','FLS','LYFL','XG','LLJZ','WG'))";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            int icount = 0;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");

                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "耕地的图斑细化代码和种植属性代码填写不统一，细化代码为【"+tbxhdm+"】，种植属性代码为【"+zzsxdm+ "】";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
                    icount++;
                    if (icount > 300)
                        break;
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

        private void Check14()
        {
            //string[] sXhdm = new string[] { "HDGD", "HQGD", "LQGD", "MQGD", "SHGD", "SMGD" };
            //List<string> lstXhdm = new List<string>(sXhdm);
            //string[] sZzsx = new string[] {"LS","FLS","LYFL","XG","LLJZ","WG" };
            //List<string> lstZzsx = new List<string>(sZzsx);
            int icount = 0;
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM like '01%') and (TBXHDM not in ('HDGD','HQGD','LQGD','MQGD','SHGD','SMGD'))  ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = null;
            {
                IFeature aErrFea = null;
                try
                {
                    pCursor = dltbClass.Search(pQF, true);
                    while ((aErrFea = pCursor.NextFeature()) != null)
                    {
                        string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                        string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");

                        if (tbxhdm != "")
                        {
                            //插入错误
                            ACheckErrorObj aError = new ACheckErrorObj();
                            aError.errorDescription = "耕地的图斑细化代码值为【" + tbxhdm + "】超出字典表";
                            aError.errorLayer = "DLTB";
                            string sRuleId = "6101";
                            aError.errorLevel = "2";
                            aError.ruleId = sRuleId;
                            aError.featureId = aErrFea.OID;
                            string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                            int iBsm = -1;
                            int.TryParse(sBsm, out iBsm);
                            aError.featureBSM = iBsm;
                            aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                            CheckErrorHelper.InsertAError(aError);
                            icount++;
                        }
                        if (icount > 600)
                            break;

                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (pCursor != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    }

                }
            }

            pQF = new QueryFilterClass();
            whereClause = "(DLBM like '01%') and  ( ZZSXDM not in ('LS','FLS','LYFL','XG','LLJZ','WG')) ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor2 = null;
            icount = 0;
            IFeature aErrFea2 = null;
            try
            {
                pCursor2 = dltbClass.Search(pQF, true);
                while ((aErrFea2 = pCursor2.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea2, "DLBM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea2, "ZZSXDM");

                    if (zzsxdm != "")
                    {
                        //插入错误
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "耕地的种植属性值为【" + zzsxdm + "】，代码超出字典表";
                        aError.errorLayer = "DLTB";
                        string sRuleId = "6101";
                        aError.errorLevel = "2";
                        aError.ruleId = sRuleId;
                        aError.featureId = aErrFea2.OID;
                        string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea2, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);
                        aError.featureBSM = iBsm;
                        aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                        CheckErrorHelper.InsertAError(aError);
                        icount++;
                    }

                    //if (icount > 600)
                    //    break;

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor2);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }

        }

        private void Check21()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  in ('0201','0202','0204','0201K','0202K','0204K')  ) and (  TBXHDM ='LQYD' ) and (ZZSXDM<>'')";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为"+dlbm+",图斑细化代码为LQYD的要素种植属性代码必须为空";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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
        private void Check22()
        {
           
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(\"DLBM\"  in ('0201','0202','0204','0201K','0202K','0204K')  ) and ( \"TBXHDM\"='' ) and ( \"ZZSXDM\" not in ('JKHF','GCHF','')    )";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");

                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                    //sql语句不起作用
                    if (zzsxdm == "JKHF" || zzsxdm == "GCHF" || zzsxdm == "")
                    {
                        continue;
                    }

                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + ",图斑细化代码空的要素种植属性代码不符合要求";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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

        private void Check23()
        {
            //判断是否超出字典表
            //string[] sXhdm = new string[] { "LQYD" };
            //List<string> lstXhdm = new List<string>(sXhdm);
            //string[] sZzsx = new string[] { "JKHF","GCHF" };
            //List<string> lstZzsx = new List<string>(sZzsx);
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = " (DLBM like '02%') and (ZZSXDM not in ('JKHF','GCHF') )    ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = null;
            
            IFeature aErrFea = null;
            try
            {
                pCursor = dltbClass.Search(pQF, true);
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");
                    if (zzsxdm != "")
                    {
                        //插入错误
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "园地的种植属性代码值为【" + zzsxdm + "】超出字典表.";
                        aError.errorLayer = "DLTB";
                        string sRuleId = "6101";
                        aError.errorLevel = "2";
                        aError.ruleId = sRuleId;
                        aError.featureId = aErrFea.OID;
                        string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);
                        aError.featureBSM = iBsm;
                        aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                        CheckErrorHelper.InsertAError(aError);
                    }
                   


                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
              
            }

            pQF = new QueryFilterClass();
            whereClause = " (DLBM like '02%') and  (TBXHDM<>'LQYD')     ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor2 = null;

            IFeature aErrFea2 = null;
            try
            {
                pCursor2 = dltbClass.Search(pQF, true);
                while ((aErrFea2 = pCursor2.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea2, "DLBM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea2, "TBXHDM");
                   
                    if (tbxhdm != "")
                    {
                        //插入错误
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "园地的图斑细化代码值为【"+tbxhdm+ "】，超出字典表";
                        aError.errorLayer = "DLTB";
                        string sRuleId = "6101";
                        aError.errorLevel = "2";
                        aError.ruleId = sRuleId;
                        aError.featureId = aErrFea2.OID;
                        string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea2, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);
                        aError.featureBSM = iBsm;
                        aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                        CheckErrorHelper.InsertAError(aError);
                    }


                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }

        }


        private void Check31()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM like '03%') and (TBXHDM<>'')";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");

                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的图斑细化代码必须为空";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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

        private void Check32()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  like '03%') and (ZZSXDM   not in ('JKHF','GCHF')) and (ZZSXDM<>'') ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = null;
            IFeature aErrFea = null;

           

            try
            {
                pCursor=dltbClass.Search(pQF, true);
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                    if (zzsxdm=="JKHF" || zzsxdm=="GCHF" || zzsxdm=="")
                    {
                        continue;
                    }

                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的种植属性代码填写不符合要求";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }
        }
        private void Check33()
        {
            //判断是否超出字典表
            
            //string[] sZzsx = new string[] { "JKHF", "GCHF" };
            //List<string> lstZzsx = new List<string>(sZzsx);
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM like '03%') and (ZZSXDM not in ('JKHF','GCHF')) ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                    if (zzsxdm != "")
                    {
                        //插入错误
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "林地的种植属性代码值为【" + zzsxdm + "】超出字典表";
                        aError.errorLayer = "DLTB";
                        string sRuleId = "6101";
                        aError.errorLevel = "2";
                        aError.ruleId = sRuleId;
                        aError.featureId = aErrFea.OID;
                        string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);
                        aError.featureBSM = iBsm;
                        aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                        CheckErrorHelper.InsertAError(aError);
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


        private void Check41()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  like '04%'  ) and (  TBXHDM <>'GCCD'   )   " ;
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            int icount = 0;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");

                    
                    if (tbxhdm != "")
                    {
                        //插入错误
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "地类编码为" + dlbm + "的图斑细化代码填写不符合要求,其值为【" + tbxhdm + "】";
                        aError.errorLayer = "DLTB";
                        string sRuleId = "6101";
                        aError.errorLevel = "2";
                        aError.ruleId = sRuleId;
                        aError.featureId = aErrFea.OID;
                        string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);
                        aError.featureBSM = iBsm;
                        aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                        CheckErrorHelper.InsertAError(aError);
                        icount++;
                        //if (icount > 300)
                        //    break;
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
        private void Check42()
        {

            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  ='0404') and (ZZSXDM<>'GCHF' )    ";
            pQF.WhereClause = whereClause;
            int icout = 0;

            {
                IFeatureCursor pCursor = dltbClass.Search(pQF, true);
                IFeature aErrFea = null;
               
                try
                {
                    while ((aErrFea = pCursor.NextFeature()) != null)
                    {
                        string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                        string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                        if (zzsxdm != "")
                        {
                            //插入错误
                            ACheckErrorObj aError = new ACheckErrorObj();
                            aError.errorDescription = "地类编码为" + dlbm + "种植属性代码填写不规范,值为【" + zzsxdm + "】";
                            aError.errorLayer = "DLTB";
                            string sRuleId = "6101";
                            aError.errorLevel = "2";
                            aError.ruleId = sRuleId;
                            aError.featureId = aErrFea.OID;
                            string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                            int iBsm = -1;
                            int.TryParse(sBsm, out iBsm);
                            aError.featureBSM = iBsm;
                            aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                            CheckErrorHelper.InsertAError(aError);
                            icout++;
                        }

                        //if (icout > 300)
                        //    break;
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }

            }
            pQF = new QueryFilterClass();
            whereClause = "(DLBM  ='0404') and ( TBXHDM<>'GCCD')    ";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor2 = dltbClass.Search(pQF, true);
            IFeature aErrFea2 = null;
            icout = 0;
            try
            {
                while ((aErrFea2 = pCursor2.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea2, "DLBM");

                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea2, "TBXHDM");
                    if (tbxhdm != "")
                    {
                        //插入错误
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "地类编码为" + dlbm + "图斑细化代码填写不规范,值为【" + tbxhdm + "】";
                        aError.errorLayer = "DLTB";
                        string sRuleId = "6101";
                        aError.errorLevel = "2";
                        aError.ruleId = sRuleId;
                        aError.featureId = aErrFea2.OID;
                        string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea2, "BSM");
                        int iBsm = -1;
                        int.TryParse(sBsm, out iBsm);
                        aError.featureBSM = iBsm;
                        aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                        CheckErrorHelper.InsertAError(aError);
                        icout++;
                    }
                    //if (icout > 500)
                    //    break;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor2);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQF);
            }

        }
       

        private void Check51()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  ='1104'  ) and (TBXHDM  <>''   )";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");

                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的图斑细化代码填写必须为空";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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


        private void Check52()
        {
           
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  ='1104'  ) and (  ZZSXDM  not in ('JKHF','GCHF','' )    )";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string zzsxdm = FeatureHelper.GetFeatureStringValue(aErrFea, "ZZSXDM");
                    if (zzsxdm.ToUpper() == "GCHF" || zzsxdm == "JKHF" || zzsxdm == "")
                        continue;
                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的种植属性代码填写不规范";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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


        private void Check61()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  ='0601'  ) and (  TBXHDM  not in ('','HDGY','GTGY','MTGY','SNGY','BLGY','DLGY' )    )";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");

                    if (tbxhdm == "" || tbxhdm == "HDGY" || tbxhdm == "GTGY" || tbxhdm == "MTGY" || tbxhdm == "SNGY" || tbxhdm == "BLGY" || tbxhdm == "DLGY")
                        continue;

                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的图斑细化代码填写不规范";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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

        private void Check62()
        {
            //如果耕地没有图斑细化类型，则种植属性必须为。。。
            IQueryFilter pQF = new QueryFilterClass();
            string whereClause = "(DLBM  in ('0602','1001','1003')  ) and (  TBXHDM  not in ('','FQ' )    )";
            pQF.WhereClause = whereClause;
            IFeatureCursor pCursor = dltbClass.Search(pQF, true);
            IFeature aErrFea = null;
            try
            {
                while ((aErrFea = pCursor.NextFeature()) != null)
                {
                    string dlbm = FeatureHelper.GetFeatureStringValue(aErrFea, "DLBM");
                    string tbxhdm = FeatureHelper.GetFeatureStringValue(aErrFea, "TBXHDM");
                    if (tbxhdm == "" || tbxhdm == "FQ")
                        continue;
                    //插入错误
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "地类编码为" + dlbm + "的图斑细化代码填写不规范";
                    aError.errorLayer = "DLTB";
                    string sRuleId = "6101";
                    aError.errorLevel = "2";
                    aError.ruleId = sRuleId;
                    aError.featureId = aErrFea.OID;
                    string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aErrFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);
                    aError.featureBSM = iBsm;
                    aError.errorType = "图斑细化代码和种植恢复属性标注错误";
                    CheckErrorHelper.InsertAError(aError);
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

        public void Check6101()
        {
            this.lblStatus.Text = "正在进行细化代码种植属性标注检查...";
            this.lblStatus.Update();
            System.Windows.Forms.Application.DoEvents();

           //按照地类编码内容进行检查
            Check11();
            //耕地 如果图斑细化类型 为 。。。。，种植或恢复属性必须为 。。。。
            Check12();
            Check13();
            Check14();

            this.Check21();
            this.Check22();
            this.Check23();

          
            //林地 图斑细化类型必须为空，
            this.Check31();
            this.Check32();

            //0404 图斑细化类型 为GCCD，或者空，恢复了ii下那个为GCHF或者空
            this.Check41();
            this.Check42();

            //1104 图斑细化类型必须为空，种植属性为空或者。。。。
            this.Check51();
            this.Check52();

            this.Check61();
            this.Check62();

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }



    }
}
