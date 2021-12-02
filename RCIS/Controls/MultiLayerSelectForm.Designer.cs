namespace RCIS.Controls
{
    partial class MultiLayerSelectForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.layerList = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.teWidth = new System.Windows.Forms.TextBox();
            this.teSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ceOutline = new DevExpress.XtraEditors.ColorEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.ceFill = new DevExpress.XtraEditors.ColorEdit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFill.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUnSelectAll);
            this.groupBox1.Controls.Add(this.btnSelectAll);
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Controls.Add(this.btnExit);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 537);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(628, 52);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnUnSelectAll
            // 
            this.btnUnSelectAll.Location = new System.Drawing.Point(124, 16);
            this.btnUnSelectAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(100, 29);
            this.btnUnSelectAll.TabIndex = 3;
            this.btnUnSelectAll.Text = "取消全选";
            this.btnUnSelectAll.UseVisualStyleBackColor = true;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(16, 16);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 29);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(388, 15);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 29);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(496, 15);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 29);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "取消";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.teWidth);
            this.groupControl1.Controls.Add(this.teSize);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.ceOutline);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.ceFill);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(628, 137);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "设置";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.layerList);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 137);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(628, 400);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "选择图层";
            // 
            // layerList
            // 
            this.layerList.CheckOnClick = true;
            this.layerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layerList.FormattingEnabled = true;
            this.layerList.Location = new System.Drawing.Point(2, 26);
            this.layerList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.layerList.Name = "layerList";
            this.layerList.Size = new System.Drawing.Size(624, 372);
            this.layerList.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(369, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 29);
            this.label4.TabIndex = 29;
            this.label4.Text = "宽度";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(369, 45);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 29);
            this.label3.TabIndex = 28;
            this.label3.Text = "大小";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // teWidth
            // 
            this.teWidth.Location = new System.Drawing.Point(476, 85);
            this.teWidth.Margin = new System.Windows.Forms.Padding(4);
            this.teWidth.Name = "teWidth";
            this.teWidth.Size = new System.Drawing.Size(127, 25);
            this.teWidth.TabIndex = 27;
            this.teWidth.Text = "1";
            // 
            // teSize
            // 
            this.teSize.Location = new System.Drawing.Point(476, 45);
            this.teSize.Margin = new System.Windows.Forms.Padding(4);
            this.teSize.Name = "teSize";
            this.teSize.Size = new System.Drawing.Size(127, 25);
            this.teSize.TabIndex = 26;
            this.teSize.Text = "1";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 29);
            this.label2.TabIndex = 25;
            this.label2.Text = "边框颜色";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceOutline
            // 
            this.ceOutline.EditValue = System.Drawing.Color.Black;
            this.ceOutline.Location = new System.Drawing.Point(124, 85);
            this.ceOutline.Margin = new System.Windows.Forms.Padding(4);
            this.ceOutline.Name = "ceOutline";
            this.ceOutline.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceOutline.Size = new System.Drawing.Size(235, 24);
            this.ceOutline.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 29);
            this.label1.TabIndex = 23;
            this.label1.Text = "填充颜色";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceFill
            // 
            this.ceFill.EditValue = System.Drawing.Color.White;
            this.ceFill.Location = new System.Drawing.Point(124, 45);
            this.ceFill.Margin = new System.Windows.Forms.Padding(4);
            this.ceFill.Name = "ceFill";
            this.ceFill.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceFill.Size = new System.Drawing.Size(235, 24);
            this.ceFill.TabIndex = 22;
            // 
            // MultiLayerSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 589);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiLayerSelectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择图层-只针对面状图层";
            this.Load += new System.EventHandler(this.MultiLayerSelectForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceOutline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFill.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button btnUnSelectAll;
        public  System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnExit;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.CheckedListBox layerList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox teWidth;
        private System.Windows.Forms.TextBox teSize;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ColorEdit ceOutline;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ColorEdit ceFill;
    }
}