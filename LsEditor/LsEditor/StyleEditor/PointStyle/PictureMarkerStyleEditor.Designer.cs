namespace RCIS.Style.StyleEditor
{
    partial class PictureMarkerStyleEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.spinAngle = new DevExpress.XtraEditors.SpinEdit();
            this.btnSelectPicure = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.spinOffsetY = new DevExpress.XtraEditors.SpinEdit();
            this.spinOffsetX = new DevExpress.XtraEditors.SpinEdit();
            this.spinSize = new DevExpress.XtraEditors.SpinEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(536, 430);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.spinAngle);
            this.tabPage1.Controls.Add(this.btnSelectPicure);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.spinOffsetY);
            this.tabPage1.Controls.Add(this.spinOffsetX);
            this.tabPage1.Controls.Add(this.spinSize);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(528, 405);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图片填充点";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(38, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 23);
            this.label1.TabIndex = 20;
            this.label1.Text = "角度";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinAngle
            // 
            this.spinAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinAngle.Location = new System.Drawing.Point(97, 86);
            this.spinAngle.Name = "spinAngle";
            this.spinAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinAngle.Size = new System.Drawing.Size(210, 21);
            this.spinAngle.TabIndex = 19;
            this.spinAngle.EditValueChanged += new System.EventHandler(this.OnStylePropertyChanged);
            // 
            // btnSelectPicure
            // 
            this.btnSelectPicure.Location = new System.Drawing.Point(97, 27);
            this.btnSelectPicure.Name = "btnSelectPicure";
            this.btnSelectPicure.Size = new System.Drawing.Size(210, 23);
            this.btnSelectPicure.TabIndex = 18;
            this.btnSelectPicure.Text = "选择图片";
            this.btnSelectPicure.UseVisualStyleBackColor = true;
            this.btnSelectPicure.Click += new System.EventHandler(this.btnSelectPicure_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(38, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 23);
            this.label5.TabIndex = 17;
            this.label5.Text = "Y偏移";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(38, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 23);
            this.label4.TabIndex = 16;
            this.label4.Text = "X偏移";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(38, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "大小";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinOffsetY
            // 
            this.spinOffsetY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetY.Location = new System.Drawing.Point(97, 142);
            this.spinOffsetY.Name = "spinOffsetY";
            this.spinOffsetY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetY.Size = new System.Drawing.Size(210, 21);
            this.spinOffsetY.TabIndex = 14;
            this.spinOffsetY.EditValueChanged += new System.EventHandler(this.OnStylePropertyChanged);
            // 
            // spinOffsetX
            // 
            this.spinOffsetX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetX.Location = new System.Drawing.Point(97, 115);
            this.spinOffsetX.Name = "spinOffsetX";
            this.spinOffsetX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetX.Size = new System.Drawing.Size(210, 21);
            this.spinOffsetX.TabIndex = 13;
            this.spinOffsetX.EditValueChanged += new System.EventHandler(this.OnStylePropertyChanged);
            // 
            // spinSize
            // 
            this.spinSize.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSize.Location = new System.Drawing.Point(97, 58);
            this.spinSize.Name = "spinSize";
            this.spinSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinSize.Size = new System.Drawing.Size(210, 21);
            this.spinSize.TabIndex = 12;
            this.spinSize.EditValueChanged += new System.EventHandler(this.OnStylePropertyChanged);
            // 
            // PictureMarkerStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PictureMarkerStyleEditor";
            this.Size = new System.Drawing.Size(536, 430);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnSelectPicure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SpinEdit spinOffsetY;
        private DevExpress.XtraEditors.SpinEdit spinOffsetX;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit spinAngle;
    }
}
