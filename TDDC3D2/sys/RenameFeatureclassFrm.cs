using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDDC3D.sys
{
    public partial class RenameFeatureclassFrm : Form
    {
        public RenameFeatureclassFrm()
        {
            InitializeComponent();
        }

        public string RetClassName
        {
            set { this.txtClassName.Text = value; }
            get { return this.txtClassName.Text.Trim(); }
        }

        public string RetAliasName
        {
            set { this.txtAlias.Text = value; }
            get { return this.txtAlias.Text.Trim(); }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
