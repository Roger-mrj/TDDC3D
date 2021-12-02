namespace RCIS.Style.StyleEditor
{
    partial class StyleLayerEditor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.layerGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.layerGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // layerGrid
            // 
            this.layerGrid.AllowUserToAddRows = false;
            this.layerGrid.AllowUserToDeleteRows = false;
            this.layerGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.layerGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.layerGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.layerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.layerGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.layerGrid.Location = new System.Drawing.Point(0, 0);
            this.layerGrid.MultiSelect = false;
            this.layerGrid.Name = "layerGrid";
            this.layerGrid.ReadOnly = true;
            this.layerGrid.RowHeadersVisible = false;
            this.layerGrid.RowTemplate.Height = 64;
            this.layerGrid.RowTemplate.ReadOnly = true;
            this.layerGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.layerGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.layerGrid.Size = new System.Drawing.Size(176, 344);
            this.layerGrid.TabIndex = 2;
            this.layerGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.layerGrid_MouseDown);
            this.layerGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellClick);
            this.layerGrid.SelectionChanged += new System.EventHandler(this.layerGrid_SelectionChanged);
            // 
            // StyleLayerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layerGrid);
            this.Name = "StyleLayerEditor";
            this.Size = new System.Drawing.Size(176, 344);
            ((System.ComponentModel.ISupportInitialize)(this.layerGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView layerGrid;

    }
}
