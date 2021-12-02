namespace TDDC3D.sys
{
    partial class OtherSetupForm
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
            this.chkFilter = new DevExpress.XtraEditors.CheckEdit();
            this.chkBGHistory = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtMoveTolerance = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chkFilter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBGHistory.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMoveTolerance.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkFilter
            // 
            this.chkFilter.Location = new System.Drawing.Point(9, 17);
            this.chkFilter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Properties.Caption = "开启过滤模式，仅用于大数据量的浏览、出图等操作,不适于编辑";
            this.chkFilter.Size = new System.Drawing.Size(397, 19);
            this.chkFilter.TabIndex = 0;
            // 
            // chkBGHistory
            // 
            this.chkBGHistory.Location = new System.Drawing.Point(12, 98);
            this.chkBGHistory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkBGHistory.Name = "chkBGHistory";
            this.chkBGHistory.Properties.Caption = "开启更新历史";
            this.chkBGHistory.Size = new System.Drawing.Size(158, 19);
            this.chkBGHistory.TabIndex = 1;
            this.chkBGHistory.CheckedChanged += new System.EventHandler(this.chkBGHistory_CheckedChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 57);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "编辑粘滞移动容差";
            // 
            // txtMoveTolerance
            // 
            this.txtMoveTolerance.EditValue = ((short)(10));
            this.txtMoveTolerance.Location = new System.Drawing.Point(114, 54);
            this.txtMoveTolerance.Name = "txtMoveTolerance";
            this.txtMoveTolerance.Size = new System.Drawing.Size(55, 20);
            this.txtMoveTolerance.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(175, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "像素";
            // 
            // OtherSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 132);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtMoveTolerance);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.chkBGHistory);
            this.Controls.Add(this.chkFilter);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OtherSetupForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "其他设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OtherSetupForm_FormClosed);
            this.Load += new System.EventHandler(this.OtherSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkFilter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBGHistory.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMoveTolerance.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkFilter;
        private DevExpress.XtraEditors.CheckEdit chkBGHistory;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtMoveTolerance;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}