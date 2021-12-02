namespace TDDC3D.raster
{
    partial class RasterReClassForm
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
            this.beOutputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beInputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.beOutputRaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beOutputRaster
            // 
            this.beOutputRaster.Location = new System.Drawing.Point(270, 61);
            this.beOutputRaster.Name = "beOutputRaster";
            this.beOutputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beOutputRaster.Size = new System.Drawing.Size(326, 24);
            this.beOutputRaster.TabIndex = 7;
            this.beOutputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beOutputRaster_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(190, 64);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "输出栅格";
            // 
            // beInputRaster
            // 
            this.beInputRaster.Location = new System.Drawing.Point(270, 22);
            this.beInputRaster.Name = "beInputRaster";
            this.beInputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beInputRaster.Size = new System.Drawing.Size(326, 24);
            this.beInputRaster.TabIndex = 5;
            this.beInputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beInputRaster_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(190, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "输入栅格";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelControl3.Location = new System.Drawing.Point(12, 12);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(117, 138);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "分级说明：\r\n0º—2º：1\r\n2º—6º：2\r\n6º—15º：3\r\n15º—25º：4\r\n25º—90º：5";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(525, 125);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 17;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(436, 125);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 16;
            this.simpleButton1.Text = "执行";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // RasterReClassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 192);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.beOutputRaster);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.beInputRaster);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RasterReClassForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "影像重分类";
            ((System.ComponentModel.ISupportInitialize)(this.beOutputRaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit beOutputRaster;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beInputRaster;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}