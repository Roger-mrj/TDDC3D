namespace TDDC3D.raster
{
    partial class Raster2PolygonFrm
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
            this.beInputRaster = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beOutputShp = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beOutputShp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beInputRaster
            // 
            this.beInputRaster.Location = new System.Drawing.Point(97, 21);
            this.beInputRaster.Name = "beInputRaster";
            this.beInputRaster.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beInputRaster.Size = new System.Drawing.Size(326, 24);
            this.beInputRaster.TabIndex = 7;
            this.beInputRaster.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beInputRaster_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "输入栅格";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 71);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(56, 18);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "输出SHP";
            // 
            // beOutputShp
            // 
            this.beOutputShp.Location = new System.Drawing.Point(97, 68);
            this.beOutputShp.Name = "beOutputShp";
            this.beOutputShp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beOutputShp.Size = new System.Drawing.Size(326, 24);
            this.beOutputShp.TabIndex = 9;
            this.beOutputShp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beOutputShp_ButtonClick);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(294, 475);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 19;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(97, 475);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 18;
            this.simpleButton1.Text = "执行";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // memoEdit1
            // 
            this.memoEdit1.Location = new System.Drawing.Point(21, 114);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(421, 315);
            this.memoEdit1.TabIndex = 20;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // Raster2PolygonFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 522);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.beOutputShp);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.beInputRaster);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Raster2PolygonFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "影像转面";
            ((System.ComponentModel.ISupportInitialize)(this.beInputRaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beOutputShp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit beInputRaster;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beOutputShp;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
    }
}