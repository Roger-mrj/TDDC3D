using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;


namespace RuleCheck
{
    public class RuleCheck3
    {
        private ESRI.ArcGIS.Geodatabase.IWorkspace currWs = null;
        private DevExpress.XtraEditors.LabelControl lblStatus = null;

        List<StandTabSchemaObj> lstStandStruct = new List<StandTabSchemaObj>(); //所有图层属性字段标准结构

        private string standMdbFile = RCIS.Global.AppParameters.ConfPath + @"\setup.mdb";

        public RuleCheck3(IWorkspace pWS, DevExpress.XtraEditors.LabelControl _label)
        {            
            this.currWs = pWS;
            this.lblStatus = _label;

            lstStandStruct = TableSchemaRead.GetAllSpatialTab(standMdbFile);
        }

        public  void Check3101()
        {
            if (this.currWs == null) return;

            this.lblStatus.Text = "正在检查图层字段结构...";
            this.lblStatus.Update();
            //3201 字段值是代码的字段取值是否符合要求
            foreach (StandTabSchemaObj aStandObj in lstStandStruct)
            {
                string currTabName = aStandObj.TabName;
                //获取该图层 结构
                Dictionary<string, ATabField> layerFields = TableSchemaRead.GetColumnByLayer(this.currWs as IFeatureWorkspace,
                    currTabName);

                if (layerFields.Count == 0)   //缺少表，
                {
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "缺少要素类" + currTabName;
                    aError.errorLayer = currTabName;
                    string sRuleId = "3101";
                    aError.ruleId = sRuleId;
                    aError.errorLevel = "1";
                    aError.errorType = "图层必备性";
                    CheckErrorHelper.InsertAError(aError);
                    continue;
                }

                //该图层所有字段
                List<ATabField> allStandFields = aStandObj.allFields;
                if (layerFields.Count < allStandFields.Count)
                {
                    ACheckErrorObj aError = new ACheckErrorObj();
                    aError.errorDescription = "要素类" + currTabName + "的字段数量小于标准";
                    aError.errorLayer = currTabName;
                    string sRuleId = "3102";
                    aError.ruleId = sRuleId;
                    aError.errorLevel = "1";
                    aError.errorType = "属性数据结构一致性";
                    CheckErrorHelper.InsertAError(aError);
                    continue;
                }

                foreach (ATabField aStandField in allStandFields)
                {

                    #region 每个字段与标准字段检查
                    //所有标准字段
                    //对于标准表的每个表的字段列表 在 规划表格中查找
                    string fieldName = aStandField.fieldName.Trim();
                    if (!layerFields.ContainsKey(fieldName.ToUpper()))
                    {
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "要素类" + currTabName + "中缺少字段" + fieldName;
                        string sRuleId = "3102";
                        aError.errorLayer = currTabName;
                        aError.ruleId = sRuleId;
                        aError.errorType = "属性数据结构一致性";
                        aError.errorLevel = "1";
                        CheckErrorHelper.InsertAError(aError);
                        continue;
                    }

                    ATabField aLayerField = layerFields[fieldName.ToUpper()];

                    if (aLayerField.fieldType.ToUpper() != aStandField.fieldType.ToUpper())
                    {
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.errorDescription = "要素类" + currTabName + "中字段" + fieldName + "类型不一致";
                        string sRuleId = "3102";
                        aError.ruleId = sRuleId;
                        aError.errorLayer = currTabName;
                        aError.errorLevel = "1";
                        aError.errorType = "属性数据结构一致性";
                        CheckErrorHelper.InsertAError(aError);

                    }

                    #endregion
                }

            }
        }

