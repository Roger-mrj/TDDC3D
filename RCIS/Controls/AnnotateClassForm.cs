using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RCIS.Controls
{
	/// <summary>
	/// AnnotateClassForm 的摘要说明。
	/// </summary>
	public class AnnotateClassForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbClassName;
        private System.Windows.Forms.RichTextBox tbWhereClause;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AnnotateClassForm()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnnotateClassForm));
            this.tbClassName = new System.Windows.Forms.TextBox();
            this.tbWhereClause = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // tbClassName
            // 
            this.tbClassName.Location = new System.Drawing.Point(88, 16);
            this.tbClassName.Name = "tbClassName";
            this.tbClassName.Size = new System.Drawing.Size(376, 21);
            this.tbClassName.TabIndex = 0;
            // 
            // tbWhereClause
            // 
            this.tbWhereClause.Location = new System.Drawing.Point(8, 64);
            this.tbWhereClause.Name = "tbWhereClause";
            this.tbWhereClause.Size = new System.Drawing.Size(456, 96);
            this.tbWhereClause.TabIndex = 1;
            this.tbWhereClause.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "标注类名称";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(456, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "在下面输入该标注类包括的要素(SQL代码)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(304, 168);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(384, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AnnotateClassForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(472, 198);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWhereClause);
            this.Controls.Add(this.tbClassName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotateClassForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标注类属性";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion
        private string m_className="";
        public string AnnotateClassName
        {
            get
            {
                return this.m_className ;
            }
            set
            {
                if(value==null)value="";
                this.m_className =value;
            }
        }
        private string m_whereClause="";
        public string AnnotateWhereClause
        {
            get
            {
                return this.m_whereClause ;
            }
            set
            {
                if(value==null)value="";
                this.m_whereClause =value;
            }
        }
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if(this.tbClassName .Text .Trim ().Equals (""))
            {
                MessageBox.Show ("标注类的名称不能为空");
                this.tbClassName .Focus ();
            }
            this.m_className =this.tbClassName .Text.Trim () ;
            this.m_whereClause =this.tbWhereClause .Text ;
            this.DialogResult =DialogResult.OK ;
            this.Close ();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
         this.DialogResult =DialogResult.Cancel ;
            this.Close ();
        }
	}
}
