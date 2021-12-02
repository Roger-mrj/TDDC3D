using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCIS.DataInterface.VCTOut
{
    public class AObjIndex
    {
        public string bsm = "";
        public double minX, minY, maxX, maxY;

        public string ClassName = "";

        /// <summary>
        /// 图形位置
        /// </summary>
        public long txPos = 0;
        /// <summary>
        /// 属性位置
        /// </summary>
        public long sxPos = 0;

        public List<AIndexPoint> lstAllPts = new List<AIndexPoint>();
        public void calEnvelop()
        {
            minX = 999999999; minY = 999999999; maxX = 0; maxY = 0;
            foreach (AIndexPoint aPt in lstAllPts)
            {
                if (minX > aPt.x)
                {
                    minX = aPt.x;
                }
                if (minY > aPt.y)
                {
                    minY = aPt.y;
                }
                if (maxX < aPt.x)
                {
                    maxX = aPt.x;
                }
                if (maxY < aPt.y)
                {
                    maxY = aPt.y;
                }
            }

        }


        
    }
    public class AIndexPoint
    {
        public double x = 9;
        public double y = 0;
        public AIndexPoint(double _x, double _y)
        {
            this.x = _x;
            this.y = _y;
        }
    }
}
