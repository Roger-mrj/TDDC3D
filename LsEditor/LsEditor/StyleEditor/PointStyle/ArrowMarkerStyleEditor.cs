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
    public partial class ArrowMarkerStyleEditor : UserControl,IStyleEditor 
    {
        public ArrowMarkerStyleEditor()
        {
            InitializeComponent();
        }



        #region IStyleEditor 成员
        private ISymbol m_pEditedStyle;
        private bool m_shouldAction = true;

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
                return "Arrow Marker Style  | 箭头点样式";
            }
        }

        public void CreateInitializeStyle()
        {
            this.EditedStyle = new ArrowMarkerSymbolClass();
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        public IArrowMarkerSymbol MarkerStyle
        {
            get
            {
                return this.EditedStyle as IArrowMarkerSymbol;
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
                else
                {
                    this.m_pEditedStyle = value;
                    this.DispatchStyle();
                }
            }
        }

        public bool CanEdit(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            try
            {
                throw new Exception("The method or operation is not implemented.");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("该符号已被锁定不能编辑", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false; }
        }
        private void DispatchStyle()
        {
            if (this.MarkerStyle != null)
            {
                this.m_shouldAction = false;
                try
                {
                    this.spinLength.Value = (decimal)this.MarkerStyle.Length;
                    this.spinWidth.Value = (decimal)this.MarkerStyle.Width;
                    this.spinOffsetX.Value = (decimal)this.MarkerStyle.XOffset;
                    this.spinOffsetY.Value = (decimal)this.MarkerStyle.YOffset;
                    this.spinAngle.Value = (decimal)this.MarkerStyle.Angle;
                    this.ceColor.Color = ColorHelper.CreateColor(this.MarkerStyle.Color);
                }
                catch (Exception ex) { }
                this.m_shouldAction = true;
            }
        }
        
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;

        #endregion

        private void OnStylePropertiesChanged(object sender, EventArgs e)
        {
            if (this.m_shouldAction && this.MarkerStyle != null)
            {
                try
                {
                    this.MarkerStyle.Length = (double)this.spinLength.Value;
                    this.MarkerStyle.Width = (double)this.spinWidth.Value;
                    this.MarkerStyle.XOffset = (double)this.spinOffsetX.Value;
                    this.MarkerStyle.YOffset = (double)this.spinOffsetY.Value;
                    this.MarkerStyle.Color = ColorHelper.CreateColor(this.ceColor.Color);
                    this.MarkerStyle.Angle = (double)this.spinAngle.Value;
                }
                catch (Exception ex) { }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());
            }
        }
    }
}
