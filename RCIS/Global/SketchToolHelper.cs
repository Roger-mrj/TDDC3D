using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

using RCIS.GISCommon;

namespace RCIS.Global
{
    /// <summary>
    /// ¼àÌý²ÝÍ¼¹¤¾ß
    /// </summary>
    public class SketchToolHelper
    {

        

        /// <summary>
        /// ÉèÖÃ»æÖÆ ·ûºÅ
        /// </summary>
        /// <param name="m_engineEditor"></param>
        public  static void    SetSketchToolSymbol(IEngineEditor m_engineEditor)
        {

            IEngineEditProperties editProp = m_engineEditor as IEngineEditProperties;
            
            #region ¶¥µã·ûºÅ
            IMarkerSymbol m_defaultPointSymbol = new SimpleMarkerSymbolClass();
            m_defaultPointSymbol = new SimpleMarkerSymbolClass();
            m_defaultPointSymbol.Color = ColorHelper.CreateColor(0, 128, 0);
            (m_defaultPointSymbol as ISimpleMarkerSymbol).Style = esriSimpleMarkerStyle.esriSMSSquare;
            (m_defaultPointSymbol as ISimpleMarkerSymbol).OutlineColor = ColorHelper.CreateColor(255, 255, 0);
            (m_defaultPointSymbol as ISimpleMarkerSymbol).OutlineSize = 1;
            (m_defaultPointSymbol as ISimpleMarkerSymbol).Outline = true;
            m_defaultPointSymbol.Size = 6;

            editProp.SketchVertexSymbol = m_defaultPointSymbol;


            #endregion
            #region ±ß·ûºÅ
            ILineSymbol m_defaultLineSymbol = new SimpleLineSymbolClass();            
            m_defaultLineSymbol.Color = ColorHelper.CreateColor(0, 128, 0);
            m_defaultLineSymbol.Width = 1.5;
            editProp.SketchSymbol = m_defaultLineSymbol;

            #endregion
          
        }
    }
}
