namespace RCIS.DataInterface
{
    partial class Txt2ShpFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Txt2ShpFrm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.rbPoly = new System.Windows.Forms.RadioButton();
            this.rbLine = new System.Windows.Forms.RadioButton();
            this.rbPt = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.label2 = new System.Windows.Forms.Label();
            this.beSpatialTxt = new DevExpress.XtraEditors.ButtonEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.beSrcFile = new DevExpress.XtraEditors.ButtonEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.lblstatus = new DevExpress.XtraEditors.LabelControl();
            this.beDestFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtCotents = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beSpatialTxt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSrcFile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestFolder.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.rbPoly);
            this.panelControl1.Controls.Add(this.rbLine);
            this.panelControl1.Controls.Add(this.rbPt);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(452, 45);
            this.panelControl1.TabIndex = 0;
            // 
            // rbPoly
            // 
            this.rbPoly.AutoSize = true;
            this.rbPoly.Checked = true;
            this.rbPoly.Location = new System.Drawing.Point(101, 14);
            this.rbPoly.Name = "rbPoly";
            this.rbPoly.Size = new System.Drawing.Size(35, 16);
            this.rbPoly.TabIndex = 21;
            this.rbPoly.TabStop = true;
            this.rbPoly.Text = "面";
            this.rbPoly.UseVisualStyleBackColor = true;
            // 
            // rbLine
            // 
            this.rbLine.AutoSize = true;
            this.rbLine.Location = new System.Drawing.Point(60, 14);
            this.rbLine.Name = "rbLine";
            this.rbLine.Size = new System.Drawing.Size(35, 16);
            this.rbLine.TabIndex = 20;
            this.rbLine.Text = "线";
            this.rbLine.UseVisualStyleBackColor = true;
            // 
            // rbPt
            // 
            this.rbPt.AutoSize = true;
            this.rbPt.Location = new System.Drawing.Point(19, 14);
            this.rbPt.Name = "rbPt";
            this.rbPt.Size = new System.Drawing.Size(35, 16);
            this.rbPt.TabIndex = 19;
            this.rbPt.Text = "点";
            this.rbPt.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightGreen;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(238, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "XYZ坐标之间使用英文逗号 , 分割";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.label2);
            this.panelControl2.Controls.Add(this.beSpatialTxt);
            this.panelControl2.Controls.Add(this.label4);
            this.panelControl2.Controls.Add(this.beSrcFile);
            this.panelControl2.Controls.Add(this.label3);
            this.panelControl2.Controls.Add(this.lblstatus);
            this.panelControl2.Controls.Add(this.beDestFolder);
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 215);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(452, 153);
            this.panelControl2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "选择空间参考文件";
            // 
            // beSpatialTxt
            // 
            this.beSpatialTxt.Location = new System.Drawing.Point(166, 68);
            this.beSpatialTxt.Name = "beSpatialTxt";
            this.beSpatialTxt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beSpatialTxt.Size = new System.Drawing.Size(268, 20);
            this.beSpatialTxt.TabIndex = 28;
            this.beSpatialTxt.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSpatialTxt_ButtonClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "选择目标SHP文件";
            // 
            // beSrcFile
            // 
            this.beSrcFile.Location = new System.Drawing.Point(166, 14);
            this.beSrcFile.Name = "beSrcFile";
            this.beSrcFile.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beSrcFile.Size = new System.Drawing.Size(268, 20);
            this.beSrcFile.TabIndex = 26;
            this.beSrcFile.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beSrcFile_ButtonClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "选择一个坐标源文件";
            // 
            // lblstatus
            // 
            this.lblstatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblstatus.Location = new System.Drawing.Point(2, 137);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(0, 14);
            this.lblstatus.TabIndex = 23;
            // 
            // beDestFolder
            // 
            this.beDestFolder.Location = new System.Drawing.Point(166, 40);
            this.beDestFolder.Name = "beDestFolder";
            this.beDestFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beDestFolder.Size = new System.Drawing.Size(268, 20);
            this.beDestFolder.TabIndex = 22;
            this.beDestFolder.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beDestFolder_ButtonClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(359, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(269, 100);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "开始转化";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtCotents
            // 
            this.txtCotents.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.txtCotents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCotents.Location = new System.Drawing.Point(0, 45);
            this.txtCotents.Multiline = true;
            this.txtCotents.Name = "txtCotents";
            this.txtCotents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCotents.Size = new System.Drawing.Size(452, 170);
            this.txtCotents.TabIndex = 2;
            this.txtCotents.Text = resources.GetString("txtCotents.Text");
            // 
            // Txt2ShpFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 368);
            this.Controls.Add(this.txtCotents);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Txt2ShpFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Txt-->Shp";
            this.Load += new System.EventHandler(this.Txt2ShpFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.beSpatialTxt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beSrcFile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beDestFolder.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.RadioButton rbPoly;
        private System.Windows.Forms.RadioButton rbLine;
        private System.Windows.Forms.RadioButton rbPt;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private DevExpress.XtraEditors.LabelControl lblstatus;
        private DevExpress.XtraEditors.ButtonEdit beSrcFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCotents;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit beDestFolder;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit beSpatialTxt;
    }
}