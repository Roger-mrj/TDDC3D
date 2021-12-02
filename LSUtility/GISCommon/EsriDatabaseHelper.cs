using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

using ESRI.ArcGIS.DataSourcesFile;
using System.Data;
using ESRI.ArcGIS.DataSourcesOleDB;
using System.Data.OleDb;

namespace RCIS.GISCommon
{
    /// <summary>
    /// Geodatabase创建数据表 ，创建字段的相关函数
    /// </summary>
    public class EsriDatabaseHelper
    {
        /// <summary>
        /// 创建文本型字段
        /// </summary>
        /// <param name="pName">字段名</param>
        /// <param name="pAliasName">字段别名</param>
        /// <param name="aWidth">字段长度</param>
        /// <returns></returns>
        public static IField CreateTextField(string pName, string pAliasName, int aWidth)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = pName.ToUpper();
            aFieldEdit.AliasName_2 = pAliasName;
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            aFieldEdit.Length_2 = aWidth;
            return aField as IField;
        }

        public static IField CreateNumberField(string pName, string pAliasName)
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = pName.ToUpper();
            aFieldEdit.AliasName_2 = pAliasName;
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
            return aField as IField;
        }
        /// <summary>
        /// 创建Objectid字段
        /// </summary>
        /// <returns></returns>
        public static IField CreateOIDField()
        {
            FieldClass aField = new FieldClass();
            IFieldEdit aFieldEdit = aField as IFieldEdit;
            aFieldEdit.Name_2 = "OBJECTID";
            aFieldEdit.AliasName_2 = "表内唯一编号";
            aFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            return aField as IField;
        }
        /// <summary>
        /// 创建shape字段
        /// </summary>
        /// <param name="pGT"></param>
        /// <param name="pSR"></param>
        /// <returns></returns>
        public static IField CreateGeometryField(esriGeometryType pGT, ISpatialReference pSR)
        {
            return CreateGeometryField(pGT, pSR, false);

        }

        public static IField CreateGeometryField(esriGeometryType pGT
            , ISpatialReference pSR, bool pHasZ)
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


        public static void ExportFeature2(IFeatureClass pInFeatureClass, string pPath, IFeatureSelection pFeatureSelection)
        {

            ISelectionSet pSelection = pFeatureSelection.SelectionSet;

            // create a new Access workspace factory       
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            string parentPath = pPath.Substring(0, pPath.LastIndexOf('\\'));
            //string fileName = pPath.Substring(pPath.LastIndexOf('\\') + 1, pPath.Length - pPath.LastIndexOf('\\') - 1);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(pPath);

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(parentPath, fileName, null, 0);
            // Cast for IName       
            IName name = (IName)pWorkspaceName;
            //Open a reference to the access workspace through the name object       
            IWorkspace pOutWorkspace = (IWorkspace)name.Open();

            IDataset pInDataset = pInFeatureClass as IDataset;
            IFeatureClassName pInFCName = pInDataset.FullName as IFeatureClassName;

            IDatasetName sourceDatasetName = (IDatasetName)pInFCName;

            IWorkspace pInWorkspace = pInDataset.Workspace;
            IDataset pOutDataset = pOutWorkspace as IDataset;
            IWorkspaceName pOutWorkspaceName = pOutDataset.FullName as IWorkspaceName;

            IFeatureClassName pOutFCName = new FeatureClassNameClass();
            IDatasetName pDatasetName = pOutFCName as IDatasetName;
            pDatasetName.WorkspaceName = pOutWorkspaceName;
            pDatasetName.Name = pInFeatureClass.AliasName;

            IFieldChecker pFieldChecker = new FieldCheckerClass();
            pFieldChecker.InputWorkspace = pInWorkspace;
            pFieldChecker.ValidateWorkspace = pOutWorkspace;
            IFields pFields = pInFeatureClass.Fields;
            IFields pOutFields;
            IEnumFieldError pEnumFieldError;
            pFieldChecker.Validate(pFields, out pEnumFieldError, out pOutFields);

            IFeatureDataConverter2 pFeatureDataConverter = new FeatureDataConverterClass();

          
            IField geometryField;
            for (int i = 0; i < pOutFields.FieldCount; i++)
            {
                if (pOutFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    geometryField = pOutFields.get_Field(i);
                    // Get the geometry field's geometry defenition          
                    IGeometryDef geometryDef = geometryField.GeometryDef;
                    //Give the geometry definition a spatial index grid count and grid size     
                    IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                    targetFCGeoDefEdit.GridCount_2 = 1;
                    targetFCGeoDefEdit.set_GridSize(0, 0);
                    //Allow ArcGIS to determine a valid grid size for the data loaded     
                    targetFCGeoDefEdit.SpatialReference_2 = (pInFeatureClass as IGeoDataset).SpatialReference;

                    break;
                }
            }
            pFeatureDataConverter.ConvertFeatureClass(sourceDatasetName, null, pSelection, null, pOutFCName, null, pOutFields, "", 100, 0);
        }

        /// <summary>
        /// 选中导出shp，正确
        /// </summary>
        /// <param name="pInFeatureClass"></param>
        /// <param name="pPath"></param>
        /// <param name="pFeatureSelection"></param>
        public static void ExportFeature3(IFeatureClass pInFeatureClass, string pPath, IFeatureSelection pFeatureSelection)
        {

            ISelectionSet pSelection = pFeatureSelection.SelectionSet;

            // create a new Access workspace factory       
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            string parentPath = System.IO.Path.GetDirectoryName(pPath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(pPath);

            IWorkspace pOutWs = WorkspaceHelper2.GetShapefileWorkspace(pPath);
            IWorkspaceName pWorkspaceName = (pOutWs as IDataset).FullName as IWorkspaceName;  //pWorkspaceFactory.Create(parentPath, fileName, null, 0);
            // Cast for IName       
            IName name = (IName)pWorkspaceName;
            //Open a reference to the access workspace through the name object       
            IWorkspace pOutWorkspace = (IWorkspace)name.Open();

            IDataset pInDataset = pInFeatureClass as IDataset;
            IFeatureClassName pInFCName = pInDataset.FullName as IFeatureClassName;

            IDatasetName sourceDatasetName = (IDatasetName)pInFCName;

            IWorkspace pInWorkspace = pInDataset.Workspace;
            IDataset pOutDataset = pOutWorkspace as IDataset;
            IWorkspaceName pOutWorkspaceName = pOutDataset.FullName as IWorkspaceName;

            IFeatureClassName pOutFCName = new FeatureClassNameClass();
            IDatasetName pDatasetName = pOutFCName as IDatasetName;
            pDatasetName.WorkspaceName = pOutWorkspaceName;
            pDatasetName.Name = fileName;  //pInFeatureClass.AliasName;

            IFieldChecker pFieldChecker = new FieldCheckerClass();
            pFieldChecker.InputWorkspace = pInWorkspace;
            pFieldChecker.ValidateWorkspace = pOutWorkspace;
            IFields pFields = pInFeatureClass.Fields;
            IFields pOutFields;
            IEnumFieldError pEnumFieldError;
            pFieldChecker.Validate(pFields, out pEnumFieldError, out pOutFields);

            IFeatureDataConverter2 pFeatureDataConverter = new FeatureDataConverterClass();


            IField geometryField;
            for (int i = 0; i < pOutFields.FieldCount; i++)
            {
                if (pOutFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    geometryField = pOutFields.get_Field(i);
                    // Get the geometry field's geometry defenition          
                    IGeometryDef geometryDef = geometryField.GeometryDef;
                    //Give the geometry definition a spatial index grid count and grid size     
                    IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                    targetFCGeoDefEdit.GridCount_2 = 1;
                    targetFCGeoDefEdit.set_GridSize(0, 0);
                    //Allow ArcGIS to determine a valid grid size for the data loaded     
                    targetFCGeoDefEdit.SpatialReference_2 = (pInFeatureClass as IGeoDataset).SpatialReference;

                    break;
                }
            }
            pFeatureDataConverter.ConvertFeatureClass(sourceDatasetName, null, pSelection, null, pOutFCName, null, pOutFields, "", 100, 0);
        }


        public static bool CopyTable(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, string nameOfSourceTable, string nameOfTargetTable, IQueryFilter pQueryFilter = null)
        {
            bool bResult = true;
            try
            {
                //create source workspace name
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
                //create source dataset name
                ITableName sourceTableName = new TableNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceTableName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = nameOfSourceTable;
                //create target workspace name
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                //create target dataset name
                ITableName targetTableName = new TableNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetTableName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = nameOfTargetTable;
                //Open input Table to get field definitions.
                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceTableName;
                ITable sourceTable = (ITable)sourceName.Open();
                //Validate the field names because you are converting between different workspace types.
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IFields targetTableFields;
                IFields sourceTableFields = sourceTable.Fields;
                IEnumFieldError enumFieldError;
                // Most importantly set the input and validate workspaces!
                fieldChecker.InputWorkspace = sourceWorkspace;
                fieldChecker.ValidateWorkspace = targetWorkspace;
                fieldChecker.Validate(sourceTableFields, out enumFieldError, out targetTableFields);
                // Load the table 
                IFeatureDataConverter pConverter = new FeatureDataConverterClass();
                IEnumInvalidObject enumErrors = pConverter.ConvertTable(sourceDatasetName, pQueryFilter, targetDatasetName, targetTableFields, "", 500, 0);

                IObjectClass pTableObject = sourceTable as IObjectClass;
                ESRI.ArcGIS.esriSystem.IName targetName = (ESRI.ArcGIS.esriSystem.IName)targetTableName;
                ITable targetTable = targetName.Open() as ITable;
                IClassSchemaEdit2 pClassSchemaEdit2 = targetTable as IClassSchemaEdit2;
                pClassSchemaEdit2.AlterAliasName(pTableObject.AliasName);

            }
            catch (Exception ex)
            {
                bResult = false;
            }
            return bResult;
        }

        public static bool ConvertFeatureClass(IWorkspace sourceWorkspace, IWorkspace targetWorkspace, 
            string nameOfSourceFeatureClass, string nameOfTargetFeatureClass, IQueryFilter pQueryFilter)
        {
            bool bResult = true;
            try
            {
                //create source workspace name
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;


                //create source dataset name
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = nameOfSourceFeatureClass;


                //create target workspace name
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;


                //create target dataset name
                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = nameOfTargetFeatureClass;


                //Open input Featureclass to get field definitions.
                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
                IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();


                //Validate the field names because you are converting between different workspace types.
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IFields targetFeatureClassFields;
                IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
                IEnumFieldError enumFieldError;


                // Most importantly set the input and validate workspaces!
                fieldChecker.InputWorkspace = sourceWorkspace;
                fieldChecker.ValidateWorkspace = targetWorkspace;
                fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError, out targetFeatureClassFields);


                // Loop through the output fields to find the geomerty field
                IField geometryField;
                for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
                {
                    if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        geometryField = targetFeatureClassFields.get_Field(i);
                        // Get the geometry field's geometry defenition
                        IGeometryDef geometryDef = geometryField.GeometryDef;


                        //Give the geometry definition a spatial index grid count and grid size
                        IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;


                        targetFCGeoDefEdit.GridCount_2 = 1;
                        targetFCGeoDefEdit.set_GridSize(0, 0); //Allow ArcGIS to determine a valid grid size for the data loaded
                        targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;



                        // Load the feature class
                        IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                        //IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, queryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);
                        IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName, pQueryFilter, null, targetFeatureClassName, geometryDef, targetFeatureClassFields, "", 1000, 0);


                        break;
                    }
                }
            }catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                bResult = false;
            }
            return bResult;
        }


        /// <summary>
        /// 要素类添加到数据集中
        /// </summary>
        /// <param name="sourceWorkspace">源工作空间</param>
        /// <param name="targetWorkspace">目标工作空间</param>
        /// <param name="nameOfSourceFeatureClass">源要素类名称</param>
        /// <param name="nameOfTargetFeatureClass">目标要素类名称</param>
        /// <param name="pFeatureDataset">数据集对象</param>
        /// <returns></returns>
        public static bool ConvertFeatureClass2FeatureDataset(IWorkspace sourceWorkspace,
           IWorkspace targetWorkspace, string nameOfSourceFeatureClass,
           string nameOfTargetFeatureClass, IFeatureDataset pFeatureDataset, string whereClause)
        {
            try
            {
                IFeatureDatasetName pName = pFeatureDataset.FullName as IFeatureDatasetName;
                //create source workspace name 
                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
                //create source dataset name   
                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
                sourceDatasetName.Name = nameOfSourceFeatureClass;

                //create target workspace name   
                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
                //create target dataset name    
                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();

                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
                targetDatasetName.WorkspaceName = targetWorkspaceName;
                targetDatasetName.Name = nameOfTargetFeatureClass;

                //Open input Featureclass to get field definitions.  
                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
                IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
                //Validate the field names because you are converting between different workspace types.   
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IFields targetFeatureClassFields;
                IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
                IEnumFieldError enumFieldError;
                // Most importantly set the input and validate workspaces! 
                fieldChecker.InputWorkspace = sourceWorkspace;
                fieldChecker.ValidateWorkspace = targetWorkspace;
                fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError,
                    out targetFeatureClassFields);
                // Loop through the output fields to find the geomerty field   
                IField geometryField;
                for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
                {
                    if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        geometryField = targetFeatureClassFields.get_Field(i);
                        // Get the geometry field's geometry defenition          
                        IGeometryDef geometryDef = geometryField.GeometryDef;
                        //Give the geometry definition a spatial index grid count and grid size     
                        IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                        targetFCGeoDefEdit.GridCount_2 = 1;
                        targetFCGeoDefEdit.set_GridSize(0, 0);
                        //Allow ArcGIS to determine a valid grid size for the data loaded     
                        targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
                        // we want to convert all of the features    
                        IQueryFilter queryFilter = new QueryFilterClass();
                        queryFilter.WhereClause = whereClause;
                        // Load the feature class            
                        IFeatureDataConverter fctofc = new FeatureDataConverterClass();
                        IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName,
                            queryFilter, pName, targetFeatureClassName,
                            geometryDef, targetFeatureClassFields, "", 1000, 0);
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 到处为shp
        /// </summary>
        /// <param name="pInFeatureClass"></param>
        /// <param name="pPath"></param>
        /// <param name="whereClause"></param>
        public static void  ExportFeature(IFeatureClass pInFeatureClass, string pPath,string whereClause)
        {
            // create a new Access workspace factory       
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
            string parentPath = pPath.Substring(0, pPath.LastIndexOf('\\'));
            string fileName = pPath.Substring(pPath.LastIndexOf('\\') + 1, pPath.Length - pPath.LastIndexOf('\\') - 1);
            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(parentPath, fileName, null, 0);
            // Cast for IName       
            IName name = (IName)pWorkspaceName;
            //Open a reference to the access workspace through the name object       
            IWorkspace pOutWorkspace = (IWorkspace)name.Open();

            IDataset pInDataset = pInFeatureClass as IDataset;
            IFeatureClassName pInFCName = pInDataset.FullName as IFeatureClassName;
            IWorkspace pInWorkspace = pInDataset.Workspace;
            IDataset pOutDataset = pOutWorkspace as IDataset;
            IWorkspaceName pOutWorkspaceName = pOutDataset.FullName as IWorkspaceName;
            IFeatureClassName pOutFCName = new FeatureClassNameClass();
            IDatasetName pDatasetName = pOutFCName as IDatasetName;
            pDatasetName.WorkspaceName = pOutWorkspaceName;
            pDatasetName.Name = pInFeatureClass.AliasName;
            IFieldChecker pFieldChecker = new FieldCheckerClass();
            pFieldChecker.InputWorkspace = pInWorkspace;
            pFieldChecker.ValidateWorkspace = pOutWorkspace;
            IFields pFields = pInFeatureClass.Fields;
            IFields pOutFields;
            IEnumFieldError pEnumFieldError;
            pFieldChecker.Validate(pFields, out pEnumFieldError, out pOutFields);
            IFeatureDataConverter pFeatureDataConverter = new FeatureDataConverterClass();

            IQueryFilter pQF = null;
            if (whereClause.Trim() != "")
            {
                pQF = new QueryFilterClass();
                pQF.WhereClause = whereClause;
            }
            IField geometryField;
            for (int i = 0; i < pOutFields.FieldCount; i++)
            {
                if (pOutFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                {
                    geometryField = pOutFields.get_Field(i);
                    // Get the geometry field's geometry defenition          
                    IGeometryDef geometryDef = geometryField.GeometryDef;
                    //Give the geometry definition a spatial index grid count and grid size     
                    IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
                    targetFCGeoDefEdit.GridCount_2 = 1;
                    targetFCGeoDefEdit.set_GridSize(0, 0);
                    //Allow ArcGIS to determine a valid grid size for the data loaded     
                    targetFCGeoDefEdit.SpatialReference_2 = (pInFeatureClass as IGeoDataset).SpatialReference;
                    
                    break;
                }
            }

            pFeatureDataConverter.ConvertFeatureClass(pInFCName, pQF, null, pOutFCName, null, pOutFields, "", 100, 0);
        }

       


        /// <summary>
        /// 要素类重民命
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="newAliasName"></param>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public static bool RenameFeatureClassName(string newName, string newAliasName, IFeatureClass pFeatureClass)
        {
            IDataset ds = pFeatureClass as IDataset;
            bool isRename = false;
            try
            {
                IClassSchemaEdit2 pClassSchemaEdit2 = pFeatureClass as IClassSchemaEdit2;
                pClassSchemaEdit2.AlterAliasName(newAliasName);
                if (ds.CanRename())
                {
                    ds.Rename(newName);
                    isRename = true;
                }
            }
            catch
            {
                isRename = false;
            }

            return isRename;
        }

        /// <summary>
        /// 数据集从命名
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="pFeatureDataset"></param>
        /// <returns></returns>
        public static bool RenameDataset(string newName, IDataset pFeatureDataset)
        {
            bool isRename = false;
            try
            {
                IClassSchemaEdit2 pClassSchemaEdit2 = pFeatureDataset as IClassSchemaEdit2;
                if (pFeatureDataset.CanRename())
                {
                    pFeatureDataset.Rename(newName);
                    isRename = true;
                }
            }
            catch
            {
                isRename = false;
            }

            return isRename;
        }

        public static DataTable ITable2DataTable(IWorkspace pWorkspace, string sql)
        {

            IFDOToADOConnection fdoToadoConnection = new FdoAdoConnectionClass();
            //ADODB.Connection adoConnection = (ADODB.Connection)fdoToadoConnection.CreateADOConnection(ipWS);
            ADODB.Connection adoConnection = new ADODB.Connection();
            fdoToadoConnection.Connect(pWorkspace, adoConnection);

            ADODB.Recordset adoRecordSet = new ADODB.Recordset();
            adoRecordSet.Open(sql, adoConnection, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic, 0);

            OleDbDataAdapter custDA = new OleDbDataAdapter();
            DataTable dt = new DataTable("ArcGISTable");
            custDA.Fill(dt, adoRecordSet);

            adoRecordSet.Close();
            adoConnection.Close();
            adoConnection = null;
            adoRecordSet = null;
            return dt;
        }

    }



}
