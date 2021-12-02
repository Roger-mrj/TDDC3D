namespace RCIS.MapTool
{
    partial class GeoReferenceToolbarFrm
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbFeatureLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFeatureLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(11, 91);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(98, 30);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "添加影像点";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(110, 91);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(101, 30);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "添加矢量点";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(215, 91);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(67, 30);
            this.simpleButton3.TabIndex = 2;
            this.simpleButton3.Text = "配准";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "影像图层";
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(110, 11);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(493, 24);
            this.cmbLayers.TabIndex = 4;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(376, 91);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(69, 30);
            this.simpleButton4.TabIndex = 5;
            this.simpleButton4.Text = "清除选点";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(288, 91);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(82, 30);
            this.simpleButton5.TabIndex = 6;
            this.simpleButton5.Text = "取消配准";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(451, 91);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(69, 30);
            this.simpleButton6.TabIndex = 7;
            this.simpleButton6.Text = "保存";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Location = new System.Drawing.Point(533, 91);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(69, 30);
            this.simpleButton7.TabIndex = 8;
            this.simpleButton7.Text = "关闭";
            this.simpleButton7.Click += new System.EventHandler(this.simpleButton7_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(11, 52);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(45, 18);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "捕捉到";
            // 
            // cmbFeatureLayer
            // 
            this.cmbFeatureLayer.Location = new System.Drawing.Point(109, 49);
            this.cmbFeatureLayer.Name = "cmbFeatureLayer";
            this.cmbFeatureLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFeatureLayer.Size = new System.Drawing.Size(493, 24);
            this.cmbFeatureLayer.TabIndex = 10;
            // 
            // GeoReferenceToolbarFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 143);
            this.Controls.Add(this.cmbFeatureLayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton7);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeoReferenceToolbarFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "影像配准";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.GeoReferenceToolbarFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFeatureLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbFeatureLayer;
    }
}