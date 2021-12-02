namespace RCIS.MapTool
{
    partial class DeltaXYForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeltaXYForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.spinEditX = new DevExpress.XtraEditors.SpinEdit();
            this.spinEditY = new DevExpress.XtraEditors.SpinEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditY.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 14);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(43, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "X偏移量";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(11, 46);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(44, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Y偏移量";
            // 
            // spinEditX
            // 
            this.spinEditX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditX.Location = new System.Drawing.Point(70, 11);
            this.spinEditX.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.spinEditX.Name = "spinEditX";
            this.spinEditX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditX.Size = new System.Drawing.Size(117, 20);
            this.spinEditX.TabIndex = 2;
            // 
            // spinEditY
            // 
            this.spinEditY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEditY.Location = new System.Drawing.Point(70, 46);
            this.spinEditY.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.spinEditY.Name = "spinEditY";
            this.spinEditY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEditY.Size = new System.Drawing.Size(117, 20);
            this.spinEditY.TabIndex = 3;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(205, 10);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 25);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "预览";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(205, 45);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 25);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "确定";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // DeltaXYForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 93);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.spinEditY);
            this.Controls.Add(this.spinEditX);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeltaXYForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "增量XY";
            this.Load += new System.EventHandler(this.DeltaXYForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.spinEditX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEditY.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit spinEditX;
        private DevExpress.XtraEditors.SpinEdit spinEditY;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}