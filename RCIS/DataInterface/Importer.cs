using System;

namespace RCIS.DataExchange
{
	/// <summary>
	/// Importer ��ժҪ˵����
	/// </summary>
	public interface Importer
	{
        bool BeginImport();
		void ImportFromShapefile(String pSrcName,String pDestName,bool pSchemaOnly);
	    void FinishImport();
    }

}
