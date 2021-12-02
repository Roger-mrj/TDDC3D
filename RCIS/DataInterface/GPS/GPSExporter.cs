using System;
using System.Collections.Generic;
using System.Text;
using System.IO ;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using RCIS.DataExchange;

using RCIS.Utility;
using RCIS.GISCommon;

namespace RCIS.DataExchange.GPS
{
    public class GPSExporter : Exporter
    {
        private String mGPSFile;
        private String mShapefile;
        private bool mHasPointNo = false;
        private bool mHasZ = false;
        
        private string mSplitChar = ",";
        public GPSExporter()
        {
        }
        public String GPSFile
        {
            get
            {
                return this.mGPSFile;
            }
            set
            {
                this.mGPSFile = value;
            }
        }
        public String Shapefile
        {
            get
            {
                return this.mShapefile;
            }
            set
            {
                this.mShapefile = value;
            }
        }
        public bool HasPointNO
        {
            get
            {
                return this.mHasPointNo;
            }
            set
            {
                this.mHasPointNo = value;
            }
        }
        public bool HasZ
        {
            get
            {
                return this.mHasZ;
            }
            set
            {
                this.mHasZ = value;
            }
        }
        
        public string SplitChar
        {
            get
            {
                return this.mSplitChar;
            }
            set
            {
                if (value == null) value = ",";
                value = value.Trim();
                if (value.Length > 1) value = value.Substring(0, 1);
                if (value.Equals("")) value = ",";
                this.mSplitChar = value;
            }
        }
#region Exporter 成员
        private StreamReader aReader;
        public bool BeginExport()
        {
            if (!File.Exists(this.GPSFile))
            {
                return false;
            }
            if (File.Exists(this.Shapefile))
            {
                return false;
            }
            try
            {
                aReader = new StreamReader(this.GPSFile, Encoding.GetEncoding("GB2312"));
            }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// 对于GPS文件 
        /// </summary>
        /// <param name="pSrcName">无效</param>
        /// <param name="pDestName">无效</param>
        /// <param name="pSchemaOnly">无效</param>
        public void ExportToShapefile(string pSrcName
            , string pDestName, bool pSchemaOnly)
        {


            string aFolder = System.IO.Path.GetDirectoryName(this.Shapefile);// FileHelper.GetFileFolder(this.Shapefile);
            string aTitle = System.IO.Path.GetFileNameWithoutExtension(this.Shapefile);// FileHelper.GetFileTitle(this.Shapefile);
            
            ShapefileWorkspaceFactoryClass aFac=new ShapefileWorkspaceFactoryClass ();
            IWorkspace aWK = aFac.OpenFromFile(aFolder, 0);
            FieldsClass aFlds = new FieldsClass();
            IField oid = EsriDatabaseHelper.CreateOIDField();
            IField ptNo = EsriDatabaseHelper.CreateTextField("PTNO", "点号", 16);
            IField geom = EsriDatabaseHelper.CreateGeometryField(esriGeometryType.esriGeometryPoint
                , new UnknownCoordinateSystemClass(),this.HasZ);
            (aFlds as IFieldsEdit ).AddField (oid);
            (aFlds as IFieldsEdit).AddField (ptNo);
            (aFlds as IFieldsEdit ).AddField (geom);
            IFeatureClass aClass = (aWK as IFeatureWorkspace).CreateFeatureClass
            (aTitle, aFlds, null, null, esriFeatureType.esriFTSimple, "SHAPE", null);
            
            IWorkspaceEdit aWKEdit = aWK as IWorkspaceEdit;
            int order = 0;
           
           
            int idx=aClass .Fields .FindField ("PTNO");
            try
            {
                aWKEdit.StartEditing(false);
                aWKEdit.StartEditOperation();
                string aLine = this.ReadLine();
                while (aLine != null)
                {
                    GPSPoint aPt = this.ReadPoint(aLine);
                    try
                    {
                        if (aPt != null
                            && aPt.Shape != null
                            && !aPt.Shape.IsEmpty)
                        {

                            IFeature aObj = aClass.CreateFeature();
                            
                            aObj.Shape =(ESRI.ArcGIS.Geometry.IGeometry) aPt.Shape;
                            aObj.set_Value(idx, aPt.PtNO);
                            aObj.Store();
                        }
                    }
                    catch  { }
                   
                    if (order++ > 5000)
                    {
                        aWKEdit.StopEditOperation();
                        aWKEdit.StopEditing(true);
                        aWKEdit.StartEditing(false);
                        aWKEdit.StartEditOperation();
                        order = 0;
                                 
                    }
                    aLine = this.ReadLine();
                }
                aWKEdit.StopEditOperation();
                aWKEdit.StopEditing(true);
                

            }
            catch {
                if (aWKEdit != null&&aWKEdit .IsBeingEdited ())
                    aWKEdit.StopEditing(false);
            }
        }

        public void FinishExport()
        {
            if (aReader != null)
                aReader.Close();
        }
        private GPSPoint ReadPoint(string aLine)
        {
            
            if (aLine != null)
            {
                String[] aTextAry = aLine.Split(this.mSplitChar.ToCharArray());
                int iStart = 0;
                int iCoords = 2;
                if (this.HasPointNO) { iStart = 1; iCoords++; }
                //不判断是否有Z 如果没有用0代表Z.
                //if (this.HasZ) { iCoords++; }

                if (aTextAry.Length < iCoords) return null;
                double px = double.NaN;
                double py = double.NaN;
                double pz = 0;


                px = TextHelper.ParseDouble(aTextAry[iStart], px);
                py = TextHelper.ParseDouble(aTextAry[iStart + 1], py);
                if (this.HasZ && aTextAry.Length > iStart + 2)
                    pz = TextHelper.ParseDouble(aTextAry[iStart + 2], pz);

                
                
                if(!double.IsNaN (px)
                    &&!double.IsNaN (py))
                {
                    GPSPoint rPt = new GPSPoint();
                    IPoint aGeom = new PointClass();
                    aGeom.PutCoords(px, py);

                    if (this.HasZ)
                    {
                        IZAware zAware = aGeom as IZAware;
                        zAware.ZAware = true;
                        aGeom.Z = pz;
                    }
                    rPt.Shape = aGeom;
                    if (this.HasPointNO)
                    {
                        rPt.PtNO = TextHelper.ParseInt(aTextAry[0], 0);
                    }
                    return rPt;
                }
            }
            return null;
        }
        private String ReadLine()
        {
            string aLine = aReader.ReadLine();
            while (aLine != null && aLine.Trim().Equals(""))
            {
                aLine = aReader.ReadLine();
            }
            return aLine;
        }
        #endregion
        private class GPSPoint
        {
            public IPoint Shape;
            public int PtNO = 0;
        }
    }
}
