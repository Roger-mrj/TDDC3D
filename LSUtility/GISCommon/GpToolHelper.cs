using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Data;
using ESRI.ArcGIS.Geometry;
using System.Threading;

namespace RCIS.GISCommon
{
    public class GpToolHelper
    {
        public static IGPFieldMapping GetFieldMapping(string ExInputPath)
        {
            IGPUtilities mGPUtilities = new GPUtilitiesClass();
            IDETable mDETable = (IDETable)mGPUtilities.MakeDataElement(ExInputPath, null, null);
            IArray mArray = new ArrayClass();
            mArray.Add(mDETable);
            IGPFieldMapping mGPFieldMapping = new GPFieldMappingClass();
            mGPFieldMapping.Initialize(mArray, null);
            //create new fieldmap
            IGPFieldMap mGPFieldMap = new GPFieldMapClass();
            //mGPFieldMap.OutputField = mFC1.Fields.get_Field(1);
            //match field
            int fieldmap_index = mGPFieldMapping.FindFieldMap("BSM");
            IGPFieldMap mGPFNew = mGPFieldMapping.GetFieldMap(fieldmap_index);
            int field_index = mGPFNew.FindInputField(mDETable, "BSM");
            IField mField = mGPFNew.GetField(field_index);
            mGPFieldMap.AddInputField(mDETable, mField, -1, -1);
            mGPFieldMapping.AddFieldMap(mGPFieldMap);
            return mGPFieldMapping;
        }

        public static Boolean Copy(string inputFeatures, string tarFeatures)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.Copy copy = new ESRI.ArcGIS.DataManagementTools.Copy();

