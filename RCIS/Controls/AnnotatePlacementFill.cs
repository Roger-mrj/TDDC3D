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
	/// AnnotatePlacementFill 的摘要说明。
	/// </summary>
	public class AnnotatePlacementFill : System.Windows.Forms.Form
	{
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbLabelWeight;
        private System.Windows.Forms.ComboBox cbFeatureWeight;
        private System.Windows.Forms.TextBox tbLabelBuffer;
        private System.Windows.Forms.CheckBox cbPlaceOverlapLabel;
        private System.Windows.Forms.RadioButton rbHorizontal;
        private System.Windows.Forms.RadioButton rbStraight;
        private System.Windows.Forms.RadioButton rbHorizontalFirst;
        private System.Windows.Forms.CheckBox cbLabelInsidePolygon;
        private System.Windows.Forms.RadioButton rbRemoveDuplicate;
        private System.Windows.Forms.RadioButton rbLabelFeature;
        private System.Windows.Forms.RadioButton rbLabelFeaturePart;
        private System.Windows.Forms.PictureBox pbSample;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.ComponentModel.IContainer components;

		public AnnotatePlacementFill()
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbRemoveDuplicate = new System.Windows.Forms.RadioButton();
            this.rbLabelFeaturePart = new System.Windows.Forms.RadioButton();
            this.rbLabelFeature = new System.Windows.Forms.RadioButton();
            this.cbLabelInsidePolygon = new System.Windows.Forms.CheckBox();
            this.rbHorizontalFirst = new System.Windows.Forms.RadioButton();
            this.rbStraight = new System.Windows.Forms.RadioButton();
            this.rbHorizontal = new System.Windows.Forms.RadioButton();
            this.pbSample = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cbPlaceOverlapLabel = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tbLabelBuffer = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbFeatureWeight = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbLabelWeight = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSample)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 262);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(384, 262);
            this.tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.cbLabelInsidePolygon);
            this.tabPage1.Controls.Add(this.rbHorizontalFirst);
            this.tabPage1.Controls.Add(this.rbStraight);
            this.tabPage1.Controls.Add(this.rbHorizontal);
            this.tabPage1.Controls.Add(this.pbSample);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(376, 237);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "位置特征";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbRemoveDuplicate);
            this.groupBox2.Controls.Add(this.rbLabelFeaturePart);
            this.groupBox2.Controls.Add(this.rbLabelFeature);
            this.groupBox2.Location = new System.Drawing.Point(8, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 96);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "重复标注处理";
            // 
            // rbRemoveDuplicate
            // 
            this.rbRemoveDuplicate.Checked = true;
            this.rbRemoveDuplicate.Location = new System.Drawing.Point(24, 16);
            this.rbRemoveDuplicate.Name = "rbRemoveDuplicate";
            this.rbRemoveDuplicate.Size = new System.Drawing.Size(168, 24);
            this.rbRemoveDuplicate.TabIndex = 5;
            this.rbRemoveDuplicate.TabStop = true;
            this.rbRemoveDuplicate.Text = "去掉重叠的标注";
            this.rbRemoveDuplicate.CheckedChanged += new System.EventHandler(this.OnLabelNumOptionChanged);
            // 
            // rbLabelFeaturePart
            // 
            this.rbLabelFeaturePart.Location = new System.Drawing.Point(24, 64);
            this.rbLabelFeaturePart.Name = "rbLabelFeaturePart";
            this.rbLabelFeaturePart.Size = new System.Drawing.Size(168, 24);
            this.rbLabelFeaturePart.TabIndex = 7;
            this.rbLabelFeaturePart.Text = "要素的每个部分一个标注";
            this.rbLabelFeaturePart.CheckedChanged += new System.EventHandler(this.OnLabelNumOptionChanged);
            // 
            // rbLabelFeature
            // 
            this.rbLabelFeature.Location = new System.Drawing.Point(24, 40);
            this.rbLabelFeature.Name = "rbLabelFeature";
            this.rbLabelFeature.Size = new System.Drawing.Size(168, 24);
            this.rbLabelFeature.TabIndex = 6;
            this.rbLabelFeature.Text = "每个要素一个标注";
            this.rbLabelFeature.CheckedChanged += new System.EventHandler(this.OnLabelNumOptionChanged);
            // 
            // cbLabelInsidePolygon
            // 
            this.cbLabelInsidePolygon.Location = new System.Drawing.Point(8, 112);
            this.cbLabelInsidePolygon.Name = "cbLabelInsidePolygon";
            this.cbLabelInsidePolygon.Size = new System.Drawing.Size(360, 24);
            this.cbLabelInsidePolygon.TabIndex = 4;
            this.cbLabelInsidePolygon.Text = "标注总是在多边形内";
            this.cbLabelInsidePolygon.CheckedChanged += new System.EventHandler(this.cbLabelInsidePolygon_CheckedChanged);
            // 
            // rbHorizontalFirst
            // 
            this.rbHorizontalFirst.Location = new System.Drawing.Point(144, 80);
            this.rbHorizontalFirst.Name = "rbHorizontalFirst";
            this.rbHorizontalFirst.Size = new System.Drawing.Size(200, 24);
            this.rbHorizontalFirst.TabIndex = 3;
            this.rbHorizontalFirst.Text = "先尝试水平标注,然后笔直标注";
            this.rbHorizontalFirst.CheckedChanged += new System.EventHandler(this.OnPlaceLocationChanged);
            // 
            // rbStraight
            // 
            this.rbStraight.Location = new System.Drawing.Point(144, 48);
            this.rbStraight.Name = "rbStraight";
            this.rbStraight.Size = new System.Drawing.Size(104, 24);
            this.rbStraight.TabIndex = 2;
            this.rbStraight.Text = "总是笔直标注";
            this.rbStraight.CheckedChanged += new System.EventHandler(this.OnPlaceLocationChanged);
            // 
            // rbHorizontal
            // 
            this.rbHorizontal.Checked = true;
            this.rbHorizontal.Location = new System.Drawing.Point(144, 16);
            this.rbHorizontal.Name = "rbHorizontal";
            this.rbHorizontal.Size = new System.Drawing.Size(104, 24);
            this.rbHorizontal.TabIndex = 1;
            this.rbHorizontal.TabStop = true;
            this.rbHorizontal.Text = "总是水平标注";
            this.rbHorizontal.CheckedChanged += new System.EventHandler(this.OnPlaceLocationChanged);
            // 
            // pbSample
            // 
            this.pbSample.Location = new System.Drawing.Point(16, 8);
            this.pbSample.Name = "pbSample";
            this.pbSample.Size = new System.Drawing.Size(96, 96);
            this.pbSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSample.TabIndex = 0;
            this.pbSample.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbPlaceOverlapLabel);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(376, 237);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "冲突处理";
            // 
            // cbPlaceOverlapLabel
            // 
            this.cbPlaceOverlapLabel.Location = new System.Drawing.Point(8, 160);
            this.cbPlaceOverlapLabel.Name = "cbPlaceOverlapLabel";
            this.cbPlaceOverlapLabel.Size = new System.Drawing.Size(360, 24);
            this.cbPlaceOverlapLabel.TabIndex = 3;
            this.cbPlaceOverlapLabel.Text = "放置交叉的标注";
            this.cbPlaceOverlapLabel.CheckedChanged += new System.EventHandler(this.cbPlaceOverlapLabel_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tbLabelBuffer);
            this.groupBox5.Location = new System.Drawing.Point(8, 104);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(360, 48);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "标注缓冲范围";
            // 
            // tbLabelBuffer
            // 
            this.tbLabelBuffer.Location = new System.Drawing.Point(24, 17);
            this.tbLabelBuffer.Name = "tbLabelBuffer";
            this.tbLabelBuffer.Size = new System.Drawing.Size(328, 21);
            this.tbLabelBuffer.TabIndex = 0;
            this.tbLabelBuffer.Text = "0";
            this.tbLabelBuffer.TextChanged += new System.EventHandler(this.tbLabelBuffer_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbFeatureWeight);
            this.groupBox4.Location = new System.Drawing.Point(8, 56);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(360, 48);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "要素权重";
            // 
            // cbFeatureWeight
            // 
            this.cbFeatureWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFeatureWeight.Location = new System.Drawing.Point(24, 18);
            this.cbFeatureWeight.Name = "cbFeatureWeight";
            this.cbFeatureWeight.Size = new System.Drawing.Size(328, 20);
            this.cbFeatureWeight.TabIndex = 1;
            this.cbFeatureWeight.SelectedIndexChanged += new System.EventHandler(this.cbFeatureWeight_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbLabelWeight);
            this.groupBox3.Location = new System.Drawing.Point(8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(360, 48);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "标注权重";
            // 
            // cbLabelWeight
            // 
            this.cbLabelWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLabelWeight.Location = new System.Drawing.Point(24, 16);
            this.cbLabelWeight.Name = "cbLabelWeight";
            this.cbLabelWeight.Size = new System.Drawing.Size(328, 20);
            this.cbLabelWeight.TabIndex = 0;
            this.cbLabelWeight.SelectedIndexChanged += new System.EventHandler(this.cbLabelWeight_SelectedIndexChanged);
            // 
            // AnnotatePlacementFill
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(384, 310);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotatePlacementFill";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注记位置";
            this.Load += new System.EventHandler(this.AnnotatePlacementFill_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSample)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
        private IBasicOverposterLayerProperties4 m_placementProp;
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
        private void OnPlaceLocationChanged(object sender, System.EventArgs e)
        {
            if(this.rbHorizontal.Checked)
            {
                this.PlacementProperties .PolygonPlacementMethod =esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal;
                this.pbSample .Image =OtherHelper.LoadImage (System.Windows.Forms.Application.StartupPath+ @"\image\FillLabelLoc\001.bmp");
            }
            if(this.rbStraight .Checked)
            {
                this.PlacementProperties .PolygonPlacementMethod =esriOverposterPolygonPlacementMethod.esriAlwaysStraight;
                this.pbSample.Image = OtherHelper.LoadImage(System.Windows.Forms.Application.StartupPath + @"\image\FillLabelLoc\002.bmp");
            }
            if(this.rbHorizontalFirst .Checked)
            {
                this.PlacementProperties .PolygonPlacementMethod =esriOverposterPolygonPlacementMethod.esriMixedStrategy;
                this.pbSample.Image = OtherHelper.LoadImage(System.Windows.Forms.Application.StartupPath + @"\image\FillLabelLoc\003.bmp");
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

        private void AnnotatePlacementFill_Load(object sender, System.EventArgs e)
        {
            if(this.PlacementProperties !=null)
            {
                if(this.PlacementProperties .PolygonPlacementMethod==esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal)
                {
                    this.rbHorizontal .Checked =true;
                }
                else if(this.PlacementProperties .PolygonPlacementMethod ==esriOverposterPolygonPlacementMethod.esriAlwaysStraight)
                {
                    this.rbStraight .Checked =true;
                }
                else
                {
                    this.rbHorizontalFirst .Checked =true;
                }
                this.cbLabelInsidePolygon .Checked=this.PlacementProperties .PlaceOnlyInsidePolygon;
                if(this.PlacementProperties .NumLabelsOption==esriBasicNumLabelsOption.esriOneLabelPerName)
                {
                    this.rbRemoveDuplicate .Checked =true;
                }
                else if(this.PlacementProperties .NumLabelsOption ==esriBasicNumLabelsOption.esriOneLabelPerShape)
                {
                    this.rbLabelFeature .Checked=true;
                }
                else 
                {
                    this.rbLabelFeaturePart .Checked =true;
                }
                this.AddLabelWeight (esriBasicOverposterWeight.esriHighWeight,"高");
                this.AddLabelWeight (esriBasicOverposterWeight.esriMediumWeight,"中");
                this.AddLabelWeight (esriBasicOverposterWeight.esriLowWeight,"低");
                

                this.AddFeatureWeight (esriBasicOverposterWeight.esriHighWeight,"高");
                this.AddFeatureWeight (esriBasicOverposterWeight.esriMediumWeight,"中");
                this.AddFeatureWeight (esriBasicOverposterWeight.esriLowWeight,"低");
                this.AddFeatureWeight (esriBasicOverposterWeight.esriNoWeight,"无");
               
                this.cbPlaceOverlapLabel .Checked =this.PlacementProperties .GenerateUnplacedLabels;
            }
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

        private void cbLabelInsidePolygon_CheckedChanged(object sender, System.EventArgs e)
        {
            this.PlacementProperties .PlaceOnlyInsidePolygon =this.cbLabelInsidePolygon .Checked ;
        }

        private void OnLabelNumOptionChanged(object sender, System.EventArgs e)
        {
            if(this.rbRemoveDuplicate .Checked )
            {
                this.PlacementProperties .NumLabelsOption=esriBasicNumLabelsOption.esriOneLabelPerName;
            }
            else if(this.rbLabelFeature.Checked )
            {
                this.PlacementProperties .NumLabelsOption=esriBasicNumLabelsOption.esriOneLabelPerShape;
            }
            else 
            {
                this.PlacementProperties .NumLabelsOption=esriBasicNumLabelsOption.esriOneLabelPerPart;
            }
        }
    }
}
