using System;
using System.Windows.Forms;

namespace TDDC3D.datado
{
    public partial class TFHInfoForm : Form
    {
        public TFHInfoForm()
        {
            InitializeComponent();
        }

        private void TFHInfoForm_Load(object sender, EventArgs e)
        {
            m_tfType.SelectedIndex = 2;
        }
        public int TFType
        {
            get { return m_tfType.SelectedIndex; }
        }

        public double TFLeft
        {
            set { this.m_left.Text = value.ToString(); }
            get { return Convert.ToDouble(m_left.Text); }
        }

        public double TFTop
        {
            set { this.m_top.Text = value.ToString(); }
            get { return Convert.ToDouble(m_top.Text); }
        }

        public double TFBottom
        {
            set { this.m_bottom.Text = value.ToString(); }
            get { return Convert.ToDouble(m_bottom.Text); }
        }

        public double TFRight
        {
            set { this.m_right.Text = value.ToString(); }
            get { return Convert.ToDouble(m_right.Text); }
        }

        private void m_left_TextChanged(object sender, EventArgs e)
        {

        }

        public bool IsInterpolate
        {
            get
            {
                return this.chkInterpo.Checked;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

       

       
    }
}