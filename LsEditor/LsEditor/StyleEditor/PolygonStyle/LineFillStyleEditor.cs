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
    public partial class LineFillStyleEditor : UserControl,IStyleEditor
    {
        public LineFillStyleEditor()
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
                return RCIS.StyleClass.StyleClassFill;
            }
        }

        public string EditorName
        {
            get
            {
                return "Line Fill Style  | 线填充样式";
            }
        }
        public void CreateInitializeStyle()
        {

            ILineFillSymbol aCharSym = new LineFillSymbolClass();
            aCharSym.Angle = 0;
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Outline.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Outline.Width = 1;
            ICartographicLineSymbol pLineSymbol = new CartographicLineSymbolClass();
            pLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            pLineSymbol.Color = ColorHelper.CreateColor(255, 0, 0);
            pLineSymbol.Join = esriLineJoinStyle.esriLJSBevel;
            pLineSymbol.MiterLimit = 1;
            pLineSymbol.Width = 1;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public ILineFillSymbol LineFillSymbol
        {
            get
            {
                return this.m_pEditedStyle as ILineFillSymbol;
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
            if (pSymbol is ILineFillSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.LineFillSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.LineFillSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is ILineFillSymbol)
                {
                    Display((this.m_pEditedStyle as ILineFillSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(ILineFillSymbol pLineFillSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pLineFillSymbol.Color);
                this.ceColor.Color = aColor;
                this.ceLineColor.Color = ColorHelper.CreateColor(pLineFillSymbol.Outline.Color);
                this.spinSize.Value = (decimal)pLineFillSymbol.Outline.Width;
                this.txtAngle.Text = pLineFillSymbol.Angle.ToString();
                this.txtOffset.Text = pLineFillSymbol.Offset.ToString();
                this.txtSeparation.Text = pLineFillSymbol.Separation.ToString();
                ICartographicLineSymbol pLineSymbol = pLineFillSymbol.LineSymbol as ICartographicLineSymbol;
                this.txtFillLineMiter.Text = pLineSymbol.MiterLimit.ToString();
                this.txtFillLineWidth.Text = pLineSymbol.Width.ToString();
                this.ceFillLineColor.Color = ColorHelper.CreateColor(pLineSymbol.Color);
                esriLineCapStyle aLineCapStyle = pLineSymbol.Cap;
                foreach (ComboBoxItem aItem in this.cbLineCapStyle.Properties.Items)
                {
                    if (aItem.ItemObject.Equals(aLineCapStyle))
                    {
                        this.cbLineCapStyle.SelectedItem = aItem;
                    }
                }
                esriLineJoinStyle aLineJoinStyle = pLineSymbol.Join;
                foreach (ComboBoxItem aItem in this.ceLineJoinStyle.Properties.Items)
                {
                    if (aItem.ItemObject.Equals(aLineJoinStyle))
                    {
                        this.ceLineJoinStyle.SelectedItem = aItem;
                    }
                }


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
            if (this.LineFillSymbol != null)
            {
                Symbolization(this.LineFillSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is ILineFillSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as ILineFillSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;

        private void Symbolization(ILineFillSymbol pLineFillSymbol)
        {
            try
            {
                pLineFillSymbol.Angle =Helper.OtherHelper.ChangeNullToDoubleZero(this.txtAngle.Text.Trim());
                pLineFillSymbol.Color =ColorHelper.CreateColor(this.ceColor.Color);
                pLineFillSymbol.Offset = Helper.OtherHelper.ChangeNullToDoubleZero(this.txtOffset.Text.Trim());
                pLineFillSymbol.Separation = Helper.OtherHelper.ChangeNullToDoubleZero(this.txtSeparation.Text.Trim());
                ILineSymbol pSimpleLineSym = pLineFillSymbol.Outline;
                if (pSimpleLineSym == null)
                {
                    pSimpleLineSym = new SimpleLineSymbolClass();
                }
                pSimpleLineSym.Color= ColorHelper.CreateColor(this.ceLineColor.Color);
                pSimpleLineSym.Width = (double)this.spinSize.Value;
                pLineFillSymbol.Outline = pSimpleLineSym as ILineSymbol;
                if (pLineFillSymbol.LineSymbol is ICartographicLineSymbol)
                {
                    ICartographicLineSymbol pFillLineSymbol = pLineFillSymbol.LineSymbol as ICartographicLineSymbol;
                    if (pFillLineSymbol == null)
                    {
                        pFillLineSymbol = new CartographicLineSymbolClass();
                    }
                    pFillLineSymbol.Color = ColorHelper.CreateColor(this.ceFillLineColor.Color);
                    try
                    {
                        pFillLineSymbol.Cap = (esriLineCapStyle)
                           (this.cbLineCapStyle.SelectedItem as ComboBoxItem).ItemObject;
                    }
                    catch (Exception ex) { }
                    try
                    {
                        pFillLineSymbol.Join = (esriLineJoinStyle)
                           (this.ceLineJoinStyle.SelectedItem as ComboBoxItem).ItemObject;
                    }
                    catch (Exception ex) { }
                    pFillLineSymbol.MiterLimit = Helper.OtherHelper.ChangeNullToDoubleZero(this.txtFillLineMiter.Text);
                    pFillLineSymbol.Width = Helper.OtherHelper.ChangeNullToDoubleZero(this.txtFillLineWidth.Text);
                    pLineFillSymbol.LineSymbol = pFillLineSymbol as ILineSymbol;
                }
            }
            catch (Exception ex) { }
        }
     
        #endregion

        private void cmdLine_Click(object sender, EventArgs e)
        {
            Style.StyleEditor.SymbolControlForm frm = new SymbolControlForm();
            frm.StyleClassType = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassLineSymbols;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ILineSymbol pLineSymbol = frm.m_pSymbol as ILineSymbol;
                if (pLineSymbol == null) return;
                if (this.LineFillSymbol != null)
                {
                    this.LineFillSymbol.Outline = pLineSymbol;
                }
                else
                {
                    if (this.m_pEditedStyle is ILineFillSymbol)
                    {
                        (this.m_pEditedStyle as ILineFillSymbol).Outline = pLineSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }

        private void cmdLineFill_Click(object sender, EventArgs e)
        {
            Style.StyleEditor.SymbolControlForm frm = new SymbolControlForm();
            frm.StyleClassType = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassLineSymbols;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ILineSymbol pLineSymbol = frm.m_pSymbol as ILineSymbol;
                if (pLineSymbol == null) return;
                if (this.LineFillSymbol != null)
                {
                    this.LineFillSymbol.LineSymbol = pLineSymbol;
                }
                else
                {
                    if (this.m_pEditedStyle is ILineFillSymbol)
                    {
                        (this.m_pEditedStyle as ILineFillSymbol).LineSymbol = pLineSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }

       

       
    }
}
