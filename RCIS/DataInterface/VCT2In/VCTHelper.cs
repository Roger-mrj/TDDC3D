using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace RCIS.DataInterface.VCT2
{
    class VCTHelper
    {

        /// <summary>
        /// 创建shp文件
        /// </summary>
        /// <param name="destDir"></param>
        /// <param name="pSR"></param>
        /// <param name="eType"></param>
        /// <param name="sSXBM"></param>
        /// <param name="ghasTableStructure"></param>
        /// <returns></returns>
        public static IFeatureClass CreateShp(string destDir, ISpatialReference pSR, esriGeometryType eType, string sSXBM, TableStructBeginEnd curItem)
        {
            try
            {
                

                // 在gpFDS中建立FeatureClass: sSXBM
                if (!System.IO.Directory.Exists(destDir))
                {
                    System.IO.Directory.CreateDirectory(destDir);
                }
                IWorkspaceFactory pWorkSpaceFac = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace pFeatureWorkSpace = pWorkSpaceFac.OpenFromFile(destDir, 0) as IFeatureWorkspace;

                //首先检查该特性类是否存在、存在的话先删除:
                IEnumDataset pEnumDataset = ((IWorkspace)pFeatureWorkSpace).get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumDataset.Reset();
                IDataset pDataset = pEnumDataset.Next();
                while (pDataset != null)
                {
                    string sName = pDataset.Name;
                    if (sName.ToUpper().Equals(sSXBM.Trim().ToUpper()) == true)
                        pDataset.Delete();
                    pDataset = pEnumDataset.Next();
                }

                //建立字段集合:
                IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                ////建立字段:ObjectID
                IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                //pFieldEdit.Name_2 = "OBJECTID";
                //pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                //pFieldsEdit.AddField(pField);

                //create the geometry field
                pField = new FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "Shape";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                IGeometryDef pGeoDef = new GeometryDefClass();
                IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
                pGeoDefEdit.SpatialReference_2 = pSR;
                pGeoDefEdit.GridCount_2 = 1;
                pGeoDefEdit.set_GridSize(0, 0.5);
                pGeoDefEdit.AvgNumPoints_2 = 2;
                pGeoDefEdit.HasM_2 = false;
                pGeoDefEdit.HasZ_2 = false;
                pGeoDefEdit.GeometryType_2 = eType;
                pFieldEdit.GeometryDef_2 = pGeoDef;
                pFieldsEdit.AddField(pField);

                //加其他的字段...
                for (int i = 0; i < curItem.aZDMCs.Count; i++)
                {
                    pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                    pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Name_2 = (string)curItem.aZDMCs[i];
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;   //缺省

                    string sZDName = (string)curItem.aZDMCs[i];
                    string sType = (string)curItem.aZDLXs[i];
                    if (sType.Equals("CHAR"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFieldEdit.Length_2 = (int)curItem.aZDJD[i];
                    }
                    else if (sType.Equals("DATE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                    }
                    else if (sType.Equals("DOUBLE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        pFieldEdit.Precision_2 = (int)curItem.aZDJD[i];
                        pFieldEdit.Scale_2 = (int)curItem.aZDJD2[i];
                    }
                    else if (sType.Equals("INT"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                        //int nLen= (int)curItem.aZDJD[i];
                        //pFieldEdit.Precision_2 = nLen;
                    }

                    pFieldsEdit.AddField(pField);
                }

                // locate the shape field,CreateFeatureClass()需要
                string sShapeFieldName = "";
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        sShapeFieldName = pFields.get_Field(i).Name;
                        break;
                    }
                }
                //Create FeatureClass
                IFeatureClass newFeatureClassC = pFeatureWorkSpace.CreateFeatureClass(sSXBM.ToUpper().Trim(), pFields, null, null, esriFeatureType.esriFTSimple, sShapeFieldName, "");
                return newFeatureClassC;
            }
            catch (Exception E)
            {
                return null;
            }
        }


        /// <summary>
        /// 创建要素类
        /// </summary>
        /// <param name="eType"></param>
        /// <param name="sSXBM"></param>
        /// <param name="ghasTableStructure"></param>
        /// <returns></returns>
        public static bool CreateFeatureClass(IFeatureDataset gpFDS, esriGeometryType eType, string sSXBM,TableStructBeginEnd curItem)
        {
            //gpWS是全局IWorkspace;
            //gpFDS是全局的IFeatureDataset, 针对的"TDDC";
            try
            {
                

                // 在gpFDS中建立FeatureClass: sSXBM
                ISpatialReference pSR = ((IGeoDataset)gpFDS).SpatialReference;
                esriGeometryType NewFeatureClassType = eType;
                string sNewFeatureClassName = sSXBM.Trim().ToUpper();
                

                //检查SXBM是否已经存在[有时VCT文件中几个要素共用一个表]
                IFeatureClassContainer FeatCon = (IFeatureClassContainer)gpFDS;
                IEnumFeatureClass featClasses = FeatCon.Classes;
                featClasses.Reset();
                IFeatureClass myFeatClass = featClasses.Next();
                bool bExist = false;
                while (myFeatClass != null)
                {
                    IDataset curDS = myFeatClass as IDataset;
                    string sName = curDS.Name.Trim().ToUpper();
                    if (sName.Equals(sNewFeatureClassName))
                    {
                        bExist = true;
                        break;
                    }
                    myFeatClass = featClasses.Next();
                }
                if (bExist)
                    return true;

                //建立字段集合:
                IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                //建立字段:ObjectID
                IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "OBJECTID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldsEdit.AddField(pField);

                //create the geometry field
                pField = new FieldClass();
                pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "Shape";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                IGeometryDef pGeoDef = new GeometryDefClass();
                IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
                pGeoDefEdit.SpatialReference_2 = pSR;
                pGeoDefEdit.GridCount_2 = 1;
                pGeoDefEdit.set_GridSize(0, 0.5);
                pGeoDefEdit.AvgNumPoints_2 = 2;
                pGeoDefEdit.HasM_2 = false;
                pGeoDefEdit.HasZ_2 = false;
                pGeoDefEdit.GeometryType_2 = NewFeatureClassType;
                pFieldEdit.GeometryDef_2 = pGeoDef;
                pFieldsEdit.AddField(pField);

                //加其他的字段...
                for (int i = 0; i < curItem.aZDMCs.Count; i++)
                {
                    pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                    pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Name_2 = (string)curItem.aZDMCs[i];
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;   //缺省

                    string sZDName = (string)curItem.aZDMCs[i];
                    string sType = (string)curItem.aZDLXs[i];
                    if (sType.Equals("CHAR"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFieldEdit.Length_2 = (int)curItem.aZDJD[i];
                    }
                    else if (sType.Equals("DATE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                    }
                    else if (sType.Equals("DOUBLE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        pFieldEdit.Precision_2 = (int)curItem.aZDJD[i];
                        pFieldEdit.Scale_2 = (int)curItem.aZDJD2[i];
                    }
                    else if (sType.Equals("INT"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                        //int nLen= (int)curItem.aZDJD[i];
                        //pFieldEdit.Precision_2 = nLen;
                    }

                    pFieldsEdit.AddField(pField);
                }

                // locate the shape field,CreateFeatureClass()需要
                string sShapeFieldName = "";
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        sShapeFieldName = pFields.get_Field(i).Name;
                        break;
                    }
                }
                //Create FeatureClass
                IFeatureClass newFeatureClassC = gpFDS.CreateFeatureClass(sNewFeatureClassName, pFields, null, null, esriFeatureType.esriFTSimple, sShapeFieldName, "");
                return true;
            }
            catch (Exception E)
            {
                return false;
            }
            //... ...
        }


        public static bool CreateSHPGdal( string destDir, string type, string sSxbm, TableStructBeginEnd curItem)
        {
            
         
            if (!System.IO.Directory.Exists(destDir))
            {
                System.IO.Directory.CreateDirectory(destDir);
            }
            //再目标目录下创建shp文件
            string shpFile = destDir + "\\" +sSxbm+".SHP";
            //OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            //// 为了使属性表字段支持中文，请添加下面这句
            //OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
            // 注册所有的驱动
            OSGeo.OGR.Ogr.RegisterAll();

            //创建数据，这里以创建ESRI的shp文件为例
            string strDriverName = "ESRI Shapefile";
            int count = OSGeo.OGR.Ogr.GetDriverCount();
            OSGeo.OGR.Driver oDriver = OSGeo.OGR.Ogr.GetDriverByName(strDriverName);
            if (oDriver == null)
            {
                return false;
            }

            // 创建数据源
            OSGeo.OGR.DataSource oDS = oDriver.CreateDataSource(shpFile, null);
            if (oDS == null)
            {
                return false;
            }

            // 创建图层，创建一个多边形图层，这里没有指定空间参考，如果需要的话，需要在这里进行指定
            OSGeo.OGR.Layer oLayer = null;
            if (type.ToUpper() == "POINT")
            {
                oLayer=oDS.CreateLayer(sSxbm, null, OSGeo.OGR.wkbGeometryType.wkbPoint, null);
            }
            else if (type.ToUpper() == "LINE")
            {
                oLayer = oDS.CreateLayer(sSxbm, null, OSGeo.OGR.wkbGeometryType.wkbLineString, null);
            }
            else if (type.ToUpper() == "POLYGON")
            {
                oLayer = oDS.CreateLayer(sSxbm, null, OSGeo.OGR.wkbGeometryType.wkbPolygon, null);
            }
            
            if (oLayer == null)
            {
                return false;
            }

            // 下面创建属性表

            for (int i = 0; i < curItem.aZDMCs.Count; i++)
            {
                OSGeo.OGR.FieldDefn oField = null;
                string sZDName = (string)curItem.aZDMCs[i];
                string sType = (string)curItem.aZDLXs[i];
                if (sType.Equals("CHAR"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName, OSGeo.OGR.FieldType.OFTString);
                    oField.SetWidth( (int)curItem.aZDJD[i]);
                }
                else if (sType.Equals("DATE"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName,OSGeo.OGR.FieldType.OFTDate);
                }
                else if (sType.Equals("DOUBLE"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName,OSGeo.OGR.FieldType.OFTReal);
                    oField.SetPrecision((int)curItem.aZDJD[i]);
                   
                }
                else if (sType.Equals("INT"))
                {
                    oField = new OSGeo.OGR.FieldDefn(sZDName, OSGeo.OGR.FieldType.OFTInteger);
                    
                }
                oLayer.CreateField(oField, 1);
            }
            if (oLayer!=null) oLayer.Dispose();
            if (oDS!=null) oDS.Dispose();
            return true;
 
        }


        /// <summary>
        /// 创建属性表
        /// </summary>
        /// <param name="gpWS"></param>
        /// <param name="sTableName"></param>
        /// <param name="ghasTableStructure"></param>
        /// <returns></returns>
        public static bool CreateAttrTable(IWorkspace gpWS,   string sTableName, Hashtable ghasTableStructure)
        {
            //gpWS是全局IWorkspace;
            try
            {
                TableStructBeginEnd curItem = (TableStructBeginEnd)ghasTableStructure[sTableName];

                // 在gpWS中建立属性表:sTableName
                IFeatureWorkspace pWS = gpWS as IFeatureWorkspace;

                //检查当前的属性表是否已经存在[有时VCT文件中几个要素共用一个表]:
                IEnumDataset pEnumDS = gpWS.get_Datasets(esriDatasetType.esriDTAny);
                pEnumDS.Reset();
                IDataset pCurDS = pEnumDS.Next();
                bool bExist = false;
                while (pCurDS != null)
                {
                    string sName = pCurDS.Name.Trim().ToUpper();
                    string ss = sTableName.Trim().ToUpper();
                    if (sName.Equals(ss) == true)
                    {
                        bExist = true;
                        break;
                    }
                    pCurDS = pEnumDS.Next();
                }
                if (bExist)
                    return true;

                //建立字段集合:
                IFields pFields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                //建立字段:ObjectID
                IField pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;
                pFieldEdit.Name_2 = "OBJECTID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldsEdit.AddField(pField);

                //加其他的字段...
                for (int i = 0; i < curItem.aZDMCs.Count; i++)
                {
                    pField = new ESRI.ArcGIS.Geodatabase.FieldClass();
                    pFieldEdit = pField as IFieldEdit;
                    pFieldEdit.Name_2 = (string)curItem.aZDMCs[i];
                    pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;   //缺省

                    string sZDName = (string)curItem.aZDMCs[i];
                    string sType = (string)curItem.aZDLXs[i];
                    if (sType.Equals("CHAR"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        pFieldEdit.Length_2 = (int)curItem.aZDJD[i];
                    }
                    else if (sType.Equals("DATE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                    }
                    else if (sType.Equals("DOUBLE"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        pFieldEdit.Precision_2 = (int)curItem.aZDJD[i];
                        pFieldEdit.Scale_2 = (int)curItem.aZDJD2[i];
                    }
                    else if (sType.Equals("INT"))
                    {
                        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                        //int nLen= (int)curItem.aZDJD[i];
                        //pFieldEdit.Precision_2 = nLen;
                    }

                    pFieldsEdit.AddField(pField);
                }

                ITable createdTable = pWS.CreateTable(sTableName, pFields, null, null, "");
                return true;
            }
            catch (Exception E)
            {
                throw E;
            }
            //... ...
        }
    
    
    }
}
