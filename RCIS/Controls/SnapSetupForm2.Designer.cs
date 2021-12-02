namespace RCIS.Controls
{
    partial class SnapSetupForm2
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
            this.tvSnap = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvSnap
            // 
            this.tvSnap.CheckBoxes = true;
            this.tvSnap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSnap.Location = new System.Drawing.Point(0, 0);
            this.tvSnap.Name = "tvSnap";
            this.tvSnap.Size = new System.Drawing.Size(443, 438);
            this.tvSnap.TabIndex = 0;
            this.tvSnap.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvSnap_AfterCheck);
            // 
            // SnapSetupForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 438);
            this.Controls.Add(this.tvSnap);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SnapSetupForm2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "编辑捕捉设置";
            this.Load += new System.EventHandler(this.SnapSetupForm2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvSnap;

    }
}