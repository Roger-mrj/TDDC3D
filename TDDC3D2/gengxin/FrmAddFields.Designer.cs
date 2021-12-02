namespace TDDC3D.gengxin
{
    partial class FrmAddFields
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkListFields = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.chkAll = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.txtSourceLayer = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkListFields)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceLayer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "选择源图层：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(26, 66);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "选择字段：";
            // 
            // chkListFields
            // 
            this.chkListFields.Location = new System.Drawing.Point(26, 90);
            this.chkListFields.Name = "chkListFields";
            this.chkListFields.Size = new System.Drawing.Size(354, 238);
            this.chkListFields.TabIndex = 3;
            // 
            // simpleButton1
            // 
            this.simpleButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.simpleButton1.Location = new System.Drawing.Point(192, 334);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(83, 41);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(26, 344);
            this.chkAll.Name = "chkAll";
            this.chkAll.Properties.Caption = "全选/全不选";
            this.chkAll.Size = new System.Drawing.Size(124, 22);
            this.chkAll.TabIndex = 7;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(297, 334);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(83, 41);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // txtSourceLayer
            // 
            this.txtSourceLayer.Location = new System.Drawing.Point(26, 36);
            this.txtSourceLayer.Name = "txtSourceLayer";
            this.txtSourceLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSourceLayer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtSourceLayer.Size = new System.Drawing.Size(354, 24);
            this.txtSourceLayer.TabIndex = 9;
            this.txtSourceLayer.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtSourceLayer_ButtonClick);
            // 
            // FrmAddFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 394);
            this.Controls.Add(this.txtSourceLayer);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.chkListFields);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddFields";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量追加字段";
            this.Load += new System.EventHandler(this.FrmAddFields_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkListFields)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSourceLayer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListFields;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckEdit chkAll;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.ButtonEdit txtSourceLayer;
    }
}