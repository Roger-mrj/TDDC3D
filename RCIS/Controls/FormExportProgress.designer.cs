namespace RCIS.Controls
{
    partial class FormExportProgress
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labelFeatureName = new System.Windows.Forms.Label();
            this.labelProgressNumber = new System.Windows.Forms.Label();
            this.progressBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Controls.Add(this.labelProgress);
            this.progressBar.Location = new System.Drawing.Point(6, 39);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(171, 30);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // labelProgress
            // 
            this.labelProgress.BackColor = System.Drawing.Color.Transparent;
            this.labelProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelProgress.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelProgress.Location = new System.Drawing.Point(58, 5);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(55, 21);
            this.labelProgress.TabIndex = 4;
            this.labelProgress.Text = "100%";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelProgress.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(56, 76);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelFeatureName
            // 
            this.labelFeatureName.Location = new System.Drawing.Point(0, -2);
            this.labelFeatureName.Name = "labelFeatureName";
            this.labelFeatureName.Size = new System.Drawing.Size(186, 23);
            this.labelFeatureName.TabIndex = 2;
            this.labelFeatureName.Text = "正在导出DLTB图层";
            this.labelFeatureName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProgressNumber
            // 
            this.labelProgressNumber.Location = new System.Drawing.Point(1, 21);
            this.labelProgressNumber.Name = "labelProgressNumber";
            this.labelProgressNumber.Size = new System.Drawing.Size(184, 18);
            this.labelProgressNumber.TabIndex = 3;
            this.labelProgressNumber.Text = "（1000/1000）";
            this.labelProgressNumber.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelProgressNumber.Visible = false;
            // 
            // FormExportProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 108);
            this.Controls.Add(this.labelProgressNumber);
            this.Controls.Add(this.labelFeatureName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExportProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出进度";
            this.Load += new System.EventHandler(this.FormExportProgress_Load);
            this.progressBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelFeatureName;
        private System.Windows.Forms.Label labelProgressNumber;
        private System.Windows.Forms.Label labelProgress;
    }
}