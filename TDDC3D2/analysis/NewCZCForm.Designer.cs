namespace TDDC3D.analysis
{
    partial class NewCZCForm
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
            this.txtSHPResult = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtSHPED = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txt20 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtjs = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnPick = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl35 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHPResult.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHPED.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt20.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtjs.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSHPResult
            // 
            this.txtSHPResult.Location = new System.Drawing.Point(91, 73);
            this.txtSHPResult.Name = "txtSHPResult";
            this.txtSHPResult.Properties.ReadOnly = true;
            this.txtSHPResult.Size = new System.Drawing.Size(462, 24);
            this.txtSHPResult.TabIndex = 12;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 76);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(75, 18);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "结果目录：";
            // 
            // txtSHPED
            // 
            this.txtSHPED.Location = new System.Drawing.Point(91, 29);
            this.txtSHPED.Name = "txtSHPED";
            this.txtSHPED.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSHPED.Properties.ReadOnly = true;
            this.txtSHPED.Size = new System.Drawing.Size(460, 24);
            this.txtSHPED.TabIndex = 10;
            this.txtSHPED.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtSHPED_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 32);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "二调图斑：";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl35);
            this.groupControl1.Controls.Add(this.txt20);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.txtjs);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Location = new System.Drawing.Point(12, 118);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(541, 210);
            this.groupControl1.TabIndex = 35;
            this.groupControl1.Text = "提取规则";
            // 
            // txt20
            // 
            this.txt20.EditValue = "300";
            this.txt20.Location = new System.Drawing.Point(427, 127);
            this.txt20.Name = "txt20";
            this.txt20.Properties.Mask.EditMask = "[0-9]*";
            this.txt20.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txt20.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txt20.Properties.MaxLength = 2;
            this.txt20.Size = new System.Drawing.Size(44, 24);
            this.txt20.TabIndex = 60;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(23, 130);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(367, 18);
            this.labelControl8.TabIndex = 58;
            this.labelControl8.Text = "20属性发生变化（如203变202，204变203等）面积大于\r\n";
            // 
            // txtjs
            // 
            this.txtjs.EditValue = "300";
            this.txtjs.Location = new System.Drawing.Point(294, 64);
            this.txtjs.Name = "txtjs";
            this.txtjs.Properties.Mask.EditMask = "[0-9]*";
            this.txtjs.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtjs.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtjs.Properties.MaxLength = 2;
            this.txtjs.Size = new System.Drawing.Size(49, 24);
            this.txtjs.TabIndex = 56;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(23, 67);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(250, 18);
            this.labelControl5.TabIndex = 55;
            this.labelControl5.Text = "非建设用地 流向 城镇村范围面积大于";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(444, 348);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 45);
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPick
            // 
            this.btnPick.Location = new System.Drawing.Point(330, 349);
            this.btnPick.Name = "btnPick";
            this.btnPick.Size = new System.Drawing.Size(97, 45);
            this.btnPick.TabIndex = 36;
            this.btnPick.Text = "提取";
            this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
            // 
            // labelControl35
            // 
            this.labelControl35.Location = new System.Drawing.Point(349, 67);
            this.labelControl35.Name = "labelControl35";
            this.labelControl35.Size = new System.Drawing.Size(45, 18);
            this.labelControl35.TabIndex = 61;
            this.labelControl35.Text = "平方米";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(477, 130);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(45, 18);
            this.labelControl2.TabIndex = 62;
            this.labelControl2.Text = "平方米";
            // 
            // NewCZCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 418);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPick);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.txtSHPResult);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtSHPED);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewCZCForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "城镇村增加情况分析";
            ((System.ComponentModel.ISupportInitialize)(this.txtSHPResult.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSHPED.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt20.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtjs.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtSHPResult;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ButtonEdit txtSHPED;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit txt20;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.TextEdit txtjs;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnPick;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl35;
    }
}