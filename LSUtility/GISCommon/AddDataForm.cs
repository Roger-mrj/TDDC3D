using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;


namespace RCIS.GISCommon
{
    public partial class AddDataForm : Form
    {
        public AddDataForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            TreeNode node = this.trwFileExplorer.SelectedNode;
            if (node == null)
                return;
            if (!(node.Tag is ILayer))
            {
                MessageBox.Show("请首先选中某一图层");
                return;
            }
            resultLyr = node.Tag as ILayer;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        GISFileExplor fe = new GISFileExplor();
        public ILayer resultLyr = null;

        private void AddDataForm_Load(object sender, EventArgs e)
        {
            fe.CreateTree(this.trwFileExplorer);
        }

        private void trwFileExplorer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                TreeNode node = fe.EnumerateDirectory(e.Node);
            }
        }
    }
}
