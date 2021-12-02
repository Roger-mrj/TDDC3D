using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

using System.Runtime.InteropServices;
using ESRI.ArcGIS.Output;
using System.Management;
using Microsoft.Win32;

namespace sycCommonLib
{
    /// <summary>
    /// sycCommonFuns 的摘要说明。
    /// </summary>
    public class sycCommonFuns : System.ComponentModel.Component
    {
        //私密成员定义处:

        public sycCommonFuns()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        protected override void Dispose(bool disposing)
        {
            // TODO:  添加 PolylineToPolygon.Dispose 实现
            if (disposing == true)
            {
                //仅释放本对象持有资源:
                ;
            }
            else
            {
                //释放未受管理资源:
                ;
            }
            base.Dispose(disposing);
        }
        ~sycCommonFuns()
        {
            Dispose(false);
        }


        //各公共函数:
        #region 公共函数


        //下面的2函数被其后面的函数调用:
        private void SetOutputQuality(IActiveView docActiveView, long iResampleRatio)
        {
            /* This function sets OutputImageQuality for the active view.  If the active view is a pagelayout, then
             * it must also set the output image quality for EACH of the Maps in the pagelayout.
             */
            IGraphicsContainer oiqGraphicsContainer;
            IElement oiqElement;
            IOutputRasterSettings docOutputRasterSettings;
            IMapFrame docMapFrame;
            IActiveView TmpActiveView;

            if (docActiveView is IMap)
            {
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
            }
            else if (docActiveView is IPageLayout)
            {
                //assign ResampleRatio for PageLayout
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
                //and assign ResampleRatio to the Maps in the PageLayout
                oiqGraphicsContainer = docActiveView as IGraphicsContainer;
                oiqGraphicsContainer.Reset();

                oiqElement = oiqGraphicsContainer.Next();
                while (oiqElement != null)
                {
                    if (oiqElement is IMapFrame)
                    {
                        docMapFrame = oiqElement as IMapFrame;
                        TmpActiveView = docMapFrame.Map as IActiveView;
                        docOutputRasterSettings = TmpActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                        docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
                    }
                    oiqElement = oiqGraphicsContainer.Next();
                }

                docMapFrame = null;
                oiqGraphicsContainer = null;
                TmpActiveView = null;
            }
            docOutputRasterSettings = null;
        }
        private IEnvelope GetGraphicsExtent(IActiveView docActiveView)
        {
            /* Gets the combined extent of all the objects in the map. */
            IEnvelope GraphicsBounds;
            IEnvelope GraphicsEnvelope;
            IGraphicsContainer oiqGraphicsContainer;
            IPageLayout docPageLayout;
            IDisplay GraphicsDisplay;
            IElement oiqElement;

            GraphicsBounds = new EnvelopeClass();
            GraphicsEnvelope = new EnvelopeClass();
            docPageLayout = docActiveView as IPageLayout;
            GraphicsDisplay = docActiveView.ScreenDisplay;
            oiqGraphicsContainer = docActiveView as IGraphicsContainer;
            oiqGraphicsContainer.Reset();

            oiqElement = oiqGraphicsContainer.Next();
            while (oiqElement != null)
            {
                oiqElement.QueryBounds(GraphicsDisplay, GraphicsEnvelope);
                GraphicsBounds.Union(GraphicsEnvelope);
                oiqElement = oiqGraphicsContainer.Next();
            }

            return GraphicsBounds;
        }

        public bool ExportActiveViewParameterized(IActiveView docActiveView, long iOutputResolution, long lResampleRatio, Boolean bClipToGraphicsExtent, string sOutFileName, ref string sRetErrorInfo)
        {
            /* EXPORT PARAMETER: (iOutputResolution) the resolution requested.
             * EXPORT PARAMETER: (lResampleRatio) Output Image Quality of the export.  The value here will only be used if the export
             * object is a format that allows setting of Output Image Quality, i.e. a vector exporter.
             * The value assigned to ResampleRatio should be in the range 1 to 5.
             * 1 corresponds to "Best", 5 corresponds to "Fast"
             * EXPORT PARAMETER: (ExportType) a string which contains the export type to create.
             * EXPORT PARAMETER: (sOutputDir) a string which contains the directory to output to.
             * EXPORT PARAMETER: (bClipToGraphicsExtent) Assign True or False to determine if export image will be clipped to the graphic 
             * extent of layout elements.  This value is ignored for data view exports
             */

            try
            {
                /* Exports the Active View of the document to selected output format. */
                IExport docExport;
                long iPrevOutputImageQuality;
                IOutputRasterSettings docOutputRasterSettings;
                IEnvelope PixelBoundsEnv;
                tagRECT exportRECT;
                tagRECT DisplayBounds;
                IDisplayTransformation docDisplayTransformation;
                IPageLayout docPageLayout;
                IEnvelope docMapExtEnv;
                long hdc;
                long tmpDC;
                string sNameRoot;
                long iScreenResolution;
                bool bReenable = false;

                IEnvelope docGraphicsExtentEnv;
                IUnitConverter pUnitConvertor;

                // The Export*Class() type initializes a new export class of the desired type.
                int nPos = sOutFileName.LastIndexOf('.');
                string ExportType = sOutFileName.Substring(nPos + 1).Trim().ToUpper();

                if (ExportType == "PDF")
                {
                    docExport = new ExportPDFClass();
                }
                else if (ExportType == "EPS")
                {
                    docExport = new ExportPSClass();
                }
                else if (ExportType == "AI")
                {
                    docExport = new ExportAIClass();
                }
                else if (ExportType == "BMP")
                {

                    docExport = new ExportBMPClass();
                }
                else if (ExportType == "TIFF")
                {
                    docExport = new ExportTIFFClass();
                }
                else if (ExportType == "SVG")
                {
                    docExport = new ExportSVGClass();
                }
                else if (ExportType == "PNG")
                {
                    docExport = new ExportPNGClass();
                }
                else if (ExportType == "GIF")
                {
                    docExport = new ExportGIFClass();
                }
                else if (ExportType == "EMF")
                {
                    docExport = new ExportEMFClass();
                }
                else if (ExportType == "JPEG" || ExportType == "JPG")
                {
                    docExport = new ExportJPEGClass();
                }
                else
                {
                    sRetErrorInfo = "错误: 不支持的图像类型 '" + ExportType + "' !";
                    return false;
                }


                // save the previous output image quality, so that when the export is complete it will be set back.
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                iPrevOutputImageQuality = docOutputRasterSettings.ResampleRatio;

                if (docExport is IExportImage)
                {
                    // always set the output quality of the DISPLAY to 1 for image export formats
                    SetOutputQuality(docActiveView, 1);
                }
                else
                {
                    // for vector formats, assign the desired ResampleRatio to control drawing of raster layers at export time   
                    SetOutputQuality(docActiveView, lResampleRatio);
                }

                //set the export filename (which is the nameroot + the appropriate file extension)
                docExport.ExportFileName = sOutFileName;

                /* Get the device context of the screen */
                tmpDC = (long)DLIB.GetDC((IntPtr)0);
                /* Get the screen resolution. */
                iScreenResolution = DLIB.GetDeviceCaps((IntPtr)tmpDC, 88); //88 is the win32 const for Logical pixels/inch in X)
                /* release the DC. */
                DLIB.ReleaseDC((IntPtr)0, (IntPtr)tmpDC);
                docExport.Resolution = iOutputResolution;

                if (docActiveView is IPageLayout)
                {
                    //get the bounds of the "exportframe" of the active view.
                    DisplayBounds = docActiveView.ExportFrame;
                    //set up pGraphicsExtent, used if clipping to graphics extent.
                    docGraphicsExtentEnv = GetGraphicsExtent(docActiveView);
                }
                else
                {
                    //Get the bounds of the deviceframe for the screen.
                    docDisplayTransformation = docActiveView.ScreenDisplay.DisplayTransformation;
                    DisplayBounds = docDisplayTransformation.get_DeviceFrame();
                }

                PixelBoundsEnv = new Envelope() as IEnvelope;
                if (bClipToGraphicsExtent && (docActiveView is IPageLayout))
                {
                    docGraphicsExtentEnv = GetGraphicsExtent(docActiveView);
                    docPageLayout = docActiveView as PageLayout;
                    pUnitConvertor = new UnitConverter();

                    //assign the x and y values representing the clipped area to the PixelBounds envelope
                    PixelBoundsEnv.XMin = 0;
                    PixelBoundsEnv.YMin = 0;
                    PixelBoundsEnv.XMax = pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.XMax, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution - pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.XMin, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution;
                    PixelBoundsEnv.YMax = pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.YMax, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution - pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.YMin, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution;

                    //'assign the x and y values representing the clipped export extent to the exportRECT
                    exportRECT.top = (int)(PixelBoundsEnv.YMin);
                    exportRECT.left = (int)(PixelBoundsEnv.XMin);
                    exportRECT.bottom = (int)(PixelBoundsEnv.YMax) + 1;
                    exportRECT.right = (int)(PixelBoundsEnv.XMax) + 1;

                    //since we're clipping to graphics extent, set the visible bounds.
                    docMapExtEnv = docGraphicsExtentEnv;
                }
                else
                {
                    double tempratio = iOutputResolution / iScreenResolution;
                    double tempbottom = DisplayBounds.bottom * tempratio;
                    double tempright = DisplayBounds.right * tempratio;
                    //'The values in the exportRECT tagRECT correspond to the width
                    //and height to export, measured in pixels with an origin in the top left corner.
                    exportRECT.bottom = (int)Math.Truncate(tempbottom);
                    exportRECT.left = 0;
                    exportRECT.top = 0;
                    exportRECT.right = (int)Math.Truncate(tempright);


                    //populate the PixelBounds envelope with the values from exportRECT.
                    // We need to do this because the exporter object requires an envelope object
                    // instead of a tagRECT structure.
                    PixelBoundsEnv.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);

                    //since it's a page layout or an unclipped page layout we don't need docMapExtEnv.
                    docMapExtEnv = null;
                }

                // Assign the envelope object to the exporter object's PixelBounds property.  The exporter object
                // will use these dimensions when allocating memory for the export file.
                docExport.PixelBounds = PixelBoundsEnv;

                // call the StartExporting method to tell docExport you're ready to start outputting.
                hdc = docExport.StartExporting();

                // Redraw the active view, rendering it to the exporter object device context instead of the app display.
                // We pass the following values:
                //  * hDC is the device context of the exporter object.
                //  * exportRECT is the tagRECT structure that describes the dimensions of the view that will be rendered.
                // The values in exportRECT should match those held in the exporter object's PixelBounds property.
                //  * docMapExtEnv is an envelope defining the section of the original image to draw into the export object.
                docActiveView.Output((int)hdc, (int)docExport.Resolution, ref exportRECT, docMapExtEnv, null);

                //finishexporting, then cleanup.
                docExport.FinishExporting();
                docExport.Cleanup();

                //set the output quality back to the previous value
                SetOutputQuality(docActiveView, iPrevOutputImageQuality);
                docMapExtEnv = null;
                PixelBoundsEnv = null;

                return true;
            }
            catch (Exception E)
            {
                sRetErrorInfo = "错误:" + E.Message + " !";
                return false;
            }
            return true;
        }


        public bool LSCT_IsOK()
        {
            //测试当前机器是否经过了城镇授权、能运行城镇的功能:
            string sType = "LSCT";
            string sJMC = CalJMString(sType);

            //对加密串进行变换、得到授权码与从公司得到的保存在注册中的授权码比较，一致时ＯＫ：
            //[01] 变换[注意，要和根据申请码计算授权码的程序一致]
            string sAuthCode = CalAuthCode(sJMC);

            string sPartA = "", sPartB = "";
            for (int i = 0; i < sAuthCode.Length; i++)
            {
                if (i % 2 == 0)
                    sPartA = sPartA + sAuthCode.Substring(i, 1);
                else if (i % 2 == 1)
                    sPartB = sPartB + sAuthCode.Substring(i, 1);
            }

            //[02] 从当前机器的Registry中得到授权码、比较:
            RegistryKey keyRead = Registry.CurrentUser;
            keyRead = keyRead.OpenSubKey("Software\\LSCT");
            if (keyRead == null)
                return false;

            object oKeyA = keyRead.GetValue("KeyA");
            object oKeyB = keyRead.GetValue("KeyB");
            if (oKeyA == null || oKeyB == null)
                return false;

            string sKeyA = (string)oKeyA;
            string sKeyB = (string)oKeyB;
            if (sPartA.Equals(sKeyA) == true && sPartB.Equals(sKeyB) == true)
                return true;
            else
                return false;
        }

        public string CalJMString(string sLSCTorLS)
        {
            //得到当前机器的:
            //Netcard NO+ CPU No+HardDisk NO 变换出一个申请码。
            //sLSCTorLS: LSCT或者LS


            //[01] network card Info:
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string sNeedStr = "";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    sNeedStr = mo["MacAddress"].ToString();
                mo.Dispose();
            }
            string sNetCardID = sNeedStr.ToUpper().Trim();

            //[02] cpu Info:
            string cupID = "";
            ManagementClass cimobject = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc2 = cimobject.GetInstances();
            foreach (ManagementObject mo in moc2)
                cupID = mo.Properties["ProcessorId"].Value.ToString().ToUpper().Trim();

            //[03] Harddisk Info:
            string HDID = "";
            cimobject = new ManagementClass("Win32_DiskDrive");
            moc2 = cimobject.GetInstances();
            foreach (ManagementObject mo in moc2)
                HDID = (string)mo.Properties["Model"].Value;
            HDID = HDID.ToUpper().Trim();

            string sLastInfo = "";
            string sTmp = sNetCardID + cupID + HDID;
            for (int i = 0; i < sTmp.Length; i++)
            {
                if (sTmp[i] == ':') ;
                else if (sTmp[i] == ' ') ;
                else
                    sLastInfo = sLastInfo + sTmp.Substring(i, 1);
            }

            //[04]把这些信息变换下:
            string sMC = "";
            Hashtable trans = new Hashtable();
            if (sLSCTorLS.Equals("LSCT") == true)
            {
                trans.Add("0", "101");
                trans.Add("1", "121");
                trans.Add("2", "131");
                trans.Add("3", "143");
                trans.Add("4", "145");
                trans.Add("5", "150");
                trans.Add("6", "161");
                trans.Add("7", "178");
                trans.Add("8", "188");
                trans.Add("9", "189");
                trans.Add("A", "201");
                trans.Add("B", "203");
                trans.Add("C", "205");
                trans.Add("D", "218");
                trans.Add("E", "220");
                trans.Add("F", "234");
                trans.Add("G", "243");
                trans.Add("H", "265");
                trans.Add("I", "278");
                trans.Add("J", "281");
                trans.Add("K", "283");
                trans.Add("L", "285");
                trans.Add("M", "291");
                trans.Add("N", "292");
                trans.Add("O", "295");
                trans.Add("P", "301");
                trans.Add("Q", "302");
                trans.Add("R", "314");
                trans.Add("S", "318");
                trans.Add("T", "333");
                trans.Add("U", "345");
                trans.Add("V", "365");
                trans.Add("W", "376");
                trans.Add("X", "389");
                trans.Add("Y", "394");
                trans.Add("Z", "400");

                for (int i = 0; i < sLastInfo.Length; i++)
                {
                    string ss = sLastInfo.Substring(i, 1);
                    string sFind = (string)trans[ss];
                    sMC = sMC + sFind;
                }
            }

