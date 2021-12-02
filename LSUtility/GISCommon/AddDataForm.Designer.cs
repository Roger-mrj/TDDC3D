namespace RCIS.GISCommon
{
    partial class AddDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDataForm));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.trwFileExplorer = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton2);
            this.groupControl1.Controls.Add(this.btnOk);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 355);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(403, 106);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "工具栏";
            // 
            // simpleButton2
            // 
            this.simpleButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButton2.Location = new System.Drawing.Point(240, 51);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(90, 33);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "取消";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(79, 51);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 33);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // trwFileExplorer
            // 
            this.trwFileExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trwFileExplorer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.trwFileExplorer.FullRowSelect = true;
            this.trwFileExplorer.HideSelection = false;
            this.trwFileExplorer.ImageIndex = 0;
            this.trwFileExplorer.ImageList = this.imageList1;
            this.trwFileExplorer.Location = new System.Drawing.Point(0, 0);
            this.trwFileExplorer.Margin = new System.Windows.Forms.Padding(4);
            this.trwFileExplorer.Name = "trwFileExplorer";
            this.trwFileExplorer.SelectedImageIndex = 0;
            this.trwFileExplorer.Size = new System.Drawing.Size(403, 355);
            this.trwFileExplorer.TabIndex = 3;
            this.trwFileExplorer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trwFileExplorer_BeforeExpand);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "FOLDER.ICO");
            this.imageList1.Images.SetKeyName(1, "DVDFolderXP.ico");
            this.imageList1.Images.SetKeyName(2, "DOCL.ICO");
            this.imageList1.Images.SetKeyName(3, "图层集.gif");
            // 
            // AddDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 461);
            this.Controls.Add(this.trwFileExplorer);
            this.Controls.Add(this.groupControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddDataForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择要素类";
            this.Load += new System.EventHandler(this.AddDataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private System.Windows.Forms.TreeView trwFileExplorer;
        private System.Windows.Forms.ImageList imageList1;
    }
}