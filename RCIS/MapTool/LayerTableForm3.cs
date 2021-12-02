using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ArcDataBinding;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Geometry;

namespace RCIS.MapTool
{
    public partial class LayerTableForm3 : Form
    {
        public LayerTableForm3()
        {
            InitializeComponent();
        }

        private ArcDataBinding.TableWrapper tableWrapper;
        public IMapControl3 mapCtrol3 = null;
        public IFeatureLayer FeatureLayer = null;


        private Dictionary<string, string> getFldAlias(IFeatureClass pFC )
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < pFC.Fields.FieldCount; i++)
            {
                IField pFld = pFC.Fields.get_Field(i);
                dic.Add(pFld.Name, pFld.AliasName);
            }
            return dic;
        }

        private void SetGridTitle(Dictionary<string, string> dic)
        {
            for (int i=0;i<this.dataGridView1.Columns.Count;i++)
            {
                if (dic.ContainsKey(this.dataGridView1.Columns[i].Name))
                {
                    this.dataGridView1.Columns[i].HeaderText = dic[this.dataGridView1.Columns[i].Name].ToString();
                    this.dataGridView1.Columns[i].SortMode= DataGridViewColumnSortMode.Programmatic;
                }
            }
        }

        public void RefreshData()
        {
            if (this.FeatureLayer == null)
                return;
            IFeatureClass pFC = this.FeatureLayer.FeatureClass;
            if (pFC == null) return;
            ITable foundITable = pFC as ITable;

            // Bind dataset to the binding source
            tableWrapper = new ArcDataBinding.TableWrapper(foundITable);
            bindingSource1.DataSource = tableWrapper;

            // Bind binding source to grid. Alternatively it is possible to bind TableWrapper
            // directly to the grid to this offers less flexibility
            dataGridView1.DataSource = bindingSource1;

            
            // Bind binding source to text box, we are binding the NAME
            // field.

            textBox1.Text = pFC.AliasName;
            Dictionary<string, string> dicTitle = getFldAlias(pFC);
            SetGridTitle(dicTitle);
        }
        private void LayerTableForm3_Load(object sender, EventArgs e)
        {

        }


      
        private void 定位图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null)
            {
                if (this.dataGridView1.SelectedRows.Count == 0)
                    return;
                //找到第一列
                string fldName = this.dataGridView1.Columns[0].Name.ToUpper().Trim();
                string sBSM = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                IFeatureClass pFC = this.FeatureLayer.FeatureClass;
                IFeature selFea = GetFeature(fldName, sBSM, pFC);
                if (selFea != null)
                {
                    IGeometry geom = selFea.Shape;
                    if (geom != null && !geom.IsEmpty)
                    {
                        IEnvelope env = geom.Envelope;
                        env.Expand(1.5, 1.5, true);
                        this.mapCtrol3.ActiveView.Extent = env;

                        this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapCtrol3.ActiveView.Extent);
                        this.mapCtrol3.ActiveView.ScreenDisplay.UpdateWindow();
                        this.mapCtrol3.FlashShape(geom, 3, 300, null);


                    }
                }
            }
        }
        private IFeature GetFeature(string fldName, string sValue, IFeatureClass pFC)
        {
            IQueryFilter qf = new QueryFilterClass();
            qf.WhereClause = fldName + " =" + sValue + "";
            IFeatureCursor pCursor = pFC.Search(qf, false);
            IFeature pFea = pCursor.NextFeature();

            if (pFea != null)
            {
                OtherHelper.ReleaseComObject(pCursor);
                return pFea;
            }
            OtherHelper.ReleaseComObject(qf);
            OtherHelper.ReleaseComObject(pCursor);
            return null;
        }

        private void 选中图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null)
            {
                IFeatureSelection pSelection = this.FeatureLayer as IFeatureSelection;
                pSelection.Clear();
                string fldName = this.dataGridView1.Columns[0].Name.ToUpper().Trim();
                IFeatureClass pFC = this.FeatureLayer.FeatureClass;
                for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
                {

                    string sBSM = this.dataGridView1.SelectedRows[i].Cells[0].Value.ToString();

                    IFeature selFea = GetFeature(fldName, sBSM, pFC);
                    if (selFea != null)
                    {
                        IGeometry geom = selFea.Shape;
                        if (geom != null && !geom.IsEmpty)
                        {
                            pSelection.Add(selFea);
                        }
                    }
                }

                this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapCtrol3.ActiveView.Extent);
                this.mapCtrol3.ActiveView.ScreenDisplay.UpdateWindow();

                this.Text = "共" + this.dataGridView1.Rows.Count + "数据，选中" + pSelection.SelectionSet.Count + "条";
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void 清楚选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null)
            {
                IFeatureSelection pSelection = this.FeatureLayer as IFeatureSelection;
                pSelection.Clear();

            }
            this.Text = "共" + this.dataGridView1.Rows.Count + "数据，选中0条";
            this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Programmatic)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    string columnBindingName = dgv.Columns[e.ColumnIndex].DataPropertyName;
                    switch (dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection)
                    {
                        case System.Windows.Forms.SortOrder.None:
                        case System.Windows.Forms.SortOrder.Ascending:
                            CustomSort(columnBindingName, false);
                            dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                            break;
                        case System.Windows.Forms.SortOrder.Descending:
                            CustomSort(columnBindingName, true);
                            dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                            break;
                    }
                    this.Cursor = Cursors.Default;
                }
                catch(Exception ex)
                { 
                    this.Cursor = Cursors.Default;
                }

            }       
        }

        private void CustomSort(string fld, bool asc)
        {
            IFeatureClass pFC = this.FeatureLayer.FeatureClass;
            if (pFC == null) return;
            ITable foundITable = pFC as ITable;
            ITableSort tsort = new TableSortClass();

            tsort.Table = foundITable;
            tsort.Fields = fld;
            tsort.set_Ascending(fld, asc);
            tsort.Sort(null);

            ItableSortToITable(tsort, ref tableWrapper);

        //    dataGridView1.Refresh();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(tsort);
        }

        private void ItableSortToITable(ITableSort tableSort, ref ArcDataBinding.TableWrapper twapper)
        {
            List<IRow> rows = new List<IRow>();
            ICursor cursor = tableSort.Rows;
            IRow row = null;
            twapper.Clear();
            while ((row = cursor.NextRow()) != null)
            {
                rows.Add(row);
                twapper.Add(row);
            }

            return;
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }

        private void 删除选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            IFeatureSelection pSelection = this.FeatureLayer as IFeatureSelection;
            if (pSelection.SelectionSet.Count == 0) return;

            IFeatureClass pFC=this.FeatureLayer.FeatureClass;

            IWorkspaceEdit pWSE = RCIS.Global.GlobalEditObject.GlobalWorkspace as IWorkspaceEdit;
            if (pWSE == null) return;
            bool isEdit = pWSE.IsBeingEdited();
            if (!isEdit)
            {
                pWSE.StartEditing(true);
                
            }
            pWSE.StartEditOperation();
            try
            {
                IEnumIDs pEnumIds= pSelection.SelectionSet.IDs;
                int id ;
                while ((id = pEnumIds.Next()) != -1)
                {
                    IFeature delFea = pFC.GetFeature(id);
                    delFea.Delete();
                }

               

                pWSE.StopEditOperation();
                if (!isEdit)
                {
                    pWSE.StopEditing(true);
                }
                RefreshData();
                this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, this.mapCtrol3.ActiveView.Extent);
                this.mapCtrol3.ActiveView.ScreenDisplay.UpdateWindow();

                this.Text = "共" + this.dataGridView1.Rows.Count + "数据，选中" + pSelection.SelectionSet.Count + "条";
            }
            catch (Exception ex)
            {
                pWSE.AbortEditOperation();
                if (!isEdit)
                {
                    pWSE.StopEditing(false);
                }
               
            }
            
                        
        }

       
    }
}
