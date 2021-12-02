namespace RCIS.Style.StyleEditor
{
    partial class CartographicLineStyleEditor
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
            this.txtFillLineMiter = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbLineCapStyle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.label4 = new System.Windows.Forms.Label();
            this.ceLineJoinStyle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFillLineMiter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbLineCapStyle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceLineJoinStyle.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(619, 394);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(611, 369);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "制图线";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.spinSize);
            this.groupBox1.Controls.Add(this.ceColor);
            this.groupBox1.Controls.Add(this.txtFillLineMiter);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.cbLineCapStyle);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ceLineJoinStyle);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(605, 363);
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
            this.spinSize.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
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
            this.ceColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // txtFillLineMiter
            // 
            this.txtFillLineMiter.EditValue = "0";
            this.txtFillLineMiter.Location = new System.Drawing.Point(88, 191);
            this.txtFillLineMiter.Name = "txtFillLineMiter";
            this.txtFillLineMiter.Size = new System.Drawing.Size(209, 21);
            this.txtFillLineMiter.TabIndex = 42;
            this.txtFillLineMiter.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(30, 194);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 41;
            this.labelControl2.Text = "斜角";
            // 
            // cbLineCapStyle
            // 
            this.cbLineCapStyle.Location = new System.Drawing.Point(88, 111);
            this.cbLineCapStyle.Name = "cbLineCapStyle";
            this.cbLineCapStyle.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbLineCapStyle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbLineCapStyle.Size = new System.Drawing.Size(209, 21);
            this.cbLineCapStyle.TabIndex = 40;
            this.cbLineCapStyle.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(30, 114);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(20, 14);
            this.labelControl1.TabIndex = 39;
            this.labelControl1.Text = "Cap";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "join ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceLineJoinStyle
            // 
            this.ceLineJoinStyle.Location = new System.Drawing.Point(88, 150);
            this.ceLineJoinStyle.Name = "ceLineJoinStyle";
            this.ceLineJoinStyle.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.ceLineJoinStyle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceLineJoinStyle.Size = new System.Drawing.Size(209, 21);
            this.ceLineJoinStyle.TabIndex = 37;
            this.ceLineJoinStyle.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
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
            // CartographicLineStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CartographicLineStyleEditor";
            this.Size = new System.Drawing.Size(619, 394);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFillLineMiter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbLineCapStyle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceLineJoinStyle.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.TextEdit txtFillLineMiter;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cbLineCapStyle;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ComboBoxEdit ceLineJoinStyle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private DevExpress.XtraEditors.ColorEdit ceColor;
    }
}
