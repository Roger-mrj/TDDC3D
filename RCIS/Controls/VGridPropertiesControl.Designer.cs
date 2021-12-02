namespace RCIS.Controls
{
    partial class VGridPropertiesControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.vGridControl = new DevExpress.XtraVerticalGrid.VGridControl();
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl)).BeginInit();
            this.SuspendLayout();
            // 
            // vGridControl
            // 
            this.vGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vGridControl.Location = new System.Drawing.Point(0, 0);
            this.vGridControl.Name = "vGridControl";
            this.vGridControl.OptionsView.AutoScaleBands = true;
            this.vGridControl.Size = new System.Drawing.Size(323, 448);
            this.vGridControl.TabIndex = 1;
            // 
            // VGridPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vGridControl);
            this.Name = "VGridPropertiesControl";
            this.Size = new System.Drawing.Size(323, 448);
            ((System.ComponentModel.ISupportInitialize)(this.vGridControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraVerticalGrid.VGridControl vGridControl;
    }
}
