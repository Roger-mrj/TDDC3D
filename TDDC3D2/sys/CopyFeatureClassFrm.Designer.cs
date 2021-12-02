namespace TDDC3D.sys
{
    partial class CopyFeatureClassFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyFeatureClassFrm));
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbDatasets = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtAlias = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatasets.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClassName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(201, 122);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 24);
            this.simpleButton2.TabIndex = 28;
            this.simpleButton2.Text = "取消";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(52, 122);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 24);
            this.simpleButton1.TabIndex = 27;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cmbDatasets
            // 
            this.cmbDatasets.Location = new System.Drawing.Point(81, 79);
            this.cmbDatasets.Margin = new System.Windows.Forms.Padding(2);
            this.cmbDatasets.Name = "cmbDatasets";
            this.cmbDatasets.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDatasets.Size = new System.Drawing.Size(244, 20);
            this.cmbDatasets.TabIndex = 26;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 83);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 25;
            this.labelControl2.Text = "目标数据集";
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(78, 47);
            this.txtAlias.Margin = new System.Windows.Forms.Padding(2);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(247, 20);
            this.txtAlias.TabIndex = 24;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(17, 50);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 23;
            this.labelControl4.Text = "要素类别名";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(78, 16);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(2);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(247, 20);
            this.txtClassName.TabIndex = 22;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 17);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "要素类名称";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(19, 159);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 14);
            this.lblStatus.TabIndex = 29;
            // 
            // CopyFeatureClassFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 191);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cmbDatasets);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CopyFeatureClassFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "复制要素类";
            this.Load += new System.EventHandler(this.CopyFeatureClassFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatasets.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClassName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDatasets;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtAlias;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtClassName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblStatus;
    }
}