namespace TDDC3D.output
{
    partial class FrmOutRsTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOutRsTable));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtShp = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbLay = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl35 = new DevExpress.XtraEditors.LabelControl();
            this.txtS = new DevExpress.XtraEditors.TextEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.txtPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnNo = new DevExpress.XtraEditors.SimpleButton();
            this.btnOut = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtShp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(37, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(135, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "遥感监测图斑数据：";
            // 
            // txtShp
            // 
            this.txtShp.Location = new System.Drawing.Point(188, 30);
            this.txtShp.Name = "txtShp";
            this.txtShp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtShp.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtShp.Size = new System.Drawing.Size(547, 24);
            this.txtShp.TabIndex = 1;
            this.txtShp.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtShp_ButtonClick);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(67, 90);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(105, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "选择图层数据：";
            // 
            // cmbLay
            // 
            this.cmbLay.Location = new System.Drawing.Point(188, 87);
            this.cmbLay.Name = "cmbLay";
            this.cmbLay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbLay.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbLay.Size = new System.Drawing.Size(547, 24);
            this.cmbLay.TabIndex = 3;
            this.cmbLay.SelectedIndexChanged += new System.EventHandler(this.cmbLay_SelectedIndexChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(720, 143);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(15, 18);
            this.labelControl4.TabIndex = 59;
            this.labelControl4.Text = "%";
            // 
            // txtP
            // 
            this.txtP.EditValue = "50";
            this.txtP.Location = new System.Drawing.Point(643, 140);
            this.txtP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtP.Name = "txtP";
            this.txtP.Properties.Mask.EditMask = "[0-9]*";
            this.txtP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtP.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtP.Properties.MaxLength = 2;
            this.txtP.Size = new System.Drawing.Size(71, 24);
            this.txtP.TabIndex = 58;
            // 
            // labelControl35
            // 
            this.labelControl35.Location = new System.Drawing.Point(386, 143);
            this.labelControl35.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl35.Name = "labelControl35";
            this.labelControl35.Size = new System.Drawing.Size(251, 18);
            this.labelControl35.TabIndex = 57;
            this.labelControl35.Text = "平方米，或者重叠部分占自身面积比>";
            // 
            // txtS
            // 
            this.txtS.EditValue = "50";
            this.txtS.Location = new System.Drawing.Point(309, 140);
            this.txtS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtS.Name = "txtS";
            this.txtS.Properties.Mask.EditMask = "[0-9]*";
            this.txtS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtS.Properties.MaxLength = 6;
            this.txtS.Size = new System.Drawing.Size(71, 24);
            this.txtS.TabIndex = 56;
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(189, 143);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(109, 18);
            this.labelControl21.TabIndex = 55;
            this.labelControl21.Text = "提取叠加面积S>";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(188, 199);
            this.txtPath.Name = "txtPath";
            this.txtPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtPath.Size = new System.Drawing.Size(547, 24);
            this.txtPath.TabIndex = 61;
            this.txtPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtPath_ButtonClick);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(97, 202);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 18);
            this.labelControl3.TabIndex = 60;
            this.labelControl3.Text = "输出路径：";
            // 
            // btnNo
            // 
            this.btnNo.Image = ((System.Drawing.Image)(resources.GetObject("btnNo.Image")));
            this.btnNo.Location = new System.Drawing.Point(642, 257);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(93, 40);
            this.btnNo.TabIndex = 63;
            this.btnNo.Text = "取消";
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnOut
            // 
            this.btnOut.Image = ((System.Drawing.Image)(resources.GetObject("btnOut.Image")));
            this.btnOut.Location = new System.Drawing.Point(523, 257);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(93, 40);
            this.btnOut.TabIndex = 62;
            this.btnOut.Text = "输出";
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // FrmOutRsTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 320);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtP);
            this.Controls.Add(this.labelControl35);
            this.Controls.Add(this.txtS);
            this.Controls.Add(this.labelControl21);
            this.Controls.Add(this.cmbLay);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtShp);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOutRsTable";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输出遥感监测图斑信息记录核实表";
            this.Load += new System.EventHandler(this.FrmOutRsTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtShp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit txtShp;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbLay;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtP;
        private DevExpress.XtraEditors.LabelControl labelControl35;
        private DevExpress.XtraEditors.TextEdit txtS;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.ButtonEdit txtPath;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnOut;
        private DevExpress.XtraEditors.SimpleButton btnNo;
    }
}