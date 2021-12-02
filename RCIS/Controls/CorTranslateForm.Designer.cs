namespace ElseTransform
{
    partial class CorTranslateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorTranslateForm));
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnHelp = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnCalParmeters = new System.Windows.Forms.Button();
            this.btnGetKZDFile = new System.Windows.Forms.Button();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Tips = new System.Windows.Forms.GroupBox();
            this.TargetFC = new System.Windows.Forms.Label();
            this.SourceFC = new System.Windows.Forms.Label();
            this.btntransformLayer = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.Tips.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(350, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(135, 21);
            this.textBox2.TabIndex = 9;
            this.textBox2.Text = "textBox2";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(254, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "SHAPE文件名:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 60);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(232, 22);
            this.progressBar1.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(579, 291);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnHelp);
            this.tabPage1.Controls.Add(this.comboBox2);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.btnCalParmeters);
            this.tabPage1.Controls.Add(this.btnGetKZDFile);
            this.tabPage1.Controls.Add(this.dataGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(571, 265);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "计算参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHelp.ForeColor = System.Drawing.Color.Red;
            this.btnHelp.Location = new System.Drawing.Point(7, 216);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(23, 23);
            this.btnHelp.TabIndex = 25;
            this.btnHelp.Text = "?";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.DropDownWidth = 180;
            this.comboBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.ForeColor = System.Drawing.Color.Blue;
            this.comboBox2.Location = new System.Drawing.Point(351, 217);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(119, 21);
            this.comboBox2.TabIndex = 24;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(478, 214);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(73, 25);
            this.button4.TabIndex = 23;
            this.button4.Text = "转换文件";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.DropDownWidth = 125;
            this.comboBox1.ForeColor = System.Drawing.Color.Blue;
            this.comboBox1.Items.AddRange(new object[] {
            "二阶仿射[12参数]",
            "一阶仿射[6参数]"});
            this.comboBox1.Location = new System.Drawing.Point(174, 217);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(105, 20);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 22;
            // 
            // btnCalParmeters
            // 
            this.btnCalParmeters.Location = new System.Drawing.Point(286, 215);
            this.btnCalParmeters.Name = "btnCalParmeters";
            this.btnCalParmeters.Size = new System.Drawing.Size(63, 24);
            this.btnCalParmeters.TabIndex = 21;
            this.btnCalParmeters.Text = "计算参数";
            this.btnCalParmeters.Click += new System.EventHandler(this.btnCalParmeters_Click_1);
            // 
            // btnGetKZDFile
            // 
            this.btnGetKZDFile.Location = new System.Drawing.Point(35, 216);
            this.btnGetKZDFile.Name = "btnGetKZDFile";
            this.btnGetKZDFile.Size = new System.Drawing.Size(138, 24);
            this.btnGetKZDFile.TabIndex = 20;
            this.btnGetKZDFile.Text = "装入控制点对坐标文件";
            this.btnGetKZDFile.Click += new System.EventHandler(this.btnGetKZDFile_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid1.CaptionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dataGrid1.CaptionText = "点对坐标显示";
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(6, 6);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dataGrid1.Size = new System.Drawing.Size(547, 205);
            this.dataGrid1.TabIndex = 19;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Tips);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.btntransformLayer);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(571, 265);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "转换图层";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Tips
            // 
            this.Tips.Controls.Add(this.progressBar1);
            this.Tips.Controls.Add(this.TargetFC);
            this.Tips.Controls.Add(this.SourceFC);
            this.Tips.Location = new System.Drawing.Point(249, 146);
            this.Tips.Name = "Tips";
            this.Tips.Size = new System.Drawing.Size(267, 93);
            this.Tips.TabIndex = 10;
            this.Tips.TabStop = false;
            this.Tips.Text = "运行中...";
            this.Tips.Visible = false;
            // 
            // TargetFC
            // 
            this.TargetFC.Location = new System.Drawing.Point(23, 38);
            this.TargetFC.Name = "TargetFC";
            this.TargetFC.Size = new System.Drawing.Size(222, 13);
            this.TargetFC.TabIndex = 1;
            this.TargetFC.Text = "目标要素类:";
            // 
            // SourceFC
            // 
            this.SourceFC.Location = new System.Drawing.Point(23, 19);
            this.SourceFC.Name = "SourceFC";
            this.SourceFC.Size = new System.Drawing.Size(222, 13);
            this.SourceFC.TabIndex = 0;
            this.SourceFC.Text = "源要素类:";
            // 
            // btntransformLayer
            // 
            this.btntransformLayer.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btntransformLayer.Location = new System.Drawing.Point(248, 113);
            this.btntransformLayer.Name = "btntransformLayer";
            this.btntransformLayer.Size = new System.Drawing.Size(265, 25);
            this.btntransformLayer.TabIndex = 6;
            this.btntransformLayer.Text = "对选中图层执行坐标转换";
            this.btntransformLayer.Click += new System.EventHandler(this.btntransformLayer_Click);
            // 
            // button5
            // 
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.Location = new System.Drawing.Point(487, 41);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(26, 23);
            this.button5.TabIndex = 5;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(269, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(217, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "textBox1";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(244, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "[03] 输出路径:";
            // 
            // listBox1
            // 
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(18, 101);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(204, 124);
            this.listBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "[02] 图层列表:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(13, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "[01] 待转换图层类型: ";
            // 
            // radioButton3
            // 
            this.radioButton3.Location = new System.Drawing.Point(144, 26);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(40, 18);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "面";
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Location = new System.Drawing.Point(83, 26);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(40, 18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "线";
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.Location = new System.Drawing.Point(19, 26);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(40, 18);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "点";
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // CorTranslateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 291);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CorTranslateForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "坐标变换";
            this.Load += new System.EventHandler(this.CorTranslateForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.Tips.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnCalParmeters;
        private System.Windows.Forms.Button btnGetKZDFile;
        public System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox Tips;
        private System.Windows.Forms.Label TargetFC;
        private System.Windows.Forms.Label SourceFC;
        private System.Windows.Forms.Button btntransformLayer;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}