using System;
using System.Drawing;
using System.Windows.Forms;
using stdole;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

using RCIS.GISCommon;


namespace RCIS.MapTool
{
    /// <summary>
    /// InputTextForm 的摘要说明。
    /// </summary>
    public class TextElementPropertyForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RichTextBox rtbAnnoContent;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbFontName;
		private System.Windows.Forms.Label lbFontColor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbPosX;
		private System.Windows.Forms.TextBox tbPosY;
        private System.Windows.Forms.ComboBox cbFontSize;
		private System.Windows.Forms.Button btnApply;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TextElementPropertyForm()
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rtbAnnoContent = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbFontSize = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPosX = new System.Windows.Forms.TextBox();
            this.tbPosY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFontColor = new System.Windows.Forms.Label();
            this.cbFontName = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnApply);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 269);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(377, 16);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "应用";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(296, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(216, 16);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(456, 269);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbAnnoContent);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(448, 244);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtbAnnoContent
            // 
            this.rtbAnnoContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAnnoContent.Location = new System.Drawing.Point(0, 0);
            this.rtbAnnoContent.Name = "rtbAnnoContent";
            this.rtbAnnoContent.Size = new System.Drawing.Size(448, 148);
            this.rtbAnnoContent.TabIndex = 1;
            this.rtbAnnoContent.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbFontSize);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tbPosX);
            this.groupBox2.Controls.Add(this.tbPosY);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lbFontColor);
            this.groupBox2.Controls.Add(this.cbFontName);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 96);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // cbFontSize
            // 
            this.cbFontSize.Location = new System.Drawing.Point(80, 40);
            this.cbFontSize.Name = "cbFontSize";
            this.cbFontSize.Size = new System.Drawing.Size(160, 20);
            this.cbFontSize.TabIndex = 15;
            this.cbFontSize.Text = "12";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(256, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 23);
            this.label5.TabIndex = 14;
            this.label5.Text = "Y:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(256, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "X:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPosX
            // 
            this.tbPosX.Location = new System.Drawing.Point(280, 16);
            this.tbPosX.Name = "tbPosX";
            this.tbPosX.Size = new System.Drawing.Size(160, 21);
            this.tbPosX.TabIndex = 11;
            // 
            // tbPosY
            // 
            this.tbPosY.Location = new System.Drawing.Point(280, 40);
            this.tbPosY.Name = "tbPosY";
            this.tbPosY.Size = new System.Drawing.Size(160, 21);
            this.tbPosY.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "颜色";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "大小";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 25);
            this.label2.TabIndex = 8;
            this.label2.Text = "字体";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbFontColor
            // 
            this.lbFontColor.BackColor = System.Drawing.SystemColors.ControlText;
            this.lbFontColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbFontColor.Location = new System.Drawing.Point(80, 64);
            this.lbFontColor.Name = "lbFontColor";
            this.lbFontColor.Size = new System.Drawing.Size(160, 23);
            this.lbFontColor.TabIndex = 2;
            this.lbFontColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbFontColor.Click += new System.EventHandler(this.lbFontColor_Click);
            // 
            // cbFontName
            // 
            this.cbFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFontName.Location = new System.Drawing.Point(80, 16);
            this.cbFontName.MaxDropDownItems = 20;
            this.cbFontName.Name = "cbFontName";
            this.cbFontName.Size = new System.Drawing.Size(160, 20);
            this.cbFontName.TabIndex = 0;
            // 
            // TextElementPropertyForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(456, 317);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "TextElementPropertyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入文本属性";
            this.Load += new System.EventHandler(this.TextElementPropertyForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion		
        private ITextElement  m_targetElement;
		public ITextElement  TargetElement
		{
			get
			{
				return this.m_targetElement ;
			}
			set
			{
				this.m_targetElement =value;
			}
		}
		
		private IActiveView m_activeView;
		public IActiveView  ActiveView
		{
			get
			{
				return this.m_activeView ;
			}
			set
			{
				this.m_activeView =value;
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.CreateTextElement ();
			this.ActiveView .PartialRefresh (esriViewDrawPhase.esriViewGraphics ,this.TargetElement ,this.ActiveView .Extent );
            this.DialogResult = DialogResult.OK;
            this.Close ();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            this.DialogResult = DialogResult.Cancel;
		    this.Close ();
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			this.CreateTextElement ();
			this.ActiveView .PartialRefresh (esriViewDrawPhase.esriViewGraphics ,this.TargetElement ,this.ActiveView .Extent );
		}

		private void TextElementPropertyForm_Load(object sender, System.EventArgs e)
		{
			if(this.TargetElement ==null)
			{
				this.TargetElement =new TextElementClass ();
			}
			#region 处理位置
			IGeometry aLoc=(this.TargetElement as IElement).Geometry ;
			IPoint anchorPoint=null;
			IEnvelope aEnv=new EnvelopeClass();
			if(aLoc ==null&&aLoc.IsEmpty )
			{
				 aEnv=this.ActiveView .Extent ;				
			}
			else 
			{
				(this.TargetElement as IElement).QueryBounds (this.ActiveView  .ScreenDisplay ,aEnv);				
			}
			anchorPoint=new PointClass ();
			anchorPoint.PutCoords ((aEnv.XMax +aEnv.XMin)/2,(aEnv.YMax +aEnv.YMin )/2);
			this.tbPosX .Text =anchorPoint.X .ToString ();
			this.tbPosY .Text =anchorPoint.Y .ToString ();
			if(aLoc==null||aLoc.IsEmpty )
			{
				(this.TargetElement as IElement).Geometry =anchorPoint;
			}
			#endregion
			this.rtbAnnoContent .Text =this.TargetElement .Text ;
			#region 处理字体
			FontFamily[] allFamily=FontFamily.Families ;
			int fCount=allFamily.Length ;
			for(int fi=0;fi<fCount;fi++)
			{
				FontFamily aFont=allFamily[fi];				
				this.cbFontName .Items.Add (aFont.Name );
			}
			this.cbFontSize .Items .Add (4);
			this.cbFontSize .Items .Add (6);
			this.cbFontSize .Items .Add (8);
			this.cbFontSize .Items .Add (12);
			this.cbFontSize .Items .Add (16);
			this.cbFontSize .Items .Add (24);
			this.cbFontSize .Items .Add (32);
			this.cbFontSize .Items .Add (48);
			this.cbFontSize .Items .Add (64);
			this.cbFontSize .Items .Add (72);
			if(this.TargetElement .Symbol !=null)
			{
				this.cbFontSize .Text =this.TargetElement .Symbol .Size .ToString ();
				string fontName=this.TargetElement .Symbol .Font .Name ;
			    this.cbFontName .SelectedText =fontName;
			}
			#endregion
			#region 处理颜色
			ITextSymbol te=this.TargetElement .Symbol ;
			if(te!=null)
			{				
				System.Drawing .Color msColor=ColorHelper.CreateColor (te.Color);
				this.lbFontColor .BackColor =msColor;
			}
			#endregion
		}
		
		private void CreateTextElement()
		{
			try
			{
				#region 处理文本
				string annoText=this.rtbAnnoContent .Text ;
				annoText=annoText.Replace ("\n","\r\n");
				this.m_targetElement .Text=annoText ;
				#endregion
				#region 处理位置
				IGeometry aGeom=(this.m_targetElement as IElement).Geometry;
				IPoint aLoc=aGeom as IPoint ;
				if(aLoc ==null||aLoc.IsEmpty )
				{
					IEnvelope  aEnv=new EnvelopeClass ();
					(this.m_targetElement as IElement).QueryBounds (this.ActiveView .ScreenDisplay ,aEnv); 
					aLoc=new PointClass();
					aLoc.PutCoords ((aEnv.XMax +aEnv.XMin)/2,(aEnv.YMax +aEnv.YMin )/2);
				}
				double px=Convert.ToDouble (this.tbPosX .Text .Trim ());
				double py=Convert.ToDouble (this.tbPosY .Text .Trim ());
				double deltaX=px-aLoc.X ;
				double deltaY=py-aLoc.Y ;
				(this.m_targetElement as ITransform2D ).Move (deltaX,deltaY);
				#endregion
				#region 处理符号
				ITextSymbol ts=this.TargetElement .Symbol ;
				if(ts==null){ts=new TextSymbolClass() ;}
				else
				{
					ts=(ts as IClone).Clone ()as ITextSymbol ;
				}
				ts.Color =ColorHelper.CreateColor (this.lbFontColor .BackColor );
				double aSize=ts.Size ;
				if(Double.TryParse (this.cbFontSize .Text ,System.Globalization .NumberStyles .Any 
					,new System.Globalization .NumberFormatInfo ()
					,out aSize))
				{
					ts.Size =aSize;
				}

				#endregion
				#region 处理字体
				if(ts.Font !=null)
				{
					ts.Font .Name =this.cbFontName .Text ;
				}
				else
				{
					StdFontClass stdFont=new StdFontClass ();
					stdFont.Name =this.cbFontName .Text ;
					ts.Font =stdFont as IFontDisp ;
				}
				this.m_targetElement .Symbol =ts;
				#endregion
			}
			catch(Exception ex)
			{
			}
		}

		private void lbFontColor_Click(object sender, System.EventArgs e)
		{
			ColorDialog cd=new ColorDialog ();
			if(cd.ShowDialog (this)==DialogResult.OK )
			{
				this.lbFontColor .BackColor =cd.Color ;
			}
		}
		
	}
}
