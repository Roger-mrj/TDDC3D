namespace TDDC3D.gengxin
{
    partial class FrmSpatialUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSpatialUpdate));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cboSource = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rdoDatas = new DevExpress.XtraEditors.RadioGroup();
            this.cboTarget = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.chkReWrite = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.rdoSpatial = new DevExpress.XtraEditors.RadioGroup();
            this.status = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.gridSrcFields = new System.Windows.Forms.DataGridView();
            this.ColumnSrcFields = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gridDestFields = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboSource.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoDatas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTarget.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkReWrite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoSpatial.Properties)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSrcFields)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDestFields)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupControl1);
            this.panel1.Controls.Add(this.groupControl2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(719, 99);
            this.panel1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cboSource);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(341, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(378, 99);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "源要素类";
            // 
            // cboSource
            // 
            this.cboSource.Location = new System.Drawing.Point(17, 39);
            this.cboSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboSource.Name = "cboSource";
            this.cboSource.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboSource.Size = new System.Drawing.Size(349, 24);
            this.cboSource.TabIndex = 1;
            this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboSource_SelectedIndexChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rdoDatas);
            this.groupControl2.Controls.Add(this.cboTarget);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(341, 99);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "目标要素类";
            // 
            // rdoDatas
            // 
            this.rdoDatas.Location = new System.Drawing.Point(20, 68);
            this.rdoDatas.Name = "rdoDatas";
            this.rdoDatas.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoDatas.Properties.Appearance.Options.UseBackColor = true;
            this.rdoDatas.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoDatas.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "选择数据"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "全部数据")});
            this.rdoDatas.Size = new System.Drawing.Size(301, 24);
            this.rdoDatas.TabIndex = 11;
            // 
            // cboTarget
            // 
            this.cboTarget.Location = new System.Drawing.Point(17, 39);
            this.cboTarget.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTarget.Name = "cboTarget";
            this.cboTarget.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboTarget.Size = new System.Drawing.Size(304, 24);
            this.cboTarget.TabIndex = 1;
            this.cboTarget.SelectedIndexChanged += new System.EventHandler(this.cboTarget_SelectedIndexChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl4);
            this.panelControl1.Controls.Add(this.groupControl3);
            this.panelControl1.Controls.Add(this.status);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.cmdCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 577);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(719, 92);
            this.panelControl1.TabIndex = 5;
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.chkReWrite);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl4.Location = new System.Drawing.Point(222, 2);
            this.groupControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(163, 88);
            this.groupControl4.TabIndex = 10;
            this.groupControl4.Text = "属性填写条件";
            // 
            // chkReWrite
            // 
            this.chkReWrite.Location = new System.Drawing.Point(22, 45);
            this.chkReWrite.Name = "chkReWrite";
            this.chkReWrite.Properties.Caption = "覆盖属性";
            this.chkReWrite.Size = new System.Drawing.Size(119, 22);
            this.chkReWrite.TabIndex = 10;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.rdoSpatial);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl3.Location = new System.Drawing.Point(2, 2);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(220, 88);
            this.groupControl3.TabIndex = 8;
            this.groupControl3.Text = "空间关系";
            // 
            // rdoSpatial
            // 
            this.rdoSpatial.Location = new System.Drawing.Point(15, 35);
            this.rdoSpatial.Name = "rdoSpatial";
            this.rdoSpatial.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoSpatial.Properties.Appearance.Options.UseBackColor = true;
            this.rdoSpatial.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoSpatial.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "最大面积"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "中心点")});
            this.rdoSpatial.Size = new System.Drawing.Size(199, 43);
            this.rdoSpatial.TabIndex = 10;
            // 
            // status
            // 
            this.status.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.Location = new System.Drawing.Point(20, 71);
            this.status.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 22);
            this.status.TabIndex = 7;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(456, 37);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(95, 31);
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Text = "更新";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
            this.cmdCancel.Location = new System.Drawing.Point(564, 37);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(95, 31);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "关闭";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 99);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(719, 478);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.gridSrcFields);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(341, 22);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(374, 452);
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
            this.gridSrcFields.Location = new System.Drawing.Point(4, 22);
            this.gridSrcFields.Margin = new System.Windows.Forms.Padding(4);
            this.gridSrcFields.Name = "gridSrcFields";
            this.gridSrcFields.RowHeadersWidth = 25;
            this.gridSrcFields.RowTemplate.Height = 23;
            this.gridSrcFields.Size = new System.Drawing.Size(366, 426);
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
            this.groupBox5.Location = new System.Drawing.Point(4, 22);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(337, 452);
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
            this.gridDestFields.Location = new System.Drawing.Point(4, 22);
            this.gridDestFields.Margin = new System.Windows.Forms.Padding(4);
            this.gridDestFields.Name = "gridDestFields";
            this.gridDestFields.RowHeadersWidth = 25;
            this.gridDestFields.RowTemplate.Height = 23;
            this.gridDestFields.Size = new System.Drawing.Size(329, 426);
            this.gridDestFields.TabIndex = 0;
            this.gridDestFields.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridDestFields_Scroll);
            // 
            // Column1
            // 
            this.Column1.FillWeight = 78.17259F;
            this.Column1.HeaderText = "字段名";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.FillWeight = 121.8274F;
            this.Column2.HeaderText = "字段类型";
            this.Column2.Name = "Column2";
            // 
            // FrmSpatialUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 669);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSpatialUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "空间关系属性更新";
            this.Load += new System.EventHandler(this.FrmSpatialUpdate_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboSource.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rdoDatas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTarget.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkReWrite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rdoSpatial.Properties)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSrcFields)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDestFields)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboSource;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cboTarget;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl status;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView gridSrcFields;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnSrcFields;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView gridDestFields;
        private DevExpress.XtraEditors.RadioGroup rdoDatas;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.CheckEdit chkReWrite;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.RadioGroup rdoSpatial;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}