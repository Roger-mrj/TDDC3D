namespace TDDC3D.gengxin
{
    partial class FrmSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSummary));
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnSummary = new DevExpress.XtraEditors.SimpleButton();
            this.btnSumXZQ = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(24, 84);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(490, 200);
            this.info.TabIndex = 30;
            this.info.UseOptimizedRendering = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(418, 22);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 41);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSummary
            // 
            this.btnSummary.Location = new System.Drawing.Point(42, 22);
            this.btnSummary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(176, 41);
            this.btnSummary.TabIndex = 28;
            this.btnSummary.Text = "计算村级调查区面积";
            this.btnSummary.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // btnSumXZQ
            // 
            this.btnSumXZQ.Location = new System.Drawing.Point(236, 22);
            this.btnSumXZQ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSumXZQ.Name = "btnSumXZQ";
            this.btnSumXZQ.Size = new System.Drawing.Size(164, 41);
            this.btnSumXZQ.TabIndex = 31;
            this.btnSumXZQ.Text = "计算行政区面积";
            this.btnSumXZQ.Click += new System.EventHandler(this.btnSumXZQ_Click);
            // 
            // FrmSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 312);
            this.Controls.Add(this.btnSumXZQ);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSummary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "行政区和村级调查区面积汇总";
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnSummary;
        private DevExpress.XtraEditors.SimpleButton btnSumXZQ;
    }
}