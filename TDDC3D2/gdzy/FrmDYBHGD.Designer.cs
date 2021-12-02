namespace TDDC3D.gdzy
{
    partial class FrmDYBHGD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDYBHGD));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.chkAll = new DevExpress.XtraEditors.CheckEdit();
            this.btnNo = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.chkList = new DevExpress.XtraEditors.CheckedListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkList)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cmbLayer);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(403, 40);
            this.panelControl1.TabIndex = 0;
            // 
            // cmbLayer
            // 
            this.cmbLayer.EditValue = "分类单元";
            this.cmbLayer.Location = new System.Drawing.Point(73, 9);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLayer.Properties.Items.AddRange(new object[] {
            "分类单元",
            "扩充分类单元"});
            this.cmbLayer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbLayer.Size = new System.Drawing.Size(318, 24);
            this.cmbLayer.TabIndex = 1;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "图层：";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.chkAll);
            this.panelControl2.Controls.Add(this.btnNo);
            this.panelControl2.Controls.Add(this.btnOk);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 341);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(403, 62);
            this.panelControl2.TabIndex = 1;
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(12, 9);
            this.chkAll.Name = "chkAll";
            this.chkAll.Properties.Caption = "全选/全不选";
            this.chkAll.Size = new System.Drawing.Size(136, 22);
            this.chkAll.TabIndex = 2;
            this.chkAll.CheckStateChanged += new System.EventHandler(this.checkEdit1_CheckStateChanged);
            // 
            // btnNo
            // 
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.Location = new System.Drawing.Point(304, 11);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(87, 39);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "关闭";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(189, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 39);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "赋值";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.chkList);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 40);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(403, 301);
            this.panelControl3.TabIndex = 2;
            // 
            // chkList
            // 
            this.chkList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkList.Location = new System.Drawing.Point(2, 2);
            this.chkList.Name = "chkList";
            this.chkList.Size = new System.Drawing.Size(399, 297);
            this.chkList.TabIndex = 0;
            // 
            // FrmDYBHGD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 403);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDYBHGD";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "单元编号赋值";
            this.Load += new System.EventHandler(this.FrmDYBHGD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLayer;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnNo;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.CheckedListBoxControl chkList;
        private DevExpress.XtraEditors.CheckEdit chkAll;

    }
}