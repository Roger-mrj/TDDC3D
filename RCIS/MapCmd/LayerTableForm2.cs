using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

using RCIS.Utility;
using RCIS.GISCommon;
namespace RCIS.MapTool
{
    public partial class LayerTableForm2 : Form
    {
        public LayerTableForm2()
        {
            InitializeComponent();
            
        }

        

        public IFeatureLayer FeatureLayer = null;
        private DataTable m_dataTable = new DataTable();

        public IMapControl3 mapCtrol3 = null;

        public IEnvelope Env = null;

        public void ShowGridData()
        {//显示数据
            if (this.FeatureLayer == null)
                return;
            DevExpress.Utils.WaitDialogForm wait = new DevExpress.Utils.WaitDialogForm("正在加载...", "请稍等，正在加载数据");
            wait.Show();

            try
            {
                wait.SetCaption("正在构造字段...");

                this.m_dataTable.Columns.Clear();
                this.m_dataTable.Rows.Clear();

                IFeatureClass curClass = this.FeatureLayer.FeatureClass;
                int fldCount = curClass.Fields.FieldCount;

                #region 构造字段
                string oidfldName = curClass.OIDFieldName;
                for (int fi = 0; fi < fldCount; fi++)
                {
                    IField curFld = curClass.Fields.get_Field(fi);
                    string fldName = curFld.Name.ToUpper();
                    DataColumn dc = new DataColumn(curFld.Name);

                    if (fldName.Equals(curClass.ShapeFieldName.ToUpper()))
                    {
                        dc.Caption = "几何图形";
                    }
                    else
                        if (curClass.LengthField != null && fldName.Equals(curClass.LengthField.Name.ToUpper()))
                        {
                            dc.Caption = "计算长度";
                        }
                        else
                            if (curClass.AreaField != null && fldName.Equals(curClass.AreaField.Name.ToUpper()))
                            {
                                dc.Caption = "计算面积";
                            }
                            else
                            {
                                dc.Caption = curFld.AliasName;
                            }
                    if (curFld.Type == esriFieldType.esriFieldTypeInteger
                        || curFld.Type == esriFieldType.esriFieldTypeSmallInteger
                        || curFld.Type == esriFieldType.esriFieldTypeOID)
                    {
                        dc.DataType = typeof(Int32);
                    }
                    else if (curFld.Type == esriFieldType.esriFieldTypeSingle
                        || curFld.Type == esriFieldType.esriFieldTypeDouble)
                    {
                        dc.DataType = typeof(Double);
                    }
                    else dc.DataType = typeof(string);
                    this.m_dataTable.Columns.Add(dc);
                }

                //主键
                this.m_dataTable.PrimaryKey=new DataColumn[] {  m_dataTable.Columns[oidfldName] };


                #endregion

                ISpatialFilter pSF = new SpatialFilterClass();
                pSF.Geometry = Env as IGeometry;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                int objCount = curClass.FeatureCount(pSF as IQueryFilter);
          
                string dispFld = this.FeatureLayer.DisplayField;
                int dispFldIndex = curClass.Fields.FindField(dispFld);
                if (dispFldIndex < 0)
                {
                    dispFld = curClass.Fields.get_Field(0).Name;
                    dispFldIndex = 0;
                }
                #region 获取数据
                wait.SetCaption("正在获取数据...");

                this.groupControl1.Text = "共" + objCount + "条数据";
                if (objCount > 0)
                {
                    
                    IFeatureCursor pCursor = curClass.Search(pSF as IQueryFilter, true);
                    IFeature pRow = pCursor.NextFeature();

                    int iCount = 0;
                    try
                    {
                        while (pRow != null)
                        {
                            object dispObj = pRow.get_Value(dispFldIndex);
                            if (dispObj == null) dispObj = "";
                            DataRow curRow = this.m_dataTable.NewRow();
                            for (int fi = 0; fi < fldCount; fi++)
                            {
                                IField curFld = curClass.Fields.get_Field(fi);
                                if (curFld.Type == esriFieldType.esriFieldTypeGeometry)
                                {
                                    IGeometry curGeom = (pRow as IFeature).Shape; // qFea.Shape;
                                    if (curGeom != null && !curGeom.IsEmpty)
                                    {
                                        curRow[fi] = "图形对象";
                                    }
                                    else
                                    {
                                        curRow[fi] = "空图形";
                                    }
                                }
                                else
                                {
                                    object oValue = pRow.get_Value(fi);
                                    if (curFld.Type == esriFieldType.esriFieldTypeSingle || curFld.Type == esriFieldType.esriFieldTypeDouble)
                                    {
                                        double dVal = 0.0;
                                        double.TryParse(oValue.ToString(), out dVal);
                                        curRow[fi] = dVal;
                                    }
                                    else
                                    {
                                        curRow[fi] = oValue;
                                    }
                                }
                            }

                            this.m_dataTable.Rows.Add(curRow);

                            pRow = pCursor.NextFeature();
                            iCount++;
                            if (iCount % 5000 == 0)
                            {
                                wait.SetCaption("已经能够加载" + iCount + "条数据...");
                            }
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {

                        RCIS.Utility.OtherHelper.ReleaseComObject(pCursor);
                        RCIS.Utility.OtherHelper.ReleaseComObject(pSF);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                }
                #endregion

                wait.Close();
                this.gridControl.DataSource = this.m_dataTable;

            }
            catch (Exception cex)
            {
                if (wait != null)
                    wait.Close();
            }

        }


        private void LayerTableForm2_Load(object sender, EventArgs e)
        {

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
           // OtherHelper.ReleaseComObject(pCursor);
            return null;
        }
        private void DisplayLayerExtentMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls|HTML files (*.html)|*.html";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;            
            saveFileDialog.Title = "导出Excel文件到";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog.FilterIndex == 1)
                {
                    this.gridControl.ExportToXls(saveFileDialog.FileName);
                }
                else if (saveFileDialog.FilterIndex == 2)
                {
                    this.gridControl.ExportToHtml(saveFileDialog.FileName);
                }
                MessageBox.Show("导出完毕","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void 选中图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null)
            {
                IFeatureSelection pSelection = this.FeatureLayer as IFeatureSelection;
                pSelection.Clear();
                string fldName = this.gridView1.Columns[0].FieldName.ToUpper();
                IFeatureClass pFC = this.FeatureLayer.FeatureClass;
                for (int i = 0; i < this.gridView1.SelectedRowsCount; i++)
                {
                    int idx=this.gridView1.GetSelectedRows()[i];
                    string sBSM = this.gridView1.GetRowCellValue(idx, fldName).ToString();
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

                this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
                this.mapCtrol3.ActiveView.ScreenDisplay.UpdateWindow();

                this.groupControl1.Text = "共" + this.m_dataTable.Rows.Count + "数据，选中" + pSelection.SelectionSet.Count + "条";
            }
        }

        private void 定位图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null)
            {
                if (this.gridView1.SelectedRowsCount == 0)
                    return;
                //找到第一列
                string fldName = this.gridView1.Columns[0].FieldName.ToUpper();
                string sBSM = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, fldName).ToString();
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

        private void 清除选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FeatureLayer != null)
            {
                IFeatureSelection pSelection = this.FeatureLayer as IFeatureSelection;
                pSelection.Clear();

            }
            this.groupControl1.Text = "共" + this.m_dataTable.Rows.Count + "数据，选中0条";
            this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
        }

        private void 删除选中数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RCIS.Global.GlobalEditObject.CurrentEngineEditor.EditState == esriEngineEditState.esriEngineStateNotEditing)
            {
                MessageBox.Show("当前必须处于编辑状态！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.gridView1.SelectedRowsCount == 0) return;

            if (this.gridView1.SelectedRowsCount > 50)
            {
                MessageBox.Show("批量删除的太多了，请谨慎操作，减少范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string fldName = this.gridView1.Columns[0].FieldName.ToUpper();
                IFeatureClass pFC = this.FeatureLayer.FeatureClass;

                string where = fldName + " in (";
                for (int i = 0; i < this.gridView1.SelectedRowsCount; i++)
                {
                    int idx = this.gridView1.GetSelectedRows()[i];
                    string sBSM = this.gridView1.GetRowCellValue(idx, fldName).ToString();

                    where += sBSM + ",";
                }
                if (where.EndsWith(","))
                    where = where.Remove(where.Length - 1, 1);
                where += " ) ";

                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = where;
                ITable pTable = pFC as ITable;
                pTable.DeleteSearchedRows(pQueryFilter);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pQueryFilter);

                this.mapCtrol3.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
                this.gridView1.DeleteSelectedRows();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowGridData();
        }
    }
}
