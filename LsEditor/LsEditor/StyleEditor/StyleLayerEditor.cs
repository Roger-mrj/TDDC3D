using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS .Carto ;
using ESRI.ArcGIS.Display ;

namespace RCIS.Style.StyleEditor
{
    public partial class StyleLayerEditor : UserControl
    {
        
        public StyleLayerEditor()
        {
            InitializeComponent();
            this.CreateContextMenu();
            
            this.m_imgVisible = FileHelper.LoadImage("可见.gif");
            this.m_imgUnvisible = FileHelper.LoadImage("不可见.gif");
            this.m_imgLock = FileHelper.LoadImage("锁定.gif");
            this.m_imgUnlock = FileHelper.LoadImage("开锁.gif");
        }
        private Image m_imgVisible;
        private Image m_imgUnvisible;
        private Image m_imgLock;
        private Image m_imgUnlock;
        private ContextMenu m_ctxMenu;
        private MenuItem m_miNew;
        private MenuItem m_miCopy;        
        private MenuItem m_miPaste;
        private MenuItem m_miDelete;
        private MenuItem m_miMoveUp;
        private MenuItem m_miMoveDown;
        private MenuItem m_miMoveToTop;
        private MenuItem m_miMoveToBottom;
        private void CreateContextMenu()
        {
            this.m_ctxMenu = new ContextMenu();
            this.m_miNew = new MenuItem("新建");
            this.m_miCopy = new MenuItem("复制");
            this.m_miPaste = new MenuItem("粘贴");
            this.m_miDelete = new MenuItem("删除");
            this.m_miMoveUp = new MenuItem("上移");
            this.m_miMoveDown = new MenuItem("下移");
            this.m_miMoveToTop = new MenuItem("置顶");
            this.m_miMoveToBottom = new MenuItem("置底");

            this.m_ctxMenu.MenuItems.Add(this.m_miNew);
            this.m_ctxMenu.MenuItems.Add(this.m_miCopy);
            this.m_ctxMenu.MenuItems.Add(this.m_miPaste);
            this.m_ctxMenu.MenuItems.Add(this.m_miDelete);
            this.m_ctxMenu.MenuItems.Add("-");
            this.m_ctxMenu.MenuItems.Add(this.m_miMoveUp);
            this.m_ctxMenu.MenuItems.Add(this.m_miMoveDown);
            this.m_ctxMenu.MenuItems.Add(this.m_miMoveToTop);
            this.m_ctxMenu.MenuItems.Add(this.m_miMoveToBottom);

            this.m_ctxMenu.Popup += new EventHandler(OnCtxPopup);
            this.m_miNew.Click += new EventHandler(OnNewLayer);
            this.m_miCopy.Click += new EventHandler(OnCopyLayer);
            this.m_miPaste.Click += new EventHandler(OnPasteLayer);
            this.m_miDelete.Click += new EventHandler(OnDeleteLayer);

            this.m_miMoveUp.Click += new EventHandler(OnMoveUp);
            this.m_miMoveDown.Click += new EventHandler(OnMoveDown);
            this.m_miMoveToTop.Click += new EventHandler(OnMoveToTop);
            this.m_miMoveToBottom.Click += new EventHandler(OnMoveToBottom);
        }

