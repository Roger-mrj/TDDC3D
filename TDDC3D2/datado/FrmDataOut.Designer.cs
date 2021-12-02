namespace TDDC3D.datado
{
    partial class FrmDataOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDataOut));
            this.lstXZQ = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.rdoXZQ = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtPath = new DevExpress.XtraEditors.ButtonEdit();
            this.chkSelectAll = new DevExpress.XtraEditors.CheckEdit();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.lstXZQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoXZQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lstXZQ
            // 
            this.lstXZQ.Location = new System.Drawing.Point(12, 12);
            this.lstXZQ.Name = "lstXZQ";
            this.lstXZQ.Size = new System.Drawing.Size(279, 262);
            this.lstXZQ.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(310, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "分库原则：";
            // 
            // rdoXZQ
            // 
            this.rdoXZQ.Location = new System.Drawing.Point(376, 19);
            this.rdoXZQ.Name = "rdoXZQ";
            this.rdoXZQ.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoXZQ.Properties.Appearance.Options.UseBackColor = true;
            this.rdoXZQ.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoXZQ.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "乡"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "村")});
            this.rdoXZQ.Size = new System.Drawing.Size(100, 32);
            this.rdoXZQ.TabIndex = 2;
            this.rdoXZQ.SelectedIndexChanged += new System.EventHandler(this.rdoXZQ_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(310, 59);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "输出目录：";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(310, 79);
            this.txtPath.Name = "txtPath";
            this.txtPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPath.Properties.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(166, 20);
            this.txtPath.TabIndex = 4;
            this.txtPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPath_ButtonClick);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Location = new System.Drawing.Point(310, 123);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Properties.Caption = "全选";
            this.chkSelectAll.Size = new System.Drawing.Size(75, 19);
            this.chkSelectAll.TabIndex = 5;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(339, 233);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 29);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "分库";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Image = ((System.Drawing.Image)(resources.GetObject("btnQuit.Image")));
            this.btnQuit.Location = new System.Drawing.Point(416, 233);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(60, 29);
            this.btnQuit.TabIndex = 7;
            this.btnQuit.Text = "关闭";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // FrmDataOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 286);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.rdoXZQ);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lstXZQ);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDataOut";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "一键分库";
            this.Load += new System.EventHandler(this.FrmDataOut_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lstXZQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoXZQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckedListBoxControl lstXZQ;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.RadioGroup rdoXZQ;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit txtPath;
        private DevExpress.XtraEditors.CheckEdit chkSelectAll;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnQuit;
    }
}