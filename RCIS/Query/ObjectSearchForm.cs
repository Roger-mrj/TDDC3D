using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.Text;
using System.Globalization ;
using ESRI.ArcGIS.esriSystem;

using RCIS.GISCommon;
using RCIS.Utility;

//using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace RCIS.MapTool
{
	/// <summary>
	/// ZDSearchForm 的摘要说明。
	/// </summary>
	public class ObjectSearchForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ComboBox cbFldName;
		private System.Windows.Forms.ComboBox cbOpCode;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbAnd;
		private System.Windows.Forms.RadioButton rbOr;
		private System.Windows.Forms.ListView queryFilterList;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.ListView queryResultList;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.ComboBox cbValue;
        private System.Windows.Forms.GroupBox gbLayer;
        private System.Windows.Forms.ComboBox cbLayer;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private Button btnCancel;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private CheckBox chkAll;
        private TextBox txtFrom;
        private ComboBox cboTo;
        private Label label6;
        private Label label5;
        private Label label4;
        private Button cmdOK;
        private Button btnOutExcel;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ObjectSearchForm()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
            
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbFldName = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOr = new System.Windows.Forms.RadioButton();
            this.rbAnd = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbOpCode = new System.Windows.Forms.ComboBox();
            this.cbValue = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.queryFilterList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu2 = new System.Windows.Forms.ContextMenu();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.queryResultList = new System.Windows.Forms.ListView();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnOutExcel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.cboTo = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbLayer = new System.Windows.Forms.GroupBox();
            this.cbLayer = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gbLayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 72);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1052, 580);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1044, 551);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "查询条件";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkAll);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.cbFldName);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Controls.Add(this.cbOpCode);
            this.groupBox3.Controls.Add(this.cbValue);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.btnQuery);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 370);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1044, 181);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(125, 104);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(279, 19);
            this.chkAll.TabIndex = 14;
            this.chkAll.Text = "显示全部信息(不选显示前100条记录)";
            this.chkAll.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(541, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 29);
            this.label3.TabIndex = 8;
            this.label3.Text = "数据值";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(891, 22);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 29);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "清除(&R)";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(777, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 11;
            this.button1.Text = "删除(&D)";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbFldName
            // 
            this.cbFldName.Location = new System.Drawing.Point(115, 51);
            this.cbFldName.MaxDropDownItems = 25;
            this.cbFldName.Name = "cbFldName";
            this.cbFldName.Size = new System.Drawing.Size(192, 23);
            this.cbFldName.TabIndex = 0;
            this.cbFldName.SelectedIndexChanged += new System.EventHandler(this.cbFldName_SelectedIndexChanged);
            this.cbFldName.SelectedValueChanged += new System.EventHandler(this.cbFldName_SelectedValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOr);
            this.groupBox1.Controls.Add(this.rbAnd);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 157);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "组合关系";
            // 
            // rbOr
            // 
            this.rbOr.Location = new System.Drawing.Point(8, 76);
            this.rbOr.Name = "rbOr";
            this.rbOr.Size = new System.Drawing.Size(75, 31);
            this.rbOr.TabIndex = 1;
            this.rbOr.Text = "或者";
            // 
            // rbAnd
            // 
            this.rbAnd.Checked = true;
            this.rbAnd.Location = new System.Drawing.Point(9, 32);
            this.rbAnd.Name = "rbAnd";
            this.rbAnd.Size = new System.Drawing.Size(75, 31);
            this.rbAnd.TabIndex = 0;
            this.rbAnd.TabStop = true;
            this.rbAnd.Text = "而且";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(777, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(214, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "返回(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbOpCode
            // 
            this.cbOpCode.Location = new System.Drawing.Point(327, 51);
            this.cbOpCode.Name = "cbOpCode";
            this.cbOpCode.Size = new System.Drawing.Size(174, 23);
            this.cbOpCode.TabIndex = 1;
            // 
            // cbValue
            // 
            this.cbValue.Location = new System.Drawing.Point(531, 54);
            this.cbValue.MaxDropDownItems = 25;
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(194, 23);
            this.cbValue.TabIndex = 12;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(777, 64);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 29);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(891, 64);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(100, 29);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "查询(&S)";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(113, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "字段";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(349, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "操作符";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.queryFilterList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1044, 370);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // queryFilterList
            // 
            this.queryFilterList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.queryFilterList.ContextMenu = this.contextMenu2;
            this.queryFilterList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryFilterList.FullRowSelect = true;
            this.queryFilterList.GridLines = true;
            this.queryFilterList.HideSelection = false;
            this.queryFilterList.Location = new System.Drawing.Point(3, 21);
            this.queryFilterList.Name = "queryFilterList";
            this.queryFilterList.Size = new System.Drawing.Size(1038, 346);
            this.queryFilterList.TabIndex = 4;
            this.queryFilterList.UseCompatibleStateImageBehavior = false;
            this.queryFilterList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "组合关系";
            this.columnHeader1.Width = 136;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "查询字段";
            this.columnHeader2.Width = 153;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "操作符";
            this.columnHeader3.Width = 79;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "数值";
            this.columnHeader4.Width = 262;
            // 
            // contextMenu2
            // 
            this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2});
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "删除查询条件";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1044, 551);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "查询结果";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.queryResultList);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1044, 485);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            // 
            // queryResultList
            // 
            this.queryResultList.ContextMenu = this.contextMenu1;
            this.queryResultList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryResultList.FullRowSelect = true;
            this.queryResultList.GridLines = true;
            this.queryResultList.HideSelection = false;
            this.queryResultList.Location = new System.Drawing.Point(3, 21);
            this.queryResultList.Name = "queryResultList";
            this.queryResultList.Size = new System.Drawing.Size(1038, 461);
            this.queryResultList.TabIndex = 0;
            this.queryResultList.UseCompatibleStateImageBehavior = false;
            this.queryResultList.View = System.Windows.Forms.View.Details;
            this.queryResultList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.queryResultList_ColumnClick);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem6,
            this.menuItem4,
            this.menuItem5,
            this.menuItem7,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "定位图形";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "选中图形";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "选中所有";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 5;
            this.menuItem3.Text = "导出到Excel";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnOutExcel);
            this.groupBox5.Controls.Add(this.cmdOK);
            this.groupBox5.Controls.Add(this.txtFrom);
            this.groupBox5.Controls.Add(this.cboTo);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(0, 485);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1044, 66);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Location = new System.Drawing.Point(912, 24);
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(113, 30);
            this.btnOutExcel.TabIndex = 6;
            this.btnOutExcel.Text = "导出Excel";
            this.btnOutExcel.UseVisualStyleBackColor = true;
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(787, 24);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(98, 30);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "刷新显示";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(57, 18);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(72, 25);
            this.txtFrom.TabIndex = 4;
            this.txtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFrom_KeyPress);
            // 
            // cboTo
            // 
            this.cboTo.FormattingEnabled = true;
            this.cboTo.Location = new System.Drawing.Point(232, 18);
            this.cboTo.Name = "cboTo";
            this.cboTo.Size = new System.Drawing.Size(77, 23);
            this.cboTo.TabIndex = 3;
            this.cboTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboTo_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(317, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "条记录";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "条记录到第";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "从第";
            // 
            // gbLayer
            // 
            this.gbLayer.Controls.Add(this.cbLayer);
            this.gbLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbLayer.Location = new System.Drawing.Point(0, 0);
            this.gbLayer.Name = "gbLayer";
            this.gbLayer.Size = new System.Drawing.Size(796, 62);
            this.gbLayer.TabIndex = 1;
            this.gbLayer.TabStop = false;
            this.gbLayer.Text = "目标图层";
            // 
            // cbLayer
            // 
            this.cbLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbLayer.Location = new System.Drawing.Point(3, 21);
            this.cbLayer.Name = "cbLayer";
            this.cbLayer.Size = new System.Drawing.Size(790, 23);
            this.cbLayer.TabIndex = 0;
            this.cbLayer.SelectedIndexChanged += new System.EventHandler(this.cbLayer_SelectedIndexChanged);
            // 
            // ObjectSearchForm
            // 
            this.AcceptButton = this.btnQuery;
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(796, 513);
            this.Controls.Add(this.gbLayer);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectSearchForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信息查询";
            this.Load += new System.EventHandler(this.ZDSearchForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.gbLayer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
        private String WildcardCharacter="%";
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
                if ((this.cbOpCode.Text != "为空") && (this.cbOpCode.Text != "不为空"))
                {
                    if (this.cbValue.Text == "")
                    {
                        MessageBox.Show("请填写数据值");
                        return;
                    }
                }
				FieldName curFld=this.cbFldName .SelectedItem as FieldName ;
				OpCodeItem curOp=this.cbOpCode .SelectedItem as OpCodeItem;
				if(curFld!=null&&curOp!=null)
				{
					QueryFilterItem curItem=new QueryFilterItem ();					
					curItem.QueryAnd =this.rbAnd .Checked ;
					curItem.FieldName =curFld;
					curItem.OpCodeItem =curOp;
                    string opV=this.cbValue .Text .Trim ();
                    int index=opV.IndexOf (" | ");
                    if(index>=0)
                        opV=opV.Substring (0,index);
					curItem.OpValue =opV;
					this.PutQueryFilterItem (curItem);
				}
			}
			catch(Exception ex)
			{
			}
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			this.queryFilterList .Items.Clear ();
		}
        public IFeatureLayer  TargetLayer;
		public IActiveView   ActiveView;
        public AxMapControl MapControl;
		private void FillLayerInfo()
		{
			this.cbFldName.Items.Clear ();
			this.cbOpCode .Items.Clear ();
			this.cbValue .Items.Clear ();
			this.queryFilterList .Items.Clear ();
			if(this.TargetLayer !=null&&this.ActiveView !=null)
			{
				IFeatureClass queryClass=this.TargetLayer .FeatureClass ;
				if(queryClass!=null)
				{
					int fldCount=queryClass.Fields .FieldCount ;
					for(int fldIndex=0;fldIndex<fldCount;fldIndex++)
					{
						IField curFld=queryClass.Fields .get_Field (fldIndex);
						if(curFld.Type !=esriFieldType.esriFieldTypeGeometry
							&& curFld.Type !=esriFieldType.esriFieldTypeRaster)
						{
							if(curFld == queryClass.LengthField
								||curFld == queryClass.AreaField )
							{//不能用来查询 跳过去
								
							}
							else
							{							
								FieldName fldName=new FieldName ();
								fldName.TargetField =curFld;
								this.cbFldName .Items.Add (fldName);
							}
						}
					}
					if(this.cbFldName .Items .Count >0)
					{
						this.cbFldName .SelectedItem =this.cbFldName .Items [0];
					}
				}
			}
			if(this.cbFldName .Items .Count <=0)
			{
				this.btnAdd .Enabled =false;
				this.btnClear .Enabled =false;
				this.btnQuery .Enabled =false;
			}
		}
		private void ZDSearchForm_Load(object sender, System.EventArgs e)
		{
            this.queryResultList.ListViewItemSorter = new ControlStyleHelper.ListViewColumnSorter();

			if(this.ActiveView !=null)
			{
				for(int i=0;i<(this.ActiveView.FocusMap ) .LayerCount ;i++)
				{
					ILayer curLayer=(this.ActiveView.FocusMap ) .get_Layer (i);
					if(curLayer is IGeoFeatureLayer )
					{
						LocalLayerObject objLayer=new LocalLayerObject ();
						objLayer.GeoLayer =curLayer as IGeoFeatureLayer ;
						this.cbLayer .Items.Add (objLayer);
					}
                    else if (curLayer is IGroupLayer)
                    {
                        ICompositeLayer compositeLayer = curLayer as ICompositeLayer;
                        for (int kk = 0; kk < compositeLayer.Count; kk++)
                        {
                            ILayer childLyr = compositeLayer.get_Layer(kk);
                            if (childLyr is IGeoFeatureLayer)
                            {
                                LocalLayerObject objLayer = new LocalLayerObject();
                                objLayer.GeoLayer = childLyr as IGeoFeatureLayer;
                                this.cbLayer.Items.Add(objLayer);
                            }
                        }
                    }
				}
				if(this.cbLayer .Items .Count >0)
				{
					this.cbLayer .SelectedIndex =0;
                    
				}
			}
		}
		private class LocalLayerObject
		{
			public IGeoFeatureLayer GeoLayer;
			public override string ToString()
			{
				return this.GeoLayer .Name ;
			}

		}
		private void PutQueryFilterItem(QueryFilterItem curItem)
		{
			if(curItem!=null)
			{
				ListViewItem lvi=new ListViewItem ();
				if(curItem.QueryAnd )
				{
					lvi.Text ="而且";
				}
				else
				{
					lvi.Text ="或者";
				}
				lvi.SubItems .Add (curItem.FieldName.TargetField .AliasName );
				lvi.SubItems .Add (curItem.OpCodeItem .OpName );
				lvi.SubItems .Add (curItem.OpValue );
				if(this.queryFilterList .Items .Count %2==1)
				{
					lvi.BackColor =Color.Wheat;
				}
				lvi.Tag =curItem;
				this.queryFilterList .Items .Add (lvi);
			}
		}
		private void cbFldName_SelectedValueChanged(object sender, System.EventArgs e)
		{
			this.cbOpCode .Items.Clear ();
            this.cbValue .Items.Clear ();
            this.cbValue.Text ="";
			FieldName curFld=this.cbFldName .SelectedItem as FieldName;
			if(curFld!=null)
			{
				switch(curFld.TargetField .Type )
				{
					case esriFieldType.esriFieldTypeString:
					{
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeEquals);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeNotEqual);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeContains);
						this.cbOpCode .Items.Add (OpCodeItem.OpCodeNotNull);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeNull);
						break;
					}
					default:
					{
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeEquals);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeNotEqual);	
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeLessEqual);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeLess);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeGreat);
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeGreatEqual);
						this.cbOpCode .Items.Add (OpCodeItem.OpCodeNotNull);						
						this.cbOpCode .Items .Add (OpCodeItem.OpCodeNull);
						break;						
					}
				}
				this.cbOpCode .SelectedItem =this.cbOpCode .Items [0];
			}
		}

		private void btnQuery_Click(object sender, System.EventArgs e)
		{

            
            try
            {
                
                if (this.TargetLayer != null && this.ActiveView != null && this.queryFilterList.Items.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    int filterItemCount = this.queryFilterList.Items.Count;
                    for (int filterIndex = 0; filterIndex < filterItemCount; filterIndex++)
                    {
                        try
                        {
                            QueryFilterItem curItem = this.queryFilterList.Items[filterIndex].Tag as QueryFilterItem;
                            if (filterIndex != 0)
                            {
                                if (curItem.QueryAnd)
                                {
                                    builder.Append(" and ");
                                }
                                else
                                {
                                    builder.Append(" or ");
                                }
                            }
                            string FldName = curItem.FieldName.TargetField.Name;
                            //						if(FldName.IndexOf(".")>-1)
                            //						{
                            //							FldName = "["+FldName+"]";
                            //						}
                            if (curItem.OpCodeItem == OpCodeItem.OpCodeContains)
                            {
                                builder.Append(FldName + " like '" + this.WildcardCharacter + curItem.OpValue + this.WildcardCharacter + "'");
                            }
                            else if (curItem.OpCodeItem == OpCodeItem.OpCodeNotNull)
                            {
                                builder.Append(FldName + " is not null ");
                            }
                            else if (curItem.OpCodeItem == OpCodeItem.OpCodeNull)
                            {
                                builder.Append(FldName + " is null ");
                            }
                            else
                            {
                                if (curItem.FieldName.TargetField.Type == esriFieldType.esriFieldTypeString)
                                {
                                    builder.Append(FldName);
                                    builder.Append(" " + curItem.OpCodeItem.OpCode + " ");
                                    builder.Append("'" + curItem.OpValue + "' ");
                                }
                                else
                                {
                                    builder.Append(FldName);
                                    builder.Append(" " + curItem.OpCodeItem.OpCode + " ");
                                    builder.Append(curItem.OpValue + " ");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    string querySQL = builder.ToString();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    this.txtFrom.Text = "1";
                    this.cboTo.Text = "100";
                    this.MakeQuery(querySQL, true);
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (Exception ex)
            { }
		}
		
        public void MakeQuery(String querySQL,bool prompt)
        {
            if(querySQL!=null&&this.TargetLayer is IGeoFeatureLayer&&this.ActiveView !=null)
            {
                IQueryFilter queryFilter=new QueryFilterClass ();
                queryFilter.WhereClause =querySQL;
                IFeatureClass queryClass=this.TargetLayer .FeatureClass ;
                int resultCount=queryClass.FeatureCount(queryFilter);
                if (resultCount == 0)
                {
                    MessageBox.Show("没有该条件的查询结果");
                    return;
                }
                DialogResult dr=DialogResult.Cancel;
                if(!prompt)
                {
                    dr=DialogResult.OK;
                }
                else
                    dr=MessageBox.Show ("一共找到"+resultCount +"个要素,是否需要提取所有信息?","信息查询",MessageBoxButtons.YesNo ,MessageBoxIcon.Question );
                if(DialogResult.Yes==dr)
                {
                    IFeatureCursor cursor=queryClass.Search (queryFilter,false);
                    ArrayList searchResultList=new ArrayList ();
                    
                    IFeature curFea=cursor.NextFeature ();
                    while(curFea!=null)
                    {
                        searchResultList.Add (curFea);
                        curFea=cursor.NextFeature ();
                    }
                    pList = searchResultList;
                    this.PutQueryResult(searchResultList);
                    this.cboTo.Items.Clear();
                    this.cboTo.Items.Add(searchResultList.Count);
                  
                   
                    
                }
            }
            OtherHelper.ReleaseComObject(Cursor);
        }
       private ArrayList pList = new ArrayList();
		private void PutQueryResult(ArrayList resultList)
		{
			this.queryResultList .Columns .Clear ();
			this.queryResultList .Items.Clear ();
			if(resultList.Count >0)
			{				
				IFeature curFea=resultList[0] as IFeature;
				int fldCount=curFea.Fields .FieldCount ;
				#region 填充字段
				for(int fldIndex=0;fldIndex<fldCount;fldIndex++)
				{
					IField curFld=curFea.Fields .get_Field (fldIndex);
					if(curFld.Type ==esriFieldType.esriFieldTypeGeometry
						||curFld.Type ==esriFieldType.esriFieldTypeRaster
						
						)
					{
						continue;
					}
					ExtColumnHeader header=new ExtColumnHeader (curFld.AliasName ,fldIndex);					
					this.queryResultList .Columns .Add (header);
				}
				#endregion

                if(this.chkAll.Checked==true)
                {
                    #region 显示全部
                    for (int feaIndex = 0; feaIndex < resultList.Count; feaIndex++)
                    {
                        curFea = resultList[feaIndex] as IFeature;
                        ListViewItem lvi = new ListViewItem();
                        fldCount = this.queryResultList.Columns.Count;
                        for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
                        {
                            ExtColumnHeader curHeader = this.queryResultList.Columns[fldIndex] as ExtColumnHeader;
                            if (fldIndex == 0)
                            {
                                lvi.Text = curFea.get_Value(0).ToString();
                            }
                            else
                            {
                                lvi.SubItems.Add(curFea.get_Value(curHeader.CurrentFieldIndex).ToString());
                            }
                        }
                        if (feaIndex % 2 == 0)
                        {
                            lvi.BackColor = Color.Wheat;
                        }
                        lvi.Tag = curFea;
                        this.queryResultList.Items.Add(lvi);
                    }
                    #endregion
                }
                else
                {
                    #region 显示前100条记录
                    int pCount = 101;
                    if (resultList.Count < 101)
                    {
                        pCount = resultList.Count;
                    }
                    for (int feaIndex = 0; feaIndex < pCount; feaIndex++)
                    {
                        curFea = resultList[feaIndex] as IFeature;
                        ListViewItem lvi = new ListViewItem();
                        fldCount = this.queryResultList.Columns.Count;
                        for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
                        {
                            ExtColumnHeader curHeader = this.queryResultList.Columns[fldIndex] as ExtColumnHeader;
                            if (fldIndex == 0)
                            {
                                lvi.Text = curFea.get_Value(0).ToString();
                            }
                            else
                            {
                                lvi.SubItems.Add(curFea.get_Value(curHeader.CurrentFieldIndex).ToString());
                            }
                        }
                        if (feaIndex % 2 == 0)
                        {
                            lvi.BackColor = Color.Wheat;
                        }
                        lvi.Tag = curFea;
                        this.queryResultList.Items.Add(lvi);
                    }
                    #endregion
                }
				this.tabControl1 .SelectedIndex =1;
			}
		}
        //定位图形
		private void menuItem1_Click(object sender, System.EventArgs e)
		{//
            this.MapControl.Map.ClearSelection();
            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.ActiveView.Extent);
			if(this.queryResultList .SelectedItems .Count >0)
			{
				ListViewItem selItem=this.queryResultList .SelectedItems [0];
				IFeature selFea=selItem.Tag as IFeature;
				if(selFea!=null)
				{
					IGeometry geom=selFea.Shape ;
					if(geom!=null&&!geom.IsEmpty )
					{
						IEnvelope env=geom.Envelope ;
						env.Expand (1.5,1.5,true);
						this.ActiveView .Extent =env;						
						this .ActiveView .PartialRefresh (esriViewDrawPhase.esriViewGeography,null,env);
						this .ActiveView.ScreenDisplay.UpdateWindow();
                      this.MapControl.FlashShape(geom, 3, 300, null);
					}
				}
			}
		}
        //选中图形
        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            this.MapControl.Map.ClearSelection();
            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.ActiveView.Extent);
            if (this.queryResultList.SelectedItems.Count > 0)
            {
                ListViewItem selItem = this.queryResultList.SelectedItems[0];
                IFeature selFea = selItem.Tag as IFeature;
                if (selFea != null)
                {
                    IGeometry geom = selFea.Shape;
                    if (geom != null && !geom.IsEmpty)
                    {
                        string aLayerName = LayerHelper.GetClassShortName(selFea.Table as IDataset);
                        ILayer aLayer = LayerHelper.QueryLayerByModelName(this.ActiveView.FocusMap, aLayerName);
                        this.ActiveView.FocusMap.SelectFeature(aLayer, selFea);
                        this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, geom.Envelope);
                        this.ActiveView.ScreenDisplay.UpdateWindow();
                    }
                }
            }
        }
        //选中所有
        private void menuItem5_Click(object sender, System.EventArgs e)
        {
            this.MapControl.Map.ClearSelection();
            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.ActiveView.Extent);
            //因为显示有两种形式，全部显示和部分显示，
            //但是现实选中所有必须选中所有的数据，到处数据也是一样道理
            IFeature selFea = null;
            for (int i = 0; i < pList.Count; i++)
            {
                selFea = pList[i] as IFeature;
                if (selFea != null)
                {
                    IGeometry geom = selFea.Shape;
                    if (geom != null && !geom.IsEmpty)
                    {
                        string aLayerName = LayerHelper.GetClassShortName(selFea.Table as IDataset);
                        ILayer aLayer = LayerHelper.QueryLayerByModelName(this.ActiveView.FocusMap, aLayerName);
                        this.ActiveView.FocusMap.SelectFeature(aLayer, selFea);
                    }
                }
            }
            this.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, this.ActiveView.Extent);
            this.ActiveView.ScreenDisplay.UpdateWindow();    
            //if(this.queryResultList .Items .Count >0)
            //{

            //    for (int i = 0; i < this.queryResultList.Items.Count;i++ )
            //    {
            //        ListViewItem selItem = this.queryResultList.Items[i];
            //        IFeature selFea = selItem.Tag as IFeature;
            //        if (selFea != null)
            //        {
            //            IGeometry geom = selFea.Shape;
            //            if (geom != null && !geom.IsEmpty)
            //            {
            //                string aLayerName = LayerHelper.GetClassShortName(selFea.Table as IDataset);
            //                ILayer aLayer = LayerHelper.QueryLayerByModelName(this.ActiveView.FocusMap, aLayerName);
            //                this.ActiveView.FocusMap.SelectFeature(aLayer, selFea);
            //            }
            //        }
            //    }
            //    this .ActiveView .PartialRefresh (esriViewDrawPhase.esriViewGeoSelection,null,this.ActiveView .Extent);
            //    this .ActiveView.ScreenDisplay.UpdateWindow();     
            //}
        }
		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			if(this.queryFilterList.SelectedItems.Count>0)
			{
				ListViewItem selItem=this.queryFilterList.SelectedItems[0];
				this.queryFilterList.Items.Remove(selItem);
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			IEnumerator iter = queryFilterList.SelectedIndices.GetEnumerator();
			while(iter.MoveNext())
			{
				int idx = (int)iter.Current;
				queryFilterList.Items.RemoveAt(idx);
			}
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
            #region 去掉导出功能
            if (this.pList.Count <= 0)
            {
                MessageBox.Show("没有找到任何数据,无法完成数据导出");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            #region 输出到Excel中去.
            #region 打开Excel
            String excelReportFilename = this.CopyExcelTemplate();
            if (excelReportFilename == null)
                return;
            Excel.ApplicationClass objApp = new Excel.ApplicationClass();
            objApp.Visible = false;
            Excel.Workbooks objWorkbooks = objApp.Workbooks;
            object o = Type.Missing;
            Excel.Workbook objBook = objWorkbooks.Open(excelReportFilename, false, false, o, o, o, true, o, o, true, o, o, true, o, o);
            #endregion
            Excel.Worksheet worksheet = (Excel.Worksheet)objBook.Worksheets.get_Item(1);
            //看看要导出多少字段
            int exportFieldCount = this.GetFieldCount();
            int headerColumnCount = exportFieldCount + 2;
            if (headerColumnCount < 5)
                headerColumnCount = 5;
            String lastColumnName = this.GetColumnName(headerColumnCount);
            Excel.Range range = worksheet.get_Range("A1", lastColumnName + "2");
            range.MergeCells = true;
            range.Value2 = "多条件查询统计表";

            worksheet.Cells[3, 1] = "查询条件";
            worksheet.Cells[3, 2] = "组合关系";
            worksheet.Cells[3, 3] = "查询字段";
            worksheet.Cells[3, 4] = "操作符";
            worksheet.Cells[3, 5] = "数值";
            int rowIndex = 4;
            foreach (ListViewItem lvi in this.queryFilterList.Items)
            {
                worksheet.Cells[rowIndex, 2] = lvi.Text;
                worksheet.Cells[rowIndex, 3] = lvi.SubItems[1].Text;
                worksheet.Cells[rowIndex, 4] = lvi.SubItems[2].Text;
                worksheet.Cells[rowIndex, 5] = lvi.SubItems[3].Text;
                rowIndex++;
            }
            #region 空一行然后输出查询结果的表头
            rowIndex++;
            worksheet.Cells[rowIndex, 1] = "统计结果";
            worksheet.Cells[rowIndex, 2] = "序号";
            ArrayList totalSum = new ArrayList();
            int fldCount = this.TargetLayer.FeatureClass.Fields.FieldCount;
            int columnIndex = 0;
            for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
            {
                totalSum.Add(0.0);
                IField curFld = this.TargetLayer.FeatureClass.Fields.get_Field(fldIndex);
                if (curFld.Type == esriFieldType.esriFieldTypeOID
                    || curFld.Type == esriFieldType.esriFieldTypeGeometry
                    || curFld == this.TargetLayer.FeatureClass.AreaField
                    || curFld == this.TargetLayer.FeatureClass.LengthField)
                {
                    continue;
                }
                else
                {
                    worksheet.Cells[rowIndex, columnIndex + 3] = curFld.AliasName;
                    columnIndex++;
                }

            }
            #endregion
            #region 输出查询到的数据
            rowIndex++;
            for (int itemIndex = 0; itemIndex < this.pList.Count; itemIndex++)
            {

                IFeature curFea = this.pList[itemIndex] as IFeature;
                worksheet.Cells[rowIndex, 2] = itemIndex + 1;
                fldCount = this.TargetLayer.FeatureClass.Fields.FieldCount;
                columnIndex = 0;
                for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
                {
                    #region 输出所有字段
                    IField curFld = this.TargetLayer.FeatureClass.Fields.get_Field(fldIndex);
                    if (curFld.Type == esriFieldType.esriFieldTypeOID
                        || curFld.Type == esriFieldType.esriFieldTypeGeometry
                        || curFld == this.TargetLayer.FeatureClass.AreaField
                        || curFld == this.TargetLayer.FeatureClass.LengthField)
                    {
                        continue;
                    }
                    else
                    {
                        string xStr = curFea.get_Value(fldIndex).ToString();
                        if (xStr.Trim().Equals(""))
                            xStr = "0";
                        #region 如果是数值那么看看是不是可以统计
                        if (curFld.Type == esriFieldType.esriFieldTypeDouble
                            || curFld.Type == esriFieldType.esriFieldTypeInteger
                            || curFld.Type == esriFieldType.esriFieldTypeSingle
                            || curFld.Type == esriFieldType.esriFieldTypeSmallInteger)
                        {
                            try
                            {
                                double xDbl = Double.NaN;
                                Double.TryParse(xStr, System.Globalization.NumberStyles.Any, new NumberFormatInfo(), out xDbl);
                                if (!Double.IsNaN(xDbl) && !(xDbl == 0.0))
                                {
                                    double oSum = (double)totalSum[fldIndex];
                                    oSum += xDbl;
                                    totalSum[fldIndex] = oSum;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                        }
                        #endregion
                        if (xStr != "0")
                        {
                            if (curFld.Name.Equals("ZDDJH"))
                                xStr = this.FormatLandNO(xStr);
                            worksheet.Cells[rowIndex, columnIndex + 3] = xStr;
                        }
                        columnIndex++;
                    }
                    #endregion
                }
                if (rowIndex % 2 == 0)
                {
                    range = worksheet.get_Range("B" + rowIndex, lastColumnName + rowIndex);
                    range.Interior.ColorIndex = 35;
                }
                rowIndex++;
            }
            #endregion
            //输出统计结果
            fldCount = this.TargetLayer.FeatureClass.Fields.FieldCount;
            columnIndex = 0;
            worksheet.Cells[rowIndex, 2] = "小计";
            for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
            {
                #region 输出所有字段
                IField curFld = this.TargetLayer.FeatureClass.Fields.get_Field(fldIndex);
                if (curFld.Type == esriFieldType.esriFieldTypeOID
                    || curFld.Type == esriFieldType.esriFieldTypeGeometry
                    || curFld == this.TargetLayer.FeatureClass.AreaField
                    || curFld == this.TargetLayer.FeatureClass.LengthField)
                {
                    continue;
                }
                else
                {
                    if (curFld.Type == esriFieldType.esriFieldTypeDouble
                        || curFld.Type == esriFieldType.esriFieldTypeInteger
                        || curFld.Type == esriFieldType.esriFieldTypeSingle
                        || curFld.Type == esriFieldType.esriFieldTypeSmallInteger)
                    {
                        try
                        {
                            double xDbl = (double)(totalSum[fldIndex]);
                            if (xDbl != 0.0)
                                worksheet.Cells[rowIndex, columnIndex + 3] = totalSum[fldIndex];
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    columnIndex++;
                }
                #endregion
            }
            range = worksheet.get_Range("B" + rowIndex, lastColumnName + rowIndex);
            range.Interior.ColorIndex = 46;
            objBook.Save();
            objApp.Visible = true;
            objApp = null;

            #endregion
            this.Cursor = System.Windows.Forms.Cursors.Default;
            #endregion
		}
		private String FormatLandNO(String xStr)
		{
			if(xStr.Length ==15)
			{
				string qhdm=xStr.Substring (0,2).Trim ("0".ToCharArray ());
                string jddm=xStr.Substring (2,3).Trim ("0".ToCharArray ());
				string jfdm=xStr.Substring (5,3).Trim ("0".ToCharArray ());
				string zdbh=xStr.Substring (8,4).Trim ("0".ToCharArray ());
				string zzdm=xStr.Substring (12,3).Trim ("0".ToCharArray ());
				xStr=qhdm+"/"+jddm+"/"+jfdm+"/"+zdbh+"/"+zzdm;
			}
			return xStr;
		}
		private int GetFieldCount()
		{
			int fldCount=this.TargetLayer .FeatureClass .Fields .FieldCount;
			int resultCount=0;
			for(int fldIndex=0;fldIndex<fldCount;fldIndex++)
			{
				IField curFld=this.TargetLayer .FeatureClass .Fields .get_Field (fldIndex);
				if(curFld.Type ==esriFieldType.esriFieldTypeOID
					||curFld.Type ==esriFieldType.esriFieldTypeGeometry
					||curFld==this.TargetLayer .FeatureClass .AreaField 
					||curFld==this.TargetLayer .FeatureClass .LengthField )
				{
					continue;
				}
				resultCount++;
			}
			return resultCount;
		}
		private string GetColumnName(int columnIndex)
		{
			int step=columnIndex/26;			
			if(step!=0)
			{
				char firstChar=(char)(64+step);
				char secondChar=(char)(64+columnIndex%26);
				return firstChar+""+secondChar;
			}
			else
			{
				char secondChar=(char)(64+columnIndex%26);
				return secondChar+"";
			}
		}
		private string CopyExcelTemplate()
		{
			String excelFilePath="";
			try
			{
                string templatePath = RCIS.Global.AppParameters.TemplatePath + "\\Excel\\CXJG.xls";
				if(!System.IO.File .Exists (templatePath))
				{
					MessageBox.Show ("Excel报表模版文件不存在.");
					return null;
				}
				int index=0;
                String reportFolder = RCIS.Global.AppParameters.OutputPath + "\\Excel\\行业分类统计表";
				if(!System.IO.Directory .Exists (reportFolder))
				{
					System.IO.Directory .CreateDirectory (reportFolder);
				}
				excelFilePath=reportFolder+"\\统计表"+index.ToString ()+".xls";
				while(System.IO.File.Exists (excelFilePath))
				{
					index++;
					excelFilePath=reportFolder+"\\统计表"+index.ToString ()+".xls";
				}
			
				System.IO .File .Copy (templatePath,excelFilePath,true);
			}
			catch
			{
				MessageBox.Show ("建立Excel文件出错,可能文件已经被打开,\n文件路径为:\n"+excelFilePath);
				return null;
			}
			return excelFilePath;
		}

		private void cbLayer_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(this.cbLayer .SelectedItem is LocalLayerObject )
			{
				LocalLayerObject layerObj=this.cbLayer .SelectedItem  as LocalLayerObject ;
				this.TargetLayer =layerObj.GeoLayer;
				this.FillLayerInfo ();
			}
		}
		public class ExtColumnHeader:ColumnHeader
		{
			public ExtColumnHeader(string colText,int fldIndex)
			{
				this.Text =colText;
				this.CurrentFieldIndex =fldIndex;
			}
			public int CurrentFieldIndex=-1;
		}
		public class QueryFilterItem
		{
			public bool  QueryAnd=false;
			public FieldName FieldName;
			public OpCodeItem OpCodeItem;
			public string  OpValue="";
		}
		public class FieldName
		{
			public IField TargetField;
			public override string ToString()
			{
				return this.TargetField .AliasName ;
			}		
		}
		public class OpCodeItem
		{
			public string OpName="";
			public string OpCode="";
			public OpCodeItem(string name,string code)
			{
				this.OpName =name;
				this.OpCode =code;
			}
			public override string ToString()
			{
				return this.OpName ;
			}
			public static OpCodeItem OpCodeEquals=new OpCodeItem ("等于"," = ");
			public static OpCodeItem OpCodeContains=new OpCodeItem ("包含"," like ");
			public static OpCodeItem OpCodeGreat=new OpCodeItem ("大于",">");
			public static OpCodeItem OpCodeGreatEqual=new OpCodeItem ("大于等于"," >= ");
			public static OpCodeItem OpCodeLess=new OpCodeItem ("小于","<");
			public static OpCodeItem OpCodeLessEqual=new OpCodeItem ("小于等于"," <= ");
			public static OpCodeItem OpCodeNotEqual=new OpCodeItem ("不等于"," <> ");
			public static OpCodeItem OpCodeNull=new OpCodeItem ("为空"," is null ");
			public static OpCodeItem OpCodeNotNull=new OpCodeItem ("不为空"," is not null ");
		}

        private void queryResultList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ControlStyleHelper.SortListViewColumn(sender,e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }


        }

        private void cboTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (pList == null) return;
            if (pList.Count == 0) return;
            if (this.txtFrom.Text == "")
            {
                MessageBox.Show("请输入起始条数");
                this.queryResultList.Items.Clear();
                return;
            }
            if (this.cboTo.Text == "")
            {
                MessageBox.Show("请输入终止条数");
                this.queryResultList.Items.Clear();
                return;
            }
            int sFrom =0;
            Int32.TryParse(this.txtFrom.Text, out sFrom);
            if (sFrom == 0)
            {
                MessageBox.Show("请输入大于0的数字");
                this.queryResultList.Items.Clear();
                return;
            }
            int sTo =0;
            Int32.TryParse(this.cboTo.Text, out sTo);
           
            if (sTo <= sFrom)
            {
                MessageBox.Show("起始条数不能大于终止条数");
                this.queryResultList.Items.Clear();
                return;
            }

            if (sTo > pList.Count)
            {
                sTo = pList.Count;
                return;
            }
          
            this.queryResultList.Items.Clear();
            if (pList.Count > 0)
            {
                
                    #region 显示前100条记录
                IFeature curFea = null;
                int fldCount = 0;
                for (int feaIndex = sFrom-1; feaIndex < sTo; feaIndex++)
                    {
                        curFea = pList[feaIndex] as IFeature;
                        ListViewItem lvi = new ListViewItem();
                        fldCount = this.queryResultList.Columns.Count;
                        for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
                        {
                            ExtColumnHeader curHeader = this.queryResultList.Columns[fldIndex] as ExtColumnHeader;
                            if (fldIndex == 0)
                            {
                                lvi.Text = curFea.get_Value(0).ToString();
                            }
                            else
                            {
                                lvi.SubItems.Add(curFea.get_Value(curHeader.CurrentFieldIndex).ToString());
                            }
                        }
                        if (feaIndex % 2 == 0)
                        {
                            lvi.BackColor = Color.Wheat;
                        }
                        lvi.Tag = curFea;
                        this.queryResultList.Items.Add(lvi);
                    }
                    #endregion
                
            }
             
        }

        //字段改变时，获取值
        private void cbFldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.TargetLayer == null)
                return;
            try
            {
                cbValue.Items.Clear();

                FieldName curFld = this.cbFldName.SelectedItem as FieldName;
                IFeatureClass queryClass = this.TargetLayer.FeatureClass;
                IIdentify identify = TargetLayer as IIdentify;

                IArray arIds = identify.Identify((queryClass.FeatureDataset as IGeoDataset).Extent);
                int length = 20;
                if (arIds.Count < 20)
                    length = arIds.Count;
                int idex = queryClass.FindField(curFld.TargetField.Name);
                if (idex == -1)
                    return;
                for (int i = 0; i < length; i++)
                {
                    IFeatureIdentifyObj pFeatIdObj = arIds.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRowObj = pFeatIdObj as IRowIdentifyObject;
                    IFeature pFeat = pRowObj.Row as IFeature;

                    string str = pFeat.get_Value(idex).ToString();
                    cbValue.Items.Add(str);

                }




            }
            catch 
            {
            }



        }


        /// <summary>
        /// 具体导出的方法
        /// </summary>
        /// <param name="listView">ListView</param>
        /// <param name="strFileName">导出到的文件名</param>
        private void DoExport(ListView listView, string strFileName)
        {
            int rowNum = listView.Items.Count;
            int columnNum = listView.Items[0].SubItems.Count;
            int rowIndex = 1;
            int columnIndex = 0;
            if (rowNum == 0 || string.IsNullOrEmpty(strFileName))
            {
                return;
            }
            if (rowNum > 0)
            {

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
                    return;
                }
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);
                //将ListView的列名导入Excel表第一行
                foreach (ColumnHeader dc in listView.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.Text;
                }
                //将ListView中的数据导入Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        //注意这个在导出的时候加了“\t” 的目的就是避免导出的数据显示为科学计数法。可以放在每行的首尾。
                        xlApp.Cells[rowIndex, columnIndex] = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
                    }
                }
                //例外需要说明的是用strFileName,Excel.XlFileFormat.xlExcel9795保存方式时 当你的Excel版本不是95、97 而是2003、2007 时导出的时候会报一个错误：异常来自 HRESULT:0x800A03EC。 解决办法就是换成strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal。
                xlBook.SaveAs(strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlApp = null;
                xlBook = null;
                
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "xls";
            sfd.Filter = "Excel文件(*.xls)|*.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    DoExport(this.queryResultList, sfd.FileName);
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("导出完毕！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

       
	}
	
	
}
