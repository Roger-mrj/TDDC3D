namespace TDDC3D.sys
{
    partial class QuickImportForm2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickImportForm2));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLocalFeatureclasses = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.beShpSrc = new DevExpress.XtraEditors.ButtonEdit();
            this.beFGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.bePGDB = new DevExpress.XtraEditors.ButtonEdit();
            this.chkListAllFC = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.beSqlLite = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShpSrc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beFGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bePGDB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkListAllFC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSqlLite.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lblStatus);
            this.panelControl3.Controls.Add(this.simpleButton2);
            this.panelControl3.Controls.Add(this.cmdCancel);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 378);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(541, 54);
            this.panelControl3.TabIndex = 13;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(2, 38);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(24, 14);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "状态";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(365, 15);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "导入";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.Location = new System.Drawing.Point(446, 15);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
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
            this.groupControl2.Location = new System.Drawing.Point(0, 310);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(541, 68);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "目标数据集";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(62, 34);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(27, 18);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "...";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cmbLocalFeatureclasses
            // 
            this.cmbLocalFeatureclasses.Location = new System.Drawing.Point(110, 33);
            this.cmbLocalFeatureclasses.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbLocalFeatureclasses.Name = "cmbLocalFeatureclasses";
            this.cmbLocalFeatureclasses.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLocalFeatureclasses.Size = new System.Drawing.Size(358, 20);
            this.cmbLocalFeatureclasses.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 36);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "标准库";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioGroup1.Location = new System.Drawing.Point(0, 0);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "SHP文件夹"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "FileGDB"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "PGDB"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "SQLLite")});
            this.radioGroup1.Size = new System.Drawing.Size(541, 118);
            this.radioGroup1.TabIndex = 15;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // beShpSrc
            // 
            this.beShpSrc.Location = new System.Drawing.Point(95, 10);
            this.beShpSrc.Name = "beShpSrc";
            this.beShpSrc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beShpSrc.Size = new System.Drawing.Size(426, 20);
            this.beShpSrc.TabIndex = 16;
            this.beShpSrc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beShpSrc_ButtonClick);
            // 
            // beFGDB
            // 
            this.beFGDB.Enabled = false;
            this.beFGDB.Location = new System.Drawing.Point(95, 36);
            this.beFGDB.Name = "beFGDB";
            this.beFGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beFGDB.Size = new System.Drawing.Size(426, 20);
            this.beFGDB.TabIndex = 17;
            this.beFGDB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beFGDB_ButtonClick);
            // 
            // bePGDB
            // 
            this.bePGDB.Enabled = false;
            this.bePGDB.Location = new System.Drawing.Point(95, 62);
            this.bePGDB.Name = "bePGDB";
            this.bePGDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.bePGDB.Size = new System.Drawing.Size(426, 20);
            this.bePGDB.TabIndex = 18;
            this.bePGDB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.bePGDB_ButtonClick);
            // 
            // chkListAllFC
            // 
            this.chkListAllFC.CheckOnClick = true;
            this.chkListAllFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkListAllFC.Location = new System.Drawing.Point(0, 118);
            this.chkListAllFC.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkListAllFC.Name = "chkListAllFC";
            this.chkListAllFC.Size = new System.Drawing.Size(541, 192);
            this.chkListAllFC.TabIndex = 19;
            // 
            // beSqlLite
            // 
            this.beSqlLite.Enabled = false;
            this.beSqlLite.Location = new System.Drawing.Point(95, 89);
            this.beSqlLite.Name = "beSqlLite";
            this.beSqlLite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beSqlLite.Size = new System.Drawing.Size(426, 20);
            this.beSqlLite.TabIndex = 20;
            this.beSqlLite.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSqlLite_ButtonClick);
            // 
            // QuickImportForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 432);
            this.Controls.Add(this.beSqlLite);
            this.Controls.Add(this.chkListAllFC);
            this.Controls.Add(this.bePGDB);
            this.Controls.Add(this.beFGDB);
            this.Controls.Add(this.beShpSrc);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panelControl3);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickImportForm2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "快速导入要素类（覆盖导入）";
            this.Load += new System.EventHandler(this.QuickImportForm2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShpSrc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beFGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bePGDB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkListAllFC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSqlLite.Properties)).EndInit();
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
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.ButtonEdit beShpSrc;
        private DevExpress.XtraEditors.ButtonEdit beFGDB;
        private DevExpress.XtraEditors.ButtonEdit bePGDB;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListAllFC;
        private DevExpress.XtraEditors.ButtonEdit beSqlLite;
    }
}