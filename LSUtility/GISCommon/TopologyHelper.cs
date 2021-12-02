using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
namespace RCIS.GISCommon
{
    public class TopologyHelper
    {
        /// <summary>
        /// 添加拓扑规则
        /// </summary>
        /// <param name="topology"></param>
        /// <param name="sourceClass"></param>
        /// <param name="destClass"></param>
        /// <param name="ruletype"></param>
        /// <param name="title"></param>
        public static void AddTopRule(ITopology topology, IFeatureClass sourceClass, IFeatureClass destClass,
            esriTopologyRuleType ruletype, string title)
        {

            ITopologyRuleContainer topologyRuleContainer = (ITopologyRuleContainer)topology;

            ITopologyRule topologyRule = new TopologyRuleClass();
            topologyRule.OriginClassID = sourceClass.ObjectClassID;
            if (destClass != null)
            {
                topologyRule.DestinationClassID = destClass.ObjectClassID;
                topologyRule.AllDestinationSubtypes = true;
            }
            topologyRule.TopologyRuleType = ruletype;
            topologyRule.AllOriginSubtypes = true;
            topologyRule.Name = title;
            if (topologyRuleContainer.get_CanAddRule(topologyRule))
            {
                topologyRuleContainer.AddRule(topologyRule);
            }

        }


        /// <summary>
        /// 创建拓扑
        /// </summary>
        /// <param name="featureDataset"></param>
        /// <param name="topologyName"></param>
        /// <returns></returns>
        public static ITopology CreateTopology(IFeatureDataset featureDataset, string topologyName)
        {
            //IWorkspace pWs = featureDataset.Workspace;
            ITopologyContainer topologyContainer = (ITopologyContainer)featureDataset;           

            ITopology topology1 = topologyContainer.CreateTopology(topologyName, topologyContainer.DefaultClusterTolerance, -1, "");
            
            return topology1;
        }

        public static void ValidateTopology(ITopology topology)
        {
            try
            {
                ESRI.ArcGIS.Geometry.ISegmentCollection location = (ESRI.ArcGIS.Geometry.ISegmentCollection)new ESRI.ArcGIS.Geometry.PolygonClass();
                IGeoDataset geoDataset = (IGeoDataset)topology;
                // Set the rectangle of the pLocation polygon to be equal to the Topology extent       
                location.SetRectangle(geoDataset.Extent);
                ESRI.ArcGIS.Geometry.IPolygon locationPolygon = (ESRI.ArcGIS.Geometry.IPolygon)location;

                // Get the Dirty Area that covers the entire topology.       
                IPolygon polygon = topology.get_DirtyArea(locationPolygon);
                // Define the Area to validate and validate the topology 
                IEnvelope areatoValidate = polygon.Envelope;
                IEnvelope areaValidated = topology.ValidateTopology(areatoValidate);
            }
            catch { }
        }


        //打开拓扑
        public static ITopology OpenTopologyName(string pTopName, IDataset mDataset)
        {
            try
            {
                ITopologyContainer aTopCon = mDataset as ITopologyContainer;
                ITopology aTop = aTopCon.get_TopologyByName(pTopName);
                return aTop;
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        /// <summary>
        /// 删除所有拓扑
        /// </summary>
        /// <param name="pDataset"></param>
        public static void DeleteAllTop(IDataset pDataset)
        {
            ITopology topology = null;
            ITopologyContainer topologyContainer = (ITopologyContainer)pDataset;
            //删除所有拓扑
            for (int i = topologyContainer.TopologyCount - 1; i >= 0; i--)
            {
                topology = topologyContainer.get_Topology(i);
                IDataset ds = topology as IDataset;
                ds.Delete();
            }
        }

    }
}
