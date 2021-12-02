using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCIS.Database;
using RCIS.GISCommon;
using RCIS.Utility;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace TDDC3D.sys
{
    public partial class YsdmSetValueForm : Form
    {
        public YsdmSetValueForm()
        {
            InitializeComponent();
        }
        public IWorkspace currWs = null;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void YsdmSetValueForm_Load(object sender, EventArgs e)
        {
            DataTable dt = LS_SetupMDBHelper.GetDataTable("select * from SYS_YSDM  where type in ('POINT','LINE','POLYGON') And CLASSNAME <> 'DLTBGXGC' And CLASSNAME <> 'CJDCQGXGC' And CLASSNAME <> 'XZQGXGC'", "ysdm");
            this.gridControl1.DataSource = dt;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //执行
            if (currWs == null)
                return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DataTable dtysdm = (DataTable)this.gridControl1.DataSource;
                IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
                IWorkspace2 pWS2 = this.currWs as IWorkspace2;
                foreach (DataRow arow in dtysdm.Rows)
                {
                    string className = arow["CLASSNAME"].ToString().Trim();
                    string ysdm = arow["YSDM"].ToString().Trim();
                    if (pWS2.get_NameExists(esriDatasetType.esriDTFeatureClass, className))
                    {
                        string sql = " update " + className + " set  YSDM ='" + ysdm + "' ";
                        this.currWs.ExecuteSQL(sql);
                    }
                }
                this.Cursor = Cursors.Default;
                MessageBox.Show("执行完毕!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
