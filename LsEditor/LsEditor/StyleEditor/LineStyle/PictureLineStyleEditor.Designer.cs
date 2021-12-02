namespace RCIS.Style.StyleEditor
{
    partial class PictureLineStyleEditor
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
            this.spinSize = new DevExpress.XtraEditors.SpinEdit();
            this.ceColor = new DevExpress.XtraEditors.ColorEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.btnSelectPicure = new System.Windows.Forms.Button();
            this.txtYScale = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtXScale = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ceBackgroundColor = new DevExpress.XtraEditors.ColorEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.colorEdit1 = new DevExpress.XtraEditors.ColorEdit();
            this.txtOffset = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceBackgroundColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOffset.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(697, 437);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(689, 412);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图片线";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.spinSize);
            this.groupBox1.Controls.Add(this.ceColor);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 406);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // spinSize
            // 
            this.spinSize.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSize.Location = new System.Drawing.Point(88, 74);
            this.spinSize.Name = "spinSize";
            this.spinSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinSize.Size = new System.Drawing.Size(210, 21);
            this.spinSize.TabIndex = 44;
            // 
            // ceColor
            // 
            this.ceColor.EditValue = System.Drawing.Color.Black;
            this.ceColor.Location = new System.Drawing.Point(88, 33);
            this.ceColor.Name = "ceColor";
            this.ceColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceColor.Size = new System.Drawing.Size(210, 21);
            this.ceColor.TabIndex = 43;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(28, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 23);
            this.label5.TabIndex = 36;
            this.label5.Text = "线宽";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(28, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 23);
            this.label6.TabIndex = 35;
            this.label6.Text = "颜色";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(646, 382);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(638, 357);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "图片线";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkEdit2);
            this.groupBox2.Controls.Add(this.checkEdit1);
            this.groupBox2.Controls.Add(this.btnSelectPicure);
            this.groupBox2.Controls.Add(this.txtYScale);
            this.groupBox2.Controls.Add(this.labelControl3);
            this.groupBox2.Controls.Add(this.txtXScale);
            this.groupBox2.Controls.Add(this.labelControl1);
            this.groupBox2.Controls.Add(this.ceBackgroundColor);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.spinEdit1);
            this.groupBox2.Controls.Add(this.colorEdit1);
            this.groupBox2.Controls.Add(this.txtOffset);
            this.groupBox2.Controls.Add(this.labelControl2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(632, 351);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(85, 205);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "是否旋转";
            this.checkEdit1.Size = new System.Drawing.Size(210, 19);
            this.checkEdit1.TabIndex = 52;
            // 
            // btnSelectPicure
            // 
            this.btnSelectPicure.Location = new System.Drawing.Point(85, 230);
            this.btnSelectPicure.Name = "btnSelectPicure";
            this.btnSelectPicure.Size = new System.Drawing.Size(210, 23);
            this.btnSelectPicure.TabIndex = 51;
            this.btnSelectPicure.Text = "选择图片";
            this.btnSelectPicure.UseVisualStyleBackColor = true;
            this.btnSelectPicure.Click += new System.EventHandler(this.btnSelectPicure_Click);
            // 
            // txtYScale
            // 
            this.txtYScale.EditValue = "0";
            this.txtYScale.Location = new System.Drawing.Point(86, 168);
            this.txtYScale.Name = "txtYScale";
            this.txtYScale.Size = new System.Drawing.Size(209, 21);
            this.txtYScale.TabIndex = 50;
            this.txtYScale.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(28, 171);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(35, 14);
            this.labelControl3.TabIndex = 49;
            this.labelControl3.Text = "XScale";
            // 
            // txtXScale
            // 
            this.txtXScale.EditValue = "0";
            this.txtXScale.Location = new System.Drawing.Point(86, 141);
            this.txtXScale.Name = "txtXScale";
            this.txtXScale.Size = new System.Drawing.Size(209, 21);
            this.txtXScale.TabIndex = 48;
            this.txtXScale.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 144);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(35, 14);
            this.labelControl1.TabIndex = 47;
            this.labelControl1.Text = "XScale";
            // 
            // ceBackgroundColor
            // 
            this.ceBackgroundColor.EditValue = System.Drawing.Color.Black;
            this.ceBackgroundColor.Location = new System.Drawing.Point(87, 60);
            this.ceBackgroundColor.Name = "ceBackgroundColor";
            this.ceBackgroundColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceBackgroundColor.Size = new System.Drawing.Size(210, 21);
            this.ceBackgroundColor.TabIndex = 46;
            this.ceBackgroundColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 45;
            this.label3.Text = "背景色";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(87, 87);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinEdit1.Size = new System.Drawing.Size(210, 21);
            this.spinEdit1.TabIndex = 44;
            this.spinEdit1.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // colorEdit1
            // 
            this.colorEdit1.EditValue = System.Drawing.Color.Black;
            this.colorEdit1.Location = new System.Drawing.Point(88, 33);
            this.colorEdit1.Name = "colorEdit1";
            this.colorEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.colorEdit1.Size = new System.Drawing.Size(210, 21);
            this.colorEdit1.TabIndex = 43;
            this.colorEdit1.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // txtOffset
            // 
            this.txtOffset.EditValue = "0";
            this.txtOffset.Location = new System.Drawing.Point(87, 114);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(209, 21);
            this.txtOffset.TabIndex = 42;
            this.txtOffset.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 117);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 41;
            this.labelControl2.Text = "偏移";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "线宽";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "颜色";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkEdit2
            // 
            this.checkEdit2.Location = new System.Drawing.Point(162, 205);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "转换前景色和背景色";
            this.checkEdit2.Size = new System.Drawing.Size(152, 19);
            this.checkEdit2.TabIndex = 53;
            this.checkEdit2.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // PictureLineStyleEditor
            // 
            this.Controls.Add(this.tabControl2);
            this.Name = "PictureLineStyleEditor";
            this.Size = new System.Drawing.Size(646, 382);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtYScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceBackgroundColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOffset.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private DevExpress.XtraEditors.ColorEdit ceColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.ColorEdit ceBackgroundColor;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.ColorEdit colorEdit1;
        private DevExpress.XtraEditors.TextEdit txtOffset;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtYScale;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtXScale;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Button btnSelectPicure;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
    }
}
