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
    public partial class PictureMarkerStyleEditor : UserControl,IStyleEditor
    {
        public PictureMarkerStyleEditor()
        {
            InitializeComponent();
        }

        #region IStyleEditor 成员
        private ISymbol m_pEditedStyle;       
        private bool m_shouldAction=true ;
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
                return "Picture Marker Style  | 图片点样式";
            }
            
        }

        public void CreateInitializeStyle()
        {
            this.EditedStyle = new PictureMarkerSymbolClass();
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
                    this.CreateInitializeStyle();
                else
                {
                    this.m_pEditedStyle = value;
                    this.DispatchStyle();
                }
            }
        }
        public IPictureMarkerSymbol MarkerStyle
        {
            get
            {
                return this.m_pEditedStyle as IPictureMarkerSymbol;
            }
        }
        private void DispatchStyle()
        {
            if (this.MarkerStyle != null)
            {
                this.m_shouldAction = false;
                try
                {
                    this.spinSize.Value = (decimal)this.MarkerStyle.Size;
                    this.spinAngle.Value = (decimal)this.MarkerStyle.Angle;
                    this.spinOffsetX .Value= (decimal)this.MarkerStyle.XOffset;
                    this.spinOffsetY.Value = (decimal)this.MarkerStyle.YOffset;                    
                }
                catch (Exception ex) { }
                this.m_shouldAction = true;
            }
        }
        public void UpdateEditedStyle()
        {
            if (this.MarkerStyle != null)
            {
                try
                {
                    this.MarkerStyle .Size =(double)this.spinSize .Value ;
                    this.MarkerStyle .Angle =(double)this.spinAngle .Value ;
                    this.MarkerStyle .XOffset =(double)this.spinOffsetX .Value ;
                    this.MarkerStyle .YOffset =(double)this.spinOffsetY .Value ;
                }
                catch (Exception ex) { }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());
            }
        }
        public bool CanEdit(ISymbol pSymbol)
        {
            if (pSymbol is IPictureMarkerSymbol) return true;
            return false;
        }
        
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;

        #endregion

        private void OnStylePropertyChanged(object sender, EventArgs e)
        {
            if (this.m_shouldAction)
                this.UpdateEditedStyle();
        }

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
                    this.MarkerStyle.Picture = aMarkerSym.Picture;
                    this.UpdateEditedStyle();
                }
                catch (Exception ex) { }
            }
        }
    }
}
