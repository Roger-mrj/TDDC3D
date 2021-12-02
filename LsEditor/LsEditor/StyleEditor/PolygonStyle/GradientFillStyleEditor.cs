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
    public partial class GradientFillStyleEditor : UserControl,IStyleEditor
    {
        public GradientFillStyleEditor()
        {
            InitializeComponent();
            this.ceStyle.Properties.Items.Add(
         new ComboBoxItem(esriGradientFillStyle.esriGFSBuffered, "Buffered"));
            this.ceStyle.Properties.Items.Add(
         new ComboBoxItem(esriGradientFillStyle.esriGFSCircular, "Circular"));
            this.ceStyle.Properties.Items.Add(
         new ComboBoxItem(esriGradientFillStyle.esriGFSLinear, "Linear"));
            this.ceStyle.Properties.Items.Add(
         new ComboBoxItem(esriGradientFillStyle.esriGFSRectangular, "Rectangular"));

            this.cbColor.Properties.Items.Add(
       new ComboBoxItem(esriColorRampAlgorithm.esriCIELabAlgorithm, "CIELab"));
            this.cbColor.Properties.Items.Add(
         new ComboBoxItem(esriColorRampAlgorithm.esriHSVAlgorithm, "HSV"));
            this.cbColor.Properties.Items.Add(
         new ComboBoxItem(esriColorRampAlgorithm.esriLabLChAlgorithm, "LabLCh"));
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
                return "Gradient Fill Style  | 分级色彩填充样式";
            }
        }
        public void CreateInitializeStyle()
        {
            IGradientFillSymbol aCharSym = new GradientFillSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Style = esriGradientFillStyle.esriGFSLinear;
            aCharSym.GradientAngle = Helper.OtherHelper.ChangeNullToDoubleZero(this.txtAngle.Text.Trim());
            aCharSym.GradientPercentage =( (double)this.spGradientPercentage.Value)/100;

            ISimpleLineSymbol pLine = new SimpleLineSymbolClass();
            pLine.Color = ColorHelper.CreateColor(0, 0, 0);
            pLine.Width = 1;
            aCharSym.Outline = pLine as ILineSymbol;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public IGradientFillSymbol GradientFillSymbol
        {
            get
            {
                return this.m_pEditedStyle as IGradientFillSymbol;
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
            if (pSymbol is IGradientFillSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.GradientFillSymbol != null)
            {
                this.m_shouldAction = false;
                Display(this.GradientFillSymbol);
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is IGradientFillSymbol)
                {
                    Display((this.m_pEditedStyle as IGradientFillSymbol));
                }
            }
            this.m_shouldAction = true;
        }
        private void Display(IGradientFillSymbol pGradientFillSymbol)
        {
            try
            {
                Color aColor = ColorHelper.CreateColor(pGradientFillSymbol.Color);
                this.ceColor.Color = aColor;
                esriGradientFillStyle aLineStyle = pGradientFillSymbol.Style;
                foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                {
                    if (aItem.ItemObject.Equals(aLineStyle))
                    {
                        this.ceStyle.SelectedItem = aItem;
                    }
                }
                this.ceLineColor.Color = ColorHelper.CreateColor(pGradientFillSymbol.Outline.Color);
                this.spinSize.Value = (decimal)pGradientFillSymbol.Outline.Width;
                this.spIntervalCount.Value = (decimal)pGradientFillSymbol.IntervalCount;
                this.spGradientPercentage.Value = (decimal)(pGradientFillSymbol.GradientPercentage*100);
                this.txtAngle.Text = pGradientFillSymbol.GradientAngle.ToString();
                IColorRamp pRamp = pGradientFillSymbol.ColorRamp;
                if (pRamp is IAlgorithmicColorRamp)
                {
                    IAlgorithmicColorRamp pAl = pRamp as IAlgorithmicColorRamp;
                    this.ceFromColor.Color = ColorHelper.CreateColor(pAl.FromColor);
                    this.ceToColor.Color = ColorHelper.CreateColor(pAl.ToColor);
                    this.spSize.Value = (decimal)pAl.Size;
                    esriColorRampAlgorithm aColorStyle = pAl.Algorithm;
                    foreach (ComboBoxItem aItem in this.cbColor.Properties.Items)
                    {
                        if (aItem.ItemObject.Equals(aColorStyle))
                        {
                            this.cbColor.SelectedItem = aItem;
                        }
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
            if (this.GradientFillSymbol != null)
            {

                Symbolization(this.GradientFillSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IGradientFillSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as IGradientFillSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
        private void Symbolization(IGradientFillSymbol pGradientFillSymbol)
        {
            try
            {
                ILineSymbol pSimpleLine = pGradientFillSymbol.Outline;
                if (pSimpleLine == null)
                {
                    pSimpleLine = new SimpleLineSymbolClass();
                }
                pSimpleLine.Width = (double)this.spinSize.Value;
                pSimpleLine.Color = ColorHelper.CreateColor(this.ceLineColor.Color);

                pGradientFillSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                pGradientFillSymbol.GradientAngle = Helper.OtherHelper.ChangeNullToDoubleZero(
                    this.txtAngle.Text.Trim());
                pGradientFillSymbol.GradientPercentage = ((double)this.spGradientPercentage.Value) / 100;
                pGradientFillSymbol.IntervalCount = (int)this.spIntervalCount.Value;
                try
                {
                    pGradientFillSymbol.Style = (esriGradientFillStyle)
                       (this.ceStyle.SelectedItem as ComboBoxItem).ItemObject;
                }
                catch (Exception ex) { }
                pGradientFillSymbol.Outline = pSimpleLine as ILineSymbol;
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
                if (this.GradientFillSymbol != null)
                {
                    this.GradientFillSymbol.Outline = pLineSymbol;
                }
                else
                {
                    if (this.m_pEditedStyle is IGradientFillSymbol)
                    {
                        (this.m_pEditedStyle as IGradientFillSymbol).Outline = pLineSymbol;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }

        private void cmdColor_Click(object sender, EventArgs e)
        {
            IAlgorithmicColorRamp pRamp = new AlgorithmicColorRampClass();
            pRamp.FromColor = ColorHelper.CreateColor(this.ceFromColor.Color);
            pRamp.ToColor = ColorHelper.CreateColor(this.ceToColor.Color);
            pRamp.Size = (int)this.spSize.Value;
            pRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
            bool ok=false;
            pRamp.CreateRamp(out ok);
          
            if (this.GradientFillSymbol != null)
            {

                this.GradientFillSymbol.ColorRamp = pRamp as IColorRamp;
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is IGradientFillSymbol)
                    {
                        (this.m_pEditedStyle as IGradientFillSymbol).ColorRamp= pRamp as IColorRamp;
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
    }
}
