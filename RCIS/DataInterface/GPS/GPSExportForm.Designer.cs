namespace RCIS.DataExchange.GPS
{
    partial class GPSExportForm
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
            this.beGPSfile = new DevExpress.XtraEditors.ButtonEdit();
            this.beShapefile = new DevExpress.XtraEditors.ButtonEdit();
            this.cbPtNO = new System.Windows.Forms.CheckBox();
            this.cbHasZ = new System.Windows.Forms.CheckBox();
            this.btnGO = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSplit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.beGPSfile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShapefile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beGPSfile
            // 
            this.beGPSfile.Location = new System.Drawing.Point(98, 26);
            this.beGPSfile.Name = "beGPSfile";
            this.beGPSfile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beGPSfile.Size = new System.Drawing.Size(344, 21);
            this.beGPSfile.TabIndex = 0;
            this.beGPSfile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beGPSfile_ButtonClick);
            // 
            // beShapefile
            // 
            this.beShapefile.Location = new System.Drawing.Point(98, 53);
            this.beShapefile.Name = "beShapefile";
            this.beShapefile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beShapefile.Size = new System.Drawing.Size(344, 21);
            this.beShapefile.TabIndex = 1;
            this.beShapefile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beShapefile_ButtonClick);
            // 
            // cbPtNO
            // 
            this.cbPtNO.AutoSize = true;
            this.cbPtNO.Location = new System.Drawing.Point(97, 81);
            this.cbPtNO.Name = "cbPtNO";
            this.cbPtNO.Size = new System.Drawing.Size(120, 16);
            this.cbPtNO.TabIndex = 2;
            this.cbPtNO.Text = "坐标数据包含点号";
            this.cbPtNO.UseVisualStyleBackColor = true;
            // 
            // cbHasZ
            // 
            this.cbHasZ.AutoSize = true;
            this.cbHasZ.Location = new System.Drawing.Point(97, 103);
            this.cbHasZ.Name = "cbHasZ";
            this.cbHasZ.Size = new System.Drawing.Size(150, 16);
            this.cbHasZ.TabIndex = 3;
            this.cbHasZ.Text = "坐标数据包含Z(高程)值";
            this.cbHasZ.UseVisualStyleBackColor = true;
            // 
            // btnGO
            // 
            this.btnGO.Location = new System.Drawing.Point(267, 127);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(75, 23);
            this.btnGO.TabIndex = 4;
            this.btnGO.Text = "转换";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(364, 127);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 5;
            this.btnQuit.Text = "退出";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "GPS文件";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Shapefile文件";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSplit
            // 
            this.tbSplit.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSplit.Location = new System.Drawing.Point(98, 126);
            this.tbSplit.MaxLength = 1;
            this.tbSplit.Name = "tbSplit";
            this.tbSplit.Size = new System.Drawing.Size(46, 26);
            this.tbSplit.TabIndex = 8;
            this.tbSplit.Text = ",";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "数据分割符";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPSExportForm
            // 
            this.AcceptButton = this.btnGO;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 159);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbSplit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.cbHasZ);
            this.Controls.Add(this.cbPtNO);
            this.Controls.Add(this.beShapefile);
            this.Controls.Add(this.beGPSfile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GPSExportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GPS点==>Shapefile";
            ((System.ComponentModel.ISupportInitialize)(this.beGPSfile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShapefile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit beGPSfile;
        private DevExpress.XtraEditors.ButtonEdit beShapefile;
        private System.Windows.Forms.CheckBox cbPtNO;
        private System.Windows.Forms.CheckBox cbHasZ;
        private System.Windows.Forms.Button btnGO;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSplit;
        private System.Windows.Forms.Label label3;
    }
}