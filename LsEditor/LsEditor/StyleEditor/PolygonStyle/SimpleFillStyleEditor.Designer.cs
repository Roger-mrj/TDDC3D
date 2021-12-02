namespace RCIS.Style.StyleEditor
{
    partial class SimpleFillStyleEditor
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
            this.cmdLine = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label4 = new System.Windows.Forms.Label();
            this.ceLineColor = new DevExpress.XtraEditors.ColorEdit();
            this.spinSize = new DevExpress.XtraEditors.SpinEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ceStyle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceColor = new DevExpress.XtraEditors.ColorEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceLineColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceStyle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(596, 391);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(588, 366);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "简单填充面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdLine);
            this.groupBox1.Controls.Add(this.groupControl1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ceStyle);
            this.groupBox1.Controls.Add(this.ceColor);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 360);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cmdLine
            // 
            this.cmdLine.Location = new System.Drawing.Point(205, 218);
            this.cmdLine.Name = "cmdLine";
            this.cmdLine.Size = new System.Drawing.Size(75, 23);
            this.cmdLine.TabIndex = 17;
            this.cmdLine.Text = "轮廓线";
            this.cmdLine.Click += new System.EventHandler(this.cmdLine_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.ceLineColor);
            this.groupControl1.Controls.Add(this.spinSize);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Location = new System.Drawing.Point(22, 89);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(284, 113);
            this.groupControl1.TabIndex = 16;
            this.groupControl1.Text = "轮廓线";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 23);
            this.label4.TabIndex = 17;
            this.label4.Text = "颜色";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceLineColor
            // 
            this.ceLineColor.EditValue = System.Drawing.Color.Black;
            this.ceLineColor.Location = new System.Drawing.Point(69, 73);
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
            this.spinSize.Location = new System.Drawing.Point(69, 34);
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
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "样式";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "颜色";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceStyle
            // 
            this.ceStyle.Location = new System.Drawing.Point(79, 62);
            this.ceStyle.Name = "ceStyle";
            this.ceStyle.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.ceStyle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceStyle.Size = new System.Drawing.Size(227, 21);
            this.ceStyle.TabIndex = 11;
            this.ceStyle.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // ceColor
            // 
            this.ceColor.EditValue = System.Drawing.Color.Black;
            this.ceColor.Location = new System.Drawing.Point(79, 34);
            this.ceColor.Name = "ceColor";
            this.ceColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceColor.Size = new System.Drawing.Size(227, 21);
            this.ceColor.TabIndex = 10;
            this.ceColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // SimpleFillStyleEditor
            // 
            this.Controls.Add(this.tabControl1);
            this.Name = "SimpleFillStyleEditor";
            this.Size = new System.Drawing.Size(596, 391);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceLineColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceStyle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit ceStyle;
        private DevExpress.XtraEditors.ColorEdit ceColor;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ColorEdit ceLineColor;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton cmdLine;
    }
}
