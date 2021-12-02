namespace TDDC3D.zrzy
{
    partial class JBNTTBBHSetValForm
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
            this.btnUnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectAll = new DevExpress.XtraEditors.SimpleButton();
            this.chkXzqList = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chkXzqList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUnSelect
            // 
            this.btnUnSelect.Location = new System.Drawing.Point(172, 30);
            this.btnUnSelect.Name = "btnUnSelect";
            this.btnUnSelect.Size = new System.Drawing.Size(71, 25);
            this.btnUnSelect.TabIndex = 3;
            this.btnUnSelect.Text = "反选";
            this.btnUnSelect.Click += new System.EventHandler(this.btnUnSelect_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(377, 30);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(68, 30);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(71, 25);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // chkXzqList
            // 
            this.chkXzqList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkXzqList.Location = new System.Drawing.Point(0, 0);
            this.chkXzqList.Name = "chkXzqList";
            this.chkXzqList.Size = new System.Drawing.Size(476, 459);
            this.chkXzqList.TabIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnUnSelect);
            this.panelControl1.Controls.Add(this.btnSelectAll);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 459);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(476, 80);
            this.panelControl1.TabIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(275, 30);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "执行";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // JBNTTBBHSetValForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 539);
            this.Controls.Add(this.chkXzqList);
            this.Controls.Add(this.panelControl1);
            this.Name = "JBNTTBBHSetValForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "保护图斑编号赋值";
            this.Load += new System.EventHandler(this.JBNTTBBHSetValForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkXzqList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnUnSelect;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton btnSelectAll;
        private DevExpress.XtraEditors.CheckedListBoxControl chkXzqList;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}