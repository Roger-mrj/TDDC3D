namespace TDDC3D.gdzy
{
    partial class FrmGetGDData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetGDData));
            this.cmbSourceLay = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkFL = new DevExpress.XtraEditors.CheckEdit();
            this.chkKcfl = new DevExpress.XtraEditors.CheckEdit();
            this.chkFZ = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtSHP = new DevExpress.XtraEditors.ButtonEdit();
            this.rdoResource = new DevExpress.XtraEditors.RadioGroup();
            this.chkSD = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceLay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKcfl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoResource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSD.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSourceLay
            // 
            this.cmbSourceLay.Location = new System.Drawing.Point(169, 14);
            this.cmbSourceLay.Name = "cmbSourceLay";
            this.cmbSourceLay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSourceLay.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbSourceLay.Size = new System.Drawing.Size(296, 24);
            this.cmbSourceLay.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(35, 103);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "目标图层：";
            // 
            // chkFL
            // 
            this.chkFL.Location = new System.Drawing.Point(169, 101);
            this.chkFL.Name = "chkFL";
            this.chkFL.Properties.Caption = "分类单元图层";
            this.chkFL.Size = new System.Drawing.Size(133, 22);
            this.chkFL.TabIndex = 3;
            // 
            // chkKcfl
            // 
            this.chkKcfl.Location = new System.Drawing.Point(169, 137);
            this.chkKcfl.Name = "chkKcfl";
            this.chkKcfl.Properties.Caption = "扩充分类单元图层";
            this.chkKcfl.Size = new System.Drawing.Size(186, 22);
            this.chkKcfl.TabIndex = 4;
            // 
            // chkFZ
            // 
            this.chkFZ.Location = new System.Drawing.Point(169, 173);
            this.chkFZ.Name = "chkFZ";
            this.chkFZ.Properties.Caption = "辅助图层";
            this.chkFZ.Size = new System.Drawing.Size(133, 22);
            this.chkFZ.TabIndex = 5;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(372, 111);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(78, 31);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "提取";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(372, 163);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(78, 31);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtSHP
            // 
            this.txtSHP.Enabled = false;
            this.txtSHP.Location = new System.Drawing.Point(169, 58);
            this.txtSHP.Name = "txtSHP";
            this.txtSHP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSHP.Size = new System.Drawing.Size(296, 24);
            this.txtSHP.TabIndex = 10;
            this.txtSHP.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtSHP_ButtonClick);
            // 
            // rdoResource
            // 
            this.rdoResource.Location = new System.Drawing.Point(12, 3);
            this.rdoResource.Name = "rdoResource";
            this.rdoResource.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoResource.Properties.Appearance.Options.UseBackColor = true;
            this.rdoResource.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoResource.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选择数据源图层："),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选择外部数据源：")});
            this.rdoResource.Size = new System.Drawing.Size(151, 91);
            this.rdoResource.TabIndex = 11;
            this.rdoResource.SelectedIndexChanged += new System.EventHandler(this.rdoResource_SelectedIndexChanged);
            // 
            // chkSD
            // 
            this.chkSD.Location = new System.Drawing.Point(169, 209);
            this.chkSD.Name = "chkSD";
            this.chkSD.Properties.Caption = "三调耕地质量图层（提取自分类单元）";
            this.chkSD.Size = new System.Drawing.Size(306, 22);
            this.chkSD.TabIndex = 12;
            // 
            // FrmGetGDData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 245);
            this.Controls.Add(this.chkSD);
            this.Controls.Add(this.rdoResource);
            this.Controls.Add(this.txtSHP);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.chkFZ);
            this.Controls.Add(this.chkKcfl);
            this.Controls.Add(this.chkFL);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbSourceLay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetGDData";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "耕地数据提取";
            this.Load += new System.EventHandler(this.FrmGetGDData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbSourceLay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkKcfl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoResource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSD.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbSourceLay;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chkFL;
        private DevExpress.XtraEditors.CheckEdit chkKcfl;
        private DevExpress.XtraEditors.CheckEdit chkFZ;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ButtonEdit txtSHP;
        private DevExpress.XtraEditors.RadioGroup rdoResource;
        private DevExpress.XtraEditors.CheckEdit chkSD;
    }
}