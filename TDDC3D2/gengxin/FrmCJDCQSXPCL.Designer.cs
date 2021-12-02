namespace TDDC3D.gengxin
{
    partial class FrmCJDCQSXPCL
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
            this.txtCJDCQBH = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnEditBGXW = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnJGJC = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtCJDCQBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCJDCQBH
            // 
            this.txtCJDCQBH.Location = new System.Drawing.Point(198, 17);
            this.txtCJDCQBH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCJDCQBH.Name = "txtCJDCQBH";
            this.txtCJDCQBH.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtCJDCQBH.Properties.ReadOnly = true;
            this.txtCJDCQBH.Size = new System.Drawing.Size(361, 28);
            this.txtCJDCQBH.TabIndex = 29;
            this.txtCJDCQBH.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtCJDCQBH_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(48, 20);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(144, 22);
            this.labelControl1.TabIndex = 28;
            this.labelControl1.Text = "变化村级调查区：";
            // 
            // btnEditBGXW
            // 
            this.btnEditBGXW.Location = new System.Drawing.Point(426, 73);
            this.btnEditBGXW.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEditBGXW.Name = "btnEditBGXW";
            this.btnEditBGXW.Size = new System.Drawing.Size(133, 48);
            this.btnEditBGXW.TabIndex = 27;
            this.btnEditBGXW.Text = "填写变更行为";
            this.btnEditBGXW.Click += new System.EventHandler(this.btnEditBGXW_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(48, 145);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(511, 240);
            this.info.TabIndex = 26;
            this.info.UseOptimizedRendering = true;
            // 
            // btnJGJC
            // 
            this.btnJGJC.Location = new System.Drawing.Point(260, 73);
            this.btnJGJC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnJGJC.Name = "btnJGJC";
            this.btnJGJC.Size = new System.Drawing.Size(133, 48);
            this.btnJGJC.TabIndex = 30;
            this.btnJGJC.Text = "结构检查";
            this.btnJGJC.Click += new System.EventHandler(this.btnJGJC_Click);
            // 
            // FrmCJDCQSXPCL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 398);
            this.Controls.Add(this.btnJGJC);
            this.Controls.Add(this.txtCJDCQBH);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnEditBGXW);
            this.Controls.Add(this.info);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCJDCQSXPCL";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "村级调查区属性批处理";
            ((System.ComponentModel.ISupportInitialize)(this.txtCJDCQBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit txtCJDCQBH;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnEditBGXW;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnJGJC;

    }
}