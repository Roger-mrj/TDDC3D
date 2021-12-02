namespace RCIS.MapTool
{
    partial class LayerTableForm2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerTableForm2));
            this.清除选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.menuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DisplayLayerExtentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.选中图形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.定位图形ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.删除选中数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.btnclose = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            this.menuTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 清除选择ToolStripMenuItem
            // 
            this.清除选择ToolStripMenuItem.Name = "清除选择ToolStripMenuItem";
            this.清除选择ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清除选择ToolStripMenuItem.Text = "清除选择";
            this.清除选择ToolStripMenuItem.Click += new System.EventHandler(this.清除选择ToolStripMenuItem_Click);
            // 
            // gridControl
            // 
            this.gridControl.ContextMenuStrip = this.menuTable;
            this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gridControl.Location = new System.Drawing.Point(2, 22);
            this.gridControl.MainView = this.gridView1;
            this.gridControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(710, 320);
            this.gridControl.TabIndex = 2;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
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
            this.menuTable.Size = new System.Drawing.Size(149, 132);
            // 
            // DisplayLayerExtentMenuItem
            // 
            this.DisplayLayerExtentMenuItem.Name = "DisplayLayerExtentMenuItem";
            this.DisplayLayerExtentMenuItem.Size = new System.Drawing.Size(148, 22);
            this.DisplayLayerExtentMenuItem.Text = "导出数据...";
            this.DisplayLayerExtentMenuItem.Click += new System.EventHandler(this.DisplayLayerExtentMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // 选中图形ToolStripMenuItem
            // 
            this.选中图形ToolStripMenuItem.Name = "选中图形ToolStripMenuItem";
            this.选中图形ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.选中图形ToolStripMenuItem.Text = "选中图形";
            this.选中图形ToolStripMenuItem.Click += new System.EventHandler(this.选中图形ToolStripMenuItem_Click);
            // 
            // 定位图形ToolStripMenuItem
            // 
            this.定位图形ToolStripMenuItem.Name = "定位图形ToolStripMenuItem";
            this.定位图形ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.定位图形ToolStripMenuItem.Text = "定位图形";
            this.定位图形ToolStripMenuItem.Click += new System.EventHandler(this.定位图形ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(145, 6);
            // 
            // 删除选中数据ToolStripMenuItem
            // 
            this.删除选中数据ToolStripMenuItem.Name = "删除选中数据ToolStripMenuItem";
            this.删除选中数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除选中数据ToolStripMenuItem.Text = "删除选中数据";
            this.删除选中数据ToolStripMenuItem.Click += new System.EventHandler(this.删除选中数据ToolStripMenuItem_Click);
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
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnRefresh);
            this.panelControl1.Controls.Add(this.btnclose);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 344);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(714, 58);
            this.panelControl1.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(505, 23);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(62, 22);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnclose
            // 
            this.btnclose.Image = ((System.Drawing.Image)(resources.GetObject("btnclose.Image")));
            this.btnclose.Location = new System.Drawing.Point(613, 23);
            this.btnclose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(62, 22);
            this.btnclose.TabIndex = 0;
            this.btnclose.Text = "关闭";
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gridControl);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(714, 344);
            this.groupControl1.TabIndex = 4;
            // 
            // LayerTableForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 402);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "LayerTableForm2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "属性表";
            this.Load += new System.EventHandler(this.LayerTableForm2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            this.menuTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem 清除选择ToolStripMenuItem;
        private DevExpress.XtraGrid.GridControl gridControl;
        private System.Windows.Forms.ContextMenuStrip menuTable;
        private System.Windows.Forms.ToolStripMenuItem DisplayLayerExtentMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 选中图形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 定位图形ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 删除选中数据ToolStripMenuItem;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnclose;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
    }
}