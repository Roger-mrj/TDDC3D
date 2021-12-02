namespace TDDC3D.edit
{
    partial class MultipartDoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipartDoForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblNumbers = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(29, 69);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择要处理的图层";
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(29, 98);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(284, 20);
            this.cmbLayer.TabIndex = 6;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // lblNumbers
            // 
            this.lblNumbers.Location = new System.Drawing.Point(34, 130);
            this.lblNumbers.Margin = new System.Windows.Forms.Padding(2);
            this.lblNumbers.Name = "lblNumbers";
            this.lblNumbers.Size = new System.Drawing.Size(0, 14);
            this.lblNumbers.TabIndex = 7;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(52, 206);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "打散";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(187, 206);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // memoEdit1
            // 
            this.memoEdit1.EditValue = "该功能将自动查找当前范围内选定图层的所部分要素,并可以进行打散！";
            this.memoEdit1.Location = new System.Drawing.Point(26, 10);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(2);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.memoEdit1.Properties.Appearance.Options.UseFont = true;
            this.memoEdit1.Size = new System.Drawing.Size(288, 47);
            this.memoEdit1.TabIndex = 15;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(29, 149);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "处理选中要素"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "处理当前范围要素")});
            this.radioGroup1.Size = new System.Drawing.Size(284, 35);
            this.radioGroup1.TabIndex = 16;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // MultipartDoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 239);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.lblNumbers);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultipartDoForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "多部分打散";
            this.Load += new System.EventHandler(this.MultipartDoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl lblNumbers;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
    }
}