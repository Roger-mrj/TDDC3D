namespace TDDC3D.edit
{
    partial class MapIntegrateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapIntegrateForm));
            this.txtTbmj = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.checkEditMjCZC = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtScale = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.lblstatus = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroupExtent = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cmbExceptedDlbms = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtTbmj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditMjCZC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupExtent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExceptedDlbms.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTbmj
            // 
            this.txtTbmj.EditValue = "500";
            this.txtTbmj.Location = new System.Drawing.Point(128, 34);
            this.txtTbmj.Margin = new System.Windows.Forms.Padding(2);
            this.txtTbmj.Name = "txtTbmj";
            this.txtTbmj.Size = new System.Drawing.Size(50, 20);
            this.txtTbmj.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(196, 36);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(132, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "平方米的将合并到相邻斑";
            // 
            // checkEditMjCZC
            // 
            this.checkEditMjCZC.EditValue = true;
            this.checkEditMjCZC.Location = new System.Drawing.Point(20, 103);
            this.checkEditMjCZC.Margin = new System.Windows.Forms.Padding(2);
            this.checkEditMjCZC.Name = "checkEditMjCZC";
            this.checkEditMjCZC.Properties.Caption = "面积除以周长小于";
            this.checkEditMjCZC.Size = new System.Drawing.Size(114, 19);
            this.checkEditMjCZC.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(196, 105);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 14);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "米的将合并到相邻图斑";
            // 
            // txtScale
            // 
            this.txtScale.EditValue = "8";
            this.txtScale.Location = new System.Drawing.Point(138, 102);
            this.txtScale.Margin = new System.Windows.Forms.Padding(2);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(53, 20);
            this.txtScale.TabIndex = 4;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(94, 302);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(70, 28);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(225, 302);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(70, 28);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "取消";
            // 
            // lblstatus
            // 
            this.lblstatus.Location = new System.Drawing.Point(23, 345);
            this.lblstatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(24, 14);
            this.lblstatus.TabIndex = 9;
            this.lblstatus.Text = "状态";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(23, 22);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(129, 14);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "选择要进行缩编的dltb层";
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(171, 19);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(202, 20);
            this.cmbLayer.TabIndex = 11;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(20, 66);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(96, 14);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "（同种地类优先）";
            // 
            // radioGroupExtent
            // 
            this.radioGroupExtent.Location = new System.Drawing.Point(22, 51);
            this.radioGroupExtent.Margin = new System.Windows.Forms.Padding(2);
            this.radioGroupExtent.Name = "radioGroupExtent";
            this.radioGroupExtent.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "所有范围"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前范围")});
            this.radioGroupExtent.Size = new System.Drawing.Size(350, 22);
            this.radioGroupExtent.TabIndex = 13;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(20, 37);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(96, 14);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "地类图斑面积小于";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cmbExceptedDlbms);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.txtTbmj);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.txtScale);
            this.groupControl1.Controls.Add(this.checkEditMjCZC);
            this.groupControl1.Location = new System.Drawing.Point(22, 91);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(350, 188);
            this.groupControl1.TabIndex = 15;
            this.groupControl1.Text = "其他选项";
            // 
            // cmbExceptedDlbms
            // 
            this.cmbExceptedDlbms.Location = new System.Drawing.Point(81, 141);
            this.cmbExceptedDlbms.Margin = new System.Windows.Forms.Padding(2);
            this.cmbExceptedDlbms.Name = "cmbExceptedDlbms";
            this.cmbExceptedDlbms.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbExceptedDlbms.Properties.DropDownRows = 20;
            this.cmbExceptedDlbms.Size = new System.Drawing.Size(226, 20);
            this.cmbExceptedDlbms.TabIndex = 16;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(20, 143);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 15;
            this.labelControl6.Text = "排除地类";
            // 
            // MapIntegrateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 368);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.radioGroupExtent);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.lblstatus);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapIntegrateForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图缩编";
            this.Load += new System.EventHandler(this.MapIntegrateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtTbmj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditMjCZC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupExtent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExceptedDlbms.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtTbmj;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit checkEditMjCZC;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtScale;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl lblstatus;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.RadioGroup radioGroupExtent;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cmbExceptedDlbms;
    }
}