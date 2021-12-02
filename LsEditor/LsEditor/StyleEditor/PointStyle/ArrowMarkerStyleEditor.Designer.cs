namespace RCIS.Style.StyleEditor
{
    partial class ArrowMarkerStyleEditor
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.spinAngle = new DevExpress.XtraEditors.SpinEdit();
            this.ceColor = new DevExpress.XtraEditors.ColorEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.spinOffsetX = new DevExpress.XtraEditors.SpinEdit();
            this.spinOffsetY = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.spinLength = new DevExpress.XtraEditors.SpinEdit();
            this.spinWidth = new DevExpress.XtraEditors.SpinEdit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinLength.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinWidth.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(470, 422);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupControl3);
            this.tabPage1.Controls.Add(this.groupControl2);
            this.tabPage1.Controls.Add(this.groupControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(462, 397);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "箭头符号";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.label4);
            this.groupControl3.Controls.Add(this.label3);
            this.groupControl3.Controls.Add(this.spinAngle);
            this.groupControl3.Controls.Add(this.ceColor);
            this.groupControl3.Location = new System.Drawing.Point(235, 6);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(220, 100);
            this.groupControl3.TabIndex = 11;
            this.groupControl3.Text = "颜色和角度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "角度";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "颜色";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinAngle
            // 
            this.spinAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinAngle.Location = new System.Drawing.Point(66, 66);
            this.spinAngle.Name = "spinAngle";
            this.spinAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinAngle.Size = new System.Drawing.Size(143, 21);
            this.spinAngle.TabIndex = 5;
            this.spinAngle.EditValueChanged += new System.EventHandler(this.OnStylePropertiesChanged);
            // 
            // ceColor
            // 
            this.ceColor.EditValue = System.Drawing.Color.Empty;
            this.ceColor.Location = new System.Drawing.Point(66, 25);
            this.ceColor.Name = "ceColor";
            this.ceColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceColor.Size = new System.Drawing.Size(143, 21);
            this.ceColor.TabIndex = 0;
            this.ceColor.EditValueChanged += new System.EventHandler(this.OnStylePropertiesChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.label6);
            this.groupControl2.Controls.Add(this.label5);
            this.groupControl2.Controls.Add(this.spinOffsetX);
            this.groupControl2.Controls.Add(this.spinOffsetY);
            this.groupControl2.Location = new System.Drawing.Point(3, 126);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(226, 93);
            this.groupControl2.TabIndex = 10;
            this.groupControl2.Text = "偏移值";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "Y:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "X:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinOffsetX
            // 
            this.spinOffsetX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetX.Location = new System.Drawing.Point(68, 24);
            this.spinOffsetX.Name = "spinOffsetX";
            this.spinOffsetX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetX.Size = new System.Drawing.Size(148, 21);
            this.spinOffsetX.TabIndex = 4;
            this.spinOffsetX.EditValueChanged += new System.EventHandler(this.OnStylePropertiesChanged);
            // 
            // spinOffsetY
            // 
            this.spinOffsetY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetY.Location = new System.Drawing.Point(68, 60);
            this.spinOffsetY.Name = "spinOffsetY";
            this.spinOffsetY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetY.Size = new System.Drawing.Size(148, 21);
            this.spinOffsetY.TabIndex = 3;
            this.spinOffsetY.EditValueChanged += new System.EventHandler(this.OnStylePropertiesChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.spinLength);
            this.groupControl1.Controls.Add(this.spinWidth);
            this.groupControl1.Location = new System.Drawing.Point(3, 6);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(226, 100);
            this.groupControl1.TabIndex = 9;
            this.groupControl1.Text = "长度和宽度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "宽度";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "长度";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinLength
            // 
            this.spinLength.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinLength.Location = new System.Drawing.Point(68, 24);
            this.spinLength.Name = "spinLength";
            this.spinLength.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinLength.Size = new System.Drawing.Size(148, 21);
            this.spinLength.TabIndex = 2;
            this.spinLength.EditValueChanged += new System.EventHandler(this.OnStylePropertiesChanged);
            // 
            // spinWidth
            // 
            this.spinWidth.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinWidth.Location = new System.Drawing.Point(68, 66);
            this.spinWidth.Name = "spinWidth";
            this.spinWidth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinWidth.Size = new System.Drawing.Size(148, 21);
            this.spinWidth.TabIndex = 1;
            this.spinWidth.EditValueChanged += new System.EventHandler(this.OnStylePropertiesChanged);
            // 
            // ArrowMarkerStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "ArrowMarkerStyleEditor";
            this.Size = new System.Drawing.Size(470, 422);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinLength.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinWidth.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SpinEdit spinAngle;
        private DevExpress.XtraEditors.ColorEdit ceColor;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SpinEdit spinOffsetX;
        private DevExpress.XtraEditors.SpinEdit spinOffsetY;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SpinEdit spinLength;
        private DevExpress.XtraEditors.SpinEdit spinWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;

    }
}
