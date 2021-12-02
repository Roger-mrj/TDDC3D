namespace TDDC3D.sys
{
    partial class RegSoftForm
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.memoinfo = new DevExpress.XtraEditors.MemoEdit();
            this.memosn = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.memoinfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memosn.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(76, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "机器ID信息";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 115);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(23, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "SN:";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(210, 209);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(100, 32);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(336, 209);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(100, 32);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "取消";
            // 
            // memoinfo
            // 
            this.memoinfo.Location = new System.Drawing.Point(114, 25);
            this.memoinfo.Name = "memoinfo";
            this.memoinfo.Size = new System.Drawing.Size(333, 69);
            this.memoinfo.TabIndex = 6;
            this.memoinfo.UseOptimizedRendering = true;
            // 
            // memosn
            // 
            this.memosn.Location = new System.Drawing.Point(114, 113);
            this.memosn.Name = "memosn";
            this.memosn.Size = new System.Drawing.Size(333, 69);
            this.memosn.TabIndex = 7;
            this.memosn.UseOptimizedRendering = true;
            // 
            // RegSoftForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 270);
            this.Controls.Add(this.memosn);
            this.Controls.Add(this.memoinfo);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegSoftForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件注册";
            this.Load += new System.EventHandler(this.RegSoftForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoinfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memosn.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.MemoEdit memoinfo;
        private DevExpress.XtraEditors.MemoEdit memosn;
    }
}