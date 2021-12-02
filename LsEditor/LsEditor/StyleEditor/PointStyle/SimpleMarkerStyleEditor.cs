using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
namespace RCIS.Style.StyleEditor
{
    public partial class SimpleMarkerStyleEditor : UserControl,IStyleEditor
    {
        public SimpleMarkerStyleEditor()
        {
            InitializeComponent();
            this.ceStyle.Properties.Items.Add(
                new ComboBoxItem(esriSimpleMarkerStyle.esriSMSCircle, "Circle"));
            this.ceStyle.Properties.Items.Add(
                            new ComboBoxItem(esriSimpleMarkerStyle.esriSMSCross, "Cross"));
            this.ceStyle.Properties.Items.Add(
                            new ComboBoxItem(esriSimpleMarkerStyle.esriSMSDiamond, "Diamond"));
            this.ceStyle.Properties.Items.Add(
                            new ComboBoxItem(esriSimpleMarkerStyle.esriSMSSquare, "Square"));
            this.ceStyle.Properties.Items.Add(
                new ComboBoxItem(esriSimpleMarkerStyle.esriSMSX, "SX"));


        }
       
        #region IStyleEditor 成员
        private bool m_shouldAction = true;
        private ISymbol m_originStyle;
        
        public string StyleClass
        {
            get
            {
                return RCIS.StyleClass.StyleClassMarker; 
            }
        }
        public string EditorName
        {
            get
            {
                return "Simple Marker Style  | 简单点样式";
            }
            
        }
        public bool CanEdit(ISymbol pSymbol)
        {
            if (pSymbol is ISimpleMarkerSymbol
               && !(pSymbol is IMultiLayerMarkerSymbol))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ISymbol EditedStyle
        {
            get
            {
                return this.m_originStyle ;
            }
            set
            {                
                if (value == null)
                {
                    this.CreateInitializeStyle();

                }else if (this.CanEdit(value))
                {
                    this.m_originStyle = value;                    
                    this.DispatchStyle();
                }                
            }
        }
        private ISimpleMarkerSymbol MarkerSymbol
        {
            get
            {
                return this.m_originStyle as ISimpleMarkerSymbol;
            }
        }
        private void DispatchStyle()
        {
            if (this.MarkerSymbol != null)
            {
                this.m_shouldAction = false;
                try
                {
                    this.ceColor.Color = ColorHelper.CreateColor(this.MarkerSymbol.Color);
                    this.spinSize.Value = (decimal)this.MarkerSymbol.Size;
                    this.spinOffsetX.Value = (decimal)this.MarkerSymbol.XOffset;
                    this.spinOffsetY.Value = (decimal)this.MarkerSymbol.YOffset;
                    esriSimpleMarkerStyle aMarkerStyle = this.MarkerSymbol.Style;
                    foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                    {
                        if (aItem.ItemObject.Equals(aMarkerStyle))
                        {
                            this.ceStyle.SelectedItem = aItem;
                        }
                    }
                    this.ceOutline.Checked = this.MarkerSymbol.Outline;
                    try
                    {
                        this.ceOutlineColor.Color = ColorHelper.CreateColor(this.MarkerSymbol.OutlineColor);
                        this.spinOutlineSize.Value = (decimal)this.MarkerSymbol.OutlineSize;
                    }
                    catch (Exception ex) { }
                }
                catch (Exception ex) { }
                this.m_shouldAction = true;
            }
        }
        public void CreateInitializeStyle()
        {
            this.EditedStyle =new SimpleMarkerSymbolClass() as ISymbol;
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());            
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        

        #endregion
        private void OnMarkerStylePropertyChanged(object sender, EventArgs e)
        {
            if (this.m_shouldAction && this.EditedStyle != null)
            {
                try
                {
                    this.MarkerSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                    try
                    {
                        this.MarkerSymbol.Style = (esriSimpleMarkerStyle)
                           (this.ceStyle.SelectedItem as ComboBoxItem).ItemObject;
                    }
                    catch (Exception ex) { }
                    this.MarkerSymbol.Size = (double)this.spinSize.Value;
                    this.MarkerSymbol.XOffset = (double)this.spinOffsetX.Value;
                    this.MarkerSymbol.YOffset = (double)this.spinOffsetY.Value;
                    //
                    this.spinOutlineSize.Enabled = this.ceOutline.Checked;
                    this.ceOutlineColor.Enabled = this.ceOutline.Checked;
                    if (this.ceOutline.Checked)
                    {
                        this.MarkerSymbol.Outline = true;
                        this.MarkerSymbol.OutlineColor = ColorHelper.CreateColor(
                            this.ceOutlineColor.Color);
                        this.MarkerSymbol.OutlineSize = (double)this.spinOutlineSize.Value;
                    }
                    else
                    {
                        this.MarkerSymbol.Outline = false;
                    }
                    if (this.OnEditedStyleChanged != null)
                        this.OnEditedStyleChanged(this, new EventArgs());
                }
                catch (Exception ex) { }
            }
        }
        
       
    }
}
