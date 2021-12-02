namespace TDDC3D.gengxin
{
    partial class FrmXZQTZLX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmXZQTZLX));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnUpdate = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cboValue = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboXZQ = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cboDLTBGX = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.btnClose2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpdate2 = new DevExpress.XtraEditors.SimpleButton();
            this.cboValue2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.cboDLTBGX2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboValue2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(506, 292);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.btnClose);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.btnUpdate);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.cboValue);
            this.xtraTabPage1.Controls.Add(this.cboXZQ);
            this.xtraTabPage1.Controls.Add(this.labelControl4);
            this.xtraTabPage1.Controls.Add(this.cboDLTBGX);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(500, 259);
            this.xtraTabPage1.Text = "调入";
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(372, 82);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 37);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose2_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(42, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 18);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "行政区图层：";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.Location = new System.Drawing.Point(372, 29);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(88, 37);
            this.btnUpdate.TabIndex = 16;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(42, 82);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(90, 18);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "图斑更新层：";
            // 
            // cboValue
            // 
            this.cboValue.EditValue = "1|调入本区县范围的地类图斑";
            this.cboValue.Location = new System.Drawing.Point(138, 132);
            this.cboValue.Name = "cboValue";
            this.cboValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboValue.Properties.Items.AddRange(new object[] {
            "1|调入本区县范围的地类图斑",
            "3|由于国界线、零米线变化，新增的地类图斑"});
            this.cboValue.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboValue.Size = new System.Drawing.Size(213, 24);
            this.cboValue.TabIndex = 15;
            // 
            // cboXZQ
            // 
            this.cboXZQ.Location = new System.Drawing.Point(138, 26);
            this.cboXZQ.Name = "cboXZQ";
            this.cboXZQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboXZQ.Size = new System.Drawing.Size(213, 24);
            this.cboXZQ.TabIndex = 11;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 135);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(120, 18);
            this.labelControl4.TabIndex = 14;
            this.labelControl4.Text = "行政区调整类型：";
            // 
            // cboDLTBGX
            // 
            this.cboDLTBGX.Location = new System.Drawing.Point(138, 79);
            this.cboDLTBGX.Name = "cboDLTBGX";
            this.cboDLTBGX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDLTBGX.Size = new System.Drawing.Size(213, 24);
            this.cboDLTBGX.TabIndex = 12;
            // 
            // labelControl3
            // 
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl3.Location = new System.Drawing.Point(30, 189);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(417, 54);
            this.labelControl3.TabIndex = 13;
            this.labelControl3.Text = "      根据图斑更新层是否在行政区范围内，更新图斑更新层的行政区调整类型属性。在行政区范围内为0，不在行政区范围内容为自选类型。";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.btnClose2);
            this.xtraTabPage2.Controls.Add(this.btnUpdate2);
            this.xtraTabPage2.Controls.Add(this.cboValue2);
            this.xtraTabPage2.Controls.Add(this.labelControl5);
            this.xtraTabPage2.Controls.Add(this.labelControl6);
            this.xtraTabPage2.Controls.Add(this.cboDLTBGX2);
            this.xtraTabPage2.Controls.Add(this.labelControl7);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(500, 259);
            this.xtraTabPage2.Text = "调出";
            // 
            // btnClose2
            // 
            this.btnClose2.Image = ((System.Drawing.Image)(resources.GetObject("btnClose2.Image")));
            this.btnClose2.Location = new System.Drawing.Point(372, 82);
            this.btnClose2.Name = "btnClose2";
            this.btnClose2.Size = new System.Drawing.Size(88, 37);
            this.btnClose2.TabIndex = 17;
            this.btnClose2.Text = "关闭";
            this.btnClose2.Click += new System.EventHandler(this.btnClose2_Click);
            // 
            // btnUpdate2
            // 
            this.btnUpdate2.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate2.Image")));
            this.btnUpdate2.Location = new System.Drawing.Point(372, 29);
            this.btnUpdate2.Name = "btnUpdate2";
            this.btnUpdate2.Size = new System.Drawing.Size(88, 37);
            this.btnUpdate2.TabIndex = 16;
            this.btnUpdate2.Text = "更新";
            this.btnUpdate2.Click += new System.EventHandler(this.btnUpdate2_Click);
            // 
            // cboValue2
            // 
            this.cboValue2.EditValue = "2|调出本区县范围的地类图斑";
            this.cboValue2.Location = new System.Drawing.Point(138, 95);
            this.cboValue2.Name = "cboValue2";
            this.cboValue2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboValue2.Properties.Items.AddRange(new object[] {
            "2|调出本区县范围的地类图斑",
            "4|由于国界线、零米线变化，减少的地类图斑"});
            this.cboValue2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboValue2.Size = new System.Drawing.Size(213, 24);
            this.cboValue2.TabIndex = 15;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 98);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(120, 18);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "行政区调整类型：";
            // 
            // labelControl6
            // 
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl6.Location = new System.Drawing.Point(30, 167);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(417, 36);
            this.labelControl6.TabIndex = 13;
            this.labelControl6.Text = "      选择需要调出的图斑更新层数据，然后将选择数据根据自定义调整类型更新行政区调整类型属性。";
            // 
            // cboDLTBGX2
            // 
            this.cboDLTBGX2.Location = new System.Drawing.Point(138, 36);
            this.cboDLTBGX2.Name = "cboDLTBGX2";
            this.cboDLTBGX2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDLTBGX2.Size = new System.Drawing.Size(213, 24);
            this.cboDLTBGX2.TabIndex = 12;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(42, 39);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(90, 18);
            this.labelControl7.TabIndex = 10;
            this.labelControl7.Text = "图斑更新层：";
            // 
            // FrmXZQTZLX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 292);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmXZQTZLX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "更新行政区调整类型属性";
            this.Load += new System.EventHandler(this.FrmXZQTZLX_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboValue2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBGX2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnUpdate;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cboValue;
        private DevExpress.XtraEditors.ComboBoxEdit cboXZQ;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cboDLTBGX;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.SimpleButton btnClose2;
        private DevExpress.XtraEditors.SimpleButton btnUpdate2;
        private DevExpress.XtraEditors.ComboBoxEdit cboValue2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit cboDLTBGX2;
        private DevExpress.XtraEditors.LabelControl labelControl7;

    }
}