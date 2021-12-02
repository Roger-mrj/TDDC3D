namespace RCIS.Controls
{
    partial class SimpleStyleFrm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.teWidth = new System.Windows.Forms.TextBox();
            this.teSize = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pbStyle = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ceOutline = new DevExpress.XtraEditors.ColorEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.ceFill = new DevExpress.XtraEditors.ColorEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFill.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(282, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 23);
            this.label4.TabIndex = 21;
            this.label4.Text = "宽度";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(282, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 20;
            this.label3.Text = "大小";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // teWidth
            // 
            this.teWidth.Location = new System.Drawing.Point(362, 62);
            this.teWidth.Name = "teWidth";
            this.teWidth.Size = new System.Drawing.Size(96, 21);
            this.teWidth.TabIndex = 19;
            this.teWidth.Text = "1";
            this.teWidth.TextChanged += new System.EventHandler(this.teWidth_TextChanged);
            // 
            // teSize
            // 
            this.teSize.Location = new System.Drawing.Point(362, 30);
            this.teSize.Name = "teSize";
            this.teSize.Size = new System.Drawing.Size(96, 21);
            this.teSize.TabIndex = 18;
            this.teSize.Text = "1";
            this.teSize.TextChanged += new System.EventHandler(this.teSize_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(386, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(298, 270);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pbStyle
            // 
            this.pbStyle.Location = new System.Drawing.Point(18, 94);
            this.pbStyle.Name = "pbStyle";
            this.pbStyle.Size = new System.Drawing.Size(456, 160);
            this.pbStyle.TabIndex = 15;
            this.pbStyle.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "边框颜色";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceOutline
            // 
            this.ceOutline.EditValue = System.Drawing.Color.Black;
            this.ceOutline.Location = new System.Drawing.Point(98, 62);
            this.ceOutline.Name = "ceOutline";
            this.ceOutline.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceOutline.Size = new System.Drawing.Size(176, 20);
            this.ceOutline.TabIndex = 13;
            this.ceOutline.EditValueChanged += new System.EventHandler(this.ceOutline_EditValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "填充颜色";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ceFill
            // 
            this.ceFill.EditValue = System.Drawing.Color.White;
            this.ceFill.Location = new System.Drawing.Point(98, 30);
            this.ceFill.Name = "ceFill";
            this.ceFill.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceFill.Size = new System.Drawing.Size(176, 20);
            this.ceFill.TabIndex = 11;
            this.ceFill.EditValueChanged += new System.EventHandler(this.ceFill_EditValueChanged);
            // 
            // SimpleStyleFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 310);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.teWidth);
            this.Controls.Add(this.teSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pbStyle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ceOutline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ceFill);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleStyleFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置简单符号";
            ((System.ComponentModel.ISupportInitialize)(this.pbStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceOutline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFill.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox teWidth;
        private System.Windows.Forms.TextBox teSize;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pbStyle;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ColorEdit ceOutline;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ColorEdit ceFill;
    }
}