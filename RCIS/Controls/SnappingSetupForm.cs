using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

using RCIS.GISCommon;

namespace RCIS.Controls
{
    public partial class SnappingSetupForm : Form
    {
        public SnappingSetupForm()
        {
            InitializeComponent();
        }

        public AxMapControl mapCtrol = null;
        public IEngineEditor globalEngineEditor = null;
        IEngineSnapEnvironment snapEnvironment = null;

        private void SnappingSetupForm_Load(object sender, EventArgs e)
        {
            if (mapCtrol == null)
                return;
            snapEnvironment = globalEngineEditor as IEngineSnapEnvironment;
            //¼ÓÔØ²¶×½»·¾³          

            chkLayers.Items.Clear();
            for (int i = 0; i < mapCtrol.LayerCount; i++)
            {
                ILayer thisLyr = mapCtrol.get_Layer(i);
                if (thisLyr is IFeatureLayer)
                {
                    IFeatureClass featureClass = (thisLyr as IFeatureLayer).FeatureClass;
                    IDataset pDS = featureClass as IDataset;
                    if (pDS.Workspace == globalEngineEditor.EditWorkspace)
                    {
                        string layerName = LayerHelper.GetClassShortName(featureClass as IDataset);

                        bool bHave = IsAddedAgent(layerName.Trim());
                        chkLayers.Items.Add(layerName, bHave);
                    }
                }
                else if (thisLyr is IGroupLayer)
                {
                    ICompositeLayer pCL = thisLyr as ICompositeLayer;
                    for (int kk = 0; kk < pCL.Count; kk++)
                    {
                        ILayer childLyr = pCL.get_Layer(kk);
                        if (childLyr is IFeatureLayer)
                        {
                            IFeatureClass featureClass = (childLyr as IFeatureLayer).FeatureClass;
                            IDataset pDS = featureClass as IDataset;
                            if (pDS.Workspace == globalEngineEditor.EditWorkspace)
                            {
                                string layerName = LayerHelper.GetClassShortName(featureClass as IDataset);
                                bool bHave = IsAddedAgent(layerName.Trim());
                                chkLayers.Items.Add(layerName, bHave);
                            }
                        }
                    }
                }
                
                
                


            }
            

        }

        private bool IsAddedAgent(string sLayerName)
        {

            for (int i = 0; i < snapEnvironment.SnapAgentCount; i++)
            {
                IEngineFeatureSnapAgent featureSnapAgent = snapEnvironment.get_SnapAgent(i) as IEngineFeatureSnapAgent;
                string snapClassName = LayerHelper.GetClassShortName(featureSnapAgent.FeatureClass as IDataset);
                if (sLayerName.ToUpper().Equals(snapClassName.Trim().ToUpper()))
                {
                    return true;
                }
            }
            return false ;
        }

        private void chkLayers_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {

            if (e.State == CheckState.Checked)
            {
                IEngineFeatureSnapAgent featureSnapAgent = new EngineFeatureSnap();

                IFeatureWorkspace pWS = this.globalEngineEditor.EditWorkspace as IFeatureWorkspace;
                string className = chkLayers.SelectedItem.ToString();
                IFeatureClass pFeatClass = pWS.OpenFeatureClass(className);

                featureSnapAgent.FeatureClass = pFeatClass;
                featureSnapAgent.HitType = esriGeometryHitPartType.esriGeometryPartVertex | esriGeometryHitPartType.esriGeometryPartEndpoint ;
                snapEnvironment.AddSnapAgent(featureSnapAgent);
                               

                ((IEngineEditProperties2)globalEngineEditor).SnapTips = true;

              

            }
            else if (e.State == CheckState.Unchecked)
            {
                int idx = -1;

                for (int i = 0; i < snapEnvironment.SnapAgentCount; i++)
                {
                    IEngineFeatureSnapAgent featureSnapAgent = snapEnvironment.get_SnapAgent(i) as IEngineFeatureSnapAgent;
                    string className = chkLayers.SelectedItem.ToString();
                    string snapClassName = LayerHelper.GetClassShortName(featureSnapAgent.FeatureClass as IDataset);

                    if (className.ToUpper().Equals(snapClassName.ToUpper()))
                    {
                        idx = i;
                        break;
                    }
                }
                if (idx > -1)
                {
                    snapEnvironment.RemoveSnapAgent(idx);
                }
            }
        }
    }
}