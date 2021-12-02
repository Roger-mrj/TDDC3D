using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using sycCommonLib;

using RCIS.Utility;
using RCIS.GISCommon;

namespace sycWS
{
	/// <summary>
	/// LSOutForm ��ժҪ˵����
	/// </summary>
	public class LSOutForm : System.Windows.Forms.Form
    {
        #region   �����Ķ���
        private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textBoxTKJL;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkTKFLW;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.ComboBox cmbCTFW;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cmbCTBLC;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;


        public DevExpress.XtraTab.XtraTabControl m_myTab;
        
		public ESRI.ArcGIS.Controls.AxMapControl m_MapControl;
		public ESRI.ArcGIS.Controls.AxPageLayoutControl m_PageControl;
		public IMapFrame m_myMapFrame;

        public IWorkspace currentWorkspace = null;
        

		
		public double m_dJ1,m_dW1,m_dJ3,m_dW3;
		public string m_sName,m_sTFH;
		public string m_sCode;
		public IGeometry m_MyPolygon;
		public IGeometry m_bufferPolygon;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TextBox textBox3;
        private Label label6;
        private TextBox textBox2;
        private Label label7;
        private TextBox textRXJZJ;
        private TextBox textBT;
        private TextBox textZXJZJ;
        private Label label4;
        private Label label3;
        private Label labelBT;
        private TabPage tabPage2;
        private CheckBox chkXZDWZJ;
        private CheckBox chkDLTBZJ;
        private CheckBox chkXZQZJ;
        private DevExpress.XtraEditors.ColorEdit ceXZDW;
        private DevExpress.XtraEditors.ComboBoxEdit cboXZDWSize;
        private DevExpress.XtraEditors.ComboBoxEdit cboXZDWFont;
        private DevExpress.XtraEditors.ColorEdit ceXZQ;
        private DevExpress.XtraEditors.ComboBoxEdit cboXZQSize;
        private DevExpress.XtraEditors.ComboBoxEdit cboXZQFont;
        private DevExpress.XtraEditors.ColorEdit ceDLTB;
        private DevExpress.XtraEditors.ComboBoxEdit cboDLTBSize;
        private DevExpress.XtraEditors.ComboBoxEdit cboDLTBFont;
        private Label label10;
        private TextBox txtXZDWCDRX;
        private TextBox txtDltbMjRX;
        private Label label9;
        private Label label8;
        private Label label11;

        #  endregion


