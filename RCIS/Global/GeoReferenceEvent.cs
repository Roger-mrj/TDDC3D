using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCIS.Global
{
    public delegate void GeoReference_EventHandler(); //配准
    public delegate void GeoReference_ClearPoint_EventHandler(); //清除配准点 reset

    public delegate void GeoReference_Reset_EventHandler(); //

    public delegate void GeoReference_Register_EventHandler(); //保存
}
