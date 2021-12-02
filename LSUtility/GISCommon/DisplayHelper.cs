using System;

using stdole;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
namespace RCIS.GISCommon
{
    public class DisplayHelper
    {
        public static void DrawGeometry(IScreenDisplay paramScreenDisplay, IGeometry paramGeom, ISymbol paramSymbol)
        {
            if (paramScreenDisplay == null
                || paramGeom == null
                || paramGeom.IsEmpty
                || paramSymbol == null)
            {
                return;
            }
            else
            {
                paramScreenDisplay.StartDrawing(paramScreenDisplay.hDC, (short)esriScreenCache.esriAllScreenCaches);
                paramScreenDisplay.SetSymbol(paramSymbol);
                if (paramGeom is IPoint)
                {
                    paramScreenDisplay.DrawPoint(paramGeom);
                }
                else if (paramGeom is IPolyline)
                {
                    paramScreenDisplay.DrawPolyline(paramGeom);
                }
                else if (paramGeom is IPolygon)
                {
                    paramScreenDisplay.DrawPolygon(paramGeom);
                }
                paramScreenDisplay.FinishDrawing();
            }
        }
        public static void DrawText(IScreenDisplay paramScreenDisplay, IGeometry paramGeom, string paramText, ISymbol paramSymbol)
        {
            if (paramScreenDisplay == null
                || paramGeom == null
                || paramGeom.IsEmpty
                || paramSymbol == null
                || !(paramSymbol is ITextSymbol))
            {
                return;
            }
            else
            {
                paramScreenDisplay.StartDrawing(paramScreenDisplay.hDC, (short)esriScreenCache.esriAllScreenCaches);
                paramScreenDisplay.UpdateWindow();
                paramScreenDisplay.SetSymbol(paramSymbol);
                if (paramText == null) paramText = "";
                paramScreenDisplay.DrawText(paramGeom, paramText);
                paramScreenDisplay.FinishDrawing();
                //paramScreenDisplay.UpdateWindow ();
            }
        }
        public static double FromPixesToRealWorld(IActiveView pView, int pixes)
        {
            if (pView == null) return pixes;
            int aWidth = pView.ExportFrame.right - pView.ExportFrame.left;
            double aWidthReal = pView.Extent.Width;
            double result = pixes * aWidthReal / aWidth;
            return result;
        }
        public static IFontDisp CreateFont(string pFontName, float pSize
            , bool pBold, bool pItalic, bool pUnderline, bool pStroke)
        {
            stdole.StdFontClass stdFont = new StdFontClass();
            stdFont.Name = pFontName;
            stdFont.Size = Convert.ToDecimal(pSize);
            stdFont.Bold = pBold;
            stdFont.Italic = pItalic;
            stdFont.Underline = pUnderline;
            stdFont.Strikethrough = pStroke;
            IFontDisp fDisp = stdFont as IFontDisp;
            return fDisp;
        }
    }
}
