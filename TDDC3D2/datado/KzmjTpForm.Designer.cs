namespace TDDC3D.datado
{
    partial class KzmjTpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KzmjTpForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtKzmj = new DevExpress.XtraEditors.TextEdit();
            this.txtHdKzmj = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHdKzmj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 20);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(122, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "陆地控制面积(平方米):";
            // 
            // txtKzmj
            // 
            this.txtKzmj.Location = new System.Drawing.Point(151, 18);
            this.txtKzmj.Margin = new System.Windows.Forms.Padding(2);
            this.txtKzmj.Name = "txtKzmj";
            this.txtKzmj.Size = new System.Drawing.Size(458, 20);
            this.txtKzmj.TabIndex = 1;
            // 
            // txtHdKzmj
            // 
            this.txtHdKzmj.Location = new System.Drawing.Point(151, 53);
            this.txtHdKzmj.Margin = new System.Windows.Forms.Padding(2);
            this.txtHdKzmj.Name = "txtHdKzmj";
            this.txtHdKzmj.Size = new System.Drawing.Size(458, 20);
            this.txtHdKzmj.TabIndex = 10;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(16, 55);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(122, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "海岛控制面积(平方米):";
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(16, 131);
            this.memoLog.Margin = new System.Windows.Forms.Padding(2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(593, 545);
            this.memoLog.TabIndex = 12;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
            this.simpleButton4.Location = new System.Drawing.Point(415, 82);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(82, 29);
            this.simpleButton4.TabIndex = 11;
            this.simpleButton4.Text = "一键调平";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(531, 82);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(76, 29);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // KzmjTpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 692);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.txtHdKzmj);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.txtKzmj);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KzmjTpForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "控制面积调平";
            this.Load += new System.EventHandler(this.KzmjTpForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtKzmj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHdKzmj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtKzmj;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.TextEdit txtHdKzmj;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.MemoEdit memoLog;
    }
}