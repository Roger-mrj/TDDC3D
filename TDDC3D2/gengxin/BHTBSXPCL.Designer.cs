namespace TDDC3D.gengxin
{
    partial class FrmBHTBSXPCL
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
            this.btnSelectBHTB = new DevExpress.XtraEditors.ButtonEdit();
            this.btnTBCheck = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditBGXW = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditTBH = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectBHTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(47, 43);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "变化图斑：";
            // 
            // btnSelectBHTB
            // 
            this.btnSelectBHTB.Location = new System.Drawing.Point(138, 40);
            this.btnSelectBHTB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectBHTB.Name = "btnSelectBHTB";
            this.btnSelectBHTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnSelectBHTB.Properties.ReadOnly = true;
            this.btnSelectBHTB.Size = new System.Drawing.Size(485, 28);
            this.btnSelectBHTB.TabIndex = 1;
            this.btnSelectBHTB.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnSelectBHTB_ButtonClick);
            // 
            // btnTBCheck
            // 
            this.btnTBCheck.Location = new System.Drawing.Point(32, 130);
            this.btnTBCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTBCheck.Name = "btnTBCheck";
            this.btnTBCheck.Size = new System.Drawing.Size(136, 48);
            this.btnTBCheck.TabIndex = 2;
            this.btnTBCheck.Text = "图斑检查";
            this.btnTBCheck.Click += new System.EventHandler(this.btnTBCheck_Click);
            // 
            // btnEditBGXW
            // 
            this.btnEditBGXW.Location = new System.Drawing.Point(267, 130);
            this.btnEditBGXW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditBGXW.Name = "btnEditBGXW";
            this.btnEditBGXW.Size = new System.Drawing.Size(136, 48);
            this.btnEditBGXW.TabIndex = 3;
            this.btnEditBGXW.Text = "填写变更行为";
            this.btnEditBGXW.Click += new System.EventHandler(this.btnEditBGXW_Click);
            // 
            // btnEditTBH
            // 
            this.btnEditTBH.Location = new System.Drawing.Point(502, 130);
            this.btnEditTBH.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditTBH.Name = "btnEditTBH";
            this.btnEditTBH.Size = new System.Drawing.Size(136, 48);
            this.btnEditTBH.TabIndex = 5;
            this.btnEditTBH.Text = "编写图斑号";
            this.btnEditTBH.Click += new System.EventHandler(this.btnEditTBH_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(12, 207);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(626, 288);
            this.info.TabIndex = 6;
            this.info.UseOptimizedRendering = true;
            // 
            // FrmBHTBSXPCL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 520);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnEditTBH);
            this.Controls.Add(this.btnEditBGXW);
            this.Controls.Add(this.btnTBCheck);
            this.Controls.Add(this.btnSelectBHTB);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBHTBSXPCL";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "变化图斑属性批处理";
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectBHTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit btnSelectBHTB;
        private DevExpress.XtraEditors.SimpleButton btnTBCheck;
        private DevExpress.XtraEditors.SimpleButton btnEditBGXW;
        private DevExpress.XtraEditors.SimpleButton btnEditTBH;
        private DevExpress.XtraEditors.MemoEdit info;
    }
}