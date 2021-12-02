namespace TDDC3D.gengxin
{
    partial class FrmDLTBGXGC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDLTBGXGC));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDLTBBH = new DevExpress.XtraEditors.ButtonEdit();
            this.btnGXGC = new DevExpress.XtraEditors.SimpleButton();
            this.btnComputeMJ = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.info = new DevExpress.XtraEditors.MemoEdit();
            this.btnComputeTBBH = new DevExpress.XtraEditors.SimpleButton();
            this.btnComputeBSM = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtDLTBBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(40, 40);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "变化图斑：";
            // 
            // txtDLTBBH
            // 
            this.txtDLTBBH.Location = new System.Drawing.Point(132, 36);
            this.txtDLTBBH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDLTBBH.Name = "txtDLTBBH";
            this.txtDLTBBH.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDLTBBH.Properties.ReadOnly = true;
            this.txtDLTBBH.Size = new System.Drawing.Size(534, 28);
            this.txtDLTBBH.TabIndex = 1;
            this.txtDLTBBH.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtDLTBBH_ButtonClick);
            // 
            // btnGXGC
            // 
            this.btnGXGC.Location = new System.Drawing.Point(40, 104);
            this.btnGXGC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGXGC.Name = "btnGXGC";
            this.btnGXGC.Size = new System.Drawing.Size(143, 46);
            this.btnGXGC.TabIndex = 2;
            this.btnGXGC.Text = "生成更新过程";
            this.btnGXGC.Click += new System.EventHandler(this.btnGXGC_Click);
            // 
            // btnComputeMJ
            // 
            this.btnComputeMJ.Location = new System.Drawing.Point(440, 104);
            this.btnComputeMJ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComputeMJ.Name = "btnComputeMJ";
            this.btnComputeMJ.Size = new System.Drawing.Size(118, 46);
            this.btnComputeMJ.TabIndex = 3;
            this.btnComputeMJ.Text = "计算面积";
            this.btnComputeMJ.Click += new System.EventHandler(this.btnComputeMJ_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(565, 104);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 46);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // info
            // 
            this.info.Location = new System.Drawing.Point(40, 179);
            this.info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.info.Name = "info";
            this.info.Properties.ReadOnly = true;
            this.info.Size = new System.Drawing.Size(626, 288);
            this.info.TabIndex = 5;
            this.info.UseOptimizedRendering = true;
            // 
            // btnComputeTBBH
            // 
            this.btnComputeTBBH.Location = new System.Drawing.Point(190, 104);
            this.btnComputeTBBH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComputeTBBH.Name = "btnComputeTBBH";
            this.btnComputeTBBH.Size = new System.Drawing.Size(118, 46);
            this.btnComputeTBBH.TabIndex = 6;
            this.btnComputeTBBH.Text = "重排图斑号";
            this.btnComputeTBBH.Click += new System.EventHandler(this.btnComputeTBBH_Click);
            // 
            // btnComputeBSM
            // 
            this.btnComputeBSM.Location = new System.Drawing.Point(315, 104);
            this.btnComputeBSM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComputeBSM.Name = "btnComputeBSM";
            this.btnComputeBSM.Size = new System.Drawing.Size(118, 46);
            this.btnComputeBSM.TabIndex = 7;
            this.btnComputeBSM.Text = "重排标识码";
            this.btnComputeBSM.Click += new System.EventHandler(this.btnComputeBSM_Click);
            // 
            // FrmDLTBGXGC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 494);
            this.Controls.Add(this.btnComputeBSM);
            this.Controls.Add(this.btnComputeTBBH);
            this.Controls.Add(this.info);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnComputeMJ);
            this.Controls.Add(this.btnGXGC);
            this.Controls.Add(this.txtDLTBBH);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDLTBGXGC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成地类图斑更新过程图层";
            ((System.ComponentModel.ISupportInitialize)(this.txtDLTBBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.info.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit txtDLTBBH;
        private DevExpress.XtraEditors.SimpleButton btnGXGC;
        private DevExpress.XtraEditors.SimpleButton btnComputeMJ;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.MemoEdit info;
        private DevExpress.XtraEditors.SimpleButton btnComputeTBBH;
        private DevExpress.XtraEditors.SimpleButton btnComputeBSM;
    }
}