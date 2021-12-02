namespace RCIS.Controls
{
    partial class CalSelFeatureForm
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
            this.cmbTjFields = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtcount = new DevExpress.XtraEditors.TextEdit();
            this.txtSum = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTjFields.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSum.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbTjFields
            // 
            this.cmbTjFields.Location = new System.Drawing.Point(128, 53);
            this.cmbTjFields.Name = "cmbTjFields";
            this.cmbTjFields.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTjFields.Size = new System.Drawing.Size(231, 24);
            this.cmbTjFields.TabIndex = 14;
            this.cmbTjFields.SelectedIndexChanged += new System.EventHandler(this.cmbTjFields_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(21, 54);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "统计字段";
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(128, 19);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(231, 24);
            this.cmbLayers.TabIndex = 12;
            this.cmbLayers.SelectedIndexChanged += new System.EventHandler(this.cmbLayers_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 18);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "选择统计图层";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(21, 98);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(50, 18);
            this.labelControl3.TabIndex = 15;
            this.labelControl3.Text = "总个数:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(21, 134);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(50, 18);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "合计值:";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Location = new System.Drawing.Point(164, 192);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 17;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtcount
            // 
            this.txtcount.Location = new System.Drawing.Point(130, 98);
            this.txtcount.Name = "txtcount";
            this.txtcount.Size = new System.Drawing.Size(227, 24);
            this.txtcount.TabIndex = 18;
            // 
            // txtSum
            // 
            this.txtSum.Location = new System.Drawing.Point(129, 132);
            this.txtSum.Name = "txtSum";
            this.txtSum.Size = new System.Drawing.Size(227, 24);
            this.txtSum.TabIndex = 19;
            // 
            // CalSelFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 229);
            this.Controls.Add(this.txtSum);
            this.Controls.Add(this.txtcount);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cmbTjFields);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalSelFeatureForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算选中要素";
            this.Load += new System.EventHandler(this.CalSelFeatureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTjFields.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtcount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSum.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbTjFields;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.TextEdit txtcount;
        private DevExpress.XtraEditors.TextEdit txtSum;
    }
}