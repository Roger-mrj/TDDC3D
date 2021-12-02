namespace RCIS.Controls
{
    partial class Polygon2CentroidForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Polygon2CentroidForm));
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.cmbPointLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPolygonLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPointLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(173, 166);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(40, 166);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "开始";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cmbPointLayer
            // 
            this.cmbPointLayer.Location = new System.Drawing.Point(141, 58);
            this.cmbPointLayer.Name = "cmbPointLayer";
            this.cmbPointLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPointLayer.Size = new System.Drawing.Size(143, 20);
            this.cmbPointLayer.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "选择目标点层";
            // 
            // cmbPolygonLayer
            // 
            this.cmbPolygonLayer.Location = new System.Drawing.Point(141, 18);
            this.cmbPolygonLayer.Name = "cmbPolygonLayer";
            this.cmbPolygonLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPolygonLayer.Size = new System.Drawing.Size(143, 20);
            this.cmbPolygonLayer.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "选择要转化的面层";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(25, 98);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "所有要素"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "当前范围"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选中要素")});
            this.radioGroup1.Size = new System.Drawing.Size(266, 35);
            this.radioGroup1.TabIndex = 12;
            // 
            // Polygon2CentroidForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 231);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbPointLayer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbPolygonLayer);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Polygon2CentroidForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取面中心点";
            this.Load += new System.EventHandler(this.Polygon2CentroidForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPointLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPointLayer;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPolygonLayer;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
    }
}