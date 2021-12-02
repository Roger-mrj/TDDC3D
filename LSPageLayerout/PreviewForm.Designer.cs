namespace RCIS.Controls
{
    partial class PreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreviewForm));
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.pagesize = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.paperOrientation = new System.Windows.Forms.Label();
            this.aaa = new System.Windows.Forms.Label();
            this.paperSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.printerName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.axPageLayoutControl1 = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.Location = new System.Drawing.Point(510, 234);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(166, 25);
            this.button11.TabIndex = 23;
            this.button11.Text = "输出到打印机";
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.pagesize);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(512, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 88);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "纸张设置: ";
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(96, 64);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(52, 18);
            this.radioButton2.TabIndex = 19;
            this.radioButton2.Text = "横向";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 64);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(59, 18);
            this.radioButton1.TabIndex = 18;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "纵向";
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "纸张大小:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pagesize
            // 
            this.pagesize.Location = new System.Drawing.Point(14, 36);
            this.pagesize.Name = "pagesize";
            this.pagesize.Size = new System.Drawing.Size(142, 21);
            this.pagesize.TabIndex = 16;
            this.pagesize.Text = "comboBox1";
            this.pagesize.SelectedIndexChanged += new System.EventHandler(this.pagesize_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.paperOrientation);
            this.groupBox1.Controls.Add(this.aaa);
            this.groupBox1.Controls.Add(this.paperSize);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.printerName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(511, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 85);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "打印参数:";
            // 
            // paperOrientation
            // 
            this.paperOrientation.AutoSize = true;
            this.paperOrientation.Location = new System.Drawing.Point(77, 60);
            this.paperOrientation.Name = "paperOrientation";
            this.paperOrientation.Size = new System.Drawing.Size(61, 13);
            this.paperOrientation.TabIndex = 5;
            this.paperOrientation.Text = "Orientation";
            this.paperOrientation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // aaa
            // 
            this.aaa.AutoSize = true;
            this.aaa.Location = new System.Drawing.Point(8, 62);
            this.aaa.Name = "aaa";
            this.aaa.Size = new System.Drawing.Size(59, 13);
            this.aaa.TabIndex = 4;
            this.aaa.Text = "纸张方向:";
            this.aaa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // paperSize
            // 
            this.paperSize.AutoSize = true;
            this.paperSize.Location = new System.Drawing.Point(78, 42);
            this.paperSize.Name = "paperSize";
            this.paperSize.Size = new System.Drawing.Size(26, 13);
            this.paperSize.TabIndex = 3;
            this.paperSize.Text = "Size";
            this.paperSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "纸张大小:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // printerName
            // 
            this.printerName.AutoSize = true;
            this.printerName.Location = new System.Drawing.Point(77, 21);
            this.printerName.Name = "printerName";
            this.printerName.Size = new System.Drawing.Size(66, 13);
            this.printerName.TabIndex = 1;
            this.printerName.Text = "printerName";
            this.printerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "打印机名称:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 477);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(680, 18);
            this.statusStrip1.TabIndex = 41;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(34, 13);
            this.toolStripStatusLabel1.Text = "song";
            // 
            // axPageLayoutControl1
            // 
            this.axPageLayoutControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axPageLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.axPageLayoutControl1.Name = "axPageLayoutControl1";
            this.axPageLayoutControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl1.OcxState")));
            this.axPageLayoutControl1.Size = new System.Drawing.Size(492, 477);
            this.axPageLayoutControl1.TabIndex = 42;
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 495);
            this.Controls.Add(this.axPageLayoutControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreviewForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PreviewForm";
            this.Load += new System.EventHandler(this.PreviewForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreviewForm_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox pagesize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label paperOrientation;
        private System.Windows.Forms.Label aaa;
        private System.Windows.Forms.Label paperSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label printerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl1;
    }
}