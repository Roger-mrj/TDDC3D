namespace RCIS.ElseTool
{
    partial class lineToPolygonFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(lineToPolygonFrm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSrcLine = new System.Windows.Forms.ComboBox();
            this.cmbDestPolygon = new System.Windows.Forms.ComboBox();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.rbCurrExtent = new System.Windows.Forms.RadioButton();
            this.rbAllFeatures = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择原始线层";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "选择目标面层";
            // 
            // cmbSrcLine
            // 
            this.cmbSrcLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSrcLine.FormattingEnabled = true;
            this.cmbSrcLine.Location = new System.Drawing.Point(133, 18);
            this.cmbSrcLine.Name = "cmbSrcLine";
            this.cmbSrcLine.Size = new System.Drawing.Size(185, 20);
            this.cmbSrcLine.TabIndex = 3;
            // 
            // cmbDestPolygon
            // 
            this.cmbDestPolygon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestPolygon.FormattingEnabled = true;
            this.cmbDestPolygon.Location = new System.Drawing.Point(133, 50);
            this.cmbDestPolygon.Name = "cmbDestPolygon";
            this.cmbDestPolygon.Size = new System.Drawing.Size(185, 20);
            this.cmbDestPolygon.TabIndex = 4;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(55, 116);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 25);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "开始";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(192, 116);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 25);
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // rbCurrExtent
            // 
            this.rbCurrExtent.AutoSize = true;
            this.rbCurrExtent.Checked = true;
            this.rbCurrExtent.Location = new System.Drawing.Point(55, 85);
            this.rbCurrExtent.Name = "rbCurrExtent";
            this.rbCurrExtent.Size = new System.Drawing.Size(71, 16);
            this.rbCurrExtent.TabIndex = 7;
            this.rbCurrExtent.TabStop = true;
            this.rbCurrExtent.Text = "当前区域";
            this.rbCurrExtent.UseVisualStyleBackColor = true;
            // 
            // rbAllFeatures
            // 
            this.rbAllFeatures.AutoSize = true;
            this.rbAllFeatures.Location = new System.Drawing.Point(172, 85);
            this.rbAllFeatures.Name = "rbAllFeatures";
            this.rbAllFeatures.Size = new System.Drawing.Size(71, 16);
            this.rbAllFeatures.TabIndex = 8;
            this.rbAllFeatures.Text = "全部要素";
            this.rbAllFeatures.UseVisualStyleBackColor = true;
            // 
            // lineToPolygonFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 162);
            this.Controls.Add(this.rbAllFeatures);
            this.Controls.Add(this.rbCurrExtent);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.cmbDestPolygon);
            this.Controls.Add(this.cmbSrcLine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "lineToPolygonFrm";
            this.ShowIcon = false;
            this.Text = "线转化为面";
            this.Load += new System.EventHandler(this.lineToPolygonFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSrcLine;
        private System.Windows.Forms.ComboBox cmbDestPolygon;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.RadioButton rbCurrExtent;
        private System.Windows.Forms.RadioButton rbAllFeatures;
    }
}