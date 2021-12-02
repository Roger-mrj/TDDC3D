namespace TDDC3D.gdzy
{
    partial class FrmBSMGD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBSMGD));
            this.chkSelectAll = new DevExpress.XtraEditors.CheckEdit();
            this.txtXian = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkListFCs = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkListFCs)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.EditValue = true;
            this.chkSelectAll.Location = new System.Drawing.Point(23, 358);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Properties.Caption = "全选/全不选";
            this.chkSelectAll.Size = new System.Drawing.Size(139, 22);
            this.chkSelectAll.TabIndex = 14;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // txtXian
            // 
            this.txtXian.Location = new System.Drawing.Point(122, 23);
            this.txtXian.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtXian.Name = "txtXian";
            this.txtXian.Size = new System.Drawing.Size(396, 24);
            this.txtXian.TabIndex = 13;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(23, 27);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "当前县代码";
            // 
            // chkListFCs
            // 
            this.chkListFCs.Location = new System.Drawing.Point(23, 102);
            this.chkListFCs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkListFCs.Name = "chkListFCs";
            this.chkListFCs.Size = new System.Drawing.Size(493, 222);
            this.chkListFCs.TabIndex = 9;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 69);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(165, 18);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "选择要重建标识码的要素";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(243, 353);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 31);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "执行";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(371, 353);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 31);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmBSMGD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 417);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.txtXian);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkListFCs);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBSMGD";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "重建标识码";
            this.Load += new System.EventHandler(this.FrmBSMOpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkListFCs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkSelectAll;
        private DevExpress.XtraEditors.TextEdit txtXian;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListFCs;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}