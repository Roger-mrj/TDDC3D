using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using System.Text.RegularExpressions;
using System.Globalization;


using stdole;
namespace RCIS.Style.StyleEditor
{
    public partial class CharacterMarkerStyleEditor : UserControl,IStyleEditor
    {
        public CharacterMarkerStyleEditor()
        {
            InitializeComponent();

            
            
            FontFamily[] fmList=FontFamily.Families;
            foreach (FontFamily fm in fmList)
            {
                ComboBoxItem aItem = new ComboBoxItem(fm, fm.Name);
                this.ceFontFamily.Properties.Items.Add(aItem);
            }
            if (this.ceFontFamily.Properties.Items.Count > 0)
            {
                this.ceFontFamily.SelectedItem = this.ceFontFamily.Properties.Items[0];
            }
             this.mCurPage = 0;
             this.mPageSize=512;
             this.mMaxPage = Int16.MaxValue / this.mPageSize ;
        }
        #region ui
        private DataTable m_charTable = new DataTable();
        private void CreateDataTable()
        {
            this.m_charTable = new DataTable();
            for (int i = 0; i < 10; i++)
            {
                DataColumn dc = new DataColumn(i.ToString ());
                this.m_charTable.Columns.Add(dc);
            }
            this.gridChar.DataSource = null;
        }




        private int mCurPage = 0;
        private int mPageSize=512;
        private int mMaxPage = Int16.MaxValue / 512;
        private void AppendCharacterPage()
        {
            
            
            if (this.mCurPage <= this.mMaxPage)
            {
                int iCodeBase = (this.mCurPage - 1) * this.mPageSize;
                int iCols = this.m_charTable.Columns.Count;
                int iRows = this.mPageSize / iCols;
                for (int i = 0; i < iRows; i++)
                {
                    DataRow aRow = this.m_charTable.NewRow();
                    for (int j = 0; j < iCols; j++)
                    {
                        int aCharInt = iCodeBase+i * iCols + j;
                        if (aCharInt < Int16.MaxValue)
                        {
                            char aChar = (char)aCharInt;
                            aRow[j] = aChar;
                            
                        }
                    }
                    this.m_charTable.Rows.Add(aRow);
                    if (i % 500 == 0)
                        Application.DoEvents();
                }
            }
            if (this.mCurPage == this.mMaxPage)
            {
                this.btnLoadAll.Enabled = false;
                this.btnNextPage.Enabled = false;
            }
        }
        private void ShowCharacterTable()
        {
            this.btnNextPage.Enabled = true;
            this.btnLoadAll.Enabled = true;
            this.CreateDataTable();
            try
            {
                ComboBoxItem aSelItem = this.ceFontFamily.SelectedItem
                as ComboBoxItem;
                if (aSelItem == null) return;
                FontFamily aFamily = aSelItem.ItemObject as FontFamily;
                if (aFamily == null) return;
                System.Drawing.Font aFt = new System.Drawing.Font(aFamily, 20);

                this.gridChar.Font = aFt;
                this.gridChar.DefaultCellStyle.Font = aFt;
                this.mCurPage = 1;

                this.AppendCharacterPage();
                this.gridChar.DataSource = this.m_charTable;
            }
            catch (Exception ex) { }
        }
        #endregion

