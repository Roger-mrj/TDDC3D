namespace TDDC3D.gengxin
{
    partial class FrmCheckData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCheckData));
            this.tabCheckData = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtDLTBBH = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnCheck = new DevExpress.XtraEditors.SimpleButton();
            this.treCheckOption = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.tabCheckData)).BeginInit();
            this.tabCheckData.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDLTBBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treCheckOption)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCheckData
            // 
            this.tabCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCheckData.Location = new System.Drawing.Point(0, 0);
            this.tabCheckData.Name = "tabCheckData";
            this.tabCheckData.SelectedTabPage = this.xtraTabPage1;
            this.tabCheckData.Size = new System.Drawing.Size(688, 421);
            this.tabCheckData.TabIndex = 0;
            this.tabCheckData.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtDLTBBH);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.info);
            this.xtraTabPage1.Controls.Add(this.progressBarControl1);
            this.xtraTabPage1.Controls.Add(this.btnClose);
            this.xtraTabPage1.Controls.Add(this.btnCheck);
            this.xtraTabPage1.Controls.Add(this.treCheckOption);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(682, 388);
            this.xtraTabPage1.Text = "检查条件";
            // 
            // txtDLTBBH
            // 
            this.txtDLTBBH.Location = new System.Drawing.Point(381, 12);
            this.txtDLTBBH.Name = "txtDLTBBH";
            this.txtDLTBBH.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDLTBBH.Properties.ReadOnly = true;
            this.txtDLTBBH.Size = new System.Drawing.Size(294, 24);
            this.txtDLTBBH.TabIndex = 15;
            this.txtDLTBBH.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtDLTBBH_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(300, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 14;
            this.labelControl1.Text = "变化图斑：";
            // 
            // info
            // 
            this.info.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.info.Location = new System.Drawing.Point(283, 101);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(399, 269);
            this.info.TabIndex = 5;
            this.info.UseOptimizedRendering = true;
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarControl1.Location = new System.Drawing.Point(283, 370);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(399, 18);
            this.progressBarControl1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(554, 51);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(93, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(440, 51);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(93, 40);
            this.btnCheck.TabIndex = 2;
            this.btnCheck.Text = "检查";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // treCheckOption
            // 
            this.treCheckOption.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treCheckOption.Dock = System.Windows.Forms.DockStyle.Left;
            this.treCheckOption.Location = new System.Drawing.Point(0, 0);
            this.treCheckOption.Name = "treCheckOption";
            this.treCheckOption.BeginUnboundLoad();
            this.treCheckOption.AppendNode(new object[] {
            "检查内容"}, -1);
            this.treCheckOption.AppendNode(new object[] {
            "地类编码与名称"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "权属性质"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "权属代码长度（19位）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "坐落单位代码长度（19位）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "扣除地类编码（必须填1203，地类是耕地）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "扣除系数（0<=x<1，地类是耕地）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "耕地类型（地类是耕地）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "耕地坡度级别（1到5，地类是耕地）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "线状地物宽度（地类必须是1001,1003,1004,1006,1009,1101,1107）"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "图斑细化代码和名称"}, 0);
            this.treCheckOption.AppendNode(new object[] {
            "种植属性代码和名称"}, 0);
            this.treCheckOption.EndUnboundLoad();
            this.treCheckOption.OptionsBehavior.Editable = false;
            this.treCheckOption.OptionsBehavior.PopulateServiceColumns = true;
            this.treCheckOption.OptionsView.ShowCheckBoxes = true;
            this.treCheckOption.Size = new System.Drawing.Size(283, 388);
            this.treCheckOption.TabIndex = 0;
            this.treCheckOption.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.treCheckOption_BeforeCheckNode);
            this.treCheckOption.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treCheckOption_AfterCheckNode);
            this.treCheckOption.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treCheckOption_MouseClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "检查条件";
            this.treeListColumn1.FieldName = "检查条件";
            this.treeListColumn1.MinWidth = 70;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gridControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(682, 388);
            this.xtraTabPage2.Text = "检查结果";
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(682, 388);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.Click += new System.EventHandler(this.gridView1_Click);
            // 
            // FrmCheckData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 421);
            this.Controls.Add(this.tabCheckData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCheckData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "变化地类图斑数据属性检查";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmCheckData_FormClosed);
            this.Load += new System.EventHandler(this.FrmCheckData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabCheckData)).EndInit();
            this.tabCheckData.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDLTBBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treCheckOption)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabCheckData;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnCheck;
        private DevExpress.XtraTreeList.TreeList treCheckOption;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;
        private DevExpress.XtraEditors.ButtonEdit txtDLTBBH;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}