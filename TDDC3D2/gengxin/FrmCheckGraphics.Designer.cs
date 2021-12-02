namespace TDDC3D.gengxin
{
    partial class FrmCheckGraphics
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
            this.components = new System.ComponentModel.Container();
            this.txtAngle = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.小图斑 = new DevExpress.XtraEditors.LabelControl();
            this.txtArea = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteRow = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.txtAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAngle
            // 
            this.txtAngle.EditValue = "200";
            this.txtAngle.Location = new System.Drawing.Point(184, 23);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(115, 24);
            this.txtAngle.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "狭长图斑:";
            // 
            // 小图斑
            // 
            this.小图斑.Location = new System.Drawing.Point(32, 82);
            this.小图斑.Name = "小图斑";
            this.小图斑.Size = new System.Drawing.Size(50, 18);
            this.小图斑.TabIndex = 5;
            this.小图斑.Text = "小图斑:";
            // 
            // txtArea
            // 
            this.txtArea.EditValue = "50";
            this.txtArea.Location = new System.Drawing.Point(184, 79);
            this.txtArea.Name = "txtArea";
            this.txtArea.Size = new System.Drawing.Size(115, 24);
            this.txtArea.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(141, 26);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(19, 18);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "E>";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(141, 85);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(19, 18);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "S<";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(326, 85);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 18);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "平方米";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(326, 26);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 18);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "E=2L/S";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(415, 26);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 74);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "分析";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridControl1.Location = new System.Drawing.Point(0, 127);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(547, 324);
            this.gridControl1.TabIndex = 17;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteRow});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 28);
            // 
            // deleteRow
            // 
            this.deleteRow.Name = "deleteRow";
            this.deleteRow.Size = new System.Drawing.Size(153, 24);
            this.deleteRow.Text = "删除选中行";
            this.deleteRow.Click += new System.EventHandler(this.deleteRow_Click);
            // 
            // FrmCheckGraphics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 451);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.小图斑);
            this.Controls.Add(this.txtArea);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtAngle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCheckGraphics";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图形预检查";
            this.Load += new System.EventHandler(this.FrmCheckGraphics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtAngle;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl 小图斑;
        private DevExpress.XtraEditors.TextEdit txtArea;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteRow;
    }
}