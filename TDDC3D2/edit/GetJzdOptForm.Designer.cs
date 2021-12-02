namespace TDDC3D.edit
{
    partial class GetJzdOptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetJzdOptForm));
            this.cmbPointlayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbPolygonLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPointlayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPointlayer
            // 
            this.cmbPointlayer.Location = new System.Drawing.Point(25, 106);
            this.cmbPointlayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPointlayer.Name = "cmbPointlayer";
            this.cmbPointlayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPointlayer.Size = new System.Drawing.Size(279, 20);
            this.cmbPointlayer.TabIndex = 19;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(26, 83);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "选择点层";
            // 
            // cmbPolygonLayer
            // 
            this.cmbPolygonLayer.Location = new System.Drawing.Point(26, 55);
            this.cmbPolygonLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPolygonLayer.Name = "cmbPolygonLayer";
            this.cmbPolygonLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPolygonLayer.Size = new System.Drawing.Size(279, 20);
            this.cmbPolygonLayer.TabIndex = 17;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 30);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "选择面层";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(52, 174);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 14;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(186, 174);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 15;
            this.simpleButton2.Text = "取消";
            // 
            // GetJzdOptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 238);
            this.Controls.Add(this.cmbPointlayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbPolygonLayer);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GetJzdOptForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取界址点";
            this.Load += new System.EventHandler(this.GetJzdOptForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPointlayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbPointlayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPolygonLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}