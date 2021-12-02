using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace RCIS.Controls
{
    public partial class BookMarkManageFrm : Form
    {
        public BookMarkManageFrm()
        {
            InitializeComponent();
        }

        public IActiveView currActiveView = null;
        private IMap currMap = null;

        private void BookMarkManageFrm_Load(object sender, EventArgs e)
        {
            currMap = this.currActiveView.FocusMap;

            this.lstAllBookmarks.Items.Clear();
            IMapBookmarks mapBookmarks = currMap as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            while (spatialBookmark != null)
            {
                this.lstAllBookmarks.Items.Add(spatialBookmark.Name.Trim());

                spatialBookmark = enumSpatialBookmark.Next();
            }


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (this.lstAllBookmarks.SelectedIndex == -1)
                return;
            string bookname = this.lstAllBookmarks.SelectedItem.ToString().Trim();

            IMapBookmarks mapBookmarks = currMap as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            while (spatialBookmark != null)
            {
                if (spatialBookmark.Name.ToUpper() == bookname.ToUpper())
                {
                    mapBookmarks.RemoveBookmark(spatialBookmark);
                    this.lstAllBookmarks.Items.RemoveAt(this.lstAllBookmarks.SelectedIndex);

                    return;
                }
                spatialBookmark = enumSpatialBookmark.Next();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.lstAllBookmarks.SelectedIndex == -1)
                return;
            string bookname= this.lstAllBookmarks.SelectedItem.ToString().Trim();

            IMapBookmarks mapBookmarks = currMap as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = mapBookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialBookmark = enumSpatialBookmark.Next();
            while (spatialBookmark != null)
            {
                if (spatialBookmark.Name.ToUpper()==bookname.ToUpper())
                {
                    spatialBookmark.ZoomTo(currMap);
                    currActiveView.Refresh();
                    return;
                }

                spatialBookmark = enumSpatialBookmark.Next();
            }
        }
    }
}
