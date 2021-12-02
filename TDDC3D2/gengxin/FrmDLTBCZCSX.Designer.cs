namespace TDDC3D.gengxin
{
    partial class FrmDLTBCZCSX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDLTBCZCSX));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cboCZCGX = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboXZQ = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cboDLTBGX = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cboCZCGX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 43);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 18);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "县级行政区代码：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(32, 96);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 18);
            this.labelControl2.TabIndex = 19;
            this.labelControl2.Text = "地类图斑更新层：";
            // 
            // cboCZCGX
            // 
            this.cboCZCGX.EditValue = "";
            this.cboCZCGX.Location = new System.Drawing.Point(163, 146);
            this.cboCZCGX.Name = "cboCZCGX";
            this.cboCZCGX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboCZCGX.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboCZCGX.Size = new System.Drawing.Size(213, 24);
            this.cboCZCGX.TabIndex = 23;
            // 
            // cboXZQ
            // 
            this.cboXZQ.Location = new System.Drawing.Point(163, 40);
            this.cboXZQ.Name = "cboXZQ";
            this.cboXZQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboXZQ.Properties.MaxLength = 6;
            this.cboXZQ.Size = new System.Drawing.Size(213, 24);
            this.cboXZQ.TabIndex = 20;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(47, 149);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(105, 18);
            this.labelControl4.TabIndex = 22;
            this.labelControl4.Text = "城镇村更新层：";
            // 
            // cboDLTBGX
            // 
            this.cboDLTBGX.Location = new System.Drawing.Point(163, 93);
            this.cboDLTBGX.Name = "cboDLTBGX";
            this.cboDLTBGX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDLTBGX.Size = new System.Drawing.Size(213, 24);
            this.cboDLTBGX.TabIndex = 21;
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(406, 96);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 37);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(406, 43);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(88, 37);
            this.btnUpdate.TabIndex = 24;
            this.btnUpdate.Text = "提取";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // FrmDLTBCZCSX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 208);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cboCZCGX);
            this.Controls.Add(this.cboXZQ);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.cboDLTBGX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDLTBCZCSX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取地类图斑更新层中城镇村属性代码";
            this.Load += new System.EventHandler(this.FrmDLTBCZCSX_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboCZCGX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cboCZCGX;
        private DevExpress.XtraEditors.ComboBoxEdit cboXZQ;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cboDLTBGX;
    }
}