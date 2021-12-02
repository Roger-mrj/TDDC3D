using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using System.IO;

namespace RCIS.GISCommon
{
    public class WorkspaceHelper2
    {
        public static string GetFeatureClassPath(IWorkspace pWS, string FeatureClassName)
        {
            string fcPath = "";
            if (!(pWS as IWorkspace2).NameExists[esriDatasetType.esriDTFeatureClass, FeatureClassName]) return fcPath;
            using (ESRI.ArcGIS.ADF.ComReleaser comRel = new ESRI.ArcGIS.ADF.ComReleaser())
            {
                IFeatureClass pFeatureClass = (pWS as IFeatureWorkspace).OpenFeatureClass(FeatureClassName);
                comRel.ManageLifetime(pFeatureClass);
                IFeatureDataset pFeatureDataset = pFeatureClass.FeatureDataset;
                if (pFeatureDataset == null)
                {
                    fcPath = pWS.PathName + "\\" + (pFeatureClass as IDataset).Name;
                }
                else
                {
                    fcPath = pWS.PathName + "\\" + pFeatureDataset.Name + "\\" + (pFeatureClass as IDataset).Name;
                }
            }
            return fcPath;
        }

        public static IFeatureClass CreateSHP(string shpName, esriGeometryType geoType, ISpatialReference pSR, IFields pFields = null)
        {
            IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
            IFeatureWorkspace pWS = (IFeatureWorkspace)pWSF.OpenFromFile(System.IO.Path.GetDirectoryName(shpName), 0);
            if (pFields == null) pFields = CreateRequiredFields(geoType, pSR);
            return pWS.CreateFeatureClass(System.IO.Path.GetFileNameWithoutExtension(shpName), pFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
        }

        public static IFields CreateRequiredFields(esriGeometryType geoType, ISpatialReference pSR)
        {
            //创建IFeatureClassDescription接口
            IFeatureClassDescription pFeaClassDesc = new FeatureClassDescriptionClass();
            IObjectClassDescription pObjClassDesc = (IObjectClassDescription)pFeaClassDesc;

            // 获取所需的字段集合
            IFields pFields = pObjClassDesc.RequiredFields;

            // 获取几何字段
            int iShapeFieldIndex = pFields.FindField(pFeaClassDesc.ShapeFieldName);
            IField pShapeField = pFields.get_Field(iShapeFieldIndex);

            // 获取几何定义
            IGeometryDef pGeometryDef = pShapeField.GeometryDef;
            IGeometryDefEdit pGeometryDefEdit = (IGeometryDefEdit)pGeometryDef;

            // 修改要素类的集合类型为线（默认为面）
            pGeometryDefEdit.GeometryType_2 = geoType;
            
            //设置坐标系
            pGeometryDefEdit.SpatialReference_2 = pSR;

            return pFields;
        }

        /// <summary>
        /// 创建数据集
        /// </summary>
        /// <param name="pWS">创建数据集的工作空间</param>
        /// <param name="sDatasetName">数据集名称</param>
        /// <returns>是否创建成功</returns>
        public static IFeatureDataset CreateFeatrueDataset(IWorkspace pWS, string sDatasetName, ISpatialReference pSpatialReference)
        {
            IFeatureWorkspace pFeatureWorkspace = pWS as IFeatureWorkspace;
            IWorkspace2 pWorkspace2 = pWS as IWorkspace2;
            IFeatureDataset pFeatureDataset = null;
            if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureDataset, sDatasetName))
            {
                try
                {
                    //如果数据集存在，删除
                    pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(sDatasetName);
                    IEnumDataset pEnumDataset = pFeatureDataset.Subsets;
                    IDataset pDataset = pEnumDataset.Next();
                    while (pDataset != null)
                    {
                        if (pDataset.CanDelete())
                        {
                            pDataset.Delete();
                        }
                        pDataset = pEnumDataset.Next();
                    }
                    pFeatureDataset.Delete();
                   
                }
                catch (Exception ex)
                {
                    
                    return null;
                }
            }
            try
            {
                IFeatureDataset tmpDataset = pFeatureWorkspace.CreateFeatureDataset(sDatasetName, pSpatialReference);
               
                return tmpDataset;
            }
            catch (Exception exex)
            {
               
                return null;
            }
        }


