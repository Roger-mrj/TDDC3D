namespace RCIS.Style.StyleEditor
{
    partial class DotDensityFillStyleEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdLine = new DevExpress.XtraEditors.SimpleButton();
            this.ceLineColor = new DevExpress.XtraEditors.ColorEdit();
            this.spinSize = new DevExpress.XtraEditors.SpinEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ceBackgroundColor = new DevExpress.XtraEditors.ColorEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.spDotSize = new DevExpress.XtraEditors.SpinEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.spDotCount = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.ceColor = new DevExpress.XtraEditors.ColorEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtDotSpacing = new DevExpress.XtraEditors.TextEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceLineColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceBackgroundColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spDotSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spDotCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDotSpacing.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(638, 389);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(630, 364);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "离散点填充面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 358);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(618, 338);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupControl1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 174);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(612, 161);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.cmdLine);
            this.groupControl1.Controls.Add(this.ceLineColor);
            this.groupControl1.Controls.Add(this.spinSize);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 17);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(606, 141);
            this.groupControl1.TabIndex = 16;
            this.groupControl1.Text = "轮廓线";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 23);
            this.label4.TabIndex = 17;
            this.label4.Text = "颜色";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdLine
            // 
            this.cmdLine.Location = new System.Drawing.Point(177, 94);
            this.cmdLine.Name = "cmdLine";
            this.cmdLine.Size = new System.Drawing.Size(75, 23);
            this.cmdLine.TabIndex = 17;
            this.cmdLine.Text = "轮廓线";
            this.cmdLine.Click += new System.EventHandler(this.cmdLine_Click);
            // 
            // ceLineColor
            // 
            this.ceLineColor.EditValue = System.Drawing.Color.Black;
            this.ceLineColor.Location = new System.Drawing.Point(42, 67);
            this.ceLineColor.Name = "ceLineColor";
            this.ceLineColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceLineColor.Size = new System.Drawing.Size(210, 21);
            this.ceLineColor.TabIndex = 16;
            this.ceLineColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // spinSize
            // 
            this.spinSize.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSize.Location = new System.Drawing.Point(42, 32);
            this.spinSize.Name = "spinSize";
            this.spinSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinSize.Size = new System.Drawing.Size(210, 21);
            this.spinSize.TabIndex = 12;
            this.spinSize.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "线宽";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ceBackgroundColor);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.spDotSize);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.spDotCount);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.ceColor);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.labelControl5);
            this.groupBox4.Controls.Add(this.txtDotSpacing);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 17);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(612, 157);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // ceBackgroundColor
            // 
            this.ceBackgroundColor.EditValue = System.Drawing.Color.Black;
            this.ceBackgroundColor.Location = new System.Drawing.Point(97, 47);
            this.ceBackgroundColor.Name = "ceBackgroundColor";
            this.ceBackgroundColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceBackgroundColor.Size = new System.Drawing.Size(210, 21);
            this.ceBackgroundColor.TabIndex = 34;
            this.ceBackgroundColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 32;
            this.label6.Text = "点大小";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spDotSize
            // 
            this.spDotSize.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spDotSize.Location = new System.Drawing.Point(97, 130);
            this.spDotSize.Name = "spDotSize";
            this.spDotSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spDotSize.Size = new System.Drawing.Size(210, 21);
            this.spDotSize.TabIndex = 31;
            this.spDotSize.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "点数量";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spDotCount
            // 
            this.spDotCount.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spDotCount.Location = new System.Drawing.Point(97, 100);
            this.spDotCount.Name = "spDotCount";
            this.spDotCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spDotCount.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spDotCount.Size = new System.Drawing.Size(210, 21);
            this.spDotCount.TabIndex = 29;
            this.spDotCount.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "背景色";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceColor
            // 
            this.ceColor.EditValue = System.Drawing.Color.Black;
            this.ceColor.Location = new System.Drawing.Point(97, 20);
            this.ceColor.Name = "ceColor";
            this.ceColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceColor.Size = new System.Drawing.Size(210, 21);
            this.ceColor.TabIndex = 10;
            this.ceColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "颜色";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(18, 78);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 25;
            this.labelControl5.Text = "点间距";
            // 
            // txtDotSpacing
            // 
            this.txtDotSpacing.EditValue = "0";
            this.txtDotSpacing.Location = new System.Drawing.Point(97, 75);
            this.txtDotSpacing.Name = "txtDotSpacing";
            this.txtDotSpacing.Size = new System.Drawing.Size(210, 21);
            this.txtDotSpacing.TabIndex = 26;
            this.txtDotSpacing.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // DotDensityFillStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "DotDensityFillStyleEditor";
            this.Size = new System.Drawing.Size(638, 389);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceLineColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceBackgroundColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spDotSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spDotCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDotSpacing.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton cmdLine;
        private DevExpress.XtraEditors.ColorEdit ceLineColor;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.ColorEdit ceBackgroundColor;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.SpinEdit spDotSize;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SpinEdit spDotCount;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ColorEdit ceColor;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtDotSpacing;
    }
}