        void OnCtxPopup(object sender, EventArgs e)
        {
            this.m_miPaste.Enabled = this.m_pCopiedLayer != null;
            bool hasCurLayer = this.CurrentStyleLayer != null;
            this.m_miCopy.Enabled = hasCurLayer;
            this.m_miDelete.Enabled = hasCurLayer;
            this.m_miMoveDown.Enabled = hasCurLayer;
            this.m_miMoveUp.Enabled = hasCurLayer;

            this.m_miMoveToBottom.Enabled = hasCurLayer;
            this.m_miMoveToTop.Enabled = hasCurLayer;
            #region 图层移动还有更多控制
            if (hasCurLayer)
            {
                StyleLayer aCurLayer=this.SelectedStyleLayer ;
                int aCurIndex = this.m_styleList.IndexOf(aCurLayer);
                if (aCurIndex == 0)
                {
                    this.m_miMoveToTop.Enabled = false;
                    this.m_miMoveUp.Enabled = false;
                }
                if (aCurIndex + 1 == this.m_styleList.Count)
                {
                    this.m_miMoveToBottom.Enabled = false;
                    this.m_miMoveDown.Enabled = false;
                }
            }
            #endregion
            #region 最后一个图层不能删除
            if (hasCurLayer)
            {
                if (this.m_styleList.Count <= 1)
                {
                    this.m_miDelete.Enabled = false;
                }
            }
            #endregion
        }
        #region 图层移动
        void OnMoveToBottom(object sender, EventArgs e)
        {
            StyleLayer aLayer = this.SelectedStyleLayer;
            int aCurIndex = this.m_styleList.IndexOf(aLayer);
            this.MoveLayerTo(aCurIndex, this.m_styleList .Count-1);
        }

        void OnMoveToTop(object sender, EventArgs e)
        {
            StyleLayer aLayer = this.SelectedStyleLayer;
            int aCurIndex = this.m_styleList.IndexOf(aLayer);
            this.MoveLayerTo(aCurIndex, 0);
        }

