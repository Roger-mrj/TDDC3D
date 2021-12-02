using System;
using ESRI.ArcGIS.Display;
using System.Drawing;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace RCIS.GISCommon
{
    /// <summary>
    /// 创建符号相关的一些函数
    /// </summary>
    public class SymbolHelper
    {
        public static ISymbol GetSymbolFromFile(string ClassName , string styleName)
        {
            string styleFile = System.Environment.CurrentDirectory + @"\style\style.ServerStyle";
            IStyleGallery styleGalley = new ESRI.ArcGIS.Display.ServerStyleGalleryClass();
            IStyleGalleryStorage styleGalleryStorage = styleGalley as IStyleGalleryStorage;
            styleGalleryStorage.AddFile(styleFile);
            IEnumStyleGalleryItem enumStyleGalleryItem = styleGalley.get_Items(ClassName, styleFile, "");
            enumStyleGalleryItem.Reset();
            IStyleGalleryItem styleGalleryItem = enumStyleGalleryItem.Next();
            ISymbol symbol = null;
            while (styleGalleryItem != null)
            {
                if (styleGalleryItem.Name.ToUpper().Trim() == styleName.ToUpper().Trim())
                {
                    symbol = (ISymbol)styleGalleryItem.Item;
                    break;
                }
                styleGalleryItem = enumStyleGalleryItem.Next();
            }
            return symbol;
        }
        private static IGeometry CreateGeometryFromSymbol(ISymbol sym, IEnvelope env)
        {
            if (sym is IMarkerSymbol)
            {
                IArea area = (IArea)env;
                return (IGeometry)area.Centroid;
            }
            else if (sym is ILineSymbol || sym is ITextSymbol)
            {
                IPolyline line = new PolylineClass();
                IPoint pt = new PointClass();
                pt.PutCoords(env.LowerLeft.X, (env.LowerLeft.Y + env.UpperRight.Y) / 2);
                line.FromPoint = pt;
                pt = new PointClass();
                pt.PutCoords(env.UpperRight.X, (env.LowerLeft.Y + env.UpperRight.Y) / 2);
                line.ToPoint = pt;
                if (sym is ITextSymbol)
                {
                    (sym as ITextSymbol).Text = "样本字符";
                }
                return (IGeometry)line;
            }
            else if (sym is IFillSymbol)
            {
                IPolygon polygon = new PolygonClass();
                IPointCollection ptCol = (IPointCollection)polygon;
                IPoint pt = new PointClass();
                pt.PutCoords(env.LowerLeft.X, env.LowerLeft.Y);
                ptCol.AddPoints(1, ref pt);
                pt.PutCoords(env.UpperLeft.X, env.UpperLeft.Y);
                ptCol.AddPoints(1, ref pt);
                pt.PutCoords(env.UpperRight.X, env.UpperRight.Y);
                ptCol.AddPoints(1, ref pt);
                pt.PutCoords(env.LowerRight.X, env.LowerRight.Y);
                ptCol.AddPoints(1, ref pt);
                pt.PutCoords(env.LowerLeft.X, env.LowerLeft.Y);
                ptCol.AddPoints(1, ref pt);
                return (IGeometry)polygon;
            }
            else
            {

                return null;
            }
        }

        private static ITransformation CreateTransformationFromHDC(IntPtr HDC, int width, int height)
        {
            IEnvelope env = new EnvelopeClass();
            env.PutCoords(0, 0, width, height);
            tagRECT frame = new tagRECT();
            frame.left = 0;
            frame.top = 0;
            frame.right = width;
            frame.bottom = height;
            double dpi = Graphics.FromHdc(HDC).DpiY;
            long lDpi = (long)dpi;
            if (lDpi == 0)
            {
                System.Windows.Forms.MessageBox.Show("获取设备比例尺失败!");
                return null;
            }
            IDisplayTransformation dispTrans = new DisplayTransformationClass();
            dispTrans.Bounds = env;
            dispTrans.VisibleBounds = env;
            dispTrans.set_DeviceFrame(ref frame);
            dispTrans.Resolution = dpi;
            return dispTrans;

        }

        /// <summary>
        /// 符号另存为图片
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image StyleToImage(ISymbol sym, int width, int height)
        {
            if (sym == null) return null;
            try
            {
                Image img = new Bitmap(width, height);
                Graphics gc = Graphics.FromImage(img);
                IntPtr hdc = gc.GetHdc();
                IEnvelope env = new EnvelopeClass();
                env.XMin = 1;
                env.XMax = width - 1;
                env.YMin = 1;
                env.YMax = height - 1;
                IGeometry geo = CreateGeometryFromSymbol(sym, env);
                if (geo != null)
                {
                    ITransformation trans = CreateTransformationFromHDC(hdc, width, height);
                    sym.SetupDC((int)hdc, trans);
                    sym.Draw(geo);
                    sym.ResetDC();
                }
                gc.ReleaseHdc(hdc);
                return img;
            }
            catch { return null; }
        }
        public static Image StyleToImage(ISymbol sym)
        {
            return StyleToImage(sym, 16, 16);
        }

        /// <summary>
        /// 创建简单线符号
        /// </summary>
        /// <param name="lineColor"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static ILineSymbol CreateSimpleLineSymbol(Color lineColor, double width)
        {
            SimpleLineSymbolClass rSimp = new SimpleLineSymbolClass();
            rSimp.Color = ColorHelper.CreateColor(lineColor);
            rSimp.Width = Math.Abs(width);
            return rSimp;
        }

        /// <summary>
        /// 创建 透明面符号
        /// </summary>
        /// <param name="outlineColor"></param>
        /// <returns></returns>
        public static IFillSymbol CreateTransparentFillSymbol(Color outlineColor)
        {
            SimpleFillSymbolClass aSimpSym = new SimpleFillSymbolClass();
            aSimpSym.Style = esriSimpleFillStyle.esriSFSHollow;
            ISimpleLineSymbol aOutline = new SimpleLineSymbolClass();
            aOutline.Style = esriSimpleLineStyle.esriSLSSolid;
            aOutline.Color = ColorHelper.CreateColor(outlineColor);
            aOutline.Width = 1.5;
            aSimpSym.Outline = aOutline;
            return aSimpSym as IFillSymbol;
        }
        /// <summary>
        /// 创建简单面符号
        /// </summary>
        /// <param name="fillColor"></param>
        /// <param name="outlineColor"></param>
        /// <returns></returns>
        public static IFillSymbol CreateFillSymbol(Color fillColor, Color outlineColor)
        {
            SimpleFillSymbolClass aSimpSym = new SimpleFillSymbolClass();
            aSimpSym.Style = esriSimpleFillStyle.esriSFSSolid;
            aSimpSym.Color = ColorHelper.CreateColor(fillColor);
            ISimpleLineSymbol aOutline = new SimpleLineSymbolClass();
            aOutline.Style = esriSimpleLineStyle.esriSLSSolid;
            aOutline.Color = ColorHelper.CreateColor(outlineColor);
            aOutline.Width = 1.0;
            aSimpSym.Outline = aOutline;
            return aSimpSym as IFillSymbol;
        }



        /// <summary>
        /// 创建显示线条方向的符号
        /// </summary>
        /// <returns></returns>
        public static IMarkerSymbol CreateSimpleMarkerSymbol(Color pColor, float pSize)
        {
            SimpleMarkerSymbolClass ms = new SimpleMarkerSymbolClass();
            ms.Color = ColorHelper.CreateColor(pColor);
            ms.Size = pSize;
            return ms as IMarkerSymbol;
        }

        public static ILineSymbol CreateLineDirectionSymbol()
        {

            ILineSymbol lineDirSym = new CartographicLineSymbolClass();
            lineDirSym.Color = ColorHelper.CreateColor(0, 0, 200);
            LineDecorationClass lineDeco = new LineDecorationClass();
            SimpleLineDecorationElementClass lineDecoEle = new SimpleLineDecorationElementClass();
            lineDecoEle.AddPosition(0.3);
            lineDecoEle.AddPosition(0.7);
            lineDecoEle.PositionAsRatio = true;
            IMarkerSymbol markerSym = (lineDecoEle.MarkerSymbol as IClone).Clone() as IMarkerSymbol;
            markerSym.Size = 9;
            markerSym.Color = ColorHelper.CreateColor(0, 200, 0);
            lineDecoEle.MarkerSymbol = markerSym;
            lineDeco.AddElement(lineDecoEle as ILineDecorationElement);

            (lineDirSym as ILineProperties).LineDecoration = lineDeco;
            return lineDirSym;
        }

        public static ISymbol CreateSymbolFromColor(string paramStyleClass, IColor paramColor)
        {
            ISymbol resultSym = null;
            if (paramStyleClass.Equals("Marker Symbols"))
            {
                SimpleMarkerSymbolClass simp = new SimpleMarkerSymbolClass();
                simp.Color = paramColor;
                simp.Style = esriSimpleMarkerStyle.esriSMSSquare;
                simp.Size = 1;
                resultSym = simp as ISymbol;
            }
            else if (paramStyleClass.Equals("Line Symbols"))
            {
                SimpleLineSymbolClass simp = new SimpleLineSymbolClass();
                simp.Color = paramColor;
                simp.Width = 1.0;
                resultSym = simp as ISymbol;
            }
            else if (paramStyleClass.Equals("Fill Symbols"))
            {
                SimpleLineSymbolClass outline = new SimpleLineSymbolClass();
                outline.Color = ColorHelper.CreateColor(Color.Black);
                outline.Width = 1.0;
                SimpleFillSymbolClass simp = new SimpleFillSymbolClass();
                simp.Color = paramColor;
                simp.Style = esriSimpleFillStyle.esriSFSSolid;
                simp.Outline = outline;
                resultSym = simp as ISymbol;
            }
            return resultSym;
        }

        public static ISymbol CreateTmpSym(esriGeometryType type)
        {
            ISymbol sym = null;
            switch (type)
            {
                case esriGeometryType.esriGeometryPoint:
                    sym = new SimpleMarkerSymbolClass();
                    (sym as ISimpleMarkerSymbol).Size = 3;
                    (sym as ISimpleMarkerSymbol).Style = esriSimpleMarkerStyle.esriSMSDiamond;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    sym = new SimpleLineSymbolClass();
                    (sym as ISimpleLineSymbol).Width = 1;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    ICartographicLineSymbol pCartoLineSymbol = new CartographicLineSymbolClass();
                    pCartoLineSymbol.Width = 0.1;
                    pCartoLineSymbol.Color = ColorHelper.CreateColor(0, 0, 0);

                    sym = new SimpleFillSymbolClass();
                    (sym as ISimpleFillSymbol).Style = esriSimpleFillStyle.esriSFSSolid;
                    (sym as ISimpleFillSymbol).Outline = pCartoLineSymbol;
                    break;

            }
            return sym;
        }

    }
}
