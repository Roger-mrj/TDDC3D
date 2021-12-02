namespace RCIS.Style.StyleEditor
{
    partial class TextSymbolStyleEditor
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ceFontColor = new DevExpress.XtraEditors.ColorEdit();
            this.ceFontStroke = new DevExpress.XtraEditors.CheckEdit();
            this.ceFontItalic = new DevExpress.XtraEditors.CheckEdit();
            this.ceFontUnderline = new DevExpress.XtraEditors.CheckEdit();
            this.cbFontSize = new System.Windows.Forms.ComboBox();
            this.cbFontName = new System.Windows.Forms.ComboBox();
            this.ceFontBold = new DevExpress.XtraEditors.CheckEdit();
            this.pbFontSample = new System.Windows.Forms.PictureBox();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtText = new DevExpress.XtraEditors.TextEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontStroke.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontItalic.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontUnderline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontBold.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFontSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtText.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(605, 399);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(597, 374);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文字样式";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(591, 368);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtText);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.pbFontSample);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.ceFontColor);
            this.groupControl1.Controls.Add(this.ceFontStroke);
            this.groupControl1.Controls.Add(this.ceFontItalic);
            this.groupControl1.Controls.Add(this.ceFontUnderline);
            this.groupControl1.Controls.Add(this.cbFontSize);
            this.groupControl1.Controls.Add(this.cbFontName);
            this.groupControl1.Controls.Add(this.ceFontBold);
            this.groupControl1.Location = new System.Drawing.Point(6, 20);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(359, 165);
            this.groupControl1.TabIndex = 16;
            this.groupControl1.Text = "基本设置";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 87);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 20;
            this.labelControl3.Text = "字体颜色";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 59);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 19;
            this.labelControl2.Text = "字体大小";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 36);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "字体类型";
            // 
            // ceFontColor
            // 
            this.ceFontColor.EditValue = System.Drawing.Color.Black;
            this.ceFontColor.Location = new System.Drawing.Point(78, 84);
            this.ceFontColor.Name = "ceFontColor";
            this.ceFontColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceFontColor.Size = new System.Drawing.Size(120, 21);
            this.ceFontColor.TabIndex = 17;
            this.ceFontColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // ceFontStroke
            // 
            this.ceFontStroke.Location = new System.Drawing.Point(148, 140);
            this.ceFontStroke.Name = "ceFontStroke";
            this.ceFontStroke.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ceFontStroke.Properties.Appearance.Options.UseFont = true;
            this.ceFontStroke.Properties.Caption = "中划线";
            this.ceFontStroke.Size = new System.Drawing.Size(76, 19);
            this.ceFontStroke.TabIndex = 16;
            this.ceFontStroke.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // ceFontItalic
            // 
            this.ceFontItalic.Location = new System.Drawing.Point(102, 140);
            this.ceFontItalic.Name = "ceFontItalic";
            this.ceFontItalic.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ceFontItalic.Properties.Appearance.Options.UseFont = true;
            this.ceFontItalic.Properties.Caption = "斜体";
            this.ceFontItalic.Size = new System.Drawing.Size(54, 19);
            this.ceFontItalic.TabIndex = 15;
            this.ceFontItalic.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // ceFontUnderline
            // 
            this.ceFontUnderline.Location = new System.Drawing.Point(46, 139);
            this.ceFontUnderline.Name = "ceFontUnderline";
            this.ceFontUnderline.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ceFontUnderline.Properties.Appearance.Options.UseFont = true;
            this.ceFontUnderline.Properties.Caption = "下划线";
            this.ceFontUnderline.Size = new System.Drawing.Size(66, 19);
            this.ceFontUnderline.TabIndex = 14;
            this.ceFontUnderline.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // cbFontSize
            // 
            this.cbFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "24",
            "32",
            "36",
            "48",
            "72"});
            this.cbFontSize.Location = new System.Drawing.Point(79, 58);
            this.cbFontSize.Name = "cbFontSize";
            this.cbFontSize.Size = new System.Drawing.Size(120, 20);
            this.cbFontSize.TabIndex = 13;
            this.cbFontSize.Text = "8";
            this.cbFontSize.SelectedIndexChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // cbFontName
            // 
            this.cbFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFontName.Location = new System.Drawing.Point(79, 35);
            this.cbFontName.MaxDropDownItems = 20;
            this.cbFontName.Name = "cbFontName";
            this.cbFontName.Size = new System.Drawing.Size(120, 20);
            this.cbFontName.TabIndex = 12;
            this.cbFontName.SelectedIndexChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // ceFontBold
            // 
            this.ceFontBold.Location = new System.Drawing.Point(-1, 139);
            this.ceFontBold.Name = "ceFontBold";
            this.ceFontBold.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ceFontBold.Properties.Appearance.Options.UseFont = true;
            this.ceFontBold.Properties.Caption = "粗体";
            this.ceFontBold.Size = new System.Drawing.Size(51, 19);
            this.ceFontBold.TabIndex = 11;
            this.ceFontBold.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // pbFontSample
            // 
            this.pbFontSample.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbFontSample.Location = new System.Drawing.Point(205, 21);
            this.pbFontSample.Name = "pbFontSample";
            this.pbFontSample.Size = new System.Drawing.Size(152, 142);
            this.pbFontSample.TabIndex = 21;
            this.pbFontSample.TabStop = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(15, 111);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 22;
            this.labelControl4.Text = "字体内容";
            // 
            // txtText
            // 
            this.txtText.EditValue = "文本样式";
            this.txtText.Location = new System.Drawing.Point(79, 113);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(119, 21);
            this.txtText.TabIndex = 23;
            this.txtText.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // TextSymbolStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "TextSymbolStyleEditor";
            this.Size = new System.Drawing.Size(605, 399);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontStroke.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontItalic.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontUnderline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontBold.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFontSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtText.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ColorEdit ceFontColor;
        private DevExpress.XtraEditors.CheckEdit ceFontStroke;
        private DevExpress.XtraEditors.CheckEdit ceFontItalic;
        private DevExpress.XtraEditors.CheckEdit ceFontUnderline;
        private System.Windows.Forms.ComboBox cbFontSize;
        private System.Windows.Forms.ComboBox cbFontName;
        private DevExpress.XtraEditors.CheckEdit ceFontBold;
        private System.Windows.Forms.PictureBox pbFontSample;
        private DevExpress.XtraEditors.TextEdit txtText;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}
