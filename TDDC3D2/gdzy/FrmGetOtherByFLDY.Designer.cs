namespace TDDC3D.gdzy
{
    partial class FrmGetOtherByFLDY
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetOtherByFLDY));
            this.chkTRPH = new DevExpress.XtraEditors.CheckEdit();
            this.chkTRZD = new DevExpress.XtraEditors.CheckEdit();
            this.chkYJZHL = new DevExpress.XtraEditors.CheckEdit();
            this.chkTCHD = new DevExpress.XtraEditors.CheckEdit();
            this.chkTRZJS = new DevExpress.XtraEditors.CheckEdit();
            this.chkSWDYX = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnNo = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.chkAll = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTRPH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTRZD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYJZHL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTCHD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTRZJS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSWDYX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkTRPH
            // 
            this.chkTRPH.Location = new System.Drawing.Point(316, 130);
            this.chkTRPH.Name = "chkTRPH";
            this.chkTRPH.Properties.Caption = "土壤pH值图斑";
            this.chkTRPH.Size = new System.Drawing.Size(144, 22);
            this.chkTRPH.TabIndex = 30;
            // 
            // chkTRZD
            // 
            this.chkTRZD.Enabled = false;
            this.chkTRZD.Location = new System.Drawing.Point(316, 79);
            this.chkTRZD.Name = "chkTRZD";
            this.chkTRZD.Properties.Caption = "土壤质地图斑";
            this.chkTRZD.Size = new System.Drawing.Size(144, 22);
            this.chkTRZD.TabIndex = 29;
            // 
            // chkYJZHL
            // 
            this.chkYJZHL.Location = new System.Drawing.Point(150, 130);
            this.chkYJZHL.Name = "chkYJZHL";
            this.chkYJZHL.Properties.Caption = "土壤有机质含量图斑";
            this.chkYJZHL.Size = new System.Drawing.Size(160, 22);
            this.chkYJZHL.TabIndex = 28;
            // 
            // chkTCHD
            // 
            this.chkTCHD.Location = new System.Drawing.Point(150, 79);
            this.chkTCHD.Name = "chkTCHD";
            this.chkTCHD.Properties.Caption = "土层厚度图斑";
            this.chkTCHD.Size = new System.Drawing.Size(144, 22);
            this.chkTCHD.TabIndex = 27;
            // 
            // chkTRZJS
            // 
            this.chkTRZJS.Location = new System.Drawing.Point(316, 181);
            this.chkTRZJS.Name = "chkTRZJS";
            this.chkTRZJS.Properties.Caption = "土壤重金属污染状况";
            this.chkTRZJS.Size = new System.Drawing.Size(155, 22);
            this.chkTRZJS.TabIndex = 22;
            // 
            // chkSWDYX
            // 
            this.chkSWDYX.Location = new System.Drawing.Point(150, 181);
            this.chkSWDYX.Name = "chkSWDYX";
            this.chkSWDYX.Properties.Caption = "生物多样性";
            this.chkSWDYX.Size = new System.Drawing.Size(144, 22);
            this.chkSWDYX.TabIndex = 19;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(24, 81);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "选择字段：";
            // 
            // cmbLayer
            // 
            this.cmbLayer.EditValue = "分类单元";
            this.cmbLayer.Location = new System.Drawing.Point(150, 26);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Properties.Items.AddRange(new object[] {
            "分类单元",
            "扩充分类单元"});
            this.cmbLayer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbLayer.Size = new System.Drawing.Size(310, 24);
            this.cmbLayer.TabIndex = 17;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "选择图层：";
            // 
            // btnNo
            // 
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.Location = new System.Drawing.Point(388, 238);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(83, 37);
            this.btnNo.TabIndex = 26;
            this.btnNo.Text = "关闭";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(280, 238);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 37);
            this.btnOk.TabIndex = 25;
            this.btnOk.Text = "提取";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(150, 246);
            this.chkAll.Name = "chkAll";
            this.chkAll.Properties.Caption = "全选/全不选";
            this.chkAll.Size = new System.Drawing.Size(113, 22);
            this.chkAll.TabIndex = 31;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // FrmGetOtherByFLDY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 295);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.chkTRPH);
            this.Controls.Add(this.chkTRZD);
            this.Controls.Add(this.chkYJZHL);
            this.Controls.Add(this.chkTCHD);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkTRZJS);
            this.Controls.Add(this.chkSWDYX);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetOtherByFLDY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "根据分类单元融合其他分类因素数据";
            ((System.ComponentModel.ISupportInitialize)(this.chkTRPH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTRZD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYJZHL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTCHD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTRZJS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSWDYX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chkTRPH;
        private DevExpress.XtraEditors.CheckEdit chkTRZD;
        private DevExpress.XtraEditors.CheckEdit chkYJZHL;
        private DevExpress.XtraEditors.CheckEdit chkTCHD;
        private DevExpress.XtraEditors.SimpleButton btnNo;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.CheckEdit chkTRZJS;
        private DevExpress.XtraEditors.CheckEdit chkSWDYX;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkAll;
    }
}