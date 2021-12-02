namespace TDDC3D.gengxin
{
    partial class FrmCreateProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCreateProcess));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.checkDLTB = new DevExpress.XtraEditors.CheckEdit();
            this.checkXZQ = new DevExpress.XtraEditors.CheckEdit();
            this.checkCJDCQ = new DevExpress.XtraEditors.CheckEdit();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.txtXian = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.txtMj = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.checkDLTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkXZQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkCJDCQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMj.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 116);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(195, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择将要生成的更新过程层：";
            // 
            // checkDLTB
            // 
            this.checkDLTB.Location = new System.Drawing.Point(26, 148);
            this.checkDLTB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkDLTB.Name = "checkDLTB";
            this.checkDLTB.Properties.Caption = "1.地类图斑";
            this.checkDLTB.Size = new System.Drawing.Size(120, 22);
            this.checkDLTB.TabIndex = 1;
            // 
            // checkXZQ
            // 
            this.checkXZQ.Location = new System.Drawing.Point(302, 148);
            this.checkXZQ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkXZQ.Name = "checkXZQ";
            this.checkXZQ.Properties.Caption = "3.行政区";
            this.checkXZQ.Size = new System.Drawing.Size(104, 22);
            this.checkXZQ.TabIndex = 2;
            // 
            // checkCJDCQ
            // 
            this.checkCJDCQ.Location = new System.Drawing.Point(152, 148);
            this.checkCJDCQ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkCJDCQ.Name = "checkCJDCQ";
            this.checkCJDCQ.Properties.Caption = "2.村级调查区";
            this.checkCJDCQ.Size = new System.Drawing.Size(137, 22);
            this.checkCJDCQ.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(308, 257);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 31);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "取消";
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(212, 257);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(77, 31);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(26, 308);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(380, 232);
            this.info.TabIndex = 16;
            this.info.UseOptimizedRendering = true;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(26, 184);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(105, 18);
            this.labelControl2.TabIndex = 17;
            this.labelControl2.Text = "选择更新年份：";
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(26, 218);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.dateEdit1.Size = new System.Drawing.Size(359, 24);
            this.dateEdit1.TabIndex = 18;
            // 
            // txtXian
            // 
            this.txtXian.Location = new System.Drawing.Point(136, 12);
            this.txtXian.Name = "txtXian";
            this.txtXian.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtXian.Size = new System.Drawing.Size(249, 24);
            this.txtXian.TabIndex = 20;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(26, 15);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 18);
            this.labelControl3.TabIndex = 19;
            this.labelControl3.Text = "当前县代码";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(26, 51);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "若存在行政区调整，调整前后控制面积差值";
            this.checkEdit1.Size = new System.Drawing.Size(359, 22);
            this.checkEdit1.TabIndex = 21;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // txtMj
            // 
            this.txtMj.Enabled = false;
            this.txtMj.Location = new System.Drawing.Point(238, 79);
            this.txtMj.Name = "txtMj";
            this.txtMj.Size = new System.Drawing.Size(140, 24);
            this.txtMj.TabIndex = 22;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(26, 82);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(185, 18);
            this.labelControl4.TabIndex = 23;
            this.labelControl4.Text = "调整后-调整前（平方米）：";
            // 
            // FrmCreateProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 559);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtMj);
            this.Controls.Add(this.checkEdit1);
            this.Controls.Add(this.txtXian);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.dateEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkCJDCQ);
            this.Controls.Add(this.checkXZQ);
            this.Controls.Add(this.checkDLTB);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCreateProcess";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取更新过程";
            this.Load += new System.EventHandler(this.FrmCreateProcess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkDLTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkXZQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkCJDCQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMj.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit checkDLTB;
        private DevExpress.XtraEditors.CheckEdit checkXZQ;
        private DevExpress.XtraEditors.CheckEdit checkCJDCQ;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit txtXian;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.TextEdit txtMj;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}