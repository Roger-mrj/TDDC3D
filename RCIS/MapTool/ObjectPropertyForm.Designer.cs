namespace RCIS.MapTool
{
    partial class ObjectPropertyForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.targetLayerComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.objectInfoListView = new System.Windows.Forms.ListView();
            this.propertyGroup = new System.Windows.Forms.GroupBox();
            this.objectTreeGroup = new System.Windows.Forms.GroupBox();
            this.objectTreeView = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbPosition = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.复制内容ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.propertyGroup.SuspendLayout();
            this.objectTreeGroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.targetLayerComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(123, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 40);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // targetLayerComboBox
            // 
            this.targetLayerComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetLayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetLayerComboBox.Location = new System.Drawing.Point(72, 17);
            this.targetLayerComboBox.MaxDropDownItems = 20;
            this.targetLayerComboBox.Name = "targetLayerComboBox";
            this.targetLayerComboBox.Size = new System.Drawing.Size(262, 20);
            this.targetLayerComboBox.TabIndex = 1;
            this.targetLayerComboBox.SelectedIndexChanged += new System.EventHandler(this.targetLayerComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(3, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "目标图层：";
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel2.Location = new System.Drawing.Point(246, 306);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(80, 16);
            this.linkLabel2.TabIndex = 9;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "扩展表中数据";
            this.linkLabel2.Visible = false;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem4,
            this.menuItem3,
            this.menuItem5});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "定位到要素";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "闪烁要素图形";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            this.menuItem4.Visible = false;
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "标记此要素";
            this.menuItem3.Visible = false;
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "修改标记颜色";
            this.menuItem5.Visible = false;
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.Location = new System.Drawing.Point(98, 306);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(80, 16);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "图形坐标数据";
            this.linkLabel1.Visible = false;
            // 
            // objectInfoListView
            // 
            this.objectInfoListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.objectInfoListView.ContextMenuStrip = this.contextMenuStrip1;
            this.objectInfoListView.FullRowSelect = true;
            this.objectInfoListView.GridLines = true;
            this.objectInfoListView.HideSelection = false;
            this.objectInfoListView.Location = new System.Drawing.Point(8, 8);
            this.objectInfoListView.MultiSelect = false;
            this.objectInfoListView.Name = "objectInfoListView";
            this.objectInfoListView.Size = new System.Drawing.Size(316, 295);
            this.objectInfoListView.TabIndex = 10;
            this.objectInfoListView.UseCompatibleStateImageBehavior = false;
            this.objectInfoListView.View = System.Windows.Forms.View.Details;
            this.objectInfoListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.objectInfoListView_KeyDown);
            // 
            // propertyGroup
            // 
            this.propertyGroup.Controls.Add(this.objectInfoListView);
            this.propertyGroup.Controls.Add(this.linkLabel2);
            this.propertyGroup.Controls.Add(this.linkLabel1);
            this.propertyGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGroup.Location = new System.Drawing.Point(3, 37);
            this.propertyGroup.Name = "propertyGroup";
            this.propertyGroup.Size = new System.Drawing.Size(331, 327);
            this.propertyGroup.TabIndex = 8;
            this.propertyGroup.TabStop = false;
            // 
            // objectTreeGroup
            // 
            this.objectTreeGroup.Controls.Add(this.objectTreeView);
            this.objectTreeGroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.objectTreeGroup.Location = new System.Drawing.Point(0, 0);
            this.objectTreeGroup.Name = "objectTreeGroup";
            this.objectTreeGroup.Size = new System.Drawing.Size(120, 367);
            this.objectTreeGroup.TabIndex = 8;
            this.objectTreeGroup.TabStop = false;
            this.objectTreeGroup.Text = "对象";
            // 
            // objectTreeView
            // 
            this.objectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTreeView.Location = new System.Drawing.Point(3, 17);
            this.objectTreeView.Name = "objectTreeView";
            this.objectTreeView.Size = new System.Drawing.Size(114, 347);
            this.objectTreeView.TabIndex = 0;
            this.objectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectTreeView_AfterSelect);
            this.objectTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.objectTreeView_MouseDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.propertyGroup);
            this.groupBox2.Controls.Add(this.lbPosition);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(123, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 367);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // lbPosition
            // 
            this.lbPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPosition.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbPosition.Location = new System.Drawing.Point(3, 17);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(331, 20);
            this.lbPosition.TabIndex = 5;
            this.lbPosition.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(120, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 367);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制内容ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // 复制内容ToolStripMenuItem
            // 
            this.复制内容ToolStripMenuItem.Name = "复制内容ToolStripMenuItem";
            this.复制内容ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.复制内容ToolStripMenuItem.Text = "复制内容";
            this.复制内容ToolStripMenuItem.Click += new System.EventHandler(this.复制内容ToolStripMenuItem_Click);
            // 
            // ObjectPropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 367);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.objectTreeGroup);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectPropertyForm";
            this.ShowIcon = false;
            this.Text = "对象信息";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.propertyGroup.ResumeLayout(false);
            this.objectTreeGroup.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox targetLayerComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ListView objectInfoListView;
        private System.Windows.Forms.GroupBox propertyGroup;
        private System.Windows.Forms.GroupBox objectTreeGroup;
        private System.Windows.Forms.TreeView objectTreeView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbPosition;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制内容ToolStripMenuItem;
    }
}