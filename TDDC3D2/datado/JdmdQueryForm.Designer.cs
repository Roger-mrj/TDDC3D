namespace TDDC3D.datado
{
    partial class JdmdQueryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JdmdQueryForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtMaxPjmd = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtMinPjmd = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayer1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lstProperties = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.lstIds = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxPjmd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinPjmd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstIds)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton3);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.simpleButton2);
            this.groupControl1.Controls.Add(this.btnQuery);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.txtMaxPjmd);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtMinPjmd);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.cmbLayer1);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(699, 105);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "选项";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(306, 62);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(88, 23);
            this.simpleButton3.TabIndex = 33;
            this.simpleButton3.Text = "抽稀选中";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(400, 32);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(146, 23);
            this.simpleButton1.TabIndex = 32;
            this.simpleButton1.Text = "提取相关要素到shp";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(400, 62);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(146, 23);
            this.simpleButton2.TabIndex = 31;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(306, 32);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(88, 23);
            this.btnQuery.TabIndex = 30;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(250, 68);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(12, 14);
            this.labelControl4.TabIndex = 26;
            this.labelControl4.Text = "米";
            // 
            // txtMaxPjmd
            // 
            this.txtMaxPjmd.EditValue = "50";
            this.txtMaxPjmd.Location = new System.Drawing.Point(210, 65);
            this.txtMaxPjmd.Name = "txtMaxPjmd";
            this.txtMaxPjmd.Size = new System.Drawing.Size(35, 20);
            this.txtMaxPjmd.TabIndex = 25;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(132, 68);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(68, 14);
            this.labelControl3.TabIndex = 24;
            this.labelControl3.Text = "米  或者大于";
            // 
            // txtMinPjmd
            // 
            this.txtMinPjmd.EditValue = "1";
            this.txtMinPjmd.Location = new System.Drawing.Point(92, 65);
            this.txtMinPjmd.Name = "txtMinPjmd";
            this.txtMinPjmd.Size = new System.Drawing.Size(35, 20);
            this.txtMinPjmd.TabIndex = 23;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(11, 68);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 22;
            this.labelControl2.Text = "平均密度小于";
            // 
            // cmbLayer1
            // 
            this.cmbLayer1.Location = new System.Drawing.Point(71, 33);
            this.cmbLayer1.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLayer1.Name = "cmbLayer1";
            this.cmbLayer1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer1.Size = new System.Drawing.Size(174, 20);
            this.cmbLayer1.TabIndex = 21;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 34);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 20;
            this.labelControl1.Text = "目标图层";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lstProperties);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 105);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(699, 391);
            this.panelControl1.TabIndex = 1;
            // 
            // lstProperties
            // 
            this.lstProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProperties.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstProperties.FullRowSelect = true;
            this.lstProperties.GridLines = true;
            this.lstProperties.Location = new System.Drawing.Point(167, 2);
            this.lstProperties.Name = "lstProperties";
            this.lstProperties.Size = new System.Drawing.Size(530, 387);
            this.lstProperties.TabIndex = 3;
            this.lstProperties.UseCompatibleStateImageBehavior = false;
            this.lstProperties.View = System.Windows.Forms.View.Details;
            this.lstProperties.DoubleClick += new System.EventHandler(this.lstProperties_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "X";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Y";
            this.columnHeader3.Width = 100;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.lstIds);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl2.Location = new System.Drawing.Point(2, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(165, 387);
            this.groupControl2.TabIndex = 0;
            this.groupControl2.Text = "要素ID";
            // 
            // lstIds
            // 
            this.lstIds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstIds.Location = new System.Drawing.Point(2, 22);
            this.lstIds.Name = "lstIds";
            this.lstIds.Size = new System.Drawing.Size(161, 363);
            this.lstIds.TabIndex = 0;
            this.lstIds.DoubleClick += new System.EventHandler(this.lstIds_DoubleClick);
            // 
            // JdmdQueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 496);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl1);
            this.Name = "JdmdQueryForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图层点距检查";
            this.Load += new System.EventHandler(this.JdmdQueryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxPjmd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMinPjmd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstIds)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnQuery;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtMaxPjmd;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtMinPjmd;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.ListBoxControl lstIds;
        private System.Windows.Forms.ListView lstProperties;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
    }
}