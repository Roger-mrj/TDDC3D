namespace TDDC3D.datado
{
    partial class GddbSetValForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GddbSetValForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cmbField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cmbDLTBLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbGDDBLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDLTBLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGDDBLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.cmbField);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.cmbDLTBLayer);
            this.groupControl1.Controls.Add(this.cmbGDDBLayer);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(955, 88);
            this.groupControl1.TabIndex = 19;
            this.groupControl1.Text = "图层选择";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(747, 45);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(75, 18);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "字段获取值";
            // 
            // cmbField
            // 
            this.cmbField.EditValue = "ZRD|自然等";
            this.cmbField.Location = new System.Drawing.Point(613, 41);
            this.cmbField.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbField.Name = "cmbField";
            this.cmbField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbField.Properties.Items.AddRange(new object[] {
            "ZRD|自然等",
            "LYD|利用等",
            "JJD|经济等"});
            this.cmbField.Size = new System.Drawing.Size(128, 24);
            this.cmbField.TabIndex = 12;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(592, 45);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(15, 18);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "从";
            // 
            // cmbDLTBLayer
            // 
            this.cmbDLTBLayer.Location = new System.Drawing.Point(401, 40);
            this.cmbDLTBLayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbDLTBLayer.Name = "cmbDLTBLayer";
            this.cmbDLTBLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDLTBLayer.Size = new System.Drawing.Size(163, 24);
            this.cmbDLTBLayer.TabIndex = 10;
            // 
            // cmbGDDBLayer
            // 
            this.cmbGDDBLayer.Location = new System.Drawing.Point(111, 41);
            this.cmbGDDBLayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbGDDBLayer.Name = "cmbGDDBLayer";
            this.cmbGDDBLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGDDBLayer.Size = new System.Drawing.Size(156, 24);
            this.cmbGDDBLayer.TabIndex = 10;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 44);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 18);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "耕地等别图层";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(285, 42);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(90, 18);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "地类图斑图层";
            // 
            // memoLog
            // 
            this.memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLog.Location = new System.Drawing.Point(0, 0);
            this.memoLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(955, 586);
            this.memoLog.TabIndex = 21;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(537, 25);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(191, 35);
            this.simpleButton3.TabIndex = 18;
            this.simpleButton3.Text = "从GDDB获取值";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 586);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(955, 74);
            this.panelControl1.TabIndex = 20;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(768, 25);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(124, 35);
            this.simpleButton2.TabIndex = 19;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // GddbSetValForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 660);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.memoLog);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GddbSetValForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "耕地等别赋值";
            this.Load += new System.EventHandler(this.GddbSetValForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDLTBLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGDDBLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cmbField;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDLTBLayer;
        private DevExpress.XtraEditors.ComboBoxEdit cmbGDDBLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}