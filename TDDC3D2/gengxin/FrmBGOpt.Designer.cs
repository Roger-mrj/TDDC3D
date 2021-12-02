namespace TDDC3D.gengxin
{
    partial class FrmBGOpt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBGOpt));
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.prgInfo = new DevExpress.XtraEditors.ProgressBarControl();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prgInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(321, 144);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 31);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(42, 12);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(105, 18);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "选择变更图斑层";
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(206, 144);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(77, 31);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(42, 41);
            this.cmbLayers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(356, 24);
            this.cmbLayers.TabIndex = 11;
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(21, 190);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(393, 185);
            this.info.TabIndex = 14;
            this.info.UseOptimizedRendering = true;
            // 
            // prgInfo
            // 
            this.prgInfo.Location = new System.Drawing.Point(21, 375);
            this.prgInfo.Name = "prgInfo";
            this.prgInfo.Size = new System.Drawing.Size(393, 20);
            this.prgInfo.TabIndex = 15;
            this.prgInfo.Visible = false;
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(42, 105);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.dateEdit1.Size = new System.Drawing.Size(356, 24);
            this.dateEdit1.TabIndex = 16;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(42, 76);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(90, 18);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "选择变更时间";
            // 
            // FrmBGOpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 399);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.dateEdit1);
            this.Controls.Add(this.prgInfo);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbLayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBGOpt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取更新层";
            this.Load += new System.EventHandler(this.FrmBGOpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prgInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.ProgressBarControl prgInfo;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}