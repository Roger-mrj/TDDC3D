using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using System.Collections;

namespace RCIS.GISCommon
{
	/// <summary>
	/// DatabaseHelper 的摘要说明。
	/// </summary>
	public class DatabaseHelper
	{
        public static void RebuildSpatialIndex(IFeatureClass featureClass, Double gridOneSize, Double gridTwoSize, Double gridThreeSize)
        {
            // Get an enumerator for indexes based on the shape field.
            IIndexes indexes = featureClass.Indexes;
            String shapeFieldName = featureClass.ShapeFieldName;
            IEnumIndex enumIndex = indexes.FindIndexesByFieldName(shapeFieldName);
            enumIndex.Reset();

            // Get the index based on the shape field (should only be one) and delete it.
            IIndex index = enumIndex.Next();
            if (index != null)
            {
                featureClass.DeleteIndex(index);
            }

            // Clone the shape field from the feature class.
            int shapeFieldIndex = featureClass.FindField(shapeFieldName);
            IFields fields = featureClass.Fields;
            IField sourceField = fields.get_Field(shapeFieldIndex);
            IClone sourceFieldClone = (IClone)sourceField;
            IClone targetFieldClone = sourceFieldClone.Clone();
            IField targetField = (IField)targetFieldClone;

            // Open the geometry definition from the cloned field and modify it.
            IGeometryDef geometryDef = targetField.GeometryDef;
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
            geometryDefEdit.GridCount_2 = 3;
            geometryDefEdit.set_GridSize(0, gridOneSize);
            geometryDefEdit.set_GridSize(1, gridTwoSize);
            geometryDefEdit.set_GridSize(2, gridThreeSize);

            // Create a spatial index and set the required attributes.
            IIndex newIndex = new IndexClass();
            IIndexEdit newIndexEdit = (IIndexEdit)newIndex;
            newIndexEdit.Name_2 = String.Concat(shapeFieldName, "_Index");
            newIndexEdit.IsAscending_2 = true;
            newIndexEdit.IsUnique_2 = false;

            // Create a fields collection and assign it to the new index.
            IFields newIndexFields = new FieldsClass();
            IFieldsEdit newIndexFieldsEdit = (IFieldsEdit)newIndexFields;
            newIndexFieldsEdit.AddField(targetField);
            newIndexEdit.Fields_2 = newIndexFields;

            // Add the spatial index back into the feature class.
            featureClass.AddIndex(newIndex);
        }

        public static void CreateIndex(ITable pTable, string fieldName)
        {
            IEnumIndex pEnumIndex = pTable.Indexes.FindIndexesByFieldName(fieldName);
            pEnumIndex.Reset();
            IIndex pIndex = pEnumIndex.Next();
            if (pIndex != null) return;
            pIndex = new IndexClass();
            IIndexEdit pIndexEdit = pIndex as IIndexEdit;
            IFields pFieldsIndex = new FieldsClass();
            IFieldsEdit pFieldsEditIndex = pFieldsIndex as IFieldsEdit;
            int feildindex = pTable.Fields.FindField(fieldName);
            IField pFieldIndex = pTable.Fields.Field[feildindex];
            pFieldsEditIndex.FieldCount_2 = 1;
            pFieldsEditIndex.set_Field(0, pFieldIndex);
            pIndexEdit.Fields_2 = pFieldsIndex;
            pIndexEdit.Name_2 = fieldName;
            pIndexEdit.IsAscending_2 = true;
            pTable.AddIndex(pIndex);
        }

        public static void DeleteIndex(IFeatureClass pFeatureClass, string fieldName)
        {
            IEnumIndex pEnumIndex = pFeatureClass.Indexes.FindIndexesByFieldName(fieldName);
            pEnumIndex.Reset();
            IIndex pIndex = pEnumIndex.Next();
            while (pIndex != null)
            {
                pFeatureClass.DeleteIndex(pIndex);
                pIndex = pEnumIndex.Next();
            }
        }

        public static void CreateIndex(IFeatureClass pFeatureClass, string fieldName)
        {
            IEnumIndex pEnumIndex = pFeatureClass.Indexes.FindIndexesByFieldName(fieldName);
            pEnumIndex.Reset();
            IIndex pIndex = pEnumIndex.Next();
            if (pIndex != null) return;
            pIndex = new IndexClass();
            IIndexEdit pIndexEdit = pIndex as IIndexEdit;
            IFields pFieldsIndex = new FieldsClass();
            IFieldsEdit pFieldsEditIndex = pFieldsIndex as IFieldsEdit;
            int feildindex = pFeatureClass.Fields.FindField(fieldName);
            IField pFieldIndex = pFeatureClass.Fields.Field[feildindex];
            pFieldsEditIndex.FieldCount_2 = 1;
            pFieldsEditIndex.set_Field(0, pFieldIndex);
            pIndexEdit.Fields_2 = pFieldsIndex;
            pIndexEdit.Name_2 = fieldName;
            pIndexEdit.IsAscending_2 = true;
            pFeatureClass.AddIndex(pIndex);
        }
        
