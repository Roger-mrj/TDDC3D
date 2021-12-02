using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

using ESRI.ArcGIS.Display;


namespace RCIS.Controls
{
    public partial class PropertyEditorForm : Form
    {
        public PropertyEditorForm()
        {
            InitializeComponent();
        }

        //private Panel EditorPanel;
        private IEngineEditor m_engineEditor = null;

        public IEngineEditor EditorObject
        {
            get { return m_engineEditor; }
            set { m_engineEditor = value; }
        }


        private VGridPropertiesControl m_gridPropEditor;
        private ContextMenu mCtxMenu;
        private void CreateContextMenu()
        {
            this.mCtxMenu = new ContextMenu();
            MenuItem miDelete = new MenuItem("É¾³ýÒªËØ");
            this.mCtxMenu.MenuItems.Add(miDelete);

            miDelete.Click += new EventHandler(OnDeleteFeature);
        }

        void OnDeleteFeature(object sender, EventArgs e)
        {
            TreeNode aSelNode = this.objectTree.SelectedNode;
            if (aSelNode == null) return;

            IFeature aObj = aSelNode.Tag as IFeature;
            if (aObj == null) return;
            try
            {
                this.EditorPanel.Controls.Clear();
                aObj.Delete();
            }
            catch  { }

            aSelNode.Remove();
        }

      

        public void RefreshGrid()
        {

            IMap layerMap = this.EditorObject.Map;
            int layerCount = layerMap.LayerCount;
            for (int li = 0; li < layerCount; li++)
            {
                ILayer curLayer = layerMap.get_Layer(li);
                if (!curLayer.Visible) continue;
                if (curLayer is IGroupLayer)
                {
                    ICompositeLayer comositeLayer = curLayer as ICompositeLayer;
                    for (int kk = 0; kk < comositeLayer.Count; kk++)
                    {
                        ILayer childLyr = comositeLayer.get_Layer(kk);
                        if (!childLyr.Visible) continue;
                        if (childLyr is IFeatureLayer)
                        {
                            this.CreateLayerNode(childLyr as IGeoFeatureLayer);
                        }
                    }
                }
                else if (curLayer is IFeatureLayer)
                {
                    this.CreateLayerNode(curLayer as IGeoFeatureLayer);
                }


                

            }
            try
            {
                if (this.objectTree.Nodes.Count > 0)
                {
                    this.objectTree.SelectedNode
                        = this.objectTree.Nodes[0].Nodes[0];
                }
            }
            catch 
            {
            }
            this.objectTree.ExpandAll();
            
        }
        private void CreateLayerNode(IGeoFeatureLayer pLayer)
        {
            IFeatureSelection aFeatureSel = pLayer as IFeatureSelection;
            int selCount = aFeatureSel.SelectionSet.Count;
            if (selCount > 0)
            {
                TreeNode layerNode = new TreeNode(pLayer.Name + "(" + selCount + "¸öÒªËØ)");
                layerNode.Tag = pLayer;
                IEnumIDs aIDEnum = aFeatureSel.SelectionSet.IDs;
                int oid = aIDEnum.Next();
                while (oid > -1)
                {
                    try
                    {
                        IFeature aFea = pLayer.FeatureClass.GetFeature(oid);

                        int dispIndex = aFea.Fields.FindField(pLayer.DisplayField);
                        object dispObj = aFea.get_Value(dispIndex);
                        if (dispObj == null) dispObj = "";
                        TreeNode objNode = new TreeNode(dispObj.ToString());
                        objNode.Tag = aFea;  //djFea;
                        layerNode.Nodes.Add(objNode);
                        
                    }
                    catch { }
                    oid = aIDEnum.Next();
                }
                this.objectTree.Nodes.Add(layerNode);
            }
        }
        private IGeoFeatureLayer SelectedLayer
        {
            get
            {
                TreeNode selNode = this.objectTree.SelectedNode;
                if (selNode != null)
                {
                    return selNode.Tag as IGeoFeatureLayer;
                }
                return null;
            }
        }
  
        private IFeature  SelectedFeature
        {
            get
            {
                TreeNode selNode = this.objectTree.SelectedNode;
                if (selNode != null)
                {
                    return selNode.Tag as IFeature ;
                }
                return null;
            }
        }
        public bool LastFeatureChanged
        {
            get
            {
                if (this.EditorPanel.Controls.Count <= 0) return false;
                PropertyEditor pe = this.EditorPanel.Controls[0] as PropertyEditor;
                if (pe != null) return pe.PropertyChanged;
                return false;
            }
            set
            {
                if (this.EditorPanel.Controls.Count <= 0) return;
                PropertyEditor pe = this.EditorPanel.Controls[0] as PropertyEditor;
                if (pe != null) pe.PropertyChanged = value;

            }
        }
     
        public void OnApply()
        {
            if (this.EditorPanel.Controls.Count <= 0) return;
            PropertyEditor pe = this.EditorPanel.Controls[0] as PropertyEditor;
            if (pe != null) pe.OnApply();
        }
        public void OnCancel()
        {
            if (this.EditorPanel.Controls.Count <= 0) return;
            PropertyEditor pe = this.EditorPanel.Controls[0] as PropertyEditor;
            if (pe != null) pe.OnCancel();
        }

        private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IFeature  aFea = this.SelectedFeature;
            if (aFea != null)
            {
                this.OnApply();
                this.EditorPanel.Controls.Clear();

               this.m_gridPropEditor.DJFeature = aFea;
               this.EditorPanel.Controls.Add(this.m_gridPropEditor);

            }
            else
            {
                this.OnApply();
                this.EditorPanel.Controls.Clear();
            }


        }

       

       

        private void objectTree_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point aPt = new Point();
                aPt.X = e.X;
                aPt.Y = e.Y;
                this.mCtxMenu.Show(this.objectTree, aPt);
            }
        }

        private void PropertyEditorForm_Load(object sender, EventArgs e)
        {
            this.m_gridPropEditor = new VGridPropertiesControl();
            this.m_gridPropEditor.Visible = true;
            this.m_gridPropEditor.Dock = DockStyle.Fill;

            CreateContextMenu();

            this.RefreshGrid();
        }

        private void PropertyEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            this.OnApply();
        }


    }
}