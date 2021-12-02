using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using RCIS.Utility;
using ESRI.ArcGIS.esriSystem;
using System.Collections;
using ESRI.ArcGIS.Geometry;
using System.Data;

namespace RCIS.GISCommon
{
    public class FeatureHelper
    {
        public static List<string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, int leftLen,string where="")
        {
            List<string> dms = new List<string>();
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
                comRel.ManageLifetime(pQDef);
                pQDef.Tables = tableName + " Group By " + keyField;
                pQDef.SubFields = keyField;
                pQDef.WhereClause = where;
                ICursor pCur = pQDef.Evaluate();
                comRel.ManageLifetime(pCur);
                IRow pRow;
                while ((pRow = pCur.NextRow()) != null)
                {
                    string dm = pRow.get_Value(0).ToString().Substring(0, leftLen);
                    if (!dms.Contains(dm)) dms.Add(dm);
                }
            }
            return dms;
        }

        public static DataTable GetTableByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string[] fieldNames)
        {
            DataTable dt = new DataTable();
            try
            {
                foreach (string item in fieldNames)
                {
                    dt.Columns.Add(item);
                }
                using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
                {
                    IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
                    comRel.ManageLifetime(pQDef);
                    pQDef.Tables = tableName + " Group By " + string.Join(",", fieldNames);
                    pQDef.SubFields = string.Join(",", fieldNames);
                    ICursor pCur = pQDef.Evaluate();
                    comRel.ManageLifetime(pCur);
                    IRow pRow;
                    while ((pRow = pCur.NextRow()) != null)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (string item in fieldNames)
                        {
                            dr[item] = pRow.get_Value(pRow.Fields.FindField(item));
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }

        public static Dictionary<string, string> GetDMMCDicByQueryDef(IFeatureWorkspace pFeatureWorkspace, string tableName, string keyField, string valueField)
        {
            Dictionary<string, string> dmmc = new Dictionary<string, string>();
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IQueryDef pQDef = pFeatureWorkspace.CreateQueryDef();
                comRel.ManageLifetime(pQDef);
                pQDef.Tables = tableName + " Group By " + keyField + "," + valueField;
                pQDef.SubFields = keyField + "," + valueField;
                ICursor pCur = pQDef.Evaluate();
                comRel.ManageLifetime(pCur);
                IRow pRow;
                while ((pRow = pCur.NextRow()) != null)
                {
                    string dm = pRow.get_Value(0).ToString();
                    string mc = pRow.get_Value(1).ToString();
                    dmmc.Add(dm, mc);
                }
            }
            return dmmc;
        }
        /// <summary>
        /// 获取 字符型 字段值
        /// </summary>
        /// <param name="pFea"></param>
        /// <param name="pField"></param>
        /// <returns></returns>
        public static string GetFeatureStringValue(IFeature pFea, string pField)
        {
            object oo = GetRowValue(pFea as IRow, pField);
            return oo.ToString();
        }

        public static double GetFeatureDoubleValue(IFeature pFea, string pField)
        {
            object oo = GetRowValue(pFea as IRow, pField);
            return TextHelper.ParseDouble(oo.ToString(), 0);

        }


        /// <summary>
        /// 拷贝一个要素 属性
        /// </summary>
        /// <param name="pSrcFea"></param>
        /// <param name="pDestFea"></param>
        public static void CopyFeature(IFeature pSrcFea, IFeature pDestFea)
        {
            CopyFeature(pSrcFea, pDestFea, true);
        }
        public static void CopyFeature(IFeature pSrcFea, IFeature pDestFea, bool pOverwrite)
        {
            try
            {
                IFeatureClass aSrcClass = pSrcFea.Table as IFeatureClass;
                IFeatureClass aDestClass = pDestFea.Table as IFeatureClass;
                int fldCount = pSrcFea.Fields.FieldCount;
                for (int fi = 0; fi < fldCount; fi++)
                {
                    IField aSrcFld = aSrcClass.Fields.get_Field(fi);
                    #region 判断源字段是否需要处理
                    if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                        || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                        || aSrcFld == aSrcClass.LengthField
                        || aSrcFld == aSrcClass.AreaField)
                    {
                        continue;
                    }
                    string aSrcFldName = aSrcFld.Name.ToUpper();
                    #endregion
                    int destFI = aDestClass.Fields.FindField(aSrcFldName);
                    #region 查找目标字段
                    if (destFI >= 0)
                    {
                        IField aDestFld = aDestClass.Fields.get_Field(destFI);
                        if (aDestFld.Type == esriFieldType.esriFieldTypeOID
                            || aDestFld.Type == esriFieldType.esriFieldTypeGeometry
                            || aDestFld == aDestClass.LengthField
                            || aDestFld == aDestClass.AreaField)
                        {
                            continue;
                        }
                        object aSrcObj = pSrcFea.get_Value(fi);

                        if (pOverwrite)
                        {
                            if (aSrcObj == null || aSrcObj is DBNull)
                            {
                                aSrcObj = null ;
                            }
                            if (aDestFld.CheckValue(aSrcObj))
                            {
                                pDestFea.set_Value(destFI, aSrcObj);
                            }
                            else
                            {
                                //如果类型不一致，强行赋值试试
                                try
                                {
                                    pDestFea.set_Value(destFI, aSrcObj);
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            object aDestOriObj = pDestFea.get_Value(destFI);
                            if (aDestOriObj == null || aDestOriObj is DBNull)
                            {
                                if (aSrcObj == null || aSrcObj is DBNull)
                                    aSrcObj = "";
                                if (aDestFld.CheckValue(aSrcObj))
                                {
                                    pDestFea.set_Value(destFI, aSrcObj);
                                }
                            }
                        }
                    }
                    #endregion
                }
                pDestFea.Store();
            }
            catch (Exception ex)
            {
            }
        }
        public static void CreateFieldMap(ITable pSrcTable, ITable pDestTable
            , out List<int> pSrcField, out List<int> pDestField)
        {
            pSrcField = new List<int>();
            pDestField = new List<int>();
            int fldCount = pSrcTable.Fields.FieldCount;
            IField aSrcLenFld = null;
            try
            {
                if (pSrcTable is IFeatureClass) aSrcLenFld = (pSrcTable as IFeatureClass).LengthField;
            }
            catch (Exception ex) { }
            IField aSrcAreaFld = null;
            try
            {
                if (pSrcTable is IFeatureClass) aSrcAreaFld = (pSrcTable as IFeatureClass).AreaField;
            }
            catch (Exception ex) { }
            IField aDestLenFld = null;
            try
            {
                if (pDestTable is IFeatureClass) aDestLenFld = (pDestTable as IFeatureClass).LengthField;
            }
            catch (Exception ex) { }
            IField aDestAreaFld = null;
            try
            {
                if (pDestTable is IFeatureClass) aDestAreaFld = (pDestTable as IFeatureClass).AreaField;
            }
            catch (Exception ex) { }
            for (int fi = 0; fi < fldCount; fi++)
            {
                IField aSrcFld = pSrcTable.Fields.get_Field(fi);
                #region 判断源字段是否需要处理
                if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                    || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                    || aSrcFld == aSrcLenFld
                    || aSrcFld == aSrcAreaFld)
                {
                    continue;
                }
                string aSrcFldName = aSrcFld.Name.ToUpper();
                #endregion
                int destFI = pDestTable.Fields.FindField(aSrcFldName);
                #region 查找目标字段
                if (destFI >= 0)
                {
                    IField aDestFld = pDestTable.Fields.get_Field(destFI);
                    if (aDestFld.Type == esriFieldType.esriFieldTypeOID
                        || aDestFld.Type == esriFieldType.esriFieldTypeGeometry
                        || aDestFld == aDestLenFld
                        || aDestFld == aDestAreaFld)
                    {
                        continue;
                    }
                    pSrcField.Add(fi);
                    pDestField.Add(destFI);
                }
                #endregion
            }
        }

        /// <summary>
        /// 这个方法自己控制开始和结束编辑
        /// </summary>
        /// <param name="pSrcTable"></param>
        /// <param name="pDestTable"></param>
        public static void CopyTable(ITable pSrcTable, ITable pDestTable)
        {
            try
            {
                if (pSrcTable == null) return;
                if (pDestTable == null) return;
                List<int> aSrcFields = null;
                List<int> aDestFields = null;
                FeatureHelper.CreateFieldMap(pSrcTable
                   , pDestTable, out aSrcFields, out aDestFields);
                ICursor aCursor = pSrcTable.Search(null, true);
                int aObjCount = pSrcTable.RowCount(null);
                if (aObjCount > 0)
                {
                    int aStep = aObjCount / 10 + 1;
                    IWorkspaceEdit aWKEdit = (pDestTable as IDataset).Workspace as IWorkspaceEdit;
                    aWKEdit.StartEditing(false);
                    aWKEdit.StartEditOperation();
                    IRow aSrcObj = aCursor.NextRow();
                    int aOrder = 1;
                    while (aSrcObj != null)
                    {
                        try
                        {
                            if (aStep >= 1000
                                && (aOrder++ % aStep == 0))
                            {
                                aWKEdit.StopEditOperation();
                                aWKEdit.StopEditing(true);
                                aWKEdit.StartEditing(false);
                                aWKEdit.StartEditOperation();
                            }
                            IRow aNewObj = pDestTable.CreateRow();
                            FeatureHelper.CopyRow(aSrcObj, aNewObj, aSrcFields, aDestFields);
                            if (aSrcObj is IFeature && aNewObj is IFeature)
                            {
                                (aNewObj as IFeature).Shape = (aSrcObj as IFeature).ShapeCopy;
                            }
                            aNewObj.Store();
                        }
                        catch (Exception ex) { }
                        aSrcObj = aCursor.NextRow();
                        aWKEdit.StopEditOperation();
                        aWKEdit.StopEditing(true);
                    }
                    Marshal.ReleaseComObject(aCursor);
                }
            }
            catch (Exception ex) { }
        }



        public static void CopyRowFeatureBuffer(IFeature pSrcFea, IFeatureBuffer pFeatureBuffer,
            List<int> pSrcField, List<int> pDestField)
        {
            for (int ii = 0; ii < pSrcField.Count; ii++)
            {
                int aSrcIndex = pSrcField[ii];
                int aDestIndex = pDestField[ii];
                if (aSrcIndex >= 0 && aDestIndex >= 0)
                {
                    try
                    {
                        IField destFld = pFeatureBuffer.Fields.get_Field(aDestIndex);
                        if (destFld.Type==esriFieldType.esriFieldTypeGeometry
                            || destFld.Type==esriFieldType.esriFieldTypeGlobalID
                            || destFld.Type==esriFieldType.esriFieldTypeGUID
                            || destFld.Type==esriFieldType.esriFieldTypeOID
                        )
                        continue;

                        object ov = pSrcFea.get_Value(aSrcIndex);
                        if (ov != null
                            && !(ov is DBNull)
                           )
                        {
                            if ( typeof(string).Equals(ov.GetType()))
                            {
                                
                                
                                string sOv = ov.ToString();
                                if (sOv.Length>destFld.Length)
                                {
                                    sOv=sOv.Substring(0,destFld.Length);
                                }
                               
                                pFeatureBuffer.set_Value(aDestIndex,sOv);
                            }
                            else
                            {

                                pFeatureBuffer.set_Value(aDestIndex, ov);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string exMSG = ex.Message;
                    }
                }
            }
        }
        /// <summary>
        /// 在大量复制数据的时候使用这个方法.
        /// </summary>
        /// <param name="pSrcFea"></param>
        /// <param name="pDestFea"></param>
        /// <param name="pSrcField"></param>
        /// <param name="pDestField"></param>
        public static void CopyRow(IRow pSrcFea, IRow pDestFea
            , List<int> pSrcField, List<int> pDestField)
        {
            for (int ii = 0; ii < pSrcField.Count; ii++)
            {
                int aSrcIndex = pSrcField[ii];
                int aDestIndex = pDestField[ii];
                if (aSrcIndex >= 0 && aDestIndex >= 0)
                {
                    try
                    {
                        object ov = pSrcFea.get_Value(aSrcIndex);
                        IField aDestFld = pDestFea.Fields.get_Field(aDestIndex);
                        if (ov != null
                            && !(ov is DBNull)
                            && aDestFld.CheckValue(ov))
                        {
                            pDestFea.set_Value(aDestIndex, ov);
                        }
                    }
                    catch (Exception ex)
                    {
                        string exMSG = ex.Message;
                    }
                }
            }
        }
        public static void CopyRow(IRow pSrcFea, IRow pDestFea)
        {
            try
            {
                ITable aSrcClass = pSrcFea.Table;
                ITable aDestClass = pDestFea.Table;
                IField aSrcLenFld = null, aSrcAreaFld = null, aDestLenFld = null, aDestAreaFld = null;
                if (aSrcClass is IFeatureClass)
                {
                    aSrcLenFld = (aSrcClass as IFeatureClass).LengthField;
                    aSrcAreaFld = (aSrcClass as IFeatureClass).AreaField;
                }
                if (aDestClass is IFeatureClass)
                {
                    aDestLenFld = (aDestClass as IFeatureClass).LengthField;
                    aDestAreaFld = (aDestClass as IFeatureClass).AreaField;
                }
                int fldCount = pSrcFea.Fields.FieldCount;
                for (int fi = 0; fi < fldCount; fi++)
                {
                    IField aSrcFld = aSrcClass.Fields.get_Field(fi);
                    #region 判断源字段是否需要处理
                    if (aSrcFld.Type == esriFieldType.esriFieldTypeOID
                        || aSrcFld.Type == esriFieldType.esriFieldTypeGeometry
                        || aSrcFld == aSrcLenFld
                        || aSrcFld == aSrcAreaFld)
                    {
                        continue;
                    }
                    string aSrcFldName = aSrcFld.Name.ToUpper();
                    #endregion
                    object aSrcObj = pSrcFea.get_Value(fi);
                    if (aSrcObj != null || (aSrcObj is DBNull))
                    {
                        int destFI = aDestClass.Fields.FindField(aSrcFldName);
                        #region 查找目标字段
                        if (destFI >= 0)
                        {
                            IField aDestFld = aDestClass.Fields.get_Field(destFI);
                            if (aDestFld.Type == esriFieldType.esriFieldTypeOID
                                || aDestFld.Type == esriFieldType.esriFieldTypeGeometry
                                || aDestFld == aDestLenFld
                                || aDestFld == aDestAreaFld)
                            {
                                continue;
                            }
                            if (aSrcObj != null
                                && !(aSrcObj is DBNull)
                                && aDestFld.CheckValue(aSrcObj))
                            {
                                pDestFea.set_Value(destFI, aSrcObj);
                            }
                        }
                        #endregion
                    }
                }
                pDestFea.Store();
            }
            catch (Exception ex)
            {
            }
        }

        public static void SetFeatureValue(IFeature pFea, string pField, object pValue)
        {
            
            SetRowValue(pFea as IRow, pField, pValue);
        }
        public static object GetFeatureValue(IFeature pFea, string pField)
        {
            return GetRowValue(pFea as IRow, pField);
        }

        public static void SetRowValue(IRow pRow, string pField, object pValue)
        {
            if (pRow == null
                || pField == null) return;
            int fldIndex = pRow.Fields.FindField(pField);
            if (fldIndex >= 0)
            {
                if (pValue == null || pValue is DBNull) pValue = "";
                IField aFld = pRow.Fields.get_Field(fldIndex);
                if (aFld.CheckValue(pValue))
                {
                    pRow.set_Value(fldIndex, pValue);
                }
                else
                {
                    try { pRow.set_Value(fldIndex, pValue); }
                    catch { }
                }
            }
        }
        public static object GetRowValue(IRow pRow, string pField)
        {
            if (pRow == null
                || pField == null) return "";
            int fldIndex = pRow.Fields.FindField(pField);
            if (fldIndex >= 0)
            {
                object rObj = pRow.get_Value(fldIndex);
                if (rObj == null || rObj is DBNull) rObj = "";
                return rObj;
            }
            return "";
        }

        public static long GetMaxStringNumberEveryOne(IFeatureClass pFeatureClass, string sFieldName, string sWhere = "")
        {
            long maxID = 0;
            try
            {
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = sWhere;
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(string.IsNullOrWhiteSpace(sWhere) ? null : pQF, true);
                IDataStatistics pDataStati = new DataStatisticsClass();
                pDataStati.Field = sFieldName;
                pDataStati.Cursor = (ICursor)pFeatureCursor;

                IEnumerator pEnumerator = pDataStati.UniqueValues;
                pEnumerator.Reset();
                while (pEnumerator.MoveNext())
                {
                    object pObj = pEnumerator.Current;
                    long ID;
                    long.TryParse(pObj.ToString(), out ID);
                    if (ID > maxID) maxID = ID;
                }
                OtherHelper.ReleaseComObject(pFeatureCursor);
            }
            catch
            { }
            return maxID;
        }

        public static long GetMaxStringNumberOrderBy(IFeatureClass pFeatureClass, string sFieldName)
        {
            long maxID = 0;
            try
            {
                ITableSort pTableSort = new TableSortClass();
                pTableSort.Table = pFeatureClass as ITable;
                pTableSort.Fields = sFieldName;
                pTableSort.set_Ascending(sFieldName, false);
                pTableSort.Sort(null);
                ICursor cursor = pTableSort.Rows;
                IRow pRow = cursor.NextRow();
                long.TryParse(pRow.get_Value(pFeatureClass.FindField(sFieldName)).ToString(), out maxID);
            }
            catch
            { }
            return maxID;
        }

        public static long GetMaxStringNumberOrderBy(IFeatureClass pFeatureClass, string sFieldName, string sWhere = "")
        {
            long maxID = 0;
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                if (!string.IsNullOrWhiteSpace(sWhere)) pQueryFilter.WhereClause = sWhere;
                IQueryFilterDefinition queryFilterDef = (IQueryFilterDefinition)pQueryFilter;
                queryFilterDef.PostfixClause = "ORDER BY " + sFieldName + " DESC";
                IFeatureCursor pFeaCursor = pFeatureClass.Search(pQueryFilter, true);
                IFeature pF = pFeaCursor.NextFeature();
                long.TryParse(pF.get_Value(pFeatureClass.FindField(sFieldName)).ToString(), out maxID);
            }
            catch
            { }
            return maxID;
        }

        public static string GetMaxStringOrderBy(IFeatureClass pFeatureClass, string sFieldName, string sWhere = "")
        {
            string maxID = "";
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                if (!string.IsNullOrWhiteSpace(sWhere)) pQueryFilter.WhereClause = sWhere;
                IQueryFilterDefinition queryFilterDef = (IQueryFilterDefinition)pQueryFilter;
                queryFilterDef.PostfixClause = "ORDER BY " + sFieldName + " DESC";
                IFeatureCursor pFeaCursor = pFeatureClass.Search(pQueryFilter, true);
                IFeature pF = pFeaCursor.NextFeature();
                maxID=pF.get_Value(pFeatureClass.FindField(sFieldName)).ToString();
            }
            catch
            { }
            return maxID;
        }

        public static Boolean UpdateFieldValues(ITable pTable, string colName, object colValue, string sWhereClause = "")
        {
            try
            {
                int nIndex = pTable.FindField(colName);
                if (nIndex == -1) return false;

                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = sWhereClause;
                pFilter.SubFields = colName;

                IRowBuffer pBuffer = pTable.CreateRowBuffer();
                pBuffer.set_Value(nIndex, colValue);
                pTable.UpdateSearchedRows(pFilter, pBuffer);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        /// <summary>
        /// 获取唯一值
        /// </summary>
        /// <param name="pFC"></param>
        /// <param name="pQF"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static ArrayList GetUniqueFieldValueByDataStatistics(IFeatureClass pFC,IQueryFilter pQF, string fieldName)
        {
            ArrayList arrValues = new ArrayList();
            try
            {
                IFeatureCursor pFeatureCursor = pFC.Search(pQF, true);
                IDataStatistics pDataStati = new DataStatisticsClass();
                pDataStati.Field = fieldName;
                pDataStati.Cursor = (ICursor)pFeatureCursor;

                IEnumerator pEnumerator = pDataStati.UniqueValues;
                pEnumerator.Reset();
                while (pEnumerator.MoveNext())
                {
                    object pObj = pEnumerator.Current;
                    arrValues.Add(pObj.ToString());
                }

                arrValues.Sort();
                Marshal.ReleaseComObject(pFeatureCursor);
            }
            catch (Exception ex)
            {
            }
            return arrValues;
        }

        public static ArrayList GetUniqueFieldValueByDataStatistics(IFeatureClass pFC, IQueryFilter pQF, string fieldName,int length)
        {
            ArrayList arrValues = new ArrayList();
            try
            {
                IFeatureCursor pFeatureCursor = pFC.Search(pQF, true);
                IDataStatistics pDataStati = new DataStatisticsClass();
                pDataStati.Field = fieldName;
                pDataStati.Cursor = (ICursor)pFeatureCursor;

                IEnumerator pEnumerator = pDataStati.UniqueValues;
                pEnumerator.Reset();
                while (pEnumerator.MoveNext())
                {
                    object pObj = pEnumerator.Current;
                    if (pObj.ToString().Length > length && !arrValues.Contains(pObj.ToString().Substring(0,length)))
                        arrValues.Add(pObj.ToString().Substring(0, length));
                }
                arrValues.Sort();
                Marshal.ReleaseComObject(pFeatureCursor);
            }
            catch (Exception ex)
            {
            }
            return arrValues;
        }

        public static ArrayList GetUniqueFieldValueByDataStatistics(ITable pFC, IQueryFilter pQF, string fieldName)
        {
            ArrayList arrValues = new ArrayList();
            try
            {
                ICursor pFeatureCursor = pFC.Search(pQF, true);
                IDataStatistics pDataStati = new DataStatisticsClass();
                pDataStati.Field = fieldName;
                pDataStati.Cursor = (ICursor)pFeatureCursor;

                IEnumerator pEnumerator = pDataStati.UniqueValues;
                pEnumerator.Reset();
                while (pEnumerator.MoveNext())
                {
                    object pObj = pEnumerator.Current;
                    arrValues.Add(pObj.ToString());
                }

                arrValues.Sort();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            catch (Exception ex)
            {
            }
            return arrValues;
        }

        /// <summary>
        /// 获取唯一值
        /// </summary>
        /// <param name="pFC"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static ArrayList GetUniqueFieldValueByDataStatistics(IFeatureClass pFC, string fieldName, IGeometry geo)
        {
            ArrayList arrValues = new ArrayList();
            try
            {
                ISpatialFilter pQueryFilter = new SpatialFilterClass();
                pQueryFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pQueryFilter.Geometry = geo;
                IFeatureCursor pFeatureCursor = null;
                pQueryFilter.SubFields = fieldName;
                pFeatureCursor = pFC.Search(pQueryFilter, true);

                IDataStatistics pDataStati = new DataStatisticsClass();
                pDataStati.Field = fieldName;
                pDataStati.Cursor = (ICursor)pFeatureCursor;

                IEnumerator pEnumerator = pDataStati.UniqueValues;
                pEnumerator.Reset();
                while (pEnumerator.MoveNext())
                {
                    object pObj = pEnumerator.Current;
                    arrValues.Add(pObj.ToString());
                }

                arrValues.Sort();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQueryFilter);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            catch (Exception ex)
            {
            }
            return arrValues;
        }


        /// <summary>
        /// 获取一个要素类所有字段
        /// </summary>
        /// <param name="pFC"></param>
        /// <returns></returns>
        public static List<IField> getAllFld(IFeatureClass pFC)
        {
            List<IField> lst = new List<IField>();
            for (int i = 0; i < pFC.Fields.FieldCount; i++)
            {
                IField pFld = pFC.Fields.get_Field(i);
                if (pFld.Type == esriFieldType.esriFieldTypeGeometry
                    || pFld.Type == esriFieldType.esriFieldTypeGlobalID
                    || pFld.Type == esriFieldType.esriFieldTypeGUID
                    || pFld.Type == esriFieldType.esriFieldTypeOID
                    || pFld.Type == esriFieldType.esriFieldTypeRaster)
                {
                    continue;
                }
                lst.Add(pFld);
            }
            return lst;
        }


        public static void SetFeatureBufferValueSameFieldName(IFeatureBuffer featureBuffer, IFeature feature, List<string> exceptFileName)
        {
            if (exceptFileName == null) exceptFileName = new List<string>();
            exceptFileName.Add("SHAPE_Length");
            exceptFileName.Add("SHAPE_Area");
            for (int i = 0; i < featureBuffer.Fields.FieldCount; i++)
            {
                IField pTarField = featureBuffer.Fields.Field[i];
                string sFieldName = pTarField.Name;
                if (pTarField.Type != esriFieldType.esriFieldTypeOID && !exceptFileName.Contains(sFieldName))
                {
                    int iIndex = feature.Fields.FindField(sFieldName);
                    if (iIndex != -1)
                    {
                        if (!string.IsNullOrWhiteSpace(feature.get_Value(iIndex).ToString()))
                        {
                            featureBuffer.set_Value(i, feature.get_Value(iIndex));
                        }
                    }
                }
            }
        }


        /// <summary>
        ///设置Featurebuffer的字段值
        /// </summary>
        /// <param name="feaBuffer"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFeatureBufferValue(IFeatureBuffer feaBuffer, string fieldName, object value)
        {
            int idx = feaBuffer.Fields.FindField(fieldName);
            if (idx == -1)
                return;
            try
            {
                feaBuffer.set_Value(idx, value);
            }
            catch { }
        }


        public static double StatsFieldSumValue(IFeatureClass pFC, IQueryFilter pQf, string fldName)
        {
            ICursor cursor = pFC.Search(pQf, false) as ICursor;
            double sum = 0; 
            try
            {
                //Create a DataStatistics object and initialize properties
                IDataStatistics dataStatistics = new DataStatisticsClass();
                dataStatistics.Field = fldName;
                dataStatistics.Cursor = cursor;

                //Get the result statistics
                IStatisticsResults statisticsResults = dataStatistics.Statistics;
               
                sum = statisticsResults.Sum;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return sum;
        }

        public static void StatsFieldValue(IFeatureClass pFC,IQueryFilter pQf, string fldName, out  double max, out  double min, out  double sum, out double mean)
        {
            //Get a feature cursor
            ICursor cursor = pFC.Search(pQf, false) as ICursor;
            max = 0; min = 0; sum = 0; mean = 0;
            try
            {
                //Create a DataStatistics object and initialize properties
                IDataStatistics dataStatistics = new DataStatisticsClass();
                dataStatistics.Field = fldName;
                dataStatistics.Cursor = cursor;
                

                //Get the result statistics
                IStatisticsResults statisticsResults = dataStatistics.Statistics;
                max = statisticsResults.Maximum;
                min = statisticsResults.Minimum;
                sum = statisticsResults.Sum;
                mean = statisticsResults.Mean;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        /// <summary>
        /// 统计合计
        /// </summary>
        /// <param name="pFC"></param>
        /// <param name="where"></param>
        /// <param name="fldName"></param>
        /// <returns></returns>
        public static double StatsFieldSumValue(IFeatureClass pFC,string where, string fldName)
        {
            //Get a feature cursor
            IQueryFilter pQf = new QueryFilterClass();
            if (where.Trim() == "")
            {
            }
            else
            {
                pQf.WhereClause = where;
            }
            ICursor cursor = pFC.Search(pQf, true) as ICursor;
            double dsum = 0;
            try
            {
                //Create a DataStatistics object and initialize properties
                IDataStatistics dataStatistics = new DataStatisticsClass();
                dataStatistics.Field = fldName;
                dataStatistics.Cursor = cursor;

                //Get the result statistics
                IStatisticsResults statisticsResults = dataStatistics.Statistics;
                dsum = statisticsResults.Sum;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQf);
            }
            return dsum;

        }
        


        
        /// <summary>
        /// 将ITable转换为DataTable
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(ITable mTable)
        {
            try
            {
                DataTable pTable = new DataTable();
                for (int i = 0; i < mTable.Fields.FieldCount; i++)
                {
                    pTable.Columns.Add(mTable.Fields.get_Field(i).Name);
                }

                ICursor pCursor = mTable.Search(null, false);
                IRow pRrow = pCursor.NextRow();
                while (pRrow != null)
                {
                    DataRow pRow = pTable.NewRow();
                    string[] StrRow = new string[pRrow.Fields.FieldCount];
                    for (int i = 0; i < pRrow.Fields.FieldCount; i++)
                    {
                        StrRow[i] = pRrow.get_Value(i).ToString();
                    }
                    pRow.ItemArray = StrRow;
                    pTable.Rows.Add(pRow);
                    pRrow = pCursor.NextRow();
                }

                return pTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
