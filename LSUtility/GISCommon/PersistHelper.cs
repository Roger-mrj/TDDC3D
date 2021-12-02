using System;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

namespace RCIS.GISCommon
{
	/// <summary>
	/// PersistStreamCode 的摘要说明。
	/// </summary>
	public class PersistHelper
	{
		private static Hashtable psTable;
        
        static PersistHelper()
        {
            psTable=new Hashtable ();
            #region 注册所有的符号
            Encode(new ArrowMarkerSymbolClass ());
            Encode(new BarChartSymbolClass());
            Encode(new CartographicLineSymbolClass());
            Encode(new CartographicLineSymbolClass());
            //Encode(new CharacterMarker3DSymbolClass());
            Encode(new CharacterMarkerSymbolClass ());
            Encode(new ColorRampSymbolClass());
            Encode(new ColorSymbolClass());
            Encode(new DotDensityFillSymbolClass());
            Encode(new GradientFillSymbolClass());
            Encode(new HashLineSymbolClass());
            Encode(new LineFillSymbolClass());
            //Encode(new Marker3DSymbolClass());
            Encode(new MarkerFillSymbolClass());
            Encode(new MarkerLineSymbolClass());
            Encode(new MultiLayerFillSymbolClass());
            Encode(new MultiLayerLineSymbolClass());
            Encode(new MultiLayerMarkerSymbolClass());
            Encode(new PictureFillSymbolClass());
            Encode(new PictureLineSymbolClass());
            Encode(new PictureMarkerSymbolClass());
            Encode(new PieChartSymbolClass());
            //Encode(new RasterRGBSymbolClass());
            Encode(new SimpleFillSymbolClass());
            //Encode(new SimpleLine3DSymbolClass());
            Encode(new SimpleLineSymbolClass());
            //Encode(new SimpleMarker3DSymbolClass());
            Encode(new SimpleMarkerSymbolClass());
            Encode(new StackedChartSymbolClass());
            Encode(new TextSymbolClass());
            //Encode(new TextureFillSymbolClass());
            //Encode(new TextureLineSymbolClass());
            #endregion
        }
        public static void Encode(IPersist ps)
        {
            if(ps is IClone)
            {
                Guid aGuid;
                ps.GetClassID (out aGuid);
                String clsid=aGuid.ToString ("B");
                psTable[clsid]=ps;                
            }
        }
        public static IPersist Decode(String pCLSID)
        {
            if(pCLSID==null)return null;
            if(psTable.ContainsKey (pCLSID))
            {
                IClone aClone=psTable[pCLSID] as IClone;
                IPersist aResult=aClone.Clone () as IPersist;
                return aResult;
            }
            return null;
        }
        public static String GetCLSID(IPersist ps)
        {
            if(ps==null)return "";
            Guid aGuid;
            ps.GetClassID (out aGuid);
            String clsid=aGuid.ToString ("B");
            return clsid;
        }
        public static byte[] Serialization(IPersist ps)
        {
            byte[] aContent = new byte[0];
            try
            {
                XMLStreamClass xmlStream = new XMLStreamClass();
                (ps as IPersistStream).Save(xmlStream, 0);
                aContent= xmlStream.SaveToBytes();
                
                //string aTmpFile = Path.Combine(FileHelper.TempFolder, Path.GetRandomFileName());
                //MemoryBlobStreamClass aStream = new MemoryBlobStreamClass();
                //(ps as IPersistStream).Save(aStream as IStream, 0);
                
                //aStream.SaveToFile(aTmpFile);
                
                //WinIO.FileStream fStream = File.OpenRead(aTmpFile);
                //int bufSize = (int)fStream.Length;
                //aContent = new byte[bufSize];
                //fStream.Read(aContent, 0, bufSize);
                //fStream.Close();
                //File.Delete(aTmpFile);
            }
            catch (Exception ex) { }
            return aContent;
        }
        public static bool Deserialization(IPersist ps, byte[] pContent)
        {
            if (pContent == null || pContent.Length == 0)
                return false;
            else
            {
                try
                {
                    XMLStreamClass xmlStream = new XMLStreamClass();
                    xmlStream .LoadFromBytes (ref pContent );
                    (ps as IPersistStream).Load(xmlStream);

                    //string aTmpFile = Path.Combine(FileHelper.TempFolder, Path.GetRandomFileName());
                    //WinIO.FileStream fs = File.Create(aTmpFile);
                    //fs.Write(pContent, 0, pContent.Length);
                    //fs.Close();

                    //MemoryBlobStreamClass aStream = new MemoryBlobStreamClass();
                    //aStream.ImportFromMemory(ref pContent[0], pContent.Length);
                    //aStream.LoadFromFile(aTmpFile);
                    //(ps as IPersistStream).Load(aStream as IStream);
                    //File.Delete(aTmpFile);
                }
                catch (Exception ex) { }
                return true;
            }
        }
    }
}
