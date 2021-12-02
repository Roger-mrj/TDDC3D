using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using DevExpress.XtraEditors;

namespace TDDC3D.sys
{
    public partial class WorkspaceSelectForm : Form
    {
        public WorkspaceSelectForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 把CheckedListBoxControl设置为单选框
        /// </summary>
        /// <param name="chkControl">CheckedListBoxControl</param>
        /// <param name="index">index当前选中的索引</param>
        public void SingleSelectCheckedListBoxControls(CheckedListBoxControl chkControl, int index)
        {
            if (chkControl.CheckedItems.Count > 0)
            {
                for (int i = 0; i < chkControl.Items.Count; i++)
                {
                    if (i != index)
                    {
                        chkControl.SetItemCheckState(i, System.Windows.Forms.CheckState.Unchecked);
                    }
                }
            }
        }
        public List<IWorkspace> sendWses = null;

        public IWorkspace retWs = null;

        private void WorkspaceSelectForm_Load(object sender, EventArgs e)
        {
            foreach (IWorkspace aWs in sendWses)
            {
                string pathName = aWs.PathName;
                int pos=pathName.LastIndexOf('\\');
                string title=pathName.Substring(pos+1,pathName.Length-pos-1);
                this.chklstWorkspaces.Items.Add(title + "|" + pathName);
            }
        }

        private void chklstWorkspaces_ItemChecking(object sender, DevExpress.XtraEditors.Controls.ItemCheckingEventArgs e)
        {
            SingleSelectCheckedListBoxControls(chklstWorkspaces, e.Index);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (this.chklstWorkspaces.CheckedItemsCount==1)
            {
                string s = this.chklstWorkspaces.CheckedItems[0].ToString();
                s = RCIS.Utility.OtherHelper.GetRightName(s);

                foreach (IWorkspace aWs in sendWses)
                {
                    string pathName = aWs.PathName;
                    if (pathName.ToUpper().Equals(s.ToUpper()))
                    {
                        retWs=aWs;
                        break;
                    }
                }
            }
            
        }
    }
}
