namespace TDDC3D.datado
{
    partial class CalmjandTpFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalmjandTpFrm));
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.txtHdKzmj = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtKzmj = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.chkRecalTbmj = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHdKzmj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecalTbmj.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(11, 127);
            this.memoLog.Margin = new System.Windows.Forms.Padding(2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(602, 543);
            this.memoLog.TabIndex = 19;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // txtHdKzmj
            // 
            this.txtHdKzmj.Location = new System.Drawing.Point(155, 47);
            this.txtHdKzmj.Margin = new System.Windows.Forms.Padding(2);
            this.txtHdKzmj.Name = "txtHdKzmj";
            this.txtHdKzmj.Size = new System.Drawing.Size(458, 20);
            this.txtHdKzmj.TabIndex = 17;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(20, 49);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(122, 14);
            this.labelControl3.TabIndex = 16;
            this.labelControl3.Text = "海岛控制面积(平方米):";
            // 
            // txtKzmj
            // 
            this.txtKzmj.Location = new System.Drawing.Point(155, 12);
            this.txtKzmj.Margin = new System.Windows.Forms.Padding(2);
            this.txtKzmj.Name = "txtKzmj";
            this.txtKzmj.Size = new System.Drawing.Size(458, 20);
            this.txtKzmj.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 14);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(122, 14);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "陆地控制面积(平方米):";
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(347, 81);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(154, 29);
            this.simpleButton4.TabIndex = 18;
            this.simpleButton4.Text = "面积计算及一键调平";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(535, 82);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(76, 29);
            this.simpleButton2.TabIndex = 15;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // chkRecalTbmj
            // 
            this.chkRecalTbmj.EditValue = true;
            this.chkRecalTbmj.Location = new System.Drawing.Point(24, 85);
            this.chkRecalTbmj.Name = "chkRecalTbmj";
            this.chkRecalTbmj.Properties.Caption = "重新计算椭球面积";
            this.chkRecalTbmj.Size = new System.Drawing.Size(128, 19);
            this.chkRecalTbmj.TabIndex = 20;
            // 
            // CalmjandTpFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 692);
            this.Controls.Add(this.chkRecalTbmj);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.txtHdKzmj);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.txtKzmj);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalmjandTpFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "面积计算及调平";
            this.Load += new System.EventHandler(this.CalmjandTpFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHdKzmj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRecalTbmj.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.TextEdit txtHdKzmj;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.TextEdit txtKzmj;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkRecalTbmj;
    }
}