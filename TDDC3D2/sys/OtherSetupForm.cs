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
    public partial class OtherSetupForm : Form
    {
        public OtherSetupForm()
        {
            InitializeComponent();
        }

        INIHelper ini = new INIHelper(RCIS.Global.AppParameters.ConfPath + "\\Setup.ini");

        private void OtherSetupForm_Load(object sender, EventArgs e)
        {
            string sb=ini.IniReadValue("system","filter");
            bool bb=false;
            bool.TryParse(sb,out bb);
            this.chkFilter.Checked = bb;

            sb = ini.IniReadValue("system", "gxhistory");
            bb = false;
            bool.TryParse(sb, out bb);
            this.chkBGHistory.Checked = bb;

            string stickMoveTolerance = ini.IniReadValue("system", "stickmovetolerance");
            int iTol = 10;
            int.TryParse(stickMoveTolerance, out iTol);
            this.txtMoveTolerance.Text = iTol.ToString();


        }

        private void OtherSetupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ini.IniWriteValue("system", "filter", this.chkFilter.Checked.ToString());
            ini.IniWriteValue("system", "gxhistory", this.chkBGHistory.Checked.ToString());

            int iTol = 0;
            int.TryParse(this.txtMoveTolerance.Text, out iTol);
            ini.IniWriteValue("system", "stickmovetolerance", iTol.ToString()); //粘滞容差

            RCIS.Global.AppParameters.GX_HISTORY = this.chkBGHistory.Checked;
            RCIS.Global.AppParameters.EDIT_STICKMOVETOLERANCE = iTol;
            RCIS.Global.AppParameters.DISPLAY_FILTER = this.chkFilter.Checked;

        }

        private void chkBGHistory_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
