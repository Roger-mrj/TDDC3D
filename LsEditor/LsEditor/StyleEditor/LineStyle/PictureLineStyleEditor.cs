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
    public partial class PictureLineStyleEditor : UserControl,IStyleEditor
    {
        public PictureLineStyleEditor()
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
                return "Picture Line Style  | 图片线样式";
            }
        }
        public void CreateInitializeStyle()
        {
            IPictureLineSymbol aCharSym = new PictureLineSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Width = 2;
            IPictureMarkerSymbol aMarkerSym = null;
            string sFilePath = FileHelper.FindFolder("style");
            aMarkerSym = new PictureMarkerSymbolClass();
            aMarkerSym.CreateMarkerSymbolFromFile(esriIPictureType.esriIPictureBitmap, sFilePath + "\\Pictures\\herring2.bmp");

            aCharSym.Picture = aMarkerSym.Picture;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IPictureLineSymbol PictureLineSymbol
        {
            get
            {
                return this.m_pEditedStyle as IPictureLineSymbol;
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
            if (pSymbol is IPictureLineSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.PictureLineSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.PictureLineSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IPictureLineSymbol)
                {
                    Display((this.m_pEditedStyle as IPictureLineSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IPictureLineSymbol pPictureLineSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pPictureLineSymbol.Color);
                this.ceColor.Color = aColor;

                this.spinEdit1.Value = (decimal)pPictureLineSymbol.Width;
                this.txtOffset.Text = pPictureLineSymbol.Offset.ToString();
                this.txtXScale.Text = pPictureLineSymbol.XScale.ToString();
                this.txtYScale.Text = pPictureLineSymbol.YScale.ToString();
                this.ceBackgroundColor.Color = ColorHelper.CreateColor(pPictureLineSymbol.BackgroundColor);
                this.checkEdit1.Checked = pPictureLineSymbol.Rotate;
            }
            catch  { }
        }
        private void OnEditedStylePropertyChanged(object pSender, EventArgs pArg)
        {
            if (!this.m_shouldAction || this.EditedStyle == null)
            {
                DispatchStyle();
                return;
            }
            if (this.PictureLineSymbol != null)
            {

                Symbolization(this.PictureLineSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IPictureLineSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IPictureLineSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private void Symbolization(IPictureLineSymbol pPictureLineSymbol)
        {
            try
            {
                pPictureLineSymbol.Width = (double)this.spinEdit1.Value;
                pPictureLineSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                pPictureLineSymbol.BackgroundColor = ColorHelper.CreateColor(this.ceBackgroundColor.Color);
                pPictureLineSymbol.Offset = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtOffset.Text.Trim());
                pPictureLineSymbol.XScale = Helper.OtherHelper.ChangeNullToDoubleZero(
                   this.txtXScale.Text.Trim());
                pPictureLineSymbol.YScale = Helper.OtherHelper.ChangeNullToDoubleZero(
                   this.txtYScale.Text.Trim());
                pPictureLineSymbol.Rotate = this.checkEdit1.Checked;
                pPictureLineSymbol.SwapForeGroundBackGroundColor = this.checkEdit2.Checked;
            }
            catch  { }
        }
        private bool m_shouldAction = true;

        #endregion

        private void btnSelectPicure_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Windows位图(*.bmp)|*.bmp"
            + "|图像元文件(*.emf)|*.emf";
            ofd.Multiselect = false;
            ofd.ReadOnlyChecked = true;
            ofd.Title = "打开图片文件";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string aExt = System.IO.Path.GetExtension(ofd.FileName).ToUpper();// FileHelper.GetFileExtentName(ofd.FileName).ToUpper();
                    IPictureLineSymbol aLineSym = null;
                    if (aExt.Equals("BMP"))
                    {
                        aLineSym = new PictureLineSymbolClass();
                        aLineSym.CreateLineSymbolFromFile(esriIPictureType.esriIPictureBitmap, ofd.FileName);
                    }
                    else if (aExt.Equals("EMF"))
                    {
                        aLineSym = new PictureLineSymbolClass();
                        aLineSym.CreateLineSymbolFromFile(esriIPictureType.esriIPictureEMF, ofd.FileName);
                    }
                    this.PictureLineSymbol.Picture = aLineSym.Picture;         
                }
                catch (Exception ex) { }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());
            }
        }
    }
}
