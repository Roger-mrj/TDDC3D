namespace TDDC3D.edit
{
    partial class Xzdw2WhereOptForm
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLayers = new DevExpress.XtraEditors.ComboBoxEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.chkDelXzdw = new DevExpress.XtraEditors.CheckEdit();
            this.chkDoTopInter = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoUnion = new DevExpress.XtraEditors.CheckEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtExtendLen = new DevExpress.XtraEditors.TextEdit();
            this.chkAutoExtSnap = new DevExpress.XtraEditors.CheckEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDelXzdw.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDoTopInter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoUnion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtendLen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoExtSnap.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(30, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(150, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "线状地物生成目标图层";
            // 
            // cmbLayers
            // 
            this.cmbLayers.Location = new System.Drawing.Point(30, 68);
            this.cmbLayers.Name = "cmbLayers";
            this.cmbLayers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayers.Size = new System.Drawing.Size(379, 24);
            this.cmbLayers.TabIndex = 1;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(109, 324);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(255, 324);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "取消";
            // 
            // chkDelXzdw
            // 
            this.chkDelXzdw.Location = new System.Drawing.Point(37, 106);
            this.chkDelXzdw.Name = "chkDelXzdw";
            this.chkDelXzdw.Properties.Caption = "是否同时删除线装状地物";
            this.chkDelXzdw.Size = new System.Drawing.Size(194, 22);
            this.chkDelXzdw.TabIndex = 4;
            // 
            // chkDoTopInter
            // 
            this.chkDoTopInter.EditValue = true;
            this.chkDoTopInter.Location = new System.Drawing.Point(37, 176);
            this.chkDoTopInter.Name = "chkDoTopInter";
            this.chkDoTopInter.Properties.Caption = "自动处理图形压盖";
            this.chkDoTopInter.Size = new System.Drawing.Size(372, 22);
            this.chkDoTopInter.TabIndex = 5;
            // 
            // chkAutoUnion
            // 
            this.chkAutoUnion.EditValue = true;
            this.chkAutoUnion.Location = new System.Drawing.Point(37, 142);
            this.chkAutoUnion.Name = "chkAutoUnion";
            this.chkAutoUnion.Properties.Caption = "自动合并（相同地类，权属，座落，权属性质，KD）";
            this.chkAutoUnion.Size = new System.Drawing.Size(406, 22);
            this.chkAutoUnion.TabIndex = 7;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(493, 437);
            this.xtraTabControl1.TabIndex = 8;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.txtExtendLen);
            this.xtraTabPage1.Controls.Add(this.chkAutoExtSnap);
            this.xtraTabPage1.Controls.Add(this.chkDoTopInter);
            this.xtraTabPage1.Controls.Add(this.chkAutoUnion);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.cmbLayers);
            this.xtraTabPage1.Controls.Add(this.chkDelXzdw);
            this.xtraTabPage1.Controls.Add(this.simpleButton1);
            this.xtraTabPage1.Controls.Add(this.simpleButton2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(487, 404);
            this.xtraTabPage1.Text = "选项";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(315, 216);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(15, 18);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "米";
            // 
            // txtExtendLen
            // 
            this.txtExtendLen.EditValue = "15";
            this.txtExtendLen.Location = new System.Drawing.Point(238, 214);
            this.txtExtendLen.Name = "txtExtendLen";
            this.txtExtendLen.Size = new System.Drawing.Size(65, 24);
            this.txtExtendLen.TabIndex = 9;
            // 
            // chkAutoExtSnap
            // 
            this.chkAutoExtSnap.EditValue = true;
            this.chkAutoExtSnap.Location = new System.Drawing.Point(37, 215);
            this.chkAutoExtSnap.Name = "chkAutoExtSnap";
            this.chkAutoExtSnap.Properties.Caption = "延伸捕捉地类图斑 容差";
            this.chkAutoExtSnap.Size = new System.Drawing.Size(372, 22);
            this.chkAutoExtSnap.TabIndex = 8;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gridControl1);
            this.xtraTabPage2.Controls.Add(this.groupControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(487, 404);
            this.xtraTabPage2.Text = "地类压盖优先级";
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(376, 404);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "地类编码";
            this.gridColumn1.FieldName = "DLBM";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "地类名称";
            this.gridColumn2.FieldName = "DLMC";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "优先级";
            this.gridColumn3.FieldName = "SORT";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton4);
            this.groupControl1.Controls.Add(this.simpleButton3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupControl1.Location = new System.Drawing.Point(376, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(111, 404);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "工具";
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(6, 81);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(86, 33);
            this.simpleButton4.TabIndex = 1;
            this.simpleButton4.Text = "下移";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(6, 35);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(86, 33);
            this.simpleButton3.TabIndex = 0;
            this.simpleButton3.Text = "上移";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // Xzdw2WhereOptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 437);
            this.Controls.Add(this.xtraTabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Xzdw2WhereOptForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "目标图层";
            this.Load += new System.EventHandler(this.Xzdw2WhereOptForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayers.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDelXzdw.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDoTopInter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoUnion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExtendLen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoExtSnap.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayers;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.CheckEdit chkDelXzdw;
        private DevExpress.XtraEditors.CheckEdit chkDoTopInter;
        private DevExpress.XtraEditors.CheckEdit chkAutoUnion;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtExtendLen;
        private DevExpress.XtraEditors.CheckEdit chkAutoExtSnap;
    }
}