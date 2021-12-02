using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RCIS.DataInterface.VCT2
{
    public class TableStructBeginEnd
    {
        public int nZDGS;             //该表字段个数
        public ArrayList aZDMCs;   //字段名称
        public ArrayList aZDLXs;    //字段类型: double,char,date
        public ArrayList aZDJD;     //字段的精度. double[10.3中的10],char[20],date[];
        public ArrayList aZDJD2;     //对double[10.3]来讲，为3。其他“”
    }
}
