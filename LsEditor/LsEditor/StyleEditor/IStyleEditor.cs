using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS .Display ;
namespace RCIS.Style.StyleEditor
{
    /// <summary>
    /// This interface is used to be an editor of 
    /// a simple style.it can not used to edit a 
    /// multilayer style.
    /// if you want to edit a multilayer style,you should 
    /// use IStyleEditor and StyleLayerEditor together.
    /// </summary>
    public interface IStyleEditor
    {
        string StyleClass { get;}
        string EditorName { get;}
        void CreateInitializeStyle();
        ISymbol EditedStyle { get;set;}
        bool CanEdit(ISymbol pSymbol);        
        //after the IStyleEditor changes the EditedStyle. 
        //OnStyleChanged event will triggered.
        event EditedStyleChangedEventHandler OnEditedStyleChanged;
    }    
}
