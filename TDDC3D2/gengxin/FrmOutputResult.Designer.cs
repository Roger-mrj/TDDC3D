namespace TDDC3D.gengxin
{
    partial class FrmOutputResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOutputResult));
            this.chkOnlyReport = new DevExpress.XtraEditors.CheckEdit();
            this.txtMetaData = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.txtExportPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKzmjLD = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.buttonEdit1 = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.filePathBB = new DevExpress.XtraEditors.ButtonEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.txttzhMJ = new DevExpress.XtraEditors.TextEdit();
            this.txtXZQDM = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyReport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMetaData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExportPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmjLD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePathBB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttzhMJ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQDM.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkOnlyReport
            // 
            this.chkOnlyReport.Location = new System.Drawing.Point(31, 784);
            this.chkOnlyReport.Margin = new System.Windows.Forms.Padding(4);
            this.chkOnlyReport.Name = "chkOnlyReport";
            this.chkOnlyReport.Properties.Caption = "只导出成果表";
            this.chkOnlyReport.Size = new System.Drawing.Size(131, 22);
            this.chkOnlyReport.TabIndex = 32;
            // 
            // txtMetaData
            // 
            this.txtMetaData.Location = new System.Drawing.Point(133, 86);
            this.txtMetaData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMetaData.Name = "txtMetaData";
            this.txtMetaData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtMetaData.Properties.ReadOnly = true;
            this.txtMetaData.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtMetaData.Size = new System.Drawing.Size(541, 24);
            this.txtMetaData.TabIndex = 30;
            this.txtMetaData.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtMetaData_ButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(30, 89);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 18);
            this.labelControl3.TabIndex = 29;
            this.labelControl3.Text = "元数据文件：";
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(30, 294);
            this.memoLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Properties.ReadOnly = true;
            this.memoLog.Size = new System.Drawing.Size(659, 467);
            this.memoLog.TabIndex = 28;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(604, 774);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 42);
            this.btnClose.TabIndex = 27;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(499, 774);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(85, 42);
            this.btnExport.TabIndex = 26;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtExportPath
            // 
            this.txtExportPath.Location = new System.Drawing.Point(133, 47);
            this.txtExportPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtExportPath.Properties.ReadOnly = true;
            this.txtExportPath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtExportPath.Size = new System.Drawing.Size(541, 24);
            this.txtExportPath.TabIndex = 25;
            this.txtExportPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtExportPath_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(30, 50);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 24;
            this.labelControl2.Text = "成果目录：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(30, 11);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 22;
            this.labelControl1.Text = "县级代码：";
            // 
            // txtKzmjLD
            // 
            this.txtKzmjLD.Location = new System.Drawing.Point(425, 173);
            this.txtKzmjLD.Name = "txtKzmjLD";
            this.txtKzmjLD.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtKzmjLD.Properties.Mask.EditMask = "([0-9]{1,}[.][0-9]*)";
            this.txtKzmjLD.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtKzmjLD.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtKzmjLD.Size = new System.Drawing.Size(246, 24);
            this.txtKzmjLD.TabIndex = 37;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(218, 176);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(165, 18);
            this.labelControl6.TabIndex = 35;
            this.labelControl6.Text = "陆地控制面积（公顷）：";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(169, 786);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(105, 18);
            this.labelControl5.TabIndex = 38;
            this.labelControl5.Text = "数据变更年份：";
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(280, 783);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.dateEdit1.Size = new System.Drawing.Size(180, 24);
            this.dateEdit1.TabIndex = 39;
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(133, 125);
            this.buttonEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.buttonEdit1.Properties.ReadOnly = true;
            this.buttonEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.buttonEdit1.Size = new System.Drawing.Size(541, 24);
            this.buttonEdit1.TabIndex = 41;
            this.buttonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(30, 128);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(90, 18);
            this.labelControl4.TabIndex = 40;
            this.labelControl4.Text = "基础数据包：";
            // 
            // filePathBB
            // 
            this.filePathBB.Enabled = false;
            this.filePathBB.Location = new System.Drawing.Point(218, 204);
            this.filePathBB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filePathBB.Name = "filePathBB";
            this.filePathBB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.filePathBB.Properties.ReadOnly = true;
            this.filePathBB.Size = new System.Drawing.Size(456, 24);
            this.filePathBB.TabIndex = 43;
            this.filePathBB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.filePathBB_ButtonClick);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(30, 167);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "年初面积取自图斑", false),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "年初面积取自报表")});
            this.radioGroup1.Size = new System.Drawing.Size(168, 68);
            this.radioGroup1.TabIndex = 42;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(31, 256);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "若有行政区调整，请输入调整后控制面积（公顷）：";
            this.checkEdit1.Size = new System.Drawing.Size(388, 22);
            this.checkEdit1.TabIndex = 44;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // txttzhMJ
            // 
            this.txttzhMJ.Location = new System.Drawing.Point(425, 256);
            this.txttzhMJ.Name = "txttzhMJ";
            this.txttzhMJ.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txttzhMJ.Properties.Mask.EditMask = "([0-9]{1,}[.][0-9]*)";
            this.txttzhMJ.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txttzhMJ.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txttzhMJ.Size = new System.Drawing.Size(249, 24);
            this.txttzhMJ.TabIndex = 45;
            // 
            // txtXZQDM
            // 
            this.txtXZQDM.Location = new System.Drawing.Point(133, 8);
            this.txtXZQDM.Name = "txtXZQDM";
            this.txtXZQDM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtXZQDM.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtXZQDM.Size = new System.Drawing.Size(541, 24);
            this.txtXZQDM.TabIndex = 46;
            // 
            // FrmOutputResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 829);
            this.Controls.Add(this.txtXZQDM);
            this.Controls.Add(this.txttzhMJ);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.filePathBB);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.dateEdit1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.txtKzmjLD);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.chkOnlyReport);
            this.Controls.Add(this.txtMetaData);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtExportPath);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FrmOutputResult";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输出更新层成果包";
            this.Load += new System.EventHandler(this.FrmOutputResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkOnlyReport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMetaData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExportPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmjLD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePathBB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttzhMJ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQDM.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkOnlyReport;
        private DevExpress.XtraEditors.ButtonEdit txtMetaData;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.ButtonEdit txtExportPath;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtKzmjLD;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ButtonEdit filePathBB;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.TextEdit txttzhMJ;
        private DevExpress.XtraEditors.ComboBoxEdit txtXZQDM;

    }
}