        void OnMoveDown(object sender, EventArgs e)
        {
            StyleLayer aLayer=this.SelectedStyleLayer ;
            int aCurIndex = this.m_styleList.IndexOf(aLayer);
            this.MoveLayerTo(aCurIndex, aCurIndex + 1);
        }
        private DataRow CloneRow(DataRow pRow)
        {
            DataRow rRow = this.m_styleTable.NewRow();
            int icount = this.m_styleTable.Columns.Count;
            for (int ic = 0; ic < icount; ic++)
            {
                rRow[ic] = pRow[ic];
            }
            return rRow;
        }
        private void MoveLayerTo(int pFromIndex, int pToIndex)
        {
            try
            {
                StyleLayer aLayer = this.m_styleList[pFromIndex];
                DataRow aRow = this.m_styleTable.Rows[pFromIndex];
                aRow = this.CloneRow(aRow);
                
                this.m_styleList.RemoveAt(pFromIndex);
                this.m_styleTable.Rows.RemoveAt(pFromIndex);

                if (pToIndex < 0) pToIndex = 0;
                if (pToIndex > this.m_styleList.Count)
                    pToIndex = this.m_styleList.Count;

                this.m_styleList.Insert(pToIndex, aLayer);
                this.m_styleTable.Rows.InsertAt(aRow, pToIndex);
                this.GenerateResultEditStyle();
            }
            catch (Exception ex) { }
        }
        void OnMoveUp(object sender, EventArgs e)
        {
            StyleLayer aLayer = this.SelectedStyleLayer;
            int aCurIndex = this.m_styleList.IndexOf(aLayer);
            this.MoveLayerTo(aCurIndex, aCurIndex - 1);
        }
        #endregion
        #region 图层新建复制粘贴删除
        void OnDeleteLayer(object sender, EventArgs e)
        {
            int aCount = this.layerGrid.SelectedRows.Count;
            if (aCount >= 0)
            {
                DataGridViewRow aSelRow = this.layerGrid.SelectedRows[0];
                int iRowIndex = this.layerGrid.Rows.IndexOf(aSelRow);
                this.m_styleTable.Rows.RemoveAt(iRowIndex);                
                this.m_styleList.RemoveAt(iRowIndex);
                this.GenerateResultEditStyle();
                MessageBox.Show("图层已删除!", "符号图层管理"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请首先选择一个图层!", "符号图层管理"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void OnPasteLayer(object sender, EventArgs e)
        {
            if (this.m_pCopiedLayer != null)
            {
                this.AppendStyleLayer(this.m_pCopiedLayer);
                this.m_pCopiedLayer = null;
            }
        }


        private StyleLayer m_pCopiedLayer = null;
        void OnCopyLayer(object sender, EventArgs e)
        {
            int aCount=this.layerGrid .SelectedRows  .Count ;
            if (aCount >= 0)
            {
                DataGridViewRow aSelRow = this.layerGrid.SelectedRows[0];
                int iRowIndex = this.layerGrid.Rows.IndexOf(aSelRow);
                StyleLayer aSelLayer = this.m_styleList[iRowIndex];
                this.m_pCopiedLayer = (aSelLayer as ICloneable ).Clone () 
                    as StyleLayer ;
                MessageBox.Show("图层已复制!", "符号图层管理"
                    ,MessageBoxButtons .OK, MessageBoxIcon.Information );
            }
            else
            {
                MessageBox.Show("请首先选择一个图层!", "符号图层管理"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

        void OnNewLayer(object sender, EventArgs e)
        {
            string aStyleClass = StyleClass.GetStyleClass(this.EditedStyle);
            ISymbol aNewSym = StyleClass.CreateDefaultStyle(aStyleClass);
            this.AppendStyleLayer(aNewSym);
            this.GenerateResultEditStyle();
            int aRowIndex = this.layerGrid.Rows.Count - 1;
            DataGridViewRow aNewRow = this.layerGrid.Rows[aRowIndex];
            aNewRow.Selected = true;
        }
        #endregion
        /// <summary>
        /// the EditedStyle is the whole style.
        /// since this is a layer editor .in the following a style
        /// just mean a layer in the whole style.
        /// </summary>
        private ISymbol m_pEditedSymbol;
        private ISymbol m_pResultSymbol;
        public ISymbol EditedStyle
        {
            get
            {
                return this.m_pEditedSymbol;
            }
            set
            {                
                this.m_pEditedSymbol = value;
                this.DispatchEditedStyle(this.m_pEditedSymbol );
                if (this.layerGrid.Rows.Count > 0)
                {
                    DataGridViewRow aSelRow = this.layerGrid.Rows[0];
                    aSelRow.Selected = true;
                }
                this.m_pCopiedLayer = null;
            }
        }
        /// <summary>
        /// This Symbol is the real result style.
        /// </summary>
        public ISymbol ResultStyle
        {
            get
            {
                return this.m_pResultSymbol;
            }
        }
        /// <summary>
        /// This method dispatch a whole style to a list of layers.
        /// </summary>
        private void DispatchEditedStyle(ISymbol pSymbol)
        {
            this.m_pEditedSymbol = pSymbol;
            this.CreateStyleTable();
            if (this.EditedStyle != null)
            {
                if (this.EditedStyle is IMarkerSymbol)
                {
                    #region Process MarkerSymbol
                    IMultiLayerMarkerSymbol aSym = this.EditedStyle as IMultiLayerMarkerSymbol;
                    if (aSym == null)
                    {
                        aSym = new MultiLayerMarkerSymbolClass();
                        aSym.AddLayer(this.EditedStyle as IMarkerSymbol);
                        this.DispatchEditedStyle(aSym as ISymbol);
                    }
                    else
                    {
                        int aLayerCount = aSym.LayerCount;
                        for (int i = 0; i < aLayerCount; i++)
                        {
                            ISymbol aLayer = aSym.get_Layer(i) as ISymbol;
                            this.AppendStyleLayer(aLayer);
                        }
                    }
                    #endregion
                }
                else if (this.EditedStyle is ILineSymbol)
                {
                    #region ProcessLineSymbol
                    IMultiLayerLineSymbol aSym = this.EditedStyle as IMultiLayerLineSymbol;
                    if (aSym == null)
                    {
                        aSym = new MultiLayerLineSymbolClass();
                        aSym.AddLayer(this.EditedStyle as ILineSymbol);
                        this.DispatchEditedStyle(aSym as ISymbol);                     
                    }
                    else
                    {
                        int lc = aSym.LayerCount;
                        for (int i = 0; i < lc; i++)
                        {
                            ISymbol aLayer = aSym.get_Layer(i) as ISymbol;
                            this.AppendStyleLayer(aLayer);
                        }
                    }
                    #endregion
                }
                else if (this.EditedStyle is IFillSymbol)
                {
                    #region ProcessFillSymbol
                    IMultiLayerFillSymbol aSym = this.EditedStyle as IMultiLayerFillSymbol;
                    if (aSym == null)
                    {
                        aSym = new MultiLayerFillSymbolClass();
                        aSym.AddLayer(this.EditedStyle as IFillSymbol);
                        this.DispatchEditedStyle(aSym as ISymbol);
                    }
                    else
                    {
                        int lc = aSym.LayerCount;
                        for (int i = 0; i < lc; i++)
                        {
                            ISymbol aLayer = aSym.get_Layer(i) as ISymbol;
                            this.AppendStyleLayer(aLayer);
                        }
                    }
                    #endregion
                }
                else if (this.EditedStyle is ITextSymbol)
                {
                    this.AppendStyleLayer(this.EditedStyle as ISymbol);
                }
            }
        }
        /// <summary>
        /// This method will create a style from a list of layers.
        /// This style will be the result to others.
        /// </summary>
        private void GenerateResultEditStyle()
        {
            List<ISymbol> aSymList = new List<ISymbol>();
            foreach (StyleLayer  aLayer in this.m_styleList )
            {                
                if (aLayer.Visible)
                {
                    aSymList.Add(aLayer.Style);
                }
            }
            aSymList.Reverse();
            if (this.EditedStyle is IMultiLayerMarkerSymbol)
            {
                this.m_pResultSymbol = new MultiLayerMarkerSymbolClass();
                foreach (IMarkerSymbol aLayer in aSymList)
                {
                    if (aLayer != null)
                    {
                        (m_pResultSymbol as IMultiLayerMarkerSymbol)
                            .AddLayer(aLayer);
                    }
                }
            }
            else if (this.EditedStyle is IMultiLayerLineSymbol)
            {
                this.m_pResultSymbol = new MultiLayerLineSymbolClass();
                foreach (ILineSymbol aLayer in aSymList)
                {
                    if (aLayer != null)
                    {
                        (m_pResultSymbol as IMultiLayerLineSymbol)
                            .AddLayer(aLayer);
                    }
                }
            }
            else if (this.EditedStyle is IMultiLayerFillSymbol)
            {
                this.m_pResultSymbol = new MultiLayerFillSymbolClass();
                foreach (IFillSymbol aLayer in aSymList)
                {
                    if (aLayer != null)
                    {
                        (m_pResultSymbol as IMultiLayerFillSymbol)
                            .AddLayer(aLayer);
                    }
                }
            }
            this.TriggerEditedStyleChangedEvent();
        }
        /// <summary>
        /// just append a new layer with pSymbol.
        /// </summary>
        /// <param name="pSymbol"></param>
        private void AppendStyleLayer(ISymbol pSymbol)
        {
            if (pSymbol != null)
            {
                StyleLayer aLayer = new StyleLayer();
                aLayer.Style = pSymbol;
                this.AppendStyleLayer(aLayer);
            }
        }
        private void AppendStyleLayer(StyleLayer pLayer)
        {
            DataRow aRow = this.m_styleTable.NewRow();
            if (pLayer.Visible)
            {
                aRow[0] = this.m_imgVisible;
            }
            else
            {
                aRow[0] = this.m_imgUnvisible;
            }
            aRow[1] = SymbolHelper.StyleToImage(pLayer.Style, 96, 48);
            if (pLayer.Lock)
            {
                aRow[2] = this.m_imgLock;
            }
            else
            {
                aRow[2] = this.m_imgUnlock;
            }
            //your must add a Layer to m_styleList first.
            //because when you add aRow into the m_styleTable
            //it will trigger event automaticlly
            this.m_styleList.Add(pLayer);
            this.m_styleTable.Rows.Add(aRow);
        }
        /// <summary>
        /// This function was used to update the current style.
        /// since this is a multilayer style editor,that's means
        /// update the selected layer's style.and that will update the
        /// whole style.
        /// </summary>
        /// <param name="pSymbol"></param>
        public void UpdateStyleLayer(ISymbol pSymbol)
        {
            DataGridViewRow aSelRow = this.layerGrid.SelectedRows[0];            
            int aRowIndex = aSelRow.Index;
            StyleLayer aLayer = this.m_styleList[aRowIndex];
            aLayer.Style = pSymbol;
            DataRow aDataRow = this.m_styleTable.Rows[aRowIndex];
            aDataRow[1] = SymbolHelper.StyleToImage(pSymbol, 96, 48);
            this.GenerateResultEditStyle();
        }
        public ISymbol CurrentStyleLayer
        {
            get
            {
                if (this.layerGrid.SelectedRows.Count > 0)
                {
                    DataGridViewRow aSelRow = this.layerGrid.SelectedRows[0];
                    int aRowIndex = aSelRow.Index;
                    StyleLayer aLayer = this.m_styleList[aRowIndex];
                    return aLayer.Style;
                }
                return null;
            }
        }
        private StyleLayer SelectedStyleLayer
        {
            get
            {
                if (this.layerGrid.SelectedRows.Count > 0)
                {
                    DataGridViewRow aSelRow = this.layerGrid.SelectedRows[0];
                    int aRowIndex = aSelRow.Index;
                    StyleLayer aLayer = this.m_styleList[aRowIndex];
                    return aLayer;
                }
                return null;
            }
        }
        private DataTable m_styleTable;
        private List<StyleLayer> m_styleList;
        private void CreateStyleTable()
        {
            this.m_styleList = new List<StyleLayer>();

            this.m_styleTable = new DataTable();
            DataColumn dc = new DataColumn("可见");
            dc.DataType = typeof(Image);
            this.m_styleTable.Columns.Add(dc);

            dc = new DataColumn("预览");
            dc.DataType = typeof(Image);
            this.m_styleTable.Columns.Add(dc);

            dc = new DataColumn("锁定");
            dc.DataType = typeof(Image);
            this.m_styleTable.Columns.Add(dc);

            this.layerGrid.DataSource = this.m_styleTable;

        }
                      
        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;
            DataRow aSelRow = this.m_styleTable.Rows[e.RowIndex];
            StyleLayer aSelLayer = this.m_styleList[e.RowIndex];
            if (e.ColumnIndex == 0)
            {
                aSelLayer.Visible = !aSelLayer.Visible;
                if (aSelLayer.Visible) aSelRow[0] = this.m_imgVisible;
                else aSelRow[0] = this.m_imgUnvisible;
                this.GenerateResultEditStyle();
            }
            else if (e.ColumnIndex == 2)
            {
                aSelLayer.Lock = !aSelLayer.Lock;
                if (aSelLayer.Lock) aSelRow[2] = this.m_imgLock;
                else aSelRow[2] = this.m_imgUnlock;
            }
        }
        public event EditedStyleChangedEventHandler OnEditedStyleChanged;
        public event EditedStyleLayerChangedEventHandler OnCurrentEditedLayerChanged;
        private void TriggerEditedStyleChangedEvent()
        {
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }
        private void TriggerSelectedEditedLayerChanged()
        {
            if(this.OnCurrentEditedLayerChanged !=null)
                this.OnCurrentEditedLayerChanged (this,new EventArgs());
        }

        private void layerGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point aLoc = new Point(e.X, e.Y);
                this.m_ctxMenu.Show(this.layerGrid,aLoc );
            }
        }

        private void layerGrid_SelectionChanged(object sender, EventArgs e)
        {
            this.TriggerSelectedEditedLayerChanged();
        }
    }
}
