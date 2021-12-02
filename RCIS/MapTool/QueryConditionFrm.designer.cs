namespace RCIS.MapTool
{
    partial class QueryConditionFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryConditionFrm));
            this.LsbcFields = new DevExpress.XtraEditors.ListBoxControl();
            this.btn_dy = new DevExpress.XtraEditors.SimpleButton();
            this.btn_in = new DevExpress.XtraEditors.SimpleButton();
            this.btn_is = new DevExpress.XtraEditors.SimpleButton();
            this.btn_bfh = new DevExpress.XtraEditors.SimpleButton();
            this.btn_not = new DevExpress.XtraEditors.SimpleButton();
            this.btn_kh = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ = new DevExpress.XtraEditors.SimpleButton();
            this.btn_or = new DevExpress.XtraEditors.SimpleButton();
            this.btn_xydy = new DevExpress.XtraEditors.SimpleButton();
            this.btn_xy = new DevExpress.XtraEditors.SimpleButton();
            this.btn_and = new DevExpress.XtraEditors.SimpleButton();
            this.btn_dydy = new DevExpress.XtraEditors.SimpleButton();
            this.btn_day = new DevExpress.XtraEditors.SimpleButton();
            this.btn_like = new DevExpress.XtraEditors.SimpleButton();
            this.btn_zkh = new DevExpress.XtraEditors.SimpleButton();
            this.btn_GetValue = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.lsbcValue = new DevExpress.XtraEditors.ListBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSelection = new System.Windows.Forms.Label();
            this.btnTest = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtCondition = new System.Windows.Forms.TextBox();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.LsbcFields)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsbcValue)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LsbcFields
            // 
            this.LsbcFields.Location = new System.Drawing.Point(16, 6);
            this.LsbcFields.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LsbcFields.Name = "LsbcFields";
            this.LsbcFields.Size = new System.Drawing.Size(469, 155);
            this.LsbcFields.TabIndex = 1;
            this.LsbcFields.SelectedIndexChanged += new System.EventHandler(this.LsbcFields_SelectedIndexChanged);
            this.LsbcFields.DoubleClick += new System.EventHandler(this.LsbcFields_DoubleClick);
            // 
            // btn_dy
            // 
            this.btn_dy.Location = new System.Drawing.Point(4, 4);
            this.btn_dy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_dy.Name = "btn_dy";
            this.btn_dy.Size = new System.Drawing.Size(43, 29);
            this.btn_dy.TabIndex = 3;
            this.btn_dy.Text = "=";
            this.btn_dy.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_in
            // 
            this.btn_in.Location = new System.Drawing.Point(55, 149);
            this.btn_in.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_in.Name = "btn_in";
            this.btn_in.Size = new System.Drawing.Size(43, 29);
            this.btn_in.TabIndex = 4;
            this.btn_in.Text = "In";
            this.btn_in.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_is
            // 
            this.btn_is.Location = new System.Drawing.Point(105, 149);
            this.btn_is.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_is.Name = "btn_is";
            this.btn_is.Size = new System.Drawing.Size(43, 29);
            this.btn_is.TabIndex = 5;
            this.btn_is.Text = "Is";
            this.btn_is.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_bfh
            // 
            this.btn_bfh.Location = new System.Drawing.Point(4, 149);
            this.btn_bfh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_bfh.Name = "btn_bfh";
            this.btn_bfh.Size = new System.Drawing.Size(43, 29);
            this.btn_bfh.TabIndex = 6;
            this.btn_bfh.Text = "%";
            this.btn_bfh.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_not
            // 
            this.btn_not.Location = new System.Drawing.Point(105, 112);
            this.btn_not.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_not.Name = "btn_not";
            this.btn_not.Size = new System.Drawing.Size(43, 29);
            this.btn_not.TabIndex = 7;
            this.btn_not.Text = "Not";
            this.btn_not.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_kh
            // 
            this.btn_kh.Location = new System.Drawing.Point(55, 112);
            this.btn_kh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_kh.Name = "btn_kh";
            this.btn_kh.Size = new System.Drawing.Size(43, 29);
            this.btn_kh.TabIndex = 8;
            this.btn_kh.Text = "()";
            this.btn_kh.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_
            // 
            this.btn_.Location = new System.Drawing.Point(4, 112);
            this.btn_.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_.Name = "btn_";
            this.btn_.Size = new System.Drawing.Size(43, 29);
            this.btn_.TabIndex = 9;
            this.btn_.Text = "_";
            this.btn_.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_or
            // 
            this.btn_or.Location = new System.Drawing.Point(105, 76);
            this.btn_or.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_or.Name = "btn_or";
            this.btn_or.Size = new System.Drawing.Size(43, 29);
            this.btn_or.TabIndex = 10;
            this.btn_or.Text = "Or";
            this.btn_or.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_xydy
            // 
            this.btn_xydy.Location = new System.Drawing.Point(55, 76);
            this.btn_xydy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_xydy.Name = "btn_xydy";
            this.btn_xydy.Size = new System.Drawing.Size(43, 29);
            this.btn_xydy.TabIndex = 11;
            this.btn_xydy.Text = "<=";
            this.btn_xydy.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_xy
            // 
            this.btn_xy.Location = new System.Drawing.Point(4, 76);
            this.btn_xy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_xy.Name = "btn_xy";
            this.btn_xy.Size = new System.Drawing.Size(43, 29);
            this.btn_xy.TabIndex = 12;
            this.btn_xy.Text = "<";
            this.btn_xy.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_and
            // 
            this.btn_and.Location = new System.Drawing.Point(105, 40);
            this.btn_and.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_and.Name = "btn_and";
            this.btn_and.Size = new System.Drawing.Size(43, 29);
            this.btn_and.TabIndex = 13;
            this.btn_and.Text = "And";
            this.btn_and.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_dydy
            // 
            this.btn_dydy.Location = new System.Drawing.Point(55, 40);
            this.btn_dydy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_dydy.Name = "btn_dydy";
            this.btn_dydy.Size = new System.Drawing.Size(43, 29);
            this.btn_dydy.TabIndex = 14;
            this.btn_dydy.Text = ">=";
            this.btn_dydy.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_day
            // 
            this.btn_day.Location = new System.Drawing.Point(4, 40);
            this.btn_day.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_day.Name = "btn_day";
            this.btn_day.Size = new System.Drawing.Size(43, 29);
            this.btn_day.TabIndex = 15;
            this.btn_day.Text = ">";
            this.btn_day.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_like
            // 
            this.btn_like.Location = new System.Drawing.Point(105, 4);
            this.btn_like.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_like.Name = "btn_like";
            this.btn_like.Size = new System.Drawing.Size(43, 29);
            this.btn_like.TabIndex = 16;
            this.btn_like.Text = "Like";
            this.btn_like.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_zkh
            // 
            this.btn_zkh.Location = new System.Drawing.Point(55, 4);
            this.btn_zkh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_zkh.Name = "btn_zkh";
            this.btn_zkh.Size = new System.Drawing.Size(43, 29);
            this.btn_zkh.TabIndex = 17;
            this.btn_zkh.Text = "<>";
            this.btn_zkh.Click += new System.EventHandler(this.btn_Click_AppendOperator);
            // 
            // btn_GetValue
            // 
            this.btn_GetValue.Location = new System.Drawing.Point(156, 149);
            this.btn_GetValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_GetValue.Name = "btn_GetValue";
            this.btn_GetValue.Size = new System.Drawing.Size(116, 29);
            this.btn_GetValue.TabIndex = 18;
            this.btn_GetValue.Text = "获取唯一值";
            this.btn_GetValue.Click += new System.EventHandler(this.btn_GetValue_Click);
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(4, 11);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(76, 29);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "清除";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.lsbcValue);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_dy);
            this.panel1.Controls.Add(this.btn_day);
            this.panel1.Controls.Add(this.btn_GetValue);
            this.panel1.Controls.Add(this.btn_zkh);
            this.panel1.Controls.Add(this.btn_is);
            this.panel1.Controls.Add(this.btn_bfh);
            this.panel1.Controls.Add(this.btn_in);
            this.panel1.Controls.Add(this.btn_not);
            this.panel1.Controls.Add(this.btn_kh);
            this.panel1.Controls.Add(this.btn_);
            this.panel1.Controls.Add(this.btn_or);
            this.panel1.Controls.Add(this.btn_xydy);
            this.panel1.Controls.Add(this.btn_xy);
            this.panel1.Controls.Add(this.btn_and);
            this.panel1.Controls.Add(this.btn_dydy);
            this.panel1.Controls.Add(this.btn_like);
            this.panel1.Location = new System.Drawing.Point(16, 169);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 184);
            this.panel1.TabIndex = 20;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(323, 151);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(137, 24);
            this.txtSearch.TabIndex = 24;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lsbcValue
            // 
            this.lsbcValue.Location = new System.Drawing.Point(156, 4);
            this.lsbcValue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lsbcValue.Name = "lsbcValue";
            this.lsbcValue.Size = new System.Drawing.Size(304, 140);
            this.lsbcValue.TabIndex = 23;
            this.lsbcValue.DoubleClick += new System.EventHandler(this.lsbcValue_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(280, 156);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "转至";
            // 
            // lblSelection
            // 
            this.lblSelection.AutoSize = true;
            this.lblSelection.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.Location = new System.Drawing.Point(16, 368);
            this.lblSelection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(41, 18);
            this.lblSelection.TabIndex = 23;
            this.lblSelection.Text = "lbCX";
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Image = ((System.Drawing.Image)(resources.GetObject("btnTest.Image")));
            this.btnTest.Location = new System.Drawing.Point(89, 11);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(76, 29);
            this.btnTest.TabIndex = 25;
            this.btnTest.Text = "验证";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOk
            // 
            this.btnOk.Image = ((System.Drawing.Image)(resources.GetObject("btnOk.Image")));
            this.btnOk.Location = new System.Drawing.Point(259, 11);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(76, 29);
            this.btnOk.TabIndex = 27;
            this.btnOk.Text = "过滤";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(344, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 29);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnTest);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Location = new System.Drawing.Point(55, 499);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(430, 49);
            this.panel2.TabIndex = 29;
            // 
            // txtCondition
            // 
            this.txtCondition.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCondition.Location = new System.Drawing.Point(16, 389);
            this.txtCondition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCondition.Multiline = true;
            this.txtCondition.Name = "txtCondition";
            this.txtCondition.Size = new System.Drawing.Size(468, 102);
            this.txtCondition.TabIndex = 30;
            this.txtCondition.TextChanged += new System.EventHandler(this.txtCondition_TextChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSelect.Image")));
            this.btnSelect.Location = new System.Drawing.Point(174, 10);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(76, 29);
            this.btnSelect.TabIndex = 29;
            this.btnSelect.Text = "选中";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // QueryConditionFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 554);
            this.Controls.Add(this.txtCondition);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblSelection);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LsbcFields);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QueryConditionFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询构建器";
            this.Load += new System.EventHandler(this.QueryConditionFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LsbcFields)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lsbcValue)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl LsbcFields;
        private DevExpress.XtraEditors.SimpleButton btn_dy;
        private DevExpress.XtraEditors.SimpleButton btn_in;
        private DevExpress.XtraEditors.SimpleButton btn_is;
        private DevExpress.XtraEditors.SimpleButton btn_bfh;
        private DevExpress.XtraEditors.SimpleButton btn_not;
        private DevExpress.XtraEditors.SimpleButton btn_kh;
        private DevExpress.XtraEditors.SimpleButton btn_;
        private DevExpress.XtraEditors.SimpleButton btn_or;
        private DevExpress.XtraEditors.SimpleButton btn_xydy;
        private DevExpress.XtraEditors.SimpleButton btn_xy;
        private DevExpress.XtraEditors.SimpleButton btn_and;
        private DevExpress.XtraEditors.SimpleButton btn_dydy;
        private DevExpress.XtraEditors.SimpleButton btn_day;
        private DevExpress.XtraEditors.SimpleButton btn_like;
        private DevExpress.XtraEditors.SimpleButton btn_zkh;
        private DevExpress.XtraEditors.SimpleButton btn_GetValue;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSelection;
        private DevExpress.XtraEditors.SimpleButton btnTest;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.TextEdit txtSearch;
        private DevExpress.XtraEditors.ListBoxControl lsbcValue;
        private System.Windows.Forms.TextBox txtCondition;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
    }
}