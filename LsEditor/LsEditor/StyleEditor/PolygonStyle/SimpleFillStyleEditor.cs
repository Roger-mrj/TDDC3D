using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
namespace RCIS.Style.StyleEditor
{
    public partial class SimpleFillStyleEditor : UserControl,IStyleEditor
    {
        public SimpleFillStyleEditor()
        {
            InitializeComponent();
            this.ceStyle.Properties.Items.Add(
             new ComboBoxItem(esriSimpleFillStyle.esriSFSBackwardDiagonal, "BackwardDiagonal"));
            this.ceStyle.Properties.Items.Add(
             new ComboBoxItem(esriSimpleFillStyle.esriSFSCross, "Cross"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSDiagonalCross, "DiagonalCross"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSForwardDiagonal, "ForwardDiagonal"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSHollow, "Hollow"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSHorizontal, "Horizontal"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSNull, "Null"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSSolid, "Solid"));
            this.ceStyle.Properties.Items.Add(
           new ComboBoxItem(esriSimpleFillStyle.esriSFSVertical, "Vertical"));
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
                return "Simple Fill Style  | 简单面填充样式";
            }
        }
        public void CreateInitializeStyle()
        {
            ISimpleFillSymbol aCharSym = new SimpleFillSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
            aCharSym.Style = esriSimpleFillStyle.esriSFSSolid;
            ISimpleLineSymbol pLine = new SimpleLineSymbolClass();
            pLine.Color = ColorHelper.CreateColor(0, 0, 0);
            pLine.Width = 1;
            aCharSym.Outline = pLine as ILineSymbol;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public ISimpleFillSymbol SimpleFillSymbol
        {
            get
            {
                return this.m_pEditedStyle as ISimpleFillSymbol;
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
            if (pSymbol is ISimpleFillSymbol)
            {
                return true;
            }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.SimpleFillSymbol != null)
            {
                this.m_shouldAction = false;
                try
                {
                    Color aColor = ColorHelper.CreateColor(this.SimpleFillSymbol.Color);
                    this.ceColor.Color = aColor;
                    esriSimpleFillStyle aLineStyle = this.SimpleFillSymbol.Style;
                    foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                    {
                        if (aItem.ItemObject.Equals(aLineStyle))
                        {
                            this.ceStyle.SelectedItem = aItem;
                        }
                    }
                    this.ceLineColor.Color = ColorHelper.CreateColor(this.SimpleFillSymbol.Outline.Color);
                    this.spinSize.Value = (decimal)this.SimpleFillSymbol.Outline.Width;
                }
                catch (Exception ex) { }
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is ISimpleFillSymbol)
                {
                    try
                    {
                        Color aColor = ColorHelper.CreateColor((this.m_pEditedStyle as ISimpleFillSymbol).Color);
                        this.ceColor.Color = aColor;
                        esriSimpleFillStyle aLineStyle = (this.m_pEditedStyle as ISimpleFillSymbol).Style;
                        foreach (ComboBoxItem aItem in this.ceStyle.Properties.Items)
                        {
                            if (aItem.ItemObject.Equals(aLineStyle))
                            {
                                this.ceStyle.SelectedItem = aItem;
                            }
                        }
                        this.ceLineColor.Color = ColorHelper.CreateColor((this.m_pEditedStyle as ISimpleFillSymbol).Outline.Color);
                        this.spinSize.Value = (decimal)(this.m_pEditedStyle as ISimpleFillSymbol).Outline.Width;
                    }
                    catch (Exception ex) { }
                }
            }
            this.m_shouldAction = true;
        }
        private void OnEditedStylePropertyChanged(object pSender, EventArgs pArg)
        {
            if (!this.m_shouldAction || this.EditedStyle == null)
            {
                DispatchStyle();
                return;
            }
            if (this.SimpleFillSymbol != null)
            {

                Symbolization(this.SimpleFillSymbol);
            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is ISimpleFillSymbol)
                    {
                        Symbolization((this.m_pEditedStyle as ISimpleFillSymbol));
                    }
                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private bool m_shouldAction = true;
        private void Symbolization(ISimpleFillSymbol pSimpleFillSymbol)
        {
            try
            {
                ILineSymbol pSimpleLine = pSimpleFillSymbol.Outline;
                if (pSimpleLine == null)
                {
                    pSimpleLine = new SimpleLineSymbolClass();
                }
                pSimpleLine.Width = (double)this.spinSize.Value;
                pSimpleLine.Color = ColorHelper.CreateColor(this.ceLineColor.Color);

                pSimpleFillSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                try
                {
                    pSimpleFillSymbol.Style = (esriSimpleFillStyle)
                       (this.ceStyle.SelectedItem as ComboBoxItem).ItemObject;
                }
                catch (Exception ex) { }
                pSimpleFillSymbol.Outline = pSimpleLine as ILineSymbol;
            }
            catch (Exception ex) { }
        }
        #endregion

        private void cmdLine_Click(object sender, EventArgs e)
        {
            Style.StyleEditor.SymbolControlForm frm = new SymbolControlForm();
            frm.StyleClassType = esriSymbologyStyleClass.esriStyleClassLineSymbols;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ILineSymbol pLineSymbol=frm.m_pSymbol as ILineSymbol;
                this.spinSize.Value = (decimal)pLineSymbol.Width;
                this.ceLineColor.Color = ColorHelper.CreateColor(pLineSymbol.Color);
                if (this.SimpleFillSymbol != null)
                {
                    this.SimpleFillSymbol.Outline = pLineSymbol ;
                }
                else
                {
                    if (this.m_pEditedStyle is ISimpleFillSymbol)
                    {
                        (this.m_pEditedStyle as ISimpleFillSymbol).Outline = pLineSymbol ;
                    }
                }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());

            }
        }
    }
}
