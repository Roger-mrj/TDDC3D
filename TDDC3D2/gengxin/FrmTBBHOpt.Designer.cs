namespace TDDC3D.gengxin
{
    partial class FrmTBBHOpt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTBBHOpt));
            this.chkXzqList = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chkXzqList)).BeginInit();
            this.SuspendLayout();
            // 
            // chkXzqList
            // 
            this.chkXzqList.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkXzqList.Location = new System.Drawing.Point(0, 0);
            this.chkXzqList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkXzqList.Name = "chkXzqList";
            this.chkXzqList.Size = new System.Drawing.Size(479, 392);
            this.chkXzqList.TabIndex = 2;
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(122, 427);
            this.simpleButton4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(71, 31);
            this.simpleButton4.TabIndex = 7;
            this.simpleButton4.Text = "反选";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(30, 427);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(71, 31);
            this.simpleButton3.TabIndex = 6;
            this.simpleButton3.Text = "全选";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(372, 427);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 31);
            this.simpleButton2.TabIndex = 5;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(214, 427);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(137, 31);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "图斑编号赋值";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmTBBHOpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 484);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.chkXzqList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTBBHOpt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图斑编号重计算";
            this.Load += new System.EventHandler(this.FrmTBBHOpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkXzqList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckedListBoxControl chkXzqList;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;

    }
}