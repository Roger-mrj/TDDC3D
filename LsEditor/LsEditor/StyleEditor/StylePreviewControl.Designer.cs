namespace RCIS.Style.StyleEditor
{
    partial class StylePreviewControl
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
            this.gpOption = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbScale = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDogleg = new System.Windows.Forms.CheckBox();
            this.cbAxies = new System.Windows.Forms.CheckBox();
            this.pbStyle = new System.Windows.Forms.PictureBox();
            this.gpOption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStyle)).BeginInit();
            this.SuspendLayout();
            // 
            // gpOption
            // 
            this.gpOption.Controls.Add(this.label2);
            this.gpOption.Controls.Add(this.cbScale);
            this.gpOption.Controls.Add(this.label1);
            this.gpOption.Controls.Add(this.cbDogleg);
            this.gpOption.Controls.Add(this.cbAxies);
            this.gpOption.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpOption.Location = new System.Drawing.Point(0, 118);
            this.gpOption.Name = "gpOption";
            this.gpOption.Size = new System.Drawing.Size(172, 50);
            this.gpOption.TabIndex = 0;
            this.gpOption.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(145, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "%";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Visible = false;
            // 
            // cbScale
            // 
            this.cbScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbScale.FormattingEnabled = true;
            this.cbScale.Items.AddRange(new object[] {
            "25",
            "50",
            "100",
            "200",
            "400",
            "800"});
            this.cbScale.Location = new System.Drawing.Point(79, 27);
            this.cbScale.Name = "cbScale";
            this.cbScale.Size = new System.Drawing.Size(64, 20);
            this.cbScale.TabIndex = 3;
            this.cbScale.Visible = false;
            this.cbScale.SelectedIndexChanged += new System.EventHandler(this.cbScale_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(3, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "显示比例";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // cbDogleg
            // 
            this.cbDogleg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDogleg.AutoSize = true;
            this.cbDogleg.Location = new System.Drawing.Point(79, 12);
            this.cbDogleg.Name = "cbDogleg";
            this.cbDogleg.Size = new System.Drawing.Size(48, 16);
            this.cbDogleg.TabIndex = 1;
            this.cbDogleg.Text = "折线";
            this.cbDogleg.UseVisualStyleBackColor = true;
            // 
            // cbAxies
            // 
            this.cbAxies.AutoSize = true;
            this.cbAxies.Location = new System.Drawing.Point(4, 11);
            this.cbAxies.Name = "cbAxies";
            this.cbAxies.Size = new System.Drawing.Size(60, 16);
            this.cbAxies.TabIndex = 0;
            this.cbAxies.Text = "坐标轴";
            this.cbAxies.UseVisualStyleBackColor = true;
            this.cbAxies.CheckedChanged += new System.EventHandler(this.cbAxies_CheckedChanged);
            // 
            // pbStyle
            // 
            this.pbStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbStyle.Location = new System.Drawing.Point(0, 0);
            this.pbStyle.Name = "pbStyle";
            this.pbStyle.Size = new System.Drawing.Size(172, 118);
            this.pbStyle.TabIndex = 1;
            this.pbStyle.TabStop = false;
            this.pbStyle.SizeChanged += new System.EventHandler(this.pbStyle_SizeChanged);
            // 
            // StylePreviewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbStyle);
            this.Controls.Add(this.gpOption);
            this.Name = "StylePreviewControl";
            this.Size = new System.Drawing.Size(172, 168);
            this.gpOption.ResumeLayout(false);
            this.gpOption.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbStyle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpOption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbDogleg;
        private System.Windows.Forms.CheckBox cbAxies;
        private System.Windows.Forms.ComboBox cbScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pbStyle;
    }
}
