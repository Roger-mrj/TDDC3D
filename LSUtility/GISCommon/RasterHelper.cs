using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseDistributed;

using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.SpatialAnalystUI;
using ESRI.ArcGIS.GeoAnalyst;

namespace RCIS.GISCommon
{
    public class RasterHelper
    {
        //ArcGIS影像操作
        /// <summary>
        /// 影像切割by yl 2008.06.16 landgis@126.com,参考http://bbs.esrichina-bj.cn/ESRI/viewthread.php?tid=28659&extra=&page=1修改
        /// </summary>
        /// <param name="pRasterLayer">//要裁切的影像图层</param>
        /// <param name="FileName">文件名为.img</param>
        public static void RasterClip(IRasterLayer pRasterLayer, IPolygon clipGeo, string FileName)
        {

            IRaster pRaster = pRasterLayer.Raster;
            IRasterProps pProps = pRaster as IRasterProps;
            object cellSizeProvider = pProps.MeanCellSize().X;
            IGeoDataset pInputDataset = pRaster as IGeoDataset;
            IExtractionOp pExtractionOp = new RasterExtractionOpClass();
            IRasterAnalysisEnvironment pRasterAnaEnvir = pExtractionOp as IRasterAnalysisEnvironment;
            pRasterAnaEnvir.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, ref cellSizeProvider);
            object extentProvider = clipGeo.Envelope;
            object snapRasterData = Type.Missing;
            pRasterAnaEnvir.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, ref extentProvider, ref snapRasterData);
            IGeoDataset pOutputDataset = pExtractionOp.Polygon(pInputDataset, clipGeo as IPolygon, true);
            IRaster clipRaster;  //裁切后得到的IRaster

            if (pOutputDataset is IRasterLayer)
            {
                IRasterLayer rasterLayer = pOutputDataset as IRasterLayer;
                clipRaster = rasterLayer.Raster;
            }

            else if (pOutputDataset is IRasterDataset)
            {

                IRasterDataset rasterDataset = pOutputDataset as IRasterDataset;
                clipRaster = rasterDataset.CreateDefaultRaster();
            }

            else if (pOutputDataset is IRaster)
            {

                clipRaster = pOutputDataset as IRaster;
            }
            else
            {
                return;
            }
            

