using System;
using ESRI.ArcGIS.Geometry;
namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// LabObject 的摘要说明。
	/// </summary>
	public class LabObject
	{
        private int m_labId; 
        private IPoint m_labGeometry;
		public LabObject(int pLabID,IPoint pGeometry)
		{
			this.m_labId =pLabID;
            this.m_labGeometry =pGeometry;
		}
        public int LabID
        {
            get
            {
                return this.m_labId;
            }
        }
        public IPoint Geometry
        {
            get
            {
                return this.m_labGeometry ;
            }
        }
	}
}