        //SDEWorkspaceFromPropertySet         
        // Returns a reference to an existing workspace via a propertyset      
        // The connection parameters are passed in as arguements     
        //        //REFERENCES(REQUIRED)        
        //ESRI.ArcGIS.Geodatabase        //ESRI.ArcGIS.DataSourcesGDB    
        public static  ESRI.ArcGIS.Geodatabase.IWorkspace SDEWorkspaceFromPropertySet(String server, 
            String instance, String user,    
            String password, String database, 
            String version)        {
        // Create and populate the property set        
            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass(); 
            propertySet.SetProperty("SERVER", server);       
            propertySet.SetProperty("INSTANCE", instance);   
            propertySet.SetProperty("DATABASE", database);   
            propertySet.SetProperty("USER", user);       
            propertySet.SetProperty("PASSWORD", password); 
            propertySet.SetProperty("VERSION", version);   
            ESRI.ArcGIS.Geodatabase.IWorkspaceFactory2 workspaceFactory;   
            workspaceFactory = (ESRI.ArcGIS.Geodatabase.IWorkspaceFactory2)new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();      
            return workspaceFactory.Open(propertySet, 0);    
        }

        /// <summary>
        /// 创建PDDB工作空间
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public  static IWorkspace CreateAccessWorkspace(string sFilePath)
        {
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            IWorkspaceName workspaceName = workspaceFactory.Create(System.IO.Path.GetDirectoryName(sFilePath),
                System.IO.Path.GetFileNameWithoutExtension(sFilePath) + ".mdb", null, 0);
            ESRI.ArcGIS.esriSystem.IName name = (ESRI.ArcGIS.esriSystem.IName)workspaceName;
            //Open a reference to the access workspace through the name object        
            IWorkspace pGDB_workspace = (IWorkspace)name.Open();
            return pGDB_workspace;
        }

        /// <summary>
        /// 获得Shapefile文件的工作空间
        /// </summary>
        /// <param name="sFilePath">Shapefile文件路径</param>
        /// <returns></returns>
        public static IWorkspace GetShapefileWorkspace(string sFilePath)
        {
            try
            {
                IWorkspaceFactory pWSF = new ShapefileWorkspaceFactoryClass();
                System.IO.DirectoryInfo pDI = new System.IO.DirectoryInfo(sFilePath);
                if (!pDI.Exists)
                {
                    //表名不是目录
                    sFilePath = System.IO.Path.GetDirectoryName(sFilePath);
                }



                IWorkspace pWS = pWSF.OpenFromFile(sFilePath, 0);
                return pWS;
            }
            catch { return null; }
        }