        public void Check3104() //代码一致性
        {
            if (this.currWs == null)
                return;
           

            foreach (StandTabSchemaObj aStandSchema in lstStandStruct)
            {
                string currLyrName = aStandSchema.TabName;
              
                this.lblStatus.Text = "正在检查"+currLyrName+"图层数据代码一致性...";
                this.lblStatus.Update();

                List<ATabField> lstFields = aStandSchema.allFields;
                foreach (ATabField aField in lstFields)
                {
                   
                    if (aField.relationCode.Trim() == "")
                        continue;
                    //如果存在 字段值为字典表中的值
                    //则取出所有字典表相关内容来
                    //然后对比
                    List<String> lstDms = TableSchemaRead.GetRelationDms(standMdbFile, aField.relationCode);

                    #region  根据所有代码数据形成Sql语句  field not in ('1','2') example
                    StringBuilder sbWhere = new StringBuilder();


                    if (lstDms.Count > 0)
                    {
                        sbWhere.Append(aField.fieldName).Append("  not in ('").Append(lstDms[0]).Append("' ");
                    }
                    for (int i = 1; i < lstDms.Count; i++)
                    {
                        sbWhere.Append(",'").Append(lstDms[i]).Append("' ");
                    }
                    if (lstDms.Count > 0)
                    {
                        //是否允许为空
                        if (aField.fieldIsMust == false)
                        {
                            sbWhere.Append(",''");
                        }
                        sbWhere.Append("  ) ");
                    }
                    string sqlWhere = sbWhere.ToString();

                    
                    #endregion  

                    if (sqlWhere.Trim() == "")
                        continue;
                    if (!aField.fieldIsMust)
                    {
                        sqlWhere += " and  (" + aField.fieldName + " is not null )";
                    }
                    //获取该要素类
                    try
                    {
                        
                        IFeatureClass pClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(currLyrName);
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = sqlWhere;
                        IFeatureCursor pCursor = pClass.Search(pQf, true);
                        IFeature aFea = null;
                        int icount = 0;
                        //2019-3月修改
                        try
                        {
                            while ((aFea = pCursor.NextFeature()) != null)
                            {
                                icount++;
                                ACheckErrorObj aError = new ACheckErrorObj();
                                aError.errorDescription = currLyrName +"字段" + aField.fieldName + "值超出字典表范围" ;
                                aError.errorLayer = currLyrName;
                                string sRuleId = "3104";
                                aError.errorLevel = "2";
                                aError.ruleId = sRuleId;
                                aError.featureId = aFea.OID;
                                string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                                int iBsm = -1;
                                int.TryParse(sBsm, out iBsm);
                                aError.featureBSM = iBsm;
                                aError.errorType = "空间数据代码一致性";
                                CheckErrorHelper.InsertAError(aError);
                                if (icount >100)
                                    break;
                            }
                        }
                        catch { }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                        }



                        //int iCount = pClass.FeatureCount(pQf);
                        //if (iCount > 0)
                        //{
                        //    #region 得到几个值
                        //    System.Collections.ArrayList arValues= RCIS.GISCommon.FeatureHelper.GetUniqueFieldValueByDataStatistics(pClass, pQf, aField.fieldName);
                        //    string stmp = "(";
                        //    foreach (string astr in arValues)
                        //    {
                        //        stmp += astr + " ";
                        //    }
                        //    stmp+=")";
                        //    #endregion

                        //    ACheckErrorObj aError = new ACheckErrorObj();
                        //    aError.errorDescription = currLyrName + "有" + iCount.ToString() + "条数据  字段" + aField.fieldName + "值超出字典表范围"+stmp;
                        //    aError.errorLayer = currLyrName;
                        //    string sRuleId = "3104";
                        //    aError.errorLevel = "2";
                        //    aError.ruleId = sRuleId;
                        //    aError.errorType = "空间数据代码一致性";
                        //    CheckErrorHelper.InsertAError(aError);

                        //}
                    }
                    catch (Exception ex)
                    {
                        //如果该表不存在会跳过的
                    }

                }
            }
        }

