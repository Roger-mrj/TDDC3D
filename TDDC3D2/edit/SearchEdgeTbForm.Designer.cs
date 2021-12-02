namespace TDDC3D.edit
{
    partial class SearchEdgeTbForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchEdgeTbForm));
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtZbuffer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtFBuffer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rdoLen = new DevExpress.XtraEditors.RadioGroup();
            this.spinEditLen2 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.spenEdtLen = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditAngle = new DevExpress.XtraEditors.SpinEdit();
            this.btnQuery2 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditAngle2 = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtZbuffer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFBuffer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditLen2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spenEdtLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAngle2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // memoEdit1
            // 
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Top;
            this.memoEdit1.EditValue = "    该功能采用正缓冲和负缓冲的方法在当前区域进行异常图形查询。\r\n    正缓冲距离一般为负缓冲距离的两倍+1";
            this.memoEdit1.Location = new System.Drawing.Point(2, 26);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.memoEdit1.Properties.Appearance.Options.UseFont = true;
            this.memoEdit1.Size = new System.Drawing.Size(736, 60);
            this.memoEdit1.TabIndex = 20;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(32, 42);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(412, 24);
            this.cmbLayer.TabIndex = 19;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 14);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 18);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "选择要查询的图层";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(579, 95);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(92, 32);
            this.simpleButton1.TabIndex = 21;
            this.simpleButton1.Text = "查询";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(579, 35);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(92, 32);
            this.simpleButton2.TabIndex = 22;
            this.simpleButton2.Text = "关闭";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(28, 101);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 23;
            this.labelControl2.Text = "正缓冲距离";
            // 
            // txtZbuffer
            // 
            this.txtZbuffer.EditValue = 5D;
            this.txtZbuffer.Location = new System.Drawing.Point(116, 99);
            this.txtZbuffer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtZbuffer.Name = "txtZbuffer";
            this.txtZbuffer.Size = new System.Drawing.Size(51, 24);
            this.txtZbuffer.TabIndex = 24;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(181, 102);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(15, 18);
            this.labelControl3.TabIndex = 25;
            this.labelControl3.Text = "米";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(421, 104);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(15, 18);
            this.labelControl4.TabIndex = 28;
            this.labelControl4.Text = "米";
            // 
            // txtFBuffer
            // 
            this.txtFBuffer.EditValue = 2D;
            this.txtFBuffer.Location = new System.Drawing.Point(356, 101);
            this.txtFBuffer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFBuffer.Name = "txtFBuffer";
            this.txtFBuffer.Size = new System.Drawing.Size(51, 24);
            this.txtFBuffer.TabIndex = 27;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(268, 102);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(75, 18);
            this.labelControl5.TabIndex = 26;
            this.labelControl5.Text = "负缓冲距离";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.memoEdit1);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.txtFBuffer);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.txtZbuffer);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 88);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(740, 150);
            this.groupControl1.TabIndex = 29;
            this.groupControl1.Text = "方法一";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmbLayer);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(740, 88);
            this.panelControl1.TabIndex = 30;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rdoLen);
            this.groupControl2.Controls.Add(this.spinEditLen2);
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.spenEdtLen);
            this.groupControl2.Controls.Add(this.labelControl7);
            this.groupControl2.Controls.Add(this.labelControl8);
            this.groupControl2.Controls.Add(this.labelControl9);
            this.groupControl2.Controls.Add(this.spinEditAngle);
            this.groupControl2.Controls.Add(this.btnQuery2);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 238);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(740, 139);
            this.groupControl2.TabIndex = 31;
            this.groupControl2.Text = "方法二";
            // 
            // rdoLen
            // 
            this.rdoLen.Location = new System.Drawing.Point(205, 36);
            this.rdoLen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rdoLen.Name = "rdoLen";
            this.rdoLen.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoLen.Properties.Appearance.Options.UseBackColor = true;
            this.rdoLen.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoLen.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "夹角两线段长度大于"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "夹角两线段长度相差大于")});
            this.rdoLen.Size = new System.Drawing.Size(221, 88);
            this.rdoLen.TabIndex = 38;
            // 
            // spinEditLen2
            // 
            this.spinEditLen2.EditValue = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.spinEditLen2.Location = new System.Drawing.Point(433, 89);
            this.spinEditLen2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spinEditLen2.Name = "spinEditLen2";
            this.spinEditLen2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditLen2.Size = new System.Drawing.Size(65, 24);
            this.spinEditLen2.TabIndex = 36;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(511, 92);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(15, 18);
            this.labelControl6.TabIndex = 37;
            this.labelControl6.Text = "米";
            // 
            // spenEdtLen
            // 
            this.spenEdtLen.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spenEdtLen.Location = new System.Drawing.Point(433, 45);
            this.spenEdtLen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spenEdtLen.Name = "spenEdtLen";
            this.spenEdtLen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spenEdtLen.Size = new System.Drawing.Size(65, 24);
            this.spenEdtLen.TabIndex = 32;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(511, 49);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(15, 18);
            this.labelControl7.TabIndex = 34;
            this.labelControl7.Text = "米";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(32, 51);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(60, 18);
            this.labelControl8.TabIndex = 29;
            this.labelControl8.Text = "夹角小于";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(185, 52);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(15, 18);
            this.labelControl9.TabIndex = 33;
            this.labelControl9.Text = "度";
            // 
            // spinEditAngle
            // 
            this.spinEditAngle.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinEditAngle.Location = new System.Drawing.Point(101, 48);
            this.spinEditAngle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spinEditAngle.Name = "spinEditAngle";
            this.spinEditAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditAngle.Size = new System.Drawing.Size(75, 24);
            this.spinEditAngle.TabIndex = 30;
            // 
            // btnQuery2
            // 
            this.btnQuery2.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery2.Image")));
            this.btnQuery2.Location = new System.Drawing.Point(579, 42);
            this.btnQuery2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnQuery2.Name = "btnQuery2";
            this.btnQuery2.Size = new System.Drawing.Size(92, 32);
            this.btnQuery2.TabIndex = 21;
            this.btnQuery2.Text = "查询";
            this.btnQuery2.Click += new System.EventHandler(this.btnQuery2_Click);
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.btnExport);
            this.groupControl3.Controls.Add(this.labelControl11);
            this.groupControl3.Controls.Add(this.labelControl12);
            this.groupControl3.Controls.Add(this.spinEditAngle2);
            this.groupControl3.Controls.Add(this.simpleButton3);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 377);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(740, 91);
            this.groupControl3.TabIndex = 32;
            this.groupControl3.Text = "方法三";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(32, 45);
            this.labelControl11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(60, 18);
            this.labelControl11.TabIndex = 29;
            this.labelControl11.Text = "夹角小于";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(185, 46);
            this.labelControl12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(15, 18);
            this.labelControl12.TabIndex = 33;
            this.labelControl12.Text = "度";
            // 
            // spinEditAngle2
            // 
            this.spinEditAngle2.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinEditAngle2.Location = new System.Drawing.Point(101, 42);
            this.spinEditAngle2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spinEditAngle2.Name = "spinEditAngle2";
            this.spinEditAngle2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditAngle2.Size = new System.Drawing.Size(75, 24);
            this.spinEditAngle2.TabIndex = 30;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(579, 39);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(92, 32);
            this.simpleButton3.TabIndex = 21;
            this.simpleButton3.Text = "查询";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(457, 39);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(92, 32);
            this.btnExport.TabIndex = 34;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // SearchEdgeTbForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 469);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchEdgeTbForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "狭长图斑查询";
            this.Load += new System.EventHandler(this.SearchEdgeTbForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtZbuffer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFBuffer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditLen2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spenEdtLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditAngle2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtZbuffer;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtFBuffer;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnQuery2;
        private DevExpress.XtraEditors.SpinEdit spenEdtLen;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SpinEdit spinEditAngle;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.SpinEdit spinEditAngle2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.RadioGroup rdoLen;
        private DevExpress.XtraEditors.SpinEdit spinEditLen2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SimpleButton btnExport;
    }
}