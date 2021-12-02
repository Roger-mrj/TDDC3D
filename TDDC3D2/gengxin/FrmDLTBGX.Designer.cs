namespace TDDC3D.gengxin
{
    partial class FrmDLTBGX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDLTBGX));
            this.btnDLTBGX = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.txtDLTBBH = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnMergeSameDLBM = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDLTBBH.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDLTBGX
            // 
            this.btnDLTBGX.Location = new System.Drawing.Point(94, 73);
            this.btnDLTBGX.Name = "btnDLTBGX";
            this.btnDLTBGX.Size = new System.Drawing.Size(115, 40);
            this.btnDLTBGX.TabIndex = 11;
            this.btnDLTBGX.Text = "生成更新层";
            this.btnDLTBGX.Click += new System.EventHandler(this.btnDLTBGX_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(29, 133);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(454, 200);
            this.info.TabIndex = 10;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(379, 73);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 40);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtDLTBBH
            // 
            this.txtDLTBBH.Location = new System.Drawing.Point(110, 26);
            this.txtDLTBBH.Name = "txtDLTBBH";
            this.txtDLTBBH.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDLTBBH.Properties.ReadOnly = true;
            this.txtDLTBBH.Size = new System.Drawing.Size(373, 24);
            this.txtDLTBBH.TabIndex = 13;
            this.txtDLTBBH.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtDLTBBH_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(29, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "变化图斑：";
            // 
            // btnMergeSameDLBM
            // 
            this.btnMergeSameDLBM.Location = new System.Drawing.Point(224, 73);
            this.btnMergeSameDLBM.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMergeSameDLBM.Name = "btnMergeSameDLBM";
            this.btnMergeSameDLBM.Size = new System.Drawing.Size(140, 40);
            this.btnMergeSameDLBM.TabIndex = 14;
            this.btnMergeSameDLBM.Text = "合并同地类图斑";
            this.btnMergeSameDLBM.Click += new System.EventHandler(this.btnMergeSameDLBM_Click);
            // 
            // FrmDLTBGX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 365);
            this.Controls.Add(this.btnMergeSameDLBM);
            this.Controls.Add(this.txtDLTBBH);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnDLTBGX);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDLTBGX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成地类图斑更新层数据";
            this.Load += new System.EventHandler(this.FrmDLTBGX_Load);
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDLTBBH.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnDLTBGX;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.ButtonEdit txtDLTBBH;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnMergeSameDLBM;
    }
}