        #region IStyleEditor 成员

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
                return "Character Marker Style  | 字符点样式";
            }            
        }
        public void CreateInitializeStyle()
        {
            ICharacterMarkerSymbol aCharSym = new CharacterMarkerSymbolClass();
            StdFontClass aFt = new StdFontClass();
            aFt.Name = FontFamily .GenericMonospace .Name ;
            aFt.Size = 10;
            aCharSym.Font = (IFontDisp)aFt;
            aCharSym.CharacterIndex = 56;
            this.EditedStyle = aCharSym as ISymbol;

            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
           
        }

        private ISymbol m_pEditedStyle;
        public ICharacterMarkerSymbol MarkerSymbol
        {
            get
            {
                return this.m_pEditedStyle as ICharacterMarkerSymbol;
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
                else if(this.CanEdit (value))
                {
                    this.m_pEditedStyle = value;
                    this.DispatchStyle();
                }
            }
        }

        public bool CanEdit(ISymbol pSymbol)
        {            
            if (pSymbol is ICharacterMarkerSymbol) return true;
            return false;
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        private void DispatchStyle()
        {
            if (this.MarkerSymbol != null)
            {
                this.m_shouldAction = false;
                try
                {
                    string aName = this.MarkerSymbol.Font.Name.ToUpper();
                    int aCharInt = this.MarkerSymbol.CharacterIndex;
                    double aSize = this.MarkerSymbol.Size;
                    double aAngle = this.MarkerSymbol.Angle;
                    double aOffsetX = this.MarkerSymbol.XOffset;
                    double aOffsetY = this.MarkerSymbol.YOffset;
                    Color aColor = ColorHelper.CreateColor(this.MarkerSymbol.Color);
                    this.SelectFontFamily(aName);
                    this.teCharInt.Text = aCharInt.ToString();
                    this.spinSize.Value = (decimal)aSize;
                    this.spinAngle .Value= (decimal)aAngle;
                    this.spinOffsetX.Value = (decimal)aOffsetX;
                    this.spinOffsetY.Value = (decimal)aOffsetY;
                    this.ceColor.Color = aColor;
                    this.SelectCharacterCell ();
                }
                catch (Exception ex) { }
                this.m_shouldAction = true;
            }
        }
        private void SelectFontFamily(string pFamilyName)
        {
            foreach (ComboBoxItem aItem in this.ceFontFamily.Properties.Items)
            {
                if (aItem != null && aItem.ItemObject is FontFamily)
                {
                    FontFamily ff = aItem.ItemObject as FontFamily;
                    string fName = ff.Name.ToUpper();
                    if (fName.Equals(pFamilyName))
                    {
                        this.ceFontFamily.SelectedItem = aItem;
                        break;
                    }
                }
            }
        }
        private bool m_shouldAction = true;
        private void OnEditedStylePropertyChanged(object pSender, EventArgs pArg)
        {
            if (!this.m_shouldAction||this.EditedStyle ==null) return;
            if (this.MarkerSymbol != null)
            {
                ComboBoxItem aSelItem = this.ceFontFamily.SelectedItem as ComboBoxItem;
                if (aSelItem == null) return;
                FontFamily aFamily = aSelItem.ItemObject as FontFamily;
                if (aFamily == null) return;
                int aCharIndex = TextHelper.ParseInt(this.teCharInt.Text, 65);
                this.SelectCharacterCell ();
                
                StdFontClass aFt = new StdFontClass();
                aFt.Name = aFamily.Name;
                aFt.Size = 10;
                this.MarkerSymbol.Font = (IFontDisp)aFt;
                this.MarkerSymbol.CharacterIndex = aCharIndex;
                try
                {
                    this.MarkerSymbol.Size = (double)this.spinSize.Value;
                    this.MarkerSymbol.Angle = (double)this.spinAngle.Value;
                    this.MarkerSymbol.Color = ColorHelper.CreateColor(this.ceColor.Color);
                    this.MarkerSymbol.XOffset = (double)this.spinOffsetX.Value;
                    this.MarkerSymbol.YOffset = (double)this.spinOffsetY.Value;
                }
                catch (Exception ex) { }
                if (this.OnEditedStyleChanged != null)
                    this.OnEditedStyleChanged(this, new EventArgs());
            }
        }
        #endregion

        private void ceFontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.ShowCharacterTable();
        }
        private void SelectCharacterCell()
        {
            #region 设置选定的符号
            try
            {
                int aCharIndex = TextHelper.ParseInt(this.teCharInt.Text, 48);
                int iRowNo = aCharIndex / this.m_charTable.Columns.Count;
                int iColNo = aCharIndex % this.m_charTable.Columns.Count;
                DataGridViewRow aSelRow = this.gridChar.Rows[iRowNo];
                aSelRow.Cells[iColNo].Selected = true;
            }
            catch (Exception ex) { }
            #endregion
        }
        private void OnCharClicked(object sender, DataGridViewCellEventArgs e)
        {
            int iCols = this.m_charTable.Columns.Count;
            int aCharInt =e.RowIndex * iCols + e.ColumnIndex;
            string aCharStr = aCharInt.ToString();
            if (!this.teCharInt.Text.Equals(aCharStr))
            {
                this.teCharInt.Text = aCharInt.ToString();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            this.mCurPage++;
            AppendCharacterPage();
        }

        private void btnLoadAll_Click(object sender, EventArgs e)
        {
            while (this.mCurPage < this.mMaxPage)
            {
                this.mCurPage = 1;
                this.AppendCharacterPage();
            }
        }
    }
}
