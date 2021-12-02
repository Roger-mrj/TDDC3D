using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing ;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;


using RCIS.Style;
namespace RCIS
{
    /// <summary>
    /// SymbolHelper 的摘要说明。
    /// </summary>
    public class SymbolHelper
    {
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
                outline.Color = ColorHelper.CreateColor (Color.Black );
                outline.Width = 1.0;
                SimpleFillSymbolClass simp = new SimpleFillSymbolClass();
                simp.Color = paramColor;
                simp.Style = esriSimpleFillStyle.esriSFSSolid;
                simp.Outline = outline;
                resultSym = simp as ISymbol;
            }
            return resultSym;
        }

        public static int GetSymbolCode(ISymbol pSymbol)
        {
            if (pSymbol == null) return -1;
            if (pSymbol is ArrowMarkerSymbolClass) return SymbolCode.SCArrowMarkerSymbol;
            else if (pSymbol is BarChartSymbolClass) return SymbolCode.SCBarChartSymbol;
            else if (pSymbol is CartographicLineSymbolClass) return SymbolCode.SCCartographicLineSymbol;
            //else if(pSymbol is CharacterMarker3DSymbolClass)return 11;            
            else if (pSymbol is CharacterMarkerSymbolClass) return SymbolCode.SCCharacterMarkerSymbol;
            else if (pSymbol is ColorRampSymbolClass) return SymbolCode.SCColorRampSymbol;
            else if (pSymbol is ColorSymbolClass) return SymbolCode.SCColorSymbol;
            else if (pSymbol is DotDensityFillSymbolClass) return SymbolCode.SCDotDensityFillSymbol;
            else if (pSymbol is GradientFillSymbolClass) return SymbolCode.SCGradientFillSymbol;
            else if (pSymbol is HashLineSymbolClass) return SymbolCode.SCHashLineSymbol;
            else if (pSymbol is LineFillSymbolClass) return SymbolCode.SCLineFillSymbol;
            //else if(pSymbol is Marker3DSymbolClass)return SymbolCode.SCMarker3DSymbol;
            else if (pSymbol is MarkerFillSymbolClass) return SymbolCode.SCMarkerFillSymbol;
            else if (pSymbol is MarkerLineSymbolClass) return SymbolCode.SCMarkerLineSymbol;
            else if (pSymbol is MultiLayerFillSymbolClass) return SymbolCode.SCMultiLayerFillSymbol;
            else if (pSymbol is MultiLayerLineSymbolClass) return SymbolCode.SCMultiLayerLineSymbol;
            else if (pSymbol is MultiLayerMarkerSymbolClass) return SymbolCode.SCMultiLayerMarkerSymbol;
            else if (pSymbol is PictureFillSymbolClass) return SymbolCode.SCPictureFillSymbol;
            else if (pSymbol is PictureLineSymbolClass) return SymbolCode.SCPictureLineSymbol;
            else if (pSymbol is PictureMarkerSymbolClass) return SymbolCode.SCPictureMarkerSymbol;
            else if (pSymbol is PieChartSymbolClass) return SymbolCode.SCPieChartSymbol;
            //else if(pSymbol is RasterRGBSymbolClass )return SymbolCode.SCRasterRGBSymbol;
            else if (pSymbol is SimpleFillSymbolClass) return SymbolCode.SCSimpleFillSymbol;
            //else if(pSymbol is SimpleLine3DSymbolClass )return SymbolCode.SCSimpleLine3DSymbol;
            else if (pSymbol is SimpleLineSymbolClass) return SymbolCode.SCSimpleLineSymbol;
            //else if(pSymbol is SimpleMarker3DSymbolClass) return SymbolCode.SCSimpleMarker3DSymbol;
            else if (pSymbol is SimpleMarkerSymbolClass) return SymbolCode.SCSimpleMarkerSymbol;
            else if (pSymbol is StackedChartSymbolClass) return SymbolCode.SCStackedChartSymbol;
            else if (pSymbol is TextSymbolClass) return SymbolCode.SCTextSymbol;
            //else if(pSymbol is TextureFillSymbolClass)return SymbolCode.SCTextureFillSymbol;
            // else if(pSymbol is TextureLineSymbolClass)return SymbolCode.SCTextureLineSymbol;
            else return SymbolCode.SCUnkown;
        }
        public static ISymbol CreateSymbolFromCode(int pSymbolCode)
        {
            if (SymbolCode.SCUnkown == pSymbolCode) return null;
            else if (SymbolCode.SCArrowMarkerSymbol == pSymbolCode) return new ArrowMarkerSymbolClass() as ISymbol;
            else if (SymbolCode.SCBarChartSymbol == pSymbolCode) return new BarChartSymbolClass() as ISymbol;
            else if (SymbolCode.SCCartographicLineSymbol == pSymbolCode) return new CartographicLineSymbolClass() as ISymbol;
            //else if(SymbolCode.SCCharacterMarker3DSymbol==pSymbolCode)return new CharacterMarker3DSymbolClass() as ISymbol;
            else if (SymbolCode.SCCharacterMarkerSymbol == pSymbolCode) return new CharacterMarkerSymbolClass() as ISymbol;
            else if (SymbolCode.SCColorRampSymbol == pSymbolCode) return new ColorRampSymbolClass() as ISymbol;
            else if (SymbolCode.SCColorSymbol == pSymbolCode) return new ColorSymbolClass() as ISymbol;
            else if (SymbolCode.SCDotDensityFillSymbol == pSymbolCode) return new DotDensityFillSymbolClass() as ISymbol;
            else if (SymbolCode.SCGradientFillSymbol == pSymbolCode) return new GradientFillSymbolClass() as ISymbol;
            else if (SymbolCode.SCHashLineSymbol == pSymbolCode) return new HashLineSymbolClass() as ISymbol;
            else if (SymbolCode.SCLineFillSymbol == pSymbolCode) return new LineFillSymbolClass() as ISymbol;
            //else if(SymbolCode.SCMarker3DSymbol==pSymbolCode)return new Marker3DSymbolClass() as ISymbol;
            else if (SymbolCode.SCMarkerFillSymbol == pSymbolCode) return new MarkerFillSymbolClass() as ISymbol;
            else if (SymbolCode.SCMarkerLineSymbol == pSymbolCode) return new MarkerLineSymbolClass() as ISymbol;
            else if (SymbolCode.SCMultiLayerFillSymbol == pSymbolCode) return new MultiLayerFillSymbolClass() as ISymbol;
            else if (SymbolCode.SCMultiLayerLineSymbol == pSymbolCode) return new MultiLayerLineSymbolClass() as ISymbol;
            else if (SymbolCode.SCMultiLayerMarkerSymbol == pSymbolCode) return new MultiLayerMarkerSymbolClass() as ISymbol;
            else if (SymbolCode.SCPictureFillSymbol == pSymbolCode) return new PictureFillSymbolClass() as ISymbol;
            else if (SymbolCode.SCPictureLineSymbol == pSymbolCode) return new PictureLineSymbolClass() as ISymbol;
            else if (SymbolCode.SCPictureMarkerSymbol == pSymbolCode) return new PictureMarkerSymbolClass() as ISymbol;
            else if (SymbolCode.SCPieChartSymbol == pSymbolCode) return new PieChartSymbolClass() as ISymbol;
            //else if(SymbolCode.SCRasterRGBSymbol==pSymbolCode)return  RasterRGBSymbolClass() as ISymbol;
            else if (SymbolCode.SCSimpleFillSymbol == pSymbolCode) return new SimpleFillSymbolClass() as ISymbol;
            //else if(SymbolCode.SCSimpleLine3DSymbol==pSymbolCode)return new SimpleLine3DSymbolClass() as ISymbol;
            else if (SymbolCode.SCSimpleLineSymbol == pSymbolCode) return new SimpleLineSymbolClass() as ISymbol;
            //else if(SymbolCode.SCSimpleMarker3DSymbol==pSymbolCode)return new SimpleMarker3DSymbol() as ISymbol;
            else if (SymbolCode.SCSimpleMarkerSymbol == pSymbolCode) return new SimpleMarkerSymbolClass() as ISymbol;
            else if (SymbolCode.SCStackedChartSymbol == pSymbolCode) return new StackedChartSymbolClass() as ISymbol;
            else if (SymbolCode.SCTextSymbol == pSymbolCode) return new TextSymbolClass() as ISymbol;
            //else if(SymbolCode.SCTextureFillSymbol ==pSymbolCode)return new TextureFillSymbolClass() as ISymbol;
            //else if(SymbolCode.SCTextureLineSymbol ==pSymbolCode)return new TextureLineSymbolClass() as ISymbol;
            else return null;
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


     
        /// <summary>
        /// This Method Create a fill symbol which has an outline only.
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
            aOutline.Width = 1.0;
            aSimpSym.Outline = aOutline;
            return aSimpSym as IFillSymbol;
        }
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
        public static ILineSymbol CreateSimpleLineSymbol(Color lineColor, double width)
        {
            SimpleLineSymbolClass rSimp = new SimpleLineSymbolClass();
            rSimp.Color = ColorHelper.CreateColor(lineColor);
            rSimp.Width = Math.Abs (width);
            return rSimp;
        }
     

    }
}
