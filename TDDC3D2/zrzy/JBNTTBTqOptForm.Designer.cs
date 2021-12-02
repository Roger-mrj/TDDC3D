namespace TDDC3D.zrzy
{
    partial class JBNTTBTqOptForm
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
            this.cmbTargetlayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbSrcLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkComDL = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.radioGFwOpt = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetlayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSrcLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkComDL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGFwOpt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbTargetlayer
            // 
            this.cmbTargetlayer.Location = new System.Drawing.Point(11, 114);
            this.cmbTargetlayer.Name = "cmbTargetlayer";
            this.cmbTargetlayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTargetlayer.Size = new System.Drawing.Size(372, 24);
            this.cmbTargetlayer.TabIndex = 23;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 86);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 22;
            this.labelControl2.Text = "目标图层";
            // 
            // cmbSrcLayer
            // 
            this.cmbSrcLayer.Location = new System.Drawing.Point(12, 51);
            this.cmbSrcLayer.Name = "cmbSrcLayer";
            this.cmbSrcLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSrcLayer.Size = new System.Drawing.Size(372, 24);
            this.cmbSrcLayer.TabIndex = 21;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 20;
            this.labelControl1.Text = "原始图层";
            // 
            // chkComDL
            // 
            this.chkComDL.EditValue = ", , ";
            this.chkComDL.Location = new System.Drawing.Point(13, 184);
            this.chkComDL.Margin = new System.Windows.Forms.Padding(4);
            this.chkComDL.Name = "chkComDL";
            this.chkComDL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.chkComDL.Properties.DropDownRows = 25;
            this.chkComDL.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "0101|水田", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "0102|水浇地", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(null, "0103|旱地", System.Windows.Forms.CheckState.Checked)});
            this.chkComDL.Size = new System.Drawing.Size(370, 24);
            this.chkComDL.TabIndex = 24;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 153);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 18);
            this.labelControl3.TabIndex = 25;
            this.labelControl3.Text = "提取地类";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(51, 303);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 29);
            this.btnOk.TabIndex = 26;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(230, 303);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(100, 29);
            this.simpleButton2.TabIndex = 27;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // radioGFwOpt
            // 
            this.radioGFwOpt.Location = new System.Drawing.Point(8, 231);
            this.radioGFwOpt.Name = "radioGFwOpt";
            this.radioGFwOpt.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前范围"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "全部要素")});
            this.radioGFwOpt.Size = new System.Drawing.Size(375, 44);
            this.radioGFwOpt.TabIndex = 28;
            // 
            // JBNTTBTqOptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 367);
            this.Controls.Add(this.radioGFwOpt);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.chkComDL);
            this.Controls.Add(this.cmbTargetlayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbSrcLayer);
            this.Controls.Add(this.labelControl1);
            this.Name = "JBNTTBTqOptForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基本农田保护图斑提取";
            this.Load += new System.EventHandler(this.JBNTTBTqOptForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetlayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSrcLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkComDL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGFwOpt.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbTargetlayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbSrcLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit chkComDL;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.RadioGroup radioGFwOpt;
    }
}