        public static IFeatureClass GetShapefileFeatureClass(string sFilePath)
        {
            try
            {
                IWorkspace pWorkspace = GetShapefileWorkspace(System.IO.Path.GetDirectoryName(sFilePath));
                IFeatureClass pFeatureClass = (pWorkspace as IFeatureWorkspace).OpenFeatureClass(System.IO.Path.GetFileName(sFilePath));
                return pFeatureClass;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得MDB数据的工作空间
        /// </summary>
        /// <param name="sFilePath">MDB数据路径</param>
        /// <returns></returns>
        public static IWorkspace GetAccessWorkspace(string sFilePath)
        {
            if (!System.IO.File.Exists(sFilePath)) return null;
            
            try
            {
                //IPropertySet pPropertySet = new PropertySetClass();
                //pPropertySet.SetProperty("DATABASE", sFilePath);
                AccessWorkspaceFactory sWSF = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();

                return sWSF.OpenFromFile(sFilePath, 0);
            }
            catch(Exception ex) { return null; }
        }

        /// <summary>
        /// 获取filegdb 工作空间
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static IWorkspace GetFileGdbWorkspace(string sFilePath)
        {

            try
            {
                IWorkspaceFactory sWSf = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactory();
                return sWSf.OpenFromFile(sFilePath, 0);
            }
            catch(Exception ex)
            { return null; }
        }


        public static IWorkspace GetRasterWorkspace(string sDir)
        {
            try
            {
                IWorkspaceFactory pWF = new ESRI.ArcGIS.DataSourcesRaster.RasterWorkspaceFactory();
                return pWF.OpenFromFile(sDir, 0);
            }
            catch {
                return null;
            }
        }

        public static IWorkspace GetWorkspace(string sFilePath)
        {
            IWorkspace returnWs = null;
            //如果是个文件，扩展名为 mdb
            string ext = System.IO.Path.GetExtension(sFilePath);
            if (ext.ToUpper() == ".MDB")
            {
                returnWs = GetAccessWorkspace(sFilePath);
            }
            else if (ext.ToUpper().Trim()=="")
            {
                returnWs = GetShapefileWorkspace(sFilePath);
            }

            return returnWs;

        }


        /// <summary>
        /// 获取sqllite工作空间
        /// </summary>
        /// <param name="sfilePath"></param>
        /// <returns></returns>
        public static IWorkspace GetSqlliteWorkspace(string sfilePath)
        {
            try
            {
                IWorkspaceFactory pWrkspcFact = new SqlWorkspaceFactoryClass() as IWorkspaceFactory ;
                IWorkspace pFtrWrkspc = pWrkspcFact.OpenFromFile(sfilePath,0) as IWorkspace ;
                return pFtrWrkspc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Access工作空间名称
        /// </summary>
        /// <param name="workspacePath"></param>
        /// <returns></returns>
        public static IWorkspaceName GetWorkspaceName(string workspacePath)
        {
            try
            {
                //Creates a new workspace name for a personal geodatabase.
                IWorkspaceName workspaceName = new WorkspaceNameClass();
                workspaceName.WorkspaceFactoryProgID = "esriDataSourcesGDB.AccessWorkspaceFactory";
                workspaceName.PathName = workspacePath;

                return workspaceName;
            }
            catch (Exception ex) { return null; }
        }



        /// <summary>
        /// 创建内存控件
        /// </summary>
        /// <returns></returns>
        public static IWorkspace CreateInMemoryWorkspace()
        {
            // Create an in-memory workspace factory.
            Type factoryType = Type.GetTypeFromProgID(
              "esriDataSourcesGDB.InMemoryWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)
              Activator.CreateInstance(factoryType);

            // Create an in-memory workspace.
            IWorkspaceName workspaceName = workspaceFactory.Create("", "MyWorkspace",
              null, 0);

            // Cast for IName and open a reference to the in-memory workspace through the name object.
            IName name = (IName)workspaceName;
            IWorkspace workspace = (IWorkspace)name.Open();
            return workspace;
        }
        public static IWorkspace DeleteAndNewGDB(string gdbPath)
        {
            IWorkspace tmpWS = null;
            if (Directory.Exists(gdbPath))
            {
                try
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(gdbPath);
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                catch
                {
                    RCIS.Utility.FileHelper.DelectDir(gdbPath);
                    //删除空文件夹
                    Directory.Delete(gdbPath);
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(System.IO.Path.GetDirectoryName(gdbPath), System.IO.Path.GetFileName(gdbPath), null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(gdbPath);
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(System.IO.Path.GetDirectoryName(gdbPath), System.IO.Path.GetFileName(gdbPath), null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(gdbPath);
            }
            return tmpWS;
        }
        /// <summary>
        /// 创建临时gdb
        /// </summary>
        /// <returns></returns>
        public static IWorkspace DeleteAndNewTmpGDB()
        {
            string path = Application.StartupPath + "\\tmp\\tmp.gdb";
            IWorkspace tmpWS = null;

            if (Directory.Exists(path))
            {
                try
                {
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(path);
                    IEnumDataset pEnumDataset = tmpWS.get_Datasets(esriDatasetType.esriDTAny);
                    pEnumDataset.Reset();
                    IDataset pDataset;
                    while ((pDataset = pEnumDataset.Next()) != null)
                    {
                        pDataset.Delete();
                    }
                }
                catch
                {
                    RCIS.Utility.FileHelper.DelectDir(path);
                    //删除空文件夹
                    Directory.Delete(path);
                    IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                    pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                    tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
                }
            }
            else
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                pWorkspaceFactory.Create(Application.StartupPath + @"\tmp", "tmp.gdb", null, 0);
                tmpWS = RCIS.GISCommon.WorkspaceHelper2.GetFileGdbWorkspace(Application.StartupPath + @"\tmp\tmp.gdb");
            }
            return tmpWS;
        }

    }

}
