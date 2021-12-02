namespace TDDC3D.edit
{
    partial class DelOutXianTbForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DelOutXianTbForm));
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.lblNum = new DevExpress.XtraEditors.LabelControl();
            this.cmbXZQLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbXZQLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(84, 18);
            this.cmbLayers.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(193, 20);
            this.cmbLayers.TabIndex = 9;
            this.cmbLayers.SelectedIndexChanged += new System.EventHandler(this.cmbLayers_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 20);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "要处理的图层";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(135, 112);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 7;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(47, 111);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 6;
            this.simpleButton1.Text = "执行";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // lblNum
            // 
            this.lblNum.Location = new System.Drawing.Point(21, 88);
            this.lblNum.Margin = new System.Windows.Forms.Padding(2);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(0, 14);
            this.lblNum.TabIndex = 10;
            // 
            // cmbXZQLayer
            // 
            this.cmbXZQLayer.Location = new System.Drawing.Point(84, 49);
            this.cmbXZQLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbXZQLayer.Name = "cmbXZQLayer";
            this.cmbXZQLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbXZQLayer.Size = new System.Drawing.Size(193, 20);
            this.cmbXZQLayer.TabIndex = 12;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(9, 51);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "行政区图层";
            // 
            // DelOutXianTbForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 162);
            this.Controls.Add(this.cmbXZQLayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblNum);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DelOutXianTbForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "删除行政区边界外图斑";
            this.Load += new System.EventHandler(this.DelOutXianTbForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbXZQLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl lblNum;
        private DevExpress.XtraEditors.ComboBoxEdit cmbXZQLayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}