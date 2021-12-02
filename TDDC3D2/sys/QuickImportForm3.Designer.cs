namespace TDDC3D.sys
{
    partial class QuickImportForm3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickImportForm3));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLocalFeatureclasses = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.beFGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.beShpSrc = new DevExpress.XtraEditors.ButtonEdit();
            this.bePGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beFGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShpSrc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bePGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lblStatus);
            this.panelControl3.Controls.Add(this.simpleButton2);
            this.panelControl3.Controls.Add(this.cmdCancel);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 236);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(721, 68);
            this.panelControl3.TabIndex = 13;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(2, 48);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(30, 18);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "状态";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(487, 19);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(100, 29);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "导入";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.Location = new System.Drawing.Point(595, 19);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(100, 29);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "关闭";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.simpleButton1);
            this.groupControl2.Controls.Add(this.cmbLocalFeatureclasses);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl2.Location = new System.Drawing.Point(0, 151);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(721, 85);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "目标数据集";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(83, 42);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(36, 22);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "...";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cmbLocalFeatureclasses
            // 
            this.cmbLocalFeatureclasses.Location = new System.Drawing.Point(147, 41);
            this.cmbLocalFeatureclasses.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLocalFeatureclasses.Name = "cmbLocalFeatureclasses";
            this.cmbLocalFeatureclasses.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLocalFeatureclasses.Size = new System.Drawing.Size(549, 24);
            this.cmbLocalFeatureclasses.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(27, 45);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "标准库";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.beFGDB);
            this.groupControl1.Controls.Add(this.beShpSrc);
            this.groupControl1.Controls.Add(this.bePGDB);
            this.groupControl1.Controls.Add(this.radioGroup1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(721, 151);
            this.groupControl1.TabIndex = 21;
            this.groupControl1.Text = "源数据库";
            // 
            // beFGDB
            // 
            this.beFGDB.Enabled = false;
            this.beFGDB.Location = new System.Drawing.Point(117, 74);
            this.beFGDB.Margin = new System.Windows.Forms.Padding(4);
            this.beFGDB.Name = "beFGDB";
            this.beFGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beFGDB.Size = new System.Drawing.Size(579, 24);
            this.beFGDB.TabIndex = 23;
            this.beFGDB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beFGDB_ButtonClick);
            // 
            // beShpSrc
            // 
            this.beShpSrc.Location = new System.Drawing.Point(117, 38);
            this.beShpSrc.Margin = new System.Windows.Forms.Padding(4);
            this.beShpSrc.Name = "beShpSrc";
            this.beShpSrc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beShpSrc.Size = new System.Drawing.Size(579, 24);
            this.beShpSrc.TabIndex = 22;
            this.beShpSrc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beShpSrc_ButtonClick);
            // 
            // bePGDB
            // 
            this.bePGDB.Location = new System.Drawing.Point(117, 110);
            this.bePGDB.Margin = new System.Windows.Forms.Padding(4);
            this.bePGDB.Name = "bePGDB";
            this.bePGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bePGDB.Size = new System.Drawing.Size(579, 24);
            this.bePGDB.TabIndex = 19;
            this.bePGDB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.bePGDB_ButtonClick);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioGroup1.Location = new System.Drawing.Point(2, 26);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "SHP文件夹"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "FileGDB"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "PGDB")});
            this.radioGroup1.Size = new System.Drawing.Size(717, 125);
            this.radioGroup1.TabIndex = 21;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // QuickImportForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 304);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panelControl3);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickImportForm3";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "一键导入二调数据库";
            this.Load += new System.EventHandler(this.QuickImportForm2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.beFGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShpSrc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bePGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLocalFeatureclasses;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ButtonEdit bePGDB;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.ButtonEdit beFGDB;
        private DevExpress.XtraEditors.ButtonEdit beShpSrc;
    }
}