        public static string QueryFieldTypeName(esriFieldType paramFT)
        {
            string resultName="";
            if(esriFieldType.esriFieldTypeBlob==paramFT)
            {//二进制
                resultName="二进制";
            }
            else if(esriFieldType.esriFieldTypeDate==paramFT)
            {//日期
                resultName="日期";
            }
            else if(esriFieldType.esriFieldTypeDouble==paramFT)
            {//Double
                resultName="浮点数";
            }
            else if(esriFieldType.esriFieldTypeGeometry==paramFT)
            {//Geometry
                resultName="图形对象";
            }
            else if(esriFieldType.esriFieldTypeGlobalID==paramFT)
            {//GlobalID
                resultName="全局编号";
            }
            else if(esriFieldType.esriFieldTypeGUID==paramFT)
            {//永久编号
                resultName="永久唯一编号";
            }
            else if(esriFieldType.esriFieldTypeInteger==paramFT)
            {
                resultName="整型";
            }
            else if(esriFieldType.esriFieldTypeOID==paramFT)
            {
                resultName="表内编号";
            }
            else if(esriFieldType.esriFieldTypeRaster==paramFT)
            {
                resultName="影像图";
            }
            else if(esriFieldType.esriFieldTypeSingle==paramFT)
            {
                resultName="浮点数";
            }
            else if(esriFieldType.esriFieldTypeSmallInteger==paramFT)
            {
                resultName="短整型";
            }
            else if(esriFieldType.esriFieldTypeString==paramFT)
            {
                resultName="字符串";
            }
            return resultName;
        }
        public static int QueryFieldLength(IField paramFld)
        {
            if(paramFld.Type==esriFieldType.esriFieldTypeSingle
                ||paramFld.Type==esriFieldType.esriFieldTypeDouble
               )
            {
                return paramFld.Precision;
            }
            else if( paramFld.Type == esriFieldType.esriFieldTypeInteger)
            {
                return paramFld.Length;
            }
            else return paramFld.Length ;
        }
        public static int QueryFieldPrecision(IField paramFld)
        {
            if(paramFld.Type==esriFieldType.esriFieldTypeSingle
                ||paramFld.Type==esriFieldType.esriFieldTypeDouble)
            {
                return paramFld.Scale;
            }
            else return 0;
        }
        public static IFeatureClass GetFeatureClass(IWorkspace pWorkspace, int pID)
        {
            IFeatureClass rClassObj=null;
            IFeatureWorkspace wkSpace = pWorkspace as IFeatureWorkspace;
            if (wkSpace == null) return null;
            List<string> aContainerList = QueryFeatureDatasetName(pWorkspace, true, true);
            foreach (string aContainer in aContainerList)
            {
                try
                {
                    IFeatureDataset fds = wkSpace.OpenFeatureDataset(aContainer);
                    IFeatureClassContainer aContainerObj = fds as IFeatureClassContainer;
                    rClassObj = aContainerObj.get_ClassByID(pID);
                    if (rClassObj != null)
                        break;
                }
                catch (Exception ex) { }
            }
            return rClassObj;
        }
        public static string GetFeatureClassName(IWorkspace pWorkspace,int pID)
        {
            IFeatureClass aClassObj = GetFeatureClass(pWorkspace, pID);
            return LayerHelper.GetClassShortName(aClassObj as IDataset);
        }
        public static List<String> QueryFeatureClassName(IWorkspace pWorkspace)
        {
            return QueryFeatureClassName(pWorkspace,false,false);
        }
        public static List<String> QueryFeatureClassName(IWorkspace pWorkspace,bool pUpperCase)
        {
            return QueryFeatureClassName(pWorkspace, pUpperCase, false);
        }
        public static List<String> QueryFeatureClassName(IWorkspace pWorkspace, bool pUpperCase, bool pEscapeMetaTable)
        {
            try
            {
                String ownerName = "";
                if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    ownerName = pWorkspace.ConnectionProperties.GetProperty("user").ToString();
                    ownerName = ownerName.ToUpper();
                }
                List<String> sc = new List<String>();
                IEnumDatasetName edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
                IDatasetName dn = edn.Next();
                while (dn != null)
                {
                    string dsName = dn.Name.ToUpper();
                    if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                    {
                        #region 添加数据集下面的FeatureClass
                        IEnumDatasetName fdn = dn.SubsetNames;

                        dn = fdn.Next();
                        while (dn != null)
                        {
                            dsName = dn.Name.ToUpper();
                            bool isTopology = dn is ITopologyName;
                            if (!isTopology)
                            {
                                string shortName = LayerHelper.GetClassShortName(dsName);
                                if (pUpperCase)
                                {
                                    shortName = shortName.ToUpper();
                                }
                                if (pEscapeMetaTable)
                                {
                                    if (!IsMetaTable(shortName))
                                    {
                                        sc.Add(shortName);
                                    }
                                }
                                else
                                {
                                    sc.Add(shortName);
                                }
                            }
                            dn = fdn.Next();
                        }
                        #endregion
                    }
                    dn = edn.Next();
                }
                #region 获取直接的FeatureClass
                edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                dn = edn.Next();
                while (dn != null)
                {
                    string dsName = dn.Name.ToUpper();
                    if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                    {
                        string shortName = LayerHelper.GetClassShortName(dsName);
                        if (pUpperCase)
                        {
                            shortName = shortName.ToUpper();
                        }
                        if (pEscapeMetaTable)
                        {
                            if (!IsMetaTable(shortName))
                            {
                                sc.Add(shortName);
                            }
                        }
                        else
                        {
                            sc.Add(shortName);
                        }
                    }
                    dn = edn.Next();
                }
                #endregion
                return sc;
            }
            catch (Exception ex) { return null; }
        }

       

