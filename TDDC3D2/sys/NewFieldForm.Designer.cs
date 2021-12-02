namespace TDDC3D.sys
{
    partial class NewFieldForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewFieldForm));
            this.txtFldAlias = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtFieldName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.comboFldType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.spinEditLength = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.spinScale = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFldAlias.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFieldName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFldType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditLength.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinScale.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFldAlias
            // 
            this.txtFldAlias.Location = new System.Drawing.Point(70, 42);
            this.txtFldAlias.Margin = new System.Windows.Forms.Padding(2);
            this.txtFldAlias.Name = "txtFldAlias";
            this.txtFldAlias.Size = new System.Drawing.Size(148, 20);
            this.txtFldAlias.TabIndex = 16;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(9, 45);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 15;
            this.labelControl4.Text = "字段别名";
            // 
            // txtFieldName
            // 
            this.txtFieldName.Location = new System.Drawing.Point(70, 10);
            this.txtFieldName.Margin = new System.Windows.Forms.Padding(2);
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.Size = new System.Drawing.Size(148, 20);
            this.txtFieldName.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 12);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "字段名称";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(9, 79);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "字段类型";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(9, 113);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 19;
            this.labelControl3.Text = "字段长度";
            // 
            // comboFldType
            // 
            this.comboFldType.Location = new System.Drawing.Point(73, 77);
            this.comboFldType.Margin = new System.Windows.Forms.Padding(2);
            this.comboFldType.Name = "comboFldType";
            this.comboFldType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboFldType.Properties.Items.AddRange(new object[] {
            "字符型",
            "浮点型",
            "日期型",
            "整型"});
            this.comboFldType.Size = new System.Drawing.Size(146, 20);
            this.comboFldType.TabIndex = 20;
            this.comboFldType.SelectedIndexChanged += new System.EventHandler(this.comboFldType_SelectedIndexChanged);
            // 
            // spinEditLength
            // 
            this.spinEditLength.EditValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinEditLength.Location = new System.Drawing.Point(72, 110);
            this.spinEditLength.Margin = new System.Windows.Forms.Padding(2);
            this.spinEditLength.Name = "spinEditLength";
            this.spinEditLength.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditLength.Size = new System.Drawing.Size(144, 20);
            this.spinEditLength.TabIndex = 21;
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(132, 183);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 24);
            this.simpleButton2.TabIndex = 23;
            this.simpleButton2.Text = "取消";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(16, 183);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 24);
            this.simpleButton1.TabIndex = 22;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(9, 142);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 24;
            this.labelControl5.Text = "小数位数";
            // 
            // spinScale
            // 
            this.spinScale.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spinScale.Location = new System.Drawing.Point(72, 140);
            this.spinScale.Margin = new System.Windows.Forms.Padding(2);
            this.spinScale.Name = "spinScale";
            this.spinScale.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinScale.Size = new System.Drawing.Size(144, 20);
            this.spinScale.TabIndex = 25;
            // 
            // NewFieldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 230);
            this.Controls.Add(this.spinScale);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.spinEditLength);
            this.Controls.Add(this.comboFldType);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtFldAlias);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtFieldName);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewFieldForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建字段";
            this.Load += new System.EventHandler(this.NewFieldForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtFldAlias.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFieldName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboFldType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditLength.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinScale.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtFldAlias;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtFieldName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit comboFldType;
        private DevExpress.XtraEditors.SpinEdit spinEditLength;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit spinScale;
    }
}