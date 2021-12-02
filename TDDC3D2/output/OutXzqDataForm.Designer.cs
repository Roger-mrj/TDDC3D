namespace TDDC3D.output
{
    partial class OutXzqDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutXzqDataForm));
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.beDestMdbFile = new DevExpress.XtraEditors.ButtonEdit();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.beShpDestDir = new DevExpress.XtraEditors.ButtonEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beDestMdbFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShpDestDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "别名";
            this.treeListColumn2.FieldName = "FCALIAS";
            this.treeListColumn2.MinWidth = 32;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 1;
            this.treeListColumn2.Width = 163;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "表名";
            this.treeListColumn1.FieldName = "FCNAME";
            this.treeListColumn1.MinWidth = 32;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 225;
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
            this.treeList1.OptionsView.ShowCheckBoxes = true;
            this.treeList1.Size = new System.Drawing.Size(446, 382);
            this.treeList1.TabIndex = 4;
            this.treeList1.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.treeList1_BeforeCheckNode);
            this.treeList1.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList1_AfterCheckNode);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "Google_Currents_flat_circle_16px_1113541_easyicon.net.png");
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(358, 145);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(68, 25);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(232, 145);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 25);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.beDestMdbFile);
            this.groupControl1.Controls.Add(this.lblStatus);
            this.groupControl1.Controls.Add(this.beShpDestDir);
            this.groupControl1.Controls.Add(this.simpleButton2);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.radioGroup1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 382);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(446, 200);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "工具栏";
            // 
            // beDestMdbFile
            // 
            this.beDestMdbFile.Enabled = false;
            this.beDestMdbFile.Location = new System.Drawing.Point(88, 90);
            this.beDestMdbFile.Margin = new System.Windows.Forms.Padding(2);
            this.beDestMdbFile.Name = "beDestMdbFile";
            this.beDestMdbFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestMdbFile.Size = new System.Drawing.Size(324, 20);
            this.beDestMdbFile.TabIndex = 6;
            this.beDestMdbFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestMdbFile_ButtonClick);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(14, 92);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 14);
            this.lblStatus.TabIndex = 4;
            // 
            // beShpDestDir
            // 
            this.beShpDestDir.Location = new System.Drawing.Point(88, 49);
            this.beShpDestDir.Margin = new System.Windows.Forms.Padding(2);
            this.beShpDestDir.Name = "beShpDestDir";
            this.beShpDestDir.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beShpDestDir.Size = new System.Drawing.Size(324, 20);
            this.beShpDestDir.TabIndex = 3;
            this.beShpDestDir.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestDir_ButtonClick);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(13, 31);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Columns = 1;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "导出SHP"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "导出MDB")});
            this.radioGroup1.Size = new System.Drawing.Size(413, 96);
            this.radioGroup1.TabIndex = 5;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // OutXzqDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 582);
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutXzqDataForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出辖区数据";
            this.Load += new System.EventHandler(this.OutXzqDataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beDestMdbFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShpDestDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ButtonEdit beShpDestDir;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.ButtonEdit beDestMdbFile;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
    }
}