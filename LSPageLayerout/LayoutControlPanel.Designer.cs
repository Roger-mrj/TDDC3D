namespace RCIS.Controls
{
    partial class LayoutControlPanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutControlPanel));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.打开文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.直接打印ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打印预览ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbtnLayerout = new System.Windows.Forms.ToolStripDropDownButton();
            this.文本标注ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑背景ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑外图框ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axPageLayoutControl = new ESRI.ArcGIS.Controls.AxPageLayoutControl();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1203, 64);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.axToolbarControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(227, 22);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(972, 38);
            this.panel2.TabIndex = 5;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(972, 28);
            this.axToolbarControl1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(4, 22);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 38);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripDropDownButton1,
            this.toolStripSeparator3,
            this.tbtnLayerout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(198, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            this.toolStripSeparator1.Visible = false;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开文件ToolStripMenuItem,
            this.保存文件ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.直接打印ToolStripMenuItem,
            this.打印预览ToolStripMenuItem,
            this.导出图片ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(98, 24);
            this.toolStripDropDownButton1.Text = "系统菜单";
            // 
            // 打开文件ToolStripMenuItem
            // 
            this.打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem";
            this.打开文件ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.打开文件ToolStripMenuItem.Text = "打开文件";
            this.打开文件ToolStripMenuItem.Click += new System.EventHandler(this.打开文件ToolStripMenuItem_Click);
            // 
            // 保存文件ToolStripMenuItem
            // 
            this.保存文件ToolStripMenuItem.Name = "保存文件ToolStripMenuItem";
            this.保存文件ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.保存文件ToolStripMenuItem.Text = "保存文件";
            this.保存文件ToolStripMenuItem.Click += new System.EventHandler(this.保存文件ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(135, 6);
            // 
            // 直接打印ToolStripMenuItem
            // 
            this.直接打印ToolStripMenuItem.Name = "直接打印ToolStripMenuItem";
            this.直接打印ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.直接打印ToolStripMenuItem.Text = "直接打印";
            this.直接打印ToolStripMenuItem.Click += new System.EventHandler(this.直接打印ToolStripMenuItem_Click);
            // 
            // 打印预览ToolStripMenuItem
            // 
            this.打印预览ToolStripMenuItem.Name = "打印预览ToolStripMenuItem";
            this.打印预览ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.打印预览ToolStripMenuItem.Text = "打印预览";
            this.打印预览ToolStripMenuItem.Visible = false;
            this.打印预览ToolStripMenuItem.Click += new System.EventHandler(this.打印预览ToolStripMenuItem_Click);
            // 
            // 导出图片ToolStripMenuItem
            // 
            this.导出图片ToolStripMenuItem.Name = "导出图片ToolStripMenuItem";
            this.导出图片ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.导出图片ToolStripMenuItem.Text = "导出图片";
            this.导出图片ToolStripMenuItem.Click += new System.EventHandler(this.导出图片ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // tbtnLayerout
            // 
            this.tbtnLayerout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbtnLayerout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文本标注ToolStripMenuItem,
            this.编辑背景ToolStripMenuItem,
            this.添加图片ToolStripMenuItem,
            this.编辑外图框ToolStripMenuItem});
            this.tbtnLayerout.Image = ((System.Drawing.Image)(resources.GetObject("tbtnLayerout.Image")));
            this.tbtnLayerout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbtnLayerout.Name = "tbtnLayerout";
            this.tbtnLayerout.Size = new System.Drawing.Size(82, 24);
            this.tbtnLayerout.Text = "整饬管理";
            // 
            // 文本标注ToolStripMenuItem
            // 
            this.文本标注ToolStripMenuItem.Name = "文本标注ToolStripMenuItem";
            this.文本标注ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.文本标注ToolStripMenuItem.Text = "文本标注";
            this.文本标注ToolStripMenuItem.Click += new System.EventHandler(this.文本标注ToolStripMenuItem_Click);
            // 
            // 编辑背景ToolStripMenuItem
            // 
            this.编辑背景ToolStripMenuItem.Name = "编辑背景ToolStripMenuItem";
            this.编辑背景ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.编辑背景ToolStripMenuItem.Text = "编辑背景";
            // 
            // 添加图片ToolStripMenuItem
            // 
            this.添加图片ToolStripMenuItem.Name = "添加图片ToolStripMenuItem";
            this.添加图片ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.添加图片ToolStripMenuItem.Text = "添加图片";
            // 
            // 编辑外图框ToolStripMenuItem
            // 
            this.编辑外图框ToolStripMenuItem.Name = "编辑外图框ToolStripMenuItem";
            this.编辑外图框ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.编辑外图框ToolStripMenuItem.Text = "编辑外图框";
            // 
            // axPageLayoutControl
            // 
            this.axPageLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axPageLayoutControl.Location = new System.Drawing.Point(0, 64);
            this.axPageLayoutControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axPageLayoutControl.Name = "axPageLayoutControl";
            this.axPageLayoutControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPageLayoutControl.OcxState")));
            this.axPageLayoutControl.Size = new System.Drawing.Size(1203, 637);
            this.axPageLayoutControl.TabIndex = 5;
            // 
            // LayoutControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axPageLayoutControl);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "LayoutControlPanel";
            this.Size = new System.Drawing.Size(1203, 701);
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axPageLayoutControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 打开文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 直接打印ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印预览ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private ESRI.ArcGIS.Controls.AxPageLayoutControl axPageLayoutControl;
        private System.Windows.Forms.ToolStripDropDownButton tbtnLayerout;
        private System.Windows.Forms.ToolStripMenuItem 文本标注ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑背景ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑外图框ToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.ToolStripMenuItem 导出图片ToolStripMenuItem;
    }
}
