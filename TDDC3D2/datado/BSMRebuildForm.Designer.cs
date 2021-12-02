namespace TDDC3D.datado
{
    partial class BSMRebuildForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BSMRebuildForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkListFCs = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtXian = new DevExpress.XtraEditors.TextEdit();
            this.chkSelectAll = new DevExpress.XtraEditors.CheckEdit();
            this.btnLSYD = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chkListFCs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 58);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(165, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择要重建标识码的要素";
            // 
            // chkListFCs
            // 
            this.chkListFCs.Location = new System.Drawing.Point(24, 91);
            this.chkListFCs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkListFCs.Name = "chkListFCs";
            this.chkListFCs.Size = new System.Drawing.Size(493, 222);
            this.chkListFCs.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(390, 342);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(88, 31);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(262, 342);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(87, 31);
            this.simpleButton3.TabIndex = 4;
            this.simpleButton3.Text = "执行";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(24, 16);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "当前县代码";
            // 
            // txtXian
            // 
            this.txtXian.Location = new System.Drawing.Point(123, 12);
            this.txtXian.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtXian.Name = "txtXian";
            this.txtXian.Size = new System.Drawing.Size(396, 24);
            this.txtXian.TabIndex = 6;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.EditValue = true;
            this.chkSelectAll.Location = new System.Drawing.Point(24, 347);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Properties.Caption = "全选/全不选";
            this.chkSelectAll.Size = new System.Drawing.Size(139, 22);
            this.chkSelectAll.TabIndex = 7;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnLSYD
            // 
            this.btnLSYD.Location = new System.Drawing.Point(24, 386);
            this.btnLSYD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLSYD.Name = "btnLSYD";
            this.btnLSYD.Size = new System.Drawing.Size(493, 31);
            this.btnLSYD.TabIndex = 8;
            this.btnLSYD.Text = "临时用地关联图斑标识码赋值";
            this.btnLSYD.Click += new System.EventHandler(this.btnLSYD_Click);
            // 
            // BSMRebuildForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 443);
            this.Controls.Add(this.btnLSYD);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.txtXian);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.chkListFCs);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BSMRebuildForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标识码重建";
            this.Load += new System.EventHandler(this.BSMRebuildForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkListFCs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListFCs;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtXian;
        private DevExpress.XtraEditors.CheckEdit chkSelectAll;
        private DevExpress.XtraEditors.SimpleButton btnLSYD;
    }
}