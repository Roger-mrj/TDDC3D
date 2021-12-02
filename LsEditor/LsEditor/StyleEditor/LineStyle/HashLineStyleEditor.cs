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
    public partial class HashLineStyleEditor : UserControl,IStyleEditor
    {
        public HashLineStyleEditor()
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
                return "Hash Line Style  | 间断线样式";
            }
        }
        public void CreateInitializeStyle()
        {
            IHashLineSymbol aCharSym = new HashLineSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Width = 1;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IHashLineSymbol HashLineSymbol
        {
            get
            {
                return this.m_pEditedStyle as IHashLineSymbol;
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
            if (pSymbol is IHashLineSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.HashLineSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.HashLineSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IHashLineSymbol)
                {
                    Display((this.m_pEditedStyle as IHashLineSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IHashLineSymbol pHashLineSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pHashLineSymbol.Color);
                this.ceColor.Color = aColor;

                this.spinSize.Value = (decimal)pHashLineSymbol.Width;
                this.txtAngle.Text = pHashLineSymbol.Angle.ToString();
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
            if (this.HashLineSymbol != null)
            {

                Symbolization(this.HashLineSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IHashLineSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IHashLineSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private void Symbolization(IHashLineSymbol pHashLineSymbol)
        {
            try
            {
                pHashLineSymbol.Width = (double)this.spinSize.Value;

                pHashLineSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);


                pHashLineSymbol.Angle = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtAngle.Text.Trim());
            }
            catch (Exception ex) { }
        }
        private bool m_shouldAction = true;

        #endregion
    }
}
