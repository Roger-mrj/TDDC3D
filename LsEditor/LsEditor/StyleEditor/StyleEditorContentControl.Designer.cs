namespace RCIS.Style.StyleEditor
{
    partial class StyleEditorContentControl
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
            this.gbHeader = new System.Windows.Forms.GroupBox();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.lbUnit = new System.Windows.Forms.Label();
            this.cbEditor = new System.Windows.Forms.ComboBox();
            this.lbStyleType = new System.Windows.Forms.Label();
            this.plContent = new System.Windows.Forms.Panel();
            this.gbHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.simpleButton1);
            this.gbHeader.Controls.Add(this.cbUnit);
            this.gbHeader.Controls.Add(this.lbUnit);
            this.gbHeader.Controls.Add(this.cbEditor);
            this.gbHeader.Controls.Add(this.lbStyleType);
            this.gbHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbHeader.Location = new System.Drawing.Point(0, 0);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(512, 40);
            this.gbHeader.TabIndex = 0;
            this.gbHeader.TabStop = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.simpleButton1.Location = new System.Drawing.Point(443, 17);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(66, 20);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "类别帮助";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cbUnit
            // 
            this.cbUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Location = new System.Drawing.Point(308, 14);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(126, 20);
            this.cbUnit.TabIndex = 3;
            this.cbUnit.Text = "象素";
            // 
            // lbUnit
            // 
            this.lbUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUnit.Location = new System.Drawing.Point(271, 14);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(31, 23);
            this.lbUnit.TabIndex = 2;
            this.lbUnit.Text = "单位";
            this.lbUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbEditor
            // 
            this.cbEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEditor.FormattingEnabled = true;
            this.cbEditor.Location = new System.Drawing.Point(58, 14);
            this.cbEditor.Name = "cbEditor";
            this.cbEditor.Size = new System.Drawing.Size(207, 20);
            this.cbEditor.TabIndex = 1;
            this.cbEditor.SelectedIndexChanged += new System.EventHandler(this.cbEditor_SelectedIndexChanged);
            // 
            // lbStyleType
            // 
            this.lbStyleType.Location = new System.Drawing.Point(3, 14);
            this.lbStyleType.Name = "lbStyleType";
            this.lbStyleType.Size = new System.Drawing.Size(61, 23);
            this.lbStyleType.TabIndex = 0;
            this.lbStyleType.Text = "符号类型";
            this.lbStyleType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // plContent
            // 
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Location = new System.Drawing.Point(0, 40);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(512, 367);
            this.plContent.TabIndex = 2;
            // 
            // StyleEditorContentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.gbHeader);
            this.Name = "StyleEditorContentControl";
            this.Size = new System.Drawing.Size(512, 407);
            this.gbHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbHeader;
        private System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.ComboBox cbEditor;
        private System.Windows.Forms.Label lbStyleType;
        private System.Windows.Forms.ComboBox cbUnit;
        private System.Windows.Forms.Label lbUnit;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}

