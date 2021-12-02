namespace TDDC3D.gengxin
{
    partial class FrmBHDLTB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBHDLTB));
            this.txtBHDLTB = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnDLTBBH = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtBHDLTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBHDLTB
            // 
            this.txtBHDLTB.Location = new System.Drawing.Point(134, 23);
            this.txtBHDLTB.Name = "txtBHDLTB";
            this.txtBHDLTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtBHDLTB.Properties.ReadOnly = true;
            this.txtBHDLTB.Size = new System.Drawing.Size(343, 24);
            this.txtBHDLTB.TabIndex = 30;
            this.txtBHDLTB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtBHDLTB_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(105, 18);
            this.labelControl1.TabIndex = 29;
            this.labelControl1.Text = "变化地类图斑：";
            // 
            // btnDLTBBH
            // 
            this.btnDLTBBH.Location = new System.Drawing.Point(257, 70);
            this.btnDLTBBH.Name = "btnDLTBBH";
            this.btnDLTBBH.Size = new System.Drawing.Size(90, 40);
            this.btnDLTBBH.TabIndex = 28;
            this.btnDLTBBH.Text = "提取";
            this.btnDLTBBH.Click += new System.EventHandler(this.btnDLTBBH_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(23, 130);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(454, 200);
            this.info.TabIndex = 27;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(388, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 40);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmBHDLTB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 353);
            this.Controls.Add(this.txtBHDLTB);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnDLTBBH);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBHDLTB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取村级调查区变化导致的变化图斑";
            ((System.ComponentModel.ISupportInitialize)(this.txtBHDLTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtBHDLTB;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnDLTBBH;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
    }
}