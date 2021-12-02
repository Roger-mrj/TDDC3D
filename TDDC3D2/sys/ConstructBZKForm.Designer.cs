namespace TDDC3D.sys
{
    partial class ConstructBZKForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstructBZKForm));
            this.chkOpen = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.cmbCoordSelect = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnSelectCoord = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.txtTolerance = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.beStandFile = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOpen.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCoordSelect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTolerance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beStandFile.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkOpen
            // 
            this.chkOpen.EditValue = true;
            this.chkOpen.Location = new System.Drawing.Point(24, 409);
            this.chkOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Properties.Caption = "创建后打开标准库";
            this.chkOpen.Size = new System.Drawing.Size(188, 22);
            this.chkOpen.TabIndex = 16;
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton2.Image")));
            this.simpleButton2.Location = new System.Drawing.Point(400, 412);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(101, 29);
            this.simpleButton2.TabIndex = 11;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(291, 412);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(101, 29);
            this.simpleButton1.TabIndex = 10;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // cmbCoordSelect
            // 
            this.cmbCoordSelect.Location = new System.Drawing.Point(99, 49);
            this.cmbCoordSelect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbCoordSelect.Name = "cmbCoordSelect";
            this.cmbCoordSelect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbCoordSelect.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbCoordSelect.Size = new System.Drawing.Size(353, 24);
            this.cmbCoordSelect.TabIndex = 28;
            this.cmbCoordSelect.SelectedIndexChanged += new System.EventHandler(this.cmbCoordSelect_SelectedIndexChanged);
            // 
            // btnSelectCoord
            // 
            this.btnSelectCoord.ImageIndex = 0;
            this.btnSelectCoord.ImageList = this.imageCollection1;
            this.btnSelectCoord.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSelectCoord.Location = new System.Drawing.Point(460, 49);
            this.btnSelectCoord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectCoord.Name = "btnSelectCoord";
            this.btnSelectCoord.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btnSelectCoord.Size = new System.Drawing.Size(28, 25);
            this.btnSelectCoord.TabIndex = 27;
            this.btnSelectCoord.Click += new System.EventHandler(this.btnSelectCoord_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "coord.png");
            // 
            // txtTolerance
            // 
            this.txtTolerance.EditValue = "0.001";
            this.txtTolerance.Location = new System.Drawing.Point(99, 84);
            this.txtTolerance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTolerance.Name = "txtTolerance";
            this.txtTolerance.Properties.Mask.EditMask = "([0-9]{1}[.][0-9]*)";
            this.txtTolerance.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtTolerance.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTolerance.Size = new System.Drawing.Size(396, 24);
            this.txtTolerance.TabIndex = 26;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(24, 84);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 18);
            this.labelControl3.TabIndex = 25;
            this.labelControl3.Text = "数据容差：";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(24, 114);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(125, 29);
            this.simpleButton3.TabIndex = 24;
            this.simpleButton3.Text = "导入空间参考";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // memoEdit1
            // 
            this.memoEdit1.Enabled = false;
            this.memoEdit1.Location = new System.Drawing.Point(24, 148);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.AllowFocused = false;
            this.memoEdit1.Size = new System.Drawing.Size(477, 241);
            this.memoEdit1.TabIndex = 23;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(24, 52);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 22;
            this.labelControl2.Text = "空间参考：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(41, 16);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(50, 18);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "标准库:";
            // 
            // beStandFile
            // 
            this.beStandFile.Location = new System.Drawing.Point(100, 12);
            this.beStandFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.beStandFile.Name = "beStandFile";
            this.beStandFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beStandFile.Size = new System.Drawing.Size(396, 24);
            this.beStandFile.TabIndex = 20;
            this.beStandFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beStandFile_ButtonClick);
            // 
            // ConstructBZKForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 446);
            this.Controls.Add(this.cmbCoordSelect);
            this.Controls.Add(this.btnSelectCoord);
            this.Controls.Add(this.txtTolerance);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.beStandFile);
            this.Controls.Add(this.chkOpen);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConstructBZKForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建标准库";
            this.Load += new System.EventHandler(this.ConstructBZKForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkOpen.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCoordSelect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTolerance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beStandFile.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckEdit chkOpen;
        private DevExpress.XtraEditors.ComboBoxEdit cmbCoordSelect;
        private DevExpress.XtraEditors.SimpleButton btnSelectCoord;
        private DevExpress.XtraEditors.TextEdit txtTolerance;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit beStandFile;
        private DevExpress.Utils.ImageCollection imageCollection1;
    }
}