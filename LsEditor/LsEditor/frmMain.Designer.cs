namespace LsEditor
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.brStatus = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.brCurrFile = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroup1 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarLine = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarPolygon = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarPoint = new DevExpress.XtraNavBar.NavBarItem();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ApplicationButtonText = null;
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.barButtonItem1,
            this.barButtonItem2,
            this.brStatus,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barButtonItem5,
            this.brCurrFile,
            this.barStaticItem1,
            this.barButtonItem6,
            this.barButtonItem7});
            this.ribbon.LargeImages = this.imageCollection1;
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ribbon.MaxItemId = 11;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
            this.ribbon.ShowToolbarCustomizeItem = false;
            this.ribbon.Size = new System.Drawing.Size(1013, 164);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            this.ribbon.Toolbar.ShowCustomizeItem = false;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "打开符号文件";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.LargeImageIndex = 3;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "新建符号文件";
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.LargeImageIndex = 4;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // brStatus
            // 
            this.brStatus.Caption = "状态";
            this.brStatus.Id = 3;
            this.brStatus.Name = "brStatus";
            this.brStatus.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "删除符号";
            this.barButtonItem3.Id = 4;
            this.barButtonItem3.LargeImageIndex = 6;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "编辑符号";
            this.barButtonItem4.Id = 5;
            this.barButtonItem4.LargeImageIndex = 1;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "新增符号";
            this.barButtonItem5.Id = 6;
            this.barButtonItem5.LargeImageIndex = 10;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // brCurrFile
            // 
            this.brCurrFile.Caption = "当前文件";
            this.brCurrFile.Id = 7;
            this.brCurrFile.Name = "brCurrFile";
            this.brCurrFile.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem1.Caption = "北京地星伟业数码科技有限公司";
            this.barStaticItem1.Id = 8;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "退出";
            this.barButtonItem6.Id = 9;
            this.barButtonItem6.LargeImageIndex = 13;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageSize = new System.Drawing.Size(64, 64);
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "package_graphics.ico");
            this.imageCollection1.Images.SetKeyName(1, "kolourpaint.ico");
            this.imageCollection1.Images.SetKeyName(2, "korganizer_todo.ico");
            this.imageCollection1.Images.SetKeyName(3, "kwrite.ico");
            this.imageCollection1.Images.SetKeyName(4, "ooo_gulls.ico");
            this.imageCollection1.Images.SetKeyName(5, "ooo_setup.ico");
            this.imageCollection1.Images.SetKeyName(6, "package.ico");
            this.imageCollection1.Images.SetKeyName(7, "package_editors.ico");
            this.imageCollection1.Images.SetKeyName(8, "package_edutainment.ico");
            this.imageCollection1.Images.SetKeyName(9, "package_system.ico");
            this.imageCollection1.Images.SetKeyName(10, "package_utilities.ico");
            this.imageCollection1.Images.SetKeyName(11, "knewsletter.ico");
            this.imageCollection1.Images.SetKeyName(12, "knotes.ico");
            this.imageCollection1.Images.SetKeyName(13, "Go_back_32px_1186174_easyicon.net.png");
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "主要功能";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem7, true);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "文件";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem5);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.ShowCaptionButton = false;
            this.ribbonPageGroup2.Text = "符号操作";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem6);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.ShowCaptionButton = false;
            this.ribbonPageGroup3.Text = "退出";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.brStatus);
            this.ribbonStatusBar.ItemLinks.Add(this.brCurrFile);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticItem1);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 675);
            this.ribbonStatusBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1013, 33);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.pictureEdit1);
            this.groupControl1.Controls.Add(this.navBarControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 164);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(246, 511);
            this.groupControl1.TabIndex = 8;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureEdit1.Location = new System.Drawing.Point(2, 230);
            this.pictureEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(242, 279);
            this.pictureEdit1.TabIndex = 4;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroup1;
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup1});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarPoint,
            this.navBarLine,
            this.navBarPolygon});
            this.navBarControl1.Location = new System.Drawing.Point(2, 26);
            this.navBarControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 242;
            this.navBarControl1.Size = new System.Drawing.Size(242, 483);
            this.navBarControl1.TabIndex = 3;
            this.navBarControl1.Text = "navBarControl1";
            // 
            // navBarGroup1
            // 
            this.navBarGroup1.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.navBarGroup1.Appearance.Options.UseFont = true;
            this.navBarGroup1.Caption = "符号类型";
            this.navBarGroup1.Expanded = true;
            this.navBarGroup1.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarLine),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarPolygon),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarPoint)});
            this.navBarGroup1.Name = "navBarGroup1";
            // 
            // navBarLine
            // 
            this.navBarLine.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.navBarLine.Appearance.Options.UseFont = true;
            this.navBarLine.AppearancePressed.BackColor = System.Drawing.SystemColors.Window;
            this.navBarLine.AppearancePressed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navBarLine.AppearancePressed.ForeColor = System.Drawing.Color.Red;
            this.navBarLine.AppearancePressed.Options.UseBackColor = true;
            this.navBarLine.AppearancePressed.Options.UseFont = true;
            this.navBarLine.AppearancePressed.Options.UseForeColor = true;
            this.navBarLine.Caption = "线符号";
            this.navBarLine.Name = "navBarLine";
            this.navBarLine.LinkPressed += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarLine_LinkPressed);
            // 
            // navBarPolygon
            // 
            this.navBarPolygon.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.navBarPolygon.Appearance.Options.UseFont = true;
            this.navBarPolygon.AppearancePressed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navBarPolygon.AppearancePressed.ForeColor = System.Drawing.Color.Red;
            this.navBarPolygon.AppearancePressed.Options.UseFont = true;
            this.navBarPolygon.AppearancePressed.Options.UseForeColor = true;
            this.navBarPolygon.Caption = "面符号";
            this.navBarPolygon.Name = "navBarPolygon";
            this.navBarPolygon.LinkPressed += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarPolygon_LinkPressed);
            // 
            // navBarPoint
            // 
            this.navBarPoint.Appearance.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.navBarPoint.Appearance.Options.UseFont = true;
            this.navBarPoint.AppearancePressed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navBarPoint.AppearancePressed.ForeColor = System.Drawing.Color.Red;
            this.navBarPoint.AppearancePressed.Options.UseFont = true;
            this.navBarPoint.AppearancePressed.Options.UseForeColor = true;
            this.navBarPoint.Caption = "点符号";
            this.navBarPoint.Name = "navBarPoint";
            this.navBarPoint.LinkPressed += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.navBarPoint_LinkPressed);
            // 
            // splitterControl1
            // 
            this.splitterControl1.Location = new System.Drawing.Point(246, 164);
            this.splitterControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 511);
            this.splitterControl1.TabIndex = 9;
            this.splitterControl1.TabStop = false;
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(251, 164);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.MenuManager = this.ribbon;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(762, 511);
            this.gridControl1.TabIndex = 10;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "编号";
            this.gridColumn4.FieldName = "ID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "名称";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "种类";
            this.gridColumn2.FieldName = "Category";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "对象";
            this.gridColumn3.FieldName = "Object";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(422, 265);
            this.axLicenseControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 13;
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "ServerStyle转换";
            this.barButtonItem7.Id = 10;
            this.barButtonItem7.LargeImageIndex = 5;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // frmMain
            // 
            this.AutoHideRibbon = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 708);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.splitterControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.Ribbon = this.ribbon;
            this.RibbonVisibility = DevExpress.XtraBars.Ribbon.RibbonVisibility.Visible;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "符号编辑器      ";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup1;
        private DevExpress.XtraNavBar.NavBarItem navBarLine;
        private DevExpress.XtraNavBar.NavBarItem navBarPolygon;
        private DevExpress.XtraNavBar.NavBarItem navBarPoint;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraBars.BarStaticItem brStatus;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraBars.BarStaticItem brCurrFile;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
    }
}