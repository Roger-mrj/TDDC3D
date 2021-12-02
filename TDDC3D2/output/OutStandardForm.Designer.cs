namespace TDDC3D.output
{
    partial class OutStandardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutStandardForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtXZQDM = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtExportPath = new DevExpress.XtraEditors.ButtonEdit();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.txtMetaData = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chkByXzqDo = new DevExpress.XtraEditors.CheckEdit();
            this.chkOnlyReport = new DevExpress.XtraEditors.CheckEdit();
            this.txtKzmjHD = new DevExpress.XtraEditors.TextEdit();
            this.txtKzmjLD = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQDM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExportPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMetaData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkByXzqDo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyReport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmjHD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmjLD.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 25);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "县级代码：";
            // 
            // txtXZQDM
            // 
            this.txtXZQDM.Location = new System.Drawing.Point(115, 22);
            this.txtXZQDM.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtXZQDM.Name = "txtXZQDM";
            this.txtXZQDM.Properties.Mask.EditMask = "[0-9]*";
            this.txtXZQDM.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtXZQDM.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtXZQDM.Size = new System.Drawing.Size(541, 24);
            this.txtXZQDM.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 61);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "成果目录：";
            // 
            // txtExportPath
            // 
            this.txtExportPath.Location = new System.Drawing.Point(115, 58);
            this.txtExportPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtExportPath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtExportPath.Size = new System.Drawing.Size(541, 24);
            this.txtExportPath.TabIndex = 3;
            this.txtExportPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtExportPath_ButtonClick);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(385, 743);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(85, 42);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(487, 743);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 42);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(12, 184);
            this.memoLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(659, 502);
            this.memoLog.TabIndex = 8;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // txtMetaData
            // 
            this.txtMetaData.Location = new System.Drawing.Point(113, 94);
            this.txtMetaData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMetaData.Name = "txtMetaData";
            this.txtMetaData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtMetaData.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtMetaData.Size = new System.Drawing.Size(541, 24);
            this.txtMetaData.TabIndex = 10;
            this.txtMetaData.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtMetaData_ButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 96);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 18);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "元数据文件：";
            // 
            // chkByXzqDo
            // 
            this.chkByXzqDo.EditValue = true;
            this.chkByXzqDo.Location = new System.Drawing.Point(115, 132);
            this.chkByXzqDo.Margin = new System.Windows.Forms.Padding(4);
            this.chkByXzqDo.Name = "chkByXzqDo";
            this.chkByXzqDo.Properties.Caption = "数据量大的时候，按XZQ分片处理";
            this.chkByXzqDo.Size = new System.Drawing.Size(289, 22);
            this.chkByXzqDo.TabIndex = 20;
            // 
            // chkOnlyReport
            // 
            this.chkOnlyReport.Location = new System.Drawing.Point(113, 753);
            this.chkOnlyReport.Margin = new System.Windows.Forms.Padding(4);
            this.chkOnlyReport.Name = "chkOnlyReport";
            this.chkOnlyReport.Properties.Caption = "只导出成果表";
            this.chkOnlyReport.Size = new System.Drawing.Size(181, 22);
            this.chkOnlyReport.TabIndex = 21;
            // 
            // txtKzmjHD
            // 
            this.txtKzmjHD.Location = new System.Drawing.Point(523, 708);
            this.txtKzmjHD.Name = "txtKzmjHD";
            this.txtKzmjHD.Properties.Mask.EditMask = "([0-9]{1,}[.][0-9]*)";
            this.txtKzmjHD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtKzmjHD.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtKzmjHD.Size = new System.Drawing.Size(129, 24);
            this.txtKzmjHD.TabIndex = 26;
            // 
            // txtKzmjLD
            // 
            this.txtKzmjLD.Location = new System.Drawing.Point(194, 708);
            this.txtKzmjLD.Name = "txtKzmjLD";
            this.txtKzmjLD.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtKzmjLD.Properties.Mask.EditMask = "([0-9]{1,}[.][0-9]*)";
            this.txtKzmjLD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtKzmjLD.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtKzmjLD.Size = new System.Drawing.Size(136, 24);
            this.txtKzmjLD.TabIndex = 25;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(344, 711);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(165, 18);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "海岛控制面积（公顷）：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(15, 711);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(165, 18);
            this.labelControl5.TabIndex = 23;
            this.labelControl5.Text = "陆地控制面积（公顷）：";
            // 
            // OutStandardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 796);
            this.Controls.Add(this.txtKzmjHD);
            this.Controls.Add(this.txtKzmjLD);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.chkOnlyReport);
            this.Controls.Add(this.chkByXzqDo);
            this.Controls.Add(this.txtMetaData);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtExportPath);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtXZQDM);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutStandardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输出成果包";
            this.Load += new System.EventHandler(this.OutStandardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQDM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExportPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMetaData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkByXzqDo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyReport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmjHD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmjLD.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtXZQDM;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit txtExportPath;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.ButtonEdit txtMetaData;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit chkByXzqDo;
        private DevExpress.XtraEditors.CheckEdit chkOnlyReport;
        private DevExpress.XtraEditors.TextEdit txtKzmjHD;
        private DevExpress.XtraEditors.TextEdit txtKzmjLD;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
    }
}