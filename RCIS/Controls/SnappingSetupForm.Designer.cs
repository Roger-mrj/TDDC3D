namespace RCIS.Controls
{
    partial class SnappingSetupForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.chkLayers = new DevExpress.XtraEditors.CheckedListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.chkLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // chkLayers
            // 
            this.chkLayers.CheckOnClick = true;
            this.chkLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLayers.Location = new System.Drawing.Point(0, 0);
            this.chkLayers.Name = "chkLayers";
            this.chkLayers.Size = new System.Drawing.Size(478, 437);
            this.chkLayers.TabIndex = 0;
            this.chkLayers.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.chkLayers_ItemCheck);
            // 
            // SnappingSetupForm
            // 
            this.ClientSize = new System.Drawing.Size(478, 437);
            this.Controls.Add(this.chkLayers);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SnappingSetupForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "捕捉设置";
            this.Load += new System.EventHandler(this.SnappingSetupForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckedListBoxControl chkLayers;


    }
}