namespace RCIS.DataInterface.VCT3In
{
    partial class VCT3InFGDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCT3InFGDB));
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.beDestDir = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beVCTFile = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beVCTFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(325, 466);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(72, 26);
            this.simpleButton1.TabIndex = 34;
            this.simpleButton1.Text = "导入";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(428, 466);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 26);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(5, 72);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.Size = new System.Drawing.Size(515, 378);
            this.xtraTabControl1.TabIndex = 32;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.memoLog);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(509, 349);
            this.xtraTabPage2.Text = "日志";
            // 
            // memoLog
            // 
            this.memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLog.Location = new System.Drawing.Point(0, 0);
            this.memoLog.Margin = new System.Windows.Forms.Padding(2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(509, 349);
            this.memoLog.TabIndex = 0;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // beDestDir
            // 
            this.beDestDir.Location = new System.Drawing.Point(92, 36);
            this.beDestDir.Margin = new System.Windows.Forms.Padding(2);
            this.beDestDir.Name = "beDestDir";
            this.beDestDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestDir.Size = new System.Drawing.Size(424, 20);
            this.beDestDir.TabIndex = 31;
            this.beDestDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestDir_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(4, 41);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 30;
            this.labelControl2.Text = "目标文件夹";
            // 
            // beVCTFile
            // 
            this.beVCTFile.Location = new System.Drawing.Point(93, 9);
            this.beVCTFile.Margin = new System.Windows.Forms.Padding(2);
            this.beVCTFile.Name = "beVCTFile";
            this.beVCTFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beVCTFile.Size = new System.Drawing.Size(424, 20);
            this.beVCTFile.TabIndex = 29;
            this.beVCTFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beVCTFile_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 14);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(47, 14);
            this.labelControl1.TabIndex = 28;
            this.labelControl1.Text = "VCT文件";
            // 
            // VCT3InFGDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 512);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.beDestDir);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.beVCTFile);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VCT3InFGDB";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VCT3->FileGDB";
            this.Load += new System.EventHandler(this.VCT3InFGDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beVCTFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.ButtonEdit beDestDir;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beVCTFile;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}