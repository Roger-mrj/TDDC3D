namespace TDDC3D.zrzy
{
    partial class CzcdydTqForm
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
            this.radioGFwOpt = new DevExpress.XtraEditors.RadioGroup();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbTargetlayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbSrcLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.radioGFwOpt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetlayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSrcLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // radioGFwOpt
            // 
            this.radioGFwOpt.Location = new System.Drawing.Point(22, 155);
            this.radioGFwOpt.Name = "radioGFwOpt";
            this.radioGFwOpt.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前范围"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "全部要素")});
            this.radioGFwOpt.Size = new System.Drawing.Size(375, 44);
            this.radioGFwOpt.TabIndex = 35;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(65, 227);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 29);
            this.btnOk.TabIndex = 33;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(244, 227);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(100, 29);
            this.simpleButton2.TabIndex = 34;
            this.simpleButton2.Text = "取消";
            // 
            // cmbTargetlayer
            // 
            this.cmbTargetlayer.Location = new System.Drawing.Point(21, 109);
            this.cmbTargetlayer.Name = "cmbTargetlayer";
            this.cmbTargetlayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTargetlayer.Size = new System.Drawing.Size(372, 24);
            this.cmbTargetlayer.TabIndex = 32;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 81);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 31;
            this.labelControl2.Text = "目标图层";
            // 
            // cmbSrcLayer
            // 
            this.cmbSrcLayer.Location = new System.Drawing.Point(22, 44);
            this.cmbSrcLayer.Name = "cmbSrcLayer";
            this.cmbSrcLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSrcLayer.Size = new System.Drawing.Size(372, 24);
            this.cmbSrcLayer.TabIndex = 30;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(135, 18);
            this.labelControl1.TabIndex = 29;
            this.labelControl1.Text = "原始二调地类图斑层";
            // 
            // CzcdydTqForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 285);
            this.Controls.Add(this.radioGFwOpt);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.cmbTargetlayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbSrcLayer);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CzcdydTqForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "城镇村等用地提取";
            this.Load += new System.EventHandler(this.CzcdydTqForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radioGFwOpt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTargetlayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSrcLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup radioGFwOpt;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbTargetlayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbSrcLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}