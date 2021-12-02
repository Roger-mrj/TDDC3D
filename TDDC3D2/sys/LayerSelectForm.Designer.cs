namespace TDDC3D.sys
{
    partial class LayerSelectForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerSelectForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.rdbSelectAll = new System.Windows.Forms.RadioButton();
            this.rdbSelectNull = new System.Windows.Forms.RadioButton();
            this.rdbSelectNotNull = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rdbSelectNotNull);
            this.groupControl1.Controls.Add(this.rdbSelectNull);
            this.groupControl1.Controls.Add(this.rdbSelectAll);
            this.groupControl1.Controls.Add(this.simpleButton2);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 465);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(440, 74);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "加载空图层时范围与关联的空间参考信息会不一致";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(358, 40);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(283, 40);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // treeList1
            // 
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Margin = new System.Windows.Forms.Padding(2);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.OptionsBehavior.ReadOnly = true;
            this.treeList1.OptionsView.ShowCheckBoxes = true;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.Size = new System.Drawing.Size(440, 465);
            this.treeList1.TabIndex = 2;
            this.treeList1.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.treeList1_BeforeCheckNode);
            this.treeList1.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList1_AfterCheckNode);
            this.treeList1.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList1_CustomDrawNodeCell);
            this.treeList1.CustomDrawNodeCheckBox += new DevExpress.XtraTreeList.CustomDrawNodeCheckBoxEventHandler(this.treeList1_CustomDrawNodeCheckBox);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "表名";
            this.treeListColumn1.FieldName = "FCNAME";
            this.treeListColumn1.MinWidth = 32;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 136;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "别名";
            this.treeListColumn2.FieldName = "FCALIAS";
            this.treeListColumn2.MinWidth = 32;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 1;
            this.treeListColumn2.Width = 257;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "Google_Currents_flat_circle_16px_1113541_easyicon.net.png");
            // 
            // rdbSelectAll
            // 
            this.rdbSelectAll.AutoSize = true;
            this.rdbSelectAll.Location = new System.Drawing.Point(5, 45);
            this.rdbSelectAll.Name = "rdbSelectAll";
            this.rdbSelectAll.Size = new System.Drawing.Size(47, 16);
            this.rdbSelectAll.TabIndex = 3;
            this.rdbSelectAll.Text = "全选";
            this.rdbSelectAll.UseVisualStyleBackColor = true;
            this.rdbSelectAll.CheckedChanged += new System.EventHandler(this.rdbSelectAll_CheckedChanged);
            // 
            // rdbSelectNull
            // 
            this.rdbSelectNull.AutoSize = true;
            this.rdbSelectNull.Location = new System.Drawing.Point(58, 45);
            this.rdbSelectNull.Name = "rdbSelectNull";
            this.rdbSelectNull.Size = new System.Drawing.Size(59, 16);
            this.rdbSelectNull.TabIndex = 4;
            this.rdbSelectNull.Text = "全不选";
            this.rdbSelectNull.UseVisualStyleBackColor = true;
            this.rdbSelectNull.CheckedChanged += new System.EventHandler(this.rdbSelectNull_CheckedChanged);
            // 
            // rdbSelectNotNull
            // 
            this.rdbSelectNotNull.AutoSize = true;
            this.rdbSelectNotNull.Location = new System.Drawing.Point(123, 45);
            this.rdbSelectNotNull.Name = "rdbSelectNotNull";
            this.rdbSelectNotNull.Size = new System.Drawing.Size(71, 16);
            this.rdbSelectNotNull.TabIndex = 5;
            this.rdbSelectNotNull.TabStop = true;
            this.rdbSelectNotNull.Text = "非空图层";
            this.rdbSelectNotNull.UseVisualStyleBackColor = true;
            this.rdbSelectNotNull.CheckedChanged += new System.EventHandler(this.rdbSelectNotNull_CheckedChanged);
            // 
            // LayerSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 539);
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LayerSelectForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择图层";
            this.Load += new System.EventHandler(this.LayerSelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private System.Windows.Forms.RadioButton rdbSelectNotNull;
        private System.Windows.Forms.RadioButton rdbSelectNull;
        private System.Windows.Forms.RadioButton rdbSelectAll;
    }
}