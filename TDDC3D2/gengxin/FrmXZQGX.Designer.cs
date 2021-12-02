namespace TDDC3D.gengxin
{
    partial class FrmXZQGX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmXZQGX));
            this.txtXZQBH = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnDLTBGX = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtXZQBH
            // 
            this.txtXZQBH.Location = new System.Drawing.Point(147, 38);
            this.txtXZQBH.Name = "txtXZQBH";
            this.txtXZQBH.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtXZQBH.Properties.ReadOnly = true;
            this.txtXZQBH.Size = new System.Drawing.Size(345, 24);
            this.txtXZQBH.TabIndex = 19;
            this.txtXZQBH.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtXZQBH_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(38, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 18);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "变化行政区：";
            // 
            // btnDLTBGX
            // 
            this.btnDLTBGX.Location = new System.Drawing.Point(242, 85);
            this.btnDLTBGX.Name = "btnDLTBGX";
            this.btnDLTBGX.Size = new System.Drawing.Size(115, 40);
            this.btnDLTBGX.TabIndex = 17;
            this.btnDLTBGX.Text = "生成更新层";
            this.btnDLTBGX.Click += new System.EventHandler(this.btnDLTBGX_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(38, 145);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(454, 200);
            this.info.TabIndex = 16;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(388, 85);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 40);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmXZQGX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 383);
            this.Controls.Add(this.txtXZQBH);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnDLTBGX);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmXZQGX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成行政区更新图层";
            this.Load += new System.EventHandler(this.FrmXZQGX_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtXZQBH;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnDLTBGX;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}