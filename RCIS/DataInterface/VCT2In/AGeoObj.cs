using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
namespace RCIS.DataInterface.VCT2
{
    public class AGeoObj
    {
        public Int32 bsm = 0;
        public IGeometry geo = null;
    }


    public  class aLineGeoPos
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        public long startPos;
        public long offset;

        public aLineGeoPos(long _start, long _end)
        {
            this.startPos = _start;
            this.offset = _end;
            
        }
    }
}
