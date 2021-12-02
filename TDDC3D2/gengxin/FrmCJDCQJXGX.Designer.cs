namespace TDDC3D.gengxin
{
    partial class FrmCJDCQJXGX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCJDCQJXGX));
            this.btnDLTBGX = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditBSM = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDLTBGX
            // 
            this.btnDLTBGX.Location = new System.Drawing.Point(194, 15);
            this.btnDLTBGX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDLTBGX.Name = "btnDLTBGX";
            this.btnDLTBGX.Size = new System.Drawing.Size(101, 48);
            this.btnDLTBGX.TabIndex = 27;
            this.btnDLTBGX.Text = "生成";
            this.btnDLTBGX.Click += new System.EventHandler(this.btnDLTBGX_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(38, 87);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(511, 240);
            this.info.TabIndex = 26;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(432, 15);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 48);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEditBSM
            // 
            this.btnEditBSM.Location = new System.Drawing.Point(313, 15);
            this.btnEditBSM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEditBSM.Name = "btnEditBSM";
            this.btnEditBSM.Size = new System.Drawing.Size(101, 48);
            this.btnEditBSM.TabIndex = 28;
            this.btnEditBSM.Text = "生成标识码";
            this.btnEditBSM.Click += new System.EventHandler(this.btnEditBSM_Click);
            // 
            // FrmCJDCQJXGX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 353);
            this.Controls.Add(this.btnEditBSM);
            this.Controls.Add(this.btnDLTBGX);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCJDCQJXGX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成村级调查区界线更新图层";
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnDLTBGX;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnEditBSM;
    }
}