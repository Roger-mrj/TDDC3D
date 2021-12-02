namespace TDDC3D.gengxin
{
    partial class FrmSCNMSJK
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
            this.txtDBPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCreateDB = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.txtXZQDM = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQDM.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDBPath
            // 
            this.txtDBPath.Location = new System.Drawing.Point(178, 66);
            this.txtDBPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDBPath.Name = "txtDBPath";
            this.txtDBPath.Properties.AllowFocused = false;
            this.txtDBPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDBPath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtDBPath.Size = new System.Drawing.Size(292, 24);
            this.txtDBPath.TabIndex = 0;
            this.txtDBPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtDBPath_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(39, 68);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(110, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "生成数据库路径:";
            // 
            // btnCreateDB
            // 
            this.btnCreateDB.Location = new System.Drawing.Point(260, 108);
            this.btnCreateDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreateDB.Name = "btnCreateDB";
            this.btnCreateDB.Size = new System.Drawing.Size(95, 41);
            this.btnCreateDB.TabIndex = 2;
            this.btnCreateDB.Text = "生成";
            this.btnCreateDB.Click += new System.EventHandler(this.btnCreateDB_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(375, 108);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(95, 41);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(39, 170);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(431, 200);
            this.info.TabIndex = 27;
            this.info.UseOptimizedRendering = true;
            // 
            // txtXZQDM
            // 
            this.txtXZQDM.Location = new System.Drawing.Point(178, 12);
            this.txtXZQDM.Name = "txtXZQDM";
            this.txtXZQDM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtXZQDM.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtXZQDM.Size = new System.Drawing.Size(292, 24);
            this.txtXZQDM.TabIndex = 48;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(39, 15);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 47;
            this.labelControl2.Text = "县级代码：";
            // 
            // FrmSCNMSJK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 388);
            this.Controls.Add(this.txtXZQDM);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.info);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.btnCreateDB);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtDBPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSCNMSJK";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成年末数据库";
            this.Load += new System.EventHandler(this.FrmSCNMSJK_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtDBPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQDM.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtDBPath;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCreateDB;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.ComboBoxEdit txtXZQDM;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}