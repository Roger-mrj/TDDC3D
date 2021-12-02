namespace TDDC3D.sys
{
    partial class FrmCoordinateSystem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCoordinateSystem));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.坐标名称 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.searchControl1 = new DevExpress.XtraEditors.SearchControl();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "过滤";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(13, 281);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "当前坐标系：";
            // 
            // memoEdit1
            // 
            this.memoEdit1.Enabled = false;
            this.memoEdit1.Location = new System.Drawing.Point(12, 301);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.AllowFocused = false;
            this.memoEdit1.Size = new System.Drawing.Size(452, 142);
            this.memoEdit1.TabIndex = 2;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // simpleButton1
            // 
            this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(3, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "确定";
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(84, 11);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 4;
            this.simpleButton2.Text = "取消";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Location = new System.Drawing.Point(302, 449);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 42);
            this.panel1.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(13, 38);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 14);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "世界坐标参考系：";
            // 
            // treeList
            // 
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.坐标名称,
            this.treeListColumn1});
            this.treeList.Location = new System.Drawing.Point(12, 58);
            this.treeList.Name = "treeList";
            this.treeList.OptionsBehavior.Editable = false;
            this.treeList.OptionsView.ShowIndicator = false;
            this.treeList.SelectImageList = this.imageCollection1;
            this.treeList.Size = new System.Drawing.Size(452, 217);
            this.treeList.TabIndex = 9;
            this.treeList.GetSelectImage += new DevExpress.XtraTreeList.GetSelectImageEventHandler(this.treeList_GetSelectImage);
            this.treeList.Click += new System.EventHandler(this.treeList_Click);
            this.treeList.DoubleClick += new System.EventHandler(this.treeList_DoubleClick);
            // 
            // 坐标名称
            // 
            this.坐标名称.Caption = "坐标名称";
            this.坐标名称.FieldName = "coordinateName";
            this.坐标名称.MinWidth = 33;
            this.坐标名称.Name = "坐标名称";
            this.坐标名称.Visible = true;
            this.坐标名称.VisibleIndex = 0;
            this.坐标名称.Width = 337;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "管理机构标识符";
            this.treeListColumn1.FieldName = "WKID";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 1;
            this.treeListColumn1.Width = 141;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "文件夹.png");
            this.imageCollection1.Images.SetKeyName(1, "地球.png");
            // 
            // searchControl1
            // 
            this.searchControl1.EditValue = "";
            this.searchControl1.Location = new System.Drawing.Point(42, 12);
            this.searchControl1.Name = "searchControl1";
            this.searchControl1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.searchControl1.Size = new System.Drawing.Size(422, 20);
            this.searchControl1.TabIndex = 10;
            // 
            // FrmCoordinateSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 494);
            this.Controls.Add(this.treeList);
            this.Controls.Add(this.searchControl1);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmCoordinateSystem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坐标参考系选择器";
            this.Load += new System.EventHandler(this.FrmCoordinateSystem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchControl1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn 坐标名称;
        private DevExpress.XtraEditors.SearchControl searchControl1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
    }
}