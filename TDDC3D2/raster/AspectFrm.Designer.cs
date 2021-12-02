namespace TDDC3D.raster
{
    partial class AspectFrm
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
            this.label1 = new System.Windows.Forms.Label();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.beOutputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beInputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.beOutputRaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(359, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "获得栅格表面的坡向。坡向用于识别从每个像元\r\n到其相邻像元方向上值的变化率最大的下坡方向。";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(252, 196);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(91, 34);
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(68, 196);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(91, 34);
            this.simpleButton1.TabIndex = 13;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // beOutputRaster
            // 
            this.beOutputRaster.Location = new System.Drawing.Point(99, 126);
            this.beOutputRaster.Name = "beOutputRaster";
            this.beOutputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beOutputRaster.Size = new System.Drawing.Size(326, 24);
            this.beOutputRaster.TabIndex = 12;
            this.beOutputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beOutputRaster_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(19, 129);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "输出栅格";
            // 
            // beInputRaster
            // 
            this.beInputRaster.Location = new System.Drawing.Point(99, 87);
            this.beInputRaster.Name = "beInputRaster";
            this.beInputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beInputRaster.Size = new System.Drawing.Size(326, 24);
            this.beInputRaster.TabIndex = 10;
            this.beInputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beInputRaster_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 90);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "输入栅格";
            // 
            // AspectFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 264);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.beOutputRaster);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.beInputRaster);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AspectFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坡向分析";
            ((System.ComponentModel.ISupportInitialize)(this.beOutputRaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ButtonEdit beOutputRaster;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beInputRaster;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}