namespace TDDC3D.gengxin
{
    partial class FrmTBBHOpt2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTBBHOpt2));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cboDLTBGX = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtTBBH = new DevExpress.XtraEditors.TextEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.txtBeforeTBH = new DevExpress.XtraEditors.TextEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnCompute = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTBBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBeforeTBH.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "地类图斑更新层：";
            // 
            // cboDLTBGX
            // 
            this.cboDLTBGX.Location = new System.Drawing.Point(149, 21);
            this.cboDLTBGX.Name = "cboDLTBGX";
            this.cboDLTBGX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDLTBGX.Size = new System.Drawing.Size(220, 24);
            this.cboDLTBGX.TabIndex = 1;
            this.cboDLTBGX.SelectedIndexChanged += new System.EventHandler(this.cboDLTBGX_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(38, 78);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(105, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "图斑初始编号：";
            // 
            // txtTBBH
            // 
            this.txtTBBH.Location = new System.Drawing.Point(149, 75);
            this.txtTBBH.Name = "txtTBBH";
            this.txtTBBH.Properties.Mask.EditMask = "[0-9]*";
            this.txtTBBH.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtTBBH.Size = new System.Drawing.Size(220, 24);
            this.txtTBBH.TabIndex = 3;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(23, 130);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "图斑号前缀：";
            this.checkEdit1.Size = new System.Drawing.Size(120, 22);
            this.checkEdit1.TabIndex = 4;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // txtBeforeTBH
            // 
            this.txtBeforeTBH.Enabled = false;
            this.txtBeforeTBH.Location = new System.Drawing.Point(149, 129);
            this.txtBeforeTBH.Name = "txtBeforeTBH";
            this.txtBeforeTBH.Size = new System.Drawing.Size(220, 24);
            this.txtBeforeTBH.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(298, 194);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(71, 31);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCompute
            // 
            this.btnCompute.Image = ((System.Drawing.Image)(resources.GetObject("btnCompute.Image")));
            this.btnCompute.Location = new System.Drawing.Point(140, 194);
            this.btnCompute.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCompute.Name = "btnCompute";
            this.btnCompute.Size = new System.Drawing.Size(137, 31);
            this.btnCompute.TabIndex = 6;
            this.btnCompute.Text = "图斑编号赋值";
            this.btnCompute.Click += new System.EventHandler(this.btnCompute_Click);
            // 
            // FrmTBBHOpt2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 252);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCompute);
            this.Controls.Add(this.txtBeforeTBH);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.txtTBBH);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cboDLTBGX);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTBBHOpt2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图斑编号重计算";
            this.Load += new System.EventHandler(this.FrmTBBHOpt2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTBBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBeforeTBH.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboDLTBGX;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtTBBH;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.TextEdit txtBeforeTBH;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnCompute;
    }
}