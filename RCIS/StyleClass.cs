using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
namespace RCIS.GISCommon
{
    public class StyleClass
    {

        public const string StyleClassAny = "";
        public const string StyleClassFill = "Fill Symbols";
        public const string StyleClassMarker = "Marker Symbols";
        public const string StyleClassLine = "Line Symbols";
        public const string StyleClassText = "Text Symbols";
        public static string GetStyleClass(ISymbol pSymbol)
        {
            if (pSymbol is IMarkerSymbol)
            {
                return StyleClassMarker;
            }
            else if (pSymbol is ILineSymbol)
            {
                return StyleClassLine;
            }
            else if (pSymbol is IFillSymbol)
            {
                return StyleClassFill;
            }
            else if (pSymbol is ITextSymbol)
            {
                return StyleClassText;
            }
            return StyleClassAny;
        }
        public static ISymbol CreateDefaultStyle(string pStyleClass)
        {
            if (StyleClassMarker.Equals(pStyleClass))
            {
                return new SimpleMarkerSymbolClass() as ISymbol;
            }
            else if (StyleClassLine.Equals(pStyleClass))
            {
                return new SimpleLineSymbolClass() as ISymbol;
            }
            else if (StyleClassFill.Equals(pStyleClass))
            {
                SimpleFillSymbolClass simpFill = new SimpleFillSymbolClass();
                simpFill.Style = esriSimpleFillStyle.esriSFSHollow;
                SimpleLineSymbolClass aline = new SimpleLineSymbolClass();
                aline.Color = RCIS.GISCommon.ColorHelper.CreateColor(System.Drawing.Color.Black);
                simpFill.Outline = aline;
                return simpFill as ISymbol;
            }
            else if (StyleClassText.Equals(pStyleClass))
            {
                return new TextSymbolClass() as ISymbol;
            }
            return null;
        }
        public static string StyleClassOf(IGeometry pGeom)
        {
            if (pGeom is IPoint) return StyleClassMarker;
            if (pGeom is IPolyline) return StyleClassLine;
            if (pGeom is IPolygon) return StyleClassFill;
            return StyleClassAny;
        }
        public static string StyleClassOf(esriGeometryType pType)
        {
            if (esriGeometryType.esriGeometryPoint == pType)
                return StyleClassMarker;
            if (esriGeometryType.esriGeometryPolyline == pType)
                return StyleClassLine;
            if (esriGeometryType.esriGeometryPolygon == pType)
                return StyleClassFill;
            return StyleClassAny;
        }
    }
}
