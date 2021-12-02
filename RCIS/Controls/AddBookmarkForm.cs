using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace RCIS.Controls
{
    public partial class AddBookmarkForm : Form
    {
        public AddBookmarkForm()
        {
            InitializeComponent();
        }
        public IMap currMap = null;

        public string newBookMarkName = "";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.txtBookmark.Text.Trim() == "")
            {
                MessageBox.Show("请先输入书签名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.newBookMarkName = this.txtBookmark.Text.Trim();
            IMapBookmarks mapBookmarks = currMap as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            while (spatialBookmark != null)
            {
                if ( spatialBookmark.Name.Trim()==this.newBookMarkName)
                {
                    MessageBox.Show("当前书签名称已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                spatialBookmark = enumSpatialBookmark.Next();
            }


            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void AddBookmarkForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
