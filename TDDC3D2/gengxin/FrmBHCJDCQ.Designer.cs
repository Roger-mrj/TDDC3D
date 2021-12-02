namespace TDDC3D.gengxin
{
    partial class FrmBHCJDCQ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBHCJDCQ));
            this.txtBHCJDCQ = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnDLTBGX = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtBHCJDCQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBHCJDCQ
            // 
            this.txtBHCJDCQ.Location = new System.Drawing.Point(181, 24);
            this.txtBHCJDCQ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBHCJDCQ.Name = "txtBHCJDCQ";
            this.txtBHCJDCQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtBHCJDCQ.Properties.ReadOnly = true;
            this.txtBHCJDCQ.Size = new System.Drawing.Size(369, 28);
            this.txtBHCJDCQ.TabIndex = 24;
            this.txtBHCJDCQ.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtBHCJDCQ_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(39, 28);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(144, 22);
            this.labelControl1.TabIndex = 23;
            this.labelControl1.Text = "变化村级调查区：";
            // 
            // btnDLTBGX
            // 
            this.btnDLTBGX.Location = new System.Drawing.Point(93, 80);
            this.btnDLTBGX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDLTBGX.Name = "btnDLTBGX";
            this.btnDLTBGX.Size = new System.Drawing.Size(101, 48);
            this.btnDLTBGX.TabIndex = 22;
            this.btnDLTBGX.Text = "提取";
            this.btnDLTBGX.Click += new System.EventHandler(this.btnDLTBGX_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(39, 152);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(511, 240);
            this.info.TabIndex = 21;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(450, 80);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 48);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(220, 80);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(204, 48);
            this.simpleButton1.TabIndex = 25;
            this.simpleButton1.Text = "编村级调查区单位代码";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmBHCJDCQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 430);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txtBHCJDCQ);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnDLTBGX);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBHCJDCQ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成变化村级调查区数据";
            ((System.ComponentModel.ISupportInitialize)(this.txtBHCJDCQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtBHCJDCQ;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnDLTBGX;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}