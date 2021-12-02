using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RCIS.Controls
{
	/// <summary>
	/// AnnotateScaleForm ��ժҪ˵����
	/// </summary>
	public class AnnotateScaleForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbSameToLayer;
        private System.Windows.Forms.CheckBox cbUseScale;
        private System.Windows.Forms.GroupBox gbScale;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit teMaxScale;
        private DevExpress.XtraEditors.TextEdit teMinScale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
		/// <summary>
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AnnotateScaleForm()
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
            this.cbSameToLayer = new System.Windows.Forms.CheckBox();
            this.cbUseScale = new System.Windows.Forms.CheckBox();
            this.gbScale = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.teMaxScale = new DevExpress.XtraEditors.TextEdit();
            this.teMinScale = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.gbScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teMaxScale.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMinScale.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSameToLayer
            // 
            this.cbSameToLayer.Checked = true;
            this.cbSameToLayer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSameToLayer.Location = new System.Drawing.Point(32, 24);
            this.cbSameToLayer.Name = "cbSameToLayer";
            this.cbSameToLayer.Size = new System.Drawing.Size(224, 24);
            this.cbSameToLayer.TabIndex = 0;
            this.cbSameToLayer.Text = "ʹ��ͼ����ʾ������";
            this.cbSameToLayer.CheckedChanged += new System.EventHandler(this.cbSameToLayer_CheckedChanged);
            // 
            // cbUseScale
            // 
            this.cbUseScale.Location = new System.Drawing.Point(32, 56);
            this.cbUseScale.Name = "cbUseScale";
            this.cbUseScale.Size = new System.Drawing.Size(224, 24);
            this.cbUseScale.TabIndex = 1;
            this.cbUseScale.Text = "�����б����߷�Χ����ʾ";
            this.cbUseScale.CheckedChanged += new System.EventHandler(this.cbUseScale_CheckedChanged);
            // 
            // gbScale
            // 
            this.gbScale.Controls.Add(this.label4);
            this.gbScale.Controls.Add(this.label3);
            this.gbScale.Controls.Add(this.teMaxScale);
            this.gbScale.Controls.Add(this.teMinScale);
            this.gbScale.Controls.Add(this.label2);
            this.gbScale.Controls.Add(this.label1);
            this.gbScale.Enabled = false;
            this.gbScale.Location = new System.Drawing.Point(32, 80);
            this.gbScale.Name = "gbScale";
            this.gbScale.Size = new System.Drawing.Size(328, 96);
            this.gbScale.TabIndex = 2;
            this.gbScale.TabStop = false;
            this.gbScale.Text = "��д������";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(224, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "(��������)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(224, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "(��С������)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // teMaxScale
            // 
            this.teMaxScale.EditValue = "0";
            this.teMaxScale.Location = new System.Drawing.Point(48, 49);
            this.teMaxScale.Name = "teMaxScale";
            this.teMaxScale.Size = new System.Drawing.Size(176, 21);
            this.teMaxScale.TabIndex = 3;
            // 
            // teMinScale
            // 
            this.teMinScale.EditValue = "0";
            this.teMinScale.Location = new System.Drawing.Point(48, 17);
            this.teMinScale.Name = "teMinScale";
            this.teMinScale.Size = new System.Drawing.Size(176, 21);
            this.teMinScale.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "<=1:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = ">=1:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(184, 192);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "ȷ��";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(272, 192);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "ȡ��";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AnnotateScaleForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(370, 224);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbScale);
            this.Controls.Add(this.cbUseScale);
            this.Controls.Add(this.cbSameToLayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnnotateScaleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "��ע��ʾ������";
            this.Load += new System.EventHandler(this.AnnotateScaleForm_Load);
            this.gbScale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.teMaxScale.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teMinScale.Properties)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion
        public bool ScaleSameToLayer
        {
            get
            {
                return this.cbSameToLayer .Checked ;
            }
            set
            {
                this.cbSameToLayer .Checked =value;
                this.cbUseScale .Checked =!value;
            }
        }
        private double m_maxScale=0;
        private double m_minScale=0;
        public double MaxScale
        {
            get
            {
              return this.m_maxScale ;
            }
            set
            {
                this.m_maxScale =value;
            }
        }
        public double MinScale
        {
            get
            {
                return this.m_minScale ;
            }
            set
            {
                this.m_minScale =value;
            }
        }
        private void cbSameToLayer_CheckedChanged(object sender, System.EventArgs e)
        {
            if(this.cbSameToLayer.Checked)
            {
                this.cbUseScale .Checked =false;
            }
        }

        private void cbUseScale_CheckedChanged(object sender, System.EventArgs e)
        {
            if(this.cbUseScale .Checked )
            {
                this.cbSameToLayer .Checked =false;
                this.gbScale .Enabled =true;
            }
            else
            {
                this.gbScale .Enabled =false;
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if(!this.cbSameToLayer .Checked )
            {
                bool maxOK=Double.TryParse (this.teMaxScale .Text .Trim (),System.Globalization .NumberStyles .Any 
                    ,new System .Globalization .NumberFormatInfo (),out this.m_maxScale);
                if(!maxOK)
                {
                    MessageBox.Show ("��ʾ��ע����������û���趨���߸�ʽ����!","��������");
                    this.teMaxScale .Focus ();
                    return;
                }
                bool minOK=Double.TryParse (this.teMinScale .Text .Trim (),System.Globalization .NumberStyles.Any 
                    ,new System .Globalization .NumberFormatInfo (),out this.m_minScale );
                if(!minOK)
                {
                    MessageBox.Show ("��ʾ��ע����С������û���趨���߸�ʽ����!","��С������");
                    this.teMinScale .Focus ();
                    return;
                }
                if(this.m_minScale<this.m_maxScale)
                {
                    MessageBox.Show ("ע�⣡�������ߵķ�ĸӦ��С����С�����ߵķ�ĸ.","�����߷�ĸ��д");
                    return;
                }
            }
            this.DialogResult =DialogResult.OK ;
            this.Close ();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult =DialogResult.Cancel ;
            this.Close ();
        }

        private void AnnotateScaleForm_Load(object sender, System.EventArgs e)
        {
            this.teMaxScale .Text =this.m_maxScale .ToString ();
            this.teMinScale .Text =this.m_minScale .ToString ();
        }
	}
}
