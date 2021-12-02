using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;

namespace RCIS.Style.StyleEditor
{
    public partial class SimpleLineStyleEditor : UserControl,IStyleEditor
    {
        public SimpleLineStyleEditor()
        {
            InitializeComponent();
            this.ceStyle.Properties.Items.Add(
             new ComboBoxItem(esriSimpleLineStyle.esriSLSDash, "Dash"));
            this.ceStyle.Properties.Items.Add(
                            new ComboBoxItem(esriSimpleLineStyle.esriSLSDashDot, "DashDot"));
            this.ceStyle.Properties.Items.Add(
                            new ComboBoxItem(esriSimpleLineStyle.esriSLSDashDotDot, "DotDot"));
            this.ceStyle.Properties.Items.Add(
                            new ComboBoxItem(esriSimpleLineStyle.esriSLSDot, "Dot"));
            this.ceStyle.Properties.Items.Add(
                new ComboBoxItem(esriSimpleLineStyle.esriSLSInsideFrame, "InsideFrame"));
            this.ceStyle.Properties.Items.Add(
              new ComboBoxItem(esriSimpleLineStyle.esriSLSNull, "Null"));
            this.ceStyle.Properties.Items.Add(
              new ComboBoxItem(esriSimpleLineStyle.esriSLSSolid, "Solid"));
        }
        #region IStyleEditor 成员

        public string StyleClass
        {
            get
            {
                return RCIS.StyleClass.StyleClassLine;
            }
        }

        public string EditorName
        {
            get
            {
                return "Simple Line Style  | 简单线样式";
            }
        }
        public void CreateInitializeStyle()
        {
            ISimpleLineSymbol aCharSym = new SimpleLineSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Style = esriSimpleLineStyle.esriSLSSolid;
            aCharSym.Width = 1;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public ISimpleLineSymbol LineSymbol
        {
            get
            {
                return this.m_pEditedStyle as ISimpleLineSymbol;
            }
            
        }
        public ISymbol EditedStyle
        {
            get
            {
                return this.m_pEditedStyle;
            }
            set
            {
                if (value == null)
                {
                    this.CreateInitializeStyle();
                }
                else if (this.CanEdit(value))
                {
                    this.m_pEditedStyle = value;
                    this.DispatchStyle();
                }
            }
        }

        public bool CanEdit(ISymbol pSymbol)
        {
            if (pSymbol is ISimpleLineSymbol)
            {
                return true;
            }
            //else if (pSymbol is ILineSymbol) { return true; }
            //else if (pSymbol is IHashLineSymbol) { return true; }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.LineSymbol != null)
            {
                this.m_shouldAction = false;
                try
                {
                    Color aColor = ColorHelper.CreateColor(this.LineSymbol.Color);
                    this.ceColor.Color = aColor;
                    esriSimpleLineStyle aLineStyle = this.LineSymbol.Style;
                    foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                    {
                        if (aItem.ItemObject.Equals(aLineStyle))
                        {
                            this.ceStyle.SelectedItem = aItem;
                        }
                    }
                    this.spinSize.Value= (decimal)this.LineSymbol.Width;
                }
                catch (Exception ex) { }
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is ISimpleLineSymbol)
                {
                    try
                    {
                        Color aColor = ColorHelper.CreateColor((this.m_pEditedStyle as ISimpleLineSymbol).Color);
                        this.ceColor.Color = aColor;
                        esriSimpleLineStyle aLineStyle = (this.m_pEditedStyle as ISimpleLineSymbol).Style;
                        foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                        {
                            if (aItem.ItemObject.Equals(aLineStyle))
                            {
                                this.ceStyle.SelectedItem = aItem;
                            }
                        }
                        this.spinSize.Value = (decimal)(this.m_pEditedStyle as ISimpleLineSymbol).Width;
                    }
                    catch (Exception ex) { }
                }
            }
            this.m_shouldAction = true;
        }
        private void OnEditedStylePropertyChanged(object pSender, EventArgs pArg)
        {
            if (!this.m_shouldAction || this.EditedStyle == null)
            {
                DispatchStyle();
                return;
            }
            if (this.LineSymbol != null)
            {
                try
                {
                    this.LineSymbol.Width = (double)this.spinSize.Value;

                    this.LineSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);

                    try
                    {
                        this.LineSymbol.Style = (esriSimpleLineStyle)
                           (this.ceStyle.SelectedItem as ComboBoxItem).ItemObject;
                    }
                    catch (Exception ex) { }
                }
                catch (Exception ex) { }
               
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is ISimpleLineSymbol)
                    {
                        (this.m_pEditedStyle as ISimpleLineSymbol).Width = (double)this.spinSize.Value;

                        (this.m_pEditedStyle as ISimpleLineSymbol).Color = ColorHelper.CreateColor(this.ceColor.Color);

                        try
                        {
                            (this.m_pEditedStyle as ISimpleLineSymbol).Style = (esriSimpleLineStyle)
                               (this.ceStyle.SelectedItem as ComboBoxItem).ItemObject;
                        }
                        catch (Exception ex) { }
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
     
        #endregion

     
        
       

      
    }
}
