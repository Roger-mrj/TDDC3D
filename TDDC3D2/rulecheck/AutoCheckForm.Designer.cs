namespace RuleCheck
{
    partial class AutoCheckForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoCheckForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.treeListRules = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.memeoError = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnStartCheck = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControlError = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.将日志显示在主窗体ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewError = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.chkCurrentExt = new DevExpress.XtraEditors.CheckEdit();
            this.brSrcmdbWs = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memeoError.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlError)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkCurrentExt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brSrcmdbWs.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.treeListRules);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(417, 758);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "检查规则";
            // 
            // treeListRules
            // 
            this.treeListRules.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.treeListRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListRules.Location = new System.Drawing.Point(2, 26);
            this.treeListRules.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.treeListRules.Name = "treeListRules";
            this.treeListRules.BeginUnboundLoad();
            this.treeListRules.AppendNode(new object[] {
            null,
            null}, -1);
            this.treeListRules.AppendNode(new object[] {
            null,
            null}, 0);
            this.treeListRules.EndUnboundLoad();
            this.treeListRules.OptionsBehavior.Editable = false;
            this.treeListRules.OptionsBehavior.ReadOnly = true;
            this.treeListRules.OptionsView.ShowCheckBoxes = true;
            this.treeListRules.Size = new System.Drawing.Size(413, 730);
            this.treeListRules.TabIndex = 6;
            this.treeListRules.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.treeListRules_BeforeCheckNode);
            this.treeListRules.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeListRules_AfterCheckNode);
            this.treeListRules.Click += new System.EventHandler(this.treeListRules_Click);
            this.treeListRules.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeListRules_MouseClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "规则编号";
            this.treeListColumn1.FieldName = "规则编号";
            this.treeListColumn1.MinWidth = 68;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 214;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "规则分类";
            this.treeListColumn2.FieldName = "规则分类";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 1;
            this.treeListColumn2.Width = 230;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(417, 0);
            this.splitterControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 758);
            this.splitterControl1.TabIndex = 4;
            this.splitterControl1.TabStop = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.xtraTabControl1);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(422, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(974, 758);
            this.panelControl1.TabIndex = 5;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 114);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(970, 642);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.memoLog);
            this.xtraTabPage1.Controls.Add(this.memeoError);
            this.xtraTabPage1.Controls.Add(this.panelControl2);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(964, 609);
            this.xtraTabPage1.Text = "开始检查";
            // 
            // memoLog
            // 
            this.memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLog.Location = new System.Drawing.Point(0, 228);
            this.memoLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(964, 330);
            this.memoLog.TabIndex = 2;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // memeoError
            // 
            this.memeoError.Dock = System.Windows.Forms.DockStyle.Top;
            this.memeoError.Location = new System.Drawing.Point(0, 0);
            this.memeoError.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memeoError.Name = "memeoError";
            this.memeoError.Size = new System.Drawing.Size(964, 228);
            this.memeoError.TabIndex = 0;
            this.memeoError.UseOptimizedRendering = true;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lblStatus);
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Controls.Add(this.btnStartCheck);
            this.panelControl2.Controls.Add(this.btnClose);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 558);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(964, 51);
            this.panelControl2.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(15, 15);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(30, 18);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "状态";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(678, 2);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(84, 47);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "全选";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnStartCheck
            // 
            this.btnStartCheck.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStartCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnStartCheck.Image")));
            this.btnStartCheck.Location = new System.Drawing.Point(762, 2);
            this.btnStartCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartCheck.Name = "btnStartCheck";
            this.btnStartCheck.Size = new System.Drawing.Size(113, 47);
            this.btnStartCheck.TabIndex = 1;
            this.btnStartCheck.Text = "开始检查";
            this.btnStartCheck.Click += new System.EventHandler(this.btnStartCheck_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(875, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 47);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gridControlError);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(964, 609);
            this.xtraTabPage2.Text = "错误列表";
            // 
            // gridControlError
            // 
            this.gridControlError.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControlError.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControlError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlError.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControlError.Location = new System.Drawing.Point(0, 0);
            this.gridControlError.MainView = this.gridViewError;
            this.gridControlError.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControlError.Name = "gridControlError";
            this.gridControlError.Size = new System.Drawing.Size(964, 609);
            this.gridControlError.TabIndex = 0;
            this.gridControlError.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewError});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出日志ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.将日志显示在主窗体ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 58);
            // 
            // 导出日志ToolStripMenuItem
            // 
            this.导出日志ToolStripMenuItem.Name = "导出日志ToolStripMenuItem";
            this.导出日志ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.导出日志ToolStripMenuItem.Text = "导出错误日志";
            this.导出日志ToolStripMenuItem.Click += new System.EventHandler(this.导出日志ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(210, 6);
            // 
            // 将日志显示在主窗体ToolStripMenuItem
            // 
            this.将日志显示在主窗体ToolStripMenuItem.Name = "将日志显示在主窗体ToolStripMenuItem";
            this.将日志显示在主窗体ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.将日志显示在主窗体ToolStripMenuItem.Text = "将日志显示在主窗体";
            this.将日志显示在主窗体ToolStripMenuItem.Click += new System.EventHandler(this.将日志显示在主窗体ToolStripMenuItem_Click);
            // 
            // gridViewError
            // 
            this.gridViewError.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.gridColumn1,
            this.gridColumn6,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn7,
            this.gridColumn4});
            this.gridViewError.GridControl = this.gridControlError;
            this.gridViewError.Name = "gridViewError";
            this.gridViewError.OptionsBehavior.Editable = false;
            this.gridViewError.OptionsBehavior.ReadOnly = true;
            this.gridViewError.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "规则编号";
            this.gridColumn5.FieldName = "规则编号";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "错误描述";
            this.gridColumn1.FieldName = "错误描述";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "错误类型";
            this.gridColumn6.FieldName = "错误类型";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "图层/表格";
            this.gridColumn2.FieldName = "涉及图层";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "标识码";
            this.gridColumn3.FieldName = "要素BSM";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "要素ID";
            this.gridColumn7.FieldName = "要素ID";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "错误级别";
            this.gridColumn4.FieldName = "错误级别";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.chkCurrentExt);
            this.groupControl2.Controls.Add(this.brSrcmdbWs);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(2, 2);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(970, 112);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "选择检查数据库";
            // 
            // chkCurrentExt
            // 
            this.chkCurrentExt.Location = new System.Drawing.Point(28, 78);
            this.chkCurrentExt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkCurrentExt.Name = "chkCurrentExt";
            this.chkCurrentExt.Properties.Caption = "图形只检查当前范围";
            this.chkCurrentExt.Size = new System.Drawing.Size(207, 22);
            this.chkCurrentExt.TabIndex = 2;
            // 
            // brSrcmdbWs
            // 
            this.brSrcmdbWs.Location = new System.Drawing.Point(164, 41);
            this.brSrcmdbWs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.brSrcmdbWs.Name = "brSrcmdbWs";
            this.brSrcmdbWs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.brSrcmdbWs.Size = new System.Drawing.Size(515, 24);
            this.brSrcmdbWs.TabIndex = 1;
            this.brSrcmdbWs.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.brSrcmdbWs_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(27, 44);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(105, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择检查数据库";
            // 
            // AutoCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 758);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AutoCheckForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据质量检查";
            this.Load += new System.EventHandler(this.AutoCheckForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memeoError.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlError)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkCurrentExt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brSrcmdbWs.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraTreeList.TreeList treeListRules;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gridControlError;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewError;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.MemoEdit memeoError;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btnStartCheck;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.ButtonEdit brSrcmdbWs;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 将日志显示在主窗体ToolStripMenuItem;
        private DevExpress.XtraEditors.CheckEdit chkCurrentExt;
        private DevExpress.XtraEditors.MemoEdit memoLog;
    }
}