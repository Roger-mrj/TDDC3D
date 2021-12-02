namespace RCIS.Controls
{
    partial class PropertyEditorForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.objectTree = new System.Windows.Forms.TreeView();
            this.EditorPanel = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectTree
            // 
            this.objectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTree.FullRowSelect = true;
            this.objectTree.HideSelection = false;
            this.objectTree.Location = new System.Drawing.Point(3, 17);
            this.objectTree.Name = "objectTree";
            this.objectTree.Size = new System.Drawing.Size(162, 419);
            this.objectTree.TabIndex = 0;
            this.objectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectTree_AfterSelect);
            this.objectTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.objectTree_MouseDown);
            // 
            // EditorPanel
            // 
            this.EditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditorPanel.Location = new System.Drawing.Point(173, 0);
            this.EditorPanel.Name = "EditorPanel";
            this.EditorPanel.Size = new System.Drawing.Size(369, 439);
            this.EditorPanel.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Info;
            this.splitter1.Location = new System.Drawing.Point(168, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 439);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.objectTree);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 439);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图层";
            // 
            // PropertyEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 439);
            this.Controls.Add(this.EditorPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "PropertyEditorForm";
            this.ShowIcon = false;
            this.Text = "属性编辑器";
            this.Load += new System.EventHandler(this.PropertyEditorForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PropertyEditorForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView objectTree;
        private System.Windows.Forms.Panel EditorPanel;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}