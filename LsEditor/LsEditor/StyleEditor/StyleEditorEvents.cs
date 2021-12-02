using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS .Display ;
namespace RCIS.Style.StyleEditor
{    
    public delegate void EditedStyleChangedEventHandler(object pSender
    , EventArgs pArg);
    public delegate void EditedStyleLayerChangedEventHandler(object pSender
    ,EventArgs pArg);
}
