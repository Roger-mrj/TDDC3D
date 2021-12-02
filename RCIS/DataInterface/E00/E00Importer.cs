using System;
using System.IO ;
using System.Text ;
using System.Collections ;
using RCIS.DataExchange ;
using ESRI.ArcGIS.DataSourcesFile;

using ESRI.ArcGIS.Geodatabase;

using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// E00Importer 的摘要说明。
	/// </summary>
	public class E00Importer:Importer
	{
		public E00Importer()
		{
			
        }
        private String m_shapefilePath;

        private String m_e00filePath;
        private StreamWriter m_writer;
        private IWorkspace   m_workspace;
        private IFeatureClass m_shapeClass;
        #region Importer 成员
        public bool BeginImport()
        {
            if(!File.Exists (this.m_shapefilePath ))return false;
            try
            {
                String shapeFolder = Path.GetDirectoryName(this.m_shapefilePath);// FileHelper.GetFileFolder(this.m_shapefilePath);
                String className = Path.GetFileNameWithoutExtension(this.m_shapefilePath);// FileHelper.GetFileTitle(this.m_shapefilePath);
                ShapefileWorkspaceFactoryClass fac=new ShapefileWorkspaceFactoryClass ();
                this.m_workspace =fac.OpenFromFile (shapeFolder,0);
                this.m_shapeClass =(this.m_workspace as IFeatureWorkspace ).OpenFeatureClass (className);
                FileStream fs=new FileStream(this.m_e00filePath ,FileMode.Create );
                Encoding gb2312=Encoding.Default ;
                try
                {
                    gb2312=Encoding.GetEncoding ("GB2312");
                }
                catch(Exception ex){}
                this.m_writer =new StreamWriter (fs,gb2312);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public void ImportFromShapefile(string pSrcName, string pDestName, bool pSchemaOnly)
        {
            
        }
        
        public void FinishImport()
        {
            
        }
        #endregion
    }
}
