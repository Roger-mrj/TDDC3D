namespace TDDC3D.gengxin
{
    partial class FrmGetChangeTB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetChangeTB));
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.cmbXq = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbXq.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // info
            // 
            this.info.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.info.Location = new System.Drawing.Point(0, 142);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(405, 243);
            this.info.TabIndex = 33;
            this.info.UseOptimizedRendering = true;
            // 
            // cmbXq
            // 
            this.cmbXq.Location = new System.Drawing.Point(13, 45);
            this.cmbXq.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbXq.Name = "cmbXq";
            this.cmbXq.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbXq.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbXq.Size = new System.Drawing.Size(379, 24);
            this.cmbXq.TabIndex = 32;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(14, 18);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(135, 18);
            this.labelControl3.TabIndex = 31;
            this.labelControl3.Text = "选择变化行政区图层";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(224, 88);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(80, 31);
            this.simpleButton1.TabIndex = 29;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(312, 88);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(80, 31);
            this.simpleButton2.TabIndex = 30;
            this.simpleButton2.Text = "取消";
            // 
            // FrmGetChangeTB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 385);
            this.Controls.Add(this.info);
            this.Controls.Add(this.cmbXq);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetChangeTB";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取变化图斑";
            this.Load += new System.EventHandler(this.FrmGetChangeTB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbXq.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.ComboBoxEdit cmbXq;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}