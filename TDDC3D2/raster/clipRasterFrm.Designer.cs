namespace TDDC3D.raster
{
    partial class clipRasterFrm
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beDestFilePath = new DevExpress.XtraEditors.ButtonEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtBuffer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBuffer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(18, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(105, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择源影像图层";
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(157, 21);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(308, 24);
            this.cmbLayers.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(18, 62);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 18);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "选择目标文件路径";
            // 
            // beDestFilePath
            // 
            this.beDestFilePath.Location = new System.Drawing.Point(157, 59);
            this.beDestFilePath.Name = "beDestFilePath";
            this.beDestFilePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestFilePath.Size = new System.Drawing.Size(308, 24);
            this.beDestFilePath.TabIndex = 6;
            this.beDestFilePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestFilePath_ButtonClick);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(18, 101);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选中面要素"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前视图区域")});
            this.radioGroup1.Size = new System.Drawing.Size(447, 35);
            this.radioGroup1.TabIndex = 7;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(79, 199);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(309, 199);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(18, 153);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 18);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "缓冲距离";
            // 
            // txtBuffer
            // 
            this.txtBuffer.EditValue = "1000";
            this.txtBuffer.Location = new System.Drawing.Point(158, 150);
            this.txtBuffer.Name = "txtBuffer";
            this.txtBuffer.Size = new System.Drawing.Size(100, 24);
            this.txtBuffer.TabIndex = 11;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(264, 153);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(15, 18);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "米";
            // 
            // clipRasterFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 253);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtBuffer);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.beDestFilePath);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.labelControl1);
            this.Name = "clipRasterFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "影像切割";
            this.Load += new System.EventHandler(this.clipRasterFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBuffer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beDestFilePath;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtBuffer;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}