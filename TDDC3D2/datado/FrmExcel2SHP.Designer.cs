namespace TDDC3D.datado
{
    partial class FrmExcel2SHP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExcel2SHP));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtExcel = new DevExpress.XtraEditors.ButtonEdit();
            this.txtSHP = new DevExpress.XtraEditors.ButtonEdit();
            this.txtSpatialReference = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cboWKTField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnImport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtExcel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpatialReference.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboWKTField.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 28);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(138, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "外业导出Excel数据：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 86);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(140, 18);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Shape File格式数据：";
            // 
            // txtExcel
            // 
            this.txtExcel.Location = new System.Drawing.Point(171, 25);
            this.txtExcel.Name = "txtExcel";
            this.txtExcel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtExcel.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtExcel.Size = new System.Drawing.Size(440, 24);
            this.txtExcel.TabIndex = 2;
            this.txtExcel.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtExcel_ButtonClick);
            // 
            // txtSHP
            // 
            this.txtSHP.Location = new System.Drawing.Point(171, 83);
            this.txtSHP.Name = "txtSHP";
            this.txtSHP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSHP.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtSHP.Size = new System.Drawing.Size(440, 24);
            this.txtSHP.TabIndex = 3;
            this.txtSHP.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtSHP_ButtonClick);
            // 
            // txtSpatialReference
            // 
            this.txtSpatialReference.Location = new System.Drawing.Point(171, 141);
            this.txtSpatialReference.Name = "txtSpatialReference";
            this.txtSpatialReference.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSpatialReference.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtSpatialReference.Size = new System.Drawing.Size(440, 24);
            this.txtSpatialReference.TabIndex = 5;
            this.txtSpatialReference.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtSpatialReference_ButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(77, 144);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 18);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "投影信息：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(74, 202);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(78, 18);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "WKT字段：";
            // 
            // cboWKTField
            // 
            this.cboWKTField.Location = new System.Drawing.Point(171, 199);
            this.cboWKTField.Name = "cboWKTField";
            this.cboWKTField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboWKTField.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboWKTField.Size = new System.Drawing.Size(132, 24);
            this.cboWKTField.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(496, 250);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 42);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.Location = new System.Drawing.Point(375, 250);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(85, 42);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "导入";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // FrmExcel2SHP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 322);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.cboWKTField);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtSpatialReference);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtSHP);
            this.Controls.Add(this.txtExcel);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExcel2SHP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入外业调查成果表格";
            ((System.ComponentModel.ISupportInitialize)(this.txtExcel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpatialReference.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboWKTField.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit txtExcel;
        private DevExpress.XtraEditors.ButtonEdit txtSHP;
        private DevExpress.XtraEditors.ButtonEdit txtSpatialReference;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cboWKTField;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnImport;
    }
}