        /// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LSOutForm()
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxTKJL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkTKFLW = new System.Windows.Forms.CheckBox();
            this.button9 = new System.Windows.Forms.Button();
            this.cmbCTFW = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbCTBLC = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textRXJZJ = new System.Windows.Forms.TextBox();
            this.textBT = new System.Windows.Forms.TextBox();
            this.textZXJZJ = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelBT = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtXZDWCDRX = new System.Windows.Forms.TextBox();
            this.txtDltbMjRX = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ceXZDW = new DevExpress.XtraEditors.ColorEdit();
            this.cboXZDWSize = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboXZDWFont = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceXZQ = new DevExpress.XtraEditors.ColorEdit();
            this.cboXZQSize = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboXZQFont = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ceDLTB = new DevExpress.XtraEditors.ColorEdit();
            this.cboDLTBSize = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cboDLTBFont = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chkXZDWZJ = new System.Windows.Forms.CheckBox();
            this.chkDLTBZJ = new System.Windows.Forms.CheckBox();
            this.chkXZQZJ = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceXZDW.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZDWSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZDWFont.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceXZQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQFont.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDLTB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBFont.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxTKJL);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkTKFLW);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.cmbCTFW);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbCTBLC);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(6, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 148);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "���β���: ";
            // 
            // textBoxTKJL
            // 
            this.textBoxTKJL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxTKJL.Location = new System.Drawing.Point(142, 42);
            this.textBoxTKJL.Name = "textBoxTKJL";
            this.textBoxTKJL.Size = new System.Drawing.Size(22, 21);
            this.textBoxTKJL.TabIndex = 12;
            this.textBoxTKJL.Text = "20";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(4, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 14);
            this.label1.TabIndex = 11;
            this.label1.Text = "ͼ������ͼ������[mm]:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkTKFLW
            // 
            this.chkTKFLW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkTKFLW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkTKFLW.Location = new System.Drawing.Point(10, 24);
            this.chkTKFLW.Name = "chkTKFLW";
            this.chkTKFLW.Size = new System.Drawing.Size(160, 16);
            this.chkTKFLW.TabIndex = 10;
            this.chkTKFLW.Text = "��ע����ͼ���ڵķ�����";
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button9.Location = new System.Drawing.Point(10, 105);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(312, 24);
            this.button9.TabIndex = 9;
            this.button9.Text = "��ѡ����ͼ��Χ�ڵ�����һ��...";
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // cmbCTFW
            // 
            this.cmbCTFW.BackColor = System.Drawing.Color.White;
            this.cmbCTFW.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCTFW.DropDownWidth = 120;
            this.cmbCTFW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCTFW.Items.AddRange(new object[] {
            "��׼�ַ�",
            "��ͼ",
            "��ͼ",
            "��ͼ",
            "��������"});
            this.cmbCTFW.Location = new System.Drawing.Point(260, 45);
            this.cmbCTFW.Name = "cmbCTFW";
            this.cmbCTFW.Size = new System.Drawing.Size(66, 21);
            this.cmbCTFW.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(168, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 14);
            this.label5.TabIndex = 7;
            this.label5.Text = "��ͼ��Χ:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmbCTBLC
            // 
            this.cmbCTBLC.BackColor = System.Drawing.Color.White;
            this.cmbCTBLC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCTBLC.ForeColor = System.Drawing.Color.Blue;
            this.cmbCTBLC.Items.AddRange(new object[] {
            "2000",
            "5000",
            "10000",
            "25000",
            "50000",
            "100000"});
            this.cmbCTBLC.Location = new System.Drawing.Point(260, 20);
            this.cmbCTBLC.Name = "cmbCTBLC";
            this.cmbCTBLC.Size = new System.Drawing.Size(66, 21);
            this.cmbCTBLC.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(168, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ԥ��ͼ������1:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(376, 44);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(49, 23);
            this.button4.TabIndex = 28;
            this.button4.Text = "ȡ��";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(376, 21);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 22);
            this.button3.TabIndex = 27;
            this.button3.Text = "ȷ��";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Right;
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(618, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(18, 474);
            this.statusStrip1.TabIndex = 39;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Silver;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 0);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 159);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(438, 221);
            this.tabControl1.TabIndex = 49;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.textRXJZJ);
            this.tabPage1.Controls.Add(this.textBT);
            this.tabPage1.Controls.Add(this.textZXJZJ);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.labelBT);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(430, 195);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "������Ϣ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox3.Location = new System.Drawing.Point(303, 69);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(76, 21);
            this.textBox3.TabIndex = 58;
            this.textBox3.Text = "�ܼ�";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(253, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 15);
            this.label6.TabIndex = 57;
            this.label6.Text = "����:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox2
            // 
            this.textBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textBox2.Location = new System.Drawing.Point(235, 39);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(144, 21);
            this.textBox2.TabIndex = 56;
            this.textBox2.Text = "ĳĳ���ع����";
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(232, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 16);
            this.label7.TabIndex = 55;
            this.label7.Text = "��ͼ��λ:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textRXJZJ
            // 
            this.textRXJZJ.Location = new System.Drawing.Point(188, 117);
            this.textRXJZJ.Multiline = true;
            this.textRXJZJ.Name = "textRXJZJ";
            this.textRXJZJ.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textRXJZJ.Size = new System.Drawing.Size(196, 46);
            this.textRXJZJ.TabIndex = 54;
            this.textRXJZJ.Text = "����Ա:\r\n��ͼԱ:\r\n���Ա:";
            // 
            // textBT
            // 
            this.textBT.Location = new System.Drawing.Point(6, 38);
            this.textBT.Multiline = true;
            this.textBT.Name = "textBT";
            this.textBT.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBT.Size = new System.Drawing.Size(220, 52);
            this.textBT.TabIndex = 50;
            this.textBT.Text = "����ע��";
            // 
            // textZXJZJ
            // 
            this.textZXJZJ.Location = new System.Drawing.Point(6, 117);
            this.textZXJZJ.Multiline = true;
            this.textZXJZJ.Name = "textZXJZJ";
            this.textZXJZJ.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textZXJZJ.Size = new System.Drawing.Size(174, 46);
            this.textZXJZJ.TabIndex = 52;
            this.textZXJZJ.Text = "ƽ������ϵ:\r\n�̻߳�׼:\r\n��ͼ��λ������:";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(186, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 21);
            this.label4.TabIndex = 53;
            this.label4.Text = "���½�ע��:";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(6, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 21);
            this.label3.TabIndex = 51;
            this.label3.Text = "���½�ע��:";
            // 
            // labelBT
            // 
            this.labelBT.ForeColor = System.Drawing.Color.Blue;
            this.labelBT.Location = new System.Drawing.Point(6, 17);
            this.labelBT.Name = "labelBT";
            this.labelBT.Size = new System.Drawing.Size(72, 18);
            this.labelBT.TabIndex = 49;
            this.labelBT.Text = "����ע��:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.txtXZDWCDRX);
            this.tabPage2.Controls.Add(this.txtDltbMjRX);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.ceXZDW);
            this.tabPage2.Controls.Add(this.cboXZDWSize);
            this.tabPage2.Controls.Add(this.cboXZDWFont);
            this.tabPage2.Controls.Add(this.ceXZQ);
            this.tabPage2.Controls.Add(this.cboXZQSize);
            this.tabPage2.Controls.Add(this.cboXZQFont);
            this.tabPage2.Controls.Add(this.ceDLTB);
            this.tabPage2.Controls.Add(this.cboDLTBSize);
            this.tabPage2.Controls.Add(this.cboDLTBFont);
            this.tabPage2.Controls.Add(this.chkXZDWZJ);
            this.tabPage2.Controls.Add(this.chkDLTBZJ);
            this.tabPage2.Controls.Add(this.chkXZQZJ);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(430, 195);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "��ע��Ϣ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(407, 161);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(19, 13);
            this.label11.TabIndex = 178;
            this.label11.Text = "��";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(407, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 177;
            this.label10.Text = "ƽ��";
            // 
            // txtXZDWCDRX
            // 
            this.txtXZDWCDRX.Location = new System.Drawing.Point(351, 156);
            this.txtXZDWCDRX.Name = "txtXZDWCDRX";
            this.txtXZDWCDRX.Size = new System.Drawing.Size(51, 21);
            this.txtXZDWCDRX.TabIndex = 176;
            this.txtXZDWCDRX.Text = "500";
            // 
            // txtDltbMjRX
            // 
            this.txtDltbMjRX.Location = new System.Drawing.Point(350, 98);
            this.txtDltbMjRX.Name = "txtDltbMjRX";
            this.txtDltbMjRX.Size = new System.Drawing.Size(51, 21);
            this.txtDltbMjRX.TabIndex = 175;
            this.txtDltbMjRX.Text = "15000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(348, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 174;
            this.label9.Text = "��������";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(347, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 173;
            this.label8.Text = "�������";
            // 
            // ceXZDW
            // 
            this.ceXZDW.EditValue = System.Drawing.Color.Black;
            this.ceXZDW.Location = new System.Drawing.Point(244, 153);
            this.ceXZDW.Name = "ceXZDW";
            this.ceXZDW.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceXZDW.Size = new System.Drawing.Size(100, 20);
            this.ceXZDW.TabIndex = 172;
            // 
            // cboXZDWSize
            // 
            this.cboXZDWSize.Location = new System.Drawing.Point(138, 153);
            this.cboXZDWSize.Name = "cboXZDWSize";
            this.cboXZDWSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboXZDWSize.Properties.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "24",
            "32",
            "36",
            "48",
            "72"});
            this.cboXZDWSize.Size = new System.Drawing.Size(100, 20);
            this.cboXZDWSize.TabIndex = 171;
            // 
            // cboXZDWFont
            // 
            this.cboXZDWFont.Location = new System.Drawing.Point(17, 153);
            this.cboXZDWFont.Name = "cboXZDWFont";
            this.cboXZDWFont.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboXZDWFont.Size = new System.Drawing.Size(115, 20);
            this.cboXZDWFont.TabIndex = 170;
            // 
            // ceXZQ
            // 
            this.ceXZQ.EditValue = System.Drawing.Color.Black;
            this.ceXZQ.Location = new System.Drawing.Point(244, 39);
            this.ceXZQ.Name = "ceXZQ";
            this.ceXZQ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceXZQ.Size = new System.Drawing.Size(100, 20);
            this.ceXZQ.TabIndex = 169;
            // 
            // cboXZQSize
            // 
            this.cboXZQSize.Location = new System.Drawing.Point(138, 39);
            this.cboXZQSize.Name = "cboXZQSize";
            this.cboXZQSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboXZQSize.Properties.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "24",
            "32",
            "36",
            "48",
            "72"});
            this.cboXZQSize.Size = new System.Drawing.Size(100, 20);
            this.cboXZQSize.TabIndex = 168;
            // 
            // cboXZQFont
            // 
            this.cboXZQFont.Location = new System.Drawing.Point(17, 39);
            this.cboXZQFont.Name = "cboXZQFont";
            this.cboXZQFont.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboXZQFont.Size = new System.Drawing.Size(115, 20);
            this.cboXZQFont.TabIndex = 167;
            // 
            // ceDLTB
            // 
            this.ceDLTB.EditValue = System.Drawing.Color.Black;
            this.ceDLTB.Location = new System.Drawing.Point(244, 96);
            this.ceDLTB.Name = "ceDLTB";
            this.ceDLTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceDLTB.Size = new System.Drawing.Size(100, 20);
            this.ceDLTB.TabIndex = 154;
            // 
            // cboDLTBSize
            // 
            this.cboDLTBSize.Location = new System.Drawing.Point(138, 96);
            this.cboDLTBSize.Name = "cboDLTBSize";
            this.cboDLTBSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDLTBSize.Properties.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "24",
            "32",
            "36",
            "48",
            "72"});
            this.cboDLTBSize.Size = new System.Drawing.Size(100, 20);
            this.cboDLTBSize.TabIndex = 153;
            // 
            // cboDLTBFont
            // 
            this.cboDLTBFont.Location = new System.Drawing.Point(17, 96);
            this.cboDLTBFont.Name = "cboDLTBFont";
            this.cboDLTBFont.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboDLTBFont.Size = new System.Drawing.Size(115, 20);
            this.cboDLTBFont.TabIndex = 152;
            // 
            // chkXZDWZJ
            // 
            this.chkXZDWZJ.AutoSize = true;
            this.chkXZDWZJ.Checked = true;
            this.chkXZDWZJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXZDWZJ.Location = new System.Drawing.Point(6, 123);
            this.chkXZDWZJ.Name = "chkXZDWZJ";
            this.chkXZDWZJ.Size = new System.Drawing.Size(98, 17);
            this.chkXZDWZJ.TabIndex = 2;
            this.chkXZDWZJ.Text = "��״�����ע";
            this.chkXZDWZJ.UseVisualStyleBackColor = true;
            // 
            // chkDLTBZJ
            // 
            this.chkDLTBZJ.AutoSize = true;
            this.chkDLTBZJ.Checked = true;
            this.chkDLTBZJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDLTBZJ.Location = new System.Drawing.Point(6, 72);
            this.chkDLTBZJ.Name = "chkDLTBZJ";
            this.chkDLTBZJ.Size = new System.Drawing.Size(98, 17);
            this.chkDLTBZJ.TabIndex = 1;
            this.chkDLTBZJ.Text = "����ͼ�߱�ע";
            this.chkDLTBZJ.UseVisualStyleBackColor = true;
            // 
            // chkXZQZJ
            // 
            this.chkXZQZJ.AutoSize = true;
            this.chkXZQZJ.Checked = true;
            this.chkXZQZJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXZQZJ.Location = new System.Drawing.Point(6, 16);
            this.chkXZQZJ.Name = "chkXZQZJ";
            this.chkXZQZJ.Size = new System.Drawing.Size(86, 17);
            this.chkXZQZJ.TabIndex = 0;
            this.chkXZQZJ.Text = "��������ע";
            this.chkXZQZJ.UseVisualStyleBackColor = true;
            // 
            // LSOutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(636, 474);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LSOutForm";
            this.Text = "�������[LS]";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.LSOutForm_Closing);
            this.Load += new System.EventHandler(this.LSOutForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ceXZDW.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZDWSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZDWFont.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceXZQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboXZQFont.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDLTB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboDLTBFont.Properties)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


        private void LoadAllFont(DevExpress.XtraEditors.ComboBoxEdit cmb)
        {
            //��λ��ϵͳ�����б� 
            cmb.Properties.Items.Clear();
            System.Drawing.Text.InstalledFontCollection fonts = new System.Drawing.Text.InstalledFontCollection();
            foreach (System.Drawing.FontFamily family in fonts.Families)
            {
                cmb.Properties.Items.Add(family.Name);
            }  

        }

        private void ZJSetup()
        {
            LoadAllFont(this.cboDLTBFont);
            LoadAllFont(this.cboXZQFont);

            this.ceDLTB.EditValue = System.Drawing.Color.Black;
            this.cboDLTBFont.Text = "����ϸ���߼���";
            this.cboDLTBSize.Text = "8.5";

            this.ceXZQ.EditValue = System.Drawing.Color.Black;
            this.cboXZQFont.Text = "����";
            this.cboXZQSize.EditValue = "25";

            this.ceXZDW.EditValue = System.Drawing.Color.Black;
            this.cboXZDWFont.Text = "����";
            this.cboXZDWSize.Text = "6.5";
        }
        //guojie ++++++++++++++++++++++++++++++


        private IMapFrame GetIMapFrame()
        {
            IGraphicsContainer container = this.m_PageControl.GraphicsContainer;// .GraphicsContainer;
            container.Reset();

            IElement ele = container.Next();
            while (ele != null)
            {
                if (ele is IMapFrame)
                {
                    return (IMapFrame)ele;
                }
                ele = container.Next();
            }

            return (IMapFrame)null;
        }


		private void LSOutForm_Load(object sender, System.EventArgs e)
		{

            this.m_myMapFrame = this.GetIMapFrame();
            ZJSetup();
            //GetAnnoStatus(); //��ȡ ��ǰͼ���ע״̬����Ϊ ��ͼ��ʱ��Ҫ�� ��עȥ������ͼ���Ҫ�ָ�

			cmbCTBLC.SelectedIndex=2;	//1:1��
			cmbCTFW.SelectedIndex=0;	//��׼ͼ��		


		}

        //ȡ��
		private void button4_Click(object sender, System.EventArgs e)
		{			
			
			ICommand myTool=new ControlsMapPanToolClass();
            myTool.OnCreate(this.m_MapControl.Object);
			m_MapControl.CurrentTool=myTool as ITool;
			m_MyPolygon=null;
			m_bufferPolygon=null;

			//...
			IGraphicsContainer mapCon=m_MapControl.ActiveView.GraphicsContainer;
			mapCon.Reset();
			IElement ele=mapCon.Next();
			int nGS=0;
			while(ele!=null) 
			{
				nGS++;
				ele=mapCon.Next();
			}
			object[] eleArr=new object[nGS];
			mapCon.Reset();
			ele=mapCon.Next();
			nGS=0;
			while(ele!=null) 
			{
				eleArr[nGS]=ele;
				nGS++;
				ele=mapCon.Next();
			}
			for(int i=0;i<nGS;i++) 
			{
				if(eleArr[i] is IMapFrame) ;
				else mapCon.DeleteElement((IElement)eleArr[i]);
			}

			IActiveView act=m_MapControl.ActiveView;
			act.Refresh();

			//...
			this.Visible=false;		
		}

		private void LSOutForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel=true;

			
			ICommand myTool=new ControlsMapPanToolClass();
            myTool.OnCreate(this.m_MapControl.Object);
			m_MapControl.CurrentTool=myTool as ITool;
			m_MyPolygon=null;
			m_bufferPolygon=null;

			//...
			IGraphicsContainer mapCon=m_MapControl.ActiveView.GraphicsContainer;
			mapCon.Reset();
			IElement ele=mapCon.Next();
			int nGS=0;
			while(ele!=null) 
			{
				nGS++;
				ele=mapCon.Next();
			}
			object[] eleArr=new object[nGS];
			mapCon.Reset();
			ele=mapCon.Next();
			nGS=0;
			while(ele!=null) 
			{
				eleArr[nGS]=ele;
				nGS++;
				ele=mapCon.Next();
			}
			for(int i=0;i<nGS;i++) 
			{
				if(eleArr[i] is IMapFrame) ;
				else mapCon.DeleteElement((IElement)eleArr[i]);
			}
			IActiveView act=m_MapControl.ActiveView;
			act.Refresh();
			this.Visible=false;		
		}

        //���ѡ���ͼ��Χ
		private void button9_Click(object sender, System.EventArgs e)
		{
			//��ѡ�������Ԫ��:
			double dMMJL=50.0;
			string sFW=cmbCTFW.SelectedItem.ToString();
			if(sFW.Equals("��׼�ַ�")==false) 
			{
				try 
				{
					dMMJL=Convert.ToDouble(textBoxTKJL.Text.Trim());
				} 
				catch(Exception E) 
				{
					MessageBox.Show(" ��������: "+E.Message+" !");
					return;
				}
			}
			string sScale=cmbCTBLC.SelectedItem.ToString();
			this.Visible=false;
			
			LSOutTool MyTool=new LSOutTool();
			MyTool.myMapControl=m_MapControl;
			MyTool.m_UseForm=this;
			MyTool.m_sFW=sFW;
			MyTool.m_sScale=sScale;
			MyTool.m_dMMJL=dMMJL;
			m_MapControl.CurrentTool=MyTool;		
			//...
		}

		
		public class FLW 
		{
			public FLW() {}

			public string sBS;
			public string sZJ1,sZJ2;
			public IPoint PP1,PP2;
		}

        private IPoint GetPoint(IPoint pT, double x, double y)
        {
            IPoint p = new PointClass();
            p.PutCoords(pT.X + x, pT.Y + y);
            return p;
        }

        # region ����ͼ��ע��
        private void SymbolizeDLTBZJ(IGraphicsContainer pGc,IFeatureLayer dltbLyr, IGeometry pGeo)
        {
            // IArea area = pGeo as IArea;

            double dltbMjRx = 0.0;
            try
            {
                dltbMjRx = double.Parse(txtDltbMjRX.Text);
            }
            catch { }


            IIdentify dltbIndentify = dltbLyr as IIdentify;
            IArray arrDltbIDs = dltbIndentify.Identify(pGeo);
            for (int i = 0; i < arrDltbIDs.Count; i++)
            {
                IFeatureIdentifyObj idObj = arrDltbIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                IFeature pfea = pRow.Row as IFeature;

                if ((pfea.Shape as IArea).Area > dltbMjRx)
                {

                    IPoint ppoint = (pfea.ShapeCopy as IArea).LabelPoint;
                    int nX = 0, nY = 0;
                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sTBBH = FeatureHelper.GetFeatureStringValue(pfea, "TBBH");

                    string sDL = FeatureHelper.GetFeatureStringValue(pfea, "DLBM");
                    //IPoint pPT =  GetPoint(curZJP, 0.25, 0.15);

                    #region //����:
                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboDLTBFont.Text, 1, FontStyle.Underline);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboDLTBSize.Text.Trim());
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;
                    textSymbol.Color = ColorHelper.CreateColor(this.ceDLTB.Color);
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sTBBH;
                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                    #endregion
                    #region  //��ĸ
                    dotNetFont = new System.Drawing.Font(this.cboDLTBFont.Text, 1);
                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboDLTBSize.Text.Trim());
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVATop;
                    textSymbol.Color = ColorHelper.CreateColor(this.ceDLTB.Color);
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sDL;
                    element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                    #endregion
                    if(i>0 &&i %10000==0)
                      m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);                       

                }
            }
        }
        
        //��״����ע��
        private void SymbolizeXZDWZJ(IGraphicsContainer pGc, IFeatureLayer xzdwLyr, IGeometry pGeo)
        {
            double dXzdlCdRx = 0.0;
            try
            {
                dXzdlCdRx = double.Parse(txtXZDWCDRX.Text);
            }
            catch { }

            IIdentify xzdwIdentify = xzdwLyr as IIdentify;
            IArray arXzdwIDs = xzdwIdentify.Identify(pGeo);
            for (int i = 0; i < arXzdwIDs.Count; i++)
            {
                IFeatureIdentifyObj identifyObj = arXzdwIDs.get_Element(i) as IFeatureIdentifyObj;
                IRowIdentifyObject pRow = identifyObj as IRowIdentifyObject;
                IFeature pfea = pRow.Row as IFeature;

                if ((pfea.ShapeCopy as IPolyline).Length > dXzdlCdRx) //����һ�����ȵĲ���ʾ��ȱ��
                {
                    IPoint pPoint = new PointClass();
                    double lineLength = (pfea.ShapeCopy as IPolyline).Length / 2; //��ʾ���м䲿��

                    (pfea.ShapeCopy as IPolyline).QueryPoint(esriSegmentExtension.esriExtendAtFrom, lineLength, false, pPoint); //��ȡ�м��

                    int nX = 0, nY = 0;
                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(pPoint, out nX, out nY);

                    nX += 2;

                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sText = FeatureHelper.GetFeatureStringValue(pfea, "KD"); //��ȱ�ע

                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXZDWFont.Text, 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXZDWSize.Text);
                    textSymbol.Color = ColorHelper.CreateColor(this.ceXZDW.Color);
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sText;
                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                    if (i > 0 && i % 10000 == 0)
                        m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }


            }


        }

        //������ע��
        private void SymbolizeXZQZJ(IGraphicsContainer pGc, IFeatureLayer xzqLyr, IGeometry pGeo)
        {

            IIdentify xzqId = xzqLyr as IIdentify;

            IArray arXZQIDS = xzqId.Identify(pGeo);
            if (arXZQIDS == null)
                return;

            try
            {
                for (int i = 0; i < arXZQIDS.Count; i++)
                {
                    IFeatureIdentifyObj idObj = arXZQIDS.get_Element(i) as IFeatureIdentifyObj;
                    IRowIdentifyObject pRow = idObj as IRowIdentifyObject;
                    IFeature pfea = pRow.Row as IFeature;
                    IGeometry pXZQGeo = pfea.ShapeCopy;
                    IGeometry pInterGeo = (pGeo as ITopologicalOperator).Intersect(pXZQGeo, esriGeometryDimension.esriGeometry2Dimension);
                    IPoint ppoint = (pInterGeo as IArea).LabelPoint;
                    int nX = 0, nY = 0;

                    (this.m_PageControl.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation.FromMapPoint(ppoint, out nX, out nY);
                    IPoint curZJP = this.m_PageControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(nX, nY);
                    string sText = FeatureHelper.GetFeatureStringValue(pfea, "XZQMC");

                    System.Drawing.Font dotNetFont = new System.Drawing.Font(this.cboXZQFont.Text, 1);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;

                    textSymbol.Size = OtherHelper.ChangeNullToDoubleZero(this.cboXZQSize.Text);
                    textSymbol.Color = ColorHelper.CreateColor(this.ceXZQ.Color);
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sText;

                    IElement element = (IElement)textEle;
                    element.Geometry = curZJP;
                    pGc.AddElement(element, 0);
                }
            }
            catch (Exception ex)
            { }



        }
        #endregion

        private void deleteAllElement(IGraphicsContainer pageCon)
        {
            #region ɾ��ԭ����element

            IElement ele = pageCon.Next();
            int nGS = 0;
            while (ele != null)
            {
                nGS++;
                ele = pageCon.Next();
            }

            object[] eleArr = new object[nGS];
            pageCon.Reset();
            ele = pageCon.Next();
            nGS = 0;
            while (ele != null)
            {
                eleArr[nGS] = ele;
                nGS++;
                ele = pageCon.Next();
            }
            for (int i = 0; i < nGS; i++)
            {
                if (eleArr[i] is IMapFrame) ;
                else pageCon.DeleteElement((IElement)eleArr[i]);
            }
            #endregion 
        }

        private void button3_Click(object sender, System.EventArgs e)
		{
			sycCommonFuns CommonClassDLL=new sycCommonLib.sycCommonFuns();

            
			ESRI.ArcGIS.Controls.AxMapControl MapControl=m_MapControl;
			ESRI.ArcGIS.Controls.AxPageLayoutControl PageControl=m_PageControl;
			IMapFrame myMapFrame=m_myMapFrame;
			DevExpress.XtraTab.XtraTabControl  myTab=m_myTab;

			//��ͼ��Χ
			if(m_bufferPolygon==null) 
			{
				MessageBox.Show("��Map��ͼ�ϡ���ѡ��Ҫ��ͼ�ķ�Χ�ˣ�");
				return;
			}
            //ResetAnnoState(false);
            this.m_MapControl.Map.ClearSelection();
            this.m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null,  this.m_MapControl.ActiveView.Extent);


			string sFW=cmbCTFW.SelectedItem.ToString();		//��׼�ַ�����ͼ����ͼ����ͼ
			IMap myMap=MapControl.ActiveView.FocusMap;
			string sScale=cmbCTBLC.SelectedItem.ToString();
			double dScale=Convert.ToDouble(sScale);				
			this.Visible=false;
			double dSelScale=dScale;
			IGraphicsContainer pageCon=PageControl.ActiveView.GraphicsContainer;
			pageCon.Reset();
            deleteAllElement(pageCon);
           
            #region  ��ͼ���
            IObjectCopy objectCopy = new ObjectCopyClass(); 
			object toCopyMap=MapControl.ActiveView.FocusMap;
			object copiedMap = objectCopy.Copy(toCopyMap);
			object toOverwriteMap = PageControl.ActiveView.FocusMap;
			objectCopy.Overwrite(copiedMap, ref toOverwriteMap);

            myTab.SelectedTabPageIndex = 1;
			IActiveView active=PageControl.ActiveView.FocusMap as IActiveView;
			active.Extent=m_bufferPolygon.Envelope;  //ȷ����ǰ����

			double dFrameWidth=m_bufferPolygon.Envelope.Width/dSelScale*1000.0;
			double dFrameHeight=m_bufferPolygon.Envelope.Height/dSelScale*1000.0;
			double dTop=30,dLeft=30;
			double dRight=30,dBottom=30;
			double dPageWidth=dFrameWidth+dLeft+dRight;
			double dPageHeight=dFrameHeight+dTop+dBottom;

			IPage page=PageControl.Page;
			page.Units=ESRI.ArcGIS.esriSystem.esriUnits.esriMillimeters;
			page.PageToPrinterMapping=esriPageToPrinterMapping.esriPageMappingCrop;
			page.FormID=esriPageFormID.esriPageFormCUSTOM;
			page.PutCustomSize(dPageWidth,dPageHeight);
			RgbColor PageBackColor=new RgbColorClass();		
			PageBackColor.Red=255;
			PageBackColor.Green=255;
			PageBackColor.Blue=255;
			page.BackgroundColor=PageBackColor;

			IEnvelope Env=((IElement)myMapFrame).Geometry.Envelope;
			double dX1=Env.XMin;
			double dY1=Env.YMin;
			double dX3=Env.XMax;
			double dY3=Env.YMax;
			double dCurHeight=Env.Height;
			double dCurWidth=Env.Width;

			IPoint curPoint=new PointClass();
			curPoint.PutCoords(dX1,dY1);
			IPoint toPoint=new PointClass();
			toPoint.PutCoords(dLeft,dBottom);

			ITransform2D trans=(ITransform2D)myMapFrame;
			trans.Move(toPoint.X-curPoint.X,toPoint.Y-curPoint.Y);
			double dWScale=dFrameWidth/dCurWidth;
			double dHScale=dFrameHeight/dCurHeight;
			trans.Scale(toPoint,dWScale,dHScale);

			ISymbolBorder border=new SymbolBorderClass();
			SimpleLineSymbolClass lineSym=new SimpleLineSymbolClass();
			lineSym.Width=0.01;
			lineSym.Style=esriSimpleLineStyle.esriSLSNull;
			RgbColorClass Color=new RgbColorClass();
			Color.Red=255;
			Color.Green=0;
			Color.Blue=0;
			lineSym.Color=Color;
			border.LineSymbol=lineSym;
			myMapFrame.Border=(IBorder)border;

			ISymbolBackground back=new SymbolBackgroundClass();
			SimpleFillSymbolClass fillSym=new SimpleFillSymbolClass();
			fillSym.Style=esriSimpleFillStyle.esriSFSSolid;
			fillSym.Color=PageBackColor;
			lineSym=new SimpleLineSymbolClass();
			lineSym.Style=esriSimpleLineStyle.esriSLSNull;
			fillSym.Outline=lineSym;
			back.FillSymbol=fillSym;
			myMapFrame.Background=back;

            PageControl.ActiveView.FocusMap.ClipGeometry = m_MyPolygon;
            PageControl.ActiveView.Refresh();
        
            #endregion

            #region ��ע����Ϣ
            for (int ilyr = 0; ilyr < myMap.LayerCount; ilyr++)
            {
                ILayer currentLyr=myMap.get_Layer(ilyr);               
                if (currentLyr.Visible == false)
                    continue;
                IFeatureLayer featlyr=currentLyr as IFeatureLayer;
                IFeatureClass currFeatCls = featlyr.FeatureClass;
                string layerName = featlyr.Name;               

                if (layerName.ToUpper() == "DLTB")
                {
                    IQueryFilter queryfilter = new QueryFilterClass();
                    queryfilter.WhereClause = " TBMJ  > " + txtDltbMjRX.Text;
                    bool isLarge = featlyr.FeatureClass.FeatureCount(queryfilter) > 5000 ? true : false;
                    if (chkDLTBZJ.Checked && !isLarge  )
                    {                      
                        SymbolizeDLTBZJ(pageCon, featlyr, m_MyPolygon);                    
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(queryfilter);
                }
                else if   (layerName.ToUpper() =="XZQ") 
                {                   
                    if (chkXZQZJ.Checked)
                    {
                        SymbolizeXZQZJ(pageCon, featlyr, m_bufferPolygon);
                    }
                }
                else if (layerName.ToUpper() == "XZDW")
                {                   
                    if (chkXZDWZJ.Checked)
                    {
                       SymbolizeXZDWZJ(pageCon, featlyr, m_MyPolygon);
                    }
                }
            }


            m_PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
            #endregion 

            //[04]:ZhengShi:
			myMap=MapControl.ActiveView.FocusMap;
			IActiveView act=MapControl.ActiveView.FocusMap as IActiveView;
			IActiveView mapAct=PageControl.ActiveView.FocusMap as IActiveView;
			IActiveView pageAct=PageControl.ActiveView;
			IPointCollection pCol=m_bufferPolygon as IPointCollection;

			PointClass NKP1=new PointClass();
			PointClass NKP2=new PointClass();
			PointClass NKP3=new PointClass();
			PointClass NKP4=new PointClass();
			if(sFW.Equals("��׼�ַ�")==true)
            {
                #region ��׼ͼ��
                NKP1.PutCoords(pCol.get_Point(0).X,pCol.get_Point(0).Y);
				NKP2.PutCoords(pCol.get_Point(1).X,pCol.get_Point(1).Y);
				NKP3.PutCoords(pCol.get_Point(2).X,pCol.get_Point(2).Y);
				NKP4.PutCoords(pCol.get_Point(3).X,pCol.get_Point(3).Y);

				EnvelopeClass env=new EnvelopeClass();
				env.XMin=0.0;
				env.XMax=1.0;
				env.YMax=0.0;
				env.YMax=1.0;
				pageAct.Extent=env;
				pageAct.Refresh();

				int nX=0,nY=0;
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP1,out nX,out nY);
				IPoint newP1=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP1.PutCoords(newP1.X,newP1.Y);
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP2,out nX,out nY);
				IPoint newP2=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP2.PutCoords(newP2.X,newP2.Y);
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP3,out nX,out nY);
				IPoint newP3=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP3.PutCoords(newP3.X,newP3.Y);
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP4,out nX,out nY);
				IPoint newP4=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP4.PutCoords(newP4.X,newP4.Y);
                #endregion 

            } 
			else
            {
                #region ������ͼ
                //���ͼ:
				string sRetErrorInfo="";
				IPoint NKP11=new PointClass();
				IPoint NKP22=new PointClass();
				IPoint NKP33=new PointClass();
				IPoint NKP44=new PointClass();
				CommonClassDLL.syc_JWD2XY(myMap,m_dJ1,m_dW1,ref NKP11,out sRetErrorInfo);
				CommonClassDLL.syc_JWD2XY(myMap,m_dJ3,m_dW1,ref NKP22,out sRetErrorInfo);
				CommonClassDLL.syc_JWD2XY(myMap,m_dJ3,m_dW3,ref NKP33,out sRetErrorInfo);
				CommonClassDLL.syc_JWD2XY(myMap,m_dJ1,m_dW3,ref NKP44,out sRetErrorInfo);

				NKP1.PutCoords(NKP11.X,NKP11.Y);
				NKP2.PutCoords(NKP22.X,NKP22.Y);
				NKP3.PutCoords(NKP33.X,NKP33.Y);
				NKP4.PutCoords(NKP44.X,NKP44.Y);
				EnvelopeClass env=new EnvelopeClass();
				env.XMin=0.0;
				env.XMax=1.0;
				env.YMax=0.0;
				env.YMax=1.0;
				pageAct.Extent=env;
				pageAct.Refresh();

				int nX=0,nY=0;
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP1,out nX,out nY);
				IPoint newP1=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP1.PutCoords(newP1.X,newP1.Y);
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP2,out nX,out nY);
				IPoint newP2=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP2.PutCoords(newP2.X,newP2.Y);
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP3,out nX,out nY);
				IPoint newP3=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP3.PutCoords(newP3.X,newP3.Y);
				mapAct.ScreenDisplay.DisplayTransformation.FromMapPoint(NKP4,out nX,out nY);
				IPoint newP4=pageAct.ScreenDisplay.DisplayTransformation.ToMapPoint(nX,nY);
				NKP4.PutCoords(newP4.X,newP4.Y);
                #endregion
            }
                
            IActiveView tmpView=PageControl.ActiveView.FocusMap as IActiveView;
			IGraphicsContainer tmpCon=tmpView.GraphicsContainer;
			tmpCon.DeleteAllElements();
			tmpCon=MapControl.ActiveView.GraphicsContainer;
			tmpCon.DeleteAllElements();

            

            #region �ڿ�:
            PolylineClass NewPol=new PolylineClass();
			pCol=(IPointCollection)NewPol;
			object Missing=Type.Missing;
			pCol.AddPoint(NKP1,ref Missing,ref Missing);
			pCol.AddPoint(NKP2,ref Missing,ref Missing);
			pCol.AddPoint(NKP3,ref Missing,ref Missing);
			pCol.AddPoint(NKP4,ref Missing,ref Missing);
			pCol.AddPoint(NKP1,ref Missing,ref Missing);

			lineSym=new SimpleLineSymbolClass();
			lineSym.Width=0.2;
			RgbColorClass eleColor=new RgbColorClass();
			eleColor.Red=0;
			eleColor.Green=0;
			eleColor.Blue=0;
			lineSym.Color=eleColor;
			LineElementClass LineEle=new LineElementClass();
			LineEle.Geometry=(IGeometry)NewPol;
			LineEle.Symbol=lineSym;
			pageCon.AddElement(LineEle,0);
            #endregion 

            #region // ���
            ILine pLine=new LineClass();

			pLine.FromPoint=NKP1;
			pLine.ToPoint=NKP2;
			PointClass tmpP1=new PointClass();
			((IConstructPoint)tmpP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,-12.0,false);
			PointClass tmpP2=new PointClass();
			double dLen=CommonClassDLL.syc_CalLength(ref NKP1,ref NKP2);
			((IConstructPoint)tmpP2).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dLen+12.0,false);

			pLine.FromPoint=NKP4;
			pLine.ToPoint=NKP3;
			PointClass tmpP4=new PointClass();
			((IConstructPoint)tmpP4).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,-12.0,false);
			PointClass tmpP3=new PointClass();
			dLen=CommonClassDLL.syc_CalLength(ref NKP3,ref NKP4);
			((IConstructPoint)tmpP3).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dLen+12.0,false);

			pLine.FromPoint=NKP1;
			pLine.ToPoint=NKP4;
			PointClass tmpPP1=new PointClass();
			((IConstructPoint)tmpPP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,-12.0,false);
			PointClass tmpPP4=new PointClass();
			dLen=CommonClassDLL.syc_CalLength(ref NKP1,ref NKP4);
			((IConstructPoint)tmpPP4).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dLen+12.0,false);

			pLine.FromPoint=NKP2;
			pLine.ToPoint=NKP3;
			PointClass tmpPP2=new PointClass();
			((IConstructPoint)tmpPP2).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,-12.0,false);
			PointClass tmpPP3=new PointClass();
			dLen=CommonClassDLL.syc_CalLength(ref NKP2,ref NKP3);
			((IConstructPoint)tmpPP3).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dLen+12.0,false);

					
			double dAF1=CommonClassDLL.syc_CalAngle(ref tmpP4,ref tmpP1);
			double dAF2=CommonClassDLL.syc_CalAngle(ref tmpPP2,ref tmpPP1);
			IPoint WKP1=new PointClass();
			((IConstructPoint)WKP1).ConstructAngleIntersection(tmpP1,dAF1,tmpPP1,dAF2);

			dAF1=CommonClassDLL.syc_CalAngle(ref tmpPP1,ref tmpPP2);
			dAF2=CommonClassDLL.syc_CalAngle(ref tmpP3,ref tmpP2);
			IPoint WKP2=new PointClass();
			((IConstructPoint)WKP2).ConstructAngleIntersection(tmpPP2,dAF1,tmpP2,dAF2);

			dAF1=CommonClassDLL.syc_CalAngle(ref tmpPP4,ref tmpPP3);
			dAF2=CommonClassDLL.syc_CalAngle(ref tmpP2,ref tmpP3);
			IPoint WKP3=new PointClass();
			((IConstructPoint)WKP3).ConstructAngleIntersection(tmpPP3,dAF1,tmpP3,dAF2);

			dAF1=CommonClassDLL.syc_CalAngle(ref tmpPP3,ref tmpPP4);
			dAF2=CommonClassDLL.syc_CalAngle(ref tmpP1,ref tmpP4);
			IPoint WKP4=new PointClass();
			((IConstructPoint)WKP4).ConstructAngleIntersection(tmpPP4,dAF1,tmpP4,dAF2);

			NewPol=new PolylineClass();
			pCol=(IPointCollection)NewPol;
			Missing=Type.Missing;
			pCol.AddPoint(WKP1,ref Missing,ref Missing);
			pCol.AddPoint(WKP2,ref Missing,ref Missing);
			pCol.AddPoint(WKP3,ref Missing,ref Missing);
			pCol.AddPoint(WKP4,ref Missing,ref Missing);
			pCol.AddPoint(WKP1,ref Missing,ref Missing);

			lineSym=new SimpleLineSymbolClass();
			lineSym.Width=0.2;
			eleColor=new RgbColorClass();
			eleColor.Red=0;
			eleColor.Green=0;
			eleColor.Blue=0;
			lineSym.Color=eleColor;
			LineEle=new LineElementClass();
			LineEle.Geometry=(IGeometry)NewPol;
			LineEle.Symbol=lineSym;
			pageCon.AddElement(LineEle,0);
            #endregion 

            if (true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP1,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpP1,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dW1.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=NKP1;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=NKP1;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP1,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpPP1,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dJ1.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=tmpPP1;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=tmpPP1;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP2,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpP2,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dW1.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=NKP2;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=NKP2;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP2,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpPP2,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dJ3.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=tmpPP2;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=tmpPP2;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP3,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpP3,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dW3.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=NKP3;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=NKP3;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP3,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpPP3,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dJ3.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=tmpPP3;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=tmpPP3;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP4,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpP4,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dW3.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=NKP4;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=NKP4;
				pageCon.AddElement(element,0);
			}

			if(true) 
			{
				object o=Type.Missing;
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(NKP4,ref o,ref o);
				((IPointCollection)pol).AddPoint(tmpPP4,ref o,ref o);
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				string sJWD=m_dJ1.ToString("F10");
				int nPos=sJWD.IndexOf(".");
				string sD=sJWD.Substring(0,nPos);
				string sF=sJWD.Substring(nPos+1,2);
				string sM=sJWD.Substring(nPos+3,6);
				double dM=Convert.ToDouble(sM)/10000.0;
				if(Math.Abs(dM-60.0)<0.1) 
				{
					sM="00";
					int nF=Convert.ToInt32(sF)+1;
					int nD=Convert.ToInt32(sD);
					if(Math.Abs(nF-60)<0.01) 
						nD=nD+1;
					sF=nF.ToString("D02");
					sD=nD.ToString();
				} 
				else 
				{
					sM=sM.Substring(0,2);
				}
				sD=sD+"��";
				sF=sF+"'";
				sM=sM+"\"";

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sD;
				IElement element=(IElement)textEle;
				element.Geometry=tmpPP4;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.20;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=sF+sM;
				element=(IElement)textEle;
				element.Geometry=tmpPP4;
				pageCon.AddElement(element,0);
			}

            //ע�Ǳ����

            #region ע�Ǳ����
            if (true) 
			{
				string sBT="";
				if(sFW.Equals("��׼�ַ�")==true)
					sBT=textBT.Text+"\r\n"+m_sTFH;
				else
					sBT=textBT.Text+"\r\n"+m_sName+"\r\n"+m_sCode;

				string sLD=textZXJZJ.Text;
				string sRD=textRXJZJ.Text;

				string delimStr = "\r\n";
				char [] delimiter=delimStr.ToCharArray();	
				string[] sBTSz=sBT.Split(delimiter);
				int nLen=sBTSz.Length;
				int nJS=0;
				double dZG=8;
				double dTextAF=CommonClassDLL.syc_CalAngle(ref WKP4,ref WKP3)*180.0/Math.PI;

				for(int i=nLen-1;i>=0;i--)
				{
					string ss=sBTSz[i].Trim();
					if(ss.Length==0)
						continue;

					PointClass pp1=new PointClass();
					pp1.X=(WKP3.X+WKP4.X)*0.5;
					pp1.Y=(WKP3.Y+WKP4.Y)*0.5;
					PointClass pp2=new PointClass();
					pp2.X=pp1.X;
					pp2.Y=pp1.Y+16.0+nJS*(dZG+2.0);	
					nJS++;

					
					System.Drawing.Font dotNetFont=new System.Drawing.Font("����",1,FontStyle.Bold);
					ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
					((IFormattedTextSymbol)textSymbol).CharacterWidth=100;
					textSymbol.Size=dZG/25.4*72.0;
					textSymbol.Angle=dTextAF;
					textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHACenter;
					textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
					TextElementClass textEle=new TextElementClass();
					textEle.Symbol = textSymbol;
					textEle.Text=ss;
					IElement element=(IElement)textEle;
					element.Geometry=pp2;
					pageCon.AddElement(element,0);
				}

				string[] LDSz=sLD.Split(delimiter);
				nLen=LDSz.Length;
				nJS=0;
				dZG=3.5;

				for(int i=0;i<nLen;i++) 
				{
					string ss=LDSz[i].Trim();
					if(ss.Length==0)
						continue;

					PointClass pp2=new PointClass();
					pp2.X=NKP1.X;
					pp2.Y=NKP1.Y-16-nJS*(dZG+2.0);
					nJS++;

					System.Drawing.Font dotNetFont=new System.Drawing.Font("����",1,FontStyle.Bold);
					ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
					((IFormattedTextSymbol)textSymbol).CharacterWidth=90;
					textSymbol.Size=dZG/25.4*72.0;
					textSymbol.Angle=dTextAF;
					textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
					textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
					TextElementClass textEle=new TextElementClass();
					textEle.Symbol = textSymbol;
					textEle.Text=ss;
					IElement element=(IElement)textEle;
					element.Geometry=pp2;
					pageCon.AddElement(element,0);
				}

				string[] RDSz=sRD.Split(delimiter);
				nLen=RDSz.Length;
				nJS=0;
				dZG=3.5;

				for(int i=0;i<nLen;i++) 
				{
					string ss=RDSz[i].Trim();
					if(ss.Length==0)
						continue;
					ss=ss+"     ";

					PointClass pp2=new PointClass();
					pp2.X=NKP2.X-30.0;
					pp2.Y=NKP2.Y-16-nJS*(dZG+2.0);
					nJS++;

					System.Drawing.Font dotNetFont=new System.Drawing.Font("����",1,FontStyle.Bold);
					ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
					((IFormattedTextSymbol)textSymbol).CharacterWidth=90;
					textSymbol.Size=dZG/25.4*72.0;
					textSymbol.Angle=dTextAF;
					textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
					textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
					TextElementClass textEle=new TextElementClass();
					textEle.Symbol = textSymbol;
					textEle.Text=ss;
					IElement element=(IElement)textEle;
					element.Geometry=pp2;
					pageCon.AddElement(element,0);
				}

				if(true) 
				{
					PointClass pp1=new PointClass();
					pp1.X=(WKP1.X+WKP2.X)*0.5;
					pp1.Y=(WKP1.Y+WKP2.Y)*0.5;
					PointClass pp2=new PointClass();
					pp2.X=pp1.X;
					pp2.Y=pp1.Y-10.0;

					int nScale=Convert.ToInt32(dScale);
					string ss="1:"+nScale.ToString();
					System.Drawing.Font dotNetFont=new System.Drawing.Font("����",1,FontStyle.Bold);
					ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
					((IFormattedTextSymbol)textSymbol).CharacterWidth=100;
					textSymbol.Size=5.0/25.4*72.0;
					textSymbol.Angle=dTextAF;
					textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
					textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
					TextElementClass textEle=new TextElementClass();
					textEle.Symbol = textSymbol;
					textEle.Text=ss;
					IElement element=(IElement)textEle;
					element.Geometry=pp2;
					pageCon.AddElement(element,0);
				}
            }
            #endregion 


            #region ���Ͻǽ�ϱ�
			if(sFW.Equals("��׼�ַ�")==true) 
			{
				PointClass[] ZJPos=new PointClass[8];
				if(true)
				{
					object o=Type.Missing;

					PointClass pp=new PointClass();
					pp.X=NKP4.X;
					pp.Y=NKP4.Y+16;
					for(int i=0;i<3;i++) 
					{
						for(int j=0;j<3;j++) 
						{
							PointClass MidP=new PointClass();
							double dx=i*15;
							double dy=j*8;
							double daf1=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3);
							((IConstructPoint)MidP).ConstructAngleDistance(pp,daf1,dx);
							PointClass BaseP=new PointClass();
							double daf2=CommonClassDLL.syc_CalAngle(ref NKP1,ref NKP4);
							((IConstructPoint)BaseP).ConstructAngleDistance(MidP,daf2,dy);

							//����p1,p2,p3,p4[��]
							PointClass p1=new PointClass();
							p1.X=BaseP.X;
							p1.Y=BaseP.Y;
							p1.Z=0.0;
							
							PointClass p2=new PointClass();
							((IConstructPoint)p2).ConstructAngleDistance(p1,daf1,15);

							PointClass p3=new PointClass();
							((IConstructPoint)p3).ConstructAngleDistance(p2,daf2,8);

							double daf3=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP4);
							PointClass p4=new PointClass();
							((IConstructPoint)p4).ConstructAngleDistance(p3,daf3,15);

							PolylineClass pol=new PolylineClass();
							((IPointCollection)pol).AddPoint(p1,ref o,ref o);
							((IPointCollection)pol).AddPoint(p2,ref o,ref o);
							((IPointCollection)pol).AddPoint(p3,ref o,ref o);
							((IPointCollection)pol).AddPoint(p4,ref o,ref o);
							((IPointCollection)pol).AddPoint(p1,ref o,ref o);
							lineSym=new SimpleLineSymbolClass();
							lineSym.Width=0.1;
							eleColor=new RgbColorClass();
							eleColor.Red=0;
							eleColor.Green=0;
							eleColor.Blue=0;
							lineSym.Color=eleColor;
							LineEle=new LineElementClass();
							LineEle.Geometry=pol;
							LineEle.Symbol=lineSym;
							pageCon.AddElement(LineEle,0);

							if(i==1 && j==1) 
							{
								PolygonClass pog=new PolygonClass();
								((IPointCollection)pog).AddPoint(p1,ref o,ref o);
								((IPointCollection)pog).AddPoint(p2,ref o,ref o);
								((IPointCollection)pog).AddPoint(p3,ref o,ref o);
								((IPointCollection)pog).AddPoint(p4,ref o,ref o);
								fillSym=new SimpleFillSymbolClass();
								fillSym.Style=esriSimpleFillStyle.esriSFSBackwardDiagonal;
								lineSym=new SimpleLineSymbolClass();
								lineSym.Style=esriSimpleLineStyle.esriSLSNull;
								fillSym.Outline=lineSym;
								PolygonElementClass pogEle=new PolygonElementClass();
								pogEle.Geometry=pog;
								pogEle.Symbol=fillSym;
								pageCon.AddElement(pogEle,0);
							}

							PointClass CenterP=new PointClass();
							CenterP.X=(p1.X+p3.X)*0.5;
							CenterP.Y=(p1.Y+p3.Y)*0.5;
							if(i==0 && j==0)
								ZJPos[0]=CenterP;
							else if(i==1 && j==0)
								ZJPos[1]=CenterP;
							else if(i==2 && j==0)
								ZJPos[2]=CenterP;
							else if(i==2 && j==1)
								ZJPos[3]=CenterP;
							else if(i==2 && j==2)
								ZJPos[4]=CenterP;
							else if(i==1 && j==2)
								ZJPos[5]=CenterP;
							else if(i==0 && j==2)
								ZJPos[6]=CenterP;
							else if(i==0 &&j==1)
								ZJPos[7]=CenterP;
						}
					}

					double dDJ=(DLIB.DFM2D(m_dJ1)+DLIB.DFM2D(m_dJ3))*0.5;
					double dDW=(DLIB.DFM2D(m_dW1)+DLIB.DFM2D(m_dW3))*0.5;
					double dDelJ=0.0,dDelW=0.0;
					DLIB.GetDelDFM(dScale,ref dDelJ,ref dDelW);
					double dDelJ_D=DLIB.DFM2D(dDelJ);
					double dDelW_D=DLIB.DFM2D(dDelW);

					double[] dJSz=new double[8];
					double[] dWSz=new double[8];
					dJSz[0]=dDJ-dDelJ_D;
					dWSz[0]=dDW-dDelW_D;
					dJSz[1]=dDJ;
					dWSz[1]=dDW-dDelW_D;
					dJSz[2]=dDJ+dDelJ_D;
					dWSz[2]=dDW-dDelW_D;
					dJSz[3]=dDJ+dDelJ_D;
					dWSz[3]=dDW;
					dJSz[4]=dDJ+dDelJ_D;
					dWSz[4]=dDW+dDelW_D;
					dJSz[5]=dDJ;
					dWSz[5]=dDW+dDelW_D;
					dJSz[6]=dDJ-dDelJ_D;
					dWSz[6]=dDW+dDelW_D;
					dJSz[7]=dDJ-dDelJ_D;
					dWSz[7]=dDW;

					string[] sTFHSz=new string[8];
					for(int i=0;i<8;i++) 
					{
						double dJ_D=dJSz[i];
						double dW_D=dWSz[i];
						double dJ=DLIB.HD2DFM(dJ_D*Math.PI/180.0);
						double dW=DLIB.HD2DFM(dW_D*Math.PI/180.0);
						StringBuilder ss=new StringBuilder(100);
						double dJ1=0.0,dW1=0.0,dJ3=0.0,dW3=0.0;
						DLIB.GetNewWaima(dScale,dJ,dW,ss);
						string sTFH=ss.ToString();
						sTFHSz[i]=sTFH;
					}

					for(int i=0;i<8;i++) 
					{
						System.Drawing.Font dotNetFont=new System.Drawing.Font("����",1);
						ITextSymbol textSymbol = new TextSymbolClass();
                        textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
						double dZH=1.80;	//mm
						textSymbol.Size=dZH/25.4*72.0;
						double daf=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3)*180.0/Math.PI;
						textSymbol.Angle=daf;
						textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHACenter;
						textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVACenter;
						TextElementClass textEle=new TextElementClass();
						textEle.Symbol = textSymbol;
						textEle.Text=sTFHSz[i];
						IElement element=(IElement)textEle;
						element.Geometry=ZJPos[i];
						pageCon.AddElement(element,0);
					}
				}
            }
            #endregion

            #region   //������
            string sErrorInfo="";
			IPoint LDP=new PointClass();
			CommonClassDLL.syc_JWD2XY(myMap,m_dJ1,m_dW1,ref LDP,out sErrorInfo);
			IPoint RDP=new PointClass();
			CommonClassDLL.syc_JWD2XY(myMap,m_dJ3,m_dW1,ref RDP,out sErrorInfo);
			IPoint RUP=new PointClass();
			CommonClassDLL.syc_JWD2XY(myMap,m_dJ3,m_dW3,ref RUP,out sErrorInfo);
			IPoint LUP=new PointClass();
			CommonClassDLL.syc_JWD2XY(myMap,m_dJ1,m_dW3,ref LUP,out sErrorInfo);

			int nXGS=0;
			FLW[] pXFLW=new FLW[500];
			int nBJ=0;
			int nLastX_KM=0;
			IPoint LastP=new PointClass();
			while(true)
			{
				FLW flw=new FLW();
				if(nBJ==0) 
				{
					nBJ++;

					//��һ��:
					int nX=(int)(LDP.X/1000.0);
					int nX2=nX+1;
					double dDel=(nX2*1000.0-LDP.X)/dScale*1000.0;

					pLine=new LineClass();
					pLine.FromPoint=NKP1;
					pLine.ToPoint=NKP2;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP2.X)*0.5;
					p1.Y=(NKP1.Y+NKP2.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP4.X)*0.5;
					p2.Y=(NKP3.Y+NKP4.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p2,ref p1);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nX2.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pXFLW[nXGS]=flw;
					nXGS++;

					nLastX_KM=nX2;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
				} 
				else 
				{
					//�ǵ�һ��:
					int nCurX_KM=nLastX_KM+1;
					int nDist_KM=1;
					double dDel=(nDist_KM*1000.0)/dScale*1000.0;	//mm

					pLine=new LineClass();
					pLine.FromPoint=LastP;
					pLine.ToPoint=NKP2;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP2.X)*0.5;
					p1.Y=(NKP1.Y+NKP2.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP4.X)*0.5;
					p2.Y=(NKP3.Y+NKP4.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p2,ref p1);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nCurX_KM.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pXFLW[nXGS]=flw;
					nXGS++;

					nLastX_KM=nCurX_KM;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
					if(flw.PP1.X>NKP2.X) 
					{
						nXGS=nXGS-1;
						break;
					}
				}
			} //while(1)
			
			for(int i=0;i<nXGS;i++) 
			{
				object oo=Type.Missing;
				FLW flw=pXFLW[i] as FLW;

				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(flw.PP1,ref oo,ref oo);
				((IPointCollection)pol).AddPoint(flw.PP2,ref oo,ref oo);
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				//Text:
				pLine=new LineClass();
				pLine.FromPoint=flw.PP1;
				pLine.ToPoint=flw.PP2;
				IPoint ZJP=new PointClass();
				((IConstructPoint)ZJP).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,9.0,false);

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=2.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ1;
				IElement element=(IElement)textEle;
				element.Geometry=ZJP;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ2;
				element=(IElement)textEle;
				element.Geometry=ZJP;
				pageCon.AddElement(element,0);
			}

			
			int nSGS=0;
			FLW[] pSFLW=new FLW[500];
			nBJ=0;
			nLastX_KM=0;
			LastP=new PointClass();
			while(true)
			{
				FLW flw=new FLW();
				if(nBJ==0) 
				{
					nBJ++;

					//��һ��:
					int nX=(int)(LUP.X/1000.0);
					int nX2=nX+1;
					double dDel=(nX2*1000.0-LUP.X)/dScale*1000.0;

					pLine=new LineClass();
					pLine.FromPoint=NKP4;
					pLine.ToPoint=NKP3;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP2.X)*0.5;
					p1.Y=(NKP1.Y+NKP2.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP4.X)*0.5;
					p2.Y=(NKP3.Y+NKP4.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p1,ref p2);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nX2.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pSFLW[nSGS]=flw;
					nSGS++;

					nLastX_KM=nX2;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
				} 
				else 
				{
					//�ǵ�һ��:
					int nCurX_KM=nLastX_KM+1;
					int nDist_KM=1;
					double dDel=(nDist_KM*1000.0)/dScale*1000.0;	//mm

					pLine=new LineClass();
					pLine.FromPoint=LastP;
					pLine.ToPoint=NKP3;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP2.X)*0.5;
					p1.Y=(NKP1.Y+NKP2.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP4.X)*0.5;
					p2.Y=(NKP3.Y+NKP4.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p1,ref p2);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nCurX_KM.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pSFLW[nSGS]=flw;
					nSGS++;

					nLastX_KM=nCurX_KM;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
					if(flw.PP1.X>NKP3.X) 
					{
						nSGS=nSGS-1;
						break;
					}
				}
			} //while(1)
			
			for(int i=0;i<nSGS;i++) 
			{
				object oo=Type.Missing;
				FLW flw=pSFLW[i] as FLW;

				//��:PP1-PP2
				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(flw.PP1,ref oo,ref oo);
				((IPointCollection)pol).AddPoint(flw.PP2,ref oo,ref oo);
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				//Text:
				pLine=new LineClass();
				pLine.FromPoint=flw.PP1;
				pLine.ToPoint=flw.PP2;
				IPoint ZJP=new PointClass();
				((IConstructPoint)ZJP).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,11.0,false);

				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=2.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ1;
				IElement element=(IElement)textEle;
				element.Geometry=ZJP;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ2;
				element=(IElement)textEle;
				element.Geometry=ZJP;
				pageCon.AddElement(element,0);
			}


			int nZGS=0;
			FLW[] pZFLW=new FLW[500];
			nBJ=0;
			int nLastY_KM=0;
			LastP=new PointClass();
			while(true)
			{
				FLW flw=new FLW();
				if(nBJ==0) 
				{
					nBJ++;

					//��һ��:
					int nY=(int)(LDP.Y/1000.0);
					int nY2=nY+1;
					double dDel=(nY2*1000.0-LDP.Y)/dScale*1000.0;

					pLine=new LineClass();
					pLine.FromPoint=NKP1;
					pLine.ToPoint=NKP4;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP4.X)*0.5;
					p1.Y=(NKP1.Y+NKP4.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP2.X)*0.5;
					p2.Y=(NKP3.Y+NKP2.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p2,ref p1);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nY2.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pZFLW[nZGS]=flw;
					nZGS++;

					nLastY_KM=nY2;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
				} 
				else 
				{
					//�ǵ�һ��:
					int nCurY_KM=nLastY_KM+1;
					int nDist_KM=1;
					double dDel=(nDist_KM*1000.0)/dScale*1000.0;	//mm

					pLine=new LineClass();
					pLine.FromPoint=LastP;
					pLine.ToPoint=NKP4;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP4.X)*0.5;
					p1.Y=(NKP1.Y+NKP4.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP2.X)*0.5;
					p2.Y=(NKP3.Y+NKP2.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p2,ref p1);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nCurY_KM.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pZFLW[nZGS]=flw;
					nZGS++;

					nLastY_KM=nCurY_KM;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
					if(flw.PP1.Y>NKP4.Y) 
					{
						nZGS=nZGS-1;
						break;
					}
				}
			} //while(1)
			
			for(int i=0;i<nZGS;i++) 
			{
				object oo=Type.Missing;
				FLW flw=pZFLW[i] as FLW;

				//��:PP1-PP2
				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(flw.PP1,ref oo,ref oo);
				((IPointCollection)pol).AddPoint(flw.PP2,ref oo,ref oo);
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				//Text:
				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=2.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ1;
				IElement element=(IElement)textEle;
				element.Geometry=flw.PP2;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ2;
				element=(IElement)textEle;
				element.Geometry=flw.PP1;
				pageCon.AddElement(element,0);
			}

			int nYGS=0;
			FLW[] pYFLW=new FLW[500];
			nBJ=0;
			nLastY_KM=0;
			LastP=new PointClass();
			while(true)
			{
				FLW flw=new FLW();
				if(nBJ==0) 
				{
					nBJ++;

					//��һ��:
					int nY=(int)(RDP.Y/1000.0);
					int nY2=nY+1;
					double dDel=(nY2*1000.0-RDP.Y)/dScale*1000.0;

					pLine=new LineClass();
					pLine.FromPoint=NKP2;
					pLine.ToPoint=NKP3;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP4.X)*0.5;
					p1.Y=(NKP1.Y+NKP4.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP2.X)*0.5;
					p2.Y=(NKP3.Y+NKP2.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p1,ref p2);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nY2.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pYFLW[nYGS]=flw;
					nYGS++;

					nLastY_KM=nY2;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
				} 
				else 
				{
					//�ǵ�һ��:
					int nCurY_KM=nLastY_KM+1;
					int nDist_KM=1;
					double dDel=(nDist_KM*1000.0)/dScale*1000.0;	//mm

					pLine=new LineClass();
					pLine.FromPoint=LastP;
					pLine.ToPoint=NKP3;
					IPoint PP1=new PointClass();
					((IConstructPoint)PP1).ConstructAlong(pLine,esriSegmentExtension.esriExtendTangents,dDel,false);
					flw.PP1=PP1;

					IPoint p1=new PointClass();
					p1.X=(NKP1.X+NKP4.X)*0.5;
					p1.Y=(NKP1.Y+NKP4.Y)*0.5;
					IPoint p2=new PointClass();
					p2.X=(NKP3.X+NKP2.X)*0.5;
					p2.Y=(NKP3.Y+NKP2.Y)*0.5;
					double dAF=CommonClassDLL.syc_CalAngle(ref p1,ref p2);
					IPoint PP2=new PointClass();
					((IConstructPoint)PP2).ConstructAngleDistance(PP1,dAF,12.0);
					flw.PP2=PP2;
					
					string sBS=nCurY_KM.ToString();
					int nLen=sBS.Length;
					string sZJ1=sBS.Substring(0,nLen-2);
					string sZJ2=sBS.Substring(nLen-2,2);
					flw.sZJ1=sZJ1;
					flw.sZJ2=sZJ2;
					flw.sBS=sBS;

					pYFLW[nYGS]=flw;
					nYGS++;

					nLastY_KM=nCurY_KM;
					LastP.PutCoords(flw.PP1.X,flw.PP1.Y);
					if(flw.PP1.Y>NKP3.Y) 
					{
						nYGS=nYGS-1;
						break;
					}
				}
			} //while(1)
			
			for(int i=0;i<nYGS;i++) 
			{
				object oo=Type.Missing;
				FLW flw=pYFLW[i] as FLW;

				//��:PP1-PP2
				PolylineClass pol=new PolylineClass();
				((IPointCollection)pol).AddPoint(flw.PP1,ref oo,ref oo);
				((IPointCollection)pol).AddPoint(flw.PP2,ref oo,ref oo);
				lineSym=new SimpleLineSymbolClass();
				lineSym.Width=0.1;
				eleColor=new RgbColorClass();
				eleColor.Red=0;
				eleColor.Green=0;
				eleColor.Blue=0;
				lineSym.Color=eleColor;
				LineEle=new LineElementClass();
				LineEle.Geometry=pol;
				LineEle.Symbol=lineSym;
				pageCon.AddElement(LineEle,0);

				//Text:
				System.Drawing.Font dotNetFont=new System.Drawing.Font("Tahoma",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=2.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ1;
				IElement element=(IElement)textEle;
				element.Geometry=flw.PP1;
				pageCon.AddElement(element,0);

				textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				dZH=3.0;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHARight;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVABottom;
				textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text=flw.sZJ2;
				element=(IElement)textEle;
				element.Geometry=flw.PP2;
				pageCon.AddElement(element,0);
            }
            #endregion 

            if (chkTKFLW.Checked==false) 
			{
				ArrayList LeftRightLines=new ArrayList();
				for(int i=0;i<nZGS;i++) 
				{
					string sLeftBS=pZFLW[i].sBS;
					bool bExist=false;
					int nIndex=-1;
					for(int j=0;j<nYGS;j++) 
					{
						if(pYFLW[j].sBS.Equals(sLeftBS)==true) 
						{
							bExist=true;
							nIndex=j;
							break;
						}
					}
					if(bExist==true) 
					{
						object oo=Type.Missing;
						PolylineClass pol=new PolylineClass();
						((IPointCollection)pol).AddPoint(pZFLW[i].PP1,ref oo,ref oo);
						((IPointCollection)pol).AddPoint(pYFLW[nIndex].PP1,ref oo,ref oo);
						LeftRightLines.Add(pol);
					}
				}

				ArrayList UpperDownLines=new ArrayList();
				for(int i=0;i<nXGS;i++) 
				{
					string sDownBS=pXFLW[i].sBS;
					bool bExist=false;
					int nIndex=-1;
					for(int j=0;j<nSGS;j++) 
					{
						if(pSFLW[j].sBS.Equals(sDownBS)==true) 
						{
							bExist=true;
							nIndex=j;
							break;
						}
					}
					if(bExist==true) 
					{
						object oo=Type.Missing;
						PolylineClass pol=new PolylineClass();
						((IPointCollection)pol).AddPoint(pXFLW[i].PP1,ref oo,ref oo);
						((IPointCollection)pol).AddPoint(pSFLW[nIndex].PP1,ref oo,ref oo);
						UpperDownLines.Add(pol);
					}
				}

				int nGS1=LeftRightLines.Count;
				for(int i=0;i<nGS1;i++) 
				{
					IPolyline LRLine=LeftRightLines[i] as IPolyline;
					IPointCollection pCol1=LRLine as IPointCollection;
					IPoint P1=pCol1.get_Point(0);
					IPoint P2=pCol1.get_Point(1);
					double dA1=CommonClassDLL.syc_CalAngle(ref P1,ref P2);

					int nGS2=UpperDownLines.Count;
					for(int j=0;j<nGS2;j++) 
					{
						IPolyline UDLine=UpperDownLines[j] as IPolyline;
						IPointCollection pCol2=UDLine as IPointCollection;
						IPoint PP1=pCol2.get_Point(0);
						IPoint PP2=pCol2.get_Point(1);
						double dA2=CommonClassDLL.syc_CalAngle(ref PP1,ref PP2);

						IPoint JP=new PointClass();
						((IConstructPoint)JP).ConstructAngleIntersection(P1,dA1,PP1,dA2);	//Intersection

						//��JPΪ����,dA1+dA2����4��:
						IPoint p1=new PointClass();
						((IConstructPoint)p1).ConstructAngleDistance(JP,dA1,5.0);
						IPoint p2=new PointClass();
						((IConstructPoint)p2).ConstructAngleDistance(JP,dA1+Math.PI,5.0);
						IPoint p3=new PointClass();
						((IConstructPoint)p3).ConstructAngleDistance(JP,dA2,5.0);
						IPoint p4=new PointClass();
						((IConstructPoint)p4).ConstructAngleDistance(JP,dA2+Math.PI,5.0);

						//��:p1-p2;p3-p4
						object oo=Type.Missing;
						PolylineClass pol=new PolylineClass();
						((IPointCollection)pol).AddPoint(p1,ref oo,ref oo);
						((IPointCollection)pol).AddPoint(p2,ref oo,ref oo);
						lineSym=new SimpleLineSymbolClass();
						lineSym.Width=0.1;
						eleColor=new RgbColorClass();
						eleColor.Red=0;
						eleColor.Green=0;
						eleColor.Blue=0;
						lineSym.Color=eleColor;
						LineEle=new LineElementClass();
						LineEle.Geometry=pol;
						LineEle.Symbol=lineSym;
						pageCon.AddElement(LineEle,0);

						pol=new PolylineClass();
						((IPointCollection)pol).AddPoint(p3,ref oo,ref oo);
						((IPointCollection)pol).AddPoint(p4,ref oo,ref oo);
						lineSym=new SimpleLineSymbolClass();
						lineSym.Width=0.1;
						eleColor=new RgbColorClass();
						eleColor.Red=0;
						eleColor.Green=0;
						eleColor.Blue=0;
						lineSym.Color=eleColor;
						LineEle=new LineElementClass();
						LineEle.Geometry=pol;
						LineEle.Symbol=lineSym;
						pageCon.AddElement(LineEle,0);
					}
				}
			}


            //2010,1,18 �ţ�
			if(true) 
			{
				//�õ� "ͼ��" ·�������е�TIF�ļ�:
				ArrayList pLegendFiles=new ArrayList();
				DirectoryInfo legendDir=new DirectoryInfo(Application.StartupPath+@"\ͼ��");
				FileInfo[] legendFileNames=legendDir.GetFiles();
				foreach(FileInfo curFile in legendFileNames) 
				{
					string sFileName=curFile.Name;
					int nPos=sFileName.IndexOf(".");
					string sExt=sFileName.Substring(nPos+1).Trim().ToUpper();
					if(sExt.Equals("TIF")==true) 
					{
						pLegendFiles.Add(sFileName);	//����·��
					}
				}

				ArrayList MSz=new ArrayList();
				for(int i=0;i<myMap.LayerCount;i++) 
				{
					ILayer lyr=myMap.get_Layer(i);
					if(lyr is IFeatureLayer) 
					{
						IFeatureLayer pFeatLayer= lyr as IFeatureLayer;
						IFeatureClass featCls=((IFeatureLayer)lyr).FeatureClass;
						IDataset dataset=featCls as IDataset;
						string sBName=dataset.BrowseName.Trim().ToUpper();
						int nPos=sBName.LastIndexOf(".");
						if(nPos!=-1) 
						{
							string ss=sBName.Substring(nPos+1);
							sBName=ss.Trim().ToUpper();
						}
                        sBName = OtherHelper.GetLeftName(sBName, "_");
						if( (sBName.Equals("DLTB")==true) ||
							(sBName.Equals("LXDW")==true) )
						{
							ISpatialFilter pFilter=new SpatialFilterClass();
							pFilter.SpatialRel=esriSpatialRelEnum.esriSpatialRelContains;
							IGeometry pSearchGeo=m_bufferPolygon as IGeometry;
							pFilter.Geometry=pSearchGeo;
							string sFldName=featCls.ShapeFieldName;
							pFilter.GeometryField=sFldName;
							pFilter.set_OutputSpatialReference(sFldName,myMap.SpatialReference);
							IFeatureCursor pFeatCursor=pFeatLayer.Search(pFilter,false);
							IFeature pFeat=pFeatCursor.NextFeature();
							while(pFeat!=null) 
							{
								nPos=pFeat.Fields.FindField("DLBM");
								object oo=null;
								if(nPos==-1);
								else 
									oo=pFeat.get_Value(nPos);
								if(oo==null || oo is System.DBNull);
								else 
								{
									string s1=(string)oo;
									string sCode=s1.Trim();
									int nn=sCode.IndexOf("|");
									if(nn!=-1) 
									{
										string ss=sCode.Substring(0,nn);
										sCode=ss.Trim();
									}
									if( sCode.Length!=0 && MSz.Contains(sCode)==false)
										MSz.Add(sCode);
								}
								pFeat=pFeatCursor.NextFeature();
							} //while
						}  //if
					}
				}

				ArrayList XSz=new ArrayList();
				for(int i=0;i<myMap.LayerCount;i++) 
				{
					ILayer lyr=myMap.get_Layer(i);
					if(lyr is IFeatureLayer) 
					{
						IFeatureLayer pFeatLayer= lyr as IFeatureLayer;
						IFeatureClass featCls=((IFeatureLayer)lyr).FeatureClass;
						IDataset dataset=featCls as IDataset;
						string sBName=dataset.BrowseName;
						int nPos=sBName.LastIndexOf(".");
						if(nPos!=-1) 
						{
							string ss=sBName.Substring(nPos+1).Trim().ToUpper();
							sBName=ss.Trim().ToUpper();
						}
                       // sBName = OtherHelper.GetLeftName(sBName, "_");
                        if (OtherHelper.GetLeftName(sBName, "_").Equals("XZDW") == true)
						{
							ISpatialFilter pFilter=new SpatialFilterClass();
							pFilter.SpatialRel=esriSpatialRelEnum.esriSpatialRelContains;
							IGeometry pSearchGeo=m_bufferPolygon as IGeometry;
							pFilter.Geometry=pSearchGeo;
							string sFldName=featCls.ShapeFieldName;
							pFilter.GeometryField=sFldName;
							pFilter.set_OutputSpatialReference(sFldName,myMap.SpatialReference);
							IFeatureCursor pFeatCursor=pFeatLayer.Search(pFilter,false);
							IFeature pFeat=pFeatCursor.NextFeature();
							while(pFeat!=null) 
							{
								nPos=pFeat.Fields.FindField("DLBM");
								object oA=null;
								if(nPos==-1);
								else 
									oA=pFeat.get_Value(nPos);
								if(oA==null || oA is System.DBNull);
								else 
								{
									string sCode=((string)oA).Trim();
									int nn=sCode.IndexOf("|");
									if(nn!=-1) 
									{
										string ss=sCode.Substring(0,nn);
										sCode=ss;
									}
									if( sCode.Length!=0 && XSz.Contains(sCode)==false)
										XSz.Add(sCode);
								}
								pFeat=pFeatCursor.NextFeature();
							}
						}  //if
					}
				}

				ArrayList OtherSz=new ArrayList();

                #region   ���ֽ���:���XZQJX���ڵ�JXLX��ֵ�������µĶ�Ӧ��ϵ:
                //		620200	����.tif
				//		630200	ʡ����������ֱϽ�н�.tif
				//		640200	�������ݡ��ؼ��н�.tif
				//		650200	�ء������ؼ��н�.tif
				//		660200	�硢�򡢽ֵ���.tif
				//		670500	���.tif
				for(int i=0;i<myMap.LayerCount;i++) 
				{
					ILayer lyr=myMap.get_Layer(i);
					if(lyr is IFeatureLayer) 
					{
						IFeatureLayer pFeatLayer= lyr as IFeatureLayer;
						IFeatureClass featCls=((IFeatureLayer)lyr).FeatureClass;
						IDataset dataset=featCls as IDataset;
						string sBName=dataset.BrowseName;
						int nPos=sBName.LastIndexOf(".");
						if(nPos!=-1) 
						{
							string ss=sBName.Substring(nPos+1).Trim().ToUpper();
							sBName=ss.Trim().ToUpper();
						}
                        if (OtherHelper.GetLeftName(sBName, "_").Equals("XZQJX") == true)
						{
							ISpatialFilter pFilter=new SpatialFilterClass();
							pFilter.SpatialRel=esriSpatialRelEnum.esriSpatialRelContains;
							IGeometry pSearchGeo=m_bufferPolygon as IGeometry;
							pFilter.Geometry=pSearchGeo;
							string sFldName=featCls.ShapeFieldName;
							pFilter.GeometryField=sFldName;
							pFilter.set_OutputSpatialReference(sFldName,myMap.SpatialReference);
							IFeatureCursor pFeatCursor=pFeatLayer.Search(pFilter,false);
							IFeature pFeat=pFeatCursor.NextFeature();
							while(pFeat!=null) 
							{
								nPos=pFeat.Fields.FindField("JXLX");
								object oA=null;
								if(nPos==-1);
								else 
									oA=pFeat.get_Value(nPos);
								if(oA==null || oA is System.DBNull);
								else 
								{
									string sCode=((string)oA).Trim();
									string sTIF="";
									if(sCode.Equals("620200")==true)
										sTIF="����.tif";
									else if(sCode.Equals("630200")==true)
										sTIF="ʡ����������ֱϽ�н�.tif";
									else if(sCode.Equals("640200")==true)
										sTIF="�������ݡ��ؼ��н�.tif";
									else if(sCode.Equals("650200")==true)
										sTIF="�ء������ؼ��н�.tif";
									else if(sCode.Equals("660200")==true)
										sTIF="�硢�򡢽ֵ���.tif";
									else if(sCode.Equals("670500")==true)
										sTIF="���.tiff";
									if(sTIF.Length!=0 && OtherSz.Contains(sTIF)==false)
										OtherSz.Add(sTIF);
								}
								pFeat=pFeatCursor.NextFeature();
							}
						}  //if
					}
                }
                #endregion 


               


                #region  �ȸ���: ���DGX���ڵ�DGXLX�ֶ�:
                //		710101		������
				//		710102		������
				for(int i=0;i<myMap.LayerCount;i++) 
				{
					ILayer lyr=myMap.get_Layer(i);
					if(lyr is IFeatureLayer) 
					{
						IFeatureLayer pFeatLayer= lyr as IFeatureLayer;
						IFeatureClass featCls=((IFeatureLayer)lyr).FeatureClass;
						IDataset dataset=featCls as IDataset;
						string sBName=dataset.BrowseName;
						int nPos=sBName.LastIndexOf(".");
						if(nPos!=-1) 
						{
							string ss=sBName.Substring(nPos+1).Trim().ToUpper();
							sBName=ss.Trim().ToUpper();
						}
                        if (OtherHelper.GetLeftName(sBName, "_").Equals("DGX") == true)
						{
							ISpatialFilter pFilter=new SpatialFilterClass();
							pFilter.SpatialRel=esriSpatialRelEnum.esriSpatialRelContains;
							IGeometry pSearchGeo=m_bufferPolygon as IGeometry;
							pFilter.Geometry=pSearchGeo;
							string sFldName=featCls.ShapeFieldName;
							pFilter.GeometryField=sFldName;
							pFilter.set_OutputSpatialReference(sFldName,myMap.SpatialReference);
							IFeatureCursor pFeatCursor=pFeatLayer.Search(pFilter,false);
							IFeature pFeat=pFeatCursor.NextFeature();
							while(pFeat!=null) 
							{
								nPos=pFeat.Fields.FindField("DGXLX");
								object oA=null;
								if(nPos==-1);
								else 
									oA=pFeat.get_Value(nPos);
								if(oA==null || oA is System.DBNull);
								else 
								{
									string sCode=((string)oA).Trim();
									string sTIF="";
									if(sCode.Equals("710101")==true)
										sTIF="������.tif";
									else if(sCode.Equals("710102")==true)
										sTIF="������.tif";

									if(sTIF.Length!=0 && OtherSz.Contains(sTIF)==false)
										OtherSz.Add(sTIF);
								}
								pFeat=pFeatCursor.NextFeature();
							}
						}  //if
					}
                }
                #endregion 


                #region ���ǵ㣫�߳̿��Ƶ�: ���CLKZD���ڵ�KZDLX�ֶ�:
				//		110102		���ǵ�.tif
				//		110200		�̵߳�.tif
				for(int i=0;i<myMap.LayerCount;i++) 
				{
					ILayer lyr=myMap.get_Layer(i);
					if(lyr is IFeatureLayer) 
					{
						IFeatureLayer pFeatLayer= lyr as IFeatureLayer;
						IFeatureClass featCls=((IFeatureLayer)lyr).FeatureClass;
						IDataset dataset=featCls as IDataset;
						string sBName=dataset.BrowseName;
						int nPos=sBName.LastIndexOf(".");
						if(nPos!=-1) 
						{
							string ss=sBName.Substring(nPos+1).Trim().ToUpper();
							sBName=ss.Trim().ToUpper();
						}
                        if (OtherHelper.GetLeftName(sBName, "_").Equals("CLKZD") == true)
						{
							ISpatialFilter pFilter=new SpatialFilterClass();
							pFilter.SpatialRel=esriSpatialRelEnum.esriSpatialRelContains;
							IGeometry pSearchGeo=m_bufferPolygon as IGeometry;
							pFilter.Geometry=pSearchGeo;
							string sFldName=featCls.ShapeFieldName;
							pFilter.GeometryField=sFldName;
							pFilter.set_OutputSpatialReference(sFldName,myMap.SpatialReference);
							IFeatureCursor pFeatCursor=pFeatLayer.Search(pFilter,false);
							IFeature pFeat=pFeatCursor.NextFeature();
							while(pFeat!=null) 
							{
								nPos=pFeat.Fields.FindField("KZDLX");
								object oA=null;
								if(nPos==-1);
								else 
									oA=pFeat.get_Value(nPos);
								if(oA==null || oA is System.DBNull);
								else 
								{
									string sCode=((string)oA).Trim();
									string sTIF="";
									if(sCode.Equals("110102")==true)
										sTIF="���ǵ�.tif";
									else if(sCode.Equals("110200")==true)
										sTIF="�̵߳�.tif";

									if(sTIF.Length!=0 && OtherSz.Contains(sTIF)==false)
										OtherSz.Add(sTIF);
								}
								pFeat=pFeatCursor.NextFeature();
							}
						}  //if
					}
                }
                #endregion 


                #region ͼ��
                //ͼ��:
				IPoint TLP=new PointClass();
				double dA=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3);
				((IConstructPoint)TLP).ConstructAngleDistance(tmpP3,dA,15.0);

				System.Drawing.Font dotNetFont=new System.Drawing.Font("����",1);
				ITextSymbol textSymbol = new TextSymbolClass();
                textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
				double dZH=6.5;	//mm
				textSymbol.Size=dZH/25.4*72.0;
				textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHACenter;
				textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVATop;
				TextElementClass textEle=new TextElementClass();
				textEle.Symbol = textSymbol;
				textEle.Text="ͼ ��";
				IElement element=(IElement)textEle;
				element.Geometry=TLP;
				pageCon.AddElement(element,0);
				
				dA=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3);
				IPoint PP=new PointClass();
				((IConstructPoint)PP).ConstructAngleDistance(tmpP3,dA,3.0);
				dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
				IPoint FirstP=new PointClass();
				((IConstructPoint)FirstP).ConstructAngleDistance(PP,dA,12.5);
				MSz.Sort(); //small-->big DLCode
				if(MSz.Count!=0) 
				{
					for(int iM=0;iM<MSz.Count;iM++) 
					{
						//3bit�������:
						string sCurCode=MSz[iM] as string;

						//��pLegendFiles�����Ƿ���ڸ�ͼ��:
						string sTLName="";
						string sTLFile="";
						for(int k=0;k<pLegendFiles.Count;k++) 
						{
							string sFileName=pLegendFiles[k] as string;
							int nPos=sFileName.IndexOf("-");
							if(nPos!=-1) 
							{
								string sTmpCode=sFileName.Substring(0,nPos);
								if(sTmpCode.Equals(sCurCode)==true) 
								{
									int nPos2=sFileName.IndexOf(".");
									if(nPos2!=-1) 
									{
										sTLName=sFileName.Substring(nPos+1,nPos2-nPos-1);
										sTLFile=Application.StartupPath+@"\ͼ��\"+sFileName;
										break;
									}
								}
							}
						}
						
						//Create Elements:
						if(sTLName.Length!=0 && sTLFile.Length!=0) 
						{
							TifPictureElementClass jpgEle=new TifPictureElementClass();
                            jpgEle.SavePictureInDocument = true;
							jpgEle.ImportPictureFromFile(sTLFile);
							jpgEle.MaintainAspectRatio=true;

							double dDX=22.0;
							double dDY=12.0;
							IPoint pp1=new PointClass();
							pp1.PutCoords(FirstP.X,FirstP.Y);

							dA=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3);
							IPoint pp2=new PointClass();
							((IConstructPoint)pp2).ConstructAngleDistance(pp1,dA,dDX);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
							IPoint pp3=new PointClass();
							((IConstructPoint)pp3).ConstructAngleDistance(pp2,dA,dDY);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP4);
							IPoint pp4=new PointClass();
							((IConstructPoint)pp4).ConstructAngleDistance(pp3,dA,dDX);

							PolygonClass pos=new PolygonClass();
							pCol=(IPointCollection)pos;
							Missing=Type.Missing;
							pCol.AddPoint(pp1,ref Missing,ref Missing);
							pCol.AddPoint(pp2,ref Missing,ref Missing);
							pCol.AddPoint(pp3,ref Missing,ref Missing);
							pCol.AddPoint(pp4,ref Missing,ref Missing);
							((IElement)jpgEle).Geometry=pos;
							pageCon.AddElement(jpgEle,0);

							dA=CommonClassDLL.syc_CalAngle(ref pp1,ref pp2);
							IPoint pp=new PointClass();
							pp.X=(pp2.X+pp3.X)*0.5;
							pp.Y=(pp2.Y+pp3.Y)*0.5;
							IPoint pp5=new PointClass();
							((IConstructPoint)pp5).ConstructAngleDistance(pp,dA,10.0);

							dotNetFont=new System.Drawing.Font("����",1);
							textSymbol = new TextSymbolClass();
                            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
							dZH=3.5;	//mm
							textSymbol.Size=dZH/25.4*72.0;
							textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
							textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVACenter;
							textEle=new TextElementClass();
							textEle.Symbol = textSymbol;
							textEle.Text=sTLName;
							element=(IElement)textEle;
							element.Geometry=pp5;
							pageCon.AddElement(element,0);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
							IPoint newP=new PointClass();
							((IConstructPoint)newP).ConstructAngleDistance(pp1,dA,15.0);	//2ͼ�������
							FirstP.X=newP.X;
							FirstP.Y=newP.Y;
						}
					} //for(int iM=0;...
				} //if(MSz.Count!=0)


				XSz.Sort();
				if(XSz.Count!=0) 
				{
					for(int iX=0;iX<XSz.Count;iX++) 
					{
						//3bit�������:
						string sCurCode=XSz[iX] as string;
						sCurCode=sCurCode+"1";

						//��pLegendFiles�����Ƿ���ڸ�ͼ��:
						string sTLName="";
						string sTLFile="";
						for(int k=0;k<pLegendFiles.Count;k++) 
						{
							string sFileName=pLegendFiles[k] as string;
							int nPos=sFileName.IndexOf("-");
							if(nPos!=-1) 
							{
								string sTmpCode=sFileName.Substring(0,nPos);
								if(sTmpCode.Equals(sCurCode)==true) 
								{
									int nPos2=sFileName.IndexOf(".");
									if(nPos2!=-1) 
									{
										sTLName=sFileName.Substring(nPos+1,nPos2-nPos-1);
										sTLFile=Application.StartupPath+@"\ͼ��\"+sFileName;
										break;
									}
								}
							}
						}
						
						//Create Elements:
						if(sTLName.Length!=0 && sTLFile.Length!=0) 
						{
							TifPictureElementClass jpgEle=new TifPictureElementClass();					
							jpgEle.ImportPictureFromFile(sTLFile);
							jpgEle.MaintainAspectRatio=true;

							double dDX=22.0;
							double dDY=12.0;
							IPoint pp1=new PointClass();
							pp1.PutCoords(FirstP.X,FirstP.Y);

							dA=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3);
							IPoint pp2=new PointClass();
							((IConstructPoint)pp2).ConstructAngleDistance(pp1,dA,dDX);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
							IPoint pp3=new PointClass();
							((IConstructPoint)pp3).ConstructAngleDistance(pp2,dA,dDY);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP4);
							IPoint pp4=new PointClass();
							((IConstructPoint)pp4).ConstructAngleDistance(pp3,dA,dDX);

							PolygonClass pos=new PolygonClass();
							pCol=(IPointCollection)pos;
							Missing=Type.Missing;
							pCol.AddPoint(pp1,ref Missing,ref Missing);
							pCol.AddPoint(pp2,ref Missing,ref Missing);
							pCol.AddPoint(pp3,ref Missing,ref Missing);
							pCol.AddPoint(pp4,ref Missing,ref Missing);
							((IElement)jpgEle).Geometry=pos;
							pageCon.AddElement(jpgEle,0);

							//ͼ����:
							dA=CommonClassDLL.syc_CalAngle(ref pp1,ref pp2);
							IPoint pp=new PointClass();
							pp.X=(pp2.X+pp3.X)*0.5;
							pp.Y=(pp2.Y+pp3.Y)*0.5;
							IPoint pp5=new PointClass();
							((IConstructPoint)pp5).ConstructAngleDistance(pp,dA,10.0);

							dotNetFont=new System.Drawing.Font("����",1);
							textSymbol = new TextSymbolClass();
                            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
							dZH=3.5;	//mm
							textSymbol.Size=dZH/25.4*72.0;
							textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
							textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVACenter;
							textEle=new TextElementClass();
							textEle.Symbol = textSymbol;
							textEle.Text=sTLName;
							element=(IElement)textEle;
							element.Geometry=pp5;
							pageCon.AddElement(element,0);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
							IPoint newP=new PointClass();
							((IConstructPoint)newP).ConstructAngleDistance(pp1,dA,15.0);	//2ͼ�������
							FirstP.X=newP.X;
							FirstP.Y=newP.Y;
						}
					} //for(int iX=0;...
				}

				//...
				if(OtherSz.Count!=0) 
				{
					for(int iO=0;iO<OtherSz.Count;iO++) 
					{	
						//����·����TIF�ļ�:
						string sTIF=OtherSz[iO] as string;
						int nPos=sTIF.IndexOf(".");
						if(nPos!=-1) ;
						else continue;

						string sTLName=sTIF.Substring(0,nPos);
						string sTLFile="";

						//����ļ��Ƿ����:
						for(int k=0;k<pLegendFiles.Count;k++) 
						{
							string sFileName=pLegendFiles[k] as string;
							if(sFileName.Equals(sTIF)==true) 
							{
								sTLFile=Application.StartupPath+@"\ͼ��\"+sFileName;
								break;
							}
						}
						
						//Create Elements:
						if(sTLName.Length!=0 && sTLFile.Length!=0) 
						{
							TifPictureElementClass jpgEle=new TifPictureElementClass();					
							jpgEle.ImportPictureFromFile(sTLFile);
							jpgEle.MaintainAspectRatio=true;

							double dDX=22.0;
							double dDY=12.0;
							IPoint pp1=new PointClass();
							pp1.PutCoords(FirstP.X,FirstP.Y);

							dA=CommonClassDLL.syc_CalAngle(ref NKP4,ref NKP3);
							IPoint pp2=new PointClass();
							((IConstructPoint)pp2).ConstructAngleDistance(pp1,dA,dDX);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
							IPoint pp3=new PointClass();
							((IConstructPoint)pp3).ConstructAngleDistance(pp2,dA,dDY);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP4);
							IPoint pp4=new PointClass();
							((IConstructPoint)pp4).ConstructAngleDistance(pp3,dA,dDX);

							PolygonClass pos=new PolygonClass();
							pCol=(IPointCollection)pos;
							Missing=Type.Missing;
							pCol.AddPoint(pp1,ref Missing,ref Missing);
							pCol.AddPoint(pp2,ref Missing,ref Missing);
							pCol.AddPoint(pp3,ref Missing,ref Missing);
							pCol.AddPoint(pp4,ref Missing,ref Missing);
							((IElement)jpgEle).Geometry=pos;
							pageCon.AddElement(jpgEle,0);

							//ͼ����:
							dA=CommonClassDLL.syc_CalAngle(ref pp1,ref pp2);
							IPoint pp=new PointClass();
							pp.X=(pp2.X+pp3.X)*0.5;
							pp.Y=(pp2.Y+pp3.Y)*0.5;
							IPoint pp5=new PointClass();
							((IConstructPoint)pp5).ConstructAngleDistance(pp,dA,10.0);

							dotNetFont=new System.Drawing.Font("����",1);
							textSymbol = new TextSymbolClass();
                            textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
							dZH=3.5;	//mm
							textSymbol.Size=dZH/25.4*72.0;
							textSymbol.HorizontalAlignment=esriTextHorizontalAlignment.esriTHALeft;
							textSymbol.VerticalAlignment=esriTextVerticalAlignment.esriTVACenter;
							textEle=new TextElementClass();
							textEle.Symbol = textSymbol;
							textEle.Text=sTLName;
							element=(IElement)textEle;
							element.Geometry=pp5;
							pageCon.AddElement(element,0);

							dA=CommonClassDLL.syc_CalAngle(ref NKP3,ref NKP2);
							IPoint newP=new PointClass();
							((IConstructPoint)newP).ConstructAngleDistance(pp1,dA,15.0);	//2ͼ�������
							FirstP.X=newP.X;
							FirstP.Y=newP.Y;
						}
					} //for(int iX=0;...
				}
                #endregion 

				//... ...
            }

                

            #region �ܼ�
            //MM:
            if (true)
            {
                double dCTDW_ZG = 8.0;      
                string sCTDW = "**�����ع����";
                double dCTDW_GAP = 13.0;    

                double dMM_ZG = 4.5;  
                string sMM = "����: ";
                double dMM_GAP = 13.0; 

                sCTDW = textBox2.Text.Trim();
                sMM = textBox3.Text.Trim();
                try
                {
                    m_MapControl = MapControl;
                   
                    m_myTab = myTab;
                    m_myMapFrame = myMapFrame;
                    m_PageControl = PageControl;

                    IElement ele = m_myMapFrame as IElement;
                    IEnvelope env = ele.Geometry.Envelope;
                    LDP = new PointClass();
                    LDP.X = env.XMin;
                    LDP.Y = env.YMin;
                    RUP = new PointClass();
                    RUP.X = env.XMax;
                    RUP.Y = env.YMax;
                    pageCon = m_PageControl.GraphicsContainer;

                    PointClass NeedP = new PointClass();
                    NeedP.X = LDP.X - dCTDW_GAP;
                    NeedP.Y = LDP.Y;

                    System.Drawing.Font dotNetFont = new System.Drawing.Font("����", 1, FontStyle.Regular);
                    ITextSymbol textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                    textSymbol.Size = dCTDW_ZG / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter;
                    ((ICharacterOrientation)textSymbol).CJKCharactersRotation = true;
                    TextElementClass textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sCTDW;
                    IElement element = (IElement)textEle;
                    element.Geometry = NeedP;
                    pageCon.AddElement(element, 0);
                    trans = (ITransform2D)element;
                    trans.Rotate(NeedP, -Math.PI / 2);
                    IEnvelope aBound = new EnvelopeClass();
                    element.QueryBounds(m_PageControl.ActiveView.ScreenDisplay, aBound);

                    IPoint newRDP = new PointClass();
                    newRDP.X = aBound.XMax;
                    newRDP.Y = aBound.YMin;
                    trans.Move(NeedP.X - newRDP.X, NeedP.Y - newRDP.Y);

                    NeedP.X = RUP.X;
                    NeedP.Y = RUP.Y + dMM_GAP;

                    dotNetFont = new System.Drawing.Font("����", 1, FontStyle.Regular);
                    textSymbol = new TextSymbolClass();
                    textSymbol.Font = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(dotNetFont) as stdole.IFontDisp;
                    ((IFormattedTextSymbol)textSymbol).CharacterWidth = 100;
                    textSymbol.Size = dMM_ZG / 25.4 * 72.0;
                    textSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
                    textSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABaseline;
                    textEle = new TextElementClass();
                    textEle.Symbol = textSymbol;
                    textEle.Text = sMM;
                    element = (IElement)textEle;
                    element.Geometry = NeedP;
                    pageCon.AddElement(element, 0);

                    m_PageControl.Refresh();

                }
                catch (Exception E)
                {
                    MessageBox.Show("����:" + E.Message);
                    return;
                }
            }
            #endregion 

            IGraphicsContainerSelect pGCSelect = this.m_PageControl.PageLayout as IGraphicsContainerSelect;
            pGCSelect.UnselectAllElements();


       
			ICommand myTool=new ControlsMapPanToolClass();
            myTool.OnCreate(this.m_MapControl.Object);
			MapControl.CurrentTool=myTool as ITool;
			m_bufferPolygon=null;
			m_MyPolygon=null;

			PageControl.ZoomToWholePage();
			PageControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics,null,null);
			CommonClassDLL.Dispose();

            //ResetAnnoState(true );
        }

       
	}
}
