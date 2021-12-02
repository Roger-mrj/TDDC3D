namespace TDDC3D.edit
{
    partial class XzdwTbhPreBgDoSmallForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XzdwTbhPreBgDoSmallForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtSmallMj = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.cmbExceptedDlbms = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSmallMj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExceptedDlbms.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(21, 46);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "要处理的临时目标图层";
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(21, 74);
            this.cmbLayers.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(284, 20);
            this.cmbLayers.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(21, 109);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(84, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "碎图斑面积容差";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(173, 110);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 12;
            this.labelControl3.Text = "平方米";
            // 
            // txtSmallMj
            // 
            this.txtSmallMj.EditValue = "20";
            this.txtSmallMj.Location = new System.Drawing.Point(116, 106);
            this.txtSmallMj.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSmallMj.Name = "txtSmallMj";
            this.txtSmallMj.Size = new System.Drawing.Size(49, 20);
            this.txtSmallMj.TabIndex = 11;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(80, 210);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 13;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(190, 210);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = "取消";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(9, 10);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "处理选中"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "处理当前区域")});
            this.radioGroup1.Size = new System.Drawing.Size(333, 21);
            this.radioGroup1.TabIndex = 15;
            // 
            // cmbExceptedDlbms
            // 
            this.cmbExceptedDlbms.EditValue = "";
            this.cmbExceptedDlbms.Location = new System.Drawing.Point(128, 143);
            this.cmbExceptedDlbms.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbExceptedDlbms.Name = "cmbExceptedDlbms";
            this.cmbExceptedDlbms.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbExceptedDlbms.Properties.DropDownRows = 20;
            this.cmbExceptedDlbms.Size = new System.Drawing.Size(178, 20);
            this.cmbExceptedDlbms.TabIndex = 18;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(19, 146);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(96, 14);
            this.labelControl6.TabIndex = 17;
            this.labelControl6.Text = "地类图斑排除地类";
            // 
            // XzdwTbhPreBgDoSmallForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 254);
            this.Controls.Add(this.cmbExceptedDlbms);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtSmallMj);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cmbLayers);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XzdwTbhPreBgDoSmallForm";
            this.ShowIcon = false;
            this.Text = "预处理碎图斑";
            this.Load += new System.EventHandler(this.XzdwTbhPreBgDoSmallForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSmallMj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExceptedDlbms.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtSmallMj;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cmbExceptedDlbms;
        private DevExpress.XtraEditors.LabelControl labelControl6;
    }
}