namespace TDDC3D.raster
{
    partial class SlopeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlopeForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.beInputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.beOutputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beOutputRaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 18);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "输入栅格";
            // 
            // beInputRaster
            // 
            this.beInputRaster.Location = new System.Drawing.Point(76, 16);
            this.beInputRaster.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.beInputRaster.Name = "beInputRaster";
            this.beInputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beInputRaster.Size = new System.Drawing.Size(244, 20);
            this.beInputRaster.TabIndex = 1;
            this.beInputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.buttonEdit1_ButtonClick);
            // 
            // beOutputRaster
            // 
            this.beOutputRaster.Location = new System.Drawing.Point(76, 47);
            this.beOutputRaster.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.beOutputRaster.Name = "beOutputRaster";
            this.beOutputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beOutputRaster.Size = new System.Drawing.Size(244, 20);
            this.beOutputRaster.TabIndex = 3;
            this.beOutputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beOutputRaster_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(16, 50);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "输出栅格";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(16, 84);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(104, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "测量单位：DEGREE";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(16, 114);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(31, 14);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "Z因子";
            // 
            // textEdit1
            // 
            this.textEdit1.EditValue = "1";
            this.textEdit1.Location = new System.Drawing.Point(76, 109);
            this.textEdit1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(75, 20);
            this.textEdit1.TabIndex = 6;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(76, 151);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 27);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(214, 151);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 27);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // SlopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 188);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.beOutputRaster);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.beInputRaster);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlopeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坡度分析";
            this.Load += new System.EventHandler(this.SlopeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beOutputRaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit beInputRaster;
        private DevExpress.XtraEditors.ButtonEdit beOutputRaster;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}