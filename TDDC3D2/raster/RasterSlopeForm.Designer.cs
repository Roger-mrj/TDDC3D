namespace TDDC3D.raster
{
    partial class RasterSlopeForm
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
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.beDestRasterPath = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestRasterPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(93, 19);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(422, 24);
            this.cmbLayers.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "输入影像";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(16, 64);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "输入影像";
            // 
            // beDestRasterPath
            // 
            this.beDestRasterPath.Location = new System.Drawing.Point(93, 62);
            this.beDestRasterPath.Name = "beDestRasterPath";
            this.beDestRasterPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestRasterPath.Size = new System.Drawing.Size(422, 24);
            this.beDestRasterPath.TabIndex = 8;
            this.beDestRasterPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestRasterPath_ButtonClick);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(93, 122);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(105, 35);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(312, 122);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(105, 35);
            this.simpleButton2.TabIndex = 10;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // RasterSlopeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 182);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.beDestRasterPath);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RasterSlopeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坡度分析";
            this.Load += new System.EventHandler(this.RasterSlopeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestRasterPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ButtonEdit beDestRasterPath;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}