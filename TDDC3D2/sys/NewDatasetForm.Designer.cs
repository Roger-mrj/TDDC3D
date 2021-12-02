namespace TDDC3D.sys
{
    partial class NewDatasetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDatasetForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDatasetName = new DevExpress.XtraEditors.TextEdit();
            this.beSprj = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatasetName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSprj.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 15);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "数据集名称";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 50);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "选择空间参考";
            // 
            // txtDatasetName
            // 
            this.txtDatasetName.Location = new System.Drawing.Point(113, 13);
            this.txtDatasetName.Margin = new System.Windows.Forms.Padding(2);
            this.txtDatasetName.Name = "txtDatasetName";
            this.txtDatasetName.Size = new System.Drawing.Size(247, 20);
            this.txtDatasetName.TabIndex = 2;
            // 
            // beSprj
            // 
            this.beSprj.Location = new System.Drawing.Point(115, 45);
            this.beSprj.Margin = new System.Windows.Forms.Padding(2);
            this.beSprj.Name = "beSprj";
            this.beSprj.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beSprj.Size = new System.Drawing.Size(245, 20);
            this.beSprj.TabIndex = 3;
            this.beSprj.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSprj_ButtonClick);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(65, 90);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 24);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(221, 90);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 24);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "取消";
            // 
            // NewDatasetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 133);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.beSprj);
            this.Controls.Add(this.txtDatasetName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewDatasetForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建数据集";
            ((System.ComponentModel.ISupportInitialize)(this.txtDatasetName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSprj.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtDatasetName;
        private DevExpress.XtraEditors.ButtonEdit beSprj;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}