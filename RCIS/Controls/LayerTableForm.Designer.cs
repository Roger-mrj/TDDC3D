namespace RCIS.Controls
{
    partial class LayerTableForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuTable = new System.Windows.Forms.ContextMenuStrip();
            this.DisplayLayerExtentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.选中图形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定位图形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.清除选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.删除选中数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.menuTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuTable
            // 
            this.menuTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplayLayerExtentMenuItem,
            this.toolStripMenuItem1,
            this.选中图形ToolStripMenuItem,
            this.定位图形ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.清除选择ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.删除选中数据ToolStripMenuItem});
            this.menuTable.Name = "menuLayer";
            this.menuTable.Size = new System.Drawing.Size(169, 142);
            // 
            // DisplayLayerExtentMenuItem
            // 
            this.DisplayLayerExtentMenuItem.Name = "DisplayLayerExtentMenuItem";
            this.DisplayLayerExtentMenuItem.Size = new System.Drawing.Size(168, 24);
            this.DisplayLayerExtentMenuItem.Text = "导出数据...";
            this.DisplayLayerExtentMenuItem.Click += new System.EventHandler(this.DisplayLayerExtentMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 6);
            // 
            // 选中图形ToolStripMenuItem
            // 
            this.选中图形ToolStripMenuItem.Name = "选中图形ToolStripMenuItem";
            this.选中图形ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.选中图形ToolStripMenuItem.Text = "选中图形";
            this.选中图形ToolStripMenuItem.Click += new System.EventHandler(this.选中图形ToolStripMenuItem_Click);
            // 
            // 定位图形ToolStripMenuItem
            // 
            this.定位图形ToolStripMenuItem.Name = "定位图形ToolStripMenuItem";
            this.定位图形ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.定位图形ToolStripMenuItem.Text = "定位图形";
            this.定位图形ToolStripMenuItem.Click += new System.EventHandler(this.定位图形ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(165, 6);
            // 
            // 清除选择ToolStripMenuItem
            // 
            this.清除选择ToolStripMenuItem.Name = "清除选择ToolStripMenuItem";
            this.清除选择ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.清除选择ToolStripMenuItem.Text = "清除选择";
            this.清除选择ToolStripMenuItem.Click += new System.EventHandler(this.清除选择ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(165, 6);
            // 
            // 删除选中数据ToolStripMenuItem
            // 
            this.删除选中数据ToolStripMenuItem.Name = "删除选中数据ToolStripMenuItem";
            this.删除选中数据ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.删除选中数据ToolStripMenuItem.Text = "删除选中数据";
            this.删除选中数据ToolStripMenuItem.Click += new System.EventHandler(this.删除选中数据ToolStripMenuItem_Click);
            // 
            // gridControl
            // 
            this.gridControl.ContextMenuStrip = this.menuTable;
            this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.Location = new System.Drawing.Point(2, 26);
            this.gridControl.MainView = this.gridView1;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(803, 393);
            this.gridControl.TabIndex = 1;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gridControl);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(807, 421);
            this.groupControl1.TabIndex = 2;
            // 
            // LayerTableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 421);
            this.Controls.Add(this.groupControl1);
            this.Name = "LayerTableForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "表属性";
            this.Load += new System.EventHandler(this.LayerTableForm_Load);
            this.menuTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuTable;
        private System.Windows.Forms.ToolStripMenuItem DisplayLayerExtentMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 选中图形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定位图形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 清除选择ToolStripMenuItem;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 删除选中数据ToolStripMenuItem;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}