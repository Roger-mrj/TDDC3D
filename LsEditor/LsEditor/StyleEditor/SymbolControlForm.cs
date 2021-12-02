using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
namespace RCIS.Style.StyleEditor
{
    public partial class SymbolControlForm : DevExpress.XtraEditors.XtraForm
    {
        public SymbolControlForm()
        {
            InitializeComponent();
            this.radioGroup1.SelectedIndex = 0;
        }
        private esriSymbologyStyleClass m_pStyleClassType = esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
        public esriSymbologyStyleClass StyleClassType
        {
            get { return m_pStyleClassType; }
            set { m_pStyleClassType = value; }
        }
        private void SymbolControlForm_Load(object sender, EventArgs e)
        {
            string sFilePath = FileHelper.FindFolder("Style");
            this.axSymbologyControl1.LoadStyleFile(sFilePath + "\\DiXingTu.ServerStyle");

            this.axSymbologyControl1.Appearance = esriControlsAppearance.esri3D;
            this.axSymbologyControl1.DisplayStyle = esriSymbologyDisplayStyle.esriDisplayStyleIcon;
            this.axSymbologyControl1.ShowContextMenu = true;
            DisplayStyleType();
        }
        private  IStyleGalleryItem m_styleGalleryItem;

        private void axSymbologyControl1_OnItemSelected(object sender, ESRI.ArcGIS.Controls.ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            //Preview the selected item
            m_styleGalleryItem = (IStyleGalleryItem)e.styleGalleryItem;
            PreviewImage();
        }

        private void PreviewImage()
        {
            //Get and set the style class 
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
            //Preview an image of the symbol
            stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(m_styleGalleryItem, this.pictureEdit1.Width, pictureEdit1.Height);
            System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            this.pictureEdit1.Image = image;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
         
            this.m_styleGalleryItem = null;
            this.Close();
          
        }

        private void cmdMore_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = FileHelper.FindFolder("Style");
            ofd.Filter = "符号文件 (*.ServerStyle)|*.ServerStyle";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sFileName = ofd.FileName;
                this.axSymbologyControl1.Clear();
                this.axSymbologyControl1.LoadStyleFile(sFileName);

                this.axSymbologyControl1.Appearance = esriControlsAppearance.esri3D;
                this.axSymbologyControl1.DisplayStyle = esriSymbologyDisplayStyle.esriDisplayStyleIcon;
                this.axSymbologyControl1.ShowContextMenu = true;
                DisplayStyleType();
            }
        }
        public ISymbol m_pSymbol = null;
        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.m_pSymbol = this.m_styleGalleryItem.Item as ISymbol;
            this.Close();
        }
        private void DisplayStyleType()
        {
            if (m_pStyleClassType == esriSymbologyStyleClass.esriStyleClassFillSymbols)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassFillSymbols;
                this.radioGroup2.SelectedIndex = 2;
            }
            else if (m_pStyleClassType == esriSymbologyStyleClass.esriStyleClassLineSymbols)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassLineSymbols;
                this.radioGroup2.SelectedIndex = 1;
            }
            else if (m_pStyleClassType == esriSymbologyStyleClass.esriStyleClassMarkerSymbols)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
                this.radioGroup2.SelectedIndex = 0;
            }
            else if (m_pStyleClassType == esriSymbologyStyleClass.esriStyleClassTextSymbols)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassTextSymbols;
                this.radioGroup2.SelectedIndex = 3;
            }
        }
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (this.radioGroup1.SelectedIndex == 0)
            {
                this.axSymbologyControl1.DisplayStyle = esriSymbologyDisplayStyle.esriDisplayStyleIcon;
            }
            else if (this.radioGroup1.SelectedIndex == 1)
            {
                this.axSymbologyControl1.DisplayStyle = esriSymbologyDisplayStyle.esriDisplayStyleSmallIcon;
            }
            else if (this.radioGroup1.SelectedIndex == 2)
            {
                this.axSymbologyControl1.DisplayStyle = esriSymbologyDisplayStyle.esriDisplayStyleReport;
            }
            else if (this.radioGroup1.SelectedIndex == 3)
            {
                this.axSymbologyControl1.DisplayStyle = esriSymbologyDisplayStyle.esriDisplayStyleList;
            }
            
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup2.SelectedIndex==0)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
            }
            else if (this.radioGroup2.SelectedIndex == 1)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassLineSymbols;
            }
            else if (this.radioGroup2.SelectedIndex == 2)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassFillSymbols;
            }
            else if (this.radioGroup2.SelectedIndex == 3)
            {
                this.axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassTextSymbols;
            }
            this.pictureEdit1.Image = null;
        }
    }
}