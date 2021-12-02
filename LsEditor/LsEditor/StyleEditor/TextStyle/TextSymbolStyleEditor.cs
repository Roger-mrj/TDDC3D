using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using stdole;
namespace RCIS.Style.StyleEditor
{
    public partial class TextSymbolStyleEditor : UserControl,IStyleEditor
    {
        public TextSymbolStyleEditor()
        {
            InitializeComponent();
            #region 提取所有可用字体
            FontFamily[] allFamily = FontFamily.Families;
            int familyCount = allFamily.Length;
            for (int fi = 0; fi < familyCount; fi++)
            {
                FontFamily ff = allFamily[fi];
                ComboBoxItem cbi = new ComboBoxItem(ff, ff.Name, fi + 1);
                this.cbFontName.Items.Add(cbi);
            }
            this.cbFontName.SelectedIndex = 0;
            #endregion
        }
        #region IStyleEditor 成员

        public string StyleClass
        {
            get
            {
                return RCIS.StyleClass.StyleClassText;
            }
        }

        public string EditorName
        {
            get
            {
                return "Text Style  | 文字样式";
            }
        }
        public void CreateInitializeStyle()
        {
            ITextSymbol aCharSym = new TextSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Size = 14;
            aCharSym.Text = this.txtText.Text;
            this.EditedStyle = aCharSym as ISymbol;
            Image sampleImage = SymbolHelper.StyleToImage(aCharSym as ISymbol, this.pbFontSample.Width, this.pbFontSample.Height);
            this.pbFontSample.Image = sampleImage;
            this.EditedStyle = aCharSym as ISymbol;


            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public ITextSymbol TextSymbol
        {
            get
            {
                return this.m_pEditedStyle as ITextSymbol;
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
            if (pSymbol is ITextSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.TextSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.TextSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is ITextSymbol)
                {
                    Display((this.m_pEditedStyle as ITextSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(ITextSymbol pTextSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pTextSymbol.Color);
                this.ceFontColor.Color = aColor;
                IFontDisp fDisp = pTextSymbol.Font;
                this.ceFontBold.Checked = fDisp.Bold;
                this.ceFontItalic.Checked = fDisp.Italic;
                this.ceFontStroke.Checked = fDisp.Strikethrough;
                this.ceFontUnderline.Checked = fDisp.Underline;
                this.cbFontSize.Text =fDisp.Size.ToString();
                this.cbFontSize.Name = fDisp.Name;
                this.txtText.Text = pTextSymbol.Text;
                Image sampleImage = SymbolHelper.StyleToImage(pTextSymbol as ISymbol, this.pbFontSample.Width, this.pbFontSample.Height);
                this.pbFontSample.Image = sampleImage;
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
            if (this.TextSymbol != null)
            {

                Symbolization(this.TextSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is ITextSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as ITextSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
        private void Symbolization(ITextSymbol pTextSymbol)
        {
            try
            {
                IFontDisp fDisp = pTextSymbol.Font;
                fDisp.Bold = this.ceFontBold.Checked;
                fDisp.Italic = this.ceFontItalic.Checked;
                fDisp.Strikethrough = this.ceFontStroke.Checked;
                fDisp.Underline = this.ceFontUnderline.Checked;
                ComboBoxItem fItem = this.cbFontName.SelectedItem as ComboBoxItem;
                string fName = "";
                if (fItem != null && fItem.ItemObject is FontFamily)
                {
                    fName = (fItem.ItemObject as FontFamily).Name;
                }
                fDisp.Name = fName;
                
                pTextSymbol.Font = fDisp;
                pTextSymbol.Color = ColorHelper.CreateColor(this.ceFontColor.Color);
                pTextSymbol.Size = Helper.OtherHelper.ChangeNullToDoubleZero(this.cbFontSize.Text);

                pTextSymbol.Text = this.txtText.Text;

                Image sampleImage = SymbolHelper.StyleToImage(pTextSymbol as ISymbol, this.pbFontSample.Width, this.pbFontSample.Height);
                this.pbFontSample.Image = sampleImage;
            }
            catch (Exception ex) { }
        }
        #endregion
    }
}
