namespace TDDC3D.analysis
{
    partial class GetLL2d23dForm
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
            this.beDltbHShp = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.beXZDWHShp = new DevExpress.XtraEditors.ButtonEdit();
            this.chkTKXS = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.rdoXZDW = new DevExpress.XtraEditors.RadioGroup();
            this.btnCompute = new DevExpress.XtraEditors.SimpleButton();
            this.btnCompute2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.beDltbHShp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beXZDWHShp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTKXS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoXZDW.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beDltbHShp
            // 
            this.beDltbHShp.Location = new System.Drawing.Point(236, 28);
            this.beDltbHShp.Name = "beDltbHShp";
            this.beDltbHShp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDltbHShp.Properties.ReadOnly = true;
            this.beDltbHShp.Size = new System.Drawing.Size(391, 24);
            this.beDltbHShp.TabIndex = 1;
            this.beDltbHShp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDltbHShp_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(34, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(170, 18);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "1.选择二调DLTB SHP图层";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(34, 126);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(139, 18);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "3.选择三调DLTB图层";
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(236, 123);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbLayers.Size = new System.Drawing.Size(391, 24);
            this.cmbLayers.TabIndex = 8;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(417, 153);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(102, 31);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "运算分析";
            this.simpleButton1.Visible = false;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(544, 195);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(83, 31);
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(184, 195);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(132, 31);
            this.simpleButton3.TabIndex = 11;
            this.simpleButton3.Text = "导出县级变更表";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(136, 248);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(119, 31);
            this.simpleButton4.TabIndex = 14;
            this.simpleButton4.Text = "导出一览表";
            this.simpleButton4.Visible = false;
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // memoLog
            // 
            this.memoLog.Location = new System.Drawing.Point(33, 266);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(594, 339);
            this.memoLog.TabIndex = 15;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(34, 89);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(175, 18);
            this.labelControl3.TabIndex = 17;
            this.labelControl3.Text = "2.选择二调XZDW SHP图层";
            // 
            // beXZDWHShp
            // 
            this.beXZDWHShp.Location = new System.Drawing.Point(236, 86);
            this.beXZDWHShp.Name = "beXZDWHShp";
            this.beXZDWHShp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beXZDWHShp.Properties.ReadOnly = true;
            this.beXZDWHShp.Size = new System.Drawing.Size(391, 24);
            this.beXZDWHShp.TabIndex = 16;
            this.beXZDWHShp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beXZDWHShp_ButtonClick);
            // 
            // chkTKXS
            // 
            this.chkTKXS.Location = new System.Drawing.Point(236, 58);
            this.chkTKXS.Name = "chkTKXS";
            this.chkTKXS.Properties.Caption = "田坎系数除以100";
            this.chkTKXS.Size = new System.Drawing.Size(181, 22);
            this.chkTKXS.TabIndex = 18;
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(364, 195);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(132, 31);
            this.simpleButton5.TabIndex = 19;
            this.simpleButton5.Text = "导出乡级变更表";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // rdoXZDW
            // 
            this.rdoXZDW.Location = new System.Drawing.Point(34, 168);
            this.rdoXZDW.Name = "rdoXZDW";
            this.rdoXZDW.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoXZDW.Properties.Appearance.Options.UseBackColor = true;
            this.rdoXZDW.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoXZDW.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "直接扣除线状地物面积"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "线状地物转面后计算面积")});
            this.rdoXZDW.Size = new System.Drawing.Size(503, 30);
            this.rdoXZDW.TabIndex = 20;
            this.rdoXZDW.Visible = false;
            // 
            // btnCompute
            // 
            this.btnCompute.Location = new System.Drawing.Point(34, 195);
            this.btnCompute.Name = "btnCompute";
            this.btnCompute.Size = new System.Drawing.Size(102, 31);
            this.btnCompute.TabIndex = 21;
            this.btnCompute.Text = "运算分析";
            this.btnCompute.Click += new System.EventHandler(this.btnCompute_Click);
            // 
            // btnCompute2
            // 
            this.btnCompute2.Location = new System.Drawing.Point(525, 153);
            this.btnCompute2.Name = "btnCompute2";
            this.btnCompute2.Size = new System.Drawing.Size(102, 31);
            this.btnCompute2.TabIndex = 22;
            this.btnCompute2.Text = "运算分析";
            this.btnCompute2.Visible = false;
            this.btnCompute2.Click += new System.EventHandler(this.btnCompute2_Click);
            // 
            // GetLL2d23dForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 636);
            this.Controls.Add(this.btnCompute2);
            this.Controls.Add(this.btnCompute);
            this.Controls.Add(this.rdoXZDW);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.chkTKXS);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.beXZDWHShp);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.beDltbHShp);
            this.Name = "GetLL2d23dForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "二调到三调的流量变化对比";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetLL2d23dForm_FormClosed);
            this.Load += new System.EventHandler(this.GetLL2d23dForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.beDltbHShp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beXZDWHShp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTKXS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoXZDW.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit beDltbHShp;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ButtonEdit beXZDWHShp;
        private DevExpress.XtraEditors.CheckEdit chkTKXS;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.RadioGroup rdoXZDW;
        private DevExpress.XtraEditors.SimpleButton btnCompute;
        private DevExpress.XtraEditors.SimpleButton btnCompute2;
    }
}