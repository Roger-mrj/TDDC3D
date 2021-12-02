namespace RCIS.Style.StyleEditor
{
    partial class SymbolControlForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolControlForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.axSymbologyControl1 = new ESRI.ArcGIS.Controls.AxSymbologyControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.cmdMore = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.cmdOK = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.radioGroup2 = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axSymbologyControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(821, 578);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.axSymbologyControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(558, 574);
            this.panelControl3.TabIndex = 1;
            // 
            // axSymbologyControl1
            // 
            this.axSymbologyControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axSymbologyControl1.Location = new System.Drawing.Point(2, 2);
            this.axSymbologyControl1.Name = "axSymbologyControl1";
            this.axSymbologyControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSymbologyControl1.OcxState")));
            this.axSymbologyControl1.Size = new System.Drawing.Size(554, 570);
            this.axSymbologyControl1.TabIndex = 0;
            this.axSymbologyControl1.OnItemSelected += new ESRI.ArcGIS.Controls.ISymbologyControlEvents_Ax_OnItemSelectedEventHandler(this.axSymbologyControl1_OnItemSelected);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panelControl5);
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl2.Location = new System.Drawing.Point(560, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(259, 574);
            this.panelControl2.TabIndex = 0;
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.xtraTabControl1);
            this.panelControl5.Controls.Add(this.cmdMore);
            this.panelControl5.Controls.Add(this.cmdCancel);
            this.panelControl5.Controls.Add(this.cmdOK);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(2, 250);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(255, 322);
            this.panelControl5.TabIndex = 1;
            // 
            // cmdMore
            // 
            this.cmdMore.Location = new System.Drawing.Point(45, 253);
            this.cmdMore.Name = "cmdMore";
            this.cmdMore.Size = new System.Drawing.Size(173, 23);
            this.cmdMore.TabIndex = 2;
            this.cmdMore.Text = "更多符号";
            this.cmdMore.Click += new System.EventHandler(this.cmdMore_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(143, 282);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "返回";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(45, 282);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "确定";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.pictureEdit1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(2, 2);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(255, 248);
            this.panelControl4.TabIndex = 0;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.Location = new System.Drawing.Point(2, 2);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(251, 244);
            this.pictureEdit1.TabIndex = 0;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup1.Location = new System.Drawing.Point(0, 0);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "符号显示样式[图标]"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "符号显示样式[小图标]"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "符号显示样式[报表]"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "符号显示样式[列表]")});
            this.radioGroup1.Size = new System.Drawing.Size(242, 213);
            this.radioGroup1.TabIndex = 3;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.Size = new System.Drawing.Size(251, 245);
            this.xtraTabControl1.TabIndex = 4;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2,
            this.xtraTabPage3});
            this.xtraTabControl1.Text = "xtraTabControl1";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.radioGroup2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(242, 213);
            this.xtraTabPage2.Text = "符号类型选择";
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.radioGroup1);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(242, 213);
            this.xtraTabPage3.Text = "符号列表样式";
            // 
            // radioGroup2
            // 
            this.radioGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup2.Location = new System.Drawing.Point(0, 0);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "点符号[Marker Style]"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "线符号[Line Style]"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "面符号[Fill Style]"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "文字符号[Text Style]")});
            this.radioGroup2.Size = new System.Drawing.Size(242, 213);
            this.radioGroup2.TabIndex = 0;
            this.radioGroup2.SelectedIndexChanged += new System.EventHandler(this.radioGroup2_SelectedIndexChanged);
            // 
            // SymbolControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 578);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SymbolControlForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "符号管理器";
            this.Load += new System.EventHandler(this.SymbolControlForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axSymbologyControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private ESRI.ArcGIS.Controls.AxSymbologyControl axSymbologyControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton cmdMore;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraEditors.SimpleButton cmdOK;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.RadioGroup radioGroup2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
    }
}