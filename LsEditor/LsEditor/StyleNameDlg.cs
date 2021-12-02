using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LsEditor
{
    public partial class StyleNameDlg : Form
    {
        public StyleNameDlg()
        {
            InitializeComponent();
        }

        private string sName;
        public string StyleName
        {
            set
            {
                this.sName = value;
                this.txtName.Text = sName;
            }
            get
            {
                return this.txtName.Text.Trim();
            }
        }


        private string sCategory;
        public string StyleCategory
        {
            set
            {
                this.sCategory = value;
                this.txtCategory.Text = sCategory;

            }
            get
            {
                return this.txtCategory.Text.Trim();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}