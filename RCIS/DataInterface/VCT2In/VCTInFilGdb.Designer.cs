namespace RCIS.DataInterface.VCT2
{
    partial class VCTInFilGdb
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
            this.beDestDir = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beVCTFile = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.chkExtATTR = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.beDestDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beVCTFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExtATTR.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beDestDir
            // 
            this.beDestDir.Location = new System.Drawing.Point(137, 46);
            this.beDestDir.Name = "beDestDir";
            this.beDestDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestDir.Size = new System.Drawing.Size(489, 24);
            this.beDestDir.TabIndex = 19;
            this.beDestDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestDir_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(20, 52);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "目标文件夹";
            // 
            // beVCTFile
            // 
            this.beVCTFile.Location = new System.Drawing.Point(138, 12);
            this.beVCTFile.Name = "beVCTFile";
            this.beVCTFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beVCTFile.Size = new System.Drawing.Size(489, 24);
            this.beVCTFile.TabIndex = 17;
            this.beVCTFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beVCTFile_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(21, 18);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(58, 18);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "VCT文件";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(21, 95);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.Size = new System.Drawing.Size(627, 361);
            this.xtraTabControl1.TabIndex = 20;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.memoLog);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(621, 328);
            this.xtraTabPage2.Text = "日志";
            // 
            // memoLog
            // 
            this.memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLog.Location = new System.Drawing.Point(0, 0);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(621, 328);
            this.memoLog.TabIndex = 0;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(449, 507);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(84, 33);
            this.btnOk.TabIndex = 22;
            this.btnOk.Text = "导入(AE)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(559, 507);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 33);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkExtATTR
            // 
            this.chkExtATTR.Location = new System.Drawing.Point(22, 513);
            this.chkExtATTR.Name = "chkExtATTR";
            this.chkExtATTR.Properties.Caption = "同时导入扩展属性";
            this.chkExtATTR.Size = new System.Drawing.Size(263, 22);
            this.chkExtATTR.TabIndex = 23;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(277, 507);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(125, 33);
            this.simpleButton1.TabIndex = 27;
            this.simpleButton1.Text = "导入（GDAL）";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // VCTInFilGdb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 569);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.chkExtATTR);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.beDestDir);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.beVCTFile);
            this.Controls.Add(this.labelControl1);
            this.Name = "VCTInFilGdb";
            this.ShowIcon = false;
            this.Text = "VCT导入";
            ((System.ComponentModel.ISupportInitialize)(this.beDestDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beVCTFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkExtATTR.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit beDestDir;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beVCTFile;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.CheckEdit chkExtATTR;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}