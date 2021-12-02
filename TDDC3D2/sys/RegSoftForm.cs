using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Utility;
namespace TDDC3D.sys
{
    public partial class RegSoftForm : Form
    {
        public RegSoftForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.memosn.Text.Trim() == "")
                return;
            
            this.DialogResult = DialogResult.OK;
        }

        private void RegSoftForm_Load(object sender, EventArgs e)
        {
            RCIS.Utility.Computer puter=new RCIS.Utility.Computer();

            this.memoinfo.Text =DESEncrypt.Encrypt(puter.CpuID + puter.MacAddress,"guojie");
        }
    }
}
