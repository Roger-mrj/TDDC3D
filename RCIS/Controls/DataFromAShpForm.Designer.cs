namespace RCIS.Controls
{
    partial class DataFromAShpForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.status = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmdCancel = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cmbLocalFeatureclasses = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btSounceFC = new DevExpress.XtraEditors.ButtonEdit();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.gridSrcFields = new System.Windows.Forms.DataGridView();
            this.ColumnSrcFields = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gridDestFields = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.选择要素类 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btSounceFC.Properties)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSrcFields)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDestFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.status);
            this.groupBox1.Controls.Add(this.simpleButton1);
            this.groupBox1.Controls.Add(this.cmdCancel);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(833, 546);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.status.Location = new System.Drawing.Point(4, 527);
            this.status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(31, 15);
            this.status.TabIndex = 5;
            this.status.Text = "...";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(568, 485);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(100, 29);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "转换";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(676, 485);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(100, 29);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(4, 22);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(825, 444);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 22);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(817, 418);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(809, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本转换设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.groupBox8);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(4, 4);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(801, 381);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(307, 345);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(382, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "注意：如果目标转换为已存在的要素类，请进行字段匹配";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.cmbLocalFeatureclasses);
            this.groupBox8.Location = new System.Drawing.Point(11, 162);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox8.Size = new System.Drawing.Size(784, 116);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "目标要素类";
            // 
            // cmbLocalFeatureclasses
            // 
            this.cmbLocalFeatureclasses.Location = new System.Drawing.Point(4, 50);
            this.cmbLocalFeatureclasses.Margin = new System.Windows.Forms.Padding(4);
            this.cmbLocalFeatureclasses.Name = "cmbLocalFeatureclasses";
            this.cmbLocalFeatureclasses.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLocalFeatureclasses.Size = new System.Drawing.Size(776, 24);
            this.cmbLocalFeatureclasses.TabIndex = 4;
            this.cmbLocalFeatureclasses.SelectedIndexChanged += new System.EventHandler(this.cmbLocalFeatureclasses_SelectedIndexChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.选择要素类);
            this.groupBox7.Controls.Add(this.btSounceFC);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Location = new System.Drawing.Point(4, 22);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox7.Size = new System.Drawing.Size(793, 112);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "源要素类";
            // 
            // btSounceFC
            // 
            this.btSounceFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btSounceFC.Location = new System.Drawing.Point(4, 22);
            this.btSounceFC.Margin = new System.Windows.Forms.Padding(4);
            this.btSounceFC.Name = "btSounceFC";
            this.btSounceFC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btSounceFC.Properties.ReadOnly = true;
            this.btSounceFC.Size = new System.Drawing.Size(785, 24);
            this.btSounceFC.TabIndex = 1;
            this.btSounceFC.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btSounceFC_ButtonClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(809, 389);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "字段匹配";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(4, 4);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(801, 381);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.gridSrcFields);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(383, 22);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox6.Size = new System.Drawing.Size(414, 355);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "源要素类字段名";
            // 
            // gridSrcFields
            // 
            this.gridSrcFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridSrcFields.BackgroundColor = System.Drawing.Color.White;
            this.gridSrcFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSrcFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSrcFields});
            this.gridSrcFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSrcFields.Location = new System.Drawing.Point(4, 22);
            this.gridSrcFields.Margin = new System.Windows.Forms.Padding(4);
            this.gridSrcFields.Name = "gridSrcFields";
            this.gridSrcFields.RowTemplate.Height = 23;
            this.gridSrcFields.Size = new System.Drawing.Size(406, 329);
            this.gridSrcFields.TabIndex = 1;
            this.gridSrcFields.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView2_Scroll);
            // 
            // ColumnSrcFields
            // 
            this.ColumnSrcFields.HeaderText = "字段名";
            this.ColumnSrcFields.Name = "ColumnSrcFields";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.gridDestFields);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(4, 22);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(379, 355);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "目标要素类字段名";
            // 
            // gridDestFields
            // 
            this.gridDestFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridDestFields.BackgroundColor = System.Drawing.Color.White;
            this.gridDestFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDestFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.gridDestFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDestFields.Location = new System.Drawing.Point(4, 22);
            this.gridDestFields.Margin = new System.Windows.Forms.Padding(4);
            this.gridDestFields.Name = "gridDestFields";
            this.gridDestFields.RowTemplate.Height = 23;
            this.gridDestFields.Size = new System.Drawing.Size(371, 329);
            this.gridDestFields.TabIndex = 0;
            this.gridDestFields.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "字段名";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "字段类型";
            this.Column2.Name = "Column2";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(400, 375);
            this.xtraTabControl1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // 选择要素类
            // 
            this.选择要素类.Location = new System.Drawing.Point(548, 64);
            this.选择要素类.Margin = new System.Windows.Forms.Padding(4);
            this.选择要素类.Name = "选择要素类";
            this.选择要素类.Size = new System.Drawing.Size(208, 29);
            this.选择要素类.TabIndex = 5;
            this.选择要素类.Text = "选择源要素类";
            this.选择要素类.Click += new System.EventHandler(this.选择要素类_Click);
            // 
            // DataFromAShpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 546);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.xtraTabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DataFromAShpForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "导入单个要素类";
            this.Load += new System.EventHandler(this.DataFromAShpForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbLocalFeatureclasses.Properties)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btSounceFC.Properties)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSrcFields)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDestFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label status;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton cmdCancel;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox8;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLocalFeatureclasses;
        private System.Windows.Forms.GroupBox groupBox7;
        private DevExpress.XtraEditors.ButtonEdit btSounceFC;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView gridSrcFields;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView gridDestFields;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnSrcFields;
        private DevExpress.XtraEditors.SimpleButton 选择要素类;
    }
}