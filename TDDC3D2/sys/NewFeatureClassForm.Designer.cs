namespace TDDC3D.sys
{
    partial class NewFeatureClassForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFeatureClassForm));
            this.txtClassName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.rgGeoType = new DevExpress.XtraEditors.RadioGroup();
            this.txtSP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtAlias = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtClassName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgGeoType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(74, 10);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(2);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(247, 20);
            this.txtClassName.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 12);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "要素类名称";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 81);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "几何类型";
            // 
            // rgGeoType
            // 
            this.rgGeoType.Location = new System.Drawing.Point(77, 75);
            this.rgGeoType.Margin = new System.Windows.Forms.Padding(2);
            this.rgGeoType.Name = "rgGeoType";
            this.rgGeoType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "点"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "线"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "面")});
            this.rgGeoType.Size = new System.Drawing.Size(244, 26);
            this.rgGeoType.TabIndex = 6;
            // 
            // txtSP
            // 
            this.txtSP.Location = new System.Drawing.Point(74, 118);
            this.txtSP.Margin = new System.Windows.Forms.Padding(2);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(247, 20);
            this.txtSP.TabIndex = 8;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(14, 120);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "空间参考";
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(203, 157);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 24);
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Text = "取消";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(47, 157);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 24);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(74, 42);
            this.txtAlias.Margin = new System.Windows.Forms.Padding(2);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(247, 20);
            this.txtAlias.TabIndex = 12;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 45);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "要素类别名";
            // 
            // NewFeatureClassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 206);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.rgGeoType);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NewFeatureClassForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建要素类";
            this.Load += new System.EventHandler(this.NewFeatureClassForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtClassName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgGeoType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAlias.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtClassName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.RadioGroup rgGeoType;
        private DevExpress.XtraEditors.TextEdit txtSP;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtAlias;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}