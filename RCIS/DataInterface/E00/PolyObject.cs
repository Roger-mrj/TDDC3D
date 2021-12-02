using System;
using System.Collections ;
namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// PolyObject 的摘要说明。
	/// </summary>
	public class PolyObject
	{
        private ArrayList m_arcList;
		public PolyObject()
		{
			this.m_arcList =new   ArrayList ();
		}
        public void AddArc(int arcID)
        {
            this.m_arcList .Add (arcID);
        }
        public int ArcCount
        {
            get
            {
                return this.m_arcList .Count ;
            }
        }
        public int GetArcID(int pArcIndex)
        {
            return (int)this.m_arcList[pArcIndex];
        }
	}
}
