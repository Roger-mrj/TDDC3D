using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
namespace RCIS.Style.StyleEditor
{
    public class StyleLayer:ICloneable 
    {
        
        public ISymbol Style;
        public bool Visible = true;
        public bool Lock = false;

        #region ICloneable 成员

        public object Clone()
        {
            StyleLayer aCloneObj = new StyleLayer();
            aCloneObj.Visible = this.Visible;
            aCloneObj.Lock = this.Lock;
            if (this.Style != null)
            {
                aCloneObj.Style = (this.Style as IClone).Clone()
                as ISymbol;
            }
            return aCloneObj;
        }

        #endregion
    }
}
