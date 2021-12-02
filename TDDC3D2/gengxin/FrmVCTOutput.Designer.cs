namespace TDDC3D.gengxin
{
    partial class FrmVCTOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVCTOutput));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnDataExport = new DevExpress.XtraEditors.SimpleButton();
            this.beTmpDir = new DevExpress.XtraEditors.ButtonEdit();
            this.btnDataConvert = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportIndex = new DevExpress.XtraEditors.SimpleButton();
            this.txtDH = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.beDestVCTfile = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtXian = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beTmpDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestVCTfile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnDataExport);
            this.groupControl1.Controls.Add(this.beTmpDir);
            this.groupControl1.Controls.Add(this.btnDataConvert);
            this.groupControl1.Location = new System.Drawing.Point(27, 474);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(572, 85);
            this.groupControl1.TabIndex = 27;
            this.groupControl1.Text = "临时目录";
            // 
            // btnDataExport
            // 
            this.btnDataExport.Image = ((System.Drawing.Image)(resources.GetObject("btnDataExport.Image")));
            this.btnDataExport.Location = new System.Drawing.Point(477, 40);
            this.btnDataExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDataExport.Name = "btnDataExport";
            this.btnDataExport.Size = new System.Drawing.Size(84, 31);
            this.btnDataExport.TabIndex = 19;
            this.btnDataExport.Text = "2导出";
            this.btnDataExport.Click += new System.EventHandler(this.btnDataExport_Click);
            // 
            // beTmpDir
            // 
            this.beTmpDir.Location = new System.Drawing.Point(12, 41);
            this.beTmpDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.beTmpDir.Name = "beTmpDir";
            this.beTmpDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beTmpDir.Properties.ReadOnly = true;
            this.beTmpDir.Size = new System.Drawing.Size(309, 24);
            this.beTmpDir.TabIndex = 18;
            this.beTmpDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beTmpDir_ButtonClick);
            // 
            // btnDataConvert
            // 
            this.btnDataConvert.Image = ((System.Drawing.Image)(resources.GetObject("btnDataConvert.Image")));
            this.btnDataConvert.Location = new System.Drawing.Point(337, 39);
            this.btnDataConvert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDataConvert.Name = "btnDataConvert";
            this.btnDataConvert.Size = new System.Drawing.Size(124, 31);
            this.btnDataConvert.TabIndex = 17;
            this.btnDataConvert.Text = "1数据准备";
            this.btnDataConvert.Click += new System.EventHandler(this.btnDataConvert_Click);
            // 
            // btnExportIndex
            // 
            this.btnExportIndex.Image = ((System.Drawing.Image)(resources.GetObject("btnExportIndex.Image")));
            this.btnExportIndex.Location = new System.Drawing.Point(345, 582);
            this.btnExportIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExportIndex.Name = "btnExportIndex";
            this.btnExportIndex.Size = new System.Drawing.Size(148, 31);
            this.btnExportIndex.TabIndex = 26;
            this.btnExportIndex.Text = "导出索引文件";
            this.btnExportIndex.Click += new System.EventHandler(this.btnExportIndex_Click);
            // 
            // txtDH
            // 
            this.txtDH.EditValue = "38";
            this.txtDH.Location = new System.Drawing.Point(79, 579);
            this.txtDH.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDH.Name = "txtDH";
            this.txtDH.Size = new System.Drawing.Size(100, 24);
            this.txtDH.TabIndex = 25;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 584);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(45, 18);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "带号：";
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(500, 582);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 31);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(27, 110);
            this.memoLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(571, 333);
            this.memoLog.TabIndex = 22;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // beDestVCTfile
            // 
            this.beDestVCTfile.Location = new System.Drawing.Point(142, 63);
            this.beDestVCTfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.beDestVCTfile.Name = "beDestVCTfile";
            this.beDestVCTfile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestVCTfile.Size = new System.Drawing.Size(457, 24);
            this.beDestVCTfile.TabIndex = 21;
            this.beDestVCTfile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestVCTfile_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(27, 67);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(103, 18);
            this.labelControl1.TabIndex = 20;
            this.labelControl1.Text = "目标VCT文件：";
            // 
            // txtXian
            // 
            this.txtXian.Location = new System.Drawing.Point(142, 12);
            this.txtXian.Name = "txtXian";
            this.txtXian.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtXian.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtXian.Size = new System.Drawing.Size(457, 24);
            this.txtXian.TabIndex = 38;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(27, 15);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(80, 18);
            this.labelControl3.TabIndex = 37;
            this.labelControl3.Text = "当前县代码:";
            // 
            // FrmVCTOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 632);
            this.Controls.Add(this.txtXian);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnExportIndex);
            this.Controls.Add(this.txtDH);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.beDestVCTfile);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmVCTOutput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "更新层导出VCT";
            this.Load += new System.EventHandler(this.FrmVCTOutput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.beTmpDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestVCTfile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnDataExport;
        private DevExpress.XtraEditors.ButtonEdit beTmpDir;
        private DevExpress.XtraEditors.SimpleButton btnDataConvert;
        private DevExpress.XtraEditors.SimpleButton btnExportIndex;
        private DevExpress.XtraEditors.TextEdit txtDH;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.ButtonEdit beDestVCTfile;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit txtXian;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}