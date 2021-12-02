using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
namespace RCIS.Style.StyleEditor.LineStyle
{
    public partial class MarkerLineStyleEditor : UserControl,IStyleEditor
    {
        public MarkerLineStyleEditor()
        {
            InitializeComponent();
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
                return "Marker Line Style  | 标记线样式";
            }
        }
        public void CreateInitializeStyle()
        {
            IMarkerLineSymbol aCharSym = new MarkerLineSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Width = 1;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IMarkerLineSymbol MarkerLineSymbol
        {
            get
            {
                return this.m_pEditedStyle as IMarkerLineSymbol;
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
            if (pSymbol is IMarkerLineSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.MarkerLineSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.MarkerLineSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IMarkerLineSymbol)
                {
                    Display((this.m_pEditedStyle as IMarkerLineSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IMarkerLineSymbol pMarkerLineSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pMarkerLineSymbol.Color);
                this.ceColor.Color = aColor;

                this.spinSize.Value = (decimal)pMarkerLineSymbol.Width;
         
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
            if (this.MarkerLineSymbol != null)
            {

                Symbolization(this.MarkerLineSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IMarkerLineSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IMarkerLineSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private void Symbolization(IMarkerLineSymbol pMarkerLineSymbol)
        {
            try
            {
                pMarkerLineSymbol.Width = (double)this.spinSize.Value;

                pMarkerLineSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);


             
            }
            catch (Exception ex) { }
        }
        private bool m_shouldAction = true;

        #endregion

        private void cmdMarker_Click(object sender, EventArgs e)
        {
            Style.StyleEditor.SymbolControlForm frm = new SymbolControlForm();
            frm.StyleClassType = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                IMarkerSymbol pMarkerSymbol = frm.m_pSymbol as IMarkerSymbol;

                if (this.MarkerLineSymbol != null)
                {
                    this.MarkerLineSymbol.MarkerSymbol = pMarkerSymbol as IMarkerSymbol;
                }
                else
                {
                    if (this.m_pEditedStyle is IMarkerLineSymbol)
                    {
                        (this.m_pEditedStyle as IMarkerLineSymbol).MarkerSymbol = pMarkerSymbol as IMarkerSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }
    }
}
