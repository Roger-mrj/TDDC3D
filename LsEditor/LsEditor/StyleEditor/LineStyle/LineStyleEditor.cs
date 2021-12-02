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
    public partial class LineStyleEditor : UserControl,IStyleEditor
    {
        public LineStyleEditor()
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
                return " Line Style | 基本线样式";
            }
        }
        public void CreateInitializeStyle()
        {
            ILineSymbol aCharSym = new SimpleLineSymbolClass();
            aCharSym.Color = ColorHelper.CreateColor(0, 0, 0);
       
            aCharSym.Width = 1;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());

        }
        private ISymbol m_pEditedStyle;
        public ILineSymbol LineSymbol
        {
            get
            {
                return this.m_pEditedStyle as ILineSymbol;
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
         
            if (pSymbol is ILineSymbol) { return true; }
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.LineSymbol != null)
            {
                this.m_shouldAction = false;
                try
                {
                    Color aColor = ColorHelper.CreateColor(this.LineSymbol.Color);
                    this.ceColor.Color = aColor;
                   
                    this.spinSize.Value = (decimal)this.LineSymbol.Width;
                }
                catch (Exception ex) { }
            }
            else
            {
                this.m_shouldAction = false;
                if (this.m_pEditedStyle is ILineSymbol)
                {
                    try
                    {
                        Color aColor = ColorHelper.CreateColor((this.m_pEditedStyle as ILineSymbol).Color);
                        this.ceColor.Color = aColor;
                        this.spinSize.Value = (decimal)(this.m_pEditedStyle as ILineSymbol).Width;
                    }
                    catch (Exception ex) { }
                }
               
            }
            this.m_shouldAction = true;
        }
  
        private bool m_shouldAction = true;

        #endregion

        private void OnEditedStylePropertyChanged(object pSender, EventArgs pArg)
        {
            if (!this.m_shouldAction || this.EditedStyle == null)
            {
                DispatchStyle();
                return;
            }
            if (this.LineSymbol != null)
            {
                try
                {
                    this.LineSymbol.Width = (double)this.spinSize.Value;

                    this.LineSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                }
                catch (Exception ex) { }

            }
            else
            {
                try
                {
                    if (this.m_pEditedStyle is ILineSymbol)
                    {
                        (this.m_pEditedStyle as ILineSymbol).Width = (double)this.spinSize.Value;

                        (this.m_pEditedStyle as ILineSymbol).Color = ColorHelper.CreateColor(this.ceColor.Color);
                    }

                }
                catch (Exception ex) { }
            }
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
    }
}
