using System;

namespace RCIS.DataExchange
{
	/// <summary>
	/// Importer 的摘要说明。
	/// </summary>
	public interface Importer
	{
        bool BeginImport();
		void ImportFromShapefile(String pSrcName,String pDestName,bool pSchemaOnly);
	    void FinishImport();
    }

}
