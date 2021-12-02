namespace RCIS.DataExchange.DWG2SHP
{
    partial class Cad2ShpForm
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
            this.tabMain = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.beDestFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.lstSrcClasses = new DevExpress.XtraEditors.ListBoxControl();
            this.label2 = new System.Windows.Forms.Label();
            this.beDWG = new DevExpress.XtraEditors.ButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beDestFolder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstSrcClasses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDWG.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnClose);
            this.groupControl1.Controls.Add(this.btnStart);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 326);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(453, 105);
            this.groupControl1.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(315, 41);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(47, 41);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "开始转化";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tabMain
            // 
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedTabPage = this.xtraTabPage1;
            this.tabMain.Size = new System.Drawing.Size(453, 326);
            this.tabMain.TabIndex = 8;
            this.tabMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.beDestFolder);
            this.xtraTabPage1.Controls.Add(this.lstSrcClasses);
            this.xtraTabPage1.Controls.Add(this.label2);
            this.xtraTabPage1.Controls.Add(this.beDWG);
            this.xtraTabPage1.Controls.Add(this.label1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(447, 297);
            this.xtraTabPage1.Text = "选项";
            // 
            // beDestFolder
            // 
            this.beDestFolder.Location = new System.Drawing.Point(16, 253);
            this.beDestFolder.Name = "beDestFolder";
            this.beDestFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestFolder.Size = new System.Drawing.Size(413, 20);
            this.beDestFolder.TabIndex = 9;
            this.beDestFolder.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestFolder_ButtonClick);
            // 
            // lstSrcClasses
            // 
            this.lstSrcClasses.Location = new System.Drawing.Point(16, 70);
            this.lstSrcClasses.Name = "lstSrcClasses";
            this.lstSrcClasses.Size = new System.Drawing.Size(413, 153);
            this.lstSrcClasses.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "选择目标文件夹";
            // 
            // beDWG
            // 
            this.beDWG.Location = new System.Drawing.Point(16, 43);
            this.beDWG.Name = "beDWG";
            this.beDWG.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDWG.Size = new System.Drawing.Size(413, 20);
            this.beDWG.TabIndex = 6;
            this.beDWG.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDWG_ButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "选择DWG文件所在路径";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.txtLog);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(447, 297);
            this.xtraTabPage2.Text = "结果";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(447, 297);
            this.txtLog.TabIndex = 0;
            // 
            // Cad2ShpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 431);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.groupControl1);
            this.Name = "Cad2ShpForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DWG转换成多个Shape文件";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beDestFolder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstSrcClasses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDWG.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnStart;
        private DevExpress.XtraTab.XtraTabControl tabMain;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.ButtonEdit beDestFolder;
        private DevExpress.XtraEditors.ListBoxControl lstSrcClasses;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit beDWG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLog;

    }
}