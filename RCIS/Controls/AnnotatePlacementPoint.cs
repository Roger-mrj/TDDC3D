using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

using RCIS.Utility;

namespace RCIS.Controls
{
	/// <summary>
	/// AnnotatePlacementPoint ��ժҪ˵����
	/// </summary>
	public class AnnotatePlacementPoint : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RadioButton rbRound;
        private System.Windows.Forms.RadioButton rbAngle;
        private System.Windows.Forms.RadioButton rbOnTop;
        private System.Windows.Forms.PictureBox pbRoundSample;
        private System.Windows.Forms.TextBox teAngle;
        private System.Windows.Forms.ListView lvRoundList;
        private System.Windows.Forms.CheckBox cbPlaceOverlapLabel;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox tbLabelBuffer;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cbFeatureWeight;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbLabelWeight;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Label label1;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AnnotatePlacementPoint()
		{
			//
			// Windows ���������֧���������
			//
			InitializeComponent();

			//
			// TODO: �� InitializeComponent ���ú�����κι��캯������
			//
		}

		/// <summary>
		/// ������������ʹ�õ���Դ��
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

		#region Windows ������������ɵĴ���
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.lvRoundList = new System.Windows.Forms.ListView();
            this.teAngle = new System.Windows.Forms.TextBox();
            this.pbRoundSample = new System.Windows.Forms.PictureBox();
            this.rbOnTop = new System.Windows.Forms.RadioButton();
            this.rbAngle = new System.Windows.Forms.RadioButton();
            this.rbRound = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbPlaceOverlapLabel = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tbLabelBuffer = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbFeatureWeight = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbLabelWeight = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoundSample)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 278);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(376, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "ȡ��";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(288, 16);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "ȷ��";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(466, 278);
            this.tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lvRoundList);
            this.tabPage1.Controls.Add(this.teAngle);
            this.tabPage1.Controls.Add(this.pbRoundSample);
            this.tabPage1.Controls.Add(this.rbOnTop);
            this.tabPage1.Controls.Add(this.rbAngle);
            this.tabPage1.Controls.Add(this.rbRound);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(458, 253);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "��עλ��";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(320, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 32);
            this.label1.TabIndex = 6;
            this.label1.Text = "(����Ƕ�ʹ��Ӣ�ķֺ�;�ָ�)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvRoundList
            // 
            this.lvRoundList.HideSelection = false;
            this.lvRoundList.Location = new System.Drawing.Point(136, 24);
            this.lvRoundList.MultiSelect = false;
            this.lvRoundList.Name = "lvRoundList";
            this.lvRoundList.Size = new System.Drawing.Size(312, 152);
            this.lvRoundList.TabIndex = 5;
            this.lvRoundList.UseCompatibleStateImageBehavior = false;
            this.lvRoundList.SelectedIndexChanged += new System.EventHandler(this.lvRoundList_SelectedIndexChanged);
            // 
            // teAngle
            // 
            this.teAngle.Location = new System.Drawing.Point(136, 184);
            this.teAngle.Name = "teAngle";
            this.teAngle.Size = new System.Drawing.Size(176, 21);
            this.teAngle.TabIndex = 4;
            this.teAngle.Text = "0";
            this.teAngle.TextChanged += new System.EventHandler(this.teAngle_TextChanged);
            // 
            // pbRoundSample
            // 
            this.pbRoundSample.Location = new System.Drawing.Point(24, 40);
            this.pbRoundSample.Name = "pbRoundSample";
            this.pbRoundSample.Size = new System.Drawing.Size(100, 64);
            this.pbRoundSample.TabIndex = 3;
            this.pbRoundSample.TabStop = false;
            // 
            // rbOnTop
            // 
            this.rbOnTop.Location = new System.Drawing.Point(16, 216);
            this.rbOnTop.Name = "rbOnTop";
            this.rbOnTop.Size = new System.Drawing.Size(168, 24);
            this.rbOnTop.TabIndex = 2;
            this.rbOnTop.Text = "��ע�ڵ������";
            this.rbOnTop.CheckedChanged += new System.EventHandler(this.OnPointLocationMethod);
            // 
            // rbAngle
            // 
            this.rbAngle.Location = new System.Drawing.Point(16, 184);
            this.rbAngle.Name = "rbAngle";
            this.rbAngle.Size = new System.Drawing.Size(104, 24);
            this.rbAngle.TabIndex = 1;
            this.rbAngle.Text = "ʹ��ָ���Ƕ�";
            this.rbAngle.CheckedChanged += new System.EventHandler(this.OnPointLocationMethod);
            // 
            // rbRound
            // 
            this.rbRound.Location = new System.Drawing.Point(16, 16);
            this.rbRound.Name = "rbRound";
            this.rbRound.Size = new System.Drawing.Size(104, 24);
            this.rbRound.TabIndex = 0;
            this.rbRound.Text = "�����ڵ���Χ";
            this.rbRound.CheckedChanged += new System.EventHandler(this.OnPointLocationMethod);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbPlaceOverlapLabel);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(458, 253);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "��ͻ����";
            // 
            // cbPlaceOverlapLabel
            // 
            this.cbPlaceOverlapLabel.Location = new System.Drawing.Point(8, 152);
            this.cbPlaceOverlapLabel.Name = "cbPlaceOverlapLabel";
            this.cbPlaceOverlapLabel.Size = new System.Drawing.Size(224, 24);
            this.cbPlaceOverlapLabel.TabIndex = 7;
            this.cbPlaceOverlapLabel.Text = "���ý����ע";
            this.cbPlaceOverlapLabel.CheckedChanged += new System.EventHandler(this.cbPlaceOverlapLabel_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tbLabelBuffer);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Location = new System.Drawing.Point(0, 96);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(458, 48);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "��ע����";
            // 
            // tbLabelBuffer
            // 
            this.tbLabelBuffer.Location = new System.Drawing.Point(16, 17);
            this.tbLabelBuffer.Name = "tbLabelBuffer";
            this.tbLabelBuffer.Size = new System.Drawing.Size(424, 21);
            this.tbLabelBuffer.TabIndex = 0;
            this.tbLabelBuffer.Text = "0";
            this.tbLabelBuffer.TextChanged += new System.EventHandler(this.tbLabelBuffer_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbFeatureWeight);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(0, 48);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(458, 48);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ҫ��Ȩ��";
            // 
            // cbFeatureWeight
            // 
            this.cbFeatureWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFeatureWeight.Location = new System.Drawing.Point(16, 22);
            this.cbFeatureWeight.Name = "cbFeatureWeight";
            this.cbFeatureWeight.Size = new System.Drawing.Size(432, 20);
            this.cbFeatureWeight.TabIndex = 1;
            this.cbFeatureWeight.SelectedIndexChanged += new System.EventHandler(this.cbFeatureWeight_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbLabelWeight);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(458, 48);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "��עȨ��";
            // 
            // cbLabelWeight
            // 
            this.cbLabelWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLabelWeight.Location = new System.Drawing.Point(16, 16);
            this.cbLabelWeight.Name = "cbLabelWeight";
            this.cbLabelWeight.Size = new System.Drawing.Size(432, 20);
            this.cbLabelWeight.TabIndex = 0;
            this.cbLabelWeight.SelectedIndexChanged += new System.EventHandler(this.cbLabelWeight_SelectedIndexChanged);
            // 
            // AnnotatePlacementPoint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(466, 326);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotatePlacementPoint";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "���עλ��";
            this.Load += new System.EventHandler(this.AnnotatePlacementPoint_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRoundSample)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
        private IBasicOverposterLayerProperties4 m_placementProp;
        private bool  m_shouldAction=true;
        public IBasicOverposterLayerProperties4 PlacementProperties
        {
            get
            {
                if(this.m_placementProp ==null)
                    this.m_placementProp =new BasicOverposterLayerPropertiesClass();
                return this.m_placementProp ;
            }
            set
            {
                if(value !=null)
                {
                    this.m_placementProp =(value as IClone).Clone () as IBasicOverposterLayerProperties4;
                }
            }
        }
        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult =DialogResult.OK ;
            this.Close ();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult =DialogResult.Cancel ;
            this.Close ();
        }

        private void AnnotatePlacementPoint_Load(object sender, System.EventArgs e)
        {
            this.m_shouldAction =false;
            #region ��ע����
            #region ����ͼƬ
            ImageList imageList=new ImageList ();
            imageList.ImageSize =new Size (48,48);
            imageList.Images .Add (OtherHelper.LoadImage ("\\PointLabelLoc\\001.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\002.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\003.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\004.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\005.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\006.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\007.bmp"));
            imageList.Images.Add(OtherHelper.LoadImage("\\PointLabelLoc\\008.bmp"));
            this.lvRoundList .LargeImageList =imageList;
            #endregion
            #region ���Ͻ�
            PointPlacementPrioritiesClass ptPri=new PointPlacementPrioritiesClass ();
            ptPri.AboveRight =1;
            ListViewItem lvi=new ListViewItem ("���Ͻ�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =0;
            lvi.StateImageIndex =0;
           
            this.lvRoundList .Items.Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region ���Ϸ�
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.AboveCenter =1;
            lvi=new ListViewItem ("���Ϸ�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =1;
            lvi.StateImageIndex =1;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region ���Ͻ�
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.AboveLeft=1;
            lvi=new ListViewItem ("���Ͻ�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =2;
            lvi.StateImageIndex =2;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region ���Ҳ�
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.CenterRight=1;
            lvi=new ListViewItem ("���Ҳ�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =3;
            lvi.StateImageIndex =3;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region ���·�
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.BelowRight=1;
            lvi=new ListViewItem ("���·�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =4;
            lvi.StateImageIndex =4;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region ���·�
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.BelowCenter=1;
            lvi=new ListViewItem ("���·�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =5;
            lvi.StateImageIndex =5;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region ���·�
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.BelowLeft=1;
            lvi=new ListViewItem ("���·�");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =6;
            lvi.StateImageIndex =6;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #region �����
            ptPri=new PointPlacementPrioritiesClass ();
            ptPri.CenterLeft=1;
            lvi=new ListViewItem ("�����");
            lvi.Tag =ptPri;           
            lvi.ImageIndex =7;
            lvi.StateImageIndex =7;
            this.lvRoundList .Items .Add (lvi);
            if(this.PlacementProperties .PointPlacementPriorities .Equals (ptPri))
                lvi.Selected =true;
            #endregion
            #endregion           
            #region ����Ƕ�            
            object angles=this.PlacementProperties .PointPlacementAngles;
            string angleStr="";
            if(angles!=null)
            {
                if(angles is double[])
                {
                    double[] angleAry=angles as double[];
                    for(int i=0;i<angleAry.Length ;i++)
                    {
                        angleStr+=angleAry[i]+";";
                    }
                    if(angleStr.Length >0)angleStr=angleStr.Substring (0,angleStr.Length -1);
                
                    this.teAngle .Text =angleStr;
                }
            }
            if(!this.teAngle .Text .Equals (""))
            {
                this.rbAngle .Checked =true;
            }
            else if(this.PlacementProperties .PointPlacementOnTop )
            {
                this.rbOnTop .Checked =true;
            }
            else 
            {
                this.rbRound .Checked=true;
            }
            #endregion
            #region ��ͻ
            this.AddLabelWeight (esriBasicOverposterWeight.esriHighWeight,"��");
            this.AddLabelWeight (esriBasicOverposterWeight.esriMediumWeight,"��");
            this.AddLabelWeight (esriBasicOverposterWeight.esriLowWeight,"��");                

            this.AddFeatureWeight (esriBasicOverposterWeight.esriHighWeight,"��");
            this.AddFeatureWeight (esriBasicOverposterWeight.esriMediumWeight,"��");
            this.AddFeatureWeight (esriBasicOverposterWeight.esriLowWeight,"��");
            this.AddFeatureWeight (esriBasicOverposterWeight.esriNoWeight,"��");
            this.tbLabelBuffer .Text =this.PlacementProperties.BufferRatio.ToString ();

            this.cbPlaceOverlapLabel .Checked =this.PlacementProperties .GenerateUnplacedLabels;
            #endregion
            this.m_shouldAction =true;
        }
        private void AddLabelWeight(esriBasicOverposterWeight labelWeight,string weightText)
        {
            ComboBoxItem cbi=new ComboBoxItem (labelWeight,weightText,this.cbLabelWeight .Items.Count+1);
            this.cbLabelWeight .Items.Add (cbi);
            if(this.PlacementProperties .LabelWeight ==labelWeight)this.cbLabelWeight .SelectedItem=cbi;
        }
        private void AddFeatureWeight(esriBasicOverposterWeight feaWeight,string weightText)
        {
            ComboBoxItem cbi=new ComboBoxItem (feaWeight,weightText,this.cbFeatureWeight  .Items.Count+1);
            this.cbFeatureWeight .Items.Add (cbi);
            if(this.PlacementProperties .FeatureWeight ==feaWeight)this.cbFeatureWeight .SelectedItem =cbi;
        }
        private void lvRoundList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(this.lvRoundList .SelectedItems .Count >0)
            {
                ListViewItem lvi=this.lvRoundList .SelectedItems [0];
                this.pbRoundSample .Image =this.lvRoundList .LargeImageList .Images[lvi.ImageIndex];
                this.PlacementProperties .PointPlacementPriorities =lvi.Tag as IPointPlacementPriorities ;
            }
        }
        private void cbLabelWeight_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                ComboBoxItem cbi=this.cbLabelWeight .SelectedItem as ComboBoxItem ;
                if(cbi!=null)
                {
                    this.PlacementProperties .LabelWeight =(esriBasicOverposterWeight)cbi.ItemObject ;
                }
            }
            catch
            {
            }
        }

        private void cbFeatureWeight_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                ComboBoxItem cbi=this.cbFeatureWeight .SelectedItem as ComboBoxItem ;
                if(cbi!=null)
                {
                    this.PlacementProperties .FeatureWeight =(esriBasicOverposterWeight)cbi.ItemObject ;
                }
            }
            catch
            {
            }
        }

        private void tbLabelBuffer_TextChanged(object sender, System.EventArgs e)
        {
            double buffer=0;
            if(Double.TryParse (this.tbLabelBuffer .Text .Trim (),System.Globalization .NumberStyles .Any 
                ,new System.Globalization .NumberFormatInfo (),out buffer))
            {
                this.PlacementProperties .BufferRatio=buffer;
            }
        }

        private void cbPlaceOverlapLabel_CheckedChanged(object sender, System.EventArgs e)
        {
            this.PlacementProperties .GenerateUnplacedLabels =this.cbPlaceOverlapLabel .Checked;
        }

        private void OnPointLocationMethod(object sender, System.EventArgs e)
        {
            if(this.rbRound .Checked)
            {
                this.lvRoundList .Enabled =true;
                this.teAngle .Enabled =false;
                this.PlacementProperties .PointPlacementOnTop =false;
                this.PlacementProperties .PointPlacementAngles =new double [0];
            }
            else if(this.rbOnTop.Checked )
            {
                this.lvRoundList .Enabled =false;
                this.PlacementProperties .PointPlacementOnTop =true;
                this.teAngle .Enabled =false;
                this.PlacementProperties .PointPlacementAngles =new double [0];
            }
            else if(this.rbAngle .Checked)
            {
                this.lvRoundList .Enabled =false;
                this.teAngle .Enabled =true;
                this.PlacementProperties .PointPlacementOnTop =true;                
                this.ReadPlacementAngle ();
            }
        }
        private void ReadPlacementAngle()
        {
            double[] angleAry=null;
            try
            {
                string[] angleList=this.teAngle .Text .Trim ().Split (";".ToCharArray ());
                angleAry=new double[angleList.Length ];
                for(int ai=0;ai<angleList.Length ;ai++)
                {
                    double angle=0;
                    string angleStr=angleList[ai];
                    double.TryParse (angleStr,
                        System.Globalization .NumberStyles .Any 
                        ,new System.Globalization .NumberFormatInfo ()
                        ,out angle);
                    angleAry[ai]=angle;
                }   
            }
            catch
            {
                angleAry=null;
  
                MessageBox.Show( "�ڽ����Ƕȵ�ʱ��������");
            }
            if(angleAry!=null)
            {
                try
                {
                    this.PlacementProperties .PointPlacementAngles=angleAry;
                }
                catch
                {
                    //RCIS.Helper.ErrorHelper.ShowErrorForm(ex, "�ڽ����Ƕȵ�ʱ��������");
                }
            }
        }

        private void teAngle_TextChanged(object sender, System.EventArgs e)
        {
            this.ReadPlacementAngle();
        }
       
	}
}