            //保存裁切后得到的clipRaster 
            //如果直接保存为img影像文件
            IWorkspaceFactory pWKSF = new RasterWorkspaceFactoryClass();
            IWorkspace pWorkspace = pWKSF.OpenFromFile(System.IO.Path.GetDirectoryName(FileName), 0);
            ISaveAs pSaveAs = clipRaster as ISaveAs;
            pSaveAs.SaveAs(System.IO.Path.GetFileName(FileName), pWorkspace, "IMAGINE Image");

        }

        ///
        /// 栅格分类专题图
        ///
        /// 栅格图层
        public static void funColorForRaster_Classify(IRasterLayer pRasterLayer)
        {
            IRasterClassifyColorRampRenderer pRClassRend = new RasterClassifyColorRampRenderer() as IRasterClassifyColorRampRenderer;
            IRasterRenderer pRRend = pRClassRend as IRasterRenderer;

            IRaster pRaster = pRasterLayer.Raster;
            IRasterBandCollection pRBandCol = pRaster as IRasterBandCollection;
            IRasterBand pRBand = pRBandCol.Item(0);
            if (pRBand.Histogram == null)
            {
                pRBand.ComputeStatsAndHist();
            }
            pRRend.Raster = pRaster;
            pRClassRend.ClassCount = 10;
            pRRend.Update();

            IRgbColor pFromColor = new RgbColor() as IRgbColor;
            pFromColor.Red = 255;
            pFromColor.Green = 0;
            pFromColor.Blue = 0;
            IRgbColor pToColor = new RgbColor() as IRgbColor;
            pToColor.Red = 0;
            pToColor.Green = 0;
            pToColor.Blue = 255;

            IAlgorithmicColorRamp colorRamp = new AlgorithmicColorRamp() as IAlgorithmicColorRamp;
            colorRamp.Size = 10;
            colorRamp.FromColor = pFromColor;
            colorRamp.ToColor = pToColor;
            bool createColorRamp;

            colorRamp.CreateRamp(out createColorRamp);

            IFillSymbol fillSymbol = new SimpleFillSymbol() as IFillSymbol;
            for (int i = 0; i < pRClassRend.ClassCount; i++)
            {
                fillSymbol.Color = colorRamp.get_Color(i);
                pRClassRend.set_Symbol(i, fillSymbol as ISymbol);
                pRClassRend.set_Label(i, pRClassRend.get_Break(i).ToString("0.00"));
            }
            pRasterLayer.Renderer = pRRend;
        }

        /// <summary>
        /// 坡向分析
        /// </summary>
        /// <param name="demFile"></param>
        /// <param name="outputFolder"></param>
        /// <param name="inputGeoDataset"></param>
        /// <param name="pWorkspace"></param>
        public static IRaster ProduceAspectData(string outputAspectName, string outputFolder, IGeoDataset inputGeoDataset, IWorkspace pWorkspace)
        {
            IRaster retRaster = null;
            ISurfaceOp pSurface = new RasterSurfaceOpClass();
            IGeoDataset outAspectDataset = pSurface.Aspect(inputGeoDataset);
            ISaveAs pAspectSaveAs = outAspectDataset as ISaveAs;

            string outputAspectFilePath = System.IO.Path.Combine(outputFolder, outputAspectName);
            if (System.IO.File.Exists(outputAspectFilePath))
            {
                IRasterDataset oldRasterDataset = (pWorkspace as IRasterWorkspace).OpenRasterDataset(outputAspectName);
                (oldRasterDataset as IDataset).Delete();
            }
            //IDataset aspectDataset = pAspectSaveAs.SaveAs(outputAspectName, pWorkspace, "GRID");
            IDataset aspectDataset = pAspectSaveAs.SaveAs(outputAspectName, pWorkspace, "IMG");
            IRasterDataset solpeRasterDataset = aspectDataset as IRasterDataset;
            if (solpeRasterDataset != null)
            {
                retRaster = solpeRasterDataset.CreateDefaultRaster();
            }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(outAspectDataset);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(aspectDataset);
            return retRaster;
        }

        

        /// <summary>
        /// 获取栅格统计
        /// </summary>
        /// <param name="pRaster"></param>
        /// <returns></returns>
        public static IRasterStatistics GetRasterStatistics(IRaster pRaster)
        {
            if (null == pRaster)
                return null;

            IRasterBandCollection pRBandCol = pRaster as IRasterBandCollection;
            IRasterBand pRBand = pRBandCol.Item(0);
            if (pRBand.Statistics == null)
            {
                pRBand.ComputeStatsAndHist();
            }
            return pRBand.Statistics;
        }

        /// <summary>
        /// 重分类
        /// </summary>
        /// <param name="demFile"></param>
        /// <param name="outputFolder"></param>
        /// <param name="pWorkspace"></param>
        /// <param name="pInRaster"></param>
        /// <returns></returns>
        public static IRaster ProduceReClass(string outputAspectName, string outputFolder,
        IWorkspace pWorkspace, IRaster pInRaster)
        {
            if (pInRaster == null)
            {
                return null;
            }
            IRaster retRaster = null;
            IReclassOp pReclassOp = new RasterReclassOpClass();
            IGeoDataset pGeodataset = pInRaster as IGeoDataset;
            INumberRemap pNumRemap = new NumberRemapClass();
            IRasterStatistics pRasterStatistic = RasterHelper.GetRasterStatistics(pInRaster);
            pNumRemap.MapRange(0, 2, 1);
            pNumRemap.MapRange(2, 6, 2);
            pNumRemap.MapRange(6, 15, 3);
            pNumRemap.MapRange(15, 25, 4);
            pNumRemap.MapRange(25, 90,5);
            
            IRemap pRemap = pNumRemap as IRemap;
            IRaster2 pOutRaster = pReclassOp.ReclassByRemap(pGeodataset, pRemap, false) as IRaster2;
            //保存
            ISaveAs pAspectSaveAs = pOutRaster as ISaveAs;
            
            if (System.IO.File.Exists(System.IO.Path.Combine(outputFolder, outputAspectName)))
            {
                IRasterDataset oldRasterDataset = (pWorkspace as IRasterWorkspace).OpenRasterDataset(outputAspectName);
                (oldRasterDataset as IDataset).Delete();
            }

            IDataset aspectDataset = pAspectSaveAs.SaveAs(outputAspectName, pWorkspace, "IMG");
            IRasterDataset solpeRasterDataset = aspectDataset as IRasterDataset;
            if (solpeRasterDataset != null)
            {
                retRaster = solpeRasterDataset.CreateDefaultRaster();
            }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(pInRaster);
            return retRaster;
        }

        



        /// <summary>
        /// 坡度分析
        /// </summary>
        /// <param name="demFile"></param>
        /// <param name="outputFolder"></param>
        /// <param name="strOutputMeasurement">"DEGREE"</param>
        /// <param name="zFactor">0</param>
        /// <param name="inputGeoDataset"></param>
        /// <param name="pWorkspace"></param>
        /// <returns></returns>
        public static IRaster ProduceSlopeData(string outputSlopeName, string outputFolder,
                string strOutputMeasurement, double zFactor, IGeoDataset inputGeoDataset, IWorkspace pWorkspace)
        {
            IRaster solpeRaster = null;
            ISurfaceOp pSurface = new RasterSurfaceOpClass();
            esriGeoAnalysisSlopeEnum enumSlope = (strOutputMeasurement == "DEGREE") ? esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopeDegrees :
                esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopePercentrise;
            IGeoDataset outSlopeDataset = pSurface.Slope(inputGeoDataset, enumSlope, zFactor);
            //输出坡度数据
            ISaveAs pSlopeSaveAs = outSlopeDataset as ISaveAs;
            if (outputSlopeName.Length > 13)
            {
                outputSlopeName = outputSlopeName.Substring(outputSlopeName.Length - 12, 12);
            }
            //string outputSlopeName = demFile.Parent.Name + "SLP.tif";

            if (System.IO.File.Exists(System.IO.Path.Combine(outputFolder, outputSlopeName)))
            {
                IRasterDataset oldRasterDataset = (pWorkspace as IRasterWorkspace).OpenRasterDataset(outputSlopeName);
                (oldRasterDataset as IDataset).Delete();
            }
            if (pSlopeSaveAs.CanSaveAs("IMG"))
            {

                IDataset solpeDataset = pSlopeSaveAs.SaveAs(outputSlopeName, pWorkspace, "IMG");
                //IDataset solpeDataset = pSlopeSaveAs.SaveAs(outputSlopeName, pWorkspace, "TIFF");
                IRasterDataset solpeRasterDataset = solpeDataset as IRasterDataset;
                if (solpeRasterDataset != null)
                {
                    solpeRaster = solpeRasterDataset.CreateDefaultRaster();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(solpeRasterDataset);
            }
            
           

            System.Runtime.InteropServices.Marshal.ReleaseComObject(outSlopeDataset);            
            return solpeRaster;
        }

    
    }
}
