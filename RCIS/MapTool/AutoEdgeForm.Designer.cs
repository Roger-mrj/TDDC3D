namespace RCIS.MapTool
{
    partial class AutoEdgeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoEdgeForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("所有结点");
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbDltbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbmasterXzq = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbSecondXZQ = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDltbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbmasterXzq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecondXZQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 16);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择DLTB层";
            // 
            // cmbDltbLayer
            // 
            this.cmbDltbLayer.Location = new System.Drawing.Point(97, 14);
            this.cmbDltbLayer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbDltbLayer.Name = "cmbDltbLayer";
            this.cmbDltbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDltbLayer.Size = new System.Drawing.Size(174, 20);
            this.cmbDltbLayer.TabIndex = 3;
            // 
            // cmbmasterXzq
            // 
            this.cmbmasterXzq.Location = new System.Drawing.Point(376, 14);
            this.cmbmasterXzq.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbmasterXzq.Name = "cmbmasterXzq";
            this.cmbmasterXzq.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbmasterXzq.Size = new System.Drawing.Size(174, 20);
            this.cmbmasterXzq.TabIndex = 5;
            this.cmbmasterXzq.SelectedIndexChanged += new System.EventHandler(this.cmbmasterXzq_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(295, 15);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(68, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "选择主边界A";
            this.labelControl2.Click += new System.EventHandler(this.labelControl2_Click);
            // 
            // cmbSecondXZQ
            // 
            this.cmbSecondXZQ.Location = new System.Drawing.Point(376, 42);
            this.cmbSecondXZQ.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbSecondXZQ.Name = "cmbSecondXZQ";
            this.cmbSecondXZQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbSecondXZQ.Size = new System.Drawing.Size(174, 20);
            this.cmbSecondXZQ.TabIndex = 7;
            this.cmbSecondXZQ.SelectedIndexChanged += new System.EventHandler(this.cmbSecondXZQ_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(295, 43);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(67, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "选择副边界B";
            this.labelControl3.Click += new System.EventHandler(this.labelControl3_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(359, 387);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(64, 26);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "处理";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(459, 387);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(64, 26);
            this.simpleButton2.TabIndex = 9;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(9, 122);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "节点0";
            treeNode1.Text = "所有结点";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.Size = new System.Drawing.Size(542, 251);
            this.treeView1.TabIndex = 10;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(16, 42);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "距离容差";
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(97, 38);
            this.txtDistance.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(131, 21);
            this.txtDistance.TabIndex = 12;
            this.txtDistance.Text = "2";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(244, 41);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 14);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "毫米";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton3.Image")));
            this.simpleButton3.Location = new System.Drawing.Point(244, 387);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(91, 26);
            this.simpleButton3.TabIndex = 14;
            this.simpleButton3.Text = "查找相近点";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(16, 75);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 15;
            this.labelControl6.Text = "距离容差";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(98, 71);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "取A边界"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "取B边界"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "两边各取一半")});
            this.radioGroup1.Size = new System.Drawing.Size(398, 25);
            this.radioGroup1.TabIndex = 16;
            // 
            // AutoEdgeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 431);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.txtDistance);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cmbSecondXZQ);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.cmbmasterXzq);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbDltbLayer);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.treeView1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoEdgeForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动接边";
            this.Load += new System.EventHandler(this.AutoEdgeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbDltbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbmasterXzq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSecondXZQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbDltbLayer;
        private DevExpress.XtraEditors.ComboBoxEdit cmbmasterXzq;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbSecondXZQ;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.TreeView treeView1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.TextBox txtDistance;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
    }
}