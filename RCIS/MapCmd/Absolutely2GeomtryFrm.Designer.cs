namespace RCIS.MapTool
{
    partial class Absolutely2GeomtryFrm
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
            this.rbPoly = new System.Windows.Forms.RadioButton();
            this.rbLine = new System.Windows.Forms.RadioButton();
            this.rbPt = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbPtList = new System.Windows.Forms.RichTextBox();
            this.cbHasPtNO = new System.Windows.Forms.CheckBox();
            this.cbHasZ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rbPoly
            // 
            this.rbPoly.AutoSize = true;
            this.rbPoly.Location = new System.Drawing.Point(101, 11);
            this.rbPoly.Name = "rbPoly";
            this.rbPoly.Size = new System.Drawing.Size(35, 16);
            this.rbPoly.TabIndex = 15;
            this.rbPoly.Text = "面";
            this.rbPoly.UseVisualStyleBackColor = true;
            // 
            // rbLine
            // 
            this.rbLine.AutoSize = true;
            this.rbLine.Checked = true;
            this.rbLine.Location = new System.Drawing.Point(60, 11);
            this.rbLine.Name = "rbLine";
            this.rbLine.Size = new System.Drawing.Size(35, 16);
            this.rbLine.TabIndex = 14;
            this.rbLine.TabStop = true;
            this.rbLine.Text = "线";
            this.rbLine.UseVisualStyleBackColor = true;
            // 
            // rbPt
            // 
            this.rbPt.AutoSize = true;
            this.rbPt.Location = new System.Drawing.Point(19, 11);
            this.rbPt.Name = "rbPt";
            this.rbPt.Size = new System.Drawing.Size(35, 16);
            this.rbPt.TabIndex = 13;
            this.rbPt.Text = "点";
            this.rbPt.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightGreen;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(217, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "XYZ坐标之间使用英文逗号 , 分割";
            // 
            // btnReadFile
            // 
            this.btnReadFile.Location = new System.Drawing.Point(343, 279);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(75, 23);
            this.btnReadFile.TabIndex = 11;
            this.btnReadFile.Text = "从文件读取";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(344, 312);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(254, 312);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbPtList
            // 
            this.tbPtList.Location = new System.Drawing.Point(16, 32);
            this.tbPtList.Name = "tbPtList";
            this.tbPtList.Size = new System.Drawing.Size(403, 244);
            this.tbPtList.TabIndex = 8;
            this.tbPtList.Text = "";
            // 
            // cbHasPtNO
            // 
            this.cbHasPtNO.AutoSize = true;
            this.cbHasPtNO.Location = new System.Drawing.Point(19, 286);
            this.cbHasPtNO.Name = "cbHasPtNO";
            this.cbHasPtNO.Size = new System.Drawing.Size(144, 16);
            this.cbHasPtNO.TabIndex = 16;
            this.cbHasPtNO.Text = "点数据文件中包含点号";
            this.cbHasPtNO.UseVisualStyleBackColor = true;
            // 
            // cbHasZ
            // 
            this.cbHasZ.AutoSize = true;
            this.cbHasZ.Location = new System.Drawing.Point(145, 12);
            this.cbHasZ.Name = "cbHasZ";
            this.cbHasZ.Size = new System.Drawing.Size(66, 16);
            this.cbHasZ.TabIndex = 17;
            this.cbHasZ.Text = "包含Z值";
            this.cbHasZ.UseVisualStyleBackColor = true;
            // 
            // Absolutely2GeomtryFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 345);
            this.Controls.Add(this.cbHasZ);
            this.Controls.Add(this.cbHasPtNO);
            this.Controls.Add(this.rbPoly);
            this.Controls.Add(this.rbLine);
            this.Controls.Add(this.rbPt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReadFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbPtList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Absolutely2GeomtryFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "读入绝对坐标生成图形";
            this.Load += new System.EventHandler(this.Absolutely2GeomtryFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbPoly;
        private System.Windows.Forms.RadioButton rbLine;
        private System.Windows.Forms.RadioButton rbPt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RichTextBox tbPtList;
        private System.Windows.Forms.CheckBox cbHasPtNO;
        private System.Windows.Forms.CheckBox cbHasZ;
    }
}