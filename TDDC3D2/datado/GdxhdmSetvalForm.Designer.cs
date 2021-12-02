namespace TDDC3D.datado
{
    partial class GdxhdmSetvalForm
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
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(244, 165);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(84, 29);
            this.simpleButton3.TabIndex = 22;
            this.simpleButton3.Text = "关闭";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(143, 165);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(84, 29);
            this.simpleButton2.TabIndex = 21;
            this.simpleButton2.Text = "赋值";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cmbLayer
            // 
            this.cmbLayer.Location = new System.Drawing.Point(40, 128);
            this.cmbLayer.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Size = new System.Drawing.Size(315, 20);
            this.cmbLayer.TabIndex = 19;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(40, 99);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 14);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "选择要处理的图层";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(40, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(304, 70);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "    如果耕地周围全是河流水面，赋值为河道耕地，\r\n    如果全是湖泊水面，赋值为湖区耕地；\r\n    如果全是林地，赋值为林区耕地；\r\n    如果全是天然" +
                "牧草地和人工牧草地，赋值为牧区耕地；\r\n    如果全是沙地，赋值为沙荒耕地";
            // 
            // GdxhdmSetvalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 218);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "GdxhdmSetvalForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "耕地细化代码赋值";
            this.Load += new System.EventHandler(this.GdxhdmSetvalForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}