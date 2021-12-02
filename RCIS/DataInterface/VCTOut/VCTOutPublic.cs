using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Geometry;
using RCIS.GISCommon;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataSourcesFile;

using System.Xml.Serialization;
namespace RCIS.DataInterface.VCTOut
{
    public class VCTOutPublic
    {

        //public static bool ExportShp(
        //    IFeatureClass fc, IQueryFilter queryFilter, string shapefileName)
        //{
        //    if (fc==null) return true;
        //    if (fc.FeatureCount(null)==0) return true;
        //    try
        //    {
        //        string folderPath = System.IO.Path.GetDirectoryName(shapefileName);
        //        string shortName = System.IO.Path.GetFileNameWithoutExtension(shapefileName);
        //        IWorkspaceFactory targetWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
        //        IWorkspace targetWorkspace = targetWorkspaceFactory.OpenFromFile(folderPath, 0);

        //        IWorkspace sourceWs = (fc as IDataset).Workspace;
        //        RCIS.GISCommon.EsriDatabaseHelper.ConvertFeatureClass(sourceWs, targetWorkspace, (fc as IDataset).Name, shortName, queryFilter);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        //public static bool SaveXml(List<TableStruct> allTableStruct)
        //{
        //    string tmpFile = System.Windows.Forms.Application.StartupPath + "\\tmp\\allstruct.xml";
        //    Stream fStream = new System.IO.FileStream(tmpFile, FileMode.Create);
        //    XmlSerializer xmlFormat = new XmlSerializer(typeof(List<TableStruct>));
        //    xmlFormat.Serialize(fStream, allTableStruct);//序列化对象
        //    fStream.Close();
        //    return true;
        //}



        public static void WriteALine(StreamWriter writer, long gid,IPolyline aEdgeLine)
        {
            writer.WriteLine(gid);
            writer.WriteLine("1099000000");
            writer.WriteLine("unknown");
            writer.WriteLine(1);  //特征类型
            writer.WriteLine(1); //线段个数
            writer.WriteLine(11); //折线

            IPointCollection aPtCol = aEdgeLine as IPointCollection;
            int aPtCount = aPtCol.PointCount;
            writer.WriteLine(aPtCount);
            for (int pi = 0; pi < aPtCount; pi++)
            {
                IPoint aPt = aPtCol.get_Point(pi);                
                writer.WriteLine(aPt.X + "," + aPt.Y);
            }

            writer.WriteLine(0);  //输出一个 0 ，结束
        }

        //获取属性表的属性字段列表
        public static  ArrayList getAttrFields(IFeatureWorkspace _pWorkspace, string className)
        {

            ArrayList ar = new ArrayList();
            IFeatureClass pFc = null;
            try
            {
                pFc = _pWorkspace.OpenFeatureClass(className);
            }
            catch { }
            if (pFc != null)
            {
                for (int i = 0; i < pFc.Fields.FieldCount; i++)
                {
                    IField field = pFc.Fields.get_Field(i);
                    string fieldName = field.Name.ToUpper();
                    if (field.Type == esriFieldType.esriFieldTypeGlobalID ||
                        field.Type == esriFieldType.esriFieldTypeOID ||
                        field.Type == esriFieldType.esriFieldTypeGeometry ||
                        field.Type == esriFieldType.esriFieldTypeGUID
                        )
                        continue;

                    if ((fieldName == "OBJECTID") || (fieldName == "FID") || (fieldName.StartsWith("SHAPE")))
                    {
                        continue;
                    }

                  
                    ar.Add(fieldName);
                }
            }
            return ar;

        }

        public static void ExportLineIDs(List<long> pLineList, StreamWriter mWriter2)
        {
            int iCount = pLineList.Count;
            for (int i = 0; i < iCount; )
            {
                StringBuilder aBuilder = new StringBuilder();
                //每行 8 个
                for (int j = 0; j < 8 && i < iCount; i++, j++)
                {
                    long aID = pLineList[i];
                    aBuilder.Append(aID);
                    if (j + 1 < 8
                        && i + 1 < iCount)
                    {
                        aBuilder.Append(",");
                    }
                }
                string aText = aBuilder.ToString();
                mWriter2.WriteLine(aText);


            }
        }


       

    }
    ///// <summary>
    ///// 记录一个对象的父亲ID
    ///// </summary>
    //public class FeatureCache
    //{
    //    //public string TableName = "";
    //    //public int ObjectID = -1;
    //    public long GlobalID = -1;
    //    public List<int> Parents = new List<int>();
    //    public IGeometry ShapeCopy;
    //}

    /// <summary>
    /// 记录一个引用线对象
    /// </summary>
    public class LineObject
    {
        public IPolyline Shape;
        public long GID;
        public LineObject() { }
        public LineObject(long _gid, IPolyline _shp)
        {
            this.GID = _gid;
            this.Shape = _shp;
        }

        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            LineObject secObj = (LineObject)obj;
            if (this.GID == secObj.GID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [Serializable]
    public class TableStruct
    {
        /// <summary>
        /// 要素名称
        /// </summary>
        public string feaName = "";
        /// <summary>
        /// 要素代码
        /// </summary>
        public string feaCode = "";
        /// <summary>
        /// 表名
        /// </summary>
        public string className = "";
        public string type = "";
        public TableStruct(string _feaname, string _feacode, string _feaclass, string _featype)
        {
            this.feaName = _feaname;
            this.feaCode = _feacode;
            this.className = _feaclass;
            this.type = _featype;
        }
        public TableStruct()
        {
        }

    }


    public class Point3Object
    {
        public int index=0; //得到线段在列表中的索引
        public int fromOrTo = 1;
        public IPoint prePt = null;
        public IPoint currPt = null;
        public IPoint nextPt = null;
    }
}
