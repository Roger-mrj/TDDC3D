namespace TDDC3D.sys
{
    partial class DictionaryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DictionaryForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("30控制点类型及等级代码表");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("31标石类型代码表");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("32标志类型代码表");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("33界线类型代码表");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("34界线性质代码表");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("35等高线类型代码表");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("36坡度级别代码表");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("37权属性质代码表");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("38图斑细化类型代码表");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("39种植属性代码表");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("40开发园区类型代码");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("41临时用地用途分类代码表");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("42临时用地具体项目用途分类代码表");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("43批准农转用项目业务类型代码表");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("44生态保护红线类型代码表");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("45无居民海岛利用现状分类代码表");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("所有字典表", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16});
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.tvAllTable = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(195, 35);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 31;
            this.simpleButton1.Text = "导出";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(387, 432);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.simpleButton1);
            this.groupControl2.Controls.Add(this.simpleButton5);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(2, 434);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(387, 75);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "工具栏";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton5.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.Image")));
            this.simpleButton5.Location = new System.Drawing.Point(277, 35);
            this.simpleButton5.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(71, 25);
            this.simpleButton5.TabIndex = 30;
            this.simpleButton5.Text = "关闭";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl1);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(269, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(391, 511);
            this.panelControl1.TabIndex = 5;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(264, 0);
            this.splitterControl1.Margin = new System.Windows.Forms.Padding(2);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 511);
            this.splitterControl1.TabIndex = 4;
            this.splitterControl1.TabStop = false;
            // 
            // tvAllTable
            // 
            this.tvAllTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAllTable.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tvAllTable.ImageIndex = 0;
            this.tvAllTable.ImageList = this.imageList1;
            this.tvAllTable.Location = new System.Drawing.Point(2, 22);
            this.tvAllTable.Margin = new System.Windows.Forms.Padding(2);
            this.tvAllTable.Name = "tvAllTable";
            treeNode1.Name = "节点3";
            treeNode1.Tag = "DIC_30控制点类型及等级代码表";
            treeNode1.Text = "30控制点类型及等级代码表";
            treeNode2.Name = "节点2";
            treeNode2.Tag = "DIC_31标石类型代码表";
            treeNode2.Text = "31标石类型代码表";
            treeNode3.Name = "节点3";
            treeNode3.Tag = "DIC_32标志类型代码表";
            treeNode3.Text = "32标志类型代码表";
            treeNode4.Name = "节点0";
            treeNode4.Tag = "DIC_33界线类型代码表";
            treeNode4.Text = "33界线类型代码表";
            treeNode5.Name = "节点1";
            treeNode5.Tag = "DIC_34界线性质代码表";
            treeNode5.Text = "34界线性质代码表";
            treeNode6.Name = "节点2";
            treeNode6.Tag = "DIC_35等高线类型代码表";
            treeNode6.Text = "35等高线类型代码表";
            treeNode7.Name = "节点3";
            treeNode7.Tag = "DIC_36坡度级别代码表";
            treeNode7.Text = "36坡度级别代码表";
            treeNode8.Name = "节点4";
            treeNode8.Tag = "DIC_37权属性质代码表";
            treeNode8.Text = "37权属性质代码表";
            treeNode9.Name = "节点5";
            treeNode9.Tag = "DIC_38图斑细化类型代码表";
            treeNode9.Text = "38图斑细化类型代码表";
            treeNode10.Name = "节点6";
            treeNode10.Tag = "DIC_39种植属性代码表";
            treeNode10.Text = "39种植属性代码表";
            treeNode11.Name = "节点7";
            treeNode11.Tag = "DIC_40开发园区类型代码";
            treeNode11.Text = "40开发园区类型代码";
            treeNode12.Name = "节点8";
            treeNode12.Tag = "DIC_41临时用地用途分类代码表";
            treeNode12.Text = "41临时用地用途分类代码表";
            treeNode13.Name = "节点9";
            treeNode13.Tag = "DIC_42临时用地具体项目用途分类代码表";
            treeNode13.Text = "42临时用地具体项目用途分类代码表";
            treeNode14.Name = "节点12";
            treeNode14.Tag = "DIC_43批准农转用项目业务类型代码表";
            treeNode14.Text = "43批准农转用项目业务类型代码表";
            treeNode15.Name = "节点15";
            treeNode15.Tag = "DIC_44生态保护红线类型代码表";
            treeNode15.Text = "44生态保护红线类型代码表";
            treeNode16.Name = "节点1";
            treeNode16.Tag = "DIC_45无居民海岛利用现状分类代码表";
            treeNode16.Text = "45无居民海岛利用现状分类代码表";
            treeNode17.Name = "节点0";
            treeNode17.Tag = "";
            treeNode17.Text = "所有字典表";
            this.tvAllTable.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode17});
            this.tvAllTable.SelectedImageIndex = 0;
            this.tvAllTable.Size = new System.Drawing.Size(260, 487);
            this.tvAllTable.TabIndex = 0;
            this.tvAllTable.DoubleClick += new System.EventHandler(this.tvAllTable_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder_remove_21.391304347826px_1198273_easyicon.net.png");
            this.imageList1.Images.SetKeyName(1, "Folder_open_22.237288135593px_1198271_easyicon.net.png");
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.tvAllTable);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(264, 511);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "所有字典表";
            // 
            // DictionaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 511);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DictionaryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "字典表";
            this.Load += new System.EventHandler(this.DictionaryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private System.Windows.Forms.TreeView tvAllTable;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}