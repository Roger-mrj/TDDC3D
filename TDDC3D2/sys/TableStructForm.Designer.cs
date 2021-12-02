namespace TDDC3D.sys
{
    partial class TableStructForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableStructForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnAddFields = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpgradeDB = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCreateHistoryLayer = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.bePgdb = new DevExpress.XtraEditors.ButtonEdit();
            this.beFgdb = new DevExpress.XtraEditors.ButtonEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.新建数据集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除数据集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.新建要素类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除要素类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.升级至三调数据库标准ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.重命名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aImgList = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bePgdb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beFgdb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl1);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(353, 2);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1033, 689);
            this.panelControl1.TabIndex = 2;
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1029, 565);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "字段名";
            this.gridColumn1.FieldName = "FIELDNAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "别名";
            this.gridColumn2.FieldName = "FIELDALIAS";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "类型";
            this.gridColumn3.FieldName = "FIELDTYPE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "长度";
            this.gridColumn4.FieldName = "FIELDLENGTH";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnAddFields);
            this.groupControl1.Controls.Add(this.btnUpgradeDB);
            this.groupControl1.Controls.Add(this.simpleButton2);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.btnCreateHistoryLayer);
            this.groupControl1.Controls.Add(this.simpleButton8);
            this.groupControl1.Controls.Add(this.simpleButton7);
            this.groupControl1.Controls.Add(this.simpleButton6);
            this.groupControl1.Controls.Add(this.simpleButton5);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(2, 567);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1029, 120);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "工具栏";
            // 
            // btnAddFields
            // 
            this.btnAddFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFields.Location = new System.Drawing.Point(171, 45);
            this.btnAddFields.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddFields.Name = "btnAddFields";
            this.btnAddFields.Size = new System.Drawing.Size(160, 28);
            this.btnAddFields.TabIndex = 13;
            this.btnAddFields.Text = "批量追加字段";
            this.btnAddFields.Click += new System.EventHandler(this.btnAddFields_Click);
            // 
            // btnUpgradeDB
            // 
            this.btnUpgradeDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpgradeDB.Location = new System.Drawing.Point(411, 82);
            this.btnUpgradeDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnUpgradeDB.Name = "btnUpgradeDB";
            this.btnUpgradeDB.Size = new System.Drawing.Size(160, 28);
            this.btnUpgradeDB.TabIndex = 12;
            this.btnUpgradeDB.Text = "升级数据库";
            this.btnUpgradeDB.Click += new System.EventHandler(this.btnUpgradeDB_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Location = new System.Drawing.Point(714, 80);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(125, 30);
            this.simpleButton2.TabIndex = 11;
            this.simpleButton2.Text = "复制要素类";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Location = new System.Drawing.Point(411, 45);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(160, 28);
            this.simpleButton1.TabIndex = 10;
            this.simpleButton1.Text = "创建图幅号图层";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnCreateHistoryLayer
            // 
            this.btnCreateHistoryLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateHistoryLayer.Location = new System.Drawing.Point(171, 77);
            this.btnCreateHistoryLayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreateHistoryLayer.Name = "btnCreateHistoryLayer";
            this.btnCreateHistoryLayer.Size = new System.Drawing.Size(160, 30);
            this.btnCreateHistoryLayer.TabIndex = 9;
            this.btnCreateHistoryLayer.Text = "创建历史及过程图层";
            this.btnCreateHistoryLayer.Visible = false;
            this.btnCreateHistoryLayer.Click += new System.EventHandler(this.btnCreateHistoryLayer_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton8.Location = new System.Drawing.Point(714, 47);
            this.simpleButton8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(125, 30);
            this.simpleButton8.TabIndex = 7;
            this.simpleButton8.Text = "复制要素类结构";
            this.simpleButton8.Click += new System.EventHandler(this.simpleButton8_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton7.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton7.Image")));
            this.simpleButton7.Location = new System.Drawing.Point(865, 47);
            this.simpleButton7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(85, 63);
            this.simpleButton7.TabIndex = 6;
            this.simpleButton7.Text = "关闭";
            this.simpleButton7.Click += new System.EventHandler(this.simpleButton7_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton6.Location = new System.Drawing.Point(597, 80);
            this.simpleButton6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(91, 30);
            this.simpleButton6.TabIndex = 5;
            this.simpleButton6.Text = "删除字段";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton5.Location = new System.Drawing.Point(597, 47);
            this.simpleButton5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(91, 30);
            this.simpleButton5.TabIndex = 4;
            this.simpleButton5.Text = "添加字段";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.bePgdb);
            this.panelControl2.Controls.Add(this.beFgdb);
            this.panelControl2.Controls.Add(this.radioGroup1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1388, 97);
            this.panelControl2.TabIndex = 3;
            // 
            // bePgdb
            // 
            this.bePgdb.Location = new System.Drawing.Point(167, 57);
            this.bePgdb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bePgdb.Name = "bePgdb";
            this.bePgdb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bePgdb.Size = new System.Drawing.Size(476, 24);
            this.bePgdb.TabIndex = 2;
            this.bePgdb.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.bePgdb_ButtonClick);
            // 
            // beFgdb
            // 
            this.beFgdb.Location = new System.Drawing.Point(167, 27);
            this.beFgdb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.beFgdb.Name = "beFgdb";
            this.beFgdb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beFgdb.Size = new System.Drawing.Size(476, 24);
            this.beFgdb.TabIndex = 1;
            this.beFgdb.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beFgdb_ButtonClick);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(21, 17);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "打开FileGDB文件"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "打开PGDB文件")});
            this.radioGroup1.Size = new System.Drawing.Size(651, 72);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.splitterControl1);
            this.panelControl3.Controls.Add(this.panelControl1);
            this.panelControl3.Controls.Add(this.treeView1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 97);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1388, 693);
            this.panelControl3.TabIndex = 4;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(353, 2);
            this.splitterControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 689);
            this.splitterControl1.TabIndex = 4;
            this.splitterControl1.TabStop = false;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.aImgList;
            this.treeView1.Location = new System.Drawing.Point(2, 2);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(351, 689);
            this.treeView1.TabIndex = 3;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建数据集ToolStripMenuItem,
            this.删除数据集ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.新建要素类ToolStripMenuItem,
            this.删除要素类ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.升级至三调数据库标准ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.重命名ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(229, 166);
            // 
            // 新建数据集ToolStripMenuItem
            // 
            this.新建数据集ToolStripMenuItem.Image = global::TDDC3D.Properties.Resources.add_16px_1115807_easyicon_net;
            this.新建数据集ToolStripMenuItem.Name = "新建数据集ToolStripMenuItem";
            this.新建数据集ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.新建数据集ToolStripMenuItem.Text = "新建数据集";
            this.新建数据集ToolStripMenuItem.Click += new System.EventHandler(this.新建数据集ToolStripMenuItem_Click);
            // 
            // 删除数据集ToolStripMenuItem
            // 
            this.删除数据集ToolStripMenuItem.Name = "删除数据集ToolStripMenuItem";
            this.删除数据集ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.删除数据集ToolStripMenuItem.Text = "删除数据集";
            this.删除数据集ToolStripMenuItem.Click += new System.EventHandler(this.删除数据集ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(225, 6);
            // 
            // 新建要素类ToolStripMenuItem
            // 
            this.新建要素类ToolStripMenuItem.Image = global::TDDC3D.Properties.Resources.add_16px_1115807_easyicon_net;
            this.新建要素类ToolStripMenuItem.Name = "新建要素类ToolStripMenuItem";
            this.新建要素类ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.新建要素类ToolStripMenuItem.Text = "新建要素类";
            this.新建要素类ToolStripMenuItem.Click += new System.EventHandler(this.新建要素类ToolStripMenuItem_Click);
            // 
            // 删除要素类ToolStripMenuItem
            // 
            this.删除要素类ToolStripMenuItem.Name = "删除要素类ToolStripMenuItem";
            this.删除要素类ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.删除要素类ToolStripMenuItem.Text = "删除要素类";
            this.删除要素类ToolStripMenuItem.Click += new System.EventHandler(this.删除要素类ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(225, 6);
            // 
            // 升级至三调数据库标准ToolStripMenuItem
            // 
            this.升级至三调数据库标准ToolStripMenuItem.Name = "升级至三调数据库标准ToolStripMenuItem";
            this.升级至三调数据库标准ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.升级至三调数据库标准ToolStripMenuItem.Text = "升级至三调数据库标准";
            this.升级至三调数据库标准ToolStripMenuItem.Click += new System.EventHandler(this.升级至三调数据库标准ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(225, 6);
            // 
            // 重命名ToolStripMenuItem
            // 
            this.重命名ToolStripMenuItem.Image = global::TDDC3D.Properties.Resources.document_rename_16px_513166_easyicon_net;
            this.重命名ToolStripMenuItem.Name = "重命名ToolStripMenuItem";
            this.重命名ToolStripMenuItem.Size = new System.Drawing.Size(228, 24);
            this.重命名ToolStripMenuItem.Text = "重命名...";
            this.重命名ToolStripMenuItem.Click += new System.EventHandler(this.重命名ToolStripMenuItem_Click);
            // 
            // aImgList
            // 
            this.aImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("aImgList.ImageStream")));
            this.aImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.aImgList.Images.SetKeyName(0, "图层集.gif");
            this.aImgList.Images.SetKeyName(1, "点图层.gif");
            this.aImgList.Images.SetKeyName(2, "线图层.gif");
            this.aImgList.Images.SetKeyName(3, "面图层.gif");
            this.aImgList.Images.SetKeyName(4, "影像图层.gif");
            this.aImgList.Images.SetKeyName(5, "栅格图层.ico");
            this.aImgList.Images.SetKeyName(6, "帮助.gif");
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TableStructForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 790);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TableStructForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "结构维护";
            this.Load += new System.EventHandler(this.TableStructForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bePgdb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beFgdb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.ButtonEdit bePgdb;
        private DevExpress.XtraEditors.ButtonEdit beFgdb;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private System.Windows.Forms.TreeView treeView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private System.Windows.Forms.ImageList aImgList;
        private DevExpress.XtraEditors.SimpleButton btnCreateHistoryLayer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新建数据集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除数据集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 新建要素类ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除要素类ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 升级至三调数据库标准ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 重命名ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton btnUpgradeDB;
        private DevExpress.XtraEditors.SimpleButton btnAddFields;
    }
}