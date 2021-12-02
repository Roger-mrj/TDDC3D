namespace TDDC3D.sys
{
    partial class QuickImportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickImportForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btSounceFC = new DevExpress.XtraEditors.ButtonEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lstSrcFeasPath = new DevExpress.XtraEditors.ListBoxControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnRemoveItem = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLocalFeatureclasses = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btSounceFC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstSrcFeasPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btSounceFC);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(488, 48);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "添加源要素类  (该功能将覆盖目标数据库中的要素类)";
            // 
            // btSounceFC
            // 
            this.btSounceFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btSounceFC.Location = new System.Drawing.Point(2, 22);
            this.btSounceFC.Name = "btSounceFC";
            this.btSounceFC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btSounceFC.Size = new System.Drawing.Size(484, 20);
            this.btSounceFC.TabIndex = 2;
            this.btSounceFC.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btSounceFC_ButtonClick);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(313, 14);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "导入";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.Location = new System.Drawing.Point(394, 14);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "关闭";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(2, 37);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(24, 14);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "状态";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lstSrcFeasPath);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 48);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(488, 122);
            this.panelControl1.TabIndex = 10;
            // 
            // lstSrcFeasPath
            // 
            this.lstSrcFeasPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSrcFeasPath.Location = new System.Drawing.Point(2, 2);
            this.lstSrcFeasPath.Margin = new System.Windows.Forms.Padding(2);
            this.lstSrcFeasPath.Name = "lstSrcFeasPath";
            this.lstSrcFeasPath.Size = new System.Drawing.Size(456, 118);
            this.lstSrcFeasPath.TabIndex = 1;
            // 
            // panelControl2
            // 
            this.panelControl2.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.panelControl2.Controls.Add(this.btnRemoveItem);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl2.Location = new System.Drawing.Point(458, 2);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(28, 118);
            this.panelControl2.TabIndex = 0;
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Appearance.Options.UseTextOptions = true;
            this.btnRemoveItem.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.btnRemoveItem.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.btnRemoveItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveItem.Location = new System.Drawing.Point(2, 2);
            this.btnRemoveItem.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(24, 114);
            this.btnRemoveItem.TabIndex = 1;
            this.btnRemoveItem.Text = "移除项";
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.simpleButton1);
            this.groupControl2.Controls.Add(this.cmbLocalFeatureclasses);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 170);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(488, 68);
            this.groupControl2.TabIndex = 11;
            this.groupControl2.Text = "目标数据集";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(62, 34);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(27, 18);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "...";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cmbLocalFeatureclasses
            // 
            this.cmbLocalFeatureclasses.Location = new System.Drawing.Point(110, 33);
            this.cmbLocalFeatureclasses.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLocalFeatureclasses.Name = "cmbLocalFeatureclasses";
            this.cmbLocalFeatureclasses.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLocalFeatureclasses.Size = new System.Drawing.Size(358, 20);
            this.cmbLocalFeatureclasses.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 36);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "标准库";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lblStatus);
            this.panelControl3.Controls.Add(this.simpleButton2);
            this.panelControl3.Controls.Add(this.cmdCancel);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 238);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(488, 53);
            this.panelControl3.TabIndex = 12;
            // 
            // QuickImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 291);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickImportForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "快速导入要素类";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QuickImportForm_FormClosed);
            this.Load += new System.EventHandler(this.QuickImportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btSounceFC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstSrcFeasPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ButtonEdit btSounceFC;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.ListBoxControl lstSrcFeasPath;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnRemoveItem;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLocalFeatureclasses;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
    }
}