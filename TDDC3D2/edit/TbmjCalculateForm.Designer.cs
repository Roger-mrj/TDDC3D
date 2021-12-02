namespace TDDC3D.edit
{
    partial class TbmjCalculateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TbmjCalculateForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblNumbers = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.cmbField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbField.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 63);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择要计算的图层";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(28, 245);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 11;
            this.simpleButton1.Text = "计算";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(214, 245);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 12;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(14, 87);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(314, 20);
            this.cmbLayer.TabIndex = 10;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // lblNumbers
            // 
            this.lblNumbers.Location = new System.Drawing.Point(13, 163);
            this.lblNumbers.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblNumbers.Name = "lblNumbers";
            this.lblNumbers.Size = new System.Drawing.Size(36, 14);
            this.lblNumbers.TabIndex = 13;
            this.lblNumbers.Text = "选中数";
            // 
            // memoEdit1
            // 
            this.memoEdit1.EditValue = "    该功能将当前范围或选中图斑要素进行椭球面积计算,并将结果赋值给【TBMJ】！";
            this.memoEdit1.Location = new System.Drawing.Point(9, 10);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.memoEdit1.Properties.Appearance.Options.UseFont = true;
            this.memoEdit1.Size = new System.Drawing.Size(318, 47);
            this.memoEdit1.TabIndex = 14;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(12, 184);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前范围"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选中要素")});
            this.radioGroup1.Size = new System.Drawing.Size(314, 35);
            this.radioGroup1.TabIndex = 16;
            // 
            // cmbField
            // 
            this.cmbField.Location = new System.Drawing.Point(14, 135);
            this.cmbField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbField.Name = "cmbField";
            this.cmbField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbField.Size = new System.Drawing.Size(314, 20);
            this.cmbField.TabIndex = 18;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 115);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "赋值字段";
            // 
            // TbmjCalculateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 308);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.lblNumbers);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TbmjCalculateForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "椭球面积重计算";
            this.Load += new System.EventHandler(this.TbmjCalculateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbField.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl lblNumbers;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbField;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}