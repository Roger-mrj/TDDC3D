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
    public partial class DotDensityFillStyleEditor : UserControl,IStyleEditor
    {
        public DotDensityFillStyleEditor()
        {
            InitializeComponent();
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
                return "DotDensity Fill Style  | 离散点填充样式";
            }
        }
        public void CreateInitializeStyle()
        {
            IDotDensityFillSymbol aCharSym = new DotDensityFillSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.DotSize = 2;
            aCharSym.DotSpacing = 1;
            aCharSym.FixedPlacement = true;
            //aCharSym.set_DotCount(1, 1);
            
            ISimpleLineSymbol pLine = new SimpleLineSymbolClass();
            pLine.Color = ColorHelper.CreateColor(0, 0, 0);
            pLine.Width = 1;
            aCharSym.Outline = pLine as ILineSymbol;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IDotDensityFillSymbol DotDensityFillSymbol
        {
            get
            {
                return this.m_pEditedStyle as IDotDensityFillSymbol;
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
            if (pSymbol is IDotDensityFillSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.DotDensityFillSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.DotDensityFillSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IGradientFillSymbol)
                {
                    Display((this.m_pEditedStyle as IDotDensityFillSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IDotDensityFillSymbol pDotDensityFillSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pDotDensityFillSymbol.Color);
                this.ceColor.Color = aColor;

                this.ceLineColor.Color = ColorHelper.CreateColor(pDotDensityFillSymbol.Outline.Color);
                this.spinSize.Value = (decimal)pDotDensityFillSymbol.Outline.Width;
                this.ceBackgroundColor.Color = ColorHelper.CreateColor(pDotDensityFillSymbol.BackgroundColor);

                this.spDotCount.Value = (decimal)pDotDensityFillSymbol.get_DotCount(1);
                this.spDotSize.Value = (decimal)(pDotDensityFillSymbol.DotSize);
                this.txtDotSpacing.Text = pDotDensityFillSymbol.DotSpacing.ToString();
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
            if (this.DotDensityFillSymbol != null)
            {

                Symbolization(this.DotDensityFillSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IDotDensityFillSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IDotDensityFillSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
        private void Symbolization(IDotDensityFillSymbol pDotDensityFillSymbol)
        {
            try
            {
                ILineSymbol pSimpleLine = pDotDensityFillSymbol.Outline;
                if (pSimpleLine == null)
                {
                    pSimpleLine = new SimpleLineSymbolClass();
                }
                pSimpleLine.Width = (double)this.spinSize.Value;
                pSimpleLine.Color = ColorHelper.CreateColor(this.ceLineColor.Color);

                pDotDensityFillSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                pDotDensityFillSymbol.BackgroundColor = ColorHelper.CreateColor(this.ceBackgroundColor.Color);
                pDotDensityFillSymbol.DotSpacing = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtDotSpacing.Text.Trim());
                pDotDensityFillSymbol.DotSize = ((double)this.spDotSize.Value);
                pDotDensityFillSymbol.set_DotCount(1, (int)this.spDotCount.Value);

                pDotDensityFillSymbol.Outline = pSimpleLine as ILineSymbol;
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
                if (this.DotDensityFillSymbol != null)
                {
                    this.DotDensityFillSymbol.Outline = pLineSymbol;
                }
                else
                {
                    if (this.m_pEditedStyle is IDotDensityFillSymbol)
                    {
                        (this.m_pEditedStyle as IDotDensityFillSymbol).Outline = pLineSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }
    }
}
