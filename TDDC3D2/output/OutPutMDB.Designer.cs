namespace TDDC3D.output
{
    partial class OutPutMDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutPutMDB));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtSHP = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cboField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtExportPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl35 = new DevExpress.XtraEditors.LabelControl();
            this.txtS = new DevExpress.XtraEditors.TextEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtP = new DevExpress.XtraEditors.TextEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExportPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(25, 73);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(105, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "外业图斑数据：";
            // 
            // txtSHP
            // 
            this.txtSHP.Location = new System.Drawing.Point(136, 70);
            this.txtSHP.Name = "txtSHP";
            this.txtSHP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSHP.Properties.ReadOnly = true;
            this.txtSHP.Size = new System.Drawing.Size(546, 24);
            this.txtSHP.TabIndex = 1;
            this.txtSHP.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtSHP_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(25, 130);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(105, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "图斑编号字段：";
            // 
            // cboField
            // 
            this.cboField.Location = new System.Drawing.Point(136, 127);
            this.cboField.Name = "cboField";
            this.cboField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboField.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboField.Size = new System.Drawing.Size(107, 24);
            this.cboField.TabIndex = 3;
            // 
            // txtExportPath
            // 
            this.txtExportPath.Location = new System.Drawing.Point(136, 217);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtExportPath.Properties.ReadOnly = true;
            this.txtExportPath.Size = new System.Drawing.Size(546, 24);
            this.txtExportPath.TabIndex = 5;
            this.txtExportPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtExportPath_ButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(25, 220);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(105, 18);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "数据导出目录：";
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(576, 265);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 42);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(474, 265);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(85, 42);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // labelControl35
            // 
            this.labelControl35.Location = new System.Drawing.Point(333, 176);
            this.labelControl35.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl35.Name = "labelControl35";
            this.labelControl35.Size = new System.Drawing.Size(251, 18);
            this.labelControl35.TabIndex = 51;
            this.labelControl35.Text = "平方米，或者重叠部分占自身面积比>";
            // 
            // txtS
            // 
            this.txtS.EditValue = "50";
            this.txtS.Location = new System.Drawing.Point(256, 173);
            this.txtS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtS.Name = "txtS";
            this.txtS.Properties.Mask.EditMask = "[0-9]*";
            this.txtS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtS.Properties.MaxLength = 6;
            this.txtS.Size = new System.Drawing.Size(71, 24);
            this.txtS.TabIndex = 50;
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(136, 176);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(109, 18);
            this.labelControl21.TabIndex = 49;
            this.labelControl21.Text = "提取叠加面积S>";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(667, 176);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(15, 18);
            this.labelControl4.TabIndex = 54;
            this.labelControl4.Text = "%";
            // 
            // txtP
            // 
            this.txtP.EditValue = "50";
            this.txtP.Location = new System.Drawing.Point(590, 173);
            this.txtP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtP.Name = "txtP";
            this.txtP.Properties.Mask.EditMask = "[0-9]*";
            this.txtP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtP.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtP.Properties.MaxLength = 2;
            this.txtP.Size = new System.Drawing.Size(71, 24);
            this.txtP.TabIndex = 53;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(130, 15);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "地类图斑";
            this.checkEdit1.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit1.Properties.RadioGroupIndex = 1;
            this.checkEdit1.Size = new System.Drawing.Size(173, 22);
            this.checkEdit1.TabIndex = 55;
            this.checkEdit1.TabStop = false;
            // 
            // checkEdit2
            // 
            this.checkEdit2.EditValue = true;
            this.checkEdit2.Location = new System.Drawing.Point(405, 15);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "地类图斑更新";
            this.checkEdit2.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.checkEdit2.Properties.RadioGroupIndex = 1;
            this.checkEdit2.Size = new System.Drawing.Size(173, 22);
            this.checkEdit2.TabIndex = 56;
            // 
            // OutPutMDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 327);
            this.Controls.Add(this.checkEdit2);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtP);
            this.Controls.Add(this.labelControl35);
            this.Controls.Add(this.txtS);
            this.Controls.Add(this.labelControl21);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtExportPath);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cboField);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtSHP);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutPutMDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出外业举证图斑信息表";
            ((System.ComponentModel.ISupportInitialize)(this.txtSHP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExportPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit txtSHP;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cboField;
        private DevExpress.XtraEditors.ButtonEdit txtExportPath;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraEditors.LabelControl labelControl35;
        private DevExpress.XtraEditors.TextEdit txtS;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtP;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
    }
}