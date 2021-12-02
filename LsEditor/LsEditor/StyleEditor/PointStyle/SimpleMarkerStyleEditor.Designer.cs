namespace RCIS.Style.StyleEditor
{
    partial class SimpleMarkerStyleEditor
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpSimple = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.spinOutlineSize = new DevExpress.XtraEditors.SpinEdit();
            this.ceOutlineColor = new DevExpress.XtraEditors.ColorEdit();
            this.ceOutline = new DevExpress.XtraEditors.CheckEdit();
            this.spinOffsetY = new DevExpress.XtraEditors.SpinEdit();
            this.spinOffsetX = new DevExpress.XtraEditors.SpinEdit();
            this.spinSize = new DevExpress.XtraEditors.SpinEdit();
            this.ceStyle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceColor = new DevExpress.XtraEditors.ColorEdit();
            this.tabControl.SuspendLayout();
            this.tpSimple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinOutlineSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutlineColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceStyle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpSimple);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(469, 387);
            this.tabControl.TabIndex = 0;
            // 
            // tpSimple
            // 
            this.tpSimple.Controls.Add(this.label5);
            this.tpSimple.Controls.Add(this.label4);
            this.tpSimple.Controls.Add(this.label3);
            this.tpSimple.Controls.Add(this.label2);
            this.tpSimple.Controls.Add(this.label1);
            this.tpSimple.Controls.Add(this.groupControl1);
            this.tpSimple.Controls.Add(this.spinOffsetY);
            this.tpSimple.Controls.Add(this.spinOffsetX);
            this.tpSimple.Controls.Add(this.spinSize);
            this.tpSimple.Controls.Add(this.ceStyle);
            this.tpSimple.Controls.Add(this.ceColor);
            this.tpSimple.Location = new System.Drawing.Point(4, 21);
            this.tpSimple.Name = "tpSimple";
            this.tpSimple.Padding = new System.Windows.Forms.Padding(3);
            this.tpSimple.Size = new System.Drawing.Size(461, 362);
            this.tpSimple.TabIndex = 0;
            this.tpSimple.Text = "简单点";
            this.tpSimple.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(25, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 23);
            this.label5.TabIndex = 11;
            this.label5.Text = "Y偏移";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(25, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "X偏移";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(25, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "大小";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(25, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "样式";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(25, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "颜色";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label7);
            this.groupControl1.Controls.Add(this.label6);
            this.groupControl1.Controls.Add(this.spinOutlineSize);
            this.groupControl1.Controls.Add(this.ceOutlineColor);
            this.groupControl1.Controls.Add(this.ceOutline);
            this.groupControl1.Location = new System.Drawing.Point(27, 192);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(267, 83);
            this.groupControl1.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(5, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "宽度";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(5, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 23);
            this.label6.TabIndex = 12;
            this.label6.Text = "颜色";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinOutlineSize
            // 
            this.spinOutlineSize.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOutlineSize.Location = new System.Drawing.Point(56, 49);
            this.spinOutlineSize.Name = "spinOutlineSize";
            this.spinOutlineSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOutlineSize.Size = new System.Drawing.Size(200, 21);
            this.spinOutlineSize.TabIndex = 7;
            this.spinOutlineSize.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // ceOutlineColor
            // 
            this.ceOutlineColor.EditValue = System.Drawing.Color.Black;
            this.ceOutlineColor.Location = new System.Drawing.Point(56, 24);
            this.ceOutlineColor.Name = "ceOutlineColor";
            this.ceOutlineColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceOutlineColor.Size = new System.Drawing.Size(200, 21);
            this.ceOutlineColor.TabIndex = 6;
            this.ceOutlineColor.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // ceOutline
            // 
            this.ceOutline.Location = new System.Drawing.Point(5, 0);
            this.ceOutline.Name = "ceOutline";
            this.ceOutline.Properties.Caption = "使用外边框";
            this.ceOutline.Size = new System.Drawing.Size(126, 19);
            this.ceOutline.TabIndex = 5;
            this.ceOutline.CheckedChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // spinOffsetY
            // 
            this.spinOffsetY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetY.Location = new System.Drawing.Point(84, 165);
            this.spinOffsetY.Name = "spinOffsetY";
            this.spinOffsetY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetY.Size = new System.Drawing.Size(210, 21);
            this.spinOffsetY.TabIndex = 4;
            this.spinOffsetY.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // spinOffsetX
            // 
            this.spinOffsetX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetX.Location = new System.Drawing.Point(84, 138);
            this.spinOffsetX.Name = "spinOffsetX";
            this.spinOffsetX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetX.Size = new System.Drawing.Size(210, 21);
            this.spinOffsetX.TabIndex = 3;
            this.spinOffsetX.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // spinSize
            // 
            this.spinSize.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSize.Location = new System.Drawing.Point(84, 111);
            this.spinSize.Name = "spinSize";
            this.spinSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinSize.Size = new System.Drawing.Size(210, 21);
            this.spinSize.TabIndex = 2;
            this.spinSize.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // ceStyle
            // 
            this.ceStyle.Location = new System.Drawing.Point(84, 83);
            this.ceStyle.Name = "ceStyle";
            this.ceStyle.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.ceStyle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceStyle.Size = new System.Drawing.Size(210, 21);
            this.ceStyle.TabIndex = 1;
            this.ceStyle.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // ceColor
            // 
            this.ceColor.EditValue = System.Drawing.Color.Black;
            this.ceColor.Location = new System.Drawing.Point(84, 55);
            this.ceColor.Name = "ceColor";
            this.ceColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceColor.Size = new System.Drawing.Size(210, 21);
            this.ceColor.TabIndex = 0;
            this.ceColor.EditValueChanged += new System.EventHandler(this.OnMarkerStylePropertyChanged);
            // 
            // SimpleMarkerStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "SimpleMarkerStyleEditor";
            this.Size = new System.Drawing.Size(469, 387);
            this.tabControl.ResumeLayout(false);
            this.tpSimple.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinOutlineSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutlineColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceStyle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpSimple;
        private DevExpress.XtraEditors.ComboBoxEdit ceStyle;
        private DevExpress.XtraEditors.ColorEdit ceColor;
        private DevExpress.XtraEditors.SpinEdit spinOffsetY;
        private DevExpress.XtraEditors.SpinEdit spinOffsetX;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit ceOutline;
        private DevExpress.XtraEditors.SpinEdit spinOutlineSize;
        private DevExpress.XtraEditors.ColorEdit ceOutlineColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}
