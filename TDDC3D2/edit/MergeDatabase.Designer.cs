namespace TDDC3D.edit
{
    partial class MergeDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeDatabase));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPath = new DevExpress.XtraEditors.ButtonEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtInfo = new DevExpress.XtraEditors.MemoEdit();
            this.btnMerge = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuit = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInfo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 23);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "数据库目录：";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(94, 21);
            this.txtPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtPath.Name = "txtPath";
            this.txtPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtPath.Size = new System.Drawing.Size(398, 20);
            this.txtPath.TabIndex = 1;
            this.txtPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPath_ButtonClick);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtInfo);
            this.groupControl1.Location = new System.Drawing.Point(22, 58);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(369, 227);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "合库信息提示";
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Location = new System.Drawing.Point(2, 22);
            this.txtInfo.Margin = new System.Windows.Forms.Padding(2);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Properties.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(365, 203);
            this.txtInfo.TabIndex = 0;
            this.txtInfo.UseOptimizedRendering = true;
            // 
            // btnMerge
            // 
            this.btnMerge.Image = ((System.Drawing.Image)(resources.GetObject("btnMerge.Image")));
            this.btnMerge.Location = new System.Drawing.Point(442, 211);
            this.btnMerge.Margin = new System.Windows.Forms.Padding(2);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(64, 25);
            this.btnMerge.TabIndex = 3;
            this.btnMerge.Text = "合并";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Image = ((System.Drawing.Image)(resources.GetObject("btnQuit.Image")));
            this.btnQuit.Location = new System.Drawing.Point(442, 250);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(2);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(64, 25);
            this.btnQuit.TabIndex = 4;
            this.btnQuit.Text = "退出";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelControl2.Location = new System.Drawing.Point(408, 70);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(206, 84);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "数据库目录选择标准库文件所在目录。\r\n\r\n标准库支持FileGDB，MDB，SHP格式。\r\n\r\n如果是SHP，需要将SHP按标准命名，\r\n同时放到数据库目录的子目" +
                "录中。";
            // 
            // MergeDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 307);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MergeDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "快速合库";
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtInfo.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit txtPath;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.MemoEdit txtInfo;
        private DevExpress.XtraEditors.SimpleButton btnMerge;
        private DevExpress.XtraEditors.SimpleButton btnQuit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}