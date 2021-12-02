namespace TDDC3D.sys
{
    partial class InAFeatureClassForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InAFeatureClassForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btSounceFC = new DevExpress.XtraEditors.ButtonEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbLocalFeatureclasses = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.gridSrcFields = new System.Windows.Forms.DataGridView();
            this.ColumnSrcFields = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gridDestFields = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.chkCover = new DevExpress.XtraEditors.CheckEdit();
            this.status = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btSounceFC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSrcFields)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDestFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkCover.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btSounceFC);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(544, 48);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "选择源要素类";
            // 
            // btSounceFC
            // 
            this.btSounceFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btSounceFC.Location = new System.Drawing.Point(2, 22);
            this.btSounceFC.Name = "btSounceFC";
            this.btSounceFC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btSounceFC.Size = new System.Drawing.Size(540, 20);
            this.btSounceFC.TabIndex = 2;
            this.btSounceFC.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btSounceFC_ButtonClick);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.simpleButton1);
            this.groupControl2.Controls.Add(this.cmbLocalFeatureclasses);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 48);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(544, 68);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "目标要素类";
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
            this.cmbLocalFeatureclasses.Size = new System.Drawing.Size(426, 20);
            this.cmbLocalFeatureclasses.TabIndex = 1;
            this.cmbLocalFeatureclasses.SelectedIndexChanged += new System.EventHandler(this.cmbLocalFeatureclasses_SelectedIndexChanged);
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 116);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(544, 404);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.gridSrcFields);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(256, 17);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(285, 384);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "源要素类字段名";
            // 
            // gridSrcFields
            // 
            this.gridSrcFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridSrcFields.BackgroundColor = System.Drawing.Color.White;
            this.gridSrcFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSrcFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSrcFields});
            this.gridSrcFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSrcFields.Location = new System.Drawing.Point(3, 17);
            this.gridSrcFields.Name = "gridSrcFields";
            this.gridSrcFields.RowTemplate.Height = 23;
            this.gridSrcFields.Size = new System.Drawing.Size(279, 364);
            this.gridSrcFields.TabIndex = 1;
            this.gridSrcFields.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridSrcFields_Scroll);
            // 
            // ColumnSrcFields
            // 
            this.ColumnSrcFields.HeaderText = "字段名";
            this.ColumnSrcFields.Name = "ColumnSrcFields";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.gridDestFields);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(3, 17);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(253, 384);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "目标要素类字段名";
            // 
            // gridDestFields
            // 
            this.gridDestFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridDestFields.BackgroundColor = System.Drawing.Color.White;
            this.gridDestFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDestFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.gridDestFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDestFields.Location = new System.Drawing.Point(3, 17);
            this.gridDestFields.Name = "gridDestFields";
            this.gridDestFields.RowTemplate.Height = 23;
            this.gridDestFields.Size = new System.Drawing.Size(247, 364);
            this.gridDestFields.TabIndex = 0;
            this.gridDestFields.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridDestFields_Scroll);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "字段名";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "字段类型";
            this.Column2.Name = "Column2";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.chkCover);
            this.panelControl1.Controls.Add(this.status);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.cmdCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 520);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(544, 74);
            this.panelControl1.TabIndex = 4;
            // 
            // chkCover
            // 
            this.chkCover.Location = new System.Drawing.Point(20, 24);
            this.chkCover.Name = "chkCover";
            this.chkCover.Properties.Caption = "清除目标图层已有数据";
            this.chkCover.Size = new System.Drawing.Size(141, 19);
            this.chkCover.TabIndex = 9;
            // 
            // status
            // 
            this.status.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(15, 57);
            this.status.Margin = new System.Windows.Forms.Padding(2);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 17);
            this.status.TabIndex = 7;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(348, 22);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Text = "转换";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.Location = new System.Drawing.Point(429, 22);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(71, 25);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "关闭";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // InAFeatureClassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 594);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InAFeatureClassForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入要素类";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InAFeatureClassForm_FormClosed);
            this.Load += new System.EventHandler(this.InAFeatureClassForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btSounceFC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSrcFields)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDestFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkCover.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLocalFeatureclasses;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView gridSrcFields;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnSrcFields;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView gridDestFields;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.ButtonEdit btSounceFC;
        private DevExpress.XtraEditors.LabelControl status;
        private DevExpress.XtraEditors.CheckEdit chkCover;
    }
}