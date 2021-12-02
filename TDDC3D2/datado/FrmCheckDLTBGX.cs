using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.datado
{
    public partial class FrmCheckDLTBGX : Form
    {
        public IWorkspace _curWS;

        public FrmCheckDLTBGX()
        {
            InitializeComponent();
        }
    }
}
