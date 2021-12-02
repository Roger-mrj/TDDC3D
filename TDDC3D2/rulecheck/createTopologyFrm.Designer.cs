namespace TDDC3D.datado
{
    partial class createTopologyFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(createTopologyFrm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmdUnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.cmdSelectAll = new DevExpress.XtraEditors.SimpleButton();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCluster = new DevExpress.XtraEditors.TextEdit();
            this.txtTopoName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.cmdRemoveAll = new DevExpress.XtraEditors.SimpleButton();
            this.cmdAddRule = new DevExpress.XtraEditors.SimpleButton();
            this.cmdRemove = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.cmdCancel1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdOK = new DevExpress.XtraEditors.SimpleButton();
            this.cboFC2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.cboRule = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cboFC1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cmdCreate = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCluster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTopoName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboFC2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboRule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFC1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.listView1);
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(400, 560);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "图层选择";
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(2, 26);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(396, 469);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "图层名称";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 157;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "图层类型";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 82;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "英文表名";
            this.columnHeader4.Width = 104;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmdUnSelect);
            this.panelControl1.Controls.Add(this.cmdSelectAll);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 495);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(396, 63);
            this.panelControl1.TabIndex = 0;
            // 
            // cmdUnSelect
            // 
            this.cmdUnSelect.Location = new System.Drawing.Point(192, 17);
            this.cmdUnSelect.Name = "cmdUnSelect";
            this.cmdUnSelect.Size = new System.Drawing.Size(97, 33);
            this.cmdUnSelect.TabIndex = 1;
            this.cmdUnSelect.Text = "反选";
            this.cmdUnSelect.Click += new System.EventHandler(this.cmdUnSelect_Click);
            // 
            // cmdSelectAll
            // 
            this.cmdSelectAll.Location = new System.Drawing.Point(52, 17);
            this.cmdSelectAll.Name = "cmdSelectAll";
            this.cmdSelectAll.Size = new System.Drawing.Size(97, 33);
            this.cmdSelectAll.TabIndex = 0;
            this.cmdSelectAll.Text = "全选";
            this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click);
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(400, 0);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 560);
            this.splitterControl1.TabIndex = 1;
            this.splitterControl1.TabStop = false;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(405, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(463, 560);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.label7);
            this.xtraTabPage1.Controls.Add(this.txtCluster);
            this.xtraTabPage1.Controls.Add(this.txtTopoName);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.pictureBox1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(457, 527);
            this.xtraTabPage1.Text = "拓扑基本设置";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(233, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 298);
            this.label7.TabIndex = 8;
            this.label7.Text = "建立拓扑流程：\r\n1，填写拓扑名称和容差值；\r\n2，添加拓扑规则；\r\n3，对拓扑规则进行设置；\r\n4，点击建立拓扑即可";
            // 
            // txtCluster
            // 
            this.txtCluster.Location = new System.Drawing.Point(145, 405);
            this.txtCluster.Name = "txtCluster";
            this.txtCluster.Size = new System.Drawing.Size(240, 24);
            this.txtCluster.TabIndex = 7;
            this.txtCluster.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCluster_KeyPress);
            // 
            // txtTopoName
            // 
            this.txtTopoName.EditValue = "TP_1";
            this.txtTopoName.Location = new System.Drawing.Point(145, 360);
            this.txtTopoName.Name = "txtTopoName";
            this.txtTopoName.Size = new System.Drawing.Size(240, 24);
            this.txtTopoName.TabIndex = 6;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(39, 408);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(45, 18);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "容差值";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(40, 360);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "拓扑名称";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(39, 25);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(183, 298);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.listView2);
            this.xtraTabPage2.Controls.Add(this.panelControl3);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(456, 527);
            this.xtraTabPage2.Text = "拓扑规则";
            // 
            // listView2
            // 
            this.listView2.CheckBoxes = true;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Margin = new System.Windows.Forms.Padding(4);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(456, 411);
            this.listView2.TabIndex = 1;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 30;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "要素类";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "拓扑规则";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 150;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "要素类";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.cmdRemoveAll);
            this.panelControl3.Controls.Add(this.cmdAddRule);
            this.panelControl3.Controls.Add(this.cmdRemove);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 411);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(456, 116);
            this.panelControl3.TabIndex = 6;
            // 
            // cmdRemoveAll
            // 
            this.cmdRemoveAll.Location = new System.Drawing.Point(174, 13);
            this.cmdRemoveAll.Name = "cmdRemoveAll";
            this.cmdRemoveAll.Size = new System.Drawing.Size(97, 33);
            this.cmdRemoveAll.TabIndex = 3;
            this.cmdRemoveAll.Text = "移除全部";
            this.cmdRemoveAll.Click += new System.EventHandler(this.cmdRemoveAll_Click);
            // 
            // cmdAddRule
            // 
            this.cmdAddRule.Location = new System.Drawing.Point(304, 13);
            this.cmdAddRule.Name = "cmdAddRule";
            this.cmdAddRule.Size = new System.Drawing.Size(97, 33);
            this.cmdAddRule.TabIndex = 4;
            this.cmdAddRule.Text = "添加规则";
            this.cmdAddRule.Click += new System.EventHandler(this.cmdAddRule_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(43, 13);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(97, 33);
            this.cmdRemove.TabIndex = 2;
            this.cmdRemove.Text = "移除";
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.cmdCancel1);
            this.xtraTabPage3.Controls.Add(this.cmdOK);
            this.xtraTabPage3.Controls.Add(this.cboFC2);
            this.xtraTabPage3.Controls.Add(this.labelControl5);
            this.xtraTabPage3.Controls.Add(this.cboRule);
            this.xtraTabPage3.Controls.Add(this.labelControl4);
            this.xtraTabPage3.Controls.Add(this.cboFC1);
            this.xtraTabPage3.Controls.Add(this.labelControl3);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(456, 527);
            this.xtraTabPage3.Text = "拓扑规则设置";
            // 
            // cmdCancel1
            // 
            this.cmdCancel1.Location = new System.Drawing.Point(276, 358);
            this.cmdCancel1.Name = "cmdCancel1";
            this.cmdCancel1.Size = new System.Drawing.Size(97, 33);
            this.cmdCancel1.TabIndex = 12;
            this.cmdCancel1.Text = "返回";
            this.cmdCancel1.Click += new System.EventHandler(this.cmdCancel1_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(58, 358);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(97, 33);
            this.cmdOK.TabIndex = 11;
            this.cmdOK.Text = "确定";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cboFC2
            // 
            this.cboFC2.Location = new System.Drawing.Point(58, 300);
            this.cboFC2.Name = "cboFC2";
            this.cboFC2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFC2.Size = new System.Drawing.Size(315, 24);
            this.cboFC2.TabIndex = 10;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(58, 257);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(75, 18);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "目标要素类";
            // 
            // cboRule
            // 
            this.cboRule.Location = new System.Drawing.Point(55, 192);
            this.cboRule.Name = "cboRule";
            this.cboRule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboRule.Size = new System.Drawing.Size(315, 24);
            this.cboRule.TabIndex = 8;
            this.cboRule.SelectedIndexChanged += new System.EventHandler(this.cboRule_SelectedIndexChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(55, 149);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 18);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "拓扑规则";
            // 
            // cboFC1
            // 
            this.cboFC1.Location = new System.Drawing.Point(55, 91);
            this.cboFC1.Name = "cboFC1";
            this.cboFC1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboFC1.Size = new System.Drawing.Size(315, 24);
            this.cboFC1.TabIndex = 6;
            this.cboFC1.SelectedIndexChanged += new System.EventHandler(this.cboFC1_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(58, 50);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 18);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "源要素类";
            // 
            // cmdCreate
            // 
            this.cmdCreate.Location = new System.Drawing.Point(59, 18);
            this.cmdCreate.Name = "cmdCreate";
            this.cmdCreate.Size = new System.Drawing.Size(97, 33);
            this.cmdCreate.TabIndex = 8;
            this.cmdCreate.Text = "建立拓扑";
            this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(289, 18);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(97, 33);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "关闭";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Controls.Add(this.cmdCreate);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(405, 497);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(463, 63);
            this.panelControl2.TabIndex = 10;
            // 
            // createTopologyFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 560);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.groupControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "createTopologyFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "创建拓扑";
            this.Load += new System.EventHandler(this.createTopologyFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCluster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTopoName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.xtraTabPage3.ResumeLayout(false);
            this.xtraTabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboFC2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboRule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboFC1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton cmdUnSelect;
        private DevExpress.XtraEditors.SimpleButton cmdSelectAll;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraEditors.TextEdit txtCluster;
        private DevExpress.XtraEditors.TextEdit txtTopoName;
        private DevExpress.XtraEditors.SimpleButton cmdAddRule;
        private DevExpress.XtraEditors.SimpleButton cmdRemoveAll;
        private DevExpress.XtraEditors.SimpleButton cmdRemove;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private DevExpress.XtraEditors.SimpleButton cmdCreate;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cboFC2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit cboRule;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cboFC1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private DevExpress.XtraEditors.SimpleButton cmdCancel1;
        private DevExpress.XtraEditors.SimpleButton cmdOK;
        private DevExpress.XtraEditors.PanelControl panelControl3;
    }
}