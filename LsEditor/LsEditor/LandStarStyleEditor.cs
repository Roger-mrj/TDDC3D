using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using RCIS.Style.StyleEditor;
namespace RCIS.Style
{
	/// <summary>
	/// ServerStyleEditor 的摘要说明。
    /// 2007/12/22 zhuliangxiong@hotmail.com
    ///   The design of the style editor contains:
    /// 1.The ServerStyleEditor is the top most class for client to
    /// use.When the client use this class to create a new style,it should
    /// provider a style class string. If the client use this class to edit
    /// a style,it should provider the existed style.
    /// 
    /// 2. 2.1)When the ServerStyleEditor get a style class string ,it should create 
    /// a default style. Or the client give the ServerStyleEditor an existed style.
    /// That style will be called EditedStyle;
    ///    2.2)ServerStyleEditor will not edit a style
    /// itself,but give it to a StyleLayerEditor;
    ///    2.3)The StyleLayedEditor will break the style
    /// down to a list of style layer which hava the same Style Class. and then send the selected 
    /// style layer to StyleEditorContentControl;
    ///    2.4)The StyleEditorContentControl will use the style
    /// layer to select a concrete style editor.
    ///    2.5)the concrete can change the property of style layer.
    /// 
    /// 3  3.1)when the concrete style editor change the properties of a style,it will fire an event;
    ///    3.2)the StyleEditorContentControl will accept this event,and fire a event too;
    ///    3.3)the ServerStyleEditor will accept the event from StyleEditorContentControl and update the
    /// current layer in StyleLayerEditor .
    ///    3.4)the change happens in the StyleLayerEditor will cause an event to be fired.
    ///    3.5)the ServerStyleEditor will got this event and get a Style object from the StyleLayerEditor
    /// and give it to the StylePreviewControl.
    ///    3.6)The StylePreviewControl will display it.
    /// 
    /// 
    /// 
	/// </summary>
	public class ServerStyleEditor : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Panel plLeft;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox gbPreview;
        private GroupBox gbLayer;
        private GroupBox gbAction;
        private Button btnCancel;
        private Button btnOK;
        private Panel plEditor;
        private ToolTip hintTool;
        private IContainer components;

		public ServerStyleEditor()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

            this.m_pPreControl = new StylePreviewControl();
            this.m_pPreControl.Dock = DockStyle.Fill;
            this.gbPreview.Controls.Add(this.m_pPreControl);

            this.m_pLayerEditor = new StyleLayerEditor();
            this.m_pLayerEditor.Dock = DockStyle.Fill;
            this.gbLayer.Controls.Add(this.m_pLayerEditor);
            this.m_pLayerEditor.OnCurrentEditedLayerChanged += new EditedStyleLayerChangedEventHandler(OnCurrentEditedLayerChanged);
            this.m_pLayerEditor.OnEditedStyleChanged += new EditedStyleChangedEventHandler(OnEditedStyleChanged);
            this.m_pStyleEditor = new StyleEditorContentControl();
            this.m_pStyleEditor.Dock = DockStyle.Fill;
            this.plEditor.Controls.Add(this.m_pStyleEditor);
            this.m_pStyleEditor.OnEditedStyleChanged += new EditedStyleChangedEventHandler(OnEditedStyleChanged);
		}

        void OnEditedStyleChanged(object pSender, EventArgs pArg)
        {
            if (pSender == this.m_pLayerEditor)
            {
                ISymbol aResultStyle = this.m_pLayerEditor.ResultStyle;
                this.m_pPreControl.PreviewedStyle = aResultStyle;
                this.m_pEditedStyle = aResultStyle;
            }
            else if (pSender == this.m_pStyleEditor)
            {
                if (this.m_pStyleEditor != null)
                {
                    IStyleEditor aCurEditor = this.m_pStyleEditor.CurrentStyleEditor;
                    if (aCurEditor != null)
                    {
                        this.m_pLayerEditor.UpdateStyleLayer(aCurEditor.EditedStyle);
                    }
                }
            }
        }

        void OnCurrentEditedLayerChanged(object pSender, EventArgs pArg)
        {
            ISymbol aCurSym = this.m_pLayerEditor.CurrentStyleLayer;
            this.m_pStyleEditor.InitializeStyleEditor(aCurSym);
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
            this.plLeft = new System.Windows.Forms.Panel();
            this.gbLayer = new System.Windows.Forms.GroupBox();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gbAction = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.plEditor = new System.Windows.Forms.Panel();
            this.hintTool = new System.Windows.Forms.ToolTip();
            this.plLeft.SuspendLayout();
            this.gbAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // plLeft
            // 
            this.plLeft.Controls.Add(this.gbLayer);
            this.plLeft.Controls.Add(this.gbPreview);
            this.plLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.plLeft.Location = new System.Drawing.Point(0, 0);
            this.plLeft.Name = "plLeft";
            this.plLeft.Size = new System.Drawing.Size(240, 566);
            this.plLeft.TabIndex = 0;
            this.plLeft.Resize += new System.EventHandler(this.plLeft_Resize);
            // 
            // gbLayer
            // 
            this.gbLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLayer.Location = new System.Drawing.Point(0, 226);
            this.gbLayer.Name = "gbLayer";
            this.gbLayer.Size = new System.Drawing.Size(240, 340);
            this.gbLayer.TabIndex = 1;
            this.gbLayer.TabStop = false;
            this.gbLayer.Text = "图层";
            // 
            // gbPreview
            // 
            this.gbPreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPreview.Location = new System.Drawing.Point(0, 0);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Size = new System.Drawing.Size(240, 226);
            this.gbPreview.TabIndex = 0;
            this.gbPreview.TabStop = false;
            this.gbPreview.Text = "预览";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Control;
            this.splitter1.Location = new System.Drawing.Point(240, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 566);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // gbAction
            // 
            this.gbAction.Controls.Add(this.btnCancel);
            this.gbAction.Controls.Add(this.btnOK);
            this.gbAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbAction.Location = new System.Drawing.Point(245, 519);
            this.gbAction.Name = "gbAction";
            this.gbAction.Size = new System.Drawing.Size(547, 47);
            this.gbAction.TabIndex = 3;
            this.gbAction.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(439, 13);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 29);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(319, 13);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 29);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.hintTool.SetToolTip(this.btnOK, "确认符号编辑结束.点击这个按钮将关闭符号编辑器");
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnOK);
            // 
            // plEditor
            // 
            this.plEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plEditor.Location = new System.Drawing.Point(245, 0);
            this.plEditor.Name = "plEditor";
            this.plEditor.Size = new System.Drawing.Size(547, 519);
            this.plEditor.TabIndex = 4;
            // 
            // hintTool
            // 
            this.hintTool.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ServerStyleEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.plEditor);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.plLeft);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerStyleEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "符号编辑器";
            this.plLeft.ResumeLayout(false);
            this.gbAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
        private StylePreviewControl m_pPreControl;
        private StyleLayerEditor m_pLayerEditor;
        private StyleEditorContentControl m_pStyleEditor;
        private ISymbol m_pEditedStyle;
        public ISymbol EditedStyle
        {
            get
            {
                return this.m_pEditedStyle;
            }
            set
            {
                this.m_pEditedStyle = value;
                this.m_pLayerEditor.EditedStyle = value;
                this.m_pPreControl.PreviewedStyle = value;
            }
        }
        
        private const int MinLeftPanelWidth = 180;
        private void plLeft_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (this.plLeft.Width < MinLeftPanelWidth)
                {
                    this.plLeft.Width = MinLeftPanelWidth;
                }
            }
        }

        #region OK/Cancel
        private void OnOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }       

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
