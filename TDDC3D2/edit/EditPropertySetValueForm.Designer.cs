namespace TDDC3D.edit
{
    partial class EditPropertySetValueForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditPropertySetValueForm));
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cmbFWTC = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.cmbJHGX = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtValues = new DevExpress.XtraEditors.TextEdit();
            this.cmbSelLayerField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.radioGroupSetValue = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFWTC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJHGX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValues.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSelLayerField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupSetValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(10, 33);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "处理选中要素"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "处理当前范围要素"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "按选中范围处理")});
            this.radioGroup1.Size = new System.Drawing.Size(377, 35);
            this.radioGroup1.TabIndex = 19;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(9, 60);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(374, 20);
            this.cmbLayer.TabIndex = 18;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 33);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 14);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "选择要赋值的图层";
            // 
            // cmbField
            // 
            this.cmbField.Location = new System.Drawing.Point(10, 109);
            this.cmbField.Margin = new System.Windows.Forms.Padding(2);
            this.cmbField.Name = "cmbField";
            this.cmbField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbField.Size = new System.Drawing.Size(374, 20);
            this.cmbField.TabIndex = 21;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(10, 87);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 20;
            this.labelControl2.Text = "赋值字段";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(70, 445);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 22;
            this.simpleButton1.Text = "执行";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(204, 445);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 23;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 76);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(84, 14);
            this.labelControl4.TabIndex = 26;
            this.labelControl4.Text = "选中面所在图层";
            // 
            // cmbFWTC
            // 
            this.cmbFWTC.Enabled = false;
            this.cmbFWTC.Location = new System.Drawing.Point(10, 97);
            this.cmbFWTC.Margin = new System.Windows.Forms.Padding(2);
            this.cmbFWTC.Name = "cmbFWTC";
            this.cmbFWTC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFWTC.Size = new System.Drawing.Size(374, 20);
            this.cmbFWTC.TabIndex = 27;
            this.cmbFWTC.SelectedIndexChanged += new System.EventHandler(this.cmbFWTC_SelectedIndexChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.cmbLayer);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.cmbField);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(401, 140);
            this.groupControl2.TabIndex = 33;
            this.groupControl2.Text = "目标图层和字段";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.cmbJHGX);
            this.groupControl3.Controls.Add(this.labelControl7);
            this.groupControl3.Controls.Add(this.radioGroup1);
            this.groupControl3.Controls.Add(this.labelControl4);
            this.groupControl3.Controls.Add(this.cmbFWTC);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 140);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(401, 159);
            this.groupControl3.TabIndex = 34;
            this.groupControl3.Text = "赋值范围设定";
            // 
            // cmbJHGX
            // 
            this.cmbJHGX.EditValue = "包含";
            this.cmbJHGX.Enabled = false;
            this.cmbJHGX.Location = new System.Drawing.Point(89, 127);
            this.cmbJHGX.Margin = new System.Windows.Forms.Padding(2);
            this.cmbJHGX.Name = "cmbJHGX";
            this.cmbJHGX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbJHGX.Properties.Items.AddRange(new object[] {
            "包含",
            "相交"});
            this.cmbJHGX.Size = new System.Drawing.Size(295, 20);
            this.cmbJHGX.TabIndex = 32;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(10, 130);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 31;
            this.labelControl7.Text = "几何关系";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtValues);
            this.groupControl1.Controls.Add(this.cmbSelLayerField);
            this.groupControl1.Controls.Add(this.radioGroupSetValue);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 299);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(401, 135);
            this.groupControl1.TabIndex = 35;
            this.groupControl1.Text = "赋值内容设定";
            // 
            // txtValues
            // 
            this.txtValues.Location = new System.Drawing.Point(92, 70);
            this.txtValues.Margin = new System.Windows.Forms.Padding(2);
            this.txtValues.Name = "txtValues";
            this.txtValues.Size = new System.Drawing.Size(295, 20);
            this.txtValues.TabIndex = 25;
            // 
            // cmbSelLayerField
            // 
            this.cmbSelLayerField.Enabled = false;
            this.cmbSelLayerField.Location = new System.Drawing.Point(92, 97);
            this.cmbSelLayerField.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSelLayerField.Name = "cmbSelLayerField";
            this.cmbSelLayerField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSelLayerField.Size = new System.Drawing.Size(295, 20);
            this.cmbSelLayerField.TabIndex = 30;
            // 
            // radioGroupSetValue
            // 
            this.radioGroupSetValue.Enabled = false;
            this.radioGroupSetValue.Location = new System.Drawing.Point(10, 27);
            this.radioGroupSetValue.Margin = new System.Windows.Forms.Padding(2);
            this.radioGroupSetValue.Name = "radioGroupSetValue";
            this.radioGroupSetValue.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "赋固定值"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "赋选中图层字段值")});
            this.radioGroupSetValue.Size = new System.Drawing.Size(377, 35);
            this.radioGroupSetValue.TabIndex = 28;
            this.radioGroupSetValue.SelectedIndexChanged += new System.EventHandler(this.radioGroupSetValue_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 71);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 24;
            this.labelControl3.Text = "赋固定值";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(10, 102);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 14);
            this.labelControl5.TabIndex = 29;
            this.labelControl5.Text = "选中图层字段";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.DarkRed;
            this.labelControl6.Location = new System.Drawing.Point(9, 482);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(374, 14);
            this.labelControl6.TabIndex = 36;
            this.labelControl6.Text = "用行政区范围赋值时，赋权属单位代码和座落单位代码会自动补足7个0";
            // 
            // EditPropertySetValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 506);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditPropertySetValueForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "属性批量赋值";
            this.Load += new System.EventHandler(this.EditPropertySetValueForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFWTC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbJHGX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValues.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSelLayerField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupSetValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbField;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cmbFWTC;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit txtValues;
        private DevExpress.XtraEditors.ComboBoxEdit cmbSelLayerField;
        private DevExpress.XtraEditors.RadioGroup radioGroupSetValue;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit cmbJHGX;
        private DevExpress.XtraEditors.LabelControl labelControl7;
    }
}