namespace TDDC3D.edit
{
    partial class XxdwkdSetValForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XxdwkdSetValForm));
            this.lblNumbers = new DevExpress.XtraEditors.LabelControl();
            this.radioGroupExtent = new DevExpress.XtraEditors.RadioGroup();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbDlbms = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.lblstatus = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupExtent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDlbms.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNumbers
            // 
            this.lblNumbers.Location = new System.Drawing.Point(22, 72);
            this.lblNumbers.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblNumbers.Name = "lblNumbers";
            this.lblNumbers.Size = new System.Drawing.Size(36, 14);
            this.lblNumbers.TabIndex = 33;
            this.lblNumbers.Text = "选中数";
            // 
            // radioGroupExtent
            // 
            this.radioGroupExtent.Location = new System.Drawing.Point(22, 97);
            this.radioGroupExtent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioGroupExtent.Name = "radioGroupExtent";
            this.radioGroupExtent.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前范围"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选中要素")});
            this.radioGroupExtent.Size = new System.Drawing.Size(281, 35);
            this.radioGroupExtent.TabIndex = 32;
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(22, 42);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(280, 20);
            this.cmbLayer.TabIndex = 31;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 18);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 30;
            this.labelControl1.Text = "选择要计算的图层";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 147);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 34;
            this.labelControl2.Text = "地类筛选";
            // 
            // cmbDlbms
            // 
            this.cmbDlbms.Location = new System.Drawing.Point(22, 174);
            this.cmbDlbms.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbDlbms.Name = "cmbDlbms";
            this.cmbDlbms.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDlbms.Properties.DropDownRows = 20;
            this.cmbDlbms.Size = new System.Drawing.Size(280, 20);
            this.cmbDlbms.TabIndex = 35;
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(184, 215);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(77, 28);
            this.simpleButton2.TabIndex = 37;
            this.simpleButton2.Text = "取消";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(53, 215);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(77, 28);
            this.simpleButton1.TabIndex = 36;
            this.simpleButton1.Text = "赋值";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // lblstatus
            // 
            this.lblstatus.Location = new System.Drawing.Point(22, 270);
            this.lblstatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(24, 14);
            this.lblstatus.TabIndex = 38;
            this.lblstatus.Text = "状态";
            // 
            // XxdwkdSetValForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 292);
            this.Controls.Add(this.lblstatus);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cmbDlbms);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblNumbers);
            this.Controls.Add(this.radioGroupExtent);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "XxdwkdSetValForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "线性地物宽度赋值";
            this.Load += new System.EventHandler(this.XxdwkdSetValForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupExtent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDlbms.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNumbers;
        private DevExpress.XtraEditors.RadioGroup radioGroupExtent;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cmbDlbms;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl lblstatus;
    }
}