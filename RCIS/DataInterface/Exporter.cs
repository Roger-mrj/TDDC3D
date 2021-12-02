using System;
using ESRI.ArcGIS.Geometry;
namespace RCIS.DataExchange
{
	/// <summary>
	/// Exporter ��ժҪ˵����
	/// </summary>
	public interface Exporter
	{
        bool BeginExport();       
		void ExportToShapefile(String pSrcName,String pDestName,bool pSchemaOnly);
        void FinishExport();
        
	}
}
