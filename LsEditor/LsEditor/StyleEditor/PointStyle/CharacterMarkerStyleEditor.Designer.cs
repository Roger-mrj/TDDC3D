namespace RCIS.Style.StyleEditor
{
    partial class CharacterMarkerStyleEditor
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
            this.btnLoadAll = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.ceColor = new DevExpress.XtraEditors.ColorEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.spinAngle = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.spinSize = new DevExpress.XtraEditors.SpinEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.spinOffsetY = new DevExpress.XtraEditors.SpinEdit();
            this.spinOffsetX = new DevExpress.XtraEditors.SpinEdit();
            this.teCharInt = new DevExpress.XtraEditors.TextEdit();
            this.gridChar = new System.Windows.Forms.DataGridView();
            this.ceSubSet = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceFontFamily = new DevExpress.XtraEditors.ComboBoxEdit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCharInt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridChar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSubSet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontFamily.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(593, 440);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnLoadAll);
            this.tabPage1.Controls.Add(this.btnNextPage);
            this.tabPage1.Controls.Add(this.groupControl4);
            this.tabPage1.Controls.Add(this.groupControl3);
            this.tabPage1.Controls.Add(this.groupControl2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupControl1);
            this.tabPage1.Controls.Add(this.teCharInt);
            this.tabPage1.Controls.Add(this.gridChar);
            this.tabPage1.Controls.Add(this.ceSubSet);
            this.tabPage1.Controls.Add(this.ceFontFamily);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(585, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "字符点状符号";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLoadAll
            // 
            this.btnLoadAll.Location = new System.Drawing.Point(386, 348);
            this.btnLoadAll.Name = "btnLoadAll";
            this.btnLoadAll.Size = new System.Drawing.Size(47, 23);
            this.btnLoadAll.TabIndex = 18;
            this.btnLoadAll.Text = ">>|";
            this.btnLoadAll.UseVisualStyleBackColor = true;
            this.btnLoadAll.Click += new System.EventHandler(this.btnLoadAll_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Location = new System.Drawing.Point(333, 348);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(47, 23);
            this.btnNextPage.TabIndex = 17;
            this.btnNextPage.Text = ">";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.ceColor);
            this.groupControl4.Location = new System.Drawing.Point(438, 198);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(141, 47);
            this.groupControl4.TabIndex = 16;
            this.groupControl4.Text = "字符颜色";
            // 
            // ceColor
            // 
            this.ceColor.EditValue = System.Drawing.Color.Empty;
            this.ceColor.Location = new System.Drawing.Point(5, 24);
            this.ceColor.Name = "ceColor";
            this.ceColor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceColor.Size = new System.Drawing.Size(131, 21);
            this.ceColor.TabIndex = 6;
            this.ceColor.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.spinAngle);
            this.groupControl3.Location = new System.Drawing.Point(438, 136);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(141, 51);
            this.groupControl3.TabIndex = 15;
            this.groupControl3.Text = "倾斜角度";
            // 
            // spinAngle
            // 
            this.spinAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinAngle.Location = new System.Drawing.Point(5, 24);
            this.spinAngle.Name = "spinAngle";
            this.spinAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinAngle.Size = new System.Drawing.Size(132, 21);
            this.spinAngle.TabIndex = 5;
            this.spinAngle.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.spinSize);
            this.groupControl2.Location = new System.Drawing.Point(438, 75);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(141, 49);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "字符大小";
            // 
            // spinSize
            // 
            this.spinSize.EditValue = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.spinSize.Location = new System.Drawing.Point(6, 23);
            this.spinSize.Name = "spinSize";
            this.spinSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinSize.Size = new System.Drawing.Size(131, 21);
            this.spinSize.TabIndex = 4;
            this.spinSize.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(65, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "字符代码";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "字符集";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "字体";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label5);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.spinOffsetY);
            this.groupControl1.Controls.Add(this.spinOffsetX);
            this.groupControl1.Location = new System.Drawing.Point(438, 258);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(137, 84);
            this.groupControl1.TabIndex = 7;
            this.groupControl1.Text = "偏移";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(5, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Y:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "X:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spinOffsetY
            // 
            this.spinOffsetY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetY.Location = new System.Drawing.Point(34, 51);
            this.spinOffsetY.Name = "spinOffsetY";
            this.spinOffsetY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetY.Size = new System.Drawing.Size(98, 21);
            this.spinOffsetY.TabIndex = 7;
            this.spinOffsetY.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // spinOffsetX
            // 
            this.spinOffsetX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOffsetX.Location = new System.Drawing.Point(34, 24);
            this.spinOffsetX.Name = "spinOffsetX";
            this.spinOffsetX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spinOffsetX.Size = new System.Drawing.Size(98, 21);
            this.spinOffsetX.TabIndex = 6;
            this.spinOffsetX.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // teCharInt
            // 
            this.teCharInt.Location = new System.Drawing.Point(132, 348);
            this.teCharInt.Name = "teCharInt";
            this.teCharInt.Size = new System.Drawing.Size(97, 21);
            this.teCharInt.TabIndex = 3;
            this.teCharInt.EditValueChanged += new System.EventHandler(this.OnEditedStylePropertyChanged);
            // 
            // gridChar
            // 
            this.gridChar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridChar.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridChar.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gridChar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridChar.ColumnHeadersVisible = false;
            this.gridChar.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridChar.Location = new System.Drawing.Point(67, 75);
            this.gridChar.MultiSelect = false;
            this.gridChar.Name = "gridChar";
            this.gridChar.ReadOnly = true;
            this.gridChar.RowHeadersVisible = false;
            this.gridChar.RowTemplate.Height = 32;
            this.gridChar.RowTemplate.ReadOnly = true;
            this.gridChar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridChar.Size = new System.Drawing.Size(365, 267);
            this.gridChar.TabIndex = 2;
            this.gridChar.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCharClicked);
            // 
            // ceSubSet
            // 
            this.ceSubSet.Location = new System.Drawing.Point(67, 48);
            this.ceSubSet.Name = "ceSubSet";
            this.ceSubSet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceSubSet.Size = new System.Drawing.Size(365, 21);
            this.ceSubSet.TabIndex = 1;
            // 
            // ceFontFamily
            // 
            this.ceFontFamily.Location = new System.Drawing.Point(67, 21);
            this.ceFontFamily.Name = "ceFontFamily";
            this.ceFontFamily.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceFontFamily.Size = new System.Drawing.Size(365, 21);
            this.ceFontFamily.TabIndex = 0;
            this.ceFontFamily.SelectedIndexChanged += new System.EventHandler(this.ceFontFamily_SelectedIndexChanged);
            // 
            // CharacterMarkerStyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CharacterMarkerStyleEditor";
            this.Size = new System.Drawing.Size(593, 440);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ceColor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teCharInt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridChar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceSubSet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceFontFamily.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private DevExpress.XtraEditors.ComboBoxEdit ceSubSet;
        private DevExpress.XtraEditors.ComboBoxEdit ceFontFamily;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SpinEdit spinOffsetY;
        private DevExpress.XtraEditors.SpinEdit spinOffsetX;
        private DevExpress.XtraEditors.ColorEdit ceColor;
        private DevExpress.XtraEditors.SpinEdit spinAngle;
        private DevExpress.XtraEditors.SpinEdit spinSize;
        private DevExpress.XtraEditors.TextEdit teCharInt;
        private System.Windows.Forms.DataGridView gridChar;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLoadAll;
        private System.Windows.Forms.Button btnNextPage;

    }
}
