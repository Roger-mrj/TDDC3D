namespace RCIS.MapTool
{
    partial class Polygon2PolylineFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Polygon2PolylineFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPolygonLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmbPolylineLayer = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.lblstatus = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolylineLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择要转化的面层";
            // 
            // cmbPolygonLayer
            // 
            this.cmbPolygonLayer.Location = new System.Drawing.Point(146, 26);
            this.cmbPolygonLayer.Name = "cmbPolygonLayer";
            this.cmbPolygonLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPolygonLayer.Size = new System.Drawing.Size(260, 20);
            this.cmbPolygonLayer.TabIndex = 1;
            // 
            // cmbPolylineLayer
            // 
            this.cmbPolylineLayer.Location = new System.Drawing.Point(146, 67);
            this.cmbPolylineLayer.Name = "cmbPolylineLayer";
            this.cmbPolylineLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPolylineLayer.Size = new System.Drawing.Size(260, 20);
            this.cmbPolylineLayer.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "选择目标线层";
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(56, 117);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "开始";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.Location = new System.Drawing.Point(306, 117);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblstatus
            // 
            this.lblstatus.Location = new System.Drawing.Point(27, 163);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(0, 14);
            this.lblstatus.TabIndex = 6;
            // 
            // Polygon2PolylineFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 199);
            this.Controls.Add(this.lblstatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbPolylineLayer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbPolygonLayer);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Polygon2PolylineFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "面转化为线";
            this.Load += new System.EventHandler(this.Polygon2PolylineFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolygonLayer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPolylineLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPolygonLayer;
        private DevExpress.XtraEditors.ComboBoxEdit cmbPolylineLayer;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl lblstatus;
    }
}