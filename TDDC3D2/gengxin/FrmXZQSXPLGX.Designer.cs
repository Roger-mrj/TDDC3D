namespace TDDC3D.gengxin
{
    partial class FrmXZQSXPLGX
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
            this.btnEditBGXW = new DevExpress.XtraEditors.SimpleButton();
            this.btnXZQCheck = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectBHXZQ = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectBHXZQ.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEditBGXW
            // 
            this.btnEditBGXW.Location = new System.Drawing.Point(451, 143);
            this.btnEditBGXW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditBGXW.Name = "btnEditBGXW";
            this.btnEditBGXW.Size = new System.Drawing.Size(136, 48);
            this.btnEditBGXW.TabIndex = 9;
            this.btnEditBGXW.Text = "填写变更行为";
            this.btnEditBGXW.Click += new System.EventHandler(this.btnEditBGXW_Click);
            // 
            // btnXZQCheck
            // 
            this.btnXZQCheck.Location = new System.Drawing.Point(295, 143);
            this.btnXZQCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXZQCheck.Name = "btnXZQCheck";
            this.btnXZQCheck.Size = new System.Drawing.Size(136, 48);
            this.btnXZQCheck.TabIndex = 8;
            this.btnXZQCheck.Text = "行政区检查";
            this.btnXZQCheck.Click += new System.EventHandler(this.btnXZQCheck_Click);
            // 
            // btnSelectBHXZQ
            // 
            this.btnSelectBHXZQ.Location = new System.Drawing.Point(159, 53);
            this.btnSelectBHXZQ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectBHXZQ.Name = "btnSelectBHXZQ";
            this.btnSelectBHXZQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnSelectBHXZQ.Properties.ReadOnly = true;
            this.btnSelectBHXZQ.Size = new System.Drawing.Size(428, 28);
            this.btnSelectBHXZQ.TabIndex = 7;
            this.btnSelectBHXZQ.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnSelectBHXZQ_ButtonClick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(58, 56);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(108, 22);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "变化行政区：";
            // 
            // FrmXZQSXPLGX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 244);
            this.Controls.Add(this.btnEditBGXW);
            this.Controls.Add(this.btnXZQCheck);
            this.Controls.Add(this.btnSelectBHXZQ);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmXZQSXPLGX";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "行政区属性批量更新";
            ((System.ComponentModel.ISupportInitialize)(this.btnSelectBHXZQ.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnEditBGXW;
        private DevExpress.XtraEditors.SimpleButton btnXZQCheck;
        private DevExpress.XtraEditors.ButtonEdit btnSelectBHXZQ;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}