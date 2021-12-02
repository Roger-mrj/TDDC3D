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
	/// AnnotatePlacementLine ��ժҪ˵����
	/// </summary>
    public class AnnotatePlacementLine : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLabelWeight;
        private System.Windows.Forms.ComboBox cbFeatureWeight;
        private System.Windows.Forms.TextBox tbLabelBuffer;
        private System.Windows.Forms.CheckBox cbPlaceOverlapLabel;
        private System.Windows.Forms.RadioButton rbOrientHorizontal;
        private System.Windows.Forms.TextBox tbOffset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbOrientCurve;
        private System.Windows.Forms.RadioButton rbOrientParallel;
        private System.Windows.Forms.RadioButton rbOrientPerpendicular;
        private System.Windows.Forms.RadioButton rbRemoveDuplicate;
        private System.Windows.Forms.RadioButton rbLabelFeaturePart;
        private System.Windows.Forms.RadioButton rbLabelFeature;
        private System.Windows.Forms.ComboBox cbOrientSys;
        private System.Windows.Forms.PictureBox pbSample;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnPriority;
        private System.Windows.Forms.ComboBox cbLinePosition;
        private System.Windows.Forms.CheckBox cbAbove;
        private System.Windows.Forms.CheckBox cbOnTheLine;
        private System.Windows.Forms.CheckBox cbUnderline;
        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AnnotatePlacementLine()
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
            this.btnPriority = new DevExpress.XtraEditors.SimpleButton();
            this.cbLinePosition = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbUnderline = new System.Windows.Forms.CheckBox();
            this.cbOnTheLine = new System.Windows.Forms.CheckBox();
            this.cbAbove = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbOffset = new System.Windows.Forms.TextBox();
            this.pbSample = new System.Windows.Forms.PictureBox();
            this.cbOrientSys = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbOrientHorizontal = new System.Windows.Forms.RadioButton();
            this.rbOrientCurve = new System.Windows.Forms.RadioButton();
            this.rbOrientParallel = new System.Windows.Forms.RadioButton();
            this.rbOrientPerpendicular = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbRemoveDuplicate = new System.Windows.Forms.RadioButton();
            this.rbLabelFeaturePart = new System.Windows.Forms.RadioButton();
            this.rbLabelFeature = new System.Windows.Forms.RadioButton();
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
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSample)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.groupBox1.Location = new System.Drawing.Point(0, 304);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 48);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "ȡ��";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(208, 16);
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
            this.tabControl.Size = new System.Drawing.Size(394, 304);
            this.tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnPriority);
            this.tabPage1.Controls.Add(this.cbLinePosition);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(386, 279);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "��עλ��";
            // 
            // btnPriority
            // 
            this.btnPriority.Location = new System.Drawing.Point(280, 160);
            this.btnPriority.Name = "btnPriority";
            this.btnPriority.Size = new System.Drawing.Size(96, 23);
            this.btnPriority.TabIndex = 6;
            this.btnPriority.Text = "�������ȼ�";
            this.btnPriority.Click += new System.EventHandler(this.btnPriority_Click);
            // 
            // cbLinePosition
            // 
            this.cbLinePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLinePosition.Location = new System.Drawing.Point(112, 160);
            this.cbLinePosition.Name = "cbLinePosition";
            this.cbLinePosition.Size = new System.Drawing.Size(160, 20);
            this.cbLinePosition.TabIndex = 5;
            this.cbLinePosition.SelectedIndexChanged += new System.EventHandler(this.cbLinePosition_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "�������ϵ�λ��";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbUnderline);
            this.groupBox4.Controls.Add(this.cbOnTheLine);
            this.groupBox4.Controls.Add(this.cbAbove);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.tbOffset);
            this.groupBox4.Controls.Add(this.pbSample);
            this.groupBox4.Controls.Add(this.cbOrientSys);
            this.groupBox4.Location = new System.Drawing.Point(160, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(226, 151);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "��עλ��";
            // 
            // cbUnderline
            // 
            this.cbUnderline.Location = new System.Drawing.Point(8, 88);
            this.cbUnderline.Name = "cbUnderline";
            this.cbUnderline.Size = new System.Drawing.Size(104, 24);
            this.cbUnderline.TabIndex = 8;
            this.cbUnderline.Text = "����";
            this.cbUnderline.CheckedChanged += new System.EventHandler(this.OnLinePositionChanged);
            // 
            // cbOnTheLine
            // 
            this.cbOnTheLine.Location = new System.Drawing.Point(8, 63);
            this.cbOnTheLine.Name = "cbOnTheLine";
            this.cbOnTheLine.Size = new System.Drawing.Size(104, 24);
            this.cbOnTheLine.TabIndex = 7;
            this.cbOnTheLine.Text = "����";
            this.cbOnTheLine.CheckedChanged += new System.EventHandler(this.OnLinePositionChanged);
            // 
            // cbAbove
            // 
            this.cbAbove.Location = new System.Drawing.Point(8, 40);
            this.cbAbove.Name = "cbAbove";
            this.cbAbove.Size = new System.Drawing.Size(104, 24);
            this.cbAbove.TabIndex = 6;
            this.cbAbove.Text = "����";
            this.cbAbove.CheckedChanged += new System.EventHandler(this.OnLinePositionChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(128, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "(��ͼ���ݵ�λ)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "ƫ��";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "����ϵͳ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(40, 120);
            this.tbOffset.Name = "tbOffset";
            this.tbOffset.Size = new System.Drawing.Size(88, 21);
            this.tbOffset.TabIndex = 2;
            this.tbOffset.Text = "0";
            this.tbOffset.TextChanged += new System.EventHandler(this.tbOffset_TextChanged);
            // 
            // pbSample
            // 
            this.pbSample.Location = new System.Drawing.Point(120, 40);
            this.pbSample.Name = "pbSample";
            this.pbSample.Size = new System.Drawing.Size(80, 72);
            this.pbSample.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSample.TabIndex = 1;
            this.pbSample.TabStop = false;
            // 
            // cbOrientSys
            // 
            this.cbOrientSys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrientSys.Location = new System.Drawing.Point(72, 16);
            this.cbOrientSys.Name = "cbOrientSys";
            this.cbOrientSys.Size = new System.Drawing.Size(136, 20);
            this.cbOrientSys.TabIndex = 0;
            this.cbOrientSys.SelectedIndexChanged += new System.EventHandler(this.cbOrientSys_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbOrientHorizontal);
            this.groupBox3.Controls.Add(this.rbOrientCurve);
            this.groupBox3.Controls.Add(this.rbOrientParallel);
            this.groupBox3.Controls.Add(this.rbOrientPerpendicular);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(160, 151);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "��ע����";
            // 
            // rbOrientHorizontal
            // 
            this.rbOrientHorizontal.Location = new System.Drawing.Point(32, 24);
            this.rbOrientHorizontal.Name = "rbOrientHorizontal";
            this.rbOrientHorizontal.Size = new System.Drawing.Size(104, 24);
            this.rbOrientHorizontal.TabIndex = 6;
            this.rbOrientHorizontal.Text = "ˮƽ";
            this.rbOrientHorizontal.CheckedChanged += new System.EventHandler(this.OnOrientChanged);
            // 
            // rbOrientCurve
            // 
            this.rbOrientCurve.Location = new System.Drawing.Point(32, 72);
            this.rbOrientCurve.Name = "rbOrientCurve";
            this.rbOrientCurve.Size = new System.Drawing.Size(104, 24);
            this.rbOrientCurve.TabIndex = 8;
            this.rbOrientCurve.Text = "����";
            this.rbOrientCurve.CheckedChanged += new System.EventHandler(this.OnOrientChanged);
            // 
            // rbOrientParallel
            // 
            this.rbOrientParallel.Location = new System.Drawing.Point(32, 48);
            this.rbOrientParallel.Name = "rbOrientParallel";
            this.rbOrientParallel.Size = new System.Drawing.Size(104, 24);
            this.rbOrientParallel.TabIndex = 7;
            this.rbOrientParallel.Text = "ƽ��";
            this.rbOrientParallel.CheckedChanged += new System.EventHandler(this.OnOrientChanged);
            // 
            // rbOrientPerpendicular
            // 
            this.rbOrientPerpendicular.Location = new System.Drawing.Point(32, 96);
            this.rbOrientPerpendicular.Name = "rbOrientPerpendicular";
            this.rbOrientPerpendicular.Size = new System.Drawing.Size(104, 24);
            this.rbOrientPerpendicular.TabIndex = 3;
            this.rbOrientPerpendicular.Text = "��ֱ";
            this.rbOrientPerpendicular.CheckedChanged += new System.EventHandler(this.OnOrientChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbRemoveDuplicate);
            this.groupBox2.Controls.Add(this.rbLabelFeaturePart);
            this.groupBox2.Controls.Add(this.rbLabelFeature);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(386, 96);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "�ظ���ע����";
            // 
            // rbRemoveDuplicate
            // 
            this.rbRemoveDuplicate.Location = new System.Drawing.Point(24, 16);
            this.rbRemoveDuplicate.Name = "rbRemoveDuplicate";
            this.rbRemoveDuplicate.Size = new System.Drawing.Size(224, 24);
            this.rbRemoveDuplicate.TabIndex = 3;
            this.rbRemoveDuplicate.Text = "ȥ���ظ��ı�ע";
            this.rbRemoveDuplicate.CheckedChanged += new System.EventHandler(this.OnDuplicateLabelChanged);
            // 
            // rbLabelFeaturePart
            // 
            this.rbLabelFeaturePart.Location = new System.Drawing.Point(24, 64);
            this.rbLabelFeaturePart.Name = "rbLabelFeaturePart";
            this.rbLabelFeaturePart.Size = new System.Drawing.Size(216, 24);
            this.rbLabelFeaturePart.TabIndex = 5;
            this.rbLabelFeaturePart.Text = "Ҫ�ص�ÿ������һ����ע";
            this.rbLabelFeaturePart.CheckedChanged += new System.EventHandler(this.OnDuplicateLabelChanged);
            // 
            // rbLabelFeature
            // 
            this.rbLabelFeature.Location = new System.Drawing.Point(24, 40);
            this.rbLabelFeature.Name = "rbLabelFeature";
            this.rbLabelFeature.Size = new System.Drawing.Size(216, 24);
            this.rbLabelFeature.TabIndex = 4;
            this.rbLabelFeature.Text = "ÿ��Ҫ��һ����ע";
            this.rbLabelFeature.CheckedChanged += new System.EventHandler(this.OnDuplicateLabelChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cbPlaceOverlapLabel);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(386, 279);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "��ͻ����";
            // 
            // cbPlaceOverlapLabel
            // 
            this.cbPlaceOverlapLabel.Location = new System.Drawing.Point(16, 144);
            this.cbPlaceOverlapLabel.Name = "cbPlaceOverlapLabel";
            this.cbPlaceOverlapLabel.Size = new System.Drawing.Size(224, 24);
            this.cbPlaceOverlapLabel.TabIndex = 3;
            this.cbPlaceOverlapLabel.Text = "���ý����ע";
            this.cbPlaceOverlapLabel.CheckedChanged += new System.EventHandler(this.cbPlaceOverlapLabel_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tbLabelBuffer);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox7.Location = new System.Drawing.Point(0, 96);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(386, 48);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "��ע����";
            // 
            // tbLabelBuffer
            // 
            this.tbLabelBuffer.Location = new System.Drawing.Point(16, 17);
            this.tbLabelBuffer.Name = "tbLabelBuffer";
            this.tbLabelBuffer.Size = new System.Drawing.Size(360, 21);
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
            this.groupBox6.Size = new System.Drawing.Size(386, 48);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ҫ��Ȩ��";
            // 
            // cbFeatureWeight
            // 
            this.cbFeatureWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFeatureWeight.Location = new System.Drawing.Point(16, 22);
            this.cbFeatureWeight.Name = "cbFeatureWeight";
            this.cbFeatureWeight.Size = new System.Drawing.Size(360, 20);
            this.cbFeatureWeight.TabIndex = 1;
            this.cbFeatureWeight.SelectedIndexChanged += new System.EventHandler(this.cbFeatureWeight_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbLabelWeight);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(386, 48);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "��עȨ��";
            // 
            // cbLabelWeight
            // 
            this.cbLabelWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLabelWeight.Location = new System.Drawing.Point(16, 16);
            this.cbLabelWeight.Name = "cbLabelWeight";
            this.cbLabelWeight.Size = new System.Drawing.Size(360, 20);
            this.cbLabelWeight.TabIndex = 0;
            this.cbLabelWeight.SelectedIndexChanged += new System.EventHandler(this.cbLabelWeight_SelectedIndexChanged);
            // 
            // AnnotatePlacementLine
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(394, 352);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotatePlacementLine";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "�߱�עλ��";
            this.Load += new System.EventHandler(this.AnnotatePlacementLine_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSample)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
        private void cbOrientSys_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                if(this.cbOrientSys .SelectedIndex ==0)
                {
                    this.cbAbove .Text ="�����Ϸ�";
                    this.cbOnTheLine .Text ="������";
                    this.cbUnderline .Text ="�����·�";
                    this.pbSample.Image = OtherHelper.LoadImage(System.Windows.Forms.Application.StartupPath + @"\image\LineLabelPos\001.bmp");
                }
                else
                {
                    this.cbAbove .Text ="�������";
                    this.cbOnTheLine .Text ="������";
                    this.cbUnderline .Text ="�����Ҳ�";
                    this.pbSample.Image = OtherHelper.LoadImage(System.Windows.Forms.Application.StartupPath + @"\image\LineLabelPos\002.bmp");            
                }
                this.ChangeLinePosition ();
            }
        }

        private void AnnotatePlacementLine_Load(object sender, System.EventArgs e)
        {
            this.m_shouldAction =false;
            ComboBoxItem cbi=new ComboBoxItem("OrientSys_Page","��ͼҳ��",1);
            this.cbOrientSys .Items.Add (cbi);
            cbi=new ComboBoxItem ("OrientSys_Line","��������",2);
            this.cbOrientSys .Items .Add (cbi);

            cbi=new ComboBoxItem ("LinePos_AtStart","��ʼ��",1);
            this.cbLinePosition .Items.Add (cbi);
            cbi=new ComboBoxItem ("LinePos_AtBest","���λ��",2);
            this.cbLinePosition .Items .Add (cbi);
            cbi=new ComboBoxItem ("LinePos_AtEnd","�ս��",3);
            this.cbLinePosition .Items .Add (cbi);

            
            if(this.PlacementProperties!=null)
            {
                #region ������λ��
                ILineLabelPosition lineLP=this.PlacementProperties .LineLabelPosition;
                if(lineLP.AtStart )this.cbLinePosition .SelectedIndex =0;
                else if(lineLP.AtEnd )this.cbLinePosition .SelectedIndex =2;
                else this.cbLinePosition .SelectedIndex =1;
                #endregion
                #region ��עλ��
                
                this.cbAbove .Checked =(lineLP.Left ||lineLP.Above );
                this.cbOnTheLine.Checked =(lineLP.InLine ||lineLP.OnTop );
                this.cbUnderline.Checked =(lineLP.Right ||lineLP.Below );

                this.tbOffset .Text =lineLP.Offset .ToString ();
                #endregion
                #region ����
                if(lineLP.Horizontal )this.rbOrientHorizontal .Checked=true;
                else if(lineLP.Parallel )this.rbOrientParallel .Checked =true;
                else if(lineLP.ProduceCurvedLabels)this.rbOrientCurve .Checked =true;
                else if(lineLP.Perpendicular )this.rbOrientPerpendicular .Checked =true;
                #endregion
                
                #region �ظ�����
                if(this.PlacementProperties .NumLabelsOption ==esriBasicNumLabelsOption.esriOneLabelPerName)
                {
                    this.rbRemoveDuplicate .Checked =true;
                }
                else if(this.PlacementProperties .NumLabelsOption ==esriBasicNumLabelsOption.esriOneLabelPerShape)
                {
                    this.rbLabelFeature .Checked =true;
                }
                else 
                {
                    this.rbLabelFeaturePart .Checked =true;
                }
                #endregion
                #region ��ͻ����
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
                #region ��ע����ϵͳ
                //1.�������� ��Ϊ���Ѿ���Ҫϵͳ����Ӧ��(��Ҫthis.m_shouldActionΪtrue)
                //2.Ҫ�����趨��עλ��֮��(cbAbove,cbOnTheLine,cbUnderline),��Ϊ��Ҫ����������
                //  ԭʼ״̬��
                if(lineLP.Left ||lineLP.Right)
                {//��������
                    
                    this.cbOrientSys .SelectedIndex =1;                    
                }
                else if(lineLP.Below||lineLP.OnTop )
                {//��������
                    this.cbOrientSys .SelectedIndex =0;
                }
                //ע����ⶼ��ѡ�е����⡣
                if(this.cbOrientSys .SelectedItem ==null)
                    this.cbOrientSys .SelectedIndex =0;
                #endregion
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
        private void btnPriority_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show ("�˹�����δʵ��,Ŀǰֻ��ʹ��Ĭ�����ȼ�","�������ȼ�");
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

        private void OnOrientChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                this.PlacementProperties .LineLabelPosition .Horizontal =this.rbOrientHorizontal .Checked ;
                this.PlacementProperties .LineLabelPosition .Parallel =this.rbOrientParallel .Checked ;
                this.PlacementProperties .LineLabelPosition .ProduceCurvedLabels =this.rbOrientCurve .Checked ;
                this.PlacementProperties .LineLabelPosition .Perpendicular =this.rbOrientPerpendicular .Checked ;
            }
        }

        private void OnDuplicateLabelChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                if(this.rbRemoveDuplicate .Checked )
                {
                    this.PlacementProperties .NumLabelsOption =esriBasicNumLabelsOption.esriOneLabelPerName;
                }
                else if(this.rbLabelFeature .Checked )
                {
                    this.PlacementProperties .NumLabelsOption =esriBasicNumLabelsOption.esriOneLabelPerShape;
                }
                else if(this.rbLabelFeaturePart .Checked )
                {
                    this.PlacementProperties .NumLabelsOption =esriBasicNumLabelsOption.esriOneLabelPerPart;
                }
            }
        }
        private void ChangeLinePosition()
        {
            if(this.cbOrientSys .SelectedIndex ==0)
            {//��ͼҳ��
                this.PlacementProperties .LineLabelPosition .Above =this.cbAbove .Checked ;
                this.PlacementProperties .LineLabelPosition .OnTop =this.cbOnTheLine .Checked ;
                this.PlacementProperties .LineLabelPosition .Below =this.cbUnderline .Checked ;
                //ȥ�������������йص�
                this.PlacementProperties .LineLabelPosition .Left =false;
                this.PlacementProperties .LineLabelPosition .InLine =false;
                this.PlacementProperties .LineLabelPosition .Right =false;
            }
            else if(this.cbOrientSys .SelectedIndex ==1)
            {//��������
                this.PlacementProperties .LineLabelPosition .Left =this.cbAbove .Checked ;
                this.PlacementProperties .LineLabelPosition .InLine  =this.cbOnTheLine .Checked ;
                this.PlacementProperties .LineLabelPosition .Right =this.cbUnderline .Checked ;
                //ȥ���͵�ͼҳ���йص�
                this.PlacementProperties .LineLabelPosition .Above =false;
                this.PlacementProperties .LineLabelPosition .OnTop =false;
                this.PlacementProperties .LineLabelPosition .Below =false;
            }
        }
        private void OnLinePositionChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                this.ChangeLinePosition ();
            }
        }

        private void tbOffset_TextChanged(object sender, System.EventArgs e)
        {
            double newOffset=0;
            bool resultOK=Double.TryParse (this.tbOffset .Text .Trim (),System.Globalization .NumberStyles .Any 
                ,new System.Globalization .NumberFormatInfo (),out newOffset);
            if(resultOK)
            {
                this.PlacementProperties .LineLabelPosition .Offset =newOffset;
            }
        }

       
        private void cbLabelWeight_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                ComboBoxItem cbi=this.cbLabelWeight .SelectedItem as ComboBoxItem;
                if(cbi!=null&&cbi.ItemObject !=null)
                {
                    this.PlacementProperties .LabelWeight =(esriBasicOverposterWeight)cbi.ItemObject ;
                }
            }
        }

        private void cbFeatureWeight_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction)
            {
                ComboBoxItem cbi=this.cbFeatureWeight .SelectedItem as ComboBoxItem;
                if(cbi!=null&&cbi.ItemObject !=null)
                {
                    this.PlacementProperties .FeatureWeight =(esriBasicOverposterWeight)cbi.ItemObject ;
                }
            }
        }

        private void tbLabelBuffer_TextChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction)
            {
                double buff=0;
                bool resultOK=Double.TryParse (this.tbLabelBuffer .Text .Trim (),
                    System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),
                    out buff);
                if(resultOK)
                {
                    this.PlacementProperties .BufferRatio =buff;
                }
            }
        }

        private void cbPlaceOverlapLabel_CheckedChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                this.PlacementProperties .GenerateUnplacedLabels =this.cbPlaceOverlapLabel .Checked;
            }
        }

        private void cbLinePosition_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(this.m_shouldAction )
            {
                if(this.cbLinePosition .SelectedIndex ==0)
                {
                    this.PlacementProperties .LineLabelPosition .AtStart =true;
                    this.PlacementProperties .LineLabelPosition .AtEnd =false;
                }
                else if(this.cbLinePosition .SelectedIndex ==2)
                {
                    this.PlacementProperties .LineLabelPosition .AtEnd =true;
                    this.PlacementProperties .LineLabelPosition .AtStart =false;
                }
                else
                {
                    this.PlacementProperties .LineLabelPosition .AtStart =false;
                    this.PlacementProperties .LineLabelPosition .AtEnd =false;
                }
            }
        }
    }
}
