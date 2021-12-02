namespace TDDC3D.gdzy
{
    partial class FrmGetSpatialAttribute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetSpatialAttribute));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkHD = new DevExpress.XtraEditors.CheckEdit();
            this.chkZD = new DevExpress.XtraEditors.CheckEdit();
            this.chkPH = new DevExpress.XtraEditors.CheckEdit();
            this.chkSW = new DevExpress.XtraEditors.CheckEdit();
            this.chkYJZ = new DevExpress.XtraEditors.CheckEdit();
            this.chkWR = new DevExpress.XtraEditors.CheckEdit();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnNo = new DevExpress.XtraEditors.SimpleButton();
            this.chkPD = new DevExpress.XtraEditors.CheckEdit();
            this.chkSZ = new DevExpress.XtraEditors.CheckEdit();
            this.chkZRQ = new DevExpress.XtraEditors.CheckEdit();
            this.chkDL = new DevExpress.XtraEditors.CheckEdit();
            this.chkAll = new DevExpress.XtraEditors.CheckEdit();
            this.chkZLFLDM = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSW.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYJZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZRQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZLFLDM.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(35, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择图层：";
            // 
            // cmbLayer
            // 
            this.cmbLayer.EditValue = "分类单元";
            this.cmbLayer.Location = new System.Drawing.Point(161, 27);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Properties.Items.AddRange(new object[] {
            "分类单元",
            "扩充分类单元"});
            this.cmbLayer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbLayer.Size = new System.Drawing.Size(310, 24);
            this.cmbLayer.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(35, 82);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "选择字段：";
            // 
            // chkHD
            // 
            this.chkHD.Location = new System.Drawing.Point(161, 182);
            this.chkHD.Name = "chkHD";
            this.chkHD.Properties.Caption = "土层厚度";
            this.chkHD.Size = new System.Drawing.Size(144, 22);
            this.chkHD.TabIndex = 3;
            // 
            // chkZD
            // 
            this.chkZD.Enabled = false;
            this.chkZD.Location = new System.Drawing.Point(161, 233);
            this.chkZD.Name = "chkZD";
            this.chkZD.Properties.Caption = "土壤质地";
            this.chkZD.Size = new System.Drawing.Size(144, 22);
            this.chkZD.TabIndex = 4;
            // 
            // chkPH
            // 
            this.chkPH.Location = new System.Drawing.Point(161, 284);
            this.chkPH.Name = "chkPH";
            this.chkPH.Properties.Caption = "土壤PH值";
            this.chkPH.Size = new System.Drawing.Size(144, 22);
            this.chkPH.TabIndex = 5;
            // 
            // chkSW
            // 
            this.chkSW.Location = new System.Drawing.Point(290, 182);
            this.chkSW.Name = "chkSW";
            this.chkSW.Properties.Caption = "生物多样性";
            this.chkSW.Size = new System.Drawing.Size(144, 22);
            this.chkSW.TabIndex = 6;
            // 
            // chkYJZ
            // 
            this.chkYJZ.Location = new System.Drawing.Point(290, 233);
            this.chkYJZ.Name = "chkYJZ";
            this.chkYJZ.Properties.Caption = "土壤有机质含量";
            this.chkYJZ.Size = new System.Drawing.Size(144, 22);
            this.chkYJZ.TabIndex = 7;
            // 
            // chkWR
            // 
            this.chkWR.Location = new System.Drawing.Point(290, 284);
            this.chkWR.Name = "chkWR";
            this.chkWR.Properties.Caption = "土壤重金属污染状况";
            this.chkWR.Size = new System.Drawing.Size(168, 22);
            this.chkWR.TabIndex = 8;
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(280, 387);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 37);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "提取";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnNo
            // 
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.Location = new System.Drawing.Point(388, 387);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(83, 37);
            this.btnNo.TabIndex = 10;
            this.btnNo.Text = "关闭";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // chkPD
            // 
            this.chkPD.Location = new System.Drawing.Point(161, 80);
            this.chkPD.Name = "chkPD";
            this.chkPD.Properties.Caption = "坡度";
            this.chkPD.Size = new System.Drawing.Size(144, 22);
            this.chkPD.TabIndex = 11;
            // 
            // chkSZ
            // 
            this.chkSZ.Location = new System.Drawing.Point(161, 131);
            this.chkSZ.Name = "chkSZ";
            this.chkSZ.Properties.Caption = "熟制";
            this.chkSZ.Size = new System.Drawing.Size(144, 22);
            this.chkSZ.TabIndex = 12;
            // 
            // chkZRQ
            // 
            this.chkZRQ.Location = new System.Drawing.Point(290, 80);
            this.chkZRQ.Name = "chkZRQ";
            this.chkZRQ.Properties.Caption = "自然区";
            this.chkZRQ.Size = new System.Drawing.Size(144, 22);
            this.chkZRQ.TabIndex = 13;
            // 
            // chkDL
            // 
            this.chkDL.Location = new System.Drawing.Point(290, 131);
            this.chkDL.Name = "chkDL";
            this.chkDL.Properties.Caption = "耕地二级地类";
            this.chkDL.Size = new System.Drawing.Size(144, 22);
            this.chkDL.TabIndex = 14;
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(161, 395);
            this.chkAll.Name = "chkAll";
            this.chkAll.Properties.Caption = "全选/全不选";
            this.chkAll.Size = new System.Drawing.Size(113, 22);
            this.chkAll.TabIndex = 15;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // chkZLFLDM
            // 
            this.chkZLFLDM.Location = new System.Drawing.Point(161, 334);
            this.chkZLFLDM.Name = "chkZLFLDM";
            this.chkZLFLDM.Properties.Caption = "质量分类代码";
            this.chkZLFLDM.Size = new System.Drawing.Size(168, 22);
            this.chkZLFLDM.TabIndex = 16;
            // 
            // FrmGetSpatialAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 436);
            this.Controls.Add(this.chkZLFLDM);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.chkDL);
            this.Controls.Add(this.chkZRQ);
            this.Controls.Add(this.chkSZ);
            this.Controls.Add(this.chkPD);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkWR);
            this.Controls.Add(this.chkYJZ);
            this.Controls.Add(this.chkSW);
            this.Controls.Add(this.chkPH);
            this.Controls.Add(this.chkZD);
            this.Controls.Add(this.chkHD);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetSpatialAttribute";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "属性提取";
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSW.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkYJZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkWR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZRQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkZLFLDM.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chkHD;
        private DevExpress.XtraEditors.CheckEdit chkZD;
        private DevExpress.XtraEditors.CheckEdit chkPH;
        private DevExpress.XtraEditors.CheckEdit chkSW;
        private DevExpress.XtraEditors.CheckEdit chkYJZ;
        private DevExpress.XtraEditors.CheckEdit chkWR;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnNo;
        private DevExpress.XtraEditors.CheckEdit chkPD;
        private DevExpress.XtraEditors.CheckEdit chkSZ;
        private DevExpress.XtraEditors.CheckEdit chkZRQ;
        private DevExpress.XtraEditors.CheckEdit chkDL;
        private DevExpress.XtraEditors.CheckEdit chkAll;
        private DevExpress.XtraEditors.CheckEdit chkZLFLDM;
    }
}