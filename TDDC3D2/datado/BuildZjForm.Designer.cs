namespace TDDC3D.datado
{
    partial class BuildZjForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuildZjForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.chkListYsdm = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.cboFont = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboSize = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbColor = new DevExpress.XtraEditors.ColorEdit();
            this.spinJg = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtXian = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkListYsdm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFont.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinJg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.chkListYsdm);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(275, 315);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "选择要素代码";
            // 
            // chkListYsdm
            // 
            this.chkListYsdm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkListYsdm.Location = new System.Drawing.Point(2, 22);
            this.chkListYsdm.Name = "chkListYsdm";
            this.chkListYsdm.Size = new System.Drawing.Size(271, 291);
            this.chkListYsdm.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(294, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "选择字体";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(294, 69);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "选择颜色";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(294, 103);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "选择大小";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(294, 137);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(24, 14);
            this.labelControl6.TabIndex = 6;
            this.labelControl6.Text = "间隔";
            // 
            // cboFont
            // 
            this.cboFont.Location = new System.Drawing.Point(362, 30);
            this.cboFont.Margin = new System.Windows.Forms.Padding(2);
            this.cboFont.Name = "cboFont";
            this.cboFont.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFont.Size = new System.Drawing.Size(181, 20);
            this.cboFont.TabIndex = 171;
            // 
            // cboSize
            // 
            this.cboSize.Location = new System.Drawing.Point(362, 100);
            this.cboSize.Margin = new System.Windows.Forms.Padding(2);
            this.cboSize.Name = "cboSize";
            this.cboSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSize.Properties.Items.AddRange(new object[] {
            "初号|42||14.826",
            "小初|36|12.708",
            "一号|26|9.178",
            "二号|24|7.766",
            "小二|22|6.354",
            "三号|20|5.648",
            "小三|18|5.295",
            "四号|14|4.942",
            "小四|12|4.236",
            "五号|10.5|3.707",
            "小五|9|3.1779",
            "六号|7.5|2.648",
            "小六|6.5|2.295",
            "七号|5.5|1.942",
            "八号|5|1.765"});
            this.cboSize.Size = new System.Drawing.Size(181, 20);
            this.cboSize.TabIndex = 172;
            // 
            // cbColor
            // 
            this.cbColor.EditValue = System.Drawing.Color.Black;
            this.cbColor.Location = new System.Drawing.Point(362, 63);
            this.cbColor.Margin = new System.Windows.Forms.Padding(2);
            this.cbColor.Name = "cbColor";
            this.cbColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbColor.Size = new System.Drawing.Size(181, 20);
            this.cbColor.TabIndex = 173;
            // 
            // spinJg
            // 
            this.spinJg.EditValue = new decimal(new int[] {
            1765,
            0,
            0,
            196608});
            this.spinJg.Location = new System.Drawing.Point(362, 134);
            this.spinJg.Name = "spinJg";
            this.spinJg.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinJg.Size = new System.Drawing.Size(181, 20);
            this.spinJg.TabIndex = 176;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(308, 263);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(70, 25);
            this.simpleButton1.TabIndex = 177;
            this.simpleButton1.Text = "生成";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(441, 263);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(70, 25);
            this.simpleButton2.TabIndex = 178;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtXian
            // 
            this.txtXian.Location = new System.Drawing.Point(362, 189);
            this.txtXian.Margin = new System.Windows.Forms.Padding(2);
            this.txtXian.Name = "txtXian";
            this.txtXian.Size = new System.Drawing.Size(181, 20);
            this.txtXian.TabIndex = 180;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(288, 192);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 179;
            this.labelControl4.Text = "当前县代码";
            // 
            // BuildZjForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 315);
            this.Controls.Add(this.txtXian);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.spinJg);
            this.Controls.Add(this.cbColor);
            this.Controls.Add(this.cboSize);
            this.Controls.Add(this.cboFont);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.groupControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuildZjForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成注记";
            this.Load += new System.EventHandler(this.BuildZjForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkListYsdm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFont.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinJg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXian.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListYsdm;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.ComboBoxEdit cboFont;
        private DevExpress.XtraEditors.ComboBoxEdit cboSize;
        private DevExpress.XtraEditors.ColorEdit cbColor;
        private DevExpress.XtraEditors.SpinEdit spinJg;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.TextEdit txtXian;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}