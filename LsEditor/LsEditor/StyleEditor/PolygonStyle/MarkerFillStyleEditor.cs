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
    public partial class MarkerFillStyleEditor : UserControl,IStyleEditor
    {
        public MarkerFillStyleEditor()
        {
            InitializeComponent();
            this.ceStyle.Properties.Items.Add(
             new ComboBoxItem(esriMarkerFillStyle.esriMFSGrid, "Grid"));
            this.ceStyle.Properties.Items.Add(
             new ComboBoxItem(esriMarkerFillStyle.esriMFSRandom, "Random"));

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
                return "Marker Fill Style  | 点填充样式";
            }
        }
        public void CreateInitializeStyle()
        {

            IMarkerFillSymbol aCharSym = new MarkerFillSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Style = esriMarkerFillStyle.esriMFSGrid;
            aCharSym.GridAngle = 0;
            ISimpleLineSymbol pLine = new SimpleLineSymbolClass();
            pLine.Color = ColorHelper.CreateColor(0, 0, 0);
            pLine.Width = 1;
            aCharSym.Outline = pLine as ILineSymbol;

            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IMarkerFillSymbol MarkerFillSymbol
        {
            get
            {
                return this.m_pEditedStyle as IMarkerFillSymbol;
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
            if (pSymbol is IMarkerFillSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.MarkerFillSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.MarkerFillSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IMarkerFillSymbol)
                {
                    Display((this.m_pEditedStyle as IMarkerFillSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IMarkerFillSymbol pMarkFillSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pMarkFillSymbol.Color);
                this.ceColor.Color = aColor;
                esriMarkerFillStyle aStyle = pMarkFillSymbol.Style;
                foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                {
                    if (aItem.ItemObject.Equals(aStyle))
                    {
                        this.ceStyle.SelectedItem = aItem;
                    }
                }
                this.txtAngle.Text = pMarkFillSymbol.GridAngle.ToString();
            //    ISimpleLineSymbol pLine = pMarkFillSymbol.Outline as ISimpleLineSymbol;
                this.ceLineColor.Color = ColorHelper.CreateColor(pMarkFillSymbol.Outline.Color);
                this.spinSize.Value = (decimal)pMarkFillSymbol.Outline.Width;

                IMarkerSymbol pMarkSymbol = pMarkFillSymbol.MarkerSymbol;
                if (pMarkSymbol != null)
                {
                    this.ceMarkerColor.Color = ColorHelper.CreateColor(pMarkSymbol.Color);
                    this.txtMarkerAngle.Text = pMarkSymbol.Angle.ToString();
                    this.spinMarkerSize.Value = (decimal)pMarkSymbol.Size;
                    this.spinOffsetX.Value = (decimal)pMarkSymbol.XOffset;
                    this.spinOffsetY.Value = (decimal)pMarkSymbol.YOffset;
                }
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
            if (this.MarkerFillSymbol != null)
            {

                Symbolization(this.MarkerFillSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IMarkerFillSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IMarkerFillSymbol));
                    }
                }
                catch  { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
        private void Symbolization(IMarkerFillSymbol pMarkerFillSymbol)
        {
            try
            {
               
                pMarkerFillSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                try
                {
                    pMarkerFillSymbol.Style = (esriMarkerFillStyle)
                       (this.ceStyle.SelectedItem as ComboBoxItem).ItemObject;
                }
                catch { }
                pMarkerFillSymbol.GridAngle = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtAngle.Text.Trim());

                IMarkerSymbol pMarkerSymbol = pMarkerFillSymbol.MarkerSymbol;
                if (pMarkerSymbol != null)
                {
                    pMarkerSymbol.Color = ColorHelper.CreateColor(this.ceMarkerColor.Color);
                    pMarkerSymbol.Size = (double)this.spinMarkerSize.Value;
                    pMarkerSymbol.Angle = Helper.OtherHelper.ChangeNullToDoubleZero(
                     this.txtMarkerAngle.Text.Trim());
                    pMarkerSymbol.XOffset = (double)this.spinOffsetX.Value;
                    pMarkerSymbol.YOffset = (double)this.spinOffsetY.Value;
                    pMarkerFillSymbol.MarkerSymbol = pMarkerSymbol;
                }
                ILineSymbol pSimpleLine = pMarkerFillSymbol.Outline;
                if (pSimpleLine == null)
                {
                    pSimpleLine = new SimpleLineSymbolClass();
                }
                pSimpleLine.Width = (double)this.spinSize.Value;
                pSimpleLine.Color = ColorHelper.CreateColor(this.ceLineColor.Color);

                pMarkerFillSymbol.Outline = pSimpleLine as ILineSymbol;
            }
            catch  { }
        }
        #endregion

       

        private void cmdMarker_Click(object sender, EventArgs e)
        {
            Style.StyleEditor.SymbolControlForm frm = new SymbolControlForm();
            frm.StyleClassType = ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                IMarkerSymbol pMarkerSymbol = frm.m_pSymbol as IMarkerSymbol;
               
                if (this.MarkerFillSymbol != null)
                {
                    this.MarkerFillSymbol.MarkerSymbol = pMarkerSymbol as IMarkerSymbol;
                    Display(this.MarkerFillSymbol);
                }
                else
                {
                    if (this.m_pEditedStyle is IMarkerFillSymbol)
                    {
                        (this.m_pEditedStyle as IMarkerFillSymbol).MarkerSymbol = pMarkerSymbol as IMarkerSymbol;
                        Display((this.m_pEditedStyle as IMarkerFillSymbol));
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

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
                if (this.MarkerFillSymbol != null)
                {
                    this.MarkerFillSymbol.Outline = pLineSymbol; 
                }
                else
                {
                    if (this.m_pEditedStyle is IMarkerFillSymbol)
                    {
                        (this.m_pEditedStyle as IMarkerFillSymbol).Outline = pLineSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }

      
    }
}
