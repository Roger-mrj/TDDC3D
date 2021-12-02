using System;
using ESRI.ArcGIS.Geometry;
namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// ArcObject 的摘要说明。
	/// </summary>
	public class ArcObject
	{
		private int m_arcID;
        private IPolyline m_arcGeometry;
        private int m_fromNode;
        private int m_toNode;
        public ArcObject(int pArcID,int pFromNode,int pToNode,IPolyline pGeometry)
        {
            this.m_arcID =pArcID;
            this.m_fromNode =pFromNode;
            this.m_toNode =pToNode;
            this.m_arcGeometry =pGeometry;
        }
        public int ArcID
        {
            get
            {
                return this.m_arcID;
            }
        }
        public int FromNode
        {
            get
            {
                return this.m_fromNode ;
            }            
        }
        public int ToNode
        {
            get
            {
                return this.m_toNode ;
            }
        }
        public IPolyline Geometry
        {
            get
            {
                return this.m_arcGeometry;
            }            
        }
	}
}
