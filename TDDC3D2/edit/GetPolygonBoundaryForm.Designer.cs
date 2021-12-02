namespace TDDC3D.edit
{
    partial class GetPolygonBoundaryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetPolygonBoundaryForm));
            this.cmbPolylinelayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbPolygonLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolylinelayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbPolylinelayer
            // 
            this.cmbPolylinelayer.Location = new System.Drawing.Point(4, 114);
            this.cmbPolylinelayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPolylinelayer.Name = "cmbPolylinelayer";
            this.cmbPolylinelayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPolylinelayer.Size = new System.Drawing.Size(279, 20);
            this.cmbPolylinelayer.TabIndex = 13;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 92);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "选择界线层";
            // 
            // cmbPolygonLayer
            // 
            this.cmbPolygonLayer.Location = new System.Drawing.Point(5, 64);
            this.cmbPolygonLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPolygonLayer.Name = "cmbPolygonLayer";
            this.cmbPolygonLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPolygonLayer.Size = new System.Drawing.Size(279, 20);
            this.cmbPolygonLayer.TabIndex = 11;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 39);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "选择面层";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(48, 179);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(66, 23);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(173, 179);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 23);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // GetPolygonBoundaryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 226);
            this.Controls.Add(this.cmbPolylinelayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbPolygonLayer);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GetPolygonBoundaryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取面边界";
            this.Load += new System.EventHandler(this.GetPolygonBoundaryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolylinelayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cmbPolylinelayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPolygonLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}