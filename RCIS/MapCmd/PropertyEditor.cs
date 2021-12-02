using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace RCIS.Controls
{
    public interface PropertyEditor
    {
        bool CanEdit { get;set;}
        IFeature  DJFeature { get;set;}
        void OnApply();
        void OnCancel();
        bool PropertyChanged { get;set;}
    }
}