            return sMC;
            //... ...
        }

        public string CalAuthCode(string sSQM)
        {
            //根据申请码、按照一定的算法计算出授权码:
            string s1 = "", s2 = "", s3 = "";
            for (int i = 0; i < sSQM.Length; i++)
            {
                if (i % 3 == 0)
                    s1 = s1 + sSQM.Substring(i, 1);
                else if (i % 3 == 1)
                    s2 = s2 + sSQM.Substring(i, 1);
                else if (i % 3 == 2)
                    s3 = s3 + sSQM.Substring(i, 1);
            }

            string sValue = "";
            for (int i = 0; i < s1.Length; i++)
            {
                string ss = s1.Substring(i, 1) + s2.Substring(i, 1) + s3.Substring(i, 1);
                long nn = Convert.ToInt64(ss);
                string sResult = Convert.ToString(nn, 16);
                sValue = sValue + sResult.ToUpper();
            }

            return sValue; //授权码
        }

        private void XY2LB(double dCenterJ, double dFalse_Easting, double dX, double dY, ref double dJ, ref double dB)
        {
            //计算(dX,dY)该点所对应的经纬度(dJ,dB)[80坐标系参数]:
            //dCenterJ: 中央经线、单位：度; dX,dY输入, 大地坐标; dJ,dB返回、弧度单位.
            double x = dX, y = dY;

            double ParaK0 = 1.57048687472752E-07;
            double ParaK1 = 5.05250559291393E-03;
            double ParaK2 = 2.98473350966158E-05;
            double ParaK3 = 2.41627215981336E-07;
            double ParaK4 = 2.22241909461273E-09;
            double ParaE1 = 6.69438499958795E-03;
            double ParaE2 = 6.73950181947292E-03;
            double ParaC = 6399596.65198801;

            double y1 = y - dFalse_Easting;
            double e = ParaK0 * x;
            double se = Math.Sin(e);
            double bf = e + Math.Cos(e) * (ParaK1 * se - ParaK2 * Math.Pow(se, 3) + ParaK3 * Math.Pow(se, 5) - ParaK4 * Math.Pow(se, 7));

            double t = Math.Tan(bf);
            double n1 = ParaE1 * Math.Pow(Math.Cos(bf), 2);
            double v = Math.Sqrt(1 + n1);
            double N = ParaC / v;
            double yn = y1 / N;
            double vt = Math.Pow(v, 2) * t;
            double t2 = Math.Pow(t, 2);
            double B = bf - vt * Math.Pow(yn, 2) / 2 + (5 + 3 * t2 + n1 - 9 * n1 * t2) * vt * Math.Pow(yn, 4) / 24 - (61 + 90 * t2 + 45 * Math.Pow(t2, 2)) * vt * Math.Pow(yn, 6) / 720;
            double cbf = 1.0 / Math.Cos(bf);
            double L = cbf * yn - (1 + 2 * t2 + n1) * cbf * Math.Pow(yn, 3) / 6 + (5 + 28 * t2 + 24 * Math.Pow(t2, 2) + 6 * n1 + 8 * n1 * t2) * cbf * Math.Pow(yn, 5) / 120 + dCenterJ * Math.PI / 180.0;
            dJ = L;
            dB = B;
            return;
        }

        private double Cal80EllipseArea(double dCenterJ, double dFalse_Easting, IPointCollection curPC, out string sErrorInfo)
        {
            //计算面的椭球面积子程序, 被CalTQMJ主程序调用。
            //计算按照80坐标系参数计算的点组成的面的椭球面积:
            //返回负数表示出现了错误。
            //dCenterJ: 中央经线、度; dFalse_Easting: 坐标系的偏移、如:40500000[前2为带号]
            double dSumArea = -1.0;

            sErrorInfo = "";
            if (curPC.PointCount < 3)
            {
                sErrorInfo = "点数少于3、无法计算其面积!";
                return dSumArea;
            }

            //[01] 计算每点对应的经纬度:
            int nPGS = curPC.PointCount;
            IPointCollection lbPC = new MultipointClass();    //X=精度，Y=纬度，弧度单位
            for (int i = 0; i < nPGS; i++)
            {
                IPoint pp = curPC.get_Point(i);
                double dX = pp.X;
                double dY = pp.Y;

                //反算(X,Y)对应的经纬度:(弧度)
                double dJ = 0.0, dB = 0.0;
                XY2LB(dCenterJ, dFalse_Easting, dY, dX, ref dJ, ref dB);

                IPoint lbP = new PointClass();
                lbP.X = dJ;
                lbP.Y = dB;
                object oo = Type.Missing;
                lbPC.AddPoint(lbP, ref oo, ref oo);
            }

            //按照中国的范围拦截下计算的错误:
            //中国地区经纬度范围为：纬度,20-52度；经度，72-133度
            bool bOK = true;
            for (int i = 0; i < lbPC.PointCount; i++)
            {
                IPoint pp = lbPC.get_Point(i);
                double dJ = pp.X * 180.0 / Math.PI;
                double dW = pp.Y * 180.0 / Math.PI;
                if (dW > 20 && dW < 52 && dJ > 72 && dJ < 133) ;
                else
                {
                    bOK = false;
                    break;
                }
            }
            if (bOK == false)
            {
                sErrorInfo = "计算的经纬度超出了范围(中国区域)!";
                return dSumArea;
            }


            //[02] 计算每2点投影到某个经线后组成的面的面积[这调用公式?]:
            dSumArea = 0.0;
            double aRadius = 637814;
            double bRadius = 6356755.29;
            double ParaAF = 1.0 / 298.257;
            double ParaE2 = 6.73950181947292E-03;
            double e = ParaE2;
            double ParaC = aRadius / (1 - ParaAF);
            double ParamA = 1 + (3.0 / 6.0) * e + (30.0 / 80.0) * Math.Pow(e, 2.0) + (35.0 / 112.0) * Math.Pow(e, 3) + (630.0 / 2304.0) * Math.Pow(e, 4.0);
            double ParamB = (1.0 / 6.0) * e + (15.0 / 80.0) * Math.Pow(e, 2.0) + (21.0 / 112.0) * Math.Pow(e, 3.0) + (420.0 / 2304.0) * Math.Pow(e, 4.0);
            double ParamC = (3.0 / 80.0) * Math.Pow(e, 2.0) + (7.0 / 112.0) * Math.Pow(e, 3.0) + (180.0 / 2304.0) * Math.Pow(e, 4.0);
            double ParamD = (1.0 / 112.0) * Math.Pow(e, 3.0) + (45.0 / 2304.0) * Math.Pow(e, 4.0);
            double ParamE = (5.0 / 2304.0) * Math.Pow(e, 4.0);
            for (int i = 0; i < lbPC.PointCount - 1; i++)
            {
                IPoint pp = lbPC.get_Point(i);
                double dJ1 = pp.X;
                double dB1 = pp.Y;   //是上面计算的、经纬度、弧度

                pp = lbPC.get_Point(i + 1);
                double dJ2 = pp.X;
                double dB2 = pp.Y;   //是上面计算的、经纬度、弧度

                double B = dB1;
                double L = dJ1;
                double B1 = dB2;
                double L1 = dJ2;
                double bDiference = B1 - B;
                double bSum = (B1 + B) / 2.0;
                double lDiference = (L1 + L) / 2.0;

                double ItemValue0 = ParamA * Math.Sin(bDiference / 2.0) * Math.Cos(bSum);
                double ItemValue1 = ParamB * Math.Sin(3.0 * bDiference / 2.0) * Math.Cos(3.0 * bSum);
                double ItemValue2 = ParamC * Math.Sin(5.0 * bDiference / 2.0) * Math.Cos(5.0 * bSum);
                double ItemValue3 = ParamD * Math.Sin(7.0 * bDiference / 2.0) * Math.Cos(7.0 * bSum);
                double ItemValue4 = ParamE * Math.Sin(9.0 * bDiference / 2.0) * Math.Cos(9.0 * bSum);
                double AreaVal = 2.0 * bRadius * lDiference * bRadius * (ItemValue0 - ItemValue1 + ItemValue2 - ItemValue3 + ItemValue4);

                dSumArea = dSumArea + AreaVal;
            }

            return Math.Abs(dSumArea);
            //... ...
        }

        //public double syc_CalTQMJ(ISpatialReference pSR, IFeature CalPolygonFeature, out string sErrorInfo)
        //{
        //    //计算面的椭球面积主程序。
        //    //CalPolygonFeature: 要计算的面特性, Input。用户必须保证该面的合法性，闭合规范。
        //    //pSR: 该特性所在数据集的坐标系, 应该是西安80系，否则按照CalPolygonFeature的XY反算就不应该。
        //    //本函数均是按照80系的参数计算的、老张提供的理论依据。[2008/04/09]

        //    double dRetArea = -1.0;
        //    sErrorInfo = "";

        //    //[01] 得到坐标系内的参数：
        //    double dCenterJ = 0.0;	    //DU
        //    double dFalse_Easting = 0.0, dFalse_North = 0.0;
        //    if (pSR is IProjectedCoordinateSystem)
        //    {
        //        IProjectedCoordinateSystem pProjectedCoordSys = pSR as IProjectedCoordinateSystem;
        //        string sPRJName = "投影名称: " + pProjectedCoordSys.Projection.Name;
        //        string sCentral_Meridian = "中央经线: " + pProjectedCoordSys.get_CentralMeridian(true).ToString();
        //        dCenterJ = pProjectedCoordSys.get_CentralMeridian(true);
        //        string sFalse_Easting = "False East: " + pProjectedCoordSys.FalseEasting.ToString();
        //        dFalse_Easting = pProjectedCoordSys.FalseEasting;
        //        string sFalse_Northing = "False North: " + pProjectedCoordSys.FalseNorthing.ToString();
        //        dFalse_North = pProjectedCoordSys.FalseNorthing;
        //        string sUnit = "坐标单位: " + pProjectedCoordSys.CoordinateUnit.Name;
        //    }
        //    else
        //    {
        //        sErrorInfo = "非投影坐标系!";
        //        return dRetArea;
        //    }

        //    //[02] 得到该面的点数组:
        //    IFeature pFeat = CalPolygonFeature;
        //    IGeometryCollection pGCol = pFeat.ShapeCopy as IGeometryCollection;
        //    int nGS = pGCol.GeometryCount;
        //    ArrayList areaSz = new ArrayList();
        //    for (int i = 0; i < nGS; i++)
        //    {
        //        IRing curRing = pGCol.get_Geometry(i) as IRing;
        //        IPointCollection curPC = curRing as IPointCollection;
        //        double dEllipseArea = Cal80EllipseArea(dCenterJ, dFalse_Easting, curPC, out sErrorInfo);
        //        if (dEllipseArea < 0.0)
        //            return dRetArea;

        //        areaSz.Add(dEllipseArea);
        //    }

        //    //[03] 找最大的面积、然后减去其内的小面积、得到最后的面积:
        //    double dLastArea = 0.0;
        //    if (areaSz.Count > 1)
        //    {
        //        double dMaxArea = 0.0;
        //        int nPos = -1;
        //        for (int i = 0; i < areaSz.Count; i++)
        //        {
        //            double dd = (double)areaSz[i];
        //            if (dd > dMaxArea)
        //            {
        //                dMaxArea = dd;
        //                nPos = i;
        //            }
        //        }
        //        for (int i = 0; i < areaSz.Count; i++)
        //        {
        //            double dd = (double)areaSz[i];
        //            if (i != nPos)
        //            {
        //                dMaxArea = dMaxArea - dd;
        //            }
        //        }
        //        dLastArea = dMaxArea;
        //    }
        //    else
        //    {
        //        dLastArea = (double)areaSz[0];
        //    }

        //    return dLastArea;
        //}

        //public double syc_CalTQMJ02(ISpatialReference pSR, IGeometry CalPolygonGeo, out string sErrorInfo)
        //{
        //    //计算面的椭球面积主程序。
        //    //CalPolygonGeo: 要计算的面特性, Input。用户必须保证该面的合法性，闭合规范。
        //    //pSR: 该特性所在数据集的坐标系, 应该是西安80系，否则按照CalPolygonFeature的XY反算就不应该。
        //    //本函数均是按照80系的参数计算的、老张提供的理论依据。[2008/04/09]

        //    double dRetArea = -1.0;
        //    sErrorInfo = "";

        //    //[01] 得到坐标系内的参数：
        //    double dCenterJ = 0.0;	    //DU
        //    double dFalse_Easting = 0.0, dFalse_North = 0.0;
        //    if (pSR is IProjectedCoordinateSystem)
        //    {
        //        IProjectedCoordinateSystem pProjectedCoordSys = pSR as IProjectedCoordinateSystem;
        //        string sPRJName = "投影名称: " + pProjectedCoordSys.Projection.Name;
        //        string sCentral_Meridian = "中央经线: " + pProjectedCoordSys.get_CentralMeridian(true).ToString();
        //        dCenterJ = pProjectedCoordSys.get_CentralMeridian(true);
        //        string sFalse_Easting = "False East: " + pProjectedCoordSys.FalseEasting.ToString();
        //        dFalse_Easting = pProjectedCoordSys.FalseEasting;
        //        string sFalse_Northing = "False North: " + pProjectedCoordSys.FalseNorthing.ToString();
        //        dFalse_North = pProjectedCoordSys.FalseNorthing;
        //        string sUnit = "坐标单位: " + pProjectedCoordSys.CoordinateUnit.Name;
        //    }
        //    else
        //    {
        //        sErrorInfo = "非投影坐标系!";
        //        return dRetArea;
        //    }

        //    //[02] 得到该面的点数组:            
        //    IGeometryCollection pGCol = CalPolygonGeo as IGeometryCollection;
        //    if (pGCol == null)
        //        return dRetArea;
        //    int nGS = pGCol.GeometryCount;
        //    ArrayList areaSz = new ArrayList();
        //    for (int i = 0; i < nGS; i++)
        //    {
        //        IRing curRing = pGCol.get_Geometry(i) as IRing;
        //        IPointCollection curPC = curRing as IPointCollection;
        //        double dEllipseArea = Cal80EllipseArea(dCenterJ, dFalse_Easting, curPC, out sErrorInfo);
        //        if (dEllipseArea < 0.0)
        //            return dRetArea;

        //        areaSz.Add(dEllipseArea);
        //    }

        //    //[03] 找最大的面积、然后减去其内的小面积、得到最后的面积:
        //    double dLastArea = 0.0;
        //    if (areaSz.Count > 1)
        //    {
        //        double dMaxArea = 0.0;
        //        int nPos = -1;
        //        for (int i = 0; i < areaSz.Count; i++)
        //        {
        //            double dd = (double)areaSz[i];
        //            if (dd > dMaxArea)
        //            {
        //                dMaxArea = dd;
        //                nPos = i;
        //            }
        //        }
        //        for (int i = 0; i < areaSz.Count; i++)
        //        {
        //            double dd = (double)areaSz[i];
        //            if (i != nPos)
        //            {
        //                dMaxArea = dMaxArea - dd;
        //            }
        //        }
        //        dLastArea = dMaxArea;
        //    }
        //    else
        //    {
        //        dLastArea = (double)areaSz[0];
        //    }

        //    return dLastArea;
        //}

        public bool syc_CreateBuffer(IGeometry pSourceGeo, double dBufferDist, ref IPolygon retBufferPolygon)
        {
            //根据几何pSourceGeo+dBufferDist建立Buffer返回:
            try
            {
                if ((pSourceGeo.GeometryType == esriGeometryType.esriGeometryPoint) ||
                    (pSourceGeo.GeometryType == esriGeometryType.esriGeometryPolyline) ||
                    (pSourceGeo.GeometryType == esriGeometryType.esriGeometryPolygon)) ;
                else return false;

                ITopologicalOperator pTopo = pSourceGeo as ITopologicalOperator;
                IGeometry newBufferGeo = pTopo.Buffer(dBufferDist);
                if (newBufferGeo == null)
                    return false;
                else
                {
                    retBufferPolygon = newBufferGeo as IPolygon;
                    pTopo = retBufferPolygon as ITopologicalOperator;
                    pTopo.Simplify();
                }
                return true;
            }
            catch (Exception E)
            {
                return false;
            }
        }

        public bool syc_CreateSingleSideBuffer(IPolyline pSourcePolyline, double dBufferDist, ref IPolygon retBufferPolygon)
        {
            //在先pLine前进方向的左侧、产生单边Buffer区域:
            try
            {
                IPolyline newPolyline = new PolylineClass();
                IConstructCurve pcc = newPolyline as IConstructCurve;
                object offType = esriConstructOffsetEnum.esriConstructOffsetMitered;
                object offBevel = Type.Missing;
                pcc.ConstructOffset(pSourcePolyline, dBufferDist, ref offType, ref offBevel);

                //构造面
                retBufferPolygon = new PolygonClass();
                ISegmentCollection pSC = retBufferPolygon as ISegmentCollection;
                ISegmentCollection pSourceSC = pSourcePolyline as ISegmentCollection;
                object oo = Type.Missing;
                for (int i = 0; i < pSourceSC.SegmentCount; i++)
                {
                    pSC.AddSegment(pSourceSC.get_Segment(i), ref oo, ref oo);
                }
                ILine pline = new LineClass();
                pline.FromPoint = pSourcePolyline.ToPoint;
                pline.ToPoint = newPolyline.ToPoint;
                pSC.AddSegment(pline as ISegment, ref oo, ref oo);

                newPolyline.ReverseOrientation();
                ISegmentCollection pSC2 = newPolyline as ISegmentCollection;
                for (int i = 0; i < pSC2.SegmentCount; i++)
                {
                    pSC.AddSegment(pSC2.get_Segment(i), ref oo, ref oo);
                }
                pline = new LineClass();
                pline.FromPoint = newPolyline.ToPoint;
                pline.ToPoint = pSourcePolyline.FromPoint;
                pSC.AddSegment(pline as ISegment, ref oo, ref oo);

                ITopologicalOperator pTopo = retBufferPolygon as ITopologicalOperator;
                pTopo.Simplify();
                return true;
            }
            catch (Exception E)
            {
                return false;
            }
        }

        public bool DrawSymbolToGC(Graphics myGC, ISymbol mySymbol, int nGap, ref string sError)
        {
            //本函数把符号绘制到myGS，边缘有nGap个间隙
            try
            {
                //[01] First clear the existing device context.
                myGC.Clear(Color.White);

                //[02]: Create the Transformation and Geometry required by ISymbol::Draw.
                //[02]-A: Create a new display transformation and set its properties.
                RectangleF rect = myGC.VisibleClipBounds;
                int nW = (int)rect.Width;
                int nH = (int)rect.Height;
                IEnvelope pBoundsEnvelope = new EnvelopeClass();
                pBoundsEnvelope.PutCoords(0, 0, nW, nH);
                tagRECT deviceRect;
                deviceRect.left = 0;
                deviceRect.top = 0;
                deviceRect.right = nW;
                deviceRect.bottom = nH;

                IDisplayTransformation pDisplayTransformation = new DisplayTransformationClass();
                pDisplayTransformation.VisibleBounds = pBoundsEnvelope;
                pDisplayTransformation.Bounds = pBoundsEnvelope;
                pDisplayTransformation.set_DeviceFrame(ref deviceRect);
                pDisplayTransformation.Resolution = myGC.DpiY;

                //[02]-B: create an appropriate Geometry type depending 
                //			   on the Symbol type passed in.
                IEnvelope pEnvelope = new EnvelopeClass();
                pEnvelope.PutCoords(nGap, nGap, nW - nGap, nH - nGap);
                IGeometry pGeo = null;
                if (mySymbol is IMarkerSymbol)
                {
                    IArea pArea = pEnvelope as IArea;
                    pGeo = pArea.Centroid;
                }
                else if (mySymbol is ILineSymbol || mySymbol is ITextSymbol)
                {
                    IPolyline pPolyline = new PolylineClass();
                    pPolyline.FromPoint = pEnvelope.LowerLeft;
                    pPolyline.ToPoint = pEnvelope.UpperRight;
                    pGeo = pPolyline;
                }
                else
                {
                    pGeo = pEnvelope;
                }

                //[03]: Perform the Draw operation.
                ITransformation pTransformation = pDisplayTransformation as ITransformation;
                IntPtr hdc = myGC.GetHdc();
                int nHDC = hdc.ToInt32();
                mySymbol.SetupDC(nHDC, pTransformation);
                mySymbol.Draw(pGeo);
                mySymbol.ResetDC();
                myGC.ReleaseHdc(hdc);
            }
            catch (Exception E)
            {
                sError = "DrawSymbolToGC Error: " + E.Message + " !";
                return false;
            }

            return true;
        }

        public bool DrawSymbolToFile(ISymbol mySymbol, int nW, int nH, int nGap, string sJPEGFile, ref string sError)
        {
            //把符号mySymbol以nW*nH[Pixels]写入文件sJPEGFile,
            //该文件应该是以'.jpeg.结尾的全名，nGap是边界预留的间隔。
            try
            {
                Image myImage = new Bitmap(nW, nH);
                Graphics myGC = Graphics.FromImage(myImage);
                bool bRet = DrawSymbolToGC(myGC, mySymbol, nGap, ref sError);
                if (!bRet)
                    return false;

                myImage.Save(sJPEGFile, ImageFormat.Jpeg);
                myGC.Dispose();
            }
            catch (Exception E)
            {
                sError = "DrawSymboToFile Error: " + E.Message + " !";
                return false;
            }
            return true;
        }

        public bool DrawSymbolToImage(ISymbol mySymbol, int nW, int nH, int nGap, out Image retImage, ref string sError)
        {
            //把符号mySymbol以nW*nH的像素宽度产生图像retImage返回。
            retImage = null;
            try
            {
                retImage = new Bitmap(nW, nH);
                Graphics myGC = Graphics.FromImage(retImage);
                bool bRet = DrawSymbolToGC(myGC, mySymbol, nGap, ref sError);
                if (!bRet)
                    return false;
            }
            catch (Exception E)
            {
                sError = "DrawSymboToFile Error: " + E.Message + " !";
                return false;
            }
            return true;
        }

        public ISymbol syc_CreateDefaultSymbol(esriGeometryType esriType)
        {
            //建立制定特性类缺省的符号:
            ISymbol sym = null;
            if (esriType == esriGeometryType.esriGeometryPoint)
            {
                ISimpleMarkerSymbol markerSym = new SimpleMarkerSymbolClass();
                markerSym.Color = syc_RGBColor(255, 0, 0);
                markerSym.Size = 5;
                markerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                sym = (ISymbol)markerSym;
            }
            else if (esriType == esriGeometryType.esriGeometryPolyline)
            {
                ISimpleLineSymbol lineSym = new SimpleLineSymbolClass();
                lineSym.Style = esriSimpleLineStyle.esriSLSSolid;
                lineSym.Width = 2;	//1
                lineSym.Color = syc_RGBColor(255, 0, 0);
                sym = (ISymbol)lineSym;
            }
            else if (esriType == esriGeometryType.esriGeometryPolygon)
            {
                ILineSymbol outlineSym = new SimpleLineSymbolClass();
                outlineSym.Color = syc_RGBColor(255, 0, 0);
                outlineSym.Width = 0.1;

                ISimpleFillSymbol smpFillSym = new SimpleFillSymbolClass();
                smpFillSym.Style = esriSimpleFillStyle.esriSFSDiagonalCross;	//.esriSFSHollow;	//中间什么也不画
                smpFillSym.Outline = outlineSym;
                smpFillSym.Color = syc_RGBColor(255, 0, 0);		//122,122,122
                sym = (ISymbol)smpFillSym;
            }

            return sym;
        }

        public double syc_CalAngle(ref IPoint p1, ref IPoint p2)
        {
            ILine pLine = new LineClass();
            pLine.FromPoint = p1;
            pLine.ToPoint = p2;
            return pLine.Angle;		//弧度
        }
        public double syc_CalAngle(ref PointClass p1, ref PointClass p2)
        {
            ILine pLine = new LineClass();
            pLine.FromPoint = p1;
            pLine.ToPoint = p2;
            return pLine.Angle;		//弧度
        }
        public double syc_CalLength(ref IPoint p1, ref IPoint p2)
        {
            ILine pLine = new LineClass();
            pLine.FromPoint = p1;
            pLine.ToPoint = p2;
            return pLine.Length;
        }
        public double syc_CalLength(ref PointClass p1, ref PointClass p2)
        {
            ILine pLine = new LineClass();
            pLine.FromPoint = p1;
            pLine.ToPoint = p2;
            return pLine.Length;
        }

        public bool syc_XY2JWD(IMap myMap, IPoint pp, out double dJ, out double dW, out string sRetErrorInfo)
        {
            dJ = dW = 0.0;
            sRetErrorInfo = "";

            //根据XY-->JWD:
            ISpatialReference pSR = myMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pSR as IProjectedCoordinateSystem;
            IGeographicCoordinateSystem pGCS = pPCS.GeographicCoordinateSystem;
            if (pGCS == null || pPCS == null)
            {
                sRetErrorInfo = "坐标系统有问题，请检查[ErrorCode:01]!";
                return false;
            }

            //double falseX,falseY,xyUnits;
            //falseX=falseY=xyUnits=0.0;
            //pPCS.GetFalseOriginAndUnits(ref falseX,ref falseY,ref xyUnits);
            //double dFE=pPCS.FalseEasting;		//maybe: 500000.0 or 0.0

            IPoint PP = new PointClass();
            PP.PutCoords(pp.X, pp.Y);
            IGeometry pGeo = PP as IGeometry;
            pGeo.SpatialReference = pPCS;
            pGeo.Project((ISpatialReference)pGCS);

            double dJingD = PP.X * 3.141592654 / 180.0;
            double dWeiD = PP.Y * 3.141592654 / 180.0;
            dJ = DLIB.HD2DFM(dJingD);
            dW = DLIB.HD2DFM(dWeiD);
            if (dJ > 72.0 && dJ < 133.0 && dW > 20.0 && dW < 52.0) ;
            else
            {
                sRetErrorInfo = "坐标系统有问题，请检查[ErrorCode:02]!";
                return false;
            }
            return true;
        }

        public bool syc_GetSRInfo(IMap myMap, out string[] sInfo, out string sError)
        {
            ISpatialReference pSR = myMap.SpatialReference;

            sError = "错误信息:";
            sInfo = new string[6];
            if (pSR is IProjectedCoordinateSystem)
            {
                double dXMin, dXMax, dYMin, dYMax;
                pSR.GetDomain(out dXMin, out dXMax, out dYMin, out dYMax);

                int nX1 = Convert.ToInt32(dXMin);
                int nY1 = Convert.ToInt32(dYMin);
                int nX3 = Convert.ToInt32(dXMax);
                int nY3 = Convert.ToInt32(dYMax);

                IProjectedCoordinateSystem pProjectedCoordSys = pSR as IProjectedCoordinateSystem;
                string sPRJName = "投影名称: " + pProjectedCoordSys.Projection.Name;
                string sCentral_Meridian = "中央经线: " + pProjectedCoordSys.get_CentralMeridian(true).ToString();
                string sFalse_Easting = "False East: " + pProjectedCoordSys.FalseEasting.ToString();
                string sFalse_Northing = "False North: " + pProjectedCoordSys.FalseNorthing.ToString();
                string sUnit = "坐标单位: " + pProjectedCoordSys.CoordinateUnit.Name;
                string sFW = "(" + nX1.ToString() + "," + nY1.ToString() + ")--(" + nX3.ToString() + "," + nY3.ToString() + ")";

                sInfo[0] = sPRJName;
                sInfo[1] = sCentral_Meridian;
                sInfo[2] = sFalse_Easting;
                sInfo[3] = sFalse_Northing;
                sInfo[4] = sUnit;
                sInfo[5] = sFW;

                return true;
            }
            else
            {
                sError = sError + "非投影坐标系 或者 未知坐标系!";
                return false;
            }
        }

        public bool syc_JWD2XY(IMap myMap, double dJ, double dW, ref IPoint retP, out string sRetErrorInfo)
        {
            sRetErrorInfo = "";

            //根据JWD-->XY
            ISpatialReference pSR = myMap.SpatialReference;
            IProjectedCoordinateSystem pPCS = pSR as IProjectedCoordinateSystem;
            IGeographicCoordinateSystem pGCS = pPCS.GeographicCoordinateSystem;
            if (pGCS == null || pPCS == null)
            {
                sRetErrorInfo = "坐标系统有问题，请检查[ErrorCode:01]!";
                return false;
            }

            double dDJ = DLIB.DFM2D(dJ);

            double ddj2 = (180 / Math.PI) * dJ;

            double dDW = DLIB.DFM2D(dW);

            double ddw2 = (180 / Math.PI) * dW;

            IPoint PP = new PointClass();
            PP.PutCoords(dDJ, dDW);
            IGeometry pGeo = PP as IGeometry;
            pGeo.SpatialReference = pGCS;
            pGeo.Project((ISpatialReference)pPCS);

            //double falseX,falseY,xyUnits;
            //falseX=falseY=xyUnits=0.0;
            //pPCS.GetFalseOriginAndUnits(ref falseX,ref falseY,ref xyUnits);
            retP.X = PP.X;
            retP.Y = PP.Y;

            return true;
        }

        public string syc_GetCOMErrorDescription(Exception E, out int nRetErrCode)
        {
            //主要处理fdoerror:
            string sDesc = "";
            int nErrCode = ((System.Runtime.InteropServices.ExternalException)(((System.Runtime.InteropServices.COMException)(E)))).ErrorCode;
            nRetErrCode = nErrCode;

            #region 各种fdoerror
            switch (nErrCode)
            {
                case -2147220991:
                    sDesc = "[FDO_E_LOADING_RESOURCE]Failed to load a resource (string, icon, bitmap, etc).";
                    break;
                case -2147220990:
                    sDesc = "[FDO_E_INDEX_OUT_OF_RANGE]The index passed was not within the valid range.";
                    break;
                case -2147220989:
                    sDesc = "[FDO_E_NOT_SUPPORTED]The operation is not supported by this implementation.";
                    break;
                case -2147220988:
                    sDesc = "[FDO_E_NOT_ENOUGH_SPACE]There is not enough storage space to complete the operation.";
                    break;
                case -2147220987:
                    sDesc = "[FDO_E_NO_PERMISSION]The user does not have permission to execute the operation.";
                    break;
                case -2147220986:
                    sDesc = "[FDO_E_IMPLEMENTATION]Signals that an implementation specific error has occurred and that the client should inspect the error object for additional errors. For example, SDE API errors.";
                    break;
                case -2147220985:
                    sDesc = "[FDO_E_INVALID_SQL]An invalid SQL statement was used.";
                    break;
                case -2147220984:
                    sDesc = "[FDO_E_NETWORK]A networking error occurred.";
                    break;
                case -2147220983:
                    sDesc = "[FDO_E_DATE_CONVERSION]A date conversion error has occurred.";
                    break;
                case -2147220982:
                    sDesc = "[FDO_E_OBJECT_IS_DELETED]The object has been deleted and is no longer valid.";
                    break;
                case -2147220981:
                    sDesc = "[FDO_E_WORKSPACE_NOT_COMPATIBLE]The workspace is of the wrong type.";
                    break;
                case -2147220980:
                    sDesc = "[FDO_E_OBJECT_IS_READONLY]Modifications to the object are not allowed.";
                    break;
                case -2147220979:
                    sDesc = "[FDO_E_OBJECT_IN_USE]Object is busy.";
                    break;
                case -2147220978:
                    sDesc = "[FDO_E_OBJECT_MAX_REACHED]Maximum number of objects reached.";
                    break;
                case -2147220977:
                    sDesc = "[FDO_E_OBJECT_IS_LOCKED]Object is currently locked.";
                    break;
                case -2147220976:
                    sDesc = "[FDO_E_INVALID_ENVELOPE]Invalid envelope encountered.";
                    break;
                case -2147220975:
                    sDesc = "[FDO_E_FILE_IO]File read/write error occurred.";
                    break;
                case -2147220974:
                    sDesc = "[FDO_E_LICENSE_FAILURE]A product licensing error occurred.";
                    break;
                case -2147220973:
                    sDesc = "[FDO_E_DBMS_ERROR]An underlying database error occurred.";
                    break;
                case -2147220972:
                    sDesc = "[FDO_E_COERCING]An error occurred trying to coerce data from one type to another.";
                    break;
                case -2147220971:
                    sDesc = "[FDO_E_BINDING]A general data binding error occurred.";
                    break;
                case -2147220970:
                    sDesc = "[FDO_E_SCHEMA_LOCK_CONFLICT]Cannot acquire a schema lock because of an existing lock.";
                    break;
                case -2147220969:
                    sDesc = "[FDO_E_MUST_BE_OWNER]Must be the owner to do this operation.";
                    break;
                case -2147220968:
                    sDesc = "[FDO_E_ESRI_PROVIDER_CONNECT_INVALID]Connection to ESRI OLE DB provider is invalid.";
                    break;
                case -2147220967:
                    sDesc = "[FDO_E_CONNECTION_CANCELLED]SDE Connection dialog is cancelled.";
                    break;
                case -2147220966:
                    sDesc = "[FDO_E_INVALID_RELEASE]This release of the Geodatabase is not up to date.";
                    break;
                case -2147220965:
                    sDesc = "[FDO_E_NO_SYSTEM_TABLES]Geodatabase System Tables not found.";
                    break;
                case -2147220964:
                    sDesc = "[FDO_E_CONNECT_PARAMETERS_CONFLICT]Conflicting connection parameters.";
                    break;
                case -2147220963:
                    sDesc = "[FDO_E_FIELDINFO_SYSTEM_TABLE_INCONSISTENCY]Geodatabase FieldInfo system table inconsistent.";
                    break;
                case -2147220962:
                    sDesc = "[FDO_E_NO_EDIT_LICENSE]The application is not licensed to edit this type of data .";
                    break;
                case -2147220961:
                    sDesc = "[FDO_E_NO_SCHEMA_LICENSE]The application is not licensed to create or modify schema for this type of data.";
                    break;
                case -2147220960:
                    sDesc = "[FDO_E_NO_OPERATION_LICENSE]The application does not have the required license for this operation.";
                    break;
                case -2147220959:
                    sDesc = "[FDO_E_OPERATION_CANNOT_BE_UNDONE]The current operation cannot be undone.";
                    break;
                case -2147220958:
                    sDesc = "[FDO_E_EDIT_OPERATION_REQUIRED]The current operation requires an edit operation.";
                    break;
                case -2147220957:
                    sDesc = "[FDO_E_RECONCILE_CANNOT_BE_UNDONE]The reconcile operation cannot be undone.";
                    break;
                case -2147220956:
                    sDesc = "[FDO_E_OBJECT_NOT_INITIALIZED]The object is not initialized.";
                    break;
                case -2147220955:
                    sDesc = "[FDO_E_INTEGER_REQUIRES_64BITS]The integer requires a 64-bit representation.";
                    break;
                case -2147220954:
                    sDesc = "[FDO_E_SYNTAX_ERROR]Syntax error.";
                    break;
                case -2147220911:
                    sDesc = "[FDO_E_WORKSPACE_NOT_CONNECTED]The workspace is not connected.";
                    break;
                case -2147220910:
                    sDesc = "[FDO_E_WORKSPACE_ALREADY_CONNECTED]The workspace is already connected.";
                    break;
                case -2147220909:
                    sDesc = "[FDO_E_SERVER_NOT_FOUND]The server was not found.";
                    break;
                case -2147220908:
                    sDesc = "[FDO_E_SERVER_NOT_AVAILABLE]The server was found, but is not available at this time.";
                    break;
                case -2147220907:
                    sDesc = "[FDO_E_SERVER_MAX_CONNECTIONS]The server does not allow anymore connections at this time.";
                    break;
                case -2147220906:
                    sDesc = "[FDO_E_USER_INVALID]The user and/or password is invalid.";
                    break;
                case -2147220905:
                    sDesc = "[FDO_E_USER_NOACCESS]The user does not have access to the workspace.";
                    break;
                case -2147220904:
                    sDesc = "[FDO_E_DATABASE_NOT_FOUND]The database was not found.";
                    break;
                case -2147220903:
                    sDesc = "[FDO_E_DATABASE_NOT_AVAILABLE]The database was found, but is not available at this time.";
                    break;
                case -2147220902:
                    sDesc = "[FDO_E_WORKSPACE_ALREADY_EXISTS]The workspace already exists.";
                    break;
                case -2147220901:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_CREATE_FAILED]Unable to instantiate workspace extension component.";
                    break;
                case -2147220900:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_INIT_FAILED]Unable to initialize workspace extension.";
                    break;
                case -2147220899:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_DATASET_CREATE_FAILED]Failed sending dataset created notification to workspace extension.";
                    break;
                case -2147220898:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_DATASET_RENAME_FAILED]Failed sending dataset renamed notification to workspace extension.";
                    break;
                case -2147220897:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_DATASET_DELETE_FAILED]Failed sending dataset deleted notification to workspace extension.";
                    break;
                case -2147220896:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_DUP_NAME]Illegal duplicate workspace extension name.";
                    break;
                case -2147220895:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_DUP_GUID]Illegal duplicate workspace extension guid.";
                    break;
                case -2147220894:
                    sDesc = "[FDO_E_WORKSPACE_EXTENSION_NO_REG_PRIV]Altering workspace extension registration requires Geodatabase DBA priveleges.";
                    break;
                case -2147220893:
                    sDesc = "[FDO_E_WORKSPACE_READONLY]Workspace or data source is read only.";
                    break;
                case -2147220892:
                    sDesc = "[FDO_E_DATASET_NOT_SUPPORTED_AT_WORKSPACE_LEVEL]The dataset is not supported at the workspace level.";
                    break;
                case -2147220735:
                    sDesc = "[FDO_E_DATASET_NOT_FOUND]The dataset was not found.";
                    break;
                case -2147220734:
                    sDesc = "[FDO_E_DATASET_INVALID_NAME]The dataset name is invalid.";
                    break;
                case -2147220733:
                    sDesc = "[FDO_E_DATASET_ALREADY_EXISTS]The dataset already exists.";
                    break;
                case -2147220732:
                    sDesc = "[FDO_E_DATASET_CANNOT_RENAME]Cannot rename the dataset with objects already open.";
                    break;
                case -2147220731:
                    sDesc = "[FDO_E_DATASET_INVALID_TYPE]Invalid dataset type.";
                    break;
                case -2147220730:
                    sDesc = "[FDO_E_DATASET_CANNOT_DELETE]Cannot delete the dataset.";
                    break;
                case -2147220729:
                    sDesc = "[FDO_E_DATASET_EXTENSION_TYPE_NOT_FOUND]Cannot find the specified feature dataset extension type.";
                    break;
                case -2147220728:
                    sDesc = "[FDO_E_DATASET_PASTE_NOT_SUPPORTED_IN_RELEASE]The paste operation on the dataset is not supported in the target release of the GeoDatabase.";
                    break;
                case -2147220727:
                    sDesc = "[FDO_E_DATASET_CANNOT_RENAME_NOT_SUPPORTED]Cannot rename the dataset.";
                    break;
                case -2147220655:
                    sDesc = "[FDO_E_TABLE_NOT_FOUND]The table was not found.";
                    break;
                case -2147220654:
                    sDesc = "[FDO_E_TABLE_INVALID_NAME]The table name is invalid.";
                    break;
                case -2147220653:
                    sDesc = "[FDO_E_TABLE_ALREADY_EXISTS]The table already exists.";
                    break;
                case -2147220652:
                    sDesc = "[FDO_E_TABLE_NO_OID_FIELD]The table does not have an OID Field.";
                    break;
                case -2147220651:
                    sDesc = "[FDO_E_TABLE_INVALID_KEYWORD]The configuration keyword is invalid.";
                    break;
                case -2147220650:
                    sDesc = "[FDO_E_TABLE_NOT_VERSIONED]The table is not multiversioned.";
                    break;
                case -2147220649:
                    sDesc = "[FDO_E_TABLE_DUPLICATE_COLUMN]Cannot create a table with a duplicate column.";
                    break;
                case -2147220648:
                    sDesc = "[FDO_E_TABLE_COLUMN_NOT_FOUND]A column was specified that does not exist.";
                    break;
                case -2147220647:
                    sDesc = "[FDO_E_TABLE_IN_USE]Cannot access this table because it is in use.";
                    break;
                case -2147220646:
                    sDesc = "[FDO_E_TABLE_RECORD_LENGTH_EXCEEDED]The maximum record length has been exceeded.";
                    break;
                case -2147220479:
                    sDesc = "[FDO_E_FEATURECLASS_NOT_FOUND]The feature class was not found.";
                    break;
                case -2147220478:
                    sDesc = "[FDO_E_FEATURECLASS_BAD_EXTENT]The feature class's extent could not be retrieved or is invalid.";
                    break;
                case -2147220477:
                    sDesc = "[FDO_E_FEATURECLASS_INVALID_NAME]Invalid feature class name.";
                    break;
                case -2147220476:
                    sDesc = "[FDO_E_FEATURECLASS_ALREADY_EXISTS]The feature class already exists.";
                    break;
                case -2147220475:
                    sDesc = "[FDO_E_FEATURECLASS_LOAD_MODE]The feature class is currently in load-only mode.";
                    break;
                case -2147220474:
                    sDesc = "[FDO_E_FEATURECLASS_NETWORK_CANNOT_DELETE]The feature class is in a geometric network and cannot be deleted.";
                    break;
                case -2147220473:
                    sDesc = "[FDO_E_FEATURECLASS_BAD_DEFAULT_SUBTYPE_CODE]The feature class' default subtype code cannot be retrieved or is invalid.";
                    break;
                case -2147220472:
                    sDesc = "[FDO_E_FEATURECLASS_NO_SUBTYPE_FIELD]The feature class does not have a specified subtype field.";
                    break;
                case -2147220471:
                    sDesc = "[FDO_E_FEATURECLASS_NETWORK_CANNOT_RENAME]The orphan junction featureclass cannot be renamed.";
                    break;
                case -2147220470:
                    sDesc = "[FDO_E_FEATURECLASS_SUBTYPE_EXISTS]The feature class already has the specified subtype.";
                    break;
                case -2147220469:
                    sDesc = "[FDO_E_FEATURECLASS_FD_NOT_EDITABLE]The feature dataset is not editable.";
                    break;
                case -2147220468:
                    sDesc = "[FDO_E_FEATURECLASS_SUBTYPE_FIELD_CANNOT_RENAME]The subtype field on a feature class cannot be renamed.";
                    break;
                case -2147220467:
                    sDesc = "[FDO_E_SUBTYPE_CODE_INVALID]The specified subtype code is either too large or small to represent.";
                    break;
                case -2147220466:
                    sDesc = "[FDO_E_SUBTYPE_CODE_DOES_NOT_EXIST]The specified subtype code does not exist.";
                    break;
                case -2147220465:
                    sDesc = "[FDO_E_SUBTYPE_CODE_IS_NULL]The value of the subtype code is NULL.";
                    break;
                case -2147220464:
                    sDesc = "[FDO_E_SUBTYPE_CODE_NOT_INTEGER]The value of the subtype code is not a long or short integer.";
                    break;
                case -2147220463:
                    sDesc = "[FDO_E_FEATURECLASS_NO_SHAPE_COLUMN]The feature class does not have a shape column.";
                    break;
                case -2147220462:
                    sDesc = "[FDO_E_FEATURECLASS_TOPOLOGY_CANNOT_DELETE]The feature class is in a topology and cannot be deleted.";
                    break;
                case -2147220461:
                    sDesc = "[FDO_E_SUBTYPE_CODE_HAS_ASSOCIATED_TOPOLOGY_RULE]The subtype code is associated with a topology rule.";
                    break;
                case -2147220460:
                    sDesc = "[FDO_E_SUBTYPE_IN_USE_CANNOT_DELETE]The subtype code is in use and cannot be deleted.";
                    break;
                case -2147220459:
                    sDesc = "[FDO_E_SUBTYPE_CANNOT_ADD]The subtype code cannot be added.";
                    break;
                case -2147220399:
                    sDesc = "[FDO_E_PLANARGRAPH_NOT_FOUND]The planargraph was not found.";
                    break;
                case -2147220223:
                    sDesc = "[FDO_E_GEOMETRICNETWORK_NOT_FOUND]The geometric network was not found.";
                    break;
                case -2147220222:
                    sDesc = "[FDO_E_ADD_FEATURE_TO_NETWORK]Error adding a feature to a network.";
                    break;
                case -2147220221:
                    sDesc = "[FDO_E_CREATE_LOGICAL_NETWORK]Error creating a logical network.";
                    break;
                case -2147220220:
                    sDesc = "[FDO_E_GEOMETRICNETWORK_ALREADY_EXISTS]The geometric network already exists.";
                    break;
                case -2147220219:
                    sDesc = "[FDO_E_ZERO_LENGTH_EDGE_ELEMENT]Geometry corresponding to edge element may not be zero length.";
                    break;
                case -2147220218:
                    sDesc = "[FDO_E_GEOMETRICNETWORK_CANNOT_RENAME]Cannot rename a geometric network.";
                    break;
                case -2147220217:
                    sDesc = "[FDO_E_GEOMETRICNETWORK_ELEMENT_INCONSISTENCY]Inconsistent elements in the geometric network.";
                    break;
                case -2147220216:
                    sDesc = "[FDO_E_NETWORK_FEATURES_HAVE_HOMOGENEOUS_Z_SUPPORT]Feature classes in a geometric network must have homogeneous support for Zs on geometry.";
                    break;
                case -2147220215:
                    sDesc = "[FDO_E_NO_ASSOCIATED_ERROR_TABLE]There is no error table associated with the geometric network.";
                    break;
                case -2147220214:
                    sDesc = "[FDO_E_FEATURE_ELEMENT_MISSING_POINT_GEOMETRY]There is no point geometry associated with the feature element.";
                    break;
                case -2147220213:
                    sDesc = "[FDO_E_NETWORK_FEATURES_HAVE_HOMOGENEOUS_M_SUPPORT]Feature classes in a geometric network must have homogeneous support for Ms on geometry.";
                    break;
                case -2147220212:
                    sDesc = "[FDO_E_CLASS_EXISTS_WITH_ORPHAN_JUNCTION_CLASS_NAME]An existing class has the same name as the orphan junction feature class.";
                    break;
                case -2147220211:
                    sDesc = "[FDO_E_GEOMETRICNETWORK_INVALID_NAME]The geometric network name is invalid.";
                    break;
                case -2147220143:
                    sDesc = "[FDO_E_DATASET_UNEDITABLE]The dataset does not support editing.";
                    break;
                case -2147220142:
                    sDesc = "[FDO_E_START_EDITING]Error starting an edit session.";
                    break;
                case -2147220141:
                    sDesc = "[FDO_E_SAVE_EDIT_SESSION]Error saving an edit session.";
                    break;
                case -2147220140:
                    sDesc = "[FDO_E_STOP_EDITING_WITH_SAVE]Error stopping an edit session with save edits.";
                    break;
                case -2147220139:
                    sDesc = "[FDO_E_STOP_EDITING_WITH_DISCARD]Error stopping an edit session with discard edits.";
                    break;
                case -2147220138:
                    sDesc = "[FDO_E_NOT_ALLOWED_WHILE_EDITING]This operation is not allowed while editing.";
                    break;
                case -2147220137:
                    sDesc = "[FDO_E_COULD_NOT_CLEAN_COVERAGE]Error in cleaning coverage during save.";
                    break;
                case -2147220136:
                    sDesc = "[FDO_E_NO_INTEGRATEABLE_LAYERS]No valid InteGrateable Feature layers within Feature Dataset.";
                    break;
                case -2147220135:
                    sDesc = "[FDO_E_INVALID_TOPOLOGY]Invalid Topology.";
                    break;
                case -2147220134:
                    sDesc = "[FDO_E_NOT_EDITING]Operation only allowed while editing.";
                    break;
                case -2147220133:
                    sDesc = "[FDO_E_COULD_NOT_ENCODE_INFO_ITEM]Error in encoding INFO item during save.";
                    break;
                case -2147220132:
                    sDesc = "[FDO_E_NODE_NOT_ON_ARC]Node must intersect an arc feature.";
                    break;
                case -2147220131:
                    sDesc = "[FDO_E_CANNOT_REMOVE_LAST_LABEL]Cannot remove last label from polygon.";
                    break;
                case -2147220130:
                    sDesc = "[FDO_E_CANNOT_MOVE_LABEL_OUT_OF_POLYGON]Cannot move label out of polygon.";
                    break;
                case -2147220129:
                    sDesc = "[FDO_E_INVALID_POLYGON_LABEL_DELETED]Label no longer within valid polygon, deleting.";
                    break;
                case -2147220128:
                    sDesc = "[FDO_E_CANNOT_BREAK_TOPOLOGY]Operations that break coverage topology are not supported.";
                    break;
                case -2147220127:
                    sDesc = "[FDO_E_CANNOT_CREATE_UNIVERSE_LABEL]Coverage labels cannot be created in the universe polygon.";
                    break;
                case -2147220126:
                    sDesc = "[FDO_E_CANNOT_MOVE_UNIVERSE_LABELS]Coverage labels in the universe polygon cannot be moved.";
                    break;
                case -2147220125:
                    sDesc = "[FDO_E_INVALID_GEOMETRY]Invalid geometry.";
                    break;
                case -2147220124:
                    sDesc = "[FDO_E_CORUPTED_COVERAGE]Corupted Coverage.";
                    break;
                case -2147220123:
                    sDesc = "[FDO_E_DUPLICATE_FIELD_NAMES]Duplicate Field Names within Table.";
                    break;
                case -2147220122:
                    sDesc = "[FDO_E_CANNOT_EDIT_ZS]Cannot edit features with Z values.";
                    break;
                case -2147220121:
                    sDesc = "[FDO_E_NO_POLYGONS_CREATED]No newly added features, after PolygonSplitLines.";
                    break;
                case -2147220120:
                    sDesc = "[FDO_E_ABORT_EDITS_FAILED]Unable to save edits because an incorrect edit operation could not be completely rolled back.";
                    break;
                case -2147220119:
                    sDesc = "[FDO_E_FLUSH_EDITS_FAILED]Unable to save edits because of failure in flushing edits to the database.";
                    break;
                case -2147220118:
                    sDesc = "[FDO_E_CANNOT_EDIT_TABLE_WITH_UNIQ_USER_INDEX]Unable to edit the table or feature class because it has a unique index on a non OID field.";
                    break;
                case -2147220117:
                    sDesc = "[FDO_E_CANNOT_EDIT_DATASET_WITH_UNIQ_USER_INDEX]Unable to edit the dataset because it contains a table or feature class with a unique index on a non OID field.";
                    break;
                case -2147220116:
                    sDesc = "[FDO_E_CANNOT_REBUILD_POLYGONS]Cannot rebuild polygons from current topology elements.";
                    break;
                case -2147220115:
                    sDesc = "[FDO_E_USERTRANSACTION_NOT_ALLOWED]User transaction not allowed at this time.";
                    break;
                case -2147219967:
                    sDesc = "[FDO_E_PROPERTY_NO_SUBTYPE]This property does not have a SubType.";
                    break;
                case -2147219966:
                    sDesc = "[FDO_E_PROPERTY_NOT_FOUND]The property was not found.";
                    break;
                case -2147219887:
                    sDesc = "[FDO_E_FIELD_INVALID]A general error when something is wrong with a Field.";
                    break;
                case -2147219886:
                    sDesc = "[FDO_E_FIELD_INVALID_NAME]The name of the Field is unacceptable.";
                    break;
                case -2147219885:
                    sDesc = "[FDO_E_FIELD_NOT_FOUND]An expected Field was not found or could not be retrieved properly.";
                    break;
                case -2147219884:
                    sDesc = "[FDO_E_FIELD_ALREADY_EXISTS]The Field already exists.";
                    break;
                case -2147219883:
                    sDesc = "[FDO_E_FIELD_INVALID_TYPE]The Field type is invalid or unsupported for the operation.";
                    break;
                case -2147219882:
                    sDesc = "[FDO_E_FIELD_UNSUPPORTED_OPERATION]The Field type does not support the current operation. For example, attempting to set OID field to NULL.";
                    break;
                case -2147219881:
                    sDesc = "[FDO_E_FIELD_INVALID_GEOMETRY_TYPE]The GeometryType property of the Field is invalid or unsupported for this operation.";
                    break;
                case -2147219880:
                    sDesc = "[FDO_E_FIELD_NOT_EDITABLE]The Field is not editable.";
                    break;
                case -2147219879:
                    sDesc = "[FDO_E_FIELD_NOT_NULLABLE]The Field is not nullable.";
                    break;
                case -2147219878:
                    sDesc = "[FDO_E_FIELD_CANNOT_DELETE_WEIGHT_FIELD]The Field corresponds to a weight and may not be deleted.";
                    break;
                case -2147219877:
                    sDesc = "[FDO_E_FIELD_CANNOT_DELETE_REQUIRED_FIELD]The Field is required and may not be deleted.";
                    break;
                case -2147219876:
                    sDesc = "[FDO_E_FIELD_CANNOT_DELETE_SUBTYPE_FIELD]The Field is a subtype field and may not be deleted.";
                    break;
                case -2147219875:
                    sDesc = "[FDO_E_FIELD_CANNOT_DELETE_LAST_FIELD]The Field is the last remaining field and may not be deleted.";
                    break;
                case -2147219874:
                    sDesc = "[FDO_E_FIELD_IS_KEYWORD]The Field is the keyword the destination DBMS.";
                    break;
                case -2147219711:
                    sDesc = "[FDO_E_FIELDS_INVALID]A general error when something is wrong with the Fields collection.";
                    break;
                case -2147219710:
                    sDesc = "[FDO_E_FIELDS_NOT_FOUND]An expected Fields collection was not found or could not be retrieved properly.";
                    break;
                case -2147219709:
                    sDesc = "[FDO_E_FIELDS_NO_GEOMETRY]The Fields collection did not contain an expected geometry field.";
                    break;
                case -2147219708:
                    sDesc = "[FDO_E_FIELDS_NO_OID]The Fields collection did not contain an expected OID field.";
                    break;
                case -2147219707:
                    sDesc = "[FDO_E_FIELDS_MULTIPLE_OIDS]The Fields collection contained multiple OID fields.";
                    break;
                case -2147219706:
                    sDesc = "[FDO_E_FIELDS_MULTIPLE_GEOMETRIES]The Fields collection contained multiple geometry fields.";
                    break;
                case -2147219705:
                    sDesc = "[FDO_E_FIELDS_MODEL_NAME_ALREADY_EXISTS]Another field within the class already has this model name.";
                    break;
                case -2147219704:
                    sDesc = "[FDO_E_FIELDS_MULTIPLE_RASTERS]The Fields collection contained multiple raster fields.";
                    break;
                case -2147219631:
                    sDesc = "[FDO_E_INDEX_WRONG_TYPE]The operation requires a different index type.";
                    break;
                case -2147219630:
                    sDesc = "[FDO_E_INDEX_ALREADY_EXISTS]The index already exists.";
                    break;
                case -2147219629:
                    sDesc = "[FDO_E_INDEX_NOT_FOUND]The index was not found.";
                    break;
                case -2147219628:
                    sDesc = "[FDO_E_INDEX_NOT_ALLOWED]This type of index is not allowed.";
                    break;
                case -2147219455:
                    sDesc = "[FDO_E_METADATA_TABLE_NOT_FOUND]Unable to find a required metadata table.";
                    break;
                case -2147219454:
                    sDesc = "[FDO_E_METADATA_FIELD_NOT_FOUND]A required Field in a metadata table could not be located.";
                    break;
                case -2147219453:
                    sDesc = "[FDO_E_METADATA_ADDING_DATASET]An error occurred adding an entry to the ESRI_DATASETS table.";
                    break;
                case -2147219452:
                    sDesc = "[FDO_E_METADATA_ADDING_FEATURECLASS]An error occurred while adding an entry to the FEATUREDATASET_CLASSES or the FEATURECLASSES table.";
                    break;
                case -2147219451:
                    sDesc = "[FDO_E_METADATA_BAD_CLSID]The CLSID read from the FEATURECLASSES table was bad (unable to convert using ::CLSIDFromString).";
                    break;
                case -2147219375:
                    sDesc = "[FDO_E_SPATIALREL_NOT_SUPPORTED]The operation does not support this spatial relationship.";
                    break;
                case -2147219374:
                    sDesc = "[FDO_E_SPATIALREL_UNKNOWN]The spatial relationship is unknown or not defined.";
                    break;
                case -2147219373:
                    sDesc = "[FDO_E_FEATURETYPE_NOT_SUPPORTED]The operation does not support this feature type.";
                    break;
                case -2147219372:
                    sDesc = "[FDO_E_FEATURETYPE_UNKNOWN]The feature type is unknown or not defined.";
                    break;
                case -2147219371:
                    sDesc = "[FDO_E_DATASETTYPE_NOT_SUPPORTED]The operation does not support this dataset type.";
                    break;
                case -2147219370:
                    sDesc = "[FDO_E_DATASETTYPE_UNKNOWN]The dataset type is unknown or not defined.";
                    break;
                case -2147219369:
                    sDesc = "[FDO_E_DRAWSTYLE_NOT_SUPPORTED]The operation does not support this draw style.";
                    break;
                case -2147219368:
                    sDesc = "[FDO_E_DRAWSTYLE_UNKNOWN]The draw style is unknown or not defined.";
                    break;
                case -2147219367:
                    sDesc = "[FDO_E_DRAWPHASE_NOT_SUPPORTED]The operation does not support this draw phase.";
                    break;
                case -2147219366:
                    sDesc = "[FDO_E_DRAWPHASE_UNKNOWN]The draw phase is unknown or not defined.";
                    break;
                case -2147219199:
                    sDesc = "[FDO_E_GEOMETRY_TYPE_NOT_SUPPORTED]No support for this geometry type.";
                    break;
                case -2147219198:
                    sDesc = "[FDO_E_MULTIPART_EDGE_FEATURE_NOT_SUPPORTED]Multipart edge feature geometries not supported.";
                    break;
                case -2147219197:
                    sDesc = "[FDO_E_GEOMETRY_HAS_NO_M_VALUES]Geometry has no M values.";
                    break;
                case -2147219196:
                    sDesc = "[FDO_E_GEOMETRY_HAS_NO_Z_VALUES]Geometry has no Z values.";
                    break;
                case -2147219195:
                    sDesc = "[FDO_E_GEOMETRY_HAS_NULL_Z_VALUES]Geometry has null Z values.";
                    break;
                case -2147219194:
                    sDesc = "[FDO_E_GEOMETRY_NOT_SIMPLE]Geometry is not simple.";
                    break;
                case -2147219193:
                    sDesc = "[FDO_E_GEOMETRY_CANNOT_HAVE_Z_VALUES]Geometry cannot have Z values.";
                    break;
                case -2147219192:
                    sDesc = "[FDO_E_GEOMETRY_SPATIAL_REFERENCE]Spatial reference (projection) related error.";
                    break;
                case -2147219191:
                    sDesc = "[FDO_E_GEOMETRY_MISSING_SPATIAL_REFERENCE]Geometry is missing required spatial reference.";
                    break;
                case -2147219119:
                    sDesc = "[FDO_E_ROW_NO_SETUPINTERFACE]The row object does not support the IRowSetup interface.";
                    break;
                case -2147219118:
                    sDesc = "[FDO_E_ROW_NOT_FOUND]A requested row object could not be located.";
                    break;
                case -2147219117:
                    sDesc = "[FDO_E_ROW_NO_OID]The row does not have an OID.";
                    break;
                case -2147219116:
                    sDesc = "[FDO_E_ROW_NO_OBJCLASS]Cannot determine the row's ObjectClass.";
                    break;
                case -2147219115:
                    sDesc = "[FDO_E_ROW_BAD_VALUE]The row contains a bad value.";
                    break;
                case -2147219114:
                    sDesc = "[FDO_E_ROW_ALREADY_EXISTS]A row with this OID already exists.";
                    break;
                case -2147219113:
                    sDesc = "[FDO_E_COMPARE_TYPE_MISMATCH]Cannot compare incompatible types.";
                    break;
                case -2147217407:
                    sDesc = "[FDO_E_FEATURE_NO_ANNO]The feature object does not have annotation.";
                    break;
                case -2147217406:
                    sDesc = "[FDO_E_FEATURE_BAD_SHAPE]The feature object does not have a valid shape.";
                    break;
                case -2147217405:
                    sDesc = "[FDO_E_FEATURE_OUTSIDE_SPATIALREF]The feature falls outside the defined spatial reference.";
                    break;
                case -2147217404:
                    sDesc = "[FDO_E_FEATURE_SHAPE_UPDATE_BLOCKED]The feature is mutually exclusive. Cannot update shape.";
                    break;
                case -2147217403:
                    sDesc = "[FDO_E_FEATURE_AREA_LENGTH_UPDATE_FAILED]Failed to update feature's area/length field in response to shape update.";
                    break;
                case -2147217402:
                    sDesc = "[FDO_E_ON_DELETE_MESSAGE_FAILED]On Delete Message returned failure.";
                    break;
                case -2147217401:
                    sDesc = "[FDO_E_DELETE_PART_OBJECTS_FAILED]Failed to delete part objects for composite object.";
                    break;
                case -2147217400:
                    sDesc = "[FDO_E_DELETE_RELATIONSHIPS_FAILED]Failed to delete relationships for object.";
                    break;
                case -2147217399:
                    sDesc = "[FDO_E_ON_CHANGED_MESSAGE_FAILED]On Changed message returned failure.";
                    break;
                case -2147217398:
                    sDesc = "[FDO_E_MOVE_RELATED_FEATURES_FAILED]Failed to move related features.";
                    break;
                case -2147217397:
                    sDesc = "[FDO_E_ROTATE_RELATED_FEATURES_FAILED]Failed to rotate related features.";
                    break;
                case -2147217396:
                    sDesc = "[FDO_E_FEATURE_DELETED]The feature has been deleted.";
                    break;
                case -2147217395:
                    sDesc = "[FDO_E_FEATURE_VALUE_TYPE_MISMATCH]The value type is incompatible.";
                    break;
                case -2147217394:
                    sDesc = "[FDO_E_CUSTOM_COMPLEX_JUNCTION_NOT_IMPLEMENTED]The required custom complex junction was not implemented.";
                    break;
                case -2147217393:
                    sDesc = "[FDO_E_FEATURE_NOT_FOUND]A requested feature object could not be located.";
                    break;
                case -2147217392:
                    sDesc = "[FDO_E_SPLIT_NOT_SUPPORTED_ON_GEOMETRY_TYPE]The split operation is not supported on the selected feature's geometry type.";
                    break;
                case -2147217391:
                    sDesc = "[FDO_E_SPLITTING_POLYGONS_REQUIRES_POLYLINE]Splitting a polygon requires a polyline splitter.";
                    break;
                case -2147217390:
                    sDesc = "[FDO_E_SPLITTING_POLYLINES_REQUIRES_POINT]Splitting a polyline requires a point splitter.";
                    break;
                case -2147217389:
                    sDesc = "[FDO_E_SPLIT_POINT_YIELDS_ZERO_LENGTH_POLYLINE]Split point results in a zero length polyline.";
                    break;
                case -2147217388:
                    sDesc = "[FDO_E_CUTTER_YIELDS_ZERO_AREA_POLYGON]Cutting polyline results in zero area polygon.";
                    break;
                case -2147217387:
                    sDesc = "[FDO_E_FEATURE_NO_GEOMETRY]The feature does not have any associated geometry.";
                    break;
                case -2147217386:
                    sDesc = "[FDO_E_REQUIRED_INTERFACE_NOT_FOUND]A required interface on the feature was not found.";
                    break;
                case -2147217385:
                    sDesc = "[FDO_E_REQUIRED_CONNECTION_POINT_NOT_FOUND]A required connection point on a complex junction was not found.";
                    break;
                case -2147217384:
                    sDesc = "[FDO_E_INVALID_CONNECTION_POINT_GEOMETRY]The geometry for a complex junction connection point is invalid.";
                    break;
                case -2147217383:
                    sDesc = "[FDO_E_FEATURE_EMPTY_GEOMETRY]The feature has empty geometry.";
                    break;
                case -2147217327:
                    sDesc = "[FDO_E_CURSOR_WRONG_TYPE]This type of cursor does not support this operation. For example, calling UpdateRow on a read-only cursor.";
                    break;
                case -2147217326:
                    sDesc = "[FDO_E_CURSOR_INVALID]The cursor is in an invalid state.";
                    break;
                case -2147217325:
                    sDesc = "[FDO_E_CURSOR_FINISHED]The cursor has completed and is at the end.";
                    break;
                case -2147217324:
                    sDesc = "[FDO_E_CURSOR_LOCKED]The cursor cannot aquire a lock against the data.";
                    break;
                case -2147217151:
                    sDesc = "[FDO_E_VERSION_BAD_NAME]The ID of the version is bad.";
                    break;
                case -2147217150:
                    sDesc = "[FDO_E_VERSION_UNEDITABLE]The current version does not support editing (base, consistent, or closed).";
                    break;
                case -2147217149:
                    sDesc = "[FDO_E_VERSION_HAS_CONFLICTS]This operation is not allowed using conflicting versions.";
                    break;
                case -2147217148:
                    sDesc = "[FDO_E_VERSION_ALREADY_EXISTS]The version already exists.";
                    break;
                case -2147217147:
                    sDesc = "[FDO_E_VERSION_REDEFINED]The version has been redefined to reference a new database state.";
                    break;
                case -2147217146:
                    sDesc = "[FDO_E_VERSION_NOT_FOUND]The version could not be located.";
                    break;
                case -2147217145:
                    sDesc = "[FDO_E_VERSION_INVALID_STATE]The version's internal state ID is invalid.";
                    break;
                case -2147217144:
                    sDesc = "[FDO_E_VERSION_NOT_OWNER]Operation only allowed by the owner of the version.";
                    break;
                case -2147217143:
                    sDesc = "[FDO_E_VERSION_HAS_CHILDREN]Operation only allowed on versions without children.";
                    break;
                case -2147217142:
                    sDesc = "[FDO_E_VERSION_NOT_RECONCILED]The version has not been reconciled.";
                    break;
                case -2147217141:
                    sDesc = "[FDO_E_VERSION_IS_PROTECTED]Operation not allowed because the version is protected.";
                    break;
                case -2147217140:
                    sDesc = "[FDO_E_VERSION_IN_USE]Operation not allowed because the version is in use.";
                    break;
                case -2147217139:
                    sDesc = "[FDO_E_VERSION_BEING_EDITED]Operation not allowed while the version is being edited.";
                    break;
                case -2147217138:
                    sDesc = "[FDO_E_VERSION_BEING_RECONCILED]Operation not allowed while the version is being reconciled.";
                    break;
                case -2147217137:
                    sDesc = "[FDO_E_RECONCILE_VERSION_NOT_AVAILABLE]Unable to reconcile: the target version is currently being reconciled against.";
                    break;
                case -2147217136:
                    sDesc = "[FDO_E_VERSION_RECONCILE_LOST]Post not allowed after undoing a reconcile.";
                    break;
                case -2147217135:
                    sDesc = "[FDO_E_FAILED_FILTERING_CONFLICTS]Unable to reconcile : Failed filtering conflicts.";
                    break;
                case -2147217134:
                    sDesc = "[FDO_E_RECONCILE_VERSION_NOT_ANCESTOR]Unable to reconcile : Reconcile version is not an ancestor.";
                    break;
                case -2147217071:
                    sDesc = "[FDO_E_DATASOURCE_LOCK_FAILED]DataSource could not be locked.";
                    break;
                case -2147217070:
                    sDesc = "[FDO_E_DATASOURCE_RELEASELOCK_FAILED]DataSource lock could not be released.";
                    break;
                case -2147217069:
                    sDesc = "[FDO_E_DATASOURCE_INUSE_ELSEWHERE]DataSource is being used in another application.";
                    break;
                case -2147216895:
                    sDesc = "[FDO_E_INVALID_UNITS]The xy units are invalid.";
                    break;
                case -2147216894:
                    sDesc = "[FDO_E_INVALID_GRID_SIZE]The spatial index grid size is invalid.";
                    break;
                case -2147216893:
                    sDesc = "[FDO_E_SPATIALREF_MISMATCH]The spatial references do not match.";
                    break;
                case -2147216892:
                    sDesc = "[FDO_E_SPATIALREF_INVALID]Invalid spatial reference.";
                    break;
                case -2147216891:
                    sDesc = "[FDO_E_INVALID_M_DOMAIN]The M domain is invalid.";
                    break;
                case -2147216890:
                    sDesc = "[FDO_E_CANNOT_ALTER_SPATIALREF]The spatial reference cannot be altered.";
                    break;
                case -2147216889:
                    sDesc = "[FDO_E_NO_SPATIALREF]No spatial reference exists.";
                    break;
                case -2147216815:
                    sDesc = "[FDO_E_SPATIALFILTER_INVALID]The spatial filter is invalid.";
                    break;
                case -2147216814:
                    sDesc = "[FDO_E_SPATIALFILTER_INVALID_GEOMETRY]The geometry property of the spatial filter is invalid.";
                    break;
                case -2147216639:
                    sDesc = "[FDO_E_SELECTION_MISMATCH]Selection sets do not match.";
                    break;
                case -2147216638:
                    sDesc = "[FDO_E_SELECTION_INVALID_TYPE]Selection type is invalid.";
                    break;
                case -2147216637:
                    sDesc = "[FDO_E_SELECTION_NO_SELECTABLE_LAYERS]No selectable layers.";
                    break;
                case -2147216559:
                    sDesc = "[FDO_E_OBJECTCLASS_COULD_NOT_CREATE_CLASS_INSTANCE]Unable to instantiate object class instance COM component.";
                    break;
                case -2147216558:
                    sDesc = "[FDO_E_OBJECTCLASS_COULD_NOT_CREATE_CLASS_EXTENSION]Unable to instantiate object class extension COM component.";
                    break;
                case -2147216557:
                    sDesc = "[FDO_E_OBJECTCLASS_COULD_NOT_INITIALIZE_CLASS_EXTENSION]Unable to initialize object class extension COM component.";
                    break;
                case -2147216556:
                    sDesc = "[FDO_E_OBJECTCLASS_REQUIRES_AN_EDIT_SESSION]Objects in this object class cannot be updated outside of an edit session.";
                    break;
                case -2147216555:
                    sDesc = "[FDO_E_OBJECTCLASS_MODEL_NAME_ALREADY_EXISTS]An object class with this model name already exists.";
                    break;
                case -2147216554:
                    sDesc = "[FDO_E_CLASS_FD_NOT_EDITABLE]The feature dataset is not editable.";
                    break;
                case -2147216553:
                    sDesc = "[FDO_E_COULD_NOT_LOAD_CLASS_EXTENSION_PROPERTIES]The class extension property set could not be loaded.";
                    break;
                case -2147216383:
                    sDesc = "[FDO_E_RELCLASS_COULD_NOT_GET_ORIG_PRIM_KEY]Unable to obtain origin primary key value.";
                    break;
                case -2147216382:
                    sDesc = "[FDO_E_RELCLASS_COULD_NOT_GET_ORIG_FOR_KEY]Unable to obtain origin foreign key value.";
                    break;
                case -2147216381:
                    sDesc = "[FDO_E_RELCLASS_COULD_NOT_GET_DEST_PRIM_KEY]Unable to obtain destination primary key value.";
                    break;
                case -2147216380:
                    sDesc = "[FDO_E_RELCLASS_COULD_NOT_GET_DEST_FOR_KEY]Unable to obtain destination foreign key value.";
                    break;
                case -2147216379:
                    sDesc = "[FDO_E_RELCLASS_INCOMPATIBLE_WITH_EXISTING_RELCLASS]The relationship class is incompatible with an existing relationship class.";
                    break;
                case -2147216378:
                    sDesc = "[FDO_E_RELCLASS_CANNOT_RESET_FKEYS]Cannot reset foreign keys for an existing relationship row.";
                    break;
                case -2147216377:
                    sDesc = "[FDO_E_RELCLASS_INVALID_FKEY]Invalid foreign key value.";
                    break;
                case -2147216127:
                    sDesc = "[FDO_E_SE_FAILURE]SDE Error.";
                    break;
                case -2147216126:
                    sDesc = "[FDO_E_SE_INVALID_LAYERINFO_OBJECT]SDE Error.";
                    break;
                case -2147216125:
                    sDesc = "[FDO_E_SE_NO_ANNOTATION]SDE Error.";
                    break;
                case -2147216124:
                    sDesc = "[FDO_E_SE_FINISHED]SDE Error.";
                    break;
                case -2147216123:
                    sDesc = "[FDO_E_SE_SDE_NOT_STARTED]SDE Error.";
                    break;
                case -2147216122:
                    sDesc = "[FDO_E_SE_UNCHANGED]SDE Error.";
                    break;
                case -2147216120:
                    sDesc = "[FDO_E_SE_CONNECTIONS_EXCEEDED]SDE Error.";
                    break;
                case -2147216119:
                    sDesc = "[FDO_E_SE_LOGIN_NOT_ALLOWED]SDE Error.";
                    break;
                case -2147216118:
                    sDesc = "[FDO_E_SE_INVALID_USER]SDE Error.";
                    break;
                case -2147216117:
                    sDesc = "[FDO_E_SE_NET_FAILURE]SDE Error.";
                    break;
                case -2147216116:
                    sDesc = "[FDO_E_SE_NET_TIMEOUT]SDE Error.";
                    break;
                case -2147216115:
                    sDesc = "[FDO_E_SE_OUT_OF_SVMEM]SDE Error.";
                    break;
                case -2147216114:
                    sDesc = "[FDO_E_SE_OUT_OF_CLMEM]SDE Error.";
                    break;
                case -2147216113:
                    sDesc = "[FDO_E_SE_OUT_OF_CONTEXT]SDE Error.";
                    break;
                case -2147216112:
                    sDesc = "[FDO_E_SE_NO_ACCESS]SDE Error.";
                    break;
                case -2147216111:
                    sDesc = "[FDO_E_SE_TOO_MANY_LAYERS]SDE Error.";
                    break;
                case -2147216110:
                    sDesc = "[FDO_E_SE_NO_LAYER_SPECIFIED]SDE Error.";
                    break;
                case -2147216109:
                    sDesc = "[FDO_E_SE_LAYER_LOCKED]SDE Error.";
                    break;
                case -2147216108:
                    sDesc = "[FDO_E_SE_LAYER_EXISTS]SDE Error.";
                    break;
                case -2147216107:
                    sDesc = "[FDO_E_SE_LAYER_NOEXIST]SDE Error.";
                    break;
                case -2147216106:
                    sDesc = "[FDO_E_SE_LAYER_INUSE]SDE Error.";
                    break;
                case -2147216104:
                    sDesc = "[FDO_E_SE_ROW_NOEXIST]SDE Error.";
                    break;
                case -2147216102:
                    sDesc = "[FDO_E_SE_ROW_EXISTS]SDE Error.";
                    break;
                case -2147216101:
                    sDesc = "[FDO_E_SE_LAYER_MISMATCH]SDE Error.";
                    break;
                case -2147216100:
                    sDesc = "[FDO_E_SE_NO_PERMISSIONS]SDE Error.";
                    break;
                case -2147216099:
                    sDesc = "[FDO_E_SE_INVALID_NOT_NULL]SDE Error.";
                    break;
                case -2147216098:
                    sDesc = "[FDO_E_SE_INVALID_SHAPE]SDE Error.";
                    break;
                case -2147216097:
                    sDesc = "[FDO_E_SE_INVALID_LAYER_NUMBER]SDE Error.";
                    break;
                case -2147216096:
                    sDesc = "[FDO_E_SE_INVALID_ENTITY_TYPE]SDE Error.";
                    break;
                case -2147216095:
                    sDesc = "[FDO_E_SE_INVALID_SEARCH_METHOD]SDE Error.";
                    break;
                case -2147216094:
                    sDesc = "[FDO_E_SE_INVALID_ETYPE_MASK]SDE Error.";
                    break;
                case -2147216093:
                    sDesc = "[FDO_E_SE_BIND_CONFLICT]SDE Error.";
                    break;
                case -2147216092:
                    sDesc = "[FDO_E_SE_INVALID_GRIDSIZE]SDE Error.";
                    break;
                case -2147216091:
                    sDesc = "[FDO_E_SE_INVALID_LOCK_MODE]SDE Error.";
                    break;
                case -2147216090:
                    sDesc = "[FDO_E_SE_ETYPE_NOT_ALLOWED]SDE Error.";
                    break;
                case -2147216088:
                    sDesc = "[FDO_E_SE_INVALID_NUM_OF_PTS]SDE Error.";
                    break;
                case -2147216087:
                    sDesc = "[FDO_E_SE_TABLE_NOEXIST]SDE Error.";
                    break;
                case -2147216086:
                    sDesc = "[FDO_E_SE_ATTR_NOEXIST]SDE Error.";
                    break;
                case -2147216085:
                    sDesc = "[FDO_E_SE_LICENSE_FAILURE]SDE Error.";
                    break;
                case -2147216084:
                    sDesc = "[FDO_E_SE_OUT_OF_LICENSES]SDE Error.";
                    break;
                case -2147216083:
                    sDesc = "[FDO_E_SE_INVALID_COLUMN_VALUE]SDE Error.";
                    break;
                case -2147216081:
                    sDesc = "[FDO_E_SE_INVALID_SQL]SDE Error.";
                    break;
                case -2147216080:
                    sDesc = "[FDO_E_SE_LOG_NOEXIST]SDE Error.";
                    break;
                case -2147216079:
                    sDesc = "[FDO_E_SE_LOG_NOACCESS]SDE Error.";
                    break;
                case -2147216078:
                    sDesc = "[FDO_E_SE_LOG_NOTOPEN]SDE Error.";
                    break;
                case -2147216077:
                    sDesc = "[FDO_E_SE_LOG_IO_ERROR]SDE Error.";
                    break;
                case -2147216076:
                    sDesc = "[FDO_E_SE_NO_SHAPES]SDE Error.";
                    break;
                case -2147216075:
                    sDesc = "[FDO_E_SE_NO_LOCKS]SDE Error.";
                    break;
                case -2147216074:
                    sDesc = "[FDO_E_SE_LOCK_CONFLICT]SDE Error.";
                    break;
                case -2147216073:
                    sDesc = "[FDO_E_SE_OUT_OF_LOCKS]SDE Error.";
                    break;
                case -2147216072:
                    sDesc = "[FDO_E_SE_DB_IO_ERROR]SDE Error.";
                    break;
                case -2147216071:
                    sDesc = "[FDO_E_SE_STREAM_IN_PROGRESS]SDE Error.";
                    break;
                case -2147216070:
                    sDesc = "[FDO_E_SE_INVALID_COLUMN_TYPE]SDE Error.";
                    break;
                case -2147216069:
                    sDesc = "[FDO_E_SE_TOPO_ERROR]SDE Error.";
                    break;
                case -2147216068:
                    sDesc = "[FDO_E_SE_ATTR_CONV_ERROR]SDE Error.";
                    break;
                case -2147216067:
                    sDesc = "[FDO_E_SE_INVALID_COLUMN_DEF]SDE Error.";
                    break;
                case -2147216066:
                    sDesc = "[FDO_E_SE_INVALID_SHAPE_BUF_SIZE]SDE Error.";
                    break;
                case -2147216065:
                    sDesc = "[FDO_E_SE_INVALID_ENVELOPE]SDE Error.";
                    break;
                case -2147216064:
                    sDesc = "[FDO_E_SE_TEMP_IO_ERROR]SDE Error.";
                    break;
                case -2147216063:
                    sDesc = "[FDO_E_SE_GSIZE_TOO_SMALL]SDE Error.";
                    break;
                case -2147216062:
                    sDesc = "[FDO_E_SE_LICENSE_EXPIRED]SDE Error.";
                    break;
                case -2147216061:
                    sDesc = "[FDO_E_SE_TABLE_EXISTS]SDE Error.";
                    break;
                case -2147216060:
                    sDesc = "[FDO_E_SE_INDEX_EXISTS]SDE Error.";
                    break;
                case -2147216059:
                    sDesc = "[FDO_E_SE_INDEX_NOEXIST]SDE Error.";
                    break;
                case -2147216058:
                    sDesc = "[FDO_E_SE_INVALID_POINTER]SDE Error.";
                    break;
                case -2147216057:
                    sDesc = "[FDO_E_SE_INVALID_PARAM_VALUE]SDE Error.";
                    break;
                case -2147216056:
                    sDesc = "[FDO_E_SE_ALL_SLIVERS]SDE Error.";
                    break;
                case -2147216055:
                    sDesc = "[FDO_E_SE_TRANS_IN_PROGRESS]SDE Error.";
                    break;
                case -2147216054:
                    sDesc = "[FDO_E_SE_IOMGR_NO_DBMS_CONNECT]SDE Error.";
                    break;
                case -2147216053:
                    sDesc = "[FDO_E_SE_DUPLICATE_ARC]SDE Error.";
                    break;
                case -2147216052:
                    sDesc = "[FDO_E_SE_INVALID_ANNO_OBJECT]SDE Error.";
                    break;
                case -2147216051:
                    sDesc = "[FDO_E_SE_PT_NO_EXIST]SDE Error.";
                    break;
                case -2147216050:
                    sDesc = "[FDO_E_SE_PTS_NOT_ADJACENT]SDE Error.";
                    break;
                case -2147216049:
                    sDesc = "[FDO_E_SE_INVALID_MID_PT]SDE Error.";
                    break;
                case -2147216048:
                    sDesc = "[FDO_E_SE_INVALID_END_PT]SDE Error.";
                    break;
                case -2147216047:
                    sDesc = "[FDO_E_SE_INVALID_RADIUS]SDE Error.";
                    break;
                case -2147216046:
                    sDesc = "[FDO_E_SE_LOAD_ONLY_LAYER]SDE Error.";
                    break;
                case -2147216045:
                    sDesc = "[FDO_E_SE_LAYERS_NOT_FOUND]SDE Error.";
                    break;
                case -2147216044:
                    sDesc = "[FDO_E_SE_FILE_IO_ERROR]SDE Error.";
                    break;
                case -2147216043:
                    sDesc = "[FDO_E_SE_BLOB_SIZE_TOO_LARGE]SDE Error.";
                    break;
                case -2147216042:
                    sDesc = "[FDO_E_SE_CORRIDOR_OUT_OF_BOUNDS]SDE Error.";
                    break;
                case -2147216041:
                    sDesc = "[FDO_E_SE_SHAPE_INTEGRITY_ERROR]SDE Error.";
                    break;
                case -2147216040:
                    sDesc = "[FDO_E_SE_NOT_IMPLEMENTED_YET]SDE Error.";
                    break;
                case -2147216039:
                    sDesc = "[FDO_E_SE_CAD_EXISTS]SDE Error.";
                    break;
                case -2147216038:
                    sDesc = "[FDO_E_SE_INVALID_TRANSID]SDE Error.";
                    break;
                case -2147216037:
                    sDesc = "[FDO_E_SE_INVALID_LAYER_NAME]SDE Error.";
                    break;
                case -2147216036:
                    sDesc = "[FDO_E_SE_INVALID_LAYER_KEYWORD]SDE Error.";
                    break;
                case -2147216035:
                    sDesc = "[FDO_E_SE_INVALID_RELEASE]SDE Error.";
                    break;
                case -2147216034:
                    sDesc = "[FDO_E_SE_VERSION_TBL_EXISTS]SDE Error.";
                    break;
                case -2147216033:
                    sDesc = "[FDO_E_SE_COLUMN_NOT_BOUND]SDE Error.";
                    break;
                case -2147216032:
                    sDesc = "[FDO_E_SE_INVALID_INDICATOR_VALUE]SDE Error.";
                    break;
                case -2147216031:
                    sDesc = "[FDO_E_SE_INVALID_CONNECTION]SDE Error.";
                    break;
                case -2147216030:
                    sDesc = "[FDO_E_SE_INVALID_DBA_PASSWORD]SDE Error.";
                    break;
                case -2147216029:
                    sDesc = "[FDO_E_SE_PATH_NOT_FOUND]SDE Error.";
                    break;
                case -2147216028:
                    sDesc = "[FDO_E_SE_SDEHOME_NOT_SET]SDE Error.";
                    break;
                case -2147216027:
                    sDesc = "[FDO_E_SE_NOT_TABLE_OWNER]SDE Error.";
                    break;
                case -2147216026:
                    sDesc = "[FDO_E_SE_PROCESS_NOT_FOUND]SDE Error.";
                    break;
                case -2147216025:
                    sDesc = "[FDO_E_SE_INVALID_DBMS_LOGIN]SDE Error.";
                    break;
                case -2147216024:
                    sDesc = "[FDO_E_SE_PASSWORD_TIMEOUT]SDE Error.";
                    break;
                case -2147216023:
                    sDesc = "[FDO_E_SE_INVALID_SERVER]SDE Error.";
                    break;
                case -2147216022:
                    sDesc = "[FDO_E_SE_IOMGR_NOT_AVAILABLE]SDE Error.";
                    break;
                case -2147216021:
                    sDesc = "[FDO_E_SE_SERVICE_NOT_FOUND]SDE Error.";
                    break;
                case -2147216020:
                    sDesc = "[FDO_E_SE_INVALID_STATS_TYPE]SDE Error.";
                    break;
                case -2147216019:
                    sDesc = "[FDO_E_SE_INVALID_DISTINCT_TYPE]SDE Error.";
                    break;
                case -2147216018:
                    sDesc = "[FDO_E_SE_INVALID_GRANT_REVOKE]SDE Error.";
                    break;
                case -2147216017:
                    sDesc = "[FDO_E_SE_INVALID_SDEHOME]SDE Error.";
                    break;
                case -2147216016:
                    sDesc = "[FDO_E_SE_INVALID_STREAM]SDE Error.";
                    break;
                case -2147216015:
                    sDesc = "[FDO_E_SE_TOO_MANY_STREAMS]SDE Error.";
                    break;
                case -2147216014:
                    sDesc = "[FDO_E_SE_OUT_OF_MUTEXES]SDE Error.";
                    break;
                case -2147216013:
                    sDesc = "[FDO_E_SE_CONNECTION_LOCKED]SDE Error.";
                    break;
                case -2147216012:
                    sDesc = "[FDO_E_SE_CONNECTION_IN_USE]SDE Error.";
                    break;
                case -2147216011:
                    sDesc = "[FDO_E_SE_NOT_A_SELECT_STATEMENT]SDE Error.";
                    break;
                case -2147216010:
                    sDesc = "[FDO_E_SE_FUNCTION_SEQUENCE_ERROR]SDE Error.";
                    break;
                case -2147216009:
                    sDesc = "[FDO_E_SE_WRONG_COLUMN_TYPE]SDE Error.";
                    break;
                case -2147216008:
                    sDesc = "[FDO_E_SE_PTABLE_LOCKED]SDE Error.";
                    break;
                case -2147216007:
                    sDesc = "[FDO_E_SE_PTABLE_IN_USE]SDE Error.";
                    break;
                case -2147216006:
                    sDesc = "[FDO_E_SE_STABLE_LOCKED]SDE Error.";
                    break;
                case -2147216005:
                    sDesc = "[FDO_E_SE_STABLE_IN_USE]SDE Error.";
                    break;
                case -2147216004:
                    sDesc = "[FDO_E_SE_INVALID_FILTER_TYPE]SDE Error.";
                    break;
                case -2147216003:
                    sDesc = "[FDO_E_SE_NO_CAD]SDE Error.";
                    break;
                case -2147216002:
                    sDesc = "[FDO_E_SE_INSTANCE_NOT_AVAILABLE]SDE Error.";
                    break;
                case -2147216001:
                    sDesc = "[FDO_E_SE_INSTANCE_TOO_EARLY]SDE Error.";
                    break;
                case -2147216000:
                    sDesc = "[FDO_E_SE_INVALID_SYSTEM_UNITS]SDE Error.";
                    break;
                case -2147215999:
                    sDesc = "[FDO_E_SE_INVALID_UNITS]SDE Error.";
                    break;
                case -2147215998:
                    sDesc = "[FDO_E_SE_INVALID_CAD_OBJECT]SDE Error.";
                    break;
                case -2147215997:
                    sDesc = "[FDO_E_SE_VERSION_NOEXIST]SDE Error.";
                    break;
                case -2147215996:
                    sDesc = "[FDO_E_SE_INVALID_SPATIAL_CONSTRAINT]SDE Error.";
                    break;
                case -2147215995:
                    sDesc = "[FDO_E_SE_INVALID_STREAM_TYPE]SDE Error.";
                    break;
                case -2147215994:
                    sDesc = "[FDO_E_SE_INVALID_SPATIAL_COLUMN]SDE Error.";
                    break;
                case -2147215993:
                    sDesc = "[FDO_E_SE_NO_SPATIAL_MASKS]SDE Error.";
                    break;
                case -2147215992:
                    sDesc = "[FDO_E_SE_IOMGR_NOT_FOUND]SDE Error.";
                    break;
                case -2147215991:
                    sDesc = "[FDO_E_SE_SYSTEM_IS_CLIENT_ONLY]SDE Error.";
                    break;
                case -2147215990:
                    sDesc = "[FDO_E_SE_MULTIPLE_SPATIAL_COLS]SDE Error.";
                    break;
                case -2147215989:
                    sDesc = "[FDO_E_SE_INVALID_SHAPE_OBJECT]SDE Error.";
                    break;
                case -2147215988:
                    sDesc = "[FDO_E_SE_INVALID_PARTNUM]SDE Error.";
                    break;
                case -2147215987:
                    sDesc = "[FDO_E_SE_INCOMPATIBLE_SHAPES]SDE Error.";
                    break;
                case -2147215986:
                    sDesc = "[FDO_E_SE_INVALID_PART_OFFSET]SDE Error.";
                    break;
                case -2147215985:
                    sDesc = "[FDO_E_SE_INCOMPATIBLE_COORDREFS]SDE Error.";
                    break;
                case -2147215984:
                    sDesc = "[FDO_E_SE_COORD_OUT_OF_BOUNDS]SDE Error.";
                    break;
                case -2147215983:
                    sDesc = "[FDO_E_SE_LAYER_CACHE_FULL]SDE Error.";
                    break;
                case -2147215982:
                    sDesc = "[FDO_E_SE_INVALID_COORDREF_OBJECT]SDE Error.";
                    break;
                case -2147215981:
                    sDesc = "[FDO_E_SE_INVALID_COORDSYS_ID]SDE Error.";
                    break;
                case -2147215980:
                    sDesc = "[FDO_E_SE_INVALID_COORDSYS_DESC]SDE Error.";
                    break;
                case -2147215979:
                    sDesc = "[FDO_E_SE_INVALID_ROW_ID_LAYER]SDE Error.";
                    break;
                case -2147215978:
                    sDesc = "[FDO_E_SE_PROJECTION_ERROR]SDE Error.";
                    break;
                case -2147215977:
                    sDesc = "[FDO_E_SE_ARRAY_BYTES_EXCEEDED]SDE Error.";
                    break;
                case -2147215976:
                    sDesc = "[FDO_E_SE_POLY_SHELLS_OVERLAP]SDE Error.";
                    break;
                case -2147215975:
                    sDesc = "[FDO_E_SE_TOO_FEW_POINTS]SDE Error.";
                    break;
                case -2147215974:
                    sDesc = "[FDO_E_SE_INVALID_PART_SEPARATOR]SDE Error.";
                    break;
                case -2147215973:
                    sDesc = "[FDO_E_SE_INVALID_POLYGON_CLOSURE]SDE Error.";
                    break;
                case -2147215972:
                    sDesc = "[FDO_E_SE_INVALID_OUTER_SHELL]SDE Error.";
                    break;
                case -2147215971:
                    sDesc = "[FDO_E_SE_ZERO_AREA_POLYGON]SDE Error.";
                    break;
                case -2147215970:
                    sDesc = "[FDO_E_SE_POLYGON_HAS_VERTICAL_LINE]SDE Error.";
                    break;
                case -2147215969:
                    sDesc = "[FDO_E_SE_OUTER_SHELLS_OVERLAP]SDE Error.";
                    break;
                case -2147215968:
                    sDesc = "[FDO_E_SE_SELF_INTERSECTING]SDE Error.";
                    break;
                case -2147215967:
                    sDesc = "[FDO_E_SE_INVALID_EXPORT_FILE]SDE Error.";
                    break;
                case -2147215966:
                    sDesc = "[FDO_E_SE_READ_ONLY_SHAPE]SDE Error.";
                    break;
                case -2147215965:
                    sDesc = "[FDO_E_SE_INVALID_DATA_SOURCE]SDE Error.";
                    break;
                case -2147215964:
                    sDesc = "[FDO_E_SE_INVALID_STREAM_SPEC]SDE Error.";
                    break;
                case -2147215963:
                    sDesc = "[FDO_E_SE_INVALID_ALTER_OPERATION]SDE Error.";
                    break;
                case -2147215962:
                    sDesc = "[FDO_E_SE_INVALID_SPATIAL_COL_NAME]SDE Error.";
                    break;
                case -2147215961:
                    sDesc = "[FDO_E_SE_INVALID_DATABASE]SDE Error.";
                    break;
                case -2147215960:
                    sDesc = "[FDO_E_SE_SPATIAL_SQL_NOT_INSTALLED]SDE Error.";
                    break;
                case -2147215959:
                    sDesc = "[FDO_E_SE_NORM_DIM_INFO_NOT_FOUND]SDE Error.";
                    break;
                case -2147215958:
                    sDesc = "[FDO_E_SE_NORM_DIM_TAB_VALUE_NOT_FOUND]SDE Error.";
                    break;
                case -2147215957:
                    sDesc = "[FDO_E_SE_UNSUPPORTED_NORMALIZED_OPERATION]SDE Error.";
                    break;
                case -2147215956:
                    sDesc = "[FDO_E_SE_INVALID_REGISTERED_LAYER_OPTION]SDE Error.";
                    break;
                case -2147215955:
                    sDesc = "[FDO_E_SE_READ_ONLY]SDE Error.";
                    break;
                case -2147215954:
                    sDesc = "[FDO_E_SE_NO_SDE_ROWID_COLUMN]SDE Error.";
                    break;
                case -2147215953:
                    sDesc = "[FDO_E_SE_READ_ONLY_COLUMN]SDE Error.";
                    break;
                case -2147215952:
                    sDesc = "[FDO_E_SE_INVALID_VERSION_NAME]SDE Error.";
                    break;
                case -2147215951:
                    sDesc = "[FDO_E_SE_STATE_NOEXIST]SDE Error.";
                    break;
                case -2147215950:
                    sDesc = "[FDO_E_SE_INVALID_STATEINFO_OBJECT]SDE Error.";
                    break;
                case -2147215949:
                    sDesc = "[FDO_E_SE_VERSION_HAS_MOVED]SDE Error.";
                    break;
                case -2147215948:
                    sDesc = "[FDO_E_SE_STATE_HAS_CHILDREN]SDE Error.";
                    break;
                case -2147215947:
                    sDesc = "[FDO_E_SE_PARENT_NOT_CLOSED]SDE Error.";
                    break;
                case -2147215946:
                    sDesc = "[FDO_E_SE_VERSION_EXISTS]SDE Error.";
                    break;
                case -2147215945:
                    sDesc = "[FDO_E_SE_TABLE_NOT_MULTIVERSION]SDE Error.";
                    break;
                case -2147215944:
                    sDesc = "[FDO_E_SE_STATE_USED_BY_VERSION]SDE Error.";
                    break;
                case -2147215943:
                    sDesc = "[FDO_E_SE_INVALID_VERSIONINFO_OBJECT]SDE Error.";
                    break;
                case -2147215942:
                    sDesc = "[FDO_E_SE_INVALID_STATE_ID]SDE Error.";
                    break;
                case -2147215941:
                    sDesc = "[FDO_E_SE_SDETRACELOC_NOT_SET]SDE Error.";
                    break;
                case -2147215940:
                    sDesc = "[FDO_E_SE_ERROR_LOADING_SSA]SDE Error.";
                    break;
                case -2147215939:
                    sDesc = "[FDO_E_SE_TOO_MANY_STATES]SDE Error.";
                    break;
                case -2147215938:
                    sDesc = "[FDO_E_SE_STATES_ARE_SAME]SDE Error.";
                    break;
                case -2147215937:
                    sDesc = "[FDO_E_SE_NO_ROWID_COLUMN]SDE Error.";
                    break;
                case -2147215936:
                    sDesc = "[FDO_E_SE_NO_STATE_SET]SDE Error.";
                    break;
                case -2147215935:
                    sDesc = "[FDO_E_SE_SSA_FUNCTION_ERROR]SDE Error.";
                    break;
                case -2147215934:
                    sDesc = "[FDO_E_SE_INVALID_REGINFO_OBJECT]SDE Error.";
                    break;
                case -2147215933:
                    sDesc = "[FDO_E_SE_NO_COMMON_LINEAGE]SDE Error.";
                    break;
                case -2147215932:
                    sDesc = "[FDO_E_SE_STATE_INUSE]SDE Error.";
                    break;
                case -2147215931:
                    sDesc = "[FDO_E_SE_STATE_TREE_INUSE]SDE Error.";
                    break;
                case -2147215930:
                    sDesc = "[FDO_E_SE_INVALID_RASTER_COLUMN]SDE Error.";
                    break;
                case -2147215929:
                    sDesc = "[FDO_E_SE_RASTERCOLUMN_EXISTS]SDE Error.";
                    break;
                case -2147215928:
                    sDesc = "[FDO_E_SE_INVALID_MVTABLE_INDEX]SDE Error.";
                    break;
                case -2147215927:
                    sDesc = "[FDO_E_SE_INVALID_STORAGE_TYPE]SDE Error.";
                    break;
                case -2147215926:
                    sDesc = "[FDO_E_SE_AMBIGUOUS_NIL_SHAPE]SDE Error.";
                    break;
                case -2147215925:
                    sDesc = "[FDO_E_SE_INVALID_BYTE_ORDER]SDE Error.";
                    break;
                case -2147215924:
                    sDesc = "[FDO_E_SE_INVALID_GEOMETRY_TYPE]SDE Error.";
                    break;
                case -2147215923:
                    sDesc = "[FDO_E_SE_INVALID_NUM_MEASURES]SDE Error.";
                    break;
                case -2147215922:
                    sDesc = "[FDO_E_SE_INVALID_NUM_PARTS]SDE Error.";
                    break;
                case -2147215921:
                    sDesc = "[FDO_E_SE_BINARY_TOO_SMALL]SDE Error.";
                    break;
                case -2147215920:
                    sDesc = "[FDO_E_SE_SHAPE_TEXT_TOO_LONG]SDE Error.";
                    break;
                case -2147215919:
                    sDesc = "[FDO_E_SE_SHAPE_TEXT_ERROR]SDE Error.";
                    break;
                case -2147215918:
                    sDesc = "[FDO_E_SE_TOO_MANY_PARTS]SDE Error.";
                    break;
                case -2147215917:
                    sDesc = "[FDO_E_SE_TYPE_MISMATCH]SDE Error.";
                    break;
                case -2147215916:
                    sDesc = "[FDO_E_SE_SQL_PARENTHESIS_MISMATCH]SDE Error.";
                    break;
                case -2147215915:
                    sDesc = "[FDO_E_SE_NIL_SHAPE_NOT_ALLOWED]SDE Error.";
                    break;
                case -2147215914:
                    sDesc = "[FDO_E_SE_INSTANCE_ALREADY_RUNNING]SDE Error.";
                    break;
                case -2147215913:
                    sDesc = "[FDO_E_SE_UNSUPPORTED_OPERATION]SDE Error.";
                    break;
                case -2147215912:
                    sDesc = "[FDO_E_SE_INVALID_EXTERNAL_LAYER_OPTION]SDE Error.";
                    break;
                case -2147215911:
                    sDesc = "[FDO_E_SE_NORMALIZE_VALUE_NOT_FOUND]SDE Error.";
                    break;
                case -2147215910:
                    sDesc = "[FDO_E_SE_INVALID_QUERY_TYPE]SDE Error.";
                    break;
                case -2147215909:
                    sDesc = "[FDO_E_SE_NO_TRACE_LIBRARY]SDE Error.";
                    break;
                case -2147215908:
                    sDesc = "[FDO_E_SE_TRACE_ON]SDE Error.";
                    break;
                case -2147215907:
                    sDesc = "[FDO_E_SE_TRACE_OFF]SDE Error.";
                    break;
                case -2147215906:
                    sDesc = "[FDO_E_SE_SCL_SYNTAX_ERROR]SDE Error.";
                    break;
                case -2147215905:
                    sDesc = "[FDO_E_SE_TABLE_REGISTERED]SDE Error.";
                    break;
                case -2147215904:
                    sDesc = "[FDO_E_SE_INVALID_REGISTRATION_ID]SDE Error.";
                    break;
                case -2147215903:
                    sDesc = "[FDO_E_SE_TABLE_NOREGISTERED]SDE Error.";
                    break;
                case -2147215902:
                    sDesc = "[FDO_E_SE_TOO_MANY_REGISTRATIONS]SDE Error.";
                    break;
                case -2147215901:
                    sDesc = "[FDO_E_SE_DELETE_NOT_ALLOWED]SDE Error.";
                    break;
                case -2147215898:
                    sDesc = "[FDO_E_SE_RASTERCOLUMN_INUSE]SDE Error.";
                    break;
                case -2147215897:
                    sDesc = "[FDO_E_SE_RASTERCOLUMN_NOEXIST]SDE Error.";
                    break;
                case -2147215896:
                    sDesc = "[FDO_E_SE_INVALID_RASTERCOLUMN_NUMBER]SDE Error.";
                    break;
                case -2147215895:
                    sDesc = "[FDO_E_SE_TOO_MANY_RASTERCOLUMNS]SDE Error.";
                    break;
                case -2147215894:
                    sDesc = "[FDO_E_SE_INVALID_RASTER_NUMBER]SDE Error.";
                    break;
                case -2147215893:
                    sDesc = "[FDO_E_SE_NO_REQUEST_STATUS]SDE Error.";
                    break;
                case -2147215892:
                    sDesc = "[FDO_E_SE_NO_REQUEST_RESULTS]SDE Error.";
                    break;
                case -2147215891:
                    sDesc = "[FDO_E_SE_RASTERBAND_EXISTS]SDE Error.";
                    break;
                case -2147215890:
                    sDesc = "[FDO_E_SE_RASTERBAND_NOEXIST]SDE Error.";
                    break;
                case -2147215889:
                    sDesc = "[FDO_E_SE_RASTER_EXISTS]SDE Error.";
                    break;
                case -2147215888:
                    sDesc = "[FDO_E_SE_RASTER_NOEXIST]SDE Error.";
                    break;
                case -2147215887:
                    sDesc = "[FDO_E_SE_TOO_MANY_RASTERBANDS]SDE Error.";
                    break;
                case -2147215886:
                    sDesc = "[FDO_E_SE_TOO_MANY_RASTERS]SDE Error.";
                    break;
                case -2147215885:
                    sDesc = "[FDO_E_SE_VIEW_EXISTS]SDE Error.";
                    break;
                case -2147215884:
                    sDesc = "[FDO_E_SE_VIEW_NOEXIST]SDE Error.";
                    break;
                case -2147215883:
                    sDesc = "[FDO_E_SE_LOCK_EXISTS]SDE Error.";
                    break;
                case -2147215882:
                    sDesc = "[FDO_E_SE_ROWLOCK_MASK_CONFLICT]SDE Error.";
                    break;
                case -2147215881:
                    sDesc = "[FDO_E_SE_NOT_IN_RASTER]SDE Error.";
                    break;
                case -2147215880:
                    sDesc = "[FDO_E_SE_INVALID_RASBANDINFO_OBJECT]SDE Error.";
                    break;
                case -2147215879:
                    sDesc = "[FDO_E_SE_INVALID_RASCOLINFO_OBJECT]SDE Error.";
                    break;
                case -2147215878:
                    sDesc = "[FDO_E_SE_INVALID_RASTERINFO_OBJECT]SDE Error.";
                    break;
                case -2147215877:
                    sDesc = "[FDO_E_SE_INVALID_RASTERBAND_NUMBER]SDE Error.";
                    break;
                case -2147215876:
                    sDesc = "[FDO_E_SE_MULTIPLE_RASTER_COLS]SDE Error.";
                    break;
                case -2147215875:
                    sDesc = "[FDO_E_SE_TABLE_SCHEMA_IS_LOCKED]SDE Error.";
                    break;
                case -2147215874:
                    sDesc = "[FDO_E_SE_INVALID_LOGINFO_OBJECT]SDE Error.";
                    break;
                case -2147215873:
                    sDesc = "[FDO_E_SE_SQL_TOO_LONG]SDE Error.";
                    break;
                case -2147215872:
                    sDesc = "[FDO_E_SE_UNSUPPORTED_ON_VIEW]SDE Error.";
                    break;
                case -2147215871:
                    sDesc = "[FDO_E_SE_LOG_EXISTS]SDE Error.";
                    break;
                case -2147215870:
                    sDesc = "[FDO_E_SE_SDE_WARNING]SDE Error.";
                    break;
                case -2147215869:
                    sDesc = "[FDO_E_SE_ETYPE_CHANGED]SDE Error.";
                    break;
                case -2147215868:
                    sDesc = "[FDO_E_SE_NO_ROWS_DELETED]SDE Error.";
                    break;
                case -2147215867:
                    sDesc = "[FDO_E_SE_TOO_MANY_DISTINCTS]SDE Error.";
                    break;
                case -2147215866:
                    sDesc = "[FDO_E_SE_NULL_VALUE]SDE Error.";
                    break;
                case -2147215865:
                    sDesc = "[FDO_E_SE_NO_ROWS_UPDATED]SDE Error.";
                    break;
                case -2147215864:
                    sDesc = "[FDO_E_SE_NO_CPGCVT]SDE Error.";
                    break;
                case -2147215863:
                    sDesc = "[FDO_E_SE_NO_CPGHOME]SDE Error.";
                    break;
                case -2147215862:
                    sDesc = "[FDO_E_SE_DBMS_DOES_NOT_SUPPORT]SDE Error.";
                    break;
                case -2147215861:
                    sDesc = "[FDO_E_SE_ROWLOCKING_ENABLED]SDE Error.";
                    break;
                case -2147215860:
                    sDesc = "[FDO_E_SE_ROWLOCKING_NOT_ENABLED]SDE Error.";
                    break;
                case -2147215859:
                    sDesc = "[FDO_E_SE_LOG_IS_OPEN]SDE Error.";
                    break;
                case -2147215858:
                    sDesc = "[FDO_E_SE_SPATIALREF_EXISTS]SDE Error.";
                    break;
                case -2147215857:
                    sDesc = "[FDO_E_SE_SPATIALREF_NOEXIST]SDE Error.";
                    break;
                case -2147215856:
                    sDesc = "[FDO_E_SE_SPATIALREF_IN_USE]SDE Error.";
                    break;
                case -2147215855:
                    sDesc = "[FDO_E_SE_INVALID_SPATIALREFINFO_OBJECT]SDE Error.";
                    break;
                case -2147215854:
                    sDesc = "[FDO_E_SE_INVALID_FUNCTION_ID]SDE Error.";
                    break;
                case -2147215853:
                    sDesc = "[FDO_E_SE_MOSAIC_NOT_ALLOWED]SDE Error.";
                    break;
                case -2147215791:
                    sDesc = "[FDO_E_INVALID_CONNECTIVITY_RULE]The connectivity rule is invalid/malformed.";
                    break;
                case -2147215790:
                    sDesc = "[FDO_E_VALIDATION_NOT_SUPPORTED]Validation not supported on non-SQL datasets.";
                    break;
                case -2147215789:
                    sDesc = "[FDO_E_INVALID_CARDINALITY]The cardinality specified for the connectivity rule is invalid.";
                    break;
                case -2147215788:
                    sDesc = "[FDO_E_DEFAULT_JUNCTIONS_NOT_SUPPORTED_IN_RELEASE]Default junctions not supported in this release of the GeoDatabase.";
                    break;
                case -2147215787:
                    sDesc = "[FDO_E_ALTERING_RULE_NOT_SUPPORTED]Altering this type of validation rule not supported.";
                    break;
                case -2147215786:
                    sDesc = "[FDO_E_CONNECTIVITY_RULES_NOT_SUPPORTED]Connectivity rules are not supported on simple features.";
                    break;
                case -2147215785:
                    sDesc = "[FDO_E_CANNOT_ALTER_NON_EXISTANT_RULE]Cannot alter a non-existant rule.";
                    break;
                case -2147215784:
                    sDesc = "[FDO_E_RULE_NOT_FOUND]Validation rule not found.";
                    break;
                case -2147215615:
                    sDesc = "[FDO_E_ZERO_LENGTH_POLYLINE]Zero-length polylines not allowed.";
                    break;
                case -2147215614:
                    sDesc = "[FDO_E_CLOSED_POLYLINE]Closed polylines not allowed.";
                    break;
                case -2147215613:
                    sDesc = "[FDO_E_NO_NETWORK_ANCILLARY_ROLE]Junction feature does not have network ancillary role.";
                    break;
                case -2147215612:
                    sDesc = "[FDO_E_FLIPPED_POLYLINE]Flipping polylines not allowed.";
                    break;
                case -2147215611:
                    sDesc = "[FDO_E_CANNOT_SPLIT_JUNCTION]Splitting junction features not allowed.";
                    break;
                case -2147215610:
                    sDesc = "[FDO_E_INVALID_NETWORK_ANCILLARY_ROLE]Invalid network ancillary role.";
                    break;
                case -2147215609:
                    sDesc = "[FDO_E_CANNOT_ADD_ORPHAN_JUNCTION_ON_EXISTING_JUNCTION]Cannot add an orphan junction on top of an existing junction.";
                    break;
                case -2147215608:
                    sDesc = "[FDO_E_INVALID_JUNCTION_INDEX]The specified junction index is invalid.";
                    break;
                case -2147215607:
                    sDesc = "[FDO_E_CANNOT_SET_ENABLED_FIELD]Unable to set the enabled field associated with a network element.";
                    break;
                case -2147215606:
                    sDesc = "[FDO_E_CANNOT_SET_WEIGHT_FIELD]Unable to set the weight field associated with a network element.";
                    break;
                case -2147215605:
                    sDesc = "[FDO_E_INVALID_GEOMETRY_FOR_COMPLEX_JUNCTION]An invalid type of geometry is being set into a complex junction.";
                    break;
                case -2147215604:
                    sDesc = "[FDO_E_INVALID_GEOMETRY_TYPE_FOR_NETWORK_FEATURE_CLASS]An invalid geometry type is associated with a network feature class.";
                    break;
                case -2147215603:
                    sDesc = "[FDO_E_NO_ASSOCIATED_NETWORK_ELEMENT]The network feature does not have an associated network element.";
                    break;
                case -2147215602:
                    sDesc = "[FDO_E_IDENTICAL_FROM_TO_JUNCTIONS]The edge feature has the same from and to junctions.";
                    break;
                case -2147215601:
                    sDesc = "[FDO_E_EDGE_MISSING_ENDPOINT_JUNCTION]The edge feature is missing either a from or to junction.";
                    break;
                case -2147215600:
                    sDesc = "[FDO_E_CONNECTED_EDGE_INVALID_CONNECTIVITY]The connected edge feature has invalid connectivity.";
                    break;
                case -2147215535:
                    sDesc = "[FDO_E_DOMAIN_NOT_FOUND]The domain was not found.";
                    break;
                case -2147215534:
                    sDesc = "[FDO_E_DOMAIN_USED_BY_ATTRIBUTE_RULE]The domain is used by an attribute rule.";
                    break;
                case -2147215533:
                    sDesc = "[FDO_E_DOMAIN_USED_AS_DEFAULT_DOMAIN]The domain is used as a default domain.";
                    break;
                case -2147215532:
                    sDesc = "[FDO_E_DOMAIN_NAME_ALREADY_EXISTS]Domain name already in use.";
                    break;
                case -2147215531:
                    sDesc = "[FDO_E_DOMAIN_VALUE_EXCEEDS_FIELD_LENGTH]The value of the domain exceeds the length of the field.";
                    break;
                case -2147215530:
                    sDesc = "[FDO_E_DOMAIN_OWNER_DOESNT_MATCH]The existing domain owner does not match that of the updated domain.";
                    break;
                case -2147215529:
                    sDesc = "[FDO_E_DOMAIN_FIELD_TYPE_DOESNT_MATCH]The existing domain field type does not match that of the updated domain.";
                    break;
                case -2147215528:
                    sDesc = "[FDO_E_DOMAIN_TYPE_NOT_SUPPORTED]The domain type is not supported.";
                    break;
                case -2147215527:
                    sDesc = "[FDO_E_CODED_VALUE_DOMAIN_VALUE_ALREADY_EXISTS]The value being added to the CodedValueDomain already exists.";
                    break;
                case -2147215526:
                    sDesc = "[FDO_E_CANNOT_LOCK_COCREATED_DOMAIN]Client cocreated domains may not be locked.";
                    break;
                case -2147215525:
                    sDesc = "[FDO_E_DOMAIN_USED_BY_OTHER_WORKSPACE]The domain is already used by another workspace.";
                    break;
                case -2147215524:
                    sDesc = "[FDO_E_DOMAIN_FIELD_TYPE_MISMATCH]The domain field type does not match that of the field it is being assigned to.";
                    break;
                case -2147215523:
                    sDesc = "[FDO_E_CANNOT_LOCK_DOMAIN_AS_NOT_OWNER]Domain may not be locked as the user is not the owner.";
                    break;
                case -2147215522:
                    sDesc = "[FDO_E_DEFAULT_DOMAIN_NOT_FOUND]The specified default domain was not found.";
                    break;
                case -2147215521:
                    sDesc = "[FDO_E_DOMAIN_RECORD_NOT_FOUND]The domain record was not found.";
                    break;
                case -2147215520:
                    sDesc = "[FDO_E_DOMAIN_TYPE_DOESNT_MATCH]The existing domain type does not match that of the updated domain.";
                    break;
                case -2147215359:
                    sDesc = "[FDO_E_DEFAULT_VALUE_NOT_NULLABLE]The default value is not nullable.";
                    break;
                case -2147215358:
                    sDesc = "[FDO_E_DEFAULT_VALUE_INVALID]The default value is not valid in the domain.";
                    break;
                case -2147215279:
                    sDesc = "[FDO_E_OBJECT_IN_ANOTHER_FEATUREDATASET]An object being transfered is in another Feature Dataset.";
                    break;
                case -2147215103:
                    sDesc = "[FDO_E_XML_PARSE_ERROR]The XML being loaded could not be parsed.";
                    break;
                case -2147215023:
                    sDesc = "[FDO_E_TOPOLOGY_ILLEGAL_RESHAPE]Can only reshape one edge at a time.";
                    break;
                case -2147215022:
                    sDesc = "[FDO_E_TOPOLOGY_ALREADY_EXISTS]The topology with the specified name already exists.";
                    break;
                case -2147215021:
                    sDesc = "[FDO_E_TOPOLOGY_NOT_FOUND]The topology was not found.";
                    break;
                case -2147215020:
                    sDesc = "[FDO_E_TOPOLOGY_CANNOT_RENAME]The topology cannot be renamed.";
                    break;
                case -2147215019:
                    sDesc = "[FDO_E_INVALID_FEATURE_TYPE_FOR_TOPOLOGY]The feature class in not simple.";
                    break;
                case -2147215018:
                    sDesc = "[FDO_E_INVALID_GEOMETRY_TYPE_FOR_TOPOLOGY]The feature class has an invalid geometry type.";
                    break;
                case -2147215017:
                    sDesc = "[FDO_E_INVALID_TOPOLOGY_RULE]The topology rule in invalid or malformed.";
                    break;
                case -2147215016:
                    sDesc = "[FDO_E_TOPOLOGY_WORKSPACE_EXTENSION_NOT_FOUND]The topology workspace extension was not found.";
                    break;
                case -2147215015:
                    sDesc = "[FDO_E_CANNOT_RESET_CLUSTER_TOLERANCE]The topology cluster tolerance cannot be reset.";
                    break;
                case -2147215014:
                    sDesc = "[FDO_E_TOPOLOGIES_NOT_SUPPORTED_IN_RELEASE]Topologies not supported in this release of the GeoDatabase.";
                    break;
                case -2147215013:
                    sDesc = "[FDO_E_TOPOLOGY_INVALID_WEIGHT]Feature class weight is invalid.";
                    break;
                case -2147215012:
                    sDesc = "[FDO_E_CANNOT_MODIFY_TOPOLOGY_ERROR_FEATURE]Topology errors cannot be directly modified.";
                    break;
                case -2147215011:
                    sDesc = "[FDO_E_TOPOCLASSES_SYSTEM_TABLE_INCONSISTENCY]Geodatabase TopoClasses system table inconsistent.";
                    break;
                case -2147215010:
                    sDesc = "[FDO_E_INVALID_CLUSTER_TOLERANCE]The specified cluster tolerance is invalid.";
                    break;
                case -2147215009:
                    sDesc = "[FDO_E_INVALID_GEOMETRY_TYPE_FOR_TOPOLOGY_RULE]The feature class has an invalid geometry type for the topology rule.";
                    break;
                case -2147215008:
                    sDesc = "[FDO_E_NOT_SUPPORTED_ON_TOPOLOGY_ERROR_FEATURE]This operation is not supported on topology errors.";
                    break;
                case -2147215007:
                    sDesc = "[FDO_E_CANNOT_MODIFY_TOPOLOGY_TABLES]Topology tables cannot be modified.";
                    break;
                case -2147215006:
                    sDesc = "[FDO_E_TOPOLOGY_EDGE_NOT_SELECTABLE]Topology edge is not selectable.";
                    break;
                case -2147215005:
                    sDesc = "[FDO_E_CLASS_ALREADY_MEMBER_OF_TOPOLOGY]The class is already a member of the topology.";
                    break;
                case -2147215004:
                    sDesc = "[FDO_E_EMPTY_ENVELOPE_FOR_CLEAN]An empty envelope was specified for the clean.";
                    break;
                case -2147215003:
                    sDesc = "[FDO_E_INVALID_TOPOLOGY_ID]Invalid Topology ID.";
                    break;
                case -2147215002:
                    sDesc = "[FDO_E_TOPOLOGY_ENGINE_FAILURE]A failure was detected in the topology engine.";
                    break;
                case -2147215001:
                    sDesc = "[FDO_E_TOPOLOGY_ENGINE_OVERPROC_FAILURE]A failure was detected in the topology engine overlay processor.";
                    break;
                case -2147215000:
                    sDesc = "[FDO_E_INVALID_TOPOLOGY_RULE_TYPE]Invalid topology rule type.";
                    break;
                case -2147214999:
                    sDesc = "[FDO_E_NO_PARTIAL_REBUILD]Cannot currently partially rebuild the topology graph.";
                    break;
                case -2147214998:
                    sDesc = "[FDO_E_CANNOT_ADD_REGISTERED_CLASS_TO_TOPOLOGY]Cannot add a registered as versioned class to the topology.";
                    break;
                case -2147214997:
                    sDesc = "[FDO_E_TOPOLOGY_ERROR_OVERFLOW]The number of errors generated during the topology analysis exceeds the specified threshold.";
                    break;
                case -2147214996:
                    sDesc = "[FDO_E_NETWORK_CANNOT_RENAME]Cannot rename a network.";
                    break;
                case -2147214995:
                    sDesc = "[FDO_E_NETWORK_INVALID_TYPE]Invalid network type.";
                    break;
                case -2147214994:
                    sDesc = "[FDO_E_NETWORK_ALREADY_EXISTS]Network already exists.";
                    break;
                case -2147214993:
                    sDesc = "[FDO_E_NETWORK_INVALID_GEOMETRY_TYPE]Network geometry invalid.";
                    break;
                case -2147214992:
                    sDesc = "[FDO_E_NETWORK_NOT_FOUND]Network not found.";
                    break;
                case -2147214991:
                    sDesc = "[FDO_E_VERSIONING_NOT_SUPPORTED]Versioning not supported.";
                    break;
                case -2147214990:
                    sDesc = "[FDO_E_CLASS_NOT_IN_TOPOLOGIES_FEATURE_DATASET]The class must be in the same feature dataset as the topology.";
                    break;
                case -2147214989:
                    sDesc = "[FDO_E_TOPOLOGY_HAS_NO_CLASSES]The topology does not contain any associated classes.";
                    break;
                case -2147214988:
                    sDesc = "[FDO_E_TOPOLOGY_INVALID_RANK]Feature class rank is invalid.";
                    break;
                case -2147214987:
                    sDesc = "[FDO_E_OUT_OF_PHYSICAL_MEMORY]All available physical memory has been consumed.";
                    break;
                case -2147214986:
                    sDesc = "[FDO_E_TOPOLOGY_OPERATION_CANCELLED]The topology operation was cancelled by the user.";
                    break;
                case -2147214985:
                    sDesc = "[FDO_E_CLASS_NOT_IN_TOPOLOGY]The method is only supported on classes participating in a topology.";
                    break;
                case -2147214984:
                    sDesc = "[FDO_E_MODIFY_EDGE_ENDPOINT]The endpoint of an edge cannot be modified if it is shared by other topology elements.";
                    break;
                case -2147214983:
                    sDesc = "[FDO_E_CANNOT_ADD_STANDALONE_CLASS_TO_TOPOLOGY]Cannot add a class that is not contained in a feature dataset to a topology.";
                    break;
                case -2147214982:
                    sDesc = "[FDO_E_CLASS_IN_TOPOLOGY_REQUIRES_EDIT_SESSION]Updates to feature classes in a topology require an edit session.";
                    break;
                case -2147214981:
                    sDesc = "[FDO_E_CANNOT_ADD_RULE_TO_VERSIONED_TOPOLOGY]A topology rule cannot be added to a versioned topology.";
                    break;
                case -2147214980:
                    sDesc = "[FDO_E_TOPOLOGY_SCHEMA_LOCK_CONFLICT]Cannot acquire a schema lock because of an existing lock; needed when validating outside edit session.";
                    break;
                case -2147214979:
                    sDesc = "[FDO_E_DIRTY_AREA_OUTSIDE_SPATIAL_DOMAIN]Cannot create a dirty area outside the topology's spatial domain.";
                    break;
                case -2147214978:
                    sDesc = "[FDO_E_TOPOLOGY_INVALID_NAME]The topology name is invalid.";
                    break;
                case -2147214977:
                    sDesc = "[FDO_E_TOPOLOGY_ENGINE_TEMP_SPACE_EXHAUSTED]The temporary file system space employed by the topology engine is full.";
                    break;
                case -2147214976:
                    sDesc = "[FDO_E_INCONSISTENT_TOPOLOGY_RULE]The topology rule is inconsistent with other topology rules.";
                    break;
                case -2147214975:
                    sDesc = "[FDO_E_UNSUPPORTED_TOPOLOGY_RULE]Unsupported topology rule type.";
                    break;
                case -2147214974:
                    sDesc = "[FDO_E_INVALID_TOPOLOGY_RULE_CLASS_ASSIGNMENT]Invalid topology rule type.";
                    break;
                case -2147214973:
                    sDesc = "[FDO_E_OPERATION_NOT_SUPPORTED_IN_EDIT_SESSION]The operation is not supported inside an edit session.";
                    break;
                case -2147214972:
                    sDesc = "[FDO_E_TOPOLOGY_EMPTY_GEOMETRY]A topology graph edit operation caused a feature geometry to become empty.";
                    break;
                case -2147214971:
                    sDesc = "[FDO_E_TOPOLOGY_EXTENT_TOO_LARGE]The topology graph cannot be constructed on the specified extent because the requested precision is too high.";
                    break;
                case -2147214970:
                    sDesc = "[FDO_E_Z_CLUSTER_TOLERANCE_NOT_SUPPORTED_IN_RELEASE]Z cluster tolerances are not supported in this release of the GeoDatabase.";
                    break;
                case -2147214969:
                    sDesc = "[FDO_E_OPERATION_REQUIRES_EDIT_SESSION]The operation requires an edit session.";
                    break;
                case -2147214968:
                    sDesc = "[FDO_E_OPERATION_NOT_SUPPORTED_IN_EDIT_OPERATION]The operation is not supported inside an edit session.";
                    break;
                case -2147213231:
                    sDesc = "[FDO_E_NAME_STRING_SYNTAX]The Name String syntax of is incorrect.";
                    break;
                case -2147213055:
                    sDesc = "[FDO_E_INVALID_IMPORT_XML]The input XML is invalid for import into the specified object.";
                    break;
                case -2147213054:
                    sDesc = "[FDO_E_REQUIRED_XML_ELEMENT_NOT_FOUND]The required XML element was not found.";
                    break;
                case -2147212975:
                    sDesc = "[FDO_E_CLASS_NOT_REPLICABLE]The object is not in Replicable format (please configure).";
                    break;
                case -2147212974:
                    sDesc = "[FDO_E_SYNCHRONIZATION_CONFLICTS]Conflicts were detected during synchronization between replica pairs.";
                    break;
                case -2147212973:
                    sDesc = "[FDO_E_CANNOT_SYNCHRONIZE]Cannot synchronize because the replica version has not be reconciled against the sync version.";
                    break;
                case -2147212972:
                    sDesc = "[FDO_E_REPLICA_NOT_FOUND]Replica data was not found.";
                    break;
                case -2147212971:
                    sDesc = "[FDO_E_RECONCILE_FAILED]Error during reconcile.";
                    break;
                case -2147212970:
                    sDesc = "[FDO_E_INVALID_REPLICA]Invalid replica.";
                    break;
                case -2147212719:
                    sDesc = "[FDO_E_NO_DEFAULT_TURN_EVALUATOR]There is no assoicated default turn evaluator.";
                    break;
                case -2147212718:
                    sDesc = "[FDO_E_FEATURE_DATASET_CONTAINERS_NOT_SUPPORTED]Feature dataset network dataset containers are not supported.";
                    break;
                case -2147212717:
                    sDesc = "[FDO_E_NETWORK_DATASET_INVALID_NAME]The network dataset name is invalid.";
                    break;
                case -2147212716:
                    sDesc = "[FDO_E_INVALID_NETWORK_ATTRIBUTE_NAME]The network attribute name is invalid.";
                    break;
                case -2147212715:
                    sDesc = "[FDO_E_INVALID_NETWORK_SOURCE_FOR_EVALUATOR]The network source is invalid for the evaluator.";
                    break;
                case -2147212714:
                    sDesc = "[FDO_E_INVALID_EXPRESSION_FOR_EVALUATOR]The expression is invalid for the evaluator.";
                    break;
                case -2147212713:
                    sDesc = "[FDO_E_INVALID_CONSTANT_FOR_NETWORK_ATTRIBUTE]Invalid constant for attribute data type.";
                    break;
                case -2147212712:
                    sDesc = "[FDO_E_SUBTYPES_REQUIRED]Subtype specification is required (UsesSubtypes is True).";
                    break;
                case -2147212711:
                    sDesc = "[FDO_E_INVALID_NETWORK_DATASET_SCHEMA]Invalid network dataset schema.";
                    break;
                case -2147212710:
                    sDesc = "[FDO_E_NETWORK_OBJECT_EVALUATOR_ERROR]Network object evaluator error.";
                    break;
                case -2147212709:
                    sDesc = "[FDO_E_NETWORK_BAD_EDGE_ORIENTATION]Bad network edge orientation.";
                    break;
                case -2147212708:
                    sDesc = "[FDO_E_NETWORK_DATASET_INVALID_ACCESS]The network dataset has an invalid access mode.";
                    break;
                case -2147212707:
                    sDesc = "[FDO_E_NO_DEFAULT_JUNCTION_EVALUATOR]There is no assoicated default junction evaluator.";
                    break;
                case -2147212706:
                    sDesc = "[FDO_E_NO_DEFAULT_EDGE_EVALUATOR]There is no assoicated default edge evaluator.";
                    break;
                case -2147212705:
                    sDesc = "[FDO_E_NETWORK_DATASET_NOTURNS]The network dataset does not support turns.";
                    break;
                case -2147212704:
                    sDesc = "[FDO_E_NETWORK_ELEMENT_EVALUATOR_ERROR]Network element evaluator error.";
                    break;
                case -2147212703:
                    sDesc = "[FDO_E_NETWORK_SOURCE_DATASET_NOT_FOUND]The dataset for the network source was not found.";
                    break;
                case -2147212702:
                    sDesc = "[FDO_E_INVALID_NETWORK_SOURCE]The source is not valid for the network.";
                    break;
                case -2147212701:
                    sDesc = "[FDO_E_INVALID_NETWORK_ATTRIBUTE]The attribute is not valid for the network.";
                    break;
                case -2147212700:
                    sDesc = "[FDO_E_INVALID_CONNECTIVITY_GROUP_NAME]The connectivity group name is invalid.";
                    break;
                case -2147212699:
                    sDesc = "[FDO_E_SUBTYPES_NOT_IN_USE]Subtype specification is not valid.";
                    break;
                case -2147212698:
                    sDesc = "[FDO_E_NETWORK_DATASETS_NOT_SUPPORTED_IN_RELEASE]Network datasets are not supported in this release of the GeoDatabase.";
                    break;
                case -2147212697:
                    sDesc = "[FDO_E_NETWORK_DATASET_ALREADY_EXISTS]The network dataset with the specified name already exists.";
                    break;
                case -2147212696:
                    sDesc = "[FDO_E_SUBTYPES_UNSPECIFIED_CONN_GROUP]A connectivity group was not specified for one or more subtypes.";
                    break;
                case -2147212695:
                    sDesc = "[FDO_E_SUBTYPES_UNSPECIFIED_CONN_POLICY]A connectivity policy was not specified for one or more subtypes.";
                    break;
                case -2147212694:
                    sDesc = "[FDO_E_INVALID_NETWORK_SOURCE_GEOMETRY_TYPE]The geometry type for the feature class is not valid for the network source.";
                    break;
                case -2147212693:
                    sDesc = "[FDO_E_INVALID_NETWORK_SOURCE_FEATURE_TYPE]The feature type of the feature class is not valid for the network source.";
                    break;
                case -2147212692:
                    sDesc = "[FDO_E_FSTAR_INVALID_FROM_EDGE]The fromEdge is not connected to the AtJunction.";
                    break;
                case -2147212691:
                    sDesc = "[FDO_E_EVALUATOR_CREATE]The evaluator object could not be created.";
                    break;
                case -2147212690:
                    sDesc = "[FDO_E_EVALUATOR_INITIALIZE_DATA]The evaluator failed to initialize data.";
                    break;
                case -2147212689:
                    sDesc = "[FDO_E_EVALUATOR_INITIALIZE_QUERY]The evaluator failed to initialize for queries.";
                    break;
                case -2147212688:
                    sDesc = "[FDO_E_EVALUATOR_QUERY]The evaluator failed to return a value.";
                    break;
                case -2147212687:
                    sDesc = "[FDO_E_INVALID_NETWORK_ELEMENT_ID]The network element id is invalid.";
                    break;
                case -2147212686:
                    sDesc = "[FDO_E_INVALID_NETWORK_EDGE_DIRECTION]The network edge direction is invalid.";
                    break;
                case -2147212685:
                    sDesc = "[FDO_E_INVALID_NETWORK_TURN_TYPE]The network edge direction is invalid.";
                    break;
                case -2147212684:
                    sDesc = "[FDO_E_TURN_NOT_PRESENT]There is no turn present at this adjacency index.";
                    break;
                case -2147212683:
                    sDesc = "[FDO_E_BUILD_NOT_SUPPORTED]Build is not supported on this network.";
                    break;
                case -2147212682:
                    sDesc = "[FDO_E_OPERATION_NOT_SUPPORTED_ON_BUILDABLE_NETWORK]The operation is not supported on a buildable network.";
                    break;
                case -2147212681:
                    sDesc = "[FDO_E_NETWORK_SOURCE_INVALID_NAME]The source name is invalid.";
                    break;
                case -2147212680:
                    sDesc = "[FDO_E_NETWORK_SOURCE_INVALID_ELEMENT_TYPE]The element type for the network source is not valid.";
                    break;
                case -2147212679:
                    sDesc = "[FDO_E_NO_SYSTEM_JUNCTION_SOURCE]There is no system junction source.";
                    break;
                case -2147212678:
                    sDesc = "[FDO_E_BAD_SYSTEM_JUNCTION_SOURCE]The system junction source is bad.";
                    break;
                case -2147212677:
                    sDesc = "[FDO_E_NETWORK_ELEMENT_NOT_INITIALIZED]The network element has not been initialized.";
                    break;
                case -2147212676:
                    sDesc = "[FDO_E_ATTRIBUTES_WITHOUT_SOURCES]Attributes cannot be added to network datasets with no sources.";
                    break;
                case -2147212675:
                    sDesc = "[FDO_E_INVALID_HIERARCHY_RANGES]The hierarchy max range values are invalid.";
                    break;
                case -2147212674:
                    sDesc = "[FDO_E_CANNOT_DELETE_NETWORK_ATTRIBUTES]Network attributes cannot be deleted.";
                    break;
                case -2147212673:
                    sDesc = "[FDO_E_SOURCE_DIRECTIONS_NOT_SUPPORTED]Network source directions not supported on this network source type.";
                    break;
                case -2147212672:
                    sDesc = "[FDO_E_NETWORK_SOURCE_ALREADY_EXISTS]The network source with the specified name already exists.";
                    break;
                case -2147212671:
                    sDesc = "[FDO_E_NETWORK_SOURCE_NAME_DOESNT_EXIST]The network source with the specified name does not exist.";
                    break;
                case -2147212670:
                    sDesc = "[FDO_E_EVALUATOR_CANNOT_BE_DEFAULT_EVALUATOR]The network evaluator cannot be used as a default evaluator.";
                    break;
                case -2147212669:
                    sDesc = "[FDO_E_INVALID_NETWORK_ATTRIBUTE_FOR_EVALUATOR]The network attribute is invalid for the evaluator.";
                    break;
                case -2147212668:
                    sDesc = "[FDO_E_EVALUATOR_NOT_VALIDATED]The network evaluator has not been validated.";
                    break;
                case -2147212667:
                    sDesc = "[FDO_E_EVALUATOR_NOT_VALID]The network evaluator is not valid.";
                    break;
                case -2147212666:
                    sDesc = "[FDO_E_EVALUATOR_NOT_INITIALIZED]The network evaluator has not been initialized.";
                    break;
                case -2147212665:
                    sDesc = "[FDO_E_EVALUATOR_SYNTAX_ERROR]The network evaluator has a syntax error.";
                    break;
                case -2147212664:
                    sDesc = "[FDO_E_FIELD_EVALUATOR_FIELD_NOT_FOUND]The network field evaluator is associated with a field than cannot be found.";
                    break;
                case -2147212663:
                    sDesc = "[FDO_E_INVALID_NETWORK_ATTRIBUTE_ID]The attribute id value is invalid.";
                    break;
                case -2147212662:
                    sDesc = "[FDO_E_NETWORK_SOURCE_INVALID_FULLNAME]The network source fullname property is invalid.";
                    break;
                case -2147212661:
                    sDesc = "[FDO_E_NOT_CONSTANT_EVALUATOR]The evaluator in use is not a constant evaluator.";
                    break;
                case -2147212660:
                    sDesc = "[FDO_E_DIRECTIONAL_EVALUATOR_WITH_JUNCTION_SOURCE]Cannot assign a directional evaluator to a junction source.";
                    break;
                case -2147212659:
                    sDesc = "[FDO_E_INCORRECT_DATA_ELEMENT_TYPE]The data element type is incorrect for the operation.";
                    break;
                case -2147212658:
                    sDesc = "[FDO_E_INVALID_SOURCES_FOR_SHAPEFILE_NETWORK_DATASET]The number or type of sources are invalid for shapefile-based network datasets.";
                    break;
                case -2147212657:
                    sDesc = "[FDO_E_NETWORK_SOURCE_INCONSISTENT_ELEVATION_SPECIFICATION]The network source has an inconsistent elevation field specification.";
                    break;
                case -2147212656:
                    sDesc = "[FDO_E_TURN_NO_NETWORK]The turn feature class does not participate in a network dataset.";
                    break;
                case -2147212655:
                    sDesc = "[FDO_E_TURN_GEOM_TOO_MANY_VERTICES]The geometry references too many edges for the turn feature class.";
                    break;
                case -2147212654:
                    sDesc = "[FDO_E_TURN_NOT_VALID]The current edge sequence is not valid.";
                    break;
                case -2147212653:
                    sDesc = "[FDO_E_TURN_GEOM_NOT_POLYLINE]The geometry must have polyline geometry type.";
                    break;
                case -2147212652:
                    sDesc = "[FDO_E_TURN_GEOM_NOT_ENOUGH_VERTICES]A turn must include at least two edges.";
                    break;
                case -2147212651:
                    sDesc = "[FDO_E_TURN_GEOM_MULTIPART]The geometry cannot be a multipart line.";
                    break;
                case -2147212650:
                    sDesc = "[FDO_E_TURN_GEOM_NO_FIRST_FEATURE]The first vertex could not be snapped to a network edge.";
                    break;
                case -2147212649:
                    sDesc = "[FDO_E_TURN_GEOM_NO_LAST_FEATURE]The last vertex could not be snapped to a network edge.";
                    break;
                case -2147212648:
                    sDesc = "[FDO_E_TURN_GEOM_NO_FEATURES]The turn references a line feature that does not have network edge elements.";
                    break;
                case -2147212647:
                    sDesc = "[FDO_E_TURN_GEOM_DISCONNECTED_FEATURES]The edges are not adjacent in the network dataset.";
                    break;
                case -2147212646:
                    sDesc = "[FDO_E_TURN_GEOM_INVALID_SEQUENCE]The edges are not adjacent in the network dataset in this sequence.";
                    break;
                case -2147212645:
                    sDesc = "[FDO_E_TURN_NOT_ENOUGH_PARTS]A turn must include at least two edges.";
                    break;
                case -2147212644:
                    sDesc = "[FDO_E_TURN_NDS_INTERIOR_EXTERIOR_CONFLICT]A network edge element used in the middle of the turn sequence cannot also be used at the start or end of the sequence.";
                    break;
                case -2147212643:
                    sDesc = "[FDO_E_TURN_NDS_EXTERIOR_LOOP]A turn cannot start or end on a self-loop edge element.";
                    break;
                case -2147212642:
                    sDesc = "[FDO_E_TURN_NO_EDGE_SOURCES]No edge feature sources have been added to the current map.";
                    break;
                case -2147212641:
                    sDesc = "[FDO_E_TURN_INVALID_EDGE1END]The value for the Edge1End field in the turn feature class is invalid.";
                    break;
                case -2147212640:
                    sDesc = "[FDO_E_TURN_GEOM_AMBIGUOUS_FEATURES]The direction of the turn cannot be determined.";
                    break;
                case -2147212639:
                    sDesc = "[FDO_E_NETWORK_MISSING_SOURCE]The network dataset cannot be opened as one of its network sources is missing.";
                    break;
                case -2147212638:
                    sDesc = "[FDO_E_DIRECTIONAL_EVALUATOR_WITH_TURN_SOURCE]Cannot assign a directional evaluator to a turn source.";
                    break;
                case -2147212637:
                    sDesc = "[FDO_E_NETWORK_SOURCE_MISSING_FEATURE_CLASS]Cannot find the feature class associated with the network source.";
                    break;
                case -2147212636:
                    sDesc = "[FDO_E_NETWORK_SOURCE_NOT_SIMPLE_FEATURE_CLASS]The network source must correspond to a simple feature class.";
                    break;
                case -2147212635:
                    sDesc = "[FDO_E_NETWORK_SOURCE_IN_MULTIPLE_NETWORKS]The network source participates in multiple network datasets.";
                    break;
                case -2147212634:
                    sDesc = "[FDO_E_NETWORK_EVALUATOR_CREATE_FAILED]Unable to instantiate the network evaluator component.";
                    break;
                case -2147212633:
                    sDesc = "[FDO_E_FIELD_EVALUATOR_AS_DEFAULT_EVALUATOR]Cannot assign a field evaluator as a default evalautor.";
                    break;
                case -2147212632:
                    sDesc = "[FDO_E_NODIRECTIONAL_EVALUATOR_WITH_EDGE_SOURCE]Cannot assign a non-directional evaluator with an edge feature source.";
                    break;
                case -2147212631:
                    sDesc = "[FDO_E_MISSING_NETWORK_SOURCE_FOR_EVALUATOR]The network source is missing for the evaluator.";
                    break;
                case -2147212630:
                    sDesc = "[FDO_E_TURN_ILLEGAL_START_END_POS]The geometry for a turn cannot start or end at a junction.";
                    break;
                case -2147212629:
                    sDesc = "[FDO_E_CANNOT_DELETE_SYSTEM_JUNCTION_SOURCE]System junction sources cannot be deleted.";
                    break;
                case -2147212628:
                    sDesc = "[FDO_E_TURN_INVALID_EDGE_DESCRIPTOR]The value for one of the edge descriptors is invalid.";
                    break;
                case -2147212627:
                    sDesc = "[FDO_E_TURN_CANNOT_CHANGE_SUPPORT]Turn support cannot be changed on an existing network dataset.";
                    break;
                case -2147212626:
                    sDesc = "[FDO_E_ID_OVERFLOW]The id value cannot be represented in 32 bits.";
                    break;
                case -2147212625:
                    sDesc = "[FDO_E_TURN_INVALID_MAX_EDGES]The value for maximum edges per turn is invalid.";
                    break;
                case -2147212624:
                    sDesc = "[FDO_E_TURN_INVALID_CUR_MAX_EDGES]The value for maximum edges per turn cannot be less than the existing value.";
                    break;
                case -2147212623:
                    sDesc = "[FDO_E_NO_SCRIPT_CONTROL]The script control is unavailable.";
                    break;
                case -2147212622:
                    sDesc = "[FDO_E_TURN_MISSING_EDGE]The vertices of the turn geometry must intersect each edge in the turn.";
                    break;
            }
            #endregion 各种fdoError
            return sDesc;
        }

        public IColor syc_RGBColor(int red, int green, int blue)
        {
            IRgbColor rGB = new RgbColorClass();
            rGB.Red = red;
            rGB.Green = green;
            rGB.Blue = blue;
            rGB.UseWindowsDithering = true;
            return rGB;
        }

        public void syc_DrawPolygonXOR(IMap myMap, IPolygon pPolygon)
        {
            // This function draws a polygon on the screen using a XOR pen
            //  it is used by the buffer routine to quickly draw  and undraw a
            //  polygon on the screen without the need to redraw any other features

            //[01]: construct the symbol
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISymbol pSymbol = pFillSymbol as ISymbol;
            pSymbol.ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPXOrPen;
            IRgbColor pColor = new RgbColorClass();
            pColor.UseWindowsDithering = false;
            pColor.Red = 45;
            pColor.Blue = 45;
            pColor.Green = 45;
            pFillSymbol.Color = pColor;

            ILineSymbol pLineSymbol = pFillSymbol.Outline;
            ((ISymbol)pLineSymbol).ROP2 = ESRI.ArcGIS.Display.esriRasterOpCode.esriROPXOrPen;
            pColor.Red = 145;
            pColor.Green = 145;
            pColor.Blue = 145;
            pColor.UseWindowsDithering = false;
            pLineSymbol.Color = pColor;
            pLineSymbol.Width = 0.1;
            pFillSymbol.Outline = pLineSymbol;

            //[02]:Set the symbol into the display
            IActiveView act = myMap as IActiveView;
            act.ScreenDisplay.StartDrawing(act.ScreenDisplay.hDC, (short)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            act.ScreenDisplay.SetSymbol((ISymbol)pFillSymbol);
            if (pPolygon != null)
                act.ScreenDisplay.DrawPolygon((IGeometry)pPolygon);
            act.ScreenDisplay.FinishDrawing();
        }

        public void syc_DrawPolygon(IMap myMap, IPolygon pPolygon)
        {
            //[01]: construct the symbol
            SimpleFillSymbolClass pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            IRgbColor pColor = new RgbColorClass();
            pColor.Green = 200;
            pFillSymbol.Color = pColor;

            ILineSymbol pLineSymbol = pFillSymbol.Outline;
            pColor.Red = 255;
            pLineSymbol.Color = pColor;
            pLineSymbol.Width = 0.1;
            pFillSymbol.Outline = pLineSymbol;

            //[02]:Set the symbol into the display
            IActiveView act = myMap as IActiveView;
            act.ScreenDisplay.StartDrawing(act.ScreenDisplay.hDC, (short)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            act.ScreenDisplay.SetSymbol((ISymbol)pFillSymbol);
            if (pPolygon != null)
                act.ScreenDisplay.DrawPolygon((IGeometry)pPolygon);
            act.ScreenDisplay.FinishDrawing();
        }

        public void syc_DrawSquareSymbol(IMap myMap, IPoint InPoint)
        {
            //在InPoint处、画一个小方块[被编辑顶点显示使用]
            IEnvelope env = InPoint.Envelope;
            double dd = syc_PixelsToRW(myMap, 6);
            env.Width = dd;
            env.Height = dd;
            env.CenterAt(InPoint);

            //[01]: construct the symbol
            SimpleFillSymbolClass pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 0;
            pColor.Green = 0;
            pColor.Blue = 255;
            pFillSymbol.Color = pColor;

            ILineSymbol pLineSymbol = pFillSymbol.Outline;
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pLineSymbol.Color = pColor;
            pLineSymbol.Width = 0.1;
            pFillSymbol.Outline = pLineSymbol;

            //[02]:Set the symbol into the display
            IActiveView act = myMap as IActiveView;
            act.ScreenDisplay.StartDrawing(act.ScreenDisplay.hDC, (short)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache);
            act.ScreenDisplay.SetSymbol((ISymbol)pFillSymbol);
            if (env != null)
                act.ScreenDisplay.DrawRectangle((IEnvelope)env);
            act.ScreenDisplay.FinishDrawing();
        }

        public double syc_PixelsToRW(IMap myMap, double pixelUnits)
        {
            IActiveView act = myMap as IActiveView;
            IDisplayTransformation pDT = act.ScreenDisplay.DisplayTransformation;
            tagRECT deviceRECT = pDT.get_DeviceFrame();
            int nPixelExtent = deviceRECT.right - deviceRECT.left;
            IEnvelope env = pDT.VisibleBounds;
            double dRealWorldDisplayExtent = env.Width;
            double dSizeOfOnePixel = dRealWorldDisplayExtent / (double)(nPixelExtent);
            double dRet = pixelUnits * dSizeOfOnePixel;
            return dRet;
        }

        public double syc_RWToPixels(IMap myMap, double dRW)
        {
            IActiveView act = myMap as IActiveView;
            IDisplayTransformation pDT = act.ScreenDisplay.DisplayTransformation;
            tagRECT deviceRECT = pDT.get_DeviceFrame();
            int nPixelExtent = deviceRECT.right - deviceRECT.left;
            IEnvelope env = pDT.VisibleBounds;
            double dRealWorldDisplayExtent = env.Width;
            double dPixelOfOneRW = (double)(nPixelExtent) / dRealWorldDisplayExtent;
            double dRet = dRW * dPixelOfOneRW;
            return dRet;
        }

        public void syc_GetNearestObject(IMap myMap, IEnvelope pSearchEnvelope, ref IFeature pNeedFeature)
        {

            //[01]:得到与已知实体交的所有
            ArrayList pObjCol = new ArrayList();
            int nLayerCount = myMap.LayerCount;
            for (int i = 0; i < nLayerCount; i++)
            {
                ILayer currLyr = myMap.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IGeoFeatureLayer pFeatLayer = childLyr as IGeoFeatureLayer;
                            IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
                            ISpatialFilter pFilter = new SpatialFilterClass();
                            pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            IGeometry pSearchGeo = pSearchEnvelope as IGeometry;
                            pFilter.Geometry = pSearchGeo;
                            string sFldName = pFeatClass.ShapeFieldName;
                            pFilter.GeometryField = sFldName;
                            pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                            IFeatureCursor pFeatCursor = pFeatLayer.Search(pFilter, false);
                            IFeature pFeat = pFeatCursor.NextFeature();
                            while (pFeat != null)
                            {
                                pObjCol.Add(pFeat);
                                pFeat = pFeatCursor.NextFeature();
                            }
                        }
                    }
                }
                else if (currLyr is IGeoFeatureLayer)
                {
                    IGeoFeatureLayer pFeatLayer = currLyr as IGeoFeatureLayer;
                    IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
                    ISpatialFilter pFilter = new SpatialFilterClass();
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    IGeometry pSearchGeo = pSearchEnvelope as IGeometry;
                    pFilter.Geometry = pSearchGeo;
                    string sFldName = pFeatClass.ShapeFieldName;
                    pFilter.GeometryField = sFldName;
                    pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                    IFeatureCursor pFeatCursor = pFeatLayer.Search(pFilter, false);
                    IFeature pFeat = pFeatCursor.NextFeature();
                    while (pFeat != null)
                    {
                        pObjCol.Add(pFeat);
                        pFeat = pFeatCursor.NextFeature();
                    }
                }


            } //for(int i=0;...

            //[02]: 把pObjCol中的实体按照离已知点的近远排序
            if (pObjCol.Count != 0)
            {
                PointClass pPoint = new PointClass();
                pPoint.X = pSearchEnvelope.XMin + pSearchEnvelope.Width / 2;
                pPoint.Y = pSearchEnvelope.YMin + pSearchEnvelope.Height / 2;

                IProximityOperator pProximity;
                pProximity = pPoint as IProximityOperator;
                int nNums = pObjCol.Count;
                double[] pDistSz = new double[nNums];
                for (int i = 0; i < nNums; i++)
                {
                    IFeature pFeat = pObjCol[i] as IFeature;
                    IGeometry geo = pFeat.Shape;
                    double dd = pProximity.ReturnDistance(geo);
                    pDistSz[i] = dd;
                }

                for (int i = 0; i < nNums; i++)
                {
                    for (int j = i + 1; j < nNums; j++)
                    {
                        if (pDistSz[i] > pDistSz[j])
                        {
                            double dDist = pDistSz[i];
                            IFeature pFeature = (IFeature)pObjCol[i];
                            pDistSz[i] = pDistSz[j];
                            pObjCol[i] = pObjCol[j];
                            pDistSz[j] = dDist;
                            pObjCol[j] = pFeature;
                        }
                    }
                } //for(i=0;...

                pNeedFeature = pObjCol[0] as IFeature;
            }
            else pNeedFeature = null;
            //... ...
        }

        public void syc_GetNearestObject(IMap myMap, IEnvelope pSearchEnvelope, esriGeometryType esriNeedType, ref IFeature pNeedFeature)
        {
            //得到与已知Envelope最近的、所需类型的特性:

            //[01]:得到与已知实体交的所有
            ArrayList pObjCol = new ArrayList();
            int nLayerCount = myMap.LayerCount;
            for (int i = 0; i < nLayerCount; i++)
            {
                ILayer currLyr = myMap.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureLayer pFeatLayer = childLyr as IFeatureLayer;
                            esriGeometryType curType = pFeatLayer.FeatureClass.ShapeType;
                            if (esriNeedType == curType)
                            {
                                IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
                                ISpatialFilter pFilter = new SpatialFilterClass();
                                pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                                IGeometry pSearchGeo = pSearchEnvelope as IGeometry;
                                pFilter.Geometry = pSearchGeo;
                                string sFldName = pFeatClass.ShapeFieldName;
                                pFilter.GeometryField = sFldName;
                                pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                                IFeatureCursor pFeatCursor = pFeatLayer.Search(pFilter, false);
                                IFeature pFeat = pFeatCursor.NextFeature();
                                while (pFeat != null)
                                {
                                    pObjCol.Add(pFeat);
                                    pFeat = pFeatCursor.NextFeature();
                                }
                            }
                        }
                    }
                }
                else if (currLyr is IGeoFeatureLayer)
                {
                    IFeatureLayer pFeatLayer = currLyr as IFeatureLayer;
                    esriGeometryType curType = pFeatLayer.FeatureClass.ShapeType;
                    if (esriNeedType == curType)
                    {
                        IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
                        ISpatialFilter pFilter = new SpatialFilterClass();
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IGeometry pSearchGeo = pSearchEnvelope as IGeometry;
                        pFilter.Geometry = pSearchGeo;
                        string sFldName = pFeatClass.ShapeFieldName;
                        pFilter.GeometryField = sFldName;
                        pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                        IFeatureCursor pFeatCursor = pFeatLayer.Search(pFilter, false);
                        IFeature pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            pObjCol.Add(pFeat);
                            pFeat = pFeatCursor.NextFeature();
                        }
                    }
                }


            } //for(int i=0;...

            //[02]: 把pObjCol中的实体按照离已知点的近远排序
            if (pObjCol.Count != 0)
            {
                PointClass pPoint = new PointClass();
                pPoint.X = pSearchEnvelope.XMin + pSearchEnvelope.Width / 2;
                pPoint.Y = pSearchEnvelope.YMin + pSearchEnvelope.Height / 2;

                IProximityOperator pProximity;
                pProximity = pPoint as IProximityOperator;
                int nNums = pObjCol.Count;
                double[] pDistSz = new double[nNums];
                for (int i = 0; i < nNums; i++)
                {
                    IFeature pFeat = pObjCol[i] as IFeature;
                    IGeometry geo = pFeat.Shape;
                    double dd = pProximity.ReturnDistance(geo);
                    pDistSz[i] = dd;
                }

                for (int i = 0; i < nNums; i++)
                {
                    for (int j = i + 1; j < nNums; j++)
                    {
                        if (pDistSz[i] > pDistSz[j])
                        {
                            double dDist = pDistSz[i];
                            IFeature pFeature = (IFeature)pObjCol[i];
                            pDistSz[i] = pDistSz[j];
                            pObjCol[i] = pObjCol[j];
                            pDistSz[j] = dDist;
                            pObjCol[j] = pFeature;
                        }
                    }
                } //for(i=0;...

                pNeedFeature = pObjCol[0] as IFeature;
            }
            else pNeedFeature = null;
            //... ...
        }

        public void syc_GetNearestObject(IMap myMap, string sSearchLayer, IEnvelope pSearchEnvelope, ref IFeature pNeedFeature)
        //找sSearchLayer层上、离pSearchEnvelope最近的实体。
        {
            //[01]:得到与已知实体交的所有
            ArrayList pObjCol = new ArrayList();
            int nLayerCount = myMap.LayerCount;
            for (int i = 0; i < nLayerCount; i++)
            {
                ILayer currLyr = myMap.get_Layer(i);
                if (currLyr is IGroupLayer)
                {
                    ICompositeLayer compositeLayer = currLyr as ICompositeLayer;
                    for (int kk = 0; kk < compositeLayer.Count; kk++)
                    {
                        ILayer childLyr = compositeLayer.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IGeoFeatureLayer pFeatLayer = childLyr as IGeoFeatureLayer;
                            string sLayerName = pFeatLayer.Name;
                            if (sLayerName.Equals(sSearchLayer) == true)
                            {
                                IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
                                ISpatialFilter pFilter = new SpatialFilterClass();
                                pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                                IGeometry pSearchGeo = pSearchEnvelope as IGeometry;
                                pFilter.Geometry = pSearchGeo;
                                string sFldName = pFeatClass.ShapeFieldName;
                                pFilter.GeometryField = sFldName;
                                pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                                IFeatureCursor pFeatCursor = pFeatLayer.Search(pFilter, false);
                                IFeature pFeat = pFeatCursor.NextFeature();
                                while (pFeat != null)
                                {
                                    pObjCol.Add(pFeat);
                                    pFeat = pFeatCursor.NextFeature();
                                }
                            }
                        }
                    }
                }
                else if (currLyr is IFeatureLayer)
                {
                    IGeoFeatureLayer pFeatLayer = currLyr as IGeoFeatureLayer;
                    string sLayerName = pFeatLayer.Name;
                    if (sLayerName.Equals(sSearchLayer) == true)
                    {
                        IFeatureClass pFeatClass = pFeatLayer.FeatureClass;
                        ISpatialFilter pFilter = new SpatialFilterClass();
                        pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        IGeometry pSearchGeo = pSearchEnvelope as IGeometry;
                        pFilter.Geometry = pSearchGeo;
                        string sFldName = pFeatClass.ShapeFieldName;
                        pFilter.GeometryField = sFldName;
                        pFilter.set_OutputSpatialReference(sFldName, myMap.SpatialReference);

                        IFeatureCursor pFeatCursor = pFeatLayer.Search(pFilter, false);
                        IFeature pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            pObjCol.Add(pFeat);
                            pFeat = pFeatCursor.NextFeature();
                        }
                    }
                }

            } //for(int i=0;...

            //[02]: 把pObjCol中的实体按照离已知点的近远排序
            if (pObjCol.Count != 0)
            {
                PointClass pPoint = new PointClass();
                pPoint.X = pSearchEnvelope.XMin + pSearchEnvelope.Width / 2;
                pPoint.Y = pSearchEnvelope.YMin + pSearchEnvelope.Height / 2;

                IProximityOperator pProximity;
                pProximity = pPoint as IProximityOperator;
                int nNums = pObjCol.Count;
                double[] pDistSz = new double[nNums];
                for (int i = 0; i < nNums; i++)
                {
                    IFeature pFeat = pObjCol[i] as IFeature;
                    IGeometry geo = pFeat.Shape;
                    double dd = pProximity.ReturnDistance(geo);
                    pDistSz[i] = dd;
                }

                for (int i = 0; i < nNums; i++)
                {
                    for (int j = i + 1; j < nNums; j++)
                    {
                        if (pDistSz[i] > pDistSz[j])
                        {
                            double dDist = pDistSz[i];
                            IFeature pFeature = (IFeature)pObjCol[i];
                            pDistSz[i] = pDistSz[j];
                            pObjCol[i] = pObjCol[j];
                            pDistSz[j] = dDist;
                            pObjCol[j] = pFeature;
                        }
                    }
                } //for(i=0;...

                pNeedFeature = pObjCol[0] as IFeature;
            }
            else pNeedFeature = null;
            //... ...
        }

        public double syc_GetArea(ArrayList PSz)
        {
            //计算点围城的面积:
            double dRetArea = 0.0;
            int nLen = PSz.Count;
            for (int i = 0; i < nLen; i++)
            {
                if (i == 0)
                {
                    IPoint PP = PSz[i] as IPoint;
                    IPoint PP1 = PSz[i + 1] as IPoint;
                    IPoint PP2 = PSz[nLen - 1] as IPoint;
                    dRetArea = dRetArea + 0.5 * PP.X * (PP1.Y - PP2.Y);
                }
                else if (i == nLen - 1)
                {
                    IPoint PP = PSz[i] as IPoint;
                    IPoint PP1 = PSz[0] as IPoint;
                    IPoint PP2 = PSz[i - 1] as IPoint;
                    dRetArea = dRetArea + 0.5 * PP.X * (PP1.Y - PP2.Y);
                }
                else
                {
                    IPoint PP = PSz[i] as IPoint;
                    IPoint PP1 = PSz[i + 1] as IPoint;
                    IPoint PP2 = PSz[i - 1] as IPoint;
                    dRetArea = dRetArea + 0.5 * PP.X * (PP1.Y - PP2.Y);
                }
            }
            return dRetArea;
        }
        #endregion 公共函数

        //... ...

    } //sycCommonFuns Class  end


    public class DLIB
    {
        [System.Runtime.InteropServices.DllImport("SYC.DLL", SetLastError = true, EntryPoint = "HD2DFM")]
        public static extern double HD2DFM(double dHD);
        [System.Runtime.InteropServices.DllImport("SYC.DLL", SetLastError = true, EntryPoint = "DFM2D")]
        public static extern double DFM2D(double dHD);
        [System.Runtime.InteropServices.DllImport("SYC.DLL", SetLastError = true, EntryPoint = "GetTufuInfo")]
        public static extern int GetTufuInfo(double dJ, double dW, double dScale, ref double dJ1, ref double dW1, ref double dJ3, ref double dW3, StringBuilder sRetTFH);
        [System.Runtime.InteropServices.DllImport("SYC.DLL", SetLastError = true, EntryPoint = "TestFunc")]
        public static extern int TestFunc(StringBuilder sP);
        [System.Runtime.InteropServices.DllImport("SYC.DLL", SetLastError = true, EntryPoint = "GetDelDFM")]
        public static extern int GetDelDFM(double dScale, ref double dDJ, ref double dDW);

        [System.Runtime.InteropServices.DllImport("SYC.DLL", SetLastError = true, EntryPoint = "GetNewWaima")]
        public static extern int GetNewWaima(double dScale, double dJ, double dW, StringBuilder sRetTFH);


        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, EntryPoint = "GetMessage")]
        public static extern bool GetMessage(
                    ref MSG msg,	//out Message msg, 
            int hWnd,			// handle to destination window 
            int wParam,		//first message parameter 
            int lParam			// second message parameter 
            );

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "TranslateMessage")]
        public static extern bool TranslateMessage(ref MSG msg);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "DispatchMessage")]
        public static extern int DispatchMessage(ref MSG lpmsg);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            int hWnd,// handle to destination window 
            int Msg, // message
            int wParam,//first message parameter 
            int lParam // second message parameter 
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "DestroyWindow")]
        public static extern int DestroyWindow(IntPtr hwnd);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "PostQuitMessage")]
        public static extern void PostQuitMessage(int nExitCode);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "DefWindowProc")]
        public static extern int DefWindowProc(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int Paras);

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("User32", EntryPoint = "ReleaseDC")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        //... ...
    } //DLIB Class end

    public class SharedFeature
    {
        public SharedFeature() { }
        public IGeometry m_xGeometry;
        public int m_nFID;
        public string m_sClassName;
    }

    public class FLW
    {
        public FLW() { }

        public string sBS;
        public string sZJ1, sZJ2;
        public IPoint PP1, PP2;	//边框内的小短线,PP1-->PP2是由内图框到外图框方向
    }

    public struct POINTAPI
    {
        internal int x;
        internal int y;
    }

    public struct MSG
    {
        internal int hwnd;
        internal int message;
        internal int wParam;
        internal int lParam;
        internal int time;
        internal POINTAPI pt;
    }

    public class MsgLoopClass
    {
        // Constants used within the code
        const int VK_ESCAPE = 0x1B;
        const int WM_QUIT = 0x12;
        const int WM_LBUTTONDOWN = 0x201;


        public bool m_exitLoop = false;
        public int m_nExitCode = -1;
        public int nHwnd = -1;
        public void EnterMesssageLoop()
        {
            this.m_exitLoop = false;
            MSG msg = new MSG();
            while (DLIB.GetMessage(ref msg, nHwnd, 0, 0))
            {
                if (m_exitLoop)
                    break;
                if (msg.wParam == VK_ESCAPE)
                {
                    m_nExitCode = -1;
                    break;
                }
                if (msg.wParam == WM_LBUTTONDOWN)
                {
                    m_nExitCode = 1;
                    break;
                }

                DLIB.TranslateMessage(ref msg);
                DLIB.DispatchMessage(ref msg);
            } //while(...)
        }
        public void ExitMsgLoop()
        {
            this.m_exitLoop = true;
        }
    } //MsgLoopClass end

}
