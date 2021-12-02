namespace TDDC3D.datado
{
    partial class TFHInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFHInfoForm));
            this.m_tfType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.chkInterpo = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.m_bottom = new DevExpress.XtraEditors.TextEdit();
            this.m_right = new DevExpress.XtraEditors.TextEdit();
            this.m_left = new DevExpress.XtraEditors.TextEdit();
            this.m_top = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkInterpo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_bottom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_right.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_left.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_top.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tfType
            // 
            this.m_tfType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_tfType.FormattingEnabled = true;
            this.m_tfType.Items.AddRange(new object[] {
            "1:1000",
            "1:2000",
            "1:5000",
            "1:10 000",
            "1:25 000",
            "1:50 000"});
            this.m_tfType.Location = new System.Drawing.Point(84, 9);
            this.m_tfType.Name = "m_tfType";
            this.m_tfType.Size = new System.Drawing.Size(288, 20);
            this.m_tfType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "图幅比例尺";
            // 
            // simpleButton1
            // 
            this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(304, 280);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(67, 23);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "取消";
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(200, 280);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(62, 23);
            this.simpleButton2.TabIndex = 6;
            this.simpleButton2.Text = "确定";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // chkInterpo
            // 
            this.chkInterpo.Location = new System.Drawing.Point(14, 245);
            this.chkInterpo.Margin = new System.Windows.Forms.Padding(2);
            this.chkInterpo.Name = "chkInterpo";
            this.chkInterpo.Properties.Caption = "秒插值";
            this.chkInterpo.Size = new System.Drawing.Size(88, 19);
            this.chkInterpo.TabIndex = 7;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.m_bottom);
            this.groupControl1.Controls.Add(this.m_right);
            this.groupControl1.Controls.Add(this.m_left);
            this.groupControl1.Controls.Add(this.m_top);
            this.groupControl1.Location = new System.Drawing.Point(14, 45);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(357, 187);
            this.groupControl1.TabIndex = 8;
            this.groupControl1.Text = "坐标范围";
            // 
            // m_bottom
            // 
            this.m_bottom.Location = new System.Drawing.Point(97, 126);
            this.m_bottom.Margin = new System.Windows.Forms.Padding(2);
            this.m_bottom.Name = "m_bottom";
            this.m_bottom.Size = new System.Drawing.Size(109, 20);
            this.m_bottom.TabIndex = 3;
            // 
            // m_right
            // 
            this.m_right.Location = new System.Drawing.Point(216, 79);
            this.m_right.Margin = new System.Windows.Forms.Padding(2);
            this.m_right.Name = "m_right";
            this.m_right.Size = new System.Drawing.Size(97, 20);
            this.m_right.TabIndex = 2;
            // 
            // m_left
            // 
            this.m_left.Location = new System.Drawing.Point(12, 79);
            this.m_left.Margin = new System.Windows.Forms.Padding(2);
            this.m_left.Name = "m_left";
            this.m_left.Size = new System.Drawing.Size(82, 20);
            this.m_left.TabIndex = 1;
            // 
            // m_top
            // 
            this.m_top.Location = new System.Drawing.Point(97, 33);
            this.m_top.Margin = new System.Windows.Forms.Padding(2);
            this.m_top.Name = "m_top";
            this.m_top.Size = new System.Drawing.Size(109, 20);
            this.m_top.TabIndex = 0;
            // 
            // TFHInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 314);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.chkInterpo);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_tfType);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TFHInfoForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "基本信息";
            this.Load += new System.EventHandler(this.TFHInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkInterpo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_bottom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_right.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_left.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_top.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_tfType;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.CheckEdit chkInterpo;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit m_bottom;
        private DevExpress.XtraEditors.TextEdit m_right;
        private DevExpress.XtraEditors.TextEdit m_left;
        private DevExpress.XtraEditors.TextEdit m_top;
    }
}