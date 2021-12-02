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
    public partial class PictureFillStyleEditor : UserControl, IStyleEditor
    {
        public PictureFillStyleEditor()
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
                return "Picture Fill Style  | 图片填充样式";
            }
        }
        public void CreateInitializeStyle()
        {
            IPictureFillSymbol aCharSym = new PictureFillSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(255, 0, 0);
            aCharSym.Angle = 0;
            aCharSym.BackgroundColor = ColorHelper.CreateColor(0, 0, 0);
            IPictureMarkerSymbol aMarkerSym = null;
            string sFilePath = FileHelper.FindFolder("style");
                    aMarkerSym = new PictureMarkerSymbolClass();
                    aMarkerSym.CreateMarkerSymbolFromFile(esriIPictureType.esriIPictureBitmap, sFilePath+"\\Pictures\\herring2.bmp");

                    aCharSym.Picture = aMarkerSym.Picture;
            ISimpleLineSymbol pLine = new SimpleLineSymbolClass();
            pLine.Color = ColorHelper.CreateColor(0, 0, 0);
            pLine.Width = 1;
            aCharSym.Outline = pLine as ILineSymbol;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IPictureFillSymbol PictureFillSymbol
        {
            get
            {
                return this.m_pEditedStyle as IPictureFillSymbol;
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
            if (pSymbol is IPictureFillSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.PictureFillSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.PictureFillSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IPictureFillSymbol)
                {
                    Display((this.m_pEditedStyle as IPictureFillSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IPictureFillSymbol pPictureFillSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pPictureFillSymbol.Color);
                this.ceColor.Color = aColor;

                this.ceLineColor.Color = ColorHelper.CreateColor(pPictureFillSymbol.Outline.Color);
                this.spinSize.Value = (decimal)pPictureFillSymbol.Outline.Width;

                this.ceBackgroundColor.Color = ColorHelper.CreateColor(pPictureFillSymbol.BackgroundColor);
                this.ceBitmapTransparencyColor.Color = ColorHelper.CreateColor(pPictureFillSymbol.BitmapTransparencyColor);
                this.txtAngle.Text = pPictureFillSymbol.Angle.ToString();
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
            if (this.PictureFillSymbol != null)
            {

                Symbolization(this.PictureFillSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IPictureFillSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IPictureFillSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
        private void Symbolization(IPictureFillSymbol pPictureFillSymbol)
        {
            try
            {
                ILineSymbol pSimpleLine = pPictureFillSymbol.Outline;
                if (pSimpleLine == null)
                {
                    pSimpleLine = new SimpleLineSymbolClass();
                }
                pSimpleLine.Width = (double)this.spinSize.Value;
                pSimpleLine.Color = ColorHelper.CreateColor(this.ceLineColor.Color);

                pPictureFillSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                pPictureFillSymbol.BackgroundColor = ColorHelper.CreateColor(this.ceBackgroundColor.Color);
                pPictureFillSymbol.BitmapTransparencyColor = ColorHelper.CreateColor(this.ceBitmapTransparencyColor.Color);
                pPictureFillSymbol.Angle = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtAngle.Text.Trim());
                pPictureFillSymbol.SwapForeGroundBackGroundColor = this.checkEdit1.Checked;

                pPictureFillSymbol.Outline = pSimpleLine as ILineSymbol;
            }
            catch (Exception ex) { }
        }
        #endregion

        private void cmdPicture_Click(object sender, EventArgs e)
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
                    string aExt = System.IO.Path.GetExtension(ofd.FileName).ToUpper();
                    IPictureMarkerSymbol aMarkerSym = null;
                    if (aExt.Equals("BMP"))
                    {
                        aMarkerSym = new PictureMarkerSymbolClass();
                        aMarkerSym.CreateMarkerSymbolFromFile(esriIPictureType.esriIPictureBitmap, ofd.FileName);
                    }
                    else if (aExt.Equals("EMF"))
                    {
                        aMarkerSym = new PictureMarkerSymbolClass();
                        aMarkerSym.CreateMarkerSymbolFromFile(esriIPictureType.esriIPictureEMF, ofd.FileName);
                    }
                    this.PictureFillSymbol.Picture = aMarkerSym.Picture;
                    if (this.OnEditedStyleChanged != null)
                        this.OnEditedStyleChanged(this, new EventArgs());
                }
                catch (Exception ex) { }
            }
        }

        private void cmdLine_Click(object sender, EventArgs e)
        {
            Style.StyleEditor.SymbolControlForm frm = new SymbolControlForm();
            frm.StyleClassType = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassLineSymbols;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ILineSymbol pLineSymbol = frm.m_pSymbol as ILineSymbol;
                if (pLineSymbol == null) return;
                if (this.PictureFillSymbol != null)
                {
                    this.PictureFillSymbol.Outline = pLineSymbol;
                }
                else
                {
                    if (this.m_pEditedStyle is IPictureFillSymbol)
                    {
                        (this.m_pEditedStyle as IPictureFillSymbol).Outline = pLineSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }
    }
}