                copy.in_data = inputFeatures;
                copy.out_data = tarFeatures;
                gp.Execute(copy, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static Boolean Clip(string inFeatures, string clipFeatures, string outFeatures)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Clip clip = new ESRI.ArcGIS.AnalysisTools.Clip();
                clip.in_features = inFeatures;
                clip.clip_features = clipFeatures;
                clip.out_feature_class = outFeatures;
                gp.Execute(clip, null);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static Boolean Append(string inputFeatures, string tarFeatures)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.Append append = new ESRI.ArcGIS.DataManagementTools.Append();

                append.inputs = inputFeatures;
                append.target = tarFeatures;
                append.schema_type = "NO_TEST";
                gp.Execute(append, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static Boolean RepairGeometry(string inFeatures)
        {
            try
            {
                Geoprocessor geoprocessor = new Geoprocessor();
                geoprocessor.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.RepairGeometry repaireGeometry = new ESRI.ArcGIS.DataManagementTools.RepairGeometry();
                repaireGeometry.in_features = inFeatures;
                geoprocessor.Execute(repaireGeometry, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static IFeatureClass Dissolve(IFeatureClass sourFeatureClass, IWorkspace tarWorkspace, string tarClassName, string[] disFields, Boolean multipart = false)
        {
            if (sourFeatureClass == null || tarWorkspace == null || string.IsNullOrWhiteSpace(tarClassName) || disFields == null || disFields.Count() == 0)
            {
                return null;
            }
            IFeatureWorkspace pFW = tarWorkspace as IFeatureWorkspace;
            if ((tarWorkspace as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, tarClassName])
            {
                IFeatureClass pFC = pFW.OpenFeatureClass(tarClassName);
                IDataset pDataset = pFC as IDataset;
                pDataset.Delete();
            }
            IFeatureClass tarFeatureClass = null;
            try
            {
                //创建目标要素类
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                for (int i = 0; i < sourFeatureClass.Fields.FieldCount; i++)
                {
                    IField pField = sourFeatureClass.Fields.Field[i];
                    if (pField.Type == esriFieldType.esriFieldTypeOID ||
                        pField.Type == esriFieldType.esriFieldTypeGeometry ||
                        disFields.Contains(pField.Name))
                    {
                        pFieldsEdit.AddField(pField);
                    }
                }
                tarFeatureClass = pFW.CreateFeatureClass(tarClassName, pFields, null, null, esriFeatureType.esriFTSimple, sourFeatureClass.ShapeFieldName, null);

                //设置table
                DataTable dt = RCIS.GISCommon.FeatureHelper.GetTableByQueryDef((sourFeatureClass as IDataset).Workspace as IFeatureWorkspace, (sourFeatureClass as IDataset).Name, disFields);
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IFeatureCursor pInsertCursor = tarFeatureClass.Insert(true);
                    comRel.ManageLifetime(pInsertCursor);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<string> sWhere = new List<string>();
                        foreach (string item in disFields)
                        {
                            sWhere.Add(item + " = '" + dt.Rows[i][item].ToString() + "'");
                        }
                        IGeometry pGeo = RCIS.GISCommon.GeometryHelper.MergeGeometry(sourFeatureClass, string.Join(" And ", sWhere.ToArray()));
                        if (multipart)
                        {
                            IFeatureBuffer pFB = tarFeatureClass.CreateFeatureBuffer();
                            pFB.Shape = pGeo;
                            foreach (string item in disFields)
                            {
                                pFB.set_Value(pFB.Fields.FindField(item), dt.Rows[i][item].ToString());
                            }
                            pInsertCursor.InsertFeature(pFB);
                        }
                        else
                        {
                            List<IGeometry> pGs = RCIS.GISCommon.GeometryHelper.MultiGeometryToList(pGeo, (sourFeatureClass as IGeoDataset).SpatialReference);
                            foreach (IGeometry pG in pGs)
                            {
                                IFeatureBuffer pFB = tarFeatureClass.CreateFeatureBuffer();
                                pFB.Shape = pG;
                                foreach (string item in disFields)
                                {
                                    pFB.set_Value(pFB.Fields.FindField(item), dt.Rows[i][item].ToString());
                                }
                                pInsertCursor.InsertFeature(pFB);
                            }
                        }
                    }
                    pInsertCursor.Flush();
                }
                RepairGeometry(tarWorkspace.PathName + "\\" + tarClassName);
            }
            catch
            {
                return null;
            }
            return tarFeatureClass; ;
        }

        //public static Boolean Dissolve(string inFeatures, string outFeatures, string[] disFields)
        //{
        //    Geoprocessor geoprocessor = new Geoprocessor();

        //    try
        //    {
        //        geoprocessor.OverwriteOutput = true;
        //        ESRI.ArcGIS.DataManagementTools.Dissolve dissolve = new ESRI.ArcGIS.DataManagementTools.Dissolve();
        //        dissolve.in_features = inFeatures;
        //        IGpValueTableObject pObject = new GpValueTableObjectClass();//对多个字段进行融合添加
        //        pObject.SetColumns(1);
        //        foreach (string field in disFields)
        //        {
        //            pObject.AddRow(field);
        //        }
        //        dissolve.dissolve_field = pObject;
        //        dissolve.multi_part = "SINGLE_PART";
        //        dissolve.unsplit_lines = "DISSOLVE_LINES";
        //        dissolve.statistics_fields = "";
        //        dissolve.out_feature_class = outFeatures;
        //        geoprocessor.Execute(dissolve, null);
        //    }
        //    catch(Exception ex)
        //    {
        //        string str = "";
        //        for (int i = 0; i < geoprocessor.MessageCount; i++)
        //        {
        //            str += geoprocessor.GetMessage(i);
        //        }
        //        //RCIS.Helper.LogAPI.Debug(str.ToString());
        //        return false;
        //    }
        //    return true;
        //}

        public static Boolean Update(string inFeatures, string updateFeatures, string outFeatures)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;

                ESRI.ArcGIS.AnalysisTools.Update pUpdate = new ESRI.ArcGIS.AnalysisTools.Update();
                pUpdate.in_features = inFeatures;
                pUpdate.update_features = updateFeatures;
                pUpdate.out_feature_class = outFeatures;
                gp.Execute(pUpdate, null);

            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取gp工具执行后的结果
        /// </summary>
        /// <param name="gp"></param>
        /// <returns></returns>
        public static string ReturnMessages(Geoprocessor gp)
        {
            string s = "";
            if (gp.MessageCount > 0)
            {
                for (int Count = 0; Count <= gp.MessageCount - 1; Count++)
                {
                    s += gp.GetMessage(Count);
                    //Console.WriteLine(gp.GetMessage(Count));
                }
            }
            return s;
        }

        //打散：多部件至单部件
        public static bool MultipartToSinglepart(string inFeature, string outFeature)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;

                ESRI.ArcGIS.DataManagementTools.MultipartToSinglepart MultipartToSinglepart = new ESRI.ArcGIS.DataManagementTools.MultipartToSinglepart();
                MultipartToSinglepart.in_features = inFeature;
                MultipartToSinglepart.out_feature_class = outFeature;
                gp.Execute(MultipartToSinglepart, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        //相交
        public static bool Intersect_analysis(string inFeature, string outFeature)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;

                ESRI.ArcGIS.AnalysisTools.Intersect Intersect = new ESRI.ArcGIS.AnalysisTools.Intersect();
                Intersect.in_features = inFeature;
                Intersect.out_feature_class = outFeature;
                gp.Execute(Intersect, null);

            }
            catch
            {
                return false;
            }
            return true;
        }

        //交集制表
        public static bool GP_TabulateIntersection(string ZoneFeatures, string ZoneField, string ClassFeatures, string ClassField, string outTable)
        {
            Geoprocessor geoprocessor = new Geoprocessor();
            geoprocessor.OverwriteOutput = true;
            ESRI.ArcGIS.AnalysisTools.TabulateIntersection TabInter = new ESRI.ArcGIS.AnalysisTools.TabulateIntersection();

            TabInter.in_zone_features = ZoneFeatures;
            TabInter.zone_fields = ZoneField;
            TabInter.in_class_features = ClassFeatures;
            TabInter.class_fields = ClassField;
            TabInter.out_table = outTable;
            try
            {
                geoprocessor.Execute(TabInter, null);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool DeleteFeatures(string inFeature)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.DataManagementTools.DeleteFeatures delete = new ESRI.ArcGIS.DataManagementTools.DeleteFeatures();
                delete.in_features = inFeature;
                gp.Execute(delete, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool Erase_analysis(string inFeature, string eraseFeature, string outFeature)
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.Erase erase = new ESRI.ArcGIS.AnalysisTools.Erase();

                erase.in_features = inFeature;
                erase.erase_features = eraseFeature;
                erase.out_feature_class = outFeature;
                gp.Execute(erase, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool SpatialJoin_analysis(string targetLay, string joinLay, string outLay, string match_option = "INTERSECT")
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.SpatialJoin spatialJoin = new ESRI.ArcGIS.AnalysisTools.SpatialJoin();
                spatialJoin.target_features = targetLay;
                spatialJoin.join_features = joinLay;
                spatialJoin.out_feature_class = outLay;
                //spatialJoin.join_operation = join_operation;
                spatialJoin.match_option = match_option;
                //spatialJoin.join_type = "KEEP_ALL";
                gp.Execute(spatialJoin, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool SpatialJoin_analysis(string targetLay, string joinLay, string outLay, string join_type,string match_option = "INTERSECT")
        {
            try
            {
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;
                ESRI.ArcGIS.AnalysisTools.SpatialJoin spatialJoin = new ESRI.ArcGIS.AnalysisTools.SpatialJoin();
                spatialJoin.target_features = targetLay;
                spatialJoin.join_features = joinLay;
                spatialJoin.out_feature_class = outLay;
                //spatialJoin.join_operation = join_operation;
                spatialJoin.match_option = match_option;
                spatialJoin.join_type = join_type;
                gp.Execute(spatialJoin, null);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool Union_analysis(string inFeature, string outFeature)
        {
            Geoprocessor gp = new Geoprocessor();
            try
            {
                gp.OverwriteOutput = true;

                ESRI.ArcGIS.AnalysisTools.Union union = new ESRI.ArcGIS.AnalysisTools.Union();
                union.in_features = inFeature;
                union.out_feature_class = outFeature;
                union.join_attributes = "ALL";
                IGeoProcessorResult tGPResult = (IGeoProcessorResult)gp.Execute(union, null);

                string strErr = "";
                if (tGPResult.MessageCount > 0)
                {
                    for (int i = 0; i < tGPResult.MessageCount; i++)
                    {
                        strErr += tGPResult.GetMessage(i);
                    }
                }
            }
            catch (Exception ex)
            {
                string str = "";
                if (gp.MessageCount > 0)
                {
                    for (int i = 0; i < gp.MessageCount; i++)
                    {
                        str += gp.GetMessage(i);
                    }
                }
                return false;
            }
            return true;
        }

        public static bool Identity(object inFeatures, object outFeatures, object idenFeatures)
        {
            Geoprocessor gp = new Geoprocessor();
            try
            {
                gp.OverwriteOutput = true;

                ESRI.ArcGIS.AnalysisTools.Identity iden = new ESRI.ArcGIS.AnalysisTools.Identity();
                iden.in_features = inFeatures;
                iden.out_feature_class = outFeatures;
                iden.identity_features = idenFeatures;
                iden.join_attributes = "ALL";
                IGeoProcessorResult tGPResult = (IGeoProcessorResult)gp.Execute(iden, null);

                string strErr = "";
                if (tGPResult.MessageCount > 0)
                {
                    for (int i = 0; i < tGPResult.MessageCount; i++)
                    {
                        strErr += tGPResult.GetMessage(i);
                    }
                }
            }
            catch (Exception ex)
            {
                string str = "";
                if (gp.MessageCount > 0)
                {
                    for (int i = 0; i < gp.MessageCount; i++)
                    {
                        str += gp.GetMessage(i);
                    }
                }
                return false;
            }
            return true;
        }

        AutoResetEvent eventSet = new AutoResetEvent(false);
        /// <summary>
        /// 融合
        /// </summary>
        /// <param name="inFeatures">输入要素</param>
        /// <param name="outFeatures">输出要素</param>
        /// <param name="disFields">融合字段</param>
        /// <param name="multiPart">是否允许多部件存在</param>
        /// <param name="unsplitLines">控制线要素的融合方式。DISSOLVE_LINES—将线融合为单个要素；UNSPLIT_LINES—只有当两条线具有一个公共结束折点时才对线进行融合</param>
        /// <returns></returns>
        public bool Dissolve(object inFeatures, object outFeatures, string[] disFields, string multiPart = "SINGLE_PART", string unsplitLines = "DISSOLVE_LINES")
        {
            bool result = false;
            Geoprocessor geoprocessor = new Geoprocessor();
            geoprocessor.OverwriteOutput = true;
            IGeoProcessorResult tGPResult = null;
            try
            {
                ESRI.ArcGIS.DataManagementTools.Dissolve dissolve = new ESRI.ArcGIS.DataManagementTools.Dissolve();
                dissolve.in_features = inFeatures;
                dissolve.dissolve_field = disFields == null ? null : string.Join(";", disFields);
                dissolve.out_feature_class = outFeatures;
                dissolve.multi_part = multiPart;
                geoprocessor.ProgressChanged += gpChanged;
                eventSet.Reset();
                geoprocessor.ToolExecuted += gpToolExecuted;
                tGPResult = (IGeoProcessorResult)geoprocessor.ExecuteAsync(dissolve);
                eventSet.WaitOne();
                if (tGPResult.Status == esriJobStatus.esriJobSucceeded)
                {
                    result = true;
                }
                else
                {
                    LogGPError(tGPResult);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogHelper.WriteLog("GP工具Dissolve执行失败：" + ex.Message);
                LogGPError(tGPResult);
                LogGPError(geoprocessor);
            }
            finally
            {
                geoprocessor.ProgressChanged -= gpChanged;
                geoprocessor.ToolExecuted -= gpToolExecuted;
            }
            return result;
        }
        public void gpChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressChangedType)
            {
                case (ProgressChangedType.Show):
                    //The tool that is running reports progress or has stopped reporting progress; make the 
                    // progress bar visible if appropriate. 
                    //progressBar.Visible = e.Show;
                    break;
                case (ProgressChangedType.Message):
                    //The application does not use these, since a tool being used reports percentage progress.
                    break;
                case (ProgressChangedType.Percentage):
                    //progressBar.Value = (int)e.ProgressPercentage;
                    break;
                default:
                    throw new ApplicationException("unexpected ProgressChangedEventsArgs.ProgressChangedType");
            }
        }
        private void gpToolExecuted(object sender, ToolExecutedEventArgs e)
        {
            eventSet.Set();
        }

        /// <summary>
        /// 写入GP失败日志
        /// </summary>
        /// <param name="tGPResult">GP执行结果</param>
        public void LogGPError(IGeoProcessorResult tGPResult)
        {
            if (tGPResult == null)
            {
                return;
            }
            string strErr = string.Empty;
            if (tGPResult.MessageCount > 0)
            {
                for (int i = 0; i < tGPResult.MessageCount; i++)
                {
                    strErr += tGPResult.GetMessage(i);
                }
            }
            LogHelper.WriteLog("GP工具执行失败：" + strErr);
        }

        /// <summary>
        /// 写入GP失败日志
        /// </summary>
        /// <param name="gp"></param>
        private void LogGPError(Geoprocessor gp)
        {
            if (gp == null)
            {
                return;
            }
            string str = "";
            if (gp.MessageCount > 0)
            {
                for (int i = 0; i < gp.MessageCount; i++)
                {
                    str += gp.GetMessage(i);
                }
            }
            LogHelper.WriteLog("GP工具执行失败：" + str);
        }
    }
}
