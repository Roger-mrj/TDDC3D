namespace TDDC3D.gengxin
{
    partial class FrmXZQJXGX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmXZQJXGX));
            this.btnXZQJXGX = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditBSM = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnXZQJXGX
            // 
            this.btnXZQJXGX.Location = new System.Drawing.Point(156, 13);
            this.btnXZQJXGX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnXZQJXGX.Name = "btnXZQJXGX";
            this.btnXZQJXGX.Size = new System.Drawing.Size(101, 48);
            this.btnXZQJXGX.TabIndex = 22;
            this.btnXZQJXGX.Text = "生成";
            this.btnXZQJXGX.Click += new System.EventHandler(this.btnXZQJXGX_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(40, 85);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(511, 240);
            this.info.TabIndex = 21;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(434, 13);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 48);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEditBSM
            // 
            this.btnEditBSM.Location = new System.Drawing.Point(295, 13);
            this.btnEditBSM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEditBSM.Name = "btnEditBSM";
            this.btnEditBSM.Size = new System.Drawing.Size(101, 48);
            this.btnEditBSM.TabIndex = 23;
            this.btnEditBSM.Text = "编写标识码";
            this.btnEditBSM.Click += new System.EventHandler(this.btnEditBSM_Click);
            // 
            // FrmXZQJXGX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 352);
            this.Controls.Add(this.btnEditBSM);
            this.Controls.Add(this.btnXZQJXGX);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmXZQJXGX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成行政区界线更新层";
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnXZQJXGX;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnEditBSM;
    }
}