namespace RCIS.DataInterface.VCTOut
{
    partial class VCTOutput3Frm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCTOutput3Frm));
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.beDestVCTfile = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDH = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.beTmpDir = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.chkByXzqDo = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestVCTfile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beTmpDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkByXzqDo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(363, 510);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(66, 25);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(16, 42);
            this.memoLog.Margin = new System.Windows.Forms.Padding(2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(428, 307);
            this.memoLog.TabIndex = 7;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // beDestVCTfile
            // 
            this.beDestVCTfile.Location = new System.Drawing.Point(102, 11);
            this.beDestVCTfile.Margin = new System.Windows.Forms.Padding(2);
            this.beDestVCTfile.Name = "beDestVCTfile";
            this.beDestVCTfile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestVCTfile.Size = new System.Drawing.Size(343, 20);
            this.beDestVCTfile.TabIndex = 6;
            this.beDestVCTfile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestVCTfile_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 14);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(83, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "目标VCT文件：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(20, 371);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "带号：";
            // 
            // txtDH
            // 
            this.txtDH.EditValue = "38";
            this.txtDH.Location = new System.Drawing.Point(58, 367);
            this.txtDH.Margin = new System.Windows.Forms.Padding(2);
            this.txtDH.Name = "txtDH";
            this.txtDH.Size = new System.Drawing.Size(75, 20);
            this.txtDH.TabIndex = 12;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(247, 510);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(111, 25);
            this.simpleButton1.TabIndex = 14;
            this.simpleButton1.Text = "导出索引文件";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton5);
            this.groupControl1.Controls.Add(this.beTmpDir);
            this.groupControl1.Controls.Add(this.simpleButton3);
            this.groupControl1.Location = new System.Drawing.Point(8, 415);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(436, 68);
            this.groupControl1.TabIndex = 17;
            this.groupControl1.Text = "临时目录";
            // 
            // simpleButton5
            // 
            this.simpleButton5.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton5.Image")));
            this.simpleButton5.Location = new System.Drawing.Point(358, 32);
            this.simpleButton5.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(63, 25);
            this.simpleButton5.TabIndex = 19;
            this.simpleButton5.Text = "2导出";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // beTmpDir
            // 
            this.beTmpDir.Location = new System.Drawing.Point(9, 33);
            this.beTmpDir.Margin = new System.Windows.Forms.Padding(2);
            this.beTmpDir.Name = "beTmpDir";
            this.beTmpDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beTmpDir.Size = new System.Drawing.Size(232, 20);
            this.beTmpDir.TabIndex = 18;
            this.beTmpDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beTmpDir_ButtonClick);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(253, 31);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(93, 25);
            this.simpleButton3.TabIndex = 17;
            this.simpleButton3.Text = "1数据准备";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // chkByXzqDo
            // 
            this.chkByXzqDo.EditValue = true;
            this.chkByXzqDo.Location = new System.Drawing.Point(161, 368);
            this.chkByXzqDo.Name = "chkByXzqDo";
            this.chkByXzqDo.Properties.Caption = "数据量大的时候，按XZQ分片处理";
            this.chkByXzqDo.Size = new System.Drawing.Size(268, 19);
            this.chkByXzqDo.TabIndex = 19;
            // 
            // VCTOutput3Frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 550);
            this.Controls.Add(this.chkByXzqDo);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txtDH);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.beDestVCTfile);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VCTOutput3Frm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出VCT3";
            this.Load += new System.EventHandler(this.VCTOutput3Frm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestVCTfile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.beTmpDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkByXzqDo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.ButtonEdit beDestVCTfile;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtDH;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.ButtonEdit beTmpDir;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.CheckEdit chkByXzqDo;
    }
}