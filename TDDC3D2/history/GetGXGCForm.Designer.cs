namespace TDDC3D.edit
{
    partial class GetGXGCForm
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
            this.checkedListBoxControl1 = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.memoLog = new DevExpress.XtraEditors.MemoEdit();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxControl1
            // 
            this.checkedListBoxControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkedListBoxControl1.Location = new System.Drawing.Point(2, 65);
            this.checkedListBoxControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkedListBoxControl1.Name = "checkedListBoxControl1";
            this.checkedListBoxControl1.Size = new System.Drawing.Size(454, 142);
            this.checkedListBoxControl1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.memoLog);
            this.groupControl1.Controls.Add(this.checkedListBoxControl1);
            this.groupControl1.Controls.Add(this.memoEdit1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(458, 438);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "选择已变更的历史";
            // 
            // memoLog
            // 
            this.memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoLog.Location = new System.Drawing.Point(2, 207);
            this.memoLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.memoLog.Name = "memoLog";
            this.memoLog.Size = new System.Drawing.Size(454, 229);
            this.memoLog.TabIndex = 1;
            this.memoLog.UseOptimizedRendering = true;
            // 
            // memoEdit1
            // 
            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Top;
            this.memoEdit1.EditValue = "      首先选择一个或多个变更历史时间点，通过开始提取进行更新过程层数据提取，然后根据更新过程层数据可以生成公顷制土地利用现状变更表。";
            this.memoEdit1.Location = new System.Drawing.Point(2, 22);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoEdit1.Properties.Appearance.Options.UseFont = true;
            this.memoEdit1.Size = new System.Drawing.Size(454, 43);
            this.memoEdit1.TabIndex = 2;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 438);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(458, 69);
            this.panelControl1.TabIndex = 2;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(171, 21);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(95, 30);
            this.simpleButton3.TabIndex = 2;
            this.simpleButton3.Text = "生成变更表";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(25, 21);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(80, 30);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "开始提取";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(326, 21);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(80, 30);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "关闭";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // GetGXGCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 507);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "GetGXGCForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取更新过程";
            this.Load += new System.EventHandler(this.GetGXGCForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoLog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
    }
}