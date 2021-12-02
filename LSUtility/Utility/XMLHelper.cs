using System;
using System.Xml ;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

using RCIS.GISCommon;

namespace RCIS.Utility
{
	/// <summary>
	/// XMLHelper 的摘要说明。
	/// </summary>
	public class XMLHelper
	{
        #region 获取属性值
        public static String GetAttribute(XmlNode pNode
            ,String pAttrName
            ,String pDefResult)
        {
           String result=pDefResult;
            try
            {
                XmlAttribute xmlAttr=pNode.Attributes [pAttrName];
                if(xmlAttr!=null)
                {
                    result=xmlAttr.Value ;
                }
            }
            catch(Exception ex)
            {
                result=pDefResult;
            }
            return result;
        }
        public static double GetAttribute(XmlNode pNode,String pAttrName,double pDefResult)
        {
            double result=pDefResult;
            String resultStr=GetAttribute(pNode,pAttrName,pDefResult.ToString ());
            if(!double.TryParse (resultStr,System.Globalization .NumberStyles .Any
                ,new System.Globalization .NumberFormatInfo (),out result))
            {
                result=pDefResult;
            }
            return result;
        }
        public static int GetAttribute(XmlNode pNode,String pAttrName,int pDefResult)
        {
            int result=pDefResult;
            String resultStr=GetAttribute(pNode,pAttrName,pDefResult.ToString ());
            try
            {
                result=Convert.ToInt32 (resultStr);
            }
            catch(Exception ex)
            {
                result=pDefResult;
            }
            return result;
        }
        public static bool GetAttribute(XmlNode pNode,String pAttrName,bool pDefResult)
        {
            bool result=pDefResult;
            String resultStr=GetAttribute(pNode,pAttrName,pDefResult.ToString ());
            try
            {
                result=Convert.ToBoolean (resultStr);
            }
            catch(Exception ex)
            {
                result=pDefResult;
            }
            return result;
        }
        #endregion
        public static XmlAttribute CreateAttribute(XmlDocument pDoc,String pAttrName,String pAttrValue)
        {
            if(pAttrValue==null)pAttrValue="";
            XmlAttribute aAttr=pDoc.CreateAttribute (pAttrName);
            aAttr.Value =pAttrValue;
            return aAttr;            
        }   
        public static XmlAttribute CreateAttribute(XmlDocument pDoc,XmlNode pOwnerNode,String pAttrName,String pAttrValue)
        {            
            if(pAttrValue==null)pAttrValue="";
            XmlAttribute aAttr=pDoc.CreateAttribute (pAttrName);
            aAttr.Value =pAttrValue;            
            pOwnerNode.Attributes .Append (aAttr);
            return aAttr;            
        }   
        #region 将对象转化为XML字符
        public static String EncodeObject(IPersistStream pObject)
        {
            try
            {
                if(pObject==null)return "";
                else
                {
                    XMLStreamClass encoder = new XMLStreamClass();                   
                    pObject.Save (encoder as IStream,1);
                    String objStr = encoder.SaveToString();
                    return objStr;
                }
            }
            catch(Exception ex)
            {
            }
            return "";
        }
        public static void DecodeObject(IPersistStream pPS,String pObjectStr)
        {
            try
            {
                if(pObjectStr==null
                    ||pObjectStr.Equals ("")
                    ||pPS==null)
                {
                    return ;
                }
                else
                {
                    XMLStreamClass decoder=new XMLStreamClass ();
                    decoder.LoadFromString (pObjectStr);
                    pPS.Load (decoder);
                }
            }
            catch(Exception ex)
            {
            }
        }
        #endregion    
        /// <summary>
        /// 获取SpatialReference
        /// </summary>
        /// <param name="srNode"></param>
        /// <returns></returns>
        public static ISpatialReference ParseSpatialReference(XmlNode srNode)
        {
            ISpatialReference prj = null;
            try
            {
                double xmin = 0, xmax = 1, ymin = 0, ymax = 1, zmin = 0, zmax = 1, mmin = 0, mmax = 1;
              
                if (srNode == null)
                {
                    prj = new UnknownCoordinateSystemClass();

                    ISpatialReferenceTolerance spTol = (ISpatialReferenceTolerance)prj;
                    spTol.XYTolerance = 0.0001;
                    spTol.MTolerance = 0.0001;
                    spTol.ZTolerance = 0.0001;

                    ISpatialReferenceResolution spRes = (ISpatialReferenceResolution)prj;
                    spRes.ConstructFromHorizon();
                    spRes.set_XYResolution(true, 0.0001);
                    spRes.set_ZResolution(true, 0.0001);



                    xmin =0.0;
                    ymin = 0.0;
                    xmax = 1999999.7049;
                    ymax = 1999999.7049;
                    prj.SetDomain(xmin, xmax, ymin, ymax);

                    return prj;
                }
                XmlNode refstrNode = srNode.SelectSingleNode("refstr");
                if (refstrNode != null)
                {
                    string refstr = refstrNode.InnerText.Trim();
                    if (refstr.Equals(""))
                    {
                        prj = new UnknownCoordinateSystemClass();

                    }
                    else
                    {
                        ProjectedCoordinateSystemClass prjSys = new ProjectedCoordinateSystemClass();
                        int bytesRead = 0;
                        prjSys.ImportFromESRISpatialReference(refstr, out bytesRead);
                        prj = prjSys;
                    }
                }
                else
                {
                    prj = new UnknownCoordinateSystemClass();
                }
               
              
              

            }
            catch (Exception ex)
            {

            }
            return prj;
        }
        public static XmlNode CreateSpatialReferenceNode(XmlDocument doc, ISpatialReference sr)
        {
            try
            {
                if (doc == null || sr == null)
                    return null;
                string refstr = GeometryHelper.CreateSpatialReferenceString(sr);

                XmlNode refNode = doc.CreateElement("spatialref");
                XmlNode refstrNode = doc.CreateElement("refstr");
                if (refstr != null)
                {
                    refstrNode.InnerText = refstr;
                }
                refNode.AppendChild(refstrNode);
                #region 创建Extent
                XmlNode extentNode = doc.CreateElement("extent");
                refNode.AppendChild(extentNode);
                double xmin, ymin, xmax, ymax, zmin, zmax, mmin, mmax;
                sr.GetDomain(out xmin, out xmax, out ymin, out ymax);
                sr.GetZDomain(out zmin, out zmax);
                sr.GetMDomain(out mmin, out mmax);


                XmlNode exNode = doc.CreateElement("ex");
                XMLHelper.CreateAttribute(doc, exNode, "min", xmin.ToString());
                XMLHelper.CreateAttribute(doc, exNode, "max", xmax.ToString());
                extentNode.AppendChild(exNode);
                XmlNode eyNode = doc.CreateElement("ey");
                XMLHelper.CreateAttribute(doc, eyNode, "min", ymin.ToString());
                XMLHelper.CreateAttribute(doc, eyNode, "max", ymax.ToString());
                extentNode.AppendChild(eyNode);

                XmlNode ezNode = doc.CreateElement("ez");
                XMLHelper.CreateAttribute(doc, ezNode, "min", zmin.ToString());
                XMLHelper.CreateAttribute(doc, ezNode, "max", zmax.ToString());
                extentNode.AppendChild(ezNode);

                XmlNode emNode = doc.CreateElement("em");
                XMLHelper.CreateAttribute(doc, emNode, "min", mmin.ToString());
                XMLHelper.CreateAttribute(doc, emNode, "max", mmax.ToString());
                extentNode.AppendChild(emNode);
                #endregion
                return refNode;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static double ParseDouble(string str, double def)
        {
            double ret = def;
            try
            {
                ret = Convert.ToDouble(str);
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        public static int ParseInt(string str, int def)
        {
            int ret = def;
            try
            {
                ret = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        public static bool ParseBool(string str, bool def)
        {
            bool ret = def;
            try
            {
                ret = Convert.ToBoolean(str);
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        public static object GetAttribute(XmlNode target, string attrName, Type enumType, object def)
        {
            object ret = def;
            try
            {
                XmlAttribute attr = target.Attributes[attrName];
                if (attr != null)
                {
                    string str = attr.Value.Trim();
                    if (Enum.IsDefined(enumType, str))
                    {
                        ret = Enum.Parse(enumType, str, true);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        public static object GetAttribute(XmlNode target, string attrName, esriFieldType fieldType, object def)
        {
            object ret = def;
            try
            {
                XmlAttribute attr = target.Attributes[attrName];
                if (attr != null)
                {
                    string str = attr.Value.Trim();
                    switch (fieldType)
                    {
                        case esriFieldType.esriFieldTypeSmallInteger:
                            {
                                ret = Convert.ToInt32(str);
                                break;
                            }
                        case esriFieldType.esriFieldTypeSingle:
                            {
                                ret = Convert.ToSingle(str);
                                break;
                            }
                        case esriFieldType.esriFieldTypeInteger:
                            {
                                ret = Convert.ToInt32(str);
                                break;
                            }
                        case esriFieldType.esriFieldTypeDouble:
                            {
                                ret = Convert.ToDouble(str);
                                break;
                            }
                        case esriFieldType.esriFieldTypeString:
                            {
                                ret = str;
                                break;
                            }
                        case esriFieldType.esriFieldTypeDate:
                            {
                                if (str == "sysdate")
                                {
                                    ret = str;
                                }
                                else
                                {
                                    ret = Convert.ToDateTime(str);
                                }
                                
                                break;
                            }
                        default:
                            {
                                ret = str;
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        public static IField ParseField(XmlNode fieldNode)
        {
            IField field = null;
            if (fieldNode == null)
                return null;
            if (!fieldNode.LocalName.Equals("field"))
            {
                return null;
            }
            
            try
            {

                string fieldName = XMLHelper.GetAttribute(fieldNode, "name", "");
                string aliasName = XMLHelper.GetAttribute(fieldNode, "aliasName", fieldName);
                esriFieldType fieldType = (esriFieldType)XMLHelper.GetAttribute(fieldNode, "fieldType", typeof(esriFieldType), esriFieldType.esriFieldTypeOID);
                if (fieldName.Equals("")
                    || fieldType == esriFieldType.esriFieldTypeOID
                    || fieldType == esriFieldType.esriFieldTypeGeometry)
                {
                    field = null;
                }
                else
                {
                    field = new FieldClass();
                    IFieldEdit editField = field as IFieldEdit;
                    editField.Name_2 = fieldName;
                    editField.AliasName_2 = aliasName;
                    editField.Type_2 = fieldType;
                    int defaultLength = 10;
                    int defalultPrecision = 16;
                    int defaultScale = 2;
                    switch (fieldType)
                    {
                        case esriFieldType.esriFieldTypeSmallInteger:
                            {
                                editField.Length_2 = XMLHelper.GetAttribute(fieldNode, "length", defaultLength);
                                break;
                            }
                        case esriFieldType.esriFieldTypeInteger:
                            {
                                editField.Length_2 = XMLHelper.GetAttribute(fieldNode, "length", defaultLength);
                                break;
                            }
                        case esriFieldType.esriFieldTypeSingle:
                            {
                                int precision = XMLHelper.GetAttribute(fieldNode, "length", defalultPrecision);
                                int scale = XMLHelper.GetAttribute(fieldNode, "scale", defaultScale);
                                if (precision > 38
                                    || precision <= 0
                                    || scale <= 0
                                    || scale >= precision)
                                {
                                    precision = 38;
                                    scale = 8;
                                }
                                editField.Precision_2 = precision;
                                editField.Scale_2 = scale;
                                break;
                            }
                        case esriFieldType.esriFieldTypeDouble:
                            {
                                int precision = XMLHelper.GetAttribute(fieldNode, "length", defalultPrecision);
                                int scale = XMLHelper.GetAttribute(fieldNode, "scale", defaultScale);
                                if (precision > 38
                                    || precision <= 0
                                    || scale <= 0
                                    || scale >= precision)
                                {
                                    precision = 38;
                                    scale = 8;
                                }
                                editField.Precision_2 = precision;
                                editField.Scale_2 = scale;
                                break;
                            }
                        case esriFieldType.esriFieldTypeString:
                            {
                                editField.Length_2 = XMLHelper.GetAttribute(fieldNode, "length", 10);
                                break;
                            }
                        case esriFieldType.esriFieldTypeDate:
                            {
                                //editField.Length_2 = 1000;
                                break;
                            }
                    }
                    editField.IsNullable_2 = XMLHelper.GetAttribute(fieldNode, "nullable", true);
                    editField.Required_2 = false;
                    editField.Editable_2 = true;
                    object defaultValue = XMLHelper.GetAttribute(fieldNode, "default", fieldType, null);
                    if (defaultValue != null)
                    {
                        editField.DefaultValue_2 = defaultValue;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return field;
        }
        public static XmlNode CreateFieldNode(XmlDocument doc, IFeatureClass feaClass, IField field)
        {
            XmlNode fieldNode = null;
            try
            {

                if (doc != null && field != null)
                {
                    if (esriFieldType.esriFieldTypeOID == field.Type
                        || esriFieldType.esriFieldTypeGeometry == field.Type)
                    {
                        fieldNode = null;
                    }
                    else if (feaClass != null && (feaClass.AreaField == field
                        || feaClass.LengthField == field))
                    {
                        fieldNode = null;
                    }
                    else
                    {
                        fieldNode = doc.CreateElement("field");
                        XMLHelper.CreateAttribute(doc, fieldNode, "name", field.Name);
                        XMLHelper.CreateAttribute(doc, fieldNode, "aliasName", field.AliasName);
                        XMLHelper.CreateAttribute(doc, fieldNode, "fieldType", field.Type.ToString());
                        XMLHelper.CreateAttribute(doc, fieldNode, "nullable", field.IsNullable.ToString());
                        #region 字段大小
                        switch (field.Type)
                        {
                            case esriFieldType.esriFieldTypeSmallInteger:
                                {
                                    XMLHelper.CreateAttribute(doc, fieldNode, "length", field.Length.ToString());
                                    break;
                                }
                            case esriFieldType.esriFieldTypeInteger:
                                {
                                    XMLHelper.CreateAttribute(doc, fieldNode, "length", field.Length.ToString());
                                    break;
                                }
                            case esriFieldType.esriFieldTypeSingle:
                                {
                                    XMLHelper.CreateAttribute(doc, fieldNode, "length", field.Length.ToString());
                                    XMLHelper.CreateAttribute(doc, fieldNode, "scale", field.Scale.ToString());
                                    break;
                                }
                            case esriFieldType.esriFieldTypeDouble:
                                {
                                    XMLHelper.CreateAttribute(doc, fieldNode, "length", field.Precision.ToString());
                                    XMLHelper.CreateAttribute(doc, fieldNode, "scale", field.Scale.ToString());
                                    break;
                                }
                            case esriFieldType.esriFieldTypeString:
                                {
                                    XMLHelper.CreateAttribute(doc, fieldNode, "length", field.Length.ToString());
                                    break;
                                }
                        }
                        #endregion
                        if (field.DefaultValue != null)
                        {
                            XMLHelper.CreateAttribute(doc, fieldNode, "default", field.DefaultValue.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fieldNode = null;
            }
            return fieldNode;
        }
        public static XmlNode CreateGeometryNode(XmlDocument doc, IGeometry geom)
        {
            XmlNode geomNode = doc.CreateElement("geometry");
            XMLHelper.CreateAttribute(doc, geomNode, "gt", geom.GeometryType.ToString());
            #region 给图形节点填充数据
            if (!geom.IsEmpty)
            {
                if (geom is IPoint)
                {//单点
                    XmlNode ptNode = XMLHelper.CreatePointNode(doc, geom as IPoint);
                    if (ptNode != null)
                    {
                        geomNode.AppendChild(ptNode);
                    }
                }
                else if (geom is IMultipoint)
                {//多点
                    IPointCollection ptCol = geom as IPointCollection;
                    int ptCount = ptCol.PointCount;
                    for (int ptIndex = 0; ptIndex < ptCount; ptIndex++)
                    {
                        IPoint curPt = ptCol.get_Point(ptIndex);
                        XmlNode ptNode = XMLHelper.CreatePointNode(doc, curPt);
                        if (ptNode != null)
                        {
                            geomNode.AppendChild(ptNode);
                        }
                    }
                }
                else if (geom is IPolyline)
                {
                    IGeometryCollection geomCol = geom as IGeometryCollection;
                    int geomCount = geomCol.GeometryCount;
                    for (int geomIndex = 0; geomIndex < geomCount; geomIndex++)
                    {
                        IPath curPath = geomCol.get_Geometry(geomIndex) as IPath;
                        XmlNode partNode = XMLHelper.CreatePartNode(doc, curPath);
                        if (partNode != null)
                            geomNode.AppendChild(partNode);

                    }
                }
                else if (geom is IPolygon)
                {
                    IGeometryCollection geomCol = geom as IGeometryCollection;
                    int geomCount = geomCol.GeometryCount;
                    for (int geomIndex = 0; geomIndex < geomCount; geomIndex++)
                    {
                        IRing curPath = geomCol.get_Geometry(geomIndex) as IRing;
                        XmlNode partNode = XMLHelper.CreatePartNode(doc, curPath);
                        if (partNode != null)
                            geomNode.AppendChild(partNode);

                    }
                }
            }
            #endregion
            return geomNode;
        }
        private static XmlNode CreatePointNode(XmlDocument doc, IPoint pt)
        {
            if (pt != null)
            {
                XmlNode ptNode = doc.CreateElement("point");

                IZAware zaware = pt as IZAware;
                IMAware maware = pt as IMAware;
                XMLHelper.CreateAttribute(doc, ptNode, "hasz", zaware.ZAware.ToString());
                XMLHelper.CreateAttribute(doc, ptNode, "hasm", maware.MAware.ToString());
                double px = 0, py = 0, pz = 0, pm = 0;
                if (!pt.IsEmpty)
                {
                    px = pt.X;
                    py = pt.Y;
                    if (zaware.ZAware)
                    {
                        pz = pt.Z;
                    }
                    if (maware.MAware)
                    {
                        pm = pt.M;
                    }
                }
                XMLHelper.CreateAttribute(doc, ptNode, "px", px.ToString());
                XMLHelper.CreateAttribute(doc, ptNode, "py", py.ToString());
                XMLHelper.CreateAttribute(doc, ptNode, "pz", pz.ToString());
                XMLHelper.CreateAttribute(doc, ptNode, "pm", pm.ToString());
                return ptNode;
            }
            else
            {
                return null;
            }
        }
        private static XmlNode CreateSegmentNode(XmlDocument doc, ISegment segment)
        {
            XmlNode segNode = doc.CreateElement("seg");
            XMLHelper.CreateAttribute(doc, segNode, "st", segment.GeometryType.ToString());
            if (segment is ILine)
            {//
                XmlNode fromNode = XMLHelper.CreatePointNode(doc, segment.FromPoint);
                XmlNode toNode = XMLHelper.CreatePointNode(doc, segment.ToPoint);
                if (fromNode != null && toNode != null)
                {
                    segNode.AppendChild(fromNode);
                    segNode.AppendChild(toNode);
                }
            }
            else if (segment is ICircularArc)
            {//圆弧
                ICircularArc arc = segment as ICircularArc;
                double fromAngle = arc.FromAngle;
                double toAngle = arc.ToAngle;
                IPoint anglePt = new PointClass();
                anglePt.PutCoords(fromAngle, toAngle);
                XmlNode angleNode = XMLHelper.CreatePointNode(doc, anglePt);
                XmlNode centerNode = XMLHelper.CreatePointNode(doc, arc.CenterPoint);
                if (angleNode != null && centerNode != null)
                {
                    segNode.AppendChild(angleNode);
                    segNode.AppendChild(centerNode);
                }
            }
            else
            {//其他的都处理为直线
                XmlNode fromNode = XMLHelper.CreatePointNode(doc, segment.FromPoint);
                XmlNode toNode = XMLHelper.CreatePointNode(doc, segment.ToPoint);
                if (fromNode != null && toNode != null)
                {
                    segNode.AppendChild(fromNode);
                    segNode.AppendChild(toNode);
                }
            }
            return segNode;
        }
        private static XmlNode CreatePartNode(XmlDocument doc, IGeometry part)
        {
            XmlNode partNode = doc.CreateElement("part");
            ISegmentCollection segCol = part as ISegmentCollection;
            int segCount = segCol.SegmentCount;
            for (int segIndex = 0; segIndex < segCount; segIndex++)
            {
                ISegment curSeg = segCol.get_Segment(segIndex);
                XmlNode segNode = XMLHelper.CreateSegmentNode(doc, curSeg);
                if (segNode != null)
                {
                    partNode.AppendChild(segNode);
                }
            }
            return partNode;
        }
        public static XmlNode CreateRowNode(XmlDocument doc, IRow row)
        {
            XmlNode rowNode = doc.CreateElement("row");
            int fldCount = row.Fields.FieldCount;
            for (int fldIndex = 0; fldIndex < fldCount; fldIndex++)
            {
                IField fld = row.Fields.get_Field(fldIndex);
                if (fld.Type == esriFieldType.esriFieldTypeOID
                    || fld.Type == esriFieldType.esriFieldTypeGeometry)
                {
                    continue;
                }
                else if (row is IFeature)
                {
                    IFeature fea = row as IFeature;
                    IFeatureClass feaClass = fea.Class as IFeatureClass;
                    if (fld == feaClass.AreaField
                        || fld == feaClass.LengthField)
                    {
                        continue;
                    }
                }
                object ov = row.get_Value(fldIndex);
                XmlNode colNode = doc.CreateElement("col");
                XMLHelper.CreateAttribute(doc, colNode, "dt", fld.Type.ToString());
                if (ov != null)
                {
                    colNode.InnerText = ov.ToString();
                }
                rowNode.AppendChild(colNode);
            }
            return rowNode;
        }
        public static XmlNode CreateFeatureNode(XmlDocument doc, IFeature fea)
        {
            XmlNode feaNode = doc.CreateElement("feature");

            IGeometry geom = fea.ShapeCopy;
            XmlNode geomNode = XMLHelper.CreateGeometryNode(doc, geom);
            XmlNode rowNode = XMLHelper.CreateRowNode(doc, fea as IRow);
            if (geomNode != null)
            {
                feaNode.AppendChild(geomNode);
            }
            if (rowNode != null)
            {
                feaNode.AppendChild(rowNode);
            }
            return feaNode;
        }

        //读取XML文件指定 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Section"></param>部分
        /// <param name="Key"></param>关键字
        /// <returns></returns>
        public static string XmlReadValue( XmlDocument doc,string Section, string Key)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            string ss = "";
            if (null != XmlCodeNode)
            {
                ss = XmlCodeNode.SelectSingleNode(Key).InnerText;
            }
            return ss;

        }

        public static string XmlReadValue(XmlDocument doc, string Section,
            string Section1, string Key)
        {
            try
            {
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                string ss = "";
                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            ss = xmlchildnode.SelectSingleNode(Key).InnerText;
                            if (ss == "") ss = "空";
                            return ss;
                        }
                    }
                }
                return ss;
            }
            catch { return ""; }

        }
        public static string XmlReadValue(XmlDocument doc, string Section,
         string Section1, string Section2, string Key)
        {
            try
            {
                string ss = "";
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            XmlNodeList childchildNodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < childchildNodeList.Count; j++)
                            {
                                XmlNode xmlchildchildnode = childchildNodeList[j];
                                if (xmlchildchildnode.LocalName.Equals(Section2))
                                {

                                    ss = xmlchildchildnode.SelectSingleNode(Key).InnerText;
                                    if (ss == "") ss = "空";
                                    return ss;
                                }
                            }
                        }
                    }

                }
                return ss;
            }
            catch (Exception ex) { return ""; }

        }
        public static string XmlReadValue(XmlDocument doc, string Section,
           string Section1, string Section2, string Section3, string Key)
        {
            try
            {
                string ss = "";
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < child2NodeList.Count; j++)
                            {
                                XmlNode xmlchild2node = child2NodeList[j];
                                if (xmlchild2node.LocalName.Equals(Section2))
                                {
                                    XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                    for (int k = 0; k < child3NodeList.Count; k++)
                                    {
                                        XmlNode xmlchild3node = child3NodeList[k];
                                        if (xmlchild3node.LocalName.Equals(Section3))
                                        {
                                            ss = xmlchild3node.SelectSingleNode(Key).InnerText;
                                            if (ss == "") ss = "空";
                                            return ss;
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
                return ss;
            }
            catch { return ""; }

        }

        public static string XmlReadValue(XmlDocument doc, string Section,
           string Section1, string Section2, string Section3,string Section4, string Key)
        {
            try
            {
                string ss = "";
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < child2NodeList.Count; j++)
                            {
                                XmlNode xmlchild2node = child2NodeList[j];
                                if (xmlchild2node.LocalName.Equals(Section2))
                                {
                                    XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                    for (int k = 0; k < child3NodeList.Count; k++)
                                    {
                                        XmlNode xmlchild3node = child3NodeList[k];
                                        if (xmlchild3node.LocalName.Equals(Section3))
                                        {
                                            XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                            for (int k4 = 0; k4 < child4NodeList.Count; k4++)
                                            {
                                                XmlNode xmlChild4Node = child4NodeList[k4];
                                                if (xmlChild4Node.LocalName.Equals(Section4))
                                                {
                                                    ss = xmlChild4Node.SelectSingleNode(Key).InnerText;
                                                    if (ss == "") ss = "空";
                                                    return ss;
                                                }

                                                
                                            }
                                            
                                        }
                                    }

                                }
                            }
                        }
                    }

                }
                return ss;
            }
            catch { return ""; }

        }


        public static string XmlReadValue(XmlDocument doc, string Section,
           string Section1, string Section2, string Section3, string Section4,string Section5, string Key)
        {
            try
            {
                string ss = "";
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < child2NodeList.Count; j++)
                            {
                                XmlNode xmlchild2node = child2NodeList[j];
                                if (xmlchild2node.LocalName.Equals(Section2))
                                {
                                    XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                    for (int k = 0; k < child3NodeList.Count; k++)
                                    {
                                        XmlNode xmlchild3node = child3NodeList[k];
                                        if (xmlchild3node.LocalName.Equals(Section3))
                                        {
                                            XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                            for (int k4 = 0; k4 < child4NodeList.Count; k4++)
                                            {
                                                XmlNode xmlChild4Node = child4NodeList[k4];
                                                if (xmlChild4Node.LocalName.Equals(Section4))
                                                {
                                                    XmlNodeList child5NodeList = xmlChild4Node.ChildNodes;
                                                    for (int k5=0;k5<child5NodeList.Count;k5++ )
                                                    {
                                                        XmlNode xmlChild5Node=child5NodeList[k5];
                                                        if (xmlChild5Node.LocalName.Equals(Section5))
                                                        {
                                                            ss = xmlChild5Node.SelectSingleNode(Key).InnerText;
                                                            if (ss == "") ss = "空";
                                                            return ss;
                                                        }
                                                    }

                                                }

                                                
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }

                }
                return ss;
            }
            catch { return ""; }

        }


        public static string XmlReadValue(XmlDocument doc, string Section,
           string Section1, string Section2, string Section3, string Section4, string Section5,string Section6, string Key)
        {
            try
            {
                string ss = "";
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < child2NodeList.Count; j++)
                            {
                                XmlNode xmlchild2node = child2NodeList[j];
                                if (xmlchild2node.LocalName.Equals(Section2))
                                {
                                    XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                    for (int k = 0; k < child3NodeList.Count; k++)
                                    {
                                        XmlNode xmlchild3node = child3NodeList[k];
                                        if (xmlchild3node.LocalName.Equals(Section3))
                                        {
                                            XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                            for (int k4 = 0; k4 < child4NodeList.Count; k4++)
                                            {
                                                XmlNode xmlChild4Node = child4NodeList[k4];
                                                if (xmlChild4Node.LocalName.Equals(Section4))
                                                {
                                                    XmlNodeList child5NodeList = xmlChild4Node.ChildNodes;
                                                    for (int k5 = 0; k5 < child5NodeList.Count; k5++)
                                                    {
                                                        XmlNode xmlChild5Node = child5NodeList[k5];
                                                        if (xmlChild5Node.LocalName.Equals(Section5))
                                                        {
                                                            XmlNodeList child6NodeList = xmlChild5Node.ChildNodes;
                                                            for (int k6 = 0; k6 < child6NodeList.Count; k6++)
                                                            {
                                                                XmlNode xmlChild6Node = child6NodeList[k6];
                                                                if (xmlChild6Node.LocalName.Equals(Section6))
                                                                {
                                                                    ss = xmlChild6Node.SelectSingleNode(Key).InnerText;
                                                                    if (ss == "") ss = "空";
                                                                    return ss;
                                                                }
                                                            }


                                                            
                                                        }
                                                    }

                                                }


                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }

                }
                return ss;
            }
            catch { return ""; }

        }

        public static string XmlReadValue(XmlDocument doc, string Section,
           string Section1, string Section2, string Section3, string Section4, string Section5, string Section6,string Section7, string Key)
        {
            try
            {
                string ss = "";
                XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

                if (null != XmlCodeNode)
                {
                    XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                    for (int i = 0; i < childNodeList.Count; i++)
                    {
                        XmlNode xmlchildnode = childNodeList[i];
                        if (xmlchildnode.LocalName.Equals(Section1))
                        {
                            XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                            for (int j = 0; j < child2NodeList.Count; j++)
                            {
                                XmlNode xmlchild2node = child2NodeList[j];
                                if (xmlchild2node.LocalName.Equals(Section2))
                                {
                                    XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                    for (int k = 0; k < child3NodeList.Count; k++)
                                    {
                                        XmlNode xmlchild3node = child3NodeList[k];
                                        if (xmlchild3node.LocalName.Equals(Section3))
                                        {
                                            XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                            for (int k4 = 0; k4 < child4NodeList.Count; k4++)
                                            {
                                                XmlNode xmlChild4Node = child4NodeList[k4];
                                                if (xmlChild4Node.LocalName.Equals(Section4))
                                                {
                                                    XmlNodeList child5NodeList = xmlChild4Node.ChildNodes;
                                                    for (int k5 = 0; k5 < child5NodeList.Count; k5++)
                                                    {
                                                        XmlNode xmlChild5Node = child5NodeList[k5];
                                                        if (xmlChild5Node.LocalName.Equals(Section5))
                                                        {
                                                            XmlNodeList child6NodeList = xmlChild5Node.ChildNodes;
                                                            for (int k6 = 0; k6 < child6NodeList.Count; k6++)
                                                            {
                                                                XmlNode xmlChild6Node = child6NodeList[k6];
                                                                if (xmlChild6Node.LocalName.Equals(Section6))
                                                                {
                                                                    XmlNodeList child7NodeList = xmlChild6Node.ChildNodes;
                                                                    for (int k7 = 0; k7 < child7NodeList.Count; k7++)
                                                                    {
                                                                        XmlNode xmlChild7Node = child7NodeList[k7];
                                                                        if (xmlChild7Node.LocalName.Equals(Section7))
                                                                        {
                                                                            ss = xmlChild7Node.SelectSingleNode(Key).InnerText;
                                                                            if (ss == "") ss = "空";
                                                                            return ss;
                                                                        }
                                                                    }


                                                                    
                                                                }
                                                            }



                                                        }
                                                    }

                                                }


                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }

                }
                return ss;
            }
            catch { return ""; }

        }
       


        
        //写XML文件 
        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
            string Section, string Key, string Value)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                if (Value == "")
                {
                    XmlCodeNode.SelectSingleNode(Key).InnerText = "无";
                }
                else
                {
                    XmlCodeNode.SelectSingleNode(Key).InnerText = Value;
                }
            }
            doc.Save(sXMLPath);
        }

        public static void XmlWriteValue(XmlDocument doc, string sXMLPath, 
            string Section,
            string sValue1,
            string sValue2,
            string Key, 
            string Value)
        {



            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        XmlNodeList childchildNodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < childchildNodeList.Count; j++)
                        {
                            XmlNode xmlchildchildnode = childchildNodeList[j];
                            if (xmlchildchildnode.LocalName.Equals(sValue2))
                            {
                                if (Value == "")
                                {
                                    xmlchildchildnode.SelectSingleNode(Key).InnerText = "无";
                                }
                                else
                                {
                                    xmlchildchildnode.SelectSingleNode(Key).InnerText = Value;
                                }
                            }
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }
        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
         string Section,
         string sValue1,
         string Key,
         string Value)
        {



            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        if (Value == "")
                        {
                            xmlchildnode.SelectSingleNode(Key).InnerText = "无";
                        }
                        else
                        {
                            xmlchildnode.SelectSingleNode(Key).InnerText = Value;
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }
        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
           string Section,
           string sValue1,
           string sValue2,
            string sValue3,
           string Key,
           string Value)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < child2NodeList.Count; j++)
                        {
                            XmlNode xmlchildchildnode = child2NodeList[j];
                            if (xmlchildchildnode.LocalName.Equals(sValue2))
                            {
                                XmlNodeList child3NodeList = xmlchildchildnode.ChildNodes;
                                for (int k = 0; k < child3NodeList.Count; k++)
                                {
                                    XmlNode xmlchildchildchildnode = child3NodeList[k];
                                    if (xmlchildchildchildnode.LocalName.Equals(sValue3))
                                    {
                                        if (Value == "")
                                        {
                                            xmlchildchildchildnode.SelectSingleNode(Key).InnerText = "无";
                                        }
                                        else
                                        {
                                            xmlchildchildchildnode.SelectSingleNode(Key).InnerText = Value;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }



        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
           string Section,
           string sValue1,
           string sValue2,
            string sValue3,
            string sValue4,
           string Key,
           string Value)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < child2NodeList.Count; j++)
                        {
                            XmlNode xmlchild2node = child2NodeList[j];
                            if (xmlchild2node.LocalName.Equals(sValue2))
                            {
                                XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                for (int k = 0; k < child3NodeList.Count; k++)
                                {
                                    XmlNode xmlchild3node = child3NodeList[k];
                                    if (xmlchild3node.LocalName.Equals(sValue3))
                                    {

                                        XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                        for (int kk = 0; kk < child4NodeList.Count; kk++)
                                        {
                                            XmlNode xml4childNode = child4NodeList[kk];
                                            if (xml4childNode.LocalName.Equals(sValue4))
                                            {
                                                if (Value == "")
                                                {
                                                    xml4childNode.SelectSingleNode(Key).InnerText = "无";
                                                }
                                                else
                                                {
                                                    xml4childNode.SelectSingleNode(Key).InnerText = Value;
                                                }
                                            }
                                        }


                                        
                                    }
                                }

                            }
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }


        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
           string Section,
           string sValue1,
           string sValue2,
            string sValue3,
            string sValue4,
            string sValue5,
           string Key,
           string Value)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < child2NodeList.Count; j++)
                        {
                            XmlNode xmlchild2node = child2NodeList[j];
                            if (xmlchild2node.LocalName.Equals(sValue2))
                            {
                                XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                for (int k = 0; k < child3NodeList.Count; k++)
                                {
                                    XmlNode xmlchild3node = child3NodeList[k];
                                    if (xmlchild3node.LocalName.Equals(sValue3))
                                    {

                                        XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                        for (int kk = 0; kk < child4NodeList.Count; kk++)
                                        {
                                            XmlNode xml4childNode = child4NodeList[kk];
                                            if (xml4childNode.LocalName.Equals(sValue4))
                                            {
                                                XmlNodeList child5NodeList = xml4childNode.ChildNodes;
                                                for (int k5 = 0; k5 < child5NodeList.Count; k5++)
                                                {
                                                    XmlNode xml5childNode = child5NodeList[k5];
                                                    if (xml5childNode.LocalName.Equals(sValue5))
                                                    {
                                                        if (Value == "")
                                                        {
                                                            xml5childNode.SelectSingleNode(Key).InnerText = "无";
                                                        }
                                                        else
                                                        {
                                                            xml5childNode.SelectSingleNode(Key).InnerText = Value;
                                                        }
                                                    }
                                                }
                                                
                                            }
                                        }



                                    }
                                }

                            }
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }

        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
           string Section,
           string sValue1,
           string sValue2,
            string sValue3,
            string sValue4,
            string sValue5,
            string sValue6,
           string Key,
           string Value)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < child2NodeList.Count; j++)
                        {
                            XmlNode xmlchild2node = child2NodeList[j];
                            if (xmlchild2node.LocalName.Equals(sValue2))
                            {
                                XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                for (int k = 0; k < child3NodeList.Count; k++)
                                {
                                    XmlNode xmlchild3node = child3NodeList[k];
                                    if (xmlchild3node.LocalName.Equals(sValue3))
                                    {

                                        XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                        for (int kk = 0; kk < child4NodeList.Count; kk++)
                                        {
                                            XmlNode xml4childNode = child4NodeList[kk];
                                            if (xml4childNode.LocalName.Equals(sValue4))
                                            {
                                                XmlNodeList child5NodeList = xml4childNode.ChildNodes;
                                                for (int k5 = 0; k5 < child5NodeList.Count; k5++)
                                                {
                                                    XmlNode xml5childNode = child5NodeList[k5];
                                                    if (xml5childNode.LocalName.Equals(sValue5))
                                                    {
                                                        XmlNodeList child6NodeList = xml5childNode.ChildNodes;
                                                        for (int k6 = 0; k6 < child6NodeList.Count; k6++)
                                                        {
                                                            XmlNode xmlChild6Node = child6NodeList[k6];
                                                            if (xmlChild6Node.LocalName.Equals(sValue6))
                                                            {
                                                                if (Value == "")
                                                                {
                                                                    xmlChild6Node.SelectSingleNode(Key).InnerText = "无";
                                                                }
                                                                else
                                                                {
                                                                    xmlChild6Node.SelectSingleNode(Key).InnerText = Value;
                                                                }
                                                            }
                                                        }


                                                        
                                                    }
                                                }

                                            }
                                        }



                                    }
                                }

                            }
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }

        public static void XmlWriteValue(XmlDocument doc, string sXMLPath,
          string Section,
          string sValue1,
          string sValue2,
           string sValue3,
           string sValue4,
           string sValue5,
           string sValue6,
          string sValue7,
          string Key,
          string Value)
        {
            XmlNode XmlCodeNode = doc.SelectSingleNode("//" + Section);

            if (null != XmlCodeNode)
            {
                XmlNodeList childNodeList = XmlCodeNode.ChildNodes;
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    XmlNode xmlchildnode = childNodeList[i];
                    if (xmlchildnode.LocalName.Equals(sValue1))
                    {
                        XmlNodeList child2NodeList = xmlchildnode.ChildNodes;
                        for (int j = 0; j < child2NodeList.Count; j++)
                        {
                            XmlNode xmlchild2node = child2NodeList[j];
                            if (xmlchild2node.LocalName.Equals(sValue2))
                            {
                                XmlNodeList child3NodeList = xmlchild2node.ChildNodes;
                                for (int k = 0; k < child3NodeList.Count; k++)
                                {
                                    XmlNode xmlchild3node = child3NodeList[k];
                                    if (xmlchild3node.LocalName.Equals(sValue3))
                                    {

                                        XmlNodeList child4NodeList = xmlchild3node.ChildNodes;
                                        for (int kk = 0; kk < child4NodeList.Count; kk++)
                                        {
                                            XmlNode xml4childNode = child4NodeList[kk];
                                            if (xml4childNode.LocalName.Equals(sValue4))
                                            {
                                                XmlNodeList child5NodeList = xml4childNode.ChildNodes;
                                                for (int k5 = 0; k5 < child5NodeList.Count; k5++)
                                                {
                                                    XmlNode xml5childNode = child5NodeList[k5];
                                                    if (xml5childNode.LocalName.Equals(sValue5))
                                                    {
                                                        XmlNodeList child6NodeList = xml5childNode.ChildNodes;
                                                        for (int k6 = 0; k6 < child6NodeList.Count; k6++)
                                                        {
                                                            XmlNode xmlChild6Node = child6NodeList[k6];
                                                            if (xmlChild6Node.LocalName.Equals(sValue6))
                                                            {
                                                                XmlNodeList xmlChild7NodeList = xmlChild6Node.ChildNodes;
                                                                for (int k7 = 0; k7 < xmlChild7NodeList.Count; k7++)
                                                                {
                                                                    XmlNode xmlChild7Node = xmlChild7NodeList[k7];
                                                                    if (xmlChild7Node.LocalName.Equals(sValue7))
                                                                    {
                                                                        if (Value == "")
                                                                        {
                                                                            xmlChild7Node.SelectSingleNode(Key).InnerText = "无";
                                                                        }
                                                                        else
                                                                        {
                                                                            xmlChild7Node.SelectSingleNode(Key).InnerText = Value;
                                                                        }
                                                                    }
                                                                }
                                                                
                                                            }
                                                        }



                                                    }
                                                }

                                            }
                                        }



                                    }
                                }

                            }
                        }
                    }
                }

            }
            doc.Save(sXMLPath);
        }
        
        //读取web.config文件指定 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Section"></param>部分
        /// <param name="Key"></param>关键字
        /// <returns></returns>
        public static string ConfReadValue(string key)
        {
            string svalue = "";
            //try
            //{
            //    svalue = System.Configuration.ConfigurationSettings.AppSettings[key];
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("未找到参数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            return svalue;
        }
        //写web.config
        /// <summary>
        /// 修改web.config文件appSettings配置节中的Add里的value属性
        /// </summary>
        /// <remarks>
        /// 注意，调用该函数后，会使整个Web Application重启，导致当前所有的会话丢失
        /// </remarks>
        /// <param name="key">要修改的键key</param>
        /// <param name="strValue">修改后的value</param>
        /// <exception cref="">找不到相关的键</exception>
        /// <exception cref="">权限不够，无法保存到web.config文件中</exception>

        public static  void ConfWriteValue(XmlDocument doc,string sXMLPath,string key, string strValue)
        {
            string XPath = "/configuration/appSettings/add[@key='?']";
            //XmlDocument domWebConfig=new XmlDocument();

            //domWebConfig.Load( (HttpContext.Current.Server.MapPath("web.config")) );
            XmlNode addKey = doc.SelectSingleNode((XPath.Replace("?", key)));
            if (addKey == null)
            {
                throw new ArgumentException("没有找到<add key='" + key + "' value=.../>的配置节");
            }
            addKey.Attributes["value"].InnerText = strValue;
            doc.Save(sXMLPath);

        }
	
    }
}
