namespace TDDC3D.raster
{
    partial class NewMosaicDsForm
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
            this.beSprj = new DevExpress.XtraEditors.ButtonEdit();
            this.txtDatasetName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.beSprj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatasetName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beSprj
            // 
            this.beSprj.Location = new System.Drawing.Point(152, 62);
            this.beSprj.Name = "beSprj";
            this.beSprj.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beSprj.Size = new System.Drawing.Size(327, 24);
            this.beSprj.TabIndex = 7;
            this.beSprj.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSprj_ButtonClick);
            // 
            // txtDatasetName
            // 
            this.txtDatasetName.Location = new System.Drawing.Point(150, 22);
            this.txtDatasetName.Name = "txtDatasetName";
            this.txtDatasetName.Size = new System.Drawing.Size(329, 24);
            this.txtDatasetName.TabIndex = 6;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(17, 69);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(90, 18);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "选择空间参考";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 25);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 18);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "数据集名称";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(366, 92);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(113, 29);
            this.simpleButton3.TabIndex = 14;
            this.simpleButton3.Text = "导入空间参考";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(310, 148);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(90, 30);
            this.simpleButton2.TabIndex = 16;
            this.simpleButton2.Text = "取消";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(97, 148);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(90, 30);
            this.simpleButton1.TabIndex = 15;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // NewMosaicDsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 208);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.beSprj);
            this.Controls.Add(this.txtDatasetName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewMosaicDsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建镶嵌数据集";
            this.Load += new System.EventHandler(this.NewMosaicDsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.beSprj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatasetName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ButtonEdit beSprj;
        private DevExpress.XtraEditors.TextEdit txtDatasetName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}