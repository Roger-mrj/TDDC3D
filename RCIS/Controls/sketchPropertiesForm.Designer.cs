namespace RCIS.Controls
{
    partial class sketchPropertiesForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lstIds = new DevExpress.XtraEditors.ListBoxControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lstProperties = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.删除结点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.lstIds)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstIds
            // 
            this.lstIds.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstIds.Location = new System.Drawing.Point(0, 0);
            this.lstIds.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstIds.Name = "lstIds";
            this.lstIds.Size = new System.Drawing.Size(160, 436);
            this.lstIds.TabIndex = 0;
            this.lstIds.SelectedIndexChanged += new System.EventHandler(this.lstIds_SelectedIndexChanged);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(160, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 436);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // lstProperties
            // 
            this.lstProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstProperties.ContextMenuStrip = this.contextMenuStrip1;
            this.lstProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProperties.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstProperties.FullRowSelect = true;
            this.lstProperties.GridLines = true;
            this.lstProperties.Location = new System.Drawing.Point(164, 0);
            this.lstProperties.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstProperties.Name = "lstProperties";
            this.lstProperties.Size = new System.Drawing.Size(481, 436);
            this.lstProperties.TabIndex = 2;
            this.lstProperties.UseCompatibleStateImageBehavior = false;
            this.lstProperties.View = System.Windows.Forms.View.Details;
            this.lstProperties.DoubleClick += new System.EventHandler(this.lstProperties_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "X";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Y";
            this.columnHeader3.Width = 100;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除结点ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 28);
            // 
            // 删除结点ToolStripMenuItem
            // 
            this.删除结点ToolStripMenuItem.Name = "删除结点ToolStripMenuItem";
            this.删除结点ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.删除结点ToolStripMenuItem.Text = "删除结点";
            this.删除结点ToolStripMenuItem.Click += new System.EventHandler(this.删除结点ToolStripMenuItem_Click);
            // 
            // sketchPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 436);
            this.Controls.Add(this.lstProperties);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lstIds);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "sketchPropertiesForm";
            this.ShowIcon = false;
            this.Text = "点属性";
            this.Load += new System.EventHandler(this.sketchPropertiesForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lstIds)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl lstIds;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ListView lstProperties;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除结点ToolStripMenuItem;
    }
}