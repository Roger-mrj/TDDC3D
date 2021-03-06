namespace TDDC3D.gengxin
{
    partial class FrmXZQGXGC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmXZQGXGC));
            this.btnComputeBSM = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnGXGC = new DevExpress.XtraEditors.SimpleButton();
            this.txtXZQBH = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQBH.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnComputeBSM
            // 
            this.btnComputeBSM.Location = new System.Drawing.Point(416, 106);
            this.btnComputeBSM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComputeBSM.Name = "btnComputeBSM";
            this.btnComputeBSM.Size = new System.Drawing.Size(118, 46);
            this.btnComputeBSM.TabIndex = 15;
            this.btnComputeBSM.Text = "重排标识码";
            this.btnComputeBSM.Click += new System.EventHandler(this.btnComputeBSM_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(36, 180);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(626, 288);
            this.info.TabIndex = 13;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(560, 106);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 46);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGXGC
            // 
            this.btnGXGC.Location = new System.Drawing.Point(248, 106);
            this.btnGXGC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGXGC.Name = "btnGXGC";
            this.btnGXGC.Size = new System.Drawing.Size(143, 46);
            this.btnGXGC.TabIndex = 10;
            this.btnGXGC.Text = "生成更新过程";
            this.btnGXGC.Click += new System.EventHandler(this.btnGXGC_Click);
            // 
            // txtXZQBH
            // 
            this.txtXZQBH.Location = new System.Drawing.Point(160, 37);
            this.txtXZQBH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtXZQBH.Name = "txtXZQBH";
            this.txtXZQBH.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtXZQBH.Properties.ReadOnly = true;
            this.txtXZQBH.Size = new System.Drawing.Size(502, 28);
            this.txtXZQBH.TabIndex = 9;
            this.txtXZQBH.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtXZQBH_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(36, 41);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(108, 22);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "变化行政区：";
            // 
            // FrmXZQGXGC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 512);
            this.Controls.Add(this.btnComputeBSM);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGXGC);
            this.Controls.Add(this.txtXZQBH);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmXZQGXGC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成行政区更新过程图层";
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXZQBH.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnComputeBSM;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnGXGC;
        private DevExpress.XtraEditors.ButtonEdit txtXZQBH;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}