        public static List<String> QueryFeatureClassName(IWorkspace pWorkspace, bool pUpperCase,
            bool pEscapeMetaTable, string sDatasetName)
        {
            String ownerName = "";
            if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                ownerName = pWorkspace.ConnectionProperties.GetProperty("user").ToString();
                ownerName = ownerName.ToUpper();
            }
            List<String> sc = new List<String>();
            IEnumDatasetName edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName dn = edn.Next();
            while (dn != null)
            {
                string dsName = dn.Name.ToUpper();
                if (dsName == sDatasetName.ToUpper())
                {
                    if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                    {
                        #region 添加数据集下面的FeatureClass
                        IEnumDatasetName fdn = dn.SubsetNames;
                        dn = fdn.Next();
                        while (dn != null)
                        {
                            dsName = dn.Name.ToUpper();
                            bool isTopology = dn is ITopologyName;
                            if (!isTopology)
                            {
                                string shortName = LayerHelper.GetClassShortName(dsName);
                                if (pUpperCase)
                                {
                                    shortName = shortName.ToUpper();
                                }
                                if (pEscapeMetaTable)
                                {
                                    if (!IsMetaTable(shortName))
                                    {
                                        sc.Add(shortName);
                                    }
                                }
                                else
                                {
                                    sc.Add(shortName);
                                }
                            }
                            dn = fdn.Next();
                        }
                        #endregion
                    }
                }
                dn = edn.Next();
            }
            #region 获取直接的FeatureClass
            edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            dn = edn.Next();
            while (dn != null)
            {
                string dsName = dn.Name.ToUpper();
                if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                {
                    string shortName = LayerHelper.GetClassShortName(dsName);
                    if (pUpperCase)
                    {
                        shortName = shortName.ToUpper();
                    }
                    if (pEscapeMetaTable)
                    {
                        if (!IsMetaTable(shortName))
                        {
                            sc.Add(shortName);
                        }
                    }
                    else
                    {
                        sc.Add(shortName);
                    }
                }
                dn = edn.Next();
            }
            #endregion
            return sc;
        }
        
        public static List<String> QueryTableName(IWorkspace pWorkspace, bool pUpperCase, bool pEscapeMetaTable)
        {
            String ownerName = "";
            if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                ownerName = pWorkspace.ConnectionProperties.GetProperty("user").ToString();
                ownerName = ownerName.ToUpper();
            }
            List<String> sc = new List<String>();
            
            #region 获取直接的Table
            IEnumDatasetName edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTTable);
            IDatasetName dn = edn.Next();
            while (dn != null)
            {
                string dsName = dn.Name.ToUpper();
                if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                {
                    string shortName = LayerHelper.GetClassShortName(dsName);
                    if (pUpperCase)
                    {
                        shortName = shortName.ToUpper();
                    }
                    if (pEscapeMetaTable)
                    {
                        if (!IsMetaTable(shortName))
                        {
                            sc.Add(shortName);
                        }
                    }
                    else
                    {
                        sc.Add(shortName);
                    }
                }
                dn = edn.Next();
            }
            #endregion
            return sc;
        }
        public static List<String> QueryHistoryMetalTableName(IWorkspace pWorkspace, bool pUpperCase, bool pEscapeMetaTable)
        {
            String ownerName = "";
            if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                ownerName = pWorkspace.ConnectionProperties.GetProperty("user").ToString();
                ownerName = ownerName.ToUpper();
            }
            List<String> sc = new List<String>();

            #region 获取直接的Table
            IEnumDatasetName edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTTable);
            IDatasetName dn = edn.Next();
            while (dn != null)
            {
                string dsName = dn.Name.ToUpper();
                if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                {
                    string shortName = LayerHelper.GetClassShortName(dsName);
                    if (pUpperCase)
                    {
                        shortName = shortName.ToUpper();
                    }
                    if (pEscapeMetaTable)
                    {

                        if (shortName.StartsWith("LSMETA_"))     
                       {
                           sc.Add(shortName);
                       }
                    }
       
                }
                dn = edn.Next();
            }
            #endregion
            return sc;
        }
        public static List<string> QueryFeatureDatasetName(IWorkspace pWorkspace, bool pUpperCase,bool pEscapedMetaDataset)
        {
            String ownerName = "";
            if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                ownerName = pWorkspace.ConnectionProperties.GetProperty("user").ToString();
                ownerName = ownerName.ToUpper();
            }
            List<String> sc = new List<String>();
            IEnumDatasetName edn = pWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            IDatasetName dn = edn.Next();
            while (dn != null)
            {
                string dsName = dn.Name.ToUpper();
                if (ownerName.Equals(LayerHelper.GetClassOwnerName(dsName)))
                {
                    dsName = LayerHelper.GetClassShortName(dsName);
                    if (!dsName.StartsWith("LSATT_")
                        && !dsName.StartsWith("LSMETA_"))
                    {
                        sc.Add(dsName);
                    }
                }
                dn = edn.Next();
            }
            return sc;
        }
        public static IField CreateTextField(string pName, string pAliasName, int aWidth)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = pName.ToUpper ();
            aFieldEdit.AliasName_2 = pAliasName;
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            aFieldEdit.Length_2 = aWidth;
            return aField as IField;
        }
        public static IField CreateOIDField()
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit=aField as IFieldEdit ;
            aFieldEdit.Name_2 ="OBJECTID";
            aFieldEdit.AliasName_2 ="表内唯一编号";
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeOID;
            return aField as IField;
        }
        public static  IField CreateGeometryField(esriGeometryType pGT,ISpatialReference pSR)
        {
            return CreateGeometryField(pGT, pSR, false);
            
        }
        public static IField CreateRasterField(string pName,string pAliasName,ISpatialReference pSR)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = pName;
            aFieldEdit.AliasName_2 = pAliasName;
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;
            IRasterDef aRasDef = new RasterDefClass();
            aRasDef.IsRasterDataset = false;
            aRasDef.SpatialReference = pSR;
            (aFieldEdit as IFieldEdit2).RasterDef = aRasDef;
            return aField as IField;
        }
        public static IField CreateGeometryField(esriGeometryType pGT
            , ISpatialReference pSR,bool pHasZ)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = "SHAPE";
            aFieldEdit.AliasName_2 = "图形对象";
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            GeometryDefClass geomDef = new GeometryDefClass();
            geomDef.IGeometryDefEdit_GeometryType_2 = pGT;
            geomDef.IGeometryDefEdit_HasM_2 = false;
            geomDef.IGeometryDefEdit_HasZ_2 = pHasZ;

            geomDef.IGeometryDefEdit_GridCount_2 = 1;
            geomDef.set_GridSize(0, 1000);
            geomDef.IGeometryDefEdit_SpatialReference_2 = pSR;
            aFieldEdit.GeometryDef_2 = geomDef;
            return aField as IField;

        }
        public static IField CreateDoubleField(String pFieldName, int pFieldLen, int pFieldScale)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = pFieldName;
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            aFieldEdit.Precision_2 = pFieldLen;
            aFieldEdit.Scale_2 = pFieldScale;
            return aField as IField;
        }
        public static IField CreateIntField(String pFieldName, int pFieldLen)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = pFieldName;
            aFieldEdit.Precision_2 = pFieldLen;
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
            return aField as IField;
        }
        
        public static bool HighPrecision(IWorkspace pWorkspace)
        {
            IGeodatabaseRelease geoVersion = pWorkspace as IGeodatabaseRelease;
            if (geoVersion == null) return false;
            if (geoVersion.MajorVersion == 2
                && geoVersion.MinorVersion == 2)
            {
                return true;
            }
            return false;
        }
        
        public static bool SupportVersion(IWorkspace pWorkspace)
        {
            return pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace;
        }
        public static bool IsMetaTable(ITable pTable)
        {
            if (pTable == null) return false;
            string aTableName = LayerHelper.GetClassShortName(pTable as IDataset);
            return IsMetaTable(aTableName);
        }
        public static bool IsMetaTable(string pTableName)
        {
            if (pTableName == null) return false;
            pTableName = pTableName.ToUpper();
            if (pTableName.StartsWith("LSMETA_")
                ||pTableName .StartsWith ("LSHIS_"))            
            {
                return true;
            }
            return false;
        }
        public static bool IsAttributeMetaTable(string pTableName)
        {
            if (pTableName == null) return false;
            pTableName = pTableName.ToUpper();
            if ( pTableName.StartsWith("NCDJ_CODE")
                 || pTableName.StartsWith("POW_")
                 || pTableName.StartsWith("WF_"))
            {
                return true;
            }
            return false;
        }
        public static IFeatureClass CloneFeatureClass(IWorkspace pDestWorkspace, IFeatureClass pSrcClass)
        {
            if (pSrcClass != null)
            {
                ISpatialReference pSR = (pSrcClass as IGeoDataset).SpatialReference;
                return CloneFeatureClass(pDestWorkspace, pSrcClass, pSR);
            }
            return null;
        
        }
        //public static IFeatureClass CloneFeatureClass(IWorkspace pDestWorkspace,
        //    IFeatureClass pSrcClass)
        //{
        //    if (pSrcClass != null)
        //    {
        //        ISpatialReference pSR = (pSrcClass as IGeoDataset).SpatialReference;
        //        return CloneFeatureClass(pDestWorkspace, pSrcClass, pSR,mTaskMonitor);
        //    }
        //    return null;

        //}
        /// <summary>
        /// 拷贝结构
        /// </summary>
        /// <param name="pDestWorkspace"></param>
        /// <param name="pSrcClass"></param>
        /// <param name="pSR"></param>
        /// <returns></returns>
        public static IFeatureClass CloneFeatureClass(IWorkspace pDestWorkspace,
            IFeatureClass pSrcClass, ISpatialReference pSR)
        {
            try
            {
                if (pDestWorkspace == null)
                    return null;
                IField lenField = null;
                try { lenField = pSrcClass.LengthField; }
                catch (Exception ex) { };
                IField areaField = null;
                try { areaField = pSrcClass.AreaField; }
                catch (Exception ex) { };

                IFields flds = new FieldsClass();
                IFieldsEdit fldsEdit = flds as IFieldsEdit;
                int fc = pSrcClass.Fields.FieldCount;
                for (int fi = 0; fi < fc; fi++)
                {
                    IField aFld = pSrcClass.Fields.get_Field(fi);
                    if (aFld != lenField && aFld != areaField)
                    {
                        if (aFld.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            aFld = (aFld as IClone).Clone() as IField;
                            IGeometryDef aGeomDef = (aFld as IFieldEdit).GeometryDef;
                            (aGeomDef as IGeometryDefEdit).SpatialReference_2 = pSR;
                            try
                            {
                                int aGridCount = 0;
                                try
                                {
                                    aGridCount = aGeomDef.GridCount;
                                }
                                catch (Exception ex) { }
                                if (aGridCount <= 0)
                                {
                                    (aGeomDef as IGeometryDefEdit).GridCount_2 = 1;
                                    (aGeomDef as IGeometryDefEdit).set_GridSize(0, 1000);
                                }
                            }
                            catch (Exception ex) { }
                            (aFld as IFieldEdit).GeometryDef_2 = aGeomDef;
                        }
                        fldsEdit.AddField(aFld);
                    }
                }
                string aClassName = LayerHelper.GetClassShortName(pSrcClass as IDataset);

                IFeatureClass resultClass = null;
                try
                {
                    resultClass = (pDestWorkspace as IFeatureWorkspace)
                    .CreateFeatureClass(aClassName, flds
                , pSrcClass.CLSID, pSrcClass.EXTCLSID, pSrcClass.FeatureType
                , pSrcClass.ShapeFieldName, null);
                }
                catch (Exception ex) { }
                try
                {
                    resultClass = (pDestWorkspace as IFeatureWorkspace).OpenFeatureClass(aClassName);
                    FeatureHelper.CopyTable(pSrcClass as ITable
                    , resultClass as ITable);

                }
                catch (Exception ex) { }
                return resultClass;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
      
        //public static IFeatureClass CloneFeatureClass(IWorkspace pDestWorkspace, IFeatureClass pSrcClass, ISpatialReference pSR)
        //{
        //    try
        //    {
        //        if (pDestWorkspace == null)
        //            return null;
        //        IField lenField = null;
        //        try { lenField = pSrcClass.LengthField; }
        //        catch (Exception ex) { };
        //        IField areaField = null;
        //        try { areaField = pSrcClass.AreaField; }
        //        catch (Exception ex) { };

        //        IFields flds = new FieldsClass();
        //        IFieldsEdit fldsEdit = flds as IFieldsEdit;
        //        int fc = pSrcClass.Fields.FieldCount;
        //        for (int fi = 0; fi < fc; fi++)
        //        {
        //            IField aFld = pSrcClass.Fields.get_Field(fi);
        //            if (aFld != lenField && aFld != areaField)
        //            {
        //                if (aFld.Type == esriFieldType.esriFieldTypeGeometry)
        //                {
        //                    aFld = (aFld as IClone).Clone() as IField;
        //                    IGeometryDef aGeomDef = (aFld as IFieldEdit).GeometryDef;                           
        //                    (aGeomDef as IGeometryDefEdit).SpatialReference_2 = pSR;
        //                    try
        //                    {
        //                        int aGridCount = 0;
        //                        try
        //                        {
        //                            aGridCount = aGeomDef.GridCount;
        //                        }
        //                        catch (Exception ex) { }
        //                        if (aGridCount <= 0)
        //                        {
        //                            (aGeomDef as IGeometryDefEdit).GridCount_2 = 1;
        //                            (aGeomDef as IGeometryDefEdit).set_GridSize(0, 1000);
        //                        }
        //                    }
        //                    catch (Exception ex) { }
        //                    (aFld as IFieldEdit).GeometryDef_2 = aGeomDef;
        //                }
        //                fldsEdit.AddField(aFld);
        //            }
        //        }
        //        string aClassName = LayerHelper.GetClassShortName(pSrcClass as IDataset);

        //        IFeatureClass resultClass = null;
        //        try
        //        {
        //            resultClass=(pDestWorkspace as IFeatureWorkspace)
        //            .CreateFeatureClass(aClassName, flds
        //        , pSrcClass.CLSID, pSrcClass.EXTCLSID, pSrcClass.FeatureType
        //        , pSrcClass.ShapeFieldName, null);
        //        }
        //        catch (Exception ex) { }
        //        try
        //        {
        //            resultClass = (pDestWorkspace as IFeatureWorkspace).OpenFeatureClass(aClassName);
        //            FeatureHelper.CopyTable(pSrcClass as ITable
        //            , resultClass as ITable);
                  
        //        }
        //        catch (Exception ex) { }
        //        return resultClass;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}
        public static IFeatureClass CloneFeatureClass(IFeatureDataset pFDS, IFeatureClass pSrcClass)
        {
            try
            {
                if (pFDS == null)
                    return null;
                ISpatialReference aSR = (pFDS as IGeoDataset).SpatialReference;
                IField lenField = null;
                try { lenField = pSrcClass.LengthField; }
                catch (Exception ex) { };
                IField areaField = null;
                try { areaField = pSrcClass.AreaField; }
                catch  { };

                IFields flds = new FieldsClass();
                IFieldsEdit fldsEdit = flds as IFieldsEdit;
                int fc = pSrcClass.Fields.FieldCount;
                for (int fi = 0; fi < fc; fi++)
                {
                    IField aFld = pSrcClass.Fields.get_Field(fi);
                    if (aFld != lenField && aFld != areaField)
                    {
                        if (aFld.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            aFld = (aFld as IClone).Clone() as IField;
                            IGeometryDef aGeomDef = (aFld as IFieldEdit).GeometryDef;
                            (aGeomDef as IGeometryDefEdit).SpatialReference_2 = aSR;
                            (aFld as IFieldEdit).GeometryDef_2 = aGeomDef;
                        }
                        fldsEdit.AddField(aFld);
                    }
                }
                string aClassName = LayerHelper.GetClassShortName(pSrcClass as IDataset);
                IFeatureClass resultClass = null;
                try
                {
                    resultClass=pFDS.CreateFeatureClass(aClassName, flds
                 , pSrcClass.CLSID, pSrcClass.EXTCLSID, pSrcClass.FeatureType
                 , pSrcClass.ShapeFieldName, null);
                }
                catch  { }
                try
                {
                    resultClass = (pFDS.Workspace as IFeatureWorkspace ).OpenFeatureClass(aClassName);
                    FeatureHelper.CopyTable(pSrcClass as ITable
                    , resultClass as ITable);
                }
                catch  { }
                return resultClass;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        //public static IFeatureClass CloneFeatureClass(IFeatureDataset pFDS, IFeatureClass pSrcClass)
        //{
        //    try
        //    {
        //        if (pFDS == null)
        //            return null;
        //        ISpatialReference aSR = (pFDS as IGeoDataset).SpatialReference;
        //        IField lenField = null;
        //        try { lenField = pSrcClass.LengthField; }
        //        catch (Exception ex) { };
        //        IField areaField = null;
        //        try { areaField = pSrcClass.AreaField; }
        //        catch (Exception ex) { };

        //        IFields flds = new FieldsClass();
        //        IFieldsEdit fldsEdit = flds as IFieldsEdit;
        //        int fc = pSrcClass.Fields.FieldCount;
        //        for (int fi = 0; fi < fc; fi++)
        //        {
        //            IField aFld = pSrcClass.Fields.get_Field(fi);
        //            if (aFld != lenField && aFld != areaField)
        //            {
        //                if (aFld.Type == esriFieldType.esriFieldTypeGeometry)
        //                {
        //                    aFld = (aFld as IClone).Clone() as IField;
        //                    IGeometryDef aGeomDef = (aFld as IFieldEdit).GeometryDef;
        //                    (aGeomDef as IGeometryDefEdit).SpatialReference_2 = aSR;
        //                    (aFld as IFieldEdit).GeometryDef_2 = aGeomDef;
        //                }
        //                fldsEdit.AddField(aFld);
        //            }
        //        }
        //        string aClassName = LayerHelper.GetClassShortName(pSrcClass as IDataset);
        //        IFeatureClass resultClass = null;
        //        try
        //        {
        //            resultClass = pFDS.CreateFeatureClass(aClassName, flds
        //         , pSrcClass.CLSID, pSrcClass.EXTCLSID, pSrcClass.FeatureType
        //         , pSrcClass.ShapeFieldName, null);
        //        }
        //        catch (Exception ex) { }
        //        try
        //        {
        //            resultClass = (pFDS.Workspace as IFeatureWorkspace).OpenFeatureClass(aClassName);
        //            FeatureHelper.CopyTable(pSrcClass as ITable
        //            , resultClass as ITable,mTaskMonitor);
        //        }
        //        catch (Exception ex) { }
        //        return resultClass;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}
        /// <summary>
        /// 备份或者复制数据库
        /// </summary>
        /// <param name="sourceWorkspace"></param>
        /// <param name="targetWorkspace"></param>
        /// <param name="objectName"></param>
        /// <param name="esriDataType"></param>
         public static  void CopyPasteGeodatabaseData(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, String objectName, esriDatasetType esriDataType)
            {

                // Validate input

                if ((sourceWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace) || (targetWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace))
                {
                    return; // Should be a throw
                }

                //create source workspace name
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
                //create target workspace name
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                //Create Name Object Based on data type
                IDatasetName datasetName;
                switch (esriDataType)
                {
                    case esriDatasetType.esriDTFeatureDataset:

                        IFeatureDatasetName inFeatureDatasetName = new FeatureDatasetNameClass();
                        datasetName = (IDatasetName)inFeatureDatasetName;
                        break;

                    case esriDatasetType.esriDTFeatureClass:

                        IFeatureClassName inFeatureClassName = new FeatureClassNameClass();

                        datasetName = (IDatasetName)inFeatureClassName;
                        break;

                    case esriDatasetType.esriDTTable:
                        ITableName inTableName = new TableNameClass();
                        datasetName = (IDatasetName)inTableName;
                        break;
                    case esriDatasetType.esriDTGeometricNetwork:

                        IGeometricNetworkName inGeometricNetworkName = new GeometricNetworkNameClass();
                        datasetName = (IDatasetName)inGeometricNetworkName;

                        break;

                    case esriDatasetType.esriDTRelationshipClass:

                        IRelationshipClassName inRelationshipClassName = new RelationshipClassNameClass();

                        datasetName = (IDatasetName)inRelationshipClassName;
                        break;

                    case esriDatasetType.esriDTNetworkDataset:

                        INetworkDatasetName inNetworkDatasetName = new NetworkDatasetNameClass();
                        datasetName = (IDatasetName)inNetworkDatasetName;
                        break;

                    case esriDatasetType.esriDTTopology:

                        ITopologyName inTopologyName = new TopologyNameClass();

                        datasetName = (IDatasetName)inTopologyName;
                        break;

                    default:
                    return; // Should be a throw
                }

                // Set the name of the object to be copied

                datasetName.WorkspaceName = (IWorkspaceName)sourceWorkspaceName;

                datasetName.Name = objectName;

                //Setup mapping for copy/paste

                IEnumNameMapping fromNameMapping;

                ESRI.ArcGIS.esriSystem.IEnumNameEdit editFromName;

                ESRI.ArcGIS.esriSystem.IEnumName fromName = new NamesEnumerator();

                editFromName = (ESRI.ArcGIS.esriSystem.IEnumNameEdit)fromName;

                editFromName.Add((ESRI.ArcGIS.esriSystem.IName)datasetName);

                ESRI.ArcGIS.esriSystem.IName toName = (ESRI.ArcGIS.esriSystem.IName)targetWorkspaceName;

                // Generate name mapping

                ESRI.ArcGIS.Geodatabase.IGeoDBDataTransfer geoDBDataTransfer = new GeoDBDataTransferClass();

                Boolean targetWorkspaceExists;

                targetWorkspaceExists = geoDBDataTransfer.GenerateNameMapping(fromName, toName, out fromNameMapping);

                // Copy/Pate the data

             geoDBDataTransfer.Transfer(fromNameMapping, toName);



            }
        /// <summary>
        /// 获得一个shapefile文件的的要素类对象
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static IFeatureClass GetShapefileWorkspaceFeatureClass(string sFilePath)
        {
            try
            {
                IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();

                string sPath = System.IO.Path.GetDirectoryName(sFilePath);
                IWorkspace pWS = pWSF.OpenFromFile(sPath, 0);

                string sFileName = System.IO.Path.GetFileNameWithoutExtension(sFilePath);

                IFeatureWorkspace pFWS = pWS as IFeatureWorkspace;

                IFeatureClass pFC = pFWS.OpenFeatureClass(sFileName);

                return pFC;
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取当前数据集所有要素类
        /// </summary>
        /// <param name="pDS"></param>
        /// <returns></returns>
        public static List<IFeatureClass> getAllFeatureClass(IFeatureDataset pDS)
        {
            List<IFeatureClass> lstClasses = new List<IFeatureClass>();
            IFeatureClassContainer pContainer = pDS as IFeatureClassContainer;
            for (int i = 0; i < pContainer.ClassCount; i++)
            {
                lstClasses.Add(pContainer.get_Class(i));
            }
            return lstClasses;
        }

        /// <summary>
        /// 获取所有要素类，包括数据集外面和里面的
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        public static List<IFeatureClass> getAllFeatureClass(IWorkspace ws)
        {
            List<IFeatureClass> lstClasses = new List<IFeatureClass>();
                IEnumDataset DSs = ws.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTAny);
                DSs.Reset();
                IDataset ds = DSs.Next();
                while (ds != null)
                {
                    if (ds.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        IFeatureClass curFeatCls = ds as IFeatureClass;
                        lstClasses.Add(curFeatCls);

                    }
                    else if (ds.Type == esriDatasetType.esriDTFeatureDataset)
                    {
                        //提取出其内的FeatureClass:
                        IFeatureWorkspace fws = ws as IFeatureWorkspace;
                        IFeatureDataset Feats = ds as IFeatureDataset;
                        IFeatureClassContainer FeatCon = (IFeatureClassContainer)Feats;
                        IEnumFeatureClass featClasses = FeatCon.Classes;

                        featClasses.Reset();
                        IFeatureClass myFeatClass = featClasses.Next();
                        while (myFeatClass != null)
                        {
                            lstClasses.Add(myFeatClass);
                            myFeatClass = featClasses.Next();
                        } //while(...
                    }
                    ds = DSs.Next();
                }
                return lstClasses;
        }


        /// <summary>
        /// 删除数据集
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <param name="sDsName"></param>
        public static void DelDataset(IWorkspace pWorkspace, string sDsName)
        {
            if (pWorkspace == null)
                return;
            IFeatureWorkspace pFcWorkspace = pWorkspace as IFeatureWorkspace;
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset pDataset = pEnumDataset.Next();
            while (pDataset != null)
            {
                string sName = pDataset.Name.ToUpper();
                if (sName.Contains("."))
                {
                    int pos = sName.IndexOf('.');
                    sName = sName.Substring(pos + 1);
                }

                if (sDsName.ToUpper() == sName)
                {
                    break;
                }
                pDataset = pEnumDataset.Next();
            }
            if (pDataset == null)
                return;

            IFeatureClassContainer FeatCon = (IFeatureClassContainer)pDataset;
            IEnumFeatureClass featClasses = FeatCon.Classes;
            featClasses.Reset();
            IFeatureClass myFeatClass = featClasses.Next();
            ArrayList delArr = new ArrayList();
            while (myFeatClass != null)
            {
                IDataset curDS = myFeatClass as IDataset;
                bool bRet = curDS.CanDelete();
                if (bRet)
                    delArr.Add(curDS);
                myFeatClass = featClasses.Next();
            }
            for (int k = 0; k < delArr.Count; k++)
            {
                IDataset ds = (IDataset)delArr[k];
                ds.Delete();
            }

            pDataset.Delete();
        }



        /// <summary>
        /// 修改字段名称
        /// </summary>
        /// <param name="pFeatureClass">目标要素类</param>
        /// <param name="oldFieldName">目标字段名称</param>
        /// <param name="newFieldName">目标字段新名称</param>
        /// <param name="aliasName">目标字段新别名</param>        
        public static bool ModifyFieldName(IFeatureClass pFeatureClass, string oldFieldName, string newFieldName, string aliasName)
        {
            bool isModified = false;
            ISchemaLock pSchemaLock = null;
            IFields pFields = pFeatureClass.Fields;
            int fIndex = pFields.FindField(oldFieldName);
            if (fIndex == -1) return isModified;
            pSchemaLock = pFeatureClass as ISchemaLock;
            pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);//设置编辑锁
            try
            {
                
                IClassSchemaEdit4 pClassSchemaEdit = pFeatureClass as IClassSchemaEdit4;
                pClassSchemaEdit.AlterFieldAliasName(oldFieldName, aliasName);
                pClassSchemaEdit.AlterFieldName(oldFieldName, newFieldName);
            }
            catch (Exception ex)
            {
                return isModified;
            }
            finally
            {
                //释放编辑锁
                pSchemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                isModified = true;
            }
            return isModified;
        }
    }
}
