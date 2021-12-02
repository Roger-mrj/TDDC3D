using System;
using System.Drawing;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.Controls
{
	/// <summary>
	/// AnnotateExpressForm 的摘要说明。
	/// </summary>
	public class AnnotateExpressForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnValidate;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.ListView lvFieldList;
        private System.Windows.Forms.RichTextBox tbExpress;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ComboBox cbScriptEngine;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AnnotateExpressForm()
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
            this.label1 = new System.Windows.Forms.Label();
            this.lvFieldList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.cbScriptEngine = new System.Windows.Forms.ComboBox();
            this.btnValidate = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.tbExpress = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(480, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "下面是所有可用来标注的字段";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvFieldList
            // 
            this.lvFieldList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvFieldList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvFieldList.FullRowSelect = true;
            this.lvFieldList.GridLines = true;
            this.lvFieldList.HideSelection = false;
            this.lvFieldList.Location = new System.Drawing.Point(0, 23);
            this.lvFieldList.Name = "lvFieldList";
            this.lvFieldList.Size = new System.Drawing.Size(480, 97);
            this.lvFieldList.TabIndex = 2;
            this.lvFieldList.UseCompatibleStateImageBehavior = false;
            this.lvFieldList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "次序";
            this.columnHeader1.Width = 42;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "字段名称";
            this.columnHeader2.Width = 78;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "字段别名";
            this.columnHeader3.Width = 124;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "字段类型";
            this.columnHeader4.Width = 149;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(480, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "在下面写一个名为FindLabel([字段名1],[字段名2]...)的函数";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbScriptEngine
            // 
            this.cbScriptEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbScriptEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScriptEngine.Location = new System.Drawing.Point(8, 288);
            this.cbScriptEngine.Name = "cbScriptEngine";
            this.cbScriptEngine.Size = new System.Drawing.Size(216, 20);
            this.cbScriptEngine.TabIndex = 4;
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidate.Location = new System.Drawing.Point(232, 288);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(75, 23);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "验证";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(312, 288);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(392, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbExpress
            // 
            this.tbExpress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExpress.HideSelection = false;
            this.tbExpress.Location = new System.Drawing.Point(0, 144);
            this.tbExpress.Name = "tbExpress";
            this.tbExpress.Size = new System.Drawing.Size(472, 136);
            this.tbExpress.TabIndex = 8;
            this.tbExpress.Text = "";
            // 
            // AnnotateExpressForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(480, 318);
            this.Controls.Add(this.tbExpress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.cbScriptEngine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvFieldList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotateExpressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标注表达式";
            this.Load += new System.EventHandler(this.AnnotateExpressForm_Load);
            this.ResumeLayout(false);

        }
		#endregion
        private IGeoFeatureLayer m_targetLayer;
        private String           m_annoExpress="";
        public IGeoFeatureLayer FeatureLayer
        {
            set
            {
                this.m_targetLayer =value;
            }
            get
            {
                return this.m_targetLayer ;
            }
        }
        public string AnnotateExpress
        {
            get
            {
                return this.m_annoExpress ;
            }
            set
            {
                if(value==null)value="";
                this.m_annoExpress =value;
            }
        }
        private void AnnotateExpressForm_Load(object sender, System.EventArgs e)
        {
            if(this.FeatureLayer !=null)
            {
                IFeatureClass curClass=this.FeatureLayer .FeatureClass ;
                int fldCount=curClass.Fields .FieldCount;
                for(int fi=0;fi<fldCount;fi++)
                {
                    IField curField=curClass.Fields .get_Field (fi);
                    ListViewItem lvi=new ListViewItem ((fi+1).ToString());
                    lvi.SubItems .Add (curField.Name );
                    lvi.SubItems .Add (curField.AliasName );
                    lvi.SubItems .Add (DatabaseHelper.QueryFieldTypeName (curField.Type ));
                    this.lvFieldList .Items.Add (lvi);
                    if(fi==1)lvi.Selected =true;
                    if(fi%2==0)lvi.BackColor =Color.Wheat ;
                }                
            }
            ComboBoxItem cbiJS=new ComboBoxItem ("JS:","使用JavaScript脚本",1);
            ComboBoxItem cbiVB=new ComboBoxItem ("VB:","使用VBScript脚本",2);
            this.cbScriptEngine .Items.Add (cbiJS);
            this.cbScriptEngine .Items .Add (cbiVB);
            this.cbScriptEngine .SelectedIndex =1;
            if(this.m_annoExpress .StartsWith ("JS:"))
            {
                this.cbScriptEngine .SelectedIndex =0;
            }
            else if(this.m_annoExpress .StartsWith ("VB:"))
            {
                this.cbScriptEngine .SelectedIndex =1;
            }
            this.tbExpress .Text =this.m_annoExpress .Substring (3);
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if(!this.ValidateExpress ())
            {
                DialogResult dr=MessageBox.Show ("表达式错误!,是否继续?","验证表达式",MessageBoxButtons.YesNo ,MessageBoxIcon.Question );
                if(DialogResult.No ==dr)
                {//不继续 就返回
                    return;
                }
            }
            string exp=this.tbExpress .Text ;
            if(this.cbScriptEngine .SelectedIndex ==0)this.m_annoExpress ="JS:"+exp;
            else if(this.cbScriptEngine .SelectedIndex ==1)this.m_annoExpress ="VB:"+exp;
            this.DialogResult =DialogResult.OK ;
            this.Close ();
        }
        private bool ValidateExpress()
        {
            return true;
        }
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult =DialogResult.Cancel ;
            this.Close ();
        }

        private void btnValidate_Click(object sender, System.EventArgs e)
        { 
            MessageBox.Show ("该功能还没有实现");
        }
    
        
	}
}
