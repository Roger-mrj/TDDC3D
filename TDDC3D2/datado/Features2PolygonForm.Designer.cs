namespace TDDC3D.datado
{
    partial class Features2PolygonForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Features2PolygonForm));
            this.txtDestLayer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayer1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDestLayer
            // 
            this.txtDestLayer.Location = new System.Drawing.Point(28, 106);
            this.txtDestLayer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDestLayer.Name = "txtDestLayer";
            this.txtDestLayer.Size = new System.Drawing.Size(282, 20);
            this.txtDestLayer.TabIndex = 21;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(27, 74);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 20;
            this.labelControl3.Text = "目标图层名";
            // 
            // cmbLayer1
            // 
            this.cmbLayer1.Location = new System.Drawing.Point(26, 40);
            this.cmbLayer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbLayer1.Name = "cmbLayer1";
            this.cmbLayer1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer1.Size = new System.Drawing.Size(284, 20);
            this.cmbLayer1.TabIndex = 19;
            this.cmbLayer1.SelectedIndexChanged += new System.EventHandler(this.cmbLayer1_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 15);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "选择线图层";
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(185, 146);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 25);
            this.simpleButton2.TabIndex = 17;
            this.simpleButton2.Text = "取消";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(51, 146);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 25);
            this.simpleButton1.TabIndex = 16;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // Features2PolygonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 202);
            this.Controls.Add(this.txtDestLayer);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cmbLayer1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Features2PolygonForm";
            this.ShowIcon = false;
            this.Text = "要素转面";
            this.Load += new System.EventHandler(this.Features2PolygonForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtDestLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtDestLayer;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}