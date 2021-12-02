namespace TDDC3D.sys
{
    partial class copyFeatureStructFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(copyFeatureStructFrm));
            this.txtAlias = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbDatasets = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatasets.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(75, 42);
            this.txtAlias.Margin = new System.Windows.Forms.Padding(2);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(247, 20);
            this.txtAlias.TabIndex = 16;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 45);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 15;
            this.labelControl4.Text = "要素类别名";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(75, 11);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(2);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(247, 20);
            this.txtClassName.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 12);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "要素类名称";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 78);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "目标数据集";
            // 
            // cmbDatasets
            // 
            this.cmbDatasets.Location = new System.Drawing.Point(78, 74);
            this.cmbDatasets.Margin = new System.Windows.Forms.Padding(2);
            this.cmbDatasets.Name = "cmbDatasets";
            this.cmbDatasets.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDatasets.Size = new System.Drawing.Size(244, 20);
            this.cmbDatasets.TabIndex = 18;
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(198, 117);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 24);
            this.simpleButton2.TabIndex = 20;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(49, 117);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 24);
            this.simpleButton1.TabIndex = 19;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // copyFeatureStructFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 164);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cmbDatasets);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "copyFeatureStructFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "复制要素类结构";
            this.Load += new System.EventHandler(this.copyFeatureStructFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDatasets.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtAlias;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtClassName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDatasets;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}