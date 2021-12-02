using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace RCIS.GISCommon
{
    public class MosaicDatasetHelper
    {
        /// <summary> 
        /// 创建镶嵌数据集 
        /// </summary> 
        /// <param name="pFgdbWorkspace">工作空间</param> 
        /// <param name="pMDame">名称</param> 
        /// <param name="pSrs">空间参考</param>
        /// <returns>镶嵌数据集</returns>
        public static IMosaicDataset CreateMosaicDataset(IWorkspace pFgdbWorkspace, string pMDame, ISpatialReference pSrs)
        {
            try
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactory();
                ICreateMosaicDatasetParameters pCreationPars = new CreateMosaicDatasetParametersClass();

                pCreationPars.BandCount = 3;
                pCreationPars.PixelType = rstPixelType.PT_UCHAR;
                IMosaicWorkspaceExtensionHelper pMosaicExentionHelper = new MosaicWorkspaceExtensionHelperClass();
                IMosaicWorkspaceExtension pMosaicExtention = pMosaicExentionHelper.FindExtension(pFgdbWorkspace);
                
                return pMosaicExtention.CreateMosaicDataset(pMDame, pSrs, pCreationPars, "DOM");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 创建镶嵌数据集
        /// </summary>
        /// <param name="pFgdbWorkspace"></param>
        /// <param name="pMDame"></param>
        /// <param name="pSrs"></param>
        /// <returns></returns>
        public static IMosaicDataset CreateSDEMosaicDataset(IWorkspace pFgdbWorkspace, string pMDame, ISpatialReference pSrs)
        {
            try
            {
                IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();// FileGDBWorkspaceFactory();
                ICreateMosaicDatasetParameters pCreationPars = new CreateMosaicDatasetParametersClass();

                pCreationPars.BandCount = 3;
                pCreationPars.PixelType = rstPixelType.PT_UCHAR;
                IMosaicWorkspaceExtensionHelper pMosaicExentionHelper = new MosaicWorkspaceExtensionHelperClass();
                IMosaicWorkspaceExtension pMosaicExtention = pMosaicExentionHelper.FindExtension(pFgdbWorkspace);

                return pMosaicExtention.CreateMosaicDataset(pMDame, pSrs, pCreationPars, "DOM");
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// 获取镶嵌数据集
        /// </summary>
        /// <param name="MosaicName">数据集名称</param>
        /// <param name="workspace">数据集所在工作空间</param>
        /// <returns>镶嵌数据集</returns>
        public static IMosaicDataset GetMosaicDataset(string MosaicName, IWorkspace workspace)
        {
            IMosaicDataset pMosicDataset = null;
            IMosaicWorkspaceExtensionHelper pMosaicWsExHelper = new MosaicWorkspaceExtensionHelperClass();
            IMosaicWorkspaceExtension pMosaicWsExt = pMosaicWsExHelper.FindExtension(workspace);
            if (pMosaicWsExt != null)
            {
                try
                {
                    pMosicDataset = pMosaicWsExt.OpenMosaicDataset(MosaicName);
                }
                catch (Exception ex)
                {
                    return pMosicDataset;
                }
            }
            return pMosicDataset;
        }


        /// <summary>
        /// 导出镶嵌数据集为删格数据
        /// </summary>
        /// <param name="RasterName">数据名称</param>
        /// <param name="workspaceDB">工作空间</param>
        /// <param name="DownLoadLocation">保存路径</param>
        /// <returns>成功返回true，失败返回false</returns>
        public static bool DownLoadMosaic(string RasterName, IWorkspace workspaceDB, string DownLoadLocation)
        {
            try
            {
                IWorkspace wsGDB = null;
                IWorkspaceFactory workspaceFactory = new FileGDBWorkspaceFactoryClass();
                //判断是GDB文件还是普通文件夹
                string locationForm = DownLoadLocation.Substring(DownLoadLocation.Length - 4, 4).ToUpper();
                if (locationForm == ".GDB")
                {
                    wsGDB = workspaceFactory.OpenFromFile(@"" + DownLoadLocation, 0);
                }
                else
                {
                    IRasterWorkspace rasterWorkspace =WorkspaceHelper2.GetWorkspace(DownLoadLocation) as IRasterWorkspace;
                    wsGDB = (IWorkspace)rasterWorkspace;
                }
                IMosaicWorkspaceExtensionHelper mosaicHelper = new MosaicWorkspaceExtensionHelperClass();
                IMosaicWorkspaceExtension mosaicWs = mosaicHelper.FindExtension(workspaceDB);
                IMosaicDataset mosaic = mosaicWs.OpenMosaicDataset(RasterName);
                IFunctionRasterDataset functionDS = (IFunctionRasterDataset)mosaic;

                ISaveAs rasterSaveAs = (ISaveAs)functionDS;
                if (locationForm == ".GDB")
                {
                    rasterSaveAs.SaveAs(RasterName, wsGDB, "GDB");
                }
                else
                {
                    rasterSaveAs.SaveAs(RasterName + ".tif", wsGDB, "TIFF");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 导入栅格数据
        /// </summary>
        /// <param name="filePath">导入文件路径</param>
        /// <param name="mosaicDataSet">镶嵌数据集</param>
        public static bool ImportRasterToMosaic(string filePath, IMosaicDataset mosaicDataSet)
        {
            try
            {
                IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                IRasterWorkspace rasterWorkspace = workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0) as IRasterWorkspace;
                IMosaicDatasetOperation mOp = (IMosaicDatasetOperation)mosaicDataSet;
                IAddRastersParameters addRs = new AddRastersParametersClass();
                IRasterDatasetCrawler rsDsetCrawl = new RasterDatasetCrawlerClass();
                rsDsetCrawl.RasterDataset = rasterWorkspace.OpenRasterDataset(System.IO.Path.GetFileName(filePath));
                IRasterTypeFactory rsFact = new RasterTypeFactoryClass();
                IRasterType rsType = rsFact.CreateRasterType("Raster dataset");
                rsType.FullName = rsDsetCrawl.DatasetName;
                addRs.Crawler = (IDataSourceCrawler)rsDsetCrawl;
                addRs.RasterType = rsType;
                mOp.AddRasters(addRs, null);
                //计算cellSize 和边界
                // Create a calculate cellsize ranges parameters object.
                ICalculateCellSizeRangesParameters computeArgs = new CalculateCellSizeRangesParametersClass();
                // Use the mosaic dataset operation interface to calculate cellsize ranges.
                mOp.CalculateCellSizeRanges(computeArgs, null);
                // Create a build boundary parameters object.
                IBuildBoundaryParameters boundaryArgs = new BuildBoundaryParametersClass();
                // Set flags that control boundary generation.
                boundaryArgs.AppendToExistingBoundary = true;
                // Use the mosaic dataset operation interface to build boundary.
                mOp.BuildBoundary(boundaryArgs, null);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        /// <summary>
        /// 删除镶嵌数据集
        /// </summary>
        /// <param name="rasterName">名称</param>
        /// <param name="workspace">工作空间</param>
        /// <returns>删除成功时返回true,否则返回false</returns>
        public static bool DeleteMosaic(string rasterName, IWorkspace workspace)
        {
            try
            {
                IMosaicWorkspaceExtensionHelper pMosaicWsExHelper = new MosaicWorkspaceExtensionHelperClass();
                IMosaicWorkspaceExtension pMosaicWsExt = pMosaicWsExHelper.FindExtension(workspace);

                IMosaicDataset theMosaicDataset=pMosaicWsExt.OpenMosaicDataset(rasterName);
                IMosaicDatasetOperation theMosaicDatasetOperation = (IMosaicDatasetOperation)  (theMosaicDataset);
                IRemoveItemsParameters removeArgs = new RemoveItemsParametersClass();
                removeArgs.DeleteOverviewImages = true;
                removeArgs.RemoveUnreferencedInstances = true;
                
                theMosaicDatasetOperation.RemoveItems(removeArgs,null);  //删除缩略图
                                
                pMosaicWsExt.DeleteMosaicDataset(rasterName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取镶嵌数据表
        /// </summary>
        /// <param name="pMosaicDataset">镶嵌数据集</param>
        /// <returns>镶嵌数据表</returns>
        public static ITable GetMosaicDatasetTable(IMosaicDataset pMosaicDataset)
        {
            ITable pTable = null;
            IEnumName pEnumName = pMosaicDataset.Children;
            pEnumName.Reset();
            ESRI.ArcGIS.esriSystem.IName pName;
            while ((pName = pEnumName.Next()) != null)
            {
                pTable = pName.Open() as ITable;
                int i = pTable.Fields.FieldCount;
                if (i >= 21) break;
            }
            return pTable;
        }



        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="theMosaicDataset"></param>
        public static  void BuildOverviewsOnMD(IMosaicDataset theMosaicDataset)
        {
            // The mosaic dataset operation interface is used to perform operations on 
            // a mosaic dataset.
            IMosaicDatasetOperation theMosaicDatasetOperation = (IMosaicDatasetOperation)
                (theMosaicDataset);

            // Create a define overview parameters object.
            IDefineOverviewsParameters defineOvArgs = new DefineOverviewsParametersClass();
            // Use the overview tile parameters interface to specify the overview factor
            // used to generate overviews.
            ((IOverviewTileParameters)defineOvArgs).OverviewFactor = 3;
            // Use the mosaic dataset operation interface to define overviews.
            theMosaicDatasetOperation.DefineOverviews(defineOvArgs, null);

            // Create a generate overviews parameters object.
            IGenerateOverviewsParameters genPars = new GenerateOverviewsParametersClass();
            // Set properties to control overview generation.
            IQueryFilter genQuery = new QueryFilterClass();
            ((ISelectionParameters)genPars).QueryFilter = genQuery;
            genPars.GenerateMissingImages = true;
            genPars.GenerateStaleImages = true;
            // Use the mosaic dataset operation interface to generate overviews.
            theMosaicDatasetOperation.GenerateOverviews(genPars, null);

            

        }

    }
}
