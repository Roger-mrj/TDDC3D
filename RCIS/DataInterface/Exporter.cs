using System;
using ESRI.ArcGIS.Geometry;
namespace RCIS.DataExchange
{
	/// <summary>
	/// Exporter 的摘要说明。
	/// </summary>
	public interface Exporter
	{
        bool BeginExport();       
		void ExportToShapefile(String pSrcName,String pDestName,bool pSchemaOnly);
        void FinishExport();
        
	}
}
