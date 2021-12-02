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
    public partial class CartographicLineStyleEditor : UserControl,IStyleEditor
    {
        public CartographicLineStyleEditor()
        {
            InitializeComponent();
                    this.cbLineCapStyle.Properties.Items.Add(
            new ComboBoxItem(esriLineCapStyle.esriLCSButt, "Butt"));
                    this.cbLineCapStyle.Properties.Items.Add(
               new ComboBoxItem(esriLineCapStyle.esriLCSRound, "Round"));
                    this.cbLineCapStyle.Properties.Items.Add(
               new ComboBoxItem(esriLineCapStyle.esriLCSSquare, "Square"));

                    this.ceLineJoinStyle.Properties.Items.Add(
            new ComboBoxItem(esriLineJoinStyle.esriLJSBevel, "Bevel"));
                    this.ceLineJoinStyle.Properties.Items.Add(
          new ComboBoxItem(esriLineJoinStyle.esriLJSMitre, "Mitre"));
                    this.ceLineJoinStyle.Properties.Items.Add(
          new ComboBoxItem(esriLineJoinStyle.esriLJSRound, "Round"));
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
                return "Cartographic Line Style  | 制图线样式";
            }
        }
        public void CreateInitializeStyle()
        {
            ICartographicLineSymbol aCharSym = new CartographicLineSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Cap = esriLineCapStyle.esriLCSButt;
            aCharSym.Join = esriLineJoinStyle.esriLJSBevel;
            aCharSym.Width = 1;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public ICartographicLineSymbol CartographicLineSymbol
        {
            get
            {
                return this.m_pEditedStyle as ICartographicLineSymbol;
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
            if (pSymbol is ICartographicLineSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.CartographicLineSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.CartographicLineSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is ICartographicLineSymbol)
                {
                    Display((this.m_pEditedStyle as ICartographicLineSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(ICartographicLineSymbol pCartoLineSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pCartoLineSymbol.Color);
                this.ceColor.Color = aColor;
                esriLineCapStyle aLineCapStyle = pCartoLineSymbol.Cap;
                foreach (ComboBoxItem aItem in this.cbLineCapStyle.Properties.Items)
                {
                    if (aItem.ItemObject.Equals(aLineCapStyle))
                    {
                        this.cbLineCapStyle.SelectedItem = aItem;
                    }
                }
                esriLineJoinStyle aLineJoinStyle = pCartoLineSymbol.Join;
                foreach (ComboBoxItem aItem in this.ceLineJoinStyle.Properties.Items)
                {
                    if (aItem.ItemObject.Equals(aLineJoinStyle))
                    {
                        this.ceLineJoinStyle.SelectedItem = aItem;
                    }
                }
                this.spinSize.Value = (decimal)pCartoLineSymbol.Width;
                this.txtFillLineMiter.Text = pCartoLineSymbol.MiterLimit.ToString();
            }
            catch (Exception ex) { }
        }
        private void OnEditedStylePropertyChanged(object pSender, EventArgs pArg)
        {
            if (!this.m_shouldAction || this.EditedStyle == null)
            {
                DispatchStyle();
                return;
            }
            if (this.CartographicLineSymbol != null)
            {

                Symbolization(this.CartographicLineSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is ICartographicLineSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as ICartographicLineSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private void Symbolization(ICartographicLineSymbol pCartoLineSymbol)
        {
            try
            {
                pCartoLineSymbol.Width = (double)this.spinSize.Value;

                pCartoLineSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);

                try
                {
                    pCartoLineSymbol.Cap = (esriLineCapStyle)
                       (this.cbLineCapStyle.SelectedItem as ComboBoxItem).ItemObject;
                }
                catch (Exception ex) { }
                try
                {
                    pCartoLineSymbol.Join = (esriLineJoinStyle)
                       (this.ceLineJoinStyle.SelectedItem as ComboBoxItem).ItemObject;
                }
                catch (Exception ex) { }
                pCartoLineSymbol.MiterLimit = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtFillLineMiter.Text.Trim());
            }
            catch (Exception ex) { }
        }
        private bool m_shouldAction = true;

        #endregion
      
    }
}