        public void Check3105() //是否值域范围一致 
        {
            if (this.currWs == null) return;
            

            foreach (StandTabSchemaObj aObj in lstStandStruct)
            {
                string aTable = aObj.TabName;
                this.lblStatus.Text = "正在检查"+aTable+"图层值域范围...";
                this.lblStatus.Update();
                List<ATabField> lstFields = aObj.allFields;
                foreach (ATabField standField in lstFields)
                {
                    if (standField.fieldClause.Trim() == "")
                        continue;
                    try
                    {
                        IFeatureClass PClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(aTable);
                        string sqlWhere = " not  (" + standField.fieldName + " " + standField.fieldClause;
                        if (standField.fieldClause2.Trim() != "")
                            sqlWhere += " and  " + standField.fieldName + " " + standField.fieldClause2;
                        sqlWhere += " )  ";
                        IQueryFilter pQf = new QueryFilterClass();
                        pQf.WhereClause = sqlWhere;

                        IFeatureCursor pCursor = PClass.Search(pQf, true);
                        IFeature aFea = null;
                        int icount = 0;
                        //2019-3月修改
                        try
                        {
                            while ((aFea = pCursor.NextFeature()) != null)
                            {
                                icount++;
                                ACheckErrorObj aError = new ACheckErrorObj();
                                aError.errorDescription = aTable + "字段" + standField.fieldName + "值超出值域范围";
                                aError.errorLayer = aTable;
                                string sRuleId = "3105";
                                aError.errorLevel = "2";
                                aError.ruleId = sRuleId;
                                aError.featureId = aFea.OID;
                                string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                                int iBsm = -1;
                                int.TryParse(sBsm, out iBsm);
                                aError.featureBSM = iBsm;
                                aError.errorType = "空间数据数值范围符合性";
                                CheckErrorHelper.InsertAError(aError);
                                if (icount > 100)
                                    break;
                            }
                        }
                        catch { }
                        finally
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                        }

                        //int iCount = PClass.FeatureCount(pQf);
                        //if (iCount > 0)
                        //{
                        //    ACheckErrorObj aError = new ACheckErrorObj();
                        //    aError.errorDescription =  aTable + "有" + iCount + "条数据  字段" + standField.fieldName + "值超出值域范围";
                        //    aError.errorLayer = aTable;
                        //    string sRuleId = "3105";
                        //    aError.errorLevel =  "2";
                        //    aError.ruleId = sRuleId;
                        //    aError.errorType = "空间数据数值范围符合性";
                        //    CheckErrorHelper.InsertAError(aError);

                        //}

                    }
                    catch { }

                }
            }
        }

        public void Check3106()  //是否必填
        {
            if (this.currWs == null)
            {
                return;
            }

            
            //对每个标准表
            foreach (StandTabSchemaObj aObj in lstStandStruct)
            {
                string aTable = aObj.TabName;
                this.lblStatus.Text = "正在检查"+aTable+"字段的必填性...";
                this.lblStatus.Update();

                List<ATabField> lstFields = aObj.allFields;
                foreach (ATabField standField in lstFields)
                {
                    if (!standField.fieldIsMust)
                        continue;
                    //如果该选项是必填的
                    //按空间查询找 该字段内容为空的情况
                   

                    string sqlWhere = "";
                    if (standField.fieldType.ToLower() == "char")
                    {
                        sqlWhere = standField.fieldName + " is null or " + standField.fieldName + " ='' ";
                    }
                    else if (standField.fieldType.ToLower() == "int"
                        || standField.fieldType.ToLower() == "float"
                        )
                    {
                        sqlWhere = standField.fieldName + " is null ";
                    }
                    IQueryFilter pQf = new QueryFilterClass();
                    pQf.WhereClause = sqlWhere;
                    IFeatureClass pClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(aTable);
                    IFeatureCursor pCursor = pClass.Search(pQf, true);
                    IFeature aFea = null;
                    int icount = 0;
                    //2019-3月修改
                    try
                    {
                        while ((aFea = pCursor.NextFeature()) != null)
                        {
                            icount++;
                            ACheckErrorObj aError = new ACheckErrorObj();
                            aError.errorDescription = aTable + "必填字段" + standField.fieldName + "值为空";
                            aError.errorLayer = aTable;
                            string sRuleId = "3105";
                            aError.errorLevel = "2";
                            aError.ruleId = sRuleId;
                            aError.featureId = aFea.OID;
                            string sBsm = RCIS.GISCommon.FeatureHelper.GetFeatureStringValue(aFea, "BSM");
                            int iBsm = -1;
                            int.TryParse(sBsm, out iBsm);
                            aError.featureBSM = iBsm;
                            aError.errorType = "空间数据字段必填性";
                            CheckErrorHelper.InsertAError(aError);
                            if (icount > 100)
                                break;
                        }
                    }
                    catch { }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    }
                    //try
                    //{
                    //    IFeatureClass pClass = (this.currWs as IFeatureWorkspace).OpenFeatureClass(aTable);
                    //    int iCount = pClass.FeatureCount(pQf);
                    //    if (iCount > 0)
                    //    {
                    //        ACheckErrorObj aError = new ACheckErrorObj();
                    //        aError.errorDescription = aTable + "有" + iCount + "条数据  必填字段" + standField.fieldName + "值为空";
                    //        aError.errorLayer = aTable;
                    //        string sRuleId = "3106";
                    //        aError.errorLevel =  "2";
                    //        aError.ruleId = sRuleId;
                    //        aError.errorType = "空间数据字段必填性";
                    //        CheckErrorHelper.InsertAError(aError);
                    //    }
                    //}
                    //catch { }


                }



            }
        }

    }

    /// <summary>
    /// 规划表格的 标准字段对象
    /// </summary>
    public class StandTabSchemaObj
    {

        /// <summary>
        /// 表名称
        /// </summary>
        public string TabName = "";
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<ATabField> allFields = new List<ATabField>();



    }

    public class ATabField
    {
        public string fieldName = "";
        public string fieldType = "";
        public int fieldLength = 0;
        public int fieldPrecision = 0;
        public string fieldClause = "";
        public string fieldClause2 = "";
        public string relationCode = "";
        public bool fieldIsMust = true;
    }

}
