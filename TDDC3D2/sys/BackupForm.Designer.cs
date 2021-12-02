namespace TDDC3D.sys
{
    partial class BackupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupForm));
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.beFileGdb = new DevExpress.XtraEditors.ButtonEdit();
            this.beMdb = new DevExpress.XtraEditors.ButtonEdit();
            this.beShp = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.beDestDir = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.lblstatus = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beFileGdb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beMdb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestDir.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(9, 11);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "FileGDB备份"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "PGDB备份"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "SHP备份")});
            this.radioGroup1.Size = new System.Drawing.Size(384, 121);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // beFileGdb
            // 
            this.beFileGdb.Location = new System.Drawing.Point(106, 24);
            this.beFileGdb.Margin = new System.Windows.Forms.Padding(2);
            this.beFileGdb.Name = "beFileGdb";
            this.beFileGdb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beFileGdb.Size = new System.Drawing.Size(268, 20);
            this.beFileGdb.TabIndex = 1;
            this.beFileGdb.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beFileGdb_ButtonClick);
            // 
            // beMdb
            // 
            this.beMdb.Enabled = false;
            this.beMdb.Location = new System.Drawing.Point(106, 58);
            this.beMdb.Margin = new System.Windows.Forms.Padding(2);
            this.beMdb.Name = "beMdb";
            this.beMdb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beMdb.Size = new System.Drawing.Size(268, 20);
            this.beMdb.TabIndex = 2;
            this.beMdb.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beMdb_ButtonClick);
            // 
            // beShp
            // 
            this.beShp.Enabled = false;
            this.beShp.Location = new System.Drawing.Point(106, 93);
            this.beShp.Margin = new System.Windows.Forms.Padding(2);
            this.beShp.Name = "beShp";
            this.beShp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beShp.Size = new System.Drawing.Size(268, 20);
            this.beShp.TabIndex = 3;
            this.beShp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beShp_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 160);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "目标文件夹";
            // 
            // beDestDir
            // 
            this.beDestDir.Location = new System.Drawing.Point(106, 155);
            this.beDestDir.Margin = new System.Windows.Forms.Padding(2);
            this.beDestDir.Name = "beDestDir";
            this.beDestDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestDir.Size = new System.Drawing.Size(268, 20);
            this.beDestDir.TabIndex = 5;
            this.beDestDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit4_ButtonClick);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(147, 206);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(64, 26);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "开始";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(244, 206);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(82, 26);
            this.simpleButton2.TabIndex = 7;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // lblstatus
            // 
            this.lblstatus.Location = new System.Drawing.Point(21, 246);
            this.lblstatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(0, 14);
            this.lblstatus.TabIndex = 8;
            // 
            // BackupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 268);
            this.Controls.Add(this.lblstatus);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.beDestDir);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.beShp);
            this.Controls.Add(this.beMdb);
            this.Controls.Add(this.beFileGdb);
            this.Controls.Add(this.radioGroup1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackupForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据备份";
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beFileGdb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beMdb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestDir.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.ButtonEdit beFileGdb;
        private DevExpress.XtraEditors.ButtonEdit beMdb;
        private DevExpress.XtraEditors.ButtonEdit beShp;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit beDestDir;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl lblstatus;
    }
}